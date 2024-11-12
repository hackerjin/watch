using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Skyray.Language;
using Skyray.EDX.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Diagnostics;
using System.Threading;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common.Component;
using Lephone.Data.Common;

namespace Skyray.UC
{
    public partial class UCUserConfigParams : UCMultiple
    {
        private string _strLoadXmlPath;

        public string StrLoadXmlPath
        {
            get { return _strLoadXmlPath; }
            set { _strLoadXmlPath = value; }
        }
        private List<FileAppSerialize> fileList = new List<FileAppSerialize>();
        private List<TreeNode> lst = new List<TreeNode>();

        public UCUserConfigParams()
        {
            InitializeComponent();
            btnNormalOptions_Click(null, null);
            StartupVisiable();
            btnStop.Enabled = false;
        }

        private void StartupVisiable()
        {
            if (DifferenceDevice.IsRohs)
            {
                this.btnRoHS.Visible = true;
                this.btnRoHS.Text = Info.ExtendParams;
            }
            if (DifferenceDevice.IsAnalyser)
            {
                this.btn3000.Visible = true;
                this.btn3000.Text = Info.ExtendParams;
            }
            if (DifferenceDevice.IsXRF || DifferenceDevice.IsAnalyser)
            {
                this.btnXRF.Visible = true;
                this.btnXRF.Text = Info.Match;
            }
        }

        private void StartParserXml(TreeNode treeNode,string nodePath)
        {
            if (string.IsNullOrEmpty(nodePath))
                return;
            XmlDocument doc = new XmlDocument();
            doc.Load(nodePath);
            RecurveInsertToTree(doc.LastChild, treeNode);
        }

        private void RecurveInsertToTree(XmlNode rootNode,TreeNode treeNode)
        {
            if (rootNode != null && rootNode.ChildNodes != null && rootNode.ChildNodes.Count > 0 && treeNode != null)
            {
                XmlNode preNode = null;
                foreach (XmlNode temp in rootNode.ChildNodes)
                {
                    if (temp.NodeType == XmlNodeType.Text)
                        treeNode.Name += "|" + temp.InnerText;
                    if (temp.NodeType != XmlNodeType.Text && temp.NodeType != XmlNodeType.Comment)
                    {
                        TreeNode currentNode = new TreeNode(temp.Name);
                        treeNode.Nodes.Add(currentNode);
                        if (preNode != null && preNode.NodeType == XmlNodeType.Comment)
                             currentNode.Name += preNode.InnerText;
                        RecurveInsertToTree(temp, currentNode);
                    }
                    preNode = temp;
                }
            }
        }

        #region
        //private void trViewConfig_AfterSelect(object sender, TreeViewEventArgs e)
        //{
        //    TreeNode node = e.Node;
        //    if (node != null && node.Name != null)
        //    {
        //        string[] str = node.Name.ToString().Split('|');
        //        this.txtContent.Clear();
        //        this.rchComment.Clear();
        //        if (str.Length > 1)
        //        {
        //            this.rchComment.Text = str[0];
        //            this.txtContent.Text = str[1];
        //        }
        //        else
        //        {
        //            this.txtContent.Text = str[0];
        //        }
        //    }
        //}

        //private void tlsAddNode_Click(object sender, EventArgs e)
        //{
        //    UCNodeNameOrPath path = new UCNodeNameOrPath(fileList);
        //    WorkCurveHelper.OpenUC(path, false);
        //    if (DialogResult.OK == path.dialogResult)
        //    {
        //        TreeNode node = new TreeNode(path.nodeName);
        //        FileAppSerialize addSerialize = new FileAppSerialize(path.nodeName, path.nodePath);
        //        node.Tag = addSerialize;
        //        this.trViewConfig.Nodes.Add(node);
        //        _strLoadXmlPath = path.nodePath;
        //        fileList.Add(addSerialize);
        //        StartParserXml(node, path.nodePath);
        //        lst.Add(node);
        //        cboxSeachNode.Items.Add(node.Text);
        //    }
        //}

        //private void tlsDelete_Click(object sender, EventArgs e)
        //{
        //    if (this.trViewConfig.SelectedNode == null || this.trViewConfig.SelectedNode.Parent != null)
        //        return;
        //    TreeNode deleteNode = this.trViewConfig.SelectedNode;
        //    if (deleteNode.Tag != null)
        //        fileList.Remove(deleteNode.Tag as FileAppSerialize);
        //    this.trViewConfig.Nodes.Remove(deleteNode);
        //    cboxSeachNode.Items.Remove(deleteNode.Text);
        //    lst.Remove(deleteNode);
        //}

        //private void txtSearchConditon_TextChanged(object sender, EventArgs e)
        //{
        //    this.trViewConfig.Nodes.Clear();
        //    if (!string.IsNullOrEmpty(this.txtSearchConditon.Text))
        //    {
        //        List<TreeNode> listTreeNode = new List<TreeNode>();
        //        TreeNode node = lst.Find(w => w.Text == this.cboxSeachNode.Text);
        //        RecurveSearchCondition(node, ref listTreeNode);
        //        foreach (TreeNode tt in listTreeNode.Distinct())
        //            this.trViewConfig.Nodes.Add(tt);
        //    }
        //    else
        //    {
        //        FillTreeViewFormFile();
        //    }
        //}

        //private void RecurveSearchCondition(TreeNode tt,ref List<TreeNode> treeNodeList)
        //{
        //    if (tt != null && tt.Nodes.Count == 0)
        //    {
        //        if (tt.Name.Split('|')[0].ToLower().Contains(this.txtSearchConditon.Text.ToLower()))
        //            treeNodeList.Add(tt);
        //    }
        //    else if(tt != null && tt.Nodes.Count > 0)
        //    {
        //        foreach(TreeNode temp in tt.Nodes)
        //            RecurveSearchCondition(temp, ref treeNodeList);
        //    }
        //}

        //private void FillTreeViewFormFile()
        //{
        //    this.trViewConfig.Nodes.Clear();
        //    lst.Clear();
        //    cboxSeachNode.Items.Clear();
        //    foreach (FileAppSerialize temp in fileList)
        //    {
        //        TreeNode node = new TreeNode(temp.NodeName);
        //        node.Tag = temp;
        //        this.trViewConfig.Nodes.Add(node);
        //        StartParserXml(node, temp.NodePath);
        //        cboxSeachNode.Items.Add(node.Text);
        //        lst.Add(node);
        //    }
        //    if (this.cboxSeachNode.Items.Count>0)
        //        this.cboxSeachNode.SelectedIndex = 0;
        //}

        //private void btnSetting_Click(object sender, EventArgs e)
        //{
        //    if (this.trViewConfig.SelectedNode == null)
        //        return;
        //    string str = string.Empty;
        //    TreeNode rootNode = ParentNodeRoot(this.trViewConfig.SelectedNode, ref str);
        //    if (rootNode.Tag != null)
        //    {
        //        FileAppSerialize fileSerialize = rootNode.Tag as FileAppSerialize;
        //        XmlDocument document = new XmlDocument();
        //        document.Load(fileSerialize.NodePath);
        //        XmlNode temp = document.SelectSingleNode(document.LastChild.Name + str);
        //        temp.InnerText = this.txtContent.Text;
        //        document.Save(fileSerialize.NodePath);
        //    }
        //}

        #endregion

        private TreeNode ParentNodeRoot(TreeNode currentNode,ref string path)
        {
            if (currentNode == null)
                return null;
            else if (currentNode.Parent == null)
                return currentNode;
            else 
            {
                path = @"/" + currentNode.Text + path;
                TreeNode temp = ParentNodeRoot(currentNode.Parent,ref path);
                return temp;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }

        private static string fileName = Application.StartupPath + "\\config.dat";

        private void UCUserConfigParams_Load(object sender, EventArgs e)
        {
            #region
            //IFormatter formatter = new BinaryFormatter();
            //if (File.Exists(fileName))
            //{
            //    using (FileStream _FileStream = new System.IO.FileStream(fileName,
            //        System.IO.FileMode.Open,
            //        System.IO.FileAccess.Read,
            //        System.IO.FileShare.None
            //        ))
            //    {
            //        _FileStream.Position = 0;
            //        _FileStream.Seek(0, SeekOrigin.Begin);
            //        fileList= (List<FileAppSerialize>)formatter.Deserialize(_FileStream);
            //    }

            //}
            //if (fileList.Count > 0)
            //{
            //    FillTreeViewFormFile();
            //}
            #endregion
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            SaveCurrentPage();
            if (DialogResult.OK == Skyray.Controls.SkyrayMsgBox.Show(Info.SetSuccessParams, Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Information))
            {
                DifferenceDevice.interClassMain.skyrayCamera.Close();
                Process p = new Process();
                p.StartInfo.FileName = Application.ExecutablePath;
                p.Start();
                Environment.Exit(0);
            }
            EDXRFHelper.GotoMainPage(this);
        }

        private void SaveSerializeObj()
        {
            IFormatter formatter = new BinaryFormatter();
            if (File.Exists(fileName))
                File.Delete(fileName);
            Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, fileList);
            stream.Close();
        }

        private void btnApplication_Click(object sender, EventArgs e)
        {
            //SaveSerializeObj();
            SaveCurrentPage();

        }

        private void SaveCurrentPage()
        {
            if (saveData != null)
            {
                if (saveData == SaveROHSParams)
                    saveData(Application.StartupPath + "\\Parameter.xml");
                else
                    saveData(Application.StartupPath + "\\AppParams.xml");
            }
        }

        private bool ValidateTextOutContent(string str)
        {
            try 
            {
                double dd = double.Parse(str);
                return true;
            
            }
            catch {

                Msg.Show(str+" "+Info.ValidateUserInput);
                return false;
            }
        }

       
        public SaveData saveData;
        private void btnNormalOptions_Click(object sender, EventArgs e)
        {
            this.lblShowTitle.Text = btnNormalOptions.Text;
            this.panel8.BackColor = Color.White;
            foreach (Control control in this.panel6.Controls)
            {
                if (control.Name != "panel7")
                    control.Visible = false;
                else
                    control.Visible = true;
            }
            ClickLoadCommonParamsFiles(Application.StartupPath + "\\AppParams.xml");
            saveData = ApplicationSaveParamsFiles;
        }

        private void btnOptionReport_Click(object sender, EventArgs e)
        {
            this.lblShowTitle.Text = btnOptionReport.Text;
            this.panel8.BackColor = Color.White;
            foreach (Control control in this.panel6.Controls)
            {
                if (control.Name != "panel9")
                    control.Visible = false;
                else
                    control.Visible = true;
            }
            this.ReportAutotectParamsFiles(Application.StartupPath + "\\AppParams.xml");
            saveData = this.SaveReportAutotectParams;
        }

        private void btnMeasureParams_Click(object sender, EventArgs e)
        {
            this.lblShowTitle.Text = btnStartRecord.Text;
            foreach (Control control in this.panel6.Controls)
            {
                if (control.Name != "panel10")
                    control.Visible = false;
                else
                    control.Visible = true;
            }
            this.LoadMeasureParams(Application.StartupPath + "\\AppParams.xml");
            saveData = this.SaveMeasureParams;
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            this.lblShowTitle.Text = btnHistoryRecord.Text;
            foreach (Control control in this.panel6.Controls)
            {
                if (control.Name != "panel11")
                    control.Visible = false;
                else
                    control.Visible = true;
            }
            this.LoadRecordParamsFiles(Application.StartupPath + "\\AppParams.xml");
            saveData = this.SaveRecordParams;
        }

        private void btn3000_Click(object sender, EventArgs e)
        {
            this.lblShowTitle.Text = btn3000.Text;
            foreach (Control control in this.panel6.Controls)
            {
                if (control.Name != "panel12")
                    control.Visible = false;
                else
                    control.Visible = true;
            }
            this.Load3000Params(Application.StartupPath + "\\AppParams.xml");
            saveData = this.Save3000Params;
        }

        private void btnRohsOpt_Click(object sender, EventArgs e)
        {
            this.lblShowTitle.Text = btnRoHS.Text;
            foreach (Control control in this.panel6.Controls)
            {
                if (control.Name != "panel13")
                    control.Visible = false;
                else
                    control.Visible = true;
            }
            LoadRohsParams(Application.StartupPath + "\\Parameter.xml");
            saveData = this.SaveROHSParams;
        }

        private void cmBoxSpecType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmBoxSpecType.SelectedIndex == 1)
            {
                this.lblSpectrumType.Visible = true;
                this.cmBoxFileType.Visible = true;
                this.lblSpectrumTypeInfo.Visible = true;
            }
            else
            {
                this.lblSpectrumType.Visible = false;
                this.cmBoxFileType.Visible = false;
                this.lblSpectrumTypeInfo.Visible = false;
            }
        }

        private void LoadRohsParams(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            this.txtBoxPeakLeft.Text = doc.SelectSingleNode("Parameter").Attributes["bLeft"].InnerText;
            this.txtBoxBaseRight.Text = doc.SelectSingleNode("Parameter").Attributes["bRight"].InnerText;
            this.txtBoxTotalRatio.Text = doc.SelectSingleNode("Parameter").Attributes["TotalRatio"].InnerText;
            XmlNode node= doc.SelectSingleNode("Parameter/System/IsMath");
            this.chBoxInnerMatch.Checked = (node == null || string.IsNullOrEmpty(node.InnerText) ? false : int.Parse(node.InnerText) == 1 ? true : false);
            node= doc.SelectSingleNode("Parameter/System/MathTime");
            this.numTestMathTime.Value = (node == null || string.IsNullOrEmpty(node.InnerText) ? 15 : int.Parse(node.InnerText));
            XmlNodeList xmlNodeList = doc.SelectNodes("Parameter/match/NoMetal/Node");
            foreach (XmlNode xTemp in xmlNodeList)
            {
                if (xTemp.Attributes["elementName"].Value.ToString().ToLower()=="cl" && xTemp.Attributes["cureType"].Value.ToString().ToLower()=="0")
                    this.txtBoxFirstRatio.Text = xTemp.Attributes["ratio"].Value.ToString();
                 if (xTemp.Attributes["elementName"].Value.ToString().ToLower()=="cl" && xTemp.Attributes["cureType"].Value.ToString().ToLower()=="4")
                    this.txtBoxFiveRatio.Text = xTemp.Attributes["ratio"].Value.ToString();
            }
            node = doc.SelectSingleNode("Parameter/ReportResults/PassResults");
            this.txtBoxPass.Text = node.InnerText;
            node = doc.SelectSingleNode("Parameter/ReportResults/FalseResults");
            this.txtBoxFailure.Text = node.InnerText;
            node = doc.SelectSingleNode("Parameter/ReportResults/STDResults");
            this.txtBoxSTD.Text = node.InnerText;
        }

        private void SaveROHSParams(string path)
        {
            if (!ValidateTextOutContent(this.txtBoxPeakLeft.Text) || !ValidateTextOutContent(this.txtBoxBaseRight.Text) || !ValidateTextOutContent(this.txtBoxTotalRatio.Text)
                || !ValidateTextOutContent(this.txtBoxFirstRatio.Text) || !ValidateTextOutContent(this.txtBoxFiveRatio.Text))
                return;
            if (!ValidateHelper.IllegalCheck(this.txtBoxPass) || !ValidateHelper.IllegalCheck(this.txtBoxFailure) || !ValidateHelper.IllegalCheck(this.txtBoxSTD))
                return;
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            doc.SelectSingleNode("Parameter").Attributes["bLeft"].InnerText= this.txtBoxPeakLeft.Text;
            doc.SelectSingleNode("Parameter").Attributes["bRight"].InnerText= this.txtBoxBaseRight.Text;
            doc.SelectSingleNode("Parameter").Attributes["TotalRatio"].InnerText =this.txtBoxTotalRatio.Text;
            XmlNode node = doc.SelectSingleNode("Parameter/System/IsMath");
            node.InnerText=this.chBoxInnerMatch.Checked?"1":"0";
            node = doc.SelectSingleNode("Parameter/System/MathTime");
            node.InnerText = this.numTestMathTime.Value.ToString();
            XmlNodeList xmlNodeList = doc.SelectNodes("Parameter/match/NoMetal/Node");
            foreach (XmlNode xTemp in xmlNodeList)
            {
                if (xTemp.Attributes["elementName"].Value.ToString().ToLower() == "cl" && xTemp.Attributes["cureType"].Value.ToString().ToLower() == "0")
                   xTemp.Attributes["ratio"].InnerText = this.txtBoxFirstRatio.Text;
                if (xTemp.Attributes["elementName"].Value.ToString().ToLower() == "cl" && xTemp.Attributes["cureType"].Value.ToString().ToLower() == "4")
                    xTemp.Attributes["ratio"].InnerText=this.txtBoxFiveRatio.Text;
            }
            node = doc.SelectSingleNode("Parameter/ReportResults/PassResults");
            node.InnerText=this.txtBoxPass.Text;
            node = doc.SelectSingleNode("Parameter/ReportResults/FalseResults");
            node.InnerText=this.txtBoxFailure.Text;
            node = doc.SelectSingleNode("Parameter/ReportResults/STDResults");
            node.InnerText=this.txtBoxSTD.Text;
            doc.Save(path);
        }

        #region 3000
        private void Load3000Params(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode node = doc.SelectSingleNode("application/EDX3000/Is3000D");
            this.chB3000D.Checked = (node == null || string.IsNullOrEmpty(node.InnerText) ? false : int.Parse(node.InnerText) == 1 ? true : false);
            node = doc.SelectSingleNode("application/EDX3000/SimilarCurve");
            this.chBoxSilimar.Checked = (node == null || string.IsNullOrEmpty(node.InnerText) ? false : int.Parse(node.InnerText) == 1 ? true : false);
            node = doc.SelectSingleNode("application/OpenHistoryRecordType/KaratTranslaterate");
            this.txtBoxKaratContent.Text = (node == null || string.IsNullOrEmpty(node.InnerText) ? "99.995" : node.InnerText);
            node = doc.SelectSingleNode("application/EDXRFGainSet/UserUpdateParams");
            this.chkUserUpdateParams.Checked = (node == null || string.IsNullOrEmpty(node.InnerText) ? false : int.Parse(node.InnerText) == 1 ? true : false);
        }

        private void Save3000Params(string path)
        {
            if (!ValidateTextOutContent(this.txtBoxKaratContent.Text))
                return;
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode node = doc.SelectSingleNode("application/EDX3000/Is3000D");
            node.InnerText=this.chB3000D.Checked?"1":"0";
            node = doc.SelectSingleNode("application/EDX3000/SimilarCurve");
            node.InnerText = this.chBoxSilimar.Checked ? "1" : "0";
            node = doc.SelectSingleNode("application/OpenHistoryRecordType/KaratTranslaterate");
            node.InnerText = this.txtBoxKaratContent.Text;
            node = doc.SelectSingleNode("application/EDXRFGainSet/UserUpdateParams");
            node.InnerText = this.chkUserUpdateParams.Checked ? "1" : "0";
            doc.Save(path);
        }
        #endregion


        #region 记录
        private void LoadRecordParamsFiles(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode node = doc.SelectSingleNode("application/OpenHistoryRecordType/HistoryRecordShowNumber");
            this.numHistoryRecordCount.Value = (node == null || string.IsNullOrEmpty(node.InnerText) ? 10 : int.Parse(node.InnerText));
            node = doc.SelectSingleNode("application/OpenHistoryRecordType/SelectRecordNumber");
            this.numSelectRecordCount.Value = (node == null || string.IsNullOrEmpty(node.InnerText) ? 10 : int.Parse(node.InnerText));
            node = doc.SelectSingleNode("application/OpenHistoryRecordType/IsShowArea");
            this.chBoxShowArea.Checked = (node == null || string.IsNullOrEmpty(node.InnerText) ? false : int.Parse(node.InnerText) == 1 ? true : false);

            node = doc.SelectSingleNode("application/OpenHistoryRecordType/IsShowError");
            this.chBoxShowError.Checked = (node == null || string.IsNullOrEmpty(node.InnerText) ? false : int.Parse(node.InnerText) == 1 ? true : false);
            node = doc.SelectSingleNode("application/OpenHistoryRecordType/IsShowIntensity");
            this.chBoxShowIntensity.Checked = (node == null || string.IsNullOrEmpty(node.InnerText) ? false : int.Parse(node.InnerText) == 1 ? true : false);
            node = doc.SelectSingleNode("application/OpenHistoryRecordType/IsShowUnitColumns");
            this.chBoxShowUnit.Checked = (node == null || string.IsNullOrEmpty(node.InnerText) ? false : int.Parse(node.InnerText) == 1 ? true : false);
            node = doc.SelectSingleNode("application/OpenHistoryRecordType/IsShowAverageColumns");
            this.chBoxShowAverage.Checked = (node == null || string.IsNullOrEmpty(node.InnerText) ? false : int.Parse(node.InnerText) == 1 ? true : false);
            node = doc.SelectSingleNode("application/OpenHistoryRecordType/IsShowElementAllName");
            this.chBoxShowElementName.Checked = (node == null || string.IsNullOrEmpty(node.InnerText) ? false : int.Parse(node.InnerText) == 1 ? true : false);
            node = doc.SelectSingleNode("application/OpenHistoryRecordType/IsOnlyShowScanRecord");
            this.chShowTestHistory.Checked = (node == null || string.IsNullOrEmpty(node.InnerText) ? false : int.Parse(node.InnerText) == 1 ? true : false);

            node = doc.SelectSingleNode("application/SoftWareCaculate/ContentBit");
            this.numContentPrise.Value = (node == null || string.IsNullOrEmpty(node.InnerText) ? 4 : int.Parse(node.InnerText));

            node = doc.SelectSingleNode("application/ShowDialog/IsShow");
            this.chBoxShowResult.Checked = (node == null || string.IsNullOrEmpty(node.InnerText) ? false : int.Parse(node.InnerText) == 1 ? true : false);
        }

        private void SaveRecordParams(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode node = doc.SelectSingleNode("application/OpenHistoryRecordType/HistoryRecordShowNumber");
           node.InnerText = this.numHistoryRecordCount.Value.ToString();
            node = doc.SelectSingleNode("application/OpenHistoryRecordType/SelectRecordNumber");
            node.InnerText=this.numSelectRecordCount.Value.ToString();
            node = doc.SelectSingleNode("application/OpenHistoryRecordType/IsShowArea");
            node.InnerText=this.chBoxShowArea.Checked?"1":"0";

            node = doc.SelectSingleNode("application/OpenHistoryRecordType/IsShowError");
            node.InnerText = this.chBoxShowError.Checked ? "1" : "0";
            node = doc.SelectSingleNode("application/OpenHistoryRecordType/IsShowIntensity");
            node.InnerText = this.chBoxShowIntensity.Checked ? "1" : "0";
            node = doc.SelectSingleNode("application/OpenHistoryRecordType/IsShowUnitColumns");
            node.InnerText = this.chBoxShowUnit.Checked ? "1" : "0";
            node = doc.SelectSingleNode("application/OpenHistoryRecordType/IsShowAverageColumns");
            node.InnerText = this.chBoxShowAverage.Checked ? "1" : "0";
            node = doc.SelectSingleNode("application/OpenHistoryRecordType/IsShowElementAllName");
            node.InnerText = this.chBoxShowElementName.Checked ? "1" : "0";
            node = doc.SelectSingleNode("application/OpenHistoryRecordType/IsOnlyShowScanRecord");
            node.InnerText = this.chShowTestHistory.Checked ? "1" : "0";

            node = doc.SelectSingleNode("application/SoftWareCaculate/ContentBit");
            node.InnerText=this.numContentPrise.Value.ToString();
            node = doc.SelectSingleNode("application/ShowDialog/IsShow");
            node.InnerText = this.chBoxShowResult.Checked ? "1" : "0";
            doc.Save(path);
        }
        #endregion

        #region 测量参数
        private void LoadMeasureParams(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode node = doc.SelectSingleNode("application/InitParams/InitTime");
            this.numInitTime.Value = (node == null || string.IsNullOrEmpty(node.InnerText) ? 10 : int.Parse(node.InnerText));
            node = doc.SelectSingleNode("application/TestParams/AdjustTime");
            this.numAdjustTime.Value = (node == null || string.IsNullOrEmpty(node.InnerText) ? 10 : int.Parse(node.InnerText));

            node = doc.SelectSingleNode("application/TestParams/AdjustInterva");
            this.numTotalAdjustInterval.Value = (node == null || string.IsNullOrEmpty(node.InnerText) ? 5 : int.Parse(node.InnerText));

            node = doc.SelectSingleNode("application/TestParams/DirectRun");
            this.chBoxDirectRun.Checked = (node == null || string.IsNullOrEmpty(node.InnerText) ? false : bool.Parse(node.InnerText));

            node = doc.SelectSingleNode("application/TestParams/IsShowLoss");
            this.chBoxShowLoss.Checked = (node == null || string.IsNullOrEmpty(node.InnerText) ? false : int.Parse(node.InnerText) == 1 ? true : false);

            node = doc.SelectSingleNode("application/TestParams/IsShowRh");
            this.chBoxShowLoss.Checked = (node == null || string.IsNullOrEmpty(node.InnerText) ? false : int.Parse(node.InnerText) == 1 ? true : false);

            node = doc.SelectSingleNode("application/TestParams/RhlayerOnlyMainElementInfluence");
            this.chBoxInflunceMain.Checked = (node == null || string.IsNullOrEmpty(node.InnerText) ? false : int.Parse(node.InnerText) == 1 ? true : false);

            node = doc.SelectSingleNode("application/TestParams/IsMatch");
            this.chBoxShowMatch.Checked = (node == null || string.IsNullOrEmpty(node.InnerText) ? false : bool.Parse(node.InnerText));

            node = doc.SelectSingleNode("application/MoveStationVisible/Distance");
            this.txtBoxPumpMovestation.Text = (node == null || string.IsNullOrEmpty(node.InnerText) ? "5" : node.InnerText);

            node = doc.SelectSingleNode("application/MoveStationVisible/X");
            this.txtBoxX.Text = (node == null || string.IsNullOrEmpty(node.InnerText) ? "700" : node.InnerText);

            node = doc.SelectSingleNode("application/MoveStationVisible/Y");
            this.txtBoxY.Text = (node == null || string.IsNullOrEmpty(node.InnerText) ? "322" : node.InnerText);


        }

        private void SaveMeasureParams(string path)
        {
            if (!ValidateTextOutContent(this.txtBoxPumpMovestation.Text) || !ValidateTextOutContent(this.txtBoxX.Text) || !ValidateTextOutContent(this.txtBoxY.Text))
                return;
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode node = doc.SelectSingleNode("application/InitParams/InitTime");
            node.InnerText=this.numInitTime.Value.ToString();
            node = doc.SelectSingleNode("application/TestParams/AdjustTime");
            node.InnerText=this.numAdjustTime.Value.ToString();

            node = doc.SelectSingleNode("application/TestParams/AdjustInterva");
            node.InnerText=this.numTotalAdjustInterval.Value.ToString();

            node = doc.SelectSingleNode("application/TestParams/DirectRun");
            node.InnerText = this.chBoxDirectRun.Checked.ToString();

            node = doc.SelectSingleNode("application/TestParams/IsShowLoss");
            node.InnerText = this.chBoxShowLoss.Checked ? "1" : "0";

            node = doc.SelectSingleNode("application/TestParams/IsShowRh");
            node.InnerText = this.chBoxShowLoss.Checked ? "1" : "0";
            node = doc.SelectSingleNode("application/TestParams/RhlayerOnlyMainElementInfluence");
            node.InnerText = this.chBoxInflunceMain.Checked ? "1" : "0";

            node = doc.SelectSingleNode("application/TestParams/IsMatch");
            node.InnerText = this.chBoxShowMatch.Checked.ToString();

            node = doc.SelectSingleNode("application/MoveStationVisible/Distance");
            node.InnerText=this.txtBoxPumpMovestation.Text;
            node = doc.SelectSingleNode("application/MoveStationVisible/X");
            node.InnerText = this.txtBoxX.Text;

            node = doc.SelectSingleNode("application/MoveStationVisible/Y");
           node.InnerText = this.txtBoxY.Text;
            doc.Save(path);

        }
        #endregion 


        #region 报告与检测
        private void ReportAutotectParamsFiles(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode node = doc.SelectSingleNode("application/Excel/ExcelModeType");
            this.numTemplateType.Value = (node == null || string.IsNullOrEmpty(node.InnerText) ? 2 : int.Parse(node.InnerText));
            node = doc.SelectSingleNode("application/Excel/IsEncryption");
            this.chBoxExcelPassword.Checked = (node == null || string.IsNullOrEmpty(node.InnerText) ? false : int.Parse(node.InnerText) == 1 ? true : false);

            node = doc.SelectSingleNode("application/AutoDetection/IsDetection");
            this.chboxAutoDetection.Checked = (node == null || string.IsNullOrEmpty(node.InnerText) ? false : int.Parse(node.InnerText) == 1 ? true : false);

            node = doc.SelectSingleNode("application/AutoDetection/DetectionType");
            this.numDetectionType.Value = node == null ? 1 : int.Parse(node.InnerText);
            numDetectionType_ValueChanged(null, null);
            XmlNode xmlCalibrationList = doc.SelectSingleNode("application/Detection/Item");
            if (xmlCalibrationList != null)
            {
                txtResolve.Text = xmlCalibrationList.Attributes["Resolve"] == null ? "0" : xmlCalibrationList.Attributes["Resolve"].InnerText;
                numResoveError.Text = xmlCalibrationList.Attributes["ResolveError"] == null ? "0" : xmlCalibrationList.Attributes["ResolveError"].InnerText;
                this.txtPeakChannel.Text = xmlCalibrationList.Attributes["PeakChannel"] == null ? "0" : xmlCalibrationList.Attributes["PeakChannel"].InnerText;
                this.txtErrorChannel.Text = xmlCalibrationList.Attributes["PeakError"] == null ? "0" : xmlCalibrationList.Attributes["PeakError"].InnerText;
                this.txtBoxMotorDelay.Text = xmlCalibrationList.Attributes["MotorDelayTime"] == null ? "0" : xmlCalibrationList.Attributes["MotorDelayTime"].InnerText;
                this.txtBoxDectorTime.Text = xmlCalibrationList.Attributes["DetectorDelayTime"] == null ? "0" : xmlCalibrationList.Attributes["DetectorDelayTime"].InnerText;
                txtPeakSec.Text = xmlCalibrationList.Attributes["PeakSecChannel"] == null ? "0" : xmlCalibrationList.Attributes["PeakSecChannel"].InnerText;
                txtPeakSecError.Text = xmlCalibrationList.Attributes["PeakSecError"] == null ? "0" : xmlCalibrationList.Attributes["PeakSecError"].InnerText;
                txtCountRate.Text = xmlCalibrationList.Attributes["CountRate"] == null ? "0" : xmlCalibrationList.Attributes["CountRate"].InnerText;
                txtCountRateError.Text = xmlCalibrationList.Attributes["CountRateError"] == null ? "0" : xmlCalibrationList.Attributes["CountRateError"].InnerText;
                this.txtBoxResolve.Text = xmlCalibrationList.Attributes["HalfWidth"] == null ? "0" : xmlCalibrationList.Attributes["HalfWidth"].InnerText;
                this.txtBoxResolveError.Text = xmlCalibrationList.Attributes["HalfWidthError"] == null ? "0" : xmlCalibrationList.Attributes["HalfWidthError"].InnerText;
            }
        }

        private void SaveReportAutotectParams(string path)
        {
            if (!ValidateTextOutContent(this.txtBoxResolve.Text) || !ValidateTextOutContent(this.txtBoxResolveError.Text) || !ValidateTextOutContent(this.txtPeakChannel.Text)
                || !ValidateTextOutContent(this.txtErrorChannel.Text) || !ValidateTextOutContent(this.txtBoxMotorDelay.Text) || !ValidateTextOutContent(this.txtBoxDectorTime.Text)
                || !ValidateTextOutContent(this.txtPeakSec.Text) || !ValidateTextOutContent(this.txtPeakSecError.Text) || !ValidateTextOutContent(this.txtCountRate.Text) || !ValidateTextOutContent(this.txtCountRateError.Text))
                return;
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode node = doc.SelectSingleNode("application/Excel/ExcelModeType");
            if (node != null)
                node.InnerText = this.numTemplateType.Value.ToString();

            node = doc.SelectSingleNode("application/Excel/IsEncryption");
            if (node != null)
                node.InnerText = this.chBoxExcelPassword.Checked ? "1" : "0";
            node = doc.SelectSingleNode("application/AutoDetection/IsDetection");
            if (node != null)
            {
                node.InnerText = this.chboxAutoDetection.Checked ? "1" : "0";
                WorkCurveHelper.IsAutoDetection = this.chboxAutoDetection.Checked;
            }
            node = doc.SelectSingleNode("application/AutoDetection/DetectionType");
            if (node != null)
            {
                node.InnerText = this.numDetectionType.Value.ToString();
            }
            XmlNode xmlCalibrationList = doc.SelectSingleNode("application/Detection/Item");
            if (xmlCalibrationList != null)
            {
                if (xmlCalibrationList.Attributes["Resolve"] != null)
                    xmlCalibrationList.Attributes["Resolve"].InnerText = this.txtResolve.Text;
                if (xmlCalibrationList.Attributes["ResolveError"] != null)
                    xmlCalibrationList.Attributes["ResolveError"].InnerText = this.numResoveError.Text;
                if (xmlCalibrationList.Attributes["PeakChannel"] != null)
                    xmlCalibrationList.Attributes["PeakChannel"].InnerText = this.txtPeakChannel.Text;
                if (xmlCalibrationList.Attributes["PeakError"] != null)
                    xmlCalibrationList.Attributes["PeakError"].InnerText = this.txtErrorChannel.Text;
                if (xmlCalibrationList.Attributes["MotorDelayTime"] != null)
                    xmlCalibrationList.Attributes["MotorDelayTime"].InnerText = this.txtBoxMotorDelay.Text;
                if (xmlCalibrationList.Attributes["DetectorDelayTime"] != null)
                    xmlCalibrationList.Attributes["DetectorDelayTime"].InnerText = this.txtBoxDectorTime.Text;
                if (xmlCalibrationList.Attributes["PeakSecChannel"] != null)
                    xmlCalibrationList.Attributes["PeakSecChannel"].InnerText = txtPeakSec.Text;
                if (xmlCalibrationList.Attributes["PeakSecError"] != null)
                    xmlCalibrationList.Attributes["PeakSecError"].InnerText = txtPeakSecError.Text;
                if (xmlCalibrationList.Attributes["CountRate"] != null)
                    xmlCalibrationList.Attributes["CountRate"].InnerText = txtCountRate.Text;
                if (xmlCalibrationList.Attributes["CountRateError"] != null)
                    xmlCalibrationList.Attributes["CountRateError"].InnerText = txtCountRateError.Text;
                if (xmlCalibrationList.Attributes["HalfWidth"] != null)
                    xmlCalibrationList.Attributes["HalfWidth"].InnerText = txtBoxResolve.Text;
                if (xmlCalibrationList.Attributes["HalfWidthError"] != null)
                    xmlCalibrationList.Attributes["HalfWidthError"].InnerText = txtBoxResolveError.Text;
            }
         doc.Save(path);
        }
        #endregion

        #region 常规
        private void ClickLoadCommonParamsFiles(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNode node = doc.SelectSingleNode("application/IsExistsKey/Key");
            WorkCurveHelper.IsExistsLock = (node == null || string.IsNullOrEmpty(node.InnerText) ? false : int.Parse(node.InnerText) == 1 ? true : false);
            this.chBoxExistsKey.Checked = WorkCurveHelper.IsExistsLock;
            node = doc.SelectSingleNode("application/Radiation/ShowIcon");
            this.chBRadiation.Checked = (node == null || string.IsNullOrEmpty(node.InnerText) ? false : int.Parse(node.InnerText) == 1 ? true : false);
            node = doc.SelectSingleNode("application/IsOpenRegister/Key");
            this.chBRegisterRight.Checked = (node==null || string.IsNullOrEmpty(node.InnerText) ? false : int.Parse(node.InnerText) == 1 ? true : false);
            node = doc.SelectSingleNode("application/ElementParams/DefaultColor");
            this.chkDefaultColor.Checked = (node == null || string.IsNullOrEmpty(node.InnerText) ? false : int.Parse(node.InnerText) == 0 ? true : false);
            node = doc.SelectSingleNode("application/TestParams/AllowOpenCoverComman");
            this.chkOpenCurve.Checked = (node == null || string.IsNullOrEmpty(node.InnerText) ? false : int.Parse(node.InnerText) == 0 ? true : false);
            XmlNode xmlCalibrationList = doc.SelectSingleNode("application/PeakCalibrate/Item");
               
            WorkCurveHelper.IPeakCalibration = Convert.ToBoolean(int.Parse(xmlCalibrationList.Attributes["iCalibration"].InnerText));
            WorkCurveHelper.FastLimit = int.Parse(xmlCalibrationList.Attributes["FastLimiter"].InnerText);
            WorkCurveHelper.PeakErrorChannel = int.Parse(xmlCalibrationList.Attributes["ErrorChannel"].InnerText);
            WorkCurveHelper.InitChannelError = double.Parse(xmlCalibrationList.Attributes["InitChannelError"].InnerText);
            WorkCurveHelper.IShowQuickInfo = Convert.ToBoolean(int.Parse(xmlCalibrationList.Attributes["ShowQuickInfo"].InnerText));
            this.chBoxPeakCurve.Checked = WorkCurveHelper.IPeakCalibration;
            this.txtPeakRange.Text = WorkCurveHelper.PeakErrorChannel.ToString();
            this.txtInitPeakRange.Text = WorkCurveHelper.InitChannelError.ToString();
            this.txtFastLimit.Text = WorkCurveHelper.FastLimit.ToString();
            this.chBoxShowQuick.Checked = WorkCurveHelper.IShowQuickInfo;
            node = doc.SelectSingleNode("application/SpecExport/SaveType");
            if (node != null)
               this.cmBoxSpecType.SelectedIndex = this.cmBoxSpecType.Items.IndexOf(node.InnerText);
            node = doc.SelectSingleNode("application/SpecExport/Type");
            if (node != null)
                this.cmBoxFileType.SelectedIndex = this.cmBoxFileType.Items.IndexOf(node.InnerText);
            this.cbxPrinterSetting.SelectedIndex = WorkCurveHelper.PrinterType;
            if (GP.CurrentUser.Role.RoleType.ToString() != "0")//只有管理员权限才可以显示此功能 Strong 2013-03-20
                this.chkOpenCurve.Visible = false;
        }

        private void ApplicationSaveParamsFiles(string path)
        {
            if (!ValidateTextOutContent(this.txtFastLimit.Text) || !ValidateTextOutContent(this.txtPeakRange.Text) || !ValidateTextOutContent(this.txtInitPeakRange.Text))
                return;
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode node = doc.SelectSingleNode("application/IsExistsKey/Key");
            if (node != null)
                node.InnerText = this.chBoxExistsKey.Checked ? "1" : "0";
            node = doc.SelectSingleNode("application/Radiation/ShowIcon");
            if (node != null)
                node.InnerText = this.chBRadiation.Checked ? "1" : "0";
            node = doc.SelectSingleNode("application/IsOpenRegister/Key");
            if (node != null)
                node.InnerText = this.chBRegisterRight.Checked ? "1" : "0";
            node = doc.SelectSingleNode("application/ElementParams/DefaultColor");
            if (node != null)
                node.InnerText = this.chkDefaultColor.Checked ? "0" : "1";

            node = doc.SelectSingleNode("application/TestParams/AllowOpenCoverComman");
            if (node != null)
                node.InnerText = this.chkOpenCurve.Checked ? "0" : "1";
                
            XmlNode xmlCalibrationList = doc.SelectSingleNode("application/PeakCalibrate/Item");
            xmlCalibrationList.Attributes["iCalibration"].InnerText = this.chBoxPeakCurve.Checked ? "1" : "0";
            xmlCalibrationList.Attributes["FastLimiter"].InnerText = this.txtFastLimit.Text;
            xmlCalibrationList.Attributes["ErrorChannel"].InnerText = this.txtPeakRange.Text;
            xmlCalibrationList.Attributes["InitChannelError"].InnerText = this.txtInitPeakRange.Text;
            xmlCalibrationList.Attributes["ShowQuickInfo"].InnerText = this.chBoxShowQuick.Checked ? "1" : "0";
            node = doc.SelectSingleNode("application/SpecExport/SaveType");
            if (node != null)
                node.InnerText = this.cmBoxSpecType.SelectedItem.ToString();
            node = doc.SelectSingleNode("application/SpecExport/Type");
            if (node != null)
                node.InnerText = this.cmBoxFileType.SelectedItem.ToString();

            WorkCurveHelper.PrinterType = cbxPrinterSetting.SelectedIndex;
            node = doc.SelectSingleNode("application/Report/PrinterType");
            if (node != null)
                node.InnerText = WorkCurveHelper.PrinterType.ToString();
            else 
            {
                node = doc.SelectSingleNode("application/Report");
                if (node != null)
                {
                    XmlElement newXmlNode_label = doc.CreateElement("PrinterType");
                    newXmlNode_label.InnerText = WorkCurveHelper.PrinterType.ToString();
                    node.AppendChild(newXmlNode_label);
                }
            }
            doc.Save(path);
           
        }
        #endregion

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        public delegate void SaveData(string path);

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnDir_Click(object sender, EventArgs e)
        {
            this.lblShowTitle.Text = btnDir.Text;
            foreach (Control control in this.panel6.Controls)
            {
                if (control.Name != "panel1")
                    control.Visible = false;
                else
                    control.Visible = true;
            }
            this.LoadDir(Application.StartupPath + "\\AppParams.xml");
            saveData = SaveDir;
        }

        private void btnSelectPath_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == this.folderBrowserDialog1.ShowDialog())
                this.txtBoxBackupPath.Text = this.folderBrowserDialog1.SelectedPath;
        }

        private void LoadDir(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode node = doc.SelectSingleNode("application/ProbeFile/BackupPath");
            if (node != null)
                this.txtBoxBackupPath.Text = node.InnerText;

            node = doc.SelectSingleNode("application/ProbeFile/ProbeInterval");
            if (node != null)
                this.numDetectInterval.Value = string.IsNullOrEmpty(node.InnerText)?0:int.Parse(node.InnerText);

            node = doc.SelectSingleNode("application/ProbeFile/IsOverided");
            this.chBoxOverided.Checked = (node == null || string.IsNullOrEmpty(node.InnerText) ? false : int.Parse(node.InnerText) == 1 ? true : false);
        }

        private void SaveDir(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode node = doc.SelectSingleNode("application/ProbeFile/BackupPath");
            if (node != null)
                node.InnerText=this.txtBoxBackupPath.Text;

            node = doc.SelectSingleNode("application/ProbeFile/ProbeInterval");
            if (node != null)
               node.InnerText = this.numDetectInterval.Value.ToString();

            node = doc.SelectSingleNode("application/ProbeFile/IsOverided");
           node.InnerText = this.chBoxOverided.Checked?"1":"0";
           doc.Save(path);
        }

        private void btnXRF_Click(object sender, EventArgs e)
        {
            this.lblShowTitle.Text = btnDir.Text;
            foreach (Control control in this.panel6.Controls)
            {
                if (control.Name != "panel2")
                    control.Visible = false;
                else
                    control.Visible = true;
            }
            this.LoadXRF(Application.StartupPath + "\\AppParams.xml");
            saveData = SaveXRF;
        }

        #region
        private void SaveXRF(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode node = doc.SelectSingleNode("application/Match/AutoMatch");
            if (node != null)
                node.InnerText = this.chkEnabelAuto.Checked.ToString();

            node = doc.SelectSingleNode("application/Match/DirectCaculate");
            if (node != null)
                node.InnerText = this.chkEnableDirectCaculate.Checked.ToString();
            doc.Save(path);
        }

        private void LoadXRF(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode node = doc.SelectSingleNode("application/Match/AutoMatch");
            this.chkEnabelAuto.Checked = (node == null || string.IsNullOrEmpty(node.InnerText)) ? true : bool.Parse(node.InnerText);

            node = doc.SelectSingleNode("application/Match/DirectCaculate");
            this.chkEnableDirectCaculate.Checked = (node == null || string.IsNullOrEmpty(node.InnerText)) ? false : bool.Parse(node.InnerText);

            //node = doc.SelectSingleNode("application/ProbeFile/MatchTest");
            //this.chkMatchTest.Checked = (node != null || string.IsNullOrEmpty(node.InnerText)) ? false : bool.Parse(node.InnerText);
        }
        #endregion

        
        private Thread thread = null;

        private delegate void EnableControl(bool flag);
        private EnableControl DEnableControl = null;

        private delegate void TxtControl();
        private TxtControl WriteTxt = null;

        private bool isDetectionDemarcate = false;
        private void btnDetectionDemarcate_Click(object sender, EventArgs e)
        {
            DEnableControl = new EnableControl(setControlEnable);
            WriteTxt = new TxtControl(writeText);
            thread = new Thread(new ThreadStart(Detection));
            thread.Start();
        }

        private void writeText()
        {
            txtCountRate.Text = DifferenceDevice.interClassMain.deviceMeasure.interfacce.ReturnCountRate.ToString("f1");
            txtBoxResolve.Text = DifferenceDevice.interClassMain.calcResolve();
            txtPeakChannel.Text = DifferenceDevice.interClassMain.deviceMeasure.interfacce.MaxChannelRealTime.ToString("f1");
            txtPeakSec.Text = DifferenceDevice.interClassMain.peakSec();
            txtResolve.Text = DifferenceDevice.interClassMain.deviceResolve.CalculateResolve().ToString("f1");
        }

        private void RecodeInfo()
        {
            txtCountRateNow.Text = DifferenceDevice.interClassMain.deviceMeasure.interfacce.ReturnCountRate.ToString("f1");
            txtBoxResolveNow.Text = DifferenceDevice.interClassMain.calcResolve();
            txtPeakChannelNow.Text = DifferenceDevice.interClassMain.deviceMeasure.interfacce.MaxChannelRealTime.ToString("f1");
            txtPeakSecNow.Text = DifferenceDevice.interClassMain.peakSec();
            txtResolveNow.Text = DifferenceDevice.interClassMain.deviceResolve.CalculateResolve().ToString("f1");
        }

        private void setControlEnable(bool flag)
        {
            btnStop.Enabled = !flag;
            btnDetectionDemarcate.Enabled = flag;
            btnDetection.Enabled = flag;
            btnApplication.Enabled = flag;
            btnSubmit.Enabled = flag;
            btnCancel.Enabled = flag;
            panel5.Enabled = flag;
            this.Refresh();
            if (!flag)
                timer1.Enabled = true;
            else
                timer1.Enabled = false;
        }

        private void Detection()
        {
            if (DialogResult.OK == Msg.Show(Info.PutInitSample, MessageBoxButtons.OKCancel, MessageBoxIcon.Information))
            {
                try
                {
                    DifferenceDevice.interClassMain.LoadDetectionParam();
                    DifferenceDevice.interClassMain.SelfCheckObject = new SelfCheckObject();
                    isDetectionDemarcate = true;
                    this.Invoke(DEnableControl, false);
                    if (DifferenceDevice.interClassMain.DetectionDetectorDemarcate(WorkCurveHelper.DeviceCurrent))
                    {
                        this.Invoke(WriteTxt);
                        Msg.Show(Info.strDetectionDemarcateSuccess);
                    }
                    else
                    {
                        DifferenceDevice.interClassMain.DetectionClosePumpLock();
                        Msg.Show(Info.strDetectionDemarcateFail);
                    }
                    this.Invoke(DEnableControl, true);
                    isDetectionDemarcate = false;
                }
                catch { }
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            DifferenceDevice.interClassMain.StopDetection();
            //this.Invoke(DEnableControl, true);
        }

        private void Detect()
        {
            if (DialogResult.OK == Msg.Show(Info.PutInitSample, MessageBoxButtons.OKCancel, MessageBoxIcon.Information))
            {
                try
                {
                    DifferenceDevice.interClassMain.LoadDetectionParam();
                    DifferenceDevice.interClassMain.SelfCheckObject = new SelfCheckObject();
                    isDetectionDemarcate = true;
                    this.Invoke(DEnableControl, false);
                    if (DifferenceDevice.interClassMain.DetectionDetector(WorkCurveHelper.DeviceCurrent))
                    {
                        Msg.Show(Info.strDetectionSuccess);
                    }
                    else
                    {
                        DifferenceDevice.interClassMain.DetectionClosePumpLock();
                        Msg.Show(Info.strDetectionFaild);
                    }
                    this.Invoke(DEnableControl, true);
                    isDetectionDemarcate = false;
                }
                catch { }
            }
        }

        private void btnDetection_Click(object sender, EventArgs e)
        {
            DEnableControl = new EnableControl(setControlEnable);
            WriteTxt = new TxtControl(writeText);
            thread = new Thread(new ThreadStart(Detect));
            thread.Start();
        }

        public override void OpenUC(bool flag, string TitleName, bool isModel, bool noneStyle)
        {
            if (!this.LoadConditionAnalyser())
                return;
            Form form = new Form();
            form.BackColor = Color.White;
            form.MinimizeBox = false;
            form.ShowInTaskbar = false;
            int padSpace = 0;
            form.Padding = new Padding(padSpace, padSpace, padSpace, padSpace);
            form.Controls.Add(this);
            form.MaximizeBox = flag;
            form.Text = TitleName;
            form.FormClosing += new FormClosingEventHandler(form_FormClosing);
            if (!flag)
            {
                form.FormBorderStyle = FormBorderStyle.FixedSingle;
            }
            if (noneStyle)
            {
                form.FormBorderStyle = FormBorderStyle.None;
            }
            this.AutoScroll = true;
            form.ClientSize = new Size(this.Width + padSpace * 2, this.Height + padSpace * 2);
            int width = Screen.PrimaryScreen.Bounds.Width;
            int hight = Screen.PrimaryScreen.Bounds.Height;
            if (width < this.Width)
                form.ClientSize = new Size(width, form.ClientSize.Height);
            if (hight < this.Height)
                form.ClientSize = new Size(form.ClientSize.Width, hight);
            form.ShowIcon = false;
            this.Dock = DockStyle.Fill;
            form.StartPosition = FormStartPosition.CenterScreen;
            if (isModel)
            {
                if (DialogResult.OK == form.ShowDialog())
                    this.ExcuteEndProcess(null);
                else
                {
                    this.ExcuteCloseProcess(null);
                }
            }
            else
            {
                form.Show();
            }
        }

        void form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isDetectionDemarcate)
                e.Cancel = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RecodeInfo();
        }

        private void numDetectionType_ValueChanged(object sender, EventArgs e)
        {
            if (numDetectionType.Value == 0)
            {
                numResoveError.Enabled = true;
                txtErrorChannel.Enabled = true;
                txtBoxResolveError.Enabled = false;
                txtPeakSecError.Enabled = false;
                txtCountRateError.Enabled = false;
            }
            else
            {
                numResoveError.Enabled = false;
                txtErrorChannel.Enabled = false;
                txtBoxResolveError.Enabled = true;
                txtPeakSecError.Enabled = true;
                txtCountRateError.Enabled = true;
            }
        }


        private void LoadSavePath(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode node = doc.SelectSingleNode("application/SavePath/IsSaveSpecData");
            this.chkIsSaveSpec.Checked = (node == null || string.IsNullOrEmpty(node.InnerText) ? true : (node.InnerText=="1"?true:false));

            node = doc.SelectSingleNode("application/SavePath/SaveSpecDataPath");
            this.txtFileSpecPath.Text = (node == null || string.IsNullOrEmpty(node.InnerText) ? Application.StartupPath + "\\Spectrum" : node.InnerText);

            //node = doc.SelectSingleNode("application/SavePath/IsSaveSampleImage");
            //this.chkIsSaveSampleImage.Checked = (node == null || string.IsNullOrEmpty(node.InnerText) ? true : (node.InnerText == "1" ? true : false));

            node = doc.SelectSingleNode("application/SavePath/SaveSampleImagePath");
            this.txtSampleImagePath.Text = (node == null || string.IsNullOrEmpty(node.InnerText) ? Application.StartupPath + "\\Image\\SampleImage" + "\\"+(DifferenceDevice.IsXRF ? "EDXX" : (DifferenceDevice.IsRohs ? "EDXR" : (DifferenceDevice.IsThick ? "EDXT" : "EDX3000"))) : node.InnerText);

            node = doc.SelectSingleNode("application/SavePath/IsSaveSpecImage");
            this.chkSpecImage.Checked = (node == null || string.IsNullOrEmpty(node.InnerText) ? false : int.Parse(node.InnerText) == 1 ? true : false);

            node = doc.SelectSingleNode("application/SavePath/SaveSpecImagePath");
            this.txtSpecImagePath.Text = (node == null || string.IsNullOrEmpty(node.InnerText) ? Application.StartupPath + "\\Image\\SpecImage" + "\\" + (DifferenceDevice.IsXRF ? "EDXX" : (DifferenceDevice.IsRohs ? "EDXR" : (DifferenceDevice.IsThick ? "EDXT" : "EDX3000"))) : node.InnerText);

            node = doc.SelectSingleNode("application/SavePath/IsSaveHistoryRecord");
            this.chkHistory.Checked = (node == null || string.IsNullOrEmpty(node.InnerText) ? true : int.Parse(node.InnerText) == 1 ? true : false);
        }

        private void SaveSavePath(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNode node = doc.SelectSingleNode("application/SavePath/IsSaveSpecData");
            node.InnerText = this.chkIsSaveSpec.Checked ? "1" : "0";

            node = doc.SelectSingleNode("application/SavePath/SaveSpecDataPath");
            node.InnerText = this.txtFileSpecPath.Text;

            //node = doc.SelectSingleNode("application/SavePath/IsSaveSampleImage");
            //node.InnerText = this.chkIsSaveSampleImage.Checked ? "1" : "0";

            node = doc.SelectSingleNode("application/SavePath/SaveSampleImagePath");
            node.InnerText = this.txtSampleImagePath.Text;

            node = doc.SelectSingleNode("application/SavePath/IsSaveSpecImage");
            node.InnerText = this.chkSpecImage.Checked ? "1" : "0";

            node = doc.SelectSingleNode("application/SavePath/SaveSpecImagePath");
            node.InnerText = this.txtSpecImagePath.Text;

            node = doc.SelectSingleNode("application/SavePath/IsSaveHistoryRecord");
            node.InnerText = this.chkHistory.Checked ? "1" : "0";
            doc.Save(path);
        }

        private void btnSaveSet_Click(object sender, EventArgs e)
        {
            this.lblShowTitle.Text = btnSaveSet.Text;
            foreach (Control control in this.panel6.Controls)
            {
                if (control.Name != "panel3")
                    control.Visible = false;
                else
                    control.Visible = true;
            }
            this.LoadSavePath(Application.StartupPath + "\\AppParams.xml");
            saveData = SaveSavePath;
        }

        private void btnExploreFileSpec_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == this.folderBrowserDialog2.ShowDialog())
                this.txtFileSpecPath.Text = this.folderBrowserDialog2.SelectedPath;
        }

        private void btnExploreSampleImage_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == this.folderBrowserDialog2.ShowDialog())
                this.txtSampleImagePath.Text = this.folderBrowserDialog2.SelectedPath;
        }

        private void btnExploreSpecImage_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == this.folderBrowserDialog2.ShowDialog())
                this.txtSpecImagePath.Text = this.folderBrowserDialog2.SelectedPath;
        }

        private void numTemplateType_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.numTemplateType.Value.ToString().Equals("1"))
            {
                this.toolTip1.SetToolTip(this.numTemplateType,Info.strPrintModel1);
            }
            else if (this.numTemplateType.Value.ToString().Equals("2"))
            {
                this.toolTip1.SetToolTip(this.numTemplateType, Info.strPrintModel2);
            }
            else if (this.numTemplateType.Value.ToString().Equals("3"))
            {
                this.toolTip1.SetToolTip(this.numTemplateType, Info.strPrintModel3);
            }
            else if (this.numTemplateType.Value.ToString().Equals("4"))
            {
                this.toolTip1.SetToolTip(this.numTemplateType, Info.strPrintModel4);
            }
            else if (this.numTemplateType.Value.ToString().Equals("5"))
            {
                this.toolTip1.SetToolTip(this.numTemplateType, Info.strPrintModel5);
            }
            else if (this.numTemplateType.Value.ToString().Equals("6"))
            {
                this.toolTip1.SetToolTip(this.numTemplateType, Info.strPrintModel6);
            }
            else if (this.numTemplateType.Value.ToString().Equals("7"))
            {
                this.toolTip1.SetToolTip(this.numTemplateType, Info.strPrintModel7);
            }
            else if (this.numTemplateType.Value.ToString().Equals("8"))
            {
                this.toolTip1.SetToolTip(this.numTemplateType, Info.strPrintModel8);
            }
            else if (this.numTemplateType.Value.ToString().Equals("9"))
            {
                this.toolTip1.SetToolTip(this.numTemplateType, Info.strPrintModel9);
            }
            else if (this.numTemplateType.Value.ToString().Equals("10"))
            {
                this.toolTip1.SetToolTip(this.numTemplateType, Info.strPrintModel10);
            }
            else if (this.numTemplateType.Value.ToString().Equals("11"))
            {
                this.toolTip1.SetToolTip(this.numTemplateType, Info.strPrintModel11);
            }
            else if (this.numTemplateType.Value.ToString().Equals("12"))
            {
                this.toolTip1.SetToolTip(this.numTemplateType, Info.strPrintModel12);
            }
            else if (this.numTemplateType.Value.ToString().Equals("13"))
            {
                this.toolTip1.SetToolTip(this.numTemplateType, Info.strPrintModel13);
            }
            else if (this.numTemplateType.Value.ToString().Equals("14"))
            {
                this.toolTip1.SetToolTip(this.numTemplateType, Info.strPrintModel14);
            }
            else if (this.numTemplateType.Value.ToString().Equals("15"))
            {
                this.toolTip1.SetToolTip(this.numTemplateType, Info.strPrintModel15);
            }
        }

        private void cbxPrinterSetting_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbxPrinterSetting.SelectedIndex == 1)
            {
                this.btnBlueSetting.Visible = true;
            }
            else
            {
                this.btnBlueSetting.Visible = false;
            }
        }

        private void btnBlueSetting_Click(object sender, EventArgs e)
        {
            FrmBlueToothCfg fb = new FrmBlueToothCfg();
            WorkCurveHelper.OpenUC(fb,false);
        }
    }


   

    [Serializable]
    public class FileAppSerialize
    {
        public string NodeName { get; set; }
        public string NodePath { get; set; }

        public FileAppSerialize(string nodeName, string nodePath)
        {
            this.NodeName = nodeName;
            this.NodePath = nodePath;
        }
    }
}
