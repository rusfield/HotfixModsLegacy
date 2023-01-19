using System.Reflection;

namespace HotfixMods.Tools.Dev.Business
{
    public class HotfixTableTool
    {
        public void GenerateAll(Type db2Type)
        {
            Console.WriteLine("DB2Stores.h");
            Console.WriteLine(GenerateDb2StoresH(db2Type.Name));
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("DB2Stores.cpp (1 / 2)");
            Console.WriteLine(GenerateDb2StoresCpp1(db2Type.Name));
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("DB2Stores.cpp (2 / 2)");
            Console.WriteLine(GenerateDb2StoresCpp2(db2Type.Name));
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("DB2Stores.cpp (2 / 2)");
            Console.WriteLine(GenerateDb2StructureH(db2Type.Name, db2Type.GetProperties()));
            Console.WriteLine();
            Console.WriteLine();
        }

        public string GenerateDb2StoresH(string db2Name)
        {
            return $"TC_GAME_API extern DB2Storage<{db2Name}Entry> s{db2Name}Store;";
        }

        public string GenerateDb2StoresCpp1(string db2Name)
        {
            return $"DB2Storage<{db2Name}Entry> s{db2Name}Store(\"{db2Name}.db2\", {db2Name}LoadInfo::Instance());";
        }

        public string GenerateDb2StoresCpp2(string db2Name)
        {
            return $"LOAD_DB2(s{db2Name}Store);";
        }

        public string GenerateDb2StructureH(string db2Name, PropertyInfo[] propertyInfos)
        {
            string output = "";
            output += $"struct {db2Name}Entry\r\n";
            output += "{\r\n";
            foreach (var propertyInfo in propertyInfos)
            {
                output += $"\t{GetMySqlFieldType(propertyInfo)} {GetMySqlFieldName(propertyInfo)};\r\n";
            }
            output += "};\r\n";
            return output;
        }

        string GetMySqlFieldType(PropertyInfo propertyInfo)
        {
            var attributes = propertyInfo.GetCustomAttributes(true).OfType<Attribute>();
            foreach (var attribute in attributes)
            {
                var attrType = attribute.GetType();
                if (attrType.Name.Contains("parentindexfield", StringComparison.OrdinalIgnoreCase))
                {
                    return "uint32";
                }
            }


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
                "System.String" => "text",
                "System.Decimal" => "float",
                _ => $"(ERROR-({propertyInfo.PropertyType}))"
            };
        }

        string GetMySqlFieldName(PropertyInfo propertyInfo)
        {
            string name = propertyInfo.Name;
            if (name.EndsWith("id", StringComparison.OrdinalIgnoreCase))
            {
                name = name.Substring(0, name.Length - 2) + "ID";
            }
            return name;
        }
    }
}
