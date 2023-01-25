using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ClientSceneEffect
    {
        [IndexField]
        public int Id { get; set; } = 0;
        public int SceneScriptPackageId { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
