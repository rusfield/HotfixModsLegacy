using HotfixMods.Core.Models.TrinityCore;

namespace HotfixMods.Infrastructure.DtoModels
{
    public interface IDto
    {
        public HotfixModsEntity HotfixModsEntity { get; set; }
        public bool IsUpdate { get; set; }
        public void AddToGroup(Type groupType, int index, Type? parentGroupType = null, int parentGroupIndex = 0);
        public void RemoveFromGroup(Type groupType, int index, Type? parentGroupType = null, int parentGroupIndex = 0);
        public void MoveInGroup(Type groupType, int oldIndex, int newIndex, Type? parentGroupType = null, int parentGroupIndex = 0);
        public void CloneInGroup(Type groupType, int index, Type? parentGroupType = null, int parentGroupIndex = 0);
        public int GetGroupCount(Type groupType, Type? parentGroupType = null, int parentGroupIndex = 0);
    }
}
