using System.Collections.Generic;
using System.Linq;

namespace Agrobook
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

        public static string[] And(this string text, params string[] otherTexts)
        {
            var stringList = new List<string>() { text };
            stringList.AddRange(otherTexts);
            return stringList.ToArray();
        }

        public static byte[] ToByteArrayFromHexString(this string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            var hexValue = _hexValue;
            for (int x = 0, i = 0; i < hexString.Length; i += 2, x += 1)
                bytes[x] = (byte)(hexValue[char.ToUpper(hexString[i + 0]) - '0'] << 4
                            | hexValue[char.ToUpper(hexString[i + 1]) - '0']);
            return bytes;
        }

        private static uint[] _hexValue = new uint[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05,
       0x06, 0x07, 0x08, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
       0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
    }
}
