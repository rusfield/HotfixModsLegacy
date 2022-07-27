using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Providers
{
    public interface IDb2Provider
    {
        public Task<T?> GetAsync<T>(Expression<Func<T, bool>> predicate)
            where T : IDb2;
        public Task<IEnumerable<T>> GetManyAsync<T>(Expression<Func<T, bool>> predicate)
            where T : IDb2;
    }
}
