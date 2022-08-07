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
    public partial class CreatureService : Service
    {
        public CreatureService(IDb2Provider db2Provider, IMySqlProvider mySqlProvider) : base(db2Provider, mySqlProvider) { }

        public async Task SaveItemAsync(CreatureDto creature)
        {

        }

        public async Task<List<CreatureDashboard>> GetCreatureDashboardAsync()
        {
            return null;
        }
    }
}
