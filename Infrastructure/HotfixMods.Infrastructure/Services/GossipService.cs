using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.AggregateModels;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Infrastructure.Handlers;
using HotfixMods.Infrastructure.Helpers;

namespace HotfixMods.Infrastructure.Services
{
    public class GossipService : ServiceBase
    {
        public GossipService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, IServerEnumProvider serverEnumProvider, IListfileProvider listfileProvider, IExceptionHandler exceptionHandler, AppConfig appConfig)
            : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, serverEnumProvider, listfileProvider, exceptionHandler, appConfig)
        {
            FromId = appConfig.GossipSettings.FromId;
            ToId = appConfig.GossipSettings.ToId;
            VerifiedBuild = appConfig.GossipSettings.VerifiedBuild;
        }

        public async Task<List<DashboardModel>> GetDashboardModelsAsync()
        {
            try
            {
                var entities = await GetAsync<HotfixModsEntity>(DefaultCallback, DefaultProgress, true, false, new DbParameter(nameof(HotfixData.VerifiedBuild), VerifiedBuild));
                return entities
                    .Select(e => new DashboardModel()
                    {
                        ID = e.RecordID,
                        Name = e.Name,
                        AvatarUrl = null
                    })
                    .OrderByDescending(e => e.ID)
                    .ToList();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return new();
        }

        public async Task<GossipDto?> GetByIdAsync(int menuId, Action<string, string, int>? callback = null)
        {
            callback ??= DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(10);

            try
            {
                var menus = await GetAsync<GossipMenu>(callback, progress, true, false, new DbParameter(nameof(GossipMenu.MenuID), menuId));
                if (!menus.Any())
                {
                    callback.Invoke(LoadingHelper.Loading, $"{nameof(GossipMenu)} not found", 100);
                    return null;
                }

                var result = new GossipDto()
                {
                    HotfixModsEntity = await GetExistingOrNewHotfixModsEntityAsync(callback, progress, menuId),
                    IsUpdate = true
                };

                foreach (var menu in menus.OrderBy(m => m.TextID))
                {
                    var npcText = await GetSingleAsync<NpcText>(callback, progress, true, new DbParameter(nameof(NpcText.ID), menu.TextID))
                        ?? new NpcText() { ID = menu.TextID, VerifiedBuild = VerifiedBuild };

                    var menuGroup = new GossipDto.MenuGroup()
                    {
                        GossipMenu = menu,
                        NpcText = npcText
                    };

                    foreach (var broadcastTextId in GetBroadcastTextIds(npcText))
                    {
                        if (broadcastTextId <= 0)
                        {
                            continue;
                        }

                        menuGroup.GreetingTextGroups.Add(new()
                        {
                            BroadcastText = await GetSingleAsync<BroadcastText>(callback, progress, new DbParameter(nameof(BroadcastText.ID), broadcastTextId))
                                ?? new BroadcastText() { ID = broadcastTextId, VerifiedBuild = VerifiedBuild }
                        });
                    }

                    result.MenuGroups.Add(menuGroup);
                }

                var options = await GetAsync<GossipMenuOption>(callback, progress, true, false, new DbParameter(nameof(GossipMenuOption.MenuID), menuId));
                foreach (var option in options.OrderBy(o => o.OptionID))
                {
                    var optionGroup = new GossipDto.OptionGroup()
                    {
                        GossipMenuOption = option
                    };

                    if (option.OptionBroadcastTextID > 0)
                    {
                        optionGroup.BroadcastText = await GetSingleAsync<BroadcastText>(callback, progress, new DbParameter(nameof(BroadcastText.ID), option.OptionBroadcastTextID))
                            ?? new BroadcastText() { ID = (int)option.OptionBroadcastTextID, VerifiedBuild = VerifiedBuild };
                    }

                    if (option.GossipNpcOptionID > 0)
                    {
                        optionGroup.GossipNpcOption = await GetSingleAsync<GossipNpcOption>(callback, progress, new DbParameter(nameof(GossipNpcOption.ID), option.GossipNpcOptionID))
                            ?? new GossipNpcOption() { ID = option.GossipNpcOptionID, VerifiedBuild = VerifiedBuild };
                    }

                    result.OptionGroups.Add(optionGroup);
                }

                if (string.IsNullOrWhiteSpace(result.HotfixModsEntity.Name))
                {
                    result.HotfixModsEntity.Name = GetGossipDisplayName(result);
                }

                callback.Invoke(LoadingHelper.Loading, "Loading successful", 100);
                return result;
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
            }

            return null;
        }

        public async Task<bool> SaveAsync(GossipDto dto, Action<string, string, int>? callback = null)
        {
            callback ??= DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(10);

            try
            {
                callback.Invoke(LoadingHelper.Saving, "Deleting existing data", progress());
                if (dto.IsUpdate && dto.HotfixModsEntity.RecordID > 0)
                {
                    await DeleteAsync(dto.HotfixModsEntity.RecordID);
                }

                callback.Invoke(LoadingHelper.Saving, "Preparing to save", progress());
                await SetIdAndVerifiedBuild(dto);

                await SaveAsync(callback, progress, dto.HotfixModsEntity);

                foreach (var menuGroup in dto.MenuGroups)
                {
                    await SaveAsync(callback, progress, menuGroup.GossipMenu);
                    await SaveAsync(callback, progress, menuGroup.NpcText);
                    await SaveAsync(callback, progress, menuGroup.GreetingTextGroups.Select(g => g.BroadcastText).Where(HasBroadcastTextContent).ToList());
                }

                foreach (var optionGroup in dto.OptionGroups)
                {
                    await SaveAsync(callback, progress, optionGroup.GossipMenuOption);

                    if (HasBroadcastTextContent(optionGroup.BroadcastText))
                    {
                        await SaveAsync(callback, progress, optionGroup.BroadcastText);
                    }

                    await SaveAsync(callback, progress, optionGroup.GossipNpcOption);
                }

                callback.Invoke(LoadingHelper.Saving, "Saving successful", 100);
                dto.IsUpdate = true;
                dto.HotfixModsEntity.Name = GetGossipDisplayName(dto);
                return true;
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
            }

            return false;
        }

        public async Task<bool> DeleteAsync(int menuId, Action<string, string, int>? callback = null)
        {
            callback ??= DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(10);

            try
            {
                var dto = await GetByIdAsync(menuId);
                if (dto == null)
                {
                    callback.Invoke(LoadingHelper.Deleting, "Nothing to delete", 100);
                    return false;
                }

                foreach (var optionGroup in dto.OptionGroups)
                {
                    if (HasGossipNpcOptionContent(optionGroup.GossipNpcOption))
                    {
                        await DeleteAsync(callback, progress, optionGroup.GossipNpcOption);
                    }

                    if (HasBroadcastTextContent(optionGroup.BroadcastText))
                    {
                        await DeleteAsync(callback, progress, optionGroup.BroadcastText);
                    }

                    await DeleteAsync(
                        callback,
                        progress,
                        _appConfig.WorldSchema,
                        nameof(GossipMenuOption).ToTableName(),
                        new DbParameter(nameof(GossipMenuOption.MenuID), optionGroup.GossipMenuOption.MenuID),
                        new DbParameter(nameof(GossipMenuOption.OptionID), optionGroup.GossipMenuOption.OptionID));
                }

                foreach (var menuGroup in dto.MenuGroups)
                {
                    foreach (var greetingGroup in menuGroup.GreetingTextGroups.Where(g => HasBroadcastTextContent(g.BroadcastText)))
                    {
                        await DeleteAsync(callback, progress, greetingGroup.BroadcastText);
                    }

                    await DeleteAsync(callback, progress, menuGroup.NpcText);
                    await DeleteAsync(
                        callback,
                        progress,
                        _appConfig.WorldSchema,
                        nameof(GossipMenu).ToTableName(),
                        new DbParameter(nameof(GossipMenu.MenuID), menuGroup.GossipMenu.MenuID),
                        new DbParameter(nameof(GossipMenu.TextID), menuGroup.GossipMenu.TextID));
                }

                await DeleteAsync(callback, progress, dto.HotfixModsEntity);
                callback.Invoke(LoadingHelper.Deleting, "Delete successful", 100);
                return true;
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
            }

            return false;
        }

        async Task SetIdAndVerifiedBuild(GossipDto dto)
        {
            var currentMenuId = dto.HotfixModsEntity.RecordID;
            var isCreateRoot = IsCreateOperation(dto.IsUpdate, currentMenuId);
            var menuId = isCreateRoot
                ? await GetNextIdInRangeAsync(_appConfig.WorldSchema, nameof(GossipMenu).ToTableName(), FromId, ToId, nameof(GossipMenu.MenuID))
                : currentMenuId;

            var nextNpcTextId = await GetNextIdInRangeAsync(_appConfig.WorldSchema, nameof(NpcText).ToTableName(), FromId, ToId, nameof(NpcText.ID));
            var nextBroadcastTextId = await GetNextIdInRangeAsync(_appConfig.HotfixesSchema, nameof(BroadcastText).ToTableName(), FromId, ToId, nameof(BroadcastText.ID));
            var nextGossipOptionId = await GetNextIdInRangeAsync(_appConfig.WorldSchema, nameof(GossipMenuOption).ToTableName(), FromId, ToId, nameof(GossipMenuOption.GossipOptionID));
            var nextGossipNpcOptionId = await GetNextIdInRangeAsync(_appConfig.HotfixesSchema, nameof(GossipNpcOption).ToTableName(), FromId, ToId, nameof(GossipNpcOption.ID));

            dto.HotfixModsEntity.ID = await GetIdByConditionsAsync<HotfixModsEntity>(dto.HotfixModsEntity.ID, dto.IsUpdate);
            dto.HotfixModsEntity.RecordID = menuId;
            dto.HotfixModsEntity.VerifiedBuild = VerifiedBuild;

            foreach (var menuGroup in dto.MenuGroups)
            {
                var isCreateNpcText = IsCreateOperation(dto.IsUpdate, menuGroup.NpcText.ID);
                var npcTextId = isCreateNpcText ? nextNpcTextId++ : (int)menuGroup.NpcText.ID;

                menuGroup.GossipMenu.MenuID = (uint)menuId;
                menuGroup.GossipMenu.TextID = (uint)npcTextId;
                menuGroup.GossipMenu.VerifiedBuild = VerifiedBuild;

                menuGroup.NpcText.ID = (uint)npcTextId;
                menuGroup.NpcText.VerifiedBuild = VerifiedBuild;

                var greetingTexts = menuGroup.GreetingTextGroups.Select(g => g.BroadcastText).Where(HasBroadcastTextContent).Take(8).ToList();
                for (var i = 0; i < greetingTexts.Count; i++)
                {
                    var broadcastText = greetingTexts[i];
                    if (IsCreateOperation(dto.IsUpdate, broadcastText.ID))
                    {
                        broadcastText.ID = nextBroadcastTextId++;
                    }

                    broadcastText.VerifiedBuild = VerifiedBuild;
                    SetNpcTextBroadcast(menuGroup.NpcText, i, broadcastText.ID);
                }

                for (var i = greetingTexts.Count; i < 8; i++)
                {
                    SetNpcTextBroadcast(menuGroup.NpcText, i, 0);
                }

            }

            for (var i = 0; i < dto.OptionGroups.Count; i++)
            {
                var optionGroup = dto.OptionGroups[i];
                var option = optionGroup.GossipMenuOption;

                option.MenuID = (uint)menuId;
                option.OptionID = (uint)i;
                if (option.GossipOptionID <= 0 || !dto.IsUpdate)
                {
                    option.GossipOptionID = nextGossipOptionId++;
                }

                option.VerifiedBuild = VerifiedBuild;

                if (HasBroadcastTextContent(optionGroup.BroadcastText))
                {
                    if (IsCreateOperation(dto.IsUpdate, optionGroup.BroadcastText.ID))
                    {
                        optionGroup.BroadcastText.ID = nextBroadcastTextId++;
                    }

                    optionGroup.BroadcastText.VerifiedBuild = VerifiedBuild;
                    option.OptionBroadcastTextID = (uint)optionGroup.BroadcastText.ID;
                }

                if (IsCreateOperation(dto.IsUpdate, optionGroup.GossipNpcOption.ID))
                {
                    optionGroup.GossipNpcOption.ID = nextGossipNpcOptionId++;
                }

                optionGroup.GossipNpcOption.GossipOptionID = option.GossipOptionID;
                optionGroup.GossipNpcOption.VerifiedBuild = VerifiedBuild;
                option.GossipNpcOptionID = optionGroup.GossipNpcOption.ID;
            }

            dto.HotfixModsEntity.Name = GetGossipDisplayName(dto);
        }

        static bool HasBroadcastTextContent(BroadcastText? broadcastText)
        {
            return broadcastText != null
                && (broadcastText.ID > 0
                    || !string.IsNullOrWhiteSpace(broadcastText.Text)
                    || !string.IsNullOrWhiteSpace(broadcastText.Text1));
        }

        static bool HasGossipNpcOptionContent(GossipNpcOption? gossipNpcOption)
        {
            return gossipNpcOption != null
                && (gossipNpcOption.ID > 0
                    || gossipNpcOption.GossipNpcOptionValue != 0
                    || gossipNpcOption.LFGDungeonsID != 0
                    || gossipNpcOption.TrainerID != 0
                    || gossipNpcOption.GarrFollowerTypeID != 0
                    || gossipNpcOption.CharShipmentID != 0
                    || gossipNpcOption.GarrTalentTreeID != 0
                    || gossipNpcOption.UiMapID != 0
                    || gossipNpcOption.UiItemInteractionID != 0
                    || gossipNpcOption.CovenantID != 0
                    || gossipNpcOption.TraitTreeID != 0
                    || gossipNpcOption.ProfessionID != 0
                    || gossipNpcOption.NeighborhoodMapID != 0
                    || gossipNpcOption.SkillLineID != 0);
        }

        static IEnumerable<int> GetBroadcastTextIds(NpcText npcText)
        {
            yield return (int)npcText.BroadcastTextID0;
            yield return (int)npcText.BroadcastTextID1;
            yield return (int)npcText.BroadcastTextID2;
            yield return (int)npcText.BroadcastTextID3;
            yield return (int)npcText.BroadcastTextID4;
            yield return (int)npcText.BroadcastTextID5;
            yield return (int)npcText.BroadcastTextID6;
            yield return (int)npcText.BroadcastTextID7;
        }

        static void SetNpcTextBroadcast(NpcText npcText, int index, int broadcastTextId)
        {
            var probability = broadcastTextId > 0 ? 1 : 0;
            switch (index)
            {
                case 0:
                    npcText.Probability0 = probability;
                    npcText.BroadcastTextID0 = (uint)broadcastTextId;
                    break;
                case 1:
                    npcText.Probability1 = probability;
                    npcText.BroadcastTextID1 = (uint)broadcastTextId;
                    break;
                case 2:
                    npcText.Probability2 = probability;
                    npcText.BroadcastTextID2 = (uint)broadcastTextId;
                    break;
                case 3:
                    npcText.Probability3 = probability;
                    npcText.BroadcastTextID3 = (uint)broadcastTextId;
                    break;
                case 4:
                    npcText.Probability4 = probability;
                    npcText.BroadcastTextID4 = (uint)broadcastTextId;
                    break;
                case 5:
                    npcText.Probability5 = probability;
                    npcText.BroadcastTextID5 = (uint)broadcastTextId;
                    break;
                case 6:
                    npcText.Probability6 = probability;
                    npcText.BroadcastTextID6 = (uint)broadcastTextId;
                    break;
                case 7:
                    npcText.Probability7 = probability;
                    npcText.BroadcastTextID7 = (uint)broadcastTextId;
                    break;
            }
        }

        static string GetGossipDisplayName(GossipDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.HotfixModsEntity.Name))
            {
                return dto.HotfixModsEntity.Name;
            }

            var optionText = dto.OptionGroups
                .Select(g => g.GossipMenuOption.OptionText)
                .FirstOrDefault(t => !string.IsNullOrWhiteSpace(t));

            if (!string.IsNullOrWhiteSpace(optionText))
            {
                return optionText;
            }

            var greeting = dto.MenuGroups
                .SelectMany(g => g.GreetingTextGroups)
                .Select(g => !string.IsNullOrWhiteSpace(g.BroadcastText.Text1) ? g.BroadcastText.Text1 : g.BroadcastText.Text)
                .FirstOrDefault(t => !string.IsNullOrWhiteSpace(t));

            if (!string.IsNullOrWhiteSpace(greeting))
            {
                return greeting;
            }

            return dto.HotfixModsEntity.RecordID > 0 ? $"Gossip {dto.HotfixModsEntity.RecordID}" : "New Gossip";
        }
    }
}
