using System;

namespace Eventing.Log
{
    public class ConsoleLogger : ILogLite
    {
        private readonly LogMessageBuilder messageBuilder;

        private const string ERROR_level = "ERROR";
        private const string FATAL_level = "FATAL";
        private const string INFO_level = "info";
        private const string VERBOSE_level = "verbose";
        private const string WARNING_level = "warning";

        private static object lockObject = new object();

        public ConsoleLogger(string componentName)
        {
            Ensure.NotNullOrWhiteSpace(componentName, nameof(componentName));

            this.messageBuilder = new LogMessageBuilder(componentName);
        }

        public void Error(string message)
        {
            this.WriteWithLock(this.messageBuilder.BuildMessage(ERROR_level, message), ConsoleColor.Red);
        }

        public void Error(Exception ex, string message)
        {
            this.WriteWithLock(this.messageBuilder.BuildMessage(ex, ERROR_level, message), ConsoleColor.Red);
        }

        public void Fatal(string message)
        {
            this.WriteWithLock(this.messageBuilder.BuildMessage(FATAL_level, message), ConsoleColor.White, ConsoleColor.Red);
        }

        public void Fatal(Exception ex, string message)
        {
            this.WriteWithLock(this.messageBuilder.BuildMessage(ex, FATAL_level, message), ConsoleColor.White, ConsoleColor.Red);
        }

        public void Info(string message)
        {
            this.WriteWithLock(this.messageBuilder.BuildMessage(INFO_level, message));
        }

        public void Warning(string message)
        {
            this.WriteWithLock(this.messageBuilder.BuildMessage(WARNING_level, message), ConsoleColor.Yellow);
        }

        public void Verbose(string message)
        {
            this.WriteWithLock(this.messageBuilder.BuildMessage(VERBOSE_level, message), ConsoleColor.DarkGray);
        }

        public void Success(string message)
        {
            this.WriteWithLock(this.messageBuilder.BuildMessage(VERBOSE_level, message), ConsoleColor.Green);
        }

        private void WriteWithLock(string message)
        {
            lock (lockObject)
            {
                Console.WriteLine(message);
            }
        }

        private void WriteWithLock(string message, ConsoleColor foregroundColor)
        {
            lock (lockObject)
            {
                Console.ForegroundColor = foregroundColor;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }

        private void WriteWithLock(string message, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            lock (lockObject)
            {
                Console.ForegroundColor = foregroundColor;
                Console.BackgroundColor = backgroundColor;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }
    }
}
