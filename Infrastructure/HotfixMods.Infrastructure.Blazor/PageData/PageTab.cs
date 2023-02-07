using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Blazor.PageData
{
    public class PageTab
    {
        public PageTab(string tabName, Type pageType)
        {
            TabName = tabName;
            PageType = pageType;
            TabID = Guid.NewGuid();
        }

        public string TabName { get; set; }
        public IDto Dto { get; set; }
        public IDto DtoCompare { get; set; }
        public Type PageType { get; set; }
        public Guid TabID { get; set; }
    }
}
