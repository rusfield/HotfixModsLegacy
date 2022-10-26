using HotfixMods.Core.Attributes;
using HotfixMods.Core.Enums.Db2;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class CreatureDisplayInfoExtra
    {
        public int Id { get; set; } = 0;
        public ChrRaceId DisplayRaceId { get; set; } = ChrRaceId.DEFAULT;
        public CreatureDisplayInfoExtraSexId DisplaySexId { get; set; } = CreatureDisplayInfoExtraSexId.MALE;
        public sbyte DisplayClassId { get; set; } = 0;
        public sbyte Flags { get; set; } = 0;
        public int BakeMaterialResourcesId { get; set; } = 0;
        public int HDBakeMaterialResourcesId { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
