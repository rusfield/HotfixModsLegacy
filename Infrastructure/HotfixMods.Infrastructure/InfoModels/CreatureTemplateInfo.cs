namespace HotfixMods.Infrastructure.InfoModels
{
    public class CreatureTemplateInfo : IInfoModel
    {
        public string ModelInfo { get; set; } = "Test";
        public bool IsRequired { get; set; } = true;
    }
}
