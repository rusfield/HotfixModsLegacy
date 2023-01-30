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
            var hotfixModsEntityId = await GetIdByConditionsAsync<HotfixModsEntity>(dto.HotfixModsEntity.Id, dto.IsUpdate);
            var gameobjectTemplateId = await GetIdByConditionsAsync<GameobjectTemplate>(dto.GameobjectTemplate.Entry, dto.IsUpdate);
            var gameobjectDisplayInfoId = await GetIdByConditionsAsync<GameobjectDisplayInfo>(dto.GameobjectDisplayInfo.Id, dto.IsUpdate);

            // Step 2: Prepare IDs of list entities
            // Nothing to do here

            // Step 3: Populate entities
            dto.HotfixModsEntity.Id = hotfixModsEntityId;
            dto.HotfixModsEntity.RecordId = gameobjectTemplateId;
            dto.HotfixModsEntity.VerifiedBuild = VerifiedBuild;

            dto.GameobjectTemplate.Entry = gameobjectTemplateId;
            dto.GameobjectTemplate.VerifiedBuild = VerifiedBuild;
            dto.GameobjectTemplate.DisplayId = gameobjectDisplayInfoId;

            dto.GameobjectDisplayInfo.Id = gameobjectDisplayInfoId;
            dto.GameobjectDisplayInfo.VerifiedBuild = VerifiedBuild;

            if(dto.GameobjectTemplateAddon != null)
            {
                dto.GameobjectTemplateAddon.Entry= gameobjectTemplateId;
                //dto.GameobjectTemplateAddon.VerifiedBuild = VerifiedBuild;
            }
        }
    }
}
