using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Configuration;

namespace MerCraftServer
{
    public class Config
    {
        
#if DEBUG
        private const bool DEFAULT_NOTIFY_BEHAVIOUR = true;
#else 
        private const bool DEFAULT_NOTIFY_BEHAVIOUR = false;
#endif

        private readonly string _configFileName;
        private readonly string _configDirectory;

        public event System.EventHandler<configFileChangedEventArgs> FileChanged;

        public Config(string configFileName)
            : this(configFileName, DEFAULT_NOTIFY_BEHAVIOUR)
        {
        }

        public Config(string __configFileName, bool notifyOnFileChange)
        {
            Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (!File.Exists(__configFileName))
            {
                string[] split = __configFileName.Split('\\');
                for (int i = 1; i <= split.Length; i++)
                {
                    string tmp = string.Join("\\", split, 0, i);
                    if (i == split.Length)
                    {
                        if (File.Exists(tmp))
                            continue;
                        else
                            Configuration.SaveAs(tmp, ConfigurationSaveMode.Full);
                    }
                    else
                    {
                        if (Directory.Exists(tmp))
                            continue;
                        else
                            Directory.CreateDirectory(tmp);
                    }
                }
            }
            
            _configFileName = __configFileName;
            _configDirectory = string.Join("\\", __configFileName.Split('\\'), 0, __configFileName.Split('\\').Length - 1);

            InitializeConfiguration();

            if (notifyOnFileChange)
                WatchConfigFile();
        }
        public System.Configuration.Configuration Configuration
        {
            get;
            set;
        }

        private void WatchConfigFile()
        {
            FileSystemWatcher watcher = new FileSystemWatcher(_configDirectory);
            watcher.Changed += ConfigFileChangedEvent;
        }

        public void InitializeConfiguration()
        {
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = _configFileName
            };

            Configuration = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            if (Configuration.Sections["versionInfo"] == null)
            {
                MerCraftServer.ConfigurationSections.versionInfo vI = new MerCraftServer.ConfigurationSections.versionInfo();
                Configuration.Sections.Add("versionInfo", vI);
            }
        }

        public void SaveConfiguration()
        {
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = _configFileName
            };
            Configuration.Save(ConfigurationSaveMode.Full, true);
        }

        private void ConfigFileChangedEvent(object sender, FileSystemEventArgs e)
        {
            if (FileChanged != null && e.ChangeType.HasFlag(WatcherChangeTypes.Changed))
                FileChanged(this, new configFileChangedEventArgs(_configFileName));
        }
    }
}
