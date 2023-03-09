using HotfixMods.Core.Enums.Db2;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Infrastructure.Extensions;

namespace HotfixMods.Infrastructure.Services
{
    public partial class AnimKitService
    {
        public async Task<Dictionary<ushort, string>> GetAnimKitPriorityOptionsAsync()
        {
            var results = new Dictionary<ushort, string>();
            var options = await GetAsync<AnimKitPriority>(true);
            foreach(var option in options.OrderByDescending(o => o.Priority))
            {
                results.Add((ushort)option.ID, option.Priority.ToString());
            }
            return results;
        }

        public async Task<Dictionary<byte, string>> GetBoneSetOptionsAsync()
        {
            var results = new Dictionary<byte, string>();
            var options = await GetAsync<AnimKitBoneSet>(true);
            foreach(var option in options)
            {
                results.Add((byte)option.ID, option.Name);
            }
            return results;
        }
    }
}
