namespace HotfixMods.Infrastructure.AggregateModels
{
    public class HealthModel
    {
        public Type? Type { get; set; }
        public HealthErrorStatus Status { get; set; }
        public string Description { get; set; }

        public enum HealthErrorStatus
        {
            ERROR,
            MISMATCH,
            MISSING
        }
    }
}
