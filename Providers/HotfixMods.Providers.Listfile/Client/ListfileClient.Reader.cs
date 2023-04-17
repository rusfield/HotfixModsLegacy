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

        Dictionary<(string, string, Type), object> _cache = new();
        async Task<Dictionary<TKey, string>> ReadFileAsync<TKey>(string filename, string? partialPath = null, string? fileType = null)
            where TKey : notnull
        {
            if (_cache.ContainsKey((filename, partialPath ?? "", typeof(TKey))))
                return (Dictionary<TKey, string>)_cache[(filename, partialPath ?? "", typeof(TKey))];

            var results = new Dictionary<TKey, string>();
            results[default(TKey)] = "None";
            await Task.Run(() =>
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                using (Stream? stream = assembly.GetManifestResourceStream($"HotfixMods.Providers.Listfile.Listfiles.{filename}.csv"))
                {
                    if (stream == null)
                        throw new Exception($"Resource {filename} not found.");

                    using (StreamReader reader = new StreamReader(stream))
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

                            value = $"{UnderscoreToCase(value)}";
                            results[key] = value;
                        }
                    }
                }
            });

            if (CacheResults)
                _cache[(filename, partialPath ?? "", typeof(TKey))] = results;

            return results;
        }
    }
}
