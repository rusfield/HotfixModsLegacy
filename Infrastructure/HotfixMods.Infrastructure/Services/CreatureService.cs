using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DashboardModels;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class CreatureService : ServiceBase
    {
        public CreatureService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, appConfig) { }

        public async Task<CreatureDto> GetNewAsync(Action<string, string, int>? callback = null)
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

            var creatureTemplate = await GetSingleAsync<CreatureTemplate>(new DbParameter(nameof(CreatureTemplate.Entry), id));
            if (creatureTemplate == null)
            {
                return null;
            }

            var result = new CreatureDto()
            {
                Entity = await GetExistingOrNewHotfixModsEntity(id),
                CreatureTemplate = creatureTemplate,
                CreatureTemplateModel = await GetSingleAsync<CreatureTemplateModel>(new DbParameter(nameof(CreatureTemplateModel.CreatureId), id)) ?? new(),
                CreatureTemplateAddon = await GetSingleByIdAsync<CreatureTemplateAddon>(id) ?? new(),
                CreatureEquipTemplate = await GetSingleAsync<CreatureEquipTemplate>(new DbParameter(nameof(CreatureEquipTemplate.CreatureId), id)) ?? new(),
                IsUpdate = true
            };

            if (result.CreatureTemplateModel != null)
            {
                result.CreatureModelInfo = await GetSingleByIdAsync<CreatureModelInfo>((int)result.CreatureTemplateModel.CreatureDisplayId);
                result.CreatureDisplayInfo = await GetSingleByIdAsync<CreatureDisplayInfo>((int)result.CreatureTemplateModel.CreatureDisplayId);
            }
            if (result.CreatureDisplayInfo != null)
            {
                result.CreatureDisplayInfoExtra = await GetSingleByIdAsync<CreatureDisplayInfoExtra>((int)result.CreatureDisplayInfo.ExtendedDisplayInfoId);
            }
            if (result.CreatureDisplayInfoExtra != null)
            {
                result.CreatureDisplayInfoOptions = await GetAsync<CreatureDisplayInfoOption>(new DbParameter(nameof(CreatureDisplayInfoOption.CreatureDisplayInfoExtraId),result.CreatureDisplayInfoExtra.Id));
            }
            if (result.CreatureDisplayInfoExtra != null)
            {
                result.NpcModelItemSlotDisplayInfos = await GetAsync<NpcModelItemSlotDisplayInfo>(new DbParameter(nameof(NpcModelItemSlotDisplayInfo.NpcModelId), result.CreatureDisplayInfoExtra.Id));
            }
            if (result.CreatureDisplayInfoOptions.Count > 0)
            {
                var customizationOption = await GetSingleByIdAsync<ChrCustomizationOption>(result.CreatureDisplayInfoOptions.First().ChrCustomizationOptionId);
                if (customizationOption != null)
                {
                    result.ChrModelId = customizationOption.ChrModelId;
                }
            }

            return result;
        }

        public async Task SaveAsync(CreatureDto dto, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            await SetIdAndVerifiedBuild(dto);

            await SaveAsync(dto.Entity);
            await SaveAsync(dto.CreatureTemplate);
            await SaveAsync(dto.CreatureDisplayInfo);
            await SaveAsync(dto.CreatureEquipTemplate);
            await SaveAsync(dto.CreatureTemplateAddon);
            await SaveAsync(dto.CreatureModelInfo);
            await SaveAsync(dto.CreatureDisplayInfoExtra);
            await SaveAsync(dto.CreatureTemplateModel);
            await SaveAsync(dto.CreatureDisplayInfoOptions.ToArray());
            await SaveAsync(dto.NpcModelItemSlotDisplayInfos.ToArray());
        }

        public async Task<bool> DeleteAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var dto = await GetByIdAsync(id);
            if(null == dto)
            {
                return false;
            }

            dto.CreatureDisplayInfoOptions.ForEach(async s =>
            {
                await DeleteAsync(s);
            });

            dto.NpcModelItemSlotDisplayInfos.ForEach(async s => 
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
            await DeleteAsync(dto.Entity);

            return true;
        }
    }
}
