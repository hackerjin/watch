using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common;
using Skyray.Language;
using Skyray.EDXRFLibrary;

namespace Skyray.UC
{
    /// <summary>
    /// 计算强度类
    /// </summary>
    public partial class UcTabParams : Skyray.Language.UCMultiple
    {
        /// <summary>
        /// 计算强度
        /// </summary>
        public UcTabParams()
        {
            InitializeComponent();
            
        }

        private void btWSubmit_Click(object sender, EventArgs e)
        {

            if (numTabResetHeight.Value > WorkCurveHelper.DeviceCurrent.MotorZMaxStep)
            {
                Msg.Show("平台复位高度不可大于Z轴最大行程");
                return;
            }

            if (numInOutDis.Value > WorkCurveHelper.DeviceCurrent.MotorYMaxStep)
            {
                Msg.Show("进出样距离不可大于Y轴最大行程");
                return;
            }

            if (DifferenceDevice.CurCameraControl.largeViewCamera != null && numTwoCameraDis.Value > numInOutDis.Value)
            {
                Msg.Show("远景与近景摄像头距离不可大于进出样距离");
                return;
            }

            if (WorkCurveHelper.DeviceCurrent.HasMotorSpin && numTestDis.Value > WorkCurveHelper.DeviceCurrent.MotorSPinMaxStep)
            {
                Msg.Show("近景摄像头与测试点距离不可大于Y1轴最大行程");
                return;
            }

            //此处仅仅对平台复位高度进行更新，ResetZ已自动更新从而在下次getZ的时候影响当前高度值
            WorkCurveHelper.TabResetHeight = (float)numTabResetHeight.Value;

            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/TabResetHeight", WorkCurveHelper.TabResetHeight.ToString());

            //以下变量更新之后立即生效
            WorkCurveHelper.InOutDis = (float)numInOutDis.Value;

            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/InOutDis", WorkCurveHelper.InOutDis.ToString());
          
            WorkCurveHelper.TwoCameraDis = (float)numTwoCameraDis.Value;
            
            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/TwoCameraDis", WorkCurveHelper.TwoCameraDis.ToString());

            WorkCurveHelper.LargeViewDis = WorkCurveHelper.InOutDis - WorkCurveHelper.TwoCameraDis;
       
            WorkCurveHelper.TestDis = (float)numTestDis.Value;
            
            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/TestDis", WorkCurveHelper.TestDis.ToString());


            //此处为远景图像大小公式的各个系数
            WorkCurveHelper.squareCoeff = (float)numSquareCoeff.Value;

            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/squareCoeff", WorkCurveHelper.squareCoeff.ToString());

            WorkCurveHelper.multiCoeff = (float)numMultiCoeff.Value;

            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/multiCoeff", WorkCurveHelper.multiCoeff.ToString());

            WorkCurveHelper.baseCoeff = (float)numBaseCoeff.Value;

            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/baseCoeff", WorkCurveHelper.baseCoeff.ToString());

            WorkCurveHelper.heightWidthRatio = (float)numHeightWidthRatio.Value;

            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/heightWidthCoeff", WorkCurveHelper.heightWidthRatio.ToString());

            new System.Threading.Thread(new System.Threading.ThreadStart(((UCCameraControl)WorkCurveHelper.ucCamera).updateLargeViewNow)).Start();

            EDXRFHelper.GotoMainPage(this);
        }

       

        private void btWCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }

        private void UcTabParams_Load(object sender, EventArgs e)
        {
            numTabResetHeight.Value = (decimal)WorkCurveHelper.TabResetHeight;
            numInOutDis.Value = (decimal)WorkCurveHelper.InOutDis;
            numTwoCameraDis.Value = (decimal)WorkCurveHelper.TwoCameraDis;
            numTestDis.Value = (decimal)WorkCurveHelper.TestDis;
            numSquareCoeff.Value = (decimal)WorkCurveHelper.squareCoeff;
            numMultiCoeff.Value = (decimal)WorkCurveHelper.multiCoeff;
            numBaseCoeff.Value = (decimal)WorkCurveHelper.baseCoeff;
            numHeightWidthRatio.Value = (decimal)WorkCurveHelper.heightWidthRatio;
        }
       
    }
}
