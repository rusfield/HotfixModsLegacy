using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellVisualAnim
    {
        
        public int ID { get; set; } = 0;
        public int InitialAnimID { get; set; } = -1;
        public int LoopAnimID { get; set; } = -1; 
        public ushort AnimKitID { get; set; } = 0; 
        public int VerifiedBuild { get; set; } = -1;
    }
}
