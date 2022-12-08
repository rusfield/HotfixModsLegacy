using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class RopeEffect
    {
        public int Id { get; set; } = 1;
        public int Flags { get; set; } = 0;
        public sbyte Field_8_3_0_32712_001 { get; set; } = 0;
        public int Field_8_3_0_32712_002 { get; set; } = 0;
        public sbyte Field_8_3_0_32712_003 { get; set; } = -1;
        public int NumberOfJoints { get; set; } = 0;
        public decimal Field_8_3_0_32712_005 { get; set; } = 0;
        public decimal Stiffness { get; set; } = 0;
        public decimal Damping { get; set; } = 0;
        public int IntegrateAtJointIndex { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
