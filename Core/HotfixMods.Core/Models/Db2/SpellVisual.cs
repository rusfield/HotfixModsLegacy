using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellVisual
    {
        public int Id { get; set; }
        public decimal MissileCastOffset1 { get; set; }
        public decimal MissileCastOffset2 { get; set; }
        public decimal MissileCastOffset3 { get; set; }
        public decimal MissileImpactOffset1 { get; set; }
        public decimal MissileImpactOffset2 { get; set; }
        public decimal MissileImpactOffset3 { get; set; }
        public uint AnimEventSoundId { get; set; }
        public int Flags { get; set; }
        public sbyte MissileAttachment { get; set; }
        public sbyte MissileDestinationAttachment { get; set; }
        public uint MissileCastPositionerId { get; set; }
        public uint MissileImpactPositionerId { get; set; }
        public int MissileTargetingKit { get; set; }
        public uint HostileSpellVisualId { get; set; }
        public uint CasterSpellVisualId { get; set; }
        public ushort SpellVisualMissileSetId { get; set; }
        public ushort DamageNumberDelay { get; set; }
        public uint LowViolenceSpellVisualId { get; set; }
        public uint RaidSpellVisualMissileSetId { get; set; }
        public int ReducedUnexpectedCameraMovementSpellVisualId { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
