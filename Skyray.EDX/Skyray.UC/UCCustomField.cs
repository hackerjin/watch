using System;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;
using Skyray.Controls;
using System.Collections.Generic;
using System.Linq;
using Skyray.Language;

namespace Skyray.UC
{
    /// <summary>
    /// 自定义类
    /// </summary>
    public partial class UCCustomField : UCMultiple
    {
        #region Fields

        /// <summary>
        /// 当前工作曲线
        /// </summary>
        private WorkCurve workCurveCurrent;

        //public event EventDelegate.InitDeviceParameter OnCustomFiled;
        private List<CustomField> lstDel = new List<CustomField>();

        #endregion

        #region Init
        /// <summary>
        /// 构造函数
        /// </summary>
        public UCCustomField()
        {
            InitializeComponent();
        }

        public override void SaveText()
        {
            Skyray.Language.Lang.Model.SaveHeaderTextProperty(this.dgvwCustom);
        }
        public override void SetText()
        {
            if (this.dgvwCustom.Columns.Count > 0)
                Skyray.Language.Lang.Model.SetHeaderTextProperty(this.dgvwCustom);
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCCustomField_Load(object sender, EventArgs e)
        {
            workCurveCurrent = WorkCurve.FindById(WorkCurveHelper.WorkCurveCurrent.Id);
            CustomToGrid();//加载数据列表
        }

        #endregion

        #region Method

        /// <summary>
        /// 加载数据列表
        /// </summary>
        private void CustomToGrid()
        {
            if (workCurveCurrent == null) return;

            BindingSource bs = new BindingSource();
            int count = workCurveCurrent.ElementList.CustomFields.Count;
            for (int i = 0; i < count; i++)
            {
                bs.Add(workCurveCurrent.ElementList.CustomFields[i]);
            }
            dgvwCustom.AutoGenerateColumns = false;
            dgvwCustom.DataSource = bs;
        }

        #endregion

        #region Events

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            workCurveCurrent.ElementList.CustomFields.Add(CustomField.New.Init("", ""));
            CustomToGrid();//加载数据列表
            int count = workCurveCurrent.ElementList.CustomFields.Count;
            if (count > 0)
            {
                dgvwCustom.Rows[0].Selected = false;
                dgvwCustom.Rows[count - 1].Selected = true;
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {

            SaveField();
            if (this.ParentForm != null)
                this.ParentForm.DialogResult = this.dialogResult = DialogResult.OK;
            EDXRFHelper.GotoMainPage(this);//返回主界面
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dgvwCustom.SelectedRows.Count > 0)
            {
                int index = dgvwCustom.SelectedRows[0].Index;
                CustomField param = workCurveCurrent.ElementList.CustomFields[index];
                // add by chuyaqin 2011-04-24
                lstDel.Add(param);
                workCurveCurrent.ElementList.CustomFields.RemoveAt(index);
                CustomToGrid();//加载数据列表
                int count = workCurveCurrent.ElementList.CustomFields.Count;
                if (count > 1)
                {
                    if (index >= count)
                    {
                        this.dgvwCustom.Rows[0].Selected = false;
                        this.dgvwCustom.Rows[count - 1].Selected = true;
                    }
                    else
                    {
                        this.dgvwCustom.Rows[0].Selected = false;
                        this.dgvwCustom.Rows[index].Selected = true;
                    }
                }
            }
            else
            {
                Msg.Show(Info.NoSelect);
            }
        }

        /// <summary>
        ///  关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            lstDel.Clear();
            EDXRFHelper.GotoMainPage(this);//返回主界面
        }

        /// <summary>
        /// 单元格验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvwCustom_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgvwCustom.Columns[e.ColumnIndex].Name.Equals("ColName"))
            {
                string name = e.FormattedValue.ToString();
                var element = workCurveCurrent.ElementList.Items.ToList().Find(ele => String.Compare(ele.Caption, name, true) == 0);
                if (element != null)
                {
                    Msg.Show(Info.CustomNameMustDifferenceWithElement);
                    dgvwCustom[e.ColumnIndex, e.RowIndex].Value = "";
                    e.Cancel = true;
                    return;
                }
                if (Atoms.AtomList.Find(a => a.AtomName.ToLower() == name.ToLower()) != null)
                {
                    Msg.Show(Info.CustomNameMustDifferenceWithElement);
                    dgvwCustom[e.ColumnIndex, e.RowIndex].Value = "";
                    e.Cancel = true;
                    return;
                }
            }
        }

        #endregion

        public override void ExcuteEndProcess(params object[] objs)
        {
            DifferenceDevice.MediumAccess.CustomFileld();
        }

        private void SaveField()
        {
            var lst11 = from CustomField p in workCurveCurrent.ElementList.CustomFields
                        select p.Name;
            var lst22 = lst11.Distinct();
            if (lst11.Count() > lst22.Count())
            {
                Msg.Show(Info.CustomNameOrExprissionRepeat);
                return;
            }
            foreach (var cf in workCurveCurrent.ElementList.CustomFields)
            {
                if (cf.Name.IsNullOrEmpty() || cf.Expression.IsNullOrEmpty())
                {
                    SkyrayMsgBox.Show(Info.CustomNameOrExprissionNull);
                    return;
                }
                else
                {
                    if (!TabControlHelper.ExpressionContainElements(cf))
                    {
                        cf.Delete();
                        Msg.Show(Info.ExpressionInvalidate);
                        return;
                    }
                }
            }
            //modify by chuyaqin 2011-04-24
            workCurveCurrent.ElementList.Save();
            //Lephone.Data.DbEntry.Context.FastSave(workCurveCurrent.ElementList.Id,
            //    new Lephone.Data.SqlEntry.DataProvider.LineInfo<CustomField>
            //    {
            //        Objs = workCurveCurrent.ElementList.CustomFields
            //    },
            //     new Lephone.Data.SqlEntry.DataProvider.LineInfo<CustomField>
            //     {
            //         IsToDelete = true,
            //         Objs = lstDel
            //     });
            //WorkCurveHelper.WorkCurveCurrent = WorkCurve.FindById(workCurveCurrent.Id); 这会导致缓存中的数据混乱。
            lstDel.Clear();
           WorkCurveHelper.WorkCurveCurrent = workCurveCurrent;
        }

        private void btnApplication_Click(object sender, EventArgs e)
        {
            SaveField();
        }

    }
}
