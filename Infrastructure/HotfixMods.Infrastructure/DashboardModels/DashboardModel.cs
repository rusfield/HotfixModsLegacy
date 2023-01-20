namespace HotfixMods.Infrastructure.DashboardModels
{
    public class DashboardModel
    {
        public int Id { get; set; }
        public int? AdditionalId { get; set; } // For example ItemDisplayInfo or CreatureDisplayInfo
        public string? Name { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Comment { get; set; }
    }
}
