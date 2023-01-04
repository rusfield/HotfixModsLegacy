using HotfixMods.Core.Models.TrinityCore;

namespace HotfixMods.Infrastructure.DtoModels
{
    public abstract class DtoBase : IDto
    {
        public DtoBase() { }
        public DtoBase(string displayName)
        {
            _displayName = displayName;
        }
        public HotfixModsEntity HotfixModsEntity { get; set; } = new();
        public bool IsUpdate { get; set; } = false;

        string _displayName;

        public string GetDisplayName()
        {
            return _displayName;
        }

        public virtual void AddToGroup(Type groupType)
        {
            throw new NotImplementedException();
        }

        public virtual void RemoveFromGroup(Type groupType, int index)
        {
            throw new NotImplementedException();
        }

        public virtual void MoveInGroup(Type groupType, int oldIndex, int newIndex)
        {
            throw new NotImplementedException();
        }
    }
}
