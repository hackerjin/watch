using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.Controls;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;
using Skyray.EDX.Common.ReportHelper;
using System.Threading;

namespace Skyray.UC
{
    public partial class FrmStatisticsResult : Skyray.Language.MultipleForm
    {
        private double dShowLimitElement;

        public double AuKNew {
            set {
                if (value == -1) 
                { 
                    chKShowKValue.Visible = false; 
                    this.labelInformation.Visible = false; }
                else
                {
                    chKShowKValue.Visible = true;
                    this.labelInformation.Visible = true;
                }
                this.labelInformation.Text = Info.IncludingAu + ":" + (value * 24 / WorkCurveHelper.KaratTranslater).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "K";
        } }


        public DataTable dataStatics
        {
            set
            {
                if (value != null)
                {
                    this.dgvStaticsResult.DataSource = value;
                }
            }
        }

        public DataTable dataMeasureResult
        {
            set
            {
                if (value != null)
                    this.dgvMeasureResult.DataSource = value;
            }
        }

        public FrmStatisticsResult()
        {
            InitializeComponent();
            chKShowKValue.Checked = true;
            dShowLimitElement = Convert.ToDouble(ReportTemplateHelper.LoadSpecifiedValue("ShowLimitElement", "Limit"));
            this.TopMost = true;
            if (WorkCurveHelper.IsSaveToDataBase)
                btnExportToSQL.Visible = true;
            else
                btnExportToSQL.Visible = false;
        }

        private void buttonStatisSubmit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonStaticsCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonReport_Click(object sender, EventArgs e)
        {
            var item = WorkCurveHelper.NaviItems.Find(w => w.Name == "StorePrint");
            item.excuteRequire(null);
            //DifferenceDevice.interClassMain.SaveExcel(null, 0);
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
            //else if (ExcelTemplateParams.iTemplateType == 6 && DifferenceDevice.IsAnalyser)
            //{
            //    if (DifferenceDevice.interClassMain.reportThreadManage == null) return;
            //    List<long> hisRecordidList = new List<long>();
            //    foreach (HistoryRecord his in DifferenceDevice.interClassMain.recordList) hisRecordidList.Add(his.Id);  

            //    string SaveReportPath = DifferenceDevice.interClassMain.reportThreadManage.GetHistoryRecordReport(hisRecordidList, 0, true,false);

                
            //    //DifferenceDevice.interClassMain.SaveHistoryRecordExcelPDF(true, false, true, selectLong, ref SaveReportPath,0);

            //    if (SaveReportPath == "") return;

            //    if (Skyray.Controls.SkyrayMsgBox.Show(PrintInfo.SaveSuccess + Skyray.EDX.Common.Info.OpenExcelOrNot, Skyray.EDX.Common.Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            //    {
            //        //Help.ShowHelp(null, SaveReportPath);
            //        Thread thread = new Thread(new ParameterizedThreadStart(ShowExcel));
            //        thread.Priority = ThreadPriority.Highest;
            //        thread.Start(SaveReportPath);
            //    }
            //}
        }
        private void ShowExcel(Object obj)
        {
            System.Diagnostics.Process.Start(obj.ToString());
        }
        private void btnWriteReport_Click(object sender, EventArgs e)
        {
           
        }

        private void chKShowKValue_CheckedChanged(object sender, EventArgs e)
        {
            this.labelInformation.Visible = chKShowKValue.Checked;
        }

        private void btnPrintReport_Click(object sender, EventArgs e)
        {
            NaviItem item = WorkCurveHelper.NaviItems.Find(w=>w.Name=="Print");
            item.excuteRequire(null);
        }

        private void btnExportSQL_Click(object sender, EventArgs e)
        {
            DifferenceDevice.interClassMain.OpenSQL();
            DifferenceDevice.interClassMain.AddToStaticToSQL();
        }


        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == WorkCurveHelper.SaveHistoryKeys && WorkCurveHelper.IsSaveToDataBase)
            {
                DifferenceDevice.interClassMain.OpenSQL();
                DifferenceDevice.interClassMain.AddToStaticToSQL();
                return true;
            }
            else
                return base.ProcessDialogKey(keyData);
        }

        private void FrmStatisticsResult_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WorkCurveHelper.IsSaveToDataBase)
            {
                DifferenceDevice.interClassMain.CloseSQL();
            }
        }
    }
}
