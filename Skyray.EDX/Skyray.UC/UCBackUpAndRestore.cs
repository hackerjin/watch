using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Threading;
using Skyray.EDX.Common.ReportHelper;

namespace Skyray.UC
{
    public partial class UCBackUpAndRestore : Skyray.Language.UCMultiple
    {
        string strFileAppParams = "AppParams.xml";
        string strFileParameter = "Parameter.xml";
        string strFileCameral = "Camera.xml";
        string strFileDataBase = string.Empty;
        string strFileConfig = string.Empty;
        string strFileToolsConfig = "ToolsConfig";
        string strFileCommonFile = string.Empty;
        string strFileAdminFile = string.Empty;
        bool isShow = false;
        bool needsBackUp = false;
        bool needsRestore = false;
        bool needsSet = false;
        string strSN = "sn.skyray";
        public UCBackUpAndRestore()
        {
            InitializeComponent();
            if (DifferenceDevice.IsAnalyser)
            {
                strFileDataBase = "EDX3000.sdb";
                strFileConfig = "Skyray.EDXRF.exe.config";
                strFileToolsConfig = "EDX3000" + strFileToolsConfig;
            }
            else if (DifferenceDevice.IsRohs)
            {
                strFileDataBase = "EDXR.sdb";
                strFileConfig = "Skyray.RoHS.exe.config";
                strFileToolsConfig = "Rohs" + strFileToolsConfig;
            }
            else if (DifferenceDevice.IsThick)
            {
                strFileDataBase = "EDXT.sdb";
                strFileConfig = "Skyray.Thick.exe.config";
                strFileToolsConfig = "Thick" + strFileToolsConfig;
            }
            else if (DifferenceDevice.IsXRF)
            {
                strFileDataBase = "EDXX.sdb";
                strFileConfig = "Skyray.XRF.exe.config";
                strFileToolsConfig = "XRF" + strFileToolsConfig;
            }
            strFileAdminFile = strFileToolsConfig + "*_Admin*.txt";
            strFileCommonFile = strFileToolsConfig + "*_CommonUser*.txt";
            MatchCurrentPathFileName(ref strFileCommonFile);
            MatchCurrentPathFileName(ref strFileAdminFile);
            this.folderBrowserDialogEx1.ShowNewFolderButton = true;
            this.txtBackUpPath.Text = ReportTemplateHelper.LoadSpecifiedValue("ProbeFile", "BackupPath");
            //this.txtBackupFolderName.Text = ReportTemplateHelper.LoadSpecifiedValue("ProbeFile", "ProbeFolderName");
            this.chkAutoBackUp.Checked = ReportTemplateHelper.LoadSpecifiedValue("ProbeFile", "AutoBackUp").ConvertToType(chkAutoBackUp.Checked);
            //this.cmbBackUpType.Items[0] = Info.sAdd;
            //this.cmbBackUpType.Items[1] = Info.sCover;
            //this.cmbBackUpType.SelectedIndex = ReportTemplateHelper.LoadSpecifiedValue("ProbeFile", "IsOverided").ConvertToType(cmbBackUpType.SelectedIndex);
            this.numericUpDown1.Value = ReportTemplateHelper.LoadSpecifiedValue("ProbeFile", "ProbeInterval").ConvertToType(numericUpDown1.Value);
            //this.txtBackupFolderName.Text =
            //    ReportTemplateHelper.LoadSpecifiedValue("ProbeFile", "ProbeFolderName").ConvertToType(txtBackupFolderName.Text);
            this.AutoSize = true;
            //gpDetails.Enabled = chkAutoBackUp.Checked;
            bool validate = ValidateHelper.IsValidatePath(txtBackUpPath.Text);
            //SwitchAutoBackUpControlsState(validate);
            if (!validate) return;
            if (!Directory.Exists(txtBackUpPath.Text))
                Directory.CreateDirectory(txtBackUpPath.Text);
            this.folderBrowserDialogEx1.SelectedPath = this.txtBackUpPath.Text;
            
        }

        //private void SwitchAutoBackUpControlsState(bool validate)
        //{
        //    chkAutoBackUp.Enabled  = cmbBackUpType.Enabled = numericUpDown1.Enabled = validate;
        //    if (!validate) chkAutoBackUp.Checked = false;
        //}

        private void btnBackUp_Click(object sender, EventArgs e)
        {
            //if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            if(folderBrowserDialogEx1.ShowDialog()==DialogResult.OK)
            {
                needsBackUp = true;
                this.txtBackUpPath.Text = folderBrowserDialogEx1.SelectedPath;
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            //if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            if(folderBrowserDialogEx1.ShowDialog()==DialogResult.OK)
            {
                needsRestore = true;
                this.txtRestorePath.Text = folderBrowserDialogEx1.SelectedPath;
            }
        }

        public void CopyDirectory(String source, String destination)
        {
            if (source.Contains(".svn"))
                return;
            Directory.CreateDirectory(destination);
            DirectoryInfo info = new DirectoryInfo(source);
            if (!Directory.Exists(source))
                return;
            foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
            {
                //if (fsi.Attributes  
                //    FileAttributes.Hidden
                //目标路径destName = 目标文件夹路径 + 原文件夹下的子文件(或文件夹)名字
                //Path.Combine(string a ,string b) 为合并两个字符串
                String destName = Path.Combine(destination, fsi.Name);
                //如果是文件类,就复制文件
                if (fsi is System.IO.FileInfo)
                    ////如果是xml文件并且目标文件存在 更新至目标
                    //if (fsi.Extension.ToLower().Contains("xml")&&File.Exists(destination+"\\"+fsi.Name))
                    //    RestoreXml(source + "\\" + fsi.Name, destination + "\\" + fsi.Name);
                    //else
                      //2013-10-25 修改备份功能
                        File.Copy(fsi.FullName, destName, true);
                //如果不是 则为文件夹,继续调用文件夹复制函数,递归
                else
                {
                    Directory.CreateDirectory(destName);
                    CopyDirectory(fsi.FullName, destName);
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
           
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                //btnSetting_Click(null, null);
                if (needsSet==false&& needsRestore==false && needsBackUp == false)
                    needsBackUp = true;
                if (needsBackUp&&!string.IsNullOrEmpty(this.txtBackUpPath.Text))
                {
                  
                    string backupPath = txtBackUpPath.Text + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    DirectoryInfo dir = new DirectoryInfo(backupPath);
                    dir.Create();

                    //备份文件
                    FilesExistsCopy(strFileAppParams, backupPath);
                    if (DifferenceDevice.IsRohs)
                        FilesExistsCopy(strFileParameter, backupPath);
                    FilesExistsCopy(strFileDataBase, backupPath);
                    FilesExistsCopy(strFileConfig, backupPath);
                    FilesExistsCopy(strFileCameral, backupPath);
                    FilesExistsCopy(strFileAdminFile, backupPath);
                    FilesExistsCopy(strFileCommonFile, backupPath);
                    FilesExistsCopy("UI.xml", backupPath);
                    FilesExistsCopy("frm3000.txt", backupPath);
                    //追加硬件说明文档
                    FilesExistsCopy("HardGuide.chm", backupPath);
                    //追加历史记录排列顺序
                    FilesExistsCopy("CustomHistory.xml", backupPath);
                    //追加Dpp100
                    FilesExistsCopy("Dpp100.cfg", backupPath);
                    //追加硬件加密sn
                    FilesExistsCopy(strSN, backupPath);


                    //备份目录
                    BackUpTemplates(AppDomain.CurrentDomain.BaseDirectory , backupPath);
                    CopyDirectory(AppDomain.CurrentDomain.BaseDirectory + "printxml", backupPath + "\\printxml");
                    CopyDirectory(AppDomain.CurrentDomain.BaseDirectory + "Image", backupPath + "\\Image");
                    if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Spectrum"))
                    {
                        CopyDirectory(AppDomain.CurrentDomain.BaseDirectory + "Spectrum", backupPath + "\\Spectrum");
                    }
                    if (!this.chkAutoBackUp.Checked)
                        AddDetectObj();

                    #region 自动备份
                    AutoBackupHelper.AutoBackupPath = backupPath;
                    AutoBackupHelper.BackupPath = txtBackUpPath.Text;
                    AutoBackupHelper.IsAutoBackUp = chkAutoBackUp.Checked;
                    AutoBackupHelper.ProbeTimeInterval = double.Parse(numericUpDown1.Value.ToString());
                  
                    string sVersion=string.Empty;
                    if(DifferenceDevice.IsAnalyser) sVersion="EDX3000";
                    else if(DifferenceDevice.IsRohs) sVersion="Rohs";
                    else if(DifferenceDevice.IsThick) sVersion="Thick";
                    else if(DifferenceDevice.IsXRF) sVersion="XRF";
                    AutoBackupHelper.sVersion = sVersion;
                    AutoBackupHelper.SetAutoBackupParameter();
                    AutoBackupHelper.StartProbeProcess();
                    #endregion

                    if (isShow&&Skyray.Controls.SkyrayMsgBox.Show(Info.BackUpSuccessed, Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                    {
                        Process.Start("explorer.exe", backupPath);
                    }
                }
                if (needsRestore&&!string.IsNullOrEmpty(this.txtRestorePath.Text))
                {
                    if (Skyray.Controls.SkyrayMsgBox.Show(Info.NeedReStartApplication, Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        RestoreXml(txtRestorePath.Text + "\\" + strFileAppParams, AppDomain.CurrentDomain.BaseDirectory + strFileAppParams);
                        if (DifferenceDevice.IsRohs)
                            RestoreXml(txtRestorePath.Text + "\\" + strFileParameter, AppDomain.CurrentDomain.BaseDirectory + strFileParameter);
                        FilesResoreCopy(txtRestorePath.Text, strFileDataBase);
                        //FilesResoreCopy(txtRestorePath.Text, strFileConfig);
                        RestoreXml(txtRestorePath.Text + "\\" + strFileConfig, AppDomain.CurrentDomain.BaseDirectory + strFileConfig);

                        FilesResoreCopy(txtRestorePath.Text, strFileAdminFile);
                        FilesResoreCopy(txtRestorePath.Text, strFileCommonFile);
                        FilesResoreCopy(txtRestorePath.Text, "UI.xml");
                        FilesResoreCopy(txtRestorePath.Text, "frm3000.txt");
                        FilesResoreCopy(txtRestorePath.Text, strFileCameral);
                        //追加硬件说明文档
                        FilesResoreCopy(txtRestorePath.Text, "HardGuide.chm");
                        //追加历史记录排列顺序
                        FilesResoreCopy(txtRestorePath.Text, "CustomHistory.xml");
                        //追加Dpp100
                        FilesResoreCopy(txtRestorePath.Text, "Dpp100.cfg");
                        //硬件加密sn
                        FilesResoreCopy(txtRestorePath.Text, strSN);

                        RestoreTempaltes(txtRestorePath.Text, AppDomain.CurrentDomain.BaseDirectory);
                        CopyDirectory(txtRestorePath.Text + "\\printxml", AppDomain.CurrentDomain.BaseDirectory + "\\printxml");
                        CopyDirectory(txtRestorePath.Text + "\\Image", AppDomain.CurrentDomain.BaseDirectory + "\\Image");
                        CopyDirectory(txtRestorePath.Text + "\\Spectrum", AppDomain.CurrentDomain.BaseDirectory + "\\Spectrum");

                        if (isShow&&Skyray.Controls.SkyrayMsgBox.Show(Info.RestoreSuccessed, Info.Suggestion, MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            DifferenceDevice.interClassMain.skyrayCamera.Close();
                            //摄像头关闭后重新保存了，这里再复制camer.xml
                            FilesResoreCopy(txtRestorePath.Text, strFileCameral);
                            Process p = new Process();
                            p.StartInfo.FileName = Application.ExecutablePath;
                            p.Start();
                            Environment.Exit(0);
                        }
                    }
                }
                this.Cursor = System.Windows.Forms.Cursors.Arrow;
            }
            catch (Exception ex)
            {
                if(isShow)
                Skyray.Controls.SkyrayMsgBox.Show(Info.BackUpAndRestoreFailed + "\n" + ex.Message, Info.Suggestion, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            btnCancel_Click(null,null);
        }

        private void CreateDirAndCopyFiles(string backupPath,string foldName)
        {
            string newPath = backupPath + "\\" + foldName;
            DirectoryInfo info = new DirectoryInfo(newPath);
            if (!info.Exists)
                info.Create();
            ReportTemplateHelper.SaveSpecifiedValue("ProbeFile", "ProbeFolderName", foldName);
            FilesExistsCopy(strFileAppParams, newPath);
            if (DifferenceDevice.IsRohs)
                FilesExistsCopy(strFileParameter, newPath);
            FilesExistsCopy(strFileDataBase, newPath);
            FilesExistsCopy(strFileConfig, newPath);
            FilesExistsCopy(strFileCameral, newPath);
            FilesExistsCopy(strFileAdminFile, newPath);
            FilesExistsCopy(strFileCommonFile, newPath);
            FilesExistsCopy("UI.xml", newPath);
            FilesExistsCopy("frm3000.txt", newPath);

            //备份目录
            BackUpTemplates(AppDomain.CurrentDomain.BaseDirectory, newPath);
            CopyDirectory(AppDomain.CurrentDomain.BaseDirectory + "printxml", newPath + "\\printxml");
            CopyDirectory(AppDomain.CurrentDomain.BaseDirectory + "Image", newPath + "\\Image");
            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Spectrum"))
                CopyDirectory(AppDomain.CurrentDomain.BaseDirectory + "Spectrum", newPath + "\\Spectrum");
        }

        //private void SaveProbeInterval()
        //{
        //    if (isShow)
        //    ReportTemplateHelper.SaveSpecifiedValue("ProbeFile", "ProbeInterval", this.numericUpDown1.Value.ConvertToType<string>());
        //}

        //private void SaveBackUpType()
        //{
        //    if (isShow)
        //    ReportTemplateHelper.SaveSpecifiedValue("ProbeFile", "IsOverided", this.cmbBackUpType.SelectedIndex.ConvertToType<string>());
        //}

        //private void SaveBackUpPath()
        //{
        //    if (isShow)
        //    ReportTemplateHelper.SaveSpecifiedValue("ProbeFile", "BackupPath", this.txtBackUpPath.Text);
        //}

        //private void SaveAutoBackUpState()
        //{
        //    if (isShow)
        //    ReportTemplateHelper.SaveSpecifiedValue("ProbeFile", "AutoBackUp", this.chkAutoBackUp.Checked.ConvertToType<string>());
        //}

        private void MatchCurrentPathFileName(ref string fileName)
        {
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + fileName))
            {
                fileName = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, fileName).FirstOrDefault().ConvertToType<string>();
                if(fileName!=null)
                    fileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
            }
        }

        private void FilesExistsCopy(string fileName,string backupPath,ref List<string> fileList)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + fileName))
            {
                fileList.Add(fileName);
                File.Copy(AppDomain.CurrentDomain.BaseDirectory + fileName, backupPath + "\\" + fileName,true);
            }
        }

        private void FilesExistsCopy(string fileName, string backupPath)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + fileName))
            {
                File.Copy(AppDomain.CurrentDomain.BaseDirectory + fileName, backupPath + "\\" + fileName, true);
            }
        }

        private void FilesResoreCopy(string restorePath,string fileName)
        {
            if (File.Exists(restorePath +"\\"+ fileName))
            {
                File.Copy(restorePath +"\\"+ fileName, AppDomain.CurrentDomain.BaseDirectory + fileName, true);
            }
        }
       
        /// <summary>
        /// 备份打印模板
        /// </summary>
        /// <param name="destination"></param>
        private void BackUpTemplates(string source,string destination)
        {
            if (destination == null | !Directory.Exists(destination)) return;
            if (ReportTemplateHelper.ExcelModeType == 2)
            { CopyDirectory(source + "\\PrintTemplate", destination + "\\PrintTemplate"); return; }
            ExcelTemplateParams.LoadExcelTemplateParams(source + strFileAppParams);
            ExcelTemplateParams.GetExcelTemplateParams();
            string[] excelTemplates = { ExcelTemplateParams.OneTimeTemplate, ExcelTemplateParams.ManyTimeTemplate,ExcelTemplateParams.ManyTimeTemplate.Insert(ExcelTemplateParams.ManyTimeTemplate.LastIndexOf('.'),"_Horizontal") };
            var excelSourcePath = source + "\\HistoryExcelTemplate\\";
            var excelDestinationPath = destination + "\\HistoryExcelTemplate\\";
            if (!Directory.Exists(excelDestinationPath)) Directory.CreateDirectory(excelDestinationPath);
            foreach (var s in excelTemplates)
            {
                if (File.Exists(excelSourcePath+s))
                    System.IO.File.Copy(excelSourcePath + s,excelDestinationPath + s,true);
            }
        }
        /// <summary>
        /// 还原模板
        /// </summary>
        private void RestoreTempaltes(string backUpPath,string restorePath)
        {
            if (ReportTemplateHelper.ExcelModeType == 2)
            {
                CopyDirectory(backUpPath + "\\PrintTemplate", restorePath + "\\PrintTemplate");
                return;
            }
            var files = Directory.GetFiles(backUpPath+"\\HistoryExcelTemplate");
            string restoreTemplate = AppDomain.CurrentDomain.BaseDirectory+"\\HistoryExcelTemplate";
            foreach (var file in files)
            {
                string fileRestore  = restoreTemplate+file.Substring(file.LastIndexOf('\\'));
                //if(!File.Exists(fileRestore))
                 System.IO.File.Copy(file, fileRestore, true);
            }
        }

        public void StartProcess(object obj)
        {
            System.Diagnostics.Process.Start(obj.ToString());
        }

        #region xml文件导入进行文本信息比较，更正

        private XmlDocument xmlDoc_old = new XmlDocument();
        private XmlDocument xmlDoc_new = new XmlDocument();
        private void RestoreXml(string OldXmlPath, string NewXmlPath)
        {
            UpdateXml.Clear();
            NodeAppearNum.Clear();
            xmlDoc_new.Load(NewXmlPath);//读取目标（新）的xml
            xmlDoc_old.Load(OldXmlPath);//读取备份xml
            FindNode(xmlDoc_new.ChildNodes);//获取旧xml在新的xml中不相同的xml
            if (UpdateXml.Count > 0 || UpdateXmlAttribute.Count>0) UpdateNewXml(NewXmlPath);//将不相同值的值更新新的xml
        }

        private void UpdateNewXml(string path)
        {
            if (UpdateXml.Count > 0)
            {
                foreach (string strpath in UpdateXml.Keys)
                {
                    Dictionary<int, string> lstr = UpdateXml[strpath];

                    XmlNodeList xmlnodelist = xmlDoc_new.SelectNodes(strpath);

                    //int i = 0;
                    //foreach (XmlNode rootNode in xmlnodelist)
                    //{
                    //    i++;
                    //    if (rootNode != null && rootNode.InnerText != lstr[i])
                    //    {
                    //        rootNode.InnerText = lstr[i];

                    //    }

                    //}

                    foreach(KeyValuePair<int ,string> pair in lstr)
                    {
                        if (xmlnodelist.Item(pair.Key-1) != null && xmlnodelist.Item(pair.Key-1).InnerText != pair.Value)
                        {
                            xmlnodelist.Item(pair.Key-1).InnerText = lstr[pair.Key];
                        }
                    }
                }
            }


            if (UpdateXmlAttribute.Count > 0)
            {
                foreach (string strpath in UpdateXmlAttribute.Keys)
                {
                    Dictionary<int, Dictionary<string, string>> lstr = UpdateXmlAttribute[strpath];

                    XmlNodeList xmlnodelist = xmlDoc_new.SelectNodes(strpath);
                    int i = 0;
                    foreach (XmlNode rootNode in xmlnodelist)
                    {
                        i++;
                        Dictionary<string, string> dlist=new Dictionary<string,string>();
                        lstr.TryGetValue(i,out dlist);
                        if (rootNode != null && dlist!=null)
                        {
                            foreach (XmlAttribute xa in rootNode.Attributes)
                            {
                                foreach (string snodename in dlist.Keys)
                                {
                                    if (xa.Name == snodename && xa.Value != dlist[snodename])
                                        xa.Value = dlist[snodename];

                                }
                            }

                        }

                    }
                }
            }
            xmlDoc_new.Save(path);
        }

        private Dictionary<string, int> NodeAppearNum = new Dictionary<string,int>();//节点出现次数

        /// <summary>
        /// 第一个string表示xml路径
        /// int表示出现节点次数
        /// string表示old的值
        /// </summary>
        private Dictionary<string, Dictionary<int,string>> UpdateXml = new Dictionary<string,Dictionary<int,string>>();//值存在不同

        /// <summary>
        /// Dictionary<string, string>存放参数值不同的内容
        /// </summary>
        private Dictionary<string, Dictionary<int, Dictionary<string, string>>> UpdateXmlAttribute = new Dictionary<string, Dictionary<int, Dictionary<string, string>>>();//值存在不同
        
        /// <summary>
        /// 根据新的xml内容，获取所有的末支节点，能后根据末支节点内容的参数内容，值内容和旧的xml进行路径查找，同时比较参数内容和值内容
        /// UpdateXml基本存放值内容不同
        /// UpdateXmlAttribute存放参数内容不同的集合
        /// 值内容和参数内容不采用同一个集合存放的原因：每一个末支节点只有一个值，而每一个末支节点具有多个参数，所以分开存放。
        /// </summary>
        /// <param name="list"></param>
        private void FindNode(XmlNodeList list)
        {
            foreach (XmlNode n in list)
            {
                //2013-10-25修改备份
                //if(n.HasChildNodes)
                //    FindNode(n.ChildNodes);
                //else
                string AtrriStrings = (n.InnerXml == null || n.InnerXml.Length <= 0) ? n.OuterXml : n.OuterXml.Replace(n.InnerXml, "");
                if (((n.Value!=null&&n.Value!=string.Empty)||(n.Attributes!=null&&n.Attributes.Count>0))
                    && AtrriStrings.IndexOf("-->") == -1
                    && n.ParentNode != null
                    && AtrriStrings.IndexOf("encoding") == -1)
                {
                    //if (n.OuterXml.IndexOf("-->") != -1 || n.ParentNode == null || n.OuterXml.IndexOf("encoding") != -1 || n.ParentNode.ParentNode == null /*|| n.Value==null*/) continue;

                    string Path="";
                    GetParentNode(n, ref Path);
                    //string values = n.Value==null?"":n.Value.ToString();
                    if (n.Value == null)//值为空时只有父节点的路径n本身就是子节点名称而不是子节点的内容
                        Path += n.Name;
                    string values = n.InnerText;
                    //
                    string oldvalues = "";                    
                    //if (Path.Length > 0) Path = Path.Substring(0, Path.Length-1);
                    Path = Path.TrimEnd('/');

                    //获取当前节点完整路径出现次数
                    int iAppearNum = 0;
                    NodeAppearNum.TryGetValue(Path, out iAppearNum);
                    //if (iAppearNum == null) 
                    //{ 
                    //    iAppearNum = 1; 
                    //    NodeAppearNum.Add(Path, iAppearNum); 
                    //} 
                    //else 
                    //{ 
                        iAppearNum = iAppearNum + 1; 
                        NodeAppearNum[Path] = iAppearNum;
                    //}
                    Dictionary<string, string> olddAttribute = new Dictionary<string, string>();
                   // if (GetOldNode(Path, values, ref oldvalues, n.ParentNode.Attributes,ref olddAttribute))
                    if (GetOldNode(Path, values, ref oldvalues, n.Attributes, ref olddAttribute))
                    {
                        //if (oldvalues != "")
                        if (n.InnerXml.IndexOf("-->")==-1&&oldvalues != "")
                        {
                            Dictionary<int, string> dilst = new Dictionary<int, string>();
                            UpdateXml.TryGetValue(Path, out dilst);
                            if (dilst == null)
                            {
                                dilst = new Dictionary<int, string>();
                                dilst.Add(iAppearNum, oldvalues);
                                UpdateXml.Add(Path, dilst);
                            }
                            else
                            {
                                dilst.Add(iAppearNum, oldvalues);
                                UpdateXml[Path] = dilst;
                            }
                        }
                        if (olddAttribute.Count > 0)
                        {
                            Dictionary<int, Dictionary<string, string>> dilst = new Dictionary<int,Dictionary<string,string>>();
                            UpdateXmlAttribute.TryGetValue(Path, out dilst);
                            if (dilst == null)
                            {
                                dilst = new Dictionary<int,Dictionary<string,string>>();
                                dilst.Add(iAppearNum, olddAttribute);
                                UpdateXmlAttribute.Add(Path, dilst);
                            }
                            else
                            {
                                dilst.Add(iAppearNum, olddAttribute);
                                UpdateXmlAttribute[Path] = dilst;
                            }

                        }

                        
                    }
                }
                //2013-10-25修改备份
                if (n.HasChildNodes)
                    FindNode(n.ChildNodes);
            }
        }

        private void GetParentNode(XmlNode n,ref string info)
        {
            if (n.ParentNode != null && n.ParentNode.Name.IndexOf("#")==-1)
            {
                info = n.ParentNode.Name + "/" + info;
                GetParentNode(n.ParentNode, ref info);
            }
        }

        private bool GetOldNode(string path, 
            string values, ref string oldvalues,
            XmlAttributeCollection Attributes, ref Dictionary<string, string> olddAttribute)
        {
            bool ishomology = false;
            if (string.IsNullOrEmpty(path))
                return false;
            XmlNodeList xmlnodelist=xmlDoc_old.SelectNodes(path);
            if (xmlnodelist == null || xmlnodelist.Count == 0) 
                return false;

            int iAppearNum = 0;
            NodeAppearNum.TryGetValue(path, out iAppearNum);

            XmlNode rootNode = xmlnodelist[iAppearNum-1];
            if (rootNode != null && rootNode.InnerText != values)
            {
                oldvalues = rootNode.InnerText;
                ishomology = true;
            }

            //比较属性
            if (rootNode != null && Attributes != null && rootNode.Attributes != null)
            {
                foreach (XmlAttribute xa in Attributes)
                {
                    foreach(XmlAttribute xaold in rootNode.Attributes)
                        if (xa.Name == xaold.Name && xa.Value != xaold.Value)
                        {
                            olddAttribute.Add(xa.Name, xaold.Value);
                            ishomology = true;
                        }
                }
            }

            return ishomology;
        }

        #endregion


        private void btnCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }

        private void txtRestorePath_TextChanged(object sender, EventArgs e)
        {

        }

        private void UCBackUpAndRestore_Load(object sender, EventArgs e)
        {
            isShow = true;
            //btnDetail_Click(null, null);
            //this.cmbBackUpType.Items.Clear();
            //this.cmbBackUpType.Items.Add(Info.sAdd);
            //this.cmbBackUpType.Items.Add(Info.sCover);
        }

        //private void txtBackUpPath_TextChanged(object sender, EventArgs e)
        //{
        //    SwitchAutoBackUpControlsState(ValidateHelper.IsValidatePath(txtBackUpPath.Text));
        //    SaveBackUpPath();
        //}

        //private void chkAutoBackUp_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (!isShow) return;
        //    needsSet = true;
        //    SaveAutoBackUpState();
        //    btnSetting_Click(btnSetting, null);
        //    gpDetails.Enabled = chkAutoBackUp.Checked;
        //}

        //private void cmbBackUpType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (isShow == true)
        //    {
        //        SaveBackUpType();
        //        needsSet = true;
        //    }
        //    if (cmbBackUpType.SelectedIndex == 1)
        //    {
        //        this.lblBackupFolder.Visible = true;
        //        this.txtBackupFolderName.Visible = true;
        //    }
        //    else
        //    {
        //        this.lblBackupFolder.Visible = false;
        //        this.txtBackupFolderName.Visible = false;
        //    }
        //}

        //private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        //{
        //    if (!isShow) return;
        //    SaveProbeInterval();
        //    needsSet = true;
        //}

        private void txtBackupFolderName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (ValidateHelper.IsValidatePath(e.KeyChar.ToString()))
                return;
        }

        public void AddDetectObj()
        {
            List<string> fileNameList = new List<string>();
            List<string> dirNameList = new List<string>();
            fileNameList.Add(strFileAppParams);
            if (DifferenceDevice.IsRohs)
                fileNameList.Add(strFileParameter);
            fileNameList.Add(strFileDataBase);
            fileNameList.Add(strFileConfig);
            fileNameList.Add(strFileCameral);
            fileNameList.Add(strFileAdminFile);
            fileNameList.Add(strFileCommonFile);
            fileNameList.Add("UI.xml");
            fileNameList.Add("frm3000.txt");
            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Spectrum"))
                dirNameList.Add("Spectrum");
            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Image"))
                dirNameList.Add("Image");
            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "printxml"))
                dirNameList.Add("printxml");
            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "PrintTemplate"))
                dirNameList.Add("PrintTemplate");
            ReportTemplateHelper.SaveSpecifiedValue("ProbeFile", "FileList", string.Join("|", fileNameList.ToArray()));
            ReportTemplateHelper.SaveSpecifiedValue("ProbeFile", "DirList", string.Join("|", dirNameList.ToArray()));
        }

        //private void btnSetting_Click(object sender, EventArgs e)
        //{
        //    if (!needsSet) return;
        //    if (chkAutoBackUp.Checked)
        //    {
        //        if (string.IsNullOrEmpty(this.txtBackUpPath.Text) || (string.IsNullOrEmpty(this.txtBackupFolderName.Text) && this.txtBackupFolderName.Visible))
        //        {
        //            Msg.Show(Info.FolderName);
        //            return;
        //        }
            
        //        AddDetectObj();
        //        SaveBackUpPath();
        //        SaveBackUpType();
        //        SaveProbeInterval();
        //        if (this.txtBackupFolderName.Visible)
        //            CreateDirAndCopyFiles(this.txtBackUpPath.Text, this.txtBackupFolderName.Text);
        //    }
        //    BackupHelper.Enable = chkAutoBackUp.Checked;
        //    SaveAutoBackUpState();
        //    if(!object.Equals(sender,null))
        //    Msg.Show(Info.SetSuccess);
        //}

        private void btnCompressDatabase_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            DBHelper.CompressDB();
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            Msg.Show(Info.sCompressDatabaseInfo);
        }

        private void txtBackupFolderName_TextChanged(object sender, EventArgs e)
        {
            if (!isShow) return;
            needsSet = true;
        }

        //private void btnDetail_Click(object sender, EventArgs e)
        //{
        //    gpDetails.Visible = !gpDetails.Visible;
        //    int change = (gpDetails.Visible ? 1 : -1) * gpDetails.Height;
        //    string title = !gpDetails.Visible ? Encoding.UTF8.GetString(Encoding.UTF8.GetBytes("︾")) : Encoding.UTF8.GetString(Encoding.UTF8.GetBytes("︽"));
        //    this.btnDetail.Text = title;
        //    if(gpDetails.Parent!=null)
        //    gpDetails.Parent.Height += change;
        //    this.Height += change;
        //    if(Parent!=null)
        //    this.Parent.Height += change;
        //    btnOK.Top += change;
        //    btnCancel.Top += change;
        //    btnCompressDatabase.Top += change;
        //}

    }
}
