using HotfixMods.Core.Models.Db2;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class CreatureModelDataDto : DtoBase
    {
        public CreatureModelDataDto() : base("Creature Model Data") { }

        public CreatureModelData CreatureModelData { get; set; } = new();
    }
}
