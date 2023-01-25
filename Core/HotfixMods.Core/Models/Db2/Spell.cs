using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class Spell
    {
        [IndexField]
        public int Id { get; set; } = 0;
        [LocalizedString]
        public string NameSubtext { get; set; } = "";
        [LocalizedString]
        public string Description { get; set; } = "";
        [LocalizedString]
        public string AuraDescription { get; set; } = "";
        public int VerifiedBuild { get; set; } = -1;
    }

}
