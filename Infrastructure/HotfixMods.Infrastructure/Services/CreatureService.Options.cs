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

        public async Task<Dictionary<byte, string>> GetCreatureTypeOptionsAsync()
        {
            var results = new Dictionary<byte, string>();
            var creatureTypes = await GetFromClientOnlyAsync("CreatureType");
            foreach (var creatureType in creatureTypes)
            {
                var key = (uint)creatureType.Columns.Where(c => c.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault()!.Value;
                var value = creatureType.Columns.Where(c => c.Name.Equals("name", StringComparison.InvariantCultureIgnoreCase))?.FirstOrDefault()!.Value;

                results.Add((byte)key, value.ToString());
            }
            return results;
        }
    }
}
