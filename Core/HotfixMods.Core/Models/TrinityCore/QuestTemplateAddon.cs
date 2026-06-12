using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [WorldSchema]
    public class QuestTemplateAddon
    {
        [IndexField]
        public uint ID { get; set; } = 0;
        public byte MaxLevel { get; set; } = 0;
        public uint AllowableClasses { get; set; } = 0;
        public uint SourceSpellID { get; set; } = 0;
        public int PrevQuestID { get; set; } = 0;
        public uint NextQuestID { get; set; } = 0;
        public int ExclusiveGroup { get; set; } = 0;
        public int BreadcrumbForQuestId { get; set; } = 0;
        public uint RewardMailTemplateID { get; set; } = 0;
        public uint RewardMailDelay { get; set; } = 0;
        public ushort RequiredSkillID { get; set; } = 0;
        public ushort RequiredSkillPoints { get; set; } = 0;
        public ushort RequiredMinRepFaction { get; set; } = 0;
        public ushort RequiredMaxRepFaction { get; set; } = 0;
        public int RequiredMinRepValue { get; set; } = 0;
        public int RequiredMaxRepValue { get; set; } = 0;
        public byte ProvidedItemCount { get; set; } = 0;
        public byte SpecialFlags { get; set; } = 0;
        public string ScriptName { get; set; } = "";
    }
}
