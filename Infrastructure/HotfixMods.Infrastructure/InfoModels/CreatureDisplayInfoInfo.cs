namespace HotfixMods.Infrastructure.InfoModels
{
    public class CreatureDisplayInfoInfo : IInfoModel
    {
        public string ID { get; set; } = "TODO";
        public string ModelID { get; set; } = "The ID found in CreatureModelData.db2 which holds information about the model this creature uses. If the creature is a character model or another type of model that uses customizations, this field should point towards a CreatureModelData ID that uses the same model as the selection in CreatureDisplayInfoExtra. Some customizations also affects the model, for example Orc Male and the Hunched/Upright back option.\r\nThe easiest way to populate this field correctly is to find the ID of a similar creature and load the values from there as a starting point.";
        public string SoundID { get; set; } = "TODO";
        public string SizeClass { get; set; } = "TODO";
        public string CreatureModelScale { get; set; } = "TODO";
        public string CreatureModelAlpha { get; set; } = "TODO";
        public string BloodID { get; set; } = "TODO";
        public string ExtendedDisplayInfoID { get; set; } = "TODO";
        public string NpcSoundID { get; set; } = "TODO";
        public string ParticleColorID { get; set; } = "TODO";
        public string PortraitCreatureDisplayInfoID { get; set; } = "TODO";
        public string PortraitTextureFileDataID { get; set; } = "TODO";
        public string ObjectEffectPackageID { get; set; } = "TODO";
        public string AnimReplacementSetID { get; set; } = "TODO";
        public string Flags { get; set; } = "TODO";
        public string StateSpellVisualKitID { get; set; } = "TODO";
        public string PlayerOverrideScale { get; set; } = "TODO";
        public string PetInstanceScale { get; set; } = "TODO";
        public string UnarmedWeaponType { get; set; } = "TODO";
        public string MountPoofSpellVisualKitID { get; set; } = "TODO";
        public string DissolveEffectID { get; set; } = "TODO";
        public string Gender { get; set; } = "TODO";
        public string DissolveOutEffectID { get; set; } = "TODO";
        public string CreatureModelMinLod { get; set; } = "TODO";
        public string TextureVariationFileDataID1 { get; set; } = "TODO";
        public string TextureVariationFileDataID2 { get; set; } = "TODO";
        public string TextureVariationFileDataID3 { get; set; } = "TODO";
        public string TextureVariationFileDataID4 { get; set; } = "TODO";
        public string VerifiedBuild { get; set; } = "TODO";
        public string ModelInfo { get; set; } = "TODO";
        public bool IsRequired { get; set; } = true;
    }
}
