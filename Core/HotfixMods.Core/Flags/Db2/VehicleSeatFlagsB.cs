namespace HotfixMods.Core.Flags.Db2
{
    [Flags]
    public enum VehicleSeatFlagsB : int
    {
        NONE = 0,
        USABLE_FORCED = 0x00000002,
        TARGETS_IN_RAIDUI = 0x00000008,
        EJECTABLE = 0x00000020,
        USABLE_FORCED_2 = 0x00000040,
        USABLE_FORCED_3 = 0x00000100,
        PASSENGER_MIRRORS_ANIMS = 0x00010000,
        KEEP_PET = 0x00020000,
        USABLE_FORCED_4 = 0x02000000,
        CAN_SWITCH = 0x04000000,
        VEHICLE_PLAYERFRAME_UI = unchecked((int)0x80000000),
    }
}
