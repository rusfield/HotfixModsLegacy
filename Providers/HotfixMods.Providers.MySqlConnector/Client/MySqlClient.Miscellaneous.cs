using HotfixMods.Core.Models;
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
            foreach(var dbParameter in dbParameters)
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
                "tinyint signed" => typeof(sbyte),
                "smallint signed" => typeof(short),
                "int" => typeof(int),
                "int signed" => typeof(int),
                "bigint signed" => typeof(long),
                "tinyint unsigned" => typeof(byte),
                "smallint unsigned" => typeof(ushort),
                "int unsigned" => typeof(uint),
                "bigint unsigned" => typeof(ulong),
                "text" => typeof(string),
                "varchar" => typeof(string),
                "nvarchar" => typeof(string),
                "float" => typeof(decimal),
                _ => throw new Exception($"{type} not implemented.")
            };
        }
    }
}
