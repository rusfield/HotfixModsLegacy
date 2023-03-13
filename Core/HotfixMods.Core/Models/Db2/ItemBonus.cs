using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema] // Client only?
    public class ItemBonus
    {
        public int ID { get; set; }
        public int Value0 { get; set; }
        public int Value1 { get; set; }
        public int Value2 { get; set; }
        public int Value3 { get; set; }
        public ushort ParentItemBonusListID { get; set; }
        public byte Type { get; set; }
        public byte OrderIndex { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
