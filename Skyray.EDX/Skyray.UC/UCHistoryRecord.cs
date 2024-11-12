using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Aspose.Cells;
using Lephone.Data;
using Lephone.Data.Common;
using Lephone.Data.Definition;
using Lephone.Data.SqlEntry;
using Skyray.API;
using Skyray.Controls;
using Skyray.EDX.Common;
using Skyray.EDX.Common.Component;
using Skyray.EDX.Common.ReportHelper;
using Skyray.EDXRFLibrary;
using Skyray.EDXRFLibrary.Define;
using Skyray.EDXRFLibrary.Spectrum;
using Skyray.Language;
using Skyray.Print;
using ZedGraph;


namespace Skyray.UC
{
    /// <summary>
    /// 历史记录类
    /// </summary>
    public partial class UCHistoryRecord : Skyray.Language.UCMultiple
    {
        private List<string> ListElemetNames = new List<string>();

        public static List<Auto> AutoDic = new List<Auto>();

        public delegate void Print(string path);

        public event Print OnPrintTemplate;

        public Skyray.Controls.ButtonW btnExportSQL1
        {
            get {return this.btnExportSQL; }


            set { this.btnExportSQL = value; }


        }
        /// <summary>
        /// 存储查询到的所有工作曲线
        /// </summary>
        private List<WorkCurve> lstCurves = new List<WorkCurve>();

        private bool isXmlLoaded = false;

        private bool isThickuser = true; //thick用户模式

        //private Timer timer = new Timer();
        /// <summary>
        /// 构造函数
        /// </summary>

        public UCHistoryRecord()
        {
            InitializeComponent();
            if (this.DesignMode)
                return;
            //string strPath = Application.StartupPath + "\\DBConnection.ini";
            //string strData = "False";
            //if (File.Exists(strPath))
            //{
            //    System.Text.StringBuilder tempbuilder = new System.Text.StringBuilder(255);
            //    Skyray.API.WinMethod.GetPrivateProfileString("Param", "ExportToSQL", "False", tempbuilder, 255, strPath);
            //    strData = tempbuilder.ToString();
            //}
            ////btnExportSQL.Visible = Convert.ToInt32(strData) == 1 ? true : false; // btnExportSQL.Visible = Convert.ToBoolean(strData);
            //btnExportSQL.Visible = (strData == "1");

            ////测试dll和xml是否加载成功
            ////Form frm = new Form();
            ////btnSetHistoryRecordInfo.Visible =
            ////    isXmlLoaded = LoadSetHistoryRecordInfo(ref frm);
            ////frm.Dispose();

            if (DifferenceDevice.IsThick && GP.CurrentUser.Role.RoleType.ToString() == "2" && !WorkCurveHelper.IsCreateHistory)
            {
                panel2.Visible = false;
                panel2.Size = new Size(0, 0);
                panel3.Location = new System.Drawing.Point(0, 5);
                grouperSearchCondition.Size = new Size(0, 0);
                buttonWPrint.Visible = false;
                btwAllElements.Visible = false;
                comboBoxReportType.Visible = false;
                chkPrintHistory.Visible = false;
                btnUpLoad.Visible = false;
            }

            colPrePoints.Clear();
            colPreNormals.Clear();
            for (int i = 0; i < 100; i++)
            {
                colPrePoints.Add(new Point(-1, -1));

            }

            for (int i = 0; i < 100; i++)
            {
                colPreNormals.Add(false);

            }

        }


        public UCHistoryRecord(bool isShowSearch)
        {
            InitializeComponent();
         
            if (!isShowSearch)
            {
                grouperSearchCondition.Visible = false;
                dgvHistoryRecord.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 15, FontStyle.Bold);
                dgvHistoryRecord.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 15);
            }


            colPrePoints.Clear();
            colPreNormals.Clear();
            for (int i = 0; i < 100; i++)
            {
                colPrePoints.Add(new Point(-1, -1));

            }

            for (int i = 0; i < 100; i++)
            {
                colPreNormals.Add(false);

            }

        }

        private Dictionary<string, string> dAddColmun = new Dictionary<string, string>();

        /// <summary>
        /// 历史记录查找按钮处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWSearch_Click(object sender, EventArgs e)
        {

            if ((this.checkBoxWSampleName.Checked && this.textBoxWInputName.Text == ""))
            {
                Msg.Show(Info.HistoryConditionNotNull);
                return;
            }
            selectLong.Clear();
            this.dgvHistoryRecord.Columns.Clear();
            int totalPage = 0;
            int currentPage = 0;
            GetSepecifiedSearchCondition(totalPage, currentPage);
            if (this.dgvHistoryRecord.CurrentCell != null)
                this.dgvHistoryRecord.CurrentCell.Selected = false;

        }
        public void GetSepecifiedSearchCondition(int totalPage, int currentPage)
        {
            WinMethod.SendMessage(DifferenceDevice.interClassMain.deviceMeasure.interfacce.OwnerHandle, DeviceInterface.CUSTOM_MESSAGE, 0, 0);

            GetSepecifiedSearchConditionRefresh(totalPage, currentPage, dgvHistoryRecord);

            foreach (Form child in Application.OpenForms)
            {
                var control = child.Controls[0];
                //  if (control is UCHistoryRecordSplitScreen)
                if (control is UCHistoryRecord)
                {
                    foreach (Control c in control.Controls)
                    {
                        if (c is Skyray.Controls.XPander.Panel)
                        {
                            foreach (Control cc in c.Controls)
                            {
                                if ((cc is Skyray.Controls.DataGridViewW) && (cc.Name == "dgvHistoryRecord"))
                                {
                                    Skyray.Controls.DataGridViewW dgv = cc as Skyray.Controls.DataGridViewW;
                                    GetSepecifiedSearchConditionRefresh(totalPage, currentPage, dgv);
                                }
                            }
                        }
                    }

                }
            }
            WinMethod.SendMessage(DifferenceDevice.interClassMain.deviceMeasure.interfacce.OwnerHandle, DeviceInterface.CUSTOM_MESSAGE_HIDE, 0, 0);

        }

        public void RefreshHistoryByCaculate()
        {
            buttonWSearch_Click(null, null);
        }

        /// <summary>
        /// 更改设备时，刷新历史记录和工作曲线
        /// </summary>
        public void RefreshHistoryByDeviceChange()
        {
            UCHistoryRecord_Load(null, null);
        }

        /// <summary>
        /// 根据sql语句，获取数据，返回datatable
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        private DataTable GetData(string strSql)
        {
            DataTable dt = new DataTable();
            string connectionString = DbEntry.Context.Driver.ConnectionString;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand(strSql, connection))
                {
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    adapter.Fill(dt);
                }
            }
            return dt;
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

        /// <summary>
        /// 历史项设置处理功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void setItem_Click(object sender, EventArgs e)
        {
            //string itemStr = ReportTemplateHelper.LoadSpecifiedValue("HistoryItem", "Setting");
            //*******************自定义历史记录列表名
            string historyItemPathSave = string.Empty;
            string historyItemPath = ReportTemplateHelper.GetHistoryItemPath(((MyItem)comboBoxCurveName.SelectedItem).Value.ToString(),
                                                                                WorkCurveHelper.DeviceCurrent.Id,
                                                                                (int)WorkCurveHelper.DeviceFunctype,
                                                                                (int)((MyItem)comboBoxCurveName.SelectedItem).Type,
                                                                                out historyItemPathSave);
            string itemStr = ReportTemplateHelper.LoadHistoryItemSpecifiedValue(historyItemPath, "Setting");//标识 HistoryItem_仪器名_曲线名

            //*******************
            string[] str = itemStr.Split(',');
            List<string> listStr = str.ToList();
            string sColumnWidth = string.Empty;
            string ColumnIndex = string.Empty;
            foreach (ToolStripMenuItem tempItem in this.contextMenuStrip1.Items)
            {
                if (!tempItem.Name.Contains("折线"))
                {
                    if (!tempItem.Checked && !listStr.Contains(tempItem.Name))
                        listStr.Add(tempItem.Name);
                    else if (tempItem.Checked && listStr.Contains(tempItem.Name))
                        listStr.Remove(tempItem.Name);
                }
                if (this.dgvHistoryRecord.Columns.Contains(tempItem.Name))
                    sColumnWidth += tempItem.Name + ":" + this.dgvHistoryRecord.Columns[tempItem.Name].Width.ToString() + ",";
                if (this.dgvHistoryRecord.Columns.Contains(tempItem.Name))
                    ColumnIndex += tempItem.Name + ":" + this.dgvHistoryRecord.Columns[tempItem.Name].DisplayIndex.ToString() + ",";

            }
            if (sColumnWidth != string.Empty) sColumnWidth = sColumnWidth.Substring(0, sColumnWidth.Length - 1);
            itemStr = string.Empty;
            foreach (string tempStr in listStr)
                itemStr += tempStr + ",";
            //ReportTemplateHelper.SaveSpecifiedValue("HistoryItem", "Setting", itemStr);
            //ReportTemplateHelper.SaveSpecifiedValue("HistoryItem", "ColumnWidth", sColumnWidth);
            //ReportTemplateHelper.SaveSpecifiedValue("HistoryItem", "ColumnIndex", ColumnIndex);
            ReportTemplateHelper.SaveHistoryItemSpecifiedValue(historyItemPathSave, "Setting", itemStr);
            ReportTemplateHelper.SaveHistoryItemSpecifiedValue(historyItemPathSave, "ColumnWidth", sColumnWidth);
            ReportTemplateHelper.SaveHistoryItemSpecifiedValue(historyItemPathSave, "ColumnIndex", ColumnIndex);
            Msg.Show(Info.SetSuccess);
        }

        /// <summary>
        /// 点击下拉菜单项项的变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemMenu_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            item.Checked = !item.Checked;
            this.dgvHistoryRecord.Columns[item.Name].Visible = item.Checked;//历史记录列的可见性设定

            string historyItemPathSave = string.Empty;
            string historyItemPath = ReportTemplateHelper.GetHistoryItemPath((comboBoxCurveName.SelectedItem == null ? "-1" : ((MyItem)comboBoxCurveName.SelectedItem).Value.ToString()),
                                                                                WorkCurveHelper.DeviceCurrent.Id,
                                                                                (int)WorkCurveHelper.DeviceFunctype,
                                                                                (comboBoxCurveName.SelectedItem == null ? 0 : (int)((MyItem)comboBoxCurveName.SelectedItem).Type),
                                                                                out historyItemPathSave);
            string tempStr = ReportTemplateHelper.LoadHistoryItemSpecifiedValue(historyItemPath, "Setting");

            if (item.Name.Contains("折线"))
            {
                if (item.Checked)
                {
                    if (!tempStr.Contains(item.Name))
                        tempStr = tempStr + "," + item.Name;
                }
                else
                {
                    if (tempStr.Contains(item.Name))
                        tempStr = tempStr.Substring(0, tempStr.IndexOf(item.Name)) + tempStr.Substring(tempStr.IndexOf(item.Name) + item.Name.Length, tempStr.Length - tempStr.IndexOf(item.Name)-item.Name.Length);
                        
                }
                ReportTemplateHelper.SaveHistoryItemSpecifiedValue(historyItemPath, "Setting", tempStr);
            }

            if (item.Text != Info.ChoiceSetting)
                contextMenuStrip1.Show();
        }

        /// <summary>
        /// 激活历史记录加载数据
        /// </summary>
        /// <param name="str"></param>
        public void LoadRefreshUI(string str)
        {

            this.comboBoxCurveName.Text = str;
            UCHistoryRecord_Load(null, null);
        }


        //修改：何晓明 20110818 历史记录小数精度
        //int HistoryRecordDecimalMedian = 4;
        //
        /// <summary>
        /// 历史记录控件的加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int countryId = -1;
        private void UCHistoryRecord_Load(object sender, EventArgs e)
        {
         

            this.buttonWPrint.Visible = WorkCurveHelper.PrinterType == 0;
            //dateTimePickerStart.CustomFormat = DateTime. + "" + DateTimePickerFormat.Time;
            string k = System.Globalization.DateTimeFormatInfo.CurrentInfo.DateSeparator;
            dateTimePickerStart.CustomFormat = "yyyy" + k + "MM" + k + "dd" + " HH:mm";
            dateTimePickerEnd.CustomFormat = "yyyy" + k + "MM" + k + "dd" + " HH:mm";
            if (this.DesignMode)
                return;

            //if (GP.CurrentUser.Role.RoleType.ToString() == "2")
            //{
            //    btnExportSQL.Visible = false;
            //}
            //else
            //{
            //    btnExportSQL.Visible = true;

            //}
            
            btnExportSQL.Visible = true;

            try
            {
                countryId = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["CountryId"]);
            }
            catch
            { }
            if (DifferenceDevice.IsAnalyser && (ReportTemplateHelper.ExcelModeType == 6 || ReportTemplateHelper.ExcelModeType == 15) || countryId == 0 || DifferenceDevice.IsRohs) //日本贵金属软件开启pdf保存功能
            {
                comboBoxReportType.Visible = true;
                comboBoxReportType.SelectedIndex = 0;
            }
            else
                comboBoxReportType.Visible = false;


            if (DifferenceDevice.IsAnalyser && Application.ProductName.Equals("India") && GP.CurrentUser.Role.RoleType.ToString() == "2")//贵金属印度版，如果是普通用户，控制删除，清空按钮出现
            {
                buttonWDeleteCurrent.Visible = false;
                btWClear.Visible = false;
            }
            if (DifferenceDevice.IsRohs)
            {
                chkStatistic.Visible = false;
                chkStatistic.Checked = false;
            }
            chkStatistic.Text = Info.StatisticsInfo;
            buttonWExcel.Left = chkStatistic.Right + 2;

            DeleColmun();

            ROWS_PER_PAGE = ReportTemplateHelper.ShowNumber;

            //if (ReportTemplateHelper.ShowUnitType) this.checkShowUnit.Checked = true; else this.checkShowUnit.Checked = false;
            this.checkShowUnit.Checked = ReportTemplateHelper.ShowUnitType;
            if (!Lephone.Data.DbEntry.Context.GetTableNames().Contains("HistoryRecord"))
            {
                HistoryRecord.FindAll();
            }
            if (!Lephone.Data.DbEntry.Context.GetTableNames().Contains("HistoryElement"))
            {
                HistoryElement.FindAll();
            }
            //if (DifferenceDevice.IsXRF) checkShowUnit.Visible = false;
            //if (!DifferenceDevice.ConfigUI)
            //    return;
            if (!Lephone.Data.DbEntry.Context.GetTableNames().Contains("Condition"))
                Skyray.EDXRFLibrary.Condition.FindAll();
            string sql = "select * from WorkCurve a inner join Condition b on a.Condition_Id = b.Id inner join Device c on b.Device_Id=c.Id where c.Id=" + WorkCurveHelper.DeviceCurrent.Id;
            if (DifferenceDevice.IsXRF)
                sql += " and a.FuncType =" + (int)FuncType.XRF;
            else if (DifferenceDevice.IsThick)
                sql += " and a.FuncType =" + (int)FuncType.Thick;
            else if (DifferenceDevice.IsRohs)
                sql += " and a.FuncType=" + (int)FuncType.Rohs;
            var listCuve = WorkCurve.FindBySql(sql);
            lstCurves = listCuve;
            //var listCuve = WorkCurve.FindBySql(sql + " and  b.Device_Id=" + WorkCurveHelper.DeviceCurrent.Id + " and b.Type=0");
            this.comboBoxCurveName.Items.Clear();
            if (!DifferenceDevice.IsThick)
                this.comboBoxCurveName.Items.Add(new MyItem("--All--", -1, ItemType.Curve));
            foreach (WorkCurve workcurve in listCuve)
            {
                if (!DifferenceDevice.IsAnalyser && workcurve.Condition.Type == ConditionType.Intelligent)
                {
                    continue;
                }
                else if ((DifferenceDevice.IsAnalyser) && workcurve.Condition.Type == ConditionType.Intelligent)//只有贵金属和xrf有智能分析功能//DifferenceDevice.IsXRF ||
                {
                    this.comboBoxCurveName.Items.Add(new MyItem(Info.Intelligent, (int)workcurve.Id, ItemType.Curve));
                    continue;
                }
                //else if (DifferenceDevice.IsXRF && workcurve.Condition.Type == ConditionType.Intelligent)
                //{ 
                //    continue;
                //}
                this.comboBoxCurveName.Items.Add(new MyItem(workcurve.Name, (int)workcurve.Id, ItemType.Curve));
            }

            if (DifferenceDevice.IsRohs)
            {
                var listRegion = WorkRegion.FindBySql("select * from WorkRegion");
                foreach (WorkRegion region in listRegion)
                {
                    this.comboBoxCurveName.Items.Add(new MyItem(region.Caption, (int)region.Id, ItemType.Region));
                }
            }
            if (DifferenceDevice.IsRohs && WorkCurveHelper.CurrentWorkRegion != null)
                this.comboBoxCurveName.Text = WorkCurveHelper.CurrentWorkRegion.Caption;
            else if (WorkCurveHelper.WorkCurveCurrent != null)
                this.comboBoxCurveName.Text = WorkCurveHelper.WorkCurveCurrent.Name;
            else if (WorkCurveHelper.WorkCurveCurrent == null)
                this.comboBoxCurveName.Text = "--All--";
            if (DifferenceDevice.IsRohs && WorkCurveHelper.HistoryDefaultCurveType == 1)
            {
                this.comboBoxCurveName.Text = "--All--";
            }
            Lang.Model.LanguageChanged += new EventHandler(Model_LanguageChanged);
            if (this.ParentForm != null)
                this.ParentForm.FormClosing += (s, ex) => Skyray.Language.Lang.Model.DGVS.Remove(this.dgvHistoryRecord);

            this.dgvHistoryRecord.CMenu.Items[1].Visible =
            this.dgvHistoryRecord.CMenu.Items[2].Visible = false;

            buttonWSearch_Click(null, null);

            if (DifferenceDevice.IsXRF && !(this.Parent is Form))
            {
                btnUpLoad.Visible = false;
            }



            UCHistoryRecord temp = (UCHistoryRecord)WorkCurveHelper.ucHistoryRecord1;

            if (WorkCurveHelper.IsDBOpen)
                temp.btnExportSQL1.Visible = true;
            else
                temp.btnExportSQL1.Visible = false;

            chkShowAreaThick.Checked = WorkCurveHelper.WorkCurveCurrent.IsThickShowAreaThick;

            for (int index = 0; index < WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count; index++)
            {
                if (WorkCurveHelper.WorkCurveCurrent.ElementList.Items[index].IsShowElement)
                {
                    if (WorkCurveHelper.WorkCurveCurrent.ElementList.Items[index].ThicknessUnit == ThicknessUnit.ur)
                    {
                        cmbThicknessUnit.SelectedIndex = 0;
                    }
                    else if (WorkCurveHelper.WorkCurveCurrent.ElementList.Items[index].ThicknessUnit == ThicknessUnit.um)
                    {
                        cmbThicknessUnit.SelectedIndex = 1;
                    }
                    else
                    {
                        cmbThicknessUnit.SelectedIndex = 2;
                    }
                }
            }
            if (WorkCurveHelper.WorkCurveCurrent.AreaThickType == "g/m^2")
                cmbDensityUnit.SelectedIndex = 0;
            else
                cmbDensityUnit.SelectedIndex = 1;

            colPrePoints.Clear();
            colPreNormals.Clear();
            for (int i = 0; i < 100; i++)
            {
                colPrePoints.Add(new Point(-1, -1));

            }

            for (int i = 0; i < 100; i++)
            {
                colPreNormals.Add(false);

            }


        }

        private void textBoxWInputName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                this.textBoxWInputName.SelectAll();
                this.checkBoxWSampleName.Checked = true;
                this.comboBoxCurveName.Text = "--All--";
                buttonWSearch_Click(null, null);
            }
        }

        void Model_LanguageChanged(object sender, EventArgs e)
        {
            if (DifferenceDevice.IsThick && GP.CurrentUser.Role.RoleType.ToString() == "2" && !WorkCurveHelper.IsCreateHistory)
            {
                grouperSearchCondition.GroupTitle = "";
            }
            chkStatistic.Text = Info.StatisticsInfo;
            buttonWExcel.Left = chkStatistic.Right + 2;
            buttonWSearch_Click(null, null);
        }

        public struct MyItem
        {
            public string Text;
            public int Value;//你需要的值类型
            public ItemType Type;
            public MyItem(string text, int value, ItemType type)
            {
                this.Text = text;
                this.Value = value;
                this.Type = type;
            }
            public override string ToString()
            {
                return Text;
            }
        }

        public enum ItemType
        {
            Curve,
            Region
        }

        /// <summary>
        /// 删除指定的历史记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWDeleteCurrent_Click(object sender, EventArgs e)
        {
            if (selectLong.Count == 0)
            {
                return;
            }
            foreach (long seleid in selectLong)
            {
                SqlStatement sqlstate = new SqlStatement("delete from historycompanyotherinfo where history_id='" + seleid.ToString() + "';delete from HistoryRecord where Id=" + seleid.ToString() + " ;delete from historyelement where HistoryRecord_Id=" + seleid.ToString());
                Lephone.Data.DbEntry.Context.ExecuteNonQuery(sqlstate);
            }
            buttonWSearch_Click(null, null);
        }

        private void buttonWCancel_Click(object sender, EventArgs e)
        {
            //EDXRFHelper.GotoMainPage(this);
            if (selectLong.Count <= 0) return;
            HistoryRecord hr = HistoryRecord.FindById(selectLong[0]);
            DifferenceDevice.interClassMain.SaveCurrentMesureResultToExcel(hr);
        }

        #region 打印功能 （暂时复制粘贴）
        PrintDocument printDocument1 = new PrintDocument();
        GridPrinter gridPrinter;
        /// <summary>
        /// 打印历史记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWPrint_Click(object sender, EventArgs e)
        {

            List<int> colIndex = new List<int>();
            int ni2Index = 0;
            foreach (DataGridViewColumn column in this.dgvHistoryRecord.Columns)
            {
                if (column.Visible && column.Index > 0)
                {
                    colIndex.Add(column.Index);
                    if ((FpWorkCurve.thickMode == ThickMode.NiNi || FpWorkCurve.thickMode == ThickMode.NiCuNi || FpWorkCurve.thickMode == ThickMode.NiCuNiFe) && column.Name.Contains("Ni2"))
                    {
                        ni2Index = column.Index;
                        colIndex.Remove(column.Index);
                    }
                }
            }
            if (FpWorkCurve.thickMode == ThickMode.NiNi || FpWorkCurve.thickMode == ThickMode.NiCuNi || FpWorkCurve.thickMode == ThickMode.NiCuNiFe)
            {
                colIndex.Add(ni2Index);
            }



            List<int> rowIndex = new List<int>();
            foreach (DataGridViewRow row in this.dgvHistoryRecord.Rows)
            {
                if (row.Cells[0].Value != null && bool.Parse(row.Cells[0].Value.ToString()))
                {
                    rowIndex.Add(row.Index);
                }
            }
            if (colIndex.Count == 0 || rowIndex.Count == 0)
            {
                Msg.Show(Info.SelectHistoryRecord);
                return;
            }
            this.dgvHistoryRecord.ToPrintCols = colIndex;
            this.dgvHistoryRecord.ToPrintRows = rowIndex;
            this.dgvHistoryRecord.PDC = new PrintDocument();
            this.dgvHistoryRecord.PDC.DefaultPageSettings.Landscape = true;
            this.dgvHistoryRecord.PDC.DocumentName = Info.HistoryRecord;
            if (InitializePrinting(colIndex, rowIndex))
            {
                PrintController pc = new StandardPrintController();
                printDocument1.PrintController = pc;

                try
                {
                    printDocument1.Print();
                }
                catch { }
            }
        }

        private bool InitializePrinting(List<int> toPrintCols, List<int> toPrintRows)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.AllowSomePages = true;
            printDialog.ShowHelp = true;

            if (printDialog.ShowDialog() != DialogResult.OK)
                return false;
            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            printDocument1.DocumentName = Info.PrintHeader;
            printDocument1.PrinterSettings = printDialog.PrinterSettings;
            printDocument1.DefaultPageSettings.PrinterSettings.PrintRange = printDialog.PrinterSettings.DefaultPageSettings.PrinterSettings.PrintRange;
            printDocument1.DefaultPageSettings.Margins = new Margins(20, 20, 20, 20);
            DataGridView dgvToPrint = dgvHistoryRecord.Copy(toPrintCols, toPrintRows);
            dgvToPrint.AllowUserToAddRows = false;
            gridPrinter = new GridPrinter(dgvToPrint, printDocument1, true, true, Info.HistoryRecord, new System.Drawing.Font("黑体", 18, FontStyle.Bold, GraphicsUnit.Point), Color.Black, true);
            return true;
        }


        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                if (e.PageSettings.PrinterSettings.PrintRange == PrintRange.SomePages)
                {
                    if (e.PageSettings.PrinterSettings.FromPage > 0)
                        e.HasMorePages = gridPrinter.DrawDataGridView(e.Graphics, e.PageSettings.PrinterSettings.FromPage, e.PageSettings.PrinterSettings.ToPage);
                }
                else
                {
                    e.HasMorePages = gridPrinter.DrawDataGridView(e.Graphics);
                }
            }
            catch
            {
                //Error2Log.To2LogFile(ex, "打印出错");
                MessageBox.Show("请检查打印机是否安装正确！", "不能连接打印机", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void dataGridViewW1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && this.dgvHistoryRecord.HitTest(e.X, e.Y).Type == DataGridViewHitTestType.ColumnHeader && GP.CurrentUser.Role.RoleType == 0)
            {
                this.contextMenuStrip1.Items.Clear();
                //string tempStr = ReportTemplateHelper.LoadSpecifiedValue("HistoryItem", "Setting");
                //********************自定义历史记录
                string historyItemPathSave = string.Empty;
                string historyItemPath = ReportTemplateHelper.GetHistoryItemPath(((MyItem)comboBoxCurveName.SelectedItem).Value.ToString(),
                                                                                WorkCurveHelper.DeviceCurrent.Id,
                                                                                (int)WorkCurveHelper.DeviceFunctype,
                                                                                (int)((MyItem)comboBoxCurveName.SelectedItem).Type,
                                                                                out historyItemPathSave);
                string tempStr = ReportTemplateHelper.LoadHistoryItemSpecifiedValue(historyItemPath, "Setting");
                //********************自定义历史记录

                string[] str = tempStr.Split(',');
                List<string> listStr = str.ToList();
                List<ColumnObject> lstCol = new List<ColumnObject>();
                for (int i = 0; i < this.dgvHistoryRecord.Columns.Count; i++)
                {
                    if (this.dgvHistoryRecord.Columns[i].Name.Equals("aa") || this.dgvHistoryRecord.Columns[i].Name.Equals("continuousnumber")
                        || this.dgvHistoryRecord.Columns[i].Name.Equals("Id") || this.dgvHistoryRecord.Columns[i].Name.Equals("HistoryRecord_Id")
                        || this.dgvHistoryRecord.Columns[i].Name.Equals("customstandard_id") || this.dgvHistoryRecord.Columns[i].Name.EndsWith("_Color")
                        || this.dgvHistoryRecord.Columns[i].Name.Equals("WorkCurveId"))
                        continue;
                    ColumnObject cobj = new ColumnObject(dgvHistoryRecord.Columns[i].Name, dgvHistoryRecord.Columns[i].DisplayIndex, dgvHistoryRecord.Columns[i].HeaderText, dgvHistoryRecord.Columns[i].Visible);
                    lstCol.Add(cobj);
                }
                List<ColumnObject> lstOrder = lstCol.OrderBy(w => w.Index).ToList();
                foreach (var co in lstOrder)
                {
                    ToolStripMenuItem itemMenu = new ToolStripMenuItem();
                    itemMenu.Name = co.Name;
                    itemMenu.Text = co.HeadText;
                    itemMenu.Checked = co.Visible;
                    if (!this.contextMenuStrip1.Items.ContainsKey(itemMenu.Name))
                        this.contextMenuStrip1.Items.Add(itemMenu);
                    itemMenu.Click += new EventHandler(itemMenu_Click);

                    if (itemMenu.Text.Contains("厚度"))
                    {
                        itemMenu = new ToolStripMenuItem();
                        itemMenu.Name = co.HeadText.Split(new char[] { '厚' })[0] + "折线";
                        itemMenu.Text = co.HeadText.Split(new char[] { '厚' })[0] + "折线";
                        if(tempStr.Contains(itemMenu.Text))
                            itemMenu.Checked = true;
                        else
                            itemMenu.Checked = false;
                        this.contextMenuStrip1.Items.Add(itemMenu);
                        itemMenu.Click += new EventHandler(itemMenu_Click);

                    }
                }

              
                if (this.contextMenuStrip1.Items.Count > 0)
                {
                    ToolStripMenuItem setItem = new ToolStripMenuItem();
                    setItem.Name = "SettingContext";
                    setItem.Text = Info.ChoiceSetting;
                    setItem.Click += new EventHandler(setItem_Click);
                    if (!this.contextMenuStrip1.Items.ContainsKey(setItem.Name))
                        this.contextMenuStrip1.Items.Add(setItem);
                }
                this.contextMenuStrip1.Show(this.dgvHistoryRecord.PointToScreen(e.Location));
            }
        }
        #endregion

        private string outMode = "excel";

        private void buttonWExcel_Click(object sender, EventArgs e)
        {
            this.outMode = "excel";


            if (selectLong.Count == 0) return;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;//等待?
            //Workbook workbook = new Workbook();
            WinMethod.SendMessage(DifferenceDevice.interClassMain.deviceMeasure.interfacce.OwnerHandle, DeviceInterface.CUSTOM_MESSAGE, 0, 0);

            string strId = "";
            foreach (long id in selectLong)
                strId += id.ToString() + ",";
            if (strId != "") strId = strId.Substring(0, strId.Length - 1);
            DataTable dt = GetData(GetSQL(1, -1, -1, strId));
            if ((DifferenceDevice.IsShowWeight || DifferenceDevice.IsShowKarat) && DifferenceDevice.IsAnalyser)
                RefreshWeightInfo(ref dt);
            SetDecimalPlaces(ref dt);
            SetTestResult(ref dt);
            //追加更改dt的列的顺序 chuyaqin 2013-03-0328
            if (this.dgvHistoryRecord.Columns.Contains("aa"))
            {
                dt.Columns.Add("aa");
            }



            foreach (DataColumn dctemp in dt.Columns)
            {
                CurveElement cueT = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().Find(w => (w.Caption + Info.Thick == dctemp.ColumnName || w.Caption + Info.Intensity == dctemp.ColumnName || w.Caption + Info.Error == dctemp.ColumnName));
                if (cueT != null)
                {
                    dctemp.ColumnName = dctemp.ColumnName.Replace(cueT.Caption, cueT.DefineElemName);
                  
                }

            }


            if (FpWorkCurve.thickMode == ThickMode.NiNi || FpWorkCurve.thickMode == ThickMode.NiCuNi || FpWorkCurve.thickMode == ThickMode.NiCuNiFe)
            {
                string colName = string.Empty;
                foreach (DataColumn dctemp in dt.Columns)
                {
                    if (dctemp.ColumnName.Contains("Fe" + Info.Thick))
                    {
                        dctemp.ColumnName = "Ni2" + Info.Thick;
                        break;
                    }

                }

            }


            string historyItemPathSave = string.Empty;
            string historyItemPath = ReportTemplateHelper.GetHistoryItemPath(((MyItem)comboBoxCurveName.SelectedItem).Value.ToString(),
                                                                            WorkCurveHelper.DeviceCurrent.Id,
                                                                            (int)WorkCurveHelper.DeviceFunctype,
                                                                            (int)((MyItem)comboBoxCurveName.SelectedItem).Type,
                                                                            out historyItemPathSave);
            string tempStr = ReportTemplateHelper.LoadHistoryItemSpecifiedValue(historyItemPath, "ColumnIndex");

            List<String> tempList = tempStr.Split(new char[]{','}).ToList();

        
            if (ReportTemplateHelper.IsShowUrlInRecords)
                dt.Columns.Add("ReportPath");
            this.dataGridViewW2.DataSource = null;
            this.dataGridViewW2.DataSource = dt;
            string strContent = string.Empty;
            foreach (DataGridViewColumn gridcol in this.dgvHistoryRecord.Columns)
                if (gridcol.Visible && gridcol.Name != "aa" && this.dataGridViewW2.Columns.Contains(gridcol.Name))
                {
                    this.dataGridViewW2.Columns[gridcol.Name].HeaderText = gridcol.HeaderText;
                }
                else if (this.dataGridViewW2.Columns.Contains(gridcol.Name))
                    this.dataGridViewW2.Columns[gridcol.Name].Visible = false;//追加更改dt的列的顺序 chuyaqin 2013-03-0328

            this.dataGridViewW2.Columns["HistoryRecord_Id"].Visible = false;
            this.dataGridViewW2.Columns["workcurveid"].Visible = false;


            if (chkStatistic.Checked || this.isStaShow)
            {
                for (int i = 0; i < dataGridViewW2.ColumnCount; i++)
                {
                    if (dataGridViewW2.Columns[i].Visible)
                    {
                        strContent = dataGridViewW2.Columns[i].Name;
                        break;
                    }
                }
                try
                {
                    AddStaticsRows((DataTable)dataGridViewW2.DataSource, strContent);
                }
                catch
                {

                    Msg.Show("所选历史记录存在无法统计的数据值");
                    return;

                }
            }


            string temp = "";
            int loc = 0;

            for (int i = 0; i < this.dataGridViewW2.Columns.Count; i++)
            {
                if (this.dataGridViewW2.Columns[i].HeaderText.Contains("厚度") || this.dataGridViewW2.Columns[i].HeaderText.Contains("面密度"))
                {

                    for (int j = 0; j < this.dataGridViewW2.Rows.Count; j++)
                    {

                        temp = this.dataGridViewW2.Rows[j].Cells[i].Value.ToString();
                        loc = temp.IndexOf('(');

                       

                        if (loc < 0)
                            continue;
                        this.dataGridViewW2.Rows[j].Cells[i].Value = temp.Substring(0, loc).Trim();
                    }

                }


                if (this.dataGridViewW2.Columns[i].HeaderText.Contains("样品名称"))
                {
                    for (int j = 0; j < this.dataGridViewW2.Rows.Count; j++)
                    {

                        if (this.dataGridViewW2.Rows[j].Cells[i].Value.ToString().Contains("#"))
                        {
                            this.dataGridViewW2.Rows[j].Cells[i].Value = this.dataGridViewW2.Rows[j].Cells[i].Value.ToString().Split(new char[] { '#' })[0];

                        }
                    }

                }


            }
            WinMethod.SendMessage(DifferenceDevice.interClassMain.deviceMeasure.interfacce.OwnerHandle, DeviceInterface.CUSTOM_MESSAGE_HIDE, 0, 0);

            OutExcel(chkStatistic.Checked && DifferenceDevice.IsThick, ReportTemplateHelper.IsShowUrlInRecords);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;//等待?
        }


        private void btWClear_Click(object sender, EventArgs e)
        {
            string workcurve_id = string.Empty;
            if (comboBoxCurveName.Text == "--All--" || comboBoxCurveName.Text == "")
            {
                workcurve_id = "-1";
            }
            else
            {
                if (((MyItem)comboBoxCurveName.SelectedItem).Type == ItemType.Curve)
                {
                    workcurve_id = "(" + ((MyItem)comboBoxCurveName.SelectedItem).Value.ToString() + ")";
                }
                else
                {
                    string sql = "select * from WorkCurve a inner join Condition b on a.Condition_Id = b.Id inner join Device c on b.Device_Id=c.Id where c.Id=" + WorkCurveHelper.DeviceCurrent.Id;
                    var lstCur = WorkCurve.FindBySql(sql + " and  b.Device_Id=" + WorkCurveHelper.DeviceCurrent.Id + " and b.Type=0 and a.WorkRegion_Id =" + ((MyItem)comboBoxCurveName.SelectedItem).Value.ToString());
                    var lstarr = from curve in lstCur select curve.Id.ToString();
                    string[] idArray = lstarr.ToArray();

                    workcurve_id = "(" + string.Join(",", idArray) + ")";
                }
            }

            if (Msg.Show(Info.RemoveAll, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                string strsql = (comboBoxCurveName.Text == "--All--") ? " and workcurveid in (select id from workcurve where condition_id in ( " +
           " select id from condition where device_id=" + WorkCurveHelper.DeviceCurrent.Id + "))" : " and workcurveid in " + workcurve_id;

                SqlStatement sqlstate = new SqlStatement("delete from historycompanyotherinfo where history_id in (select id from  historyrecord where 1=1 " + strsql + ");  " +
                " delete from  historyElement where historyrecord_id in ( " +
                             " select id from  historyrecord where  1=1 " + strsql + ");" +
                " delete from  historyrecord where  1=1 " + strsql + "");
                Lephone.Data.DbEntry.Context.ExecuteNonQuery(sqlstate);
                buttonWSearch_Click(null, null);

            }

        }




        private void btwAllElements_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;//等待?
            WinMethod.SendMessage(DifferenceDevice.interClassMain.deviceMeasure.interfacce.OwnerHandle, DeviceInterface.CUSTOM_MESSAGE, 0, 0);

            DataTable dt = GetData(GetSQL(2, -1, -1, ""));
            if ((DifferenceDevice.IsShowWeight || DifferenceDevice.IsShowKarat) && DifferenceDevice.IsAnalyser)
                RefreshWeightInfo(ref dt);
            SetDecimalPlaces(ref dt);
            SetTestResult(ref dt);
            //追加更改dt的列的顺序 chuyaqin 2013-03-0328
            if (this.dgvHistoryRecord.Columns.Contains("aa"))
            {
                dt.Columns.Add("aa");
            }

            foreach (DataColumn dctemp in dt.Columns)
            {
                CurveElement cueT = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().Find(w => (w.Caption + Info.Thick == dctemp.ColumnName || w.Caption + Info.Intensity == dctemp.ColumnName || w.Caption + Info.Error == dctemp.ColumnName));
                if (cueT != null)
                {
                    dctemp.ColumnName = dctemp.ColumnName.Replace(cueT.Caption, cueT.DefineElemName);
                    //if (dctemp.ColumnName.Contains(Info.Thick))
                    //    dctemp.ColumnName = cueT.DefineElemName + Info.Thick;
                    //else if (dctemp.ColumnName.Contains(Info.Intensity))
                    //    dctemp.ColumnName = cueT.DefineElemName + Info.Intensity;
                    //else if (dctemp.ColumnName.Contains(Info.Error))
                    //    dctemp.ColumnName = cueT.DefineElemName + Info.Error;

                }

            }

            if (FpWorkCurve.thickMode == ThickMode.NiNi || FpWorkCurve.thickMode == ThickMode.NiCuNi || FpWorkCurve.thickMode == ThickMode.NiCuNiFe)
            {
                foreach (DataColumn dctemp in dt.Columns)
                {
                    if (dctemp.ColumnName.Contains("Fe" + Info.Thick))
                    {
                        dctemp.ColumnName = "Ni2" + Info.Thick;
                        break;
                    }

                }


            }
            

            dt.Columns.Remove("aa");
            if (ReportTemplateHelper.IsShowUrlInRecords)
                dt.Columns.Add("ReportPath");
            this.dataGridViewW2.DataSource = null;
            this.dataGridViewW2.DataSource = dt;
            foreach (DataGridViewColumn gridcol in this.dgvHistoryRecord.Columns)
                if (gridcol.Visible && gridcol.Name != "aa" && this.dataGridViewW2.Columns.Contains(gridcol.Name))
                    this.dataGridViewW2.Columns[gridcol.Name].HeaderText = gridcol.HeaderText;
                else if (gridcol.Visible == false && gridcol.Name != "aa" && this.dataGridViewW2.Columns.Contains(gridcol.Name))
                    this.dataGridViewW2.Columns[gridcol.Name].Visible = false;
            this.dataGridViewW2.Columns["HistoryRecord_Id"].Visible = false;
            this.dataGridViewW2.Columns["workcurveid"].Visible = false;



            string temp = "";
            int loc = 0;

            for (int i = 0; i < this.dataGridViewW2.Columns.Count; i++)
            {
                if (this.dataGridViewW2.Columns[i].HeaderText.Contains("厚度") || this.dataGridViewW2.Columns[i].HeaderText.Contains("面密度"))
                {

                    for (int j = 0; j < this.dataGridViewW2.Rows.Count; j++)
                    {



                        temp = this.dataGridViewW2.Rows[j].Cells[i].Value.ToString();
                        loc = temp.IndexOf('(');

                     

                        if (loc < 0)
                            continue;
                        this.dataGridViewW2.Rows[j].Cells[i].Value = temp.Substring(0, loc).Trim();
                    }

                }

                if (this.dataGridViewW2.Columns[i].HeaderText.Contains("样品名称"))
                {
                    for (int j = 0; j < this.dataGridViewW2.Rows.Count; j++)
                    {

                        if (this.dataGridViewW2.Rows[j].Cells[i].Value.ToString().Contains("#"))
                        {
                            this.dataGridViewW2.Rows[j].Cells[i].Value = this.dataGridViewW2.Rows[j].Cells[i].Value.ToString().Split(new char[] { '#' })[0];

                        }
                    }

                }


            }


            WinMethod.SendMessage(DifferenceDevice.interClassMain.deviceMeasure.interfacce.OwnerHandle, DeviceInterface.CUSTOM_MESSAGE_HIDE, 0, 0);

            //OutExcel(this.dataGridViewW2,false,true);
            OutExcel(false, ReportTemplateHelper.IsShowUrlInRecords);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;//等待?

        }



        private void OutExcel(bool bStatics, bool bExportReport)
        {
            SaveFileDialog sdlg = new SaveFileDialog();
            sdlg.Filter = "Excel File(*.xls)|*.xls";

            string ReportPath = "";

            if (!this.isStaShow)
            {
                if (sdlg.ShowDialog() != DialogResult.OK)
                    return;
                ReportPath = sdlg.FileName.Substring(0, sdlg.FileName.LastIndexOf("\\")) + "\\Report\\";
            }




            bool hasRecord = false;
            bool xmlLoaded = false;
            Workbook workbook = new Workbook();
            string fileName = "";

            //先导出报告
            if (bExportReport && dataGridViewW2.Columns.Contains("ReportPath"))
            {
                
                //int cellIndex = dataGridViewW2.Columns.Add("ReportPath", Info.Report);
                if (!Directory.Exists(ReportPath)) Directory.CreateDirectory(ReportPath);
                long selId = -1;
                ListElemetNames = this.FilterElementsNameByDGV(this.dgvHistoryRecord);//列头值元素名赋值给变量ListElemetNames以便打印模版时调用
                for (int i = 0; i < dataGridViewW2.RowCount; i++)
                {
                    selectLong.Clear();
                    selId = long.Parse(dataGridViewW2.Rows[i].Cells["ID"].Value.ToString());
                    selectLong.Add(selId);
                    string reportName = GetDefineReportName();
                    string reportName2 = reportName.Replace('#', '_');
                    try
                    {

                        if (!File.Exists(WorkCurveHelper.ExcelPath + "\\" + reportName + ".xls"))
                        {
                            Report report = new Report();
                            report.ReportPath = WorkCurveHelper.ExcelPath;
                            ElementList elementList = ElementList.New;
                            List<HistoryElement> elements = new List<HistoryElement>();//增加历史元素，以便建立DataTable调用 Strong 2012/10/25
                            if (selectLong.Count > WorkCurveHelper.PrintExcelCount)
                            {
                                Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
                                return;
                            }

                            if (!this.AddReportParameters(report, elementList, ref elements, 1))//获取Report基本参数{9代表打印模式}
                                return;

                            report.InterestElemCount = elementList.Items.ToList().FindAll(w => w.IsShowElement).Count;
                            report.TempletFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.OneTimeTemplate;
                            report.GenerateReport(reportName, true);
                        }
                        if (File.Exists(WorkCurveHelper.ExcelPath + "\\" + reportName + ".xls") && (WorkCurveHelper.ExcelPath + "\\").CompareTo(ReportPath) != 0)
                        {
                            File.Copy(WorkCurveHelper.ExcelPath + "\\" + reportName + ".xls", ReportPath + reportName2 + ".xls", true);
                        }
                        if (File.Exists(ReportPath + reportName2 + ".xls"))
                        {
                            dataGridViewW2.Rows[i].Cells["ReportPath"].Value = ".\\Report\\" + reportName2 + ".xls";
                            //dataGridViewW2.Rows[i].Cells[cellIndex].Value = "C:\\Documents and Settings\\Administrator\\桌面\\test\\Report\\8_青铜33__20140710_144556.xls";
                            // dataGridViewW2.Rows[i].Cells[cellIndex].Value = ".\\Report\\8_青铜33__20140710_144556.xls";
                            //dataGridViewW2.Rows[i].Cells[cellIndex].Value = "8_青铜33__20140710_144556.xls";
                        }
                        else
                        {
                            dataGridViewW2.Rows[i].Cells["ReportPath"].Value = null;
                        }
                    }
                    catch
                    {
                        dataGridViewW2.Rows[i].Cells["ReportPath"].Value = null;
                    }
                }
                selectLong.Clear();
            }
            if (DifferenceDevice.IsAnalyser)
            {
                foreach (DataGridViewRow row in dataGridViewW2.Rows)
                {
                    if (!row.Cells[1].Value.ToString().Contains(Info.MeanValue) && !row.Cells[1].Value.ToString().Contains(Info.Total + Info.Weight)) continue;
                    foreach (DataGridViewCell col in row.Cells)
                    {
                        col.Style.Font = new System.Drawing.Font("宋体", 12, System.Drawing.FontStyle.Bold);
                        col.Style.ForeColor = Color.Red;
                    }
                }
            }
            if (ReportTemplateHelper.IsOurExcel == 1)
            {
                List<string> otherinfo = new List<string>();
                if (WorkCurveHelper.isShowND)
                    otherinfo.Add(Info.Remarks + WorkCurveHelper.NDValue.ToString() + "ppm");
                otherinfo.Add(Info.Subface);
                otherinfo.Add(Info.TotalContent);

                List<string> redFieldInfo = new List<string>();
                redFieldInfo.Add(Info.strStandard);
                redFieldInfo.Add(Info.WorkingCurve);

                fileName = dataGridViewW2.ExportExcel_Public_SpecialRowNew(false, out hasRecord, out workbook, otherinfo, redFieldInfo);
            }
            else if (ReportTemplateHelper.IsOurExcel == 2)
            {
                List<string> otherinfo = new List<string>();
                otherinfo.Add(Info.Total + " " + dataGridViewW2.RowCount + " " + Info.Article);
                List<string> redFieldInfo = new List<string>();
                fileName = dataGridViewW2.ExportExcel_Public_SpecialRowNew(false, out hasRecord, out workbook, otherinfo, redFieldInfo);
            }
            else
            {
                if (!isXmlLoaded)
                    fileName = dataGridViewW2.ExportExcel_Public_SpecialRow(false, out hasRecord, out workbook);
                else
                {
                    fileName = dataGridViewW2.ExportExcel_Public_SpecialRow1(false, out hasRecord, out workbook, this.FilterElementsNameByDGV(this.dgvHistoryRecord), WorkCurveHelper.CurrentWorkRegion.Caption, out xmlLoaded);
                    if (!xmlLoaded)
                    {
                        Msg.Show(Info.Shri_LoadXmlFailed);
                        this.Cursor = System.Windows.Forms.Cursors.Arrow;
                        return;
                    }
                }
            }


            if (!hasRecord)
            {
                Msg.Show(Info.SelectHistoryRecord);
                this.Cursor = System.Windows.Forms.Cursors.Arrow;//等待?
                return;
            }

            Worksheet sheet = workbook.Worksheets[0];



            int dataEndRow = 0;
            int staStartRow = 0;

            if (this.isStaShow)
                this.staStartRow = this.dataGridViewW2.Rows.Count - 5;

            if ((this.outMode == "excel" && chkStatistic.Checked) || this.isStaShow)
            {
                dataEndRow = this.dataGridViewW2.Rows.Count - 5;
                staStartRow = this.dataGridViewW2.Rows.Count - 5;
            }
            else
            {
                dataEndRow = this.dataGridViewW2.Rows.Count;

            }



            int eleNum = 0;
            ArrayList indexList = new ArrayList();
            for (int i = 0; i < sheet.Cells.Columns.Count; i++)
            {
                if (sheet.Cells[0, i].Value.ToString().Contains("厚度") || sheet.Cells[0, i].Value.ToString().Contains("面密度"))
                {

                    eleNum++;
                    indexList.Add(i);
                }
            }

            if (eleNum >= 1 )
            {
                for (int i = 1; i < dataEndRow; i++)
                {
                  
                    for (int j = 0; j < eleNum; j++)
                    {
                        double curThick = 0;

                        try
                        {
                            curThick = double.Parse(double.Parse(sheet.Cells[i, (int)indexList[j]].Value.ToString().Trim()).ToString("f" + WorkCurveHelper.ThickBit));
                        }
                        catch
                        {
                            break;

                        }
        
                        sheet.Cells[i, (int)indexList[j]].Value = curThick;
                       
                    }

                }

            }


            if (bStatics)
            {
                string bitmapPath = CreateStasticBitmap(dataGridViewW2, workbook);
                //dataGridViewW2.CreateChart(workbook, dataGridViewW2.RowCount+1, 0, bitmapPath);
            }
            //SaveFileDialog sdlg = new SaveFileDialog();
            //sdlg.Filter = "Excel File(*.xls)|*.xls";
            //if (sdlg.ShowDialog() == DialogResult.OK)
            //{
            ExcelExporter excelExporter = new ExcelExporter(null);
            if (excelExporter.IsFileUsed(sdlg.FileName) && File.Exists(sdlg.FileName))
            {
                Msg.Show(Info.strOpenExcel);
                this.Cursor = System.Windows.Forms.Cursors.Arrow;//等待?
                return;
            }


            if ((this.outMode == "excel" && chkStatistic.Checked) || this.isStaShow)
            {
                double res = 0;
                string formula = "";
                string rows = "";
                //设置公式
                for (int i = 0; i < sheet.Cells.Columns.Count; i++)
                {
                    if (sheet.Cells[0, i].Value.ToString().Contains("厚度") || sheet.Cells[0, i].Value.ToString().Contains("面密度"))
                    {
                        rows = "";
                        for (int row = 1; row < dataEndRow; row++)
                        {
                            if (double.TryParse(sheet.Cells[row, i].Value.ToString(), out  res))
                                rows += ((char)(65 + i)) + (row + 1).ToString() + ",";


                        }

                        if (rows == "")
                        {
                            sheet.Cells[staStartRow, i].Value = "--";
                            sheet.Cells[staStartRow + 1, i].Value = "--";
                            sheet.Cells[staStartRow + 2, i].Value = "--";
                            sheet.Cells[staStartRow + 3, i].Value = "--";
                            sheet.Cells[staStartRow + 4, i].Value = "--";
                            sheet.Cells[staStartRow + 5, i].Value = "--";

                            continue;
                        }

                        rows = rows.Substring(0, rows.Length - 1);

                        formula = "=ROUND(AVERAGE(";
                        formula += rows;
                        formula += ")," + WorkCurveHelper.ThickBit.ToString() + ")";
                        sheet.Cells[staStartRow, i].Formula = formula;


                        formula = "=ROUND(STDEV(";
                        formula += rows;
                        formula += ")," + WorkCurveHelper.ThickBit.ToString() + ")";
                        sheet.Cells[staStartRow + 1, i].Formula = formula;


                        formula = "=TEXT(ROUND(STDEV(";
                        formula += rows;
                        formula += ")/AVERAGE(";
                        formula += rows;
                        formula += ")," + WorkCurveHelper.ThickBit.ToString() + "),\"0.0000%\")";
                        sheet.Cells[staStartRow + 2, i].Formula = formula;



                        formula = "=ROUND(MAX(";
                        formula += rows;
                        formula += ")," + WorkCurveHelper.ThickBit.ToString() + ")";
                        sheet.Cells[staStartRow + 3, i].Formula = formula;


                        formula = "=ROUND(MIN(";
                        formula += rows;
                        formula += ")," + WorkCurveHelper.ThickBit.ToString() + ")";
                        sheet.Cells[staStartRow + 4, i].Formula = formula;



                        formula = "=ROUND(MAX(";
                        formula += rows;
                        formula += ")-MIN(";
                        formula += rows;
                        formula += ")," + WorkCurveHelper.ThickBit.ToString() + ")";
                        sheet.Cells[staStartRow + 5, i].Formula = formula;



                    }

                }
            }



            if (this.isStaShow)
            {
                sheet.CalculateFormula(true, true, null);

                this.sheetTemp = sheet;
                return;
            }
            else
            {
                //设置居中对齐
                Aspose.Cells.Range range = sheet.Cells.CreateRange(0, 0, 1000, 1000);
                Aspose.Cells.Style style = new Aspose.Cells.Style();
                style.HorizontalAlignment = TextAlignmentType.Center;
                range.SetStyle(style);


                workbook.Save(sdlg.FileName);

                fileName = sdlg.FileName;

            }


            if (!fileName.IsNullOrEmpty())
                UIHelper.AskToOpenFile(fileName);
        }



        private void dataGridViewW1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvHistoryRecord.Rows)
            {

                foreach (DataGridViewColumn col in dgvHistoryRecord.Columns)
                {
                    if (col.HeaderText.Contains("_Color"))
                    {

                        if (row.Cells[col.HeaderText].Value.ToString() == "true")
                        {
                            row.Cells[col.HeaderText.Replace("_Color", "")].Style.ForeColor = Color.Red;
                            string elem = col.HeaderText.Replace("_Color", "");
                            if (dgvHistoryRecord.Columns.Contains(elem + Info.strError)) row.Cells[elem + Info.strError].Style.ForeColor = Color.Red;
                        }

                        //col.Visible = false;

                    }

                }
            }
            foreach (DataGridViewColumn col in dataGridViewW2.Columns)
            {
                if (col.HeaderText.Contains("_Color"))
                {
                    col.Visible = false;
                }
            }
        }

        private void dataGridViewW2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (DifferenceDevice.IsRohs && ReportTemplateHelper.IsOurExcel == 0)
            {
                foreach (DataGridViewRow row in dataGridViewW2.Rows)
                {
                    foreach (DataGridViewColumn col in dataGridViewW2.Columns)
                    {


                        if (col.HeaderText.Contains("_Color"))
                        {
                            if (row.Cells[col.HeaderText].Value.ToString() == "true")
                            {
                                row.Cells[col.HeaderText.Replace("_Color", "")].Style.ForeColor = Color.Red;
                                string elem = col.HeaderText.Replace("_Color", "");
                                if (dataGridViewW2.Columns.Contains(elem + Info.strError)) row.Cells[elem + Info.strError].Style.ForeColor = Color.Red;
                            }

                        }

                    }


                }
                foreach (DataGridViewColumn col in dataGridViewW2.Columns)
                {
                    if (col.HeaderText.Contains("_Color"))
                    {
                        col.Visible = false;
                    }
                }

            }
        }

        /// <summary>
        /// 获取界面元素信息
        /// </summary>
        /// <param name="dgvHistoryRecord"></param>
        /// <returns></returns>
        private List<string> FilterElementsNameByDGV(DataGridView dgvHistoryRecord)
        {
            List<string> seleElement = new List<string>();
            foreach (DataGridViewColumn col in dgvHistoryRecord.Columns)
            {
                if (DifferenceDevice.IsThick)
                {
                    string[] satom = col.HeaderText.Replace(Info.EditContent, "").Replace(Info.Thick, "").Split('|');
                    foreach (string stom in satom)
                    {
                        Atom atom = Atoms.AtomList.Find(s => s.AtomName == stom);
                        if (atom != null && !seleElement.Contains(stom) && col.Visible)
                            seleElement.Add(stom);
                    }
                }
                else if (DifferenceDevice.IsXRF) //氧化物处理
                {

                    Atom atom = Atoms.AtomList.Find(s => s.AtomName == col.HeaderText);
                    List<Oxide> oxidel = Oxide.FindBySql("select * from Oxide where OxideName='" + col.HeaderText + "'");
                    if ((atom != null || oxidel.Count > 0) && !seleElement.Contains(col.HeaderText) && col.Visible)
                        seleElement.Add(col.HeaderText);
                    else
                    {
                        List<CustomField> customs = CustomField.FindBySql("select * from CustomField where Name='" + col.HeaderText + "'");
                        if (customs.Count > 0)
                        {
                            seleElement.Add(col.HeaderText);
                        }
                    }
                }
                else
                {
                    Atom atom = Atoms.AtomList.Find(s => s.AtomName == col.HeaderText);
                    if (atom != null && !seleElement.Contains(col.HeaderText) && col.Visible)
                        seleElement.Add(col.HeaderText);
                    else
                    {
                        List<CustomField> customs = CustomField.FindBySql("select * from CustomField where Name='" + col.HeaderText + "'");
                        if (customs.Count > 0)
                        {
                            seleElement.Add(col.HeaderText);
                        }
                    }
                }
            }
            return seleElement;
        }
        /// <summary>
        /// 根据历史记录DataGridView界面生成Elementlist
        /// 界面元素已经排序或隐藏 导出以界面为准
        /// </summary>
        /// <param name="dgv"></param>
        /// <returns></returns>
        private ElementList FilterElementsByDGV(long historyId, bool IsRohs)
        {
            HistoryRecord historyRecord = HistoryRecord.FindById(historyId);
            if (historyRecord == null) return null;
            WorkCurve workCurve = WorkCurve.FindById(historyRecord.WorkCurveId);
            if (workCurve == null) return null;
            ElementList elementList = workCurve.ElementList;//感兴趣元素集合
            ElementList backElementList = ElementList.New;
            //历史记录元素集合
            List<HistoryElement> hisElementList = HistoryElement.FindBySql("select * from HistoryElement where HistoryRecord_Id =" + historyId);

            //List<string> eleNames = FilterElementsNameByDGV(dgv);//取消此段，改为ListElemetNames Strong 2012/10/25

            foreach (string s in ListElemetNames)
            {
                CurveElement curEle = elementList.Items.ToList().Find(w => (w.Formula == s));
                if (curEle == null)//感兴趣元素外的元素
                {
                    //if (IsRohs) continue;
                    curEle = CurveElement.New;
                    curEle.Formula = s;
                    curEle.Caption = s;
                    curEle.IsShowContent = curEle.IsShowElement = true;
                    curEle.RowsIndex = -1;
                    //Oxide oxide = Oxide.Where(w => w.OxideName == s);
                    //if (oxide!=null)
                    //    curEle.Caption = oxide.Atom.AtomName;
                }
                HistoryElement hisEle = hisElementList.Find(w => w.elementName == s);
                if (hisEle != null)//历史记录中存在该元素 赋值
                {
                    double content = 0;
                    double.TryParse(hisEle.contextelementValue, out content);
                    if (hisEle.unitValue == (int)ContentUnit.per)
                    {
                        curEle.Content = content;
                        curEle.Error = hisEle.Error;
                    }
                    else if (hisEle.unitValue == (int)ContentUnit.permillage)
                    {
                        curEle.Content = content / 10;
                        curEle.Error = hisEle.Error / 10;
                    }
                    else if (hisEle.unitValue == (int)ContentUnit.ppm)
                    {
                        curEle.Content = content / 10000;
                        curEle.Error = hisEle.Error / 10000;
                    }
                    curEle.Intensity = hisEle.CaculateIntensity;
                    double.TryParse(hisEle.thickelementValue, out content);
                    curEle.Thickness = content;
                }
                else//历史记录中也不存在 不赋值
                {
                    curEle.Content = 0;
                    curEle.Error = 0;
                    curEle.Intensity = 0;
                    curEle.Thickness = 0;
                }
                backElementList.Items.Add(curEle);
            }
            backElementList.RhIsLayer = elementList.RhIsLayer;
            backElementList.LayerElemsInAnalyzer = elementList.LayerElemsInAnalyzer;
            return backElementList;
        }


        private void btwTemplateExcel_Click(object sender, EventArgs e)
        {
            if (selectLong.Count == 0) return;
            if (WorkCurveHelper.PrinterType == 1 && chkPrintHistory.Checked)
            {
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(PrintBlue), null);
                return;
            }

            string valid = DifferenceDevice.interClassMain.GenerateGenericReport(FilterElementsByDGV(selectLong.FirstOrDefault(), false), selectLong);
            if (!string.IsNullOrEmpty(valid))
            {
                DifferenceDevice.interClassMain.OpenPathThread(valid);
                WinMethod.SendMessage(DifferenceDevice.interClassMain.deviceMeasure.interfacce.OwnerHandle, DeviceInterface.CUSTOM_MESSAGE_HIDE, 0, 0);

                return;
            }
            WinMethod.SendMessage(DifferenceDevice.interClassMain.deviceMeasure.interfacce.OwnerHandle, DeviceInterface.CUSTOM_MESSAGE, 0, 0);

            ListElemetNames = this.FilterElementsNameByDGV(this.dgvHistoryRecord);//列头值元素名赋值给变量ListElemetNames以便打印模版时调用


            if (chkPrintHistory.Checked) OnPrintTemplate = new Print(DifferenceDevice.interClassMain.PrintThread);
            else OnPrintTemplate = new Print(DifferenceDevice.interClassMain.OpenPathThread);

            #region 整理新的打印方式，现启用 Strong 2012/10/26
            switch (ReportTemplateHelper.ExcelModeType)
            {
                case 1:
                case 4:
                case 23:
                case 28:
                    this.PrintMode1();
                    break;
                case 2:
                    this.PrintModel2();
                    break;
                case 6:
                    this.PrintModel6();
                    break;
                case 7:
                    this.PrintModel7();
                    break;
                case 9:
                case 20:
                case 21:
                case 24:
                    this.PrintModel9();
                    break;
                case 11:
                    this.PrintModel11();
                    break;
                case 12:
                case 25:
                    this.PrintModel12();
                    break;
                case 13:
                    this.PrintModel13();
                    break;
                case 14:
                    this.PrintModel13();
                    break;
                case 19:
                    this.PrintModel19();
                    break;
                case 26:
                    this.PrintModel26();
                    break;
                case 30:
                    this.PrintModel30();
                    break;
                case 31:
                    this.PrintModel31();
                    break;
                default:
                    this.PrintModelDefault();
                    break;

            }
            WinMethod.SendMessage(DifferenceDevice.interClassMain.deviceMeasure.interfacce.OwnerHandle, DeviceInterface.CUSTOM_MESSAGE_HIDE, 0, 0);

            #endregion
        }


        #region 打印模式1
        private void PrintMode1()
        {
            List<string> SeleWorkCurveNameList = new List<string>();
            string StrSavePath = string.Empty;
            int reportType = comboBoxReportType.Text.ToLower() == "pdf" ? 1 : 0;
            #region
            Report report = new Report();
            report.ReportFileType = reportType;
            report.isShowND = WorkCurveHelper.isShowND;
            ElementList elementList = ElementList.New;
            List<HistoryElement> elements = new List<HistoryElement>();//增加历史元素，以便建立DataTable调用 Strong 2012/10/25
            if (selectLong.Count > WorkCurveHelper.PrintExcelCount)
            {
                Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
                return;
            }


            if (!this.AddReportParameters(report, elementList, ref elements, 1))//获取Report基本参数{1代表打印模式}
                return;

            string reportName = GetDefineReportName();
            if (ReportTemplateHelper.ExcelModeType != 23)
            {
                foreach (var em in report.elements.Items)
                {
                    em.ContentUnit = ContentUnit.ppm;// 模板1是显示全ppm
                }
            }

            if (selectLong.Count == 1)
            {
                report.InterestElemCount = report.Elements.Items.ToList().FindAll(w => w.IsShowElement).Count;//elementList.Items.Count;
                report.TempletFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.OneTimeTemplate;
                report.ReportPath = WorkCurveHelper.ExcelPath;
                report.historyRecordid = selectLong.FirstOrDefault().ToString();
                StrSavePath = report.GenerateReport(reportName, true);
            }
            else
            {
                DataTable dt = this.GetReportDT1(report, elements);//获取打印内容

                report.InterestElemCount = elementList.Items.ToList().FindAll(w => w.IsShowElement).Count;
                List<long> lstEleId = new List<long>();
                foreach (var ele in elementList.Items)
                {
                    lstEleId.Add(ele.Id);
                }
                if (lstEleId.Distinct().Count() > (string.IsNullOrEmpty(ExcelTemplateParams.TotalColumnMun) ? 9 : int.Parse(ExcelTemplateParams.TotalColumnMun)))
                    if (File.Exists(Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.ManyTimeTemplate.TrimEnd(".xls".ToCharArray()) + "_Horizontal.xls"))
                        ExcelTemplateParams.ManyTimeTemplate = ExcelTemplateParams.ManyTimeTemplate.TrimEnd(".xls".ToCharArray()) + "_Horizontal.xls";
                report.RetestFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.ManyTimeTemplate;
                report.ReportPath = WorkCurveHelper.ExcelPath;
                StrSavePath = report.GenerateRetestReport(reportName, dt, true, false);
            }
            if (StrSavePath == string.Empty || !File.Exists(StrSavePath))
                return;
            else
            {
                OnPrintTemplate(StrSavePath);
            }
            #endregion
        }



        private DataTable GetReportDT1(Report report, List<HistoryElement> elements)
        {

            DataTable dt = CreateReTestTable(report.Elements);//CreateReTestTable(elementList);
            CustomStandard standard = null;
            if (report.historyStandID > -1)
            {
                standard = CustomStandard.FindById(report.historyStandID);
                if (standard != null && standard.StandardDatas != null && standard.StandardDatas.Count > 0 && standard.IsSelectTotal)
                {
                    dt.Columns.Add(Info.TotalPassReslt);
                }
            }

            int cont = 0;

            var groupHistoryElements = elements.GroupBy(w => w.HistoryRecord.Id);
            #region
            foreach (var iter in groupHistoryElements)
            {
                List<HistoryElement> tt = iter.ToList<HistoryElement>();
                DataRow rowNew = dt.NewRow();
                rowNew["Time"] = ++cont;
                double totalContent = 0d;
                foreach (DataColumn dc in dt.Columns)
                    if (dc.ColumnName != "Time")
                    {
                        HistoryElement wc = tt.Find(w => w.elementName == dc.ColumnName);
                        if (wc == null)
                            continue;
                        string valueStr = wc.contextelementValue;
                        if (!string.IsNullOrEmpty(valueStr))
                        {
                            double Value = double.Parse(valueStr);
                            if (wc.unitValue == 1)
                                Value = Value * 10000;
                            else if (wc.unitValue == 3)
                                Value = Value * 1000;
                            //2014-03-24 模板1 都使用ppm单位
                            if (Value <= WorkCurveHelper.NDValue)
                            {
                                // if (wc.unitValue == 1)
                                // {
                                //    rowNew[dc.ColumnName] = "ND(%)";
                                // }
                                // else if (wc.unitValue == 2)
                                // {
                                rowNew[dc.ColumnName] = "ND(ppm)";
                                // }
                                // else
                                //   rowNew[dc.ColumnName] = "ND(‰)";
                            }
                            else
                            {
                                // if (wc.unitValue == 1)
                                // {
                                //    rowNew[dc.ColumnName] = (Value / 10000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(%)";
                                // }
                                // else if (wc.unitValue == 2)
                                // {
                                rowNew[dc.ColumnName] = Value.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(ppm)";
                                //}
                                //else
                                //{
                                //    rowNew[dc.ColumnName] = (Value / 1000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(‰)";
                                //}
                            }
                            totalContent += Value;
                        }
                        if (standard != null && standard.StandardDatas != null && standard.StandardDatas.Count > 0 && standard.IsSelectTotal)
                        {
                            string strPass = ExcelTemplateParams.PassResults;
                            if (totalContent > standard.TotalContentStandard)
                                strPass = ExcelTemplateParams.FalseResults;
                            rowNew[Info.TotalPassReslt] = strPass;
                        }
                        // rowNew[dc.ColumnName] = tt.Find(w => w.elementName == dc.ColumnName).contextelementValue;
                    }
                dt.Rows.Add(rowNew);
            }
            #endregion

            return dt;

        }
        #endregion

        #region 打印模式2
        private void PrintModel2()
        {
            List<string> SeleWorkCurveNameList = new List<string>();

            #region
            if (selectLong.Count == 0)
            {
                Msg.Show(Info.SelectHistoryRecord);
                return;
            }
            if (selectLong.Count > WorkCurveHelper.PrintExcelCount)
            {
                Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
                return;
            }

            string workCurveName = string.Empty;
            SpecEntity spec = new SpecEntity();
            List<InterfaceClass.PrintObject> seleHistoryPrintObjectL = new List<InterfaceClass.PrintObject>();
            foreach (long seleid in selectLong)
            {
                long id = long.Parse(seleid.ToString());

                HistoryRecord record = HistoryRecord.FindById(id);
                if (record != null)
                {
                    WorkCurve workCurve = WorkCurve.FindById(record.WorkCurveId);
                    if (workCurve != null)
                    {
                        if (!SeleWorkCurveNameList.Contains(workCurve.Name)) SeleWorkCurveNameList.Add(workCurve.Name);
                        string specListName = record.SpecListName;
                        workCurveName = workCurve.Name;
                        SpecListEntity tempList = DataBaseHelper.QueryByEdition(specListName, record.FilePath, record.EditionType);
                        if (tempList != null && tempList.Specs.Length > 0)
                        {
                            seleHistoryPrintObjectL.Add(new InterfaceClass.PrintObject(tempList, workCurve, 1, id));
                        }
                        else
                        {
                            Msg.Show(Info.DataDelete);
                            return;
                        }
                    }
                    else
                    {
                        Msg.Show(Info.WorkCurveDelete);
                        return;
                    }
                }
            }

            if (comboBoxCurveName.Text == "--All--" && SeleWorkCurveNameList.Count > 1)
            {
                Msg.Show(Info.strSeleWorkCurveName);
                return;
            }

            if (InterfaceClass.SetPrintTemplate(ListElemetNames, seleHistoryPrintObjectL))//原参数seleElement=>ListElemetNames
            {
                EDXRFHelper.NewPrintDirectPrintHelper(InterfaceClass.seledataFountain);
            }
            else Msg.Show(Info.NoLoadSource);

            #endregion
        }
        #endregion

        #region 打印模式6
        private void PrintModel6()
        {
            if (DifferenceDevice.IsAnalyser && selectLong.Count > 0)
            {
                if (DifferenceDevice.interClassMain.reportThreadManage == null) return;
                bool isexcel = false;
                bool isdpf = false;
                string OpenExcelOrNot = Skyray.EDX.Common.Info.OpenPDFOrNot; ;
                if (comboBoxReportType.Text.ToLower() == "excel") { isexcel = true; OpenExcelOrNot = Skyray.EDX.Common.Info.OpenExcelOrNot; }
                else { isdpf = true; }

                string SaveReportPath = DifferenceDevice.interClassMain.reportThreadManage.GetHistoryRecordReport(selectLong, 0, isexcel, isdpf);

                if (SaveReportPath == "") return;
                else
                {
                    //DifferenceDevice.interClassMain.OpenPathThread(SaveReportPath);
                    OnPrintTemplate(SaveReportPath);
                }

            }
        }
        #endregion

        #region 打印模式7
        private void PrintModel7()
        {
            List<string> SeleWorkCurveNameList = new List<string>();
            #region
            if (comboBoxCurveName.Text == "--All--" && SeleWorkCurveNameList.Count > 1)
            {
                Msg.Show(Info.strSeleWorkCurveName);
                return;
            }
            if (selectLong.Count > WorkCurveHelper.PrintExcelCount)
            {
                Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
                return;
            }

            ExcelTemplateParams.GetExcelTemplateParams();
            string reportName = GetDefineReportName();

            Report report = new Report();
            report.bProtect = false;

            report.TempletFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.OneTimeTemplate;
            // if (selectLong.Count > 0) report.TempletFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.ManyTimeTemplate;
            report.ReportPath = WorkCurveHelper.ExcelPath;
            report.Elements = FilterElementsByDGV(selectLong.FirstOrDefault(), DifferenceDevice.IsRohs);//只能第一条有效数据导出
            report.historyRecordid = selectLong.FirstOrDefault().ToString();


            string strSavedPath = report.GenerateRepeaterReportBrass(reportName, this.contextMenuStrip1, true, selectLong);

            if (strSavedPath == string.Empty || !File.Exists(strSavedPath))
                return;
            else
            {
                OnPrintTemplate(strSavedPath);
            }

            #endregion
        }
        #endregion

        #region 打印模式9
        private void PrintModel9()
        {
            List<string> SeleWorkCurveNameList = new List<string>();
            #region
            ExcelTemplateParams.GetExcelTemplateParams();
            Report report = new Report();
            report.ReportFileType = comboBoxReportType.SelectedIndex;// comboBoxReportType.Text.ToLower() == "pdf" ? 1 : 0;
            ElementList elementList = ElementList.New;
            List<HistoryElement> elements = new List<HistoryElement>();//增加历史元素，以便建立DataTable调用 Strong 2012/10/25

            if (selectLong.Count > WorkCurveHelper.PrintExcelCount)
            {
                Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
                return;
            }

            if (!this.AddReportParameters(report, elementList, ref elements, 9))//获取Report基本参数{9代表打印模式}
                return;

            string reportName = GetDefineReportName();
            string ReportSavedPath = string.Empty;

            report.IsAnalyser = (ReportTemplateHelper.ExcelModeType == 21 || ReportTemplateHelper.ExcelModeType == 9 || ReportTemplateHelper.ExcelModeType == 6 || ReportTemplateHelper.ExcelModeType == 15) ? true : false;
            if (selectLong.Count == 1)
            {
                report.InterestElemCount = elementList.Items.ToList().FindAll(w => w.IsShowElement).Count;
                report.TempletFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.OneTimeTemplate;
                report.ReportPath = WorkCurveHelper.ExcelPath;
                ReportSavedPath = report.GenerateReport(reportName, true);

            }
            else
            {
                DataTable dt = this.GetReportDT9(report, elementList);//获取打印数据
                bool flag = false;
                bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestStatistics"), out flag);
                string strStaticContent = ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestStaContent");
                bool bFullName = false;
                try
                {
                    bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestFullElemName"), out bFullName);
                }
                catch
                {
                    bFullName = false;
                }
                bool bNeedTimesResult = true;
                try
                {
                    bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestNeedMulResults"), out bNeedTimesResult);
                }
                catch
                {
                    bNeedTimesResult = true;
                }

                List<string> statics = (strStaticContent == null || strStaticContent.Trim() == string.Empty) ? null : strStaticContent.Split(',').ToList();
                if (flag)
                    // AddStaticsRows(dt, "time");
                    DifferenceDevice.interClassMain.AddStaticsRows(dt, "time", statics);
                if (bFullName)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        //if (ReportTemplateHelper.ExcelModeType == 21)
                        //{
                        //    Atom atom = Atoms.AtomList.Find(s => s.AtomName == curEle.Caption);
                        //    string atomNameCN = (atom == null) ? "" : atom.AtomNameCN;
                        //    showElem = Lang.Model.CurrentLang.IsDefaultLang ? atomNameCN + "(" + curEle.Caption + ")" : curEle.Caption;
                        //}
                        //else
                        //    showElem = curEle.Caption;

                        Atom atom = Atoms.AtomList.Find(s => s.AtomName == dc.ColumnName);
                        if (atom == null) continue;
                        string atomNameCN = (atom == null) ? "" : atom.AtomNameCN;
                        string atomNameEN = (atom == null) ? "" : atom.AtomNameEN;
                        dc.Caption = Skyray.Language.Lang.Model.CurrentLang.IsDefaultLang ? atomNameCN : atomNameEN;
                    }
                }
                if (ReportTemplateHelper.ExcelModeType == 21 || ReportTemplateHelper.ExcelModeType == 26)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        Atom atom = Atoms.AtomList.Find(s => s.AtomName == dc.ColumnName);
                        if (atom == null) continue;
                        string atomNameCN = (atom == null) ? "" : atom.AtomNameCN;
                        dc.Caption = Skyray.Language.Lang.Model.CurrentLang.IsDefaultLang ? atomNameCN + "(" + dc.ColumnName + ")" : dc.ColumnName;
                    }
                }
                report.InterestElemCount = elementList.Items.ToList().FindAll(w => w.IsShowElement).Count;
                report.RetestFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.ManyTimeTemplate;
                report.ReportPath = WorkCurveHelper.ExcelPath;
                report.NeedMultiResults = bNeedTimesResult;
                ReportSavedPath = report.GenerateRetestReport(reportName, dt, true, true);
            }

            if (ReportSavedPath == string.Empty || !File.Exists(ReportSavedPath))
                return;
            else
            {
                OnPrintTemplate(ReportSavedPath);
            }


            #endregion

        }


        private void PrintModel26()
        {
            List<string> SeleWorkCurveNameList = new List<string>();
            #region
            ExcelTemplateParams.GetExcelTemplateParams();
            Report report = new Report();
            report.ReportFileType = comboBoxReportType.SelectedIndex;// comboBoxReportType.Text.ToLower() == "pdf" ? 1 : 0;
            ElementList elementList = ElementList.New;
            List<HistoryElement> elements = new List<HistoryElement>();//增加历史元素，以便建立DataTable调用 Strong 2012/10/25

            if (selectLong.Count > WorkCurveHelper.PrintExcelCount)
            {
                Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
                return;
            }

            if (!this.AddReportParameters(report, elementList, ref elements, 9))//获取Report基本参数{9代表打印模式}
                return;

            string reportName = GetDefineReportName();
            string ReportSavedPath = string.Empty;

            report.IsAnalyser = false;
            if (selectLong.Count == 1)
            {
                report.InterestElemCount = elementList.Items.ToList().FindAll(w => w.IsShowElement).Count;
                report.TempletFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.OneTimeTemplate;
                report.ReportPath = WorkCurveHelper.ExcelPath;
                ReportSavedPath = report.GenerateReport(reportName, true);

            }
            else
            {
                bool showUnit = false;
                bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestIsShowUnit"), out showUnit);
                DataTable dt = this.GetReportDT11(report, elementList, showUnit);//获取打印数据

                bool flag = false;
                bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestStatistics"), out flag);
                string strStaticContent = ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestStaContent");
                bool bFullName = false;
                try
                {
                    bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestFullElemName"), out bFullName);
                }
                catch
                {
                    bFullName = false;
                }
                bool bNeedTimesResult = true;
                try
                {
                    bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestNeedMulResults"), out bNeedTimesResult);
                }
                catch
                {
                    bNeedTimesResult = true;
                }

                List<string> statics = (strStaticContent == null || strStaticContent.Trim() == string.Empty) ? null : strStaticContent.Split(',').ToList();
                if (flag)
                    // AddStaticsRows(dt, "time");
                    DifferenceDevice.interClassMain.AddStaticsRows(dt, "time", statics);
                if (bFullName)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        //if (ReportTemplateHelper.ExcelModeType == 21)
                        //{
                        //    Atom atom = Atoms.AtomList.Find(s => s.AtomName == curEle.Caption);
                        //    string atomNameCN = (atom == null) ? "" : atom.AtomNameCN;
                        //    showElem = Lang.Model.CurrentLang.IsDefaultLang ? atomNameCN + "(" + curEle.Caption + ")" : curEle.Caption;
                        //}
                        //else
                        //    showElem = curEle.Caption;

                        Atom atom = Atoms.AtomList.Find(s => s.AtomName == dc.ColumnName);
                        if (atom == null) continue;
                        string atomNameCN = (atom == null) ? "" : atom.AtomNameCN;
                        string atomNameEN = (atom == null) ? "" : atom.AtomNameEN;
                        dc.Caption = Skyray.Language.Lang.Model.CurrentLang.IsDefaultLang ? atomNameCN : atomNameEN;
                    }
                }
                if (ReportTemplateHelper.ExcelModeType == 21 || ReportTemplateHelper.ExcelModeType == 26)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        Atom atom = Atoms.AtomList.Find(s => s.AtomName == dc.ColumnName);
                        if (atom == null) continue;
                        string atomNameCN = (atom == null) ? "" : atom.AtomNameCN;
                        dc.Caption = Skyray.Language.Lang.Model.CurrentLang.IsDefaultLang ? atomNameCN + "(" + dc.ColumnName + ")" : dc.ColumnName;
                    }
                }
                report.InterestElemCount = elementList.Items.ToList().FindAll(w => w.IsShowElement).Count;
                report.RetestFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.ManyTimeTemplate;
                report.ReportPath = WorkCurveHelper.ExcelPath;
                report.NeedMultiResults = bNeedTimesResult;







                ReportSavedPath = report.GenerateRetestReport(reportName, dt, true, true);
            }

            if (ReportSavedPath == string.Empty || !File.Exists(ReportSavedPath))
                return;
            else
            {
                OnPrintTemplate(ReportSavedPath);
            }


            #endregion

        }

        private void PrintModel30()
        {
            #region
            ExcelTemplateParams.GetExcelTemplateParams();
            Report rpt = new BengalReport();
            rpt.ReportFileType = 0;
            var elementList = ElementList.New;
            var elements = new List<HistoryElement>();//增加历史元素，以便建立DataTable调用 Strong 2012/10/25

            if (selectLong.Count > WorkCurveHelper.PrintExcelCount)
            {
                Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
                return;
            }

            if (!this.AddReportParameters(rpt, elementList, ref elements, 9))//获取Report基本参数{9代表打印模式}
                return;

            string reportName = GetDefineReportName();
            string ReportSavedPath = string.Empty;

            rpt.IsAnalyser = true;
            if (selectLong.Count == 1)
            {
                rpt.InterestElemCount = elementList.Items.ToList().FindAll(w => w.IsShowElement).Count;
                rpt.TempletFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.OneTimeTemplate;
                rpt.ReportPath = WorkCurveHelper.ExcelPath;
                ReportSavedPath = rpt.GenerateReport(reportName, true);

            }
            else
            {
                DataTable dt = this.GetReportDT9(rpt, elementList);//获取打印数据
                bool bFullName = true;
                bool bNeedTimesResult = false;
                if (bFullName)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        Atom atom = Atoms.AtomList.Find(s => s.AtomName == dc.ColumnName);
                        if (atom == null) continue;
                        string atomNameCN = (atom == null) ? "" : atom.AtomNameCN;
                        string atomNameEN = (atom == null) ? "" : atom.AtomNameEN;
                        dc.Caption = Skyray.Language.Lang.Model.CurrentLang.IsDefaultLang ? atomNameCN : atomNameEN;
                    }
                }
                rpt.InterestElemCount = elementList.Items.ToList().FindAll(w => w.IsShowElement).Count;
                rpt.RetestFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.ManyTimeTemplate;
                rpt.ReportPath = WorkCurveHelper.ExcelPath;
                rpt.NeedMultiResults = bNeedTimesResult;
                ReportSavedPath = rpt.GenerateRetestReport(reportName, dt, true, true);
            }

            if (ReportSavedPath == string.Empty || !File.Exists(ReportSavedPath))
                return;
            else
            {
                OnPrintTemplate(ReportSavedPath);
            }


            #endregion

        }

        private void PrintModel31()
        {
            #region
            ExcelTemplateParams.GetExcelTemplateParams();
            Report rpt = new LYReport();
            rpt.ReportFileType = 0;
            var elementList = ElementList.New;
            var elements = new List<HistoryElement>();//增加历史元素，以便建立DataTable调用 Strong 2012/10/25

            if (selectLong.Count > WorkCurveHelper.PrintExcelCount)
            {
                Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
                return;
            }

            if (!this.AddReportParameters(rpt, elementList, ref elements, 9))//获取Report基本参数{9代表打印模式}
                return;

            string reportName = GetDefineReportName();
            string ReportSavedPath = string.Empty;

            rpt.IsAnalyser = true;
            if (selectLong.Count == 1)
            {
                rpt.InterestElemCount = elementList.Items.ToList().FindAll(w => w.IsShowElement).Count;
                rpt.TempletFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.OneTimeTemplate;
                rpt.ReportPath = WorkCurveHelper.ExcelPath;
                ReportSavedPath = rpt.GenerateReport(reportName, true);

            }
            else
            {
                DataTable dt = this.GetReportDT31(rpt, elementList);//获取打印数据
                rpt.InterestElemCount = elementList.Items.ToList().FindAll(w => w.IsShowElement).Count;
                rpt.RetestFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.ManyTimeTemplate;
                rpt.ReportPath = WorkCurveHelper.ExcelPath;
                rpt.NeedMultiResults = true;
                ReportSavedPath = rpt.GenerateRetestReport(reportName, dt, true, true);
            }

            if (ReportSavedPath == string.Empty || !File.Exists(ReportSavedPath))
                return;
            else
            {
                OnPrintTemplate(ReportSavedPath);
            }


            #endregion

        }
        private DataTable GetReportDT9(Report report, ElementList elementList)
        {
            var m = "au";
            if (elementList != null && elementList.MainElementToCalcKarat != null && elementList.MainElementToCalcKarat.Trim().Length > 0)
            {
                m = elementList.MainElementToCalcKarat.Trim().ToLower();
            }
            DataTable dt = CreateReTestTable(elementList);
            if (DifferenceDevice.IsShowKarat && dt.Columns.Contains(m)) dt.Columns.Add("Karat");//新增K值列
            int cont = 0;
            report.elementListPDF.Clear();
            string[] strLayerElems = Helper.ToStrs(elementList.LayerElemsInAnalyzer == null ? "" : elementList.LayerElemsInAnalyzer);
            for (int j = 0; j < selectLong.Count; j++)
            {
                DataRow rowNew = dt.NewRow();
                rowNew["Time"] = ++cont;
                double dKValue = 0;
                foreach (DataGridViewColumn column in this.dgvHistoryRecord.Columns)
                {
                    //删除不显示的部分元素
                    if (!column.Visible)
                    {
                        if (dt.Columns.Contains(column.HeaderText))
                            dt.Columns.Remove(column.HeaderText);
                        continue;
                    }
                    HistoryElement element = HistoryElement.FindOne(w => w.elementName == column.HeaderText && w.HistoryRecord.Id == selectLong[j]);
                    if (element != null && column.Visible && dt.Columns.Contains(column.HeaderText))
                    {
                        report.elementListPDF.Add(element);
                        string valueStr = element.contextelementValue;
                        // if (elementList.RhIsLayer && element.elementName.ToUpper().Equals("RH"))
                        if (elementList.RhIsLayer && strLayerElems.Contains(element.elementName))
                        {
                            valueStr = element.thickelementValue;
                            rowNew[column.HeaderText] = string.IsNullOrEmpty(valueStr) ? "" : double.Parse(valueStr).ToString("f" + WorkCurveHelper.ThickBit.ToString()) + "(um)";
                            continue;
                        }
                        if (!string.IsNullOrEmpty(valueStr))//如果为空将导致dt为空
                        {
                            double Value = double.Parse(valueStr);
                            //if (element.unitValue == 1)
                            //    Value = Value * 10000;
                            //else if (element.unitValue == 3)
                            //    Value = Value * 10;

                            //if (element.unitValue == 1)
                            //{
                            //    rowNew[column.HeaderText] = (Value / 10000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(%)";
                            //}
                            //else if (element.unitValue == 2)
                            //{
                            //    rowNew[column.HeaderText] = Value.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(ppm)";
                            //}
                            //else
                            //{
                            //    rowNew[column.HeaderText] = (Value / 10).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(‰)";
                            //}
                            rowNew[column.HeaderText] = Value.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + (element.unitValue == 1 ? "(%)" : (element.unitValue == 2 ? "(ppm)" : "(‰)"));
                            if (element.unitValue == 2)
                                Value = Value / 10000;
                            else if (element.unitValue == 3)
                                Value = Value / 10;
                            if (element.elementName.ToLower().Equals(m)) dKValue = Value * 24 / WorkCurveHelper.KaratTranslater;
                        }
                    }
                    else if (element == null && column.Visible && dt.Columns.Contains(column.HeaderText))//无数据时处理方式
                    {
                        rowNew[column.HeaderText] = default(double).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
                    }

                }
                if (countryId == 0 && dt.Columns.Contains(Info.TestResult))
                    rowNew[Info.TestResult] = ExcelTemplateParams.TestRetultForEdx(selectLong[j].ToString());
                if (dt.Columns.Contains("Karat")) rowNew["Karat"] = dKValue.ToString("F3");
                dt.Rows.Add(rowNew);
            }

            return dt;

        }

        private DataTable GetReportDT31(Report report, ElementList elementList)
        {
            //var m = "au";
            //if (elementList != null && elementList.MainElementToCalcKarat != null && elementList.MainElementToCalcKarat.Trim().Length > 0)
            //{
            //    m = elementList.MainElementToCalcKarat.Trim().ToLower();
            //}
            DataTable dt = CreateReTestTable(elementList);
            //if (DifferenceDevice.IsShowKarat && dt.Columns.Contains(m)) dt.Columns.Add("Karat");//新增K值列
            int cont = 0;
            report.elementListPDF.Clear();
            string[] strLayerElems = Helper.ToStrs(elementList.LayerElemsInAnalyzer == null ? "" : elementList.LayerElemsInAnalyzer);
            for (int j = 0; j < selectLong.Count; j++)
            {
                DataRow rowNew = dt.NewRow();
                rowNew["Time"] = ++cont;
                double dKValue = 0;
                foreach (DataGridViewColumn column in this.dgvHistoryRecord.Columns)
                {
                    //删除不显示的部分元素
                    if (!column.Visible || column.HeaderText == Info.TestResult)
                    {
                        if (dt.Columns.Contains(column.HeaderText))
                            dt.Columns.Remove(column.HeaderText);
                        continue;
                    }
                    HistoryElement element = HistoryElement.FindOne(w => w.elementName == column.HeaderText && w.HistoryRecord.Id == selectLong[j]);
                    if (element != null && column.Visible && dt.Columns.Contains(column.HeaderText))
                    {
                        report.elementListPDF.Add(element);
                        string valueStr = element.contextelementValue;
                        // if (elementList.RhIsLayer && element.elementName.ToUpper().Equals("RH"))
                        if (elementList.RhIsLayer && strLayerElems.Contains(element.elementName))
                        {
                            valueStr = element.thickelementValue;
                            rowNew[column.HeaderText] = string.IsNullOrEmpty(valueStr) ? "" : double.Parse(valueStr).ToString("f" + WorkCurveHelper.ThickBit.ToString()) + "(um)";
                            continue;
                        }
                        var unit = element.unitValue == 1 ? "(%)" : (element.unitValue == 2 ? "(ppm)" : "(‰)");
                        if (!string.IsNullOrEmpty(valueStr))//如果为空将导致dt为空
                        {
                            double Value = double.Parse(valueStr);

                            rowNew[column.HeaderText] = Value.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());

                            if (element.unitValue == 2)
                                Value = Value / 10000;
                            else if (element.unitValue == 3)
                                Value = Value / 10;
                            //if (element.elementName.ToLower().Equals(m)) dKValue = Value * 24 / WorkCurveHelper.KaratTranslater;
                        }
                    }
                    else if (element == null && column.Visible && dt.Columns.Contains(column.HeaderText))//无数据时处理方式
                    {
                        rowNew[column.HeaderText] = default(double).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
                    }

                }
                //if (dt.Columns.Contains("Karat")) rowNew["Karat"] = dKValue.ToString("F3");
                dt.Rows.Add(rowNew);
            }

            return dt;

        }
        #endregion

        #region 打印模式11
        private void PrintModel11()
        {
            List<string> SeleWorkCurveNameList = new List<string>();
            #region
            ExcelTemplateParams.GetExcelTemplateParams();
            Report report = new Report();
            ElementList elementList = ElementList.New;
            List<HistoryElement> elements = new List<HistoryElement>();//增加历史元素，以便建立DataTable调用 Strong 2012/10/25
            if (selectLong.Count > WorkCurveHelper.PrintExcelCount)
            {
                Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
                return;
            }

            string reportName = GetDefineReportName();

            if (!this.AddReportParameters(report, elementList, ref elements, 11))//获取Report基本参数{11代表打印模式}
                return;
            bool showUnit = false;
            bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestIsShowUnit"), out showUnit);
            DataTable dt = this.GetReportDT11(report, elementList, showUnit);//获取打印数据

            bool flag = false;
            bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestStatistics"), out flag);
            string strSavedPath = string.Empty;
            if (flag && selectLong.Count > 1)
            {
                AddStaticsRows(dt, "time");
                dt.Rows.RemoveAt(dt.Rows.Count - 3);
                dt.Rows.RemoveAt(dt.Rows.Count - 3);
                dt.Rows.RemoveAt(dt.Rows.Count - 3);
            }
            report.InterestElemCount = elementList.Items.ToList().FindAll(w => w.IsShowElement).Count;
            report.RetestFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.OneTimeTemplate;
            report.ReportPath = WorkCurveHelper.ExcelPath;
            strSavedPath = report.GenerateRetestReport(reportName, dt, true, false);

            if (strSavedPath == string.Empty || !File.Exists(strSavedPath))
                return;
            OnPrintTemplate(strSavedPath);
            #endregion
        }

        private DataTable GetReportDT11(Report report, ElementList elementList, bool showUnit)
        {
            DataTable dt = CreateReTestTable(elementList);
            int cont = 0;
            string sContentBit = WorkCurveHelper.SoftWareContentBit.ToString();
            for (int j = 0; j < selectLong.Count; j++)
            {
                DataRow rowNew = dt.NewRow();
                rowNew["Time"] = ++cont;
                foreach (DataGridViewColumn column in this.dgvHistoryRecord.Columns)
                {
                    HistoryElement element = HistoryElement.FindOne(w => w.elementName == column.HeaderText && w.HistoryRecord.Id == selectLong[j]);
                    if (element != null && column.Visible && dt.Columns.Contains(column.HeaderText))
                    {
                        string valueStr = element.contextelementValue;
                        if (!string.IsNullOrEmpty(valueStr))//如果为空将导致dt为空
                        {
                            double Value = double.Parse(valueStr);
                            // rowNew[column.HeaderText] = Value.ToString("f" + sContentBit);
                            if (showUnit)
                            {
                                if (element.unitValue == 1)
                                    Value = Value * 10000;
                                else if (element.unitValue == 3)
                                    Value = Value * 1000;

                                if (element.unitValue == 1)
                                {
                                    rowNew[column.HeaderText] = (Value / 10000).ToString("f" + sContentBit) + "(%)";
                                }
                                else if (element.unitValue == 2)
                                {
                                    rowNew[column.HeaderText] = Value.ToString("f" + sContentBit) + "(ppm)";
                                }
                                else
                                {
                                    rowNew[column.HeaderText] = (Value / 1000).ToString("f" + sContentBit) + "(‰)";
                                }
                            }
                            else
                                rowNew[column.HeaderText] = Value.ToString("f" + sContentBit);
                        }
                    }
                    else if (element == null && column.Visible && dt.Columns.Contains(column.HeaderText))//无数据时处理方式
                    {
                        rowNew[column.HeaderText] = default(double).ToString("f" + sContentBit);
                    }
                }
                dt.Rows.Add(rowNew);
            }

            return dt;
        }
        #endregion


        #region 打印模式12
        private void PrintModel12()
        {
            #region
            ExcelTemplateParams.GetExcelTemplateParams();
            Report report = new Report();
            ElementList elementList = ElementList.New;
            List<HistoryElement> elements = new List<HistoryElement>();//增加历史元素，以便建立DataTable调用 Strong 2012/10/25
            if (selectLong.Count > WorkCurveHelper.PrintExcelCount)
            {
                Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
                return;
            }

            string reportName = GetDefineReportName();

            if (!this.AddReportParameters(report, elementList, ref elements, 12))//获取Report基本参数{12代表打印模式}
                return;

            //DataTable dt = this.GetReportDT12(report, elementList);//获取打印数据

            DataTable dt = null;
            if (ReportTemplateHelper.ExcelModeType == 12)
                dt = DifferenceDevice.interClassMain.GetReportDT12(report, elementList, selectLong);//获取打印数据
            else if (ReportTemplateHelper.ExcelModeType == 25)
                dt = DifferenceDevice.interClassMain.GetReportDT25(report, elementList, elements, selectLong);//获取打印数据
            bool flag = false;
            bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestStatistics"), out flag);
            if (flag && selectLong.Count >= 2)
            {
                
                try
                {
                    AddStaticsRows(dt, Info.SerialNumber);
                }
                catch
                {

                    Msg.Show("所选历史记录存在无法统计的数据值");
                    return;

                }
            }
            report.InterestElemCount = elementList.Items.ToList().FindAll(w => w.IsShowElement).Count;
            report.RetestFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.ManyTimeTemplate;
            report.ReportPath = WorkCurveHelper.ExcelPath;


            string temp = "";
            int loc = 0;

            this.dataGridViewW2.DataSource = dt;

            for (int i = 0; i < this.dataGridViewW2.Columns.Count; i++)
            {
                if (this.dataGridViewW2.Columns[i].HeaderText.Contains("厚度") || this.dataGridViewW2.Columns[i].HeaderText.Contains("Thickness") || this.dataGridViewW2.Columns[i].HeaderText.Contains("面密度") || this.dataGridViewW2.Columns[i].HeaderText.Contains("Density") || this.dataGridViewW2.Columns[i].HeaderText.Contains("含量") || this.dataGridViewW2.Columns[i].HeaderText.Contains("Concentration"))
                {

                    for (int j = 0; j < this.dataGridViewW2.Rows.Count; j++)
                    {

                        temp = this.dataGridViewW2.Rows[j].Cells[i].Value.ToString();
                        loc = temp.IndexOf('(');


                        if (loc > 0)
                        {
                            this.dataGridViewW2.Rows[j].Cells[i].Value = temp.Substring(0, loc).Trim();

                        }
                        
                    }


                    if (this.dataGridViewW2.Columns[i].HeaderText.Contains("厚度") || this.dataGridViewW2.Columns[i].HeaderText.Contains("Thickness"))
                    {
                        for (int index = 0; index < WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count; index++)
                        {
                            if (WorkCurveHelper.WorkCurveCurrent.ElementList.Items[index].IsShowElement)
                            {
                                if (WorkCurveHelper.WorkCurveCurrent.ElementList.Items[index].ThicknessUnit == ThicknessUnit.ur)
                                {
                                    dt.Columns[i].Caption = dt.Columns[i].Caption + "(" + this.cmbThicknessUnit.Items[0] + ")";
                                }
                                else if (WorkCurveHelper.WorkCurveCurrent.ElementList.Items[index].ThicknessUnit == ThicknessUnit.um)
                                {
                                    dt.Columns[i].Caption = dt.Columns[i].Caption + "(" + this.cmbThicknessUnit.Items[1]  + ")";

                                }
                                else
                                {
                                    dt.Columns[i].Caption = dt.Columns[i].Caption + "(" + this.cmbThicknessUnit.Items[2] + ")";

                                }

                                break;
                            }
                        }


                    }
                    else if (this.dataGridViewW2.Columns[i].HeaderText.Contains("面密度") || this.dataGridViewW2.Columns[i].HeaderText.Contains("Density"))
                    {
                        if (WorkCurveHelper.WorkCurveCurrent.AreaThickType == "g/m^2")
                            dt.Columns[i].Caption = dt.Columns[i].Caption + "(" + this.cmbDensityUnit.Items[0] + ")";
                        else
                            dt.Columns[i].Caption = dt.Columns[i].Caption + "(" + this.cmbDensityUnit.Items[1] + ")";
                    }
                }


                if (this.dataGridViewW2.Columns[i].HeaderText.Contains("样品名称") || this.dataGridViewW2.Columns[i].HeaderText.Contains("Sample"))
                {
                    for (int j = 0; j < this.dataGridViewW2.Rows.Count; j++)
                    {

                        if (this.dataGridViewW2.Rows[j].Cells[i].Value.ToString().Contains("#"))
                        {
                            this.dataGridViewW2.Rows[j].Cells[i].Value = this.dataGridViewW2.Rows[j].Cells[i].Value.ToString().Split(new char[] { '#' })[0];

                        }
                    }

                }
            }



            //report.GenerateThickRetestReport(reportName, dt, true);
            string strSavedPath = report.GenerateRetestReport(reportName, dt, flag, false);
            if (strSavedPath == string.Empty || !File.Exists(strSavedPath))
                return;
            else
            {
                OnPrintTemplate(strSavedPath);
            }


            #endregion
        }





        #region 打印模式19
        private void PrintModel19()
        {
            #region
            ExcelTemplateParams.GetExcelTemplateParams();
            Report report = new Report();
            ElementList elementList = ElementList.New;
            List<HistoryElement> elements = new List<HistoryElement>();//增加历史元素，以便建立DataTable调用 Strong 2012/10/25
            if (selectLong.Count > WorkCurveHelper.PrintExcelCount)
            {
                Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
                return;
            }

            string reportName = GetDefineReportName();

            if (!this.AddReportParameters(report, elementList, ref elements, 12))//获取Report基本参数{12代表打印模式}
                return;

            //DataTable dt = this.GetReportDT12(report, elementList);//获取打印数据
            if (WorkCurveHelper.CurrentStandard != null)
                report.historyRecordid = WorkCurveHelper.CurrentStandard.Id.ToString();
            DataTable dt = DifferenceDevice.interClassMain.GetReportDT19(report, elementList, selectLong);//获取打印数据
            bool flag = false;
            bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestStatistics"), out flag);
            if (flag && selectLong.Count >= 2)
                AddStaticsRows(dt, Info.SerialNumber);
            report.InterestElemCount = elementList.Items.ToList().FindAll(w => w.IsShowElement).Count;
            report.RetestFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.ManyTimeTemplate;
            report.ReportPath = WorkCurveHelper.ExcelPath;
            //report.GenerateThickRetestReport(reportName, dt, true);
            string strSavedPath = report.GenerateRetestReport(reportName, dt, flag, false);
            if (strSavedPath == string.Empty || !File.Exists(strSavedPath))
                return;
            else
            {
                OnPrintTemplate(strSavedPath);
            }


            #endregion
        }
        #endregion
        //private DataTable GetReportDT12(Report report, ElementList elementList)
        //{
        //    #region

        //    WorkCurve workcurve=WorkCurve.FindById(HistoryRecord.FindById(selectLong[0]).WorkCurveId);
        //    bool IsShowError = (ReportTemplateHelper.LoadSpecifiedValue("OpenHistoryRecordType", "IsShowError") == "0") ? false : true;
        //    DataTable dt = new DataTable();
        //    dt.Columns.Clear();
        //    dt.Columns.Add(Info.SerialNumber, typeof(string));
        //    dt.Columns.Add(Info.SampleName, typeof(string));


        //   // var orders = elementList.Items.OrderBy(w => w.LayerNumber).Select(w => w.Caption);//排序去取元素名 所有感兴趣元素
        //    var orders = elementList.Items.OrderBy(w => w.LayerNumber);      //yuzhaomodify:加入层数显示
        //    IList<string> ThickCollection = new List<string>();
        //    IList<string> OtherCollection = new List<string>();
        //    foreach (var element in orders)
        //    {
        //        if (
        //            !ThickCollection.Contains(element.Caption + " " + element.LayerNumber + " " + Info.Thick) &&
        //            !OtherCollection.Contains(element.Caption + " " + element.LayerNumber + " " + Info.EditContent) &&
        //            !OtherCollection.Contains(element.Caption + " " + element.LayerNumber + " " + Info.strAreaDensity))
        //        {
        //            if (elementList.Items.ToList().Find(a => a.Caption == element.Caption && a.IsShowElement) != null)
        //            {
        //                if (workcurve != null && workcurve.IsThickShowAreaThick)
        //                {
        //                    OtherCollection.Add(element.Caption + " " + element.LayerNumber + " " + Info.strAreaDensity);   //yuzhaomodify:加入层数显示
        //                }
        //                else 
        //                {
        //                    if (ThickCollection.Count > 0 && ThickCollection[ThickCollection.Count - 1].Contains(element.LayerNumber.ToString()))
        //                    {
        //                        ThickCollection[ThickCollection.Count - 1] = ThickCollection[ThickCollection.Count - 1].Split(' ')[0] + "|" +
        //                                                                      element.Caption + " " + element.LayerNumber + " " + Info.Thick;
        //                    }
        //                    else
        //                        ThickCollection.Add(element.Caption + " " + element.LayerNumber + " " + Info.Thick);            //yuzhaomodify:加入层数显示
        //                }
        //                if (WorkCurveHelper.CalcType != CalcType.PeakDivBase)
        //                {
        //                    if (elementList.Items.ToList().Find(a => a.Caption == element.Caption && a.IsShowContent) != null)
        //                        OtherCollection.Add(element.Caption + " " + element.LayerNumber + " " + Info.EditContent);  //yuzhaomodify:加入层数显示
        //                }
        //                if (IsShowError) OtherCollection.Add(element.Caption + " " + element.LayerNumber + " " + Info.strError);  //yuzhaomodify:加入层数显示
        //            }
        //        }

        //    }
        //    //foreach (string s1 in ThickCollection)
        //    //    dt.Columns.Add(s1,typeof(string));
        //    //foreach (string s2 in OtherCollection)
        //    //    dt.Columns.Add(s2,typeof(string));
        //    if(orders != null && orders.Count() > 1)
        //    {
        //        int max = orders.ElementAt<CurveElement>(orders.Count() - 1).LayerNumber;
        //        for (int i = 1; i < max + 1; i++)
        //        {
        //            foreach (string s1 in ThickCollection)
        //            {
        //                if (s1.Split(' ')[1] == i.ToString())
        //                    dt.Columns.Add(s1, typeof(string));
        //            }
        //            foreach (string s2 in OtherCollection)
        //            {
        //                if (s2.Split(' ')[1] == i.ToString())
        //                    dt.Columns.Add(s2, typeof(string));
        //            }
        //        }
        //      }

        //    int cont = 0;
        //    for (int j = 0; j < selectLong.Count; j++)
        //    {
        //        DataRow rowNew = dt.NewRow();
        //        rowNew[Info.SerialNumber] = ++cont;
        //        HistoryRecord itemHistory = HistoryRecord.FindOne(w => w.Id == selectLong[j]);
        //        rowNew[Info.SampleName] = itemHistory.SampleName;
        //        foreach (DataColumn column in dt.Columns)
        //        {
        //            if (column.Caption == Info.SerialNumber) continue;
        //            HistoryElement element = HistoryElement.FindOne(w => w.elementName == column.Caption.Split(' ')[0].Split('|')[0] && w.HistoryRecord.Id == selectLong[j]);
        //            if (element != null)
        //            {
        //                if (column.Caption.Split(' ')[2] == Info.EditContent)
        //                {
        //                    string valueStr = element.contextelementValue;
        //                    if (!string.IsNullOrEmpty(valueStr))//如果为空将导致dt为空
        //                    {
        //                        double Value = double.Parse(valueStr);
        //                        if (element.unitValue == 1)
        //                            Value = Value * 10000;
        //                        else if (element.unitValue == 3)
        //                            Value = Value * 1000;

        //                        if (element.unitValue == 1)
        //                        {
        //                            rowNew[column.Caption] = (Value / 10000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(%)";
        //                        }
        //                        else if (element.unitValue == 2)
        //                        {
        //                            rowNew[column.Caption] = Value.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(ppm)";
        //                        }
        //                        else
        //                        {
        //                            rowNew[column.Caption] = (Value / 1000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(‰)";
        //                        }


        //                    }
        //                }
        //                else if (column.Caption.Split(' ')[2] == Info.Thick)
        //                {
        //                    if (element.thickunitValue == 1)
        //                    {
        //                        rowNew[column.Caption] = double.Parse(element.thickelementValue).ToString("f" + WorkCurveHelper.ThickBit.ToString()) + "(u〞)";
        //                    }
        //                    else
        //                    {
        //                        rowNew[column.Caption] = double.Parse(element.thickelementValue).ToString("f" + WorkCurveHelper.ThickBit.ToString()) + "(um)";
        //                    }
        //                }
        //                else if (column.Caption.Split(' ')[2] == Info.strError)
        //                {
        //                    rowNew[column.Caption] = element.Error.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
        //                }
        //                else if (column.Caption.Split(' ')[2] == Info.strAreaDensity)
        //                {
        //                    rowNew[column.Caption] = double.Parse(element.densityelementValue).ToString("f" + WorkCurveHelper.ThickBit.ToString())+"("+(element.densityunitValue==string.Empty?Info.strAreaDensityUnit:element.densityunitValue)+")";
        //                }

        //            }
        //            else
        //            {
        //                rowNew[column.Caption] = double.Parse("0").ToString("f" + ((column.Caption.Split(' ')[2] == Info.Thick) ? WorkCurveHelper.ThickBit.ToString() : WorkCurveHelper.SoftWareContentBit.ToString())) + " ";

        //            }
        //        }
        //        dt.Rows.Add(rowNew);
        //    }
        //    #endregion
        //    return dt;
        //}
        #endregion

        #region 打印模式13
        private void PrintModel13()
        {
            List<string> SeleWorkCurveNameList = new List<string>();
            #region
            ExcelTemplateParams.GetExcelTemplateParams();
            Report report = new Report();
            report.IsAnalyser = true;
            ElementList elementList = ElementList.New;
            List<HistoryElement> elements = new List<HistoryElement>();//增加历史元素，以便建立DataTable调用 Strong 2012/10/25
            if (selectLong.Count > WorkCurveHelper.PrintExcelCount)
            {
                Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
                return;
            }

            if (!this.AddReportParameters(report, elementList, ref elements, 13))//获取Report基本参数{9代表打印模式}
                return;

            string reportName = GetDefineReportName();
            string strSavePath = string.Empty;

            if (selectLong.Count == 1)
            {
                report.InterestElemCount = elementList.Items.ToList().FindAll(w => w.IsShowElement).Count;
                report.TempletFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.OneTimeTemplate;
                report.ReportPath = WorkCurveHelper.ExcelPath;
                strSavePath = report.GenerateReport(reportName, true);
            }
            else
            {
                DataTable dt = this.GetReportDT13(report, elementList);//获取打印数据

                bool flag = false;
                bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestStatistics"), out flag);
                if (flag)
                    AddStaticsRows(dt, "NAMES", 13);

                report.InterestElemCount = elementList.Items.ToList().FindAll(w => w.IsShowElement).Count;
                report.RetestFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.ManyTimeTemplate;
                report.ReportPath = WorkCurveHelper.ExcelPath;
                strSavePath = report.GenerateRetestReport(reportName, dt, true, false);
            }
            if (strSavePath == string.Empty || !File.Exists(strSavePath))
                return;
            else
            {
                OnPrintTemplate(strSavePath);
            }
            #endregion
        }

        private DataTable GetReportDT13(Report report, ElementList elementList)
        {
            #region
            DataTable dt = new DataTable();
            dt.Columns.Clear();
            dt.Columns.Add("NAMES", typeof(string));
            var orders = elementList.Items.ToList().FindAll(w => w.IsShowElement).OrderBy(w => w.RowsIndex).Select(w => w.Caption);//排序去取元素名 所有感兴趣元素
            foreach (var element in orders)
            {
                string elem = "";
                Atom ato = Atoms.AtomList.Find(s => s.AtomName == element);
                elem = (ato == null) ? elem : ato.AtomNameEN + "(" + ato.AtomName + ")";
                if (!dt.Columns.Contains(element))
                {
                    DataColumn dc = new DataColumn(element);
                    dc.Caption = elem;
                    dt.Columns.Add(dc);
                }
            }
            dt.Columns.Add("KARAT (kt)", typeof(string));

            for (int j = 0; j < selectLong.Count; j++)
            {
                DataRow rowNew = dt.NewRow();
                HistoryRecord HR = HistoryRecord.FindById(selectLong[j]);
                if (HR != null)
                    rowNew["NAMES"] = HR.SampleName;
                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName.Equals("KARAT (kt)"))
                    {
                        HistoryElement element = HistoryElement.FindOne(w => w.elementName == "Au" && w.HistoryRecord.Id == selectLong[j]);
                        if (element != null)
                            rowNew[column.Caption] = (double.Parse(element.contextelementValue) * 24 / WorkCurveHelper.KaratTranslater).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
                    }
                    else
                    {
                        HistoryElement element = HistoryElement.FindOne(w => w.elementName == column.ColumnName && w.HistoryRecord.Id == selectLong[j]);
                        if (element != null)
                        {
                            string valueStr = element.contextelementValue;
                            if (!string.IsNullOrEmpty(valueStr))//如果为空将导致dt为空
                            {
                                double Value = double.Parse(valueStr);
                                if (element.unitValue == 1)
                                    Value = Value * 10000;
                                else if (element.unitValue == 3)
                                    Value = Value * 1000;

                                if (element.unitValue == 1)
                                {
                                    rowNew[column.ColumnName] = (Value / 10000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(%)";
                                }
                                else if (element.unitValue == 2)
                                {
                                    rowNew[column.ColumnName] = Value.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(ppm)";
                                }
                                else
                                {
                                    rowNew[column.ColumnName] = (Value / 1000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(‰)";
                                }
                            }
                        }
                    }
                }
                dt.Rows.Add(rowNew);
            }
            #endregion

            return dt;
        }
        #endregion

        #region 打印模式Default
        private void PrintModelDefault()
        {
            List<string> SeleWorkCurveNameList = new List<string>();
            string StrSavePath = string.Empty;
            int reportType = comboBoxReportType.Text.ToLower() == "pdf" ? 1 : 0;
            #region
            ExcelTemplateParams.GetExcelTemplateParams();
            Report report = new Report();
            report.ReportFileType = reportType;
            report.isShowND = WorkCurveHelper.isShowND && DifferenceDevice.IsRohs;
            report.IsAnalyser = (ReportTemplateHelper.ExcelModeType == 15 || ReportTemplateHelper.ExcelModeType == 6 || ReportTemplateHelper.ExcelModeType == 9) ? true : false;
            ElementList elementList = ElementList.New;
            List<HistoryElement> elements = new List<HistoryElement>();//增加历史元素，以便建立DataTable调用 Strong 2012/10/25
            if (selectLong.Count > WorkCurveHelper.PrintExcelCount)
            {
                Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
                return;
            }
            else
            {

                if (!this.AddReportParameters(report, elementList, ref elements, 0))//获取Report基本参数{0代表其他的模式}
                    return;

                string reportName = GetDefineReportName();//获取打印模版名称

                report.bProtect = false;
                if (selectLong.Count == 1)
                {
                    report.InterestElemCount = elementList.Items.ToList().FindAll(w => w.IsShowElement).Count;
                    report.TempletFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.OneTimeTemplate;
                    report.ReportPath = WorkCurveHelper.ExcelPath;
                    StrSavePath = report.GenerateReport(reportName, true);
                }
                else
                {
                    DataTable dt = this.GetReportDTOther(report, elementList);//获取打印数据

                    report.InterestElemCount = elementList.Items.ToList().FindAll(w => w.IsShowElement).Count;
                    List<long> lstEleId = new List<long>();
                    foreach (var ele in elementList.Items)
                    {
                        lstEleId.Add(ele.Id);
                    }
                    if (lstEleId.Distinct().Count() > (string.IsNullOrEmpty(ExcelTemplateParams.TotalColumnMun) ? 9 : int.Parse(ExcelTemplateParams.TotalColumnMun)))
                        if (File.Exists(Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.ManyTimeTemplate.TrimEnd(".xls".ToCharArray()) + "_Horizontal.xls"))
                            ExcelTemplateParams.ManyTimeTemplate = ExcelTemplateParams.ManyTimeTemplate.TrimEnd(".xls".ToCharArray()) + "_Horizontal.xls";
                    report.RetestFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.ManyTimeTemplate;
                    report.ReportPath = WorkCurveHelper.ExcelPath;
                    StrSavePath = report.GenerateRetestReport(reportName, dt, true, false);

                }

                //if (!File.Exists(report.ReportPath + reportName + ".xls") && !File.Exists(report.ReportPath + reportName + "_Retry.xls"))
                if (!File.Exists(StrSavePath))
                    return;
                else
                {
                    OnPrintTemplate(StrSavePath);
                    //DifferenceDevice.interClassMain.OpenPathThread(StrSavePath);
                    //string excelOnePath = report.ReportPath + reportName + ".xls";
                    //string excelManyPath = report.ReportPath + reportName + "_Retry" + ".xls";
                    //if (selectLong.Count == 1)
                    //    DifferenceDevice.interClassMain.OpenPathThread(excelOnePath);
                    //else
                    //    DifferenceDevice.interClassMain.OpenPathThread(excelManyPath);
                }
            }
            #endregion
        }

        private DataTable GetReportDTOther(Report report, ElementList elementList)
        {
            #region
            DataTable dt = CreateReTestTable(elementList);
            CustomStandard standard = null;
            if (report.historyStandID > -1)
            {
                standard = CustomStandard.FindById(report.historyStandID);
                if (standard != null && standard.StandardDatas != null && standard.StandardDatas.Count > 0 && standard.IsSelectTotal)
                {
                    dt.Columns.Add(Info.TotalPassReslt);
                }
            }
            int cont = 0;
            for (int j = 0; j < selectLong.Count; j++)
            {
                DataRow rowNew = dt.NewRow();
                rowNew["Time"] = ++cont;
                double totalContent = 0d;
                foreach (DataGridViewColumn column in this.dgvHistoryRecord.Columns)
                {
                    HistoryElement element = HistoryElement.FindOne(w => w.elementName == column.HeaderText && w.HistoryRecord.Id == selectLong[j]);
                    if (element != null && column.Visible && dt.Columns.Contains(column.HeaderText))
                    {
                        string valueStr = element.contextelementValue;
                        if (!string.IsNullOrEmpty(valueStr))
                        {
                            double Value = double.Parse(valueStr);
                            if (element.unitValue == 1)
                                Value = Value * 10000;
                            else if (element.unitValue == 3)
                                Value = Value * 1000;
                            if (report.isShowND && Value <= WorkCurveHelper.NDValue)
                            {
                                if (element.unitValue == 1)
                                {
                                    rowNew[column.HeaderText] = "ND" + "(%)";
                                }
                                else if (element.unitValue == 2)
                                {
                                    rowNew[column.HeaderText] = "ND" + "(ppm)";
                                }
                                else
                                {
                                    rowNew[column.HeaderText] = "ND" + "(‰)";
                                }
                            }
                            else
                            {
                                if (element.unitValue == 1)
                                {
                                    rowNew[column.HeaderText] = (Value / 10000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(%)";
                                }
                                else if (element.unitValue == 2)
                                {
                                    rowNew[column.HeaderText] = Value.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(ppm)";
                                }
                                else
                                {
                                    rowNew[column.HeaderText] = (Value / 1000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(‰)";

                                }
                            }
                            totalContent += Value;
                        }
                    }
                }
                if (standard != null && standard.StandardDatas != null && standard.StandardDatas.Count > 0 && standard.IsSelectTotal)
                {
                    string strPass = ExcelTemplateParams.PassResults;
                    if (totalContent > standard.TotalContentStandard)
                        strPass = ExcelTemplateParams.FalseResults;
                    rowNew[Info.TotalPassReslt] = strPass;
                }
                dt.Rows.Add(rowNew);
            }
            #endregion

            return dt;
        }
        #endregion

        private void PrintBlue(object obj)
        {
            DifferenceDevice.interClassMain.PrinterBlue(selectLong);
        }
        /// <summary>
        /// 给Report赋值基本参数
        /// </summary>
        /// <param name="report">Report打印模版</param>
        /// <param name="elementList">ElementList参数</param>
        /// <param name="item">打印模式</param>
        private bool AddReportParameters(Report report, ElementList elementList, ref List<HistoryElement> elements, int item)
        {
            bool blFlag = true;
            List<string> SeleWorkCurveNameList = new List<string>();
            report.isShowND = WorkCurveHelper.isShowND && DifferenceDevice.IsRohs;
            if (selectLong.Count > WorkCurveHelper.PrintExcelCount)
            {
                Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
                blFlag = false;
                return blFlag;
            }

            ExcelTemplateParams.GetExcelTemplateParams();
            string workCurveName = string.Empty;
            SpecEntity spec = new SpecEntity();
            SpecListEntity tempList = new SpecListEntity();

            #region
            string historyListStr = Helper.ToStrsRemoveLast(selectLong.ToArray());
            List<HistoryRecord> records = HistoryRecord.FindBySql("select * from HistoryRecord where Id in (" + historyListStr + ")");
            var workCurveIds = from temp in records select temp.WorkCurveId;
            List<WorkCurve> workCurves = WorkCurve.FindBySql("select * from WorkCurve where id in (" + Helper.ToStrsRemoveLast(workCurveIds.ToArray()) + ") ");
            if (workCurves != null && workCurves.Count > 0)
            {
                workCurveName = workCurves[0].Name;
                elementList.MainElementToCalcKarat = workCurves[0].ElementList != null ? workCurves[0].ElementList.MainElementToCalcKarat : "Au";
                elementList.RhIsLayer = workCurves[0].ElementList != null ? workCurves[0].ElementList.RhIsLayer : false;
                elementList.LayerElemsInAnalyzer = workCurves[0].ElementList != null ? workCurves[0].ElementList.LayerElemsInAnalyzer : "";
            }
            SeleWorkCurveNameList = (from tt in workCurves select tt.Name).ToList();
            if (records != null && records.Count > 0)//如下数据取第一笔为准
            {
                tempList = DataBaseHelper.QueryByEdition(records[0].SpecListName, records[0].FilePath, records[0].EditionType);
                if (tempList != null && tempList.Specs.Length > 0)
                {
                    if (!string.IsNullOrEmpty(tempList.Specs[0].SpecData))
                        spec = tempList.Specs[0];
                    report.dWeight = records[0].Weight;
                    report.ReadingNo = records[0].HistoryRecordCode;
                    report.historyRecordid = records[0].Id.ToString();
                    // report.Specification = records[0].Specifications;
                    report.Specification = records.Count > 0 ? (System.Text.RegularExpressions.Regex.Replace((records[0].Specifications != null ? records[0].Specifications : ""), "%.?%|%.+%", "%")) : "";
                }
                else
                {
                    Msg.Show(Info.DataDelete);
                    blFlag = false;
                    return blFlag;
                }
            }
            elements = HistoryElement.FindBySql("select * from HistoryElement where HistoryRecord_Id in (" + historyListStr + ")");

            this.AddElementList(report, elements, elementList, Helper.ToStrsRemoveLast(workCurveIds.ToArray()), item);
            #endregion

            if (comboBoxCurveName.Text == "--All--" && SeleWorkCurveNameList.Count > 1)
            {
                Msg.Show(Info.strSeleWorkCurveName);
                blFlag = false;
                return blFlag;
            }
            foreach (ToolStripMenuItem tempItem in this.contextMenuStrip1.Items)
            {
                if (!tempItem.Checked && Atoms.AtomList.ToList().Find(w => w.AtomName == tempItem.Name) != null)
                {
                    CurveElement tempFind = elementList.Items.ToList().Find(w => w.Caption == tempItem.Name);
                    if (tempFind != null)
                        elementList.Items.Remove(tempFind);
                }
                else if ((tempItem.Checked && Atoms.AtomList.ToList().Find(w => w.AtomName == tempItem.Name) != null) && (item != 1))
                {
                    CurveElement tempFind = elementList.Items.ToList().Find(w => w.Caption == tempItem.Name);
                    if (tempFind == null)
                    {
                        tempFind = CurveElement.New;
                        tempFind.Caption = tempItem.Name;
                        tempFind.RowsIndex = contextMenuStrip1.Items.IndexOf(tempItem);
                        elementList.Items.Add(tempFind);
                    }
                }
            }
            if (elementList.Items.Count == 0)
            {
                Msg.Show(Info.NoLoadSource, Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                blFlag = false;
                return blFlag;
            }

            #region 给Report赋值
            report.Spec = spec;
            report.specList = tempList;
            report.operateMember = FrmLogon.userName;
            report.WorkCurveName = workCurveName;
            if (item == 1)
            {
                report.Elements = FilterElementsByDGV(selectLong.FirstOrDefault(), DifferenceDevice.IsRohs);
            }
            else
            {
                report.Elements = elementList;
                var eles = elementList.Items.ToList().OrderBy(w => w.RowsIndex);
                elementList.Items.Clear();
                eles.All(w => { elementList.Items.Add(w); return true; });
                //report.WorkCurveID = WorkCurveID;
                //report.historyRecordid = historyRecordid;
                report.selectLong = selectLong;
            }

            report.FirstContIntr.Add(elementList.Items.Count);
            #endregion

            return blFlag;

        }

        /// <summary>
        /// 给ElementList赋值
        /// </summary>
        /// <param name="report">Report打印模版</param>
        /// <param name="elements">List<HistoryElement> 参数</param>
        /// <param name="elementList">ElementList参数</param>
        /// <param name="workCurveIds">WorkCurve集合</param>
        /// <param name="item">打印模式</param>
        private void AddElementList(Report report, List<HistoryElement> elements, ElementList elementList, string workCurveIds, int item)
        {
            if (item == 1 || item == 12)
            {
                #region
                foreach (var element in elements)
                {
                    var temp = CurveElement.FindBySql("select * from CurveElement a,ElementList b,WorkCurve c on a.ElementList_Id=b.Id and b.WorkCurve_Id=c.Id where Caption='" + element.elementName + "' and c.Id in (" + workCurveIds + ")");
                    var result = temp.FindAll(w => w.IsShowElement).ToList();
                    if (result != null && result.Count > 0 && elementList.Items.ToList().FindIndex(w => w.Formula == element.elementName) < 0)
                        elementList.Items.Add(result[0]);
                    report.historyStandID = element.customstandard_Id;
                }
                #endregion
                // elementList.CustomFields.Add(CustomField
            }
            else if (item == 9)
            {
                #region
                foreach (var element in elements)
                {
                    var temp = CurveElement.FindBySql("select * from CurveElement a,ElementList b,WorkCurve c on a.ElementList_Id=b.Id and b.WorkCurve_Id=c.Id where Caption='" + element.elementName + "' and c.Id in (" + workCurveIds + ")");
                    var result = temp.FindAll(w => w.IsShowElement).ToList();

                    double content = 0.0;
                    double.TryParse(element.contextelementValue, out content);
                    if (result != null && result.Count > 0)
                    {
                        if (elementList.Items.ToList().FindIndex(w => w.Formula == element.elementName) >= 0) continue;
                        result[0].Intensity = element.CaculateIntensity;
                        result[0].Error = element.Error;
                        if (element.unitValue == 2)
                            result[0].Error = result[0].Error / 10000;
                        else
                            result[0].Error = result[0].Error / 10;

                        if (element.unitValue == 1)
                            result[0].Content = content;
                        else if (element.unitValue == 2)
                            result[0].Content = content / 10000;
                        else
                            result[0].Content = content / 10;
                        double.TryParse(element.thickelementValue, out content);
                        result[0].Thickness = content;
                        elementList.Items.Add(result[0]);
                        report.historyStandID = element.customstandard_Id;
                    }
                    else
                    {
                        CurveElement tempe = CurveElement.New;
                        tempe.Caption = element.elementName;
                        tempe.Intensity = element.CaculateIntensity;
                        tempe.Error = element.Error;
                        tempe.Content = content;
                        if (element.unitValue == 2)
                        {
                            tempe.Content /= 10000;
                            tempe.Error /= 10000;
                        }
                        else if (element.unitValue == 3)
                        {
                            tempe.Error /= 10;
                            tempe.Content /= 10;
                        }
                        tempe.ContentUnit = (ContentUnit)element.unitValue;
                        tempe.IsShowElement = true;
                        tempe.IsDisplay = true;  //pfby20161019  //贵金属模板要显示氧化物
                        elementList.Items.Add(tempe);
                        report.historyStandID = element.customstandard_Id;
                    }

                }
                #endregion
            }
            else if (item == 11)
            {
                #region
                foreach (var element in elements)
                {
                    var temp = CurveElement.FindBySql("select * from CurveElement a,ElementList b,WorkCurve c on a.ElementList_Id=b.Id and b.WorkCurve_Id=c.Id where Caption='" + element.elementName + "' and c.Id in (" + workCurveIds + ")");
                    var result = temp.FindAll(w => w.IsShowElement).ToList();
                    double content = 0.0;
                    double.TryParse(element.contextelementValue, out content);

                    if (result != null && result.Count > 0)
                    {
                        if (elementList.Items.ToList().FindIndex(w => w.Formula == element.elementName) >= 0) continue;
                        result[0].Intensity = element.CaculateIntensity;
                        result[0].Error = element.Error;
                        result[0].Content = content;
                        elementList.Items.Add(result[0]);
                        report.historyStandID = element.customstandard_Id;
                    }
                }
                #endregion
            }
            else
            {
                #region
                foreach (var element in elements)
                {
                    //CurveElement temp = CurveElement.Find(w => w.Formula == element.elementName).Find(w => w.ElementList.WorkCurve.Id == workCurve.Id);
                    var temp = CurveElement.FindBySql("select * from CurveElement a,ElementList b,WorkCurve c on a.ElementList_Id=b.Id and b.WorkCurve_Id=c.Id where Formula='" + element.elementName + "' and c.Id in (" + workCurveIds + ")");
                    var result = temp.FindAll(w => w.IsShowElement).ToList();

                    double content = 0.0;
                    double.TryParse(element.contextelementValue, out content);
                    if (result != null && result.Count > 0)
                    {
                        if (elementList.Items.ToList().FindIndex(w => w.Formula == element.elementName) >= 0) continue;
                        result[0].Intensity = element.CaculateIntensity;
                        result[0].Error = element.Error;
                        if (element.unitValue == 2)
                            result[0].Error = result[0].Error / 10000;
                        else
                            result[0].Error = result[0].Error / 10;

                        if (element.unitValue == 1)
                            result[0].Content = content;
                        else if (element.unitValue == 2)
                            result[0].Content = content / 10000;
                        else
                            result[0].Content = content / 10;
                        elementList.Items.Add(result[0]);
                        report.historyStandID = element.customstandard_Id;
                    }
                    else
                    {
                        CurveElement tempe = CurveElement.New;
                        tempe.Caption = element.elementName;
                        tempe.Intensity = element.CaculateIntensity;
                        tempe.Error = element.Error;
                        tempe.Content = content;
                        if (element.unitValue == 2)
                        {
                            tempe.Content /= 10000;
                            tempe.Error /= 10000;
                        }
                        else if (element.unitValue == 3)
                        {
                            tempe.Error /= 10;
                            tempe.Content /= 10;
                        }
                        tempe.ContentUnit = (ContentUnit)element.unitValue;
                        tempe.IsShowElement = true;
                        //elementList.Items.Add(tempe);
                        elementList.Items.Insert(0, tempe);
                        report.historyStandID = element.customstandard_Id;
                    }
                }
                #endregion
            }
        }


        // private List<DataRow> AddStaticsRows(DataTable dt, string HeadColumnName)
        private void AddStaticsRows(DataTable dt, string HeadColumnName)
        {
            if (!DifferenceDevice.IsThick)
            {
                DifferenceDevice.interClassMain.AddStaticsRows(dt, HeadColumnName, null);
            }
            else
                DifferenceDevice.interClassMain.AddThickStaticsRows(dt, HeadColumnName);                   //yuzhaomodify:Thick专用模板
        }

        private List<DataRow> AddStaticsRows(DataTable dt, string HeadColumnName, int count)
        {
            return DifferenceDevice.interClassMain.AddStaticsRows(dt, HeadColumnName, count);
        }

        /// <summary>
        /// 获取自定义报表名称
        /// </summary>
        /// <returns></returns>
        private string GetDefineReportName()
        {
            HistoryRecord hr = null;
            for (int i = 0; i < selectLong.Count; i++)
            {
                hr = HistoryRecord.FindById(selectLong[i]);
                if (hr != null)
                    break;
            }
            WorkCurve wc = WorkCurve.FindById(hr.WorkCurveId);
            SpecListEntity sl = DataBaseHelper.QueryByEdition(hr.SpecListName, hr.FilePath, hr.EditionType);
            string reportName = DifferenceDevice.interClassMain.GetDefineReportName(sl, wc, selectLong.FirstOrDefault());
            return reportName;
        }
        //
        /// <summary>
        /// 创建连续测量结果数据库 根据感兴趣元素
        /// </summary>
        private DataTable CreateReTestTable(ElementList elementList)
        {
            #region //无排序功能
            //DataTable reTestTable = new DataTable();
            //reTestTable.Columns.Clear();
            //reTestTable.Columns.Add("Time", typeof(string));
            //for (int i = 0; i < selectLong.Count;i++ )
            //{
            //    HistoryRecord record = HistoryRecord.FindById(selectLong[i]);
            //    if (record != null)
            //    {
            //        var elements = HistoryElement.Find(w => w.HistoryRecord.Id == record.Id);
            //        //elements = elements.Union(elementList.Items);
            //        var sort = from l in elementList.Items
            //                   join e in elements
            //                   on l.Caption equals e.elementName
            //                   orderby l.RowsIndex
            //                   select l.Caption;
            //        foreach (var element in sort.Distinct())
            //        {
            //            if (!reTestTable.Columns.Contains(element))
            //                reTestTable.Columns.Add(element, typeof(string));
            //        }
            //    }
            //}
            //return reTestTable;
            #endregion

            #region //与菜单中共用
            //List<string> lstHide = new List<string>();//需要隐藏的元素
            //foreach (ToolStripMenuItem item in this.contextMenuStrip1.Items)
            //{
            //    if (!item.Checked && Atoms.AtomList.Exists(w => string.Compare(w.AtomName, item.Text, true) == 0))
            //        lstHide.Add(item.Text);
            //}
            //return DifferenceDevice.interClassMain.CreateReTestTable(selectLong,lstHide.ToArray());
            #endregion

            DataTable reTestTable = new DataTable();
            reTestTable.Columns.Clear();
            reTestTable.Columns.Add("Time", typeof(string));
            foreach (CurveElement curEle in elementList.Items.ToList().FindAll(w => w.IsShowElement && w.IsDisplay))  //yuzhaomodify:不显示元素的时候处理
            {
                //if (ReportTemplateHelper.ExcelModeType == 21)
                //{
                //    Atom atom = Atoms.AtomList.Find(s => s.AtomName == curEle.Caption);
                //    string atomNameCN = (atom == null) ? "" : atom.AtomNameCN;
                //    showElem = Lang.Model.CurrentLang.IsDefaultLang ? atomNameCN + "(" + curEle.Caption + ")" : curEle.Caption;
                //}
                //else
                //    showElem = curEle.Caption;
                var header = curEle.IsOxide ? curEle.Formula : curEle.Caption;
                if (!reTestTable.Columns.Contains(header))
                    reTestTable.Columns.Add(header, typeof(string));
            }
            if (countryId == 0)
            {
                reTestTable.Columns.Add(Info.TestResult, typeof(string));
            }
            return reTestTable;
        }

        #region 翻页管理

        /// <summary>
        /// 每页显示的数据的行数
        /// </summary>
        public int ROWS_PER_PAGE = 0;
        private void buttonPrePage_Click(object sender, EventArgs e)
        {
            int totalPage = int.Parse(this.labelTotalPage.Text.Trim());
            int currentPage = int.Parse(labelCurrentPage.Text.Trim());
            //if (cbHeader != null)
            //    cbHeader.Checked = false;
            if (currentPage < 0) return;
            else if (currentPage == 1) currentPage = 0;
            else currentPage = currentPage - 2;

            GetSepecifiedSearchCondition(totalPage, currentPage);

        }

        private void buttonGoto_Click(object sender, EventArgs e)
        {
            int totalPage = int.Parse(this.labelTotalPage.Text.Trim());
            int currentPage = int.Parse(labelCurrentPage.Text.Trim());
            if (string.IsNullOrEmpty(textBoxPageNum.Text) || !textBoxPageNum.Text.IsInt())
                return;
            //if (cbHeader != null)
            //    cbHeader.Checked = false;
            try
            {
                currentPage = int.Parse(textBoxPageNum.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            if (currentPage > totalPage || currentPage < 1)
            {
                Msg.Show(Info.strGotoInfo);
                return;
            }

            if (currentPage == totalPage) currentPage = totalPage - 1;
            else if (currentPage == 1) currentPage = 0;
            else currentPage = currentPage - 1;


            GetSepecifiedSearchCondition(totalPage, currentPage);
        }

        private List<long> selectLong = new List<long>();

        private void buttonNexPage_Click(object sender, EventArgs e)
        {
            //if (cbHeader != null)
            //    cbHeader.Checked = false;
            int totalPage = int.Parse(this.labelTotalPage.Text.Trim());
            int currentPage = int.Parse(labelCurrentPage.Text.Trim());
            if (currentPage > totalPage)
                return;
            else if (currentPage == totalPage) currentPage = totalPage - 1;

            GetSepecifiedSearchCondition(totalPage, currentPage);
        }


        private void buttonHomePage_Click(object sender, EventArgs e)
        {
            int totalPage = int.Parse(this.labelTotalPage.Text.Trim());
            int currentPage = int.Parse(labelCurrentPage.Text.Trim());
            //if (cbHeader != null)
            //    cbHeader.Checked = false;
            currentPage = 0;

            GetSepecifiedSearchCondition(totalPage, currentPage);
        }

        private void buttonEndPage_Click(object sender, EventArgs e)
        {
            int totalPage = int.Parse(this.labelTotalPage.Text.Trim());
            int currentPage = int.Parse(labelCurrentPage.Text.Trim());
            //if (cbHeader != null)
            //    cbHeader.Checked = false;
            currentPage = totalPage - 1;

            GetSepecifiedSearchCondition(totalPage, currentPage);
        }

        //private DatagridViewCheckBoxHeaderCell cbHeader;

        /// <summary>
        /// 设置测量结果 20111031设置为pass，fail，tbd
        /// </summary>
        /// <param name="dt"></param>
        private void SetTestResult(ref DataTable dt)
        {
            if (DifferenceDevice.IsRohs)
                foreach (DataRow row in dt.Rows)
                {
                    string strCustomStandName = string.Empty;
                    row["TestResult"] = ExcelTemplateParams.TestRetult(row["HistoryRecord_Id"].ToString(), false, out strCustomStandName);// DifferenceDevice.interClassMain.TestRetult(row["HistoryRecord_Id"].ToString()); //InterfaceClass.TestRetult(row["HistoryRecord_Id"].ToString());
                    row["CustomStandard"] = strCustomStandName;
                }
            if (DifferenceDevice.IsThick)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string strCustomStandName = string.Empty;
                    row["TestResult"] = ExcelTemplateParams.TestRetultForThick(row["HistoryRecord_Id"].ToString(), out strCustomStandName);// DifferenceDevice.interClassMain.TestRetult(row["HistoryRecord_Id"].ToString()); //InterfaceClass.TestRetult(row["HistoryRecord_Id"].ToString());
                    row["CustomStandard"] = strCustomStandName;
                }
            }
            if (WorkCurveHelper.isShowXRFStandard == 1)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string strCustomStandName = string.Empty;
                    row["TestResult"] = ExcelTemplateParams.TestRetultForXRF(row["HistoryRecord_Id"].ToString(), out strCustomStandName);// DifferenceDevice.interClassMain.TestRetult(row["HistoryRecord_Id"].ToString()); //InterfaceClass.TestRetult(row["HistoryRecord_Id"].ToString());
                    row["CustomStandard"] = strCustomStandName;
                }
            }

            if (countryId == 0)
                foreach (DataRow row in dt.Rows)
                    row["TestResult"] = ExcelTemplateParams.TestRetultForEdx(row["HistoryRecord_Id"].ToString());



        }

        /// <summary>
        /// 查询语句组合
        /// </summary>
        /// <param name="sType">0：分页查询，1：选择导出，2：全部导出</param>
        /// <param name="totalPage">分页查询总页数</param>
        /// <param name="currentPage">分页查询当前页</param>
        /// <param name="strSelectID">选择的历史记录</param>
        /// <returns></returns>
        private string GetSQL(int sType, int totalPage, int currentPage, string strSelectID)
        {
            string workcurve_id = string.Empty;
            if (comboBoxCurveName.Text == "--All--" || comboBoxCurveName.Text == "")
            {
                workcurve_id = "-1";
            }
            else
            {
                if (((MyItem)comboBoxCurveName.SelectedItem).Type == ItemType.Curve)
                {
                    workcurve_id = "(" + ((MyItem)comboBoxCurveName.SelectedItem).Value.ToString() + ")";
                }
                else
                {
                    string sql = "select * from WorkCurve a inner join Condition b on a.Condition_Id = b.Id inner join Device c on b.Device_Id=c.Id where c.Id=" + WorkCurveHelper.DeviceCurrent.Id;
                    var lstCur = WorkCurve.FindBySql(sql + " and  b.Device_Id=" + WorkCurveHelper.DeviceCurrent.Id + " and b.Type=0 and a.WorkRegion_Id =" + ((MyItem)comboBoxCurveName.SelectedItem).Value.ToString());
                    var lstarr = from curve in lstCur select curve.Id.ToString();
                    string[] idArray = lstarr.ToArray();

                    workcurve_id = "(" + string.Join(",", idArray) + ")";
                }
            }


            GetSetColmun();
            string strSampleNameAndTestDate = (this.checkBoxWSampleName.Checked) ? " and SampleName like '%" + this.textBoxWInputName.Text.ToString() + "%'" : "";
            strSampleNameAndTestDate += (this.checkBoxWDate.Checked) ? " and datetime(specdate)>='" + this.dateTimePickerStart.Value.ToString("yyyy-MM-dd HH:mm:ss") + "' and datetime(specdate)<'" + this.dateTimePickerEnd.Value.ToString("yyyy-MM-dd HH:mm:ss") + "'" : "";

            string strSql = " select elementname,c.layernumber,e.atomnameen from  ";
            strSql += "(select (case when IsOxide=1 then Formula else caption end) elementname from curveelement where elementlist_id in ( " +
                        " select id from elementlist where 1=1 " + ((workcurve_id == "-1") ? " and workcurve_id in (select id from workcurve where condition_id in ( " +
                        " select id from condition where device_id=" + WorkCurveHelper.DeviceCurrent.Id + "))" : " and workcurve_id in " + workcurve_id) + " ) and (IsShowElement=1" + (DifferenceDevice.IsThick ? " OR IsShowContent=1)" : ")") +
                        " union select Name nameQ from CustomField where elementlist_id in ( select id from elementlist where 1=1 " + ((workcurve_id == "-1") ? " and workcurve_id in (select id from workcurve where condition_id in ( " +  //追加自定义项
                        " select id from condition where device_id=" + WorkCurveHelper.DeviceCurrent.Id + "))" : " and workcurve_id in " + workcurve_id) + "))";
            if (comboBoxCurveName.Text == Info.Intelligent)//选择智能模式的时候
            {
                strSql += " left outer join Atom e on e.atomname=elementName " +

                       //当为FPThick时，获取各元素所在的层
                       " left outer join  (   select * from curveelement where elementlist_id in ( " +
                    // " select id from elementlist where 1=1 " + ((comboBoxCurveName.Text == "--All--") ? " and workcurve_id in (select id from workcurve where condition_id in ( " +
                      " select id from elementlist where 1=1 " + ((workcurve_id == "-1") ? " and workcurve_id in (select id from workcurve where condition_id in ( " +
      " select id from condition where device_id=" + WorkCurveHelper.DeviceCurrent.Id + "))" : " and workcurve_id in " + workcurve_id) + " ) ) c on " + ((DifferenceDevice.IsXRF) ? "c.caption=case  when d.AtomName isnull then elementName else d.AtomName end " : "c.caption=elementName") +//氧化物排序问题

                        "  group by elementname order by  case when c.rowsindex is null then 99+e.AtomID else c.rowsindex end ";


            }
            else
            {
                //氧化物排序问题
                if (DifferenceDevice.IsXRF)
                    strSql += " left outer join oxide on oxide.oxidename=elementname left outer join Atom d on d.atomid=oxide.Atom_Id ";
                strSql += " left outer join Atom e on e.atomname=elementName " +

                           //当为FPThick时，获取各元素所在的层
                           " left outer join  (   select * from curveelement where elementlist_id in ( " +
                    // " select id from elementlist where 1=1 " + ((comboBoxCurveName.Text == "--All--") ? " and workcurve_id in (select id from workcurve where condition_id in ( " +
                          " select id from elementlist where 1=1 " + ((workcurve_id == "-1") ? " and workcurve_id in (select id from workcurve where condition_id in ( " +
          " select id from condition where device_id=" + WorkCurveHelper.DeviceCurrent.Id + "))" : " and workcurve_id in " + workcurve_id) + " ) ) c on " + ((DifferenceDevice.IsXRF) ? "c.caption=case  when d.AtomName isnull then elementName else d.AtomName end " : "c.caption=elementName") +//氧化物排序问题

                            "  group by elementname order by  case when c.rowsindex is null then 99+e.AtomID else c.rowsindex end ";
            }

            DataTable dt_historyelement = GetData(strSql);//获取所有历史记录元素 及感兴趣元素
            string strPassReslt = "";
            if ((DifferenceDevice.IsRohs || (WorkCurveHelper.isShowXRFStandard == 1)) && ReportTemplateHelper.IsOurExcel == 1 && sType != 0 && strSelectID != "")
            {

                strPassReslt += " ,case ";
                foreach (DataRow row in dt_historyelement.Rows)
                    strPassReslt += "when b." + row["elementname"].ToString() + "_Color<>'' then '" + ExcelTemplateParams.FalseResults + "'";
                strPassReslt += " else '" + ExcelTemplateParams.PassResults + "' end as '" + Info.strPassReslt + "'";

            }


            string strAddColmun = "";
            if (dAddColmun != null && dAddColmun.Count > 0)
                foreach (string strcol in dAddColmun.Keys)
                    strAddColmun += ",a." + strcol;

            //获取公司其它信息列表
            List<CompanyOthersInfo> CompanyOthersInfoList = null;
            CompanyOthersInfoList = CompanyOthersInfo.FindBySql("select * from companyothersinfo where display=1 and ExcelModeType='" + ReportTemplateHelper.ExcelModeType.ToString() + "'");

            string strComOtherInfo = "";
            if (CompanyOthersInfoList.Count > 0)
            {
                GetCombInfo();
                foreach (CompanyOthersInfo comotherinfo in CompanyOthersInfoList)
                {
                    string sComOtherInfo = comotherinfo.Name;
                    if (comotherinfo.IsReport && comotherinfo.ExcelModeTarget != "")
                        GetCombReportInfoByTarget(comotherinfo.ExcelModeTarget, ref sComOtherInfo);
                    strComOtherInfo += "(select listinfo from historycompanyotherinfo where history_id=a.id and WorkCurveId=a.WorkCurveId and companyothersinfo_id='" + comotherinfo.Id + "') as '" + sComOtherInfo + "',";
                }
            }

            //List<string> layernumberList = new List<string>();//当为FPThick时，存储已经选择的层
            strSql = "select " + strComOtherInfo + "a.HistoryRecordCode,";
            if (DifferenceDevice.IsRohs || DifferenceDevice.IsThick || countryId == 0 || WorkCurveHelper.isShowXRFStandard == 1) strSql += " '' as TestResult,'' as CustomStandard, ";
            if (DifferenceDevice.IsRohs && ReportTemplateHelper.IsShowUnitColumns) strSql += " 'ppm' as Unit, ";


            // strSql += " (select Name from workcurve where id=a.workcurveid) as WorkCurveName, " +
            if (comboBoxCurveName.Text == Info.Intelligent)
                strSql += "'" + Info.Intelligent + "' as WorkCurveName, ";
            else
                strSql += " (select Name from workcurve where id=a.workcurveid) as WorkCurveName, ";//,a.AreaDensity
            if (ReportTemplateHelper.IsShowResole)
            {                                                                                                                                                              //a.SpecDate
                strSql += " a.Id,a.SampleName,a.SpecListName,a.Height,a.CalcAngleHeight,a.Supplier,a.Operater,strftime('%Y-%m-%d %H:%M:%S',datetime(a.SpecDate)) as SpecDate ,a.SpecSummary,a.WorkCurveId,a.DeviceName, a.CaculateTime,cast(a.ActualVoltage as int) as ActualVoltage,cast(a.ActualCurrent as int) as ActualCurrent,cast(a.CountRate as int) as CountRate,a.PeakChannel,cast(a.Resole as int) as Resole,a.TotalCount,a.Specifications,b.*" + strPassReslt + strAddColmun + " from historyrecord a left join (select HistoryRecord_Id ";
            }
            else
            {
                strSql += " a.Id,a.SampleName,a.SpecListName,a.Height,a.CalcAngleHeight,a.Supplier,a.Operater,strftime('%Y-%m-%d %H:%M:%S',datetime(a.SpecDate)) as SpecDate,a.SpecSummary,a.WorkCurveId,a.DeviceName, a.CaculateTime,cast(a.ActualVoltage as int) as ActualVoltage,cast(a.ActualCurrent as int) as ActualCurrent,cast(a.CountRate as int) as CountRate,a.PeakChannel,a.TotalCount,a.Specifications,b.*" + strPassReslt + strAddColmun + " from historyrecord a left join (select HistoryRecord_Id ";
            }
            if (DifferenceDevice.IsThick)
            {
                //获取当前工作曲线
                WorkCurve seleWorkCurve = null;
                if (DifferenceDevice.IsThick)
                {
                    //WorkCurveHelper.DeviceCurrent.
                    List<WorkCurve> WorkCurveList = WorkCurve.FindBySql("select * from  workcurve where  name='" + comboBoxCurveName.Text + "' and condition_id in (select id from condition where device_id=" + WorkCurveHelper.DeviceCurrent.Id + ")");
                    if (WorkCurveList != null && WorkCurveList.Count > 0) seleWorkCurve = WorkCurveList[0];
                }
                if (WorkCurveHelper.CalcType == CalcType.PeakDivBase)
                {
                    if (seleWorkCurve.ElementList != null)
                        foreach (var a in seleWorkCurve.ElementList.Items)
                        {
                            a.IsShowContent = false;
                        }
                }
                //Thick中，同层元素合并为一列，含量一定要显示在厚度前
                IEnumerable<IGrouping<string, DataRow>> result = dt_historyelement.Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["layernumber"].ToString());//按layernumber分组
                foreach (IGrouping<string, DataRow> ig in result)
                {

                    if (ig.Key != "")
                    {

                        string elementName = "";
                        string strelementname = "";

                        //显示每个元素的含量
                        foreach (var dr in ig)
                        {
                            elementName = dr["elementname"].ToString();

                            if (seleWorkCurve != null && seleWorkCurve.ElementList.Items.ToList().Find(w => w.Caption == elementName && w.IsShowElement) != null)
                                strelementname += dr["elementname"].ToString() + "|";
                            //如果编辑元素中，勾选显示含量，则显示
                            if (seleWorkCurve != null && seleWorkCurve.ElementList.Items.ToList().Find(w => w.Caption == elementName && w.IsShowContent) != null)//&& w.IsShowElement
                            {
                                if (!seleWorkCurve.IsThickShowAreaThick)
                                {
                                    strSql += " ,max(case when [elementname] =" + "'" + dr["elementname"].ToString() + "'" + " then round(cast ([contextelementvalue] as float)," + WorkCurveHelper.SoftWareContentBit.ToString() + ")  else  round(cast ('0' as float)," + WorkCurveHelper.SoftWareContentBit.ToString() + ") end) " +
                                              " ||max(case when [elementname] =" + "'" + dr["elementname"].ToString() + "'" + " then (case when [unitValue]=1 then " + Convert.ToString((checkShowUnit.Checked) ? "' (%)'" : "''") + " else " + Convert.ToString((checkShowUnit.Checked) ? "' (ppm)'" : "''") + " end ) else 0 end)" +
                                             " as " + "'" + dr["elementname"].ToString() + Info.EditContent + "'";
                                }
                                else
                                {
                                    string densityUnit = (WorkCurveHelper.WorkCurveCurrent.AreaThickType == null || WorkCurveHelper.WorkCurveCurrent.AreaThickType == string.Empty) ? Info.strAreaDensityUnit : WorkCurveHelper.WorkCurveCurrent.AreaThickType;

                                    //strSql += " ,max(case when [elementname] =" + "'" + dr["elementname"].ToString() + "'" + " then round(cast ([contextelementvalue] as float)," + WorkCurveHelper.SoftWareContentBit.ToString() + ")  else  round(cast ('0' as float)," + WorkCurveHelper.SoftWareContentBit.ToString() + ") end) " +
                                    //       " ||max(case when [elementname] =" + "'" + dr["elementname"].ToString() + "'" + " then "+ Convert.ToString((checkShowUnit.Checked) ? "' (" + densityUnit + ")'" : "''") +  " else 0 end)" +
                                    //      " as " + "'" + dr["elementname"].ToString() + Info.EditContent + "'";
                                    strSql += " ,max(case when [elementname] =" + "'" + dr["elementname"].ToString() + "'" + " then round(cast ([contextelementvalue] as float)," + WorkCurveHelper.SoftWareContentBit.ToString() + ")  else  round(cast ('0' as float)," + WorkCurveHelper.SoftWareContentBit.ToString() + ") end) " +
                                                     " ||max(case when [elementname] =" + "'" + dr["elementname"].ToString() + "'" + " then (case when [densityunitValue]<>'' then " + Convert.ToString((checkShowUnit.Checked) ? "'('||[densityunitValue]||')'" : "''") + " else " + Convert.ToString((checkShowUnit.Checked) ? " '(" + densityUnit + ")'" : "''") + " end ) else 0 end) " +
                                                    " as " + "'" + dr["elementname"].ToString() + Info.EditContent + "'";

                                }
                            }
                            if (ReportTemplateHelper.IsShowArea)
                                strSql += " ,max(case when [elementname] =" + "'" + dr["elementname"].ToString() + "'" + " then   round([ElementIntensity]," + WorkCurveHelper.SoftWareContentBit.ToString() + ")  else 0 end) as " + "'" + dr["elementname"].ToString() + Info.strArea + "'";
                            if (ReportTemplateHelper.IsShowError)
                                strSql += " ,max(case when [elementname] =" + "'" + dr["elementname"].ToString() + "'" + " then   round([Error]," + WorkCurveHelper.SoftWareContentBit.ToString() + ")  else 0 end) as " + "'" + dr["elementname"].ToString() + Info.strError + "'";
                            if (ReportTemplateHelper.IsShowIntensity)
                                strSql += " ,max(case when [elementname] =" + "'" + dr["elementname"].ToString() + "'" + " then   round([CaculateIntensity]," + WorkCurveHelper.SoftWareIntensityBit.ToString() + ")  else 0 end) as " + "'" + dr["elementname"].ToString() + Info.Intensity + "'";
                            if (ReportTemplateHelper.IsShowAverageColumns)
                                strSql += " ,max(case when [elementname] =" + "'" + dr["elementname"].ToString() + "'" + " then   round([AverageValue]," + WorkCurveHelper.SoftWareContentBit.ToString() + ")  else 0 end) as " + "'" + dr["elementname"].ToString() + Info.MeanValue + "'";
                            if (ReportTemplateHelper.IsShowRContent)
                                strSql += " ,max(case when [elementname] =" + "'" + dr["elementname"].ToString() + "'" + " then   round([contentRealelemValue]," + WorkCurveHelper.SoftWareContentBit.ToString() + ")  else 0 end) as " + "'" + dr["elementname"].ToString() + "_R" + "'";

                        }
                        if (strelementname == "" || strelementname == string.Empty) continue;
                        if (seleWorkCurve.IsThickShowAreaThick)
                        { //显示每一层的厚度
                            string densityUnit = (WorkCurveHelper.WorkCurveCurrent.AreaThickType == null || WorkCurveHelper.WorkCurveCurrent.AreaThickType == string.Empty) ? Info.strAreaDensityUnit : WorkCurveHelper.WorkCurveCurrent.AreaThickType;
                            strelementname = strelementname.Substring(0, strelementname.Length - 1);
                            strSql += " ,max(case when [elementname] =" + "'" + elementName + "'" + " then round(cast ([densityelementValue] as float)," + WorkCurveHelper.ThickBit.ToString() + ")  else  round(cast ('0' as float)," + WorkCurveHelper.ThickBit.ToString() + ") end)  " +
                                " ||max(case when [elementname] =" + "'" + elementName + "'" + " then (case when [densityunitValue]<>'' then " + Convert.ToString((checkShowUnit.Checked) ? "'('||[densityunitValue]||')'" : "''") + " else " + Convert.ToString((checkShowUnit.Checked) ? " '(" + densityUnit + ")'" : "''") + " end ) else 0 end) " +
                                         "as " + "'" + strelementname + Info.strAreaDensity + "'";
                        }
                        else
                        {
                            strelementname = strelementname.Substring(0, strelementname.Length - 1);
                            //strSql += " ,max(case when [elementname] =" + "'" + elementName + "'" + " then round(cast ([thickelementvalue] as float)," + WorkCurveHelper.ThickBit.ToString() + ")  else  round(cast ('0' as float)," + WorkCurveHelper.ThickBit.ToString() + ") end)  " +
                            //              " ||max(case when [elementname] =" + "'" + elementName + "'" + " then (case when [thickunitValue]=1 then " + Convert.ToString((checkShowUnit.Checked) ? "' (u〞)'" : "''") + " else " + Convert.ToString((checkShowUnit.Checked) ? "' (um)'" : "''") + " end ) else 0 end)" +
                            //             "as " + "'" + strelementname + Info.Thick + "'";


                            strSql += " ,max(case when [elementname] =" + "'" + elementName + "'" +
                                "then (case when round((case when [thickunitValue]=1 then round([thickelementvalue],4)*0.0254 else " +
                                "round([thickelementvalue],4) end),4)>" + WorkCurveHelper.ThicknessLimit.ToString() + " then '--' else round([thickelementvalue],4) end) else 0 end)" +
                                          " ||max(case when [elementname] =" + "'" + elementName + "'" +
                                          " then (case when [thickunitValue]=1 then " + Convert.ToString((checkShowUnit.Checked) ? "' (u〞)'" : "''") +
                                          " when [thickunitValue]=2 then " + Convert.ToString((checkShowUnit.Checked) ? "' (um)'" : "''") +
                                           " when [thickunitValue]=3 then " + Convert.ToString((checkShowUnit.Checked) ? "' (g/L)'" : "''") +
                                          " end ) else 0 end)" +
                                         "as " + "'" + strelementname + Info.Thick + "'";



                        }
                    }
                    else
                    {
                        foreach (var dr in ig)
                        {
                            string elementName = "";
                            string strelementname = "";
                            if (seleWorkCurve != null && seleWorkCurve.ElementList.Items.ToList().Find(w => w.Caption == elementName && w.IsShowElement) == null)
                            {
                                elementName += dr["elementname"].ToString();
                                //strSql += " ,max(case when [elementname] =" + "'" + elementName + "'" + " then round(cast ([thickelementvalue] as float)," + WorkCurveHelper.ThickBit.ToString() + ")  else  round(cast ('0' as float)," + WorkCurveHelper.ThickBit.ToString() + ") end)  " +
                                //strSql += " ,max(case when [elementname] =" + "'" + elementName + "'" + " then [thickelementvalue] end)  " +
                                //        "as " + "'" + elementName + "'";


                                strSql += " ,max(case when [elementname] =" + "'" + elementName + "'" +
                                 "then (case when round((case when [thickunitValue]=1 then round([thickelementvalue],4)*0.0254 else " +
                                 "round([thickelementvalue],4) end),4)>" + WorkCurveHelper.ThicknessLimit.ToString() + " then '--' else round([thickelementvalue],4) end) else 0 end)" +
                               " ||max(case when [elementname] =" + "'" + elementName + "'" +
                                    // " then (case when [thickunitValue]=1 then " + Convert.ToString((checkShowUnit.Checked) ? "' (u〞)'" : "''") + " else " + Convert.ToString((checkShowUnit.Checked) ? "' (um)'" : "''") +
                                " then (case when [thickunitValue]=1 then " + Convert.ToString((checkShowUnit.Checked) ? "' (u〞)'" : "''") +
                                          " when [thickunitValue]=2 then " + Convert.ToString((checkShowUnit.Checked) ? "' (um)'" : "''") +
                                           " when [thickunitValue]=3 then " + Convert.ToString((checkShowUnit.Checked) ? "' (g/L)'" : "''") +
                               " end ) else 0 end)" +
                              "as " + "'" + elementName + "'";

                                continue;
                            }
                        }
                    }

                }
            }
            else
            {
                foreach (DataRow row in dt_historyelement.Rows)
                {

                    string elementname = (ReportTemplateHelper.IsShowElementAllName && DifferenceDevice.IsAnalyser) ? row["atomnameen"].ToString() : row["elementname"].ToString();

                    //追加Rh是镀层
                    if (DifferenceDevice.IsAnalyser)
                    {
                        WorkCurve seleWorkCurve = null;
                        //List<WorkCurve> WorkCurveList = WorkCurve.FindBySql("select * from  workcurve where  Name='" + comboBoxCurveName.Text + "' and condition_id in (select id from condition where device_id=" + WorkCurveHelper.DeviceCurrent.Id + ")");
                        string nameSql = comboBoxCurveName.Text == "--All--" ? string.Empty : ("Name='" + comboBoxCurveName.Text + "' and");
                        List<WorkCurve> WorkCurveList = WorkCurve.FindBySql("select * from  workcurve where " + nameSql + " condition_id in (select id from condition where device_id=" + WorkCurveHelper.DeviceCurrent.Id + ")");
                        if (WorkCurveList != null && WorkCurveList.Count > 0) seleWorkCurve = WorkCurveList[0];
                        string[] strLayerElems = Helper.ToStrs((seleWorkCurve == null || seleWorkCurve.ElementList.LayerElemsInAnalyzer == null) ? "" : seleWorkCurve.ElementList.LayerElemsInAnalyzer);
                        if (seleWorkCurve != null
                            && seleWorkCurve.ElementList.RhIsLayer
                            // && seleWorkCurve.ElementList.Items.First(w => w.Caption.ToUpper().Equals("RH")) != null)
                            && strLayerElems.Contains(elementname))
                        {
                            //strSql += " ,max(case when [elementname] =\'Rh\'" +
                            //     " then   round(cast ([thickelementvalue] as float)," + WorkCurveHelper.ThickBit.ToString() + ")  else  round(cast ('0' as float)," + WorkCurveHelper.ThickBit.ToString() + ") end) " +
                            //     " ||max(case when [elementname] =\'Rh\' then " + Convert.ToString((checkShowUnit.Checked) ? "' (um)'" : "''") + " else 0 end)" +
                            //     " as " + "'" + "Rh" + "'";
                            strSql += " ,max(case when [elementname] =\'" + elementname + "\'" +
                                 " then   round(cast ([thickelementvalue] as float)," + WorkCurveHelper.ThickBit.ToString() + ")  else  round(cast ('0' as float)," + WorkCurveHelper.ThickBit.ToString() + ") end) " +
                                 " ||max(case when [elementname] =\'" + elementname + "\' then " + Convert.ToString((checkShowUnit.Checked) ? "' (um)'" : "''") + " else 0 end)" +
                                 " as " + "'" + elementname + "'";
                        }
                        else
                        {
                            strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" +
                                " then   round(cast ([contextelementvalue] as float)," + WorkCurveHelper.SoftWareContentBit.ToString() + ")  else  round(cast ('0' as float)," + WorkCurveHelper.SoftWareContentBit.ToString() + ") end) " +
                              " ||max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" +
                              " then (case when [unitValue]=1 then " + Convert.ToString((checkShowUnit.Checked) ? "' (%)'" : "''") +
                              " when [unitValue] = 2 then " + Convert.ToString((checkShowUnit.Checked) ? "' (ppm)'" : "''") +
                              " when [unitValue] = 3 then " + Convert.ToString((checkShowUnit.Checked) ? "' (‰)'" : "''") + " end ) else 0 end)" +
                              " as " + "'" + elementname + "'";
                        }
                    }
                    else if (DifferenceDevice.IsRohs)
                    {
                        if (WorkCurveHelper.isShowND)
                            strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" +
                                " then   (case when [unitValue]=1 then (case when (cast([contextelementvalue] as float)*10000.0)<" + WorkCurveHelper.NDValue.ToString() + "  then 'ND' else round(cast ([contextelementvalue] as float)," + WorkCurveHelper.SoftWareContentBit.ToString() + ") end)||" + Convert.ToString((checkShowUnit.Checked) ? "' (%)'" : "''") +
                                " when [unitValue]=2 then (case when cast([contextelementvalue] as float)<" + WorkCurveHelper.NDValue.ToString() + "  then 'ND' else round(cast ([contextelementvalue] as float)," + WorkCurveHelper.SoftWareContentBit.ToString() + ") end ) ||" + Convert.ToString((checkShowUnit.Checked) ? "' (ppm)'" : "''") +
                                " else (case when (cast([contextelementvalue] as float))*1000<" + WorkCurveHelper.NDValue.ToString() + "  then 'ND' else round(cast ([contextelementvalue] as float)," + WorkCurveHelper.SoftWareContentBit.ToString() + ") end ) ||" + Convert.ToString((checkShowUnit.Checked) ? "' (‰)'" : "''") +
                                "  END)  else '' end) as " + "'" + elementname + ((ReportTemplateHelper.IsOurExcel == 1 && sType != 0) ? Info.EditContent + "(" + Info.strStandard + row["StandardContent"].ToString() + "ppm)" : "") + "'";
                        else
                            strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" +
                                " then   (case when [unitValue]=1 then round(cast ([contextelementvalue] as float)," + WorkCurveHelper.SoftWareContentBit.ToString() + ")||" + Convert.ToString((checkShowUnit.Checked) ? "' (%)'" : "''") +
                                "when [unitValue]=3 then round(cast ([contextelementvalue] as float)," + WorkCurveHelper.SoftWareContentBit.ToString() + ")||" + Convert.ToString((checkShowUnit.Checked) ? "' (‰)'" : "''") +
                                " else round(cast ([contextelementvalue] as float)," + WorkCurveHelper.SoftWareContentBit.ToString() + ") ||" + Convert.ToString((checkShowUnit.Checked) ? "' (ppm)'" : "''") + "  END)  else '' end) as " + "'" + elementname + ((ReportTemplateHelper.IsOurExcel == 1 && sType != 0) ? Info.EditContent + "(" + Info.strStandard + row["StandardContent"].ToString() + "ppm)" : "") + "'";

                        strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" +
                            " then   (case when [unitValue]=1 then (case when ((cast([contextelementvalue] as float) " + (WorkCurveHelper.iResultJudgingType == 3 ? "+ cast([Error] as float) " : "") +
                            ")*10000.0)>cast([StandardContent] as float) then 'true' else '' end) " +
                            "when [unitValue]=3 then (case when ((cast([contextelementvalue] as float) " + (WorkCurveHelper.iResultJudgingType == 3 ? "+cast([Error] as float) " : "") +
                            ")*1000.0)>cast([StandardContent] as float) then 'true' else '' end) " +
                            "else     " +
                           " case when (cast([contextelementvalue] as float) " + (WorkCurveHelper.iResultJudgingType == 3 ? "+cast([Error] as float) " : "") +
                            ")>cast([StandardContent] as float) then 'true' else '' end" +
                           " end) else '' end) as " + "'" + elementname + "_Color" + "'";
                        //2016-05-11 追加含量和误差和的判断。
                        // " then   (case when [unitValue]=1 then (case when (cast([contextelementvalue] as float)*10000.0)>cast([StandardContent] as float) then 'true' else '' end) "+
                        // "when [unitValue]=3 then (case when (cast([contextelementvalue] as float)*1000.0)>cast([StandardContent] as float) then 'true' else '' end) " +
                        // "else     " +
                        //" case when cast([contextelementvalue] as float)>cast([StandardContent] as float) then 'true' else '' end" +
                        //" end) else '' end) as " + "'" + elementname + "_Color" + "'";

                    }
                    else
                    {
                        if (DifferenceDevice.IsXRF && WorkCurveHelper.isShowXRFStandard == 1)
                        {
                            strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" +
                               " then   (case when [unitValue]=1 then round(cast ([contextelementvalue] as float)," + WorkCurveHelper.SoftWareContentBit.ToString() + ")||" + Convert.ToString((checkShowUnit.Checked) ? "' (%)'" : "''") +
                               "when [unitValue]=3 then round(cast ([contextelementvalue] as float)," + WorkCurveHelper.SoftWareContentBit.ToString() + ")||" + Convert.ToString((checkShowUnit.Checked) ? "' (‰)'" : "''") +
                               " else round(cast ([contextelementvalue] as float)," + WorkCurveHelper.SoftWareContentBit.ToString() + ") ||" + Convert.ToString((checkShowUnit.Checked) ? "' (ppm)'" : "''") + "  END)  else '' end) as " + "'" + elementname + ((ReportTemplateHelper.IsOurExcel == 1 && sType != 0) ? Info.EditContent + "(" + Info.strStandard + row["StandardContent"].ToString() + "ppm)" : "") + "'";

                            strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" +
                                " then   (case when [unitValue]=1 then (case when (((cast([contextelementvalue] as float) " + (WorkCurveHelper.iResultJudgingType == 3 ? "+ cast([Error] as float) " : "") +
                                ")*10000.0)>cast([StandardContent] as float) or ((cast([contextelementvalue] as float) " + (WorkCurveHelper.iResultJudgingType == 3 ? "+ cast([Error] as float) " : "") +
                                ")*10000.0)<cast([StartStandardContent] as float)) then 'true' else '' end) " +
                                "when [unitValue]=3 then (case when (((cast([contextelementvalue] as float) " + (WorkCurveHelper.iResultJudgingType == 3 ? "+cast([Error] as float) " : "") +
                                ")*1000.0)>cast([StandardContent] as float) or ((cast([contextelementvalue] as float) " + (WorkCurveHelper.iResultJudgingType == 3 ? "+cast([Error] as float) " : "") +
                                ")*1000.0)<cast([StartStandardContent] as float)) then 'true' else '' end) " +
                                "else     " +
                               " case when ((cast([contextelementvalue] as float) " + (WorkCurveHelper.iResultJudgingType == 3 ? "+cast([Error] as float) " : "") +
                                ")>cast([StandardContent] as float) or  (cast([contextelementvalue] as float) " + (WorkCurveHelper.iResultJudgingType == 3 ? "+cast([Error] as float) " : "") +
                                ")<cast([StartStandardContent] as float))then 'true' else '' end" +
                               " end) else '' end) as " + "'" + elementname + "_Color" + "'";
                        }
                        else
                        {
                            strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" +
                                     " then   round(cast ([contextelementvalue] as float)," + WorkCurveHelper.SoftWareContentBit.ToString() + ")  else  round(cast ('0' as float)," + WorkCurveHelper.SoftWareContentBit.ToString() + ") end) " +
                                   " ||max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" +
                                   " then (case when [unitValue]=1 then " + Convert.ToString((checkShowUnit.Checked) ? "' (%)'" : "''") +
                                   " when [unitValue] = 2 then " + Convert.ToString((checkShowUnit.Checked) ? "' (ppm)'" : "''") +
                                   " when [unitValue] = 3 then " + Convert.ToString((checkShowUnit.Checked) ? "' (‰)'" : "''") + " end ) else 0 end)" +
                                   " as " + "'" + elementname + "'";
                        }
                    }
                    ////印度版需求
                    //if (DifferenceDevice.IsAnalyser && DifferenceDevice.IsShowWeight)
                    //{
                    //    strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + " then   round(cast ([contextelementvalue] as float )," + WorkCurveHelper.SoftWareContentBit.ToString() + ")  else 0.0 end) " +
                    //                 "||' (g)'" + "as " + "'" + elementname + "(" + Info.Weight + ")'";
                    //}
                    if (ReportTemplateHelper.IsShowArea)
                        strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + " then   round([ElementIntensity]," + WorkCurveHelper.SoftWareContentBit.ToString() + ")  else 0 end) as " + "'" + elementname + Info.strArea + "'";
                    if (ReportTemplateHelper.IsShowError)
                        strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + " then   round([Error]," + WorkCurveHelper.SoftWareContentBit.ToString() + ")  else 0 end) as " + "'" + elementname + Info.strError + "'";
                    if (ReportTemplateHelper.IsShowIntensity)
                        strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + " then   round([CaculateIntensity]," + WorkCurveHelper.SoftWareIntensityBit.ToString() + ")  else 0 end) as " + "'" + elementname + Info.Intensity + "'";
                    if (ReportTemplateHelper.IsShowAverageColumns)
                        strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + " then   round([AverageValue]," + WorkCurveHelper.SoftWareContentBit.ToString() + ")  else 0 end) as " + "'" + elementname + Info.MeanValue + "'";

                }
            }
            if (DifferenceDevice.IsAnalyser && DifferenceDevice.IsShowKarat)
            {
                strSql += " ,'0.0K' " + "as " + "'" + Info.IncludingAu + "'";
            }
            string strCondition = (ReportTemplateHelper.IsOnlyShowScanRecord) ? " and a.IsScan=1 " : "";
            switch (sType)
            {
                case 0:
                    strCondition += "  order by Id desc limit " + ROWS_PER_PAGE + "  offset " + Convert.ToString((currentPage < 0) ? 0 : currentPage * ROWS_PER_PAGE);
                    break;
                case 1:
                    if (strSelectID != "")
                        strCondition += "  and id in (" + strSelectID + ")";
                    break;
            }
            string strIntellCondition = comboBoxCurveName.Text == Info.Intelligent ? " type= " + ((int)ConditionType.Intelligent).ToString() + " and " : " type<>" + ((int)ConditionType.Intelligent).ToString() + " and ";
            strSql += "from historyelement a " +
                      " left outer join standarddata b on b.CustomStandard_Id=a.customstandard_Id and b.ElementCaption=a.elementName " +
                      " where a.historyrecord_id in (select id from historyrecord a where 1= 1  " + ((workcurve_id == "-1") ? " and a.workcurveid in (select id from workcurve where condition_id in ( " +
           " select id from condition where " + strIntellCondition + "device_id=" + WorkCurveHelper.DeviceCurrent.Id + "))" : " and  a.WorkCurveId in " + workcurve_id) + strSampleNameAndTestDate + strCondition + ")" +
                      " group by HistoryRecord_Id) b on b.HistoryRecord_Id=a.id where 1= 1 " + ((workcurve_id == "-1") ? " and a.workcurveid in (select id from workcurve where condition_id in ( " +
           " select id from condition where " + strIntellCondition + "device_id=" + WorkCurveHelper.DeviceCurrent.Id + "))" : " and  a.WorkCurveId in " + workcurve_id) + strSampleNameAndTestDate + strCondition;


            if (sType == 0)
                strSql += " ; select count(*) from historyrecord a  where 1= 1 " + ((workcurve_id == "-1") ? " and a.workcurveid in (select id from workcurve where condition_id in ( " +
           " select id from condition where " + strIntellCondition + "device_id=" + WorkCurveHelper.DeviceCurrent.Id + "))" : " and  a.WorkCurveId in " + workcurve_id) + strSampleNameAndTestDate + ((ReportTemplateHelper.IsOnlyShowScanRecord) ? " and a.IsScan=1 " : "");

            return strSql;


        }

        #region 获取公司其他信息
        private XmlNodeList xmlnodelist;
        private void GetCombInfo()
        {
            string sReportPath = AppDomain.CurrentDomain.BaseDirectory + "//printxml//CompanyInfo.xml";
            XmlDocument xmlDocReport = new XmlDocument();
            xmlDocReport.Load(sReportPath);
            string strWhere = "";
            if (ReportTemplateHelper.ExcelModeType != 2)
                strWhere = "/Data/template[@Name = '" + ReportTemplateHelper.ExcelModeType + "']";
            else
                strWhere = "/Data/template[@Name = '" + ReportTemplateHelper.LoadReportName() + "']";


            xmlnodelist = xmlDocReport.SelectNodes(strWhere);
        }

        private void GetCombReportInfoByTarget(string sTarget, ref string sCombReportInfo)
        {

            if (xmlnodelist != null || xmlnodelist.Count > 0)
            {
                string strWhere = "lable[@Target = '" + sTarget + "']";
                XmlNodeList childxmlnodelist = xmlnodelist[0].SelectNodes(strWhere);
                if (childxmlnodelist == null || childxmlnodelist.Count == 0) return;

                sCombReportInfo = childxmlnodelist[0].Attributes[WorkCurveHelper.LanguageShortName].Value;
            }
        }

        #endregion

        private void SetDecimalPlaces(ref DataTable dt)
        {
            //获取元素列对象
            List<string> ElemList = new List<string>();
            foreach (DataColumn col in dt.Columns)
            {
                Atom atom = Atoms.AtomList.Find(s => s.AtomName == col.ColumnName);
                if (atom == null) continue;
                ElemList.Add(col.ColumnName);
            }

            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    if (ElemList.Contains(col.ColumnName))
                    {
                        //小数点位数
                        string sValue = row[col.ColumnName].ToString();
                        sValue = (sValue.Contains("(")) ? sValue.Substring(0, sValue.IndexOf("(")) : sValue;
                        string sUnit = string.Empty;
                        if (sValue != "")
                            sUnit = row[col.ColumnName].ToString().Replace(sValue, "");
                        double temp = 0.0;
                        int bitTemp = (sUnit.Contains("(u")) ? WorkCurveHelper.ThickBit : WorkCurveHelper.SoftWareContentBit;
                        if (double.TryParse(sValue, out temp))
                            row[col.ColumnName] = double.Parse((sValue == "") ? "0" : sValue).ToString("f" + bitTemp.ToString()) + sUnit;

                    }
                }
            }
        }


        private void GetSepecifiedSearchConditionRefresh(int totalPage, int currentPage, Skyray.Controls.DataGridViewW dgvHistory)
        {
            if (comboBoxCurveName.Items.Count == 0) return;

            isCrossPage = false;
            checkBoxWSeleAll.Checked = false;


            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;//等待?

            //获取组合条件
            DataSet ds = GetDataSet(GetSQL(0, totalPage, currentPage, ""));
            if (ds == null || ds.Tables.Count < 2) { this.Cursor = System.Windows.Forms.Cursors.Arrow; return; }
            DataTable dt = ds.Tables[0];
            //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            //stopwatch.Start(); //  开始监视代码运行时间
            if ((DifferenceDevice.IsShowWeight || DifferenceDevice.IsShowKarat) && DifferenceDevice.IsAnalyser)
                RefreshWeightInfo(ref dt);
            SetDecimalPlaces(ref dt);
            SetTestResult(ref dt);

            //stopwatch.Stop();
            //TimeSpan timespan = stopwatch.Elapsed;
            //Console.WriteLine(timespan.TotalMilliseconds);
            dgvHistory.Columns.Clear();

            if (!dgvHistory.Columns.Contains("aa"))
            {
                DataGridViewCheckBoxColumn colCB = new DataGridViewCheckBoxColumn();
                colCB.Name = "aa";
                colCB.Width = 20;
                colCB.HeaderText = "";
                dgvHistory.Columns.Add(colCB);
            }

          

            if (WorkCurveHelper.WorkCurveCurrent.ElementList != null && WorkCurveHelper.WorkCurveCurrent.ElementList.Items != null && WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count > 0)
            {
                foreach (DataColumn dctemp in dt.Columns)
                {
                    CurveElement cueT = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().Find(w => (w.Caption + Info.Thick == dctemp.ColumnName || w.Caption + Info.Intensity == dctemp.ColumnName || w.Caption + Info.Error == dctemp.ColumnName));
                    if (cueT != null)
                    {
                        dctemp.ColumnName = dctemp.ColumnName.Replace(cueT.Caption, cueT.DefineElemName);
                        //if (dctemp.ColumnName.Contains(Info.Thick))

                        //    dctemp.ColumnName = cueT.DefineElemName + Info.Thick;
                        //else if (dctemp.ColumnName.Contains(Info.Intensity))
                        //    dctemp.ColumnName = cueT.DefineElemName + Info.Intensity;
                        //else if (dctemp.ColumnName.Contains(Info.Error))
                        //    dctemp.ColumnName = cueT.DefineElemName + Info.Error;

                    }

                }
            }


            dgvHistory.DataSource = dt;




            if (WorkCurveHelper.WorkCurveCurrent.ElementList != null && WorkCurveHelper.WorkCurveCurrent.ElementList.Items != null && WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count > 0)
            {
                if (FpWorkCurve.thickMode == ThickMode.NiNi || FpWorkCurve.thickMode == ThickMode.NiCuNi || FpWorkCurve.thickMode == ThickMode.NiCuNiFe)
                {
                    string colName = string.Empty;
                    foreach (DataColumn dctemp in dt.Columns)
                    {
                        if (dctemp.ColumnName.Contains("Fe" + Info.Thick))
                        {
                            dctemp.ColumnName = "Ni2" + Info.Thick;
                            break;
                        }

                    }

                    dgvHistory.DataSource = null;
                    dgvHistory.DataSource = dt;

                }
            }





            if (GP.CurrentUser.Role.RoleType != 0)
            {
                for (int m = 0; m < dgvHistory.Columns.Count; m++)
                {
                    dgvHistory.Columns[m].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
            dgvHistory.Columns["Id"].Visible = false;
            dgvHistory.Columns["HistoryRecordCode"].HeaderText = Info.strHistoryRecordCode;
            dgvHistory.Columns["SampleName"].HeaderText = Info.SampleName;
            dgvHistory.Columns["SpecListName"].HeaderText = Info.SpecName;
            //this.dgvHistoryRecord.Columns["SpecListName"].Visible = false;
            dgvHistory.Columns["Supplier"].HeaderText = Info.Supplier;
            // dgvHistory.Columns["Weight"].HeaderText = Info.Weight;
            // dgvHistory.Columns["Shape"].HeaderText = Info.Shape;
            dgvHistory.Columns["Operater"].HeaderText = Info.Operator;
            dgvHistory.Columns["SpecDate"].HeaderText = Info.SpecDate;
            dgvHistory.Columns["SpecDate"].SortMode = DataGridViewColumnSortMode.Automatic;
            //this.dgvHistoryRecord.Columns["SpecDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            //this.dgvHistoryRecord.Columns["SpecDate"].Width = 110;
            dgvHistory.Columns["SpecSummary"].HeaderText = Info.Description;
            dgvHistory.Columns["WorkCurveId"].HeaderText = Info.WorkingCurve;
            dgvHistory.Columns["WorkCurveId"].Visible = false;
            dgvHistory.Columns["WorkCurveName"].HeaderText = Info.WorkingCurve;
            dgvHistory.Columns["DeviceName"].HeaderText = Info.Device;
            dgvHistory.Columns["CaculateTime"].HeaderText = Info.CaculateTime;
            dgvHistory.Columns["HistoryRecord_Id"].Visible = false;
            dgvHistory.Columns["ActualVoltage"].HeaderText = Info.Voltage;
            dgvHistory.Columns["ActualCurrent"].HeaderText = Info.Current;
            dgvHistory.Columns["CountRate"].HeaderText = Info.CountRate;
            dgvHistory.Columns["PeakChannel"].HeaderText = Info.PeakChannel;
            if (ReportTemplateHelper.IsShowResole)
                dgvHistory.Columns["Resole"].HeaderText = Info.Resolve;
            //  this.dgvHistoryRecord.Columns["Resole"].Visible = false;
            dgvHistory.Columns["TotalCount"].HeaderText = Info.strTotalCount;
            dgvHistory.Columns["Height"].HeaderText = Info.Height;
            dgvHistory.Columns["CalcAngleHeight"].HeaderText = Info.CalcAngleHeight;
            if (DifferenceDevice.IsRohs || DifferenceDevice.IsThick || countryId == 0 || WorkCurveHelper.isShowXRFStandard == 1)
                dgvHistory.Columns["TestResult"].HeaderText = Info.TestResult;
            dgvHistory.Columns["Specifications"].HeaderText = Info.SpecificationsCategory;
            if (dgvHistory.Columns.Contains("CustomStandard"))
                dgvHistory.Columns["CustomStandard"].HeaderText = Info.StandardStone;

            //是否显示单位列
            if (DifferenceDevice.IsRohs && ReportTemplateHelper.IsShowUnitColumns) dgvHistory.Columns["Unit"].HeaderText = Info.strUnit;

            this.contextMenuStrip1.Items.Clear();


            //*******追加不同的曲线对应不同的历史记录设置 2014-04-22
            string historyItemPathSave = string.Empty;
            string historyItemPath = ReportTemplateHelper.GetHistoryItemPath((comboBoxCurveName.SelectedItem == null ? "-1" : ((MyItem)comboBoxCurveName.SelectedItem).Value.ToString()),
                                                                                WorkCurveHelper.DeviceCurrent.Id,
                                                                                (int)WorkCurveHelper.DeviceFunctype,
                                                                                (comboBoxCurveName.SelectedItem == null ? 0 : (int)((MyItem)comboBoxCurveName.SelectedItem).Type),
                                                                                out historyItemPathSave);
            string tempStr = ReportTemplateHelper.LoadHistoryItemSpecifiedValue(historyItemPath, "Setting");
            string[] sColumnWidthList = ReportTemplateHelper.LoadHistoryItemSpecifiedValue(historyItemPath, "ColumnWidth").Split(',');
            string[] ColumnIndexList = ReportTemplateHelper.LoadHistoryItemSpecifiedValue(historyItemPath, "ColumnIndex").Split(',');
            //*******************
            //stopwatch.Reset();
            //stopwatch.Start();
            string[] str = tempStr.Split(',');
            List<string> listStr = str.ToList();
            //var vv = User.Find(w => w.Name == FrmLogon.userName);
            //要显示的列
            List<string> ShowColums = new List<string>();
            ShowColums.Add("aa");
            for (int i = 0; i < ColumnIndexList.Length; i++)
            {
                string ColIndex = ColumnIndexList[i];
                if (ColIndex != string.Empty && ColIndex.IndexOf(":") != -1)
                {
                    ColIndex = ColIndex.Substring(0, ColIndex.IndexOf(":"));
                    if (dgvHistory.Columns.Contains(ColIndex))
                        ShowColums.Add(ColIndex);
                }
            }
            for (int i = 0; i < dgvHistory.Columns.Count; i++)
            {
                if (!ShowColums.Contains(dgvHistory.Columns[i].Name)) ShowColums.Add(dgvHistory.Columns[i].Name);
                if (dgvHistory.Columns[i].Name.Equals("aa") || dgvHistory.Columns[i].Name.Equals("continuousnumber")
                    || dgvHistory.Columns[i].Name.Equals("Id") || dgvHistory.Columns[i].Name.Equals("HistoryRecord_Id")
                    || dgvHistory.Columns[i].Name.Equals("customstandard_id") || dgvHistory.Columns[i].Name.EndsWith("_Color")
                    || dgvHistory.Columns[i].Name.Equals("WorkCurveId") || dgvHistory.Columns[i].Name.Equals("SpecListId")
                    )
                {
                    if (dgvHistory.Columns[i].Name.EndsWith("_Color")) dgvHistory.Columns[i].Visible = false;
                    continue;
                }
                if (dAddColmun.ContainsKey(dgvHistory.Columns[i].Name))
                {
                    dgvHistory.Columns[i].ReadOnly = false;
                    dgvHistory.Columns[i].HeaderText = dAddColmun[dgvHistory.Columns[i].Name];
                }
                else
                    dgvHistory.Columns[i].ReadOnly = true;
                if (Atoms.AtomList.Find(w => w.AtomName == dgvHistory.Columns[i].Name) != null)
                    dgvHistory.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                else if (dgvHistory.Columns[i].Name.IndexOf("|") != -1)
                    dgvHistory.Columns[i].Width = 100;
                else
                    dgvHistory.Columns[i].Width = 80;
                ToolStripMenuItem itemMenu = new ToolStripMenuItem();
                itemMenu.Name = dgvHistory.Columns[i].Name;
                itemMenu.Text = dgvHistory.Columns[i].HeaderText;
                if (listStr.Contains(dgvHistory.Columns[i].Name))
                    dgvHistory.Columns[i].Visible = false;
                if (sColumnWidthList.ToList().Exists(delegate(string v) { return v.ToLower().Substring(0, (v.ToLower().IndexOf(":") < 0 ? 0 : v.ToLower().IndexOf(":"))) == dgvHistory.Columns[i].Name.ToLower(); }))
                {
                    string sColumnWidth = sColumnWidthList.ToList().Find(delegate(string v) { return v.ToLower().Substring(0, (v.ToLower().IndexOf(":") < 0 ? 0 : v.ToLower().IndexOf(":"))) == dgvHistory.Columns[i].Name.ToLower(); });
                    //string sColumnWidth=sColumnWidthList.ToList().Find(delegate(string v) { return v.ToLower().Contains(dgvHistoryRecord.Columns[i].Name.ToLower() + ":"); });
                    if (sColumnWidth != "" && sColumnWidth.IndexOf(":") != -1)
                    {
                        sColumnWidth = sColumnWidth.ToLower().Replace(dgvHistory.Columns[i].Name.ToLower() + ":", "");
                        dgvHistory.Columns[i].Width = int.Parse(sColumnWidth);
                    }
                }

                //if (ColumnIndexList.ToList().Exists(delegate(string v) { return v.ToLower().Substring(0, (v.ToLower().IndexOf(":") < 0 ? 0 : v.ToLower().IndexOf(":"))) == dgvHistoryRecord.Columns[i].Name.ToLower(); }))
                //{
                //    string ColumnIndex = ColumnIndexList.ToList().Find(delegate(string v) { return v.ToLower().Substring(0, (v.ToLower().IndexOf(":") < 0 ? 0 : v.ToLower().IndexOf(":"))) == dgvHistoryRecord.Columns[i].Name.ToLower(); });
                //    if (ColumnIndex != "" && ColumnIndex.IndexOf(":") != -1)
                //    {
                //        ColumnIndex = ColumnIndex.ToLower().Replace(dgvHistoryRecord.Columns[i].Name.ToLower() + ":", "");
                //        if (dgvHistoryRecord.Columns.Count > int.Parse(ColumnIndex))
                //            this.dgvHistoryRecord.Columns[i].DisplayIndex = int.Parse(ColumnIndex);
                //    }
                //}


                itemMenu.Checked = dgvHistory.Columns[i].Visible;
                //if (vv.Count > 0 && vv[0].Role.RoleType == 2)
                //    continue;

                if (!this.contextMenuStrip1.Items.ContainsKey(itemMenu.Name))
                    this.contextMenuStrip1.Items.Add(itemMenu);

                // if (this.contextMenuStrip1.Items.ContainsKey(itemMenu.Name))
                //this.dgvHistoryRecord.Columns[i].Visible = ((ToolStripMenuItem)this.contextMenuStrip1.Items.Find(itemMenu.Name, false)[0]).Checked;




                itemMenu.Click += new EventHandler(itemMenu_Click);




            }




            if (FpWorkCurve.thickMode == ThickMode.NiNi || FpWorkCurve.thickMode == ThickMode.NiCuNi || FpWorkCurve.thickMode == ThickMode.NiCuNiFe)
            {
                ShowColums.Remove("Ni2" + Info.Thick);
                ShowColums.Add("Ni2" + Info.Thick);
            }



            //修改显示顺序
            for (int i = 0; i < dgvHistory.Columns.Count; i++)
            {
                dgvHistory.Columns[ShowColums[i]].DisplayIndex = i;
            }









            if (this.contextMenuStrip1.Items.Count > 0)
            {
                ToolStripMenuItem setItem = new ToolStripMenuItem();
                setItem.Name = "SettingContext";
                setItem.Text = Info.ChoiceSetting;
                setItem.Click += new EventHandler(setItem_Click);
                if (!this.contextMenuStrip1.Items.ContainsKey(setItem.Name))
                    this.contextMenuStrip1.Items.Add(setItem);


                ToolStripMenuItem setItemUserColumnManage = new ToolStripMenuItem();
                setItemUserColumnManage.Name = "ColumnManage";
                setItemUserColumnManage.Text = Info.strUserColumnManage;
                setItemUserColumnManage.Click += new EventHandler(setItemUserColumnManage_Click);
                if (!this.contextMenuStrip1.Items.ContainsKey(setItemUserColumnManage.Name))
                    this.contextMenuStrip1.Items.Add(setItemUserColumnManage);
            }


            DataGridViewW dgv = new DataGridViewW();

            foreach (DataGridViewColumn column in dgvHistory.Columns)
            {
                dgv.Columns.Add((DataGridViewColumn)column.Clone());
            }

            foreach (DataGridViewRow row in dgvHistory.Rows)
            {
                dgv.Rows.Add(row.Clone());
            }


            
            int num = 0;

            for (int i = 0; i < dgv.ColumnCount; i++)
            {
                if (dgv.Columns[i].HeaderText.Contains("厚度"))
                {
                    DataGridViewColumn dataColumn = (DataGridViewColumn)dgv.Columns[i].Clone();
                    dataColumn.HeaderText = dgv.Columns[i].HeaderText.Split(new char[] { '厚' })[0] + "折线";
                    dataColumn.Name = dataColumn.HeaderText;     
                    num++;
                    dgvHistory.Columns.Insert(i + num, dataColumn);

                    if (!tempStr.Contains(dataColumn.HeaderText))
                    {
                        dgvHistory.Columns[i + num].Visible = false;
                    }
                }
            }       
                

            string temp = "";
            int loc = 0;
            string unit = "";


            for (int i = 0; i < dgvHistory.Columns.Count; i++)
            {
                if (dgvHistory.Columns[i].HeaderText.Contains("厚度") || dgvHistory.Columns[i].HeaderText.Contains("Thickness") || dgvHistory.Columns[i].HeaderText.Contains("面密度") || dgvHistory.Columns[i].HeaderText.Contains("Density") || dgvHistory.Columns[i].HeaderText.Contains("含量") || dgvHistory.Columns[i].HeaderText.Contains("Concentration"))
                {

                    for (int j = 0; j < dgvHistory.Rows.Count; j++)
                    {

                        temp = dgvHistory.Rows[j].Cells[i].Value.ToString();
                        loc = temp.IndexOf('(');

                        if (loc > 0)
                        {
                            unit = dgvHistory.Rows[j].Cells[i].Value.ToString().Substring(loc, temp.Length - loc);
                            dgvHistory.Rows[j].Cells[i].Value = temp.Substring(0, loc).Trim();
                        }
                    }


                    if (dgvHistory.Columns[i].HeaderText.Contains("厚度") || dgvHistory.Columns[i].HeaderText.Contains("Thickness"))
                    {
                        for (int index = 0; index < WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count; index++)
                        {
                            if (WorkCurveHelper.WorkCurveCurrent.ElementList.Items[index].IsShowElement)
                            {
                                if (WorkCurveHelper.WorkCurveCurrent.ElementList.Items[index].ThicknessUnit == ThicknessUnit.ur)
                                {
                                    dgvHistory.Columns[i].HeaderText = dgvHistory.Columns[i].HeaderText + "(" +this.cmbThicknessUnit.Items[0] + ")";    
                                }
                                else if (WorkCurveHelper.WorkCurveCurrent.ElementList.Items[index].ThicknessUnit == ThicknessUnit.um)
                                {
                                    dgvHistory.Columns[i].HeaderText = dgvHistory.Columns[i].HeaderText + "(" +this.cmbThicknessUnit.Items[1] + ")";    
                                    
                                }
                                else
                                {
                                    dgvHistory.Columns[i].HeaderText = dgvHistory.Columns[i].HeaderText + "(" + this.cmbThicknessUnit.Items[2] + ")";    
                                    
                                }

                                break;
                            }
                        }


                    }
                    else if (dgvHistory.Columns[i].HeaderText.Contains("面密度") || dgvHistory.Columns[i].HeaderText.Contains("Density"))
                    {
                        if (WorkCurveHelper.WorkCurveCurrent.AreaThickType == "g/m^2")
                            dgvHistory.Columns[i].HeaderText = dgvHistory.Columns[i].HeaderText + "(" + this.cmbDensityUnit.Items[0] + ")";
                        else
                            dgvHistory.Columns[i].HeaderText = dgvHistory.Columns[i].HeaderText + "(" + this.cmbDensityUnit.Items[1] + ")";
                    }
                    else if (dgvHistory.Columns[i].HeaderText.Contains("含量") || dgvHistory.Columns[i].HeaderText.Contains("Concentration"))
                    {
                        dgvHistory.Columns[i].HeaderText = dgvHistory.Columns[i].HeaderText + "(%)";

                    }

                }



                if (dgvHistory.Columns[i].HeaderText.Contains("样品名称") || dgvHistory.Columns[i].HeaderText.Contains("Sample"))
                {

                    for (int j = 0; j < dgvHistory.Rows.Count; j++)
                    {

                        if(dgvHistory.Rows[j].Cells[i].Value.ToString().Contains("#"))
                        {
                            dgvHistory.Rows[j].Cells[i].Value = dgvHistory.Rows[j].Cells[i].Value.ToString().Split(new char[]{'#'})[0];

                        }
                    }

                }
            }




            dgvHistory.Refresh();


            int totalRow = int.Parse(ds.Tables[1].Rows[0][0].ToString());
            lblTotaleRowCount.Text = "0";
            lblTotaleRowCount.Text = totalRow.ToString();
            labelTotalPage.Text = (Convert.ToInt32(Math.Ceiling((totalRow + 0.0) / ROWS_PER_PAGE))).ToString();
            labelCurrentPage.Text = Convert.ToString(currentPage + 1);

            int selei = 0;
            if (this.selectLong.Count > 0 && dgvHistory.Rows.Count > 0)
            {

                foreach (DataGridViewRow row in dgvHistory.Rows)
                {
                    long selectId = long.Parse(row.Cells["Id"].Value.ToString());
                    if (selectLong.Contains(selectId))
                    {
                        selei++;
                        row.Cells[0].Value = true;
                    }
                }
            }

            if (selei == ROWS_PER_PAGE) checkBoxWSeleAll.Checked = true;

            ////修改：何晓明 20110818 历史记录小数精度
            //for (int i = 0; i < this.dgvHistoryRecord.Columns.Count; i++)
            //{
            //    if (this.dgvHistoryRecord.Columns[i].Name != "SpecDate")
            //        this.dgvHistoryRecord.Columns[i].DefaultCellStyle.Format = "f" + HistoryRecordDecimalMedian.ToString();
            //}

            isCrossPage = true;
            //stopwatch.Stop();
            //timespan = stopwatch.Elapsed;
            //Console.WriteLine(timespan.TotalMilliseconds);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;

        }



        private Pen unNormalPen = new Pen(Color.Red, 5);

        private Pen normalPen = new Pen(Color.Blue, 5);

        private Pen linePen = new Pen(Color.Blue, 2);

        private List<Point> colPrePoints = new List<Point>();
        private List<bool> colPreNormals = new List<bool>();

        private Brush unNormalBack = new SolidBrush(Color.Red);

        private void dgvHistoryRecord_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {


            if (e.RowIndex == -1)
                return;


            if (dgvHistoryRecord.Columns[e.ColumnIndex].Name.Contains("折线") && dgvHistoryRecord.Columns[e.ColumnIndex].Visible)
            {

                using (Brush backColorBrush = new SolidBrush(Color.Cyan))
                {
                    e.Graphics.FillRectangle(backColorBrush, e.CellBounds);
                }

                DataGridViewColumn dataColumn = (DataGridViewColumn)dgvHistoryRecord.Columns[e.ColumnIndex].Clone();
                string eleName = dataColumn.Name.Split(new char[] { '折' })[0];

                double low = 0f;
                double high = 0f;

                CustomStandard devStandard = WorkCurveHelper.CurrentStandard;

                if (devStandard != null)
                {
                    bool find = false;
                    for (int j = 0; j < devStandard.StandardDatas.Count; j++)
                    {
                        if (devStandard.StandardDatas[j].ElementCaption.Equals(eleName))
                        {
                            low = devStandard.StandardDatas[j].StandardThick;
                            high = devStandard.StandardDatas[j].StandardThickMax;
                            find = true;
                            break;
                        }
                    }
                    if (!find)
                    {
                        low = 0;
                        high = 100;

                    }
                }
                else
                {
                    low = 0;
                    high = 100;
                }



                int x = -1;
                int y = e.CellBounds.Y + (int)(e.CellBounds.Height / 2.0);




                int radius = 2;
                if (e.RowIndex == 0)
                {
                    colPreNormals[e.ColumnIndex] = false;
                    dgvHistoryRecord.Columns[e.ColumnIndex].HeaderText = low.ToString("f2") + "--" + high.ToString("f2");

                }
                else
                    colPreNormals[e.ColumnIndex] = true;

                double thickNess = dgvHistoryRecord[e.ColumnIndex, e.RowIndex].Value.ToString().Contains("--") ? double.MaxValue : double.Parse(dgvHistoryRecord[e.ColumnIndex, e.RowIndex].Value.ToString().Split(new char[] { '(' })[0].Trim());

                if (thickNess <= low)
                {


                    // 填充单元格背景
                    e.Graphics.FillRectangle(unNormalBack, e.CellBounds);

                    x = e.CellBounds.X;
                    e.Graphics.DrawEllipse(unNormalPen, x, y, 0, 0);
                    if (colPreNormals[e.ColumnIndex])
                        e.Graphics.DrawLine(linePen, colPrePoints[e.ColumnIndex], new Point(x, y));
                    colPrePoints[e.ColumnIndex] = new Point(x, y);



                }
                else if (thickNess >= high)
                {
                    // 填充单元格背景
                    e.Graphics.FillRectangle(unNormalBack, e.CellBounds);

                    x = e.CellBounds.X + e.CellBounds.Width;
                    e.Graphics.DrawEllipse(unNormalPen, x, y, 0, 0);
                    if (colPreNormals[e.ColumnIndex])
                        e.Graphics.DrawLine(linePen, colPrePoints[e.ColumnIndex], new Point(x, y));
                    colPrePoints[e.ColumnIndex] = new Point(x, y);


                }
                else
                {

                    x = e.CellBounds.X + (int)((thickNess - low) / (high - low) * e.CellBounds.Width);
                    if (x < e.CellBounds.X + 4)
                        x = e.CellBounds.X + 4;
                    else if (x > e.CellBounds.X + e.CellBounds.Width - 4)
                        x = e.CellBounds.X + e.CellBounds.Width - 4;

                    e.Graphics.DrawEllipse(normalPen, x, y, radius, radius);
                    if (colPreNormals[e.ColumnIndex])
                        e.Graphics.DrawLine(linePen, colPrePoints[e.ColumnIndex], new Point(x, y));
                    colPrePoints[e.ColumnIndex] = new Point(x, y);

                }


                e.Handled = true;
            }



        }



        //private void GetSepecifiedSearchCondition(int totalPage, int currentPage)
        //{
        //    if (comboBoxCurveName.Items.Count == 0) return;

        //    isCrossPage = false;
        //    checkBoxWSeleAll.Checked = false;


        //    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;//等待?

        //    //获取组合条件
        //    DataSet ds = GetDataSet(GetSQL(0, totalPage, currentPage, ""));
        //    if (ds == null || ds.Tables.Count < 2) { this.Cursor = System.Windows.Forms.Cursors.Arrow; return; }
        //    DataTable dt = ds.Tables[0];
        //    //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        //    //stopwatch.Start(); //  开始监视代码运行时间
        //    if ((DifferenceDevice.IsShowWeight || DifferenceDevice.IsShowKarat) && DifferenceDevice.IsAnalyser)
        //        RefreshWeightInfo(ref dt);
        //    SetDecimalPlaces(ref dt);
        //    SetTestResult(ref dt);
        //    //stopwatch.Stop();
        //    //TimeSpan timespan = stopwatch.Elapsed;
        //    //Console.WriteLine(timespan.TotalMilliseconds);
        //    this.dgvHistoryRecord.Columns.Clear();

        //    if (!this.dgvHistoryRecord.Columns.Contains("aa"))
        //    {
        //        DataGridViewCheckBoxColumn colCB = new DataGridViewCheckBoxColumn();
        //        colCB.Name = "aa";
        //        colCB.Width = 20;
        //        colCB.HeaderText = "";
        //        this.dgvHistoryRecord.Columns.Add(colCB);
        //    }

        //    //this.dataGridViewW1.AllowUserToResizeColumns = true;
        //    //this.dataGridViewW1.AllowUserToOrderColumns = false;
        //    this.dgvHistoryRecord.DataSource = dt;
        //    if (GP.CurrentUser.Role.RoleType != 0)
        //    {
        //        for (int m = 0; m < this.dgvHistoryRecord.Columns.Count; m++)
        //        {
        //            this.dgvHistoryRecord.Columns[m].SortMode = DataGridViewColumnSortMode.NotSortable;
        //        }
        //    }
        //    this.dgvHistoryRecord.Columns["Id"].Visible = false;
        //    this.dgvHistoryRecord.Columns["HistoryRecordCode"].HeaderText = Info.strHistoryRecordCode;
        //    this.dgvHistoryRecord.Columns["SampleName"].HeaderText = Info.SampleName;
        //    this.dgvHistoryRecord.Columns["SpecListName"].HeaderText = Info.SpecName;
        //    //this.dgvHistoryRecord.Columns["SpecListName"].Visible = false;
        //    this.dgvHistoryRecord.Columns["Supplier"].HeaderText = Info.Supplier;
        //    this.dgvHistoryRecord.Columns["Weight"].HeaderText = Info.Weight;
        //    this.dgvHistoryRecord.Columns["Shape"].HeaderText = Info.Shape;
        //    this.dgvHistoryRecord.Columns["Operater"].HeaderText = Info.Operator;
        //    this.dgvHistoryRecord.Columns["SpecDate"].HeaderText = Info.SpecDate;
        //    this.dgvHistoryRecord.Columns["SpecDate"].SortMode = DataGridViewColumnSortMode.Automatic;
        //    //this.dgvHistoryRecord.Columns["SpecDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        //    //this.dgvHistoryRecord.Columns["SpecDate"].Width = 110;
        //    this.dgvHistoryRecord.Columns["SpecSummary"].HeaderText = Info.Description;
        //    this.dgvHistoryRecord.Columns["WorkCurveId"].HeaderText = Info.WorkingCurve;
        //    this.dgvHistoryRecord.Columns["WorkCurveId"].Visible = false;
        //    this.dgvHistoryRecord.Columns["WorkCurveName"].HeaderText = Info.WorkingCurve;
        //    this.dgvHistoryRecord.Columns["DeviceName"].HeaderText = Info.Device;
        //    this.dgvHistoryRecord.Columns["CaculateTime"].HeaderText = Info.CaculateTime;
        //    this.dgvHistoryRecord.Columns["HistoryRecord_Id"].Visible = false;
        //    this.dgvHistoryRecord.Columns["ActualVoltage"].HeaderText = Info.Voltage;
        //    this.dgvHistoryRecord.Columns["ActualCurrent"].HeaderText = Info.Current;
        //    this.dgvHistoryRecord.Columns["CountRate"].HeaderText = Info.CountRate;
        //    this.dgvHistoryRecord.Columns["PeakChannel"].HeaderText = Info.PeakChannel;
        //    if (ReportTemplateHelper.IsShowResole)
        //        this.dgvHistoryRecord.Columns["Resole"].HeaderText = Info.Resolve;
        //  //  this.dgvHistoryRecord.Columns["Resole"].Visible = false;
        //    this.dgvHistoryRecord.Columns["TotalCount"].HeaderText = Info.strTotalCount;
        //    //this.dgvHistoryRecord.Columns["AreaDensity"].HeaderText = Info.strAreaDensity;
        //    //this.dgvHistoryRecord.Columns["AreaDensity"].SortMode = DataGridViewColumnSortMode.Automatic;
        //    //this.dgvHistoryRecord.Columns["SpecListId"].Visible = false;//TestResult
        //    if (DifferenceDevice.IsRohs)
        //        this.dgvHistoryRecord.Columns["TestResult"].HeaderText = Info.TestResult;
        //    this.dgvHistoryRecord.Columns["Specifications"].HeaderText = Info.SpecificationsCategory;

        //    //是否显示单位列
        //    if (DifferenceDevice.IsRohs && ReportTemplateHelper.IsShowUnitColumns) this.dgvHistoryRecord.Columns["Unit"].HeaderText = Info.strUnit;

        //    //this.dataGridViewW1.ReadOnly = true;

        //    this.contextMenuStrip1.Items.Clear();


        //    //*******追加不同的曲线对应不同的历史记录设置 2014-04-22
        //    string historyItemPathSave = string.Empty;
        //    string historyItemPath = ReportTemplateHelper.GetHistoryItemPath(((MyItem)comboBoxCurveName.SelectedItem).Value.ToString(),
        //                                                                        WorkCurveHelper.DeviceCurrent.Id,
        //                                                                        (int)WorkCurveHelper.DeviceFunctype,
        //                                                                        (int)((MyItem)comboBoxCurveName.SelectedItem).Type,
        //                                                                        out historyItemPathSave);
        //    //string tempStr = ReportTemplateHelper.LoadHistoryItemSpecifiedValue("HistoryItem", "Setting");
        //    //string[] sColumnWidthList = ReportTemplateHelper.LoadHistoryItemSpecifiedValue("HistoryItem", "ColumnWidth").Split(',');
        //    //string[] ColumnIndexList = ReportTemplateHelper.LoadHistoryItemSpecifiedValue("HistoryItem", "ColumnIndex").Split(',');
        //    string tempStr = ReportTemplateHelper.LoadHistoryItemSpecifiedValue(historyItemPath, "Setting");
        //    string[] sColumnWidthList = ReportTemplateHelper.LoadHistoryItemSpecifiedValue(historyItemPath, "ColumnWidth").Split(',');
        //    string[] ColumnIndexList = ReportTemplateHelper.LoadHistoryItemSpecifiedValue(historyItemPath, "ColumnIndex").Split(',');
        //    //*******************
        //    //stopwatch.Reset();
        //    //stopwatch.Start();
        //    string[] str = tempStr.Split(',');
        //    List<string> listStr = str.ToList();
        //    //var vv = User.Find(w => w.Name == FrmLogon.userName);
        //    //要显示的列
        //    List<string> ShowColums = new List<string>();
        //    ShowColums.Add("aa");
        //    for (int i = 0; i < ColumnIndexList.Length; i++)
        //    {
        //        string ColIndex = ColumnIndexList[i];
        //        if (ColIndex != string.Empty && ColIndex.IndexOf(":") != -1)
        //        {
        //            ColIndex = ColIndex.Substring(0, ColIndex.IndexOf(":"));
        //            if (this.dgvHistoryRecord.Columns.Contains(ColIndex))
        //                ShowColums.Add(ColIndex);
        //        }
        //    }
        //    for (int i = 0; i < this.dgvHistoryRecord.Columns.Count; i++)
        //    {
        //        if (!ShowColums.Contains(this.dgvHistoryRecord.Columns[i].Name)) ShowColums.Add(this.dgvHistoryRecord.Columns[i].Name);
        //        if (this.dgvHistoryRecord.Columns[i].Name.Equals("aa") || this.dgvHistoryRecord.Columns[i].Name.Equals("continuousnumber")
        //            || this.dgvHistoryRecord.Columns[i].Name.Equals("Id") || this.dgvHistoryRecord.Columns[i].Name.Equals("HistoryRecord_Id")
        //            || this.dgvHistoryRecord.Columns[i].Name.Equals("customstandard_id") || this.dgvHistoryRecord.Columns[i].Name.EndsWith("_Color")
        //            || this.dgvHistoryRecord.Columns[i].Name.Equals("WorkCurveId") || this.dgvHistoryRecord.Columns[i].Name.Equals("SpecListId")
        //            )
        //            continue;
        //        if (dAddColmun.ContainsKey(dgvHistoryRecord.Columns[i].Name))
        //        {
        //            this.dgvHistoryRecord.Columns[i].ReadOnly = false;
        //            this.dgvHistoryRecord.Columns[i].HeaderText = dAddColmun[dgvHistoryRecord.Columns[i].Name];
        //        }
        //        else
        //            this.dgvHistoryRecord.Columns[i].ReadOnly = true;
        //        if (Atoms.AtomList.Find(w => w.AtomName == this.dgvHistoryRecord.Columns[i].Name) != null)
        //            this.dgvHistoryRecord.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        //        else if (this.dgvHistoryRecord.Columns[i].Name.IndexOf("|") != -1)
        //            this.dgvHistoryRecord.Columns[i].Width = 100;
        //        else
        //            this.dgvHistoryRecord.Columns[i].Width = 80;
        //        ToolStripMenuItem itemMenu = new ToolStripMenuItem();
        //        itemMenu.Name = this.dgvHistoryRecord.Columns[i].Name;
        //        itemMenu.Text = this.dgvHistoryRecord.Columns[i].HeaderText;
        //        if (listStr.Contains(this.dgvHistoryRecord.Columns[i].Name))
        //            this.dgvHistoryRecord.Columns[i].Visible = false;
        //        //if (this.dgvHistoryRecord.Columns[i].Name.Equals("Resole"))
        //        //    this.dgvHistoryRecord.Columns[i].Visible = false;
        //        //if (this.contextMenuStrip1.Items.ContainsKey(itemMenu.Name) && !((ToolStripMenuItem)this.contextMenuStrip1.Items.Find(itemMenu.Name, false)[0]).Checked)
        //        //{
        //        //    this.dataGridViewW1.Columns[i].Visible = false;
        //        //}


        //        if (sColumnWidthList.ToList().Exists(delegate(string v) { return v.ToLower().Substring(0, (v.ToLower().IndexOf(":") < 0 ? 0 : v.ToLower().IndexOf(":"))) == dgvHistoryRecord.Columns[i].Name.ToLower(); }))
        //        {
        //            string sColumnWidth = sColumnWidthList.ToList().Find(delegate(string v) { return v.ToLower().Substring(0, (v.ToLower().IndexOf(":") < 0 ? 0 : v.ToLower().IndexOf(":"))) == dgvHistoryRecord.Columns[i].Name.ToLower(); });
        //            //string sColumnWidth=sColumnWidthList.ToList().Find(delegate(string v) { return v.ToLower().Contains(dgvHistoryRecord.Columns[i].Name.ToLower() + ":"); });
        //            if (sColumnWidth != "" && sColumnWidth.IndexOf(":") != -1)
        //            {
        //                sColumnWidth = sColumnWidth.ToLower().Replace(dgvHistoryRecord.Columns[i].Name.ToLower() + ":", "");
        //                this.dgvHistoryRecord.Columns[i].Width = int.Parse(sColumnWidth);
        //            }
        //        }

        //        //if (ColumnIndexList.ToList().Exists(delegate(string v) { return v.ToLower().Substring(0, (v.ToLower().IndexOf(":") < 0 ? 0 : v.ToLower().IndexOf(":"))) == dgvHistoryRecord.Columns[i].Name.ToLower(); }))
        //        //{
        //        //    string ColumnIndex = ColumnIndexList.ToList().Find(delegate(string v) { return v.ToLower().Substring(0, (v.ToLower().IndexOf(":") < 0 ? 0 : v.ToLower().IndexOf(":"))) == dgvHistoryRecord.Columns[i].Name.ToLower(); });
        //        //    if (ColumnIndex != "" && ColumnIndex.IndexOf(":") != -1)
        //        //    {
        //        //        ColumnIndex = ColumnIndex.ToLower().Replace(dgvHistoryRecord.Columns[i].Name.ToLower() + ":", "");
        //        //        if (dgvHistoryRecord.Columns.Count > int.Parse(ColumnIndex))
        //        //            this.dgvHistoryRecord.Columns[i].DisplayIndex = int.Parse(ColumnIndex);
        //        //    }
        //        //}


        //        itemMenu.Checked = this.dgvHistoryRecord.Columns[i].Visible;
        //        //if (vv.Count > 0 && vv[0].Role.RoleType == 2)
        //        //    continue;

        //        if (!this.contextMenuStrip1.Items.ContainsKey(itemMenu.Name))
        //            this.contextMenuStrip1.Items.Add(itemMenu);

        //       // if (this.contextMenuStrip1.Items.ContainsKey(itemMenu.Name))
        //        //this.dgvHistoryRecord.Columns[i].Visible = ((ToolStripMenuItem)this.contextMenuStrip1.Items.Find(itemMenu.Name, false)[0]).Checked;




        //        itemMenu.Click += new EventHandler(itemMenu_Click);




        //    }

        //    //修改显示顺序
        //    for (int i = 0; i < this.dgvHistoryRecord.Columns.Count; i++)
        //    {
        //        this.dgvHistoryRecord.Columns[ShowColums[i]].DisplayIndex = i;
        //    }
        //    if (this.contextMenuStrip1.Items.Count > 0)
        //    {
        //        ToolStripMenuItem setItem = new ToolStripMenuItem();
        //        setItem.Name = "SettingContext";
        //        setItem.Text = Info.ChoiceSetting;
        //        setItem.Click += new EventHandler(setItem_Click);
        //        if (!this.contextMenuStrip1.Items.ContainsKey(setItem.Name))
        //            this.contextMenuStrip1.Items.Add(setItem);


        //        ToolStripMenuItem setItemUserColumnManage = new ToolStripMenuItem();
        //        setItemUserColumnManage.Name = "ColumnManage";
        //        setItemUserColumnManage.Text = Info.strUserColumnManage;
        //        setItemUserColumnManage.Click += new EventHandler(setItemUserColumnManage_Click);
        //        if (!this.contextMenuStrip1.Items.ContainsKey(setItemUserColumnManage.Name))
        //            this.contextMenuStrip1.Items.Add(setItemUserColumnManage);
        //    }
        //    this.dgvHistoryRecord.Refresh();


        //    int totalRow = int.Parse(ds.Tables[1].Rows[0][0].ToString());
        //    lblTotaleRowCount.Text = "0";
        //    lblTotaleRowCount.Text = totalRow.ToString();
        //    labelTotalPage.Text = (Convert.ToInt32(Math.Ceiling((totalRow + 0.0) / ROWS_PER_PAGE))).ToString();
        //    labelCurrentPage.Text = Convert.ToString(currentPage + 1);

        //    int selei = 0;
        //    if (this.selectLong.Count > 0 && this.dgvHistoryRecord.Rows.Count > 0)
        //    {

        //        foreach (DataGridViewRow row in this.dgvHistoryRecord.Rows)
        //        {
        //            long selectId = long.Parse(row.Cells["Id"].Value.ToString());
        //            if (selectLong.Contains(selectId))
        //            {
        //                selei++;
        //                row.Cells[0].Value = true;
        //            }
        //        }
        //    }

        //    if (selei == ROWS_PER_PAGE) checkBoxWSeleAll.Checked = true;

        //    ////修改：何晓明 20110818 历史记录小数精度
        //    //for (int i = 0; i < this.dgvHistoryRecord.Columns.Count; i++)
        //    //{
        //    //    if (this.dgvHistoryRecord.Columns[i].Name != "SpecDate")
        //    //        this.dgvHistoryRecord.Columns[i].DefaultCellStyle.Format = "f" + HistoryRecordDecimalMedian.ToString();
        //    //}

        //    isCrossPage = true;
        //    //stopwatch.Stop();
        //    //timespan = stopwatch.Elapsed;
        //    //Console.WriteLine(timespan.TotalMilliseconds);
        //    this.Cursor = System.Windows.Forms.Cursors.Arrow;
        //}

        #region 动态加载列

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void setItemUserColumnManage_Click(object sender, EventArgs e)
        {
            FrmHistoryColumnManage frm = new FrmHistoryColumnManage();
            if (DialogResult.OK == frm.ShowDialog())
            {
                int totalPage = int.Parse(this.labelTotalPage.Text.Trim());
                int currentPage = int.Parse(labelCurrentPage.Text.Trim());
                GetSepecifiedSearchCondition(totalPage, currentPage - 1);
            }
        }

        private void GetSetColmun()
        {
            dAddColmun.Clear();
            string[] strcolmun = ReportTemplateHelper.LoadSpecifiedValue("HistoryItem", "AddColumn").Split(',');
            foreach (string strc in strcolmun)
            {
                if (strc.IndexOf("~") != -1)
                {
                    dAddColmun.Add(strc.Split('~')[0], strc.Split('~')[1]);
                }
            }

        }

        private void DeleColmun()
        {
            dAddColmun.Clear();
            string[] strcolmun = ReportTemplateHelper.LoadSpecifiedValue("HistoryItem", "AddColumn").Split(',');
            foreach (string strc in strcolmun)
            {
                if (strc.IndexOf("~") != -1)
                {
                    dAddColmun.Add(strc.Split('~')[0], strc.Split('~')[1]);
                }
            }

            //删除字段处理
            bool isdelecolumn = false;
            string stroldcolumn = "";
            string stroldcolumn1 = "";
            DataTable dt = GetData("select sql from sqlite_master where tbl_name='HistoryRecord' and type='table'");
            string sinfo = dt.Rows[0][0].ToString();
            int ibracket = sinfo.IndexOf("(");
            sinfo = sinfo.Substring(ibracket + 1, sinfo.Length - (ibracket + 1));

            List<string> deleColmun = new List<string>();
            string[] colmun = sinfo.Split(',');
            foreach (string s in colmun)
            {
                string cil = "";
                if (s.Split(' ')[0] == "") cil = s.Split(' ')[1]; else cil = s.Split(' ')[0];
                if (cil.IndexOf('[') != -1) cil = cil.Split('[')[1];
                if (cil.IndexOf(']') != -1) cil = cil.Split(']')[0];


                if (cil.Contains("column") && !dAddColmun.ContainsKey(cil)) isdelecolumn = true;

                if (cil.Contains("column") && dAddColmun.ContainsKey(cil))
                {
                    stroldcolumn1 += "," + cil.Replace("(", "").Replace(")", "");
                    stroldcolumn += "," + s.Replace("(", "").Replace(")", "");
                }
                else if (!cil.Contains("column"))
                {
                    stroldcolumn1 += "," + cil;
                    stroldcolumn += "," + s;
                }
                else
                    deleColmun.Add(cil);
            }



            if (isdelecolumn)
            {

                string strSetting = ReportTemplateHelper.LoadSpecifiedValue("HistoryItem", "Setting");
                foreach (string s in deleColmun)
                    if (strSetting.Contains(s)) strSetting = strSetting.Replace(s, "");
                ReportTemplateHelper.SaveSpecifiedValue("HistoryItem", "Setting", strSetting);

                stroldcolumn1 = stroldcolumn1.Substring(1, stroldcolumn1.Length - 1);
                stroldcolumn = stroldcolumn.Substring(1, stroldcolumn.Length - 1);
                string delecolmun = @"BEGIN TRANSACTION; " +
                " CREATE TEMPORARY TABLE HistoryRecord_backup(" + stroldcolumn + ");" +
                " INSERT INTO HistoryRecord_backup SELECT " + stroldcolumn1 + " FROM HistoryRecord;" +
                " DROP TABLE HistoryRecord;" +
                " CREATE TABLE HistoryRecord(" + stroldcolumn + ");" +
                " INSERT INTO HistoryRecord SELECT " + stroldcolumn1 + " FROM HistoryRecord_backup;" +
                " DROP TABLE HistoryRecord_backup;" +
                " COMMIT;";

                Lephone.Data.DbEntry.Context.ExecuteNonQuery(delecolmun);
            }
            else if (isdelecolumn && dAddColmun.Count > 0)
            {
                dAddColmun.Clear();
                ReportTemplateHelper.SaveSpecifiedValue("HistoryItem", "AddColumn", "");
            }







        }

        private void dataGridViewW1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgvHistoryRecord.CurrentRow == null || dgvHistoryRecord.RowCount <= 0
                || !dAddColmun.ContainsKey(dgvHistoryRecord.Columns[e.ColumnIndex].Name)
                || e.FormattedValue.ToString() == "") return;

            string strsql = "update HistoryRecord set " + dgvHistoryRecord.Columns[e.ColumnIndex].Name + "='" + e.FormattedValue.ToString() + "' where Id=" + dgvHistoryRecord.Rows[e.RowIndex].Cells["Id"].Value.ToString() + ";";

            Lephone.Data.DbEntry.Context.ExecuteNonQuery(strsql);




        }

        #endregion

        private bool isCrossPage = true;//翻页时，全选为空，防止为空时，进行非全选时间发生

        private void checkShowUnit_Click(object sender, EventArgs e)
        {
            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedValue("OpenHistoryRecordType", "HistoryRecordShowUnitType", (checkShowUnit.Checked) ? "1" : "0");
            ReportTemplateHelper.ShowUnitType = checkShowUnit.Checked;
            buttonWSearch_Click(null, null);
        }




        #endregion

        private void dataGridViewW1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex != -1)
            {
                if (Convert.ToBoolean(dgvHistoryRecord.Rows[e.RowIndex].Cells["aa"].Value) == false)
                {
                    dgvHistoryRecord.Rows[e.RowIndex].Cells["aa"].Value = true;
                    long selectId = long.Parse(dgvHistoryRecord.Rows[e.RowIndex].Cells["Id"].Value.ToString());
                    if (!selectLong.Contains(selectId)) selectLong.Add(selectId);
                }
                else
                {
                    dgvHistoryRecord.Rows[e.RowIndex].Cells["aa"].Value = false;
                    long selectId = long.Parse(dgvHistoryRecord.Rows[e.RowIndex].Cells["Id"].Value.ToString());
                    if (selectLong.Contains(selectId)) selectLong.Remove(selectId);
                }
            }


        }

        private void checkBoxWSeleAll_CheckedChanged(object sender, EventArgs e)
        {
            if (isCrossPage)
                foreach (DataGridViewRow row in dgvHistoryRecord.Rows)
                {
                    row.Cells["aa"].Value = checkBoxWSeleAll.Checked;
                    long selectId = long.Parse(row.Cells["Id"].Value.ToString());

                    if (checkBoxWSeleAll.Checked && !selectLong.Contains(selectId)) selectLong.Add(selectId);
                    else if (!checkBoxWSeleAll.Checked && selectLong.Contains(selectId)) selectLong.Remove(selectId);
                }
        }
        public string CreateStasticBitmap(DataGridViewW DataGridViewW2, Workbook workbook)
        {
            string strFilePath = string.Empty;// = "C:\\temp.bitmap";
            Bitmap ss = new Bitmap(600, 320);
            ZedGraphControl zgc = new ZedGraphControl();
            zgc.Size = new Size(600, 320);
            GraphPane myPane = zgc.GraphPane;
            myPane.IsAlignGrids = true;
            myPane.Title.Text = Info.Thick + "/" + Info.strAreaDensity + "_" + Info.StatisticsInfo;
            myPane.Legend.IsHStack = false;
            myPane.Legend.Position = LegendPos.Right;
            myPane.Border = new ZedGraph.Border(true, Color.Black, 1f);
            myPane.YAxis.Title.Text = "";
            myPane.XAxis.Title.Text = "";
            myPane.Margin.Top = 30;
            myPane.Margin.Right = 10;
            Color[] colors = { Color.Green, Color.Brown, Color.Yellow, Color.Pink, Color.Red };
            int colorIndex = 0;
            for (int i = 0; i < DataGridViewW2.ColumnCount; i++)
            {
                if (!DataGridViewW2.Columns[i].Visible
                    || (!DataGridViewW2.Columns[i].HeaderText.Contains(Info.Thick) && !DataGridViewW2.Columns[i].HeaderText.Contains(Info.strAreaDensity)))//只导出厚度
                    continue;
                PointPairList list = new PointPairList();
                for (int j = 0; j < DataGridViewW2.RowCount - 5; j++)
                {
                    string sinfo = DataGridViewW2.Rows[j].Cells[i].Value.ToString();
                    if (sinfo.Contains("--"))
                        continue;
                    string[] values = sinfo.Split('(');
                    list.Add(j, Convert.ToDouble(values[0]));
                }
                Color colorTemp = colors[colorIndex++];
                LineItem myCurve = myPane.AddCurve(DataGridViewW2.Columns[i].HeaderText, list, colorTemp, SymbolType.Default);
                myCurve.Line.Fill = new Fill(Color.Transparent, Color.Transparent, 45F);
                myCurve.Symbol.Fill = new Fill(colorTemp);
            }
            myPane.Chart.Fill = new Fill(Color.Gray, Color.Transparent, 45F);
            myPane.Fill = new Fill(Color.Transparent, Color.Transparent, 45F);
            zgc.AxisChange();
            zgc.DrawToBitmap(ss, new Rectangle(0, 0, 600, 320));
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                ss.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                //ss.Save(ms, Image.RawFormat);  
                //byte[] byteImage = new Byte[ms.Length];  
                //byteImage = ms.ToArray();
                workbook.Worksheets[0].Pictures.Add(DataGridViewW2.RowCount + 1, 0, ms);
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            finally
            {
                ms.Close();
            }
            //ss.Save(strFilePath);
            return strFilePath;

        }

        private void btnExportSQL_Click(object sender, EventArgs e)
        {
            if (selectLong.Count <= 0)
            {

                DialogResult dlgResult = Msg.Show(Info.DataBaseBackUP, Info.BackUpAndRestore, MessageBoxButtons.OKCancel);
                if (dlgResult == DialogResult.No || dlgResult == DialogResult.Cancel)
                {
                    return;
                }
            }
            System.Data.SqlClient.SqlConnection SqlConn = ConnectToSQLDB();

            if (SqlConn == null || SqlConn.State != ConnectionState.Open)
                return;
            try
            {
                if (selectLong.Count <= 0 && ClearDateBaseSQL(SqlConn))
                {
                    ExportSpecToSQL(SqlConn);
                    ExportHistoryToSQL(SqlConn);
                }
                else if (selectLong.Count > 0 && CheckDateBaseSQL(SqlConn))
                {
                    ExportSelHistoryToSQL(SqlConn);
                }
            }
            catch (Exception ex)
            {
                Msg.Show(ex.Message);
            }

            // 调用sqlcmd   
            // ProcessStartInfo info = new ProcessStartInfo("sqlcmd", @" -S .\MSSQLSERVER -i " + strPath);  
            ////禁用OS Shell   
            // info.UseShellExecute = false;  
            ////禁止弹出新窗口   
            //info.CreateNoWindow = true;  
            // //隐藏windows style   
            //info.WindowStyle = ProcessWindowStyle.Hidden;  
            ////标准输出   
            //info.RedirectStandardOutput = true;  
            //Process proc = new Process();  
            //proc.StartInfo = info;  
            ////启动进程   
            //proc.Start();  
            if (SqlConn.State == ConnectionState.Open)
                SqlConn.Close();
            Msg.Show(Info.strSpecifications);
        }

        private System.Data.SqlClient.SqlConnection ConnectToSQLDB()
        {
            if (!WorkCurveHelper.IsDBOpen)
                return null;
            System.Data.SqlClient.SqlConnection SqlConn = new System.Data.SqlClient.SqlConnection();
            string strPath = Application.StartupPath + "\\DBConnection.ini";
            string contstr = string.Empty;
            System.Text.StringBuilder tempbuilder = new System.Text.StringBuilder(255);
            string strDataSource = string.Empty;
            Skyray.API.WinMethod.GetPrivateProfileString("Param", "DataSource", "Local", tempbuilder, 255, strPath);
            strDataSource = tempbuilder.ToString();
            contstr += "Server=" + strDataSource + ";";

            Skyray.API.WinMethod.GetPrivateProfileString("Param", "InitialCatalog", "EDXRFDB", tempbuilder, 255, strPath);
            contstr += "Initial Catalog=" + tempbuilder.ToString() + ";";

            Skyray.API.WinMethod.GetPrivateProfileString("Param", "UserID", "sa", tempbuilder, 255, strPath);
            contstr += "uid=" + tempbuilder.ToString() + ";";

            Skyray.API.WinMethod.GetPrivateProfileString("Param", "password", "", tempbuilder, 255, strPath);
            contstr += "pwd=" + tempbuilder.ToString();
            try
            {
                SqlConn.ConnectionString = contstr;
                SqlConn.Open();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show("无法连接数据库");
                return null;
            }
            return SqlConn;
        }

        private void ExportSpecToSQL(System.Data.SqlClient.SqlConnection SqlCon)
        {

            try
            {
                if (selectLong.Count <= 0)
                {
                    List<SpecListEntity> list = WorkCurveHelper.DataAccess.GetAllSpectrum();
                    int SpecListCount = list.Count;
                    for (int i = 0; i < SpecListCount; i++)
                    {
                        string fileNameFull = WorkCurveHelper.SaveSamplePath + "\\" + list[i].Name + ".jpg";
                        byte[] photo = GetPhoto(fileNameFull);
                        int specount = list[i].Specs.Length;
                        string StrSql = "insert into SpecList(Name,SpecLength,SpecCount,SampleName,SpecDate,Photo) Values(@Name,@SpecLength,@SpecCount,@SampleName,@SpecDate,@Photo)";
                        System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                        cmd.Connection = SqlCon;
                        cmd.CommandText = StrSql;
                        cmd.Parameters.Add("@Name", list[i].Name);
                        cmd.Parameters.Add("@SpecLength", WorkCurveHelper.DeviceCurrent.SpecLength);
                        cmd.Parameters.Add("@SpecCount", specount);
                        cmd.Parameters.Add("@SampleName", list[i].SampleName);
                        cmd.Parameters.Add("@SpecDate", list[i].SpecDate);
                        cmd.Parameters.Add("@Photo", SqlDbType.Image, photo.Length).Value = photo;
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        for (int j = 0; j < specount; j++)
                        {
                            StrSql = "DECLARE @cnt int; Select @cnt =max(id) from SpecList where SpecList.Name=@Name1;insert into Spec(Name,SpecList_ID,Spec_ID,UsedTime,Voltage,[Current],SpecData) Values(@Name,@cnt,@Spec_ID,@UsedTime,@Voltage,@Current,";
                            StrSql += "'" + list[i].Specs[j].SpecData + "')";
                            cmd.CommandText = StrSql;
                            cmd.Parameters.Add("@Name", list[i].Specs[j].Name);
                            cmd.Parameters.Add("@Name1", list[i].Name);
                            cmd.Parameters.Add("@Spec_ID", j);
                            cmd.Parameters.Add("@UsedTime", list[i].Specs[j].UsedTime);
                            cmd.Parameters.Add("@Voltage", list[i].Specs[j].TubVoltage);
                            cmd.Parameters.Add("@Current", list[i].Specs[j].TubCurrent);
                            // cmd.Parameters.Add("@SpecData", SqlDbType.Text, list[i].Specs[j].SpecData.Length).Value = list[i].Specs[j].SpecData;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ExportHistoryToSQL(System.Data.SqlClient.SqlConnection SqlCon)
        {
            try
            {
                string sql = "select * from WorkCurve a inner join Condition b on a.Condition_Id = b.Id inner join Device c on b.Device_Id=c.Id where c.Id=" + WorkCurveHelper.DeviceCurrent.Id;
                List<WorkCurve> listCur = WorkCurve.FindBySql(sql + " and b.Type=0 ");
                int CountCur = listCur.Count;
                for (int curi = 0; curi < CountCur; curi++)
                {
                    List<HistoryRecord> list = HistoryRecord.FindBySql("select * from historyrecord where WorkCurveId=" + listCur[curi].Id);
                    if (listCur[curi].ElementList == null || listCur[curi].ElementList.Items.Count <= 0)
                        continue;
                    for (int i = 0; i < list.Count; i++)
                    {
                        string StrSql = "DECLARE @SpecList_Id int;Select @SpecList_Id =max(id) from SpecList Where Name= @Name ; insert into History(SpecList_Id,CurveName,Layers) Values(@SpecList_Id,@CurveName,@Layers);";
                        System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                        cmd.Connection = SqlCon;
                        cmd.CommandText = StrSql;
                        cmd.Parameters.Add("@Name", list[i].SpecListName);
                        cmd.Parameters.Add("@CurveName", listCur[curi].Name);
                        int layersCount = listCur[curi].ElementList.Items.OrderByDescending(w => w.LayerNumber).ToList()[0].LayerNumber;
                        cmd.Parameters.Add("@Layers", layersCount);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        StrSql = "DECLARE @History_Id int;Select @History_Id =max(id) from History; ";
                        for (int j = 0; j < listCur[curi].ElementList.Items.Count; j++)
                        {
                            StrSql += "insert into ElemContent(History_Id,ElementName,Content,ContentLayer,ContentUnit) Values(@History_Id,@ElementName" + j + ",@Content" + j + ",@ContentLayer" + j + ",@ContentUnit" + j + ");";
                            string element = listCur[curi].ElementList.Items[j].Caption;
                            cmd.Parameters.Add("@ElementName" + j, element);
                            cmd.Parameters.Add("@ContentLayer" + j, listCur[curi].ElementList.Items[j].LayerNumber);
                            string strContent = list[i].HistoryElement.First(w => w.elementName == element) != null ? list[i].HistoryElement.First(w => w.elementName == element).contextelementValue : "0";
                            cmd.Parameters.Add("@Content" + j, float.Parse(strContent));
                            int contentUnit = list[i].HistoryElement.First(w => w.elementName == element) != null ? list[i].HistoryElement.First(w => w.elementName == element).unitValue : 1;
                            string strContentUnit = contentUnit == 1 ? "%" : (contentUnit == 2 ? "ppm" : "‰");
                            cmd.Parameters.Add("@ContentUnit" + j, strContentUnit);
                        }
                        for (int j = 1; j < layersCount; j++)
                        {
                            StrSql += "insert into LayerThick(History_Id,Layer,Thickness,ThickUnit) Values(@History_Id,@Layer" + j + ",@Thickness" + j + ",@ThickUnit" + j + ");";
                            string element = listCur[curi].ElementList.Items.First(w => w.LayerNumber == j).Caption;
                            cmd.Parameters.Add("@Layer" + j, j);
                            cmd.Parameters.Add("@Thickness" + j, list[i].HistoryElement.First(w => w.elementName == element) != null ? float.Parse(list[i].HistoryElement.First(w => w.elementName == element).thickelementValue) : 0);
                            int thickUnit = list[i].HistoryElement.First(w => w.elementName == element) != null ? list[i].HistoryElement.First(w => w.elementName == element).thickunitValue : 1;
                            //string strthickUnit = thickUnit == 1 ? "u〞" : "um";
                            string strthickUnit = thickUnit == 1 ? "u〞" : (thickUnit == 2 ? "um" : "g/L");
                            cmd.Parameters.Add("@ThickUnit" + j, strthickUnit);
                        }
                        cmd.CommandText = StrSql;
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                    }
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool ClearDateBaseSQL(System.Data.SqlClient.SqlConnection SqlCon)
        {
            string strSql = "SET XACT_ABORT on;begin transaction;";
            strSql += "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Spec]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[Spec]; ";
            strSql += " CREATE TABLE [dbo].[Spec] ([id] [int] IDENTITY (1, 1) NOT NULL ,[SpecList_ID] [int] NOT NULL ,[Spec_ID] [int] NOT NULL ,[Name] [varchar] (100) COLLATE Chinese_PRC_CI_AS NULL ,";
            strSql += "[UsedTime] [float] NOT NULL ,[Voltage] [float] NULL ,[Current] [float] NULL ,[SpecData] [text] COLLATE Chinese_PRC_CI_AS NULL) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];";

            strSql += "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SpecList]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[SpecList]; ";
            strSql += "CREATE TABLE [dbo].[SpecList] ([Id] [int] IDENTITY (1, 1) NOT NULL ,[Name] [varchar] (100) COLLATE Chinese_PRC_CI_AS NOT NULL ,[SpecLength] [int] NOT NULL ,[SpecCount] [int] NOT NULL ,";
            strSql += "[SampleName] [varchar] (100) COLLATE Chinese_PRC_CI_AS NULL ,[SpecDate] [datetime] NULL ,[Photo] [image] NULL ,[Description] [varchar] (100) COLLATE Chinese_PRC_CI_AS NULL) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];";


            strSql += "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[History]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[History]; ";
            strSql += "CREATE TABLE [dbo].[History] ([Id] [int] IDENTITY (1, 1) NOT NULL ,[SpecList_Id] [int] NULL ,[CurveName] [varchar] (100) COLLATE Chinese_PRC_CI_AS NULL ,[Layers] [int] NULL) ON [PRIMARY];";


            strSql += "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LayerThick]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[LayerThick]";
            strSql += "CREATE TABLE [dbo].[LayerThick] ( [id] [int] IDENTITY (1, 1) NOT NULL ,[History_id] [int] NOT NULL ,[Layer] [int] NOT NULL ,[Thickness] [float] NOT NULL ,[Density] [float] NULL,[ThickUnit] [varchar] (10) COLLATE Chinese_PRC_CI_AS NULL ) ON [PRIMARY];";


            strSql += "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ElemContent]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[ElemContent]";
            strSql += "CREATE TABLE [dbo].[ElemContent] ([Id] [int] IDENTITY (1, 1) NOT NULL ,[History_id] [int] NOT NULL ,[ElementName] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,";
            strSql += "[Content] [float] NULL ,[ContentUnit] [varchar] (10) COLLATE Chinese_PRC_CI_AS NULL ,[ContentLayer] [int] NULL) ON [PRIMARY];";
            strSql += "commit transaction;";
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            cmd.Connection = SqlCon;
            try
            {
                cmd.CommandText = strSql;
                cmd.ExecuteNonQuery();
                return true;

            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

        }

        //检查当前连接的数据库中是否已经创建即将写入数据的表

        private bool CheckDateBaseSQL(System.Data.SqlClient.SqlConnection SqlCon)
        {
            if (!WorkCurveHelper.IsDBOpen)
                return false;

            string strSql = "";

            //string strSql = "SET XACT_ABORT on;begin transaction;";


            //strSql += "if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Spec]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) ";
            //strSql += "CREATE TABLE [dbo].[Spec] ([id] [int] IDENTITY (1, 1) NOT NULL ,[SpecList_ID] [int] NOT NULL ,[Spec_ID] [int] NOT NULL ,[Name] [varchar] (100) COLLATE Chinese_PRC_CI_AS NULL ,";
            //strSql += "[UsedTime] [float] NOT NULL ,[Voltage] [float] NULL ,[Current] [float] NULL ,[SpecData] [text] COLLATE Chinese_PRC_CI_AS NULL) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];";

            //strSql += "if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SpecList]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) ";
            //strSql += "CREATE TABLE [dbo].[SpecList] ([Id] [int] IDENTITY (1, 1) NOT NULL ,[Name] [varchar] (100) COLLATE Chinese_PRC_CI_AS NOT NULL ,[SpecLength] [int] NOT NULL ,[SpecCount] [int] NOT NULL ,";
            //strSql += "[SampleName] [varchar] (100) COLLATE Chinese_PRC_CI_AS NULL ,[SpecDate] [datetime] NULL ,[Photo] [image] NULL ,[Description] [varchar] (100) COLLATE Chinese_PRC_CI_AS NULL) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];";


            //strSql += "if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[History]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) ";
            //strSql += "CREATE TABLE [dbo].[History] ([Id] [int] IDENTITY (1, 1) NOT NULL ,[SpecList_Id] [int] NULL ,[CurveName] [varchar] (100) COLLATE Chinese_PRC_CI_AS NULL ,[Layers] [int] NULL) ON [PRIMARY];";


            //strSql += "if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LayerThick]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) ";
            //strSql += "CREATE TABLE [dbo].[LayerThick] ( [id] [int] IDENTITY (1, 1) NOT NULL ,[History_id] [int] NOT NULL ,[Layer] [int] NOT NULL ,[Thickness] [float] NOT NULL ,[Density] [float] NULL,[ThickUnit] [varchar] (100) COLLATE Chinese_PRC_CI_AS NULL ) ON [PRIMARY];";


            //strSql += "if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ElemContent]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) ";
            //strSql += "CREATE TABLE [dbo].[ElemContent] ([Id] [int] IDENTITY (1, 1) NOT NULL ,[History_id] [int] NOT NULL ,[ElementName] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,";
            //strSql += "[Content] [float] NULL ,[ContentUnit] [varchar] (100) COLLATE Chinese_PRC_CI_AS NULL ,[ContentLayer] [int] NULL) ON [PRIMARY];";
            //strSql += "commit transaction;";

            //检查是否有TestRecord表

            strSql += "if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TestRecord]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) ";
            strSql += @"

                        CREATE TABLE [dbo].[TestRecord](
	                        [Id] [int] NOT NULL,
	                        [Name] [varchar](100) NOT NULL,
	                        [RecordCord] [varchar](100) ,
	                        [InspectionCode] [varchar](100),
	                        [WorkCurveName] [varchar](100) NOT NULL,
	                        [DeviceName] [varchar](100) NOT NULL,
	                        [SpecDate] [datetime] NOT NULL,
	                        [CountRate] [float] NOT NULL,
	                        [PeakChannel] [float] NOT NULL,
	                        [TubVoltage] [float] NOT NULL,
	                        [TubCurrent] [float] NOT NULL,
                         CONSTRAINT [PK_TestRecord] PRIMARY KEY CLUSTERED 
                        (
	                        [Id] ASC
                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                        ) ON [PRIMARY]

                   
                        ;";

            //检查是否有TestElement表

            strSql += "if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TestElement]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) ";
            strSql += @"
                       begin

                        CREATE TABLE [dbo].[TestElement](
	                        [Id] [int] IDENTITY(1,1) NOT NULL,
	                        [TestRecord_Id] [int] NOT NULL,
	                        [ElementName] [varchar](50) NOT NULL,
	                        [ElementValue] [float] NOT NULL,
                         CONSTRAINT [PK_TestElement] PRIMARY KEY CLUSTERED 
                        (
	                        [Id] ASC
                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                        ) ON [PRIMARY] ";


            strSql += @"
                        ALTER TABLE [dbo].[TestElement]  WITH CHECK ADD  CONSTRAINT [FK_TestElement_TestRecord] FOREIGN KEY([TestRecord_Id])
                        REFERENCES [dbo].[TestRecord] ([Id])
                        ON UPDATE CASCADE
                        ON DELETE CASCADE 

                        
                        end; ";





            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            cmd.Connection = SqlCon;
            try
            {
                cmd.CommandText = strSql;
                cmd.ExecuteNonQuery();
                return true;

            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show("无法创建数据表");
                return false;
            }

        }



        private bool ExportSelHistoryToSQL(System.Data.SqlClient.SqlConnection SqlCon)
        {
            if (!WorkCurveHelper.IsDBOpen)
                return false;

            string strSql = "";

            //此处先获取样品编号以及送检编号在sqlite中的id
            long sampleId = 0;
            long checkId = 0;
            try
            {

                sampleId = CompanyOthersInfo.FindBySql("select * from CompanyOthersInfo a where  a.Name = '" + "样品编号" + "' ")[0].Id;
                checkId = CompanyOthersInfo.FindBySql("select * from CompanyOthersInfo a where  a.Name = '" + "送检编号" + "' ")[0].Id;
            }
            catch
            {
                Msg.Show("请先添加样品编号以及送检编号信息");
                return false;
            }

            for (int i = 0; i < selectLong.Count; i++)
            {

                HistoryRecord hd = HistoryRecord.FindById(selectLong[i]);
                List<HistoryElement> he = HistoryElement.FindBySql("select * from HistoryElement where Historyrecord_Id=" + hd.Id);
                WorkCurve wk = WorkCurve.FindById(hd.WorkCurveId);
                string[] eleNames = wk.Name.Split('-').RemoveLast();

                for (int j = 0; j < he.Count(); j++)
                {
                    if (!eleNames.Contains(he[j].elementName))
                    {
                        he[j] = null;

                    }
                }

                //此处得到了样品的实际编号以及送检的实际编号


                string sampleInfo = "";
                List<HistoryCompanyOtherInfo> temp1 = HistoryCompanyOtherInfo.FindBySql("select * from HistoryCompanyOtherInfo where History_Id = '" + selectLong[i].ToString() + "' and CompanyOthersInfo_Id = '" + sampleId.ToString() + "'");
                if (temp1.Count > 0)
                    sampleInfo = temp1[0].ListInfo;

                string checkInfo = "";

                List<HistoryCompanyOtherInfo> temp2 = HistoryCompanyOtherInfo.FindBySql("select * from HistoryCompanyOtherInfo where History_Id = '" + selectLong[i].ToString() + "' and CompanyOthersInfo_Id = '" + checkId.ToString() + "'");
                if (temp2.Count > 0)
                    checkInfo = temp2[0].ListInfo;






                strSql += string.Format("if not exists (select * from TestRecord where Name = '{0}' and DeviceName = '{1}' and  WorkCurveName = '{2}' ) ", hd.SampleName, hd.DeviceName, wk.Name);

                if (sampleInfo != "" && checkInfo != "")
                {

                    strSql += string.Format(@"
                                INSERT INTO [dbo].[TestRecord]
                                   ([Id]
                                   ,[Name]
                                   ,[RecordCord]
                                   ,[InspectionCode]
                                   ,[WorkCurveName]
                                   ,[DeviceName]
                                   ,[SpecDate]
                                   ,[CountRate]
                                   ,[PeakChannel]
                                   ,[TubVoltage]
                                   ,[TubCurrent])
                             VALUES
                                   ({0}
                                   ,'{1}'
                                   ,'{2}'
                                   ,'{3}'
                                   ,'{4}'
                                   ,'{5}'
                                   ,'{6}'
                                   ,{7}
                                   ,{8}
                                   ,{9}
                                   ,{10})

                              ;", selectLong[i], hd.SampleName, sampleInfo, checkInfo, wk.Name, hd.DeviceName, hd.SpecDate, hd.CountRate, hd.PeakChannel, hd.ActualVoltage, hd.ActualCurrent);

                }

                else
                {
                    strSql += string.Format(@"
                                INSERT INTO [dbo].[TestRecord]
                                   ([Id]
                                   ,[Name]
                                  
                                   ,[WorkCurveName]
                                   ,[DeviceName]
                                   ,[SpecDate]
                                   ,[CountRate]
                                   ,[PeakChannel]
                                   ,[TubVoltage]
                                   ,[TubCurrent])
                             VALUES
                                   ({0}
                                   ,'{1}'
                                  
                              
                                   ,'{2}'
                                   ,'{3}'
                                   ,'{4}'
                                   ,{5}
                                   ,{6}
                                   ,{7}
                                   ,{8})

                              ;", selectLong[i], hd.SampleName, wk.Name, hd.DeviceName, hd.SpecDate, hd.CountRate, hd.PeakChannel, hd.ActualVoltage, hd.ActualCurrent);


                }

                strSql += string.Format("if  exists (select * from TestRecord where Name = '{0}' and DeviceName = '{1}' and  WorkCurveName = '{2}' ) ", hd.SampleName, hd.DeviceName, wk.Name);

                if (sampleInfo != "" && checkInfo != "")
                {

                    strSql += string.Format(@"
                                UPDATE [dbo].[TestRecord]
                               SET [Id] = {0}
                                  ,[Name] = '{1}'
                                  ,[RecordCord] = '{2}'
                                  ,[InspectionCode] = '{3}'
                                  ,[WorkCurveName] = '{4}'
                                  ,[DeviceName] = '{5}'
                                  ,[SpecDate] = '{6}'
                                  ,[CountRate] = {7}
                                  ,[PeakChannel] = {8}
                                  ,[TubVoltage] = {9}
                                  ,[TubCurrent] = {10}
                             WHERE 
                                  Name = '{11}' and DeviceName = '{12}' and WorkCurveName = '{13}'

                              ;", selectLong[i], hd.SampleName, sampleInfo, checkInfo, wk.Name, hd.DeviceName, hd.SpecDate, hd.CountRate, hd.PeakChannel, hd.ActualVoltage, hd.ActualCurrent, hd.SampleName, hd.DeviceName, wk.Name);


                }

                else
                {

                    strSql += string.Format(@"
                                UPDATE [dbo].[TestRecord]
                               SET [Id] = {0}

                                  ,[Name] = '{1}'
                                  
                                  ,[WorkCurveName] = '{2}'
                                  ,[DeviceName] = '{3}'
                                  ,[SpecDate] = '{4}'
                                  ,[CountRate] = {5}
                                  ,[PeakChannel] = {6}
                                  ,[TubVoltage] = {7}
                                  ,[TubCurrent] = {8}
                             WHERE 
                                  Name = '{9}' and DeviceName = '{10}' and WorkCurveName = '{11}'

                              ;", selectLong[i], hd.SampleName, wk.Name, hd.DeviceName, hd.SpecDate, hd.CountRate, hd.PeakChannel, hd.ActualVoltage, hd.ActualCurrent, hd.SampleName, hd.DeviceName, wk.Name);


                }

                for (int k = 0; k < he.Count; k++)
                {
                    if (he[k] != null)
                    {
                        strSql += string.Format("if not exists (select * from TestElement where TestRecord_Id = {0} and  ElementName = '{1}' ) ", selectLong[i], he[k].elementName);

                        strSql += string.Format(@"
                                        INSERT INTO [dbo].[TestElement]
                                       (
                                       [TestRecord_Id]
                                       ,[ElementName]
                                       ,[ElementValue])
                                 VALUES
                                       (
                                       {0}
                                       ,'{1}'
                                       ,{2})
                                      ;", selectLong[i], he[k].elementName, he[k].thickelementValue);

                        strSql += string.Format("if  exists (select * from TestElement where TestRecord_Id = {0} and  ElementName = '{1}' ) ", selectLong[i], he[k].elementName);

                        strSql += string.Format(@"
                               UPDATE [dbo].[TestElement]
                               SET 
                                  [ElementValue] = {0}
                                WHERE
                                    TestRecord_Id = {1} and ElementName = '{2}'
                                     ;", he[k].thickelementValue, selectLong[i], he[k].elementName);


                    }

                }






            }




            try
            {


                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = SqlCon;
                cmd.CommandText = strSql;
                cmd.ExecuteNonQuery();
                return true;


            }


         /*
             string fileNameFull = WorkCurveHelper.SaveSamplePath + "\\" + hd.SpecListName + ".jpg";
             byte[] photo = GetPhoto(fileNameFull);
             SpecListEntity se = WorkCurveHelper.DataAccess.GetSpecList("'" + hd.SpecListName + "'").Count > 0 ? WorkCurveHelper.DataAccess.GetSpecList("'" + hd.SpecListName + "'")[0] : null;
             if (se == null)
                 continue;
             int specount = se.Specs.Length;
             string StrSql = "if not exists(select * from SpecList where Name= @Name) ";
             StrSql += "insert into SpecList(Name,SpecLength,SpecCount,SampleName,SpecDate,Photo) Values(@Name,@SpecLength,@SpecCount,@SampleName,@SpecDate,@Photo) ";
             StrSql += " else Update SpecList set SpecLength= @SpecLength,SpecCount=@SpecCount,SampleName=@SampleName,SpecDate=@SpecDate,Photo=@Photo Where Name=@Name;";
             System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
             cmd.Connection = SqlCon;
             cmd.CommandText = StrSql;
             cmd.Parameters.Add("@Name", se.Name);
             cmd.Parameters.Add("@SpecLength", WorkCurveHelper.DeviceCurrent.SpecLength);
             cmd.Parameters.Add("@SpecCount", specount);
             cmd.Parameters.Add("@SampleName", se.SampleName);
             cmd.Parameters.Add("@SpecDate", se.SpecDate);
             cmd.Parameters.Add("@Photo", SqlDbType.Image, photo.Length).Value = photo;
             cmd.ExecuteNonQuery();
             cmd.Parameters.Clear();
             StrSql = "DECLARE @cnt int; Select @cnt =max(id) from SpecList where SpecList.Name='"+se.Name+"';delete  from Spec Where SpecList_ID=@cnt;";
             cmd.CommandText = StrSql;
             cmd.ExecuteNonQuery();
             for (int j = 0; j < specount; j++)
             {
                 StrSql = "DECLARE @cnt int; Select @cnt =max(id) from SpecList where SpecList.Name=@Name1;";
                 StrSql+="insert into Spec(Name,SpecList_ID,Spec_ID,UsedTime,Voltage,[Current],SpecData) Values(@Name,@cnt,@Spec_ID,@UsedTime,@Voltage,@Current,";
                 StrSql += "'" + se.Specs[j].SpecData + "')";
                 cmd.CommandText = StrSql;
                 cmd.Parameters.Add("@Name", se.Specs[j].Name);
                 cmd.Parameters.Add("@Name1", se.Name);
                 cmd.Parameters.Add("@Spec_ID", j);
                 cmd.Parameters.Add("@UsedTime", se.Specs[j].UsedTime);
                 cmd.Parameters.Add("@Voltage", se.Specs[j].TubVoltage);
                 cmd.Parameters.Add("@Current", se.Specs[j].TubCurrent);
                 // cmd.Parameters.Add("@SpecData", SqlDbType.Text, list[i].Specs[j].SpecData.Length).Value = list[i].Specs[j].SpecData;
                 cmd.ExecuteNonQuery();
                 cmd.Parameters.Clear();
             }
             //插入或更新历史记录数据
             WorkCurve wc = WorkCurve.FindById(hd.WorkCurveId);
             StrSql = "DECLARE @SpecList_Id int;Select @SpecList_Id =max(id) from SpecList Where Name= @Name ;";
             StrSql+="if not  exists(select * from History Where SpecList_Id = @SpecList_Id and CurveName= @CurveName)";
             StrSql+=" insert into History(SpecList_Id,CurveName,Layers) Values(@SpecList_Id,@CurveName,@Layers)";
             StrSql+=" else Update History set Layers=@Layers Where SpecList_Id = @SpecList_Id and CurveName= @CurveName;";
             cmd.Connection = SqlCon;
             cmd.CommandText = StrSql;
             cmd.Parameters.Add("@Name", hd.SpecListName);
             cmd.Parameters.Add("@CurveName", wc.Name);
             int layersCount = wc.ElementList.Items.OrderByDescending(w => w.LayerNumber).ToList()[0].LayerNumber;
             cmd.Parameters.Add("@Layers", layersCount);
             cmd.ExecuteNonQuery();
             cmd.Parameters.Clear();
             StrSql = "DECLARE @History_Id int;Select @History_Id =max(a.id) from History a ,SpecList b where a.SpecList_Id=b.ID and b.Name=@Name;";
             StrSql+=" delete from ElemContent where History_Id=@History_Id ;delete from LayerThick where History_Id=@History_Id ;";
             cmd.Parameters.Add("@Name", hd.SpecListName);
             cmd.CommandText = StrSql;
             cmd.ExecuteNonQuery();
             cmd.Parameters.Clear();
             StrSql = "DECLARE @History_Id int;Select @History_Id =max(a.id) from History a ,SpecList b where a.SpecList_Id=b.ID and b.Name=@Name;";
             cmd.Parameters.Add("@Name", hd.SpecListName);
             for (int j = 0; j < wc.ElementList.Items.Count; j++)
             {
                 StrSql += "insert into ElemContent(History_Id,ElementName,Content,ContentLayer,ContentUnit) Values(@History_Id,@ElementName" + j + ",@Content" + j + ",@ContentLayer" + j + ",@ContentUnit" + j + ");";
                 string element = wc.ElementList.Items[j].Caption;
                 cmd.Parameters.Add("@ElementName" + j, element);
                 cmd.Parameters.Add("@ContentLayer" + j, wc.ElementList.Items[j].LayerNumber);
                 string strContent = hd.HistoryElement.First(w => w.elementName == element) != null ? hd.HistoryElement.First(w => w.elementName == element).contextelementValue : "0";
                 cmd.Parameters.Add("@Content" + j, float.Parse(strContent));
                 int contentUnit=hd.HistoryElement.First(w => w.elementName == element) != null ? hd.HistoryElement.First(w => w.elementName == element).unitValue : 1;
                 string strContentUnit=contentUnit==1?"%":(contentUnit==2?"ppm":"‰");
                 cmd.Parameters.Add("@ContentUnit" + j, strContentUnit);
             }
             for (int j = 1; j < layersCount; j++)
             {
                 StrSql += "insert into LayerThick(History_Id,Layer,Thickness) Values(@History_Id,@Layer" + j + ",@Thickness" + j + ",@ThickUnit" + j + ");";
                 string element = wc.ElementList.Items.First(w => w.LayerNumber == j).Caption;
                 cmd.Parameters.Add("@Layer" + j, j);
                 cmd.Parameters.Add("@Thickness" + j, hd.HistoryElement.First(w => w.elementName == element) != null ? float.Parse(hd.HistoryElement.First(w => w.elementName == element).thickelementValue) : 0);
                 int thickUnit = hd.HistoryElement.First(w => w.elementName == element) != null ? hd.HistoryElement.First(w => w.elementName == element).thickunitValue : 1;
                // string strthickUnit =thickUnit==1?"u〞" : "um";
                 string strthickUnit = thickUnit == 1 ? "u〞" : (thickUnit == 2 ? "um" : "g/L");
                           
                 cmd.Parameters.Add("@ThickUnit" + j, strthickUnit);
             }
             cmd.CommandText = StrSql;
             cmd.ExecuteNonQuery();
             cmd.Parameters.Clear();
         }
         return true;
     }
            
              */

            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public static byte[] GetPhoto(string filePath)
        {
            if (!File.Exists(filePath))
                return new byte[1];
            FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(stream);
            byte[] photoqq = reader.ReadBytes((int)stream.Length);
            reader.Close();
            stream.Close();
            return photoqq;
        }

        private void RefreshWeightInfo(ref DataTable t)
        {
            if (!t.Columns.Contains("Weight")) return;
            //double KaratTranslater = 99.995f;
            //string KaratTranslaterate = ReportTemplateHelper.LoadSpecifiedValue("OpenHistoryRecordType", "KaratTranslaterate");
            //try
            //{
            //    KaratTranslater = double.Parse(KaratTranslaterate);
            //}
            //catch
            //{
            //    KaratTranslater = 99.995;
            //}

            //@CYR180416
            string elementToCalc = "Au"; //默认为Au
            DataTable dtTemp = t.Copy();
            //if (WorkCurveHelper.WorkCurveCurrent != null
            //    && WorkCurveHelper.WorkCurveCurrent.ElementList != null
            //    )
            //{
            //    elementToCalc = WorkCurveHelper.WorkCurveCurrent.ElementList.MainElementToCalcKarat;
            //}


            for (int i = 0; i < t.Rows.Count; i++)
            {
                for (int j = 0; j < t.Columns.Count; j++)
                {
                    if (t.Columns[j].ColumnName.Contains(Info.IncludingAu))//更新k值
                    {
                        //if (!t.Columns.Contains("Au")) t.Rows[i][j] = "0.0K";
                        //else
                        //{
                        //    int length = t.Rows[i]["Au"].ToString().Trim().IndexOf('(') > 0 ? t.Rows[i]["Au"].ToString().Trim().IndexOf('(') : t.Rows[i]["Au"].ToString().Trim().Length;
                        //    t.Rows[i][j] = (double.Parse(t.Rows[i]["Au"].ToString().Trim().Substring(0, length)) * 24 / WorkCurveHelper.KaratTranslater).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(K)";
                        //}
                        var cur = lstCurves.Find(c => c.Name == dtTemp.Rows[i]["WorkCurveName"].ToString());
                        if (cur != null)
                        {
                            elementToCalc = cur.ElementList.MainElementToCalcKarat;
                        }
                        if (!t.Columns.Contains(elementToCalc)) t.Rows[i][j] = "0.0K";
                        else
                        {
                            int length = t.Rows[i][elementToCalc].ToString().Trim().IndexOf('(') > 0 ? t.Rows[i][elementToCalc].ToString().Trim().IndexOf('(') : t.Rows[i][elementToCalc].ToString().Trim().Length;
                            t.Rows[i][j] = (double.Parse(t.Rows[i][elementToCalc].ToString().Trim().Substring(0, length)) * 24 / WorkCurveHelper.KaratTranslater).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(K)";
                        }
                        continue;
                    }
                    if (!t.Columns[j].ColumnName.Contains("(" + Info.Weight + ")"))//更新重量
                        continue;
                    //string Elem=t.Columns[j].ColumnName.Replace("("+Info.Weight+")","").Trim();
                    //int length=t.Rows[i][Elem].ToString().Trim().IndexOf('(')>0?t.Rows[i][Elem].ToString().Trim().IndexOf('('):t.Rows[i][Elem].ToString().Trim().Length;
                    //t.Rows[i][j]=(double.Parse(t.Rows[i][Elem].ToString().Trim().Substring(0,length))*double.Parse(t.Rows[i][Info.Weight].ToString())).ToString("F"+WorkCurveHelper.SoftWareContentBit);
                    t.Rows[i][j] = (double.Parse(t.Rows[i][j].ToString().Replace("(g)", "").Trim()) / 100 * double.Parse(t.Rows[i]["Weight"].ToString())).ToString("F" + WorkCurveHelper.SoftWareContentBit) + "(g)";
                }
            }
        }

        private void chkPrintHistory_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPrintHistory.Checked) btwTemplateExcel.Text = Info.PrinHistory;
            else btwTemplateExcel.Text = Info.ExportHistory;
        }

        private void comboBoxReportType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #region SetHistoryRecordInfo Methods
        private bool LoadSetHistoryRecordInfo(ref Form frm)
        {
            try
            {
                string dllPath = Application.StartupPath + "\\Skyray.SetHistoryRecordInfo.dll";
                string xmlPath = Application.StartupPath + "\\ElementParams.xml";
                string className = "Skyray.SetHistoryRecordInfo.Form1";
                List<string> workRegionList = new List<string>();
                Dictionary<string, List<string>> dicRegionElements = new Dictionary<string, List<string>>();

                if (File.Exists(dllPath) && File.Exists(xmlPath))
                {
                    Assembly ass = Assembly.LoadFile(dllPath);
                    Type t = ass.GetType(className);
                    if (t != null)
                    {
                        frm = Activator.CreateInstance(t) as Form;
                        var listRegion = WorkRegion.FindBySql("select * from WorkRegion");
                        foreach (WorkRegion region in listRegion)
                        {
                            if (!workRegionList.Contains(region.Caption))
                                workRegionList.Add(region.Caption);
                            List<string> elementList = new List<string>();
                            HasMany<WorkCurve> workCurves = region.WorkCurves;
                            foreach (WorkCurve curve in workCurves)
                            {
                                if (curve.ElementList == null)
                                    continue;
                                HasMany<CurveElement> elements = curve.ElementList.Items;

                                foreach (CurveElement e in elements)
                                {
                                    if (!elementList.Contains(e.Caption))
                                        elementList.Add(e.Caption);
                                }
                            }

                            if (!dicRegionElements.ContainsKey(region.Caption))
                                dicRegionElements.Add(region.Caption, elementList);
                        }
                        FieldInfo fi = t.GetField("DataSourceDictionary");
                        if (fi != null)
                        {
                            fi.SetValue(frm, dicRegionElements);
                        }
                        else
                        {
                            return false;
                        }

                        FieldInfo fi2 = t.GetField("_currentWorkRegion", BindingFlags.Instance | BindingFlags.NonPublic);
                        if (fi2 != null)
                        {
                            fi2.SetValue(frm, WorkCurveHelper.CurrentWorkRegion.Caption);
                        }
                        else
                        {
                            return false;
                        }

                        FieldInfo fi3 = t.GetField("_dicLanguage", BindingFlags.Instance | BindingFlags.NonPublic);
                        if (fi3 != null)
                        {
                            fi3.SetValue(frm, new Dictionary<string, string>()
                            {
                                
                                {"lblHeader.Text", Info.Shri_Setting},

                                {"grpSetRangeColor.Text", Info.Shri_SetRangeColor},
                                {"btnQuery.Text", Info.Shri_Query},
                                {"btnModify.Text", Info.Shri_Modify},
                                {"btnAdd.Text", Info.Shri_Add},
                                {"btnDelete.Text", Info.Shri_Delete},
                                {"btnCancel.Text", Info.Shri_Cancel},

                                {"grpAddColumns.Text", Info.Shri_AddColumns},
                                {"btnQuery2.Text", Info.Shri_Query},
                                {"btnAdd2.Text", Info.Shri_Add},
                                {"btnDelete2.Text", Info.Shri_Delete},
                                {"btnCancel2.Text", Info.Shri_Cancel},

                                {"btnSave.Text", Info.Shri_Save},

                                {"dataGridView1.WorkRegion", Info.Shri_WorkRegion},
                                {"dataGridView1.Element", Info.Shri_Element},
                                {"dataGridView1.MinValue", Info.Shri_MinValue},
                                {"dataGridView1.MaxValue", Info.Shri_MaxValue},
                                {"dataGridView1.FillColor", Info.Shri_FillColor},
                                {"dataGridView1.TextColor", Info.Shri_TextColor},
                                {"dataGridView1.Content", Info.Shri_Content},
                                {"dataGridView2.Name", Info.Shri_Name},
                                {"dataGridView2.Value", Info.Shri_Value},
                                {"dataGridView2.Position", Info.Shri_Position}
                            });
                        }
                        else
                        {
                            return false;
                        }
                        return true;
                    }

                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        private void btnSetHistoryRecordInfo_Click(object sender, EventArgs e)
        {
            Form frm = new Form();
            isXmlLoaded = LoadSetHistoryRecordInfo(ref frm);
            if (isXmlLoaded)
                frm.ShowDialog();
            else
            {
                MessageBox.Show(Info.Shri_LoadXmlFailed);
            }
        }
        #endregion

        private void dgvHistoryRecord_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!DifferenceDevice.IsAnalyser)
                return;
            if (e.ColumnIndex <= 0 || e.RowIndex < 0)
                return;
            long id = 0;
            long.TryParse(dgvHistoryRecord.Rows[e.RowIndex].Cells["ID"].Value.ToString(), out id);
            HistoryRecord rd = HistoryRecord.FindById(id);
            if (rd == null)
                return;
            SpecListEntity tmpList = DataBaseHelper.QueryByEdition(rd.SpecListName, rd.FilePath, rd.EditionType);
            if (tmpList == null)
                return;
            WorkCurveHelper.IsOpenSpecByHistoryRecord = true;
            DifferenceDevice.interClassMain.OpenWorkSpec(new List<SpecListEntity>() { tmpList }, true);
            WorkCurveHelper.IsOpenSpecByHistoryRecord = false;
        }

  

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonW1_Click(object sender, EventArgs e)
        {
            FrmDeleteSpec deleteSpec = new FrmDeleteSpec();
            deleteSpec.ShowDialog();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }



        public void CreateContructStatis(ElementList elementList)
        {

            this.staShow.dgvSta.Columns.Clear();
            this.staShow.dgvSta.Rows.Clear();
            if (elementList == null || elementList.Items.Count == 0)
                return;
            DataGridViewTextBoxColumn elemColumns = new DataGridViewTextBoxColumn();
            elemColumns.Name = "statColumns".ToString();
            elemColumns.HeaderText = Info.ElementName;
            elemColumns.ReadOnly = true;
            elemColumns.SortMode = DataGridViewColumnSortMode.NotSortable;
            elemColumns.Width = 80;
            this.staShow.dgvSta.Columns.Add(elemColumns);

            string[] staticsHeadText = { Info.MaxValue, Info.MinValue, Info.MeanValue, Info.SDValue };
            elemColumns = new DataGridViewTextBoxColumn();
            elemColumns.Name = "MaxValue";
            elemColumns.HeaderText = Info.MaxValue;
            elemColumns.ReadOnly = true;
            elemColumns.SortMode = DataGridViewColumnSortMode.NotSortable;
            elemColumns.Width = 80;
            this.staShow.dgvSta.Columns.Add(elemColumns);

            elemColumns = new DataGridViewTextBoxColumn();
            elemColumns.Name = "MinValue";
            elemColumns.HeaderText = Info.MinValue;
            elemColumns.ReadOnly = true;
            elemColumns.SortMode = DataGridViewColumnSortMode.NotSortable;
            elemColumns.Width = 80;
            this.staShow.dgvSta.Columns.Add(elemColumns);

            elemColumns = new DataGridViewTextBoxColumn();
            elemColumns.Name = "MeanValue";
            elemColumns.HeaderText = Info.MeanValue;
            elemColumns.ReadOnly = true;
            elemColumns.SortMode = DataGridViewColumnSortMode.NotSortable;
            elemColumns.Width = 80;
            this.staShow.dgvSta.Columns.Add(elemColumns);

            elemColumns = new DataGridViewTextBoxColumn();
            elemColumns.Name = "SDValue";
            elemColumns.HeaderText = Info.SDValue;
            elemColumns.ReadOnly = true;
            elemColumns.SortMode = DataGridViewColumnSortMode.NotSortable;
            elemColumns.Width = 80;
            this.staShow.dgvSta.Columns.Add(elemColumns);

            elemColumns = new DataGridViewTextBoxColumn();
            elemColumns.Name = "RSDValue";
            elemColumns.HeaderText = Info.RSDValue;
            elemColumns.ReadOnly = true;
            elemColumns.SortMode = DataGridViewColumnSortMode.NotSortable;
            elemColumns.Width = 100;
            this.staShow.dgvSta.Columns.Add(elemColumns);

            var query = from elementl in elementList.Items.ToList().FindAll(w => w.IsShowElement) orderby elementl.LayerNumber group elementl by new { elementl.LayerNumBackUp, elementl.ThicknessUnit };


            this.staShow.dgvSta.RowCount = query.Count() > 0 ? query.Count() : 1;



            //if (WorkCurveHelper.WorkCurveCurrent.IsThickShowAreaThick)
            //    this.dgvStatics.RowCount += query.Count();

            //if (WorkCurveHelper.WorkCurveCurrent.IsThickShowContent)
            //this.dgvStatics.RowCount += elementList.Items.ToList().FindAll(w => w.IsShowElement && w.IsShowContent).Count + 1;
            //this.dgvStatics.RowCount += elementList.Items.ToList().FindAll(w => w.IsShowElement && w.IsShowContent).Count;
            this.staShow.dgvSta.RowCount += elementList.Items.ToList().FindAll(w => w.IsShowContent).Count;
            query = from elementl in elementList.Items.ToList().FindAll(w => w.Caption != string.Empty) orderby elementl.LayerNumber group elementl by new { elementl.LayerNumBackUp, elementl.ThicknessUnit };
            int j = 0;
            foreach (var elementlQuery in query)
            {
                string unit_h = string.Empty;
                string unit = string.Empty;
                string caption = string.Empty;
                // unit = (elementlQuery.Key.ThicknessUnit.ToString().Equals("ur")) ? "u〞" : elementlQuery.Key.ThicknessUnit.ToString();
                switch (elementlQuery.Key.ThicknessUnit)
                {
                    case ThicknessUnit.ur:
                        unit = "u〞";
                        break;
                    case ThicknessUnit.gl:
                        unit = "g/L";
                        break;
                    default:
                        unit = elementlQuery.Key.ThicknessUnit.ToString();
                        break;
                }
                List<CurveElement> CurveElementlist = elementList.Items.ToList().FindAll(delegate(CurveElement v) { return v.LayerNumBackUp == elementlQuery.Key.LayerNumBackUp && v.ThicknessUnit == elementlQuery.Key.ThicknessUnit; });

                CurveElementlist.ForEach(ss =>
                {
                    if (ss.IsShowElement)
                        caption += ss.DefineElemName + "|";
                    // caption += ss.Caption + "|";
                    //if (WorkCurveHelper.WorkCurveCurrent.IsThickShowContent)
                    if (WorkCurveHelper.CalcType != CalcType.PeakDivBase)
                    {
                        if (ss.IsShowContent)
                        {
                            unit_h = string.Empty;
                            unit_h = (ss.ContentUnit.ToString().Equals("per")) ? "%" : ss.ContentUnit.ToString();
                            this.staShow.dgvSta.Rows[j].Cells["statColumns"].Value = ss.DefineElemName + "(" + unit_h + ")";//ss.Caption + "(" + unit_h + ")";
                            this.staShow.dgvSta.Rows[j].Cells["statColumns"].Tag = "Content";
                            j++;
                        }
                    }
                });

                if (caption == string.Empty) continue;
                if (WorkCurveHelper.WorkCurveCurrent.IsThickShowAreaThick)
                {
                    string dUnit = Info.strAreaDensityUnit;
                    List<AreaDensityUnit> units = AreaDensityUnit.FindAll();
                    if (units.Find(w => w.Name == WorkCurveHelper.WorkCurveCurrent.AreaThickType) != null) dUnit = WorkCurveHelper.WorkCurveCurrent.AreaThickType;

                    this.staShow.dgvSta.Rows[j].Cells["statColumns"].Value = (caption == "") ? "" : caption.Substring(0, caption.Length - 1) + "(" + dUnit + ")";
                    this.staShow.dgvSta.Rows[j].Cells["statColumns"].Tag = "AreaDensity";
                    j++;
                }
                else
                {

                    this.staShow.dgvSta.Rows[j].Cells["statColumns"].Value = (caption == "") ? "" : caption.Substring(0, caption.Length - 1) + "(" + unit + ")";
                    if ((FpWorkCurve.thickMode == ThickMode.NiNi || FpWorkCurve.thickMode == ThickMode.NiCuNi || FpWorkCurve.thickMode == ThickMode.NiCuNiFe || FpWorkCurve.thickMode == ThickMode.NiCuNiFe2) && caption.Substring(0, caption.Length - 1).ToUpper().Equals("FE"))
                        this.staShow.dgvSta.Rows[j].Cells["statColumns"].Value = "Ni2" + "(" + unit + ")";


                    this.staShow.dgvSta.Rows[j].Cells["statColumns"].Tag = "Thick";
                    j++;
                }


            }
        }



        private bool isStaShow = false;



        private Worksheet sheetTemp = null;

        private FrmStaShow staShow;

        private int staStartRow = 0;

        private void btStatisticShow_Click(object sender, EventArgs e)
        {

            this.staShow = new FrmStaShow();

            CreateContructStatis(WorkCurveHelper.WorkCurveCurrent.ElementList);

            this.isStaShow = true;

            buttonWExcel_Click(null, null);

            if (sheetTemp == null)
            {
                Msg.Show("请选择待统计历史记录");
                return;
            }
          
            int eleNum = 0;
            ArrayList indexList = new ArrayList();
            for (int i = 0; i < this.sheetTemp.Cells.Columns.Count; i++)
            {
                if (this.sheetTemp.Cells[0, i].Value.ToString().Contains("厚度"))
                {

                    eleNum++;
                    indexList.Add(i);
                }
            }


            int curRow = 0;
            for (int i = 0; i < this.sheetTemp.Cells.Columns.Count; i++)
            {
                if (this.sheetTemp.Cells[0, i].Value.ToString().Contains("厚度"))
                {

                    this.staShow.dgvSta.Rows[curRow].Cells[1].Value = this.sheetTemp.Cells[this.staStartRow + 3, i].Value;
                    this.staShow.dgvSta.Rows[curRow].Cells[2].Value = this.sheetTemp.Cells[this.staStartRow + 4, i].Value;
                    this.staShow.dgvSta.Rows[curRow].Cells[3].Value = this.sheetTemp.Cells[this.staStartRow, i].Value;
                    this.staShow.dgvSta.Rows[curRow].Cells[4].Value = this.sheetTemp.Cells[this.staStartRow + 1, i].Value;
                    this.staShow.dgvSta.Rows[curRow].Cells[5].Value = this.sheetTemp.Cells[this.staStartRow + 2, i].Value;
                    curRow++;
                }

            }

            this.staShow.ShowDialog();
            this.isStaShow = false;
            this.sheetTemp = null;




        }

        private void chkShowAreaThick_CheckedChanged(object sender, EventArgs e)
        {
            WorkCurve workCurveCurrent = WorkCurveHelper.WorkCurveCurrent;

            int IsThickShowAreaThick = chkShowAreaThick.Checked ? 1 : 0;
            string sql = "Update WorkCurve Set IsThickShowAreaThick= " + IsThickShowAreaThick + " Where Id = " + workCurveCurrent.Id;
            Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
            WorkCurveHelper.WorkCurveCurrent = WorkCurve.FindById(workCurveCurrent.Id);
        }

        private void cmbThicknessUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            WorkCurve workCurveCurrent = WorkCurveHelper.WorkCurveCurrent;
            if (this.cmbThicknessUnit.SelectedItem.ToString() == "um")
            {

                for (int index = 0; index < workCurveCurrent.ElementList.Items.Count; index++)
                {
                    if (workCurveCurrent.ElementList.Items[index].IsShowElement)
                    {
                        workCurveCurrent.ElementList.Items[index].ThicknessUnit = ThicknessUnit.um;

                        string sqlUp = "update CurveElement set ThicknessUnit = " + "2" + " where Caption = '" + workCurveCurrent.ElementList.Items[index].Caption + "'";
                        Lephone.Data.DbEntry.Context.ExecuteNonQuery(sqlUp);
                    }

                }
            }
            
            else if (this.cmbThicknessUnit.SelectedItem.ToString() == "g/L")
            {
               
                for (int index = 0; index < workCurveCurrent.ElementList.Items.Count; index++)
                {
                    if (workCurveCurrent.ElementList.Items[index].IsShowElement)
                    {
                        workCurveCurrent.ElementList.Items[index].ThicknessUnit = ThicknessUnit.gl;


                        string sqlUp = "update CurveElement set ThicknessUnit = " + "3" + " where Caption = '" + workCurveCurrent.ElementList.Items[index].Caption + "'";
                        Lephone.Data.DbEntry.Context.ExecuteNonQuery(sqlUp);

                    }
                }
            }
            else
            {

                for (int index = 0; index < workCurveCurrent.ElementList.Items.Count; index++)
                {
                    if (workCurveCurrent.ElementList.Items[index].IsShowElement)
                    {
                        workCurveCurrent.ElementList.Items[index].ThicknessUnit = ThicknessUnit.ur;

                        string sqlUp = "update CurveElement set ThicknessUnit = " + "1" + " where Caption = '" + workCurveCurrent.ElementList.Items[index].Caption + "'";
                        Lephone.Data.DbEntry.Context.ExecuteNonQuery(sqlUp);
                    }

                }
            }

            WorkCurveHelper.WorkCurveCurrent = WorkCurve.FindById(workCurveCurrent.Id);



        }


        private void cmbDensityUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            WorkCurve workCurveCurrent = WorkCurveHelper.WorkCurveCurrent;

            int IsThickShowAreaThick = chkShowAreaThick.Checked ? 1 : 0;
            workCurveCurrent.AreaThickType = (cmbDensityUnit.Visible && cmbDensityUnit.SelectedItem != null) ? cmbDensityUnit.SelectedItem.ToString() : Info.strAreaDensityUnit;
            string sql = "Update WorkCurve Set IsThickShowAreaThick= " + IsThickShowAreaThick +
                " , AreaThickType='" + workCurveCurrent.AreaThickType + "'" + " Where Id = " + workCurveCurrent.Id;
            Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
            WorkCurveHelper.WorkCurveCurrent = WorkCurve.FindById(workCurveCurrent.Id);
        }

        private void panel2_Paint_1(object sender, PaintEventArgs e)
        {

        }

        


    }
}
