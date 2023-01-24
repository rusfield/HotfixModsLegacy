namespace HotfixMods.Infrastructure.InfoModels
{
    public class SpellNameInfo : IInfoModel
    {
        public string Name = "The name of the spell.";
        public string ModelInfo { get; set; } = "TODO";
        public bool IsRequired { get; set; } = false;
    }
}
