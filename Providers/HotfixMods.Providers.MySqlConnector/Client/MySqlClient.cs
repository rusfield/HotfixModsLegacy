using HotfixMods.Providers.Interfaces;
using HotfixMods.Providers.Models;
using MySqlConnector;

namespace HotfixMods.Providers.MySqlConnector.Client
{
    public partial class MySqlClient : IServerDbProvider, IServerDbDefinitionProvider
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
            var replaceQuery = $"REPLACE INTO {schemaName}.{tableName} VALUES ";

            using var mySqlConnection = new MySqlConnection(_connectionString);
            await mySqlConnection.OpenAsync();
            using (var cmd = new MySqlCommand(mySqlConnection, null))
            {
                foreach (var dbColumn in dbRows)
                {
                    string values = string.Join(",", dbColumn.Columns.Select(d => $"'{MySqlHelper.EscapeString(d.Value.ToString()!)}'"));
                    queries.Add($"({values})");

                    if (queries.Count == AddBatchSize)
                    {
                        cmd.CommandText = replaceQuery + string.Join(",", queries);
                        await cmd.ExecuteNonQueryAsync();
                        queries = new();
                    }
                }

                if (queries.Count > 0)
                {
                    cmd.CommandText = replaceQuery + string.Join(",", queries);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            await mySqlConnection.CloseAsync();
        }

        public async Task CreateTableIfNotExistsAsync(string schemaName, string tableName, DbRowDefinition dbRowDefinition)
        {
            string columns = "";
            for (int i = 0; i < dbRowDefinition.ColumnDefinitions.Count(); i++)
            {
                var fieldName = dbRowDefinition.ColumnDefinitions.ElementAt(i).Name;
                var fieldType = CSharpTypeToMySqlDataType(dbRowDefinition.ColumnDefinitions.ElementAt(i).Type);
                columns += $"`{fieldName}` {fieldType},";
            }

            string createSchemaQuery = $"CREATE SCHEMA IF NOT EXISTS `{schemaName}`;";
            string createTableQuery = $"CREATE TABLE IF NOT EXISTS `{schemaName}`.`{tableName}` ({columns} primary key(ID));";
            using var mySqlConnection = new MySqlConnection(_connectionString);
            using var cmd = new MySqlCommand(createSchemaQuery + createTableQuery, mySqlConnection);

            await mySqlConnection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
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

        public async Task<PagedDbResult> GetAsync(string schemaName, string tableName, DbRowDefinition dbRowDefinition, int pageIndex, int pageSize, params DbParameter[] parameters)
        {
            if (null == dbRowDefinition || !dbRowDefinition.ColumnDefinitions.Any())
            {
                throw new Exception("Missing definitions.");
            }

            var results = new PagedDbResult();

            if (!await TableExistsAsync(schemaName, tableName))
            {
                return results;
            }

            var dataQuery = $"SELECT * FROM {schemaName}.{tableName} {DbParameterToWhereClause(parameters)} LIMIT {pageSize} OFFSET {pageIndex * pageSize};";
            var countQuery = $"SELECT count(*) FROM {schemaName}.{tableName} {DbParameterToWhereClause(parameters)};";

            using var mySqlConnection = new MySqlConnection(_connectionString);
            using var dataCommand = new MySqlCommand(dataQuery, mySqlConnection);
            using var countCommand = new MySqlCommand(countQuery, mySqlConnection);

            await mySqlConnection.OpenAsync();
            // Count query
            using var countReader = await countCommand.ExecuteReaderAsync();
            while (countReader.Read())
            {
                if (!countReader.IsDBNull(0))
                    results.TotalRowCount = countReader.GetInt64(0);
                break;
            }

            // Data Query
            using var dataReader = await dataCommand.ExecuteReaderAsync();
            while (dataReader.Read())
            {
                var dbRow = new DbRow(tableName);
                for (int i = 0; i < dbRowDefinition.ColumnDefinitions.Count(); i++)
                {
                    var column = dbRowDefinition.ColumnDefinitions.ElementAt(i);
                    var fieldName = column.Name;

                    object propertyValue;
                    var serverType = column.GetServerType();

                    if (dataReader.IsDBNull(i))
                    {
                        if(serverType == typeof(string))
                        {
                            propertyValue = "";
                        }
                        else
                        {
                            propertyValue = serverType.IsValueType ? Activator.CreateInstance(serverType) : null;
                        }
                    }
                    else
                    {
                        propertyValue = Convert.ChangeType(dataReader.GetValue(i), serverType);
                    }

                    if (propertyValue == null)
                    {
                        throw new Exception($"{dbRowDefinition.ColumnDefinitions.ElementAt(i).Type} not implemented.");
                    }

                    dbRow.Columns.Add(new()
                    {
                        Value = propertyValue,
                        Definition = column
                    });
                }
                results.Rows.Add(dbRow);
            }
            await mySqlConnection.CloseAsync();

            return results;
        }

        public async Task<string> GetHighestIdAsync(string schemaName, string tableName, string minId, string maxId, string idPropertyName = "id")
        {
            if (!await TableExistsAsync(schemaName, tableName))
            {
                return "0";
            }
            using var mySqlConnection = new MySqlConnection(_connectionString);
            await mySqlConnection.OpenAsync();
            string query = $"SELECT max(@idPropertyName) FROM @schemaName.@tableName WHERE @idPropertyName >= @minId AND @idPropertyName <= @maxId;";
            using var cmd = new MySqlCommand(query, mySqlConnection); cmd.Parameters.AddWithValue("@idPropertyName", idPropertyName);
            cmd.Parameters.AddWithValue("@schemaName", schemaName);
            cmd.Parameters.AddWithValue("@tableName", tableName);
            cmd.Parameters.AddWithValue("@minId", minId);
            cmd.Parameters.AddWithValue("@maxId", maxId);
            var reader = await cmd.ExecuteReaderAsync();
            var highestId = minId;
            while (reader.Read())
            {
                if (!reader.IsDBNull(0))
                    highestId = reader.GetValue(0).ToString();
                break;
            }
            await mySqlConnection.CloseAsync();
            return highestId ?? "0";
        }

        public Task<DbRow> GetSingleAsync(string schemaName, string tableName, DbRowDefinition dbRowDefinition, params DbParameter[] parameters)
        {
            throw new NotImplementedException();
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

        public async Task<bool> SchemaExistsAsync(string schemaName)
        {
            using var mySqlConnection = new MySqlConnection(_connectionString);
            await mySqlConnection.OpenAsync();
            using var cmd = new MySqlCommand($"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = @schemaName;", mySqlConnection);
            cmd.Parameters.AddWithValue("@schemaName", schemaName);
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

        public async Task<bool> TableExistsAsync(string schemaName, string tableName)
        {
            using var mySqlConnection = new MySqlConnection(_connectionString);
            await mySqlConnection.OpenAsync();
            using var cmd = new MySqlCommand($"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = @schemaName AND table_name = @tableName;", mySqlConnection);
            cmd.Parameters.AddWithValue("schemaName", schemaName);
            cmd.Parameters.AddWithValue("tableName", tableName);
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
                var colName = reader.GetString(0);
                dbRowDefinition.ColumnDefinitions.Add(new()
                {
                    Name = colName,
                    Type = MySqlDataTypeToCSharpType(reader.GetString(1)),
                    IsIndex = reader.GetString(3).Equals("PRI", StringComparison.InvariantCultureIgnoreCase) && !colName.Equals("VerifiedBuild", StringComparison.InvariantCultureIgnoreCase),

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
    }
}
