using Eventing.Log;
using System;

namespace Eventing.GetEventStore
{
    public class EventStoreLogAdapter : EventStore.ClientAPI.ILogger
    {
        private readonly ILogLite logger;

        public EventStoreLogAdapter(ILogLite logger)
        {
            Ensure.NotNull(logger, nameof(logger));

            this.logger = logger;
        }

        public void Debug(string format, params object[] args)
        {
            this.logger.Verbose(string.Format(format, args));
        }

        public void Debug(Exception ex, string format, params object[] args)
        {
            this.logger.Error(ex, string.Format(format, args));
        }

        public void Error(string format, params object[] args)
        {
            this.logger.Error(string.Format(format, args));
        }

        public void Error(Exception ex, string format, params object[] args)
        {
            this.logger.Error(ex, string.Format(format, args));
        }

        public void Info(string format, params object[] args)
        {
            this.logger.Info(string.Format(format, args));
        }

        public void Info(Exception ex, string format, params object[] args)
        {
            this.logger.Error(ex, string.Format(format, args));
        }
    }
}
