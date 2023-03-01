namespace HotfixMods.Infrastructure.AggregateModels
{
    public class HealthModel
    {
        public Type Type { get; set; }
        public HealthStatus Status { get; set; }
        public string? Description { get; set; }

        public enum HealthStatus
        {
            OK,
            MISMATCH,
            MISSING
        }
    }
}
