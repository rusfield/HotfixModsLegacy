namespace HotfixMods.Core.Enums.Db2
{
    public enum AnimKitSegmentStartCondition
    {
        /// <summary>
        /// Start the segment from the AnimKit root timeline.
        /// StartConditionDelay is treated as an additional delay in milliseconds.
        /// </summary>
        IMMEDIATE = 0,

        /// <summary>
        /// Start when the referenced segment starts, plus StartConditionDelay milliseconds.
        /// StartConditionParam points to another segment's OrderIndex.
        /// This is the overlapping / staggered chaining mode.
        /// </summary>
        AFTER_SEGMENT_START = 1,

        /// <summary>
        /// Start when the referenced segment ends, plus StartConditionDelay milliseconds.
        /// StartConditionParam points to another segment's OrderIndex.
        /// This is the sequential chaining mode.
        /// </summary>
        AFTER_SEGMENT_END = 2
    }
}
