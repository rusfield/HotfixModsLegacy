using System.Reflection;

namespace HotfixMods.Tools.Dev.Business
{
    public class HotfixTableTool
    {
        public void GenerateAll(params Type[] db2Types)
        {

            var db2StoresH = new List<string>();
            var db2StoresCpp1 = new List<string>();
            var db2StoresCpp2 = new List<string>();
            var db2StructureH = new List<string>();

            foreach (var db2Type in db2Types)
            {
                db2StoresH.Add(GenerateDb2StoresH(db2Type));
                db2StoresCpp1.Add(GenerateDb2StoresCpp1(db2Type));
                db2StoresCpp2.Add(GenerateDb2StoresCpp2(db2Type));
                db2StructureH.Add(GenerateDb2StructureH(db2Type));
            }

            Console.WriteLine("* * * * * * * *");
            Console.WriteLine("* DB2Stores.h *");
            Console.WriteLine("* * * * * * * *");
            Console.WriteLine(string.Join("\r\n", db2StoresH));
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("* * * * * * * * * * * * *");
            Console.WriteLine("* DB2Stores.cpp (1 / 2) *");
            Console.WriteLine("* * * * * * * * * * * * *");
            Console.WriteLine(string.Join("\r\n", db2StoresCpp1));
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("* * * * * * * * * * * * *");
            Console.WriteLine("* DB2Stores.cpp (2 / 2) *");
            Console.WriteLine("* * * * * * * * * * * * *");
            Console.WriteLine(string.Join("\r\n", db2StoresCpp2));
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("* * * * * * * * * *");
            Console.WriteLine("* DB2Structure.h  *");
            Console.WriteLine("* * * * * * * * * *");
            Console.WriteLine(string.Join("\r\n", db2StructureH));
            Console.WriteLine();
            Console.WriteLine();

            /*
            Console.WriteLine("* * * * * * * *");
            Console.WriteLine("* MySQL Query *");
            Console.WriteLine("* * * * * * * *");
            Console.WriteLine(GenerateMySqlQuery(db2Type));
            Console.WriteLine();
            Console.WriteLine();
            */
        }

        public string GenerateDb2StoresH(Type db2Type)
        {
            string db2Name = db2Type.Name;
            return $"TC_GAME_API extern DB2Storage<{db2Name}Entry> s{db2Name}Store;";
        }

        public string GenerateDb2StoresCpp1(Type db2Type)
        {
            string db2Name = db2Type.Name;
            return $"DB2Storage<{db2Name}Entry> s{db2Name}Store(\"{db2Name}.db2\", &{db2Name}LoadInfo::Instance);";
        }

        public string GenerateDb2StoresCpp2(Type db2Type)
        {
            string db2Name = db2Type.Name;
            return $"LOAD_DB2(s{db2Name}Store);";
        }

        public string GenerateDb2StructureH(Type db2Type)
        {
            string db2Name = db2Type.Name;
            string output = "";
            output += $"struct {db2Name}Entry\r\n";
            output += "{\r\n";
            foreach (var propertyInfo in db2Type.GetProperties())
            {
                if (propertyInfo.Name == "VerifiedBuild")
                    continue;
                output += $"\t{GetMySqlFieldType(propertyInfo)} {GetMySqlFieldName(propertyInfo)};\r\n";
            }
            output += "};\r\n";
            return output;
        }

        public string GenerateHotfixDatabaseH(Type db2Type)
        {
            string output = "HOTFIX_SEL_";
            output += GetUnderscoreBetweenUpperCase(db2Type.Name);
            return $"{output.ToUpper()},\r\n{output.ToUpper()}_MAX_ID,\r\n";
        }

        public string GenerateHotfixDatabaseCpp(Type db2Type)
        {
            string db2Name = db2Type.Name;
            string output = $"// {db2Name}.db2\r\n";
            string prepareStatement = $"PrepareStatement(HOTFIX_SEL_{GetUnderscoreBetweenUpperCase(db2Type.Name).ToUpper()}, \"SELECT ";
            foreach (var propertyInfo in db2Type.GetProperties())
            {
                prepareStatement += $"{GetMySqlFieldName(propertyInfo)}, ";
            }
            prepareStatement = prepareStatement.Substring(0, prepareStatement.Length - 2);
            prepareStatement += $" FROM {GetUnderscoreBetweenUpperCase(db2Type.Name).ToLower()} WHERE (`VerifiedBuild` > 0) = ?\", CONNECTION_SYNCH);\r\n";

            string prepareMaxIdStmt = $"PREPARE_MAX_ID_STMT(HOTFIX_SEL_{GetUnderscoreBetweenUpperCase(db2Type.Name).ToUpper()}, \"SELECT MAX(ID) + 1 FROM {GetUnderscoreBetweenUpperCase(db2Type.Name).ToLower()}\", CONNECTION_SYNCH);\r\n";

            output += prepareStatement + prepareMaxIdStmt;
            return output;
        }

        public string GenerateMySqlQuery(Type db2Type)
        {
            string output = $"CREATE TABLE {GetUnderscoreBetweenUpperCase(db2Type.Name).ToLower()} (";
            foreach (var propertyInfo in db2Type.GetProperties())
            {
                output += $"{GetUnderscoreBetweenUpperCase(propertyInfo.Name).ToLower()} ";
                if (IsParentIndexField(propertyInfo))
                {
                    // Force unsigned
                    output += propertyInfo.PropertyType.ToString() switch
                    {
                        "System.SByte" => "tinyint unsigned",
                        "System.Int16" => "smallint unsigned",
                        "System.Int32" => "int unsigned",
                        "System.Int64" => "bigint unsigned",
                        "System.Byte" => "tinyint unsigned",
                        "System.UInt16" => "smallint unsigned",
                        "System.UInt32" => "int unsigned",
                        "System.UInt64" => "bigint unsigned",
                        "System.String" => "text",
                        "System.Decimal" => "float",
                        _ => $"(ERROR-({propertyInfo.PropertyType}))"
                    };
                }
                else
                {
                    output += propertyInfo.PropertyType.ToString() switch
                    {
                        "System.SByte" => "tinyint signed",
                        "System.Int16" => "smallint signed",
                        "System.Int32" => "int signed",
                        "System.Int64" => "bigint signed",
                        "System.Byte" => "tinyint unsigned",
                        "System.UInt16" => "smallint unsigned",
                        "System.UInt32" => "int unsigned",
                        "System.UInt64" => "bigint unsigned",
                        "System.String" => "text",
                        "System.Decimal" => "float",
                        _ => $"(ERROR-({propertyInfo.PropertyType}))"
                    };
                }
                output += ",";
            }
            output = output.Substring(0, output.Length - 1);
            output += ");\r\n";
            return output.Replace("verified_build", "VerifiedBuild", StringComparison.OrdinalIgnoreCase);
        }



        string GetUnderscoreBetweenUpperCase(string input)
        {
            string output = "";
            char previousChar = 'A';
            foreach (var letter in input.ToCharArray())
            {
                if (char.IsUpper(letter) && !char.IsUpper(previousChar))
                {
                    output += "_";
                }
                output += letter;
                previousChar = letter;
            }
            return output;
        }

        string GetMySqlFieldType(PropertyInfo propertyInfo)
        {

            if (IsParentIndexField(propertyInfo))
            {
                // Force unsigned
                return propertyInfo.PropertyType.ToString() switch
                {
                    "System.SByte" => "uint8",
                    "System.Int16" => "uint16",
                    "System.Int32" => "uint32",
                    "System.Int64" => "uint64",
                    "System.Byte" => "uint8",
                    "System.UInt16" => "uint16",
                    "System.UInt32" => "uint32",
                    "System.UInt64" => "uint64",
                    "System.String" => GetStringValue(propertyInfo),
                    "System.Decimal" => "float",
                    _ => $"(ERROR-({propertyInfo.PropertyType}))"
                };
            }
            else
            {
                return propertyInfo.PropertyType.ToString() switch
                {
                    "System.SByte" => "int8",
                    "System.Int16" => "int16",
                    "System.Int32" => "int32",
                    "System.Int64" => "int64",
                    "System.Byte" => "uint8",
                    "System.UInt16" => "uint16",
                    "System.UInt32" => "uint32",
                    "System.UInt64" => "uint64",
                    "System.String" => GetStringValue(propertyInfo),
                    "System.Decimal" => "float",
                    _ => $"(ERROR-({propertyInfo.PropertyType}))"
                };
            }
        }

        string GetStringValue(PropertyInfo propertyInfo)
        {
            var attributes = propertyInfo.GetCustomAttributes(true).OfType<Attribute>();
            foreach (var attribute in attributes)
            {
                var attrType = attribute.GetType();
                if (attrType.Name.Contains("LocalizedString", StringComparison.OrdinalIgnoreCase))
                {
                    return "LocalizedString";
                }
            }
            return "char const*";
        }

        string GetMySqlFieldName(PropertyInfo propertyInfo)
        {
            string name = propertyInfo.Name;
            return name;
        }

        bool IsParentIndexField(PropertyInfo propertyInfo)
        {
            var attributes = propertyInfo.GetCustomAttributes(true).OfType<Attribute>();
            foreach (var attribute in attributes)
            {
                var attrType = attribute.GetType();
                if (attrType.Name.Contains("parentindexfield", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
