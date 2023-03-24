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
            var options = await GetDb2OptionsAsync<ushort>("AnimKitPriority", "Priority");
            options[0] = "Disabled";
            return options.SortByValue<ushort>(false);
        }

        public async Task<Dictionary<byte, string>> GetBoneSetOptionsAsync()
        {
            return await GetDb2OptionsAsync<byte>("AnimKitBoneSet", "Name");
        }
    }
}
