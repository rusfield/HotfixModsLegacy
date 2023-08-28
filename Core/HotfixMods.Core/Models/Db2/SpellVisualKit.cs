using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellVisualKit
    {
        
        public int ID { get; set; } = 0;
        public sbyte ClutterLevel { get; set; } = 0;
        public int FallbackSpellVisualKitID { get; set; } = 0;
        public ushort DelayMin { get; set; } = 0;
        public ushort DelayMax { get; set; } = 0;
        public int Flags0 { get; set; } = 0;
        public int Flags1 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
