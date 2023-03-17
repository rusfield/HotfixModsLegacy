using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Attributes
{
    public class Db2DescriptionAttribute : Attribute
    {
        public string? Value { get; private set; }
        public Db2DescriptionAttribute(string description)
        {
            Value = description;
        }
    }
}
