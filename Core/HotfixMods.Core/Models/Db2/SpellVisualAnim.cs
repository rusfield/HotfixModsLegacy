using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellVisualAnim
    {
        [IndexField]
        public int Id { get; set; } = 0;
        public int InitialAnimId { get; set; } = -1;
        public int LoopAnimId { get; set; } = -1; 
        public ushort AnimKitId { get; set; } = 0; 
        public int VerifiedBuild { get; set; } = -1;
    }
}
