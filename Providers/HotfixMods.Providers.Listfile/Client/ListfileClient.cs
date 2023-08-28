using HotfixMods.Providers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Providers.Listfile.Client
{
    public partial class ListfileClient : IListfileProvider
    {
        public string ListFilePath { get; set; } = "/";

        public ListfileClient(string listfilePath)
        {
            ListFilePath = listfilePath;
        }

        public async Task<Dictionary<TKey, string>> GetIconsAsync<TKey>() 
            where TKey : notnull
        {
            return await ReadFileAsync<TKey>("interface/icons/", "blp");
        }

        public async Task<Dictionary<TKey, string>> GetTexturesAsync<TKey>()
            where TKey : notnull
        {
            return await ReadFileAsync<TKey>(null, "blp");
        }

        public async Task<Dictionary<TKey, string>> GetModelsAsync<TKey>()
            where TKey : notnull
        {
            return await ReadFileAsync<TKey>(null, "m2", true);
        }
    }
}
