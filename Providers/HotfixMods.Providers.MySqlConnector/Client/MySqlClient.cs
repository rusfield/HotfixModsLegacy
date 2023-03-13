using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using MySqlConnector;

namespace HotfixMods.Providers.MySqlConnector.Client
{
    public partial class MySqlClient : IServerDbProvider, IServerDbDefinitionProvider, IClientDbProvider, IClientDbDefinitionProvider
    {
        string _connectionString;
        public int AddBatchSize { get; set; } = 2000; // Run this in MySql and then restart the server:      SET GLOBAL max_allowed_packet=1073741824; 

        public MySqlClient(string server, string port, string user, string password)
        {
            _connectionString = $"Server={server}; Port={port}; Uid={user}; Pwd={password};default command timeout=3600;";
        }

        public async Task AddOrUpdateAsync(string schemaName, string tableName, params DbRow[] dbRows)
        {
            if (!dbRows.Any())
            {
                throw new Exception("Nothing to add or update.");
            }

            var queries = new List<string>();
            //var columns = string.Join(",", dbRows.First().Columns.Select(d => $"`{d.Name}`"));
            //var replaceQuery = $"REPLACE INTO {schemaName}.{tableName} ({columns}) VALUES ";
            var replaceQuery = $"REPLACE INTO {schemaName}.{tableName} VALUES ";

            using var mySqlConnection = new MySqlConnection(_connectionString);
            await mySqlConnection.OpenAsync();
            int batchCount = 1;
            using (var cmd = new MySqlCommand(mySqlConnection, null))
            {
                foreach (var dbColumn in dbRows)
                {

                    string values = string.Join(",", dbColumn.Columns.Select(d => $"'{MySqlHelper.EscapeString(d.Value.ToString()!)}'"));
                    queries.Add($"({values})");

                    if (queries.Count == AddBatchSize)
                    {
                        Console.WriteLine($"Inserting {AddBatchSize * batchCount++} queries");
                        cmd.CommandText = replaceQuery + string.Join(",", queries);
                        await cmd.ExecuteNonQueryAsync();
                        queries = new();
                    }
                }

                if (queries.Count > 0)
                {
                    Console.WriteLine($"Inserting {AddBatchSize * batchCount++} queries");
                    cmd.CommandText = replaceQuery + string.Join(",", queries);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            await mySqlConnection.CloseAsync();
        }

        public async Task DeleteAsync(string schemaName, string tableName, params DbParameter[] parameters)
        {
            if (parameters.Length == 0)
                throw new Exception("Parameter required for delete.");

            var query = $"DELETE FROM {schemaName}.{tableName} {DbParameterToWhereClause(parameters)};";
            using var mySqlConnection = new MySqlConnection(_connectionString);

            using var cmd = new MySqlCommand(query, mySqlConnection);

            await mySqlConnection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await mySqlConnection.CloseAsync();
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

            // EDIT: Try for a while to not select by name, as names changes much more often than types in DB2
            //string columns = string.Join(",", dbRowDefinition.ColumnDefinitions.Select(c => $"`{c.Name}`"));
            //var query = $"SELECT {columns} FROM {schemaName}.{tableName} {DbParameterToWhereClause(parameters)};";
            var query = $"SELECT * FROM {schemaName}.{tableName} {DbParameterToWhereClause(parameters)};";
            using var mySqlConnection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, mySqlConnection);

            await mySqlConnection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                var dbRow = new DbRow(tableName);
                for (int i = 0; i < dbRowDefinition.ColumnDefinitions.Count(); i++)
                {
                    var column = dbRowDefinition.ColumnDefinitions.ElementAt(i);
                    var fieldName = column.Name;

                    object propertyValue;

                    if (reader.IsDBNull(i))
                    {
                        propertyValue = column.GetServerType().ToString() switch
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
                            "System.Decimal" => Convert.ToDecimal(reader.GetFloat(i)),
                            _ => throw new Exception($"{dbRowDefinition.ColumnDefinitions.ElementAt(i).Type} not implemented.")
                        };
                    }
                    else
                    {
                        propertyValue = column.GetServerType().ToString() switch
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
                            "System.Decimal" => Convert.ToDecimal(reader.GetFloat(i)),
                            _ => throw new Exception($"{dbRowDefinition.ColumnDefinitions.ElementAt(i).Type} not implemented.")
                        };
                    }

                    if(column.GetServerType() != column.Type)
                    {
                        propertyValue = Convert.ChangeType(propertyValue, column.Type);
                    }

                    dbRow.Columns.Add(new()
                    {
                        Name = fieldName,
                        Type = column.Type,
                        Value = propertyValue,

                        // TODO?
                        IsLocalized = column.IsLocalized,
                        IsParentIndex = column.IsParentIndex,
                        ReferenceDb2 = column.ReferenceDb2,
                        ReferenceDb2Field = column.ReferenceDb2Field
                    });

                }
                results.Add(dbRow);
            }
            await mySqlConnection.CloseAsync();

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
            using var mySqlConnection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand(query, mySqlConnection);

            await mySqlConnection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                dbRowDefinition.ColumnDefinitions.Add(new()
                {
                    Name = reader.GetString(0),
                    Type = MySqlDataTypeToCSharpType(reader.GetString(1)),
                    IsIndex = reader.GetString(3) == "PRI",

                    // TODO?
                    IsLocalized = false,
                    IsParentIndex = false,
                    ReferenceDb2 = null,
                    ReferenceDb2Field = null
            });
            }
            await mySqlConnection.CloseAsync();
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
            using var mySqlConnection = new MySqlConnection(_connectionString);
            using var cmd = new MySqlCommand(createSchemaQuery + createTableQuery, mySqlConnection);

            await mySqlConnection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await mySqlConnection.CloseAsync();
        }

        public async Task<bool> IsAvailableAsync()
        {
            try
            {
                using var mySqlConnection = new MySqlConnection(_connectionString);
                await mySqlConnection.OpenAsync();
                await mySqlConnection.CloseAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> TableExistsAsync(string schemaName, string tableName)
        {
            using var mySqlConnection = new MySqlConnection(_connectionString);
            await mySqlConnection.OpenAsync();
            using var cmd = new MySqlCommand($"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = '{schemaName}' AND table_name = '{tableName}';", mySqlConnection);
            var reader = await cmd.ExecuteReaderAsync();
            bool exists = false;
            while (reader.Read())
            {
                int count = reader.GetInt32(0);
                exists = count > 0;
            }
            await mySqlConnection.CloseAsync();
            return exists;
        }

        public async Task<bool> SchemaExistsAsync(string schemaName)
        {
            using var mySqlConnection = new MySqlConnection(_connectionString);
            await mySqlConnection.OpenAsync();
            using var cmd = new MySqlCommand($"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = '{schemaName}';", mySqlConnection);
            var reader = await cmd.ExecuteReaderAsync();
            bool exists = false;
            while (reader.Read())
            {
                int count = reader.GetInt32(0);
                exists = count > 0;
            }
            await mySqlConnection.CloseAsync();
            return exists;
        }

        public async Task<int> GetHighestIdAsync(string schemaName, string tableName, int minId, int maxId, string idPropertyName = "id")
        {
            if (!await TableExistsAsync(schemaName, tableName))
            {
                return 0;
            }
            using var mySqlConnection = new MySqlConnection(_connectionString);
            await mySqlConnection.OpenAsync();
            using var cmd = new MySqlCommand($"SELECT max({idPropertyName}) FROM {schemaName}.{tableName} WHERE {idPropertyName} >= {minId} AND {idPropertyName} <= {maxId};", mySqlConnection);
            var reader = await cmd.ExecuteReaderAsync();
            var highestId = minId;
            while (reader.Read())
            {
                if (!reader.IsDBNull(0))
                    highestId = reader.GetInt32(0);
                break;
            }
            await mySqlConnection.CloseAsync();
            return highestId;
        }

        // From IClientDbProvider, will be for hotfixes only.
        public async Task<IEnumerable<string>> GetDefinitionNamesAsync()
        {
            var results = new List<string>();
            using var mySqlConnection = new MySqlConnection(_connectionString);
            await mySqlConnection.OpenAsync();
            // TODO: Hotfixes is hardcoded here. Get it from elsewhere
            using var cmd = new MySqlCommand($"SELECT TABLE_NAME FROM information_schema.tables WHERE table_schema = 'hotfixes' ORDER BY TABLE_NAME ASC;", mySqlConnection);
            var reader = await cmd.ExecuteReaderAsync();
            while (reader.Read())
            {
                var name = reader.GetString(0);
                // TODO: Normalize name
                results.Add(name);
            }
            await mySqlConnection.CloseAsync();
            return results;
        }

        // Used by IClientDbProvider interface
        public async Task<bool> Db2ExistsAsync(string schemaName, string tableName)
        {
            return await TableExistsAsync(schemaName, tableName);
        }
    }
}
