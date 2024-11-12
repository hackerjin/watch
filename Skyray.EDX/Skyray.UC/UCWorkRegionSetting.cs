using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;
using Lephone.Data.Common;
using Skyray.EDX.Common;

namespace Skyray.UC
{
    public partial class UCWorkRegionSetting : Skyray.Language.UCMultiple
    {
        private DbObjectList<WorkRegion> lstWorkRegion;

        public UCWorkRegionSetting()
        {
            InitializeComponent();
        }

        private void UCWorkRegionSetting_Load(object sender, EventArgs e)
        {
            BindingData();
            if (lstWorkRegion.Count > 0)
            {
                //this.cboLikeWorkRegionName.Text = lstWorkRegion[0].Caption;
                this.cboLikeWorkRegionName.SelectedIndex = 0;
            }
        }

        private void BindingData()
        {
            //this.cboLikeWorkRegionName.Items.Clear();
            lstWorkRegion = WorkRegion.FindAll();
            cboLikeWorkRegionName.DataSource = lstWorkRegion;
            cboLikeWorkRegionName.DisplayMember = "Caption";
            cboLikeWorkRegionName.ValueMember = "Name";
            cboLikeWorkRegionName.ColumnNames = "Caption";
            cboLikeWorkRegionName.ColumnWidths = this.cboLikeWorkRegionName.Width.ToString();
            //lstWorkRegion = WorkRegion.FindAll();
            BindingSource bs = new BindingSource();
            //this.cboLikeWorkRegionName.Items.Clear();
            foreach (var region in lstWorkRegion)
            {
                bs.Add(region);
            }
            dgvWorkRegion.AutoGenerateColumns = false;
            dgvWorkRegion.DataSource = bs;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //if (txtWorkRegionName.Text.IsNum())
            //{
            //    return;
            //}
            
            if (ValidateHelper.IllegalCheck(txtWorkRegionName))
            {
                if (cboLikeWorkRegionName.Text == "")
                {
                    Msg.Show(Info.SelectWorkRegion);
                    return;
                }
                var region = lstWorkRegion.Find(w=>w.Caption == cboLikeWorkRegionName.Text);
                WorkRegion workRegion = WorkRegion.New.Init(txtWorkRegionName.Text, region.RohsSampleType, false, txtWorkRegionName.Text);
                workRegion.Save();
                if (checkBoxWSimilarWorkgion.Checked)
                    AddRegionWorkCurve(workRegion);
                if (DifferenceDevice.irohs != null)
                {
                    DifferenceDevice.irohs.ReloadWorkSpace(workRegion.Name);
                }
                BindingData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvWorkRegion.SelectedRows.Count > 0)
            {
                int index = dgvWorkRegion.SelectedRows[0].Index;
                if (lstWorkRegion[index].IsDefaultWorkRegion)
                {
                    Msg.Show(Info.DeleteDefaultWorkRegion);
                    return;
                }
                DbObjectList<WorkCurve> lstWorkCurve = WorkCurve.Find(w=>w.WorkRegion.Id == lstWorkRegion[index].Id);
                foreach(var curve in lstWorkCurve)
                {
                    curve.Delete();
                }
                lstWorkRegion[index].Delete();
                //lstWorkRegion.RemoveAt(index);
                if (DifferenceDevice.irohs != null)
                {
                    DifferenceDevice.irohs.ReloadWorkSpace(lstWorkRegion[index].Name);
                }
                BindingData();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);//返回主界面
        }

        private void AddRegionWorkCurve(WorkRegion workRegion)
        {
            WorkRegion defaultRegion = lstWorkRegion.Find(w => w.RohsSampleType == workRegion.RohsSampleType && w.IsDefaultWorkRegion);
            if (defaultRegion == null) return;
            DbObjectList<WorkCurve> lstWorkCurve = WorkCurve.Find(w => w.WorkRegion.Id == defaultRegion.Id);
            if (lstWorkCurve.Count == 0) return;
            foreach (var curve in lstWorkCurve)
            {
                WorkCurve work = CopyWorkCurveInfo(curve);
                work.WorkRegion = workRegion;
                work.Save();
            }
        }

        private WorkCurve CopyWorkCurveInfo(WorkCurve curve)
        {
            WorkCurve work = WorkCurve.New.Init(curve.Name, curve.ConditionName, curve.CalcType, curve.FuncType, curve.IsAbsorb, curve.LimitThickness, false, false, false, false, false, 0, "", false, Info.strAreaDensityUnit, curve.TestTime,true);
            work.IsDefaultWorkCurve = curve.IsDefaultWorkCurve;
            work.Condition = curve.Condition;
            return work;
        }
    }
}
