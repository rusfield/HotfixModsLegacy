using HotfixMods.Core.Enums.Db2;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Infrastructure.Extensions;
using System.Collections.Immutable;

namespace HotfixMods.Infrastructure.Services
{
    public partial class AnimKitService
    {
        public async Task<Dictionary<uint, string>> GetPriorityOptionsAsync()
        {
            var options = await GetDb2OptionsAsync<uint>("AnimKitPriority", "Priority");
            options[0] = "10000"; // Sort to top
            options = options.SortByValue(false);
            options[0] = "Disabled";
            return options;
        }

        public async Task<Dictionary<uint, string>> GetBoneSetOptionsAsync()
        {
            return await GetDb2OptionsAsync<uint>("AnimKitBoneSet", "Name");
        }
    }
}
