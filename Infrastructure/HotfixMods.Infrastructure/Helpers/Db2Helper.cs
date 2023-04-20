namespace HotfixMods.Infrastructure.Helpers
{
    public static class Db2Helper
    {
        public static string ConvertToHexColor(int color, bool maintainTransparency = false)
        {
            var hex = color.ToString("X8");
            return $"#{hex.Substring(2)}{(maintainTransparency ? hex.Substring(0, 2) : "FF")}";
        }
    }
}
