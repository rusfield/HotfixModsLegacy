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
        public Task<T?> GetAsync<T>(Expression<Func<T, bool>> predicate)
            where T : class, ITrinityCore;
        public Task<IEnumerable<T>> GetManyAsync<T>(Expression<Func<T, bool>> predicate)
            where T : class, ITrinityCore;
        public Task AddAsync<T>(T entity)
            where T : class, ITrinityCore;
        public Task AddManyAsync<T>(IEnumerable<T> entities)
            where T : class, ITrinityCore;
        public Task UpdateAsync<T>(T entity)
            where T : class, ITrinityCore;
        public Task UpdateManyAsync<T>(IEnumerable<T> entities)
            where T : class, ITrinityCore;
        public Task DeleteAsync<T>(T entity)
            where T : class, ITrinityCore;
        public Task DeleteManyAsync<T>(IEnumerable<T> entities)
            where T : class, ITrinityCore;
        public Task<bool> TableExists<T>()
            where T : class, ITrinityCore;
        public Task<bool> CharactersConnectionTestAsync();
        public Task<bool> HotfixesConnectionTestAsync();
        public Task<bool> WorldConnectionTestAsync();
        public Task<bool> CreateCreatureCreatorTableIfNotExist();
    }
}
