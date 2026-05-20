using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class Faction
    {
        [IndexField]
        public int ID { get; set; } = 0;
        public long ReputationRaceMask1 { get; set; } = 0;
        public long ReputationRaceMask2 { get; set; } = 0;
        public long ReputationRaceMask3 { get; set; } = 0;
        public long ReputationRaceMask4 { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public short ReputationIndex { get; set; } = 0;
        public ushort ParentFactionID { get; set; } = 0;
        public byte Expansion { get; set; } = 0;
        public uint FriendshipRepID { get; set; } = 0;
        public int Flags { get; set; } = 0;
        public ushort ParagonFactionID { get; set; } = 0;
        public int RenownFactionID { get; set; } = 0;
        public int RenownCurrencyID { get; set; } = 0;
        public short ReputationClassMask1 { get; set; } = 0;
        public short ReputationClassMask2 { get; set; } = 0;
        public short ReputationClassMask3 { get; set; } = 0;
        public short ReputationClassMask4 { get; set; } = 0;
        public ushort ReputationFlags1 { get; set; } = 0;
        public ushort ReputationFlags2 { get; set; } = 0;
        public ushort ReputationFlags3 { get; set; } = 0;
        public ushort ReputationFlags4 { get; set; } = 0;
        public int ReputationBase1 { get; set; } = 0;
        public int ReputationBase2 { get; set; } = 0;
        public int ReputationBase3 { get; set; } = 0;
        public int ReputationBase4 { get; set; } = 0;
        public int ReputationMax1 { get; set; } = 0;
        public int ReputationMax2 { get; set; } = 0;
        public int ReputationMax3 { get; set; } = 0;
        public int ReputationMax4 { get; set; } = 0;
        public decimal ParentFactionMod1 { get; set; } = 0;
        public decimal ParentFactionMod2 { get; set; } = 0;
        public byte ParentFactionCap1 { get; set; } = 0;
        public byte ParentFactionCap2 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
