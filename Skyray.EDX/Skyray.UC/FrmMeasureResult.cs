using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;
using Skyray.Print;
using Skyray.Language;
using Skyray.Controls;
using Skyray.EDX.Common.ReportHelper;
using System.Threading;

using Lephone.Data.Common;
using Lephone.Data.Definition;
using System.Data.SQLite;
using System.Reflection;
using System.Collections;
using System.Linq.Expressions;

namespace Skyray.UC
{
    public partial class FrmMeasureResult : MultipleForm
    {

        

        private string stringAu=string.Empty;
        
        public event Skyray.UC.EventDelegate.TestResultReport OnTestResultReport;

        private ElementList _dataSource;

        private double dShowLimitElement;

        public bool isShowElemFullname = false;

        //private double KaratTranslater = 99.995;

        public ElementList DataSource
        {
            set
            {
                //SetIsShowAuValue(value);
                if (value != null)
                {
                    _dataSource = value;
                    GetElementContent();
                    //this.dataGridViewW1.Rows.Clear();
                    //foreach (var posGroup in value)
                    //{
                    //    CurveElement element = posGroup as CurveElement;
                    //    Atom atomElement = Atoms.AtomList.ToList().Find(w => w.AtomName.ToLower() == element.Caption.ToLower());
                    //    if (atomElement != null && element.Content >= dShowLimitElement)
                    //        this.dataGridViewW1.Rows.Add((isShowElemFullname) ? atomElement.AtomNameEN : atomElement.AtomName, element.Intensity.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()), element.Content.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()));
                    //    else if (element.Content >= dShowLimitElement)
                    //        this.dataGridViewW1.Rows.Add(element.Caption, element.Intensity.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()), element.Content.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()));
                    //    if (element.Caption == "Au" && element.Content >= dShowLimitElement)
                    //    {
                    //        chKShowKValue.Visible = true;
                    //        this.labelInformation.Visible = true;
                    //        stringAu = (element.Content * 24 / 99.995).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "K";
                    //        this.labelInformation.Text = Info.IncludingAu + ":" + stringAu;
                    //    }
                    //    else
                    //    {
                    //        chKShowKValue.Visible = false;
                    //        this.labelInformation.Visible = false;
                    //    }
                    //}
                }
                else
                {
                    _dataSource = null;
                    ContructCurveData();
                }
                if (OnTestResultReport != null)
                    OnTestResultReport(this.dataGridViewW1, stringAu);
            }
        }

        private WorkCurve _currentWorkcurve = null;
        public WorkCurve CurrentWorkcurve
        {
            get { return _currentWorkcurve; }
            set { _currentWorkcurve = value; }
        }
        public bool IsShowWeight
        {
            get { return checkBoxWeight==null? false:checkBoxWeight.Checked; }
            set {
                  if (checkBoxWeight!=null)
                  checkBoxWeight.Checked = value;
                  textBoxResultWeight.Enabled = checkBoxWeight.Checked;
                }
        }
        public bool IsShowAuKValue
        {
            get { return chKShowKValue==null ? false:chKShowKValue.Checked; }
            set {
                if (chKShowKValue!=null)
                    chKShowKValue.Checked = value;
                }
        }
        private string _SelElementName=string.Empty;
        public string SelElementName
        {
            get { return _SelElementName; }
        }
        //private void SetIsShowAuValue(List<CurveElement> eleList)
        //{
        //    if (DifferenceDevice.IsAnalyser) return;
        //    if (eleList == null) chKShowKValue.Visible = false;
        //    else
        //    {
        //        if (eleList.Exists(delegate(CurveElement v) { return v.Caption.ToLower() == "au"; }))
        //            chKShowKValue.Visible = true;
        //        else
        //        { chKShowKValue.Visible = false; this.labelInformation.Visible = false; }
        //    }
        //}

        public string SampleName
        {
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    this.textBoxSampleName.Text = value;
                }
            }
        }
        //paul 20110523获取开始中重量 
        public string Weight
        {
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    this.textBoxResultWeight.Text = value;
                }
            }
        }

        //public string StatusLabel
        //{
        //    set
        //    {
        //        if (!value.IsNullOrEmpty())
        //        {
        //            this.labelInformation.Text = value;
        //        }
        //    }
        //}

        public FrmMeasureResult()
        {
            InitializeComponent();
            this.textBoxResultWeight.Enabled = false;
            chKShowKValue.Checked = DifferenceDevice.IsShowKarat;
            this.cobBoxShowBit.SelectedIndex = this.cobBoxShowBit.Items.IndexOf(ReportTemplateHelper.LoadSpecifiedValue("SoftWareCaculate", "ContentBit"));
            dShowLimitElement = Convert.ToDouble(ReportTemplateHelper.LoadSpecifiedValue("ShowLimitElement", "Limit"));
            this.TopMost = true;
            //string KaratTranslaterate = ReportTemplateHelper.LoadSpecifiedValue("OpenHistoryRecordType", "KaratTranslaterate");
            //try
            //{
            //    KaratTranslater = double.Parse(KaratTranslaterate);
            //}
            //catch
            //{
            //    KaratTranslater = 99.995;
            //}
             this.checkBoxWeight.Checked = DifferenceDevice.IsShowWeight;
             this.btnJoinCurve.Visible = !(GP.CurrentUser.Role.RoleType==2);
             this.btnNewCurve.Visible = !(GP.CurrentUser.Role.RoleType == 2);
             this.btnHotKey.Visible = WorkCurveHelper.IsSaveToDataBase;
             this.btnExportToSQL.Visible = WorkCurveHelper.IsSaveToDataBase;

             if (Lang.Model.CurrentLang.IsDefaultLang)
             {
                 DataGridViewTextBoxColumn tmpCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
                 tmpCol.HeaderText = Info.ElementName + " ";
                 tmpCol.Name = "Name";
                 tmpCol.ReadOnly = true;
                 tmpCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                 this.dataGridViewW1.Columns.Insert(this.dataGridViewW1.Columns.IndexOf(this.ElementName) + 1, tmpCol);
                 this.ElementName.Visible = false;
             }
        }

        private void buttonMeasureSubmit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonMeasureCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void checkBoxWeight_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxWeight.Checked)
            {
                //if (this.textBoxResultWeight.Text!="")
                //this.textBoxResultWeight.Text = "";
                //this.textBoxResultWeight.Enabled = true;
                DifferenceDevice.IsShowWeight = true;
                DataGridViewTextBoxColumn tepColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
                tepColumn.HeaderText = Info.Weight + "(g)";
                tepColumn.Name = "Weight";
                tepColumn.ReadOnly = true;
                tepColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                this.dataGridViewW1.Columns.Add(tepColumn);

                textBoxResultWeight_TextChanged(null,null);
            }
            else
            {
                DifferenceDevice.IsShowWeight = false;
                this.dataGridViewW1.Columns.Remove("Weight");
                //this.textBoxResultWeight.Text = "";
                //this.textBoxResultWeight.Enabled = false;
                
            }
        }

        private void textBoxResultWeight_TextChanged(object sender, EventArgs e)
        {
            if (!this.checkBoxWeight.Checked)
                return;
            if (!textBoxResultWeight.Text.IsNullOrEmpty())
            {
                try
                {
                   float totalW = float.Parse(textBoxResultWeight.Text);
                    for (int i = 0; i < this.dataGridViewW1.RowCount; i++)
                    {
                        string StrContext=this.dataGridViewW1.Rows[i].Cells["Context"].Value.ToString().Split('(')[0];
                        
                        this.dataGridViewW1.Rows[i].Cells["Weight"].Value =StrContext==string.Empty?StrContext: (totalW / 100 * float.Parse(StrContext)).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
                    }

                }
                catch 
                {
                    Msg.Show(Info.ValidateUserInput);
                }
            }
        }

        private void buttonSavePrint_Click(object sender, EventArgs e)
        {

        }

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            DifferenceDevice.interClassMain.SaveExcel(null, (checkBoxWeight.Checked) ? (double.Parse((textBoxResultWeight.Text == "") ? "0" : textBoxResultWeight.Text)) : 0);
            #region
            //if (ExcelTemplateParams.iTemplateType == 0)
            //    DifferenceDevice.uc.ExcutePrint(null);
            //else if (ExcelTemplateParams.iTemplateType == 2)
            //{
            //    if (InterfaceClass.SetPrintTemplate(null,null))
            //        EDXRFHelper.NewPrintDirectPrintHelper(InterfaceClass.seledataFountain);
            //    else
            //    {
            //        Msg.Show(Info.NoLoadSource);
            //    }
            //}
            //else if (ExcelTemplateParams.iTemplateType == 6)
            //{
            //    if (DifferenceDevice.interClassMain.reportThreadManage == null) return;
            //    List<long> hisRecordidList = new List<long>();
            //    foreach (HistoryRecord his in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(his.Id);

            //    string SaveReportPath = DifferenceDevice.interClassMain.reportThreadManage.GetHistoryRecordReport(hisRecordidList, (checkBoxWeight.Checked)?(double.Parse((textBoxResultWeight.Text == "") ? "0" : textBoxResultWeight.Text)):0, true, false);


            //    if (SaveReportPath == "") return;

            //    if (Skyray.Controls.SkyrayMsgBox.Show(PrintInfo.SaveSuccess + Skyray.EDX.Common.Info.OpenExcelOrNot, Skyray.EDX.Common.Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            //    {
            //        Thread thread = new Thread(new ParameterizedThreadStart(ShowExcel));
            //        thread.Priority = ThreadPriority.Highest;
            //        thread.Start(SaveReportPath);
            //    }
            //}
            //else if (ExcelTemplateParams.iTemplateType == 7)
            //{
            //    List<long> hisRecordidList = new List<long>();
            //    foreach (HistoryRecord his in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(his.Id);

            //    string excelPath = DifferenceDevice.interClassMain.BrassReport(hisRecordidList);
            //    if (excelPath == "") return;
            //    if (Skyray.Controls.SkyrayMsgBox.Show(PrintInfo.SaveSuccess + Skyray.EDX.Common.Info.OpenExcelOrNot, Skyray.EDX.Common.Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            //    {
            //        Thread thread = new Thread(new ParameterizedThreadStart(ShowExcel));
            //        thread.Priority = ThreadPriority.Highest;
            //        thread.Start(excelPath);
            //    }
            //}
            #endregion
        }

      

        public void Print()
        {
            buttonPrint_Click(null,null);
        }

        private void btnWriteReport_Click(object sender, EventArgs e)
        {
            //var ucPrint = new UCPrint();
            //ucPrint.Load += new EventHandler(PageLoad);
            //WorkCurveHelper.OpenUC(ucPrint, true, Info.DefiniteTemplate);
        }

        public new void PageLoad(object sender, EventArgs e)
        {
            if (Skyray.Language.Lang.Model.CurrentLang.IsDefaultLang)
            {
                Skyray.Language.Lang.Model.SaveFormText(true, (sender as UCPrint).PropertyPanel);
                Skyray.Language.Lang.Model.SaveFormText(true, sender);
            }
            else
            {
                Skyray.Language.Lang.Model.SetFormText(true, (sender as UCPrint).PropertyPanel);
                Skyray.Language.Lang.Model.SetFormText(true, sender);
            }

            Skyray.Language.Lang.Model.LanguageChanged += (s, ex) =>
            {
                Skyray.Language.Lang.Model.SetFormText(true, (sender as UCPrint).PropertyPanel);
                Skyray.Language.Lang.Model.SetFormText(true, sender);
            };

            Form fom = (sender as UCPrint).ParentForm;
            if (fom != null)
            {
                fom.MaximizeBox = true;
                fom.MinimizeBox = false;
                fom.WindowState = FormWindowState.Maximized;
            }
        }

        private void chKShowKValue_CheckedChanged(object sender, EventArgs e)
        {
            this.labelInformation.Visible = chKShowKValue.Checked;
            DifferenceDevice.IsShowKarat = chKShowKValue.Checked;
        }

        private void btnPrintReport_Click(object sender, EventArgs e)
        {
            NaviItem item = WorkCurveHelper.NaviItems.Find(w => w.Name == "Print");
            item.excuteRequire(null);
        }

        private void GetElementContent()
        {
            this.dataGridViewW1.Rows.Clear();
            if (_dataSource == null || _dataSource.Items.Count <= 0) return;
            string[] elementName = new string[_dataSource.Items.Count];
            string[] valuse = new string[_dataSource.Items.Count];
            int i=0;
            string[] strLayerElems = Helper.ToStrs(_dataSource.LayerElemsInAnalyzer == null ? "" : _dataSource.LayerElemsInAnalyzer);
            List<object> rowValues = null;
            #region //以前
            //foreach (var posGroup in _dataSource.Items.ToList().FindAll(w => w.IsShowElement).OrderBy(w => w.RowsIndex))
            //{
            //    CurveElement element = posGroup as CurveElement;
            //    Atom atomElement = Atoms.AtomList.ToList().Find(w => w.AtomName.ToLower() == element.Caption.ToLower());
            //    elementName[i] = element.Caption;
            //    string contentStr = "";
            //    //铑是镀层
            //    //if (_dataSource.RhIsLayer && element.Caption.ToUpper().Equals("RH"))
            //    if (_dataSource.RhIsLayer && strLayerElems.Contains(element.Caption))
            //    {
            //        checkBoxWeight.Enabled = false;
            //        contentStr = element.Thickness.ToString("f" + WorkCurveHelper.ThickBit.ToString()) + "(um)";
            //        valuse[i] = contentStr;
            //        if (atomElement != null)
            //            this.dataGridViewW1.Rows.Add((isShowElemFullname) ? atomElement.AtomNameEN : atomElement.AtomName, element.Intensity.ToString("f" + WorkCurveHelper.SoftWareIntensityBit.ToString()), contentStr);
            //        else if (element.Content >= dShowLimitElement)
            //            this.dataGridViewW1.Rows.Add(element.Caption, element.Intensity.ToString("f" + WorkCurveHelper.SoftWareIntensityBit.ToString()), contentStr);
            //        i++;
            //        continue;
            //    }
            //    if (element.ContentUnit == ContentUnit.per)
            //        contentStr = element.Content.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(%)";
            //    else if (element.ContentUnit == ContentUnit.ppm)
            //        contentStr = (element.Content * 10000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(ppm)";
            //    else
            //        contentStr = (element.Content * 10).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(‰)";


            //    if (atomElement != null && element.Content >= dShowLimitElement)
            //        this.dataGridViewW1.Rows.Add((isShowElemFullname) ? atomElement.AtomNameEN : atomElement.AtomName, element.Intensity.ToString("f" + WorkCurveHelper.SoftWareIntensityBit.ToString()), contentStr);
            //    else if (element.Content >= dShowLimitElement)
            //        this.dataGridViewW1.Rows.Add(element.Caption, element.Intensity.ToString("f" + WorkCurveHelper.SoftWareIntensityBit.ToString()), contentStr);
            //    valuse[i] = contentStr;
            //    i++;
            //}
            #endregion
            foreach (var posGroup in _dataSource.Items.ToList().FindAll(w => w.IsShowElement).OrderBy(w => w.RowsIndex))
            {
                CurveElement element = posGroup as CurveElement;
                Atom atomElement = Atoms.AtomList.ToList().Find(w => w.AtomName.ToLower() == element.Caption.ToLower());
                elementName[i] = element.Caption;
                string contentStr = "";
                //铑是镀层
                //if (_dataSource.RhIsLayer && element.Caption.ToUpper().Equals("RH"))
                if (_dataSource.RhIsLayer && strLayerElems.Contains(element.Caption))
                {
                    checkBoxWeight.Enabled = false;
                    contentStr = element.Thickness.ToString("f" + WorkCurveHelper.ThickBit.ToString()) + "(um)";
                    valuse[i] = contentStr;
                    //if (atomElement != null)
                    //    this.dataGridViewW1.Rows.Add((isShowElemFullname) ? atomElement.AtomNameEN : atomElement.AtomName, element.Intensity.ToString("f" + WorkCurveHelper.SoftWareIntensityBit.ToString()), contentStr);
                    //else if (element.Content >= dShowLimitElement)
                    //    this.dataGridViewW1.Rows.Add(element.Caption, element.Intensity.ToString("f" + WorkCurveHelper.SoftWareIntensityBit.ToString()), contentStr);
                    if (atomElement != null)
                    {
                        rowValues = new List<object>()
                        {
                            (isShowElemFullname) ? atomElement.AtomNameEN : atomElement.AtomName,
                            element.Intensity.ToString("f" + WorkCurveHelper.SoftWareIntensityBit.ToString()),
                            contentStr
                        };
                    }
                    //this.dataGridViewW1.Rows.Add((isShowElemFullname) ? atomElement.AtomNameEN : atomElement.AtomName, element.Intensity.ToString("f" + WorkCurveHelper.SoftWareIntensityBit.ToString()), contentStr);
                    else if (element.Content >= dShowLimitElement)
                    {
                        rowValues = new List<object>()
                        {
                            element.Caption, 
                            element.Intensity.ToString("f" + WorkCurveHelper.SoftWareIntensityBit.ToString()),
                            contentStr
                        };
                    }
                    if (Lang.Model.CurrentLang.IsDefaultLang)
                    {
                        if (rowValues != null)
                        {
                            rowValues.Insert(this.dataGridViewW1.Columns.IndexOf(this.ElementName) + 1, (atomElement == null ? string.Empty : atomElement.AtomNameCN) + element.Caption);
                        }
                    }
                    //this.dataGridViewW1.Rows.Add(element.Caption, element.Intensity.ToString("f" + WorkCurveHelper.SoftWareIntensityBit.ToString()), contentStr);
                    this.dataGridViewW1.Rows.Add(rowValues.ToArray());
                    i++;
                    continue;
                }
                if (element.ContentUnit == ContentUnit.per)
                    contentStr = element.Content.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(%)";
                else if (element.ContentUnit == ContentUnit.ppm)
                    contentStr = (element.Content * 10000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(ppm)";
                else
                    contentStr = (element.Content * 10).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(‰)";


                //if (atomElement != null && element.Content >= dShowLimitElement)
                //    this.dataGridViewW1.Rows.Add((isShowElemFullname) ? atomElement.AtomNameEN : atomElement.AtomName, element.Intensity.ToString("f" + WorkCurveHelper.SoftWareIntensityBit.ToString()), contentStr);
                //else if (element.Content >= dShowLimitElement)
                //    this.dataGridViewW1.Rows.Add(element.Caption, element.Intensity.ToString("f" + WorkCurveHelper.SoftWareIntensityBit.ToString()), contentStr);
                if (atomElement != null && element.Content >= dShowLimitElement)
                {

                    rowValues = new List<object>()
                        {
                            (isShowElemFullname) ? atomElement.AtomNameEN : atomElement.AtomName, 
                            element.Intensity.ToString("f" + WorkCurveHelper.SoftWareIntensityBit.ToString()), 
                            contentStr
                        };
                }
                //this.dataGridViewW1.Rows.Add((isShowElemFullname) ? atomElement.AtomNameEN : atomElement.AtomName, element.Intensity.ToString("f" + WorkCurveHelper.SoftWareIntensityBit.ToString()), contentStr);
                else if (element.Content >= dShowLimitElement)
                {
                    rowValues = new List<object>()
                        {
                            element.Caption, 
                            element.Intensity.ToString("f" + WorkCurveHelper.SoftWareIntensityBit.ToString()), 
                            contentStr
                        };
                }
                if (Lang.Model.CurrentLang.IsDefaultLang)
                {
                    if (rowValues != null)
                    {
                        rowValues.Insert(this.dataGridViewW1.Columns.IndexOf(this.ElementName) + 1, (atomElement == null ? string.Empty : atomElement.AtomNameCN) + element.Caption);
                    }
                }
                //this.dataGridViewW1.Rows.Add(element.Caption, element.Intensity.ToString("f" + WorkCurveHelper.SoftWareIntensityBit.ToString()), contentStr);
                this.dataGridViewW1.Rows.Add(rowValues.ToArray());
                valuse[i] = contentStr;
                i++;
            }
            if (_dataSource.Items.ToList().Exists(delegate(CurveElement v) { return v.Caption.ToLower() == "au" && v.Content >= dShowLimitElement; }))
            {
                chKShowKValue.Visible=this.labelInformation.Visible = true;
                this.labelInformation.Location = new Point(chKShowKValue.Width + chKShowKValue.Location.X + 4, this.labelInformation.Location.Y);
                //labelInformation.Text = Info.IncludingAu + ":" + (_dataSource.Items.ToList().Find(w => w.Caption.ToUpper().Equals("AU")).Content * 24 / WorkCurveHelper.KaratTranslater).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "K";
                labelInformation.Text = Info.IncludingAu + ":" + (_dataSource.Items.ToList().Find(w => w.Caption.ToUpper().Equals(_dataSource.MainElementToCalcKarat.ToUpper())).Content * 24 / WorkCurveHelper.KaratTranslater).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "K";
            }
            else
            {
                chKShowKValue.Visible =this.labelInformation.Visible= false;
            }
            foreach (var custom in _dataSource.CustomFields)
            {
                double value = double.Epsilon;
                TabControlHelper.CustomFieldByFortum(custom.Expression, elementName, valuse, 0, out value);
                this.dataGridViewW1.Rows.Add(Lang.Model.CurrentLang.IsDefaultLang ? new object[] { custom.Name, "", "", value } : new object[] { custom.Name, "", value });
                //this.dataGridViewW1.Rows.Add(custom.Name, "", value);
            }
            textBoxResultWeight_TextChanged(null, null);
        }

        private void cobBoxShowBit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cobBoxShowBit.SelectedItem == null || _dataSource==null)
                return;
            WorkCurveHelper.SoftWareContentBit = int.Parse(this.cobBoxShowBit.SelectedItem.ToString());
            ReportTemplateHelper.SaveSpecifiedValue("SoftWareCaculate", "ContentBit", WorkCurveHelper.SoftWareContentBit.ToString());
            GetElementContent();
            textBoxResultWeight_TextChanged(null, null);
        }

        /// <summary>
        /// 加入曲线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnJoinCurve_Click(object sender, EventArgs e)
        {
            string strSampleName = this.textBoxSampleName.Text.Trim();
            if (_dataSource != null && strSampleName != string.Empty)
            {
                foreach (var element in _dataSource.Items)
                {
                    StandSample sample = element.Samples.ToList().Find(s => s.SampleName == strSampleName);
                    if (sample == null)
                    {
                        double dblDensity = Atoms.AtomList.Find(w => w.AtomName.ToUpper().Equals(element.Caption.ToUpper())).AtomDensity;
                        StandSample ss = StandSample.New.Init(strSampleName,DifferenceDevice.interClassMain.deviceMeasure.interfacce.ReturnEncoderValue.ToString("f2"),DifferenceDevice.interClassMain.deviceMeasure.interfacce.ReturnEncoderHeight.ToString("f2"), element.Intensity.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()), "0", "0", false, element.Caption, 0, 0, "", dblDensity,"0");
                        element.Samples.Add(ss);
                        element.Save();
                    }
                    else
                    {
                        sample.X = element.Intensity.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
                        sample.Save();
                    }
                }
            }
         
        }
        /// <summary>
        /// 新建曲线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewCurve_Click(object sender, EventArgs e)
        {
            string strSampleName = this.textBoxSampleName.Text.Trim();
            var deviceId = WorkCurveHelper.DeviceCurrent.Id;
            string sql = "select * from WorkCurve where Condition_Id in (select Id from condition where Type = 0 and Device_id = "
                + deviceId + ") and FuncType=" + (int)WorkCurveHelper.DeviceFunctype + " order by LOWER(Name)";
            Lephone.Data.Common.DbObjectList < WorkCurve > lstWorkCurve = WorkCurve.FindBySql(sql); 
            FrmCurveReName frmReName = new FrmCurveReName(string.Empty, lstWorkCurve);
            frmReName.TopMost = true;
            if (DialogResult.OK == frmReName.ShowDialog()&&frmReName.newDeviceName.Trim()!=string.Empty)
            {
                var wcTemp = WorkCurve.New.Init(frmReName.newDeviceName.Trim(), "", WorkCurveHelper.WorkCurveCurrent.CalcType, WorkCurveHelper.DeviceFunctype,
                  WorkCurveHelper.WorkCurveCurrent.IsAbsorb, WorkCurveHelper.WorkCurveCurrent.LimitThickness, WorkCurveHelper.WorkCurveCurrent.RemoveBackGround, WorkCurveHelper.WorkCurveCurrent.RemoveSum,
                    WorkCurveHelper.WorkCurveCurrent.RemoveEscape, false, false, -1, "", false, Info.strAreaDensityUnit, WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList[0].PrecTime,true);
               // var wcTemp = WorkCurve.New.Init(frmReName.newDeviceName.Trim(), "", CalcType.FP, WorkCurveHelper.DeviceFunctype, false, 0, false, false, false, false, false, 0, "", false, Info.strAreaDensityUnit);
                var calibrationParam = CalibrationParam.New.Init(false, 45, 1, false, 2, 1, false, 1, 1, false, 1, 1, false, "", false, 1, 0, 0);
                calibrationParam.WorkCurve = wcTemp;
                wcTemp.Condition = WorkCurveHelper.WorkCurveCurrent.Condition;
                wcTemp.ConditionName = WorkCurveHelper.WorkCurveCurrent.Condition.Name;
                wcTemp.FuncType = WorkCurveHelper.WorkCurveCurrent.FuncType;
                wcTemp.WorkRegion = WorkCurveHelper.WorkCurveCurrent.WorkRegion ;
                
                wcTemp.SimilarCurveId = 0;
                wcTemp.SimilarCurveName = "";
                wcTemp.Save();
              
                wcTemp.ElementList=ElementList.New.Init(WorkCurveHelper.WorkCurveCurrent.ElementList.IsUnitary,WorkCurveHelper.WorkCurveCurrent.ElementList.UnitaryValue,WorkCurveHelper.WorkCurveCurrent.ElementList.TubeWindowThickness,
                                    WorkCurveHelper.WorkCurveCurrent.ElementList.RhIsLayer,WorkCurveHelper.WorkCurveCurrent.ElementList.RhLayerFactor,WorkCurveHelper.WorkCurveCurrent.ElementList.DBlLimt,WorkCurveHelper.WorkCurveCurrent.ElementList.IsReportCategory);
                wcTemp.ElementList.MatchSpecListIdStr = "";
                wcTemp.ElementList.RefSpecListIdStr = "";
                wcTemp.ElementList.MainElementToCalcKarat = "Au";
                wcTemp.ElementList.LayerElemsInAnalyzer = WorkCurveHelper.WorkCurveCurrent.ElementList.LayerElemsInAnalyzer;
                foreach(var em in WorkCurveHelper.WorkCurveCurrent.ElementList.Items)
                {
                    CurveElement ce=CurveElement.New.Init(em.Caption,em.IsDisplay,em.Formula,em.AtomicNumber,em.LayerNumber,em.LayerNumBackUp,em.AnalyteLine,em.PeakLow,em.PeakHigh,em.BaseLow,em.BaseHigh,em.PeakDivBase,em.LayerDensity,
                        em.Intensity,em.Content,em.Thickness,em.IntensityWay,em.IntensityComparison,em.ComparisonCoefficient,em.BPeakLow,em.BPeakHigh,CalculationWay.Linear,em.FpCalculationWay,em.Flag==ElementFlag.Internal?ElementFlag.Added:em.Flag,em.LayerFlag,em.ContentUnit,
                                                        em.ThicknessUnit,em.SReferenceElements,em.SSpectrumData,em.SInfluenceElements,em.SInfluenceCoefficients,em.Asrat,em.Msthk,em.Loi,em.Limit,em.K1,em.K0,em.Error,em.ErrorK1,em.ErrorK0,em.RowsIndex,em.ElementSpecName,
                                                        em.DifferenceHelp, em.Color, em.ColorHelper, em.ConditionID, em.DistrubThreshold, em.IsOxide, em.IsShowContent, em.IsShowContent, em.IsAlert, em.Contentcoeff,em.ContentRealValue,em.ElementEncoderSpecName,em.ElementSpecNameNoFilter, em.SSpectrumDataNotFilter,em.IsShowDefineName,em.DefineElemName);
                    foreach (var refs in em.ElementRefs)
                    {
                        ElementRef ElementRef = ElementRef.New.Init(refs.Name, refs.IsRef, refs.RefConf);
                        ce.ElementRefs.Add(ElementRef);
                    }
                    ce.DevParamId = em.DevParamId;

                    double dblDensity = Atoms.AtomList.Find(w => w.AtomName.ToUpper().Equals(ce.Caption.ToUpper())).AtomDensity;
                    StandSample ss = StandSample.New.Init(strSampleName, DifferenceDevice.interClassMain.deviceMeasure.interfacce.ReturnEncoderValue.ToString("f2"), DifferenceDevice.interClassMain.deviceMeasure.interfacce.ReturnEncoderHeight.ToString("f2"), ce.Intensity.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()), ce.Content.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()), "0", true, ce.Caption, 0, 0, "", dblDensity, "0");
                    wcTemp.ElementList.Items.Add(ce);
                    //wcTemp.ElementList.Save();
                    ce.Samples.Add(ss);
                    if (wcTemp.CalcType == CalcType.EC)//ec要两点，插入0点
                    {
                        StandSample ss1 = StandSample.New.Init("0#","0","0", "0", "0", "0", true, ce.Caption, 0, 0, "", dblDensity,"0");
                        ce.Samples.Add(ss1);
                    }
                    wcTemp.ElementList.Items.Add(ce);
                   // ce.Save();
                }
                wcTemp.Save();
                WorkCurveHelper.WorkCurveCurrent = WorkCurve.FindById(wcTemp.Id);
                this.Close(); 
                DifferenceDevice.interClassMain.OpenWorkCurveLog(WorkCurveHelper.WorkCurveCurrent, 1);
            }

        }


        public void ContructCurveData()
        {
            this.ReSetDialogFont(new Font("Arial", 9.0f, FontStyle.Bold));
            this.btnJoinCurve.Visible = false;
            this.btnNewCurve.Visible = false;
            this.buttonMeasureSubmit.Visible = false;
            this.buttonMeasureCancel.Visible = false;
            //if (GP.CurrentUser.Role.RoleType != 0) cobBoxShowBit.Visible = false;
            this.dataGridViewW1.Rows.Clear();
            if (_dataSource == null || _dataSource.Items.Count <= 0) return;
            string[] elementName = new string[_dataSource.Items.Count];
            string[] valuse = new string[_dataSource.Items.Count];
            int i = 0;
            List<object> rowValues = null;
            #region //以前
            //foreach (var posGroup in _dataSource.Items.ToList().FindAll(w => w.IsShowElement).OrderBy(w => w.RowsIndex))
            //{
            //    CurveElement element = posGroup as CurveElement;
            //    Atom atomElement = Atoms.AtomList.ToList().Find(w => w.AtomName.ToLower() == element.Caption.ToLower());
            //    elementName[i] = element.Caption;
            //    string contentStr = "";
            //     if (atomElement != null && element.Content >= dShowLimitElement)
            //         this.dataGridViewW1.Rows.Add((isShowElemFullname) ? atomElement.AtomNameEN : atomElement.AtomName, contentStr, contentStr);
            //    else if (element.Content >= dShowLimitElement)
            //         this.dataGridViewW1.Rows.Add(element.Caption, contentStr, contentStr);
            //    i++;
            //}
            //if (_dataSource.Items.ToList().Find(w => w.IsShowContent && w.Caption.ToLower().Equals("au")) != null)
            //{
            //    chKShowKValue.Visible = true;
            //}
            //else chKShowKValue.Visible = this.labelInformation.Visible = false;
            //foreach (var custom in _dataSource.CustomFields)
            //{
            //    double value = double.Epsilon;
            //    TabControlHelper.CustomFieldByFortum(custom.Expression, elementName, valuse, 0, out value);
            //    this.dataGridViewW1.Rows.Add(custom.Name, "", "");
            //}
            #endregion
            foreach (var posGroup in _dataSource.Items.ToList().FindAll(w => w.IsShowElement).OrderBy(w => w.RowsIndex))
            {
                CurveElement element = posGroup as CurveElement;
                Atom atomElement = Atoms.AtomList.ToList().Find(w => w.AtomName.ToLower() == element.Caption.ToLower());
                elementName[i] = element.Caption;
                string contentStr = "";
                // if (atomElement != null && element.Content >= dShowLimitElement)
                //     this.dataGridViewW1.Rows.Add((isShowElemFullname) ? atomElement.AtomNameEN : atomElement.AtomName, contentStr, contentStr);
                //else if (element.Content >= dShowLimitElement)
                //     this.dataGridViewW1.Rows.Add(element.Caption, contentStr, contentStr);
                if (atomElement != null && element.Content >= dShowLimitElement)
                {

                    rowValues = new List<object>()
                        {
                            (isShowElemFullname) ? atomElement.AtomNameEN : atomElement.AtomName, 
                            contentStr, 
                            contentStr
                        };
                }
                else if (element.Content >= dShowLimitElement)
                {
                    rowValues = new List<object>()
                        {
                            element.Caption, 
                            contentStr, 
                            contentStr
                        };
                }
                if (Lang.Model.CurrentLang.IsDefaultLang)
                {
                    if (rowValues != null)
                    {
                        rowValues.Insert(this.dataGridViewW1.Columns.IndexOf(this.ElementName) + 1, (atomElement == null ? string.Empty : atomElement.AtomNameCN) + element.Caption);
                    }
                }
                this.dataGridViewW1.Rows.Add(rowValues.ToArray());
                i++;
            }
            if (_dataSource.Items.ToList().Find(w => w.IsShowContent && w.Caption.ToLower().Equals("au")) != null)
            {
                chKShowKValue.Visible = true;
            }
            else chKShowKValue.Visible = this.labelInformation.Visible = false;
            foreach (var custom in _dataSource.CustomFields)
            {
                double value = double.Epsilon;
                TabControlHelper.CustomFieldByFortum(custom.Expression, elementName, valuse, 0, out value);
                this.dataGridViewW1.Rows.Add(Lang.Model.CurrentLang.IsDefaultLang ? new object[] { custom.Name, "", "", "" } : new object[] { custom.Name, "", "" });
            }
        }
        ////public void RefreshResultData(ElementList elementlist)
        //{
        //    for (int i = 0; i < this.dataGridViewW1.RowCount; i++)
        //    {
        //        string strElem = this.dataGridViewW1.Rows[i].Cells[0].Value.ToString();
        //        Atom atoElement= Atoms.AtomList.ToList()
        //        if(isShowElemFullname) strElem=
        //    }

        //        foreach (var posGroup in _dataSource.Items.ToList().FindAll(w => w.IsShowElement).OrderBy(w => w.RowsIndex))
        //        {
        //            CurveElement element = posGroup as CurveElement;
        //            Atom atomElement = Atoms.AtomList.ToList().Find(w => w.AtomName.ToLower() == element.Caption.ToLower());
        //            elementName[i] = element.Caption;
        //            string contentStr = "";
        //            //铑是镀层
        //            if (_dataSource.RhIsLayer && element.Caption.ToUpper().Equals("RH"))
        //            {
        //                checkBoxWeight.Enabled = false;
        //                contentStr = element.Thickness.ToString("f" + WorkCurveHelper.ThickBit.ToString()) + "(um)";
        //                valuse[i] = contentStr;
        //                if (atomElement != null)
        //                    this.dataGridViewW1.Rows.Add((isShowElemFullname) ? atomElement.AtomNameEN : atomElement.AtomName, element.Intensity.ToString("f" + WorkCurveHelper.SoftWareIntensityBit.ToString()), contentStr);
        //                else if (element.Content >= dShowLimitElement)
        //                    this.dataGridViewW1.Rows.Add(element.Caption, element.Intensity.ToString("f" + WorkCurveHelper.SoftWareIntensityBit.ToString()), contentStr);
        //                i++;
        //                continue;
        //            }
        //            if (element.ContentUnit == ContentUnit.per)
        //                contentStr = element.Content.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(%)";
        //            else if (element.ContentUnit == ContentUnit.ppm)
        //                contentStr = (element.Content * 10000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(ppm)";
        //            else
        //                contentStr = (element.Content * 10).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(‰)";


        //            if (atomElement != null && element.Content >= dShowLimitElement)
        //                this.dataGridViewW1.Rows.Add((isShowElemFullname) ? atomElement.AtomNameEN : atomElement.AtomName, element.Intensity.ToString("f" + WorkCurveHelper.SoftWareIntensityBit.ToString()), contentStr);
        //            else if (element.Content >= dShowLimitElement)
        //                this.dataGridViewW1.Rows.Add(element.Caption, element.Intensity.ToString("f" + WorkCurveHelper.SoftWareIntensityBit.ToString()), contentStr);
        //            valuse[i] = contentStr;
        //            i++;
        //        }
        //    if (_dataSource.Items.ToList().Exists(delegate(CurveElement v) { return v.Caption.ToLower() == "au" && v.Content >= dShowLimitElement; }))
        //    {
        //        chKShowKValue.Visible = this.labelInformation.Visible = true;
        //        labelInformation.Text = Info.IncludingAu + ":" + (_dataSource.Items.ToList().Find(w => w.Caption.ToUpper().Equals("AU")).Content * 24 / KaratTranslater).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "K";
        //    }
        //    else
        //    {
        //        chKShowKValue.Visible = this.labelInformation.Visible = false;
        //    }
        //    foreach (var custom in _dataSource.CustomFields)
        //    {
        //        double value = double.Epsilon;
        //        TabControlHelper.CustomFieldByFortum(custom.Expression, elementName, valuse, 0, out value);
        //        this.dataGridViewW1.Rows.Add(custom.Name, "", value);
        //    }
        //}


        public void ReSetDialogFont(Font newFont)
        {
            //this.Font = newFont;
            //this.panel1.Font = newFont;
            //foreach(var ctrl in panel1.Controls)
            //{
            //    ((Control)ctrl).Font = newFont;
            //}
            this.dataGridViewW1.Font = newFont;
            
        }

        private void dataGridViewW1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewW1.SelectedRows.Count <= 0)
            {
                _SelElementName = string.Empty;
                return;
            }
            string elementfullName = dataGridViewW1.SelectedRows[0].Cells[ElementName.Index].Value.ToString();
            Atom atom=isShowElemFullname?Atoms.AtomList.Find(w=>w.AtomNameEN==elementfullName||w.AtomNameCN==elementfullName):Atoms.AtomList.Find(w=>w.AtomName==elementfullName);
            if (atom != null)
                _SelElementName = atom.AtomName;
        }

        private void btnExportSQL_Click(object sender, EventArgs e)
        {
            DifferenceDevice.interClassMain.OpenSQL();
            DifferenceDevice.interClassMain.AddToHistorySQL();
        }

        private void FrmMeasureResult_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WorkCurveHelper.IsSaveToDataBase)
                DifferenceDevice.interClassMain.CloseSQL();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnHotKey_Click(object sender, EventArgs e)
        {
            FrmHotSet _frmHot = new FrmHotSet();
            _frmHot.ShowDialog();
           
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == WorkCurveHelper.SaveHistoryKeys && WorkCurveHelper.IsSaveToDataBase)
            {
                DifferenceDevice.interClassMain.OpenSQL();
                DifferenceDevice.interClassMain.AddToHistorySQL();
                return true;
            }
            else
                return base.ProcessDialogKey(keyData);
        }
    }
   
}
