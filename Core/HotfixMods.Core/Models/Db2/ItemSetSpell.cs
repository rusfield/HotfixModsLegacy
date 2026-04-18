using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemSetSpell
    {
        [IndexField]
        public int ID { get; set; } = 0;
        public ushort ChrSpecID { get; set; } = 0;
        public uint SpellID { get; set; } = 0;
        public ushort TraitSubTreeID { get; set; } = 0;
        public byte Threshold { get; set; } = 0;
        public int ItemSetID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
