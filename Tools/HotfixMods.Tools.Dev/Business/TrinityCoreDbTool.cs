

using HotfixMods.Core.Models;
using System.Data.Common;

namespace HotfixMods.Tools.Dev.Business
{
    public class TrinityCoreDbTool
    {
        public async Task DbDefToCSharp(DbRowDefinition definition)
        {
            await TextCopy.ClipboardService.SetTextAsync("");
            await WriteToConsoleAndClipboard("[INSERT_SCHEMA]");
            await WriteToConsoleAndClipboard($"public class {FixUnderscoresAndCasing(definition.DbName)}");
            await WriteToConsoleAndClipboard("{");

            foreach (var column in definition.ColumnDefinitions)
            {
                var propertyName = FixUnderscoresAndCasing(column.Name);
                if(!column.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase))
                    await WriteToConsoleAndClipboard($"[Column(\"{column.Name}\")]");

                await WriteToConsoleAndClipboard($"public {GetTypeName(column.Type.Name)} {propertyName}" + " { get; set; }");
            }

            await WriteToConsoleAndClipboard("}");
        }

        string FixUnderscoresAndCasing(string input)
        {
            var nameSplits = input.Split('_');
            var result = "";
            foreach (var nameSplit in nameSplits)
                result += nameSplit[0].ToString().ToUpper() + nameSplit.Substring(1);
            if(result.EndsWith("ID", StringComparison.InvariantCulture))
                result = result.Substring(0, result.Length - 2) + "Id";
            return result;
        }

        string GetTypeName(string prop)
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
