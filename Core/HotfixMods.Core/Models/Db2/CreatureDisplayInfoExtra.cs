using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class CreatureDisplayInfoExtra
    {
        [IndexField]
        public int Id { get; set; } = 0;
        public sbyte DisplayRaceId { get; set; } = 0;
        public sbyte DisplaySexId { get; set; } = 0;
        public sbyte DisplayClassId { get; set; } = 0;
        public sbyte Flags { get; set; } = 0;
        public int BakeMaterialResourcesId { get; set; } = 0;
        public int HDBakeMaterialResourcesId { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
