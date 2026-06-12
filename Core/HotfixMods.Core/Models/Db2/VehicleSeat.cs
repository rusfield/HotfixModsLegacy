using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class VehicleSeat
    {
        [IndexField]
        public int ID { get; set; } = 0;
        public decimal AttachmentOffset0 { get; set; } = 0;
        public decimal AttachmentOffset1 { get; set; } = 0;
        public decimal AttachmentOffset2 { get; set; } = 0;
        public decimal CameraOffset0 { get; set; } = 0;
        public decimal CameraOffset1 { get; set; } = 0;
        public decimal CameraOffset2 { get; set; } = 0;
        public int Flags { get; set; } = 0;
        public int FlagsB { get; set; } = 0;
        public int FlagsC { get; set; } = 0;
        public int AttachmentID { get; set; } = 0;
        public decimal EnterPreDelay { get; set; } = 0;
        public decimal EnterSpeed { get; set; } = 0;
        public decimal EnterGravity { get; set; } = 0;
        public decimal EnterMinDuration { get; set; } = 0;
        public decimal EnterMaxDuration { get; set; } = 0;
        public decimal EnterMinArcHeight { get; set; } = 0;
        public decimal EnterMaxArcHeight { get; set; } = 0;
        public short EnterAnimStart { get; set; } = 0;
        public short EnterAnimLoop { get; set; } = 0;
        public short RideAnimStart { get; set; } = 0;
        public short RideAnimLoop { get; set; } = 0;
        public short RideUpperAnimStart { get; set; } = 0;
        public short RideUpperAnimLoop { get; set; } = 0;
        public decimal ExitPreDelay { get; set; } = 0;
        public decimal ExitSpeed { get; set; } = 0;
        public decimal ExitGravity { get; set; } = 0;
        public decimal ExitMinDuration { get; set; } = 0;
        public decimal ExitMaxDuration { get; set; } = 0;
        public decimal ExitMinArcHeight { get; set; } = 0;
        public decimal ExitMaxArcHeight { get; set; } = 0;
        public short ExitAnimStart { get; set; } = 0;
        public short ExitAnimLoop { get; set; } = 0;
        public short ExitAnimEnd { get; set; } = 0;
        public short VehicleEnterAnim { get; set; } = 0;
        public sbyte VehicleEnterAnimBone { get; set; } = 0;
        public short VehicleExitAnim { get; set; } = 0;
        public sbyte VehicleExitAnimBone { get; set; } = 0;
        public short VehicleRideAnimLoop { get; set; } = 0;
        public sbyte VehicleRideAnimLoopBone { get; set; } = 0;
        public sbyte PassengerAttachmentID { get; set; } = 0;
        public decimal PassengerYaw { get; set; } = 0;
        public decimal PassengerPitch { get; set; } = 0;
        public decimal PassengerRoll { get; set; } = 0;
        public decimal VehicleEnterAnimDelay { get; set; } = 0;
        public decimal VehicleExitAnimDelay { get; set; } = 0;
        public sbyte VehicleAbilityDisplay { get; set; } = 0;
        public uint EnterUISoundID { get; set; } = 0;
        public uint ExitUISoundID { get; set; } = 0;
        public int UiSkinFileDataID { get; set; } = 0;
        public decimal CameraEnteringDelay { get; set; } = 0;
        public decimal CameraEnteringDuration { get; set; } = 0;
        public decimal CameraExitingDelay { get; set; } = 0;
        public decimal CameraExitingDuration { get; set; } = 0;
        public decimal CameraPosChaseRate { get; set; } = 0;
        public decimal CameraFacingChaseRate { get; set; } = 0;
        public decimal CameraEnteringZoom { get; set; } = 0;
        public decimal CameraSeatZoomMin { get; set; } = 0;
        public decimal CameraSeatZoomMax { get; set; } = 0;
        public int EnterAnimKitID { get; set; } = 0;
        public int RideAnimKitID { get; set; } = 0;
        public int ExitAnimKitID { get; set; } = 0;
        public int VehicleEnterAnimKitID { get; set; } = 0;
        public int VehicleRideAnimKitID { get; set; } = 0;
        public int VehicleExitAnimKitID { get; set; } = 0;
        public short CameraModeID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
