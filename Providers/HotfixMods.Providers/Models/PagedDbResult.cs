namespace HotfixMods.Providers.Models
{
    public class PagedDbResult : PagedDbResult<DbRow>
    {
        public PagedDbResult() : base() { }
        public PagedDbResult(int pageIndex, int pageSize, ulong totalRowCount) : base(pageIndex, pageSize, totalRowCount) { }
    }


    public class PagedDbResult<T>
    {
        public PagedDbResult()
        {
        }

        public PagedDbResult(int pageIndex, int pageSize, ulong totalRowCount)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalRowCount = totalRowCount;
        }

        public List<T> Rows { get; set; } = new();
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public ulong TotalRowCount { get; set; }

        public ulong TotalPageCount => (ulong)Math.Ceiling((double)TotalRowCount / PageSize);
    }
}
