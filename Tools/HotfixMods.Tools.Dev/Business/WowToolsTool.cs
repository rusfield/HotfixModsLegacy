using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HotfixMods.Tools.Dev.Business
{
    public class WowToolsTool
    {
        public async Task<string> ArrayInClipboardToCSharp()
        {
            var value = await TextCopy.ClipboardService.GetTextAsync();
            return string.IsNullOrEmpty(value) ? "" : await ArrayInClipboardToCSharp(value);
        }
        public async Task<string> ArrayInClipboardToCSharp(string wowToolsArray)
        {
            await TextCopy.ClipboardService.SetTextAsync("");
            var arrayRows = wowToolsArray.Split("\r\n").ToList();
            string name = arrayRows[0].Split(" ")[1];
            await WriteToConsoleAndClipboard($"public enum {name}");
            await WriteToConsoleAndClipboard("{");

            var rows = arrayRows.Where(a => a.Trim().StartsWith("\""));

            int index = 0;
            foreach(var row in rows)
            {
                var enumValue = Regex.Replace(row, @"(?<!_|^)([A-Z])", "_$1").Trim().ToUpper();

                if(enumValue.StartsWith("\"_"))
                    enumValue = enumValue.Substring(2);
                if(enumValue.EndsWith(","))
                    enumValue = enumValue.Substring(0, enumValue.Length - 1);
                if (enumValue.EndsWith("\""))
                    enumValue = enumValue.Substring(0, enumValue.Length - 1);

                enumValue = enumValue.Replace("1_H", "_1H");
                enumValue = enumValue.Replace("2_H", "_2H");
                enumValue = enumValue.Replace("W_A_", "WA_");
                enumValue = enumValue.Replace("00", "_00");
                enumValue = enumValue.Replace("01", "_01");
                enumValue = enumValue.Replace("02", "_02");
                enumValue = enumValue.Replace("03", "_03");
                enumValue = enumValue.Replace("04", "_04");
                enumValue = enumValue.Replace("05", "_05");
                enumValue = enumValue.Replace("06", "_06");
                enumValue = enumValue.Replace("07", "_07");
                enumValue = enumValue.Replace("08", "_08");
                enumValue = enumValue.Replace("09", "_09");

                await WriteToConsoleAndClipboard($"{enumValue} = {index++},");
            }
            await WriteToConsoleAndClipboard("}");

            return (await TextCopy.ClipboardService.GetTextAsync())!;
        }
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
