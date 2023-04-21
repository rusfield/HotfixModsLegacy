using HotfixMods.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Providers.Listfile.Client
{
    public partial class ListfileClient : IListfileProvider
    {
        public bool CacheResults { get; set; } = true;

        IMemoryCache _cache;
        MemoryCacheEntryOptions _cacheOptions;
        string _listfilePath;

        public ListfileClient(string listfilePath)
        {
            _cache = new MemoryCache(new MemoryCacheOptions()
            {
                TrackLinkedCacheEntries = true
            });
            _cacheOptions = new()
            {
                SlidingExpiration = TimeSpan.FromMinutes(15)
            };
            _listfilePath = listfilePath;
        }

        public async Task<Dictionary<TKey, string>> GetIconsAsync<TKey>() 
            where TKey : notnull
        {
            return await ReadFileAsync<TKey>("interface/icons/", "blp");
        }

        public async Task<Dictionary<TKey, string>> GetItemTexturesAsync<TKey>()
            where TKey : notnull
        {
            return await ReadFileAsync<TKey>("item/", "blp");
        }

        public async Task<Dictionary<TKey, string>> GetModelFilesAsync<TKey>()
            where TKey : notnull
        {
            return await ReadFileAsync<TKey>(null, "m2", true);
        }
    }
}
