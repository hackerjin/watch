using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Security;
using System.Security.Permissions;

namespace Skyray.EDX.Common
{
    public class BackupHelper
    {
        /// <summary>
        /// BackupHelper启动开关
        /// </summary>
        private static bool enable = false;
        public static bool Enable
        {
            get { return enable; }
            set
            {
                //enable = value;
                if (value)
                    StartProbeProcess();
                else
                    UnRegisterFileChange();
            }
        }
        /// <summary>
        /// 是否启用自动保存
        /// </summary>
        public static bool IsAutoBackUp = false;
        /// <summary>
        /// 是否覆盖
        /// </summary>
        public static bool IsOverided = false;

        /// <summary>
        /// 要监测的文件名列表,为当前目录下文件
        /// </summary>
        public static List<string> ProbeFileList = new List<string>();

        /// <summary>
        /// 要监测目录列表
        /// </summary>
        public static List<string> ProbeDirList = new List<string>();

        /// <summary>
        /// 监测间隔
        /// </summary>
        public static double ProbeTimeInterval;

        private static string folderName;

        /// <summary>
        /// 备份目录
        /// </summary>
        public static string BackupBaseDirectory;

        private static List<string> FileExtension = new List<string>() { ".jpg",".xml",".xls"};


        public static FileSystemWatcher _FileSystemWatcher0;

        public static List<FileSystemWatcher> _FileDirWatcher = new List<FileSystemWatcher>();

        /// <summary>
        /// 监视指定目录下的指定文件
        /// </summary>
        public static void ProbeFileChangeFile()
        {
             _FileSystemWatcher0 = new FileSystemWatcher();
            _FileSystemWatcher0.Path = AppDomain.CurrentDomain.BaseDirectory;
            _FileSystemWatcher0.Filter = "*.*";
            _FileSystemWatcher0.IncludeSubdirectories = false;
            _FileSystemWatcher0.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.LastAccess |//NotifyFilters.CreationTime|
                NotifyFilters.FileName; //| NotifyFilters.DirectoryName;
            _FileSystemWatcher0.Changed += new FileSystemEventHandler(_FileSystemWatcher_Changed);
            _FileSystemWatcher0.Deleted += new FileSystemEventHandler(_FileSystemWatcher_Changed);
            _FileSystemWatcher0.EnableRaisingEvents = true;
        }

        public static void UnRegisterFileChange()
        {
            if (_FileSystemWatcher0 != null)
            {
                _FileSystemWatcher0.Changed -= _FileSystemWatcher_Changed;
                _FileSystemWatcher0.Deleted -= _FileSystemWatcher_Changed;
            }
            if (_FileDirWatcher.Count == 0)
                return;
            foreach (FileSystemWatcher tt in _FileDirWatcher)
            {
                if (tt == null)
                    continue;
                tt.Changed -= _FileSystemWatcherDir_Changed;
                tt.Deleted -= _FileSystemWatcherDir_Changed;
            }
            enable = false;
        }

        /// <summary>
        ///  监视指定目录
        /// </summary>
        public static void ProbeFileChangeDir()
        {
            if (ProbeDirList.Count == 0)
                return;
            ProbeDirList.ForEach(w =>
            {
                FileSystemWatcher _FileSystemWatcher = new FileSystemWatcher();
                _FileSystemWatcher.Path = w;
                _FileSystemWatcher.Filter = "*.*";
                _FileSystemWatcher.IncludeSubdirectories = true;
                _FileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.LastAccess |//NotifyFilters.CreationTime|
                    NotifyFilters.FileName;// | NotifyFilters.DirectoryName;
                _FileSystemWatcher.Changed += new FileSystemEventHandler(_FileSystemWatcherDir_Changed);
                _FileSystemWatcher.Deleted += new FileSystemEventHandler(_FileSystemWatcherDir_Changed);
                _FileSystemWatcher.EnableRaisingEvents = true;
                _FileDirWatcher.Add(_FileSystemWatcher);
            });
        }
        private static XDocument LoadAppParams()
        {
            XDocument doc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "\\AppParams.xml");
            return doc;
        }
        public static void GenerateDefalut()
        {
            Thread.Sleep(1000);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + AppDomain.CurrentDomain.BaseDirectory.TrimEnd("\\/".ToCharArray()).Split("\\/".ToCharArray()).LastOrDefault();
            XDocument doc = LoadAppParams();
            XElement probe = new XElement("ProbeFile",
                new XElement("AutoBackUp", new XText(Boolean.FalseString)),
                new XElement("ProbeFolderName", new XText("BackUp")),
                new XElement("BackupPath", new XText(path))
                ,new XElement("IsOverided", new XText("1"))
                , new XElement("FileList", new XText("AppParams.xml|EDX3000.sdb|Skyray.EDXRF.exe.config|Camera.xml|EDX3000ToolsConfig1_CommonUser.txt|UI.xml|frm3000.txt"))
                ,new XElement("DirList", new XText("Image|printxml|PrintTemplate")), new XElement("ProbeInterval", new XText("0")));
            doc.Element("application").Add(probe);
            doc.Save(AppDomain.CurrentDomain.BaseDirectory+"\\AppParams.xml");
        }
        /// <summary>
        /// 加载自身参数
        /// </summary>
        /// <returns></returns>
        public static bool ReloadProbeSettings()
        {
            if (LoadAppParams()==null||LoadAppParams().Element("application")==null||LoadAppParams().Element("application").Element("ProbeFile") == null)
                GenerateDefalut();
            IsAutoBackUp = ReportTemplateHelper.LoadSpecifiedValue("ProbeFile", "AutoBackUp").ConvertToType<bool>();
            enable = IsAutoBackUp;
            if (!IsAutoBackUp) return false;
            string backupPath = ReportTemplateHelper.LoadSpecifiedValue("ProbeFile", "BackupPath");
            if (!ValidateHelper.IsValidatePath(backupPath))
                return false;
            string FileList = ReportTemplateHelper.LoadSpecifiedValue("ProbeFile", "FileList");
            string DirList = ReportTemplateHelper.LoadSpecifiedValue("ProbeFile", "DirList");
            if (string.IsNullOrEmpty(FileList)&&string.IsNullOrEmpty(DirList))
                return false;
            folderName = ReportTemplateHelper.LoadSpecifiedValue("ProbeFile", "ProbeFolderName");
           
            string timeInterval = ReportTemplateHelper.LoadSpecifiedValue("ProbeFile", "ProbeInterval");
            string isOverided = ReportTemplateHelper.LoadSpecifiedValue("ProbeFile", "IsOverided");

            ProbeDirList = new List<string>(DirList.Split("|".ToCharArray(),StringSplitOptions.RemoveEmptyEntries));
            ProbeFileList = new List<string>(FileList.Split("|".ToCharArray(),StringSplitOptions.RemoveEmptyEntries));
            BackupBaseDirectory = backupPath;
            IsOverided = string.IsNullOrEmpty(isOverided) ? false : isOverided == "0" ? false : true;
            if (IsOverided)
            {
                if (string.IsNullOrEmpty(folderName)||!Directory.Exists(backupPath + "\\" + folderName))
                    return false;
            }
            ProbeTimeInterval = string.IsNullOrEmpty(timeInterval) ? 0 : timeInterval.ConvertToType(ProbeTimeInterval);
            //Skyray.EDX.Common.ReportHelper.ExcelTemplateParams.LoadExcelTemplateParams(AppDomain.CurrentDomain.BaseDirectory + "\\AppParams.xml");
            //Skyray.EDX.Common.ReportHelper.ExcelTemplateParams.GetExcelTemplateParams();
            return true;
        }

        /// <summary>
        /// 启动监视器
        /// </summary>
        public static void StartProbeProcess()
        {
            BackupHelper.UnRegisterFileChange();
            BackupHelper._FileDirWatcher.Clear();
            if (!ReloadProbeSettings()) return;
            ProbeFileChangeFile();
            ProbeFileChangeDir();
            enable = true;
        }

        static void _FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (ProbeFileList.Contains(e.Name))
            {
                Thread.Sleep(100);
                //if (string.Compare(e.Name, "AppParams.xml", true) == 0)
                //    ReloadProbeSettings();//重新加载自身参数
                string backupPath = BackupBaseDirectory + "\\" + DateTime.Now.ToString("yyyyMMddHH");
                if (!IsOverided)
                {
                    if (Directory.Exists(backupPath))
                    {
                        FileInfo tt = IsBackupCurrentTime(e.Name, e.FullPath, backupPath);
                        if (tt != null&&File.Exists(e.FullPath))
                            File.Copy(e.FullPath, tt.FullName, true);
                    }
                    else
                    {
                        Directory.CreateDirectory(backupPath);
                        BackFileDir(backupPath);
                    }
                }
                else if (IsOverided)
                {
                    FileInfo tt = IsBackupCurrentTimeDir(e.Name, e.FullPath);
                    if (tt != null)
                        File.Copy(e.FullPath, tt.FullName, true);
                }
            }
        }


        static void _FileSystemWatcherDir_Changed(object sender, FileSystemEventArgs e)
        {
            if (!IsOverided)
            {
                if (Directory.Exists(e.FullPath) || string.IsNullOrEmpty(Path.GetExtension(e.FullPath))|| !FileExtension.Contains(Path.GetExtension(e.FullPath))) return;
                string backupPath = BackupBaseDirectory + "\\" + DateTime.Now.ToString("yyyyMMddHH");
                if (Directory.Exists(backupPath))
                {
                    FileInfo tt = IsBackupCurrentTime(e.Name, e.FullPath, backupPath);
                    if (tt == null)
                        return;
                    if(File.Exists(e.FullPath))
                        File.Copy(e.FullPath, tt.FullName, true);
                }
                else
                {
                     Directory.CreateDirectory(backupPath);
                     BackFileDir(backupPath);
                }
            }
            else 
            {
               FileInfo tt = IsBackupCurrentTimeDir(e.Name, e.FullPath);
               if (tt != null)
                   File.Copy(e.FullPath, tt.FullName, true);
            }
        }


        private static FileInfo GetSpecifiedDirFiles(string fileName,string backupPath)
        {
            DirectoryInfo info = new DirectoryInfo(backupPath);
            if (info.GetFileSystemInfos() == null || info.GetFileSystemInfos().Length == 0)
                return null;
            foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
            {
                if (fsi is System.IO.FileInfo && fsi.FullName.Contains(fileName))
                    return (FileInfo)fsi;
            }
            return null;
        }

        private static FileInfo IsBackupCurrentTimeDir(string fileName, string filePath)
        {
            string path = BackupBaseDirectory + "\\" + folderName;
            FileInfo fileInfo = GetSpecifiedDirFiles(fileName, path);
            if (fileInfo == null)
            {
                if(File.Exists(filePath))
                    File.Copy(filePath, path + "\\" + filePath,true);
                return null;
            }
            FileInfo currentInfo = new FileInfo(filePath);
            if ((currentInfo.LastWriteTime - fileInfo.LastWriteTime).Hours >= ProbeTimeInterval)
                return fileInfo;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static FileInfo IsBackupCurrentTime(string fileName,string filePath,string backPath)
        {
            List<FileInfo> returnBackupName = FindCurrentDirByName(BackupBaseDirectory, fileName);
            if (returnBackupName == null || returnBackupName.Count == 0)
            {
                File.Copy(filePath, backPath + "\\" + filePath);
                return null;
            }
            var temp = returnBackupName.Max(w => w.LastWriteTime);
            FileInfo lastWriteInfo = returnBackupName.Find(wde => wde.LastWriteTime == temp);
            FileInfo currentInfo = new FileInfo(filePath);
            if ((currentInfo.LastWriteTime - temp).Hours >= ProbeTimeInterval)
                return lastWriteInfo;
            return null;
        }

        /// <summary>
        /// 找指定目录下相同名称的文件列表
        /// </summary>
        /// <param name="dir">指定目录</param>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        public static List<FileInfo> FindCurrentDirByName(string dir, string fileName)
        {
            if (!ValidateHelper.IsValidatePath(dir)) return null;
            if (!Directory.Exists(dir))
            { Directory.CreateDirectory(dir); return null; }
            List<FileInfo> returnInfo = new List<FileInfo>();
            DirectoryInfo info = new DirectoryInfo(dir);
            foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
            {
                //if (fsi is System.IO.FileInfo && fsi.Name.Equals(fileName, StringComparison.OrdinalIgnoreCase))
                if(fsi is System.IO.FileInfo&&fsi.FullName.Contains(fileName))
                    returnInfo.Add((FileInfo)fsi);
                else if (fsi is System.IO.DirectoryInfo)
                {
                    List<FileInfo> newFindInfo = FindCurrentDirByName(fsi.FullName, fileName);
                    if (newFindInfo != null && newFindInfo.Count>0)
                        returnInfo.AddRange(newFindInfo);
                }
            }
            return returnInfo;
        }

        /// <summary>
        /// 备份文件和目录
        /// </summary>
        /// <param name="backDir"></param>
        public static void BackFileDir(string backDir)
        {
            if (ProbeFileList.Count == 0 && ProbeDirList.Count == 0)
                return;
            if (ProbeFileList.Count > 0)
                ProbeFileList.ForEach(w => { File.Copy(AppDomain.CurrentDomain.BaseDirectory + w, backDir + "\\" + w, true); });
            if (ProbeDirList.Count > 0)
                ProbeDirList.ForEach(w => { CopyDirectory(AppDomain.CurrentDomain.BaseDirectory + w, backDir + "\\" + w); });
        }

        /// <summary>
        /// 备份指定目录下的所有文件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public static void CopyDirectory(String source, String destination)
        {
            if (source.Contains(".svn"))
                return;
            Directory.CreateDirectory(destination);
            DirectoryInfo info = new DirectoryInfo(source);
            foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
            {
                String destName = Path.Combine(destination, fsi.Name);
                if (fsi is System.IO.FileInfo)
                        File.Copy(fsi.FullName, destName, true);
                else
                {
                    Directory.CreateDirectory(destName);
                    CopyDirectory(fsi.FullName, destName);
                }
            }
        }

        public static void ApplicationEndCopyData()
        {
            bool autoBackup = ReportTemplateHelper.LoadSpecifiedValue("ProbeFile", "AutoBackUp").ConvertToType<bool>();
            if (!autoBackup)
            {
                string backupPath = ReportTemplateHelper.LoadSpecifiedValue("ProbeFile", "BackupPath");
                if (!ValidateHelper.IsValidatePath(backupPath))
                    return;
                string FileList = ReportTemplateHelper.LoadSpecifiedValue("ProbeFile", "FileList");
                if (string.IsNullOrEmpty(FileList))
                    return;
                string DirList = ReportTemplateHelper.LoadSpecifiedValue("ProbeFile", "DirList");
                if (string.IsNullOrEmpty(DirList))
                    return;
                ProbeDirList = new List<string>(DirList.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                ProbeFileList = new List<string>(FileList.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                string path = backupPath + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                BackFileDir(path);
            }
        }
    }

    public class AutoBackupHelper
    {
        
        /// <summary>
        /// 是否启用自动保存
        /// </summary>
        public static bool IsAutoBackUp = false;

        /// <summary>
        /// 要监测的文件名列表,为当前目录下文件
        /// </summary>
        public static List<string> ProbeFileList = new List<string>();

        /// <summary>
        /// 监测间隔
        /// </summary>
        public static double ProbeTimeInterval;

        /// <summary>
        /// 自动备份路径
        /// </summary>
        public static string AutoBackupPath = string.Empty;

        public static string BackupPath = string.Empty;

        private static System.Windows.Forms.Timer timer;

        /// <summary>
        /// 当前系统版本
        /// </summary>
        public static string sVersion = string.Empty;

        public static List<FileSystemWatcher> lFileSystemWatcher;


        /// <summary>
        /// 启动监视器
        /// </summary>
        public static void StartProbeProcess()
        {
            GenerateDefalut();
            if(!IsAutoBackUp) return;
            Detection();
        }

        /// <summary>
        /// 读取自动备份参数
        /// </summary>
        public static void GenerateDefalut()
        {
            IsAutoBackUp=(ReportTemplateHelper.LoadSpecifiedValue("ProbeFile","AutoBackUp")==string.Empty || ReportTemplateHelper.LoadSpecifiedValue("ProbeFile","AutoBackUp").ToLower() == "false") ? false : true;
            if(!IsAutoBackUp) return;

            ProbeFileList = new List<string>(ReportTemplateHelper.LoadSpecifiedValue("ProbeFile", "FileList").Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            ProbeTimeInterval = double.Parse(ReportTemplateHelper.LoadSpecifiedValue("ProbeFile", "ProbeInterval"));
            AutoBackupPath = ReportTemplateHelper.LoadSpecifiedValue("ProbeFile", "AutoBackupPath");
            BackupPath = ReportTemplateHelper.LoadSpecifiedValue("ProbeFile", "BackupPath");
            
        }

        /// <summary>
        /// 保存自动备份参数
        /// </summary>
        /// <param name="sVersion"></param>
        public static void SetAutoBackupParameter()
        {
            if (lFileSystemWatcher != null && lFileSystemWatcher.Count > 0)
                foreach (FileSystemWatcher filewatcher in lFileSystemWatcher)
                    filewatcher.EnableRaisingEvents = false;

            string sProbeFile = "AppParams.xml";
            string sFileToolsConfig = "ToolsConfig";
            switch (sVersion)
            {
                case "EDX3000":sProbeFile += "|Skyray.EDXRF.exe.config";break;
                case "Rohs": sProbeFile += "|Skyray.RoHS.exe.config"; break;
                case "Thick": sProbeFile += "|Skyray.Thick.exe.config"; break;
                case "XRF": sProbeFile += "|Skyray.XRF.exe.config"; break;

            }
            sFileToolsConfig = sVersion + sFileToolsConfig;
            sProbeFile += "|Camera.xml|UI.xml|frm3000.txt|" + sFileToolsConfig + "_Admin.txt" + "|" + sFileToolsConfig + "*_CommonUser.txt";
            ProbeFileList = new List<string>(sProbeFile.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));


            Thread.Sleep(1000);

            XmlDocument docdele = new XmlDocument();
            docdele.Load(AppDomain.CurrentDomain.BaseDirectory + "\\AppParams.xml");
            XmlNode xmlTemp = docdele.SelectSingleNode("application");
            XmlNode xmlTemp_ProbeFile = docdele.SelectSingleNode("application/ProbeFile");
            if (xmlTemp_ProbeFile != null) xmlTemp.RemoveChild(xmlTemp_ProbeFile);
            docdele.Save(AppDomain.CurrentDomain.BaseDirectory + "\\AppParams.xml");

            XDocument doc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "\\AppParams.xml");
            XElement probe = new XElement("ProbeFile",
                new XElement("AutoBackUp", new XText(IsAutoBackUp.ToString())),
                new XElement("AutoBackupPath", new XText(AutoBackupPath))
                , new XElement("BackupPath", new XText(BackupPath))
                , new XElement("FileList", new XText(sProbeFile))
                , new XElement("ProbeInterval", new XText(ProbeTimeInterval.ToString())));
            doc.Element("application").Add(probe);
            doc.Save(AppDomain.CurrentDomain.BaseDirectory + "\\AppParams.xml");

        }

        #region 监视文件
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void Detection()
        {
            if (!IsAutoBackUp || AutoBackupPath == string.Empty || ProbeFileList.Count == 0) return;

            lFileSystemWatcher = new List<FileSystemWatcher>();
            ProbeFileList.ForEach(w =>
            {
                FileSystemWatcher watcher = new FileSystemWatcher();
                watcher.Path = AppDomain.CurrentDomain.BaseDirectory + "\\";
                watcher.Filter = w;
                watcher.EnableRaisingEvents = true;//开启提交事件
                watcher.IncludeSubdirectories = true;//允许侦测此目录下的子目录
                watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.LastAccess |//NotifyFilters.CreationTime|
                        NotifyFilters.FileName;// | NotifyFilters.DirectoryName;;
                watcher.Changed += BackupFile;
                lFileSystemWatcher.Add(watcher);
            });

            if (ProbeTimeInterval > 0)
            {
                timer = new System.Windows.Forms.Timer();
                timer.Enabled = true;
                timer.Interval = int.Parse(Convert.ToString(1000 * ProbeTimeInterval * 3600));
                timer.Tick += new EventHandler(timer_Tick);//定时器事件
            }
        }

        private static void timer_Tick(object source, EventArgs e)
        {

            Thread BackupThread = new Thread(new ThreadStart(AutoBackup));
            BackupThread.IsBackground = true;
            BackupThread.SetApartmentState(ApartmentState.STA);
            BackupThread.Start();

            
        }

        private static void AutoBackup()
        {
            try
            {
                string sFileName = string.Empty;
                switch (sVersion)
                {
                    case "EDX3000": sFileName = "EDX3000.sdb"; break;
                    case "Rohs": sFileName = "EDXR.sdb"; break;
                    case "Thick": sFileName = "EDXT.sdb"; break;
                    case "XRF": sFileName = "EDXX.sdb"; break;
                }

                File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\" + sFileName, AutoBackupPath + "\\" + sFileName, true);
                CopyDirectory(AppDomain.CurrentDomain.BaseDirectory + "printxml", AutoBackupPath + "\\printxml");
                CopyDirectory(AppDomain.CurrentDomain.BaseDirectory + "Image", AutoBackupPath + "\\Image");
                if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Spectrum"))
                {
                    CopyDirectory(AppDomain.CurrentDomain.BaseDirectory + "Spectrum", AutoBackupPath + "\\Spectrum");
                }
            }
            catch { }
        }

        public static  void CopyDirectory(String source, String destination)
        {
            if (source.Contains(".svn"))
                return;
            Directory.CreateDirectory(destination);
            DirectoryInfo info = new DirectoryInfo(source);
            if (!Directory.Exists(source))
                return;
            foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
            {
                String destName = Path.Combine(destination, fsi.Name);
                //如果是文件类,就复制文件
                if (fsi is System.IO.FileInfo)
                    File.Copy(fsi.FullName, destName, true);
                //如果不是 则为文件夹,继续调用文件夹复制函数,递归
                else
                {
                    Directory.CreateDirectory(destName);
                    CopyDirectory(fsi.FullName, destName);
                }
            }
        }

        private static void BackupFile(object source, FileSystemEventArgs e)
        {
            try
            {
                string sFileName = e.FullPath.Split('\\').Last();
                if (AutoBackupPath == string.Empty) return;
                Thread.Sleep(1000);
                File.Copy(e.FullPath, AutoBackupPath + "\\" + sFileName, true);
            }
            catch { }
        }

        #endregion




    }
}
