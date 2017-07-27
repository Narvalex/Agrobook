namespace Eventing
{
    public static class StringExtensions
    {
        public static string WithFirstCharInLower(this string text)
        {
            return char.ToLower(text[0]) + text.Substring(1);
        }
    }
}
