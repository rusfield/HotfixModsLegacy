using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Extensions;
using System.Text.Json;

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

        public virtual void AddToGroup(Type groupType, int index, Type? parentGroupType = null, int parentGroupIndex = 0)
        {
            this.GetDtoGroup(groupType, parentGroupType, parentGroupIndex).Insert(index, Activator.CreateInstance(groupType));
        }

        public virtual void RemoveFromGroup(Type groupType, int index, Type? parentGroupType = null, int parentGroupIndex = 0)
        {
            var group = this.GetDtoGroup(groupType, parentGroupType, parentGroupIndex);
            if (group.Count > index)
                group.RemoveAt(index);
        }

        public virtual void MoveInGroup(Type groupType, int oldIndex, int newIndex, Type? parentGroupType = null, int parentGroupIndex = 0)
        {
            var group = this.GetDtoGroup(groupType, parentGroupType, parentGroupIndex);
            group.MoveElement(oldIndex, newIndex);
        }

        public virtual void CloneInGroup(Type groupType, int index, Type? parentGroupType = null, int parentGroupIndex = 0)
        {
            var group = this.GetDtoGroup(groupType, parentGroupType, parentGroupIndex);
            if(index >= 0 && index < group.Count)
            {
                var item = group[index];
                string json = JsonSerializer.Serialize(item);
                var copy = JsonSerializer.Deserialize(json, item!.GetType());
                group.Insert(index , copy);
            }
        }

        public virtual int GetGroupCount(Type groupType, Type? parentGroupType = null, int parentGroupIndex = 0)
        {
            return this.GetDtoGroup(groupType, parentGroupType, parentGroupIndex).Count;
        }
    }
}
