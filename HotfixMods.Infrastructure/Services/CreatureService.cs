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
    public partial class CreatureService : Service
    {
        public CreatureService(IDb2Provider db2Provider, IMySqlProvider mySqlProvider) : base(db2Provider, mySqlProvider) { }

        public async Task SaveItemAsync(CreatureDto creature)
        {

        }

        public async Task DeleteItem(int id)
        {

        }

        public async Task<List<CreatureDashboard>> GetCreatureDashboardAsync()
        {
            var creatures = await _mySql.GetManyAsync<CreatureTemplate>(c => c.VerifiedBuild == VerifiedBuild);
            var result = new List<CreatureDashboard>();
            foreach (var creature in creatures)
            {
                var displayInfo = await _mySql.GetAsync<CreatureDisplayInfoExtra>(c => c.Id == creature.Entry);
                if (displayInfo == null)
                    continue;
                result.Add(new CreatureDashboard()
                {
                    Id = creature.Entry,
                    Name = creature.Name,
                    AvatarUrl = $"/images/creatures/avatars/{displayInfo.DisplaySexId.ToString().ToLower()}/{displayInfo.DisplayRaceId.ToString().ToLower().Replace("_", "")}.jpg"
                });
            }
            // Newest on top
            result.Reverse();
            return result;
        }
    }
}
