using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemSet
    {
        [IndexField]
        public int ID { get; set; } = 0;
        [LocalizedString]
        public string Name { get; set; } = "";
        public int SetFlags { get; set; } = 0;
        public uint RequiredSkill { get; set; } = 0;
        public ushort RequiredSkillRank { get; set; } = 0;
        public uint ItemID0 { get; set; } = 0;
        public uint ItemID1 { get; set; } = 0;
        public uint ItemID2 { get; set; } = 0;
        public uint ItemID3 { get; set; } = 0;
        public uint ItemID4 { get; set; } = 0;
        public uint ItemID5 { get; set; } = 0;
        public uint ItemID6 { get; set; } = 0;
        public uint ItemID7 { get; set; } = 0;
        public uint ItemID8 { get; set; } = 0;
        public uint ItemID9 { get; set; } = 0;
        public uint ItemID10 { get; set; } = 0;
        public uint ItemID11 { get; set; } = 0;
        public uint ItemID12 { get; set; } = 0;
        public uint ItemID13 { get; set; } = 0;
        public uint ItemID14 { get; set; } = 0;
        public uint ItemID15 { get; set; } = 0;
        public uint ItemID16 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
