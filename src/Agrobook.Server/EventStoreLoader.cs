using System.Diagnostics;

namespace Agrobook.Server
{
    public class EventStoreLoader
    {
        private static Process process;
        private const string path = @".\EventStore\EventStore.ClusterNode.exe";
        private const string args = "--config=./EventStore/config.yaml";

        internal static void Load()
        {
            process = new Process()
            {
                StartInfo =
                {
                    UseShellExecute = false, // Start in a new console shell
                    CreateNoWindow = false,
                    FileName = path,
                    Arguments = args,
                    Verb = "runas",
                }
            };

            process.Start();
        }
    }
}
