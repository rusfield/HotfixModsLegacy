using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemSubClass
    {
        public string DisplayName { get; set; }
        public string VerboseName { get; set; }
        public uint Id { get; set; }
        public sbyte ClassId { get; set; }
        public sbyte SubClassId { get; set; }
        public byte AuctionHouseSortOrder { get; set; }
        public sbyte PrerequisiteProficiency { get; set; }
        public int Flags { get; set; }
        public int DisplayFlags { get; set; }
        public sbyte WeaponSwingSize { get; set; }
        public sbyte PostrequisiteProficiency { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
