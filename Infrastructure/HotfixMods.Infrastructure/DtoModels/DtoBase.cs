using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Extensions;
using System.Runtime.Serialization.Formatters.Binary;
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

        public virtual void AddToGroup(Type groupType, int index)
        {
            this.GetDtoGroup(groupType).Insert(index, Activator.CreateInstance(groupType));
        }

        public virtual void RemoveFromGroup(Type groupType, int index)
        {
            var group = this.GetDtoGroup(groupType);
            if (group.Count > index)
                group.RemoveAt(index);
        }

        public virtual void MoveInGroup(Type groupType, int oldIndex, int newIndex)
        {
            var group = this.GetDtoGroup(groupType);
            if (oldIndex >= 0 && oldIndex < group.Count && newIndex >= 0 && newIndex < group.Count)
            {
                var item = group[oldIndex];
                group.RemoveAt(oldIndex);
                group.Insert(newIndex, item);
            }
        }

        public virtual void CloneInGroup(Type groupType, int index)
        {
            var group = this.GetDtoGroup(groupType);
            if(index >= 0 && index < group.Count)
            {
                var item = group[index];
                string json = JsonSerializer.Serialize(item);
                var copy = JsonSerializer.Deserialize(json, item!.GetType());
                group.Insert(index , copy);
            }
        }

        public virtual int GetGroupCount(Type groupType)
        {
            return this.GetDtoGroup(groupType).Count;
        }
    }
}
