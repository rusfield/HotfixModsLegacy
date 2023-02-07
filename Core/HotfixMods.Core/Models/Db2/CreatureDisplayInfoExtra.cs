using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class CreatureDisplayInfoExtra
    {
        [IndexField]
        public uint ID { get; set; } = 0;
        public sbyte DisplayRaceID { get; set; } = 0;
        public sbyte DisplaySexID { get; set; } = 0;
        public sbyte DisplayClassID { get; set; } = 0;
        public sbyte Flags { get; set; } = 0;
        public int BakeMaterialResourcesID { get => 0; set { } } // Should always be 0
        public int HDBakeMaterialResourcesID { get => 0; set { } } // Should always be 0
        public int VerifiedBuild { get; set; } = -1;
    }

}
