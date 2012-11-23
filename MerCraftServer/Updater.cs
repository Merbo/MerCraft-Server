using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;

namespace MerCraftServer
{
    class Updater
    {
        WebClient webClient;
        string version;
        string serverVersion;
        string downloadFileName;

        public bool Ready = false;

        public Updater(string v)
        {
            version = v;
            webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(this.Complete);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.ProgressChanged);
        }

        public bool upToDate()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://173.48.92.80/MerCraft-server/version.txt");
            req.Method = "GET";
            WebResponse resp = req.GetResponse();
            StreamReader sr = new StreamReader(resp.GetResponseStream(), System.Text.Encoding.UTF8);
            serverVersion = sr.ReadToEnd();
            sr.Close();
            resp.Close();

            bool correctJar = false;

            try
            {
                correctJar = File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".mercraft-server", "Standard", "mercraft.jar"));
            }
            catch (Exception)
            {
                return false;
            }

            return version == serverVersion && correctJar;
        }

        public bool Update()
        {
            bool ret = true;

            try
            {
                if (!upToDate())
                {
                    Program.core.Log("Outdated. Updating to version " + serverVersion, logLevel.Download);
                    downloadFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".mercraft-server", "Standard.zip");
                    if (!File.Exists(downloadFileName))
                    {
                        string[] split = downloadFileName.Split('\\');
                        for (int i = 1; i <= split.Length; i++)
                        {
                            string tmp = string.Join("\\", split, 0, i);
                            if (i == split.Length)
                            {
                                if (File.Exists(tmp))
                                    return false;
                                else
                                    webClient.DownloadFileAsync(new Uri("http://173.48.92.80/MerCraft-server/Downloads/Version-" + serverVersion + "/Standard.zip"), downloadFileName);
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
                }
                else
                    Ready = true;
            }
            catch (Exception ex)
            {
                Program.core.Log(ex.ToString(), logLevel.Error);
            }

            return ret;
        }

        private void Complete(object sender, AsyncCompletedEventArgs e)
        {
            MerCraftServer.ConfigurationSections.versionInfo vI;
            vI = (MerCraftServer.ConfigurationSections.versionInfo)Program.core.config.Configuration.GetSection("versionInfo");
            vI.Version = serverVersion;
            Program.core.config.Configuration.Sections.Remove("versionInfo");
            Program.core.config.Configuration.Sections.Add("versionInfo", vI);
            Program.core.config.SaveConfiguration();

            Program.core.Log("Up to date; Unzipping file...", logLevel.Download);
            ZipFileHandler.UnZipFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".mercraft-server\\Standard.zip"));
            Ready = true;
            File.Delete(downloadFileName);
            this.version = serverVersion;
            Console.Title = "MerCraft Server Version " + this.version;
        }

        int lastPercentage = -1;
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage % 5 == 0 && e.ProgressPercentage != lastPercentage)
            {
                lastPercentage = e.ProgressPercentage;
                Program.core.Log("Updating: " + e.ProgressPercentage + "% (" + e.BytesReceived + " of " + e.TotalBytesToReceive + " bytes)", logLevel.Download);
            }
        }
    }
}
