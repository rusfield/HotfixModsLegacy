﻿using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;

namespace HotfixMods.Infrastructure.Services
{
    public partial class ItemService
    {

        public async Task<List<ItemClass>> GetItemClassOptions()
        {
            return await GetAsync<ItemClass>(false, true);
        }

        public async Task<List<ItemSubClass>> GetItemSubClassOptions(sbyte classId)
        {
            return await GetAsync<ItemSubClass>(false, true, new DbParameter(nameof(ItemSubClass.ClassID), classId));
        }
    }
}
