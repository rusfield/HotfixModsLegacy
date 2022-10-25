using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Razor.PageData
{
    public class PageTab<T>
        where T : new()
    {
        public PageTab(string tabName) 
        {
            TabName = tabName;
        }

        public string TabName { get; set; }
        public T? Dto { get; set; }
        public T? LookupDto { get; set; }
    }
}
