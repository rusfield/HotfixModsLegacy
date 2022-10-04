using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HotfixMods.Providers.MySql.MySqlConnector.Client
{
    public partial class MySqlClient
    {
        // Simple SQL injection protection,
        // remove if needed.
        void ValidateInput(params string?[] inputs)
        {
            var regex = new Regex("^[A-Za-z0-9 ='_-]*$");
            foreach (var input in inputs)
            {
                if (!string.IsNullOrEmpty(input))
                {
                    if (!regex.Match(input).Success)
                        throw new Exception($"'{input}' is not a valid input.");
                }
            }
        }
    }
}
