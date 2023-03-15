using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Extensions;

namespace HotfixMods.Infrastructure.Services
{
    public partial class CreatureService
    {
        public async Task<Dictionary<ushort, string>> GetFactionOptionsAsync()
        {
            var results = new Dictionary<ushort, string>();
            var factions = await GetOptionsAsync<ushort, uint>(_appConfig.HotfixesSchema, "Faction", "Name");
            var factionTemplates = await GetAsync(_appConfig.HotfixesSchema, "FactionTemplate", false, true);

            foreach(var factionTemplate in factionTemplates)
            {
                var id = factionTemplate.GetIdValue();
                string displayName = id.ToString();
                if (factions.ContainsKey((ushort)id))
                    displayName += $" - {factions[(ushort)id]}";

                results.Add((ushort)id, displayName);
            }
            return results;
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
