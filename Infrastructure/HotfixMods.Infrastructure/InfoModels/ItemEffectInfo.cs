namespace HotfixMods.Infrastructure.InfoModels
{
    public class ItemEffectInfo : IInfoModel
    {
        public string LegacySlotIndex { get; set; } = "TODO";
        public string TriggerType { get; set; } = "TODO";
        public string Charges { get; set; } = "TODO";
        public string CoolDownMSec { get; set; } = "TODO";
        public string CategoryCoolDownMSec { get; set; } = "TODO";
        public string SpellCategoryID { get; set; } = "TODO";
        public string SpellID { get; set; } = "TODO";
        public string ChrSpecializationID { get; set; } = "TODO";

        public string ModelInfo { get; set; } = "TODO";
        public bool IsRequired { get; set; } = false;
    }
}
