using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Providers.TrinityCore.Client
{
    public partial class TrinityCoreClient
    {
        Dictionary<(string, string), object> _cache = new();

        async Task<Dictionary<TKey, string>> GetEnumAsync<TKey>(string filePath, string enumName, params string[] enumStringValueRemoves)
            where TKey : notnull
        {
            if (_cache.ContainsKey((filePath, enumName)))
                return (Dictionary<TKey, string>)_cache[(filePath, enumName)];

            var results = new Dictionary<TKey, string>();
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
                            if (line.Contains("}"))
                            {
                                break;
                            }
                            else if (line.Contains("="))
                            {
                                // Remove comments
                                line = line.Split("//")[0];

                                // enumStringValueRemove is the beginning of the enum in TrinityCore (the naming convention). No need to dispaly the same over and over.
                                var enumString = line.Split('=')[0].Trim();
                                var enumNumber = line.Split('=')[1].Replace(",", "").Trim();

                                foreach(var valueRemove in enumStringValueRemoves)
                                {
                                    if(enumString.StartsWith(valueRemove, StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        enumString = enumString.Substring(valueRemove.Length);
                                    }
                                    if(enumString.EndsWith(valueRemove, StringComparison.InvariantCultureIgnoreCase) )
                                    {
                                        enumString = enumString.Substring(0, enumString.Length - valueRemove.Length);
                                    }
                                }

                                results.Add((TKey)Convert.ChangeType(enumNumber, typeof(TKey)), UnderscoreToCase(enumString));
                            }
                        }

                        var compareLine = $"enum {enumName}";
                        if (line.Split(':')[0].Trim().Equals(compareLine, StringComparison.InvariantCultureIgnoreCase))
                            enumFound = true;
                    }
                }

                _cache.Add((filePath, enumName), results);
            }

            return results;
        }
    }
}
