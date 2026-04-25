using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [WorldSchema]
    public class ConversationTemplate
    {
        [IndexField]
        public int Id { get; set; } = 0;
        public int FirstLineId { get; set; } = 0;
        public int TextureKitId { get; set; } = 0;
        public byte Flags { get; set; } = 0;
        public string ScriptName { get; set; } = "";
        public int VerifiedBuild { get; set; } = -1;
    }
}
