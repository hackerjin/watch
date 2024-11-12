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
using Skyray.EDX.Common.ReportHelper;
using Skyray.Language;

namespace Skyray.UC
{
    public partial class FrmUncertainty : MultipleForm
    {
        public FrmUncertainty(List<UncertaintySample> sampleList, UncertaintyResult ur, List<double> dblTestResults, UncertaintySample ucs, List<StandSample> sample, string caption, string sampleName, string displayContent)
        {

            InitializeComponent();
            _ucs = ucs;
            _sample = sample;
            _caption = caption;
            _sampleName = sampleName;
            _displayContent = displayContent;
            _dblTestResults = dblTestResults;
            _ur = ur;
            _sampleName = sampleName;
            _sampleList = sampleList;
            lblcertainty.Text = displayContent;
        }

        private List<StandSample> _sample;

        private string _caption;

        private string _sampleName;

        private string _displayContent;

        private UncertaintySample _ucs;

        private List<double> _dblTestResults;

        private UncertaintyResult _ur;

        private List<UncertaintySample> _sampleList;

        private void btnReport_Click(object sender, EventArgs e)
        {
            CalcUncertainty(_sampleList,_ur,_dblTestResults, _ucs,_sample, _caption, _sampleName, true);
        }

        private void btnPrintReport_Click(object sender, EventArgs e)
        {
            CalcUncertainty(_sampleList, _ur, _dblTestResults, _ucs, _sample, _caption, _sampleName, false);
        }

        private void CalcUncertainty(List<UncertaintySample> sampleList,UncertaintyResult ur, List<double> dblTestResults, UncertaintySample ucs, List<StandSample> sample, string caption, string sampleName, bool isNotPrint)
        {
            if (sampleList.Count == 0)
            {
                Msg.Show("缺少标准物质的数据！");
            }
            double A = 0;
            double B = 0;
            double[] Y = new double[sampleList.Count];
            double[] X = new double[sampleList.Count];
            int index = 0;
            foreach (UncertaintySample sp in sampleList)
            {
                X[index] = sp.SampleValue;
                Y[index] = sp.ActualValue;
                index++;
            }
            Report.LineFitting(X, Y, X.Length, ref A, ref B);

            DataRow dr = null;
            DataTable sDT = new DataTable();
            sDT.Columns.Add(new DataColumn("time"));
            sDT.Columns.Add(new DataColumn("标准物质"));
            sDT.Columns.Add(new DataColumn("标准值"));
            sDT.Columns.Add(new DataColumn("测量均值"));
            sDT.Columns.Add(new DataColumn("标准值不确定度"));
            foreach (UncertaintySample sp in sampleList)
            {
                dr = sDT.NewRow();
                dr["time"] = "";
                dr["标准物质"] = sp.SampleName;
                dr["标准值"] = sp.SampleValue;
                dr["测量均值"] = sp.ActualValue;
                dr["标准值不确定度"] = sp.SampleUncertainty;
                sDT.Rows.Add(dr);
            }

            int ii = 0;
            DataTable rDT = new DataTable();
            rDT.Columns.Add(new DataColumn("time"));
            rDT.Columns.Add(new DataColumn("测量值"));
            rDT.Columns.Add(new DataColumn("校正值"));
            foreach (double r in dblTestResults)
            {
                ii++;
                dr = rDT.NewRow();
                dr["time"] = ii;
                dr["测量值"] = r;
                dr["校正值"] = ((r - B) / A).ToString("F" + WorkCurveHelper.SoftWareContentBit);
                rDT.Rows.Add(dr);
            }
            double Ava = 0, RSD = 0;
            var value = from R in dblTestResults
                        group R by 1 into g
                        let dAva = g.Average()
                        let dSD = Math.Sqrt(g.Aggregate(0.0, (rt, p) => rt +
                               Math.Pow(p -
                               dAva
                               , 2)
                               / (g.Count() - 1))
                               )
                        select new
                        {//获取最大值、最小值、平均值、标准偏差数值
                            Ava = dAva,
                            RSD = (dAva <= 0 || dSD <= 0) ? dSD : (Math.Round(dSD, WorkCurveHelper.SoftWareContentBit, MidpointRounding.AwayFromZero)
                            / Math.Round(dAva, WorkCurveHelper.SoftWareContentBit, MidpointRounding.AwayFromZero) * 100)
                        };


            dr = rDT.NewRow();
            dr["time"] = "平均值";
            dr["测量值"] = "";
            dr["校正值"] = value.FirstOrDefault().Ava;
            rDT.Rows.Add(dr);

            dr = rDT.NewRow();
            dr["time"] = "RSD%";
            dr["测量值"] = "";
            dr["校正值"] = value.FirstOrDefault().RSD;
            rDT.Rows.Add(dr);


            Report report = new Report();
            //if (Skyray.EDX.Common.WorkCurveHelper.ReportVkOrVe == 1)
            //{
            //    report = new EtReport();
            //}
            //else
            //{
            //    report = new ExReport();
            //}
            report.GenerateUncertaintyReport(Environment.CurrentDirectory + "//HistoryExcelTemplate//Uncertainty.xls", sDT, rDT, isNotPrint, Y, X, ur, sampleName);
        }
    }
}
