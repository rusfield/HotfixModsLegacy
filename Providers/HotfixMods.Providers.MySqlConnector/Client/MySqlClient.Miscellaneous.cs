using HotfixMods.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HotfixMods.Core.Models.DbParameter;

namespace HotfixMods.Providers.MySqlConnector.Client
{
    public partial class MySqlClient
    {
        object ObjectToValue(Type type, object value)
        {
            return type.ToString() switch
            {
                "System.SByte" => Convert.ToSByte(value),
                "System.Int16" => Convert.ToInt16(value),
                "System.Int32" => Convert.ToInt32(value),
                "System.Int64" => Convert.ToInt64(value),
                "System.Byte" => Convert.ToByte(value),
                "System.UInt16" => Convert.ToUInt16(value),
                "System.UInt32" => Convert.ToUInt32(value),
                "System.UInt64" => Convert.ToUInt64(value),
                "System.String" => Convert.ToString(value)!,
                "System.Decimal" => Convert.ToDecimal(value),
                _ => throw new Exception($"{type} not implemented.")
            };
        }

        string DbParameterToWhereClause(params DbParameter[] dbParameters)
        {
            if (dbParameters.Length == 0)
                return "";

            var conditions = new List<string>();
            foreach(var dbParameter in dbParameters)
            {
                var queryOperator = dbParameter.Operator switch
                {
                    DbOperator.EQ => "=",
                    DbOperator.GT => ">",
                    DbOperator.GTE => ">=",
                    DbOperator.LT => "<",
                    DbOperator.LTE => "<=",
                    _ => throw new NotImplementedException()
                };
                conditions.Add($"{dbParameter.Property} {queryOperator} '{dbParameter.Value}'");
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
            return type.ToString() switch
            {
                "tinyint signed" => typeof(sbyte),
                "smallint signed" => typeof(short),
                "int signed" => typeof(int),
                "bigint signed" => typeof(long),
                "tinyint unsigned" => typeof(byte),
                "smallint unsigned" => typeof(ushort),
                "int unsigned" => typeof(uint),
                "bigint unsigned" => typeof(ulong),
                "text" => typeof(string),
                "float" => typeof(decimal),
                _ => throw new Exception($"{type} not implemented.")
            };
        }
    }
}
