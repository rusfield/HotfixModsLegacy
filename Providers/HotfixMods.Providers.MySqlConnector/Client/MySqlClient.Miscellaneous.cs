using HotfixMods.Core.Models;
using System.Text;
using static HotfixMods.Core.Models.DbParameter;

namespace HotfixMods.Providers.MySqlConnector.Client
{
    public partial class MySqlClient
    {
        string DbParameterToWhereClause(params DbParameter[] dbParameters)
        {
            if (dbParameters.Length == 0)
                return "";

            var conditions = new List<string>();
            foreach (var dbParameter in dbParameters)
            {
                var queryOperator = dbParameter.Operator switch
                {
                    DbOperator.EQ => "= '{0}'",
                    DbOperator.GT => "> '{0}'",
                    DbOperator.GTE => ">= '{0}'",
                    DbOperator.LT => "< '{0}'",
                    DbOperator.LTE => "<= '{0}'",
                    DbOperator.CONTAINS => "LIKE '{0}'",
                    _ => throw new NotImplementedException()
                };
                conditions.Add($"{dbParameter.Property} {string.Format(queryOperator, dbParameter.Value)}");
            }
            return $"WHERE {string.Join(" AND ", conditions)}";
        }

        string CSharpTypeToMySqlDataType(Type type)
        {
            return type.ToString() switch
            {
                "System.SByte" => "tinyint signed",
                "System.Int16" => "smallint signed",
                "System.Int32" => "int signed",
                "System.Int64" => "bigint signed",
                "System.Byte" => "tinyint unsigned",
                "System.UInt16" => "smallint unsigned",
                "System.UInt32" => "int unsigned",
                "System.UInt64" => "bigint unsigned",
                "System.String" => "text",
                "System.Decimal" => "float",
                _ => throw new Exception($"{type} not implemented.")
            };
        }

        Type MySqlDataTypeToCSharpType(string type)
        {
            type = type.Split("(")[0]; // for varchar(100), etc.
            return type.ToString() switch
            {
                "tinyint" => typeof(sbyte),
                "tinyint signed" => typeof(sbyte),
                "tinyint unsigned" => typeof(byte),
                "smallint" => typeof(short),
                "smallint signed" => typeof(short),
                "smallint unsigned" => typeof(ushort),
                "int" => typeof(int),
                "int signed" => typeof(int),
                "int unsigned" => typeof(uint),
                "bigint" => typeof(long),
                "bigint signed" => typeof(long),
                "bigint unsigned" => typeof(ulong),
                "text" => typeof(string),
                "varchar" => typeof(string),
                "nvarchar" => typeof(string),
                "tinytext" => typeof(string),
                "mediumtext" => typeof(string),
                "longtext" => typeof(string),
                "float" => typeof(decimal),
                _ => throw new Exception($"{type} not implemented.")
            };
        }

        object GetDefaultValueForType(Type type)
        {
            return type.ToString() switch
            {
                "System.SByte" => (sbyte)0,
                "System.Int16" => (short)0,
                "System.Int32" => (int)0,
                "System.Int64" => (long)0,
                "System.Byte" => (byte)0,
                "System.UInt16" => (ushort)0,
                "System.UInt32" => (uint)0,
                "System.UInt64" => (ulong)0,
                "System.String" => "",
                "System.Decimal" => 0m,
                _ => throw new Exception($"{type} not implemented.")
            };
        }

        object GetValueForServerColumn(DbRow dbRow, string serverColumnName, Type serverColumnType)
        {
            var sourceColumn = FindSourceColumn(dbRow, serverColumnName);
            if (sourceColumn != null)
                return sourceColumn.Value;

            return GetDefaultValueForType(serverColumnType);
        }

        DbColumn? FindSourceColumn(DbRow dbRow, string serverColumnName)
        {
            if (TryGetSourceColumnAlias(dbRow.Db2Name, serverColumnName, out var sourceColumnAlias))
            {
                var aliasMatch = dbRow.Columns.FirstOrDefault(c => c.Name.Equals(sourceColumnAlias, StringComparison.InvariantCultureIgnoreCase));
                if (aliasMatch != null)
                    return aliasMatch;
            }

            var exactMatch = dbRow.Columns.FirstOrDefault(c => c.Name.Equals(serverColumnName, StringComparison.InvariantCultureIgnoreCase));
            if (exactMatch != null)
                return exactMatch;

            if (ServerColumnAliases.TryGetValue(serverColumnName, out var alias))
            {
                var aliasMatch = dbRow.Columns.FirstOrDefault(c => c.Name.Equals(alias, StringComparison.InvariantCultureIgnoreCase));
                if (aliasMatch != null)
                    return aliasMatch;
            }

            var normalizedServerName = NormalizeColumnName(serverColumnName);
            var normalizedMatch = dbRow.Columns.FirstOrDefault(c => NormalizeColumnName(c.Name) == normalizedServerName);
            if (normalizedMatch != null)
                return normalizedMatch;

            return null;
        }

        static bool TryGetSourceColumnAlias(string db2Name, string serverColumnName, out string sourceColumnName)
        {
            sourceColumnName = string.Empty;

            if (!TryGetTableSpecificAliases(db2Name, out var aliases))
                return false;

            return aliases.TryGetValue(serverColumnName, out sourceColumnName);
        }

        static bool TryGetServerColumnAlias(string db2Name, string sourceColumnName, out string serverColumnName)
        {
            serverColumnName = string.Empty;

            if (!TryGetTableSpecificAliases(db2Name, out var aliases))
                return false;

            foreach (var alias in aliases)
            {
                if (alias.Value.Equals(sourceColumnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    serverColumnName = alias.Key;
                    return true;
                }
            }

            return false;
        }

        static bool TryGetTableSpecificAliases(string db2Name, out Dictionary<string, string> aliases)
        {
            return TableSpecificColumnAliases.TryGetValue(NormalizeTableName(db2Name), out aliases);
        }

        int FindReaderOrdinal(System.Data.Common.DbDataReader reader, string db2Name, string sourceColumnName)
        {
            if (TryGetServerColumnAlias(db2Name, sourceColumnName, out var serverColumnAlias))
            {
                var aliasOrdinal = GetReaderOrdinal(reader, serverColumnAlias);
                if (aliasOrdinal >= 0)
                    return aliasOrdinal;
            }

            var exactOrdinal = GetReaderOrdinal(reader, sourceColumnName);
            if (exactOrdinal >= 0)
                return exactOrdinal;

            if (ServerColumnAliases.TryGetValue(sourceColumnName, out var serverAlias))
            {
                var aliasOrdinal = GetReaderOrdinal(reader, serverAlias);
                if (aliasOrdinal >= 0)
                    return aliasOrdinal;
            }

            var normalizedSourceName = NormalizeColumnName(sourceColumnName);
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (NormalizeColumnName(reader.GetName(i)) == normalizedSourceName)
                    return i;
            }

            return -1;
        }

        static int GetReaderOrdinal(System.Data.Common.DbDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return i;
            }

            return -1;
        }

        static string NormalizeColumnName(string name)
        {
            var builder = new StringBuilder(name.Length);
            foreach (var character in name)
            {
                if (character != '_')
                    builder.Append(char.ToLowerInvariant(character));
            }

            return builder.ToString();
        }

        static string NormalizeTableName(string name) => NormalizeColumnName(name);

        static readonly Dictionary<string, string> ServerColumnAliases = new(StringComparer.InvariantCultureIgnoreCase)
        {
            ["FactionRelated"] = "OppositeFactionItemID",
            ["DamageDamageType"] = "DamageType"
        };

        static Dictionary<string, string> BuildIndexedAliases(string prefix, int serverStart, int sourceStart, int count)
        {
            var aliases = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            for (int i = 0; i < count; i++)
            {
                aliases[$"{prefix}{serverStart + i}"] = $"{prefix}{sourceStart + i}";
            }

            return aliases;
        }

        static readonly Dictionary<string, Dictionary<string, string>> TableSpecificColumnAliases = new(StringComparer.InvariantCultureIgnoreCase)
        {
            [NormalizeTableName(nameof(Core.Models.Db2.ItemDisplayInfo))] = new(StringComparer.InvariantCultureIgnoreCase)
            {
                ["ModelResourcesID1"] = "ModelResourcesID0",
                ["ModelResourcesID2"] = "ModelResourcesID1",
                ["ModelMaterialResourcesID1"] = "ModelMaterialResourcesID0",
                ["ModelMaterialResourcesID2"] = "ModelMaterialResourcesID1",
                ["ModelType1"] = "ModelType0",
                ["ModelType2"] = "ModelType1",
                ["GeosetGroup1"] = "GeosetGroup0",
                ["GeosetGroup2"] = "GeosetGroup1",
                ["GeosetGroup3"] = "GeosetGroup2",
                ["GeosetGroup4"] = "GeosetGroup3",
                ["GeosetGroup5"] = "GeosetGroup4",
                ["GeosetGroup6"] = "GeosetGroup5",
                ["AttachmentGeosetGroup1"] = "AttachmentGeosetGroup0",
                ["AttachmentGeosetGroup2"] = "AttachmentGeosetGroup1",
                ["AttachmentGeosetGroup3"] = "AttachmentGeosetGroup2",
                ["AttachmentGeosetGroup4"] = "AttachmentGeosetGroup3",
                ["AttachmentGeosetGroup5"] = "AttachmentGeosetGroup4",
                ["AttachmentGeosetGroup6"] = "AttachmentGeosetGroup5",
                ["HelmetGeosetVis1"] = "HelmetGeosetVis0",
                ["HelmetGeosetVis2"] = "HelmetGeosetVis1"
            },
            [NormalizeTableName(nameof(Core.Models.Db2.SpellMisc))] = new(BuildIndexedAliases("Attributes", 1, 0, 17), StringComparer.InvariantCultureIgnoreCase),
            [NormalizeTableName(nameof(Core.Models.Db2.SpellEffect))] = new(StringComparer.InvariantCultureIgnoreCase)
            {
                ["EffectBasePoints"] = "EffectBasePointsF",
                ["TargetNodeGraph"] = "Node_Field120063534001",
                ["EffectMiscValue1"] = "EffectMiscValue0",
                ["EffectMiscValue2"] = "EffectMiscValue1",
                ["EffectRadiusIndex1"] = "EffectRadiusIndex0",
                ["EffectRadiusIndex2"] = "EffectRadiusIndex1",
                ["EffectSpellClassMask1"] = "EffectSpellClassMask0",
                ["EffectSpellClassMask2"] = "EffectSpellClassMask1",
                ["EffectSpellClassMask3"] = "EffectSpellClassMask2",
                ["EffectSpellClassMask4"] = "EffectSpellClassMask3",
                ["ImplicitTarget1"] = "ImplicitTarget0",
                ["ImplicitTarget2"] = "ImplicitTarget1"
            }
        };
    }
}
