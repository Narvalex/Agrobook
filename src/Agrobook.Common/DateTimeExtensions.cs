using System;

namespace Agrobook
{
    public static class DateTimeExtensions
    {
        public static void EnsureIsNotDefault(this DateTime date, string argumentName)
        {
            if (date == default(DateTime))
                throw new ArgumentOutOfRangeException(argumentName, $"La fecha de {argumentName} es inválida");
        }
    }
}
