using DBDefsLib;
using DBCD.Providers;
using HotfixMods.Providers.Db2.WoWDev.Providers;
using HotfixMods.Core.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace HotfixMods.Providers.WowDev.Client
{
    public partial class Db2Client
    {
        readonly string defUrl = @"https://api.github.com/repos/wowdev/WoWDBDefs/git/trees/1488972b2d701cec80c9b71b63046e7694df6d0e";
        readonly string singleDefUrl = @"https://raw.githubusercontent.com/wowdev/WoWDBDefs/master/definitions/{0}.dbd";

        string TrimDb2Name(string db2Name)
        {
            db2Name = db2Name.Trim();
            if (db2Name.EndsWith(".db2") || db2Name.EndsWith(".dbd"))
                db2Name = db2Name.Substring(0, db2Name.Length - 4);
            return db2Name;
        }

        async Task<Structs.DBDefinition> GetDbDefinitionByDb2Name(string db2Name)
        {
            var stream = await GetDb2StreamByDb2Name(db2Name);
            return await GetDbDefinitionByDb2Stream(stream);
        }

        async Task<Structs.DBDefinition> GetDbDefinitionByDb2Stream(Stream db2Stream)
        {
            var dbdReader = new DBDReader();
            return dbdReader.Read(db2Stream);
        }

        async Task<(Structs.DBDefinition, Structs.VersionDefinitions)> GetDbDefinitionAndVersionDefinitionsByDb2Stream(Stream db2Stream, string build)
        {
            var databaseDefinitions = await GetDbDefinitionByDb2Stream(db2Stream);
            var dbBuild = new Build(build);

            if (!Utils.GetVersionDefinitionByBuild(databaseDefinitions, dbBuild, out var versionToUse) || null == versionToUse)
                throw new($"No definition found from db2 stream and build {build}.");

            return (databaseDefinitions, versionToUse.Value);
        }

        async Task<(Structs.DBDefinition, Structs.VersionDefinitions)> GetDbDefinitionAndVersionDefinitionsByDb2Name(string db2Name, string build)
        {
            var databaseDefinitions = await GetDbDefinitionByDb2Name(db2Name);
            var dbBuild = new Build(build);

            if (!Utils.GetVersionDefinitionByBuild(databaseDefinitions, dbBuild, out var versionToUse) || null == versionToUse)
                throw new($"No definition found for DB2: {db2Name} with build: {build}.");

            return (databaseDefinitions, versionToUse.Value);
        }

        async Task<Stream> GetDb2StreamByDb2Name(string db2Name)
        {
            var url = string.Format(singleDefUrl, db2Name);
            var data = await _httpClient.GetAsync(url);
            if (data.IsSuccessStatusCode)
            {
                return await data.Content.ReadAsStreamAsync();
            }
            throw new Exception($"Unable to load definition columns from URL {url}.");
        }

        async Task<Stream> GetDb2Stream(string db2Name)
        {
            var url = string.Format(singleDefUrl, db2Name);
            var data = await _httpClient.GetAsync(url);
            if (data.IsSuccessStatusCode)
            {
                return await data.Content.ReadAsStreamAsync();
            }
            throw new Exception($"Unable to load definition columns from URL {url}.");
        }

        Type FieldDefinitionToType(Structs.Definition field, Structs.ColumnDefinition column)
        {
            switch (column.type)
            {
                case "int":
                    {
                        return field.size switch
                        {
                            8 => field.isSigned ? typeof(sbyte) : typeof(byte),
                            16 => field.isSigned ? typeof(short) : typeof(ushort),
                            32 => field.isSigned ? typeof(int) : typeof(uint),
                            64 => field.isSigned ? typeof(long) : typeof(ulong),
                            _ => throw new Exception(@"Invalid int size {field.size}")
                        };
                    }
                case "string":
                case "locstring":
                    return typeof(string);
                case "float":
                    return typeof(decimal);
                default:
                    throw new ArgumentException($"Unable to construct C# type from {column.type}");
            }
        }

        async Task<IEnumerable<DbRow>> ReadDb2FileAsync(string location, string db2Name, string build, DbParameter[] parameters, bool firstOnly)
        {
            var results = new List<DbRow>();
            var streamForStructs = await GetDb2Stream(db2Name);
            var streamForProvider = new MemoryStream();

            // Need to make 2 because the DBCD closes the one it uses.
            streamForStructs.CopyTo(streamForProvider);
            streamForStructs.Position = 0;
            streamForProvider.Position = 0;

            var (dbDef, versionDef) = await GetDbDefinitionAndVersionDefinitionsByDb2Stream(streamForStructs, build);
            var dbcProvider = new DbcProvider(location);
            var dbdProvider = new DbDefProvider(streamForProvider);
            var dbcd = new DBCD.DBCD(dbcProvider, dbdProvider);
            var db2Results = dbcd.Load($"{db2Name}", build);

            foreach (var db2Result in db2Results.Values)
            {
                var rowResult = new DbRow();

                for (int i = 0; i<versionDef.definitions.Length; i++)
                {
                    var fieldDef = versionDef.definitions[i];
                    var columnDefinition = dbDef.columnDefinitions[fieldDef.name];
                    var name = fieldDef.name;
                    var type = FieldDefinitionToType(fieldDef, columnDefinition);

                    if (fieldDef.arrLength != 0)
                    {
                        var values = db2Result.Field<object>(name) as Array;

                        for (int j = 0; j<fieldDef.arrLength; j++)
                        {
                            var arrayColName = $"{name}{j + 1}";
                            var value = values?.GetValue(j);
                            rowResult.Columns.Add(new()
                            {
                                Name = arrayColName,
                                Type = type,
                                Value = value!
                            });
                        }
                    }
                    else
                    {
                        var value = db2Result.Field<object>(name);
                        name = name.Replace("_lang", "");
                        rowResult.Columns.Add(new()
                        {
                            Name = name,
                            Type = type,
                            Value = value
                        });
                    }
                }

                if (!rowResult.Columns.Any(c => c.Name == "VerifiedBuild"))
                    rowResult.Columns.Add(new()
                    {
                        Name = "VerifiedBuild",
                        Type = typeof(int),
                        Value = 0
                    });

                if(MeetsDbParameterRequirements(parameters, rowResult))
                {
                    results.Add(rowResult);
                    if (firstOnly)
                    {
                        return results;
                    }
                }


            }

            return results;
        }

        // This will check DbParameters as dbParameter1 AND dbParameter2. OR is not supported yet.
        bool MeetsDbParameterRequirements(DbParameter[] dbParameters, DbRow dbRow)
        {
            foreach(var dbParameter in dbParameters)
            {
                var column = dbRow.Columns.Where(c => c.Name == dbParameter.Property).FirstOrDefault();
                if (null == column)
                    return false;

                if(decimal.TryParse(column.Value.ToString(), out var numericValue))
                {
                    var numericParameter = decimal.Parse(dbParameter.Value.ToString());

                    bool match = dbParameter.Operator switch
                    {
                        DbParameter.DbOperator.EQ => numericValue == numericParameter,
                        DbParameter.DbOperator.LT => numericValue < numericParameter,
                        DbParameter.DbOperator.LTE => numericValue <= numericParameter,
                        DbParameter.DbOperator.GT => numericValue > numericParameter,
                        DbParameter.DbOperator.GTE => numericValue >= numericParameter,
                        _ => throw new NotImplementedException()
                    };

                    // Return false as soon as it occurs, or continue to next parameter if match is true.
                    if (!match)
                        return false;
                }
                else
                {
                    var stringValue = column.Value.ToString();
                    var stringParameter = dbParameter.Value.ToString();
                    bool match = dbParameter.Operator switch
                    {
                        DbParameter.DbOperator.EQ => stringValue.Equals(stringParameter, StringComparison.InvariantCultureIgnoreCase),
                        DbParameter.DbOperator.CONTAINS => stringValue.Contains(stringParameter, StringComparison.InvariantCultureIgnoreCase),
                        _ => throw new NotImplementedException()
                    };

                    // Return false as soon as it occurs, or continue to next parameter if match is true.
                    if (!match)
                        return false;
                }
            }
            return true;
        }

        async Task<IEnumerable<string>> GetAllDefinitionsAsync()
        {
            var data = await _httpClient.GetAsync(defUrl);
            if (data.IsSuccessStatusCode)
            {
                var content = await data.Content.ReadAsStreamAsync();
                var jsonData = await JsonSerializer.DeserializeAsync<JsonElement>(content);
                var results = new List<string>();
                foreach (var jObject in jsonData.GetProperty("tree").EnumerateArray())
                {
                    var definition = jObject.GetProperty("path").ToString();
                    results.Add(definition.Replace(".dbd", ""));
                }
                return results;
            }
            throw new Exception($"Unable to load definitions from URL {defUrl}");
        }

        async Task<IEnumerable<string>> GetBuildsAsync(string db2Name)
        {
            if (string.IsNullOrWhiteSpace(db2Name))
            {
                throw new Exception("Db2 Name and Build must have a value.");
            }
            db2Name = TrimDb2Name(db2Name);

            var databaseDefinitions = await GetDbDefinitionByDb2Name(db2Name);
            var results = new List<string>();
            foreach (var definition in databaseDefinitions.versionDefinitions)
            {
                results.AddRange(definition.builds.Select(b => b.ToString()));
            }
            results.Reverse();
            return results;
        }
    }
}
