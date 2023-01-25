using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class CreatureDisplayInfo
    {
        [IndexField]
        public int Id { get; set; } = 0;
        public ushort ModelId { get; set; } = 0;
        public ushort SoundId { get; set; } = 0;
        public sbyte SizeClass { get; set; } = 0;
        public decimal CreatureModelScale { get; set; } = 1;
        public byte CreatureModelAlpha { get; set; } = 255;
        public byte BloodId { get; set; } = 0;
        public int ExtendedDisplayInfoId { get; set; } = 0;
        public ushort NPCSoundId { get; set; } = 0;
        public ushort ParticleColorId { get; set; } = 0;
        public int PortraitCreatureDisplayInfoId { get; set; } = 0;
        public int PortraitTextureFileDataId { get; set; } = 0;
        public ushort ObjectEffectPackageId { get; set; } = 0;
        public ushort AnimReplacementSetId { get; set; } = 0;
        public byte Flags { get; set; } = 0;
        public int StateSpellVisualKitId { get; set; } = 0;
        public decimal PlayerOverrideScale { get; set; } = 0;
        public decimal PetInstanceScale { get; set; } = 1;
        public sbyte UnarmedWeaponType { get; set; } = -1;
        public int MountPoofSpellVisualKitId { get; set; } = 0;
        public int DissolveEffectId { get; set; } = 0;
        public sbyte Gender { get; set; } = 0;
        public int DissolveOutEffectId { get; set; } = 0;
        public sbyte CreatureModelMinLod { get; set; } = 0;
        public int TextureVariationFileDataID1 { get; set; } = 0; 
        public int TextureVariationFileDataID2 { get; set; } = 0;
        public int TextureVariationFileDataID3 { get; set; } = 0;
        public int TextureVariationFileDataID4 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
