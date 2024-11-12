using System;
using System.Windows.Forms;
using Lephone.Data.Common;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;
using Skyray.Controls;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Reflection;
using System.Collections;
using Lephone.Data.Definition;
using System.Data;
using System.Data.SQLite;

namespace Skyray.UC
{
    /// <summary>
    /// 工作曲线类
    /// </summary>
    public partial class UCCurve : Skyray.Language.UCMultiple
    {

        #region Fields

        /// <summary>
        /// 工作曲线
        /// </summary>
        private DbObjectList<WorkCurve> lstWorkCurve;

        /// <summary>
        /// 测量条件
        /// </summary>
        private DbObjectList<Condition> lstCondition;

        /// <summary>
        /// 当前选中工作曲线
        /// </summary>
        private WorkCurve workCurveCurrent;

        /// <summary>
        /// 对工作曲线进行读写
        /// </summary>
        public WorkCurve WorkCurveCurrent
        {
            get { return workCurveCurrent; }
            set { workCurveCurrent = value; }
        }
        /// <summary>
        /// 对话结果
        /// </summary>
        public DialogResult DialogResult { get; set; }

        /// <summary>
        /// 打开工作曲线事件
        /// </summary>
        //public event Skyray.UC.EventDelegate.InitDeviceParameter OnInitDeviceParameter;

        private DbObjectList<WorkRegion> lstWorkRegion;

        private WorkRegion CurrentWorkRegion;

        #endregion

        #region Init
        /// <summary>
        /// 构造函数
        /// </summary>
        public UCCurve()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCCurve_Load(object sender, EventArgs e)
        {
            if (WorkCurveHelper.DeviceCurrent != null)
            {
                var deviceId = WorkCurveHelper.DeviceCurrent.Id;
                string sql = "select * from WorkCurve where Condition_Id in (select Id from condition where Type = 0 and Device_id = "
                    + deviceId + ") and FuncType=" + (int)WorkCurveHelper.DeviceFunctype + " order by LOWER(Name)";

                lstWorkCurve = WorkCurve.FindBySql(sql);
            }
            else
            {
                lstWorkCurve = WorkCurve.FindAll();
                lstCondition = Condition.Find(c => c.Type == ConditionType.Normal);
            }
            lstCondition = Condition.Find(c => c.Device.Id == WorkCurveHelper.DeviceCurrent.Id && c.Type == ConditionType.Normal);
            //lstCondition.ForEach(c =>
            //{
            //    if (c.Type == ConditionType.Match) c.Name = Info.Match;
            //    else if (c.Type == ConditionType.Intelligent) c.Name = Info.IntelligentCondition;
            //});

            this.dgvwCurveList.Columns["ColSimilarCurve"].Visible = false;
            this.dgvwCurveList.Columns["ColName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            if (DifferenceDevice.IsAnalyser)//SimilarCurveName
            {
                string sSimilarCurve = ReportTemplateHelper.LoadSpecifiedValue("EDX3000", "SimilarCurve");
                if (sSimilarCurve == "1")
                {
                    this.dgvwCurveList.Columns["ColSimilarCurve"].Visible = true;
                    this.dgvwCurveList.Columns["ColName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
                }
            } 
            WorkCurveToGrid();//加载曲线列表
            this.cboConditionList.DataSource = lstCondition;
            this.cboConditionList.DisplayMember = "Name";
            this.cboConditionList.ValueMember = "Id";
            this.cboConditionList.ColumnNames = "Name";
            this.cboConditionList.ColumnWidths = this.cboConditionList.Width.ToString();
            this.cboCurveType.Items.Clear();

            this.cboCurveType.Items.Add(CalcType.FP);
            this.cboCurveType.Items.Add(CalcType.PeakDivBase);
            if (DifferenceDevice.IsThick)
            {
                if (WorkCurveHelper.IsDppValidate)
                {
                    this.cboCurveType.Items.Add(CalcType.EC);
                   
                }
                this.dgvwCurveList.Columns["colRemarkName"].Visible = true;
            }

            this.cboCurveType.SelectedIndex = 0;
            var vv = User.Find(w => w.Name == FrmLogon.userName);
            ColIsDefaultWorkCurve.ReadOnly = (vv[0].Role.RoleType == 2 || vv[0].Role.RoleType == 1);
            if (vv.Count > 0 && vv[0].Role.RoleType == 2)
            {
                this.lblCurveName.Visible = false;
                this.txtCurveName.Visible = false;
                this.lblTestCondition.Visible = false;
                this.cboConditionList.Visible = false;
                this.cboCurveType.Visible = false;
                this.lblCurveType.Visible = false;
                this.btnAdd.Visible = false;
                this.btnDel.Visible = false;
                this.btwExportCurve.Visible = false;
                this.btWImport.Visible = false;
                this.btWCurveToElement.Visible = false;
                this.btnReName.Visible = false;
                
                ColMainElement.Visible = false;
            }           


            if (DifferenceDevice.IsRohs)
            {
                lblWorkArea.Visible = true;
                cboWorkArea.Visible = true;
                //ColCondition.Width = 100;
                //ColIsDefaultWorkCurve.Visible = true;
                lstWorkRegion = WorkRegion.FindAll();
                foreach (var region in lstWorkRegion)
                {
                    cboWorkArea.Items.Add(region.Name);
                }
                if (WorkCurveHelper.CurrentWorkRegion != null)
                {
                    cboWorkArea.Text = WorkCurveHelper.CurrentWorkRegion.Name;
                    CurrentWorkRegion = WorkCurveHelper.CurrentWorkRegion;
                }
                else if(lstWorkRegion.Count > 0)
                {
                    cboWorkArea.Text = lstWorkRegion[0].Name;
                    CurrentWorkRegion = lstWorkRegion[0];
                }
            }

            if (DifferenceDevice.IsRohs || DifferenceDevice.IsThick||WorkCurveHelper.MatchType == 0)
            {
                ColMainElement.Visible = false;
            }

           
                
        }

        #endregion

        #region Methods

        /// <summary>
        /// 获取设备ID
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        private int GetDeviceId(WorkCurve w)
        {
            return (int)w.Condition.Device.Id;
        }

        /// <summary>
        /// 工作曲线列表绑定至表格
        /// </summary>
        private void WorkCurveToGrid()
        {
            BindingSource bs = new BindingSource();
            for (int i = 0; i < lstWorkCurve.Count; i++)
            {
                if (!WorkCurveHelper.IsDppValidate)
                {
                    if (lstWorkCurve[i].CalcType == CalcType.FP || lstWorkCurve[i].CalcType ==CalcType.PeakDivBase)
                        bs.Add(lstWorkCurve[i]);
                }
                else
                {
                    bs.Add(lstWorkCurve[i]);
                }
            }
            this.dgvwCurveList.AutoGenerateColumns = false;
            this.dgvwCurveList.DataSource = bs;
            for (int i = 0; i < dgvwCurveList.Rows.Count; i++)
            {
                if (lstWorkCurve[i].Condition.Type == ConditionType.Intelligent)
                {
                    this.dgvwCurveList[2, i].Value = Info.IntelligentCondition;
                    continue;
                }
                if (lstWorkCurve[i].Condition.Type == ConditionType.Match)
                {
                    this.dgvwCurveList[2, i].Value = Info.Match;
                    continue;
                }
                if (lstWorkCurve[i].Condition.Type == ConditionType.Detection)
                {
                    this.dgvwCurveList[2, i].Value = Info.strDetection;
                    continue;
                }
            }

            SimilarCurve();
           
        }

        /// <summary>
        /// 绑定相识曲线
        /// </summary>
        private void SimilarCurve()
        {
            if (!this.ColSimilarCurve.Visible) return;

            DataTable dtSimilarCurve = new DataTable();
            DataColumn dcCurveName = new DataColumn("CurveName");//显示Name
            DataColumn dcCurveId = new DataColumn("CurveId");//绑定的Value
            dtSimilarCurve.Columns.Add(dcCurveName);
            dtSimilarCurve.Columns.Add(dcCurveId);

            dtSimilarCurve.Rows.Add("", 0);
            foreach (WorkCurve workcurve in lstWorkCurve)
            {
                dtSimilarCurve.Rows.Add(workcurve.Name, (int)workcurve.Id);
            }

            this.ColSimilarCurve.DataSource = dtSimilarCurve;
            this.ColSimilarCurve.ValueMember = "CurveId";
            this.ColSimilarCurve.DisplayMember = "CurveName";

            for (int i = 0; i < dgvwCurveList.Rows.Count; i++)
            {
                WorkCurve findWorkCurve = lstWorkCurve.ToList().Find(x => x.Name == dgvwCurveList.Rows[i].Cells["ColName"].Value.ToString()); 
                
                //绑定初始值显示Name，就要绑定Value
                ((DataGridViewComboBoxCell)dgvwCurveList.Rows[i].Cells["ColSimilarCurve"]).Value = findWorkCurve.SimilarCurveId;
            }
        }

        /// <summary>
        /// 选择曲线
        /// </summary>
        private void SelectCurve()
        {
            if (WorkCurveHelper.WorkCurveCurrent!=null && WorkCurveHelper.WorkCurveCurrent.ElementList != null)
                DifferenceDevice.interClassMain.PreviousRefSpecListIdStr = WorkCurveHelper.WorkCurveCurrent.ElementList.RefSpecListIdStr;
            WorkCurveCurrent = lstWorkCurve[dgvwCurveList.SelectedRows[0].Index];
            WorkCurveHelper.WorkCurveCurrent = WorkCurveCurrent;
            WorkCurveHelper.CalcType = WorkCurveCurrent.CalcType;
            WorkCurveCurrent.Condition.Device = WorkCurveHelper.DeviceCurrent;

            try
            {
                WorkCurveHelper.NaviItems.Find(w => w.Name == "EditElement").EnabledControl = true;
            }
            catch
            {
                Console.WriteLine("不存在EditElement");
            }
            //for (int i = 0; i < WorkCurveHelper.NaviItems.Count; i++)
            //{
            //    if (WorkCurveHelper.NaviItems[i].Name == "EditElement")
            //    {
            //        WorkCurveHelper.NaviItems[i].EnabledControl = true;
            //        break;
            //    }
            //}
            //add by chuyaqin 2011-07-23光斑的修改
            if (DifferenceDevice.interClassMain.skyrayCamera != null)
            {
                DifferenceDevice.interClassMain.skyrayCamera.FociIndex = WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList[0].CollimatorIdx-1;
            }

            //if (WorkCurveHelper.WorkCurveCurrent.FuncType == FuncType.XRF && WorkCurveHelper.WorkCurveCurrent.CalcType == CalcType.FP)
            //{
            //    bool calResult = false;
            //    FpWorkCurve fpwork = new FpWorkCurve();

            //    string strCurrentDir = System.IO.Directory.GetCurrentDirectory();
            //    System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            //    System.IO.File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\fpFiles\\temp.cal", "temp.cal", true);
            //    if (FpWorkCurve.FPSetRootPath("./FpFiles"))
            //    {
            //        if (FpWorkCurve.SetSourceData(WorkCurveHelper.WorkCurveCurrent.Condition, WorkCurveHelper.WorkCurveCurrent.Condition.Device.Tubes))
            //        {
            //            List<string> listSamples;
            //            calResult = fpwork.Calibrate(WorkCurveHelper.WorkCurveCurrent.ElementList, 0, out listSamples);
            //        }
            //    }

            //    System.IO.Directory.SetCurrentDirectory(strCurrentDir);

            //    if (!calResult)
            //    {
            //        Msg.Show(WorkCurveHelper.WorkCurveCurrent.Name + ": Failed to be calibrated.");
            //    }
            //}
        }

        #endregion

        #region Events

        /// <summary>
        /// 添加曲线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateHelper.IllegalCheck(txtCurveName))
            {
                if (cboConditionList.SelectedIndex < 0 || cboCurveType.SelectedIndex < 0)
                {
                    SkyrayMsgBox.Show(Info.IsNull);
                }
                else if (lstWorkCurve.Find(w => w.Name == txtCurveName.Text && w.Condition.Device.Id == WorkCurveHelper.DeviceCurrent.Id) != null)
                {
                    SkyrayMsgBox.Show(Info.ExistName);
                }
                else
                {
                    var curve = WorkCurve.New.Init(this.txtCurveName.Text, "", (CalcType)(this.cboCurveType.SelectedItem), WorkCurveHelper.DeviceFunctype, false, 0, false, false, false, false, false, 0, "", false, Info.strAreaDensityUnit,40,true);
                    var calibrationParam = CalibrationParam.New.Init(false, 45, 1, false, 2, 1, false, 1, 1, false, 1, 1, false, "",false,1,0,0);
                    calibrationParam.WorkCurve = curve;
                    curve.Condition = lstCondition[this.cboConditionList.SelectedIndex];
                    curve.ConditionName = lstCondition[this.cboConditionList.SelectedIndex].Name;
                    curve.FuncType = WorkCurveHelper.DeviceFunctype;
                    if (CurrentWorkRegion != null)
                        curve.WorkRegion = CurrentWorkRegion;
                    curve.SimilarCurveId = 0;
                    curve.SimilarCurveName = "";
                    curve.InCalSampName = "";
                    curve.TestTime = curve.Condition.DeviceParamList[0].PrecTime;
                    curve.Save();//保存新曲线
                    lstWorkCurve.Add(curve);
                    WorkCurveToGrid();//加载曲线列表
                    txtCurveName.Text = "";
                    dgvwCurveList.Rows[lstWorkCurve.Count - 1].Selected = true;

                    //添加到曲线列表中
                    //Curve curveList = new Curve();
                    //curveList.type = DataGridViewType.CurveList;
                    //curveList.IsValidate = false;
                    //CurveItem curveItem = new CurveItem(curve.Name, this.cboCurveType.SelectedItem.ToString(), curve.ConditionName, curve.Id);
                    //curveList.curveResult.Add(curveItem);
                    //MainForm.localTaskList.Add(curveList);
                    
                }
            }
        }

        private bool CopyWorkCurveInfo(WorkCurve beCopyCurve, WorkCurve returnCurve)
        {
            if (beCopyCurve.ElementList == null || beCopyCurve.ElementList.Items.Count <= 0)
            {
                return true;
            }
            else
            {
                ElementList ElementList = Default.GetElementList();
                returnCurve.ElementList = ElementList;
                foreach (var element in beCopyCurve.ElementList.Items)
                {
                    CurveElement newElement = CurveElement.New.Init(element.Caption, element.IsDisplay, element.Formula, element.AtomicNumber, element.LayerNumber, element.LayerNumBackUp,
                        element.AnalyteLine, element.PeakLow, element.PeakHigh, element.BaseLow, element.BaseHigh, element.PeakDivBase, element.LayerDensity, element.Intensity, element.Content,
                        element.Thickness, element.IntensityWay, element.IntensityComparison, element.ComparisonCoefficient, element.BPeakLow, element.BPeakHigh, element.CalculationWay,
                        element.FpCalculationWay, element.Flag, element.LayerFlag, element.ContentUnit, element.ThicknessUnit, element.SReferenceElements, element.SSpectrumData,
                        element.SInfluenceElements, element.SInfluenceCoefficients, element.Asrat, element.Msthk, element.Loi, element.Limit, element.K1, element.K0, element.Error, element.ErrorK1,
                        element.ErrorK0, element.RowsIndex, element.ElementSpecName, element.DifferenceHelp, element.Color, element.ColorHelper, element.ConditionID, element.DistrubThreshold, element.IsOxide, element.IsShowElement, element.IsShowContent, element.IsAlert, element.Contentcoeff, element.ContentRealValue, element.ElementEncoderSpecName, element.ElementSpecNameNoFilter, element.SSpectrumDataNotFilter, element.IsShowDefineName, element.DefineElemName);
                    if (returnCurve.Condition.Name == beCopyCurve.Condition.Name)
                    {
                        newElement.DevParamId = element.DevParamId;
                    }
                    else
                    {
                        newElement.DevParamId = returnCurve.Condition.DeviceParamList[0].Id;
                    }
                    foreach(var refs in element.ElementRefs)
                    {
                        ElementRef ElementRef = ElementRef.New.Init(refs.Name, refs.IsRef, refs.RefConf);
                        newElement.ElementRefs.Add(ElementRef);
                    }
                    ElementList.Items.Add(newElement);
                }
                return true;
            }
        }

        /// <summary>
        /// 删除曲线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dgvwCurveList.SelectedRows.Count > 0)
            {
                DialogResult dr = SkyrayMsgBox.Show(Info.DeleteCurve, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.OK)
                {
                    int index = dgvwCurveList.SelectedRows[0].Index;
                    WorkCurve workCurve = lstWorkCurve[index];
                    if (WorkCurveHelper.WorkCurveCurrent != null && workCurve.Id == WorkCurveHelper.WorkCurveCurrent.Id)
                    {
                        SkyrayMsgBox.Show(Info.WorkCurveBeUsed);
                        return;
                    }

                    ////////删除
                    if (workCurve.ElementList != null)
                    {
                        foreach (var item in workCurve.ElementList.Items)
                        {
                            StandSample.DeleteAll(ss => ss.Element.Id == item.Id);//标准样品
                            Optimiztion.DeleteAll(o => o.element.Id == item.Id);//优化列表
                            ElementRef.DeleteAll(er => er.Element.Id == item.Id);//影响元素
                            item.Delete();//元素
                        }
                        CustomField.DeleteAll(cf => cf.ElementList.Id == workCurve.ElementList.Id);//自定义项

                        workCurve.ElementList.Delete();//原素列表
                    }
                    if (workCurve.CalibrationParam != null)
                    {
                        workCurve.CalibrationParam.Delete();//校正参数
                    }

                    Lephone.Data.DbEntry.Context.ExecuteNonQuery("update workcurve set SimilarCurveId=0,SimilarCurveName='' where  SimilarCurveId=" + workCurve.Id);
                    //删除lstWorkCurve对象中，当前待删除对象iD在lstWorkCurve中的相似曲线值
                    if (lstWorkCurve.Exists(a => a.SimilarCurveId == workCurve.Id))
                    {
                        lstWorkCurve.Find(a => a.SimilarCurveId == workCurve.Id).SimilarCurveName = "";
                        lstWorkCurve.Find(a => a.SimilarCurveId == workCurve.Id).SimilarCurveId = 0;
                    }
                    if (ReportTemplateHelper.ExistSpecifiedNode("HistoryItem_" + WorkCurveHelper.DeviceCurrent.Id + "_" + workCurve.Id, null))
                    {
                        ReportTemplateHelper.DeleteHistoryItemSpecifiedValue("HistoryItem_" + WorkCurveHelper.DeviceCurrent.Id + "_" + workCurve.Id);
                    }
                    Condition con = workCurve.Condition;
                    workCurve.Delete();//删除数据
                    con.WorkCurves.Remove(workCurve);
                    lstWorkCurve.Remove(workCurve);//删除缓存
                    ////////删除

                    //if (DifferenceDevice.IsRohs)
                    //{
                    //    EDXRFHelper.ReloadCurve();
                    //}
                    WorkCurveToGrid();//加载曲线列表
                    if (lstWorkCurve.Count > index)
                    {
                        dgvwCurveList.Rows[index].Selected = true;
                    }
                    else if (lstWorkCurve.Count > 0)
                    {
                        dgvwCurveList.Rows[index - 1].Selected = true;
                    }
                }
            }
            else
            {
                SkyrayMsgBox.Show(Info.NoSelect);
            }
        }

        /// <summary>
        /// 打开曲线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenCurve_Click(object sender, EventArgs e)
        {
            
            for (int i = 0; i < lstWorkCurve.Count; i++)
            {
                lstWorkCurve[i].Save();
            }


            if (dgvwCurveList.SelectedRows.Count > 0)
            {
               
                SelectCurve();//选中曲线

                switch (WorkCurveHelper.WorkCurveCurrent.Name)
                {
                    case "Ni-Cu-Ni-FeNdB":
                        FpWorkCurve.thickMode = ThickMode.NiNi;
                        break;
                    case "NiP-Fe":
                        FpWorkCurve.thickMode = ThickMode.NiP;
                        break;
                    case "Coating":
                        FpWorkCurve.thickMode = ThickMode.Plating;
                        break;                
                    case "Ni-Cu-Ni-Mag":
                        FpWorkCurve.thickMode = ThickMode.NiCuNi;
                        break;
                    case "Ni-Cu-Ni-Fe":
                    //case "Ag-Ni-Cu-Ni-Fe":
                    //case "Sn-Ni-Cu-Ni-Fe":
                    //case "Au-Ni-Cu-Ni-Fe":
                        FpWorkCurve.thickMode = ThickMode.NiCuNiFe;
                        break;
                    case "Ag-Ni-Cu-Ni-Fe":
                    case "Sn-Ni-Cu-Ni-Fe":
                    case "Au-Ni-Cu-Ni-Fe":
                        FpWorkCurve.thickMode = ThickMode.NiCuNiFe2;
                        break;
                    default:
                        FpWorkCurve.thickMode = ThickMode.Normal;
                        break;
                }

                if (WorkCurveHelper.WorkCurveCurrent.IsNiP2 && WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList.Count > 1)
                {
                    FpWorkCurve.thickMode = ThickMode.NiP2;
                }
            }
            else
            {
                SkyrayMsgBox.Show(Info.NoSelect);
                return;
            }
            //WorkCurveHelper.WorkCurveCurrent.Serialize();
            if (this.ParentForm != null)
                this.ParentForm.DialogResult = this.DialogResult=this.dialogResult = DialogResult.OK;
            EDXRFHelper.GotoMainPage(this);//到达主界面
        }

        /// <summary>
        /// 选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvwCurveList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0 || e.ColumnIndex == ColMainElement.Index)
            {
                return;
            }

            if (dgvwCurveList.Columns[e.ColumnIndex].Name.Equals("ColSimilarCurve")) return;
            for (int i = 0; i < lstWorkCurve.Count; i++)
            {
                lstWorkCurve[i].Save();
            }
            SelectCurve();//选中曲线
            if (this.ParentForm != null)
                this.ParentForm.DialogResult = this.DialogResult=this.dialogResult = DialogResult.OK;
            EDXRFHelper.GotoMainPage(this);//到达主界面
        }

        #endregion

        private void cboWorkArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstWorkCurve != null && cboWorkArea.Text != "")
            {
                CurrentWorkRegion = lstWorkRegion.Find(w => w.Name == cboWorkArea.Text);
                var deviceId = WorkCurveHelper.DeviceCurrent.Id;
                string sql = "select * from WorkCurve where Condition_Id in (select Id from condition where Device_id = "
                    + deviceId + ") and FuncType=" + (int)WorkCurveHelper.DeviceFunctype + " and WorkRegion_Id=" + CurrentWorkRegion.Id;
                lstWorkCurve = WorkCurve.FindBySql(sql);
                WorkCurveToGrid();
            }

            
        }

        private void dgvwCurveList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0||(e.ColumnIndex==ColIsDefaultWorkCurve.Index&&ColIsDefaultWorkCurve.ReadOnly))
                return;
            if (dgvwCurveList.Columns[e.ColumnIndex].Name.Equals("ColIsDefaultWorkCurve"))//默认
            {
                if (dgvwCurveList[e.ColumnIndex, e.RowIndex].Value.Equals(true))
                {
                    lstWorkCurve[e.RowIndex].IsDefaultWorkCurve = false;
                    lstWorkCurve[e.RowIndex].Save(); 
                }
                else
                {
                    for (int i = 0; i < dgvwCurveList.Rows.Count; i++)
                    {
                        if (i != e.RowIndex)
                        {
                            dgvwCurveList["ColIsDefaultWorkCurve", i].Value = false;
                        }
                        else
                        {
                            dgvwCurveList["ColIsDefaultWorkCurve", i].Value = true;
                        }
                        lstWorkCurve[i].Save();
                    }
                }


                //if (dgvwCurveList[e.ColumnIndex, e.RowIndex].Value.Equals(false))
                //{
                //    dgvwCurveList[e.ColumnIndex, e.RowIndex].Value = true;
                //    for (int i = 0; i < dgvwCurveList.Rows.Count; i++)
                //    {
                //        if (i != e.RowIndex)
                //        {
                //            dgvwCurveList["ColIsDefaultWorkCurve", i].Value = false;
                //        }
                //        lstWorkCurve[i].Save();
                //    }
                //}
                //else
                //{
                //    dgvwCurveList[e.ColumnIndex, e.RowIndex].Value = true;
                //}
            }
            
        }

        private bool flag = false;
        public override void ExcuteEndProcess(params object[] objs)
        {
            EDXRFHelper.DisplayWorkCurveControls();
            if (DifferenceDevice.IsRohs && cboWorkArea.Text != "")
            {
                WorkCurveHelper.CurrentWorkRegion = lstWorkRegion.Find(w => w.Name == cboWorkArea.Text);
            }
            DifferenceDevice.MediumAccess.OpenCurveSubmit();
            if (flag)
            {
                ToolStripControls test = MenuLoadHelper.MenuStripCollection.Find(w => w.CurrentNaviItem.Name == "EditElement");
                EDXRFHelper.RecurveNextUC(test);
            }
            DifferenceDevice.MediumAccess.RefreshHistory();
            
        }

        #region
        private void btwExportCurve_Click(object sender, EventArgs e)
        {
            if (dgvwCurveList.SelectedRows == null || dgvwCurveList.SelectedRows.Count ==0)
                return;
            WorkCurve tempCurve = lstWorkCurve[dgvwCurveList.SelectedRows[0].Index];
            if (DialogResult.OK == this.saveFileDialog.ShowDialog())
            {
                string fileName = this.saveFileDialog.FileName;
                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration newDec = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(newDec);
                XmlElement CurveInfo = xmlDoc.CreateElement("CurveInfo");
                xmlDoc.AppendChild(CurveInfo);

                XmlElement newWorkCurve = xmlDoc.CreateElement("WorkCurve");
                CurveInfo.AppendChild(newWorkCurve);
                XmlElement childWorkCurve = xmlDoc.CreateElement("Name");
                childWorkCurve.InnerText = tempCurve.Name;
                newWorkCurve.AppendChild(childWorkCurve);

                #region 曲线及条件
                childWorkCurve = xmlDoc.CreateElement("CalcType");
                childWorkCurve.InnerText = tempCurve.CalcType.ToString();
                newWorkCurve.AppendChild(childWorkCurve);

                childWorkCurve = xmlDoc.CreateElement("FuncType");
                childWorkCurve.InnerText = tempCurve.FuncType.ToString();
                newWorkCurve.AppendChild(childWorkCurve);

                childWorkCurve = xmlDoc.CreateElement("IsAbsorb");
                childWorkCurve.InnerText = tempCurve.IsAbsorb.ToString();
                newWorkCurve.AppendChild(childWorkCurve);

                //childWorkCurve = xmlDoc.CreateElement("IsThickShowContent");
                //childWorkCurve.InnerText = tempCurve.IsThickShowContent.ToString();
                //newWorkCurve.AppendChild(childWorkCurve);

                childWorkCurve = xmlDoc.CreateElement("LimitThickness");
                childWorkCurve.InnerText = tempCurve.LimitThickness.ToString();
                newWorkCurve.AppendChild(childWorkCurve);

                childWorkCurve = xmlDoc.CreateElement("RemoveBackGround");
                childWorkCurve.InnerText = tempCurve.RemoveBackGround.ToString();
                newWorkCurve.AppendChild(childWorkCurve);

                childWorkCurve = xmlDoc.CreateElement("RemoveSum");
                childWorkCurve.InnerText = tempCurve.RemoveSum.ToString();
                newWorkCurve.AppendChild(childWorkCurve);

                childWorkCurve = xmlDoc.CreateElement("RemoveEscape");
                childWorkCurve.InnerText = tempCurve.RemoveEscape.ToString();
                newWorkCurve.AppendChild(childWorkCurve);

                childWorkCurve = xmlDoc.CreateElement("IsDefaultWorkCurve");
                childWorkCurve.InnerText = tempCurve.IsDefaultWorkCurve.ToString();
                newWorkCurve.AppendChild(childWorkCurve);

                childWorkCurve = xmlDoc.CreateElement("IsJoinMatch");
                childWorkCurve.InnerText = tempCurve.IsJoinMatch.ToString();
                newWorkCurve.AppendChild(childWorkCurve);

                childWorkCurve = xmlDoc.CreateElement("IsNiP2");
                childWorkCurve.InnerText = tempCurve.IsNiP2.ToString();
                newWorkCurve.AppendChild(childWorkCurve);

                childWorkCurve = xmlDoc.CreateElement("IsBaseAdjust");
                childWorkCurve.InnerText = tempCurve.IsBaseAdjust.ToString();
                newWorkCurve.AppendChild(childWorkCurve);

                XmlElement newCondition = xmlDoc.CreateElement("Condition");
                CurveInfo.AppendChild(newCondition);

                childWorkCurve = xmlDoc.CreateElement("Name");
                childWorkCurve.InnerText = tempCurve.Condition.Name;
                newCondition.AppendChild(childWorkCurve);

                childWorkCurve = xmlDoc.CreateElement("Type");
                childWorkCurve.InnerText = tempCurve.Condition.Type.ToString();
                newCondition.AppendChild(childWorkCurve);

                childWorkCurve = xmlDoc.CreateElement("TubVoltage");
                childWorkCurve.InnerText = tempCurve.Condition.InitParam.TubVoltage.ToString();
                newCondition.AppendChild(childWorkCurve);

                childWorkCurve = xmlDoc.CreateElement("TubCurrent");
                childWorkCurve.InnerText = tempCurve.Condition.InitParam.TubCurrent.ToString();
                newCondition.AppendChild(childWorkCurve);

                childWorkCurve = xmlDoc.CreateElement("ElemName");
                childWorkCurve.InnerText = tempCurve.Condition.InitParam.ElemName.ToString();
                newCondition.AppendChild(childWorkCurve);

                childWorkCurve = xmlDoc.CreateElement("Gain");
                childWorkCurve.InnerText = tempCurve.Condition.InitParam.Gain.ToString();
                newCondition.AppendChild(childWorkCurve);

                childWorkCurve = xmlDoc.CreateElement("FineGain");
                childWorkCurve.InnerText = tempCurve.Condition.InitParam.FineGain.ToString();
                newCondition.AppendChild(childWorkCurve);

                childWorkCurve = xmlDoc.CreateElement("Channel");
                childWorkCurve.InnerText = tempCurve.Condition.InitParam.Channel.ToString();
                newCondition.AppendChild(childWorkCurve);

                childWorkCurve = xmlDoc.CreateElement("Filter");
                childWorkCurve.InnerText = tempCurve.Condition.InitParam.Filter.ToString();
                newCondition.AppendChild(childWorkCurve);

                childWorkCurve = xmlDoc.CreateElement("Collimator");
                childWorkCurve.InnerText = tempCurve.Condition.InitParam.Collimator.ToString();
                newCondition.AppendChild(childWorkCurve);

                childWorkCurve = xmlDoc.CreateElement("ChannelError");
                childWorkCurve.InnerText = tempCurve.Condition.InitParam.ChannelError.ToString();
                newCondition.AppendChild(childWorkCurve);

                childWorkCurve = xmlDoc.CreateElement("CurrentRate");
                childWorkCurve.InnerText = tempCurve.Condition.InitParam.CurrentRate.ToString();
                newCondition.AppendChild(childWorkCurve);

                childWorkCurve = xmlDoc.CreateElement("Target");
                childWorkCurve.InnerText = tempCurve.Condition.InitParam.Target.ToString();
                newCondition.AppendChild(childWorkCurve);

                childWorkCurve = xmlDoc.CreateElement("TargetMode");
                childWorkCurve.InnerText = tempCurve.Condition.InitParam.TargetMode.ToString();
                newCondition.AppendChild(childWorkCurve);

                #endregion
                XmlElement newDeviceParamList = xmlDoc.CreateElement("DeviceParamList");
                CurveInfo.AppendChild(newDeviceParamList);
                if (tempCurve.Condition.DeviceParamList.Count > 0)
                {
                    foreach (DeviceParameter deviceParms in tempCurve.Condition.DeviceParamList)
                    {
                        #region 条件之测量条件
                        childWorkCurve = xmlDoc.CreateElement("DeviceParameter");
                        newDeviceParamList.AppendChild(childWorkCurve);

                        XmlElement accentElement = xmlDoc.CreateElement("Name");
                        accentElement.InnerText = deviceParms.Name;
                        childWorkCurve.AppendChild(accentElement);

                        accentElement = xmlDoc.CreateElement("PrecTime");
                        accentElement.InnerText = deviceParms.PrecTime.ToString();
                        childWorkCurve.AppendChild(accentElement);

                        accentElement = xmlDoc.CreateElement("TubVoltage");
                        accentElement.InnerText = deviceParms.TubVoltage.ToString();
                        childWorkCurve.AppendChild(accentElement);


                        accentElement = xmlDoc.CreateElement("TubCurrent");
                        accentElement.InnerText = deviceParms.TubCurrent.ToString();
                        childWorkCurve.AppendChild(accentElement);

                        accentElement = xmlDoc.CreateElement("Filter");
                        accentElement.InnerText = deviceParms.FilterIdx.ToString();
                        childWorkCurve.AppendChild(accentElement);

                        accentElement = xmlDoc.CreateElement("Collimator");
                        accentElement.InnerText = deviceParms.CollimatorIdx.ToString();
                        childWorkCurve.AppendChild(accentElement);

                        accentElement = xmlDoc.CreateElement("IsVacuum");
                        accentElement.InnerText = deviceParms.IsVacuum.ToString();
                        childWorkCurve.AppendChild(accentElement);

                        accentElement = xmlDoc.CreateElement("VacuumTime");
                        accentElement.InnerText = deviceParms.VacuumTime.ToString();
                        childWorkCurve.AppendChild(accentElement);

                        accentElement = xmlDoc.CreateElement("IsVacuumDegree");
                        accentElement.InnerText = deviceParms.IsVacuumDegree.ToString();
                        childWorkCurve.AppendChild(accentElement);

                        accentElement = xmlDoc.CreateElement("VacuumDegree");
                        accentElement.InnerText = deviceParms.VacuumDegree.ToString();
                        childWorkCurve.AppendChild(accentElement);

                        accentElement = xmlDoc.CreateElement("IsAdjustRate");
                        accentElement.InnerText = deviceParms.IsAdjustRate.ToString();
                        childWorkCurve.AppendChild(accentElement);

                        accentElement = xmlDoc.CreateElement("MinRate");
                        accentElement.InnerText = deviceParms.MinRate.ToString();
                        childWorkCurve.AppendChild(accentElement);

                        accentElement = xmlDoc.CreateElement("MaxRate");
                        accentElement.InnerText = deviceParms.MaxRate.ToString();
                        childWorkCurve.AppendChild(accentElement);

                        accentElement = xmlDoc.CreateElement("BeginChann");
                        accentElement.InnerText = deviceParms.BeginChann.ToString();
                        childWorkCurve.AppendChild(accentElement);

                        accentElement = xmlDoc.CreateElement("EndChann");
                        accentElement.InnerText = deviceParms.EndChann.ToString();
                        childWorkCurve.AppendChild(accentElement);

                        accentElement = xmlDoc.CreateElement("IsDistrubAlert");
                        accentElement.InnerText = deviceParms.IsDistrubAlert.ToString();
                        childWorkCurve.AppendChild(accentElement);

                        accentElement = xmlDoc.CreateElement("IsPeakFloat");
                        accentElement.InnerText = deviceParms.IsPeakFloat.ToString();
                        childWorkCurve.AppendChild(accentElement);

                        accentElement = xmlDoc.CreateElement("PeakFloatLeft");
                        accentElement.InnerText = deviceParms.PeakFloatLeft.ToString();
                        childWorkCurve.AppendChild(accentElement);

                        accentElement = xmlDoc.CreateElement("PeakFloatRight");
                        accentElement.InnerText = deviceParms.PeakFloatRight.ToString();
                        childWorkCurve.AppendChild(accentElement);

                        accentElement = xmlDoc.CreateElement("PeakFloatChannel");
                        accentElement.InnerText = deviceParms.PeakFloatChannel.ToString();
                        childWorkCurve.AppendChild(accentElement);

                        accentElement = xmlDoc.CreateElement("PeakCheckTime");
                        accentElement.InnerText = deviceParms.PeakCheckTime.ToString();
                        childWorkCurve.AppendChild(accentElement);

                        accentElement = xmlDoc.CreateElement("PeakFloatError");
                        accentElement.InnerText = deviceParms.PeakFloatError.ToString();
                        childWorkCurve.AppendChild(accentElement);

                        accentElement = xmlDoc.CreateElement("CurrentRate");
                        accentElement.InnerText = deviceParms.CurrentRate.ToString();
                        childWorkCurve.AppendChild(accentElement);

                        accentElement = xmlDoc.CreateElement("TargetIdx");
                        accentElement.InnerText = deviceParms.TargetIdx.ToString();
                        childWorkCurve.AppendChild(accentElement);

                        accentElement = xmlDoc.CreateElement("TargetMode");
                        accentElement.InnerText = deviceParms.TargetMode.ToString();
                        childWorkCurve.AppendChild(accentElement);
                        #endregion
                    }
                }

                XmlElement newDemarcateEnergy = xmlDoc.CreateElement("DemarcateEnergyList");
                CurveInfo.AppendChild(newDemarcateEnergy);
                if (tempCurve.Condition.DemarcateEnergys.Count > 0)
                {
                    foreach (DemarcateEnergy emergyDe in tempCurve.Condition.DemarcateEnergys)
                    {
                        childWorkCurve = xmlDoc.CreateElement("DemarcateEnergy");
                        newDemarcateEnergy.AppendChild(childWorkCurve);

                        XmlElement accentElement = xmlDoc.CreateElement("ElementName");
                        accentElement.InnerText = emergyDe.ElementName;
                        childWorkCurve.AppendChild(accentElement);

                        accentElement = xmlDoc.CreateElement("Line");
                        accentElement.InnerText = emergyDe.Line.ToString();
                        childWorkCurve.AppendChild(accentElement);

                        accentElement = xmlDoc.CreateElement("Energy");
                        accentElement.InnerText = emergyDe.Energy.ToString();
                        childWorkCurve.AppendChild(accentElement);

                        accentElement = xmlDoc.CreateElement("Channel");
                        accentElement.InnerText = emergyDe.Channel.ToString();
                        childWorkCurve.AppendChild(accentElement);
                    }
                }


                if (tempCurve.WorkRegion != null)
                {
                    XmlElement newWorkRegion = xmlDoc.CreateElement("WorkRegion");
                    CurveInfo.AppendChild(newWorkRegion);

                    childWorkCurve = xmlDoc.CreateElement("Name");
                    childWorkCurve.InnerText = tempCurve.WorkRegion.Name.ToString();
                    newWorkRegion.AppendChild(childWorkCurve);

                    childWorkCurve = xmlDoc.CreateElement("RohsSampleType");
                    childWorkCurve.InnerText = tempCurve.WorkRegion.RohsSampleType.ToString();
                    newWorkRegion.AppendChild(childWorkCurve);

                    childWorkCurve = xmlDoc.CreateElement("IsDefaultWorkRegion");
                    childWorkCurve.InnerText = tempCurve.WorkRegion.IsDefaultWorkRegion.ToString();
                    newWorkRegion.AppendChild(childWorkCurve);
                }
                if (tempCurve.ElementList != null)
                {
                    #region 感兴趣元素
                    newWorkCurve = xmlDoc.CreateElement("ElementList");
                    CurveInfo.AppendChild(newWorkCurve);

                    childWorkCurve = xmlDoc.CreateElement("IsUnitary");
                    childWorkCurve.InnerText = tempCurve.ElementList.IsUnitary.ToString();
                    newWorkCurve.AppendChild(childWorkCurve);

                    childWorkCurve = xmlDoc.CreateElement("UnitaryValue");
                    childWorkCurve.InnerText = tempCurve.ElementList.UnitaryValue.ToString();
                    newWorkCurve.AppendChild(childWorkCurve);

                    childWorkCurve = xmlDoc.CreateElement("TubeWindowThickness");
                    childWorkCurve.InnerText = tempCurve.ElementList.TubeWindowThickness.ToString();
                    newWorkCurve.AppendChild(childWorkCurve);

                    childWorkCurve = xmlDoc.CreateElement("RhIsLayer");
                    childWorkCurve.InnerText = tempCurve.ElementList.RhIsLayer.ToString();
                    newWorkCurve.AppendChild(childWorkCurve);

                    childWorkCurve = xmlDoc.CreateElement("RhLayerFactor");
                    childWorkCurve.InnerText = tempCurve.ElementList.RhLayerFactor.ToString();
                    newWorkCurve.AppendChild(childWorkCurve);

                    childWorkCurve = xmlDoc.CreateElement("LayerElemsInAnalyzer");
                    childWorkCurve.InnerText = tempCurve.ElementList.LayerElemsInAnalyzer;
                    newWorkCurve.AppendChild(childWorkCurve);

                    childWorkCurve = xmlDoc.CreateElement("IsAbsorption");
                    childWorkCurve.InnerText = tempCurve.ElementList.IsAbsorption.ToString();
                    newWorkCurve.AppendChild(childWorkCurve);


                    childWorkCurve = xmlDoc.CreateElement("ThCalculationWay");
                    childWorkCurve.InnerText = tempCurve.ElementList.ThCalculationWay.ToString();
                    newWorkCurve.AppendChild(childWorkCurve);

                    childWorkCurve = xmlDoc.CreateElement("DBlLimt");
                    childWorkCurve.InnerText = tempCurve.ElementList.DBlLimt.ToString();
                    newWorkCurve.AppendChild(childWorkCurve);

                    childWorkCurve = xmlDoc.CreateElement("IsRemoveBk");
                    childWorkCurve.InnerText = tempCurve.ElementList.IsRemoveBk.ToString();
                    newWorkCurve.AppendChild(childWorkCurve);

                    childWorkCurve = xmlDoc.CreateElement("SpecListId");
                    childWorkCurve.InnerText = (tempCurve.ElementList.SpecListName!=null)?tempCurve.ElementList.SpecListName.ToString():"";
                    newWorkCurve.AppendChild(childWorkCurve);

                    childWorkCurve = xmlDoc.CreateElement("IsReportCategory");
                    childWorkCurve.InnerText = tempCurve.ElementList.IsReportCategory.ToString();
                    newWorkCurve.AppendChild(childWorkCurve);

                    childWorkCurve = xmlDoc.CreateElement("PureAsInfinite");
                    childWorkCurve.InnerText = tempCurve.ElementList.PureAsInfinite.ToString();
                    newWorkCurve.AppendChild(childWorkCurve);

                    childWorkCurve = xmlDoc.CreateElement("MatchSpecListIdStr");
                    childWorkCurve.InnerText = tempCurve.ElementList.MatchSpecListIdStr.ToString();
                    newWorkCurve.AppendChild(childWorkCurve);

                    //@CYR180502
                    childWorkCurve = xmlDoc.CreateElement("MainElementToCalcKarat");
                    childWorkCurve.InnerText = tempCurve.ElementList.MainElementToCalcKarat.ToString();
                    newWorkCurve.AppendChild(childWorkCurve);

                    #endregion

                    if (tempCurve.ElementList.Items != null && tempCurve.ElementList.Items.Count > 0)
                    {
                        foreach (CurveElement element in tempCurve.ElementList.Items)
                        {
                            #region 感兴趣元素信息
                            childWorkCurve = xmlDoc.CreateElement("CurveElement");
                            newWorkCurve.AppendChild(childWorkCurve);

                            XmlElement docElement = xmlDoc.CreateElement("Caption");
                            docElement.InnerText = element.Caption;
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("RowIndex");
                            docElement.InnerText = element.RowsIndex.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("IsDisplay");
                            docElement.InnerText = element.IsDisplay.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("Formula");
                            docElement.InnerText = element.Formula;
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("AtomicNumber");
                            docElement.InnerText = element.AtomicNumber.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("DeviceParamsName");
                            if (tempCurve.Condition.DeviceParamList.First(w => w.Id == element.DevParamId)==null)
                            {
                                Msg.Show("Failed to Saved！" );
                            }
                            docElement.InnerText = tempCurve.Condition.DeviceParamList.First(w=>w.Id == element.DevParamId).Name;
                            childWorkCurve.AppendChild(docElement);
                          
                            docElement = xmlDoc.CreateElement("LayerNumber");
                            docElement.InnerText = element.LayerNumber.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("LayerNumBackUp");
                            docElement.InnerText = element.LayerNumBackUp;
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("AnalyteLine");
                            docElement.InnerText = element.AnalyteLine.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("PeakLow");
                            docElement.InnerText = element.PeakLow.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("PeakHigh");
                            docElement.InnerText = element.PeakHigh.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("BaseLow");
                            docElement.InnerText = element.BaseLow.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("BaseHigh");
                            docElement.InnerText = element.BaseHigh.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("PeakDivBase");
                            docElement.InnerText = element.PeakDivBase.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("LayerDensity");
                            docElement.InnerText = element.LayerDensity.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("IntensityWay");
                            docElement.InnerText = element.IntensityWay.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("IntensityComparison");
                            docElement.InnerText = element.IntensityComparison.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("ComparisonCoefficient");
                            docElement.InnerText = element.ComparisonCoefficient.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("BPeakLow");
                            docElement.InnerText = element.BPeakLow.ToString();
                            childWorkCurve.AppendChild(docElement);


                            docElement = xmlDoc.CreateElement("BPeakHigh");
                            docElement.InnerText = element.BPeakHigh.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("CalculationWay");
                            docElement.InnerText = element.CalculationWay.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("FpCalculationWay");
                            docElement.InnerText = element.FpCalculationWay.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("Flag");
                            docElement.InnerText = element.Flag.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("LayerFlag");
                            docElement.InnerText = element.LayerFlag.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("ContentUnit");
                            docElement.InnerText = element.ContentUnit.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("ThicknessUnit");
                            docElement.InnerText = element.ThicknessUnit.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("SReferenceElements");
                            docElement.InnerText = element.SReferenceElements.ToString();
                            childWorkCurve.AppendChild(docElement);

                            //docElement = xmlDoc.CreateElement("ThicknessUnit");
                            //docElement.InnerText = element.ThicknessUnit.ToString();
                            //childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("SSpectrumData");
                            docElement.InnerText = element.SSpectrumData.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("SInfluenceElements");
                            docElement.InnerText = element.SInfluenceElements.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("DistrubThreshold");
                            docElement.InnerText = element.DistrubThreshold.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("IsInfluence");
                            docElement.InnerText = element.IsInfluence.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("SInfluenceCoefficients");
                            docElement.InnerText = element.SInfluenceCoefficients.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("ElementSpecName");
                            docElement.InnerText = element.ElementSpecName.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("Color");
                            docElement.InnerText = element.Color.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("ColorHelper");
                            docElement.InnerText = element.ColorHelper;
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("ContentCoeff");
                            docElement.InnerText = element.Contentcoeff.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("IsOxide");
                            docElement.InnerText = element.IsOxide.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("ElementEncoderSpecName");
                            docElement.InnerText = element.ElementEncoderSpecName.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("IsBorderlineElem");
                            docElement.InnerText = element.IsBorderlineElem.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("ElementSpecNameNoFilter");
                            docElement.InnerText = element.ElementSpecNameNoFilter.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("SSpectrumDataNotFilter");
                            docElement.InnerText = element.SSpectrumDataNotFilter.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("IsShowDefineName");
                            docElement.InnerText = element.IsShowDefineName.ToString();
                            childWorkCurve.AppendChild(docElement);

                            docElement = xmlDoc.CreateElement("DefineElemName");
                            docElement.InnerText = element.DefineElemName.ToString();
                            childWorkCurve.AppendChild(docElement);
                            
                            #endregion
                            if (element.Samples != null && element.Samples.Count > 0)
                            {
                                #region 元素对应的标样
                                foreach (StandSample samples in element.Samples)
                                {
                                    docElement = xmlDoc.CreateElement("StandSample");
                                    childWorkCurve.AppendChild(docElement);

                                    XmlElement xmlStandSample = xmlDoc.CreateElement("SampleName");
                                    xmlStandSample.InnerText = samples.SampleName;
                                    docElement.AppendChild(xmlStandSample);

                                    xmlStandSample = xmlDoc.CreateElement("Height");
                                    xmlStandSample.InnerText = samples.Height.ToString();
                                    docElement.AppendChild(xmlStandSample);

                                    xmlStandSample = xmlDoc.CreateElement("CalcAngleHeight");
                                    xmlStandSample.InnerText = samples.CalcAngleHeight.ToString();
                                    docElement.AppendChild(xmlStandSample);

                                    xmlStandSample = xmlDoc.CreateElement("X");
                                    xmlStandSample.InnerText = samples.X.ToString();
                                    docElement.AppendChild(xmlStandSample);

                                    xmlStandSample = xmlDoc.CreateElement("Y");
                                    xmlStandSample.InnerText = samples.Y.ToString();
                                    docElement.AppendChild(xmlStandSample);

                                    xmlStandSample = xmlDoc.CreateElement("Z");
                                    xmlStandSample.InnerText = samples.Z.ToString();
                                    docElement.AppendChild(xmlStandSample);

                                    xmlStandSample = xmlDoc.CreateElement("TheoryX");
                                    xmlStandSample.InnerText = samples.TheoryX.ToString();
                                    docElement.AppendChild(xmlStandSample);

                                    xmlStandSample = xmlDoc.CreateElement("Level");
                                    xmlStandSample.InnerText = samples.Level.ToString();
                                    docElement.AppendChild(xmlStandSample);

                                    xmlStandSample = xmlDoc.CreateElement("Active");
                                    xmlStandSample.InnerText = samples.Active.ToString();
                                    docElement.AppendChild(xmlStandSample);

                                    xmlStandSample = xmlDoc.CreateElement("ElementName");
                                    xmlStandSample.InnerText = samples.ElementName.ToString();
                                    docElement.AppendChild(xmlStandSample);

                                    //xmlStandSample = xmlDoc.CreateElement("SpecListId");
                                    //xmlStandSample.InnerText = samples.SpecListId.ToString();
                                    //docElement.AppendChild(xmlStandSample);

                                    xmlStandSample = xmlDoc.CreateElement("Layer");
                                    xmlStandSample.InnerText = samples.Layer.ToString();
                                    docElement.AppendChild(xmlStandSample);

                                    xmlStandSample = xmlDoc.CreateElement("TotalLayer");
                                    xmlStandSample.InnerText = samples.TotalLayer.ToString();
                                    docElement.AppendChild(xmlStandSample);

                                    xmlStandSample = xmlDoc.CreateElement("IsMatch");
                                    xmlStandSample.InnerText = samples.IsMatch.ToString();
                                    docElement.AppendChild(xmlStandSample);

                                    //xmlStandSample = xmlDoc.CreateElement("MatchSpecListId");
                                    //xmlStandSample.InnerText = samples.MatchSpecListId.ToString();
                                    //docElement.AppendChild(xmlStandSample);

                                    xmlStandSample = xmlDoc.CreateElement("MatchSpecName");
                                    xmlStandSample.InnerText = samples.MatchSpecName == null ? "" : samples.MatchSpecName.ToString();
                                    docElement.AppendChild(xmlStandSample);

                                    //追加密度
                                    xmlStandSample = xmlDoc.CreateElement("Density");
                                    xmlStandSample.InnerText = samples.Density == null ? "" : samples.Density.ToString();
                                    docElement.AppendChild(xmlStandSample);
                                    //追加不确定度
                                    xmlStandSample = xmlDoc.CreateElement("Uncertainty");
                                    xmlStandSample.InnerText = samples.Uncertainty == null ? "" : samples.Uncertainty;
                                    docElement.AppendChild(xmlStandSample);
                                }
                                #endregion
                            }

                            if (element.References != null && element.References.Count > 0)
                            {
                                #region 元素对应的拟合元素
                                foreach (ReferenceElement refenceTemp in element.References)
                                {
                                    docElement = xmlDoc.CreateElement("ReferenceElement");
                                    childWorkCurve.AppendChild(docElement);

                                    XmlElement xmlStandSample = xmlDoc.CreateElement("MainElementName");
                                    xmlStandSample.InnerText = refenceTemp.MainElementName;
                                    docElement.AppendChild(xmlStandSample);

                                    xmlStandSample = xmlDoc.CreateElement("ReferenceElementName");
                                    xmlStandSample.InnerText = refenceTemp.ReferenceElementName;
                                    docElement.AppendChild(xmlStandSample);

                                    xmlStandSample = xmlDoc.CreateElement("ReferenceLeftBorder");
                                    xmlStandSample.InnerText = refenceTemp.ReferenceLeftBorder.ToString();
                                    docElement.AppendChild(xmlStandSample);

                                    xmlStandSample = xmlDoc.CreateElement("ReferenceRightBorder");
                                    xmlStandSample.InnerText = refenceTemp.ReferenceRightBorder.ToString();
                                    docElement.AppendChild(xmlStandSample);

                                    xmlStandSample = xmlDoc.CreateElement("ReferenceBackLeft");
                                    xmlStandSample.InnerText = refenceTemp.ReferenceBackLeft.ToString();
                                    docElement.AppendChild(xmlStandSample);

                                    xmlStandSample = xmlDoc.CreateElement("ReferenceBackRight");
                                    xmlStandSample.InnerText = refenceTemp.ReferenceBackRight.ToString();
                                    docElement.AppendChild(xmlStandSample);

                                    xmlStandSample = xmlDoc.CreateElement("PeakToBack");
                                    xmlStandSample.InnerText = refenceTemp.PeakToBack.ToString();
                                    docElement.AppendChild(xmlStandSample);
                                }
                                #endregion
                            }

                            if (element.Optimiztion != null && element.Optimiztion.Count > 0)
                            {
                                #region 元素对应的优化因子
                                foreach (Optimiztion optimode in element.Optimiztion)
                                {
                                    docElement = xmlDoc.CreateElement("Optimiztion");
                                    childWorkCurve.AppendChild(docElement);

                                    XmlElement xmlOptimizetionType = xmlDoc.CreateElement("OptimizetionType");
                                    xmlOptimizetionType.InnerText = optimode.OptimizetionType.ToString();
                                    docElement.AppendChild(xmlOptimizetionType);

                                    XmlElement xmlOptimiztion = xmlDoc.CreateElement("OptimiztionValue");
                                    xmlOptimiztion.InnerText = optimode.OptimiztionValue.ToString();
                                    docElement.AppendChild(xmlOptimiztion);

                                    xmlOptimiztion = xmlDoc.CreateElement("OptimiztionRange");
                                    xmlOptimiztion.InnerText = optimode.OptimiztionRange.ToString();
                                    docElement.AppendChild(xmlOptimiztion);

                                    xmlOptimiztion = xmlDoc.CreateElement("OptimiztionMin");
                                    xmlOptimiztion.InnerText = optimode.OptimiztionMin.ToString();
                                    docElement.AppendChild(xmlOptimiztion);

                                    xmlOptimiztion = xmlDoc.CreateElement("OptimiztionMax");
                                    xmlOptimiztion.InnerText = optimode.OptimiztionMax.ToString();
                                    docElement.AppendChild(xmlOptimiztion);

                                    xmlOptimiztion = xmlDoc.CreateElement("OptimiztionFactor");
                                    xmlOptimiztion.InnerText = optimode.OptimiztionFactor.ToString();
                                    docElement.AppendChild(xmlOptimiztion);

                                    xmlOptimiztion = xmlDoc.CreateElement("OptExpression");
                                    xmlOptimiztion.InnerText = optimode.OptExpression.ToString();
                                    docElement.AppendChild(xmlOptimiztion);

                                    xmlOptimiztion = xmlDoc.CreateElement("IsJoinIntensity");
                                    xmlOptimiztion.InnerText = optimode.IsJoinIntensity.ToString();
                                    docElement.AppendChild(xmlOptimiztion);
                                }
                                #endregion
                            }

                            if (element.ElementRefs != null && element.ElementRefs.Count > 0)
                            {
                                #region 元素对应的影响元素
                                foreach (ElementRef refelement in element.ElementRefs)
                                {
                                    docElement = xmlDoc.CreateElement("ElementRefs");
                                    childWorkCurve.AppendChild(docElement);

                                    XmlElement xmlElementRefs = xmlDoc.CreateElement("Name");
                                    xmlElementRefs.InnerText = refelement.Name.ToString();
                                    docElement.AppendChild(xmlElementRefs);

                                    xmlElementRefs = xmlDoc.CreateElement("IsRef");
                                    xmlElementRefs.InnerText = refelement.IsRef.ToString();
                                    docElement.AppendChild(xmlElementRefs);

                                    xmlElementRefs = xmlDoc.CreateElement("RefConf");
                                    xmlElementRefs.InnerText = refelement.RefConf.ToString();
                                    docElement.AppendChild(xmlElementRefs);

                                    xmlElementRefs = xmlDoc.CreateElement("DistrubThreshold");
                                    xmlElementRefs.InnerText = refelement.DistrubThreshold.ToString();
                                    docElement.AppendChild(xmlElementRefs);
                                }
                                #endregion
                            }
                        }
                    }

                    if (tempCurve.ElementList.CustomFields != null && tempCurve.ElementList.CustomFields.Count > 0)
                    {
                        foreach (CustomField field in tempCurve.ElementList.CustomFields)
                        {
                            # region 元素自定义
                            childWorkCurve = xmlDoc.CreateElement("CustomField");
                            newWorkCurve.AppendChild(childWorkCurve);

                            XmlElement xmlCustomer = xmlDoc.CreateElement("Name");
                            xmlCustomer.InnerText = field.Name;
                            childWorkCurve.AppendChild(xmlCustomer);

                            xmlCustomer = xmlDoc.CreateElement("Expression");
                            xmlCustomer.InnerText = field.Expression;
                            childWorkCurve.AppendChild(xmlCustomer);
                            #endregion
                        }
                    }
                    if (tempCurve.IntensityCalibration != null && tempCurve.IntensityCalibration.Count > 0)
                    {
                        foreach (IntensityCalibration IntenCal in tempCurve.IntensityCalibration)
                        {
                            # region 元素自定义
                            childWorkCurve = xmlDoc.CreateElement("IntensityCalibration");
                            newWorkCurve.AppendChild(childWorkCurve);

                            XmlElement xmlCustomer = xmlDoc.CreateElement("Element");
                            xmlCustomer.InnerText = IntenCal.Element;
                            childWorkCurve.AppendChild(xmlCustomer);

                            xmlCustomer = xmlDoc.CreateElement("OriginalIn");
                            xmlCustomer.InnerText = IntenCal.OriginalIn.ToString();
                            childWorkCurve.AppendChild(xmlCustomer);

                            xmlCustomer = xmlDoc.CreateElement("CalibrateIn");
                            xmlCustomer.InnerText = IntenCal.CalibrateIn.ToString();
                            childWorkCurve.AppendChild(xmlCustomer);

                            xmlCustomer = xmlDoc.CreateElement("PeakLeft");
                            xmlCustomer.InnerText = IntenCal.PeakLeft.ToString();
                            childWorkCurve.AppendChild(xmlCustomer);
                            xmlCustomer = xmlDoc.CreateElement("PeakRight");
                            xmlCustomer.InnerText = IntenCal.PeakRight.ToString();
                            childWorkCurve.AppendChild(xmlCustomer);

                            xmlCustomer = xmlDoc.CreateElement("OriginalBaseIn");
                            xmlCustomer.InnerText = IntenCal.OriginalBaseIn.ToString();
                            childWorkCurve.AppendChild(xmlCustomer);
                            xmlCustomer = xmlDoc.CreateElement("CalibrateBaseIn");
                            xmlCustomer.InnerText = IntenCal.CalibrateBaseIn.ToString();
                            childWorkCurve.AppendChild(xmlCustomer);
                            #endregion
                        }
                    }

                }
                xmlDoc.Save(fileName);
            }
        }

        private void btWImport_Click(object sender, EventArgs e)
        {
            UCCurveImportParams prams = new UCCurveImportParams();
            WorkCurveHelper.OpenUC(prams, false, Info.CurveExport,true);
            if (prams.dialogResult == DialogResult.OK)
            {
                if (DifferenceDevice.IsRohs)
                    cboWorkArea_SelectedIndexChanged(null, null);
                else
                    UCCurve_Load(null, null);
            }
        }

        private void ExportCurve(Type type,XmlElement element,XmlDocument document,object obTemp)
        {
            foreach (PropertyInfo info in type.GetProperties())
            {
                object obj = info.GetValue(obTemp, null);
                if (info.PropertyType.GetInterface("IEnumerable", true) != null 
                    && info.PropertyType != typeof(HasMany<WorkCurve>))
                {
                    IEnumerable IEnum = (IEnumerable)obj;
                    foreach (object tempTT in IEnum)
                    {
                        ExportCurve(tempTT.GetType(), element, document, tempTT);
                    }
                }
                else  if (info.PropertyType.IsClass && info.PropertyType != typeof(Device))
                {
                   XmlElement classElement = document.CreateElement(info.Name);
                   element.AppendChild(classElement);
                   ExportCurve(info.PropertyType, classElement, document, obj);
                }
                else
                {
                    XmlElement atomElement = document.CreateElement(info.Name);
                    atomElement.InnerText = obj.ToString();
                    element.AppendChild(atomElement);
                }
            }
        }

        private void RecurseValue(Type type, XmlDocument xmlDoc, string fileName, object obj)
        {
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo info in properties)
            {
                XmlNode xmlNode = xmlDoc.SelectSingleNode(fileName + "/" + info.Name);
                if (xmlNode != null && xmlNode.ChildNodes != null && xmlNode.ChildNodes.Count == 1)
                {
                    if (info.PropertyType.IsEnum)
                        info.SetValue(obj, Enum.Parse(info.PropertyType,xmlNode.InnerText), null);
                    else
                        info.SetValue(obj, xmlNode.InnerText, null);
                }
                else if (xmlNode != null && xmlNode.ChildNodes != null && xmlNode.ChildNodes.Count > 1)
                {
                    fileName = fileName + "/" + info.Name;
                    object objtt = ObjectInfo.GetInstance(info.PropertyType).Handler.CreateInstance();
                    RecurseValue(info.PropertyType, xmlDoc, fileName, objtt);
                }
            }
        }
        #endregion

        private void btWCurveToElement_Click(object sender, EventArgs e)
        {
            btnOpenCurve_Click(null, null);
            flag = true;
        }

        private void btnReName_Click(object sender, EventArgs e)
        {
            if (dgvwCurveList.SelectedRows == null || dgvwCurveList.SelectedRows.Count ==0)
                return;
            WorkCurve tempCurve = lstWorkCurve[dgvwCurveList.SelectedRows[0].Index];
            FrmCurveReName frmReName = new FrmCurveReName(tempCurve.Name, lstWorkCurve);
            if (DialogResult.OK == frmReName.ShowDialog())
            {
                tempCurve.Name = frmReName.newDeviceName;
                tempCurve.Save();

                Lephone.Data.DbEntry.Context.ExecuteNonQuery("update workcurve set SimilarCurveName='" + tempCurve.Name + "' where  SimilarCurveId=" + tempCurve.Id);
            }
            dgvwCurveList.Refresh();
            WorkCurveToGrid();
        }

        private void dgvwCurveList_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            //if (dgvwCurveList.IsCurrentCellDirty)
            //    dgvwCurveList.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvwCurveList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvwCurveList.Rows.Count > 0
                && e.ColumnIndex==ColSimilarCurve.Index
                && dgvwCurveList.Columns[e.ColumnIndex].Name.Equals("ColSimilarCurve"))
            {
                int sSimilarCurveId = (dgvwCurveList.Rows[e.RowIndex].Cells["ColSimilarCurve"] == null 
                    || dgvwCurveList.Rows[e.RowIndex].Cells["ColSimilarCurve"].Value == null) ? 0 : Convert.ToInt32(dgvwCurveList.Rows[e.RowIndex].Cells["ColSimilarCurve"].Value.ToString());

                List<WorkCurve> WorkCurveList = WorkCurve.FindAll();

                WorkCurve workcurve = WorkCurveList.Find(w => w.Name == Convert.ToString(dgvwCurveList.Rows[e.RowIndex].Cells["ColName"].Value));


                if (sSimilarCurveId != 0 
                    && WorkCurveList.Exists(delegate(WorkCurve v) { return v.SimilarCurveId == sSimilarCurveId && v.Name != Convert.ToString(dgvwCurveList.Rows[e.RowIndex].Cells["ColName"].Value); }))
                {
                    

                    ((DataGridViewComboBoxCell)dgvwCurveList.Rows[e.RowIndex].Cells["ColSimilarCurve"]).Value = workcurve.SimilarCurveId;
                    return;
                }

                //WorkCurve cworkcurve = WorkCurveList.Find(w => w.Name == Convert.ToString(dgvwCurveList.Rows[e.RowIndex].Cells["ColName"].Value));
                if (workcurve.Id == sSimilarCurveId)
                {
                    ((DataGridViewComboBoxCell)dgvwCurveList.Rows[e.RowIndex].Cells["ColSimilarCurve"]).Value = workcurve.SimilarCurveId;
                    return;
                }

                Lephone.Data.DbEntry.Context.ExecuteNonQuery("update workcurve set " + ((sSimilarCurveId == 0) ? "SimilarCurveName='',SimilarCurveId=0" 
                    : " SimilarCurveName=(select name from workcurve where id=" + sSimilarCurveId + "),SimilarCurveId=" + sSimilarCurveId) + " where  name='" + Convert.ToString(dgvwCurveList.Rows[e.RowIndex].Cells["ColName"].Value) + "'");

               
                
                lstWorkCurve.Find(a => a.Id == workcurve.Id).SimilarCurveId = sSimilarCurveId;
              
            }

        }

        private void dgvwCurveList_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgvwCurveList.Columns[e.ColumnIndex].Name.Equals("colTestTime"))
            {
                try { int.Parse(e.FormattedValue.ToString()); }
                catch (FormatException)
                {
                    dgvwCurveList[e.ColumnIndex, e.RowIndex].Value = 40;
                    e.Cancel = true;
                    return;
                }
            }
        }       

        
    }
}
