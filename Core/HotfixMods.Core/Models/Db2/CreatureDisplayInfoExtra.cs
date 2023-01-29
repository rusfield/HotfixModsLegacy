using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class CreatureDisplayInfoExtra
    {
        [IndexField]
        public uint Id { get; set; } = 0;
        public sbyte DisplayRaceId { get; set; } = 0;
        public sbyte DisplaySexId { get; set; } = 0;
        public sbyte DisplayClassId { get; set; } = 0;
        public sbyte Flags { get; set; } = 0;
        public int BakeMaterialResourcesId { get => 0; set { } } // Should always be 0
        public int HDBakeMaterialResourcesId { get => 0; set { } } // Should always be 0
        public int VerifiedBuild { get; set; } = -1;
    }

}
