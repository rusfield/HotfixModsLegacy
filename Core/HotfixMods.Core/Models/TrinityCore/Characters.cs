using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    // Helper model to load characters by name
    [CharactersSchema]
    public class Characters 
    {
        public string Name { get; set; }
        public byte Level { get; set; }
    }
}
