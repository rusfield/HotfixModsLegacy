using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [WorldSchema]
    public class GossipMenuOption
    {
        public uint MenuID { get; set; } = 0;
        public int GossipOptionID { get; set; } = 0;
        [IndexField]
        public uint OptionID { get; set; } = 0;
        public byte OptionNpc { get; set; } = 0;
        public string OptionText { get; set; } = "";
        public uint OptionBroadcastTextID { get; set; } = 0;
        public uint Language { get; set; } = 0;
        public int Flags { get; set; } = 0;
        public uint ActionMenuID { get; set; } = 0;
        public uint ActionPoiID { get; set; } = 0;
        public int GossipNpcOptionID { get; set; } = 0;
        public byte BoxCoded { get; set; } = 0;
        public ulong BoxMoney { get; set; } = 0;
        public string BoxText { get; set; } = "";
        public uint BoxBroadcastTextID { get; set; } = 0;
        public int SpellID { get; set; } = 0;
        public int OverrideIconID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
