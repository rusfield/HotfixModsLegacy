using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [WorldSchema]
    public class SpellCustomAttr
    {
        [IndexField]
        public uint Entry { get; set; } = 0;
        public uint Attributes { get; set; } = 0;
    }
}
