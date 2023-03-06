using HotfixMods.Core.Flags.Db2;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Infrastructure.Extensions;

namespace HotfixMods.Infrastructure.Services
{
    public partial class SoundKitService
    {
        public async Task<Dictionary<int, string>> GetFlagOptionsAsync()
        {
            return Enum.GetValues<SoundkitFlags>().ToDictionary(key => (int)key, value => value.ToDisplayString());
        }

        public async Task<Dictionary<int, string>> GetSoundTypeOptionsAsync()
        {
            return Enum.GetValues<SoundkitSoundType>().ToDictionary(key => (int)key, value => value.ToDisplayString());
        }
    }
}
