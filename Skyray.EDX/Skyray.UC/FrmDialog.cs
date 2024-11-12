using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.API;

namespace Skyray.UC
{
    public partial class FrmDialog : Skyray.Language.MultipleForm
    {
        public event EventDelegate.EventHandleDialog OnSubmit;
        public bool flag;
        private string strAlter = string.Empty;
        private string strTitle = string.Empty;

        public FrmDialog()
        {
            InitializeComponent();
           
        }

        public FrmDialog(string title, string context, bool flag, MessageBoxIcon icon)
            :this()
        {
            strTitle = title;
            strAlter = context;
            this.flag = flag;
            if (icon != MessageBoxIcon.None)
            {
                this.vPicBox.Visible = true;
                this.vPicBox.Image = this.DrawIcon(icon);
            }
            else
            {
                this.vPicBox.Image = null;
                this.vPicBox.Visible = false;
            }
        }

        private void buttonWSubmit_Click(object sender, EventArgs e)
        {
            if (OnSubmit != null)
                OnSubmit(this.flag);
            this.Close();
        }

        private void buttonWCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private Image DrawIcon(MessageBoxIcon icon)
        {
            Icon asterisk = null;
            if (icon == MessageBoxIcon.Asterisk)
                asterisk = SystemIcons.Asterisk;
            else if ((icon == MessageBoxIcon.Hand) || (icon == MessageBoxIcon.Hand))
                asterisk = SystemIcons.Error;
            else if (icon == MessageBoxIcon.Exclamation)
                asterisk = SystemIcons.Exclamation;
            else if (icon == MessageBoxIcon.Hand)
                asterisk = SystemIcons.Hand;
            else if (icon == MessageBoxIcon.Asterisk)
                asterisk = SystemIcons.Information;
            else if (icon == MessageBoxIcon.Question)
                asterisk = SystemIcons.Question;
            else if (icon == MessageBoxIcon.Exclamation)
                asterisk = SystemIcons.Warning;
            Bitmap image = new Bitmap(asterisk.Width, asterisk.Height);
            image.MakeTransparent();
            using (Graphics graphics = Graphics.FromImage(image))
            {
                if (((Environment.Version.Build <= 0xe79) && (Environment.Version.Revision == 0x120)) && ((Environment.Version.Major == 1) && (Environment.Version.Minor == 0)))
                {
                    IntPtr hdc = graphics.GetHdc();
                    try
                    {
                        WinMethod.DrawIconEx(hdc, 0, 0, asterisk.Handle, asterisk.Width, asterisk.Height, 0, IntPtr.Zero, 3);
                        return image;
                    }
                    finally
                    {
                        graphics.ReleaseHdc(hdc);
                    }
                }
                if (asterisk.Handle == IntPtr.Zero)
                    return image;
                try
                {
                    graphics.DrawIcon(asterisk, 0, 0);
                    return image;
                }
                catch
                {
                    return image;
                }
            }

        }

        private void FrmDialog_Load(object sender, EventArgs e)
        {
          
        }

        private void FrmDialog_Shown(object sender, EventArgs e)
        {
            lblAlertInfo.Text = strAlter;
            this.Text = strTitle;
        }
    }
}
