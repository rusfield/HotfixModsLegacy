using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class GameobjectService
    {
        async Task SetIdAndVerifiedBuild(GameobjectDto gameobjectDto)
        {
            if (!gameobjectDto.IsUpdate)
            {
                var newGameobjectTemplateId = await GetNextIdAsync<GameobjectTemplate>();
                var newGameobjectDisplayInfoId = await GetNextIdAsync<GameobjectDisplayInfo>();


                gameobjectDto.Entity.Id = await GetNextIdAsync<HotfixModsEntity>();
                gameobjectDto.Entity.RecordId = newGameobjectDisplayInfoId;
                gameobjectDto.GameobjectTemplate.DisplayId = (uint)newGameobjectDisplayInfoId;
                gameobjectDto.GameobjectTemplate.Entry = (uint)newGameobjectTemplateId;

                if (gameobjectDto.GameobjectTemplateAddon != null)
                    gameobjectDto.GameobjectTemplateAddon.Entry = (uint)newGameobjectTemplateId;

                if (gameobjectDto.GameobjectDisplayInfo != null)
                    gameobjectDto.GameobjectDisplayInfo.Id = newGameobjectDisplayInfoId;
            }

            gameobjectDto.Entity.VerifiedBuild = VerifiedBuild;
            gameobjectDto.GameobjectTemplate.VerifiedBuild = VerifiedBuild;

            if(gameobjectDto.GameobjectDisplayInfo != null)
                gameobjectDto.GameobjectDisplayInfo.VerifiedBuild = VerifiedBuild;

            // VerifiedBuild currently not in this model
            // gameobjectDto.GameobjectTemplateAddon.VerifiedBuild = VerifiedBuild;
        }
    }
}
