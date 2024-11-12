using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Skyray.EDX.Common.Library
{
    public class FileFoundHelper
    {
        public static List<FileInfo> GetDirAllFiles(string path, string prefix)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(prefix))
                return null;
            DirectoryInfo dirInfos = new DirectoryInfo(path);
            if (!dirInfos.Exists)
                return null;
            return GetCurrentAllFiles(path, prefix);
        }

        public static List<FileInfo> GetCurrentAllFiles(string path,string prefix)
        {
            List<FileInfo> returnAllFiles = new List<FileInfo>();
            DirectoryInfo info = new DirectoryInfo(path);
            FileInfo[] files = info.GetFiles("*." + prefix);
            if (files != null && files.Length > 0)
                 returnAllFiles.AddRange(files);
            DirectoryInfo[] dires = info.GetDirectories();
            if (dires == null || dires.Length == 0)
                return returnAllFiles;
            foreach (DirectoryInfo tt in dires)
            {
               List<FileInfo> temp = GetCurrentAllFiles(tt.FullName, prefix);
               if (temp != null && temp.Count > 0)
                   returnAllFiles.AddRange(temp);
            }
            return returnAllFiles;
        }
    }
}
