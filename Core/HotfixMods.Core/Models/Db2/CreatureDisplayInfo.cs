using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class CreatureDisplayInfo
    {
        public int Id { get; set; }
        public ushort ModelId { get; set; }
        public ushort SoundId { get; set; }
        public sbyte SizeClass { get; set; }
        public decimal CreatureModelScale { get; set; }
        public byte CreatureModelAlpha { get; set; }
        public byte BloodId { get; set; }
        public int ExtendedDisplayInfoId { get; set; }
        public ushort NPCSoundId { get; set; }
        public ushort ParticleColorId { get; set; }
        public int PortraitCreatureDisplayInfoId { get; set; }
        public int PortraitTextureFileDataId { get; set; }
        public ushort ObjectEffectPackageId { get; set; }
        public ushort AnimReplacementSetId { get; set; }
        public byte Flags { get; set; }
        public int StateSpellVisualKitId { get; set; }
        public decimal PlayerOverrideScale { get; set; }
        public decimal PetInstanceScale { get; set; }
        public sbyte UnarmedWeaponType { get; set; }
        public int MountPoofSpellVisualKitId { get; set; }
        public int DissolveEffectId { get; set; }
        public sbyte Gender { get; set; }
        public int DissolveOutEffectId { get; set; }
        public sbyte CreatureModelMinLod { get; set; }
        public int TextureVariationFileDataID1 { get; set; }
        public int TextureVariationFileDataID2 { get; set; }
        public int TextureVariationFileDataID3 { get; set; }
        public int TextureVariationFileDataID4 { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
