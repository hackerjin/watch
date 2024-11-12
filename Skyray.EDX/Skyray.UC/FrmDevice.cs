using System;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;
using Skyray.Controls;
using Skyray.EDX.Common;
using Lephone.Data.Common;
using Lephone.Data.Definition;
using Skyray.EDX.Common.Component;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Skyray.UC
{
    /// <summary>
    /// 测量设备信息
    /// </summary>
    public partial class FrmDevice : Skyray.Language.UCMultiple
    {

        #region Fields

        /// <summary>
        /// 所有设备集合 
        /// </summary>
        private DbObjectList<Device> lstDevice;

        /// <summary>
        /// 当前设备
        /// </summary>
        private Device devCurrent;

        public Dp5Interface dp5Device;

        //public double[] Peaking;

        #endregion

        #region Init

        /// <summary>
        /// 加载时设置控件值范围
        /// </summary>
        public FrmDevice()
        {
            InitializeComponent();

            SetCtrlRelation();//设置控件关系

            SetCtrlRangeInfo();//设置控件边界信息            
           
            Skyray.Language.Lang.Model.LanguageChanged += new EventHandler(Model_LanguageChanged);//语言改变事件（主要用于改变自动生成控件部分的语言）

        }

        private void FrmDevice_Load(object sender, EventArgs e)
        {
            //string IsDetection = ReportTemplateHelper.LoadSpecifiedValue("AutoDetection", "IsDetection");
            //if (IsDetection == "0") chkAutoDetection.Visible = false;
            //else chkAutoDetection.Visible = true; 
            this.tpTarget.Parent = null;
            InitControls();//自动生成控件

            lstDevice = Device.FindAll();//所有设备

            foreach (var device in lstDevice)
            {
                lbwDevice.Items.Add(device.Name);//控件加载当前设备
            }
            
            if (lstDevice.Count > 0)
            {
                devCurrent = WorkCurveHelper.DeviceCurrent;//当前设备
                if (devCurrent != null) lbwDevice.SelectedItem = devCurrent.Name;//选中当前设备
            }
            BindCurrentDeviceInfo();//绑定当前设备信息
            if (DifferenceDevice.IsAnalyser)
            { this.panel1.Visible = false; }
            btnSampleCal.Visible = DifferenceDevice.IsThick;
        }

        void Model_LanguageChanged(object sender, EventArgs e)
        {
            Skyray.Language.Lang.Model.SetTextProperty(false, this.meDetector.LabelControls);
            Skyray.Language.Lang.Model.SetTextProperty(false, this.meTubes.LabelControls);
        }

        /// <summary>
        /// 设置控件取值范围
        /// </summary>
        private void SetCtrlRangeInfo()
        {
            numVoltageScaleFactor.Maximum = Ranges.RangeDictionary["VoltageScaleFactor"].Max;//管压比例因子
            numVoltageScaleFactor.DecimalPlaces = Ranges.RangeDictionary["VoltageScaleFactor"].DecimalPlaces;
            numVoltageScaleFactor.Increment = Ranges.RangeDictionary["VoltageScaleFactor"].Increment;
            numCurrentScaleFactor.Maximum = Ranges.RangeDictionary["CurrentScaleFactor"].Max;//管压比例因子
            numCurrentScaleFactor.DecimalPlaces = Ranges.RangeDictionary["CurrentScaleFactor"].DecimalPlaces;
            numCurrentScaleFactor.Increment = Ranges.RangeDictionary["CurrentScaleFactor"].Increment;
            numCollimatorCode.Maximum = Ranges.RangeDictionary["ElectricalCode"].Max;//设置准直器电机编号最大值
            numCollimatorDirect.Maximum = Ranges.RangeDictionary["ElectricalDirect"].Max;//设置准直器方向最大值
            numFilterCode.Maximum = Ranges.RangeDictionary["ElectricalCode"].Max;//设置滤光片电机编号最大值
            numFilterDirect.Maximum = Ranges.RangeDictionary["ElectricalDirect"].Max;//设置滤光片方向最大值
            numChamberCode.Maximum = Ranges.RangeDictionary["ElectricalCode"].Max;//设置样品腔电机编号最大值
            numChamberDirect.Maximum = Ranges.RangeDictionary["ElectricalDirect"].Max;//设置样品腔方向最大值
            numTargetCode.Maximum = Ranges.RangeDictionary["ElectricalCode"].Max;//设置靶材电机编号最大值
            numTargetDirect.Maximum = Ranges.RangeDictionary["ElectricalDirect"].Max;//设置靶材方向最大值
            numMotorXCode.Maximum = Ranges.RangeDictionary["MotorXCode"].Max;//X轴电机编号最大值
            numMotorXDirect.Maximum = Ranges.RangeDictionary["MotorXDirect"].Max;//X轴电机方向最大值
            numMotorXMaxDis.Maximum = Ranges.RangeDictionary["MotorXMaxStep"].Max;//X轴电机步长最大值
            numMotorYCode.Maximum = Ranges.RangeDictionary["MotorYCode"].Max;//Y轴电机编号最大值
            numMotorYDirect.Maximum = Ranges.RangeDictionary["MotorYDirect"].Max;//Y轴电机方向最大值
            numMotorYMaxDis.Maximum = Ranges.RangeDictionary["MotorYMaxStep"].Max;//Y轴电机步长最大值
            numMotorZCode.Maximum = Ranges.RangeDictionary["MotorZCode"].Max;//Z轴电机编号最大值
            numMotorZDirect.Maximum = Ranges.RangeDictionary["MotorZDirect"].Max;//Z轴电机方向最大值
            numMotorZMaxDis.Maximum = Ranges.RangeDictionary["MotorZMaxStep"].Max;//Z轴电机步长最大值
            numCollimatorMaxNum.Maximum = Ranges.RangeDictionary["CollimatorMaxNum"].Max;//设置准直器最大编号
            numFilterMaxNum.Maximum = Ranges.RangeDictionary["FilterMaxNum"].Max;//设置滤光片最大编号
            numChamberMaxNum.Maximum = Ranges.RangeDictionary["ChamberMaxNum"].Max;//设置样品腔最大编号
            numTargetMaxNum.Maximum = Ranges.RangeDictionary["TargetMaxNum"].Max;//设置靶材最大编号
            numMotorY1Code.Maximum = Ranges.RangeDictionary["MotorY1Code"].Max;//Y1电机编号最大值
            numMotorY1Direct.Maximum = Ranges.RangeDictionary["MotorY1Direct"].Max;//Y1电机方向最大值
            numMotorY1MaxDis.Maximum = Ranges.RangeDictionary["MotorY1MaxStep"].Max;//Y1电机步长最大值
            
        }

        /// <summary>
        /// 实例自动生成控件
        /// </summary>
        private void InitControls()
        {
            CtrlHelper.EnumToComboBox(cboPortType, typeof(DllType));//枚举绑定至下拉框
            CtrlHelper.EnumToComboBox(comboBoxWVersion, typeof(UsbVersion));//枚举绑定至下拉框
            CtrlHelper.EnumToComboBox(cboHeapUP, typeof(OFFON));
            CtrlHelper.EnumToComboBox(cboDp5Type, typeof(Dp5Version));
            cboDp5Type.Items.Remove(Dp5Version.Dp5_FastUsb.ToString());//先删除fastusb
            cboVacuumPumpType.Items.AddRange(new string[] { Info.Atmospheric, Info.VacuumSi, Info.Fixed });
            cboPeakingTime.Items.AddRange(new string[]{"0.8","1.6","2.4","3.2","4.0","4.8","5.6","6.4","8.0","9.6","11.2","12.8","16.0","19.2","22.4","25.6",
                                                        "32.0","38.4","44.8","51.2","64.0","76.8","89.6","102.4"});
            cboRate.Items.AddRange(new string[] { "20M", "80M" });
            cboBits.Items.AddRange(new string[] { "19200", "38400" });
            cboPocket.Items.Clear();
            cboPocket.Items.AddRange(new object[] { Pocket.PocketIII, Pocket.PortableI });//线系 
            this.cmbSpecLength.Items.Add(1024);//谱长度
            this.cmbSpecLength.Items.Add(2048);
            this.cmbSpecLength.Items.Add(4096);
            meDetector.Init(typeof(Detector), LayoutSource.New.Init(false, true, true, string.Empty, string.Empty, 3, LabelPosition.Top));
            meTubes.Init(typeof(Tubes), LayoutSource.New.Init(false, true, true, string.Empty, string.Empty, 3, LabelPosition.Top));
            Skyray.Language.Lang.Model.SaveTextProperty(false, this.meDetector.LabelControls);
            Skyray.Language.Lang.Model.SaveTextProperty(false, this.meTubes.LabelControls);
            Skyray.Language.Lang.Model.SaveProperty(new string[] { "GroupTitle" }, this.meDetector, this.meTubes);
            Skyray.Language.Lang.Model.SetTextProperty(false, this.meDetector.LabelControls);
            Skyray.Language.Lang.Model.SetTextProperty(false, this.meTubes.LabelControls);
            Skyray.Language.Lang.Model.SetProperty(new string[] { "GroupTitle" }, this.meDetector, this.meTubes);
        }

        /// <summary>
        /// 设置控件关系
        /// </summary>
        private void SetCtrlRelation()
        {
            pnlXMotor.BindEnabledToCtrl(chkHasMotorX, "Checked");
            pnlYMotor.BindEnabledToCtrl(chkHasMotorY, "Checked");
            pnlZMotor.BindEnabledToCtrl(chkHasMotorZ, "Checked");
            pnlY1Motor.BindEnabledToCtrl(chkHasMotorY1, "Checked");
            pnlLightMotor.BindEnabledToCtrl(chkHasMotorLight, "Checked");
            dgvwCollimator.BindEnabledToCtrl(chkCollimator, "Checked");
            dgvwFilter.BindEnabledToCtrl(chkFilter, "Checked");
            dgvwChamber.BindEnabledToCtrl(chkChamber, "Checked");
            dgvwTarget.BindEnabledToCtrl(chkTarget, "Checked");
            numCollimatorCode.BindEnabledToCtrl(chkCollimator, "Checked");
            numCollimatorDirect.BindEnabledToCtrl(chkCollimator, "Checked");
            numCollimatorMaxNum.BindEnabledToCtrl(chkCollimator, "Checked");
            numCollimatorSpeed.BindEnabledToCtrl(chkCollimator, "Checked");
            numFilterCode.BindEnabledToCtrl(chkFilter, "Checked");
            numFilterDirect.BindEnabledToCtrl(chkFilter, "Checked");
            numFilterMaxNum.BindEnabledToCtrl(chkFilter, "Checked");
            numFilterSpeed.BindEnabledToCtrl(chkFilter, "Checked");
            numChamberCode.BindEnabledToCtrl(chkChamber, "Checked");
            numChamberDirect.BindEnabledToCtrl(chkChamber, "Checked");
            numChamberMaxNum.BindEnabledToCtrl(chkChamber, "Checked");
            numChamberSpeed.BindEnabledToCtrl(chkChamber, "Checked");
            numTargetCode.BindEnabledToCtrl(chkTarget, "Checked");
            numTargetDirect.BindEnabledToCtrl(chkTarget, "Checked");
            numTargetMaxNum.BindEnabledToCtrl(chkTarget, "Checked");
            numTargetSpeed.BindEnabledToCtrl(chkTarget, "Checked");
            grpParallel.BindVisibleToCtrl(radParallel, "Checked");
            grpDP5Params.BindVisibleToCtrl(radIsDP5, "Checked");
            lblDp5Type.BindVisibleToCtrl(radIsDP5, "Checked");
            cboDp5Type.BindVisibleToCtrl(radIsDP5, "Checked");
            grpUSB.BindVisibleToCtrl(radUSB, "Checked");
            grpFPGA.BindVisibleToCtrl(radFPGA, "Checked");
            //追加Dpp100的参数设置begin-----------
            numFastLimit.BindVisibleToCtrl(radioDpp100, "Checked");
            lblFastLimit.BindVisibleToCtrl(radioDpp100, "Checked");
            lblGain.BindVisibleToCtrl(radioDpp100, "Checked");
            cboGain.BindVisibleToCtrl(radioDpp100, "Checked");
            //lblFlatTop.BindVisibleToCtrl(radioDMCA, "Checked");
            cboFlatTop.BindVisibleToCtrl(radioDMCA, "Checked");
            lblRate.BindVisibleToCtrl(radioDMCA, "Checked");
            cboRate.BindVisibleToCtrl(radioDMCA, "Checked");
            lblIntercept.BindVisibleToCtrl(radioDMCA, "Checked");
            numIntercept.BindVisibleToCtrl(radioDMCA, "Checked");
            cboPeakingTimDpp100.BindVisibleToCtrl(radioDpp100, "Checked");
            cboPeakingTime.BindVisibleToCtrl(radioDMCA, "Checked");

            lblBLRSpeed.BindVisibleToCtrl(radioDpp100, "Checked");
            cboBLRSpeedDpp100.BindVisibleToCtrl(radioDpp100, "Checked");
            lblZeroOffset.BindVisibleToCtrl(radioDpp100, "Checked");
            numZeroOffSetDpp100.BindVisibleToCtrl(radioDpp100, "Checked");
            lblPreAmp.BindVisibleToCtrl(radioDpp100, "Checked");
            cboPreAMPDpp100.BindVisibleToCtrl(radioDpp100, "Checked");
            cboFlatTopDpp100.BindVisibleToCtrl(radioDpp100, "Checked");
            
            lblQCProtect.BindVisibleToCtrl(radioDpp100, "Checked");
            numQCProtect.BindVisibleToCtrl(radioDpp100, "Checked");
            //追加Dpp100的参数设置end-------------

            grpBlueTooth.BindVisibleToCtrl(radBlueTooth, "Checked");
            pnlVacuum.BindVisibleToCtrl(radVacuumPump, "Checked");
            
        }

        #endregion

        #region Methods

        /// <summary>
        /// 绑定当前设备信息
        /// </summary>
        private void BindCurrentDeviceInfo()
        {
            if (devCurrent == null) return;
            BindHelper.BindCheckedToCtrl(chkCollimator, devCurrent, "HasCollimator", true);//控件与数据源绑定
            BindHelper.BindCheckedToCtrl(chkFilter, devCurrent, "HasFilter", true);
            BindHelper.BindCheckedToCtrl(chkChamber, devCurrent, "HasChamber", true);
            BindHelper.BindCheckedToCtrl(chkTarget, devCurrent, "HasTarget", true);
            BindHelper.BindCheckedToCtrl(chkHasMotorX, devCurrent, "HasMotorX", true);
            BindHelper.BindCheckedToCtrl(chkHasMotorY, devCurrent, "HasMotorY", true);
            BindHelper.BindCheckedToCtrl(chkHasMotorZ, devCurrent, "HasMotorZ", true);
            BindHelper.BindCheckedToCtrl(chkHasMotorY1, devCurrent, "HasMotorSpin", true);
            BindHelper.BindCheckedToCtrl(chkHasMotorLight, devCurrent, "HasMotorLight", true);

            BindHelper.BindTextToCtrl(numCollimatorCode, devCurrent, "CollimatorElectricalCode", true);
            BindHelper.BindTextToCtrl(numCollimatorDirect, devCurrent, "CollimatorElectricalDirect", true);
            BindHelper.BindTextToCtrl(numCollimatorSpeed, devCurrent, "CollimatorSpeed", true);
            BindHelper.BindTextToCtrl(numFilterCode, devCurrent, "FilterElectricalCode", true);
            BindHelper.BindTextToCtrl(numFilterDirect, devCurrent, "FilterElectricalDirect", true);
            BindHelper.BindTextToCtrl(numFilterSpeed, devCurrent, "FilterSpeed", true);
            BindHelper.BindTextToCtrl(numChamberCode, devCurrent, "ChamberElectricalCode", true);
            BindHelper.BindTextToCtrl(numChamberDirect, devCurrent, "ChamberElectricalDirect", true);
            BindHelper.BindTextToCtrl(numChamberSpeed, devCurrent, "ChamberSpeed", true);

            BindHelper.BindTextToCtrl(numTargetCode, devCurrent, "TargetElectricalCode", true);
            BindHelper.BindTextToCtrl(numTargetDirect, devCurrent, "TargetElectricalDirect", true);
            BindHelper.BindTextToCtrl(numTargetSpeed, devCurrent, "TargetSpeed", true);

            BindHelper.BindTextToCtrl(numMotorXCode, devCurrent, "MotorXCode", true);
            BindHelper.BindTextToCtrl(numMotorXDirect, devCurrent, "MotorXDirect", true);
            BindHelper.BindTextToCtrl(numMotorXSpeed, devCurrent, "MotorXSpeed", true);
            BindHelper.BindTextToCtrl(numMotorXMaxDis, devCurrent, "MotorXMaxStep", true);
            BindHelper.BindTextToCtrl(numMotorYCode, devCurrent, "MotorYCode", true);
            BindHelper.BindTextToCtrl(numMotorYDirect, devCurrent, "MotorYDirect", true);
            BindHelper.BindTextToCtrl(numMotorYSpeed, devCurrent, "MotorYSpeed", true);
            BindHelper.BindTextToCtrl(numMotorYMaxDis, devCurrent, "MotorYMaxStep", true);
            BindHelper.BindTextToCtrl(numMotorZCode, devCurrent, "MotorZCode", true);
            BindHelper.BindTextToCtrl(numMotorZDirect, devCurrent, "MotorZDirect", true);
            BindHelper.BindTextToCtrl(numMotorZSpeed, devCurrent, "MotorZSpeed", true);
            BindHelper.BindTextToCtrl(numMotorZMaxDis, devCurrent, "MotorZMaxStep", true);
            BindHelper.BindTextToCtrl(numDutyUp, devCurrent, "MotorZDutyRatioUp", true);
            BindHelper.BindTextToCtrl(numDutyDown, devCurrent, "MotorZDutyRatioDown", true);
            BindHelper.BindTextToCtrl(numMotorY1Code, devCurrent, "MotorSpinCode", true);
            BindHelper.BindTextToCtrl(numMotorY1Direct, devCurrent, "MotorSpinDirect", true);
            BindHelper.BindTextToCtrl(numMotorY1Speed, devCurrent, "MotorSpinSpeed", true);
            BindHelper.BindTextToCtrl(numMotorY1MaxDis, devCurrent, "MotorSpinMaxStep", true);

            BindHelper.BindTextToCtrl(numMotorLightCode, devCurrent, "MotorLightCode", true);
            BindHelper.BindTextToCtrl(numMotorLightDirect, devCurrent, "MotorLightDirect", true);
            BindHelper.BindTextToCtrl(numMotorLightSpeed, devCurrent, "MotorLightSpeed", true);
            BindHelper.BindTextToCtrl(numMotorLightMaxStep, devCurrent, "MotorLightMaxStep", true);

            BindHelper.BindTextToCtrl(cboPortType, devCurrent, "PortType", true);
            BindHelper.BindTextToCtrl(comboBoxWVersion, devCurrent, "UsbVersion", true);
            BindHelper.BindValueToCtrl(numCollimatorMaxNum, devCurrent, "CollimatorMaxNum");
            BindHelper.BindValueToCtrl(numFilterMaxNum, devCurrent, "FilterMaxNum");
            BindHelper.BindValueToCtrl(numChamberMaxNum, devCurrent, "ChamberMaxNum");
            BindHelper.BindValueToCtrl(numTargetMaxNum, devCurrent, "TargetMaxNum");
            BindHelper.BindTextToCtrl(numVoltageScaleFactor, devCurrent, "VoltageScaleFactor",true);
            BindHelper.BindTextToCtrl(numCurrentScaleFactor, devCurrent, "CurrentScaleFactor",true);
            BindHelper.BindTextToCtrl(diMaxVoltage, devCurrent, "MaxVoltage", true);
            BindHelper.BindTextToCtrl(diMaxCurrent, devCurrent, "MaxCurrent", true);
            BindHelper.BindTextToCtrl(cboDp5Type, devCurrent, "Dp5Version", true);

            if (!devCurrent.HasCollimator) dgvwCollimator.Enabled = false;
            if (!devCurrent.HasFilter) dgvwFilter.Enabled = false;
            if (!devCurrent.HasChamber) dgvwChamber.Enabled = false;
            if (!devCurrent.HasTarget) dgvwTarget.Enabled = false;
            BindingCollimators();
            BindingFilter();
            BindingChamber();
            BindingTarget();
            cmbSpecLength.Text = ((int)devCurrent.SpecLength).ToString();
            BindHelper.BindCheckedToCtrl(radVacuumPump, devCurrent, "HasVacuumPump");
            if (!radVacuumPump.Checked) radVacuumPump2.Checked = true;
            BindHelper.BindCheckedToCtrl(radHasElectromagnet, devCurrent, "HasElectromagnet");
            if (!radHasElectromagnet.Checked) radHasElectromagnet2.Checked = true;
            BindHelper.BindCheckedToCtrl(radIsAllowOpenCover, devCurrent, "IsAllowOpenCover");
            if (!radIsAllowOpenCover.Checked) radIsAllowOpenCover2.Checked = true;
            BindHelper.BindCheckedToCtrl(radIsPassward, devCurrent, "IsPassward");
            if (!radIsPassward.Checked) radIsPassward2.Checked = true;

           
            if (devCurrent.FPGAParams == null)
            {
                FPGAParams fpga = Default.GetFPGAParams();
                devCurrent.FPGAParams = fpga;
            }
            BindHelper.BindTextToCtrl(cboHeapUP, devCurrent.FPGAParams, "HeapUP", true);
            BindHelper.BindTextToCtrl(cboPeakingTime, devCurrent.FPGAParams, "PeakingTime", true);
            BindHelper.BindTextToCtrl(cboFlatTop, devCurrent.FPGAParams, "FlatTop", true);
            BindHelper.BindSelectedIndexToCtrl(cboRate, devCurrent.FPGAParams, "Rate", true);
            BindHelper.BindValueToCtrl(numFastLimit, devCurrent.FPGAParams, "FastLimit", true);//控件绑定错误导致slowLimit设置为0后，下次打开界面又变成初始值
            BindHelper.BindValueToCtrl(numSlowLimit, devCurrent.FPGAParams, "SlowLimit", true);//
            BindHelper.BindTextToCtrl(txtIP, devCurrent.FPGAParams, "IP", true);
            BindHelper.BindTextToCtrl(numIntercept, devCurrent.FPGAParams, "Intercept", true);
            BindHelper.BindTextToCtrl(txtDp5Ip, devCurrent.FPGAParams, "IP", true);

            if (devCurrent.ComType == ComType.BlueTooth)
            {
                radBlueTooth.Checked = true;
            }
            else if (devCurrent.ComType == ComType.FPGA)
            {
                radFPGA.Checked = true;
            }
            else if(devCurrent.ComType == ComType.USB)
            {
                radUSB.Checked = true;
            }
            else if (devCurrent.ComType == ComType.Parallel)
            {
                radParallel.Checked = true;
            }
            BindHelper.BindCheckedToCtrl(radIsDP5, devCurrent, "IsDP5");

            if (!radIsDP5.Checked) radIsNotDP5.Checked = true;
            BindHelper.BindTextToCtrl(numComNum, devCurrent, "ComNum", true);
            BindHelper.BindTextToCtrl(cboBits, devCurrent, "Bits", true);
            BindHelper.BindTextToCtrl(cboPocket, devCurrent, "Pocket", true);

            //chkAutoDetection.Checked = devCurrent.IsAutoDetection;
            switch (devCurrent.VacuumPumpType)
            {
                case VacuumPumpType.Atmospheric:
                    cboVacuumPumpType.Text = Info.Atmospheric;
                    break;
                case VacuumPumpType.VacuumSi:
                    cboVacuumPumpType.Text = Info.VacuumSi;
                    break;
                case VacuumPumpType.Fixed:
                    cboVacuumPumpType.Text = Info.Fixed;
                    break;
            }

            //追加Dpp100的参数设置begin-----------
            BindHelper.BindCheckedToCtrl(radioDpp100, devCurrent, "IsDP5");
            cboPeakingTimDpp100.Items.Clear();
            //cboPeakingTimDpp100.Items.AddRange(new string[] { "0.2uS", "0.4uS", "0.8uS", "1.6uS", "3.2uS" });
            cboPeakingTimDpp100.Items.AddRange(new string[] { "0.2uS", "0.4uS", "0.8uS", "1.6uS", "3.2uS", "6.4uS", "12.8uS", "25.6uS" });
            cboFlatTopDpp100.Items.Clear();
            cboFlatTopDpp100.Items.AddRange(new string[] { "0.2uS", "0.4uS"});

            cboPreAMPDpp100.Items.Clear();
            cboPreAMPDpp100.Items.AddRange(new string[] { "25.6uS", "51.2uS", "102.4uS", "204.8uS" });

            cboBLRSpeedDpp100.Items.Clear();
            cboBLRSpeedDpp100.Items.AddRange(new string[] { "Fast", "Medium", "Slow", "Very Slow" });

            cboGain.Items.Clear();
            cboGain.Items.AddRange(new string[] { "GAIN1", "GAIN2", "GAIN4" });
            radioDMCA.Checked = !radioDpp100.Checked;
            radioDMCA_CheckedChanged(null, null);
            //追加Dpp100的参数设置end-------------

            this.chkHasMotorEncoder.Checked = WorkCurveHelper.hasMotorEncoder;
            this.numMotorEncoderCode.Value = WorkCurveHelper.encoderMotorCode;
        }

        /// <summary>
        /// 绑准直器
        /// </summary>
        private void BindingCollimators()
        {
            BindingSource bsCollimators = new BindingSource();
            for (int i = 0; i < devCurrent.Collimators.Count; i++)
            {
                bsCollimators.Add(devCurrent.Collimators[i]);
            }
            this.dgvwCollimator.AutoGenerateColumns = false;
            this.dgvwCollimator.DataSource = bsCollimators;//绑定准直器
        }

        /// <summary>
        /// 绑滤光片
        /// </summary>
        private void BindingFilter()
        {
            BindingSource bsFilter = new BindingSource();
            for (int i = 0; i < devCurrent.Filter.Count; i++)
            {
                bsFilter.Add(devCurrent.Filter[i]);
            }
            this.dgvwFilter.AutoGenerateColumns = false;
            this.dgvwFilter.DataSource = bsFilter;//绑定滤光片
        }

        /// <summary>
        /// 绑样品腔
        /// </summary>
        private void BindingChamber()
        {
            BindingSource Chamber = new BindingSource();
            for (int i = 0; i < devCurrent.Chamber.Count; i++)
            {
                Chamber.Add(devCurrent.Chamber[i]);
            }
            this.dgvwChamber.AutoGenerateColumns = false;
            this.dgvwChamber.DataSource = Chamber;//绑定样品腔
        }

        private void BindingTarget()
        {
            BindingSource Target = new BindingSource();
            for (int i = 0; i < devCurrent.Target.Count; i++)
            {
                Target.Add(devCurrent.Target[i]);
            }
            this.dgvwTarget.AutoGenerateColumns = false;
            this.dgvwTarget.DataSource = Target;//绑定靶材
        }

        public void SaveDevice()
        {

            if (devCurrent == null)
            {
                EDXRFHelper.GotoMainPage(this);//返回主界面
                return;
            }

            WorkCurveHelper.hasMotorEncoder = this.chkHasMotorEncoder.Checked;
            WorkCurveHelper.encoderMotorCode = (int)this.numMotorEncoderCode.Value;


            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/hasMotorEncoder", WorkCurveHelper.hasMotorEncoder.ToString());

            Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/encoderMotorCode", WorkCurveHelper.encoderMotorCode.ToString());

            DifferenceDevice.CurCameraControl.GbxEncoderMove.Visible = WorkCurveHelper.hasMotorEncoder;

            List<int> lh = new List<int>();
            if (devCurrent.HasChamber)
                lh.Add(devCurrent.ChamberElectricalCode);
            if (devCurrent.HasCollimator)
                lh.Add(devCurrent.CollimatorElectricalCode);
            if (devCurrent.HasFilter)
                lh.Add(devCurrent.FilterElectricalCode);
            if (devCurrent.HasTarget)
                lh.Add(devCurrent.TargetElectricalCode);
            if (devCurrent.HasMotorX)
                lh.Add(devCurrent.MotorXCode);
            if (devCurrent.HasMotorY)
                lh.Add(devCurrent.MotorYCode);
            if (devCurrent.HasMotorZ)
                lh.Add(devCurrent.MotorZCode);
            if (devCurrent.HasMotorSpin)
                lh.Add(devCurrent.MotorSpinCode);
            int aaa = Enumerable.Distinct(lh).Count();
            if (aaa != lh.Count)
            {
                Msg.Show(Info.ElectricalRepeat);
                return;
            }

            try
            {
                devCurrent.SpecLength = (SpecLength)(int.Parse(cmbSpecLength.Text));
                if (cboVacuumPumpType.Text.Equals(Info.Atmospheric))
                {
                    devCurrent.VacuumPumpType = VacuumPumpType.Atmospheric;
                }
                else if (cboVacuumPumpType.Text.Equals(Info.VacuumSi))
                {
                    devCurrent.VacuumPumpType = VacuumPumpType.VacuumSi;
                }
                else if (cboVacuumPumpType.Text.Equals(Info.Fixed))
                {
                    devCurrent.VacuumPumpType = VacuumPumpType.Fixed;
                }
            }
            catch (Exception) { Msg.Show(Info.DataError); }



            WorkCurveHelper.DeviceCurrent.MotorXMaxStep = (int)numMotorXMaxDis.Value;
            WorkCurveHelper.DeviceCurrent.MotorYMaxStep = (int)numMotorYMaxDis.Value;


            //devCurrent.IsAutoDetection = chkAutoDetection.Checked;
            if (!lstDevice.Exists(w => w.Name == devCurrent.Name))
            {
                InitSysParams.InitCondition(devCurrent, true);
                devCurrent.Save();
                lstDevice.Add(devCurrent);
            }
            foreach (var device in lstDevice)
            {
                if (WorkCurveHelper.DeviceCurrent != null 
                    && WorkCurveHelper.DeviceCurrent.Id != devCurrent.Id
                    && device.Id == WorkCurveHelper.DeviceCurrent.Id)
                {
                    device.IsDefaultDevice = false;//取消之前的默认设备
                    _ChangeDevice = true;
                }
                else if (WorkCurveHelper.DeviceCurrent != null
                    && WorkCurveHelper.DeviceCurrent.Id != devCurrent.Id
                    && device.Id == devCurrent.Id)
                {
                    device.IsDefaultDevice = true;
                }
                device.Save();
            }
            //if (WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.Id != devCurrent.Id)
            //{
            //    WorkCurveHelper.DeviceCurrent.IsDefaultDevice = false;//取消之前的默认设备
            //    WorkCurveHelper.DeviceCurrent.Save();
            //    _ChangeDevice = true;
            //}
            //devCurrent.IsDefaultDevice = true;//设置新的默认设备
            UpdateConditionForMAXVoltage(devCurrent);
            //devCurrent.Save();
            if (devCurrent.IsDP5)
            {
                SaveDP5Params(devCurrent.ComType==ComType.USB);
            }
            //WorkCurveHelper.DeviceCurrent = devCurrent;
            WorkCurveHelper.DeviceCurrent = Device.FindById(devCurrent.Id);
            devCurrent = WorkCurveHelper.DeviceCurrent;

            WorkCurveHelper.TabWidth = WorkCurveHelper.DeviceCurrent.MotorXMaxStep ;
            WorkCurveHelper.TabHeight = WorkCurveHelper.DeviceCurrent.MotorYMaxStep;

            if (WorkCurveHelper.DeviceCurrent.HasMotorSpin && WorkCurveHelper.DeviceCurrent.MotorSpinDirect == 1)
            {
                WorkCurveHelper.Y1Coeff = WorkCurveHelper.YCoeff;
            }
            else if (WorkCurveHelper.DeviceCurrent.HasMotorSpin && WorkCurveHelper.DeviceCurrent.MotorSpinDirect == 0)
            {
                WorkCurveHelper.Y1Coeff = float.Parse(ReportTemplateHelper.LoadSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/Y1Coeff"));
            }



            if (WorkCurveHelper.TabResetHeight > WorkCurveHelper.DeviceCurrent.MotorZMaxStep)
            {
                Msg.Show("请注意当前设置的Z轴最大行程已小于平台复位高度，平台复位高度需重新设置");
            }

            if (WorkCurveHelper.InOutDis > WorkCurveHelper.DeviceCurrent.MotorYMaxStep)
            {
                Msg.Show("请注意当前设置的Y轴最大行程已小于进出样距离，进出样距离需重新设置");
            }
            if (WorkCurveHelper.DeviceCurrent.HasMotorSpin && WorkCurveHelper.TestDis > WorkCurveHelper.DeviceCurrent.MotorSPinMaxStep)
            {
                Msg.Show("请注意当前设置的Y1轴最大行程已小于近景摄像头与测试点距离，近景摄像头与测试点距离需重新设置");
            }



            new System.Threading.Thread(new System.Threading.ThreadStart(((UCCameraControl)WorkCurveHelper.ucCamera).updateLargeViewNow)).Start();

            return;
        }

        private void SetCurrentWorkCurve()
        {
            string sql = "";
            if (DifferenceDevice.interClassMain.currentSelectMode==0)
                sql = "select * from WorkCurve as a join Condition as b on a.Condition_Id = b.Id join Device as d on d.Id=b.Device_Id where d.Id =" + devCurrent.Id + " and a.IsDefaultWorkCurve = 1 "+((DifferenceDevice.IsRohs && WorkCurveHelper.CurrentWorkRegion!=null)?" and a.workregion_id="+WorkCurveHelper.CurrentWorkRegion.Id:"");
            else
                sql = "select * from WorkCurve as a join Condition as b on a.Condition_Id = b.Id join Device as d on d.Id=b.Device_Id where d.Id =" + devCurrent.Id + " and b.Type = 2";
            WorkCurve current = WorkCurve.FindBySql(sql).Count == 0 ? null : WorkCurve.FindBySql(sql)[0];
            if (current != null)
            {
                WorkCurveHelper.WorkCurveCurrent = current;
                
                DifferenceDevice.interClassMain.OpenWorkCurveLog(WorkCurveHelper.WorkCurveCurrent, 1);
            }
        }

        private void DisEnableControl()
        {
            cmbSpecLength.Enabled = false;
            radUSB.Enabled = false;
            radFPGA.Enabled = false;
            radBlueTooth.Enabled = false;
            radParallel.Enabled = false;
        }

        private void EnableControl()
        {
            cmbSpecLength.Enabled = true;
            radUSB.Enabled = true;
            radFPGA.Enabled = true;
            radBlueTooth.Enabled = true;
            radParallel.Enabled = true;
        }

        #endregion

        #region Event
        /// <summary>
        /// 选择设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbwDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbwDevice.SelectedIndex > -1 && lbwDevice.SelectedIndex < lstDevice.Count)
            {
                devCurrent = lstDevice[lbwDevice.SelectedIndex];
                meDetector.DataSource = devCurrent.Detector;
                meTubes.DataSource = devCurrent.Tubes;
                meTubes.AllControls[4].Visible = false;     
                meTubes.AllControls[5].Visible = false;
                 
                //meFPGA.DataSource = devCurrent.FPGAParams;
                BindCurrentDeviceInfo();//绑定当前设备信息
                DisEnableControl();
            }
            else
            {
                meDetector.DataSource = devCurrent.Detector;
                meTubes.DataSource = devCurrent.Tubes;       
                meTubes.AllControls[4].Visible = false;           
                meTubes.AllControls[5].Visible = false;
                 
                BindCurrentDeviceInfo();//绑定当前设备信息
            }
        }

        /// <summary>
        /// 保存设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
           
            if (this.ParentForm != null)
                this.ParentForm.DialogResult = this.dialogResult = DialogResult.OK;
            EDXRFHelper.DisplayWorkCurveControls();
            EDXRFHelper.GotoMainPage(this);
            
        }

        private bool _ChangeDevice;

        private void UpdateConditionForMAXVoltage(Device device)
        {
            DbObjectList<Condition> lstCondition = Condition.Find(c => c.Device.Id == device.Id);
            foreach (var condition in lstCondition)
            {
                if (condition.InitParam == null)
                {
                    condition.InitParam = InitParameter.New.Init(40, 100, 60, 120, 60, 120, 1105, 0, "Ag", 1, 1, 1, TargetMode.OneTarget, 1,"x",1);
                    condition.InitParam.Save();
                }
                InitParameter init = condition.InitParam;
                //if (init == null) init = InitParameter.New.Init(40, 100, 60, 120, 60, 120, 1105, 0, "Ag", 1, 1, 1, TargetMode.OneTarget, 1);
                init.TubVoltage = init.TubVoltage > device.MaxVoltage ? device.MaxVoltage : init.TubVoltage;
                init.TubCurrent = init.TubCurrent > device.MaxCurrent ? device.MaxCurrent : init.TubCurrent;
                //init.Save();
                HasMany<DeviceParameter> paras = condition.DeviceParamList;
                foreach (var para in paras)
                {
                    para.TubVoltage = para.TubVoltage > device.MaxVoltage ? device.MaxVoltage : para.TubVoltage;
                    para.TubCurrent = para.TubCurrent > device.MaxCurrent ? device.MaxCurrent : para.TubCurrent;
                    //para.Save();
                }
                condition.Save();
            }
        }
        /// <summary>
        /// 添加新设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateHelper.IllegalCheck(txtNewDevice))
            {
                Device dev = lstDevice.Find(d => d.Name == txtNewDevice.Text);
                if (dev == null)
                {
                    string deviceId = lstDevice.Count.ToString() + DateTime.Now.Millisecond;
                    var devicea = Device.New.Init(txtNewDevice.Text, WorkCurveHelper.ArmInfo.DeviceId, DllType.DLL3, 1, 1, deviceId, SpecLength.Normal, 1, 0, 2, 120, 120, 120, 150, 150, 150, 50, 800, 3, 120, 0, 0, "x");
                    devicea.MotorXCode = 2;
                    devicea.MotorYCode = 3;
                    devicea.MotorZCode = 4;
                    devicea.MotorSpinCode = 3;
                    devicea.MotorSpinSpeed = 5;
                    devicea.Detector = Detector.New.Init(Skyray.EDXRFLibrary.DetectorType.Si, 5.895, 170);
                    devicea.Detector.FixGaussDelta = (18 * 1.0) / (1105 * 1.0);
                    devicea.Tubes = Tubes.New.Init(74, 19, 1.9, "SiO2", 40, 35, 14);
                    devicea.FPGAParams = Default.GetFPGAParams();
                    //devicea.Save();
                    //InitSysParams.InitCondition(devicea,true);
                    devCurrent = devicea;
                    //lstDevice.Add(devicea);
                    lbwDevice.Items.Add(txtNewDevice.Text);
                    lbwDevice.SelectedItem = txtNewDevice.Text;
                    txtNewDevice.Text = "";
                    EnableControl();
                    if (cboVoltage.Items.Count>0) cboVoltage.SelectedIndex = 0;

                    lbwDevice.Enabled = false;
                }
                else
                {
                    Msg.Show(Info.ExistName);
                }
            }
        }

        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            int index = lbwDevice.SelectedIndex;
            if (index >= 0 )
            {
                if (WorkCurveHelper.DeviceCurrent != null && lstDevice[index].Id == WorkCurveHelper.DeviceCurrent.Id)
                {
                    SkyrayMsgBox.Show(Info.CanotDelCurrentDevice);
                }
                else
                {
                    DialogResult dr = Msg.Show(Info.DeleteDevice, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (dr == DialogResult.OK)
                    {
                        Device device = lstDevice[index];
                        //Condition.DeleteAll(c => c.Device.Id == device.Id);
                        List<Condition> lscons = Condition.Find(c => c.Device.Id == device.Id);
                        foreach (var c in lscons)
                        {
                            List<WorkCurve> lswc = WorkCurve.Find(w => w.Condition.Id == c.Id);//曲线删除
                            foreach (var wc in lswc)
                            {
                                List<ElementList> lselemst=ElementList.Find(w=>w.WorkCurve.Id==wc.Id);//删除元素列表
                                foreach(var elst in lselemst)
                                {
                                    List<CurveElement> lswurElems = CurveElement.Find(w => w.ElementList.Id == elst.Id);
                                    foreach (var cure in lswurElems)
                                    {
                                        StandSample.DeleteAll(w => w.Element.Id == cure.Id);//删除标样
                                        ElementRef.DeleteAll(w => w.Element.Id == cure.Id);
                                        Optimiztion.DeleteAll(w => w.element.Id == cure.Id);
                                        ReferenceElement.DeleteAll(w => w.Element.Id == cure.Id);
                                        cure.Delete();
                                    }
                                    CustomField.DeleteAll(w => w.ElementList.Id == elst.Id);//自定义
                                    elst.Delete();
                                }
                                IntensityCalibration.DeleteAll(w => w.WorkCurve.Id == wc.Id);
                                wc.Delete();
                            }
                            DemarcateEnergy.DeleteAll(w => w.Condition.Id == c.Id);//能量刻度
                            DeviceParameter.DeleteAll(w => w.Condition.Id == c.Id);//小条件
                            InitParameter.DeleteAll(w => w.Condition.Id == c.Id);
                            c.Delete();
                        }
                        SpectrumData.DeleteAll(w => w.DeviceName == device.Name);//删除谱数据
                        Target.DeleteAll(w => w.Device.Id == device.Id);
                        //删除历史纪录
                        List<HistoryRecord> lshistory = HistoryRecord.Find(w => w.DeviceName == device.Name);
                        foreach (var hi in lshistory)
                        {
                            HistoryElement.DeleteAll(w => w.HistoryRecord.Id == hi.Id);
                            hi.Delete();
                        }
                        device.Delete();
                        //lstDevice[index].Delete();
                        lstDevice.RemoveAt(index);
                        //DelCascade(device);
                        lbwDevice.Items.RemoveAt(index);//删除缓存
                        if(lbwDevice.Items != null && lbwDevice.Items.Count > 0)
                        {
                            if (index == 0)
                            { 
                                lbwDevice.SelectedIndex = 0;
                                lbwDevice_SelectedIndexChanged(null, null);
                            }
                            else
                            lbwDevice.SelectedIndex = index - 1;
                        }
                        lbwDevice.Enabled = true;
                    }
                }
            }
            else
            {
                Msg.Show(Info.NoSelect);
            }
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);//返回主界面
        }

        /// <summary>
        /// 数据验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvw_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if ((DataGridViewW)sender == dgvwFilter && dgvwFilter.Columns[e.ColumnIndex].Name.Equals("ColCaption"))
            {
                string caption = e.FormattedValue.ToString();
                bool result = ValidateHelper.StringContainCn(caption);
                if (!result)//不含中文
                {
                    var atom = Atoms.AtomList.Find(a => a.AtomName == caption);
                    if (atom != null)
                    {
                        devCurrent.Filter[e.RowIndex].AtomNum = atom.AtomID;
                    }
                    else
                    {
                        devCurrent.Filter[e.RowIndex].AtomNum = 0;
                    }
                }
                else
                {
                    e.Cancel = true;
                    Msg.Show(Info.strContainsCn);
                }
                return;
            }
            else if ((DataGridViewW)sender == dgvwTarget && dgvwTarget.Columns[e.ColumnIndex].Name.Equals("ColTargetCaption"))
            {
                string caption = e.FormattedValue.ToString();
                bool result = ValidateHelper.StringContainCn(caption);
                if (!result)//不含中文
                {
                    var atom = Atoms.AtomList.Find(a => a.AtomName == caption);
                    if (atom != null)
                    {
                        devCurrent.Target[e.RowIndex].AtomNum = atom.AtomID;
                    }
                    else
                    {
                        devCurrent.Target[e.RowIndex].AtomNum = 0;
                    }
                }
                else
                {
                    e.Cancel = true;
                    Msg.Show(Info.strContainsCn);
                }
                return;
            }
            if (String.IsNullOrEmpty(e.FormattedValue.ToString()))
            {
                //Msg.Show(Info.IsNull);
                ((DataGridViewW)sender)[e.ColumnIndex, e.RowIndex].Value = (e.RowIndex + 1) * 1000;
                e.Cancel = true;
                return;
            }
            try
            {
               double.Parse(e.FormattedValue.ToString());
            }
            catch (FormatException)
            {
                //Msg.Show(Info.DataError);
                ((DataGridViewW)sender)[e.ColumnIndex, e.RowIndex].Value = (e.RowIndex + 1) * 1000;
                e.Cancel = true;
                return;
            }

            if (e.RowIndex == 1 && ((DataGridViewW)sender).Columns[e.ColumnIndex].DataPropertyName == "Step")
            {
                double DeltaStep = 1000;
                double FirstStep = double.Parse(((DataGridViewW)sender)[e.ColumnIndex, 0].Value.ToString());
                double SecondStep = e.Cancel ? double.Parse(((DataGridViewW)sender)[e.ColumnIndex, 1].Value.ToString()) : double.Parse(e.FormattedValue.ToString());
                DeltaStep = SecondStep - FirstStep;
                for (int i = 2; i < ((DataGridViewW)sender).RowCount; i++)
                    ((DataGridViewW)sender)[e.ColumnIndex, i].Value = FirstStep + i * DeltaStep;
            }
            if (e.RowIndex == 1 && (((DataGridViewW)sender).Columns[e.ColumnIndex].DataPropertyName == "StepCoef1" || ((DataGridViewW)sender).Columns[e.ColumnIndex].DataPropertyName == "StepCoef2"))
            {
                double SecondStep = e.Cancel ? double.Parse(((DataGridViewW)sender)[e.ColumnIndex, 1].Value.ToString()) : double.Parse(e.FormattedValue.ToString());
                for (int i = 2; i < ((DataGridViewW)sender).RowCount; i++)
                    ((DataGridViewW)sender)[e.ColumnIndex, i].Value = SecondStep;
            }
            //if (((DataGridViewW)sender).Name.Equals("dgvwCollimator") && e.RowIndex == 0 && e.ColumnIndex == 2)
            //{
            //    for (int i = 1; i < dgvwCollimator.Rows.Count; i++)
            //    {

            //        dgvwCollimator[2, i].Value = (double.Parse(e.FormattedValue.ToString()) - i * 1000 > 0) ? double.Parse(e.FormattedValue.ToString()) - i * 1000 : 0;
            //    }
            //}
            //if (((DataGridViewW)sender).Name.Equals("dgvwFilter") && e.RowIndex == 0 && e.ColumnIndex == 1)
            //{
            //    for (int i = 1; i < dgvwFilter.Rows.Count; i++)
            //    {
            //        dgvwFilter[1, i].Value = double.Parse(e.FormattedValue.ToString()) + i * 1100>0?double.Parse(e.FormattedValue.ToString()) + i * 1100:0;
            //    }
            //}
            //if (((DataGridViewW)sender).Name.Equals("dgvwChamber") && e.RowIndex == 0 && e.ColumnIndex == 1)
            //{
            //    for (int i = 1; i < dgvwChamber.Rows.Count; i++)
            //    {
            //        dgvwChamber[1, i].Value = double.Parse(e.FormattedValue.ToString()) + i * 960>0?double.Parse(e.FormattedValue.ToString()) + i * 960:0;
            //    }
            //}
        }
        /// <summary>
        /// 数据出错
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvw_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Msg.Show(Info.DataError);
        }
        /// <summary>
        /// 准直器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numCollimatorMaxNum_ValueChanged(object sender, EventArgs e)
        {
            double[] Diameter = { 8, 6, 4, 3, 2, 1, 0.5, 0.2 };
            int num = (int)numCollimatorMaxNum.Value;
            if (devCurrent.Collimators.Count < num)//增加一行
            {
                devCurrent.Collimators.Add(Collimator.New.Init(num, Diameter[num-1], ((int)numCollimatorMaxNum.Maximum - num) * 1000));
            }
            else if (devCurrent.Collimators.Count > num)//减少一行
            {
                devCurrent.Collimators.RemoveAt(num);
            }
            BindingCollimators();
        }
        /// <summary>
        /// 滤光片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numFilterMaxNum_ValueChanged(object sender, EventArgs e)
        {
            int num = (int)numFilterMaxNum.Value;
            if (devCurrent.Filter.Count < num)//增加一行
            {
                devCurrent.Filter.Add(Filter.New.Init(num, num * 1000, "Cu", 0));
            }
            else if (devCurrent.Filter.Count > num)//减少一行
            {
                devCurrent.Filter.RemoveAt(num);
            }
            BindingFilter();
        }
        /// <summary>
        /// 样品腔
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numChamberMaxNum_ValueChanged(object sender, EventArgs e)
        {
            int num = (int)numChamberMaxNum.Value;
            if (devCurrent.Chamber.Count < num)//增加一行
            {
                devCurrent.Chamber.Add(Chamber.New.Init(num, num * 1000,0,0));
            }
            else if (devCurrent.Chamber.Count > num)//减少一行
            {
                devCurrent.Chamber.RemoveAt(num);
            }
            BindingChamber();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numTargetMaxNum_ValueChanged(object sender, EventArgs e)
        {
            int num = (int)numTargetMaxNum.Value;
            if (devCurrent.Target.Count < num)
            {
                devCurrent.Target.Add(Target.New.Init(num, num * 1000, "Cu", 0));
            }
            else if (devCurrent.Target.Count > num)
            {
                devCurrent.Target.RemoveAt(num);
            }
            BindingTarget();
        }

        #endregion

        private void cboPeakingTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strValue = "";
            cboFlatTop.Items.Clear();
            if (cboPeakingTime.Text != "")
            {
                bool isFind = false;
                double dblValue = Convert.ToDouble(cboPeakingTime.Text.ToString());
                if (dblValue <= 6.4)
                {
                    for (int i = 1; i <= 16; i++)
                    {
                        strValue = Convert.ToDouble(0.2 * i).ToString("0.0");
                        cboFlatTop.Items.Add(Convert.ToDouble(strValue));
                        if (Convert.ToDouble(strValue) == devCurrent.FPGAParams.FlatTop)
                        {
                            isFind = true;
                            cboFlatTop.SelectedIndex = i - 1;
                        }
                    }
                }
                else if (dblValue <= 12.8)
                {
                    for (int i = 1; i <= 16; i++)
                    {
                        strValue = Convert.ToDouble(0.4 * i).ToString("0.0");
                        cboFlatTop.Items.Add(Convert.ToDouble(strValue));
                        if (Convert.ToDouble(strValue) == devCurrent.FPGAParams.FlatTop)
                        {
                            isFind = true;
                            cboFlatTop.SelectedIndex = i - 1;
                        }
                    }
                }
                else if (dblValue <= 25.6)
                {
                    for (int i = 1; i <= 16; i++)
                    {
                        strValue = Convert.ToDouble(0.8 * i).ToString("0.0");
                        cboFlatTop.Items.Add(Convert.ToDouble(strValue));
                        if (Convert.ToDouble(strValue) == devCurrent.FPGAParams.FlatTop)
                        {
                            isFind = true;
                            cboFlatTop.SelectedIndex = i - 1;
                        }
                    }
                }
                else if (dblValue <= 51.2)
                {
                    for (int i = 1; i <= 16; i++)
                    {
                        strValue = Convert.ToDouble(1.6 * i).ToString("0.0");
                        cboFlatTop.Items.Add(Convert.ToDouble(strValue));
                        if (Convert.ToDouble(strValue) == devCurrent.FPGAParams.FlatTop)
                        {
                            isFind = true;
                            cboFlatTop.SelectedIndex = i - 1;
                        }
                    }
                }
                else
                {
                    for (int i = 1; i <= 16; i++)
                    {
                        strValue = Convert.ToDouble(3.2 * i).ToString("0.0");
                        cboFlatTop.Items.Add(Convert.ToDouble(strValue));
                        if (Convert.ToDouble(strValue) == devCurrent.FPGAParams.FlatTop)
                        {
                            isFind = true;
                            cboFlatTop.SelectedIndex = i - 1;
                        }
                    }
                }
                if (!isFind)
                {
                    cboFlatTop.SelectedIndex = 0;
                }
                cboFlatTop.SelectedIndex = 0;
            }
        }

        #region InitDP5Params
        //private string[] arrCboVoltage = new string[] { "134","157", "245", "354" };
        private void InitDP5Params()
        {
            switch (devCurrent.Dp5Version)
            {
                case Dp5Version.Dp5_FastNet:
                    dp5Device = new Dp5FastNetDevice();
                    break;
                case Dp5Version.Dp5_CommonUsb:
                default:
                    dp5Device = new Dp5Device();
                    break;
            }
            cboDP5HeapUP.Items.Clear();
            cboDP5HeapUP.Items.AddRange(new string[] { "OFF", "ON" });
            cboVoltage.Items.Clear();
            cboVoltage.Items.AddRange(dp5Device.GetDp5HV().ToArray());
            cboVoltage.SelectedIndex = 0;
            string[] Peakings = dp5Device.GetPeakingTimesStr().ToArray();
            this.cboDP5PeakingTime.Items.Clear();
            this.cboDP5PeakingTime.Items.AddRange(Peakings);
            int purOn = 0;
            int fastThreshold = 0;
            string peakingTime = string.Empty;
            string flatTop = string.Empty;
            int hv = 0;
            dp5Device.LoadDP5CfgOther(ref purOn, ref fastThreshold, ref peakingTime, ref flatTop, ref hv);
            cboDP5PeakingTime.SelectedItem = peakingTime;
            this.cboDP5FlatTop.Items.Clear();
            List<string> flatTops=dp5Device.GetFlatTopsStr(Convert.ToSingle(peakingTime));
            this.cboDP5FlatTop.Items.AddRange(flatTops.ToArray());
            this.cboDP5FlatTop.SelectedItem = flatTop;
            numDP5FastThreshold.Value = fastThreshold;
            if (cboVoltage.Items.Contains(hv.ToString()))
                cboVoltage.SelectedItem = hv.ToString();
            else cboVoltage.SelectedIndex = 0;
            cboDP5HeapUP.SelectedItem = purOn == 0 ? cboDP5HeapUP.Items[0].ToString() : cboDP5HeapUP.Items[1].ToString();
        }

        private void cboDP5PeakingTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cboDP5FlatTop.Items.Clear();
            string peakingTime = cboDP5PeakingTime.SelectedItem.ToString();
            List<string> flattops = dp5Device.GetFlatTopsStr(Convert.ToSingle(peakingTime));
            this.cboDP5FlatTop.Items.AddRange(flattops.ToArray());
            this.cboDP5FlatTop.SelectedIndex = 0;
        }

        private void SaveDP5Params(bool IsUSB)
        {
            if (IsUSB)
            {
                int purOn = 0;
                int fastThreshold = 0;
                string tempPeakingTime = cboDP5PeakingTime.Text;
                string tempFlatTop = cboDP5FlatTop.Text;
                string tempPueOn = cboDP5HeapUP.Text;
                int tempfastThreshold = int.Parse(numDP5FastThreshold.Value.ToString());
                int intVlotage = Convert.ToInt32(cboVoltage.Text);
                if (tempPueOn.Equals("OFF"))
                {
                    purOn = 0;
                }
                else
                {
                    purOn = 1;
                }
                fastThreshold = tempfastThreshold;
                dp5Device.OpenDevice();
                dp5Device.LoadDP5Cfg();
                dp5Device.SaveDP5CfgOther(purOn, fastThreshold, tempPeakingTime, tempFlatTop, intVlotage);
                dp5Device.CloseDevice();
            }
            else
            {
                string tempPeakingTime = cboPeakingTimDpp100.SelectedItem.ToString();
                string tempGain = cboGain.SelectedItem.ToString();
                string tempFast = numFastLimit.Value.ToString();
                string tempSlow = numSlowLimit.Value.ToString();
                string tempPipeUp = cboHeapUP.SelectedItem.ToString();
                string tempFlatTop = cboFlatTopDpp100.SelectedItem.ToString();
                string tempPreAmp = cboPreAMPDpp100.SelectedItem.ToString();
                int tempBLRSpeed = cboBLRSpeedDpp100.SelectedIndex;
                int tempZeroOffset = (int)numZeroOffSetDpp100.Value;
                int tempQCProtect = (int)numQCProtect.Value;
                NewNetPortProtocol netpro = new NewNetPortProtocol(4096);
                bool bResult = netpro.SaveCfgsToFile(tempPeakingTime, tempGain, tempFast, tempSlow, tempPipeUp, tempFlatTop, tempPreAmp, tempBLRSpeed, tempZeroOffset, tempQCProtect);
               if (!bResult) Msg.Show("保存失败");
            }
        }

        #endregion

        private void radUSB_CheckedChanged(object sender, EventArgs e)
        {
            if (radUSB.Checked)
            {
                devCurrent.ComType = ComType.USB;
            }
        }

        private void radFPGA_CheckedChanged(object sender, EventArgs e)
        {
            if (radFPGA.Checked)
            {
                if (devCurrent.ComType != ComType.FPGA)
                {
                    devCurrent.ComType = ComType.FPGA;
                    devCurrent.CollimatorSpeed = 5;
                    devCurrent.FilterSpeed = 5;
                }
            }
            //else
            //{
            //    devCurrent.CollimatorSpeed = 120;
            //    devCurrent.FilterSpeed = 120;
            //}
        }

        private void radBlueTooth_CheckedChanged(object sender, EventArgs e)
        {
            if (radBlueTooth.Checked)
            {
                devCurrent.ComType = ComType.BlueTooth;
            }
        }

        private void radParallel_CheckedChanged(object sender, EventArgs e)
        {
            if (radParallel.Checked)
            {
                devCurrent.ComType = ComType.Parallel;
            }
        }

        #region
        public override void ExcuteEndProcess(params object[] objs)
        {
            if (_ChangeDevice)
            {
                WorkCurveHelper.WorkCurveCurrent = null;
                SetCurrentWorkCurve();
                EDXRFHelper.DisplayWorkCurveControls();
                DifferenceDevice.MediumAccess.DevicceChange();
            }
            NaviItem item = WorkCurveHelper.NaviItems.Find(w => w.Name == "MoveWorkStation");
            DifferenceDevice.MediumAccess.SaveDevice(item, null);
        }
        #endregion

       
        private void btnApplication_Click(object sender, EventArgs e)
        {

            lbwDevice.Enabled = true;
            SaveDevice();
            DisEnableControl();
            SetCurrentWorkCurve();
            lbwDevice_SelectedIndexChanged(null, null);

        }


        private void radVacuumPump_CheckedChanged(object sender, EventArgs e)
        {
            //if (radVacuumPump.Checked)
            //{
            //    radIsAllowOpenCover2.Checked = true;
            //    radVacuumPump.Checked = true;
            //}
        }

        private void radIsAllowOpenCover_CheckedChanged(object sender, EventArgs e)
        {
            if (radIsAllowOpenCover.Checked)
            {
                radVacuumPump2.Checked = true;
                radIsAllowOpenCover.Checked = true;
            }
        }

        public string newDeviceName = string.Empty;
        private void lbwDevice_DoubleClick(object sender, EventArgs e)
        {
            if (lbwDevice.SelectedIndex >= 0 && lbwDevice.SelectedIndex < lstDevice.Count)
            {
                newDeviceName = lbwDevice.SelectedItem.ToString();
                devCurrent = lstDevice[lbwDevice.SelectedIndex];
                newDeviceName = devCurrent.Name;
                string oldName = devCurrent.Name;
               // FrmDeviceNewName frm = new FrmDeviceNewName(newDeviceName);
                FrmDeviceNewName frm = new FrmDeviceNewName(Info.NewDeviceName, Info.NewDeviceName, Info.NewDeviceName, newDeviceName);
                frm.ShowDialog();
                if (frm.DialogResult == DialogResult.OK)
                {
                    newDeviceName = frm.newDeviceName.Trim();//获取新设备名称
                    if (lstDevice.Find(w => w.Name == newDeviceName) == null)
                    {
                        devCurrent.Name = newDeviceName;
                        devCurrent.Save();
                        string sql = "Update SpectrumData set DeviceName = '" + newDeviceName + "' where DeviceName = '" + oldName +"';";
                        sql += "Update PureSpecParam set DeviceName = '" + newDeviceName + "' where DeviceName = '" + oldName + "';";
                        Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
                    }
                    else
                    {
                        Msg.Show(Info.ExistName);
                        return;
                    }
                }
                frm.Close();
                lbwDevice.Items.Clear();//清空列表
                foreach (var device in lstDevice)
                {
                    lbwDevice.Items.Add(device.Name);
                }
                var selectDevice = lstDevice.Find(w => w.Name == devCurrent.Name);
                lbwDevice.SelectedIndex = lstDevice.IndexOf(selectDevice);
            }
        }

        private void tsmRename_Click(object sender, EventArgs e)
        {
            lbwDevice_DoubleClick(null, null);
        }

        private void radIsDP5_CheckedChanged(object sender, EventArgs e)
        {
            if (devCurrent != null&&devCurrent.IsDP5)
               InitDP5Params();
        }


        private void lbwDevice_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                for (int i = 0; i < lbwDevice.Items.Count;i++ )
                {
                    if(lbwDevice.GetItemRectangle(i).Contains(e.Location))
                    {
                        lbwDevice.SelectedIndex = i;
                        tsmRename.ShowDropDown();
                    }
                }
            }
        }

        private void cboDp5Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDp5Type.SelectedItem == Dp5Version.Dp5_FastNet.ToString())
            {
                lblDp5IP.Enabled = true;
                txtDp5Ip.Enabled = true;
            }
            else
            {
                lblDp5IP.Enabled = false;
                txtDp5Ip.Enabled = false;
            }
            if (devCurrent!=null&&devCurrent.IsDP5) InitDP5Params();
        }

        private void radioDMCA_CheckedChanged(object sender, EventArgs e)
        {
            if (radioDpp100.Checked)
            {
                NewNetPortProtocol netpro = new NewNetPortProtocol(4096);
                string strPeakingTime=string.Empty;
                string strGain = string.Empty;
                string strFastLimit= string.Empty;
                string strSlowLimit = string.Empty;
                string strPileUp = string.Empty;
                string strFlatTop = string.Empty;
                string strPreAmpReset = string.Empty;
                int iFirBLRSpeed = 0;
                int iZeroOffsetValue = 0;
                int iQCProtect = 0;
                netpro.ReadCfgsFromFile(ref strPeakingTime, ref strGain, ref strFastLimit, ref strSlowLimit, ref strPileUp,
                    ref strFlatTop, ref strPreAmpReset, ref iFirBLRSpeed, ref iZeroOffsetValue, ref iQCProtect);
                cboPeakingTimDpp100.SelectedItem = strPeakingTime;
                cboGain.SelectedItem = strGain;
                numFastLimit.Value = int.Parse(strFastLimit);
                numSlowLimit.Value = int.Parse(strSlowLimit);
                cboHeapUP.SelectedItem = strPileUp;
                cboFlatTopDpp100.SelectedItem = strFlatTop;
                cboPreAMPDpp100.SelectedItem = strPreAmpReset;
                cboBLRSpeedDpp100.SelectedIndex = iFirBLRSpeed;
                numZeroOffSetDpp100.Value = iZeroOffsetValue;
                numQCProtect.Value = iQCProtect;


            }
        }

        private void btnSampleCal_Click(object sender, EventArgs e)
        {
            UCSampleCalSet fb = new UCSampleCalSet(WorkCurveHelper.SampleCalPath);
            WorkCurveHelper.OpenUC(fb, false,btnSampleCal.Text,true);
        }


    }
}
