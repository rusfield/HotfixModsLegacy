namespace HotfixMods.Providers.Models
{
    public class DbRowDefinition
    {
        public DbRowDefinition(string dbName)
        {
            DbName = dbName;
            ColumnDefinitions = new();
        }
        public string DbName { get; set; }
        public List<DbColumnDefinition> ColumnDefinitions { get; set; }
    }
}
