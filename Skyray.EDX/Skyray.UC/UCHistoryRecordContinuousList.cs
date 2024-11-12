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
using Excel = Microsoft.Office.Interop.Excel;
using Lephone.Data.SqlEntry;
using Skyray.EDXRFLibrary.Spectrum;


namespace Skyray.UC
{
    /// <summary>
    /// 供应商类
    /// </summary>
    public partial class UCHistoryRecordContinuousList : Skyray.Language.UCMultiple
    {
        private string sContinuousnumber;
        private List<string> lSelect=new List<string>();
        /// <summary>
        /// 构造函数
        /// </summary>
        public UCHistoryRecordContinuousList()
        {
            InitializeComponent();
            if (this.DesignMode)
                return;
            if (!DifferenceDevice.ConfigUI && (!Lephone.Data.DbEntry.Context.GetTableNames().Contains("HistoryRecordContinuoustList")))
            {
                HistoryRecord.FindAll();
            }
          var vv = User.Find(w => w.Name == FrmLogon.userName);
          if (vv.Count > 0 && vv[0].Role.RoleType == 2)
          {
              this.btWJoinData.Visible = false;
          }

          DataGridViewCheckBoxColumn colCB = new DataGridViewCheckBoxColumn();
          colCB.Name = "aa";
          colCB.Width = 20;
          DatagridViewCheckBoxHeaderCell cbHeader = new DatagridViewCheckBoxHeaderCell();
          cbHeader.OnCheckBoxClicked += new CheckBoxClickedHandler(cbHeader_OnCheckBoxClicked);
          colCB.HeaderCell = cbHeader;
          colCB.HeaderText = "";
          this.dataGridViewW1.Columns.Add(colCB);
          this.dataGridViewW1.AllowUserToResizeColumns = true;
          this.dataGridViewW1.AllowUserToOrderColumns = false;
        }

        public void ExecuteInput(string sContinuousnumber)
        {
            this.sContinuousnumber = sContinuousnumber;
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


        private void GetHistoryRecordContinuousList()
        {
            //获取组合条件
            string strSampleName = (this.textBoxWInputName.Text != "") ? " and a.SampleName like '%" + this.textBoxWInputName.Text.ToString() + "%'" : "";

            string strSql = "select elementname from historyelement where HistoryRecord_Id in ( select historyid from  continuousresult where continuousnumber='" + sContinuousnumber + "' ) group by elementname ";
            DataTable dt_historyelement = GetData(strSql);

            strSql = "select * from historyrecord a left join (select HistoryRecord_Id ";
            foreach (DataRow row in dt_historyelement.Rows)
            {
                strSql += " ,max(case when [elementname] =" + "'" + row["elementname"].ToString() + "'" + " then   [contextelementvalue] else '' end) as " + "'" + row["elementname"].ToString() + "'";
            }
            strSql += "from historyelement where HistoryRecord_Id in ( select historyid from  continuousresult where continuousnumber='" + sContinuousnumber + "' ) group by HistoryRecord_Id"+
                ") b on b.HistoryRecord_Id=a.id where 1= 1 and  a.id in ( select historyid from  continuousresult where continuousnumber='" + sContinuousnumber + "' ) "+strSampleName;
            DataTable dt = GetData(strSql);

            this.dataGridViewW1.DataSource = dt;
            this.dataGridViewW1.Columns["Id"].Visible = false;
            this.dataGridViewW1.Columns["SampleName"].HeaderText = Info.SampleName;
            this.dataGridViewW1.Columns["SpecListName"].HeaderText = Info.SpecName;
            this.dataGridViewW1.Columns["Supplier"].HeaderText = Info.Supplier;
            //this.dataGridViewW1.Columns["Weight"].HeaderText = Info.Weight;
            this.dataGridViewW1.Columns["Shape"].HeaderText = Info.Shape;
            this.dataGridViewW1.Columns["Operater"].HeaderText = Info.Operator;
            this.dataGridViewW1.Columns["SpecDate"].HeaderText = Info.SpecDate;
            this.dataGridViewW1.Columns["SpecSummary"].HeaderText = Info.Description;
            this.dataGridViewW1.Columns["WorkCurveId"].Visible = false;
            this.dataGridViewW1.Columns["DeviceName"].HeaderText = Info.Device;
            this.dataGridViewW1.Columns["CaculateTime"].HeaderText = Info.CaculateTime;
            this.dataGridViewW1.Columns["HistoryRecord_Id"].Visible = false;
            string tempStr = ReportTemplateHelper.LoadSpecifiedValue("HistoryItem", "Setting");
            string[] str = tempStr.Split(',');
            List<string> listStr = str.ToList();
            var vv = User.Find(w => w.Name == FrmLogon.userName);

            for (int i = 0; i < this.dataGridViewW1.Columns.Count; i++)
            {
                if (this.dataGridViewW1.Columns[i].Name.Equals("aa") || this.dataGridViewW1.Columns[i].Name.Equals("Id") || this.dataGridViewW1.Columns[i].Name.Equals("HistoryRecord_Id"))
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
                itemMenu.Checked = this.dataGridViewW1.Columns[i].Visible;
                if (vv.Count > 0 && vv[0].Role.RoleType == 2)
                    continue;
                this.contextMenuStrip1.Items.Add(itemMenu);
                itemMenu.Click += new EventHandler(itemMenu_Click);

            }
            if (this.contextMenuStrip1.Items.Count > 0)
            {
                ToolStripMenuItem setItem = new ToolStripMenuItem();
                setItem.Name = "SettingContext";
                setItem.Text = Info.ChoiceSetting;
                setItem.Click += new EventHandler(setItem_Click);
                this.contextMenuStrip1.Items.Add(setItem);
            }
            this.dataGridViewW1.Refresh();
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
            foreach (ToolStripMenuItem tempItem in this.contextMenuStrip1.Items)
            {
                if (!tempItem.Checked && !listStr.Contains(tempItem.Name))
                    listStr.Add(tempItem.Name);
                else if (tempItem.Checked && listStr.Contains(tempItem.Name))
                    listStr.Remove(tempItem.Name);
            }
            itemStr = string.Empty;
            foreach (string tempStr in listStr)
                itemStr += tempStr + ",";
            ReportTemplateHelper.SaveSpecifiedValue("HistoryItem", "Setting", itemStr);
            Msg.Show(Info.SetSuccess);
        }

        private void buttonWSearch_Click(object sender, EventArgs e)
        {
            GetHistoryRecordContinuousList(); 
            
        }

        private void UCHistoryRecordContinuousList_Load(object sender, EventArgs e)
        {
            buttonWSearch_Click(null,null);
        }


        public event EventDelegate.ContinuousData OnLoadDataSource;
        private void butOK_Click(object sender, EventArgs e)
        {
            btWJoinData_Click(null, null);
            //if(this.checkLuSu.Checked)
            //    EDXRFHelper.DirectPrintHelper("rohs-zhongda.template");
            //else
            //    EDXRFHelper.DirectPrintHelper("rohsNew.template");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);//返回主界面
        }

        private void dataGridViewW1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right &&
                this.dataGridViewW1.HitTest(e.X, e.Y).Type == DataGridViewHitTestType.ColumnHeader)
            {
                this.contextMenuStrip1.Show(this.dataGridViewW1.PointToScreen(e.Location));
            }
        }

        private void btWJoinData_Click(object sender, EventArgs e)
        {
            #region
 
            #endregion
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn(Info.Number, typeof(string)));//元素
            List<HistoryRecord> historyList = new List<HistoryRecord>();
            foreach (DataGridViewRow row in dataGridViewW1.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == "True")
                {
                    HistoryRecord tempRecord = HistoryRecord.New;
                    //tempRecord.SpecListName = row.Cells["SpecListName"].Value.ToString();
                    tempRecord.Id = long.Parse(row.Cells["Id"].Value.ToString());
                    tempRecord.WorkCurveId = long.Parse(row.Cells["WorkCurveId"].Value.ToString());
                    historyList.Add(tempRecord);
                }
            }
            foreach (HistoryRecord recorder in historyList)
            {
                WorkCurve workCurve = WorkCurve.FindById(recorder.WorkCurveId);
                foreach (CurveElement curveElement in workCurve.ElementList.Items)
                {
                    DataColumn column = new DataColumn();
                    column.ColumnName = curveElement.Caption + "(" + curveElement.ContentUnit.ToString()+")";
                    dt.Columns.Add(column);
                }
            }
            DataRow newRow = dt.NewRow();
            newRow[Info.Number] = 1;
            int i = 0;
            List<object> listMap = new List<object>();
            SpecListEntity tempResult =new SpecListEntity();
            foreach (HistoryRecord recorder in historyList)
            {
                 WorkCurve workCurve = WorkCurve.FindById(recorder.WorkCurveId);
                 foreach (CurveElement curveElement in workCurve.ElementList.Items)
                 {
                     List<HistoryElement> elementList = HistoryElement.FindBySql("select * from HistoryElement a inner join HistoryRecord b on a.HistoryRecord_Id=b.Id where b.Id="+recorder.Id+" and a.elementName='"+curveElement.Caption+"'");
                     if (elementList != null && elementList.Count > 0)
                         newRow[curveElement.Caption + "(" + curveElement.ContentUnit.ToString() + ")"] = elementList[0].contextelementValue;
                 }
                 string specListName = recorder.SpecListName;
//                 string sql = @"select * from SpecList a inner join Condition b
//                                        on a.Condition_Id = b.Id inner join Device d on b.Device_Id=d.Id where a.Name='" + specListName + "' and b.Device_Id=" + WorkCurveHelper.DeviceCurrent.Id;
                 SpecListEntity tempList = DataBaseHelper.QueryByEdition(specListName,recorder.FilePath,recorder.EditionType);
                 //if (tempList != null && tempList.Count > 0)
                 //{
                     //SpecList currentList = tempList[0];
                     //tempResult = currentList;
                     List<SpecEntity> specTempList = new List<SpecEntity>();
                     specTempList.Add(tempList.Specs[0]);
                     System.Drawing.Bitmap bitMap1 = new Bitmap(400, 300);
                     SpecDrawing.DrawSpec(ref bitMap1, specTempList, workCurve.ElementList);
                     TreeNodeInfo treeNodeInfo = new TreeNodeInfo
                     {
                         Type = CtrlType.Image,
                         Name = "SampleGraph" + i,
                         Caption = Info.SampleGraph + i,//工作曲线名称
                         Text = Info.SampleGraph + i,////工作曲线名称
                         Image = bitMap1
                     };
                     i++;
                     listMap.Add(treeNodeInfo);

                 //}
            }
            dt.Rows.Add(newRow);
             List<DataTable> lDt = new List<DataTable>();
             lDt.Add(dt);
             //if (tempResult.Image != null)
             //{
             //    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(tempResult.Image))
             //    {
             //        System.Drawing.Bitmap b = new Bitmap(ms);
             //        TreeNodeInfo tempSample = new TreeNodeInfo
             //               {
             //                   Type = CtrlType.Image,
             //                   Name = "SampleImage",
             //                   Caption = Info.SampleImage,//工作曲线名称
             //                   Text = Info.SampleImage,////工作曲线名称
             //                   Image = b
             //               };
             //        listMap.Add(tempSample);
             //    }
             //}
             if (OnLoadDataSource != null)
                 OnLoadDataSource(lDt, listMap);
        }

        private void buttonWSearch_Click_1(object sender, EventArgs e)
        {
            GetHistoryRecordContinuousList();
        }

    }
}
