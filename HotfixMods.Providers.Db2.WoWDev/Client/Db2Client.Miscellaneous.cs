using DBCD.Providers;
using DBDefsLib;
using HotfixMods.Providers.Db2.WoWDev.Providers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static DBDefsLib.Structs;

namespace HotfixMods.Providers.Db2.WoWDev.Client
{
    public partial class Db2Client
    {
        readonly string defUrl = @"https://api.github.com/repos/wowdev/WoWDBDefs/git/trees/1488972b2d701cec80c9b71b63046e7694df6d0e";
        readonly string singleDefUrl = @"https://raw.githubusercontent.com/wowdev/WoWDBDefs/master/definitions/{0}.dbd";

        async Task<IEnumerable<IDictionary<string, KeyValuePair<Type, object?>>>> ReadDb2Async(string db2Path, string db2Name, string build)
        {
            var results = new List<Dictionary<string, KeyValuePair<Type, object?>>>();
            var streamForStructs = await GetDb2Stream(db2Name);
            var streamForProvider = new MemoryStream();

            // Need to make 2 because the DBCD closes the one it uses.
            streamForStructs.CopyTo(streamForProvider);
            streamForStructs.Position = 0;
            streamForProvider.Position = 0;

            var (dbDef, versionDef) = await GetStructsAsync(streamForStructs, build);
            var dbcProvider = new DbcProvider(db2Path);
            var dbdProvider = new DbDefProvider(streamForProvider);
            var dbcd = new DBCD.DBCD(dbcProvider, dbdProvider);
            var db2Results = dbcd.Load(db2Name, build);

            foreach (var db2Result in db2Results.Values)
            {
                var rowResult = new Dictionary<string, KeyValuePair<Type, object?>>();

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
                            rowResult.Add(arrayColName, new(type, values?.GetValue(j)));
                        }
                    }
                    else
                    {
                        var value = db2Result.Field<object>(name);
                        rowResult.Add(name.Replace("_lang", ""), new(type, value));
                    }
                }
                results.Add(rowResult);
            }

            return results;
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

        async Task<IDictionary<string, Type>> GetColumnsAsync(string db2Name, string build)
        {
            if (string.IsNullOrWhiteSpace(db2Name) || string.IsNullOrWhiteSpace(build))
            {
                throw new Exception("Db2 Name and Build must have a value.");
            }
            db2Name = TrimDb2Name(db2Name);

            var (databaseDefinitions, versionDefinition) = await GetStructsAsync(db2Name, build);
            var results = new Dictionary<string, Type>();
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
                        results.Add(arrayColName, type);
                    }
                }
                else
                {
                    results.Add(name, type);
                }
            }
            results.Add("VerifiedBuild", typeof(int));
            return results;
        }

        async Task<IEnumerable<string>> GetBuildsAsync(string db2Name)
        {
            if (string.IsNullOrWhiteSpace(db2Name))
            {
                throw new Exception("Db2 Name and Build must have a value.");
            }
            db2Name = TrimDb2Name(db2Name);

            var databaseDefinitions = await GetStructsAsync(db2Name);
            var results = new List<string>();
            foreach (var definition in databaseDefinitions.versionDefinitions)
            {
                results.AddRange(definition.builds.Select(b => b.ToString()));
            }
            results.Reverse();
            return results;
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

        async Task<Structs.DBDefinition> GetStructsAsync(string db2Name)
        {
            var stream = await GetDb2Stream(db2Name);
            return await GetStructsAsync(stream);
        }

        async Task<Structs.DBDefinition> GetStructsAsync(Stream db2Stream)
        {
            var dbdReader = new DBDReader();
            return dbdReader.Read(db2Stream);
        }

        async Task<(Structs.DBDefinition, Structs.VersionDefinitions)> GetStructsAsync(Stream db2Stream, string build)
        {
            var databaseDefinitions = await GetStructsAsync(db2Stream);
            var dbBuild = new Build(build);

            if (!Utils.GetVersionDefinitionByBuild(databaseDefinitions, dbBuild, out var versionToUse) || null == versionToUse)
                throw new($"No definition found from db2 stream and build {build}.");

            return (databaseDefinitions, versionToUse.Value);
        }

        async Task<(Structs.DBDefinition, Structs.VersionDefinitions)> GetStructsAsync(string db2Name, string build)
        {
            var databaseDefinitions = await GetStructsAsync(db2Name);
            var dbBuild = new Build(build);

            if (!Utils.GetVersionDefinitionByBuild(databaseDefinitions, dbBuild, out var versionToUse) || null == versionToUse)
                throw new($"No definition found for DB2: {db2Name} with build: {build}.");

            return (databaseDefinitions, versionToUse.Value);
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

        string TrimDb2Name(string db2Name)
        {
            db2Name = db2Name.Trim();
            if (db2Name.EndsWith(".db2") || db2Name.EndsWith(".dbd"))
                db2Name = db2Name.Substring(0, db2Name.Length - 4);
            return db2Name;
        }
    }

}