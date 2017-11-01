using Eventing.Core.Domain;
using Eventing.Log;
using EventStore.ClientAPI.Exceptions;
using EventStore.ClientAPI.Projections;
using EventStore.ClientAPI.SystemData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventing.GetEventStore.Messaging
{
    public class ProjectionDefinition
    {
        private readonly ILogLite logger = LogManager.GetLoggerFor<ProjectionDefinition>();

        private ProjectionsManager manager;
        private UserCredentials credentials;
        private string projectionName;
        private string projectionScript;

        internal ProjectionDefinition(ProjectionsManager manager, UserCredentials credentials, string projectionName, string emittedStream, List<string> streams)
        {
            Ensure.NotNull(manager, nameof(manager));
            Ensure.NotNull(credentials, nameof(credentials));
            Ensure.NotNullOrWhiteSpace(projectionName, nameof(projectionName));
            Ensure.NotNullOrWhiteSpace(emittedStream, nameof(emittedStream));
            Ensure.NotNull(streams, nameof(streams));
            Ensure.Positive(streams.Count, nameof(streams));

            this.manager = manager;
            this.credentials = credentials;
            this.projectionName = projectionName;
            this.EmittedStream = emittedStream;
            this.projectionScript = buildScript(emittedStream, streams);
        }

        public static ProjectionDefinitionInitBuilder New(string projectionName, string emittedStream, ProjectionsManager manager, UserCredentials credentials)
            => new ProjectionDefinitionInitBuilder(projectionName, emittedStream, manager, credentials);

        public string EmittedStream { get; }

        public async Task EnsureExistence()
        {
            this.logger.Verbose($"Ensuring existence of projection {projectionName}...");

            string persistedScript;
            bool existeLaProyeccion;
            try
            {
                persistedScript = await this.manager.GetQueryAsync(this.projectionName, this.credentials);
                existeLaProyeccion = true;
                this.logger.Verbose($"The projection {this.projectionName} was found. Updating if needed..");
            }
            catch (ProjectionCommandFailedException ex)
            {
                if (ex.HttpStatusCode == 404)
                {
                    existeLaProyeccion = false;
                    persistedScript = null;
                    this.logger.Verbose($"The projection {this.projectionName} was NOT FOUND. Creating projection...");
                }
                else throw;
            }

            if (existeLaProyeccion && this.projectionScript != persistedScript)
                await this.manager.UpdateQueryAsync(this.projectionName, this.projectionScript, credentials);
            else if (!existeLaProyeccion)
                await this.manager.CreateContinuousAsync(this.projectionName, this.projectionScript, credentials);

            await this.manager.EnableAsync(this.projectionName, credentials);

            this.logger.Verbose($"The projection {this.projectionName} is up and running!");
        }

        private static string buildScript(string emittedStream, List<string> streams)
        {
            var streamsInString = streams.Aggregate(string.Empty, (acumulado, stringActual) => $"'{stringActual}',");
            return $"fromStreams([{streamsInString}]).when({{'$any':function(s, e) {{linkTo('{emittedStream}', e);}}}});";
        }
    }

    public class ProjectionDefinitionInitBuilder
    {
        internal ProjectionDefinitionInitBuilder(string projectionName, string emittedStream, ProjectionsManager manager, UserCredentials credentials)
        {
            this.ProjectionName = projectionName;
            this.EmittedStream = emittedStream;
            this.ProjectionManager = manager;
            this.Credentials = credentials;
        }

        internal string ProjectionName { get; }
        internal string EmittedStream { get; }
        internal ProjectionsManager ProjectionManager { get; }
        internal UserCredentials Credentials { get; }

        public ProjectionDefinitionBuilder From<T>() where T : class, IEventSourced, new()
        {
            return new ProjectionDefinitionBuilder(this, StreamCategoryAttribute.GetCategoryProjectionStream<T>());
        }
    }

    public class ProjectionDefinitionBuilder
    {
        private readonly ProjectionDefinitionInitBuilder init;
        private readonly List<string> streams;

        internal ProjectionDefinitionBuilder(ProjectionDefinitionInitBuilder init, string stream)
        {
            this.init = init;
            this.streams = new List<string>();
            this.streams.Add(stream);
        }

        public ProjectionDefinitionBuilder And<T>() where T : class, IEventSourced, new()
        {
            var stream = StreamCategoryAttribute.GetCategoryProjectionStream<T>();
            if (this.streams.Any(x => x == stream)) throw new ArgumentException("The stream was already registered!");
            this.streams.Add(stream);
            return this;
        }

        public ProjectionDefinition AndNothingMore()
            => new ProjectionDefinition(this.init.ProjectionManager, this.init.Credentials, this.init.ProjectionName, this.init.EmittedStream, this.streams);
    }
}
