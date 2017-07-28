using System;

namespace Eventing.Log
{
    public static class LogManager
    {
        private static Func<string, ILogLite> logFactory = n => new ConsoleLogger(n);
        public static ILogLite GlobalLogger => LogManager.GetLoggerFor("Global");

        static LogManager()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                var ex = e.ExceptionObject as Exception;
                if (ex != null)
                    GlobalLogger.Fatal(ex, "Global unhanled exception ocurred.");
                else
                    GlobalLogger.Fatal($"Global unhanled exception ocurred. Object error: {e.ExceptionObject}");
            };
        }

        public static ILogLite GetLoggerFor<T>()
        {
            return GetLoggerFor(typeof(T).Name);
        }

        public static ILogLite GetLoggerFor(string name)
        {
            return new LazyLogger(() => logFactory.Invoke(name));
        }
    }
}
