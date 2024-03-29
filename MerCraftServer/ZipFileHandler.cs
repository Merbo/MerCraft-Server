﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;

namespace MerCraftServer
{
    class ZipFileHandler
    {
        public static bool UnZipFile(string InputPathOfZipFile)
        {
            bool ret = true;
            try
            {
                if (File.Exists(InputPathOfZipFile))
                {
                    string baseDirectory = Path.GetDirectoryName(InputPathOfZipFile);

                    using (ZipInputStream ZipStream = new ZipInputStream(File.OpenRead(InputPathOfZipFile)))
                    {
                        ZipEntry theEntry;
                        while ((theEntry = ZipStream.GetNextEntry()) != null)
                        {
                            if (theEntry.IsFile)
                            {
                                if (theEntry.Name != "")
                                {
                                    string strNewFile = @"" + baseDirectory + @"\" + 
                                        theEntry.Name;
                                    if (File.Exists(strNewFile))
                                    {
                                        continue;
                                    }

                                    using (FileStream streamWriter = File.Create(strNewFile))
                                    {
                                        int size = 2048;
                                        byte[] data = new byte[2048];
                                        while (true)
                                        {
                                            size = ZipStream.Read(data, 0, data.Length);
                                            if (size > 0)
                                                streamWriter.Write(data, 0, size);
                                            else
                                                break;
                                        }
                                        streamWriter.Close();
                                    }
                                }
                            }
                            else if (theEntry.IsDirectory)
                            {
                                string strNewDirectory = @"" + baseDirectory + @"\" + 
                                    theEntry.Name;
                                if (!Directory.Exists(strNewDirectory))
                                {
                                    Directory.CreateDirectory(strNewDirectory);
                                }
                            }
                        }
                        ZipStream.Close();
                    }
                }
            }
            catch (Exception)
            {
                ret = false;
            }
            return ret;
        }

    }
}
