using System;
using System.Runtime.InteropServices;

namespace Agrobook.Server
{
    partial class Program
    {
        // Source: http://stackoverflow.com/questions/474679/capture-console-exit-c-sharp
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(ExtConsoleHandler handler, bool add);
        private delegate bool ExtConsoleHandler(CtrlType signal);

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        internal static void OnProgramExit(Action onExit)
        {
            SetConsoleCtrlHandler(signal =>
            {
                onExit();
                // Shutdown right away
                Environment.Exit(-1);
                return true;
            }, true);
        }
    }
}
