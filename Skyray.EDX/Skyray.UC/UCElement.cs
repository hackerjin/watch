using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;
using Skyray.EDXRFLibrary.Define;
using Skyray.EDX.Common;
using Skyray.Controls;
using System.Threading;
using Lephone.Data.Definition;
using System.Collections;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.UC
{
    /// <summary>
    /// 编辑感兴趣元素类
    /// </summary>
    public partial class UCElement : Skyray.Language.UCMultiple
    {

        #region Fileds

        /// <summary>
        /// 拟合元素
        /// </summary>
        private string fitElements = string.Empty;

        /// <summary>
        /// 当前工作曲线
        /// </summary>
        private WorkCurve workCurveCurrent;

        /// <summary>
        /// 纯元素谱List
        /// </summary>
        private SpecListEntity specListSpectrum;

        /// <summary>
        /// 元素组
        /// </summary>
        private string[] atomnames;

        /// <summary>
        /// 线系组
        /// </summary>
        private int[] atomlines;

        private bool useBlueColor;

        /// <summary>
        /// 用来表示层数的列
        /// </summary>
        private DataGridViewTextBoxColumn dgvtxt;

        /// <summary>
        /// 用来表示差额的列
        /// </summary>
        private DataGridViewCheckBoxColumn ColDifference;

        /// <summary>
        /// 对话结果
        /// </summary>
        public DialogResult DialogResult { get; set; }

        /// <summary>
        /// 测试条件
        /// </summary>
        //private object[] Condition;

        /// <summary>
        /// 强度计算方法
        /// </summary>
        private List<Auto> lstIntensityMethod;

        /// <summary>
        /// 背景强度计算方法
        /// </summary>
        private List<Auto> lstBaseIntensityMethod;

        /// <summary>
        /// 待删除
        /// </summary>
        private List<CurveElement> lstReadyDel;

        /// <summary>
        /// 定时器时刻侦听
        /// </summary>
        private System.Timers.Timer timer = new System.Timers.Timer();
        /// <summary>
        /// 全局变量获取dgvwElements定位位置
        /// </summary>
        private int firstDisplayScrollingRowIndex = 0;
        /// <summary>
        /// 默认用于计算K值主元素为Au
        /// </summary>
        private const string defaultMainElementToCalcKarat = "Au";

        /// <summary>
        /// 可添加纯元素列表
        /// </summary>
        private List<string> lstPureElems;
        #endregion

        #region Init

        /// <summary>
        /// 构造
        /// </summary>
        public UCElement()
        {
            InitializeComponent();
            dgvwElements.CellPainting += new DataGridViewCellPaintingEventHandler(dgvwElements_CellPainting);
            dgvwElements.CellValueChanged += new DataGridViewCellEventHandler(dgvwElements_CellValueChanged);
            DialogResult = DialogResult.No;
            lstReadyDel = new List<CurveElement>();

            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Interval = 30;

            OnFixDGVRowIndex = ActiveFixDgvRowIndex;

            useBlueColor = (ReportTemplateHelper.LoadSpecifiedValue("ElementParams", "DefaultColor") == "1") ? true : false;
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (firstDisplayScrollingRowIndex > 0 && this.dgvwElements.Rows[0].Selected == true)
            {
                this.Invoke(OnFixDGVRowIndex);
                firstDisplayScrollingRowIndex = 0;
                timer.Enabled = false;
            }
        }

        private delegate void FixDGVRowIndex();
        private FixDGVRowIndex OnFixDGVRowIndex = null;
        private void ActiveFixDgvRowIndex()
        {
            dgvwElements.FirstDisplayedScrollingRowIndex = firstDisplayScrollingRowIndex;
            dgvwElements.CurrentCell = dgvwElements.Rows[firstDisplayScrollingRowIndex].Cells[0];
            this.dgvwElements_CellDoubleClick(null, new DataGridViewCellEventArgs(0, firstDisplayScrollingRowIndex));
        }
        /// <summary>
        /// 重画单元格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgvwElements_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                var cell = dgvwElements[e.ColumnIndex, e.RowIndex];
                if (e.ColumnIndex == ColColor.Index)
                {
                    cell.Style.BackColor = cell.Style.SelectionBackColor = Color.FromArgb(workCurveCurrent.ElementList.Items[e.RowIndex].Color);
                }
                else if (e.ColumnIndex == ColIntensityWay.Index)
                {
                    string key = workCurveCurrent.ElementList.Items[e.RowIndex].IntensityWay.ToString();
                    var auto = lstIntensityMethod.Find(a => a.Key == key);
                    if (auto != null) cell.Value = auto.Key;
                }
                else if (e.ColumnIndex == ColBaseIntensityWay.Index)
                {
                    string key = workCurveCurrent.ElementList.Items[e.RowIndex].BaseIntensityWay.ToString();
                    var auto = lstBaseIntensityMethod.Find(a => a.Key == key);
                    if (auto != null) cell.Value = auto.Key;
                }
                else if (e.ColumnIndex == ColContentUnit.Index)
                {
                    //"‰"  permillage
                    if (workCurveCurrent.ElementList.Items[e.RowIndex].ContentUnit == ContentUnit.per)
                    {
                        dgvwElements[ColContentUnit.Name, e.RowIndex].Value = "%";
                    }
                    else if (workCurveCurrent.ElementList.Items[e.RowIndex].ContentUnit == ContentUnit.ppm)
                    {
                        dgvwElements[ColContentUnit.Name, e.RowIndex].Value = "ppm";
                    }
                    else
                    {
                        dgvwElements[ColContentUnit.Name, e.RowIndex].Value = "‰";
                    }
                }
                else if (e.ColumnIndex == ColThicknessUnit.Index)
                {
                    if (workCurveCurrent.ElementList.Items[e.RowIndex].ThicknessUnit == ThicknessUnit.um)
                    { 
                         dgvwElements[ColThicknessUnit.Name, e.RowIndex].Value ="um";

                    }
                    else if (workCurveCurrent.ElementList.Items[e.RowIndex].ThicknessUnit == ThicknessUnit.ur)
                    {
                        dgvwElements[ColThicknessUnit.Name, e.RowIndex].Value = "u〞";
                    }
                    else if (workCurveCurrent.ElementList.Items[e.RowIndex].ThicknessUnit == ThicknessUnit.gl)
                    {
                        dgvwElements[ColThicknessUnit.Name, e.RowIndex].Value = "g/L";
                    }
                    else
                    {
                        dgvwElements[ColThicknessUnit.Name, e.RowIndex].Value = "mgm2";
                    }
                  //  dgvwElements[ColThicknessUnit.Name, e.RowIndex].Value = workCurveCurrent.ElementList.Items[e.RowIndex].ThicknessUnit == ThicknessUnit.um ? "um" : "u〞";
                }
            }
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCElement_Load(object sender, EventArgs e)
        {
            workCurveCurrent = WorkCurve.FindById(WorkCurveHelper.WorkCurveCurrent.Id);
            if (workCurveCurrent == null) return;
            lstIntensityMethod = GetIntensityMethod(workCurveCurrent.FuncType, workCurveCurrent.CalcType);
            lstBaseIntensityMethod = GetBaseIntensityMethod();
            //lstPureElems = GetPureElems();
            //Condition = new object[workCurveCurrent.Condition.DeviceParamList.Count];
            //for (int i = 0; i < workCurveCurrent.Condition.DeviceParamList.Count; i++)
            //{
            //    Condition[i] = workCurveCurrent.Condition.DeviceParamList[i].Name;//加载当前测量条件下的所有测试小条件
            //}
            InitDGVColumn();//初始化列头
            //InitSomeControl();//初始化UI里的一些控件
            if (workCurveCurrent.ElementList != null)
            {
                if (workCurveCurrent.ElementList.Items.Count > 0)
                {
                    List<CurveElement> lst = workCurveCurrent.ElementList.Items.ToList().OrderBy(data => data.RowsIndex).ToList();
                    for (int i = 0; i < workCurveCurrent.ElementList.Items.Count; i++)
                    {
                        workCurveCurrent.ElementList.Items[i] = lst[i];
                        //if (DifferenceDevice.IsAnalyser && workCurveCurrent.ElementList.Items[i].ReferenceElements.Length >= 1)
                        //    workCurveCurrent.ElementList.Items[i].IntensityWay = IntensityWay.FixedReference;
                    }
                    var ele = workCurveCurrent.ElementList.Items.ToList().Find(d => d.Samples.Count > 0);
                    ElementListToGrid();//元素列表绑定至表格
                    //修改：何晓明 20110715 英文状态下加载所在层为中文
                    if (workCurveCurrent.ElementList != null)
                    {
                        atomnames = new string[workCurveCurrent.ElementList.Items.Count];
                        atomlines = new int[workCurveCurrent.ElementList.Items.Count];
                        for (int i = 0; i < atomnames.Length; i++)
                        {
                            atomnames[i] = workCurveCurrent.ElementList.Items[i].Caption;
                            atomlines[i] = (int)workCurveCurrent.ElementList.Items[i].AnalyteLine;
                        }
                    }
                    bool isEc = WorkCurveHelper.CalcType == CalcType.FP ? false : true;
                    FrmElementTable frm = new FrmElementTable(atomnames, atomlines, workCurveCurrent.ElementList, false, isEc);
                    if (DifferenceDevice.IsThick)//测厚
                    {
                        if (workCurveCurrent.ElementList.Items.OrderBy(s => s.LayerNumber).ToList()[0].Samples.Count > 0)
                        {
                            btnAdd.Enabled = btnDel.Enabled = false;
                        }
                        ElementForThick(frm.lstName);//测厚添加元素
                        
                    }

                    //
                    ReferenceElementToGrid();
                }
            }
            else
            {
                workCurveCurrent.ElementList = Default.GetElementList();
                workCurveCurrent.ElementList.PureAsInfinite = true;
                workCurveCurrent.ElementList.MatchSpecListIdStr = "";
                workCurveCurrent.ElementList.RefSpecListIdStr = "";
                workCurveCurrent.ElementList.Save();
            }
            txtLayerElems.BindVisibleToCtrl(chkRhThick,"Checked");
            BindHelper.BindTextToCtrl(txtLayerElems, workCurveCurrent.ElementList, "LayerElemsInAnalyzer", true);
            BindHelper.BindCheckedToCtrl(cbeUnitary, workCurveCurrent.ElementList, "IsUnitary", true);//绑定数据
            BindHelper.BindTextToCtrl(numUnitaryValue, workCurveCurrent.ElementList, "UnitaryValue", true);
           // BindHelper.BindCheckedToCtrl(radRhIsLayer, workCurveCurrent.ElementList, "RhIsLayer", true);//绑定铑是镀层
            //BindHelper.BindTextToCtrl(diRhLayerFactor, workCurveCurrent.ElementList, "RhLayerFactor", true);
            BindHelper.BindCheckedToCtrl(chkRemoveBack, workCurveCurrent.ElementList, "IsRemoveBk", true);//绑定数据
            numUnitaryValue.Maximum = Ranges.RangeDictionary["UnitaryValue"].Max;//数据范围
            numUnitaryValue.Minimum = Ranges.RangeDictionary["UnitaryValue"].Min;//最小值
            numUnitaryValue.DecimalPlaces = Ranges.RangeDictionary["UnitaryValue"].DecimalPlaces;
            numUnitaryValue.Increment = Ranges.RangeDictionary["UnitaryValue"].Increment;

            BindHelper.BindCheckedToCtrl(chkIsAbsorb, workCurveCurrent, "IsAbsorb");//是否吸收法
            BindHelper.BindCheckedToCtrl(chkShowContent, workCurveCurrent, "IsThickShowContent");//是否显示含量
            BindHelper.BindCheckedToCtrl(chkInfinite, workCurveCurrent.ElementList, "PureAsInfinite");//纯元素作为无限厚点
            BindHelper.BindCheckedToCtrl(chkRMBG, workCurveCurrent, "RemoveBackGround");//扣背景
            BindHelper.BindCheckedToCtrl(chkRMSum, workCurveCurrent, "RemoveSum");//去和峰
            BindHelper.BindCheckedToCtrl(chkRMEscape, workCurveCurrent, "RemoveEscape");//去逃逸峰
            BindHelper.BindCheckedToCtrl(chkIsReportCategory, workCurveCurrent.ElementList, "IsReportCategory");//是否报规格
            BindHelper.BindCheckedToCtrl(chkNoStandardAlert, workCurveCurrent.ElementList, "NoStandardAlert");//非标报警
            BindHelper.BindCheckedToCtrl(chkCurrent, workCurveCurrent, "IsCurrentNormalize");//绑定数据
            BindHelper.BindCheckedToCtrl(chkNip2, workCurveCurrent, "IsNiP2");//绑定数据
            BindHelper.BindCheckedToCtrl(chkBaseAdjust, workCurveCurrent, "IsBaseAdjust");//绑定数据
            BindHelper.BindCheckedToCtrl(chkUnToMainElem, workCurveCurrent.ElementList, "RhIsMainElementInfluence",true);//绑定数据
            ///控件显示说明
            ///归一含量：XRF、3000显示
            ///报规格：自定义牌号XRF显示
            ///铑是合金、铑是镀层：3000显示
            ///扣背景、去和峰、去逃逸峰目前不用
            ///吸收法：FPThick EC显示
            ///显示含量、纯元素作为作为无限厚度加入：FPThick FP显示

            //this.chkRemoveBack.Visible = false;
            this.chkRMBG.Visible = false;
            this.chkRMSum.Visible = false;
            this.chkRMEscape.Visible = false;
            //this.chkIsReportCategory.Visible = false;
            if (WorkCurveHelper.CalcType == CalcType.PeakDivBase && DifferenceDevice.IsThick)
            {
                this.chkIsAbsorb.Visible = false;
                this.chkInfinite.Visible = false;
            }
            this.dgvwElements.Columns[0].Frozen = true;

            //@CYR180428
            if (DifferenceDevice.IsAnalyser && Application.ProductName.Equals(@"Skyray.EDX3000"))
            {
                cbxWMainElement.Visible = lblMainElement.Visible = true;
                if (atomnames != null && atomnames.Length > 0)
                {
                    cbxWMainElement.Items.AddRange(atomnames);
                }
                //若不包含默认元素, 则添加
                if (!cbxWMainElement.Items.Contains(defaultMainElementToCalcKarat))
                {
                    cbxWMainElement.Items.Add(defaultMainElementToCalcKarat);
                }
                cbxWMainElement.Text = workCurveCurrent.ElementList == null ? defaultMainElementToCalcKarat : workCurveCurrent.ElementList.MainElementToCalcKarat;
            }
            else
            {
                cbxWMainElement.Text = defaultMainElementToCalcKarat;
            }
            
        }

        #endregion

        #region Methods

        /// <summary>
        /// 获取强度计算方法
        /// </summary>
        /// <param name="funcType"></param>
        /// <returns></returns>
        private List<Auto> GetIntensityMethod(FuncType funcType, CalcType calcType)
        {
            List<Auto> lst = new List<Auto>();
            if (funcType == FuncType.Thick && calcType != CalcType.PeakDivBase)
            {
                lst.Add(new Auto("Reference", "纯元素拟合"));
                lst.Add(new Auto("FixedReference", "Fixed纯元素拟合"));
                lst.Add(new Auto("FullArea", "全面积"));
                lst.Add(new Auto("NetArea", "净面积"));
                lst.Add(new Auto("Gauss", "高斯拟合"));
               // lst.Add(new Auto("FixedReference", "Fixed纯元素拟合"));
                //if (calcType == CalcType.FP)//去除测厚里的其他计算方法
                //{
                //    lst.Add(new Auto("Gauss", "高斯拟合"));
                //    lst.Add(new Auto("FPGauss", "FP高斯拟合"));
                //    lst.Add(new Auto("FullArea", "全面积"));
                //    lst.Add(new Auto("NetArea", "净面积"));
                //}
                //if (calcType == CalcType.EC)
                //{
                //    lst.Add(new Auto("FixedReference", "Fixed纯元素拟合"));
                //}
                //else
                //{
                //    lst.Add(new Auto("Gauss", "高斯拟合"));
                //    //lst.Add(new Auto("FPGauss", "FP高斯拟合"));
                //}
            }
            else if (funcType == FuncType.Rohs)
            {
                lst.Add(new Auto("FullArea", "全面积"));
                lst.Add(new Auto("NetArea", "净面积"));
                lst.Add(new Auto("Reference", "纯元素拟合"));
                lst.Add(new Auto("FixedGauss", "高斯拟合"));
                if (calcType == CalcType.FP)
                {
                    lst.Add(new Auto("FPGauss", "FP高斯拟合"));
                }
            }
            else
            {
                lst.Add(new Auto("FullArea", "全面积"));
                lst.Add(new Auto("NetArea", "净面积"));
                lst.Add(new Auto("FixedReference", "Fixed纯元素拟合"));
                lst.Add(new Auto("Gauss", "高斯拟合"));
                //if (calcType == CalcType.FP)
                //{
                lst.Add(new Auto("FPGauss", "FP高斯拟合"));
                //}
            }

            if (Skyray.Language.Lang.Model.CurrentLang.IsDefaultLang)
            {
                Skyray.Language.Lang.Model.SaveTextProperty(lst);
            }
            else
            {
                Skyray.Language.Lang.Model.SetTextProperty(lst);
            }

            return lst;
        }

        private List<Auto> GetBaseIntensityMethod()
        {
            List<Auto> lst = new List<Auto>();
            lst.Add(new Auto("FullArea", "全面积"));
            lst.Add(new Auto("WipeSpecialtyArea", "连续谱背景"));
            if (Skyray.Language.Lang.Model.CurrentLang.IsDefaultLang)
            {
                Skyray.Language.Lang.Model.SaveTextProperty(lst);
            }
            else
            {
                Skyray.Language.Lang.Model.SetTextProperty(lst);
            }
            return lst;
        }

        /// <summary>
        /// 添加数据列
        /// </summary>
        private void AddColForElements()
        {
            if (workCurveCurrent.CalcType == CalcType.EC)
            {
            }
            else
            {
                if (!dgvwElements.Columns.Contains("ColDifference"))
                {
                    ColDifference = new DataGridViewCheckBoxColumn();//差额
                    ColDifference.Name = "ColDifference";
                    ColDifference.DataPropertyName = "DifferenceHelp";
                    ColDifference.HeaderText = Info.Difference;
                    ColDifference.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                    dgvwElements.Columns.Insert(8, ColDifference);
                }
            }
        }

        /// <summary>
        /// 初始化纯元素可选列表
        /// </summary>
        /// <returns></returns>
        private List<string> GetPureElems()
        {
            List<string> lst = new List<string>();
            string sql = "select * from PureSpecParam where DeviceName ='" + WorkCurveHelper.WorkCurveCurrent.Condition.Device.Name + "' group by ElementName";
            List<PureSpecParam> lstparam = PureSpecParam.FindBySql(sql);
            foreach (var a in lstparam)
            {
                string strsql = "select * from PureSpecParam where DeviceName ='" + WorkCurveHelper.WorkCurveCurrent.Condition.Device.Name + "' and ElementName ='" + a.ElementName + "'";
                List<PureSpecParam> pureList = PureSpecParam.FindBySql(strsql);
                if (pureList.Count >= 3)
                    lst.Add(a.ElementName);
            }
            return lst;
        }

        /// <summary>
        /// 初始化列头
        /// </summary>
        private void InitDGVColumn()
        {
            AddColForElements();
            if (DifferenceDevice.IsAnalyser)
            {
                chkShowAreaThick.Visible = false;
                cmbDensityUnits.Visible = false;
                grpNOTThick.Visible = true;
                grpThick.Visible = false;
                chkNoStandardAlert.Visible = true;
                chkShowAreaThick.Visible = false;
                cmbDensityUnits.Visible = false;
                btnCustomUnit.Visible = false;
                //this.ColContentUnit.Visible = false;
                //if (workCurveCurrent.CalcType == CalcType.FP)
                //{
                //    radRhIsAlloy.Visible = true;
                //    radRhIsLayer.Visible = true;
                //    diRhLayerFactor.Visible = false;
                //}
                if (GP.CurrentUser.Role.RoleType == 0)
                {
                    this.chkNoStandardAlert.Visible = true;
                    ColAlert.Visible = true;
                }
                else
                    this.chkNoStandardAlert.Visible = false;

            }
            else if (DifferenceDevice.IsXRF)
            {
                chkShowAreaThick.Visible = false;
                cmbDensityUnits.Visible = false;
                btnCustomUnit.Visible = false;
                grpNOTThick.Visible = true;
                grpThick.Visible = false;
                //chkRemoveBack.Visible = false;
                //chkUnToMainElem.Visible=radRhIsLayer.Visible = false;
                chkUnToMainElem.Visible = false;
                //diRhLayerFactor.Visible = false;
                chkIsReportCategory.Visible = true;
                colContentcoefficient.Visible = false;
                if (!dgvwElements.Columns.Contains("ColOxide"))
                {
                    DataGridViewComboBoxColumn dgvcomOxide = new DataGridViewComboBoxColumn();//氧化物信息
                    dgvcomOxide.Name = "ColOxide";
                    dgvcomOxide.DataPropertyName = "Oxide";
                    dgvcomOxide.HeaderText = Info.strOxide;
                    dgvcomOxide.Width = 90;
                    dgvwElements.Columns.Insert(1, dgvcomOxide);
                }
            }
            else if (DifferenceDevice.IsRohs)
            {
                chkShowAreaThick.Visible = false;
                cmbDensityUnits.Visible = false;
                btnCustomUnit.Visible = false;
                grpNOTThick.Visible = false;
                grpThick.Visible = false;
                dgvwReferences.Height = 138;
                colContentcoefficient.Visible = false;
                //dgvwElements.Height = 340;
                if (!dgvwElements.Columns.Contains("ColIntensityComparison"))
                {
                    DataGridViewCheckBoxColumn dgvchk = new DataGridViewCheckBoxColumn();//是否比较a，b系的强度
                    dgvchk.Name = "ColIntensityComparison";
                    dgvchk.DataPropertyName = "IntensityComparison";
                    dgvchk.HeaderText = Info.IsOrNotCompareIntensity;
                    dgvchk.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                    dgvwElements.Columns.Insert(4, dgvchk);
                }

                if (!dgvwElements.Columns.Contains("dgvBPeakLow"))
                {
                    DataGridViewTextBoxColumn dgvBPeakLow = new DataGridViewTextBoxColumn();
                    dgvBPeakLow.Name = "dgvBPeakLow";
                    dgvBPeakLow.DataPropertyName = "BPeakLow";//b峰左界
                    dgvBPeakLow.HeaderText = Info.BPeakLow;
                    dgvBPeakLow.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                    dgvwElements.Columns.Insert(5, dgvBPeakLow);
                }

                if (!dgvwElements.Columns.Contains("dgvBPeakHigh"))
                {
                    DataGridViewTextBoxColumn dgvBPeakHigh = new DataGridViewTextBoxColumn();
                    dgvBPeakHigh.Name = "dgvBPeakHigh";
                    dgvBPeakHigh.DataPropertyName = "BPeakHigh";//b峰右界
                    dgvBPeakHigh.HeaderText = Info.BPeakHigh;
                    dgvBPeakHigh.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                    dgvwElements.Columns.Insert(6, dgvBPeakHigh);
                }
            }
            else if (DifferenceDevice.IsThick)
            {
                grpNOTThick.Visible = false;
                grpThick.Visible = true;
                ColThicknessUnit.Visible = true;
                ColIsShowContent.Visible = true;
                colContentcoefficient.Visible = false;
                btnCustomUnit.Visible = true;
                cmbDensityUnits.Visible = true;
                chkShowAreaThick.Visible = true;
                if (WorkCurveHelper.WorkCurveCurrent.IsNiP2 )
                    colPureNameNotFilter.Visible = true;
                else
                    colPureNameNotFilter.Visible = false;
                if (workCurveCurrent.CalcType == CalcType.FP)
                {
                    chkIsAbsorb.Visible = false;


                    //grpNOTThick.Visible = true;
                   // chkUnToMainElem.Visible=radRhIsLayer.Visible = false;
                    chkUnToMainElem.Visible = false;
                    //diRhLayerFactor.Visible = false;
                   // chkShowAreaThick.Visible = true;
                    //cmbDensityUnits.Visible = true;
                    //btnCustomUnit.Visible = true;
                    //chkShowAreaThick.Checked = WorkCurveHelper.WorkCurveCurrent.IsThickShowAreaThick;
                    //RefreshComDensityUnits();
                }
                else if (workCurveCurrent.CalcType == CalcType.EC)
                {
                    //chkShowAreaThick.Visible = false;
                    //cmbDensityUnits.Visible = false;
                    //btnCustomUnit.Visible = false;
                    chkShowContent.Visible = false;
                    chkInfinite.Visible = false;
                    chkRMBG.Visible = false;
                    chkRMSum.Visible = false;
                    chkRMEscape.Visible = false;
                    chkIsAbsorb.Location = new Point(10, 20);

                    //paul 20110616  如果是EC算法，则不存在含量列

                    if (dgvwElements.Columns.Contains("ColContentUnit"))
                        dgvwElements.Columns["ColContentUnit"].Visible = false;
                }

                //if (WorkCurveHelper.isShowEncoder)
                //{
                //    ColPureElement.Visible = false;
                //    colPureElemSpec.Visible = true;
                //    this.colPureElemSpec.DataSource = GetPureElems();
                //}
                //else
                {
                    ColPureElement.Visible = true;
                    colPureElemSpec.Visible = false;
                }
                chkShowAreaThick.Checked = WorkCurveHelper.WorkCurveCurrent.IsThickShowAreaThick;
                RefreshComDensityUnits();
                
                dgvtxt = new DataGridViewTextBoxColumn();
                dgvtxt.Frozen = true;
                dgvtxt.Name = "dgvtxt";
                dgvtxt.ReadOnly = true;
                dgvtxt.DataPropertyName = "LayerNumBackUp";//所在层
                dgvtxt.HeaderText = Info.LayNum;
                dgvtxt.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                if (!dgvwElements.Columns.Contains(dgvtxt.Name))
                    dgvwElements.Columns.Insert(0, dgvtxt);
            }


            string IsRhShow = ReportTemplateHelper.LoadSpecifiedValue("TestParams", "IsShowRh");
            if (DifferenceDevice.IsAnalyser || (!string.IsNullOrEmpty(IsRhShow) && IsRhShow.Trim() == "1"))
            {
                this.chkRhThick.Visible = true;

                ElementList currElementList = ElementList.FindById((WorkCurveHelper.WorkCurveCurrent.ElementList == null) ? -1 : WorkCurveHelper.WorkCurveCurrent.ElementList.Id);
                this.chkRhThick.Checked = (WorkCurveHelper.WorkCurveCurrent.ElementList == null || currElementList == null) ? false : currElementList.RhIsLayer;
                if (workCurveCurrent.CalcType != CalcType.FP && this.chkRhThick.Checked)
                {
                    this.txtRhfactor.Visible = true;//!this.chkUnToMainElem.Checked;
                    this.txtRhfactor.Text = currElementList.RhLayerFactor.ToString();
                   // this.txtLayerElems.Visible = true;
                }
                else
                {
                    this.txtRhfactor.Visible = false;
                    this.txtRhfactor.Text = "";
                    //this.txtLayerElems.Visible = false;
                }
                this.txtLayerElems.Visible = this.chkRhThick.Checked;
            }
            else
            {
                this.chkRhThick.Visible = false;
                this.txtRhfactor.Visible = false;
                this.txtLayerElems.Visible = false;
            }


            //修改：何晓明 20111128 点击第一次应用后重新加载
            this.ColIntensityWay.DataSource = null;
            this.ColIntensityWay.Items.Clear();
            this.ColIntensityWay.DataSource = lstIntensityMethod;
            this.ColIntensityWay.DisplayMember = "Text";
            this.ColIntensityWay.ValueMember = "Key";

            this.ColBaseIntensityWay.DataSource = null;
            this.ColBaseIntensityWay.Items.Clear();
            this.ColBaseIntensityWay.DataSource = lstBaseIntensityMethod;
            this.ColBaseIntensityWay.DisplayMember = "Text";
            this.ColBaseIntensityWay.ValueMember = "Key";

            this.ColFlag.Items.Clear();
            this.ColFlag.Items.AddRange(new object[] { XLine.Ka, XLine.Kb, XLine.La, XLine.Lb });//线系 
            ColContentUnit.Items.Clear();
            ColContentUnit.Items.AddRange(new object[] { "%", "ppm", "‰" });
            ColThicknessUnit.Items.Clear();
            ColThicknessUnit.Items.AddRange(new object[] { "um", "u〞","g/L" });
            //this.ColConditionDevice.Items.Clear();
            //this.ColConditionDevice.Items.AddRange(Condition);//测试条件

            this.ColConditionDevice.DataSource = workCurveCurrent.Condition.DeviceParamList.ToList();
            this.ColConditionDevice.DisplayMember = "Name";
            this.ColConditionDevice.ValueMember = "Id";

            
        }

      

        private void InitSomeControl()
        {
            if (DifferenceDevice.IsAnalyser)
            {
                if (FrmLogon.userName != null)
                {
                     var vv = User.FindOne(w => w.Name == FrmLogon.userName);
                     if (vv.Role.RoleType == 0)
                         this.chkNoStandardAlert.Visible = true;
                     else
                         this.chkNoStandardAlert.Visible = false;
                }
            }
        }


        /// <summary>
        /// 元素列表绑定至表格
        /// </summary>
        private void ElementListToGrid()
        {
            int count = workCurveCurrent.ElementList.Items.Count;
            BindingSource bs = new BindingSource();
            for (int i = 0; i < count; i++)
            {
                bs.Add(workCurveCurrent.ElementList.Items[i]);
            }
            this.dgvwElements.AutoGenerateColumns = false;

            this.dgvwElements.DataSource = bs;

            if (workCurveCurrent.CalcType == CalcType.FP)
            {
                RefreshElements();
            }
        }

        private void ReferenceElementToGrid()
        {
            int count = dgvwElements.SelectedRows.Count;
            if (count > 0)
            {
                int index = dgvwElements.SelectedRows[0].Index;
                int refCount = workCurveCurrent.ElementList.Items[index].References.Count;
                BindingSource bs = new BindingSource();
                for (int i = 0; i < refCount; i++)
                {
                    bs.Add(workCurveCurrent.ElementList.Items[index].References[i]);
                }
                this.dgvwReferences.AutoGenerateColumns = false;

                this.dgvwReferences.DataSource = bs;
            }
        }

        /// <summary>
        /// 刷新元素列表
        /// </summary>
        private void RefreshElements()
        {
            for (int i = 0; i < workCurveCurrent.ElementList.Items.Count; i++)
            {
                if (workCurveCurrent.ElementList.Items[i].Flag == ElementFlag.Difference)
                {
                    dgvwElements[ColDifference.Name, i].Value = true;
                }
                else
                {
                    dgvwElements[ColDifference.Name, i].Value = false;
                }
            }
        }

        /// <summary>
        /// 添加感兴趣元素
        /// </summary>
        /// <param name="Caption"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private CurveElement AddElement(string Caption, int index, int line, IntensityWay IntensityWay, LayerFlag LayerFlag, int rowsIndex)
        {
            Atom atom = Atoms.AtomList.Find(x => x.AtomName == Caption);
            int AtomId = 0;
            if (atom != null)
            {
                AtomId = atom.AtomID;
            }
            int color = Color.Blue.ToArgb();
            if (useBlueColor)
            {
                //string sql = "Select Color from CurveElement where Caption = '" + Caption + "'";
                //Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
                CurveElement ce = CurveElement.FindOne(a => a.Caption == Caption);
                if (ce != null)
                    color = ce.Color;
                else
                    color = Color.Blue.ToArgb();
            }
            else
                color = WorkCurveHelper.DefaultElementColor[rowsIndex].ToArgb();
            double atomDensity = atom != null ? atom.AtomDensity : 0;
            CurveElement curve = CurveElement.New.Init(Caption, true, Caption, AtomId, index, "", (XLine)line, 0, 0, 0, 0, false, atomDensity, 0, 0, 0.00, IntensityWay, false,
                                    0, 0, 0, CalculationWay.Insert, FpCalculationWay.LinearWithoutAnIntercept, ElementFlag.Calculated, LayerFlag,
                                    ContentUnit.per, ThicknessUnit.um, "", "", "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, rowsIndex, "", false, color, " ", 0, "", false, true, false, false, 1, 0, "", "", "", false, Caption);
            curve.BaseIntensityWay = BaseIntensityWay.FullArea;
            return curve;

        }

        #endregion

        #region Events

        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (workCurveCurrent.ElementList != null)
            {
                atomnames = new string[workCurveCurrent.ElementList.Items.Count];
                atomlines = new int[workCurveCurrent.ElementList.Items.Count];
                for (int i = 0; i < atomnames.Length; i++)
                {
                    atomnames[i] = workCurveCurrent.ElementList.Items[i].Caption;
                    atomlines[i] = (int)workCurveCurrent.ElementList.Items[i].AnalyteLine;
                }

                //新增氧化物的元素信息
                if (DifferenceDevice.IsXRF && workCurveCurrent.ElementList.Items.ToList().Exists(delegate(CurveElement v) { return v.IsOxide == true; }))
                    XRFOxideElement();
            }
            FrmElementTable frm = new FrmElementTable(atomnames, atomlines, workCurveCurrent.ElementList, false, false);
            WorkCurveHelper.OpenUC(frm, false, Info.SelectElement,true);
            if (DialogResult.OK == frm.DialogResult)
            {
                if (DifferenceDevice.IsThick)//测厚
                {
                    ElementForThick(frm.lstName);//测厚添加元素
                }
                else
                {
                    XRFCurveElement(frm.names, frm.lines);//感兴趣元素
                    //if (DifferenceDevice.IsXRF) XRFOxideCurveElement(frm.lstOxideName);
                    ElementListToGrid();//绑定至界面
                }
               
                int count = workCurveCurrent.ElementList.Items.Count;
                if (count > 0)
                {
                    this.dgvwElements.Rows[0].Selected = false;
                    this.dgvwElements.Rows[count - 1].Selected = true;
                }
            }
        }

        /// <summary>
        /// 设置默认元素边界
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private CurveElement SetDefaultPeak(CurveElement element)
        {
            double energy = 0;
            Atom atom = Atoms.AtomList.Find(a => a.AtomName == element.Caption);
            int Id = 0;
            if (atom != null)
            {
                Id = atom.AtomID;
                if (Id <= 56)
                {
                    energy = Atoms.AtomList.Find(a => a.AtomName == element.Caption).Ka;
                }
                else if (Id == 79 || Id == 73 || Id == 77 || Id == 78 || Id == 82)  //Au, Ta,Ir,Pt,Pb默认LB
                {
                    energy = Atoms.AtomList.Find(a => a.AtomName == element.Caption).Lb;
                    element.AnalyteLine = XLine.Lb;
                }
                else
                {
                    energy = Atoms.AtomList.Find(a => a.AtomName == element.Caption).La;
                    element.AnalyteLine = XLine.La;
                }
                int channel = DemarcateEnergyHelp.GetChannel(energy);
                int specLength = (int)workCurveCurrent.Condition.Device.SpecLength;
                element.PeakLow = channel - DifferenceDevice.PeakLowHightSpacing < 0 ? 0 : channel - DifferenceDevice.PeakLowHightSpacing;
                element.PeakLow = element.PeakLow > specLength ? specLength - 1 : element.PeakLow;
                element.PeakHigh = channel + DifferenceDevice.PeakLowHightSpacing > specLength ? specLength - 1 : channel + DifferenceDevice.PeakLowHightSpacing;
                element.PeakHigh = element.PeakHigh < 0 ? 0 : element.PeakHigh;
                if (workCurveCurrent.Condition.DeviceParamList != null && workCurveCurrent.Condition.DeviceParamList.Count > 0)
                {
                    DeviceParameter dp = workCurveCurrent.Condition.DeviceParamList[0];
                    element.BaseLow = dp.BeginChann;
                    element.BaseHigh = dp.EndChann;
                }
                if (element.PeakLow > element.PeakHigh) element.PeakLow = element.PeakHigh;
                if (element.LayerNumber == 0)
                {
                    element.IsShowElement = false;
                }
            }
            else
            {
            }
            return element;
        }

        /// <summary>
        /// AddNewElementForThick
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        private int AddNewElementForThick(string[][] arr)
        {
            int layer = 0;
            for (int i = 0; i < DifferenceDevice.MAX_LAYER_NUMBER_INT; i++)
            {
                if (arr[i] != null)
                {
                    for (int j = 0; j < arr[i].Length; j++)
                    {
                        var element = AddElement(arr[i][j], i, 0, IntensityWay.Reference, LayerFlag.Calculated, i);
                        element.DevParamId = workCurveCurrent.Condition.DeviceParamList[0].Id;
                        element = SetDefaultPeak(element);
                        workCurveCurrent.ElementList.Items.Add(element);//测厚添加新元素
                    }
                    if (i == DifferenceDevice.MAX_LAYER_NUMBER_INT - 1)
                    {
                        layer = DifferenceDevice.MAX_LAYER_NUMBER_INT;
                        break;
                    }
                }
                else
                {
                    layer = i;
                    break;
                }
            }
            return layer;
        }

        /// <summary>
        /// UpdateElementSubstrate
        /// </summary>
        /// <param name="arr"></param>
        private void UpdateElementSubstrate(string[][] arr)
        {
            bool hasElement = false;
            //获取基材元素
            CurveElement ele = workCurveCurrent.ElementList.Items.ToList().OrderByDescending(element => element.LayerNumber).ToList<CurveElement>()[0];
            List<CurveElement> lce = workCurveCurrent.ElementList.Items.ToList().FindAll(el => el.LayerNumber == ele.LayerNumber);
            for (int j = 0; j < arr[0].Length; j++)
            {
                hasElement = false;
                for (int k = 0; k < lce.Count; k++)
                {
                    if (lce[k].Caption.Equals(arr[0][j]))
                    {
                        lce[k].LayerNumber = 0;
                        hasElement = true;
                        break;
                    }
                }
                if (!hasElement && lce.Count > 0)
                {
                    var element = AddElement(arr[0][j], 0, 0, IntensityWay.Reference, LayerFlag.Calculated, 0);
                    element.DevParamId = workCurveCurrent.Condition.DeviceParamList[0].Id;
                    element = SetDefaultPeak(element);
                    workCurveCurrent.ElementList.Items.Add(element);
                }
            }
            for (int m = lce.Count - 1; m >= 0; m--)//遍历增加后的元素列表
            {
                hasElement = false;
                int index = workCurveCurrent.ElementList.Items.ToList().FindIndex(a => a.Caption == lce[m].Caption && a.LayerNumber == lce[m].LayerNumber);
                workCurveCurrent.ElementList.Items[index].LayerNumber = 0;
                for (int j = 0; j < arr[0].Length; j++)
                {
                    if (arr[0][j].Equals(lce[m].Caption))
                    {
                        hasElement = true;
                        break;
                    }
                }
                if (!hasElement)
                {
                    if (lce[m].Id != 0)
                    {
                        lstReadyDel.Add(lce[m]);
                    }
                    workCurveCurrent.ElementList.Items.RemoveAt(index);
                }
            }
        }

        private void IfDeleteLayer(string[][] arr, int i)
        {
            List<CurveElement> lc = workCurveCurrent.ElementList.Items.ToList().FindAll(w => w.LayerNumber == i);
            if (arr[i] == null && lc.Count > 0)
            {
                for (int m = lc.Count - 1; m >= 0; m--)
                {
                    int index = workCurveCurrent.ElementList.Items.ToList().FindIndex(a => a.Caption == lc[m].Caption && a.LayerNumber == lc[m].LayerNumber);
                    workCurveCurrent.ElementList.Items.RemoveAt(index);
                }
            }
        }

        /// <summary>
        /// 更新层
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private int UpdateElementLayer(string[][] arr, int i)
        {
            bool hasElement = false;
            int layer = 0;
            List<CurveElement> lc = workCurveCurrent.ElementList.Items.ToList().FindAll(w => w.LayerNumber == i);
            for (int j = 0; j < arr[i].Length; j++)
            {
                hasElement = false;
                for (int k = 0; k < lc.Count; k++)
                {
                    if (lc[k].Caption.Equals(arr[i][j]))
                    {
                        hasElement = true;
                        break;
                    }
                }
                if (!hasElement)
                {
                    var element = AddElement(arr[i][j], i, 0, IntensityWay.Reference, LayerFlag.Calculated, i);
                    element.DevParamId = workCurveCurrent.Condition.DeviceParamList[0].Id;
                    element = SetDefaultPeak(element);
                    workCurveCurrent.ElementList.Items.Add(element);
                }
            }
            for (int m = lc.Count - 1; m >= 0; m--)//遍历增加后的元素列表
            {
                hasElement = false;
                int index = workCurveCurrent.ElementList.Items.ToList().FindIndex(a => a.Caption == lc[m].Caption && a.LayerNumber == lc[m].LayerNumber);
                for (int j = 0; j < arr[i].Length; j++)
                {
                    if (arr[i][j].Equals(lc[m].Caption))
                    {
                        hasElement = true;
                        break;
                    }
                }
                if (!hasElement)
                {
                    if (lc[m].Id != 0)
                    {
                        lstReadyDel.Add(lc[m]);
                    }
                    workCurveCurrent.ElementList.Items.RemoveAt(index);
                }
            }
            if (i == DifferenceDevice.MAX_LAYER_NUMBER_INT - 1)
            {
                layer = DifferenceDevice.MAX_LAYER_NUMBER_INT;
            }
            return layer;
        }

        /// <summary>
        /// 测厚增加或修改元素
        /// </summary>
        /// <param name="arr"></param>
        private void ElementForThick(string[][] arr)
        {
            int layer = 0;
            if (workCurveCurrent.ElementList.Items.Count == 0)//新添元素
            {
                layer = AddNewElementForThick(arr);
            }
            else//修改元素
            {
                //bool hasElement = false;
                UpdateElementSubstrate(arr);
                for (int i = 1; i < DifferenceDevice.MAX_LAYER_NUMBER_INT; i++)
                {
                    if (arr[i] != null)
                    {
                        layer = UpdateElementLayer(arr, i);
                    }
                    else
                    {
                        layer = i;
                        break;
                    }
                }
                for (int i = 1; i < DifferenceDevice.MAX_LAYER_NUMBER_INT; i++)
                {
                    IfDeleteLayer(arr, i);
                }
            }
            ElementListToGrid();//绑定至界面
            if (dgvtxt != null)//层数列
            {
                string[] name = new string[] { Info.Substrate, Info.FirstLayer, Info.SecondLayer, Info.ThirdLayer, Info.ForthLayer, Info.FifthLayer };
                //string[] name = DifferenceDevice.LayerName;
                for (int i = 0; i < workCurveCurrent.ElementList.Items.Count; i++)
                {
                    int num = workCurveCurrent.ElementList.Items[i].LayerNumber;
                    dgvwElements[dgvtxt.Name, i].Value = name[num];
                    if (num == 0)
                    {
                        workCurveCurrent.ElementList.Items[i].LayerNumber = layer;
                        workCurveCurrent.ElementList.Items[i].LayerFlag = LayerFlag.Fixed;
                    }
                }
            }
            if (workCurveCurrent.ElementList.Items.Count == 0) return;
            ////获取基材元素
            //CurveElement ele = workCurveCurrent.ElementList.Items.ToList().OrderByDescending(element => element.LayerNumber).ToList<CurveElement>()[0];
            //ele.LayerFlag = LayerFlag.Fixed;
        }

        /// <summary>
        /// XRF增加或修改元素
        /// </summary>
        /// <param name="names"></param>
        /// <param name="lines"></param>
        private void XRFCurveElement(string[] names, int[] lines)
        {
            if (names.Length > ConstParam.MAXELT)
            {
                Msg.Show(Info.MAXELT);
                return;
            }
            atomnames = names;

            atomlines = lines;
            //bool hasElement = false;
            if (workCurveCurrent.ElementList.Items.Count == 0)//新添元素
            {
                for (int i = 0; i < atomnames.Length; i++)
                {
                    int line = atomlines[i] == 0 ? 0 : 2;
                    var element = AddElement(atomnames[i], 0, line, IntensityWay.FullArea, LayerFlag.Fixed, i);
                    if (workCurveCurrent.CalcType == CalcType.FP)
                        element = AddElement(atomnames[i], 0, line, IntensityWay.FPGauss, LayerFlag.Fixed, i);
                    element.DevParamId = workCurveCurrent.Condition.DeviceParamList[0].Id;
                    element = SetDefaultPeak(element);
                    workCurveCurrent.ElementList.Items.Add(element);
                }
            }
            else//修改元素
            {
                for (int i = 0; i < atomnames.Length; i++)//遍历新数组
                {

                    if (workCurveCurrent.ElementList.Items.ToList().Exists(w => w.Caption.Equals(atomnames[i]))) continue;
                    int line = atomlines[i] == 0 ? 0 : 2;
                    var element = AddElement(atomnames[i], 0, line, IntensityWay.FullArea, LayerFlag.Fixed, i);
                    if (workCurveCurrent.CalcType == CalcType.FP)
                        element = AddElement(atomnames[i], 0, line, IntensityWay.FPGauss, LayerFlag.Fixed, i);
                    element.DevParamId = workCurveCurrent.Condition.DeviceParamList[0].Id;
                    element = SetDefaultPeak(element);
                    element.ConditionID = 0;
                    workCurveCurrent.ElementList.Items.Add(element);
                    if (workCurveCurrent.ElementList.Items.Count > 0 && workCurveCurrent.ElementList.Items[0].Samples.Count > 0)
                    {
                        //AddStandSample(element);
                        foreach (var sample in workCurveCurrent.ElementList.Items[0].Samples)
                        {
                            SpecListEntity sl = WorkCurveHelper.DataAccess.Query(sample.SampleName);
                            StandSample sam = null;
                            if (sl == null)
                            {
                                double dblDensity = Atoms.AtomList.Find(w => w.AtomName.ToUpper().Equals(sample.ElementName.ToUpper())).AtomDensity;
                                sam = StandSample.New.Init(sample.SampleName, sample.Height.ToString(),sample.CalcAngleHeight, "0", "0", "0", true, sample.ElementName, 0, 0, "", dblDensity, "0");
                            }
                            else
                            {
                                try
                                {
                                    workCurveCurrent.CaculateIntensity(sl);
                                }
                                catch (Exception ex)
                                {
                                    Msg.Show(sl.SampleName + ex.Message);
                                    continue;
                                }
                                double dblDensity = Atoms.AtomList.Find(w => w.AtomName.ToUpper().Equals(sample.ElementName.ToUpper())).AtomDensity;
                                sam = StandSample.New.Init(sl.Name, sl.Height.ToString(), sl.CalcAngleHeight.ToString(), element.Intensity.ToString(), "0", "0", true, element.Caption, 0, 0, "", dblDensity, "0");
                            }
                            element.Samples.Add(sam);
                            sam.Save();
                        }
                    }
                }
                for (int i = workCurveCurrent.ElementList.Items.Count - 1; i >= 0; i--)//遍历增加后的元素列表
                {
                    if (atomnames.ToList().Exists(w => w.Equals(workCurveCurrent.ElementList.Items[i].Caption))) continue;
                    if (workCurveCurrent.ElementList.Items[i].Id != 0)
                    {
                        lstReadyDel.Add(workCurveCurrent.ElementList.Items[i]);
                    }
                    workCurveCurrent.ElementList.Items.RemoveAt(i);
                }
                OrderByRowIndex();

                #region 备份
                //for (int i = 0; i < atomnames.Length; i++)//遍历新数组
                //{
                //    hasElement = false;
                //    for (int j = 0; j < workCurveCurrent.ElementList.Items.Count; j++)
                //    {
                //        if (workCurveCurrent.ElementList.Items[j].Caption.Equals(atomnames[i]))
                //        {
                //            hasElement = true;
                //            break;
                //        }
                //    }
                //    if (!hasElement)
                //    {
                //        int line = atomlines[i] == 0 ? 0 : 2;
                //        var element = AddElement(atomnames[i], 0, line, IntensityWay.FullArea, LayerFlag.Fixed, i);
                //        if (workCurveCurrent.CalcType == CalcType.FP)
                //            element = AddElement(atomnames[i], 0, line, IntensityWay.FPGauss, LayerFlag.Fixed, i);
                //        element.DevParamId = workCurveCurrent.Condition.DeviceParamList[0].Id;
                //        element = SetDefaultPeak(element);
                //        element.ConditionID = 0;
                //        workCurveCurrent.ElementList.Items.Add(element);
                //        if (workCurveCurrent.ElementList.Items.Count > 0 && workCurveCurrent.ElementList.Items[0].Samples.Count > 0)
                //        {
                //            foreach (var sample in workCurveCurrent.ElementList.Items[0].Samples)
                //            {
                //                SpecListEntity sl = WorkCurveHelper.DataAccess.Query(sample.SampleName);
                //                StandSample sam = null;
                //                if (sl == null)
                //                {
                //                    sam = StandSample.New.Init(sample.SampleName, "0", "0", "0", true, sample.ElementName, 0, 0, "");
                //                }
                //                else
                //                {
                //                    try
                //                    {
                //                        workCurveCurrent.CaculateIntensity(sl);
                //                    }
                //                    catch (Exception ex)
                //                    {
                //                        Msg.Show(sl.SampleName + ex.Message);
                //                        continue;
                //                    }
                //                    sam = StandSample.New.Init(sl.Name, element.Intensity.ToString(), "0", "0", true, element.Caption, 0, 0, "");
                //                }
                //                element.Samples.Add(sam);
                //                sam.Save();
                //            }
                //        }
                //    }
                //}
                //for (int i = workCurveCurrent.ElementList.Items.Count - 1; i >= 0; i--)//遍历增加后的元素列表
                //{
                //    hasElement = false;
                //    for (int j = 0; j < atomnames.Length; j++)
                //    {
                //        if (atomnames[j].Equals(workCurveCurrent.ElementList.Items[i].Caption))
                //        {
                //            hasElement = true;
                //            break;
                //        }
                //    }
                //    if (!hasElement)
                //    {
                //        if (workCurveCurrent.ElementList.Items[i].Id != 0)
                //        {
                //            lstReadyDel.Add(workCurveCurrent.ElementList.Items[i]);
                //        }
                //        workCurveCurrent.ElementList.Items.RemoveAt(i);
                //    }
                //}
                //OrderByRowIndex();

                #endregion
            }
        }

        #region 异步处理
        void Event1(CurveElement element)
        {
            foreach (var sample in workCurveCurrent.ElementList.Items[0].Samples)
            {
                SpecListEntity sl = WorkCurveHelper.DataAccess.Query(sample.SampleName);
                StandSample sam = null;
                if (sl == null)
                {
                    double dblDensity = Atoms.AtomList.Find(w => w.AtomName.ToUpper().Equals(sample.ElementName.ToUpper())).AtomDensity;
                    sam = StandSample.New.Init(sample.SampleName, sample.Height.ToString(), sample.CalcAngleHeight.ToString(), "0", "0", "0", true, sample.ElementName, 0, 0, "", dblDensity, "0");
                }
                else
                {
                    try
                    {
                        workCurveCurrent.CaculateIntensity(sl);
                    }
                    catch (Exception ex)
                    {
                        Msg.Show(sl.SampleName + ex.Message);
                        continue;
                    }
                    Console.WriteLine(element.Caption + " " + element.Samples.Count.ToString());
                    double dblDensity = Atoms.AtomList.Find(w => w.AtomName.ToUpper().Equals(sample.ElementName.ToUpper())).AtomDensity;
                    sam = StandSample.New.Init(sl.Name, sl.Height.ToString(), sl.CalcAngleHeight.ToString(), element.Intensity.ToString(), "0", "0", true, element.Caption, 0, 0, "", dblDensity, "0");
                }
                element.Samples.Add(sam);
                sam.Save();
            }
        }



        private void AddStandSample(CurveElement element)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                Event1(element);
            });

        }



        #endregion

        private void OrderByRowIndex()
        {
            //修改：何晓明 20111130 感兴趣元素重新排序
            for (int i = 0; i < workCurveCurrent.ElementList.Items.Count; i++)
            {
                workCurveCurrent.ElementList.Items[i].RowsIndex = i;
            }
            //
        }

        #region 氧化物

        /// <summary>
        /// XRF增加或修改氧化物
        /// </summary>
        /// <param name="lstOxideName"></param>
        private void XRFOxideCurveElement(string[] lstOxideName)
        {
            List<string> lstONames = (lstOxideName == null) ? new List<string>() : lstOxideName.ToList();
            //删除以有的氧化物
            List<CurveElement> cElementList = workCurveCurrent.ElementList.Items.ToList().FindAll(delegate(CurveElement v) { return v.IsOxide == true; });
            if (cElementList.Count > 0)
            {
                foreach (CurveElement cElement in cElementList)
                {
                    if (!lstONames.Exists(delegate(string v) { return v == cElement.Formula; }))//删除已有的氧化物
                    {
                        lstReadyDel.Add(cElement);
                        workCurveCurrent.ElementList.Items.Remove(cElement);
                    }
                }
            }

            //新增氧化物
            int i = -1;
            foreach (string sOxideName in lstONames)//遍历新数组
            {
                if (!workCurveCurrent.ElementList.Items.ToList().Exists(delegate(CurveElement v) { return v.IsOxide == true && v.Formula == sOxideName; }))
                {
                    i++;
                    int line = 0;
                    string sEleName = System.Text.RegularExpressions.Regex.Replace(sOxideName, "[0-9]", "").Replace("O", "");
                    var element = AddElement(sEleName, 0, line, IntensityWay.FullArea, LayerFlag.Fixed, i);
                    if (workCurveCurrent.CalcType == CalcType.FP)
                        element = AddElement(sEleName, 0, line, IntensityWay.FPGauss, LayerFlag.Fixed, i);
                    element.DevParamId = workCurveCurrent.Condition.DeviceParamList[0].Id;
                    element = SetDefaultPeak(element);
                    element.IsOxide = true;
                    element.Formula = sOxideName;
                    workCurveCurrent.ElementList.Items.Add(element);
                }

            }
        }

        private void XRFOxideElement()
        {
            List<CurveElement> cElementList = workCurveCurrent.ElementList.Items.ToList().FindAll(delegate(CurveElement v) { return v.IsOxide == true; });
            if (cElementList.Count > 0)
            {
                List<string> lstNames = atomnames.ToList();
                List<int> lstLines = atomlines.ToList();

                foreach (CurveElement cElement in cElementList)
                {
                    string sOxideName = System.Text.RegularExpressions.Regex.Replace(cElement.Formula, "[0-9]", "").Replace("O", "");
                    if (!lstNames.Exists(delegate(string v) { return v == sOxideName; }))
                    {
                        lstNames.Add(sOxideName);
                        lstLines.Add(0);
                    }

                    int index = lstNames.FindIndex(delegate(string v) { return v == cElement.Formula; });
                    if (index == -1) continue;
                    lstNames.RemoveAt(index);
                    lstLines.RemoveAt(index);

                }

                atomnames = lstNames.ToArray<string>();
                atomlines = lstLines.ToArray<int>();
            }
        }

        #endregion

        /// <summary>
        /// 删除元素
        /// </summary>
        private void btnDel_Click(object sender, EventArgs e)
        {
            int selectCount = dgvwElements.SelectedRows.Count;
            if (selectCount <= 0)
            {
                return;
            }

            int index = dgvwElements.SelectedRows[0].Index;
            if (workCurveCurrent.ElementList.Items[index].Id != 0)
            {
                lstReadyDel.Add(workCurveCurrent.ElementList.Items[index]);
            }
            int layNumber = workCurveCurrent.ElementList.Items[index].LayerNumber;
            workCurveCurrent.ElementList.Items.RemoveAt(index);
            if (DifferenceDevice.IsThick && workCurveCurrent.ElementList.Items.Count > 0)//厚度层数渐变
            {
                if (workCurveCurrent.ElementList.Items.ToList().Find(w => w.LayerNumber == layNumber) == null)
                {
                    string[] name = new string[] { Info.Substrate, Info.FirstLayer, Info.SecondLayer, Info.ThirdLayer, Info.ForthLayer, Info.FifthLayer };
                    foreach (var it in workCurveCurrent.ElementList.Items)
                    {
                        if (it.LayerNumber > layNumber)
                        {
                            it.LayerNumber -= 1;

                            it.LayerNumBackUp = name[it.LayerNumber];
                        }
                    }
                    CurveElement ele = workCurveCurrent.ElementList.Items.ToList().OrderByDescending(element => element.LayerNumber).ToList<CurveElement>()[0];
                    int lay = ele.LayerNumber;
                    foreach (var it in workCurveCurrent.ElementList.Items)
                    {
                        if (it.LayerNumber == lay)
                        {
                            it.LayerFlag = LayerFlag.Fixed;
                            it.LayerNumBackUp = name[0];
                        }
                    }
                }
            }
            ElementListToGrid();//绑定至界面
            int count = workCurveCurrent.ElementList.Items.Count;
            if (count <= 0)//删掉最后一个
            {
                this.btnAdd.Enabled = true;
            }
            else
            {
                if (index >= count)
                {
                    this.dgvwElements.Rows[0].Selected = false;
                    this.dgvwElements.Rows[count - 1].Selected = true;
                }
                else
                {
                    this.dgvwElements.Rows[0].Selected = false;
                    this.dgvwElements.Rows[index].Selected = true;
                }
            }
            //List<HistoryRecord> listRecord = HistoryRecord.Find(w => w.WorkCuveName == WorkCurveHelper.WorkCurveCurrent.Name);

        }

        /// <summary>
        /// 上移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            if (dgvwElements.SelectedRows.Count < 1)
            {
                return;
            }
            var element = workCurveCurrent.ElementList.Items[dgvwElements.SelectedRows[0].Index];
            int index = 0;
            //int temp;
            CurveElement tempElement;
            for (int i = 1; i < workCurveCurrent.ElementList.Items.Count; i++)
            {
                if (element.Caption == workCurveCurrent.ElementList.Items[i].Caption)
                {
                    index = i;
                    //temp = workCurveCurrent.ElementList.Items[i - 1].RowsIndex;
                    //workCurveCurrent.ElementList.Items[i - 1].RowsIndex = workCurveCurrent.ElementList.Items[i].RowsIndex;
                    //workCurveCurrent.ElementList.Items[i].RowsIndex = temp;

                    tempElement = workCurveCurrent.ElementList.Items[i - 1];
                    workCurveCurrent.ElementList.Items[i - 1] = workCurveCurrent.ElementList.Items[i];
                    workCurveCurrent.ElementList.Items[i] = tempElement;
                    ElementListToGrid();//绑定至界面
                    dgvwElements.Rows[0].Selected = false;
                    dgvwElements.Rows[i - 1].Selected = true;
                    dgvwElements.CurrentCell = dgvwElements.Rows[i - 1].Cells[0];
                    break;
                }
            }
            dgvwElements.FirstDisplayedScrollingRowIndex = (index == 0) ? index : index - 1;


        }

        /// <summary>
        /// 验证是否有纯元素谱
        /// </summary>
        /// <param name="element"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        private bool FindSpectrum(string[] element, HasMany<CurveElement> items , bool isValidataPureLib)
        {
            bool returnValue = true;
            foreach (string ele in element)
            {
                var elem = items.ToList().Find(i => i.Caption == ele);
                if (isValidataPureLib)   //验证纯元素谱库
                {
                    string sql = "select * from PureSpecParam where DeviceName ='" + WorkCurveHelper.WorkCurveCurrent.Condition.Device.Name + "' and ElementName ='" + elem.Caption + "'";
                    List<PureSpecParam> pureList = PureSpecParam.FindBySql(sql);
                    if (pureList == null || pureList.Count < 3)
                    {
                        returnValue = false;
                        SkyrayMsgBox.Show(ele + Info.PureSpecLibNoData);
                        //SkyrayMsgBox.Show(Info.NeedSpecData);
                        break;
                    }

                }
                if (elem != null)
                {
                    string SpectrumData = elem.SSpectrumData;
                    if (SpectrumData == null || SpectrumData == "")
                    {
                        returnValue = false;
                        SkyrayMsgBox.Show(Info.fitElements + ele + Info.NeedSpecs);
                        //SkyrayMsgBox.Show(Info.NeedSpecData);
                        break;
                    }
                }
                

            }
            return returnValue;
        }

        private void SaveRhParamsValue()
        {
            if (WorkCurveHelper.WorkCurveCurrent != null && WorkCurveHelper.WorkCurveCurrent.ElementList != null)
            {
                //string sRhIsMainElementInfluence = ReportTemplateHelper.LoadSpecifiedValue("TestParams", "RhlayerOnlyMainElementInfluence");
                WorkCurveHelper.WorkCurveCurrent.ElementList.RhIsMainElementInfluence = workCurveCurrent.ElementList.RhIsMainElementInfluence;
                WorkCurveHelper.WorkCurveCurrent.ElementList.RhIsLayer = (this.chkRhThick.Visible && this.chkRhThick.Checked) ? true : false;
                WorkCurveHelper.WorkCurveCurrent.ElementList.RhLayerFactor = (WorkCurveHelper.WorkCurveCurrent.ElementList.RhIsLayer) ? double.Parse((this.txtRhfactor.Text == "") ? "0" : this.txtRhfactor.Text) : 0;
                WorkCurveHelper.WorkCurveCurrent.ElementList.RhLayerFactor = WorkCurveHelper.WorkCurveCurrent.ElementList.RhIsMainElementInfluence ? 1 : WorkCurveHelper.WorkCurveCurrent.ElementList.RhLayerFactor;
                int Islayer = this.chkRhThick.Checked ? 1 : 0;
                string sql = "Update ElementList Set RhLayerFactor = " + WorkCurveHelper.WorkCurveCurrent.ElementList.RhLayerFactor + ", RhIsLayer = "
                       + Islayer + ",RhIsMainElementInfluence=" + ((WorkCurveHelper.WorkCurveCurrent.ElementList.RhIsMainElementInfluence) ? "1" : "0") + "  Where Id = " + WorkCurveHelper.WorkCurveCurrent.ElementList.Id;
                Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);

            }
        }

        private bool SaveElements()
        {
            workCurveCurrent.ElementList.MainElementToCalcKarat = cbxWMainElement.Text;
            var lstItems = workCurveCurrent.ElementList.Items;
            List<DeviceParameter> lstParam = new List<DeviceParameter>();
            CurveElement item = null;
            for (int i = 0; i < lstItems.Count; i++)
            {
                item = lstItems[i];
                if (item.IntensityWay == IntensityWay.Reference || item.IntensityWay == IntensityWay.FixedReference)
                {
                    if ("".Equals(item.SReferenceElements))
                    {
                        SkyrayMsgBox.Show(Info.NeedReference);
                        return false;
                    }
                    else if (!FindSpectrum(item.ReferenceElements, lstItems, WorkCurveHelper.isShowEncoder))
                    {
                            return false;
                    } 
                    //else if (!WorkCurveHelper.isShowEncoder)
                    //{
                    //    if (!FindSpectrum(item.ReferenceElements, lstItems))
                    //        return false;
                    //} 
                }
                if (workCurveCurrent.FuncType == FuncType.Thick && (item.IntensityWay == IntensityWay.Reference || item.IntensityWay == IntensityWay.FixedReference))
                {
                    string SpectrumData = item.SSpectrumData;
                    if (SpectrumData == null || SpectrumData == "" || item.ElementSpecName == null || item.ElementSpecName == "")
                    {
                        SkyrayMsgBox.Show(item.Caption + Info.NeedSpecs);
                        return false;
                    }
                }

                if (item.PeakLow > item.PeakHigh || item.BaseLow > item.BaseHigh || item.BPeakLow > item.BPeakHigh)
                {
                    Msg.Show(item.Caption + Info.DataError);
                    return false;
                }
                DeviceParameter tempDp = workCurveCurrent.Condition.DeviceParamList.ToList().Find(delegate(DeviceParameter w)
                { return w.Id == long.Parse(dgvwElements[ColConditionDevice.Name, i].Value.ToString()); });
                if (tempDp != null)
                    item.DevParamId = tempDp.Id;
                if (useBlueColor)
                {
                    string sqlUp = "update CurveElement set Color =" + item.Color + " where Caption = '" + item.Caption + "'";
                    Lephone.Data.DbEntry.Context.ExecuteNonQuery(sqlUp);
                }
            }

            for (int i = 0; i < workCurveCurrent.Condition.DeviceParamList.Count; i++)
            {
                List<CurveElement> tempElementList = lstItems.ToList().FindAll(delegate(CurveElement vv) { return vv.DevParamId == workCurveCurrent.Condition.DeviceParamList[i].Id; });
                if (tempElementList != null && tempElementList.Count > 0)
                {
                    tempElementList.ForEach(wc => { wc.ConditionID = i; });
                }
            }

            //氧化物
            if (DifferenceDevice.IsXRF)
            {
                foreach (DataGridViewRow row in dgvwElements.Rows)
                {
                    string sEleNanme = row.Cells["ColElementCaption"].Value.ToString();

                    CurveElement curveElement = workCurveCurrent.ElementList.Items.ToList().Find(delegate(CurveElement v) { return v.Caption == row.Cells["ColElementCaption"].Value.ToString(); });
                    DataGridViewComboBoxCell dgvcom = row.Cells["ColOxide"] as DataGridViewComboBoxCell;
                    if (dgvcom.Value != null)
                    {
                        curveElement.IsOxide = true;
                        curveElement.Formula = dgvcom.Value.ToString();
                    }
                    else
                    {
                        curveElement.IsOxide = false;
                        curveElement.Formula = sEleNanme;
                    }

                }
            }

            //foreach (DeviceParameter dp in workCurveCurrent.Condition.DeviceParamList)
            //{
            //    List<CurveElement> tempElementList = lstItems.ToList().FindAll(delegate(CurveElement vv) { return vv.DevParamId == dp.Id; });
            //    if (tempElementList != null && tempElementList.Count > 0)
            //    {
            //        if (!lstParam.Contains(dp))
            //            lstParam.Add(dp);
            //        tempElementList.ForEach(wc => { wc.ConditionID = lstParam.IndexOf(dp); });
            //    }
            //}
            OrderByRowIndex();
            var id = workCurveCurrent.ElementList.Id;
            workCurveCurrent.ElementList.Save();
            Lephone.Data.DbEntry.Context.FastSave(id,
                //new Lephone.Data.SqlEntry.DataProvider.LineInfo<CurveElement>
                //{
                //    Objs = workCurveCurrent.ElementList.Items
                //},
                 new Lephone.Data.SqlEntry.DataProvider.LineInfo<CurveElement>
                 {
                     IsToDelete = true,
                     Objs = this.lstReadyDel
                 });
            lstReadyDel.Clear();



            SaveRhParamsValue();
            int isUnitary = workCurveCurrent.ElementList.IsUnitary ? 1 : 0;
            int isRemoveBack = workCurveCurrent.ElementList.IsRemoveBk ? 1 : 0;
            //int RhIsLayer = workCurveCurrent.ElementList.RhIsLayer ? 1 : 0;
            //int PureAsInfinite = workCurveCurrent.ElementList.PureAsInfinite ? 1 : 0;
            //int isReportCategory = workCurveCurrent.ElementList.IsReportCategory ? 1 : 0;
            //string sql = "Update ElementList Set IsUnitary = " + isUnitary + ", UnitaryValue = " + workCurveCurrent.ElementList.UnitaryValue + ", IsRemoveBk =" + isRemoveBack +
            //    ", SpecListId=" + workCurveCurrent.ElementList.SpecListId + ", RhIsLayer =" + RhIsLayer + ", RhLayerFactor=" + workCurveCurrent.ElementList.RhLayerFactor +
            //    ", PureAsInfinite =" + PureAsInfinite + ",IsReportCategory =" + isReportCategory + " Where Id = " + workCurveCurrent.ElementList.Id;
            //Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
            int IsAbsorb = workCurveCurrent.IsAbsorb ? 1 : 0;
            int IsThickShowAreaThick = chkShowAreaThick.Checked ? 1 : 0;
            workCurveCurrent.AreaThickType = (cmbDensityUnits.Visible && cmbDensityUnits.SelectedItem != null) ? cmbDensityUnits.SelectedItem.ToString() : Info.strAreaDensityUnit;
            int RemoveBg = workCurveCurrent.RemoveBackGround ? 1 : 0;
            int RemoveSum = workCurveCurrent.RemoveSum ? 1 : 0;
            int RemoveEscape = workCurveCurrent.RemoveEscape ? 1 : 0;
            string sql = "Update WorkCurve Set IsAbsorb = " + IsAbsorb + ", RemoveBackGround =" + RemoveBg +
                ", RemoveSum=" + RemoveSum + ", RemoveEscape=" + RemoveEscape + ", IsThickShowAreaThick=" + IsThickShowAreaThick +
                 ", AreaThickType='" + workCurveCurrent.AreaThickType + "'" + " Where Id = " + workCurveCurrent.Id;
            Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
            // add by chuyaqin 2011-04-24 修改了元素曲线之后 应该删除所有强度校正数据，且重新更新
            if (workCurveCurrent.FuncType == FuncType.XRF)
            {
                id = workCurveCurrent.Id;
                List<IntensityCalibration> lst = workCurveCurrent.IntensityCalibration.ToList();
                workCurveCurrent.IntensityCalibration.Clear();
                //for (int i = 0; i < workCurveCurrent.ElementList.Items.Count; i++)
                //{
                //    IntensityCalibration ic = IntensityCalibration.New;
                //    ic.Element = workCurveCurrent.ElementList.Items[i].Caption;
                //    ic.OriginalIn = 0;
                //    ic.CalibrateIn = 0;
                //    workCurveCurrent.IntensityCalibration.Add(ic);
                //}
                Lephone.Data.DbEntry.Context.FastSave(id,
                    new Lephone.Data.SqlEntry.DataProvider.LineInfo<IntensityCalibration>
                    {
                        Objs = workCurveCurrent.IntensityCalibration
                    },
                     new Lephone.Data.SqlEntry.DataProvider.LineInfo<IntensityCalibration>
                     {
                         IsToDelete = true,
                         Objs = lst
                     });
            }
            WorkCurveHelper.WorkCurveCurrent = WorkCurve.FindById(workCurveCurrent.Id);
            workCurveCurrent = WorkCurveHelper.WorkCurveCurrent;
        
            BindHelper.BindCheckedToCtrl(chkShowContent, workCurveCurrent, "IsThickShowContent");
            //
            ElementListToGrid();
            return true;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtRhfactor.Visible == true && chkRhThick.Checked && chkRhThick.Visible == true&&workCurveCurrent.CalcType==CalcType.EC && (txtRhfactor.Text == "" || double.Parse(txtRhfactor.Text) > 1))
            {
                Msg.Show(Info.RhfactorErrorInfo);
                return;
            }
            if (SaveElements())
            {
                if (this.ParentForm != null)
                    this.ParentForm.DialogResult = this.dialogResult = DialogResult.OK;
                EDXRFHelper.GotoMainPage(this);//回到主界面
            }
            //放标样测试
        }

        /// <summary>
        /// 下移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            if (dgvwElements.SelectedRows.Count < 1)
            {
                return;
            }
            var element = workCurveCurrent.ElementList.Items[dgvwElements.SelectedRows[0].Index];
            int index = 0;
            //int temp;
            CurveElement tempElement;
            for (int i = 0; i < workCurveCurrent.ElementList.Items.Count - 1; i++)
            {
                if (element.Caption == workCurveCurrent.ElementList.Items[i].Caption)
                {
                    index = i;
                    //temp = workCurveCurrent.ElementList.Items[i + 1].RowsIndex;
                    //workCurveCurrent.ElementList.Items[i + 1].RowsIndex = workCurveCurrent.ElementList.Items[i].RowsIndex;
                    //workCurveCurrent.ElementList.Items[i].RowsIndex = temp;

                    tempElement = workCurveCurrent.ElementList.Items[i + 1];
                    workCurveCurrent.ElementList.Items[i + 1] = workCurveCurrent.ElementList.Items[i];
                    workCurveCurrent.ElementList.Items[i] = tempElement;
                    ElementListToGrid();//绑定至界面
                    dgvwElements.Rows[0].Selected = false;
                    dgvwElements.Rows[i + 1].Selected = true;
                    dgvwElements.CurrentCell = dgvwElements.Rows[i + 1].Cells[0];
                    break;
                }
            }
            if (index != 0) dgvwElements.FirstDisplayedScrollingRowIndex = index;
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            //add by chuyaqin
            lstReadyDel.Clear();
            WorkCurveHelper.WorkCurveCurrent = WorkCurve.FindById(WorkCurveHelper.WorkCurveCurrent.Id);
            EDXRFHelper.GotoMainPage(this);//回到主界面
        }

        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvwElements_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1) return;
            if (dgvwElements.Columns[e.ColumnIndex].Name.Equals("ColSReferenceElements"))//拟合元素
            {
                string Calc = Convert.ToString(dgvwElements["ColIntensityWay", e.RowIndex].Value);
                if (Calc != IntensityWay.FixedReference.ToString() && Calc != IntensityWay.Reference.ToString())//不需要拟合
                {
                    // SkyrayMsgBox.Show(Info.UnNeedReference);
                    return;
                }
                Rectangle rect = dgvwElements.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                fitListBox.Items.Clear();//清空
                //添加所有元素
                for (int i = 0; i < dgvwElements.RowCount; i++)
                {
                    fitListBox.Items.Add(workCurveCurrent.ElementList.Items[i].Caption);
                }
                string fitElements = Convert.ToString(dgvwElements[e.ColumnIndex, e.RowIndex].Value);
                //检查是否是拟合元素
                string[] fitElems = fitElements.Split(',');
                for (int j = 0; j < fitListBox.Items.Count; j++)
                {
                    fitListBox.SetItemChecked(j, false);  
                    for (int i = 0; i < fitElems.Length; i++)
                    {
                        if (fitListBox.Items[j].ToString().ToUpper().Equals(fitElems[i].ToUpper()))
                        {
                            fitListBox.SetItemChecked(j, true);
                            break;
                        }
                    }
                }
                fitListBox.SetBounds(dgvwElements.Location.X + rect.X, dgvwElements.Location.Y + rect.Y, rect.Width, fitListBox.Height);
                fitListBox.Visible = true;
                fitListBox.BringToFront();
            }
            else if (dgvwElements.Columns[e.ColumnIndex].Name.Equals("ColPureElement"))//纯元素谱
            {

                List<SpecListEntity> returnResult = EDXRFHelper.GetReturnSpectrum(false, false);
                if (returnResult == null || returnResult.Count == 0)
                    return;
                specListSpectrum = returnResult[0];
                if (specListSpectrum == null)
                    return;
                //纯元素谱List

                long dgvId = workCurveCurrent.ElementList.Items[e.RowIndex].DevParamId;
                SpecEntity spec;
                string IsCondition = ReportTemplateHelper.LoadSpecifiedValue("FPThick", "IsRestrictPurElement");
                if (IsCondition == "1" && WorkCurveHelper.DeviceFunctype == FuncType.Thick)   //thick 扣背景的时候取消条件打开限制
                    spec = specListSpectrum.Specs.ToList()[0];
                else
                    spec = specListSpectrum.Specs.ToList().Find(s => s.DeviceParameter.Name == DeviceParameter.FindById(dgvId).Name);
                if (spec != null)
                {
                    dgvwElements[e.ColumnIndex, e.RowIndex].Value = spec.Name;
                    workCurveCurrent.ElementList.Items[e.RowIndex].ElementSpecName = specListSpectrum.Name;
                }
                else
                {
                    Msg.Show(Info.HaveNoSuitSpectrum);
                    return;
                }
                workCurveCurrent.ElementList.Items[e.RowIndex].SSpectrumData = Helper.TransforToDivTimeAndCurrent(Helper.ToStrs(spec.SpecDatac), spec.UsedTime, spec.TubCurrent);


                ElementListToGrid();
                firstDisplayScrollingRowIndex = e.RowIndex;
                this.dgvwElements.FirstDisplayedCell = this.dgvwElements.Rows[e.RowIndex].Cells[e.ColumnIndex];
                timer.Enabled = true;
            }
            else if (dgvwElements.Columns[e.ColumnIndex].Name.Equals("colPureNameNotFilter"))//纯元素谱
            {

                List<SpecListEntity> returnResult = EDXRFHelper.GetReturnSpectrum(false, false);
                if (returnResult == null || returnResult.Count == 0)
                    return;
                specListSpectrum = returnResult[0];
                if (specListSpectrum == null)
                    return;
                //纯元素谱List

                long dgvId = workCurveCurrent.ElementList.Items[e.RowIndex].DevParamId;
                SpecEntity spec;
                //string IsCondition = ReportTemplateHelper.LoadSpecifiedValue("FPThick", "IsRestrictPurElement");
                //if (IsCondition == "1" && WorkCurveHelper.DeviceFunctype == FuncType.Thick)   //thick 扣背景的时候取消条件打开限制
                spec = specListSpectrum.Specs.ToList()[0];
                //else
                //    spec = specListSpectrum.Specs.ToList().Find(s => s.DeviceParameter.Name == DeviceParameter.FindById(dgvId).Name);
                if (spec != null)
                {
                    dgvwElements[e.ColumnIndex, e.RowIndex].Value = spec.Name;
                    workCurveCurrent.ElementList.Items[e.RowIndex].ElementSpecNameNoFilter = specListSpectrum.Name;
                }
                else
                {
                    Msg.Show(Info.HaveNoSuitSpectrum);
                    return;
                }
                workCurveCurrent.ElementList.Items[e.RowIndex].SSpectrumDataNotFilter = Helper.TransforToDivTimeAndCurrent(Helper.ToStrs(spec.SpecDatac), spec.UsedTime, spec.TubCurrent);


                ElementListToGrid();
                firstDisplayScrollingRowIndex = e.RowIndex;
                this.dgvwElements.FirstDisplayedCell = this.dgvwElements.Rows[e.RowIndex].Cells[e.ColumnIndex];
                timer.Enabled = true;
            }
            else if (dgvwElements.Columns[e.ColumnIndex].Name.Equals("ColColor"))//选择颜色
            {
                ColorDialog cd = new ColorDialog();
                cd.FullOpen = true;
                cd.Color = Color.FromArgb(workCurveCurrent.ElementList.Items[e.RowIndex].Color);
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    dgvwElements[e.ColumnIndex, e.RowIndex].Style.BackColor = dgvwElements[e.ColumnIndex, e.RowIndex].Style.ForeColor =
                        dgvwElements[e.ColumnIndex, e.RowIndex].Style.SelectionBackColor =
                        dgvwElements[e.ColumnIndex, e.RowIndex].Style.SelectionForeColor = cd.Color;
                    workCurveCurrent.ElementList.Items[e.RowIndex].Color = cd.Color.ToArgb();
                }
            }
            else if (dgvwElements.Columns[e.ColumnIndex].Name.Equals("ColDifference"))//差额
            {
                if (dgvwElements[e.ColumnIndex, e.RowIndex].Value != null)
                {
                    if (dgvwElements[e.ColumnIndex, e.RowIndex].Value.Equals(false))
                    {
                        for (int i = 0; i < this.dgvwElements.RowCount; i++)
                        {
                            if (i == e.RowIndex)
                            {
                                workCurveCurrent.ElementList.Items[e.RowIndex].Flag = ElementFlag.Difference;
                            }
                            else
                            {
                                dgvwElements[e.ColumnIndex, i].Value = false;
                                workCurveCurrent.ElementList.Items[i].Flag = ElementFlag.Calculated;
                            }
                        }
                    }
                    else
                    {
                        workCurveCurrent.ElementList.Items[e.RowIndex].Flag = ElementFlag.Calculated;
                    }
                }
                else
                {
                    for (int i = 0; i < this.dgvwElements.RowCount; i++)
                    {
                        if (i == e.RowIndex)
                        {
                            workCurveCurrent.ElementList.Items[e.RowIndex].Flag = ElementFlag.Difference;
                        }
                        else
                        {
                            dgvwElements[e.ColumnIndex, i].Value = false;
                            workCurveCurrent.ElementList.Items[i].Flag = ElementFlag.Calculated;
                        }
                    }
                }
            }
            ReferenceElementToGrid();
        }

        Dictionary<int, int> d0 = new Dictionary<int, int>();
        private void dgvwElements_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox && dgvwElements.CurrentCell.OwningColumn.Name == "ColConditionDevice")
            {
                int indexrow = dgvwElements.CurrentRow.Index;

                if (!d0.Keys.Contains(indexrow)) d0.Add(indexrow, workCurveCurrent.ElementList.Items[indexrow].ConditionID);
                ((ComboBox)e.Control).SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);

            }
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int indexrow = dgvwElements.CurrentRow.Index;
            if (d0.Keys.Contains(indexrow))
            {
                if (((ComboBox)sender).SelectedIndex.ToString() != d0[indexrow].ToString())
                {
                    int isele = ((ComboBox)sender).SelectedIndex;
                    dgvwElements.CurrentRow.Cells["ColPureElement"].Value = "";
                    ((ComboBox)sender).SelectedIndex = isele;
                    workCurveCurrent.ElementList.Items[indexrow].SSpectrumData = "";
                    workCurveCurrent.ElementList.Items[indexrow].ElementSpecName = "";
                }
                d0.Remove(indexrow);
            }
        }


        /// <summary>
        /// 单元格内容改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvwElements_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == ColContentUnit.Index)
            {
                if (dgvwElements[e.ColumnIndex, e.RowIndex].Value.ToString() == "%")
                {
                    workCurveCurrent.ElementList.Items[e.RowIndex].ContentUnit = ContentUnit.per;
                }
                else if (dgvwElements[e.ColumnIndex, e.RowIndex].Value.ToString() == "ppm")
                {
                    workCurveCurrent.ElementList.Items[e.RowIndex].ContentUnit = ContentUnit.ppm;
                }
                else
                {
                    workCurveCurrent.ElementList.Items[e.RowIndex].ContentUnit = ContentUnit.permillage;
                }
            }
            if (e.ColumnIndex == ColThicknessUnit.Index)
            {
                if (dgvwElements[e.ColumnIndex, e.RowIndex].Value.ToString() == "um")
                {
                    workCurveCurrent.ElementList.Items[e.RowIndex].ThicknessUnit = ThicknessUnit.um;
                }
                else if (dgvwElements[e.ColumnIndex, e.RowIndex].Value.ToString() == "u〞")
                {
                    workCurveCurrent.ElementList.Items[e.RowIndex].ThicknessUnit = ThicknessUnit.ur;
                }
                else
                {
                    workCurveCurrent.ElementList.Items[e.RowIndex].ThicknessUnit = ThicknessUnit.gl;
                }
                
                //workCurveCurrent.ElementList.Items[e.RowIndex].ThicknessUnit = dgvwElements[e.ColumnIndex, e.RowIndex].Value.ToString() == "um" ? ThicknessUnit.um : ThicknessUnit.ur;
            }
            //add by chuyaqin 2011-04-11 峰边界和背边界的修改的限定
            if (e.ColumnIndex == ColConditionDevice.Index)
            {
                //DeviceParameter dpar = workCurveCurrent.Condition.DeviceParamList.First(w => String.Compare(w.Name, dgvwElements[ColConditionDevice.Index, e.RowIndex].Value.ToString(), false) == 0);
                DeviceParameter dpar = workCurveCurrent.Condition.DeviceParamList.ToList().Find(d => d.Id == int.Parse(dgvwElements[ColConditionDevice.Index, e.RowIndex].Value.ToString()));
                int Max = dpar.EndChann;
                int Min = dpar.BeginChann;
                if (int.Parse(dgvwElements[PeakHigh.Index, e.RowIndex].Value.ToString()) > Max || int.Parse(dgvwElements[PeakHigh.Index, e.RowIndex].Value.ToString()) < Min)
                {
                    dgvwElements[PeakHigh.Index, e.RowIndex].Value = dpar.EndChann;
                }
                if (int.Parse(dgvwElements[PeakLow.Index, e.RowIndex].Value.ToString()) < Min || int.Parse(dgvwElements[PeakLow.Index, e.RowIndex].Value.ToString()) > Max)
                {
                    dgvwElements[PeakLow.Index, e.RowIndex].Value = dpar.BeginChann;
                }
                if (int.Parse(dgvwElements[BaseLow.Index, e.RowIndex].Value.ToString()) < Min || int.Parse(dgvwElements[BaseLow.Index, e.RowIndex].Value.ToString()) > Max)
                {
                    dgvwElements[BaseLow.Index, e.RowIndex].Value = Min;
                }
                if (int.Parse(dgvwElements[BaseHigh.Index, e.RowIndex].Value.ToString()) > Max || int.Parse(dgvwElements[BaseHigh.Index, e.RowIndex].Value.ToString()) < Min)
                {
                    dgvwElements[BaseHigh.Index, e.RowIndex].Value = Max;
                }
                //for (int i = 0; i < dgvwElements.ColumnCount; i++)
                //{
                //    if (dgvwElements.Columns[i].Name.Equals("dgvBPeakLow")
                //        || dgvwElements.Columns[i].Name.Equals("BaseLow"))
                //    {
                //        int tempFormatValue = int.Parse(dgvwElements[i, e.RowIndex].Value.ToString());
                //        if (tempFormatValue < Min || tempFormatValue > Max)
                //        {
                //            dgvwElements[i, e.RowIndex].Value = Min;
                //        }

                //    }
                //    else if (dgvwElements.Columns[i].Name.Equals("dgvBPeakHigh")
                //        || dgvwElements.Columns[i].Name.Equals("BaseHigh"))
                //    {
                //        int tempFormatValue = int.Parse(dgvwElements[i, e.RowIndex].Value.ToString());
                //        if (tempFormatValue < Min || tempFormatValue > Max)
                //        {
                //            dgvwElements[i, e.RowIndex].Value = Max;
                //        }
                //    }
                //}

            }
            if (e.ColumnIndex == ColIntensityWay.Index)
            {
                workCurveCurrent.ElementList.Items[e.RowIndex].IntensityWay
                    = (IntensityWay)Enum.Parse(typeof(IntensityWay),
                    dgvwElements[e.ColumnIndex, e.RowIndex].Value.ToString());

                string Calc = Convert.ToString(dgvwElements[e.ColumnIndex, e.RowIndex].Value);
                if (Calc != IntensityWay.FixedReference.ToString() && Calc != IntensityWay.Reference.ToString())//不需要拟合元素
                {
                    dgvwElements["ColSReferenceElements", e.RowIndex].Value = "";
                    workCurveCurrent.ElementList.Items[e.RowIndex].References.Clear();
                    return;
                }
            }
            if (e.ColumnIndex == ColBaseIntensityWay.Index)
            {
                workCurveCurrent.ElementList.Items[e.RowIndex].BaseIntensityWay
                                   = (BaseIntensityWay)Enum.Parse(typeof(BaseIntensityWay),
                                   dgvwElements[e.ColumnIndex, e.RowIndex].Value.ToString());
            }
        }

        /// <summary>
        /// 数据验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvwElements_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //modify by chuyaqin 2011-04-11 修改左右界受条件边界的限制
            //DeviceParameter dpar = workCurveCurrent.Condition.DeviceParamList.First(w => String.Compare(w.Name, dgvwElements[ColConditionDevice.Index, e.RowIndex].Value.ToString(), false) == 0);
            DeviceParameter dpar = workCurveCurrent.Condition.DeviceParamList.ToList().Find(d => d.Id == int.Parse(dgvwElements[ColConditionDevice.Index, e.RowIndex].Value.ToString()));
            int Max = dpar.EndChann;
            int Min = dpar.BeginChann;
            if (dgvwElements.Columns[e.ColumnIndex].Name.Equals("dgvComparisonCoefficient"))
            {

                //int Max = int.Parse(Ranges.RangeDictionary["ComparisonCoefficient"].Max.ToString());
                //int Min = 0;
                try { int.Parse(e.FormattedValue.ToString()); }
                catch (FormatException)
                {
                    dgvwElements[e.ColumnIndex, e.RowIndex].Value = Min;
                    e.Cancel = true;
                    return;
                }
                if (int.Parse(e.FormattedValue.ToString()) > Max)
                {
                    dgvwElements[e.ColumnIndex, e.RowIndex].Value = Max;
                    e.Cancel = true;
                }
                if (int.Parse(e.FormattedValue.ToString()) < Min)
                {
                    dgvwElements[e.ColumnIndex, e.RowIndex].Value = Min;
                    e.Cancel = true;
                }
            }
            if (dgvwElements.Columns[e.ColumnIndex].Name.Equals("PeakLow")
                || dgvwElements.Columns[e.ColumnIndex].Name.Equals("PeakHigh")
                || dgvwElements.Columns[e.ColumnIndex].Name.Equals("BaseLow")
                || dgvwElements.Columns[e.ColumnIndex].Name.Equals("BaseHigh")
                || dgvwElements.Columns[e.ColumnIndex].Name.Equals("dgvBPeakLow")
                || dgvwElements.Columns[e.ColumnIndex].Name.Equals("dgvBPeakHigh"))
            {
                //int Max = (int)WorkCurveHelper.DeviceCurrent.SpecLength - 1;
                //int Min = 0;
                try { int.Parse(e.FormattedValue.ToString()); }
                catch (FormatException)
                {
                    dgvwElements[e.ColumnIndex, e.RowIndex].Value = Min;
                    e.Cancel = true;
                    return;
                }
                if (int.Parse(e.FormattedValue.ToString()) > Max && int.Parse(e.FormattedValue.ToString()) != 0)
                {
                    dgvwElements[e.ColumnIndex, e.RowIndex].Value = Max;
                    e.Cancel = true;
                    return;
                }
                if (int.Parse(e.FormattedValue.ToString()) < Min && int.Parse(e.FormattedValue.ToString()) != 0)
                {
                    dgvwElements[e.ColumnIndex, e.RowIndex].Value = Min;
                    e.Cancel = true;
                    return;
                }
            }

            if (dgvwElements.Columns[e.ColumnIndex].Name.Equals("colContentcoefficient"))
            {
                try { double.Parse(e.FormattedValue.ToString()); }
                catch (Exception)
                {
                    dgvwElements[e.ColumnIndex, e.RowIndex].Value = 1.00;
                    e.Cancel = true;
                    return;
                }
               
            }
        }

        /// <summary>
        /// 数据异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvwElements_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Msg.Show(dgvwElements.Columns[e.ColumnIndex].Name.ToString());
        }

        /// <summary>
        /// 隐藏拟合元素框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvwElements_MouseMove(object sender, MouseEventArgs e)
        {
            if (!fitListBox.Bounds.Contains(e.Location))
            {
                fitListBox.Visible = false;
                int count = dgvwElements.SelectedRows.Count;
                if (count <= 0) return;
                int index = dgvwElements.SelectedRows[0].Index;
                string Calc = Convert.ToString(dgvwElements["ColIntensityWay", index].Value);
                if (Calc != IntensityWay.Reference.ToString()) return;
                string element = Convert.ToString(dgvwElements["ColElementCaption", index].Value);
                string allElem = string.Empty;
                foreach (var elem in workCurveCurrent.ElementList.Items)
                {
                    allElem += elem.Caption + ",";
                }
                allElem = allElem.Length > 0 ? allElem.Substring(0, allElem.Length - 1) : string.Empty;
                if (workCurveCurrent.ElementList.Items[index].ReferenceElements.Contains(element)) return;
                if (dgvwElements["ColSReferenceElements", index].Value == null || dgvwElements["ColSReferenceElements", index].Value.ToString() == string.Empty)
                {
                   // dgvwElements["ColSReferenceElements", index].Value = element;
                   // var elem = workCurveCurrent.ElementList.Items[index];
                    dgvwElements["ColSReferenceElements", index].Value = allElem;
                    foreach (var elem in workCurveCurrent.ElementList.Items)
                    {
                        if (elem.References.ToList().Find(w => w.ReferenceElementName == elem.Caption) == null)
                        {
                            ReferenceElement refElement;
                            if (DifferenceDevice.IsThick)
                            {
                                DeviceParameter dp = workCurveCurrent.Condition.DeviceParamList[0];
                                refElement = ReferenceElement.New.Init(elem.Caption, elem.Caption, dp.BeginChann, dp.EndChann, elem.BaseLow, elem.BaseHigh, elem.PeakDivBase);
                            }
                            else
                                refElement = ReferenceElement.New.Init(elem.Caption, elem.Caption, elem.PeakLow, elem.PeakHigh, elem.BaseLow, elem.BaseHigh, elem.PeakDivBase);
                            workCurveCurrent.ElementList.Items[index].References.Add(refElement);
                            //workCurveCurrent.ElementList.Items[index].Save();
                            ReferenceElementToGrid();
                        }
                    }
                }
                //else
                //{
                //    if (DifferenceDevice.IsThick)
                //    {
                //        dgvwElements["ColSReferenceElements", index].Value += ("," + element);
                //    }

                //}
            }

        }

        /// <summary>
        /// 影响元素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fitListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            int count = dgvwElements.SelectedRows.Count;
            if (count <= 0) return;
            int index = dgvwElements.SelectedRows[0].Index;
            var elem = workCurveCurrent.ElementList.Items[index];
            fitElements = string.Empty;
            for (int i = 0; i < fitListBox.CheckedItems.Count; i++)
            {
                string name = fitListBox.CheckedItems[i].ToString();
                fitElements += name + ',';//拟合元素
                if (elem.References.ToList().Find(w => w.ReferenceElementName.ToUpper() == name.ToUpper()) == null)
                {
                    var refElement = workCurveCurrent.ElementList.Items.ToList().Find(a => a.Caption == name);
                    if (refElement != null)
                    {
                         ReferenceElement refE;
                        if (DifferenceDevice.IsThick)
                        {
                            DeviceParameter dp = workCurveCurrent.Condition.DeviceParamList[0];
                            refE = ReferenceElement.New.Init(elem.Caption, name, dp.BeginChann, dp.EndChann, elem.BaseLow, elem.BaseHigh, elem.PeakDivBase);
                        }
                        else
                         refE = ReferenceElement.New.Init(elem.Caption, name, refElement.PeakLow, refElement.PeakHigh, refElement.BaseLow, refElement.BaseHigh, refElement.PeakDivBase);
                       
                        elem.References.Add(refE);
                        //elem.Save();
                    }
                }
            }
            if (fitElements.Length >= 1)
            {
                fitElements = fitElements.Substring(0, fitElements.Length - 1);
            }
            if (dgvwElements.CurrentCell != null)
            {
                dgvwElements.CurrentCell.Value = fitElements;
                workCurveCurrent.ElementList.Items[index].SReferenceElements = fitElements;
            }
            string[] str = fitElements.Split(',');
            for (int i = elem.References.Count - 1; i >= 0; i--)
            {
                string na = elem.References[i].ReferenceElementName;
                if (str.ToList().Find(s => s == na) == null)
                {
                    elem.References[i].Delete();
                    elem.References.RemoveAt(i);
                }
            }
            ReferenceElementToGrid();
        }

        /// <summary>
        /// 扣本底
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkRemoveBack_Click(object sender, EventArgs e)
        {
            if (chkRemoveBack.Checked)
            {
                SelectSample sample = new SelectSample(AddSpectrumType.OpenSpectrum);
                WorkCurveHelper.OpenUC(sample, false, Info.SelectBackSpec,true);
                if (sample.DialogResult == DialogResult.OK)
                {
                    workCurveCurrent.ElementList.Id = long.Parse(sample.specListCurrent.WorkCurveName);
                }
                else
                {
                    chkRemoveBack.Checked = false;
                }
            }
            else
            {
                workCurveCurrent.ElementList.Id = 0;
                workCurveCurrent.ElementList.IsRemoveBk = false;
            }
        }

        #endregion


        public override void ExcuteEndProcess(params object[] objs)
        {
            if (WorkCurveHelper.WorkCurveCurrent.ElementList != null)
            {
                EDXRFHelper.DisplayWorkCurveControls();
                List<CurveElement> lst = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.OrderBy(d => d.RowsIndex).ToList();
                WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Clear();
                foreach (var v in lst)
                { WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Add(v); }
                DifferenceDevice.MediumAccess.AddIntrestedElements();
                if (flag)
                {
                    ToolStripControls test = MenuLoadHelper.MenuStripCollection.Find(w => w.CurrentNaviItem.Name == "ElementRef");
                    EDXRFHelper.RecurveNextUC(test);
                }

                if (upflag)
                {
                    ToolStripControls test = MenuLoadHelper.MenuStripCollection.Find(w => w.CurrentNaviItem.Name == "OpenCurve");
                    EDXRFHelper.RecurseUpUC(test);
                }
            }
        }
        private bool flag = false;
        private bool upflag = false;
        private void btnApplication_Click(object sender, EventArgs e)
        {

            if (txtRhfactor.Visible == true && chkRhThick.Checked && chkRhThick.Visible == true && workCurveCurrent.CalcType == CalcType.EC&&!workCurveCurrent.ElementList.RhIsMainElementInfluence && (txtRhfactor.Text == "" || double.Parse(txtRhfactor.Text) > 1))
            {
                Msg.Show(Info.RhfactorErrorInfo);
                return;
            }

            int index = (dgvwElements.CurrentRow == null) ? 0 : dgvwElements.CurrentRow.Index;
            //int index = -1;
            //foreach(DataGridViewRow row in dgvwElements.Rows)
            //    if (row)
            if (SaveElements())
            {
                DifferenceDevice.MediumAccess.AddIntrestedElements();

                //修改：何晓明 20111128 第一次点击应用有效第二次无效BUG修正
                UCElement_Load(null, null);
            }

            if (dgvwElements.Rows.Count > 0 && index >= 0)
            {
                dgvwElements.FirstDisplayedScrollingRowIndex = (index == 0) ? index : index - 1;
                dgvwElements.Rows[index].Selected = true;
                dgvwElements.CurrentCell = dgvwElements.Rows[index].Cells[0];
            }

           
        }

        private void btnElementTo_Click(object sender, EventArgs e)
        {
            if (SaveElements())
            {
                if (this.ParentForm != null)
                    this.ParentForm.DialogResult = this.dialogResult = DialogResult.OK;
                EDXRFHelper.GotoMainPage(this);//回到主界面
                flag = true;
            }

        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (SaveElements())
            {
                if (this.ParentForm != null)
                    this.ParentForm.DialogResult = this.dialogResult = DialogResult.OK;
                EDXRFHelper.GotoMainPage(this);//回到主界面
                upflag = true;
            }
        }


        #region 纯元素谱清除
        private void ToolStripMenuItemDeleColPureElement_Click(object sender, EventArgs e)
        {
            if (iSelectRowIndex != -1 && iSelectColumnIndex != -1)
            {
                dgvwElements[iSelectColumnIndex, iSelectRowIndex].Value = "";
                workCurveCurrent.ElementList.Items[iSelectRowIndex].SSpectrumData = "";
                workCurveCurrent.ElementList.Items[iSelectRowIndex].ElementSpecName = "";
            }


        }

        int iSelectRowIndex = -1;
        int iSelectColumnIndex = -1;
        private void dgvwElements_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            iSelectRowIndex = -1;
            iSelectColumnIndex = -1;
            if (e.Button == MouseButtons.Right && dgvwElements.Columns[e.ColumnIndex].Name.Equals("ColPureElement"))
            {
                if (e.RowIndex >= 0)
                {
                    dgvwElements.ClearSelection();
                    dgvwElements.Rows[e.RowIndex].Selected = true;
                    iSelectRowIndex = e.RowIndex;
                    iSelectColumnIndex = e.ColumnIndex;
                    ReferenceElementToGrid();

                }
            }

        }
        #endregion

        private void dgvwReferences_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgvwElements.CurrentRow == null || dgvwElements.RowCount <= 0) return;
            CurveElement curveElement = workCurveCurrent.ElementList.Items.ToList().Find(delegate(CurveElement v) { return v.Caption == dgvwReferences.CurrentRow.Cells["ColReferenceElement"].Value.ToString(); });
            //DeviceParameter dpar = workCurveCurrent.Condition.DeviceParamList.ToList().Find(d => d.Id == int.Parse(dgvwElements.CurrentRow.Cells[ColConditionDevice.Index].Value.ToString()));

            //int Max = dpar.EndChann;
            //int Min = dpar.BeginChann;

            int DefaultValue = 0;

            if (dgvwReferences.Columns[e.ColumnIndex].Name.Equals("ColReferenceLeft") && curveElement != null)
            {
                DefaultValue = curveElement.PeakLow;
            }
            else if (dgvwReferences.Columns[e.ColumnIndex].Name.Equals("ColReferenceRight") && curveElement != null)
            {
                DefaultValue = curveElement.PeakHigh;
            }
            else if (dgvwReferences.Columns[e.ColumnIndex].Name.Equals("ColBackLeft") && curveElement != null)
            {
                DefaultValue = curveElement.BaseLow;
            }
            else if (dgvwReferences.Columns[e.ColumnIndex].Name.Equals("ColBackRight") && curveElement != null)
            {
                DefaultValue = curveElement.BaseHigh;
            }

            if (dgvwReferences.Columns[e.ColumnIndex].Name.Equals("ColReferenceLeft")
                || dgvwReferences.Columns[e.ColumnIndex].Name.Equals("ColReferenceRight")
                || dgvwReferences.Columns[e.ColumnIndex].Name.Equals("ColBackLeft")
                || dgvwReferences.Columns[e.ColumnIndex].Name.Equals("ColBackRight"))
            {
                try { int.Parse(e.FormattedValue.ToString()); }
                catch
                {
                    dgvwReferences[e.ColumnIndex, e.RowIndex].Value = DefaultValue;
                    e.Cancel = true;
                    return;
                }
                //if (int.Parse(e.FormattedValue.ToString()) > DefaultValue && int.Parse(e.FormattedValue.ToString()) != 0)
                if (int.Parse(e.FormattedValue.ToString()) >= (int)WorkCurveHelper.DeviceCurrent.SpecLength && int.Parse(e.FormattedValue.ToString()) != 0)
                {
                    dgvwReferences[e.ColumnIndex, e.RowIndex].Value = DefaultValue;
                    e.Cancel = true;
                    return;
                }
                if (int.Parse(e.FormattedValue.ToString()) < 0 && int.Parse(e.FormattedValue.ToString()) != 0)
                {
                    dgvwReferences[e.ColumnIndex, e.RowIndex].Value = DefaultValue;
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void dgvwElements_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dgvwElements.CurrentRow == null || dgvwElements.RowCount <= 0 || !DifferenceDevice.IsXRF) return;
            foreach (DataGridViewRow row in dgvwElements.Rows)
            {
                string sEleNanme = row.Cells["ColElementCaption"].Value.ToString();

                CurveElement curveElement = workCurveCurrent.ElementList.Items.ToList().Find(delegate(CurveElement v) { return v.Caption == row.Cells["ColElementCaption"].Value.ToString(); });
                DataGridViewComboBoxCell dgvcom = row.Cells["ColOxide"] as DataGridViewComboBoxCell;
                List<Oxide> oxidel = Oxide.FindBySql("select * from Oxide where Atom_Id in (select atomid from atom where  atomname='" + sEleNanme + "')");

                dgvcom.Items.Clear();
                if (oxidel.Count > 0)
                {
                    dgvcom.Items.Add("");
                    foreach (Oxide oxide in oxidel)
                        dgvcom.Items.Add(oxide.OxideName);
                }
                if (curveElement.IsOxide)
                    dgvcom.Value = curveElement.Formula;

                //dgvcom.DataSource = oxidel;
                //dgvcom.ValueMember = "OxideName";
                //dgvcom.DisplayMember = "OxideName";
                //if (curveElement.IsOxide)
                //    dgvcom.Value = curveElement.Formula;
            }

        }

        private void chkRhThick_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRhThick.Checked && workCurveCurrent.CalcType != CalcType.FP)
            {
                
                this.txtRhfactor.Visible = true;
            }
            else
            {
                this.txtRhfactor.Visible = false;
            }
            txtLayerElems.Visible = chkRhThick.Checked;
        }



        private void txtRhfactor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar > (char)47 && e.KeyChar < (char)58 || e.KeyChar == (char)8 || e.KeyChar == (char)46 || e.KeyChar == (char)3 || e.KeyChar == (char)22)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show(Info.IllegalInput);
            }
        }

        private void chkShowAreaThick_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void btnCustomUnit_Click(object sender, EventArgs e)
        {
            FrmAreaDensityUnit uit = new FrmAreaDensityUnit();
            uit.ShowDialog();
            RefreshComDensityUnits();            
        }
        private void RefreshComDensityUnits()
        {
            cmbDensityUnits.Items.Clear();
            cmbDensityUnits.Items.Add(Info.strAreaDensityUnit);
            List<AreaDensityUnit> units = AreaDensityUnit.FindAll();
            for (int i = 0; i < units.Count; i++)
            {
                cmbDensityUnits.Items.Add(units[i].Name);
            }
            if (WorkCurveHelper.WorkCurveCurrent.AreaThickType.IsNullOrEmpty() || !cmbDensityUnits.Items.Contains(WorkCurveHelper.WorkCurveCurrent.AreaThickType))
            {
                cmbDensityUnits.SelectedItem = Info.strAreaDensityUnit;
            }
            else cmbDensityUnits.SelectedItem = WorkCurveHelper.WorkCurveCurrent.AreaThickType;
        }

        private void chkLstbxLayerElems_SelectedValueChanged(object sender, EventArgs e)
        {
            string layerElems = string.Empty;
            for (int i = 0; i < chkLstbxLayerElems.CheckedItems.Count; i++)
            {
                string name = chkLstbxLayerElems.CheckedItems[i].ToString();
                layerElems += name + ',';//拟合元素
            }
            if (layerElems.Length >= 1)
            {
                layerElems = layerElems.Substring(0, layerElems.Length - 1);
            }
            txtLayerElems.Text = layerElems;
        }

        private void txtLayerElems_Click(object sender, EventArgs e)
        {
            if (workCurveCurrent.CalcType != CalcType.FP) return;
            chkLstbxLayerElems.Items.Clear();//清空
            //添加所有元素
            for (int i = 0; i < dgvwElements.RowCount; i++)
            {
                chkLstbxLayerElems.Items.Add(workCurveCurrent.ElementList.Items[i].Caption);
            }
            //检查是否是拟合元素
            string[] layerElems = txtLayerElems.Text.Split(',');
            for (int j = 0; j < chkLstbxLayerElems.Items.Count; j++)
            {
                chkLstbxLayerElems.SetItemChecked(j, false);
                for (int i = 0; i < layerElems.Length; i++)
                {
                    if (chkLstbxLayerElems.Items[j].ToString().ToUpper().Equals(layerElems[i].ToUpper()))
                    {
                        chkLstbxLayerElems.SetItemChecked(j, true);
                        break;
                    }
                }
            }

            chkLstbxLayerElems.SetBounds(grpNOTThick.Location.X + txtLayerElems.Location.X, grpNOTThick.Location.Y+txtLayerElems.Location.Y+txtLayerElems.Height-chkLstbxLayerElems.Height-2, txtLayerElems.Width, chkLstbxLayerElems.Height);
            chkLstbxLayerElems.Visible = true;
            chkLstbxLayerElems.BringToFront();

        }

        private void UCElement_MouseMove(object sender, MouseEventArgs e)
        {
            if (!chkLstbxLayerElems.Bounds.Contains(e.Location))
            {
                chkLstbxLayerElems.Visible = false;
            }
        }

        private void chkNip2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNip2.Checked)
            {
                colPureNameNotFilter.Visible = true;
            }
            else
            {
                colPureNameNotFilter.Visible = false;
            }
        }

     
        


    }
}
