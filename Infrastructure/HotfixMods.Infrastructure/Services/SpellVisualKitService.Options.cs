using HotfixMods.Core.Enums.Db2;
using HotfixMods.Infrastructure.Extensions;

namespace HotfixMods.Infrastructure.Services
{
    public partial class SpellVisualKitService
    {
        #region SpellVisualKitEffect
        public async Task<Dictionary<int, string>> GetEffectOptionsAsync()
        {
            return Enum.GetValues<SpellVisualKitEffect_EffectType>().ToDictionary(key => (int)key, value => value.ToDisplayString());
        }
        #endregion
    }
}
