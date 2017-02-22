using System;
using System.Text;

namespace Agrobook.CLI.Utils
{
    public static class ExceptionExtensions
    {
        public static string GetAllMessages(this Exception ex)
        {
            var stringBuilder = new StringBuilder();
            while (ex != null)
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine(ex.ToString());
                ex = ex.InnerException;
            }

            return stringBuilder.ToString();
        }
    }
}
