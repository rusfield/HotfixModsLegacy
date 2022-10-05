using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HotfixMods.Providers.MySql.MySqlConnector.Client
{
    public partial class MySqlClient
    {
        MySqlConnection _mySqlConnection;

        public MySqlClient(string server, string port, string user, string password)
        {
            _mySqlConnection = new MySqlConnection($"Server={server}; Port={port}; Uid={user}; Pwd={password};");
        }

        public async Task<IEnumerable<IDictionary<string, object>>> GetAsync(string schemaName, string tableName, IDictionary<string, Type> definitions, string? whereClause = null)
        {
            ValidateInput(tableName, whereClause);

            if (definitions.Count == 0)
            {
                throw new Exception("Missing definitions.");
            }

            var results = new List<Dictionary<string, object>>();

            string columns = "";
            foreach (var definition in definitions)
            {
                ValidateInput(definition.Key);
                columns += $"{definition.Key},";
            }
            columns = columns.Remove(columns.Length - 1);

            var query = $"SELECT {columns} FROM {schemaName}.{tableName}";
            query += string.IsNullOrWhiteSpace(whereClause) ? ";" : $" WHERE {whereClause};";

            using var command = new MySqlCommand(query, _mySqlConnection);
            using var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                var result = new Dictionary<string, object>();
                for (int i = 0; i < definitions.Count; i++)
                {
                    string fieldName = definitions.ElementAt(i).Key;
                    object fieldType = definitions.ElementAt(i).Value.ToString() switch
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
                    result.Add(fieldName, fieldType);
                }
                results.Add(result);
            }

            return results;
        }

        public async Task AddOrUpdateAsync(string schemaName, string tableName, IDictionary<string, object> data)
        {
            ValidateInput(schemaName, tableName);

            if (data.Count == 0)
            {
                throw new Exception("Nothing to add or update.");
            }

            using var cmd = new MySqlCommand(_mySqlConnection, null);
            string columns = "";
            string values = "";
            for (int i = 0; i < data.Count; i++)
            {
                columns += $"{data.ElementAt(i).Key},";
                values += $"@{data.ElementAt(i).Key},";
                cmd.Parameters.AddWithValue($"{data.ElementAt(i).Key}", data.ElementAt(i).Value);
            }

            string query = $"REPLACE INTO {schemaName}.{tableName} ({columns}) VALUES({values});";
            cmd.CommandText = query;
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(string schemaName, string tableName, string whereClause)
        {
            ValidateInput(schemaName, tableName, whereClause);
            if (string.IsNullOrWhiteSpace(tableName) || string.IsNullOrWhiteSpace(whereClause))
                throw new Exception("Parameters can not be null.");

            var query = $"DELETE FROM {schemaName}.{tableName} WHERE {whereClause};";
            using var cmd = new MySqlCommand(query, _mySqlConnection);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task CreateTableIfNotExistAsync(string schemaName, string tableName, IDictionary<string, Type> definitions)
        {
            ValidateInput(schemaName, tableName);
            string columns = "";
            for (int i = 0; i < definitions.Count; i++)
            {
                var fieldName = definitions.ElementAt(i).Key;
                var fieldType = definitions.ElementAt(i).Value.ToString() switch
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
                    _ => throw new Exception($"{definitions.ElementAt(i).Value} not implemented.")
                };
                ValidateInput(fieldName);
                columns += $"{fieldName} {fieldType},";
            }

            string createSchemaQuery = $"CREATE SCHEMA IF NOT EXISTS {schemaName};";
            string createTableQuery = $"CREATE TABLE IF NOT EXISTS {schemaName}.{tableName} ({columns} primary key(ID, VerifiedBuild));";
            using var cmd = new MySqlCommand(createSchemaQuery + createTableQuery, _mySqlConnection);
            await _mySqlConnection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await _mySqlConnection.CloseAsync();
        }
    }
}
