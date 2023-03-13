using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemSetSpell
    {
        public int ID { get; set; }
        public ushort ChrSpecID { get; set; }
        public uint SpellID { get; set; }
        public byte Threshold { get; set; }
        public int ItemSetID { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
