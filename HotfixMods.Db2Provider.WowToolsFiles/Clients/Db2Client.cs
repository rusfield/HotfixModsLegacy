using HotfixMods.Core.Models.Interfaces;
using HotfixMods.Core.Providers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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

            var filePath = $"{Directory.GetParent(Directory.GetCurrentDirectory())?.FullName}\\CreatureCreator.Db2Provider.WowToolsFiles\\Files\\{fileName}.csv";

            var result = new List<T>();
            var headers = new List<string>();
            var row = -1;
            try
            {
                foreach (string line in await File.ReadAllLinesAsync(@filePath))
                {
                    row++;
                    var columns = line.Split(',');
                    if (row == 0)
                    {
                        foreach (var header in columns)
                        {
                            headers.Add(header.Replace("[", "").Replace("]", "").Replace("_lang", ""));
                        }
                    }
                    else
                    {
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
            }
            catch (Exception ex)
            {
                throw new Exception($"Crashed on line {row + 1} in file {fileName}. Error message: {ex.Message}");
            }

            return result;
        }

    }
}
