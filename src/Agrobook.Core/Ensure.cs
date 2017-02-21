using System;

namespace Agrobook.Core
{
    public static class Ensure
    {
        public static void NotNull<T>(T argument, string argumentName) where T : class
        {
            if (argument == null) throw new ArgumentNullException(argumentName);
        }

        public static void NotNullOrWhiteSpace(string text, string argumentName)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException($"The text of '{argumentName}' should not be null or white space.");
        }

        public static void Positive(int number, string argumentName)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException(argumentName, $"{argumentName} should be positive.");
        }

        public static void Positive(double number, string argumentName)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException(argumentName, $"{argumentName} should be positive.");
        }
    }
}
