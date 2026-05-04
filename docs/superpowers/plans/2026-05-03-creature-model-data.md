# Creature Model Data Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add a focused `CreatureModelData` editor opened by creature ID and saved through the existing hotfix pipeline.

**Architecture:** Follow the legacy Blazor aggregate pattern with a typed DB2 model, DTO, service, tab page, search dialog, dashboard, menu wiring, settings, and DI registration.

**Tech Stack:** C#/.NET, MAUI Blazor, MudBlazor, existing MySQL/DB2 provider abstractions.

---

### Task 1: Core Model And DTO

**Files:**
- Create: `Core/HotfixMods.Core/Models/Db2/CreatureModelData.cs`
- Create: `Infrastructure/HotfixMods.Infrastructure/DtoModels/CreatureModelDataDto.cs`

- [ ] Add a typed `[HotfixesSchema]` `CreatureModelData` class matching the live `hotfixes.creature_model_data` columns.
- [ ] Add `CreatureModelDataDto : DtoBase` with one `CreatureModelData` property.
- [ ] Build `HotfixMods.Infrastructure` and fix type/namespace errors.

### Task 2: Service

**Files:**
- Create: `Infrastructure/HotfixMods.Infrastructure/Services/CreatureModelDataService.cs`
- Modify: `Infrastructure/HotfixMods.Infrastructure/Config/AppConfig.cs`

- [ ] Add `CreatureModelDataSettings` to `AppConfig`.
- [ ] Add service methods for dashboard listing, available display options by creature ID, load by creature ID/display option, load by model-data ID, save, and delete.
- [ ] Use the existing `SaveAsync<T>` and `DeleteAsync<T>` hotfix behavior.
- [ ] Build `HotfixMods.Infrastructure`.

### Task 3: Blazor UI

**Files:**
- Create: `Infrastructure/HotfixMods.Infrastructure.Blazor/Pages/CreatureModelData.razor`
- Create: `Infrastructure/HotfixMods.Infrastructure.Blazor/Pages/CreatureModelDataTabs/CreatureModelData_Tab.razor`
- Create: `Infrastructure/HotfixMods.Infrastructure.Blazor/Components/Dialogs/CreatureModelDataSearchCreatureId_Dialog.razor`
- Create: `Infrastructure/HotfixMods.Infrastructure.Blazor/Pages/Dashboards/CreatureModelDataDashboard.razor`

- [ ] Add a page that renders `DtoTabsContent<CreatureModelDataDto>` with only the model-data tab.
- [ ] Add the search dialog with creature ID entry and display-option selection.
- [ ] Add the dashboard listing existing saved rows.
- [ ] Build `HotfixMods.Infrastructure.Blazor`.

### Task 4: App Wiring

**Files:**
- Modify: `Apps/HotfixMods.Apps.MauiBlazor/MauiProgram.cs`
- Modify: `Infrastructure/HotfixMods.Infrastructure.Blazor/Pages/Index.razor`
- Modify: `Infrastructure/HotfixMods.Infrastructure.Blazor/Pages/Settings.razor`
- Modify: `Infrastructure/HotfixMods.Infrastructure.Blazor/Handlers/IconHandler.cs`

- [ ] Register `CreatureModelDataService`.
- [ ] Add bottom-menu items for dashboard, search, and new row.
- [ ] Add settings fields for From ID, To ID, Verified Build.
- [ ] Add an icon mapping for the new page.
- [ ] Build the MAUI Blazor project.
