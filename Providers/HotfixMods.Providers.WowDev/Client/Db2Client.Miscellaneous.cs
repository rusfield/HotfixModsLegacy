using DBDefsLib;
using HotfixMods.Providers.Extensions;
using HotfixMods.Providers.Models;
using HotfixMods.Providers.WowDev.Libs.Internal;

namespace HotfixMods.Providers.WowDev.Client
{
    public partial class Db2Client
    {
        async Task<PagedDbResult> ReadDb2FileAsync(string db2Name, DbRowDefinition definition, int pageIndex, int pageSize, DbParameter[] parameters)
        {
            PagedDbResult pagedResult = new();

            await Task.Run(() =>
            {
                var dbcProvider = new DbcProvider(Db2Folder);
                var dbdProvider = new DbdProvider(GetDbdStream(db2Name));

                var dbcd = new DBCD.DBCD(dbcProvider, dbdProvider);
                var db2Results = dbcd.Load($"{db2Name}", Build);
                var filteredResults = db2Results.Values.WhereDbParameters(parameters);

                pagedResult = new PagedDbResult(pageIndex, pageSize, (ulong)filteredResults.Count());

                foreach (var db2Value in filteredResults.Skip(pageIndex * pageSize).Take(pageSize))
                {
                    var row = new DbRow(db2Name);
                    foreach (var columnDefinition in definition.ColumnDefinitions)
                    {
                        row.Columns.Add(new()
                        {
                            Definition = columnDefinition,
                            Value = db2Value
                        });
                    }
                    pagedResult.Rows.Add(row);
                }
            });

            return pagedResult;
        }

        async Task<DbRowDefinition?> GetDbDefinitionByDb2Name(string db2Name)
        {
            DbRowDefinition? dbRowDefinition = null;
            await Task.Run(() =>
            {
                var dbdReader = new DBDReader();
                var dbDef = dbdReader.Read(GetDbdStream(db2Name));
                var dbBuild = new Build(Build);

                if (Utils.GetVersionDefinitionByBuild(dbDef, dbBuild, out var versionToUse) && null != versionToUse)
                {
                    dbRowDefinition = new DbRowDefinition(db2Name);
                    foreach (var fieldDefinition in versionToUse.Value.definitions)
                    {
                        var columnDefinition = dbDef.columnDefinitions[fieldDefinition.name];
                        var definitionName = fieldDefinition.name.Replace("_lang", "");

                        // Assume name does not start with underscore or contains two underscores after one another, and remove underscore and set uppercase
                        // Exception is for properties named Field_{patch}
                        string name = "";
                        if (definitionName.StartsWith("Field"))
                        {
                            name = definitionName;
                        }
                        else
                        {
                            bool isUnderscore = false;

                            foreach (var c in definitionName)
                            {
                                if (isUnderscore)
                                {
                                    // previous was underscore
                                    name += char.ToUpper(c);
                                    isUnderscore = false;
                                }
                                else
                                {
                                    isUnderscore = c == '_';
                                    if (!isUnderscore)
                                        name += c;
                                }
                            }
                        }


                        var type = FieldDefinitionToType(fieldDefinition, columnDefinition);

                        if (fieldDefinition.arrLength != 0)
                        {
                            for (int i = 0; i < fieldDefinition.arrLength; i++)
                            {
                                var arrayColName = $"{name}{i}";
                                dbRowDefinition.ColumnDefinitions.Add(new()
                                {
                                    Name = arrayColName,
                                    Type = type,
                                    IsIndex = fieldDefinition.isID,
                                    IsParentIndex = fieldDefinition.isRelation,
                                    ReferenceDb2 = columnDefinition.foreignTable,
                                    ReferenceDb2Field = columnDefinition.foreignColumn,
                                    IsLocalized = columnDefinition.type == "locstring"
                                });
                            }
                        }
                        else
                        {
                            dbRowDefinition.ColumnDefinitions.Add(new()
                            {
                                Name = name,
                                Type = type,
                                IsIndex = fieldDefinition.isID,
                                IsParentIndex = fieldDefinition.isRelation,
                                ReferenceDb2 = columnDefinition.foreignTable,
                                ReferenceDb2Field = columnDefinition.foreignColumn,
                                IsLocalized = columnDefinition.type == "locstring"
                            });
                        }
                    }
                }
                else
                {
                    throw new($"No definition found for DB2: {db2Name} with build: {Build}.");
                }
            });
            return dbRowDefinition;
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

        Stream GetDbdStream(string db2Name)
        {
            db2Name += ".dbd";
            var filePath = Path.Combine(DbdFolder, db2Name);
            return new FileStream(filePath, FileMode.Open, FileAccess.Read);
        }
    }
}
