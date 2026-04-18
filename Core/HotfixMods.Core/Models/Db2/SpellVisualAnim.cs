using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellVisualAnim
    {
        [IndexField]
        public int ID { get; set; } = 0;
        public short InitialAnimID { get; set; } = -1;
        public short LoopAnimID { get; set; } = -1;
        public ushort AnimKitID { get; set; } = 0;
        public int Field_12_0_0_63967_003 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
