using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Lephone.Data.Common;
using Skyray.EDXRFLibrary;
using Skyray.Controls;
using Skyray.EDX.Common;
using Skyray.Controls.ElementTable;
using System.Data.SQLite;
using Lephone.Data;
using System.IO;
using Lephone.Data.SqlEntry;
using System.Data;
using System.Collections.Generic;
using ZedGraph;

namespace Skyray.UC
{
    public partial class UCTrendElem : Skyray.Language.UCMultiple
    {
        private WorkCurve currentWorkCurve;

       // private Runner currentRunner;

      //  private DbObjectList<Runner> lstRunner;

        private List<PointPairList> lstppl;

        //private List<string> lstsmapleInfo;


        private List<DateTime> lstTestDate;

        private List<string> lstUnit;

        private List<Int64> lstHistoryId;

        private static string elemCaption=string.Empty;


        private int _rows;
        public int RowsH
        {
            get { return _rows; }
            set { _rows = value; }
        }

        private int _cols;
        public int ColsH
        {
            get { return _cols; }
            set { _cols = value; }
        }

        public bool IsReloadThread
        {
            set
            {
                if (value)
                {
                   RefreshTrendData();
                   
                }
            }
        }
        public UCTrendElem()
        {
            InitializeComponent();
            LoadLines();
            LoadCols();
           
        }

        private void LoadLines()
        {
            
            //dgLine.Rows.Clear();
            //for (int i = 1; i <= _rows; i++)
            //{
            //    dgLine.Rows.Add(i.ToString());
            //}
        }

        private void LoadCols()
        {

            //dgCol.Rows.Clear();
            //for (int i = 1; i <= _cols; i++)
            //{
            //    dgCol.Rows.Add(i.ToString());
            //}
        }


      

        private void UCTrend_Load(object sender, EventArgs e)
        {
          
        }


       

      

        public void Draw(double[] datas,double ymax,double ymin, int xmax)
        {
            trendChart1.GraphPane.CurveList.Clear();
            trendChart1.XTitle = "次数";
            trendChart1.YTitle = "测量值";
            
            trendChart1.IXMaxChannel = xmax;
            trendChart1.IYMaxChannel = (int)Math.Ceiling(ymax + ymax * 0.2) <= 0 ? 100 : (int)Math.Ceiling(ymax + ymax * 0.2);
            trendChart1.GraphPane.YAxis.Scale.Min = (int)Math.Floor(ymin - ymin * 0.2) > 0 ? (int)Math.Floor(ymin - ymin * 0.2) : 0;
            trendChart1.GraphPane.YAxis.Scale.Max = trendChart1.IYMaxChannel;//(int)Math.Ceiling(ymax + ymax * 0.2);
            trendChart1.GraphPane.XAxis.Scale.Min = 1;
            trendChart1.GraphPane.XAxis.Scale.Max = xmax;
           // double[] datas = new double[_cols];
           // int startindex = index * _cols;
           // Array.ConstrainedCopy(WorkCurveHelper.MapAllData, startindex, datas, 0, _cols);
            PointPairList ppl = new PointPairList();
            
            for (int i = 0; i < datas.Length; i++)
            {
                ppl.Add(i + 1, datas[i]);
             }
            LineItem li = trendChart1.GraphPane.AddCurve("data", ppl, Color.DarkBlue, SymbolType.None);
          
            trendChart1.GraphPane.Fill = new Fill(Color.White, Color.FromArgb(200, 200, 255), 45.0f);

            trendChart1.GraphPane.XAxis.Scale.MajorStep = 1;

            trendChart1.GraphPane.YAxis.Scale.MinorStep = trendChart1.GraphPane.YAxis.Scale.Max / 8;//10
            trendChart1.GraphPane.YAxis.Scale.MajorStep = trendChart1.GraphPane.YAxis.Scale.Max / 4; //50

            trendChart1.AxisChange();//在数据变化时绘图             //更新图表            
            trendChart1.Invalidate();             //重绘控件            
            Refresh();
            trendChart1.Refresh();
           
        }

        private void comboBoxCurveName_SelectedIndexChanged(object sender, EventArgs e)
        {

            //Runner runnerList = lstRunner[comboBoxCurveName.SelectedIndex];
            //WorkCurve WorkCurveName = WorkCurve.FindOne(w => w.Name == runnerList.WorkCurveName);
            //GetSelCurve(WorkCurveName);

        }

        /// <summary>
        /// trendcharts
        /// 所有元素的曲线图
        /// </summary>
        /// <param name="WorkCurveName"></param>
        //private void GetSelCurve(WorkCurve WorkCurveName)
        //{
        //    if (this.trendChart1 == null) return;
        //    lstppl = new List<PointPairList>();
        //    trendChart1.GraphPane.CurveList.Clear();
        //    List<double> lstPositionSeat = new List<double>();
        //    trendChart1.XTitle = Info.MeasureTime;
        //    trendChart1.YTitle = Info.Content;
        //    //trendChart1.IYMaxChannel = 100;
        //    double maxchannel = 0;
        //    string sqlMaxY = "SELECT max(contextelementValue) FROM HISTORYELEMENT a  where a.HistoryRecord_Id in (select id from  historyrecord where WorkCurveId =" + WorkCurveName.Id;
        //    //string sqldate = (this.checkBoxWDate.Checked) ? " and datetime(specdate)>='" + Convert.ToDateTime(this.dateTimePickerStart.Value.ToString("D").ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "' and datetime(specdate)<'" + Convert.ToDateTime(this.dateTimePickerStart.Value.AddDays(1).ToString("D").ToString()).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss") + "')"
        //    //                                             : " and datetime(specdate)>='" + Convert.ToDateTime(DateTime.Now.ToString("D").ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "' and datetime(specdate)<'" + Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("D").ToString()).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss") + "')";

        //   // object obj = Lephone.Data.DbEntry.Context.ExecuteScalar(sqlMaxY+ sqldate);
        //    object obj = Lephone.Data.DbEntry.Context.ExecuteScalar(sqlMaxY );
        //    if (obj == null || obj.ToString() == "")
        //    {
        //        Refresh();
        //        trendChart1.Refresh();
        //        return;
        //    }
        //   // if (obj != null || obj!="")
        //    {
        //        maxchannel = double.Parse(obj.ToString());
        //        trendChart1.IYMaxChannel = Convert.ToInt32(maxchannel) + Convert.ToInt32(maxchannel * 0.2);
        //    }
        //    bool bfirst = true;
        //    trendChart1.IsUseScroll = true;
        //    foreach (var elem in WorkCurveName.ElementList.Items)
        //    {
        //        PointPairList ppl = new PointPairList();
        //        if (!elem.IsShowElement) continue;
        //        DataSet dselem = GetSQL(WorkCurveName, elem.Caption);
        //        DataTable dtelem = dselem.Tables[0];
        //        List<string> lstelem = dtelem.AsEnumerable().Select(d => d.Field<string>("contextelementValue")).ToList();
        //        lstUnit = dtelem.AsEnumerable().Select(d => d.Field<string>("UnitReplace")).ToList();
        //        DataSet dsSampleInfo = GetDateAndName(WorkCurveName, elem.Caption);
        //        DataTable dtSampleInfo = dsSampleInfo.Tables[0];
        //        //lstsmapleInfo = dtSampleInfo.AsEnumerable().Select(w => w.Field<string>("samplename")).ToList();
        //        lstTestDate = dtSampleInfo.AsEnumerable().Select(d => d.Field<DateTime>("specdate")).ToList();
        //        lstHistoryId = dtSampleInfo.AsEnumerable().Select(d => d.Field<Int64>("id")).ToList();
                
        //       // if (lstelem.Count > 0 && lstsmapleInfo.Count > 0 && lstTestDate.Count > 0)
        //        if (lstelem.Count > 0 && lstTestDate.Count > 0)
        //        {   
        //            int cnt = 0;
        //            if (bfirst)
        //            {
        //                for (int i = 0; i < lstTestDate.Count; i++)
        //                {
        //                    double x = (new XDate(lstTestDate[i]));
        //                    ppl.Add(x, Convert.ToDouble(lstelem[i]));
        //                    lstPositionSeat.Add(x);

        //                }
        //                bfirst = false;
        //            }
        //            else
        //            {
        //                for (int i = 0; i < lstTestDate.Count; i++)
        //                {
        //                    double x = (new XDate(lstTestDate[i]));
        //                    ppl.Add(x, Convert.ToDouble(lstelem[i]));
        //                }
        //            }

        //            LineItem li = trendChart1.GraphPane.AddCurve(elem.Caption, ppl, Color.FromArgb(elem.Color), SymbolType.None);
        //            trendChart1.GraphPane.Fill = new Fill(Color.White, Color.FromArgb(200, 200, 255), 45.0f);             //以上生成的图标X轴为数字，下面将转换为日期的文本            
                 
                   
        //        }

              

        //    }
        //    //if (currentRunner.IsHSO)
        //    //{
        //    //    PointPairList ppl = new PointPairList();
        //    //    DataSet dsHSO = GetDateAndHSO(WorkCurveName);
        //    //    DataTable dtHSO = dsHSO.Tables[0];
        //    //    //lstsmapleInfo = dtSampleInfo.AsEnumerable().Select(w => w.Field<string>("samplename")).ToList();
        //    //    lstTestDate = dtHSO.AsEnumerable().Select(d => d.Field<DateTime>("specdate")).ToList();
        //    //    List<double> lstHSOValue = dtHSO.AsEnumerable().Select(d => d.Field<double>("HSOValue")).ToList();
        //    //    List<double> lstHSOConductivity = dtHSO.AsEnumerable().Select(d => d.Field<double>("HSOConductivity")).ToList();

        //    //    lstHistoryId = dtHSO.AsEnumerable().Select(d => d.Field<Int64>("id")).ToList();
        //    //    if (lstTestDate.Count > 0)
        //    //    {
        //    //        for (int i = 0; i < lstTestDate.Count; i++)
        //    //        {
        //    //            double x = (new XDate(lstTestDate[i]));
        //    //            ppl.Add(x, Convert.ToDouble(lstHSOValue[i]));
                       

        //    //        }
        //    //        LineItem li = trendChart1.GraphPane.AddCurve("H2SO4", ppl, Color.Black, SymbolType.None);
        //    //        trendChart1.GraphPane.Fill = new Fill(Color.White, Color.FromArgb(200, 200, 255), 45.0f);             //以上生成的图标X轴为数字，下面将转换为日期的文本            


        //    //        for (int i = 0; i < lstTestDate.Count; i++)
        //    //        {
        //    //            double x = (new XDate(lstTestDate[i]));
        //    //            ppl.Add(x, Convert.ToDouble(lstHSOConductivity[i]));


        //    //        }
        //    //        LineItem lic = trendChart1.GraphPane.AddCurve("lstHSOConductivity", ppl, Color.SpringGreen, SymbolType.None);
        //    //        trendChart1.GraphPane.Fill = new Fill(Color.White, Color.FromArgb(200, 200, 255), 45.0f);             //以上生成的图标X轴为数字，下面将转换为日期的文本            

        //    //    }
        //    //}

        //    trendChart1.lstvalues = lstPositionSeat;
        //    ////double min = new XDate(this.checkBoxWDate.Checked ? Convert.ToDateTime(this.dateTimePickerStart.Value.ToString("D").ToString()): DateTime.Now.Date);
        //    ////double max = new XDate(this.checkBoxWDate.Checked ? Convert.ToDateTime(this.dateTimePickerStart.Value.AddDays(1).ToString("D")): Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("D").ToString()));
        //    //double min = new XDate(this.checkBoxWDate.Checked ? Convert.ToDateTime(this.dateTimePickerStart.Value.ToString("D").ToString()) : DateTime.Now.Date);
        //    //double max = new XDate(this.checkBoxWDate.Checked ? Convert.ToDateTime(this.dateTimePickerStart.Value.AddDays(1).ToString("D")) : Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("D").ToString()));



        //    trendChart1.GraphPane.YAxis.Scale.Min = 0;
        //    trendChart1.GraphPane.XAxis.Scale.Min = 0;
        //    trendChart1.GraphPane.XAxis.Scale.Max = 100;

        //    //trendChart1.GraphPane.YAxis.Scale.Max = 100;
        //    //trendChart1.GraphPane.XAxis.Scale.Min = min;
        //    //trendChart1.GraphPane.XAxis.Scale.Max = max;
        //    //trendChart1.GraphPane.XAxis.Type = AxisType.Date;
        //    //trendChart1.GraphPane.XAxis.Scale.Format = "HH:mm";
        //    //trendChart1.GraphPane.XAxis.Scale.FontSpec.Size = 10;
        //    //trendChart1.GraphPane.XAxis.MinorGrid.DashOff = 1f;
        //    //trendChart1.GraphPane.XAxis.MinorGrid.DashOn = 1f;

        //    trendChart1.AxisChange();//在数据变化时绘图             //更新图表            
        //    trendChart1.Invalidate();             //重绘控件            
        //    Refresh();
        //    trendChart1.Refresh();

        //}

        /// <summary>
        /// trendcharts
        /// 单个元素曲线图
        /// </summary>
        /// <param name="WorkCurveName"></param>
        private void GetSelElem(WorkCurve WorkCurveName, string elem)
        {

           //// if (this.trendChart1 == null) return;
           //// List<double> lstPositionSeat = new List<double>();
           //// trendChart1.GraphPane.CurveList.Clear();
           //// trendChart1.XTitle = Info.MeasureTime;
           //// trendChart1.YTitle = Info.Content;
           //// CurveElement currentElem = WorkCurveName.ElementList.Items.ToList().Find(w => w.Caption == elem);
           //// double maxchannel = 0;
           //// if (currentElem != null)
           //// {
           ////     lstppl = new List<PointPairList>();
            
           ////     //trendChart1.IYMaxChannel = 100;
           ////     string sqlMaxY = "SELECT max( cast (contextelementValue as decimal)) FROM HISTORYELEMENT a  where a.HistoryRecord_Id in (select id from  historyrecord where WorkCurveId =" + WorkCurveName.Id;
           ////     string sqldate = (this.checkBoxWDate.Checked) ? " and datetime(specdate)>='" + Convert.ToDateTime(this.dateTimePickerStart.Value.ToString("D").ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "' and datetime(specdate)<'" + Convert.ToDateTime(this.dateTimePickerStart.Value.AddDays(1).ToString("D").ToString()).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss") + "')"
           ////                            : " and datetime(specdate)>='" + Convert.ToDateTime(DateTime.Now.ToString("D").ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "' and datetime(specdate)<'" + Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("D").ToString()).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss") + "')";
           ////     string endsql = " and elementName ='" + elem + "' ";
           ////     object obj = Lephone.Data.DbEntry.Context.ExecuteScalar(sqlMaxY + sqldate + endsql);
           ////     if (obj == null || obj.ToString() == "")
           ////     {
           ////         Refresh();
           ////         trendChart1.Refresh();
           ////         return;
           ////     }
           ////     // if (obj != null || obj!="")
           ////     {
           ////         maxchannel = double.Parse(obj.ToString());// > currentElem.RangeHigh ? double.Parse(obj.ToString()) : currentElem.RangeHigh;
           ////         trendChart1.IYMaxChannel = Convert.ToInt32(maxchannel + maxchannel * 0.2) + 1;
           ////     }
           ////     bool bfirst = true;
           ////     trendChart1.IsUseScroll = true;

           ////     PointPairList pplelem = new PointPairList();
           ////     PointPairList pplLower = new PointPairList();
           ////     PointPairList pplHigh = new PointPairList();
           ////     DataSet dselem = GetSQL(WorkCurveName, elem);
           ////     DataTable dtelem = dselem.Tables[0];
           ////     List<string> lstelem = dtelem.AsEnumerable().Select(d => d.Field<string>("contextelementValue")).ToList();
           ////     lstUnit = dtelem.AsEnumerable().Select(d => d.Field<string>("UnitReplace")).ToList();
           ////     DataSet dsSampleInfo = GetDateAndName(WorkCurveName, elem);
           ////     DataTable dtSampleInfo = dsSampleInfo.Tables[0];
           ////     //lstsmapleInfo = dtSampleInfo.AsEnumerable().Select(w => w.Field<string>("samplename")).ToList();
           ////     lstTestDate = dtSampleInfo.AsEnumerable().Select(d => d.Field<DateTime>("specdate")).ToList();
           ////     lstHistoryId = dtSampleInfo.AsEnumerable().Select(d => d.Field<Int64>("id")).ToList();

           ////     // if (lstelem.Count > 0 && lstsmapleInfo.Count > 0 && lstTestDate.Count > 0)

           ////     if (lstelem.Count > 0 && lstTestDate.Count > 0)
           ////     {


           ////         int cnt = 0;
           ////         //if (bfirst)
           ////         //{
           ////         for (int i = 0; i < lstTestDate.Count; i++)
           ////         {
           ////             double x = (new XDate(lstTestDate[i]));
           ////             pplelem.Add(x, Convert.ToDouble(lstelem[i]));
           ////             if (currentElem != null)
           ////             {
           ////                 //pplHigh.Add(x, currentElem.RangeHigh);
           ////                 //pplLower.Add(x, currentElem.RangeLower);

           ////                 pplHigh.Add(x, 100);
           ////                 pplLower.Add(x, 0);
           ////             }
           ////             lstPositionSeat.Add(x);

           ////         }
           ////         bfirst = false;
           ////         //}
           ////         //else
           ////         //{
           ////         //    for (int i = 0; i < lstTestDate.Count; i++)
           ////         //    {
           ////         //        double x = (new XDate(lstTestDate[i]));
           ////         //        pplelem.Add(x, Convert.ToDouble(lstelem[i]));
           ////         //    }
           ////         //}

           ////         trendChart1.GraphPane.AddCurve(elem, pplelem, Color.Blue, SymbolType.None);
           ////         trendChart1.GraphPane.AddCurve("L", pplLower, Color.Red, SymbolType.None);
           ////         trendChart1.GraphPane.AddCurve("H", pplHigh, Color.Orange, SymbolType.None);
           ////         trendChart1.GraphPane.Fill = new Fill(Color.White, Color.FromArgb(200, 200, 255), 45.0f);             //以上生成的图标X轴为数字，下面将转换为日期的文本            

           ////     }
           //// }
           ////// if (WorkCurveHelper.CurrentRunner.IsHSO)
           //// //if (elem == Info.HSO || elem == Info.HSOConductivity || elem == CurrentRunner.DefineName)
           //// //{
           //// //    PointPairList ppl = new PointPairList();
           //// //    PointPairList pplH = new PointPairList();
           //// //    PointPairList pplL = new PointPairList();
           //// //    DataSet dsHSO = GetDateAndHSO(WorkCurveName);
           //// //    DataTable dtHSO = dsHSO.Tables[0];
           //// //    //lstsmapleInfo = dtSampleInfo.AsEnumerable().Select(w => w.Field<string>("samplename")).ToList();
           //// //    lstTestDate = dtHSO.AsEnumerable().Select(d => d.Field<DateTime>("specdate")).ToList();
           //// //    List<double> lstHSOValue = dtHSO.AsEnumerable().Select(d => d.Field<double>("HSOValue")).ToList();
           //// //    List<double> lstHSOConductivity = dtHSO.AsEnumerable().Select(d => d.Field<double>("HSOConductivity")).ToList();
           //// //    //自定义值
           //// //    List<double> lstDefineValue = dtHSO.AsEnumerable().Select(d => d.Field<double>("DefineValue")).ToList();
           //// //    lstHistoryId = dtHSO.AsEnumerable().Select(d => d.Field<Int64>("id")).ToList();
           //// //    //if (lstTestDate.Count > 0)
           //// //    //{
           //// //    //    if (elem == Info.HSO)
           //// //    //    {
           //// //    //        if (lstHSOValue == null || lstHSOValue.Count <= 0) return;
           //// //    //        maxchannel = lstHSOValue.Max() > CurrentRunner.HSOMax ? lstHSOValue.Max() : CurrentRunner.HSOMax;
           //// //    //        trendChart1.IYMaxChannel = Convert.ToInt32(maxchannel) + Convert.ToInt32(maxchannel * 0.2);
             
           //// //    //        for (int i = 0; i < lstTestDate.Count; i++)
           //// //    //        {
           //// //    //            double x = (new XDate(lstTestDate[i]));
           //// //    //            ppl.Add(x, Convert.ToDouble(lstHSOValue[i]));
           //// //    //            pplH.Add(x, CurrentRunner.HSOMax);
           //// //    //            pplL.Add(x, CurrentRunner.HSOMin);
           //// //    //            lstPositionSeat.Add(x);
           //// //    //        }
           //// //    //         trendChart1.GraphPane.AddCurve("H2SO4", ppl, Color.Blue, SymbolType.None);
           //// //    //         trendChart1.GraphPane.AddCurve("L", pplL, Color.Red, SymbolType.None);
           //// //    //         trendChart1.GraphPane.AddCurve("H", pplH, Color.Orange, SymbolType.None);
           //// //    //        trendChart1.GraphPane.Fill = new Fill(Color.White, Color.FromArgb(200, 200, 255), 45.0f);             //以上生成的图标X轴为数字，下面将转换为日期的文本            

           //// //    //    }
           //// //    //    else if (elem == Info.HSOConductivity)
           //// //    //    {
           //// //    //        maxchannel = lstHSOConductivity.Max();
           //// //    //        trendChart1.IYMaxChannel = Convert.ToInt32(maxchannel) + Convert.ToInt32(maxchannel * 0.2);

           //// //    //        for (int i = 0; i < lstTestDate.Count; i++)
           //// //    //        {
           //// //    //            double x = (new XDate(lstTestDate[i]));
           //// //    //            ppl.Add(x, Convert.ToDouble(lstHSOConductivity[i]));
           //// //    //            pplH.Add(x, CurrentRunner.ConductivityMax);
           //// //    //            pplL.Add(x, CurrentRunner.ConductivityMin);

           //// //    //            lstPositionSeat.Add(x);
           //// //    //        }
           //// //    //        trendChart1.GraphPane.AddCurve("Conductivity", ppl, Color.SpringGreen, SymbolType.None);
           //// //    //        trendChart1.GraphPane.AddCurve("L", pplL, Color.Red, SymbolType.None);
           //// //    //        trendChart1.GraphPane.AddCurve("H", pplH, Color.Orange, SymbolType.None);
           //// //    //        trendChart1.GraphPane.Fill = new Fill(Color.White, Color.FromArgb(200, 200, 255), 45.0f);             //以上生成的图标X轴为数字，下面将转换为日期的文本            
           //// //    //    }
           //// //    //    else if (elem == CurrentRunner.DefineName)
           //// //    //    {
           //// //    //        if (lstDefineValue == null || lstDefineValue.Count <= 0) return;
           //// //    //        maxchannel = lstDefineValue.Max();
           //// //    //        trendChart1.IYMaxChannel = Convert.ToInt32(maxchannel) + Convert.ToInt32(maxchannel * 0.2);

           //// //    //        for (int i = 0; i < lstTestDate.Count; i++)
           //// //    //        {
           //// //    //            double x = (new XDate(lstTestDate[i]));
           //// //    //            ppl.Add(x, Convert.ToDouble(lstDefineValue[i]));
           //// //    //            pplH.Add(x, CurrentRunner.DefineMax);
           //// //    //            pplL.Add(x, CurrentRunner.DefineMin);
           //// //    //            lstPositionSeat.Add(x);
           //// //    //        }
           //// //    //        trendChart1.GraphPane.AddCurve(CurrentRunner.DefineName, ppl, Color.Blue, SymbolType.None);
           //// //    //        trendChart1.GraphPane.AddCurve("L", pplL, Color.Red, SymbolType.None);
           //// //    //        trendChart1.GraphPane.AddCurve("H", pplH, Color.Orange, SymbolType.None);
           //// //    //        trendChart1.GraphPane.Fill = new Fill(Color.White, Color.FromArgb(200, 200, 255), 45.0f);         
                        
           //// //    //    }
           //// //    //}
           //// //}

           //// trendChart1.lstvalues = lstPositionSeat;
           //// double min = new XDate(this.checkBoxWDate.Checked ? Convert.ToDateTime(this.dateTimePickerStart.Value.ToString("D").ToString()) : DateTime.Now.Date);
           //// double max = new XDate(this.checkBoxWDate.Checked ? Convert.ToDateTime(this.dateTimePickerStart.Value.AddDays(1).ToString("D")) : Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("D").ToString()));

           //// trendChart1.GraphPane.YAxis.Scale.Min = 0;
           //// //trendChart1.GraphPane.YAxis.Scale.Max = 100;
           //// trendChart1.GraphPane.XAxis.Scale.Min = min;
           //// trendChart1.GraphPane.XAxis.Scale.Max = max;
           //// trendChart1.GraphPane.XAxis.Type = AxisType.Date;
           //// trendChart1.GraphPane.XAxis.Scale.Format = "HH:mm";
           //// trendChart1.GraphPane.XAxis.Scale.FontSpec.Size = 10;
           //// //trendChart1.GraphPane.XAxis.MinorGrid.DashOff = 1f;
           //// //trendChart1.GraphPane.XAxis.MinorGrid.DashOn = 1f;

           //// trendChart1.AxisChange();//在数据变化时绘图             //更新图表            
           //// trendChart1.Invalidate();             //重绘控件            
           //// trendChart1.Reduction();
           //// Refresh();
           //// trendChart1.Refresh();
            

        }
        private void GetSearchLinePosition(double num, ref int position, ref double value)
        {


        }


        #region feiqi
        //private void GetSelCurve(WorkCurve WorkCurveName)
        //{
        //    if (this.trendChart1 == null) return;
        //    lstppl = new List<PointPairList>();
        //    trendChart1.GraphPane.CurveList.Clear();
        //    createPane(trendChart1, WorkCurveName);
        //}

        //public void createPane(ZedGraphControl zgc, WorkCurve WorkCurveName)
        //{
        //    GraphPane myPane = zgc.GraphPane;             //设置图表标题 和 x y 轴标题      
        //    myPane.Title.Text = "趋势图";
        //    myPane.XAxis.Title.Text = "X轴标题";
        //    myPane.YAxis.Title.Text = "Y轴标题";             //更改标题的字体        
        //    myPane.XAxis.Scale.Max = 133;
        //    myPane.YAxis.Scale.Max = 1500;
        //    FontSpec myFont = new FontSpec("Arial", 20, Color.Red, false, false, false);
        //    myPane.Title.FontSpec = myFont;
        //    myPane.XAxis.Title.FontSpec = myFont;
        //    myPane.YAxis.Title.FontSpec = myFont;             // 造一些数据，PointPairList里有数据对x，y的数组             
        //    foreach (var elem in WorkCurveName.ElementList.Items)
        //    {

        //        PointPairList ppl = new PointPairList();
        //        if (!elem.IsShowElement) continue;
        //        DataSet dselem = GetSQL(WorkCurveName, elem.Caption);
        //        DataTable dtelem = dselem.Tables[0];
        //        List<string> lstelem = dtelem.AsEnumerable().Select(d => d.Field<string>("contextelementValue")).ToList();

        //        DataSet dsSampleInfo = GetDateAndName(WorkCurveName, elem.Caption);
        //        DataTable dtSampleInfo = dsSampleInfo.Tables[0];
        //        List<string> lstsmapleInfo = dtSampleInfo.AsEnumerable().Select(w => w.Field<string>("samplename")).ToList();
        //        List<DateTime> lstTestDate = dtSampleInfo.AsEnumerable().Select(d => d.Field<DateTime>("specdate")).ToList();


        //        if (lstelem.Count > 0 && lstsmapleInfo.Count > 0 && lstTestDate.Count > 0)
        //        {
        //            //string maxvalue = dtelem.AsEnumerable().Max(d => Convert.ToDouble(d.Field<string>("contextelementValue"))).ToString();
        //            int cnt = 0;
        //            foreach (var a in lstelem)
        //            {

        //                ppl.Add(cnt, Convert.ToDouble(a));
        //                lstppl.Add(ppl);
        //                cnt++;
        //            }

        //            //for (int i = 0; i < lstTestDate.Count; i++)
        //            //{
        //            //    double x = (new XDate(lstTestDate[i]));
        //            //    ppl.Add(x, Convert.ToDouble(lstelem[i]));
        //            //}

        //            LineItem li = trendChart1.GraphPane.AddCurve(elem.Caption, ppl, Color.FromArgb(elem.Color), SymbolType.None);
        //        }
        //    }
        //    //LineItem myCurve = myPane.AddCurve("曲线1", list1, Color.Red, SymbolType.None);//填充图表颜色   
        //    //LineItem myCurve2 = myPane.AddCurve("曲线2", list2, Color.Blue, SymbolType.None);//填充图表颜色  
        //    myPane.Fill = new Fill(Color.White, Color.FromArgb(200, 200, 255), 45.0f);             //以上生成的图标X轴为数字，下面将转换为日期的文本            

        //    double min = new XDate(DateTime.Now.Date);
        //    double max = new XDate(Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("D").ToString()));
        //    myPane.XAxis.Scale.Min = min;
        //    myPane.XAxis.Scale.Max = max;
        //    myPane.XAxis.Type = AxisType.Date;
        //    myPane.XAxis.Scale.Format = "HH:mm";


        //    //点线中点与点之间的间隔

        //    myPane.XAxis.MinorGrid.DashOff = 1f;

        //    //点线中点的长度

        //    myPane.XAxis.MinorGrid.DashOn = 1f;

        //    myPane.XAxis.Scale.FontSpec.Size = 10;
        //    //画到zedGraphControl1控件中，此句必加             
        //    zgc.AxisChange();//在数据变化时绘图             //更新图表            
        //    zgc.Invalidate();             //重绘控件            
        //    Refresh();
        //}

        #endregion

        private string strElenent(string element)
        {
            string str =string.Empty;
            str = "(select contextelementValue from historyelement where  elementName=" + element +") as " + element;
            return  str;
        }

        private DataSet GetSQL(WorkCurve WorkCurveName,string elemcaption)
        {

            ////string sqldate = (this.checkBoxWDate.Checked) ? " and datetime(specdate)>='" + Convert.ToDateTime(this.dateTimePickerStart.Value.ToString("D").ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "' and datetime(specdate)<'" + Convert.ToDateTime(this.dateTimePickerStart.Value.AddDays(1).ToString("D").ToString()).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss") + "'"
            ////                                              : " and datetime(specdate)>='" + Convert.ToDateTime(DateTime.Now.ToString("D").ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "' and datetime(specdate)<'" + Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("D").ToString()).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss") + "'";

            ////string sqlHistory = "(select id from HistoryRecord b where b.WorkCurveId =" + WorkCurveName.Id + sqldate + " order by specdate)";
            ////string sql = "select elementName,contextelementValue,UnitReplace,HistoryRecord_Id from HistoryElement a where a.elementname='" + elemcaption + "' and a.HistoryRecord_Id in " + sqlHistory;
            //////string sqldate = (this.checkBoxWDate.Checked) ? " and datetime(specdate)>='" + Convert.ToDateTime(this.dateTimePickerStart.Value.ToString("D").ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "' and datetime(specdate)<'" + Convert.ToDateTime(this.dateTimePickerStart.Value.AddDays(1).ToString("D").ToString()).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss") + "'"
            //////                                             : " and datetime(specdate)>='" + Convert.ToDateTime(DateTime.Now.ToString("D").ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "' and datetime(specdate)<'" + Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("D").ToString()).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss") + "'";

            //////string sqlHistory = "(select id from HistoryRecord b where b.WorkCurveId =" + WorkCurveName.Id + sqldate + " order by specdate)";
            //////string sql = "select a.elementName,a.contextelementValue,a.unitValue,UnitReplace,HistoryRecord_Id from HistoryElement a where a.elementname='" + elemcaption + "' and a.HistoryRecord_Id in " + sqlHistory;
            
            ////DataSet ds = GetDataSet(sql);
            ////if (ds == null) return null;
            ////DataTable dt = ds.Tables[0];

            ////int rows = dt.Rows.Count;
            DataSet ds =new DataSet();
            return ds;
        }


        private DataSet GetDateAndName(WorkCurve WorkCurveName, string elemcaption)
        {
            ////string sqldate = (this.checkBoxWDate.Checked) ? " and datetime(specdate)>='" + Convert.ToDateTime(this.dateTimePickerStart.Value.ToString("D").ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "' and datetime(specdate)<'" + Convert.ToDateTime(this.dateTimePickerStart.Value.AddDays(1).ToString("D").ToString()).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss") + "'"
            ////                                             : " and datetime(specdate)>='" + Convert.ToDateTime(DateTime.Now.ToString("D").ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "' and datetime(specdate)<'" + Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("D").ToString()).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss") + "'";
            ////string sql = "select id,samplename,specdate from HistoryRecord  where WorkCurveId =" + WorkCurveName.Id + sqldate + " order by specdate";
            //////string sql = "select id,samplename,specdate,HSOValue,HSOConductivity from HistoryRecord  where WorkCurveId =" + WorkCurveName.Id + sqldate + " order by specdate";

            //// DataSet ds = GetDataSet(sql);
            //// if (ds == null) return null;
            //// DataTable dt = ds.Tables[0];

            //// int rows = dt.Rows.Count;

            //// return ds;

            DataSet ds = new DataSet();
            return ds;
        }


        private DataSet GetDateAndHSO(WorkCurve WorkCurveName)
        {
           //// string sqldate = (this.checkBoxWDate.Checked) ? " and datetime(specdate)>='" + Convert.ToDateTime(this.dateTimePickerStart.Value.ToString("D").ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "' and datetime(specdate)<'" + Convert.ToDateTime(this.dateTimePickerStart.Value.AddDays(1).ToString("D").ToString()).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss") + "'"
           ////                                              : " and datetime(specdate)>='" + Convert.ToDateTime(DateTime.Now.ToString("D").ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "' and datetime(specdate)<'" + Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("D").ToString()).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss") + "'";
           ////// string sql = "select id,samplename,specdate from HistoryRecord  where WorkCurveId =" + WorkCurveName.Id + sqldate + " order by specdate";
           //// string sql = "select id,samplename,specdate,HSOValue,HSOConductivity,DefineValue from HistoryRecord  where WorkCurveId =" + WorkCurveName.Id + sqldate + " order by specdate";

           //// DataSet ds = GetDataSet(sql);
           //// if (ds == null) return null;
           //// DataTable dt = ds.Tables[0];

           //// int rows = dt.Rows.Count;

           //// return ds;

            DataSet ds = new DataSet();
            return ds;
        }


        private DataSet GetContextAndUnit(int history)
        {
            string sql = "select elementName,contextelementValue,UnitReplace from HistoryElement where HistoryRecord_Id =" + history;
            DataSet ds = GetDataSet(sql);
            if (ds == null) return null;
            DataTable dt = ds.Tables[0];

            int rows = dt.Rows.Count;

            return ds;
        }

        /// <summary>
        /// 根据sql语句，获取数据，返回datatable
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        private DataSet GetDataSet(string strSql)
        {
            DataSet ds = new DataSet();
            string connectionString = DbEntry.Context.Driver.ConnectionString;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand(strSql, connection))
                {
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    adapter.Fill(ds);
                }
            }
            return ds;
        }
        private void buttonWSearch_Click(object sender, EventArgs e)
        {

        }

        private void buttonWSearch_Click_1(object sender, EventArgs e)
        {

        }

        //private void radioButton1_CheckedChanged(object sender, EventArgs e)
        //{
        //    elemCaption = string.Empty;
        //    WorkCurve WorkCurveName = WorkCurve.FindOne(w => w.Name == ((RadioButton)sender).Text);
        //    currentWorkCurve = WorkCurveName;
        //    WorkCurveHelper.CurCheckRunnerInTrend = WorkCurveName.Name;
        //    //GetSelCurve(WorkCurveName);
        //    Runner ThisRunner = Runner.FindOne(w => w.Name == ((RadioButton)sender).Text);
        //    currentRunner = ThisRunner;
        //    InitControl(WorkCurveName, ThisRunner);
        //    LoadCurrentElem(0);
        //}


       

        System.Windows.Forms.Label lbl;
        private void InitControl(WorkCurve curWorkCurve)
        {

            ////dataViewElem.Rows.Clear();
            ////if(currentWorkCurve==null || currentWorkCurve.ElementList==null || currentWorkCurve.ElementList.Items==null || currentWorkCurve.ElementList.Items.Count==0) return;
            ////foreach (CurveElement elem in currentWorkCurve.ElementList.Items.OrderBy(d=>d.RowsIndex))
            ////{
            ////    dataViewElem.Rows.Add(elem.Caption);
            ////}
            //////if (ThisRunner.IsHSO)
            //////{
            //////    dataViewElem.Rows.Add(Info.HSO);
            //////    dataViewElem.Rows.Add(Info.HSOConductivity);
            //////}
            //////if (ThisRunner.IsShowDefine)
            //////{
            //////    dataViewElem.Rows.Add(ThisRunner.DefineName);
            //////}
           

        }

        private bool trendChart1_MouseMoveEvent(ZedGraphControl sender, MouseEventArgs e)
        {
            RefreshTrendInfo(elemCaption);

            return default(bool);
        }

        private void RefreshTrendInfo(string elemCaption)
        {
            ////if (dataViewElem.Rows.Count <= 0) return;
            ////if (lstTestDate != null && lstTestDate.Count > 0)
            ////{
            ////    int iCurrentRunner = trendChart1.PositionInList;

            ////    if (iCurrentRunner < 0) return;
            ////    Console.WriteLine(iCurrentRunner.ToString());
            ////    if (iCurrentRunner > 0)
            ////    {

            ////        lblShowTime.Text =  lstTestDate[iCurrentRunner].ToString("yyyy-MM-dd HH:mm:ss");

            ////        //Info.SpecDate + " : " +
            ////    }
            ////    else
            ////    {
            ////        lblShowTime.Text = "";//Info.SpecDate + " : " 
            ////    }

            ////    Int64 id = lstHistoryId[iCurrentRunner];

            ////    List<HistoryElement> hisElementList = HistoryElement.FindBySql("select * from historyelement where historyrecord_id=" + id);
              
            ////        HistoryElement hisElement = hisElementList.Find(delegate(HistoryElement v) { return v.elementName == elemCaption; });
            ////        if (hisElement != null)
            ////        {

            ////            lblShowContent.Text = hisElement.elementName + " : " + double.Parse(hisElement.contextelementValue).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());

            ////        }
            ////        else
            ////        {
            ////            lblShowContent.Text = " ";
            ////        }
                

            ////    //else
            ////    //{
            ////    //    HistoryRecord curhistory = HistoryRecord.FindById(lstHistoryId[iCurrentRunner]);
                    
            ////    //    if (elemCaption == Info.HSO)
            ////    //    {
            ////    //        lblShowContent.Text = Info.HSO + ":" + curhistory.HSOValue.ToString();
            ////    //    }
            ////    //    else if (elemCaption == Info.HSOConductivity)
            ////    //    {
            ////    //        lblShowContent.Text = Info.HSO + ":" + curhistory.HSOConductivity.ToString();
            ////    //    }
            ////    //    if (elemCaption == currentRunner.DefineName)
            ////    //    {
            ////    //        lblShowContent.Text = currentRunner.DefineName + ":" + curhistory.DefineValue;
            ////    //    }

            ////    //}

            ////    //if (WorkCurveHelper.CurrentRunner.IsHSO)
            ////    //{
            ////    //    HistoryRecord curhistory = HistoryRecord.FindById(lstHistoryId[iCurrentRunner]);
            ////    //    Control[] controls = grpReview.Controls.Find("lblConductivity", true);
            ////    //    if (controls != null && controls.Length > 0)
            ////    //    {
            ////    //        controls[0] = controls[0] as System.Windows.Forms.Label;
            ////    //        if (iCurrentRunner != 0)
            ////    //            controls[0].Text = Info.HSOConductivity + ":" + curhistory.HSOConductivity.ToString();
            ////    //        else
            ////    //            controls[0].Text = Info.HSOConductivity + ":";

            ////    //    }
            ////    //    controls = grpReview.Controls.Find("lblHSO", true);
            ////    //    if (controls != null && controls.Length > 0)
            ////    //    {
            ////    //        controls[0] = controls[0] as System.Windows.Forms.Label;
            ////    //        if (iCurrentRunner != 0)
            ////    //            controls[0].Text = Info.HSO + ":" + curhistory.HSOValue.ToString();
            ////    //        else
            ////    //            controls[0].Text = Info.HSO + ":";
            ////    //    }

            ////    //}

            ////}
        }


        public void RefreshTrendData()
        {
            //LoadRunner();
            LoadLines();
            LoadCols();
        }

        public void RefreshCheckTrend(RadioButton radio)
        {
           // radioButton1_CheckedChanged(radio, null);
          
        }

        private void lblTestDateValue_Click(object sender, EventArgs e)
        {

        }

        private void dataViewElem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadCurrentElem(e.RowIndex);
        }

        private void LoadCurrentElem(int rowIndex)
        {
            
            //if (dataViewElem.Rows.Count > 0)
            //{
            //    string elemstr = dataViewElem[0, rowIndex].Value.ToString();
            //    elemCaption = elemstr;
            //    GetSelElem(currentWorkCurve, elemstr);
            //}
        }

        private void dgLine_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Draw(e.RowIndex);
        }

       
       
      
    }

   
}
 