using System.Linq;

namespace Agrobook.Core
{
    public static class StringExtensions
    {
        public static string ToLowerTrimmedAndWhiteSpaceless(this string text)
        {
            // Removing white spaces:
            // http://stackoverflow.com/questions/6219454/efficient-way-to-remove-all-whitespace-from-stringS

            text = new string(text.Trim().Where(c => !char.IsWhiteSpace(c)).ToArray()).ToLowerInvariant();
            return text;
        }

        public static bool EqualsIgnoringCase(this string a, string b)
        {
            return a.Equals(b, System.StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
