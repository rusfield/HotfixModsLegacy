using Microsoft.Extensions.Caching.Memory;
using System.IO.Enumeration;
using System.Reflection;

namespace HotfixMods.Providers.Listfile.Client
{
    public partial class ListfileClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey">Intended for some numeric whole number type (int, short, etc)</typeparam>
        /// <param name="filename">Name of the split listfile (its been pre-split by a dev tool).</param>
        /// <param name="partialPath">Filter out a specific path in the file content.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>

        async Task<Dictionary<TKey, string>> ReadFileAsync<TKey>(string? partialPath, string? fileType = null, bool formatValue = false)
            where TKey : notnull
        {
            partialPath = partialPath ?? "";
            fileType = fileType ?? "";
            string cacheKey = $"{partialPath}{fileType}{formatValue}{typeof(TKey).Name}";
            if (_cache.TryGetValue(cacheKey, out var cachedResults))
                return (Dictionary<TKey, string>)cachedResults;

            var results = new Dictionary<TKey, string>();
            results[default(TKey)] = "None";
            await Task.Run(() =>
            {

                if (!File.Exists(_listfilePath))
                    throw new Exception($"Listfile not found.");

                using (StreamReader reader = new StreamReader(_listfilePath))
                {
                    // Example input: 132089;interface/icons/ability_ambush.blp

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string value = line.Split(';')[1]; // interface/icons/ability_ambush.blp

                        // Check if the partialPath is part of the value. Example partialPath: interface/icons
                        if (!string.IsNullOrWhiteSpace(partialPath) && !value.StartsWith(partialPath, StringComparison.InvariantCultureIgnoreCase))
                            continue;

                        // Check if the value has the correct fileType.
                        if (!string.IsNullOrWhiteSpace(fileType) && !value.EndsWith(fileType, StringComparison.InvariantCultureIgnoreCase))
                            continue;

                        value = value.Split('/').Last(); // ability_ambush.blp
                        value = value.Split('.').First(); // ability_ambush

                        string keyString = line.Split(';')[0]; // 132089
                        var key = (TKey)Convert.ChangeType(keyString, typeof(TKey));

                        if (formatValue)
                            value = $"{UnderscoreToCase(value)}";

                        results[key] = value;
                    }
                }
            });

            if (CacheResults)
                _cache.Set(cacheKey, results, _cacheOptions);

            return results;
        }
    }
}
