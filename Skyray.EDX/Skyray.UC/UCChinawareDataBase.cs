using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Skyray.UC
{
    public partial class UCChinawareDataBase : Skyray.Language.UCMultiple
    {

        public string ChinawareFileName
        {
            get { return this.txtFileName.Text; }
            set { this.txtFileName.Text = value; }
        }

        public int MaxCount
        {
            get { return Convert.ToInt32(txtCount.Text); }
            set { this.txtCount.Text = value.ToString(); }
        }

        public double StumerValue 
        {
            get { return Convert.ToDouble(txtStumer.Text); }
            set { this.txtStumer.Text = value.ToString();}
        }

        public UCChinawareDataBase()
        {
            InitializeComponent();
            this.txtStumer.Text = DifferenceDevice.StumerValue.ToString();
            this.txtCount.Text = DifferenceDevice.MaxCount.ToString();
            this.txtFileName.Text = DifferenceDevice.ChinawareFileName;
        }

        private void btnChinawareDataBase_Click(object sender, EventArgs e)
        {
            Chinaware.StandardLibrary.Close();
            openFileDialog1.Filter = "*.cdb|*.cdb";
            if(openFileDialog1.ShowDialog()== DialogResult.OK)
            {
                ChinawareFileName = System.IO.Path.GetFullPath(openFileDialog1.FileName);
                string strPath = openFileDialog1.FileName;
                //openFileDialog1.Dispose();
                if (!strPath.Equals(AppDomain.CurrentDomain.BaseDirectory + System.IO.Path.GetFileName(openFileDialog1.FileName)))
                {
                    System.IO.File.Copy(strPath, AppDomain.CurrentDomain.BaseDirectory + System.IO.Path.GetFileName(ChinawareFileName), true);
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DifferenceDevice.ChinawareFileName = ChinawareFileName;
            DifferenceDevice.MaxCount = MaxCount;
            DifferenceDevice.StumerValue = StumerValue;
            Chinaware.StandardLibrary.Open(DifferenceDevice.ChinawareFileName);
            Chinaware.StandardLibrary.Close();
            try
            {
                Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedValue("ChinawareDataBase", "Path", ChinawareFileName.Contains(AppDomain.CurrentDomain.BaseDirectory) ? ChinawareFileName : AppDomain.CurrentDomain.BaseDirectory + ChinawareFileName);
                Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedValue("ChinawareDataBase", "IsUsed", this.chKChinaware.Checked.ToString());
                //DifferenceDevice.ReportChinaWare = this.chKChinaware.Checked;
            }
            catch (Exception ex)
            {
#if DEBUG
                Skyray.Controls.SkyrayMsgBox.Show(ex.ToString());
#endif
            }
            btnCancel_Click(sender, e);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.ParentForm != null)
                this.ParentForm.Close();
        }

        private void chKChinaware_CheckedChanged(object sender, EventArgs e)
        {
            if (chKChinaware.Checked && Chinaware.StandardLibrary.ElementList != null)
                Chinaware.StandardLibrary.ElementList = null;
        }

        private void UCChinawareDataBase_Load(object sender, EventArgs e)
        {
            try
            {
                this.txtFileName.Text = Skyray.EDX.Common.ReportTemplateHelper.LoadSpecifiedValue("ChinawareDataBase", "Path");
                this.chKChinaware.Checked = Convert.ToBoolean(Skyray.EDX.Common.ReportTemplateHelper.LoadSpecifiedValue("ChinawareDataBase", "IsUsed"));
            }
            catch (Exception ex)
            {
#if DEBUG
                Skyray.Controls.SkyrayMsgBox.Show(ex.ToString());
#endif
                 
            }
        }
    }
}
