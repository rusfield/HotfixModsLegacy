using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using MySqlConnector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Providers.MySqlConnector.Client
{
    public partial class MySqlClient : IServerDbProvider, IServerDbDefinitionProvider, IClientDbProvider, IClientDbDefinitionProvider
    {
        MySqlConnection _mySqlConnection;
        public int AddBatchSize { get; set; } = 1000; // Run this in MySql and then restart the server:      SET GLOBAL max_allowed_packet=1073741824; 

        public MySqlClient(string server, string port, string user, string password)
        {
            _mySqlConnection = new MySqlConnection($"Server={server}; Port={port}; Uid={user}; Pwd={password};default command timeout=3600;");
        }

        public async Task AddOrUpdateAsync(string schemaName, string tableName, params DbRow[] dbRows)
        {
            if (!dbRows.Any())
            {
                throw new Exception("Nothing to add or update.");
            }

            var queries = new List<string>();

            await _mySqlConnection.OpenAsync();
            int batchCount = 1;
            using (var cmd = new MySqlCommand(_mySqlConnection, null))
            {
                foreach (var dbColumn in dbRows)
                {
                    string columns = string.Join(",", dbColumn.Columns.Select(d => d.Name));
                    string values = string.Join(",", dbColumn.Columns.Select(d => $"'{MySqlHelper.EscapeString(d.Value.ToString()!)}'"));

                    queries.Add($"REPLACE INTO {schemaName}.{tableName} ({columns}) VALUES({values});");

                    if (queries.Count == AddBatchSize)
                    {
                        Console.WriteLine($"Inserting {AddBatchSize * batchCount++} queries");
                        cmd.CommandText = string.Join("", queries);
                        await cmd.ExecuteNonQueryAsync();
                        queries = new();
                    }
                }

                if(queries.Count > 0)
                {
                    Console.WriteLine($"Inserting {AddBatchSize * batchCount++} queries");
                    cmd.CommandText = string.Join("", queries);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            await _mySqlConnection.CloseAsync();
        }

        public async Task DeleteAsync(string schemaName, string tableName, params DbParameter[] parameters)
        {
            if (parameters.Length == 0)
                throw new Exception("Parameter required for delete.");

            var query = $"DELETE FROM {schemaName}.{tableName} {DbParameterToWhereClause(parameters)};";
            using var cmd = new MySqlCommand(query, _mySqlConnection);

            await _mySqlConnection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await _mySqlConnection.CloseAsync();
        }

        public async Task<IEnumerable<DbRow>> GetAsync(string schemaName, string tableName, DbRowDefinition dbRowDefinition, params DbParameter[] parameters)
        {
            if (null == dbRowDefinition || !dbRowDefinition.ColumnDefinitions.Any())
            {
                throw new Exception("Missing definitions.");
            }

            var results = new List<DbRow>();

            if (!await TableExistsAsync(schemaName, tableName))
            {
                return results;
            }

            string columns = string.Join(",", dbRowDefinition.ColumnDefinitions);
            var query = $"SELECT {columns} FROM {schemaName}.{tableName} {DbParameterToWhereClause(parameters)};";

            using var command = new MySqlCommand(query, _mySqlConnection);

            await _mySqlConnection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                var dbRow = new DbRow(tableName);
                for (int i = 0; i < dbRowDefinition.ColumnDefinitions.Count(); i++)
                {
                    var fieldName = dbRowDefinition.ColumnDefinitions.ElementAt(i).Name;
                    var fieldType = dbRowDefinition.ColumnDefinitions.ElementAt(i).Type;
                    object fieldValue = fieldType.ToString() switch
                    {
                        "System.SByte" => reader.GetSByte(i),
                        "System.Int16" => reader.GetInt16(i),
                        "System.Int32" => reader.GetInt32(i),
                        "System.Int64" => reader.GetInt64(i),
                        "System.Byte" => reader.GetByte(i),
                        "System.UInt16" => reader.GetUInt16(i),
                        "System.UInt32" => reader.GetUInt32(i),
                        "System.UInt64" => reader.GetUInt64(i),
                        "System.String" => reader.GetString(i),
                        "System.Decimal" => (decimal)reader.GetFloat(i),
                        _ => throw new Exception($"{dbRowDefinition.ColumnDefinitions.ElementAt(i).Type} not implemented.")
                    };
                    dbRow.Columns.Add(new()
                    {
                        Name = fieldName,
                        Type = fieldType,
                        Value = fieldValue
                    });
                }
                results.Add(dbRow);
            }
            await _mySqlConnection.CloseAsync();

            return results;
        }

        public async Task<DbRow?> GetSingleAsync(string schemaName, string tableName, DbRowDefinition dbRowDefinition, params DbParameter[] parameters)
        {
            return (await GetAsync(schemaName, tableName, dbRowDefinition, parameters)).FirstOrDefault();
        }

        public async Task<DbRowDefinition?> GetDefinitionAsync(string schemaName, string tableName)
        {
            if (!await TableExistsAsync(schemaName, tableName))
            {
                return null;
            }

            var query = $"DESCRIBE {schemaName}.{tableName};";
            var dbRowDefinition = new DbRowDefinition(tableName);

            using var command = new MySqlCommand(query, _mySqlConnection);

            await _mySqlConnection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                dbRowDefinition.ColumnDefinitions.Add(new()
                {
                    Name = reader.GetString(0),
                    Type = MySqlDataTypeToCSharpType(reader.GetString(1))
                });
            }
            await _mySqlConnection.CloseAsync();
            return dbRowDefinition;
        }

        public async Task CreateTableIfNotExistsAsync(string schemaName, string tableName, DbRowDefinition dbRowDefinition)
        {
            string columns = "";
            for (int i = 0; i < dbRowDefinition.ColumnDefinitions.Count(); i++)
            {
                var fieldName = dbRowDefinition.ColumnDefinitions.ElementAt(i).Name;
                var fieldType = CSharpTypeToMySqlDataType(dbRowDefinition.ColumnDefinitions.ElementAt(i).Type);
                columns += $"{fieldName} {fieldType},";
            }

            string createSchemaQuery = $"CREATE SCHEMA IF NOT EXISTS {schemaName};";
            string createTableQuery = $"CREATE TABLE IF NOT EXISTS {schemaName}.{tableName} ({columns} primary key(ID));";
            using var cmd = new MySqlCommand(createSchemaQuery + createTableQuery, _mySqlConnection);

            await _mySqlConnection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await _mySqlConnection.CloseAsync();
        }

        public async Task<bool> IsAvailableAsync()
        {
            try
            {
                await _mySqlConnection.OpenAsync();
                await _mySqlConnection.CloseAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SchemaExistsAsync(string schemaName)
        {
            try
            {
                await _mySqlConnection.OpenAsync();
                using var cmd = new MySqlCommand($"SELECT schema_name FROM information_schema.schemata WHERE schema_name = '{schemaName}';", _mySqlConnection);
                var reader = await cmd.ExecuteReaderAsync();
                bool exists = false;
                while (reader.Read())
                {
                    int count = reader.GetInt32(0);
                    exists = count > 0;
                }
                await _mySqlConnection.CloseAsync();
                return exists;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> TableExistsAsync(string schemaName, string tableName)
        {
            await _mySqlConnection.OpenAsync();
            using var cmd = new MySqlCommand($"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = '{schemaName}' AND table_name = '{tableName}';", _mySqlConnection);
            var reader = await cmd.ExecuteReaderAsync();
            bool exists = false;
            while (reader.Read())
            {
                int count = reader.GetInt32(0);
                exists = count > 0;
            }
            await _mySqlConnection.CloseAsync();
            return exists;
        }

        public async Task<int> GetNextIdAsync(string schemaName, string tableName, int minId, string idPropertyName = "id")
        {
            await _mySqlConnection.OpenAsync();
            using var cmd = new MySqlCommand($"SELECT {idPropertyName} FROM {schemaName}.{tableName} WHERE {idPropertyName} >= {minId} ORDER BY {idPropertyName} ASC;", _mySqlConnection);
            var reader = await cmd.ExecuteReaderAsync();
            var newId = minId;
            while (reader.Read())
            {
                var dbId = reader.GetInt32(0);
                if (newId != dbId)
                    break;

                newId++;
            }
            await _mySqlConnection.CloseAsync();
            return newId;
        }
    }
}
