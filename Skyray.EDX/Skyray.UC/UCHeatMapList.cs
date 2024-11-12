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
using System.IO;

using Lephone.Data.SqlEntry;
using System.Data.SQLite;

using Lephone.Data;

namespace Skyray.UC
{
    public partial class UCHeatMapList : Skyray.Language.UCMultiple
    {
        double[] DValues = new double[100];
       
        DataTable dt = new DataTable();

        private int CurrentTimes;

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


        private double[] _testDatas;
        public double[] TestDatas
        {
            get { return _testDatas; }

            set { _testDatas = value; }
        }

        /// <summary>
        /// 当前被选中的元素
        /// </summary>
        private string _currentElem;
        public string CurrentElem
        {
            get { return _currentElem; }

            set { _currentElem = value; }
        }


        private double _currentValue;
        public double CurrentValue
        {
            get { return _currentValue; }

            set { _currentValue = value; }
        }



        private int IsCurrentRowsOrColumns = 0; //默认0为行，1为列
        private int RowOrColIndex = 0;  //index

        public void SetCurrentTimes(int times)
        {
            CurrentTimes = times;
        }
     
        public UCHeatMapList()
        {

           
            InitializeComponent();
                
            if (DifferenceDevice.CurCameraControl.skyrayCamera1.Mode != Skyray.Camera.SkyrayCamera.CameraMode.Move && DifferenceDevice.CurCameraControl.skyrayCamera1.Mode != Skyray.Camera.SkyrayCamera.CameraMode.Coee && DifferenceDevice.CurCameraControl.skyrayCamera1.Mode != Skyray.Camera.SkyrayCamera.CameraMode.lonely)
            {
                if (WorkCurveHelper.testedRows % WorkCurveHelper.testParamsBackup.MeasureParams.MeasureNumber != 0)
                {
                    RowsH = 1;
                    ColsH = WorkCurveHelper.testedRows;

                }
                else
                {

                    RowsH = WorkCurveHelper.testedRows / WorkCurveHelper.testParamsBackup.MeasureParams.MeasureNumber;
                    ColsH = WorkCurveHelper.testedRows / RowsH;
                }
            }
            else
            {
                RowsH = 1;
                ColsH = WorkCurveHelper.testedRows;
            }
  
        }


        private void UCHeatMapList_Load(object sender, EventArgs e)
        {

            //加载所有元素
            LoadAllElem();
            if (dgCurveElem.Rows.Count > 0)
            {
                dgCurveElem.FirstDisplayedScrollingRowIndex = 0;
               
                //此处为窗口打开时的一次初始化，后续只需要更新数据
                this.SetCurrentTimes(WorkCurveHelper.testedRows);
                
                ContructRowsData();
                LoadLines();
                
                //默认显示第一个元素
                _currentElem = dgCurveElem[0, 0].Value.ToString();

                //更新表格数据
                LoadOneElemInfo(_currentElem);

                //画热力图  cnt =行*列
                DrawMap(RowsH * ColsH);

                //折线图
                ucTrendElem1.RowsH = RowsH;
                ucTrendElem1.ColsH = ColsH;
                IsCurrentRowsOrColumns = 0;
                RowOrColIndex = 0;
                DrawRowsAndCols(IsCurrentRowsOrColumns, RowOrColIndex);
            }
        }


        //测量后刷新数据
        public void RefreshData()
        {
            if (WorkCurveHelper.WorkCurveCurrent.ElementList == null || WorkCurveHelper.WorkCurveCurrent.ElementList == null || WorkCurveHelper.WorkCurveCurrent.ElementList.Items == null || WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count <= 0)
                return;
            if (_currentElem == null || _currentElem == "")
            {
                if (dgCurveElem[0, 0] != null && dgCurveElem[0, 0].Value != null)
                    _currentElem = dgCurveElem[0, 0].Value.ToString();
                else
                    return;

            }
            LoadOneElemInfo(_currentElem);

            DrawMap(_rows * _cols);

            DrawRowsAndCols(IsCurrentRowsOrColumns, RowOrColIndex);

        }


        private void LoadLines()
        {
            dgLine.Rows.Clear();
            for (int i = 1; i <= _rows; i++)
            {
                dgLine.Rows.Add(i.ToString());
            }

            dgCol.Rows.Clear();
            for (int i = 1; i <= _cols; i++)
            {
                dgCol.Rows.Add(i.ToString());
            }
        }

       

        public void ReloadHeatMap()
        {
            UCHeatMapList_Load(null, null);
        }


        private void DrawMap(int count)
        {
            MockDatasGen datasGen;
            Console.WriteLine("Create some mock datas");
            //datasGen = new MockDatasGen(WIDTH, HEIGHT);
             int WIDTH = 45 * _cols;
             int HEIGHT = 40 *_rows;
             double[] alldata = new double[_cols * _rows];
             int cnt = _testDatas.Length;
             Array.ConstrainedCopy(_testDatas, 0, alldata, 0, cnt);
            datasGen = new MockDatasGen(WIDTH, HEIGHT);
            List<DataType> datas = datasGen.CreateMockDatas(count, alldata, _cols);

            Console.WriteLine("Set datas");
            HeatMapImage heatMapImage = new HeatMapImage(WIDTH, HEIGHT, 200, 30);
            heatMapImage.SetDatas(datas);

            Console.WriteLine("Calculate and generate heatmap");
            Bitmap img = heatMapImage.GetHeatMap();

            panel1.BackgroundImage = img;
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
        }


 
        private void LoadAllElem()
        {
           
            if (WorkCurveHelper.WorkCurveCurrent != null && WorkCurveHelper.WorkCurveCurrent.ElementList != null
                && WorkCurveHelper.WorkCurveCurrent.ElementList.Items != null
                && WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count > 0)
            {
                this.ucTrendElem1.trendChart1.GraphPane.Title.Text = WorkCurveHelper.WorkCurveCurrent.Name;
                dgCurveElem.Rows.Clear();
                foreach (CurveElement elem in WorkCurveHelper.WorkCurveCurrent.ElementList.Items.OrderBy(d => d.RowsIndex))
                {
                    if (WorkCurveHelper.WorkCurveCurrent.FuncType == FuncType.Thick)
                    {
                        if (elem.IsShowContent)
                            dgCurveElem.Rows.Add(elem.Caption);
                    }
                    else
                    {
                        if (elem.IsShowElement)
                            dgCurveElem.Rows.Add(elem.Caption);
                    }
                }

            }
        }


      


        private void LoadOneElemInfo(string elemName)
        {
            DataSet dselem = GetSQL(WorkCurveHelper.WorkCurveCurrent, elemName, CurrentTimes);
            DataTable dtelem = dselem.Tables[0];
            List<string> lstelem = dtelem.AsEnumerable().Select(d => d.Field<string>("contextelementValue")).ToList();
            _testDatas = Array.ConvertAll<string, double>(lstelem.ToArray(), s =>Math.Round(double.Parse(s),WorkCurveHelper.SoftWareContentBit));
          
            for (int i = 0; i < CurrentTimes; i++)
            {
                int curCol = (i) % ColsH;
                int curRow = i / ColsH;
                dt.Rows[curRow][curCol + 1] = _testDatas[i];
            }

        }

        private void ContructRowsData()
        {
            // dgvAllPoint.Rows.Clear();
            dt.Columns.Clear();
            dt.Rows.Clear();
            dt.Columns.Add("测试点", typeof(string));
            for (int m = 0; m < ColsH; m++)
            {
                dt.Columns.Add("第" + (m + 1).ToString() + "次");
            }
            double[] allData = new double[ColsH * RowsH];
            for (int i = 0; i < RowsH; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i + 1;

                double[] datas = new double[ColsH];
                int startindex = i * ColsH;
                Array.ConstrainedCopy(allData, startindex, datas, 0, ColsH);
                for (int j = 0; j < ColsH; j++)
                {
                    dr[j + 1] = " ";
                }
                dt.Rows.Add(dr);
            }

            this.dgvAllPoint.AutoGenerateColumns = true;
            this.dgvAllPoint.DataSource = dt;


        }


 
        private void dgCurveElem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            _currentElem = dgCurveElem[0, e.RowIndex].Value.ToString();
            RefreshData();
           
        }

        private void dgLine_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex<0) return;
            IsCurrentRowsOrColumns = 0;   //行
            RowOrColIndex = e.RowIndex;   //第几行
            DrawRowsAndCols(0, RowOrColIndex);

            
        }

        private void dgCol_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            IsCurrentRowsOrColumns = 1;   //行
            RowOrColIndex = e.RowIndex;   //第几行

            DrawRowsAndCols(1, RowOrColIndex);

         
            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">画行0， 画列1</param>
        /// <param name="index">序号</param>
        private void DrawRowsAndCols(int type, int rowIndex)
        {
            int totalcnt = 0;
            List<double> lstdata = new List<double>();
            if (type == 0)
            {
                totalcnt = _cols;
                for (int i = 1; i <= _cols; i++)
                {
                    if (dt.Rows[rowIndex][i].ToString().Trim() != "")
                    {
                        lstdata.Add(double.Parse(dt.Rows[rowIndex][i].ToString()));
                    }

                }

            }
            else
            {
                totalcnt = _rows;
                for (int i = 0; i < _rows; i++)
                {

                    if (dt.Rows[i][rowIndex + 1].ToString().Trim() != "")
                    {
                        lstdata.Add(double.Parse(dt.Rows[i][rowIndex + 1].ToString()));
                    }

                }
            }


            if (lstdata.Count > 0)
            {
                double[] datas = lstdata.ToArray();

                double max = lstdata.Max();
                double min = lstdata.Min();
                ucTrendElem1.Draw(datas, max, min, totalcnt);
            }
        }

        //获取数据
        private DataSet GetSQL(WorkCurve WorkCurveName, string elemcaption, int currenttimes)
        {
            string sqlHistoryid = " (select id from (select * from HistoryRecord where workcurveid =" + WorkCurveName.Id + "  order by id desc limit " + currenttimes + ") as a order by a.id ); ";
            string sql = "select elementName,contextelementValue,HistoryRecord_Id from HistoryElement a where a.elementname='" + elemcaption + "' and a.HistoryRecord_Id in " + sqlHistoryid;
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
       
    }
}
