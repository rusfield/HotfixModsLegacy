using System.Text.RegularExpressions;

namespace HotfixMods.Tools.Dev.Business
{
    public class TrinityCoreCodeTool
    {
        public void GetFields(string db2Name)
        {
            List<(string, int, bool)> values = new();
            var pattern = $@"^(?:.*\s+)?struct\s+{db2Name}Meta\s*{{\s*(.*?)\s*}}";
            var structRegex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var filePath = @"C:\Users\Disconnected\source\repos\TrinityCore\src\server\game\DataStores\DB2Metadata.h";

            using (var reader = new StreamReader(filePath))
            {
                var content = reader.ReadToEnd();

                var structMatch = structRegex.Match(content);
                if (structMatch.Success)
                {
                    var fieldPattern = @"{\s*((?:\{.*?\}|[^{}])*)\s*};";
                    var fieldRegex = new Regex(fieldPattern, RegexOptions.Singleline);
                    var fieldMatch = fieldRegex.Match(content);

                    if (fieldMatch.Success)
                    {
                        var fieldValues = fieldMatch.Groups[1].Value;
                        var fieldLines = fieldValues.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

                        foreach (var line in fieldLines)
                        {
                            var linePattern = @"\{\s*([^,]+),\s*([^,]+),\s*([^,]+)\s*\}";
                            var lineRegex = new Regex(linePattern);
                            var lineMatch = lineRegex.Match(line);

                            if (lineMatch.Success)
                            {
                                var strValue = lineMatch.Groups[1].Value.Trim();
                                var intValue = int.Parse(lineMatch.Groups[2].Value.Trim());
                                var boolValue = bool.Parse(lineMatch.Groups[3].Value.Trim());

                                values.Add((strValue, intValue, boolValue));
                            }
                        }
                    }
                }
            }
        }

        public void GetInstanceParameters(string db2Name)
        {
            var pattern = $@"(?:.*\s+)?struct\s+{db2Name}Meta\s*{{\s*(.*?)\s*(static\s+constexpr\s+DB2Meta\s+Instance{{\s*(.*?)\s*}};)\s*}}";
            var structRegex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var filePath = @"C:\Users\Disconnected\source\repos\TrinityCore\src\server\game\DataStores\DB2Metadata.h";

            using (var reader = new StreamReader(filePath))
            {
                var content = reader.ReadToEnd();
                var structMatch = structRegex.Match(content);
                if (structMatch.Success)
                {
                    string instanceLine = structMatch.Groups[2].Value;
                    var instanceRegex = new Regex(@"\{(.+?)\};", RegexOptions.Singleline);
                    var instanceMatch = instanceRegex.Match(instanceLine);
                    if (instanceMatch.Success)
                    {
                        string[] instanceValues = instanceMatch.Groups[1].Value.Split(',');
                        uint fileDataId = uint.Parse(instanceValues[0].Trim());
                        int indexField = int.Parse(instanceValues[1].Trim());
                        uint fieldCount = uint.Parse(instanceValues[2].Trim());
                        uint fileFieldCount = uint.Parse(instanceValues[3].Trim());
                        // skip hex value
                        int parentIndexField = int.Parse(instanceValues[6].Trim());

                        // Do something with the parsed values here...
                    }
                }
            }
        }
    }
}
