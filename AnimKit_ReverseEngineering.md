# AnimKit Reverse Engineering Notes

This file is a working knowledge base for investigating and authoring `AnimKit` data in this solution.

Future Codex contexts should read this file before making claims about `AnimKitSegment`, `AnimKitConfig`, bone sets, or playback behavior.

## Scope

This note is based on:

- local source code in this repo
- DB2 and hotfix mirror data accessed through the MySQL MCP
- repeated in-game validation from the user by playing test AnimKits in one-shot and loop mode

Some conclusions are strong, some are still educated guesses. When uncertain, say so plainly and prefer verifying with a small test AnimKit.

## Important Local Files

Core model / enum files:

- `Core/HotfixMods.Core/Models/Db2/AnimKitSegment.cs`
- `Core/HotfixMods.Core/Enums/Db2/AnimKitSegmentStartCondition.cs`
- `Core/HotfixMods.Core/Enums/Db2/AnimKitSegmentEndCondition.cs`
- `Core/HotfixMods.Core/Flags/Db2/AnimKitSegmentSegmentFlags.cs`
- `Core/HotfixMods.Core/Models/Db2/AnimKitConfigBoneSet.cs`
- `Core/HotfixMods.Core/Flags/Db2/AnimKitConfigConfigFlags.cs`
- `Core/HotfixMods.Core/Enums/Db2/AnimationDataId.cs`

Blazor/UI files:

- `Infrastructure/HotfixMods.Infrastructure.Blazor/Pages/AnimKitTabs/AnimKitSegment_Tab.razor`
- `Infrastructure/HotfixMods.Infrastructure/Services/AnimKitService.Options.cs`

Useful local facts:

- Animation names are available in `AnimationDataId.cs`. Use that enum for friendly names like `KNOCKDOWN = 121`, `EMOTE_DANCE_SPECIAL = 211`, etc.
- Bone set option labels are loaded from DB2 table `AnimKitBoneSet` via `GetDb2OptionsAsync<uint>("AnimKitBoneSet", "Name")`.

## Important MySQL MCP Tables

Use the MySQL MCP, not guesses, when checking saved data or looking for patterns.

Primary DB2 tables:

- `db2.anim_kit`
- `db2.anim_kit_segment`
- `db2.anim_kit_config`
- `db2.anim_kit_config_bone_set`
- `db2.anim_kit_bone_set`
- `db2.anim_kit_priority`

Primary hotfix tables:

- `hotfixes.anim_kit`
- `hotfixes.anim_kit_segment`
- `hotfixes.anim_kit_config`
- `hotfixes.anim_kit_config_bone_set`

Recommended query pattern:

1. Read the `hotfixes` row you are editing.
2. Read all `hotfixes.anim_kit_segment` rows for the AnimKit ordered by `OrderIndex`.
3. Read the linked `hotfixes.anim_kit_config` and `hotfixes.anim_kit_config_bone_set` rows.
4. Compare against `db2` examples with similar `AnimID`, `AnimStartTime`, `Speed`, and conditions.

Example query skeleton:

```sql
SELECT * FROM hotfixes.anim_kit WHERE ID = 60004;

SELECT *
FROM hotfixes.anim_kit_segment
WHERE ParentAnimKitID = 60004
ORDER BY OrderIndex;

SELECT *
FROM hotfixes.anim_kit_config
WHERE ID IN (
    SELECT AnimKitConfigID
    FROM hotfixes.anim_kit_segment
    WHERE ParentAnimKitID = 60004
);

SELECT *
FROM hotfixes.anim_kit_config_bone_set
WHERE ParentAnimKitConfigID IN (
    SELECT AnimKitConfigID
    FROM hotfixes.anim_kit_segment
    WHERE ParentAnimKitID = 60004
)
ORDER BY ParentAnimKitConfigID, ID;
```

## Ground Rules For Testing

- Prefer very small AnimKits when isolating behavior: 1 to 3 segments is ideal.
- Ask the user to test in both one-shot and loop mode.
- Remember that some models do not support some animations. A valid AnimKit can still appear to do nothing if the model lacks the requested `AnimID`.
- When a test seems dead, inspect the saved `hotfixes` rows first before theorizing.
- Always compare exact saved values, especially `EndConditionParam` vs `EndConditionDelay`.

## AnimID Names

The repo enum `AnimationDataId.cs` is the reliable local source for animation names.

Examples:

- `121 = KNOCKDOWN`
- `211 = EMOTE_DANCE_SPECIAL`

Do not rely on the mirrored `db2.animation_data` table for names in this repo context. The user explicitly confirmed the local enum names are correct and easier to work with.

## Strong Findings: AnimKitSegment

### AnimStartTime

Best current interpretation:

- `AnimStartTime` is an offset into the source animation.
- The UI currently labels this as frames.
- In practice, values like `400`, `900`, `1200`, `5700`, `6330` have matched user-observed playback windows well.

Practical effect:

- `AnimStartTime = 0` starts from the beginning of the animation.
- Non-zero values let a segment start from the middle or end portion of an animation.
- This is one of the main tools for building slices, holds, and forward/reverse playback chains.

### Speed

Strong current interpretation:

- `1` = normal speed
- `0.5` = half speed
- `2` = double speed
- `0` = hold/freeze on the starting frame of the segment
- negative values = play backwards

Examples:

- `Speed = 0` is useful for a pause / held pose segment.
- `Speed = -1` is the straightforward way to reverse an animation segment.

### StartCondition

These are strongly validated.

- `0 = IMMEDIATE`
  - start from the AnimKit root timeline
  - `StartConditionDelay` adds a delay in milliseconds

- `1 = AFTER_SEGMENT_START`
  - start when another segment starts
  - `StartConditionParam` points to the referenced segment's `OrderIndex`
  - `StartConditionDelay` adds a delay in milliseconds
  - use this for overlapping / staggered chains

- `2 = AFTER_SEGMENT_END`
  - start when another segment ends
  - `StartConditionParam` points to the referenced segment's `OrderIndex`
  - `StartConditionDelay` adds a delay in milliseconds
  - use this for sequential chains

This is one of the most reliable areas of the reverse engineering.

### EndCondition

These are partially solved. Be careful not to overclaim.

- `0 = NONE_OR_PERSIST`
  - no clearly identified automatic stop behavior
  - common on persistent / held segments and chain glue

- `1 = PLAY_UNTIL_ANIMATION_END`
  - likely natural playback completion
  - used on straightforward play-once segments

- `2 = TIMED_STOP_VARIANT`
  - behaves like a timed stop using `EndConditionDelay`
  - still not cleanly separated from condition `3`

- `3 = STOP_AFTER_MS`
  - strongest confidence
  - stop after `EndConditionDelay` milliseconds

- `4 = TIMED_SLICE_VARIANT`
  - often behaves like a bounded slice or tail-window mode
  - often seen in looping and end-slice experiments

- `5 = TIMED_SLICE_VARIANT_RARE`
  - rarer relative of condition `4`

### EndConditionParam vs EndConditionDelay

This is a common pitfall.

For the practical timed patterns we have used:

- `EndConditionDelay` is the time value that matters
- `EndConditionParam` is usually `0` for conditions `2` and `3`

Real bug caught during testing:

- An AnimKit was saved with `EndCondition = 3`, `EndConditionParam = 630`, `EndConditionDelay = 0`
- This did not behave as intended
- Moving the duration to `EndConditionDelay = 630` fixed it

When a segment does not stop at the expected time, inspect these two fields first.

### SegmentFlags

Only a few bits are decoded with useful confidence.

- `2 = USE_FORCED_VARIATION`
  - this is not random selection
  - with this flag enabled, `ForcedVariation` appears to select a concrete variation index:
    - `0` = first/default variation
    - `1` = second variation
    - likely higher values map to higher variation indices
  - without this flag, `ForcedVariation` does not appear to matter

- `256 = ENABLE_BLEND_IN`
  - makes `BlendInTimeMs` relevant

- `512 = ENABLE_BLEND_OUT`
  - makes `BlendOutTimeMs` relevant

Unknown / caution:

- Other bits remain unresolved.
- Reverse playback should still be understood as primarily controlled by negative `Speed`, not by a special flag.

### ForcedVariation

Current practical rule:

- If `SegmentFlags` includes bit `2`, then `ForcedVariation` selects the variation index.
- If bit `2` is not set, this field usually has no visible effect.

Important correction:

- Do not assume `ForcedVariation = 0` means random.
- User validation indicated that `0` with the flag enabled behaves like the first/default variation.

### LoopToSegmentIndex

Best current interpretation:

- Non-negative values point to the `OrderIndex` to jump back to when the AnimKit is played in loop mode.
- `-1` commonly means no explicit loop target on that segment.

Practical use:

- Use it to build back-and-forth or held loops.
- For one-shot behavior, `-1` is usually the clean default.

## Bone Set IDs And Names

Source of truth:

- Bone Set IDs and names come from `db2.anim_kit_bone_set`
- The display name is the `Name` column
- The hierarchy is represented by `ParentAnimKitBoneSetID`

Example rows:

- `0 = Full Body`
- `1 = Upper Body`
- `2 = Right Shoulder`
- `3 = Left Shoulder`
- `4 = Head`
- `5 = Right Arm`
- `6 = Left Arm`
- `7 = Right Hand`
- `8 = Left Hand`
- `9 = Jaw`

Hierarchy example:

- `Upper Body (1)` -> parent `Full Body (0)`
- `Right Shoulder (2)` -> parent `Upper Body (1)`
- `Right Arm (5)` -> parent `Right Shoulder (2)`
- `Right Hand (7)` -> parent `Right Arm (5)`

Relevant table columns:

- `ID`
- `Name`
- `BoneDataID`
- `ParentAnimKitBoneSetID`
- `AltAnimKitBoneSetID`
- `AltBoneDataID`

### AnimKitConfig / Bone Set Use

Segments point to `AnimKitConfigID`.
That config points to one or more rows in `AnimKitConfigBoneSet`.
Those rows choose:

- which bone set is used
- which priority is used

Useful linked table:

- `db2.anim_kit_priority`

Example confirmed in testing:

- one segment used `Upper Body`
- follow-up segment used `Full Body`
- user observed upper-body-only animation first, then full-body animation including legs

## Practical Recipes

### 1. Start An Animation From The Middle

Use a single segment:

- set `AnimID`
- set `AnimStartTime` to the desired offset
- set `Speed = 1`
- set `StartCondition = 0`
- usually set `EndCondition = 1` or `3`

Example:

- `KNOCKDOWN`
- `AnimStartTime = 1200`
- `Speed = 0.5`

### 2. Pause On A Pose

Use a segment with:

- same `AnimID`
- `AnimStartTime` equal to the frame you want to hold on
- `Speed = 0`
- `EndCondition = 3`
- `EndConditionDelay = hold duration in ms`

This is the cleanest known pause pattern.

### 3. Forward Then Reverse

Use two segments:

Segment 0:

- forward playback
- `AnimStartTime = start`
- `Speed = 1`
- `EndCondition = 3`
- `EndConditionDelay = window length`

Segment 1:

- reverse playback
- `AnimStartTime = end`
- `Speed = -1`
- `StartCondition = 2`
- `StartConditionParam = 0`
- `EndCondition = 3`
- `EndConditionDelay = same window length`

Example:

- `EMOTE_DANCE_SPECIAL`
- segment 0 from `5700` for `630 ms`
- segment 1 from `6330` at `Speed = -1` for `630 ms`

### 4. Forward, Pause, Then Reverse

Use three segments:

Segment 0:

- forward window

Segment 1:

- hold segment at the end pose
- `Speed = 0`
- `StartCondition = 2`
- `StartConditionParam = 0`

Segment 2:

- reverse window
- `StartCondition = 2`
- `StartConditionParam = 1`

Important pitfall:

- If the reverse segment incorrectly points to segment `0` instead of the pause segment, the reverse will start immediately after the forward part and the pause will appear to be ignored.

This exact mistake happened on AnimKit `60004`.

### 5. Freeze On The Final Pose

Common pattern:

- segment 0 plays a timed window
- segment 1 starts after segment 0 ends
- segment 1 uses the same `AnimID`
- segment 1 `AnimStartTime` equals the end frame
- segment 1 `Speed = 0`

### 6. Overlapping / Staggered Chains

Use `StartCondition = 1`.

Pattern:

- segment N starts some milliseconds after the previous segment starts
- this causes overlap rather than waiting for completion

### 7. Sequential Chains

Use `StartCondition = 2`.

Pattern:

- segment N waits until segment N-1 ends
- easiest pattern for deterministic step-by-step sequences

### 8. Approximating Ease In / Ease Out

There is no confirmed per-segment easing curve field in `AnimKitSegment`.

Practical workaround:

- split the same animation into several short segments
- raise `AnimStartTime` each time
- reduce `Speed` for later segments

This approximates slowing down toward the end.

Do not claim that a true easing field exists unless it is actually demonstrated.

## AnimKitConfigConfigFlags Notes

Useful known flags from `AnimKitConfigConfigFlags.cs` include:

- `32 = USE_PARENT_IF_MISSING_BONE_SET`
- `256 = PLAY_ON_ALL_CHILDREN_AND_DESCENDANT_BONE_SETS_AT_SAME_PRIORITY`
- `134217728 = USE_MOD_CAST_SPEED`
- `2147483648 = SPEED_UP_BASED_ON_PERCENTAGE_COMPLETE`

Important current finding:

- No rows were found in either `db2.anim_kit_config` or `hotfixes.anim_kit_config` using the top-bit flag `2147483648`
- Therefore it appears unused in the mirrored data we currently have
- Do not claim a behavior for it without a custom A/B test

## Useful Known Test Cases

These were especially informative during reverse engineering.

### StartCondition

- `AnimKit 782`
  - `EMOTE_KNEEL` then `KNEEL_LOOP`
  - useful for `StartCondition = 1`

- `AnimKit 1389`
  - `READY_THROWN` then `ATTACK_THROWN`
  - useful for `StartCondition = 2`
  - also useful for upper-body vs full-body config behavior

- `AnimKit 2273`
  - multiple `ATTACK_UNARMED` segments
  - clearly supports `StartCondition = 1` as overlap from prior segment start

- `AnimKit 11250`
  - multi-hit attack chain
  - also useful for variation testing

### End / slice behavior

- `AnimKit 597`
  - helpful for `EndCondition = 3`

- `AnimKit 694`
  - useful for non-zero `AnimStartTime` plus `EndCondition = 4`

- `AnimKit 1946`
  - useful for `EndCondition = 2` plus follow-up segment from later `AnimStartTime`

- `AnimKit 6362`
  - especially useful for reverse and tiny tail-window looping behavior

### KNOCKDOWN

Good existing DB2 examples for `AnimID = 121`:

- `AnimKit 1985` starts at `1250`
- `AnimKit 2054` starts at `800`

### Custom hotfix debugging example

- `AnimKit 60004`
  - used to validate forward -> pause -> reverse sequencing
  - useful reminder that wrong `StartConditionParam` can skip the pause

## Workflow For Future Codex Contexts

When the user asks to investigate or build an AnimKit:

1. Read this file.
2. Read the relevant source files listed above.
3. Query the current `hotfixes` rows first if a custom AnimKit already exists.
4. Query similar `db2.anim_kit_segment` rows by `AnimID` and by patterns such as negative `Speed`, non-zero `AnimStartTime`, or a chosen `EndCondition`.
5. Use `AnimationDataId.cs` for animation names.
6. Use `db2.anim_kit_bone_set.Name` for bone set names.
7. If behavior is still ambiguous, propose a very small test AnimKit and ask the user to validate in one-shot and loop mode.
8. When reporting conclusions, separate:
   - strongly verified behavior
   - inferred but not fully proven behavior

## Things To Be Careful About

- Do not assume a dead AnimKit is invalid. The target model may simply not support the requested animation.
- Do not confuse `EndConditionParam` with `EndConditionDelay`.
- Do not assume `ForcedVariation = 0` means random.
- Do not overstate the meaning of unresolved end-condition variants.
- Do not assume a reverse-play flag exists when negative `Speed` already explains the effect.
- Do not assume top-bit config flags are in live use unless confirmed in the data.

## Open Questions

These remain worth investigating:

- the exact difference between `EndCondition = 2` and `EndCondition = 3`
- the exact semantics of `EndCondition = 4` and `5`
- whether any unresolved `SegmentFlags` bits have visible gameplay meaning
- whether any config-level flag can truly provide dynamic speed scaling rather than requiring multi-segment approximation

## Suggested Prompt For Future Contexts

If a future context needs help quickly, the user can say something like:

> Read `AnimKit_ReverseEngineering.md` and use the MySQL MCP with `db2` and `hotfixes` AnimKit tables before answering.

