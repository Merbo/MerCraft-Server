using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace MerCraftServer
{
    class ServerConfiguration
    {
        private StreamWriter writer;
        private string configFile;
        private string opsFile;
        public bool Ready = false;

        public ServerConfiguration(string pathToConfig)
        {
            string[] split = pathToConfig.Split('\\');
            opsFile = Path.Combine(string.Join("\\", split, 0, split.Length - 1), "ops.txt");
            configFile = pathToConfig;

            if (configExists && opsExists)
                Ready = true;         
        }

        public bool configExists
        {
            get
            {
                return File.Exists(configFile);
            }
        }

        public bool opsExists
        {
            get
            {
                return File.Exists(opsFile);
            }
        }

        public void StartConfiguration()
        {
            if (configExists)
                File.Delete(configFile);

            if (opsExists)
                File.Delete(opsFile);

            writer = File.CreateText(configFile);
            writer.AutoFlush = true;  

            writer.WriteLine("#MerCraft properties");
            writer.WriteLine("#" + DateTime.Now.ToString());

            
            Program.core.Log(new String('-', 25), logLevel.Config);
            Program.core.Log("Server configuration time!", logLevel.Config);
            Program.core.Log("Let us know some information your server will need.", logLevel.Config);     
            Program.core.Log(new String('-', 25), logLevel.Config);

            portQuestion:
            string portS;
            Program.core.Log("What port is the server to run on? [25566]", logLevel.Config);
            portS = Console.ReadLine();
            if (portS == "")
                portS = "25566";
            int port;
            if (int.TryParse(portS, out port))
                writer.WriteLine("server-port=" + portS);
            else
                goto portQuestion;

        modeQuestion:
            string gameMode;
            Program.core.Log("Is the game creative or survival? [Survival]", logLevel.Config);
            gameMode = Console.ReadLine();
            if (gameMode == "")
                gameMode = "Survival";
            gameMode = gameMode.ToLowerInvariant();
            switch (gameMode)
            {
                case "survival":
                    gameMode = "0";
                    break;
                case "creative":
                    gameMode = "1";
                    break;
                default:
                    goto modeQuestion;
            }

            switch (gameMode)
            {
                case "0":
                    writer.WriteLine("difficulty=3");
                    writer.WriteLine("spawn-animals=true");
                    writer.WriteLine("spawn-npcs=true");
                    writer.WriteLine("spawn-monsters=true");
                    writer.WriteLine("generate-structures=true");
                    writer.WriteLine("level-type=DEFAULT");
                    writer.WriteLine("gamemode=0");
                    break;
                case "1":
                    writer.WriteLine("difficulty=0");
                    writer.WriteLine("spawn-animals=false");
                    writer.WriteLine("spawn-npcs=false");
                    writer.WriteLine("spawn-monsters=false");
                    writer.WriteLine("generate-structures=false");
                    writer.WriteLine("level-type=FLAT");
                    writer.WriteLine("gamemode=1");
                    writer.WriteLine("generator-settings=2;7,4x3,2;4");
                    break;
                default:
                    goto modeQuestion;
            }

        motdQuestion:
            string motd;
            Program.core.Log("Server message of the day? [MerCraft Server]", logLevel.Config);
            motd = Console.ReadLine();
            if (motd == "")
                motd = "MerCraft Server";
            if (motd.Length < 59)
                writer.WriteLine("motd=" + motd);
            else
            {
                Program.core.Log("That value is too long. Keep the value less than 59 characters.", logLevel.Error);
                goto motdQuestion;
            }

        maxPlayersQuestion:
            string maxPlayersS;
            Program.core.Log("How many players may play at once? [30]", logLevel.Config);
            maxPlayersS = Console.ReadLine();
            if (maxPlayersS == "")
                maxPlayersS = "30";
            int maxPlayers;
            if (int.TryParse(maxPlayersS, out maxPlayers))
                writer.WriteLine("max-players=" + maxPlayers);
            else
                goto maxPlayersQuestion;

            writer.WriteLine("allow-nether=true");
            writer.WriteLine("level-name=MerbosMagic-Unofficial");
            writer.WriteLine("enable-query=false");
            writer.WriteLine("allow-flight=false");
            writer.WriteLine("enable-rcon=false");
            writer.WriteLine("level-seed=");
            writer.WriteLine("server-ip=");
            writer.WriteLine("max-build-height=256");
            writer.WriteLine("white-list=false");
            writer.WriteLine("snooper-enabled=true");
            writer.WriteLine("hardcore=false");
            writer.WriteLine("texture-pack=http\\://173.48.92.80/MerCraft/Faithful32.zip");
            writer.WriteLine("online-mode=false");
            writer.WriteLine("pvp=true");
            writer.WriteLine("enable-command-block=true");
            writer.WriteLine("view-distance=10");
            writer.WriteLine("spawn-protection=16");

            writer.Close();

            writer = File.CreateText(opsFile);
            writer.AutoFlush = true;

            Program.core.Log(new String('-', 25), logLevel.Config);
            Program.core.Log("Ops configuration", logLevel.Config);
            Program.core.Log("Enter a list of usernames you want to be operators.", logLevel.Config);
            Program.core.Log("Separate by newLines (enter key)", logLevel.Config);
            Program.core.Log(new String('-', 25), logLevel.Config);

            bool moarOps = true;
            while (moarOps)
            {
                string op;
                op = Console.ReadLine();
                if (op == "")
                    moarOps = false;
                else
                {
                    writer.WriteLine(op);
                    Program.core.Log("\"" + op + "\" is now an operator on " + motd, logLevel.Config);
                }
            }

            writer.Close();

            Ready = true;
        }
    }
}
