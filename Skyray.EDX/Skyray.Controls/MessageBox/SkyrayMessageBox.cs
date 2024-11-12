using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Skyray.API;

namespace Skyray.Controls
{
    // 内部自定义的消息框窗体，用于替代Windows提供的标准消息框
    public sealed class MessageWindow : Form
    {
        private PictureBox vPicBox;
        private Label lblAlertInfo;
        private bool vBool1 = true;
        private bool vBool2 = true;
        private bool vBool3 = true;
        private MessageBoxButtons msgBoxButtons;
        private ButtonW vbutton1;
        private ButtonW vbutton2;
        private ButtonW vbutton3;
        private IContainer vContainer1 = null;

        private DialogResult res = DialogResult.None;
        private IContainer components;
        private MyLabel myLabel1;

        private bool _bMultiply = false;

        public DialogResult Res
        {
            get { return res; }
            //set { res = value; }
        }


        public static bool PlaySound = true;

        public MessageWindow()
        {
            this.InitializeComponent();
            this.TopMost = true;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.vPicBox = new System.Windows.Forms.PictureBox();
            this.lblAlertInfo = new System.Windows.Forms.Label();
            this.myLabel1 = new Skyray.Controls.MyLabel(this.components);
            this.vbutton3 = new Skyray.Controls.ButtonW();
            this.vbutton2 = new Skyray.Controls.ButtonW();
            this.vbutton1 = new Skyray.Controls.ButtonW();
            ((System.ComponentModel.ISupportInitialize)(this.vPicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // vPicBox
            // 
            this.vPicBox.BackColor = System.Drawing.Color.Transparent;
            this.vPicBox.Location = new System.Drawing.Point(23, 21);
            this.vPicBox.Name = "vPicBox";
            this.vPicBox.Size = new System.Drawing.Size(37, 34);
            this.vPicBox.TabIndex = 3;
            this.vPicBox.TabStop = false;
            // 
            // lblAlertInfo
            // 
            this.lblAlertInfo.AutoSize = true;
            this.lblAlertInfo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblAlertInfo.Location = new System.Drawing.Point(72, 32);
            this.lblAlertInfo.MaximumSize = new System.Drawing.Size(371, 178);
            this.lblAlertInfo.Name = "lblAlertInfo";
            this.lblAlertInfo.Size = new System.Drawing.Size(42, 14);
            this.lblAlertInfo.TabIndex = 4;
            this.lblAlertInfo.Text = "Test!";
            // 
            // myLabel1
            // 
            this.myLabel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.myLabel1.LineDistance = 5;
            this.myLabel1.Location = new System.Drawing.Point(72, 32);
            this.myLabel1.MaximumSize = new System.Drawing.Size(371, 178);
            this.myLabel1.Name = "myLabel1";
            this.myLabel1.Size = new System.Drawing.Size(41, 13);
            this.myLabel1.TabIndex = 5;
            this.myLabel1.Text = "Test!";
            // 
            // vbutton3
            // 
            this.vbutton3.bSilver = false;
            this.vbutton3.Location = new System.Drawing.Point(239, 0);
            this.vbutton3.MaxImageSize = new System.Drawing.Point(0, 0);
            this.vbutton3.MenuPos = new System.Drawing.Point(0, 0);
            this.vbutton3.Name = "vbutton3";
            this.vbutton3.Size = new System.Drawing.Size(87, 24);
            this.vbutton3.Style = Skyray.Controls.Style.Office2007Blue;
            this.vbutton3.TabIndex = 3;
            this.vbutton3.Text = "Ignore";
            this.vbutton3.ToFocused = false;
            this.vbutton3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // vbutton2
            // 
            this.vbutton2.bSilver = false;
            this.vbutton2.Location = new System.Drawing.Point(118, -1);
            this.vbutton2.MaxImageSize = new System.Drawing.Point(0, 0);
            this.vbutton2.MenuPos = new System.Drawing.Point(0, 0);
            this.vbutton2.Name = "vbutton2";
            this.vbutton2.Size = new System.Drawing.Size(87, 24);
            this.vbutton2.Style = Skyray.Controls.Style.Office2007Blue;
            this.vbutton2.TabIndex = 2;
            this.vbutton2.Text = "Cancel";
            this.vbutton2.ToFocused = false;
            this.vbutton2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // vbutton1
            // 
            this.vbutton1.bSilver = false;
            this.vbutton1.Location = new System.Drawing.Point(0, 0);
            this.vbutton1.MaxImageSize = new System.Drawing.Point(0, 0);
            this.vbutton1.MenuPos = new System.Drawing.Point(0, 0);
            this.vbutton1.Name = "vbutton1";
            this.vbutton1.Size = new System.Drawing.Size(87, 24);
            this.vbutton1.Style = Skyray.Controls.Style.Office2007Blue;
            this.vbutton1.TabIndex = 1;
            this.vbutton1.Text = "Ok";
            this.vbutton1.ToFocused = false;
            this.vbutton1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // MessageWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(419, 135);
            this.Controls.Add(this.myLabel1);
            this.Controls.Add(this.vbutton3);
            this.Controls.Add(this.vbutton2);
            this.Controls.Add(this.vPicBox);
            this.Controls.Add(this.vbutton1);
            this.Controls.Add(this.lblAlertInfo);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("SimSun", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(582, 323);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(139, 84);
            this.Name = "MessageWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)(this.vPicBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public DialogResult DrawWindow(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultbutton)
        {
            return DrawWindow(owner, text, caption, buttons, icon, defaultbutton, true);
        }

        private void SafeCall(Control con, Action act)
        {
            if (con == null || act == null)
                return;
            if (con.InvokeRequired)
            {
                con.Invoke(act);
                return;
            }
            act();
        }

        public DialogResult DrawWindow(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultbutton, bool IsModelDialog)
        {
            
            this.msgBoxButtons = buttons;
            this.Text = (caption != String.Empty) ? caption : " ";
            if (text.IndexOf('\r') >0 && text.IndexOf('\n') > 0)
            {
                SafeCall(this.myLabel1, () => 
                {
                    this.myLabel1.Visible = true;
                    this.lblAlertInfo.Visible = false;
                    this.myLabel1.Text = text;
                    _bMultiply = true;
                });
                
            }
            else
            {
                SafeCall(this.myLabel1, () => 
                {
                    this.lblAlertInfo.Visible = true;
                    this.myLabel1.Visible = false;
                    this.lblAlertInfo.Text = text;
                    _bMultiply = false;
                });
            }

            #region Pic
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
            #endregion

            #region CancelButton

            if (buttons == MessageBoxButtons.OK)
            {
                this.AcceptButton = this.vbutton1;
                this.CancelButton = this.vbutton1;
            }
            else if (((buttons == MessageBoxButtons.OKCancel) || (buttons == MessageBoxButtons.RetryCancel)) || (buttons == MessageBoxButtons.YesNo))
            {
                base.AcceptButton = this.vbutton1;
                base.CancelButton = this.vbutton2;
            }
            else if (buttons == MessageBoxButtons.YesNoCancel)
            {
                base.AcceptButton = this.vbutton1;
                base.CancelButton = this.vbutton3;
            }

            #endregion

            this.SetButtonText(buttons);

            if ((defaultbutton == MessageBoxDefaultButton.Button1) && vbutton1.Visible)
                this.vbutton1.Select(); 
            else if ((defaultbutton == MessageBoxDefaultButton.Button2) && vbutton2.Visible)
                this.vbutton2.Select();
            else if ((defaultbutton == MessageBoxDefaultButton.Button3) && vbutton3.Visible)
                this.vbutton3.Select();

            SetLocationAndSize();//设置窗体大小及按钮位置

            #region Play Sound

            if (PlaySound)
            {
                MethodEx.PlaySoundByIcon(icon);//根据不同的提示框类型，播放不同的声音
            }

            #endregion

            if (IsModelDialog)
            {
                return base.Visible ? base.DialogResult : base.ShowDialog(owner);
            }
            else
            {
                base.Show(owner);
                return base.DialogResult;
            }
            
        }

        private void SetButtonText(MessageBoxButtons buttons)
        {
            if (buttons == MessageBoxButtons.AbortRetryIgnore)
            {
                this.vbutton1.Text = CommonsInfo.MessageBoxButtonsAbort;
                this.vbutton2.Text = CommonsInfo.MessageBoxButtonsRetry;
                this.vbutton3.Text = CommonsInfo.MessageBoxButtonsIgnore;
                this.vbutton1.Visible = vBool1 = true;
                this.vbutton2.Visible = vBool2 = true;
                this.vbutton3.Visible = vBool3 = true;
            }
            else if (buttons == MessageBoxButtons.OK)
            {
                this.vbutton1.Text = CommonsInfo.MessageBoxButtonsOK;
                this.vbutton1.Visible = vBool1 = true;
                this.vbutton2.Visible = vBool2 = false;
                this.vbutton3.Visible = vBool3 = false;
            }
            else if (buttons == MessageBoxButtons.OKCancel)
            {
                this.vbutton1.Text = CommonsInfo.MessageBoxButtonsOK;
                this.vbutton2.Text = CommonsInfo.MessageBoxButtonsCancel;
                this.vbutton1.Visible = vBool1 = true;
                this.vbutton2.Visible = vBool2 = true;
                this.vbutton3.Visible = vBool3 = false;
            }
            else if (buttons == MessageBoxButtons.RetryCancel)
            {
                this.vbutton1.Text = CommonsInfo.MessageBoxButtonsRetry;
                this.vbutton2.Text = CommonsInfo.MessageBoxButtonsCancel;
                this.vbutton1.Visible = vBool1 = true;
                this.vbutton2.Visible = vBool2 = true;
                this.vbutton3.Visible = vBool3 = false;
            }
            else if (buttons == MessageBoxButtons.YesNo)
            {
                this.vbutton1.Text = CommonsInfo.MessageBoxButtonsYes;
                this.vbutton2.Text = CommonsInfo.MessageBoxButtonsNo;
                this.vbutton1.Visible = vBool1 = true;
                this.vbutton2.Visible = vBool2 = true;
                this.vbutton3.Visible = vBool3 = false;
            }
            else if (buttons == MessageBoxButtons.YesNoCancel)
            {
                this.vbutton1.Text = CommonsInfo.MessageBoxButtonsYes;
                this.vbutton2.Text = CommonsInfo.MessageBoxButtonsNo;
                this.vbutton3.Text = CommonsInfo.MessageBoxButtonsCancel;
                this.vbutton1.Visible = vBool1 = true;
                this.vbutton2.Visible = vBool2 = true;
                this.vbutton3.Visible = vBool3 = true;
            }
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

        public void SetLocationAndSize()
        {
            //设置label 的大小
            if (_bMultiply)
                this.myLabel1.RefreshSize();
            int labelWith = _bMultiply ? this.myLabel1.Width : this.lblAlertInfo.Width;
            int labelHeight = _bMultiply ? this.myLabel1.Height : this.lblAlertInfo.Height;
            //本身高度
            //this.Height = this.lblAlertInfo.Height > 32 ? 20 + this.lblAlertInfo.Height + 50 + 24 + 20 : 20 + 32 + 45 + 24 + 20;
            this.Height = labelHeight > 32 ? 20 + labelHeight + 50 + 24 + 20 : 20 + 32 + 45 + 24 + 20;
            //没有图标
            if (this.vPicBox.Image != null)
            {
                //this.Width = 30 + 32 + 10 + this.lblAlertInfo.Width + 30;
                //this.lblAlertInfo.Location = new Point(62, this.lblAlertInfo.Location.Y);
                this.Width = 30 + 32 + 10 + labelWith+ 30;
                if (_bMultiply) this.myLabel1.Location = new Point(62, this.myLabel1.Location.Y);
                else this.lblAlertInfo.Location = new Point(62, this.lblAlertInfo.Location.Y);
            }
            else//有图标
            {
                //this.lblAlertInfo.Location = new Point(20, this.lblAlertInfo.Location.Y);
                //this.Width = 30 + this.lblAlertInfo.Width + 30;
                this.Width = 30 + labelWith + 30;
                if (_bMultiply) this.myLabel1.Location = new Point(20, this.myLabel1.Location.Y);
                else this.lblAlertInfo.Location = new Point(20, this.lblAlertInfo.Location.Y);
            }
            //一个按钮
            if (vBool1 && !vBool2 && !vBool3)
            {
                if (this.Width < 45 + 77 + 45)
                { this.Width = 45 + 77 + 45; }

                //this.vbutton1.Location = new Point(this.Width - this.vbutton1.Width - 18, this.Height - this.vbutton1.Height - 18 - SystemInformation.CaptionHeight);
                this.vbutton1.Location = new Point((this.Width - 77) / 2, this.Height - this.vbutton1.Height - 25 - SystemInformation.CaptionHeight);
            }
            //两个按钮
            if (vBool1 && vBool2 && !vBool3)
            {
                if (this.Width < 45 + 77 + 8 + 77 + 45)
                { this.Width = 45 + 77 + 8 + 77 + 45; }
                this.vbutton2.Location = new Point((this.Width - 77 * 2 - 8) / 2 + 77 + 8, this.Height - this.vbutton2.Height - 25 - SystemInformation.CaptionHeight);
                this.vbutton1.Location = new Point(this.vbutton2.Location.X - this.vbutton1.Width - 8, this.vbutton2.Location.Y);
                //this.vbutton2.Location = new Point(this.Width - this.vbutton2.Width - 18, this.Height - this.vbutton2.Height - 18 - SystemInformation.CaptionHeight);
                //this.vbutton1.Location = new Point(this.vbutton2.Location.X - this.vbutton1.Width - 8, this.vbutton2.Location.Y);
            }
            //三个按钮
            if (vBool1 && vBool2 && vBool3)
            {
                if (this.Width < 45 + 77 + 8 + 77 + 8 + 77 + 45)
                { this.Width = 45 + 77 + 8 + 77 + 8 + 77 + 45; }

                this.vbutton3.Location = new Point(this.Width - this.vbutton3.Width - 18, this.Height - this.vbutton3.Height - 25 - SystemInformation.CaptionHeight);
                this.vbutton2.Location = new Point(this.vbutton3.Location.X - this.vbutton2.Width - 8, this.vbutton3.Location.Y);
                this.vbutton1.Location = new Point(this.vbutton2.Location.X - this.vbutton1.Width - 8, this.vbutton3.Location.Y);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            DialogResult oK = DialogResult.OK;
            if ((this.msgBoxButtons == MessageBoxButtons.OK) || (this.msgBoxButtons == MessageBoxButtons.OKCancel))
            {
                oK = DialogResult.OK;
            }
            else if ((this.msgBoxButtons == MessageBoxButtons.YesNo) || (this.msgBoxButtons == MessageBoxButtons.YesNoCancel))
            {
                oK = DialogResult.Yes;
            }
            else if (this.msgBoxButtons == MessageBoxButtons.AbortRetryIgnore)
            {
                oK = DialogResult.Abort;
            }
            else if (this.msgBoxButtons == MessageBoxButtons.RetryCancel)
            {
                oK = DialogResult.Retry;
            }
            res = base.DialogResult = oK;

           // this.Visible = false;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            DialogResult cancel = DialogResult.Cancel;
            if (this.msgBoxButtons == MessageBoxButtons.OKCancel)
            {
                cancel = DialogResult.Cancel;
            }
            else if ((this.msgBoxButtons == MessageBoxButtons.YesNo) || (this.msgBoxButtons == MessageBoxButtons.YesNoCancel))
            {
                cancel = DialogResult.No;
            }
            else if (this.msgBoxButtons == MessageBoxButtons.AbortRetryIgnore)
            {
                cancel = DialogResult.Retry;
            }
            else if (this.msgBoxButtons == MessageBoxButtons.RetryCancel)
            {
                cancel = DialogResult.Cancel;
            }
            res = base.DialogResult = cancel;

           // this.Visible = false;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            DialogResult cancel = DialogResult.Cancel;
            if (this.msgBoxButtons == MessageBoxButtons.AbortRetryIgnore)
            {
                cancel = DialogResult.Ignore;
            }
            else if (this.msgBoxButtons == MessageBoxButtons.YesNoCancel)
            {
                cancel = DialogResult.Cancel;
            }
            res = base.DialogResult = cancel;

           // this.Visible = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.vContainer1 != null))
            {
                this.vContainer1.Dispose();
            }
            base.Dispose(disposing);
        }

    }

}
