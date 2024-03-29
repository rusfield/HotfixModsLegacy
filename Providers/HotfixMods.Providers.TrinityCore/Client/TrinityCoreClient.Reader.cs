﻿using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Providers.TrinityCore.Client
{
    public partial class TrinityCoreClient
    {
        /// <summary>
        /// Get the enum values from TrinityCore source code as a C# Dictionary.
        /// </summary>
        /// <typeparam name="TKey">type of Dictionary Key</typeparam>
        /// <param name="filePath">File path to the enum (store as a string value for multiple use, don't enter directly)</param>
        /// <param name="enumName">Name of enum. Code will look for 'enum {enumName}' and 'enum class {enumName}'</param>
        /// <param name="enumStringValueRemoves">Parts of the enum result to remove. If an enum value matches this value exactly (resulting in a string.IsNullOrWhiteSpae == true), the entire value will be skipped.</param>
        /// <returns></returns>
        async Task<Dictionary<TKey, string>> GetEnumAsync<TKey>(string filePath, string enumName, params string[] enumStringValueRemoves)
            where TKey : notnull
        {
            string cacheKey = $"{filePath}{enumName}{typeof(TKey)}";
            if (_cache.TryGetValue(cacheKey, out var cachedResults))
                return (Dictionary<TKey, string>)cachedResults;

            var results = new Dictionary<TKey, string>();
            /*
            var defaultKey = default(TKey);
            if (defaultKey != null)
                results[defaultKey] = "NONE";
            */
            var fullPath = Path.Combine(TrinityCorePath, filePath);
            if (File.Exists(fullPath))
            {
                using (StreamReader reader = new StreamReader(fullPath))
                {
                    string? line;
                    bool enumFound = false;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        if (enumFound)
                        {
                            if (line.Contains("}") || line.Trim().StartsWith("MAX_"))
                            {
                                break;
                            }
                            else if (line.Contains("="))
                            {
                                // Remove comments
                                line = line.Split("//")[0];

                                var enumString = line.Split('=')[0].Trim();
                                var enumNumber = line.Split('=')[1].Replace(",", "").Trim();

                                // Check if value is valid
                                if (!ValueIsValid(enumNumber))
                                    continue;

                                // enumStringValueRemove is the beginning or end of the enum in TrinityCore (the naming convention). No need to dispaly the same over and over.
                                foreach (var valueRemove in enumStringValueRemoves)
                                {
                                    if (enumString.StartsWith(valueRemove, StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        enumString = enumString.Substring(valueRemove.Length);
                                    }
                                    if (enumString.EndsWith(valueRemove, StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        enumString = enumString.Substring(0, enumString.Length - valueRemove.Length);
                                    }
                                }

                                // If value is stripped, skip it.
                                // Intentional way of skipping certain values.
                                if (string.IsNullOrWhiteSpace(enumString))
                                    continue;

                                // Check and fix if enumNumber is hex
                                enumNumber = ConvertIfHex<TKey>(enumNumber);

                                // Check and fix if enumNumber uses shifting
                                enumNumber = ConvertIfShifting<TKey>(results, enumNumber);

                                var key = (TKey)Convert.ChangeType(enumNumber, typeof(TKey));
                                results[key] = UnderscoreToCase(enumString);
                            }
                        }

                        var compareEnumLine = $"enum {enumName}";
                        var compareEnumClassLine = $"enum class {enumName}";
                        var lineCompare = line.Split(':')[0].Trim();
                        if (lineCompare.StartsWith(compareEnumLine, StringComparison.InvariantCultureIgnoreCase) || lineCompare.StartsWith(compareEnumClassLine, StringComparison.InvariantCultureIgnoreCase))
                            enumFound = true;
                    }
                }

                if (CacheResults)
                    _cache.Set(cacheKey, results, _cacheOptions);
            }

            return results;
        }
    }
}
