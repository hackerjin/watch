using System;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;
using Skyray.Controls;
using Lephone.Data.Common;
using System.Linq;
using System.Collections.Generic;

namespace Skyray.UC
{
    /// <summary>
    /// 数据优化类
    /// </summary>
    public partial class UCOptimization : Skyray.Language.UCMultiple
    {

        #region Fields

        /// <summary>
        /// 当前工作曲线
        /// </summary>
        private WorkCurve workCurveCurrent;

        /// <summary>
        /// 当前选择INDEX
        /// </summary>
        private int SelectIndex = 0;

        /// <summary>
        /// 元素集合
        /// </summary>
        private DbObjectList<CurveElement> lstElement;

        #endregion

        #region Init

        public UCOptimization()
        {
            InitializeComponent();
        }

        private void UCOptimization_Load(object sender, EventArgs e)
        {
            if (WorkCurveHelper.WorkCurveCurrent != null)
                workCurveCurrent = WorkCurve.FindById(WorkCurveHelper.WorkCurveCurrent.Id);
            if (workCurveCurrent != null)
            {
                //workCurveCurrent.ElementList.Items
                lstElement = CurveElement.Find(c => c.ElementList.Id == workCurveCurrent.ElementList.Id);
                lbwOptimization.Items.Clear();
                foreach (var sp in lstElement)
                {
                    lbwOptimization.Items.Add(sp.Caption);
                }
                string[] strOptimizations = { Info.DataOptimization1, Info.DataOptimization2, Info.DataOptimization3, Info.DataOptimization4 };
                string[] strThickOptimizations = { Info.DataOptimization1, Info.DataOptimization2, Info.DataOptimization3, Info.DataOptimization4,Info.DataOptimization5 };

                if (workCurveCurrent.FuncType == FuncType.Thick)
                    comBoxOptimizationType.Items.AddRange(strThickOptimizations);
                else
                    comBoxOptimizationType.Items.AddRange(strOptimizations);

                this.comBoxOptimizationType.SelectedIndex = 0;
                if (workCurveCurrent.FuncType == FuncType.Rohs )
                    this.comBoxOptimizationType.Visible = false;
            }
        }

        public override void PageLoad(object sender, EventArgs e) 
        {
            base.PageLoad(sender, e);
            if (lstElement.Count > 0)
            {
                this.lbwOptimization.SelectedIndex = 0;
            }
        }

        #endregion

        #region Method

        /// <summary>
        /// 绑定列表
        /// </summary>
        private void OptimizationToGrid()
        { 
            dgvwOptimization.Rows.Clear();
            dgvwOptimization.DataSource = null;
         

            BindingSource bs = new BindingSource();
            for (int i = 0; i < lstElement[SelectIndex].Optimiztion.Count; i++)
            {
               if (lstElement[SelectIndex].Optimiztion[i].OptimizetionType == this.comBoxOptimizationType.SelectedIndex)
                bs.Add(lstElement[SelectIndex].Optimiztion[i]);
            }

            dgvwOptimization.AutoGenerateColumns = false;
            dgvwOptimization.DataSource = bs;//绑定数据源

        }

        #endregion

        #region Event

        /// <summary>
        /// 选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbwOptimization_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.lbwOptimization.SelectedIndex;
            SelectIndex = index;
            string unit = string.Empty;
            if (DifferenceDevice.IsThick)
            {
                //unit = lstElement[SelectIndex].ThicknessUnit == ThicknessUnit.um ? "(um)" :"(u〞)" ;
                if (lstElement[SelectIndex].ThicknessUnit == ThicknessUnit.um)
                {
                    unit = "(um)";
                }
                else if (lstElement[SelectIndex].ThicknessUnit == ThicknessUnit.um)
                {
                    unit = "(u〞)";
                }
                else
                {
                    unit = "(g/L)";
                }
                if (comBoxOptimizationType.SelectedIndex == 3)
                    unit = string.Empty;
            }
            else
            {
                if (lstElement[SelectIndex].ContentUnit == ContentUnit.per)
                {
                    unit = "(%)";
                }
                else if (lstElement[SelectIndex].ContentUnit == ContentUnit.ppm)
                {
                    unit = "(ppm)";
                }
                else
                {
                    unit = "(‰)";
                }
            }
                ////if (lstElement[SelectIndex].ContentUnit == ContentUnit.per)
                ////{
                ////    unit = "(%)";
                ////}
                ////else if (lstElement[SelectIndex].ContentUnit == ContentUnit.ppm)
                ////{
                ////    unit = "(ppm)";
                ////}
                ////else
                ////{
                ////    unit = "(‰)";
                ////}
            OptimiztionValue.HeaderText = Info.OptimizationValue + unit;
            OptimiztionMax.HeaderText = Info.RangePlus + unit;
            OptimiztionMin .HeaderText = Info.RangeSub + unit;
            OptExpression.HeaderText = Info.Expression;
            OptimizationToGrid();//刷新列表
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Optimiztion op = Optimiztion.New.Init(0, 0, 0, 0, 0, lstElement[SelectIndex].Caption, false);
            op.OptimizetionType = this.comBoxOptimizationType.SelectedIndex;
            lstElement[SelectIndex].Optimiztion.Add(op);
            OptimizationToGrid();//刷新列表
            int count = lstElement[SelectIndex].Optimiztion.Count;
            if (count > 0)
            {
                var temp = from test in lstElement[SelectIndex].Optimiztion where test.OptimizetionType == this.comBoxOptimizationType.SelectedIndex select test;
                count = temp.Count();
                dgvwOptimization.Rows[0].Selected = false;
                dgvwOptimization.Rows[count - 1].Selected = true;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dgvwOptimization.SelectedRows.Count <= 0)
            {
                SkyrayMsgBox.Show(Info.NoSelect);
                return;
            }
            int index = dgvwOptimization.SelectedRows[0].Index;
            //Optimiztion op = lstElement[SelectIndex].Optimiztion[index];
           // lstElement[SelectIndex].Optimiztion.RemoveAt(index);
            Optimiztion delopt = lstElement[SelectIndex].Optimiztion.ToList().Find(w=>w.OptimizetionType == comBoxOptimizationType.SelectedIndex);
            if (delopt != null)
            {
                lstElement[SelectIndex].Optimiztion.Remove(delopt);
            }
           // lstElement[SelectIndex].Optimiztion.ToList().Find(w=>w.OptimizetionType == comBoxOptimizationType.SelectedIndex).
            OptimizationToGrid();//刷新列表
            int count = lstElement[SelectIndex].Optimiztion.Count;
            if (count > 1)
            {
                var temp = from test in lstElement[SelectIndex].Optimiztion where test.OptimizetionType == this.comBoxOptimizationType.SelectedIndex select test;
                count = temp.Count();
                if (index >= count)
                {
                    this.dgvwOptimization.Rows[0].Selected = false;
                    this.dgvwOptimization.Rows[count - 1].Selected = true;
                }
                else
                {
                    this.dgvwOptimization.Rows[0].Selected = false;
                    this.dgvwOptimization.Rows[index].Selected = true;
                }
            }
            
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (var ele in lstElement)
            {
                ele.Save();
            }
            
            WorkCurveHelper.WorkCurveCurrent = WorkCurve.FindById(WorkCurveHelper.WorkCurveCurrent.Id);
            if (this.ParentForm != null)
                this.ParentForm.DialogResult = this.dialogResult = DialogResult.OK;
            EDXRFHelper.GotoMainPage(this);//转到主界面
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }

        /// <summary>
        /// 数据验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvwOptimization_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgvwOptimization.Columns[e.ColumnIndex].Name.Equals("OptimiztionValue") || dgvwOptimization.Columns[e.ColumnIndex].Name.Equals("OptimiztionMax") ||
                dgvwOptimization.Columns[e.ColumnIndex].Name.Equals("OptimiztionMin") || dgvwOptimization.Columns[e.ColumnIndex].Name.Equals("OptimiztionRange"))// ||
                //dgvwOptimization.Columns[e.ColumnIndex].Name.Equals("OptimiztionFactor"))
            {
                decimal Max = Ranges.RangeDictionary[dgvwOptimization.Columns[e.ColumnIndex].Name].Max;
                decimal Min = Ranges.RangeDictionary[dgvwOptimization.Columns[e.ColumnIndex].Name].Min;
                if (dgvwOptimization.Columns[e.ColumnIndex].Name.Equals("OptimiztionValue")||dgvwOptimization.Columns[e.ColumnIndex].Name.Equals("OptimiztionMax") ||
                dgvwOptimization.Columns[e.ColumnIndex].Name.Equals("OptimiztionMin"))
                {
                    if (workCurveCurrent.FuncType == FuncType.Thick)
                    {
                        Max = Max * 10000;
                    }
                    else
                    {
                        if (lstElement[SelectIndex].ContentUnit == ContentUnit.ppm)
                        {
                            Max = Max * 10000;
                        }
                        else if (lstElement[SelectIndex].ContentUnit== ContentUnit.permillage)
                        {
                            Max = Max * 10;
                        }
                        //Max = lstElement[SelectIndex].ContentUnit == ContentUnit.per ? Max : Max * 10000;
                    }
                }
                //if (this.comBoxOptimizationType.SelectedIndex == 1)
                //{
                //    Min = Max * (-100);
                //    Max = Max * 100;
                //}
                //try { double.Parse(e.FormattedValue.ToString()); }
                try { Convert.ToDecimal(e.FormattedValue.ToString()); }
                catch (Exception)
                {
                    //SkyrayMsgBox.Show(Info.StyleError);
                    dgvwOptimization.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Min;
                    e.Cancel = true;
                    return;
                }
                if (Convert.ToDecimal(e.FormattedValue.ToString()) > Max)
                {
                    //SkyrayMsgBox.Show(Info.OutOfRange);
                    dgvwOptimization.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Max;
                    e.Cancel = true;
                    return;
                }
                if (Convert.ToDecimal(e.FormattedValue.ToString()) < Min)
                {
                    dgvwOptimization.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Min;
                    e.Cancel = true;
                }
                return;
            }
        }

        /// <summary>
        /// 数据出错
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvwOptimization_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            SkyrayMsgBox.Show(Info.StyleError);
        }

        #endregion

        private void btnApplication_Click(object sender, EventArgs e)
        {
            foreach (var ele in lstElement)
            {
                ele.Save();
            }

            WorkCurveHelper.WorkCurveCurrent = WorkCurve.FindById(WorkCurveHelper.WorkCurveCurrent.Id);
        }

        private void btWOptimizeTo_Click(object sender, EventArgs e)
        {
            btnSave_Click(null, null);
            flag = true;
        }
        private bool flag = false;
        private bool upflag = false;
        public override void ExcuteEndProcess(params object[] objs)
        {
            if (flag)
            {
                ToolStripControls test = MenuLoadHelper.MenuStripCollection.Find(w => w.CurrentNaviItem.Name == "EditData");
                EDXRFHelper.RecurveNextUC(test);
            }

            if (upflag)
            {
                ToolStripControls test = MenuLoadHelper.MenuStripCollection.Find(w => w.CurrentNaviItem.Name == "ElementRef");
                EDXRFHelper.RecurseUpUC(test);
            }
        }

        private void btWUp_Click(object sender, EventArgs e)
        {
            btnSave_Click(null, null);
            upflag = true;
        }

        private void comBoxOptimizationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lbwOptimization.SelectedIndex < 0)
                return;
            switch (workCurveCurrent.FuncType)
            { 
                case FuncType.Thick:
                    SelectOptionShowThick();
                    break;
                default:
                    SelectOptionShow();
                    break;
            }

           
            lbwOptimization_SelectedIndexChanged(null,null);
        }

        private void SelectOptionShow()
        {
            if (this.comBoxOptimizationType.SelectedIndex == 1)
            {
                this.OptimiztionFactor.Visible = false;
                this.OptimiztionMax.Visible = true;
                this.OptimiztionMin.Visible = true;
                this.OptimiztionValue.Visible = true;
            }
            else if (this.comBoxOptimizationType.SelectedIndex == 3)
            {
                this.OptimiztionFactor.Visible = true;
                this.OptimiztionMax.Visible = false;
                this.OptimiztionMin.Visible = false;
                this.OptimiztionValue.Visible = false;

            }
            else
            {
                this.OptimiztionFactor.Visible = true;
                this.OptimiztionMax.Visible = true;
                this.OptimiztionMin.Visible = true;
                this.OptimiztionValue.Visible = true;
            }
        }


        private void SelectOptionShowThick()
        {
            if (this.comBoxOptimizationType.SelectedIndex == 0 )
            {
                this.OptimiztionFactor.Visible = true;
                this.OptimiztionMax.Visible = true;
                this.OptimiztionMin.Visible = true;
                this.OptimiztionValue.Visible = true;
                this.OptExpression.Visible = false;
                this.IsJoinIntensity.Visible = false;
            }
            else if (this.comBoxOptimizationType.SelectedIndex == 1)
            {
                this.OptimiztionFactor.Visible = true;
                this.OptimiztionMax.Visible = false;
                this.OptimiztionMin.Visible = false;
                this.OptimiztionValue.Visible = false;
                this.OptExpression.Visible = false;
                this.IsJoinIntensity.Visible = false;
            }
            else if (this.comBoxOptimizationType.SelectedIndex == 3)
            {
                this.OptimiztionFactor.Visible = false;
                this.OptimiztionMax.Visible = true;
                this.OptimiztionMin.Visible = true;
                this.OptimiztionValue.Visible = false;
                this.OptExpression.Visible = true;
                this.IsJoinIntensity.Visible = true;
            }
            else if (this.comBoxOptimizationType.SelectedIndex == 4)
            {
                this.OptimiztionFactor.Visible = true;
                this.OptimiztionMax.Visible = false;
                this.OptimiztionMin.Visible = false;
                this.OptimiztionValue.Visible = false;
                this.OptExpression.Visible = false;
                this.IsJoinIntensity.Visible = false;
            }
            else
            {
                this.OptimiztionFactor.Visible = false;
                this.OptimiztionMax.Visible = false;
                this.OptimiztionMin.Visible = false;
                this.OptimiztionValue.Visible = false;
                this.OptExpression.Visible = true;
                this.IsJoinIntensity.Visible = false;
            }
            
            
        }

    }
}
