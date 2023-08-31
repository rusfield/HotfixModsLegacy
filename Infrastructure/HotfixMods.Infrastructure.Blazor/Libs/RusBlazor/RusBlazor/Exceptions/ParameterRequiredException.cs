using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RusBlazor.Exceptions
{
    public class ParameterRequiredException : Exception
    {
        public ParameterRequiredException(string parameterName)
                    : base($"The required parameter {parameterName} is missing.")
        {

        }
    }
}
