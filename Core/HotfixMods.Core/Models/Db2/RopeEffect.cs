using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class RopeEffect
    {
        public int Id { get; set; } = 1;
        public int Flags { get; set; }
        public sbyte Field_8_3_0_32712_001 { get; set; }
        public int Field_8_3_0_32712_002 { get; set; }
        public sbyte Field_8_3_0_32712_003 { get; set; }
        public int NumberOfJoints { get; set; }
        public decimal Field_8_3_0_32712_005 { get; set; }
        public decimal Stiffness { get; set; }
        public decimal Damping { get; set; }
        public int IntegrateAtJointIndex { get; set; }
        public int VerifiedBuild { get; set; } = -1;
    }
}
