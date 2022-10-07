using DBDefsLib;

namespace HotfixMods.Providers.Db2.WoWDev.Client
{
    /*
     * This client is getting data from the <a href="https://github.com/wowdev/WoWDBDefs">WoWDBDefs repository in GitHub, by wowdev</a>.
     * Code is mostly from <a href="https://github.com/wowdev/DBCD>WoWDev's DBCD repository</a>.
     * Helper methods are based on <a href="https://github.com/MaxtorCoder/Wow.DB2DefinitionDumper>MaxtorCoder's Wow.DB2DefinitionDumper</a>.
     */

    public partial class Db2Client
    {
        HttpClient _httpClient;

        public Db2Client(HttpClient? httpClient = null)
        {
            _httpClient = httpClient ?? new();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "HotfixMods");
        }

        public async Task<IEnumerable<IDictionary<string, KeyValuePair<Type, object>>>> ReadDb2FileAsync(string db2Path, string db2Name, string build)
        {
            return await ReadDb2Async(db2Path, db2Name, build);
        }

        public async Task<IDictionary<string, Type>> GetAvailableColumnsAsync(string db2Name, string build)
        {
            return await GetColumnsAsync(db2Name, build);
        }

        public async Task<IEnumerable<string>> GetAvailableDefinitionsAsync()
        {
            return await GetAllDefinitionsAsync();
        }

        public async Task<IEnumerable<string>> GetAvailableBuildsForDefinitionAsync(string db2Name)
        {
            return await GetBuildsAsync(db2Name);
        }


    }
}
