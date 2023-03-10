using HotfixMods.Core.Models;

namespace HotfixMods.Tools.HotfixInitializer.Tool
{
    public partial class HotfixInitializerTool
    {
        public string GenerateDb2StoresH(string db2Name)
        {
            return $"TC_GAME_API extern DB2Storage<{db2Name}Entry> s{db2Name}Store;\r\n";
        }

        public string GenerateDb2StoresCpp1(string db2Name)
        {
            return $"DB2Storage<{db2Name}Entry> s{db2Name}Store(\"{db2Name}.db2\", &{db2Name}LoadInfo::Instance);\r\n";
        }

        public string GenerateDb2StoresCpp2(string db2Name)
        {
            return $"LOAD_DB2(s{db2Name}Store);\r\n";
        }

        public string GenerateDb2StructureH(string trinityCorePath, DbRowDefinition definition, string db2Name)
        {
            var result = "";
            var fields = GetFieldTypes(trinityCorePath, db2Name);
            if (fields.Count != definition.ColumnDefinitions.Where(d => d.Name != "VerifiedBuild").Count())
            {
                throw new Exception($"Unable to generate Db2Structure. Fields and Definitions are not the same length.");
            }
            result += $"struct {db2Name}Entry\r\n";
            result += "{\r\n";
            for (int i = 0; i < fields.Count; i++)
            {
                var field = fields[i];
                var defName = definition.ColumnDefinitions[i].Name;
                result += $"\t{GetDb2StructType(field.Item1, field.Item2)} {defName};\r\n";
            }
            result += "};\r\n";
            result += "\r\n";

            return result;
        }
    }
}
