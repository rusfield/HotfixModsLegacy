using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellVisualScreenEffect
    {
        public int Id { get; set; } = 1;
        public int ScreenEffectId { get; set; }
        public int ScreenEffectTypeId { get; set; }
        public int VerifiedBuild { get; set; } = -1;
    }
}
