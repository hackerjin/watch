using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using Skyray.EDX.Common;
using Skyray.EDXRFLibrary;
using System.Drawing;

using System.IO;
using Lephone.Data.Definition;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.EDX.Common
{
    public class ImputData
    {
        public List<string> FileList = new List<string>();
        public List<string> CurveFileList = new List<string>();


        private string ExistNodeForValue(XmlNode currentNode, string label, string defaultStr)
        {
            if (currentNode != null && currentNode.SelectSingleNode(label) != null)
                return currentNode.SelectSingleNode(label).InnerText;
            else
                return defaultStr;
        }

        public void CurveImport(string path, Device device, bool isSimilarConditon, bool isSimilarCurve, string conditionNameInput, string curveNameInput)
        {
            XmlDocument xmlDoc = new XmlDocument();
            //using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            //{
            string rawXml = System.IO.File.ReadAllText(path);
            xmlDoc.LoadXml(rawXml);
            Color[] colorlist = GetElementColor();
            int icolor = 0;
            WorkCurve curve = null;
            string curveName = string.Empty;
            if (!string.IsNullOrEmpty(curveNameInput))
                curveName = curveNameInput;
            else
                curveName = ExistNodeForValue(xmlDoc, "CurveInfo/WorkCurve/Name", string.Empty);
            string sql = @"select * from WorkCurve a inner join Condition b on a.Condition_Id = b.Id 
            inner join Device c on b.Device_Id=c.Id where c.Id=" + device.Id + " and a.Name='" + curveName + "'";
            List<WorkCurve> findCurve = WorkCurve.FindBySql(sql);
            if (isSimilarCurve && findCurve != null && findCurve.Count > 0)
            {
                curve = findCurve[0];
            }
            else
            {
                if (findCurve != null && findCurve.Count > 0)
                    curve.Delete();
                curve = WorkCurve.New.Init("", "", CalcType.EC, FuncType.XRF, false, 0, false, false, false, false, false, 0, "", false, Info.strAreaDensityUnit, 40, true);
                curve.Name = curveName;
                curve.CalcType = (CalcType)Enum.Parse(typeof(CalcType), ExistNodeForValue(xmlDoc, "CurveInfo/WorkCurve/CalcType", CalcType.EC.ToString()));
                curve.FuncType = (FuncType)Enum.Parse(typeof(FuncType), ExistNodeForValue(xmlDoc, "CurveInfo/WorkCurve/FuncType", FuncType.XRF.ToString()));
                curve.IsAbsorb = bool.Parse(ExistNodeForValue(xmlDoc, "CurveInfo/WorkCurve/IsAbsorb", false.ToString()));
                //curve.IsThickShowContent = bool.Parse(ExistNodeForValue(xmlDoc, "CurveInfo/WorkCurve/IsThickShowContent", false.ToString()));
                curve.LimitThickness = int.Parse(ExistNodeForValue(xmlDoc, "CurveInfo/WorkCurve/LimitThickness", "0"));
                curve.RemoveBackGround = bool.Parse(ExistNodeForValue(xmlDoc, "CurveInfo/WorkCurve/RemoveBackGround", false.ToString()));
                curve.RemoveSum = bool.Parse(ExistNodeForValue(xmlDoc, "CurveInfo/WorkCurve/RemoveSum", false.ToString()));
                curve.RemoveEscape = bool.Parse(ExistNodeForValue(xmlDoc, "CurveInfo/WorkCurve/RemoveEscape", false.ToString()));
                curve.IsNiP2 = bool.Parse(ExistNodeForValue(xmlDoc, "CurveInfo/WorkCurve/IsNiP2", false.ToString()));
                curve.IsBaseAdjust = bool.Parse(ExistNodeForValue(xmlDoc, "CurveInfo/WorkCurve/IsBaseAdjust", false.ToString()));
                //判断是否已经存在默认的工作曲线，如果存在，则当前导入的不为默认
                sql = "select * from WorkCurve where Condition_Id in (select Id from condition where Type = 0 and Device_id = "
                    + device.Id + ") and FuncType=" + (int)WorkCurveHelper.DeviceFunctype + " and IsDefaultWorkCurve=1 order by LOWER(Name)";
                Lephone.Data.Common.DbObjectList<WorkCurve> lstWorkCurve = WorkCurve.FindBySql(sql);

                curve.IsDefaultWorkCurve = (lstWorkCurve.Count > 0) ? false : bool.Parse(xmlDoc.SelectSingleNode("CurveInfo/WorkCurve/IsDefaultWorkCurve").InnerText);
                curve.IsJoinMatch = bool.Parse(ExistNodeForValue(xmlDoc, "CurveInfo/WorkCurve/IsJoinMatch", false.ToString()));
            }
            string conditionName = string.Empty;
            if (!string.IsNullOrEmpty(conditionNameInput))
                conditionName = conditionNameInput;
            else
                conditionName = ExistNodeForValue(xmlDoc, "CurveInfo/Condition/Name", "");
            sql = "select * from Condition a where a.Device_Id=" + device.Id + " and a.Name='" + conditionName + "'";
            List<Condition> returnCondi = Condition.FindBySql(sql);
            Condition condition = null;
            if (isSimilarConditon && returnCondi != null && returnCondi.Count > 0)
            {
                condition = returnCondi[0];
            }
            else
            {
                InitParameter initParams = null;
                condition = Condition.New;
                initParams = InitParameter.New;
                if (returnCondi != null && returnCondi.Count > 0)
                {
                    sql = "select * from Condition a where a.Device_Id=" + device.Id + "";
                    List<Condition> silimarNameCon = Condition.FindBySql(sql);
                    if (silimarNameCon != null && silimarNameCon.Count > 0)
                        conditionName += silimarNameCon.Count;
                }
                condition.Name = conditionName;
                condition.Type = ConditionType.Normal; //(ConditionType)Enum.Parse(typeof(ConditionType), xmlDoc.SelectSingleNode("CurveInfo/Condition/Type").InnerText);
                initParams.TubCurrent = int.Parse(ExistNodeForValue(xmlDoc, "CurveInfo/Condition/TubCurrent", "100"));
                initParams.TubVoltage = int.Parse(ExistNodeForValue(xmlDoc, "CurveInfo/Condition/TubVoltage", "40"));
                initParams.Gain = float.Parse(ExistNodeForValue(xmlDoc, "CurveInfo/Condition/Gain", "60"));
                initParams.FineGain = float.Parse(ExistNodeForValue(xmlDoc, "CurveInfo/Condition/FineGain", "120"));
                initParams.Channel = int.Parse(ExistNodeForValue(xmlDoc, "CurveInfo/Condition/Channel", "1105"));
                initParams.ChannelError = int.Parse(ExistNodeForValue(xmlDoc, "CurveInfo/Condition/ChannelError", "0"));
                initParams.Filter = int.Parse(ExistNodeForValue(xmlDoc, "CurveInfo/Condition/Filter", "1"));
                initParams.Collimator = int.Parse(ExistNodeForValue(xmlDoc, "CurveInfo/Condition/Collimator", "1"));
                initParams.CurrentRate = int.Parse(ExistNodeForValue(xmlDoc, "CurveInfo/Condition/CurrentRate", "1"));
                initParams.ElemName = ExistNodeForValue(xmlDoc, "CurveInfo/Condition/ElemName", string.Empty);
                initParams.Target = int.Parse(ExistNodeForValue(xmlDoc, "CurveInfo/Condition/Target", "1"));
                initParams.TargetMode = (TargetMode)Enum.Parse(typeof(TargetMode), ExistNodeForValue(xmlDoc, "CurveInfo/Condition/TargetMode", "OneTarget"));

                XmlNodeList listDeviceParams = xmlDoc.SelectNodes("CurveInfo/DeviceParamList/DeviceParameter");
                foreach (XmlNode mode in listDeviceParams)
                {
                    DeviceParameter deviceParams = null;
                    deviceParams = DeviceParameter.New;
                    deviceParams.Name = ExistNodeForValue(mode, "Name", string.Empty);
                    deviceParams.PrecTime = int.Parse(ExistNodeForValue(mode, "PrecTime", "50"));
                    deviceParams.TubCurrent = int.Parse(ExistNodeForValue(mode, "TubCurrent", "100"));
                    deviceParams.TubVoltage = int.Parse(ExistNodeForValue(mode, "TubVoltage", "40"));
                    deviceParams.FilterIdx = int.Parse(ExistNodeForValue(mode, "Filter", "1"));
                    deviceParams.CollimatorIdx = int.Parse(ExistNodeForValue(mode, "Collimator", "1"));
                    deviceParams.IsVacuum = bool.Parse(ExistNodeForValue(mode, "IsVacuum", false.ToString()));
                    deviceParams.VacuumTime = int.Parse(ExistNodeForValue(mode, "VacuumTime", "0"));
                    deviceParams.IsVacuumDegree = bool.Parse(ExistNodeForValue(mode, "IsVacuumDegree", false.ToString()));
                    deviceParams.VacuumDegree = double.Parse(ExistNodeForValue(mode, "VacuumDegree", "0"));
                    deviceParams.IsAdjustRate = bool.Parse(ExistNodeForValue(mode, "IsAdjustRate", false.ToString()));
                    deviceParams.MinRate = int.Parse(ExistNodeForValue(mode, "MinRate", "1000"));
                    deviceParams.MaxRate = int.Parse(ExistNodeForValue(mode, "MaxRate", "4000"));
                    deviceParams.BeginChann = int.Parse(ExistNodeForValue(mode, "BeginChann", "50"));
                    deviceParams.EndChann = int.Parse(ExistNodeForValue(mode, "EndChann", ((int)device.SpecLength - 50).ToString()));
                    deviceParams.IsDistrubAlert = bool.Parse(ExistNodeForValue(mode, "IsDistrubAlert", false.ToString()));
                    deviceParams.IsPeakFloat = bool.Parse(ExistNodeForValue(mode, "IsPeakFloat", false.ToString()));
                    deviceParams.PeakFloatLeft = int.Parse(ExistNodeForValue(mode, "PeakFloatLeft", "0"));
                    deviceParams.PeakFloatRight = int.Parse(ExistNodeForValue(mode, "PeakFloatRight", "0"));
                    deviceParams.PeakFloatChannel = int.Parse(ExistNodeForValue(mode, "PeakFloatChannel", "0"));
                    deviceParams.PeakCheckTime = int.Parse(ExistNodeForValue(mode, "PeakCheckTime", "15"));
                    deviceParams.PeakFloatError = int.Parse(ExistNodeForValue(mode, "PeakFloatError", "0"));
                    deviceParams.CurrentRate = int.Parse(ExistNodeForValue(mode, "CurrentRate", "1"));
                    deviceParams.TargetIdx = int.Parse(ExistNodeForValue(mode, "TargetIdx", "1"));
                    deviceParams.TargetMode = (TargetMode)Enum.Parse(typeof(TargetMode), ExistNodeForValue(mode, "TargetMode", "OneTarget"));
                    condition.DeviceParamList.Insert(condition.DeviceParamList.Count, deviceParams);
                    listDeviceParams = xmlDoc.SelectNodes("CurveInfo/DemarcateEnergyList/DemarcateEnergy");
                    foreach (XmlNode energyNode in listDeviceParams)
                    {
                        DemarcateEnergy newEnergy = DemarcateEnergy.New;
                        newEnergy.ElementName = ExistNodeForValue(energyNode, "ElementName", string.Empty);
                        newEnergy.Channel = double.Parse(ExistNodeForValue(energyNode, "Channel", "0"));
                        newEnergy.Energy = double.Parse(ExistNodeForValue(energyNode, "Energy", "0"));
                        newEnergy.Line = (XLine)Enum.Parse(typeof(XLine), ExistNodeForValue(energyNode, "Line", XLine.Ka.ToString()));
                        condition.DemarcateEnergys.Add(newEnergy);
                    }
                }
                condition.InitParam = initParams;
                condition.Device = device;
                condition.Save();
            }
            curve.Condition = condition;
            curve.ConditionName = curve.Condition.Name;

            XmlNode selectElementList = xmlDoc.SelectSingleNode("CurveInfo/ElementList");

            ElementList elementList = ElementList.New;
            elementList.IsUnitary = bool.Parse(ExistNodeForValue(selectElementList, "IsUnitary", false.ToString()));
            elementList.UnitaryValue = double.Parse(ExistNodeForValue(selectElementList, "UnitaryValue", "99.96"));
            elementList.TubeWindowThickness = double.Parse(ExistNodeForValue(selectElementList, "TubeWindowThickness", "0"));
            elementList.RhIsLayer = bool.Parse(ExistNodeForValue(selectElementList, "RhIsLayer", false.ToString()));
            elementList.RhLayerFactor = double.Parse(ExistNodeForValue(selectElementList, "RhLayerFactor", "0"));
            elementList.LayerElemsInAnalyzer = ExistNodeForValue(selectElementList, "LayerElemsInAnalyzer", "Rh");
            elementList.IsAbsorption = bool.Parse(ExistNodeForValue(selectElementList, "IsAbsorption", false.ToString()));
            elementList.ThCalculationWay = (ThCalculationWay)Enum.Parse(typeof(ThCalculationWay), ExistNodeForValue(selectElementList, "ThCalculationWay", ThCalculationWay.ThInsert.ToString()));
            elementList.DBlLimt = double.Parse(ExistNodeForValue(selectElementList, "DBlLimt", "0.99999"));
            elementList.IsRemoveBk = bool.Parse(ExistNodeForValue(selectElementList, "IsRemoveBk", false.ToString()));
            //elementList.SpecListId = long.Parse(ExistNodeForValue(selectElementList, "SpecListId", "0"));
            elementList.IsReportCategory = bool.Parse(ExistNodeForValue(selectElementList, "IsReportCategory", false.ToString()));
            elementList.PureAsInfinite = bool.Parse(ExistNodeForValue(selectElementList, "PureAsInfinite", false.ToString()));
            elementList.MatchSpecListIdStr = ExistNodeForValue(selectElementList, "MatchSpecListIdStr", string.Empty);
            elementList.MainElementToCalcKarat = ExistNodeForValue(selectElementList, "MainElementToCalcKarat", "Au");
            elementList.LayerElemsInAnalyzer = ExistNodeForValue(selectElementList, "LayerElemsInAnalyzer", "Rh");
            elementList.RefSpecListIdStr = "";

            elementList.Items.Clear();
            #region  元素
            XmlNodeList curveElementList = selectElementList.SelectNodes("CurveElement");
            foreach (XmlNode curveElementNode in curveElementList)
            {
                CurveElement curveElement = CurveElement.New;
                if (curve.Condition == null)
                    continue;
                curveElement.Caption = ExistNodeForValue(curveElementNode, "Caption", string.Empty);
                curveElement.RowsIndex = int.Parse(ExistNodeForValue(curveElementNode, "RowIndex", "0"));
                curveElement.IsDisplay = bool.Parse(ExistNodeForValue(curveElementNode, "IsDisplay", false.ToString()));
                curveElement.Formula = ExistNodeForValue(curveElementNode, "Formula", string.Empty);
                curveElement.AtomicNumber = int.Parse(ExistNodeForValue(curveElementNode, "AtomicNumber", "1"));
                curveElement.LayerNumber = int.Parse(ExistNodeForValue(curveElementNode, "LayerNumber", "0"));
                curveElement.LayerNumBackUp = ExistNodeForValue(curveElementNode, "LayerNumBackUp", "0");
                curveElement.AnalyteLine = (XLine)Enum.Parse(typeof(XLine), ExistNodeForValue(curveElementNode, "AnalyteLine", XLine.Ka.ToString()));
                curveElement.PeakLow = int.Parse(ExistNodeForValue(curveElementNode, "PeakLow", "0"));
                curveElement.PeakHigh = int.Parse(ExistNodeForValue(curveElementNode, "PeakHigh", "0"));
                if (curve.Condition.DeviceParamList.Count > 0)
                {
                    bool falg = false;
                    if (curveElementNode.SelectSingleNode("DeviceParamsName") != null)
                    {
                        DeviceParameter tempFind = curve.Condition.DeviceParamList.ToList().Find(delegate(DeviceParameter w) { return w.Name == curveElementNode.SelectSingleNode("DeviceParamsName").InnerText.ToString(); });
                        if (tempFind != null)
                        {
                            falg = true;
                            curveElement.DevParamId = tempFind.Id;
                        }
                    }
                    if (!falg)
                    {
                        DeviceParameter findParams = curve.Condition.DeviceParamList.ToList().Find(delegate(DeviceParameter w) { return curveElement.PeakLow >= w.BeginChann && curveElement.PeakHigh <= w.EndChann; });
                        if (findParams != null)
                            curveElement.DevParamId = findParams.Id;
                        else
                            curveElement.DevParamId = curve.Condition.DeviceParamList[0].Id;
                    }
                }
                curveElement.BaseLow = int.Parse(ExistNodeForValue(curveElementNode, "BaseLow", "0"));
                curveElement.BaseHigh = int.Parse(ExistNodeForValue(curveElementNode, "BaseHigh", "0"));
                curveElement.PeakDivBase = bool.Parse(ExistNodeForValue(curveElementNode, "PeakDivBase", false.ToString()));
                curveElement.LayerDensity = double.Parse(ExistNodeForValue(curveElementNode, "LayerDensity", "0"));
                curveElement.IntensityWay = (IntensityWay)Enum.Parse(typeof(IntensityWay), ExistNodeForValue(curveElementNode, "IntensityWay", IntensityWay.FullArea.ToString()));
                curveElement.IntensityComparison = bool.Parse(ExistNodeForValue(curveElementNode, "IntensityComparison", false.ToString()));
                curveElement.ComparisonCoefficient = int.Parse(ExistNodeForValue(curveElementNode, "ComparisonCoefficient", "1"));
                curveElement.BPeakLow = int.Parse(ExistNodeForValue(curveElementNode, "BPeakLow", "0"));
                curveElement.BPeakHigh = int.Parse(ExistNodeForValue(curveElementNode, "BPeakHigh", "0"));
                curveElement.CalculationWay = (CalculationWay)Enum.Parse(typeof(CalculationWay), ExistNodeForValue(curveElementNode, "CalculationWay", CalculationWay.Insert.ToString()));
                curveElement.FpCalculationWay = (FpCalculationWay)Enum.Parse(typeof(FpCalculationWay), ExistNodeForValue(curveElementNode, "FpCalculationWay", FpCalculationWay.LinearWithAnIntercept.ToString()));
                curveElement.Flag = (ElementFlag)Enum.Parse(typeof(ElementFlag), ExistNodeForValue(curveElementNode, "Flag", ElementFlag.Added.ToString()));
                curveElement.LayerFlag = (LayerFlag)Enum.Parse(typeof(LayerFlag), ExistNodeForValue(curveElementNode, "LayerFlag", LayerFlag.Calculated.ToString()));
                //curveElement. = (ElementFlag)Enum.Parse(typeof(ElementFlag), ExistNodeForValue(curveElementNode,"Flag").InnerText.ToString());
                //curveElement.LayerFlag = (LayerFlag)Enum.Parse(typeof(LayerFlag), ExistNodeForValue(curveElementNode,"LayerFlag").InnerText.ToString());
                // add by chuyaqin 2011-04-28
                curveElement.IsShowElement = true;
                curveElement.BaseIntensityWay = BaseIntensityWay.FullArea;
                curveElement.ContentUnit = (ContentUnit)Enum.Parse(typeof(ContentUnit), ExistNodeForValue(curveElementNode, "ContentUnit", ContentUnit.per.ToString()));
                curveElement.ThicknessUnit = (ThicknessUnit)Enum.Parse(typeof(ThicknessUnit), ExistNodeForValue(curveElementNode, "ThicknessUnit", ThicknessUnit.um.ToString()));
                curveElement.SReferenceElements = ExistNodeForValue(curveElementNode, "SReferenceElements", string.Empty);
                curveElement.SSpectrumData = ExistNodeForValue(curveElementNode, "SSpectrumData", string.Empty);
                curveElement.SInfluenceElements = ExistNodeForValue(curveElementNode, "SInfluenceElements", string.Empty);
                curveElement.DistrubThreshold = ExistNodeForValue(curveElementNode, "DistrubThreshold", string.Empty);
                curveElement.IsInfluence = bool.Parse(ExistNodeForValue(curveElementNode, "IsInfluence", false.ToString()));
                curveElement.SInfluenceCoefficients = ExistNodeForValue(curveElementNode, "SInfluenceCoefficients", string.Empty);
                curveElement.ElementSpecName = ExistNodeForValue(curveElementNode, "ElementSpecName", string.Empty);
                curveElement.Contentcoeff = double.Parse(ExistNodeForValue(curveElementNode, "Contentcoeff", "1.0"));
                icolor++;
                curveElement.Color = (icolor > colorlist.Length) ? colorlist[1].ToArgb() : colorlist[icolor].ToArgb();
                curveElement.ColorHelper = ExistNodeForValue(curveElementNode, "ColorHelper", string.Empty);
                curveElement.IsOxide = bool.Parse(ExistNodeForValue(curveElementNode, "IsOxide", "false"));
                curveElement.ElementEncoderSpecName = ExistNodeForValue(curveElementNode, "ElementEncoderSpecName", string.Empty);
                curveElement.IsBorderlineElem = bool.Parse(ExistNodeForValue(curveElementNode, "IsBorderlineElem", "false"));
                curveElement.ElementSpecNameNoFilter = ExistNodeForValue(curveElementNode, "ElementSpecNameNoFilter", string.Empty);
                curveElement.SSpectrumDataNotFilter = ExistNodeForValue(curveElementNode, "SSpectrumDataNotFilter", string.Empty);
                curveElement.IsShowDefineName = bool.Parse(ExistNodeForValue(curveElementNode, "IsShowDefineName", "false"));
                curveElement.DefineElemName = ExistNodeForValue(curveElementNode, "DefineElemName", curveElement.Caption);

                #region 标样
                XmlNodeList standSampleList = curveElementNode.SelectNodes("StandSample");
                if (standSampleList != null)
                {
                    foreach (XmlNode standSampleNode in standSampleList)
                    {
                        StandSample standSample = StandSample.New;
                        standSample.SampleName = ExistNodeForValue(standSampleNode, "SampleName", string.Empty);
                        standSample.Height = ExistNodeForValue(standSampleNode, "Height", "0");
                        standSample.CalcAngleHeight = ExistNodeForValue(standSampleNode, "CalcAngleHeight", "0");
                        standSample.X = ExistNodeForValue(standSampleNode, "X", "0");
                        standSample.Y = ExistNodeForValue(standSampleNode, "Y", "0");
                        standSample.Z = ExistNodeForValue(standSampleNode, "Z", "0");
                        standSample.TheoryX = double.Parse(ExistNodeForValue(standSampleNode, "TheoryX", "0"));
                        standSample.Active = bool.Parse(ExistNodeForValue(standSampleNode, "Active", false.ToString()));
                        standSample.ElementName = ExistNodeForValue(standSampleNode, "ElementName", string.Empty);
                        //standSample.SpecListId = long.Parse(ExistNodeForValue(standSampleNode, "SpecListId", "0"));
                        standSample.Layer = int.Parse(ExistNodeForValue(standSampleNode, "Layer", "0"));
                        standSample.Level = ExistNodeForValue(standSampleNode, "Level", "");
                        //Encoding utf8 = Encoding.UTF8;
                        //Encoding gb2312 = Encoding.GetEncoding("gb2312");
                        //if (standSampleNode.SelectSingleNode("Level") != null)
                        //{
                        //    string str = standSampleNode.SelectSingleNode("Level").InnerText;
                        //    byte[] temp = utf8.GetBytes(str);
                        //    byte[] temp1 = Encoding.Convert(utf8, gb2312, temp);
                        //    standSample.Level = gb2312.GetString(temp1);
                        //}
                        //else
                        //standSample.Level = "单层";
                        standSample.TotalLayer = int.Parse(ExistNodeForValue(standSampleNode, "TotalLayer", "0"));
                        standSample.IsMatch = bool.Parse(ExistNodeForValue(standSampleNode, "IsMatch", "0"));
                        //standSample.MatchSpecListId = int.Parse(ExistNodeForValue(standSampleNode, "MatchSpecListId", "0"));
                        standSample.MatchSpecName = ExistNodeForValue(standSampleNode, "MatchSpecName", string.Empty);
                        standSample.Uncertainty = ExistNodeForValue(standSampleNode, "Uncertainty", "0");
                        standSample.Density = double.Parse(ExistNodeForValue(standSampleNode, "Density", "0"));
                        curveElement.Samples.Add(standSample);
                    }
                }
                #endregion

                #region 拟合元素
                XmlNodeList ReferenceElementList = curveElementNode.SelectNodes("ReferenceElement");
                if (ReferenceElementList != null)
                {
                    foreach (XmlNode ReferenceElementNode in ReferenceElementList)
                    {
                        ReferenceElement referenceElement = ReferenceElement.New;
                        referenceElement.MainElementName = ExistNodeForValue(ReferenceElementNode, "MainElementName", string.Empty);
                        referenceElement.ReferenceElementName = ExistNodeForValue(ReferenceElementNode, "ReferenceElementName", string.Empty);
                        referenceElement.ReferenceLeftBorder = int.Parse(ExistNodeForValue(ReferenceElementNode, "ReferenceLeftBorder", "0"));
                        referenceElement.ReferenceRightBorder = int.Parse(ExistNodeForValue(ReferenceElementNode, "ReferenceRightBorder", "0"));
                        referenceElement.ReferenceBackLeft = int.Parse(ExistNodeForValue(ReferenceElementNode, "ReferenceBackLeft", "0"));
                        referenceElement.ReferenceBackRight = int.Parse(ExistNodeForValue(ReferenceElementNode, "ReferenceBackRight", "0"));
                        referenceElement.PeakToBack = bool.Parse(ExistNodeForValue(ReferenceElementNode, "PeakToBack", false.ToString()));
                        curveElement.References.Add(referenceElement);
                    }
                }
                #endregion

                #region 元素对应的优化因子
                XmlNodeList OptimizationList = curveElementNode.SelectNodes("Optimiztion");
                if (OptimizationList != null)
                {
                    foreach (XmlNode OptimizationNode in OptimizationList)
                    {
                        Optimiztion optimize = Optimiztion.New;
                        optimize.OptimiztionValue = double.Parse(ExistNodeForValue(OptimizationNode, "OptimiztionValue", "0"));
                        optimize.OptimiztionRange = double.Parse(ExistNodeForValue(OptimizationNode, "OptimiztionRange", "0"));
                        optimize.OptimiztionMax = double.Parse(ExistNodeForValue(OptimizationNode, "OptimiztionMax", "0"));
                        optimize.OptimiztionMin = double.Parse(ExistNodeForValue(OptimizationNode, "OptimiztionMin", "0"));
                        optimize.OptimiztionFactor = double.Parse(ExistNodeForValue(OptimizationNode, "OptimiztionFactor", "0"));
                        optimize.OptExpression = ExistNodeForValue(OptimizationNode, "OptExpression", "x");
                        optimize.IsJoinIntensity = bool.Parse(ExistNodeForValue(OptimizationNode, "IsJoinIntensity", "1"));
                        optimize.OptimizetionType = int.Parse(ExistNodeForValue(OptimizationNode, "OptimizetionType", "0"));
                        curveElement.Optimiztion.Add(optimize);

                    }
                }
                #endregion

                #region 元素对应的影响元素
                XmlNodeList ElementRefList = curveElementNode.SelectNodes("ElementRefs");
                if (ElementRefList != null)
                {
                    foreach (XmlNode ElementRdfNode in ElementRefList)
                    {
                        string influnceName = ExistNodeForValue(ElementRdfNode, "Name", string.Empty);
                        ElementRef elementRef = ElementRef.New;
                        elementRef.Name = influnceName;
                        elementRef.IsRef = bool.Parse(ExistNodeForValue(ElementRdfNode, "IsRef", false.ToString()));
                        elementRef.RefConf = double.Parse(ExistNodeForValue(ElementRdfNode, "RefConf", "0"));
                        elementRef.DistrubThreshold = double.Parse(ExistNodeForValue(ElementRdfNode, "DistrubThreshold", "0"));
                        curveElement.ElementRefs.Add(elementRef);
                    }
                }
                #endregion
                elementList.Items.Add(curveElement);
            }
            #endregion

            #region 元素自定义
            XmlNodeList CustomList = selectElementList.SelectNodes("CustomFields");
            if (CustomList != null)
            {
                foreach (XmlNode custom in CustomList)
                {
                    CustomField cf = CustomField.New;
                    cf.Expression = custom.SelectSingleNode("Expression").InnerText;
                    cf.Name = custom.SelectSingleNode("Name").InnerText;
                    elementList.CustomFields.Add(cf);
                }
            }
            #endregion
            curve.ElementList = elementList;

            #region  曲线下的校正强度数据
            List<IntensityCalibration> listItensitycal = new List<IntensityCalibration>();
            listItensitycal = curve.IntensityCalibration.ToList();
            curve.IntensityCalibration.Clear();
            XmlNodeList IntensityCallist = xmlDoc.SelectNodes("IntensityCalibration");
            if (IntensityCallist != null)
            {
                foreach (XmlNode custom in IntensityCallist)
                {
                    IntensityCalibration cf = IntensityCalibration.New;
                    cf.CalibrateIn = double.Parse(custom.SelectSingleNode("CalibrateIn").InnerText);
                    cf.Element = custom.SelectSingleNode("Element").InnerText;
                    cf.OriginalIn = double.Parse(custom.SelectSingleNode("OriginalIn").InnerText);
                    cf.CalibrateBaseIn = double.Parse(ExistNodeForValue(custom, "CalibrateBaseIn", "0"));
                    cf.OriginalBaseIn = double.Parse(ExistNodeForValue(custom, "OriginalBaseIn", "0"));
                    cf.PeakLeft = int.Parse(ExistNodeForValue(custom, "PeakLeft", "0"));
                    cf.PeakRight = int.Parse(ExistNodeForValue(custom, "PeakRight", "0"));
                    curve.IntensityCalibration.Add(cf);
                }
            }
            #endregion

            #region 工作区
            XmlNode WorkRegionNode = xmlDoc.SelectSingleNode("CurveInfo/WorkRegion");
            if (WorkRegionNode != null && curve != null)
            {
                WorkRegion temp = WorkRegion.FindOne(w => w.Name == WorkRegionNode.SelectSingleNode("Name").InnerText);
                if (temp != null)
                    curve.WorkRegion = temp;
                else
                {
                    WorkRegion rgion = WorkRegion.New;
                    rgion.Name = WorkRegionNode.SelectSingleNode("Name").InnerText;
                    rgion.RohsSampleType = (RohsSampleType)Enum.Parse(typeof(RohsSampleType), WorkRegionNode.SelectSingleNode("RohsSampleType").InnerText);
                    rgion.IsDefaultWorkRegion = bool.Parse(WorkRegionNode.SelectSingleNode("IsDefaultWorkRegion").InnerText);
                    curve.WorkRegion = rgion;
                }
            }

            #endregion
            curve.Save();

        }

        //public bool CurveImport(string path,string UsbType)
        //{
        //    XmlDocument xmlDoc = new XmlDocument();
        //    xmlDoc.Load(path);
        //    #region 更新设备
        //    string DllType = (xmlDoc.SelectSingleNode("CurveInfo/WorkCurve/DllType") == null) ? "" : xmlDoc.SelectSingleNode("CurveInfo/WorkCurve/DllType").InnerText;
        //    string strPortType = "";
        //    string strUsbType = "";
        //    if (UsbType != "" && UsbType == "usb1") strUsbType = "0 ";
        //    else if (UsbType != "" && UsbType == "usb2") strUsbType = "1 ";
        //    if (DllType == "dll3") strPortType = "0"; else if (DllType == "dll4") strPortType = "1";
        //    if (strPortType != "") Device.FindBySql("update device set PortType=" + strPortType + " where IsDefaultDevice=1");
        //    if (strUsbType != "") Device.FindBySql("update device set UsbVersion=" + strUsbType + " where IsDefaultDevice=1");
        //    #endregion
        //    Device device = Device.FindOne(w => w.IsDefaultDevice == true);
        //    CurveImport(path,device,true,true,string.Empty,string.Empty);
        //    #region
        //    //try
        //    //{
        //    //    curve = WorkCurve.New;
        //    //    if (xmlDoc.SelectSingleNode("CurveInfo/WorkCurve/Name")!=null)
        //    //    curve.Name = xmlDoc.SelectSingleNode("CurveInfo/WorkCurve/Name").InnerText;
        //    //    //curve.Name = _cureName;
        //    //    if (xmlDoc.SelectSingleNode("CurveInfo/WorkCurve/CalcType")!=null)
        //    //    curve.CalcType = (CalcType)Enum.Parse(typeof(CalcType), xmlDoc.SelectSingleNode("CurveInfo/WorkCurve/CalcType").InnerText);
        //    //    if (xmlDoc.SelectSingleNode("CurveInfo/WorkCurve/FuncType")!=null)
        //    //    curve.FuncType = (FuncType)Enum.Parse(typeof(FuncType), xmlDoc.SelectSingleNode("CurveInfo/WorkCurve/FuncType").InnerText);
        //    //    if (xmlDoc.SelectSingleNode("CurveInfo/WorkCurve/IsAbsorb")!=null)
        //    //    curve.IsAbsorb = bool.Parse(xmlDoc.SelectSingleNode("CurveInfo/WorkCurve/IsAbsorb").InnerText);
        //    //    if (xmlDoc.SelectSingleNode("CurveInfo/WorkCurve/IsThickShowContent")!=null)
        //    //    curve.IsThickShowContent = bool.Parse(xmlDoc.SelectSingleNode("CurveInfo/WorkCurve/IsThickShowContent").InnerText);
        //    //    if (xmlDoc.SelectSingleNode("CurveInfo/WorkCurve/LimitThickness")!=null)
        //    //    curve.LimitThickness = int.Parse(xmlDoc.SelectSingleNode("CurveInfo/WorkCurve/LimitThickness").InnerText);
        //    //    if (xmlDoc.SelectSingleNode("CurveInfo/WorkCurve/RemoveBackGround")!=null)
        //    //    curve.RemoveBackGround = bool.Parse(xmlDoc.SelectSingleNode("CurveInfo/WorkCurve/RemoveBackGround").InnerText);
        //    //    if (xmlDoc.SelectSingleNode("CurveInfo/WorkCurve/RemoveSum")!=null)
        //    //    curve.RemoveSum = bool.Parse(xmlDoc.SelectSingleNode("CurveInfo/WorkCurve/RemoveSum").InnerText);
        //    //    if (xmlDoc.SelectSingleNode("CurveInfo/WorkCurve/RemoveEscape")!=null)
        //    //    curve.RemoveEscape = bool.Parse(xmlDoc.SelectSingleNode("CurveInfo/WorkCurve/RemoveEscape").InnerText);
        //    //    //curve.IsDefaultWorkCurve = bool.Parse(xmlDoc.SelectSingleNode("CurveInfo/WorkCurve/IsDefaultWorkCurve").InnerText);
        //    //    curve.IsDefaultWorkCurve = curve.IsDefaultWorkCurve || false;
        //    //    if (xmlDoc.SelectSingleNode("CurveInfo/WorkCurve/IsJoinMatch")!=null)
        //    //    curve.IsJoinMatch = bool.Parse(xmlDoc.SelectSingleNode("CurveInfo/WorkCurve/IsJoinMatch").InnerText);
        //    //    //Condition condition = null;
        //    //    List<CurveElement> lstElements = new List<CurveElement>();
        //    //    List<CustomField> listCustoms = new List<CustomField>();

        //    //    InitParameter initParams = null;
        //    //    initParams = InitParameter.New;
        //    //    this.ccondition.Name = (xmlDoc.SelectSingleNode("CurveInfo/Condition/Name")==null)?"":xmlDoc.SelectSingleNode("CurveInfo/Condition/Name").InnerText;
        //    //    //condition.Name = _conditionNme;
        //    //    //}
        //    //    this.ccondition.Type = ConditionType.Normal; //(ConditionType)Enum.Parse(typeof(ConditionType), xmlDoc.SelectSingleNode("CurveInfo/Condition/Type").InnerText);
        //    //    if (xmlDoc.SelectSingleNode("CurveInfo/Condition/TubCurrent")!=null)
        //    //    initParams.TubCurrent = int.Parse(xmlDoc.SelectSingleNode("CurveInfo/Condition/TubCurrent").InnerText);
        //    //    if (xmlDoc.SelectSingleNode("CurveInfo/Condition/TubVoltage")!=null)
        //    //    initParams.TubVoltage = int.Parse(xmlDoc.SelectSingleNode("CurveInfo/Condition/TubVoltage").InnerText);
        //    //    if (xmlDoc.SelectSingleNode("CurveInfo/Condition/Gain")!=null)
        //    //    initParams.Gain = float.Parse(xmlDoc.SelectSingleNode("CurveInfo/Condition/Gain").InnerText.ToString());
        //    //    if (xmlDoc.SelectSingleNode("CurveInfo/Condition/FineGain")!=null)
        //    //    initParams.FineGain = float.Parse(xmlDoc.SelectSingleNode("CurveInfo/Condition/FineGain").InnerText);
        //    //    if (xmlDoc.SelectSingleNode("CurveInfo/Condition/Channel")!=null)
        //    //    initParams.Channel = int.Parse(xmlDoc.SelectSingleNode("CurveInfo/Condition/Channel").InnerText.ToString());
        //    //    if (xmlDoc.SelectSingleNode("CurveInfo/Condition/ChannelError")!=null)
        //    //    initParams.ChannelError = int.Parse(xmlDoc.SelectSingleNode("CurveInfo/Condition/ChannelError").InnerText);
        //    //    if (xmlDoc.SelectSingleNode("CurveInfo/Condition/Filter")!=null)
        //    //    initParams.Filter = int.Parse(xmlDoc.SelectSingleNode("CurveInfo/Condition/Filter").InnerText);
        //    //    if (xmlDoc.SelectSingleNode("CurveInfo/Condition/Collimator")!=null)
        //    //    initParams.Collimator = int.Parse(xmlDoc.SelectSingleNode("CurveInfo/Condition/Collimator").InnerText);
        //    //    if (xmlDoc.SelectSingleNode("CurveInfo/Condition/ElemName")!=null)
        //    //    initParams.ElemName = xmlDoc.SelectSingleNode("CurveInfo/Condition/ElemName").InnerText;
        //    //    initParams.CurrentRate = 1;
        //    //    this.ccondition.InitParam = initParams;


        //    //    //this.ccondition.Device = device;
        //    //    //删除
        //    //    //condition.DeviceParamList.Clear();
        //    //    XmlNodeList listDeviceParams = xmlDoc.SelectNodes("CurveInfo/DeviceParamList/DeviceParameter");
        //    //    foreach (XmlNode mode in listDeviceParams)
        //    //    {
        //    //        string ConditionName = (mode.SelectSingleNode("Name")==null)?"":mode.SelectSingleNode("Name").InnerText;
        //    //        DeviceParameter deviceParams = null;
        //    //            #region 修改
        //    //            deviceParams = this.ccondition.DeviceParamList[0];
        //    //            deviceParams.Name = ConditionName;
        //    //            if (mode.SelectSingleNode("PrecTime")!=null)
        //    //            deviceParams.PrecTime = int.Parse(mode.SelectSingleNode("PrecTime").InnerText);
        //    //            if (mode.SelectSingleNode("TubCurrent")!=null)
        //    //            deviceParams.TubCurrent = int.Parse(mode.SelectSingleNode("TubCurrent").InnerText);
        //    //            if (mode.SelectSingleNode("TubVoltage") != null)
        //    //            deviceParams.TubVoltage = int.Parse(mode.SelectSingleNode("TubVoltage").InnerText);
        //    //            if (mode.SelectSingleNode("Filter") != null)
        //    //            deviceParams.FilterIdx = int.Parse(mode.SelectSingleNode("Filter").InnerText);
        //    //            if (mode.SelectSingleNode("Collimator") != null)
        //    //            deviceParams.CollimatorIdx = int.Parse(mode.SelectSingleNode("Collimator").InnerText);
        //    //            if (mode.SelectSingleNode("IsVacuum") != null)
        //    //            deviceParams.IsVacuum = bool.Parse(mode.SelectSingleNode("IsVacuum").InnerText);
        //    //            if (mode.SelectSingleNode("VacuumTime") != null)
        //    //            deviceParams.VacuumTime = int.Parse(mode.SelectSingleNode("VacuumTime").InnerText);
        //    //            if (mode.SelectSingleNode("IsVacuumDegree") != null)
        //    //            deviceParams.IsVacuumDegree = bool.Parse(mode.SelectSingleNode("IsVacuumDegree").InnerText);
        //    //            if (mode.SelectSingleNode("VacuumDegree") != null)
        //    //            deviceParams.VacuumDegree = double.Parse(mode.SelectSingleNode("VacuumDegree").InnerText);
        //    //            if (mode.SelectSingleNode("IsAdjustRate") != null)
        //    //            deviceParams.IsAdjustRate = bool.Parse(mode.SelectSingleNode("IsAdjustRate").InnerText);
        //    //            if (mode.SelectSingleNode("MinRate") != null)
        //    //            deviceParams.MinRate = int.Parse(mode.SelectSingleNode("MinRate").InnerText);
        //    //            if (mode.SelectSingleNode("MaxRate") != null)
        //    //            deviceParams.MaxRate = int.Parse(mode.SelectSingleNode("MaxRate").InnerText);
        //    //            if (mode.SelectSingleNode("BeginChann") != null)
        //    //            deviceParams.BeginChann = int.Parse(mode.SelectSingleNode("BeginChann").InnerText);
        //    //            if (mode.SelectSingleNode("EndChann") != null)
        //    //            deviceParams.EndChann = int.Parse(mode.SelectSingleNode("EndChann").InnerText);
        //    //            if (mode.SelectSingleNode("IsDistrubAlert") != null)
        //    //            deviceParams.IsDistrubAlert = bool.Parse(mode.SelectSingleNode("IsDistrubAlert").InnerText);
        //    //            if (mode.SelectSingleNode("IsPeakFloat") != null)
        //    //            deviceParams.IsPeakFloat = bool.Parse(mode.SelectSingleNode("IsPeakFloat").InnerText);
        //    //            if (mode.SelectSingleNode("PeakFloatLeft") != null)
        //    //            deviceParams.PeakFloatLeft = int.Parse(mode.SelectSingleNode("PeakFloatLeft").InnerText);
        //    //            if (mode.SelectSingleNode("PeakFloatRight") != null)
        //    //            deviceParams.PeakFloatRight = int.Parse(mode.SelectSingleNode("PeakFloatRight").InnerText);
        //    //            if (mode.SelectSingleNode("PeakFloatChannel") != null)
        //    //            deviceParams.PeakFloatChannel = int.Parse(mode.SelectSingleNode("PeakFloatChannel").InnerText);
        //    //            if (mode.SelectSingleNode("PeakCheckTime") != null)
        //    //            deviceParams.PeakCheckTime = int.Parse(mode.SelectSingleNode("PeakCheckTime").InnerText);
        //    //            if (mode.SelectSingleNode("PeakFloatError") != null)
        //    //            deviceParams.PeakFloatError = int.Parse(mode.SelectSingleNode("PeakFloatError").InnerText);
        //    //            deviceParams.CurrentRate = 1;
        //    //            #endregion

        //    //    }
        //    //    listDeviceParams = xmlDoc.SelectNodes("CurveInfo/DemarcateEnergyList/DemarcateEnergy");
        //    //    foreach (XmlNode energyNode in listDeviceParams)
        //    //    {
        //    //        string elementName = (energyNode.SelectSingleNode("ElementName")==null)?"":energyNode.SelectSingleNode("ElementName").InnerText.ToString();

        //    //        //if (condition.DemarcateEnergys.Count>0 && condition.DemarcateEnergys.First(wde => wde.ElementName.Equals(elementName)) != null)
        //    //        if (this.ccondition.DemarcateEnergys.Count > 0 && this.ccondition.DemarcateEnergys.ToList().Exists(delegate(DemarcateEnergy v) { return v.ElementName == elementName; }))
        //    //        {
        //    //            continue;
        //    //        }
        //    //        DemarcateEnergy newEnergy = DemarcateEnergy.New;
        //    //        if (energyNode.SelectSingleNode("ElementName") != null)
        //    //        newEnergy.ElementName = energyNode.SelectSingleNode("ElementName").InnerText.ToString();
        //    //        if (energyNode.SelectSingleNode("Channel") != null)
        //    //        newEnergy.Channel = int.Parse(energyNode.SelectSingleNode("Channel").InnerText.ToString());
        //    //        if (energyNode.SelectSingleNode("Energy") != null)
        //    //        newEnergy.Energy = double.Parse(energyNode.SelectSingleNode("Energy").InnerText.ToString());
        //    //        if (energyNode.SelectSingleNode("Line") != null)
        //    //        newEnergy.Line = (XLine)Enum.Parse(typeof(XLine), energyNode.SelectSingleNode("Line").InnerText.ToString());
        //    //        this.ccondition.DemarcateEnergys.Add(newEnergy);
        //    //    }
        //    //    curve.Condition = this.ccondition;
        //    //    curve.ConditionName = curve.Condition.Name;
        //    //        this.ccondition.Save();


        //    //        //修改动态库 //修改Usb接口





        //    //    curve.ConditionName = curve.Condition.Name;
        //    //    XmlNode selectElementList = xmlDoc.SelectSingleNode("CurveInfo/ElementList");


        //    //    if (selectElementList != null)
        //    //    {
        //    //        ElementList elementList;
        //    //        elementList = ElementList.New;
        //    //        if (selectElementList.SelectSingleNode("IsUnitary").InnerText != null)
        //    //        elementList.IsUnitary = bool.Parse(selectElementList.SelectSingleNode("IsUnitary").InnerText.ToString());
        //    //        if (selectElementList.SelectSingleNode("UnitaryValue").InnerText != null)
        //    //        elementList.UnitaryValue = double.Parse(selectElementList.SelectSingleNode("UnitaryValue").InnerText.ToString());
        //    //        if (selectElementList.SelectSingleNode("TubeWindowThickness").InnerText != null)
        //    //        elementList.TubeWindowThickness = double.Parse(selectElementList.SelectSingleNode("TubeWindowThickness").InnerText.ToString());
        //    //        if (selectElementList.SelectSingleNode("RhIsLayer").InnerText != null)
        //    //        elementList.RhIsLayer = bool.Parse(selectElementList.SelectSingleNode("RhIsLayer").InnerText.ToString());
        //    //        if (selectElementList.SelectSingleNode("RhLayerFactor").InnerText != null)
        //    //        elementList.RhLayerFactor = double.Parse(selectElementList.SelectSingleNode("RhLayerFactor").InnerText.ToString());
        //    //        if (selectElementList.SelectSingleNode("IsAbsorption").InnerText != null)
        //    //        elementList.IsAbsorption = bool.Parse(selectElementList.SelectSingleNode("IsAbsorption").InnerText.ToString());
        //    //        if (selectElementList.SelectSingleNode("ThCalculationWay").InnerText != null)
        //    //        elementList.ThCalculationWay = (ThCalculationWay)Enum.Parse(typeof(ThCalculationWay), selectElementList.SelectSingleNode("ThCalculationWay").InnerText.ToString());
        //    //        if (selectElementList.SelectSingleNode("DBlLimt").InnerText != null)
        //    //        elementList.DBlLimt = double.Parse(selectElementList.SelectSingleNode("DBlLimt").InnerText.ToString());
        //    //        if (!selectElementList.SelectSingleNode("IsRemoveBk").InnerText != null)
        //    //        elementList.IsRemoveBk = bool.Parse(selectElementList.SelectSingleNode("IsRemoveBk").InnerText.ToString());
        //    //        if (selectElementList.SelectSingleNode("SpecListId").InnerText != null)
        //    //        elementList.SpecListId = long.Parse(selectElementList.SelectSingleNode("SpecListId").InnerText.ToString());
        //    //        if (selectElementList.SelectSingleNode("IsReportCategory").InnerText != null)
        //    //        elementList.IsReportCategory = bool.Parse(selectElementList.SelectSingleNode("IsReportCategory").InnerText.ToString());
        //    //        if (selectElementList.SelectSingleNode("PureAsInfinite").InnerText != null)
        //    //        elementList.PureAsInfinite = bool.Parse(selectElementList.SelectSingleNode("PureAsInfinite").InnerText.ToString());
        //    //        //workCurveCurrent.IntensityCalibration.ToList();
        //    //        lstElements = elementList.Items.ToList();
        //    //        elementList.Items.Clear();

        //    //        #region  元素
        //    //        XmlNodeList curveElementList = selectElementList.SelectNodes("CurveElement");
        //    //        foreach (XmlNode curveElementNode in curveElementList)
        //    //        {
        //    //            CurveElement curveElement = CurveElement.New;
        //    //            if (curve.Condition == null)
        //    //                continue;
        //    //            curveElement.DevParamId = curve.Condition.DeviceParamList[0].Id;
        //    //            if (curveElementNode.SelectSingleNode("Caption") != null)
        //    //            curveElement.Caption = curveElementNode.SelectSingleNode("Caption").InnerText;
        //    //            if (curveElementNode.SelectSingleNode("IsDisplay") != null)
        //    //            curveElement.IsDisplay = bool.Parse(curveElementNode.SelectSingleNode("IsDisplay").InnerText.ToString());
        //    //            if (curveElementNode.SelectSingleNode("Formula") != null)
        //    //            curveElement.Formula = curveElementNode.SelectSingleNode("Formula").InnerText.ToString();
        //    //            if (curveElementNode.SelectSingleNode("AtomicNumber") != null)
        //    //            curveElement.AtomicNumber = int.Parse(curveElementNode.SelectSingleNode("AtomicNumber").InnerText.ToString());
        //    //            if (curveElementNode.SelectSingleNode("LayerNumber") != null)
        //    //            curveElement.LayerNumber = int.Parse(curveElementNode.SelectSingleNode("LayerNumber").InnerText.ToString());
        //    //            if (curveElementNode.SelectSingleNode("LayerNumBackUp") != null)
        //    //            curveElement.LayerNumBackUp = curveElementNode.SelectSingleNode("LayerNumBackUp").InnerText.ToString();
        //    //            if (curveElementNode.SelectSingleNode("AnalyteLine") != null)
        //    //            curveElement.AnalyteLine = (XLine)Enum.Parse(typeof(XLine), curveElementNode.SelectSingleNode("AnalyteLine").InnerText.ToString());
        //    //            if (curveElementNode.SelectSingleNode("PeakLow") != null)
        //    //            curveElement.PeakLow = int.Parse(curveElementNode.SelectSingleNode("PeakLow").InnerText.ToString());
        //    //            if (curveElementNode.SelectSingleNode("PeakHigh") != null)
        //    //            curveElement.PeakHigh = int.Parse(curveElementNode.SelectSingleNode("PeakHigh").InnerText.ToString());
        //    //            if (curveElementNode.SelectSingleNode("BaseLow") != null)
        //    //            curveElement.BaseLow = int.Parse(curveElementNode.SelectSingleNode("BaseLow").InnerText.ToString());
        //    //            if (curveElementNode.SelectSingleNode("BaseHigh") != null)
        //    //            curveElement.BaseHigh = int.Parse(curveElementNode.SelectSingleNode("BaseHigh").InnerText.ToString());
        //    //            if (curveElementNode.SelectSingleNode("PeakDivBase") != null)
        //    //            curveElement.PeakDivBase = bool.Parse(curveElementNode.SelectSingleNode("PeakDivBase").InnerText.ToString());
        //    //            if (curveElementNode.SelectSingleNode("LayerDensity") != null)
        //    //            curveElement.LayerDensity = int.Parse(curveElementNode.SelectSingleNode("LayerDensity").InnerText.ToString());
        //    //            if (curveElementNode.SelectSingleNode("IntensityWay") != null)
        //    //            curveElement.IntensityWay = (IntensityWay)Enum.Parse(typeof(IntensityWay), curveElementNode.SelectSingleNode("IntensityWay").InnerText.ToString());
        //    //            if (curveElementNode.SelectSingleNode("IntensityComparison") != null)
        //    //            curveElement.IntensityComparison = bool.Parse(curveElementNode.SelectSingleNode("IntensityComparison").InnerText.ToString());
        //    //            if (curveElementNode.SelectSingleNode("ComparisonCoefficient") != null)
        //    //            curveElement.ComparisonCoefficient = int.Parse(curveElementNode.SelectSingleNode("ComparisonCoefficient").InnerText.ToString());
        //    //            if (curveElementNode.SelectSingleNode("BPeakLow") != null)
        //    //            curveElement.BPeakLow = int.Parse(curveElementNode.SelectSingleNode("BPeakLow").InnerText.ToString());
        //    //            if (curveElementNode.SelectSingleNode("BPeakHigh") != null)
        //    //            curveElement.BPeakHigh = int.Parse(curveElementNode.SelectSingleNode("BPeakHigh").InnerText.ToString());
        //    //            if (curveElementNode.SelectSingleNode("CalculationWay") != null)
        //    //            curveElement.CalculationWay = (CalculationWay)Enum.Parse(typeof(CalculationWay), curveElementNode.SelectSingleNode("CalculationWay").InnerText.ToString());
        //    //            if (curveElementNode.SelectSingleNode("FpCalculationWay") != null)
        //    //            curveElement.FpCalculationWay = (FpCalculationWay)Enum.Parse(typeof(FpCalculationWay), curveElementNode.SelectSingleNode("FpCalculationWay").InnerText.ToString());
        //    //            if (curveElementNode.SelectSingleNode("Flag") != null)
        //    //            curveElement.Flag = (ElementFlag)Enum.Parse(typeof(ElementFlag), curveElementNode.SelectSingleNode("Flag").InnerText.ToString());
        //    //            if (curveElementNode.SelectSingleNode("LayerFlag") != null)
        //    //            curveElement.LayerFlag = (LayerFlag)Enum.Parse(typeof(LayerFlag), curveElementNode.SelectSingleNode("LayerFlag").InnerText.ToString());
        //    //            //curveElement. = (ElementFlag)Enum.Parse(typeof(ElementFlag), curveElementNode.SelectSingleNode("Flag").InnerText.ToString());
        //    //            //curveElement.LayerFlag = (LayerFlag)Enum.Parse(typeof(LayerFlag), curveElementNode.SelectSingleNode("LayerFlag").InnerText.ToString());
        //    //            // add by chuyaqin 2011-04-28

        //    //            if (curveElementNode.SelectSingleNode("ContentUnit") != null)
        //    //            curveElement.ContentUnit = (ContentUnit)Enum.Parse(typeof(ContentUnit), curveElementNode.SelectSingleNode("ContentUnit").InnerText.ToString());
        //    //            if (curveElementNode.SelectSingleNode("ThicknessUnit") != null)
        //    //            curveElement.ThicknessUnit = (ThicknessUnit)Enum.Parse(typeof(ThicknessUnit), curveElementNode.SelectSingleNode("ThicknessUnit").InnerText.ToString());
        //    //            if (curveElementNode.SelectSingleNode("SReferenceElements") != null)
        //    //            curveElement.SReferenceElements = curveElementNode.SelectSingleNode("SReferenceElements").InnerText.ToString();
        //    //            if (curveElementNode.SelectSingleNode("SSpectrumData") != null)
        //    //            curveElement.SSpectrumData = curveElementNode.SelectSingleNode("SSpectrumData").InnerText.ToString();
        //    //            if (curveElementNode.SelectSingleNode("SInfluenceElements") != null)
        //    //            curveElement.SInfluenceElements = curveElementNode.SelectSingleNode("SInfluenceElements").InnerText.ToString();
        //    //            if (curveElementNode.SelectSingleNode("DistrubThreshold") != null)
        //    //            curveElement.DistrubThreshold = curveElementNode.SelectSingleNode("DistrubThreshold").InnerText.ToString();
        //    //            if (curveElementNode.SelectSingleNode("IsInfluence") != null)
        //    //            curveElement.IsInfluence = bool.Parse(curveElementNode.SelectSingleNode("IsInfluence").InnerText.ToString());
        //    //            if (curveElementNode.SelectSingleNode("SInfluenceCoefficients") != null)
        //    //            curveElement.SInfluenceCoefficients = curveElementNode.SelectSingleNode("SInfluenceCoefficients").InnerText.ToString();
        //    //            if (curveElementNode.SelectSingleNode("ElementSpecName") != null)
        //    //            curveElement.ElementSpecName = curveElementNode.SelectSingleNode("ElementSpecName").InnerText.ToString();



        //    //            #region 标样
        //    //            XmlNodeList standSampleList = curveElementNode.SelectNodes("StandSample");
        //    //            if (standSampleList != null)
        //    //            {
        //    //                foreach (XmlNode standSampleNode in standSampleList)
        //    //                {
        //    //                    StandSample standSample = StandSample.New;
        //    //                    if (standSampleNode.SelectSingleNode("SampleName") != null)
        //    //                    standSample.SampleName = standSampleNode.SelectSingleNode("SampleName").InnerText.ToString();
        //    //                    if (standSampleNode.SelectSingleNode("X") != null)
        //    //                    standSample.X = float.Parse(standSampleNode.SelectSingleNode("X").InnerText.ToString());
        //    //                    if (standSampleNode.SelectSingleNode("Y") != null)
        //    //                    standSample.Y = float.Parse(standSampleNode.SelectSingleNode("Y").InnerText.ToString());
        //    //                    if (standSampleNode.SelectSingleNode("Z") != null)
        //    //                    standSample.Z = float.Parse(standSampleNode.SelectSingleNode("Z").InnerText.ToString());
        //    //                    if (standSampleNode.SelectSingleNode("TheoryX") != null)
        //    //                    standSample.TheoryX = float.Parse(standSampleNode.SelectSingleNode("TheoryX").InnerText.ToString());
        //    //                    if (standSampleNode.SelectSingleNode("Active") != null)
        //    //                    standSample.Active = bool.Parse(standSampleNode.SelectSingleNode("Active").InnerText.ToString());
        //    //                    if (standSampleNode.SelectSingleNode("ElementName") != null)
        //    //                    standSample.ElementName = standSampleNode.SelectSingleNode("ElementName").InnerText.ToString();
        //    //                    if (standSampleNode.SelectSingleNode("SpecListId") != null)
        //    //                    standSample.SpecListId = long.Parse(standSampleNode.SelectSingleNode("SpecListId").InnerText.ToString());
        //    //                    if (standSampleNode.SelectSingleNode("Layer") != null)
        //    //                    standSample.Layer = int.Parse(standSampleNode.SelectSingleNode("Layer").InnerText.ToString());
        //    //                    if (standSampleNode.SelectSingleNode("Level") != null)
        //    //                    standSample.Level = standSampleNode.SelectSingleNode("Level").InnerText.ToString();
        //    //                    if (standSampleNode.SelectSingleNode("TotalLayer") != null)
        //    //                    standSample.TotalLayer = int.Parse(standSampleNode.SelectSingleNode("TotalLayer").InnerText.ToString());
        //    //                    if (standSampleNode.SelectSingleNode("IsMatch") != null)
        //    //                    standSample.IsMatch = bool.Parse(standSampleNode.SelectSingleNode("IsMatch").InnerText.ToString());
        //    //                    //SpecList temp = pecList.FindOne(w => w.Name.EquFals(standSample.SampleName));
        //    //                    SpecList temp=SpecList.FindAll().ToList().Find(delegate(SpecList v) { return v.Name == standSample.SampleName; });
        //    //                    if (temp != null)
        //    //                    {
        //    //                        standSample.MatchSpecListId = temp.Id;
        //    //                        standSample.MatchSpecName = temp.Name;
        //    //                        standSample.IsMatch = true;
        //    //                    }

        //    //                    curveElement.Samples.Add(standSample);
        //    //                }
        //    //            }
        //    //            #endregion
        //    //            #region 元素对应的拟合元素
        //    //            XmlNodeList ReferenceElementList = curveElementNode.SelectNodes("ReferenceElement");
        //    //            if (ReferenceElementList != null)
        //    //            {
        //    //                foreach (XmlNode ReferenceElementNode in ReferenceElementList)
        //    //                {
        //    //                    ReferenceElement referenceElement = ReferenceElement.New;
        //    //                    if (ReferenceElementNode.SelectSingleNode("MainElementName") != null)
        //    //                    referenceElement.MainElementName = ReferenceElementNode.SelectSingleNode("MainElementName").InnerText.ToString();
        //    //                    if (string.IsNullOrEmpty(referenceElement.MainElementName))
        //    //                        continue;
        //    //                    if (ReferenceElementNode.SelectSingleNode("ReferenceElementName") != null)
        //    //                    referenceElement.ReferenceElementName = ReferenceElementNode.SelectSingleNode("ReferenceElementName").InnerText.ToString();
        //    //                    if (ReferenceElementNode.SelectSingleNode("ReferenceLeftBorder") != null)
        //    //                    referenceElement.ReferenceLeftBorder = int.Parse(ReferenceElementNode.SelectSingleNode("ReferenceLeftBorder").InnerText.ToString());
        //    //                    if (ReferenceElementNode.SelectSingleNode("ReferenceRightBorder") != null)
        //    //                    referenceElement.ReferenceRightBorder = int.Parse(ReferenceElementNode.SelectSingleNode("ReferenceRightBorder").InnerText.ToString());
        //    //                    if (ReferenceElementNode.SelectSingleNode("ReferenceBackLeft") != null)
        //    //                    referenceElement.ReferenceBackLeft = int.Parse(ReferenceElementNode.SelectSingleNode("ReferenceBackLeft").InnerText.ToString());
        //    //                    if (ReferenceElementNode.SelectSingleNode("ReferenceBackRight") != null)
        //    //                    referenceElement.ReferenceBackRight = int.Parse(ReferenceElementNode.SelectSingleNode("ReferenceBackRight").InnerText.ToString());
        //    //                    if (ReferenceElementNode.SelectSingleNode("PeakToBack") != null)
        //    //                    referenceElement.PeakToBack = bool.Parse(ReferenceElementNode.SelectSingleNode("PeakToBack").InnerText.ToString());
        //    //                    curveElement.References.Add(referenceElement);
        //    //                }
        //    //            }
        //    //            #endregion
        //    //            #region 元素对应的优化因子
        //    //            XmlNodeList OptimizationList = curveElementNode.SelectNodes("Optimiztion");
        //    //            if (OptimizationList != null)
        //    //            {
        //    //                foreach (XmlNode OptimizationNode in OptimizationList)
        //    //                {
        //    //                    Optimiztion optimize = Optimiztion.New;
        //    //                    if (OptimizationNode.SelectSingleNode("OptimiztionValue") != null)
        //    //                    optimize.OptimiztionValue = double.Parse(OptimizationNode.SelectSingleNode("OptimiztionValue").InnerText.ToString());
        //    //                    if (OptimizationNode.SelectSingleNode("OptimiztionRange") != null)
        //    //                    optimize.OptimiztionRange = double.Parse(OptimizationNode.SelectSingleNode("OptimiztionRange").InnerText.ToString());
        //    //                    if (OptimizationNode.SelectSingleNode("OptimiztionFactor") != null)
        //    //                    optimize.OptimiztionFactor = double.Parse(OptimizationNode.SelectSingleNode("OptimiztionFactor").InnerText.ToString());
        //    //                    curveElement.Optimiztion.Add(optimize);
        //    //                }
        //    //            }
        //    //            #endregion

        //    //            #region 元素对应的影响元素
        //    //            XmlNodeList ElementRefList = curveElementNode.SelectNodes("ElementRefs");
        //    //            if (ElementRefList != null)
        //    //            {
        //    //                foreach (XmlNode ElementRdfNode in ElementRefList)
        //    //                {
        //    //                    ElementRef elementRef = ElementRef.New;
        //    //                    if (ElementRdfNode.SelectSingleNode("Name")!=null)
        //    //                    elementRef.Name = ElementRdfNode.SelectSingleNode("Name").InnerText.ToString();
        //    //                    if (ElementRdfNode.SelectSingleNode("IsRef") != null)
        //    //                    elementRef.IsRef = bool.Parse(ElementRdfNode.SelectSingleNode("IsRef").InnerText.ToString());
        //    //                    if (ElementRdfNode.SelectSingleNode("RefConf") != null)
        //    //                    elementRef.RefConf = double.Parse(ElementRdfNode.SelectSingleNode("RefConf").InnerText.ToString());
        //    //                    if (ElementRdfNode.SelectSingleNode("DistrubThreshold") != null)
        //    //                    elementRef.DistrubThreshold = double.Parse(ElementRdfNode.SelectSingleNode("DistrubThreshold").InnerText.ToString());
        //    //                    curveElement.ElementRefs.Add(elementRef);
        //    //                }
        //    //            }
        //    //            #endregion
        //    //            elementList.Items.Add(curveElement);

        //    //        }
        //    //        #endregion

        //    //        #region 元素自定义

        //    //        listCustoms = elementList.CustomFields.ToList();
        //    //        elementList.CustomFields.Clear();
        //    //        XmlNodeList CustomList = selectElementList.SelectNodes("CustomFields");
        //    //        if (CustomList != null)
        //    //        {
        //    //            foreach (XmlNode custom in CustomList)
        //    //            {
        //    //                CustomField cf = CustomField.New;
        //    //                if (custom.SelectSingleNode("Expression")!=null)
        //    //                cf.Expression = custom.SelectSingleNode("Expression").InnerText;
        //    //                if (custom.SelectSingleNode("Name")!=null)
        //    //                cf.Name = custom.SelectSingleNode("Name").InnerText;
        //    //                elementList.CustomFields.Add(cf);
        //    //            }
        //    //        }
        //    //        #endregion
        //    //        curve.ElementList = elementList;
        //    //    }
        //    //    else curve.ElementList = null;

        //    //    #region  曲线下的校正强度数据
        //    //    List<IntensityCalibration> listItensitycal = new List<IntensityCalibration>();
        //    //    listItensitycal = curve.IntensityCalibration.ToList();
        //    //    curve.IntensityCalibration.Clear();
        //    //    XmlNodeList IntensityCallist = xmlDoc.SelectNodes("IntensityCalibration");
        //    //    if (IntensityCallist != null)
        //    //    {
        //    //        foreach (XmlNode custom in IntensityCallist)
        //    //        {
        //    //            IntensityCalibration cf = IntensityCalibration.New;
        //    //            if (custom.SelectSingleNode("CalibrateIn")!=null)
        //    //            cf.CalibrateIn = double.Parse(custom.SelectSingleNode("CalibrateIn").InnerText);
        //    //            if (custom.SelectSingleNode("Element") != null)
        //    //            cf.Element = custom.SelectSingleNode("Element").InnerText;
        //    //            if (custom.SelectSingleNode("OriginalIn") != null)
        //    //            cf.OriginalIn = double.Parse(custom.SelectSingleNode("OriginalIn").InnerText);
        //    //            curve.IntensityCalibration.Add(cf);
        //    //        }
        //    //    }
        //    //    #endregion


        //    //    XmlNode WorkRegionNode = xmlDoc.SelectSingleNode("CurveInfo/WorkRegion");
        //    //    if (WorkRegionNode != null && curve != null)
        //    //    {
        //    //        WorkRegion temp = WorkRegion.FindOne(w => w.Name == ((WorkRegionNode.SelectSingleNode("Name")==null)?"":WorkRegionNode.SelectSingleNode("Name").InnerText));
        //    //        if (temp != null)
        //    //            curve.WorkRegion = temp;
        //    //        else
        //    //        {
        //    //            WorkRegion rgion = WorkRegion.New;
        //    //            if (WorkRegionNode.SelectSingleNode("Name")!=null)
        //    //            rgion.Name = WorkRegionNode.SelectSingleNode("Name").InnerText;
        //    //            if (WorkRegionNode.SelectSingleNode("RohsSampleType") != null)
        //    //            rgion.RohsSampleType = (RohsSampleType)Enum.Parse(typeof(RohsSampleType), WorkRegionNode.SelectSingleNode("RohsSampleType").InnerText);
        //    //            if (WorkRegionNode.SelectSingleNode("IsDefaultWorkRegion") != null)
        //    //            rgion.IsDefaultWorkRegion = bool.Parse(WorkRegionNode.SelectSingleNode("IsDefaultWorkRegion").InnerText);
        //    //            curve.WorkRegion = rgion;
        //    //        }
        //    //    }
        //    //    curve.Save();
        //    //    for (int i = 0; i < lstElements.Count; i++)
        //    //    {
        //    //        lstElements[i].Delete();
        //    //    }
        //    //    for (int i = 0; i < listCustoms.Count; i++)
        //    //    {
        //    //        listCustoms[i].Delete();
        //    //    }
        //    //    for (int i = 0; i < listItensitycal.Count; i++)
        //    //    {
        //    //        listItensitycal[i].Delete();
        //    //    }


        //    //}
        //    //catch (Exception e)
        //    //{
        //    //    string ss = e.Message.ToString();
        //    //}
        //    #endregion
        //    return false;
        //}

        public void RepeatCaculateSampleIntensity()
        {
            List<SpecListEntity> list = WorkCurveHelper.DataAccess.GetAllSpectrum();

            foreach (SpecListEntity temp in list)
            {
                List<WorkCurve> curveList = WorkCurve.FindAll().ToList().FindAll(delegate(WorkCurve v) { return v.ElementList != null && v.ElementList.Items.Count > 0 && v.ElementList.Items[0].Samples != null && v.ElementList.Items[0].Samples.Count > 0 && v.ElementList.Items[0].Samples.ToList().FindAll(delegate(StandSample s) { return s.SampleName == temp.Name; }) != null; });
                foreach (WorkCurve tempCurve in curveList)
                {
                    tempCurve.CaculateIntensity(temp);
                    tempCurve.Save();
                }
            }
        }

        public static Color[] GetElementColor()
        {
            return new Color[] 
            {
                Color.BlueViolet,
                Color.Brown,
                Color.DarkOrange,
                Color.Aqua,
                Color.DeepSkyBlue,
                Color.DimGray,
                Color.Chartreuse,
                Color.Chocolate,
                Color.DodgerBlue,
                Color.Black,
                Color.DarkBlue,
                Color.DarkRed,
                Color.Aquamarine,
                Color.CornflowerBlue,
                Color.DarkCyan,
                Color.DarkGoldenrod,
                Color.DarkGray,
                Color.DarkGreen,
                Color.DarkKhaki,
                Color.DarkMagenta,
                Color.DarkOliveGreen,
                Color.DarkOrchid,
                Color.DarkSalmon,
                Color.DarkSeaGreen,
                Color.DarkSlateBlue,
                Color.DarkSlateGray,
                Color.DarkTurquoise,
                Color.DarkViolet,
                Color.DeepPink,
                Color.Azure,
                Color.Beige,
                Color.Bisque,
                Color.BlanchedAlmond,
                Color.BurlyWood,
                Color.CadetBlue,
                Color.Coral,
                Color.Cornsilk,
                Color.Crimson,
                Color.Cyan,
                Color.AliceBlue,
                Color.AntiqueWhite,
                Color.HotPink
            };
        }

        //private Lephone.Data.DbContext context = null;
        public SpecListEntity SampleImport(string path)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                string rawXml = System.IO.File.ReadAllText(path);
                xmlDoc.LoadXml(rawXml);
                string ID = (xmlDoc.SelectSingleNode("Spectrum/ID") == null) ? "" : xmlDoc.SelectSingleNode("Spectrum/ID").InnerText;
                string Description = (xmlDoc.SelectSingleNode("Spectrum/Description") == null) ? "" : xmlDoc.SelectSingleNode("Spectrum/Description").InnerText;
                string Container = (xmlDoc.SelectSingleNode("Spectrum/Container") == null) ? "" : xmlDoc.SelectSingleNode("Spectrum/Container").InnerText;
                string PrecTime = (xmlDoc.SelectSingleNode("Spectrum/PrecTime") == null) ? "" : xmlDoc.SelectSingleNode("Spectrum/PrecTime").InnerText;
                string TubVoltage = (xmlDoc.SelectSingleNode("Spectrum/TubVoltage") == null) ? "" : xmlDoc.SelectSingleNode("Spectrum/TubVoltage").InnerText;
                string TubCurrent = (xmlDoc.SelectSingleNode("Spectrum/TubCurrent") == null) ? "" : xmlDoc.SelectSingleNode("Spectrum/TubCurrent").InnerText;
                string Eimax = (xmlDoc.SelectSingleNode("Spectrum/Eimax") == null) ? "" : xmlDoc.SelectSingleNode("Spectrum/Eimax").InnerText;
                string SpecDate = (xmlDoc.SelectSingleNode("Spectrum/SpecDate") == null) ? "" : xmlDoc.SelectSingleNode("Spectrum/SpecDate").InnerText;
                string SampleData = (xmlDoc.SelectSingleNode("Spectrum/Data") == null) ? "" : xmlDoc.SelectSingleNode("Spectrum/Data").InnerText;

                //修改设备的峰通道
                if (SampleData == "") return null;
                int maxChannel = SampleData.Split(',').Length - 1;
                Device device = Device.FindOne(w => w.IsDefaultDevice == true);
                if (device == null)
                    return null;
                device.SpecLength = (maxChannel == 1024) ? SpecLength.Min : ((maxChannel == 2048) ? SpecLength.Normal : SpecLength.Max);

                Device.FindBySql("update device set SpecLength=" + maxChannel + " where IsDefaultDevice=1");
                //导入谱文件
                string SampleName = path.Split('\\')[path.Split('\\').Length - 1].ToString();
                SampleName = (SampleName.IndexOf(".") != -1) ? SampleName.Substring(0, SampleName.IndexOf(".")) : SampleName;

                SpecEntity spec = new SpecEntity(SampleName, SampleData, int.Parse((PrecTime == "") ? "0" : PrecTime), int.Parse((PrecTime == "") ? "0" : PrecTime), int.Parse((TubVoltage == "") ? "0" : TubVoltage), int.Parse((TubCurrent == "") ? "0" : TubCurrent), "");
                SpecListEntity sl = new SpecListEntity();
                sl.Name = SampleName;
                sl.SampleName = SampleName;
                if (!string.IsNullOrEmpty(SpecDate))
                    sl.SpecDate = DateTime.Parse(SpecDate);
                spec.DeviceParameter = WorkCurveHelper.WorkCurveCurrent != null ? WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList[0].ConvertFrom() : device.Conditions[0].DeviceParamList[0].ConvertFrom();
                sl.InitParam = WorkCurveHelper.WorkCurveCurrent != null ? WorkCurveHelper.WorkCurveCurrent.Condition.InitParam.ConvertToNewEntity() : device.Conditions[0].InitParam.ConvertToNewEntity();
                sl.Specs = new SpecEntity[1];
                sl.Specs[0] = spec;
                spec.IsSmooth = false;
                sl.ImageShow = true;
                return sl;
            }
            catch
            {
            }
            return null;
        }

        public SpecListEntity SampleImportNew(string path)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                string rawXml = System.IO.File.ReadAllText(path);
                xmlDoc.LoadXml(rawXml);
                string ID = (xmlDoc.SelectSingleNode("Spectrum/ID") == null) ? "" : xmlDoc.SelectSingleNode("Spectrum/ID").InnerText;
                string Description = (xmlDoc.SelectSingleNode("Spectrum/Description") == null) ? "" : xmlDoc.SelectSingleNode("Spectrum/Description").InnerText;
                string Container = (xmlDoc.SelectSingleNode("Spectrum/Container") == null) ? "" : xmlDoc.SelectSingleNode("Spectrum/Container").InnerText;
                string PrecTime = (xmlDoc.SelectSingleNode("Spectrum/PrecTime") == null) ? "" : xmlDoc.SelectSingleNode("Spectrum/PrecTime").InnerText;
                string TubVoltage = (xmlDoc.SelectSingleNode("Spectrum/TubVoltage") == null) ? "" : xmlDoc.SelectSingleNode("Spectrum/TubVoltage").InnerText;
                string TubCurrent = (xmlDoc.SelectSingleNode("Spectrum/TubCurrent") == null) ? "" : xmlDoc.SelectSingleNode("Spectrum/TubCurrent").InnerText;
                string Eimax = (xmlDoc.SelectSingleNode("Spectrum/Eimax") == null) ? "" : xmlDoc.SelectSingleNode("Spectrum/Eimax").InnerText;
                string SpecDate = (xmlDoc.SelectSingleNode("Spectrum/SpecDate") == null) ? "" : xmlDoc.SelectSingleNode("Spectrum/SpecDate").InnerText;
                string SampleData = (xmlDoc.SelectSingleNode("Spectrum/Data") == null) ? "" : xmlDoc.SelectSingleNode("Spectrum/Data").InnerText;

                //修改设备的峰通道
                if (SampleData == "") return null;
                int maxChannel = SampleData.Split(',').Length - 1;


                //device.SpecLength = (maxChannel == 1024) ? SpecLength.Min : ((maxChannel == 2048) ? SpecLength.Normal : SpecLength.Max);

                //Device.FindBySql("update device set SpecLength=" + maxChannel + " where IsDefaultDevice=1");

                //导入谱文件
                string SampleName = path.Split('\\')[path.Split('\\').Length - 1].ToString();
                SampleName = (SampleName.IndexOf(".") != -1) ? SampleName.Substring(0, SampleName.IndexOf(".")) : SampleName;

                Device device = Device.FindOne(w => w.IsDefaultDevice == true);
                if (device == null)
                    return null;
                int[] ints = Helper.ToInts(SampleData);
                SpecListEntity sl = new SpecListEntity();
                sl.SampleName = SampleName;
                sl.Color = Color.Blue.ToArgb();
                sl.VirtualColor = Color.Blue.ToArgb();
                Condition conditon = null;
                if (WorkCurveHelper.WorkCurveCurrent != null && WorkCurveHelper.WorkCurveCurrent.Condition != null)
                    conditon = WorkCurveHelper.WorkCurveCurrent.Condition;
                else
                    conditon = device.Conditions[0];
                //sl.Condition = conditon;
                if (conditon.DeviceParamList != null && conditon.DeviceParamList.Count > 0)
                {
                    sl.Specs = new SpecEntity[conditon.DeviceParamList.Count];
                    int k = 0;
                    foreach (DeviceParameter tep in conditon.DeviceParamList)
                    {
                        int[] deviceParamsData = new int[(int)device.SpecLength];
                        Array.Copy(ints, tep.BeginChann, deviceParamsData, tep.BeginChann, tep.EndChann - tep.BeginChann);
                        for (int i = 0; i < deviceParamsData.Length; i++)
                            deviceParamsData[i] = (int)(deviceParamsData[i] * (float)tep.PrecTime / conditon.DeviceParamList[0].PrecTime);
                        SpecEntity spec = new SpecEntity(SampleName, Helper.ToStrs(deviceParamsData), tep.PrecTime, tep.PrecTime, tep.TubVoltage, tep.TubCurrent, "");
                        if (!string.IsNullOrEmpty(SpecDate))
                            sl.SpecDate = DateTime.Parse(SpecDate);
                        spec.DeviceParameter = new DeviceParameterEntity(tep.Name, tep.PrecTime, tep.TubCurrent, tep.TubVoltage, tep.FilterIdx, tep.CollimatorIdx, tep.TargetIdx, tep.IsVacuum, tep.VacuumTime, tep.IsVacuumDegree, tep.VacuumDegree, tep.IsAdjustRate, tep.MinRate, tep.MaxRate, tep.BeginChann, tep.EndChann, tep.IsDistrubAlert, tep.IsPeakFloat, tep.PeakFloatLeft, tep.PeakFloatRight, tep.PeakFloatChannel, tep.PeakFloatError, tep.PeakCheckTime, tep.TargetMode, tep.CurrentRate);
                        spec.IsSmooth = false;
                        sl.Specs[k] = spec;
                        k++;
                    }
                }
                sl.InitParam = conditon.InitParam.ConvertToNewEntity();
                sl.Name = SampleName;
                sl.ImageShow = true;
                return sl;
            }
            catch
            {

            }
            return null;
        }


        //public void SetCondition(string EditionType)
        //{
        //    ccondition = Condition.New.Init("EDX" + EditionType);
        //    //HasMany<DeviceParameter> DeviceParameterlist = new HasMany<DeviceParameter>();
        //    DeviceParameter dparameter = DeviceParameter.New;
        //    dparameter.Name = "edx";
        //    //DeviceParameterlist.Add(dparameter);
        //    ccondition.DeviceParamList.Add(dparameter);
        //    Device device1 = Device.FindOne(x => x.IsDefaultDevice == true);//取得设备
        //    ccondition.Device = device1;
        //    ccondition.Save();
        //}

        public void GetFiles(string strPath, string type)
        {
            try
            {
                //获取文件夹下的所有文件
                DirectoryInfo fdir = new DirectoryInfo(strPath);
                FileInfo[] file = fdir.GetFiles();

                //遍历该文件夹下的所有文件
                foreach (FileInfo f in file)
                {
                    //如果文件的扩展名包含于该ArrayList内
                    if (f.Extension.ToUpper() == ".XML")
                    {
                        if (type == "Sample")
                            FileList.Add(f.FullName.ToString());
                        else if (type == "Curve")
                            CurveFileList.Add(f.FullName.ToString());
                    }

                }
            }
            catch
            {

            }
        }
    }
}
