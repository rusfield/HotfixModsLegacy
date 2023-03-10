using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;

namespace HotfixMods.Infrastructure.Services
{
    public partial class CreatureService
    {
        public async Task<Dictionary<ushort, string>> GetFactionOptionsAsync()
        {
            return await GetOptionsAsync<ushort, uint>(_appConfig.HotfixesSchema, "Faction", "Name");
        }

        public async Task<Dictionary<byte, string>> GetCreatureTypeOptionsAsync()
        {
            return await GetOptionsAsync<byte, uint>(_appConfig.HotfixesSchema, "CreatureType", "Name");
        }

        public async Task<Dictionary<byte, string>> GetRankOptionsAsync()
        {
            return await GetEnumOptionsAsync<byte>(typeof(CreatureTemplate), nameof(CreatureTemplate.Rank));
        }
    }
}
