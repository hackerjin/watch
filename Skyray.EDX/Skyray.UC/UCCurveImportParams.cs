using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;
using System.Xml;
using Skyray.EDX.Common;

namespace Skyray.UC
{
    public partial class UCCurveImportParams : Skyray.Language.UCMultiple
    {

        private string _cureName;
        private string _conditionNme;
        private string _path;

        public UCCurveImportParams()
        {
            InitializeComponent();
            rdoNotLoadCondition.Checked = true;
        }

        private void btnSelectPath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBoxW1.Text = this.openFileDialog1.FileName;
                if (this.textBoxW1.Text.IsNullOrEmpty())
                {
                    Msg.Show(Info.TemplatePathEmpty);
                    return;
                }
                WorkCurve curve = WorkCurve.New;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(this.textBoxW1.Text);
                string curveName = "";
                string conditionName = "";

                try
                {
                    curveName = xmlDoc.SelectSingleNode("CurveInfo/WorkCurve/Name").InnerText;
                    conditionName = xmlDoc.SelectSingleNode("CurveInfo/Condition/Name").InnerText;
                }
                catch  { }

                if (curveName.IsNullOrEmpty() || conditionName.IsNullOrEmpty())
                {
                    Msg.Show(Info.FileFormatError);
                    return;
                }
                

                this.txbCurveReName.Text = curveName;
                this.txbReNameConditon.Text = conditionName;
                _path = this.textBoxW1.Text;
            }
        }


        private string ExistNodeForValue(XmlNode currentNode, string label, string defaultStr)
        {
            if (currentNode != null && currentNode.SelectSingleNode(label) != null)
                return currentNode.SelectSingleNode(label).InnerText;
            else
                return defaultStr;
        }


        private bool CurveImport(string path)
        {
            if (_conditionNme.Equals(Info.Match) || _conditionNme.Equals(Info.IntelligentCondition))
            {
                string strFormat = Info.ConditionCanotBe + Info.Match + "," + Info.IntelligentCondition;
                Msg.Show(strFormat);
                return false;
            }
            //查找曲线
            string sql= @"select * from WorkCurve a inner join Condition b on a.Condition_Id = b.Id 
            inner join Device c on b.Device_Id=c.Id where c.Id=" + WorkCurveHelper.DeviceCurrent.Id + " and a.Name='" + _cureName+"'";
            List<WorkCurve> findCurve = WorkCurve.FindBySql(sql);
            //查找条件
            sql = "select * from Condition a where a.Device_Id=" + WorkCurveHelper.DeviceCurrent.Id + " and a.Name='" + _conditionNme + "'";
            List<Condition> returnCondi = Condition.FindBySql(sql);

            if (rdoNotLoadCondition.Checked && returnCondi != null && returnCondi.Count > 0)
             {
                 Msg.Show(Info.SelectConditionExist);
                 return false;
             }
            if (rdoNotLoadCondition.Checked && findCurve != null && findCurve.Count > 0)
            {
                Msg.Show(Info.SelectCurveExist);
                return false;
            }
            ImputData data = new ImputData();
            data.CurveImport(path, WorkCurveHelper.DeviceCurrent, rdoUseOldCondition.Checked, chbCurveReName.Checked, _conditionNme, _cureName);
            #region
            //            try
//            {
//                //曲线名重名，在原来的曲线中修改
//                if (chbCurveReName.Checked && findCurve != null && findCurve.Count > 0)
//                    curve = findCurve[0];
//                else curve = WorkCurve.New;
//                //curve.Name = xmlDoc.SelectSingleNode("CurveInfo/WorkCurve/Name").InnerText;
//                curve.Name = _cureName;
//                curve.CalcType = (CalcType)Enum.Parse(typeof(CalcType), ExistNodeForValue(xmlDoc,"CurveInfo/WorkCurve/CalcType",CalcType.EC.ToString()));
//                curve.FuncType = (FuncType)Enum.Parse(typeof(FuncType), ExistNodeForValue(xmlDoc,"CurveInfo/WorkCurve/FuncType",FuncType.XRF.ToString()));
//                curve.IsAbsorb = bool.Parse(ExistNodeForValue(xmlDoc,"CurveInfo/WorkCurve/IsAbsorb",false.ToString()));
//                curve.IsThickShowContent = bool.Parse(ExistNodeForValue(xmlDoc,"CurveInfo/WorkCurve/IsThickShowContent",false.ToString()));
//                curve.LimitThickness = int.Parse(ExistNodeForValue(xmlDoc,"CurveInfo/WorkCurve/LimitThickness","0"));
//                curve.RemoveBackGround = bool.Parse(ExistNodeForValue(xmlDoc,"CurveInfo/WorkCurve/RemoveBackGround",false.ToString()));
//                curve.RemoveSum = bool.Parse(ExistNodeForValue(xmlDoc,"CurveInfo/WorkCurve/RemoveSum",false.ToString()));
//                curve.RemoveEscape = bool.Parse(ExistNodeForValue(xmlDoc,"CurveInfo/WorkCurve/RemoveEscape",false.ToString()));
//                //curve.IsDefaultWorkCurve = bool.Parse(xmlDoc.SelectSingleNode("CurveInfo/WorkCurve/IsDefaultWorkCurve").InnerText);
//                curve.IsDefaultWorkCurve = curve.IsDefaultWorkCurve||false;
//                curve.IsJoinMatch = bool.Parse(ExistNodeForValue(xmlDoc,"CurveInfo/WorkCurve/IsJoinMatch",false.ToString()));
//                Condition condition = null;
//                List<CurveElement> lstElements = new List<CurveElement>();
//                List<CustomField> listCustoms = new List<CustomField>();
//                //运用原来老的条件
//                if (rdoUseOldCondition.Checked && returnCondi != null && returnCondi.Count > 0)
//                {
//                    curve.Condition = returnCondi[0];
//                }
//                //覆盖原来的旧条件，或者是新建条件
//                else
//                {
//                    InitParameter initParams = null;
//                    if (rdoModifyCondition.Checked && returnCondi != null && returnCondi.Count > 0)
//                    {
//                        condition = returnCondi[0];
//                        initParams = condition.InitParam;
//                    }
//                    else
//                    {
//                        condition = Condition.New;
//                        initParams = InitParameter.New;
//                        // condition.Name = xmlDoc.SelectSingleNode("CurveInfo/Condition/Name").InnerText;
//                        condition.Name = _conditionNme;
//                    }
//                    condition.Type = ConditionType.Normal; //(ConditionType)Enum.Parse(typeof(ConditionType), xmlDoc.SelectSingleNode("CurveInfo/Condition/Type").InnerText);
//                    initParams.TubCurrent = int.Parse(ExistNodeForValue(xmlDoc,"CurveInfo/Condition/TubCurrent","100"));
//                    initParams.TubVoltage = int.Parse(ExistNodeForValue(xmlDoc,"CurveInfo/Condition/TubVoltage","40"));
//                    initParams.Gain = float.Parse(ExistNodeForValue(xmlDoc,"CurveInfo/Condition/Gain","60"));
//                    initParams.FineGain = float.Parse(ExistNodeForValue(xmlDoc,"CurveInfo/Condition/FineGain","120"));
//                    initParams.Channel = int.Parse(ExistNodeForValue(xmlDoc,"CurveInfo/Condition/Channel","1105"));
//                    initParams.ChannelError = int.Parse(ExistNodeForValue(xmlDoc,"CurveInfo/Condition/ChannelError","0"));
//                    initParams.Filter = int.Parse(ExistNodeForValue(xmlDoc,"CurveInfo/Condition/Filter","1"));
//                    initParams.Collimator = int.Parse(ExistNodeForValue(xmlDoc,"CurveInfo/Condition/Collimator","1"));
//                    initParams.ElemName = ExistNodeForValue(xmlDoc,"CurveInfo/Condition/ElemName",string.Empty);
//                    condition.InitParam = initParams;
//                    condition.Device = WorkCurveHelper.DeviceCurrent;
//                    //删除
//                    //condition.DeviceParamList.Clear();
//                    XmlNodeList listDeviceParams = xmlDoc.SelectNodes("CurveInfo/DeviceParamList/DeviceParameter");
//                    foreach (XmlNode mode in listDeviceParams)
//                    {
//                        int ConditionIndex=-1;
//                        string ConditionName = mode.SelectSingleNode("Name").InnerText;
//                        for (int i = 0; i < condition.DeviceParamList.Count;i++ )
//                        {
//                            if (condition.DeviceParamList[i].Name.Equals(ConditionName))
//                            {
//                                ConditionIndex = i;
//                                break;
//                            }
//                        }
//                        DeviceParameter deviceParams=null; 
//                        if (ConditionIndex>=0)//修改
//                        {
//                            deviceParams = condition.DeviceParamList[ConditionIndex];
//                            deviceParams.PrecTime = int.Parse(ExistNodeForValue(mode,"PrecTime","50"));
//                            deviceParams.TubCurrent = int.Parse(ExistNodeForValue(mode,"TubCurrent","100"));
//                            deviceParams.TubVoltage = int.Parse(ExistNodeForValue(mode,"TubVoltage","40"));
//                            deviceParams.FilterIdx = int.Parse(ExistNodeForValue(mode,"Filter","1"));
//                            deviceParams.CollimatorIdx = int.Parse(ExistNodeForValue(mode,"Collimator","1"));
//                            deviceParams.IsVacuum = bool.Parse(ExistNodeForValue(mode,"IsVacuum",false.ToString()));
//                            deviceParams.VacuumTime = int.Parse(ExistNodeForValue(mode,"VacuumTime","0"));
//                            deviceParams.IsVacuumDegree = bool.Parse(ExistNodeForValue(mode,"IsVacuumDegree",false.ToString()));
//                            deviceParams.VacuumDegree = double.Parse(ExistNodeForValue(mode,"VacuumDegree","0.00"));
//                            deviceParams.IsAdjustRate = bool.Parse(ExistNodeForValue(mode,"IsAdjustRate",false.ToString()));
//                            deviceParams.MinRate = int.Parse(ExistNodeForValue(mode,"MinRate","1000"));
//                            deviceParams.MaxRate = int.Parse(ExistNodeForValue(mode,"MaxRate","4000"));
//                            deviceParams.BeginChann = int.Parse(ExistNodeForValue(mode,"BeginChann","50"));
//                            deviceParams.EndChann = int.Parse(ExistNodeForValue(mode,"EndChann",((int)WorkCurveHelper.DeviceCurrent.SpecLength-50).ToString()));
//                            deviceParams.IsDistrubAlert = bool.Parse(ExistNodeForValue(mode,"IsDistrubAlert",false.ToString()));
//                            deviceParams.IsPeakFloat = bool.Parse(ExistNodeForValue(mode,"IsPeakFloat",false.ToString()));
//                            deviceParams.PeakFloatLeft = int.Parse(ExistNodeForValue(mode,"PeakFloatLeft","0"));
//                            deviceParams.PeakFloatRight = int.Parse(ExistNodeForValue(mode,"PeakFloatRight","0"));
//                            deviceParams.PeakFloatChannel = int.Parse(ExistNodeForValue(mode,"PeakFloatChannel","0"));
//                            deviceParams.PeakCheckTime = int.Parse(ExistNodeForValue(mode,"PeakCheckTime","15"));
//                            deviceParams.PeakFloatError = int.Parse(ExistNodeForValue(mode,"PeakFloatError","0"));
//                        }
//                        else //追加
//                        {
//                            deviceParams = DeviceParameter.New;
//                            deviceParams.Name = ExistNodeForValue(mode,"Name",string.Empty);
//                            deviceParams.PrecTime = int.Parse(ExistNodeForValue(mode,"PrecTime","50"));
//                            deviceParams.TubCurrent = int.Parse(ExistNodeForValue(mode,"TubCurrent","100"));
//                            deviceParams.TubVoltage = int.Parse(ExistNodeForValue(mode,"TubVoltage","40"));
//                            deviceParams.FilterIdx = int.Parse(ExistNodeForValue(mode,"Filter","1"));
//                            deviceParams.CollimatorIdx = int.Parse(ExistNodeForValue(mode,"Collimator","1"));
//                            deviceParams.IsVacuum = bool.Parse(ExistNodeForValue(mode,"IsVacuum",false.ToString()));
//                            deviceParams.VacuumTime = int.Parse(ExistNodeForValue(mode,"VacuumTime","0"));
//                            deviceParams.IsVacuumDegree = bool.Parse(ExistNodeForValue(mode,"IsVacuumDegree",false.ToString()));
//                            deviceParams.VacuumDegree = double.Parse(ExistNodeForValue(mode,"VacuumDegree","0"));
//                            deviceParams.IsAdjustRate = bool.Parse(ExistNodeForValue(mode,"IsAdjustRate",false.ToString()));
//                            deviceParams.MinRate = int.Parse(ExistNodeForValue(mode,"MinRate","1000"));
//                            deviceParams.MaxRate = int.Parse(ExistNodeForValue(mode,"MaxRate","4000"));
//                            deviceParams.BeginChann = int.Parse(ExistNodeForValue(mode,"BeginChann","50"));
//                            deviceParams.EndChann = int.Parse(ExistNodeForValue(mode,"EndChann",((int)WorkCurveHelper.DeviceCurrent.SpecLength-50).ToString()));
//                            deviceParams.IsDistrubAlert = bool.Parse(ExistNodeForValue(mode,"IsDistrubAlert",false.ToString()));
//                            deviceParams.IsPeakFloat = bool.Parse(ExistNodeForValue(mode,"IsPeakFloat",false.ToString()));
//                            deviceParams.PeakFloatLeft = int.Parse(ExistNodeForValue(mode,"PeakFloatLeft","0"));
//                            deviceParams.PeakFloatRight = int.Parse(ExistNodeForValue(mode,"PeakFloatRight","0"));
//                            deviceParams.PeakFloatChannel = int.Parse(ExistNodeForValue(mode,"PeakFloatChannel","0"));
//                            deviceParams.PeakCheckTime = int.Parse(ExistNodeForValue(mode,"PeakCheckTime","15"));
//                            deviceParams.PeakFloatError = int.Parse(ExistNodeForValue(mode,"PeakFloatError","0"));
//                            condition.DeviceParamList.Insert(condition.DeviceParamList.Count, deviceParams);
//                        }
//                    }
//                    //condition.DemarcateEnergys.Clear();
//                    listDeviceParams = xmlDoc.SelectNodes("CurveInfo/DemarcateEnergyList/DemarcateEnergy");
//                    foreach (XmlNode energyNode in listDeviceParams)
//                    {
//                        string elementName=ExistNodeForValue(energyNode,"ElementName",string.Empty);

//                        //if (condition.DemarcateEnergys.Count>0 && condition.DemarcateEnergys.First(wde => wde.ElementName.Equals(elementName)) != null)
//                        if (condition.DemarcateEnergys.Count > 0 && condition.DemarcateEnergys.ToList().Exists(delegate(DemarcateEnergy v) { return v.ElementName == elementName; }))
//                        {
//                            continue;
//                        }
//                        DemarcateEnergy newEnergy = DemarcateEnergy.New;
//                        newEnergy.ElementName = ExistNodeForValue(energyNode,"ElementName",string.Empty);
//                        newEnergy.Channel = double.Parse(ExistNodeForValue(energyNode,"Channel","0"));
//                        newEnergy.Energy = double.Parse(ExistNodeForValue(energyNode,"Energy","0"));
//                        newEnergy.Line = (XLine)Enum.Parse(typeof(XLine),ExistNodeForValue(energyNode,"Line",XLine.Ka.ToString()));
//                        condition.DemarcateEnergys.Add(newEnergy);
//                    }
//                    curve.Condition = condition;
//                    curve.ConditionName = curve.Condition.Name;
//                    condition.Save();
                    
//                }
//                curve.ConditionName = curve.Condition.Name;
//                XmlNode selectElementList = xmlDoc.SelectSingleNode("CurveInfo/ElementList");

                
//                if (selectElementList != null)
//                {
//                    ElementList elementList;
//                    //曲线名重名，在原来的曲线中修改
//                    if (chbCurveReName.Checked && findCurve != null && findCurve.Count > 0)
//                    {
//                        if (curve.ElementList != null)
//                        elementList = curve.ElementList;
//                        else
//                            elementList = ElementList.New;
//                    }
//                    else elementList = ElementList.New;
//                    elementList.IsUnitary = bool.Parse(ExistNodeForValue(selectElementList,"IsUnitary",false.ToString()));
//                    elementList.UnitaryValue = double.Parse(ExistNodeForValue(selectElementList,"UnitaryValue","99.96"));
//                    elementList.TubeWindowThickness = double.Parse(ExistNodeForValue(selectElementList,"TubeWindowThickness","0"));
//                    elementList.RhIsLayer = bool.Parse(ExistNodeForValue(selectElementList,"RhIsLayer",false.ToString()));
//                    elementList.RhLayerFactor = double.Parse(ExistNodeForValue(selectElementList,"RhLayerFactor","0"));
//                    elementList.IsAbsorption = bool.Parse(ExistNodeForValue(selectElementList,"IsAbsorption",false.ToString()));
//                    elementList.ThCalculationWay = (ThCalculationWay)Enum.Parse(typeof(ThCalculationWay), ExistNodeForValue(selectElementList,"ThCalculationWay",ThCalculationWay.ThInsert.ToString()));
//                    elementList.DBlLimt = double.Parse(ExistNodeForValue(selectElementList,"DBlLimt","0.99999"));
//                    elementList.IsRemoveBk = bool.Parse(ExistNodeForValue(selectElementList,"IsRemoveBk",false.ToString()));
//                    elementList.SpecListId = long.Parse(ExistNodeForValue(selectElementList,"SpecListId","0"));
//                    elementList.IsReportCategory = bool.Parse(ExistNodeForValue(selectElementList,"IsReportCategory",false.ToString()));
//                    elementList.PureAsInfinite = bool.Parse(ExistNodeForValue(selectElementList,"PureAsInfinite",false.ToString()));
//                    elementList.MatchSpecListIdStr = ExistNodeForValue(selectElementList,"MatchSpecListIdStr",string.Empty);
//                    lstElements = elementList.Items.ToList();
//                    elementList.Items.Clear();

//                    #region  元素
//                    XmlNodeList curveElementList = selectElementList.SelectNodes("CurveElement");
//                    foreach (XmlNode curveElementNode in curveElementList)
//                    {
//                        CurveElement curveElement = CurveElement.New;
//                        if (curve.Condition == null)
//                            continue;
//                        curveElement.DevParamId = curve.Condition.DeviceParamList.First(w => w.Name == curveElementNode.SelectSingleNode("DeviceParamsName").InnerText.ToString()).Id;
//                        curveElement.Caption = ExistNodeForValue(curveElementNode,"Caption",string.Empty);
//                        curveElement.IsDisplay = bool.Parse(ExistNodeForValue(curveElementNode,"IsDisplay",false.ToString()));
//                        curveElement.Formula = ExistNodeForValue(curveElementNode,"Formula",string.Empty);
//                        curveElement.AtomicNumber = int.Parse(ExistNodeForValue(curveElementNode,"AtomicNumber","1"));
//                        curveElement.LayerNumber = int.Parse(ExistNodeForValue(curveElementNode,"LayerNumber","0"));
//                        curveElement.LayerNumBackUp = ExistNodeForValue(curveElementNode,"LayerNumBackUp","0");
//                        curveElement.AnalyteLine = (XLine)Enum.Parse(typeof(XLine), ExistNodeForValue(curveElementNode,"AnalyteLine",XLine.Ka.ToString()));
//                        curveElement.PeakLow = int.Parse(ExistNodeForValue(curveElementNode,"PeakLow","0"));
//                        curveElement.PeakHigh = int.Parse(ExistNodeForValue(curveElementNode,"PeakHigh","0"));
//                        curveElement.BaseLow = int.Parse(ExistNodeForValue(curveElementNode, "BaseLow", "0"));
//                        curveElement.BaseHigh = int.Parse(ExistNodeForValue(curveElementNode,"BaseHigh","0"));
//                        curveElement.PeakDivBase = bool.Parse(ExistNodeForValue(curveElementNode,"PeakDivBase",false.ToString()));
//                        curveElement.LayerDensity = int.Parse(ExistNodeForValue(curveElementNode,"LayerDensity","1"));
//                        curveElement.IntensityWay = (IntensityWay)Enum.Parse(typeof(IntensityWay), ExistNodeForValue(curveElementNode,"IntensityWay",IntensityWay.FullArea.ToString()));
//                        curveElement.IntensityComparison = bool.Parse(ExistNodeForValue(curveElementNode,"IntensityComparison",false.ToString()));
//                        curveElement.ComparisonCoefficient = int.Parse(ExistNodeForValue(curveElementNode,"ComparisonCoefficient","1"));
//                        curveElement.BPeakLow = int.Parse(ExistNodeForValue(curveElementNode,"BPeakLow","0"));
//                        curveElement.BPeakHigh = int.Parse(ExistNodeForValue(curveElementNode,"BPeakHigh","0"));
//                        curveElement.CalculationWay = (CalculationWay)Enum.Parse(typeof(CalculationWay), ExistNodeForValue(curveElementNode,"CalculationWay",CalculationWay.Insert.ToString()));
//                        curveElement.FpCalculationWay = (FpCalculationWay)Enum.Parse(typeof(FpCalculationWay), ExistNodeForValue(curveElementNode,"FpCalculationWay",FpCalculationWay.LinearWithAnIntercept.ToString()));
//                        curveElement.Flag = (ElementFlag)Enum.Parse(typeof(ElementFlag), ExistNodeForValue(curveElementNode,"Flag",ElementFlag.Added.ToString()));
//                        curveElement.LayerFlag = (LayerFlag)Enum.Parse(typeof(LayerFlag), ExistNodeForValue(curveElementNode,"LayerFlag",LayerFlag.Calculated.ToString()));
//                        //curveElement. = (ElementFlag)Enum.Parse(typeof(ElementFlag), ExistNodeForValue(curveElementNode,"Flag").InnerText.ToString());
//                        //curveElement.LayerFlag = (LayerFlag)Enum.Parse(typeof(LayerFlag), ExistNodeForValue(curveElementNode,"LayerFlag").InnerText.ToString());
//                        // add by chuyaqin 2011-04-28

//                        curveElement.ContentUnit = (ContentUnit)Enum.Parse(typeof(ContentUnit), ExistNodeForValue(curveElementNode,"ContentUnit",ContentUnit.per.ToString()));
//                        curveElement.ThicknessUnit = (ThicknessUnit)Enum.Parse(typeof(ThicknessUnit), ExistNodeForValue(curveElementNode,"ThicknessUnit",ThicknessUnit.um.ToString()));
//                        curveElement.SReferenceElements = ExistNodeForValue(curveElementNode,"SReferenceElements",string.Empty);
//                        curveElement.SSpectrumData = ExistNodeForValue(curveElementNode,"SSpectrumData",string.Empty);
//                        curveElement.SInfluenceElements = ExistNodeForValue(curveElementNode,"SInfluenceElements",string.Empty);
//                        curveElement.DistrubThreshold = ExistNodeForValue(curveElementNode,"DistrubThreshold",string.Empty);
//                        curveElement.IsInfluence = bool.Parse(ExistNodeForValue(curveElementNode,"IsInfluence",false.ToString()));
//                        curveElement.SInfluenceCoefficients = ExistNodeForValue(curveElementNode,"SInfluenceCoefficients",string.Empty);
//                        curveElement.ElementSpecName = ExistNodeForValue(curveElementNode,"ElementSpecName",string.Empty);
//                        curveElement.Color = int.Parse(ExistNodeForValue(curveElementNode,"Color",Color.Blue.ToArgb().ToString()));
//                        curveElement.ColorHelper = ExistNodeForValue(curveElementNode,"ColorHelper",string.Empty);

//                        #region 标样
//                        XmlNodeList standSampleList = curveElementNode.SelectNodes("StandSample");
//                        if (standSampleList != null)
//                        {
//                            foreach (XmlNode standSampleNode in standSampleList)
//                            {
//                                StandSample standSample = StandSample.New;
//                                standSample.SampleName = ExistNodeForValue(standSampleNode,"SampleName",string.Empty);
//                                standSample.X = float.Parse(ExistNodeForValue(standSampleNode,"X","0"));
//                                standSample.Y = float.Parse(ExistNodeForValue(standSampleNode, "Y", "0"));
//                                standSample.Z = float.Parse(ExistNodeForValue(standSampleNode, "Z", "0"));
//                                standSample.TheoryX = float.Parse(ExistNodeForValue(standSampleNode, "TheoryX", "0"));
//                                standSample.Active = bool.Parse(ExistNodeForValue(standSampleNode,"Active",false.ToString()));
//                                standSample.ElementName = ExistNodeForValue(standSampleNode,"ElementName",string.Empty);
//                                standSample.SpecListId = long.Parse(ExistNodeForValue(standSampleNode,"SpecListId","0"));
//                                standSample.Layer = int.Parse(ExistNodeForValue(standSampleNode, "Layer", "0"));
//                                standSample.Level = ExistNodeForValue(standSampleNode,"Level",string.Empty);
//                                standSample.TotalLayer = int.Parse(ExistNodeForValue(standSampleNode, "TotalLayer", "0"));
//                                standSample.IsMatch = bool.Parse(ExistNodeForValue(standSampleNode, "IsMatch", "0"));
//                                standSample.MatchSpecListId = int.Parse(ExistNodeForValue(standSampleNode, "MatchSpecListId", "0"));
//                                standSample.MatchSpecName = ExistNodeForValue(standSampleNode,"MatchSpecListId",string.Empty);
//                                curveElement.Samples.Add(standSample);
//                            }
//                        }
//                        #endregion

//                        XmlNodeList ReferenceElementList = curveElementNode.SelectNodes("ReferenceElement");
//                        if (ReferenceElementList != null)
//                        {
//                            foreach (XmlNode ReferenceElementNode in ReferenceElementList)
//                            {
//                                ReferenceElement referenceElement = ReferenceElement.New;
//                                referenceElement.MainElementName = ExistNodeForValue(ReferenceElementNode,"MainElementName",string.Empty);
//                                referenceElement.ReferenceElementName = ExistNodeForValue(ReferenceElementNode, "ReferenceElementName", string.Empty);
//                                referenceElement.ReferenceLeftBorder = int.Parse(ExistNodeForValue(ReferenceElementNode,"ReferenceLeftBorder","0"));
//                                referenceElement.ReferenceRightBorder = int.Parse(ExistNodeForValue(ReferenceElementNode, "ReferenceRightBorder", "0"));
//                                referenceElement.ReferenceBackLeft = int.Parse(ExistNodeForValue(ReferenceElementNode, "ReferenceBackLeft", "0"));
//                                referenceElement.ReferenceBackRight = int.Parse(ExistNodeForValue(ReferenceElementNode, "ReferenceBackRight", "0"));
//                                referenceElement.PeakToBack = bool.Parse(ExistNodeForValue(ReferenceElementNode,"PeakToBack",false.ToString()));
//                                curveElement.References.Add(referenceElement);
//                            }
//                        }
//                        #region 元素对应的优化因子
//                        XmlNodeList OptimizationList = curveElementNode.SelectNodes("Optimiztion");
//                        if (OptimizationList != null)
//                        {
//                            foreach (XmlNode OptimizationNode in OptimizationList)
//                            {
//                                Optimiztion optimize = Optimiztion.New;
//                                optimize.OptimiztionValue = double.Parse(ExistNodeForValue(OptimizationNode,"OptimiztionValue","0"));
//                                optimize.OptimiztionRange = double.Parse(ExistNodeForValue(OptimizationNode, "OptimiztionRange", "0"));
//                                optimize.OptimiztionMax = double.Parse(ExistNodeForValue(OptimizationNode, "OptimiztionMax", "0"));
//                                optimize.OptimiztionMin = double.Parse(ExistNodeForValue(OptimizationNode, "OptimiztionMin", "0"));
//                                optimize.OptimiztionFactor = double.Parse(ExistNodeForValue(OptimizationNode, "OptimiztionFactor", "0"));
//                                curveElement.Optimiztion.Add(optimize);
//                            }
//                        }
//                        #endregion

//                        #region 元素对应的影响元素
//                        XmlNodeList ElementRefList = curveElementNode.SelectNodes("ElementRefs");
//                        if (ElementRefList != null)
//                        {
//                            foreach (XmlNode ElementRdfNode in ElementRefList)
//                            {
//                                ElementRef elementRef = ElementRef.New;
//                                elementRef.Name = ExistNodeForValue(ElementRdfNode,"Name",string.Empty);
//                                elementRef.IsRef = bool.Parse(ExistNodeForValue(ElementRdfNode,"IsRef",false.ToString()));
//                                elementRef.RefConf = double.Parse(ExistNodeForValue(ElementRdfNode,"RefConf","0"));
//                                elementRef.DistrubThreshold = double.Parse(ExistNodeForValue(ElementRdfNode, "DistrubThreshold", "0"));
//                                curveElement.ElementRefs.Add(elementRef);
//                            }
//                        }
//                        #endregion
//                        elementList.Items.Add(curveElement);

//                    }
//                    #endregion

//                    #region 元素自定义
                   
//                    listCustoms = elementList.CustomFields.ToList();
//                    elementList.CustomFields.Clear();
//                    XmlNodeList CustomList = selectElementList.SelectNodes("CustomFields");
//                    if (CustomList != null)
//                    {
//                        foreach (XmlNode custom in CustomList)
//                        {
//                            CustomField cf = CustomField.New;
//                            cf.Expression = custom.SelectSingleNode("Expression").InnerText;
//                            cf.Name = custom.SelectSingleNode("Name").InnerText;
//                            elementList.CustomFields.Add(cf);
//                        }
//                    }
//                    #endregion
//                    curve.ElementList = elementList;
//                }
//                else curve.ElementList = null;
               
//                #region  曲线下的校正强度数据
//                List<IntensityCalibration> listItensitycal = new List<IntensityCalibration>();
//                listItensitycal = curve.IntensityCalibration.ToList();
//                curve.IntensityCalibration.Clear();
//                XmlNodeList IntensityCallist = xmlDoc.SelectNodes("IntensityCalibration");
//                if (IntensityCallist != null)
//                {
//                    foreach (XmlNode custom in IntensityCallist)
//                    {
//                        IntensityCalibration cf = IntensityCalibration.New;
//                        cf.CalibrateIn = double.Parse(custom.SelectSingleNode("CalibrateIn").InnerText);
//                        cf.Element = custom.SelectSingleNode("Element").InnerText;
//                        cf.OriginalIn = double.Parse(custom.SelectSingleNode("OriginalIn").InnerText);
//                        curve.IntensityCalibration.Add(cf);
//                    }
//                }
//                #endregion
                

//                XmlNode WorkRegionNode = xmlDoc.SelectSingleNode("CurveInfo/WorkRegion");
//                if (WorkRegionNode != null && curve != null)
//                {
//                    WorkRegion temp = WorkRegion.FindOne(w => w.Name == WorkRegionNode.SelectSingleNode("Name").InnerText);
//                    if (temp != null)
//                        curve.WorkRegion = temp;
//                    else
//                    {
//                        WorkRegion rgion = WorkRegion.New;
//                        rgion.Name = WorkRegionNode.SelectSingleNode("Name").InnerText;
//                        rgion.RohsSampleType = (RohsSampleType)Enum.Parse(typeof(RohsSampleType), WorkRegionNode.SelectSingleNode("RohsSampleType").InnerText);
//                        rgion.IsDefaultWorkRegion = bool.Parse(WorkRegionNode.SelectSingleNode("IsDefaultWorkRegion").InnerText);
//                        curve.WorkRegion = rgion;
//                    }
//                }
//                curve.Save();
//                for (int i = 0; i < lstElements.Count; i++)
//                {
//                    lstElements[i].Delete();
//                }
//                for (int i = 0; i < listCustoms.Count; i++)
//                {
//                    listCustoms[i].Delete();
//                }
//                for (int i = 0; i < listItensitycal.Count; i++)
//                {
//                    listItensitycal[i].Delete();
//                }
//            }
//            catch (Exception e){
//                Msg.Show(e.Message);
            //            }
            #endregion
            return true;

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            _cureName = txbCurveReName.Text;
            _conditionNme = txbReNameConditon.Text;
            if (_path.IsNullOrEmpty()||!CurveImport(_path))
                return;
            this.dialogResult = DialogResult.OK;
            EDXRFHelper.GotoMainPage(this);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            EDXRFHelper.GotoMainPage(this);
        }

        private void rdoNotLoadCondition_CheckedChanged(object sender, EventArgs e)
        {

            if (rdoManControl.Checked)
            {
                chbCurveReName.Checked = true;
                chbCurveReName.Enabled = false;
                grpConditin.Enabled = true;
                grpSettingParams.Enabled = true;
            }
            else
            {
                grpConditin.Enabled = false;
                grpSettingParams.Enabled = false;
            }
        }

        private void rdoManControl_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoManControl.Checked)
                rdoUseOldCondition.Checked = true;
        }

    }
}
