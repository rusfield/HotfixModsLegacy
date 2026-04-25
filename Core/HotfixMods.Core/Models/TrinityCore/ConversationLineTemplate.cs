using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [WorldSchema]
    public class ConversationLineTemplate
    {
        [IndexField]
        public int Id { get; set; } = 0;
        public int UiCameraID { get; set; } = 0;
        public byte ActorIdx { get; set; } = 0;
        public byte Flags { get; set; } = 0;
        public byte ChatType { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
