using HotfixMods.Infrastructure.Config;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace HotfixMods.Apps.MauiBlazor.Config
{
    public static class ConfigHandler
    {
        public static AppConfig GetAppConfig()
        {


            var appConfig = new AppConfig();

            try
            {
                if (!File.Exists(ConfigPath))
                {
                    var serializer = new JsonSerializerOptions();
                    serializer.WriteIndented = true;
                    File.WriteAllText(ConfigPath, JsonSerializer.Serialize(appConfig, serializer));
                    appConfig.FirstLoad = true;
                }

                appConfig = JsonSerializer.Deserialize<AppConfig>(File.ReadAllText(ConfigPath)) ?? new AppConfig();
                if (appConfig.GossipSettings.FromId == 450000
                    && appConfig.GossipSettings.ToId == 500000)
                {
                    appConfig.GossipSettings = new(5600000, 5700000, -55510);
                    Save(appConfig);
                }
            }
            catch
            {
                appConfig.LoadedCorrectly = false;
            }

            appConfig.Save = () => Save(appConfig);
            appConfig.ConfigFilePath = ConfigPath.Replace("/config.json", "");
            return appConfig;
        }

        public static void Save(AppConfig appConfig)
        {
            var serializer = new JsonSerializerOptions();
            serializer.WriteIndented = true;
            File.WriteAllText(ConfigPath, JsonSerializer.Serialize(appConfig, serializer));
        }

        public static string ConfigPath
        {
            get
            {
                string mainDir = Path.Combine(FileSystem.Current.AppDataDirectory, "HotfixMods");
                Directory.CreateDirectory(mainDir);
                var configPath = $"{mainDir}/config.json";
                return configPath;
            }
        }
    }
}
