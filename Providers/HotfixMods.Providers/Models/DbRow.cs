namespace HotfixMods.Providers.Models
{
    public class DbRow
    {
        public DbRow() : this("New") { }
        public DbRow(string dbName)
        {
            Db2Name = dbName;
            Columns = new();
        }
        public string Db2Name { get; set; }
        public List<DbColumn> Columns { get; set; }
    }
}
