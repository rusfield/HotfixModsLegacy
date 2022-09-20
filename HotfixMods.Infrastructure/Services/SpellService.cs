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
    public class SpellService : Service
    {
        public SpellService(IDb2Provider db2Provider, IMySqlProvider mySqlProvider) : base(db2Provider, mySqlProvider) { }

        public async Task SaveAsync(SpellDto spellDto)
        {

        }

        public async Task<SpellDto> GetNewSpellAsync(Action<string, string, int>? progressCallback = null)
        {
            return new SpellDto()
            {
                SpellEffects = new()
            };
        }

        public async Task<SpellDto?> GetSpellByIdAsync(int spellId, Action<string, string, int>? progressCallback = null)
        {
            return new SpellDto()
            {
                // TODO
            };
        }

        public async Task<List<DashboardModel>> GetDashboardAsync()
        {
            return new();
        }

        public async Task DeleteAsync(int spellId)
        {

        }
    }
}
