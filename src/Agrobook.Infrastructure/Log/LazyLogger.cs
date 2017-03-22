using Agrobook.Core;
using System;

namespace Agrobook.Infrastructure.Log
{
    public class LazyLogger : ILogLite
    {
        private readonly Lazy<ILogLite> log;

        public LazyLogger(Func<ILogLite> factory)
        {
            Ensure.NotNull(factory, nameof(factory));

            this.log = new Lazy<ILogLite>(factory);
        }

        public void Error(string message)
        {
            this.log.Value.Error(message);
        }

        public void Error(Exception ex, string message)
        {
            this.log.Value.Error(ex, message);
        }

        public void Fatal(string message)
        {
            this.log.Value.Fatal(message);
        }

        public void Fatal(Exception ex, string message)
        {
            this.log.Value.Fatal(ex, message);
        }

        public void Info(string message)
        {
            this.log.Value.Info(message);
        }

        public void Verbose(string message)
        {
            this.log.Value.Verbose(message);
        }
    }
}
