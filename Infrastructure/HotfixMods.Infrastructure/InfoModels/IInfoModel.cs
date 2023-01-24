namespace HotfixMods.Infrastructure.InfoModels
{
    public interface IInfoModel
    {
        public string ModelInfo { get; set; }
        public bool IsRequired { get; set; }
    }
}
