using HotfixMods.Core.Models.Db2;

namespace HotfixMods.Infrastructure.Services
{
    public partial class CreatureService
    {
        public async Task<Dictionary<ushort, string>> GetFactionOptionsAsync()
        {
            return await GetOptionsAsync<ushort, uint>("Faction", "Name");
        }

        public async Task<Dictionary<byte, string>> GetCreatureTypeOptionsAsync()
        {
            return await GetOptionsAsync<byte, uint>("CreatureType", "Name");
        }
    }
}
