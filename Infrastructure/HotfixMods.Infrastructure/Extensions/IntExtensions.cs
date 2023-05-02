namespace HotfixMods.Infrastructure.Extensions
{
    public static class IntExtensions
    {
        /// <summary>
        /// Adds a + or - to the number, depending on whether it is positive or negative. If 0, nothing is added.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToStringWithSign(this int value)
        {
            if (value <= 0)
                return value.ToString();
            return $"+{value}";
        }
    }
}
