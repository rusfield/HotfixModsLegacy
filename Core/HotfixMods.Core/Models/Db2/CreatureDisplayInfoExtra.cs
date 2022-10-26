using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class CreatureDisplayInfoExtra
    {
        public int Id { get; set; }
        public sbyte DisplayRaceId { get; set; }
        public sbyte DisplaySexId { get; set; }
        public sbyte DisplayClassId { get; set; }
        public sbyte Flags { get; set; }
        public int BakeMaterialResourcesId { get; set; }
        public int HDBakeMaterialResourcesId { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
