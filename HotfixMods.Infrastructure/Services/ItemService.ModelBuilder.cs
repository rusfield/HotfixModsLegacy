using HotfixMods.Core.Enums;
using HotfixMods.Core.Models;
using HotfixMods.Core.Strings;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Services
{
    public partial class ItemService
    {
        List<ItemDisplayInfoMaterialRes> BuildItemDisplayInfoMaterialRes(ItemDto item, List<HotfixData> hotfixes, int initHotfixId)
        {
            var result = new List<ItemDisplayInfoMaterialRes>();

            int id = (int)item.Id;
            int itemDisplayInfoId = (int)item.ItemDisplayInfoId;
            if (!item.ComponentArmLower.IsNullOrZero())
            {
                result.Add(new ItemDisplayInfoMaterialRes()
                {
                    Id = id + (int)ComponentSections.ARM_LOWER,
                    ComponentSection = ComponentSections.ARM_LOWER,
                    MaterialResourcesId = (int)item.ComponentArmLower,
                    ItemDisplayInfoId = itemDisplayInfoId
                });

                hotfixes.Add(new HotfixData()
                {
                    Id = hotfixes.Count > 0 ? hotfixes.Max(h => h.Id) + 1 : initHotfixId,
                    RecordId = id + (int)ComponentSections.ARM_LOWER,
                    Status = HotfixStatuses.VALID,
                    TableHash = TableHashes.ItemDisplayInfoMaterialRes,
                    UniqueId = id,
                    VerifiedBuild = _verifiedBuild
                });
            }

            return result;
        }
    }
}
