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
                foreach(var option in result)
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

            var newCreatureTemplateId = await GetNextIdAsync<CreatureTemplate>();
            var newCreatureDisplayInfoId = await GetNextIdAsync<CreatureDisplayInfo>();
            var newCreatureDisplayInfoExtraId = await GetNextIdAsync<CreatureDisplayInfoExtra>();
            var newNpcModelItemSlotDisplayInfoId = await GetNextIdAsync<NpcModelItemSlotDisplayInfo>();
            var newCreatureDisplayInfoOptionsId = await GetNextIdAsync<CreatureDisplayInfoOption>();

            if (dto.HotfixModsEntity.RecordId == 0 || !dto.IsUpdate)
            {
                dto.HotfixModsEntity.RecordId = newCreatureTemplateId;
                dto.HotfixModsEntity.Id = await GetNextIdAsync<HotfixModsEntity>();
            }

            if (dto.CreatureTemplate.Entry == 0 || !dto.IsUpdate)
            {
                dto.CreatureTemplate.Entry = (uint)newCreatureTemplateId;
            }

            if (dto.CreatureTemplateAddon != null && (dto.CreatureTemplateAddon.Entry == 0 || !dto.IsUpdate))
            {
                dto.CreatureTemplateAddon.Entry = dto.CreatureTemplate.Entry;
            }

            if (dto.CreatureDisplayInfoExtra != null && (dto.CreatureDisplayInfoExtra.Id == 0 || !dto.IsUpdate))
            {
                dto.CreatureDisplayInfoExtra.Id = newCreatureDisplayInfoExtraId;
            }

            if (dto.CreatureDisplayInfo != null && (dto.CreatureDisplayInfo.Id == 0 || !dto.IsUpdate))
            {
                dto.CreatureDisplayInfo.Id = newCreatureDisplayInfoId;
                dto.CreatureDisplayInfo.ExtendedDisplayInfoId = (int)dto.CreatureDisplayInfoExtra.Id;
            }

            if (dto.CreatureTemplateModel != null && (dto.CreatureTemplateModel.CreatureId == 0 || !dto.IsUpdate))
            {
                dto.CreatureTemplateModel.CreatureId = dto.CreatureTemplate.Entry;
                dto.CreatureTemplateModel.CreatureDisplayId = dto.CreatureDisplayInfo.Id;
            }

            if (dto.CreatureEquipTemplate != null && (dto.CreatureEquipTemplate.Id == 0 || !dto.IsUpdate))
            {
                dto.CreatureEquipTemplate.CreatureId = dto.CreatureTemplate.Entry;
            }

            if (dto.CreatureModelInfo != null && (dto.CreatureModelInfo.DisplayId == 0 || !dto.IsUpdate))
            {
                dto.CreatureModelInfo.DisplayId = dto.CreatureDisplayInfo.Id;
            }

            dto.NpcModelItemSlotDisplayInfo.ForEach(s =>
            {
                if (s.Id == 0 || !dto.IsUpdate)
                {
                    s.Id = newNpcModelItemSlotDisplayInfoId;
                    s.NpcModelId = (int)dto.CreatureDisplayInfoExtra.Id;

                    newNpcModelItemSlotDisplayInfoId++;
                }
            });

            dto.CreatureDisplayInfoOption.ForEach(s =>
            {
                if (s.Id == 0 || !dto.IsUpdate)
                {
                    s.Id = newCreatureDisplayInfoOptionsId;
                    s.CreatureDisplayInfoExtraId =(int) newCreatureDisplayInfoExtraId;

                    newCreatureDisplayInfoOptionsId++;
                }
            });


            dto.HotfixModsEntity.VerifiedBuild = VerifiedBuild;
            dto.CreatureTemplate.VerifiedBuild = VerifiedBuild;
            //dto.CreatureTemplateAddon.VerifiedBuild = VerifiedBuild; 

            if (dto.CreatureTemplateModel != null)
                dto.CreatureTemplateModel.VerifiedBuild = VerifiedBuild;

            if (dto.CreatureDisplayInfo != null)
                dto.CreatureDisplayInfo.VerifiedBuild = VerifiedBuild;

            if (dto.CreatureDisplayInfoExtra != null)
                dto.CreatureDisplayInfoExtra.VerifiedBuild = VerifiedBuild;

            if (dto.CreatureEquipTemplate != null)
                dto.CreatureEquipTemplate.VerifiedBuild = VerifiedBuild;

            if (dto.CreatureModelInfo != null)
                dto.CreatureModelInfo.VerifiedBuild = VerifiedBuild;

            dto.NpcModelItemSlotDisplayInfo.ForEach(s =>
            {
                s.VerifiedBuild = VerifiedBuild;
            });
            dto.CreatureDisplayInfoOption.ForEach(s =>
            {
                s.VerifiedBuild = VerifiedBuild;
            });
        }
    }
}
