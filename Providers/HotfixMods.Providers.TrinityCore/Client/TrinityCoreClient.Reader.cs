using HotfixMods.Core.Models;
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
                                            "FT_BYTE" => (isSigned ? typeof(sbyte) : typeof(byte), i == parentIndexField),
                                            "FT_SHORT" => (isSigned ? typeof(short) : typeof(ushort), i == parentIndexField),
                                            "FT_INT" => (isSigned ? typeof(int) : typeof(uint), i == parentIndexField),
                                            "FT_LONG" => (isSigned ? typeof(long) : typeof(ulong), i == parentIndexField),
                                            "FT_FLOAT" => (typeof(decimal), i == parentIndexField),
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

        public string GenerateDb2StoresH(string db2Name)
        {
            return $"TC_GAME_API extern DB2Storage<{db2Name}Entry> s{db2Name}Store;";
        }

        public string GenerateDb2StoresCpp1(string db2Name)
        {
            return $"DB2Storage<{db2Name}Entry> s{db2Name}Store(\"{db2Name}.db2\", &{db2Name}LoadInfo::Instance);";
        }

        public string GenerateDb2StoresCpp2(string db2Name)
        {
            return $"LOAD_DB2(s{db2Name}Store);";
        }

        public string GenerateDb2StructureH(string db2Name, List<(Type, bool)> fields, DbRowDefinition definition)
        {
            if(fields.Count != definition.ColumnDefinitions.Count)
            {
                throw new Exception($"Unable to generate Db2Structure. Fields and Definitions are not the same length.");
            }
            string output = "";
            output += $"struct {db2Name}Entry\r\n";
            output += "{\r\n";
            for(int i = 0; i < fields.Count; i++)
            {
                var field = fields[i];
                var defName = definition.ColumnDefinitions[i].Name;
                output += $"\t{GetDb2StructType(field.Item1, field.Item2)} {defName};\r\n";
            }
            output += "};\r\n";
            return output;
        }

        string GetDb2StructType(Type type, bool isTrue)
        {
            // isTrue is true for LocalizedStrings or ParentIndexFields
            if (isTrue)
            {
                // Force unsigned or localized
                return type.ToString() switch
                {
                    "System.SByte" => "uint8",
                    "System.Int16" => "uint16",
                    "System.Int32" => "uint32",
                    "System.Int64" => "uint64",
                    "System.Byte" => "uint8",
                    "System.UInt16" => "uint16",
                    "System.UInt32" => "uint32",
                    "System.UInt64" => "uint64",
                    "System.String" => "LocalizedString",
                    "System.Decimal" => "float",
                    _ => throw new Exception($"Unable to get Db2Structure Type for {type.ToString()},")
                };
            }
            else
            {
                return type.ToString() switch
                {
                    "System.SByte" => "int8",
                    "System.Int16" => "int16",
                    "System.Int32" => "int32",
                    "System.Int64" => "int64",
                    "System.Byte" => "uint8",
                    "System.UInt16" => "uint16",
                    "System.UInt32" => "uint32",
                    "System.UInt64" => "uint64",
                    "System.String" => "LocalizedString",
                    "System.Decimal" => "float",
                    _ => throw new Exception($"Unable to get Db2Structure Type for {type.ToString()},")
                };
            }
        }
    }
}
