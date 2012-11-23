using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MerCraftServer
{
    class Launcher
    {
        public Process P = null;
        Updater U;

        public StreamReader standardOutput = null;
        public StreamWriter standardInput = null;
        public StreamReader consoleInput;

        private string getJava()
        {
            return Path.Combine(JavaDetect.JavaPath.GetJavaBinaryPath(), "java.exe");
        }

        public Launcher(Updater updater)
        {
            U = updater;
            consoleInput = new StreamReader(Console.OpenStandardInput());
        }

        public void Launch()
        {
            string pathToJar = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".mercraft-server", "Standard", "mercraft.jar");
            if (File.Exists(pathToJar))
            {
                P = new Process();
                P.StartInfo.FileName = getJava();
                P.StartInfo.UseShellExecute = false;
                P.StartInfo.RedirectStandardOutput = true;
                P.StartInfo.RedirectStandardInput = true;
                P.StartInfo.Arguments = "" +
                "-Xmx2G " +
                "-Xmn512M " +
                "-jar \"" + pathToJar + "\" " +
                "nogui";

                P.StartInfo.WorkingDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".mercraft-server", "Standard");

                P.Start();

                standardInput = P.StandardInput;
                standardOutput = P.StandardOutput;

                getStandardIO();
            }
        }

        private async void getStandardIO()
        {
            while (standardOutput.Peek() >= 0 && Running)
            {
                Task<string> line = standardOutput.ReadLineAsync();

                if (consoleInput.Peek() >= 0)
                    standardInput.WriteLine(await consoleInput.ReadLineAsync());

                Program.core.Log(await line, logLevel.MerCraft);
            }
        }

        public bool Running
        {
            get
            {
                if (this.P != null)
                    if (!this.P.HasExited)
                        return true;
                return false;
            }
        }
    }
}
