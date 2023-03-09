using System.Text.RegularExpressions;

namespace HotfixMods.Infrastructure.Comparers
{
    public class NumericStringComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            // Extract the number from each string using a regular expression
            int xNum = int.Parse(Regex.Match(x, "(\\d+)").Value);
            int yNum = int.Parse(Regex.Match(y, "(\\d+)").Value);

            // Compare the numbers using the default comparer
            return Comparer<int>.Default.Compare(xNum, yNum);
        }
    }
}
