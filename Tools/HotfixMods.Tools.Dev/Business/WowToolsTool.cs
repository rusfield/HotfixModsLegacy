using System;
using System.Collections.Generic;
using System.Data;
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
            var enumRows = wowToolsEnum.Split("\r\n").Where(e => !e.StartsWith("//")).ToList();

            string enumName = enumRows[0].Split(" ")[1];
            var isArray = enumRows[0].StartsWith("let");

            var dictionaries = new Dictionary<string, Dictionary<int, string>>();
            int skip = 0; // header
            int take = 0;
            enumRows = enumRows.Skip(1).ToList(); // Remove header
            foreach (var enumRow in enumRows) 
            {
                take++;
                if (enumRow.Equals(string.Empty))
                {
                    var tempKey = isArray ? enumRows.Skip(skip).First().Split(" ")[0].Replace("[", "").Replace("]", "") : enumName;
                    Console.WriteLine(tempKey);
                    dictionaries.Add(char.ToUpper(tempKey[0]) + tempKey.Substring(1), enumRows.Skip(skip).Take(take).Where(e => e.Contains(":")).ToDictionary(en => Convert.ToInt32(en.Split(":")[0].Trim()), en => en.Split(":")[1]));
                    skip += take;
                    take = 0;
                }
            }
            var key = isArray ? enumRows.Skip(skip).First().Split(" ")[0].Replace("[", "").Replace("]", "") : enumName;
            dictionaries.Add(char.ToUpper(key[0]) + key.Substring(1), enumRows.Skip(skip).Take(take).Where(e => e.Contains(":")).ToDictionary(en => Convert.ToInt32(en.Split(":")[0].Trim()), en => en.Split(":")[1]));

            foreach (var enums in dictionaries)
            {
                await WriteToConsoleAndClipboard($"public enum {enums.Key}");
                await WriteToConsoleAndClipboard("{");

                for (int i = 0; i <= enums.Value.Max(e => e.Key); i++)
                {
                    if (enums.Value.ContainsKey(i))
                    {
                        var name = enums.Value[i];

                        name = name.Replace(" - ", " ");
                        name = name.Replace("(", "").Replace(")", "");
                        name = name.Replace("&", "AND");
                        name = name.Replace("-", " ");
                        name = name.Replace("can't", "can not");
                        name = name.Replace("don't", "do not");

                        name = name.Split("'")[1].Replace(" ", "_").ToUpper();
                        await WriteToConsoleAndClipboard($"{name} = {i},");
                    }
                    else
                    {
                        await WriteToConsoleAndClipboard($"UNK_{i} = {i},");
                    }
                }
                await WriteToConsoleAndClipboard("}\r\n");
            }
            return (await TextCopy.ClipboardService.GetTextAsync())!;
        }



        async Task WriteToConsoleAndClipboard(string input)
        {
            Console.WriteLine(input);
            await TextCopy.ClipboardService.SetTextAsync(await TextCopy.ClipboardService.GetTextAsync() + input + Environment.NewLine);
        }
    }
}
