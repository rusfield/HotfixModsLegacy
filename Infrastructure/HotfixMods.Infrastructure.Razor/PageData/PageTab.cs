using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Razor.PageData
{
    public class PageTab
    {
        public PageTab(string tabName, Type pageType)
        {
            TabName = tabName;
            PageType=pageType;
            TabId = Guid.NewGuid();
        }

        public string TabName { get; set; }
        public Guid TabId { get; }
        public object? Dto { get; set; }
        public object? LookupDto { get; set; }
        public Type PageType { get; set; }

    }
}
