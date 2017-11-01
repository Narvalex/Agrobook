using Eventing.Log;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Eventing.Core.Utils
{
    public static class RetryIfNeeded
    {
        public static async Task<T> This<T>(TimeSpan timeout, ILogLite log, string operationName, Func<Task<T>> func)
        {
            Ensure.Positive(timeout.TotalMilliseconds, nameof(timeout));

            var sw = new Stopwatch();
            sw.Start();
            var count = 0;
            T result;
            do
            {
                try
                {
                    result = await func.Invoke();
                    return result;
                }
                catch (Exception ex)
                {
                    count++;
                    var elapsed = sw.Elapsed;
                    if (elapsed >= timeout)
                    {
                        log.Error(ex, $"La operación se reintentó demasiadas veces. Se reintentó {count} veces en {sw.Elapsed} segundos.");
                        throw;
                    }
                    else
                    {
                        log.Verbose($"{operationName}. Reintentando operacion. Intento #{count}");
                        Thread.Sleep(100 * count);
                    }
                }
            } while (true);
        }
    }
}
