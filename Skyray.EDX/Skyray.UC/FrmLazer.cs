using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common;
using Skyray.EDX.Common.Component;

namespace Skyray.UC
{
    public partial class FrmLazer : Skyray.Language.UCMultiple
    {
        public FrmLazer()
        {
            //if (DifferenceDevice.interClassMain.deviceMeasure.interfacce.IndiaLazer == null)
            //    DifferenceDevice.interClassMain.deviceMeasure.interfacce.IndiaLazer = new Lazer(DifferenceDevice.interClassMain.deviceMeasure.interfacce.port,
            //                                                                                    true, true);
            //if (DifferenceDevice.interClassMain.deviceMeasure.interfacce.ShellCover == null)
            //    DifferenceDevice.interClassMain.deviceMeasure.interfacce.ShellCover = new Shell(DifferenceDevice.interClassMain.deviceMeasure.interfacce.port,
            //                                                                                    WorkCurveHelper.DeviceCurrent.MotorY1Code,
            //                                                                                    WorkCurveHelper.DeviceCurrent.MotorY1Direct,
            //                                                                                    WorkCurveHelper.DeviceCurrent.Name);
            InitializeComponent();
            InitalCtr();
            Model_LanguageChanged(null,null);
            Language.Lang.Model.LanguageChanged += new EventHandler(Model_LanguageChanged);
        }

        void Model_LanguageChanged(object sender, EventArgs e)
        {
            groupLaser.GroupTitle = Info.Laser;
            groupShell.GroupTitle = Info.Shell;
            rbAuto.Text = Info.Auto;
            rbManual.Text = Info.Manual;
            rbOn.Text = Info.On;
            rbOff.Text= Info.Off;
        }

        private void InitalCtr()
        {
            if (DifferenceDevice.interClassMain.deviceMeasure.interfacce.IndiaLazer != null)
            {
                if (DifferenceDevice.interClassMain.deviceMeasure.interfacce.IndiaLazer.IsManual)
                    rbManual.Checked = true;
                else
                    rbAuto.Checked = true;
            }
            if (DifferenceDevice.interClassMain.deviceMeasure.interfacce.ShellCover != null)
            {
                numId.Value = DifferenceDevice.interClassMain.deviceMeasure.interfacce.ShellCover.MotorCode;
                numSpeed.Value = DifferenceDevice.interClassMain.deviceMeasure.interfacce.ShellCover.Speed;
            }
        }


        void cmbShellMotor_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }

        private void rbManual_CheckedChanged(object sender, EventArgs e)
        {
            if (rbManual.Checked)
            {
                rbOff.Enabled = true;
                rbOn.Enabled = true;
                if(DifferenceDevice.interClassMain.deviceMeasure.interfacce.IndiaLazer != null)
                  DifferenceDevice.interClassMain.deviceMeasure.interfacce.IndiaLazer.SetLaserMode(true);
                if (rbOn.Checked)
                    rbOn_CheckedChanged(null,null);
                else if (rbOff.Checked)
                    rbOff_CheckedChanged(null, null);
            }
            else
            {
                rbOff.Enabled = false;
                rbOn.Enabled = false;
            }
        }

        private void rbAuto_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAuto.Checked && DifferenceDevice.interClassMain.deviceMeasure.interfacce.IndiaLazer != null)
                DifferenceDevice.interClassMain.deviceMeasure.interfacce.IndiaLazer.SetLaserMode(false);
        }

        private void rbOn_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOn.Checked && DifferenceDevice.interClassMain.deviceMeasure.interfacce.IndiaLazer != null)
                DifferenceDevice.interClassMain.deviceMeasure.interfacce.IndiaLazer.Open();
        }

        private void rbOff_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOff.Checked && DifferenceDevice.interClassMain.deviceMeasure.interfacce.IndiaLazer != null)
                DifferenceDevice.interClassMain.deviceMeasure.interfacce.IndiaLazer.Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            btnApply_Click(null, null);
            EDXRFHelper.GotoMainPage(this);
         
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            DifferenceDevice.interClassMain.deviceMeasure.interfacce.IndiaLazer = new Lazer(DifferenceDevice.interClassMain.deviceMeasure.interfacce.port, rbManual.Checked);
            DifferenceDevice.interClassMain.deviceMeasure.interfacce.ShellCover = new Shell(DifferenceDevice.interClassMain.deviceMeasure.interfacce.port,
                                                                                            (int)numId.Value, (int)numSpeed.Value);
            ReportTemplateHelper.SaveSpecifiedValueandCreate("LazerShell", "DeviceId", WorkCurveHelper.DeviceCurrent.Id.ToString());
            ReportTemplateHelper.SaveSpecifiedValueandCreate("LazerShell", "LazerIsManual", rbManual.Checked.ToString());
            ReportTemplateHelper.SaveSpecifiedValueandCreate("LazerShell", "MotorId", numId.Value.ToString());
            ReportTemplateHelper.SaveSpecifiedValueandCreate("LazerShell", "Speed", numSpeed.Value.ToString());
        }

        Form form;
        public override void OpenUC(bool flag, string TitleName, bool isModel, bool noneStyle)
        {
            if (this.IsSignlObject) return;
            form = new Form();
            //this.autoDockManage1.DockForm = form;
            form.BackColor = Color.White;
            form.Text = Info.Laser + " & " + Info.Shell;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.ShowInTaskbar = false;
            int padSpace = 0;
            form.Padding = new Padding(padSpace, padSpace, padSpace, padSpace);
            form.FormClosing += (s, ex) =>
            {
                this.IsSignlObject = false;
            };
            form.Controls.Add(this);
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.ClientSize = new Size(this.Width + padSpace * 2, this.Height + padSpace * 2);
            form.ShowIcon = false;
            form.TopMost = true;
            this.Dock = DockStyle.Fill;
            form.StartPosition = FormStartPosition.CenterParent;
            form.Show();
            this.IsSignlObject = true;
        }
    }
}
