using HotfixMods.Apps.Console.Configuration;
using HotfixMods.Providers.MySqlConnector.Client;
using HotfixMods.Providers.WowDev.Client;

namespace HotfixMods.Apps.Console.Commands;

internal interface IConsoleCommand
{
    string Name { get; }
    string Description { get; }
    string Usage { get; }
    Task ExecuteAsync(ConsoleCommandContext context);
}

internal sealed class ConsoleCommandDispatcher
{
    private readonly IReadOnlyDictionary<string, IConsoleCommand> _commands;
    private readonly ConsoleAppSettings _settings;

    public ConsoleCommandDispatcher(ConsoleAppSettings settings, IEnumerable<IConsoleCommand> commands)
    {
        _settings = settings;
        _commands = commands.ToDictionary(command => command.Name, StringComparer.OrdinalIgnoreCase);
    }

    public async Task<int> RunAsync(string[] args)
    {
        if (args.Length == 0)
        {
            if (!string.IsNullOrWhiteSpace(_settings.StartupCommand))
                args = _settings.StartupCommand.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            else
            {
                PrintOverview();
                return 0;
            }
        }

        if (IsHelpToken(args[0]))
        {
            PrintOverview();
            return 0;
        }

        if (!_commands.TryGetValue(args[0], out var command))
        {
            System.Console.Error.WriteLine($"Unknown command '{args[0]}'.");
            PrintOverview();
            return 1;
        }

        var parsedArguments = ConsoleArguments.Parse(args.Skip(1));
        if (parsedArguments.HasFlag("help"))
        {
            PrintCommandHelp(command);
            return 0;
        }

        try
        {
            await command.ExecuteAsync(new ConsoleCommandContext(_settings, parsedArguments));
            return 0;
        }
        catch (ConsoleCommandException ex)
        {
            System.Console.Error.WriteLine(ex.Message);
            System.Console.Error.WriteLine();
            PrintCommandHelp(command);
            return 1;
        }
    }

    private void PrintOverview()
    {
        System.Console.WriteLine("Usage:");
        System.Console.WriteLine("  HotfixMods.Apps.Console <command> [--option value]");
        System.Console.WriteLine();
        System.Console.WriteLine("Commands:");

        foreach (var command in _commands.Values.OrderBy(c => c.Name, StringComparer.OrdinalIgnoreCase))
        {
            System.Console.WriteLine($"  {command.Name,-32} {command.Description}");
        }

        System.Console.WriteLine();
        System.Console.WriteLine("Use '--help' after a command for details.");
    }

    private static void PrintCommandHelp(IConsoleCommand command)
    {
        System.Console.WriteLine($"{command.Name}");
        System.Console.WriteLine($"  {command.Description}");
        System.Console.WriteLine();
        System.Console.WriteLine("Usage:");
        System.Console.WriteLine($"  HotfixMods.Apps.Console {command.Usage}");
    }

    private static bool IsHelpToken(string value)
    {
        return value.Equals("--help", StringComparison.OrdinalIgnoreCase)
            || value.Equals("-h", StringComparison.OrdinalIgnoreCase)
            || value.Equals("help", StringComparison.OrdinalIgnoreCase);
    }
}

internal sealed class ConsoleCommandContext
{
    public ConsoleAppSettings Settings { get; }
    public ConsoleArguments Arguments { get; }

    public ConsoleCommandContext(ConsoleAppSettings settings, ConsoleArguments arguments)
    {
        Settings = settings;
        Arguments = arguments;
    }

    public string RequireBuild()
    {
        var build = Arguments.GetOrDefault("build", Settings.BuildInfo);
        if (string.IsNullOrWhiteSpace(build))
            throw new ConsoleCommandException("A build is required. Pass '--build <value>' or set 'BuildInfo' in appsettings.json.");

        return build;
    }

    public string RequireDefinitionsPath()
    {
        if (string.IsNullOrWhiteSpace(Settings.DbdDefinitionsPath))
        {
            throw new ConsoleCommandException(
                "The console app setting 'DbdDefinitionsPath' must point at a WoWDBDefs definitions directory.");
        }

        return Settings.DbdDefinitionsPath;
    }

    public string RequireDb2Path()
    {
        var db2Path = Arguments.GetOrDefault("db2-path", GetConfiguredDb2Path());
        if (string.IsNullOrWhiteSpace(db2Path))
        {
            throw new ConsoleCommandException(
                "A DB2 path is required. Pass '--db2-path <path>' or set 'CustomizationRequirementOverride:Db2Path' or 'EyeColorExport:Db2Path' in appsettings.json.");
        }

        return db2Path;
    }

    public string RequireListfilePath()
    {
        var listfilePath = Arguments.GetOrDefault("listfile-path", Settings.ListfilePath);
        if (string.IsNullOrWhiteSpace(listfilePath))
        {
            throw new ConsoleCommandException(
                "A listfile path is required. Pass '--listfile-path <path>' or set 'ListfilePath' in appsettings.json.");
        }

        return listfilePath;
    }

    private string GetConfiguredDb2Path()
    {
        if (!string.IsNullOrWhiteSpace(Settings.CustomizationRequirementOverride.Db2Path))
            return Settings.CustomizationRequirementOverride.Db2Path;

        if (!string.IsNullOrWhiteSpace(Settings.EyeColorExport.Db2Path))
            return Settings.EyeColorExport.Db2Path;

        return string.Empty;
    }

    public CustomizationRequirementOverrideSettings CustomizationRequirementOverride => Settings.CustomizationRequirementOverride;
    public EyeColorExportSettings EyeColorExport => Settings.EyeColorExport;

    public Db2Client CreateDb2Client()
    {
        return new Db2Client(RequireBuild(), RequireDefinitionsPath());
    }

    public MySqlClient CreateMySqlClient()
    {
        var server = Arguments.GetOrDefault("server", Settings.MySql.Server);
        var port = Arguments.GetOrDefault("port", Settings.MySql.Port);
        var user = Arguments.GetOrDefault("user", Settings.MySql.User);
        var password = Arguments.GetOrDefault("password", Settings.MySql.Password);

        if (string.IsNullOrWhiteSpace(server) || string.IsNullOrWhiteSpace(port) || string.IsNullOrWhiteSpace(user))
        {
            throw new ConsoleCommandException(
                "MySQL connection settings are incomplete. Pass '--server', '--port', and '--user' or set them in appsettings.json.");
        }

        return new MySqlClient(server, port, user, password);
    }
}

internal sealed class ConsoleArguments
{
    private readonly Dictionary<string, string> _namedValues;
    private readonly HashSet<string> _flags;

    public IReadOnlyList<string> Positionals { get; }

    private ConsoleArguments(Dictionary<string, string> namedValues, HashSet<string> flags, List<string> positionals)
    {
        _namedValues = namedValues;
        _flags = flags;
        Positionals = positionals;
    }

    public static ConsoleArguments Parse(IEnumerable<string> args)
    {
        var namedValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var flags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var positionals = new List<string>();

        var tokens = args.ToArray();
        for (var i = 0; i < tokens.Length; i++)
        {
            var token = tokens[i];
            if (!token.StartsWith("--", StringComparison.Ordinal))
            {
                positionals.Add(token);
                continue;
            }

            var key = token[2..];
            if (i + 1 < tokens.Length && !tokens[i + 1].StartsWith("--", StringComparison.Ordinal))
            {
                namedValues[key] = tokens[++i];
            }
            else
            {
                flags.Add(key);
            }
        }

        return new ConsoleArguments(namedValues, flags, positionals);
    }

    public string? Get(string name)
    {
        return _namedValues.TryGetValue(name, out var value) ? value : null;
    }

    public string GetRequired(string name)
    {
        var value = Get(name);
        if (string.IsNullOrWhiteSpace(value))
            throw new ConsoleCommandException($"Missing required option '--{name}'.");

        return value;
    }

    public string GetOrDefault(string name, string fallback)
    {
        return string.IsNullOrWhiteSpace(Get(name)) ? fallback : Get(name)!;
    }

    public int GetInt(string name, int fallback)
    {
        var value = Get(name);
        return int.TryParse(value, out var result) ? result : fallback;
    }

    public long GetLong(string name, long fallback)
    {
        var value = Get(name);
        return long.TryParse(value, out var result) ? result : fallback;
    }

    public bool HasFlag(string name)
    {
        return _flags.Contains(name);
    }

    public List<string> GetCsv(string name)
    {
        var value = Get(name);
        return string.IsNullOrWhiteSpace(value)
            ? []
            : value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
    }
}

internal sealed class ConsoleCommandException : Exception
{
    public ConsoleCommandException(string message) : base(message)
    {
    }
}
