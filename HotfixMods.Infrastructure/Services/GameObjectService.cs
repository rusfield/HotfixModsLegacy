using HotfixMods.Core.Enums;
using HotfixMods.Core.Models;
using HotfixMods.Core.Providers;
using HotfixMods.Infrastructure.DashboardModels;
using HotfixMods.Infrastructure.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Services
{
    public partial class GameObjectService : Service
    {
        public GameObjectService(IDb2Provider db2Provider, IMySqlProvider mySqlProvider) : base(db2Provider, mySqlProvider) { }

        public async Task SaveAsync(GameObjectDto dto)
        {
            var hotfixId = await GetNextHotfixIdAsync();
            dto.InitHotfixes(hotfixId, VerifiedBuild);

            dto.HotfixModsName = dto.Name;

            // Nothing special to do whether IsUpdate is true or false for GameObject.
            
            await _mySql.AddOrUpdateAsync(BuildGameObjectTemplate(dto));
            await _mySql.AddOrUpdateAsync(BuildGameObjectTemplateAddon(dto));
            await _mySql.AddOrUpdateAsync(BuildGameObjectDisplayInfo(dto));

            await _mySql.AddOrUpdateAsync(BuildHotfixModsData(dto));
            await AddHotfixes(dto.GetHotfixes());
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteFromWorldAsync(id);
            await DeleteFromHotfixesAsync(id);
        }

        public async Task<GameObjectDto> GetNewAsync(Action<string, string, int>? progressCallback = null)
        {
            return new GameObjectDto()
            {
                Id = await GetNextIdAsync()
            };
        }

        public async Task<GameObjectDto?> GetByIdAsync(int id, Action<string, string, int>? progressCallback = null)
        {
            var gameObjectTemplate = await _mySql.GetSingleAsync<GameObjectTemplate>(c => c.Entry == id);
            if(null == gameObjectTemplate)
            {
                return null;
            }

            var gameObjectDisplayInfo = await _mySql.GetSingleAsync<GameObjectDisplayInfo>(g => g.Id == gameObjectTemplate.DisplayId) ?? await _db2.GetSingleAsync<GameObjectDisplayInfo>(g => g.Id == gameObjectTemplate.DisplayId);
            if (null == gameObjectDisplayInfo)
            {
                return null;
            }

            var gameObjectTemplateAddon = await _mySql.GetSingleAsync<GameObjectTemplateAddon>(c => c.Entry == id);
            var hmData = await _mySql.GetSingleAsync<HotfixModsData>(h => h.RecordId == id && h.VerifiedBuild == VerifiedBuild);

            var result = new GameObjectDto()
            {
                Id = hmData != null ? id : await GetNextIdAsync(),
                CastBarCaption = gameObjectTemplate.CastBarCaption,
                Name = gameObjectTemplate.Name,
                Size = gameObjectTemplate.Size,
                Type = gameObjectTemplate.Type,
                Flags = gameObjectTemplateAddon?.Flags,
                Faction = gameObjectTemplateAddon?.Faction,
                FileDataId = gameObjectDisplayInfo.FileDataId,
                GeoBox0 = gameObjectDisplayInfo.GeoBox0,
                GeoBox1 = gameObjectDisplayInfo.GeoBox1,
                GeoBox2 = gameObjectDisplayInfo.GeoBox2,
                GeoBox3 = gameObjectDisplayInfo.GeoBox3,
                GeoBox4 = gameObjectDisplayInfo.GeoBox4,
                GeoBox5 = gameObjectDisplayInfo.GeoBox5,
                HotfixModsName = hmData?.Name,
                HotfixModsComment = hmData?.Comment,
                IsUpdate = hmData != null
            };


            return result;
        }

        public async Task<List<DashboardModel>> GetDashboardAsync()
        {
            var hotfixModsData = await _mySql.GetAsync<HotfixModsData>(c => c.VerifiedBuild == VerifiedBuild);
            var result = new List<DashboardModel>();
            foreach (var data in hotfixModsData)
            {
                result.Add(new DashboardModel()
                {
                    Id = data.RecordId,
                    Name = data.Name,
                    Comment = data.Comment,
                    AvatarUrl = "TODO"
                });
            }
            // Newest on top
            result.Reverse();
            return result;
        }



        async Task DeleteFromHotfixesAsync(int id)
        {
            var gameobjectDisplayInfo = await _mySql.GetSingleAsync<GameObjectDisplayInfo>(g => g.Id == id);
            var hotfixModsData = await _mySql.GetSingleAsync<HotfixModsData>(h => h.Id == id && h.VerifiedBuild == VerifiedBuild);

            if (null != gameobjectDisplayInfo)
                await _mySql.DeleteAsync(gameobjectDisplayInfo);

            var hotfixData = await _mySql.GetAsync<HotfixData>(h => h.UniqueId == id && h.VerifiedBuild == VerifiedBuild);
            if (hotfixData != null && hotfixData.Count() > 0)
            {
                foreach (var hotfix in hotfixData)
                {
                    hotfix.Status = HotfixStatuses.INVALID;
                }
                await _mySql.AddOrUpdateAsync(hotfixData.ToArray());
            }

            if (null != hotfixModsData)
                await _mySql.DeleteAsync(hotfixModsData);
        }

        async Task DeleteFromWorldAsync(int id)
        {
            var gameObjects = await _mySql.GetAsync<GameObject>(c => c.Guid == id);
            var gameObjectTemplate = await _mySql.GetSingleAsync<GameObjectTemplate>(c => c.Entry == id);
            var gameObjectTemplateAddon = await _mySql.GetSingleAsync<GameObjectTemplateAddon>(c => c.Entry == id);


            if (null != gameObjectTemplate)
                await _mySql.DeleteAsync(gameObjectTemplate);

            if (null != gameObjectTemplateAddon)
                await _mySql.DeleteAsync(gameObjectTemplateAddon);

            if (gameObjects.Count() > 0)
                await _mySql.DeleteAsync(gameObjects.ToArray());
        }
    }
}
