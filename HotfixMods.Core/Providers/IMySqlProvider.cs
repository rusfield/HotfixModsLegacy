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
        public Task<T?> GetSingleAsync<T>(string schema, string tableName, string? whereClause = null)
            where T : new();
        public Task<IEnumerable<T>> GetAsync<T>(string schema, string tableName, string? whereClause = null)
            where T : new();
        public Task AddOrUpdateAsync<T>(string schema, string tableName, params T[] entities)
            where T : new();
        public Task DeleteAsync(string schema, string tableName, string whereClause);
        public Task<bool> ConnectionExists();
        public Task<bool> SchemaExists(string schema);
        public Task<bool> TableExists(string schema, string tableName);
    }
}
