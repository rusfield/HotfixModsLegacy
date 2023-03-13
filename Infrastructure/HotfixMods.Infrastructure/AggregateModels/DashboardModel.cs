namespace HotfixMods.Infrastructure.AggregateModels
{
    public class DashboardModel
    {
        public int ID { get; set; }
        public uint? AdditionalID { get; set; } // For example ItemDisplayInfo or CreatureDisplayInfo
        public string? Name { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Comment { get; set; }
    }
}
