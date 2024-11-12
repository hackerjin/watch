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
using Skyray.Language;

namespace Skyray.UC
{
    public partial class UCPortraitMotor :UCMultiple
    {
        //修改：何晓明 20110711 移动平台创建单体实例
        public static UCPortraitMotor singleObject;
        public static UCPortraitMotor CreateInstance()
        {
            if (singleObject == null)
                singleObject = new UCPortraitMotor();
            return singleObject;
        }
        public override void ExcuteCloseProcess(params object[] objs)
        {
            singleObject = null;
            IsSignlObject = false;
        }

        public override bool IsSignlObject
        {
            get
            {
                return base.IsSignlObject;
            }
            set
            {
                base.IsSignlObject = value;
                if (!value)
                {
                    singleObject = null;
                }
            }
        }

        public UCPortraitMotor()
        {
            InitializeComponent();
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
                && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasMotorZ)
                DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorZCode, WorkCurveHelper.DeviceCurrent.MotorZDirect, 9000000, true, WorkCurveHelper.DeviceCurrent.MotorZSpeed);
        }

        private void buttonDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
                && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasMotorZ)
            {
                DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorZCode, Math.Abs(WorkCurveHelper.DeviceCurrent.MotorZDirect - 1), 9000000, true, WorkCurveHelper.DeviceCurrent.MotorZSpeed);
            }
        }

        private void buttonDown_MouseUp(object sender, MouseEventArgs e)
        {
            if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
                && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasMotorZ)
                DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorZCode, Math.Abs(WorkCurveHelper.DeviceCurrent.MotorZDirect - 1), 0, true, WorkCurveHelper.DeviceCurrent.MotorZSpeed);
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
                && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasMotorZ)
                DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorZCode, WorkCurveHelper.DeviceCurrent.MotorZDirect, 0, true, WorkCurveHelper.DeviceCurrent.MotorZSpeed);
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            //修改：何晓明 20110711 移动平台创建单体实例
            singleObject = null;
            IsSignlObject = false;
            EDXRFHelper.GotoMainPage(this);
        }
    }
}
