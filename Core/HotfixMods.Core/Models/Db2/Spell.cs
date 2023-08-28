using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class Spell
    {
        
        public int ID { get; set; } = 0;
        
        public string NameSubtext { get; set; } = "";
        
        [Db2Description("The text in the tooltip that appears when you hover over the spell.")]
        public string Description { get; set; } = "";
        
        [Db2Description("The text in the tooltip that appears when you hover over the icon buff/debuff.")]
        public string AuraDescription { get; set; } = "";
        public int VerifiedBuild { get; set; } = -1;
    }

}
