using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class Vehicle
    {
        [IndexField]
        public int ID { get; set; } = 0;
        public int Flags { get; set; } = 0;
        public int FlagsB { get; set; } = 0;
        public decimal TurnSpeed { get; set; } = 0;
        public decimal PitchSpeed { get; set; } = 0;
        public decimal PitchMin { get; set; } = 0;
        public decimal PitchMax { get; set; } = 0;
        public decimal MouseLookOffsetPitch { get; set; } = 0;
        public decimal CameraFadeDistScalarMin { get; set; } = 0;
        public decimal CameraFadeDistScalarMax { get; set; } = 0;
        public decimal CameraPitchOffset { get; set; } = 0;
        public decimal FacingLimitRight { get; set; } = 0;
        public decimal FacingLimitLeft { get; set; } = 0;
        public decimal CameraYawOffset { get; set; } = 0;
        public int VehicleUIIndicatorID { get; set; } = 0;
        public int MissileTargetingID { get; set; } = 0;
        public int VehiclePOITypeID { get; set; } = 0;
        public ushort SeatID0 { get; set; } = 0;
        public ushort SeatID1 { get; set; } = 0;
        public ushort SeatID2 { get; set; } = 0;
        public ushort SeatID3 { get; set; } = 0;
        public ushort SeatID4 { get; set; } = 0;
        public ushort SeatID5 { get; set; } = 0;
        public ushort SeatID6 { get; set; } = 0;
        public ushort SeatID7 { get; set; } = 0;
        public ushort PowerDisplayID0 { get; set; } = 0;
        public ushort PowerDisplayID1 { get; set; } = 0;
        public ushort PowerDisplayID2 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
