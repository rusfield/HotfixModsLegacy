using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Services
{
    public partial class GameobjectService
    {
        protected async Task SetIdAndVerifiedBuild(GameobjectDto gameobjectDto)
        {
            if (!gameobjectDto.IsUpdate)
            {
                var newGameobjectTemplateId = await GetNextIdAsync<GameobjectTemplate>();
                var newGameobjectDisplayInfoId = await GetNextIdAsync<GameobjectDisplayInfo>();


                gameobjectDto.Entity.Id = await GetNextIdAsync<HotfixModsEntity>();
                gameobjectDto.Entity.RecordId = newGameobjectDisplayInfoId;
                gameobjectDto.GameobjectTemplate.DisplayId = (uint)newGameobjectDisplayInfoId;
                gameobjectDto.GameobjectTemplate.Entry = (uint)newGameobjectTemplateId;
                gameobjectDto.GameobjectTemplateAddon.Entry = (uint)newGameobjectTemplateId;
                gameobjectDto.GameobjectDisplayInfo.Id = newGameobjectDisplayInfoId;
            }

            gameobjectDto.Entity.VerifiedBuild = VerifiedBuild;
            gameobjectDto.GameobjectTemplate.VerifiedBuild = VerifiedBuild;
            gameobjectDto.GameobjectDisplayInfo.VerifiedBuild = VerifiedBuild;
            // gameobjectDto.GameobjectTemplateAddon.VerifiedBuild = VerifiedBuild;
        }
    }
}
