namespace Agrobook.Core
{
    public static class HelpfulExtensions
    {
        public static string ToLowerCamelCase(this string text)
        {
            return char.ToLower(text[0]) + text.Substring(1);
        }
    }
}
