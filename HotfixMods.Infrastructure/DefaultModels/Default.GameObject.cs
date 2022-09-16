using HotfixMods.Core.Models;
using HotfixMods.Core.Enums;
using HotfixMods.Core.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.DefaultModels
{
    public static partial class Default
    {
        public static readonly GameObjectTemplate GameObjectTemplate = new()
        {
            IconName = "",
            Name = "New Object",
            ScriptName = "",
            Size = 1,
            Type = GameObjectTypes.CHEST,
            Unk1 = "",
            AiName = "TODO",
            CastBarCaption = "Using",

            VerifiedBuild = -1,
            Entry = -1,
            DisplayId = -1,
        };

        public static readonly GameObjectTemplateAddon GameObjectTemplateAddon = new()
        {
            Faction = 0,
            Flags = GameObjectAddonFlags.NONE,

            Entry = -1,
            //VerifiedBuild = -1
        };

        public static readonly GameObjectDisplayInfo GameObjectDisplayInfo = new()
        {
            FileDataId = 0, // TODO: Find ID of chest
            GeoBox0 = 0,
            GeoBox1 = 0,
            GeoBox2 = 0,
            GeoBox3 = 0,
            GeoBox4 = 0,
            GeoBox5 = 0,

            Id = -1
        };
    }
}
