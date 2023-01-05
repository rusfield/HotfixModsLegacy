namespace HotfixMods.Infrastructure.DtoModels
{
    public interface IDto
    {
        public void AddToGroup(Type groupType);
        public void RemoveFromGroup(Type groupType, int index);
        public void MoveInGroup(Type groupType, int oldIndex, int newIndex);
        public int GetGroupCount(Type groupType);
    }
}
