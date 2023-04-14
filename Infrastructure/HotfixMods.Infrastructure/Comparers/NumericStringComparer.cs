using System.Text.RegularExpressions;

namespace HotfixMods.Infrastructure.Comparers
{
    public class NumericStringComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            // Extract the number from each string using a regular expression
            int xNum, yNum = 0;
            int.TryParse(Regex.Match(x, "(\\d+)").Value, out xNum);
            int.TryParse(Regex.Match(y, "(\\d+)").Value, out yNum);

            // Compare the numbers using the default comparer
            return Comparer<int>.Default.Compare(xNum, yNum);
        }
    }
}
