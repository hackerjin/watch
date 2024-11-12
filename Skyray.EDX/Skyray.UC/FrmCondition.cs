using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;
using Skyray.Controls;
using Lephone.Data.Common;
using Lephone.Util;
using Skyray.EDX.Common;
using System.Linq;
using System.Data;
using System.Drawing;

namespace Skyray.UC
{
    /// <summary>
    /// 测量条件
    /// </summary>
    public partial class FrmCondition : Skyray.Language.UCMultiple
    {

        #region Init


        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmCondition()
        {
            InitializeComponent();
            if (Ranges.RangeDictionary != null)
            {
                

                Ranges.RangeDictionary.Remove("TubVoltage");
                Ranges.RangeDictionary.Add("TubVoltage", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = WorkCurveHelper.DeviceCurrent.MaxVoltage, Min = 0 });
                //if (WorkCurveHelper.DeviceCurrent.HasTarget)
                //{
                //    Ranges.RangeDictionary.Remove("TubVoltage");
                //    Ranges.RangeDictionary.Add("TubVoltage", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = WorkCurveHelper.DeviceCurrent.MaxVoltage, Min = 10 });
                //}
                Ranges.RangeDictionary.Remove("TubCurrent");
                Ranges.RangeDictionary.Add("TubCurrent", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = WorkCurveHelper.DeviceCurrent.MaxCurrent, Min = 0 });
                Ranges.RangeDictionary.Remove("FilterIdx");
                Ranges.RangeDictionary.Add("FilterIdx", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = WorkCurveHelper.DeviceCurrent.Filter.Count, Min = 1 });
                Ranges.RangeDictionary.Remove("Filter");
                Ranges.RangeDictionary.Add("Filter", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = WorkCurveHelper.DeviceCurrent.Filter.Count, Min = 1 });
                Ranges.RangeDictionary.Remove("CollimatorIdx");
                Ranges.RangeDictionary.Add("CollimatorIdx", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = WorkCurveHelper.DeviceCurrent.Collimators.Count, Min = 1 });
                Ranges.RangeDictionary.Remove("Collimator");
                Ranges.RangeDictionary.Add("Collimator", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = WorkCurveHelper.DeviceCurrent.Collimators.Count, Min = 1 });
               
            }
            meInit.LayoutType = typeof(InitParameter);
            meInit.LayoutSource = LayoutSource.New.Init(false, true, true, string.Empty, string.Empty, 3, LabelPosition.Left);
            meInit.GroupTitle = Info.strInitCondition;
            this.ColTargetMode.Items.Clear();
            this.ColTargetMode.Items.AddRange(new object[] { TargetMode.OneTarget, TargetMode.TwoTarget }); 
            if (DifferenceDevice.IsRohs)
            {
                ColIsDistrubAlert.Visible = true;
            }
           
            ((NumricUpDownW)meInit.Controls.Find("AutoCtrlMinRate", true)[0]).ValueChanged += new EventHandler(MinAndMaxValueComparison);

            if (WorkCurveHelper.DeviceCurrent.HasTarget)
            {
                ((ComboBoxW)meInit.Controls.Find("AutoCtrlTargetMode", true)[0]).SelectedIndexChanged += new EventHandler(MinVolValueForTarget);

                ((NumricUpDownW)meInit.Controls.Find("AutoCtrlTubVoltage", true)[0]).ValueChanged += new EventHandler(MinVolValueForVol);
            }


            string showPeakCalibrate = ReportTemplateHelper.LoadSpecifiedValue("ConditonShowPeakCalibrate", "IsShowCalibrate");
            if (showPeakCalibrate == "0")
            {
                ColIsPeakFloat.Visible = PeakCheckTime.Visible = ColPeakFloatLeft.Visible = ColPeakFloatRight.Visible = ColPeakFloatChannel.Visible = ColPeakFloatError.Visible = false;
            }
        }

        void MinAndMaxValueComparison(object sender, EventArgs e)
        {
            if (((NumricUpDownW)meInit.Controls.Find("AutoCtrlMinRate", true)[0]).Value > ((NumricUpDownW)meInit.Controls.Find("AutoCtrlMaxRate", true)[0]).Value)
                ((NumricUpDownW)meInit.Controls.Find("AutoCtrlMinRate", true)[0]).Value = ((NumricUpDownW)meInit.Controls.Find("AutoCtrlMaxRate", true)[0]).Value;
        }

        void MinVolValueForTarget(object sender, EventArgs e)
        {
            if (WorkCurveHelper.DeviceCurrent.HasTarget &&((ComboBoxW)meInit.Controls.Find("AutoCtrlTargetMode", true)[0]).SelectedItem.ToString() == TargetMode.TwoTarget.ToString())
            {
                if (((NumricUpDownW)meInit.Controls.Find("AutoCtrlTubVoltage", true)[0]).Value < 10)
                    ((NumricUpDownW)meInit.Controls.Find("AutoCtrlTubVoltage", true)[0]).Value = 10;
            }
        }

        void MinVolValueForVol(object sender, EventArgs e)
        {
            if (WorkCurveHelper.DeviceCurrent.HasTarget
                && ((ComboBoxW)meInit.Controls.Find("AutoCtrlTargetMode", true)[0]).SelectedItem.ToString() == TargetMode.TwoTarget.ToString())
            {
                if (((NumricUpDownW)meInit.Controls.Find("AutoCtrlTubVoltage", true)[0]).Value < 10)
                    ((NumricUpDownW)meInit.Controls.Find("AutoCtrlTubVoltage", true)[0]).Value = 10;
            }
            
        }

        /// <summary>
        /// 重写父类方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void PageLoad(object sender, EventArgs e)
        {
            if (!WorkCurveHelper.DeviceCurrent.HasVacuumPump)
            {
                ColIsVacuum.Visible = VacuumTime.Visible = ColIsVacuumDegree.Visible = VacuumDegree.Visible = false;
            }
            if (!WorkCurveHelper.DeviceCurrent.HasFilter)
            {
                this.FilterIdx.Visible = false;
            }
            if (!WorkCurveHelper.DeviceCurrent.HasCollimator)
            {
                this.CollimatorIdx.Visible = false;
            }
            if (!WorkCurveHelper.DeviceCurrent.HasTarget)
            {
                this.ColTargetIdx.Visible = false;
                this.ColTargetMode.Visible = false;
                this.ColCurrentRate.Visible = false;
            }
            if (DifferenceDevice.IsAnalyser)
            {
                btnAddTestCondition.Visible = btnDelTestCondition.Visible = false;
            }
            if (WorkCurveHelper.DeviceCurrent.ComType == ComType.FPGA)
            {
                this.ColPeakFloatLeft.Visible = false;
                this.ColPeakFloatRight.Visible = false;
                this.ColPeakFloatChannel.Visible = false;
                this.PeakCheckTime.Visible = false;
                this.ColIsPeakFloat.Visible = false;
                this.ColPeakFloatError.Visible = false;
            }
            GetConditions();
            var ctrl = this.meInit.Controls.Find("AutoCtrlElemName", true);
            if (ctrl != null && ctrl.Length > 0)
            {
                InitElementCtrl = ctrl[0];
                ((TextBox)InitElementCtrl).ReadOnly = true;
                InitElementCtrl.Click += new EventHandler(InitElement_Click);
            }

            var ct = this.meInit.Controls.Find("AutoCtrlInitFistCount", true);
            if (ct != null && ct.Length > 0)
            {
                InitFirstCountCtrl = ct[0];
                ((NumricUpDownW)InitFirstCountCtrl).ReadOnly = true;
                ((NumricUpDownW)InitFirstCountCtrl).Controls.RemoveAt(0);
                InitFirstCountCtrl.Click += new EventHandler(InitFirstCountCtrl_Click);
            }

           

           
            base.PageLoad(sender, e);
        }

        private Control InitFirstCountCtrl;
        private Control InitElementCtrl;

        /// <summary>
        /// 初始化元素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void InitElement_Click(object sender, EventArgs e)
        {
            ElementTableAtom table = new ElementTableAtom();

            string[] strs = new string[] { InitElementCtrl.Text };
            table.SelectedItems = strs;

            WorkCurveHelper.OpenUC(table, false, Info.SelectElement,true);//打开元素周期表
            if (table.DialogResult == DialogResult.OK && table.SelectedItems != null && table.SelectedItems.Length > 0)
            {
                InitElementCtrl.Text = table.SelectedItems[0];
            }
        }

        /// <summary>
        /// 初始化计数率
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void InitFirstCountCtrl_Click(object sender, EventArgs e)
        {
            UcInitCalibrateParam ucInOutSample = new UcInitCalibrateParam();
            ucInOutSample.InitCount =WorkCurveHelper.WorkCurveCurrent.Condition.InitParam.InitFistCount;
            ucInOutSample.InitRadio = WorkCurveHelper.WorkCurveCurrent.Condition.InitParam.InitCalibrateRatio;
            WorkCurveHelper.OpenUC(ucInOutSample, false, "Init Param", true);
            if (ucInOutSample.dialogResult == DialogResult.OK )
            {
                ((NumricUpDownW)InitFirstCountCtrl).Value =Convert.ToDecimal(ucInOutSample.InitCount);
                //WorkCurveHelper.WorkCurveCurrent.Condition.InitParam.InitCalibrateRatio = Convert.ToDouble(ucInOutSample.InitRadio);
                //WorkCurveHelper.WorkCurveCurrent.Condition.InitParam.Save();
            }
            //ElementTableAtom table = new ElementTableAtom();

            //string[] strs = new string[] { InitElementCtrl.Text };
            //table.SelectedItems = strs;

            //WorkCurveHelper.OpenUC(table, false, Info.SelectElement, true);//打开元素周期表
            //if (table.DialogResult == DialogResult.OK && table.SelectedItems != null && table.SelectedItems.Length > 0)
            //{
            //    InitFirstCountCtrl.Text = table.SelectedItems[0];
            //}
        }

        /// <summary>
        /// 获取文本
        /// </summary>
        public override void SetText()
        {
            Skyray.Language.Lang.Model.SetTextProperty(this.meInit.LabelControls);
        }

        /// <summary>
        /// 保存文本
        /// </summary>
        public override void SaveText()
        {
            Skyray.Language.Lang.Model.SaveTextProperty(false, this.meInit.LabelControls);
        }

        /// <summary>
        /// 获取测量条件
        /// </summary>
        private void GetConditions()
        {
            if (WorkCurveHelper.DeviceCurrent != null)
            {
                //获取当前设备下的工作条件列表
                initCondition();
            }
            else
            {
                //查询所有条件
                lstCondition = Condition.Find(c => c.Type == ConditionType.Normal);
                var lst = Condition.Find(c => c.Type == ConditionType.Match);
                if (lst.Count > 0)
                {
                    lst[0].Name = Info.Match;
                    lstCondition.Add(lst[0]);
                }
                lst = Condition.Find(c => c.Type == ConditionType.Intelligent);
                if (lst.Count > 0)
                {
                    lst[0].Name = Info.IntelligentCondition;
                    lstCondition.Add(lst[0]);
                }
                lst = Condition.Find(c => c.Type == ConditionType.Detection);
                if (lst.Count > 0)
                {
                    lst[0].Name = Info.strDetection;
                    lstCondition.Add(lst[0]);
                }
                lst = Condition.Find(c => c.Type == ConditionType.Match2);
                if (lst.Count > 0)
                {
                    lst[0].Name = Info.Match+2;
                    lstCondition.Add(lst[0]);
                }
            }
            //UpdatedRowIndexs.Clear();//清空已经编辑条件索引列表
            this.cboConditionList.DataSource = lstCondition.ToList();//条件列表设置数据源
            this.cboConditionList.DisplayMember = "Name";
            this.cboConditionList.ValueMember = "Id";
            this.cboConditionList.ColumnNames = "Name";
            this.cboConditionList.ColumnWidths = this.cboConditionList.Width.ToString();
            if (WorkCurveHelper.WorkCurveCurrent != null)
            {
                this.cboConditionList.SelectedValue = WorkCurveHelper.WorkCurveCurrent.Condition.Id;
            }
        }

        #endregion

        #region Fields

        /// <summary>
        /// 数据库中查询所得的条件列表
        /// </summary>
        DbObjectList<Skyray.EDXRFLibrary.Condition> lstCondition;
        /// <summary>
        /// 记录已经更改的条件列表
        /// </summary>
        //List<int> UpdatedRowIndexs = new List<int>();
        /// <summary>
        /// 当前选择的条件
        /// </summary>
        Skyray.EDXRFLibrary.Condition conditionCurrent;

        /// <summary>
        /// 当前选择条件的初始化条件
        /// </summary>
        InitParameter initParamCurrent;

        /// <summary>
        /// 记录CELL的前一个值
        /// </summary>
        private string preCellValue = string.Empty;

        #endregion

        #region Methods

        /// <summary>
        /// 添加测试条件
        /// </summary>
        private void AddDefaultDeivceParam()
        {
            if (conditionCurrent.Type != ConditionType.Normal)
            {
                return;
            }
            if (conditionCurrent.DeviceParamList.Count >= DifferenceDevice.DefaultMaxConditionParamterCount)
            {
                //Msg.Show(Info.DeviceParamterOutRange);
                return;
            }
            if (DifferenceDevice.IsAnalyser && conditionCurrent.DeviceParamList.Count >= 1)
            { return; }
            DeviceParameter dp = Default.GetDeviceParameter(WorkCurveHelper.DeviceCurrent.SpecLength,conditionCurrent.DeviceParamList.Count + 1);
            //dp.Save();
            conditionCurrent.DeviceParamList.Add(dp);//加载默认值至列表
            DeviceParameterToGrid();//绑定至界面
            int count = conditionCurrent.DeviceParamList.Count;
            if (count > 1)
            {
                this.dgvwTestConList.Rows[0].Selected = false;
                this.dgvwTestConList.Rows[count - 1].Selected = true;
            }
        }

        /// <summary>
        /// 测量条件列表绑定至表格
        /// </summary>
        private void DeviceParameterToGrid()
        {
            dgvwTestConList.Rows.Clear();
            dgvwTestConList.DataSource = null;
            BindingSource bs = new BindingSource();
            for (int i = 0; i < conditionCurrent.DeviceParamList.Count; i++)
            {
                bs.Add(conditionCurrent.DeviceParamList[i]);
            }
            dgvwTestConList.AutoGenerateColumns = false;
            dgvwTestConList.DataSource = bs;
        }

        #endregion

        #region Event

        /// <summary>
        /// 选择测量条件改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboConditionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            preCellValue = string.Empty;
            int index = cboConditionList.SelectedIndex;
            //if (!UpdatedRowIndexs.Contains(index)) UpdatedRowIndexs.Add(index);
            if (index < 0) return;
            conditionCurrent = lstCondition[index];//当前条件
            initParamCurrent = lstCondition[index].InitParam;//当前初始化条件
            DeviceParameterToGrid();//测量条件列表绑定至表格
            if (initParamCurrent != null)
                CtrlFactory.BindValue(meInit.EditControls, initParamCurrent, true);
            //if (WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.ComType == ComType.FPGA)
            //    BindHelper.BindTextToCtrl(cboFPGAGain, initParamCurrent, "Gain", true);
        }


        //private void ChangeVoltforTarget()
        //{
        //    if (WorkCurveHelper.DeviceCurrent.HasTarget)// && (TargetMode)dgvwTestConList["ColTargetMode",0].Value == TargetMode.TwoTarget)
        //    {
        //         Ranges.RangeDictionary.Remove("TubVoltage");
        //        Ranges.RangeDictionary.Add("TubVoltage", new RangeInfo { DecimalPlaces = 0, Increment = 1, Max = WorkCurveHelper.DeviceCurrent.MaxVoltage, Min = 10 });
        //    }
        //}

        /// <summary>
        /// 保存更改过的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOKCondition_Click(object sender, EventArgs e)
        {
  

            EDXRFHelper.GotoMainPage(this);
        }

        /// <summary>
        /// 取消编辑，并关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelCondition_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }

        /// <summary>
        /// 删除一个测量条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelCondition_Click(object sender, EventArgs e)
        {
            int index = cboConditionList.SelectedIndex;//获取选择索引  
            if (index != -1)
            {
                if (lstCondition[index].Type != ConditionType.Normal)
                {
                    Msg.Show(Info.ConditionCanotDel);
                    return;
                }

                if (lstCondition[index].WorkCurves.Count > 0)
                {
                    Msg.Show(Info.BeUsed);
                    return;
                }

                string sql = "delete from DemarcateEnergy where Condition_Id =" + lstCondition[index].Id;
                Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
                lstCondition[index].Delete();//删除该条记录
                GetConditions();//重新获取条件列表 
                if (lstCondition == null || lstCondition.Count == 0)
                {
                    cboConditionList.Text = string.Empty;//列表为空
                }
            }
        }

        /// <summary>
        /// 添加一条测量条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCondition_Click(object sender, EventArgs e)
        {
            //this.txtConditionName.Text = "";
            this.txtConditionName.Visible = true;
            this.btnSaveNew.Visible = true;
            btnCancelSave.Visible = true;

            btnApplication.Visible //隐藏应用按钮
                =btnOK.Visible //隐藏保存按钮
                = btnCancel.Visible //隐藏退出按钮
                = btnDelCondition.Visible //隐藏删除按钮
                = btnAddCondition.Visible//隐藏添加按钮
                = btnApplication.Visible//隐藏应用按钮
                = false;
        }

        /// <summary>
        /// 添加测试条件事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddTestCondition_Click(object sender, EventArgs e)
        {
            CommonHelper.CatchAll(AddDefaultDeivceParam);
        }

        /// <summary>
        /// 删除测试条件事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelTestCondition_Click(object sender, EventArgs e)
        {
            if (dgvwTestConList.SelectedRows.Count > 0)
            {
                if (conditionCurrent.DeviceParamList.Count > 1)
                {
                    int index = dgvwTestConList.SelectedRows[0].Index;
                    DeviceParameter param = conditionCurrent.DeviceParamList[index];
                    var element = CurveElement.Find(ce => ce.DevParamId == param.Id);
                    if (element != null && element.Count > 0)
                    {
                        Msg.Show(Info.BeUsed);
                        return;
                    }
                    conditionCurrent.DeviceParamList.RemoveAt(index);//删除缓存
                    //dgvwTestConList.Rows.RemoveAt(index);
                }
                else
                {
                    //Msg.Show(Info.AtLeastOneCondition);
                    return;
                }
            }
            else
            {
                //Msg.Show(Info.NoSelect);
                return;
            }
            DeviceParameterToGrid();//刷新数据列表
            int count = conditionCurrent.DeviceParamList.Count;
            if (count > 1)
            {
                this.dgvwTestConList.Rows[0].Selected = false;
                this.dgvwTestConList.Rows[count - 1].Selected = true;
            }
        }

        private bool hasId(string tableName)
        {
            int id = 0;
            string sql = "select Max(Id) from " + tableName;
            object obj = Lephone.Data.DbEntry.Context.ExecuteScalar(sql);
            if (obj == null || obj.ToString() == "")
                return false;
            else
                return true;
        }


        private int GetMaxId(string tableName)
        {
            int id = 0;
            string sql = "select Max(Id) from " + tableName;
            object obj = Lephone.Data.DbEntry.Context.ExecuteScalar(sql);
            if (obj == null || obj.ToString() == "")
            {
                id = 1;
            }
            else
            {
                id = int.Parse(obj.ToString());
            }
            return id;
        }

        /// <summary>
        /// 保存新的测量条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveNew_Click(object sender, EventArgs e)
        {
            if (ValidateHelper.IllegalCheck(txtConditionName) && WorkCurveHelper.DeviceCurrent!=null)
            {
                if (Condition.FindOne(c => c.Name == txtConditionName.Text && c.Device.Id == WorkCurveHelper.DeviceCurrent.Id) != null)
                {
                    Msg.Show(Info.ExistName);
                }
                else if (txtConditionName.Text.Equals(Info.Match) || txtConditionName.Text.Equals(Info.IntelligentCondition))
                {
                    string strFormat = Info.ConditionCanotBe + Info.Match + "," + Info.IntelligentCondition;
                    Msg.Show(strFormat);
                }
                else
                {
                    string sql = "insert into Condition(Name, Type, Device_Id) Values('" + txtConditionName.Text + "' ,0," + WorkCurveHelper.DeviceCurrent.Id + ")";
                    Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
                    int ConditionId = GetMaxId("Condition");

                    //不采取默认的
                    InitParameter InitParam = initParamCurrent;

                    //InitParameter InitParam = Default.GetInitParameter(WorkCurveHelper.DeviceCurrent.SpecLength);
                    //InitParam.FineGain = WorkCurveHelper.DeviceCurrent.ComType == ComType.FPGA ? 1 : (WorkCurveHelper.DeviceCurrent.ComType == ComType.USB && WorkCurveHelper.DeviceCurrent.IsDP5 ? (WorkCurveHelper.DeviceCurrent.Dp5Version == Dp5Version.Dp5_CommonUsb ? 8200 : 8.0f) : 120);
                    //InitParam.Gain = WorkCurveHelper.DeviceCurrent.ComType == ComType.FPGA ? 66 : (WorkCurveHelper.DeviceCurrent.ComType == ComType.USB && WorkCurveHelper.DeviceCurrent.IsDP5 ? 13 : 60);

                    
                    sql = "insert into InitParameter(TubVoltage, Tubcurrent, ElemName, Gain, FineGain, ActGain, ActFineGain, Channel, Filter, Collimator, ChannelError, Condition_Id,Target,TargetMode,CurrentRate,IsAdjustRate,MinRate,MaxRate,IsJoinInit,ExpressionFineGain) Values ("
                    + InitParam.TubVoltage + "," + InitParam.TubCurrent + ",'" + InitParam.ElemName + "'," + InitParam.Gain + "," + InitParam.FineGain + "," + InitParam.ActGain + ","
                    + InitParam.ActFineGain + "," + InitParam.Channel + "," + InitParam.Filter + "," + InitParam.Collimator + "," + InitParam.ChannelError + "," + ConditionId + "," + InitParam.Target + "," + (int)InitParam.TargetMode + "," + InitParam.CurrentRate + "," + (InitParam.IsAdjustRate ? 1 : 0) + "," + InitParam.MinRate + "," + InitParam.MaxRate + "," + (InitParam.IsJoinInit ? 1 : 0) + ",'" + InitParam.ExpressionFineGain + "')";
                    Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);


                    //同步最新的初始化参数
                    sql = "update InitParameter set TubVoltage = " + InitParam.TubVoltage + ", Tubcurrent = " + InitParam.TubCurrent + ", ElemName = '" + InitParam.ElemName + "', Gain = " + InitParam.Gain + ", FineGain = " + InitParam.FineGain + ", ActGain = " + InitParam.ActGain + ", ActFineGain = " + InitParam.ActFineGain +  ", Channel = " +  InitParam.Channel+", Filter =  " + InitParam.Filter +

                        ", Collimator = " + InitParam.Collimator + ", ChannelError = " + InitParam.ChannelError + ",Target = " + InitParam.Target + ",TargetMode = " + (int)InitParam.TargetMode + ",CurrentRate = " + InitParam.CurrentRate +

                        ",IsAdjustRate = " + (InitParam.IsAdjustRate ? 1 : 0) + ",MinRate = " + InitParam.MinRate + ",MaxRate = " + InitParam.MaxRate +

                        ",IsJoinInit = " + (InitParam.IsJoinInit ? 1 : 0) + ",ExpressionFineGain = '" + InitParam.ExpressionFineGain + "'";
                        
                    
                    Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);



                    DeviceParameter DevParam = Default.GetDeviceParameter(WorkCurveHelper.DeviceCurrent.SpecLength,1);
                    sql = "insert into DeviceParameter(Name, PrecTime, Tubcurrent, TubVoltage, FilterIdx, CollimatorIdx, TargetIdx, IsVacuum, VacuumTime, IsVacuumDegree, VacuumDegree, IsAdjustRate, MinRate," +
                          " MaxRate, BeginChann, EndChann, IsDistrubAlert, IsPeakFloat, PeakFloatLeft, PeakFloatRight, PeakFloatChannel, PeakFloatError, PeakCheckTime, Condition_Id,TargetMode,CurrentRate,IsFaceTubVoltage,FaceTubVoltage) Values('"
                          + DevParam.Name + "'," + DevParam.PrecTime + "," + DevParam.TubCurrent + "," + DevParam.TubVoltage + "," + DevParam.FilterIdx + "," + DevParam.CollimatorIdx + "," + DevParam.TargetIdx
                          + ",0," + DevParam.VacuumTime + ",0," + DevParam.VacuumDegree + ",0," + DevParam.MinRate + "," + DevParam.MaxRate + "," + DevParam.BeginChann + ","
                          + DevParam.EndChann + ",0,0,0,0,0,0,15," + ConditionId + ","+(int)DevParam.TargetMode+","+DevParam.CurrentRate+",0,40)";
                    Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
                    DemarcateEnergy DemarcateEnergy = Default.GetDemarcateEnergyAg(WorkCurveHelper.DeviceCurrent.SpecLength);
                    sql = "insert into DemarcateEnergy(ElementName, Line, Energy, Channel, Condition_Id) Values('" + DemarcateEnergy.ElementName + "'," + (int)DemarcateEnergy.Line + "," + DemarcateEnergy.Energy + "," + DemarcateEnergy.Channel + "," + ConditionId + ")";
                    Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
                    DemarcateEnergy = Default.GetDemarcateEnergyCu(WorkCurveHelper.DeviceCurrent.SpecLength);
                    sql = "insert into DemarcateEnergy(ElementName, Line, Energy, Channel, Condition_Id) Values('" + DemarcateEnergy.ElementName + "'," + (int)DemarcateEnergy.Line + "," + DemarcateEnergy.Energy + "," + DemarcateEnergy.Channel + "," + ConditionId + ")";
                    Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
                    GetConditions();
                    int index = lstCondition.FindIndex(l => l.Name == txtConditionName.Text);
                    this.cboConditionList.SelectedIndex = index;
                    this.txtConditionName.Visible = false;
                    this.btnSaveNew.Visible = false;
                    btnCancelSave.Visible = false;
                    btnOK.Visible = true;
                    btnCancel.Visible = true;
                    btnApplication.Visible = true;
                    btnDelCondition.Visible = true;
                    btnAddCondition.Visible = true;
                    btnApplication.Visible = true;
                }
            }
        }

        /// <summary>
        /// 取消保存测量条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelSave_Click(object sender, EventArgs e)
        {
            this.txtConditionName.Visible = false;
            this.btnSaveNew.Visible = false;
            btnCancelSave.Visible = false;
            btnOK.Visible = true;
            btnCancel.Visible = true;
            btnApplication.Visible = true;
            btnDelCondition.Visible = true;
            btnAddCondition.Visible = true;
            btnApplication.Visible = true;
        }

        /// <summary>
        /// 单元格验证事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvwTestConList_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            
            //if (dgvwTestConList.Columns[e.ColumnIndex].Name.Equals("ColName"))//名称
            //{
            //    if (String.IsNullOrEmpty(e.FormattedValue.ToString()))
            //    {
            //        Msg.Show(Info.NameIsNull);
            //        e.Cancel = true;
            //        dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = preCellValue;
            //        return;
            //    }
            //    for (int i = 0; i < dgvwTestConList.RowCount; i++)
            //    {
            //        if (e.RowIndex != i && dgvwTestConList[e.ColumnIndex, i].FormattedValue.ToString().Equals(e.FormattedValue.ToString()))
            //        {
            //            Msg.Show(Info.NameRepeat);
            //            dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = preCellValue;
            //            e.Cancel = true;
            //            return;
            //        }
            //    }
            //    string paramName = conditionCurrent.DeviceParamList[e.RowIndex].Name;
            //    var element = CurveElement.Find(ce => ce.ConditionDeviceName == paramName);
            //    if (element != null && element.Count > 0 && !paramName.Equals(e.FormattedValue.ToString()))
            //    {
            //        Msg.Show(Info.BeUsedCannotUpdate);
            //        dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = conditionCurrent.DeviceParamList[e.RowIndex].Name;
            //        return;
            //    }
            //    return;
            //}

            if (dgvwTestConList.Columns[e.ColumnIndex].Name.Equals("FaceTubVoltage")
                /*&& bool.Parse(dgvwTestConList.Rows[e.RowIndex].Cells["IsFaceTubVoltage"].FormattedValue.ToString())*/
               )//IsFaceTubVoltage
            {
                try
                {
                    double.Parse(e.FormattedValue.ToString());
                    //强制转换为Int型绑定数据为Int类型
                    dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (int)(double.Parse(e.FormattedValue.ToString())+0.5);
                }
                catch
                {
                    dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvwTestConList.Rows[e.RowIndex].Cells["TubVoltage"].Value;
                }

                if (double.Parse(dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) <= 0) dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvwTestConList.Rows[e.RowIndex].Cells["TubVoltage"].Value;
            }

            if (preCellValue == string.Empty)//第一次进入
            { return; }

            if (dgvwTestConList.Columns[e.ColumnIndex].Name.Equals("IsFaceTubVoltage") 
                && double.Parse(dgvwTestConList.Rows[e.RowIndex].Cells["FaceTubVoltage"].Value.ToString())<=0
                && bool.Parse(dgvwTestConList.Rows[e.RowIndex].Cells["IsFaceTubVoltage"].FormattedValue.ToString()))
            {
                dgvwTestConList.Rows[e.RowIndex].Cells["FaceTubVoltage"].Value = dgvwTestConList.Rows[e.RowIndex].Cells["TubVoltage"].Value;
            }
            


            if (dgvwTestConList.Columns[e.ColumnIndex].Name.Equals("PrecTime") || dgvwTestConList.Columns[e.ColumnIndex].Name.Equals("TubVoltage") ||
                dgvwTestConList.Columns[e.ColumnIndex].Name.Equals("TubCurrent") || dgvwTestConList.Columns[e.ColumnIndex].Name.Equals("FilterIdx") ||
                dgvwTestConList.Columns[e.ColumnIndex].Name.Equals("ColTargetIdx") || dgvwTestConList.Columns[e.ColumnIndex].Name.Equals("CollimatorIdx") || 
                dgvwTestConList.Columns[e.ColumnIndex].Name.Equals("VacuumTime") || dgvwTestConList.Columns[e.ColumnIndex].Name.Equals("MinRate") ||
                dgvwTestConList.Columns[e.ColumnIndex].Name.Equals("MaxRate") || dgvwTestConList.Columns[e.ColumnIndex].Name.Equals("ColCurrentRate") )//测量时间,管压,管流,滤光片...
            {
                decimal Max = Ranges.RangeDictionary[dgvwTestConList.Columns[e.ColumnIndex].Name].Max;
                decimal Min = Ranges.RangeDictionary[dgvwTestConList.Columns[e.ColumnIndex].Name].Min;
                try { int.Parse(e.FormattedValue.ToString()); }
                catch (Exception)
                {
                    //SkyrayMsgBox.Show(Info.StyleError);
                    e.Cancel = true;
                    dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Max;
                    return;
                }
                if (int.Parse(e.FormattedValue.ToString()) > Max)
                {
                    //SkyrayMsgBox.Show(Info.OutOfRange);
                    dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Max;
                    e.Cancel = true;
                }
                if (int.Parse(e.FormattedValue.ToString()) < Min)
                {
                    //SkyrayMsgBox.Show(Info.OutOfRange);
                    dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Min;
                    e.Cancel = true;
                }

                if (dgvwTestConList.Columns[e.ColumnIndex].Name.Equals("MaxRate"))
                {
                    if (int.Parse(dgvwTestConList["MinRate", e.RowIndex].Value.ToString()) > int.Parse(e.FormattedValue.ToString()))
                    {
                        //SkyrayMsgBox.Show(Info.MaxMustBigThanMin);
                        dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Max;
                        e.Cancel = true;
                    }
                    return;
                }
                if (dgvwTestConList.Columns[e.ColumnIndex].Name.Equals("MinRate"))
                {
                    if (int.Parse(dgvwTestConList["MaxRate", e.RowIndex].Value.ToString()) < int.Parse(e.FormattedValue.ToString()))
                    {
                        //SkyrayMsgBox.Show(Info.MaxMustBigThanMin);
                        dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Min;
                        e.Cancel = true;
                    }
                    return;
                }
                if (dgvwTestConList.Columns[e.ColumnIndex].Name.Equals("TubVoltage") && WorkCurveHelper.DeviceCurrent.HasTarget)
                {
                    if ((TargetMode)dgvwTestConList["ColTargetMode", e.RowIndex].Value == TargetMode.TwoTarget 
                        && int.Parse(dgvwTestConList["TubVoltage", e.RowIndex].Value.ToString())< 10)
                    {
                        dgvwTestConList["TubVoltage", e.RowIndex].Value = 10;
                    }
                }
            }
            if (dgvwTestConList.Columns[e.ColumnIndex].Name.Equals("ColTargetMode") && WorkCurveHelper.DeviceCurrent.HasTarget)
            {
                if ((TargetMode)dgvwTestConList["ColTargetMode", e.RowIndex].Value == TargetMode.TwoTarget
                    && int.Parse(dgvwTestConList["TubVoltage", e.RowIndex].Value.ToString()) < 10)
                {
                    dgvwTestConList["TubVoltage", e.RowIndex].Value = 10;
                }
            }

            if (dgvwTestConList.Columns[e.ColumnIndex].Name.Equals("ColBeginChann"))
            {
                try { int.Parse(e.FormattedValue.ToString()); }
                catch (Exception)
                {
                    e.Cancel = true;
                    dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 50;
                    return;
                }
                if (int.Parse(e.FormattedValue.ToString()) < 0)
                {
                    dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0;
                    e.Cancel = true;
                }
                if (int.Parse(e.FormattedValue.ToString()) > (int)WorkCurveHelper.DeviceCurrent.SpecLength)
                {
                    dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (int)WorkCurveHelper.DeviceCurrent.SpecLength - 1;
                    e.Cancel = true;
                }
            }
            if (dgvwTestConList.Columns[e.ColumnIndex].Name.Equals("ColEndChann"))
            {
                try { int.Parse(e.FormattedValue.ToString()); }
                catch (Exception)
                {
                    e.Cancel = true;
                    dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (int)WorkCurveHelper.DeviceCurrent.SpecLength - 1;
                    return;
                }
                if (int.Parse(e.FormattedValue.ToString()) < 0)
                {
                    dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (int)WorkCurveHelper.DeviceCurrent.SpecLength - 1;
                    e.Cancel = true;
                }
                if (int.Parse(e.FormattedValue.ToString()) >= (int)WorkCurveHelper.DeviceCurrent.SpecLength)
                {
                    dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (int)WorkCurveHelper.DeviceCurrent.SpecLength - 1;
                    e.Cancel = true;
                }
            }
            if (dgvwTestConList.Columns[e.ColumnIndex].Name.Equals("VacuumDegree"))
            {
                decimal Max = Ranges.RangeDictionary[dgvwTestConList.Columns[e.ColumnIndex].Name].Max;
                decimal Min = Ranges.RangeDictionary[dgvwTestConList.Columns[e.ColumnIndex].Name].Min;
                string value = e.FormattedValue.ToString();
                try { decimal.Parse(e.FormattedValue.ToString()); }
                catch (Exception)
                {
                    dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Max;
                    value = Max.ToString();
                    e.Cancel = true;
                    return;
                }
                if (decimal.Parse(e.FormattedValue.ToString()) > Max)
                {
                    dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Max;
                    value = Max.ToString();
                    e.Cancel = true;
                }
                if (decimal.Parse(e.FormattedValue.ToString()) < Min)
                {
                    dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Min;
                    value = Min.ToString();
                    e.Cancel = true;
                }
                if (value.Contains("."))
                {
                    dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = decimal.Parse(value).ToString("f2");
                }
            }
            if(dgvwTestConList.Columns[e.ColumnIndex].Name.Equals(PeakCheckTime.Name))
            {
                int Max = int.Parse( dgvwTestConList.Rows[e.RowIndex].Cells[PrecTime.Name].Value.ToString());
                int Min = 0;
                string value = e.FormattedValue.ToString();
                try
                {
                    int.Parse(e.FormattedValue.ToString());
                }
                catch (Exception)
                {
                    dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Min;
                    value = Min.ToString();
                    e.Cancel = true;
                    return;
                }
                if (int.Parse(e.FormattedValue.ToString()) > Max)
                {
                    dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Max;
                    value = Max.ToString();
                    e.Cancel = true;
                }
                if (int.Parse(e.FormattedValue.ToString()) < Min)
                {
                    dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Min;
                    value = Min.ToString();
                    e.Cancel = true;
                }
            }
            if (dgvwTestConList.Columns[e.ColumnIndex].Name.Equals(ColPeakFloatLeft.Name) ||
                dgvwTestConList.Columns[e.ColumnIndex].Name.Equals(ColPeakFloatRight.Name))
            {
                int Max = int.Parse(dgvwTestConList.Rows[e.RowIndex].Cells[ColEndChann.Name].Value.ToString());
                int Min = int.Parse(dgvwTestConList.Rows[e.RowIndex].Cells[ColBeginChann.Name].Value.ToString());
                string value = e.FormattedValue.ToString();
                try
                {
                    int.Parse(e.FormattedValue.ToString());
                }
                catch (Exception)
                {
                    if (dgvwTestConList.Columns[e.RowIndex].Name.Equals(ColPeakFloatLeft.Name))
                        dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Min;
                    else if (dgvwTestConList.Columns[e.RowIndex].Name.Equals(ColPeakFloatRight.Name))
                        dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Max;
                    value = dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    e.Cancel = true;
                    return;
                }
                if (int.Parse(e.FormattedValue.ToString()) > Max)
                {
                    dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Max;
                    value = Max.ToString();
                    e.Cancel = true;
                }
                if (int.Parse(e.FormattedValue.ToString()) < Min)
                {
                    dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Min;
                    value = Min.ToString();
                    e.Cancel = true;
                }
                if(dgvwTestConList.Columns[e.ColumnIndex].Name.Equals(ColPeakFloatLeft.Name)&&dgvwTestConList.Rows[0].Cells[ColPeakFloatRight.Name].Value.ToString()=="0")
                {
                    dgvwTestConList.Rows[e.RowIndex].Cells[ColPeakFloatLeft.Name].Value = int.Parse(e.FormattedValue.ToString());
                    dgvwTestConList.Rows[e.RowIndex].Cells[ColPeakFloatRight.Name].Value = Max;
                }
                if (int.Parse(dgvwTestConList.Rows[e.RowIndex].Cells[ColPeakFloatRight.Name].Value.ToString()) <= int.Parse(dgvwTestConList.Rows[e.RowIndex].Cells[ColPeakFloatLeft.Name].Value.ToString()))
                {
                    dgvwTestConList.Rows[e.RowIndex].Cells[ColPeakFloatLeft.Name].Value = dgvwTestConList.Rows[e.RowIndex].Cells[ColBeginChann.Name].Value;
                    dgvwTestConList.Rows[e.RowIndex].Cells[ColPeakFloatRight.Name].Value = dgvwTestConList.Rows[e.RowIndex].Cells[ColEndChann.Name].Value;
                }
            }
            if (dgvwTestConList.Columns[e.ColumnIndex].Name.Equals(ColPeakFloatChannel.Name))
            {
                int Max = int.Parse(dgvwTestConList.Rows[e.RowIndex].Cells[ColEndChann.Name].Value.ToString());
                int Min = int.Parse(dgvwTestConList.Rows[e.RowIndex].Cells[ColBeginChann.Name].Value.ToString());
                string value = e.FormattedValue.ToString();
                try
                {
                    int.Parse(e.FormattedValue.ToString());
                }
                catch (Exception)
                {
                    dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Min;
                    value = Min.ToString();
                    e.Cancel = true;
                    return;
                }
                if (int.Parse(e.FormattedValue.ToString()) > Max)
                {
                    dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Max;
                    value = Max.ToString();
                    e.Cancel = true;
                }
                if (int.Parse(e.FormattedValue.ToString()) < Min)
                {
                    dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Min;
                    value = Min.ToString();
                    e.Cancel = true;
                }
            }
            if (dgvwTestConList.Columns[e.ColumnIndex].Name.Equals(ColPeakFloatError.Name))
            {
                int Max = int.Parse(dgvwTestConList.Rows[e.RowIndex].Cells[ColEndChann.Name].Value.ToString());
                int Min = 0;
                string value = e.FormattedValue.ToString();
                try
                {
                    int.Parse(e.FormattedValue.ToString());
                }
                catch (Exception)
                {
                    dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Min;
                    value = Min.ToString();
                    e.Cancel = true;
                    return;
                }
                if (int.Parse(e.FormattedValue.ToString()) > Max)
                {
                    dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Max;
                    value = Max.ToString();
                    e.Cancel = true;
                }
                if (int.Parse(e.FormattedValue.ToString()) < Min)
                {
                    dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Min;
                    value = Min.ToString();
                    e.Cancel = true;
                }
            }
        }
        /// <summary>
        /// 单元格点击，用于记录内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvwTestConList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                preCellValue = dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null ? "" : dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                if (dgvwTestConList.Columns[e.ColumnIndex].Name.Equals("IsFaceTubVoltage")
                && double.Parse(dgvwTestConList.Rows[e.RowIndex].Cells["FaceTubVoltage"].Value.ToString()) <= 0
                && !bool.Parse(dgvwTestConList.Rows[e.RowIndex].Cells["IsFaceTubVoltage"].FormattedValue.ToString()))
                {
                    dgvwTestConList.Rows[e.RowIndex].Cells["FaceTubVoltage"].Value = dgvwTestConList.Rows[e.RowIndex].Cells["TubVoltage"].Value;
                }
            }

        }
        /// <summary>
        /// 单元格内容，用于判断抽真空不重复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvwTestConList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (dgvwTestConList.Columns[e.ColumnIndex].Name.Equals("ColIsVacuum"))//时间抽真空
            {
                if (dgvwTestConList["ColIsVacuumDegree", e.RowIndex].Value.Equals(true))
                {
                    dgvwTestConList[e.ColumnIndex, e.RowIndex].Value = true;
                    dgvwTestConList["ColIsVacuumDegree", e.RowIndex].Value = false;
                    //Msg.Show(Info.IsVacuumSelectBoth);
                    return;
                }
            }
            else if (dgvwTestConList.Columns[e.ColumnIndex].Name.Equals("ColIsVacuumDegree"))//真空度抽真空
            {
                if (dgvwTestConList["ColIsVacuum", e.RowIndex].Value.Equals(true))
                {
                    dgvwTestConList[e.ColumnIndex, e.RowIndex].Value = true;
                    dgvwTestConList["ColIsVacuum", e.RowIndex].Value = false;
                    //Msg.Show(Info.IsVacuumSelectBoth);
                    return;
                }
            }
        }

        #endregion

        private void btnApplication_Click(object sender, EventArgs e)
        {
            // if (dgvwTestConList.Columns["ColTargetMode"])
            AppUse();
            //修改：何晓明 20111101 条件第二次按应用无效
            cboConditionList_SelectedIndexChanged(null, null);
            
        }

        private void AppUse()
        {
        
            for (int i = 0; i < lstCondition.Count; i++)
            {
                //if (UpdatedRowIndexs.Contains(i))
                {
                    DeviceParameter wParams = lstCondition[i].DeviceParamList.ToList().Find(delegate(DeviceParameter s) { return s.MinRate > s.MaxRate; });
                    if (wParams != null)
                    {
                        return;
                    }
                }
            }

           
           
            for (int i = 0; i < lstCondition.Count; i++)
            {
                            
                lstCondition[i].Save();//未更改的数据不保存
                if (WorkCurveHelper.WorkCurveCurrent != null && WorkCurveHelper.WorkCurveCurrent.Condition.Id == lstCondition[i].Id)
                {
                    initParamCurrent.Id = lstCondition[i].InitParam.Condition.Id;
                    lstCondition[i].InitParam = initParamCurrent;
                    WorkCurveHelper.WorkCurveCurrent.Condition = lstCondition[i];
                }
                
            }

            //此处将最后更改的初始化条件同步到所有条件
            InitParameter InitParam = initParamCurrent;
            
            //同步最新的初始化参数
            
            string sql = "update InitParameter set TubVoltage = " + InitParam.TubVoltage + ", Tubcurrent = " + InitParam.TubCurrent + ", ElemName = '" + InitParam.ElemName + "', Gain = " + InitParam.Gain + ", FineGain = " + InitParam.FineGain + ", ActGain = " + InitParam.ActGain + ", ActFineGain = " + InitParam.ActFineGain + ", Channel = " + InitParam.Channel + ", Filter =  " + InitParam.Filter +

                ", Collimator = " + InitParam.Collimator + ", ChannelError = " + InitParam.ChannelError + ",Target = " + InitParam.Target + ",TargetMode = " + (int)InitParam.TargetMode + ",CurrentRate = " + InitParam.CurrentRate +

                ",IsAdjustRate = " + (InitParam.IsAdjustRate ? 1 : 0) + ",MinRate = " + InitParam.MinRate + ",MaxRate = " + InitParam.MaxRate +

                ",IsJoinInit = " + (InitParam.IsJoinInit ? 1 : 0) + ",ExpressionFineGain = '" + InitParam.ExpressionFineGain + "'";

            Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
            


            WorkCurveHelper.DeviceCurrent = Device.FindById(WorkCurveHelper.DeviceCurrent.Id);
            initCondition();
        }

        private void initCondition()
        {
            lstCondition = Condition.Find(c => c.Device.Id == WorkCurveHelper.DeviceCurrent.Id && c.Type == ConditionType.Normal);
            var lst = Condition.Find(c => c.Device.Id == WorkCurveHelper.DeviceCurrent.Id && c.Type == ConditionType.Match);
            if (lst.Count > 0)
            {
                lst[0].Name = Info.Match;
                lstCondition.Add(lst[0]);
            }
            lst = Condition.Find(c => c.Device.Id == WorkCurveHelper.DeviceCurrent.Id && c.Type == ConditionType.Intelligent);
            
            if (lst.Count > 0)
            {
                lst[0].Name = Info.IntelligentCondition;
                lstCondition.Add(lst[0]);
            }
            lst = Condition.Find(c => c.Device.Id == WorkCurveHelper.DeviceCurrent.Id && c.Type == ConditionType.Detection);
            if (lst.Count > 0)
            {
                lst[0].Name = Info.strDetection;
                lstCondition.Add(lst[0]);
            }
            lst = Condition.Find(c => c.Device.Id == WorkCurveHelper.DeviceCurrent.Id && c.Type == ConditionType.Match2);
            if (lst.Count > 0)
            {
                lst[0].Name = Info.Match+2;
                lstCondition.Add(lst[0]);
            }
        }

        private void dgvwTestConList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1) return;

           

            if ( WorkCurveHelper.DeviceCurrent.HasTarget &&dgvwTestConList.Columns[e.ColumnIndex].Name.Equals("TubVoltage") )
            {
                if ((TargetMode)dgvwTestConList["ColTargetMode", e.RowIndex].Value == TargetMode.TwoTarget
                    && int.Parse(dgvwTestConList["TubVoltage", e.RowIndex].Value.ToString()) < 10)
                {
                    dgvwTestConList["TubVoltage", e.RowIndex].Value = 10;
                    dgvwTestConList["FaceTubVoltage", e.RowIndex].Value = 10;
                }
            }

            if (WorkCurveHelper.DeviceCurrent.HasTarget && dgvwTestConList.Columns[e.ColumnIndex].Name.Equals("ColTargetMode")  )
            {
                if ((TargetMode)dgvwTestConList["ColTargetMode", e.RowIndex].Value == TargetMode.TwoTarget
                    && int.Parse(dgvwTestConList["TubVoltage", e.RowIndex].Value.ToString()) < 10)
                {
                    dgvwTestConList["TubVoltage", e.RowIndex].Value = 10;
                    dgvwTestConList["FaceTubVoltage", e.RowIndex].Value = 10;
                }
            }

            if (dgvwTestConList.Columns[e.ColumnIndex].Name.Equals("FaceTubVoltage")
             && bool.Parse(dgvwTestConList.Rows[e.RowIndex].Cells["IsFaceTubVoltage"].FormattedValue.ToString())
            )//IsFaceTubVoltage
            {
                try
                {
                    double.Parse(dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                    //if ((TargetMode)dgvwTestConList["ColTargetMode", e.RowIndex].Value == TargetMode.TwoTarget
                    //&& int.Parse(dgvwTestConList["FaceTubVoltage", e.RowIndex].Value.ToString()) < 10)
                    //{
                    //    dgvwTestConList["FaceTubVoltage", e.RowIndex].Value = 10;
                    //}
                }
                catch
                {
                    dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvwTestConList.Rows[e.RowIndex].Cells["TubVoltage"].Value;
                }

                if (double.Parse(dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) <= 0) dgvwTestConList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvwTestConList.Rows[e.RowIndex].Cells["TubVoltage"].Value;
            }
            
        }
    }
}