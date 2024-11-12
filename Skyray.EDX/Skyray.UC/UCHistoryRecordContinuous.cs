using System;
using System.Windows.Forms;
using System.Reflection;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;
using System.Data.Common;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Lephone.Data.Common;
using Lephone.Data.Definition;
using Skyray.Controls;
using System.Data;
using Skyray.Print;
using System.Drawing.Printing;
using System.Drawing;
using Aspose.Cells;
using System.Data.SQLite;
using Lephone.Data;
using System.IO;
using Lephone.Data.SqlEntry;
using Skyray.EDX.Common.ReportHelper;
using Skyray.EDXRFLibrary.Spectrum;
using Skyray.EDXRFLibrary.Define;
using System.Xml;
using System.Xml.Linq;
using Skyray.Language;
using System.Runtime.Remoting.Messaging;

namespace Skyray.UC
{
    /// <summary>
    /// 历史记录类
    /// </summary>
    public partial class UCHistoryRecordContinuous : Skyray.Language.UCMultiple
    {

        public event Skyray.UC.EventDelegate.ContinuousData OnContinuousData;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UCHistoryRecordContinuous()
        {
            InitializeComponent();
            if (this.DesignMode)
                return;
            if ((!Lephone.Data.DbEntry.Context.GetTableNames().Contains("HistoryRecordContinuous")||
                !Lephone.Data.DbEntry.Context.GetTableNames().Contains("HistoryElement")))
            {
                HistoryRecord.FindAll();
            }
            this.dataGridViewW1.AllowUserToResizeColumns = true;
        }


        /// <summary>
        /// 单击全选CheckBox按钮
        /// </summary>
        /// <param name="e"></param>
        private void cbHeader_OnCheckBoxClicked(datagridviewCheckboxHeaderEventArgs e)
        {
            this.dataGridViewW1.CurrentCell = null;
            foreach (DataGridViewRow dgvRow in this.dataGridViewW1.Rows)
            {
                if (dgvRow.Index <= dataGridViewW1.Rows.Count - 1)
                {
                    if (e.CheckedState)
                        dgvRow.Cells[0].Value = true;
                    else
                        dgvRow.Cells[0].Value = false;
                }
            }
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
            this.dataGridViewW1.Columns[item.Name].Visible = item.Checked;//历史记录列的可见性设定
            if (item.Text != Info.ChoiceSetting)
                contextMenuStrip1.Show();
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
            string ColumnIndex = string.Empty;
            foreach (ToolStripMenuItem tempItem in this.contextMenuStrip1.Items)
            {
                if (!tempItem.Checked && !listStr.Contains(tempItem.Name))
                    listStr.Add(tempItem.Name);
                else if (tempItem.Checked && listStr.Contains(tempItem.Name))
                    listStr.Remove(tempItem.Name);

                if (this.dataGridViewW1.Columns.Contains(tempItem.Name))
                    ColumnIndex += tempItem.Name + ":" + this.dataGridViewW1.Columns[tempItem.Name].DisplayIndex.ToString() + ",";


            }
            if (ColumnIndex != string.Empty) ColumnIndex = ColumnIndex.Substring(0, ColumnIndex.Length - 1);
            itemStr = string.Empty;
            foreach (string tempStr in listStr)
                itemStr += tempStr + ",";
            ReportTemplateHelper.SaveSpecifiedValue("HistoryItem", "Setting", itemStr);
            ReportTemplateHelper.SaveSpecifiedValue("HistoryItem", "ColumnIndex", ColumnIndex);
            Msg.Show(Info.SetSuccess);
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


        private void dataGridViewW1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewW1.Columns[e.ColumnIndex].Name == "Show" && e.RowIndex!=-1)
            {
                UCHistoryRecordContinuousList tempForm = new UCHistoryRecordContinuousList();
                tempForm.OnLoadDataSource += new EventDelegate.ContinuousData(tempForm_OnLoadDataSource);
                tempForm.ExecuteInput(this.dataGridViewW1.Rows[e.RowIndex].Cells["continuousnumber"].Value.ToString());
                WorkCurveHelper.OpenUC(tempForm, false, Info.strHistoryRecordContinuoustList,true);
            }
        }

        void tempForm_OnLoadDataSource(List<DataTable> dt, List<Object> obj)
        {
            if (OnContinuousData != null)
                OnContinuousData(dt, obj);
        }

        private void buttonWSearch_Click(object sender, EventArgs e)
        {
            int totalPage = 0;
            int currentPage = 0;
            selectLong.Clear();
            GetHistoryRecordContinuous(totalPage, currentPage);
        }

        private void UCHistoryRecordContinuous_Load(object sender, EventArgs e)
        {
            string k = System.Globalization.DateTimeFormatInfo.CurrentInfo.DateSeparator;
            dateTimePickerStart.CustomFormat = "yyyy" + k + "MM" + k + "dd" + " HH:mm";
            dateTimePickerEnd.CustomFormat = "yyyy" + k + "MM" + k + "dd" + " HH:mm";
            if (ReportTemplateHelper.ContinuousShowUnitType) this.checkShowUnit.Checked = true; else this.checkShowUnit.Checked = false;

            if (DifferenceDevice.IsXRF) checkShowUnit.Visible = false;
            buttonWSearch_Click(null,null);
        }

       
        private void buttonWPrint_Click(object sender, EventArgs e)
        {

        }
       
        private void dataGridViewW1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && this.dataGridViewW1.HitTest(e.X, e.Y).Type == DataGridViewHitTestType.ColumnHeader && GP.CurrentUser.Role.RoleType == 0)
            {
                this.contextMenuStrip1.Items.Clear();
                string tempStr = ReportTemplateHelper.LoadSpecifiedValue("HistoryItem", "Setting");
                string[] str = tempStr.Split(',');
                List<string> listStr = str.ToList();
                List<ColumnObject> lstCol = new List<ColumnObject>();
                for (int i = 0; i < this.dataGridViewW1.Columns.Count; i++)
                {
                    if (this.dataGridViewW1.Columns[i].Name.Equals("aa") || this.dataGridViewW1.Columns[i].Name.Equals("continuousnumber")
                        || this.dataGridViewW1.Columns[i].Name.Equals("Id") || this.dataGridViewW1.Columns[i].Name.Equals("HistoryRecord_Id")
                        || this.dataGridViewW1.Columns[i].Name.Equals("customstandard_id") || this.dataGridViewW1.Columns[i].Name.EndsWith("_Color")
                        || this.dataGridViewW1.Columns[i].Name.Equals("WorkCurveId"))
                        continue;
                    ColumnObject cobj = new ColumnObject(dataGridViewW1.Columns[i].Name, dataGridViewW1.Columns[i].DisplayIndex, dataGridViewW1.Columns[i].HeaderText, dataGridViewW1.Columns[i].Visible);
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
                this.contextMenuStrip1.Show(this.dataGridViewW1.PointToScreen(e.Location));
            }
        }

        private void buttonWCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }

        private void btWPrint_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;//等待?
            btWJoinData();
            this.Cursor = System.Windows.Forms.Cursors.Arrow;//等待?
        }

        /// <summary>
        /// 创建连续测量结果数据库
        /// </summary>
        private DataTable CreateReTestTable(ElementList elementList)
        {
            DataTable reTestTable = new DataTable();
            reTestTable.Columns.Clear();
            reTestTable.Columns.Add("Time", typeof(string));
            //reTestTable.Columns.Add("testdate", typeof(DateTime));
            //reTestTable.Columns.Add("LotNo", typeof(string));//添加批号 090722
            for (int i = 0; i < elementList.Items.Count; i++)
            {
                if (elementList.Items[i].IsDisplay) //090223 修正创建连续测量结果表时出现同名字段
                {
                    reTestTable.Columns.Add(elementList.Items[i].Caption, typeof(string));
                }
            }
            return reTestTable;
        }

        private List<string> lSelect = new List<string>();
        private bool btWJoinData()
        {
            if (selectLong.Count == 0) return false;
            #region
            //if (ExcelTemplateParams.iTemplateType==0)
            //{
            //    #region
            //    List<DataTable> lDt = new List<DataTable>();
            //    DataTable tempDt = new DataTable();
            //    DataColumn dataCount = new DataColumn();
            //    dataCount.ColumnName = Info.Count;
            //    tempDt.Columns.Add(dataCount);

            //    foreach (DataGridViewColumn column in this.dataGridViewW1.Columns)
            //    {
            //        if (!column.Visible || column.Name == "aa" || column.Name == "testdate")
            //            continue;
            //        DataColumn tempData = new DataColumn();
            //        tempData.ColumnName = column.HeaderText;
            //        tempDt.Columns.Add(tempData);
            //    }
            //    int g = 1;
            //    //DataGridViewRow saveOther = null;
            //    foreach (long seleid in selectLong)
            //    {
            //        DataTable dt = GetSelData(seleid.ToString());

            //        //if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == "True")
            //        //{
            //        //    saveOther = row;
            //            DataRow rowTemp = tempDt.NewRow();
            //            rowTemp[Info.Count] = g++;
            //            foreach (DataGridViewColumn column in this.dataGridViewW1.Columns)
            //            {
            //                if (!column.Visible || column.Name == "aa" || column.Name == "testdate")
            //                    continue;
            //                rowTemp[column.HeaderText] = dt.Rows[0][column.Name].ToString();
            //            }
            //            tempDt.Rows.Add(rowTemp);
            //        //}
            //    }
            //    //if (saveOther == null)
            //    //{
            //    //    Msg.Show(Info.SelectHistoryRecord);
            //    //    return false;
            //    //}
            //    if (tempDt.Rows.Count >= WorkCurveHelper.PrintExcelCount)
            //    {
            //        Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
            //        return false;
            //    }
            //    DataTable staticsTable = new DataTable();
            //    DataColumn elemColumns = new DataColumn();
            //    elemColumns.ColumnName = Info.ElementName;
            //    staticsTable.Columns.Add(elemColumns);
            //    string[] staticsHeadText = { Info.MaxValue, Info.MinValue, Info.MeanValue, Info.SDValue };
            //    elemColumns = new DataColumn();
            //    elemColumns.ColumnName = Info.MaxValue;
            //    staticsTable.Columns.Add(elemColumns);

            //    elemColumns = new DataColumn();
            //    elemColumns.ColumnName = Info.MinValue;
            //    staticsTable.Columns.Add(elemColumns);

            //    elemColumns = new DataColumn();
            //    elemColumns.ColumnName = Info.MeanValue;
            //    staticsTable.Columns.Add(elemColumns);

            //    elemColumns = new DataColumn();
            //    elemColumns.ColumnName = Info.SDValue;
            //    staticsTable.Columns.Add(elemColumns);
            //    if (tempDt.Columns.Count > 0)
            //    {
            //        foreach (DataColumn column in tempDt.Columns)
            //        {
            //            if (column.ColumnName == Info.Count || column.ColumnName == Info.SampleName || column.ColumnName == Info.SpecDate)
            //                continue;
            //            DataRow newrow = staticsTable.NewRow();
            //            string maxValue = string.Empty;
            //            string minValue = string.Empty;
            //            string avaValue = string.Empty;
            //            string sdValue = string.Empty;
            //            TabControlHelper.CaculateStaticsData(ref minValue, ref maxValue, ref avaValue, ref sdValue, tempDt, g - 1, column.ColumnName);

            //            newrow[Info.ElementName] = column.ColumnName;
            //            newrow[Info.MaxValue] = maxValue;
            //            newrow[Info.MinValue] = minValue;
            //            newrow[Info.SDValue] = sdValue;
            //            newrow[Info.MeanValue] = avaValue;
            //            staticsTable.Rows.Add(newrow);
            //        }
            //    }

            //    long continuousId = long.Parse(selectLong[0].ToString());
            //    ContinuousResult resultTemp = ContinuousResult.FindById(continuousId);
            //    if (resultTemp == null)
            //    {
            //        Msg.Show(Info.NoLoadSource, Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            //        return false;
            //    }
            //    HistoryRecord tempHistory = HistoryRecord.FindById(resultTemp.HistoryId);
            //    if (tempHistory == null)
            //    {
            //        Msg.Show(Info.NoLoadSource, Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            //        return false;
            //    }
            //    long WorkCurveId = tempHistory.WorkCurveId;
            //    string specListName = tempHistory.SpecListName;
            //    if (string.IsNullOrEmpty(specListName))
            //        return false;
            //    SpecList tempSpecList = SpecList.FindOne(w => w.Name == specListName);
            //    if (tempSpecList == null)
            //    {
            //        Msg.Show(Info.NoLoadSource, Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            //        return false;
            //    }
            //    WorkCurve findWorCurve = WorkCurve.FindById(WorkCurveId);
            //    if (findWorCurve == null)
            //    {
            //        Msg.Show(Info.NoLoadSource, Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            //        return false;
            //    }
            //    SpecAdditional additianlImage = SpecAdditional.FindOne(w => w.SpecListId == tempSpecList.Id);
            //    if (additianlImage == null)
            //    {
            //        Msg.Show(Info.NoLoadSource, Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            //        return false;
            //    }
            //    DirectPrintLibcs.lst.Clear();
            //    if (!EDXRFHelper.LoadTemplate(DirectPrintLibcs.lst, findWorCurve, tempSpecList))
            //    {
            //        Msg.Show(Info.NoLoadSource, Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            //        return false;
            //    }
            //    TreeNodeInfo info;
            //    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(additianlImage.GraphicData))
            //    {
            //        Bitmap b = new Bitmap(ms);
            //        if (b != null)
            //        {
            //            info = DirectPrintLibcs.lst.Find(w => w.Name == "putu");
            //            if (info != null)
            //                DirectPrintLibcs.lst.Remove(info);
            //            DirectPrintLibcs.lst.Add(new TreeNodeInfo
            //            {
            //                Type = CtrlType.Image,
            //                Name = "putu",
            //                Caption = Info.SampleGraph,//工作曲线名称
            //                Text = Info.SampleGraph,////工作曲线名称
            //                Image = b
            //            });
            //        }
            //    }
            //    info = DirectPrintLibcs.lst.Find(w => w.Name == "TestResult");
            //    if (info != null)
            //        DirectPrintLibcs.lst.Remove(info);
            //    if (tempDt != null && tempDt.Columns.Count > 0)
            //    {
            //        DirectPrintLibcs.lst.Add(new TreeNodeInfo
            //        {
            //            Type = CtrlType.Grid,
            //            Name = "TestResult",
            //            Table = tempDt,
            //            Text = Info.TestResult,
            //            Caption = Info.TestResult
            //        });
            //    }
            //    info = DirectPrintLibcs.lst.Find(w => w.Name == "StatisticsInfo");
            //    if (info != null)
            //        DirectPrintLibcs.lst.Remove(info);
            //    if (staticsTable != null && staticsTable.Rows.Count > 0 && staticsTable.Columns.Count > 0)
            //    {
            //        staticsTable.Columns[0].Caption = "";
            //        DirectPrintLibcs.lst.Add(new TreeNodeInfo
            //        {
            //            Type = CtrlType.Grid,
            //            Name = "StatisticsInfo",
            //            Table = staticsTable,
            //            Text = Info.StatisticsInfo,
            //            Caption = Info.StatisticsInfo
            //        });
            //    }
            //    EDXRFHelper.DirectPrintHelper();
            //    #endregion
            //}
            //else 
            #endregion
             //if (ExcelTemplateParams.iTemplateType == 1)
            if(ReportTemplateHelper.ExcelModeType!=2)
            {
                #region
                Report report = new Report();
                report.isShowND = WorkCurveHelper.isShowND;
                ElementList elementList = ElementList.New;
                report.Elements = elementList;
                if (selectLong.Count == 0)
                {
                    Msg.Show(Info.SelectHistoryRecord);
                    return false;
                }
                if (selectLong.Count > WorkCurveHelper.PrintExcelCount)
                {
                    Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
                    return false;
                }
                else
                {
                    ContinuousResult tempResult = ContinuousResult.FindById(selectLong[0]);
                    List<ContinuousResult> continuousResult = ContinuousResult.Find(w => w.ContinuousNumber == tempResult.ContinuousNumber);
                    List<SpecEntity> passingSpec = new List<SpecEntity>();
                    string workCurveName = string.Empty;
                    SpecListEntity tempList = new SpecListEntity();
                    List<ElementList> testResult = new List<ElementList>();
                    foreach (ContinuousResult result in continuousResult)
                    {
                        HistoryRecord record = HistoryRecord.FindById(result.HistoryId);
                        if (record == null)
                            continue;
                        WorkCurve workCurve = WorkCurve.FindById(record.WorkCurveId);
                        if (workCurve == null || workCurve.ElementList == null || workCurve.ElementList.Items.Count == 0)
                            continue;
                        string specListName = record.SpecListName;
                        workCurveName += " " + workCurve.Name;
                        tempList = DataBaseHelper.QueryByEdition(record.SpecListName,record.FilePath,record.EditionType);
                        if (tempList == null || tempList.Specs.Length <= 0)
                        {
                            Msg.Show(Info.NoLoadSource, Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                            return false;
                        }
                        passingSpec.Add(tempList.Specs[0]);
                        ElementList tempElement = ElementList.New;
                        var elements = HistoryElement.Find(w => w.HistoryRecord.Id == record.Id);
                        foreach (var element in elements)
                        {
                            var temp = CurveElement.FindAll().Find(delegate(CurveElement curveElement) { return curveElement.Caption == element.elementName && curveElement.ElementList != null && curveElement.ElementList.WorkCurve != null && curveElement.ElementList.WorkCurve.Id == workCurve.Id; });
                            if (temp == null)
                                continue;
                            double content = 0.0;
                            double.TryParse(element.contextelementValue, out content);
                            temp.Intensity = element.CaculateIntensity;

                            if (element.unitValue == 1)
                            {
                                temp.Content = content;
                                temp.Error = element.Error;
                            }
                            else if (element.unitValue == 2)
                            {
                                temp.Content = content / 10000;
                                temp.Error = element.Error / 10000;
                            }
                            else
                            {
                                temp.Content = content / 10;
                                temp.Error = element.Error/10;
                            }
                            temp.ContentUnit = ContentUnit.ppm;//rohs 使用1号模板 ppm
                            temp.ConditionID = passingSpec.Count - 1;//联测的谱图拼成一个谱图
                            tempElement.Items.Add(temp);
                            elementList.Items.Add(temp);
                            report.historyStandID = element.customstandard_Id;
                        }
                        testResult.Add(tempElement);
                    }
                 
                    if (passingSpec.Count == 0)
                    {
                        Msg.Show(Info.DeleteSpecFile);
                        return false;
                    }
                    foreach (DataGridViewColumn column in this.dataGridViewW1.Columns)
                    {
                        if (!column.Visible && column.Name != "sampleName" && column.Name != "aa" && column.Name != "testdate")
                        {
                            CurveElement tempFind = elementList.Items.ToList().Find(w => w.Caption == column.HeaderText);
                            if (tempFind != null)
                            {
                                elementList.Items.Remove(tempFind);
                                foreach (ElementList tt in testResult)
                                    tt.Items.Remove(tempFind);
                            }
                        }
                    }
                    foreach (ElementList tt in testResult)
                        report.FirstContIntr.Add(tt.Items.Count);
                    report.Spec = passingSpec[0];
                    report.specList = tempList;
                    report.operateMember = FrmLogon.userName;
                    report.WorkCurveName = workCurveName;
                    //修改：何晓明 20111129 报表名称设置
                    //string nowTime = DateTime.Now.ToString("yyyyMMddhhmmss");
                    HistoryRecord hr = HistoryRecord.FindById(long.Parse(continuousResult[0].HistoryId.ToString()));
                    report.historyRecordid = hr.Id.ToString();
                    SpecListEntity sl = DataBaseHelper.QueryByEdition(hr.SpecListName,hr.FilePath,hr.EditionType);
                    WorkCurve wc = WorkCurve.FindById(hr.WorkCurveId);

                    string reportName = DifferenceDevice.interClassMain.GetDefineReportName(sl,wc,this.selectLong.FirstOrDefault());
                    //
                    string strSavePath = string.Empty;
                    if (selectLong.Count == 1 && passingSpec.Count <= 2)
                    {
                        //if (passingSpec.Count == 2)
                        //    report.unitSpec = passingSpec[1];
                        //report.specCount = passingSpec.Count;
                        report.specList.Specs = new SpecEntity[passingSpec.Count];
                        report.specList.Specs[0] = passingSpec[0];
                        if (passingSpec.Count > 1)
                            report.specList.Specs[passingSpec.Count - 1] = passingSpec[passingSpec.Count - 1];
                        report.InterestElemCount = elementList.Items.ToList().FindAll(w => w.IsShowElement).Count;
                        report.TempletFileName = Application.StartupPath + "\\HistoryExcelTemplate\\" + ExcelTemplateParams.OneTimeTemplate;
                        report.ReportPath = WorkCurveHelper.ExcelPath;
                        strSavePath=report.GenerateReport(reportName, true);
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
                            foreach (DataGridViewColumn column in this.dataGridViewW1.Columns)
                            {
                                //修改：何晓明 20110819 多条记录导出中达模板无元素数据
                                var continueResult = ContinuousResult.Find(w => w.Id == selectLong[j]);
                                foreach (var results in continueResult)
                                {                        
                                    var continueNumberResult = ContinuousResult.Find(w => w.ContinuousNumber == results.ContinuousNumber);
                                    foreach (var result in continueNumberResult)
                                    {
                                        var historyRecordId = result.HistoryId;
                                        HistoryElement element = HistoryElement.FindOne(w => w.elementName == column.HeaderText && w.HistoryRecord.Id == historyRecordId);
                                        //HistoryElement element = HistoryElement.FindOne(w => w.elementName == column.HeaderText && w.HistoryRecord.Id == selectLong[j]);
                                        //                       
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
                                                if (Value <= WorkCurveHelper.NDValue && WorkCurveHelper.isShowND)
                                                    rowNew[column.HeaderText] = "ND";
                                                else rowNew[column.HeaderText] = Value.ToString("f2");
                                                totalContent += Value;
                                            }
                                        }
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
                        strSavePath=report.GenerateRetestReport(reportName, dt, true, false);
                    }
                    //var pProcess = System.Diagnostics.Process.GetProcessesByName("Excel");
                    //if (pProcess != null && pProcess.Length > 0) try { pProcess[0].Kill(); }
                    //    catch { };
                    if (!File.Exists(strSavePath))
                        return false;
                    //
                    //修改：何晓明 20110715 按模板导出打开Excel
                    //Msg.Show(Info.ExportSuccessOld);
                    if (Skyray.Controls.SkyrayMsgBox.Show(PrintInfo.SaveSuccess + Skyray.EDX.Common.Info.OpenExcelOrNot, Skyray.EDX.Common.Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        //string excelOnePath = report.ReportPath + reportName + /*"_" + report.Spec.SpecList.SampleName +*/ ".xls";
                        //string excelManyPath = report.ReportPath + reportName +/* "_" + report.Spec.SpecList.SampleName +*/ "_Retry" + ".xls";
                        //if (selectLong.Count == 1)
                        //    Help.ShowHelp(null, excelOnePath);
                        //else
                        //    Help.ShowHelp(null, excelManyPath);
                        Help.ShowHelp(null, strSavePath);
                    }
                }
                #endregion
            }
            else if (ReportTemplateHelper.ExcelModeType == 2)
            {
                #region
                if (selectLong.Count == 0)
                    return false;
                if (selectLong.Count > WorkCurveHelper.PrintExcelCount)
                {
                    Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
                    return false;
                }

                List<string> seleElement = new List<string>();
                foreach (DataGridViewColumn col in this.dataGridViewW1.Columns)
                {
                    Atom atom = Atoms.AtomList.Find(s => s.AtomName == col.HeaderText);
                    if (atom != null && !seleElement.Contains(col.HeaderText) && col.Visible)
                        seleElement.Add(col.HeaderText);
                }


                //int i = 0;
                //InterfaceClass.selePrintObjectL.Clear();
                List<InterfaceClass.PrintObject> seleHistoryPrintObjectL = new List<InterfaceClass.PrintObject>();
                foreach (long seleId in selectLong)
                {
                    long id = long.Parse(seleId.ToString());
                    ContinuousResult resultContinuous = ContinuousResult.FindById(id);

                    if (resultContinuous == null)
                         continue;
                    //
                    HistoryRecord hr= HistoryRecord.FindById(resultContinuous.HistoryId);
                    SpecListEntity sl = DataBaseHelper.QueryByEdition(hr.SpecListName,hr.FilePath,hr.EditionType);
                    WorkCurve wc = WorkCurve.FindById(hr.WorkCurveId);
                    seleHistoryPrintObjectL.Add(new InterfaceClass.PrintObject(sl,wc,2, int.Parse(id.ToString())));
                    //seleHistoryPrintObjectL.Add(new InterfaceClass.PrintObject(null, null, 2, int.Parse(id.ToString())));
                    //

//                    HistoryRecord historyRecord = HistoryRecord.FindById(resultContinuous.HistoryId);
//                    WorkCurve workCurve = WorkCurve.FindById(historyRecord.WorkCurveId);

//                    if (workCurve == null) return false;
//                    string specListName = historyRecord.SpecListName;
//                    string sql = @"select * from SpecList a inner join Condition b
//                                        on a.Condition_Id = b.Id inner join Device d on b.Device_Id=d.Id where a.Name='" + specListName + "' and b.Device_Id=" + WorkCurveHelper.DeviceCurrent.Id;
//                    List<SpecList> tempList = SpecList.FindBySql(sql);
//                    if (tempList == null || tempList.Count == 0)
//                    {
//                        Msg.Show(Info.NoLoadSource, Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
//                        return false;
//                    }
//                    for (int iSpec = 0; iSpec < tempList.Count; iSpec++)
//                    {
//                        if (tempList[iSpec].Specs.Count <= 0)
//                        {
//                            Msg.Show(Info.NoLoadSource, Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
//                            return false;
//                        }
//                    }
//                    SpecList tempSpecList = tempList[0];
//                    if (tempSpecList == null)
//                    {
//                        Msg.Show(Info.NoLoadSource, Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
//                        return false;
//                    }
//                    i++;

                    //List<CustomStandard> customStandard = CustomStandard.FindBySql("select * from customstandard where id in (select customstandard_id from historyelement where historyrecord_id in(  select id from historyrecord where id in (select historyId from continuousresult where id=" + seleId.ToString() + ")))");

                    //NewPrintClassList.Add(new InterfaceClass.NewPrintClass(NewPrintClassList.Count + 1, workCurve, ((customStandard == null || customStandard.Count == 0) ? null : customStandard[0]), tempList[0]));

                }
                if (InterfaceClass.SetPrintTemplate(seleElement, seleHistoryPrintObjectL))
                    EDXRFHelper.NewPrintDirectPrintHelper(InterfaceClass.seledataFountain);
                else Msg.Show(Info.NoLoadSource);
                #endregion
            }
            return true;

            #region 备份
//            if (!ExcelTemplateParams.TemplateType)
//            {
//                #region
//                List<DataTable> lDt = new List<DataTable>();
//                DataTable tempDt = new DataTable();
//                DataColumn dataCount = new DataColumn();
//                dataCount.ColumnName = Info.Count;
//                tempDt.Columns.Add(dataCount);

//                foreach (DataGridViewColumn column in this.dataGridViewW1.Columns)
//                {
//                    if (!column.Visible || column.Name == "aa" || column.Name == "testdate")
//                        continue;
//                    DataColumn tempData = new DataColumn();
//                    tempData.ColumnName = column.HeaderText;
//                    tempDt.Columns.Add(tempData);
//                }
//                int g = 1;
//                DataGridViewRow saveOther = null;
//                foreach (DataGridViewRow row in dataGridViewW1.Rows)
//                {
//                    if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == "True")
//                    {
//                        saveOther = row;
//                        DataRow rowTemp = tempDt.NewRow();
//                        rowTemp[Info.Count] = g++;
//                        foreach (DataGridViewColumn column in this.dataGridViewW1.Columns)
//                        {
//                            if (!column.Visible || column.Name == "aa" || column.Name == "testdate")
//                                continue;
//                            rowTemp[column.HeaderText] = row.Cells[column.Name].Value.ToString();
//                        }
//                        tempDt.Rows.Add(rowTemp);
//                    }
//                }
//                if (saveOther == null)
//                {
//                    Msg.Show(Info.SelectHistoryRecord);
//                    return false;
//                }
//                if (tempDt.Rows.Count >= WorkCurveHelper.PrintExcelCount)
//                {
//                    Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
//                    return false;
//                }
//                DataTable staticsTable = new DataTable();
//                DataColumn elemColumns = new DataColumn();
//                elemColumns.ColumnName = Info.ElementName;
//                staticsTable.Columns.Add(elemColumns);
//                string[] staticsHeadText = { Info.MaxValue, Info.MinValue, Info.MeanValue, Info.SDValue };
//                elemColumns = new DataColumn();
//                elemColumns.ColumnName = Info.MaxValue;
//                staticsTable.Columns.Add(elemColumns);

//                elemColumns = new DataColumn();
//                elemColumns.ColumnName = Info.MinValue;
//                staticsTable.Columns.Add(elemColumns);

//                elemColumns = new DataColumn();
//                elemColumns.ColumnName = Info.MeanValue;
//                staticsTable.Columns.Add(elemColumns);

//                elemColumns = new DataColumn();
//                elemColumns.ColumnName = Info.SDValue;
//                staticsTable.Columns.Add(elemColumns);
//                if (tempDt.Columns.Count > 0)
//                {
//                    foreach (DataColumn column in tempDt.Columns)
//                    {
//                        if (column.ColumnName == Info.Count || column.ColumnName == Info.SampleName || column.ColumnName == Info.SpecDate)
//                            continue;
//                        DataRow newrow = staticsTable.NewRow();
//                        string maxValue = string.Empty;
//                        string minValue = string.Empty;
//                        string avaValue = string.Empty;
//                        string sdValue = string.Empty;
//                        TabControlHelper.CaculateStaticsData(ref minValue, ref maxValue, ref avaValue, ref sdValue, tempDt, g - 1, column.ColumnName);

//                        newrow[Info.ElementName] = column.ColumnName;
//                        newrow[Info.MaxValue] = maxValue;
//                        newrow[Info.MinValue] = minValue;
//                        newrow[Info.SDValue] = sdValue;
//                        newrow[Info.MeanValue] = avaValue;
//                        staticsTable.Rows.Add(newrow);
//                    }
//                }

//                long continuousId = long.Parse(saveOther.Cells["Id"].Value.ToString());
//                ContinuousResult resultTemp = ContinuousResult.FindById(continuousId);
//                if (resultTemp == null)
//                    return false;
//                HistoryRecord tempHistory = HistoryRecord.FindById(resultTemp.HistoryId);
//                if (tempHistory == null)
//                    return false;
//                long WorkCurveId = tempHistory.WorkCurveId;
//                string specListName = tempHistory.SpecListName;
//                if (string.IsNullOrEmpty(specListName))
//                    return false;
//                SpecList tempSpecList = SpecList.FindOne(w => w.Name == specListName);
//                if (tempSpecList == null)
//                    return false;
//                WorkCurve findWorCurve = WorkCurve.FindById(WorkCurveId);
//                if (findWorCurve == null)
//                    return false;
//                SpecAdditional additianlImage = SpecAdditional.FindOne(w => w.SpecListId == tempSpecList.Id);
//                if (additianlImage == null)
//                    return false;
//                DirectPrintLibcs.lst.Clear();
//                if (!EDXRFHelper.LoadTemplate(DirectPrintLibcs.lst, findWorCurve, tempSpecList))
//                    return false;
//                TreeNodeInfo info;
//                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(additianlImage.GraphicData))
//                {
//                    Bitmap b = new Bitmap(ms);
//                    if (b != null)
//                    {
//                        info = DirectPrintLibcs.lst.Find(w => w.Name == "putu");
//                        if (info != null)
//                            DirectPrintLibcs.lst.Remove(info);
//                        DirectPrintLibcs.lst.Add(new TreeNodeInfo
//                        {
//                            Type = CtrlType.Image,
//                            Name = "putu",
//                            Caption = Info.SampleGraph,//工作曲线名称
//                            Text = Info.SampleGraph,////工作曲线名称
//                            Image = b
//                        });
//                    }
//                }
//                info = DirectPrintLibcs.lst.Find(w => w.Name == "TestResult");
//                if (info != null)
//                    DirectPrintLibcs.lst.Remove(info);
//                if (tempDt != null && tempDt.Columns.Count > 0)
//                {
//                    DirectPrintLibcs.lst.Add(new TreeNodeInfo
//                    {
//                        Type = CtrlType.Grid,
//                        Name = "TestResult",
//                        Table = tempDt,
//                        Text = Info.TestResult,
//                        Caption = Info.TestResult
//                    });
//                }
//                info = DirectPrintLibcs.lst.Find(w => w.Name == "StatisticsInfo");
//                if (info != null)
//                    DirectPrintLibcs.lst.Remove(info);
//                if (staticsTable != null && staticsTable.Rows.Count > 0 && staticsTable.Columns.Count > 0)
//                {
//                    staticsTable.Columns[0].Caption = "";
//                    DirectPrintLibcs.lst.Add(new TreeNodeInfo
//                    {
//                        Type = CtrlType.Grid,
//                        Name = "StatisticsInfo",
//                        Table = staticsTable,
//                        Text = Info.StatisticsInfo,
//                        Caption = Info.StatisticsInfo
//                    });
//                }
//                EDXRFHelper.DirectPrintHelper();
//                #endregion
//            }
//            else
//            {
//                #region
//                Report report = new Report();
//                ElementList elementList = ElementList.New;
//                report.Elements = elementList;
//                List<DataGridViewRow> selectRows = new List<DataGridViewRow>();
//                foreach (DataGridViewRow row in dataGridViewW1.Rows)
//                {
//                    if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == "True")
//                    {
//                        selectRows.Add(row);
//                    }
//                }
//                if (selectRows.Count == 0)
//                    return false;
//                if (selectRows.Count >= WorkCurveHelper.PrintExcelCount)
//                {
//                    Msg.Show(string.Format(Info.ExportMaxCount, WorkCurveHelper.PrintExcelCount));
//                    return false;
//                }
//                else
//                {
//                    if (selectRows[0].Cells.Count == 0 || selectRows[0].Cells["continuousnumber"] == null)
//                    {
//                        Msg.Show(Info.SelectHistoryRecord);
//                        return false;
//                    }
//                    long id = long.Parse(selectRows[0].Cells["continuousnumber"].Value.ToString());
//                    List<ContinuousResult> continuousResult = ContinuousResult.Find(w => w.ContinuousNumber == id);
//                    List<Spec> passingSpec = new List<Spec>();
//                    string workCurveName = string.Empty;
//                    foreach (ContinuousResult result in continuousResult)
//                    {
//                        HistoryRecord record = HistoryRecord.FindById(result.HistoryId);
//                        if (record == null)
//                            continue;
//                        WorkCurve workCurve = WorkCurve.FindById(record.WorkCurveId);
//                        if (workCurve == null)
//                            continue;
//                        string specListName = record.SpecListName;
//                        workCurveName += " "+workCurve.Name;
//                        string sql = @"select * from SpecList a inner join Condition b
//                                        on a.Condition_Id = b.Id inner join Device d on b.Device_Id=d.Id where a.Name='" + specListName + "' and b.Device_Id=" + WorkCurveHelper.DeviceCurrent.Id;
//                        List<SpecList> tempList = SpecList.FindBySql(sql);
//                        if (tempList != null && tempList.Count > 0)
//                        {
//                            SpecList currentList = tempList[0];
//                            workCurve.CaculateIntensity(currentList);
//                            workCurve.CacultateContent();
//                            if (!string.IsNullOrEmpty(currentList.Specs[0].SpecData))
//                                passingSpec.Add(currentList.Specs[0]);
//                            if (workCurve == null || workCurve.ElementList == null || workCurve.ElementList.Items.Count == 0)
//                                continue;
//                            ElementList tempElement = ElementList.New;
//                            foreach (CurveElement elementCurve in workCurve.ElementList.Items)
//                            {
//                                tempElement.Items.Add(elementCurve);
//                            }
//                            foreach (DataGridViewColumn column in this.dataGridViewW1.Columns)
//                            {
//                                if (!column.Visible && column.Name != "sampleName" && column.Name != "aa" && column.Name != "testdate")
//                                {
//                                    CurveElement tempFind = tempElement.Items.ToList().Find(w => w.Caption == column.HeaderText);
//                                    if (tempFind != null)
//                                    {
//                                        tempElement.Items.Remove(tempFind);
//                                    }
//                                }
//                            }
//                            report.FirstContIntr.Add(tempElement.Items.Count);
//                            foreach (CurveElement elementCurve in tempElement.Items)
//                            {
//                                elementList.Items.Add(elementCurve);
//                            }
//                            if (record.HistoryElement != null && record.HistoryElement.Count > 0 && record.HistoryElement[0].customstandard_Id.ToString()!="0")
//                                report.historyStandID = long.Parse(record.HistoryElement[0].customstandard_Id.ToString());
//                            else
//                                report.historyStandID = -1;

//                        }
//                        else
//                        {
//                            Msg.Show(Info.DataDelete);
//                            return false;
//                        }
//                    }
//                    if (passingSpec.Count == 0)
//                    {
//                        Msg.Show(Info.DeleteSpecFile);
//                        return false;
//                    }
//                    report.Spec = passingSpec[0];
//                    report.operateMember = FrmLogon.userName;
//                    report.WorkCurveName = workCurveName;
//                    string nowTime = DateTime.Now.ToString("yyyyMMddhhmmss");
//                    if (selectRows.Count == 1 && passingSpec.Count <=2)
//                    {
//                        if (passingSpec.Count == 2)
//                            report.unitSpec = passingSpec[1];
//                        report.specCount = passingSpec.Count;
//                        report.InterestElemCount = elementList.Items.Count;
//                        report.TempletFileName = Application.StartupPath + "\\" + ExcelTemplateParams.OneTimeTemplate;
//                        report.ReportPath = WorkCurveHelper.ExcelPath;
//                        report.GenerateReport(nowTime,true);
//                    }
//                    else
//                    {
//                        DataTable dt = CreateReTestTable(elementList);
//                        CustomStandard standard = null;
//                        if (report.historyStandID > -1)
//                        {
//                            standard = CustomStandard.FindById(report.historyStandID);
//                            if (standard != null && standard.StandardDatas != null && standard.StandardDatas.Count > 0 && standard.IsSelectTotal)
//                            {
//                                dt.Columns.Add(Info.TotalPassReslt);
//                            }
//                        }
//                        int cont = 0;
//                        foreach (DataGridViewRow tempGridRow in selectRows)
//                        {
//                            DataRow rowNew = dt.NewRow();
//                            rowNew["Time"] = ++cont;
//                            double totalContent = 0d;
//                            foreach (DataGridViewColumn column in this.dataGridViewW1.Columns)
//                            {
//                                if (column.Visible && dt.Columns.Contains(column.HeaderText) && tempGridRow.Cells[column.HeaderText].Value != null)
//                                {
//                                    string valueStr = tempGridRow.Cells[column.HeaderText].Value.ToString();
//                                    string splitValue = valueStr.Split('(')[0];
//                                    if (!string.IsNullOrEmpty(splitValue) && !splitValue.Contains("ND"))
//                                    { 
//                                        double Value = double.Parse(splitValue);
//                                        rowNew[column.HeaderText] = Value.ToString("f2");
//                                        if (valueStr.Contains("("))
//                                            totalContent += Value;
//                                    }
//                                    else
//                                        rowNew[column.HeaderText] = splitValue;
//                                }
//                            }
//                            if (standard != null && standard.StandardDatas != null && standard.StandardDatas.Count > 0 && standard.IsSelectTotal)
//                            {
//                                string strPass = "Pass";
//                                if (totalContent > standard.TotalContentStandard)
//                                    strPass = "False";
//                                rowNew[Info.TotalPassReslt] = strPass;
//                            }
//                            dt.Rows.Add(rowNew);
//                        }
//                        report.InterestElemCount = elementList.Items.Count;
//                        report.RetestFileName = Application.StartupPath + "\\" + ExcelTemplateParams.ManyTimeTemplate;
//                        report.ReportPath = WorkCurveHelper.ExcelPath;
//                        report.GenerateRetestReport(nowTime, dt,true);
//                    }
//                    Msg.Show(Info.ExportSuccessOld);
//                }
//                #endregion
//            }
//            return true;

            #endregion
        }

        private DataTable GetSelData(string strID)
        {
            DataTable dt = new DataTable();
            string strSqlCondition = " ( " +
                            " select id from   WorkCurve where condition_id in ( select id from  condition where device_id=" + WorkCurveHelper.DeviceCurrent.Id + "   ) )  ";

            string strSql = "select elementname from historyelement where HistoryRecord_Id in ( select historyId " +
                           " from continuousResult  where  historyid in (select id from historyrecord where devicename='" + WorkCurveHelper.DeviceCurrent.Name + "') ) group by elementname ";

            DataTable dt_HistoryElement = GetData(strSql);

            //新增公司其他信息
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
                    strComOtherInfo += "(select listinfo from historycompanyotherinfo where history_id=a.continuousnumber and  companyothersinfo_id='" + comotherinfo.Id + "') as '" + sComOtherInfo + "',";
                }
            }


            strSql = " select " + strComOtherInfo + " a.* ,b.customstandard_id,b.standardname" +
                     "  from ( select max(id) as Id,max(a.samplename) as samplename,max(a.historyrecordcode) as historyrecordcode,max(a.testdate) as testdate,b.*,''as TestResult from continuousResult a " +
                     " left outer join ( select continuousnumber  ";


            foreach (DataRow row in dt_HistoryElement.Rows)
            {
                strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + " then   (case when [unitValue]=1 then (case when (cast([contextelementvalue] as float)*10000.0)<"+WorkCurveHelper.NDValue.ToString()+"  then 'ND' else [contextelementvalue] end)||" + Convert.ToString((checkShowUnit.Checked) ? "' (%)'" : "''") + " else (case when cast([contextelementvalue] as float)<"+WorkCurveHelper.NDValue.ToString()+"  then 'ND' else [contextelementvalue] end ) || " + Convert.ToString((checkShowUnit.Checked) ? "' (ppm)'" : "''") + " END)  else '' end) as " + "'" + row["elementname"].ToString() + "'";

                strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + " then   (case when [unitValue]=1 then (case when (cast([contextelementvalue] as float)*10000.0)>cast([StandardContent] as float)then 'true' else '' end) else     " +
                    " case when cast([contextelementvalue] as float)>cast([StandardContent] as float)then 'true' else '' end" +
                    " end) else '' end) as " + "'" + row["elementname"].ToString() + "_Color" + "'";

                if (ReportTemplateHelper.IsShowArea)
                    strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + " then   round([ElementIntensity]," + WorkCurveHelper.SoftWareContentBit.ToString() + ")  else 0 end) as " + "'" + row["elementname"].ToString() + Info.strArea + "'";
                if (ReportTemplateHelper.IsShowError)
                {
                    if (ReportTemplateHelper.IsShowNA)
                        strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + " then   (case when cast([contextelementvalue] as float)>0 then cast(round([Error],4) as char)  else 'N/A' end ) else '0' end) as " + "'" + row["elementname"].ToString() + Info.strError + "'";
                    else
                        strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + " then   round([Error]," + WorkCurveHelper.SoftWareContentBit.ToString() + ")  else 0 end) as " + "'" + row["elementname"].ToString() + Info.strError + "'";
                }
                if (ReportTemplateHelper.IsShowIntensity)
                    strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + " then   round([CaculateIntensity]," + WorkCurveHelper.SoftWareIntensityBit.ToString() + ")  else 0 end) as " + "'" + row["elementname"].ToString() + Info.Intensity + "'";
                if (ReportTemplateHelper.IsShowAverageColumns)
                    strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + " then   round([AverageValue]," + WorkCurveHelper.SoftWareContentBit.ToString() + ")  else 0 end) as " + "'" + row["elementname"].ToString() + Info.MeanValue + "'";



            }
            strSql += "from (select a.elementname,a.contextelementvalue,b.continuousnumber,a.unitValue,c.StandardContent,ElementIntensity ,Error,CaculateIntensity,AverageValue from historyelement a  " +
                      " left outer join continuousresult b on b.historyid=a.historyRecord_id " +
                      " left outer join standarddata c on c.CustomStandard_Id=a.customstandard_Id and c.elementcaption=a.elementName " +
                      " where b.continuousnumber<>'')GROUP BY continuousnumber " +

                      ") b on b.continuousnumber=a.continuousnumber " +
                      " where  a.historyid in (select id from historyrecord where devicename='" + WorkCurveHelper.DeviceCurrent.Name + "') "
                      + (this.checkBoxWSampleName.Checked && this.textBoxWInputName.Text.Trim() != "" ? " and a.samplename like '%" + this.textBoxWInputName.Text.Trim() + "%'" : "")
                      + (this.checkBoxWDate.Checked ? " and a.testdate between datetime('" + this.dateTimePickerStart.Value.ToString("yyyy-MM-dd HH:mm:ss") + "') and datetime('" + this.dateTimePickerEnd.Value.ToString("yyyy-MM-dd HH:mm:ss") + "')" : "");
            strSql += " group by a.continuousnumber)a" +
                      " left outer join ( " +
                      " select a.customstandard_id,b.standardname,c.id from historyelement a " +
                      " left outer join customstandard b on b.id=a.customstandard_id  " +
                      " left outer join continuousresult c on c.historyid=a.historyrecord_id group by a.customstandard_id,b.standardname,c.id" +
                      //" ) b on b.id=a.Id where id in {" + strID + "}";
                      " ) b on b.id = a.Id where a.id in ("+strID+")";

            return dt = GetData(strSql);

        }

        private void btwDelete_Click(object sender, EventArgs e)
        {
            if (selectLong.Count == 0) return;
            foreach (long seleid in selectLong)
            {
                SqlStatement sqlstate = new SqlStatement("delete from ContinuousResult where Id=" + seleid.ToString());
                Lephone.Data.DbEntry.Context.ExecuteNonQuery(sqlstate);
            }
            buttonWSearch_Click(null, null);
        }

        private void btwClear_Click(object sender, EventArgs e)
        {
            if (dataGridViewW1.RowCount > 0)
            {
                if (Msg.Show(Info.RemoveAll, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    ContinuousResult.DeleteAll();
                    buttonWSearch_Click(null, null);
                    
                }
            }

        }

        private void dataGridViewW1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //if (WorkCurveHelper.CurrentStandard == null || WorkCurveHelper.CurrentStandard.StandardDatas == null) return;//判断是否存在标准库，如果不存在，则跳出
            foreach (DataGridViewRow row in dataGridViewW1.Rows)
            {
                foreach (DataGridViewColumn col in dataGridViewW1.Columns)
                    if (col.HeaderText.Contains("_Color"))
                    {
                        if (row.Cells[col.HeaderText].Value.ToString() == "true")
                            row.Cells[col.HeaderText.Replace("_Color", "")].Style.ForeColor = Color.Red;

                        col.Visible = false;
                    }
            }   
        }

        #region 翻页管理

         ///<summary>
         ///每页显示的数据的行数
         ///</summary>
        private int ROWS_PER_PAGE = ReportTemplateHelper.ContinuousNumber; 

        private void buttonPrePage_Click(object sender, EventArgs e)
        {
            int totalPage = int.Parse(this.labelTotalPage.Text.Trim());
            int currentPage = int.Parse(labelCurrentPage.Text.Trim());

            if (currentPage < 0) return;
            else if (currentPage == 1) currentPage = 0;
            else currentPage = currentPage - 2;

            GetHistoryRecordContinuous(totalPage, currentPage);
        }

        private void buttonGoto_Click(object sender, EventArgs e)
        {
            int totalPage = int.Parse(this.labelTotalPage.Text.Trim());
            int currentPage = int.Parse(labelCurrentPage.Text.Trim());

            if (string.IsNullOrEmpty(textBoxPageNum.Text) || !textBoxPageNum.Text.IsInt())
                return;

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


            GetHistoryRecordContinuous(totalPage, currentPage);
        }

        private void buttonNexPage_Click(object sender, EventArgs e)
        {

            int totalPage = int.Parse(this.labelTotalPage.Text.Trim());
            int currentPage = int.Parse(labelCurrentPage.Text.Trim());

            if (currentPage > totalPage)
                return;
            else if (currentPage == totalPage) currentPage = totalPage - 1;

            GetHistoryRecordContinuous(totalPage, currentPage);
        }


        private void buttonHomePage_Click(object sender, EventArgs e)
        {
            int totalPage = int.Parse(this.labelTotalPage.Text.Trim());
            int currentPage = int.Parse(labelCurrentPage.Text.Trim());
            
            currentPage = 0;

            GetHistoryRecordContinuous(totalPage, currentPage);
        }

        private void buttonEndPage_Click(object sender, EventArgs e)
        {
            int totalPage = int.Parse(this.labelTotalPage.Text.Trim());
            int currentPage = int.Parse(labelCurrentPage.Text.Trim());
            
            currentPage = totalPage - 1;

            GetHistoryRecordContinuous(totalPage, currentPage);
        }

        private void GetHistoryRecordContinuous(int totalPage, int currentPage)
        {
            isCrossPage = false;
            checkBoxWSeleAll.Checked = false;

            string strSqlCondition = " ( " +
                            " select id from   WorkCurve where condition_id in ( select id from  condition where device_id=" + WorkCurveHelper.DeviceCurrent.Id + "   ) )  ";

            string strSql = "select elementname from historyelement where HistoryRecord_Id in ( select historyId " +
                           " from continuousResult  where  historyid in (select id from historyrecord where devicename='" + WorkCurveHelper.DeviceCurrent.Name + "') ) group by elementname ";
            
            DataTable dt_HistoryElement = GetData(strSql);
            strSql = " select a.* ,b.customstandard_id,b.standardname" +
                     "  from ( select  ";
            //新增公司其他信息
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
                    strComOtherInfo += "(select listinfo from historycompanyotherinfo where history_id=a.continuousnumber and  companyothersinfo_id='" + comotherinfo.Id + "') as '" + sComOtherInfo + "',";
                }
            }


            strSql += strComOtherInfo + " max(id) as Id,max(a.samplename) as samplename,max(a.Supplier) as Supplier,max(a.historyrecordcode) as historyrecordcode,a.testdate as testdate,b.*,''as TestResult from continuousResult a " +
                     " left outer join ( select continuousnumber  ";


            bool isExitCl = false;
            bool isExitBr = false;
            foreach (DataRow row in dt_HistoryElement.Rows)
            {
                strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + " then   (case when [unitValue]=1 then (case when (cast([contextelementvalue] as float)*10000.0)<"+WorkCurveHelper.NDValue.ToString()+"  then 'ND' else [contextelementvalue] end)||" + Convert.ToString((checkShowUnit.Checked) ? "' (%)'" : "''")
                     + " when [unitValue]=3 then (case when (cast([contextelementvalue] as float)*1000)<" + WorkCurveHelper.NDValue.ToString() + "  then 'ND' else [contextelementvalue] end ) || " + Convert.ToString((checkShowUnit.Checked) ? "' (‰)'" : "''")
                    + " else (case when cast([contextelementvalue] as float)<"+WorkCurveHelper.NDValue.ToString()+"  then 'ND' else [contextelementvalue] end ) || " + Convert.ToString((checkShowUnit.Checked) ? "' (ppm)'" : "''") + " END)  else '' end) as " + "'" + row["elementname"].ToString() + "'";

                strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + " then   (case when [unitValue]=1 then (case when (cast([contextelementvalue] as float)*10000.0)>cast([StandardContent] as float)then 'true' else '' end) else     " +
                    " case when cast([contextelementvalue] as float)>cast([StandardContent] as float)then 'true' else '' end" +
                    " end) else '' end) as " + "'" + row["elementname"].ToString() + "_Color" + "'";


                if (ReportTemplateHelper.IsShowArea)
                    strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + " then   round([ElementIntensity]," + WorkCurveHelper.SoftWareContentBit.ToString() + ")  else 0 end) as " + "'" + row["elementname"].ToString() + Info.strArea + "'";
                if (ReportTemplateHelper.IsShowError)
                {
                    if (ReportTemplateHelper.IsShowNA)
                        strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + " then   (case when cast([contextelementvalue] as float)>0 then cast(round([Error],4) as char)  else 'N/A' end ) else '0' end) as " + "'" + row["elementname"].ToString() + Info.strError + "'";
                    else
                        strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + " then   round([Error]," + WorkCurveHelper.SoftWareContentBit.ToString() + ")  else 0 end) as " + "'" + row["elementname"].ToString() + Info.strError + "'";
                }

                if (ReportTemplateHelper.IsShowIntensity)
                    strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + " then   round([CaculateIntensity]," + WorkCurveHelper.SoftWareIntensityBit.ToString() + ")  else 0 end) as " + "'" + row["elementname"].ToString() + Info.Intensity + "'";
                if (ReportTemplateHelper.IsShowAverageColumns)
                    strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + " then   round([AverageValue]," + WorkCurveHelper.SoftWareContentBit.ToString() + ")  else 0 end) as " + "'" + row["elementname"].ToString() + Info.MeanValue + "'";


                if (row["elementname"].ToString().ToLower().Contains("cl")) isExitCl = true;
                if (row["elementname"].ToString().ToLower().Contains("br")) isExitBr = true;
            }

            if (ReportTemplateHelper.IsShowClAndBr && (isExitCl || isExitBr))
            {
                string sClAndBrC = ((isExitCl) ? " max(case when [elementname] ='Cl' then   round([contextelementvalue]," + WorkCurveHelper.SoftWareContentBit.ToString() + ")  else 0 end)" : "0") + "+" + ((isExitBr) ? " max(case when [elementname] ='Br' then   round([contextelementvalue]," + WorkCurveHelper.SoftWareContentBit.ToString() + ")  else 0 end)" : "0");
                string sClAndBrE = ((isExitCl) ? " max(case when [elementname] ='Cl' then   round([Error]," + WorkCurveHelper.SoftWareContentBit.ToString() + ")  else 0 end)" : "0") + "+" + ((isExitBr) ? " max(case when [elementname] ='Br' then   round([Error]," + WorkCurveHelper.SoftWareContentBit.ToString() + ")  else 0 end)" : "0");
                strSql += "," + sClAndBrC + " as 'Cl+Br' ";
                if (ReportTemplateHelper.IsShowError)
                {
                    if (ReportTemplateHelper.IsShowNA)
                        strSql += " ,case when cast(" + sClAndBrC + " as float)>0 then cast(" + sClAndBrE + " as char)  else 'N/A' end  as " + "'" + "Cl+Br" + Info.strError + "'";
                    else
                        strSql += " ," + sClAndBrE + " as " + "'" + "Cl+Br" + Info.strError + "'";
                }

            }

            strSql += "from (select a.elementname,a.contextelementvalue,b.continuousnumber,a.unitValue,c.StandardContent,ElementIntensity ,Error,CaculateIntensity,AverageValue from historyelement a  " +
                     " left outer join continuousresult b on b.historyid=a.historyRecord_id " +
                     " left outer join standarddata c on c.CustomStandard_Id=a.customstandard_Id and c.elementcaption=a.elementName " +
                     " where b.continuousnumber<>''  "+
                     #region 优化点
                     " and a.historyrecord_id in ( select historyid from  continuousresult where continuousNumber in ( "+
                     " select continuousNumber from continuousresult  where historyid in (select id from historyrecord where devicename='" + WorkCurveHelper.DeviceCurrent.Name + "') " +
                     " group by continuousNumber order by continuousNumber limit " + ROWS_PER_PAGE + "  offset " + Convert.ToString((currentPage < 0) ? 0 : currentPage * ROWS_PER_PAGE) + ")) " +
                     #endregion

                      " )GROUP BY continuousnumber " +

                     ") b on b.continuousnumber=a.continuousnumber " +

            #region 优化点
                     " where  a.historyid in ( select historyid from  continuousresult where continuousNumber in ( " +
                     " select continuousNumber from continuousresult  where historyid in (select id from historyrecord where devicename='" + WorkCurveHelper.DeviceCurrent.Name + "') " +
                     " group by continuousNumber order by continuousNumber limit " + ROWS_PER_PAGE + "  offset " + Convert.ToString((currentPage < 0) ? 0 : currentPage * ROWS_PER_PAGE) + "))" 
                     //" where  a.historyid in (select id from historyrecord where devicename='" + WorkCurveHelper.DeviceCurrent.Name + "') "
            #endregion
                     + (this.checkBoxWSampleName.Checked && this.textBoxWInputName.Text.Trim() != "" ? " and a.samplename like '%" + this.textBoxWInputName.Text.Trim() + "%'" : "")
                     + (this.checkBoxWDate.Checked ? " and a.testdate between datetime('" + this.dateTimePickerStart.Value.ToString("yyyy-MM-dd HH:mm:ss") + "') and datetime('" + this.dateTimePickerEnd.Value.ToString("yyyy-MM-dd HH:mm:ss") + "')" : "");
            strSql += " group by a.continuousnumber)a" +
                      " left outer join ( " +
                      " select a.customstandard_id,b.standardname,c.id from historyelement a " +
                      " left outer join customstandard b on b.id=a.customstandard_id  " +
                      " left outer join continuousresult c on c.historyid=a.historyrecord_id " +

            #region 优化点
                      " where a.historyrecord_id in ( select historyid from  continuousresult where continuousNumber in (select continuousNumber from continuousresult "+
                      " where historyid in (select id from historyrecord where devicename='" + WorkCurveHelper.DeviceCurrent.Name + "') group by continuousNumber order by continuousNumber limit " + ROWS_PER_PAGE + "  offset " + Convert.ToString((currentPage < 0) ?0: currentPage * ROWS_PER_PAGE) +"))" +

            #endregion

                      " group by a.customstandard_id,b.standardname,c.id  ) b on b.id=a.Id" ;

            //统计总计数数
            string strTotalSql = "; select count(*) from (select a.continuousNumber from continuousresult a  "+
                                 " where a.historyid in (select id from historyrecord where devicename='" + WorkCurveHelper.DeviceCurrent.Name + "') "
                                 + (this.checkBoxWSampleName.Checked && this.textBoxWInputName.Text.Trim() != "" ? " and a.samplename like '%" + this.textBoxWInputName.Text.Trim() + "%'" : "")
                                 + (this.checkBoxWDate.Checked ? " and a.testdate between datetime('" + this.dateTimePickerStart.Value.ToString("yyyy-MM-dd HH:mm:ss") + "') and datetime('" + this.dateTimePickerEnd.Value.ToString("yyyy-MM-dd HH:mm:ss") + "')" : "") +
                                 " group by a.continuousNumber) a ";


            DataSet ds = GetDataSet(strSql + strTotalSql);
            if (ds == null || ds.Tables.Count < 2) { this.Cursor = System.Windows.Forms.Cursors.Arrow; return; }

            DataTable dt = ds.Tables[0];
            SetTestResult(ref dt);

           this.dataGridViewW1.Columns.Clear();
           if (!this.dataGridViewW1.Columns.Contains("aa"))
           {
               DataGridViewCheckBoxColumn colCB = new DataGridViewCheckBoxColumn();
               colCB.Name = "aa";
               colCB.Width = 20;
               //DatagridViewCheckBoxHeaderCell cbHeader = new DatagridViewCheckBoxHeaderCell();
               //cbHeader.OnCheckBoxClicked += new CheckBoxClickedHandler(cbHeader_OnCheckBoxClicked);
               //colCB.HeaderCell = cbHeader;
               colCB.HeaderText = "";
               this.dataGridViewW1.Columns.Add(colCB);
           }
           this.dataGridViewW1.AllowUserToResizeColumns = true;
           //this.dataGridViewW1.AllowUserToOrderColumns = false;
            this.dataGridViewW1.DataSource = dt;
            this.dataGridViewW1.Columns["Id"].Visible = false;
            this.dataGridViewW1.Columns["sampleName"].HeaderText = Info.SampleName;
            this.dataGridViewW1.Columns["testdate"].HeaderText = Info.SpecDate;
            this.dataGridViewW1.Columns["testdate"].SortMode = DataGridViewColumnSortMode.Automatic;
            this.dataGridViewW1.Columns["customstandard_id"].Visible=false;
            this.dataGridViewW1.Columns["standardname"].HeaderText = Info.strstandardname;
            this.dataGridViewW1.Columns["continuousnumber"].Visible = false;
            this.dataGridViewW1.Columns["TestResult"].HeaderText = Info.TestResult;
            this.dataGridViewW1.Columns["historyrecordcode"].HeaderText = Info.strHistoryRecordCode;
            this.dataGridViewW1.Columns["Supplier"].HeaderText = Info.Supplier;
            this.contextMenuStrip1.Items.Clear();
            string tempStr = ReportTemplateHelper.LoadSpecifiedValue("HistoryItem", "Setting");
            string[] ColumnIndexList = ReportTemplateHelper.LoadSpecifiedValue("HistoryItem", "ColumnIndex").Split(',');
            string[] str = tempStr.Split(',');
            List<string> listStr = str.ToList();
            var vv = User.Find(w => w.Name == FrmLogon.userName);

            for (int i = 0; i < this.dataGridViewW1.Columns.Count; i++)
            {
                if (this.dataGridViewW1.Columns[i].Name.Equals("aa") || this.dataGridViewW1.Columns[i].Name.Equals("continuousnumber")
                    || this.dataGridViewW1.Columns[i].Name.Equals("Id") || this.dataGridViewW1.Columns[i].Name.Equals("HistoryRecord_Id") 
                    || this.dataGridViewW1.Columns[i].Name.Equals("customstandard_id") || this.dataGridViewW1.Columns[i].Name.EndsWith("_Color")
                    || this.dataGridViewW1.Columns[i].Name.Equals("WorkCurveId"))
                    continue;
                this.dataGridViewW1.Columns[i].ReadOnly = true;
                if (Atoms.AtomList.ToList().Find(w => w.AtomName == this.dataGridViewW1.Columns[i].Name) != null)
                    this.dataGridViewW1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                else
                    this.dataGridViewW1.Columns[i].Width = 80;
                ToolStripMenuItem itemMenu = new ToolStripMenuItem();
                itemMenu.Name = this.dataGridViewW1.Columns[i].Name;
                itemMenu.Text = this.dataGridViewW1.Columns[i].HeaderText;
                if (listStr.Contains(this.dataGridViewW1.Columns[i].Name))
                    this.dataGridViewW1.Columns[i].Visible = false;
                if (ColumnIndexList.ToList().Exists(delegate(string v) { return v.ToLower().Substring(0, (v.ToLower().IndexOf(":") < 0 ? 0 : v.ToLower().IndexOf(":"))) == dataGridViewW1.Columns[i].Name.ToLower(); }))
                {
                    string ColumnIndex = ColumnIndexList.ToList().Find(delegate(string v) { return v.ToLower().Substring(0, (v.ToLower().IndexOf(":") < 0 ? 0 : v.ToLower().IndexOf(":"))) == dataGridViewW1.Columns[i].Name.ToLower(); });
                    if (ColumnIndex != "" && ColumnIndex.IndexOf(":") != -1)
                    {
                        ColumnIndex = ColumnIndex.ToLower().Replace(dataGridViewW1.Columns[i].Name.ToLower() + ":", "");
                        if (dataGridViewW1.Columns.Count > int.Parse(ColumnIndex))
                            this.dataGridViewW1.Columns[i].DisplayIndex = int.Parse(ColumnIndex);
                    }
                }
                itemMenu.Checked = this.dataGridViewW1.Columns[i].Visible;
                if (vv.Count > 0 && vv[0].Role.RoleType == 2)
                    continue;
                if (!this.contextMenuStrip1.Items.ContainsKey(itemMenu.Name)) 
                this.contextMenuStrip1.Items.Add(itemMenu);
                if (this.contextMenuStrip1.Items.ContainsKey(itemMenu.Name))
                    this.dataGridViewW1.Columns[i].Visible = ((ToolStripMenuItem)this.contextMenuStrip1.Items.Find(itemMenu.Name, false)[0]).Checked;
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
            }
            checkLushu_CheckedChanged(null, null);
            this.dataGridViewW1.Refresh();


            int totalRow = int.Parse(ds.Tables[1].Rows[0][0].ToString());
            lblTotaleRowCount.Text = "0";
            lblTotaleRowCount.Text = totalRow.ToString();
            labelTotalPage.Text = (Convert.ToInt32(Math.Ceiling((totalRow + 0.0) / ROWS_PER_PAGE))).ToString();
            labelCurrentPage.Text = Convert.ToString(currentPage + 1);

            int selei = 0;
            if (this.selectLong.Count > 0 && this.dataGridViewW1.Rows.Count > 0)
            {

                foreach (DataGridViewRow row in this.dataGridViewW1.Rows)
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

        #endregion

        private void checkLushu_CheckedChanged(object sender, EventArgs e)
        {
            string tempStr = ReportTemplateHelper.LoadSpecifiedValue("HistoryItem", "Setting");
            string[] str = tempStr.Split(',');
            List<string> listStr = str.ToList();

            if (checkLushu.Checked)
            {
                foreach (DataGridViewColumn temp in this.dataGridViewW1.Columns)
                {
                    if (temp.Name.Equals("sampleName",StringComparison.OrdinalIgnoreCase) || temp.Name.Equals("aa",StringComparison.OrdinalIgnoreCase)
                        || temp.Name.Equals("testdate",StringComparison.OrdinalIgnoreCase) || temp.Name.Equals("standardname",StringComparison.OrdinalIgnoreCase))
                        continue;
                    if (temp.Visible && typeof(DataGridViewTextBoxColumn) == temp.GetType() && !temp.Name.Equals("Br", StringComparison.OrdinalIgnoreCase) &&
                        !temp.Name.Equals("Cl", StringComparison.OrdinalIgnoreCase) && !listStr.Contains(temp.Name))
                    {
                        temp.Visible = false;
                    }
                }
            }
            else
            {
                foreach (DataGridViewColumn temp in this.dataGridViewW1.Columns)
                {
                    if (temp.Name.Equals("sampleName", StringComparison.OrdinalIgnoreCase) || temp.Name.Equals("aa", StringComparison.OrdinalIgnoreCase)
                        || temp.Name.Equals("testdate", StringComparison.OrdinalIgnoreCase) || temp.Name.Equals("standardname", StringComparison.OrdinalIgnoreCase))
                        continue;
                    Atom tom = Atoms.AtomList.Find(delegate(Atom s) { return s.AtomName == temp.Name; });
                    if (tom != null && typeof(DataGridViewTextBoxColumn) == temp.GetType() && !temp.Name.Equals("Br", StringComparison.OrdinalIgnoreCase) &&
                        !temp.Name.Equals("Cl", StringComparison.OrdinalIgnoreCase) && !listStr.Contains(temp.Name))
                        temp.Visible = true;
                }
            }

            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedValue("OpenHistoryRecordType", "HistoryRecordContinuousLushuType", (checkLushu.Checked) ? "1" : "0");
            ReportTemplateHelper.ContinuousLushuType = checkLushu.Checked;
        }

        private void checkShowUnit_Click(object sender, EventArgs e)
        {
            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedValue("OpenHistoryRecordType", "HistoryRecordContinuousShowUnitType", (checkShowUnit.Checked) ? "1" : "0");
            ReportTemplateHelper.ContinuousShowUnitType = checkShowUnit.Checked;
            buttonWSearch_Click(null,null);
        }

        private List<long> selectLong = new List<long>();
        private bool isCrossPage = true;//翻页时，全选为空，防止为空时，进行非全选时间发生
        private void dataGridViewW1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex != -1)
            {
                if (Convert.ToBoolean(dataGridViewW1.Rows[e.RowIndex].Cells["aa"].Value) == false)
                {
                    dataGridViewW1.Rows[e.RowIndex].Cells["aa"].Value = true;
                    long selectId = long.Parse(dataGridViewW1.Rows[e.RowIndex].Cells["Id"].Value.ToString());
                    if (!selectLong.Contains(selectId)) selectLong.Add(selectId);
                }
                else
                {
                    dataGridViewW1.Rows[e.RowIndex].Cells["aa"].Value = false;
                    long selectId = long.Parse(dataGridViewW1.Rows[e.RowIndex].Cells["Id"].Value.ToString());
                    if (selectLong.Contains(selectId)) selectLong.Remove(selectId);
                }
            }


        }

        private void checkBoxWSeleAll_CheckedChanged(object sender, EventArgs e)
        {
            if (isCrossPage)
                foreach (DataGridViewRow row in dataGridViewW1.Rows)
                {
                    row.Cells["aa"].Value = checkBoxWSeleAll.Checked;
                    long selectId = long.Parse(row.Cells["Id"].Value.ToString());

                    if (checkBoxWSeleAll.Checked && !selectLong.Contains(selectId)) selectLong.Add(selectId);
                    else if (!checkBoxWSeleAll.Checked && selectLong.Contains(selectId)) selectLong.Remove(selectId);
                }

        }

        #region 导出excel
        private void buttonWExcel_Click(object sender, EventArgs e)
        {
            if (selectLong.Count == 0) return;


            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;//等待?
            string strId = "";
            foreach (long id in selectLong)
                strId += id.ToString() + ",";
            if (strId != "") strId = strId.Substring(0, strId.Length - 1);
            DataTable dt = GetSelData(strId);
            SetDecimalPlaces(ref dt);
            SetTestResult(ref dt);
            this.dataGridViewW2.DataSource = dt;
            foreach (DataGridViewColumn gridcol in this.dataGridViewW1.Columns)
                if (gridcol.Visible && gridcol.Name != "aa" && this.dataGridViewW2.Columns.Contains(gridcol.Name))
                    this.dataGridViewW2.Columns[gridcol.Name].HeaderText = gridcol.HeaderText;
                else if (gridcol.Visible == false && gridcol.Name != "aa" && this.dataGridViewW2.Columns.Contains(gridcol.Name))
                    this.dataGridViewW2.Columns[gridcol.Name].Visible = false;

            this.dataGridViewW2.Columns["TestResult"].HeaderText = Info.TestResult;
            this.dataGridViewW2.Columns["historyrecordcode"].HeaderText = Info.strHistoryRecordCode;
            //this.dataGridViewW2.Columns["HistoryRecord_Id"].Visible = false;
            //this.dataGridViewW2.Columns["workcurveid"].Visible = false;


            OutExcel(this.dataGridViewW2);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;//等待?
        }



        private void SetTestResult(ref DataTable dt)
        {
            if (DifferenceDevice.IsRohs)
                foreach (DataRow row in dt.Rows)
                {
                    string strCustomStandName = string.Empty;
                    row["TestResult"] = ExcelTemplateParams.TestRetult(row["continuousnumber"].ToString(), true,out strCustomStandName);// DifferenceDevice.interClassMain.TestRetult(row["HistoryRecord_Id"].ToString()); //InterfaceClass.TestRetult(row["HistoryRecord_Id"].ToString());
                    
                }
        }


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
                        if (double.TryParse(sValue, out temp))
                            row[col.ColumnName] = double.Parse((sValue == "") ? "0" : sValue).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + sUnit;

                    }
                }
            }
        }

        private void OutExcel(DataGridViewW dataGridViewW2)
        {
            bool hasRecord = false;
            Workbook workbook = new Workbook();
            string fileName = "";
            if (ReportTemplateHelper.IsOurExcel==1)
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
            if (ReportTemplateHelper.IsOurExcel==2)
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
     

        private delegate void CrossThreadControl();
        #endregion

    }
}
