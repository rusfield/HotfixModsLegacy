using HotfixMods.Infrastructure.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Apps.Console
{
    public class TestClass
        
    {
        public void Test<T>()
        {
            //var options = Enum.GetValues<T>().ToDictionary(e => e, v => v.ToString());
            var options2 = Enum.GetValues(Nullable.GetUnderlyingType(typeof(T)));
            var options3 = ((T[])options2).ToDictionary(e => e, v => v.ToString());
        }
    }
}
