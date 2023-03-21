namespace HotfixMods.Providers.Listfile.Client
{
    public partial class ListfileClient
    {
        string UnderscoreToCase(string value)
        {
            string result = "";
            var words = value.ToString().Split("_");
            foreach (var word in words)
            {
                if (word.Length > 1)
                    result += $"{word.Substring(0, 1).ToUpper()}{word.Substring(1, word.Length - 1).ToLower()} ";
                else if (word.Length == 1)
                    result += $"{word.ToString().ToUpper()} ";
            }
            return result;
        }
    }
}
