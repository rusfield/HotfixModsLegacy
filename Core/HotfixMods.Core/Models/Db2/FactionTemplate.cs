using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class FactionTemplate
    {
        [IndexField]
        public int ID { get; set; } = 0;
        public ushort Faction { get; set; } = 0;
        public int Flags { get; set; } = 0;
        public byte FactionGroup { get; set; } = 0;
        public byte FriendGroup { get; set; } = 0;
        public byte EnemyGroup { get; set; } = 0;
        public ushort Enemies1 { get; set; } = 0;
        public ushort Enemies2 { get; set; } = 0;
        public ushort Enemies3 { get; set; } = 0;
        public ushort Enemies4 { get; set; } = 0;
        public ushort Enemies5 { get; set; } = 0;
        public ushort Enemies6 { get; set; } = 0;
        public ushort Enemies7 { get; set; } = 0;
        public ushort Enemies8 { get; set; } = 0;
        public ushort Friend1 { get; set; } = 0;
        public ushort Friend2 { get; set; } = 0;
        public ushort Friend3 { get; set; } = 0;
        public ushort Friend4 { get; set; } = 0;
        public ushort Friend5 { get; set; } = 0;
        public ushort Friend6 { get; set; } = 0;
        public ushort Friend7 { get; set; } = 0;
        public ushort Friend8 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
