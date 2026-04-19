namespace HotfixMods.Infrastructure.Blazor.PageData
{
    public class InstanceData
    {
        public InstanceData(Type groupType, int currentInstance, int instanceCount, Type? parentGroupType = null, int parentGroupIndex = 0) 
        {
            GroupType = groupType;
            CurrentInstance = currentInstance;
            InstanceCount = instanceCount;
            ParentGroupType = parentGroupType;
            ParentGroupIndex = parentGroupIndex;
        }

        public Type GroupType { get; set; }
        public Type? ParentGroupType { get; set; }
        public int ParentGroupIndex { get; set; }
        public int CurrentInstance { get; set; }
        public int InstanceCount { get; set; }
    }
}
