using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Extensions;
using System;
using System.Collections;
using System.Reflection;

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
            this.GetDtoGroup(groupType).Add(Activator.CreateInstance(groupType));
        }

        public virtual void RemoveFromGroup(Type groupType, int index)
        {
            this.GetDtoGroup(groupType).RemoveAt(index);
        }

        public virtual void MoveInGroup(Type groupType, int oldIndex, int newIndex)
        {
            // TODO
        }

        public virtual int GetGroupCount(Type groupType)
        {
            return this.GetDtoGroup(groupType).Count;
        }
    }
}
