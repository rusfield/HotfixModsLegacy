namespace DBCD.Providers
{
    public interface IDBCProvider
    {
        Stream StreamForTableName(string tableName, string build);
    }
}