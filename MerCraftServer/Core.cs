using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;

namespace MerCraftServer
{

    class Core
    {
        public readonly Config config;
        public readonly string version;
        public Core()
        {
            config = new Config(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".mercraft-server\\config.cfg"), true);
            config.FileChanged += configFileChanged;
            MerCraftServer.ConfigurationSections.versionInfo vI;
            vI = config.Configuration.GetSection("versionInfo") as MerCraftServer.ConfigurationSections.versionInfo;
            version = vI.Version;
        }

        public void Log(object o, logLevel l)
        {
            Console.ForegroundColor = l.foreGround;
            Console.BackgroundColor = l.backGround;
            string bracketInfo = l.infoBracket;


            int maxLogLevelLength = -1;
            foreach (logLevel ll in logLevel.logLevelsArray)
            {
                if (ll.infoBracket.Length > maxLogLevelLength)
                    maxLogLevelLength = ll.infoBracket.Length;
            }

            if (maxLogLevelLength > 0 && bracketInfo.Length < maxLogLevelLength)
            {
                bracketInfo += new String(' ', maxLogLevelLength - bracketInfo.Length);
            }

            bracketInfo += " | ";

            Console.WriteLine(bracketInfo + o.ToString());
            Console.ResetColor();
        }

        public void Log(object o, ConsoleColor foreGround, ConsoleColor backGround)
        {
            Console.ForegroundColor = foreGround;
            Console.BackgroundColor = backGround;
            Console.WriteLine(o);
            Console.ResetColor();
        }

        private void configFileChanged(object sender, configFileChangedEventArgs e)
        {
            Log("Config file changed!", logLevel.Debug);
        }
    }
}
