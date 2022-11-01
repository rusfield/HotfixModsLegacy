using HotfixMods.Infrastructure.Config;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace HotfixMods.Apps.MauiBlazor.Config
{
    public static class ConfigBuilder
    {
        public static IConfigurationRoot Build()
        {
            string mainDir = Path.Combine(FileSystem.Current.AppDataDirectory, "HotfixMods");
            Directory.CreateDirectory(mainDir);
            var configPath = $"{mainDir}/config.json";

            if (!File.Exists(configPath))
            {
                var appConfig = new AppConfig();
                var serializer = new JsonSerializerOptions();
                serializer.WriteIndented = true;
                File.WriteAllText(configPath, JsonSerializer.Serialize(appConfig, serializer));
            }

            return new ConfigurationBuilder()
            .AddJsonFile(configPath)
            .Build();
        }
    }
}
