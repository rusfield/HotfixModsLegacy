using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class Spell
    {
        public int Id { get; set; } = 0;
        public string NameSubtext { get; set; } = "";
        public string Description { get; set; } = "";
        public string AuraDescription { get; set; } = "";
        public int VerifiedBuild { get; set; } = -1;
    }

}
