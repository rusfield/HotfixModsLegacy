namespace HotfixMods.Infrastructure.InfoModels
{
    public class SpellEffectEmissionInfo : IInfoModel
    {
        public string EmissionRate { get; set; } = "TODO";
        public string ModelScale { get; set; } = "TODO";
        public string AreaModelID { get; set; } = "TODO";
        public string Flags { get; set; } = "TODO";

        public string ModelInfo { get; set; } = "TODO";
        public bool IsRequired { get; set; } = false;
    }
}