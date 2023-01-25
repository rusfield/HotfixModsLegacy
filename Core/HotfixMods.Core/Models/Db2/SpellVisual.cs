using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellVisual
    {
        [IndexField]
        public int Id { get; set; } = 0;
        public decimal MissileCastOffset1 { get; set; } = 0;
        public decimal MissileCastOffset2 { get; set; } = 0;
        public decimal MissileCastOffset3 { get; set; } = 0;
        public decimal MissileImpactOffset1 { get; set; } = 0;
        public decimal MissileImpactOffset2 { get; set; } = 0;
        public decimal MissileImpactOffset3 { get; set; } = 0;
        public uint AnimEventSoundId { get; set; } = 0;
        public int Flags { get; set; } = 0;
        public sbyte MissileAttachment { get; set; } = -1;
        public sbyte MissileDestinationAttachment { get; set; } = 0;
        public uint MissileCastPositionerId { get; set; } = 0;
        public uint MissileImpactPositionerId { get; set; } = 0;
        public int MissileTargetingKit { get; set; } = 0;
        public uint HostileSpellVisualId { get; set; } = 0;
        public uint CasterSpellVisualId { get; set; } = 0;
        public ushort SpellVisualMissileSetId { get; set; } = 0;
        public ushort DamageNumberDelay { get; set; } = 0;
        public uint LowViolenceSpellVisualId { get; set; } = 0;
        public uint RaidSpellVisualMissileSetId { get; set; } = 0;
        public int ReducedUnexpectedCameraMovementSpellVisualId { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
