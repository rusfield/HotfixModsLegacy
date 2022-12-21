
namespace HotfixMods.Infrastructure.Config
{
    public class AppConfig
    {
        public string HotfixesSchema { get; } = "hotfixes";
        public string CharactersSchema { get; } = "characters";
        public string WorldSchema { get; } = "world";
        public string HotfixModsSchema { get; } = "hotfixmods";
        public string Location { get; } = @"C:\hotfixMods";

        public string HotfixDataTableName { get; } = "hotfix_data";
        public string HotfixDataRecordIdColumnName { get; } = "RecordId";
        public string HotfixDataTableHashColumnName { get; } = "TableHash";
        public string HotfixDataTableStatusColumnName { get; } = "Status";
        public int HotfixDataTableFromId { get; } = 15000000;
        public int HotfixDataTableToId { get; } = 16000000;

        public bool CacheResults { get; set; } = true;
    }
}
