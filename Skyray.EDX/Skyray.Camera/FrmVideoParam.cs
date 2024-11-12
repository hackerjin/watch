/* ***
 * 作者：江苏天瑞仪器股份有限公司
 * 修改日期：2009-03-05
 * 类型：窗体类FrmVideoParam
 * 描述：配置摄像头参数
 * ***/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common;
namespace Skyray.Camera
{
    /// <summary>
    /// 窗体类：摄像头参数配置
    /// </summary>
    public partial class FrmVideoParam : Form
    {
        #region 常量

        private const double VIEW_WIDTH_MAX = 500;
        private const double VIEW_HEIGHT_MAX = 500;
        ///// <summary>
        ///// 视频对象
        ///// </summary>
        private SkyrayCamera camera;
        /// <summary>
        /// 焦点
        /// </summary>
        private int FociIndex;

        #endregion

        #region 字段

        /*!!!!!! 依赖：即以下各数据必须与FociWebCamera中初始化数据保持一致 !!!!!!*/
        private const double DefaultViewWidth = 80;     // 默认视窗宽度
        private const double DefaultViewHeight = 80;    // 默认视窗高度
        private const double DefaultRuleUnit = 5;       // 默认刻度单位
        private const double DefaultFociWidth = 0.2;     // 默认焦斑宽度
        private const double DefaultFociHeight = 0.2;    // 默认焦斑高度

        #endregion

        #region 属性

        /// <summary>
        /// 焦点X轴位置
        /// </summary>
        public Double FociX
        {
            set
            {
                if (value < 0.001)
                    txtFocusX.Text = Convert.ToString(DefaultViewWidth / 2);
                else
                    txtFocusX.Text = value.ToString();
            }
            get
            {
                double temp = Convert.ToDouble(txtFocusX.Text);
                if (temp < 0.001)
                    return DefaultViewWidth / 2;
                else
                    return temp;
            }
        }

        /// <summary>
        /// 焦点Y轴位置
        /// </summary>
        public Double FociY
        {
            set
            {
                if (value < 0.001)
                    txtFocusY.Text = Convert.ToString(DefaultViewHeight / 2);
                else
                    txtFocusY.Text = value.ToString();
            }
            get
            {
                double temp = Convert.ToDouble(txtFocusY.Text);
                if (temp < 0.001)
                    return DefaultViewHeight / 2;
                else
                    return temp;
            }
        }

        /// <summary>
        /// 坐标单位长度
        /// </summary>
        public Double RuleUnit
        {
            set
            {
                if (value < 0.001)
                    txtScaleUnit.Text = Convert.ToString(DefaultRuleUnit);
                else
                    txtScaleUnit.Text = value.ToString();
            }
            get
            {
                double temp = Convert.ToDouble(txtScaleUnit.Text);
                if (temp < 0.001)
                    return DefaultRuleUnit;
                else
                    return temp;
            }
        }

        /// <summary>
        /// 焦斑宽度
        /// </summary>
        public Double FociWidth
        {
            set
            {
                if (value < 0.001)
                    txtSpotWidth.Text = Convert.ToString(DefaultFociWidth);
                else
                    txtSpotWidth.Text = value.ToString();
            }
            get
            {
                double temp = Convert.ToDouble(txtSpotWidth.Text);
                if (temp < 0.001)
                    return DefaultFociWidth;
                else
                    return temp;
            }
        }

        /// <summary>
        /// 焦斑高度
        /// </summary>
        public Double FociHeight
        {
            set
            {
                if (value < 0.001)
                    txtSpotHeight.Text = Convert.ToString(DefaultFociHeight);
                else
                    txtSpotHeight.Text = value.ToString();
            }
            get
            {
                double temp = Convert.ToDouble(txtSpotHeight.Text);
                if (temp < 0.001)
                    return DefaultFociHeight;
                else
                    return temp;
            }
        }

        /// <summary>
        /// 视频显示宽度
        /// </summary>
        public Double ViewWidth
        {
            set
            {
                if (value < 0.001)
                    txtViewWidth.Text = Convert.ToString(DefaultViewWidth);
                else
                    txtViewWidth.Text = value.ToString();
            }
            get
            {
                double temp = Convert.ToDouble(txtViewWidth.Text);
                if (temp < 0.001)
                    return DefaultViewWidth;
                else
                    return temp;
            }
        }

        /// <summary>
        /// 视频显示高度
        /// </summary>
        public Double ViewHeight
        {
            set
            {
                if (value < 0.001)
                    txtViewHeight.Text = Convert.ToString(DefaultViewHeight);
                else
                    txtViewHeight.Text = value.ToString();
            }
            get
            {
                double temp = Convert.ToDouble(txtViewHeight.Text);
                if (temp < 0.001)
                    return DefaultViewHeight;
                else
                    return temp;
            }
        }

        /// <summary>
        /// 焦斑形状
        /// </summary>
        public Skyray.Camera.SkyrayCamera.FociShape Shape
        {
            set
            {
                if (value == Skyray.Camera.SkyrayCamera.FociShape.Rectangle)
                    rbtnSpotShapeRectangle.Checked = true;
                else
                    rbtnSpotShapeEllipse.Checked = true;
            }
            get
            {
                if (rbtnSpotShapeRectangle.Checked)
                    return Skyray.Camera.SkyrayCamera.FociShape.Rectangle;
                else
                    return Skyray.Camera.SkyrayCamera.FociShape.Ellipse;
            }
        }

        /// <summary>
        /// 画面宽度
        /// </summary>
        public double pViewWidth
        {
            get
            {
                double result;
                double.TryParse(txtViewWidth.Text, out result);
                return result;
            }
            set
            {
                txtViewWidth.Text = value.ToString("f2");
            }
        }

        /// <summary>
        /// 画面高度
        /// </summary>
        public double pViewHeight
        {
            get
            {
                double result;
                double.TryParse(txtViewHeight.Text, out result);
                return result;
            }
            set
            {
                txtViewHeight.Text = value.ToString("f2");
            }
        }

        /// <summary>
        /// 单位刻度制
        /// </summary>
        public double pScaleUnit
        {
            get
            {
                double result;
                double.TryParse(txtScaleUnit.Text, out result);
                return result;
            }
            set
            {
                txtScaleUnit.Text = value.ToString("f2");
            }
        }

        /// <summary>
        /// 焦点X坐标值
        /// </summary>
        public double pFocusX
        {
            get
            {
                double result;
                double.TryParse(txtFocusX.Text, out result);
                return result;
            }
            set 
            {
                    txtFocusX.Text = value.ToString("f2");
            }
        }

        /// <summary>
        /// 焦点Y坐标值
        /// </summary>
        public double pFocusY
        {
            get
            {
                double result;
                double.TryParse(txtFocusY.Text, out result);
                return result;
            }
            set
            {
                    txtFocusY.Text = value.ToString("f2");
            }
        }

        /// <summary>
        /// 焦斑宽度
        /// </summary>
        public double pSpotWidth
        {
            get
            {
                double result;
                double.TryParse(txtSpotWidth.Text, out result);
                return result;
            }
            set
            {
                    txtSpotWidth.Text = value.ToString("f2");
            }
        }

        /// <summary>
        /// 焦斑高度
        /// </summary>
        public double pSpotHeight
        {
            get
            {
                double result;
                double.TryParse(txtSpotHeight.Text, out result);
                return result;
            }
            set
            {
                txtSpotHeight.Text = value.ToString("f2");
            }
        }

        #endregion

        #region 构造器

        /// <summary>
        /// 实例构造器
        /// </summary>
        public FrmVideoParam(SkyrayCamera sc, int index)
        {
            InitializeComponent();
            this.Text = SkyrayCamera.SkyrayCameraLangDic.ContainsKey("tsmiCameraParam") ? SkyrayCamera.SkyrayCameraLangDic["tsmiCameraParam"] : "视频参数";
            this.gbxFocus.Text = SkyrayCamera.SkyrayCameraLangDic.ContainsKey("gbxFocus") ? SkyrayCamera.SkyrayCameraLangDic["gbxFocus"] : "光斑中心";
            this.lblFocusX.Text = SkyrayCamera.SkyrayCameraLangDic.ContainsKey("lblFocusX") ? SkyrayCamera.SkyrayCameraLangDic["lblFocusX"] : "X坐标(mm)";
            this.lblFocusY.Text = SkyrayCamera.SkyrayCameraLangDic.ContainsKey("lblFocusY") ? SkyrayCamera.SkyrayCameraLangDic["lblFocusY"] : "Y坐标(mm)";
            this.lblSpotShape.Text = SkyrayCamera.SkyrayCameraLangDic.ContainsKey("lblSpotShape") ? SkyrayCamera.SkyrayCameraLangDic["lblSpotShape"] : "形  状";
            this.rbtnSpotShapeEllipse.Text = SkyrayCamera.SkyrayCameraLangDic.ContainsKey("rbtnSpotShapeEllipse") ? SkyrayCamera.SkyrayCameraLangDic["rbtnSpotShapeEllipse"] : "椭    圆";
            this.rbtnSpotShapeRectangle.Text = SkyrayCamera.SkyrayCameraLangDic.ContainsKey("rbtnSpotShapeRectangle") ? SkyrayCamera.SkyrayCameraLangDic["rbtnSpotShapeRectangle"] : "矩    形";

            this.gbxView.Text = SkyrayCamera.SkyrayCameraLangDic.ContainsKey("gbxView") ? SkyrayCamera.SkyrayCameraLangDic["gbxView"] : "画    面";
            this.lblViewWidth.Text = SkyrayCamera.SkyrayCameraLangDic.ContainsKey("lblViewWidth") ? SkyrayCamera.SkyrayCameraLangDic["lblViewWidth"] : "宽度(mm)";

            this.lblViewHeight.Text = SkyrayCamera.SkyrayCameraLangDic.ContainsKey("lblViewHeight") ? SkyrayCamera.SkyrayCameraLangDic["lblViewHeight"] : "高度(mm)";
            this.lblScaleUnit.Text = SkyrayCamera.SkyrayCameraLangDic.ContainsKey("lblScaleUnit") ? SkyrayCamera.SkyrayCameraLangDic["lblScaleUnit"] : "尺度单位(mm)";
            this.btnDefault.Text = SkyrayCamera.SkyrayCameraLangDic.ContainsKey("btnDefault") ? SkyrayCamera.SkyrayCameraLangDic["btnDefault"] : "默  认";
            this.btnAccept.Text = SkyrayCamera.SkyrayCameraLangDic.ContainsKey("btnAccept") ? SkyrayCamera.SkyrayCameraLangDic["btnAccept"] : "确  定";

            camera = sc;
            FociIndex = index;
            this.rbtnSpotShapeEllipse.Text = CameraInfo.SpotShapeEllipse;
            this.rbtnSpotShapeRectangle.Text = CameraInfo.SpotShapeRectangle;
        }

        private void FrmVideoParam_Load(object sender, EventArgs e)
        {
            gbxSpot.Visible = true;
            this.txtFocusX.ValueChanged += new System.EventHandler(this.setCameraParam);
            this.txtFocusY.ValueChanged += new System.EventHandler(this.setCameraParam);
            this.txtScaleUnit.ValueChanged += new System.EventHandler(this.setCameraParam);
            this.txtSpotHeight.ValueChanged += new System.EventHandler(this.setCameraParam);
            this.txtSpotWidth.ValueChanged += new System.EventHandler(this.setCameraParam);
            this.rbtnSpotShapeEllipse.CheckedChanged += new System.EventHandler(this.setCameraParam);
            this.rbtnSpotShapeRectangle.CheckedChanged += new System.EventHandler(this.setCameraParam);
            this.txtViewHeight.ValueChanged += new System.EventHandler(this.setCameraParam);
            this.txtViewWidth.ValueChanged += new System.EventHandler(this.setCameraParam);
        }
        #endregion

        #region 事件

        /** 刷新文本 **/
        private void VideoParamForm_Shown(object sender, EventArgs e)
        {
            RefreshUI();
        }

        /** 默认焦点位置在中心 **/
        private void btnDefault_Click(object sender, EventArgs e)
        {
            pFocusX = pViewWidth / 2;
            pFocusY = pViewHeight / 2;
            pSpotWidth = pViewWidth;
            pSpotHeight = pViewHeight;
            pScaleUnit = pViewHeight < pViewWidth ? pViewHeight / 10 : pViewWidth / 10;
            setCameraParam();
        }

        private void txtViewWidth_TextChanged(object sender, EventArgs e)
        {
            if (pViewWidth < 0.001)
            {
                pViewWidth = -pViewWidth;
            }
            else if (pViewWidth > VIEW_WIDTH_MAX)
            {
                pViewWidth = VIEW_WIDTH_MAX;
            }
            if (pFocusX > pViewWidth)
            {
                pFocusX = pViewWidth;
            }
            //if (pSpotWidth > pViewWidth / 2)
            //{
                pSpotWidth = pViewWidth / 2;
            //}
            if (pViewWidth <= pViewHeight && pScaleUnit > pViewWidth / 4)
            {
                pScaleUnit = pViewWidth / 4;
            }
        }

        private void txtViewHeight_TextChanged(object sender, EventArgs e)
        {
            if (pViewHeight < 0.001)
            {
                pViewHeight = -pViewHeight;
            }
            else if (pViewHeight > VIEW_HEIGHT_MAX)
            {
                pViewHeight = VIEW_HEIGHT_MAX;
            }
            if (pFocusY > pViewHeight)
            {
                pFocusY = pViewHeight;
            }
            //if (pSpotHeight > pViewHeight / 2)
            //{
                pSpotHeight = pViewHeight / 2;
            //}
            if (pViewHeight < pViewWidth && pScaleUnit > pViewHeight / 4)
            {
                pScaleUnit = pViewHeight / 4;
            }
        }

        private void txtScaleUnit_TextChanged(object sender, EventArgs e)
        {
            if (pScaleUnit < 0.001)
            {
                pScaleUnit = -pScaleUnit;
            }
            if (pViewHeight < pViewWidth && pScaleUnit > pViewHeight / 4)
            {
                pScaleUnit = pViewHeight / 4;
            }
            else if (pViewWidth <= pViewHeight && pScaleUnit > pViewWidth / 4)
            {
                pScaleUnit = pViewWidth / 4;
            }
        }

        private void txtFocusX_TextChanged(object sender, EventArgs e)
        {
            if (pFocusX < 0.001)
            {
                pFocusX = -pFocusX;
            }
            if (pFocusX > pViewWidth)
            {
                pFocusX = pViewWidth;
            }
        }

        private void txtFocusY_TextChanged(object sender, EventArgs e)
        {
            if (pFocusY < 0.001)
            {
                pFocusY = -pFocusY;
            }
            if (pFocusY > pViewHeight)
            {
                pFocusY = pViewHeight;
            }
        }

        private void txtSpotWidth_TextChanged(object sender, EventArgs e)
        {
            if (pSpotWidth < 0.001)
            {
                pSpotWidth = -pSpotWidth;
            }
            if (pSpotWidth > pViewWidth / 2)
            {
                pSpotWidth = pViewWidth / 2;
            }
        }

        private void txtSpotHeight_TextChanged(object sender, EventArgs e)
        {
            if (pSpotHeight < 0.001)
            {
                pSpotHeight = -pSpotHeight;
            }
            if (pSpotHeight > pViewHeight / 2)
            {
                pSpotHeight = pViewHeight / 2;
            }
        }
        #endregion

        #region 方法 

        private void RefreshUI()
        {
            //btnAccept.Text = Skyray.EDX.Common.Info.ButtonTextAccept;
            //btnCancel.Text = Skyray.EDX.Common.Info.ButtonTextCancel;
            //btnDefault.Text = Skyray.EDX.Common.Info.ButtonTextDefult;

            //gbxFocus.Text = Skyray.EDX.Common.Info.GroupBoxTextVideoParameterFocus;
            //gbxSpot.Text = Skyray.EDX.Common.Info.GroupBoxTextVideoParameterSpot;
            //gbxView.Text = Skyray.EDX.Common.Info.GroupBoxTextVideoParameterView;

            //lblFocusX.Text = Skyray.EDX.Common.Info.LabelTextVideoParameterFocusX;
            //lblFocusY.Text = Skyray.EDX.Common.Info.LabelTextVideoParameterFocusY;
            //lblSpotShape.Text = Skyray.EDX.Common.Info.LabelTextVideoParameterSpotShape;
            //lblFociWidth.Text = Skyray.EDX.Common.Info.LabelTextVideoParameterSpotWidth;
            //lblFociHeight.Text = Skyray.EDX.Common.Info.LabelTextVideoParameterSpotHeight;
            //lblScaleUnit.Text = Skyray.EDX.Common.Info.LabelTextVideoParameterRuleUnit;
            //lblViewHeight.Text = Skyray.EDX.Common.Info.LabelTextVideoParameterViewHeight;
            //lblViewWidth.Text = Skyray.EDX.Common.Info.LabelTextVideoParameterViewWidth;

            //rbtnSpotShapeRectangle.Text = Skyray.EDX.Common.Info.RatioButtonTextVideoParameterRectangle;
            //rbtnSpotShapeEllipse.Text = Skyray.EDX.Common.Info.RatioButtonTextVideoParameterEllipse;

            //Text = Skyray.EDX.Common.Info.FormTextVideoParameter;
        }

        #endregion

        /// <summary>
        /// 设置视频参数
        /// </summary>
        private void setCameraParam()
        {
            if (decimal.Parse(this.pScaleUnit.ToString()) == 0) return;
            for (int i = 0; i < camera.Focis.Length;i++ )
                camera.Focis[i].Shape = this.Shape;
            //camera.Focis[this.FociIndex].Shape = this.Shape;
            camera.Focis[this.FociIndex].Width = this.pSpotWidth;
            camera.Focis[this.FociIndex].Height = this.pSpotHeight;
            camera.Focis[this.FociIndex].FociX = this.pFocusX;
            camera.Focis[this.FociIndex].FociY = this.pFocusY;
            camera.RuleUnit = (float)this.pScaleUnit;
            camera.FociX = this.pFocusX;
            camera.FociY = this.pFocusY;
            camera.ViewWidth = this.pViewWidth;
            camera.ViewHeight = this.pViewHeight;
            switch (cboColor.Text)
            {
                case ("红"):
                {
                    SkyrayCamera.curColor = Color.Red;
                    ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/PenColor","1");

                    break;
                }
                case ("橙"):
                {
                    SkyrayCamera.curColor = Color.Orange;
                    ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/PenColor", "2");

                    break;
                }
                case ("黄"):
                {
                    SkyrayCamera.curColor = Color.Yellow;
                    ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/PenColor", "3");

                    break;
                }
                case ("绿"):
                {
                    SkyrayCamera.curColor = Color.Green;
                    ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/PenColor", "4");

                    break;
                }
                case ("青"):
                {
                    SkyrayCamera.curColor = Color.Cyan;
                    ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/PenColor", "5");

                    break;
                }
                case ("蓝"):
                {
                    SkyrayCamera.curColor = Color.Blue;
                    ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/PenColor", "6");

                    break;
                }
                case ("紫"):
                {
                    SkyrayCamera.curColor = Color.Purple;
                    ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/PenColor", "7");

                    break;
                }
                case ("黑"):
                {
                    SkyrayCamera.curColor = Color.Black;
                    ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/PenColor", "8");

                    break;
                }
                case ("白"):
                {
                    SkyrayCamera.curColor = Color.White;
                    ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/PenColor", "9");
                    break;
                }

            }
            camera.UpdateOverlay();
            //camera.Save();
        }
        /// <summary>
        /// 设置视频参数
        /// </summary>
        private void setCameraParam(object sender, EventArgs e)
        {
            setCameraParam();

            
        }

      
    }
}