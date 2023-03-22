using HotfixMods.Core.Enums.Db2;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Infrastructure.Extensions;
using System.Collections.Immutable;

namespace HotfixMods.Infrastructure.Services
{
    public partial class AnimKitService
    {
        public async Task<Dictionary<ushort, string>> GetPriorityOptionsAsync()
        {
            var options = await GetOptionsAsync<ushort>("AnimKitPriority", "Priority");
            foreach (var key in options.Keys)
                options[key] = $"{key} ➜ Pri {options[key].Split(" ➜ ")[1]}";

            options[0] = "0 ➜ Disabled";
            return options.SortByValue<ushort>(false);
        }

        public async Task<Dictionary<byte, string>> GetBoneSetOptionsAsync()
        {
            return await GetOptionsAsync<byte>("AnimKitBoneSet", "Name");
        }
    }
}
