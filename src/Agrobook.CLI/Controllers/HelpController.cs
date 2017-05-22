using Agrobook.CLI.Common;
using Agrobook.CLI.Views;
using System.Linq;

namespace Agrobook.CLI.Controllers
{
    public class HelpController : CommonController
    {
        private readonly HelpView view;
        private readonly string[] availableCommands;

        public HelpController(HelpView view, params CommonController[] controllers) : base("help")
        {
            this.view = view;
            this.availableCommands = controllers.Select(c => c.CommandDescription).ToArray();
        }

        public override string CommandDescription => "Type help to see available commands";

        public void StartHelpCommandLoop()
        {
            this.view.ShowAvailableCommands(this.availableCommands);
        }
    }
}
