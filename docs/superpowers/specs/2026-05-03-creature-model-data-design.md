# Creature Model Data Design

## Goal

Add a focused editor for `creature_model_data` that can be opened from a creature ID, edits only the resolved `CreatureModelData` row, and saves both `creature_model_data` and `hotfix_data` through the existing hotfix save pipeline.

## Architecture

The feature follows the existing aggregate-object pattern in the legacy Blazor app:

- `CreatureModelDataDto` contains `HotfixModsEntity` and one typed `CreatureModelData` entity.
- `CreatureModelDataService` owns search, dashboard, save, and delete behavior.
- `CreatureModelDataSearchCreatureId_Dialog` searches by creature ID, reuses the existing display-option flow when a creature has multiple `creature_template_model` rows, then opens the resolved model-data DTO.
- `CreatureModelData_Tab` is the only tab for the DTO, so the opened page shows exactly one editable entity.

## Data Flow

Search by creature ID loads `CreatureTemplateModel` rows for the creature. If there is more than one display option, the dialog lets the user pick the intended display. The selected `CreatureDisplayInfo.ID` is loaded, its `ModelID` is resolved to `CreatureModelData.ID`, and the DTO is opened.

Saving uses the existing `ServiceBase.SaveAsync<T>` path for `CreatureModelData`, which writes the typed row to `hotfixes.creature_model_data` and writes the corresponding `hotfix_data` row using the `CREATURE_MODEL_DATA` table hash.

## UI

The bottom app menu gets a `Creature Model Data` menu entry with:

- dashboard for existing saved model-data entries
- search by creature ID
- new model-data row

Settings gets `Creature Model Data` service settings matching the other aggregate objects: From ID, To ID, and Verified Build.

## Error Handling

The dialog reports missing creature templates, missing creature display info, and missing creature model data through the existing `DialogSearch` loader message pattern. Service exceptions continue through `HandleException`.

## Verification

Build the legacy solution or at minimum the MAUI Blazor project to confirm the typed model, service registration, Razor components, and menu wiring compile.
