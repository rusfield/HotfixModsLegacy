using HotfixMods.Core.Models.Interfaces;

namespace HotfixMods.Core.Models
{
    public class CreatureTemplateAddon : IWorldSchema
    {
        public int Entry { get; set; }
        public string Auras { get; set; }
    }
}
