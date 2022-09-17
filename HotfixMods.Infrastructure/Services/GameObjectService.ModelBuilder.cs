using HotfixMods.Infrastructure.DefaultModels;
using HotfixMods.Core.Models;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Core.Enums;

namespace HotfixMods.Infrastructure.Services
{
    public partial class GameObjectService
    {
        GameObjectDisplayInfo BuildGameObjectDisplayInfo(GameObjectDto gameObjectDto)
        {
            gameObjectDto.AddHotfix(gameObjectDto.Id, TableHashes.GAME_OBJECT_DISPLAY_INFO, HotfixStatuses.VALID);
            return new GameObjectDisplayInfo()
            {
                Id = gameObjectDto.Id,
                VerifiedBuild = VerifiedBuild,

                FileDataId = gameObjectDto.FileDataId ?? Default.GameObjectDisplayInfo.FileDataId,
                GeoBox0 = gameObjectDto.GeoBox0 ?? Default.GameObjectDisplayInfo.GeoBox0,
                GeoBox1 = gameObjectDto.GeoBox1 ?? Default.GameObjectDisplayInfo.GeoBox1,
                GeoBox2 = gameObjectDto.GeoBox2 ?? Default.GameObjectDisplayInfo.GeoBox2,
                GeoBox3 = gameObjectDto.GeoBox3 ?? Default.GameObjectDisplayInfo.GeoBox3,
                GeoBox4 = gameObjectDto.GeoBox4 ?? Default.GameObjectDisplayInfo.GeoBox4,
                GeoBox5 = gameObjectDto.GeoBox5 ?? Default.GameObjectDisplayInfo.GeoBox5
            };
        }

        GameObjectTemplate BuildGameObjectTemplate(GameObjectDto gameObjectDto)
        {
            return new GameObjectTemplate()
            {
                Entry = gameObjectDto.Id,
                VerifiedBuild = VerifiedBuild,
                DisplayId = gameObjectDto.Id,

                Name = gameObjectDto.Name ?? Default.GameObjectTemplate.Name,
                CastBarCaption = gameObjectDto.CastBarCaption ?? Default.GameObjectTemplate.CastBarCaption,
                Size = gameObjectDto.Size ?? Default.GameObjectTemplate.Size,
                Type = gameObjectDto.Type ?? Default.GameObjectTemplate.Type,

                AiName = Default.GameObjectTemplate.AiName,
                IconName = Default.GameObjectTemplate.IconName,
                ScriptName = Default.GameObjectTemplate.ScriptName,
                Unk1 = Default.GameObjectTemplate.Unk1
            };
        }

        GameObjectTemplateAddon BuildGameObjectTemplateAddon(GameObjectDto gameObjectDto)
        {
            return new GameObjectTemplateAddon()
            {
                Entry = gameObjectDto.Id,
                //VerifiedBuild = VerifiedBuild,
                
                Faction = gameObjectDto.Faction ?? Default.GameObjectTemplateAddon.Faction,
                Flags = gameObjectDto.Flags ?? Default.GameObjectTemplateAddon.Flags
            };
        }
    }
}
