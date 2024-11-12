using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;
using ZedGraph;
using Skyray.Controls;
using System.Threading;
using Skyray.EDXRFLibrary.Spectrum;

using Skyray.EDX.Common.Component;
using Skyray.EDX.Common.ReportHelper;
using System.Runtime.InteropServices;

using Skyray.API;
using Skyray.EDX.Common.Component;
namespace Skyray.UC
{
    /// <summary>
    /// 编辑元素计算强度等参数类
    /// </summary>
    public partial class UCEditData : Skyray.Language.UCMultiple
    {

        #region Fields

        /// <summary>
        /// 当前工作曲线
        /// </summary>
        private WorkCurve workCurveCurrent;

        /// <summary>
        /// 样品名
        /// </summary>
        private List<SpecListEntity> specListSelected;

        /// <summary>
        /// 元素列表下标
        /// </summary>
        private int SelectDgvIndex = 0;

        /// <summary>
        /// 激活样品集合
        /// </summary>
        private List<StandSample> lstActiveSample;
        /// <summary>
        /// 未激活样品集合
        /// </summary>
        private List<StandSample> lstNoActiveSample;
        /// <summary>
        /// 激活点
        /// </summary>
        private List<PointPairList> lstActiveppl;
        /// <summary>
        /// 未激活点
        /// </summary>
        private List<PointPairList> lstNoActiveppl;
        /// <summary>
        /// 斜率
        /// </summary>
        private double Slope; //斜率
        /// <summary>
        /// 截距
        /// </summary>
        private double Intercept; //截距
        /// <summary>
        /// 计算曲线的点
        /// </summary>
        private PointF[] pfarr;//计算曲线的点

        private PointF[] pScalePoints;//记录刻度的点
        /// <summary>
        /// 全部激活是否点击
        /// </summary>
        //private bool IschkColActiveClicked = true;
        /// <summary>
        /// 全部匹配是否点击
        /// </summary>
        private bool IschkColIsMatchClicked = true;
        /// <summary>
        /// Thick全选激活是否点击
        /// </summary>
        private bool IschkSelectAllClicked = true;

        #endregion

        #region Init

        /// <summary>
        /// 构造
        /// </summary>
        public UCEditData()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCEditData_Load(object sender, EventArgs e)
        {
            workCurveCurrent = WorkCurve.FindById(WorkCurveHelper.WorkCurveCurrent.Id);
            List<CurveElement> lst = workCurveCurrent.ElementList.Items.ToList().OrderBy(data => data.RowsIndex).ToList();
            for (int i = 0; i < workCurveCurrent.ElementList.Items.Count; i++)
            {
                workCurveCurrent.ElementList.Items[i] = lst[i];
            }
            int SetCurrent = workCurveCurrent.Condition.DeviceParamList[0].TubCurrent;
            workCurveCurrent.SetCurrent(SetCurrent);
            workCurveCurrent.SetBasePureSpec(WorkCurveHelper.GetbasePureSpec());
            BindHelper.BindCheckedToCtrl(chkInviteMatch, workCurveCurrent, "IsJoinMatch", true);
            ColumnUncertainty.Visible = DifferenceDevice.IsAnalyser;
           // dgvSampleDensity.Visible = WorkCurveHelper.DeviceFunctype == FuncType.Thick && WorkCurveHelper.CalcType == CalcType.PeakDivBase;
            //dgvEleLayerDensity.Visible = WorkCurveHelper.DeviceFunctype == FuncType.Thick;
            if (WorkCurveHelper.DeviceFunctype == FuncType.Thick && WorkCurveHelper.CalcType != CalcType.PeakDivBase)
            {
                if (WorkCurveHelper.CalcType == CalcType.FP)
                    tabcwEdit.TabPages.Remove(this.tabpData);
                else
                    tabcwEdit.TabPages.Remove(this.tabThickData);
                this.ColThickness.Visible = true;
                //if (workCurveCurrent.IsThickShowContent)
                //{
                    this.EditContent.Visible = true;
                //}
                //else
                //{
                //    this.ColElement.Width = 200;
                //    this.EditIntensity.Width = 160;
                //    this.ColThickness.Width = 160;
                //    this.ColActive.Width = 120;
                //    this.dgvCContent.Visible = false;
                //}
                if (WorkCurveHelper.CalcType == CalcType.EC)
                {
                    this.ColLevel.Visible = true;
                }
                InitLayer();
            }
            else if (WorkCurveHelper.DeviceFunctype == FuncType.Rohs)
            {
                tabcwEdit.TabPages.Remove(this.tabThickData);
                this.ColElement.Width = 200;
                this.EditIntensity.Width = 160;
                this.EditContent.Width = 160;
                //this.ColActive.Width = 120;
                this.EditContent.Visible = true;
            }
            else
            {
            this.EditContent.Visible = true;
            tabcwEdit.TabPages.Remove(this.tabThickData);
            }
            specListSelected = new List<SpecListEntity>();
            LoadMethod(true);
        }



        public void InitLayer()
        {
            if (workCurveCurrent.ElementList.Items.Count <= 1) return;
            List<StandSample> lsTemp = workCurveCurrent.ElementList.Items[1].Samples.ToList();
            for (int m = 2; m < workCurveCurrent.ElementList.Items.Count; m++)
            {
                for (int n = 0; n < workCurveCurrent.ElementList.Items[m].Samples.Count; n++)
                {
                    if (lsTemp.Find(w => w.SampleName == workCurveCurrent.ElementList.Items[m].Samples[n].SampleName) == null)
                    {
                        lsTemp.Add(workCurveCurrent.ElementList.Items[m].Samples[n]);
                    }
                }
            }
           
            int sampleCount = lsTemp.Count;
            int elementCount = workCurveCurrent.ElementList.Items.Count;
            for (int i = 0; i < sampleCount; i++)
            {

                for (int j = 0; j < elementCount; j++)
                {
                     
                    if (workCurveCurrent.ElementList.Items[j].Samples.Count == 0) continue;
                    try
                    {
                        StandSample standi = workCurveCurrent.ElementList.Items[j].Samples.First(w => w.SampleName == lsTemp[i].SampleName);

                        if (standi.Level == "单层" || standi.Level == "Single layer")
                        {
                            standi.Level = Info.SingleLayer;
                        }
                        else if (standi.Level == "多层" || standi.Level == "Multiple layers")
                        {
                            standi.Level = Info.MultiLayer;
                        }
                    }
                    catch
                    { }
                }

            }
        }

        public override void PageLoad(object sender, EventArgs e)
        {
            base.PageLoad(sender, e);
            SetContentUnit(0);
        }

        private void LoadMethod(bool bl)
        {
            try
            {
                foreach (var element in workCurveCurrent.ElementList.Items)
                {
                    if (element.ContentUnit == ContentUnit.ppm)
                    {
                        foreach (var sample in element.Samples)
                        {
                            //sample.Y = float.Parse(Convert.ToString(decimal.Parse(sample.Y.ToString()) * 10000));
                            sample.Y = (double.Parse(sample.Y) * 10000.0).ToString();
                        }
                    }
                    if (element.ContentUnit == ContentUnit.permillage)
                    {
                        foreach (var sample in element.Samples)
                        {
                            //sample.Y = float.Parse(Convert.ToString(decimal.Parse(sample.Y.ToString()) * 10));
                            sample.Y = (double.Parse(sample.Y) * 10.0).ToString();
                        }
                    }
                    if (element.ThicknessUnit == ThicknessUnit.ur)
                    {
                        foreach (var sample in element.Samples)
                        {
                            //sample.Z = float.Parse(Convert.ToString(decimal.Parse(sample.Z.ToString()) / (decimal)0.0254));
                            sample.Z = (double.Parse(sample.Z) / 0.0254).ToString();
                        }
                    }
                }
            }
            catch{}

            InitElement(bl);//初始化元素
            InitStandSample(true);//初始化标准样品
            InitNeedChk();//初始化GROUP
            SetContentUnit(0);

            if (!this.tabcwEdit.TabPages.Contains(this.tabThickData))
                return;
            this.dvgSampleThick.Rows.Clear();
            this.dvgThick.Rows.Clear();
            if (WorkCurveHelper.DeviceFunctype == FuncType.Thick && WorkCurveHelper.CalcType == CalcType.FP)
            {
                List<string> listDgv = new List<string>();
                Dictionary<string, float> layer = new Dictionary<string, float>();
                foreach (var element in workCurveCurrent.ElementList.Items)
                {
                    element.Samples.ToList().ForEach(wc =>
                    {
                        if (!listDgv.Contains(wc.SampleName))
                        {
                            this.dvgSampleThick.Rows.Add(wc.SampleName);
                            listDgv.Add(wc.SampleName);
                        }
                    });
                }
            }
        }

        /// <summary>
        /// 初始化元素
        /// </summary>
        private void InitElement(bool isFirst)
        {
            dgvwElement.Rows.Clear();
            dgvwElement.Rows.Add(workCurveCurrent.ElementList.Items.Count);
           
            if (workCurveCurrent.CalcType == CalcType.EC && WorkCurveHelper.DeviceFunctype == FuncType.XRF && isFirst)//EC算法添加主元素列
            {
                DataGridViewCheckBoxColumn dgvchk = new DataGridViewCheckBoxColumn();//添加主元素列
                dgvchk.Name = "ColMain";
                dgvchk.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvchk.HeaderText = Info.ElementMain;
                //dgvwElement.Columns.Add(dgvchk);
                dgvwElement.Columns.Add(dgvchk);
            }
            
            for (int i = 0; i < workCurveCurrent.ElementList.Items.Count; i++)
            {
                dgvwElement[0, i].Value = workCurveCurrent.ElementList.Items[i].Caption;
                dgvwElement[dgvEleLayerDensity.Index,i].Value = workCurveCurrent.ElementList.Items[i].LayerDensity;
                if (workCurveCurrent.CalcType == CalcType.EC && WorkCurveHelper.DeviceFunctype == FuncType.XRF)
                {
                    int colIndex = dgvwElement.Columns["ColMain"].Index;
                    if (workCurveCurrent.ElementList.Items[i].Flag == ElementFlag.Internal)
                    {
                        dgvwElement[colIndex, i].Value = true;
                    }
                }
            }
        }

        /// <summary>
        /// 初始化标准样品
        /// </summary>
        private void InitStandSample(bool isTranslate)
        {
            if (!this.tabcwEdit.TabPages.Contains(this.tabpData))
                return;
            dgwSample.Rows.Clear();
            dgwSample.DataSource = null;
            BindingSource bs = new BindingSource();
            var list = workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples.OrderBy(w => w.SampleName);
            foreach (var temp in list)
            {
                bs.Add(temp);
            }
            this.dgwSample.AutoGenerateColumns = false;
            this.dgwSample.DataSource = bs;//绑定数据源
            //this.dgwSample.Columns["EditContent"].DefaultCellStyle.Format = "c";

            
        }

        private void SetSeleAll()
        {
            if (WorkCurveHelper.DeviceFunctype == FuncType.Thick)
            {
                bool isThickActive = false;
                if (dvgContentIntensity.Columns["ColActiveThick"].Visible)
                { chkSelectAll.Visible = true; isThickActive = true; }
                else
                { chkSelectAll.Visible = false; isThickActive = false; }


                int iThickActive = 0;
                foreach (DataGridViewRow row in dvgContentIntensity.Rows)
                {
                    if (isThickActive && row.Cells["ColActiveThick"].Value.ToString().ToLower() == "true") iThickActive++;
                }

                if (isThickActive && iThickActive == dvgContentIntensity.Rows.Count)
                    chkSelectAll.Checked = true;
                else
                    chkSelectAll.Checked = false;

                if (dvgContentIntensity.Rows.Count == 0) { chkSelectAll.Checked = false; }            
            }
            else
            {
                bool isColActive = false;
                bool isColIsMatch = false;
                if (dgwSample.Columns["ColActive"].Visible)
                { chkColActive.Visible = true; isColActive = true; }
                else
                { chkColActive.Visible = false; isColActive = false; }

                if (dgwSample.Columns["ColIsMatch"].Visible)
                { chkColIsMatch.Visible = true; isColIsMatch = true; }
                else
                { chkColIsMatch.Visible = false; isColIsMatch = false; }

                int iColActive = 0;
                int iColIsMatch = 0;
                foreach (DataGridViewRow row in dgwSample.Rows)
                {
                    if (isColActive && row.Cells["ColActive"].Value.ToString().ToLower() == "true") iColActive++;
                    if (isColIsMatch && row.Cells["ColIsMatch"].Value.ToString().ToLower() == "true") iColIsMatch++;
                }

                if (isColActive && iColActive == dgwSample.Rows.Count)
                    chkColActive.Checked = true;
                else
                    chkColActive.Checked = false;
                if (isColIsMatch && iColIsMatch == dgwSample.Rows.Count)
                    chkColIsMatch.Checked = true;
                else
                    chkColIsMatch.Checked = false;
                if (dgwSample.Rows.Count == 0) { chkColActive.Checked = false; chkColIsMatch.Checked = false; }
            }
        }
        

        private void chkColIsMatch_CheckedChanged(object sender, EventArgs e)
        {
            //foreach (DataGridViewRow row in dgwSample.Rows)
            //{
            //    row.Cells["ColIsMatch"].Value = chkColIsMatch.Checked;
            //}
                
        }

        private void chkColActive_CheckedChanged(object sender, EventArgs e)
        {
            //修改：何晓明 20110922 激活与全部激活不同步
            //if(chkColActive.Tag==null)
            //
            //IschkColActiveClicked = false;
            //foreach (DataGridViewRow row in dgwSample.Rows)
            //{
            //    row.Cells["ColActive"].Value = chkColActive.Checked;
            //}
        }


        /// <summary>
        /// 初始化GROUP
        /// </summary>
        private void InitNeedChk()
        {
            if (WorkCurveHelper.DeviceFunctype == FuncType.XRF)
            {
                if (workCurveCurrent.CalcType == CalcType.EC)//EC法
                {
                    grpEC.Visible = true;
                }
                else if (workCurveCurrent.CalcType == CalcType.FP)
                {
                    //20110518 paul
                    //if (tabcwEdit.TabPages.IndexOfKey("tabpGraph") != -1)
                    //tabcwEdit.TabPages.RemoveByKey("tabpGraph");

                    grpEC.Visible = false;
                    grpFP.Visible = true;
                }
                else
                { 
                    
                }
                //ColIsMatch.Visible = true;
                //chkColIsMatch.Visible = true;
                //ColumnMatchSpec.Visible = true;
            }
            else if (WorkCurveHelper.DeviceFunctype == FuncType.Thick )//测厚
            {
                //20110518 paul
                //if (tabcwEdit.TabPages.IndexOfKey("tabpGraph") != -1)
                //    tabcwEdit.TabPages.RemoveByKey("tabpGraph");
                if (workCurveCurrent.CalcType == CalcType.EC)
                {
                    grpThick.Visible = true;
                    grpFP.Visible = false;
                }
                else if (workCurveCurrent.CalcType == CalcType.FP)
                {
                    grpThick.Visible = false;
                    grpFP.Visible = true;
                    radContentVS.Visible = false;
                    lblCoee.Visible = false;
                    txtCoee.Visible = false;
                    radIntensityVS.Checked = true;


                    if (workCurveCurrent.ElementList.ThCalculationWay == ThCalculationWay.ThLinear)
                    {
                        radLineThick.Checked = true;

                    }
                    else
                    {
                        radInsertThick.Checked = true;
                    }



                }
                else
                {
                    grpEC.Visible = true;
                    grpFP.Visible = false;
                }
                chkInviteMatch.Visible = false;
            }
            else if (WorkCurveHelper.DeviceFunctype == FuncType.Rohs)
            {
                if (workCurveCurrent.CalcType == CalcType.EC)//EC法
                {
                    grpEC.Visible = true;
                    grpOnes.Visible = false;
                    radContentContect.Visible = false;
                    radIntensityCorrect.Visible = false;
                    btnCorrectCoee.Visible = false;
                    chkInviteMatch.Visible = false;
                }
            }

            SetSeleAll();
        }

        #endregion

        #region Events

        /// <summary>
        /// 增加数据元素处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //20110601  paul 判断当前元素是否为基础层，如为则不能进行添加
            if (WorkCurveHelper.DeviceFunctype == FuncType.Thick && (WorkCurveHelper.CalcType==CalcType.EC
                || WorkCurveHelper.CalcType == CalcType.PeakDivBase))
            {
                var element = workCurveCurrent.ElementList.Items[SelectDgvIndex];
                if (element == null || element.LayerNumBackUp == null ) return;
                //if (element.LayerNumBackUp.ToString() == "基材")
                if (workCurveCurrent.ElementList.Items.Count == element.LayerNumber)
                {
                    Msg.Show(Info.strLayerNumber);
                    return;
                }
            }

            List<SpecListEntity> returnResult = EDXRFHelper.GetReturnSpectrum(false,false);
            if (returnResult == null || returnResult.Count == 0)
                return;
            specListSelected = returnResult;
            //if (WorkCurveHelper.SaveType == 0)
            //{
            //    SelectSample sample = new SelectSample(AddSpectrumType.OpenStandardSpec);//标样谱
            //    sample.IsCaculate = false;
            //    WorkCurveHelper.OpenUC(sample, false);
            //    if (sample.DialogResult != DialogResult.OK)//未选择标样
            //    { return; }
            //    specListSelected = sample.SelectedSpecList;
            //}
            //else
            //{
            //    var returnTT = DifferenceDevice.interClassMain.OpenFileDialog(false);
            //    if (returnTT == null)
            //        return;
            //    specListSelected.Add(returnTT);
            //}
            //SelectSample sample = new SelectSample(AddSpectrumType.OpenStandardSpec);//标样谱
            //sample.IsCaculate = false;
            //WorkCurveHelper.OpenUC(sample, false);
            //if (sample.DialogResult != DialogResult.OK)//未选择标样
            //{ return; }

            //根据20111108讨论，添加标样的条件个数必须和当前待测样品个数相同WorkCurveMeasureCondition
            foreach (SpecListEntity etSpeclIst in specListSelected)
            {
                if (workCurveCurrent.Condition.DeviceParamList != null && etSpeclIst.Specs.Length < workCurveCurrent.Condition.DeviceParamList.Count)
                {
                    Msg.Show(Info.WorkCurveMeasureCondition);
                    return;
                }
            }
            
          
            //Application.DoEvents();
            ThreadPool.QueueUserWorkItem(delegate
            {
               
                BeginInvoke((MethodInvoker)delegate
                {
                    WinMethod.SendMessage(DifferenceDevice.interClassMain.deviceMeasure.interfacce.OwnerHandle, DeviceInterface.CUSTOM_MESSAGE, 0, 0);

                    #region 测厚
                    if (WorkCurveHelper.DeviceFunctype == FuncType.Thick )//测厚
                    {
                        if (WorkCurveHelper.CalcType == CalcType.EC)//EC显示层级
                        {
                            WinMethod.SendMessage(DifferenceDevice.interClassMain.deviceMeasure.interfacce.OwnerHandle, DeviceInterface.CUSTOM_MESSAGE_HIDE, 0, 0);
                            var items = workCurveCurrent.ElementList.Items;
                            var eleSampleNames = from p in workCurveCurrent.ElementList.Items[0].Samples select p.SampleName;

                            FrmSelectLayer frmLayer = new FrmSelectLayer();
                            frmLayer.ShowDialog();
                            if (frmLayer.DialogResult != DialogResult.OK)//未选择单层/多层
                            { return; }
                            for (int i = 0; i < specListSelected.Count; i++)
                            {
                                var ele = workCurveCurrent.ElementList.Items[SelectDgvIndex];
                                //if (ele.Samples.ToList().Find(s => s.SampleName == specListSelected[i].Name) != null)//标样已存在
                                //{ continue; }
                                if (ele.Samples.ToList().Find(s => s.SampleName == specListSelected[i].Name) == null)//标样已存在
                                {
                                    try
                                    {
                                        workCurveCurrent.CaculateIntensity(specListSelected[i]);
                                    }
                                    catch (Exception ex)
                                    {
                                        SkyrayMsgBox.Show(specListSelected[i].SampleName + ex.Message);
                                        continue;
                                    }
                                    int TotalLayer = workCurveCurrent.ElementList.Items.OrderByDescending(data => data.LayerNumber).ToList<CurveElement>()[0].LayerNumber;//共有层数
                                    if (frmLayer.LayerNO == 1)//单层
                                    {
                                        int currentLayer = workCurveCurrent.ElementList.Items[SelectDgvIndex].LayerNumber;
                                        foreach (CurveElement ce in workCurveCurrent.ElementList.Items)//为元素添加标样
                                        {
                                            if (ce.LayerNumber == currentLayer)
                                            {
                                                double dblDensity = Atoms.AtomList.Find(w => w.AtomName.ToUpper().Equals(ce.Caption.ToUpper())).AtomDensity;
                                                var sam = StandSample.New.Init(specListSelected[i].Name, specListSelected[i].Height.ToString(), specListSelected[i].CalcAngleHeight.ToString(), ce.Intensity.ToString(), "100", "0", true, ce.Caption, 1, 2, Info.SingleLayer, dblDensity, "0");
                                                ce.Samples.Add(sam);
                                            }
                                        }
                                    }
                                    else if (frmLayer.LayerNO == 2)//多层
                                    {
                                        foreach (CurveElement ce in workCurveCurrent.ElementList.Items)//为元素添加标样
                                        {
                                            if (ce.LayerNumber < TotalLayer)
                                            {
                                                double dblDensity = Atoms.AtomList.Find(w => w.AtomName.ToUpper().Equals(ce.Caption.ToUpper())).AtomDensity;
                                                var sam = StandSample.New.Init(specListSelected[i].Name, specListSelected[i].Height.ToString(), specListSelected[i].CalcAngleHeight.ToString(), ce.Intensity.ToString(), "100", "0", true, ce.Caption, ce.LayerNumber, TotalLayer, Info.MultiLayer, dblDensity, "0");
                                                ce.Samples.Add(sam);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        workCurveCurrent.CaculateIntensity(specListSelected[i]);
                                    }
                                    catch (Exception ex)
                                    {
                                        SkyrayMsgBox.Show(specListSelected[i].SampleName + ex.Message);
                                        continue;
                                    }
                                    foreach (var item in workCurveCurrent.ElementList.Items)
                                    {
                                        //var itemTarget = item.Samples.First(x => x.SampleName == specListSelected[i].Name);
                                        //itemTarget.X = item.Intensity.ToString();
                                        Console.WriteLine(item.Caption + " " + item.Samples.Count.ToString());//加上这一句即可，原因暂不明！
                                        var itemTarget = item.Samples.ToList().Find(delegate(StandSample ss) { return ss.SampleName == specListSelected[i].Name; });
                                        if (itemTarget != null)
                                            itemTarget.X = item.Intensity.ToString();
                                    }
                                }
                            }


                            //var fff = from p in specListSelected
                            //          where eleSampleNames.Contains(p.Name)
                            //          select p;
                            //foreach (var specList in fff)
                            //{
                            //    try
                            //    {
                            //        workCurveCurrent.CaculateIntensity(specList);
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        Msg.Show(specList.SampleName + ex.Message);
                            //        continue;
                            //    }
                            //    foreach (var item in items)
                            //    {
                            //        var itemTarget = item.Samples.First(x => x.SampleName == specList.Name);
                            //        itemTarget.X = item.Intensity.ToString();
                            //    }
                            //}
                        }
                        else if (WorkCurveHelper.CalcType == CalcType.PeakDivBase)//扣背景
                        {
                            //var items = workCurveCurrent.ElementList.Items;
                            //var eleSampleNames = from p in workCurveCurrent.ElementList.Items[0].Samples select p.SampleName;

                            for (int i = 0; i < specListSelected.Count; i++)
                            {
                                var ele = workCurveCurrent.ElementList.Items[SelectDgvIndex];
                               // if (ele.Samples.ToList().Find(s => s.SampleName == specListSelected[i].Name) != null)//标样已存在
                                //{ continue; }
                                if (ele.Samples.ToList().Find(s => s.SampleName == specListSelected[i].Name) == null)//标样已存在
                                {
                                    try
                                    {
                                        workCurveCurrent.CaculateIntensity(specListSelected[i]);
                                    }
                                    catch (Exception ex)
                                    {
                                        SkyrayMsgBox.Show(specListSelected[i].SampleName + ex.Message);
                                        continue;
                                    }
                                    int TotalLayer = workCurveCurrent.ElementList.Items.OrderByDescending(data => data.LayerNumber).ToList<CurveElement>()[0].LayerNumber;//共有层数
                                    foreach (CurveElement ce in workCurveCurrent.ElementList.Items)//为元素添加标样
                                    {
                                        if (ce.LayerNumber < TotalLayer)
                                        {
                                            double dblDensity = Atoms.AtomList.Find(w => w.AtomName.ToUpper().Equals(ce.Caption.ToUpper())).AtomDensity;
                                            var sam = StandSample.New.Init(specListSelected[i].Name, specListSelected[i].Height.ToString(), specListSelected[i].CalcAngleHeight.ToString(), ce.Intensity.ToString(), "0", "0", true, ce.Caption, ce.LayerNumber, TotalLayer, "", dblDensity, "0");
                                            ce.Samples.Add(sam);
                                        }
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        workCurveCurrent.CaculateIntensity(specListSelected[i]);
                                    }
                                    catch (Exception ex)
                                    {
                                        SkyrayMsgBox.Show(specListSelected[i].SampleName + ex.Message);
                                        continue;
                                    }
                                    foreach (var item in workCurveCurrent.ElementList.Items)
                                    {
                                        //var itemTarget = item.Samples.First(x => x.SampleName == specListSelected[i].Name);
                                        //itemTarget.X = item.Intensity.ToString();
                                        Console.WriteLine(item.Caption + " " + item.Samples.Count.ToString());//加上这一句即可，原因暂不明！
                                        var itemTarget = item.Samples.ToList().Find(delegate(StandSample ss) { return ss.SampleName == specListSelected[i].Name; });
                                        if (itemTarget != null)
                                            itemTarget.X = item.Intensity.ToString();
                                    }
                                }
                            }
                         
                            
                        }
                        else 
                        {
                            var items = workCurveCurrent.ElementList.Items;
                            var eleSampleNames = from p in workCurveCurrent.ElementList.Items[0].Samples select p.SampleName;

                            var ddd = from p in specListSelected
                                      where eleSampleNames.Contains(p.Name) == false
                                      select p;
                            int TotalLayer = workCurveCurrent.ElementList.Items.OrderByDescending(data => data.LayerNumber).ToList<CurveElement>()[0].LayerNumber;//共有层数
                            foreach (var specList in ddd)
                            {
                                try
                                {
                                    workCurveCurrent.CaculateIntensity(specList);
                                }
                                catch (Exception ex)
                                {
                                    Msg.Show(specList.SampleName + ex.Message);
                                    continue;
                                }
                                foreach (var item in items)
                                {
                                    float con = 0;
                                    con = item.ContentUnit == ContentUnit.per ? 100 : 1000000;
                                    if (item.ContentUnit == ContentUnit.per)
                                    {
                                        con = 100;
                                    }
                                    else if (item.ContentUnit == ContentUnit.ppm)
                                    {
                                        con = 1000000;
                                    }
                                    else
                                    {
                                        con = 1000;
                                    }
                                    double dblDensity = Atoms.AtomList.Find(w => w.AtomName.ToUpper().Equals(item.Caption.ToUpper())).AtomDensity;
                                    var sam = StandSample.New.Init(specList.Name, specList.Height.ToString(), specList.CalcAngleHeight.ToString(), item.Intensity.ToString(), con.ToString(), "0", true, item.Caption, item.LayerNumber, TotalLayer, Info.MultiLayer, dblDensity, "0");
                                    item.Samples.Add(sam);
                                }
                            }
                            var fff = from p in specListSelected
                                      where eleSampleNames.Contains(p.Name)
                                      select p;
                            foreach (var specList in fff)
                            {
                                try
                                {
                                    workCurveCurrent.CaculateIntensity(specList);
                                }
                                catch (Exception ex)
                                {
                                    Msg.Show(specList.SampleName + ex.Message);
                                    continue;
                                }
                                foreach (var item in items)
                                {
                                    var itemTarget = item.Samples.First(x => x.SampleName == specList.Name);
                                    itemTarget.X = item.Intensity.ToString();
                                }
                            }
                        }
                    }
                    #endregion
                    else
                    {
                        var items = workCurveCurrent.ElementList.Items;
                        var eleSampleNames = from p in workCurveCurrent.ElementList.Items[0].Samples select p.SampleName;

                        var ddd = from p in specListSelected
                                  where eleSampleNames.Contains(p.Name) == false //orderby p.Name
                                  select p ;
                        foreach (var specList in ddd)
                        {
                            try
                            {
                                workCurveCurrent.CaculateIntensity(specList);
                            }
                            catch (Exception ex)
                            {
                                Msg.Show(specList.SampleName + ex.Message);
                                continue;
                            }
                            foreach (var item in items)
                            {
                                Console.WriteLine(item.Caption + " " + item.Samples.Count.ToString());//加上这一句即可，原因暂不明！
                                double dblDensity = Atoms.AtomList.Find(w => w.AtomName.ToUpper().Equals(item.Caption.ToUpper())).AtomDensity;
                                var sam = StandSample.New.Init(specList.Name, specList.Height.ToString(), specList.CalcAngleHeight.ToString(), item.Intensity.ToString(), "0", "0", true, item.Caption, 0, 0, "", dblDensity, "0");
                                item.Samples.Add(sam);
                            }
                        }

                        var fff = from p in specListSelected
                                  where eleSampleNames.Contains(p.Name) //orderby p.Name
                                  select p;
                        foreach (var specList in fff)
                        {
                            try
                            {
                                workCurveCurrent.CaculateIntensity(specList);
                            }
                            catch (Exception ex)
                            {
                                Msg.Show(specList.SampleName + ex.Message);
                                continue;
                            }
                            foreach (var item in items)
                            {
                                Console.WriteLine(item.Caption + " " + item.Samples.Count.ToString());//加上这一句即可，原因暂不明！
                                var itemTarget = item.Samples.ToList().Find(delegate(StandSample ss) { return ss.SampleName == specList.Name; });
                                if (itemTarget != null)
                                    itemTarget.X = item.Intensity.ToString();
                            }
                        }
                    }
                    InitStandSample(false);
                    WinMethod.SendMessage(DifferenceDevice.interClassMain.deviceMeasure.interfacce.OwnerHandle, DeviceInterface.CUSTOM_MESSAGE_HIDE, 0, 0);

                });
            });
          
            //Application.DoEvents();
        }

        /// <summary>
        /// 删除标准样品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dgwSample.SelectedRows.Count <= 0)
            { return; }
            if (WorkCurveHelper.DeviceFunctype == FuncType.Thick)//测厚
            {
                //string sampleName = workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[dgwSample.SelectedRows[0].Index].SampleName;
                string sampleName = this.dgwSample.SelectedRows[0].Cells[0].Value.ToString();
                foreach (var item in workCurveCurrent.ElementList.Items)
                {
                    StandSample ss = item.Samples.ToList().Find(s => s.SampleName == sampleName);
                    int index = item.Samples.IndexOf(ss);
                    if (ss != null)
                    {
                        item.Samples.RemoveAt(index);
                    }
                }
            }
            else
            {
                string samName = this.dgwSample.SelectedRows[0].Cells[0].Value.ToString();
                foreach (var item in workCurveCurrent.ElementList.Items)
                {
                    if (item.Samples.Count > dgwSample.SelectedRows[0].Index)
                    {
                        for (int i = 0; i < item.Samples.Count; i++)
                        {
                            if (item.Samples[i].SampleName == samName)
                            {
                                item.Samples.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }
            }
            //TranslateUnit();
            InitStandSample(false);//刷新标准样品
        }

        /// <summary>
        /// 选主元素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvwElement_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (WorkCurveHelper.DeviceFunctype == FuncType.Thick)
            { return; }
            if (e.RowIndex < 0)
            { return; }
            //int colindex = this.dgvwElement.Columns["ColMain"].Index; //非EC情况下，没有ColMain列，会导致空引用
            if (workCurveCurrent.CalcType == CalcType.EC)
            {
                //if (e.ColumnIndex == 1)
                if (e.ColumnIndex == this.dgvwElement.Columns["ColMain"].Index)
                {
                    mainElementSet(e.RowIndex);
                }
            }
        }

        /// <summary>
        /// 一次曲线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radACurve_CheckedChanged(object sender, EventArgs e)
        {
            if (radACurve.Checked)
            {
                workCurveCurrent.ElementList.Items[SelectDgvIndex].CalculationWay = CalculationWay.Linear;//设置计算方法

                this.grpOnes.Visible = (workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples.Count > 1) ? true : false;
                DrawGraph();//画图
            }
            else
            {
                this.grpOnes.Visible = false;
            }
        }

        /// <summary>
        /// 二次曲线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radConic_CheckedChanged(object sender, EventArgs e)
        {
            if (radConic.Checked)
            {
                workCurveCurrent.ElementList.Items[SelectDgvIndex].CalculationWay = CalculationWay.Conic;
                DrawGraph();//画图
            }
        }

        /// <summary>
        /// 含量校正
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radContentContect_CheckedChanged(object sender, EventArgs e)
        {
            //if (workCurveCurrent.ElementList.Items[SelectDgvIndex].Flag == ElementFlag.Internal) return;
            if (radContentContect.Checked)
            {
                grpOnes.Visible = false;
                this.btnCorrectCoee.Visible = true;
                workCurveCurrent.ElementList.Items[SelectDgvIndex].CalculationWay = CalculationWay.ContentContect;
                string elementName = workCurveCurrent.ElementList.Items[SelectDgvIndex].Caption;
                double[] dIntensity;
                try
                {
                    if (workCurveCurrent.GetCalibrateContent(elementName, out dIntensity))
                    {
                        for (int i = 0; i < workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples.Count; i++)
                        {
                            workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].TheoryX = dIntensity[i];
                        }
                        CalLinearParam();//计算斜率和截距
                        DrawGraph();//画图
                    }
                }
                catch (Exception ex)
                {
                    SkyrayMsgBox.Show(ex.Message);
                }
            }
            else
            {
                this.btnCorrectCoee.Visible = false;
            }
        }

        /// <summary>
        /// 强度校正
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radIntensityCorrect_CheckedChanged(object sender, EventArgs e)
        {
            //if (workCurveCurrent.ElementList.Items[SelectDgvIndex].Flag == ElementFlag.Internal) return;
            if (radIntensityCorrect.Checked)
            {
                grpOnes.Visible = false;
                this.btnCorrectCoee.Visible = true;
                workCurveCurrent.ElementList.Items[SelectDgvIndex].CalculationWay = CalculationWay.IntensityCorrect;
                string elementName = workCurveCurrent.ElementList.Items[SelectDgvIndex].Caption;
                double[] dIntensity;
                try
                {
                    if (workCurveCurrent.GetCalibrateIntensity(elementName, out dIntensity))
                    {
                        for (int i = 0; i < workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples.Count; i++)
                        {
                            workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].TheoryX = dIntensity[i];
                        }
                        CalLinearParam();//计算斜率和截距
                        DrawGraph();//画图
                    }
                }
                catch (Exception ex)
                {
                    SkyrayMsgBox.Show(ex.Message);
                }
            }
            else
            {
                this.btnCorrectCoee.Visible = false;
            }
        }

        /// <summary>
        /// 插值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radInsert_CheckedChanged(object sender, EventArgs e)
        {
            if (radInsert.Checked)
            {
                grpOnes.Visible = false;
                workCurveCurrent.ElementList.Items[SelectDgvIndex].CalculationWay = CalculationWay.Insert;
                DrawGraph();//画图
            }
        }

        /// <summary>
        /// 校正系数按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCorrectCoee_Click(object sender, EventArgs e)
        {
            UCRef uc = new UCRef(true);
            WorkCurveHelper.OpenUC(uc, false, Info.ElementRef,true);
            if (uc.DialogResult == DialogResult.Yes)
            {
                #region
                //if (radIntensityCorrect.Checked)//强度校正
                //{
                //    if (workCurveCurrent.GetCalibrateIntensity(elementName, out dIntensity))
                //    {
                //        for (int i = 0; i < workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples.Count; i++)
                //        {
                //            workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].TheoryX = dIntensity[i];
                //        }
                //        //CalLinearParam();
                //        //DrawGraph();
                //    }
                //}
                //else if (radContentContect.Checked)//含量校正
                //{
                //    if (workCurveCurrent.GetCalibrateContent(elementName, out dIntensity))
                //    {
                //        for (int i = 0; i < workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples.Count; i++)
                //        {
                //            workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].TheoryX = dIntensity[i];
                //        }
                //        //CalLinearParam();
                //        //DrawGraph();
                //    }
                //} 
                #endregion
            }
        }

        /// <summary>
        /// 选择元素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabcwEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabcwEdit.SelectedIndex == 1)
            {
                SelectDgvIndex = dgvwElement.SelectedRows[0].Index;
                InitCalculationWay();//选择计算方式
                //DrawGraph();
            }
            
        }

        /// <summary>
        /// 设置含量单位
        /// </summary>
        /// <param name="index"></param>
        private void SetContentUnit(int index)
        {
            string unit = "";
            if (WorkCurveHelper.CalcType != CalcType.PeakDivBase)
            {
                if (workCurveCurrent.ElementList.Items[index].ContentUnit == ContentUnit.per)
                {
                    unit = "(%)";
                }
                else if (workCurveCurrent.ElementList.Items[index].ContentUnit == ContentUnit.ppm)
                {
                    unit = "(ppm)";
                }
                else
                {
                    unit = "(‰)";
                    EditContent.Width = 120;
                }
                EditContent.HeaderText = Info.EditContent + unit;
                dgvCContent.HeaderText = Info.EditContent + unit;
            }
            else
            {
               // unit = workCurveCurrent.ElementList.Items[index].ThicknessUnit == ThicknessUnit.um ? "(um)" : "(u〞)";
                if (workCurveCurrent.ElementList.Items[index].ThicknessUnit == ThicknessUnit.um)
                {
                    unit = "(um)";
                }
                else if (workCurveCurrent.ElementList.Items[index].ThicknessUnit == ThicknessUnit.ur)
                {
                    unit = "(u〞)";
                }
                else
                {
                    unit = "(g/L)";
                }
                EditContent.HeaderText = Info.Thick + unit;
                dgvCContent.HeaderText = Info.Thick + unit;
            }
            
           // string ness = workCurveCurrent.ElementList.Items[index].ThicknessUnit == ThicknessUnit.um ? "(um)" : "(u〞)";
            string ness = string.Empty;
            if (workCurveCurrent.ElementList.Items[index].ThicknessUnit == ThicknessUnit.um)
            {
                ness = "(um)";
            }
            else if (workCurveCurrent.ElementList.Items[index].ThicknessUnit == ThicknessUnit.ur)
            {
                ness = "(u〞)";
            }
            else
            {
                ness = "(g/L)";
            }
            ColThickness.HeaderText = Info.Thick + ness;
            dgvThickCol.HeaderText = Info.Thick + ness;
            ColumnUncertainty.HeaderText = Info.Uncertainty+"(%)";
        }

        /// <summary>
        /// 选择元素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvwElement_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            SelectDgvIndex = e.RowIndex;
            SetContentUnit(SelectDgvIndex);
            //TranslateUnit();
            InitStandSample(false);
            SetSeleAll();
            //if (WorkCurveHelper.DeviceFunctype == FuncType.XRF)
            //{
            //    if (workCurveCurrent.CalcType == CalcType.EC) InitCalculationWay();
            //}
            //if (workCurveCurrent.CalcType == CalcType.EC) 

            if (tabcwEdit.SelectedIndex == 1)
            {
                InitCalculationWay();
            }
            //DrawGraph();//画图
        }

        /// <summary>
        /// 保存标准样品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            //if (JudgeMatchAndFile())
            //{
            //    SkyrayMsgBox.Show(Info.strJudgeMatchAndFile);
            //    return;
            //}

            if (ExitLevel()) return;
            int result = CheckMainSample();
            if (result == 1)
            {
                Msg.Show(Info.strIntensityIsNull);
                return;
            }
            else if (result == 2)
            {
                Msg.Show(Info.strContentIsNull);
                return;
            }
            TranslateUnit();
            //DifferenceDevice.TransSpecDataForRemoveBg(workCurveCurrent);
           
            workCurveCurrent.Save();
            WorkCurveHelper.WorkCurveCurrent = workCurveCurrent;
            if (this.ParentForm != null)
                this.ParentForm.DialogResult = this.dialogResult = DialogResult.OK;
            EDXRFHelper.GotoMainPage(this);//返回主界面

        }

        /// <summary>
        /// 比较是否参与匹配和匹配文件是否存在相同  20110518 paul
        /// </summary>
        /// <returns></returns>
        //private bool JudgeMatchAndFile()
        //{
        //    bool bSucceed = false;
        //    foreach (var item in workCurveCurrent.ElementList.Items)
        //        {
        //            if (item.Samples.ToList().Exists(delegate(StandSample v) { return (v.IsMatch == true && string.IsNullOrEmpty(v.MatchSpecName) || (v.IsMatch == false && !string.IsNullOrEmpty(v.MatchSpecName))); }))
        //            {
        //                bSucceed = true;
        //                return bSucceed;
        //            }
        //        }

        //    return bSucceed;
        //}

        /// <summary>
        /// 鼠标在画图控件移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xrfEdit_MouseMove(object sender, MouseEventArgs e)
        {
            //if (workCurveCurrent.CalcType == CalcType.FP) return;
            if (workCurveCurrent.ElementList.Items[SelectDgvIndex].CalculationWay != CalculationWay.Insert && workCurveCurrent.ElementList.Items[SelectDgvIndex].CalculationWay != CalculationWay.Linear && workCurveCurrent.ElementList.Items[SelectDgvIndex].CalculationWay != CalculationWay.Conic)
            { return; }
            bool bl = false;
            float scaleFactor = xrfEdit.GraphPane.CalcScaleFactor();
            float width = 8f * scaleFactor;
            float num2 = width / 2f;
            if (lstActiveSample == null || lstActiveppl == null || lstActiveSample.Count == 0 || lstActiveppl.Count == 0)
            {
                return;
            }
            ElementList.IsPKCatchValue = false;
            //数据显示错误的修改。
            List<StandSample> SamplesTemp = new List<StandSample>();
            
            for (int i = 0; i < lstActiveSample.Count; i++)
            {
                for (int j = 0; j < lstActiveppl[i].Count; j++)
                {
                    float a = xrfEdit.GraphPane.XAxis.Scale.Transform(lstActiveppl[i][j].X);
                    float b = xrfEdit.GraphPane.YAxis.Scale.Transform(lstActiveppl[i][j].Y);
                    if ((((a - e.X) * (a - e.X)) < num2 * num2) && (((b - e.Y) * (b - e.Y)) < num2 * num2) && !radThickInsert.Checked && (WorkCurveHelper.DeviceFunctype != FuncType.Thick || (WorkCurveHelper.DeviceFunctype == FuncType.Thick && WorkCurveHelper.WorkCurveCurrent.CalcType==CalcType.FP)))//显示数据
                    {
                        if (workCurveCurrent.CalcType == CalcType.EC)
                        {
                            workCurveCurrent.GetECStandSamplesContent(workCurveCurrent.ElementList, SelectDgvIndex, out SamplesTemp, null);
                        }
                        this.grp.Visible = true;
                        this.grp.BringToFront();
                        double TheoryValue = 0;
                        lblSampleName.Text = lstActiveSample[i].SampleName;
                        if (workCurveCurrent.CalcType == CalcType.FP)
                        {
                            if (radIntensityVS.Checked)
                            {
                                lblTrue.Text = "MI:";
                                lblTheory.Text = "TI:";
                            }
                            else
                            {
                                lblTrue.Text = "FC:"; 
                                lblTheory.Text = "CC:";
                            }
                            lblTrueValue.Text = lstActiveppl[i][0].X.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
                            lblTheoryValue.Text = lstActiveppl[i][0].Y.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
                            lblErrorValue.Text = (lstActiveppl[i][0].X - lstActiveppl[i][0].Y).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
                        }
                        else
                        {
                            lblTrueValue.Text = double.Parse(lstActiveSample[i].Y).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());//真实值
                            //if (workCurveCurrent.ElementList.Items[SelectDgvIndex].CalculationWay == CalculationWay.Linear)
                            //{
                            //    TheoryValue = double.Parse(lstActiveSample[i].X) * (Slope + workCurveCurrent.ElementList.Items[SelectDgvIndex].SlopeOptimalFactor) + Intercept;
                            //}
                            //else if (workCurveCurrent.ElementList.Items[SelectDgvIndex].CalculationWay == CalculationWay.Conic)
                            //{
                            //    if (xrfEdit.Coeff != null)
                            //        TheoryValue = xrfEdit.Coeff[0] * double.Parse(lstActiveSample[i].X) * double.Parse(lstActiveSample[i].X) + xrfEdit.Coeff[1] * double.Parse(lstActiveSample[i].X) + xrfEdit.Coeff[2];
                            //}
                            //else if (workCurveCurrent.ElementList.Items[SelectDgvIndex].CalculationWay == CalculationWay.Insert)
                            //{
                            //    TheoryValue = double.Parse(lstActiveSample[i].Y);
                            //}
                            string sampleName = lstActiveSample[i].SampleName;
                            TheoryValue = SamplesTemp.Find(w => w.SampleName == sampleName).TheoryX;
                            lblTheoryValue.Text = TheoryValue.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());//理论值
                            lblErrorValue.Text = (double.Parse(lstActiveSample[i].Y) - TheoryValue).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
                        }
                        grp.Location = new Point(e.X + tabcwEdit.Location.X + xrfEdit.Location.X - grp.Width + 2, e.Y - 2);

                        bl = true;
                        xrfEdit.Refresh();
                        break;
                    }
                }
                if (bl)
                {
                    break;
                }
            }
            for (int i = 0; i < lstNoActiveSample.Count; i++)
            {
                for (int j = 0; j < lstNoActiveppl[i].Count; j++)
                {
                    float a = xrfEdit.GraphPane.XAxis.Scale.Transform(lstNoActiveppl[i][j].X);
                    float b = xrfEdit.GraphPane.YAxis.Scale.Transform(lstNoActiveppl[i][j].Y);
                    if ((((a - e.X) * (a - e.X)) < num2 * num2) && (((b - e.Y) * (b - e.Y)) < num2 * num2) && !radThickInsert.Checked && (WorkCurveHelper.DeviceFunctype != FuncType.Thick || (WorkCurveHelper.DeviceFunctype == FuncType.Thick && WorkCurveHelper.WorkCurveCurrent.CalcType == CalcType.FP)))//显示数据
                    {
                        if (workCurveCurrent.CalcType == CalcType.FP)
                        {
                            this.grp.Visible = true;
                            this.grp.BringToFront();
                            lblSampleName.Text = lstNoActiveSample[i].SampleName;
                            if (radIntensityVS.Checked)
                            {
                                lblTrue.Text = "MI:";
                                lblTheory.Text = "TI:";
                            }
                            else
                            {
                                lblTrue.Text = "FC:";
                                lblTheory.Text = "CC:";
                            }
                            lblTrueValue.Text = lstNoActiveppl[i][0].X.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
                            lblTheoryValue.Text = lstNoActiveppl[i][0].Y.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
                            lblErrorValue.Text = (lstNoActiveppl[i][0].X - lstNoActiveppl[i][0].Y).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
                        }
                        else
                        {
                            if (workCurveCurrent.CalcType == CalcType.EC)
                            {
                                workCurveCurrent.GetECStandSamplesContent(workCurveCurrent.ElementList, SelectDgvIndex, out SamplesTemp, null);
                            }
                            lblTrueValue.Text = double.Parse(lstNoActiveSample[i].Y).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());//真实值
                            //if (workCurveCurrent.ElementList.Items[SelectDgvIndex].CalculationWay == CalculationWay.Linear)
                            //{
                            //    TheoryValue = double.Parse(lstActiveSample[i].X) * (Slope + workCurveCurrent.ElementList.Items[SelectDgvIndex].SlopeOptimalFactor) + Intercept;
                            //}
                            //else if (workCurveCurrent.ElementList.Items[SelectDgvIndex].CalculationWay == CalculationWay.Conic)
                            //{
                            //    if (xrfEdit.Coeff != null)
                            //        TheoryValue = xrfEdit.Coeff[0] * double.Parse(lstActiveSample[i].X) * double.Parse(lstActiveSample[i].X) + xrfEdit.Coeff[1] * double.Parse(lstActiveSample[i].X) + xrfEdit.Coeff[2];
                            //}
                            //else if (workCurveCurrent.ElementList.Items[SelectDgvIndex].CalculationWay == CalculationWay.Insert)
                            //{
                            //    TheoryValue = double.Parse(lstActiveSample[i].Y);
                            //}
                            string sampleName = lstNoActiveSample[i].SampleName;
                            double TheoryValue = SamplesTemp.Find(w => w.SampleName == sampleName).TheoryX;
                            lblTheoryValue.Text = TheoryValue.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());//理论值
                            lblErrorValue.Text = (double.Parse(lstActiveSample[i].Y) - TheoryValue).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
                        }
                
                        grp.Location = new Point(e.X + tabcwEdit.Location.X + xrfEdit.Location.X - grp.Width + 2, e.Y - 2);

                        bl = true;
                        xrfEdit.Refresh();
                        break;
                    }
                }
                if (bl)
                {
                    break;
                }
            }
            if (!bl)
            {
                this.grp.Visible = false;
            }
        }

        /// <summary>
        /// 点击画图控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xrfEdit_MouseClick(object sender, MouseEventArgs e)
        {
            if (WorkCurveHelper.DeviceFunctype == FuncType.Thick)
            {
                return;
            }
            if (lstActiveSample == null || lstActiveppl == null)
            { return; }
            float scaleFactor = xrfEdit.GraphPane.CalcScaleFactor();
            float width = 8f * scaleFactor;
            float num2 = width / 2f;
            for (int i = 0; i < lstActiveSample.Count; i++)
            {
                for (int j = 0; j < lstActiveppl[i].Count; j++)
                {
                    float a = xrfEdit.GraphPane.XAxis.Scale.Transform(lstActiveppl[i][j].X);
                    float b = xrfEdit.GraphPane.YAxis.Scale.Transform(lstActiveppl[i][j].Y);
                    if ((((a - e.X) * (a - e.X)) < num2 * num2) && (((b - e.Y) * (b - e.Y)) < num2 * num2))
                    {
                        FindSample(lstActiveSample[i], false);
                        CalLinearParam();//计算斜率和截距
                        DrawGraph();//画图
                        xrfEdit.Refresh();
                        return;
                    }
                }
            }
            for (int i = 0; i < lstNoActiveSample.Count; i++)
            {
                for (int j = 0; j < lstNoActiveppl[i].Count; j++)
                {
                    float a = xrfEdit.GraphPane.XAxis.Scale.Transform(lstNoActiveppl[i][j].X);
                    float b = xrfEdit.GraphPane.YAxis.Scale.Transform(lstNoActiveppl[i][j].Y);
                    if ((((a - e.X) * (a - e.X)) < num2 * num2) && (((b - e.Y) * (b - e.Y)) < num2 * num2))
                    {
                        FindSample(lstNoActiveSample[i], true);
                        CalLinearParam();//计算斜率和截距
                        DrawGraph();
                        xrfEdit.Refresh();
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 数据错误处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgwSample_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            SkyrayMsgBox.Show(Info.DataError);
        }

        /// <summary>
        /// 数据验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgwSample_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgwSample.Columns[e.ColumnIndex].Name.Equals("EditContent") && WorkCurveHelper.DeviceFunctype != FuncType.Thick)
            {
                //decimal Max = Ranges.RangeDictionary[dgwSample.Columns[e.ColumnIndex].Name].Max;
                //decimal Min = Ranges.RangeDictionary[dgwSample.Columns[e.ColumnIndex].Name].Min;
                double Max = double.Parse(Ranges.RangeDictionary[dgwSample.Columns[e.ColumnIndex].Name].Max.ToString());
                double Min = double.Parse(Ranges.RangeDictionary[dgwSample.Columns[e.ColumnIndex].Name].Min.ToString());
                if (workCurveCurrent.ElementList.Items[SelectDgvIndex].ContentUnit == ContentUnit.per)
                {
                    //Max = Max;
                }
                else if (workCurveCurrent.ElementList.Items[SelectDgvIndex].ContentUnit == ContentUnit.ppm)
                {
                    Max = Max * 10000;
                }
                else
                {
                    Max = Max * 10;
                }
                try { double.Parse(e.FormattedValue.ToString()); }
                catch (Exception)
                {
                    dgwSample[e.ColumnIndex, e.RowIndex].Value = Min;
                    e.Cancel = true;
                    return;
                }
                if (double.Parse(e.FormattedValue.ToString()) > Max)
                {
                    dgwSample[e.ColumnIndex, e.RowIndex].Value = Max;
                    e.Cancel = true;
                    return;
                }
                if (double.Parse(e.FormattedValue.ToString()) < Min)
                {
                    dgwSample[e.ColumnIndex, e.RowIndex].Value = Min;
                    e.Cancel = true;
                }
                return;
            }
            if (dgwSample.Columns[e.ColumnIndex].Name.Equals("EditIntensity"))
            {
                double Max = double.Parse(Ranges.RangeDictionary[dgwSample.Columns[e.ColumnIndex].Name].Max.ToString());
                double Min = double.Parse(Ranges.RangeDictionary[dgwSample.Columns[e.ColumnIndex].Name].Min.ToString());
                try { double.Parse(e.FormattedValue.ToString()); }
                catch (Exception)
                {
                    dgwSample[e.ColumnIndex, e.RowIndex].Value = Min;
                    e.Cancel = true;
                    return;
                }
                if (double.Parse(e.FormattedValue.ToString()) > Max)
                {
                    dgwSample[e.ColumnIndex, e.RowIndex].Value = Max;
                    e.Cancel = true;
                    return;
                }
                if (double.Parse(e.FormattedValue.ToString()) < Min)
                {
                    dgwSample[e.ColumnIndex, e.RowIndex].Value = Min;
                    e.Cancel = true;
                }
                return;
            }
            else if (e.ColumnIndex == ColumnUncertainty.Index)
            {
                try { double.Parse(e.FormattedValue.ToString()); }
                catch {
                      dgwSample[e.ColumnIndex, e.RowIndex].Value = "0";
                      e.Cancel = true; 
                      }
                return;
            }
        }

        /// <summary>
        /// 点击取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        { 
            EDXRFHelper.GotoMainPage(this);//返回主界面
        }

        #endregion

        #region Methods

        private void TranslateUnit()
        {
            try
            {

                foreach (var element in workCurveCurrent.ElementList.Items)
                {
                    if (element.ContentUnit == ContentUnit.ppm)
                    {
                        foreach (var sample in element.Samples)
                        {
                            sample.Y = (double.Parse(sample.Y) / 10000.0).ToString();
                        }
                    }
                    if (element.ContentUnit == ContentUnit.permillage)
                    {
                        foreach (var sample in element.Samples)
                        {
                            sample.Y = (double.Parse(sample.Y) / 10.0).ToString();
                        }
                    }
                    if (element.ThicknessUnit == ThicknessUnit.ur)
                    {
                        foreach (var sample in element.Samples)
                        {
                            sample.Z = (double.Parse(sample.Z) * 0.0254).ToString();
                        }
                    }
                    foreach (var sample in element.Samples)
                    {
                        sample.TheoryX = 0;
                    }
                    element.Intensity = 0;
                    element.Content = 0;
                    element.Error = 0;
                }
            }
            catch (Exception ex)
            {
                Msg.Show(ex.Message);
            }
        }

        /// <summary>
        /// 设置主元素
        /// </summary>
        /// <param name="rowIndex"></param>
        private void mainElementSet(int rowIndex)
        {
            int colindex = this.dgvwElement.Columns["ColMain"].Index;
            //if (this.dgvwElement.Rows[rowIndex].Cells[1].Value == null)
            if (this.dgvwElement.Rows[rowIndex].Cells[colindex].Value == null)
            {
                var xnull = workCurveCurrent.ElementList.Items[rowIndex].Samples.ToList().Find(s => s.X == "0");
                if (xnull != null)
                {
                    InitElement(false);
                    this.dgvwElement.Rows[rowIndex].Selected = true;
                    Msg.Show(Info.strIntensityIsNull);
                    return;
                }
                var ynull = workCurveCurrent.ElementList.Items[rowIndex].Samples.ToList().Find(s => s.Y == "0");
                if (ynull != null)
                {
                    InitElement(false);
                    this.dgvwElement.Rows[rowIndex].Selected = true;
                    Msg.Show(Info.strContentIsNull);
                    return;
                }
            }
            //if (this.dgvwElement.Rows[rowIndex].Cells[1].Value != null)
             if (this.dgvwElement.Rows[rowIndex].Cells[colindex].Value != null)
            {
                //if (!this.dgvwElement.Rows[rowIndex].Cells[1].Value.Equals(true))
                if (!this.dgvwElement.Rows[rowIndex].Cells[colindex].Value.Equals(true))
                {
                    for (int i = 0; i < this.dgvwElement.RowCount; i++)
                    {
                        //this.dgvwElement.Rows[i].Cells[1].Value = false;
                        this.dgvwElement.Rows[i].Cells[colindex].Value = false;
                        workCurveCurrent.ElementList.Items[i].Flag = ElementFlag.Calculated;
                    }
                    //this.dgvwElement.Rows[rowIndex].Cells[1].Value = true;
                    this.dgvwElement.Rows[rowIndex].Cells[colindex].Value = true;
                    workCurveCurrent.ElementList.Items[rowIndex].Flag = ElementFlag.Internal;
                }
                else
                {
                    //this.dgvwElement.Rows[rowIndex].Cells[1].Value = false;
                    this.dgvwElement.Rows[rowIndex].Cells[colindex].Value = false;
                    workCurveCurrent.ElementList.Items[rowIndex].Flag = ElementFlag.Calculated;
                }
            }
            else
            {
                for (int i = 0; i < this.dgvwElement.RowCount; i++)
                {
                    //this.dgvwElement.Rows[i].Cells[1].Value = false;
                    this.dgvwElement.Rows[i].Cells[colindex].Value = false;
                    workCurveCurrent.ElementList.Items[i].Flag = ElementFlag.Calculated;
                }
                //this.dgvwElement.Rows[rowIndex].Cells[1].Value = true;
                this.dgvwElement.Rows[rowIndex].Cells[colindex].Value = true;
                workCurveCurrent.ElementList.Items[rowIndex].Flag = ElementFlag.Internal;
            }
            DrawGraph();//画图
        }

        /// <summary>
        /// 找到对应样品
        /// </summary>
        /// <param name="ss"></param>
        /// <param name="flag"></param>
        private void FindSample(StandSample ss, bool flag)
        {
            if (workCurveCurrent.CalcType == CalcType.FP)
            {
                foreach (CurveElement ce in workCurveCurrent.ElementList.Items)
                {
                    foreach (var sSa in ce.Samples)
                    {
                        if (sSa.SampleName.Equals(ss.SampleName))
                            sSa.Active = flag;
                    }
                }
            }
            else
            {
                foreach (StandSample sample in workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples)
                {
                    if (ss.SampleName.Equals(sample.SampleName))
                    {
                        sample.Active = flag;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 计算斜率和截距
        /// </summary>
        private void CalLinearParam()
        {
            Intercept = 0;
            Slope = 0;
            double x = 0;
            double y = 0;
            double xy = 0;
            double xx = 0;
            int count = 0;
            if (workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples.Count == 0)
            {
                return;
            }
            CurveElement ce = workCurveCurrent.ElementList.Items.ToList().Find(f => f.Flag == ElementFlag.Internal);
            for (int i = 0; i < workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples.Count; i++)
            {
                if (workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].Active)
                {
                    if (ce != null)
                    {
                        StandSample ss = ce.Samples.ToList().Find(c => c.SampleName == workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].SampleName);//主元素同名标样
                        if (radACurve.Checked)//一次曲线
                        {
                            x += double.Parse(workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].X) / double.Parse(ss.X);
                            y += double.Parse(workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].Y) / double.Parse(ss.Y);
                            xy += (double.Parse(workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].X) / double.Parse(ss.X)) * (double.Parse(workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].Y) / double.Parse(ss.Y));
                            xx += (double.Parse(workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].X) / double.Parse(ss.X)) * (double.Parse(workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].X) / double.Parse(ss.X));
                        }
                        else
                        {
                            x += workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].TheoryX;
                            y += double.Parse(workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].Y) / double.Parse(ss.Y);
                            xy += (workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].TheoryX) * (double.Parse(workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].Y) / double.Parse(ss.Y));
                            xx += (workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].TheoryX) * (workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].TheoryX);
                        }
                    }
                    else
                    {
                        if (radACurve.Checked)//一次曲线
                        {
                            x += double.Parse(workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].X);
                            y += double.Parse(workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].Y);
                            xy += double.Parse(workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].X) * double.Parse(workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].Y);
                            xx += double.Parse(workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].X) * double.Parse(workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].X);
                        }
                        else// if (radIntensityCorrect.Checked)//校正
                        {
                            x += workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].TheoryX;
                            y += double.Parse(workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].Y);
                            xy += workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].TheoryX * double.Parse(workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].Y);
                            xx += workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].TheoryX * workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].TheoryX;
                        }
                    }
                    count++;
                }
            }
            if ((count * xx - x * x) != 0)
            {
                Slope = (count * xy - x * y) / (count * xx - x * x);
                Intercept = (y - Slope * x) / count;
            }
        }

        /// <summary>
        /// 画图
        /// </summary>
        private void DrawGraph()
        {
            if (workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples.Count == 0)
            {
                return;
            }
            lstActiveSample = new List<StandSample>();
            lstNoActiveSample = new List<StandSample>();
            for (int i = 0; i < workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples.Count; i++)
            {
                if (workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].Active)
                {
                    lstActiveSample.Add(workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i]);
                }
                else
                {
                    lstNoActiveSample.Add(workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i]);
                }
            }
            if (WorkCurveHelper.DeviceFunctype == FuncType.XRF && WorkCurveHelper.WorkCurveCurrent.CalcType == CalcType.EC)
                Draw();
            else if (WorkCurveHelper.DeviceFunctype == FuncType.Thick && WorkCurveHelper.WorkCurveCurrent.CalcType == CalcType.EC)
                DrawThick();
            else if (WorkCurveHelper.DeviceFunctype == FuncType.XRF && WorkCurveHelper.WorkCurveCurrent.CalcType == CalcType.FP)
                DrawFpCurve();
            else if (WorkCurveHelper.DeviceFunctype == FuncType.Thick && WorkCurveHelper.WorkCurveCurrent.CalcType == CalcType.FP)
                DrawFpCurve();
            else
                Draw();
            this.xrfEdit.AxisChange();
            this.xrfEdit.Refresh();
        }
        #region 修改XY轴的信息 begin-------
        ///// <summary>
        ///// 根据不同曲线类型画图
        ///// </summary>
        //private void Draw()
        //{
        //    pScalePoints = new PointF[2];
        //    List<StandSample> lsTemp = workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples.OrderByDescending(data => double.Parse(data.X)).ToList<StandSample>();
        //    double minX = 0;
        //    double maxX = 0;
        //    double minY = 0;
        //    double maxY = 0;
        //    xrfEdit.IsMainElement = false;
        //    CurveElement ce = workCurveCurrent.ElementList.Items.ToList().Find(f => f.Flag == ElementFlag.Internal);
        //    if (ce != null)//选择了主元素
        //    {
        //        if (ce.Caption == workCurveCurrent.ElementList.Items[SelectDgvIndex].Caption)
        //        {
        //            xrfEdit.IsMainElement = true;
        //        }
        //        else
        //        {
        //            PointF[] tempPoints = new PointF[lsTemp.Count];
        //            if (radIntensityCorrect.Checked || radContentContect.Checked)
        //            {
        //                for (int i = 0; i < tempPoints.Length; i++)
        //                {
        //                    StandSample standi = ce.Samples.ToList().Find(c => c.SampleName == lsTemp[i].SampleName);//和主元素同名标样
        //                    if (standi == null)
        //                    { tempPoints[i] = new PointF(0F, 0F); }
        //                    else
        //                    {
        //                        //tempPoints[i] = new PointF(float.Parse(lsTemp[i].TheoryX.ToString()), float.Parse((lsTemp[i].Y / standi.Y).ToString()));
        //                        tempPoints[i] = new PointF(float.Parse(lsTemp[i].TheoryX.ToString()), float.Parse(lsTemp[i].Y) / float.Parse(standi.Y));
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                for (int i = 0; i < tempPoints.Length; i++)
        //                {
        //                    StandSample standi = ce.Samples.ToList().Find(c => c.SampleName == lsTemp[i].SampleName);//和主元素同名标样

        //                    if (standi == null)
        //                    { tempPoints[i] = new PointF(0F, 0F); }
        //                    else
        //                    {
        //                        //tempPoints[i] = new PointF(float.Parse((lsTemp[i].X / standi.X).ToString()), float.Parse((lsTemp[i].Y / standi.Y).ToString()));
        //                        tempPoints[i] = new PointF(float.Parse(lsTemp[i].X) / float.Parse(standi.X), float.Parse(lsTemp[i].Y) / float.Parse(standi.Y));
        //                    }
        //                }
        //            }
        //            minX = tempPoints.OrderBy(data => data.X).ToList()[0].X;
        //            maxX = tempPoints.OrderByDescending(data => data.X).ToList()[0].X;

        //            minY = tempPoints.OrderBy(data => data.Y).ToList()[0].Y;
        //            maxY = tempPoints.OrderByDescending(data => data.Y).ToList()[0].Y;
        //        }
        //    }
        //    else
        //    {
        //        if (radIntensityCorrect.Checked || radContentContect.Checked)
        //        {
        //            lsTemp = workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples.OrderByDescending(data => data.TheoryX).ToList<StandSample>();
        //            minX = lsTemp[lsTemp.Count - 1].TheoryX;
        //            maxX = lsTemp[0].TheoryX;
        //        }
        //        else
        //        {
        //            minX = double.Parse(lsTemp[lsTemp.Count - 1].X);
        //            maxX = double.Parse(lsTemp[0].X);
        //        }
        //        minY = double.Parse(lsTemp.OrderBy(data => double.Parse(data.Y)).ToList()[0].Y);
        //        maxY = double.Parse(lsTemp.OrderByDescending(data => double.Parse(data.Y)).ToList()[0].Y);
        //    }
        //    pScalePoints[0] = new PointF(float.Parse(minX.ToString()), float.Parse(minY.ToString()));
        //    pScalePoints[1] = new PointF(float.Parse(maxX.ToString()), float.Parse(maxY.ToString()));

        //    xrfEdit.GraphPane.CurveList.Clear();
        //    lstActiveSample = lstActiveSample.OrderByDescending(data => double.Parse(data.X)).ToList<StandSample>();
        //    lstActiveppl = new List<PointPairList>();
        //    pfarr = new PointF[lstActiveSample.Count];

        //    for (int i = 0; i < lstActiveSample.Count; i++)
        //    {
        //        PointPairList ppl = new PointPairList();
        //        LineItem li = xrfEdit.GraphPane.AddCurve("", ppl, Color.Red, SymbolType.Square);
        //        li.Symbol.Fill = new Fill(Color.Red);
        //        li.Symbol.Size = 8f;
        //        if (ce != null)
        //        {
        //            StandSample ss = ce.Samples.ToList().Find(c => c.SampleName == lstActiveSample[i].SampleName);//和主元素同名标样
        //            if ((radIntensityCorrect.Checked || radContentContect.Checked) && !xrfEdit.IsMainElement)
        //            {
        //                ppl.Add(lstActiveSample[i].TheoryX, double.Parse(lstActiveSample[i].Y) / double.Parse(ss.Y));
        //                lstActiveppl.Add(ppl);
        //                pfarr[i] = new PointF(float.Parse(lstActiveSample[i].TheoryX.ToString()), float.Parse(lstActiveSample[i].Y) / float.Parse(ss.Y));
        //            }
        //            else if (ss!=null)
        //            {
        //                ppl.Add(double.Parse(lstActiveSample[i].X) / double.Parse(ss.X), double.Parse(lstActiveSample[i].Y) / double.Parse(ss.Y));
        //                lstActiveppl.Add(ppl);
        //                pfarr[i] = new PointF(float.Parse(lstActiveSample[i].X) / float.Parse(ss.X), float.Parse(lstActiveSample[i].Y) / float.Parse(ss.Y));
        //            }
        //        }
        //        else
        //        {
        //            if ((radIntensityCorrect.Checked || radContentContect.Checked) && !xrfEdit.IsMainElement)
        //            {
        //                ppl.Add(lstActiveSample[i].TheoryX, double.Parse(lstActiveSample[i].Y));
        //                lstActiveppl.Add(ppl);
        //                pfarr[i] = new PointF(float.Parse(lstActiveSample[i].TheoryX.ToString()), float.Parse(lstActiveSample[i].Y.ToString()));
        //            }
        //            else
        //            {
        //                ppl.Add(double.Parse(lstActiveSample[i].X), double.Parse(lstActiveSample[i].Y));
        //                lstActiveppl.Add(ppl);
        //                pfarr[i] = new PointF(float.Parse(lstActiveSample[i].X.ToString()), float.Parse(lstActiveSample[i].Y.ToString()));
        //            }
        //        }
        //    }
        //    lstNoActiveSample = lstNoActiveSample.OrderByDescending(data => data.Y).ToList<StandSample>();
        //    lstNoActiveppl = new List<PointPairList>();
        //    for (int i = 0; i < lstNoActiveSample.Count; i++)
        //    {
        //        PointPairList ppl = new PointPairList();
        //        LineItem li = xrfEdit.GraphPane.AddCurve("", ppl, Color.Blue, SymbolType.Square);
        //        li.Symbol.Fill = new Fill(Color.Blue);
        //        li.Symbol.Size = 8f;
        //        if (ce != null)
        //        {
        //            StandSample ss = ce.Samples.ToList().Find(c => c.SampleName == lstNoActiveSample[i].SampleName);//主元素相同标样
        //            if ((radIntensityCorrect.Checked || radContentContect.Checked) && !xrfEdit.IsMainElement)
        //            {
        //                ppl.Add(lstNoActiveSample[i].TheoryX, double.Parse(lstNoActiveSample[i].Y) / double.Parse(ss.Y));
        //                lstNoActiveppl.Add(ppl);
        //            }
        //            else
        //            {
        //                ppl.Add(double.Parse(lstNoActiveSample[i].X) / double.Parse(ss.X), double.Parse(lstNoActiveSample[i].Y) / double.Parse(ss.Y));
        //                lstNoActiveppl.Add(ppl);
        //            }
        //        }
        //        else
        //        {
        //            if ((radIntensityCorrect.Checked || radContentContect.Checked) && !xrfEdit.IsMainElement)
        //            {
        //                ppl.Add(lstNoActiveSample[i].TheoryX, double.Parse(lstNoActiveSample[i].Y));
        //                lstNoActiveppl.Add(ppl);
        //            }
        //            else
        //            {
        //                ppl.Add(double.Parse(lstNoActiveSample[i].X), double.Parse(lstNoActiveSample[i].Y));
        //                lstNoActiveppl.Add(ppl);
        //            }
        //        }
        //    }
        //    xrfEdit.ScalePoints = pScalePoints;
        //    if (radConic.Checked)//二次曲线
        //    {
        //        xrfEdit.IZero = false;
        //        var pp = pfarr.Distinct().ToArray();
        //        if (pp.Length < 3 && !xrfEdit.IsMainElement)
        //        {
        //            SkyrayMsgBox.Show(Info.NeedConicPoint);
        //        }
        //        else
        //        {
        //            xrfEdit.CalculationWay = CalculationWay.Conic;
        //            xrfEdit.CalculationPoints = pp;
        //        }
        //    }
        //    else if (radIntensityCorrect.Checked)//强度校正
        //    {
        //        xrfEdit.CalculationWay = CalculationWay.IntensityCorrect;
        //        pfarr = new PointF[2];
        //        minY = minX * (Slope + workCurveCurrent.ElementList.Items[SelectDgvIndex].SlopeOptimalFactor) + Intercept;
        //        maxY = maxX * (Slope + workCurveCurrent.ElementList.Items[SelectDgvIndex].SlopeOptimalFactor) + Intercept;
        //        pfarr[0] = new PointF(float.Parse(minX.ToString()), float.Parse(minY.ToString()));
        //        pfarr[1] = new PointF(float.Parse(maxX.ToString()), float.Parse(maxY.ToString()));
        //        xrfEdit.CalculationPoints = pfarr;
        //    }
        //    else if (radContentContect.Checked)//含量校正
        //    {
        //        xrfEdit.CalculationWay = CalculationWay.ContentContect;
        //        pfarr = new PointF[2];
        //        minY = minX * (Slope + workCurveCurrent.ElementList.Items[SelectDgvIndex].SlopeOptimalFactor) + Intercept;
        //        maxY = maxX * (Slope + workCurveCurrent.ElementList.Items[SelectDgvIndex].SlopeOptimalFactor) + Intercept;
        //        pfarr[0] = new PointF(float.Parse(minX.ToString()), float.Parse(minY.ToString()));
        //        pfarr[1] = new PointF(float.Parse(maxX.ToString()), float.Parse(maxY.ToString()));
        //        xrfEdit.CalculationPoints = pfarr;
        //    }
        //    else if (radInsert.Checked)//插值
        //    {
        //        xrfEdit.CalculationWay = CalculationWay.Insert;
        //        xrfEdit.CalculationPoints = pfarr;
        //        PointPairList ppl = new PointPairList();
        //        LineItem li = xrfEdit.GraphPane.AddCurve("", ppl, Color.Red, SymbolType.Square);
        //        li.Symbol.Fill = new Fill(Color.White);
        //        li.Symbol.Size = 8f;
        //        pfarr = pfarr.OrderBy(p => p.X).ToArray();
        //        foreach (PointF pf in pfarr)
        //            ppl.Add(pf.X, pf.Y);
        //    }
        //    else if (radACurve.Checked)//一次曲线
        //    {
        //        //绑定斜率和截距
        //        BindHelper.BindTextToCtrl(txtSlopeOptimalFactor, workCurveCurrent.ElementList.Items[SelectDgvIndex], "SlopeOptimalFactor", true);
        //        BindHelper.BindTextToCtrl(txtInterceptOptimalFactor, workCurveCurrent.ElementList.Items[SelectDgvIndex], "InterceptOptimalFactor", true);
        //        CalLinearParam();//计算斜率和截距
        //        this.lblEquationExpression.Text = "y=" + Slope.ToString("f6") + "*x" + (Intercept > 0 ? "+" : "") + Intercept.ToString("f6");
        //        var R = Math.Pow(MathFunc.CalcCoefficientR(pfarr), 2d);   //OES  //两种计算系数的方法基本结果一样
        //        this.lblR2Value.Text = R.ToString("f6");
        //        xrfEdit.CalculationWay = CalculationWay.Linear;
        //        pfarr = new PointF[2];
        //        minY = minX * (Slope + workCurveCurrent.ElementList.Items[SelectDgvIndex].SlopeOptimalFactor) + Intercept;
        //        maxY = maxX * (Slope + workCurveCurrent.ElementList.Items[SelectDgvIndex].SlopeOptimalFactor) + Intercept;
        //        pfarr[0] = new PointF(float.Parse(minX.ToString()), float.Parse(minY.ToString()));
        //        pfarr[1] = new PointF(float.Parse(maxX.ToString()), float.Parse(maxY.ToString()));
        //        xrfEdit.CalculationPoints = pfarr;
        //    }
        //    this.xrfEdit.AxisChange();
        //    this.xrfEdit.Refresh();
        //    if (radConic.Checked)
        //    {
        //        var coffs = this.xrfEdit.Coeff;
        //        if (coffs != null)
        //        {
        //            string sFormat = "f6";
        //            var a = coffs[0].ToString(sFormat);
        //            var b = coffs[1] >= 0 ? "+" + coffs[1].ToString(sFormat) : coffs[1].ToString(sFormat);
        //            var c = coffs[2] >= 0 ? "+" + coffs[2].ToString(sFormat) : coffs[2].ToString(sFormat);
        //            this.lblEquationExpression.Text = "y=" + a + "x²" + b + "x" + c;
        //            //理论前实际值后
        //            this.grpOnes.Visible = true;
        //            PointF[] pp = new PointF[pfarr.Length];
        //            for (int i = 0; i < pfarr.Length;i++ )
        //            {
        //                float x = float.Parse((pfarr[i].X * pfarr[i].X * coffs[0] + pfarr[i].X * coffs[1] + coffs[2]).ToString());
        //                pp[i] = new PointF(x,pfarr[i].Y);
        //            }
        //            var R = Math.Pow(MathFunc.CalcCoefficientR(pp), 2d);   
        //            this.lblR2Value.Text = R.ToString("f6");
        //        }
        //    }
        //}
        #endregion end-------
        
        /// <summary>
        /// 根据不同曲线类型画图
        /// </summary>
        private void Draw()
        {
            xrfEdit.XTitle = Info.Content;
            xrfEdit.YTitle = Info.Intensity;
            pScalePoints = new PointF[2];
            List<StandSample> lsTemp = workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples.OrderByDescending(data => double.Parse(data.X)).ToList<StandSample>();
            double minX = 0;
            double maxX = 0;
            double minY = 0;
            double maxY = 0;
            xrfEdit.IsMainElement = false;
            CurveElement ce = workCurveCurrent.ElementList.Items.ToList().Find(f => f.Flag == ElementFlag.Internal);
            if (ce != null)//选择了主元素
            {
                
                if (ce.Caption == workCurveCurrent.ElementList.Items[SelectDgvIndex].Caption)
                {
                    xrfEdit.IsMainElement = true;
                }
                else
                {
                    PointF[] tempPoints = new PointF[lsTemp.Count];
                    if (radIntensityCorrect.Checked || radContentContect.Checked)
                    {
                        for (int i = 0; i < tempPoints.Length; i++)
                        {
                            StandSample standi = ce.Samples.ToList().Find(c => c.SampleName == lsTemp[i].SampleName);//和主元素同名标样
                            if (standi == null)
                            { tempPoints[i] = new PointF(0F, 0F); }
                            else
                            {
                                //tempPoints[i] = new PointF(float.Parse(lsTemp[i].TheoryX.ToString()), float.Parse((lsTemp[i].Y / standi.Y).ToString()));
                                //tempPoints[i] = new PointF(float.Parse(lsTemp[i].TheoryX.ToString()), float.Parse(lsTemp[i].Y) / float.Parse(standi.Y));
                                tempPoints[i] = new PointF(float.Parse(lsTemp[i].Y) / float.Parse(standi.Y),float.Parse(lsTemp[i].TheoryX.ToString()));
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < tempPoints.Length; i++)
                        {
                            StandSample standi = ce.Samples.ToList().Find(c => c.SampleName == lsTemp[i].SampleName);//和主元素同名标样
                            if (standi == null)
                            { tempPoints[i] = new PointF(0F, 0F); }
                            else
                            {
                                //tempPoints[i] = new PointF(float.Parse((lsTemp[i].X / standi.X).ToString()), float.Parse((lsTemp[i].Y / standi.Y).ToString()));
                                //tempPoints[i] = new PointF(float.Parse(lsTemp[i].X) / float.Parse(standi.X), float.Parse(lsTemp[i].Y) / float.Parse(standi.Y));
                                tempPoints[i] = new PointF(float.Parse(lsTemp[i].Y) / float.Parse(standi.Y),float.Parse(lsTemp[i].X) / float.Parse(standi.X));
                            }
                        }
                    }
                    minX = tempPoints.OrderBy(data => data.X).ToList()[0].X;
                    maxX = tempPoints.OrderByDescending(data => data.X).ToList()[0].X;

                    minY = tempPoints.OrderBy(data => data.Y).ToList()[0].Y;
                    maxY = tempPoints.OrderByDescending(data => data.Y).ToList()[0].Y;
                }
            } 
            else
            {
                if (radIntensityCorrect.Checked || radContentContect.Checked)
                {
                    lsTemp = workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples.OrderByDescending(data => data.TheoryX).ToList<StandSample>();
                    //minX = lsTemp[lsTemp.Count - 1].TheoryX;
                   // maxX = lsTemp[0].TheoryX;
                    minY = lsTemp[lsTemp.Count - 1].TheoryX;
                    maxY = lsTemp[0].TheoryX;
                }
                else
                {
                   // minX = double.Parse(lsTemp[lsTemp.Count - 1].X);
                   // maxX = double.Parse(lsTemp[0].X);
                    minY = double.Parse(lsTemp[lsTemp.Count - 1].X);
                    maxY = double.Parse(lsTemp[0].X);
                }
                //minY = double.Parse(lsTemp.OrderBy(data => double.Parse(data.Y)).ToList()[0].Y);
                //maxY = double.Parse(lsTemp.OrderByDescending(data => double.Parse(data.Y)).ToList()[0].Y);
                minX= double.Parse(lsTemp.OrderBy(data => double.Parse(data.Y)).ToList()[0].Y);
                maxX = double.Parse(lsTemp.OrderByDescending(data => double.Parse(data.Y)).ToList()[0].Y);
            }
            pScalePoints[0] = new PointF(float.Parse(minX.ToString()), float.Parse(minY.ToString()));
            pScalePoints[1] = new PointF(float.Parse(maxX.ToString()), float.Parse(maxY.ToString()));

            xrfEdit.GraphPane.CurveList.Clear();
            lstActiveSample = lstActiveSample.OrderByDescending(data => double.Parse(data.X)).ToList<StandSample>();
            lstActiveppl = new List<PointPairList>();
            pfarr = new PointF[lstActiveSample.Count];

            for (int i = 0; i < lstActiveSample.Count; i++)
            {
                PointPairList ppl = new PointPairList();
                LineItem li = xrfEdit.GraphPane.AddCurve("", ppl, Color.Red, SymbolType.Square);
                li.Symbol.Fill = new Fill(Color.Red);
                li.Symbol.Size = 8f;
                if (ce != null)
                {
                    StandSample ss = ce.Samples.ToList().Find(c => c.SampleName == lstActiveSample[i].SampleName);//和主元素同名标样
                    if ((radIntensityCorrect.Checked || radContentContect.Checked) && !xrfEdit.IsMainElement)
                    {
                        //ppl.Add(lstActiveSample[i].TheoryX, double.Parse(lstActiveSample[i].Y) / double.Parse(ss.Y));
                        ppl.Add(double.Parse(lstActiveSample[i].Y) / double.Parse(ss.Y), lstActiveSample[i].TheoryX);
                        lstActiveppl.Add(ppl);
                        //pfarr[i] = new PointF(float.Parse(lstActiveSample[i].TheoryX.ToString()), float.Parse(lstActiveSample[i].Y) / float.Parse(ss.Y));
                        pfarr[i] = new PointF(float.Parse(lstActiveSample[i].Y) / float.Parse(ss.Y),float.Parse(lstActiveSample[i].TheoryX.ToString()));
                    }
                    else if (ss != null)
                    {
                        //ppl.Add(double.Parse(lstActiveSample[i].X) / double.Parse(ss.X), double.Parse(lstActiveSample[i].Y) / double.Parse(ss.Y));
                        ppl.Add(double.Parse(lstActiveSample[i].Y) / double.Parse(ss.Y),double.Parse(lstActiveSample[i].X) / double.Parse(ss.X));
                        lstActiveppl.Add(ppl);
                        //pfarr[i] = new PointF(float.Parse(lstActiveSample[i].X) / float.Parse(ss.X), float.Parse(lstActiveSample[i].Y) / float.Parse(ss.Y));
                        pfarr[i] = new PointF(float.Parse(lstActiveSample[i].Y) / float.Parse(ss.Y),float.Parse(lstActiveSample[i].X) / float.Parse(ss.X));
                    }
                }
                else
                {
                    if ((radIntensityCorrect.Checked || radContentContect.Checked) && !xrfEdit.IsMainElement)
                    {
                        //ppl.Add(lstActiveSample[i].TheoryX, double.Parse(lstActiveSample[i].Y));
                        ppl.Add(double.Parse(lstActiveSample[i].Y),lstActiveSample[i].TheoryX);
                        lstActiveppl.Add(ppl);
                        //pfarr[i] = new PointF(float.Parse(lstActiveSample[i].TheoryX.ToString()), float.Parse(lstActiveSample[i].Y.ToString()));
                        pfarr[i] = new PointF(float.Parse(lstActiveSample[i].Y.ToString()),float.Parse(lstActiveSample[i].TheoryX.ToString()));
                    }
                    else
                    {
                       //ppl.Add(double.Parse(lstActiveSample[i].X), double.Parse(lstActiveSample[i].Y));
                        ppl.Add(double.Parse(lstActiveSample[i].Y),double.Parse(lstActiveSample[i].X));
                        lstActiveppl.Add(ppl);
                        //pfarr[i] = new PointF(float.Parse(lstActiveSample[i].X.ToString()), float.Parse(lstActiveSample[i].Y.ToString()));
                        pfarr[i] = new PointF(float.Parse(lstActiveSample[i].Y.ToString()),float.Parse(lstActiveSample[i].X.ToString()));
                    }
                }
            }
            lstNoActiveSample = lstNoActiveSample.OrderByDescending(data => data.Y).ToList<StandSample>();
            lstNoActiveppl = new List<PointPairList>();
            for (int i = 0; i < lstNoActiveSample.Count; i++)
            {
                PointPairList ppl = new PointPairList();
                LineItem li = xrfEdit.GraphPane.AddCurve("", ppl, Color.Blue, SymbolType.Square);
                li.Symbol.Fill = new Fill(Color.Blue);
                li.Symbol.Size = 8f;
                if (ce != null)
                {
                    StandSample ss = ce.Samples.ToList().Find(c => c.SampleName == lstNoActiveSample[i].SampleName);//主元素相同标样
                    if ((radIntensityCorrect.Checked || radContentContect.Checked) && !xrfEdit.IsMainElement)
                    {
                        //ppl.Add(lstNoActiveSample[i].TheoryX, double.Parse(lstNoActiveSample[i].Y) / double.Parse(ss.Y));
                        ppl.Add(double.Parse(lstNoActiveSample[i].Y) / double.Parse(ss.Y),lstNoActiveSample[i].TheoryX);
                        lstNoActiveppl.Add(ppl);
                    }
                    else
                    {
                       // ppl.Add(double.Parse(lstNoActiveSample[i].X) / double.Parse(ss.X), double.Parse(lstNoActiveSample[i].Y) / double.Parse(ss.Y));
                        ppl.Add(double.Parse(lstNoActiveSample[i].Y) / double.Parse(ss.Y),double.Parse(lstNoActiveSample[i].X) / double.Parse(ss.X));
                        lstNoActiveppl.Add(ppl);
                    }
                }
                else
                {
                    if ((radIntensityCorrect.Checked || radContentContect.Checked) && !xrfEdit.IsMainElement)
                    {
                        //ppl.Add(lstNoActiveSample[i].TheoryX, double.Parse(lstNoActiveSample[i].Y));
                        ppl.Add(double.Parse(lstNoActiveSample[i].Y),lstNoActiveSample[i].TheoryX);
                        lstNoActiveppl.Add(ppl);
                    }
                    else
                    {
                        //ppl.Add(double.Parse(lstNoActiveSample[i].X), double.Parse(lstNoActiveSample[i].Y));
                        ppl.Add(double.Parse(lstNoActiveSample[i].Y),double.Parse(lstNoActiveSample[i].X));
                        lstNoActiveppl.Add(ppl);
                    }
                }
            }
            xrfEdit.ScalePoints = pScalePoints;
            if (radConic.Checked)//二次曲线
            {
                xrfEdit.IZero = false;
                var pp = pfarr.Distinct().ToArray();
                if (pp.Length < 3 && !xrfEdit.IsMainElement)
                {
                    SkyrayMsgBox.Show(Info.NeedConicPoint);
                }
                else
                {
                    xrfEdit.CalculationWay = CalculationWay.Conic;
                    xrfEdit.CalculationPoints = pp;
                }
            }
            else if (radIntensityCorrect.Checked)//强度校正
            {
                xrfEdit.CalculationWay = CalculationWay.IntensityCorrect;
                pfarr = new PointF[2];
                //minY = minX * (Slope + workCurveCurrent.ElementList.Items[SelectDgvIndex].SlopeOptimalFactor) + Intercept;
                //maxY = maxX * (Slope + workCurveCurrent.ElementList.Items[SelectDgvIndex].SlopeOptimalFactor) + Intercept;
                minX = minY * (Slope + workCurveCurrent.ElementList.Items[SelectDgvIndex].SlopeOptimalFactor) + Intercept;
                maxX = maxY * (Slope + workCurveCurrent.ElementList.Items[SelectDgvIndex].SlopeOptimalFactor) + Intercept;
                pfarr[0] = new PointF(float.Parse(minX.ToString()), float.Parse(minY.ToString()));
                pfarr[1] = new PointF(float.Parse(maxX.ToString()), float.Parse(maxY.ToString()));
                xrfEdit.CalculationPoints = pfarr;
            }
            else if (radContentContect.Checked)//含量校正
            {
                xrfEdit.CalculationWay = CalculationWay.ContentContect;
                pfarr = new PointF[2];
                //minY = minX * (Slope + workCurveCurrent.ElementList.Items[SelectDgvIndex].SlopeOptimalFactor) + Intercept;
                //maxY = maxX * (Slope + workCurveCurrent.ElementList.Items[SelectDgvIndex].SlopeOptimalFactor) + Intercept;
                minX = minY * (Slope + workCurveCurrent.ElementList.Items[SelectDgvIndex].SlopeOptimalFactor) + Intercept;
                maxX = maxY * (Slope + workCurveCurrent.ElementList.Items[SelectDgvIndex].SlopeOptimalFactor) + Intercept;
                pfarr[0] = new PointF(float.Parse(minX.ToString()), float.Parse(minY.ToString()));
                pfarr[1] = new PointF(float.Parse(maxX.ToString()), float.Parse(maxY.ToString()));
                xrfEdit.CalculationPoints = pfarr;
            }
            else if (radInsert.Checked)//插值
            {
                xrfEdit.CalculationWay = CalculationWay.Insert;
                xrfEdit.CalculationPoints = pfarr;
                PointPairList ppl = new PointPairList();
                LineItem li = xrfEdit.GraphPane.AddCurve("", ppl, Color.Red, SymbolType.Square);
                li.Symbol.Fill = new Fill(Color.White);
                li.Symbol.Size = 8f;
                pfarr = pfarr.OrderBy(p => p.X).ToArray();
                foreach (PointF pf in pfarr)
                    ppl.Add(pf.X, pf.Y);
            }
            else if (radACurve.Checked)//一次曲线
            {
                //绑定斜率和截距
                BindHelper.BindTextToCtrl(txtSlopeOptimalFactor, workCurveCurrent.ElementList.Items[SelectDgvIndex], "SlopeOptimalFactor", true);
                BindHelper.BindTextToCtrl(txtInterceptOptimalFactor, workCurveCurrent.ElementList.Items[SelectDgvIndex], "InterceptOptimalFactor", true);
                CalLinearParam();//计算斜率和截距 
                this.lblEquationExpression.Text = "y=  " + Slope.ToString("E4") + "*x" + (Intercept > 0 ? "  +" : "  ") + Intercept.ToString("E4");
                var R = Math.Pow(MathFunc.CalcCoefficientR(pfarr), 2d);   //OES  //两种计算系数的方法基本结果一样
                this.lblR2Value.Text = R.ToString("f6");
                xrfEdit.CalculationWay = CalculationWay.Linear;
                pfarr = new PointF[2];
                //minY = minX * (Slope + workCurveCurrent.ElementList.Items[SelectDgvIndex].SlopeOptimalFactor) + Intercept;
                //maxY = maxX * (Slope + workCurveCurrent.ElementList.Items[SelectDgvIndex].SlopeOptimalFactor) + Intercept;
                minX = minY * (Slope + workCurveCurrent.ElementList.Items[SelectDgvIndex].SlopeOptimalFactor) + Intercept;
                maxX = maxY * (Slope + workCurveCurrent.ElementList.Items[SelectDgvIndex].SlopeOptimalFactor) + Intercept;
                pfarr[0] = new PointF(float.Parse(minX.ToString()), float.Parse(minY.ToString()));
                pfarr[1] = new PointF(float.Parse(maxX.ToString()), float.Parse(maxY.ToString()));
                xrfEdit.CalculationPoints = pfarr;
            }
            this.xrfEdit.AxisChange();
            this.xrfEdit.Refresh();
            if (radConic.Checked)
            {
                var coffs = this.xrfEdit.Coeff;
                if (coffs != null)
                {
                    string sFormat = "E4";
                    var a = coffs[0].ToString(sFormat);
                    var b = coffs[1] >= 0 ? "  +" + coffs[1].ToString(sFormat) : "  " + coffs[1].ToString(sFormat);
                    var c = coffs[2] >= 0 ? "  +" + coffs[2].ToString(sFormat) : "  " + coffs[2].ToString(sFormat);
                    //this.lblEquationExpression.Text = "y=" + a + "x²" + b + "x" + c;
                    this.lblEquationExpression.Text = "x=  " + a + "y²" + b + "y" + c;
                    //理论前实际值后
                    this.grpOnes.Visible = true;
                    PointF[] pp = new PointF[pfarr.Length];
                    for (int i = 0; i < pfarr.Length; i++)
                    {
                        float x = float.Parse((pfarr[i].X * pfarr[i].X * coffs[0] + pfarr[i].X * coffs[1] + coffs[2]).ToString());
                        pp[i] = new PointF(x, pfarr[i].Y);
                    }
                    var R = Math.Pow(MathFunc.CalcCoefficientR(pp), 2d);
                    this.lblR2Value.Text = R.ToString("f6");
                }
            }
        }
        /// <summary>
        /// 初始化计算方式
        /// </summary>
        private void InitCalculationWay()
        {
            if ((WorkCurveHelper.DeviceFunctype == FuncType.XRF && WorkCurveHelper.WorkCurveCurrent.CalcType == CalcType.EC || WorkCurveHelper.DeviceFunctype == FuncType.Rohs)
                || (WorkCurveHelper.DeviceFunctype == FuncType.Thick && WorkCurveHelper.WorkCurveCurrent.CalcType == CalcType.PeakDivBase))
            {
                if (workCurveCurrent.ElementList.Items[SelectDgvIndex].CalculationWay == CalculationWay.Linear)
                {
                    if (radACurve.Checked)
                    {
                        radACurve_CheckedChanged(null, null);
                    }
                    this.radACurve.Checked = true;
                }
                else if (workCurveCurrent.ElementList.Items[SelectDgvIndex].CalculationWay == CalculationWay.Conic)
                {
                    if (radConic.Checked)
                    {
                        radConic_CheckedChanged(null, null);
                    }
                    this.radConic.Checked = true;
                }
                else if (workCurveCurrent.ElementList.Items[SelectDgvIndex].CalculationWay == CalculationWay.Insert)
                {
                    if (radInsert.Checked)
                    { 
                        radInsert_CheckedChanged(null, null);
                    }
                    this.radInsert.Checked = true;
                }
                else if (workCurveCurrent.ElementList.Items[SelectDgvIndex].CalculationWay == CalculationWay.IntensityCorrect)
                {
                    if (radIntensityCorrect.Checked)
                    {
                        radIntensityCorrect_CheckedChanged(null, null); 
                    }
                    this.radIntensityCorrect.Checked = true;
                }
                else
                {
                    if (radContentContect.Checked)
                    {
                        radContentContect_CheckedChanged(null, null);
                    }
                    this.radContentContect.Checked = true;
                }
            }
            //add by chuyaqin 2011-04-24 添加fp曲线功能
            else if (WorkCurveHelper.DeviceFunctype == FuncType.XRF && WorkCurveHelper.WorkCurveCurrent.CalcType == CalcType.FP)
            {
              
                if (!radContentVS.Checked&&!radIntensityVS.Checked)
                {
                    radIntensityVS.Checked = true;
                }
                if (radIntensityVS.Checked)
                {
                    grpFpIntensityVs.Visible = true;
                    switch (workCurveCurrent.ElementList.Items[SelectDgvIndex].FpCalculationWay)
                    {
                        case FpCalculationWay.LinearWithAnIntercept:
                            radOneNoForcedOrigin.Checked = true;
                            break;
                        case FpCalculationWay.LinearWithoutAnIntercept:
                            radOneForcedOrigin.Checked = true;
                            break;
                        case FpCalculationWay.SquareWithoutAnIntercept:
                            radTwoForcedOrigin.Checked = true;
                            break;
                        case FpCalculationWay.SquareWithAnIntercept:
                            radTwoNoForcedOrigin.Checked = true;
                            break;
                    }

                    radOneNoForcedOrigin_CheckedChanged(null, null);
                }
                else if (radContentVS.Checked)
                {
                    grpFpIntensityVs.Visible = false;
                    switch (workCurveCurrent.ElementList.Items[SelectDgvIndex].FpCalculationWay)
                    {
                        case FpCalculationWay.LinearWithAnIntercept:
                            radOneNoForcedOrigin.Checked = true;
                            break;
                        case FpCalculationWay.LinearWithoutAnIntercept:
                            radOneForcedOrigin.Checked = true;
                            break;
                        case FpCalculationWay.SquareWithoutAnIntercept:
                            radTwoForcedOrigin.Checked = true;
                            break;
                        case FpCalculationWay.SquareWithAnIntercept:
                            radTwoNoForcedOrigin.Checked = true;
                            break;
                    }
                    radContentVS_CheckedChanged(null, null);
                }
            }
            else if (WorkCurveHelper.DeviceFunctype == FuncType.Thick && WorkCurveHelper.WorkCurveCurrent.CalcType == CalcType.EC)
            {
                if (workCurveCurrent.ElementList.Items[SelectDgvIndex].CalculationWay == CalculationWay.Linear)
                {
                    if (radThickLine.Checked)
                    {
                        radThickLine_CheckedChanged(null, null);
                    }
                    this.radThickLine.Checked = true;
                }
                else if (workCurveCurrent.ElementList.Items[SelectDgvIndex].CalculationWay == CalculationWay.Insert)
                {
                    if (radThickInsert.Checked)
                    {
                        radThickInsert_CheckedChanged(null, null);
                    }
                    this.radThickInsert.Checked = true;
                }
            }
            else if (WorkCurveHelper.DeviceFunctype == FuncType.Thick && WorkCurveHelper.WorkCurveCurrent.CalcType == CalcType.FP)
            {
                radIntensityVS.Checked = true;
                if (radIntensityVS.Checked)
                {
                    grpFpIntensityVs.Visible = true;
                    switch (workCurveCurrent.ElementList.Items[SelectDgvIndex].FpCalculationWay)
                    {
                        case FpCalculationWay.LinearWithAnIntercept:
                            radOneNoForcedOrigin.Checked = true;
                            break;
                        case FpCalculationWay.LinearWithoutAnIntercept:
                            radOneForcedOrigin.Checked = true;
                            break;
                        case FpCalculationWay.SquareWithAnIntercept:
                            radTwoNoForcedOrigin.Checked = true;
                            break;
                        case FpCalculationWay.SquareWithoutAnIntercept:
                            radTwoForcedOrigin.Checked = true;
                            break;
                    }
                    radOneNoForcedOrigin_CheckedChanged(null, null);
                }
            }
        }

        #endregion

        #region Thick
        /// <summary>
        /// 测厚线性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radThickLine_CheckedChanged(object sender, EventArgs e)
        {
            if (radThickLine.Checked)
            {
                workCurveCurrent.ElementList.ThCalculationWay = ThCalculationWay.ThLinear;
                //workCurveCurrent.ElementList.Items[SelectDgvIndex].CalculationWay = CalculationWay.Linear;//设置计算方法
                foreach (var element in workCurveCurrent.ElementList.Items)
                {
                    element.CalculationWay = CalculationWay.Linear;//设置计算方法
                }
                DrawGraph();//画图
            }
        }
        /// <summary>
        /// 测厚插值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radThickInsert_CheckedChanged(object sender, EventArgs e)
        {
            if (radThickInsert.Checked)
            {
                workCurveCurrent.ElementList.ThCalculationWay = ThCalculationWay.ThInsert;
                //workCurveCurrent.ElementList.Items[SelectDgvIndex].CalculationWay = CalculationWay.Insert;
                foreach (var element in workCurveCurrent.ElementList.Items)
                {
                    element.CalculationWay = CalculationWay.Insert;//设置计算方法
                }
                DrawGraph();
            }
        }
        /// <summary>
        /// 测厚画图
        /// </summary>
        private void DrawThick()
        {
            if (WorkCurveHelper.CalcType == CalcType.EC)
            {
                //lstActiveSample = lstActiveSample.FindAll(l => l.Level != Info.SingleLayer);
                //lstNoActiveSample = lstNoActiveSample.FindAll(l => l.Level != Info.SingleLayer);
                string elementName = workCurveCurrent.ElementList.Items[SelectDgvIndex].Caption;
                double[] dIntensity;
                try
                {
                    if (workCurveCurrent.GetCalibrateIntensity(elementName, out dIntensity))
                    {
                        for (int i = 0; i < workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples.Count; i++)
                        {
                            workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples[i].TheoryX = dIntensity[i];
                        }
                    }
                }
                catch (Exception ex)
                {
                    SkyrayMsgBox.Show(ex.Message);//计算测厚校正强度的出错  "计算校正强度失败！"
                    return;
                }
                pScalePoints = new PointF[2];
                List<StandSample> lsTemp = workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples.OrderByDescending(data => data.TheoryX).ToList<StandSample>();
                //lsTemp = lsTemp.FindAll(l => l.Level != Info.SingleLayer);
                xrfEdit.IsMainElement = false;
                if (lsTemp.Count == 0)
                    return;
                if (lsTemp.Count == 1 && workCurveCurrent.FuncType == FuncType.Thick)
                {
                    StandSample sp=StandSample.New;
                    sp.X="0";
                    sp.Y="0";
                    sp.Z="0";
                    sp.Active = true;
                    lsTemp.Add(sp);
                }
                double minX = lsTemp[lsTemp.Count - 1].TheoryX;
                double maxX = lsTemp[0].TheoryX;
                double minY = double.Parse(lsTemp.OrderBy(data => double.Parse(data.Z)).ToList()[0].Z);
                double maxY = double.Parse(lsTemp.OrderByDescending(data => double.Parse(data.Z)).ToList()[0].Z);
                pScalePoints[0] = new PointF(float.Parse(minX.ToString()), float.Parse(minY.ToString()));
                pScalePoints[1] = new PointF(float.Parse(maxX.ToString()), float.Parse(maxY.ToString()));
                xrfEdit.GraphPane.CurveList.Clear();
                lstActiveSample = lstActiveSample.OrderByDescending(data => data.TheoryX).ToList<StandSample>();
                lstActiveppl = new List<PointPairList>();
                pfarr = new PointF[lstActiveSample.Count];
                for (int i = 0; i < lstActiveSample.Count; i++)
                {
                    PointPairList ppl = new PointPairList();
                    LineItem li = xrfEdit.GraphPane.AddCurve("", ppl, Color.Red, SymbolType.Square);
                    li.Symbol.Fill = new Fill(Color.Red);
                    li.Symbol.Size = 8f;
                    ppl.Add(lstActiveSample[i].TheoryX, double.Parse(lstActiveSample[i].Z));
                    lstActiveppl.Add(ppl);
                    pfarr[i] = new PointF(float.Parse(lstActiveSample[i].TheoryX.ToString()), float.Parse(lstActiveSample[i].Z.ToString()));
                }
                lstNoActiveSample = lstNoActiveSample.OrderByDescending(data => data.TheoryX).ToList<StandSample>();
                lstNoActiveppl = new List<PointPairList>();
                for (int i = 0; i < lstNoActiveSample.Count; i++)
                {
                    PointPairList ppl = new PointPairList();
                    LineItem li = xrfEdit.GraphPane.AddCurve("", ppl, Color.Blue, SymbolType.Square);
                    li.Symbol.Fill = new Fill(Color.Blue);
                    li.Symbol.Size = 8f;
                    ppl.Add(lstNoActiveSample[i].TheoryX, double.Parse(lstNoActiveSample[i].Z));
                    lstNoActiveppl.Add(ppl);
                }
                xrfEdit.ScalePoints = pScalePoints;
                if (radThickInsert.Checked)//插值
                {
                    
                    xrfEdit.CalculationWay = CalculationWay.Insert;
                    xrfEdit.CalculationPoints = pfarr;
                    PointPairList ppl = new PointPairList();
                    LineItem li = xrfEdit.GraphPane.AddCurve("", ppl, Color.Red, SymbolType.Square);
                    li.Symbol.Fill = new Fill(Color.Red);
                    li.Symbol.Size = 8f;
                    foreach (PointF pf in pfarr)
                        ppl.Add(pf.X, pf.Y);
                    if (pfarr.Length == 1)
                    {
                        ppl.Add(0, 0);
                    }
                }
                else if (radThickLine.Checked)
                {
                    CalLinearParamThick(lsTemp);//计算斜率和截距
                    xrfEdit.CalculationWay = CalculationWay.Linear;
                    pfarr = new PointF[2];

                    minY = (0.9*minX) * (Slope + workCurveCurrent.ElementList.Items[SelectDgvIndex].SlopeOptimalFactor) + Intercept;
                    maxY = (1.1*maxX) * (Slope + workCurveCurrent.ElementList.Items[SelectDgvIndex].SlopeOptimalFactor) + Intercept;
                    pfarr[0] = new PointF(float.Parse((0.9 * minX).ToString()), float.Parse(minY.ToString()));
                    pfarr[1] = new PointF(float.Parse((1.1 * maxX).ToString()), float.Parse(maxY.ToString()));
                    xrfEdit.CalculationPoints = pfarr;
                }
            }
            else if (WorkCurveHelper.CalcType == CalcType.FP)
            {
                //string elementName = workCurveCurrent.ElementList.Items[SelectDgvIndex].Caption;
                //double[] dIntensity;
                //try
                //{
                //    workCurveCurrent.GetCalibrateItensity();
                //}
                //catch (Exception ex)
                //{
                //    SkyrayMsgBox.Show(ex.Message);
                //}
                pScalePoints = new PointF[2];
                List<StandSample> lsTemp = workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples.OrderByDescending(data => double.Parse(data.X)).ToList<StandSample>();
                xrfEdit.IsMainElement = false;
                double minX = double.Parse(lsTemp[lsTemp.Count - 1].X);
                double maxX = double.Parse(lsTemp[0].X);
                double minY = double.Parse(lsTemp.OrderBy(data => double.Parse(data.Z)).ToList()[0].Z);
                double maxY = double.Parse(lsTemp.OrderByDescending(data => double.Parse(data.Z)).ToList()[0].Z);
                pScalePoints[0] = new PointF(float.Parse(minX.ToString()), float.Parse(minY.ToString()));
                pScalePoints[1] = new PointF(float.Parse(maxX.ToString()), float.Parse(maxY.ToString()));
                xrfEdit.GraphPane.CurveList.Clear();
                lstActiveSample = lstActiveSample.OrderByDescending(data => double.Parse(data.X)).ToList<StandSample>();
                lstActiveppl = new List<PointPairList>();
                pfarr = new PointF[lstActiveSample.Count];
                for (int i = 0; i < lstActiveSample.Count; i++)
                {
                    PointPairList ppl = new PointPairList();
                    LineItem li = xrfEdit.GraphPane.AddCurve("", ppl, Color.Red, SymbolType.Square);
                    li.Symbol.Fill = new Fill(Color.Red);
                    li.Symbol.Size = 8f;
                    ppl.Add(double.Parse(lstActiveSample[i].X), double.Parse(lstActiveSample[i].Z));
                    lstActiveppl.Add(ppl);
                    pfarr[i] = new PointF(float.Parse(lstActiveSample[i].X.ToString()), float.Parse(lstActiveSample[i].Z.ToString()));
                }
                lstNoActiveSample = lstNoActiveSample.OrderByDescending(data => double.Parse(data.X)).ToList<StandSample>();
                lstNoActiveppl = new List<PointPairList>();
                for (int i = 0; i < lstNoActiveSample.Count; i++)
                {
                    PointPairList ppl = new PointPairList();
                    LineItem li = xrfEdit.GraphPane.AddCurve("", ppl, Color.Blue, SymbolType.Square);
                    li.Symbol.Fill = new Fill(Color.Blue);
                    li.Symbol.Size = 8f;
                    ppl.Add(double.Parse(lstNoActiveSample[i].X), double.Parse(lstNoActiveSample[i].Z));
                    lstNoActiveppl.Add(ppl);
                }
                xrfEdit.ScalePoints = pScalePoints;
                //if (radThickInsert.Checked)//插值
                //{
                //    xrfEdit.CalculationWay = CalculationWay.Insert;
                //    xrfEdit.CalculationPoints = pfarr;
                //    PointPairList ppl = new PointPairList();
                //    LineItem li = xrfEdit.GraphPane.AddCurve("", ppl, Color.Red, SymbolType.Square);
                //    li.Symbol.Fill = new Fill(Color.White);
                //    li.Symbol.Size = 8f;
                //    foreach (PointF pf in pfarr)
                //        ppl.Add(pf.X, pf.Y);
                //}
                //else if (radThickLine.Checked)
                //{
                    CalLinearParamThick(lsTemp);//计算斜率和截距
                    xrfEdit.CalculationWay = CalculationWay.Linear;
                    pfarr = new PointF[2];
                    minY = minX * (Slope + workCurveCurrent.ElementList.Items[SelectDgvIndex].SlopeOptimalFactor) + Intercept;
                    maxY = maxX * (Slope + workCurveCurrent.ElementList.Items[SelectDgvIndex].SlopeOptimalFactor) + Intercept;
                    pfarr[0] = new PointF(float.Parse(minX.ToString()), float.Parse(minY.ToString()));
                    pfarr[1] = new PointF(float.Parse(maxX.ToString()), float.Parse(maxY.ToString()));
                    xrfEdit.CalculationPoints = pfarr;
                //}
            }
            this.xrfEdit.AxisChange();
            this.xrfEdit.Refresh();
        }
        /// <summary>
        /// 测厚计算斜率截距
        /// </summary>
        private void CalLinearParamThick(List<StandSample> lsTemp)
        {
            Intercept = 0;
            Slope = 0;
            double x = 0;
            double y = 0;
            double xy = 0;
            double xx = 0;
            int count = 0;
            if (lsTemp.Count == 0)
            {
                return;
            }
            for (int i = 0; i < lsTemp.Count; i++)
            {
                if (WorkCurveHelper.CalcType == CalcType.EC)
                {
                    if (lsTemp[i].Active)
                    {
                        x += lsTemp[i].TheoryX;
                        y += double.Parse(lsTemp[i].Z);
                        xy += lsTemp[i].TheoryX * double.Parse(lsTemp[i].Z);
                        xx += lsTemp[i].TheoryX * lsTemp[i].TheoryX;
                        count++;
                    }
                }
                else if (WorkCurveHelper.CalcType == CalcType.FP)
                {
                    if (lsTemp[i].Active)
                    {
                        x += double.Parse(lsTemp[i].X);
                        y += double.Parse(lsTemp[i].Z);
                        xy += double.Parse(lsTemp[i].X) * double.Parse(lsTemp[i].Z);
                        xx += double.Parse(lsTemp[i].X) * double.Parse(lsTemp[i].X);
                        count++;
                    }
                }
            }
            if ((count * xx - x * x) != 0)
            {
                Slope = (count * xy - x * y) / (count * xx - x * x);
                Intercept = (y - Slope * x) / count;
            }
        }

        #endregion

        private void dgwSample_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1) return;
            if (dgwSample.Columns[e.ColumnIndex].Name.Equals("ColumnMatchSpec"))
            {
                string sElementSpecName = (dgwSample.Rows[e.RowIndex].Cells["ColSampleName"] == null) ? "" : dgwSample.Rows[e.RowIndex].Cells["ColSampleName"].Value.ToString();
                SpecListEntity entity = null;
                //if (WorkCurveHelper.SaveType == 0)
                //{
                //    SelectSample sample = new SelectSample(AddSpectrumType.OpenStandardSpec);
                //    WorkCurveHelper.OpenUC(sample, false, Info.SelectSpec);
                //    if (sample.DialogResult == DialogResult.OK)
                //        entity = sample.specListCurrent;
                //}
                //else
                //{
                //    entity = DifferenceDevice.interClassMain.OpenFileDialog(false);
                //}
                List<SpecListEntity> returnResult = EDXRFHelper.GetReturnSpectrum(false,false);
                if (returnResult == null || returnResult.Count == 0)
                    return;
                entity = returnResult[0];
                if (entity != null)
                {

                     //选择匹配谱，判断匹配谱是否已经在其他曲线中存在。
                    List<StandSample> ListStandSample = StandSample.FindBySql("select * from StandSample where MatchSpecName='" + entity.Name.ToString()+"'");
                     if (ListStandSample.Count > 0 && Skyray.Controls.SkyrayMsgBox.Show(Info.isExitMatchSpecList, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                         return;

                     foreach (var item in workCurveCurrent.ElementList.Items)
                     {
                         var samplesList = item.Samples.ToList().FindAll(w => w.SampleName == sElementSpecName);
                         if (samplesList == null || samplesList.Count == 0) continue;
                         foreach (StandSample sSample in samplesList)
                         {
                             sSample.MatchSpecName = entity.Name;
                             //sSample.MatchSpecListId = sample.specListCurrent.Id;
                             sSample.IsMatch = true;
                         }
                     }
                     InitStandSample(false);
                 }
            }
            

        }

        private bool IsUserSample()
        {
            bool isUserSample = false;

            return isUserSample;
        }

        private void btnApplication_Click(object sender, EventArgs e)
        {
            //if (JudgeMatchAndFile())
            //{
            //    SkyrayMsgBox.Show(Info.strJudgeMatchAndFile);
            //    return;
            //}

            if (ExitLevel()) return;
            int result = CheckMainSample();
            if (result==1)
            {
                Msg.Show(Info.strIntensityIsNull);
                return;
            }
            else if (result == 2)
            {
                Msg.Show(Info.strContentIsNull);
                return;
            }
            int index = this.dgvwElement.SelectedRows[0].Index;
            TranslateUnit();
            workCurveCurrent.Save();
            WorkCurveHelper.WorkCurveCurrent = WorkCurve.FindById(workCurveCurrent.Id);
            LoadMethod(false);
            //this.dgvwElement.Rows[index].Selected = true;
        }

        private bool ExitLevel()
        {
            bool isExitLevel = false;
            //paul 20110616 如果是FPThick并且是EC算法时，非基材层必须存在单层
            if (WorkCurveHelper.DeviceFunctype == FuncType.Thick && WorkCurveHelper.WorkCurveCurrent.CalcType == CalcType.EC)
            {
                //List<CurveElement> CurveElementlist = workCurveCurrent.ElementList.Items.ToList().FindAll(delegate(CurveElement v) { return v.LayerNumBackUp != "基材"; });
                //foreach (CurveElement ce in CurveElementlist)
                //    if (!ce.Samples.ToList().Exists(delegate(StandSample v) { return v.Level == "单层"; }))
                //    {
                //        Msg.Show(Info.strLevelInfo);
                //        return true;
                //    }
                List<CurveElement> CurveElementlist = workCurveCurrent.ElementList.Items.ToList().FindAll(delegate(CurveElement v) { return v.LayerNumBackUp != "Substrate" && v.LayerNumBackUp != "基材"; });
                foreach (CurveElement ce in CurveElementlist)
                    if (!ce.Samples.ToList().Exists(delegate(StandSample v) { return v.Level == Info.SingleLayer; }))
                    {
                        Msg.Show(Info.strLevelInfo);
                        return true;
                    }
            }
            return isExitLevel;
        }

        private int CheckMainSample()
        {
            int result = 0;
            foreach (var item in workCurveCurrent.ElementList.Items)
            {
                if(item.Flag == ElementFlag.Internal)
                {
                    foreach (var sam in item.Samples)
                    {
                        if (sam.X == "0")
                        {
                            result = 1;
                            return result;
                        }
                        if (sam.Y == "0")
                        {
                            result = 2;
                            return result;
                        }
                    }
                }
            }
            return result;
        }

        public override void ExcuteEndProcess(params object[] objs)
        {
            if (upflag)
            {
                ToolStripControls test = MenuLoadHelper.MenuStripCollection.Find(w => w.CurrentNaviItem.Name == "DataOptimization");
                EDXRFHelper.RecurseUpUC(test);
            }

            if (flag)
            {
                ToolStripControls test = MenuLoadHelper.MenuStripCollection.Find(w => w.CurrentNaviItem.Name == "EditMatch");
                EDXRFHelper.RecurveNextUC(test);
            }
        }

        private bool flag = false;
        private bool upflag = false;

        private void btWUp_Click(object sender, EventArgs e)
        {
            btnSave_Click(null, null);
            upflag = true;

        }

        //add by chuyaqin 2011-04-24
        private void DrawFpCurve()
        {
            pScalePoints = new PointF[2];
            xrfEdit.IsMainElement = false;
            List<PointF> lstActivePoint = new List<PointF>();
            List<PointF> lstNoActivePoin = new List<PointF>();
            xrfEdit.GraphPane.CurveList.Clear();
            double minX = 0;
            double maxX = 0;
            double minY = 0;
            double maxY = 0;
            try
            {
                #region 理论强度和实际强度
                if (radIntensityVS.Checked)//理论强度和实际强度
                {
                    double[,] elemC; //理论强度
                    double[,] elemI; //测量强度
                    List<string> listSamples;
                    List<string> listElements;
                    if (!workCurveCurrent.GetCalibrateItensity(out elemI, out elemC, out listSamples, out listElements))
                    {
                        tabcwEdit.SelectedTab = tabpData;
                        return;
                    }
                    string eleName = workCurveCurrent.ElementList.Items[SelectDgvIndex].Caption.ToUpper();
                    int Index = listElements.FindIndex(w => w.Trim().Equals(eleName));
                    if (Index < 0)
                    {
                        tabcwEdit.SelectedTab = tabpData;
                        return;
                    }
                    List<PointF> tempPoints = new List<PointF>();
                    //PointF[] tempPoints = new PointF[workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples.Count];
                    if (listSamples != null && listSamples.Count > 0)
                    {
                        for (int i = 0; i < listSamples.Count; i++)
                        {
                            StandSample standi = workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples.ToList().Find(w => w.SampleName == listSamples[i]);
                            PointF tempPoint = new PointF((float)elemI[i, Index], (float)elemC[i, Index]);
                            if (float.IsNaN(tempPoint.X))
                            {
                                tempPoint.X = 0f;
                            }
                            if (float.IsNaN(tempPoint.Y))
                            {
                                tempPoint.Y = 0f;
                            }

                            if ((standi != null && standi.Active)
                                || (workCurveCurrent.ElementList.PureAsInfinite && standi == null && workCurveCurrent.FuncType==FuncType.Thick
                                && listSamples[i].CompareTo(workCurveCurrent.ElementList.Items[SelectDgvIndex].ElementSpecName)==0))
                            {
                                lstActivePoint.Add(tempPoint);
                                if (standi == null && listSamples[i].CompareTo(workCurveCurrent.ElementList.Items[SelectDgvIndex].ElementSpecName) == 0)
                                {
                                    StandSample std=StandSample.New;
                                    std.ElementName = workCurveCurrent.ElementList.Items[SelectDgvIndex].Caption;
                                    std.SampleName = listSamples[i];
                                    std.Active=true;
                                    std.IntensityC=tempPoint.Y;
                                    std.X=tempPoint.X.ToString();
                                    lstActiveSample.Add(std);
                                }
                            }
                            else if (standi != null && !standi.Active)
                            {
                                lstNoActivePoin.Add(tempPoint);
                            }
                            //tempPoints[i] = tempPoint;
                            tempPoints.Add(tempPoint);
                        }
                        
                    }

                    foreach (var sam in workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples)
                    {
                        string hasSample = listSamples.Find(l => l.Equals(sam.SampleName));
                        if (hasSample != null && hasSample != "")
                        { }
                        else
                        {
                            PointF tempPoint = new PointF(float.Parse(sam.X), 0f);
                            lstNoActivePoin.Add(tempPoint);
                            tempPoints.Add(tempPoint);
                        }
                    }
                    maxX = tempPoints.OrderByDescending(data => data.X).ToList()[0].X;
                    maxY = tempPoints.OrderByDescending(data => data.Y).ToList()[0].Y;
                    pScalePoints[0] = new PointF(float.Parse(minX.ToString()), float.Parse(minY.ToString()));
                    pScalePoints[1] = new PointF(float.Parse(maxX.ToString()), float.Parse(maxY.ToString()));
                    xrfEdit.ScalePoints = pScalePoints;

                    //pfarr = new PointF[lstActiveSample.Count];
                    pfarr = new PointF[lstActivePoint.Count];
                    lstActiveppl = new List<PointPairList>();

                    for (int i = 0; i < lstActivePoint.Count; i++)
                    {
                        PointPairList ppl = new PointPairList();
                        LineItem li = xrfEdit.GraphPane.AddCurve("", ppl, Color.Red, SymbolType.Square);
                        li.Symbol.Fill = new Fill(Color.Red);
                        li.Symbol.Size = 8f;
                        ppl.Add(lstActivePoint[i].X, lstActivePoint[i].Y);
                        lstActiveppl.Add(ppl);
                        pfarr[i] = new PointF(float.Parse(lstActivePoint[i].X.ToString()), float.Parse(lstActivePoint[i].Y.ToString()));
                    }
                    lstNoActiveppl = new List<PointPairList>();
                    for (int i = 0; i < lstNoActivePoin.Count; i++)
                    {
                        PointPairList ppl = new PointPairList();
                        LineItem li = xrfEdit.GraphPane.AddCurve("", ppl, Color.Blue, SymbolType.Square);
                        li.Symbol.Fill = new Fill(Color.Blue);
                        li.Symbol.Size = 8f;
                        ppl.Add(lstNoActivePoin[i].X, lstNoActivePoin[i].Y);
                        lstNoActiveppl.Add(ppl);
                    }
                    if (workCurveCurrent.ElementList.Items[SelectDgvIndex].FpCalculationWay == FpCalculationWay.LinearWithAnIntercept)
                    {
                        X = new double[2];
                        xrfEdit.CalculateCurve(lstActivePoint.ToArray(), 1, false, X);
                        xrfEdit.CalculationWay = CalculationWay.Linear;
                        pfarr = new PointF[2];
                        minY = minX * X[0] + X[1];
                        maxY = maxX * X[0] + X[1];
                        pfarr[0] = new PointF(float.Parse(minX.ToString()), float.Parse(minY.ToString()));
                        pfarr[1] = new PointF(float.Parse(maxX.ToString()), float.Parse(maxY.ToString()));
                        xrfEdit.CalculationPoints = pfarr;
                    }
                    else if (workCurveCurrent.ElementList.Items[SelectDgvIndex].FpCalculationWay == FpCalculationWay.LinearWithoutAnIntercept)
                    {
                        X = new double[2];
                        xrfEdit.CalculateCurve(lstActivePoint.ToArray(), 1, true, X);
                        xrfEdit.CalculationWay = CalculationWay.Linear;
                        pfarr = new PointF[2];
                        minY = minX * X[0];
                        maxY = maxX * X[0];
                        pfarr[0] = new PointF(float.Parse(minX.ToString()), float.Parse(minY.ToString()));
                        pfarr[1] = new PointF(float.Parse(maxX.ToString()), float.Parse(maxY.ToString()));
                        xrfEdit.CalculationPoints = pfarr;
                    }
                    else if (workCurveCurrent.ElementList.Items[SelectDgvIndex].FpCalculationWay == FpCalculationWay.SquareWithAnIntercept)
                    {
                        xrfEdit.IZero = false;
                        xrfEdit.CalculationWay = CalculationWay.Conic;
                        var pp = pfarr.Distinct().ToArray();
                        if (pp.Length < 3)
                        {
                            SkyrayMsgBox.Show(Info.NeedConicPoint);
                        }
                        else
                        {
                            xrfEdit.CalculationPoints = pp;
                        }
                    }
                    else if (workCurveCurrent.ElementList.Items[SelectDgvIndex].FpCalculationWay == FpCalculationWay.SquareWithoutAnIntercept)
                    {
                        xrfEdit.IZero = true;
                        xrfEdit.CalculationWay = CalculationWay.Conic;
                        var pp = pfarr.Distinct().ToArray();
                        if (pp.Length < 3)
                        {
                            SkyrayMsgBox.Show(Info.NeedConicPoint);
                        }
                        else
                        {
                            xrfEdit.CalculationPoints = pp;
                        }
                    }
                }
                #endregion
                else //计算含量和实际含量
                {
                    List<StandSample> lsTemp;
                    if (!workCurveCurrent.GetFpStandSamplesContent(workCurveCurrent.ElementList, SelectDgvIndex, out lsTemp,null))
                    {
                        return;
                    }
                    if (lsTemp != null && lsTemp.Count > 0)
                    {
                        PointF[] tempPoints = new PointF[lsTemp.Count];
                        for (int i = 0; i < lsTemp.Count; i++)
                        {
                            StandSample standi = workCurveCurrent.ElementList.Items[SelectDgvIndex].Samples.First(w => w.SampleName == lsTemp[i].SampleName);

                            PointF tempPoint = new PointF(float.Parse(lsTemp[i].Y), (float)lsTemp[i].TheoryX);
                            if (standi.Active)
                                lstActivePoint.Add(tempPoint);
                            else
                                lstNoActivePoin.Add(tempPoint);
                            tempPoints[i] = tempPoint;
                        }
                        minX = tempPoints.OrderBy(data => data.X).ToList()[0].X;
                        maxX = tempPoints.OrderByDescending(data => data.X).ToList()[0].X;
                        minY = tempPoints.OrderBy(data => data.Y).ToList()[0].Y;
                        maxY = tempPoints.OrderByDescending(data => data.Y).ToList()[0].Y;
                        pScalePoints[0] = new PointF(float.Parse(minX.ToString()), float.Parse(minY.ToString()));
                        pScalePoints[1] = new PointF(float.Parse(maxX.ToString()), float.Parse(maxY.ToString()));
                        xrfEdit.ScalePoints = pScalePoints;
                    }
                    pfarr = new PointF[lstActiveSample.Count];
                    lstActiveppl = new List<PointPairList>();
                    for (int i = 0; i < lstActivePoint.Count; i++)
                    {
                        PointPairList ppl = new PointPairList();
                        LineItem li = xrfEdit.GraphPane.AddCurve("", ppl, Color.Red, SymbolType.Square);
                        li.Symbol.Fill = new Fill(Color.Red);
                        li.Symbol.Size = 8f;
                        ppl.Add(lstActivePoint[i].X, lstActivePoint[i].Y);
                        lstActiveppl.Add(ppl);
                        pfarr[i] = new PointF(float.Parse(lstActivePoint[i].X.ToString()), float.Parse(lstActivePoint[i].Y.ToString()));
                    }
                    lstNoActiveppl = new List<PointPairList>();
                    for (int i = 0; i < lstNoActivePoin.Count; i++)
                    {
                        PointPairList ppl = new PointPairList();
                        LineItem li = xrfEdit.GraphPane.AddCurve("", ppl, Color.Blue, SymbolType.Square);
                        li.Symbol.Fill = new Fill(Color.Blue);
                        li.Symbol.Size = 8f;
                        ppl.Add(lstNoActivePoin[i].X, lstNoActivePoin[i].Y);
                        lstNoActiveppl.Add(ppl);
                    }
                    X = new double[2];
                    xrfEdit.CalculateCurve(lstActivePoint.ToArray(), 1, false, X);
                    xrfEdit.CalculationWay = CalculationWay.Linear;
                    pfarr = new PointF[2];
                    minY = minX * X[0] + X[1];
                    maxY = maxX * X[0] + X[1];
                    pfarr[0] = new PointF(float.Parse(minX.ToString()), float.Parse(minY.ToString()));
                    pfarr[1] = new PointF(float.Parse(maxX.ToString()), float.Parse(maxY.ToString()));
                    xrfEdit.CalculationPoints = pfarr;
                }
                this.xrfEdit.AxisChange();
                this.xrfEdit.Refresh();

                if (radIntensityVS.Checked && (workCurveCurrent.ElementList.Items[SelectDgvIndex].FpCalculationWay == FpCalculationWay.SquareWithAnIntercept || workCurveCurrent.ElementList.Items[SelectDgvIndex].FpCalculationWay == FpCalculationWay.SquareWithoutAnIntercept))
                {
                    var coffs = this.xrfEdit.Coeff;
                    if (coffs != null)
                    {
                        string sFormat = "f6";
                        var a = coffs[0].ToString(sFormat);
                        var b = coffs[1] >= 0 ? "+" + coffs[1].ToString(sFormat) : coffs[1].ToString(sFormat);
                        var c = coffs[2] >= 0 ? "+" + coffs[2].ToString(sFormat) : coffs[2].ToString(sFormat);
                        //this.lblEquationExpression.Text = "y=" + a + "x²" + b + "x" + c;
                        //理论前实际值后
                        //this.grpOnes.Visible = true;
                        PointF[] pp = new PointF[pfarr.Length];
                        for (int i = 0; i < pfarr.Length; i++)
                        {
                            float x = float.Parse((pfarr[i].X * pfarr[i].X * coffs[0] + pfarr[i].X * coffs[1] + coffs[2]).ToString());
                            pp[i] = new PointF(x, pfarr[i].Y);
                        }
                        var R = Math.Pow(MathFunc.CalcCoefficientR(pp), 2d);
                        this.txtCoee.Text = ((double.IsNaN(R))?0:R).ToString("f10");
                    }
                }
                else
                {
                    PointF[] pp = new PointF[lstActivePoint.Count];
                    for (int i = 0; i < lstActivePoint.Count; i++)
                    {
                        float x = float.Parse((lstActivePoint[i].X * X[0] + X[1]).ToString());
                        pp[i] = new PointF(x, lstActivePoint[i].Y);
                    }
                    var R = Math.Pow(MathFunc.CalcCoefficientR(pp), 2d);
                    this.txtCoee.Text = ((double.IsNaN(R)) ? 0 : R).ToString("f10");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private double[] X;
        /// <summary>
        /// 计算斜率截距
        /// </summary>
        private void CalLinearParam(List<PointF> lsTemp)
        {
            Intercept = 0;
            Slope = 0;
            double x = 0;
            double y = 0;
            double xy = 0;
            double xx = 0;
            int count = 0;
            if (lsTemp.Count == 0)
            {
                return;
            }
            for (int i = 0; i < lsTemp.Count; i++)
            {
                x += lsTemp[i].X;
                y += lsTemp[i].Y;
                xy += lsTemp[i].X * lsTemp[i].Y;
                xx += lsTemp[i].X * lsTemp[i].X;
                count++;
            }
            if ((count * xx - x * x) != 0)
            {
                Slope = (count * xy - x * y) / (count * xx - x * x);
                Intercept = (y - Slope * x) / count;
            }
        }

        private void radOneNoForcedOrigin_CheckedChanged(object sender, EventArgs e)
        {
            if (radOneNoForcedOrigin.Checked)
            {
                workCurveCurrent.ElementList.Items[SelectDgvIndex].FpCalculationWay = FpCalculationWay.LinearWithAnIntercept;
                DrawGraph();
            }
            if (radOneForcedOrigin.Checked)
            {
                workCurveCurrent.ElementList.Items[SelectDgvIndex].FpCalculationWay = FpCalculationWay.LinearWithoutAnIntercept;
                DrawGraph();
            }
            if (radTwoNoForcedOrigin.Checked)
            {
                workCurveCurrent.ElementList.Items[SelectDgvIndex].FpCalculationWay = FpCalculationWay.SquareWithAnIntercept;
                DrawGraph();
            }
            if (radTwoForcedOrigin.Checked)
            {
                workCurveCurrent.ElementList.Items[SelectDgvIndex].FpCalculationWay = FpCalculationWay.SquareWithoutAnIntercept;
                DrawGraph();
            }
        }

        private void radIntensityVS_CheckedChanged(object sender, EventArgs e)
        {
            if (radIntensityVS.Checked)
            {
                grpFpIntensityVs.Visible = true;
                DrawGraph();
            }
        }


        private void radContentVS_CheckedChanged(object sender, EventArgs e)
        {
            if (radContentVS.Checked)
            {
                grpFpIntensityVs.Visible = false;
                DrawGraph();//画图
            }
        }

        #region datagridview中单击CheckBox获取值 20110518 Paul
        private void dgwSample_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1) return;
            if (dgwSample.Columns[e.ColumnIndex].Name.Equals("ColIsMatch"))
            {
                if (IschkColIsMatchClicked)
                {
                    bool IschkMatch = true;
                    foreach (DataGridViewRow row in dgwSample.Rows)
                    {
                        IschkMatch = IschkMatch && ((bool)row.Cells[e.ColumnIndex].Value);
                    }
                    this.chkColIsMatch.Checked = IschkMatch; 
                }
                DataGridViewCheckBoxCell dgvCheckBoxCell = this.dgwSample.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;

                foreach (var item in workCurveCurrent.ElementList.Items)
                {
                    string sElementSpecName = (dgwSample.Rows[e.RowIndex].Cells["ColSampleName"] == null) ? "" : dgwSample.Rows[e.RowIndex].Cells["ColSampleName"].Value.ToString();
                    var samplesList = item.Samples.ToList().FindAll(w => w.SampleName == sElementSpecName);
                    if (samplesList == null || samplesList.Count == 0) continue;
                    foreach (StandSample sSample in samplesList)
                    {
                        sSample.IsMatch = (bool)dgvCheckBoxCell.Value;//赋值;
                    }
                }
            }
            ////修改：何晓明 20110922 激活与全部激活不同步
            //if (dgwSample.Columns[e.ColumnIndex].Name.Equals(ColActive.Name) && IschkColActiveClicked)
            //{
            //    //DataGridViewCheckBoxCell dgvCheckBoxCell = this.dgwSample.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;
            //    bool checkFlag = true;
            //    foreach (DataGridViewRow row in dgwSample.Rows)
            //    {
            //        //if (row.Index != e.RowIndex)
            //            checkFlag = checkFlag && ((bool)row.Cells[ColActive.Name].Value);
            //    }
            //    //this.chkColActive.Tag = checkFlag && ((bool)dgvCheckBoxCell.Value);
            //    this.chkColActive.Checked = checkFlag;//&& ((bool)dgvCheckBoxCell.Value);
            //}            
            ////
        }
        #endregion

        #region 清空匹配谱

        //private void ToolStripMenuItemDeleColPureElement_Click(object sender, EventArgs e)
        //{
        //    if (iSelectRowIndex != -1 && iSelectColumnIndex != -1)
        //    {
        //        dgwSample[iSelectColumnIndex, iSelectRowIndex].Value = "";
        //        string sElementSpecName = (dgwSample.Rows[iSelectRowIndex].Cells["ColSampleName"] == null) ? "" : dgwSample.Rows[iSelectRowIndex].Cells["ColSampleName"].Value.ToString();
        //        foreach (var item in workCurveCurrent.ElementList.Items)
        //        {
        //            var samplesList = item.Samples.ToList().FindAll(w => w.SampleName == sElementSpecName);
        //            if (samplesList == null || samplesList.Count == 0) continue;
        //            foreach (StandSample sSample in samplesList)
        //            {
        //                sSample.MatchSpecName = "";
        //                //sSample.MatchSpecListId = 0;
        //                sSample.IsMatch = false;
        //            }
        //        }
        //        InitStandSample(false);
        //    }
        //}

        //int iSelectRowIndex = -1;
        //int iSelectColumnIndex = -1;

        //private void dgwSample_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    iSelectRowIndex = -1;
        //    iSelectColumnIndex = -1;
        //    if (e.Button == MouseButtons.Right && dgwSample.Columns[e.ColumnIndex].Name.Equals("ColumnMatchSpec"))
        //    {
        //        if (e.RowIndex >= 0)
        //        {
        //            dgwSample.ClearSelection();
        //            dgwSample.Rows[e.RowIndex].Selected = true;
        //            iSelectRowIndex = e.RowIndex;
        //            iSelectColumnIndex = e.ColumnIndex;

        //        }
        //    }
        //}

        #endregion
        #region ThickData
        private void btnAddThick_Click(object sender, EventArgs e)
        {
            //if (WorkCurveHelper.SaveType == 0)
            //{
            //    SelectSample sample = new SelectSample(AddSpectrumType.OpenStandardSpec);//标样谱
            //    sample.IsCaculate = false;
            //    WorkCurveHelper.OpenUC(sample, false);
            //    if (sample.DialogResult != DialogResult.OK)//未选择标样
            //    { return; }
            //    specListSelected = sample.SelectedSpecList;
            //}
            //else
            //{
            //    var returnEntiy = DifferenceDevice.interClassMain.OpenFileDialog(false);
            //    if (returnEntiy == null)
            //        return;
            //    specListSelected.Add(returnEntiy);
            //}
            List<SpecListEntity> returnResult = EDXRFHelper.GetReturnSpectrum(false,false);
            if (returnResult == null || returnResult.Count == 0)
                return;
            specListSelected = returnResult;
            var items = workCurveCurrent.ElementList.Items;
            var eleSampleNames = from p in workCurveCurrent.ElementList.Items[0].Samples select p.SampleName;

            var ddd = from p in specListSelected
                      where eleSampleNames.Contains(p.Name) == false
                      select p;
            int TotalLayer = workCurveCurrent.ElementList.Items.OrderByDescending(data => data.LayerNumber).ToList<CurveElement>()[0].LayerNumber;//共有层数
            foreach (var specList in ddd)
            {
                try
                {
                    if (WorkCurveHelper.isShowEncoder)
                    {

                        if (WorkCurveHelper.IsPureElemCurrentUnify)
                            DifferenceDevice.TransHeightPureZero(workCurveCurrent, specList.Height, false, specList, 0);
                        else
                            DifferenceDevice.TransHeightPureByZero(workCurveCurrent, specList.Height, false, specList, 0);
                    }
                    workCurveCurrent.CaculateIntensity(specList);
                }
                catch (Exception ex)
                {
                    Msg.Show(specList.SampleName + ex.Message);
                    continue;
                }
                foreach (var item in items)
                {
                    float con = 0;
                    con = item.ContentUnit == ContentUnit.per ? 100 : 1000000;
                    if (item.ContentUnit == ContentUnit.per)
                    {
                        con = 100;
                    }
                    else if (item.ContentUnit == ContentUnit.ppm)
                    {
                        con = 1000000;
                    }
                    else
                    {
                        con = 1000;
                    }
                    double dblDensity = Atoms.AtomList.Find(w => w.AtomName.ToUpper().Equals(item.Caption.ToUpper())).AtomDensity;
                    var sam = StandSample.New.Init(specList.Name, specList.Height.ToString(), specList.CalcAngleHeight.ToString(), item.Intensity.ToString(), con.ToString(), "0", true, item.Caption, item.LayerNumber, TotalLayer, Info.MultiLayer, dblDensity, "0");
                    item.Samples.Add(sam);
                }
                this.dvgSampleThick.Rows.Add(specList.Name);
            }
            var fff = from p in specListSelected
                      where eleSampleNames.Contains(p.Name)
                      select p;
            foreach (var specList in fff)
            {
                try
                {
                    workCurveCurrent.CaculateIntensity(specList);
                }
                catch (Exception ex)
                {
                    Msg.Show(specList.SampleName + ex.Message);
                    continue;
                }
                foreach (var item in items)
                {
                    var itemTarget = item.Samples.First(x => x.SampleName == specList.Name);
                    itemTarget.X = item.Intensity.ToString();
                }
            }

        }
        private void btnDeleteThick_Click(object sender, EventArgs e)
        {
            if (this.dvgSampleThick.SelectedRows.Count == 0)
                return;
            string sampleName = this.dvgSampleThick.SelectedRows[0].Cells[0].Value.ToString();
            foreach (var item in workCurveCurrent.ElementList.Items)
            {
                StandSample ss = item.Samples.ToList().Find(s => s.SampleName == sampleName);
                int index = item.Samples.IndexOf(ss);
                if (ss != null)
                {
                    item.Samples.RemoveAt(index);
                }
            }
            this.dvgSampleThick.Rows.Remove(this.dvgSampleThick.SelectedRows[0]);
        }

        private void btnApplicationThick_Click(object sender, EventArgs e)
        {
            //if (JudgeMatchAndFile())
            //{
            //    SkyrayMsgBox.Show(Info.strJudgeMatchAndFile);
            //    return;
            //}

            if (ExitLevel()) return;
            int index = this.dgvwElement.SelectedRows[0].Index;
            TranslateUnit();
            workCurveCurrent.Save();
            WorkCurveHelper.WorkCurveCurrent = workCurveCurrent;
            LoadMethod(false);
            this.dgvwElement.Rows[index].Selected = true;
            WorkCurveHelper.WorkCurveCurrent = workCurveCurrent;
        }

        private void btnSubmitThick_Click(object sender, EventArgs e)
        {
            btnSave_Click(null, null);
        }

        private void btnCacelThick_Click(object sender, EventArgs e)
        {
            if (this.ParentForm != null)
                this.ParentForm.DialogResult = this.dialogResult = DialogResult.OK;
            EDXRFHelper.GotoMainPage(this);//返回主界面
        }

        private void btnUpThick_Click(object sender, EventArgs e)
        {
            btnSave_Click(null, null);
            upflag = true;
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            btnSave_Click(null, null);
            flag = true;
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            //foreach (DataGridViewRow row in dvgContentIntensity.Rows)
            //{
            //    row.Cells["ColActiveThick"].Value = chkSelectAll.Checked;
            //}
        }

        private void dvgThick_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1)
                return;
            if (!this.tabcwEdit.TabPages.Contains(this.tabThickData))
                return;
            var items = workCurveCurrent.ElementList.Items;
            if (dvgThick.Columns[e.ColumnIndex].Name == "dgvThickCol")
            {
                foreach (var item in items)
                {
                    var itemTarget = item.Samples.First(x => x.SampleName == this.dvgSampleThick.CurrentCell.Value.ToString());
                    try
                    {
                        if (itemTarget != null && item.LayerNumBackUp ==  dvgThick["dgvThickLevel", e.RowIndex].Value.ToString())
                            itemTarget.Z = dvgThick[e.ColumnIndex, e.RowIndex].Value.ToString();
                    }
                    catch {
                        dvgThick[e.ColumnIndex, e.RowIndex].Value = itemTarget.Z;
                       };
                }
            }
        }

        private void dvgContentIntensity_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1)
                return;
            if (!this.tabcwEdit.TabPages.Contains(this.tabThickData))
                return;
            var items = workCurveCurrent.ElementList.Items;
            if (dvgContentIntensity.Columns[e.ColumnIndex].Name == "dgvCIntensity")
            {
                foreach (var item in items)
                {
                    if (item.Caption == dvgContentIntensity.Rows[e.RowIndex].Cells["dgvCCaption"].Value.ToString() && item.LayerNumBackUp == dvgThick.CurrentRow.Cells["dgvThickLevel"].Value.ToString())
                    {
                        var itemTarget = item.Samples.First(x => x.SampleName == this.dvgSampleThick.CurrentCell.Value.ToString());
                        try
                        {
                            if (itemTarget != null)
                                itemTarget.X = dvgContentIntensity[e.ColumnIndex, e.RowIndex].Value.ToString();
                        }
                        catch {

                            dvgContentIntensity[e.ColumnIndex, e.RowIndex].Value = itemTarget.X;
                        };
                        break;
                    }
                }
            }
            else if(dvgContentIntensity.Columns[e.ColumnIndex].Name == "dgvCContent")
            {
                int curlayer = 1;
                foreach (var item in items)
                {
                    if (item.Caption == dvgContentIntensity.Rows[e.RowIndex].Cells["dgvCCaption"].Value.ToString() && item.LayerNumBackUp == dvgThick.CurrentRow.Cells["dgvThickLevel"].Value.ToString())
                    {
                        curlayer = item.LayerNumber;
                        var itemTarget = item.Samples.First(x => x.SampleName == this.dvgSampleThick.CurrentCell.Value.ToString());
                        try
                        {
                            //float totalContent = 0f;
                            //foreach (DataGridViewRow row in dvgContentIntensity.Rows)
                            //{
                            //    totalContent += float.Parse(row.Cells["dgvCContent"].Value.ToString());
                            //}
                            if (itemTarget != null)
                            {
                                itemTarget.Y = dvgContentIntensity[e.ColumnIndex, e.RowIndex].Value.ToString();
                            }
                            //else
                            //{
                            //    dvgContentIntensity[e.ColumnIndex, e.RowIndex].Value = itemTarget.Y;
                            //}
                        }
                        catch { dvgContentIntensity[e.ColumnIndex, e.RowIndex].Value = itemTarget.Y; };
                        break;
                    }
                }
                double dbldensity = 0;
                double dblSumY = 0;
                double tempCal = 0;
                for (int tempi = 0; tempi < items.Count; tempi++)
                {
                    if (items[tempi].LayerNumber == curlayer)
                    {
                        StandSample stemp = items[tempi].Samples.First(w => w.SampleName == this.dvgSampleThick.CurrentCell.Value.ToString());
                        if (stemp != null && stemp.Density > 0 && stemp.Y != null && double.Parse(stemp.Y) > 0)
                        {
                            tempCal += double.Parse(stemp.Y) / stemp.Density;
                            dblSumY += double.Parse(stemp.Y);
                        }
                    }
                }
                if (tempCal > 0)
                    dbldensity = dblSumY / tempCal;
                dvgThick.CurrentRow.Cells["dgvThickDensity"].Value = dbldensity.ToString("F4");
                    
            }
            else if (dvgContentIntensity.Columns[e.ColumnIndex].Name == "ColActiveThick")
            {
                foreach (var item in items)
                {
                    if (item.Caption == dvgContentIntensity.Rows[e.RowIndex].Cells["dgvCCaption"].Value.ToString() && item.LayerNumBackUp == dvgThick.CurrentRow.Cells["dgvThickLevel"].Value.ToString())
                    {
                        var itemTarget = item.Samples.First(x => x.SampleName == this.dvgSampleThick.CurrentCell.Value.ToString());
                        try
                        {
                            if (itemTarget != null)
                                itemTarget.Active = bool.Parse(dvgContentIntensity[e.ColumnIndex, e.RowIndex].Value.ToString());
                        }
                        catch { dvgContentIntensity[e.ColumnIndex, e.RowIndex].Value = itemTarget.Active; };
                    }
                }
            }
            else if (dvgContentIntensity.Columns[e.ColumnIndex].Name == "dgvCDensity")//修改元素密度，以及修改膜层密度的显示。2013-06-21
            {
                int curlayer = 1;
                foreach (var item in items)
                {
                    if (item.Caption == dvgContentIntensity.Rows[e.RowIndex].Cells["dgvCCaption"].Value.ToString() && item.LayerNumBackUp == dvgThick.CurrentRow.Cells["dgvThickLevel"].Value.ToString())
                    {
                        curlayer = item.LayerNumber;
                        var itemTarget = item.Samples.First(x => x.SampleName == this.dvgSampleThick.CurrentCell.Value.ToString());
                        try
                        {
                            if (itemTarget != null)
                                itemTarget.Density = double.Parse(dvgContentIntensity[e.ColumnIndex, e.RowIndex].Value.ToString());
                        }
                        catch { dvgContentIntensity[e.ColumnIndex, e.RowIndex].Value = itemTarget.Density.ToString("F4"); };
                        break;
                    }
                }

                double dbldensity = 0;
                double dblSumY = 0;
                double tempCal = 0;
                for (int tempi = 0; tempi < items.Count; tempi++)
                {
                    if (items[tempi].LayerNumber == curlayer)
                    {
                        StandSample stemp = items[tempi].Samples.First(w => w.SampleName == this.dvgSampleThick.CurrentCell.Value.ToString());
                        if (stemp != null && stemp.Density > 0 && stemp.Y != null && double.Parse(stemp.Y) > 0)
                        {
                            tempCal += double.Parse(stemp.Y) / stemp.Density;
                            dblSumY += double.Parse(stemp.Y);
                        }
                    }
                }
                if (tempCal > 0)
                    dbldensity = dblSumY / tempCal;
                dvgThick.CurrentRow.Cells["dgvThickDensity"].Value= dbldensity.ToString("F4");
            }
        }

        private void dvgSampleThick_SelectionChanged(object sender, EventArgs e)
        {
            if (!this.tabcwEdit.TabPages.Contains(this.tabThickData))
                return;
            this.dvgThick.Rows.Clear();
            Dictionary<string, float> layer = new Dictionary<string, float>();
            foreach (var element in workCurveCurrent.ElementList.Items)
            {
                StandSample samples = element.Samples.ToList().Find(w => w.SampleName == this.dvgSampleThick.CurrentCell.Value.ToString());
                float thickNess = 0f;
                //if (samples != null && !layer.TryGetValue(element.LayerNumBackUp, out thickNess))
                if (samples != null)
                {
                  
                    if(element.LayerNumBackUp =="基材")
                    {
                        element.LayerNumBackUp = Info.Substrate;
                    }
                    else if (element.LayerNumBackUp =="第一层")
                    {
                        element.LayerNumBackUp = Info.FirstLayer;
                    }
                    else if(element.LayerNumBackUp =="第二层")
                    {
                        element.LayerNumBackUp = Info.SecondLayer;
                    }
                    else if (element.LayerNumBackUp == "第三层")
                    {
                        element.LayerNumBackUp = Info.ThirdLayer;
                    }
                    else if (element.LayerNumBackUp =="第四层")
                    {
                        element.LayerNumBackUp = Info.ForthLayer;
                    }
                    else if (element.LayerNumBackUp == "第五层")
                    {
                        element.LayerNumBackUp = Info.FifthLayer;
                    }

                    
                    if (!layer.TryGetValue(element.LayerNumBackUp, out thickNess))//更改同层上多元素的错误
                    {
                        layer.Add(element.LayerNumBackUp, float.Parse(samples.Z.ToString()));
                        double dbldensity = 0;
                        double dblSumY = 0;
                        double tempCal = 0;
                        for (int tempi = 0; tempi < workCurveCurrent.ElementList.Items.Count; tempi++)
                        {
                            if (workCurveCurrent.ElementList.Items[tempi].LayerNumber == element.LayerNumber)
                            {
                                StandSample stemp = workCurveCurrent.ElementList.Items[tempi].Samples.First(w => w.SampleName == samples.SampleName);
                                if (stemp != null && stemp.Density > 0 && stemp.Y != null && double.Parse(stemp.Y)>0)
                                {
                                    tempCal += double.Parse(stemp.Y) / stemp.Density;
                                    dblSumY += double.Parse(stemp.Y);
                                }
                            }
                        }
                        if (tempCal > 0)
                            dbldensity = dblSumY / tempCal;
                        this.dvgThick.Rows.Add(element.LayerNumBackUp, samples.Z,dbldensity.ToString("F4"));
                    }
                    
                }
            }
            SetSeleAll();
        }

        private void dvgThick_SelectionChanged(object sender, EventArgs e)
        {
            if(this.dvgSampleThick.CurrentCell != null)
                RefreshThick(this.dvgSampleThick.CurrentCell.Value.ToString(), this.dvgThick.CurrentRow.Cells["dgvThickLevel"].Value.ToString());
            SetSeleAll();
        }

        private void RefreshThick(string standSampleName,string standSampleLevel)
        {
            if (!this.tabcwEdit.TabPages.Contains(this.tabThickData))
                return;
            this.dvgContentIntensity.Rows.Clear();
            List<CurveElement> listCurve = workCurveCurrent.ElementList.Items.ToList().FindAll(delegate(CurveElement ww) { return ww.LayerNumBackUp == standSampleLevel; });
            foreach (CurveElement temp in listCurve)
            {
                StandSample samples = temp.Samples.ToList().Find(w => w.SampleName == standSampleName);
                if (samples != null)
                    this.dvgContentIntensity.Rows.Add(temp.Caption, samples.X, samples.Y, samples.Active,samples.Density,samples.Height);
            }
        }
        #endregion

        private void chkColActive_Click(object sender, EventArgs e)
        {
            //IschkColActiveClicked = false;
            foreach (DataGridViewRow row in dgwSample.Rows)
            {
                row.Cells["ColActive"].Value = chkColActive.Checked;
            }
            if (workCurveCurrent.CalcType == CalcType.FP)
            {
                foreach (var item in workCurveCurrent.ElementList.Items)
                {
                    foreach (var ss in item.Samples)
                    {
                        ss.Active = chkColActive.Checked;
                    }
                }
            }
            //IschkColActiveClicked = true;
        }

        private void chkColIsMatch_Click(object sender, EventArgs e)
        {
            IschkColIsMatchClicked = false;
            foreach (DataGridViewRow row in dgwSample.Rows)
            {
                row.Cells["ColIsMatch"].Value = chkColIsMatch.Checked;
            }
            IschkColIsMatchClicked = true;
        }

        private void chkSelectAll_Click(object sender, EventArgs e)
        {
            IschkSelectAllClicked = false;
            foreach (DataGridViewRow row in dvgContentIntensity.Rows)
            {
                row.Cells["ColActiveThick"].Value = chkSelectAll.Checked;
            }
            IschkSelectAllClicked = true;
        }

        private void dvgContentIntensity_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1) return;
            if (dvgContentIntensity.Columns[e.ColumnIndex].Name.Equals("ColActiveThick") && IschkSelectAllClicked)
            {
                bool IschkActiveThick = true;
                 foreach (DataGridViewRow row in dvgContentIntensity.Rows)
                {
                    IschkActiveThick = IschkActiveThick && ((bool)row.Cells[e.ColumnIndex].Value);
                }
                 this.chkSelectAll.Checked = IschkActiveThick;           
            }
        }

        private void dvgContentIntensity_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (this.dvgContentIntensity.IsCurrentCellDirty) //有未提交的更改
            {
                this.dvgContentIntensity.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        /// <summary>
        /// paul 激活与全部激活同步问题 20111011
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgwSample_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1) return;
            //if (!dgwSample.Columns[e.ColumnIndex].Name.Equals("ColActive")) return;

            if (dgwSample.Columns[e.ColumnIndex].Name.Equals("ColActive"))
            {

                DataGridViewCell cell = dgwSample.Rows[e.RowIndex].Cells[e.ColumnIndex];
                bool ischeck1 = Convert.ToBoolean(cell.FormattedValue);
                bool ischeck2 = Convert.ToBoolean(cell.EditedFormattedValue);
                if (!ischeck2)
                {
                    this.chkColActive.Checked = false;
                }
                else
                {
                    bool checkFlag = true;
                    //foreach (DataGridViewRow row in dgwSample.Rows)
                    //{
                    //    if (row.Index != e.RowIndex)
                    //        checkFlag = checkFlag && ((bool)row.Cells[ColActive.Name].Value);
                    //}
                    this.chkColActive.Checked = checkFlag;
                }
                if (workCurveCurrent.CalcType == CalcType.FP)
                {
                    string sampleName = dgwSample["ColSampleName", e.RowIndex].Value.ToString();
                    foreach (CurveElement ce in workCurveCurrent.ElementList.Items)
                    {
                        foreach (var ss in ce.Samples)
                        {
                            if (ss.SampleName.Equals(sampleName))
                                ss.Active = this.chkColActive.Checked;
                        }
                    }
                }
            }
            else if (dgwSample.Columns[e.ColumnIndex].Name.Equals("ColIsMatch"))
            {

                DataGridViewCell cell = dgwSample.Rows[e.RowIndex].Cells[e.ColumnIndex];
                bool ischeck1 = Convert.ToBoolean(cell.FormattedValue);
                bool ischeck2 = Convert.ToBoolean(cell.EditedFormattedValue);
                if (!ischeck2) this.chkColIsMatch.Checked = false;
                else
                {
                    bool checkFlag = true;
                    foreach (DataGridViewRow row in dgwSample.Rows)
                    {
                        if (row.Index != e.RowIndex)
                            checkFlag = checkFlag && ((bool)row.Cells[ColIsMatch.Name].Value);
                    }
                    this.chkColIsMatch.Checked = checkFlag;
                }
            }
            
        }
        private void dgvwElement_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvwElement_CellMouseDown(null,new DataGridViewCellMouseEventArgs(e.ColumnIndex,e.RowIndex,Cursor.Position.X,Cursor.Position.Y,
                new MouseEventArgs(MouseButtons.Left,1,Cursor.Position.X,Cursor.Position.Y,0)));
        }

        private void radLineThick_CheckedChanged(object sender, EventArgs e)
        {
            if (radLineThick.Checked)
            {
                workCurveCurrent.ElementList.ThCalculationWay = ThCalculationWay.ThLinear;
                foreach (var element in workCurveCurrent.ElementList.Items)
                {
                    element.CalculationWay = CalculationWay.Linear;//设置计算方法
                }
            }
        }

        private void radInsertThick_CheckedChanged(object sender, EventArgs e)
        {
            if (radInsertThick.Checked)
            {
                workCurveCurrent.ElementList.ThCalculationWay = ThCalculationWay.ThInsert;
                foreach (var element in workCurveCurrent.ElementList.Items)
                {
                    element.CalculationWay = CalculationWay.Insert;//设置计算方法
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            //const int WM_DEVICECHANGE = 0x219;
            //修改： 何晓明 20100714 软件关闭时几率性dgvDevice.RowCount=0
            const int WM_SYSTEMCOMMAND = 0x0112;
            const int SC_CLOSE = 0xF060;

            // const int CUSTOM_MESSAGE = 0X400 + 2;//自定义消息
            // const int CUSTOM_MESSAGE_HIDE = 0X400 + 3;
            //
            switch (m.Msg)
            {
                

                case DeviceInterface.CUSTOM_MESSAGE: //处理消息
                    Console.WriteLine("edit显示");
                    ShowWindow(FindWindow(null, "ProcessBox"), 1);

                    break;
                case DeviceInterface.CUSTOM_MESSAGE_HIDE:
                    Console.WriteLine("edit隐藏");
                    ShowWindow(FindWindow(null, "ProcessBox"), 0);
                    break;

            }
            base.WndProc(ref m);
        }

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        private void dgvwElement_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != dgvEleLayerDensity.Index) return;
            var items = workCurveCurrent.ElementList.Items;
            foreach (var item in items)
            {
                if (item.Caption == dgvwElement.Rows[e.RowIndex].Cells[ColElement.Index].Value.ToString())
                {
                    try
                    {
                        item.LayerDensity = Convert.ToDouble(dgvwElement[e.ColumnIndex, e.RowIndex].Value);
                    }
                    catch
                    {

                        dvgContentIntensity[e.ColumnIndex, e.RowIndex].Value = item.LayerDensity;
                    };
                    break;
                }
            }
        }

        private void dvgSampleThick_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
