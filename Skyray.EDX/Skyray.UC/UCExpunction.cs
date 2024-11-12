using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;
using Lephone.Data.Common;

namespace Skyray.UC
{
    public partial class UCExpunction : Skyray.Language.UCMultiple
    {
        private WorkCurve workCurve;
        /// <summary>
        /// 元素集合
        /// </summary>
        private DbObjectList<CurveElement> lstElement;

        public UCExpunction()
        {
            InitializeComponent();
        }

        private void UCExpunction_Load(object sender, EventArgs e)
        {
            if (WorkCurveHelper.WorkCurveCurrent != null)
            {
                workCurve = WorkCurve.FindById(WorkCurveHelper.WorkCurveCurrent.Id);
                lstElement = CurveElement.Find(c => c.ElementList.Id == workCurve.ElementList.Id);
                foreach (var element in lstElement)
                {
                    if (element.Expunction == null)
                    {
                        var expunction = Expunction.New;
                        expunction.ElementName = element.Caption;
                        element.Expunction = expunction;
                        element.Save();
                    }
                }
                ExpunctionToGrid();
            }
        }

        /// <summary>
        /// 绑定列表
        /// </summary>
        private void ExpunctionToGrid()
        {
            dgvwExpunction.Rows.Clear();
            dgvwExpunction.DataSource = null;
            BindingSource bs = new BindingSource();
            foreach (var element in lstElement)
            {
                bs.Add(element.Expunction);
            }
            dgvwExpunction.AutoGenerateColumns = false;
            dgvwExpunction.DataSource = bs;//绑定数据源

        }

        private void btnOK_Click(object sender, EventArgs e)
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);//转到主界面
        }

        private void dgvwExpunction_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (!dgvwExpunction.Columns[e.ColumnIndex].Name.Equals("ColElement"))
            {
                try { decimal.Parse(e.FormattedValue.ToString()); }
                catch (Exception)
                {
                    e.Cancel = true;
                    dgvwExpunction.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0;
                    return;
                }
            }
        }

        private void dgvwExpunction_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Msg.Show(Info.StyleError);
            return;
        }
    }
}
