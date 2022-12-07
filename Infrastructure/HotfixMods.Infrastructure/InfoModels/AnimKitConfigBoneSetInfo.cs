namespace HotfixMods.Infrastructure.InfoModels
{
    public static class AnimKitConfigBoneSetInfo
    {
        public static string AnimKitBoneSetId = "The ID of the bone set that the current segment will be played on.\r\nAll bone sets except Full Body has a parent, so this property can be used to isolate an animation on a specific body part. Remember to set Priority accordingly.";
        public static string AnimKitPriorityId = "The ID of the priority that the current segment will be played with.\r\nWith multiple segments, the client needs to know in what priority the bone set in the current segment shall be played on.\r\n\r\nThe higher the number, the higher the priority.";
    }
}
