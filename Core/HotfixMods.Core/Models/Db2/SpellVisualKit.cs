using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellVisualKit
    {
        [IndexField]
        public int Id { get; set; } = 0;
        public sbyte ClutterLevel { get; set; } = 0;
        public int FallbackSpellVisualKitId { get; set; } = 0;
        public ushort DelayMin { get; set; } = 0;
        public ushort DelayMax { get; set; } = 0;
        public int Flags1 { get; set; } = 0;
        public int Flags2 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
