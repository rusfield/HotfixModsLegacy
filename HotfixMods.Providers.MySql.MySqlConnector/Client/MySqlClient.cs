using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Providers.MySql.MySqlConnector.Client
{
    public class MySqlClient
    {
        string _server;
        string _user;
        string _password;
        string _worldSchemaName;
        string _charactersSchemaName;
        string _hotfixesSchemaName;

        MySqlConnection _mySqlConnection;

        public MySqlClient(string server, string user, string password, string worldSchemaName, string charactersSchemaName, string hotfixesSchemaName)
        {
            _server = server;
            _user = user;
            _password = password;
            _worldSchemaName = worldSchemaName;
            _charactersSchemaName = charactersSchemaName;
            _hotfixesSchemaName = hotfixesSchemaName;
        }

        public async Task<Dictionary<string, object>> RawGetQueryAsync(Dictionary<string, Type> definitions, string tableName)
        {
            var results = new Dictionary<string, object>();
            var query = $"SELECT ";
            foreach (var definition in definitions)
                query += $"{definitions},";
            query = query.Remove(query.Length - 1, 1);
            query += $" FROM {tableName}";

            using var command = new MySqlCommand(query, _mySqlConnection);
            using var reader = command.ExecuteReader();

            for (int i = 0; i<definitions.Count; i++)
            {
                string fieldName = definitions.ElementAt(i).Key;
                object fieldProperty = definitions.ElementAt(i).Value.ToString() switch
                {
                    "System.Int8" => reader.GetSByte(i),
                    "System.Int16" => reader.GetInt16(i),
                    "System.Int32" => reader.GetInt32(i),
                    "System.Int64" => reader.GetInt64(i),
                    "System.UInt8" => reader.GetByte(i),
                    "System.UInt16" => reader.GetUInt16(i),
                    "System.UInt32" => reader.GetUInt32(i),
                    "System.UInt64" => reader.GetUInt64(i),
                    "System.String" => reader.GetString(i),
                    "System.Decimal" => (decimal)reader.GetFloat(i),
                    _ => throw new Exception("Not implemented")
                };
                results.Add(fieldName, fieldProperty);
            }
            return results;
        }
    }
}
