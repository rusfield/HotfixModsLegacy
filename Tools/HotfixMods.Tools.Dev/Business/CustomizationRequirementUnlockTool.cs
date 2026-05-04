using HotfixMods.Core.Enums;
using HotfixMods.Core.Models;
using HotfixMods.Providers.WowDev.Client;
using System.Text;

namespace HotfixMods.Tools.Dev.Business
{
    public sealed class CustomizationRequirementUnlockTool
    {
        private const int NpcReqType = 2;
        private const int ChoiceReqType = 3;
        private readonly Db2Client _db2Client;
        private readonly CustomizationRequirementUnlockOptions _options;

        public CustomizationRequirementUnlockTool(
            Db2Client db2Client,
            CustomizationRequirementUnlockOptions options)
        {
            _db2Client = db2Client;
            _options = options;
        }

        public async Task<string> GenerateAsync()
        {
            var outputDirectory = GetOutputDirectory();
            Directory.CreateDirectory(outputDirectory);

            var reqDefinition = await _db2Client.GetDefinitionAsync(_options.Db2DataPath, "ChrCustomizationReq")
                ?? throw new InvalidOperationException("Could not load ChrCustomizationReq definition.");
            var choiceDefinition = await _db2Client.GetDefinitionAsync(_options.Db2DataPath, "ChrCustomizationChoice")
                ?? throw new InvalidOperationException("Could not load ChrCustomizationChoice definition.");

            var requirements = await _db2Client.GetAsync(_options.Db2DataPath, "ChrCustomizationReq", reqDefinition);
            var choices = await _db2Client.GetAsync(_options.Db2DataPath, "ChrCustomizationChoice", choiceDefinition);
            var usedRequirementIds = choices
                .Select(choice => GetInt(choice, "ChrCustomizationReqID"))
                .Where(id => id != 0)
                .ToHashSet();

            var rows = requirements
                .Where(req => usedRequirementIds.Contains(GetInt(req, "ID")))
                .Select(ToUnlockRow)
                .Where(ShouldUnlock)
                .OrderBy(row => row.ID)
                .ToList();

            var script = GenerateScript(rows, _options);
            var outputPath = Path.Combine(outputDirectory, "customization-requirement-unlocks.txt");
            await File.WriteAllTextAsync(outputPath, script);
            Console.WriteLine(outputPath);
            Console.WriteLine($"Generated {rows.Count} customization requirement overrides.");
            return outputPath;
        }

        public static string GenerateScript(
            IEnumerable<CustomizationRequirementUnlockRow> rows,
            CustomizationRequirementUnlockOptions options)
        {
            var output = new StringBuilder();
            var hotfixId = options.HotfixStartId;

            output.AppendLine($"SET @VerifiedBuild = {options.VerifiedBuild};");
            output.AppendLine();

            foreach (var row in rows.OrderBy(row => row.ID))
            {
                var reqType = row.ReqType == NpcReqType ? ChoiceReqType : row.ReqType;
                var reqSource = EscapeSql(row.ReqSource);

                output.AppendLine("REPLACE INTO hotfixes.chr_customization_req");
                output.AppendLine("(ID, RaceMask, ReqSource, Flags, ClassMask, RegionGroupMask, AchievementID, QuestID, OverrideArchive, ItemModifiedAppearanceID, VerifiedBuild)");
                output.AppendLine($"VALUES ({row.ID}, {row.RaceMask}, '{reqSource}', {reqType}, {row.ClassMask}, {row.RegionGroupMask}, 0, 0, {row.OverrideArchive}, 0, @VerifiedBuild);");
                output.AppendLine($"INSERT INTO hotfixes.hotfix_data (Id, UniqueId, TableHash, RecordId, Status, VerifiedBuild) VALUES ({hotfixId}, 0, {(uint)TableHashes.CHR_CUSTOMIZATION_REQ}, {row.ID}, 1, @VerifiedBuild);");
                output.AppendLine();
                hotfixId++;
            }

            return output.ToString();
        }

        private static bool ShouldUnlock(CustomizationRequirementUnlockRow row)
        {
            return row.ReqType == NpcReqType
                || row.ReqAchievementID != 0
                || row.ReqQuestID != 0
                || row.ReqItemModifiedAppearanceID != 0;
        }

        private static CustomizationRequirementUnlockRow ToUnlockRow(DbRow row)
        {
            return new CustomizationRequirementUnlockRow(
                ID: GetInt(row, "ID"),
                ReqType: GetInt(row, "ReqType"),
                RaceMask: GetLong(row, "RaceMask"),
                ClassMask: GetInt(row, "ClassMask"),
                RegionGroupMask: GetInt(row, "RegionGroupMask"),
                ReqAchievementID: GetInt(row, "ReqAchievementID"),
                ReqQuestID: GetInt(row, "ReqQuestID"),
                OverrideArchive: GetInt(row, "OverrideArchive"),
                ReqItemModifiedAppearanceID: GetInt(row, "ReqItemModifiedAppearanceID"),
                ReqSource: GetString(row, "ReqSource"));
        }

        private static int GetInt(DbRow row, string columnName)
        {
            var column = row.Columns.FirstOrDefault(column => column.Name.Equals(columnName, StringComparison.InvariantCultureIgnoreCase));
            if (column?.Value == null)
                return 0;

            return Convert.ToInt32(column.Value);
        }

        private static long GetLong(DbRow row, string columnName)
        {
            var column = row.Columns.FirstOrDefault(column => column.Name.Equals(columnName, StringComparison.InvariantCultureIgnoreCase));
            if (column?.Value == null)
                return 0;

            return Convert.ToInt64(column.Value);
        }

        private static string GetString(DbRow row, string columnName)
        {
            var column = row.Columns.FirstOrDefault(column => column.Name.Equals(columnName, StringComparison.InvariantCultureIgnoreCase));
            return column?.Value?.ToString() ?? string.Empty;
        }

        private static string EscapeSql(string value)
        {
            return value.Replace("'", "''");
        }

        private string GetOutputDirectory()
        {
            return Path.Combine(_options.OutputPath, $"{nameof(CustomizationRequirementUnlockTool)}.{nameof(GenerateAsync)}");
        }
    }

    public sealed record CustomizationRequirementUnlockRow(
        int ID,
        int ReqType,
        long RaceMask,
        int ClassMask,
        int RegionGroupMask,
        int ReqAchievementID,
        int ReqQuestID,
        int OverrideArchive,
        int ReqItemModifiedAppearanceID,
        string ReqSource);

    public sealed class CustomizationRequirementUnlockOptions
    {
        public string Db2DataPath { get; set; } = string.Empty;
        public string OutputPath { get; set; } = string.Empty;
        public int HotfixStartId { get; set; } = 902100000;
        public int VerifiedBuild { get; set; } = -55500;
    }
}
