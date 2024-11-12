using System;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;
using Skyray.Controls;
using System.Linq;
using Skyray.Language;

namespace Skyray.UC
{
    /// <summary>
    /// 编辑受影响元素类
    /// </summary>
    public partial class UCRef : Skyray.Language.UCMultiple
    {

        #region Fields

        /// <summary>
        /// 当前工作曲线
        /// </summary>
        private WorkCurve workCurveCurrent;

        /// <summary>
        /// 选择元素数据下标
        /// </summary>
        private int SelectIndex = 0;

        /// <summary>
        /// 返回对话结果
        /// </summary>
        public DialogResult DialogResult { get; set; }

        #endregion

        #region Init

        /// <summary>
        /// 获取文本
        /// </summary>
        public override void SetText()
        {
            this.dgwRefElement.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        }

        public UCRef()
        {
            InitializeComponent();
            if (DifferenceDevice.IsRohs)
            {
                ColDistrubThreshold.Visible = true;
                ColName.Width = 90;
                ColRefConf.Width = 90;
                ColIsRef.Width = 90;
            }
        }

        public UCRef(bool isEditData)
        {
            InitializeComponent();
            if (isEditData)
            {
                btnSave.Visible = false;
                btnCancel.Visible = false;
                btnApplication.Visible = true;
                btnUp.Visible = false;
                btWRefTo.Visible = false;
            }
            if (DifferenceDevice.IsRohs)
            {
                ColDistrubThreshold.Visible = true;
                ColName.Width = 90;
                ColRefConf.Width = 90;
                ColIsRef.Width = 90;
            }
        }

        private void UCRef_Load(object sender, EventArgs e)
        {
            if (WorkCurveHelper.WorkCurveCurrent == null) return;
            workCurveCurrent = WorkCurve.FindById(WorkCurveHelper.WorkCurveCurrent.Id);
            if (workCurveCurrent != null)
            {
                lbwElementList.Items.Clear();
                foreach (var sp in workCurveCurrent.ElementList.Items)
                {
                    lbwElementList.Items.Add(sp.Caption);
                }
                if (workCurveCurrent.ElementList.Items.Count > 0)
                {
                    this.lbwElementList.SelectedIndex = 0;
                }
            }
            if (DifferenceDevice.IsXRF && !WorkCurveHelper.IsEditRefCoefficient)
            {
                this.ColRefConf.ReadOnly = true;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 工作曲线列表绑定至表格
        /// </summary>
        private void ElementRefListToGrid()
        {
            InitElementRef();
            BindingSource bs = new BindingSource();
            for (int i = 0; i < workCurveCurrent.ElementList.Items[SelectIndex].ElementRefs.Count; i++)
            {
                bs.Add(workCurveCurrent.ElementList.Items[SelectIndex].ElementRefs[i]);
            }
            this.dgwRefElement.AutoGenerateColumns = false;
            this.dgwRefElement.DataSource = bs;
        }

        /// <summary>
        /// 初始化影响元素
        /// </summary>
        private void InitElementRef()
        {
            //if (workCurveCurrent.ElementList.Items[SelectIndex].ElementRefs.Count == 0)
            {
                for (int i = 0; i < workCurveCurrent.ElementList.Items.Count; i++)
                {
                    if (i != SelectIndex && workCurveCurrent.ElementList.Items[SelectIndex].ElementRefs.ToList().Find(w => w.Name == workCurveCurrent.ElementList.Items[i].Caption)==null)
                    {
                        workCurveCurrent.ElementList.Items[SelectIndex].ElementRefs.Add(ElementRef.New.Init(workCurveCurrent.ElementList.Items[i].Caption, false, 0));
                    }
                }
            }
        }

        #endregion

        #region Events

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveRef();
            if (this.ParentForm != null)
                this.ParentForm.DialogResult = this.dialogResult = DialogResult.OK;
            EDXRFHelper.GotoMainPage(this);//返回主界面
        }

        /// <summary>
        /// 改变所选元素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbwElementList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectIndex = this.lbwElementList.SelectedIndex;
            ElementRefListToGrid();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            EDXRFHelper.GotoMainPage(this);//返回主界面
        }

        /// <summary>
        /// 数据异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgwRefElement_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Msg.Show(Info.StyleError);
            return;
        }

        /// <summary>
        /// 数据类型验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgwRefElement_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgwRefElement.Columns[e.ColumnIndex].Name.Equals("ColRefConf"))
            {
                decimal Max = Ranges.RangeDictionary["RefConf"].Max;
                decimal Min = Ranges.RangeDictionary["RefConf"].Min;
                try { decimal.Parse(e.FormattedValue.ToString()); }
                catch (Exception)
                {
                    //Msg.Show(Info.StyleError);
                    e.Cancel = true;
                    dgwRefElement.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Max;
                    return;
                }
                if (decimal.Parse(e.FormattedValue.ToString()) > Max)
                {
                    //SkyrayMsgBox.Show(Info.OutOfRange);
                    dgwRefElement.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Max;
                    e.Cancel = true;
                    return;
                }
                if (decimal.Parse(e.FormattedValue.ToString()) < Min)
                {
                    dgwRefElement.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Min;
                    e.Cancel = true;
                }
            }
            else if (dgwRefElement.Columns[e.ColumnIndex].Name.Equals("ColDistrubThreshold"))
            {
                try { decimal.Parse(e.FormattedValue.ToString()); }
                catch (Exception)
                {
                    e.Cancel = true;
                    dgwRefElement.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0;
                    return;
                }
            }
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            SaveRef();
        }

        private void SaveRef()
        {
            #region 影响元素拼字符串
            foreach (CurveElement element in workCurveCurrent.ElementList.Items)
            {
                string SInfluenceElements = "";
                string SInfluenceCoees = "";
                string DistrubThreshold = "";
                foreach (ElementRef eleRef in element.ElementRefs)
                {
                    if (eleRef.IsRef)
                    {
                        SInfluenceElements += eleRef.Name + ",";
                        SInfluenceCoees += eleRef.RefConf.ToString() + ",";
                        DistrubThreshold += eleRef.DistrubThreshold.ToString() + ",";
                    }
                }
                if (!DistrubThreshold.Equals(""))
                    DistrubThreshold = DistrubThreshold.Substring(0, DistrubThreshold.Length - 1);
                if (!SInfluenceElements.Equals(""))
                    SInfluenceElements = SInfluenceElements.Substring(0, SInfluenceElements.Length - 1);
                if (!SInfluenceCoees.Equals(""))
                    SInfluenceCoees = SInfluenceCoees.Substring(0, SInfluenceCoees.Length - 1);
                if (element.ElementRefs.ToList().FindAll(flu => flu.IsRef).ToList().Count > 0)
                    element.IsInfluence = true;
                element.SInfluenceElements = SInfluenceElements;
                element.SInfluenceCoefficients = SInfluenceCoees;
                element.DistrubThreshold = DistrubThreshold;
            }
            #endregion

            workCurveCurrent.Save();
            WorkCurveHelper.WorkCurveCurrent = workCurveCurrent;
        }

        #endregion

        private void btWRefTo_Click(object sender, EventArgs e)
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
                ToolStripControls test = MenuLoadHelper.MenuStripCollection.Find(w => w.CurrentNaviItem.Name == "DataOptimization");
                EDXRFHelper.RecurveNextUC(test);
            }

            if (upflag)
            {
                ToolStripControls test = MenuLoadHelper.MenuStripCollection.Find(w => w.CurrentNaviItem.Name == "EditElement");
                EDXRFHelper.RecurseUpUC(test);
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            btnSave_Click(null, null);
            upflag = true;
        }

    }
}

