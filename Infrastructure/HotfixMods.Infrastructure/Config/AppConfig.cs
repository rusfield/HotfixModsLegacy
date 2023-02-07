
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
        public string HotfixDataRecordIDColumnName { get; set; } = "RecordID";
        public string HotfixDataTableHashColumnName { get; set; } = "TableHash";
        public string HotfixDataTableStatusColumnName { get; set; } = "Status";
        public uint HotfixDataTableFromId { get; set; } = 15000000;
        public uint HotfixDataTableToId { get; set; } = 16000000;
        public string GitHubAccessToken { get; set; } = "";
        public string BuildInfo { get; set; } = "10.0.5.47871";
        public bool CacheResults { get; set; } = true;
    }
}
