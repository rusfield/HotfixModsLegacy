namespace DBCD.Providers
{
    public class GithubDBDProvider : IDBDProvider
    {
        private static Uri BaseURI = new Uri("https://raw.githubusercontent.com/wowdev/WoWDBDefs/master/definitions/");
        private HttpClient client = new HttpClient();

        public GithubDBDProvider()
        {
            client.BaseAddress = BaseURI;
            client.DefaultRequestHeaders.Add("Authorization", "Bearer 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"); // scope to public repo only, but try not to deploy this...
        }

        public Stream StreamForTableName(string tableName, string? build = null)
        {
            var query = $"{tableName}.dbd";
            var bytes = client.GetByteArrayAsync(query).Result;

            return new MemoryStream(bytes);
        }
    }
}