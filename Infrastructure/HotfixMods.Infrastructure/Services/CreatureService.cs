using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DashboardModels;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Extensions;

namespace HotfixMods.Infrastructure.Services
{
    public partial class CreatureService : ServiceBase
    {
        public CreatureService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, appConfig) { }

        public CreatureDto GetNew(Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var result = new CreatureDto();

            return result;
        }

        public async Task<List<DashboardModel>> GetDashboardModelsAsync()
        {
            var dtos = await GetAsync<HotfixModsEntity>(new DbParameter(nameof(HotfixData.VerifiedBuild), VerifiedBuild));
            var results = new List<DashboardModel>();
            foreach(var dto in dtos)
            {
                results.Add(new ()
                {
                    Id = dto.RecordId,
                    Name = dto.Name,
                    AvatarUrl = null
                });
            }
            return results;
        }



        public async Task<CreatureDto?> GetByIdAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            callback("Test", "Test", 50);

            var creatureTemplate = await GetSingleAsync<CreatureTemplate>(new DbParameter(nameof(CreatureTemplate.Entry), id));
            if (creatureTemplate == null)
            {
                return null;
            }

            var result = new CreatureDto()
            {
                HotfixModsEntity = await GetExistingOrNewHotfixModsEntity(id),
                CreatureTemplate = creatureTemplate,
                CreatureTemplateModel = await GetSingleAsync<CreatureTemplateModel>(new DbParameter(nameof(CreatureTemplateModel.CreatureId), id)),
                CreatureTemplateAddon = await GetSingleAsync<CreatureTemplateAddon>(new DbParameter(nameof(CreatureTemplateAddon.Entry), id)),
                CreatureEquipTemplate = await GetSingleAsync<CreatureEquipTemplate>(new DbParameter(nameof(CreatureEquipTemplate.CreatureId), id)),
                IsUpdate = true
            };

            if (result.CreatureTemplateModel != null)
            {
                result.CreatureModelInfo = await GetSingleAsync<CreatureModelInfo>(new DbParameter(nameof(CreatureModelInfo.DisplayId), (int)result.CreatureTemplateModel.CreatureDisplayId));
                result.CreatureDisplayInfo = await GetSingleAsync<CreatureDisplayInfo>(new DbParameter(nameof(CreatureDisplayInfo.Id), (int)result.CreatureTemplateModel.CreatureDisplayId));
            }
            if (result.CreatureDisplayInfo != null)
            {
                result.CreatureDisplayInfoExtra = await GetSingleAsync<CreatureDisplayInfoExtra>(new DbParameter(nameof(CreatureDisplayInfoExtra.Id), result.CreatureDisplayInfo.ExtendedDisplayInfoId));
            }
            if (result.CreatureDisplayInfoExtra != null)
            {
                result.CreatureDisplayInfoOption = await GetAsync<CreatureDisplayInfoOption>(new DbParameter(nameof(CreatureDisplayInfoOption.CreatureDisplayInfoExtraId),result.CreatureDisplayInfoExtra.Id));
            }
            if (result.CreatureDisplayInfoExtra != null)
            {
                result.NpcModelItemSlotDisplayInfo = await GetAsync<NpcModelItemSlotDisplayInfo>(new DbParameter(nameof(NpcModelItemSlotDisplayInfo.NpcModelId), result.CreatureDisplayInfoExtra.Id));
            }

            return result;
        }

        public async Task<bool> SaveAsync(CreatureDto dto, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            int currentInvoke = 1;
            Func<int> increaseProgress = () => currentInvoke++ * 100 / 12; // divide by total invokes
            
            try
            {
                callback.Invoke("Saving", "Preparing ID", increaseProgress());
                await SetIdAndVerifiedBuild(dto);

                callback.Invoke("Saving", $"Saving {nameof(HotfixModsEntity)}", increaseProgress());
                await SaveAsync(dto.HotfixModsEntity);

                callback.Invoke("Saving", $"Saving {nameof(CreatureTemplate)}", increaseProgress());
                await SaveAsync(dto.CreatureTemplate);

                callback.Invoke("Saving", $"Saving {nameof(CreatureDisplayInfo)}", increaseProgress());
                await SaveAsync(dto.CreatureDisplayInfo);

                callback.Invoke("Saving", $"Saving {nameof(CreatureEquipTemplate)}", increaseProgress());
                await SaveAsync(dto.CreatureEquipTemplate);

                callback.Invoke("Saving", $"Saving {nameof(CreatureTemplateAddon)}", increaseProgress());
                await SaveAsync(dto.CreatureTemplateAddon);

                callback.Invoke("Saving", $"Saving {nameof(CreatureModelInfo)}", increaseProgress());
                await SaveAsync(dto.CreatureModelInfo);

                callback.Invoke("Saving", $"Saving {nameof(CreatureDisplayInfoExtra)}", increaseProgress());
                await SaveAsync(dto.CreatureDisplayInfoExtra);

                callback.Invoke("Saving", $"Saving {nameof(CreatureTemplateModel)}", increaseProgress());
                await SaveAsync(dto.CreatureTemplateModel);

                callback.Invoke("Saving", $"Saving {nameof(CreatureDisplayInfoOption)}", increaseProgress());
                await SaveAsync(dto.CreatureDisplayInfoOption.ToArray());

                callback.Invoke("Saving", $"Saving {nameof(NpcModelItemSlotDisplayInfo)}", increaseProgress());
                await SaveAsync(dto.NpcModelItemSlotDisplayInfo.ToArray());
            }
            catch(Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                return false;
            }

            callback.Invoke("Saving", "Saving successful.", 100);
            dto.IsUpdate = true;
            return true;
        }

        public async Task<bool> DeleteAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var dto = await GetByIdAsync(id);
            if(null == dto)
            {
                return false;
            }

            await dto.CreatureDisplayInfoOption.ForEachAsync(async s =>
            {
                await DeleteAsync(s);
            });

            await dto.NpcModelItemSlotDisplayInfo.ForEachAsync(async s => 
            {
                await DeleteAsync(s);
            });

            await DeleteAsync(dto.CreatureDisplayInfoExtra);
            await DeleteAsync(dto.CreatureDisplayInfo);
            await DeleteAsync(dto.CreatureModelInfo);
            await DeleteAsync(dto.CreatureTemplateModel);
            await DeleteAsync(dto.CreatureEquipTemplate);
            await DeleteAsync(dto.CreatureTemplateAddon);
            await DeleteAsync(dto.CreatureTemplate);
            await DeleteAsync(dto.HotfixModsEntity);

            return true;
        }
    }
}
