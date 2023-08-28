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
            // Step 1: Init IDs of single entities
            var hotfixModsEntityId = await GetIdByConditionsAsync<HotfixModsEntity>(dto.HotfixModsEntity.ID, dto.IsUpdate);
            var gameobjectTemplateId = await GetIdByConditionsAsync<GameobjectTemplate>(dto.GameobjectTemplate.Entry, dto.IsUpdate);
            var gameobjectDisplayInfoId = await GetIdByConditionsAsync<GameobjectDisplayInfo>((ulong)dto.GameobjectDisplayInfo.ID, dto.IsUpdate);

            // Step 2: Prepare IDs of list entities
            // Nothing to do here

            // Step 3: Populate entities
            dto.HotfixModsEntity.ID = hotfixModsEntityId;
            dto.HotfixModsEntity.RecordID = gameobjectTemplateId;
            dto.HotfixModsEntity.VerifiedBuild = VerifiedBuild;

            dto.GameobjectTemplate.Entry = (uint)gameobjectTemplateId;
            dto.GameobjectTemplate.VerifiedBuild = VerifiedBuild;
            dto.GameobjectTemplate.DisplayID = (uint)gameobjectDisplayInfoId;

            dto.GameobjectDisplayInfo.ID = (int)gameobjectDisplayInfoId;
            dto.GameobjectDisplayInfo.VerifiedBuild = VerifiedBuild;

            if(dto.GameobjectTemplateAddon != null)
            {
                dto.GameobjectTemplateAddon.Entry= (uint)gameobjectTemplateId;
                //dto.GameobjectTemplateAddon.VerifiedBuild = VerifiedBuild;
            }
        }
    }
}
