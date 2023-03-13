using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemDisplayInfo
    {
        [IndexField]
        public int ID { get; set; } = 0;
        public int Field_3_4_1_46917_000 { get; set; } = 0;
        public int ItemVisual { get; set; } = 0;
        public int ParticleColorID { get; set; } = 0;
        public uint ItemRangedDisplayInfoID { get; set; } = 0;
        public uint OverrideSwooshSoundKitID { get; set; } = 0;
        public int SheatheTransformMatrixID { get; set; } = 0;
        public int StateSpellVisualKitID { get; set; } = 0;
        public int SheathedSpellVisualKitID { get; set; } = 0;
        public uint UnsheathedSpellVisualKitID { get; set; } = 0;
        public int Flags { get; set; } = 0;
        public uint ModelResourcesID0 { get; set; } = 0;
        public uint ModelResourcesID1 { get; set; } = 0;
        public int ModelMaterialResourcesID0 { get; set; } = 0;
        public int ModelMaterialResourcesID1 { get; set; } = 0;
        public int ModelType0 { get; set; } = 0;
        public int ModelType1 { get; set; } = 0;
        public int GeosetGroup0 { get; set; } = 0;
        public int GeosetGroup1 { get; set; } = 0;
        public int GeosetGroup2 { get; set; } = 0;
        public int GeosetGroup3 { get; set; } = 0;
        public int GeosetGroup4 { get; set; } = 0;
        public int GeosetGroup5 { get; set; } = 0;
        public int AttachmentGeosetGroup0 { get; set; } = 0;
        public int AttachmentGeosetGroup1 { get; set; } = 0;
        public int AttachmentGeosetGroup2 { get; set; } = 0;
        public int AttachmentGeosetGroup3 { get; set; } = 0;
        public int AttachmentGeosetGroup4 { get; set; } = 0;
        public int AttachmentGeosetGroup5 { get; set; } = 0;
        public int HelmetGeosetVis0 { get; set; } = 0;
        public int HelmetGeosetVis1 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
