using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemSet
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public uint SetFlags { get; set; }
        public uint RequiredSkill { get; set; }
        public ushort RequiredSkillRank { get; set; }
        public uint ItemID1 { get; set; }
        public uint ItemID2 { get; set; }
        public uint ItemID3 { get; set; }
        public uint ItemID4 { get; set; }
        public uint ItemID5 { get; set; }
        public uint ItemID6 { get; set; }
        public uint ItemID7 { get; set; }
        public uint ItemID8 { get; set; }
        public uint ItemID9 { get; set; }
        public uint ItemID10 { get; set; }
        public uint ItemID11 { get; set; }
        public uint ItemID12 { get; set; }
        public uint ItemID13 { get; set; }
        public uint ItemID14 { get; set; }
        public uint ItemID15 { get; set; }
        public uint ItemID16 { get; set; }
        public uint ItemID17 { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
