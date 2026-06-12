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
    public partial class QuestService : ServiceBase
    {
        public QuestService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, IServerEnumProvider serverEnumProvider, IListfileProvider listfileProvider, IExceptionHandler exceptionHandler, AppConfig appConfig)
            : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, serverEnumProvider, listfileProvider, exceptionHandler, appConfig)
        {
            FromId = appConfig.QuestSettings.FromId;
            ToId = appConfig.QuestSettings.ToId;
            VerifiedBuild = appConfig.QuestSettings.VerifiedBuild;
        }

        public async Task<List<DashboardModel>> GetDashboardModelsAsync()
        {
            try
            {
                var dtos = await GetAsync<HotfixModsEntity>(DefaultCallback, DefaultProgress, true, false, new DbParameter(nameof(HotfixData.VerifiedBuild), VerifiedBuild));
                var results = new List<DashboardModel>();
                foreach (var dto in dtos)
                {
                    results.Add(new()
                    {
                        ID = dto.RecordID,
                        Name = dto.Name,
                        AvatarUrl = null
                    });
                }
                return results.OrderByDescending(d => d.ID).ToList();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return new();
        }

        public async Task<QuestDto?> GetByIdAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(10);

            try
            {
                var questTemplate = await GetSingleAsync<QuestTemplate>(callback, progress, new DbParameter(nameof(QuestTemplate.ID), id));
                if (questTemplate == null)
                {
                    callback.Invoke(LoadingHelper.Loading, $"{nameof(QuestTemplate)} not found", 100);
                    return null;
                }

                var result = new QuestDto()
                {
                    HotfixModsEntity = await GetExistingOrNewHotfixModsEntityAsync(callback, progress, id),
                    QuestTemplate = questTemplate,
                    QuestOfferReward = await GetSingleAsync<QuestOfferReward>(callback, progress, new DbParameter(nameof(QuestOfferReward.ID), id)) ?? new(),
                    QuestRequestItems = await GetSingleAsync<QuestRequestItems>(callback, progress, new DbParameter(nameof(QuestRequestItems.ID), id)) ?? new(),
                    QuestV2 = await GetSingleAsync<QuestV2>(callback, progress, new DbParameter(nameof(QuestV2.ID), id)) ?? new(),
                    QuestTemplateAddon = await GetSingleAsync<QuestTemplateAddon>(callback, progress, new DbParameter(nameof(QuestTemplateAddon.ID), id)),
                    QuestDetails = await GetSingleAsync<QuestDetails>(callback, progress, new DbParameter(nameof(QuestDetails.ID), id)),
                    QuestObjectiveGroups = new(),
                    IsUpdate = true
                };

                var objectives = await GetAsync<QuestObjectives>(callback, progress, new DbParameter(nameof(QuestObjectives.QuestID), id));
                foreach (var objective in objectives.OrderBy(o => o.Order))
                {
                    var db2Objective = await GetSingleAsync<QuestObjective>(new DbParameter(nameof(QuestObjective.ID), objective.ID)) ?? new();
                    result.QuestObjectiveGroups.Add(new()
                    {
                        QuestObjectives = objective,
                        QuestObjective = db2Objective
                    });
                }

                var creatureStarters = await GetAsync<CreatureQueststarter>(callback, progress, new DbParameter(nameof(CreatureQueststarter.Quest), id));
                result.CreatureQueststarterGroups = creatureStarters.Select(s => new QuestDto.CreatureQueststarterGroup { CreatureQueststarter = s }).ToList();

                var gameobjectStarters = await GetAsync<GameobjectQueststarter>(callback, progress, new DbParameter(nameof(GameobjectQueststarter.Quest), id));
                result.GameobjectQueststarterGroups = gameobjectStarters.Select(s => new QuestDto.GameobjectQueststarterGroup { GameobjectQueststarter = s }).ToList();

                var gameobjectEnders = await GetAsync<GameobjectQuestender>(callback, progress, new DbParameter(nameof(GameobjectQuestender.Quest), id));
                result.GameobjectQuestenderGroups = gameobjectEnders.Select(s => new QuestDto.GameobjectQuestenderGroup { GameobjectQuestender = s }).ToList();

                if (string.IsNullOrWhiteSpace(result.HotfixModsEntity.Name))
                {
                    result.HotfixModsEntity.Name = string.IsNullOrWhiteSpace(result.QuestTemplate.LogTitle) ? "New Quest" : result.QuestTemplate.LogTitle;
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

        public async Task<bool> SaveAsync(QuestDto dto, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(12);

            try
            {
                callback.Invoke(LoadingHelper.Saving, "Deleting existing data", progress());
                if (dto.IsUpdate)
                {
                    await DeleteAsync((int)dto.QuestTemplate.ID);
                }

                callback.Invoke(LoadingHelper.Saving, "Preparing to save", progress());
                await SetIdAndVerifiedBuild(dto);

                await SaveAsync(callback, progress, dto.HotfixModsEntity);
                await SaveAsync(callback, progress, dto.QuestTemplate);
                await SaveAsync(callback, progress, dto.QuestOfferReward);
                await SaveAsync(callback, progress, dto.QuestRequestItems);
                await SaveAsync(callback, progress, dto.QuestV2);
                await SaveAsync(callback, progress, dto.QuestTemplateAddon);
                await SaveAsync(callback, progress, dto.QuestDetails);

                await SaveAsync(callback, progress, dto.QuestObjectiveGroups.Select(g => g.QuestObjectives).ToList());
                await SaveAsync(callback, progress, dto.QuestObjectiveGroups.Select(g => g.QuestObjective).ToList());

                await SaveAsync(callback, progress, dto.CreatureQueststarterGroups.Select(g => g.CreatureQueststarter).ToList());
                await SaveAsync(callback, progress, dto.GameobjectQueststarterGroups.Select(g => g.GameobjectQueststarter).ToList());
                await SaveAsync(callback, progress, dto.GameobjectQuestenderGroups.Select(g => g.GameobjectQuestender).ToList());

                callback.Invoke(LoadingHelper.Saving, "Saving successful", 100);
                dto.IsUpdate = true;
                return true;
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(10);

            try
            {
                var dto = await GetByIdAsync(id);
                if (null == dto)
                {
                    callback.Invoke(LoadingHelper.Deleting, "Nothing to delete", 100);
                    return false;
                }

                var ownsQuest = HasConfiguredVerifiedBuild(dto.QuestTemplate);

                foreach (var group in dto.CreatureQueststarterGroups)
                {
                    await DeleteAsync(
                        callback, progress,
                        _appConfig.WorldSchema,
                        nameof(CreatureQueststarter).ToTableName(),
                        new DbParameter(nameof(CreatureQueststarter.Id), group.CreatureQueststarter.Id),
                        new DbParameter(nameof(CreatureQueststarter.Quest), group.CreatureQueststarter.Quest));
                }

                foreach (var group in dto.GameobjectQueststarterGroups)
                {
                    await DeleteAsync(
                        callback, progress,
                        _appConfig.WorldSchema,
                        nameof(GameobjectQueststarter).ToTableName(),
                        new DbParameter(nameof(GameobjectQueststarter.Id), group.GameobjectQueststarter.Id),
                        new DbParameter(nameof(GameobjectQueststarter.Quest), group.GameobjectQueststarter.Quest));
                }

                foreach (var group in dto.GameobjectQuestenderGroups)
                {
                    await DeleteAsync(
                        callback, progress,
                        _appConfig.WorldSchema,
                        nameof(GameobjectQuestender).ToTableName(),
                        new DbParameter(nameof(GameobjectQuestender.Id), group.GameobjectQuestender.Id),
                        new DbParameter(nameof(GameobjectQuestender.Quest), group.GameobjectQuestender.Quest));
                }

                foreach (var group in dto.QuestObjectiveGroups)
                {
                    await DeleteAsync(group.QuestObjectives);
                    await DeleteAsync(group.QuestObjective);
                }

                await DeleteAsync(callback, progress, dto.QuestDetails);
                await DeleteAsync(callback, progress, dto.QuestTemplateAddon);
                await DeleteAsync(callback, progress, dto.QuestV2);
                await DeleteAsync(callback, progress, dto.QuestRequestItems);
                await DeleteAsync(callback, progress, dto.QuestOfferReward);

                if (ownsQuest)
                    await DeleteAsync(callback, progress, dto.QuestTemplate);

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
    }
}
