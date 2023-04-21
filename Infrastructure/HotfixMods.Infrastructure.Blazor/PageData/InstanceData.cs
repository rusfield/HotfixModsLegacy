namespace HotfixMods.Infrastructure.Blazor.PageData
{
    public class InstanceData
    {
        public InstanceData(Type groupType, int currentInstance, int instanceCount) 
        {
            GroupType = groupType;
            CurrentInstance = currentInstance;
            InstanceCount = instanceCount;
        }

        public Type GroupType { get; set; }
        public int CurrentInstance { get; set; }
        public int InstanceCount { get; set; }
        public List<Type> HiddenComponentTypes { get; set; }
    }
}
