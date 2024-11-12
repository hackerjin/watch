using System;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;
using Skyray.Controls;
using Skyray.EDX.Common;
using Lephone.Data.Common;
using Lephone.Data.Definition;
using Skyray.EDX.Common.Component;
using System.Collections.Generic;
using System.Threading;
using System.Drawing;

namespace Skyray.UC
{
    public partial class FrmAutoDetection : Skyray.Language.MultipleForm
    {
        public FrmAutoDetection()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 当前设备
        /// </summary>
        private Device devCurrent;

        private Thread thread = null;
        private delegate void delUpdateControl(Control lbl, string value, Color color);
        private delUpdateControl delUpdateLable = null;
        private delegate void delUpdateProgress(int value);
        private delUpdateProgress delUpdateProBar = null;

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            if (thread != null)
                thread.Abort();
        }

        private void UpdateControl(Control lbl, string value, Color color)
        {
            lbl.Text = value;
            lbl.ForeColor = color;
        }

        private void UpdateProBar(int value)
        {
            this.proBarDetection.Value = value;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //DialogResult = DialogResult.Cancel;
            DialogResult = DialogResult.OK;
            if (thread != null)
                thread.Abort();
        }

        private void FrmAutoDetection_Load(object sender, EventArgs e)
        {
            devCurrent = WorkCurveHelper.DeviceCurrent;
            delUpdateLable = new delUpdateControl(UpdateControl);
            delUpdateProBar = new delUpdateProgress(UpdateProBar);
            thread = new Thread(new ThreadStart(Detection));
            //thread.ApartmentState = ApartmentState.STA;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void Detection()
        {
            if (DialogResult.OK == Msg.Show(Info.PleaseCurveCalibrationSample + "Ag", MessageBoxButtons.OKCancel, MessageBoxIcon.Information))//放入AG继续检测
            {
                DifferenceDevice.interClassMain.SelfCheckObject = new SelfCheckObject();
                DifferenceDevice.interClassMain.LoadDetectionParam();
                if (!DeviceConnection())//先连接设备
                {
                    DetectionFaild();
                    return;
                }
                if (!DetectionHVLock())//判断高压开关信号
                {
                    DetectionFaild();
                    return;
                }
                this.Invoke(delUpdateProBar, 100 / 6);
                if (!DetectionCollimator())//判断准直器
                {
                    DetectionFaild();
                    return;
                }
                this.Invoke(delUpdateProBar, 100 * 2 / 6);
                if (!DetectionFilter())//判断滤光片
                {
                    DetectionFaild();
                    return;
                }
                this.Invoke(delUpdateProBar, 100 * 3 / 6);
                if (!DetectionHV())//判断高压
                {
                    DetectionFaild();
                    return;
                }
                this.Invoke(delUpdateProBar, 100 * 4 / 6);
                if (!DetectionDetector())//判断探测器
                {
                    DetectionFaild();
                    return;
                }
                this.Invoke(delUpdateProBar, 100 * 5 / 6);
                if (!DetectionVacuumPump())//判断真空泵
                {
                    DetectionFaild();
                    return;
                }
                this.Invoke(delUpdateProBar, 100);
                this.Invoke(delUpdateLable, lblDetectionResult, Info.strDetectionSuccess, System.Drawing.Color.Green);
                //Msg.Show(Info.strDetectionSuccess);
                if (Msg.Show(Info.strDetectionSuccess + "," + Info.IsPrintReport, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DifferenceDevice.interClassMain.SelfCheckObject.Result = "Pass";
                    //保存报告
                    DifferenceDevice.interClassMain.SaveSelfCheckReport();
                }
                DialogResult = DialogResult.OK;
                if (thread != null)
                    thread.Abort();
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void DetectionFaild()
        {
            //Msg.Show(Info.strDetectionFaild);
            if (Msg.Show(Info.strDetectionFaild + "," + Info.IsPrintReport, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DifferenceDevice.interClassMain.SelfCheckObject.Result = "False";
                //保存报告
                DifferenceDevice.interClassMain.SaveSelfCheckReport();
            }
            DialogResult = DialogResult.OK;
            if (thread != null)
                thread.Abort();
        }

        private bool DeviceConnection()
        {
            if (DifferenceDevice.interClassMain.DeviceConnection())
            {
                return true;
            }
            else
            {
                if (DialogResult.OK == Msg.Show(Info.NoDeviceConnect, MessageBoxButtons.OKCancel, MessageBoxIcon.Information))
                {
                    return DeviceConnection();
                }
                else
                {
                    return false;
                }
            }
        }

        private bool DetectionHVLock()
        {
            if (!devCurrent.HasElectromagnet)
            {
                DifferenceDevice.interClassMain.SelfCheckObject.HVLock = "No";
                return true;
            }
            this.Invoke(delUpdateLable, lblDetectionResult, Info.strHVLock + Info.strDetecting, System.Drawing.Color.Black);
            if (DifferenceDevice.interClassMain.DetectionHVLock())
            {
                DifferenceDevice.interClassMain.SelfCheckObject.HVLock = "Pass";
                this.Invoke(delUpdateLable, lblDetectionResult, Info.strHVLock + Info.strNormal, System.Drawing.Color.Green);
                return true;
            }
            else
            {
                this.Invoke(delUpdateLable, lblDetectionResult, Info.strHVLock + Info.strAbnormity, System.Drawing.Color.Red);
                if (DialogResult.OK == Msg.Show(Info.strHVLock + Info.strAbnormity + "," + Info.strPleaseInspect, MessageBoxButtons.OKCancel, MessageBoxIcon.Information))
                {
                    return DetectionHVLock();
                }
                else
                {
                    DifferenceDevice.interClassMain.DetectionClosePumpLock();
                    DifferenceDevice.interClassMain.SelfCheckObject.HVLock = "False";
                    return false;
                }
            }
        }

        private bool DetectionCollimator()
        {
            if (!devCurrent.HasCollimator)
            {
                DifferenceDevice.interClassMain.SelfCheckObject.Collimator = "No";
                return true;
            }
            this.Invoke(delUpdateLable, lblDetectionResult, Info.Collimator + Info.strDetecting, System.Drawing.Color.Black);
            if (DifferenceDevice.interClassMain.DetectionMotor(devCurrent.CollimatorElectricalCode, devCurrent.CollimatorElectricalDirect, devCurrent.CollimatorSpeed))
            {
                this.Invoke(delUpdateLable, lblDetectionResult, Info.Collimator + Info.strNormal, System.Drawing.Color.Green);
                DifferenceDevice.interClassMain.SelfCheckObject.Collimator = "Pass";
                return true;
            }
            else
            {
                this.Invoke(delUpdateLable, lblDetectionResult, Info.Collimator + Info.strAbnormity, System.Drawing.Color.Red);
                DifferenceDevice.interClassMain.DetectionClosePumpLock();
                DifferenceDevice.interClassMain.SelfCheckObject.Collimator = "False";
                return false;
            }
        }

        private bool DetectionFilter()
        {
            if (!devCurrent.HasFilter)
            {
                DifferenceDevice.interClassMain.SelfCheckObject.Filter = "No";
                return true;
            }
            this.Invoke(delUpdateLable, lblDetectionResult, Info.Filter + Info.strDetecting, System.Drawing.Color.Black);
            if (DifferenceDevice.interClassMain.DetectionMotor(devCurrent.FilterElectricalCode, devCurrent.FilterElectricalDirect, devCurrent.FilterSpeed))
            {
                this.Invoke(delUpdateLable, lblDetectionResult, Info.Filter + Info.strNormal, System.Drawing.Color.Green);
                DifferenceDevice.interClassMain.SelfCheckObject.Filter = "Pass";
                return true;
            }
            else
            {
                this.Invoke(delUpdateLable, lblDetectionResult, Info.Filter + Info.strAbnormity, System.Drawing.Color.Red);
                DifferenceDevice.interClassMain.DetectionClosePumpLock();
                DifferenceDevice.interClassMain.SelfCheckObject.Filter = "False";
                return false;
            }
        }

        private bool DetectionHV()
        {
            this.Invoke(delUpdateLable, lblDetectionResult, Info.strHighVoltage + Info.strDetecting, System.Drawing.Color.Black);
            if (DifferenceDevice.interClassMain.DetectionHV())
            {
                this.Invoke(delUpdateLable, lblDetectionResult, Info.strHighVoltage + Info.strNormal, System.Drawing.Color.Green);
                DifferenceDevice.interClassMain.SelfCheckObject.HVoltage = "Pass";
                return true;
            }
            else
            {
                this.Invoke(delUpdateLable, lblDetectionResult, Info.strHighVoltage + Info.strAbnormity, System.Drawing.Color.Red);
                DifferenceDevice.interClassMain.DetectionClosePumpLock();
                DifferenceDevice.interClassMain.SelfCheckObject.HVoltage = "False";
                return false;
            }
        }

        private bool DetectionDetector()
        {
            this.Invoke(delUpdateLable, lblDetectionResult, Info.strDetector + Info.strDetecting, System.Drawing.Color.Black);
            if (DifferenceDevice.interClassMain.DetectionDetector(devCurrent))
            {
                this.Invoke(delUpdateLable, lblDetectionResult, Info.strDetector + Info.strNormal, System.Drawing.Color.Green);
                return true;
            }
            else
            {
                this.Invoke(delUpdateLable, lblDetectionResult, Info.strDetector + Info.strAbnormity, System.Drawing.Color.Red);
                DifferenceDevice.interClassMain.DetectionClosePumpLock();
                return false;
            }
        }

        private bool DetectionVacuumPump()
        {
            if (!devCurrent.HasVacuumPump)
            {
                DifferenceDevice.interClassMain.SelfCheckObject.Pump = "No";
                return true;
            }
            this.Invoke(delUpdateLable, lblDetectionResult, Info.strVacuumPump + Info.strDetecting, System.Drawing.Color.Black);
            if (DifferenceDevice.interClassMain.DetectionVacuumPump(devCurrent))
            {
                this.Invoke(delUpdateLable, lblDetectionResult, Info.strVacuumPump + Info.strNormal, System.Drawing.Color.Green);
                DifferenceDevice.interClassMain.SelfCheckObject.Pump = "Pass";
                return true;
            }
            else
            {
                this.Invoke(delUpdateLable, lblDetectionResult, Info.strVacuumPump + Info.strAbnormity, System.Drawing.Color.Red);
                DifferenceDevice.interClassMain.DetectionClosePumpLock();
                DifferenceDevice.interClassMain.SelfCheckObject.Pump = "False";
                return false;
            }
        }
    }
}
