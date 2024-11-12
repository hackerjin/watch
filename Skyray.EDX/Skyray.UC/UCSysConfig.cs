using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary;
using System.Collections;
using System.Xml;
using Skyray.EDX.Common.UIHelper;

namespace Skyray.UC
{
    public partial class UCSysConfig : Skyray.Language.UCMultiple
    {
        private Device currentDevice;

        private SysConfig sysConfig;

        public UCSysConfig()
        {
            InitializeComponent();
            currentDevice = Device.FindById(WorkCurveHelper.DeviceCurrent.Id);
            sysConfig = currentDevice.SysConfig;
            InitCbo();
        }

        private void UCSysConfig_Load(object sender, EventArgs e)
        {
            if (sysConfig == null)
            {
                sysConfig = SysConfig.New.Init(false);
                sysConfig.Device = currentDevice;
                sysConfig.Save();
            }
            BindingData();
            if (DifferenceDevice.IsXRF)
                GetContext("FormXRF");
            else if (DifferenceDevice.IsRohs)
                GetContext("FormRohs");
            else if (DifferenceDevice.IsThick)
                GetContext("FrmThick");
            else
                GetContext("Frm3000");
        }

        private void InitCbo()
        {
            System.Drawing.Text.InstalledFontCollection myFonts = new System.Drawing.Text.InstalledFontCollection();
            List<string> strList = new List<string>();
            foreach (FontFamily currFont in myFonts.Families)
            {
                comFontType.Items.Add(currFont.GetName(0));
                strList.Add(currFont.GetName(0));
            }
            comFontType.SelectedIndex = strList.IndexOf("宋体");
            ArrayList list = new ArrayList();
            foreach (int i in Enum.GetValues(typeof(System.Drawing.FontStyle)))
            {
                listBox1.Items.Add(Enum.GetName(typeof(System.Drawing.FontStyle), i));
            }
            listBox1.SelectedIndex = 0;
            this.numericUpDown1.Value = 12;
            this.numericUpDown2.Value = 20;
        }

        private void BindingData()
        {
            if (sysConfig != null)
            {
                BindHelper.BindCheckedToCtrl(radHasSound, sysConfig, "IsTipSound");
                if (!radHasSound.Checked) radNoSound.Checked = true;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            sysConfig.Save();
            currentDevice.Save();
            WorkCurveHelper.DeviceCurrent = currentDevice;
            if (DifferenceDevice.IsXRF)
                SaveContent("FormXRF");
            else if (DifferenceDevice.IsRohs)
                SaveContent("FormRohs");
            else if (DifferenceDevice.IsThick)
                SaveContent("FrmThick");
            else
                SaveContent("Frm3000");
            if (this.ParentForm != null)
                this.ParentForm.DialogResult = this.dialogResult = DialogResult.OK;
            EDXRFHelper.GotoMainPage(this);
        }

        private void SaveContent(string applicationName)
        {
            string strFilePath = AppDomain.CurrentDomain.BaseDirectory + "UI.xml";
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(strFilePath);
            xmldoc.SelectSingleNode("UI/"+applicationName+"/Font").InnerText = this.comFontType.SelectedItem.ToString();
            xmldoc.SelectSingleNode("UI/"+applicationName+"/FontStyle").InnerText = this.txtFontStyle.Text;
            xmldoc.SelectSingleNode("UI/"+applicationName+"/FontSize").InnerText = this.numericUpDown1.Value.ToString();
            xmldoc.SelectSingleNode("UI/"+applicationName+"/ImageSize").InnerText = this.numericUpDown2.Value.ToString();
            xmldoc.Save(strFilePath);
        }

        private void GetContext(string applicationName)
        {
            string strFilePath = AppDomain.CurrentDomain.BaseDirectory + "UI.xml";
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(strFilePath);
            this.comFontType.Text = xmldoc.SelectSingleNode("UI/" + applicationName + "/Font").InnerText;
            this.txtFontStyle.Text = xmldoc.SelectSingleNode("UI/" + applicationName + "/FontStyle").InnerText;
            this.numericUpDown1.Value = string.IsNullOrEmpty(xmldoc.SelectSingleNode("UI/" + applicationName + "/FontSize").InnerText) ? 8 : decimal.Parse(xmldoc.SelectSingleNode("UI/" + applicationName + "/FontSize").InnerText);
            this.numericUpDown2.Value = string.IsNullOrEmpty(xmldoc.SelectSingleNode("UI/" + applicationName + "/ImageSize").InnerText) ? 20 : decimal.Parse(xmldoc.SelectSingleNode("UI/" + applicationName + "/ImageSize").InnerText);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }

        public override void ExcuteEndProcess(params object[] objs)
        {
            //Console.WriteLine("sfsdf");
        }

        private void cbo_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            //MessageBox.Show(energyKey.KeyModifiers.ToString());
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtFontStyle.Text = this.listBox1.SelectedItem.ToString();
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            this.comFontType.Text = "宋体";
            this.txtFontStyle.Text = "Regular";
            this.numericUpDown1.Value = 9;
            if (DifferenceDevice.IsXRF || DifferenceDevice.IsRohs || DifferenceDevice.IsThick)
                this.numericUpDown2.Value = 20;
            else
                this.numericUpDown2.Value = 35;
        }
    }
}
