
namespace HotfixMods.Infrastructure.Razor.PageData
{
    public sealed class PageTab
    {
        public PageTab(string tabName, Type pageType)
        {
            TabName = tabName;
            PageType=pageType;
            TabId = Guid.NewGuid();
        }

        public string TabName { get; set; }
        public Guid TabId { get; }
        public object? Dto { get; set; }
        public object? DtoLookup { get; set; }
        public Type PageType { get; set; }

    }
}
