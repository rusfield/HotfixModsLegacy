using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellName
    {
        [IndexField]
        public uint ID { get; set; } = 0;
        [LocalizedString]
        public string Name { get; set; } = "";
        public int VerifiedBuild { get; set; } = -1;
    }

}
