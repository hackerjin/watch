using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common.Component;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary;

namespace Skyray.UC
{
    public partial class UCMoveWorksStation : Skyray.Language.UCMultiple
    {
        // BgColor =234, 247, 254

        private Device currentDevie;
        private MotorInstance motorHelper = new MotorInstance();
        public event StopEnableControl OnStopEnableControl;

        public Device CurrentDecvice
        {
            get { return currentDevie; }
            set
            {
                DisableControl(value);
                currentDevie = value;
                WorkCurveHelper.DeviceCurrent = currentDevie;
                motorHelper.CreateInstance(MoterType.XMotor);
                motorHelper.CreateInstance(MoterType.YMotor);
                motorHelper.CreateInstance(MoterType.ZMotor);
            }
        }

        public UCMoveWorksStation()
        {
            InitializeComponent();
            //CameraRefMotor.OnMotorNoMoveEventHandle += new CameraRefMotor.MotorNoMoveEventHandle(CameraRefMotor_OnMotorNoMoveEventHandle);
            this.textBoxWInputWalk.Text = "20";
        }


        public override void PageLoad(object sender, EventArgs e)
        {
            if (!(this.Parent is Form))
            {
                this.buttonCancel.Visible = false;
                this.buttonSubmit.Text = Info.Save;
            }
            base.PageLoad(sender, e);
        }

        void CameraRefMotor_OnMotorNoMoveEventHandle()
        {
            Msg.Show(Info.SubmitMotorInformation);
        }

        public void DisableControl(Device device)
        {
            if (!device.HasMotorX && !device.HasMotorY)
                this.vScrollXYSpeed.Enabled = false;

            if (!device.HasMotorX)
            {
                this.btnXAxisLeft.Enabled = false;
                this.btnXAxisRight.Enabled = false;
            }
            else
            {
                this.btnXAxisLeft.Enabled = true;
                this.btnXAxisRight.Enabled = true;
                this.vScrollXYSpeed.Enabled = true;

                if (device.MotorXSpeed > this.vScrollXYSpeed.Maximum)
                    device.MotorXSpeed = this.vScrollXYSpeed.Maximum;

                if (device.MotorXSpeed < this.vScrollXYSpeed.Minimum)
                    device.MotorXSpeed = this.vScrollXYSpeed.Minimum;
                this.vScrollXYSpeed.Value = device.MotorXSpeed;
                this.lblXYSpeed.Text = (this.vScrollXYSpeed.Maximum - this.vScrollXYSpeed.Value + 1).ToString();
            }

            if (!device.HasMotorY)
            {
                this.btnYAxisIn.Enabled = false;
                this.btnYAxisOut.Enabled = false;
            }
            else
            {
                this.btnYAxisIn.Enabled = true;
                this.btnYAxisOut.Enabled = true;
                this.vScrollXYSpeed.Enabled = true;
            }

            if (!device.HasMotorZ)
            {
                this.btnZAxisOpen.Enabled = false;
                this.btnZAxisClose.Enabled = false;
                this.vScrollZSpeed.Enabled = false;
                this.btnAutoFocal.Enabled = false;
                this.chkIsLazerOpen.Enabled = false;
            }
            else
            {
                this.btnZAxisOpen.Enabled = true;
                this.btnZAxisClose.Enabled = true;
                this.vScrollZSpeed.Enabled = true;
                this.btnAutoFocal.Enabled = true;
                this.chkIsLazerOpen.Enabled = true;

                if (device.MotorZSpeed > this.vScrollZSpeed.Maximum)
                    device.MotorZSpeed = this.vScrollZSpeed.Maximum;

                if (device.MotorZSpeed < this.vScrollZSpeed.Minimum)
                    device.MotorZSpeed = this.vScrollZSpeed.Minimum;
                this.vScrollZSpeed.Value = device.MotorZSpeed;
                this.lblZSpeed.Text = (this.vScrollZSpeed.Maximum - this.vScrollZSpeed.Value + 1).ToString();
            }

            if (!this.chkIsLazerOpen.Checked)
                btnAutoFocal.Enabled = false;
            this.buttonStop.Enabled = true;
            this.buttonReset.Enabled = true;
            this.checkBoxWalk.Enabled = true;
            this.textBoxWInputWalk.Enabled = true;
            this.buttonSubmit.Enabled = true;

            if (!device.HasMotorZ && !device.HasMotorY && !device.HasMotorX)
            {
                this.buttonStop.Enabled = false;
                this.buttonSubmit.Enabled = false;
                this.buttonReset.Enabled = false;
                this.checkBoxWalk.Enabled = false;
                this.textBoxWInputWalk.Enabled = false;
            }
        }


        /// <summary>
        /// 规定步长
        /// </summary>
        public int FixedStep
        {
            get
            {
                try
                {
                    int fixedStep = (int)(textBoxWInputWalk.Value);
                    if (fixedStep > CameraRefMotor.MAX_XYZ_AXES_MOTOR_STEP)
                        fixedStep = CameraRefMotor.MAX_XYZ_AXES_MOTOR_STEP;
                    return fixedStep;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// X Y轴速度
        /// </summary>
        public int XYSpeed
        {
            get
            {
                if (vScrollXYSpeed.Value < CameraRefMotor.MAX_XY_AXES_MOTOR_SPEED_GEAR)
                {
                    return CameraRefMotor.MAX_XY_AXES_MOTOR_SPEED_GEAR;
                }
                else
                {
                    int speed = vScrollXYSpeed.Value;
                    return speed;
                }
            }
        }

        // 升降平台速度
        public int ZSpeed
        {
            get
            {
                if (vScrollZSpeed.Value < CameraRefMotor.MAX_Z_AXES_MOTOR_SPEED_GEAR)
                {
                    return CameraRefMotor.MAX_Z_AXES_MOTOR_SPEED_GEAR;
                }
                else
                {
                    int speed = vScrollZSpeed.Value;
                    return speed;
                }
            }
        }


        /// <summary>
        /// y轴向上按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnYAxisIn_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && currentDevie.HasMotorY)
            {

                int value = XYSpeed;
                if (checkBoxWalk.Checked)
                    MotorInstance.s_singleInstanceY.MoveIn(FixedStep, value);
                else
                    MotorInstance.s_singleInstanceY.MoveIn(CameraRefMotor.MAX_XYZ_AXES_MOTOR_STEP, value);
            }
        }

        /// <summary>
        ///y轴向上弹起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnYAxisIn_MouseUp(object sender, MouseEventArgs e)
        {
            if (!checkBoxWalk.Checked && e.Button == MouseButtons.Left && currentDevie.HasMotorY)
            {
                MotorInstance.s_singleInstanceY.Cancel();
            }
        }

        /// <summary>
        /// 往左移动按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnXAxisLeft_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && currentDevie.HasMotorX)
            {
                int value = XYSpeed;
                if (checkBoxWalk.Checked)
                    MotorInstance.s_singleInstanceX.MoveLeft(FixedStep, value);
                else
                    MotorInstance.s_singleInstanceX.MoveLeft(CameraRefMotor.MAX_XYZ_AXES_MOTOR_STEP, value);
            }
        }

        /// <summary>
        ///  往左移动弹起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnXAxisLeft_MouseUp(object sender, MouseEventArgs e)
        {
            if (!checkBoxWalk.Checked && e.Button == MouseButtons.Left && currentDevie.HasMotorX)
            {
                MotorInstance.s_singleInstanceX.Cancel();
            }
        }

        /// <summary>
        /// y轴向下弹起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnYAxisOut_MouseUp(object sender, MouseEventArgs e)
        {
            if (!checkBoxWalk.Checked && e.Button == MouseButtons.Left && currentDevie.HasMotorY)
            {
                MotorInstance.s_singleInstanceY.Cancel();
            }
        }

        /// <summary>
        /// y轴向下按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnYAxisOut_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && currentDevie.HasMotorY)
            {
                int value = XYSpeed;
                if (checkBoxWalk.Checked)
                    MotorInstance.s_singleInstanceY.MoveOut(FixedStep, value);
                else
                    MotorInstance.s_singleInstanceY.MoveOut(CameraRefMotor.MAX_XYZ_AXES_MOTOR_STEP, value);
            }
        }

        /// <summary>
        /// X轴按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnXAxisRight_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && currentDevie.HasMotorX)
            {
                int value = XYSpeed;
                if (checkBoxWalk.Checked)
                    MotorInstance.s_singleInstanceX.MoveRight(FixedStep, value);
                else
                    MotorInstance.s_singleInstanceX.MoveRight(CameraRefMotor.MAX_XYZ_AXES_MOTOR_STEP, value);
            }
        }

        /// <summary>
        /// X轴弹起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnXAxisRight_MouseUp(object sender, MouseEventArgs e)
        {
            if (!checkBoxWalk.Checked && e.Button == MouseButtons.Left && currentDevie.HasMotorX)
            {
                MotorInstance.s_singleInstanceX.Cancel();
            }
        }

        /// <summary>
        /// Z轴向上按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnZAxisOpen_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && currentDevie.HasMotorZ)
            {
                int value = ZSpeed;
                if (checkBoxWalk.Checked)
                    MotorInstance.s_singleInstanceZ.MoveUp(FixedStep, value);
                //else
                //    MotorInstance.s_singleInstanceZ.MoveUp(CameraRefMotor.MAX_XYZ_AXES_MOTOR_STEP, value);
                else
                    MotorInstance.s_singleInstanceZ.MoveUpSafely(value);
            }
        }

        /// <summary>4
        /// Z轴向上弹起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnZAxisOpen_MouseUp(object sender, MouseEventArgs e)
        {
            if (!checkBoxWalk.Checked && e.Button == MouseButtons.Left && currentDevie.HasMotorZ)
            {
                MotorInstance.s_singleInstanceZ.Cancel();
            }
        }

        /// <summary>
        /// Z轴向下按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnZAxisClose_MouseUp(object sender, MouseEventArgs e)
        {
            if (!checkBoxWalk.Checked && e.Button == MouseButtons.Left && currentDevie.HasMotorZ)
            {
                MotorInstance.s_singleInstanceZ.Cancel();
            }
        }

        /// <summary>
        /// Z轴向下弹起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnZAxisClose_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && currentDevie.HasMotorZ)
            {
                int value = ZSpeed;
                if (checkBoxWalk.Checked)
                    MotorInstance.s_singleInstanceZ.MoveDown(FixedStep, value);
                else
                    MotorInstance.s_singleInstanceZ.MoveDownSafely(value);
            }
        }

        private void chkIsLazerOpen_Click(object sender, EventArgs e)
        {
            if (currentDevie.HasMotorZ)
            {
                if (chkIsLazerOpen.Checked)
                {
                    btnAutoFocal.Enabled = true;
                    MotorInstance.s_singleInstanceZ.OpenHeightLazer();
                }
                else
                {
                    btnAutoFocal.Enabled = false;
                    MotorInstance.s_singleInstanceZ.CloseHeightLazer();
                }
            }
        }

        private void vScrollXYSpeed_ValueChanged(object sender, EventArgs e)
        {
            if (lblXYSpeed.Created && (currentDevie.HasMotorX || currentDevie.HasMotorY))
            {
                lblXYSpeed.Text = (vScrollXYSpeed.Maximum - XYSpeed + 1).ToString();
                CurrentDecvice.MotorYSpeed = XYSpeed;
                CurrentDecvice.MotorXSpeed = XYSpeed;
                string stringSql = "update Device set MotorYSpeed =" + XYSpeed + ", MotorXSpeed=" + XYSpeed + " where id=" + CurrentDecvice.Id;
                Lephone.Data.DbEntry.Context.ExecuteNonQuery(stringSql);
            }
        }

        private void vScrollZSpeed_ValueChanged(object sender, EventArgs e)
        {
            if (lblZSpeed.Created && currentDevie.HasMotorZ)
            {
                lblZSpeed.Text = (vScrollZSpeed.Maximum - ZSpeed + 1).ToString();
                CurrentDecvice.MotorZSpeed = ZSpeed;
                string stringSql = "update Device set MotorZSpeed =" + ZSpeed + " where id=" + CurrentDecvice.Id;
                Lephone.Data.DbEntry.Context.ExecuteNonQuery(stringSql);
            }
        }

        private void btnAutoFocal_Click(object sender, EventArgs e)
        {
            MotorAdvance.AutoTuneHeightSync(ZSpeed);
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            CameraRefMotor.CancelAll();
            if (OnStopEnableControl != null)
                OnStopEnableControl();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            MotorInstance instance = new MotorInstance();
            if (currentDevie.HasMotorY)
            {
                instance.CreateInstance(MoterType.YMotor);
                MotorInstance.s_singleInstanceY.Reposition(0);
            }
            if (currentDevie.HasMotorX)
            {
                instance.CreateInstance(MoterType.XMotor);
                MotorInstance.s_singleInstanceX.Reposition(0);
            }
            
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }

        private void btnZAxisOpen_Click(object sender, EventArgs e)
        {

        }

        private void btnZAxisClose_Click(object sender, EventArgs e)
        {

        }
    }

    public delegate void StopEnableControl();
}
