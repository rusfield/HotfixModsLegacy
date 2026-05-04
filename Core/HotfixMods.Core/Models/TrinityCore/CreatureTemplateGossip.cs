using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [WorldSchema]
    public class CreatureTemplateGossip
    {
        [IndexField]
        public uint CreatureID { get; set; } = 0;
        public uint MenuID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
