namespace HotfixMods.Infrastructure.AggregateModels
{
    public class HealthModel
    {
        public Type DtoType { get; set; }
        public Type DtoPropertyType { get; set; }
        public HealthErrorStatus Status { get; set; }
        public string Description { get; set; }

        public enum HealthErrorStatus
        {
            MISMATCH,
            MISSING
        }
    }
}
