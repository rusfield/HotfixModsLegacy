namespace HotfixMods.Infrastructure.InfoModels
{
    public class CreatureModelInfoInfo : IInfoModel
    {
        public string BoundingRadius = "TODO";
        public string CombatReach = "TODO";
        public string DisplayId_Other_Gender = "The Display ID of the creature as the other gender. To utilize this for a custom creature in HotfixMods, create a new creature in the different gender and link them to each other here.\r\nFor more advanced configurations on multiple display models on the same creature, edit in TrinityCore directly.";
        public string ModelInfo { get; set; } = "TODO";
        public bool IsRequired { get; set; } = true;
    }
}
