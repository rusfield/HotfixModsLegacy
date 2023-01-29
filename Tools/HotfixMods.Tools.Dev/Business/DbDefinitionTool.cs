using HotfixMods.Core.Models;
using HotfixMods.Providers.WowDev.Client;
using System.Runtime.CompilerServices;

namespace HotfixMods.Tools.Dev.Business
{

    public class DbDefinitionTool
    {
        public async Task<string> DefinitionToCSharp(string defName, string build)
        {
            await TextCopy.ClipboardService.SetTextAsync("");

            var defHelper = new Db2Client(build, "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");
            var definition = await defHelper.GetDefinitionAsync(null, defName);

            await WriteToConsoleAndClipboard("[HotfixesSchema]");
            await WriteToConsoleAndClipboard($"public class {defName}");
            await WriteToConsoleAndClipboard("{");
            foreach (var def in definition.ColumnDefinitions)
            {
                string end = "";
                if (def.Name == "VerifiedBuild")
                    end = " = -1;";
                else if (def.Name == "ID")
                    end = " = 1;";

                await WriteToConsoleAndClipboard($"public {GetPropertyName(def.Type.Name.ToString())} {def.Name.Replace("ID", "Id", StringComparison.InvariantCulture)} " + "{ get; set; }" + $"{end}");
            }
            await WriteToConsoleAndClipboard("}");
            return (await TextCopy.ClipboardService.GetTextAsync())!;
        }

        public List<string> CompareDefinitions(DbRowDefinition masterDefinition, DbRowDefinition otherDefinition)
        {
            if(null == masterDefinition || null == otherDefinition)
            {
                Console.WriteLine($"One of the definitions is null.");
                return new();
            }
            var result = new List<string>();

            foreach (var def in masterDefinition.ColumnDefinitions)
            {
                var otherDef = otherDefinition.ColumnDefinitions.Where(d => d.Name.Equals(def.Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (null == otherDef)
                {
                    result.Add($"{def.Name} with type {GetPropertyName(def.Type.Name)} is missing.");
                }
                else if (def.Type != otherDef.Type)
                {
                    result.Add($"{def.Name} with type {GetPropertyName(otherDef.Type.Name)} should be {GetPropertyName(def.Type.Name)}.");
                }
            }
            foreach (var def in otherDefinition.ColumnDefinitions)
            {
                var masterDef = masterDefinition.ColumnDefinitions.Where(d => d.Name.Equals(def.Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (null == masterDef)
                {
                    result.Add($"Property {def.Name} does not exist in master.");
                }
            }
            if (result.Count == 0)
                Console.WriteLine($"{masterDefinition.DbName} is OK");
            else
                foreach (var r in result)
                {
                    Console.WriteLine(r);
                }
            return result;
        }

        string GetPropertyName(string prop)
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

        async Task WriteToConsoleAndClipboard(string input)
        {
            Console.WriteLine(input);
            await TextCopy.ClipboardService.SetTextAsync(await TextCopy.ClipboardService.GetTextAsync() + input + Environment.NewLine);
        }


    }
}
