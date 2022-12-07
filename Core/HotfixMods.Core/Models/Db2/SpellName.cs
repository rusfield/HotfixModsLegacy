using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellName
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = "";
        public int VerifiedBuild { get; set; } = -1;
    }

}
