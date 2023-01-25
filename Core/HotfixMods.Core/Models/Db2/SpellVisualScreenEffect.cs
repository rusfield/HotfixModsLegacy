using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellVisualScreenEffect
    {
        [IndexField]
        public int Id { get; set; } = 0;
        public int ScreenEffectId { get; set; } = 0;
        public int ScreenEffectTypeId { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
