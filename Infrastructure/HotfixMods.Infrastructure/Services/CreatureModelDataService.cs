using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.AggregateModels;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Handlers;
using HotfixMods.Infrastructure.Helpers;

namespace HotfixMods.Infrastructure.Services
{
    public class CreatureModelDataService : ServiceBase
    {
        public CreatureModelDataService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, IServerEnumProvider serverEnumProvider, IListfileProvider listfileProvider, IExceptionHandler exceptionHandler, AppConfig appConfig)
            : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, serverEnumProvider, listfileProvider, exceptionHandler, appConfig)
        {
            FromId = appConfig.CreatureModelDataSettings.FromId;
            ToId = appConfig.CreatureModelDataSettings.ToId;
            VerifiedBuild = appConfig.CreatureModelDataSettings.VerifiedBuild;
        }

        public async Task<List<DashboardModel>> GetDashboardModelsAsync()
        {
            try
            {
                var entities = await GetAsync<HotfixModsEntity>(DefaultCallback, DefaultProgress, true, false, new DbParameter(nameof(HotfixData.VerifiedBuild), VerifiedBuild));
                var entityNamesByRecordId = entities
                    .GroupBy(e => e.RecordID)
                    .ToDictionary(g => g.Key, g => g.First().Name);
                var modelDataRows = await GetAsync<CreatureModelData>(DefaultCallback, DefaultProgress, false, true, new DbParameter(nameof(CreatureModelData.VerifiedBuild), VerifiedBuild));
                var results = new List<DashboardModel>();

                foreach (var modelData in modelDataRows)
                {
                    entityNamesByRecordId.TryGetValue(modelData.ID, out var name);
                    results.Add(new()
                    {
                        ID = modelData.ID,
                        AdditionalID = modelData.FileDataID,
                        Name = string.IsNullOrWhiteSpace(name) ? $"Creature Model Data {modelData.ID}" : name,
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

        public async Task<Dictionary<uint, string>> GetAvailableDisplayOptionsAsync(uint creatureId)
        {
            try
            {
                var result = new Dictionary<uint, string>();
                var creatureTemplateModels = await GetAsync<CreatureTemplateModel>(DefaultCallback, DefaultProgress, true, false, new DbParameter(nameof(CreatureTemplateModel.CreatureID), creatureId));

                foreach (var templateModel in creatureTemplateModels.OrderBy(m => m.Idx))
                {
                    var label = $"Display {templateModel.CreatureDisplayID} (Idx {templateModel.Idx})";
                    var creatureDisplayInfo = await GetSingleAsync<CreatureDisplayInfo>(new DbParameter(nameof(CreatureDisplayInfo.ID), templateModel.CreatureDisplayID));

                    if (creatureDisplayInfo != null)
                    {
                        label = $"Display {templateModel.CreatureDisplayID} / Model {creatureDisplayInfo.ModelID} (Idx {templateModel.Idx})";
                    }

                    result[templateModel.Idx] = label;
                }

                return result;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return new();
        }

        public async Task<CreatureModelDataDto?> GetByCreatureIdAsync(int creatureId, int idx = 0, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(5);

            try
            {
                var creatureTemplateModel = await GetSingleAsync<CreatureTemplateModel>(callback, progress, true, new DbParameter(nameof(CreatureTemplateModel.CreatureID), creatureId), new DbParameter(nameof(CreatureTemplateModel.Idx), idx));
                if (creatureTemplateModel == null)
                {
                    callback.Invoke(LoadingHelper.Loading, $"{nameof(CreatureTemplateModel)} not found", 100);
                    return null;
                }

                var creatureDisplayInfo = await GetSingleAsync<CreatureDisplayInfo>(callback, progress, new DbParameter(nameof(CreatureDisplayInfo.ID), creatureTemplateModel.CreatureDisplayID));
                if (creatureDisplayInfo == null)
                {
                    callback.Invoke(LoadingHelper.Loading, $"{nameof(CreatureDisplayInfo)} not found", 100);
                    return null;
                }

                var result = await GetByIdAsync(creatureDisplayInfo.ModelID, callback, progress);
                callback.Invoke(LoadingHelper.Loading, result == null ? $"{nameof(CreatureModelData)} not found" : "Loading successful", 100);
                return result;
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
            }

            return null;
        }

        public async Task<CreatureModelDataDto?> GetByIdAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(3);
            return await GetByIdAsync(id, callback, progress);
        }

        async Task<CreatureModelDataDto?> GetByIdAsync(int id, Action<string, string, int> callback, Func<int> progress)
        {
            var creatureModelData = await GetSingleAsync<CreatureModelData>(callback, progress, new DbParameter(nameof(CreatureModelData.ID), id));
            if (creatureModelData == null)
            {
                callback.Invoke(LoadingHelper.Loading, $"{nameof(CreatureModelData)} not found", 100);
                return null;
            }

            var result = new CreatureModelDataDto()
            {
                CreatureModelData = creatureModelData,
                HotfixModsEntity = await GetExistingOrNewHotfixModsEntityAsync(callback, progress, creatureModelData.ID),
                IsUpdate = true
            };

            if (string.IsNullOrWhiteSpace(result.HotfixModsEntity.Name))
            {
                result.HotfixModsEntity.Name = await GetDefaultNameAsync(creatureModelData);
            }

            return result;
        }

        public async Task<bool> SaveAsync(CreatureModelDataDto dto, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(5);

            try
            {
                callback.Invoke(LoadingHelper.Saving, "Preparing to save", progress());
                await SetIdAndVerifiedBuildAsync(dto);

                callback.Invoke(LoadingHelper.Saving, "Deleting existing data", progress());
                if (dto.IsUpdate)
                {
                    await DeleteAsync(dto.CreatureModelData.ID);
                }

                await SaveAsync(callback, progress, dto.HotfixModsEntity);
                await SaveAsync(callback, progress, dto.CreatureModelData);

                callback.Invoke(LoadingHelper.Saving, "Saving successful", 100);
                dto.IsUpdate = true;
                return true;
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
            }

            return false;
        }

        public async Task<bool> DeleteAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(3);

            try
            {
                var dto = await GetByIdAsync(id);
                if (dto == null)
                {
                    callback.Invoke(LoadingHelper.Deleting, "Nothing to delete", 100);
                    return false;
                }

                await DeleteAsync(callback, progress, dto.CreatureModelData);
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

        async Task SetIdAndVerifiedBuildAsync(CreatureModelDataDto dto)
        {
            var hotfixModsEntityId = await GetIdByConditionsAsync<HotfixModsEntity>(dto.HotfixModsEntity.ID, dto.IsUpdate);
            var creatureModelDataId = await GetIdByConditionsAsync<CreatureModelData>(dto.CreatureModelData.ID, dto.IsUpdate);

            dto.HotfixModsEntity.ID = hotfixModsEntityId;
            dto.HotfixModsEntity.RecordID = creatureModelDataId;
            dto.HotfixModsEntity.VerifiedBuild = VerifiedBuild;

            dto.CreatureModelData.ID = creatureModelDataId;
            dto.CreatureModelData.VerifiedBuild = VerifiedBuild;
        }

        async Task<string> GetDefaultNameAsync(CreatureModelData creatureModelData)
        {
            var options = await GetCreatureModelDataOptionsAsync<int>();
            if (options.TryGetValue(creatureModelData.ID, out var modelName) && !string.IsNullOrWhiteSpace(modelName))
            {
                return modelName;
            }

            return $"Creature Model Data {creatureModelData.ID}";
        }
    }
}
