namespace HotfixMods.Core.Flags.Db2
{
    [Flags]
    public enum AnimKitSegmentSegmentFlags : ushort
    {
        /// <summary>
        /// Observed on some looping / held segments, but the exact meaning is still unknown.
        /// </summary>
        UNKNOWN_1 = 1,

        /// <summary>
        /// Enables ForcedVariation.
        /// With this flag enabled, ForcedVariation appears to select a concrete variation index:
        /// 0 = first variation, 1 = second variation, and so on.
        /// Without this flag, ForcedVariation does not appear to matter.
        /// </summary>
        USE_FORCED_VARIATION = 2,

        /// <summary>
        /// Exact behavior still unknown.
        /// Reverse playback is already well explained by negative Speed, so this flag is probably not simply "play backwards".
        /// </summary>
        UNKNOWN_4 = 4,

        /// <summary>
        /// Exact behavior still unknown.
        /// </summary>
        UNKNOWN_8 = 8,

        /// <summary>
        /// Exact behavior still unknown.
        /// Seen on some held / frozen-looking segments, but not enough to name with confidence yet.
        /// </summary>
        UNKNOWN_16 = 16,

        /// <summary>
        /// Exact behavior still unknown.
        /// </summary>
        UNKNOWN_32 = 32,

        /// <summary>
        /// Exact behavior still unknown.
        /// </summary>
        UNKNOWN_64 = 64,

        /// <summary>
        /// Exact behavior still unknown.
        /// </summary>
        UNKNOWN_128 = 128,

        /// <summary>
        /// Enables BlendInTimeMs.
        /// </summary>
        ENABLE_BLEND_IN = 256,

        /// <summary>
        /// Enables BlendOutTimeMs.
        /// </summary>
        ENABLE_BLEND_OUT = 512,

        /// <summary>
        /// Exact behavior still unknown.
        /// </summary>
        UNKNOWN_1024 = 1024,

        /// <summary>
        /// Exact behavior still unknown.
        /// </summary>
        UNKNOWN_2048 = 2048,

        /// <summary>
        /// Exact behavior still unknown.
        /// </summary>
        UNKNOWN_4096 = 4096,

        /// <summary>
        /// Exact behavior still unknown.
        /// </summary>
        UNKNOWN_8192 = 8192,

        /// <summary>
        /// Exact behavior still unknown.
        /// </summary>
        UNKNOWN_16384 = 16384,

        /// <summary>
        /// Exact behavior still unknown.
        /// </summary>
        UNKNOWN_32768 = 32768
    }
}
