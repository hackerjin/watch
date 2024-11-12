using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.UC
{
    public partial class UCCreateTitle : Skyray.Language.UCMultiple
    {
        public UCCreateTitle()
        {
            InitializeComponent();
        }

        private void btnSelectICO_Click(object sender, EventArgs e)
        {
            this.ofdIco.Filter = "(*.ico)|*.ico";
            if (DialogResult.OK == this.ofdIco.ShowDialog())
            {
                try
                {
                    string ico = this.ofdIco.FileName;
                    this.txtICO.Text = ico;
                    this.pbICO.Image = Image.FromFile(ico);
                }
                catch
                {
                    this.txtICO.Text = "";
                    MessageBox.Show(Info.SelectICOFile);
                }
            }
        }

        public override void ExcuteEndProcess(params object[] objs)
        {
            DifferenceDevice.TitleIco.Text = this.txtTitle.Text;

            if (this.ofdIco.FileName.IsNullOrEmpty())
                return;
            Bitmap myBitmap = new Bitmap(this.ofdIco.FileName);

            this.CreateGraphics().DrawImage(myBitmap, 0, 0);

            IntPtr Hicon = myBitmap.GetHicon();

            Icon newIcon = Icon.FromHandle(Hicon);

            DifferenceDevice.TitleIco.Ico = newIcon;

            //DestroyIcon(newIcon.Handle);

            SerializeHelper.ObjToFile(DifferenceDevice.TitleIco, Application.StartupPath + "\\TitleIco");
            DifferenceDevice.MediumAccess.UpdateTitleICO();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);//回到主界面
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.ParentForm != null)
                this.ParentForm.DialogResult = this.dialogResult = DialogResult.OK;
            EDXRFHelper.GotoMainPage(this);//回到主界面
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr handle);

    }
}
