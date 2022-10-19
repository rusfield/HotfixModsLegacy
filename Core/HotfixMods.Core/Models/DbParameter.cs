using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HotfixMods.Core.Models.DbParameter;

namespace HotfixMods.Core.Models
{
    public class DbParameter
    {
        public DbParameter(string property, DbOperator dbOperator, object value)
        {
            Property = property;
            Operator = dbOperator;
            Value = value;
        }

        public DbParameter(string property, object value)
        {
            Property = property;
            Operator = DbOperator.EQ;
            Value = value;
        }

        public string Property { get; set; }
        public DbOperator Operator { get; set; }
        public object Value { get; set; }

        public enum DbOperator
        {
            EQ,
            CONTAINS,
            GT,
            GTE,
            LT,
            LTE
        }
    }
}
