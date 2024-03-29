﻿using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    // TODO: Should get most values from ItemSparse?

    [HotfixesSchema]
    public class ItemSearchName
    {
        [IndexField]
        public int ID { get; set; } = 0;
        public long AllowableRace { get; set; } = 0;
        [LocalizedString]
        public string Display { get; set; } = "";
        public byte OverallQualityID { get; set; } = 0;
        public int ExpansionID { get; set; } = 0;
        public ushort MinFactionID { get; set; } = 0;
        public int MinReputation { get; set; } = 0;
        public int AllowableClass { get; set; } = 0;
        public sbyte RequiredLevel { get; set; } = 0;
        public ushort RequiredSkill { get; set; } = 0;
        public ushort RequiredSkillRank { get; set; } = 0;
        public uint RequiredAbility { get; set; } = 0;
        public ushort ItemLevel { get; set; } = 0;
        public int Flags0 { get; set; } = 0;
        public int Flags1 { get; set; } = 0;
        public int Flags2 { get; set; } = 0;
        public int Flags3 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
