using HotfixMods.Core.Models.TrinityCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.DtoModels
{
    public abstract class DtoBase
    {
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
