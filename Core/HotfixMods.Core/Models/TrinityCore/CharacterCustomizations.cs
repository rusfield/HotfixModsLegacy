using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [CharactersSchema]
    public class CharacterCustomizations
    {
        public ulong Guid { get; set; }
        public uint ChrCustomizationOptionId { get; set; }
        public uint ChrCustomizationChoiceId { get; set; }
    }

}
