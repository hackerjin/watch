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

namespace Skyray.UC
{
    public partial class UCResolveParams : Skyray.Language.UCMultiple
    {
        public UCResolveParams()
        {
            InitializeComponent();
            LoadDataSource();
        }

        private void LoadDataSource()
        {
            var listElementName = from item in Atoms.AtomList select item.AtomName;
            this.cbxElement.DataSource = listElementName.ToList();
            this.cbxElement.AutoCompleteCustomSource.AddRange(listElementName.ToArray());
            this.cbxElement.AutoCompleteMode = AutoCompleteMode.Append;
            this.cbxElement.AutoCompleteSource = AutoCompleteSource.ListItems;
            if (this.cbxElement.Items.Count > 0)
                this.cbxElement.SelectedIndex = 0;
            var lstCondition = Condition.Find(c => c.Device.Id == WorkCurveHelper.DeviceCurrent.Id && c.Type == ConditionType.Normal);
            var conditionList = from item in lstCondition select item.Name;
            this.cbxCondition.DataSource = conditionList.ToList();
            this.cbxCondition.AutoCompleteCustomSource.AddRange(conditionList.ToArray());
            this.cbxCondition.AutoCompleteMode = AutoCompleteMode.Append;
            this.cbxCondition.AutoCompleteSource = AutoCompleteSource.ListItems;
            if (this.cbxCondition.Items.Count > 0)
                this.cbxCondition.SelectedIndex = 0;
        }

        private Condition condition;
        private Atom atom;


        private void btnSubmit_Click_1(object sender, EventArgs e)
        {
            if (DifferenceDevice.irohs != null)
                atom = Atoms.AtomList.Find(w => w.AtomName == this.cbxElement.Text);
            if (this.ParentForm != null)
                this.ParentForm.DialogResult = this.dialogResult = DialogResult.OK;
            EDXRFHelper.GotoMainPage(this);
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }

        public override void ExcuteEndProcess(params object[] objs)
        {
            if (this.condition != null && atom != null)
                DifferenceDevice.irohs.ExcuteResolveCaculate(condition, atom.AtomID,this.cbxDeviceParamsList.SelectedIndex);
            else
            {
                Msg.Show(Info.SelectError);
            }
        }

        private void cbxCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cbxDeviceParamsList.Items.Clear();
            this.condition = Condition.FindOne(c => c.Device.Id == WorkCurveHelper.DeviceCurrent.Id && c.Type == ConditionType.Normal && c.Name == this.cbxCondition.Text);
            foreach (DeviceParameter deviceParams in this.condition.DeviceParamList)
                this.cbxDeviceParamsList.Items.Add(deviceParams.Name);
            if (this.cbxDeviceParamsList.Items.Count > 0)
                this.cbxDeviceParamsList.SelectedIndex = 0;
        }
    }
}
