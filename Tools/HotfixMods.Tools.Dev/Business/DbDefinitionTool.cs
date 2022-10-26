using HotfixMods.Providers.WowDev.Client;
using System.Runtime.CompilerServices;

namespace HotfixMods.Tools.Dev.Business
{

    public class DbDefinitionTool
    {
        public async Task<string> DefinitionToCSharp(string defName, string build)
        {
            await TextCopy.ClipboardService.SetTextAsync("");

            var defHelper = new Db2Client(build);
            var definition = await defHelper.GetDefinitionAsync(null, defName);

            await WriteToConsoleAndClipboard("[HotfixesSchema]");
            await WriteToConsoleAndClipboard($"public class {defName}");
            await WriteToConsoleAndClipboard("{");
            foreach (var def in definition.ColumnDefinitions)
            {
                await WriteToConsoleAndClipboard($"public {GetPropertyName(def.Type.Name.ToString().Replace("ID", "Id", StringComparison.InvariantCulture))} {def.Name} " + "{ get; set; }");
            }
            await WriteToConsoleAndClipboard("}");
            return (await TextCopy.ClipboardService.GetTextAsync())!;
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
