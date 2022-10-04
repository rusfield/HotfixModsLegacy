using DBDefsLib;

namespace HotfixMods.Providers.DbDef.WoWDev.Client
{
    /*
     * This client is getting data from the <a href="https://github.com/wowdev/WoWDBDefs">WoWDBDefs repository in GitHub, by wowdev</a>.
     * Code is based on <a href="https://github.com/MaxtorCoder/Wow.DB2DefinitionDumper>MaxtorCoder's Wow.DB2DefinitionDumper</a>.
     */

    public partial class DbDefClient
    {
        HttpClient _httpClient;

        public DbDefClient(HttpClient? httpClient = null)
        {
            _httpClient = httpClient ?? new();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "HotfixMods");
        }

        public async Task<IDictionary<string, Type>> GetAvailableColumnsAsync(string db2Name, string build)
        {
            if(string.IsNullOrWhiteSpace(db2Name) || string.IsNullOrWhiteSpace(build))
            {
                throw new Exception("Db2 Name and Build must have a value.");
            }
            db2Name = db2Name.Trim();
            if (db2Name.EndsWith(".db2") || db2Name.EndsWith(".dbd"))
                db2Name = db2Name.Substring(0, db2Name.Length - 4);

            return await GetColumnsAsync(db2Name, build);
        }

        public async Task<IEnumerable<string>> GetAvailableDefinitionsAsync()
        {
            return await GetDefinitionsAsync();
        }

        public async Task<IEnumerable<string>> GetAvailableBuildsForDefinitionAsync(string db2Name)
        {
            if (string.IsNullOrWhiteSpace(db2Name))
            {
                throw new Exception("Db2 Name and Build must have a value.");
            }
            db2Name = db2Name.Trim();
            if (db2Name.EndsWith(".db2") || db2Name.EndsWith(".dbd"))
                db2Name = db2Name.Substring(0, db2Name.Length - 4);

            return await GetBuildsAsync(db2Name);
        }


    }
}
