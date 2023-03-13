using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class CreatureDisplayInfo
    {
        [IndexField]
        public int ID { get; set; } = 0;
        public ushort ModelID { get; set; } = 0;
        public ushort SoundID { get; set; } = 0;
        public sbyte SizeClass { get; set; } = 0;
        public decimal CreatureModelScale { get; set; } = 1;
        public byte CreatureModelAlpha { get; set; } = 255;
        public byte BloodID { get; set; } = 0;
        public int ExtendedDisplayInfoID { get; set; } = 0;
        public ushort NpcSoundID { get; set; } = 0;
        public ushort ParticleColorID { get; set; } = 0;
        public int PortraitCreatureDisplayInfoID { get; set; } = 0;
        public int PortraitTextureFileDataID { get; set; } = 0;
        public ushort ObjectEffectPackageID { get; set; } = 0;
        public ushort AnimReplacementSetID { get; set; } = 0;
        public byte Flags { get; set; } = 0;
        public int StateSpellVisualKitID { get; set; } = 0;
        public decimal PlayerOverrideScale { get; set; } = 0;
        public decimal PetInstanceScale { get; set; } = 1;
        public sbyte UnarmedWeaponType { get; set; } = -1;
        public int MountPoofSpellVisualKitID { get; set; } = 0;
        public int DissolveEffectID { get; set; } = 0;
        public sbyte Gender { get; set; } = 0;
        public int DissolveOutEffectID { get; set; } = 0;
        public sbyte CreatureModelMinLod { get; set; } = 0;
        public int TextureVariationFileDataID0 { get => 0; set { } } // Should always be 0
        public int TextureVariationFileDataID1 { get => 0; set { } } // Should always be 0
        public int TextureVariationFileDataID2 { get => 0; set { } } // Should always be 0
        public int TextureVariationFileDataID3 { get => 0; set { } } // Should always be 0
        public int VerifiedBuild { get; set; } = -1;
    }

}
