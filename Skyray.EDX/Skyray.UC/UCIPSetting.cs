using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Skyray.EDX.Common;
using System.Net;

namespace Skyray.UC
{
    public partial class UCIPSetting : Skyray.Language.UCMultiple
    {
        public UCIPSetting()
        {
            InitializeComponent();
            this.txtSubnet.clickFocus += new KeyEventHandler(txtSubNet_clickFocus);
            if (WorkCurveHelper.DeviceCurrent !=null &&WorkCurveHelper.DeviceCurrent.FPGAParams !=null)
                this.txtIP.Text = WorkCurveHelper.DeviceCurrent.FPGAParams.IP;
            this.txtSubnet.Text = "255.255.255.0";
            this.txtGateWay.Text = "192.168.2.1";
            this.txtDNS.Text = "192.168.1.2";
        }

        void txtSubNet_clickFocus(object sender, KeyEventArgs e)
        {
            if (!JudgeSite(txtIP.Text) && JudgeSite(lblSubNet.Text))
            {
                int currentIp=Convert.ToInt32(txtIP.Text.Split('.')[0]);
                if (currentIp >= 1 && currentIp < 127)
                    txtSubnet.Text = "255.0.0.0";
                if (currentIp>127&&currentIp<=191)
                    txtSubnet.Text = "255.255.0.0";
                if (currentIp>191&&currentIp<=223)
                    txtSubnet.Text = "255.255.255.0";
                if (currentIp>223&&currentIp<=255)
                    txtSubnet.Text = "255.255.255.255";
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.ParentForm != null)
                this.ParentForm.DialogResult = this.dialogResult = DialogResult.OK;
            EDXRFHelper.GotoMainPage(this);
        }

        public override void ExcuteEndProcess(params object[] objs)
        {
            if (JudgeSite(txtIP.Text) || JudgeSite(txtDNS.Text) || JudgeSite(txtGateWay.Text) || JudgeSite(txtSubnet.Text))
            {
                MessageBox.Show(Info.strIpAddress);
                return;
            }
            bool b = DifferenceDevice.MediumAccess.IPSettings(this.txtIP.Text, this.txtSubnet.Text, this.txtGateWay.Text, this.txtDNS.Text);
            if (b)
            {
                WorkCurveHelper.DeviceCurrent.FPGAParams.IP = this.txtIP.Text;
                WorkCurveHelper.DeviceCurrent.Save();
            }
        }

        private void buttonW2_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);//返回主界面
        }

        #region 验证输入内容
        private bool JudgeSite(string sContent)
        {
            bool IsSucceed = false;
            string[] lst = sContent.Split('.');
            for (int i = 0; i < lst.Length; i++)
                if (lst[i].Length <= 0)
                    return true;
            return IsSucceed;
        }

        #endregion
    }
}
