using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Dev.Helpers
{
    public class WowToolsConverter
    {
        public void ConvertFlagToCSharp(string filePath)
        {
            /* Excepted file format:
                0: 'NONE',
                1: 'BIND_SIGHT',
                2: 'MOD_POSSESS',
            */

            var result = new Dictionary<long, string>();

            using (StreamReader file = new StreamReader(filePath))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    line = line.Replace("//", "");
                    line = line.Replace("'", "");
                    line = line.Replace("\\", "");
                    line = line.Replace("(", "");
                    line = line.Replace(")", "");

                    line = line.Trim();

                    var hex = GetStringBefore(line, ":");
                    var enumNumber = StringHexToInt(hex.Trim());
                    var enumName = GetStringAfter(line, ":").Trim();

                    if (enumName == ",")
                        enumName = $"UNK_{enumNumber}";

                    if (enumName.Contains(","))
                        enumName = GetStringBefore(enumName, ",");

                    if (long.TryParse(enumName, out var parsed))
                        enumName = $"UNK_{enumName}";

                    result.Add(enumNumber, enumName.Trim().ToUpper().Replace(" ", "_"));
                }
                file.Close();
            }

            long i = 0;
            while (i<=result.Last().Key)
            {
                if (result.ContainsKey(i))
                {
                    Console.WriteLine($"{result[i]} = {i},");
                }
                else
                {
                    Console.WriteLine($"UNK_{i} = {i},");
                }

                if (i == 0)
                    i = 1;
                else
                    i = i * 2;
            }
        }

        public void ConvertEnumToCSharp(string filePath)
        {
            /* Excepted file format:
                0: 'NONE',
                1: 'BIND_SIGHT',
                2: 'MOD_POSSESS',
            */

            var result = new Dictionary<int, string>();

            using (StreamReader file = new StreamReader(filePath))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    line = line.Replace(" ", "");
                    line = line.Replace("//", "");
                    line = line.Replace("'", "");

                    var enumNumber = GetStringBefore(line, ":");
                    var enumName = GetStringAfter(line, ":");

                    if (enumName == ",")
                        enumName = $"UNK_{enumNumber}";

                    if (enumName.Contains(","))
                        enumName = GetStringBefore(enumName, ",");

                    if (int.TryParse(enumName, out var parsed))
                        enumName = $"UNK_{enumName}";

                    result.Add(int.Parse(enumNumber), enumName);
                }
                file.Close();
            }

            for (int i = 0; i<=result.Last().Key; i++)
            {
                if (result.ContainsKey(i))
                {
                    Console.WriteLine($"{result[i]} = {i},");
                }
                else
                {
                    Console.WriteLine($"UNK_{i} = {i},");
                }
            }
        }




        string GetStringBefore(string text, string stopAt)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return text.Substring(0, charLocation);
                }
            }

            throw new Exception("Missing delimiter");
        }

        string GetStringAfter(string text, string startAt)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                int charLocation = text.IndexOf(startAt, StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return text.Substring(charLocation + 1);
                }
            }

            throw new Exception("Missing delimiter");
        }

        long StringHexToInt(string hex)
        {
            return Convert.ToInt64(hex, 16);
        }
    }
}
