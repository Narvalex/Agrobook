using System.Linq;

namespace System
{
    public static class StringExtensions
    {
        public static string ToTrimmedAndWhiteSpaceless(this string text)
        {
            text = new string(text.Trim().Where(c => !char.IsWhiteSpace(c)).ToArray());
            return text;
        }

        public static bool EqualsIgnoringCase(this string a, string b)
        {
            return a.Equals(b, System.StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
