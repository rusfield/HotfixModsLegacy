namespace HotfixMods.Infrastructure.InfoModels
{
    public class ClientSceneEffectInfo : IInfoModel
    {
        public string SceneScriptPackageId { get; set; } = "TODO";

        public string ModelInfo { get; set; } = "TODO";
        public bool IsRequired { get; set; } = false;
    }
}