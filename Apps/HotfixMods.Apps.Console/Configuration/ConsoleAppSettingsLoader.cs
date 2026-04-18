using System.Text.Json;

namespace HotfixMods.Apps.Console.Configuration;

public static class ConsoleAppSettingsLoader
{
    public static ConsoleAppSettings Load(string baseDirectory)
    {
        var appSettingsPath = Path.Combine(baseDirectory, "appsettings.json");
        if (!File.Exists(appSettingsPath))
            throw new FileNotFoundException($"Unable to find console app settings file: {appSettingsPath}");

        var appSettings = JsonSerializer.Deserialize<ConsoleAppSettings>(File.ReadAllText(appSettingsPath));
        if (appSettings is null)
            throw new InvalidOperationException($"Unable to load console app settings from {appSettingsPath}");

        return appSettings;
    }
}
