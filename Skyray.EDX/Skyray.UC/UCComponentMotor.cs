using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common;

namespace Skyray.UC
{
    public partial class UCComponentMotor : Skyray.Language.UCMultiple
    {

        private bool preXVisiable = false;
        private bool preYVisiable = false;
        private bool preZVisiable = false;


        private UCComponentMotor()
        {
            InitializeComponent();
            if (this.ucMotorX.MoterType == Skyray.EDX.Common.Component.MoterType.XMotor)
            {
                if (!WorkCurveHelper.DeviceCurrent.HasMotorX)
                {
                    this.ucMotorX.Visible = false;
                    preXVisiable = false;
                    this.Height -= 91;
                }
                else
                    preXVisiable = true;
                this.ucMotorX.MotorStep = CameraRefMotor.MAX_XYZ_AXES_MOTOR_STEP;
            }
            if (this.ucMotorY.MoterType == Skyray.EDX.Common.Component.MoterType.YMotor)
            {
                if (!WorkCurveHelper.DeviceCurrent.HasMotorY)
                {
                    this.ucMotorY.Visible = false;
                    preYVisiable = false;
                    this.Height -= 91;
                }
                else
                    preYVisiable = true;
                this.ucMotorY.MotorStep = CameraRefMotor.MAX_XYZ_AXES_MOTOR_STEP; 
            }
            if (this.ucMotorZ.MoterType == Skyray.EDX.Common.Component.MoterType.ZMotor)
            {
                if (!WorkCurveHelper.DeviceCurrent.HasMotorZ)
                {
                    this.ucMotorZ.Visible = false;
                    preZVisiable = false;
                    this.Height -= 91;
                }
                else
                    preZVisiable = true;
                this.ucMotorZ.MotorStep = CameraRefMotor.MAX_XYZ_AXES_MOTOR_STEP; 
            }
            if (DifferenceDevice.interClassMain != null)
                DifferenceDevice.interClassMain.DeviceSaveMove += new EventHandler(interClassMain_DeviceSaveMove);
        }

        void interClassMain_DeviceSaveMove(object sender, EventArgs e)
        {
            bool changeVisiable = false;
            if (preXVisiable == !WorkCurveHelper.DeviceCurrent.HasMotorX)
                changeVisiable = true;
            if (preYVisiable == !WorkCurveHelper.DeviceCurrent.HasMotorY)
                changeVisiable = true;
            if (preZVisiable == !WorkCurveHelper.DeviceCurrent.HasMotorZ)
                changeVisiable = true;
            if (form != null && changeVisiable)
                form.Close();
        }

        public bool Enable
        {
            set  {
                if (!value)
                {
                    this.ucMotorX.EnableDisable = value;
                    this.ucMotorY.EnableDisable = value;
                    this.ucMotorZ.EnableDisable = value;
                }
                else
                {
                    this.ucMotorX.RefrenshEnable();
                    this.ucMotorY.RefrenshEnable();
                    this.ucMotorZ.RefrenshEnable();
                }
            }
        }

        Form form;
        public override void OpenUC(bool flag, string TitleName, bool isModel, bool noneStyle)
        {
            if (this.IsSignlObject) return;
            form = new Form();
            //this.autoDockManage1.DockForm = form;
            form.BackColor = Color.White;
            form.Text = Info.MoveWorkStation;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.ShowInTaskbar = false;
            int padSpace = 0;
            form.Padding = new Padding(padSpace, padSpace, padSpace, padSpace);
            form.FormClosing += (s, ex) =>
            {
                ReportTemplateHelper.SaveSpecifiedValue("MoveStationVisible", "X", form.Location.X.ToString());
                ReportTemplateHelper.SaveSpecifiedValue("MoveStationVisible", "Y", form.Location.Y.ToString());
                this.IsSignlObject = false;
                zMotor = null;
               
            };
            form.Controls.Add(this);
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.ClientSize = new Size(this.Width + padSpace * 2, this.Height + padSpace * 2);
            form.ShowIcon = false;
            form.TopMost = true;
            this.Dock = DockStyle.Fill;
            form.StartPosition = FormStartPosition.Manual;
            string tempX = ReportTemplateHelper.LoadSpecifiedValue("MoveStationVisible", "X");
            string tempY = ReportTemplateHelper.LoadSpecifiedValue("MoveStationVisible", "Y");
            int width = int.Parse(tempX);
            form.Location = new Point(int.Parse(tempX)==0?(Screen.PrimaryScreen.WorkingArea.Width - this.Width) * 3 / 4:int.Parse(tempX), int.Parse(tempY));
            form.Show();
            this.IsSignlObject = true;
        }

        public static UCComponentMotor zMotor;

        public static UCComponentMotor CreateSingleInstance()
        {
            if (zMotor == null)
            {
                zMotor = new UCComponentMotor();
            }
            return zMotor;
        }
    }
}
