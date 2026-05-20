using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Core.Models;
using HotfixMods.Core.Flags.TrinityCore;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Infrastructure.Services;
using HotfixMods.Tools.Dev.Business;

var dto = new GossipDto();
var creatureDto = new CreatureDto();
var factionDto = new FactionDto();
var spellDto = new SpellDto();

Assert(dto.MenuGroups.Count == 0, "GossipDto should start with no menu groups.");
Assert(creatureDto.CreatureTemplateGossip == null, "CreatureDto should expose creature_template_gossip as an optional link.");
Assert(factionDto.Faction is Faction, "FactionDto should expose the faction hotfix row.");
Assert(factionDto.FactionTemplate is FactionTemplate, "FactionDto should expose the faction_template hotfix row.");
Assert(factionDto.GetDisplayName() == "Faction", "FactionDto should use the Faction display name.");
Assert(spellDto.SpellCustomAttr == null, "SpellDto should expose spell_custom_attr as an optional world table.");

spellDto.SetDtoValueToDefault(typeof(SpellCustomAttr));
Assert(spellDto.SpellCustomAttr is SpellCustomAttr, "SpellDto should let the tab system create optional spell_custom_attr rows.");

spellDto.Spell.ID = 12345;
spellDto.SpellCustomAttr!.Entry = (uint)spellDto.Spell.ID;
spellDto.SpellCustomAttr.Attributes = (uint)(SpellCustomAttributes.SPELL_ATTR0_CU_CAN_CRIT | SpellCustomAttributes.SPELL_ATTR0_CU_DIRECT_DAMAGE);
Assert(spellDto.SpellCustomAttr.Entry == 12345, "spell_custom_attr should use Entry as the spell ID.");
Assert((uint)SpellCustomAttributes.SPELL_ATTR0_CU_CAN_CRIT == 0x00000080, "SpellCustomAttributes should match TrinityCore flag values.");
Assert(spellDto.SpellCustomAttr.Attributes == 0x00000180, "spell_custom_attr attributes should store the combined flag mask.");
Assert((uint)CreatureStaticFlags.MOUNTABLE == 0x00000001, "CreatureStaticFlags should match TrinityCore flag values.");
Assert((uint)CreatureStaticFlags.LARGE_AOI == 0x80000000, "CreatureStaticFlags should include the high-bit TrinityCore value.");
Assert((uint)CreatureStaticFlags2.NO_PET_SCALING == 0x00000001, "CreatureStaticFlags2 should match TrinityCore flag values.");
Assert((uint)CreatureStaticFlags3.AI_CAN_AUTO_LAND_IN_COMBAT == 0x80000000, "CreatureStaticFlags3 should include the high-bit TrinityCore value.");
Assert((uint)CreatureStaticFlags4.QUEST_BOSS == 0x80000000, "CreatureStaticFlags4 should match TrinityCore flag values.");
Assert((uint)CreatureStaticFlags5.GIVE_CRITERIA_KILL_CREDIT_WHEN_CHARMED == 0x80000000, "CreatureStaticFlags5 should match TrinityCore flag values.");
Assert((uint)CreatureStaticFlags6.APPLY_PROCEDURAL_WOUND_ANIM_TO_BASE == 0x80000000, "CreatureStaticFlags6 should match TrinityCore flag values.");
Assert((uint)CreatureStaticFlags7.AI_ADDITIONAL_PATHING == 0x00080000, "CreatureStaticFlags7 should preserve sparse TrinityCore flag values.");
Assert((uint)CreatureStaticFlags8.USE_FAST_CLASSIC_HEARTBEAT == 0x00000040, "CreatureStaticFlags8 should preserve sparse TrinityCore flag values.");

var config = new AppConfig();
Assert(config.FactionSettings.VerifiedBuild < 0, "Faction settings should expose a configurable negative VerifiedBuild.");

var factionTemplateRow = new DbRow("FactionTemplate");
factionTemplateRow.Columns.Add(new DbColumn { Name = "ID", Type = typeof(int), Value = 12 });
factionTemplateRow.Columns.Add(new DbColumn { Name = "Faction", Type = typeof(int), Value = 72 });
var factionName = ServiceBase.GetFactionTemplateDisplayName(factionTemplateRow, new Dictionary<int, string> { [72] = "Stormwind" });
Assert(factionName == "Stormwind", "Faction template options should display the linked faction name, not the template ID.");

dto.MenuGroups.Add(new GossipDto.MenuGroup());
dto.MenuGroups[0].GreetingTextGroups.Add(new GossipDto.GreetingTextGroup());
dto.OptionGroups.Add(new GossipDto.OptionGroup());

Assert(dto.MenuGroups[0].GossipMenu is GossipMenu, "Menu group must expose gossip_menu.");
Assert(dto.MenuGroups[0].NpcText is NpcText, "Menu group must expose npc_text.");
Assert(dto.MenuGroups[0].GreetingTextGroups[0].BroadcastText is BroadcastText, "Greeting text group must expose broadcast_text.");
Assert(dto.OptionGroups[0].GossipMenuOption is GossipMenuOption, "Option group must expose gossip_menu_option.");
Assert(dto.OptionGroups[0].BroadcastText is BroadcastText, "Option group must expose option broadcast_text.");
Assert(dto.OptionGroups[0].GossipNpcOption is GossipNpcOption, "Option group must expose gossip_npc_option.");

var script = CustomizationRequirementUnlockTool.GenerateScript(
    [
        new CustomizationRequirementUnlockRow(
            ID: 12,
            ReqType: 2,
            RaceMask: -1,
            ClassMask: 0,
            RegionGroupMask: 7,
            ReqAchievementID: 123,
            ReqQuestID: 456,
            OverrideArchive: -1,
            ReqItemModifiedAppearanceID: 789,
            ReqSource: "Curious source")
    ],
    new CustomizationRequirementUnlockOptions
    {
        HotfixStartId = 902100000,
        VerifiedBuild = -55500
    });

Assert(script.Contains("SET @VerifiedBuild = -55500;"), "Customization unlock script should declare the configured negative VerifiedBuild.");
Assert(script.Contains("REPLACE INTO hotfixes.chr_customization_req"), "Customization unlock script should write chr_customization_req rows.");
Assert(script.Contains("VALUES (12, -1, 'Curious source', 3, 0, 7, 0, 0, -1, 0, @VerifiedBuild);"), "Customization unlock script should preserve masks/source/archive, clear unlock IDs, and convert NPC req type to choice req type.");
Assert(script.Contains("INSERT INTO hotfixes.hotfix_data (Id, UniqueId, TableHash, RecordId, Status, VerifiedBuild) VALUES (902100000, 0, 1631787621, 12, 1, @VerifiedBuild);"), "Customization unlock script should write matching hotfix_data rows.");

static void Assert(bool condition, string message)
{
    if (!condition)
    {
        throw new Exception(message);
    }
}
