using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    // TODO: Should get most values from ItemSparse?

    [HotfixesSchema]
    public class ItemSearchName
    {
        public int Id { get; set; }
        public long AllowableRace { get; set; }
        public string Display { get; set; }
        public byte OverallQualityId { get; set; }
        public int ExpansionId { get; set; }
        public ushort MinFactionId { get; set; }
        public int MinReputation { get; set; }
        public int AllowableClass { get; set; }
        public sbyte RequiredLevel { get; set; }
        public ushort RequiredSkill { get; set; }
        public ushort RequiredSkillRank { get; set; }
        public uint RequiredAbility { get; set; }
        public ushort ItemLevel { get; set; }
        public int Flags1 { get; set; }
        public int Flags2 { get; set; }
        public int Flags3 { get; set; }
        public int Flags4 { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
