﻿
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
        public string BuildInfo { get; set; } = "10.0.5.47871";
        public MySqlSettings MySql { get; set; } = new("127.0.0.1", "3306", "root", "root");
        public ServiceSettings GenericHotfixSettings { get; set; } = new(0, 0, -5501);
        public ServiceSettings AnimKitSettings { get; set; } = new(0, 0, -5502);
        public ServiceSettings GameobjectSettings { get; set; } = new(0, 0, -5503);
        public ServiceSettings SoundKitSettings { get; set; } = new(0, 0, -5504);
        public ServiceSettings ItemSettings { get; set; } = new(2000000, 2100000, -5505);
        public ServiceSettings CreatureSettings { get; set; } = new(3200000, 3300000, -5506);
        public ServiceSettings SpellSettings { get; set; } = new(5400000, 5500000, -5507);
        public ServiceSettings SpellVisualKitSettings { get; set; } = new(900000, 910000, -5508);

        public class MySqlSettings
        {
            public MySqlSettings(string server, string port,string username, string password)
            {
                Server = server;
                Port = port;
                Username = username;
                Password = password;
            }
            public string Server { get; set; }
            public string Port { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class ServiceSettings
        {
            public ServiceSettings(uint fromId, uint toId, int verifiedBuild)
            {
                FromId = fromId;
                ToId = toId;
                VerifiedBuild = verifiedBuild;
            }

            public uint FromId { get; set; }
            public uint ToId { get; set; }
            public int VerifiedBuild { get; set; }
        }
    }
}
