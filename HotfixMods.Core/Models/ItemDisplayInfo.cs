using HotfixMods.Core.Enums;
using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class ItemDisplayInfo : IHotfixesSchema, IDb2
    {
        [Key]
        public int Id { get; set; }
        public ItemDisplayInfoFlags Flags { get; set; }
        public int ModelResourcesId0 { get; set; }
        public int ModelResourcesId1 { get; set; }
        public int ModelMaterialResourcesId0 { get; set; }
        public int ModelMaterialResourcesId1 { get; set; }
        public int VerifiedBuild { get; set; }
        [Column("ItemDisplayModelType1")]
        public int ModelType0 { get; set; }
        [Column("ItemDisplayModelType2")]
        public int ModelType1 { get; set; }
        public int GeosetGroup0 { get; set; }
        public int GeosetGroup1 { get; set; }
        public int GeosetGroup2 { get; set; }
        public int GeosetGroup3 { get; set; }
        public int GeosetGroup4 { get; set; }
        public int GeosetGroup5 { get; set; }
        public int AttachmentGeosetGroup0 { get; set; }
        public int AttachmentGeosetGroup1{ get; set; }
        public int AttachmentGeosetGroup2 { get; set; }
        public int AttachmentGeosetGroup3 { get; set; }
        public int AttachmentGeosetGroup4 { get; set; }
        public int AttachmentGeosetGroup5 { get; set; }
        public int HelmetGeosetVis0 { get; set; }
        public int HelmetGeosetVis1 { get; set; }

        public int ParticleColorID { get; set; }
        public int ItemVisual { get; set; }
        public int ItemRangedDisplayInfoID { get; set; }
        public int OverrideSwooshSoundKitID { get; set; }
        public int SheatheTransformMatrixID { get; set; }
        public int StateSpellVisualKitID { get; set; }
        public int SheathedSpellVisualKitID { get; set; }
        public int UnsheathedSpellVisualKitID { get; set; }

    }
}
