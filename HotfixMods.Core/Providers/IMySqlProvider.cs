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
        public Task<T?> GetSingleAsync<T>(Expression<Func<T, bool>> predicate)
            where T : class, ITrinityCore;
        public Task<IEnumerable<T>> GetAsync<T>(Expression<Func<T, bool>> predicate)
            where T : class, ITrinityCore;
        public Task AddOrUpdateAsync<T>(params T[] entities)
            where T : class, ITrinityCore;

        public Task DeleteAsync<T>(params T[] entities)
            where T : class, ITrinityCore;

        public Task<bool> TableExists<T>()
            where T : class, ITrinityCore;
        public Task<bool> CharactersConnectionTestAsync();
        public Task<bool> HotfixesConnectionTestAsync();
        public Task<bool> WorldConnectionTestAsync();
    }
}
