using HotfixMods.Core.Models.Db2;

namespace HotfixMods.Infrastructure.Services
{
    public partial class CreatureService
    {
        public async Task<Dictionary<ushort, string>> GetFactionOptionsAsync()
        {
            var results = new Dictionary<ushort, string>();
            var factions = await GetFromClientOnlyAsync<Faction>();
            foreach (var faction in factions)
            {
                results.Add((ushort)faction.ID, faction.Name);
            }
            return results;
        }
    }
}
