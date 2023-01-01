using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Blazor.PageData
{
    public abstract class PageTab
    {
        public PageTab(string tabName, Type pageType, Type dtoType)
        {
            TabName = tabName;
            PageType = pageType;
            DtoType = dtoType;
            TabId = Guid.NewGuid();
        }

        public string TabName { get; set; }
        public IDto Dto { get; set; }
        public IDto DtoCompare { get; set; }
        public Type PageType { get; set; }
        public Type DtoType { get; set; }
        public Guid TabId { get; set; }
    }
}
