using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;
using System.Windows.Forms;

namespace Skyray.UC
{
    public partial class UCSpecialSpec : Skyray.Language.UCMultiple
    {
        private WorkCurve workCurveCurrent;
        private List<SpecialRemoveItem> lst = new List<SpecialRemoveItem>();
        BindingSource bs = new BindingSource();
        public UCSpecialSpec()
        {
            InitializeComponent();
        }

        private void UCSpecialSpec_Load(object sender, EventArgs e)
        {
            if (WorkCurveHelper.WorkCurveCurrent != null)
                workCurveCurrent = WorkCurve.FindById(WorkCurveHelper.WorkCurveCurrent.Id);
            if (workCurveCurrent.SpecialRemoveParam == null)
            { 
                workCurveCurrent.SpecialRemoveParam = SpecialRemoveSpec.New.Init(0, 0);
                workCurveCurrent.SpecialRemoveParam.Save();
            }
            if (workCurveCurrent != null)
            {
                dgvwSpecalItems.Rows.Clear();
                dgvwSpecalItems.DataSource = null;
                
                for (int i = 0; i < workCurveCurrent.SpecialRemoveParam.RemoveItems.Count; i++)
                {
                    bs.Add(workCurveCurrent.SpecialRemoveParam.RemoveItems[i]);
                }

                dgvwSpecalItems.AutoGenerateColumns = false;
                dgvwSpecalItems.DataSource = bs;//绑定数据源
                txtBaseHigh.Text = workCurveCurrent.SpecialRemoveParam.BaseHigh.ToString();
                txtBaseLow.Text = workCurveCurrent.SpecialRemoveParam.BaseLow.ToString();
            }
        }

        private void btnApplication_Click(object sender, EventArgs e)
        {
            SaveParams();
           // if (workCurveCurrent != null) workCurveCurrent.SpecialRemoveParam.Save();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveParams();
            
            if (WorkCurveHelper.WorkCurveCurrent != null)
                WorkCurveHelper.WorkCurveCurrent = WorkCurve.FindById(WorkCurveHelper.WorkCurveCurrent.Id);
            if (this.ParentForm != null)
                this.ParentForm.DialogResult = this.dialogResult = DialogResult.OK;
            EDXRFHelper.DisplayWorkCurveControls();
            EDXRFHelper.GotoMainPage(this);
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (WorkCurveHelper.WorkCurveCurrent != null)
                WorkCurveHelper.WorkCurveCurrent = WorkCurve.FindById(WorkCurveHelper.WorkCurveCurrent.Id);
            EDXRFHelper.GotoMainPage(this);//返回主界面
        }

        private void SaveParams()
        {
            if (workCurveCurrent == null) return;
            Lephone.Data.DbEntry.Context.FastSave(workCurveCurrent.SpecialRemoveParam.Id,
                 new Lephone.Data.SqlEntry.DataProvider.LineInfo<SpecialRemoveItem>
                 {
                     Objs = workCurveCurrent.SpecialRemoveParam.RemoveItems
                 },
                  new Lephone.Data.SqlEntry.DataProvider.LineInfo<SpecialRemoveItem>
                  {
                      IsToDelete = true,
                      Objs = lst
                  });
            lst.Clear();
            workCurveCurrent = WorkCurve.FindById(WorkCurveHelper.WorkCurveCurrent.Id);
            workCurveCurrent.SpecialRemoveParam.BaseLow = Convert.ToInt32(txtBaseLow.Text);
            workCurveCurrent.SpecialRemoveParam.BaseHigh = Convert.ToInt32(txtBaseHigh.Text);
            workCurveCurrent.SpecialRemoveParam.Save();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (workCurveCurrent == null) return;
            FrmDeviceNewName frm = new FrmDeviceNewName("", Info.NewName, Info.NewName, "");
            frm.ShowDialog();
            if (frm.DialogResult == DialogResult.OK)
            {
                SpecialRemoveItem item = SpecialRemoveItem.New.Init(frm.newDeviceName, 0, 0, 0);
                if (dgvwSpecalItems.SelectedRows.Count>0) dgvwSpecalItems.SelectedRows[0].Selected = false;
                workCurveCurrent.SpecialRemoveParam.RemoveItems.Add(item);
                bs.Add(item);
                //dgvwSpecalItems.Refresh();
                
               // int index = dgvwSpecalItems.Rows.Count-1;

                //dgvwSpecalItems.Rows[index].Selected = true;
            }
            
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dgvwSpecalItems.SelectedRows.Count <= 0) return;
            int index = dgvwSpecalItems.SelectedRows[0].Index;
            string strCaption = dgvwSpecalItems.SelectedRows[0].Cells[SpeccialCaption.Index].Value.ToString();
            int lstIndex = workCurveCurrent.SpecialRemoveParam.RemoveItems.ToList().FindIndex(w => w.Caption == strCaption);
            if (lstIndex >= 0)
            {
                //
                lst.Add(workCurveCurrent.SpecialRemoveParam.RemoveItems[lstIndex]);
                workCurveCurrent.SpecialRemoveParam.RemoveItems.RemoveAt(lstIndex);
                bs.RemoveAt(index);
               // dgvwSpecalItems.Rows.RemoveAt(index);
            }
        } 
    }
}
