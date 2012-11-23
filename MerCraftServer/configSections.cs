using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MerCraftServer.ConfigurationSections
{
    public sealed class versionInfo : ConfigurationSection
    {
        public versionInfo()
        {

        }

        [ConfigurationProperty("version",
         DefaultValue = "1.0.0.0",
         IsRequired = true,
         IsKey = true)]
        public string Version
        {
            get
            {
                return (string)this["version"];
            }
            set
            {
                this["version"] = value;
            }
        }

        [ConfigurationProperty("beta",
            DefaultValue = (bool)false,
            IsRequired = false)]
        public bool Beta
        {
            get
            {
                return (bool)this["beta"];
            }
            set
            {
                this["beta"] = value;
            }
        }
    }
}
