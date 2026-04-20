namespace HotfixMods.Core.Enums.Db2
{
    public enum AnimKitSegmentEndCondition
    {
        /// <summary>
        /// No clearly identified automatic stop behavior.
        /// Often used for persistent / held segments, and sometimes as glue in multi-segment chains.
        /// EndConditionParam is occasionally used as chain metadata here, but the exact meaning is still not fully decoded.
        /// </summary>
        NONE_OR_PERSIST = 0,

        /// <summary>
        /// Play until the animation naturally reaches its end.
        /// Common on segments that simply play once and stop without an explicit timer.
        /// </summary>
        PLAY_UNTIL_ANIMATION_END = 1,

        /// <summary>
        /// Timed stop variant.
        /// EndConditionDelay behaves like milliseconds, but this mode still differs from STOP_AFTER_MS in subtle ways.
        /// Often used together with follow-up segments that continue from a later AnimStartTime.
        /// </summary>
        TIMED_STOP_VARIANT = 2,

        /// <summary>
        /// Stop the segment after EndConditionDelay milliseconds.
        /// This is the clearest general-purpose timed cutoff mode.
        /// </summary>
        STOP_AFTER_MS = 3,

        /// <summary>
        /// Timed slice / tail-window variant.
        /// In practice this often behaves like a bounded end-slice of the animation, especially when combined with AnimStartTime or looping.
        /// EndConditionParam is usually 0 or the segment's own OrderIndex.
        /// </summary>
        TIMED_SLICE_VARIANT = 4,

        /// <summary>
        /// Rare variant related to TIMED_SLICE_VARIANT.
        /// Observed much less frequently and still not fully decoded.
        /// </summary>
        TIMED_SLICE_VARIANT_RARE = 5
    }
}
