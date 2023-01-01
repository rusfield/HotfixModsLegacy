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


    }
}
