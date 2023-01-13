using HotfixMods.Core.Models.TrinityCore;

namespace HotfixMods.Infrastructure.DtoModels
{
    public interface IDto
    {
        public HotfixModsEntity HotfixModsEntity { get; set; }
        public bool IsUpdate { get; set; }
        public void AddToGroup(Type groupType, int index);
        public void RemoveFromGroup(Type groupType, int index);
        public void MoveInGroup(Type groupType, int oldIndex, int newIndex);
        public void CloneInGroup(Type groupType, int index);
        public int GetGroupCount(Type groupType);
    }
}
