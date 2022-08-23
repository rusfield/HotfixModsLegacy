using HotfixMods.Core.Models.Interfaces;
using HotfixMods.Core.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.MySqlProvider.Debug.Clients
{
    public class MySqlProvider : IMySqlProvider
    {
        public async Task<bool> CharactersConnectionTestAsync()
        {
            return true;
        }

        public async Task<bool> HotfixesConnectionTestAsync()
        {
            return true;
        }

        public async Task<bool> WorldConnectionTestAsync()
        {
            return true;
        }

        public async Task AddAsync<T>(T entity)
            where T : class, ITrinityCore
        {
            return;
        }

        public async Task AddManyAsync<T>(IEnumerable<T> entities)
            where T : class, ITrinityCore
        {
            return;
        }

        public async Task DeleteAsync<T>(T entity)
            where T : class, ITrinityCore
        {
            return;
        }

        public async Task DeleteManyAsync<T>(IEnumerable<T> entities)
            where T : class, ITrinityCore
        {
            return;
        }

        public async Task<T?> GetAsync<T>(Expression<Func<T, bool>> predicate)
            where T : class, ITrinityCore
        {
            return Activator.CreateInstance<T>();
        }

        public async Task<IEnumerable<T>> GetManyAsync<T>(Expression<Func<T, bool>> predicate)
            where T : class, ITrinityCore
        {
            return new List<T>() { Activator.CreateInstance<T>() };
        }

        public async Task<bool> TableExists<T>()
            where T : class, ITrinityCore
        {
            return true;
        }

        public async Task UpdateAsync<T>(T entity)
            where T : class, ITrinityCore
        {
            return;
        }

        public async Task UpdateManyAsync<T>(IEnumerable<T> entities)
            where T : class, ITrinityCore
        {
            return;
        }
    }
}
