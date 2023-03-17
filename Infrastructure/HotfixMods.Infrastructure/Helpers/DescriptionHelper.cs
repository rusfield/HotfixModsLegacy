using HotfixMods.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Helpers
{
    public class DescriptionHelper
    {
        /// <summary>
        /// Tries to find the Db2Description of a type and return the value.
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public string? TryGetDescription(PropertyInfo? prop)
        {
            var descriptionAttribute = (Db2DescriptionAttribute?)prop?.GetCustomAttribute(typeof(Db2DescriptionAttribute));
            return descriptionAttribute?.Value;
        }
    }
}
