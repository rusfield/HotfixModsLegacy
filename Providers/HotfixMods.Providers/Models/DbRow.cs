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

        /// <summary>
        /// Sets the value of a column. Column name is case insensitive. Type of value must be the same.
        /// </summary>
        /// <param name="columnName">Name of column/property. Case insensitive.</param>
        /// <param name="value">New value. Type must be correct.</param>
        /// <exception cref="Exception"></exception>
        public void SetColumnValue(string columnName, object value)
        {
            var column = Columns.Where(c => c.Name.Equals(columnName, StringComparison.InvariantCultureIgnoreCase));
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
