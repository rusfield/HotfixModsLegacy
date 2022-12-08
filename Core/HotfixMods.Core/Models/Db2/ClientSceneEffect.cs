using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ClientSceneEffect
    {
        public int Id { get; set; } = 1;
        public int SceneScriptPackageId { get; set; }
        public int VerifiedBuild { get; set; } = -1;
    }
}
