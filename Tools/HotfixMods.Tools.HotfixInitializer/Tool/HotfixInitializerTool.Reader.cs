using HotfixMods.Core.Models;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace HotfixMods.Tools.HotfixInitializer.Tool
{
    public partial class HotfixInitializerTool
    {
        List<(Type, bool)> GetFieldTypes(string db2Name)
        {
            var results = new List<(Type, bool)>();

            using (var reader = new StreamReader(GetDb2MetadataStream()))
            {
                bool match = false;
                bool lastBreak = false;
                var content = "";
                while (reader.Peek() != -1)
                {
                    var line = reader.ReadLine();
                    if (line == null)
                        break;
                    else
                        line = line.Trim();

                    if (line.Equals($"struct {db2Name}Meta", StringComparison.InvariantCultureIgnoreCase))
                        match = true;

                    if (match)
                        content += line + "\r\n";

                    if (match && line.Equals("};"))
                    {
                        if(lastBreak)
                            break;
                        else
                            lastBreak = true;
                    }
                }

                if (match)
                {
                    var fieldPattern = @"{\s*((?:\{.*?\}|[^{}])*)\s*};";
                    var fieldRegex = new Regex(fieldPattern, RegexOptions.Singleline);
                    var fieldMatch = fieldRegex.Match(content);

                    if (fieldMatch.Success)
                    {
                        var fieldValues = fieldMatch.Groups[1].Value;
                        var fieldLines = fieldValues.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                        (var indexField, var parentIndexField) = GetInstanceParameters(content, db2Name);

                        for (int i = 0; i < fieldLines.Length; i++)
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
                if (string.Equals(resourceName, $"HotfixMods.Tools.HotfixInitializer.Db2Metadata.DB2Metadata.h", StringComparison.OrdinalIgnoreCase))
                {
                    return assembly.GetManifestResourceStream(resourceName);
                }
            }
            throw new Exception($"DB2MEtadata.h not found.");
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


        // Method that takes name and fileContent parameters and returns the struct part
        public string ExtractStruct(string name, string fileContent)
        {
            // Create a regex pattern that matches a struct declaration with curly braces and any content inside them
            // The pattern uses a named group "struct" to capture the whole struct part
            // The pattern also uses a backreference "\k<name>" to match the same name parameter as before 
            string pattern = @"struct\s+(?<name>" + name + @")\s*Meta\s*\{(?<struct>.*?)\};";

            // Create a regex object with the pattern
            Regex regex = new Regex(pattern);

            // Apply the regex to the file content and get the first match
            Match match = regex.Match(fileContent);

            // If there is a match,
            if (match.Success)
            {
                // Return the captured group "struct" with some formatting 
                return "struct " + name + "Meta\n{\n" + match.Groups["struct"].Value + "\n};";

            }

            else
            {
                // Otherwise return an empty string or an error message 
                return "";

            }

        }
    }
}
