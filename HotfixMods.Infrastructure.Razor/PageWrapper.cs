using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Razor
{
    public class PageWrapper
    {

    }

    public class TabWrapper
    {
        public string TabName { get; set; }
        public string TableName { get; set; }
        public Dictionary<string, Type> Columns { get; set; }
        public Dictionary<string, object> Values { get; set; }

        public TabWrapper()
        {
            int test = 123;
            Values.Add("test", test);
        }
    }
}
