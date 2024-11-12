using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;
using Lephone.Data.Common;
using Skyray.Controls;
using Skyray.EDX.Common;
using ZedGraph;
using Skyray.Controls.Extension;
using System.Linq;
using System.IO;
using System.Security.AccessControl;
using System.Collections;
using System.Data;
using System.Data.SQLite;
using Lephone.Data;
using System.ComponentModel;
using System.Drawing.Imaging;
using Skyray.Controls.Win32Util;
using System.Text;
using System.Xml;
using System.Reflection;
using Skyray.EDXRFLibrary.Spectrum;
using Skyray.EDXRFLibrary.Define;
//using System.Data.SQLite;
//using Lephone.Data;

namespace Skyray.UC
{
    /// <summary>
    /// 选择谱图对象类
    /// </summary>
    public partial class SelectSample : Skyray.Language.UCMultiple
    {
        public DataTable dt_SpecList = null;
#if !DesignMode

        public List<SpecListEntity> lstGetSpecList = new List<SpecListEntity>();
#endif
        #region 系统初始化

        /// <summary>
        /// 添加排序方式多语言
        /// </summary>
        private void SetcobTimeAndName()
        {
            ArrayList list = new ArrayList();
            ValueObject vo1 = new ValueObject();
            vo1.Name = Info.strAsc;
            vo1.Value = "1";
            list.Add(vo1);

            ValueObject vo2 = new ValueObject();
            vo2.Name = Info.strDesc;
            vo2.Value = "2";
            list.Add(vo2);
            cobTimeOrder.DataSource = list;
            cobTimeOrder.DisplayMember = "Name";
            cobTimeOrder.ValueMember = "Value";


            ArrayList list1 = list.Clone() as ArrayList;

            cobNameOrder.DataSource = list1;
            cobNameOrder.DisplayMember = "Name";
            cobNameOrder.ValueMember = "Value";


            cobTimeOrder.Text = cobNameOrder.Text = Info.strAsc;
        }

        /// <summary>
        /// 界面显示谱方式
        /// </summary>
        private void GetCurrentDateSpec()
        {
            string OpenSelectSampleType = Skyray.EDX.Common.ReportTemplateHelper.LoadSpecifiedValue("OpenSelectSampleType", "Type");
            if (OpenSelectSampleType == "1")
                Expand();
            chkDate.Checked = bool.Parse(Skyray.EDX.Common.ReportTemplateHelper.LoadSpecifiedValue("OpenSelectSampleType", "IsSelectDate"));
            chkTimeOrder.Checked = false;
            DateTime dNow = DateTime.Now;
            DateTime dTo = DateTime.Now;
            dNow = (Skyray.EDX.Common.ReportTemplateHelper.LoadSpecifiedValue("OpenSelectSampleType", "dateTimePickerFrom") == "" ? dNow : DateTime.Parse(Skyray.EDX.Common.ReportTemplateHelper.LoadSpecifiedValue("OpenSelectSampleType", "dateTimePickerFrom")));
            dTo = (Skyray.EDX.Common.ReportTemplateHelper.LoadSpecifiedValue("OpenSelectSampleType", "dateTimePickerTo") == "" ? dTo : DateTime.Parse(Skyray.EDX.Common.ReportTemplateHelper.LoadSpecifiedValue("OpenSelectSampleType", "dateTimePickerTo")));
            dateTimePickerFrom.Value = dNow;
            dateTimePickerTo.Value = dTo;
            // DateTime.TryParse(Skyray.EDX.Common.ReportTemplateHelper.LoadSpecifiedValue("OpenSelectSampleType", "dateTimePickerFrom"), out dNow);
            
           // DateTime.TryParse(Skyray.EDX.Common.ReportTemplateHelper.LoadSpecifiedValue("OpenSelectSampleType", "dateTimePickerTo"), out dTo);
           
            
            if (cobTimeOrder.Items.Count > 1)
                cobTimeOrder.SelectedIndex = 1;
        }


        private void RegisterHighPerformanceListView()
        {
            HighPerformanceListView1.OnItemSelectedChange = listViewWSpecList_ItemSelectionChanged;
            HighPerformanceListView1.OnNewDoubleClick = listViewWSpecList_DoubleClick;
            HighPerformanceListView1.OnRefeshData = BindCheckInfo;
            HighPerformanceListView1.ListViewProvider = listViewProvider;
            HighPerformanceListView1.OnScrollLoadData = ScrollLoadDataOper;
        }

        private void ScrollLoadDataOper()
        {
            dt_SpecList = GetSpecList();
            GetSpec();
        }

        public SelectSample(AddSpectrumType AddSpectrumType)
        {
            InitializeComponent();
            xcSpec.IsRadiation = false;
            type = AddSpectrumType;

            SpecCurrentName = new List<string>();
            SelectedSpecList = new List<SpecListEntity>();

            //添加排序方式多语言
            SetcobTimeAndName();
            RegisterHighPerformanceListView();
            //查看谱数据显示方式
            GetCurrentDateSpec();
            this.btnImport.Visible = true;
            this.btnExport.Visible = true;

            chkTimeOrder.Checked = true;
            //读取数据信息
            dt_SpecList = GetSpecList();
            //填充数据
            GetSpec();
        }


        #endregion

        #region 查询、获取数据

        private int totalCount = 0;

        private DataTable GetSpecList()
        {
            DisplayStartIdx = this.HighPerformanceListView1.DisplayStartIdx;
            //在测试过程中，针对XRF多条件扫谱，如果扫完一个条件谱，当打开谱文件管理时，不应该显示没有全部扫完的谱文件信息
            string sqlTestSpec = "";
            if (DifferenceDevice.interClassMain.progressInfo.ToolStripProgressBar!=null 
                && DifferenceDevice.interClassMain.progressInfo.ToolStripProgressBar.Value > 0)
            {
                sqlTestSpec = " and name not in ('" + DifferenceDevice.interClassMain.specList.Name + "') "; 
            }

            sql = "select id,name from SpectrumData where 1=1 " + sqlTestSpec;
            //if (WorkCurveHelper.DeviceCurrent != null)
            //{
            //    var deviceId = WorkCurveHelper.DeviceCurrent.Id;
            //    sql += " where Condition_Id in (select Id from condition where Device_id = " + deviceId + ")" + sqlTestSpec;
            //}

            if (chkName.Checked) sql += " And Name like '%" + txtKeywords.Text.Trim() + "%'";
            if (chkDate.Checked) sql += "And datetime(SpecDate) <=datetime('" + dateTimePickerTo.Value.ToString("yyyy-MM-dd") + " 23:59:59')" + " and datetime(SpecDate)>=datetime('" + dateTimePickerFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00')";


            if (WorkCurveHelper.DeviceCurrent!=null) sql += " and DeviceName='" + WorkCurveHelper.DeviceCurrent.Name + "'";

            //if (GP.CurrentUser.Role.RoleType == 2)
            //{
            //    sql += " and SpecType = 2";
            //}
            if (!chkNameOrder.Checked && !chkTimeOrder.Checked)
                sql += " order by  SpecTypeValue asc,SpecDate asc,LOWER(Name) asc";
            else
            {
                sql += " order by SpecTypeValue asc";
                if (chkTimeOrder.Checked) { sql += (cobTimeOrder.SelectedIndex == 0) ? " ,SpecDate asc" : " ,SpecDate desc"; }
                if (chkNameOrder.Checked) { sql += (cobNameOrder.SelectedIndex == 0) ? " ,LOWER(Name) asc" : " ,LOWER(Name) desc";  }
                
            }
            int rows = this.HighPerformanceListView1.GetListViewVisisbleRows();
            sql = @"select * from (" + sql + ")   limit " + rows + "  offset " + rows * this.HighPerformanceListView1.DisplayStartIdx + ";select count(*) from (" + sql + ")";

            DataSet dataSet = GetNewData(sql);
            totalCount = int.Parse(dataSet.Tables[1].Rows[0][0].ToString());
            return dataSet.Tables[0];
        }

        private DataSet GetNewData(string strSql)
        {
            DataSet dt = new DataSet();
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


        #endregion

        #region 控件和谱之间操作
        /// <summary>
        /// 获取谱信息列表
        /// </summary>
        private void GetSpec()
        {
            //修改：何晓明 20110701 谱名称3列显示
            //this.HighPerformanceListView1.Items.Clear();
            List<DataRow> listRow = new List<DataRow>();
            listViewProvider.DataCount = totalCount;
            foreach (DataRow row in dt_SpecList.Rows)
            {
                listRow.Add(row);
                //修改：何晓明 20110701 谱名称3列显示
                //this.HighPerformanceListView1.Items.Add(row["name"].ToString());
                //this.HighPerformanceListView1.Items[this.HighPerformanceListView1.Items.Count - 1].Name = row["id"].ToString();
            }
            listViewProvider.DataList = listRow;
        }

        private void listViewWSpecList_ItemSelectionChanged(ListView.SelectedListViewItemCollection e)
        {
            if (Control.ModifierKeys != Keys.Control && Control.ModifierKeys != Keys.Shift)
                lstGetSpecList.Clear();
            string strSelectId = "";
            List<string> existNameList = new List<string>();
            string strCurSpecID = string.Empty;
            foreach (ListViewItem temp in e)
            {
                if (!lstGetSpecList.Exists(w => w.Name == temp.Text))
                {
                    //strSelectId += "'" + temp.Text.ToString() + "',";
                    strSelectId += "" + temp.Name.ToString() + ",";
                    strCurSpecID = (strCurSpecID == string.Empty || long.Parse(temp.Name) < long.Parse(strCurSpecID)) ? temp.Name : strCurSpecID;
                }
                else
                {
                    existNameList.Add(temp.Text);
                }
            }
            lstGetSpecList.RemoveAll(w => !existNameList.Contains(w.Name));
            if (strSelectId == "") return;
            strSelectId = strSelectId.Substring(0, strSelectId.Length - 1);
            //List<SpecListEntity> seleSpecList = WorkCurveHelper.DataAccess.GetSpecList(strSelectId);
            List<SpecListEntity> seleSpecList = WorkCurveHelper.DataAccess.GetSpecListById(strSelectId);
            if (seleSpecList != null && seleSpecList.Count > 0)
            {
                lstGetSpecList.AddRange(seleSpecList);

                SetCurrentSpecInfo(seleSpecList[0],strCurSpecID);
            }
            if (specListCurrent.CompanyInfoList != null && specListCurrent.CompanyInfoList.Count > 0)
                buttonWOtherInfoSet.Visible = true;
            else
                buttonWOtherInfoSet.Visible = false;
        }

        #endregion

        #region 导入数据
        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            //if (DialogResult.OK == this.fbdImport.ShowDialog())
            //{
            //    if (this.fbdImport.SelectedPath.IsNullOrEmpty())
            //    {
            //        MessageBox.Show("选择路径！路径不能为空！");
            //        return;
            //    }

            //    if (ImportFiles(this.fbdImport.SelectedPath))
            //    {
            //        this.btnImport.Visible = true;
            //        this.btnExport.Visible = true;
            //        btnSearch_Click(null, null);
            //        if (selectVisual)
            //        {
            //            selectVir();
            //        }
            //    }
            //}
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = "Spec file (*.Spec)|*.Spec";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] strFileNames = openFileDialog1.FileNames;
                if (strFileNames.Length <= 0) return;
                FileInfo file0 = new FileInfo(strFileNames[0]);
                ISpectrumDAL dal = WorkCurveHelper.GetExportType(file0.DirectoryName);

                List<BackUpSpecList> backUpSpecList = new List<BackUpSpecList>();
                foreach (string em in strFileNames)
                {
                    FileInfo f = new FileInfo(em);
                    if (f.Name.IndexOf(".Spec") == -1 && f.Name.IndexOf(".spec") != -1)
                        GetBackUpSpecList(f, ref backUpSpecList);
                    else
                    {
                        SpecListEntity entity = dal.Query(f.Name.Replace(".Spec", "").Trim());
                        SQliteDAL savDal = new SQliteDAL();
                        entity.DeviceName = WorkCurveHelper.DeviceCurrent.Name;
                        savDal.Save(entity);
                    }
                }
                if (backUpSpecList.Count > 0) SaveBackUpSpecList(backUpSpecList);
                btnSearch_Click(null, null);
                if (selectVisual)
                {
                    selectVir();
                }
            }
        }

        private bool ImportFiles(string path)
        {
            if (path.IsNullOrEmpty())
                return false;
            if (!Directory.Exists(path))
                return false;
            DirectoryInfo dir = new DirectoryInfo(path);
            if (dir == null)
                return false;
            FileInfo[] fineInfo = dir.GetFiles("*.Spec");
            if (fineInfo == null || fineInfo.Length == 0)
                return false;
            this.btnImport.Visible = true;
            this.btnExport.Visible = true;
            ISpectrumDAL dal = WorkCurveHelper.GetExportType(path);

            List<BackUpSpecList> backUpSpecList = new List<BackUpSpecList>();
            foreach (FileInfo info in fineInfo)
            {
                if (info.Name.IndexOf(".Spec") == -1 && info.Name.IndexOf(".spec") != -1)
                    GetBackUpSpecList(info, ref backUpSpecList);
                else
                {
                    SpecListEntity entity = dal.Query(info.Name.Replace(".Spec", "").Trim());
                    SQliteDAL savDal = new SQliteDAL();
                    entity.DeviceName = WorkCurveHelper.DeviceCurrent.Name;
                    savDal.Save(entity);
                }
            }

            if (backUpSpecList.Count > 0) SaveBackUpSpecList(backUpSpecList);
            return true;
        }

        #region 导入能谱合并旧版本谱文件
        private void GetBackUpSpecList(FileInfo info, ref List<BackUpSpecList> backUpSpecList)
        {
            BackUpSpecList lst = SerializeHelper.FileToObj(info.FullName) as BackUpSpecList;
            foreach (var sp in lst.BackUpSpecLst)
            {
                Spec spec = Spec.New.Init(sp.Name, sp.SpecData, sp.SpecTime, sp.UsedTime, sp.TubVoltage, sp.TubCurrent, "");
                spec.IsSmooth = true;
                Device devCurrent = WorkCurveHelper.DeviceCurrent;//当前设备

                if (devCurrent == null || spec == null || spec.SpecDatas.Length != (int)devCurrent.SpecLength)
                {
                    Msg.Show(Info.strErrorSpecLength);
                    return;
                }
            }

            if (lst.Name != info.Name.Replace(".spec", "")) lst.Name = info.Name.Replace(".spec", "");
            //if (null != lstAll.Find(w => w.Name == lst.Name))
            if (dt_SpecList.Select("name = '" + lst.Name + "'").Length > 0)
            {
                backUpSpecList.Clear();
                Msg.Show(Info.SilimarSpectrum + "\"" + lst.Name + "\"");
                return;
            }
            backUpSpecList.Add(lst);
        }

        private void GetBackUpSpecList(string fileFullName,string fileShortName, ref List<BackUpSpecList> backUpSpecList)
        {
            BackUpSpecList lst = SerializeHelper.FileToObj(fileFullName) as BackUpSpecList;
            foreach (var sp in lst.BackUpSpecLst)
            {
                Spec spec = Spec.New.Init(sp.Name, sp.SpecData, sp.SpecTime, sp.UsedTime, sp.TubVoltage, sp.TubCurrent, "");
                spec.IsSmooth = true;
                Device devCurrent = WorkCurveHelper.DeviceCurrent;//当前设备

                if (devCurrent == null || spec == null || spec.SpecDatas.Length != (int)devCurrent.SpecLength)
                {
                    Msg.Show(Info.strErrorSpecLength);
                    return;
                }
            }

            if (lst.Name != fileShortName.Replace(".spec", "")) lst.Name = fileShortName.Replace(".spec", "");
            //if (null != lstAll.Find(w => w.Name == lst.Name))
            if (dt_SpecList.Select("name = '" + lst.Name + "'").Length > 0)
            {
                backUpSpecList.Clear();
                Msg.Show(Info.SilimarSpectrum + "\"" + lst.Name + "\"");
                return;
            }
            backUpSpecList.Add(lst);
        }
        private void SaveBackUpSpecList(List<BackUpSpecList> backUpSpecList)
        {
            foreach (BackUpSpecList lst in backUpSpecList)
            {
                SpecList sl = SpecList.New.Init(lst.Name, lst.SampleName, lst.Supplier, lst.Weight, lst.Shape, lst.Operater, lst.SpecDate, lst.SpecSummary, lst.SpecType);
                sl.Color = lst.Color;
                sl.VirtualColor = lst.VirtualColor;
                if (!lst.IsNewType)
                    sl.Image = lst.Image;
                if (WorkCurveHelper.DeviceCurrent.Conditions.Count > 0)
                {
                    Skyray.EDXRFLibrary.Condition cond = WorkCurveHelper.DeviceCurrent.Conditions.ToList().Find(c => c.Id == lst.ConditionId);
                    if (cond != null)
                    {
                        sl.Condition = cond;
                    }
                    else
                    {
                        cond = WorkCurveHelper.DeviceCurrent.Conditions.ToList().Find(c => c.Name == "copy");
                        if (cond != null)
                        {
                            sl.Condition = cond;
                        }
                        else
                        {
                            Skyray.EDXRFLibrary.Condition con = Skyray.EDXRFLibrary.Condition.New.Init("copy");
                            InitParameter ip = Default.GetInitParameter(WorkCurveHelper.DeviceCurrent.SpecLength);
                            con.InitParam = ip;
                            DeviceParameter dp = Default.GetDeviceParameter(WorkCurveHelper.DeviceCurrent.SpecLength,1);
                            con.DeviceParamList.Add(dp);
                            con.Device = WorkCurveHelper.DeviceCurrent;
                            sl.Condition = con;
                            con.Save();
                            WorkCurveHelper.DeviceCurrent.Conditions.Add(con);
                        }
                    }
                }
                else
                {
                    Skyray.EDXRFLibrary.Condition con = Skyray.EDXRFLibrary.Condition.New.Init("copy");
                    InitParameter ip = Default.GetInitParameter(WorkCurveHelper.DeviceCurrent.SpecLength);
                    con.InitParam = ip;
                    DeviceParameter dp = Default.GetDeviceParameter(WorkCurveHelper.DeviceCurrent.SpecLength,1);
                    con.DeviceParamList.Add(dp);
                    con.Device = WorkCurveHelper.DeviceCurrent;
                    sl.Condition = con;
                    con.Save();
                    WorkCurveHelper.DeviceCurrent.Conditions.Add(con);
                }
                foreach (var sp in lst.BackUpSpecLst)
                {
                    Spec spec = Spec.New.Init(sp.Name, sp.SpecData, sp.SpecTime, sp.UsedTime, sp.TubVoltage, sp.TubCurrent, "");
                    spec.IsSmooth = true;
                    //spec.SpecList = sl;
                    spec.DeviceParameter = sl.Condition.DeviceParamList[0];
                    sl.Specs.Add(spec);
                    
                }

                SpecListEntity etity = new SpecListEntity();
                if (sl.WorkCurveId != 0)
                    etity.WorkCurveName = WorkCurve.FindById(sl.WorkCurveId).Name;
                Device device = Device.FindOne(w => w.IsDefaultDevice == true);
                if (device != null)
                    etity.DeviceName = device.Name;
                etity.Name = sl.Name.Replace("*", "");
                etity.SampleName = (sl.SampleName != null) ? sl.SampleName : etity.Name;
                etity.Supplier = sl.Supplier;
                etity.Loss = sl.Loss;
                etity.Weight = sl.Weight;
                etity.Shape = sl.Shape;
                etity.SpecDate = sl.SpecDate;
                etity.Operater = sl.Operater;
                etity.SpecSummary = sl.SpecSummary;
                etity.SpecType = sl.SpecType;
                etity.Color = sl.Color;
                etity.VirtualColor = sl.VirtualColor;
                int count = sl.Specs.Count;
                etity.InitParam = sl.Condition.InitParam.ConvertToNewEntity();
                etity.DemarcateEnergys = Default.ConvertFormOldToNew(sl.Condition.DemarcateEnergys, WorkCurveHelper.DeviceCurrent.SpecLength);
                etity.Specs = new SpecEntity[count];
                for (int i = 0; i < count; i++)
                    etity.Specs[i] = sl.Specs[i].ConvertSpecToNew();

                SQliteDAL savDal = new SQliteDAL();
                etity.DeviceName=WorkCurveHelper.DeviceCurrent.Name;
                savDal.Save(etity);
            }
        }

        #endregion

        #endregion

        #region 导出数据

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            //string path = Application.StartupPath + @"\BackUp\" + DateTime.Now.Date.ToShortDateString() + @"\";
            if (DialogResult.OK == this.fbdImport.ShowDialog())
            {
                if (this.fbdImport.SelectedPath.IsNullOrEmpty())
                {
                    MessageBox.Show("选择路径！路径不能为空！");
                    return;
                }
                else
                {
                    string path = this.fbdImport.SelectedPath + @"\" + DateTime.Now.Date.ToString("yyyyMMddhhmmss") + @"\";
                    if (!Directory.Exists(path))
                    {
                        DirectoryInfo info = new DirectoryInfo(path);
                        info.Create();
                    }
                    //修改：何晓明 20110701 谱名称3列显示
                    //int count = dgvwSample.SelectedRows.Count;
                    int count = lstGetSpecList.Count;
                    //
                    if (count > 0)
                    {
                        if (WorkCurveHelper.SpecExportType==1)
                        {
                            ExportSpecType2(count,path);
                        }
                        else if(WorkCurveHelper.SpecExportType==3)
                        {
                            ExportSpecTypeMCA(count,path);
                        }
                        else 
                        {
                             ExportSpecType1(count, path);
                        }
                        btnSearch_Click(null, null);
                        if (DialogResult.OK == SkyrayMsgBox.Show(Info.ExportSuccess, MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                        {
                            System.Diagnostics.Process.Start("iexplore.exe", path);
                        }
                    }
                    else
                    { SkyrayMsgBox.Show(Info.NoSelect); }
                }
            }
        }

        private void ExportSpecType1(int count, string path)
        {
            ISpectrumDAL dal = WorkCurveHelper.GetExportType(path);
            for (int i = count - 1; i >= 0; i--)
            {
                SpecListEntity back = lstGetSpecList[i];
                back.DeviceName=WorkCurveHelper.DeviceCurrent.Name;
                dal.Save(back);
                if (WorkCurveHelper.DelAfterExport)
                {
                    //SpectrumData data = SpectrumData.FindOne(w => w.Name == back.Name&&w.DeviceName==wo);
                    //data.Delete();
                    WorkCurveHelper.DataAccess.DeleteRecord(back.Name);
                    if (WorkCurveHelper.VirtualSpecList != null && WorkCurveHelper.VirtualSpecList.Count > 0)
                    {
                        var tempList = WorkCurveHelper.VirtualSpecList.Find(s => s.Name == back.Name);
                        if (tempList != null) WorkCurveHelper.VirtualSpecList.Remove(tempList);
                    }
                }
            }
        }

        private void ExportSpecType2(int count, string path)
        {
            List<SpecListEntity> lstDel = new List<SpecListEntity>();
            for (int i = count - 1; i >= 0; i--)
            {
                SpecListEntity back = lstGetSpecList[i];
                int conditionCount = 0;
                foreach (var s in back.Specs)
                {
                    string fileName = path + back.Name +"_"+ conditionCount.ToString() + ".txt";
                    using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
                    {
                        using (StreamWriter sw = new StreamWriter(fileStream))
                        {
                            sw.WriteLine("TestTime=" + s.UsedTime);
                            sw.WriteLine("TubCurrent=" + s.TubCurrent);
                            sw.WriteLine("TubVoltage=" + s.TubVoltage);
                            sw.WriteLine("Filter=1");
                            sw.WriteLine("Collimator=1");
                            sw.WriteLine("Vacuumed=false");
                            sw.WriteLine("AdjustRate=false");
                            sw.WriteLine("Channel=" + s.SpecDatas.Length);
                            sw.WriteLine("Data=");
                            StringBuilder data = new StringBuilder();
                            int[] intSpec = Helper.ToInts(s.SpecData);
                            //int[] intSpec = s.SpecDatas;
                            foreach (var m in intSpec)
                            {
                                data.Append(m + "\r\n");
                            }
                            sw.WriteLine(data.ToString());
                        }
                    }
                    conditionCount++;
                }
                lstDel.Add(back);
            }
            foreach (var specList in lstDel)
            {
                ////Spec.DeleteAll(s => s.SpecList.Id == specList.Id);
                //DataRow[] drs = dt_SpecList.Select("name = '" + specList.Name + "'");
                //if (drs != null && drs.Length > 0)
                //{
                //    dt_SpecList.Rows.Remove(drs[0]);
                //    dt_SpecList.AcceptChanges();
                //}
                //lstGetSpecList.Remove(specList);
                //lstOriginal = lstSpecList;
                if (WorkCurveHelper.VirtualSpecList != null && WorkCurveHelper.VirtualSpecList.Count > 0)
                {
                    var tempList = WorkCurveHelper.VirtualSpecList.Find(s => s.Name == specList.Name);
                    if (tempList != null) WorkCurveHelper.VirtualSpecList.Remove(tempList);
                }
                //specList.Delete();
                if (WorkCurveHelper.DelAfterExport)
                WorkCurveHelper.DataAccess.DeleteRecord(specList.Name);
            }
        }

        private void ExportSpecTypeMCA(int count, string path)
        {
            List<SpecListEntity> lstDel = new List<SpecListEntity>();
            for (int i = count - 1; i >= 0; i--)
            {
                SpecListEntity back = lstGetSpecList[i];
                int conditionCount = 0;
                foreach (var s in back.Specs)
                {
                    string fileName = path + back.Name + "_" + conditionCount.ToString() + ".mca";
                    using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
                    {
                        using (StreamWriter sw = new StreamWriter(fileStream))
                        {
                            sw.WriteLine("<<PMCA SPECTRUM>>");
                            sw.WriteLine("TAG - live_data");
                            sw.WriteLine("DESCRIPTION - ");
                            sw.WriteLine("GAIN - 4");
                            sw.WriteLine("THRESHOLD - 0");
                            sw.WriteLine("LIVE_MODE - 0");
                            sw.WriteLine("PRESET_TIME - 0");
                            sw.WriteLine("LIVE_TIME - "+s.UsedTime);
                            sw.WriteLine("REAL_TIME - "+s.UsedTime);
                            sw.WriteLine("START_TIME - "+back.SpecDate.ToString());
                            sw.WriteLine("SERIAL_NUMBER - 0");
                            sw.WriteLine("<<DATA>>");
                            StringBuilder data = new StringBuilder();
                            int[] intSpec = Helper.ToInts(s.SpecData);
                            //int[] intSpec = s.SpecDatas;
                            foreach (var m in intSpec)
                            {
                                data.Append(m + "\r\n");
                            }
                            sw.WriteLine(data.ToString());
                            sw.WriteLine("<<END>>");
                        }
                    }
                    conditionCount++;
                }
                lstDel.Add(back);
            }
            foreach (var specList in lstDel)
            {
                ////Spec.DeleteAll(s => s.SpecList.Id == specList.Id);
                //DataRow[] drs = dt_SpecList.Select("name = '" + specList.Name + "'");
                //if (drs != null && drs.Length > 0)
                //{
                //    dt_SpecList.Rows.Remove(drs[0]);
                //    dt_SpecList.AcceptChanges();
                //}
                //lstGetSpecList.Remove(specList);
                //lstOriginal = lstSpecList;
                if (WorkCurveHelper.VirtualSpecList != null && WorkCurveHelper.VirtualSpecList.Count > 0)
                {
                    var tempList = WorkCurveHelper.VirtualSpecList.Find(s => s.Name == specList.Name);
                    if (tempList != null) WorkCurveHelper.VirtualSpecList.Remove(tempList);
                }
                //specList.Delete();
                if (WorkCurveHelper.DelAfterExport)
                WorkCurveHelper.DataAccess.DeleteRecord(specList.Name);
            }
        }

        #endregion

        #region 其他控件操作
        /// <summary>
        /// 控制显示谱数量控制，显示一列还是多列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExpand_Click(object sender, EventArgs e)
        {

            Expand();
            btnSearch_Click(null, null);

            if (dt_SpecList != null && dt_SpecList.Rows.Count>0)
                GetSpec();
           
        }

        private void Expand()
        {
            if (this.btnExpand.Text == ">>")
            {
                this.btnExpand.Text = "<<";
                this.splitContainer1.SplitterDistance = this.splitContainer1.Width - 21;
                //this.splitContainer1.SplitterDistance = 757;
                //BindCheckInfo();
                Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedValue("OpenSelectSampleType", "Type", "1");
                //foreach (int i in SelectedItems)
                //{
                //    HighPerformanceListView1.Items[i].Selected = true;
                //}
                //HighPerformanceListView1.Focus();
            }
            else if (this.btnExpand.Text == "<<")
            {
                this.btnExpand.Text = ">>";
                this.splitContainer1.SplitterDistance = 212;
                //BindCheckInfo();
                Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedValue("OpenSelectSampleType", "Type", "0");
                //foreach (int i in SelectedItems)
                //{
                //    HighPerformanceListView1.Items[i].Selected = true;
                //}
                //HighPerformanceListView1.Focus();

            }
        }


        #endregion



        #region Fields

        //修改：何晓明 20110630 缓存数据直接从数据库读取
        private string sql = string.Empty;
       // private AddSpectrumType internalSpectrumType = AddSpectrumType.OpenVisualSpec;
        string conditionSql = string.Empty;
        string pageSql = string.Empty;
        string nameSql = string.Empty;
        string dateSql = string.Empty;
        string specTypeOrderSql = string.Empty;
        string nameOrderSql = string.Empty;
        string datetimeOrderSql = string.Empty;
        //

        int DisplayStartIdx = 0;

        private bool _ShowFilterBox;

        /// <summary>
        /// 数据库中查询所得的谱信息列表
        /// </summary>
        //public List<SpecListEntity> lstSpecList;

        /// <summary>
        /// 当前选择的谱
        /// </summary>
        public SpecListEntity specListCurrent;
        private long specListCurrentId;

        /// <summary>
        /// 当前选择的谱列表
        /// </summary>
        public List<SpecListEntity> SelectedSpecList;

        /// <summary>
        /// 上次所选对比谱
        /// </summary>
        private List<SpecListEntity> SelectedSpecListOld;

        /// <summary>
        /// 当前选择的谱名称
        /// </summary>
        public List<string> SpecCurrentName;

        /// <summary>
        /// 对话结果
        /// </summary>
        public DialogResult DialogResult { get; set; }


        private AddSpectrumType type;

        private bool selectVisual = false;//判断是否打开对比谱

        public bool ShowFilterBox
        {
            get { return _ShowFilterBox; }
            set
            {
                _ShowFilterBox = value;
            }
        }

        //private List<SpecListEntity> lstOriginal;
        #endregion

        #region 构造函数

        /// <summary>
        /// 获取文本
        /// </summary>
        public override void SetText()
        {
            //this.dgvwSample.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        }

        public class ValueObject
        {
            private string _name;
            private string _value;

            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            public string Value
            {
                get { return _value; }
                set { _value = value; }
            }
        }

        
        public bool IsCaculate = true;


        
        //修改：何晓明 20110630 缓存数据直接从数据库读取
//<<<<<<< .mine
//        //private void InitPage()
//        //{
//        //    conditionSql = sql.Substring(0, sql.IndexOf("order by"));
//        //    string countSql = "select count(*) from (" + conditionSql + nameSql + dateSql + ")";
//        //    object o = Lephone.Data.DbEntry.Context.ExecuteScalar(countSql);
//        //    int recordTotal = 0;
//        //    int.TryParse(o.ToString(), out recordTotal);
//        //}
//=======
//        private void InitPage()
//        {
//            //conditionSql = sql.Substring(0, sql.IndexOf("order by"));
//            //string countSql = "select count(*) from (" + conditionSql + nameSql + dateSql + ")";
//            //object o = Lephone.Data.DbEntry.Context.ExecuteScalar(countSql);
//            //int recordTotal = 0;
//            //int.TryParse(o.ToString(), out recordTotal);
//        }
//>>>>>>> .r2235

//        /// <summary>
        /// 组合查询条件
        /// </summary>
        //private void GetSearchCondition()
        //{
        //    pageSql = string.Empty;
        //    specTypeOrderSql = "Order by SpecType asc";

        //    if (chkName.Checked)
        //    {
        //        nameSql = " And Name like '%" + txtKeywords.Text.Trim() + "%'";
        //    }
        //    else
        //    {
        //        nameSql = string.Empty;
        //    }
        //    if (chkDate.Checked)
        //    {
        //        dateSql = "And datetime(SpecDate) <=datetime('" + dateTimePickerTo.Value.ToString("yyyy-MM-dd") + " 23:59:59')" + " and datetime(SpecDate)>=datetime('" + dateTimePickerFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00')";
        //    }
        //    else
        //    {
        //        dateSql = string.Empty;
        //    }
        //    if (chkNameOrder.Checked)
        //    {
        //        if (cobNameOrder.SelectedIndex == 0)
        //            nameOrderSql = ",Name asc";
        //        else
        //            nameOrderSql = ",Name desc";
        //    }
        //    else
        //    {
        //        nameOrderSql = string.Empty;
        //    }
        //    if (chkTimeOrder.Checked)
        //    {
        //        if (cobTimeOrder.SelectedIndex == 0)
        //            datetimeOrderSql = ",SpecDate asc";
        //        else
        //            datetimeOrderSql = ",SpecDate desc";
        //    }
        //    else
        //    {
        //        datetimeOrderSql = string.Empty;
        //    }
        //    //pageSql = specTypeOrderSql + datetimeOrderSql + nameOrderSql + limitSql;
        //    pageSql = specTypeOrderSql + datetimeOrderSql + nameOrderSql;
        //}

//<<<<<<< .mine
        //private void ChangePageInfo()
        //{
        //    if (pageSql == string.Empty)
        //        pageSql = " order by SpecType asc,SpecDate desc,Name asc ";
        //    //else
        //    //    pageSql = specTypeOrderSql + datetimeOrderSql + nameOrderSql + pageSql;
        //    switch (internalSpectrumType)
        //    {
        //        case AddSpectrumType.OpenSpectrum://实谱
        //            lstSpecList = SpecList.FindBySql(conditionSql + nameSql + dateSql + pageSql);//GetSpecList();
        //            break;
        //        case AddSpectrumType.OpenVisualSpec://对比谱
        //            //btnDel.Visible = true;
        //            this.btWSelectDemo.Visible = true;
        //            selectVisual = true;
        //            lstSpecList = SpecList.FindBySql(conditionSql + nameSql + dateSql + pageSql);
        //            break;
        //        case AddSpectrumType.OpenStandardSpec://标准谱
        //            //btnDel.Visible = true;
        //            //lstSpecList = GetSampleSpecList();
        //            lstSpecList = SpecList.FindBySql(conditionSql + nameSql + dateSql + pageSql);//GetSpecList();
        //            break;
        //        case AddSpectrumType.OpenPureSpec://纯元素谱
        //            //btnDel.Visible = true;
        //            lstSpecList = SpecList.FindBySql(conditionSql + nameSql + dateSql + pageSql);//GetPureSpecList();
        //            break;
        //        case AddSpectrumType.OpenExploreSpec://智能谱
        //            //btnDel.Visible = true;
        //            lstSpecList = SpecList.FindBySql(conditionSql + nameSql + dateSql + pageSql);//GetExploreSpecList();
        //            break;
        //        default:
        //            break;
        //    }
        //    GetSpec();
        //    lstOriginal = lstSpecList;
        //    InitPage();
        //}
//=======
//        private void ChangePageInfo()
//        {
//            if (pageSql == string.Empty)
//                pageSql = " order by SpecType asc,SpecDate desc,Name asc ";
//            //else
//            //    pageSql = specTypeOrderSql + datetimeOrderSql + nameOrderSql + pageSql;
//            switch (internalSpectrumType)
//            {
//                case AddSpectrumType.OpenSpectrum://实谱
//                    lstSpecList = SpecList.FindBySql(conditionSql + nameSql + dateSql + pageSql);//GetSpecList();
//                    break;
//                case AddSpectrumType.OpenVisualSpec://对比谱
//                    //btnDel.Visible = true;
//                    this.btWSelectDemo.Visible = true;
//                    selectVisual = true;
//                    lstSpecList = SpecList.FindBySql(conditionSql + nameSql + dateSql + pageSql);
//                    break;
//                case AddSpectrumType.OpenStandardSpec://标准谱
//                    //btnDel.Visible = true;
//                    //lstSpecList = GetSampleSpecList();
//                    lstSpecList = SpecList.FindBySql(conditionSql + nameSql + dateSql + pageSql);//GetSpecList();
//                    break;
//                case AddSpectrumType.OpenPureSpec://纯元素谱
//                    //btnDel.Visible = true;
//                    lstSpecList = SpecList.FindBySql(conditionSql + nameSql + dateSql + pageSql);//GetPureSpecList();
//                    break;
//                case AddSpectrumType.OpenExploreSpec://智能谱
//                    //btnDel.Visible = true;
//                    lstSpecList = SpecList.FindBySql(conditionSql + nameSql + dateSql + pageSql);//GetExploreSpecList();
//                    break;
//                default:
//                    break;
//            }
//            GetSpec();
//            lstOriginal = lstSpecList;
//            //InitPage();
//        }
//>>>>>>> .r2235

        private void BindCheckInfo()
        {
            for (int i = 0; i < HighPerformanceListView1.Items.Count; i++)
            {
                if (HighPerformanceListView1.Items[i] == null)
                    continue;
                var spec = lstGetSpecList.Find(w => w.Name == HighPerformanceListView1.Items[i].Text);
                if (spec != null)
                {
                    HighPerformanceListView1.Items[i].Selected = true;
                }
            }
        }
        //private string sOrderType;

        /// <summary>
        /// 每页显示的数据的行数
        /// </summary>
        //private int ROWS_PER_PAGE = 99999999;       


        /// <summary>
        /// 加载实谱
        /// </summary>
        //private DbObjectList<SpecList> GetSpecList()
        //{
        //    this.btnImport.Visible = true;
        //    this.btnExport.Visible = true;
        //    //修改：何晓明 20110630 缓存数据直接从数据库读取
        //    //string sql = "select * from SpecList ";
        //    //
        //    sql = "select * from SpecList ";
        //    if (WorkCurveHelper.DeviceCurrent != null)
        //    {
        //        var deviceId = WorkCurveHelper.DeviceCurrent.Id;
        //        //sql += " where Condition_Id in (select Id from condition where Device_id = " + deviceId + " and Type = 0)";
        //        sql += " where Condition_Id in (select Id from condition where Device_id = " + deviceId + ")";
        //    }
        //    if (GP.CurrentUser.Role.RoleType == 2)
        //    {
        //        sql += " and SpecType = 2";
        //    }
        //    sql += " order by SpecType asc,SpecDate desc,Name asc"; 
        //    return SpecList.FindBySql(sql);
        //}



        
        /// <summary>
        /// 加载对比谱
        /// </summary>
        //private DbObjectList<SpecList> GetVisualSpecList()
        //{
        //    //修改：何晓明 20110630 缓存数据直接从数据库读取
        //    //string sql = "select distinct c.* from speclist c inner join spec d on c.Id = d.speclist_id where d.deviceparameter_id  in" +
        //    //
        //    sql = "select distinct c.* from speclist c inner join spec d on c.Id = d.speclist_id where d.deviceparameter_id  in" +
        //        "(select b.deviceparameter_id from speclist a  inner join spec b on a.Id = b.speclist_id where a.Id = " + WorkCurveHelper.MainSpecList.Id +
        //        " ) and exists (select h.Id from SpecList h inner join Condition  j on h.Condition_Id=j.Id where h.id =c.id and j.Device_id=" + WorkCurveHelper.DeviceCurrent.Id + ")";
        //    sql += " order by SpecType asc,SpecDate desc,Name asc ";

        //    return SpecList.FindBySql(sql);
        //}

        /// <summary>
        /// 加载纯元素谱
        /// </summary>
        //private DbObjectList<SpecList> GetPureSpecList()
        //{
        //    //修改：何晓明 20110630 缓存数据直接从数据库读取
        //    //string sql = "select * from SpecList ";
        //    //
        //    sql = "select * from SpecList ";
        //    if (WorkCurveHelper.DeviceCurrent != null)
        //    {
        //        var deviceId = WorkCurveHelper.DeviceCurrent.Id;
        //        sql += " where Condition_Id in (select Id from condition where Type = 0 and Device_id = " + deviceId + ") and SpecType = 1";
        //    }
        //    sql += " order by SpecType asc,SpecDate desc,Name asc ";

        //    return SpecList.FindBySql(sql);
        //}

        /// <summary>
        /// 加载标样谱
        /// </summary>
        /// <returns></returns>
        //private DbObjectList<SpecList> GetSampleSpecList()
        //{
        //    //修改：何晓明 20110630 缓存数据直接从数据库读取
        //    //string sql = "select * from SpecList ";
        //    //
        //    sql = "select * from SpecList ";
        //    if (WorkCurveHelper.DeviceCurrent != null)
        //    {
        //        var deviceId = WorkCurveHelper.DeviceCurrent.Id;
        //        sql += " where Condition_Id in (select Id from condition where Type = 0 and Device_id = " + deviceId + ") and SpecType == 0";
        //    }
        //    sql += " order by SpecType asc,SpecDate desc,Name asc ";

        //    return SpecList.FindBySql(sql);
        //}

        /// <summary>
        /// 加载智能谱
        /// </summary>
        /// <returns></returns>
        //private DbObjectList<SpecList> GetExploreSpecList()
        //{
        //    //修改：何晓明 20110630 缓存数据直接从数据库读取
        //    //string sql = "select * from SpecList ";
        //    //
        //    sql = "select * from SpecList ";
        //    if (WorkCurveHelper.DeviceCurrent != null)
        //    {
        //        var deviceId = WorkCurveHelper.DeviceCurrent.Id;
        //        sql += " where Condition_Id in (select Id from condition where Type = 2 and Device_id = " + deviceId + ") and SpecType = 1";
        //    }
        //    sql += " order by SpecType asc,SpecDate desc,Name asc ";

        //    return SpecList.FindBySql(sql);
        //}

        private ListViewExampleProvider listViewProvider = new ListViewExampleProvider();
        /// <summary>
        /// 加对比谱
        /// </summary>
        public SelectSample(List<SpecListEntity> lst)
        {
            InitializeComponent();
            SelectedSpecListOld = new List<SpecListEntity>();
            foreach (var v in lst)
            {
                SelectedSpecListOld.Add(WorkCurveHelper.DataAccess.Query(v.Name));
            }
            btnDel.Visible = false;
            SpecCurrentName = new List<string>();
            //if (WorkCurveHelper.MainSpecList != null && WorkCurveHelper.MainSpecList.Condition.Type == ConditionType.Normal)
            //    lstSpecList = GetVisualSpecList();
            //else
            //lstSpecList = GetSpecList();
            RegisterHighPerformanceListView();
            GetCurrentDateSpec();
            dt_SpecList = GetSpecList();
            selectVisual = true;
            GetSpec();//获取谱列表
            selectVir();
            //lstOriginal = lstSpecList;
            //修改：何晓明 20110630 缓存数据直接从数据库读取
            //InitPage();
            SetcobTimeAndName();
            xcSpec.IsRadiation = false;
        }

        List<int> SelectedItems = new List<int>(); 

        private void selectVir()
        {
            //ColVisualSpec.Visible = true;
            SelectedSpecList = SelectedSpecListOld;
            //修改：何晓明 20110701 谱名称3列显示
            //int count = dgvwSample.Rows.Count;
            int count = HighPerformanceListView1.Items.Count;
            //
            for (int i = 0; i < count; i++)
            {
                //修改：何晓明 20110701 谱名称3列显示
                //long id = lstSpecList[i].Id;                
                //SpecList sl = SelectedSpecListOld.Find(s => s.Id == id);
                string name = HighPerformanceListView1.Items[i].Text;
                long id = 0;
                long.TryParse(HighPerformanceListView1.Items[i].Name,out id);
                SpecListEntity sl = SelectedSpecListOld.Find(s =>s!=null && s.Name == name);
                //
                if (sl != null)
                {
                    //修改：何晓明 20110701 谱名称3列显示
                    //dgvwSample.Rows[i].Cells["ColVisualSpec"].Value = true;
                    //listViewWSpecList.Items[i].Checked = true; 
                    HighPerformanceListView1.Items[i].Selected = true;
                    SelectedItems.Add(i);
                    //
                }
            }

        }

        /// <summary>
        /// 加纯元素谱
        /// </summary>
        //public SelectSample(List<SpecListEntity> splist)
        //{
        //    InitializeComponent();
        //    btnDel.Visible = false;
        //    SpecCurrentName = new List<string>();
        //    RegisterHighPerformanceListView();
        //    lstSpecList = splist;
        //    GetCurrentDateSpec();
        //    GetSpec();//获取谱列表
        //    lstOriginal = lstSpecList;
        //}

        #endregion

        #region Methods

       

        ///// <summary>
        ///// SetCurrentSpecInfo
        ///// </summary>
        ///// <param name="index"></param>
        //private void SetCurrentSpecInfo(int index)
        //{
        //    specListCurrent = lstSpecList[index];
        //    if (selectVisual)
        //    {
        //        txtColor.BackColor = specListCurrent.VirtualColor == 0 ? DifferenceDevice.DefaultSpecColor : Color.FromArgb(specListCurrent.VirtualColor);
        //    }
        //    else
        //    {
        //        txtColor.BackColor = specListCurrent.Color == 0 ? DifferenceDevice.DefaultSpecColor : Color.FromArgb(specListCurrent.Color);
        //    }
        //    colDlg.Color = txtColor.BackColor;
        //    BindValue();
        //    DrawSpec(txtColor.BackColor);
        //}
        //修改：何晓明 20110701 谱名称3列显示        
        private void SetCurrentSpecInfo(SpecListEntity SpecListCurrent,string strSpecID)
        {
            this.specListCurrent = SpecListCurrent;
            this.specListCurrentId =strSpecID!=string.Empty? long.Parse(strSpecID):0;
            if (selectVisual)
            {
                txtColor.BackColor = specListCurrent.VirtualColor == 0 ? DifferenceDevice.DefaultSpecColor : Color.FromArgb(specListCurrent.VirtualColor);
            }
            else
            {
                txtColor.BackColor = specListCurrent.Color == 0 ? DifferenceDevice.DefaultSpecColor : Color.FromArgb(specListCurrent.Color);
            }
            colDlg.Color = txtColor.BackColor;
            BindValue();
            DrawSpec(txtColor.BackColor);
        }
        

        /// <summary>
        /// 绑定值到控件
        /// </summary>
        private void BindValue()
        {
            BindHelper.BindTextToCtrl(txtSampleName, specListCurrent, "SampleName", true); ////样品名称
            BindHelper.BindTextToCtrl(txtSpecDate, specListCurrent, "SpecDate", true); ////测量日期
            BindHelper.BindTextToCtrl(txtSupplier, specListCurrent, "Supplier", true);  ////供应商
            BindHelper.BindTextToCtrl(txtShape, specListCurrent, "Shape", true);  ////形状
            BindHelper.BindTextToCtrl(txtWeight, specListCurrent, "Weight", true);  ////重量
            BindHelper.BindTextToCtrl(txtSummary, specListCurrent, "SpecSummary", true);  
        }

        /// <summary>
        /// 画谱图
        /// </summary>
        private void DrawSpec(Color color)
        {
            //xcSpec.MultiPanel(null, specListCurrent, new List<SpecList>(), null, color.ToArgb());
            MultiPanel(xcSpec, specListCurrent, this.CreateGraphics(), color);
        }

        /// <summary>
        /// 创建多格谱图对象
        /// </summary>
        /// <param name="xrf"></param>
        /// <param name="specList"></param>
        /// <param name="g"></param>
        private void MultiPanel(XRFChart xrf, SpecListEntity specList, Graphics g, Color color)
        {
            MasterPane myMaster = xrf.MasterPane;
            myMaster.PaneList.Clear();
            myMaster.Fill = new Fill(Color.Transparent, Color.Transparent, 45.0F);
            myMaster.Legend.IsVisible = true;
            myMaster.Legend.Position = LegendPos.TopCenter;
            int SpecCount = specList.Specs.Length;
            for (int j = 0; j < SpecCount; j++)
            {
                GraphPane myPane = CreatePanel(specList.Specs[j], new Rectangle(10, 10, 10, 10), color, true);
                xrf.MasterPane.Add(myPane);
            }
            myMaster.DoLayout(g);
            myMaster.AxisChange(g);
            g.Dispose();
            xrf.EnableMoveLine = false;
            xrf.IsUseScroll = false;
            xrf.IsRadiation = false;
            xrf.EnableWheel = false;
            foreach (var pane in myMaster.PaneList)
            {
                float scaleFactor = pane.CalcScaleFactor();
            }
            xrf.Refresh();
        }

        /// <summary>
        /// 创建谱图panel
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        /// <param name="isFill"></param>
        /// <returns></returns>
        private GraphPane CreatePanel(SpecEntity spec, Rectangle rect, Color color, bool isFill)
        {
            GraphPane myPane = new GraphPane(rect, "", "", "");
            myPane.IsFontsScaled = false;
            myPane.Title.IsVisible = false;
            myPane.Fill = new Fill(Color.Transparent, Color.Transparent, 45.0F);
            myPane.BaseDimension = 8.0F;
            PointPairList list = new PointPairList();
            int[] arrPoints = spec.SpecDatas;
            //arrPoints = Helper.Smooth(arrPoints, (int)spec.SpecList.Condition.Device.SpecLength);
            for (int i = 0; i < arrPoints.Length; i++)
            {
                double x = i;
                double y = double.Parse(arrPoints[i].ToString());
                list.Add(x, y);
            }
            LineItem myCurve = myPane.AddCurve("", list, color, SymbolType.None);
            if (isFill)
            {
                myCurve.Line.Fill = new Fill(color, color, 180f);
            }
            if (list.Count > 0)
                myPane.XAxis.Scale.Max = list.OrderByDescending(d => d.X).ToList()[0].X;
            return myPane;
        }

        #endregion

        #region Event

        /// <summary>
        /// 关掉form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (selectVisual)
            {
                SelectedSpecList = SelectedSpecListOld;
                foreach (var c in WorkCurveHelper.DefaultVirtualColor)
                {
                    if (SelectedSpecList.Find(ss => ss.VirtualColor == c.Color.ToArgb()) == null)
                    { c.BeSelected = false; }
                }
            }
            EDXRFHelper.GotoMainPage(this);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            //修改：何晓明 20110701 谱名称3列显示
            //int count = dgvwSample.SelectedRows.Count;
            int count = lstGetSpecList.Count;

            //删除谱时，检验是否在工作曲线中充当和使用为匹配谱
            bool isExitStandSample = false;
            bool isPureOrStand = false;
            string ssplistID = "";
            foreach (SpecListEntity splist in lstGetSpecList)
            {
                ssplistID += splist.Name.ToString() + ",";
                if (splist.SpecType == SpecType.PureSpec || splist.SpecType == SpecType.StandSpec)
                    isPureOrStand = true;
            }
            if (isPureOrStand)
            {
                UCPWDLock uc = new UCPWDLock(true);
                WorkCurveHelper.OpenUC(uc, false, Info.PWDLock,true);
                if (uc.DialogResult == DialogResult.No)
                {
                    return;
                }
            }
            ssplistID = (ssplistID.Length > 0) ? ssplistID.Substring(0, ssplistID.Length - 1) : "";
            //if (ssplistID != "")
            //{
            //    List<StandSample> ListStandSample = StandSample.FindBySql("select * from StandSample where MatchSpecListId in (" + ssplistID + ")");
            //    if (ListStandSample.Count > 0) { isExitStandSample = true;}
            //}

            //现在匹配谱信息已加到ElementList中
            if (ssplistID != "")
            {
                List<ElementList> listElementList = ElementList.FindAll();
                foreach (var mathElementList in listElementList)
                {
                    if (string.IsNullOrEmpty(mathElementList.MatchSpecListIdStr))
                        continue;
                   string[] strs = mathElementList.MatchSpecListIdStr.Split(',');
                   List<string> temp = new List<string>();
                   for (int i = 0; i < strs.Length; i++)
                       if (i % 2 == 0)
                           temp.Add(strs[i]);
                   var tt = temp.Intersect(ssplistID.Split(',').ToList());
                    if (tt != null && tt.Count() > 0)
                    {
                        isExitStandSample = true;
                        break;
                    }
                }
            }
          
            if (isExitStandSample && Skyray.Controls.SkyrayMsgBox.Show(Info.isExitMatchSpecList, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                return;
            if (count > 0)
            {
                DialogResult dr = SkyrayMsgBox.Show(Info.ConfirmDel, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.OK)
                {

                    string[] speclistId = ssplistID.Split(',');
                    foreach (string sSpecListId in speclistId)
                    {
                        var specListExists = DifferenceDevice.interClassMain.selectSpeclist.Find(w=>w.Name.ToString()==sSpecListId);
                        if(specListExists!=null)
                        {
                            Msg.Show("\""+specListExists.Name+"\""+Info.DeleteSpecListsFail);
                            return;
                        }
                        if (WorkCurveHelper.VirtualSpecList != null && WorkCurveHelper.VirtualSpecList.Count > 0)
                        {
                            var tempList = WorkCurveHelper.VirtualSpecList.Find(s => s.Name == sSpecListId);
                            if (tempList != null) WorkCurveHelper.VirtualSpecList.Remove(tempList);
                        }

                        if (!string.IsNullOrEmpty(sSpecListId))
                        {
                            FileInfo file = new FileInfo(WorkCurveHelper.SaveSamplePath + "\\" + sSpecListId + ".jpg");
                            if (file.Exists)
                                file.Delete();
                            file = new FileInfo(WorkCurveHelper.SaveGraphicPath + "\\" + sSpecListId + ".jpg");
                            if (file.Exists)
                                file.Delete();
                        }
                       
                        WorkCurveHelper.DataAccess.DeleteRecord(sSpecListId);
                    }
                    //删除数据库中的谱文件
                    lstGetSpecList.Clear();
                    #region 备注

                    //                    List<SpecList> lstDel = new List<SpecList>();
//                    for (int i = count - 1; i >= 0; i--)
//                    {
//                        //修改：何晓明 20110701 谱名称3列显示
//                        //SpecList sl = lstSpecList[dgvwSample.SelectedRows[i].Index];
//                        SpecList sl = lstGetSpecList[i];
//                        //
//                        lstDel.Add(sl);
//                    }
//                    foreach (var specList in lstDel)
//                    {
//                        List<SpecList> tempMatchSpec = SpecList.FindBySql(@"select * from SpecList a inner join Condition b
//                                        on a.Condition_Id = b.Id inner join Device d on b.Device_Id=d.Id where a.Id=" + specList.Id + " and b.Device_Id=" + WorkCurveHelper.DeviceCurrent.Id + " and a.Id in (select MatchSpecListId from StandSample)");
//                        if (tempMatchSpec != null && tempMatchSpec.Count > 0)
//                        {
//                            Msg.Show(Info.NoDeleteSpec);
//                            return;
//                        }
//                        Spec.DeleteAll(s => s.SpecList.Id == specList.Id);

//                        //修改：何晓明 20110728 删除datatable中记录
//                        DataRow[] drs = dt_SpecList.Select("name = '" + specList.Name + "' and id = " + specList.Id);
//                        if (drs != null && drs.Length > 0)
//                        {
//                            dt_SpecList.Rows.Remove(drs[0]);
//                            dt_SpecList.AcceptChanges();
//                        }
//                        //lstSpecList.Remove(specList);


//                        //修改：何晓明 20110701 谱名称3列显示
//                        lstGetSpecList.Remove(specList);
//                        //
//                        lstOriginal = lstSpecList;
//                        if (WorkCurveHelper.VirtualSpecList != null && WorkCurveHelper.VirtualSpecList.Count > 0)
//                        {
//                            var tempList = WorkCurveHelper.VirtualSpecList.Find(s => s.Id == specList.Id);
//                            if (tempList != null) WorkCurveHelper.VirtualSpecList.Remove(tempList);
//                        }

//                        specList.Delete();
//                        DirectoryInfo info = new DirectoryInfo(WorkCurveHelper.SaveSamplePath);
//                        if (info.Exists && specList.ImageShow)
//                        {
//                            FileInfo file = new FileInfo(WorkCurveHelper.SaveSamplePath + "\\" + specList.Id + ".jpg");
//                            if (file.Exists)
//                                file.Delete();
//                        }
//                        info = new DirectoryInfo(WorkCurveHelper.SaveGraphicPath);
//                        if (info.Exists && specList.ImageShow)
//                        {
//                            FileInfo file = new FileInfo(WorkCurveHelper.SaveGraphicPath + "\\" + specList.Id + ".jpg");
//                            if (file.Exists)
//                                file.Delete();
//                        }
//                        SpecAdditional.DeleteAll(w => w.SpecListId == specList.Id);
                    //                    }

                    #endregion
                    int hScrollValue = HighPerformanceListView1.hScrollValue;
                    this.HighPerformanceListView1.DisplayStartIdx = DisplayStartIdx;
                    btnSearch_Click(null, null);
                    this.HighPerformanceListView1.hScrollValue = hScrollValue;

                }
            }
            else
            { SkyrayMsgBox.Show(Info.NoSelect); }
        }

        /// <summary>
        /// 选择谱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {            
            if (selectVisual)
            {
                SpecCurrentName.Clear();
                SelectedSpecList.Clear();
                for (int i = 0; i < lstGetSpecList.Count; i++)
                {
                    foreach (var c in WorkCurveHelper.DefaultVirtualColor)
                    {
                        if (!c.BeSelected)
                        {
                            lstGetSpecList[i].VirtualColor = c.Color.ToArgb();
                            c.BeSelected = true;
                            break;
                        }
                    }

                    SpecCurrentName.Add(lstGetSpecList[i].Name);
                    SelectedSpecList.Add(lstGetSpecList[i]);
                }
            }
            else
            {
                if (lstGetSpecList.Count == 0)
                {
                    SkyrayMsgBox.Show(Info.NoSelect);
                    return;
                }
                for (int i = 0; i < this.lstGetSpecList.Count; i++)
                {
                    SpecCurrentName.Add(lstGetSpecList[i].Name);
                    SelectedSpecList.Insert(0, lstGetSpecList[i]);
                }
                //specListCurrent = lstGetSpecList[0];
                //lstSpecList = lstGetSpecList;
                //

            }
            this.DialogResult = DialogResult.OK;
            if (this.ParentForm != null)
                this.ParentForm.DialogResult = this.dialogResult = DialogResult.OK;
            SaveSpecOption();
            EDXRFHelper.GotoMainPage(this);
        }

        /// <summary>
        /// 保存打开谱信息
        /// </summary>
        private void SaveSpecOption()
        {
            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedValue("OpenSelectSampleType", "dateTimePickerFrom", dateTimePickerFrom.Value.ToString());
            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedValue("OpenSelectSampleType", "dateTimePickerTo", dateTimePickerTo.Value.ToString());
        }

        /// <summary>
        /// 选择颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtColor_MouseClick(object sender, MouseEventArgs e)
        {
            if (specListCurrent == null) return;
            if (DialogResult.OK == colDlg.ShowDialog())
            {
                txtColor.BackColor = colDlg.Color;
                DrawSpec(txtColor.BackColor);
                xcSpec.Focus();
            }
        }

        /// <summary>
        /// 颜色改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtColor_BackColorChanged(object sender, EventArgs e)
        {
            if (!selectVisual)
            {
                specListCurrent.Color = txtColor.BackColor.ToArgb();
            }
            else
            {
                specListCurrent.VirtualColor = txtColor.BackColor.ToArgb();
            }
            //WorkCurveHelper.DataAccess.DeleteRecord(specListCurrent.Name);
            WorkCurveHelper.DataAccess.Save(specListCurrent, specListCurrentId);
        }

        #endregion

        public override void ExcuteEndProcess(params object[] objs)
        {
            if (!selectVisual && type == AddSpectrumType.OpenSpectrum && IsCaculate)//
                DifferenceDevice.MediumAccess.OpenWorkSpectrumSelect(lstGetSpecList);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //this.btnImport.Visible = true;
            //this.btnExport.Visible = true;
            //读取数据信息
            dt_SpecList = GetSpecList();

            //填充数据
            GetSpec();
        }

        private void btWRatioSubmit_Click(object sender, EventArgs e)
        {
            //btnSelect_Click(null, null);
            if ( lstGetSpecList != null &&  lstGetSpecList.Count>0)
                WorkCurveHelper.DemoSpecList = lstGetSpecList[0];
            EDXRFHelper.GotoMainPage(this);
        } 

        private void listViewWSpecList_DoubleClick()
        {
            btnSelect_Click(null, null);
        }

        Point p = new Point();
        private void listViewWSpecList_MouseDown(object sender, MouseEventArgs e)
        {
            this.p.X = e.X;
            this.p.Y = e.Y;
        }

        private void SelectSample_Load(object sender, EventArgs e)
        {
            if (WorkCurveHelper.DemoInstance)
                this.btWSelectDemo.Visible = true;
        }

        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedValue("OpenSelectSampleType", "IsSelectDate", this.chkDate.Checked.ToString());
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            DialogResult dr = SkyrayMsgBox.Show(Info.ConfirmDel, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dr != DialogResult.OK) return;
            string strName = string.Empty,
                strBegTime = string.Empty,
                strEnTime = string.Empty,
                strNameOr = string.Empty,
                strTimeOr = string.Empty; 
            if (chkDate.Checked || chkDate.Checked)//条件谱的清空
            {
                strName = chkName.Checked && txtKeywords.Text.Trim() != string.Empty ? txtKeywords.Text.Trim() : null;
                strBegTime = chkDate.Checked && dateTimePickerFrom.Value != null ? dateTimePickerFrom.Value.ToString("yyyy-MM-dd") : null;
                strEnTime = chkDate.Checked && dateTimePickerTo.Value != null && dateTimePickerTo.Value >= dateTimePickerFrom.Value ? dateTimePickerTo.Value.ToString("yyyy-MM-dd") : strBegTime;
                strNameOr = chkNameOrder.Checked && cobNameOrder.SelectedIndex != -1 ? (cobNameOrder.SelectedIndex == 0 ? "asc" : "desc") : null;
                strTimeOr = chkTimeOrder.Checked && cobTimeOrder.SelectedIndex != -1 ? (cobTimeOrder.SelectedIndex == 0 ? "asc" : "desc") : null;
                //lstGetSpecList = WorkCurveHelper.DataAccess.ResearchByConditions(strName, strBegTime, strEnTime, strNameOr, strTimeOr);
            }
            UCThinDatabase td = new UCThinDatabase(1);
            td.Execute(new Action(() =>
            {
                decimal tmpResult = ClearSpec(strName, strBegTime, strEnTime, strNameOr, strTimeOr, 1000, td.GetClearWorker(), td.SetCount);
                if (tmpResult == 0 || tmpResult == 1)
                {
                    UCThinDatabase.ShowText(td, Info.UnderReleaseSpace);
                    UCThinDatabase.ReleaseSpace();
                }
            }));
            WorkCurveHelper.OpenUC(td, false);
            
            int hScrollValue = HighPerformanceListView1.hScrollValue;
            this.HighPerformanceListView1.DisplayStartIdx = DisplayStartIdx;
            btnSearch_Click(null, null);
            this.HighPerformanceListView1.hScrollValue = hScrollValue;
        }

        

        public static decimal ClearSpec(string name, string beginTime, string endTime, string nameOrder, string timeOrder, int limit, BackgroundWorker bgWorker, Action<decimal, decimal> action)
        {
            string[] mathArr = GetMathArr();
            Decimal totalCount = WorkCurveHelper.DataAccess.HandleSpecListEntityByConditions(name, beginTime, endTime, nameOrder, timeOrder, null, null);
            Decimal num = 0;
            action(num, totalCount);
            int percent = 1;
            Func<SpecListEntity, int> func = new Func<SpecListEntity, int>(sp =>
            {
                if (sp.SpecType == SpecType.UnKownSpec || sp.SpecType == SpecType.UnSelected)
                {
                    if (!mathArr.Contains(sp.Name) && DifferenceDevice.interClassMain.selectSpeclist.Find(w => w.Name == sp.Name) == null)
                    {
                        if (WorkCurveHelper.VirtualSpecList != null && WorkCurveHelper.VirtualSpecList.Count > 0)
                        {
                            var tempList = WorkCurveHelper.VirtualSpecList.Find(s => s.Name == sp.Name);
                            if (tempList != null) WorkCurveHelper.VirtualSpecList.Remove(tempList);
                        }
                        FileInfo file = new FileInfo(WorkCurveHelper.SaveSamplePath + "\\" + sp.Name + ".jpg");
                        if (file.Exists)
                            file.Delete();
                        file = new FileInfo(WorkCurveHelper.SaveGraphicPath + "\\" + sp.Name + ".jpg");
                        if (file.Exists)
                            file.Delete();
                        WorkCurveHelper.DataAccess.DeleteRecord(sp.Name);
                    }
                }
                num++;
                action(num, totalCount);
                percent = (100 * num / totalCount > 1 ? (int)(100 * num / totalCount) : 1);
                bgWorker.ReportProgress(percent);
                if (bgWorker != null && bgWorker.CancellationPending)
                    return 0;
                return 1;
            });
            decimal result = WorkCurveHelper.DataAccess.HandleSpecListEntityByConditions(name, beginTime, endTime, nameOrder, timeOrder, limit, func);
            return result;
        }

        public static string[] GetMathArr()
        {
            string mathStr = string.Empty;
            string querySql = "Select* from ElementList a,WorkCurve b,Condition c,Device d Where a.WorkCurve_Id=b.Id And b.Condition_Id=c.Id And c.Device_Id=" + WorkCurveHelper.DeviceCurrent.Id;
            List<ElementList> listElementList = ElementList.FindBySql(querySql);
            foreach (var mathElementList in listElementList)
            {
                if (string.IsNullOrEmpty(mathElementList.MatchSpecListIdStr))
                    continue;
                string[] strs = mathElementList.MatchSpecListIdStr.Split(',');
                for (int i = 0; i < strs.Length; i++)
                    if (i % 2 == 0)
                        mathStr += strs[i] + ",";
            }
            string[] mathArr = mathStr.Split(',');
            return mathArr;
        }

        private void btnExportbyResearch_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == this.fbdImport.ShowDialog())
            {
                if (this.fbdImport.SelectedPath.IsNullOrEmpty())
                {
                    MessageBox.Show("选择路径！路径不能为空！");
                    return;
                }
                else
                {
                    string path = this.fbdImport.SelectedPath + @"\" + DateTime.Now.Date.ToString("yyyyMMddhhmmss") + @"\";
                    if (!Directory.Exists(path))
                    {
                        DirectoryInfo info = new DirectoryInfo(path);
                        info.Create();
                    }
                    //修改：何晓明 20110701 谱名称3列显示
                    //int count = dgvwSample.SelectedRows.Count;
                    string strName = chkName.Checked && txtKeywords.Text.Trim() != string.Empty ? txtKeywords.Text.Trim() : null;
                    string strBegTime = chkDate.Checked && dateTimePickerFrom.Value != null ? dateTimePickerFrom.Value.ToString("yyyy-MM-dd") : null;
                    string strEnTime = chkDate.Checked && dateTimePickerTo.Value != null && dateTimePickerTo.Value >= dateTimePickerFrom.Value ? dateTimePickerTo.Value.ToString("yyyy-MM-dd") : strBegTime;
                    string strNameOr = chkNameOrder.Checked && cobNameOrder.SelectedIndex != -1 ? (cobNameOrder.SelectedIndex == 0 ? "asc" : "desc") : null;
                    string strTimeOr = chkTimeOrder.Checked && cobTimeOrder.SelectedIndex != -1 ? (cobTimeOrder.SelectedIndex == 0 ? "asc" : "desc") : null;
                    lstGetSpecList = WorkCurveHelper.DataAccess.ResearchByConditions(strName, strBegTime, strEnTime, strNameOr, strTimeOr);
                    int count = lstGetSpecList.Count;
                    //i
                    if (count > 0)
                    {
                        if (WorkCurveHelper.SpecExportType == 1)
                        {
                            ExportSpecType2(count, path);
                        }
                        else if (WorkCurveHelper.SpecExportType == 3)
                        {
                            ExportSpecTypeMCA(count, path);
                        }
                        else
                        {
                            ExportSpecType1(count, path);
                        }
                        btnSearch_Click(null, null);
                        if (DialogResult.OK == SkyrayMsgBox.Show(Info.ExportSuccess, MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                        {
                            System.Diagnostics.Process.Start("iexplore.exe", path);
                        }
                    }
                    else
                    { SkyrayMsgBox.Show(Info.NoSelect); }
                }
            }
        }

        private void buttonW1_Click(object sender, EventArgs e)
        {
            SpecListEntity tempSpeclist = specListCurrent;
            tempSpeclist.SampleName = txtSampleName.Text;
            try
            {
                tempSpeclist.Weight = double.Parse(txtWeight.Text);
            }
            catch
            { }
            tempSpeclist.Shape = txtShape.Text;
            tempSpeclist.Operater = specListCurrent.Operater;
            tempSpeclist.SpecDate = txtSpecDate.Value;
            tempSpeclist.SpecSummary = txtSummary.Text;
            List<CompanyOthersInfo> companyOthersInfo_List = CompanyOthersInfo.FindBySql("select * from companyothersinfo where  Display =1 and ExcelModeType='" + ReportTemplateHelper.ExcelModeType.ToString() + "'");
            if (companyOthersInfo_List != null)
                tempSpeclist.CompanyInfoList = companyOthersInfo_List;
            if (WorkCurveHelper.DataAccess.Query(specListCurrent.Name) != null)
            {
                WorkCurveHelper.DataAccess.DeleteRecord(specListCurrent.Name);
                WorkCurveHelper.DataAccess.Save(tempSpeclist);
                btnSearch_Click(this,null);
            }
        }

        void txtWeight_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar > (char)47 && e.KeyChar < (char)58 || e.KeyChar == (char)8 || e.KeyChar == (char)46 || e.KeyChar == (char)3 || e.KeyChar == (char)22)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show(Info.IllegalInput);
            }
        }

        private void buttonWOtherInfoSet_Click(object sender, EventArgs e)
        {
            WorkCurveHelper.OpenUC(new UCOtherInfoSet(this.specListCurrent.CompanyInfoList), false);
        }

    }
}
