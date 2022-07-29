using HotfixMods.Core.Enums;
using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class ItemDisplayInfo : IHotfixesSchema, IDb2
    {
        public int Id { get; set; }
        public ItemDisplayInfoFlags Flags { get; set; }
        public int ModelResourcesId0 { get; set; }
        public int ModelResourcesId1 { get; set; }
        public int ModelMaterialResourcesId0 { get; set; }
        public int ModelMaterialResourcesId1 { get; set; }
        public int VerifiedBuild { get; set; }


        // Temp

        [Column("ItemDisplayModelType1")]
        public int ModelType0 { get; set; } = 1;

        // TODO: Implement all these
        /*
         * ALTER TABLE hotfixes.item_display_info ALTER COLUMN ItemVisual SET DEFAULT 0;
ALTER TABLE hotfixes.item_display_info ALTER COLUMN ParticleColorID SET DEFAULT 0;
ALTER TABLE hotfixes.item_display_info ALTER COLUMN ItemRangedDisplayInfoID SET DEFAULT 0;
ALTER TABLE hotfixes.item_display_info ALTER COLUMN OverrideSwooshSoundKitID SET DEFAULT 0;
ALTER TABLE hotfixes.item_display_info ALTER COLUMN SheatheTransformMatrixID SET DEFAULT 0;
ALTER TABLE hotfixes.item_display_info ALTER COLUMN StateSpellVisualKitID SET DEFAULT 0;
ALTER TABLE hotfixes.item_display_info ALTER COLUMN SheathedSpellVisualKitID SET DEFAULT 0;
ALTER TABLE hotfixes.item_display_info ALTER COLUMN UnsheathedSpellVisualKitID SET DEFAULT 0;
ALTER TABLE hotfixes.item_display_info ALTER COLUMN ItemDisplayModelType1 SET DEFAULT 0;
ALTER TABLE hotfixes.item_display_info ALTER COLUMN ItemDisplayModelType2 SET DEFAULT 0;
ALTER TABLE hotfixes.item_display_info ALTER COLUMN GeosetGroup0 SET DEFAULT 0;
ALTER TABLE hotfixes.item_display_info ALTER COLUMN GeosetGroup1 SET DEFAULT 0;
ALTER TABLE hotfixes.item_display_info ALTER COLUMN GeosetGroup2 SET DEFAULT 0;
ALTER TABLE hotfixes.item_display_info ALTER COLUMN GeosetGroup3 SET DEFAULT 0;
ALTER TABLE hotfixes.item_display_info ALTER COLUMN GeosetGroup4 SET DEFAULT 0;
ALTER TABLE hotfixes.item_display_info ALTER COLUMN GeosetGroup5 SET DEFAULT 0;
ALTER TABLE hotfixes.item_display_info ALTER COLUMN AttachmentGeosetGroup0 SET DEFAULT 0;
ALTER TABLE hotfixes.item_display_info ALTER COLUMN AttachmentGeosetGroup1 SET DEFAULT 0;
ALTER TABLE hotfixes.item_display_info ALTER COLUMN AttachmentGeosetGroup2 SET DEFAULT 0;
ALTER TABLE hotfixes.item_display_info ALTER COLUMN AttachmentGeosetGroup3 SET DEFAULT 0;
ALTER TABLE hotfixes.item_display_info ALTER COLUMN AttachmentGeosetGroup4 SET DEFAULT 0;
ALTER TABLE hotfixes.item_display_info ALTER COLUMN AttachmentGeosetGroup5 SET DEFAULT 0;
ALTER TABLE hotfixes.item_display_info ALTER COLUMN HelmetGeosetVis0 SET DEFAULT 0;
ALTER TABLE hotfixes.item_display_info ALTER COLUMN HelmetGeosetVis1 SET DEFAULT 0;
        */
    }
}
