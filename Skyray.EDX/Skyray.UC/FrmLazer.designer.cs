using Skyray.EDX.Common;
namespace Skyray.UC
{
    partial class FrmLazer
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.rbManual = new System.Windows.Forms.RadioButton();
            this.rbAuto = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbOff = new System.Windows.Forms.RadioButton();
            this.rbOn = new System.Windows.Forms.RadioButton();
            this.groupLaser = new Skyray.Controls.Grouper();
            this.groupShell = new Skyray.Controls.Grouper();
            this.numSpeed = new Skyray.Controls.NumricUpDownW();
            this.lblMotorXSpeed = new Skyray.Controls.LabelW();
            this.numId = new Skyray.Controls.NumricUpDownW();
            this.lblMotorXCode = new Skyray.Controls.LabelW();
            this.buttonWSubmit = new Skyray.Controls.ButtonW();
            this.btnApplication = new Skyray.Controls.ButtonW();
            this.panel1.SuspendLayout();
            this.groupLaser.SuspendLayout();
            this.groupShell.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numId)).BeginInit();
            this.SuspendLayout();
            // 
            // rbManual
            // 
            this.rbManual.AutoSize = true;
            this.rbManual.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbManual.Location = new System.Drawing.Point(22, 68);
            this.rbManual.Name = "rbManual";
            this.rbManual.Size = new System.Drawing.Size(91, 33);
            this.rbManual.TabIndex = 0;
            this.rbManual.TabStop = true;
            this.rbManual.Text = "手动";
            this.rbManual.UseVisualStyleBackColor = true;
            this.rbManual.CheckedChanged += new System.EventHandler(this.rbManual_CheckedChanged);
            // 
            // rbAuto
            // 
            this.rbAuto.AutoSize = true;
            this.rbAuto.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbAuto.Location = new System.Drawing.Point(22, 29);
            this.rbAuto.Name = "rbAuto";
            this.rbAuto.Size = new System.Drawing.Size(91, 33);
            this.rbAuto.TabIndex = 1;
            this.rbAuto.TabStop = true;
            this.rbAuto.Text = "自动";
            this.rbAuto.UseVisualStyleBackColor = true;
            this.rbAuto.CheckedChanged += new System.EventHandler(this.rbAuto_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbOff);
            this.panel1.Controls.Add(this.rbOn);
            this.panel1.Location = new System.Drawing.Point(46, 120);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(89, 91);
            this.panel1.TabIndex = 2;
            // 
            // rbOff
            // 
            this.rbOff.AutoSize = true;
            this.rbOff.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbOff.Location = new System.Drawing.Point(3, 50);
            this.rbOff.Name = "rbOff";
            this.rbOff.Size = new System.Drawing.Size(53, 28);
            this.rbOff.TabIndex = 3;
            this.rbOff.TabStop = true;
            this.rbOff.Text = "关";
            this.rbOff.UseVisualStyleBackColor = true;
            this.rbOff.CheckedChanged += new System.EventHandler(this.rbOff_CheckedChanged);
            // 
            // rbOn
            // 
            this.rbOn.AutoSize = true;
            this.rbOn.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbOn.Location = new System.Drawing.Point(3, 3);
            this.rbOn.Name = "rbOn";
            this.rbOn.Size = new System.Drawing.Size(53, 28);
            this.rbOn.TabIndex = 2;
            this.rbOn.TabStop = true;
            this.rbOn.Text = "开";
            this.rbOn.UseVisualStyleBackColor = true;
            this.rbOn.CheckedChanged += new System.EventHandler(this.rbOn_CheckedChanged);
            // 
            // groupLaser
            // 
            this.groupLaser.BackgroundColor = System.Drawing.Color.Transparent;
            this.groupLaser.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.groupLaser.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.groupLaser.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.groupLaser.BorderThickness = 1F;
            this.groupLaser.BorderTopOnly = false;
            this.groupLaser.Controls.Add(this.rbAuto);
            this.groupLaser.Controls.Add(this.panel1);
            this.groupLaser.Controls.Add(this.rbManual);
            this.groupLaser.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.groupLaser.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Center;
            this.groupLaser.GroupImage = null;
            this.groupLaser.GroupTitle = "激光器";
            this.groupLaser.HeaderRoundCorners = 4;
            this.groupLaser.Location = new System.Drawing.Point(11, 0);
            this.groupLaser.Name = "groupLaser";
            this.groupLaser.PaintGroupBox = false;
            this.groupLaser.RoundCorners = 4;
            this.groupLaser.ShadowColor = System.Drawing.Color.DarkGray;
            this.groupLaser.ShadowControl = false;
            this.groupLaser.ShadowThickness = 3;
            this.groupLaser.Size = new System.Drawing.Size(185, 227);
            this.groupLaser.TabIndex = 3;
            this.groupLaser.TextLineSpace = 2;
            this.groupLaser.TitleLeftSpace = 18;
            // 
            // groupShell
            // 
            this.groupShell.BackgroundColor = System.Drawing.Color.Transparent;
            this.groupShell.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.groupShell.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.groupShell.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.groupShell.BorderThickness = 1F;
            this.groupShell.BorderTopOnly = false;
            this.groupShell.Controls.Add(this.numSpeed);
            this.groupShell.Controls.Add(this.lblMotorXSpeed);
            this.groupShell.Controls.Add(this.numId);
            this.groupShell.Controls.Add(this.lblMotorXCode);
            this.groupShell.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.groupShell.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Center;
            this.groupShell.GroupImage = null;
            this.groupShell.GroupTitle = "防护罩";
            this.groupShell.HeaderRoundCorners = 4;
            this.groupShell.Location = new System.Drawing.Point(202, 0);
            this.groupShell.Name = "groupShell";
            this.groupShell.PaintGroupBox = false;
            this.groupShell.RoundCorners = 4;
            this.groupShell.ShadowColor = System.Drawing.Color.DarkGray;
            this.groupShell.ShadowControl = false;
            this.groupShell.ShadowThickness = 3;
            this.groupShell.Size = new System.Drawing.Size(192, 227);
            this.groupShell.TabIndex = 4;
            this.groupShell.TextLineSpace = 2;
            this.groupShell.TitleLeftSpace = 18;
            // 
            // numSpeed
            // 
            this.numSpeed.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numSpeed.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numSpeed.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numSpeed.Location = new System.Drawing.Point(24, 134);
            this.numSpeed.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numSpeed.Name = "numSpeed";
            this.numSpeed.Size = new System.Drawing.Size(120, 21);
            this.numSpeed.TabIndex = 3;
            // 
            // lblMotorXSpeed
            // 
            this.lblMotorXSpeed.AutoSize = true;
            this.lblMotorXSpeed.BackColor = System.Drawing.Color.Transparent;
            this.lblMotorXSpeed.Location = new System.Drawing.Point(23, 111);
            this.lblMotorXSpeed.Name = "lblMotorXSpeed";
            this.lblMotorXSpeed.Size = new System.Drawing.Size(35, 12);
            this.lblMotorXSpeed.TabIndex = 2;
            this.lblMotorXSpeed.Text = "速度:";
            // 
            // numId
            // 
            this.numId.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numId.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numId.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numId.Location = new System.Drawing.Point(24, 68);
            this.numId.Name = "numId";
            this.numId.Size = new System.Drawing.Size(120, 21);
            this.numId.TabIndex = 1;
            // 
            // lblMotorXCode
            // 
            this.lblMotorXCode.AutoSize = true;
            this.lblMotorXCode.BackColor = System.Drawing.Color.Transparent;
            this.lblMotorXCode.Location = new System.Drawing.Point(23, 46);
            this.lblMotorXCode.Name = "lblMotorXCode";
            this.lblMotorXCode.Size = new System.Drawing.Size(59, 12);
            this.lblMotorXCode.TabIndex = 0;
            this.lblMotorXCode.Text = "电机编号:";
            // 
            // buttonWSubmit
            // 
            this.buttonWSubmit.bSilver = false;
            this.buttonWSubmit.Location = new System.Drawing.Point(238, 241);
            this.buttonWSubmit.MaxImageSize = new System.Drawing.Point(0, 0);
            this.buttonWSubmit.MenuPos = new System.Drawing.Point(0, 0);
            this.buttonWSubmit.Name = "buttonWSubmit";
            this.buttonWSubmit.Size = new System.Drawing.Size(75, 23);
            this.buttonWSubmit.Style = Skyray.Controls.Style.Office2007Blue;
            this.buttonWSubmit.TabIndex = 5;
            this.buttonWSubmit.Text = "OK";
            this.buttonWSubmit.ToFocused = false;
            this.buttonWSubmit.UseVisualStyleBackColor = true;
            this.buttonWSubmit.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnApplication
            // 
            this.btnApplication.bSilver = false;
            this.btnApplication.Location = new System.Drawing.Point(60, 241);
            this.btnApplication.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnApplication.MenuPos = new System.Drawing.Point(0, 0);
            this.btnApplication.Name = "btnApplication";
            this.btnApplication.Size = new System.Drawing.Size(75, 23);
            this.btnApplication.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnApplication.TabIndex = 6;
            this.btnApplication.Text = "应用";
            this.btnApplication.ToFocused = false;
            this.btnApplication.UseVisualStyleBackColor = true;
            this.btnApplication.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // FrmLazer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnApplication);
            this.Controls.Add(this.buttonWSubmit);
            this.Controls.Add(this.groupShell);
            this.Controls.Add(this.groupLaser);
            this.Name = "FrmLazer";
            this.Size = new System.Drawing.Size(405, 275);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupLaser.ResumeLayout(false);
            this.groupLaser.PerformLayout();
            this.groupShell.ResumeLayout(false);
            this.groupShell.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numId)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rbManual;
        private System.Windows.Forms.RadioButton rbAuto;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbOff;
        private System.Windows.Forms.RadioButton rbOn;
        private Skyray.Controls.Grouper groupLaser;
        private Skyray.Controls.Grouper groupShell;
        private Skyray.Controls.ButtonW buttonWSubmit;
        private Skyray.Controls.ButtonW btnApplication;
        private Skyray.Controls.NumricUpDownW numSpeed;
        private Skyray.Controls.LabelW lblMotorXSpeed;
        private Skyray.Controls.NumricUpDownW numId;
        private Skyray.Controls.LabelW lblMotorXCode;

    }
}
