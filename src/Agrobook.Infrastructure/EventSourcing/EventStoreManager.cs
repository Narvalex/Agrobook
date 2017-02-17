using Agrobook.Core;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Agrobook.Infrastructure.EventSourcing
{
    public class EventStoreManager
    {
        private Process process;
        private readonly string path = @".\EventStore\EventStore.ClusterNode.exe";
        private readonly string args = "--db=./ESData --run-projections=all";

        private readonly UserCredentials userCredentials;
        private readonly IPEndPoint ipEndPoint;

        private IEventStoreConnection failFastConnection = null;
        private IEventStoreConnection resilientConnection = null;

        private object connectionPoolLock = new object();

        public EventStoreManager(
            string defaultUserName = "admin",
            string defaultPassword = "changeit",
            string extIp = "127.0.0.1",
            int tcpPort = 1113)
        {
            Ensure.NotNullOrWhiteSpace(defaultUserName, nameof(defaultUserName));
            Ensure.NotNullOrWhiteSpace(defaultPassword, nameof(defaultPassword));
            Ensure.NotNullOrWhiteSpace(extIp, nameof(extIp));

            this.args += $" --ext-ip={extIp}";

            this.userCredentials = new UserCredentials(defaultUserName, defaultPassword);
            this.ipEndPoint = new IPEndPoint(IPAddress.Parse(extIp), tcpPort);
        }

        public void InitializeDb()
        {
            this.process = new Process()
            {
                StartInfo =
                {
                    UseShellExecute = true, // Start in a new console shell
                    CreateNoWindow = false,
                    FileName = this.path,
                    Arguments = this.args,
                    Verb = "runas",
                }
            };

            this.process.Start();
        }

        public IEventStoreConnection FailFastConnection
        {
            get
            {
                if (this.failFastConnection is null)
                    lock (this.connectionPoolLock)
                        this.EstablishFailFastConnectionAsync().Wait();

                return this.failFastConnection;
            }
        }

        public IEventStoreConnection ResilientConnection
        {
            get
            {
                if (this.resilientConnection is null)
                    lock (this.connectionPoolLock)
                        this.EstablishResilientConnectionAsync().Wait();

                return this.resilientConnection;
            }
        }

        private async Task EstablishFailFastConnectionAsync()
        {
            var settings =
                ConnectionSettings
                .Create()
                .UseConsoleLogger()
                .SetDefaultUserCredentials(this.userCredentials)
                .Build();

            this.failFastConnection = EventStoreConnection.Create(settings, this.ipEndPoint);
            this.failFastConnection.Closed += (s, e) => this.EstablishFailFastConnectionAsync().Wait();

            await this.failFastConnection.ConnectAsync();
        }

        private async Task EstablishResilientConnectionAsync()
        {
            var settings =
                ConnectionSettings
                .Create()
                .KeepReconnecting()
                .KeepRetrying()
                .UseConsoleLogger()
                .SetDefaultUserCredentials(this.userCredentials)
                .Build();

            this.resilientConnection = EventStoreConnection.Create(settings, this.ipEndPoint);
            await this.resilientConnection.ConnectAsync();
        }

        public void TearDown(bool deleteAll = false)
        {
            lock (this.connectionPoolLock)
            {
                if (this.failFastConnection != null)
                    this.failFastConnection.Close();

                if (this.resilientConnection != null)
                    this.resilientConnection.Close();
            }

            if (this.process == null || this.process.HasExited) return;

            this.process.Kill();
            this.process.WaitForExit();

            if (deleteAll)
                Directory.Delete(@".\ESData", true);
        }
    }
}
