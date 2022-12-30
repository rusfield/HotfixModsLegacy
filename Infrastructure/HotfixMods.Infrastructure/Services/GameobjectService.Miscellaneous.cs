using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class GameobjectService
    {
        async Task SetIdAndVerifiedBuild(GameobjectDto dto)
        {
            if (!dto.IsUpdate)
            {
                var newGameobjectTemplateId = await GetNextIdAsync<GameobjectTemplate>();
                var newGameobjectDisplayInfoId = await GetNextIdAsync<GameobjectDisplayInfo>();


                dto.HotfixModsEntity.Id = await GetNextIdAsync<HotfixModsEntity>();
                dto.HotfixModsEntity.RecordId = newGameobjectDisplayInfoId;
                dto.GameobjectTemplate.DisplayId = (uint)newGameobjectDisplayInfoId;
                dto.GameobjectTemplate.Entry = (uint)newGameobjectTemplateId;

                if (dto.GameobjectTemplateAddon != null)
                    dto.GameobjectTemplateAddon.Entry = (uint)newGameobjectTemplateId;

                if (dto.GameobjectDisplayInfo != null)
                    dto.GameobjectDisplayInfo.Id = newGameobjectDisplayInfoId;
            }

            dto.HotfixModsEntity.VerifiedBuild = VerifiedBuild;
            dto.GameobjectTemplate.VerifiedBuild = VerifiedBuild;

            if(dto.GameobjectDisplayInfo != null)
                dto.GameobjectDisplayInfo.VerifiedBuild = VerifiedBuild;

            // VerifiedBuild currently not in this model
            // gameobjectDto.GameobjectTemplateAddon.VerifiedBuild = VerifiedBuild;
        }
    }
}
