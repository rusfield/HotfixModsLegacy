namespace HotfixMods.Infrastructure.DashboardModels
{
    public class DashboardModel
    {
        public uint ID { get; set; }
        public uint? AdditionalID { get; set; } // For example ItemDisplayInfo or CreatureDisplayInfo
        public string? Name { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Comment { get; set; }
    }
}
