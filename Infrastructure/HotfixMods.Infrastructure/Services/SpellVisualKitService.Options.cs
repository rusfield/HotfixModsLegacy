using HotfixMods.Core.Enums.Db2;
using HotfixMods.Infrastructure.Extensions;

namespace HotfixMods.Infrastructure.Services
{
    public partial class SpellVisualKitService
    {
        static Dictionary<T, string> GetAnimationOptions<T>()
            where T : notnull
        {
            return Enum.GetValues<AnimationDataId>().ToDictionary(key => (T)Convert.ChangeType((int)key, typeof(T)), value => value.ToDisplayString());
        }

        public async Task<Dictionary<short, string>> GetAnimationIdOptionsAsync()
        {
            return GetAnimationOptions<short>();
        }

        #region SpellVisualKitEffect
        public async Task<Dictionary<int, string>> GetEffectOptionsAsync()
        {
            return Enum.GetValues<SpellVisualKitEffect_EffectType>().ToDictionary(key => (int)key, value => value.ToDisplayString());
        }
        #endregion

        public async Task<Dictionary<int, string>> GetSpellVisualEffectNameTypeOptionsAsync()
        {
            return Enum.GetValues<SpellVisualEffectNameType>().ToDictionary(key => (int)key, value => value.ToDisplayString());
        }
    }
}
