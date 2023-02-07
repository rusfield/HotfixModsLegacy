namespace HotfixMods.Infrastructure.InfoModels
{
    public class SpellAuraOptionsInfo : IInfoModel
    {
        public string DifficultyID { get; set; } = "TODO";
        public string CumulativeAura { get; set; } = "TODO";
        public string ProcCategoryRecovery { get; set; } = "TODO";
        public string ProcChance { get; set; } = "TODO";
        public string ProcCharges { get; set; } = "TODO";
        public string SpellProcsPerMinuteID { get; set; } = "TODO";
        public string ProcTypeMask0 { get; set; } = "TODO";
        public string ProcTypeMask1 { get; set; } = "TODO";
        public string SpellID { get; set; } = "TODO";

        public string ModelInfo { get; set; } = "TODO";
        public bool IsRequired { get; set; } = false;
    }
}
