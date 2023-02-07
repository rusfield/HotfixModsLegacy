using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [CharactersSchema]
    public class CharacterCustomizations
    {
        public ulong Guid { get; set; }
        public uint ChrCustomizationOptionID { get; set; }
        public uint ChrCustomizationChoiceID { get; set; }
    }

}
