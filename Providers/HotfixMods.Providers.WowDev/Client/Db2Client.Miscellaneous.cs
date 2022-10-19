using DBDefsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
