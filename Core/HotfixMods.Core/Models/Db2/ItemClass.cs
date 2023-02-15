using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemClass
    {
        public uint ID { get; set; } = 1;
        public string ClassName { get; set; } = "";
        public sbyte ClassID { get; set; } = 0;
        public decimal PriceModifier { get; set; } = 0.25M; 
        public byte Flags { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
