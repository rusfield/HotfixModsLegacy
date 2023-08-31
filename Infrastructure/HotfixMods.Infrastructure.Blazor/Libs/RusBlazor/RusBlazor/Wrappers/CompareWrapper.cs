using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RusBlazor.Wrappers
{
    public class CompareWrapper<T>
    {
        public T Value { get; set; } = default(T);

        public CompareWrapper(T value)
        {
            Value = value;
        }
    }
}
