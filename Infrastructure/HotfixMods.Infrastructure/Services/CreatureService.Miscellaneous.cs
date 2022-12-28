using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class CreatureService
    {
        Dictionary<int, Dictionary<ChrCustomizationOption, List<ChrCustomizationChoice>>> customizationCache = new();

        public async Task<Dictionary<ChrCustomizationOption, List<ChrCustomizationChoice>>> GetCustomizationOptions(int chrModelId)
        {
            if (chrModelId == 0)
            {
                // This selection seems to be a bit bugged in DB.
                // Use it as default and return nothing.
                return new();
            }
            else if (customizationCache.ContainsKey(chrModelId))
            {
                return customizationCache[chrModelId];
            }

            var result = new Dictionary<ChrCustomizationOption, List<ChrCustomizationChoice>>();

            var options = await GetAsync<ChrCustomizationOption>(new DbParameter(nameof(ChrCustomizationOption.ChrModelId), chrModelId));
            foreach (var option in options)
            {
                var choices = await GetAsync<ChrCustomizationChoice>(new DbParameter(nameof(ChrCustomizationChoice.ChrCustomizationOptionId), option.Id));
                result.Add(option, choices);
            }

            // In case it has been added elsewhere in the meantime
            if (!customizationCache.ContainsKey(chrModelId) && result.Count > 0)
                customizationCache.Add(chrModelId, result);
            return result;
        }

        async Task SetIdAndVerifiedBuild(CreatureDto dto)
        {

            var newCreatureTemplateId = await GetNextIdAsync<CreatureTemplate>();
            var newCreatureDisplayInfoId = await GetNextIdAsync<CreatureDisplayInfo>();
            var newCreatureDisplayInfoExtraId = await GetNextIdAsync<CreatureDisplayInfoExtra>();
            var newNpcModelItemSlotDisplayInfoId = await GetNextIdAsync<NpcModelItemSlotDisplayInfo>();
            var newCreatureDisplayInfoOptionsId = await GetNextIdAsync<CreatureDisplayInfoOption>();

            if (dto.Entity.RecordId == 0 || !dto.IsUpdate)
            {
                dto.Entity.RecordId = newCreatureTemplateId;
                dto.Entity.Id = await GetNextIdAsync<HotfixModsEntity>();
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
                dto.CreatureDisplayInfo.ExtendedDisplayInfoId = dto.CreatureDisplayInfoExtra.Id;
            }

            if (dto.CreatureTemplateModel != null && (dto.CreatureTemplateModel.CreatureId == 0 || !dto.IsUpdate))
            {
                dto.CreatureTemplateModel.CreatureId = dto.CreatureTemplate.Entry;
                dto.CreatureTemplateModel.CreatureDisplayId = (uint)dto.CreatureDisplayInfo.Id;
            }

            if (dto.CreatureEquipTemplate != null && (dto.CreatureEquipTemplate.Id == 0 || !dto.IsUpdate))
            {
                dto.CreatureEquipTemplate.CreatureId = dto.CreatureTemplate.Entry;
            }

            if (dto.CreatureModelInfo != null && (dto.CreatureModelInfo.DisplayId == 0 || !dto.IsUpdate))
            {
                dto.CreatureModelInfo.DisplayId = (uint)dto.CreatureDisplayInfo.Id;
            }

            dto.NpcModelItemSlotDisplayInfos.ForEach(s =>
            {
                if (s.Id == 0 || !dto.IsUpdate)
                {
                    s.Id = newNpcModelItemSlotDisplayInfoId;
                    s.NpcModelId = dto.CreatureDisplayInfoExtra.Id;

                    newNpcModelItemSlotDisplayInfoId++;
                }
            });

            dto.CreatureDisplayInfoOptions.ForEach(s =>
            {
                if (s.Id == 0 || !dto.IsUpdate)
                {
                    s.Id = newCreatureDisplayInfoOptionsId;
                    s.CreatureDisplayInfoExtraId = newCreatureDisplayInfoExtraId;

                    newCreatureDisplayInfoOptionsId++;
                }
            });


            dto.Entity.VerifiedBuild = VerifiedBuild;
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

            dto.NpcModelItemSlotDisplayInfos.ForEach(s =>
            {
                s.VerifiedBuild = VerifiedBuild;
            });
            dto.CreatureDisplayInfoOptions.ForEach(s =>
            {
                s.VerifiedBuild = VerifiedBuild;
            });
        }
    }
}
