using Agrobook.Core;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Agrobook.Infrastructure.EventSourcing
{
    public class EventStoreManager
    {
        private Process process;
        private readonly string path = @".\EventStore\EventStore.ClusterNode.exe";
        private readonly string args = "--db=./ESData --start-standard-projections=true --run-projections=all";

        private readonly UserCredentials userCredentials;
        private readonly IPEndPoint ipEndPoint;

        private readonly string resilientConnectionNamePrefix;
        private readonly string failFastConnectionNamePrefix;

        private static int failFastNumber = 0;

        private bool failFastConnectionWasEstablished = false;
        private bool resilientConnectionWasEstablished = false;

        private IEventStoreConnection failFastConnection = null;
        private IEventStoreConnection resilientConnection = null;

        private readonly object resilientConnectionLock = new object();

        public EventStoreManager(
            string defaultUserName = "admin",
            string defaultPassword = "changeit",
            string extIp = "127.0.0.1",
            int tcpPort = 1113,
            string resilientConnectionNamePrefix = "anonymous-resilient",
            string failFastConnectionNamePrefix = "anonymous-fail-fast")
        {
            Ensure.NotNullOrWhiteSpace(defaultUserName, nameof(defaultUserName));
            Ensure.NotNullOrWhiteSpace(defaultPassword, nameof(defaultPassword));
            Ensure.NotNullOrWhiteSpace(extIp, nameof(extIp));
            Ensure.NotNullOrWhiteSpace(failFastConnectionNamePrefix, nameof(failFastConnectionNamePrefix));
            Ensure.NotNull(resilientConnectionNamePrefix, nameof(resilientConnectionNamePrefix));

            this.args += $" --ext-ip={extIp}";

            this.userCredentials = new UserCredentials(defaultUserName, defaultPassword);
            this.ipEndPoint = new IPEndPoint(IPAddress.Parse(extIp), tcpPort);

            this.resilientConnectionNamePrefix = resilientConnectionNamePrefix;
            this.failFastConnectionNamePrefix = failFastConnectionNamePrefix;
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

        public async Task<IEventStoreConnection> GetFailFastConnection()
        {
            if (!this.failFastConnectionWasEstablished)
            {
                await this.EstablishFailFastConnectionAsync();
                this.failFastConnectionWasEstablished = true;
            }

            return this.failFastConnection;
        }

        public IEventStoreConnection ResilientConnection
        {
            get
            {
                if (!this.resilientConnectionWasEstablished)
                {
                    lock (this.resilientConnectionLock)
                    {
                        if (!this.resilientConnectionWasEstablished)
                        {
                            this.EstablishResilientConnectionAsync().Wait();
                            this.resilientConnectionWasEstablished = true;
                        }
                    }
                }

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

            this.failFastConnection = EventStoreConnection.Create(settings, this.ipEndPoint, this.FormatConnectionName(this.failFastConnectionNamePrefix));
            this.failFastConnection.Closed += async (s, e) => await this.EstablishFailFastConnectionAsync();

            await this.failFastConnection.ConnectAsync();
        }

        private string FormatConnectionName(string prefix)
        {
            return $"{prefix}-{Interlocked.Increment(ref failFastNumber)}-{DateTime.UtcNow.ToString("dd/MM/yyy-hh:mm:ss")}";
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

            this.resilientConnection = EventStoreConnection.Create(settings, this.ipEndPoint, this.FormatConnectionName(this.resilientConnectionNamePrefix));
            await this.resilientConnection.ConnectAsync();
        }

        public void TearDown(bool deleteAll = false)
        {
            if (this.failFastConnectionWasEstablished)
                this.failFastConnection.Close();

            if (this.resilientConnectionWasEstablished)
                this.resilientConnection.Close();

            if (this.process != null && !this.process.HasExited)
            {
                this.process.Kill();
                this.process.WaitForExit();
            }

            if (deleteAll)
                Directory.Delete(@".\ESData", true);
        }
    }
}
