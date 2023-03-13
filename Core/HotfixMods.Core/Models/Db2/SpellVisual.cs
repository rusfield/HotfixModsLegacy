using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellVisual
    {
        [IndexField]
        public int ID { get; set; } = 0;
        public decimal MissileCastOffset0 { get; set; } = 0;
        public decimal MissileCastOffset1 { get; set; } = 0;
        public decimal MissileCastOffset2 { get; set; } = 0;
        public decimal MissileImpactOffset0 { get; set; } = 0;
        public decimal MissileImpactOffset1 { get; set; } = 0;
        public decimal MissileImpactOffset2 { get; set; } = 0;
        public uint AnimEventSoundID { get; set; } = 0;
        public int Flags { get; set; } = 0;
        public sbyte MissileAttachment { get; set; } = -1;
        public sbyte MissileDestinationAttachment { get; set; } = 0;
        public uint MissileCastPositionerID { get; set; } = 0;
        public uint MissileImpactPositionerID { get; set; } = 0;
        public int MissileTargetingKit { get; set; } = 0;
        public uint HostileSpellVisualID { get; set; } = 0;
        public uint CasterSpellVisualID { get; set; } = 0;
        public ushort SpellVisualMissileSetID { get; set; } = 0;
        public ushort DamageNumberDelay { get; set; } = 0;
        public uint LowViolenceSpellVisualID { get; set; } = 0;
        public uint RaidSpellVisualMissileSetID { get; set; } = 0;
        public int ReducedUnexpectedCameraMovementSpellVisualID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
