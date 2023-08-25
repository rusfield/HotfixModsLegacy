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

        public Task AddOrUpdateAsync(string schemaName, string tableName, params DbRow[] dbRows)
        {
            throw new NotImplementedException();
        }

        public Task CreateTableIfNotExistsAsync(string schemaName, string tableName, DbRowDefinition dbRowDefinition)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string schemaName, string tableName, params DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DbRow>> GetAsync(string schemaName, string tableName, DbRowDefinition dbRowDefinition, params DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetHighestIdAsync(string schema, string tableName, int minId, int maxId, string idPropertyName = "id")
        {
            throw new NotImplementedException();
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
            return await CheckIfSchemaExistsAsync(schemaName);
        }

        public async Task<bool> TableExistsAsync(string schemaName, string tableName)
        {
            return await CheckIfTableExistsAsync(schemaName, tableName);
        }
    }
}
