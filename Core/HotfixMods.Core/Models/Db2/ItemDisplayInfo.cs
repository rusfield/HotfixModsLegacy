using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemDisplayInfo
    {
        [IndexField]
        public uint Id { get; set; } = 0;
        public int Field_3_4_1_46917_000 { get; set; } = 0;
        public int ItemVisual { get; set; } = 0;
        public int ParticleColorId { get; set; } = 0;
        public uint ItemRangedDisplayInfoId { get; set; } = 0;
        public uint OverrideSwooshSoundKitId { get; set; } = 0;
        public int SheatheTransformMatrixId { get; set; } = 0;
        public int StateSpellVisualKitId { get; set; } = 0;
        public int SheathedSpellVisualKitId { get; set; } = 0;
        public uint UnsheathedSpellVisualKitId { get; set; } = 0;
        public int Flags { get; set; } = 0;
        public uint ModelResourcesID1 { get; set; } = 0;
        public uint ModelResourcesID2 { get; set; } = 0;
        public int ModelMaterialResourcesID1 { get; set; } = 0;
        public int ModelMaterialResourcesID2 { get; set; } = 0;
        public int ModelType1 { get; set; } = 0;
        public int ModelType2 { get; set; } = 0;
        public int GeosetGroup1 { get; set; } = 0;
        public int GeosetGroup2 { get; set; } = 0;
        public int GeosetGroup3 { get; set; } = 0;
        public int GeosetGroup4 { get; set; } = 0;
        public int GeosetGroup5 { get; set; } = 0;
        public int GeosetGroup6 { get; set; } = 0;
        public int AttachmentGeosetGroup1 { get; set; } = 0;
        public int AttachmentGeosetGroup2 { get; set; } = 0;
        public int AttachmentGeosetGroup3 { get; set; } = 0;
        public int AttachmentGeosetGroup4 { get; set; } = 0;
        public int AttachmentGeosetGroup5 { get; set; } = 0;
        public int AttachmentGeosetGroup6 { get; set; } = 0;
        public int HelmetGeosetVis1 { get; set; } = 0;
        public int HelmetGeosetVis2 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
