using HotfixMods.Core.Providers;
using HotfixMods.Infrastructure.Dashboard;
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

        public async Task SaveAsync(GameObjectDto gameObjectDto)
        {

        }

        public async Task DeleteAsync(int id)
        {

        }

        public async Task<GameObjectDto> GetNewGameObjectAsync(Action<string, string, int>? progressCallback = null)
        {
            return new GameObjectDto()
            {
                Id = await GetNextIdAsync()
            };
        }

        public async Task<GameObjectDto> GetGameObjectByIdAsync(int id, Action<string, string, int>? progressCallback = null)
        {
            return null;
        }

        public async Task<List<DashboardModel>> GetDashboardAsync()
        {
            return new();
        }
    }
}
