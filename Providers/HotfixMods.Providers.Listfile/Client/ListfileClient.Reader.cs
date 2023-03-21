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
        async Task<Dictionary<TKey, string>> ReadFileAsync<TKey>(string filename, string? partialPath = null)
            where TKey : notnull
        {
            var results = new Dictionary<TKey, string>();
            results[default(TKey)] = "0 - None";
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

                            value = value.Split('/').Last(); // ability_ambush.blp
                            value = value.Split('.').First(); // ability_ambush

                            string keyString = line.Split(';')[0]; // 132089
                            var key = (TKey)Convert.ChangeType(keyString, typeof(TKey));

                            value = $"{key} - {UnderscoreToCase(value)}";
                            results[key] = value;
                        }
                    }
                }
            });
            return results;
        }
    }
}
