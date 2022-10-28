namespace HotfixMods.Core.Models
{
    public class DbRow
    {
        public DbRow(string dbName)
        {
            DbName = dbName;
            Columns = new();
        }
        public string DbName { get; set; }
        public List<DbColumn> Columns { get; set; }

        public void SetColumnValue(string columnName, object value)
        {
            var column = Columns.Where(c => c.Name == columnName);
            if(null == column)
            {
                throw new Exception($"Column {columnName} not found.");
            }
            if(value.GetType() != column.First().Type)
            {
                throw new Exception($"Column {columnName} of type {column.First().Type} can not be set to type {value.GetType()}.");
            }
            column.First().Value = value;
        }
    }
}
