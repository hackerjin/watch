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
    public partial class UCPWDLock : Skyray.Language.UCMultiple
    {
        private PWDLock CurrentPWD;

        private bool Flag = false;
        /// <summary>
        /// 对话结果
        /// </summary>
        public DialogResult DialogResult { get; set; }

        public UCPWDLock(bool flag)
        {
            InitializeComponent();
            Flag = flag;
            this.DialogResult = DialogResult.No;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (Flag)
            {
                this.DialogResult = DialogResult.No;
            }
            EDXRFHelper.GotoMainPage(this);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!Flag)
            {
                CurrentPWD.Password = this.txtPWDLock.Text;
                CurrentPWD.Save();
            }
            else
            {
                if (CurrentPWD.Password == this.txtPWDLock.Text)
                {
                    this.DialogResult = DialogResult.Yes;
                }
                else
                {
                    Msg.Show(Info.PWDError);
                    return;
                    //this.DialogResult = DialogResult.No;
                }
            }
            EDXRFHelper.GotoMainPage(this);
        }

        private void UCPWDLock_Load(object sender, EventArgs e)
        {
            DbObjectList<PWDLock> lst = PWDLock.FindAll();
            if (lst.Count == 0)
            {
                CurrentPWD = PWDLock.New.Init("skyray");
                CurrentPWD.Save();
            }
            else
                CurrentPWD = lst[0];
            if (!Flag)
            {
                this.txtPWDLock.Text = CurrentPWD.Password;
            }
        }
    }
}
