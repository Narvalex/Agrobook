using Agrobook.Common;
using Eventing;

namespace Agrobook.CLI.Common
{
    public abstract class CommonController
    {
        private readonly string command;

        public CommonController(string command)
        {
            Ensure.NotNullOrWhiteSpace(command, nameof(command));

            this.command = command;
        }

        public bool WasInvoked(string cmd)
        {
            return cmd.Equals(this.command, System.StringComparison.OrdinalIgnoreCase);
        }

        public abstract string CommandDescription { get; }
    }
}