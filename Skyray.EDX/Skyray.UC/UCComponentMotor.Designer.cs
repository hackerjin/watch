namespace Skyray.UC
{
    partial class UCComponentMotor
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
            this.components = new System.ComponentModel.Container();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ucMotorZ = new Skyray.UC.UCXYZMotor();
            this.ucMotorY = new Skyray.UC.UCXYZMotor();
            this.ucMotorX = new Skyray.UC.UCXYZMotor();
            this.autoDockManage1 = new Skyray.UC.AutoDockManage(this.components);
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ucMotorZ);
            this.panel2.Controls.Add(this.ucMotorY);
            this.panel2.Controls.Add(this.ucMotorX);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(8, 8);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(199, 273);
            this.panel2.TabIndex = 5;
            // 
            // ucMotorZ
            // 
            this.ucMotorZ.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucMotorZ.EnableDisable = true;
            this.ucMotorZ.GrpText = "方向 Z";
            this.ucMotorZ.IsShowScroll = false;
            this.ucMotorZ.IsShowTools = false;
            this.ucMotorZ.LeftImage = global::Skyray.UC.Properties.Resources.Top;
            this.ucMotorZ.Location = new System.Drawing.Point(0, 182);
            this.ucMotorZ.MoterType = Skyray.EDX.Common.Component.MoterType.ZMotor;
            this.ucMotorZ.MotorStep = 9000000;
            this.ucMotorZ.Name = "ucMotorZ";
            this.ucMotorZ.Padding = new System.Windows.Forms.Padding(8);
            this.ucMotorZ.RightImage = global::Skyray.UC.Properties.Resources.Bottom;
            this.ucMotorZ.ShowStop = false;
            this.ucMotorZ.Size = new System.Drawing.Size(199, 91);
            this.ucMotorZ.StopImag = global::Skyray.UC.Properties.Resources.StopTest;
            this.ucMotorZ.TabIndex = 3;
            // 
            // ucMotorY
            // 
            this.ucMotorY.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucMotorY.EnableDisable = true;
            this.ucMotorY.GrpText = "方向 Y";
            this.ucMotorY.IsShowScroll = false;
            this.ucMotorY.IsShowTools = false;
            this.ucMotorY.LeftImage = global::Skyray.UC.Properties.Resources.go_up;
            this.ucMotorY.Location = new System.Drawing.Point(0, 91);
            this.ucMotorY.MoterType = Skyray.EDX.Common.Component.MoterType.YMotor;
            this.ucMotorY.MotorStep = 9000000;
            this.ucMotorY.Name = "ucMotorY";
            this.ucMotorY.Padding = new System.Windows.Forms.Padding(8);
            this.ucMotorY.RightImage = global::Skyray.UC.Properties.Resources.down;
            this.ucMotorY.ShowStop = false;
            this.ucMotorY.Size = new System.Drawing.Size(199, 91);
            this.ucMotorY.StopImag = global::Skyray.UC.Properties.Resources.StopTest;
            this.ucMotorY.TabIndex = 2;
            // 
            // ucMotorX
            // 
            this.ucMotorX.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucMotorX.EnableDisable = true;
            this.ucMotorX.GrpText = "方向 X";
            this.ucMotorX.IsShowScroll = false;
            this.ucMotorX.IsShowTools = false;
            this.ucMotorX.LeftImage = global::Skyray.UC.Properties.Resources.back;
            this.ucMotorX.Location = new System.Drawing.Point(0, 0);
            this.ucMotorX.MoterType = Skyray.EDX.Common.Component.MoterType.XMotor;
            this.ucMotorX.MotorStep = 9000000;
            this.ucMotorX.Name = "ucMotorX";
            this.ucMotorX.Padding = new System.Windows.Forms.Padding(8);
            this.ucMotorX.RightImage = global::Skyray.UC.Properties.Resources.forward;
            this.ucMotorX.ShowStop = false;
            this.ucMotorX.Size = new System.Drawing.Size(199, 91);
            this.ucMotorX.StopImag = global::Skyray.UC.Properties.Resources.StopTest;
            this.ucMotorX.TabIndex = 1;
            // 
            // autoDockManage1
            // 
            this.autoDockManage1.DockForm = null;
            // 
            // UCComponentMotor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Name = "UCComponentMotor";
            this.Size = new System.Drawing.Size(215, 289);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UCXYZMotor ucMotorX;
        private UCXYZMotor ucMotorY;
        private UCXYZMotor ucMotorZ;
        private System.Windows.Forms.Panel panel2;
        private AutoDockManage autoDockManage1;
    }
}
