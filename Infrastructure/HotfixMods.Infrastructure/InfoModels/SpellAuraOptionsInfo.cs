namespace HotfixMods.Infrastructure.InfoModels
{
    public class SpellAuraOptionsInfo : IInfoModel
    {
        public string DifficultyId { get; set; } = "TODO";
        public string CumulativeAura { get; set; } = "TODO";
        public string ProcCategoryRecovery { get; set; } = "TODO";
        public string ProcChance { get; set; } = "TODO";
        public string ProcCharges { get; set; } = "TODO";
        public string SpellProcsPerMinuteId { get; set; } = "TODO";
        public string ProcTypeMask1 { get; set; } = "TODO";
        public string ProcTypeMask2 { get; set; } = "TODO";
        public string SpellId { get; set; } = "TODO";

        public string ModelInfo { get; set; } = "TODO";
        public bool IsRequired { get; set; } = false;
    }
}
