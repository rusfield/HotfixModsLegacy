using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Razor.PageData
{
    public sealed class PageTab<T>
        where T : IDto
    {
        public PageTab(string tabName, Type pageType)
        {
            TabName = tabName;
            PageType = pageType;
            TabId = Guid.NewGuid();
        }

        public string TabName { get; set; }
        public T Dto { get; set; }
        public T DtoCompare { get; set; }
        public Type PageType { get; set; }
        public Guid TabId { get; set; }
    }
}
