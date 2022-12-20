using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [CharactersSchema]
    public class CharacterCustomizations 
    {
        public int Guid { get; set; }
        public int ChrCustomizationOptionId { get; set; }
        public int ChrCustomizationChoiceId { get; set; }
    }
}
