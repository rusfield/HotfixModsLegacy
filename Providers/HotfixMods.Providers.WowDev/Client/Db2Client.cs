/*
 * This client is getting data from the <a href="https://github.com/wowdev/WoWDBDefs">WoWDBDefs repository in GitHub, by wowdev</a>.
 * Code is mostly from <a href="https://github.com/wowdev/DBCD>WoWDev's DBCD repository</a>.
 * Helper methods are based on <a href="https://github.com/MaxtorCoder/Wow.DB2DefinitionDumper>MaxtorCoder's Wow.DB2DefinitionDumper</a>.
 */

using DBDefsLib;
using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;

namespace HotfixMods.Providers.WowDev.Client
{
    public partial class Db2Client : IClientDbProvider, IClientDbDefinitionProvider
    {
        HttpClient _httpClient;

        public string Build { get; set; }

        public Db2Client(string build)
        {
            Build = build;
            _httpClient = new();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "HotfixMods");
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer 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"); // useless scope with limited lifetime, but try not to deploy this... EDIT: shit
        }

        public async Task<IEnumerable<DbRow>> GetAsync(string location, string db2Name, DbRowDefinition dbRowDefinition, params DbParameter[] parameters)
        {
            if (!await Db2ExistsAsync(location, db2Name))
                return new List<DbRow>();
            return await ReadDb2FileAsync(location, db2Name, Build, parameters, false);
        }

        public async Task<DbRow?> GetSingleAsync(string location, string db2Name, DbRowDefinition dbRowDefinition, params DbParameter[] parameters)
        {
            if (!await Db2ExistsAsync(location, db2Name))
                return null;
            return (await ReadDb2FileAsync(location, db2Name, Build, parameters, true)).FirstOrDefault();
        }

        public async Task<DbRowDefinition?> GetDefinitionAsync(string location, string db2Name)
        {
            if (string.IsNullOrWhiteSpace(db2Name))
            {
                throw new Exception("Db2 Name and Build must have a value.");
            }
            db2Name = TrimDb2Name(db2Name);

            var (databaseDefinitions, versionDefinition) = await GetDbDefinitionAndVersionDefinitionsByDb2Name(db2Name, Build);
            var dbRowDefinition = new DbRowDefinition(db2Name);
            foreach (var fieldDefinition in versionDefinition.definitions)
            {
                var columnDefinition = databaseDefinitions.columnDefinitions[fieldDefinition.name];
                var name = fieldDefinition.name.Replace("_lang", "");
                var type = FieldDefinitionToType(fieldDefinition, columnDefinition);

                if (fieldDefinition.arrLength != 0)
                {
                    for (int i = 0; i<fieldDefinition.arrLength; i++)
                    {
                        var arrayColName = $"{name}{i+1}";
                        dbRowDefinition.ColumnDefinitions.Add(new()
                        {
                            Name = arrayColName,
                            Type = type
                        });
                    }
                }
                else
                {
                    dbRowDefinition.ColumnDefinitions.Add(new()
                    {
                        Name = name,
                        Type = type
                    });
                }
            }
            dbRowDefinition.ColumnDefinitions.Add(new()
            {
                Name = "VerifiedBuild",
                Type = typeof(int)
            });
            return dbRowDefinition;
        }

        public async Task<IEnumerable<string>> GetDefinitionNamesAsync()
        {
            return await GetAllDefinitionsAsync();
        }

        public async Task<IEnumerable<string>> GetAvailableBuildsForDefinitionAsync(string db2Name)
        {
            return await GetBuildsAsync(db2Name);
        }

        public async Task<bool> Db2ExistsAsync(string location, string db2Name)
        {
            if(!location.EndsWith("\\"))
                location += "\\";
            return await Task.Run(() => File.Exists($"{location}{db2Name}.db2"));
        }
    }
}
