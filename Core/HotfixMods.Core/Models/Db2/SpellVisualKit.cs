using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellVisualKit
    {
        [IndexField]
        public int ID { get; set; } = 0;
        public int ClutterLevel { get; set; } = 0;
        public int FallbackSpellVisualKitID { get; set; } = 0;
        public ushort DelayMin { get; set; } = 0;
        public ushort DelayMax { get; set; } = 0;
        public int MinimumSpellVisualDensityFilterType { get; set; } = 0;
        public int MinimumSpellVisualDensityFilterParam { get; set; } = 0;
        public int ReducedSpellVisualDensityFilterType { get; set; } = 0;
        public int ReducedSpellVisualDensityFilterParam { get; set; } = 0;
        public int Flags0 { get; set; } = 0;
        public int Flags1 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
