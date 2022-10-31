using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Razor.Extensions
{
    public static class ListExtensions
    {
        public static Dictionary<T, T> ListToOptions<T>(this IEnumerable<T> list)
        {
            if(null == list)
                return new Dictionary<T, T>();
            return list.Distinct().ToDictionary(x => x, y => y);
        }
    }
}
