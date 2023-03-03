using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class Faction
    {
        public uint ID { get; set; } = 1;
        public long ReputationRaceMask0 { get; set; }
        public long ReputationRaceMask1 { get; set; }
        public long ReputationRaceMask2 { get; set; }
        public long ReputationRaceMask3 { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public short ReputationIndex { get; set; }
        public ushort ParentFactionID { get; set; }
        public byte Expansion { get; set; }
        public uint FriendshipRepID { get; set; }
        public int Flags { get; set; }
        public ushort ParagonFactionID { get; set; }
        public int RenownFactionID { get; set; }
        public int RenownCurrencyID { get; set; }
        public short ReputationClassMask0 { get; set; }
        public short ReputationClassMask1 { get; set; }
        public short ReputationClassMask2 { get; set; }
        public short ReputationClassMask3 { get; set; }
        public ushort ReputationFlags0 { get; set; }
        public ushort ReputationFlags1 { get; set; }
        public ushort ReputationFlags2 { get; set; }
        public ushort ReputationFlags3 { get; set; }
        public int ReputationBase0 { get; set; }
        public int ReputationBase1 { get; set; }
        public int ReputationBase2 { get; set; }
        public int ReputationBase3 { get; set; }
        public int ReputationMax0 { get; set; }
        public int ReputationMax1 { get; set; }
        public int ReputationMax2 { get; set; }
        public int ReputationMax3 { get; set; }
        public decimal ParentFactionMod0 { get; set; }
        public decimal ParentFactionMod1 { get; set; }
        public byte ParentFactionCap0 { get; set; }
        public byte ParentFactionCap1 { get; set; }
        public int VerifiedBuild { get; set; } = -1;
    }
}
