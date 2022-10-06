using HotfixMods.Providers.DbDef.WoWDev.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Dev.Helpers
{
    public static class DefinitionHelper
    {
        public static async Task DefinitionToCSharp(string defName, string build)
        {
            var defHelper = new DbDefClient();
            var definition = await defHelper.GetAvailableColumnsAsync(defName, build);
            Console.WriteLine($"public class {defName}");
            Console.WriteLine("{");
            foreach(var def in definition)
            {
                Console.WriteLine($"public {GetPropertyName(def.Value.Name)} {def.Key} " + "{ get; set; }");
            }
            Console.WriteLine("}");
        }

        static string GetPropertyName(string prop)
        {
            return prop switch
            {
                "Decimal" => "decimal",
                "String" => "string",
                "SByte" => "sbyte",
                "Byte" => "byte",
                "Int16" => "short",
                "UInt16" => "ushort",
                "Int32" => "int",
                "UInt32" => "uint",
                "Int64" => "long",
                "UInt64" => "ulong",
                _ => prop
                //_ => throw new Exception($"{prop} not implemented.")
            };
        }
    }
}
