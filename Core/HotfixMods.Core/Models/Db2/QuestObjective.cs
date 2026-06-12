using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class QuestObjective
    {
        [IndexField]
        public int ID { get; set; } = 0;
        public string Description { get; set; } = "";
        public int Type { get; set; } = 0;
        public int Amount { get; set; } = 0;
        public int ObjectID { get; set; } = 0;
        public int OrderIndex { get; set; } = 0;
        public int Flags { get; set; } = 0;
        public int StorageIndex { get; set; } = 0;
        public int Field_12_0_0_63534_007 { get; set; } = 0;
        public int QuestID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
