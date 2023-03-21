using HotfixMods.Core.Interfaces;
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
        public async Task<Dictionary<TKey, string>> GetIconsAsync<TKey>() where TKey : notnull
        {
            return await ReadFileAsync<TKey>("interface", "interface/icons");
        }
    }
}
