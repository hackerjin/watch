#define Test
using System;
using System.Windows.Forms;
using System.Reflection;
using Skyray.EDXRFLibrary;
using Skyray.EDXRFLibrary.Define;
using Skyray.EDX.Common;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Skyray.Controls;
using System.Data;
using Skyray.Print;
using System.Drawing.Printing;
using System.Drawing;
using Aspose.Cells;
using System.Data.SQLite;
using Lephone.Data;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using Lephone.Data.SqlEntry;
using Skyray.EDX.Common.ReportHelper;
using Skyray.Language;
using System.Xml;
using Skyray.EDXRFLibrary.Spectrum;



namespace Skyray.UC
{
    /// <summary>
    /// 历史记录类
    /// </summary>
    public partial class UCStock : Skyray.Language.UCMultiple
    {
        public static List<Auto> AutoDic = new List<Auto>();

        public delegate void Print(string path);

        public event Print OnPrintTemplate;

        public UCStock()
        {
            InitializeComponent();
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
            lblFactWeightValue.Text = "g";
            lblTheoryWeightValue.Text = "g";
            selectLong.Clear();
            this.dgvHistoryRecord.Columns.Clear();
            int totalPage = 0;
            int currentPage = 0;
            GetSepecifiedSearchCondition(totalPage, currentPage);
            if (this.dgvHistoryRecord.CurrentCell != null)
                this.dgvHistoryRecord.CurrentCell.Selected = false;
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
            string itemStr = ReportTemplateHelper.LoadSpecifiedValue("HistoryItem", "Setting");
            string[] str = itemStr.Split(',');
            List<string> listStr = str.ToList();
            string sColumnWidth = string.Empty;
            foreach (ToolStripMenuItem tempItem in this.contextMenuStrip1.Items)
            {
                if (!tempItem.Checked && !listStr.Contains(tempItem.Name))
                    listStr.Add(tempItem.Name);
                else if (tempItem.Checked && listStr.Contains(tempItem.Name))
                    listStr.Remove(tempItem.Name);

                if (this.dgvHistoryRecord.Columns.Contains(tempItem.Name))
                    sColumnWidth += tempItem.Name + ":" + this.dgvHistoryRecord.Columns[tempItem.Name].Width.ToString() + ",";


            }
            if (sColumnWidth != string.Empty) sColumnWidth = sColumnWidth.Substring(0, sColumnWidth.Length-1);
            itemStr = string.Empty;
            foreach (string tempStr in listStr)
                itemStr += tempStr + ",";
            ReportTemplateHelper.SaveSpecifiedValue("HistoryItem", "Setting", itemStr);
            ReportTemplateHelper.SaveSpecifiedValue("HistoryItem", "ColumnWidth", sColumnWidth);
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

        /// <summary>
        /// 历史记录控件的加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCHistoryRecord_Load(object sender, EventArgs e)
        {
            if (DifferenceDevice.IsAnalyser && ExcelTemplateParams.iTemplateType == 6)
            {
                comboBoxReportType.Visible = true;
                comboBoxReportType.SelectedIndex = 0;
                if (GP.CurrentUser.Role.RoleType.ToString() == "2")//贵金属印度版，如果是普通用户，控制删除，清空按钮出现
                {
                    buttonWDeleteCurrent.Visible = false;
                    btWClear.Visible = false;
                }
            }
            else
                comboBoxReportType.Visible = false;
            if (this.DesignMode)
                return;
            DeleColmun();
            ROWS_PER_PAGE = int.Parse(Skyray.EDX.Common.ReportTemplateHelper.LoadSpecifiedValue("OpenHistoryRecordType", "HistoryRecordShowNumber"));
            string HistoryRecordShowUnitType = Skyray.EDX.Common.ReportTemplateHelper.LoadSpecifiedValue("OpenHistoryRecordType", "HistoryRecordShowUnitType");
            WorkCurveHelper.PrintExcelCount = int.Parse(Skyray.EDX.Common.ReportTemplateHelper.LoadSpecifiedValue("OpenHistoryRecordType", "SelectRecordNumber").ToString());
            if (HistoryRecordShowUnitType == "1") this.checkShowUnit.Checked = true; else this.checkShowUnit.Checked = false;
            if (!Lephone.Data.DbEntry.Context.GetTableNames().Contains("HistoryRecord"))
            {
                HistoryRecord.FindAll();
            }
            if (!Lephone.Data.DbEntry.Context.GetTableNames().Contains("HistoryElement"))
            {
                HistoryElement.FindAll();
            }
            if (!Lephone.Data.DbEntry.Context.GetTableNames().Contains("Condition"))
                Skyray.EDXRFLibrary.Condition.FindAll();
            string sql = "select * from WorkCurve a inner join Condition b on a.Condition_Id = b.Id inner join Device c on b.Device_Id=c.Id where c.Id=" + WorkCurveHelper.DeviceCurrent.Id;
            if (DifferenceDevice.IsXRF)
                sql += " and a.FuncType =" + (int)FuncType.XRF;
            else if (DifferenceDevice.IsThick)
                sql += " and a.FuncType =" + (int)FuncType.Thick;
            else if (DifferenceDevice.IsRohs)
                sql += " and a.FuncType=" + (int)FuncType.Rohs;
            var listCuve = WorkCurve.FindBySql(sql + " and  b.Device_Id=" + WorkCurveHelper.DeviceCurrent.Id +" and b.Type=0");
            this.comboBoxCurveName.Items.Clear();
            if (!DifferenceDevice.IsThick)
            this.comboBoxCurveName.Items.Add(new MyItem("--All--", -1));
            foreach (WorkCurve workcurve in listCuve)
            {
                this.comboBoxCurveName.Items.Add(new MyItem(workcurve.Name, (int)workcurve.Id));
            }
            this.cboStatus.Items.Clear();
            this.cboStatus.Items.Add(new MyItem("--All--", -1));
            this.cboStatus.Items.Add(new MyItem(Info.Storage, 1));
            this.cboStatus.Items.Add(new MyItem(Info.Extraction, 0));
            this.cboStatus.SelectedIndex = 0;
            if (WorkCurveHelper.WorkCurveCurrent != null)
                this.comboBoxCurveName.Text = WorkCurveHelper.WorkCurveCurrent.Name;
            Lang.Model.LanguageChanged += new EventHandler(Model_LanguageChanged);
            if (this.ParentForm != null)
                this.ParentForm.FormClosing += (s, ex) => Skyray.Language.Lang.Model.DGVS.Remove(this.dgvHistoryRecord);
            this.dgvHistoryRecord.CMenu.Items[1].Visible =
            this.dgvHistoryRecord.CMenu.Items[2].Visible = false;
            buttonWSearch_Click(null, null);
            if (DifferenceDevice.IsXRF && !(this.Parent is Form))
            {
                buttonWCancel.Visible = false;
            }
            if (DifferenceDevice.interClassMain.IsBarcodeScanning)
            {
                this.textBoxWInputName.KeyPress += new KeyPressEventHandler(textBoxWInputName_KeyPress);
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
            buttonWSearch_Click(null, null);
        }

        public struct MyItem
        {
            public string Text;
            public int Value;//你需要的值类型
            public MyItem(string text, int value)
            {
                this.Text = text;
                this.Value = value;
            }
            public override string ToString()
            {
                return Text;
            }
        }

        /// <summary>
        /// 删除指定的历史记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWDeleteCurrent_Click(object sender, EventArgs e)
        {
            if (selectLong.Count == 0) return;
            foreach (long seleid in selectLong)
            {
                SqlStatement sqlstate = new SqlStatement("delete from historycompanyotherinfo where history_id='" + seleid.ToString() + "';delete from HistoryRecord where Id=" + seleid.ToString() + " ;delete from historyelement where HistoryRecord_Id=" + seleid.ToString());
                Lephone.Data.DbEntry.Context.ExecuteNonQuery(sqlstate);
            }
            buttonWSearch_Click(null, null);
        }

        private void buttonWCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
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
            foreach (DataGridViewColumn column in this.dgvHistoryRecord.Columns)
            {
                if (column.Visible && column.Index > 0)
                {
                    colIndex.Add(column.Index);
                }
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
            printDocument1.DocumentName = "打印标题";
            printDocument1.PrinterSettings = printDialog.PrinterSettings;
            printDocument1.DefaultPageSettings.PrinterSettings.PrintRange = printDialog.PrinterSettings.DefaultPageSettings.PrinterSettings.PrintRange;
            printDocument1.DefaultPageSettings.Margins = new Margins(20, 20, 20, 20);
            DataGridView dgvToPrint = dgvHistoryRecord.Copy(toPrintCols, toPrintRows);
            dgvToPrint.AllowUserToAddRows = false;
            gridPrinter = new GridPrinter(dgvToPrint, printDocument1, true, true, "历史记录", new System.Drawing.Font("黑体", 18, FontStyle.Bold, GraphicsUnit.Point), Color.Black, true);
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
            if (e.Button == MouseButtons.Right &&
                this.dgvHistoryRecord.HitTest(e.X, e.Y).Type == DataGridViewHitTestType.ColumnHeader)
            {
                this.contextMenuStrip1.Show(this.dgvHistoryRecord.PointToScreen(e.Location));
            }
        }
        #endregion

        private void buttonWExcel_Click(object sender, EventArgs e)
        {
            if (selectLong.Count == 0) return;


            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;//等待?
            //Workbook workbook = new Workbook();

            string strId = "";
            foreach (long id in selectLong)
                strId += id.ToString() + ",";
            if (strId != "") strId = strId.Substring(0, strId.Length - 1);
            DataTable dt = GetData(GetSQL(1, -1, -1, strId));
            SetDecimalPlaces(ref dt);
            SetTestResult(ref dt);
            this.dataGridViewW2.DataSource = dt;
            foreach (DataGridViewColumn gridcol in this.dgvHistoryRecord.Columns)
                if (gridcol.Visible && gridcol.Name != "aa" && this.dataGridViewW2.Columns.Contains(gridcol.Name))
                    this.dataGridViewW2.Columns[gridcol.Name].HeaderText = gridcol.HeaderText;
                else if (gridcol.Visible == false && gridcol.Name != "aa" && this.dataGridViewW2.Columns.Contains(gridcol.Name))
                    this.dataGridViewW2.Columns[gridcol.Name].Visible = false;
            this.dataGridViewW2.Columns["HistoryRecord_Id"].Visible = false;
            this.dataGridViewW2.Columns["workcurveid"].Visible = false;


            OutExcel(this.dataGridViewW2);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;//等待?
        }

        private void btWClear_Click(object sender, EventArgs e)
        {
            if (Msg.Show(Info.RemoveAll, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                string strsql = (comboBoxCurveName.Text == "--All--") ? " and workcurveid in (select id from workcurve where condition_id in ( " +
           " select id from condition where device_id=" + WorkCurveHelper.DeviceCurrent.Id + "))" : " and workcurveid in (select ID from  workcurve where  name='" + comboBoxCurveName.Text + "' and condition_id in (select id from condition where device_id=" + WorkCurveHelper.DeviceCurrent.Id + "))";

                SqlStatement sqlstate = new SqlStatement("delete from historycompanyotherinfo where history_id in (select id from  historyrecord where 1=1 "+strsql+");  " +
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

            DataTable dt = GetData(GetSQL(2, -1, -1, ""));
            SetDecimalPlaces(ref dt);
            SetTestResult(ref dt);

            this.dataGridViewW2.DataSource = dt;
            foreach (DataGridViewColumn gridcol in this.dgvHistoryRecord.Columns)
                if (gridcol.Visible && gridcol.Name != "aa" && this.dataGridViewW2.Columns.Contains(gridcol.Name))
                    this.dataGridViewW2.Columns[gridcol.Name].HeaderText = gridcol.HeaderText;
                else if (gridcol.Visible == false && gridcol.Name != "aa" && this.dataGridViewW2.Columns.Contains(gridcol.Name))
                    this.dataGridViewW2.Columns[gridcol.Name].Visible = false;
            this.dataGridViewW2.Columns["HistoryRecord_Id"].Visible = false;
            this.dataGridViewW2.Columns["workcurveid"].Visible = false;
            OutExcel(this.dataGridViewW2);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;//等待?

        }

        private void OutExcel(DataGridViewW dataGridViewW2)
        {
            bool hasRecord = false;
            Workbook workbook = new Workbook();
            string fileName="";



            string excelModeType = ReportTemplateHelper.LoadSpecifiedValue("Excel", "IsOurExcel");
            if (excelModeType == "1")
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
            if (excelModeType == "2")
            {
                List<string> otherinfo = new List<string>();
                otherinfo.Add(Info.Total + " " + dataGridViewW2.RowCount + " " + Info.Article);
                List<string> redFieldInfo = new List<string>();
                fileName = dataGridViewW2.ExportExcel_Public_SpecialRowNew(false, out hasRecord, out workbook, otherinfo, redFieldInfo);
            }
            else
            {
                fileName = dataGridViewW2.ExportExcel_Public_SpecialRow(false, out hasRecord, out workbook);
            }


            if (!hasRecord)
            {
                Msg.Show(Info.SelectHistoryRecord);
                this.Cursor = System.Windows.Forms.Cursors.Arrow;//等待?
                return;
            }

            SaveFileDialog sdlg = new SaveFileDialog();
            sdlg.Filter = "Excel File(*.xls)|*.xls";
            if (sdlg.ShowDialog() == DialogResult.OK)
            {
                ExcelExporter excelExporter = new ExcelExporter(null);
                if (excelExporter.IsFileUsed(sdlg.FileName) && File.Exists(sdlg.FileName))
                {
                    Msg.Show(Info.strOpenExcel);
                    this.Cursor = System.Windows.Forms.Cursors.Arrow;//等待?
                    return;
                }
                workbook.Save(sdlg.FileName);
                fileName = sdlg.FileName;
            }

            if (!fileName.IsNullOrEmpty())
                UIHelper.AskToOpenFile(fileName);
        }
        /// <summary>
        ///历史记录导入Excel
        /// </summary>
        /// <param name="fileName">保存的文件名，为空时直接打印</param>
        /// <param name="colContent">含量显示起始列</param>
        private void ExcelReport(string fileName, DataTable dgvRecord)
        {
            if (WorkCurveHelper.IsExcelInstall())//添加判断是否安装了Excel 0319
            {
                PrintDocument printDoc = new PrintDocument();
                Excel.Application excelApp = new Excel.Application();
                excelApp.ScreenUpdating = false;
                Excel.Workbook wbook = excelApp.Workbooks.Add(Missing.Value);
                Excel.Worksheet sheet = (Excel.Worksheet)wbook.Sheets[1];
                Excel.Range range = sheet.get_Range("A1", Missing.Value);
                try
                {
                    string[] titles = new string[dgvRecord.Columns.Count];
                    int cols = 0;
                    foreach (DataColumn columnTemp in dgvRecord.Columns)
                    {
                        titles[cols] = columnTemp.Caption;
                        cols++;
                    }
                    range = range.get_Resize(1, cols);
                    range.set_Value(Missing.Value, titles);
                    if (dgvRecord.Rows.Count > 0)
                    {
                        object[,] cellsValue = new object[dgvRecord.Rows.Count, cols];
                        cols = 0;
                        foreach (DataColumn column in dgvRecord.Columns)
                        {
                            for (int j = 0; j < dgvRecord.Rows.Count; j++)
                            {
                                cellsValue[j, cols] = dgvRecord.Rows[j][column.ColumnName];
                                Excel.Range rangeFormat = sheet.get_Range("A" + (j + 2), Missing.Value);
                                rangeFormat.NumberFormatLocal = "@"; //将Excel中A列设为文本格式 以防止000保存为0
                                Excel.Range rangeFormatDate = sheet.get_Range("B" + (j + 2), Missing.Value);
                                rangeFormatDate.ColumnWidth = 20;
                                Excel.Range rangeFormatLotNo = sheet.get_Range("D" + (j + 2), Missing.Value);
                                rangeFormatLotNo.NumberFormatLocal = "@"; //将Excel中D列设为文本格式 以防止000保存为0
                            }
                            cols++;
                        }
                        range = sheet.get_Range("A2", Missing.Value);
                        range = range.get_Resize(dgvRecord.Rows.Count, cols);
                        range.set_Value(Missing.Value, cellsValue);
                        range.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    }
                    excelApp.Visible = false;
                    try
                    {
                        if (fileName != "") //保存
                        {
                            if (System.IO.File.Exists(fileName))
                            {
                                System.IO.File.Delete(fileName);
                            }
                            wbook.SaveAs(fileName, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                Excel.XlSaveAsAccessMode.xlExclusive, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                        }
                        else //打印
                        {
                            sheet.PrintOut(Missing.Value, Missing.Value, Missing.Value, false, Missing.Value, false, false, Missing.Value);

                        }
                    }
                    catch (Exception)
                    {

                    }
                    finally  //确保关闭工作簿
                    {
                        sheet = null;
                        wbook.Close(false, Missing.Value, false);
                        wbook = null;
                    }
                }
                finally //确保关闭Excel进程
                {
                    if (excelApp != null)
                    {
                        VkExcel excelReport=new VkExcel();
                        excelReport.stopExcel();
                        excelReport.stopExcelByexcelApp(excelApp);
                    }
                }
            }
            else
            {
            }

        }

        public static void OpenFile(string FileName)
        {
            Help.ShowHelp(null, FileName);
        }

        private DataTable UniteDataTable(DataTable dt1, DataTable dt2)
        {
            if (dt1 == null && dt2 != null)
                return dt2;
            DataTable dt3 = dt1.Clone();
            int dt2Column = 0;
            for (int i = 0; i < dt2.Columns.Count; i++)
            {
                if (!dt3.Columns.Contains(dt2.Columns[i].ColumnName))
                {
                    dt2Column++;
                    dt3.Columns.Add(dt2.Columns[i].ColumnName);
                }
            }
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                DataRow tempR = dt3.NewRow();
                foreach (DataColumn temp in dt1.Columns)
                {
                    tempR[temp.ColumnName] = dt1.Rows[i][temp.ColumnName];
                }
                dt3.Rows.Add(tempR);
            }
            if (dt1.Rows.Count >= dt2.Rows.Count)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    foreach (DataColumn temp in dt2.Columns)
                    {
                        dt3.Rows[i][temp.ColumnName] = dt2.Rows[i][temp.ColumnName].ToString();
                    }
                }
            }
            else
            {
                DataRow dr3;
                for (int i = 0; i < dt2.Rows.Count - dt1.Rows.Count; i++)
                {
                    dr3 = dt3.NewRow();
                    dt3.Rows.Add(dr3);
                }
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    foreach (DataColumn temp in dt2.Columns)
                    {
                        dt3.Rows[i][temp.ColumnName] = dt2.Rows[i][temp.ColumnName].ToString();
                    }
                }
            }
            return dt3;
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
                            row.Cells[col.HeaderText.Replace("_Color", "")].Style.ForeColor = Color.Red;

                        col.Visible = false;

                        
                        
                    }

                }
            }

        }

        private void dataGridViewW2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            string excelModeType = ReportTemplateHelper.LoadSpecifiedValue("Excel", "IsOurExcel");
            if (DifferenceDevice.IsRohs && excelModeType == "0")
                foreach (DataGridViewRow row in dataGridViewW2.Rows)
                {
                    foreach (DataGridViewColumn col in dataGridViewW2.Columns)
                    {


                        if (col.HeaderText.Contains("_Color"))
                        {
                            if (row.Cells[col.HeaderText].Value.ToString() == "true")
                                row.Cells[col.HeaderText.Replace("_Color", "")].Style.ForeColor = Color.Red;

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
                }
                else
                {
                    Atom atom = Atoms.AtomList.Find(s => s.AtomName == col.HeaderText);
                    if (atom != null && !seleElement.Contains(col.HeaderText) && col.Visible)
                        seleElement.Add(col.HeaderText);
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
        private ElementList FilterElementsByDGV(DataGridView dgv,long historyId)
        {
            HistoryRecord historyRecord = HistoryRecord.FindById(historyId);
            if (historyRecord == null) return null;
            WorkCurve workCurve = WorkCurve.FindById(historyRecord.WorkCurveId);
            if (workCurve == null) return null;
            ElementList elementList = workCurve.ElementList;//感兴趣元素集合
            ElementList backElementList = ElementList.New;
            //历史记录元素集合
            List<HistoryElement> hisElementList = HistoryElement.FindBySql("select * from HistoryElement where HistoryRecord_Id ="+historyId);
            List<string> eleNames = FilterElementsNameByDGV(dgv);
            foreach (string s in eleNames)
            {
                CurveElement curEle = elementList.Items.ToList().Find(w => (w.Formula==s));
                if (curEle == null)//感兴趣元素外的元素
                {
                    curEle = CurveElement.New;
                    curEle.Formula = s; 
                    curEle.Caption = s;
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
                }
                else//历史记录中也不存在 不赋值
                { }
                backElementList.Items.Add(curEle);
            }
            return backElementList;
        }

        private void btwTemplateExcel_Click(object sender, EventArgs e)
        {
            if (selectLong.Count == 0) return;
            string valid = DifferenceDevice.interClassMain.GenerateGenericReport(FilterElementsByDGV(dgvHistoryRecord, selectLong.FirstOrDefault()), selectLong);
            if (!string.IsNullOrEmpty(valid))
            {
                DifferenceDevice.interClassMain.OpenPathThread(valid);
                return;
            }
            List<string> SeleWorkCurveNameList = new List<string>();
            if (ExcelTemplateParams.iTemplateType == 1)
            {
                #region
                Report report = new Report();
                report.isShowND = WorkCurveHelper.isShowND;
                ElementList elementList = ElementList.New;
                if (selectLong.Count > WorkCurveHelper.PrintExcelCount)
                {
                    Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
                    return;
                }
                else
                {
                    //何晓明 20100715 Rohs4模板多语言切换
                    ExcelTemplateParams.GetExcelTemplateParams();
                    string workCurveName = string.Empty;
                    SpecEntity spec = new SpecEntity();
                    SpecListEntity tempList = new SpecListEntity();
                    for (int i = 0; i < selectLong.Count; i++)
                    {
                        HistoryRecord record = HistoryRecord.FindById(selectLong[i]);
                        if (record == null)
                            continue;

                        report.historyRecordid = record.Id.ToString();
                        WorkCurve workCurve = WorkCurve.FindById(record.WorkCurveId);
                        if (workCurve == null)
                            continue;
                        workCurveName = workCurve.Name;
                        if (!SeleWorkCurveNameList.Contains(workCurve.Name)) SeleWorkCurveNameList.Add(workCurve.Name);
                        tempList = DataBaseHelper.QueryByEdition(record.SpecListName,record.FilePath,record.EditionType);
                        if (tempList != null &&tempList.Specs.Length > 0)
                        {
                            if (!string.IsNullOrEmpty(tempList.Specs[0].SpecData))
                                spec = tempList.Specs[0];
                        }
                        else
                        {
                            Msg.Show(Info.DataDelete);
                            return;
                        }
                        var elements = HistoryElement.Find(w => w.HistoryRecord.Id == record.Id);
                        foreach (var element in elements)
                        {
                            var temp = CurveElement.FindAll().Find(delegate(CurveElement curveElement) { return curveElement.Caption == element.elementName && curveElement.ElementList != null && curveElement.ElementList.WorkCurve!=null
                                && curveElement.ElementList.WorkCurve.Id == workCurve.Id; });

                            if (temp == null)//感兴趣元素外的历史记录元素
                            {
                                temp = CurveElement.New;
                                temp.Intensity = element.CaculateIntensity;
                                temp.Error = element.Error;
                                if (element.unitValue == 2)
                                    temp.Error = temp.Error / 10000;
                                else
                                    temp.Error = temp.Error / 10;
                                double elecontent = 0.0;
                                double.TryParse(element.contextelementValue, out elecontent);
                                if (element.unitValue == 1)
                                    temp.Content = elecontent;
                                else if (element.unitValue == 2)
                                    temp.Content = elecontent / 10000;
                                else
                                    temp.Content = elecontent / 10;
                                elementList.Items.Add(temp);
                            }
                            double content = 0.0;
                            double.TryParse(element.contextelementValue, out content);
                            temp.Intensity = element.CaculateIntensity;
                            temp.Error = element.Error;
                            if (element.unitValue == 2)
                                temp.Error = temp.Error / 10000;
                            else
                                temp.Error = temp.Error / 10;
                            //temp.Error = element.Error;
                            if (element.unitValue == 1)
                                temp.Content = content;
                            else if (element.unitValue == 2)
                                temp.Content = content / 10000;
                            else
                                temp.Content = content / 10;
                            elementList.Items.Add(temp);
                            report.historyStandID = element.customstandard_Id;
                        }
                    }
                    if (comboBoxCurveName.Text == "--All--" && SeleWorkCurveNameList.Count > 1)
                    {
                        Msg.Show(Info.strSeleWorkCurveName);
                        return;
                    }
                    #region
                    foreach (ToolStripMenuItem tempItem in this.contextMenuStrip1.Items)
                    {
                        if (!tempItem.Checked && Atoms.AtomList.ToList().Find(w => w.AtomName == tempItem.Name) != null)
                        {
                            CurveElement tempFind = elementList.Items.ToList().Find(w => w.Caption == tempItem.Name);
                            if (tempFind != null)
                                elementList.Items.Remove(tempFind);
                        }

                    }
                    if (elementList.Items.Count == 0)
                    {
                        Msg.Show(Info.NoLoadSource, Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        return;
                    }
                    #endregion
                    report.Spec = spec;
                    report.specList = tempList;
                    report.operateMember = FrmLogon.userName;
                    report.Elements = FilterElementsByDGV(dgvHistoryRecord,selectLong.FirstOrDefault()); //elementList;
                    report.WorkCurveName = workCurveName;
                    report.FirstContIntr.Add(elementList.Items.Count);
                    //修改：何晓明 20111129 报告命名设置 赋值设置名称
                    //string nowTime = DateTime.Now.ToString("yyyyMMddhhmmss");
                    //获取自定义报告名称
                    string reportName = GetDefineReportName();
                    //
                    if (selectLong.Count == 1)
                    {
                        report.InterestElemCount = report.Elements.Items.Count;//elementList.Items.Count;
                        report.TempletFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.OneTimeTemplate;
                        report.ReportPath = WorkCurveHelper.ExcelPath;
                        //修改：何晓明 20111129 报告命名设置 单条记录模板
                        report.GenerateReport(reportName, true);
                        //report.GenerateReport(nowTime, true);
                        //
                    }
                    else
                    {
                        DataTable dt =  CreateReTestTable(report.Elements);//CreateReTestTable(elementList);
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
                                element = element ?? HistoryElement.New;//统一方式处理为空数据 得到结果与为0结果一致
                                element.contextelementValue =element.contextelementValue ?? "0";
                                element.thickelementValue = element.thickelementValue??"0";                                
                                if (element != null && column.Visible && dt.Columns.Contains(column.HeaderText))
                                {
                                    string valueStr =element.contextelementValue;
                                    if (!string.IsNullOrEmpty(valueStr))
                                    {
                                        double Value = double.Parse(valueStr);
                                        if (element.unitValue == 1)
                                            Value = Value * 10000;
                                        else if (element.unitValue == 3)
                                            Value = Value * 1000;
                                        if (Value <= WorkCurveHelper.NDValue)
                                        {
                                            if (element.unitValue == 1)
                                            {
                                                rowNew[column.HeaderText] = "ND(%)";
                                            }
                                            else if (element.unitValue == 2)
                                            {
                                                rowNew[column.HeaderText] = "ND(ppm)";
                                            }
                                            else
                                                rowNew[column.HeaderText] = "ND(‰)";
                                        }
                                        else
                                        {
                                            if (element.unitValue == 1)
                                            {
                                                rowNew[column.HeaderText] = (Value / 10000).ToString("f"+WorkCurveHelper.SoftWareContentBit.ToString())+"(%)";
                                            }
                                            else if (element.unitValue == 2)
                                            {
                                                rowNew[column.HeaderText] = Value.ToString("f"+WorkCurveHelper.SoftWareContentBit.ToString()) + "(ppm)";
                                            }
                                            else
                                            {
                                                rowNew[column.HeaderText] = (Value / 1000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(‰)";
                                            }
                                        }
                                        totalContent += Value;
                                    }
                                }
                                else if (element == null && column.Visible && dt.Columns.Contains(column.HeaderText))//无数据时处理方式
                                {
                                    //rowNew[column.HeaderText] = default(double).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());//所得数据格式与之前数据格式不一致
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
                        report.InterestElemCount = elementList.Items.Count;
                        List<long> lstEleId = new List<long>();
                        foreach(var ele in elementList.Items)
                        {
                            lstEleId.Add(ele.Id);
                        }
                        if (lstEleId.Distinct().Count() > (string.IsNullOrEmpty(ExcelTemplateParams.TotalColumnMun) ? 9 : int.Parse(ExcelTemplateParams.TotalColumnMun)))
                            if (File.Exists(Application.StartupPath + "\\HistoryExcelTemplate\\"+ExcelTemplateParams.ManyTimeTemplate.TrimEnd(".xls".ToCharArray()) + "_Horizontal.xls"))
                            ExcelTemplateParams.ManyTimeTemplate=ExcelTemplateParams.ManyTimeTemplate.TrimEnd(".xls".ToCharArray()) + "_Horizontal.xls";
                        report.RetestFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.ManyTimeTemplate;
                        report.ReportPath = WorkCurveHelper.ExcelPath;
                        //修改：何晓明 20111129 报告命名设置 多条记录模板
                        report.GenerateRetestReport(reportName, dt, true);
                        //report.GenerateRetestReport(nowTime, dt, true);
                        //
                    }
                    ////修改：何晓明 20110715 按模板导出打开Excel
                    //var pProcess = System.Diagnostics.Process.GetProcessesByName("Excel");
                    //if (pProcess != null && pProcess.Length > 0) try { pProcess[0].Kill(); }
                    //    catch { };
                    //
                    //修改：何晓明 20110715 按模板导出打开Excel
                    //Msg.Show(Info.ExportSuccessOld);
                    if(!File.Exists(report.ReportPath+reportName+".xls")&&!File.Exists(report.ReportPath+reportName+"_Retry.xls"))
                        return;
                    if (Skyray.Controls.SkyrayMsgBox.Show(PrintInfo.SaveSuccess + Skyray.EDX.Common.Info.OpenExcelOrNot, Skyray.EDX.Common.Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        //
                        string excelOnePath = report.ReportPath + reportName + ".xls";
                        string excelManyPath = report.ReportPath + reportName + "_Retry" + ".xls";
                        //string excelOnePath = report.ReportPath + nowTime + "_" + report.Spec.SpecList.SampleName + ".xls";
                        //string excelManyPath = report.ReportPath + nowTime + "_" + report.Spec.SpecList.SampleName + "_Retry" + ".xls";
                        //
                        if (selectLong.Count == 1)
                            Help.ShowHelp(null, excelOnePath);
                        else
                            Help.ShowHelp(null, excelManyPath);
                    }
                    //
                }
                #endregion
            }
            else if (ExcelTemplateParams.iTemplateType == 2)
            {
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
                else
                {
                    List<string> seleElement = new List<string>();
                    foreach (DataGridViewColumn col in this.dgvHistoryRecord.Columns)
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
                            if ((atom != null || oxidel.Count>0) && !seleElement.Contains(col.HeaderText) && col.Visible)
                                seleElement.Add(col.HeaderText);
                        }
                        else
                        {
                            Atom atom = Atoms.AtomList.Find(s => s.AtomName == col.HeaderText);
                            if (atom != null && !seleElement.Contains(col.HeaderText) && col.Visible)
                                seleElement.Add(col.HeaderText);
                        }

                    }

                    string workCurveName = string.Empty;
                    SpecEntity spec = new SpecEntity();
                    List<InterfaceClass.PrintObject> seleHistoryPrintObjectL = new List<InterfaceClass.PrintObject>();
                    foreach (long seleid in selectLong)
                    {
                        long id = long.Parse(seleid.ToString());

                        //List<CustomStandard> customStandard = CustomStandard.FindBySql("select * from customstandard where id in (select customstandard_id from historyelement where  historyrecord_id=" + id.ToString() + ")");

                        HistoryRecord record = HistoryRecord.FindById(id);
                        if (record != null)
                        {
                            WorkCurve workCurve = WorkCurve.FindById(record.WorkCurveId);
                            if (workCurve != null)
                            {
                                if (!SeleWorkCurveNameList.Contains(workCurve.Name)) SeleWorkCurveNameList.Add(workCurve.Name);
                                string specListName = record.SpecListName;
                                workCurveName = workCurve.Name;
                                SpecListEntity tempList = DataBaseHelper.QueryByEdition(specListName,record.FilePath,record.EditionType);
                                if (tempList != null &&tempList.Specs.Length > 0)
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

                    if (InterfaceClass.SetPrintTemplate(seleElement, seleHistoryPrintObjectL))
                    {                        
                        EDXRFHelper.NewPrintDirectPrintHelper(InterfaceClass.seledataFountain);
                    }
                    else Msg.Show(Info.NoLoadSource);

                }

                #endregion
            }
            else if (ExcelTemplateParams.iTemplateType == 6)
            {
                #region
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

                     if (Skyray.Controls.SkyrayMsgBox.Show(PrintInfo.SaveSuccess + OpenExcelOrNot, Skyray.EDX.Common.Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                     {
                         Help.ShowHelp(null, SaveReportPath);
                     }


                }
                #endregion
            }
            else if (ExcelTemplateParams.iTemplateType == 7 )
            {
                #region //

               // if (selectLong.Count > WorkCurveHelper.PrintExcelCount)
               // {
               //     Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
               //     return;
               // }

               // string Language = "";
               // Languages CurrentLang = Languages.FindOne(l => l.IsCurrentLang == true);
               // Language = CurrentLang.ShortName;
               // if (Language.ToLower() == "english") Language = "EN";
               //  else if (Language.ToLower() == "chinese") Language = "CN";

               //  string reportName = GetDefineReportName();
               //  Report report = new Report();
               //  List<BrassReport> BrassReportList = new List<BrassReport>();

               //  DataTable dt_BrassStatistics = new DataTable();

               //  ElementList elementStatisticsList = null;

               //  foreach (long hisid in selectLong)
               //  {
               //      #region
               //      string SampleName="";
               //      DateTime TestTime=DateTime.Now;
               //      string WorkCurveName="";
               //      string Specification="";
               //      string SupplierName="";
               //      string Weight="";
               //      string Operater="";
               //      string Address="";
               //      string appAddress = "";
               //      Address = appAddress;

               //      DataTable dt_Brass=null;
               //      List<SpecList> lSpecList = SpecList.FindBySql("select * from speclist where id in (select speclistid from historyrecord where id=" + hisid + " )");
               //      if (lSpecList.Count == 0) return;

               //      SampleName = lSpecList[0].Name;
               //      TestTime = DateTime.Parse(lSpecList[0].SpecDate.ToString());

               //      WorkCurve workCurve = WorkCurve.FindById(lSpecList[0].WorkCurveId);
               //      WorkCurveName = (workCurve==null)?"":workCurve.Name;
               //      SupplierName = lSpecList[0].Supplier;
               //      Weight = lSpecList[0].Weight.ToString();
               //      Operater = lSpecList[0].Operater;
               //      Specification = lSpecList[0].SpecSummary;
               //      ElementList elementList = ElementList.New;
               //      var elements = HistoryElement.Find(w => w.HistoryRecord.Id == hisid);
               //      foreach (var element in elements)
               //      {
               //          var temp = CurveElement.FindAll().Find(delegate(CurveElement curveElement) { return curveElement.Caption == element.elementName && curveElement.ElementList.WorkCurve.Id == workCurve.Id; });
               //          if (temp == null)
               //              continue;
               //          double content = 0.0;
               //          double.TryParse(element.contextelementValue, out content);
               //          temp.Intensity = element.CaculateIntensity;
               //          temp.Error = element.Error;
               //           if (element.unitValue == 2)
               //               temp.Error = temp.Error / 10000;
               //          else
               //               temp.Error = temp.Error / 10;

               //          if (element.unitValue == 1)
               //              temp.Content = content;
               //          else if (element.unitValue == 2)
               //              temp.Content = content / 10000;
               //          else
               //              temp.Content = content / 10;
               //          elementList.Items.Add(temp);
               //      }

               //      foreach (ToolStripMenuItem tempItem in this.contextMenuStrip1.Items)
               //      {
               //          if (Atoms.AtomList.ToList().Find(w => w.AtomName == tempItem.Name) != null)
               //          {
               //              if(!tempItem.Checked)
               //              {
               //                  CurveElement tempFind = elementList.Items.ToList().Find(w => w.Caption == tempItem.Name);
               //                  if (tempFind != null)
               //                      elementList.Items.Remove(tempFind);
               //              }

               //              if(!elementList.Items.ToList().Exists(delegate(CurveElement v){return v.Caption==tempItem.Name;}))
               //              {
               //                  var temp = CurveElement.FindAll().Find(delegate(CurveElement curveElement) { return curveElement.Caption == tempItem.Name && curveElement.ElementList.WorkCurve.Id == workCurve.Id; });
               //                  if (temp == null)
               //                      continue;
               //                  double content = 0.0;
               //                  double.TryParse("0", out content);
               //                  temp.Intensity = 0;
               //                  temp.Error = 0;
               //                  temp.Content = 0;
               //                  elementList.Items.Add(temp);
               //              }
               //          }


               //      }


               //      if (elementStatisticsList!=null && elementList.Items.Count > elementStatisticsList.Items.Count)
               //      elementStatisticsList = elementList;
               //      else if (elementStatisticsList == null) elementStatisticsList = elementList;

               //      dt_Brass = SetColumns(elementList);
               //      dt_Brass.Columns.RemoveAt(0);

               //      for (int i = 1; i <= 3; i++)
               //      {
               //          DataRow dr = dt_Brass.NewRow();
               //          dt_Brass.Rows.Add(dr);
               //      }



               //      foreach (CurveElement curele in elementList.Items.ToList().OrderBy(d => d.RowsIndex).ToList())
               //      {
               //          Atom atom = Atoms.AtomList.ToList().Find(w => w.AtomName == curele.Caption);
               //          dt_Brass.Rows[0][curele.Caption] = (atom == null) ? "" : ((Language == "CN") ? atom.AtomNameCN : atom.AtomNameEN);
               //          dt_Brass.Rows[1][curele.Caption] = (atom == null) ? "" : atom.AtomName;
               //          dt_Brass.Rows[2][curele.Caption] = curele.Content.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
               //      }

               //      BrassReport BrassReport = new BrassReport(SampleName, TestTime, WorkCurveName, Specification,
               //          SupplierName, Weight, Operater, Address, dt_Brass);

               //      BrassReportList.Add(BrassReport);

               //      #endregion
               //  }

               //  if (BrassReportList.Count > 1)
               //  {

               //      //获取统计信息
               //      dt_BrassStatistics = SetColumns(elementStatisticsList);
               //      for (int i = 1; i <= 5; i++)
               //      {
               //          DataRow dr = dt_BrassStatistics.NewRow();
               //          dt_BrassStatistics.Rows.Add(dr);
               //      }

               //      for (int i = 0; i < 5; i++)
               //      {
               //          switch (i)
               //          {
               //              case 0:
               //                  dt_BrassStatistics.Rows[i][0] = Info.Statics;
               //                  break;
               //              case 1:
               //                  dt_BrassStatistics.Rows[i][0] = Info.MeanValue;
               //                  break;
               //              case 2:
               //                  dt_BrassStatistics.Rows[i][0] = Info.SDValue;
               //                  break;
               //              case 3:
               //                  dt_BrassStatistics.Rows[i][0] = Info.MaxValue;
               //                  break;
               //              case 4:
               //                  dt_BrassStatistics.Rows[i][0] = Info.MinValue;
               //                  break;
               //          }

               //      }


               //     foreach (DataColumn col in dt_BrassStatistics.Columns)
               //     {
               //         if (col.Caption.ToLower() == "time") continue;
               //         double sMean = 0;
               //         double sVariance = 0;
               //         double sMaximum = 0;
               //         double sMinimum = 0;
               //         GetStatisticsByEele(BrassReportList, col.Caption, ref sMean, ref sVariance, ref sMaximum, ref sMinimum);
               //         dt_BrassStatistics.Rows[0][col.Caption] = col.Caption;// "统计"; 
               //         dt_BrassStatistics.Rows[1][col.Caption] = sMean.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());// "平均值";
               //         dt_BrassStatistics.Rows[2][col.Caption] = sVariance.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());// "方差";
               //         dt_BrassStatistics.Rows[3][col.Caption] = sMaximum.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());// "最大值";
               //         dt_BrassStatistics.Rows[4][col.Caption] = sMinimum.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());// "最小值";
               //     }
               // }


               //// report.RetestFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ((selectLong.Count > 1) ? ExcelTemplateParams.ManyTimeTemplate : ExcelTemplateParams.OneTimeTemplate);
               //// report.ReportPath = WorkCurveHelper.ExcelPath;
               //////report.GenerateRetestReport_Brass(reportName, BrassReportList, dt_BrassStatistics, true);



               //// if (!File.Exists(report.ReportPath + reportName + ".xls") && !File.Exists(report.ReportPath + reportName + "_Retry.xls"))
               ////     return;
               //// if (Skyray.Controls.SkyrayMsgBox.Show(PrintInfo.SaveSuccess + Skyray.EDX.Common.Info.OpenExcelOrNot, Skyray.EDX.Common.Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
               //// {
               ////     string excelOnePath = report.ReportPath + reportName + ".xls";
               ////     string excelManyPath = report.ReportPath + reportName + "_Retry" + ".xls";
               ////     //if (selectLong.Count == 1)
               ////     //    Help.ShowHelp(null, excelOnePath);
               ////     //else
               ////         Help.ShowHelp(null, excelManyPath);
               //// }

                #endregion

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
                else
                {
                    ExcelTemplateParams.GetExcelTemplateParams();
                    string reportName = GetDefineReportName();
                    //
                    Report report = new Report();
                    report.bProtect = false;

                    report.TempletFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.OneTimeTemplate;
                    report.ReportPath = WorkCurveHelper.ExcelPath;
                    report.Elements = FilterElementsByDGV(dgvHistoryRecord, selectLong.FirstOrDefault());//只能第一条有效数据导出
                    report.historyRecordid = selectLong.FirstOrDefault().ToString();


                    report.GenerateRepeaterReport(reportName, this.contextMenuStrip1, true, selectLong);

                    if (!File.Exists(report.ReportPath + reportName + ".xls") && !File.Exists(report.ReportPath + reportName + "_Retry.xls"))
                        return;
                    if (Skyray.Controls.SkyrayMsgBox.Show(PrintInfo.SaveSuccess + Skyray.EDX.Common.Info.OpenExcelOrNot, Skyray.EDX.Common.Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        string excelOnePath = report.ReportPath + reportName + ".xls";
                        string excelManyPath = report.ReportPath + reportName + "_Retry" + ".xls";
                        if (selectLong.Count == 1)
                            Help.ShowHelp(null, excelOnePath);
                        else
                            Help.ShowHelp(null, excelManyPath);
                    }
                    //
                }
                #endregion
                
            }
            else if (ExcelTemplateParams.iTemplateType == 9)
            {
                #region
                ExcelTemplateParams.GetExcelTemplateParams();
                Report report = new Report();
                ElementList elementList = ElementList.New;
                if (selectLong.Count > WorkCurveHelper.PrintExcelCount)
                {
                    Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
                    return;
                }
                else
                {
                    string workCurveName = string.Empty;
                    string WorkCurveID = string.Empty;
                    string historyRecordid = string.Empty;
                    SpecEntity spec = new SpecEntity();
                    SpecListEntity tempList = new SpecListEntity();
                    for (int i = 0; i < selectLong.Count; i++)
                    {
                        HistoryRecord record = HistoryRecord.FindById(selectLong[i]);
                        if (record == null)
                            continue;
                        WorkCurve workCurve = WorkCurve.FindById(record.WorkCurveId);
                        if (workCurve == null)
                            continue;

                        WorkCurveID = workCurve.Id.ToString();
                        workCurveName = workCurve.Name;
                        historyRecordid = selectLong[i].ToString();
                        report.ReadingNo = record.HistoryRecordCode;
                        if (!SeleWorkCurveNameList.Contains(workCurve.Name)) SeleWorkCurveNameList.Add(workCurve.Name);
                        tempList = DataBaseHelper.QueryByEdition(record.SpecListName,record.FilePath,record.EditionType);
                        if (tempList != null && tempList.Specs.Length > 0)
                        {
                            if (!string.IsNullOrEmpty(tempList.Specs[0].SpecData))
                                spec = tempList.Specs[0];
                        }
                        else
                        {
                            Msg.Show(Info.DataDelete);
                            return;
                        }
                        var elements = HistoryElement.Find(w => w.HistoryRecord.Id == record.Id);
                        foreach (var element in elements)
                        {
                            var temp = CurveElement.FindAll().Find(delegate(CurveElement curveElement) { return curveElement.Caption == element.elementName && curveElement.ElementList.WorkCurve.Id == workCurve.Id; });
                            if (temp == null)
                                continue;
                            double content = 0.0;
                            double.TryParse(element.contextelementValue, out content);
                            temp.Intensity = element.CaculateIntensity;
                            temp.Error = element.Error;
                            if (element.unitValue == 2)
                                temp.Error = temp.Error / 10000;
                            else
                                temp.Error = temp.Error / 10;

                            if (element.unitValue == 1)
                                temp.Content = content;
                            else if (element.unitValue == 2)
                                temp.Content = content / 10000;
                            else
                                temp.Content = content / 10;
                            elementList.Items.Add(temp);
                            report.historyStandID = element.customstandard_Id;
                        }
                    }
                    if (comboBoxCurveName.Text == "--All--" && SeleWorkCurveNameList.Count > 1)
                    {
                        Msg.Show(Info.strSeleWorkCurveName);
                        return;
                    }
                    #region
                    foreach (ToolStripMenuItem tempItem in this.contextMenuStrip1.Items)
                    {
                        if (!tempItem.Checked && Atoms.AtomList.ToList().Find(w => w.AtomName == tempItem.Name) != null)
                        {
                            CurveElement tempFind = elementList.Items.ToList().Find(w => w.Caption == tempItem.Name);
                            if (tempFind != null)
                                elementList.Items.Remove(tempFind);
                        }
                        else if (tempItem.Checked && Atoms.AtomList.ToList().Find(w => w.AtomName == tempItem.Name) != null)
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
                        return;
                    }
                    #endregion
                    report.Spec = spec;
                    report.specList = tempList;
                    report.operateMember = FrmLogon.userName;
                    report.Elements = elementList;
                    var eles = elementList.Items.ToList().OrderBy(w => w.RowsIndex);
                    elementList.Items.Clear();
                    eles.All(w => { elementList.Items.Add(w); return true; });
                    report.WorkCurveName = workCurveName;
                    report.WorkCurveID = WorkCurveID;
                    report.historyRecordid = historyRecordid;
                    report.FirstContIntr.Add(elementList.Items.Count);
                    string reportName = GetDefineReportName();
                    //


                    if (selectLong.Count == 1)
                    {
                        report.InterestElemCount = elementList.Items.Count;
                        report.TempletFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.OneTimeTemplate;
                        report.ReportPath = WorkCurveHelper.ExcelPath;
                        report.GenerateReport(reportName, true);
                        //
                    }
                    else
                    {
                        DataTable dt = CreateReTestTable(elementList);
                        int cont = 0;
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
                                        if (element.unitValue == 1)
                                            Value = Value * 10000;
                                        else if (element.unitValue == 3)
                                            Value = Value * 1000;

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
                                }
                                else if (element == null && column.Visible && dt.Columns.Contains(column.HeaderText))//无数据时处理方式
                                {
                                    rowNew[column.HeaderText] = default(double).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
                                }
                            }                            
                            dt.Rows.Add(rowNew);                            
                        }

                        bool flag =false;
                        bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestStatistics"),out flag);                         
                        if (flag)
                        AddStaticsRows(dt, "time"); 

                        report.InterestElemCount = elementList.Items.Count;                  
                        report.RetestFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.ManyTimeTemplate;
                        report.ReportPath = WorkCurveHelper.ExcelPath;
                        report.GenerateRetestReport(reportName, dt, true);
                        //
                    }
                    if (!File.Exists(report.ReportPath + reportName + ".xls") && !File.Exists(report.ReportPath + reportName + "_Retry.xls"))
                        return;
                    if (Skyray.Controls.SkyrayMsgBox.Show(PrintInfo.SaveSuccess + Skyray.EDX.Common.Info.OpenExcelOrNot, Skyray.EDX.Common.Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        string excelOnePath = report.ReportPath + reportName + ".xls";
                        string excelManyPath = report.ReportPath + reportName + "_Retry" + ".xls";
                        if (selectLong.Count == 1)
                            Help.ShowHelp(null, excelOnePath);
                        else
                            Help.ShowHelp(null, excelManyPath);
                    }
                    //
                }
                #endregion
            }
            else if(ExcelTemplateParams.iTemplateType == 11)
            {
                #region
                ExcelTemplateParams.GetExcelTemplateParams();
                Report report = new Report();
                ElementList elementList = ElementList.New;
                if (selectLong.Count > WorkCurveHelper.PrintExcelCount)
                {
                    Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
                    return;
                }
                else
                {
                    string workCurveName = string.Empty;
                    string WorkCurveID = string.Empty;
                    string historyRecordid = string.Empty;
                    SpecEntity spec = new SpecEntity();
                    SpecListEntity tempList = new SpecListEntity();
                    for (int i = 0; i < selectLong.Count; i++)
                    {
                        elementList = ElementList.New;
                        HistoryRecord record = HistoryRecord.FindById(selectLong[i]);
                        if (record == null)
                            continue;
                        WorkCurve workCurve = WorkCurve.FindById(record.WorkCurveId);
                        if (workCurve == null)
                            continue;

                        WorkCurveID = workCurve.Id.ToString();
                        workCurveName = workCurve.Name;
                        historyRecordid = selectLong[i].ToString();
                        report.ReadingNo = record.HistoryRecordCode;
                        if (!SeleWorkCurveNameList.Contains(workCurve.Name)) SeleWorkCurveNameList.Add(workCurve.Name);
                        tempList = DataBaseHelper.QueryByEdition(record.SpecListName,record.FilePath,record.EditionType);
                        if (tempList != null && tempList.Specs.Length > 0)
                        {
                            if (!string.IsNullOrEmpty(tempList.Specs[0].SpecData))
                                spec = tempList.Specs[0];
                            //for (int iSpec = 1; iSpec < tempList.Specs.Count; iSpec++)
                            //{
                            //    for (int iSpecData = 0; iSpecData < tempList.Specs[iSpec].SpecDatas.Length; iSpecData++)
                            //    {
                            //        spec.SpecDatas[iSpecData] += tempList.Specs[iSpec].SpecDatas[iSpecData];//叠加谱数据
                            //    }
                            //}
                        }
                        else
                        {
                            Msg.Show(Info.DataDelete);
                            return;
                        }
                        var elements = HistoryElement.Find(w => w.HistoryRecord.Id == record.Id);
                        
                        foreach (var element in elements)
                        {
                            var temp = CurveElement.FindAll().Find(delegate(CurveElement curveElement) { return curveElement.Caption == element.elementName && curveElement.ElementList.WorkCurve.Id == workCurve.Id; });
                            if (temp == null)
                                continue;
                            double content = 0.0;
                            double.TryParse(element.contextelementValue, out content);
                            temp.Intensity = element.CaculateIntensity;
                            temp.Error = element.Error;
                            //if (element.unitValue == 2)
                            //    temp.Error = temp.Error / 10000;
                            //else
                            //    temp.Error = temp.Error / 10;

                            //if (element.unitValue == 1)
                            //    temp.Content = content;
                            //else if (element.unitValue == 2)
                            //    temp.Content = content / 10000;
                            //else
                            //    temp.Content = content / 10;
                            temp.Content = content;
                            elementList.Items.Add(temp);
                            report.historyStandID = element.customstandard_Id;
                        }
                    }
                    if (comboBoxCurveName.Text == "--All--" && SeleWorkCurveNameList.Count > 1)
                    {
                        Msg.Show(Info.strSeleWorkCurveName);
                        return;
                    }
                    #region
                    foreach (ToolStripMenuItem tempItem in this.contextMenuStrip1.Items)
                    {
                        if (!tempItem.Checked && Atoms.AtomList.ToList().Find(w => w.AtomName == tempItem.Name) != null)
                        {
                            CurveElement tempFind = elementList.Items.ToList().Find(w => w.Caption == tempItem.Name);
                            if (tempFind != null)
                                elementList.Items.Remove(tempFind);
                        }
                        else if (tempItem.Checked && Atoms.AtomList.ToList().Find(w => w.AtomName == tempItem.Name) != null)
                        {
                            CurveElement tempFind =elementList.Items.ToList().Find(w => w.Caption == tempItem.Name);
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
                        return;
                    }
                    #endregion
                    report.Spec = spec;
                    report.specList = tempList;
                    report.operateMember = FrmLogon.userName;
                    var lst = elementList.Items.ToList().OrderBy(w => w.RowsIndex);
                    report.Elements = elementList;
                    report.Elements.Items.Clear();
                    lst.ToList().ForEach(w => report.Elements.Items.Add(w));
                    report.WorkCurveName = workCurveName;
                    report.WorkCurveID = WorkCurveID;
                    report.historyRecordid = historyRecordid;
                    report.FirstContIntr.Add(elementList.Items.Count);
                    report.selectLong = selectLong;
                    string reportName = GetDefineReportName();
                    //


                    //if (selectLong.Count == 1)
                    //{
                    //    report.InterestElemCount = elementList.Items.Count;
                    //    report.TempletFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.OneTimeTemplate;
                    //    report.ReportPath = WorkCurveHelper.ExcelPath;
                    //    report.GenerateReport(reportName, true);
                    //    //
                    //}
                    //else
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
                                        //if (element.unitValue == 1)
                                        //    Value = Value * 10000;
                                        //else if (element.unitValue == 3)
                                        //    Value = Value * 1000;

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
                                        //    rowNew[column.HeaderText] = (Value / 1000).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + "(‰)";
                                        //}
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

                        bool flag = false;
                        bool.TryParse(ReportTemplateHelper.LoadSpecifiedValue("Report/CommonReport", "ReTestStatistics"), out flag);

                        if (flag && selectLong.Count > 1)
                        {
                            AddStaticsRows(dt, "time");
                            dt.Rows.RemoveAt(dt.Rows.Count - 3);
                            dt.Rows.RemoveAt(dt.Rows.Count - 3);
                            dt.Rows.RemoveAt(dt.Rows.Count - 3);
                        }
                        report.InterestElemCount = elementList.Items.Count;
                        report.RetestFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.OneTimeTemplate;
                        report.ReportPath = WorkCurveHelper.ExcelPath;
                        report.GenerateRetestReport(reportName, dt, true);
                        //
                    }
                    if (!File.Exists(report.ReportPath + reportName + ".xls") && !File.Exists(report.ReportPath + reportName + "_Retry.xls"))
                        return;
                    //if ((sender as System.Windows.Forms.Button) != null)
                    if (OnPrintTemplate == null)
                    {
                        if (Skyray.Controls.SkyrayMsgBox.Show(PrintInfo.SaveSuccess + Skyray.EDX.Common.Info.OpenExcelOrNot, Skyray.EDX.Common.Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                        {
                            string excelOnePath = report.ReportPath + reportName + ".xls";
                            string excelManyPath = report.ReportPath + reportName + "_Retry" + ".xls";
                            //if (selectLong.Count == 1)
                            //    Help.ShowHelp(null, excelOnePath);
                            //else
                            Help.ShowHelp(null, excelManyPath);
                        }
                    }
                    //else
                    //{
                    //    var send = sender as string[];
                    //    if (send !=null)
                    //    send[0] = File.Exists(report.ReportPath + reportName + ".xls") ? 
                    //        report.ReportPath + reportName + ".xls" : report.ReportPath + reportName + "_Retry.xls";
                    //}
                    else
                    {
                        OnPrintTemplate(File.Exists(report.ReportPath + reportName + ".xls") ? 
                            report.ReportPath + reportName + ".xls" : report.ReportPath + reportName + "_Retry.xls");
                    }
                    //
                }
                #endregion
            }
            else if (ExcelTemplateParams.iTemplateType == 12)
            {
                Report report = new Report();
                report.Elements = FilterElementsByDGV(dgvHistoryRecord, selectLong.FirstOrDefault());
                report.InterestElemCount = report.Elements.Items.Count;
                report.historyRecordid = selectLong.FirstOrDefault().ToString();
                var reportName = GetDefineReportName();
                string sFileTemplateName = AppDomain.CurrentDomain.BaseDirectory + "/HistoryExcelTemplate/" +
                    (selectLong.Count > 1 ? ExcelTemplateParams.ManyTimeTemplate : ExcelTemplateParams.OneTimeTemplate);
                string sReturn = report.GenerateGenericReport(sFileTemplateName, reportName, selectLong);

                if (File.Exists(sReturn) &&
                    Skyray.Controls.SkyrayMsgBox.Show(PrintInfo.SaveSuccess + Skyray.EDX.Common.Info.OpenExcelOrNot, Skyray.EDX.Common.Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    Help.ShowHelp(null, sReturn);
                }
            }
            else
            {
                #region
                ExcelTemplateParams.GetExcelTemplateParams();
                Report report = new Report();
                report.isShowND = WorkCurveHelper.isShowND;
                ElementList elementList = ElementList.New;
                if (selectLong.Count > WorkCurveHelper.PrintExcelCount)
                {
                    Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
                    return;
                }
                else
                {
                    //何晓明 20100715 Rohs4模板多语言切换
                    ExcelTemplateParams.GetExcelTemplateParams();
                    string workCurveName = string.Empty;
                    string WorkCurveID = string.Empty;
                    string historyRecordid = string.Empty;
                    SpecEntity spec = new SpecEntity();
                    SpecListEntity tempList = new SpecListEntity();
                    for (int i = 0; i < selectLong.Count; i++)
                    {
                        HistoryRecord record = HistoryRecord.FindById(selectLong[i]);
                        if (record == null)
                            continue;
                        WorkCurve workCurve = WorkCurve.FindById(record.WorkCurveId);
                        if (workCurve == null)
                            continue;

                        WorkCurveID = workCurve.Id.ToString();
                        workCurveName = workCurve.Name;
                        historyRecordid = selectLong[i].ToString();
                        report.ReadingNo = record.HistoryRecordCode;
                        if (!SeleWorkCurveNameList.Contains(workCurve.Name)) SeleWorkCurveNameList.Add(workCurve.Name);
                        tempList = DataBaseHelper.QueryByEdition(record.SpecListName,record.FilePath,record.EditionType);
                        if (tempList != null && tempList.Specs.Length > 0)
                        {
                            if (!string.IsNullOrEmpty(tempList.Specs[0].SpecData))
                                spec = tempList.Specs[0];
                        }
                        else
                        {
                            Msg.Show(Info.DataDelete);
                            return;
                        }
                        var elements = HistoryElement.Find(w => w.HistoryRecord.Id == record.Id);
                        foreach (var element in elements)
                        {
                            //var temp = CurveElement.FindAll().Find(delegate(CurveElement curveElement) { return curveElement.Caption == element.elementName && curveElement.ElementList.WorkCurve.Id == workCurve.Id; });
                            //CurveElement temp = CurveElement.Find(w => w.Formula == element.elementName && w.ElementList.WorkCurve.Id == workCurve.Id).FirstOrDefault();
                            CurveElement temp = CurveElement.Find(w => w.Formula == element.elementName).Find(w => w.ElementList.WorkCurve.Id == workCurve.Id);
                            if (temp == null)
                                continue;
                            double content = 0.0;
                            double.TryParse(element.contextelementValue, out content);
                            temp.Intensity = element.CaculateIntensity;
                            temp.Error = element.Error;
                            if (element.unitValue == 2)
                                temp.Error = temp.Error / 10000;
                            else
                                temp.Error = temp.Error / 10;

                            if (element.unitValue == 1)
                                temp.Content = content;
                            else if (element.unitValue == 2)
                                temp.Content = content / 10000;
                            else
                                temp.Content = content / 10;
                            elementList.Items.Add(temp);
                            report.historyStandID = element.customstandard_Id;
                        }
                    }
                    if (comboBoxCurveName.Text == "--All--" && SeleWorkCurveNameList.Count > 1)
                    {
                        Msg.Show(Info.strSeleWorkCurveName);
                        return;
                    }
                    #region
                    foreach (ToolStripMenuItem tempItem in this.contextMenuStrip1.Items)
                    {
                        if (!tempItem.Checked && Atoms.AtomList.ToList().Find(w => w.AtomName == tempItem.Name) != null)
                        {
                            CurveElement tempFind = elementList.Items.ToList().Find(w => w.Caption == tempItem.Name);
                            if (tempFind != null)
                                elementList.Items.Remove(tempFind);
                        }
                        else if (tempItem.Checked && Atoms.AtomList.ToList().Find(w => w.AtomName == tempItem.Name) != null)
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
                        return;
                    }
                    #endregion
                    report.Spec = spec;
                    report.specList = tempList;
                    report.operateMember = FrmLogon.userName;
                    report.Elements = elementList;
                    var eles = report.Elements.Items.ToList().OrderBy(w => w.RowsIndex);//获取一个排序的感兴趣元素深度拷贝
                    elementList.Items.Clear();//清除感兴趣元素 引用 报表中一起清除
                    eles.All(w => { elementList.Items.Add(w); return true; });//添加感兴趣元素
                    report.WorkCurveName = workCurveName;
                    report.WorkCurveID = WorkCurveID;
                    report.historyRecordid = historyRecordid;
                    report.FirstContIntr.Add(elementList.Items.Count);
                    //修改：何晓明 20111129 报告命名设置 赋值设置名称
                    //string nowTime = DateTime.Now.ToString("yyyyMMddhhmmss");
                    string reportName = GetDefineReportName();
                    //
                    report.bProtect = false;
                    if (selectLong.Count == 1)
                    {
                        report.InterestElemCount = elementList.Items.Count;
                        report.TempletFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.OneTimeTemplate;
                        report.ReportPath = WorkCurveHelper.ExcelPath;
                        //修改：何晓明 20111129 报告命名设置 赋值设置名称
                        //report.GenerateReport(nowTime, true);
                        report.GenerateReport(reportName, true);
                        //
                    }
                    else
                    {
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
                                        if (Value <= WorkCurveHelper.NDValue)
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
                        report.InterestElemCount = elementList.Items.Count;
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
                        //修改：何晓明 20111129 报告命名设置 赋值设置名称
                        report.GenerateRetestReport(reportName, dt, true);
                        //report.GenerateRetestReport(nowTime, dt, true);
                        //
                    }
                    ////修改：何晓明 20110715 按模板导出打开Excel
                    //var pProcess = System.Diagnostics.Process.GetProcessesByName("Excel");
                    //if (pProcess != null && pProcess.Length > 0) try { pProcess[0].Kill(); }
                    //    catch { };
                    //
                    //修改：何晓明 20110715 按模板导出打开Excel
                    //Msg.Show(Info.ExportSuccessOld);
                    if (!File.Exists(report.ReportPath + reportName + ".xls") && !File.Exists(report.ReportPath + reportName + "_Retry.xls"))
                        return;
                    if (Skyray.Controls.SkyrayMsgBox.Show(PrintInfo.SaveSuccess + Skyray.EDX.Common.Info.OpenExcelOrNot, Skyray.EDX.Common.Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        //修改：何晓明 20111129 报告命名设置 赋值设置名称
                        string excelOnePath = report.ReportPath + reportName + ".xls";
                        string excelManyPath = report.ReportPath + reportName + "_Retry" + ".xls";
                        //string excelOnePath = report.ReportPath + nowTime + "_" + report.Spec.SpecList.SampleName + ".xls";
                        //string excelManyPath = report.ReportPath + nowTime + "_" + report.Spec.SpecList.SampleName + "_Retry" + ".xls";
                        //
                        if (selectLong.Count == 1)
                            Help.ShowHelp(null, excelOnePath);
                        else
                            Help.ShowHelp(null, excelManyPath);
                    }
                    //
                }
                #endregion
            }
        }

        private List<DataRow> AddStaticsRows(DataTable dt,string HeadColumnName)
        {
            return DifferenceDevice.interClassMain.AddStaticsRows(dt, HeadColumnName);
        }

        private void GetStatisticsByEele(List<BrassReport> BrassReportList, string sEelName,
            ref double sMean, ref double sVariance, ref double sMaximum, ref double sMinimum)
        {
            sMaximum = 0;
            sMinimum = -1;
            double dEelContent=0;
            foreach (BrassReport brassReport in BrassReportList)
            {
                if (!brassReport.dt_Brass.Columns.Contains(sEelName)) continue;
                dEelContent = double.Parse((brassReport.dt_Brass.Rows[2][sEelName] == null || brassReport.dt_Brass.Rows[2][sEelName].ToString() == "") ? "0" : brassReport.dt_Brass.Rows[2][sEelName].ToString());
                sMean+=dEelContent;
                if (dEelContent > sMaximum) sMaximum = dEelContent;
                if (dEelContent < sMinimum || sMinimum == -1) sMinimum = dEelContent;

            }

            sMean /= BrassReportList.Count;


            sMean = Math.Round(sMean, 4);
            double powTotal = 0;
            foreach (BrassReport brassReport in BrassReportList)
            {
                if (!brassReport.dt_Brass.Columns.Contains(sEelName)) continue;
                dEelContent = double.Parse((brassReport.dt_Brass.Rows[2][sEelName] == null || brassReport.dt_Brass.Rows[2][sEelName].ToString() == "") ? "0" : brassReport.dt_Brass.Rows[2][sEelName].ToString());
                powTotal += Math.Pow((dEelContent - sMean), 2);

            }
            sVariance = Math.Sqrt(powTotal / (BrassReportList.Count - 1));
        }

        private DataTable SetColumns(ElementList elementList)
        {
            DataTable reTestTable = new DataTable();
            reTestTable.Columns.Clear();
            reTestTable.Columns.Add("Time", typeof(string));
            foreach (CurveElement curele in elementList.Items.ToList().OrderBy(d => d.RowsIndex).ToList())
                reTestTable.Columns.Add(curele.Caption, typeof(string));           

            return reTestTable;

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
            SpecListEntity sl = DataBaseHelper.QueryByEdition(hr.SpecListName,hr.FilePath,hr.EditionType);
            string reportName = DifferenceDevice.interClassMain.GetDefineReportName(sl, wc,selectLong.FirstOrDefault());
            return reportName;
        }

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
            foreach(CurveElement curEle in elementList.Items)
            {
                var header = curEle.IsOxide ? curEle.Formula : curEle.Caption;
                if(!reTestTable.Columns.Contains(header))
                    reTestTable.Columns.Add(header, typeof(string));
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
                    row["TestResult"] = ExcelTemplateParams.TestRetult(row["HistoryRecord_Id"].ToString());// DifferenceDevice.interClassMain.TestRetult(row["HistoryRecord_Id"].ToString()); //InterfaceClass.TestRetult(row["HistoryRecord_Id"].ToString());
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
            
            string excelModeType = ReportTemplateHelper.LoadSpecifiedValue("Excel", "IsOurExcel");
            GetSetColmun();
            string strSampleNameAndTestDate = (this.checkBoxWSampleName.Checked) ? " and SampleName like '%" + this.textBoxWInputName.Text.ToString() + "%'" : "";
            strSampleNameAndTestDate += (this.checkBoxWDate.Checked) ? " and datetime(specdate)>='" + this.dateTimePickerStart.Value.ToString("yyyy-MM-dd HH:mm:ss") + "' and datetime(specdate)<'" + this.dateTimePickerEnd.Value.ToString("yyyy-MM-dd HH:mm:ss") + "'" : "";
            strSampleNameAndTestDate += (this.chkByStatus.Checked) ? (this.cboStatus.Text == "--All--" ? "" : " and StockStatus=" + ((MyItem)this.cboStatus.SelectedItem).Value + "") : "";
            bool IsShowArea = false;
            bool IsShowError = false;
            bool IsShowIntensity = false;
            bool IsShowUnitColumns = false;
            bool IsShowAverageColumns = false;
            bool IsShowElementAllName = false;
            bool IsOnlyShowScanRecord = false;
            XmlDocument doc = new XmlDocument();
            string path = Application.StartupPath + "\\AppParams.xml";
            if (File.Exists(path))
            {
                doc.Load(path);
                XmlNodeList nodeList = doc.SelectNodes("application/OpenHistoryRecordType");
                foreach (XmlNode node in nodeList)
                {
                    IsShowArea = node.SelectSingleNode("IsShowArea").InnerText == "0" ? false : true;
                    IsShowError = node.SelectSingleNode("IsShowError").InnerText == "0" ? false : true;
                    IsShowIntensity = node.SelectSingleNode("IsShowIntensity").InnerText == "0" ? false : true;
                    IsShowUnitColumns = node.SelectSingleNode("IsShowUnitColumns").InnerText == "0" ? false : true;
                    IsShowAverageColumns = node.SelectSingleNode("IsShowAverageColumns").InnerText == "0" ? false : true;
                    IsShowElementAllName = node.SelectSingleNode("IsShowElementAllName").InnerText == "0" ? false : true;
                    IsOnlyShowScanRecord = node.SelectSingleNode("IsOnlyShowScanRecord").InnerText == "0" ? false : true; 
                }
            }

            string strSql = " select elementname,standarddata.StandardContent,c.layernumber,e.atomnameen from historyelement " +
                            " left outer join customstandard on customstandard.id=historyelement.customstandard_Id "+ 
                            " left outer join standarddata  on standarddata.ElementCaption=historyelement.elementName and  standarddata.CustomStandard_Id=customstandard.id "+
                            //氧化物排序问题
                            " left outer join oxide on oxide.oxidename=historyelement.elementname left outer join Atom d on d.atomid=oxide.Atom_Id "+
                            " left outer join Atom e on e.atomname=historyelement.elementName " +
                            //当为FPThick时，获取各元素所在的层
                            " left outer join  (   select * from curveelement where elementlist_id in ( " +
                            " select id from elementlist where 1=1 " + ((comboBoxCurveName.Text == "--All--") ? " and workcurve_id in (select id from workcurve where condition_id in ( " +
           " select id from condition where device_id=" + WorkCurveHelper.DeviceCurrent.Id + "))" : " and workcurve_id in (select ID from  workcurve where  name='" + comboBoxCurveName.Text + "' and condition_id in (select id from condition where device_id=" + WorkCurveHelper.DeviceCurrent.Id + "))") + " ) ) c on " + ((DifferenceDevice.IsXRF) ? "c.caption=case  when d.AtomName isnull then historyelement.elementName else d.AtomName end " : "c.caption=historyelement.elementName") +//氧化物排序问题

                            " where HistoryRecord_Id in ( select Id from HistoryRecord where 1=1  " + ((comboBoxCurveName.Text == "--All--") ? " and workcurveid in (select id from workcurve where condition_id in ( " +
           " select id from condition where device_id=" + WorkCurveHelper.DeviceCurrent.Id + "))" : " and WorkCurveId  in (select ID from  workcurve where  name='" + comboBoxCurveName.Text + "'  and condition_id in (select id from condition where device_id=" + WorkCurveHelper.DeviceCurrent.Id + "))") + strSampleNameAndTestDate + " ) group by elementname order by  case when c.rowsindex is null then 99+e.AtomID else c.rowsindex end ";
            

            DataTable dt_historyelement = GetData(strSql);//获取所有历史记录元素 及感兴趣元素
            string strPassReslt = "";
            if (DifferenceDevice.IsRohs && excelModeType == "1" && sType != 0 && strSelectID != "")
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
            CompanyOthersInfoList = CompanyOthersInfo.FindBySql("select * from companyothersinfo where display=1 and ExcelModeType='" + ExcelTemplateParams.iTemplateType.ToString() + "'");

            string strComOtherInfo="";
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

            List<string> layernumberList = new List<string>();//当为FPThick时，存储已经选择的层
            strSql = "select " + strComOtherInfo + "a.HistoryRecordCode,";
            if (DifferenceDevice.IsRohs) strSql += " '' as TestResult, ";
            if (DifferenceDevice.IsRohs && IsShowUnitColumns) strSql += " 'ppm' as Unit, ";
            
            strSql+= " (select Name from workcurve where id=a.workcurveid) as WorkCurveName, " +
                " a.Id,a.SampleName,a.SpecListName,a.Supplier,round(a.Weight,"+
                WorkCurveHelper.SoftWareContentBit.ToString() + ") as Weight,a.Shape,a.Operater,a.SpecDate,a.SpecSummary,a.WorkCurveId,a.DeviceName,a.CaculateTime,cast(a.ActualVoltage as int) as ActualVoltage,cast(a.ActualCurrent as int) as ActualCurrent,cast(a.CountRate as int) as CountRate,a.PeakChannel,(case when a.StockStatus = 0 then '" + Info.Extraction + "' else '" + Info.Storage + "' end) as StockStatus,cast(a.Resole as int) as Resole,a.TotalCount,b.*" + 
                strPassReslt + strAddColmun + " from historyrecord a left join (select HistoryRecord_Id ";

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

                //Thick中，同层元素合并为一列，含量一定要显示在厚度前
                IEnumerable<IGrouping<string, DataRow>> result = dt_historyelement.Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["layernumber"].ToString());//按layernumber分组
                foreach (IGrouping<string, DataRow> ig in result)
                {

                    if (ig.Key == "") continue;

                    string elementName = "";
                    string strelementname = "";

                    //显示每个元素的含量
                    foreach (var dr in ig)
                    {
                        elementName = dr["elementname"].ToString();
                        strelementname += dr["elementname"].ToString() + "|";
                        //如果编辑元素中，勾选显示含量，则显示
                        if (seleWorkCurve != null && seleWorkCurve.IsThickShowContent)
                            strSql += " ,max(case when [elementname] =" + "'" + dr["elementname"].ToString() + "'" + " then round(cast ([contextelementvalue] as float)," + WorkCurveHelper.ContentBit.ToString() + ")  else  round(cast ('0' as float)," + WorkCurveHelper.ContentBit.ToString() + ") end) " +
                                      " ||max(case when [elementname] =" + "'" + dr["elementname"].ToString() + "'" + " then (case when [unitValue]=1 then " + Convert.ToString((checkShowUnit.Checked) ? "' (%)'" : "''") + " else " + Convert.ToString((checkShowUnit.Checked) ? "' (ppm)'" : "''") + " end ) else 0 end)" +
                                     " as " + "'" + dr["elementname"].ToString() + Info.EditContent + "'";

                        if (IsShowArea)
                            strSql += " ,max(case when [elementname] =" + "'" + dr["elementname"].ToString() + "'" + " then   round([ElementIntensity]," + WorkCurveHelper.ContentBit.ToString() + ")  else 0 end) as " + "'" + dr["elementname"].ToString() + Info.strArea + "'";
                        if (IsShowError)
                            strSql += " ,max(case when [elementname] =" + "'" + dr["elementname"].ToString() + "'" + " then   round([Error]," + WorkCurveHelper.ContentBit.ToString() + ")  else 0 end) as " + "'" + dr["elementname"].ToString() + Info.strError + "'";
                        if (IsShowIntensity)
                            strSql += " ,max(case when [elementname] =" + "'" + dr["elementname"].ToString() + "'" + " then   round([CaculateIntensity]," + WorkCurveHelper.ContentBit.ToString() + ")  else 0 end) as " + "'" + dr["elementname"].ToString() + Info.Intensity + "'";
                        if (IsShowAverageColumns)
                            strSql += " ,max(case when [elementname] =" + "'" + dr["elementname"].ToString() + "'" + " then   round([AverageValue]," + WorkCurveHelper.ContentBit.ToString() + ")  else 0 end) as " + "'" + dr["elementname"].ToString() + Info.MeanValue + "'";

                    }
                    //显示每一层的厚度
                    strelementname = strelementname.Substring(0, strelementname.Length-1);
                    strSql += " ,max(case when [elementname] =" + "'" + elementName + "'" + " then round(cast ([thickelementvalue] as float)," + WorkCurveHelper.ThickBit.ToString() + ")  else  round(cast ('0' as float)," + WorkCurveHelper.ThickBit.ToString() + ") end)  " +
                                  " ||max(case when [elementname] =" + "'" + elementName + "'" + " then (case when [thickunitValue]=1 then " + Convert.ToString((checkShowUnit.Checked) ? "' (u〞)'" : "''") + " else " + Convert.ToString((checkShowUnit.Checked) ? "' (um)'" : "''") + " end ) else 0 end)" +
                                 "as " + "'" + strelementname + Info.Thick + "'";
                } 
            }
            else
            {
                foreach (DataRow row in dt_historyelement.Rows)
                {

                    string elementname = (IsShowElementAllName && DifferenceDevice.IsAnalyser) ? row["atomnameen"].ToString() : row["elementname"].ToString();

                    if (DifferenceDevice.IsRohs)
                    {
                        if (WorkCurveHelper.isShowND)
                            strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + " then   (case when [unitValue]=1 then (case when (cast([contextelementvalue] as float)*10000.0)<" + WorkCurveHelper.NDValue.ToString() + "  then 'ND' else round(cast ([contextelementvalue] as float)," + WorkCurveHelper.SoftWareContentBit.ToString() + ") end)||" + Convert.ToString((checkShowUnit.Checked) ? "' (%)'" : "''") + " else (case when cast([contextelementvalue] as float)<" + WorkCurveHelper.NDValue.ToString() + "  then 'ND' else round(cast ([contextelementvalue] as float)," + WorkCurveHelper.SoftWareContentBit.ToString() + ") end ) ||" + Convert.ToString((checkShowUnit.Checked) ? "' (ppm)'" : "''") + "  END)  else '' end) as " + "'" + elementname + ((excelModeType == "1" && sType != 0) ? Info.EditContent + "(" + Info.strStandard + row["StandardContent"].ToString() + "ppm)" : "") + "'";
                        else
                            strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + " then   (case when [unitValue]=1 then round(cast ([contextelementvalue] as float)," + WorkCurveHelper.SoftWareContentBit.ToString() + ")||" + Convert.ToString((checkShowUnit.Checked) ? "' (%)'" : "''") + " else round(cast ([contextelementvalue] as float)," + WorkCurveHelper.SoftWareContentBit.ToString() + ") ||" + Convert.ToString((checkShowUnit.Checked) ? "' (ppm)'" : "''") + "  END)  else '' end) as " + "'" + elementname + ((excelModeType == "1" && sType != 0) ? Info.EditContent + "(" + Info.strStandard + row["StandardContent"].ToString() + "ppm)" : "") + "'";

                        strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + 
                            " then   (case when [unitValue]=1 then (case when (cast([contextelementvalue] as float)*10000.0)>cast([StandardContent] as float) then 'true' else '' end) else     " +
                           " case when cast([contextelementvalue] as float)>cast([StandardContent] as float) then 'true' else '' end" +
                           " end) else '' end) as " + "'" + elementname + "_Color" + "'";

                    }
                    else
                    {
                        strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + 
                                  " then   round(cast ([contextelementvalue] as float)," + WorkCurveHelper.SoftWareContentBit.ToString() + ")  else  round(cast ('0' as float)," + 
                                  WorkCurveHelper.SoftWareContentBit.ToString() + ") end) " +
                                " ||max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + 
                                " then (case when [unitValue]=1 then " + Convert.ToString((checkShowUnit.Checked) ? "' (%)'" : "''") +
                                " when [unitValue] = 2 then " + Convert.ToString((checkShowUnit.Checked) ? "' (ppm)'" : "''") +
                                " when [unitValue] = 3 then " + Convert.ToString((checkShowUnit.Checked) ? "' (‰)'" : "''") + " end ) else 0 end)" +
                                " as " + "'" + elementname + "'";
                    }

                    if (IsShowArea)
                        strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + " then   round([ElementIntensity]," + WorkCurveHelper.SoftWareContentBit.ToString() + ")  else 0 end) as " + "'" + elementname + Info.strArea + "'";
                    if (IsShowError)
                        strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + " then   round([Error]," + WorkCurveHelper.SoftWareContentBit.ToString() + ")  else 0 end) as " + "'" + elementname + Info.strError + "'";
                    if (IsShowIntensity)
                        strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + " then   round([CaculateIntensity]," + WorkCurveHelper.SoftWareContentBit.ToString() + ")  else 0 end) as " + "'" + elementname + Info.Intensity + "'";
                    if(IsShowAverageColumns)
                        strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + " then   round([AverageValue]," + WorkCurveHelper.SoftWareContentBit.ToString() + ")  else 0 end) as " + "'" + elementname + Info.MeanValue + "'";
                }
            }

            string strCondition = (IsOnlyShowScanRecord)? " and a.IsScan=1 ":"";
            switch (sType)
            {
                case 0:
                    strCondition += "  order by Id desc limit " + ROWS_PER_PAGE + "  offset " + Convert.ToString((currentPage < 0) ? 0 : currentPage * ROWS_PER_PAGE);
                    break;
                case 1:
                    if (strSelectID!="")
                    strCondition += "  and id in (" + strSelectID + ")";
                    break;
            }
            strSql += "from historyelement a " +
                      " left outer join standarddata b on b.CustomStandard_Id=a.customstandard_Id and b.ElementCaption=a.elementName " +
                      " where a.historyrecord_id in (select id from historyrecord a where 1= 1  " + ((comboBoxCurveName.Text == "--All--") ? " and a.workcurveid in (select id from workcurve where condition_id in ( " +
           " select id from condition where device_id=" + WorkCurveHelper.DeviceCurrent.Id + "))" : " and  a.WorkCurveId   in (select ID from  workcurve where  name='" + comboBoxCurveName.Text + "'  and condition_id in (select id from condition where device_id=" + WorkCurveHelper.DeviceCurrent.Id + "))") + strSampleNameAndTestDate + strCondition + ")" +
                      " group by HistoryRecord_Id) b on b.HistoryRecord_Id=a.id where 1= 1 " + ((comboBoxCurveName.Text == "--All--") ? " and a.workcurveid in (select id from workcurve where condition_id in ( " +
           " select id from condition where device_id=" + WorkCurveHelper.DeviceCurrent.Id + "))" : " and  a.WorkCurveId   in (select ID from  workcurve where  name='" + comboBoxCurveName.Text + "'  and condition_id in (select id from condition where device_id=" + WorkCurveHelper.DeviceCurrent.Id + "))") + strSampleNameAndTestDate + strCondition;


            if (sType == 0)
                strSql += " ; select count(*) from historyrecord a  where 1= 1 " + ((comboBoxCurveName.Text == "--All--") ? " and a.workcurveid in (select id from workcurve where condition_id in ( " +
           " select id from condition where device_id=" + WorkCurveHelper.DeviceCurrent.Id + "))" : " and  a.WorkCurveId   in (select ID from  workcurve where  name='" + comboBoxCurveName.Text + "'  and condition_id in (select id from condition where device_id=" + WorkCurveHelper.DeviceCurrent.Id + "))") + strSampleNameAndTestDate + ((IsOnlyShowScanRecord) ? " and a.IsScan=1 " : "");
            return strSql;


        }

        #region 获取公司其他信息
        private XmlNodeList xmlnodelist;
        private void GetCombInfo()
        {
            string sReportPath = AppDomain.CurrentDomain.BaseDirectory + "//printxml//CompanyInfo.xml";
            XmlDocument xmlDocReport = new XmlDocument();
            xmlDocReport.Load(sReportPath);

            string Language = "";
            Languages CurrentLang = Languages.FindOne(l => l.IsCurrentLang == true);
            Language = CurrentLang.ShortName;
            if (Language.ToLower() == "english") Language = "EN";
            else if (Language.ToLower() == "chinese") Language = "CN";

            string strWhere = "";
            if (ExcelTemplateParams.iTemplateType != 2)
                strWhere = "/Data/template[@Name = '" + ExcelTemplateParams.iTemplateType + "']";
            else
                strWhere = "/Data/template[@Name = '" + ReportTemplateHelper.LoadReportName() + "']";


            xmlnodelist = xmlDocReport.SelectNodes(strWhere);
        }

        private void GetCombReportInfoByTarget(string sTarget, ref string sCombReportInfo)
        {
            string Language = "";
            Languages CurrentLang = Languages.FindOne(l => l.IsCurrentLang == true);
            Language = CurrentLang.ShortName;
            if (Language.ToLower() == "english") Language = "EN";
            else if (Language.ToLower() == "chinese") Language = "CN";

            if (xmlnodelist != null || xmlnodelist.Count > 0)
            {
                string strWhere = "lable[@Target = '" + sTarget + "']";
                XmlNodeList childxmlnodelist = xmlnodelist[0].SelectNodes(strWhere);
                if (childxmlnodelist == null || childxmlnodelist.Count == 0) return;

                sCombReportInfo = childxmlnodelist[0].Attributes[Language].Value;
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
                        if(sValue!="")
                        sUnit =  row[col.ColumnName].ToString().Replace(sValue, "");
                        double temp=0.0;
                        if(double.TryParse(sValue,out temp))
                        row[col.ColumnName] = double.Parse((sValue == "") ? "0" : sValue).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + sUnit;

                    }
                }
            }
        }

        private void GetSepecifiedSearchCondition(int totalPage, int currentPage)
        {
            if (comboBoxCurveName.Items.Count == 0) return;

            isCrossPage = false;
            checkBoxWSeleAll.Checked = false;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;//等待?
            //获取组合条件
            DataSet ds = GetDataSet(GetSQL(0, totalPage, currentPage, ""));
            if (ds == null || ds.Tables.Count < 2) { this.Cursor = System.Windows.Forms.Cursors.Arrow; return; }

            DataTable dt = ds.Tables[0];

            SetDecimalPlaces(ref dt);
            SetTestResult(ref dt);

            this.dgvHistoryRecord.Columns.Clear();

            if (!this.dgvHistoryRecord.Columns.Contains("aa"))
            {
                DataGridViewCheckBoxColumn colCB = new DataGridViewCheckBoxColumn();
                colCB.Name = "aa";
                colCB.Width = 20;
                colCB.HeaderText = "";
                this.dgvHistoryRecord.Columns.Add(colCB);
            }

            this.dgvHistoryRecord.DataSource = dt;
            if (!this.dgvHistoryRecord.Columns.Contains("storage"))
            {
                DataGridViewButtonColumn colStorage = new DataGridViewButtonColumn();
                colStorage.Name = "storage";
                colStorage.Width = 10;
                colStorage.HeaderText = Info.Storage;
                colStorage.Text = Info.Storage;
                colStorage.UseColumnTextForButtonValue = true;
                this.dgvHistoryRecord.Columns.Add(colStorage);
            }
            if (!this.dgvHistoryRecord.Columns.Contains("extraction"))
            {
                DataGridViewButtonColumn colExtraction = new DataGridViewButtonColumn();
                colExtraction.Name = "extraction";
                colExtraction.Width = 10;
                colExtraction.HeaderText = Info.Extraction;
                colExtraction.Text = Info.Extraction;
                colExtraction.UseColumnTextForButtonValue = true;
                this.dgvHistoryRecord.Columns.Add(colExtraction);
            }
            this.dgvHistoryRecord.Columns["Id"].Visible = false;
            this.dgvHistoryRecord.Columns["HistoryRecordCode"].HeaderText = Info.strHistoryRecordCode;
            this.dgvHistoryRecord.Columns["SampleName"].HeaderText = Info.SampleName;
            this.dgvHistoryRecord.Columns["SpecListName"].HeaderText = Info.SpecName;
            this.dgvHistoryRecord.Columns["SpecListName"].Visible = false;
            this.dgvHistoryRecord.Columns["Supplier"].HeaderText = Info.Supplier;
            this.dgvHistoryRecord.Columns["Weight"].HeaderText = Info.Weight;
            this.dgvHistoryRecord.Columns["Shape"].HeaderText = Info.Shape;
            this.dgvHistoryRecord.Columns["Operater"].HeaderText = Info.Operator;
            this.dgvHistoryRecord.Columns["SpecDate"].HeaderText = Info.SpecDate;
            this.dgvHistoryRecord.Columns["SpecSummary"].HeaderText = Info.Description;
            this.dgvHistoryRecord.Columns["WorkCurveId"].HeaderText = Info.WorkingCurve;
            this.dgvHistoryRecord.Columns["WorkCurveId"].Visible = false;
            this.dgvHistoryRecord.Columns["WorkCurveName"].HeaderText = Info.WorkingCurve;
            this.dgvHistoryRecord.Columns["DeviceName"].HeaderText = Info.Device;
            this.dgvHistoryRecord.Columns["CaculateTime"].HeaderText = Info.CaculateTime;
            this.dgvHistoryRecord.Columns["HistoryRecord_Id"].Visible = false;
            this.dgvHistoryRecord.Columns["ActualVoltage"].HeaderText = Info.Voltage;
            this.dgvHistoryRecord.Columns["ActualCurrent"].HeaderText = Info.Current;
            this.dgvHistoryRecord.Columns["CountRate"].HeaderText = Info.CountRate;
            this.dgvHistoryRecord.Columns["PeakChannel"].HeaderText = Info.PeakChannel;
            this.dgvHistoryRecord.Columns["Resole"].HeaderText = Info.Resolve;
            this.dgvHistoryRecord.Columns["TotalCount"].HeaderText = Info.strTotalCount;
            this.dgvHistoryRecord.Columns["StockStatus"].HeaderText = Info.Status;
            if (DifferenceDevice.IsRohs)
            this.dgvHistoryRecord.Columns["TestResult"].HeaderText = Info.TestResult;

            //是否显示单位列
            bool IsShowUnitColumns = (ReportTemplateHelper.LoadSpecifiedValue("OpenHistoryRecordType", "IsShowUnitColumns") == "0") ? false : true;
            if (DifferenceDevice.IsRohs && IsShowUnitColumns) this.dgvHistoryRecord.Columns["Unit"].HeaderText = Info.strUnit;
            this.contextMenuStrip1.Items.Clear();
            string tempStr = ReportTemplateHelper.LoadSpecifiedValue("HistoryItem", "Setting");
            string [] sColumnWidthList = ReportTemplateHelper.LoadSpecifiedValue("HistoryItem", "ColumnWidth").Split(',');
            string[] str = tempStr.Split(',');
            List<string> listStr = str.ToList();
            var vv = User.Find(w => w.Name == FrmLogon.userName);
            for (int i = 0; i < this.dgvHistoryRecord.Columns.Count; i++)
            {

                if (this.dgvHistoryRecord.Columns[i].Name.Equals("aa") || this.dgvHistoryRecord.Columns[i].Name.Equals("continuousnumber")
                    || this.dgvHistoryRecord.Columns[i].Name.Equals("Id") || this.dgvHistoryRecord.Columns[i].Name.Equals("HistoryRecord_Id")
                    || this.dgvHistoryRecord.Columns[i].Name.Equals("customstandard_id") || this.dgvHistoryRecord.Columns[i].Name.EndsWith("_Color")
                    || this.dgvHistoryRecord.Columns[i].Name.Equals("WorkCurveId") || this.dgvHistoryRecord.Columns[i].Name.Equals("SpecListId") || this.dgvHistoryRecord.Columns[i].Name.Equals("SpecListName")
                    )
                    continue;

                if (dAddColmun.ContainsKey(dgvHistoryRecord.Columns[i].Name))
                {
                    this.dgvHistoryRecord.Columns[i].ReadOnly = false;
                    this.dgvHistoryRecord.Columns[i].HeaderText = dAddColmun[dgvHistoryRecord.Columns[i].Name];
                }
                else
                    this.dgvHistoryRecord.Columns[i].ReadOnly = true;
                if (Atoms.AtomList.ToList().Find(w => w.AtomName == this.dgvHistoryRecord.Columns[i].Name) != null)
                    this.dgvHistoryRecord.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                else if (this.dgvHistoryRecord.Columns[i].Name.IndexOf("|")!=-1)
                    this.dgvHistoryRecord.Columns[i].Width = 100;
                else
                    this.dgvHistoryRecord.Columns[i].Width = 80;
                ToolStripMenuItem itemMenu = new ToolStripMenuItem();
                itemMenu.Name = this.dgvHistoryRecord.Columns[i].Name;
                itemMenu.Text = this.dgvHistoryRecord.Columns[i].HeaderText;
                if (listStr.Contains(this.dgvHistoryRecord.Columns[i].Name))
                    this.dgvHistoryRecord.Columns[i].Visible = false;
                if (sColumnWidthList.ToList().Exists(delegate(string v) { return v.ToLower().Substring(0, (v.ToLower().IndexOf(":") < 0 ? 0 : v.ToLower().IndexOf(":"))) == dgvHistoryRecord.Columns[i].Name.ToLower(); }))
                {
                    string sColumnWidth = sColumnWidthList.ToList().Find(delegate(string v) { return v.ToLower().Substring(0, (v.ToLower().IndexOf(":") < 0 ? 0 : v.ToLower().IndexOf(":"))) == dgvHistoryRecord.Columns[i].Name.ToLower(); });
                    if (sColumnWidth != "" && sColumnWidth.IndexOf(":") != -1)
                    {
                        sColumnWidth = sColumnWidth.ToLower().Replace(dgvHistoryRecord.Columns[i].Name.ToLower() + ":", "");
                        this.dgvHistoryRecord.Columns[i].Width = int.Parse(sColumnWidth);
                    }
                }
                itemMenu.Checked = this.dgvHistoryRecord.Columns[i].Visible;
                if (!this.contextMenuStrip1.Items.ContainsKey(itemMenu.Name))
                    this.contextMenuStrip1.Items.Add(itemMenu);

                if (this.contextMenuStrip1.Items.ContainsKey(itemMenu.Name))
                    this.dgvHistoryRecord.Columns[i].Visible = ((ToolStripMenuItem)this.contextMenuStrip1.Items.Find(itemMenu.Name, false)[0]).Checked;
                itemMenu.Click += new EventHandler(itemMenu_Click);
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
            this.dgvHistoryRecord.Refresh();
            int totalRow = int.Parse(ds.Tables[1].Rows[0][0].ToString());
            lblTotaleRowCount.Text = "0";
            lblTotaleRowCount.Text = totalRow.ToString();
            labelTotalPage.Text = (Convert.ToInt32(Math.Ceiling((totalRow + 0.0) / ROWS_PER_PAGE))).ToString();
            labelCurrentPage.Text = Convert.ToString(currentPage + 1);
            int selei = 0;
            if (this.selectLong.Count > 0 && this.dgvHistoryRecord.Rows.Count > 0)
            {

                foreach (DataGridViewRow row in this.dgvHistoryRecord.Rows)
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
            isCrossPage = true;
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
        }

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
            buttonWSearch_Click(null,null);
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
                GetWeight();
            }
            else if (dgvHistoryRecord.Columns[e.ColumnIndex].Name == "storage" && e.RowIndex != -1)
            {
                Object obj = dgvHistoryRecord["Id", e.RowIndex].Value;
                if (obj != null)
                {
                    long id = (long)obj;
                    try
                    {
                        HistoryRecord hr = HistoryRecord.FindById(id);
                        hr.StockStatus = 1;
                        hr.Save();
                        dgvHistoryRecord["StockStatus", e.RowIndex].Value = Info.Storage;
                    }
                    catch { }
                }
            }
            else if (dgvHistoryRecord.Columns[e.ColumnIndex].Name == "extraction" && e.RowIndex != -1)
            {
                Object obj = dgvHistoryRecord["Id", e.RowIndex].Value;
                if (obj != null)
                {
                    long id = (long)obj;
                    try
                    {
                        HistoryRecord hr = HistoryRecord.FindById(id);
                        hr.StockStatus = 0;
                        hr.Save();
                        dgvHistoryRecord["StockStatus", e.RowIndex].Value = Info.Extraction;
                    }
                    catch { }
                }
            }
        }

        private void GetWeight()
        {
            string ids = "";
            foreach (var id in selectLong)
            {
                ids += id + ",";
            }
            ids = ids.TrimEnd(',');
            string sql = "select sum(Weight) from HistoryRecord where Id in (" + ids + ")";
            Object weight = DbEntry.Context.ExecuteScalar(sql);
            lblFactWeightValue.Text = weight.ToString() + "g";
            sql = "Select sum(Weight * averagevalue / 99.99) From HistoryRecord as a left join historyelement as b where a.id in (" + ids + ") and b.elementName='Au' and b.HistoryRecord_Id = a.id";
            weight = DbEntry.Context.ExecuteScalar(sql);
            lblTheoryWeightValue.Text = (weight.ToString() == "" ? "" : ((double)weight).ToString("f4")) + "g";
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
            GetWeight();
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

        }

        private void dgvHistoryRecord_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (dgvHistoryRecord.Columns[e.ColumnIndex].Name == "StockStatus")
                {
                    var cell = dgvHistoryRecord[e.ColumnIndex, e.RowIndex];
                    string status = cell.Value.ToString();
                    if (status.Equals(Info.Extraction))
                    {
                        cell.Style.ForeColor = Color.Blue;
                    }
                    else if (status.Equals(Info.Storage))
                    {
                        cell.Style.ForeColor = Color.Red;
                    }
                }
            }
        }
    }
}
