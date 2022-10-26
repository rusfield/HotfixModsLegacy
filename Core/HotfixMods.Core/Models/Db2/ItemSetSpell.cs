using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemSetSpell
    {
        public int Id { get; set; }
        public ushort ChrSpecId { get; set; }
        public uint SpellId { get; set; }
        public byte Threshold { get; set; }
        public int ItemSetId { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
