using DBDefsLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HotfixMods.Providers.DbDef.WoWDev.Client
{
    public partial class DbDefClient
    {
        readonly string defUrl = @"https://api.github.com/repos/wowdev/WoWDBDefs/git/trees/1488972b2d701cec80c9b71b63046e7694df6d0e";
        readonly string singleDefUrl = @"https://raw.githubusercontent.com/wowdev/WoWDBDefs/master/definitions/{0}.dbd";

        async Task<IEnumerable<string>> GetDefinitionsAsync()
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
            var (databaseDefinitions, versionDefinition) = await GetStructsAsync(db2Name, build);
            var results = new Dictionary<string, Type>();
            foreach (var fieldDefinition in versionDefinition.definitions)
            {
                var columnDefinition = databaseDefinitions.columnDefinitions[fieldDefinition.name];
                var name = fieldDefinition.name;
                var type = FieldDefinitionToType(fieldDefinition, columnDefinition);

                if (fieldDefinition.arrLength != 0)
                {
                    for (int i = 0; i<fieldDefinition.arrLength; i++)
                    {
                        name = $"{name}{i+1}";
                        results.Add(name, type);
                    }
                }
                else
                {
                    results.Add(name, type);
                }
            }
            return results;
        }

        async Task<IEnumerable<string>> GetBuildsAsync(string db2Name)
        {
            var databaseDefinitions = await GetStructsAsync(db2Name);
            var results = new List<string>();
            foreach(var definition in databaseDefinitions.versionDefinitions)
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
            var url = string.Format(singleDefUrl, db2Name);
            var data = await _httpClient.GetAsync(url);
            if (data.IsSuccessStatusCode)
            {
                var stream = await data.Content.ReadAsStreamAsync();
                var dbdReader = new DBDReader();
                return dbdReader.Read(stream);
            }
            throw new Exception($"Unable to load definition columns from URL {url}.");
        }

        async Task<(Structs.DBDefinition, Structs.VersionDefinitions)> GetStructsAsync(string db2Name, string build)
        {
            var databaseDefinitions = await GetStructsAsync(db2Name);
            var dbBuild = new Build(build);

            if (!Utils.GetVersionDefinitionByBuild(databaseDefinitions, dbBuild, out var versionToUse) || null == versionToUse)
                throw new($"No definition found for DB2: {db2Name} with build: {build}.");

            return (databaseDefinitions, versionToUse.Value);
        }
    }

}