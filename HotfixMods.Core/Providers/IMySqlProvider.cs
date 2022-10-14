using HotfixMods.Core.Models.App;
using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Providers
{
    public interface IMySqlProvider
    {
        public Task<bool> ConnectionExistsAsync();

        public Task<T?> GetSingleAsync<T>(string? whereClause = null)
            where T : new();
        public Task<IEnumerable<T>> GetAsync<T>(string? whereClause = null)
            where T : new();
        public Task AddOrUpdateAsync<T>(params T[] db2Rows)
            where T : new();
        public Task DeleteAsync<T>(string whereClause)
            where T : new();
        public Task<bool> SchemaExistsAsync<T>()
            where T : new();
        public Task<bool> TableExistsAsync<T>()
            where T : new();
        public Task<int> GetNextIdAsync<T>(int fromId)
            where T : new();

        public Task<IEnumerable<Db2Column>> GetSingleAsync(string schemaName, string tableName, IEnumerable<Db2ColumnDefinition> definitions, string? whereClause = null);
        public Task<IEnumerable<IEnumerable<Db2Column>>> GetAsync(string schemaName, string tableName, IEnumerable<Db2ColumnDefinition> definitions, string? whereClause= null);
        public Task AddOrUpdateAsync(string schemaName, string tableName, params IEnumerable<Db2Column>[] db2Rows);
        public Task DeleteAsync(string schema, string tableName, string whereClause);
        public Task<bool> SchemaExistsAsync(string schema);
        public Task<bool> TableExistsAsync(string schema, string tableName);
        public Task<int> GetNextIdAsync(string schema, string tableName, int fromId);
    }
}
