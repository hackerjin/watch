using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.UC
{
    public partial class UCSampleCal : Skyray.Language.UCMultiple
    {
        private CSampleCal m_SampleCal = new CSampleCal();
        DataTable dtblCal = new DataTable();
        private bool bQualified = false;
        
        public UCSampleCal()
        {
            InitializeComponent();
            dtblCal.Columns.Add(Info.ElementName);
            dtblCal.Columns.Add(Info.Content+"(S)");
            dtblCal.Columns.Add(Info.strError + "(S)");
            dtblCal.Columns.Add(Info.Content);
            dtblCal.Columns.Add(Info.strError);
            DifferenceDevice.interClassMain.DoOtherFormReceive +=new InterfaceClass.ReceiveDataElse(ReceiveData);
            DifferenceDevice.interClassMain.DoOtherFormEndTest += new InterfaceClass.EndTestElse(EndTest);
            m_SampleCal.LoadDatasFromFile(WorkCurveHelper.SampleCalPath);
            if (m_SampleCal.IsCSampleCalEnabled)
            {
                List<string> strElems = new List<string>();
                foreach (WorkCurve wc in WorkCurveHelper.CurrentWorkRegion.WorkCurves)
                {
                    if(wc.ElementList==null) continue;
                    foreach (CurveElement ce in wc.ElementList.Items)
                    {
                        if (strElems.Contains(ce.Caption)) continue;
                        strElems.Add(ce.Caption);
                    }
                }
                bool IsPlasticContinuous = (ReportTemplateHelper.LoadByParameterSpecifiedValue("System", "PlasticContinuous") == "0") ? false : true;
                if( (WorkCurveHelper.CurrentWorkRegion.RohsSampleType == RohsSampleType.CrClInPlastic
                    || WorkCurveHelper.CurrentWorkRegion.RohsSampleType == RohsSampleType.Polyethylene) && IsPlasticContinuous)
                { 
                    RohsSampleType rohsTypeFind=WorkCurveHelper.CurrentWorkRegion.RohsSampleType == RohsSampleType.CrClInPlastic?RohsSampleType.Polyethylene:RohsSampleType.CrClInPlastic;
                    WorkRegion wr = WorkRegion.FindOne(w => w.RohsSampleType == rohsTypeFind);
                    if (wr != null && wr.WorkCurves.Count > 0)
                    {
                        foreach (WorkCurve wc in wr.WorkCurves)
                        {
                            if (wc.ElementList == null) continue;
                            foreach (CurveElement ce in wc.ElementList.Items)
                            {
                                if (strElems.Contains(ce.Caption)) continue;
                                strElems.Add(ce.Caption);
                            }
                        }
 
                    }

                }
                lblSampleCalInfo.Text = Info.Workgion + ":\r\n " + WorkCurveHelper.CurrentWorkRegion.Name+"\r\n\r\n";
                lblSampleCalInfo.Text += Info.SampleName + ":\r\n " + m_SampleCal.ControlSampleName;
                foreach (CSampleCalData cscd in m_SampleCal.ListSampleDatas)
                {
                    if (!strElems.Contains(cscd.ElemCaption)) continue;
                    object[] row ={
                                  cscd.ElemCaption,
                                  cscd.ElemContent,
                                  cscd.Error
                               };
                    dtblCal.Rows.Add(row);
                }

                dgvwStandardDatas.DataSource = dtblCal;
                for (int i = 0; i < dgvwStandardDatas.Columns.Count; i++)
                {
                    dgvwStandardDatas.Columns[i].ReadOnly = true;
                    dgvwStandardDatas.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable; 
                }
            }
           
        }

        public void ReceiveData(ElementList elems, float totalTime, float usedTime)
        {
            progressBar1.Maximum = (int)totalTime;
            progressBar1.Value = (int)usedTime;
            bool brefresh = bool.Parse(System.Configuration.ConfigurationSettings.AppSettings["RefreshBySencond"]);
            if(!brefresh) return;
            for (int i = 0; i < dtblCal.Rows.Count; i++)
            {
               string Elem= dtblCal.Rows[i][Info.ElementName].ToString();
               CurveElement ce=elems.Items.ToList().Find(w=>w.Caption==Elem);
                if(ce==null) continue;
                string StrContent=string.Empty;
                switch(ce.ContentUnit)
                {
                    case ContentUnit.per:
                        StrContent=ce.Content.ToString("F"+WorkCurveHelper.SoftWareContentBit);
                        break;
                    case ContentUnit.permillage:
                        StrContent=(ce.Content*10).ToString("F"+WorkCurveHelper.SoftWareContentBit);
                        break;
                    case ContentUnit.ppm:
                        StrContent=(ce.Content*10000).ToString("F"+WorkCurveHelper.SoftWareContentBit);
                        break;
                    default:
                        StrContent = ce.Content.ToString("F" + WorkCurveHelper.SoftWareContentBit);
                        break;



                }
                dtblCal.Rows[i][Info.Content] = StrContent;
            }
 
        }

        public void EndTest(ElementList elems)
        {
            //progressBar1.Maximum = (int)totalTime;
            //progressBar1.Value = (int)usedTime;
            for (int i = 0; i < dtblCal.Rows.Count; i++)
            {
                string Elem = dtblCal.Rows[i][Info.ElementName].ToString();
                CSampleCalData csSample = m_SampleCal.ListSampleDatas.Find(w => w.ElemCaption == Elem);
                CurveElement ce = elems.Items.ToList().Find(w => w.Caption == Elem);
                if (ce == null) continue;
                double dblContent = 0;
                switch (ce.ContentUnit)
                {
                    case ContentUnit.per:
                        dblContent = ce.Content;
                        break;
                    case ContentUnit.permillage:
                        dblContent = ce.Content * 10;
                        break;
                    case ContentUnit.ppm:
                        dblContent = ce.Content * 10000;
                        break;
                    default:
                        dblContent = ce.Content;
                        break;
                }

                dtblCal.Rows[i][Info.Content] = dblContent.ToString("F" + WorkCurveHelper.SoftWareContentBit);
                string strerr=Math.Abs(100*(dblContent - csSample.ElemContent) / csSample.ElemContent).ToString("F" + WorkCurveHelper.SoftWareContentBit);
                dtblCal.Rows[i][Info.strError] = strerr;
                bQualified = bQualified && Convert.ToSingle(strerr) <= csSample.Error;
                dgvwStandardDatas.Rows[i].DefaultCellStyle.BackColor = Convert.ToSingle(strerr) > csSample.Error ? Color.Red : Color.White;
            }
            lblQualifiedInfo.Text=bQualified?Info.DeviceQualified:Info.DeviceUnqualified;
            this.Text =strSpecName;
            lblQualifiedInfo.ForeColor = !bQualified ? Color.Red : Color.Black;
            if(!bQualified)
            lblQualifiedInfo.Font = new Font(lblQualifiedInfo.Font.Name,this.lblQualifiedInfo.Font.Size,FontStyle.Bold);
            if (this.ParentForm != null)
            {
                int TextStartIndex = this.ParentForm.Text.IndexOf("(");
                if (TextStartIndex > 0) this.ParentForm.Text = this.ParentForm.Text.Substring(0, TextStartIndex);
                this.ParentForm.Text += "(" + strSpecName + ")";
            }
        }

        public override void ExcuteCloseProcess(params object[] objs)
        {
            DifferenceDevice.interClassMain.DoOtherFormReceive -= new InterfaceClass.ReceiveDataElse(ReceiveData);
            DifferenceDevice.interClassMain.DoOtherFormEndTest -= new InterfaceClass.EndTestElse(EndTest);

        }
        string strSpecName = string.Empty;
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (DifferenceDevice.interClassMain.startTest) return;
            bQualified = true;
            lblQualifiedInfo.Text = string.Empty;
            if (this.ParentForm != null)
            {
                int TextStartIndex = this.ParentForm.Text.IndexOf("(");
                if (TextStartIndex > 0) this.ParentForm.Text = this.ParentForm.Text.Substring(0, TextStartIndex);
            }
            if (!m_SampleCal.IsCSampleCalEnabled||dgvwStandardDatas.Rows.Count<=0)
            {
                Msg.Show(Info.strPleaseInspect);
                return;
            }
             string strFormat = string.Format(Info.PleaseCurveCalibrationSample + m_SampleCal.ControlSampleName);
             if (Msg.Show(strFormat, MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK) return;


            strSpecName = "ControlSample_"+DateTime.Now.ToString("yyyyMMdd_HHmmss");
            SpecListEntity specList = new SpecListEntity(strSpecName, strSpecName, DifferenceDevice.interClassMain.deviceMeasure.interfacce.ReturnEncoderValue, DifferenceDevice.interClassMain.deviceMeasure.interfacce.ReturnEncoderHeight,
                "", 0, "", FrmLogon.userName, DateTime.Now, GP.UserName, SpecType.UnKownSpec, DifferenceDevice.DefaultSpecColor.ToArgb(), DifferenceDevice.DefaultSpecColor.ToArgb());
            specList.Loss = 0.0;
            List<WordCureTest> localWorkCurve = new List<WordCureTest>();
            WordCureTest test = new WordCureTest();
            test.WordCurveName =WorkCurveHelper.WorkCurveCurrent==null?WorkCurveHelper.CurrentWorkRegion.WorkCurves[0].Name: WorkCurveHelper.WorkCurveCurrent.Name;
            test.Spec = specList;
            test.SerialNumber = "0";
             localWorkCurve.Add(test);
            string DropTime = ReportTemplateHelper.LoadSpecifiedValueNoWait("TestParams", "DropTime");
            MeasureParams MeasureParams = new MeasureParams(1, int.Parse(DropTime != string.Empty ? DropTime : "0"), false);
            TestDevicePassedParams testDevicePassedParams = new TestDevicePassedParams(false, MeasureParams, localWorkCurve, WorkCurveHelper.DeviceCurrent.IsAllowOpenCover,
                              SpecType.UnKownSpec, "", false, true);
            DifferenceDevice.MediumAccess.ExcuteTestStart(testDevicePassedParams);

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (DifferenceDevice.interClassMain.startTest)
            {
                DifferenceDevice.MediumAccess.TestStop();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (DifferenceDevice.interClassMain.startTest)
                return;
            this.dialogResult = DialogResult.Cancel;
            if (this.ParentForm != null)
                this.ParentForm.DialogResult = DialogResult.Cancel;
            this.dialogResult = DialogResult.Cancel;
            EDXRFHelper.GotoMainPage(this);
        }

        private void UCSampleCal_Load(object sender, EventArgs e)
        {
            if (this.ParentForm != null) this.ParentForm.ControlBox = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.ParentForm == null) return;
            string filename = WorkCurveHelper.ExcelPath + "/" + strSpecName + ".xls";
            using (Bitmap bmp = new Bitmap(this.ParentForm.Width, this.ParentForm.Height))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.CopyFromScreen(this.ParentForm.Left, this.ParentForm.Top, 0, 0, new Size(this.ParentForm.Width, this.ParentForm.Height));
                    if (!System.IO.File.Exists(filename)) System.IO.File.Create(filename).Close();
                    Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook(Aspose.Cells.FileFormatType.Excel97To2003);
                    workbook.Open(filename);
                    Aspose.Cells.Worksheet ws = workbook.Worksheets[0];
                     System.IO.MemoryStream mstream = new System.IO.MemoryStream();
                     bmp.Save(mstream, System.Drawing.Imaging.ImageFormat.Bmp);
                    int index = ws.Pictures.Add( 0, 0,mstream);
                    ws.Name = strSpecName;
                    workbook.Save(filename);
                }

            }
            if (System.IO.File.Exists(filename))
            DifferenceDevice.interClassMain.OpenPathThread(filename);
        }
    }
}
