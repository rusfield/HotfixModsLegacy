using DBDefsLib;
using HotfixMods.Providers.Models;
using System.Reflection;
using static DBDefsLib.Structs;

namespace HotfixMods.Providers.WowDev.Client
{
    public partial class Db2Client
    {
        async Task<DbRowDefinition> GetDbDefinitionByDb2Name(string db2Name)
        {
            db2Name += ".dbd";
            var filePath = Path.Combine(DbdFolder, db2Name);
            var dbdStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var dbdReader = new DBDReader();
            var dbDef = dbdReader.Read(dbdStream);

            var dbBuild = new Build(Build);

            if (Utils.GetVersionDefinitionByBuild(dbDef, dbBuild, out var versionToUse) || null == versionToUse)
            {
                var dbRowDefinition = new DbRowDefinition(db2Name);
                foreach (var fieldDefinition in versionToUse.Value.definitions)
                {
                    var columnDefinition = dbDef.columnDefinitions[fieldDefinition.name];
                    var definitionName = fieldDefinition.name.Replace("_lang", "");

                    // Remove underscore and set uppercase
                    // Assuming name does not start with underscore or contains two underscores after one another
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
                dbRowDefinition.ColumnDefinitions.Add(new()
                {
                    Name = "VerifiedBuild",
                    Type = typeof(int),
                    IsIndex = false,
                    IsParentIndex = false,
                    ReferenceDb2 = null,
                    ReferenceDb2Field = null,
                    IsLocalized = false
                });
                return dbRowDefinition;
            }
            else
            {
                throw new($"No definition found for DB2: {db2Name} with build: {Build}.");
            }
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
