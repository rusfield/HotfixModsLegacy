namespace HotfixMods.Infrastructure.InfoModels
{
    public class SpellCooldownsInfo : IInfoModel
    {
        public string DifficultyId { get; set; } = "TODO";
        public string CategoryRecoveryTime { get; set; } = "TODO";
        public string RecoveryTime { get; set; } = "TODO";
        public string StartRecoveryTime { get; set; } = "TODO";
        public string AuraSpellId { get; set; } = "TODO";
        public string SpellId { get; set; } = "TODO";

        public string ModelInfo { get; set; } = "TODO";
        public bool IsRequired { get; set; } = false;
    }
}