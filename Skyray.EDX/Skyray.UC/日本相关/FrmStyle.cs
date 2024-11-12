using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common.Component;
using Skyray.EDX.Common;
using System.Xml;

namespace Skyray.UC
{
    public partial class FrmStyle : Skyray.Language.UCMultiple
    {
        public FrmStyle()
        {
            InitializeComponent();
            LoadStyleParams();
            BindToCtr();
        }

        #region yuzhao:数据绑定
        void BindToCtr()
        {
            BindHelper.BindToCtrl(this.lblOkForeColor, "BackColor", WorkCurveHelper.OKStyle, "ForeColor", true);//控件与数据源绑定
            BindHelper.BindToCtrl(this.lblOK, "ForeColor", WorkCurveHelper.OKStyle, "ForeColor", true);//控件与数据源绑定
            BindHelper.BindToCtrl(this.panelOK, "BackColor", WorkCurveHelper.OKStyle, "BackColor", true);//控件与数据源绑定
            BindHelper.BindToCtrl(this.lblOkBackColor, "BackColor", WorkCurveHelper.OKStyle, "BackColor", true);//控件与数据源绑定
            BindHelper.BindValueToCtrl(this.numOKFontSize, WorkCurveHelper.OKStyle, "Style.Size", true);
            BindHelper.BindToCtrl(this.lblOK, "Font", WorkCurveHelper.OKStyle, "Style", true);

            BindHelper.BindToCtrl(this.lblNGForeColor, "BackColor", WorkCurveHelper.NGStyle, "ForeColor", true);//控件与数据源绑定
            BindHelper.BindToCtrl(this.lblNG, "ForeColor", WorkCurveHelper.NGStyle, "ForeColor", true);//控件与数据源绑定
            BindHelper.BindToCtrl(this.panelNG, "BackColor", WorkCurveHelper.NGStyle, "BackColor", true);//控件与数据源绑定
            BindHelper.BindToCtrl(this.lblNGBackColor, "BackColor", WorkCurveHelper.NGStyle, "BackColor", true);//控件与数据源绑定
            BindHelper.BindValueToCtrl(this.numNGFontSize, WorkCurveHelper.NGStyle, "Style.Size", true);
            BindHelper.BindToCtrl(this.lblNG, "Font", WorkCurveHelper.NGStyle, "Style", true);
        }
        #endregion

        #region yuzhaozhu:参数读取与保存
        private void LoadStyleParams()
        {
            XmlNode node = ReportTemplateHelper.LoadSpecifiedNode("JapanStyle", null);
            if (node != null)
            {
                foreach (XmlNode xn in node.ChildNodes)
                {
                    if (xn.Name == "OK")
                        WorkCurveHelper.OKStyle = JapanStyle.ParseFromString(xn.InnerText);
                    else if (xn.Name == "NG")
                        WorkCurveHelper.NGStyle = JapanStyle.ParseFromString(xn.InnerText);
                }
            }
            else
            {
                WorkCurveHelper.OKStyle  = JapanStyle.Default();
                WorkCurveHelper.NGStyle = JapanStyle.Default();
            }
        }
        private void SaveStyleParams()
        {
            ReportTemplateHelper.SaveSpecifiedValueandCreate("JapanStyle", "OK", WorkCurveHelper.OKStyle.ParseToString());
            ReportTemplateHelper.SaveSpecifiedValueandCreate("JapanStyle", "NG", WorkCurveHelper.NGStyle.ParseToString());
        }
        #endregion

        #region yuzhao:OK相关设置
        private void lblOkForeColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                WorkCurveHelper.OKStyle.ForeColor = cd.Color;
            }

        }

        private void lblOkBackColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                WorkCurveHelper.OKStyle.BackColor = cd.Color;
            }
        }

        void numOKFontSize_ValueChanged(object sender, System.EventArgs e)
        {
            WorkCurveHelper.OKStyle.Style = new Font("宋体", (float)numOKFontSize.Value, FontStyle.Bold);
        }
        #endregion

        #region yuzhao:NG相关设置
        private void lblNGForeColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                WorkCurveHelper.NGStyle.ForeColor = cd.Color;
            }
        }

        private void lblNGBackColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                WorkCurveHelper.NGStyle.BackColor = cd.Color;
            }
        }
        void numNGFontSize_ValueChanged(object sender, System.EventArgs e)
        {
             WorkCurveHelper.NGStyle.Style = new Font("宋体", (float)numNGFontSize.Value, FontStyle.Bold);
        }
        #endregion

        #region yzuhao:Button Events

        private void btnApply_Click(object sender, EventArgs e)
        {
            SaveStyleParams();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            SaveStyleParams();
            EDXRFHelper.GotoMainPage(this);
        }

        #endregion
    }
}
