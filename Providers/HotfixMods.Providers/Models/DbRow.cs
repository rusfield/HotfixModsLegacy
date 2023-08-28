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

        // Set column value on provided property name
        public void SetColumnValue(string columnName, object value)
        {
            var column = Columns.Where(c => c.Definition.Name.Equals(columnName, StringComparison.InvariantCultureIgnoreCase));
            if (null == column)
            {
                throw new Exception($"Column {columnName} not found.");
            }
            if (value.GetType() != column.First().Definition.Type)
            {
                throw new Exception($"Column {columnName} of type {column.First().Definition.Type} can not be set to type {value.GetType()}.");
            }
            column.First().Value = value;
        }
    }
}
