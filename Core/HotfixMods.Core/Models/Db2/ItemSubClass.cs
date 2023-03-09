using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema] // Client 
    public class ItemSubClass
    {
        public string DisplayName { get; set; } = "";
        public string VerboseName { get; set; } = "";
        public uint ID { get; set; } = 1;
        public sbyte ClassID { get; set; } = 0; 
        public sbyte SubClassID { get; set; } = 0;
        public byte AuctionHouseSortOrder { get; set; } = 0;
        public sbyte PrerequisiteProficiency { get; set; } = -1;
        public int Flags { get; set; } = 0; 
        public int DisplayFlags { get; set; } = 0; 
        public sbyte WeaponSwingSize { get; set; } = 0; 
        public sbyte PostrequisiteProficiency { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
