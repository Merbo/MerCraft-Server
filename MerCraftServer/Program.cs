using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace MerCraftServer
{
    class Program
    {
        public static Core core = null;
        public static Updater updater = null;
        public static Launcher launcher = null;
        public static ServerConfiguration serverConfig = null;
        private static bool isClosing = false;

        static void Main(string[] args)
        {
            SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);

            while (!isClosing)
            {
                Console.Title = "MerCraft Server";

                core = new Core();

#if DEBUG
                core.Log("Initializing Updater to version " + core.version, logLevel.Debug);
#endif
                Console.Title = "MerCraft Server Version " + core.version;

                updater = new Updater(core.version);
                updater.Update();

                while (!updater.Ready)
                {
                    //Do nothing
                }

#if DEBUG
                core.Log("Initializing Launcher to version " + core.version + " and updater", logLevel.Debug);
#endif

                serverConfig = new ServerConfiguration(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".mercraft-server", "Standard", "server.properties"));
                if (!serverConfig.configExists)
                    serverConfig.StartConfiguration();

                while (!serverConfig.Ready)
                {
                    //Do nothing
                }

                launcher = new Launcher(updater);
                launcher.Launch();
            }

            if (launcher != null)
                if (launcher.Running)
                    launcher.P.Kill();

            if (core != null)
                if (core.config != null)
                    core.config.SaveConfiguration();
        }

        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            // Put your own handler here
            switch (ctrlType)
            {
                case CtrlTypes.CTRL_C_EVENT:
                    isClosing = true;
                    break;

                case CtrlTypes.CTRL_BREAK_EVENT:
                    isClosing = true;
                    break;

                case CtrlTypes.CTRL_CLOSE_EVENT:
                    isClosing = true;
                    break;

                case CtrlTypes.CTRL_LOGOFF_EVENT:
                case CtrlTypes.CTRL_SHUTDOWN_EVENT:
                    isClosing = true;
                    break;

            }
            return true;
        }



        #region unmanaged
        // Declare the SetConsoleCtrlHandler function
        // as external and receiving a delegate.

        [DllImport("Kernel32.dll")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);

        // A delegate type to be used as the handler routine
        // for SetConsoleCtrlHandler.
        public delegate bool HandlerRoutine(CtrlTypes CtrlType);

        // An enumerated type for the control messages
        // sent to the handler routine.
        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }

        #endregion
    }
}
