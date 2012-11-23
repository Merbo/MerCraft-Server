using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerCraftServer
{
    public sealed class logLevel
    {
        public string infoBracket;
        public ConsoleColor foreGround;
        public ConsoleColor backGround;

        public logLevel(string s, ConsoleColor cf, ConsoleColor cb)
        {
            infoBracket = s;
            foreGround = cf;
            backGround = cb;
        }

        public override String ToString()
        {
            return infoBracket;
        }

        public static readonly logLevel Error = new logLevel("[ERROR]", ConsoleColor.Red, ConsoleColor.Black);
        public static readonly logLevel Warning = new logLevel("[WARNING]", ConsoleColor.Yellow, ConsoleColor.Black);
        public static readonly logLevel Debug = new logLevel("[DEBUG]", ConsoleColor.Gray, ConsoleColor.Black);
        public static readonly logLevel Download = new logLevel("[DOWNLOAD]", ConsoleColor.Green, ConsoleColor.Black);
        public static readonly logLevel MerCraft = new logLevel("[MERCRAFT]", ConsoleColor.White, ConsoleColor.Black);
        public static readonly logLevel Config = new logLevel("[CONFIG]", ConsoleColor.Cyan, ConsoleColor.Black);

        public static readonly logLevel[] logLevelsArray = 
        {
            Error,
            Warning,
            Debug,
            Download,
            MerCraft,
            Config,
        };

    }
}
