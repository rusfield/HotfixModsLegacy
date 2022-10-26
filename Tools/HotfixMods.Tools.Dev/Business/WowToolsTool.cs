using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Tools.Dev.Business
{
    public class WowToolsTool
    {
        public async Task<string> FlagInClipboardToCSharp()
        {
            var value = await TextCopy.ClipboardService.GetTextAsync();
            return string.IsNullOrEmpty(value) ? "" : await FlagToCSharp(value);
        }
        public async Task<string> FlagToCSharp(string wowToolsFlag)
        {
            await TextCopy.ClipboardService.SetTextAsync("");
            var flagRows = wowToolsFlag.Split("\r\n").ToList();
            string flagName = flagRows[0].Split(" ")[1];
            await WriteToConsoleAndClipboard(char.ToUpper(flagName[0]) + flagName.Substring(1));

            var flags = flagRows.Where(f => f.Contains(":")).ToDictionary(flag => Convert.ToInt64(flag.Split(":")[0].Trim(), 16), flag => flag.Split(":")[1]);

            await WriteToConsoleAndClipboard("[Flags]");
            await WriteToConsoleAndClipboard($"public enum {flagName} : long");
            await WriteToConsoleAndClipboard("{");
            await WriteToConsoleAndClipboard($"DEFAULT = 0,");
            for(long i = 1; i <= 2147483648; i = i * 2)
            {
                if (flags.ContainsKey(i))
                {
                    var name = flags[i];

                    name = name.Replace("Don\\'t", "do not");

                    name = name.Split("'")[1].Replace(" ", "_").ToUpper();
                    await WriteToConsoleAndClipboard($"{name} = {i},");
                }
                else
                {
                    await WriteToConsoleAndClipboard($"UNK_{i} = {i},");
                }
            }
            await WriteToConsoleAndClipboard("}");
            return (await TextCopy.ClipboardService.GetTextAsync())!;
        }

        public async Task<string> EnumInClipboardToCSharp()
        {
            var value = await TextCopy.ClipboardService.GetTextAsync();
            return string.IsNullOrEmpty(value) ? "" : await EnumToCSharp(value);
        }

        public async Task<string> EnumToCSharp(string wowToolsEnum)
        {
            await TextCopy.ClipboardService.SetTextAsync("");
            var enumRows = wowToolsEnum.Split("\r\n").ToList();
            string enumName = enumRows[0].Split(" ")[1];
            await WriteToConsoleAndClipboard(char.ToUpper(enumName[0]) + enumName.Substring(1));
            var enums = enumRows.Where(e => e.Contains(":")).ToDictionary(en => Convert.ToInt32(en.Split(":")[0].Trim()), en => en.Split(":")[1]);

            await WriteToConsoleAndClipboard($"public enum {enumName} : int");
            await WriteToConsoleAndClipboard("{");

            for (int i = 0; i <= enums.Max(e => e.Key); i++)
            {
                if (enums.ContainsKey(i))
                {
                    var name = enums[i];

                    name = name.Replace(" - ", " ");
                    name = name.Replace("(", "").Replace(")", "");

                    name = name.Split("'")[1].Replace(" ", "_").ToUpper();
                    await WriteToConsoleAndClipboard($"{name} = {i},");
                }
                else
                {
                    await WriteToConsoleAndClipboard($"UNK_{i} = {i},");
                }
            }
            await WriteToConsoleAndClipboard("}");
            return (await TextCopy.ClipboardService.GetTextAsync())!;
        }



        async Task WriteToConsoleAndClipboard(string input)
        {
            Console.WriteLine(input);
            await TextCopy.ClipboardService.SetTextAsync(await TextCopy.ClipboardService.GetTextAsync() + input + Environment.NewLine);
        }
    }
}
