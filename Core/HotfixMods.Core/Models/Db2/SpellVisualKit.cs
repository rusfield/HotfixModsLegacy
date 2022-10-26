using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellVisualKit
    {
        public int Id { get; set; }
        public sbyte ClutterLevel { get; set; }
        public int FallbackSpellVisualKitId { get; set; }
        public ushort DelayMin { get; set; }
        public ushort DelayMax { get; set; }
        public int Flags1 { get; set; }
        public int Flags2 { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
