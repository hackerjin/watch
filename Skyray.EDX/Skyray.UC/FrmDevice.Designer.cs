namespace Skyray.UC
{
    partial class FrmDevice
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnApplication = new Skyray.Controls.ButtonW();
            this.txtNewDevice = new Skyray.Controls.TextBoxW();
            this.btnAdd = new Skyray.Controls.ButtonW();
            this.btnDel = new Skyray.Controls.ButtonW();
            this.btnCancel = new Skyray.Controls.ButtonW();
            this.btnOK = new Skyray.Controls.ButtonW();
            this.tabCWDevice = new Skyray.Controls.TabControlW();
            this.tpDevice = new System.Windows.Forms.TabPage();
            this.grpFPGA = new Skyray.Controls.Grouper();
            this.numQCProtect = new Skyray.Controls.NumricUpDownW();
            this.lblQCProtect = new Skyray.Controls.LabelW();
            this.cboBLRSpeedDpp100 = new Skyray.Controls.ComboBoxW();
            this.numZeroOffSetDpp100 = new Skyray.Controls.NumricUpDownW();
            this.lblZeroOffset = new Skyray.Controls.LabelW();
            this.lblBLRSpeed = new Skyray.Controls.LabelW();
            this.cboPreAMPDpp100 = new Skyray.Controls.ComboBoxW();
            this.lblPreAmp = new Skyray.Controls.LabelW();
            this.cboFlatTopDpp100 = new Skyray.Controls.ComboBoxW();
            this.cboPeakingTimDpp100 = new Skyray.Controls.ComboBoxW();
            this.cboGain = new Skyray.Controls.ComboBoxW();
            this.lblGain = new Skyray.Controls.LabelW();
            this.radioDMCA = new System.Windows.Forms.RadioButton();
            this.radioDpp100 = new System.Windows.Forms.RadioButton();
            this.numIntercept = new Skyray.Controls.NumricUpDownW();
            this.lblIntercept = new Skyray.Controls.LabelW();
            this.numFastLimit = new Skyray.Controls.NumricUpDownW();
            this.lblFastLimit = new Skyray.Controls.LabelW();
            this.lblIP = new Skyray.Controls.LabelW();
            this.txtIP = new Skyray.Controls.TextBoxW();
            this.lblSlowLimit = new Skyray.Controls.LabelW();
            this.lblFlatTop = new Skyray.Controls.LabelW();
            this.lblPeakingTime = new Skyray.Controls.LabelW();
            this.lblRate = new Skyray.Controls.LabelW();
            this.lblHeapUP = new Skyray.Controls.LabelW();
            this.numSlowLimit = new Skyray.Controls.NumricUpDownW();
            this.cboRate = new Skyray.Controls.ComboBoxW();
            this.cboFlatTop = new Skyray.Controls.ComboBoxW();
            this.cboPeakingTime = new Skyray.Controls.ComboBoxW();
            this.cboHeapUP = new Skyray.Controls.ComboBoxW();
            this.grpBlueTooth = new Skyray.Controls.Grouper();
            this.cboPocket = new Skyray.Controls.ComboBoxW();
            this.lblPocket = new Skyray.Controls.LabelW();
            this.cboBits = new Skyray.Controls.ComboBoxW();
            this.lblCom = new Skyray.Controls.LabelW();
            this.numComNum = new Skyray.Controls.NumricUpDownW();
            this.lblBits = new Skyray.Controls.LabelW();
            this.lblComNum = new Skyray.Controls.LabelW();
            this.grpUSB = new Skyray.Controls.Grouper();
            this.lblDp5Type = new Skyray.Controls.LabelW();
            this.cboDp5Type = new Skyray.Controls.ComboBoxW();
            this.grpDP5Params = new Skyray.Controls.Grouper();
            this.lblDp5IP = new Skyray.Controls.LabelW();
            this.txtDp5Ip = new Skyray.Controls.TextBoxW();
            this.cboVoltage = new Skyray.Controls.ComboBoxW();
            this.lblVoltage = new Skyray.Controls.LabelW();
            this.numDP5FastThreshold = new Skyray.Controls.NumricUpDownW();
            this.cboDP5HeapUP = new Skyray.Controls.ComboBoxW();
            this.cboDP5FlatTop = new Skyray.Controls.ComboBoxW();
            this.cboDP5PeakingTime = new Skyray.Controls.ComboBoxW();
            this.lblDP5FastThreshold = new Skyray.Controls.LabelW();
            this.lblDP5HeapUP = new Skyray.Controls.LabelW();
            this.lblDP5FlatTop = new Skyray.Controls.LabelW();
            this.lblDP5PeakingTime = new Skyray.Controls.LabelW();
            this.cboPortType = new Skyray.Controls.ComboBoxW();
            this.comboBoxWVersion = new Skyray.Controls.ComboBoxW();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblIsPassword = new Skyray.Controls.LabelW();
            this.radIsPassward = new System.Windows.Forms.RadioButton();
            this.radIsPassward2 = new System.Windows.Forms.RadioButton();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lblDP5 = new Skyray.Controls.LabelW();
            this.radIsDP5 = new System.Windows.Forms.RadioButton();
            this.radIsNotDP5 = new System.Windows.Forms.RadioButton();
            this.grpParallel = new Skyray.Controls.Grouper();
            this.radParallel = new System.Windows.Forms.RadioButton();
            this.grpDevice = new Skyray.Controls.Grouper();
            this.btnSampleCal = new Skyray.Controls.ButtonW();
            this.chkAutoDetection = new Skyray.Controls.CheckBoxW();
            this.pnlVacuum = new System.Windows.Forms.Panel();
            this.lblVacuumPumpType = new Skyray.Controls.LabelW();
            this.cboVacuumPumpType = new Skyray.Controls.ComboBoxW();
            this.diMaxCurrent = new Skyray.Controls.DoubleInputW();
            this.diMaxVoltage = new Skyray.Controls.DoubleInputW();
            this.lblMaxCurrent = new Skyray.Controls.LabelW();
            this.lblMaxVoltage = new Skyray.Controls.LabelW();
            this.panel3 = new System.Windows.Forms.Panel();
            this.radIsAllowOpenCover2 = new System.Windows.Forms.RadioButton();
            this.lblIsAllowOpenCover = new Skyray.Controls.LabelW();
            this.radIsAllowOpenCover = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblHasElectromagnet = new Skyray.Controls.LabelW();
            this.radHasElectromagnet = new System.Windows.Forms.RadioButton();
            this.radHasElectromagnet2 = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radVacuumPump = new System.Windows.Forms.RadioButton();
            this.radVacuumPump2 = new System.Windows.Forms.RadioButton();
            this.lblVacuumPump = new Skyray.Controls.LabelW();
            this.cmbSpecLength = new Skyray.Controls.ComboBoxW();
            this.lblVoltageScaleFactor = new Skyray.Controls.LabelW();
            this.numVoltageScaleFactor = new Skyray.Controls.NumricUpDownW();
            this.lblSpecLength = new Skyray.Controls.LabelW();
            this.numCurrentScaleFactor = new Skyray.Controls.NumricUpDownW();
            this.lblCurrentScaleFactor = new Skyray.Controls.LabelW();
            this.radBlueTooth = new System.Windows.Forms.RadioButton();
            this.radFPGA = new System.Windows.Forms.RadioButton();
            this.radUSB = new System.Windows.Forms.RadioButton();
            this.lblPortType = new Skyray.Controls.LabelW();
            this.tpFilter = new System.Windows.Forms.TabPage();
            this.numFilterSpeed = new Skyray.Controls.NumricUpDownW();
            this.lblFilterSpeed = new Skyray.Controls.LabelW();
            this.numCollimatorSpeed = new Skyray.Controls.NumricUpDownW();
            this.lblCollimatorSpeed = new Skyray.Controls.LabelW();
            this.numFilterMaxNum = new Skyray.Controls.NumricUpDownW();
            this.lblFilterMaxNum = new Skyray.Controls.LabelW();
            this.numCollimatorMaxNum = new Skyray.Controls.NumricUpDownW();
            this.lblCollimatorMaxNum = new Skyray.Controls.LabelW();
            this.numFilterDirect = new Skyray.Controls.NumricUpDownW();
            this.lblFilterDirect = new Skyray.Controls.LabelW();
            this.numFilterCode = new Skyray.Controls.NumricUpDownW();
            this.lblFilterCode = new Skyray.Controls.LabelW();
            this.numCollimatorDirect = new Skyray.Controls.NumricUpDownW();
            this.lblCollimatorDirect = new Skyray.Controls.LabelW();
            this.numCollimatorCode = new Skyray.Controls.NumricUpDownW();
            this.lblCollimatorCode = new Skyray.Controls.LabelW();
            this.chkFilter = new Skyray.Controls.CheckBoxW();
            this.chkCollimator = new Skyray.Controls.CheckBoxW();
            this.dgvwFilter = new Skyray.Controls.DataGridViewW();
            this.ColFilterNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColFilterStep = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColCaption = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColFilterThickness = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvwCollimator = new Skyray.Controls.DataGridViewW();
            this.ColCollimatorNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColDiameter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColCollimatorStep = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpTarget = new System.Windows.Forms.TabPage();
            this.numTargetSpeed = new Skyray.Controls.NumricUpDownW();
            this.lblTargetSpeed = new Skyray.Controls.LabelW();
            this.numTargetMaxNum = new Skyray.Controls.NumricUpDownW();
            this.lblTargetMaxNum = new Skyray.Controls.LabelW();
            this.numTargetDirect = new Skyray.Controls.NumricUpDownW();
            this.lblTargetDirect = new Skyray.Controls.LabelW();
            this.numTargetCode = new Skyray.Controls.NumricUpDownW();
            this.lblTargetCode = new Skyray.Controls.LabelW();
            this.chkTarget = new Skyray.Controls.CheckBoxW();
            this.dgvwTarget = new Skyray.Controls.DataGridViewW();
            this.ColTargetNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColTargetStep = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColTargetCaption = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColTargetThickness = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numChamberSpeed = new Skyray.Controls.NumricUpDownW();
            this.lblChamberSpeed = new Skyray.Controls.LabelW();
            this.numChamberMaxNum = new Skyray.Controls.NumricUpDownW();
            this.lblChamberMaxNum = new Skyray.Controls.LabelW();
            this.numChamberDirect = new Skyray.Controls.NumricUpDownW();
            this.lblChamberDirect = new Skyray.Controls.LabelW();
            this.numChamberCode = new Skyray.Controls.NumricUpDownW();
            this.lblChamberCode = new Skyray.Controls.LabelW();
            this.chkChamber = new Skyray.Controls.CheckBoxW();
            this.dgvwChamber = new Skyray.Controls.DataGridViewW();
            this.ColChamberNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColChamberStep = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColChamberStepCoef1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColChamberStepCoef2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpother = new System.Windows.Forms.TabPage();
            this.gpXYZ = new Skyray.Controls.Grouper();
            this.pnlEncoderMotor = new System.Windows.Forms.Panel();
            this.numMotorEncoderSpeed = new Skyray.Controls.NumricUpDownW();
            this.lblMotorEncoderSpeed = new Skyray.Controls.LabelW();
            this.lblMotorEncoderCode = new Skyray.Controls.LabelW();
            this.numMotorEncoderCode = new Skyray.Controls.NumricUpDownW();
            this.lblMotorEncoderDirect = new Skyray.Controls.LabelW();
            this.numMotorEncoderDirect = new Skyray.Controls.NumricUpDownW();
            this.lblMotorEncoderMaxStep = new Skyray.Controls.LabelW();
            this.numMotorEncoderMaxStep = new Skyray.Controls.NumricUpDownW();
            this.chkHasMotorEncoder = new Skyray.Controls.CheckBoxW();
            this.pnlLightMotor = new System.Windows.Forms.Panel();
            this.numMotorLightSpeed = new Skyray.Controls.NumricUpDownW();
            this.lblMotorLightSpeed = new Skyray.Controls.LabelW();
            this.lblMotorLightCode = new Skyray.Controls.LabelW();
            this.numMotorLightCode = new Skyray.Controls.NumricUpDownW();
            this.lblMotorLightDirect = new Skyray.Controls.LabelW();
            this.numMotorLightDirect = new Skyray.Controls.NumricUpDownW();
            this.lblMotorLightMaxStep = new Skyray.Controls.LabelW();
            this.numMotorLightMaxStep = new Skyray.Controls.NumricUpDownW();
            this.chkHasMotorLight = new Skyray.Controls.CheckBoxW();
            this.pnlY1Motor = new System.Windows.Forms.Panel();
            this.numMotorY1Speed = new Skyray.Controls.NumricUpDownW();
            this.lblMotorY1Speed = new Skyray.Controls.LabelW();
            this.lblMotorY1Code = new Skyray.Controls.LabelW();
            this.numMotorY1Code = new Skyray.Controls.NumricUpDownW();
            this.lblMotorY1Direct = new Skyray.Controls.LabelW();
            this.numMotorY1Direct = new Skyray.Controls.NumricUpDownW();
            this.lblMotorY1MaxDis = new Skyray.Controls.LabelW();
            this.numMotorY1MaxDis = new Skyray.Controls.NumricUpDownW();
            this.chkHasMotorY1 = new Skyray.Controls.CheckBoxW();
            this.pnlXMotor = new System.Windows.Forms.Panel();
            this.numMotorXSpeed = new Skyray.Controls.NumricUpDownW();
            this.lblMotorXSpeed = new Skyray.Controls.LabelW();
            this.lblMotorXCode = new Skyray.Controls.LabelW();
            this.numMotorXCode = new Skyray.Controls.NumricUpDownW();
            this.lblMotorXDirect = new Skyray.Controls.LabelW();
            this.numMotorXDirect = new Skyray.Controls.NumricUpDownW();
            this.lblMotorXMaxDis = new Skyray.Controls.LabelW();
            this.numMotorXMaxDis = new Skyray.Controls.NumricUpDownW();
            this.chkHasMotorX = new Skyray.Controls.CheckBoxW();
            this.pnlZMotor = new System.Windows.Forms.Panel();
            this.numDutyDown = new Skyray.Controls.NumricUpDownW();
            this.lblDutyDown = new Skyray.Controls.LabelW();
            this.numDutyUp = new Skyray.Controls.NumricUpDownW();
            this.lblDutyUp = new Skyray.Controls.LabelW();
            this.numMotorZSpeed = new Skyray.Controls.NumricUpDownW();
            this.lblMotorZSpeed = new Skyray.Controls.LabelW();
            this.lblMotorZCode = new Skyray.Controls.LabelW();
            this.numMotorZCode = new Skyray.Controls.NumricUpDownW();
            this.lblMotorZDirect = new Skyray.Controls.LabelW();
            this.numMotorZDirect = new Skyray.Controls.NumricUpDownW();
            this.lblMotorZMaxDis = new Skyray.Controls.LabelW();
            this.numMotorZMaxDis = new Skyray.Controls.NumricUpDownW();
            this.chkHasMotorY = new Skyray.Controls.CheckBoxW();
            this.pnlYMotor = new System.Windows.Forms.Panel();
            this.numMotorYSpeed = new Skyray.Controls.NumricUpDownW();
            this.lblMotorYSpeed = new Skyray.Controls.LabelW();
            this.lblMotorYCode = new Skyray.Controls.LabelW();
            this.numMotorYCode = new Skyray.Controls.NumricUpDownW();
            this.lblMotorYDirect = new Skyray.Controls.LabelW();
            this.numMotorYDirect = new Skyray.Controls.NumricUpDownW();
            this.lblMotorYMaxDis = new Skyray.Controls.LabelW();
            this.numMotorYMaxDis = new Skyray.Controls.NumricUpDownW();
            this.chkHasMotorZ = new Skyray.Controls.CheckBoxW();
            this.meTubes = new Skyray.EDX.Common.ModelEditor();
            this.meDetector = new Skyray.EDX.Common.ModelEditor();
            this.lbwDevice = new Skyray.Controls.ListBoxW();
            this.cmsRenameDevice = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmRename = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabCWDevice.SuspendLayout();
            this.tpDevice.SuspendLayout();
            this.grpFPGA.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQCProtect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numZeroOffSetDpp100)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIntercept)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFastLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSlowLimit)).BeginInit();
            this.grpBlueTooth.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numComNum)).BeginInit();
            this.grpUSB.SuspendLayout();
            this.grpDP5Params.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDP5FastThreshold)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.grpDevice.SuspendLayout();
            this.pnlVacuum.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numVoltageScaleFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCurrentScaleFactor)).BeginInit();
            this.tpFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFilterSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCollimatorSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFilterMaxNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCollimatorMaxNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFilterDirect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFilterCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCollimatorDirect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCollimatorCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvwFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvwCollimator)).BeginInit();
            this.tpTarget.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTargetSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTargetMaxNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTargetDirect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTargetCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvwTarget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChamberSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChamberMaxNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChamberDirect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChamberCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvwChamber)).BeginInit();
            this.tpother.SuspendLayout();
            this.gpXYZ.SuspendLayout();
            this.pnlEncoderMotor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorEncoderSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorEncoderCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorEncoderDirect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorEncoderMaxStep)).BeginInit();
            this.pnlLightMotor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorLightSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorLightCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorLightDirect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorLightMaxStep)).BeginInit();
            this.pnlY1Motor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorY1Speed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorY1Code)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorY1Direct)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorY1MaxDis)).BeginInit();
            this.pnlXMotor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorXSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorXCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorXDirect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorXMaxDis)).BeginInit();
            this.pnlZMotor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDutyDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDutyUp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorZSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorZCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorZDirect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorZMaxDis)).BeginInit();
            this.pnlYMotor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorYSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorYCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorYDirect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorYMaxDis)).BeginInit();
            this.cmsRenameDevice.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnApplication
            // 
            this.btnApplication.bSilver = false;
            this.btnApplication.Location = new System.Drawing.Point(506, 585);
            this.btnApplication.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnApplication.MenuPos = new System.Drawing.Point(0, 0);
            this.btnApplication.Name = "btnApplication";
            this.btnApplication.Size = new System.Drawing.Size(89, 23);
            this.btnApplication.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnApplication.TabIndex = 53;
            this.btnApplication.Text = "应用";
            this.btnApplication.ToFocused = false;
            this.btnApplication.UseVisualStyleBackColor = true;
            this.btnApplication.Click += new System.EventHandler(this.btnApplication_Click);
            // 
            // txtNewDevice
            // 
            this.txtNewDevice.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtNewDevice.Location = new System.Drawing.Point(14, 444);
            this.txtNewDevice.Name = "txtNewDevice";
            this.txtNewDevice.Size = new System.Drawing.Size(118, 21);
            this.txtNewDevice.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtNewDevice.TabIndex = 5;
            // 
            // btnAdd
            // 
            this.btnAdd.bSilver = false;
            this.btnAdd.Location = new System.Drawing.Point(14, 473);
            this.btnAdd.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnAdd.MenuPos = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(118, 23);
            this.btnAdd.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "新建";
            this.btnAdd.ToFocused = false;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDel
            // 
            this.btnDel.bSilver = false;
            this.btnDel.Location = new System.Drawing.Point(14, 506);
            this.btnDel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnDel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(118, 23);
            this.btnDel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnDel.TabIndex = 6;
            this.btnDel.Text = "删除选择";
            this.btnDel.ToFocused = false;
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.bSilver = false;
            this.btnCancel.Location = new System.Drawing.Point(719, 585);
            this.btnCancel.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnCancel.MenuPos = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(89, 23);
            this.btnCancel.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.ToFocused = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOK
            // 
            this.btnOK.bSilver = false;
            this.btnOK.Location = new System.Drawing.Point(614, 585);
            this.btnOK.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnOK.MenuPos = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(89, 23);
            this.btnOK.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "确定";
            this.btnOK.ToFocused = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tabCWDevice
            // 
            this.tabCWDevice.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(79)))), ((int)(((byte)(125)))));
            this.tabCWDevice.BackColor = System.Drawing.Color.GhostWhite;
            this.tabCWDevice.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.tabCWDevice.Controls.Add(this.tpDevice);
            this.tabCWDevice.Controls.Add(this.tpFilter);
            this.tabCWDevice.Controls.Add(this.tpTarget);
            this.tabCWDevice.Controls.Add(this.tpother);
            this.tabCWDevice.Location = new System.Drawing.Point(138, 13);
            this.tabCWDevice.Name = "tabCWDevice";
            this.tabCWDevice.SelectedIndex = 0;
            this.tabCWDevice.ShowTabs = true;
            this.tabCWDevice.Size = new System.Drawing.Size(716, 554);
            this.tabCWDevice.Style = Skyray.Controls.Style.Office2007Blue;
            this.tabCWDevice.TabIndex = 1;
            // 
            // tpDevice
            // 
            this.tpDevice.BackColor = System.Drawing.Color.GhostWhite;
            this.tpDevice.Controls.Add(this.grpFPGA);
            this.tpDevice.Controls.Add(this.grpBlueTooth);
            this.tpDevice.Controls.Add(this.grpUSB);
            this.tpDevice.Controls.Add(this.grpParallel);
            this.tpDevice.Controls.Add(this.radParallel);
            this.tpDevice.Controls.Add(this.grpDevice);
            this.tpDevice.Controls.Add(this.radBlueTooth);
            this.tpDevice.Controls.Add(this.radFPGA);
            this.tpDevice.Controls.Add(this.radUSB);
            this.tpDevice.Controls.Add(this.lblPortType);
            this.tpDevice.Location = new System.Drawing.Point(4, 26);
            this.tpDevice.Name = "tpDevice";
            this.tpDevice.Padding = new System.Windows.Forms.Padding(3);
            this.tpDevice.Size = new System.Drawing.Size(708, 524);
            this.tpDevice.TabIndex = 0;
            this.tpDevice.Text = "仪器信息";
            this.tpDevice.UseVisualStyleBackColor = true;
            // 
            // grpFPGA
            // 
            this.grpFPGA.BackgroundColor = System.Drawing.Color.Transparent;
            this.grpFPGA.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grpFPGA.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grpFPGA.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grpFPGA.BorderThickness = 1F;
            this.grpFPGA.BorderTopOnly = false;
            this.grpFPGA.Controls.Add(this.numQCProtect);
            this.grpFPGA.Controls.Add(this.lblQCProtect);
            this.grpFPGA.Controls.Add(this.cboBLRSpeedDpp100);
            this.grpFPGA.Controls.Add(this.numZeroOffSetDpp100);
            this.grpFPGA.Controls.Add(this.lblZeroOffset);
            this.grpFPGA.Controls.Add(this.lblBLRSpeed);
            this.grpFPGA.Controls.Add(this.cboPreAMPDpp100);
            this.grpFPGA.Controls.Add(this.lblPreAmp);
            this.grpFPGA.Controls.Add(this.cboFlatTopDpp100);
            this.grpFPGA.Controls.Add(this.cboPeakingTimDpp100);
            this.grpFPGA.Controls.Add(this.cboGain);
            this.grpFPGA.Controls.Add(this.lblGain);
            this.grpFPGA.Controls.Add(this.radioDMCA);
            this.grpFPGA.Controls.Add(this.radioDpp100);
            this.grpFPGA.Controls.Add(this.numIntercept);
            this.grpFPGA.Controls.Add(this.lblIntercept);
            this.grpFPGA.Controls.Add(this.numFastLimit);
            this.grpFPGA.Controls.Add(this.lblFastLimit);
            this.grpFPGA.Controls.Add(this.lblIP);
            this.grpFPGA.Controls.Add(this.txtIP);
            this.grpFPGA.Controls.Add(this.lblSlowLimit);
            this.grpFPGA.Controls.Add(this.lblFlatTop);
            this.grpFPGA.Controls.Add(this.lblPeakingTime);
            this.grpFPGA.Controls.Add(this.lblRate);
            this.grpFPGA.Controls.Add(this.lblHeapUP);
            this.grpFPGA.Controls.Add(this.numSlowLimit);
            this.grpFPGA.Controls.Add(this.cboRate);
            this.grpFPGA.Controls.Add(this.cboFlatTop);
            this.grpFPGA.Controls.Add(this.cboPeakingTime);
            this.grpFPGA.Controls.Add(this.cboHeapUP);
            this.grpFPGA.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grpFPGA.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Center;
            this.grpFPGA.GroupImage = null;
            this.grpFPGA.GroupTitle = "";
            this.grpFPGA.HeaderRoundCorners = 4;
            this.grpFPGA.Location = new System.Drawing.Point(10, 46);
            this.grpFPGA.Name = "grpFPGA";
            this.grpFPGA.PaintGroupBox = false;
            this.grpFPGA.RoundCorners = 4;
            this.grpFPGA.ShadowColor = System.Drawing.Color.DarkGray;
            this.grpFPGA.ShadowControl = false;
            this.grpFPGA.ShadowThickness = 3;
            this.grpFPGA.Size = new System.Drawing.Size(687, 188);
            this.grpFPGA.TabIndex = 111;
            this.grpFPGA.TextLineSpace = 2;
            this.grpFPGA.TitleLeftSpace = 18;
            // 
            // numQCProtect
            // 
            this.numQCProtect.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numQCProtect.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numQCProtect.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numQCProtect.Location = new System.Drawing.Point(566, 101);
            this.numQCProtect.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numQCProtect.Name = "numQCProtect";
            this.numQCProtect.Size = new System.Drawing.Size(120, 21);
            this.numQCProtect.TabIndex = 109;
            // 
            // lblQCProtect
            // 
            this.lblQCProtect.AutoSize = true;
            this.lblQCProtect.BackColor = System.Drawing.Color.Transparent;
            this.lblQCProtect.Location = new System.Drawing.Point(564, 86);
            this.lblQCProtect.Name = "lblQCProtect";
            this.lblQCProtect.Size = new System.Drawing.Size(71, 12);
            this.lblQCProtect.TabIndex = 108;
            this.lblQCProtect.Text = "Detector HV";
            // 
            // cboBLRSpeedDpp100
            // 
            this.cboBLRSpeedDpp100.AutoComplete = false;
            this.cboBLRSpeedDpp100.AutoDropdown = false;
            this.cboBLRSpeedDpp100.BackColorEven = System.Drawing.Color.White;
            this.cboBLRSpeedDpp100.BackColorOdd = System.Drawing.Color.White;
            this.cboBLRSpeedDpp100.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cboBLRSpeedDpp100.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cboBLRSpeedDpp100.ColumnNames = "";
            this.cboBLRSpeedDpp100.ColumnWidthDefault = 75;
            this.cboBLRSpeedDpp100.ColumnWidths = "";
            this.cboBLRSpeedDpp100.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboBLRSpeedDpp100.FormattingEnabled = true;
            this.cboBLRSpeedDpp100.LinkedColumnIndex = 0;
            this.cboBLRSpeedDpp100.LinkedTextBox = null;
            this.cboBLRSpeedDpp100.Location = new System.Drawing.Point(406, 96);
            this.cboBLRSpeedDpp100.Name = "cboBLRSpeedDpp100";
            this.cboBLRSpeedDpp100.Size = new System.Drawing.Size(121, 22);
            this.cboBLRSpeedDpp100.TabIndex = 107;
            // 
            // numZeroOffSetDpp100
            // 
            this.numZeroOffSetDpp100.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numZeroOffSetDpp100.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numZeroOffSetDpp100.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numZeroOffSetDpp100.Location = new System.Drawing.Point(406, 148);
            this.numZeroOffSetDpp100.Maximum = new decimal(new int[] {
            63,
            0,
            0,
            0});
            this.numZeroOffSetDpp100.Name = "numZeroOffSetDpp100";
            this.numZeroOffSetDpp100.Size = new System.Drawing.Size(120, 21);
            this.numZeroOffSetDpp100.TabIndex = 106;
            // 
            // lblZeroOffset
            // 
            this.lblZeroOffset.AutoSize = true;
            this.lblZeroOffset.BackColor = System.Drawing.Color.Transparent;
            this.lblZeroOffset.Location = new System.Drawing.Point(404, 133);
            this.lblZeroOffset.Name = "lblZeroOffset";
            this.lblZeroOffset.Size = new System.Drawing.Size(53, 12);
            this.lblZeroOffset.TabIndex = 105;
            this.lblZeroOffset.Text = "零偏移量";
            // 
            // lblBLRSpeed
            // 
            this.lblBLRSpeed.AutoSize = true;
            this.lblBLRSpeed.BackColor = System.Drawing.Color.Transparent;
            this.lblBLRSpeed.Location = new System.Drawing.Point(404, 81);
            this.lblBLRSpeed.Name = "lblBLRSpeed";
            this.lblBLRSpeed.Size = new System.Drawing.Size(53, 12);
            this.lblBLRSpeed.TabIndex = 103;
            this.lblBLRSpeed.Text = "BLR 速度";
            // 
            // cboPreAMPDpp100
            // 
            this.cboPreAMPDpp100.AutoComplete = false;
            this.cboPreAMPDpp100.AutoDropdown = false;
            this.cboPreAMPDpp100.BackColorEven = System.Drawing.Color.White;
            this.cboPreAMPDpp100.BackColorOdd = System.Drawing.Color.White;
            this.cboPreAMPDpp100.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cboPreAMPDpp100.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cboPreAMPDpp100.ColumnNames = "";
            this.cboPreAMPDpp100.ColumnWidthDefault = 75;
            this.cboPreAMPDpp100.ColumnWidths = "";
            this.cboPreAMPDpp100.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboPreAMPDpp100.FormattingEnabled = true;
            this.cboPreAMPDpp100.LinkedColumnIndex = 0;
            this.cboPreAMPDpp100.LinkedTextBox = null;
            this.cboPreAMPDpp100.Location = new System.Drawing.Point(566, 47);
            this.cboPreAMPDpp100.Name = "cboPreAMPDpp100";
            this.cboPreAMPDpp100.Size = new System.Drawing.Size(121, 22);
            this.cboPreAMPDpp100.TabIndex = 102;
            // 
            // lblPreAmp
            // 
            this.lblPreAmp.AutoSize = true;
            this.lblPreAmp.BackColor = System.Drawing.Color.Transparent;
            this.lblPreAmp.Location = new System.Drawing.Point(564, 32);
            this.lblPreAmp.Name = "lblPreAmp";
            this.lblPreAmp.Size = new System.Drawing.Size(41, 12);
            this.lblPreAmp.TabIndex = 101;
            this.lblPreAmp.Text = "PreAMP";
            // 
            // cboFlatTopDpp100
            // 
            this.cboFlatTopDpp100.AutoComplete = false;
            this.cboFlatTopDpp100.AutoDropdown = false;
            this.cboFlatTopDpp100.BackColorEven = System.Drawing.Color.White;
            this.cboFlatTopDpp100.BackColorOdd = System.Drawing.Color.White;
            this.cboFlatTopDpp100.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cboFlatTopDpp100.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cboFlatTopDpp100.ColumnNames = "";
            this.cboFlatTopDpp100.ColumnWidthDefault = 75;
            this.cboFlatTopDpp100.ColumnWidths = "";
            this.cboFlatTopDpp100.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboFlatTopDpp100.FormattingEnabled = true;
            this.cboFlatTopDpp100.LinkedColumnIndex = 0;
            this.cboFlatTopDpp100.LinkedTextBox = null;
            this.cboFlatTopDpp100.Location = new System.Drawing.Point(406, 46);
            this.cboFlatTopDpp100.Name = "cboFlatTopDpp100";
            this.cboFlatTopDpp100.Size = new System.Drawing.Size(121, 22);
            this.cboFlatTopDpp100.TabIndex = 100;
            // 
            // cboPeakingTimDpp100
            // 
            this.cboPeakingTimDpp100.AutoComplete = false;
            this.cboPeakingTimDpp100.AutoDropdown = false;
            this.cboPeakingTimDpp100.BackColorEven = System.Drawing.Color.White;
            this.cboPeakingTimDpp100.BackColorOdd = System.Drawing.Color.White;
            this.cboPeakingTimDpp100.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cboPeakingTimDpp100.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cboPeakingTimDpp100.ColumnNames = "";
            this.cboPeakingTimDpp100.ColumnWidthDefault = 75;
            this.cboPeakingTimDpp100.ColumnWidths = "";
            this.cboPeakingTimDpp100.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboPeakingTimDpp100.FormattingEnabled = true;
            this.cboPeakingTimDpp100.LinkedColumnIndex = 0;
            this.cboPeakingTimDpp100.LinkedTextBox = null;
            this.cboPeakingTimDpp100.Location = new System.Drawing.Point(250, 46);
            this.cboPeakingTimDpp100.Name = "cboPeakingTimDpp100";
            this.cboPeakingTimDpp100.Size = new System.Drawing.Size(121, 22);
            this.cboPeakingTimDpp100.TabIndex = 99;
            // 
            // cboGain
            // 
            this.cboGain.AutoComplete = false;
            this.cboGain.AutoDropdown = false;
            this.cboGain.BackColorEven = System.Drawing.Color.White;
            this.cboGain.BackColorOdd = System.Drawing.Color.White;
            this.cboGain.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cboGain.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cboGain.ColumnNames = "";
            this.cboGain.ColumnWidthDefault = 75;
            this.cboGain.ColumnWidths = "";
            this.cboGain.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboGain.FormattingEnabled = true;
            this.cboGain.LinkedColumnIndex = 0;
            this.cboGain.LinkedTextBox = null;
            this.cboGain.Location = new System.Drawing.Point(96, 95);
            this.cboGain.Name = "cboGain";
            this.cboGain.Size = new System.Drawing.Size(121, 22);
            this.cboGain.TabIndex = 98;
            // 
            // lblGain
            // 
            this.lblGain.AutoSize = true;
            this.lblGain.BackColor = System.Drawing.Color.Transparent;
            this.lblGain.Location = new System.Drawing.Point(97, 80);
            this.lblGain.Name = "lblGain";
            this.lblGain.Size = new System.Drawing.Size(29, 12);
            this.lblGain.TabIndex = 97;
            this.lblGain.Text = "粗调";
            // 
            // radioDMCA
            // 
            this.radioDMCA.Location = new System.Drawing.Point(13, 54);
            this.radioDMCA.Name = "radioDMCA";
            this.radioDMCA.Size = new System.Drawing.Size(74, 27);
            this.radioDMCA.TabIndex = 94;
            this.radioDMCA.TabStop = true;
            this.radioDMCA.Text = "DMCA";
            this.radioDMCA.UseVisualStyleBackColor = true;
            this.radioDMCA.CheckedChanged += new System.EventHandler(this.radioDMCA_CheckedChanged);
            // 
            // radioDpp100
            // 
            this.radioDpp100.Location = new System.Drawing.Point(13, 94);
            this.radioDpp100.Name = "radioDpp100";
            this.radioDpp100.Size = new System.Drawing.Size(74, 27);
            this.radioDpp100.TabIndex = 95;
            this.radioDpp100.Text = "Dpp100";
            this.radioDpp100.UseVisualStyleBackColor = true;
            // 
            // numIntercept
            // 
            this.numIntercept.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numIntercept.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numIntercept.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numIntercept.DecimalPlaces = 8;
            this.numIntercept.Location = new System.Drawing.Point(406, 96);
            this.numIntercept.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numIntercept.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numIntercept.Name = "numIntercept";
            this.numIntercept.Size = new System.Drawing.Size(120, 21);
            this.numIntercept.TabIndex = 85;
            this.numIntercept.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblIntercept
            // 
            this.lblIntercept.AutoSize = true;
            this.lblIntercept.BackColor = System.Drawing.Color.Transparent;
            this.lblIntercept.Location = new System.Drawing.Point(404, 81);
            this.lblIntercept.Name = "lblIntercept";
            this.lblIntercept.Size = new System.Drawing.Size(53, 12);
            this.lblIntercept.TabIndex = 84;
            this.lblIntercept.Text = "线性截距";
            // 
            // numFastLimit
            // 
            this.numFastLimit.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numFastLimit.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numFastLimit.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numFastLimit.Location = new System.Drawing.Point(250, 148);
            this.numFastLimit.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numFastLimit.Name = "numFastLimit";
            this.numFastLimit.Size = new System.Drawing.Size(120, 21);
            this.numFastLimit.TabIndex = 83;
            this.numFastLimit.Visible = false;
            // 
            // lblFastLimit
            // 
            this.lblFastLimit.AutoSize = true;
            this.lblFastLimit.BackColor = System.Drawing.Color.Transparent;
            this.lblFastLimit.Location = new System.Drawing.Point(247, 133);
            this.lblFastLimit.Name = "lblFastLimit";
            this.lblFastLimit.Size = new System.Drawing.Size(77, 12);
            this.lblFastLimit.TabIndex = 82;
            this.lblFastLimit.Text = "快成型门限值";
            this.lblFastLimit.Visible = false;
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.BackColor = System.Drawing.Color.Transparent;
            this.lblIP.Location = new System.Drawing.Point(94, 134);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(65, 12);
            this.lblIP.TabIndex = 81;
            this.lblIP.Text = "IP地址设置";
            // 
            // txtIP
            // 
            this.txtIP.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtIP.Location = new System.Drawing.Point(96, 149);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(121, 21);
            this.txtIP.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtIP.TabIndex = 80;
            // 
            // lblSlowLimit
            // 
            this.lblSlowLimit.AutoSize = true;
            this.lblSlowLimit.BackColor = System.Drawing.Color.Transparent;
            this.lblSlowLimit.Location = new System.Drawing.Point(247, 81);
            this.lblSlowLimit.Name = "lblSlowLimit";
            this.lblSlowLimit.Size = new System.Drawing.Size(77, 12);
            this.lblSlowLimit.TabIndex = 79;
            this.lblSlowLimit.Text = "慢成形门限值";
            // 
            // lblFlatTop
            // 
            this.lblFlatTop.AutoSize = true;
            this.lblFlatTop.BackColor = System.Drawing.Color.Transparent;
            this.lblFlatTop.Location = new System.Drawing.Point(404, 31);
            this.lblFlatTop.Name = "lblFlatTop";
            this.lblFlatTop.Size = new System.Drawing.Size(113, 12);
            this.lblFlatTop.TabIndex = 78;
            this.lblFlatTop.Text = "梯形顶宽时间寄存器";
            // 
            // lblPeakingTime
            // 
            this.lblPeakingTime.AutoSize = true;
            this.lblPeakingTime.BackColor = System.Drawing.Color.Transparent;
            this.lblPeakingTime.Location = new System.Drawing.Point(247, 31);
            this.lblPeakingTime.Name = "lblPeakingTime";
            this.lblPeakingTime.Size = new System.Drawing.Size(113, 12);
            this.lblPeakingTime.TabIndex = 77;
            this.lblPeakingTime.Text = "梯形上升时间寄存器";
            // 
            // lblRate
            // 
            this.lblRate.AutoSize = true;
            this.lblRate.BackColor = System.Drawing.Color.Transparent;
            this.lblRate.Location = new System.Drawing.Point(93, 80);
            this.lblRate.Name = "lblRate";
            this.lblRate.Size = new System.Drawing.Size(53, 12);
            this.lblRate.TabIndex = 76;
            this.lblRate.Text = "运行时钟";
            // 
            // lblHeapUP
            // 
            this.lblHeapUP.AutoSize = true;
            this.lblHeapUP.BackColor = System.Drawing.Color.Transparent;
            this.lblHeapUP.Location = new System.Drawing.Point(93, 31);
            this.lblHeapUP.Name = "lblHeapUP";
            this.lblHeapUP.Size = new System.Drawing.Size(125, 12);
            this.lblHeapUP.TabIndex = 75;
            this.lblHeapUP.Text = "堆积叛弃功能运行标志";
            // 
            // numSlowLimit
            // 
            this.numSlowLimit.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numSlowLimit.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numSlowLimit.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numSlowLimit.Location = new System.Drawing.Point(249, 96);
            this.numSlowLimit.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numSlowLimit.Name = "numSlowLimit";
            this.numSlowLimit.Size = new System.Drawing.Size(121, 21);
            this.numSlowLimit.TabIndex = 72;
            this.numSlowLimit.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // cboRate
            // 
            this.cboRate.AutoComplete = false;
            this.cboRate.AutoDropdown = false;
            this.cboRate.BackColorEven = System.Drawing.Color.White;
            this.cboRate.BackColorOdd = System.Drawing.Color.White;
            this.cboRate.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cboRate.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cboRate.ColumnNames = "";
            this.cboRate.ColumnWidthDefault = 75;
            this.cboRate.ColumnWidths = "";
            this.cboRate.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboRate.FormattingEnabled = true;
            this.cboRate.LinkedColumnIndex = 0;
            this.cboRate.LinkedTextBox = null;
            this.cboRate.Location = new System.Drawing.Point(96, 95);
            this.cboRate.Name = "cboRate";
            this.cboRate.Size = new System.Drawing.Size(121, 22);
            this.cboRate.TabIndex = 5;
            // 
            // cboFlatTop
            // 
            this.cboFlatTop.AutoComplete = false;
            this.cboFlatTop.AutoDropdown = false;
            this.cboFlatTop.BackColorEven = System.Drawing.Color.White;
            this.cboFlatTop.BackColorOdd = System.Drawing.Color.White;
            this.cboFlatTop.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cboFlatTop.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cboFlatTop.ColumnNames = "";
            this.cboFlatTop.ColumnWidthDefault = 75;
            this.cboFlatTop.ColumnWidths = "";
            this.cboFlatTop.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboFlatTop.FormattingEnabled = true;
            this.cboFlatTop.LinkedColumnIndex = 0;
            this.cboFlatTop.LinkedTextBox = null;
            this.cboFlatTop.Location = new System.Drawing.Point(406, 46);
            this.cboFlatTop.Name = "cboFlatTop";
            this.cboFlatTop.Size = new System.Drawing.Size(121, 22);
            this.cboFlatTop.TabIndex = 4;
            // 
            // cboPeakingTime
            // 
            this.cboPeakingTime.AutoComplete = false;
            this.cboPeakingTime.AutoDropdown = false;
            this.cboPeakingTime.BackColorEven = System.Drawing.Color.White;
            this.cboPeakingTime.BackColorOdd = System.Drawing.Color.White;
            this.cboPeakingTime.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cboPeakingTime.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cboPeakingTime.ColumnNames = "";
            this.cboPeakingTime.ColumnWidthDefault = 75;
            this.cboPeakingTime.ColumnWidths = "";
            this.cboPeakingTime.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboPeakingTime.FormattingEnabled = true;
            this.cboPeakingTime.LinkedColumnIndex = 0;
            this.cboPeakingTime.LinkedTextBox = null;
            this.cboPeakingTime.Location = new System.Drawing.Point(249, 46);
            this.cboPeakingTime.Name = "cboPeakingTime";
            this.cboPeakingTime.Size = new System.Drawing.Size(121, 22);
            this.cboPeakingTime.TabIndex = 3;
            this.cboPeakingTime.SelectedIndexChanged += new System.EventHandler(this.cboPeakingTime_SelectedIndexChanged);
            // 
            // cboHeapUP
            // 
            this.cboHeapUP.AutoComplete = false;
            this.cboHeapUP.AutoDropdown = false;
            this.cboHeapUP.BackColorEven = System.Drawing.Color.White;
            this.cboHeapUP.BackColorOdd = System.Drawing.Color.White;
            this.cboHeapUP.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cboHeapUP.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cboHeapUP.ColumnNames = "";
            this.cboHeapUP.ColumnWidthDefault = 75;
            this.cboHeapUP.ColumnWidths = "";
            this.cboHeapUP.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboHeapUP.FormattingEnabled = true;
            this.cboHeapUP.LinkedColumnIndex = 0;
            this.cboHeapUP.LinkedTextBox = null;
            this.cboHeapUP.Location = new System.Drawing.Point(96, 46);
            this.cboHeapUP.Name = "cboHeapUP";
            this.cboHeapUP.Size = new System.Drawing.Size(121, 22);
            this.cboHeapUP.TabIndex = 2;
            // 
            // grpBlueTooth
            // 
            this.grpBlueTooth.BackgroundColor = System.Drawing.Color.Transparent;
            this.grpBlueTooth.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grpBlueTooth.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grpBlueTooth.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grpBlueTooth.BorderThickness = 1F;
            this.grpBlueTooth.BorderTopOnly = false;
            this.grpBlueTooth.Controls.Add(this.cboPocket);
            this.grpBlueTooth.Controls.Add(this.lblPocket);
            this.grpBlueTooth.Controls.Add(this.cboBits);
            this.grpBlueTooth.Controls.Add(this.lblCom);
            this.grpBlueTooth.Controls.Add(this.numComNum);
            this.grpBlueTooth.Controls.Add(this.lblBits);
            this.grpBlueTooth.Controls.Add(this.lblComNum);
            this.grpBlueTooth.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grpBlueTooth.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Center;
            this.grpBlueTooth.GroupImage = null;
            this.grpBlueTooth.GroupTitle = "";
            this.grpBlueTooth.HeaderRoundCorners = 4;
            this.grpBlueTooth.Location = new System.Drawing.Point(10, 45);
            this.grpBlueTooth.Name = "grpBlueTooth";
            this.grpBlueTooth.PaintGroupBox = false;
            this.grpBlueTooth.RoundCorners = 4;
            this.grpBlueTooth.ShadowColor = System.Drawing.Color.DarkGray;
            this.grpBlueTooth.ShadowControl = false;
            this.grpBlueTooth.ShadowThickness = 3;
            this.grpBlueTooth.Size = new System.Drawing.Size(689, 188);
            this.grpBlueTooth.TabIndex = 113;
            this.grpBlueTooth.TextLineSpace = 2;
            this.grpBlueTooth.TitleLeftSpace = 18;
            // 
            // cboPocket
            // 
            this.cboPocket.AutoComplete = false;
            this.cboPocket.AutoDropdown = false;
            this.cboPocket.BackColorEven = System.Drawing.Color.White;
            this.cboPocket.BackColorOdd = System.Drawing.Color.White;
            this.cboPocket.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cboPocket.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cboPocket.ColumnNames = "";
            this.cboPocket.ColumnWidthDefault = 75;
            this.cboPocket.ColumnWidths = "";
            this.cboPocket.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboPocket.FormattingEnabled = true;
            this.cboPocket.LinkedColumnIndex = 0;
            this.cboPocket.LinkedTextBox = null;
            this.cboPocket.Location = new System.Drawing.Point(92, 41);
            this.cboPocket.Name = "cboPocket";
            this.cboPocket.Size = new System.Drawing.Size(152, 22);
            this.cboPocket.TabIndex = 6;
            // 
            // lblPocket
            // 
            this.lblPocket.AutoSize = true;
            this.lblPocket.BackColor = System.Drawing.Color.Transparent;
            this.lblPocket.Location = new System.Drawing.Point(25, 47);
            this.lblPocket.Name = "lblPocket";
            this.lblPocket.Size = new System.Drawing.Size(41, 12);
            this.lblPocket.TabIndex = 5;
            this.lblPocket.Text = "类型：";
            // 
            // cboBits
            // 
            this.cboBits.AutoComplete = false;
            this.cboBits.AutoDropdown = false;
            this.cboBits.BackColorEven = System.Drawing.Color.White;
            this.cboBits.BackColorOdd = System.Drawing.Color.White;
            this.cboBits.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cboBits.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cboBits.ColumnNames = "";
            this.cboBits.ColumnWidthDefault = 75;
            this.cboBits.ColumnWidths = "";
            this.cboBits.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboBits.FormattingEnabled = true;
            this.cboBits.LinkedColumnIndex = 0;
            this.cboBits.LinkedTextBox = null;
            this.cboBits.Location = new System.Drawing.Point(92, 131);
            this.cboBits.Name = "cboBits";
            this.cboBits.Size = new System.Drawing.Size(152, 22);
            this.cboBits.TabIndex = 4;
            // 
            // lblCom
            // 
            this.lblCom.AutoSize = true;
            this.lblCom.BackColor = System.Drawing.Color.Transparent;
            this.lblCom.Location = new System.Drawing.Point(90, 90);
            this.lblCom.Name = "lblCom";
            this.lblCom.Size = new System.Drawing.Size(23, 12);
            this.lblCom.TabIndex = 3;
            this.lblCom.Text = "Com";
            // 
            // numComNum
            // 
            this.numComNum.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numComNum.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numComNum.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numComNum.Location = new System.Drawing.Point(152, 85);
            this.numComNum.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numComNum.Name = "numComNum";
            this.numComNum.Size = new System.Drawing.Size(92, 21);
            this.numComNum.TabIndex = 2;
            // 
            // lblBits
            // 
            this.lblBits.AutoSize = true;
            this.lblBits.BackColor = System.Drawing.Color.Transparent;
            this.lblBits.Location = new System.Drawing.Point(26, 138);
            this.lblBits.Name = "lblBits";
            this.lblBits.Size = new System.Drawing.Size(53, 12);
            this.lblBits.TabIndex = 1;
            this.lblBits.Text = "波特率：";
            // 
            // lblComNum
            // 
            this.lblComNum.AutoSize = true;
            this.lblComNum.BackColor = System.Drawing.Color.Transparent;
            this.lblComNum.Location = new System.Drawing.Point(26, 91);
            this.lblComNum.Name = "lblComNum";
            this.lblComNum.Size = new System.Drawing.Size(53, 12);
            this.lblComNum.TabIndex = 0;
            this.lblComNum.Text = "端口号：";
            // 
            // grpUSB
            // 
            this.grpUSB.BackgroundColor = System.Drawing.Color.Transparent;
            this.grpUSB.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grpUSB.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grpUSB.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grpUSB.BorderThickness = 1F;
            this.grpUSB.BorderTopOnly = false;
            this.grpUSB.Controls.Add(this.lblDp5Type);
            this.grpUSB.Controls.Add(this.cboDp5Type);
            this.grpUSB.Controls.Add(this.grpDP5Params);
            this.grpUSB.Controls.Add(this.cboPortType);
            this.grpUSB.Controls.Add(this.comboBoxWVersion);
            this.grpUSB.Controls.Add(this.panel4);
            this.grpUSB.Controls.Add(this.panel5);
            this.grpUSB.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grpUSB.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Center;
            this.grpUSB.GroupImage = null;
            this.grpUSB.GroupTitle = "";
            this.grpUSB.HeaderRoundCorners = 4;
            this.grpUSB.Location = new System.Drawing.Point(10, 47);
            this.grpUSB.Name = "grpUSB";
            this.grpUSB.PaintGroupBox = false;
            this.grpUSB.RoundCorners = 4;
            this.grpUSB.ShadowColor = System.Drawing.Color.DarkGray;
            this.grpUSB.ShadowControl = false;
            this.grpUSB.ShadowThickness = 3;
            this.grpUSB.Size = new System.Drawing.Size(687, 188);
            this.grpUSB.TabIndex = 110;
            this.grpUSB.TextLineSpace = 2;
            this.grpUSB.TitleLeftSpace = 18;
            // 
            // lblDp5Type
            // 
            this.lblDp5Type.AutoSize = true;
            this.lblDp5Type.BackColor = System.Drawing.Color.Transparent;
            this.lblDp5Type.Location = new System.Drawing.Point(25, 149);
            this.lblDp5Type.Name = "lblDp5Type";
            this.lblDp5Type.Size = new System.Drawing.Size(47, 12);
            this.lblDp5Type.TabIndex = 101;
            this.lblDp5Type.Text = "DP5类型";
            // 
            // cboDp5Type
            // 
            this.cboDp5Type.AutoComplete = false;
            this.cboDp5Type.AutoDropdown = false;
            this.cboDp5Type.BackColorEven = System.Drawing.Color.White;
            this.cboDp5Type.BackColorOdd = System.Drawing.Color.White;
            this.cboDp5Type.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cboDp5Type.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cboDp5Type.ColumnNames = "";
            this.cboDp5Type.ColumnWidthDefault = 75;
            this.cboDp5Type.ColumnWidths = "";
            this.cboDp5Type.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboDp5Type.FormattingEnabled = true;
            this.cboDp5Type.LinkedColumnIndex = 0;
            this.cboDp5Type.LinkedTextBox = null;
            this.cboDp5Type.Location = new System.Drawing.Point(83, 143);
            this.cboDp5Type.Name = "cboDp5Type";
            this.cboDp5Type.Size = new System.Drawing.Size(106, 22);
            this.cboDp5Type.TabIndex = 100;
            this.cboDp5Type.SelectedIndexChanged += new System.EventHandler(this.cboDp5Type_SelectedIndexChanged);
            // 
            // grpDP5Params
            // 
            this.grpDP5Params.BackgroundColor = System.Drawing.Color.Transparent;
            this.grpDP5Params.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grpDP5Params.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grpDP5Params.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grpDP5Params.BorderThickness = 1F;
            this.grpDP5Params.BorderTopOnly = false;
            this.grpDP5Params.Controls.Add(this.lblDp5IP);
            this.grpDP5Params.Controls.Add(this.txtDp5Ip);
            this.grpDP5Params.Controls.Add(this.cboVoltage);
            this.grpDP5Params.Controls.Add(this.lblVoltage);
            this.grpDP5Params.Controls.Add(this.numDP5FastThreshold);
            this.grpDP5Params.Controls.Add(this.cboDP5HeapUP);
            this.grpDP5Params.Controls.Add(this.cboDP5FlatTop);
            this.grpDP5Params.Controls.Add(this.cboDP5PeakingTime);
            this.grpDP5Params.Controls.Add(this.lblDP5FastThreshold);
            this.grpDP5Params.Controls.Add(this.lblDP5HeapUP);
            this.grpDP5Params.Controls.Add(this.lblDP5FlatTop);
            this.grpDP5Params.Controls.Add(this.lblDP5PeakingTime);
            this.grpDP5Params.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grpDP5Params.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Left;
            this.grpDP5Params.GroupImage = null;
            this.grpDP5Params.GroupTitle = "DP5参数";
            this.grpDP5Params.HeaderRoundCorners = 4;
            this.grpDP5Params.Location = new System.Drawing.Point(259, 31);
            this.grpDP5Params.Name = "grpDP5Params";
            this.grpDP5Params.PaintGroupBox = false;
            this.grpDP5Params.RoundCorners = 4;
            this.grpDP5Params.ShadowColor = System.Drawing.Color.DarkGray;
            this.grpDP5Params.ShadowControl = false;
            this.grpDP5Params.ShadowThickness = 3;
            this.grpDP5Params.Size = new System.Drawing.Size(421, 147);
            this.grpDP5Params.TabIndex = 99;
            this.grpDP5Params.TextLineSpace = 2;
            this.grpDP5Params.TitleLeftSpace = 18;
            // 
            // lblDp5IP
            // 
            this.lblDp5IP.AutoSize = true;
            this.lblDp5IP.BackColor = System.Drawing.Color.Transparent;
            this.lblDp5IP.Location = new System.Drawing.Point(212, 115);
            this.lblDp5IP.Name = "lblDp5IP";
            this.lblDp5IP.Size = new System.Drawing.Size(23, 12);
            this.lblDp5IP.TabIndex = 83;
            this.lblDp5IP.Text = "IP:";
            // 
            // txtDp5Ip
            // 
            this.txtDp5Ip.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.txtDp5Ip.Location = new System.Drawing.Point(257, 111);
            this.txtDp5Ip.Name = "txtDp5Ip";
            this.txtDp5Ip.Size = new System.Drawing.Size(121, 21);
            this.txtDp5Ip.Style = Skyray.Controls.Style.Office2007Blue;
            this.txtDp5Ip.TabIndex = 82;
            // 
            // cboVoltage
            // 
            this.cboVoltage.AutoComplete = false;
            this.cboVoltage.AutoDropdown = false;
            this.cboVoltage.BackColorEven = System.Drawing.Color.White;
            this.cboVoltage.BackColorOdd = System.Drawing.Color.White;
            this.cboVoltage.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cboVoltage.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cboVoltage.ColumnNames = "";
            this.cboVoltage.ColumnWidthDefault = 75;
            this.cboVoltage.ColumnWidths = "";
            this.cboVoltage.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboVoltage.FormattingEnabled = true;
            this.cboVoltage.LinkedColumnIndex = 0;
            this.cboVoltage.LinkedTextBox = null;
            this.cboVoltage.Location = new System.Drawing.Point(114, 110);
            this.cboVoltage.Name = "cboVoltage";
            this.cboVoltage.Size = new System.Drawing.Size(80, 22);
            this.cboVoltage.TabIndex = 9;
            // 
            // lblVoltage
            // 
            this.lblVoltage.AutoSize = true;
            this.lblVoltage.BackColor = System.Drawing.Color.Transparent;
            this.lblVoltage.Location = new System.Drawing.Point(11, 116);
            this.lblVoltage.Name = "lblVoltage";
            this.lblVoltage.Size = new System.Drawing.Size(65, 12);
            this.lblVoltage.TabIndex = 8;
            this.lblVoltage.Text = "高压设置：";
            // 
            // numDP5FastThreshold
            // 
            this.numDP5FastThreshold.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numDP5FastThreshold.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numDP5FastThreshold.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numDP5FastThreshold.Location = new System.Drawing.Point(298, 71);
            this.numDP5FastThreshold.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numDP5FastThreshold.Name = "numDP5FastThreshold";
            this.numDP5FastThreshold.Size = new System.Drawing.Size(77, 21);
            this.numDP5FastThreshold.TabIndex = 7;
            // 
            // cboDP5HeapUP
            // 
            this.cboDP5HeapUP.AutoComplete = false;
            this.cboDP5HeapUP.AutoDropdown = false;
            this.cboDP5HeapUP.BackColorEven = System.Drawing.Color.White;
            this.cboDP5HeapUP.BackColorOdd = System.Drawing.Color.White;
            this.cboDP5HeapUP.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cboDP5HeapUP.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cboDP5HeapUP.ColumnNames = "";
            this.cboDP5HeapUP.ColumnWidthDefault = 75;
            this.cboDP5HeapUP.ColumnWidths = "";
            this.cboDP5HeapUP.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboDP5HeapUP.FormattingEnabled = true;
            this.cboDP5HeapUP.LinkedColumnIndex = 0;
            this.cboDP5HeapUP.LinkedTextBox = null;
            this.cboDP5HeapUP.Location = new System.Drawing.Point(298, 31);
            this.cboDP5HeapUP.Name = "cboDP5HeapUP";
            this.cboDP5HeapUP.Size = new System.Drawing.Size(77, 22);
            this.cboDP5HeapUP.TabIndex = 6;
            // 
            // cboDP5FlatTop
            // 
            this.cboDP5FlatTop.AutoComplete = false;
            this.cboDP5FlatTop.AutoDropdown = false;
            this.cboDP5FlatTop.BackColorEven = System.Drawing.Color.White;
            this.cboDP5FlatTop.BackColorOdd = System.Drawing.Color.White;
            this.cboDP5FlatTop.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cboDP5FlatTop.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cboDP5FlatTop.ColumnNames = "";
            this.cboDP5FlatTop.ColumnWidthDefault = 75;
            this.cboDP5FlatTop.ColumnWidths = "";
            this.cboDP5FlatTop.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboDP5FlatTop.FormattingEnabled = true;
            this.cboDP5FlatTop.LinkedColumnIndex = 0;
            this.cboDP5FlatTop.LinkedTextBox = null;
            this.cboDP5FlatTop.Location = new System.Drawing.Point(114, 71);
            this.cboDP5FlatTop.Name = "cboDP5FlatTop";
            this.cboDP5FlatTop.Size = new System.Drawing.Size(80, 22);
            this.cboDP5FlatTop.TabIndex = 5;
            // 
            // cboDP5PeakingTime
            // 
            this.cboDP5PeakingTime.AutoComplete = false;
            this.cboDP5PeakingTime.AutoDropdown = false;
            this.cboDP5PeakingTime.BackColorEven = System.Drawing.Color.White;
            this.cboDP5PeakingTime.BackColorOdd = System.Drawing.Color.White;
            this.cboDP5PeakingTime.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cboDP5PeakingTime.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cboDP5PeakingTime.ColumnNames = "";
            this.cboDP5PeakingTime.ColumnWidthDefault = 75;
            this.cboDP5PeakingTime.ColumnWidths = "";
            this.cboDP5PeakingTime.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboDP5PeakingTime.FormattingEnabled = true;
            this.cboDP5PeakingTime.LinkedColumnIndex = 0;
            this.cboDP5PeakingTime.LinkedTextBox = null;
            this.cboDP5PeakingTime.Location = new System.Drawing.Point(114, 32);
            this.cboDP5PeakingTime.Name = "cboDP5PeakingTime";
            this.cboDP5PeakingTime.Size = new System.Drawing.Size(80, 22);
            this.cboDP5PeakingTime.TabIndex = 4;
            this.cboDP5PeakingTime.SelectedIndexChanged += new System.EventHandler(this.cboDP5PeakingTime_SelectedIndexChanged);
            // 
            // lblDP5FastThreshold
            // 
            this.lblDP5FastThreshold.AutoSize = true;
            this.lblDP5FastThreshold.BackColor = System.Drawing.Color.Transparent;
            this.lblDP5FastThreshold.Location = new System.Drawing.Point(206, 77);
            this.lblDP5FastThreshold.Name = "lblDP5FastThreshold";
            this.lblDP5FastThreshold.Size = new System.Drawing.Size(77, 12);
            this.lblDP5FastThreshold.TabIndex = 3;
            this.lblDP5FastThreshold.Text = "快成型下阀：";
            // 
            // lblDP5HeapUP
            // 
            this.lblDP5HeapUP.AutoSize = true;
            this.lblDP5HeapUP.BackColor = System.Drawing.Color.Transparent;
            this.lblDP5HeapUP.Location = new System.Drawing.Point(206, 39);
            this.lblDP5HeapUP.Name = "lblDP5HeapUP";
            this.lblDP5HeapUP.Size = new System.Drawing.Size(65, 12);
            this.lblDP5HeapUP.TabIndex = 2;
            this.lblDP5HeapUP.Text = "堆积叛弃：";
            // 
            // lblDP5FlatTop
            // 
            this.lblDP5FlatTop.AutoSize = true;
            this.lblDP5FlatTop.BackColor = System.Drawing.Color.Transparent;
            this.lblDP5FlatTop.Location = new System.Drawing.Point(11, 77);
            this.lblDP5FlatTop.Name = "lblDP5FlatTop";
            this.lblDP5FlatTop.Size = new System.Drawing.Size(89, 12);
            this.lblDP5FlatTop.TabIndex = 1;
            this.lblDP5FlatTop.Text = "梯形顶宽时间：";
            // 
            // lblDP5PeakingTime
            // 
            this.lblDP5PeakingTime.AutoSize = true;
            this.lblDP5PeakingTime.BackColor = System.Drawing.Color.Transparent;
            this.lblDP5PeakingTime.Location = new System.Drawing.Point(11, 37);
            this.lblDP5PeakingTime.Name = "lblDP5PeakingTime";
            this.lblDP5PeakingTime.Size = new System.Drawing.Size(89, 12);
            this.lblDP5PeakingTime.TabIndex = 0;
            this.lblDP5PeakingTime.Text = "梯形上升时间：";
            // 
            // cboPortType
            // 
            this.cboPortType.AutoComplete = false;
            this.cboPortType.AutoDropdown = false;
            this.cboPortType.BackColorEven = System.Drawing.Color.White;
            this.cboPortType.BackColorOdd = System.Drawing.Color.White;
            this.cboPortType.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cboPortType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cboPortType.ColumnNames = "";
            this.cboPortType.ColumnWidthDefault = 75;
            this.cboPortType.ColumnWidths = "";
            this.cboPortType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboPortType.FormattingEnabled = true;
            this.cboPortType.LinkedColumnIndex = 0;
            this.cboPortType.LinkedTextBox = null;
            this.cboPortType.Location = new System.Drawing.Point(25, 28);
            this.cboPortType.Name = "cboPortType";
            this.cboPortType.Size = new System.Drawing.Size(104, 22);
            this.cboPortType.TabIndex = 85;
            // 
            // comboBoxWVersion
            // 
            this.comboBoxWVersion.AutoComplete = false;
            this.comboBoxWVersion.AutoDropdown = false;
            this.comboBoxWVersion.BackColorEven = System.Drawing.Color.White;
            this.comboBoxWVersion.BackColorOdd = System.Drawing.Color.White;
            this.comboBoxWVersion.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.comboBoxWVersion.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.comboBoxWVersion.ColumnNames = "";
            this.comboBoxWVersion.ColumnWidthDefault = 75;
            this.comboBoxWVersion.ColumnWidths = "";
            this.comboBoxWVersion.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxWVersion.FormattingEnabled = true;
            this.comboBoxWVersion.LinkedColumnIndex = 0;
            this.comboBoxWVersion.LinkedTextBox = null;
            this.comboBoxWVersion.Location = new System.Drawing.Point(145, 28);
            this.comboBoxWVersion.Name = "comboBoxWVersion";
            this.comboBoxWVersion.Size = new System.Drawing.Size(104, 22);
            this.comboBoxWVersion.TabIndex = 90;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.lblIsPassword);
            this.panel4.Controls.Add(this.radIsPassward);
            this.panel4.Controls.Add(this.radIsPassward2);
            this.panel4.Location = new System.Drawing.Point(10, 63);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(203, 28);
            this.panel4.TabIndex = 95;
            // 
            // lblIsPassword
            // 
            this.lblIsPassword.AutoSize = true;
            this.lblIsPassword.BackColor = System.Drawing.Color.Transparent;
            this.lblIsPassword.Location = new System.Drawing.Point(17, 8);
            this.lblIsPassword.Name = "lblIsPassword";
            this.lblIsPassword.Size = new System.Drawing.Size(29, 12);
            this.lblIsPassword.TabIndex = 94;
            this.lblIsPassword.Text = "加密";
            // 
            // radIsPassward
            // 
            this.radIsPassward.Location = new System.Drawing.Point(73, 2);
            this.radIsPassward.Name = "radIsPassward";
            this.radIsPassward.Size = new System.Drawing.Size(48, 24);
            this.radIsPassward.TabIndex = 92;
            this.radIsPassward.TabStop = true;
            this.radIsPassward.Text = "是";
            this.radIsPassward.UseVisualStyleBackColor = true;
            // 
            // radIsPassward2
            // 
            this.radIsPassward2.Location = new System.Drawing.Point(135, 2);
            this.radIsPassward2.Name = "radIsPassward2";
            this.radIsPassward2.Size = new System.Drawing.Size(48, 24);
            this.radIsPassward2.TabIndex = 93;
            this.radIsPassward2.TabStop = true;
            this.radIsPassward2.Text = "否";
            this.radIsPassward2.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.lblDP5);
            this.panel5.Controls.Add(this.radIsDP5);
            this.panel5.Controls.Add(this.radIsNotDP5);
            this.panel5.Location = new System.Drawing.Point(10, 104);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(203, 28);
            this.panel5.TabIndex = 98;
            // 
            // lblDP5
            // 
            this.lblDP5.AutoSize = true;
            this.lblDP5.BackColor = System.Drawing.Color.Transparent;
            this.lblDP5.Location = new System.Drawing.Point(17, 8);
            this.lblDP5.Name = "lblDP5";
            this.lblDP5.Size = new System.Drawing.Size(23, 12);
            this.lblDP5.TabIndex = 94;
            this.lblDP5.Text = "DP5";
            // 
            // radIsDP5
            // 
            this.radIsDP5.Location = new System.Drawing.Point(73, 2);
            this.radIsDP5.Name = "radIsDP5";
            this.radIsDP5.Size = new System.Drawing.Size(48, 24);
            this.radIsDP5.TabIndex = 92;
            this.radIsDP5.TabStop = true;
            this.radIsDP5.Text = "是";
            this.radIsDP5.UseVisualStyleBackColor = true;
            this.radIsDP5.CheckedChanged += new System.EventHandler(this.radIsDP5_CheckedChanged);
            // 
            // radIsNotDP5
            // 
            this.radIsNotDP5.Location = new System.Drawing.Point(135, 2);
            this.radIsNotDP5.Name = "radIsNotDP5";
            this.radIsNotDP5.Size = new System.Drawing.Size(48, 24);
            this.radIsNotDP5.TabIndex = 93;
            this.radIsNotDP5.Text = "否";
            this.radIsNotDP5.UseVisualStyleBackColor = true;
            // 
            // grpParallel
            // 
            this.grpParallel.BackgroundColor = System.Drawing.Color.Transparent;
            this.grpParallel.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grpParallel.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grpParallel.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grpParallel.BorderThickness = 1F;
            this.grpParallel.BorderTopOnly = false;
            this.grpParallel.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grpParallel.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Center;
            this.grpParallel.GroupImage = null;
            this.grpParallel.GroupTitle = "";
            this.grpParallel.HeaderRoundCorners = 4;
            this.grpParallel.Location = new System.Drawing.Point(10, 48);
            this.grpParallel.Name = "grpParallel";
            this.grpParallel.PaintGroupBox = false;
            this.grpParallel.RoundCorners = 4;
            this.grpParallel.ShadowColor = System.Drawing.Color.DarkGray;
            this.grpParallel.ShadowControl = false;
            this.grpParallel.ShadowThickness = 3;
            this.grpParallel.Size = new System.Drawing.Size(689, 188);
            this.grpParallel.TabIndex = 7;
            this.grpParallel.TextLineSpace = 2;
            this.grpParallel.TitleLeftSpace = 18;
            // 
            // radParallel
            // 
            this.radParallel.Location = new System.Drawing.Point(477, 20);
            this.radParallel.Name = "radParallel";
            this.radParallel.Size = new System.Drawing.Size(89, 27);
            this.radParallel.TabIndex = 115;
            this.radParallel.TabStop = true;
            this.radParallel.Text = "Parallel";
            this.radParallel.UseVisualStyleBackColor = true;
            this.radParallel.CheckedChanged += new System.EventHandler(this.radParallel_CheckedChanged);
            // 
            // grpDevice
            // 
            this.grpDevice.BackgroundColor = System.Drawing.Color.Transparent;
            this.grpDevice.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grpDevice.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.grpDevice.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.grpDevice.BorderThickness = 1F;
            this.grpDevice.BorderTopOnly = false;
            this.grpDevice.Controls.Add(this.btnSampleCal);
            this.grpDevice.Controls.Add(this.chkAutoDetection);
            this.grpDevice.Controls.Add(this.pnlVacuum);
            this.grpDevice.Controls.Add(this.diMaxCurrent);
            this.grpDevice.Controls.Add(this.diMaxVoltage);
            this.grpDevice.Controls.Add(this.lblMaxCurrent);
            this.grpDevice.Controls.Add(this.lblMaxVoltage);
            this.grpDevice.Controls.Add(this.panel3);
            this.grpDevice.Controls.Add(this.panel2);
            this.grpDevice.Controls.Add(this.panel1);
            this.grpDevice.Controls.Add(this.cmbSpecLength);
            this.grpDevice.Controls.Add(this.lblVoltageScaleFactor);
            this.grpDevice.Controls.Add(this.numVoltageScaleFactor);
            this.grpDevice.Controls.Add(this.lblSpecLength);
            this.grpDevice.Controls.Add(this.numCurrentScaleFactor);
            this.grpDevice.Controls.Add(this.lblCurrentScaleFactor);
            this.grpDevice.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.grpDevice.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Left;
            this.grpDevice.GroupImage = null;
            this.grpDevice.GroupTitle = "仪器";
            this.grpDevice.HeaderRoundCorners = 4;
            this.grpDevice.Location = new System.Drawing.Point(10, 267);
            this.grpDevice.Name = "grpDevice";
            this.grpDevice.PaintGroupBox = false;
            this.grpDevice.RoundCorners = 4;
            this.grpDevice.ShadowColor = System.Drawing.Color.DarkGray;
            this.grpDevice.ShadowControl = false;
            this.grpDevice.ShadowThickness = 3;
            this.grpDevice.Size = new System.Drawing.Size(689, 244);
            this.grpDevice.TabIndex = 114;
            this.grpDevice.TextLineSpace = 2;
            this.grpDevice.TitleLeftSpace = 18;
            // 
            // btnSampleCal
            // 
            this.btnSampleCal.bSilver = false;
            this.btnSampleCal.Location = new System.Drawing.Point(272, 218);
            this.btnSampleCal.MaxImageSize = new System.Drawing.Point(0, 0);
            this.btnSampleCal.MenuPos = new System.Drawing.Point(0, 0);
            this.btnSampleCal.Name = "btnSampleCal";
            this.btnSampleCal.Size = new System.Drawing.Size(118, 23);
            this.btnSampleCal.Style = Skyray.Controls.Style.Office2007Blue;
            this.btnSampleCal.TabIndex = 47;
            this.btnSampleCal.Text = "自检设置";
            this.btnSampleCal.ToFocused = false;
            this.btnSampleCal.UseVisualStyleBackColor = true;
            this.btnSampleCal.Click += new System.EventHandler(this.btnSampleCal_Click);
            // 
            // chkAutoDetection
            // 
            this.chkAutoDetection.AutoSize = true;
            this.chkAutoDetection.BackColor = System.Drawing.Color.Transparent;
            this.chkAutoDetection.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkAutoDetection.Location = new System.Drawing.Point(43, 218);
            this.chkAutoDetection.Name = "chkAutoDetection";
            this.chkAutoDetection.Size = new System.Drawing.Size(72, 16);
            this.chkAutoDetection.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkAutoDetection.TabIndex = 46;
            this.chkAutoDetection.Text = "仪器自检";
            this.chkAutoDetection.UseVisualStyleBackColor = false;
            this.chkAutoDetection.Visible = false;
            // 
            // pnlVacuum
            // 
            this.pnlVacuum.Controls.Add(this.lblVacuumPumpType);
            this.pnlVacuum.Controls.Add(this.cboVacuumPumpType);
            this.pnlVacuum.Location = new System.Drawing.Point(492, 150);
            this.pnlVacuum.Name = "pnlVacuum";
            this.pnlVacuum.Size = new System.Drawing.Size(150, 58);
            this.pnlVacuum.TabIndex = 25;
            // 
            // lblVacuumPumpType
            // 
            this.lblVacuumPumpType.AutoSize = true;
            this.lblVacuumPumpType.BackColor = System.Drawing.Color.Transparent;
            this.lblVacuumPumpType.Location = new System.Drawing.Point(7, 3);
            this.lblVacuumPumpType.Name = "lblVacuumPumpType";
            this.lblVacuumPumpType.Size = new System.Drawing.Size(89, 12);
            this.lblVacuumPumpType.TabIndex = 17;
            this.lblVacuumPumpType.Text = "真空度显示类型";
            // 
            // cboVacuumPumpType
            // 
            this.cboVacuumPumpType.AutoComplete = false;
            this.cboVacuumPumpType.AutoDropdown = false;
            this.cboVacuumPumpType.BackColorEven = System.Drawing.Color.White;
            this.cboVacuumPumpType.BackColorOdd = System.Drawing.Color.White;
            this.cboVacuumPumpType.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cboVacuumPumpType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cboVacuumPumpType.ColumnNames = "";
            this.cboVacuumPumpType.ColumnWidthDefault = 75;
            this.cboVacuumPumpType.ColumnWidths = "";
            this.cboVacuumPumpType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboVacuumPumpType.FormattingEnabled = true;
            this.cboVacuumPumpType.LinkedColumnIndex = 0;
            this.cboVacuumPumpType.LinkedTextBox = null;
            this.cboVacuumPumpType.Location = new System.Drawing.Point(7, 26);
            this.cboVacuumPumpType.Name = "cboVacuumPumpType";
            this.cboVacuumPumpType.Size = new System.Drawing.Size(137, 22);
            this.cboVacuumPumpType.TabIndex = 6;
            // 
            // diMaxCurrent
            // 
            this.diMaxCurrent.BorderColor = System.Drawing.Color.Empty;
            this.diMaxCurrent.Location = new System.Drawing.Point(279, 115);
            this.diMaxCurrent.Name = "diMaxCurrent";
            this.diMaxCurrent.Size = new System.Drawing.Size(120, 21);
            this.diMaxCurrent.TabIndex = 24;
            // 
            // diMaxVoltage
            // 
            this.diMaxVoltage.BorderColor = System.Drawing.Color.Empty;
            this.diMaxVoltage.Location = new System.Drawing.Point(49, 115);
            this.diMaxVoltage.Name = "diMaxVoltage";
            this.diMaxVoltage.Size = new System.Drawing.Size(120, 21);
            this.diMaxVoltage.TabIndex = 23;
            // 
            // lblMaxCurrent
            // 
            this.lblMaxCurrent.AutoSize = true;
            this.lblMaxCurrent.BackColor = System.Drawing.Color.Transparent;
            this.lblMaxCurrent.Location = new System.Drawing.Point(279, 100);
            this.lblMaxCurrent.Name = "lblMaxCurrent";
            this.lblMaxCurrent.Size = new System.Drawing.Size(53, 12);
            this.lblMaxCurrent.TabIndex = 21;
            this.lblMaxCurrent.Text = "管流上限";
            // 
            // lblMaxVoltage
            // 
            this.lblMaxVoltage.AutoSize = true;
            this.lblMaxVoltage.BackColor = System.Drawing.Color.Transparent;
            this.lblMaxVoltage.Location = new System.Drawing.Point(49, 100);
            this.lblMaxVoltage.Name = "lblMaxVoltage";
            this.lblMaxVoltage.Size = new System.Drawing.Size(53, 12);
            this.lblMaxVoltage.TabIndex = 19;
            this.lblMaxVoltage.Text = "管压上限";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.radIsAllowOpenCover2);
            this.panel3.Controls.Add(this.lblIsAllowOpenCover);
            this.panel3.Controls.Add(this.radIsAllowOpenCover);
            this.panel3.Location = new System.Drawing.Point(43, 165);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(170, 36);
            this.panel3.TabIndex = 18;
            // 
            // radIsAllowOpenCover2
            // 
            this.radIsAllowOpenCover2.AutoSize = true;
            this.radIsAllowOpenCover2.Location = new System.Drawing.Point(113, 13);
            this.radIsAllowOpenCover2.Name = "radIsAllowOpenCover2";
            this.radIsAllowOpenCover2.Size = new System.Drawing.Size(35, 16);
            this.radIsAllowOpenCover2.TabIndex = 20;
            this.radIsAllowOpenCover2.TabStop = true;
            this.radIsAllowOpenCover2.Text = "否";
            this.radIsAllowOpenCover2.UseVisualStyleBackColor = true;
            // 
            // lblIsAllowOpenCover
            // 
            this.lblIsAllowOpenCover.AutoSize = true;
            this.lblIsAllowOpenCover.BackColor = System.Drawing.Color.Transparent;
            this.lblIsAllowOpenCover.Location = new System.Drawing.Point(2, 15);
            this.lblIsAllowOpenCover.Name = "lblIsAllowOpenCover";
            this.lblIsAllowOpenCover.Size = new System.Drawing.Size(53, 12);
            this.lblIsAllowOpenCover.TabIndex = 19;
            this.lblIsAllowOpenCover.Text = "开盖测试";
            // 
            // radIsAllowOpenCover
            // 
            this.radIsAllowOpenCover.AutoSize = true;
            this.radIsAllowOpenCover.Checked = true;
            this.radIsAllowOpenCover.Location = new System.Drawing.Point(67, 13);
            this.radIsAllowOpenCover.Name = "radIsAllowOpenCover";
            this.radIsAllowOpenCover.Size = new System.Drawing.Size(35, 16);
            this.radIsAllowOpenCover.TabIndex = 0;
            this.radIsAllowOpenCover.TabStop = true;
            this.radIsAllowOpenCover.Text = "是";
            this.radIsAllowOpenCover.UseVisualStyleBackColor = true;
            this.radIsAllowOpenCover.CheckedChanged += new System.EventHandler(this.radIsAllowOpenCover_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblHasElectromagnet);
            this.panel2.Controls.Add(this.radHasElectromagnet);
            this.panel2.Controls.Add(this.radHasElectromagnet2);
            this.panel2.Location = new System.Drawing.Point(273, 165);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(167, 36);
            this.panel2.TabIndex = 17;
            // 
            // lblHasElectromagnet
            // 
            this.lblHasElectromagnet.AutoSize = true;
            this.lblHasElectromagnet.BackColor = System.Drawing.Color.Transparent;
            this.lblHasElectromagnet.Location = new System.Drawing.Point(3, 15);
            this.lblHasElectromagnet.Name = "lblHasElectromagnet";
            this.lblHasElectromagnet.Size = new System.Drawing.Size(41, 12);
            this.lblHasElectromagnet.TabIndex = 16;
            this.lblHasElectromagnet.Text = "电子锁";
            // 
            // radHasElectromagnet
            // 
            this.radHasElectromagnet.AutoSize = true;
            this.radHasElectromagnet.Checked = true;
            this.radHasElectromagnet.Location = new System.Drawing.Point(66, 13);
            this.radHasElectromagnet.Name = "radHasElectromagnet";
            this.radHasElectromagnet.Size = new System.Drawing.Size(35, 16);
            this.radHasElectromagnet.TabIndex = 12;
            this.radHasElectromagnet.TabStop = true;
            this.radHasElectromagnet.Text = "有";
            this.radHasElectromagnet.UseVisualStyleBackColor = true;
            // 
            // radHasElectromagnet2
            // 
            this.radHasElectromagnet2.AutoSize = true;
            this.radHasElectromagnet2.Location = new System.Drawing.Point(113, 13);
            this.radHasElectromagnet2.Name = "radHasElectromagnet2";
            this.radHasElectromagnet2.Size = new System.Drawing.Size(35, 16);
            this.radHasElectromagnet2.TabIndex = 13;
            this.radHasElectromagnet2.TabStop = true;
            this.radHasElectromagnet2.Text = "无";
            this.radHasElectromagnet2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radVacuumPump);
            this.panel1.Controls.Add(this.radVacuumPump2);
            this.panel1.Controls.Add(this.lblVacuumPump);
            this.panel1.Location = new System.Drawing.Point(492, 103);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(159, 36);
            this.panel1.TabIndex = 15;
            // 
            // radVacuumPump
            // 
            this.radVacuumPump.AutoSize = true;
            this.radVacuumPump.Checked = true;
            this.radVacuumPump.Location = new System.Drawing.Point(74, 13);
            this.radVacuumPump.Name = "radVacuumPump";
            this.radVacuumPump.Size = new System.Drawing.Size(35, 16);
            this.radVacuumPump.TabIndex = 9;
            this.radVacuumPump.TabStop = true;
            this.radVacuumPump.Text = "有";
            this.radVacuumPump.UseVisualStyleBackColor = true;
            this.radVacuumPump.CheckedChanged += new System.EventHandler(this.radVacuumPump_CheckedChanged);
            // 
            // radVacuumPump2
            // 
            this.radVacuumPump2.AutoSize = true;
            this.radVacuumPump2.Location = new System.Drawing.Point(117, 13);
            this.radVacuumPump2.Name = "radVacuumPump2";
            this.radVacuumPump2.Size = new System.Drawing.Size(35, 16);
            this.radVacuumPump2.TabIndex = 10;
            this.radVacuumPump2.TabStop = true;
            this.radVacuumPump2.Text = "无";
            this.radVacuumPump2.UseVisualStyleBackColor = true;
            // 
            // lblVacuumPump
            // 
            this.lblVacuumPump.AutoSize = true;
            this.lblVacuumPump.BackColor = System.Drawing.Color.Transparent;
            this.lblVacuumPump.Location = new System.Drawing.Point(6, 15);
            this.lblVacuumPump.Name = "lblVacuumPump";
            this.lblVacuumPump.Size = new System.Drawing.Size(41, 12);
            this.lblVacuumPump.TabIndex = 11;
            this.lblVacuumPump.Text = "真空泵";
            // 
            // cmbSpecLength
            // 
            this.cmbSpecLength.AutoComplete = false;
            this.cmbSpecLength.AutoDropdown = false;
            this.cmbSpecLength.BackColorEven = System.Drawing.Color.White;
            this.cmbSpecLength.BackColorOdd = System.Drawing.Color.White;
            this.cmbSpecLength.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.cmbSpecLength.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.cmbSpecLength.ColumnNames = "";
            this.cmbSpecLength.ColumnWidthDefault = 75;
            this.cmbSpecLength.ColumnWidths = "";
            this.cmbSpecLength.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbSpecLength.FormattingEnabled = true;
            this.cmbSpecLength.LinkedColumnIndex = 0;
            this.cmbSpecLength.LinkedTextBox = null;
            this.cmbSpecLength.Location = new System.Drawing.Point(498, 49);
            this.cmbSpecLength.Name = "cmbSpecLength";
            this.cmbSpecLength.Size = new System.Drawing.Size(121, 22);
            this.cmbSpecLength.TabIndex = 5;
            // 
            // lblVoltageScaleFactor
            // 
            this.lblVoltageScaleFactor.AutoSize = true;
            this.lblVoltageScaleFactor.BackColor = System.Drawing.Color.Transparent;
            this.lblVoltageScaleFactor.Location = new System.Drawing.Point(49, 32);
            this.lblVoltageScaleFactor.Name = "lblVoltageScaleFactor";
            this.lblVoltageScaleFactor.Size = new System.Drawing.Size(77, 12);
            this.lblVoltageScaleFactor.TabIndex = 4;
            this.lblVoltageScaleFactor.Text = "管压比例因子";
            // 
            // numVoltageScaleFactor
            // 
            this.numVoltageScaleFactor.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numVoltageScaleFactor.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numVoltageScaleFactor.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numVoltageScaleFactor.Location = new System.Drawing.Point(49, 50);
            this.numVoltageScaleFactor.Name = "numVoltageScaleFactor";
            this.numVoltageScaleFactor.Size = new System.Drawing.Size(120, 21);
            this.numVoltageScaleFactor.TabIndex = 3;
            // 
            // lblSpecLength
            // 
            this.lblSpecLength.AutoSize = true;
            this.lblSpecLength.BackColor = System.Drawing.Color.Transparent;
            this.lblSpecLength.Location = new System.Drawing.Point(498, 32);
            this.lblSpecLength.Name = "lblSpecLength";
            this.lblSpecLength.Size = new System.Drawing.Size(41, 12);
            this.lblSpecLength.TabIndex = 2;
            this.lblSpecLength.Text = "谱长度";
            // 
            // numCurrentScaleFactor
            // 
            this.numCurrentScaleFactor.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numCurrentScaleFactor.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numCurrentScaleFactor.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numCurrentScaleFactor.Location = new System.Drawing.Point(279, 50);
            this.numCurrentScaleFactor.Name = "numCurrentScaleFactor";
            this.numCurrentScaleFactor.Size = new System.Drawing.Size(120, 21);
            this.numCurrentScaleFactor.TabIndex = 1;
            // 
            // lblCurrentScaleFactor
            // 
            this.lblCurrentScaleFactor.AutoSize = true;
            this.lblCurrentScaleFactor.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrentScaleFactor.Location = new System.Drawing.Point(279, 32);
            this.lblCurrentScaleFactor.Name = "lblCurrentScaleFactor";
            this.lblCurrentScaleFactor.Size = new System.Drawing.Size(77, 12);
            this.lblCurrentScaleFactor.TabIndex = 0;
            this.lblCurrentScaleFactor.Text = "管流比例因子";
            // 
            // radBlueTooth
            // 
            this.radBlueTooth.Location = new System.Drawing.Point(336, 20);
            this.radBlueTooth.Name = "radBlueTooth";
            this.radBlueTooth.Size = new System.Drawing.Size(95, 27);
            this.radBlueTooth.TabIndex = 112;
            this.radBlueTooth.TabStop = true;
            this.radBlueTooth.Text = "BlueTooth";
            this.radBlueTooth.UseVisualStyleBackColor = true;
            this.radBlueTooth.CheckedChanged += new System.EventHandler(this.radBlueTooth_CheckedChanged);
            // 
            // radFPGA
            // 
            this.radFPGA.Location = new System.Drawing.Point(225, 20);
            this.radFPGA.Name = "radFPGA";
            this.radFPGA.Size = new System.Drawing.Size(65, 27);
            this.radFPGA.TabIndex = 109;
            this.radFPGA.TabStop = true;
            this.radFPGA.Text = "FPGA";
            this.radFPGA.UseVisualStyleBackColor = true;
            this.radFPGA.CheckedChanged += new System.EventHandler(this.radFPGA_CheckedChanged);
            // 
            // radUSB
            // 
            this.radUSB.Location = new System.Drawing.Point(113, 20);
            this.radUSB.Name = "radUSB";
            this.radUSB.Size = new System.Drawing.Size(66, 27);
            this.radUSB.TabIndex = 108;
            this.radUSB.TabStop = true;
            this.radUSB.Text = "USB";
            this.radUSB.UseVisualStyleBackColor = true;
            this.radUSB.CheckedChanged += new System.EventHandler(this.radUSB_CheckedChanged);
            // 
            // lblPortType
            // 
            this.lblPortType.AutoSize = true;
            this.lblPortType.BackColor = System.Drawing.Color.Transparent;
            this.lblPortType.Location = new System.Drawing.Point(32, 27);
            this.lblPortType.Name = "lblPortType";
            this.lblPortType.Size = new System.Drawing.Size(53, 12);
            this.lblPortType.TabIndex = 107;
            this.lblPortType.Text = "通讯类型";
            // 
            // tpFilter
            // 
            this.tpFilter.BackColor = System.Drawing.Color.GhostWhite;
            this.tpFilter.Controls.Add(this.numFilterSpeed);
            this.tpFilter.Controls.Add(this.lblFilterSpeed);
            this.tpFilter.Controls.Add(this.numCollimatorSpeed);
            this.tpFilter.Controls.Add(this.lblCollimatorSpeed);
            this.tpFilter.Controls.Add(this.numFilterMaxNum);
            this.tpFilter.Controls.Add(this.lblFilterMaxNum);
            this.tpFilter.Controls.Add(this.numCollimatorMaxNum);
            this.tpFilter.Controls.Add(this.lblCollimatorMaxNum);
            this.tpFilter.Controls.Add(this.numFilterDirect);
            this.tpFilter.Controls.Add(this.lblFilterDirect);
            this.tpFilter.Controls.Add(this.numFilterCode);
            this.tpFilter.Controls.Add(this.lblFilterCode);
            this.tpFilter.Controls.Add(this.numCollimatorDirect);
            this.tpFilter.Controls.Add(this.lblCollimatorDirect);
            this.tpFilter.Controls.Add(this.numCollimatorCode);
            this.tpFilter.Controls.Add(this.lblCollimatorCode);
            this.tpFilter.Controls.Add(this.chkFilter);
            this.tpFilter.Controls.Add(this.chkCollimator);
            this.tpFilter.Controls.Add(this.dgvwFilter);
            this.tpFilter.Controls.Add(this.dgvwCollimator);
            this.tpFilter.Location = new System.Drawing.Point(4, 26);
            this.tpFilter.Name = "tpFilter";
            this.tpFilter.Padding = new System.Windows.Forms.Padding(3);
            this.tpFilter.Size = new System.Drawing.Size(192, 70);
            this.tpFilter.TabIndex = 1;
            this.tpFilter.Text = "准直器/滤光片";
            this.tpFilter.UseVisualStyleBackColor = true;
            // 
            // numFilterSpeed
            // 
            this.numFilterSpeed.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numFilterSpeed.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numFilterSpeed.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numFilterSpeed.Location = new System.Drawing.Point(601, 434);
            this.numFilterSpeed.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numFilterSpeed.Name = "numFilterSpeed";
            this.numFilterSpeed.Size = new System.Drawing.Size(65, 21);
            this.numFilterSpeed.TabIndex = 69;
            // 
            // lblFilterSpeed
            // 
            this.lblFilterSpeed.AutoSize = true;
            this.lblFilterSpeed.BackColor = System.Drawing.Color.Transparent;
            this.lblFilterSpeed.Location = new System.Drawing.Point(599, 405);
            this.lblFilterSpeed.Name = "lblFilterSpeed";
            this.lblFilterSpeed.Size = new System.Drawing.Size(29, 12);
            this.lblFilterSpeed.TabIndex = 68;
            this.lblFilterSpeed.Text = "速度";
            // 
            // numCollimatorSpeed
            // 
            this.numCollimatorSpeed.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numCollimatorSpeed.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numCollimatorSpeed.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numCollimatorSpeed.Location = new System.Drawing.Point(260, 434);
            this.numCollimatorSpeed.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numCollimatorSpeed.Name = "numCollimatorSpeed";
            this.numCollimatorSpeed.Size = new System.Drawing.Size(65, 21);
            this.numCollimatorSpeed.TabIndex = 67;
            // 
            // lblCollimatorSpeed
            // 
            this.lblCollimatorSpeed.AutoSize = true;
            this.lblCollimatorSpeed.BackColor = System.Drawing.Color.Transparent;
            this.lblCollimatorSpeed.Location = new System.Drawing.Point(258, 405);
            this.lblCollimatorSpeed.Name = "lblCollimatorSpeed";
            this.lblCollimatorSpeed.Size = new System.Drawing.Size(29, 12);
            this.lblCollimatorSpeed.TabIndex = 66;
            this.lblCollimatorSpeed.Text = "速度";
            // 
            // numFilterMaxNum
            // 
            this.numFilterMaxNum.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numFilterMaxNum.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numFilterMaxNum.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numFilterMaxNum.Location = new System.Drawing.Point(624, 41);
            this.numFilterMaxNum.Name = "numFilterMaxNum";
            this.numFilterMaxNum.ReadOnly = true;
            this.numFilterMaxNum.Size = new System.Drawing.Size(42, 21);
            this.numFilterMaxNum.TabIndex = 63;
            this.numFilterMaxNum.ValueChanged += new System.EventHandler(this.numFilterMaxNum_ValueChanged);
            // 
            // lblFilterMaxNum
            // 
            this.lblFilterMaxNum.AutoSize = true;
            this.lblFilterMaxNum.BackColor = System.Drawing.Color.Transparent;
            this.lblFilterMaxNum.Location = new System.Drawing.Point(529, 45);
            this.lblFilterMaxNum.Name = "lblFilterMaxNum";
            this.lblFilterMaxNum.Size = new System.Drawing.Size(53, 12);
            this.lblFilterMaxNum.TabIndex = 62;
            this.lblFilterMaxNum.Text = "最大编号";
            // 
            // numCollimatorMaxNum
            // 
            this.numCollimatorMaxNum.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numCollimatorMaxNum.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numCollimatorMaxNum.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numCollimatorMaxNum.Location = new System.Drawing.Point(283, 45);
            this.numCollimatorMaxNum.Name = "numCollimatorMaxNum";
            this.numCollimatorMaxNum.ReadOnly = true;
            this.numCollimatorMaxNum.Size = new System.Drawing.Size(42, 21);
            this.numCollimatorMaxNum.TabIndex = 61;
            this.numCollimatorMaxNum.ValueChanged += new System.EventHandler(this.numCollimatorMaxNum_ValueChanged);
            // 
            // lblCollimatorMaxNum
            // 
            this.lblCollimatorMaxNum.AutoSize = true;
            this.lblCollimatorMaxNum.BackColor = System.Drawing.Color.Transparent;
            this.lblCollimatorMaxNum.Location = new System.Drawing.Point(188, 45);
            this.lblCollimatorMaxNum.Name = "lblCollimatorMaxNum";
            this.lblCollimatorMaxNum.Size = new System.Drawing.Size(53, 12);
            this.lblCollimatorMaxNum.TabIndex = 60;
            this.lblCollimatorMaxNum.Text = "最大编号";
            // 
            // numFilterDirect
            // 
            this.numFilterDirect.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numFilterDirect.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numFilterDirect.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numFilterDirect.Location = new System.Drawing.Point(493, 434);
            this.numFilterDirect.Name = "numFilterDirect";
            this.numFilterDirect.Size = new System.Drawing.Size(48, 21);
            this.numFilterDirect.TabIndex = 55;
            // 
            // lblFilterDirect
            // 
            this.lblFilterDirect.AutoSize = true;
            this.lblFilterDirect.BackColor = System.Drawing.Color.Transparent;
            this.lblFilterDirect.Location = new System.Drawing.Point(491, 408);
            this.lblFilterDirect.Name = "lblFilterDirect";
            this.lblFilterDirect.Size = new System.Drawing.Size(29, 12);
            this.lblFilterDirect.TabIndex = 54;
            this.lblFilterDirect.Text = "方向";
            // 
            // numFilterCode
            // 
            this.numFilterCode.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numFilterCode.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numFilterCode.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numFilterCode.Location = new System.Drawing.Point(377, 434);
            this.numFilterCode.Name = "numFilterCode";
            this.numFilterCode.Size = new System.Drawing.Size(51, 21);
            this.numFilterCode.TabIndex = 53;
            // 
            // lblFilterCode
            // 
            this.lblFilterCode.AutoSize = true;
            this.lblFilterCode.BackColor = System.Drawing.Color.Transparent;
            this.lblFilterCode.Location = new System.Drawing.Point(375, 405);
            this.lblFilterCode.Name = "lblFilterCode";
            this.lblFilterCode.Size = new System.Drawing.Size(53, 12);
            this.lblFilterCode.TabIndex = 52;
            this.lblFilterCode.Text = "电机编号";
            // 
            // numCollimatorDirect
            // 
            this.numCollimatorDirect.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numCollimatorDirect.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numCollimatorDirect.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numCollimatorDirect.Location = new System.Drawing.Point(152, 434);
            this.numCollimatorDirect.Name = "numCollimatorDirect";
            this.numCollimatorDirect.Size = new System.Drawing.Size(48, 21);
            this.numCollimatorDirect.TabIndex = 51;
            // 
            // lblCollimatorDirect
            // 
            this.lblCollimatorDirect.AutoSize = true;
            this.lblCollimatorDirect.BackColor = System.Drawing.Color.Transparent;
            this.lblCollimatorDirect.Location = new System.Drawing.Point(150, 408);
            this.lblCollimatorDirect.Name = "lblCollimatorDirect";
            this.lblCollimatorDirect.Size = new System.Drawing.Size(29, 12);
            this.lblCollimatorDirect.TabIndex = 50;
            this.lblCollimatorDirect.Text = "方向";
            // 
            // numCollimatorCode
            // 
            this.numCollimatorCode.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numCollimatorCode.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numCollimatorCode.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numCollimatorCode.Location = new System.Drawing.Point(36, 434);
            this.numCollimatorCode.Name = "numCollimatorCode";
            this.numCollimatorCode.Size = new System.Drawing.Size(51, 21);
            this.numCollimatorCode.TabIndex = 49;
            // 
            // lblCollimatorCode
            // 
            this.lblCollimatorCode.AutoSize = true;
            this.lblCollimatorCode.BackColor = System.Drawing.Color.Transparent;
            this.lblCollimatorCode.Location = new System.Drawing.Point(34, 408);
            this.lblCollimatorCode.Name = "lblCollimatorCode";
            this.lblCollimatorCode.Size = new System.Drawing.Size(53, 12);
            this.lblCollimatorCode.TabIndex = 48;
            this.lblCollimatorCode.Text = "电机编号";
            // 
            // chkFilter
            // 
            this.chkFilter.AutoSize = true;
            this.chkFilter.BackColor = System.Drawing.Color.Transparent;
            this.chkFilter.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkFilter.Location = new System.Drawing.Point(377, 41);
            this.chkFilter.Name = "chkFilter";
            this.chkFilter.Size = new System.Drawing.Size(60, 16);
            this.chkFilter.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkFilter.TabIndex = 46;
            this.chkFilter.Text = "滤光片";
            this.chkFilter.UseVisualStyleBackColor = false;
            // 
            // chkCollimator
            // 
            this.chkCollimator.AutoSize = true;
            this.chkCollimator.BackColor = System.Drawing.Color.Transparent;
            this.chkCollimator.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkCollimator.Location = new System.Drawing.Point(36, 45);
            this.chkCollimator.Name = "chkCollimator";
            this.chkCollimator.Size = new System.Drawing.Size(60, 16);
            this.chkCollimator.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkCollimator.TabIndex = 45;
            this.chkCollimator.Text = "准直器";
            this.chkCollimator.UseVisualStyleBackColor = false;
            // 
            // dgvwFilter
            // 
            this.dgvwFilter.AllowUserToAddRows = false;
            this.dgvwFilter.AllowUserToDeleteRows = false;
            this.dgvwFilter.AllowUserToResizeColumns = false;
            this.dgvwFilter.AllowUserToResizeRows = false;
            this.dgvwFilter.BackgroundColor = System.Drawing.Color.White;
            this.dgvwFilter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvwFilter.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvwFilter.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvwFilter.ColumnHeadersHeight = 24;
            this.dgvwFilter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvwFilter.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColFilterNum,
            this.ColFilterStep,
            this.ColCaption,
            this.ColFilterThickness});
            this.dgvwFilter.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvwFilter.Location = new System.Drawing.Point(377, 78);
            this.dgvwFilter.Name = "dgvwFilter";
            this.dgvwFilter.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvwFilter.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvwFilter.RowHeadersVisible = false;
            this.dgvwFilter.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvwFilter.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvwFilter.RowTemplate.Height = 23;
            this.dgvwFilter.SecondaryLength = 1;
            this.dgvwFilter.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvwFilter.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvwFilter.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvwFilter.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvwFilter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvwFilter.ShowEportContextMenu = true;
            this.dgvwFilter.Size = new System.Drawing.Size(289, 310);
            this.dgvwFilter.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvwFilter.TabIndex = 36;
            this.dgvwFilter.ToPrintCols = null;
            this.dgvwFilter.ToPrintRows = null;
            this.dgvwFilter.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvw_CellValidating);
            this.dgvwFilter.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvw_DataError);
            // 
            // ColFilterNum
            // 
            this.ColFilterNum.DataPropertyName = "Num";
            this.ColFilterNum.HeaderText = "编号";
            this.ColFilterNum.Name = "ColFilterNum";
            this.ColFilterNum.ReadOnly = true;
            this.ColFilterNum.Width = 58;
            // 
            // ColFilterStep
            // 
            this.ColFilterStep.DataPropertyName = "Step";
            this.ColFilterStep.HeaderText = "步数";
            this.ColFilterStep.Name = "ColFilterStep";
            this.ColFilterStep.Width = 72;
            // 
            // ColCaption
            // 
            this.ColCaption.DataPropertyName = "Caption";
            this.ColCaption.HeaderText = "成分";
            this.ColCaption.Name = "ColCaption";
            this.ColCaption.Width = 72;
            // 
            // ColFilterThickness
            // 
            this.ColFilterThickness.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColFilterThickness.DataPropertyName = "FilterThickness";
            this.ColFilterThickness.HeaderText = "厚度";
            this.ColFilterThickness.Name = "ColFilterThickness";
            // 
            // dgvwCollimator
            // 
            this.dgvwCollimator.AllowUserToAddRows = false;
            this.dgvwCollimator.AllowUserToDeleteRows = false;
            this.dgvwCollimator.AllowUserToResizeColumns = false;
            this.dgvwCollimator.AllowUserToResizeRows = false;
            this.dgvwCollimator.BackgroundColor = System.Drawing.Color.White;
            this.dgvwCollimator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvwCollimator.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvwCollimator.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvwCollimator.ColumnHeadersHeight = 24;
            this.dgvwCollimator.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvwCollimator.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColCollimatorNum,
            this.ColDiameter,
            this.ColCollimatorStep});
            this.dgvwCollimator.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvwCollimator.Location = new System.Drawing.Point(36, 78);
            this.dgvwCollimator.Name = "dgvwCollimator";
            this.dgvwCollimator.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvwCollimator.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvwCollimator.RowHeadersVisible = false;
            this.dgvwCollimator.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvwCollimator.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvwCollimator.RowTemplate.Height = 23;
            this.dgvwCollimator.SecondaryLength = 1;
            this.dgvwCollimator.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvwCollimator.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvwCollimator.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvwCollimator.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvwCollimator.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvwCollimator.ShowEportContextMenu = true;
            this.dgvwCollimator.Size = new System.Drawing.Size(289, 310);
            this.dgvwCollimator.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvwCollimator.TabIndex = 33;
            this.dgvwCollimator.ToPrintCols = null;
            this.dgvwCollimator.ToPrintRows = null;
            this.dgvwCollimator.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvw_CellValidating);
            this.dgvwCollimator.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvw_DataError);
            // 
            // ColCollimatorNum
            // 
            this.ColCollimatorNum.DataPropertyName = "Num";
            this.ColCollimatorNum.HeaderText = "编号";
            this.ColCollimatorNum.Name = "ColCollimatorNum";
            this.ColCollimatorNum.ReadOnly = true;
            this.ColCollimatorNum.Width = 86;
            // 
            // ColDiameter
            // 
            this.ColDiameter.DataPropertyName = "Diameter";
            this.ColDiameter.HeaderText = "直径(mm)";
            this.ColDiameter.Name = "ColDiameter";
            this.ColDiameter.Width = 90;
            // 
            // ColCollimatorStep
            // 
            this.ColCollimatorStep.DataPropertyName = "Step";
            this.ColCollimatorStep.HeaderText = "步数";
            this.ColCollimatorStep.Name = "ColCollimatorStep";
            this.ColCollimatorStep.Width = 110;
            // 
            // tpTarget
            // 
            this.tpTarget.Controls.Add(this.numTargetSpeed);
            this.tpTarget.Controls.Add(this.lblTargetSpeed);
            this.tpTarget.Controls.Add(this.numTargetMaxNum);
            this.tpTarget.Controls.Add(this.lblTargetMaxNum);
            this.tpTarget.Controls.Add(this.numTargetDirect);
            this.tpTarget.Controls.Add(this.lblTargetDirect);
            this.tpTarget.Controls.Add(this.numTargetCode);
            this.tpTarget.Controls.Add(this.lblTargetCode);
            this.tpTarget.Controls.Add(this.chkTarget);
            this.tpTarget.Controls.Add(this.dgvwTarget);
            this.tpTarget.Controls.Add(this.numChamberSpeed);
            this.tpTarget.Controls.Add(this.lblChamberSpeed);
            this.tpTarget.Controls.Add(this.numChamberMaxNum);
            this.tpTarget.Controls.Add(this.lblChamberMaxNum);
            this.tpTarget.Controls.Add(this.numChamberDirect);
            this.tpTarget.Controls.Add(this.lblChamberDirect);
            this.tpTarget.Controls.Add(this.numChamberCode);
            this.tpTarget.Controls.Add(this.lblChamberCode);
            this.tpTarget.Controls.Add(this.chkChamber);
            this.tpTarget.Controls.Add(this.dgvwChamber);
            this.tpTarget.Location = new System.Drawing.Point(4, 26);
            this.tpTarget.Name = "tpTarget";
            this.tpTarget.Padding = new System.Windows.Forms.Padding(3);
            this.tpTarget.Size = new System.Drawing.Size(192, 70);
            this.tpTarget.TabIndex = 3;
            this.tpTarget.Text = "样品转盘/靶材";
            this.tpTarget.UseVisualStyleBackColor = true;
            // 
            // numTargetSpeed
            // 
            this.numTargetSpeed.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numTargetSpeed.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numTargetSpeed.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numTargetSpeed.Location = new System.Drawing.Point(601, 412);
            this.numTargetSpeed.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numTargetSpeed.Name = "numTargetSpeed";
            this.numTargetSpeed.Size = new System.Drawing.Size(65, 21);
            this.numTargetSpeed.TabIndex = 91;
            // 
            // lblTargetSpeed
            // 
            this.lblTargetSpeed.AutoSize = true;
            this.lblTargetSpeed.BackColor = System.Drawing.Color.Transparent;
            this.lblTargetSpeed.Location = new System.Drawing.Point(599, 391);
            this.lblTargetSpeed.Name = "lblTargetSpeed";
            this.lblTargetSpeed.Size = new System.Drawing.Size(29, 12);
            this.lblTargetSpeed.TabIndex = 90;
            this.lblTargetSpeed.Text = "速度";
            // 
            // numTargetMaxNum
            // 
            this.numTargetMaxNum.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numTargetMaxNum.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numTargetMaxNum.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numTargetMaxNum.Location = new System.Drawing.Point(624, 29);
            this.numTargetMaxNum.Name = "numTargetMaxNum";
            this.numTargetMaxNum.ReadOnly = true;
            this.numTargetMaxNum.Size = new System.Drawing.Size(42, 21);
            this.numTargetMaxNum.TabIndex = 89;
            this.numTargetMaxNum.ValueChanged += new System.EventHandler(this.numTargetMaxNum_ValueChanged);
            // 
            // lblTargetMaxNum
            // 
            this.lblTargetMaxNum.AutoSize = true;
            this.lblTargetMaxNum.BackColor = System.Drawing.Color.Transparent;
            this.lblTargetMaxNum.Location = new System.Drawing.Point(529, 33);
            this.lblTargetMaxNum.Name = "lblTargetMaxNum";
            this.lblTargetMaxNum.Size = new System.Drawing.Size(53, 12);
            this.lblTargetMaxNum.TabIndex = 88;
            this.lblTargetMaxNum.Text = "最大编号";
            // 
            // numTargetDirect
            // 
            this.numTargetDirect.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numTargetDirect.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numTargetDirect.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numTargetDirect.Location = new System.Drawing.Point(492, 412);
            this.numTargetDirect.Name = "numTargetDirect";
            this.numTargetDirect.Size = new System.Drawing.Size(48, 21);
            this.numTargetDirect.TabIndex = 87;
            // 
            // lblTargetDirect
            // 
            this.lblTargetDirect.AutoSize = true;
            this.lblTargetDirect.BackColor = System.Drawing.Color.Transparent;
            this.lblTargetDirect.Location = new System.Drawing.Point(491, 391);
            this.lblTargetDirect.Name = "lblTargetDirect";
            this.lblTargetDirect.Size = new System.Drawing.Size(29, 12);
            this.lblTargetDirect.TabIndex = 86;
            this.lblTargetDirect.Text = "方向";
            // 
            // numTargetCode
            // 
            this.numTargetCode.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numTargetCode.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numTargetCode.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numTargetCode.Location = new System.Drawing.Point(377, 412);
            this.numTargetCode.Name = "numTargetCode";
            this.numTargetCode.Size = new System.Drawing.Size(51, 21);
            this.numTargetCode.TabIndex = 85;
            // 
            // lblTargetCode
            // 
            this.lblTargetCode.AutoSize = true;
            this.lblTargetCode.BackColor = System.Drawing.Color.Transparent;
            this.lblTargetCode.Location = new System.Drawing.Point(375, 391);
            this.lblTargetCode.Name = "lblTargetCode";
            this.lblTargetCode.Size = new System.Drawing.Size(53, 12);
            this.lblTargetCode.TabIndex = 84;
            this.lblTargetCode.Text = "电机编号";
            // 
            // chkTarget
            // 
            this.chkTarget.AutoSize = true;
            this.chkTarget.BackColor = System.Drawing.Color.Transparent;
            this.chkTarget.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkTarget.Location = new System.Drawing.Point(377, 30);
            this.chkTarget.Name = "chkTarget";
            this.chkTarget.Size = new System.Drawing.Size(48, 16);
            this.chkTarget.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkTarget.TabIndex = 83;
            this.chkTarget.Text = "靶材";
            this.chkTarget.UseVisualStyleBackColor = false;
            // 
            // dgvwTarget
            // 
            this.dgvwTarget.AllowUserToAddRows = false;
            this.dgvwTarget.AllowUserToDeleteRows = false;
            this.dgvwTarget.AllowUserToResizeColumns = false;
            this.dgvwTarget.AllowUserToResizeRows = false;
            this.dgvwTarget.BackgroundColor = System.Drawing.Color.White;
            this.dgvwTarget.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvwTarget.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvwTarget.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvwTarget.ColumnHeadersHeight = 24;
            this.dgvwTarget.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvwTarget.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColTargetNum,
            this.ColTargetStep,
            this.ColTargetCaption,
            this.ColTargetThickness});
            this.dgvwTarget.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvwTarget.Location = new System.Drawing.Point(377, 56);
            this.dgvwTarget.Name = "dgvwTarget";
            this.dgvwTarget.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvwTarget.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvwTarget.RowHeadersVisible = false;
            this.dgvwTarget.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvwTarget.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvwTarget.RowTemplate.Height = 23;
            this.dgvwTarget.SecondaryLength = 1;
            this.dgvwTarget.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvwTarget.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvwTarget.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvwTarget.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvwTarget.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvwTarget.ShowEportContextMenu = true;
            this.dgvwTarget.Size = new System.Drawing.Size(289, 310);
            this.dgvwTarget.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvwTarget.TabIndex = 82;
            this.dgvwTarget.ToPrintCols = null;
            this.dgvwTarget.ToPrintRows = null;
            this.dgvwTarget.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvw_CellValidating);
            this.dgvwTarget.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvw_DataError);
            // 
            // ColTargetNum
            // 
            this.ColTargetNum.DataPropertyName = "Num";
            this.ColTargetNum.HeaderText = "编号";
            this.ColTargetNum.Name = "ColTargetNum";
            this.ColTargetNum.ReadOnly = true;
            this.ColTargetNum.Width = 70;
            // 
            // ColTargetStep
            // 
            this.ColTargetStep.DataPropertyName = "Step";
            this.ColTargetStep.HeaderText = "步长";
            this.ColTargetStep.Name = "ColTargetStep";
            this.ColTargetStep.Width = 72;
            // 
            // ColTargetCaption
            // 
            this.ColTargetCaption.DataPropertyName = "Caption";
            this.ColTargetCaption.HeaderText = "成分";
            this.ColTargetCaption.Name = "ColTargetCaption";
            this.ColTargetCaption.Width = 72;
            // 
            // ColTargetThickness
            // 
            this.ColTargetThickness.DataPropertyName = "TargetThickness";
            this.ColTargetThickness.HeaderText = "厚度";
            this.ColTargetThickness.Name = "ColTargetThickness";
            this.ColTargetThickness.Width = 72;
            // 
            // numChamberSpeed
            // 
            this.numChamberSpeed.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numChamberSpeed.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numChamberSpeed.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numChamberSpeed.Location = new System.Drawing.Point(260, 412);
            this.numChamberSpeed.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numChamberSpeed.Name = "numChamberSpeed";
            this.numChamberSpeed.Size = new System.Drawing.Size(65, 21);
            this.numChamberSpeed.TabIndex = 81;
            // 
            // lblChamberSpeed
            // 
            this.lblChamberSpeed.AutoSize = true;
            this.lblChamberSpeed.BackColor = System.Drawing.Color.Transparent;
            this.lblChamberSpeed.Location = new System.Drawing.Point(258, 391);
            this.lblChamberSpeed.Name = "lblChamberSpeed";
            this.lblChamberSpeed.Size = new System.Drawing.Size(29, 12);
            this.lblChamberSpeed.TabIndex = 80;
            this.lblChamberSpeed.Text = "速度";
            // 
            // numChamberMaxNum
            // 
            this.numChamberMaxNum.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numChamberMaxNum.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numChamberMaxNum.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numChamberMaxNum.Location = new System.Drawing.Point(283, 29);
            this.numChamberMaxNum.Name = "numChamberMaxNum";
            this.numChamberMaxNum.ReadOnly = true;
            this.numChamberMaxNum.Size = new System.Drawing.Size(42, 21);
            this.numChamberMaxNum.TabIndex = 79;
            this.numChamberMaxNum.ValueChanged += new System.EventHandler(this.numChamberMaxNum_ValueChanged);
            // 
            // lblChamberMaxNum
            // 
            this.lblChamberMaxNum.AutoSize = true;
            this.lblChamberMaxNum.BackColor = System.Drawing.Color.Transparent;
            this.lblChamberMaxNum.Location = new System.Drawing.Point(188, 33);
            this.lblChamberMaxNum.Name = "lblChamberMaxNum";
            this.lblChamberMaxNum.Size = new System.Drawing.Size(53, 12);
            this.lblChamberMaxNum.TabIndex = 78;
            this.lblChamberMaxNum.Text = "最大编号";
            // 
            // numChamberDirect
            // 
            this.numChamberDirect.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numChamberDirect.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numChamberDirect.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numChamberDirect.Location = new System.Drawing.Point(151, 412);
            this.numChamberDirect.Name = "numChamberDirect";
            this.numChamberDirect.Size = new System.Drawing.Size(48, 21);
            this.numChamberDirect.TabIndex = 77;
            // 
            // lblChamberDirect
            // 
            this.lblChamberDirect.AutoSize = true;
            this.lblChamberDirect.BackColor = System.Drawing.Color.Transparent;
            this.lblChamberDirect.Location = new System.Drawing.Point(150, 391);
            this.lblChamberDirect.Name = "lblChamberDirect";
            this.lblChamberDirect.Size = new System.Drawing.Size(29, 12);
            this.lblChamberDirect.TabIndex = 76;
            this.lblChamberDirect.Text = "方向";
            // 
            // numChamberCode
            // 
            this.numChamberCode.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numChamberCode.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numChamberCode.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numChamberCode.Location = new System.Drawing.Point(36, 412);
            this.numChamberCode.Name = "numChamberCode";
            this.numChamberCode.Size = new System.Drawing.Size(51, 21);
            this.numChamberCode.TabIndex = 75;
            // 
            // lblChamberCode
            // 
            this.lblChamberCode.AutoSize = true;
            this.lblChamberCode.BackColor = System.Drawing.Color.Transparent;
            this.lblChamberCode.Location = new System.Drawing.Point(34, 391);
            this.lblChamberCode.Name = "lblChamberCode";
            this.lblChamberCode.Size = new System.Drawing.Size(53, 12);
            this.lblChamberCode.TabIndex = 74;
            this.lblChamberCode.Text = "电机编号";
            // 
            // chkChamber
            // 
            this.chkChamber.AutoSize = true;
            this.chkChamber.BackColor = System.Drawing.Color.Transparent;
            this.chkChamber.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkChamber.Location = new System.Drawing.Point(36, 30);
            this.chkChamber.Name = "chkChamber";
            this.chkChamber.Size = new System.Drawing.Size(96, 16);
            this.chkChamber.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkChamber.TabIndex = 73;
            this.chkChamber.Text = "自动样品转盘";
            this.chkChamber.UseVisualStyleBackColor = false;
            // 
            // dgvwChamber
            // 
            this.dgvwChamber.AllowUserToAddRows = false;
            this.dgvwChamber.AllowUserToDeleteRows = false;
            this.dgvwChamber.AllowUserToResizeColumns = false;
            this.dgvwChamber.AllowUserToResizeRows = false;
            this.dgvwChamber.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvwChamber.BackgroundColor = System.Drawing.Color.White;
            this.dgvwChamber.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvwChamber.ColumnHeaderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dgvwChamber.ColumnHeaderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.dgvwChamber.ColumnHeadersHeight = 24;
            this.dgvwChamber.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvwChamber.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColChamberNum,
            this.ColChamberStep,
            this.ColChamberStepCoef1,
            this.ColChamberStepCoef2});
            this.dgvwChamber.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvwChamber.Location = new System.Drawing.Point(36, 56);
            this.dgvwChamber.Name = "dgvwChamber";
            this.dgvwChamber.PrimaryRowcolor1 = System.Drawing.Color.White;
            this.dgvwChamber.PrimaryRowcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(249)))), ((int)(((byte)(232)))));
            this.dgvwChamber.RowHeadersVisible = false;
            this.dgvwChamber.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvwChamber.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvwChamber.RowTemplate.Height = 23;
            this.dgvwChamber.SecondaryLength = 1;
            this.dgvwChamber.SecondaryRowColor1 = System.Drawing.Color.White;
            this.dgvwChamber.SecondaryRowColor2 = System.Drawing.Color.Black;
            this.dgvwChamber.SelectedRowColor1 = System.Drawing.Color.White;
            this.dgvwChamber.SelectedRowColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(237)))), ((int)(((byte)(206)))));
            this.dgvwChamber.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvwChamber.ShowEportContextMenu = true;
            this.dgvwChamber.Size = new System.Drawing.Size(289, 310);
            this.dgvwChamber.Style = Skyray.Controls.Style.Office2007Blue;
            this.dgvwChamber.TabIndex = 72;
            this.dgvwChamber.ToPrintCols = null;
            this.dgvwChamber.ToPrintRows = null;
            this.dgvwChamber.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvw_CellValidating);
            this.dgvwChamber.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvw_DataError);
            // 
            // ColChamberNum
            // 
            this.ColChamberNum.DataPropertyName = "Num";
            this.ColChamberNum.HeaderText = "编号";
            this.ColChamberNum.Name = "ColChamberNum";
            this.ColChamberNum.ReadOnly = true;
            // 
            // ColChamberStep
            // 
            this.ColChamberStep.DataPropertyName = "Step";
            this.ColChamberStep.HeaderText = "步数";
            this.ColChamberStep.Name = "ColChamberStep";
            // 
            // ColChamberStepCoef1
            // 
            this.ColChamberStepCoef1.DataPropertyName = "StepCoef1";
            this.ColChamberStepCoef1.HeaderText = "偏移+";
            this.ColChamberStepCoef1.Name = "ColChamberStepCoef1";
            // 
            // ColChamberStepCoef2
            // 
            this.ColChamberStepCoef2.DataPropertyName = "StepCoef2";
            this.ColChamberStepCoef2.HeaderText = "偏移-";
            this.ColChamberStepCoef2.Name = "ColChamberStepCoef2";
            // 
            // tpother
            // 
            this.tpother.BackColor = System.Drawing.Color.GhostWhite;
            this.tpother.Controls.Add(this.gpXYZ);
            this.tpother.Controls.Add(this.meTubes);
            this.tpother.Controls.Add(this.meDetector);
            this.tpother.Location = new System.Drawing.Point(4, 26);
            this.tpother.Name = "tpother";
            this.tpother.Padding = new System.Windows.Forms.Padding(3);
            this.tpother.Size = new System.Drawing.Size(708, 524);
            this.tpother.TabIndex = 2;
            this.tpother.Text = "其它设置";
            this.tpother.UseVisualStyleBackColor = true;
            // 
            // gpXYZ
            // 
            this.gpXYZ.BackgroundColor = System.Drawing.Color.Transparent;
            this.gpXYZ.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.gpXYZ.BackgroundGradientMode = Skyray.Controls.Grouper.GroupBoxGradientMode.None;
            this.gpXYZ.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.gpXYZ.BorderThickness = 1F;
            this.gpXYZ.BorderTopOnly = false;
            this.gpXYZ.Controls.Add(this.pnlEncoderMotor);
            this.gpXYZ.Controls.Add(this.chkHasMotorEncoder);
            this.gpXYZ.Controls.Add(this.pnlLightMotor);
            this.gpXYZ.Controls.Add(this.chkHasMotorLight);
            this.gpXYZ.Controls.Add(this.pnlY1Motor);
            this.gpXYZ.Controls.Add(this.chkHasMotorY1);
            this.gpXYZ.Controls.Add(this.pnlXMotor);
            this.gpXYZ.Controls.Add(this.chkHasMotorX);
            this.gpXYZ.Controls.Add(this.pnlZMotor);
            this.gpXYZ.Controls.Add(this.chkHasMotorY);
            this.gpXYZ.Controls.Add(this.pnlYMotor);
            this.gpXYZ.Controls.Add(this.chkHasMotorZ);
            this.gpXYZ.CustomGroupBoxColor = System.Drawing.Color.Transparent;
            this.gpXYZ.GroupBoxAlign = Skyray.Controls.Grouper.GroupBoxAlignMode.Center;
            this.gpXYZ.GroupImage = null;
            this.gpXYZ.GroupTitle = "";
            this.gpXYZ.HeaderRoundCorners = 4;
            this.gpXYZ.Location = new System.Drawing.Point(6, 264);
            this.gpXYZ.Name = "gpXYZ";
            this.gpXYZ.PaintGroupBox = false;
            this.gpXYZ.RoundCorners = 4;
            this.gpXYZ.ShadowColor = System.Drawing.Color.DarkGray;
            this.gpXYZ.ShadowControl = false;
            this.gpXYZ.ShadowThickness = 3;
            this.gpXYZ.Size = new System.Drawing.Size(686, 257);
            this.gpXYZ.TabIndex = 0;
            this.gpXYZ.TextLineSpace = 2;
            this.gpXYZ.TitleLeftSpace = 18;
            // 
            // pnlEncoderMotor
            // 
            this.pnlEncoderMotor.Controls.Add(this.numMotorEncoderSpeed);
            this.pnlEncoderMotor.Controls.Add(this.lblMotorEncoderSpeed);
            this.pnlEncoderMotor.Controls.Add(this.lblMotorEncoderCode);
            this.pnlEncoderMotor.Controls.Add(this.numMotorEncoderCode);
            this.pnlEncoderMotor.Controls.Add(this.lblMotorEncoderDirect);
            this.pnlEncoderMotor.Controls.Add(this.numMotorEncoderDirect);
            this.pnlEncoderMotor.Controls.Add(this.lblMotorEncoderMaxStep);
            this.pnlEncoderMotor.Controls.Add(this.numMotorEncoderMaxStep);
            this.pnlEncoderMotor.Location = new System.Drawing.Point(113, 187);
            this.pnlEncoderMotor.Name = "pnlEncoderMotor";
            this.pnlEncoderMotor.Size = new System.Drawing.Size(549, 32);
            this.pnlEncoderMotor.TabIndex = 97;
            // 
            // numMotorEncoderSpeed
            // 
            this.numMotorEncoderSpeed.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numMotorEncoderSpeed.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numMotorEncoderSpeed.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numMotorEncoderSpeed.Location = new System.Drawing.Point(301, 5);
            this.numMotorEncoderSpeed.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numMotorEncoderSpeed.Name = "numMotorEncoderSpeed";
            this.numMotorEncoderSpeed.Size = new System.Drawing.Size(51, 21);
            this.numMotorEncoderSpeed.TabIndex = 96;
            this.numMotorEncoderSpeed.Visible = false;
            // 
            // lblMotorEncoderSpeed
            // 
            this.lblMotorEncoderSpeed.AutoSize = true;
            this.lblMotorEncoderSpeed.BackColor = System.Drawing.Color.Transparent;
            this.lblMotorEncoderSpeed.Location = new System.Drawing.Point(243, 11);
            this.lblMotorEncoderSpeed.Name = "lblMotorEncoderSpeed";
            this.lblMotorEncoderSpeed.Size = new System.Drawing.Size(29, 12);
            this.lblMotorEncoderSpeed.TabIndex = 95;
            this.lblMotorEncoderSpeed.Text = "速度";
            this.lblMotorEncoderSpeed.Visible = false;
            // 
            // lblMotorEncoderCode
            // 
            this.lblMotorEncoderCode.AutoSize = true;
            this.lblMotorEncoderCode.BackColor = System.Drawing.Color.Transparent;
            this.lblMotorEncoderCode.Location = new System.Drawing.Point(18, 10);
            this.lblMotorEncoderCode.Name = "lblMotorEncoderCode";
            this.lblMotorEncoderCode.Size = new System.Drawing.Size(29, 12);
            this.lblMotorEncoderCode.TabIndex = 70;
            this.lblMotorEncoderCode.Text = "编号";
            // 
            // numMotorEncoderCode
            // 
            this.numMotorEncoderCode.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numMotorEncoderCode.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numMotorEncoderCode.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numMotorEncoderCode.Location = new System.Drawing.Point(61, 5);
            this.numMotorEncoderCode.Name = "numMotorEncoderCode";
            this.numMotorEncoderCode.Size = new System.Drawing.Size(44, 21);
            this.numMotorEncoderCode.TabIndex = 71;
            // 
            // lblMotorEncoderDirect
            // 
            this.lblMotorEncoderDirect.AutoSize = true;
            this.lblMotorEncoderDirect.BackColor = System.Drawing.Color.Transparent;
            this.lblMotorEncoderDirect.Location = new System.Drawing.Point(121, 10);
            this.lblMotorEncoderDirect.Name = "lblMotorEncoderDirect";
            this.lblMotorEncoderDirect.Size = new System.Drawing.Size(29, 12);
            this.lblMotorEncoderDirect.TabIndex = 76;
            this.lblMotorEncoderDirect.Text = "方向";
            this.lblMotorEncoderDirect.Visible = false;
            // 
            // numMotorEncoderDirect
            // 
            this.numMotorEncoderDirect.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numMotorEncoderDirect.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numMotorEncoderDirect.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numMotorEncoderDirect.Location = new System.Drawing.Point(180, 5);
            this.numMotorEncoderDirect.Name = "numMotorEncoderDirect";
            this.numMotorEncoderDirect.Size = new System.Drawing.Size(44, 21);
            this.numMotorEncoderDirect.TabIndex = 77;
            this.numMotorEncoderDirect.Visible = false;
            // 
            // lblMotorEncoderMaxStep
            // 
            this.lblMotorEncoderMaxStep.AutoSize = true;
            this.lblMotorEncoderMaxStep.BackColor = System.Drawing.Color.Transparent;
            this.lblMotorEncoderMaxStep.Location = new System.Drawing.Point(376, 10);
            this.lblMotorEncoderMaxStep.Name = "lblMotorEncoderMaxStep";
            this.lblMotorEncoderMaxStep.Size = new System.Drawing.Size(53, 12);
            this.lblMotorEncoderMaxStep.TabIndex = 82;
            this.lblMotorEncoderMaxStep.Text = "最大步数";
            this.lblMotorEncoderMaxStep.Visible = false;
            // 
            // numMotorEncoderMaxStep
            // 
            this.numMotorEncoderMaxStep.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numMotorEncoderMaxStep.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numMotorEncoderMaxStep.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numMotorEncoderMaxStep.Location = new System.Drawing.Point(458, 5);
            this.numMotorEncoderMaxStep.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMotorEncoderMaxStep.Name = "numMotorEncoderMaxStep";
            this.numMotorEncoderMaxStep.Size = new System.Drawing.Size(78, 21);
            this.numMotorEncoderMaxStep.TabIndex = 83;
            this.numMotorEncoderMaxStep.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMotorEncoderMaxStep.Visible = false;
            // 
            // chkHasMotorEncoder
            // 
            this.chkHasMotorEncoder.AutoSize = true;
            this.chkHasMotorEncoder.BackColor = System.Drawing.Color.Transparent;
            this.chkHasMotorEncoder.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkHasMotorEncoder.Location = new System.Drawing.Point(24, 194);
            this.chkHasMotorEncoder.Name = "chkHasMotorEncoder";
            this.chkHasMotorEncoder.Size = new System.Drawing.Size(84, 16);
            this.chkHasMotorEncoder.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkHasMotorEncoder.TabIndex = 93;
            this.chkHasMotorEncoder.Text = "编码器电机";
            this.chkHasMotorEncoder.UseVisualStyleBackColor = false;
            // 
            // pnlLightMotor
            // 
            this.pnlLightMotor.Controls.Add(this.numMotorLightSpeed);
            this.pnlLightMotor.Controls.Add(this.lblMotorLightSpeed);
            this.pnlLightMotor.Controls.Add(this.lblMotorLightCode);
            this.pnlLightMotor.Controls.Add(this.numMotorLightCode);
            this.pnlLightMotor.Controls.Add(this.lblMotorLightDirect);
            this.pnlLightMotor.Controls.Add(this.numMotorLightDirect);
            this.pnlLightMotor.Controls.Add(this.lblMotorLightMaxStep);
            this.pnlLightMotor.Controls.Add(this.numMotorLightMaxStep);
            this.pnlLightMotor.Location = new System.Drawing.Point(113, 220);
            this.pnlLightMotor.Name = "pnlLightMotor";
            this.pnlLightMotor.Size = new System.Drawing.Size(549, 32);
            this.pnlLightMotor.TabIndex = 92;
            this.pnlLightMotor.Visible = false;
            // 
            // numMotorLightSpeed
            // 
            this.numMotorLightSpeed.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numMotorLightSpeed.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numMotorLightSpeed.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numMotorLightSpeed.Location = new System.Drawing.Point(301, 6);
            this.numMotorLightSpeed.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numMotorLightSpeed.Name = "numMotorLightSpeed";
            this.numMotorLightSpeed.Size = new System.Drawing.Size(51, 21);
            this.numMotorLightSpeed.TabIndex = 96;
            // 
            // lblMotorLightSpeed
            // 
            this.lblMotorLightSpeed.AutoSize = true;
            this.lblMotorLightSpeed.BackColor = System.Drawing.Color.Transparent;
            this.lblMotorLightSpeed.Location = new System.Drawing.Point(243, 12);
            this.lblMotorLightSpeed.Name = "lblMotorLightSpeed";
            this.lblMotorLightSpeed.Size = new System.Drawing.Size(29, 12);
            this.lblMotorLightSpeed.TabIndex = 95;
            this.lblMotorLightSpeed.Text = "速度";
            // 
            // lblMotorLightCode
            // 
            this.lblMotorLightCode.AutoSize = true;
            this.lblMotorLightCode.BackColor = System.Drawing.Color.Transparent;
            this.lblMotorLightCode.Location = new System.Drawing.Point(18, 11);
            this.lblMotorLightCode.Name = "lblMotorLightCode";
            this.lblMotorLightCode.Size = new System.Drawing.Size(29, 12);
            this.lblMotorLightCode.TabIndex = 70;
            this.lblMotorLightCode.Text = "编号";
            // 
            // numMotorLightCode
            // 
            this.numMotorLightCode.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numMotorLightCode.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numMotorLightCode.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numMotorLightCode.Location = new System.Drawing.Point(61, 6);
            this.numMotorLightCode.Name = "numMotorLightCode";
            this.numMotorLightCode.Size = new System.Drawing.Size(44, 21);
            this.numMotorLightCode.TabIndex = 71;
            // 
            // lblMotorLightDirect
            // 
            this.lblMotorLightDirect.AutoSize = true;
            this.lblMotorLightDirect.BackColor = System.Drawing.Color.Transparent;
            this.lblMotorLightDirect.Location = new System.Drawing.Point(121, 11);
            this.lblMotorLightDirect.Name = "lblMotorLightDirect";
            this.lblMotorLightDirect.Size = new System.Drawing.Size(29, 12);
            this.lblMotorLightDirect.TabIndex = 76;
            this.lblMotorLightDirect.Text = "方向";
            // 
            // numMotorLightDirect
            // 
            this.numMotorLightDirect.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numMotorLightDirect.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numMotorLightDirect.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numMotorLightDirect.Location = new System.Drawing.Point(180, 6);
            this.numMotorLightDirect.Name = "numMotorLightDirect";
            this.numMotorLightDirect.Size = new System.Drawing.Size(44, 21);
            this.numMotorLightDirect.TabIndex = 77;
            // 
            // lblMotorLightMaxStep
            // 
            this.lblMotorLightMaxStep.AutoSize = true;
            this.lblMotorLightMaxStep.BackColor = System.Drawing.Color.Transparent;
            this.lblMotorLightMaxStep.Location = new System.Drawing.Point(376, 11);
            this.lblMotorLightMaxStep.Name = "lblMotorLightMaxStep";
            this.lblMotorLightMaxStep.Size = new System.Drawing.Size(53, 12);
            this.lblMotorLightMaxStep.TabIndex = 82;
            this.lblMotorLightMaxStep.Text = "最大步数";
            // 
            // numMotorLightMaxStep
            // 
            this.numMotorLightMaxStep.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numMotorLightMaxStep.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numMotorLightMaxStep.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numMotorLightMaxStep.Location = new System.Drawing.Point(458, 6);
            this.numMotorLightMaxStep.Name = "numMotorLightMaxStep";
            this.numMotorLightMaxStep.Size = new System.Drawing.Size(78, 21);
            this.numMotorLightMaxStep.TabIndex = 83;
            // 
            // chkHasMotorLight
            // 
            this.chkHasMotorLight.AutoSize = true;
            this.chkHasMotorLight.BackColor = System.Drawing.Color.Transparent;
            this.chkHasMotorLight.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkHasMotorLight.Location = new System.Drawing.Point(24, 226);
            this.chkHasMotorLight.Name = "chkHasMotorLight";
            this.chkHasMotorLight.Size = new System.Drawing.Size(72, 16);
            this.chkHasMotorLight.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkHasMotorLight.TabIndex = 91;
            this.chkHasMotorLight.Text = "光闸电机";
            this.chkHasMotorLight.UseVisualStyleBackColor = false;
            this.chkHasMotorLight.Visible = false;
            // 
            // pnlY1Motor
            // 
            this.pnlY1Motor.Controls.Add(this.numMotorY1Speed);
            this.pnlY1Motor.Controls.Add(this.lblMotorY1Speed);
            this.pnlY1Motor.Controls.Add(this.lblMotorY1Code);
            this.pnlY1Motor.Controls.Add(this.numMotorY1Code);
            this.pnlY1Motor.Controls.Add(this.lblMotorY1Direct);
            this.pnlY1Motor.Controls.Add(this.numMotorY1Direct);
            this.pnlY1Motor.Controls.Add(this.lblMotorY1MaxDis);
            this.pnlY1Motor.Controls.Add(this.numMotorY1MaxDis);
            this.pnlY1Motor.Location = new System.Drawing.Point(113, 81);
            this.pnlY1Motor.Name = "pnlY1Motor";
            this.pnlY1Motor.Size = new System.Drawing.Size(549, 32);
            this.pnlY1Motor.TabIndex = 90;
            // 
            // numMotorY1Speed
            // 
            this.numMotorY1Speed.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numMotorY1Speed.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numMotorY1Speed.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numMotorY1Speed.Location = new System.Drawing.Point(301, 5);
            this.numMotorY1Speed.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numMotorY1Speed.Name = "numMotorY1Speed";
            this.numMotorY1Speed.Size = new System.Drawing.Size(51, 21);
            this.numMotorY1Speed.TabIndex = 96;
            // 
            // lblMotorY1Speed
            // 
            this.lblMotorY1Speed.AutoSize = true;
            this.lblMotorY1Speed.BackColor = System.Drawing.Color.Transparent;
            this.lblMotorY1Speed.Location = new System.Drawing.Point(243, 11);
            this.lblMotorY1Speed.Name = "lblMotorY1Speed";
            this.lblMotorY1Speed.Size = new System.Drawing.Size(29, 12);
            this.lblMotorY1Speed.TabIndex = 95;
            this.lblMotorY1Speed.Text = "速度";
            // 
            // lblMotorY1Code
            // 
            this.lblMotorY1Code.AutoSize = true;
            this.lblMotorY1Code.BackColor = System.Drawing.Color.Transparent;
            this.lblMotorY1Code.Location = new System.Drawing.Point(18, 10);
            this.lblMotorY1Code.Name = "lblMotorY1Code";
            this.lblMotorY1Code.Size = new System.Drawing.Size(29, 12);
            this.lblMotorY1Code.TabIndex = 70;
            this.lblMotorY1Code.Text = "编号";
            // 
            // numMotorY1Code
            // 
            this.numMotorY1Code.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numMotorY1Code.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numMotorY1Code.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numMotorY1Code.Location = new System.Drawing.Point(61, 5);
            this.numMotorY1Code.Name = "numMotorY1Code";
            this.numMotorY1Code.Size = new System.Drawing.Size(44, 21);
            this.numMotorY1Code.TabIndex = 71;
            // 
            // lblMotorY1Direct
            // 
            this.lblMotorY1Direct.AutoSize = true;
            this.lblMotorY1Direct.BackColor = System.Drawing.Color.Transparent;
            this.lblMotorY1Direct.Location = new System.Drawing.Point(121, 10);
            this.lblMotorY1Direct.Name = "lblMotorY1Direct";
            this.lblMotorY1Direct.Size = new System.Drawing.Size(29, 12);
            this.lblMotorY1Direct.TabIndex = 76;
            this.lblMotorY1Direct.Text = "方向";
            // 
            // numMotorY1Direct
            // 
            this.numMotorY1Direct.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numMotorY1Direct.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numMotorY1Direct.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numMotorY1Direct.Location = new System.Drawing.Point(180, 5);
            this.numMotorY1Direct.Name = "numMotorY1Direct";
            this.numMotorY1Direct.Size = new System.Drawing.Size(44, 21);
            this.numMotorY1Direct.TabIndex = 77;
            // 
            // lblMotorY1MaxDis
            // 
            this.lblMotorY1MaxDis.AutoSize = true;
            this.lblMotorY1MaxDis.BackColor = System.Drawing.Color.Transparent;
            this.lblMotorY1MaxDis.Location = new System.Drawing.Point(376, 10);
            this.lblMotorY1MaxDis.Name = "lblMotorY1MaxDis";
            this.lblMotorY1MaxDis.Size = new System.Drawing.Size(53, 12);
            this.lblMotorY1MaxDis.TabIndex = 82;
            this.lblMotorY1MaxDis.Text = "最大行程";
            // 
            // numMotorY1MaxDis
            // 
            this.numMotorY1MaxDis.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numMotorY1MaxDis.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numMotorY1MaxDis.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numMotorY1MaxDis.Location = new System.Drawing.Point(458, 5);
            this.numMotorY1MaxDis.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMotorY1MaxDis.Name = "numMotorY1MaxDis";
            this.numMotorY1MaxDis.Size = new System.Drawing.Size(78, 21);
            this.numMotorY1MaxDis.TabIndex = 83;
            this.numMotorY1MaxDis.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chkHasMotorY1
            // 
            this.chkHasMotorY1.AutoSize = true;
            this.chkHasMotorY1.BackColor = System.Drawing.Color.Transparent;
            this.chkHasMotorY1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkHasMotorY1.Location = new System.Drawing.Point(24, 90);
            this.chkHasMotorY1.Name = "chkHasMotorY1";
            this.chkHasMotorY1.Size = new System.Drawing.Size(72, 16);
            this.chkHasMotorY1.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkHasMotorY1.TabIndex = 89;
            this.chkHasMotorY1.Text = "Y1轴电机";
            this.chkHasMotorY1.UseVisualStyleBackColor = false;
            // 
            // pnlXMotor
            // 
            this.pnlXMotor.Controls.Add(this.numMotorXSpeed);
            this.pnlXMotor.Controls.Add(this.lblMotorXSpeed);
            this.pnlXMotor.Controls.Add(this.lblMotorXCode);
            this.pnlXMotor.Controls.Add(this.numMotorXCode);
            this.pnlXMotor.Controls.Add(this.lblMotorXDirect);
            this.pnlXMotor.Controls.Add(this.numMotorXDirect);
            this.pnlXMotor.Controls.Add(this.lblMotorXMaxDis);
            this.pnlXMotor.Controls.Add(this.numMotorXMaxDis);
            this.pnlXMotor.Enabled = false;
            this.pnlXMotor.Location = new System.Drawing.Point(113, 15);
            this.pnlXMotor.Name = "pnlXMotor";
            this.pnlXMotor.Size = new System.Drawing.Size(549, 32);
            this.pnlXMotor.TabIndex = 86;
            // 
            // numMotorXSpeed
            // 
            this.numMotorXSpeed.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numMotorXSpeed.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numMotorXSpeed.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numMotorXSpeed.Location = new System.Drawing.Point(301, 6);
            this.numMotorXSpeed.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numMotorXSpeed.Name = "numMotorXSpeed";
            this.numMotorXSpeed.Size = new System.Drawing.Size(51, 21);
            this.numMotorXSpeed.TabIndex = 92;
            // 
            // lblMotorXSpeed
            // 
            this.lblMotorXSpeed.AutoSize = true;
            this.lblMotorXSpeed.BackColor = System.Drawing.Color.Transparent;
            this.lblMotorXSpeed.Location = new System.Drawing.Point(243, 10);
            this.lblMotorXSpeed.Name = "lblMotorXSpeed";
            this.lblMotorXSpeed.Size = new System.Drawing.Size(29, 12);
            this.lblMotorXSpeed.TabIndex = 92;
            this.lblMotorXSpeed.Text = "速度";
            // 
            // lblMotorXCode
            // 
            this.lblMotorXCode.AutoSize = true;
            this.lblMotorXCode.BackColor = System.Drawing.Color.Transparent;
            this.lblMotorXCode.Location = new System.Drawing.Point(18, 10);
            this.lblMotorXCode.Name = "lblMotorXCode";
            this.lblMotorXCode.Size = new System.Drawing.Size(29, 12);
            this.lblMotorXCode.TabIndex = 66;
            this.lblMotorXCode.Text = "编号";
            // 
            // numMotorXCode
            // 
            this.numMotorXCode.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numMotorXCode.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numMotorXCode.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numMotorXCode.Location = new System.Drawing.Point(61, 6);
            this.numMotorXCode.Name = "numMotorXCode";
            this.numMotorXCode.Size = new System.Drawing.Size(44, 21);
            this.numMotorXCode.TabIndex = 67;
            // 
            // lblMotorXDirect
            // 
            this.lblMotorXDirect.AutoSize = true;
            this.lblMotorXDirect.BackColor = System.Drawing.Color.Transparent;
            this.lblMotorXDirect.Location = new System.Drawing.Point(121, 10);
            this.lblMotorXDirect.Name = "lblMotorXDirect";
            this.lblMotorXDirect.Size = new System.Drawing.Size(29, 12);
            this.lblMotorXDirect.TabIndex = 72;
            this.lblMotorXDirect.Text = "方向";
            // 
            // numMotorXDirect
            // 
            this.numMotorXDirect.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numMotorXDirect.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numMotorXDirect.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numMotorXDirect.Location = new System.Drawing.Point(180, 6);
            this.numMotorXDirect.Name = "numMotorXDirect";
            this.numMotorXDirect.Size = new System.Drawing.Size(44, 21);
            this.numMotorXDirect.TabIndex = 73;
            // 
            // lblMotorXMaxDis
            // 
            this.lblMotorXMaxDis.AutoSize = true;
            this.lblMotorXMaxDis.BackColor = System.Drawing.Color.Transparent;
            this.lblMotorXMaxDis.Location = new System.Drawing.Point(376, 10);
            this.lblMotorXMaxDis.Name = "lblMotorXMaxDis";
            this.lblMotorXMaxDis.Size = new System.Drawing.Size(53, 12);
            this.lblMotorXMaxDis.TabIndex = 78;
            this.lblMotorXMaxDis.Text = "最大行程";
            // 
            // numMotorXMaxDis
            // 
            this.numMotorXMaxDis.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numMotorXMaxDis.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numMotorXMaxDis.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numMotorXMaxDis.Location = new System.Drawing.Point(458, 6);
            this.numMotorXMaxDis.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMotorXMaxDis.Name = "numMotorXMaxDis";
            this.numMotorXMaxDis.Size = new System.Drawing.Size(78, 21);
            this.numMotorXMaxDis.TabIndex = 79;
            this.numMotorXMaxDis.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chkHasMotorX
            // 
            this.chkHasMotorX.AutoSize = true;
            this.chkHasMotorX.BackColor = System.Drawing.Color.Transparent;
            this.chkHasMotorX.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkHasMotorX.Location = new System.Drawing.Point(24, 24);
            this.chkHasMotorX.Name = "chkHasMotorX";
            this.chkHasMotorX.Size = new System.Drawing.Size(66, 16);
            this.chkHasMotorX.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkHasMotorX.TabIndex = 62;
            this.chkHasMotorX.Text = "X轴电机";
            this.chkHasMotorX.UseVisualStyleBackColor = false;
            // 
            // pnlZMotor
            // 
            this.pnlZMotor.Controls.Add(this.numDutyDown);
            this.pnlZMotor.Controls.Add(this.lblDutyDown);
            this.pnlZMotor.Controls.Add(this.numDutyUp);
            this.pnlZMotor.Controls.Add(this.lblDutyUp);
            this.pnlZMotor.Controls.Add(this.numMotorZSpeed);
            this.pnlZMotor.Controls.Add(this.lblMotorZSpeed);
            this.pnlZMotor.Controls.Add(this.lblMotorZCode);
            this.pnlZMotor.Controls.Add(this.numMotorZCode);
            this.pnlZMotor.Controls.Add(this.lblMotorZDirect);
            this.pnlZMotor.Controls.Add(this.numMotorZDirect);
            this.pnlZMotor.Controls.Add(this.lblMotorZMaxDis);
            this.pnlZMotor.Controls.Add(this.numMotorZMaxDis);
            this.pnlZMotor.Location = new System.Drawing.Point(113, 114);
            this.pnlZMotor.Name = "pnlZMotor";
            this.pnlZMotor.Size = new System.Drawing.Size(549, 72);
            this.pnlZMotor.TabIndex = 88;
            // 
            // numDutyDown
            // 
            this.numDutyDown.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numDutyDown.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numDutyDown.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numDutyDown.DecimalPlaces = 4;
            this.numDutyDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.numDutyDown.Location = new System.Drawing.Point(313, 38);
            this.numDutyDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDutyDown.Name = "numDutyDown";
            this.numDutyDown.Size = new System.Drawing.Size(63, 21);
            this.numDutyDown.TabIndex = 101;
            // 
            // lblDutyDown
            // 
            this.lblDutyDown.AutoSize = true;
            this.lblDutyDown.BackColor = System.Drawing.Color.Transparent;
            this.lblDutyDown.Location = new System.Drawing.Point(217, 43);
            this.lblDutyDown.Name = "lblDutyDown";
            this.lblDutyDown.Size = new System.Drawing.Size(83, 12);
            this.lblDutyDown.TabIndex = 100;
            this.lblDutyDown.Text = "占空比%(下行)";
            // 
            // numDutyUp
            // 
            this.numDutyUp.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numDutyUp.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numDutyUp.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numDutyUp.DecimalPlaces = 4;
            this.numDutyUp.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.numDutyUp.Location = new System.Drawing.Point(123, 38);
            this.numDutyUp.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDutyUp.Name = "numDutyUp";
            this.numDutyUp.Size = new System.Drawing.Size(63, 21);
            this.numDutyUp.TabIndex = 99;
            // 
            // lblDutyUp
            // 
            this.lblDutyUp.AutoSize = true;
            this.lblDutyUp.BackColor = System.Drawing.Color.Transparent;
            this.lblDutyUp.Location = new System.Drawing.Point(18, 43);
            this.lblDutyUp.Name = "lblDutyUp";
            this.lblDutyUp.Size = new System.Drawing.Size(83, 12);
            this.lblDutyUp.TabIndex = 97;
            this.lblDutyUp.Text = "占空比%(上行)";
            // 
            // numMotorZSpeed
            // 
            this.numMotorZSpeed.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numMotorZSpeed.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numMotorZSpeed.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numMotorZSpeed.Location = new System.Drawing.Point(301, 6);
            this.numMotorZSpeed.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numMotorZSpeed.Name = "numMotorZSpeed";
            this.numMotorZSpeed.Size = new System.Drawing.Size(51, 21);
            this.numMotorZSpeed.TabIndex = 96;
            // 
            // lblMotorZSpeed
            // 
            this.lblMotorZSpeed.AutoSize = true;
            this.lblMotorZSpeed.BackColor = System.Drawing.Color.Transparent;
            this.lblMotorZSpeed.Location = new System.Drawing.Point(243, 12);
            this.lblMotorZSpeed.Name = "lblMotorZSpeed";
            this.lblMotorZSpeed.Size = new System.Drawing.Size(29, 12);
            this.lblMotorZSpeed.TabIndex = 95;
            this.lblMotorZSpeed.Text = "速度";
            // 
            // lblMotorZCode
            // 
            this.lblMotorZCode.AutoSize = true;
            this.lblMotorZCode.BackColor = System.Drawing.Color.Transparent;
            this.lblMotorZCode.Location = new System.Drawing.Point(18, 11);
            this.lblMotorZCode.Name = "lblMotorZCode";
            this.lblMotorZCode.Size = new System.Drawing.Size(29, 12);
            this.lblMotorZCode.TabIndex = 70;
            this.lblMotorZCode.Text = "编号";
            // 
            // numMotorZCode
            // 
            this.numMotorZCode.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numMotorZCode.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numMotorZCode.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numMotorZCode.Location = new System.Drawing.Point(61, 6);
            this.numMotorZCode.Name = "numMotorZCode";
            this.numMotorZCode.Size = new System.Drawing.Size(44, 21);
            this.numMotorZCode.TabIndex = 71;
            // 
            // lblMotorZDirect
            // 
            this.lblMotorZDirect.AutoSize = true;
            this.lblMotorZDirect.BackColor = System.Drawing.Color.Transparent;
            this.lblMotorZDirect.Location = new System.Drawing.Point(121, 11);
            this.lblMotorZDirect.Name = "lblMotorZDirect";
            this.lblMotorZDirect.Size = new System.Drawing.Size(29, 12);
            this.lblMotorZDirect.TabIndex = 76;
            this.lblMotorZDirect.Text = "方向";
            // 
            // numMotorZDirect
            // 
            this.numMotorZDirect.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numMotorZDirect.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numMotorZDirect.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numMotorZDirect.Location = new System.Drawing.Point(180, 6);
            this.numMotorZDirect.Name = "numMotorZDirect";
            this.numMotorZDirect.Size = new System.Drawing.Size(44, 21);
            this.numMotorZDirect.TabIndex = 77;
            // 
            // lblMotorZMaxDis
            // 
            this.lblMotorZMaxDis.AutoSize = true;
            this.lblMotorZMaxDis.BackColor = System.Drawing.Color.Transparent;
            this.lblMotorZMaxDis.Location = new System.Drawing.Point(376, 11);
            this.lblMotorZMaxDis.Name = "lblMotorZMaxDis";
            this.lblMotorZMaxDis.Size = new System.Drawing.Size(53, 12);
            this.lblMotorZMaxDis.TabIndex = 82;
            this.lblMotorZMaxDis.Text = "最大行程";
            // 
            // numMotorZMaxDis
            // 
            this.numMotorZMaxDis.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numMotorZMaxDis.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numMotorZMaxDis.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numMotorZMaxDis.Location = new System.Drawing.Point(458, 6);
            this.numMotorZMaxDis.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMotorZMaxDis.Name = "numMotorZMaxDis";
            this.numMotorZMaxDis.Size = new System.Drawing.Size(78, 21);
            this.numMotorZMaxDis.TabIndex = 83;
            this.numMotorZMaxDis.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chkHasMotorY
            // 
            this.chkHasMotorY.AutoSize = true;
            this.chkHasMotorY.BackColor = System.Drawing.Color.Transparent;
            this.chkHasMotorY.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkHasMotorY.Location = new System.Drawing.Point(24, 57);
            this.chkHasMotorY.Name = "chkHasMotorY";
            this.chkHasMotorY.Size = new System.Drawing.Size(66, 16);
            this.chkHasMotorY.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkHasMotorY.TabIndex = 63;
            this.chkHasMotorY.Text = "Y轴电机";
            this.chkHasMotorY.UseVisualStyleBackColor = false;
            // 
            // pnlYMotor
            // 
            this.pnlYMotor.Controls.Add(this.numMotorYSpeed);
            this.pnlYMotor.Controls.Add(this.lblMotorYSpeed);
            this.pnlYMotor.Controls.Add(this.lblMotorYCode);
            this.pnlYMotor.Controls.Add(this.numMotorYCode);
            this.pnlYMotor.Controls.Add(this.lblMotorYDirect);
            this.pnlYMotor.Controls.Add(this.numMotorYDirect);
            this.pnlYMotor.Controls.Add(this.lblMotorYMaxDis);
            this.pnlYMotor.Controls.Add(this.numMotorYMaxDis);
            this.pnlYMotor.Location = new System.Drawing.Point(113, 48);
            this.pnlYMotor.Name = "pnlYMotor";
            this.pnlYMotor.Size = new System.Drawing.Size(549, 32);
            this.pnlYMotor.TabIndex = 87;
            // 
            // numMotorYSpeed
            // 
            this.numMotorYSpeed.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numMotorYSpeed.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numMotorYSpeed.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numMotorYSpeed.Location = new System.Drawing.Point(301, 6);
            this.numMotorYSpeed.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numMotorYSpeed.Name = "numMotorYSpeed";
            this.numMotorYSpeed.Size = new System.Drawing.Size(51, 21);
            this.numMotorYSpeed.TabIndex = 94;
            // 
            // lblMotorYSpeed
            // 
            this.lblMotorYSpeed.AutoSize = true;
            this.lblMotorYSpeed.BackColor = System.Drawing.Color.Transparent;
            this.lblMotorYSpeed.Location = new System.Drawing.Point(243, 12);
            this.lblMotorYSpeed.Name = "lblMotorYSpeed";
            this.lblMotorYSpeed.Size = new System.Drawing.Size(29, 12);
            this.lblMotorYSpeed.TabIndex = 93;
            this.lblMotorYSpeed.Text = "速度";
            // 
            // lblMotorYCode
            // 
            this.lblMotorYCode.AutoSize = true;
            this.lblMotorYCode.BackColor = System.Drawing.Color.Transparent;
            this.lblMotorYCode.Location = new System.Drawing.Point(18, 12);
            this.lblMotorYCode.Name = "lblMotorYCode";
            this.lblMotorYCode.Size = new System.Drawing.Size(29, 12);
            this.lblMotorYCode.TabIndex = 68;
            this.lblMotorYCode.Text = "编号";
            // 
            // numMotorYCode
            // 
            this.numMotorYCode.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numMotorYCode.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numMotorYCode.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numMotorYCode.Location = new System.Drawing.Point(61, 6);
            this.numMotorYCode.Name = "numMotorYCode";
            this.numMotorYCode.Size = new System.Drawing.Size(44, 21);
            this.numMotorYCode.TabIndex = 69;
            // 
            // lblMotorYDirect
            // 
            this.lblMotorYDirect.AutoSize = true;
            this.lblMotorYDirect.BackColor = System.Drawing.Color.Transparent;
            this.lblMotorYDirect.Location = new System.Drawing.Point(121, 12);
            this.lblMotorYDirect.Name = "lblMotorYDirect";
            this.lblMotorYDirect.Size = new System.Drawing.Size(29, 12);
            this.lblMotorYDirect.TabIndex = 74;
            this.lblMotorYDirect.Text = "方向";
            // 
            // numMotorYDirect
            // 
            this.numMotorYDirect.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numMotorYDirect.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numMotorYDirect.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numMotorYDirect.Location = new System.Drawing.Point(180, 7);
            this.numMotorYDirect.Name = "numMotorYDirect";
            this.numMotorYDirect.Size = new System.Drawing.Size(44, 21);
            this.numMotorYDirect.TabIndex = 75;
            // 
            // lblMotorYMaxDis
            // 
            this.lblMotorYMaxDis.AutoSize = true;
            this.lblMotorYMaxDis.BackColor = System.Drawing.Color.Transparent;
            this.lblMotorYMaxDis.Location = new System.Drawing.Point(376, 12);
            this.lblMotorYMaxDis.Name = "lblMotorYMaxDis";
            this.lblMotorYMaxDis.Size = new System.Drawing.Size(53, 12);
            this.lblMotorYMaxDis.TabIndex = 80;
            this.lblMotorYMaxDis.Text = "最大行程";
            // 
            // numMotorYMaxDis
            // 
            this.numMotorYMaxDis.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(88)))), ((int)(((byte)(128)))));
            this.numMotorYMaxDis.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.numMotorYMaxDis.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.numMotorYMaxDis.Location = new System.Drawing.Point(458, 6);
            this.numMotorYMaxDis.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMotorYMaxDis.Name = "numMotorYMaxDis";
            this.numMotorYMaxDis.Size = new System.Drawing.Size(78, 21);
            this.numMotorYMaxDis.TabIndex = 81;
            this.numMotorYMaxDis.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chkHasMotorZ
            // 
            this.chkHasMotorZ.AutoSize = true;
            this.chkHasMotorZ.BackColor = System.Drawing.Color.Transparent;
            this.chkHasMotorZ.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.chkHasMotorZ.Location = new System.Drawing.Point(24, 124);
            this.chkHasMotorZ.Name = "chkHasMotorZ";
            this.chkHasMotorZ.Size = new System.Drawing.Size(66, 16);
            this.chkHasMotorZ.Style = Skyray.Controls.Style.Office2007Blue;
            this.chkHasMotorZ.TabIndex = 64;
            this.chkHasMotorZ.Text = "Z轴电机";
            this.chkHasMotorZ.UseVisualStyleBackColor = false;
            // 
            // meTubes
            // 
            this.meTubes.BackColor = System.Drawing.Color.Transparent;
            this.meTubes.DataSource = null;
            this.meTubes.GroupTitle = "";
            this.meTubes.LabelPosition = Skyray.EDX.Common.LabelPosition.Top;
            this.meTubes.LayoutType = null;
            this.meTubes.Location = new System.Drawing.Point(6, 86);
            this.meTubes.Name = "meTubes";
            this.meTubes.Size = new System.Drawing.Size(696, 172);
            this.meTubes.SLayoutType = null;
            this.meTubes.TabIndex = 90;
            // 
            // meDetector
            // 
            this.meDetector.BackColor = System.Drawing.Color.Transparent;
            this.meDetector.DataSource = null;
            this.meDetector.GroupTitle = "";
            this.meDetector.LabelPosition = Skyray.EDX.Common.LabelPosition.Top;
            this.meDetector.LayoutType = null;
            this.meDetector.Location = new System.Drawing.Point(6, 0);
            this.meDetector.Name = "meDetector";
            this.meDetector.Size = new System.Drawing.Size(696, 85);
            this.meDetector.SLayoutType = null;
            this.meDetector.TabIndex = 89;
            // 
            // lbwDevice
            // 
            this.lbwDevice.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(153)))), ((int)(((byte)(194)))));
            this.lbwDevice.ContextMenuStrip = this.cmsRenameDevice;
            this.lbwDevice.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbwDevice.FormattingEnabled = true;
            this.lbwDevice.HorizontalScrollbar = true;
            this.lbwDevice.ItemHeight = 17;
            this.lbwDevice.Location = new System.Drawing.Point(14, 15);
            this.lbwDevice.Name = "lbwDevice";
            this.lbwDevice.Size = new System.Drawing.Size(118, 412);
            this.lbwDevice.Style = Skyray.Controls.Style.Office2007Blue;
            this.lbwDevice.TabIndex = 0;
            this.lbwDevice.SelectedIndexChanged += new System.EventHandler(this.lbwDevice_SelectedIndexChanged);
            this.lbwDevice.DoubleClick += new System.EventHandler(this.lbwDevice_DoubleClick);
            this.lbwDevice.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbwDevice_MouseDown);
            // 
            // cmsRenameDevice
            // 
            this.cmsRenameDevice.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmRename});
            this.cmsRenameDevice.Name = "contextMenuStrip1";
            this.cmsRenameDevice.Size = new System.Drawing.Size(113, 26);
            // 
            // tsmRename
            // 
            this.tsmRename.Name = "tsmRename";
            this.tsmRename.Size = new System.Drawing.Size(112, 22);
            this.tsmRename.Text = "重命名";
            this.tsmRename.Click += new System.EventHandler(this.tsmRename_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Num";
            this.dataGridViewTextBoxColumn1.HeaderText = "编号";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 58;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Step";
            this.dataGridViewTextBoxColumn2.HeaderText = "步数";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 72;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Caption";
            this.dataGridViewTextBoxColumn3.HeaderText = "成分";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 72;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.DataPropertyName = "FilterThickness";
            this.dataGridViewTextBoxColumn4.HeaderText = "厚度";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "Num";
            this.dataGridViewTextBoxColumn5.HeaderText = "编号";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 86;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "Diameter";
            this.dataGridViewTextBoxColumn6.HeaderText = "直径(mm)";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Width = 90;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "Step";
            this.dataGridViewTextBoxColumn7.HeaderText = "步数";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.Width = 110;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "Num";
            this.dataGridViewTextBoxColumn8.HeaderText = "编号";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Width = 70;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "Step";
            this.dataGridViewTextBoxColumn9.HeaderText = "步长";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.Width = 72;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "Caption";
            this.dataGridViewTextBoxColumn10.HeaderText = "成分";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.Width = 72;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "TargetThickness";
            this.dataGridViewTextBoxColumn11.HeaderText = "厚度";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.Width = 72;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.DataPropertyName = "Num";
            this.dataGridViewTextBoxColumn12.HeaderText = "编号";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            this.dataGridViewTextBoxColumn12.Width = 72;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.DataPropertyName = "Step";
            this.dataGridViewTextBoxColumn13.HeaderText = "步数";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.Width = 71;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.DataPropertyName = "StepCoef1";
            this.dataGridViewTextBoxColumn14.HeaderText = "偏移+";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.Width = 72;
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.DataPropertyName = "StepCoef2";
            this.dataGridViewTextBoxColumn15.HeaderText = "偏移-";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.Width = 71;
            // 
            // FrmDevice
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtNewDevice);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApplication);
            this.Controls.Add(this.tabCWDevice);
            this.Controls.Add(this.lbwDevice);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnOK);
            this.Name = "FrmDevice";
            this.Size = new System.Drawing.Size(872, 627);
            this.Load += new System.EventHandler(this.FrmDevice_Load);
            this.tabCWDevice.ResumeLayout(false);
            this.tpDevice.ResumeLayout(false);
            this.tpDevice.PerformLayout();
            this.grpFPGA.ResumeLayout(false);
            this.grpFPGA.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQCProtect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numZeroOffSetDpp100)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIntercept)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFastLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSlowLimit)).EndInit();
            this.grpBlueTooth.ResumeLayout(false);
            this.grpBlueTooth.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numComNum)).EndInit();
            this.grpUSB.ResumeLayout(false);
            this.grpUSB.PerformLayout();
            this.grpDP5Params.ResumeLayout(false);
            this.grpDP5Params.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDP5FastThreshold)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.grpDevice.ResumeLayout(false);
            this.grpDevice.PerformLayout();
            this.pnlVacuum.ResumeLayout(false);
            this.pnlVacuum.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numVoltageScaleFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCurrentScaleFactor)).EndInit();
            this.tpFilter.ResumeLayout(false);
            this.tpFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFilterSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCollimatorSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFilterMaxNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCollimatorMaxNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFilterDirect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFilterCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCollimatorDirect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCollimatorCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvwFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvwCollimator)).EndInit();
            this.tpTarget.ResumeLayout(false);
            this.tpTarget.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTargetSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTargetMaxNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTargetDirect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTargetCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvwTarget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChamberSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChamberMaxNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChamberDirect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChamberCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvwChamber)).EndInit();
            this.tpother.ResumeLayout(false);
            this.gpXYZ.ResumeLayout(false);
            this.gpXYZ.PerformLayout();
            this.pnlEncoderMotor.ResumeLayout(false);
            this.pnlEncoderMotor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorEncoderSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorEncoderCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorEncoderDirect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorEncoderMaxStep)).EndInit();
            this.pnlLightMotor.ResumeLayout(false);
            this.pnlLightMotor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorLightSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorLightCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorLightDirect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorLightMaxStep)).EndInit();
            this.pnlY1Motor.ResumeLayout(false);
            this.pnlY1Motor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorY1Speed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorY1Code)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorY1Direct)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorY1MaxDis)).EndInit();
            this.pnlXMotor.ResumeLayout(false);
            this.pnlXMotor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorXSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorXCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorXDirect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorXMaxDis)).EndInit();
            this.pnlZMotor.ResumeLayout(false);
            this.pnlZMotor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDutyDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDutyUp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorZSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorZCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorZDirect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorZMaxDis)).EndInit();
            this.pnlYMotor.ResumeLayout(false);
            this.pnlYMotor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorYSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorYCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorYDirect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMotorYMaxDis)).EndInit();
            this.cmsRenameDevice.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Skyray.Controls.ListBoxW lbwDevice;
        private Skyray.Controls.ButtonW btnOK;
        private Skyray.Controls.ButtonW btnCancel;
        private Skyray.Controls.ButtonW btnAdd;
        private Skyray.Controls.TextBoxW txtNewDevice;
        private Skyray.Controls.ButtonW btnDel;
        private System.Windows.Forms.TabPage tpFilter;
        private Skyray.Controls.DataGridViewW dgvwFilter;
        private Skyray.Controls.DataGridViewW dgvwCollimator;
        private System.Windows.Forms.TabPage tpDevice;
        private Skyray.Controls.TabControlW tabCWDevice;
        private Skyray.Controls.CheckBoxW chkCollimator;
        private Skyray.Controls.CheckBoxW chkFilter;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private Skyray.Controls.LabelW lblCollimatorCode;
        private Skyray.Controls.NumricUpDownW numCollimatorCode;
        private Skyray.Controls.LabelW lblCollimatorDirect;
        private Skyray.Controls.NumricUpDownW numCollimatorDirect;
        private Skyray.Controls.NumricUpDownW numFilterDirect;
        private Skyray.Controls.LabelW lblFilterDirect;
        private Skyray.Controls.NumricUpDownW numFilterCode;
        private Skyray.Controls.LabelW lblFilterCode;
        private System.Windows.Forms.TabPage tpother;
        private Skyray.Controls.CheckBoxW chkHasMotorZ;
        private Skyray.Controls.CheckBoxW chkHasMotorY;
        private Skyray.Controls.CheckBoxW chkHasMotorX;
        private Skyray.Controls.NumricUpDownW numMotorZCode;
        private Skyray.Controls.LabelW lblMotorZCode;
        private Skyray.Controls.NumricUpDownW numMotorYCode;
        private Skyray.Controls.LabelW lblMotorYCode;
        private Skyray.Controls.NumricUpDownW numMotorXCode;
        private Skyray.Controls.LabelW lblMotorXCode;
        private Skyray.Controls.LabelW lblMotorXMaxDis;
        private Skyray.Controls.NumricUpDownW numMotorZDirect;
        private Skyray.Controls.LabelW lblMotorZDirect;
        private Skyray.Controls.NumricUpDownW numMotorYDirect;
        private Skyray.Controls.LabelW lblMotorYDirect;
        private Skyray.Controls.NumricUpDownW numMotorXDirect;
        private Skyray.Controls.LabelW lblMotorXDirect;
        private Skyray.Controls.NumricUpDownW numMotorZMaxDis;
        private Skyray.Controls.LabelW lblMotorZMaxDis;
        private Skyray.Controls.NumricUpDownW numMotorYMaxDis;
        private Skyray.Controls.LabelW lblMotorYMaxDis;
        private Skyray.Controls.NumricUpDownW numMotorXMaxDis;
        private System.Windows.Forms.Panel pnlXMotor;
        private System.Windows.Forms.Panel pnlZMotor;
        private System.Windows.Forms.Panel pnlYMotor;
        private Skyray.Controls.LabelW lblCollimatorMaxNum;
        private Skyray.Controls.NumricUpDownW numCollimatorMaxNum;
        private Skyray.Controls.NumricUpDownW numFilterMaxNum;
        private Skyray.Controls.LabelW lblFilterMaxNum;
        private Skyray.Controls.LabelW lblCollimatorSpeed;
        private Skyray.Controls.NumricUpDownW numCollimatorSpeed;
        private Skyray.Controls.NumricUpDownW numFilterSpeed;
        private Skyray.Controls.LabelW lblFilterSpeed;
        private Skyray.Controls.LabelW lblMotorXSpeed;
        private Skyray.Controls.NumricUpDownW numMotorXSpeed;
        private Skyray.Controls.NumricUpDownW numMotorYSpeed;
        private Skyray.Controls.LabelW lblMotorYSpeed;
        private Skyray.Controls.NumricUpDownW numMotorZSpeed;
        private Skyray.Controls.LabelW lblMotorZSpeed;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private Skyray.Controls.ButtonW btnApplication;
        private Skyray.Controls.Grouper grpBlueTooth;
        private Skyray.Controls.ComboBoxW cboPocket;
        private Skyray.Controls.LabelW lblPocket;
        private Skyray.Controls.ComboBoxW cboBits;
        private Skyray.Controls.LabelW lblCom;
        private Skyray.Controls.NumricUpDownW numComNum;
        private Skyray.Controls.LabelW lblBits;
        private Skyray.Controls.LabelW lblComNum;
        private System.Windows.Forms.RadioButton radBlueTooth;
        private Skyray.Controls.Grouper grpFPGA;
        private Skyray.Controls.LabelW lblIP;
        private Skyray.Controls.TextBoxW txtIP;
        private Skyray.Controls.LabelW lblSlowLimit;
        private Skyray.Controls.LabelW lblFlatTop;
        private Skyray.Controls.LabelW lblPeakingTime;
        private Skyray.Controls.LabelW lblRate;
        private Skyray.Controls.LabelW lblHeapUP;
        private Skyray.Controls.NumricUpDownW numSlowLimit;
        private Skyray.Controls.ComboBoxW cboRate;
        private Skyray.Controls.ComboBoxW cboFlatTop;
        private Skyray.Controls.ComboBoxW cboPeakingTime;
        private Skyray.Controls.ComboBoxW cboHeapUP;
        private Skyray.Controls.Grouper grpUSB;
        private Skyray.Controls.Grouper grpDP5Params;
        private Skyray.Controls.NumricUpDownW numDP5FastThreshold;
        private Skyray.Controls.ComboBoxW cboDP5HeapUP;
        private Skyray.Controls.ComboBoxW cboDP5FlatTop;
        private Skyray.Controls.ComboBoxW cboDP5PeakingTime;
        private Skyray.Controls.LabelW lblDP5FastThreshold;
        private Skyray.Controls.LabelW lblDP5HeapUP;
        private Skyray.Controls.LabelW lblDP5FlatTop;
        private Skyray.Controls.LabelW lblDP5PeakingTime;
        private Skyray.Controls.ComboBoxW cboPortType;
        private Skyray.Controls.ComboBoxW comboBoxWVersion;
        private System.Windows.Forms.Panel panel4;
        private Skyray.Controls.LabelW lblIsPassword;
        private System.Windows.Forms.RadioButton radIsPassward;
        private System.Windows.Forms.RadioButton radIsPassward2;
        private System.Windows.Forms.Panel panel5;
        private Skyray.Controls.LabelW lblDP5;
        private System.Windows.Forms.RadioButton radIsDP5;
        private System.Windows.Forms.RadioButton radIsNotDP5;
        private System.Windows.Forms.RadioButton radFPGA;
        private System.Windows.Forms.RadioButton radUSB;
        private Skyray.Controls.LabelW lblPortType;
        private Skyray.EDX.Common.ModelEditor meTubes;
        private Skyray.EDX.Common.ModelEditor meDetector;
        private Skyray.Controls.Grouper gpXYZ;
        private Skyray.Controls.Grouper grpDevice;
        private System.Windows.Forms.Panel pnlVacuum;
        private Skyray.Controls.ComboBoxW cboVacuumPumpType;
        private Skyray.Controls.DoubleInputW diMaxCurrent;
        private Skyray.Controls.DoubleInputW diMaxVoltage;
        private Skyray.Controls.LabelW lblMaxCurrent;
        private Skyray.Controls.LabelW lblMaxVoltage;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton radIsAllowOpenCover2;
        private Skyray.Controls.LabelW lblIsAllowOpenCover;
        private System.Windows.Forms.RadioButton radIsAllowOpenCover;
        private System.Windows.Forms.Panel panel2;
        private Skyray.Controls.LabelW lblHasElectromagnet;
        private System.Windows.Forms.RadioButton radHasElectromagnet;
        private System.Windows.Forms.RadioButton radHasElectromagnet2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radVacuumPump;
        private System.Windows.Forms.RadioButton radVacuumPump2;
        private Skyray.Controls.LabelW lblVacuumPump;
        private Skyray.Controls.ComboBoxW cmbSpecLength;
        private Skyray.Controls.LabelW lblVoltageScaleFactor;
        private Skyray.Controls.NumricUpDownW numVoltageScaleFactor;
        private Skyray.Controls.LabelW lblSpecLength;
        private Skyray.Controls.NumricUpDownW numCurrentScaleFactor;
        private Skyray.Controls.LabelW lblCurrentScaleFactor;
        private Skyray.Controls.ComboBoxW cboVoltage;
        private Skyray.Controls.LabelW lblVoltage;
        private Skyray.Controls.LabelW lblVacuumPumpType;
        private System.Windows.Forms.RadioButton radParallel;
        private Skyray.Controls.Grouper grpParallel;
        private System.Windows.Forms.TabPage tpTarget;
        private Skyray.Controls.NumricUpDownW numChamberSpeed;
        private Skyray.Controls.LabelW lblChamberSpeed;
        private Skyray.Controls.NumricUpDownW numChamberMaxNum;
        private Skyray.Controls.LabelW lblChamberMaxNum;
        private Skyray.Controls.NumricUpDownW numChamberDirect;
        private Skyray.Controls.LabelW lblChamberDirect;
        private Skyray.Controls.NumricUpDownW numChamberCode;
        private Skyray.Controls.LabelW lblChamberCode;
        private Skyray.Controls.CheckBoxW chkChamber;
        private Skyray.Controls.DataGridViewW dgvwChamber;
        private Skyray.Controls.NumricUpDownW numTargetSpeed;
        private Skyray.Controls.LabelW lblTargetSpeed;
        private Skyray.Controls.NumricUpDownW numTargetMaxNum;
        private Skyray.Controls.LabelW lblTargetMaxNum;
        private Skyray.Controls.NumricUpDownW numTargetDirect;
        private Skyray.Controls.LabelW lblTargetDirect;
        private Skyray.Controls.NumricUpDownW numTargetCode;
        private Skyray.Controls.LabelW lblTargetCode;
        private Skyray.Controls.CheckBoxW chkTarget;
        private Skyray.Controls.DataGridViewW dgvwTarget;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColTargetNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColTargetStep;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColTargetCaption;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColTargetThickness;
        private Skyray.Controls.LabelW lblFastLimit;
        private Skyray.Controls.NumricUpDownW numFastLimit;
        private System.Windows.Forms.ContextMenuStrip cmsRenameDevice;
        private System.Windows.Forms.ToolStripMenuItem tsmRename;
        private Skyray.Controls.CheckBoxW chkAutoDetection;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColFilterNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColFilterStep;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColCaption;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColFilterThickness;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColCollimatorNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColDiameter;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColCollimatorStep;
        private System.Windows.Forms.Panel pnlY1Motor;
        private Skyray.Controls.NumricUpDownW numMotorY1Speed;
        private Skyray.Controls.LabelW lblMotorY1Speed;
        private Skyray.Controls.LabelW lblMotorY1Code;
        private Skyray.Controls.NumricUpDownW numMotorY1Code;
        private Skyray.Controls.LabelW lblMotorY1Direct;
        private Skyray.Controls.NumricUpDownW numMotorY1Direct;
        private Skyray.Controls.LabelW lblMotorY1MaxDis;
        private Skyray.Controls.NumricUpDownW numMotorY1MaxDis;
        private Skyray.Controls.CheckBoxW chkHasMotorY1;
        private Skyray.Controls.CheckBoxW chkHasMotorLight;
        private System.Windows.Forms.Panel pnlLightMotor;
        private Skyray.Controls.NumricUpDownW numMotorLightSpeed;
        private Skyray.Controls.LabelW lblMotorLightSpeed;
        private Skyray.Controls.LabelW lblMotorLightCode;
        private Skyray.Controls.NumricUpDownW numMotorLightCode;
        private Skyray.Controls.LabelW lblMotorLightDirect;
        private Skyray.Controls.NumricUpDownW numMotorLightDirect;
        private Skyray.Controls.LabelW lblMotorLightMaxStep;
        private Skyray.Controls.NumricUpDownW numMotorLightMaxStep;
        private Skyray.Controls.LabelW lblDutyUp;
        private Skyray.Controls.NumricUpDownW numDutyUp;
        private Skyray.Controls.LabelW lblDutyDown;
        private Skyray.Controls.NumricUpDownW numDutyDown;
        private Skyray.Controls.NumricUpDownW numIntercept;
        private Skyray.Controls.LabelW lblIntercept;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        //private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private Skyray.Controls.LabelW lblDp5Type;
        private Skyray.Controls.ComboBoxW cboDp5Type;
        private Skyray.Controls.LabelW lblDp5IP;
        private Skyray.Controls.TextBoxW txtDp5Ip;
        private System.Windows.Forms.RadioButton radioDMCA;
        private System.Windows.Forms.RadioButton radioDpp100;
        private Skyray.Controls.LabelW lblGain;
        private Skyray.Controls.ComboBoxW cboGain;
        private Skyray.Controls.ComboBoxW cboPeakingTimDpp100;
        private Skyray.Controls.ComboBoxW cboFlatTopDpp100;
        private Skyray.Controls.ComboBoxW cboPreAMPDpp100;
        private Skyray.Controls.LabelW lblPreAmp;
        private Skyray.Controls.NumricUpDownW numZeroOffSetDpp100;
        private Skyray.Controls.LabelW lblZeroOffset;
        private Skyray.Controls.LabelW lblBLRSpeed;
        private Skyray.Controls.ComboBoxW cboBLRSpeedDpp100;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColChamberNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColChamberStep;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColChamberStepCoef1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColChamberStepCoef2;
        private Skyray.Controls.NumricUpDownW numQCProtect;
        private Skyray.Controls.LabelW lblQCProtect;
        private Skyray.Controls.ButtonW btnSampleCal;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private System.Windows.Forms.Panel pnlEncoderMotor;
        private Skyray.Controls.NumricUpDownW numMotorEncoderSpeed;
        private Skyray.Controls.LabelW lblMotorEncoderSpeed;
        private Skyray.Controls.LabelW lblMotorEncoderCode;
        private Skyray.Controls.NumricUpDownW numMotorEncoderCode;
        private Skyray.Controls.LabelW lblMotorEncoderDirect;
        private Skyray.Controls.NumricUpDownW numMotorEncoderDirect;
        private Skyray.Controls.LabelW lblMotorEncoderMaxStep;
        private Skyray.Controls.NumricUpDownW numMotorEncoderMaxStep;
        private Skyray.Controls.CheckBoxW chkHasMotorEncoder;
    }
}