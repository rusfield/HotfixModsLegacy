namespace HotfixMods.Infrastructure.InfoModels
{
    public class SpellInfo : IInfoModel
    {
        public string NameSubtext = "TODO";
        public string Description = "The text in the tooltip that appears when you hover over the spell.";
        public string AuraDescription = "The text in the tooltip that appears when you hover over the icon buff/debuff.";

        public string ModelInfo { get; set; } = "TODO";
        public bool IsRequired { get; set; } = false;
    }
}
