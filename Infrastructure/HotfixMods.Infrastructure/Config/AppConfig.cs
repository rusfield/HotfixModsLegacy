
namespace HotfixMods.Infrastructure.Config
{
    public class AppConfig
    {
        public string HotfixesSchema { get; set; } = "hotfixes";
        public string CharactersSchema { get; set; } = "characters";
        public string WorldSchema { get; set; } = "world";
        public string HotfixModsSchema { get; set; } = "hotfixmods";
        public string Location { get; set; } = @"C:\hotfixMods";
        public string HotfixDataTableName { get; set; } = "hotfix_data";
        public string HotfixDataRecordIdColumnName { get; set; } = "RecordId";
        public string HotfixDataTableHashColumnName { get; set; } = "TableHash";
        public string HotfixDataTableStatusColumnName { get; set; } = "Status";
        public int HotfixDataTableFromId { get; set; } = 15000000;
        public int HotfixDataTableToId { get; set; } = 16000000;
        public string GitHubAccessToken { get; set; } = "";
        public bool CacheResults { get; set; } = true;
    }
}
