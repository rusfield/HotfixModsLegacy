using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemDisplayInfo
    {
        public int Id { get; set; }
        public int ItemVisual { get; set; }
        public int ParticleColorId { get; set; }
        public uint ItemRangedDisplayInfoId { get; set; }
        public uint OverrideSwooshSoundKitId { get; set; }
        public int SheatheTransformMatrixId { get; set; }
        public int StateSpellVisualKitId { get; set; }
        public int SheathedSpellVisualKitId { get; set; }
        public uint UnsheathedSpellVisualKitId { get; set; }
        public int Flags { get; set; }
        public uint ModelResourcesID1 { get; set; }
        public uint ModelResourcesID2 { get; set; }
        public int ModelMaterialResourcesID1 { get; set; }
        public int ModelMaterialResourcesID2 { get; set; }
        public int ModelType1 { get; set; }
        public int ModelType2 { get; set; }
        public int GeosetGroup1 { get; set; }
        public int GeosetGroup2 { get; set; }
        public int GeosetGroup3 { get; set; }
        public int GeosetGroup4 { get; set; }
        public int GeosetGroup5 { get; set; }
        public int GeosetGroup6 { get; set; }
        public int AttachmentGeosetGroup1 { get; set; }
        public int AttachmentGeosetGroup2 { get; set; }
        public int AttachmentGeosetGroup3 { get; set; }
        public int AttachmentGeosetGroup4 { get; set; }
        public int AttachmentGeosetGroup5 { get; set; }
        public int AttachmentGeosetGroup6 { get; set; }
        public int HelmetGeosetVis1 { get; set; }
        public int HelmetGeosetVis2 { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
