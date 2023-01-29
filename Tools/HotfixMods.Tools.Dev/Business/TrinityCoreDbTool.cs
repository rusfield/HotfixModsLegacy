

using HotfixMods.Core.Models;
using System.Data.Common;
using TextCopy;

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
                // Add Column attribute if db name mismatches from C# standard
                /*
                var propertyName = FixUnderscoresAndCasing(column.Name);
                if(!column.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase))
                    await WriteToConsoleAndClipboard($"[Column(\"{column.Name}\")]");
                */
                await WriteToConsoleAndClipboard($"public {GetTypeName(column.Type.Name)} {FixCasing(column.Name)}" + " { get; set; }");
            }

            await WriteToConsoleAndClipboard("}");
        }

        public async Task Db2HashEnumInClipboardToCSharp()
        {
            // Copy content between brackets only.
            // https://github.com/TrinityCore/WowPacketParser/blob/master/WowPacketParser/Enums/DB2Hash.cs

            var rows = await ClipboardService.GetTextAsync();
            await ClipboardService.SetTextAsync("");
            foreach(var row in rows.Split(Environment.NewLine))
            {
                var rowData = row.Split("=");
                var tempName = rowData[0].Trim();
                string name = "";
                foreach(var c in tempName)
                {
                    if (char.IsUpper(c) && name.Length > 0)
                    {
                        name += "_";
                    }
                    name += c;
                }
                var stringValue = rowData[1].Trim();
                stringValue = stringValue.Substring(0, stringValue.Length - 1);
                var value = Convert.ToUInt32(stringValue, 16);
                await WriteToConsoleAndClipboard($"{name.ToUpper()} = {value},");
            }
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

        string FixCasing(string input)
        {
            var nameSplits = input.Split('_');
            var result = "";
            foreach (var nameSplit in nameSplits)
                result += "_" + nameSplit[0].ToString().ToUpper() + nameSplit.Substring(1);
            if (result.EndsWith("ID", StringComparison.InvariantCulture))
                result = result.Substring(0, result.Length - 2) + "Id";
            return result.Substring(1);
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
