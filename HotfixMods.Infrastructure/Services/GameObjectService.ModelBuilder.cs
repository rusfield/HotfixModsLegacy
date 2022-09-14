using HotfixMods.Core.Constants;
using HotfixMods.Core.Models;
using HotfixMods.Infrastructure.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Services
{
    public partial class GameObjectService
    {
        protected GameObjectDisplayInfo BuildGameObjectDisplayInfo(GameObjectDto gameObjectDto)
        {
            return new GameObjectDisplayInfo()
            {
                Id = gameObjectDto.Id,
                FileDataId = gameObjectDto.FileDataId ?? GameObjectDefaults.FileDataId,
                GeoBox0 = gameObjectDto.GeoBox0 ?? GameObjectDefaults.GeoBox0,
                GeoBox1 = gameObjectDto.GeoBox1 ?? GameObjectDefaults.GeoBox1,
                Geobox2 = gameObjectDto.GeoBox2 ?? GameObjectDefaults.GeoBox2,
                Geobox3 = gameObjectDto.GeoBox3 ?? GameObjectDefaults.GeoBox3,
                Geobox4 = gameObjectDto.GeoBox4 ?? GameObjectDefaults.GeoBox4,
                Geobox5 = gameObjectDto.GeoBox5 ?? GameObjectDefaults.GeoBox5
            };
        }

        protected GameObjectTemplate BuildGameObjectTemplate(GameObjectDto gameObjectDto)
        {
            return new GameObjectTemplate()
            {
                Entry = gameObjectDto.Id,
                Name = gameObjectDto.Name ?? GameObjectDefaults.Name,
                CastBarCaption = gameObjectDto.CastBarCaption ?? GameObjectDefaults.CastBarCaption,
                VerifiedBuild = VerifiedBuild,
                DisplayId = gameObjectDto.Id,
                Size = gameObjectDto.Size ?? GameObjectDefaults.Size,
                
                // TODO: Continue
            };
        }
    }
}
