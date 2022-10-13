using HotfixMods.Core.Models.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HotfixMods.Providers.MySql.MySqlConnector.Client
{
    public partial class MySqlClient
    {
        object GetValueWithDefault(Type type, object value)
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

        IEnumerable<Db2ColumnDefinition> ObjectTypeToDb2ColumnDefinitions<T>()
            where T : new()
        {
            var result = new List<Db2ColumnDefinition>();
            foreach (var property in typeof(T).GetProperties())
            {
                result.Add(new Db2ColumnDefinition()
                {
                    Name = property.Name,
                    Type = property.PropertyType
                });
            }
            result.Reverse();
            return result;
        }

        IEnumerable<Db2Column>? ObjectToDb2Columns<T>(T? obj)
            where T : new()
        {
            if(null == default(T))
                return null;

            var result = new List<Db2Column>();
            foreach (var property in typeof(T).GetProperties())
            {
                result.Add(new Db2Column()
                {
                    Name = property.Name,
                    Type = property.PropertyType,
                    Value = property.GetValue(obj)!
                });
            }
            result.Reverse();
            return result;
        }

        T? Db2ColumnsToObject<T>(IEnumerable<Db2Column> db2Columns)
            where T : new()
        {
            if (null == db2Columns)
                return default(T);

            T result = new();
            foreach (var db2Column in db2Columns)
            {
                var existingProperty = result.GetType().GetProperty(db2Column.Name);
                if (existingProperty != null)
                    existingProperty.SetValue(result, db2Column.Value);
            }
            return result;
        }


        void ValidateInput(params string?[] inputs)
        {
            var regex = new Regex("^[A-Za-z0-9 ='_-]*$");
            foreach (var input in inputs)
            {
                if (!string.IsNullOrEmpty(input))
                {
                    if (!regex.Match(input).Success)
                        throw new Exception($"'{input}' is not a valid input.");
                }
            }
        }
    }
}
