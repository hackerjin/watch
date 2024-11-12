using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common;
using System.Text.RegularExpressions;
using Lephone.Data.Common;
using Skyray.EDXRFLibrary;
using Skyray.EDXRFLibrary.Define;
using Skyray.Controls;
using System.Xml.Linq;
using Skyray.EDX.Common.ReportHelper;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using Skyray.EDXRFLibrary.Spectrum;
using System.Threading;
using System.Xml;
using Lephone.Data.Common;
using Lephone.Data.Definition;
using Skyray.EDX.Common.Component;

namespace Skyray.UC
{
    /// <summary>
    /// 开始扫描类对象
    /// </summary>
    public partial class NewWorkSpec : Skyray.Language.UCMultiple
    {
        /// <summary>
        /// 工作曲线
        /// </summary>
        private DbObjectList<WorkCurve> lstWorkCurve;

        //private bool HasWeight = false;
        //private bool HasBarcode = false;
        /// <summary>
        /// 匹配设置是模式索引
        /// </summary>
        public int modelTool;

        public static NewWorkSpec signObject;

        public bool StartType;

        public TestDevicePassedParams TestDevicePassedParams;

        public SpecType SpecType;

        /// <summary>
        /// 根据设备的样腔动态生成控件的间隔
        /// </summary>
        public const int RowInternal = 25;

        /// <summary>
        /// 动态生成控件控件的名称
        /// </summary>
        public const string controlName = "controlName";

        /// <summary>
        /// 保存样品腔的相关信息字典
        /// </summary>
        private Dictionary<string, WordCureTest> listWorkCurveTest;

        // private string sampleNameFromTime = "";
        private string sampleNameFromAppXml = "";

        private DbObjectList<CustomStandard> lstStandard;

        DbObjectList<Supplier> lstSupplier;

        private List<string> SampleNameList = new List<string>();
        //  private List<CompanyOthersInfo> listOtherInfo;  //其它信息集合
        //public double BarWeight 
        //{
        //    set 
        //    {
        //        if (value != null)
        //        {
        //            this.txtWeight.Text = value.ToString();
        //            HasWeight = true;
        //        }
        //    }
        //}

        //public string BarSampleName
        //{
        //    set 
        //    {
        //        if (value != null)
        //        {
        //            this.textBoxWSampleName.Text = value.ToString();
        //            HasBarcode = true;
        //        }
        //    }
        //}

        private string GetDefineSampleName(string samplename)
        {
            string str = string.Empty;
            string splitStr = string.Empty;
            if (radioButtonStandand.Checked)
            {
                str = Info.StandandSample;
            }
            if (radioButtonUnknown.Checked)
            {
                str = Info.Test;
            }
            str = Info.Test;
            splitStr = DifferenceDevice.interClassMain.currentSelectMode == 0 ? Info.NormalMode : Info.Intelligent;

            //string samplename = string.Empty;
            try
            {
                string otherpath = Application.StartupPath + "\\AppParams.xml";
                XElement xele = XElement.Load(otherpath);
                if (xele == null) return str;
                var names = xele.Elements("NameSetting").Elements("Name").ToList();
                for (int i = 0; i < names.Count; i++)
                {
                    if (names[i].Attribute(XName.Get("Flag")).Value.ToString() == "SampleName")
                    {
                        string[] judge = names[i].Value.ToString().Split(("_").ToCharArray());
                        if (judge != null && judge.Count() > 0)
                        {
                            for (int j = 0; j < judge.Count(); j++)
                            {
                                switch (judge[j])
                                {
                                    case "bSpecListName":
                                        samplename += "_";
                                        break;
                                    case "bSpecType":
                                        samplename += (str + "_");
                                        break;
                                    case "bInitConditionName":
                                        samplename += (DifferenceDevice.interClassMain.initParams.Condition.Name + "_");
                                        break;
                                    case "bCurrentWorkCurve":
                                        samplename += (WorkCurveHelper.WorkCurveCurrent.Name + "_");
                                        break;
                                    case "bOptMode":
                                        samplename += (splitStr + "_");
                                        break;
                                    case "bTestTime":
                                        samplename += (System.DateTime.Now.ToString("HHmmss") + "_");
                                        break;
                                    case "bTestDate":
                                        samplename += (System.DateTime.Now.ToString("yyyyMMdd") + "_");
                                        break;
                                    case "bWorks":
                                        samplename += (WorkCurveHelper.WorkCurveCurrent.WorkRegion.Name + "_");
                                        break;
                                    case "bCollimator":
                                        samplename += (WorkCurveHelper.WorkCurveCurrent.Condition == null
                                                        || WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList == null
                                                        || WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList.Count <= 0
                                                        || !WorkCurveHelper.DeviceCurrent.HasCollimator
                                                        || WorkCurveHelper.DeviceCurrent.Collimators.Count < WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList[0].CollimatorIdx ? "" : WorkCurveHelper.DeviceCurrent.Collimators[WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList[0].CollimatorIdx - 1].Diameter.ToString() + "mm_");
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        else
                        {
                            return str;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(samplename))
                {
                    samplename = samplename.Remove(samplename.Length - 1);
                }
                return samplename;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return String.Empty;
            }
        }

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public NewWorkSpec()
        {
            //listOtherInfo = CompanyOthersInfo.FindBySql("select * from companyothersinfo where 1=1 and Display =1 and ExcelModeType='" + ReportTemplateHelper.ExcelModeType.ToString() + "' ");

            InitializeComponent();

            if ((WorkCurveHelper.DeviceCurrent != null && !WorkCurveHelper.DeviceCurrent.HasChamber) || WorkCurveHelper.DeviceCurrent == null)
                grpCurve.Visible = false;
            if (WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasChamber)
            {
                if (WorkCurveHelper.DeviceCurrent.Chamber.Count == 0)
                {
                    grpCurve.Visible = false;
                }
                else
                {
                    grpSample.Visible = false;
                }
            }
            if (DifferenceDevice.IsThick)
                grpMatch.Visible = false;
            if (WorkCurveHelper.isShowEncoder)
            {
                lblHeght.Visible = true;
                nbtxtHeight.Visible = true;
            }

            //sampleNameFromTime = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            // sampleNameFromTime = GetDefineSampleName(string.Empty);
            if (this.grpCurve.Visible)
            {
                LayoutChamberSetting();
                this.grpMatch.Visible = false;
            }
            this.LoadComboboxInfo();
            this.listWorkCurveTest = new Dictionary<string, WordCureTest>();
            SerializeChamberData();
            int height = 0;
            foreach (Control control in this.Controls)
            {
                if (control.Visible && control.Name != "grouperSampleType")
                    height += control.Height;
            }
            if (WorkCurveHelper.WorkCurveCurrent != null && WorkCurveHelper.WorkCurveCurrent.ElementList != null && WorkCurveHelper.WorkCurveCurrent.ElementList.Items != null)
            {
                if (WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count > 5)
                {
                    groupStand.Height = groupStand.Height + 25;
                }
                else if (WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count <= 3)
                {
                    ucSingStand1.Location = new Point(127, 34);
                    ucSingStand2.Location = new Point(339, 34);
                }
            }
            else
            {
                groupStand.Visible = false;
            }
            this.Height = height + 20;
            this.numDropTime.Value = 5;

        }

        public NewWorkSpec(int mode, bool startType)
            : this(mode)
        {
            this.StartType = startType;
            this.chkManualTest.Visible = StartType && (WorkCurveHelper.DeviceFunctype == FuncType.Thick);
            this.chkManualTest.Checked = WorkCurveHelper.IsManualTest && grpTestParams.Visible && (WorkCurveHelper.DeviceFunctype == FuncType.Thick);//fpthick的手动测试
            if (!this.chkManualTest.Visible)
            {
                lblTestCount.Left = lblWsampleName.Left;
                numTestCount.Left = comboBoxElementName.Left;
                lblDropTime.Left = chkRuleName.Left;
                numDropTime.Left = chkAscend.Left;
            }

        }

        private void SerializeChamberData()
        {
            IFormatter formatter = new BinaryFormatter();
            string fileName = Application.StartupPath + "\\chamberSave";
            if (File.Exists(fileName))
            {
                using (FileStream _FileStream = new System.IO.FileStream(fileName,
                    System.IO.FileMode.Open,
                    System.IO.FileAccess.Read,
                    System.IO.FileShare.None
                    ))
                {
                    _FileStream.Position = 0;
                    _FileStream.Seek(0, SeekOrigin.Begin);
                    List<WordCureTestSerialize> serializeData = (List<WordCureTestSerialize>)formatter.Deserialize(_FileStream);
                    if (serializeData.Count == WorkCurveHelper.DeviceCurrent.Chamber.Count)
                        chkTotalSelected.Checked = true;
                    if (serializeData != null && serializeData.Count > 0)
                    {
                        foreach (WordCureTestSerialize temp in serializeData)
                        {
                            Control[] controls = grpCurve.Controls.Find(controlName + (int.Parse(temp.SerialNumber) - 1).ToString(), true);
                            if (controls != null && controls.Length > 0)
                            {
                                CheckBoxW chkCheck = (controls.First(wde => wde.GetType() == typeof(CheckBoxW))) as CheckBoxW;
                                chkCheck.CheckedChanged -= new EventHandler(checkBox_CheckedChanged);
                                chkCheck.Checked = true;
                                chkCheck.CheckedChanged += new EventHandler(checkBox_CheckedChanged);
                                TextBoxW texbox = (controls.First(wde => wde.GetType() == typeof(TextBoxW))) as TextBoxW;
                                texbox.Text = temp.sampleInfo.SampleName;

                                ComboBoxW combox = (controls.First(wde => wde.GetType() == typeof(ComboBoxW))) as ComboBoxW;
                                if (combox.Items.Count > 0 && combox.Items.IndexOf(temp.WordCurveName) != -1)
                                    combox.SelectedIndex = combox.Items.IndexOf(temp.WordCurveName);
                                else if (combox.Items.Count > 0)
                                    combox.SelectedIndex = 0;
                                WordCureTest testTemp = new WordCureTest(temp.SerialNumber, new SpecListEntity("", temp.sampleInfo.SampleName, 0.0, 0.0, temp.sampleInfo.Supplier, temp.sampleInfo.Weight, temp.sampleInfo.Shape, "", DateTime.Now, temp.sampleInfo.SpecSummary, this.SpecType, DifferenceDevice.DefaultSpecColor.ToArgb(), DifferenceDevice.DefaultSpecColor.ToArgb()), temp.WordCurveName, temp.WordCurveID);
                                testTemp.sampleInfo = temp.sampleInfo;

                                if (listWorkCurveTest.ContainsKey(texbox.Name)) listWorkCurveTest.Remove(texbox.Name);
                                listWorkCurveTest.Add(texbox.Name, testTemp);
                            }
                        }
                    }
                }
            }
        }


        public NewWorkSpec(int mode) :
            this()
        {
            this.modelTool = mode;
            StartType = true;
            try
            {
                XmlDocument doc = new XmlDocument();
                string path = Application.StartupPath + "\\AppParams.xml";
                if (File.Exists(path))
                {
                    doc.Load(path);
                    XmlNodeList nodeList = doc.SelectNodes("application/TestParams");
                    foreach (XmlNode node in nodeList)
                    {
                        //增加用户选择是否要显示开盒测试 By Sunjian 2012/10/9
                        if (node.SelectSingleNode("AllowOpenCover") != null)
                        {
                            string allowOpenCover = node.SelectSingleNode("AllowOpenCover").InnerText;
                            if (allowOpenCover.ToUpper().Equals("TRUE"))
                            {
                                if (GP.CurrentUser.Role.RoleType.ToString() == "0")
                                {
                                    this.panCover.Visible = true;
                                    this.radIsAllowOpenCover.Checked = WorkCurveHelper.DeviceCurrent.IsAllowOpenCover;
                                    this.radIsAllowOpenCover2.Checked = !WorkCurveHelper.DeviceCurrent.IsAllowOpenCover;
                                }
                                else
                                {
                                    allowOpenCover = node.SelectSingleNode("AllowOpenCoverComman").InnerText;
                                    if (allowOpenCover.Equals("0"))
                                    {
                                        this.panCover.Visible = true;
                                        this.radIsAllowOpenCover.Checked = WorkCurveHelper.DeviceCurrent.IsAllowOpenCover;
                                        this.radIsAllowOpenCover2.Checked = !WorkCurveHelper.DeviceCurrent.IsAllowOpenCover;
                                    }
                                    else
                                    {
                                        this.panCover.Visible = false;
                                    }
                                }
                            }
                            else
                            {
                                this.panCover.Visible = false;
                            }
                        }
                        else
                        {
                            this.panCover.Visible = false;
                        }

                        //增加是否显示打开曲线和比对谱快捷 By Strong 2013-3-16
                        if (node.SelectSingleNode("AllowShowCurve") != null)
                        {
                            string allowShowCurve = node.SelectSingleNode("AllowShowCurve").InnerText;
                            if (allowShowCurve.ToUpper().Equals("TRUE"))
                            {
                                this.panChooseCurve.Visible = true;
                            }
                            else
                            {
                                this.panChooseCurve.Visible = false;
                            }
                        }
                        else
                        {
                            this.panChooseCurve.Visible = false;
                        }
                        //
                        uint type = 2;
                        string specType = node.SelectSingleNode("SpecType").InnerText;
                        type = uint.Parse(specType);
                        SetSpecType(type);
                        string sampleName = node.SelectSingleNode("SampleName").InnerText;
                        // sampleNameFromTime = GetDefineSampleName(sampleName);
                        //if (!sampleName.IsNullOrEmpty())
                        //{
                        //    if (type == 0 || type == 2)
                        //        this.textBoxWSampleName.Text = sampleNameFromTime;
                        //    else
                        //        this.comboBoxElementName.Text = sampleName;
                        //}
                        this.comboBoxElementName.Text = sampleName;
                        sampleNameFromAppXml = sampleName;
                        string weight = node.SelectSingleNode("Weight").InnerText;
                        if (!weight.IsNullOrEmpty())
                            this.txtWeight.Text = weight;
                        string shape = node.SelectSingleNode("Shape").InnerText;
                        if (!shape.IsNullOrEmpty() && Shape.FindOne(w => w.Name.ToLower() == shape.ToLower()) != null)
                            this.comboBoxWShape.Text = shape;
                        XmlNode xmlnode = node.SelectSingleNode("OperMatch");
                        string operMatch = xmlnode == null ? "0" : xmlnode.InnerText;
                        int OpMat = 0;
                        if (!operMatch.IsNullOrEmpty())
                            OpMat = int.Parse(operMatch);
                        if (OpMat == 0 && GP.CurrentUser.Role.RoleType != 0)
                        {
                            chkRuleName.Enabled = false;
                            //grpMatch.Visible = false;                              //yuzhao20131030:开放镀铑功能
                        }
                        string supplier = node.SelectSingleNode("Supplier").InnerText;
                        if (!supplier.IsNullOrEmpty() && Supplier.FindOne(w => w.Name.ToLower() == supplier.ToLower()) != null)
                            this.comboBoxWSupplier.Text = supplier;
                        string desc = node.SelectSingleNode("Description").InnerText;
                        if (!desc.IsNullOrEmpty())
                            this.txtDescription.Text = desc;
                        string times = node.SelectSingleNode("MeasureTime").InnerText;
                        if (!times.IsNullOrEmpty() && this.numTestCount.Enabled)
                            this.numTestCount.Value = int.Parse(times);
                        string dropTime = node.SelectSingleNode("DropTime").InnerText;
                        if (!dropTime.IsNullOrEmpty())
                            this.numDropTime.Value = int.Parse(dropTime);
                        string isShowLoss = node.SelectSingleNode("IsShowLoss").InnerText;
                        if (!string.IsNullOrEmpty(isShowLoss) && int.Parse(isShowLoss) == 0)
                            this.panel7.Visible = false;
                        else if (!string.IsNullOrEmpty(isShowLoss) && int.Parse(isShowLoss) == 1)
                        {
                            this.panel7.Visible = true;
                            if (WorkCurveHelper.WorkCurveCurrent.ElementList != null
                                && !WorkCurveHelper.WorkCurveCurrent.ElementList.IsUnitary) this.txtLoss.Enabled = false;
                        }
                        string IsMatch = node.SelectSingleNode("IsMatch").InnerText;
                        if (!IsMatch.IsNullOrEmpty() && !DifferenceDevice.IsThick)
                            this.chkMatch.Checked = bool.Parse(IsMatch);
                        string IsRhShow = node.SelectSingleNode("IsShowRh").InnerText;


                        if (DifferenceDevice.IsAnalyser || (!string.IsNullOrEmpty(IsRhShow) && IsRhShow.Trim() == "1"))
                        {
                            this.chkRhThick.Visible = true;

                            ElementList currElementList = ElementList.FindById((WorkCurveHelper.WorkCurveCurrent == null || WorkCurveHelper.WorkCurveCurrent.ElementList == null) ? -1 : WorkCurveHelper.WorkCurveCurrent.ElementList.Id);
                            this.chkRhThick.Checked = (WorkCurveHelper.WorkCurveCurrent == null || WorkCurveHelper.WorkCurveCurrent.ElementList == null || currElementList == null) ? false : currElementList.RhIsLayer;


                            if (this.chkRhThick.Checked && WorkCurveHelper.WorkCurveCurrent.CalcType != CalcType.FP && !WorkCurveHelper.WorkCurveCurrent.ElementList.RhIsMainElementInfluence)
                            {
                                this.txtRhfactor.Visible = true;
                                this.txtRhfactor.Text = currElementList.RhLayerFactor.ToString();
                            }
                            else
                            {
                                this.txtRhfactor.Visible = false;
                                this.txtRhfactor.Text = "";
                            }
                        }
                        else
                        {
                            this.chkRhThick.Visible = false;
                            this.txtRhfactor.Visible = false;
                        }


                        string IsRuleName = node.SelectSingleNode("IsRuleName").InnerText;
                        if (!IsRuleName.IsNullOrEmpty())
                        {
                            if (WorkCurveHelper.DeviceCurrent.HasChamber) this.chkMuliRuleName.Checked = bool.Parse(IsRuleName);
                            else
                            {
                                this.chkRuleName.Checked = bool.Parse(IsRuleName);
                                SetTxtSampleName(textBoxWSampleName, "textBox", chkRuleName);
                            }
                        }
                        string IsAscendName = node.SelectSingleNode("IsAscendName") == null ? "false" : node.SelectSingleNode("IsAscendName").InnerText;
                        if (bool.Parse(IsAscendName))
                        {
                            chkAscend.Checked = true;
                            string AscendScalar = node.SelectSingleNode("AscendScalar") == null ? "1" : node.SelectSingleNode("AscendScalar").InnerText;
                            numAscend.Text = AscendScalar;
                        }
                        chkAutoUploadData.Checked = WorkCurveHelper.IsAutoUpload;
                    }
                }
            }
            catch (Exception e)
            {
                Msg.Show(e.Message);
            }

            var vv = User.Find(w => w.Name == FrmLogon.userName);
            if (vv.Count > 0 && vv[0].Role.RoleType == 2)
            {
                this.panel2.Visible = false;
                if (!DifferenceDevice.IsAnalyser)
                    this.panel3.Visible = false;
                //this.numTestCount.Value = 1;
                this.radioButtonUnknown.Checked = true;
            }
            else if (mode != 1)
            {
                this.radioButtonStandand.Enabled = true;
                this.radioButtonPureElement.Enabled = true;
            }
            else if (mode == 1)
            {
                this.radioButtonUnknown.Checked = true;
            }

            this.chkManualTest.Visible = StartType && (WorkCurveHelper.DeviceFunctype == FuncType.Thick);
            this.chkManualTest.Checked = WorkCurveHelper.IsManualTest && grpTestParams.Visible && (WorkCurveHelper.DeviceFunctype == FuncType.Thick);//fpthick的手动测试
            this.chkManualTest.Enabled = this.radioButtonUnknown.Checked;
            if (!this.chkManualTest.Visible)
            {
                lblTestCount.Left = lblWsampleName.Left;
                numTestCount.Left = comboBoxElementName.Left;
                lblDropTime.Left = chkRuleName.Left;
                numDropTime.Left = chkAscend.Left;
            }
            if (!WorkCurveHelper.IsShowPureSpec)
            {
                panel3.Visible = false;
            }

            //如果当前是modbus操作
            if (WorkCurveHelper.startDoing == 1)
            {
                textBoxWSampleName.Text = ReportTemplateHelper.LoadSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/modbusNo");
                buttonWSubmit_Click(null, null);
                ExcuteEndProcess(null);

            }
        }

        private void SetStandValue()
        {
            //if (WorkCurveHelper.CurrentStandard != null)
            //{
            //    this.cmbSelectStand.SelectedText = WorkCurveHelper.CurrentStandard.StandardName;
            //}
            if (radioButtonUnknown.Checked)
            {
                //foreach(var elem in WorkCurveHelper.CurrentStandard.StandardDatas)
                //{ 
                //   elem.Id
                //}
                //StandardData standard = WorkCurveHelper.CurrentStandard;
                //if (standard != null && standard.StandardDatas != null && standard.StandardDatas.Count > 0)
                //{
                //    StandardData standSample = standard.StandardDatas.ToList<StandardData>().Find(delegate(StandardData w)
                //    {
                //        return string.Compare(w.ElementCaption, hiselement.elementName, true) == 0;
                //    });
                //    if (standSample != null)
                //    {

                //        if (standSample.StandardThick <= elemThickvalue && elemThickvalue <= standSample.StandardThickMax)
                //        {
                //            sJudgeValue = strPass;
                //        }
                //        else
                //        {
                //            sJudgeValue = strFalse;

                //        }
                //    }
                //}
            }



        }

        public static NewWorkSpec GetInstance(int model)
        {
            if (signObject == null || WorkCurveHelper.startDoing == 1)
                signObject = new NewWorkSpec(model);
            return signObject;
        }


        private void chkTotalSelected_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Control controlTemp in grpCurve.Controls)
            {
                if (controlTemp.GetType() == typeof(CheckBoxW))
                {
                    CheckBoxW checKtemp = controlTemp as CheckBoxW;
                    checKtemp.Checked = this.chkTotalSelected.Checked;
                }
            }
        }


        /// <summary>
        /// 根据设备样品腔设置的数目自动生成控件
        /// </summary>
        private void LayoutChamberSetting()
        {
            List<Chamber> listChamber = WorkCurveHelper.DeviceCurrent.Chamber.ToList();
            if (listChamber == null || listChamber.Count == 0)
                return;
            Point numberPosition = new Point(27, 51);
            Point samplePosition = new Point(107, 51);
            Point wordCurvePosition = new Point(267, 51);
            List<WorkCurve> listCurves = WorkCurve.FindBySql(@"select * from WorkCurve as w left join Condition as c on w.Condition_Id = c.Id left join Device as d on c.Device_Id = d.Id 
            where d.Id =" + WorkCurveHelper.DeviceCurrent.Id + " and c.Type=" + (int)ConditionType.Normal);
            Size checkSize = new Size(36, 16);
            Size txtBoxSize = new Size(120, 21);
            Size comBoxSize = new Size(121, 22);

            for (int i = 0; i < listChamber.Count; i++)
            {
                CheckBoxW checkBox = new CheckBoxW();
                checkBox.CheckedChanged += new EventHandler(checkBox_CheckedChanged);
                checkBox.Text = listChamber[i].Num.ToString();
                checkBox.Name = controlName + i;
                checkBox.Location = new Point(numberPosition.X, numberPosition.Y + RowInternal);
                checkBox.Size = checkSize;
                checkBox.Visible = true;
                grpCurve.Controls.Add(checkBox);
                numberPosition = checkBox.Location;

                TextBoxW textBox = new TextBoxW();
                textBox.Click += new System.EventHandler(this.textBox_Click);
                textBox.Location = new Point(samplePosition.X, samplePosition.Y + RowInternal);
                textBox.Name = controlName + i;
                textBox.Size = txtBoxSize;
                textBox.Visible = true;
                textBox.ReadOnly = true;
                grpCurve.Controls.Add(textBox);
                samplePosition = textBox.Location;

                ComboBoxW combox = new ComboBoxW();
                combox.Location = new Point(wordCurvePosition.X, wordCurvePosition.Y + RowInternal);
                combox.Size = comBoxSize;
                combox.Name = controlName + i;

                combox.Visible = true;
                grpCurve.Controls.Add(combox);
                wordCurvePosition = combox.Location;
                foreach (WorkCurve temp in listCurves)
                    combox.Items.Add(temp.Name);
                //修改：何晓明 20110712 选中当前曲线
                //if (combox.Items.Count>0)
                //    combox.SelectedIndex = 0;
                if (combox.Items.Count > 0 && WorkCurveHelper.WorkCurveCurrent != null && combox.Items.IndexOf(WorkCurveHelper.WorkCurveCurrent.Name.ToString()) != -1)
                    combox.SelectedIndex = combox.Items.IndexOf(WorkCurveHelper.WorkCurveCurrent.Name.ToString());
                else if (combox.Items.Count > 0)
                    combox.SelectedIndex = 0;

                grpCurve.Size = new Size(grpCurve.Size.Width, grpCurve.Size.Height + RowInternal);
            }
        }
        /// <summary>
        /// 添加样品腔默认的标样名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBoxW checkbox = sender as CheckBoxW;
            if (checkbox.Checked)
            {
                Control[] controls = this.Controls.Find(controlName + (int.Parse(checkbox.Text) - 1).ToString(), true);
                TextBoxW texbox = (controls.First(wde => wde.GetType() == typeof(TextBoxW))) as TextBoxW;
                if (texbox.Text.Equals(string.Empty) && !radioButtonPureElement.Checked)
                {
                    //texbox.Text = sampleNameFromTime + "#" + checkbox.Text;
                    texbox.Text = sampleNameFromAppXml + "#" + checkbox.Text;
                }
                WordCureTest test = new WordCureTest();
                test.sampleInfo = new SampleInfo();
                test.sampleInfo.SampleName = texbox.Text;
                if (this.listWorkCurveTest.ContainsKey(texbox.Name))
                    this.listWorkCurveTest.Remove(texbox.Name);
                this.listWorkCurveTest.Add(texbox.Name, test);
            }
            else
            {
                Control[] controls = this.Controls.Find(controlName + (int.Parse(checkbox.Text) - 1).ToString(), true);
                TextBoxW texbox = (controls.First(wde => wde.GetType() == typeof(TextBoxW))) as TextBoxW;
                texbox.Text = "";
                this.listWorkCurveTest.Remove(texbox.Name);
            }
        }
        /// <summary>
        /// 对于多样品腔情况下设置该样品的样品信息等
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_Click(object sender, EventArgs e)
        {
            TextBoxW texbox = sender as TextBoxW;
            FrmSampleInfo frmSampleInfo = new FrmSampleInfo(this.SpecType);

            //获取字典中的当前工作曲线信息
            if (listWorkCurveTest.ContainsKey(texbox.Name))
            {
                WordCureTest wordCureTest = listWorkCurveTest[texbox.Name];
                if (wordCureTest.sampleInfo != null)
                {
                    frmSampleInfo.SampleName = (wordCureTest.sampleInfo.SampleName == null) ? "" : wordCureTest.sampleInfo.SampleName;
                    frmSampleInfo.SampleShape = (wordCureTest.sampleInfo.Shape == null) ? "" : wordCureTest.sampleInfo.Shape;
                    frmSampleInfo.SampleSupplier = (wordCureTest.sampleInfo.Supplier == null) ? "" : wordCureTest.sampleInfo.Supplier;
                    frmSampleInfo.Weight = wordCureTest.sampleInfo.Weight.ToString();
                    frmSampleInfo.Loss = wordCureTest.sampleInfo.Loss.ToString();
                    frmSampleInfo.SpecSummary = (wordCureTest.sampleInfo.SpecSummary == null) ? "" : wordCureTest.sampleInfo.SpecSummary;
                }
                if (wordCureTest.CompanyInfoList != null)
                {
                    frmSampleInfo.CompanyInfoList = wordCureTest.CompanyInfoList;
                }
            }
            if (frmSampleInfo.CompanyInfoList == null || frmSampleInfo.CompanyInfoList.Count <= 0)
            {
                frmSampleInfo.CompanyInfoList = CompanyOthersInfo.FindBySql("select * from companyothersinfo where  Display =1 and ExcelModeType='" + ReportTemplateHelper.ExcelModeType.ToString() + "'");
                foreach (var name in frmSampleInfo.CompanyInfoList)
                {
                    name.DefaultValue = "";
                }

            }
            frmSampleInfo.SampleName = texbox.Text;
            if (DialogResult.OK == frmSampleInfo.ShowDialog())
            {
                SampleInfo sampInfo = frmSampleInfo.SampleInfo;
                foreach (KeyValuePair<string, WordCureTest> pair in listWorkCurveTest)
                {
                    //modify by chuyaqin 2011-04-15 样品名重名
                    if (pair.Value.sampleInfo.SampleName.Equals(sampInfo.SampleName) && !pair.Key.Equals(texbox.Name))
                    {
                        Msg.Show(Info.SilimarSampleInfo);
                        return;
                    }
                }
                texbox.Text = sampInfo.SampleName;
                WordCureTest test = new WordCureTest();
                test.sampleInfo = sampInfo;
                if (this.listWorkCurveTest.ContainsKey(texbox.Name))
                    this.listWorkCurveTest.Remove(texbox.Name);
                test.CompanyInfoList = frmSampleInfo.CompanyInfoList;
                this.listWorkCurveTest.Add(texbox.Name, test);
            }
            //add by chuyaqin 选择样品腔的时候填入样品信息点击取消时，界面置空，且删除曲线信息
            else
            {
                texbox.Text = string.Empty;
                this.listWorkCurveTest.Remove(texbox.Name);
            }
        }

        /// <summary>
        /// 根据目前样品腔的设置保存，并对谱列表进行设置
        /// </summary>
        /// <param name="name">文本框的名称</param>
        /// <param name="test">当前样品腔测试保存对象</param>
        /// <param name="sampInfo">当前样品腔设置的样品信息</param>
        private bool WorkCurveInfoValue(string name, WordCureTest test, SampleInfo sampInfo)
        {
            Control[] controls = this.grpCurve.Controls.Find(name, true);
            if ((controls == null) || controls.Length == 0)
                return false;
            CheckBoxW checkBoxControl = null;
            ComboBoxW comboxCurve = null;
            foreach (Control controlTemp in controls)
            {
                if (controlTemp.GetType() == typeof(CheckBoxW))
                {
                    CheckBoxW checKtemp = controlTemp as CheckBoxW;
                    if (checKtemp.Checked)
                        checkBoxControl = checKtemp;
                }
                else if (controlTemp.GetType() == typeof(ComboBoxW))
                {
                    comboxCurve = controlTemp as ComboBoxW;
                }
            }
            if (checkBoxControl == null)
            {
                //Msg.Show(Info.CheckBoxSelect);
                return false;
            }
            if (comboxCurve == null)
            {
                //Msg.Show(Info.ComboBoxSelect);
                return false;
            }
            test.SerialNumber = checkBoxControl.Text;
            test.Spec = new SpecListEntity("", sampInfo.SampleName, 0.0, 0.0, sampInfo.Supplier, sampInfo.Weight, sampInfo.Shape, "", DateTime.Now, sampInfo.SpecSummary, this.SpecType, DifferenceDevice.DefaultSpecColor.ToArgb(), DifferenceDevice.DefaultSpecColor.ToArgb());
            //样品腔添加烧矢量
            if (DifferenceDevice.IsXRF) test.Spec.Loss = sampInfo.Loss;
            test.WordCurveName = comboxCurve.Text;
            return true;
        }


        /// <summary>
        /// 对combox控件进行充值
        /// </summary>
        private void LoadComboboxInfo()
        {
            this.comboBoxWShape.Items.Clear();
            var list = from item in Shape.FindAll() select item.Name;
            this.comboBoxWShape.DataSource = list.ToList();
            this.comboBoxWShape.AutoCompleteMode = AutoCompleteMode.Suggest;
            this.comboBoxWShape.AutoCompleteSource = AutoCompleteSource.ListItems;
            if (this.comboBoxWShape.Items.Count > 0)
                this.comboBoxWShape.SelectedIndex = 0;



            this.comboBoxWSupplier.Items.Clear();
            var listSuppier = from item in Supplier.FindAll() select item.Name;
            this.comboBoxWSupplier.DataSource = listSuppier.ToList();
            this.comboBoxWSupplier.AutoCompleteMode = AutoCompleteMode.Suggest;
            this.comboBoxWSupplier.AutoCompleteSource = AutoCompleteSource.ListItems;
            if (this.comboBoxWSupplier.Items.Count > 0)
                this.comboBoxWSupplier.SelectedIndex = 0;

            this.numTestCount.Value = 1;

            this.comboBoxElementName.Items.Clear();
            var listElementName = from item in Atoms.AtomList select item.AtomName;
            //DbObjectList<DefinePureElement> lstDefinePureElement = DefinePureElement.FindAll();
            var listDefineElementName = from itemE in DefinePureElement.FindAll() select itemE.Name;
            if (listDefineElementName != null)
                listElementName = listElementName.Union(listDefineElementName);

            this.comboBoxElementName.DataSource = listElementName.ToList();
            this.comboBoxElementName.AutoCompleteMode = AutoCompleteMode.Suggest;
            this.comboBoxElementName.AutoCompleteSource = AutoCompleteSource.ListItems;
            if (this.comboBoxElementName.Items.Count > 0)
                this.comboBoxElementName.SelectedIndex = 0;

            this.comboBoxWChooseCurve.Items.Clear();
            if (WorkCurveHelper.DeviceCurrent != null)
            {
                var deviceId = WorkCurveHelper.DeviceCurrent.Id;
                string sql = "select * from WorkCurve where Condition_Id in (select Id from condition where Type = 0 and Device_id = "
                    + deviceId + ") and FuncType=" + (int)WorkCurveHelper.DeviceFunctype + " order by LOWER(Name)";

                lstWorkCurve = WorkCurve.FindBySql(sql);
            }
            else
            {
                lstWorkCurve = WorkCurve.FindAll();
            }

            for (int i = 0; i < lstWorkCurve.Count; i++)
            {
                this.comboBoxWChooseCurve.Items.Add(lstWorkCurve[i].Name);
            }

            if (WorkCurveHelper.WorkCurveCurrent != null)
            {
                this.comboBoxWChooseCurve.SelectedText = WorkCurveHelper.WorkCurveCurrent.Name;
            }

            this.comboBoxWChooseCurve.AutoCompleteMode = AutoCompleteMode.Suggest;
            this.comboBoxWChooseCurve.AutoCompleteSource = AutoCompleteSource.ListItems;

            lstStandard = CustomStandard.FindAll();//获取所有标准
            this.cmbSelectStand.Items.Clear();
            if (lstStandard != null && lstStandard.Count > 0)
            {
                foreach (var s in lstStandard)
                {
                    this.cmbSelectStand.Items.Add(s.StandardName);
                }
            }

            if (WorkCurveHelper.CurrentStandard != null)
            {

                this.cmbSelectStand.SelectedText = WorkCurveHelper.CurrentStandard.StandardName;
                for (int i = 0; i < lstStandard.Count; i++)
                {
                    if (WorkCurveHelper.CurrentStandard.StandardName.Equals(lstStandard[i].StandardName))
                    {
                        this.cmbSelectStand.SelectedIndex = i;
                        break;
                    }
                }
                HasMany<CurveElement> elements = WorkCurveHelper.WorkCurveCurrent.ElementList.Items;
                for (int i = 0; i < elements.Count; i++)
                {
                    if (elements[i].LayerNumber != elements.Count)
                    {
                        Control[] controls = groupStand.Controls.Find("ucSingStand" + elements[i].LayerNumber.ToString(), true);
                        if (controls != null)
                        {
                            controls[0] = controls[0] as UCSingStand;
                            controls[0].Visible = true;
                            ((UCSingStand)controls[0]).LayerName = ShowfloorName(elements[i]);//elements[i].Caption;
                            ((UCSingStand)controls[0]).SName = elements[i].Caption;
                            StandardData OneElemData = WorkCurveHelper.CurrentStandard.StandardDatas.ToList<StandardData>().Find(w => w.ElementCaption == elements[i].Caption);
                            if (OneElemData != null)
                            {
                                ((UCSingStand)controls[0]).StandMin = OneElemData.StandardThick;
                                ((UCSingStand)controls[0]).StandMax = OneElemData.StandardThickMax;
                            }
                            else
                            {
                                ((UCSingStand)controls[0]).StandMin = 0;
                                ((UCSingStand)controls[0]).StandMax = 100;
                            }
                        }
                    }
                }
            }

        }

        private string ShowfloorName(CurveElement element)
        {
            if (element.LayerNumBackUp == "基材")
            {
                element.LayerNumBackUp = Info.Substrate;
            }
            else if (element.LayerNumBackUp == "第一层")
            {
                element.LayerNumBackUp = Info.FirstLayer;
            }
            else if (element.LayerNumBackUp == "第二层")
            {
                element.LayerNumBackUp = Info.SecondLayer;
            }
            else if (element.LayerNumBackUp == "第三层")
            {
                element.LayerNumBackUp = Info.ThirdLayer;
            }
            else if (element.LayerNumBackUp == "第四层")
            {
                element.LayerNumBackUp = Info.ForthLayer;
            }
            else if (element.LayerNumBackUp == "第五层")
            {
                element.LayerNumBackUp = Info.FifthLayer;
            }
            return element.LayerNumBackUp;
        }

        /// <summary>
        /// 得到工作谱信息
        /// </summary>
        /// <returns></returns>
        private bool GetSpecFromInputInfor()
        {
            if (textBoxWSampleName.Visible && textBoxWSampleName.Text.Trim() == "")
            {
                Msg.Show(Info.TestSampleNameValidate, Info.SelectSampleType, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ///this.textBoxWSampleName.Focus();
                return false;
            }

            string sampleName = textBoxWSampleName.Text.Trim();
            if (chkRuleName.Checked) sampleName = GetDefineSampleName(sampleName);
            double weight = double.Epsilon;
            try
            {
                if (this.txtWeight.Text == "")
                    weight = 0.00;
                else
                    weight = double.Parse(this.txtWeight.Text);
            }
            catch
            {
                Msg.Show(Info.PleaseCorrectWeight);
                this.txtWeight.Focus();
                return false;
            }

            InterfaceClass.selePrintObjectL.Clear();

            List<string> specListNameList = new List<string>();
            specListNameList.Add(sampleName);
            double Height = 0;
            double.TryParse(nbtxtHeight.Text, out Height);
            //if (!SpeclistNameValidate(specListNameList)) return false;


            //SpecListEntity specList = new SpecListEntity(sampleName, sampleName,
            //    this.comboBoxWSupplier.SelectedItem == null ? "" : this.comboBoxWSupplier.SelectedItem.ToString(), weight,
            //    this.comboBoxWShape.SelectedItem == null ? "" : this.comboBoxWShape.SelectedItem.ToString(), FrmLogon.userName, DateTime.Now, this.txtDescription.Text.ToString(), this.SpecType, DifferenceDevice.DefaultSpecColor.ToArgb(), DifferenceDevice.DefaultSpecColor.ToArgb());
            SpecListEntity specList = new SpecListEntity(sampleName, sampleName, Height, DifferenceDevice.interClassMain.deviceMeasure.interfacce.ReturnEncoderHeight,
              this.comboBoxWSupplier.Text == null ? "" : this.comboBoxWSupplier.Text.ToString(), weight,
              this.comboBoxWShape.SelectedItem == null ? "" : this.comboBoxWShape.SelectedItem.ToString(), FrmLogon.userName, DateTime.Now, this.txtDescription.Text.ToString(), this.SpecType, DifferenceDevice.DefaultSpecColor.ToArgb(), DifferenceDevice.DefaultSpecColor.ToArgb());


            //烧失量控制
            double iloss = (DifferenceDevice.IsXRF && txtLoss.Text != "") ? double.Parse(txtLoss.Text) : 0.00;
            specList.Loss = iloss;

            WordCureTest test = new WordCureTest();
            test.WordCurveName = WorkCurveHelper.WorkCurveCurrent.Name;
            test.WordCurveID = WorkCurveHelper.WorkCurveCurrent.Id.ToString();
            test.Spec = specList;
            test.SerialNumber = "0";
            test.CompanyInfoList = listOtherInfo = CompanyOthersInfo.FindBySql("select * from companyothersinfo where 1=1 and Display =1 and ExcelModeType='" + ReportTemplateHelper.ExcelModeType.ToString() + "' ");

            this.localWorkCurve.Add(test);


            return true;

        }

        //        private bool SpeclistNameValidate(List<string> specListNameList)
        //        {
        //            bool isSucceed = true; 
        //            //判断数据库中是否存在指定的样品名称
        //            bool isDeleSpecList = false;
        //            #region
        //            InterfaceClass.IsIncrement = false;
        //            bool isExitSameName=false;//是否存在相同的名称
        //            foreach (string strSpecName in specListNameList)
        //            {
        //                string stringSql = @"select a.samplename from SpecList a inner join Condition b 
        //                                on a.Condition_Id=b.Id inner join Device c on b.Device_Id=c.Id where 1=1  " + (((WorkCurveHelper.DeviceCurrent.HasChamber) ? chkMuliRuleName.Checked : chkRuleName.Checked) ? " and a.SampleName like '" + strSpecName + "%'" : " and a.name='" + strSpecName + "'") + "  and b.Device_Id="
        //                                     + WorkCurveHelper.DeviceCurrent.Id + " group by a.samplename";

        //                var testv = EDXRFHelper.GetData(stringSql);
        //                if (testv != null && testv.Rows.Count > 0) { isExitSameName = true; break; }
        //            }

        //            if (!isExitSameName) return true;


        //            var result = Msg.Show(Info.strCoverSpecName, Info.Suggestion, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //            if (result == DialogResult.Yes)
        //            {
        //                InterfaceClass.IsIncrement = true;
        //            }
        //            else if (result == DialogResult.No)
        //            {
        //                if (radioButtonPureElement.Checked || radioButtonStandand.Checked)
        //                {
        //                    UCPWDLock uc = new UCPWDLock(true);
        //                    WorkCurveHelper.OpenUC(uc, false, Info.PWDLock);
        //                    if (uc.DialogResult == DialogResult.No)
        //                    {
        //                        return false;
        //                    }
        //                }
        //                isDeleSpecList = true;
        //                InterfaceClass.IsIncrement = false;
        //            }
        //            else return false;


        //            #endregion

        //            #region 删除谱文件
        //            if (isDeleSpecList)
        //            {
        //                foreach (string strSpecName in specListNameList)
        //                {

        //                    string stringSql = @"select a.* from SpecList a inner join Condition b 
        //                                on a.Condition_Id=b.Id inner join Device c on b.Device_Id=c.Id where 1=1 " + (((WorkCurveHelper.DeviceCurrent.HasChamber)?chkMuliRuleName.Checked: chkRuleName.Checked) ? " and a.SampleName like '" + strSpecName + "%'" : " and a.name='" + strSpecName + "'") + " and b.Device_Id="
        //                                             + WorkCurveHelper.DeviceCurrent.Id;
        //                    var testv = EDXRFHelper.GetData(stringSql);
        //                    if (testv != null && testv.Rows.Count > 0)
        //                    {
        //                        //覆盖原有的谱文件和记录
        //                        string ssplistID = "";
        //                        foreach (DataRow row in testv.Rows)
        //                        {
        //                            ssplistID += row["Id"] + ",";
        //                            DirectoryInfo info = new DirectoryInfo(WorkCurveHelper.SaveSamplePath);
        //                            if (info.Exists)
        //                            {
        //                                FileInfo file = new FileInfo(WorkCurveHelper.SaveSamplePath + "\\" + row["Id"] + ".jpg");
        //                                if (file.Exists)
        //                                    file.Delete();
        //                            }
        //                            info = new DirectoryInfo(WorkCurveHelper.SaveGraphicPath);
        //                            if (info.Exists)
        //                            {
        //                                FileInfo file = new FileInfo(WorkCurveHelper.SaveGraphicPath + "\\" + row["Id"] + ".jpg");
        //                                if (file.Exists)
        //                                    file.Delete();
        //                            }
        //                        }
        //                        ssplistID = (ssplistID.Length > 0) ? ssplistID.Substring(0, ssplistID.Length - 1) : "";
        //                        if (ssplistID != "")
        //                        {
        //                            Lephone.Data.DbEntry.Context.ExecuteNonQuery("delete from historycompanyotherinfo where history_id in (select id from historyrecord where speclistId in (" + ssplistID + "));" +
        //                                "delete from historyelement where historyrecord_id in (select id from historyrecord where speclistId in (" + ssplistID + "));" +
        //                                "delete from historyrecord where speclistId in (" + ssplistID + ");" +
        //                                "delete from SpecList where  id in (" + ssplistID + "); delete from spec where  speclist_id in (" + ssplistID + ");");
        //                        }
        //                    }
        //                }
        //            }

        //            #endregion


        //            return isSucceed;
        //        }
        List<WordCureTest> localWorkCurve = new List<WordCureTest>();
        List<WordCureTestSerialize> serializeChamber = new List<WordCureTestSerialize>();
        /// <summary>
        /// 根据用户选择的状态保存相关信息
        /// </summary>
        //private bool GetWordCurveInfo()
        //{
        //    bool flag = false;
        //    foreach (Control controls in this.grpCurve.Controls)
        //    {
        //        if (controls.GetType() != typeof(CheckBoxW))
        //            continue;
        //        CheckBoxW checkBox = (CheckBoxW)controls;
        //        if (checkBox.Checked)
        //        {
        //            string tempName = checkBox.Name;
        //            WordCureTest testWord = new WordCureTest();
        //            this.listWorkCurveTest.TryGetValue(tempName, out testWord);
        //            Control[] textBox = this.grpCurve.Controls.Find(tempName, true);
        //            if ((textBox == null) || (textBox.Length == 0) || testWord == null)
        //                continue;
        //            foreach (Control control in textBox)
        //            {
        //                if (control.GetType() == typeof(ComboBoxW))
        //                {
        //                    ComboBoxW comboxW = (ComboBoxW)control;
        //                    if (comboxW.SelectedItem == null)
        //                    {
        //                        flag = false;
        //                        break;
        //                    }
        //                    testWord.WordCurveName = comboxW.SelectedItem.ToString();
        //                }
        //            }
        //            flag = true;
        //        }
        //    }
        //    return flag;
        //}


        private void SaveRhParamsValue()
        {
            if (WorkCurveHelper.WorkCurveCurrent != null && WorkCurveHelper.WorkCurveCurrent.ElementList != null)
            {

                //string sRhIsMainElementInfluence = ReportTemplateHelper.LoadSpecifiedValue("TestParams", "RhlayerOnlyMainElementInfluence");


                WorkCurveHelper.WorkCurveCurrent.ElementList.RhIsLayer = (this.chkRhThick.Visible && this.chkRhThick.Checked) ? true : false;
                WorkCurveHelper.WorkCurveCurrent.ElementList.RhLayerFactor = (WorkCurveHelper.WorkCurveCurrent.ElementList.RhIsLayer) ? double.Parse((this.txtRhfactor.Text == "") ? "0" : this.txtRhfactor.Text) : 0;
                WorkCurveHelper.WorkCurveCurrent.ElementList.RhLayerFactor = WorkCurveHelper.WorkCurveCurrent.ElementList.RhIsMainElementInfluence ? 1 : WorkCurveHelper.WorkCurveCurrent.ElementList.RhLayerFactor;
                WorkCurveHelper.WorkCurveCurrent.ElementList.LayerElemsInAnalyzer = this.txtLayerElems.Text.Trim();
                int Islayer = this.chkRhThick.Checked ? 1 : 0;
                string sql = "Update ElementList Set RhLayerFactor = " + WorkCurveHelper.WorkCurveCurrent.ElementList.RhLayerFactor + ", RhIsLayer = "
                    + Islayer + ",LayerElemsInAnalyzer= '" + WorkCurveHelper.WorkCurveCurrent.ElementList.LayerElemsInAnalyzer + "'  Where Id = " + WorkCurveHelper.WorkCurveCurrent.ElementList.Id;
                Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);

            }

            //if (this.chkRhThick.Visible&&this.txtRhfactor.Visible&&this.chkRhThick.Checked)
            //{
            //    try
            //    {
            //        double temp = double.Parse(this.txtRhfactor.Text);
            //        if (WorkCurveHelper.WorkCurveCurrent != null && WorkCurveHelper.WorkCurveCurrent.ElementList != null)
            //        {
            //            WorkCurveHelper.WorkCurveCurrent.ElementList.RhIsLayer = this.chkRhThick.Checked;
            //            WorkCurveHelper.WorkCurveCurrent.ElementList.RhLayerFactor = temp;
            //            int Islayer = this.chkRhThick.Checked ? 1 : 0;
            //            string sql = "Update ElementList Set RhLayerFactor = " + WorkCurveHelper.WorkCurveCurrent.ElementList.RhLayerFactor + ", RhIsLayer = "
            //                   + Islayer + " Where Id = " + WorkCurveHelper.WorkCurveCurrent.ElementList.Id;
            //            Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
            //            return true;
            //        }
            //    }
            //    catch
            //    {
            //        return false;
            //    }
            //}
            //return true;
        }

        /// <summary>
        /// 点击确定按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWSubmit_Click(object sender, EventArgs e)
        {
            #region 过滤样品名称中会导致命名文件失败的非法字符@20210406
            var inputText = this.textBoxWSampleName.Text;

            if (DifferenceDevice.CurCameraControl.skyrayCamera1.ContiTestPoints.Count == 0 && this.radioButtonUnknown.Checked)
            {
                if (inputText.Contains("#"))
                    inputText = inputText.Split(new char[] { '#' })[0] + "#" + DateTime.Now.ToString("yyyyMMddHHmmss").ToString();
                else
                    inputText = inputText + "#" + DateTime.Now.ToString("yyyyMMddHHmmss").ToString();

            }


            var invalidChars = Path.GetInvalidFileNameChars();
            for (int i = 0; i < invalidChars.Length; i++)
            {
                inputText = inputText.Replace(invalidChars[i].ToString(), string.Empty);
            }
            this.textBoxWSampleName.Text = inputText;
            #endregion

            
            List<CompanyOthersInfo> companyOthersInfo_List = CompanyOthersInfo.FindBySql("select * from companyothersinfo where  Display =1 and ExcelModeType='" + ReportTemplateHelper.ExcelModeType.ToString() + "'");
            if (companyOthersInfo_List != null && companyOthersInfo_List.Count > 0 && companyOthersInfo_List.Count <= 2)
            { OtherInfoOK(); }
            DifferenceDevice.interClassMain.TempIsKey = WorkCurveHelper.DirectRun.IsKeyCall;
            WorkCurveHelper.DirectRun.IsKeyCall = false;
            WorkCurveHelper.IsManualTest = (grpTestParams.Visible && chkManualTest.Checked && chkManualTest.Enabled && (WorkCurveHelper.DeviceFunctype == FuncType.Thick)) ? true : false;//2013-0409 
            if (!radioButtonPureElement.Checked && (!WorkCurveHelper.DeviceCurrent.HasChamber && !ValidateHelper.IllegalCheck(textBoxWSampleName))) return;
            //if (radioButtonUnknown.Checked && txtRhfactor.Visible==true && chkRhThick.Checked && txtRhfactor.Text != "" && double.Parse(txtRhfactor.Text) > 1)
            //{
            //    Msg.Show(Info.RhfactorErrorInfo);
            //    return;
            //}

            //增加是否开盒测试 by Sunjian 2012/10/9
            if (this.panCover.Visible)
            {
                if (radIsAllowOpenCover.Checked)
                {
                    WorkCurveHelper.DeviceCurrent.IsAllowOpenCover = true;
                    WorkCurveHelper.DeviceCurrent.Save();
                }
                else
                {
                    WorkCurveHelper.DeviceCurrent.IsAllowOpenCover = false;
                    WorkCurveHelper.DeviceCurrent.Save();
                }
            }

            //增加是否显示打开工作曲线 By Strong 2013-3-6
            if (this.panChooseCurve.Visible && (comboBoxWChooseCurve.SelectedIndex != -1))
            {
                WorkCurveHelper.WorkCurveCurrent = lstWorkCurve[comboBoxWChooseCurve.SelectedIndex];
                WorkCurveHelper.CalcType = lstWorkCurve[comboBoxWChooseCurve.SelectedIndex].CalcType;

                if (DifferenceDevice.interClassMain.skyrayCamera != null)
                {
                    DifferenceDevice.interClassMain.skyrayCamera.FociIndex = WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList[0].CollimatorIdx - 1;
                }
            }

            string strAdjust = ReportTemplateHelper.LoadSpecifiedValue("TestParams", "AdjustTime");
            //if (!string.IsNullOrEmpty(strAdjust) && this.numDropTime.Value >= int.Parse(strAdjust))
            //{
            //    return;
            //}

            if (txtRhfactor.Visible == true && chkRhThick.Checked && chkRhThick.Visible == true && (txtRhfactor.Text == "" || double.Parse(txtRhfactor.Text) > 1))
            {
                Msg.Show(Info.RhfactorErrorInfo);
                return;
            }
            bool IsAllowOpenCover = false;
            if (WorkCurveHelper.WorkCurveCurrent == null && this.modelTool == 0)
            {
                Msg.Show(Info.WarningTestContext, Info.TestWarning, MessageBoxButtons.OK, MessageBoxIcon.Information);
                EDXRFHelper.GotoMainPage(this);
                return;
            }
            if (SpecType == SpecType.UnSelected)
            {
                Msg.Show(Info.PleaseSelectSpecType);
                return;
            }
            if (numDropTime.Value > 0 && WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList.ToList().Exists(delegate(DeviceParameter v) { return v.PrecTime <= numDropTime.Value; }))
            {
                Msg.Show(Info.strDropTime);
                return;
            }

            if (DifferenceDevice.IsXRF && double.Parse((txtLoss.Text == "") ? "0" : txtLoss.Text) > 100)
            {
                Msg.Show(Info.strErrorLoss);
                return;
            }
            if (SpecType == SpecType.PureSpec && this.comboBoxElementName.Visible)
            {
                #region 不存在样品腔

                var listElementName = from item in Atoms.AtomList select item.AtomName;
                //DbObjectList<DefinePureElement> lstDefinePureElement = DefinePureElement.FindAll();
                var listDefineElementName = from itemE in DefinePureElement.FindAll() select itemE.Name;
                if (listDefineElementName != null)
                    listElementName = listElementName.Union(listDefineElementName);

                // var list = from atom in Atoms.AtomList where String.Compare(this.comboBoxElementName.Text, atom.AtomName, true) == 0 select atom;
                var isContainElem = listElementName.Contains(this.comboBoxElementName.Text);

                // if (list.Count() == 0)
                if (!isContainElem)
                {
                    Msg.Show(Info.SelectElementNotTable);
                    if (this.comboBoxElementName.Items.Count > 0)
                    {
                        this.comboBoxElementName.SelectedIndex = 0;
                        this.textBoxWSampleName.Text = this.comboBoxElementName.Text;
                    }
                    return;
                }
                else
                {
                    string txtStr = this.comboBoxElementName.Text;
                    if (txtStr.Length > 0 && !Char.IsUpper(txtStr, 0))
                    {
                        txtStr = txtStr.Substring(0, 1).ToUpper() + txtStr.Substring(1);

                    }
                    if (txtStr.Length > 0)
                    {
                        this.comboBoxElementName.Text = txtStr;
                        if (WorkCurveHelper.isShowEncoder)
                            this.textBoxWSampleName.Text = this.comboBoxElementName.Text + "-(" + this.nbtxtHeight.Text + ")";
                        else
                            this.textBoxWSampleName.Text = this.comboBoxElementName.Text;
                    }
                }
                #endregion
            }
            if (textBoxWSampleName.Visible || comboBoxElementName.Visible)
            {
                if (!GetSpecFromInputInfor())
                    return;
            }
            if (numTestCount.Visible && numTestCount.Value == 0)
            {
                Msg.Show(Info.PleaseWriteTestTimes);
                return;
            }
            SaveRhParamsValue();

            bool isNotSampleName = false;//多样品腔时，判断是否存在样品名称存在空的。
            if (listWorkCurveTest != null && listWorkCurveTest.Count > 0)
            {
                List<string> specListNameList = new List<string>();
                foreach (KeyValuePair<string, WordCureTest> pair in this.listWorkCurveTest)
                {
                    if (!specListNameList.Exists(delegate(string v) { return v == pair.Value.sampleInfo.SampleName; })) specListNameList.Add(pair.Value.sampleInfo.SampleName);
                    if (pair.Value.sampleInfo.SampleName == "") isNotSampleName = true;
                    if (!WorkCurveInfoValue(pair.Key, pair.Value, pair.Value.sampleInfo))
                        continue;
                    if (pair.Value.WordCurveName != null && pair.Value.Spec != null && pair.Value.Spec.SampleName != null)
                    {
                        localWorkCurve.Add(pair.Value);
                        WordCureTestSerialize serializeTemp = new WordCureTestSerialize(pair.Value.SerialNumber, pair.Value.WordCurveName, pair.Value.sampleInfo, pair.Value.WordCurveID);
                        serializeChamber.Add(serializeTemp);
                    }
                }
            }
            if (localWorkCurve.Count == 0 || isNotSampleName)
            {
                localWorkCurve.Clear();
                Msg.Show(Info.PleaseWriteNameAndCode);
                return;
            }

            if (groupStand.Visible)
            {
                if ((cmbSelectStand.SelectedText != null || cmbSelectStand.SelectedText != "") && chkSelectStand.Checked)
                {
                    // 1209
                    WorkCurveHelper.CurrentStandard = lstStandard[cmbSelectStand.SelectedIndex];
                    CustomStandard devStandard = WorkCurveHelper.CurrentStandard;


                    if (WorkCurveHelper.WorkCurveCurrent.ElementList.Items != null && WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count > 0)
                    {
                        int layerCount = WorkCurveHelper.WorkCurveCurrent.ElementList.Items[0].LayerNumber;
                        for (int i = 0; i < layerCount - 1; i++)
                        {
                            Control[] controls = groupStand.Controls.Find("ucSingStand" + (i + 1), true);
                            if (controls != null)
                            {
                                controls[0] = controls[0] as UCSingStand;
                                bool hasElement = false;

                                for (int j = 0; j < devStandard.StandardDatas.Count; j++)
                                {
                                    hasElement = false;
                                    if (devStandard.StandardDatas[j].ElementCaption.Equals(((UCSingStand)controls[0]).SName))
                                    {
                                        devStandard.StandardDatas[j].StandardThick = ((UCSingStand)controls[0]).StandMin;
                                        devStandard.StandardDatas[j].StandardThickMax = ((UCSingStand)controls[0]).StandMax;
                                        hasElement = true;
                                        break;
                                    }
                                }
                                if (!hasElement)
                                {
                                    devStandard.StandardDatas.Add(StandardData.New.Init(((UCSingStand)controls[0]).SName, 0, ((UCSingStand)controls[0]).StandMin, ((UCSingStand)controls[0]).StandMax));
                                }

                            }


                        }
                        WorkCurveHelper.CurrentStandard = devStandard;
                        WorkCurveHelper.CurrentStandard.Save();
                    }
                    //Control[] controls = groupStand.Controls.Find("ucSingStand" + elements[i].LayerNumber.ToString(), true);
                    //if (controls != null)
                    //{
                    //    controls[0] = controls[0] as UCSingStand;
                    //    controls[0].Visible = true;
                    //    ((UCSingStand)controls[0]).LayerName = ShowfloorName(elements[i]);//elements[i].Caption;
                    //    StandardData OneElemData = WorkCurveHelper.CurrentStandard.StandardDatas.ToList<StandardData>().Find(w => w.ElementCaption == elements[i].Caption);
                    //    if (OneElemData != null)
                    //    {
                    //        ((UCSingStand)controls[0]).StandMin = OneElemData.StandardThick;
                    //        ((UCSingStand)controls[0]).StandMax = OneElemData.StandardThickMax;
                    //    }
                    //    else
                    //    {
                    //        ((UCSingStand)controls[0]).StandMin = 1;
                    //        ((UCSingStand)controls[0]).StandMax = 2;
                    //    }
                    //}

                    //WorkCurveHelper.CurrentStandard = lstStandard[cmbSelectStand.SelectedIndex];
                }
            }

            List<WordCureTest> orderWorkCurve = localWorkCurve.OrderBy(w => int.Parse(w.SerialNumber)).ToList();
            try
            {
                this.numTestCount.Value = this.chkMatch.Checked && WorkCurveHelper.IsDirectCaculate ? 1 : this.numTestCount.Value;
                if (int.Parse(this.numTestCount.Value.ToString()) > 1)
                {
                    InterfaceClass.selePrintObjectL.Clear();
                    InterfaceClass.isMulitTest = true;
                }

                WorkCurveHelper.testTimes = int.Parse(this.numTestCount.Value.ToString());
                MeasureParams MeasureParams = new MeasureParams(int.Parse(this.numTestCount.Value.ToString()),
                                                                int.Parse(this.numDropTime.Value.ToString()),
                                                                chkManualTest.Visible && chkManualTest.Checked && chkManualTest.Enabled);
                this.TestDevicePassedParams = new TestDevicePassedParams(this.chkMatch.Checked && SpecType != SpecType.PureSpec, MeasureParams, orderWorkCurve, IsAllowOpenCover,
                                                                         SpecType, this.comboBoxElementName.Visible == true ? this.comboBoxElementName.Text : "",
                                                                         (SpecType == SpecType.StandSpec) ? chkSamples.Checked : false,
                                                                         (WorkCurveHelper.DeviceCurrent.HasChamber) ? chkMuliRuleName.Checked : chkRuleName.Checked);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (this.ParentForm != null)
                this.ParentForm.DialogResult = this.dialogResult = DialogResult.OK;
            signObject = null;
            DifferenceDevice.IsConnect = true;
            EDXRFHelper.GotoMainPage(this);
        }
        protected override bool ProcessKeyPreview(ref Message m)
        {
            //if (WorkCurveHelper.DirectRun.IsKeyCall && WorkCurveHelper.DirectRun.IsDirectRun)
            //{
            //    DirectRun();
            //}
            return base.ProcessKeyPreview(ref m);
        }

        public void DirectRun()
        {
            buttonWSubmit_Click(null, null);
        }

        /// <summary>
        /// 根据用户的选择指定扫描的类型
        /// </summary>
        /// <returns></returns>
        private SpecType GetSpecType()
        {
            SpecType type = SpecType.UnSelected;
            if (radioButtonPureElement.Checked)
                type = SpecType.PureSpec;
            else if (radioButtonStandand.Checked)
                type = SpecType.StandSpec;
            else if (radioButtonUnknown.Checked)
                type = SpecType.UnKownSpec;
            return type;
        }

        private void SetSpecType(uint type)
        {
            switch (type)
            {
                case 0:
                    this.radioButtonStandand.Checked = true;
                    break;
                case 1:
                    this.radioButtonPureElement.Checked = true;
                    break;
                case 2:
                    this.radioButtonUnknown.Checked = true;
                    break;
                default:
                    break;
            }

        }
        /// <summary>
        /// 对不同的扫描类型及软件功能对现实的控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxW1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMatch.Checked && (WorkCurveHelper.DeviceCurrent.HasChamber && WorkCurveHelper.DeviceCurrent.Chamber.Count > 0))
            {
                //this.numTestCount.Value = 1;
                //this.numTestCount.Enabled = false;
            }
            else if (chkMatch.Checked && !WorkCurveHelper.DeviceCurrent.HasChamber)
            {
                this.grpCurve.Visible = false;
                //this.grpTestParams.Visible = false;
                //this.numTestCount.Value = 1;
                //this.numTestCount.Enabled = false;
            }
            else
            {
                this.numTestCount.Enabled = true;
                //this.grpTestParams.Visible = true;
            }
        }

        /// <summary>
        /// 取消按钮处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            signObject = null;
            if (WorkCurveHelper.IsPureAuTest)
            {
                WorkCurveHelper.WorkCurveCurrent = WorkCurveHelper.BeforePureAuCurveCurrent;
                DifferenceDevice.MediumAccess.OpenCurveSubmit();
                WorkCurveHelper.IsPureAuTest = false;
            }
            EDXRFHelper.GotoMainPage(this);
        }

        /// <summary>
        ///  用户选择纯元素扫描的处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonPureElement_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonPureElement.Checked)
            {

                this.textBoxWSampleName.Visible = false;
                this.comboBoxElementName.Visible = true;
                this.grpMatch.Visible = false;
                //this.grpTestParams.Visible = true;
                //lblTestCount.Visible = false;
                //numTestCount.Visible = false;
                this.numTestCount.Value = 1;
                this.numTestCount.Enabled = false;
                this.SpecType = SpecType.PureSpec;
                //if (this.modelTool == 1)
                //    this.grOptions.Visible = true;
                //else
                //this.chkRhThick.Visible = false;
                this.grOptions.Visible = false;
                this.chkPureElement.Visible = true;
                this.chkSamples.Visible = false;
                this.groupStand.Visible = false;
                if (!DifferenceDevice.IsXRF && !DifferenceDevice.IsAnalyser)
                {
                    this.grOptions.Visible = false;
                }
                this.radioButtonStandand.Checked = false;
                this.radioButtonUnknown.Checked = false;

                var vv = User.Find(w => w.Name == FrmLogon.userName);
                if (vv.Count > 0 && vv[0].Role.RoleType == 2)
                {
                    this.lblDropTime.Visible = false;
                    this.numDropTime.Visible = false;
                }
                else
                {
                    this.lblDropTime.Visible = true;
                    this.numDropTime.Visible = true;
                }
                this.chkManualTest.Enabled = this.radioButtonUnknown.Checked; ;
                SwitchSpecType(true);
            }
            else SwitchSpecType(false);
        }

        private void SwitchSpecType(bool isPureElement)
        {
            //选择纯元素切换时，进行样品名清空,如果切换到待测谱和标样谱时，样品名称自动填写
            foreach (KeyValuePair<string, WordCureTest> pair in listWorkCurveTest)
            {
                WordCureTest wordCureTest = pair.Value;
                if (isPureElement)
                {
                    Control[] controls = this.Controls.Find(pair.Key, true);
                    TextBoxW texbox = (controls.First(wde => wde.GetType() == typeof(TextBoxW))) as TextBoxW;
                    texbox.Text = "";
                    if (wordCureTest.sampleInfo != null) wordCureTest.sampleInfo.SampleName = "";
                    if (wordCureTest.Spec != null) wordCureTest.Spec.SampleName = "";
                }
                else
                {
                    Control[] controls = this.Controls.Find(pair.Key, true);
                    TextBoxW texbox = (controls.First(wde => wde.GetType() == typeof(TextBoxW))) as TextBoxW;
                    //texbox.Text = sampleNameFromTime + "#" + pair.Key.Replace("controlName", "");
                    texbox.Text = sampleNameFromAppXml + "#" + pair.Key.Replace("controlName", "");
                    if (wordCureTest.sampleInfo != null) wordCureTest.sampleInfo.SampleName = texbox.Text;
                    if (wordCureTest.Spec != null) wordCureTest.Spec.SampleName = texbox.Text;
                }

            }
        }

        private void radioButtonUnknown_CheckedChanged(object sender, EventArgs e)
        {

            if (this.radioButtonUnknown.Checked)
            {
                lblTestCount.Visible = true;
                numTestCount.Visible = true;
                this.textBoxWSampleName.Visible = true;
                this.labelSampleNameList.Visible = true;
                SetTxtSampleName(textBoxWSampleName, "textBox", chkRuleName);
                //this.textBoxWSampleName.Text = "";
                //this.textBoxWSampleName.Text = sampleNameFromTime;
                this.numTestCount.Value = 1;
                this.numTestCount.Enabled = true;
                //this.chkRhThick.Visible = true;

                //lblTestCount.Visible = true;
                //numTestCount.Visible = true;
                if (!DifferenceDevice.IsThick && this.modelTool == 1)
                {
                    this.grpMatch.Visible = false;
                    //this.grpTestParams.Visible = false;
                    this.comboBoxElementName.Visible = false;
                    this.textBoxWSampleName.Visible = true;
                    this.radioButtonStandand.Visible = false;
                    this.panel2.Visible = false;
                }
                else if (!DifferenceDevice.IsThick && this.modelTool == 0)
                {
                    this.grpTestParams.Visible = true;
                    this.comboBoxElementName.Visible = false;
                    this.grpMatch.Visible = true;
                    //this.labelWMatchTime.Visible = false;
                    //this.numricUpDownWMatch.Visible = false;
                    //var vv = User.FindOne(w => w.Name == FrmLogon.userName);
                    //if (vv != null && vv.Role.RoleType != 0)
                    //    this.grpMatch.Visible = false;
                    //if (chkMatch.Checked)
                    //{
                    //    this.numTestCount.Enabled = false;
                    //}
                }
                else if (DifferenceDevice.IsThick)
                {
                    this.grpMatch.Visible = false;
                    this.grpTestParams.Visible = true;
                    this.comboBoxElementName.Visible = false;
                }

                if (lstStandard.Count > 0)
                {
                    this.groupStand.Visible = true;
                }
                else
                {
                    this.groupStand.Visible = false;
                }

                if ((DifferenceDevice.IsXRF || DifferenceDevice.IsAnalyser) && this.modelTool == 1)
                {
                    this.numTestCount.Value = 1;
                    this.numTestCount.Enabled = false;
                }

                this.SpecType = SpecType.UnKownSpec;
                this.chkPureElement.Visible = false;
                this.chkSamples.Visible = false;
                this.grOptions.Visible = false;
                this.radioButtonPureElement.Checked = false;
                this.radioButtonStandand.Checked = false;
                var vvt = User.Find(w => w.Name == FrmLogon.userName);
                if (vvt.Count > 0 && vvt[0].Role.RoleType == 2)
                {
                    this.lblDropTime.Visible = false;
                    this.numDropTime.Visible = false;
                    //this.grpMatch.Visible = false;
                }
                else
                {
                    this.lblDropTime.Visible = true;
                    this.numDropTime.Visible = true;
                }

                if (WorkCurveHelper.IsPureAuTest) grpMatch.Visible = false;
                this.chkManualTest.Enabled = this.radioButtonUnknown.Checked;
            }
        }

        private void radioButtonStandand_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonStandand.Checked)
            {
                this.textBoxWSampleName.Visible = true;
                //this.textBoxWSampleName.Text = "";
                SetTxtSampleName(textBoxWSampleName, "textBox", chkRuleName);
                this.labelSampleNameList.Visible = true;
                this.comboBoxElementName.Visible = false;
                this.grpMatch.Visible = false;
                //this.chkRhThick.Visible = false;
                //this.grpTestParams.Visible = true;
                //lblTestCount.Visible = false;
                //numTestCount.Visible = false;
                this.numTestCount.Value = 1;
                this.numTestCount.Enabled = false;
                this.SpecType = SpecType.StandSpec;
                this.chkPureElement.Visible = false;
                this.chkSamples.Visible = true;
                this.grOptions.Visible = true;
                this.groupStand.Visible = false;
                if (!DifferenceDevice.IsXRF && !DifferenceDevice.IsAnalyser)
                {
                    this.grOptions.Visible = false;
                }
                this.radioButtonUnknown.Checked = false;
                this.radioButtonPureElement.Checked = false;
                var vvt = User.Find(w => w.Name == FrmLogon.userName);
                if (vvt.Count > 0 && vvt[0].Role.RoleType == 2)
                {
                    this.lblDropTime.Visible = false;
                    this.numDropTime.Visible = false;

                }
                else
                {
                    this.lblDropTime.Visible = true;
                    this.numDropTime.Visible = true;
                }
                this.chkManualTest.Enabled = this.radioButtonUnknown.Checked;
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                buttonWSubmit_Click(null, null);
                return true;
            }
            else if (keyData == Keys.Escape)
            {
                btnCancel_Click(null, null);
                return true;
            }
            else
                return base.ProcessDialogKey(keyData);
        }

        public override void ExcuteEndProcess(params object[] objs)
        {



            Bitmap testDemoImg = null;

            if (WorkCurveHelper.DeviceCurrent.HasMotorSpin)
            {
                testDemoImg =  DifferenceDevice.CurCameraControl.skyrayCamera1.GrabImage();
                WorkCurveHelper.reportImage = testDemoImg;
            }

            WorkCurveHelper.reportImage = testDemoImg;

            if (WorkCurveHelper.testNum == 1)
            {
                if (WorkCurveHelper.DeviceCurrent.HasMotorSpin)
                {

                    DifferenceDevice.CurCameraControl.skyrayCamera1.BackgroundImageLayout = ImageLayout.Stretch;

                    DifferenceDevice.CurCameraControl.skyrayCamera1.BackgroundImage = testDemoImg;

                    DifferenceDevice.CurCameraControl.skyrayCamera1.Stop();
                }
                
                
               
            }

            ReportTemplateHelper.SaveSpecifiedValue("TestParams", "SpecType", ((int)SpecType).ToString());
            string sample = string.Empty;
            if (SpecType == SpecType.StandSpec || SpecType == SpecType.UnKownSpec)
                sample = this.textBoxWSampleName.Text;
            else
                sample = this.comboBoxElementName.Text;

            if(sample.Contains("#"))
                sample = sample.Split(new char[] { '#' })[0];
            ReportTemplateHelper.SaveSpecifiedValue("TestParams", "SampleName", sample);

            if (!SampleNameList.Contains(sample))
            {
                SampleNameList.Add(sample);
            }
            if (SampleNameList.Count > 10)
            {
                int removecnt = SampleNameList.Count - 10;
                for (int i = 0; i < removecnt; i++)
                {
                    SampleNameList.RemoveAt(i);
                }
            }
           

            string smpStr = string.Empty;
            string temp = "";

            foreach(var s in SampleNameList)
            {
                if (s.Contains("#"))
                    temp = s.Split(new char[] { '#' })[0];
                else
                    temp = s;
                smpStr += temp + ",";
            }



            ReportTemplateHelper.SaveSpecifiedValue(Application.StartupPath + "\\AppParams.xml", "TestParams", "SampleNameList", smpStr.Substring(0, smpStr.Length - 1));
            ReportTemplateHelper.SaveSpecifiedValue("TestParams", "Shape", this.comboBoxWShape.Text);
            if (this.comboBoxWSupplier.Text != "" || this.comboBoxWSupplier.Text != null)
            {
                lstSupplier = Supplier.FindAll();
                if (lstSupplier.Find(l => l.Name == comboBoxWSupplier.Text) != null)
                {

                }
                else
                {
                    var v = Supplier.New.Init(this.comboBoxWSupplier.Text, DateTime.Today.ToShortDateString());
                    lstSupplier.Add(v);
                    foreach (var lst in lstSupplier)
                    {
                        lst.Save();
                    }
                }
            }
            ReportTemplateHelper.SaveSpecifiedValue("TestParams", "Supplier", this.comboBoxWSupplier.Text);
            ReportTemplateHelper.SaveSpecifiedValue("TestParams", "Weight", this.txtWeight.Text);
            ReportTemplateHelper.SaveSpecifiedValue("TestParams", "Description", this.txtDescription.Text);
            ReportTemplateHelper.SaveSpecifiedValue("TestParams", "MeasureTime", this.numTestCount.Value.ToString());
            ReportTemplateHelper.SaveSpecifiedValue("TestParams", "DropTime", this.numDropTime.Value.ToString());
            ReportTemplateHelper.SaveSpecifiedValue("TestParams", "IsMatch", (this.chkMatch.Checked ? true : false).ToString());
            //ReportTemplateHelper.SaveSpecifiedValue("TestParams", "IsShowRh", this.chkRhThick.Visible?"1":"0");
            ReportTemplateHelper.SaveSpecifiedValue("TestParams", "IsRhlayer", this.chkRhThick.Checked ? "1" : "0");
            ReportTemplateHelper.SaveSpecifiedValue("TestParams", "Rhfactor", this.txtRhfactor.Text);
            ReportTemplateHelper.SaveSpecifiedValue("TestParams", "IsRuleName", (WorkCurveHelper.DeviceCurrent.HasChamber) ? (this.chkMuliRuleName.Checked ? true : false).ToString() : (this.chkRuleName.Checked ? true : false).ToString());

            ReportTemplateHelper.SaveSpecifiedValue("TestParams", "IsAscendName", chkAscend.Checked ? "true" : "false");
            ReportTemplateHelper.SaveSpecifiedValue(Application.StartupPath + "\\AppParams.xml", "TestParams", "AutoUpload", chkAutoUploadData.Checked.ToString());
            //if (sample.IsNumeric() && numAscend.Text != "" && chkAscend.Checked)
            if (numAscend.Text != "" && chkAscend.Checked)
            {
                //int num = int.Parse(sample) + int.Parse(numAscend.Text);
                ReportTemplateHelper.SaveSpecifiedValue("TestParams", "AscendScalar", numAscend.Text);
                //ReportTemplateHelper.SaveSpecifiedValue("TestParams", "SampleName", num.ToString());
            }
            if (serializeChamber.Count > 0)
            {
                libSerialize.SaveObj(serializeChamber, Application.StartupPath + "\\chamberSave");
            }
            WorkCurveHelper.MainSpecList = null;


            if (StartType)
            {
                WorkCurveHelper.contiOffsetInTemp = 0;
                WorkCurveHelper.testParamsBackup = TestDevicePassedParams;
                DifferenceDevice.MediumAccess.ExcuteTestStart(TestDevicePassedParams);
                
            }
            
        }

        public override void ExcuteCloseProcess(params object[] objs)
        {
            signObject = null;
        }

        public override bool LoadConditionAnalyser()
        {
            if (WorkCurveHelper.WorkCurveCurrent == null && this.modelTool == 0)
            {
                Msg.Show(Info.WarningTestContext, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        //修改：何晓明 2011-05-24 排除负数、小数等
        private void NumericUpDown_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ',' || e.KeyChar == '.' || e.KeyChar == '-')
            {
                e.Handled = true;
            }
        }

        private void textBoxWSampleName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '/' || e.KeyChar == '\\')
            {
                e.Handled = true;
            }
        }

        private List<CompanyOthersInfo> listOtherInfo;

        private void NewWorkSpec_Load(object sender, EventArgs e)
        {

            //界面排版
            if (!this.chkManualTest.Visible)
            {
                lblTestCount.Left = lblWsampleName.Left;
                numTestCount.Left = comboBoxElementName.Left;
                lblDropTime.Left = chkRuleName.Left;
                numDropTime.Left = chkAscend.Left;
            }
            listOtherInfo = new List<CompanyOthersInfo>();
            List<CompanyOthersInfo> companyOthersInfo_List = CompanyOthersInfo.FindBySql("select * from companyothersinfo where  Display =1 and ExcelModeType='" + ReportTemplateHelper.ExcelModeType.ToString() + "'");
            if (companyOthersInfo_List != null && companyOthersInfo_List.Count > 0)
            {
                WorkCurveHelper.SeleCompanyOthersInfo.Clear();
                //if (companyOthersInfo_List.Count <= 5)
                if (companyOthersInfo_List.Count <= 2)
                {
                    UCOtherInfoSetOper();
                }
                else
                {
                    buttonWOtherInfoSet.Visible = true;
                }
            }
            if (!DifferenceDevice.IsXRF) txtLoss.Enabled = false;
            bool flag = false;
            bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("TestParams", "DirectRun"), out flag);
            WorkCurveHelper.DirectRun.IsDirectRun = flag;
            if ((WorkCurveHelper.DirectRun.IsKeyCall && WorkCurveHelper.DirectRun.IsDirectRun) ||
                (DifferenceDevice.IsAnalyser && WorkCurveHelper.ButtonDirectRun))
            {
                Form frm = this.ParentForm;
                if (frm != null)
                    frm.Opacity = 0;
            }
            txtLayerElems.BindVisibleToCtrl(chkRhThick, "Checked");
            if (WorkCurveHelper.WorkCurveCurrent != null
                && WorkCurveHelper.WorkCurveCurrent.ElementList != null
                && !string.IsNullOrEmpty(WorkCurveHelper.WorkCurveCurrent.ElementList.LayerElemsInAnalyzer)
                )
            {
                txtLayerElems.Text = WorkCurveHelper.WorkCurveCurrent.ElementList.LayerElemsInAnalyzer;
            }

            if ((WorkCurveHelper.DirectRun.IsDirectRun && WorkCurveHelper.DirectRun.IsKeyCall) ||
                (DifferenceDevice.IsAnalyser && WorkCurveHelper.ButtonDirectRun))
                buttonWSubmit_Click(null, null);
            if (DifferenceDevice.IsAnalyser && Application.ProductName.Equals(@"Skyray.EDX3000"))
            {
                chkAutoUploadData.Visible = WorkCurveHelper.AutoUploadVisible;
            }
            nbtxtHeight.Text = DifferenceDevice.interClassMain.deviceMeasure.interfacce.ReturnEncoderValue.ToString("f2");

            LoadSampleNameList();
        }

        private void LoadSampleNameList()
        {
            SampleNameList.Clear();
            string smp = ReportTemplateHelper.LoadSpecifiedValue("TestParams", "SampleNameList");
            if (smp != null && (!smp.Equals(string.Empty)))
            {
                string[] samplenames = smp.Split(',');
                if (samplenames.Length > 0)
                {
                    foreach (var s in samplenames)
                    {
                        SampleNameList.Add(s);
                    }
                }
            }
            else
            {
                string sm = ReportTemplateHelper.LoadSpecifiedValue("TestParams", "SampleName");
                if (sm != null && sm.Length > 0)
                    SampleNameList.Add(sm);
            }

            if (SampleNameList.Count > 0)
            {
                List<string> lstTempSampList = new List<string>(SampleNameList);
                lstTempSampList.Reverse();
                lstSampleName.DataSource = lstTempSampList;
            }

        }
        private List<Control> listControls;
        private void UCOtherInfoSetOper()
        {
            listOtherInfo = CompanyOthersInfo.FindBySql("select * from companyothersinfo where 1=1 and Display =1 and ExcelModeType='" + ReportTemplateHelper.ExcelModeType.ToString() + "' ");
            listControls = new List<Control>();
            int i = 0;
            foreach (var name in listOtherInfo)
            {
                Label label = new Label();
                label.Name = "Label" + name.Id;
                label.Text = name.Name;
                label.AutoSize = true;
                label.BackColor = System.Drawing.Color.Transparent;
                label.Location = new System.Drawing.Point(18 + ((i) % 3) * 230, 85 + ((i + 1) / 3) * 34);
                // label.Location = new System.Drawing.Point(87 + i * 200, 50);
                label.Size = new System.Drawing.Size(50, 12);
                listControls.Add(label);
                if (name.ControlType == 1)
                {
                    List<CompanyOthersListInfo> listOtherInfoData = CompanyOthersListInfo.FindBySql("select * from companyothersListinfo where 1=1 and companyothersinfo_id =" + name.Id);
                    Skyray.Controls.ComboBoxW combox = new Skyray.Controls.ComboBoxW();
                    combox.AutoComplete = false;
                    combox.AutoDropdown = false;
                    combox.BackColorEven = System.Drawing.Color.White;
                    combox.BackColorOdd = System.Drawing.Color.White;
                    combox.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
                    combox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
                    combox.ColumnNames = "";
                    combox.ColumnWidthDefault = 75;
                    combox.ColumnWidths = "";
                    combox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
                    combox.FormattingEnabled = true;
                    combox.LinkedTextBox = null;
                    // combox.Location = new System.Drawing.Point(87 + ((i + 1) % 3) * 150, 44 + ((i + 1) / 3) * 34);
                    combox.Location = new System.Drawing.Point(118 + ((i) % 3) * 230, 80 + ((i + 1) / 3) * 34);
                    combox.Name = "comBox_" + name.Id;
                    combox.Size = new System.Drawing.Size(90, 20);
                    foreach (var data in listOtherInfoData)
                    {
                        combox.Items.Add(data.ListInfo);
                    }
                    if (combox.Items.Count > 0)
                    {
                        CompanyOthersListInfo DisplayListInfo = listOtherInfoData.Find(l => l.Display == true);
                        if (DisplayListInfo != null)
                            combox.Text = DisplayListInfo.ListInfo;
                        else combox.SelectedIndex = 0;
                    }
                    BindHelper.BindTextToCtrl(combox, name, "DefaultValue", true);
                    combox.TextChanged += new EventHandler(combox_Click);
                    listControls.Add(combox);
                }
                else if (name.ControlType == 0)
                {
                    Skyray.Controls.TextBoxW textbox = new Skyray.Controls.TextBoxW();
                    textbox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
                    textbox.Location = new System.Drawing.Point(118 + ((i) % 3) * 230, 80 + ((i + 1) / 3) * 34);
                    textbox.Name = "textbox_" + name.Id;
                    textbox.Size = new System.Drawing.Size(100, 20);
                    BindHelper.BindTextToCtrl(textbox, name, "DefaultValue", true);
                    listControls.Add(textbox);
                }
                else if (name.ControlType == 2)
                {
                    DateTimePicker dateTimePicker = new DateTimePicker();
                    dateTimePicker.Location = new System.Drawing.Point(118 + ((i) % 3) * 230, 80 + ((i + 1) / 3) * 34);
                    dateTimePicker.Name = "dateTimePicker_" + name.Id;
                    dateTimePicker.Size = new System.Drawing.Size(90, 20);
                    dateTimePicker.Format = DateTimePickerFormat.Custom;
                    dateTimePicker.CustomFormat = "yyyy-MM-dd";
                    BindHelper.BindTextToCtrl(dateTimePicker, name, "DefaultValue", true);
                    listControls.Add(dateTimePicker);
                }
                i++;
            }
            foreach (var ctrl in listControls)
            {
                pnlOtherInfo.Controls.Add(ctrl);
            }
        }

        private void combox_Click(object sender, EventArgs e)
        {
            string ListInfo = ((Skyray.Controls.ComboBoxW)sender).Text;
            string CompanyOthersInfo_Id = (((Skyray.Controls.ComboBoxW)sender).Name.IndexOf("_") != -1) ? ((Skyray.Controls.ComboBoxW)sender).Name.Split('_')[1] : "";
            if (CompanyOthersInfo_Id == "") return;
            Lephone.Data.DbEntry.Context.ExecuteNonQuery("update  companyothersListinfo set display=0 where companyothersinfo_id='" + CompanyOthersInfo_Id + "'; update  companyothersListinfo set display=1 where companyothersinfo_id='" + CompanyOthersInfo_Id + "' and listinfo='" + ListInfo + "'");
        }

        private void GetSetCompanyOthersInfo()
        {
            WorkCurveHelper.SeleCompanyOthersInfo.Clear();
            foreach (Control c in listControls)
            {
                if (c is Skyray.Controls.ComboBoxW || c is Skyray.Controls.TextBoxW || c is DateTimePicker)
                {
                    if (c.Name.IndexOf("_") != -1)
                    {
                        WorkCurveHelper.SeleCompanyOthersInfo.Add(c.Name.Split('_')[1], c.Text);
                    }
                }
            }

        }

        private void OtherInfoOK()
        {
            GetSetCompanyOthersInfo();
            foreach (var name in listOtherInfo)
            {
                name.Save();
            }
        }

        //protected override void WndProc(ref Message m)
        //{
        //    const int WM_HOTKEY = 0x0312;
        //    switch (m.Msg)
        //    {
        //        case WM_HOTKEY:
        //            switch (m.WParam.ToInt32())
        //            {
        //                case 600:
        //                    MessageBox.Show("sads");
        //                    break;
        //            }
        //            break;
        //    }
        //    base.WndProc(ref m);
        //}

        //private void SetOtherInformation()
        //{
        //    if (listOtherInfo!=null && listOtherInfo.Count <=2 )
        //    {
        //        buttonWOtherInfoSet.Visible = false;   //2个及以内不显示 其它信息按钮

        //        List<Control> listControls = new List<Control>();
        //        int i = 0;
        //        foreach (var name in listOtherInfo)
        //        {
        //            Label label = new Label();
        //            label.Name = "Label" + name.Id;
        //            label.Text = name.Name;
        //            label.AutoSize = true;
        //            label.BackColor = System.Drawing.Color.Transparent;
        //            label.Location = new System.Drawing.Point(20 + i * 200, 11);
        //            label.Size = new System.Drawing.Size(47, 15);
        //            listControls.Add(label);
        //            if (name.ControlType == 1)
        //            {
        //                List<CompanyOthersListInfo> listOtherInfoData = CompanyOthersListInfo.FindBySql("select * from companyothersListinfo where 1=1 and companyothersinfo_id =" + name.Id);
        //                Skyray.Controls.ComboBoxW combox = new Skyray.Controls.ComboBoxW();
        //                combox.AutoComplete = false;
        //                combox.AutoDropdown = false;
        //                combox.BackColorEven = System.Drawing.Color.White;
        //                combox.BackColorOdd = System.Drawing.Color.White;
        //                combox.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
        //                combox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
        //                combox.ColumnNames = "";
        //                combox.ColumnWidthDefault = 75;
        //                combox.ColumnWidths = "";
        //                combox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
        //                combox.FormattingEnabled = true;
        //                combox.LinkedTextBox = null;
        //                combox.Location = new System.Drawing.Point(20+ i*200 , 11);
        //                combox.Name = "comBox_" + name.Id;
        //                combox.Size = new System.Drawing.Size(121, 20);
        //                foreach (var data in listOtherInfoData)
        //                {
        //                    combox.Items.Add(data.ListInfo);
        //                }
        //                if (combox.Items.Count > 0)
        //                {
        //                    CompanyOthersListInfo DisplayListInfo = listOtherInfoData.Find(l => l.Display == true);
        //                    if (DisplayListInfo != null)
        //                        combox.Text = DisplayListInfo.ListInfo;
        //                    else combox.SelectedIndex = 0;
        //                }
        //                BindHelper.BindTextToCtrl(combox, name, "DefaultValue", true);
        //                combox.TextChanged += new EventHandler(combox_Click);
        //                listControls.Add(combox);
        //            }
        //            else if (name.ControlType == 0)
        //            {
        //                Skyray.Controls.TextBoxW textbox = new Skyray.Controls.TextBoxW();
        //                textbox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
        //                textbox.Location = new System.Drawing.Point(20 + i * 200, 11);
        //                textbox.Name = "textbox_" + name.Id;
        //                textbox.Size = new System.Drawing.Size(121, 20);
        //                BindHelper.BindTextToCtrl(textbox, name, "DefaultValue", true);
        //                listControls.Add(textbox);
        //            }
        //            else if (name.ControlType == 2)
        //            {
        //                DateTimePicker dateTimePicker = new DateTimePicker();
        //                //dateTimePicker.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
        //                dateTimePicker.Location = new System.Drawing.Point(20 + i * 200, 11);
        //                dateTimePicker.Name = "dateTimePicker_" + name.Id;
        //                dateTimePicker.Size = new System.Drawing.Size(121, 20);
        //                dateTimePicker.Format = DateTimePickerFormat.Custom;
        //                dateTimePicker.CustomFormat = "yyyy-MM-dd";
        //                BindHelper.BindTextToCtrl(dateTimePicker, name, "DefaultValue", true);
        //                listControls.Add(dateTimePicker);
        //            }
        //            i++;
        //        }
        //        foreach (var ctrl in listControls)
        //        {
        //            panel6.Controls.Add(ctrl);
        //        }
        //    }
        //}


        private void buttonWOtherInfoSet_Click(object sender, EventArgs e)
        {
            WorkCurveHelper.OpenUC(new UCOtherInfoSet(), false);
        }

        private void txtLoss_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar > (char)47 && e.KeyChar < (char)58 || e.KeyChar == (char)8 || e.KeyChar == (char)46 || e.KeyChar == (char)3 || e.KeyChar == (char)22)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtWeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar > (char)47 && e.KeyChar < (char)58 || e.KeyChar == (char)8 || e.KeyChar == (char)46 || e.KeyChar == (char)3 || e.KeyChar == (char)22)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void chkRhThick_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRhThick.Checked && WorkCurveHelper.WorkCurveCurrent.CalcType != CalcType.FP && !WorkCurveHelper.WorkCurveCurrent.ElementList.RhIsMainElementInfluence)
                this.txtRhfactor.Visible = true;
            else
                this.txtRhfactor.Visible = false;
            txtLayerElems.Visible = chkRhThick.Checked;
        }

        private void txtRhfactor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar > (char)47 && e.KeyChar < (char)58 || e.KeyChar == (char)8 || e.KeyChar == (char)46 || e.KeyChar == (char)3 || e.KeyChar == (char)22)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show(Info.IllegalInput);
            }
        }

        private void chkRuleName_CheckedChanged(object sender, EventArgs e)
        {
            SetTxtSampleName(textBoxWSampleName, "textBox", chkRuleName);
        }

        private void SetTxtSampleName(object Control, string ControlType, CheckBoxW chkrule)
        {
            if (ControlType == "comboBox")
            {
                string sampleName = ReportTemplateHelper.LoadSpecifiedValue("TestParams", "SampleName");
                if (!sampleName.IsNullOrEmpty())
                    ((ComboBoxW)Control).Text = sampleName;
            }
            else if (ControlType == "textBox")
            {
                string sampleName = ReportTemplateHelper.LoadSpecifiedValue("TestParams", "SampleName");
                //(TextBoxW)Control).Text = (chkrule.Checked) ? sampleNameFromTime : sampleName;
                ((TextBoxW)Control).Text = sampleName;
            }
        }

        //#region 条码扫描
        //BarCodeHook BarCode = new BarCodeHook();
        //private delegate void ShowInfoDelegate(BarCodeHook.BarCodes barCode);
        //private void ShowInfo(BarCodeHook.BarCodes barCode)
        //{
        //    if (this.InvokeRequired)
        //    {
        //        this.BeginInvoke(new ShowInfoDelegate(ShowInfo), new object[] { barCode });
        //    }
        //    else
        //    {
        //        DifferenceDevice.interClassMain.BarcodeScanningSampleName = barCode.IsValid ? barCode.BarCode : "";//是否为扫描枪输入，如果为true则是 否则为键盘输入

        //        if (DifferenceDevice.interClassMain.BarcodeScanningSampleName != "" && DifferenceDevice.interClassMain.BarcodeScanningSampleName != string.Empty)
        //        {
        //            this.textBoxWSampleName.Text = DifferenceDevice.interClassMain.BarcodeScanningSampleName;
        //        }

        //    }
        //}

        //void BarCode_BarCodeEvent(BarCodeHook.BarCodes barCode)
        //{
        //    ShowInfo(barCode);
        //}
        //#endregion

        private void radIsAllowOpenCover_CheckedChanged(object sender, EventArgs e)
        {
            if (radIsAllowOpenCover.Checked)
            {
                radIsAllowOpenCover.Checked = true;
            }
        }

        private void btnMatchSpec_Click(object sender, EventArgs e)
        {
            NaviItem NaviItem = WorkCurveHelper.NaviItems.Find(n => n.Name == "AddVirtualSpec");
            if (NaviItem.TT != null)
            {
                var obj = NaviItem.TT();
                if (obj == null) return;
                if (obj.GetType() == typeof(Form))
                    NaviItem.TT().Show();
                else
                {
                    WorkCurveHelper.OpenUC(obj, NaviItem.IsMaxnium, NaviItem.Text, NaviItem.IsModel, NaviItem.NoneStyle);
                }
            }
        }

        private void comboBoxWChooseCurve_SelectedIndexChanged(object sender, EventArgs e)
        {
            EDXRFHelper.DisplayWorkCurveControls();

            DifferenceDevice.MediumAccess.OpenCurveSubmit();

            DifferenceDevice.MediumAccess.RefreshHistory();
        }



        private void lblDescription_Click(object sender, EventArgs e)
        {

        }

        private void chkAutoUploadData_CheckedChanged(object sender, EventArgs e)
        {
            WorkCurveHelper.IsAutoUpload = chkAutoUploadData.Checked;
        }

        private void txtLayerElems_Click(object sender, EventArgs e)
        {
            if (WorkCurveHelper.WorkCurveCurrent.CalcType != CalcType.FP) return;
            chkLstbxLayerElems.Items.Clear();//清空
            //添加所有元素
            for (int i = 0; i < WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count; i++)
            {
                chkLstbxLayerElems.Items.Add(WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].Caption);
            }
            //检查是否是拟合元素
            string[] layerElems = txtLayerElems.Text.Split(',');
            for (int j = 0; j < chkLstbxLayerElems.Items.Count; j++)
            {
                chkLstbxLayerElems.SetItemChecked(j, false);
                for (int i = 0; i < layerElems.Length; i++)
                {
                    if (chkLstbxLayerElems.Items[j].ToString().ToUpper().Equals(layerElems[i].ToUpper()))
                    {
                        chkLstbxLayerElems.SetItemChecked(j, true);
                        break;
                    }
                }
            }
            chkLstbxLayerElems.SetBounds(grpMatch.Location.X + txtLayerElems.Location.X, grpMatch.Location.Y + txtLayerElems.Location.Y + txtLayerElems.Height - chkLstbxLayerElems.Height - 2, txtLayerElems.Width, chkLstbxLayerElems.Height);
            chkLstbxLayerElems.Visible = true;
            chkLstbxLayerElems.BringToFront();
        }

        private void NewWorkSpec_MouseMove(object sender, MouseEventArgs e)
        {
            if (!chkLstbxLayerElems.Bounds.Contains(e.Location))
            {
                chkLstbxLayerElems.Visible = false;
            }

            if (!lstSampleName.Bounds.Contains(e.Location))
            {
                lstSampleName.Visible = false;
            }
        }

        private void chkLstbxLayerElems_SelectedValueChanged(object sender, EventArgs e)
        {
            string layerElems = string.Empty;
            for (int i = 0; i < chkLstbxLayerElems.CheckedItems.Count; i++)
            {
                string name = chkLstbxLayerElems.CheckedItems[i].ToString();
                layerElems += name + ',';//拟合元素
            }
            if (layerElems.Length >= 1)
            {
                layerElems = layerElems.Substring(0, layerElems.Length - 1);
            }
            txtLayerElems.Text = layerElems;
        }

        private void textBoxWSampleName_Click(object sender, EventArgs e)
        {

            if (lstSampleName.Items.Count > 0)
            {
                lstSampleName.SetBounds(grpSample.Location.X + textBoxWSampleName.Location.X, grpSample.Location.Y + textBoxWSampleName.Location.Y + textBoxWSampleName.Height - 2, lstSampleName.Width, lstSampleName.Height);
                lstSampleName.Visible = true;
                lstSampleName.BringToFront();
            }
        }

        private void textBoxWSampleName_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (int)Keys.Down)
            {

                if (lstSampleName.Items.Count != 0)
                {
                    lstSampleName.Focus();

                    lstSampleName.SelectedIndex = 0;

                }

            }
        }

        private void lstSampleName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (int)Keys.Enter)
            {
                this.textBoxWSampleName.Text = lstSampleName.SelectedItem.ToString();

                this.lstSampleName.Visible = false;

                this.SendToBack();

            }
        }

        private void lstSampleName_Click(object sender, EventArgs e)
        {
            if (lstSampleName.SelectedItem != null)
            {

                this.textBoxWSampleName.Text = lstSampleName.SelectedItem.ToString();

                this.lstSampleName.Visible = false;

                this.SendToBack();

            }
        }

        private void textBoxWSampleName_Leave(object sender, EventArgs e)
        {
            if (lstSampleName.Focused == false)
            {
                this.lstSampleName.Visible = false;

                this.SendToBack();

            }
        }

        private void lstSampleName_Leave(object sender, EventArgs e)
        {
            if (textBoxWSampleName.Focused == false)
            {

                this.lstSampleName.Visible = false;

                this.SendToBack();

            }
        }

        private void cmbSelectStand_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chkSelectStand_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelectStand.Checked)
            {
                ucSingStand1.Enabled = true;
                ucSingStand2.Enabled = true;
                ucSingStand3.Enabled = true;
                ucSingStand4.Enabled = true;
                ucSingStand5.Enabled = true;
            }
            else
            {
                ucSingStand1.Enabled = false;
                ucSingStand2.Enabled = false;
                ucSingStand3.Enabled = false;
                ucSingStand4.Enabled = false;
                ucSingStand5.Enabled = false;
            }
        }



        //private void textBoxWSampleName_KeyPress_1(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar == (char)13)
        //    {
        //        textBoxWSampleName.SelectAll();
        //        HasBarcode = true;
        //    }
        //}

        //private void timer1_Tick(object sender, EventArgs e)
        //{
        //    if (HasBarcode && HasWeight)
        //    {
        //        Thread.Sleep(DifferenceDevice.interClassMain.IMeasureDelay * 1000);
        //        buttonWSubmit_Click(null, null);
        //        HasBarcode = false;
        //        HasWeight = false;
        //    }
        //}

        //public override void OpenUC(bool flag, string TitleName, bool isModel)
        //{
        //    if (!this.LoadConditionAnalyser())
        //        return;
        //    Form form = new Form();
        //    form.BackColor = Color.White;
        //    form.MinimizeBox = false;
        //    form.ShowInTaskbar = false;
        //    int padSpace = 0;
        //    form.Padding = new Padding(padSpace, padSpace, padSpace, padSpace);
        //    form.Controls.Add(this);
        //    form.MaximizeBox = flag;
        //    form.MaximizeBox = flag;
        //    form.Text = TitleName;
        //    form.TopMost = true;
        //    form.FormClosing += (s, ex) =>
        //    {
        //        signObject = null;
        //    };
        //    if (!flag)
        //    {
        //        form.FormBorderStyle = FormBorderStyle.FixedSingle;
        //    }
        //    this.AutoScroll = true;
        //    form.ClientSize = new Size(this.Width + padSpace * 2, this.Height + padSpace * 2);
        //    int width = Screen.PrimaryScreen.Bounds.Width;
        //    int hight = Screen.PrimaryScreen.Bounds.Height;
        //    if (width < this.Width)
        //        form.ClientSize = new Size(width, form.ClientSize.Height);
        //    if (hight < this.Height)
        //        form.ClientSize = new Size(form.ClientSize.Width, hight);
        //    form.ShowIcon = false;
        //    this.Dock = DockStyle.Fill;
        //    form.StartPosition = FormStartPosition.CenterScreen;
        //    if (isModel)
        //    {
        //        form.ShowDialog();
        //    }
        //    else
        //    {
        //        form.Show();
        //    }
        //}
    }
}
