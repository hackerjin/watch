using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDXRFLibrary.Define;
using Lephone.Data.Common;
using Skyray.EDX.Common;

namespace Skyray.UC
{
    public partial class UCInputPWD : Skyray.Language.MultipleForm
    {
        private PWDLock CurrentPWD;

        private bool Flag = false;
        /// <summary>
        /// 对话结果
        /// </summary>
     //   public DialogResult DialogResult { get; set; }
        string pwd = "skyray";
        public UCInputPWD()
        {
            InitializeComponent();
           
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            this.DialogResult = DialogResult.Cancel;

           // EDXRFHelper.GotoMainPage(this);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            if (this.txtPWDLock.Text == pwd)
            {
                this.DialogResult = DialogResult.Yes;
            }
            else
            {
                Msg.Show(Info.PWDError);
                return;
                //this.DialogResult = DialogResult.No;
            }

            //EDXRFHelper.GotoMainPage(this);
        }

        private void UCPWDLock_Load(object sender, EventArgs e)
        {
           
        }
    }
}
