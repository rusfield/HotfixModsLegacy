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

        public async Task AddOrUpdateAsync<T>(params T[] entities)
            where T : class, ITrinityCore
        {
            return;
        }

        public async Task DeleteAsync<T>(params T[] entities)
            where T : class, ITrinityCore
        {
            return;
        }

        public async Task<T?> GetSingleAsync<T>(Expression<Func<T, bool>> predicate)
            where T : class, ITrinityCore
        {
            return null;
            //return Activator.CreateInstance<T>();
        }

        public async Task<IEnumerable<T>> GetAsync<T>(Expression<Func<T, bool>> predicate)
            where T : class, ITrinityCore
        {
            return new List<T>() { };
            //return new List<T>() { Activator.CreateInstance<T>() };
        }

        public async Task<bool> TableExists<T>()
            where T : class, ITrinityCore
        {
            return true;
        }
    }
}
