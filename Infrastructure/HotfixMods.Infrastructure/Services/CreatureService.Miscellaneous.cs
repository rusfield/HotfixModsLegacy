using HotfixMods.Core.Enums.Db2;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class CreatureService
    {
        Dictionary<int, Dictionary<ChrCustomizationOption, List<ChrCustomizationChoice>>> customizationCache = new();

        public async Task<Dictionary<ChrCustomizationOption, List<ChrCustomizationChoice>>> GetCustomizationOptions(int chrRaceId, int gender, bool includeDruidForms = false)
        {
            var result = new Dictionary<ChrCustomizationOption, List<ChrCustomizationChoice>>();
            var chrRaceXChrModel = await GetSingleAsync<ChrRaceXChrModel>(new DbParameter(nameof(ChrRaceXChrModel.ChrRacesId), chrRaceId), new DbParameter(nameof(ChrRaceXChrModel.Sex), gender));
            if (null == chrRaceXChrModel)
            {
                // No customizations for this combination, or possibly old/missing data from ChrRaceXChrModel.
                return result;
            }
            else if (customizationCache.ContainsKey(chrRaceXChrModel.ChrModelId))
            {
                result = customizationCache[chrRaceXChrModel.ChrModelId];
            }
            else
            {
                var options = await GetAsync<ChrCustomizationOption>(new DbParameter(nameof(ChrCustomizationOption.ChrModelId), chrRaceXChrModel.ChrModelId));
                foreach (var option in options)
                {
                    var choices = await GetAsync<ChrCustomizationChoice>(new DbParameter(nameof(ChrCustomizationChoice.ChrCustomizationOptionId), option.Id));
                    result.Add(option, choices);
                }

                // In case it has been added elsewhere in the meantime
                if (!customizationCache.ContainsKey(chrRaceXChrModel.ChrModelId) && result.Count > 0)
                    customizationCache.Add(chrRaceXChrModel.ChrModelId, result);
            }

            if (!includeDruidForms)
            {
                var filteredResult = new Dictionary<ChrCustomizationOption, List<ChrCustomizationChoice>>();
                foreach (var option in result)
                {
                    // Currently, only druid forms are named like "Moonkin Form", etc.
                    // Edit this condition if it should affect new customizations at some point.
                    if (!option.Key.Name.ToLower().EndsWith(" form"))
                        filteredResult.Add(option.Key, option.Value);
                }
                return filteredResult;
            }
            return result;
        }

        async Task SetIdAndVerifiedBuild(CreatureDto dto)
        {
            // Step 1: Init IDs of single entities
            var hotfixModsEntityId = await GetIdByConditionsAsync<HotfixModsEntity>(dto.HotfixModsEntity.Id, dto.IsUpdate);
            var creatureTemplateId = await GetIdByConditionsAsync<CreatureTemplate>(dto.CreatureTemplate.Entry, dto.IsUpdate);
            var creatureDisplayInfoId = await GetIdByConditionsAsync<CreatureDisplayInfo>(dto.CreatureDisplayInfo.Id, dto.IsUpdate);
            var creatureDisplayInfoExtraId = await GetIdByConditionsAsync<CreatureDisplayInfoExtra>(dto.CreatureDisplayInfoExtra?.Id, dto.IsUpdate);

            // Step 2: Prepare IDs of list entities
            var nextNpcModelItemSlotDisplayInfo = await GetNextIdAsync<NpcModelItemSlotDisplayInfo>();
            var nextCreatureDisplayInfoOption = await GetNextIdAsync<CreatureDisplayInfoOption>();

            // Step 3: Populate entities
            dto.HotfixModsEntity.Id = hotfixModsEntityId;
            dto.HotfixModsEntity.RecordId = creatureTemplateId;
            dto.HotfixModsEntity.VerifiedBuild = VerifiedBuild;

            dto.CreatureTemplate.Entry = creatureTemplateId;
            dto.CreatureTemplate.VerifiedBuild = VerifiedBuild;

            dto.CreatureTemplateModel.CreatureId = creatureTemplateId;
            dto.CreatureTemplateModel.CreatureDisplayId = creatureDisplayInfoId;
            dto.CreatureTemplateModel.VerifiedBuild = VerifiedBuild;

            dto.CreatureDisplayInfo.Id = creatureDisplayInfoId;
            dto.CreatureDisplayInfo.ExtendedDisplayInfoId = (int)creatureDisplayInfoExtraId;
            dto.CreatureDisplayInfo.VerifiedBuild = VerifiedBuild;


            dto.CreatureModelInfo.DisplayId = creatureDisplayInfoId;
            dto.CreatureModelInfo.VerifiedBuild = VerifiedBuild;

            if (dto.CreatureEquipTemplate != null)
            {
                dto.CreatureEquipTemplate.CreatureId = creatureTemplateId;
                dto.CreatureEquipTemplate.VerifiedBuild = VerifiedBuild;
            }

            if (dto.CreatureTemplateAddon!= null)
            {
                dto.CreatureTemplateAddon.Entry = creatureTemplateId;
                //dto.CreatureTempalteAddon.VerifiedBuild = VerifiedBuild; // property does not currently exist
            }

            if (dto.CreatureDisplayInfoExtra != null)
            {
                dto.CreatureDisplayInfoExtra.Id = creatureDisplayInfoExtraId;
                dto.CreatureDisplayInfoExtra.VerifiedBuild = VerifiedBuild;

                if (dto.NpcModelItemSlotDisplayInfo?.Any() ?? false)
                {
                    dto.NpcModelItemSlotDisplayInfo.ForEach(item =>
                    {
                        item.NpcModelId = (int)creatureDisplayInfoExtraId;
                        item.Id = nextNpcModelItemSlotDisplayInfo++;
                        item.VerifiedBuild = VerifiedBuild;
                    });
                }

                if (dto.CreatureDisplayInfoOption?.Any() ?? false)
                {
                    dto.CreatureDisplayInfoOption.ForEach(item =>
                    {
                        item.CreatureDisplayInfoExtraId = (int)creatureDisplayInfoExtraId;
                        item.Id = nextCreatureDisplayInfoOption++;
                        item.VerifiedBuild = VerifiedBuild;
                    });
                }
            }


        }
    }
}
