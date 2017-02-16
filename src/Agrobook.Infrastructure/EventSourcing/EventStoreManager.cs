using EventStore.ClientAPI;
using System.Diagnostics;

namespace Agrobook.Infrastructure.EventSourcing
{
    public static class EventStoreManager
    {
        private static IEventStoreConnection failFastConnection = null;
        private static IEventStoreConnection resilientConnection = null;

        private static string extIp = "127.0.0.1";

        private static Process _process;
        private const string path = @".\EventStore\EventStore.ClusterNode.exe";
        private const string baseArgs = "--db=./ESData --run-projections=all";

        public static void InitializeDb(
            string externalIp = null)
        {
            if (!string.IsNullOrWhiteSpace(externalIp))
                extIp = externalIp;

            string args = baseArgs;
            args += $" --ext-ip={extIp}";

            _process = new Process()
            {
                StartInfo =
                {
                    UseShellExecute = true, // Start in a new console shell
                    CreateNoWindow = false,
                    FileName = path,
                    Arguments = args,
                    Verb = "runas",
                }
            };

            _process.Start();
        }

        public static void InitializeConnectionPool()
        {

        }

        public static void EstablishFailFastConnection()
        {

        }

        public static void TearDown()
        {
            if (_process == null || _process.HasExited) return;

            _process.Kill();
            _process.WaitForExit();
        }
    }
}
