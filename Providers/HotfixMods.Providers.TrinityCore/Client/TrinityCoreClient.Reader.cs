using System.Reflection;
using System.Text.RegularExpressions;

namespace HotfixMods.Providers.TrinityCore.Client
{
    public partial class TrinityCoreClient
    {
        public List<(Type, bool)> GetFieldTypes(string db2Name)
        {
            var results = new List<(Type, bool)>();
            var pattern = $@"^(?:.*\s+)?struct\s+{db2Name}Meta\s*{{\s*(.*?)\s*}}";
            var structRegex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            using (var reader = new StreamReader(GetDb2MetadataStream()))
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
                        (var indexField, var parentIndexField) = GetInstanceParameters(content, db2Name);

                        for(int i = 0; i < fieldLines.Length; i++)
                        {
                            var line = fieldLines[i];
                            var linePattern = @"\{\s*([^,]+),\s*([^,]+),\s*([^,]+)\s*\}";
                            var lineRegex = new Regex(linePattern);
                            var lineMatch = lineRegex.Match(line);

                            if (lineMatch.Success)
                            {
                                var tcType = lineMatch.Groups[1].Value.Trim();
                                var lengthAsArray = int.Parse(lineMatch.Groups[2].Value.Trim());
                                var isSigned = bool.Parse(lineMatch.Groups[3].Value.Trim());


                                for (int j = 0; j < lengthAsArray; j++)
                                {
                                    results.Add(
                                        tcType switch
                                        {
                                            "FT_STRING" => (typeof(string), true),
                                            "FT_STRING_NOT_LOCALIZED" => (typeof(string), false),
                                            "FT_BYTE" => (isSigned ? typeof(sbyte) : typeof(byte), i == indexField || i == parentIndexField),
                                            "FT_SHORT" => (isSigned ? typeof(short) : typeof(ushort), i == indexField || i == parentIndexField),
                                            "FT_INT" => (isSigned ? typeof(int) : typeof(uint), i == indexField || i == parentIndexField),
                                            "FT_LONG" => (isSigned ? typeof(long) : typeof(ulong), i == indexField || i == parentIndexField),
                                            "FT_FLOAT" => (typeof(decimal), i == indexField || i == parentIndexField),
                                            _ => throw new Exception($"Unhandled type: {tcType}.")
                                        });
                                }
                            }
                        }
                    }
                }
            }

            return results;
        }

        (int, int) GetInstanceParameters(string content, string db2Name)
        {
            var pattern = $@"(?:.*\s+)?struct\s+{db2Name}Meta\s*{{\s*(.*?)\s*(static\s+constexpr\s+DB2Meta\s+Instance{{\s*(.*?)\s*}};)\s*}}";
            var structRegex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            var structMatch = structRegex.Match(content);
            if (structMatch.Success)
            {
                string instanceLine = structMatch.Groups[2].Value;
                var instanceRegex = new Regex(@"\{(.+?)\};", RegexOptions.Singleline);
                var instanceMatch = instanceRegex.Match(instanceLine);
                if (instanceMatch.Success)
                {
                    string[] instanceValues = instanceMatch.Groups[1].Value.Split(',');

                    int indexField = int.Parse(instanceValues[1].Trim());
                    int parentIndexField = int.Parse(instanceValues[6].Trim());

                    return (indexField, parentIndexField);
                }
            }


            throw new Exception($"Unable to read DB2Metadata.h for {db2Name}.");
        }

        Stream GetDb2MetadataStream()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames = assembly.GetManifestResourceNames();
            foreach (var resourceName in resourceNames)
            {
                if (string.Equals(resourceName, $"HotfixMods.Providers.TrinityCore.Db2Metadata.DB2Metadata.h", StringComparison.OrdinalIgnoreCase))
                {
                    return assembly.GetManifestResourceStream(resourceName);
                }
            }
            throw new Exception($"DB2MEtadata.h not found.");
        }
    }
}
