
namespace HotfixMods.Infrastructure.InfoModels
{
    public class HotfixModsEntityInfo : IInfoModel
    {
        public string Id = "Choose whether you want to save a new or update an existing entity based on ID.\r\n\r\nAn existing entity can at any time be saved as a new entity, however due to all the foreign keys an entity can only be updated if opened directly.";

        public string ModelInfo { get; set; } = "TODO";
        public bool IsRequired { get; set; } = true;
    }
}
