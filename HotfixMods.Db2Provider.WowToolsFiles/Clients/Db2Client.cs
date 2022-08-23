using HotfixMods.Core.Models.Interfaces;
using HotfixMods.Core.Providers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HotfixMods.Db2Provider.WowToolsFiles.Clients
{
    public class Db2Client : IDb2Provider
    {
        public async Task<T?> GetAsync<T>(Expression<Func<T, bool>> predicate)
            where T : IDb2
        {
            return (await ReadFiles(predicate, true)).FirstOrDefault();
        }

        public async Task<IEnumerable<T>> GetManyAsync<T>(Expression<Func<T, bool>> predicate)
            where T : IDb2
        {
            return await ReadFiles(predicate, false);
        }

        async Task<IEnumerable<T>> ReadFiles<T>(Expression<Func<T, bool>> predicate, bool firstOnly)
        {
            var compiledPredicate = predicate.Compile();
            var fileName = typeof(T).Name.ToLower();

            // TODO: Find a less hacky way for this...
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            int end = baseDirectory.LastIndexOf("\\HotfixMods\\");
            baseDirectory = baseDirectory.Substring(0, end + 12);

            var filePath = $"{baseDirectory}HotfixMods.Db2Provider.WowToolsFiles\\Files\\{fileName}.csv";

            var result = new List<T>();
            var headers = new List<string>();
            var row = 0;

            // Some columns contains the comma-delimiter symbol, and are messing up
            // the default string.Split(','). Ex. name of an item.
            var regex = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

            foreach (string line in await File.ReadAllLinesAsync(@filePath))
            {
                try
                {
                    var columns = regex.Split(line);
                    if (row == 0)
                    {
                        row++;
                        foreach (var header in columns)
                        {
                            headers.Add(header.Replace("[", "").Replace("]", "").Replace("_lang", "").Replace("_", ""));
                        }
                    }
                    else
                    {
                        row++;
                        var lineJsonObject = new JObject();
                        for (int i = 0; i < headers.Count; i++)
                        {

                            lineJsonObject.Add(headers[i], columns[i]);
                        }

                        var lineObject = lineJsonObject.ToObject<T>();
                        if (lineObject == null || !compiledPredicate(lineObject))
                            continue;

                        result.Add(lineObject);

                        if (firstOnly && result.Count == 1)
                            return result;
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error on line {row + 1} in file {fileName}. Error message: {ex.Message}. This entity is being skipped.");
                }
            }

            return result;
        }

    }
}
