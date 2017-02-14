using System.Diagnostics;

namespace Agrobook.Server
{
    public static class EventStoreLoader
    {
        private static Process _process;
        private const string path = @".\EventStore\EventStore.ClusterNode.exe";
        private const string args = "--config=./EventStore/config.yaml";

        internal static void Load()
        {
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

        internal static void TearDown()
        {
            if (_process == null || _process.HasExited) return;

            _process.Kill();
            _process.WaitForExit();
        }
    }
}
