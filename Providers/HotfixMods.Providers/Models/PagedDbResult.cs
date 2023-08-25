namespace HotfixMods.Providers.Models
{
    public class PagedDbResult
    {
        public PagedDbResult() 
        {
            Rows = new();
        }
        public PagedDbResult(int pageIndex, int pageSize, int totalRowCount) 
        {
            Rows = new();
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalRowCount = totalRowCount;
            TotalPageCount = (int)Math.Ceiling((double)totalRowCount / pageSize);
        }
        public List<DbRow> Rows { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPageCount { get; set; }
        public int TotalRowCount { get; set; }
    }
}
