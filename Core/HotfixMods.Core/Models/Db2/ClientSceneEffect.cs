using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ClientSceneEffect
    {
        [IndexField]
        public uint ID { get; set; } = 0;
        public int SceneScriptPackageID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
