using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class CreatureModelData
    {
        [IndexField]
        public int ID { get; set; } = 0;
        public decimal GeoBox1 { get; set; } = 0;
        public decimal GeoBox2 { get; set; } = 0;
        public decimal GeoBox3 { get; set; } = 0;
        public decimal GeoBox4 { get; set; } = 0;
        public decimal GeoBox5 { get; set; } = 0;
        public decimal GeoBox6 { get; set; } = 0;
        public int Flags { get; set; } = 0;
        public uint FileDataID { get; set; } = 0;
        public decimal WalkSpeed { get; set; } = 0;
        public decimal RunSpeed { get; set; } = 0;
        public uint BloodID { get; set; } = 0;
        public uint FootprintTextureID { get; set; } = 0;
        public decimal FootprintTextureLength { get; set; } = 0;
        public decimal FootprintTextureWidth { get; set; } = 0;
        public decimal FootprintParticleScale { get; set; } = 0;
        public uint FoleyMaterialID { get; set; } = 0;
        public uint FootstepCameraEffectID { get; set; } = 0;
        public uint DeathThudCameraEffectID { get; set; } = 0;
        public uint SoundID { get; set; } = 0;
        public sbyte SizeClass { get; set; } = 0;
        public decimal CollisionWidth { get; set; } = 0;
        public decimal CollisionHeight { get; set; } = 0;
        public decimal WorldEffectScale { get; set; } = 0;
        public uint CreatureGeosetDataID { get; set; } = 0;
        public decimal HoverHeight { get; set; } = 0;
        public decimal AttachedEffectScale { get; set; } = 0;
        public decimal ModelScale { get; set; } = 0;
        public decimal MissileCollisionRadius { get; set; } = 0;
        public decimal MissileCollisionPush { get; set; } = 0;
        public decimal MissileCollisionRaise { get; set; } = 0;
        public decimal MountHeight { get; set; } = 0;
        public decimal OverrideLootEffectScale { get; set; } = 0;
        public decimal OverrideNameScale { get; set; } = 0;
        public decimal OverrideSelectionRadius { get; set; } = 0;
        public decimal TamedPetBaseScale { get; set; } = 0;
        public sbyte MountScaleOtherIndex { get; set; } = 0;
        public decimal MountScaleSelf { get; set; } = 0;
        public ushort Unknown1100 { get; set; } = 0;
        public decimal MountScaleOther1 { get; set; } = 0;
        public decimal MountScaleOther2 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
