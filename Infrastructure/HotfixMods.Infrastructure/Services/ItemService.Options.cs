using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;

namespace HotfixMods.Infrastructure.Services
{
    public partial class ItemService
    {

        public async Task<List<ItemClass>> GetItemClassOptions()
        {
            return await GetFromClientOnlyAsync<ItemClass>();
        }

        public async Task<List<ItemSubClass>> GetItemSubClassOptions(sbyte classId)
        {
            return await GetFromClientOnlyAsync<ItemSubClass>(new DbParameter(nameof(ItemSubClass.ClassID), classId));
        }
    }
}
