using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DashboardModels;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Infrastructure.Helpers;

namespace HotfixMods.Infrastructure.Services
{
    public partial class CreatureService : ServiceBase
    {
        public CreatureService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, appConfig) { }

        public CreatureDto GetNew(Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            callback.Invoke(LoadingHelper.Loading, "Returning new template", 100);
            return new();
        }

        public async Task<List<DashboardModel>> GetDashboardModelsAsync()
        {
            var dtos = await GetAsync<HotfixModsEntity>(new DbParameter(nameof(HotfixData.VerifiedBuild), VerifiedBuild));
            var results = new List<DashboardModel>();
            foreach (var dto in dtos)
            {
                results.Add(new()
                {
                    Id = dto.RecordId,
                    Name = dto.Name,
                    AvatarUrl = null
                });
            }
            return results;
        }



        public async Task<CreatureDto?> GetByIdAsync(uint id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            var progress = LoadingHelper.GetLoaderFunc(11);

            var creatureTemplate = await GetSingleAsync<CreatureTemplate>(callback, progress, new DbParameter(nameof(CreatureTemplate.Entry), id));
            if (creatureTemplate == null)
            {
                callback.Invoke(LoadingHelper.Loading, $"{nameof(CreatureTemplate)} not found", 100);
                return null;
            }

            var result = new CreatureDto()
            {
                HotfixModsEntity = await GetExistingOrNewHotfixModsEntity(callback, progress, id),
                CreatureTemplateModel = await GetSingleAsync<CreatureTemplateModel>(callback, progress, new DbParameter(nameof(CreatureTemplateModel.CreatureId), id)) ?? new(),
                CreatureTemplateAddon = await GetSingleAsync<CreatureTemplateAddon>(callback, progress, new DbParameter(nameof(CreatureTemplateAddon.Entry), id)),
                CreatureEquipTemplate = await GetSingleAsync<CreatureEquipTemplate>(callback, progress, new DbParameter(nameof(CreatureEquipTemplate.CreatureId), id)),
                CreatureTemplate = creatureTemplate,
                IsUpdate = true
            };

            result.CreatureModelInfo = await GetSingleAsync<CreatureModelInfo>(callback, progress, new DbParameter(nameof(CreatureModelInfo.DisplayId), (int)result.CreatureTemplateModel.CreatureDisplayId)) ?? new();
            result.CreatureDisplayInfo = await GetSingleAsync<CreatureDisplayInfo>(callback, progress, new DbParameter(nameof(CreatureDisplayInfo.Id), (int)result.CreatureTemplateModel.CreatureDisplayId)) ?? new();
            result.CreatureDisplayInfoExtra = await GetSingleAsync<CreatureDisplayInfoExtra>(callback, progress, new DbParameter(nameof(CreatureDisplayInfoExtra.Id), result.CreatureDisplayInfo.ExtendedDisplayInfoId));

            if (result.CreatureDisplayInfoExtra != null)
            {
                result.CreatureDisplayInfoOption = await GetAsync<CreatureDisplayInfoOption>(callback, progress, new DbParameter(nameof(CreatureDisplayInfoOption.CreatureDisplayInfoExtraId), result.CreatureDisplayInfoExtra.Id));
            }
            if (result.CreatureDisplayInfoExtra != null)
            {
                result.NpcModelItemSlotDisplayInfo = await GetAsync<NpcModelItemSlotDisplayInfo>(callback, progress, new DbParameter(nameof(NpcModelItemSlotDisplayInfo.NpcModelId), result.CreatureDisplayInfoExtra.Id));
            }

            callback.Invoke(LoadingHelper.Loading, $"Loading successful", 100);
            return result;
        }

        public async Task<bool> SaveAsync(CreatureDto dto, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            var progress = LoadingHelper.GetLoaderFunc(14);

            try
            {
                callback.Invoke(LoadingHelper.Saving, "Deleting existing data", progress());
                if (dto.IsUpdate)
                {
                    await DeleteAsync(dto.CreatureTemplate.Entry);
                }

                callback.Invoke(LoadingHelper.Saving, "Preparing to save", progress());
                await SetIdAndVerifiedBuild(dto);

                await SaveAsync(callback, progress, dto.HotfixModsEntity);
                await SaveAsync(callback, progress, dto.CreatureTemplate);
                await SaveAsync(callback, progress, dto.CreatureTemplateAddon);
                await SaveAsync(callback, progress, dto.CreatureEquipTemplate);
                await SaveAsync(callback, progress, dto.CreatureTemplateModel);
                await SaveAsync(callback, progress, dto.CreatureDisplayInfo);
                await SaveAsync(callback, progress, dto.CreatureModelInfo);

                if(dto.CreatureDisplayInfoExtra!= null)
                {
                    await SaveAsync(callback, progress, dto.CreatureDisplayInfoExtra);
                    await SaveAsync(callback, progress, dto.NpcModelItemSlotDisplayInfo?.ToList() ?? new());
                    await SaveAsync(callback, progress, dto.CreatureDisplayInfoOption?.ToList() ?? new());
                }
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                return false;
            }

            callback.Invoke("Saving", "Saving successful", 100);
            dto.IsUpdate = true;
            return true;
        }

        public async Task<bool> DeleteAsync(uint id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            var progress = LoadingHelper.GetLoaderFunc(10);

            var dto = await GetByIdAsync(id);
            if (null == dto)
            {
                callback.Invoke(LoadingHelper.Deleting, "Nothing to delete", 100);
                return false;
            }

            await DeleteAsync(callback, progress, dto.CreatureDisplayInfoExtra);
            await DeleteAsync(callback, progress, dto.CreatureDisplayInfo);
            await DeleteAsync(callback, progress, dto.CreatureModelInfo);
            await DeleteAsync(callback, progress, dto.CreatureTemplateModel);
            await DeleteAsync(callback, progress, dto.CreatureEquipTemplate);
            await DeleteAsync(callback, progress, dto.CreatureTemplateAddon);
            await DeleteAsync(callback, progress, dto.NpcModelItemSlotDisplayInfo ?? new());
            await DeleteAsync(callback, progress, dto.CreatureDisplayInfoOption ?? new());
            await DeleteAsync(callback, progress, dto.CreatureTemplate);
            await DeleteAsync(callback, progress, dto.HotfixModsEntity);

            return true;
        }
    }
}
