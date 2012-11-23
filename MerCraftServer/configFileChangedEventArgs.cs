using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerCraftServer
{
    public class configFileChangedEventArgs : EventArgs
    {
        public readonly DateTime timeOfChange;
        public readonly string configFileName;

        public configFileChangedEventArgs(string fileName)
        {
            this.timeOfChange = DateTime.Now;
            this.configFileName = fileName;
        }
    }
}
