namespace HotfixMods.Apps.Console.Configuration;

public sealed class ConsoleAppSettings
{
    public string BuildInfo { get; set; } = "12.0.1.66838";
    public string DbdDefinitionsPath { get; set; } = string.Empty;
    public string ListfilePath { get; set; } = string.Empty;
    public string? StartupCommand { get; set; }
    public MySqlConnectionSettings MySql { get; set; } = new();
    public EyeColorExportSettings EyeColorExport { get; set; } = new();
}

public sealed class MySqlConnectionSettings
{
    public string Server { get; set; } = "localhost";
    public string Port { get; set; } = "3306";
    public string User { get; set; } = "root";
    public string Password { get; set; } = string.Empty;
}

public sealed class EyeColorExportSettings
{
    public string Db2Path { get; set; } = string.Empty;
    public string OutputPath { get; set; } = string.Empty;
    public int ChoiceStartId { get; set; } = 33000;
    public int ElementStartId { get; set; } = 120000;
    public int HotfixStartId { get; set; } = 901000000;
    public int VerifiedBuild { get; set; } = -1340;
}
