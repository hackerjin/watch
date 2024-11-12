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

namespace Skyray.UC
{
    public partial class UCCustomStandard : Skyray.Language.UCMultiple
    {

        #region Fields
        /// <summary>
        /// 标准集合
        /// </summary>
        private DbObjectList<CustomStandard> lstStandard;


        /// <summary>
        /// 临时存放标准集合
        /// </summary>
        private DbObjectList<CustomStandard> lstTempStandard;
        /// <summary>
        /// 当前标准
        /// </summary>
        private CustomStandard devStandard;


        /// <summary>
        /// 工作曲线
        /// </summary>
        private DbObjectList<WorkCurve> lstWorkCurve;


        //public event EventDelegate.SaveChangeStandand OnSaveChangeStandand;

        /// <summary>
        /// 选择元素
        /// </summary>
       // private string SelectElement;

        #endregion

        #region Init
        /// <summary>
        /// 构造
        /// </summary>
        public UCCustomStandard()
        {
            InitializeComponent();
             if (!Lephone.Data.DbEntry.Context.GetTableNames().Contains("StandardData"))
                 StandardData.FindAll();
        }

        
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomStandard_Load(object sender, EventArgs e)
        {

          
            if (WorkCurveHelper.iResultJudgingType != 2 && WorkCurveHelper.iResultJudgingType != 4)
            {
                dgvwStandardDatas.Columns["StartStandardContent"].Visible = false;
            }
            if (WorkCurveHelper.isShowXRFStandard == 1)
            {
                dgvwStandardDatas.Columns["StartStandardContent"].Visible = true;
            }

            if (WorkCurveHelper.isThickLimit == 1)
            {
                dgvwStandardDatas.Columns["StartStandardContent"].Visible = false;
                dgvwStandardDatas.Columns["StandardContent"].Visible = false;
                dgvwStandardDatas.Columns["StandardThick"].Visible = true;
                dgvwStandardDatas.Columns["StandardThickMax"].Visible = true;

            }

            if (!WorkCurveHelper.IsResultEdit && GP.CurrentUser.Role.RoleType != 0)
            {

                dgvwStandardDatas.Columns["StartStandardContent"].ReadOnly = true;
                dgvwStandardDatas.Columns["StandardContent"].ReadOnly = true;
                btnAdd.Visible = false;
                txtNewStandard.Visible = false;
                btnDel.Visible = false;
                btnSelectElement.Visible = false;
                btnDelData.Visible = false;
                btnApplication.Visible = false;
                checkTotalValue.Enabled = false;
                txbTotalContext.Enabled = false;
            }

            lstStandard = CustomStandard.FindAll();//获取所有标准
            lstTempStandard = CustomStandard.FindAll();//获取临时所有标准
            colStandardName.Items.Clear();
            colStandardName.Items.Add("");
            foreach (var Standard in lstStandard)
            {
                lbwStandardList.Items.Add(Standard.StandardName);
                colStandardName.Items.Add(Standard.StandardName);
            }
            LoadPosition();

            if (lstStandard.Count > 0)
            {
                devStandard = WorkCurveHelper.CurrentStandard;
                if (devStandard != null) lbwStandardList.SelectedItem = devStandard.StandardName;

            }
            string IsTotalAnalysis = ReportTemplateHelper.LoadSpecifiedValue("TestParams", "IsTotalAnalysis");
            if (!string.IsNullOrEmpty(IsTotalAnalysis) && int.Parse(IsTotalAnalysis) == 0)
            {
                this.checkTotalValue.Visible = false;
                this.txbTotalContext.Visible = false;
            }
            else if (!string.IsNullOrEmpty(IsTotalAnalysis) && int.Parse(IsTotalAnalysis) == 1)
            {
                this.checkTotalValue.Visible = true;
                this.txbTotalContext.Visible = true;
            }

          

        }

        #endregion

        #region Method


        private void LoadPosition()
        {
            if (!DifferenceDevice.IsThick)
            {
                gpCurve.Visible = false;
                gpStand.Location = new Point(8, 8);
            }
            else
            {
                //加载所有曲线的标准
                LoadWorkCurveStandard();
            }
        }


        private void LoadWorkCurveStandard()
        {
           // colStandardName.Items.Clear();
            if (WorkCurveHelper.DeviceCurrent != null)
            {
                var deviceId = WorkCurveHelper.DeviceCurrent.Id;
                string sql = "select * from WorkCurve where Condition_Id in (select Id from condition where Type = 0 and Device_id = "
                    + deviceId + ") and FuncType=" + (int)WorkCurveHelper.DeviceFunctype + " order by LOWER(Name)";

                lstWorkCurve = WorkCurve.FindBySql(sql);
            }
            else
            {
                lstWorkCurve = WorkCurve.FindAll();
            }
            

            BindingSource bs = new BindingSource();
            for (int i = 0; i < lstWorkCurve.Count; i++)
            {
                if (lstStandard.Find(w => w.StandardName.Equals(lstWorkCurve[i].ThickStandardName)) == null)
                    lstWorkCurve[i].ThickStandardName = "";
                bs.Add(lstWorkCurve[i]);
            }
            this.dgvCurveStandardName.AutoGenerateColumns = false;
            this.dgvCurveStandardName.DataSource = bs;
        }
        /// <summary>
        /// 加载标准列表
        /// </summary>
        private void GetGrid()
        {
            BindingSource bs = new BindingSource();
            for (int i = 0; i < devStandard.StandardDatas.Count; i++)
            {
                bs.Add(devStandard.StandardDatas[i]);
            }
            this.dgvwStandardDatas.AutoGenerateColumns = false;
            this.dgvwStandardDatas.DataSource = bs;//绑定数据源  
            int count = this.dgvwStandardDatas.Rows.Count;
            if (count > 1)
            {
                dgvwStandardDatas.Rows[0].Selected = false;
                dgvwStandardDatas.Rows[count - 1].Selected = true;
            }
            if (devStandard.IsSelectTotal)
            {
                this.txbTotalContext.Text = devStandard.TotalContentStandard.ToString();
            }
            else this.txbTotalContext.Text = "";
            this.checkTotalValue.Checked = devStandard.IsSelectTotal;
        }

        #endregion

        #region Events
        /// <summary>
        /// 选择标准
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbwStandardList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbwStandardList.SelectedIndex > -1)
            {
                devStandard = lstStandard[lbwStandardList.SelectedIndex];
                GetGrid();//刷新标准列表
            }
            else
            {
                devStandard = null;
            }
        }
        /// <summary>
        /// 添加新标准
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!txtNewStandard.Text.Equals(""))
            {
                if (Atoms.AtomList.Find(w => w.AtomName.Equals(txtNewStandard.Text)) != null)
                {
                    SkyrayMsgBox.Show(Info.IllegalInput);
                    return;
                }

                CustomStandard cs = lstStandard.Find(S => S.StandardName == txtNewStandard.Text);//查找同名标准
                if (cs == null)
                {
                    
                    var Standard = CustomStandard.New.Init(txtNewStandard.Text);
                    if (this.checkTotalValue.Checked && !this.txbTotalContext.Text.IsNullOrEmpty())
                    {
                        Standard.TotalContentStandard = double.Parse(this.txbTotalContext.Text);
                        Standard.IsSelectTotal = true;
                    }
                    else
                        Standard.IsSelectTotal = false;

                    Standard.Save();//保存新标准
                    lstStandard.Add(Standard);//添加到缓存
                    colStandardName.Items.Add(Standard.StandardName);
                    lbwStandardList.Items.Add(txtNewStandard.Text);//添加到列表
                    lbwStandardList.SelectedItem = txtNewStandard.Text;
                    txtNewStandard.Text = "";
                    devStandard = Standard;
                }
                else
                {
                    SkyrayMsgBox.Show(Info.ExistName);
                }
            }
            else
            {
                SkyrayMsgBox.Show(Info.IsNull);
            }
        }
        /// <summary>
        /// 删除标准
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            int index = lbwStandardList.SelectedIndex;
            if (index >= 0)
            {
                DialogResult dr = SkyrayMsgBox.Show(Info.ConfirmDel, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.OK)
                {
                    if (lstWorkCurve!=null)
                    foreach (var curve in lstWorkCurve)
                    {
                        if (curve.ThickStandardName == null) continue;
                        if (curve.ThickStandardName.Equals(lstStandard[index].StandardName))
                        {
                            curve.ThickStandardName = "";
                        }
                    }

                    CustomStandard cs = lstStandard[index];

                    if (WorkCurveHelper.CurrentStandard != null && cs.Id == WorkCurveHelper.CurrentStandard.Id)
                    {
                        WorkCurveHelper.CurrentStandard = null;
                    }
                    //cs.StandardDatas.Clear();
                    //cs.Save();
                    cs.Delete();
                    //thick
                    colStandardName.Items.RemoveAt(index);
                    //lstStandard[index].Delete();//删除数据
                    lbwStandardList.Items.RemoveAt(index);//删除缓存
                    dgvwStandardDatas.Rows.Clear();
                    if (index > 0 && lbwStandardList.Items.Count > 0)
                    {
                        lbwStandardList.SelectedIndex = index - 1;
                        devStandard = lstStandard[index - 1];
                    }
                    else if (index == 0 && lbwStandardList.Items.Count > 0)
                    {
                        lbwStandardList.SelectedIndex = 0;
                        devStandard = lstStandard[0];
                    }
                }
            }
            else
            {
                SkyrayMsgBox.Show(Info.NoSelect);
            }
        }

        private void SaveStandard()
        {
            //thick  加载当前曲线的标准
            if (DifferenceDevice.IsThick)
            {

                if (devStandard != null)
                    devStandard.IsSelectTotal = this.checkTotalValue.Checked;
                //ValidateHelper.IllegalCheck(txbTotalContext)
                if (!this.txbTotalContext.Text.IsNullOrEmpty())
                    devStandard.TotalContentStandard = double.Parse(this.txbTotalContext.Text);
                if (devStandard != null)
                {
                    devStandard.Save();
                }

                for (int i = 0; i < lstWorkCurve.Count; i++)
                {
                    lstWorkCurve[i].Save();
                }

                WorkCurve curCurve = lstWorkCurve.Find(S => S.Name == WorkCurveHelper.WorkCurveCurrent.Name);
                if (curCurve != null)
                    WorkCurveHelper.CurrentStandard = lstStandard.Find(S => S.StandardName == curCurve.ThickStandardName);
                else
                    WorkCurveHelper.CurrentStandard = null;
            }
            else
            {
                if (WorkCurveHelper.CurrentStandard != null && WorkCurveHelper.CurrentStandard.Id != devStandard.Id)
                {
                    WorkCurveHelper.CurrentStandard.CurrentSatadard = false;
                    WorkCurveHelper.CurrentStandard.Save();
                }
                else if (WorkCurveHelper.CurrentStandard != null && WorkCurveHelper.CurrentStandard.Id == devStandard.Id)
                {
                    WorkCurveHelper.CurrentStandard = devStandard;
                }

                if (WorkCurveHelper.CurrentStandard == null || WorkCurveHelper.CurrentStandard.Id != devStandard.Id)
                {
                    WorkCurveHelper.CurrentStandard = devStandard;

                }

                if (devStandard != null)
                    devStandard.IsSelectTotal = this.checkTotalValue.Checked;
                //ValidateHelper.IllegalCheck(txbTotalContext)
                if (!this.txbTotalContext.Text.IsNullOrEmpty())
                    devStandard.TotalContentStandard = double.Parse(this.txbTotalContext.Text);
                if (devStandard != null)
                {
                    devStandard.CurrentSatadard = true;
                    devStandard.Save();
                }
            }
           


        }

        /// <summary>
        /// 保存标准
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            //if (devStandard == null)
            //{
            //    EDXRFHelper.GotoMainPage(this);//返回主界面
            //    return;
            //}
            SaveStandard();
            
            if (this.ParentForm != null)
                this.ParentForm.DialogResult = this.dialogResult = DialogResult.OK;
            EDXRFHelper.GotoMainPage(this);//返回主界面
        }
        
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (DifferenceDevice.IsThick)
                SaveStandard();
            EDXRFHelper.GotoMainPage(this);//返回主界面
        }

        /// <summary>
        /// 数据验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void dgvwStandardDatas_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (String.IsNullOrEmpty(e.FormattedValue.ToString()))
            {
                dgvwStandardDatas[e.ColumnIndex, e.RowIndex].Value = 0;
                e.Cancel = true;
                return;
            }
            if (dgvwStandardDatas.Columns[e.ColumnIndex].Name.Equals("StandardContent")
                || dgvwStandardDatas.Columns[e.ColumnIndex].Name.Equals("StandardThick")
                || dgvwStandardDatas.Columns[e.ColumnIndex].Name.Equals("StartStandardContent")
                 || dgvwStandardDatas.Columns[e.ColumnIndex].Name.Equals("StandardThickMax"))
            {
                var max = Ranges.RangeDictionary[dgvwStandardDatas.Columns[e.ColumnIndex].Name].Max;
                var min = Ranges.RangeDictionary[dgvwStandardDatas.Columns[e.ColumnIndex].Name].Min;
                try
                {
                    double.Parse(e.FormattedValue.ToString());
                    //int.Parse(e.FormattedValue.ToString());
                }
                catch (Exception)
                {
                    //SkyrayMsgBox.Show(Info.StyleError);
                    dgvwStandardDatas[e.ColumnIndex, e.RowIndex].Value = min;
                    e.Cancel = true;
                    return;
                }
                if (decimal.Parse(e.FormattedValue.ToString()) > max)
                {
                    dgvwStandardDatas[e.ColumnIndex, e.RowIndex].Value = max;
                    e.Cancel = true;
                }
                else if (decimal.Parse(e.FormattedValue.ToString()) < min)
                {
                    dgvwStandardDatas[e.ColumnIndex, e.RowIndex].Value = min;
                    e.Cancel = true;
                }
                return;
            }
        }
        /// <summary>
        /// 选择元素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectElement_Click(object sender, EventArgs e)
        {
            if (devStandard == null) return;

            ElementTableAtom table = new ElementTableAtom();

            string[] strs = new string[dgvwStandardDatas.Rows.Count];
            for (int i = 0; i < dgvwStandardDatas.Rows.Count; i++)
            {
                strs[i] = dgvwStandardDatas.Rows[i].Cells[0].Value.ToString();
            }

            table.MultiSelect = true;
            table.SelectedItems = strs;

            WorkCurveHelper.OpenUC(table, false, Info.SelectElement,true);//打开元素周期表


            if (table.SelectedItems != null && table.SelectedItems.Length > 0)
            {
                var atomnames = table.SelectedItems;
                bool hasElement = false;
                for (int i = 0; i < atomnames.Length; i++)//遍历新数组
                {
                    hasElement = false;

                    for (int j = 0; j < devStandard.StandardDatas.Count; j++)
                    {
                        if (devStandard.StandardDatas[j].ElementCaption.Equals(atomnames[i]))
                        {
                            hasElement = true;
                            break;
                        }
                    }
                    if (!hasElement)
                    {
                        devStandard.StandardDatas.Add(StandardData.New.Init(atomnames[i], 0.00, 0.00,0.00));
                    }
                }
                for (int i = devStandard.StandardDatas.Count - 1; i >= 0; i--)//遍历增加后的元素列表
                {
                    hasElement = false;
                    for (int j = 0; j < atomnames.Length; j++)
                    {
                        if (atomnames[j].Equals(devStandard.StandardDatas[i].ElementCaption))
                        {
                            hasElement = true;
                            break;
                        }
                    }
                    if (!hasElement)
                    {
                        devStandard.StandardDatas.RemoveAt(i);
                    }
                }
            }
            else
            {
                devStandard.StandardDatas.Clear();
            }

            GetGrid();

            //if (table.DialogResult == DialogResult.OK)
            //{
            //    SelectElement = table.SelectedItems == null ? null : table.SelectedItems[0];//元素
            //    if (devStandard.StandardDatas.ToList().Find(sd => sd.ElementCaption == SelectElement) != null)
            //    {
            //        Msg.Show(Info.CurrentStandardExistElement);
            //        this.txtElementSelected.Text = "";
            //    }
            //    else
            //    {
            //        this.txtElementSelected.Text = SelectElement;
            //    }
            //}
        }
        ///// <summary>
        ///// 添加数据
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void btnAddData_Click(object sender, EventArgs e)
        //{
        //    StandardData sd;
        //    if (devStandard == null)
        //    {
        //        SkyrayMsgBox.Show(Info.SelectSampleFirst);
        //        return;
        //    }
        //    if (txtElementSelected.Text.Equals(""))
        //    {
        //        SkyrayMsgBox.Show(Info.SelectElement);
        //        return;
        //    }
        //    if (numStandardContent.Value > Ranges.RangeDictionary["StandardContent"].Max || numStandardContent.Value < Ranges.RangeDictionary["StandardContent"].Min)
        //    {
        //        SkyrayMsgBox.Show(Info.OutOfRange);
        //        return;
        //    }
        //    sd = StandardData.New.Init(txtElementSelected.Text, double.Parse(numStandardContent.Value.ToString()));
        //    devStandard.StandardDatas.Add(sd);//添加到缓存
        //    GetGrid();//刷新标准列表
        //    this.txtElementSelected.Text = "";
        //}
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelData_Click(object sender, EventArgs e)
        {
            int count = dgvwStandardDatas.SelectedRows.Count;
            if (count <= 0)
            {
                SkyrayMsgBox.Show(Info.NoSelect);
                return;
            }
            for (int i = dgvwStandardDatas.SelectedRows.Count - 1; i >= 0; i--)
            {
                int index = dgvwStandardDatas.SelectedRows[i].Index;
                // StandardData sd = devStandard.StandardDatas[index];
                // sd.Delete();
                //devStandard.StandardDatas[index].Delete();//删除数据
                //devStandard.StandardDatas.Remove(sd);//删除缓存
                devStandard.StandardDatas.RemoveAt(index);
            }
            GetGrid();//刷新标准列表
        }

        #endregion

        public override void ExcuteEndProcess(params object[] objs)
        {
            //if (WorkCurveHelper.CurrentStandard != null && WorkCurveHelper.CurrentStandard.Id != devStandard.Id)
            //{
                DifferenceDevice.MediumAccess.SaveChangeStandand();
            //}
        }

        private void btnApplication_Click(object sender, EventArgs e)
        {
            SaveStandard();
        }

        private void checkTotalValue_CheckedChanged(object sender, EventArgs e)
        {
            devStandard.IsSelectTotal = this.checkTotalValue.Checked;
        }

        private void txbTotalContext_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double.Parse(this.txbTotalContext.Text);
            }
            catch 
            {
                //Msg.Show(ec.Message);
                this.txbTotalContext.Text = "";
            }
        }

        private void dgvwStandardDatas_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1 || (WorkCurveHelper.iResultJudgingType != 2 && WorkCurveHelper.iResultJudgingType != 4 && WorkCurveHelper.isThickLimit == 0)) return;

            if (dgvwStandardDatas.Columns[e.ColumnIndex].Name.Equals("StandardContent")
                || dgvwStandardDatas.Columns[e.ColumnIndex].Name.Equals("StartStandardContent"))
            { 
                var max = Ranges.RangeDictionary[dgvwStandardDatas.Columns[e.ColumnIndex].Name].Max;
                var min = Ranges.RangeDictionary[dgvwStandardDatas.Columns[e.ColumnIndex].Name].Min;

                if (double.Parse(dgvwStandardDatas.Rows[e.RowIndex].Cells["StartStandardContent"].Value.ToString()) > double.Parse(dgvwStandardDatas.Rows[e.RowIndex].Cells["StandardContent"].Value.ToString())
                    && dgvwStandardDatas.Columns[e.ColumnIndex].Name.Equals("StartStandardContent"))
                    dgvwStandardDatas.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = min;
                else if (double.Parse(dgvwStandardDatas.Rows[e.RowIndex].Cells["StartStandardContent"].Value.ToString()) > double.Parse(dgvwStandardDatas.Rows[e.RowIndex].Cells["StandardContent"].Value.ToString())
                    && dgvwStandardDatas.Columns[e.ColumnIndex].Name.Equals("StandardContent"))
                    dgvwStandardDatas.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = max;
                    
            }

            if (dgvwStandardDatas.Columns[e.ColumnIndex].Name.Equals("StandardThick")
                || dgvwStandardDatas.Columns[e.ColumnIndex].Name.Equals("StandardThickMax"))
            {
                var max = Ranges.RangeDictionary[dgvwStandardDatas.Columns[e.ColumnIndex].Name].Max;
                var min = Ranges.RangeDictionary[dgvwStandardDatas.Columns[e.ColumnIndex].Name].Min;

                if (double.Parse(dgvwStandardDatas.Rows[e.RowIndex].Cells["StandardThick"].Value.ToString()) > double.Parse(dgvwStandardDatas.Rows[e.RowIndex].Cells["StandardThickMax"].Value.ToString())
                    && dgvwStandardDatas.Columns[e.ColumnIndex].Name.Equals("StandardThick"))
                    dgvwStandardDatas.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = min;
                else if (double.Parse(dgvwStandardDatas.Rows[e.RowIndex].Cells["StandardThick"].Value.ToString()) > double.Parse(dgvwStandardDatas.Rows[e.RowIndex].Cells["StandardThickMax"].Value.ToString())
                    && dgvwStandardDatas.Columns[e.ColumnIndex].Name.Equals("StandardThickMax"))
                    dgvwStandardDatas.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = max;

            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {

        }

        private void btnAddCustum_Click(object sender, EventArgs e)
        {
            FrmDeviceNewName frm = new FrmDeviceNewName("", Info.CustomFiled, Info.CustomFiled, "");
            frm.ShowDialog();
            if (frm.DialogResult == DialogResult.OK)
            {
                string customName = frm.newDeviceName.Trim();//获取新设备名称
                bool hasElement = false;
                for (int j = 0; j < devStandard.StandardDatas.Count; j++)
                {
                    if (devStandard.StandardDatas[j].ElementCaption.Equals(customName))
                    {
                        hasElement = true;
                        break;
                    }
                }
                if (!hasElement)
                {
                    devStandard.StandardDatas.Add(StandardData.New.Init(customName, 0.00, 0.00, 0.00));
                }
                GetGrid();//刷新标准列表
            }
        }

     
    }
}
