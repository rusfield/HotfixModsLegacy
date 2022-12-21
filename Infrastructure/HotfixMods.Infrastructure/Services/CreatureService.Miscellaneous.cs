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
            if (!dto.IsUpdate)
            {
                var newCreatureTemplateId = await GetNextIdAsync<CreatureTemplate>();
                var newCreatureDisplayInfoId = await GetNextIdAsync<CreatureDisplayInfo>();
                var newCreatureDisplayInfoExtraId = await GetNextIdAsync<CreatureDisplayInfoExtra>();
                var newNpcModelItemSlotDisplayInfoId = await GetNextIdAsync<NpcModelItemSlotDisplayInfo>();
                var newCreatureDisplayInfoOptionsId = await GetNextIdAsync<CreatureDisplayInfoOption>();

                dto.Entity.Id = await GetNextIdAsync<HotfixModsEntity>();
                dto.Entity.RecordId = newCreatureTemplateId;

                dto.CreatureTemplate.Entry = (uint)newCreatureTemplateId;

                if (dto.CreatureTemplateAddon != null)
                {
                    dto.CreatureTemplateAddon.Entry = (uint)newCreatureTemplateId;
                }

                if (dto.CreatureTemplateModel != null)
                {
                    dto.CreatureTemplateModel.CreatureId = (uint)newCreatureTemplateId;
                    dto.CreatureTemplateModel.CreatureDisplayId = (uint)newCreatureDisplayInfoId;
                }

                if (dto.CreatureDisplayInfo != null)
                {
                    dto.CreatureDisplayInfo.Id = newCreatureDisplayInfoId;
                    dto.CreatureDisplayInfo.ExtendedDisplayInfoId = newCreatureDisplayInfoExtraId;
                }


                if (dto.CreatureDisplayInfoExtra != null)
                {
                    dto.CreatureDisplayInfoExtra.Id = newCreatureDisplayInfoExtraId;
                }

                if (dto.CreatureEquipTemplate != null)
                {
                    dto.CreatureEquipTemplate.CreatureId = (uint)newCreatureTemplateId;
                }

                if (dto.CreatureModelInfo != null)
                {
                    dto.CreatureModelInfo.DisplayId = (uint)newCreatureDisplayInfoId;
                }

                dto.NpcModelItemSlotDisplayInfos.ForEach(s =>
                {
                    s.Id = newNpcModelItemSlotDisplayInfoId;
                    s.NpcModelId = newCreatureDisplayInfoExtraId;

                    newNpcModelItemSlotDisplayInfoId++;
                });

                dto.CreatureDisplayInfoOptions.ForEach(s =>
                {
                    s.Id = newCreatureDisplayInfoOptionsId;
                    s.CreatureDisplayInfoExtraId = newCreatureDisplayInfoExtraId;

                    newCreatureDisplayInfoOptionsId++;
                });
            }

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
