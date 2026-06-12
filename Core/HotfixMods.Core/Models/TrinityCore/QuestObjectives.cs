using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [WorldSchema]
    public class QuestObjectives
    {
        [IndexField]
        public uint ID { get; set; } = 0;
        public uint QuestID { get; set; } = 0;
        public byte Type { get; set; } = 0;
        public byte Order { get; set; } = 0;
        public sbyte StorageIndex { get; set; } = 0;
        public int ObjectID { get; set; } = 0;
        public int Amount { get; set; } = 0;
        public int ConditionalAmount { get; set; } = 0;
        public uint Flags { get; set; } = 0;
        public uint Flags2 { get; set; } = 0;
        public float ProgressBarWeight { get; set; } = 0;
        public int ParentObjectiveID { get; set; } = 0;
        public byte Visible { get; set; } = 1;
        public string Description { get; set; } = "";
        public int VerifiedBuild { get; set; } = -1;
    }
}
