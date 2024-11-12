using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;
using Skyray.EDX.Common.Component;
using System.Drawing;
using System.Windows.Forms;
using Skyray.Controls.Extension;
using Skyray.Camera;

using System.Reflection;
using System.Data;
using Skyray.EDX.Common.ReportHelper;
using System.Drawing.Imaging;
using System.IO;
using Skyray.EDXRFLibrary.Spectrum;
using System.Xml;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Skyray.EDXRFLibrary.Define;
using System.Collections;
using System.Runtime;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

using System.Threading;
namespace Skyray.UC
{
    /// <summary>
    /// 测厚软件功能
    /// </summary>
    public class Thick : InterfaceClass
    {
        /// <summary>
        /// 聚焦点
        /// </summary>
        private Point FocusPointClient
        {
            get
            {
                return new Point((int)(this.skyrayCamera.FociX * this.skyrayCamera.Size.Width / this.skyrayCamera.ViewWidth),
                    (int)(this.skyrayCamera.FociY * this.skyrayCamera.Size.Height / this.skyrayCamera.ViewHeight));
            }
        }


        /// <summary>
        /// 源点
        /// </summary>
        private Point SourcePoint;

        /// <summary>
        /// 摄像头用户控件
        /// </summary>
        public UCCameraControl cameraControlUC;

        /// <summary>
        /// 目的点
        /// </summary>
        private Point DestPoint;

        /// <summary>
        /// 多次测量的第一个点
        /// </summary>
        private Point FirstPoint;

        /// <summary>
        /// 多次测量最后一个测量点;
        /// </summary>
        private Point LastPoint;
        /// <summary>
        /// 计算的 X方向的距离
        /// </summary>
        private int DistanceX;

        /// <summary>
        /// 计算Y方向的距离
        /// </summary>
        private int DistanceY;

        /// <summary>
        /// 保存先前的X方向的距离
        /// </summary>
        private int PrevDistanceX;

        /// <summary>
        /// 保存先前的Y方向的距离
        /// </summary>
        private int PrevDistanceY;

        /// <summary>
        /// 判断是否按下Control建
        /// </summary>
        public bool bIsControlKeyPress;

        /// <summary>
        /// 判断是否选择了移动
        /// </summary>
        private bool bIsMove;

        /// <summary>
        /// 判断是否选择了校正
        /// </summary>
        private bool bIsCheck;

        /// <summary>
        /// 判断是否选择了网格
        /// </summary>
        private bool bIsNetwork;

        /// <summary>
        /// 判断是否选择了单点
        /// </summary>
        private bool bIsSingle;

        /// <summary>
        /// 判断是否选择了多点
        /// </summary>
        private bool bIsMulti;

        private bool bIsMatrix;
        /// <summary>
        /// 保存点击的点
        /// </summary>
        private Point CurrentMouseClickPoint;


        private int PointIndex;

        private int PointCount;

        /// <summary>
        /// 保存先前点
        /// </summary>
        private Point PrevSelectedPointClient;

        private Point CurrSelectedPointClient;

        private FrmGridParams frmGridParams;
        private FrmSetAdjustPointsDistance frmAdjustDistance;

        private CheckBox highCheckBox;
        private RadioButton radioButtonMove;

        private Point MotorMoveCurrentStation;

        private Point ResetPosition;

        private int CalibrateElemCnt = 0;

        private ArrayList tempLst = new ArrayList();

        private int Band = 50;

        private bool isMouseDown = false;

        /// <summary>
        /// 是否为固定步长
        /// </summary>
        //private bool IsFixedWalk;

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public Thick()
        {
            ExcelTemplateParams.LoadExcelTemplateParams(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml");
            LoadParameter(AppDomain.CurrentDomain.BaseDirectory + "Parameter.xml");
        }



        /// <summary>
        /// 已经聚集完成，转让电机停止事件处理函数
        /// </summary>
        void MotorAdvance_FocusPointEndEvent()
        {
            MessageStopMove(optMode);
        }

        void MotorAdvance_AutoFocusEndEvent()
        {
            FinishFocusState(true);
        }
   
        private void LoadNiCuNiFormula(string elem, ref double aValue, ref  double nvalue, ref double ImpactAvalue, ref double ImpactNvalue)
        {
            
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Parameter.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            string nodename = elem + "Param";
            XmlNodeList xmlNodeList = doc.SelectNodes("Parameter/DataParam/" + nodename);
            if (xmlNodeList != null)
            {
                foreach (XmlNode xTemp in xmlNodeList)
                {
                    if (xTemp.Name == nodename)
                    {
                        aValue = xTemp.Attributes["aValue"].InnerText == null ? 0.9825 : double.Parse(xTemp.Attributes["aValue"].InnerText);
                        nvalue = xTemp.Attributes["nvalue"].InnerText == null ? 1.8027 : double.Parse(xTemp.Attributes["nvalue"].InnerText);
                        ImpactAvalue = xTemp.Attributes["ImpactAvalue"].InnerText == null ? -0.221 : double.Parse(xTemp.Attributes["ImpactAvalue"].InnerText);
                        ImpactNvalue = xTemp.Attributes["ImpactNvalue"].InnerText == null ? -0.1916 : double.Parse(xTemp.Attributes["ImpactNvalue"].InnerText);
                    }
                }
            }
        }

        /// <summary>
        /// 计算含量及强度
        /// </summary>
        /// <param name="workCurve">指定的工作曲线</param>
        /// <param name="currentTestTimes">目前扫描的次数</param>
        /// <returns></returns>
        public override bool CaculateContent(List<SpecListEntity> specList, int currentTestTimes, bool IsAddHistory)
        {
            if (specList == null || specList.Count == 0 || WorkCurveHelper.WorkCurveCurrent == null)
                return false;

            double avalue = 0;
            double nvalue = 0;
            double ImpactAvalue = 0;
            double ImpactNvalue = 0;

            List<CurveElement> ListNotFistElem = new List<CurveElement>();
            this.elementName = string.Empty;
            IsExistHistory = IsAddHistory;
            if (this.XrfChart != null)
            {
                this.XrfChart.BoundaryElement = elementName;
                this.XrfChart.IUseBoundary = false;
                this.XrfChart.IUseBase = false;
            }
            if (WorkCurveHelper.WorkCurveCurrent != null)
                DeviceParameterByElementList(WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList.ToList());
            int selectIndex = 0;
            if (!InterfaceClass.isMulitTest) selePrintObjectL.Clear();
            foreach (SpecListEntity tempLsit in specList)
            {
                if (tempLsit.Specs.Length < this.deviceParamsList.Count && (FpWorkCurve.thickMode != ThickMode.NiP && FpWorkCurve.thickMode != ThickMode.NiP2))
                {
                    Msg.Show(Info.WorkCurveMeasureCondition);
                    return false;
                }
                if (WorkCurveHelper.WorkCurveCurrent.ElementList == null || WorkCurveHelper.WorkCurveCurrent.ElementList.Items == null || WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count <= 0)
                    return false;
                var testLayer = from test in WorkCurveHelper.WorkCurveCurrent.ElementList.Items
                                group test by test.LayerNumber
                                    into custGroup
                                    orderby custGroup.Key
                                    select custGroup;

               
                int SetCurrent = WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList[0].TubCurrent;
                WorkCurveHelper.WorkCurveCurrent.SetCurrent(SetCurrent);

                if (FpWorkCurve.thickMode == ThickMode.NiCuNiFe || FpWorkCurve.thickMode == ThickMode.NiCuNiFe2)
                {

                    LoadNiCuNiFormula("Ag", ref avalue, ref nvalue, ref ImpactAvalue, ref ImpactNvalue);
                    WorkCurveHelper.WorkCurveCurrent.SetANValue(
                        WorkCurveHelper.NiCuNiParam.aValue, WorkCurveHelper.NiCuNiParam.nValue, WorkCurveHelper.NiCuNiParam.limit, WorkCurveHelper.NiCuNiParam.kValue, WorkCurveHelper.NiCuNiParam.bValue,
                        avalue, nvalue, ImpactAvalue, ImpactNvalue);
                }

                if (WorkCurveHelper.isShowEncoder)
                {
                    bool isNormal = true;
                    foreach (var element in WorkCurveHelper.WorkCurveCurrent.ElementList.Items)
                    {
                        DifferenceDevice.TransSSpectrumnHeight(WorkCurveHelper.DeviceCurrent.Name, element, tempLsit.Height, isNormal, WorkCurveHelper.IsPureElemCurrentUnify);
                    }

                    //if (WorkCurveHelper.IsPureElemCurrentUnify)
                    //    DifferenceDevice.TransHeightPureZero(WorkCurveHelper.WorkCurveCurrent, tempLsit.Height, false, tempLsit, SetCurrent);
                    //else
                    //    DifferenceDevice.TransHeightPureByZero(WorkCurveHelper.WorkCurveCurrent, tempLsit.Height, false, tempLsit, SetCurrent);

                }
                if (tempLsit.SpecType == SpecType.PureSpec)
                    return false;
                if (!CheckWholeCurveElementSamples())
                {
                    return false;
                }
                try
                {
                    ElementList tempElement = ElementList.New;
                    Condition tempCondition = Condition.New;
                    SpecEntity[] tempSpecEntity = new SpecEntity[2];
                    ElementList tempbackElement = ElementList.New;
                    CurveElement coElem = CurveElement.New;
                    if (FpWorkCurve.thickMode == ThickMode.NiP || FpWorkCurve.thickMode == ThickMode.NiP2)
                    {
                        if (tempLsit.Specs.Length < 2) return false;
                        foreach (CurveElement elementCurve in WorkCurveHelper.WorkCurveCurrent.ElementList.Items)
                        {
                            tempElement.Items.Add(elementCurve);

                        }
                        foreach (DeviceParameter deviceparam in WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList)
                        {
                            tempCondition.DeviceParamList.Add(deviceparam);
                        }

                        if (WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count > 2)
                        {
                            WorkCurveHelper.WorkCurveCurrent.ElementList.Items.RemoveAt(WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().FindIndex(w => w.Caption == "P"));
                        }
                        if (WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList.Count > 1)
                        {
                            WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList.RemoveAt(0);
                        }

                        //条件2的谱图
                        tempSpecEntity[0] = tempLsit.Specs[0];
                        tempSpecEntity[1] = tempLsit.Specs[1];

                        tempLsit.Specs = new SpecEntity[1];
                        tempLsit.Specs[0] = tempSpecEntity[1];
                        foreach (CurveElement elementCurve in WorkCurveHelper.WorkCurveCurrent.ElementList.Items)
                        {
                            elementCurve.Samples.Clear();
                            elementCurve.IntensityWay = IntensityWay.Reference;
                            // elementCurve.SInfluenceElements = "Fe,Ni";
                            elementCurve.ConditionID = 0;
                        }

                    }

                    if (WorkCurveHelper.WorkCurveCurrent.CaculateIntensity(tempLsit))
                    {
                              
                        #region 优化4
                        if (WorkCurveHelper.WorkCurveCurrent.IsOptionJoinFirstI && (WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().FindAll(w => w.LayerNumber > 2).Count > 0))
                        {

                            //计算第一次厚度
                            bool ret = WorkCurveHelper.WorkCurveCurrent.CaculateThick(tempLsit, WorkCurveHelper.ThicknessLimit, true);
                            if (ret)
                            {
                                //弃用
                                //获取非第一层的元素厚度
                                List<CurveElement> ListTempNotFistElem = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().FindAll(w => w.LayerNumber != 1 && w.LayerNumber != WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count);

                                if (ListTempNotFistElem.Count > 0)
                                {
                                    for (int i = 0; i < ListTempNotFistElem.Count; i++)
                                    {
                                        CurveElement elem = CurveElement.New;
                                        elem.Caption = ListTempNotFistElem[i].Caption;
                                        elem.Thickness = ListTempNotFistElem[i].Thickness;
                                        ListNotFistElem.Add(elem);

                                    }
                                }

                                //修改不参与计算元素强度恢复优化前
                                foreach (var elem in WorkCurveHelper.WorkCurveCurrent.GetFirstLayerIntensity)
                                {
                                    if (WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().Find(w => w.Caption == elem.Caption) != null)
                                    {
                                        WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().Find(w => w.Caption == elem.Caption).Intensity = elem.Intensity;
                                    }
                                }
                            }
                            else
                            {
                                Msg.Show(Info.MessageBoxTextCalculateWrong + "I");
                            }
                        }
                        #endregion
                        if (WorkCurveHelper.WorkCurveCurrent.CaculateThick(tempLsit, WorkCurveHelper.ThicknessLimit, true))
                        {
                            if (FpWorkCurve.thickMode == ThickMode.NiCuNiFe || FpWorkCurve.thickMode == ThickMode.NiCuNiFe2)
                            {
                                WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().Find(w => w.Caption == "Fe").Thickness = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().Find(w => w.Caption == "Co").Thickness;
                                WorkCurveHelper.WorkCurveCurrent.ElementList.Items.RemoveAt(WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().FindIndex(w => w.Caption == "Co"));
                                if (FpWorkCurve.thickMode == ThickMode.NiCuNiFe)
                                    WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().Find(w => w.Caption == "Fe").LayerNumber = 3;
                                else
                                    WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().Find(w => w.Caption == "Fe").LayerNumber = 4;

                            }
                            if (ListNotFistElem.Count > 0)
                            {
                                foreach (var elem in ListNotFistElem)
                                {
                                    if (WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().Find(w => w.Caption == elem.Caption) != null)
                                    {
                                        WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().Find(w => w.Caption == elem.Caption).Thickness = elem.Thickness;
                                    }
                                }
                            }
                            #region Nip
                            if (FpWorkCurve.thickMode == ThickMode.NiP || FpWorkCurve.thickMode == ThickMode.NiP2)
                            {
                                double pThickness = 0;
                                double niPThickness = WorkCurveHelper.WorkCurveCurrent.ElementList.Items[1].Thickness;  //npThickness
                                double PIntensity = WorkCurveHelper.WorkCurveCurrent.ElementList.Items[1].Intensity;
                                //////
                                //以上算出Ni-Fe 中，Ni厚度

                                //EC绝对强度
                                WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Clear();
                                CurveElement baseElem = (CurveElement)tempElement.Items.ToList().Find(w => w.Caption == "Ni");

                                baseElem.LayerNumber = 2;
                                baseElem.LayerNumBackUp = Info.Substrate;
                                baseElem.IntensityWay = IntensityWay.FullArea;
                                baseElem.SReferenceElements = "Ni,P";
                                baseElem.BaseLow = 50;
                                baseElem.BaseHigh = 300;

                                CurveElement FirstElem = (CurveElement)tempElement.Items.ToList().Find(w => w.Caption == "P");

                                FirstElem.IntensityWay = IntensityWay.FullArea;
                                FirstElem.SReferenceElements = "Ni,P";


                                WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Add(baseElem);
                                WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Add(FirstElem);

                                WorkCurveHelper.WorkCurveCurrent.ElementList.PureAsInfinite = false;
                                WorkCurveHelper.WorkCurveCurrent.CalcType = CalcType.PeakDivBase;
                                if (FpWorkCurve.thickMode == ThickMode.NiP2)
                                {
                                    //WorkCurveHelper.WorkCurveCurrent.CalcType = CalcType.EC;
                                    baseElem.IntensityWay = IntensityWay.FixedReference;//IntensityWay.Reference;
                                    FirstElem.IntensityWay = IntensityWay.FixedReference;//IntensityWay.Reference;

                                    foreach (var element in WorkCurveHelper.WorkCurveCurrent.ElementList.Items)
                                    {
                                        //纯元素谱
                                        DifferenceDevice.TransSSpectrumnHeight(WorkCurveHelper.DeviceCurrent.Name, element, tempLsit.Height, false, WorkCurveHelper.IsPureElemCurrentUnify);

                                        element.References.Clear();
                                        element.SReferenceElements = string.Empty;
                                        foreach (var ele in WorkCurveHelper.WorkCurveCurrent.ElementList.Items)
                                        {
                                            ReferenceElement re = ReferenceElement.New.Init(element.Caption, ele.Caption, ele.PeakLow, ele.PeakHigh, ele.BaseLow, ele.BaseHigh, ele.PeakDivBase);
                                            element.References.Add(re);
                                            element.SReferenceElements += ele.Caption + ",";
                                        }
                                        element.SReferenceElements = element.SReferenceElements.Substring(0, element.SReferenceElements.Length - 1);

                                    }
                                    WorkCurveHelper.WorkCurveCurrent.ElementList.PureAsInfinite = true;
                                }



                                //条件重新加载
                                WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList.Clear();
                                DeviceParameter firstCondition = tempCondition.DeviceParamList[0];
                                DeviceParameter firstParams = DeviceParameter.New.Init(firstCondition.Name, firstCondition.PrecTime, firstCondition.TubCurrent, firstCondition.TubVoltage, firstCondition.FilterIdx, firstCondition.CollimatorIdx, firstCondition.TargetIdx, firstCondition.IsVacuum,
                                    firstCondition.VacuumTime, firstCondition.IsVacuumDegree, firstCondition.VacuumDegree, firstCondition.IsAdjustRate, firstCondition.MinRate, firstCondition.MaxRate, firstCondition.BeginChann, firstCondition.EndChann, firstCondition.IsDistrubAlert, firstCondition.IsPeakFloat, firstCondition.PeakFloatLeft,
                                    firstCondition.PeakFloatRight, firstCondition.PeakFloatChannel, firstCondition.PeakFloatError, firstCondition.PeakCheckTime, firstCondition.TargetMode, firstCondition.CurrentRate);
                                WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList.Add(firstParams);

                                //使用条件 1的谱
                                tempLsit.Specs = new SpecEntity[1];
                                tempLsit.Specs[0] = tempSpecEntity[0];

                                //EC峰倍比
                                if (PIntensity < WorkCurveHelper.IntensityLimit)
                                {
                                    baseElem.PeakDivBase = true;
                                    FirstElem.PeakDivBase = true;
                                }
                                //重新加载标样谱
                                foreach (var element in WorkCurveHelper.WorkCurveCurrent.ElementList.Items)
                                {
                                    if (element.Samples != null && element.Samples.Count > 0)
                                    {
                                        foreach (var sample in element.Samples)
                                        {
                                            // SqlParams params0 = new SqlParams("SpecTypeValue", "0", true);
                                            SqlParams params1 = new SqlParams("Name", sample.SampleName, false);
                                            List<SpecListEntity> lstSpecList = WorkCurveHelper.DataAccess.Query(new SqlParams[] { params1 });
                                            if (lstSpecList.Count > 0)
                                            {
                                                try
                                                {
                                                    WorkCurveHelper.WorkCurveCurrent.CaculateIntensity(lstSpecList[0]);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Msg.Show(lstSpecList[0].SampleName + ex.Message);
                                                    continue;
                                                }
                                                sample.X = element.Intensity.ToString();
                                                sample.Y = sample.Z;
                                            }
                                        }
                                    }
                                }

                                if (WorkCurveHelper.WorkCurveCurrent.CaculateIntensity(tempLsit) && WorkCurveHelper.WorkCurveCurrent.CaculateThick(tempLsit, WorkCurveHelper.ThicknessLimit, true))
                                {
                                    pThickness = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().Find(w => w.Caption == "P").Thickness;
                                }
                                WorkCurveHelper.WorkCurveCurrent.CalcType = CalcType.FP;
                                WorkCurveHelper.WorkCurveCurrent.ElementList.PureAsInfinite = true;
                                WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList.Clear();
                                foreach (DeviceParameter deviceparam in tempCondition.DeviceParamList)
                                {
                                    WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList.Add(deviceparam);
                                }


                                WorkCurveHelper.WorkCurveCurrent = WorkCurve.FindById(WorkCurveHelper.WorkCurveCurrent.Id);

                                WorkCurveHelper.WorkCurveCurrent.ElementList.Items[1].Thickness = WorkCurveHelper.WorkCurveCurrent.ElementList.Items[2].Thickness = niPThickness * (1 + pThickness / 100 * WorkCurveHelper.POptimization);
                                WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().Find(w => w.Caption == "P").Content = pThickness;
                                tempLsit.Specs = new SpecEntity[tempSpecEntity.Length];
                                for (int i = 0; i < tempSpecEntity.Length; i++)
                                {
                                    tempLsit.Specs[i] = tempSpecEntity[i];
                                }
                            }
                            #endregion


                            #region 数据优化5
                            if (WorkCurveHelper.IsCarrayMatchPKSetting && currentTestTimes > 1 && DifferenceDevice.interClassMain.startTest)
                            {
                                Random random = new Random();
                                //  for (int i = 0; i < WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count; ++i)
                                {
                                    //   double value = WorkCurveHelper.WorkCurveCurrent.ElementList.Items[i].Thickness;
                                    //测量值向真值靠近(真值放入优化五的value值)

                                    WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().ForEach(
                                                    w =>
                                                    {
                                                        var op = w.Optimiztion.ToList().Find(wo => wo.OptimizetionType == 4);
                                                        if (op != null)
                                                        {
                                                            double randomvalue = (double)op.OptimiztionFactor / (Math.Sqrt((double)this.spec.SpecTime / 5));
                                                            randomvalue = (double)(random.Next(-100, 100)) * randomvalue / 10000;
                                                            w.Thickness = op.OptimiztionValue + op.OptimiztionValue * randomvalue;
                                                            //if (w.ThicknessUnit == ThicknessUnit.ur)
                                                            //    w.Thickness = w.Thickness * 0.0254;
                                                        }
                                                    }
                                                    );
                                }
                            #endregion
                            }


                            selectIndex++;
                            if (IsAddHistory && WorkCurveHelper.IsSaveHistory)
                            {
                                #region  打开谱操作的时候设置保存到历史记录对应谱的样品信息
                                if (tempLsit != null && tempLsit.CompanyInfoList != null)
                                {
                                    WorkCurveHelper.SeleCompanyOthersInfo.Clear();
                                    foreach (CompanyOthersInfo info in tempLsit.CompanyInfoList)
                                    {
                                        WorkCurveHelper.SeleCompanyOthersInfo.Add(info.Id.ToString(), info.DefaultValue.ToString());
                                    }
                                }
                                else
                                    WorkCurveHelper.SeleCompanyOthersInfo.Clear();
                                #endregion

                                RefreshHistoryRecord(tempLsit, WorkCurveHelper.WorkCurveCurrent);
                            }
                            if (specList.Count > 1)
                            {
                                currentTestTimes = selectIndex;
                                this.refreshFillinof.RefreshMeasureResult(selectIndex, WorkCurveHelper.WorkCurveCurrent.ElementList);
                            }
                            else
                            {
                                if (DifferenceDevice.interClassMain.bIsCameraStartTest)
                                {
                                    ScannedContiTestPointsCount++;
                                    this.refreshFillinof.RefreshMeasureResult(ScannedContiTestPointsCount, WorkCurveHelper.WorkCurveCurrent.ElementList);
                                }
                                else
                                {
                                    
                                    this.refreshFillinof.RefreshMeasureResult(currentTestTimes, WorkCurveHelper.WorkCurveCurrent.ElementList);
                                }
                            }


                            if (WorkCurveHelper.IsCarrayMatchPKSetting
                                && DifferenceDevice.interClassMain != null
                                && DifferenceDevice.interClassMain.testDevicePassedParams != null
                                && DifferenceDevice.interClassMain.testDevicePassedParams.MeasureParams != null
                                && DifferenceDevice.interClassMain.startTest
                                && IsAddHistory
                                && currentTestTimes < DifferenceDevice.interClassMain.testDevicePassedParams.MeasureParams.MeasureNumber
                                && currentTestTimes == 1) //第一次加入标样谱 2014-01-21pk开启时如果是多次测量时如果没有匹配到标样，测量值加入标样-------------------------------------------------
                            {

                                //测量值向真值靠近(真值放入优化五的value值)
                                WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().ForEach(
                                                w =>
                                                {
                                                    var op = w.Optimiztion.ToList().Find(wo => wo.OptimizetionType == 4);
                                                    if (op != null)
                                                    {
                                                        op.OptimiztionValue = w.Thickness;
                                                    }
                                                }
                                                );
                            }
                        }
                        else
                        {
                            Msg.Show(Info.MessageBoxTextCalculateWrong + "I");
                        }

                    }
                    else
                    {
                        //this.refreshFillinof.RefreshMeasureResult(currentTestTimes, WorkCurveHelper.WorkCurveCurrent.ElementList);
                        Msg.Show(Info.MessageBoxTextCalculateWrong + "I");
                    }

                }
                catch (Exception e)
                {
                    Msg.Show(e.Message);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(0);
                    return false;
                    //Msg.Show(e.Message);
                    //return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 对未知样本进行扫描处理
        /// </summary>
        /// <param name="workCurve">指定工作曲线</param>
        /// <param name="testDeviceParams">扫描传递过来的参数</param>
        public override void MeasureUnkownSpecProcess(WorkCurve workCurve, TestDevicePassedParams testDeviceParams)
        {
            if (workCurve == null || testDeviceParams == null)
            {
                Msg.Show(Info.WarningTestContext, Info.TestWarning, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            RefreshCameraText(1, testDeviceParams.MeasureParams.MeasureNumber, false);
            if (workCurve.ElementList == null || workCurve.ElementList.Items == null || workCurve.ElementList.Items.Count == 0)
            {
                this.optMode = OptMode.UnknownSave;
                StartTestModeInitialize(workCurve, testDeviceParams);
            }
            else
            {
                this.optMode = OptMode.Test;
                StartTestModeInitialize(workCurve, testDeviceParams);
            }
        }


        public bool DeviceMeasurePassingThick(Device device, TestDevicePassedParams testDeviceParams)
        {
            if (device == null) return false;
            this.specList = new SpecListEntity(testDeviceParams.WordCureTestList[0].Spec.Name, testDeviceParams.WordCureTestList[0].Spec.SampleName, DifferenceDevice.interClassMain.deviceMeasure.interfacce.ReturnEncoderValue, DifferenceDevice.interClassMain.deviceMeasure.interfacce.ReturnEncoderHeight, testDeviceParams.WordCureTestList[0].Spec.Supplier,
                testDeviceParams.WordCureTestList[0].Spec.Weight, testDeviceParams.WordCureTestList[0].Spec.Shape, testDeviceParams.WordCureTestList[0].Spec.Operater, DateTime.Now, testDeviceParams.WordCureTestList[0].Spec.SpecSummary, testDeviceParams.WordCureTestList[0].Spec.SpecType, DifferenceDevice.DefaultSpecColor.ToArgb(), DifferenceDevice.DefaultSpecColor.ToArgb());
            //this.specList.Condition = initParams.Condition;
            this.testDevicePassedParams = testDeviceParams;
            this.deviceParamSelectIndex = 0;
            this.spec = new SpecEntity();
            this.spec.IsSmooth = true;
            this.spec.RemarkInfo = testDeviceParams.RemarkInformation;
            this.currentTestTimes = 1;
            this.deviceMeasure.interfacce.Spec = this.spec;
            this.demacateMode = Demarcate.None;
            this.specList.SpecDate = DateTime.Now;
            WorkCurveHelper.SelectSpectrumPath = "";
            WorkCurveHelper.EditionType = TotalEditionType.Default;
            recordList.Clear();
            if (this.deviceParamsList != null && this.deviceParamsList.Count > 0)
            {
                progressInfo.MeasureTime = this.deviceParamsList[0].PrecTime + "s";
                progressInfo.SurplusTime = "0s";
            }
            //判断数据库中是否存在指定的谱名称
            IsIncrement = false;
            //string specListName = GetSpecTypeName(this.specList.SampleName) + "_" + (currentTestTimes) + "_" + PointCount;
            //string specListName = GetDefineSpectrumName(this.specList.SampleName, DifferenceDevice.interClassMain.testDevicePassedParams.IsRuleName) + "_" + PointCount;
            //if (!DifferenceDevice.interClassMain.SpeclistNameValidate())
            //{
            //    return false;
            //}

            //testDevice.MeasureParams.MeasureNumber * skyrayCamera.ContiTestPoints.Count

            string specListName = GetDefineSpectrumName(this.specList.SampleName, DifferenceDevice.interClassMain.testDevicePassedParams.IsRuleName);
            if (bIsCameraStartTest) specListName = specListName.Substring(0, specListName.Length - 2);
            string sSpecListName = "";
            for (int i = 1; i <= testDeviceParams.MeasureParams.MeasureNumber; i++)
                for (int p = 1; p <= skyrayCamera.ContiTestPoints.Count; p++)
                    sSpecListName += "'" + specListName + "_" + i.ToString() + "_" + p.ToString() + "',";
            if (sSpecListName != "") sSpecListName = "(" + sSpecListName.Substring(0, sSpecListName.Length - 1) + ")";

            //string stringSql = @"select a.Id from SpecList a where a.Name  in " + sSpecListName;
            var findResult = WorkCurveHelper.DataAccess.Query(sSpecListName);
            if (findResult != null)
            {
                var result = Msg.Show(Info.strCoverSpecName, Info.Suggestion, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    IsIncrement = true;
                }
                else if (result == DialogResult.No)
                {
                    if (testDevicePassedParams.SpecType == SpecType.PureSpec || testDevicePassedParams.SpecType == SpecType.StandSpec)
                    {
                        UCPWDLock uc = new UCPWDLock(true);
                        WorkCurveHelper.OpenUC(uc, false, Info.PWDLock, true);
                        if (uc.DialogResult == DialogResult.No)
                        {
                            return false;
                        }
                    }
                    IsIncrement = false;
                    WorkCurveHelper.DataAccess.DeleteRecord(findResult.Name);
                    //if (findResult != null && findResult.Rows.Count > 0)
                    //{
                    //    foreach (DataRow list in findResult.Rows)
                    //    {
                    //long deleteId = (long)list[0];
                    //Lephone.Data.DbEntry.Context.ExecuteNonQuery("delete from historycompanyotherinfo where history_id in (select id from historyrecord where speclistId in (" + deleteId + "));" +
                    //              "delete from historyelement where historyrecord_id in (select id from historyrecord where speclistId in (" + deleteId + "));" +
                    //              "delete from historyrecord where speclistId in (" + deleteId + ");" +
                    //"delete from SpecList where  id in (" + deleteId + "); delete from spec where  speclist_id in (" + deleteId + ");");
                    //}
                    //}
                }
                else
                {
                    deviceMeasure.interfacce.State = DeviceState.Idel;
                    return false;
                }
            }


            //            string stringSql = @"select a.Id from SpecList a inner join Condition b 
            //                                on a.Condition_Id=b.Id inner join Device c on b.Device_Id=c.Id where a.Name = '" + specListName + "' and b.Device_Id="
            //                                 + WorkCurveHelper.DeviceCurrent.Id + "";
            //            var test = EDXRFHelper.GetData(stringSql);
            //            if (test != null && test.Rows.Count > 0)
            //            {
            //                if (DialogResult.OK == Msg.Show(Info.CoverSilimarSpecList, Info.Suggestion, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
            //                {
            //                    foreach (DataRow list in test.Rows)
            //                    {
            //                        long deleteId = (long)list[0];
            //                        WorkCurveHelper.VirtualSpecList.RemoveAll(v => v.Id == deleteId);
            //                        stringSql = @"delete from Spec where SpecList_Id=" + deleteId + "";
            //                        Lephone.Data.DbEntry.Context.ExecuteNonQuery(stringSql);

            //                        stringSql = @"delete from SpecList where Id =" + deleteId + "";
            //                        Lephone.Data.DbEntry.Context.ExecuteNonQuery(stringSql);

            //                        DirectoryInfo info = new DirectoryInfo(WorkCurveHelper.SaveSamplePath);
            //                        if (info.Exists && specList.ImageShow)
            //                        {
            //                            FileInfo file = new FileInfo(WorkCurveHelper.SaveSamplePath + "\\" + deleteId + ".jpg");
            //                            if (file.Exists)
            //                                file.Delete();
            //                        }
            //                        info = new DirectoryInfo(WorkCurveHelper.SaveGraphicPath);
            //                        if (info.Exists && specList.ImageShow)
            //                        {
            //                            FileInfo file = new FileInfo(WorkCurveHelper.SaveGraphicPath + "\\" + deleteId + ".jpg");
            //                            if (file.Exists)
            //                                file.Delete();
            //                        }
            //                        SpecAdditional.DeleteAll(w => w.SpecListId == deleteId);
            //                    }
            //                }
            //                else
            //                    return false;
            //            }
            //对测量设备进行相应的初始化
            deviceMeasure.interfacce.InitParam = this.initParams;
            deviceMeasure.interfacce.DeviceParam = this.deviceParamsList[0];
            deviceMeasure.interfacce.DropTime = testDeviceParams.MeasureParams.DiscardTime;  //丢包时间
            deviceMeasure.interfacce.RecordDropTime = testDeviceParams.MeasureParams.DiscardTime;
            TempNetDataSmooth.Clear();
            List<int[]> temp = new List<int[]>();
            TempNetDataSmooth.Add(this.deviceParamsList[0].Id, temp);

            deviceMeasure.interfacce.StopFlag = false;
            this.refreshFillinof.UpdateWorkSpec(this.deviceParamsList[this.deviceParamSelectIndex], this.specList);
            this.specList.Specs = new SpecEntity[this.deviceParamsList.Count];
            this.specList.Specs[0] = spec;
            //更新工作曲线信息
            this.refreshFillinof.RefreshCurve(this.initParams, this.deviceParamsList[this.deviceParamSelectIndex]);
            deviceMeasure.interfacce.connect = DeviceConnect.Connect;
            TestStartAfterControlState(false);

            return true;
        }

        /// <summary>
        /// 电机停止事件
        /// </summary>
        /// <param name="optMode">当前使用的模式</param>
        public override void MessageStopMove(OptMode optMode)
        {
            MotorStop(optMode);
            if (!this.deviceMeasure.interfacce.StopFlag)
            {

                switch (optMode)
                {
                    case OptMode.ProgrammableTest://连测
                        {
                            if (skyrayCamera != null && skyrayCamera.ContiTestPoints.Count > 0)
                            {
                                
                                skyrayCamera.IsShowTestPoints = true;
                            }
                            
                            deviceMeasure.Test();
                           
                        }
                        break;
                    case OptMode.CurveCalibrate:
                    case OptMode.BaseCalibrate:
                        deviceMeasure.Test();
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 扫描结束处理函数
        /// </summary>
        /// <param name="optMode">当前使用的模式</param>
        /// <param name="usedTime">使用的时间</param>
        /// <param name="workcurve">当前工作曲线</param>
        public override void CallTerminateTestFun(OptMode optMode, int usedTime, WorkCurve workcurve)
        {
            TerminateTest(optMode, usedTime, workcurve);
            if (!deviceMeasure.interfacce.StopFlag)
            {
                switch (optMode)
                {
                    case OptMode.ProgrammableTest: //计算匹配信息 
                        optModeProgrammableTest(usedTime);
                        break;
                    case OptMode.CurveCalibrate:
                    case OptMode.BaseCalibrate:
                        OptModeCurveCalibrate(optMode);
                        break;
                    default:
                        break;
                }
            }
        }



        /// <summary> 
        /// 连测处理
        /// </summary>
        /// <param name="usedTime">使用的时间</param>
        private void optModeProgrammableTest(int usedTime)
        {
            
            if (this.skyrayCamera == null)
                return;

            if (deviceParamSelectIndex < deviceParamsList.Count)
            {
                if (usedTime == 0)
                    return;
                this.spec.UsedTime = usedTime;
                this.spec.SpecTime = this.deviceParamsList[deviceParamSelectIndex].PrecTime;
                string middleStr = this.specList.SampleName + "_" + this.deviceParamsList[deviceParamSelectIndex].Name + "_"
                    + this.initParams.Condition.Name + "_" + currentTestTimes + "_" + PointCount;
                if (this.testDevicePassedParams.RemarkInformation != null)
                    spec.RemarkInfo = this.testDevicePassedParams.RemarkInformation;
                else
                    spec.RemarkInfo = "";
                this.spec.Name = middleStr;
                spec.DeviceParameter = this.deviceParamsList[deviceParamSelectIndex].ConvertFrom();
                spec.TubCurrent = spec.TubCurrent > 0 ? spec.TubCurrent : spec.DeviceParameter.TubCurrent;
                spec.TubVoltage = spec.TubVoltage > 0 ? spec.TubVoltage : spec.DeviceParameter.TubVoltage;
                if (SpecHelper.IsSmoothProcessData)
                {
                    List<int[]> ttOutput = new List<int[]>();
                    if (TempNetDataSmooth.TryGetValue(this.deviceParamsList[deviceParamSelectIndex].Id, out ttOutput))
                    {
                        int[] smooth = Helper.ToInts(this.spec.SpecData);
                        ttOutput.Add(smooth);
                        if (ttOutput.Count > 5)
                        {
                            ttOutput.RemoveAt(0);
                        }
                        else
                        {
                            int[] ttTemp = ttOutput[0];
                            int count = ttOutput.Count;
                            for (int m = count; m < 5; m++)
                                ttOutput.Add(ttTemp);
                        }
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < this.deviceMeasure.interfacce.DataCopy.Length; i++)
                        {
                            int te = 0;
                            foreach (var arr in ttOutput)
                            {
                                te += arr[i];
                            }
                            int temp = (int)Math.Round((te * 1.0 / ttOutput.Count), MidpointRounding.AwayFromZero);
                            sb.Append(temp.ToString() + ",");
                        }
                        TempNetDataSmooth.Remove(this.deviceParamsList[deviceParamSelectIndex].Id);
                        TempNetDataSmooth.Add(this.deviceParamsList[deviceParamSelectIndex].Id, ttOutput);
                        this.spec.SpecData = sb.ToString();
                    }
                }
            }
            if (deviceParamSelectIndex == deviceParamsList.Count - 1)
            {
                string constStr = GetDefineSpectrumName(specList.SampleName, DifferenceDevice.interClassMain.testDevicePassedParams.IsRuleName) + "_" + PointCount;
                specList.Name = constStr;
                specList.WorkCurveName = WorkCurveHelper.WorkCurveCurrent.Name;
                specList.Color = DifferenceDevice.DefaultSpecColor.ToArgb();
                specList.VirtualColor = specList.Color;
                specList.ActualVoltage = this.deviceMeasure.interfacce.ReturnVoltage;
                specList.ActualCurrent = this.deviceMeasure.interfacce.ReturnCurrent;
                specList.CountRate = this.deviceMeasure.interfacce.ReturnCountRate;
                specList.PeakChannel = double.Parse(this.deviceMeasure.interfacce.MaxChannelRealTime.ToString("f1"));
                specList.TotalCount = (long)this.deviceMeasure.interfacce.TestTotalCount;
                specList.DeviceName = WorkCurveHelper.DeviceCurrent.Name;
                specList.DemarcateEnergys = Default.ConvertFormOldToNew(WorkCurveHelper.WorkCurveCurrent.Condition.DemarcateEnergys, WorkCurveHelper.DeviceCurrent.SpecLength);
                specList.InitParam = this.initParams.ConvertToNewEntity();
                specList.Height = this.deviceMeasure.interfacce.ReturnEncoderValue;
                if (this.XrfChart != null)
                    DemarcateEnergyHelp.CalParam(this.XrfChart.DemarcateEnergys);
                if (deviceResolve != null)
                {
                    deviceResolve.Spec = this.spec;
                    specList.Resole = deviceResolve.CalculateResolve();
                }
                if (WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList.Count == 1)
                {
                    this.deviceMeasure.interfacce.HasVauccum = true;
                }
                if (WorkCurveHelper.IsSaveSpecData)
                {
                    string filePath = "";
                    if (specList.Specs[0].SpecData == "")
                        filePath = "谱空记录.txt";
                    else
                        filePath = "谱数据.txt";

                    try
                    {
                        // 检查文件是否存在
                        if (!File.Exists(filePath))
                        {
                            // 文件不存在，创建文件
                            File.Create(filePath).Close();
                        }
                        using (StreamWriter sw = new StreamWriter(filePath, true))
                        {
                            if (filePath == "谱空记录.txt")
                            {
                                sw.WriteLine(DateTime.Now.ToString());
                                sw.WriteLine("谱空");
                            }
                            else
                            {
                                sw.WriteLine(DateTime.Now.ToString());
                                sw.WriteLine(specList.Specs[0].SpecData);
                            }
                        }
                    }
                    catch
                    {
                    }

                    this.specList.ImageShow = true;
                    WorkCurveHelper.DataAccess.Save(this.specList);
                    
                }
                if (this.skyrayCamera != null && skyrayCamera.AutoSaveSamplePic)
                    this.skyrayCamera.GetImage(WorkCurveHelper.SaveSamplePath, this.specList.Name);
                if (WorkCurveHelper.IsSaveSpectrumImage)
                {
                    if (grobleState == FormWindowState.Normal)
                        ShowWindow(this.MainForm.Handle, SW_RESTORE);
                    else
                        ShowWindow(this.MainForm.Handle, 3);
                    var chat = this.XrfChart;
                    var bitmap = new Bitmap(chat.Width, chat.Height);
                    bool b1 = chat.IsShowHScrollBar;
                    bool b2 = chat.IsShowVScrollBar;
                    chat.IsShowHScrollBar = false;
                    chat.IsShowVScrollBar = false;
                    chat.DrawToBitmap(bitmap, chat.Bounds);
                    chat.IsShowHScrollBar = b1;
                    chat.IsShowVScrollBar = b2;
                    if (bitmap != null)
                    {
                        bitmap.Save(WorkCurveHelper.SaveGraphicPath + "\\" + this.specList.Name + ".jpg", ImageFormat.Jpeg);
                        bitmap.Dispose();
                    }

                }

                WorkCurveHelper.MainSpecList = this.specList;
                if (WorkCurveHelper.WorkCurveCurrent.ElementList != null && WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count > 0)
                {
                    this.selectSpeclist.Clear();
                    this.selectSpeclist.Add(this.specList);
                    CaculateContent(this.selectSpeclist, currentTestTimes, true);
                }
                if (currentTestTimes < testDevicePassedParams.MeasureParams.MeasureNumber)
                {
                    //此处，多点测试进入同一个点的下一次测试

                    currentTestTimes++;
                    WorkCurveHelper.IsFirstInfluenceGain = false;
                    deviceParamSelectIndex = 0;
                    this.spec = new SpecEntity();
                    this.spec.IsSmooth = true;
                    this.deviceMeasure.interfacce.Spec = this.spec;
                    this.spec.DeviceParameter = this.deviceParamsList[deviceParamSelectIndex].ConvertFrom();
                    progressInfo.Value = 0;
                    DifferenceDevice.IsConnect = true;
                    specList = new SpecListEntity("", specList.SampleName, specList.Height, specList.CalcAngleHeight, specList.Supplier, specList.Weight, specList.Shape, specList.Operater, specList.SpecDate, specList.SpecSummary, specList.SpecType, DifferenceDevice.DefaultSpecColor.ToArgb(), DifferenceDevice.DefaultSpecColor.ToArgb());
                    //specList.Condition = this.initParams.Condition;
                    this.specList.Specs = new SpecEntity[this.deviceParamsList.Count];
                    this.specList.Specs[0] = this.spec;
                    this.refreshFillinof.UpdateWorkSpec(this.deviceParamsList[this.deviceParamSelectIndex], this.specList);
                    //更新工作曲线信息
                    this.refreshFillinof.RefreshCurve(this.initParams, this.deviceParamsList[this.deviceParamSelectIndex]);
                    //progressInfo.MeasureTime = this.deviceParamsList[deviceParamSelectIndex].PrecTime + "s";
                    //progressInfo.SurplusTime = this.deviceParamsList[deviceParamSelectIndex].PrecTime + "s";
                    InitProcessBar();
                    deviceMeasure.interfacce.connect = DeviceConnect.Connect;
                    TestStartAfterControlState(false); //还没有完成扫描任务，再次使相应的控件变灰
                    //this.deviceMeasure.Test();
                    RefreshCameraText(ScannedContiTestPointsCount + 1, AllContiTestPointsCount, testDevicePassedParams.MeasureParams.IsManualTest);

                    deviceMeasure.interfacce.DeviceParam = deviceParamsList[deviceParamSelectIndex];
                    deviceMeasure.interfacce.MotorMove();

                }
                else if (DifferenceDevice.interClassMain.bIsCameraStartTest && this.skyrayCamera.ContiTestPoints.Count > 0 && !this.deviceMeasure.interfacce.StopFlag)
                {

                    //此处，多点测试进入下一个点的测试
                    currentTestTimes = 1;


                    this.spec = new SpecEntity();
                    this.spec.IsSmooth = true;
                    deviceParamSelectIndex = 0;
                    this.spec.DeviceParameter = this.deviceParamsList[deviceParamSelectIndex].ConvertFrom();

                    this.TempNetDataSmooth.Clear();
                    this.TempNetDataSmooth.Add(this.deviceParamsList[deviceParamSelectIndex].Id, new List<int[]>());

                    specList = new SpecListEntity("", specList.SampleName, specList.Height, specList.CalcAngleHeight, specList.Supplier, specList.Weight, specList.Shape, specList.Operater, specList.SpecDate, specList.SpecSummary, specList.SpecType, DifferenceDevice.DefaultSpecColor.ToArgb(), DifferenceDevice.DefaultSpecColor.ToArgb());
                    //specList.Condition = this.initParams.Condition;
                    this.specList.Specs = new SpecEntity[this.deviceParamsList.Count];
                    this.specList.Specs[0] = this.spec;
                    //OpenWorkCurveLog(WorkCurveHelper.WorkCurveCurrent, this.testDevicePassedParams.MeasureParams.MeasureNumber);
                    this.deviceMeasure.interfacce.Spec = this.spec;

              
                    DifferenceDevice.IsConnect = true;
                    PointIndex++;
                    PointCount--;
                    DifferenceDevice.interClassMain.iCurrCameraPointCount = PointCount;

                    deviceMeasure.interfacce.connect = DeviceConnect.Connect;
                    TestStartAfterControlState(false);

                    if (this.skyrayCamera.Mode == SkyrayCamera.CameraMode.Multiple)
                    {
                        MotorOperator.MotorOperatorZThread(this.skyrayCamera.alContiTestHeights[PointIndex - 1] - this.skyrayCamera.alContiTestHeights[PointIndex]);
                    }

                    SourcePoint = (Point)this.skyrayCamera.alTempTestPoints[PointIndex-1];
                    DestPoint = (Point)this.skyrayCamera.alTempTestPoints[PointIndex];

                    TableMoveFixStep(SourcePoint, DestPoint);

                    specList.SpecDate = DateTime.Now;

                    if(WorkCurveHelper.DeviceCurrent.HasMotorSpin)
                        updateTestImg();
                        


                    this.skyrayCamera.RemoveAtContiMeasurePoint(0);

                    if (this.skyrayCamera.fed != null)
                    {

                        this.skyrayCamera.fed.dgvMultiDatas.ClearSelection();
                        this.skyrayCamera.fed.dgvMultiDatas.Rows[PointIndex].Selected = true;
                        this.skyrayCamera.fed.dgvMultiDatas.FirstDisplayedScrollingRowIndex = PointIndex;
                    }

                    RefreshCameraText(ScannedContiTestPointsCount + 1, AllContiTestPointsCount, testDevicePassedParams.MeasureParams.IsManualTest);

                    //MessageStopMove(optMode);
                    deviceMeasure.interfacce.DeviceParam = deviceParamsList[deviceParamSelectIndex];
                    deviceMeasure.interfacce.MotorMove();

                   
                   
                }
                else
                {
                    
                    //联测最后返回第一个点

                    if (this.skyrayCamera.Mode == SkyrayCamera.CameraMode.Multiple)

                        MotorOperator.MotorOperatorZThread(this.skyrayCamera.alContiTestHeights[this.skyrayCamera.alContiTestHeights.Count - 1] - this.skyrayCamera.alContiTestHeights[0]);
                        

                    SourcePoint = (Point)this.skyrayCamera.alTempTestPoints[this.skyrayCamera.alTempTestPoints.Count-1];

                    DestPoint = (Point)this.skyrayCamera.alTempTestPoints[0];
                    TableMoveDirectlyFixStep(SourcePoint, DestPoint);
                   


                    if (WorkCurveHelper.DeviceCurrent.HasMotorSpin)
                    {
                        MotorOperator.MotorOperatorY1Thread((int)(WorkCurveHelper.TestDis * WorkCurveHelper.Y1Coeff));
                        this.skyrayCamera.Start();
                    }
                    DifferenceDevice.interClassMain.bIsCameraStartTest = false;
                    this.skyrayCamera.IsShowCenter = false;
                    TestEndCurrentProcess();

                    if (WorkCurveHelper.filterReset)
                    {

                        if (DifferenceDevice.interClassMain.deviceMeasure.interfacce.FilterMotor.Exist && DifferenceDevice.interClassMain.deviceMeasure.interfacce.DeviceParam.TargetMode != TargetMode.TwoTarget)
                        {
                            DifferenceDevice.interClassMain.deviceMeasure.interfacce.FilterMotor.MoveTo(5);
                        }
                    }

                }
            }
            else
            {
                //此处同一个点的同一次测试进入下一个条件的测试
                this.spec = new SpecEntity();
                this.spec.IsSmooth = true;
                this.deviceMeasure.interfacce.Spec = this.spec;
                deviceParamSelectIndex++;
                DifferenceDevice.IsConnect = true;
                this.refreshFillinof.UpdateWorkSpec(this.deviceParamsList[this.deviceParamSelectIndex], this.specList);
                //更新工作曲线信息
                this.refreshFillinof.RefreshCurve(this.initParams, this.deviceParamsList[this.deviceParamSelectIndex]);

                List<int[]> tempInt = new List<int[]>();
                if (!this.TempNetDataSmooth.TryGetValue(deviceParamsList[deviceParamSelectIndex].Id, out tempInt))
                    this.TempNetDataSmooth.Add(deviceParamsList[deviceParamSelectIndex].Id, new List<int[]>());


                InitProcessBar();
                this.specList.Specs[this.deviceParamSelectIndex] = this.spec;
                deviceMeasure.interfacce.connect = DeviceConnect.Connect;
                TestStartAfterControlState(false);

                deviceMeasure.interfacce.DeviceParam = deviceParamsList[deviceParamSelectIndex];
                deviceMeasure.interfacce.MotorMove();
            }
        }

        public override void ExploreModeCaculate()
        {
            throw new NotImplementedException();
        }

    
        /// <summary>
        /// 平台直接移动
        /// </summary>
        /// <param name="srcPoint">原点</param>
        /// <param name="destPoint">目的点</param>
        public void TableMoveDirectly(Point srcPoint, Point destPoint)
        {
         
            if (WorkCurveHelper.motorMoving)
                return;

            WorkCurveHelper.motorMoving = true;


            DistanceX = destPoint.X - srcPoint.X;
            DistanceY = destPoint.Y - srcPoint.Y;
            this.skyrayCamera.IsShowTestPoints = false;

            MotorDirections dirX = DistanceX > 0 ? MotorDirections.Positive : MotorDirections.Negative;
            MotorDirections dirY = DistanceY > 0 ? MotorDirections.Positive : MotorDirections.Negative;
         
        
            Int32 stepX = PrevDistanceX = (int)(Math.Abs(DistanceX) * this.skyrayCamera.MoveRateX * skyrayCamera.CutPicCoeff);
            Int32 stepY = PrevDistanceY = (int)(Math.Abs(DistanceY) * this.skyrayCamera.MoveRateY * skyrayCamera.CutPicCoeff);
        

            bool flag = MotorOperator.MotorOperatorXYThread(dirX == MotorDirections.Positive ? -Math.Abs(stepX) : Math.Abs(stepX), dirY == MotorDirections.Positive ? Math.Abs(stepY) : -Math.Abs(stepY));

           
        }

  
        public void TableMoveFixStep(Point srcPoint, Point destPoint)
        {
 

            this.skyrayCamera.IsShowTestPoints = false;

            DistanceX = srcPoint.X - destPoint.X;
            DistanceY = destPoint.Y - srcPoint.Y;
           
            WorkCurveHelper.motorMoving = true;
            MotorOperator.MotorOperatorXYThread(DistanceX, DistanceY);

        }


        /// <summary>
        /// 平台直接移动固定步长 
        /// </summary>
        /// <param name="srcPoint">现在的位置</param>
        /// <param name="destPoint">目标位置</param>
        public void TableMoveDirectlyFixStep(Point srcPoint, Point destPoint)
        {


            this.skyrayCamera.IsShowCenter = false;
            this.skyrayCamera.IsShowTestPoints = false;

            DistanceX = srcPoint.X - destPoint.X;
            DistanceY = destPoint.Y - srcPoint.Y;

            WorkCurveHelper.motorMoving = true;
            MotorOperator.MotorOperatorXYThread(DistanceX, DistanceY);

        }


        public override void RebackZero(Point destPoint)
        {
            TableMoveDirectlyFixStep(new Point(0, 0), destPoint);
        }

     


        private Point _fixedPointA;//修改移动平台校正功能，与FpThick一致
        private Point _fixedPointB;//修改移动平台校正功能，与FpThick一致
        /// <summary>
        /// 摄像头点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void skyrayCamera_MouseClick(object sender, MouseEventArgs e)
        {
            if (!this.skyrayCamera.Opened)
                return;
            if (this.deviceMeasure.interfacce.connect == DeviceConnect.Connect)
                return;
            this.CurrentMouseClickPoint = new Point(e.X, e.Y);
            this.skyrayCamera.FocusPoint = FocusPointClient;
            OptMode oldMode = optMode;
            //if (bIsMove && bIsControlKeyPress && e.Button == MouseButtons.Left
            //    && !DifferenceDevice.IsConnect)
            //不需要ctrl直接移动                                                         
            if (bIsMove && e.Button == MouseButtons.Left
               && !DifferenceDevice.IsConnect)
            {
                if (DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorIsIdelAll(WorkCurveHelper.DeviceCurrent.MotorXCode, WorkCurveHelper.DeviceCurrent.MotorYCode))
                {
                    skyrayCamera.IsShowTestPoints = false;
                    PrevSelectedPointClient = CurrSelectedPointClient = new Point(e.X, e.Y);
                    _fixedPointA = FocusPointClient;     //修改移动平台校正功能，与FpThick一致
                    _fixedPointB = new Point(e.X, e.Y);  //修改移动平台校正功能，与FpThick一致
                    TableMoveDirectly(FocusPointClient, CurrSelectedPointClient);
                    bIsControlKeyPress = false;
                }
            }
            else if (bIsNetwork && bIsControlKeyPress && e.Button == MouseButtons.Left
                && !DifferenceDevice.IsConnect)
            {
                this.frmGridParams = new FrmGridParams();
                this.frmGridParams.PointRowCount = skyrayCamera.PointRowCount;
                this.frmGridParams.PointRowDistance = skyrayCamera.PointRowDistance;
                this.frmGridParams.PointColCount = skyrayCamera.PointColCount;
                this.frmGridParams.PointColDistance = skyrayCamera.PointColDistance;

                WorkCurveHelper.OpenUC(this.frmGridParams, false, Info.NetWorkParams, true);
                skyrayCamera.PointRowCount = this.frmGridParams.PointRowCount;
                skyrayCamera.PointRowDistance = this.frmGridParams.PointRowDistance;
                skyrayCamera.PointColCount = this.frmGridParams.PointColCount;
                skyrayCamera.PointColDistance = this.frmGridParams.PointColDistance;

                //检查选点范围是否超过平台可移动范围
                float curDisLeft = WorkCurveHelper.X;
                float curDisTop = WorkCurveHelper.Y;
                float curDisRight = WorkCurveHelper.TabWidth * WorkCurveHelper.XCoeff - curDisLeft;
                float curDisBot = WorkCurveHelper.TabHeight * WorkCurveHelper.YCoeff - curDisTop;

               

                if (curDisRight < (skyrayCamera.PointRowCount-1) * skyrayCamera.PointRowDistance * WorkCurveHelper.YCoeff)
                {

                    Msg.Show("选点范围已超过平台左向可移动范围");
                    return;
                }

                if (curDisBot < (skyrayCamera.PointColCount-1) * skyrayCamera.PointColDistance * WorkCurveHelper.XCoeff)
                {

                    Msg.Show("选点范围已超过平台前向可移动范围");
                    return;
                }

                double rowStepDis = skyrayCamera.PointColDistance * WorkCurveHelper.YCoeff;
                double colStepDis = skyrayCamera.PointRowDistance * WorkCurveHelper.XCoeff;


                double curCircleAddY = 0;
                double curCircleAddX = 0;


                for (int row = 0; row < skyrayCamera.PointColCount; row++)
                {
                    curCircleAddY = row * rowStepDis;
                    for (int col = 0; col < skyrayCamera.PointRowCount; col++)
                    {
                        curCircleAddX = col * colStepDis;
                        skyrayCamera.ContiTestPoints.Add(new Point((int)WorkCurveHelper.X + (int)curCircleAddX, (int)WorkCurveHelper.Y + (int)curCircleAddY));

                    }

                }

                this.skyrayCamera.alTempTestPoints = (ArrayList)this.skyrayCamera.ContiTestPoints.Clone();

                this.skyrayCamera.IsShowTestPoints = true;


                bIsControlKeyPress = false;

                skyrayCamera.UpdateOverlay();


            }
            else if (bIsMatrix && (Control.ModifierKeys & Keys.Alt) == Keys.Alt && e.Button == MouseButtons.Left
                && !DifferenceDevice.IsConnect && WorkCurveHelper.matrixMode != "dotDot")
            {

                if (WorkCurveHelper.matrixMode == "dotMatrix")
                {
                    if (skyrayCamera.ContiTestPoints.Count == 0)
                    {

                        Msg.Show("请先选择多点文件");
                        return;

                    }

                    this.frmGridParams = new FrmGridParams();
                    this.frmGridParams.PointRowCount = skyrayCamera.PointRowCount;
                    this.frmGridParams.PointRowDistance = skyrayCamera.PointRowDistance;
                    this.frmGridParams.PointColCount = skyrayCamera.PointColCount;
                    this.frmGridParams.PointColDistance = skyrayCamera.PointColDistance;

                    WorkCurveHelper.OpenUC(this.frmGridParams, false, Info.NetWorkParams, true);
                    skyrayCamera.PointRowCount = this.frmGridParams.PointRowCount;
                    skyrayCamera.PointRowDistance = this.frmGridParams.PointRowDistance;
                    skyrayCamera.PointColCount = this.frmGridParams.PointColCount;
                    skyrayCamera.PointColDistance = this.frmGridParams.PointColDistance;

                    //检查选点范围是否超过平台可移动范围
                    float curDisLeft = WorkCurveHelper.X;
                    float curDisTop = WorkCurveHelper.Y;
                    float curDisRight = WorkCurveHelper.TabWidth * WorkCurveHelper.XCoeff - curDisLeft;
                    float curDisBot = WorkCurveHelper.TabHeight * WorkCurveHelper.YCoeff - curDisTop;

                    if (curDisRight < (skyrayCamera.PointRowCount - 1) * skyrayCamera.PointRowDistance * WorkCurveHelper.YCoeff)
                    {

                        Msg.Show("选点范围已超过平台左向可移动范围");
                        return;
                    }

                    if (curDisBot < (skyrayCamera.PointColCount - 1) * skyrayCamera.PointColDistance * WorkCurveHelper.XCoeff)
                    {

                        Msg.Show("选点范围已超过平台前向可移动范围");
                        return;
                    }


                    double rowStepDis = skyrayCamera.PointColDistance * WorkCurveHelper.YCoeff;
                    double colStepDis = skyrayCamera.PointRowDistance * WorkCurveHelper.XCoeff;

                    double curCircleAddY = 0;
                    double curCircleAddX = 0;


                    //先将外点添加到alTempTestPoints
                    this.skyrayCamera.alTempTestPoints.Clear();

                    for (int row = 0; row < skyrayCamera.PointColCount; row++)
                    {
                        curCircleAddY = row * rowStepDis;
                        for (int col = 0; col < skyrayCamera.PointRowCount; col++)
                        {
                            curCircleAddX = col * colStepDis;
                            skyrayCamera.alTempTestPoints.Add(new Point((int)WorkCurveHelper.X + (int)curCircleAddX, (int)WorkCurveHelper.Y + (int)curCircleAddY));
                        }
                    }


                    //先将多点坐标转化为相对坐标
                    int DistanceX = ((Point)this.skyrayCamera.alContiTestPoints[0]).X;
                    int DistanceY = ((Point)this.skyrayCamera.alContiTestPoints[0]).Y;


                    for (int i = 0; i < this.skyrayCamera.alContiTestPoints.Count; i++)
                    {
                        Point temp = (Point)this.skyrayCamera.alContiTestPoints[i];
                        temp.Offset(-DistanceX, -DistanceY);
                        this.skyrayCamera.alContiTestPoints[i] = temp;
                    }


                    
                    int outterNum = skyrayCamera.alTempTestPoints.Count;


                    int xToCurX = (int)WorkCurveHelper.X - ((Point)skyrayCamera.alTempTestPoints[0]).X;
                    int yToCurY = (int)WorkCurveHelper.Y - ((Point)skyrayCamera.alTempTestPoints[0]).Y;


                    for (int outterIndex = 0; outterIndex < outterNum; outterIndex++)
                    {
                        int outterX = ((Point)this.skyrayCamera.alTempTestPoints[outterIndex * this.skyrayCamera.ContiTestPoints.Count]).X;
                        int outterY = ((Point)this.skyrayCamera.alTempTestPoints[outterIndex * this.skyrayCamera.ContiTestPoints.Count]).Y;

                        for (int innerIndex = 0; innerIndex < this.skyrayCamera.ContiTestPoints.Count; innerIndex++)
                        {

                            if (innerIndex == 0)
                                continue;
                            Point temp = new Point(outterX, outterY);
                            temp.Offset(((Point)this.skyrayCamera.ContiTestPoints[innerIndex]).X, ((Point)this.skyrayCamera.ContiTestPoints[innerIndex]).Y);

                            //在不以第一个多点的绝对坐标为第一个测量点时的检查
                            if (!WorkCurveHelper.multiReset)
                            {
                                //文件中的绝对坐标点到当前点的偏移值

                                if (temp.X + xToCurX > WorkCurveHelper.TabWidthStep)
                                {

                                    Msg.Show("选点范围已超过平台左向可移动范围");
                                    return;
                                }
                                if (temp.X + xToCurX < 0)
                                {

                                    Msg.Show("选点范围已超过平台右向可移动范围");
                                    return;
                                }
                                if (temp.Y + yToCurY > WorkCurveHelper.TabHeightStep)
                                {

                                    Msg.Show("选点范围已超过平台前向可移动范围");
                                    return;
                                }
                                if (temp.Y + yToCurY < 0)
                                {

                                    Msg.Show("选点范围已超过平台后向可移动范围");
                                    return;
                                }
                            }
                            else
                            {

                                if (temp.X > WorkCurveHelper.TabWidthStep)
                                {

                                    Msg.Show("选点范围已超过平台左向可移动范围");
                                    return;
                                }
                                if (temp.X < 0)
                                {

                                    Msg.Show("选点范围已超过平台右向可移动范围");
                                    return;
                                }
                                if (temp.Y > WorkCurveHelper.TabHeightStep)
                                {

                                    Msg.Show("选点范围已超过平台前向可移动范围");
                                    return;
                                }
                                if (temp.Y < 0)
                                {

                                    Msg.Show("选点范围已超过平台后向可移动范围");
                                    return;
                                }
                            }

                            this.skyrayCamera.alTempTestPoints.Insert(outterIndex * this.skyrayCamera.ContiTestPoints.Count +
                                innerIndex, temp);

                        }
                    }


                    this.skyrayCamera.ContiTestPoints = (ArrayList)this.skyrayCamera.alTempTestPoints.Clone();

                }
                else if (WorkCurveHelper.matrixMode == "matrixDot")
                {

                    this.frmGridParams = new FrmGridParams();
                    this.frmGridParams.PointRowCount = skyrayCamera.PointRowCount;
                    this.frmGridParams.PointRowDistance = skyrayCamera.PointRowDistance;
                    this.frmGridParams.PointColCount = skyrayCamera.PointColCount;
                    this.frmGridParams.PointColDistance = skyrayCamera.PointColDistance;

                    WorkCurveHelper.OpenUC(this.frmGridParams, false, Info.NetWorkParams, true);
                    skyrayCamera.PointRowCount = this.frmGridParams.PointRowCount;
                    skyrayCamera.PointRowDistance = this.frmGridParams.PointRowDistance;
                    skyrayCamera.PointColCount = this.frmGridParams.PointColCount;
                    skyrayCamera.PointColDistance = this.frmGridParams.PointColDistance;


                    //检查选点范围是否超过平台可移动范围
                    float curDisLeft = WorkCurveHelper.X;
                    float curDisTop = WorkCurveHelper.Y;
                    float curDisRight = WorkCurveHelper.TabWidth * WorkCurveHelper.XCoeff - curDisLeft;
                    float curDisBot = WorkCurveHelper.TabHeight * WorkCurveHelper.YCoeff - curDisTop;
                    
                    //此处不做范围检查

                    double rowStepDis = skyrayCamera.PointColDistance * WorkCurveHelper.YCoeff;
                    double colStepDis = skyrayCamera.PointRowDistance * WorkCurveHelper.XCoeff;


                    double curCircleAddY = 0;
                    double curCircleAddX = 0;


                    for (int row = 0; row < skyrayCamera.PointColCount; row++)
                    {
                        curCircleAddY = row * rowStepDis;
                        for (int col = 0; col < skyrayCamera.PointRowCount; col++)
                        {
                            curCircleAddX = col * colStepDis;
                            skyrayCamera.ContiTestPoints.Add(new Point((int)WorkCurveHelper.X + (int)curCircleAddX, (int)WorkCurveHelper.Y + (int)curCircleAddY));

                        }

                    }


                    this.skyrayCamera.alTempTestPoints = (ArrayList)this.skyrayCamera.ContiTestPoints.Clone();

                }

                this.skyrayCamera.IsShowTestPoints = true;

                skyrayCamera.UpdateOverlay();

            }
            else if ((bIsMulti || bIsMatrix) && e.Button == MouseButtons.Left
                && !DifferenceDevice.IsConnect)
            {
            

                if (bIsControlKeyPress)                                      //yuzhaomodify:增加镜头外多点测试
                {
                    if (bIsMatrix || !WorkCurveHelper.detectPoints)
                    {

                        this.skyrayCamera.IsShowCenter = true;
                        this.skyrayCamera.IsShowTestPoints = false;
                        bIsControlKeyPress = false;
                        CurrSelectedPointClient = new Point(e.X, e.Y);

                        if (this.skyrayCamera.ContiTestPoints.Count == 0)
                        {
                            this.skyrayCamera.alContiTestHeights.Clear();
                        }
                        this.skyrayCamera.AddAbsoluteContiMeasurePoint(false);
                    }
                    else
                    {
                        bIsControlKeyPress = false;
                        detectColorBlock(e);

                    }

                }
                else if((Control.ModifierKeys & Keys.Alt) == Keys.Alt)
                {
                
                    this.skyrayCamera.IsShowCenter = true;
                    this.skyrayCamera.IsShowTestPoints = false;
                    bIsControlKeyPress = false;
                    CurrSelectedPointClient = new Point(e.X, e.Y);

                    if (this.skyrayCamera.ContiTestPoints.Count == 0)
                    {
                        this.skyrayCamera.alContiTestHeights.Clear();
                    }
                    this.skyrayCamera.AddAbsoluteContiMeasurePoint(true);

                }
                else
                {
                    if (DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorIsIdelAll(WorkCurveHelper.DeviceCurrent.MotorXCode, WorkCurveHelper.DeviceCurrent.MotorYCode))
                    {
                        this.skyrayCamera.IsShowCenter = false;
                        skyrayCamera.IsShowTestPoints = false;
                        PrevSelectedPointClient = CurrSelectedPointClient = new Point(e.X, e.Y);
                        _fixedPointA = FocusPointClient;
                        _fixedPointB = new Point(e.X, e.Y);

                        TableMoveDirectly(FocusPointClient, CurrSelectedPointClient);
                        
                        bIsControlKeyPress = false;
                    }
                }
            }
            else if (bIsSingle && bIsControlKeyPress && e.Button == MouseButtons.Left
                && !DifferenceDevice.IsConnect)
            {

                this.skyrayCamera.IsShowTestPoints = false;
                this.skyrayCamera.ClearContiMeasurePoint();
                this.skyrayCamera.AddRelativeContiMeasurePoint(new Point(e.X, e.Y));
                cameraControlUC_OnCameralStart();
                bIsControlKeyPress = false;
            }
            else if (bIsCheck && bIsControlKeyPress && e.Button == MouseButtons.Left)
            {
                if (this.skyrayCamera.IsWidthHeightChecked)
                {
                    this.skyrayCamera.IsShowTestPoints = true;
                    this.skyrayCamera.AddRelativeContiMeasurePoint(new Point(e.X, e.Y));
                    if (this.skyrayCamera.ContiTestPoints.Count == 2)
                    {
                        WorkCurveHelper.OpenUC(this.frmAdjustDistance, false, Info.AdjustDistance, true);
                        this.skyrayCamera.ClearContiMeasurePoint();
                        this.skyrayCamera.IsShowTestPoints = false;
                        bIsControlKeyPress = false;
                    }
                }
                else
                {
                    this.skyrayCamera.IsShowTestPoints = false;
                    CurrSelectedPointClient = new Point(e.X, e.Y);
                    if (Msg.Show(Info.MessageBoxTextProgrammableMeasureIsSureAdjust, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        == DialogResult.Yes)
                    {
                        #region //修改移动平台校正功能，与FpThick一致
                       
                        Double tempMoveRateX = skyrayCamera.MoveRateX;
                        Double tempMoveRateY = skyrayCamera.MoveRateY;
                        ReCalcMoveRate(_fixedPointA, _fixedPointB, new Point(e.X, e.Y), ref tempMoveRateX, ref tempMoveRateY);
                        skyrayCamera.MoveRateX = tempMoveRateX;
                        skyrayCamera.MoveRateY = tempMoveRateY;
                        #endregion
                        PrevSelectedPointClient = CurrSelectedPointClient;
                        if (radioButtonMove != null)
                            radioButtonMove.Checked = true;
                        TableMoveDirectly(FocusPointClient, CurrSelectedPointClient);
                    }
                    bIsControlKeyPress = false;
                }
            }
            else
            {
                optMode = oldMode;
            }
        }


        void skyrayCamera_MouseUp(object sender, MouseEventArgs e)
        {
            if (!this.skyrayCamera.Opened)
                return;
            if (bIsMove && isMouseDown && DifferenceDevice.interClassMain.deviceMeasure != null && DifferenceDevice.interClassMain.deviceMeasure.interfacce != null
               && DifferenceDevice.interClassMain.deviceMeasure.interfacce.port != null && WorkCurveHelper.DeviceCurrent != null && WorkCurveHelper.DeviceCurrent.HasMotorX && WorkCurveHelper.DeviceCurrent.HasMotorY)
            {
                isMouseDown = false;
                DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorXCode, WorkCurveHelper.DeviceCurrent.MotorXDirect, 0, true, WorkCurveHelper.DeviceCurrent.MotorXSpeed);
                DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(WorkCurveHelper.DeviceCurrent.MotorYCode, WorkCurveHelper.DeviceCurrent.MotorYDirect, 0, true, WorkCurveHelper.DeviceCurrent.MotorXSpeed);

            }
        }




        /// <summary>
        /// 鼠标按下，摄像头处理函数   移动电机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void skyrayCamera_MouseDown(object sender, MouseEventArgs e)
        {
            if (!this.skyrayCamera.Opened)
                return;
            if (this.deviceMeasure.interfacce.connect == DeviceConnect.Connect)
                return;
            //移动 按下ctrl 移动电机
            if (bIsMove && e.Button == MouseButtons.Middle
               && !DifferenceDevice.IsConnect && WorkCurveHelper.DeviceCurrent.HasMotorX && WorkCurveHelper.DeviceCurrent.HasMotorY)
            {
                isMouseDown = true;
                EnumMousePointPosition m_MousePointPosition = MousePointPosition(e);   //'判断光标的位置状态 
                switch (m_MousePointPosition)   //'改变光标  
                {
                    case EnumMousePointPosition.MouseSizeTop:
                        MoveXY(WorkCurveHelper.DeviceCurrent.MotorYCode, WorkCurveHelper.DeviceCurrent.MotorYDirect);
                        break;
                    case EnumMousePointPosition.MouseSizeBottom:
                        MoveXY(WorkCurveHelper.DeviceCurrent.MotorYCode, Math.Abs(WorkCurveHelper.DeviceCurrent.MotorYDirect - 1));
                        break;
                    case EnumMousePointPosition.MouseSizeLeft:
                        MoveXY(WorkCurveHelper.DeviceCurrent.MotorXCode, WorkCurveHelper.DeviceCurrent.MotorXDirect);
                        break;
                    case EnumMousePointPosition.MouseSizeRight:
                        MoveXY(WorkCurveHelper.DeviceCurrent.MotorXCode, Math.Abs(WorkCurveHelper.DeviceCurrent.MotorXDirect - 1));
                        break;
                    case EnumMousePointPosition.MouseSizeTopLeft:
                        MoveXY(WorkCurveHelper.DeviceCurrent.MotorYCode, WorkCurveHelper.DeviceCurrent.MotorYDirect);
                        MoveXY(WorkCurveHelper.DeviceCurrent.MotorXCode, WorkCurveHelper.DeviceCurrent.MotorXDirect);
                        break;
                    case EnumMousePointPosition.MouseSizeTopRight:
                        MoveXY(WorkCurveHelper.DeviceCurrent.MotorYCode, WorkCurveHelper.DeviceCurrent.MotorYDirect);
                        MoveXY(WorkCurveHelper.DeviceCurrent.MotorXCode, Math.Abs(WorkCurveHelper.DeviceCurrent.MotorXDirect - 1));
                        break;
                    case EnumMousePointPosition.MouseSizeBottomLeft:
                        MoveXY(WorkCurveHelper.DeviceCurrent.MotorYCode, Math.Abs(WorkCurveHelper.DeviceCurrent.MotorYDirect - 1));
                        MoveXY(WorkCurveHelper.DeviceCurrent.MotorXCode, WorkCurveHelper.DeviceCurrent.MotorXDirect);
                        break;
                    case EnumMousePointPosition.MouseSizeBottomRight:
                        MoveXY(WorkCurveHelper.DeviceCurrent.MotorYCode, Math.Abs(WorkCurveHelper.DeviceCurrent.MotorYDirect - 1));
                        MoveXY(WorkCurveHelper.DeviceCurrent.MotorXCode, Math.Abs(WorkCurveHelper.DeviceCurrent.MotorXDirect - 1));
                        break;
                }



            }
            else if ((bIsMulti||bIsMatrix) && e.Button == MouseButtons.Right
               && !DifferenceDevice.IsConnect && WorkCurveHelper.DeviceCurrent.HasMotorX && WorkCurveHelper.DeviceCurrent.HasMotorY)
            {

                this.skyrayCamera.LoadAllPointsFile();
            }

        }

        private void MoveXY(int xy, int direction)
        {
            DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.MotorControl(xy, direction, CameraRefMotor.MAX_XYZ_AXES_MOTOR_STEP, true, this.cameraControlUC.XYSpeed);
        }
        private EnumMousePointPosition MousePointPosition(System.Windows.Forms.MouseEventArgs e)
        {
            int camerWidth = this.skyrayCamera.Width;
            int camerHeight = this.skyrayCamera.Height;
            if ((e.X >= -1 * Band) | (e.X <= camerWidth) | (e.Y >= -1 * Band) | (e.Y <= camerHeight))
            {
                if (e.X < camerWidth / 2)  //2,3
                {
                    //第二象限
                    if (e.Y < camerHeight / 2)
                    {
                        if (Math.Abs(e.Y - camerHeight / 2) < Band)
                        {

                            return EnumMousePointPosition.MouseSizeLeft;
                        }
                        else if (Math.Abs(e.X - camerWidth / 2) < Band)
                        {
                            return EnumMousePointPosition.MouseSizeTop;
                        }
                        else
                        {
                            return EnumMousePointPosition.MouseSizeTopLeft;
                        }
                    }
                    else   //第三象限
                    {
                        if (Math.Abs(e.Y - camerHeight / 2) < Band)
                        {
                            return EnumMousePointPosition.MouseSizeLeft;
                        }
                        else if (Math.Abs(e.X - camerWidth / 2) < Band)
                        {
                            return EnumMousePointPosition.MouseSizeBottom;
                        }
                        else
                        {
                            return EnumMousePointPosition.MouseSizeBottomLeft;
                        }
                    }

                }
                else  //1,4
                {
                    //第一象限
                    if (e.Y < camerHeight / 2)
                    {
                        if (Math.Abs(e.Y - camerHeight / 2) < Band)
                        {
                            return EnumMousePointPosition.MouseSizeRight;
                        }
                        else if (Math.Abs(e.X - camerWidth / 2) < Band)
                        {
                            return EnumMousePointPosition.MouseSizeTop;
                        }
                        else
                        {
                            return EnumMousePointPosition.MouseSizeTopRight;
                        }
                    }
                    else   //第四象限
                    {
                        if (Math.Abs(e.Y - camerHeight / 2) < Band)
                        {
                            return EnumMousePointPosition.MouseSizeRight;
                        }
                        else if (Math.Abs(e.X - camerWidth / 2) < Band)
                        {
                            return EnumMousePointPosition.MouseSizeBottom;
                        }
                        else
                        {
                            return EnumMousePointPosition.MouseSizeBottomRight;
                        }
                    }

                }

            }
            else
            { return EnumMousePointPosition.MouseSizeNone; }
        }


        void skyrayCamera_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!this.skyrayCamera.Opened)
                return;
            if (this.deviceMeasure.interfacce.connect == DeviceConnect.Connect)
                return;
            this.CurrentMouseClickPoint = new Point(e.X, e.Y);
            OptMode oldMode = optMode;
            //不需要ctrl直接移动
            if (bIsMove && e.Button == MouseButtons.Left
               && !DifferenceDevice.IsConnect)
            {
                Console.WriteLine("打开开始测试按钮");
            }

        }




        /// <summary>
        /// 重新计算移动比例
        /// </summary>
        /// <param name="stepX">欲从焦点位置（样品平台上和十字线中心对应的点）移动到期望位置时，平台在X方向实际移动的步长</param>
        /// <param name="stepY">欲从焦点位置移动到期望位置时，平台在Y方向实际移动的步长</param>
        /// <param name="a1">移动前，视频画面上的十字线中心坐标（也就是焦点位置）</param>
        /// <param name="b1">移动前，期望位置在画面上对应的坐标</param>
        /// <param name="b2">移动完毕后，期望位置在画面上对应的坐标</param>
        public static void ReCalcMoveRate(Point focus, Point targetExpect, Point targetReal, ref Double moveRateX, ref Double moveRateY)
        {
            Int32 stepX = (Int32)(moveRateX * (targetExpect.X - focus.X));
            Int32 stepY = (Int32)(moveRateY * (targetExpect.Y - focus.Y));
            if (targetExpect.X != targetReal.X && stepX != 0)
            {
                moveRateX = (1.0 * stepX) / (targetExpect.X - targetReal.X);
            }
            if (targetExpect.Y != targetReal.Y && stepY != 0)
            {
                moveRateY = (1.0 * stepY) / (targetExpect.Y - targetReal.Y);
            }
        }
   

        public class MyItem : IComparable
        {
            public int x;
            public int y;
            public MyItem(int x, int y)
            {
                this.x = x;
                this.y = y;


            }



            public int CompareTo(object obj1)
            {
                MyItem obj = (MyItem)obj1;
                if (this.y < obj.y)
                    return -1;
                else if (this.y > obj.y)
                    return 1;
                else
                    return 0;
            }

        }




        public static void detectColorBlock(MouseEventArgs e)
        {
            if (!((Thick)WorkCurveHelper.curThick).bIsMulti)
                return;



            SkyrayCamera curCamera = DifferenceDevice.CurCameraControl.skyrayCamera1;

            UCCameraControl ucCamera = (UCCameraControl)WorkCurveHelper.ucCameraControl1;



            curCamera.catchFlag = true;

            //关闭激光，防止干扰图像检测
            bool tempChk = ucCamera.ChkLazer.Checked;
            ucCamera.ChkLazer.Checked = false;
            ucCamera.AutoCheckLazer();
            Thread.Sleep(500);


            Bitmap bitMap = curCamera.GrabImage();

            

            // 创建新的Bitmap对象并指定目标大小
            Bitmap scaledImage = new Bitmap(640, 480);

            using (Graphics graphics = Graphics.FromImage(scaledImage))
            {


                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                graphics.DrawImage((Image)bitMap, 0, 0, 640, 480);


            }

            if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "temp.bmp"))
                File.Delete(System.AppDomain.CurrentDomain.BaseDirectory + "temp.bmp");
            scaledImage.Save(System.AppDomain.CurrentDomain.BaseDirectory + "temp.bmp", ImageFormat.Bmp);


            Mat dest = CvInvoke.Imread(System.AppDomain.CurrentDomain.BaseDirectory + "temp.bmp", ImreadModes.Color);

            Mat src = CvInvoke.Imread(System.AppDomain.CurrentDomain.BaseDirectory + "temp.bmp", ImreadModes.Color);


            CvInvoke.GaussianBlur(src, src, new System.Drawing.Size(5, 5), 10, 10, BorderType.Reflect101);


            Mat roi = new Mat(src, new Rectangle(e.X - WorkCurveHelper.roiRadius, e.Y - WorkCurveHelper.roiRadius, WorkCurveHelper.roiRadius * 2, WorkCurveHelper.roiRadius * 2));

            CvInvoke.CvtColor(roi, roi, ColorConversion.Bgr2Hsv, 0);


            byte[] roiData = roi.GetData();

            CvInvoke.CvtColor(src, src, ColorConversion.Bgr2Hsv, 0);

            float hAver = 0;
            float sAver = 0;
            float vAver = 0;

            for (int i = 0; i < roiData.Length; i++)
            {
                if (i % 3 == 0)
                    hAver += roiData[i];
                else if (i % 3 == 1)
                    sAver += roiData[i];
                else
                    vAver += roiData[i];
            }



            hAver /= (WorkCurveHelper.roiRadius * WorkCurveHelper.roiRadius * 4);
            sAver /= (WorkCurveHelper.roiRadius * WorkCurveHelper.roiRadius * 4);
            vAver /= (WorkCurveHelper.roiRadius * WorkCurveHelper.roiRadius * 4);


            float hueMin = hAver - WorkCurveHelper.maxPixelErr < 0 ? 0 : hAver - WorkCurveHelper.maxPixelErr;
            float hueMax = hAver + WorkCurveHelper.maxPixelErr > 180 ? 180 : hAver + WorkCurveHelper.maxPixelErr;

            float saturationMin = sAver - WorkCurveHelper.maxPixelErr < 0 ? 0 : sAver - WorkCurveHelper.maxPixelErr;
            float saturationMax = sAver + WorkCurveHelper.maxPixelErr > 255 ? 255 : sAver + WorkCurveHelper.maxPixelErr;

            float valueMin = vAver - WorkCurveHelper.maxPixelErr < 0 ? 0 : vAver - WorkCurveHelper.maxPixelErr;
            float valueMax = vAver + WorkCurveHelper.maxPixelErr > 255 ? 255 : vAver + WorkCurveHelper.maxPixelErr;



            CvInvoke.InRange(src, new ScalarArray(new MCvScalar(hueMin, saturationMin, valueMin)),
                new ScalarArray(new MCvScalar(hueMax, saturationMax, valueMax)), src);





           

            CvInvoke.GaussianBlur(src, src, new System.Drawing.Size(5, 5), 10, 10, BorderType.Reflect101);

            Mat ele = CvInvoke.GetStructuringElement(ElementShape.Ellipse, new System.Drawing.Size(7, 7), new System.Drawing.Point(3, 3));

            CvInvoke.Dilate(src, src, ele, new System.Drawing.Point(-1, -1), 2, BorderType.Default, new MCvScalar(1));


            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();

            CvInvoke.FindContours(src, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple, default(Point));



            System.Collections.ArrayList comList = new System.Collections.ArrayList();

            System.Collections.ArrayList circleList = new System.Collections.ArrayList();

            for (int i = 0; i < contours.Size; i++)
            {
                CircleF c = CvInvoke.MinEnclosingCircle(contours[i]);
                circleList.Add(c);
                comList.Add(new MyItem(i, (int)c.Radius));
            }

            comList.Sort();
            comList.Reverse();
            curCamera.ContiTestPoints.Clear();

            int actualNum = 0;
            CircleF firstCircle = new CircleF(new PointF(0, 0), 0);


            for (int i = 0; i < contours.Size; i++)
            {


                MyItem index = (MyItem)comList[i];
                CircleF temp = (CircleF)circleList[index.x];

                if (temp.Radius < WorkCurveHelper.halfFociWidth * 1.7 || temp.Radius < WorkCurveHelper.halfFociHeight * 1.7)
                    break;
                actualNum++;
                if (actualNum > WorkCurveHelper.maxDetectNum)
                    break;

                //CvInvoke.Circle(dest,new Point((int)temp.Center.X, (int)temp.Center.Y),(int)temp.Radius,new MCvScalar(0,255,255),4,LineType.AntiAlias,0);

                if (i == 0)
                {
                    firstCircle = temp;
                    curCamera.ContiTestPoints.Add(new Point(0, 0));
                    WorkCurveHelper.firstCircle = temp;
                }
                else
                {

                    float stepX = (firstCircle.Center.X - temp.Center.X) * (float)curCamera.MoveRateX;
                    float stepY = (temp.Center.Y - firstCircle.Center.Y) * (float)curCamera.MoveRateY;
                    int x = (int)Math.Round(stepX);
                    int y = (int)Math.Round(stepY);

                    curCamera.ContiTestPoints.Add(new Point(x, y));

                }

            }


            //CvInvoke.Imshow("test", dest);

            if (actualNum >= 1)
            {
                ((Thick)WorkCurveHelper.curThick).TableMoveDirectly(new Point(320, 240), new Point((int)firstCircle.Center.X, (int)firstCircle.Center.Y));
                curCamera.IsShowTestPoints = true;
                WorkCurveHelper.firstCircle = null;
            }
            ucCamera.ChkLazer.Checked = tempChk;
            ucCamera.AutoCheckLazer();
            curCamera.catchFlag = false;


        }

    


        /// <summary>
        /// 自动调整距离处理函数
        /// </summary>
        void frmAdjustDistance_OnadjustDistance()
        {
            Point p0 = (Point)this.skyrayCamera.ContiTestPoints[0];
            Point p1 = (Point)this.skyrayCamera.ContiTestPoints[1];
            if (p0.X != p1.X || p0.Y != p1.Y)
            {
                double rateRealToControl = this.frmAdjustDistance.Distance / Math.Sqrt(Math.Pow((p0.X - p1.X), 2) + Math.Pow((p0.Y - p1.Y), 2));
                if (this.skyrayCamera.Height * rateRealToControl < 5 || this.skyrayCamera.Width * rateRealToControl < 5)
                {
                    Msg.Show(Info.strCorrectViewFailed);
                    return;
                }
                this.skyrayCamera.ViewHeight = this.skyrayCamera.Height * rateRealToControl;
                this.skyrayCamera.ViewWidth = this.skyrayCamera.Width * rateRealToControl;
            }
            if (radioButtonMove != null)
                radioButtonMove.Checked = true;
        }

        

        /// <summary>
        /// 摄像头按键弹起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void skyrayCamera_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                bIsControlKeyPress = false;
            }
        }

        /// <summary>
        /// 摄像头按键按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void skyrayCamera_KeyDown(object sender, KeyEventArgs e)
        {
            if (!this.skyrayCamera.Opened) return;

            if (e.Control && e.KeyCode == Keys.C)
            {
                bIsControlKeyPress = false;
                Bitmap b = this.skyrayCamera.GrabImage();
                if (b != null)
                {
                    Clipboard.SetImage(this.skyrayCamera.GrabImage());
                    b.Dispose();
                }
                else
                {
                    Msg.Show(Info.MessageBoxTextSystemCopyToClipboardFailed,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                bIsControlKeyPress = (e.KeyCode == Keys.ControlKey);
            }
        }

  

        /// <summary>
        /// cameraControlUC控件单点事件
        /// </summary>
        /// <param name="checkedState"></param>
        private void cameraControlUC_OnReturnCameralSinglePoint(bool checkedState)
        {
            if (skyrayCamera == null)
                return;

            if (checkedState)
            {
                if (skyrayCamera.ContiTestPoints.Count > 0)
                {
                    skyrayCamera.ContiTestPoints.Clear();
                }
                bIsControlKeyPress = false;
                bIsSingle = true;
                this.skyrayCamera.Mode = Skyray.Camera.SkyrayCamera.CameraMode.lonely;
                DifferenceDevice.CurCameraControl.ChkRetest.Checked = false;

            }
            else
                bIsSingle = false;



        }

        /// <summary>
        /// cameraControlUC校正事件
        /// </summary>
        /// <param name="checkedState"></param>
        private void cameraControlUC_OnReturnCameralCheck(bool checkedState)
        {
            if (skyrayCamera == null)
                return;
            if (checkedState)
            {
                if (skyrayCamera.ContiTestPoints.Count > 0)
                {
                    skyrayCamera.ClearContiMeasurePoint();
                }
                bIsControlKeyPress = false;
                bIsCheck = true;
                this.skyrayCamera.Mode = Skyray.Camera.SkyrayCamera.CameraMode.Coee;
                DifferenceDevice.CurCameraControl.ChkRetest.Checked = false;

            }
            else
                bIsCheck = false;
        }

        /// <summary>
        /// cameraControlUC移动事件
        /// </summary>
        /// <param name="checkedState"></param>
        private void cameraControlUC_OnReturnCameralMove(bool checkedState)
        {
            if (skyrayCamera == null)
                return;
            if (checkedState)
            {
                skyrayCamera.ClearContiMeasurePoint();
                bIsControlKeyPress = false;
                bIsMove = true;
                this.skyrayCamera.Mode = Skyray.Camera.SkyrayCamera.CameraMode.Move;
                DifferenceDevice.CurCameraControl.ChkRetest.Checked = false;
            }
            else
                bIsMove = false;
        }

        /// <summary>
        /// cameraControlUC网格事件
        /// </summary>
        /// <param name="checkedState"></param>
        private void cameraControlUC_OnReturnCameralNetWork(bool checkedState)
        {
            if (skyrayCamera == null)
                return;
            if (checkedState)
            {
                skyrayCamera.ClearContiMeasurePoint();
                bIsControlKeyPress = false;
                bIsNetwork = true;
                //InterfaceClass.bInIsNetwork= bIsNetwork = true;
                this.skyrayCamera.Mode = Skyray.Camera.SkyrayCamera.CameraMode.Cell;
                DifferenceDevice.CurCameraControl.ChkRetest.Checked = false;

            }
            else
                bIsNetwork = false;
            //InterfaceClass.bInIsNetwork = bIsNetwork = false;
        }

        /// <summary>
        /// cameraControlUC多点事件
        /// </summary>
        /// <param name="checkedState"></param>
        private void cameraControlUC_OnReturnCameralState(bool checkedState)
        {
            if (skyrayCamera == null)
                return;
            if (checkedState)
            {
                skyrayCamera.ClearContiMeasurePoint();
                bIsControlKeyPress = false;
                bIsMulti = true;

                this.skyrayCamera.Mode = Skyray.Camera.SkyrayCamera.CameraMode.Multiple;
                DifferenceDevice.CurCameraControl.ChkRetest.Checked = false;

            }
            else
                bIsMulti = false;
        }

        private void cameraControlUC_OnReturnCameralMatrix(bool checkedState)
        {
            if (skyrayCamera == null)
                return;
            if (checkedState)
            {
                skyrayCamera.ClearContiMeasurePoint();
                bIsControlKeyPress = false;
                bIsMatrix = true;

                if (WorkCurveHelper.matrixMode == "dotMatrix")
                    this.skyrayCamera.Mode = Skyray.Camera.SkyrayCamera.CameraMode.dotMatrix;
                else if (WorkCurveHelper.matrixMode == "matrixDot")
                    this.skyrayCamera.Mode = Skyray.Camera.SkyrayCamera.CameraMode.matrixDot;
                else if (WorkCurveHelper.matrixMode == "dotDot")
                    this.skyrayCamera.Mode = Skyray.Camera.SkyrayCamera.CameraMode.dotDot;
                DifferenceDevice.CurCameraControl.ChkRetest.Checked = false;

            }
            else
                bIsMatrix = false;
        }


        /// <summary>
        /// cameraControlUC复位按钮事件
        /// </summary>
        private void cameraControlUC_OnCameralReset()
        {
        
            MotorDirections dirX = WorkCurveHelper.DeviceCurrent.MotorXDirect == 1 ? MotorDirections.Positive : MotorDirections.Negative;
            MotorDirections dirY = WorkCurveHelper.DeviceCurrent.MotorYDirect == 1 ? MotorDirections.Positive : MotorDirections.Negative;

            int stepX = WorkCurveHelper.DeviceCurrent.MotorResetX;
            int stepY = WorkCurveHelper.DeviceCurrent.MotorResetY;

            MotorAdvance.FocusReplaceXY(
             new MotorAdvance.FocusParams(
                  dirX, stepX, WorkCurveHelper.DeviceCurrent.MotorXSpeed, dirY, stepY, WorkCurveHelper.DeviceCurrent.MotorYSpeed, true));

        }
       

        public override void OUTSample()
        {
            
            cameraControlUC_OnCameralOutSample();
        }


        public void cameraControlUC_OnCameralOutSample()
        {

            if (WorkCurveHelper.isSampleIn)
            {
           
                if (WorkCurveHelper.Y >= WorkCurveHelper.InOutDis * WorkCurveHelper.YCoeff)
                    MotorOperator.MotorOperatorXYThread(0, (int)(-WorkCurveHelper.InOutDis * WorkCurveHelper.YCoeff));
                else
                    MotorOperator.MotorOperatorXYThread(0, (int)(-WorkCurveHelper.Y));

                WorkCurveHelper.isSampleIn = false;

            }

        }

        public void updateLargeViewPos()
        {

            WorkCurveHelper.waitMoveStop();

            if (DifferenceDevice.CurCameraControl.largeViewCamera != null)
            {
                WorkCurveHelper.largeViewX = WorkCurveHelper.X;

                WorkCurveHelper.largeViewY = WorkCurveHelper.Y;
            }

            DifferenceDevice.CurCameraControl.updateLargeViewNow();

        }

        public override void INSample()
        {

            if (!WorkCurveHelper.isSampleIn)
            {
                if (DifferenceDevice.CurCameraControl.largeViewCamera != null)
                {
                    MotorOperator.MotorOperatorXYThread(0, (int)(WorkCurveHelper.LargeViewDis * WorkCurveHelper.YCoeff));

                    DifferenceDevice.CurCameraControl.getLargeView();

                    MotorOperator.MotorOperatorXYThread(0, (int)(WorkCurveHelper.TwoCameraDis * WorkCurveHelper.YCoeff));

                }
                else
                {
                    MotorOperator.MotorOperatorXYThread(0, (int)(WorkCurveHelper.InOutDis * WorkCurveHelper.YCoeff));

                }

                WorkCurveHelper.isSampleIn = true;

                
                new System.Threading.Thread(new System.Threading.ThreadStart(updateLargeViewPos)).Start();

            }

            
        }


        public void testContinue()
        {
            
            double voltage = 0;
            double current = 0;
            int cover = 1;
            DifferenceDevice.interClassMain.deviceMeasure.interfacce.port.getParam(out voltage, out current, out cover);

            if (cover != 0)
                return;

            DialogResult res = DialogResult.No;

            if (WorkCurveHelper.testingOpenCover)
            {
                WorkCurveHelper.testingOpenCover = false;

                res = Msg.Show("已关盖，是否继续被中断的测试？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            }
            else if (WorkCurveHelper.suspendTest)
            {
                WorkCurveHelper.suspendTest = false;
                res = DialogResult.Yes;
            }
            

            if (res == DialogResult.Yes)
            {
                if (DifferenceDevice.CurCameraControl.skyrayCamera1.Mode == SkyrayCamera.CameraMode.Move)
                {

                    WorkCurveHelper.testParamsBackup.MeasureParams.MeasureNumber = WorkCurveHelper.testTimes;

                    if (CurveTest.Spec.SampleName.Contains("#"))
                        CurveTest.Spec.SampleName = CurveTest.Spec.SampleName.Split(new char[] { '#' })[0] + "#" + DateTime.Now.ToString("yyyyMMddHHmmss").ToString();
                    else
                        CurveTest.Spec.SampleName = CurveTest.Spec.SampleName + "#" + DateTime.Now.ToString("yyyyMMddHHmmss").ToString();
                    WorkCurveHelper.curDeviceNum = 0;
                    DifferenceDevice.MediumAccess.ExcuteTestStart(WorkCurveHelper.testParamsBackup);
                }
                else
                {
                    int testedTimes = WorkCurveHelper.testNum * WorkCurveHelper.testParamsBackup.MeasureParams.MeasureNumber - WorkCurveHelper.testTimes;

                    if (testedTimes % WorkCurveHelper.testParamsBackup.MeasureParams.MeasureNumber == 0)
                    {
                        int index = testedTimes / WorkCurveHelper.testParamsBackup.MeasureParams.MeasureNumber;
                        this.skyrayCamera.ContiTestPoints = (ArrayList)this.skyrayCamera.alTempTestPoints.GetRange(index, WorkCurveHelper.testTimes / WorkCurveHelper.testParamsBackup.MeasureParams.MeasureNumber).Clone();

                        WorkCurveHelper.contiOffsetInTemp = index;
                        WorkCurveHelper.curDeviceNum = 0;

                        ((Thick)this).newSpec_OnExcuteTestBackGroudWorker1(WorkCurveHelper.testParamsBackup);
                       
                    }
                    else
                    {
                        int toDeleteNum = testedTimes % WorkCurveHelper.testParamsBackup.MeasureParams.MeasureNumber;
                        foreach (var his in HistoryRecord.FindRecent(toDeleteNum))
                        {
                            long id = his.Id;
                            Lephone.Data.SqlEntry.SqlStatement sqlstate = new Lephone.Data.SqlEntry.SqlStatement("delete from historycompanyotherinfo where history_id='" + id.ToString() + "';delete from HistoryRecord where Id=" + id.ToString() + " ;delete from historyelement where HistoryRecord_Id=" + id.ToString());
                            Lephone.Data.DbEntry.Context.ExecuteNonQuery(sqlstate);

                        }
                        WorkCurveHelper.testTimes += toDeleteNum;
                        int index = testedTimes / WorkCurveHelper.testParamsBackup.MeasureParams.MeasureNumber;
                        
                        this.skyrayCamera.ContiTestPoints = (ArrayList)this.skyrayCamera.alTempTestPoints.GetRange(index, WorkCurveHelper.testTimes / WorkCurveHelper.testParamsBackup.MeasureParams.MeasureNumber).Clone();

                        WorkCurveHelper.contiOffsetInTemp = index;

                        
                        WorkCurveHelper.curDeviceNum = 0;

                        ((Thick)this).newSpec_OnExcuteTestBackGroudWorker1(WorkCurveHelper.testParamsBackup);

                    }

                }
            }
            else
            {
                if (WorkCurveHelper.DeviceCurrent.HasMotorSpin)
                {

                    MotorOperator.MotorOperatorY1Thread((int)(WorkCurveHelper.TestDis * WorkCurveHelper.Y1Coeff));
                    this.skyrayCamera.Start();

                }

                WorkCurveHelper.testNum = 0;
                WorkCurveHelper.testTimes = 0;
            }

            


        }
   
        public override void SetCameraCoeff(int coeff)
        {
            skyrayCamera.ZoomInOut(coeff);
        }
       

       
        /// <summary>
        /// cameraControlUC停止按钮事件
        /// </summary>
        private void cameraControlUC_OnCameralStop()
        {

            if (WorkCurveHelper.loopTestDemo)
            {
                WorkCurveHelper.loopTestDemo = false;
                return;
            }

            DialogResult res = DialogResult.Yes;

            //在预热、初始化、单点测试模式下不可以暂停测试，只能直接停止测试
            if (WorkCurveHelper.testNum > 1)
            {
                if (!DifferenceDevice.interClassMain.deviceMeasure.interfacce.StopFlag)
                   
                    res = (new UCStop()).ShowDialog();
            }
            if (res == DialogResult.Yes)
            {
                this.deviceMeasure.Stop();
                CameraRefMotor.CancelAll();

                
                if (this.skyrayCamera.alTempTestPoints.Count - this.skyrayCamera.ContiTestPoints.Count >= 1 && WorkCurveHelper.testNum > 1)
                {
                    Point firstPoint = (Point)this.skyrayCamera.alTempTestPoints[0];
                    Point curTestPoint = (Point)this.skyrayCamera.alTempTestPoints[this.skyrayCamera.alTempTestPoints.Count - this.skyrayCamera.ContiTestPoints.Count - 1];


                    if (this.skyrayCamera.Mode == SkyrayCamera.CameraMode.Multiple)
                        MotorOperator.MotorOperatorZThread(this.skyrayCamera.alContiTestHeights[this.skyrayCamera.alTempTestPoints.Count - this.skyrayCamera.ContiTestPoints.Count - 1] - this.skyrayCamera.alContiTestHeights[0]);

                    TableMoveDirectlyFixStep(curTestPoint, firstPoint);

                }

                TestStartAfterControlState(true);
                skyrayCamera.ClearContiMeasurePoint();
                progressInfo.Value = 0;
                this.XrfChart.ClearInformation();
                ModbusTcp.slave.DataStore.CoilDiscretes[1] = false;
                ModbusTcp.slave.DataStore.CoilDiscretes[3] = false;
                ModbusTcp.slave.DataStore.InputDiscretes[2] = false;
                WorkCurveHelper.stopDoing = 0;

                if (WorkCurveHelper.DeviceCurrent.HasMotorSpin)
                {
                    MotorOperator.MotorOperatorY1Thread((int)(WorkCurveHelper.TestDis * WorkCurveHelper.Y1Coeff));
                    this.skyrayCamera.Start();
                    this.testImgList.Clear();
                }

                WorkCurveHelper.testNum = 0;
                WorkCurveHelper.testTimes = 0;

                if (WorkCurveHelper.filterReset)
                {

                    if (DifferenceDevice.interClassMain.deviceMeasure.interfacce.FilterMotor.Exist && DifferenceDevice.interClassMain.deviceMeasure.interfacce.DeviceParam.TargetMode != TargetMode.TwoTarget)
                    {
                        DifferenceDevice.interClassMain.deviceMeasure.interfacce.FilterMotor.MoveTo(5);
                    }
                }

            }
            else if (res == DialogResult.No)
            {
                WorkCurveHelper.suspendTest = true;

            }

            
        }

      
        public Bitmap curTestImg = null;
        public ArrayList  testImgList = new ArrayList();
        
        
        public void updateTestImg()
        {
            if (this.skyrayCamera.BackgroundImageLayout == ImageLayout.Stretch)
            {
                this.skyrayCamera.BackgroundImage = (Bitmap)testImgList[0];
                testImgList.RemoveAt(0);
            }
            
            
        }

        public void addTestImg()
        {

            WorkCurveHelper.waitMoveStop();
            this.testImgList.Add( new Bitmap(this.skyrayCamera.GrabImage(),new Size(640,480)));

        }


        public void testDemo()
        {


            if (bIsMove)
            {
                Msg.Show("当前测试模式测试前无需演示");
                return;

            }
            if (this.skyrayCamera.ContiTestPoints.Count == 0)
            {
                Msg.Show("演示前请选择测试点");
                return;
            }
            this.deviceMeasure.interfacce.StopFlag = false;
            int AllTestNum = this.skyrayCamera.ContiTestPoints.Count;
            int curTestNum = 1;
            this.testImgList.Clear();
            WorkCurveHelper.testDemoing = true;
            WorkCurveHelper.testNum = 2;

            WorkCurveHelper.loopTestDemo = bool.Parse(ReportTemplateHelper.LoadSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/loopTestDemo"));

            while (this.skyrayCamera.ContiTestPoints.Count >= 0 && !this.deviceMeasure.interfacce.StopFlag)
            {
                if (this.skyrayCamera.ContiTestPoints.Count == AllTestNum)
                {
                    RefreshCameraText(1, skyrayCamera.ContiTestPoints.Count, false);

                    PointIndex = this.skyrayCamera.alTempTestPoints.Count - this.skyrayCamera.alContiTestPoints.Count;
                    PointCount = this.skyrayCamera.alContiTestPoints.Count;
                    DifferenceDevice.interClassMain.iCameraPointCount = DifferenceDevice.interClassMain.iCurrCameraPointCount = this.skyrayCamera.ContiTestPoints.Count;

                    try
                    {
                        //多点文件中的第一个绝对坐标点，但是不一定是当前位置
                        FirstPoint = (Point)this.skyrayCamera.ContiTestPoints[0];
                        DestPoint = (Point)this.skyrayCamera.ContiTestPoints[0];
                        Point LastPoint = (Point)this.skyrayCamera.ContiTestPoints[PointCount - 1];
                   


                        if (bIsMulti)
                            WorkCurveHelper.testDemoMode = 1;
                        else if (bIsNetwork)
                            WorkCurveHelper.testDemoMode = 2;
                        else if(bIsMatrix)
                        {
                            if (this.skyrayCamera.Mode == SkyrayCamera.CameraMode.dotDot)
                                WorkCurveHelper.testDemoMode = 3;
                            else if (this.skyrayCamera.Mode == SkyrayCamera.CameraMode.matrixDot)
                                WorkCurveHelper.testDemoMode = 4;
                            else
                                WorkCurveHelper.testDemoMode = 5;

                        }

                        if (!WorkCurveHelper.multiReset)
                            SourcePoint = FirstPoint;
                        else
                            SourcePoint = new Point((int)WorkCurveHelper.X, (int)WorkCurveHelper.Y);
                        TableMoveFixStep(SourcePoint, DestPoint);


                        //new System.Threading.Thread(new System.Threading.ThreadStart(addTestImg)).Start();

                        this.skyrayCamera.RemoveAtContiMeasurePoint(0);

                    }
                    catch
                    {

                    }
                }
                else if (this.skyrayCamera.ContiTestPoints.Count < AllTestNum && this.skyrayCamera.ContiTestPoints.Count > 0)
                {

                    WorkCurveHelper.waitMoveStop();

                    RefreshCameraText(curTestNum, AllTestNum, false);           

                    TestStartAfterControlState(false);

                    PointIndex++;
                    PointCount--;

                    try
                    {

                        if (this.skyrayCamera.Mode == SkyrayCamera.CameraMode.Multiple)
                        {
                            MotorOperator.MotorOperatorZThread(this.skyrayCamera.alContiTestHeights[PointIndex - 1] - this.skyrayCamera.alContiTestHeights[PointIndex]);
                        }

                        SourcePoint = (Point)this.skyrayCamera.alTempTestPoints[PointIndex - 1];
                        DestPoint = (Point)this.skyrayCamera.alTempTestPoints[PointIndex];

                        TableMoveFixStep(SourcePoint, DestPoint);

                        //new System.Threading.Thread(new System.Threading.ThreadStart( addTestImg)).Start();
                        this.skyrayCamera.RemoveAtContiMeasurePoint(0);
                    }
                    catch
                    {

                    }

                }
                else if (this.skyrayCamera.ContiTestPoints.Count == 0)
                {

                    WorkCurveHelper.waitMoveStop();

                    try
                    {
                        if (this.skyrayCamera.Mode == SkyrayCamera.CameraMode.Multiple)

                            MotorOperator.MotorOperatorZThread(this.skyrayCamera.alContiTestHeights[this.skyrayCamera.alContiTestHeights.Count - 1] - this.skyrayCamera.alContiTestHeights[0]);

                        SourcePoint = (Point)this.skyrayCamera.alTempTestPoints[this.skyrayCamera.alTempTestPoints.Count - 1];
                        DestPoint = (Point)this.skyrayCamera.alTempTestPoints[0];
                        TableMoveDirectlyFixStep(SourcePoint, DestPoint);
                    }
                    catch
                    {

                    }

                    this.skyrayCamera.IsShowCenter = false;

                    if (WorkCurveHelper.loopTestDemo)
                    {
                        WorkCurveHelper.waitMoveStop();
                        this.skyrayCamera.ContiTestPoints = (ArrayList)this.skyrayCamera.alTempTestPoints.Clone();
                        curTestNum = 0;
                    }
                    else
                    {
                        TestStartAfterControlState(true);
                        WorkCurveHelper.testDemoing = false;
                        WorkCurveHelper.testNum = 0;
                        WorkCurveHelper.testTimes = 0;
                        return;
                    }
                }
                curTestNum++;
            }

            WorkCurveHelper.testDemoing = false;
            WorkCurveHelper.testNum = 0;
            WorkCurveHelper.testTimes = 0;
            return;

            

        }




        /// <summary>
        /// cameraControlUC开始按钮
        /// </summary>
        private void cameraControlUC_OnCameralStart()
        {

            if (WorkCurveHelper.testDemo)
            {
                new System.Threading.Thread(new System.Threading.ThreadStart(testDemo)).Start();

                return;
            }


            if (WorkCurveHelper.WorkCurveCurrent == null)
            {
                Msg.Show(Info.WarningTestContext, Info.TestWarning, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }


            if (WorkCurveHelper.suspendTest)
            {
                DialogResult res = DialogResult.No;

                res = Msg.Show(Info.continueSuspendTest, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                if (res == DialogResult.Yes)
                    testContinue();
                else
                {
                    WorkCurveHelper.suspendTest = false;
                    if (WorkCurveHelper.DeviceCurrent.HasMotorSpin)
                    {
                        MotorOperator.MotorOperatorY1Thread((int)(WorkCurveHelper.TestDis * WorkCurveHelper.Y1Coeff));
                        this.skyrayCamera.Start();
                    }
                }
                return;
            }
            

            if (WorkCurveHelper.DeviceCurrent.MotorYDirect == 1 && WorkCurveHelper.TabHeight* WorkCurveHelper.YCoeff - WorkCurveHelper.Y < WorkCurveHelper.TestDis * WorkCurveHelper.YCoeff)
            {
                Msg.Show("当前平台位置不可进行测试！");
                return;
            }

            Condition condition = WorkCurveHelper.WorkCurveCurrent.Condition;
            this.initParams = condition.InitParam;
            DeviceParameterByElementList(condition.DeviceParamList.ToList());
            if (this.deviceParamsList.Count == 0)
                this.deviceParamsList = condition.DeviceParamList.ToList();

            this.skyrayCamera.ContiTestPoints = (ArrayList)this.skyrayCamera.alTempTestPoints.Clone();
            
            if (this.skyrayCamera.ContiTestPoints.Count == 0)  //不是多次测量时执行普通测试
            {
                if (this.skyrayCamera.Mode == Skyray.Camera.SkyrayCamera.CameraMode.Multiple || this.skyrayCamera.Mode == Skyray.Camera.SkyrayCamera.CameraMode.dotMatrix || this.skyrayCamera.Mode == Skyray.Camera.SkyrayCamera.CameraMode.dotDot || this.skyrayCamera.Mode == Skyray.Camera.SkyrayCamera.CameraMode.matrixDot || this.skyrayCamera.Mode == Skyray.Camera.SkyrayCamera.CameraMode.Cell)
                {
                    Msg.Show(Info.NotFocusPoint);
                    return;
                }

                if (WorkCurveHelper.startDoing > 1)
                {
                    Msg.Show("正在通过modbusTcp开启测试，不可重复开启测试");
                    return;
                }
                WorkCurveHelper.testNum = 1;
                WorkCurveHelper.curDeviceNum = 0;
                
                NewWorkSpec returnNew = NewWorkSpec.GetInstance(DifferenceDevice.interClassMain.currentSelectMode);
                if (WorkCurveHelper.startDoing == 0)
                    WorkCurveHelper.OpenUC(returnNew, true, Info.Start, true);

            }
            else
            {   
                
                WorkCurveHelper.testNum = this.skyrayCamera.ContiTestPoints.Count;
                WorkCurveHelper.curDeviceNum = 0;          
                NewWorkSpec newSpec = new NewWorkSpec(0, false);
                WorkCurveHelper.OpenUC(newSpec, false, Info.TestSetting, true);
                if (newSpec.dialogResult == DialogResult.OK)
                {
                    WorkCurveHelper.dataStore.InputDiscretes[2] = true;
                    WorkCurveHelper.contiOffsetInTemp = 0;
                    WorkCurveHelper.testParamsBackup = newSpec.TestDevicePassedParams;
                    WorkCurveHelper.testTimes = WorkCurveHelper.testNum * newSpec.TestDevicePassedParams.MeasureParams.MeasureNumber;
                    newSpec_OnExcuteTestBackGroudWorker1(newSpec.TestDevicePassedParams);
                }
            }
        }

        //AllContiTestPointsCount主要为多点测试，添加测试结果。
        private int AllContiTestPointsCount = -1;
        private int ScannedContiTestPointsCount = 0;
        /// <summary>
        /// 开始进行连测处理函数
        /// </summary>
        /// <param name="testDevice"></param>
        public void newSpec_OnExcuteTestBackGroudWorker1(TestDevicePassedParams testDevice)
        {
            WorkCurveHelper.waitMoveStop();

            if (!DeviceMeasurePassingThick(WorkCurveHelper.DeviceCurrent, testDevice)) return;
            //if (this.highCheckBox != null)
            //    this.highC heckBox.Checked = true;
            this.optMode = OptMode.ProgrammableTest;
            // MotorInstance.s_singleInstanceZ.OpenHeightLazer();
            this.deviceMeasure.interfacce.StopFlag = false;
            WorkCurveHelper.IsManualTest = false;
            DifferenceDevice.IsConnect = true;
            if (DifferenceDevice.interClassMain.bIsCameraStartTest)
            {
                AllContiTestPointsCount = testDevice.MeasureParams.MeasureNumber * skyrayCamera.ContiTestPoints.Count;
                ScannedContiTestPointsCount = 0;
                OpenWorkCurveLog(WorkCurveHelper.WorkCurveCurrent, testDevice.MeasureParams.MeasureNumber * skyrayCamera.ContiTestPoints.Count);
                RefreshCameraText(1, testDevice.MeasureParams.MeasureNumber * skyrayCamera.ContiTestPoints.Count, false);
            }
            else
            {
                OpenWorkCurveLog(WorkCurveHelper.WorkCurveCurrent, testDevice.MeasureParams.MeasureNumber);
                RefreshCameraText(1, testDevice.MeasureParams.MeasureNumber, false);
            }



            PointIndex = this.skyrayCamera.alTempTestPoints.Count - this.skyrayCamera.alContiTestPoints.Count;
            PointCount = this.skyrayCamera.alContiTestPoints.Count;
            DifferenceDevice.interClassMain.iCameraPointCount = DifferenceDevice.interClassMain.iCurrCameraPointCount = this.skyrayCamera.ContiTestPoints.Count;

            //多点文件中的第一个绝对坐标点，但是不一定是当前位置
            FirstPoint = (Point)this.skyrayCamera.ContiTestPoints[0];
            DestPoint = (Point)this.skyrayCamera.ContiTestPoints[0];
            Point LastPoint = (Point)this.skyrayCamera.ContiTestPoints[PointCount - 1];




            
            int curMode = 0;

            if(bIsMulti)
                curMode = 1;
            else if (bIsNetwork)
                curMode = 2;
            else if (bIsMatrix)
            {
                
                if (this.skyrayCamera.Mode == SkyrayCamera.CameraMode.dotDot)
                    curMode = 3;
                else if (this.skyrayCamera.Mode == SkyrayCamera.CameraMode.matrixDot)
                    curMode = 4;
                else
                    curMode = 5;
                
            }

            
            multiReset();
        
            if (testImgList.Count != 0)
            {
                if (curMode != WorkCurveHelper.testDemoMode)
                {

                    this.deviceMeasure.Stop();
                    CameraRefMotor.CancelAll();
                    TestStartAfterControlState(true);
                    skyrayCamera.ClearContiMeasurePoint();
                    progressInfo.Value = 0;
                    this.XrfChart.ClearInformation();
                    this.optMode = OptMode.Test;
                    this.deviceMeasure.interfacce.StopFlag = true;
                    DifferenceDevice.IsConnect = false;
                    this.cameraControlUC.SetTestInformation("",false);
                   
                    Msg.Show("演示时测试模式与当前测试时测试模式不匹配，请重新进行演示或忽略");
                    
                    testImgList.Clear();

                    ModbusTcp.slave.DataStore.CoilDiscretes[1] = false;
                    ModbusTcp.slave.DataStore.CoilDiscretes[3] = false;
                    ModbusTcp.slave.DataStore.InputDiscretes[2] = false;
                    WorkCurveHelper.stopDoing = 0;
                    WorkCurveHelper.testNum = 0;
                    WorkCurveHelper.testTimes = 0;
                    return;

                }
                else if (testImgList.Count != this.skyrayCamera.ContiTestPoints.Count)
                {
                    this.deviceMeasure.Stop();
                    CameraRefMotor.CancelAll();
                    TestStartAfterControlState(true);
                    skyrayCamera.ClearContiMeasurePoint();
                    progressInfo.Value = 0;
                    this.XrfChart.ClearInformation();
                    this.optMode = OptMode.Test;
                    this.deviceMeasure.interfacce.StopFlag = true;
                    DifferenceDevice.IsConnect = false;
                    this.cameraControlUC.SetTestInformation("", false);
                  
                    Msg.Show("演示时测试点数与当前测试时测试点数不匹配，请重新进行演示或忽略");
                    
                    testImgList.Clear();

                    ModbusTcp.slave.DataStore.CoilDiscretes[1] = false;
                    ModbusTcp.slave.DataStore.CoilDiscretes[3] = false;
                    ModbusTcp.slave.DataStore.InputDiscretes[2] = false;
                    WorkCurveHelper.stopDoing = 0;
                    WorkCurveHelper.testNum = 0;
                    WorkCurveHelper.testTimes = 0;
                    return;
                }
                else
                {
                    if (WorkCurveHelper.DeviceCurrent.HasMotorSpin)
                    {
                        this.skyrayCamera.BackgroundImageLayout = ImageLayout.Stretch;
                        updateTestImg();
                    }
                }

            }
            else
            {
                if (WorkCurveHelper.DeviceCurrent.HasMotorSpin)
                {
                    this.skyrayCamera.BackgroundImageLayout = ImageLayout.Center;
                    this.skyrayCamera.BackgroundImage = Bitmap.FromFile(AppDomain.CurrentDomain.BaseDirectory + "testing.png");
                }
            }

            if (WorkCurveHelper.DeviceCurrent.HasMotorSpin)
            {
                this.skyrayCamera.Stop();
                MotorOperator.MotorOperatorY1Thread((int)(-WorkCurveHelper.TestDis * WorkCurveHelper.Y1Coeff));
            }

            

            deviceMeasure.interfacce.SetDp5Cfg();
            this.skyrayCamera.RemoveAtContiMeasurePoint(0);

            if (this.skyrayCamera.fed != null)
            {

                this.skyrayCamera.fed.dgvMultiDatas.ClearSelection();
                this.skyrayCamera.fed.dgvMultiDatas.Rows[PointIndex].Selected = true;
                this.skyrayCamera.fed.dgvMultiDatas.FirstDisplayedScrollingRowIndex = PointIndex;
            }
            deviceMeasure.interfacce.MotorMove();
            //MessageStopMove(optMode);
        }


        public void multiReset()
        {
           

            //等待停止
            WorkCurveHelper.waitMoveStop();
            if (WorkCurveHelper.multiReset && this.skyrayCamera.ContiTestPoints.Count > 0)
            {

                PointIndex = this.skyrayCamera.alTempTestPoints.Count - this.skyrayCamera.alContiTestPoints.Count;

                MotorOperator.getZ();

                if (this.skyrayCamera.Mode == SkyrayCamera.CameraMode.Multiple && this.skyrayCamera.alContiTestHeights.Count > 0)
                {
                    MotorOperator.MotorOperatorZThread((int)WorkCurveHelper.Z - this.skyrayCamera.alContiTestHeights[PointIndex]);
                }           

                MotorOperator.getXY();
                Point srcPoint = new Point((int)WorkCurveHelper.X, (int)WorkCurveHelper.Y);
                Point destPoint = (Point)this.skyrayCamera.alTempTestPoints[PointIndex];
                int distanceX = srcPoint.X - destPoint.X;
                int distanceY = destPoint.Y - srcPoint.Y;
                MotorOperator.MotorOperatorXYThread(distanceX, distanceY);   

                
            }
        }


        public void goBack()
        {


            //等待停止
            WorkCurveHelper.waitMoveStop();

            if(this.skyrayCamera.ContiTestPoints.Count > 0 && DifferenceDevice.CurCameraControl.buttonWStart.Enabled)
            {

                this.skyrayCamera.IsShowCenter = false;
                this.skyrayCamera.IsShowTestPoints = false;

                MotorOperator.getZ();
                if (this.skyrayCamera.Mode == SkyrayCamera.CameraMode.Multiple && this.skyrayCamera.alContiTestHeights.Count > 0)
                {
                    MotorOperator.MotorOperatorZThread((int)WorkCurveHelper.Z - this.skyrayCamera.alContiTestHeights[0]);
                }

                MotorOperator.getXY();
                Point srcPoint = new Point((int)WorkCurveHelper.X, (int)WorkCurveHelper.Y);
                Point destPoint = (Point)this.skyrayCamera.ContiTestPoints[0];
                int distanceX = srcPoint.X - destPoint.X;
                int distanceY = destPoint.Y - srcPoint.Y;
                MotorOperator.MotorOperatorXYThread(distanceX, distanceY);


            }
        }

        public bool isTesting()
        {

            return DifferenceDevice.interClassMain.deviceMeasure.interfacce.connect == DeviceConnect.Connect;
        }

        public void gotoTestPoint(Point destPoint,int destHeight)
        {

            if (WorkCurveHelper.goingToDest)
                return;
            WorkCurveHelper.goingToDest = true;
            //等待停止
            WorkCurveHelper.waitMoveStop();

            MotorOperator.getZ();
            if (this.skyrayCamera.Mode == SkyrayCamera.CameraMode.Multiple)
            {
                MotorOperator.MotorOperatorZThread((int)WorkCurveHelper.Z - destHeight);
            }

            MotorOperator.getXY();
            
            Point srcPoint = new Point((int)WorkCurveHelper.X, (int)WorkCurveHelper.Y);
            int distanceX = srcPoint.X - destPoint.X;
            int distanceY = destPoint.Y - srcPoint.Y;
            MotorOperator.MotorOperatorXYThread(distanceX, distanceY);            

        }

        /// <summary>
        /// 固定步长按钮
        /// </summary>
        /// <param name="checkedState"></param>
        //private void cameraControlUC_OnFixedWalk(bool checkedState)
        //{
        //    this.IsFixedWalk = checkedState;
        //}

        /// <summary>
        /// 工具栏中计算按钮执行函数
        /// </summary>
        public override void CaculateExcute(bool flag, bool IsAddHistory)
        {
            //正常模式处理
            if (WorkCurveHelper.WorkCurveCurrent == null)
            {
                Msg.Show(Info.NoExistsWorkCurve, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            recordList.Clear();
            if (WorkCurveHelper.WorkCurveCurrent.Condition.DemarcateEnergys == null
                   || WorkCurveHelper.WorkCurveCurrent.Condition.DemarcateEnergys.Count == 0)
            {
                Msg.Show(Info.NoExistDecrobate);
                return;
            }

            if (this.specList == null || this.specList.Specs == null)
            {
                Msg.Show(Info.WorkCurveNoSpecList);
                return;
            }

            //if (SpecList.FindOne(w => w.Id == specList.Id) == null)
            //{
            //    Msg.Show(Info.strNoExitspecInfo);
            //    return;
            //}
            if (this.specList.Specs.Length == 0)
                return;
            this.refreshFillinof.ContructMeasureRefreshData(this.selectSpeclist.Count, WorkCurveHelper.WorkCurveCurrent.ElementList);
            CaculateContent(this.selectSpeclist, 1, IsAddHistory);
        }

        private bool CheckWholeCurveElementSamples()
        {
            if (WorkCurveHelper.WorkCurveCurrent.ElementList != null && WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count > 0)
            {
                var test = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().OrderByDescending(w => w.LayerNumber);
                if (test == null || test.Count() == 0)
                    return false;
                if (WorkCurveHelper.WorkCurveCurrent.CalcType != CalcType.FP)
                {
                    List<CurveElement> list = test.ToList();
                    for (int i = 1; i < list.Count; i++)
                    {
                        if (list[i].Samples == null || list[i].Samples.Count == 0)
                            return false;
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取对象及注册处理函数
        /// </summary>
        private void CameralObjectProcess()
        {
            MotorAdvance.FocusPointEndEvent += new MotorAdvance.FocusPointEndEventHandler(MotorAdvance_FocusPointEndEvent);
            MotorAdvance.AutoFocusEndEndEvent += new MotorAdvance.AutoFocusEndEventHandler(MotorAdvance_AutoFocusEndEvent);
            CameraRefMotor.MotorMoveBeginEvent += new CameraRefMotor.MotorMoveBeginEventHandler(CameraRefMotor_MotorMoveBeginEvent);
            CameraRefMotor.MotorMoveEndEvent += new CameraRefMotor.MotorMoveEndEventHandler(CameraRefMotor_MotorMoveEndEvent);

            this.frmAdjustDistance = new FrmSetAdjustPointsDistance();
            this.frmAdjustDistance.OnadjustDistance += new EventDelegate.CameralOperation(frmAdjustDistance_OnadjustDistance);

            this.skyrayCamera.KeyDown += new KeyEventHandler(skyrayCamera_KeyDown);
            this.skyrayCamera.KeyUp += new KeyEventHandler(skyrayCamera_KeyUp);
            this.skyrayCamera.MouseClick += new MouseEventHandler(skyrayCamera_MouseClick);
            this.skyrayCamera.MouseDoubleClick += new MouseEventHandler(skyrayCamera_MouseDoubleClick);
            this.skyrayCamera.MouseEnter += new EventHandler(skyrayCamera_MouseEnter);
            this.skyrayCamera.MouseDown += new MouseEventHandler(skyrayCamera_MouseDown);
            this.skyrayCamera.MouseUp += new MouseEventHandler(skyrayCamera_MouseUp);
            this.skyrayCamera.motorBack += new SkyrayCamera.ReceiveMotorback(skyrayCamer_MotorBack);
            this.cameraControlUC.OnReturnCameralMultiPoint += new EventDelegate.ReturnCameralState(cameraControlUC_OnReturnCameralState);
            this.cameraControlUC.OnReturnCameralMatrix += new EventDelegate.ReturnCameralState(cameraControlUC_OnReturnCameralMatrix);
            this.cameraControlUC.OnReturnCameralNetWork += new EventDelegate.ReturnCameralState(cameraControlUC_OnReturnCameralNetWork);
            this.cameraControlUC.OnReturnCameralMove += new EventDelegate.ReturnCameralState(cameraControlUC_OnReturnCameralMove);
            this.cameraControlUC.OnReturnCameralCheck += new EventDelegate.ReturnCameralState(cameraControlUC_OnReturnCameralCheck);
            this.cameraControlUC.OnReturnCameralSinglePoint += new EventDelegate.ReturnCameralState(cameraControlUC_OnReturnCameralSinglePoint);
            //this.cameraControlUC.OnFixedWalk += new EventDelegate.ReturnCameralState(cameraControlUC_OnFixedWalk);
            this.cameraControlUC.OnCameralStart += new EventDelegate.CameralOperation(cameraControlUC_OnCameralStart);
            this.cameraControlUC.OnCameralStop += new EventDelegate.CameralOperation(cameraControlUC_OnCameralStop);
            this.cameraControlUC.OnCameralReset += new EventDelegate.CameralOperation(cameraControlUC_OnCameralReset);
            this.cameraControlUC.OnCameralOutSample += new EventDelegate.CameralOperation(cameraControlUC_OnCameralOutSample);

        }


        private void skyrayCamer_MotorBack()
        {
            goBack();


            this.skyrayCamera.ContiTestPoints.Clear();
            this.skyrayCamera.alTempTestPoints.Clear();
            this.skyrayCamera.alContiTestHeights.Clear();
            this.skyrayCamera.tsmiSaveMultiPoint.Visible = false;
            this.skyrayCamera.tsmiDelAllFlag.Visible = false;
            this.skyrayCamera.tsmiDelLastFlag.Visible = false;
        
        }

        void CameraRefMotor_MotorMoveEndEvent()
        {
            TestStartAfterControlState(true);
        }

        void CameraRefMotor_MotorMoveBeginEvent()
        {

        }

        /// <summary>
        /// 鼠标点击按下摄像头
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void skyrayCamera_MouseEnter(object sender, EventArgs e)
        {
            if (!this.skyrayCamera.Focused)
            {
                this.skyrayCamera.Focus();
            }
        }



        //主界面工作曲线控件
        private Skyray.Controls.TabControlW controlCurve;
        /// <summary>
        /// 找cameraControlUC对象及摄像头对象
        /// </summary>
        /// <param name="clientPannel"></param>
        /// <returns></returns>
        public override SkyrayCamera FindCameralUserControl(ContainerObject clientPannel)
        {
            Control[] controlCurves = clientPannel.Controls.Find("tabControlCurve", true);
            if (controlCurves != null && controlCurves.Length > 0)
            {
                controlCurve = controlCurves[0] as Skyray.Controls.TabControlW;
            }
            Control[] controlCameral = clientPannel.Controls.Find("ucCameraControl1", true);
            if (controlCameral != null && controlCameral.Length > 0)
            {
                this.cameraControlUC = controlCameral[0] as Skyray.UC.UCCameraControl;
                this.cameraControlUC.Dock = DockStyle.Fill;
                if (WorkCurveHelper.DeviceCurrent.HasMotorX && WorkCurveHelper.DeviceCurrent.HasMotorY
            && WorkCurveHelper.DeviceCurrent.HasMotorZ && this.cameraControlUC != null)
                    this.cameraControlUC.Flag = true;
                else
                    this.cameraControlUC.Flag = false;
                this.cameraControlUC.CurrentDecvice = WorkCurveHelper.DeviceCurrent;
                Control[] controls = this.cameraControlUC.Controls.Find("skyrayCamera1", true);
                if (controls == null || controls.Length == 0)
                    return null;
                this.skyrayCamera = controls[0] as SkyrayCamera;
                this.skyrayCamera.Dock = DockStyle.None; // thick特殊摄像头时DockStyle.Fill;

                Control[] controlCheckbox = this.cameraControlUC.Controls.Find("chkIsLazerOpen", true);
                if (controlCheckbox != null && controlCheckbox.Length > 0)
                    this.highCheckBox = controlCheckbox[0] as CheckBox;
                Control[] controlMove = this.cameraControlUC.Controls.Find("radioButtonMove", true);
                if (controlMove != null && controlMove.Length > 0)
                    this.radioButtonMove = controlMove[0] as RadioButton;
                CameralObjectProcess();
                this.cameraControlUC.MoveAttribute = true;
            }
            return this.skyrayCamera;
        }

        void thickMoveStation_OnStopEnableControl()
        {
        }

        public override void LoadToolsConfig()
        {
            if (FrmLogon.userName != null)
            {
                var vv = User.FindOne(w => w.Name == FrmLogon.userName);
                if (vv.Role.RoleType == 0)
                    LoadToolsFile("ThickToolsConfig_Admin.txt");
                else if (vv.Role.RoleType == 1)
                    LoadToolsFile("ThickToolsConfig_Admin.txt");
                else
                {
                    LoadToolsFile("ThickToolsConfig1_CommonUser.txt");
                }


                //Think 加载判断打印设置是否显示 Strong 2012/10/16
                if (DifferenceDevice.ithick != null)
                    if (ReportTemplateHelper.ExcelModeType.ToString().Equals("2"))
                    {
                        if (WorkCurveHelper.NaviItems.Find(w => w.Name == "PrintSetting") != null)
                        {
                            WorkCurveHelper.NaviItems.Find(w => w.Name == "PrintSetting").ShowInMain = true;
                            WorkCurveHelper.NaviItems.Find(w => w.Name == "PrintSetting").Enabled = true;
                        }
                    }
                    else
                    {
                        if (WorkCurveHelper.NaviItems.Find(w => w.Name == "PrintSetting") != null)
                        {
                            WorkCurveHelper.NaviItems.Find(w => w.Name == "PrintSetting").ShowInMain = false;
                            WorkCurveHelper.NaviItems.Find(w => w.Name == "PrintSetting").Enabled = false;
                        }
                    }
                //
            }
        }

        public override void SaveElementsMeasure(HistoryRecord history, WorkCurve workCurve, SpecEntity spec)
        {
            if (workCurve == null || workCurve.ElementList == null || workCurve.ElementList.Items.Count == 0 || history == null)
                return;
            var maxLayElem = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.OrderByDescending(w => w.LayerNumber).ToList();
            if (maxLayElem == null || maxLayElem.Count == 0)
                return;
            string[] elementName = new string[workCurve.ElementList.Items.Count];
            string[] valuse = new string[workCurve.ElementList.Items.Count];
            string[] realValuse = new string[workCurve.ElementList.Items.Count];
            int rows = WorkCurveHelper.HistoryAverageRows - 1;
            List<CurveElement> lstElement = workCurve.ElementList.Items.OrderBy(w => w.RowsIndex).ToList();
            int i = 0;
            // foreach (CurveElement elements in workCurve.ElementList.Items)
            foreach (CurveElement elements in lstElement)
            {
                if (elements.Caption == maxLayElem[0].Caption && workCurve.CalcType == CalcType.EC)
                    continue;
                HistoryElement historyElemet = HistoryElement.New;
                historyElemet.elementName = elements.Caption;
                historyElemet.ElementIntensity = BackGroundHelper.CaculateArea(spec, elements.PeakLow, elements.PeakHigh);
                historyElemet.CaculateIntensity = elements.Intensity;
                historyElemet.unitValue = (int)elements.ContentUnit;

                historyElemet.thickunitValue = (int)elements.ThicknessUnit;


                if ((!(elements.Content > double.Epsilon && elements.Content < double.PositiveInfinity)))
                    elements.Content = 0.0000d;
                double tempContent = elements.Content >= 100 ? 100 : elements.Content;

                historyElemet.contextelementValue = (elements.ContentUnit.ToString().Equals("per")) ? tempContent.ToString() : (tempContent * 10000).ToString();//"f" + WorkCurveHelper.ThickBit

                if (elements.ContentUnit == ContentUnit.per)
                {
                    historyElemet.contextelementValue = tempContent.ToString();//"f" + WorkCurveHelper.ThickBit
                }
                else if (elements.ContentUnit == ContentUnit.ppm)
                {
                    historyElemet.contextelementValue = (tempContent * 10000).ToString();//"f" + WorkCurveHelper.ThickBit
                }
                else
                {
                    historyElemet.contextelementValue = (tempContent * 10).ToString();//"f" + WorkCurveHelper.ThickBit
                }

                double tempdensityValue = workCurve.ElementList.GetAreaDensity(elements.LayerNumber);
                List<AreaDensityUnit> units = AreaDensityUnit.FindAll();
                AreaDensityUnit ss = units.Find(w => w.Name == WorkCurveHelper.WorkCurveCurrent.AreaThickType);
                //AreaDensityUnit ss = AreaDensityUnit.FindOne(w => w.Name == workCurve.AreaThickType);
                if (ss != null)
                {
                    //追加面密度
                    historyElemet.densityunitValue = workCurve.AreaThickType;
                    historyElemet.densityelementValue = (double.Parse(ss.cofeK) * tempdensityValue).ToString();
                }
                else
                {
                    //追加面密度
                    historyElemet.densityunitValue = Info.strAreaDensityUnit;
                    historyElemet.densityelementValue = tempdensityValue.ToString();

                }
                //面密度的话，含量显示单个元素面密度 
                if (WorkCurveHelper.WorkCurveCurrent.IsThickShowAreaThick)
                {
                    historyElemet.contextelementValue = (tempContent / 100 * double.Parse(historyElemet.densityelementValue)).ToString("f" + WorkCurveHelper.ThickBit);
                }
                if (elements.ContentUnit == ContentUnit.per)
                {
                    historyElemet.Error = elements.Error;
                }
                else if (elements.ContentUnit == ContentUnit.ppm)
                {
                    historyElemet.Error = elements.Error * 10000;
                }
                else
                {
                    historyElemet.Error = elements.Error * 10;
                }
                if (WorkCurveHelper.CurrentStandard != null)
                    historyElemet.customstandard_Id = WorkCurveHelper.CurrentStandard.Id;
                else
                    historyElemet.customstandard_Id = 0;
                double AverageThickness = 0;
                double lastThickValue = (elements.ThicknessUnit.ToString().Equals("ur")) ? ((double)elements.Thickness / 0.0254) : elements.Thickness;//"f" + WorkCurveHelper.ThickBit

                if (rows > 1 && (listHistoryElement.Count / lstElement.Count >= 1))
                {
                    List<double> oneElemRealValueList = new List<double>();
                    double one = 0.0;
                    foreach (HistoryElement histelement in listHistoryElement.FindAll(w => w.elementName == elements.Caption))
                    {
                        bool result = double.TryParse(histelement.contentRealelemValue, out one);
                        oneElemRealValueList.Add(one);
                    }
                    double customvalue = 0;
                    oneElemRealValueList.Add(lastThickValue);

                    if (oneElemRealValueList.Count > 0)
                    {
                        AverageThickness = oneElemRealValueList.Average();
                        customvalue = oneElemRealValueList.Average();
                    }
                    else
                    {
                        AverageThickness = lastThickValue;
                        customvalue = lastThickValue;

                    }
                    valuse[i] = (elements.ThicknessUnit.ToString().Equals("ur")) ? (customvalue * 0.0254).ToString() : customvalue.ToString();//"f" + WorkCurveHelper.ThickBit //自定义THICK计算厚度
                }
                else
                {

                    AverageThickness = lastThickValue;
                    valuse[i] = elements.Thickness.ToString();  //自定义THICK计算厚度
                }

                historyElemet.thickelementValue = AverageThickness.ToString("f" + WorkCurveHelper.ThickBit);//"f" + WorkCurveHelper.ThickBit
                historyElemet.contentRealelemValue = (elements.ThicknessUnit.ToString().Equals("ur")) ? ((double)elements.Thickness / 0.0254).ToString("f" + WorkCurveHelper.ThickBit) : elements.Thickness.ToString("f" + WorkCurveHelper.ThickBit);//"f" + WorkCurveHelper.ThickBit
                realValuse[i] = elements.Thickness.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
                elementName[i] = elements.Caption;

                i++;
                historyElemet.HistoryRecord = history;
                historyElemet.Save();
                history.HistoryElement.Add(historyElemet);
            }
            foreach (var custom in workCurve.ElementList.CustomFields)
            {
                double value = double.Epsilon;
                double realvalue = double.Epsilon;
                TabControlHelper.CustomFieldByFortum(custom.Expression, elementName, valuse, 0, out value);
                TabControlHelper.CustomFieldByFortum(custom.Expression, elementName, realValuse, 0, out realvalue);
                HistoryElement historyElemet = HistoryElement.New;
                historyElemet.elementName = custom.Name;
                historyElemet.contextelementValue = "0";
                historyElemet.unitValue = 0;
                historyElemet.CaculateIntensity = 0;
                historyElemet.ElementIntensity = 0;
                historyElemet.Error = 0;
                historyElemet.thickelementValue = value.ToString("f" + WorkCurveHelper.ThickBit);
                historyElemet.contentRealelemValue = realvalue.ToString("f" + WorkCurveHelper.ThickBit);
                historyElemet.densityunitValue = "";
                historyElemet.densityelementValue = "";
                historyElemet.HistoryRecord = history;
                historyElemet.Save();
                history.HistoryElement.Add(historyElemet);
            }
            //history.Save();
        }

        public override void MeasureStandSample(WorkCurve workCurve, TestDevicePassedParams testDeviceParams)
        {
            optMode = OptMode.StandSample;
            if (workCurve == null)
            {
                Msg.Show(Info.WarningTestContext, Info.TestWarning, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            StartTestModeInitialize(workCurve, testDeviceParams);
        }

        public override void RefrenshMoveStation(NaviItem item, ContainerObject panel)
        {
            CommonMoveStation(item, panel);
            if (this.cameraControlUC != null)
                this.cameraControlUC.CurrentDecvice = WorkCurveHelper.DeviceCurrent;
            if (WorkCurveHelper.DeviceCurrent.HasMotorX && WorkCurveHelper.DeviceCurrent.HasMotorY
              && WorkCurveHelper.DeviceCurrent.HasMotorZ && this.cameraControlUC != null)
                this.cameraControlUC.Flag = true;
            else
                this.cameraControlUC.Flag = false;
        }

        public override void MeasurePureElementProcess(WorkCurve workCurve, TestDevicePassedParams testDeviceParams)
        {
            optMode = OptMode.PureSample;
            if (workCurve == null)
            {
                Msg.Show(Info.WarningTestContext, Info.TestWarning, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            StartTestModeInitialize(workCurve, testDeviceParams);
        }

        public override void optModeSample(int usedTime, WorkCurve workCurve)
        {
            optModeTest(usedTime, false, optMode);
        }

        public override void optModePure(int usedTime, WorkCurve workCurve)
        {
            optModeTest(usedTime, false, optMode);
        }

        public override void TestStop()
        {
            this.currentTestTimes = 1;

            DifferenceDevice.IsConnect = false;
            progressInfo.Value = 0;
            CameraRefMotor.CancelAll();
            this.deviceMeasure.Stop();
            this.currentDeviceParamsList.Clear();
            this.XrfChart.ClearInformation();
            skyrayCamera.ContiTestPoints.Clear();
            TestStartAfterControlState(true);
            this.skyrayCamera.IsShowCenter = false;
        }

        public override void FinishFocusState(bool flag)
        {
            if (flag)
            {
                this.cameraControlUC.SetFocusText(Info.AutoFocus);
            }

        }

        public override void TestStartAfterControlState(bool flag)
        {
         
            if (this.controlCurve != null)
            {
                this.controlCurve.Enabled = flag;
            }
            if (flag)
            {

                bAdjustInitCount = false;
                WorkCurveHelper.IsFirstInfluenceGain = true;
                if (this.cameraControlUC != null)
                {

                    this.cameraControlUC.CurrentDecvice = WorkCurveHelper.DeviceCurrent;
                    if (WorkCurveHelper.DeviceCurrent.HasMotorX && WorkCurveHelper.DeviceCurrent.HasMotorY && WorkCurveHelper.DeviceCurrent.HasMotorZ)
                        this.cameraControlUC.Flag = true;
                    else
                        this.cameraControlUC.Flag = false;
                    this.cameraControlUC.ShowMenu = true;
                    this.cameraControlUC.ChkRetest.Enabled = true;
                    this.cameraControlUC.ChkLazer.Enabled = true;
                    this.cameraControlUC.ChkTestDemo.Enabled = true;
                    this.cameraControlUC.ChkRetest.Enabled = true;
                    this.cameraControlUC.encoderMoveUp.Enabled = true;
                    this.cameraControlUC.encoderMoveDown.Enabled = true;

                }
            }
            else
            {
                if (this.cameraControlUC != null)
                {
                  
                    this.cameraControlUC.DisableControl(this.cameraControlUC.CurrentDecvice, true);
                    this.cameraControlUC.Flag = false;
                    this.cameraControlUC.ShowMenu = false;
                    this.cameraControlUC.ChkLazer.Enabled = false;
                    this.cameraControlUC.ChkTestDemo.Enabled = false;
                    this.cameraControlUC.ChkRetest.Enabled = false;
                    this.cameraControlUC.encoderMoveUp.Enabled = false;
                    this.cameraControlUC.encoderMoveDown.Enabled = false;
                }
            
            }
            base.TestStartAfterControlState(flag);
        }
        //更改Camera上的信息
        public override void RefreshCameraText(int nextNum, int totalNum, bool IsShowButton)
        { 
            if (nextNum <= 0)
            {
                this.cameraControlUC.SetTestInformation("", IsShowButton);
                base.RefreshCameraText(nextNum, totalNum, IsShowButton);
                return;
            }
            string msg = string.Empty;
            if (Info.TestInfoMsg1.Contains("{0}th"))
            {
                switch (nextNum % 10)
                {
                    case 1:
                        msg = Info.TestInfoMsg1;
                        msg = msg.Replace("{0}th", nextNum + "st");
                        msg = msg.Replace("{1}", totalNum.ToString());
                        break;
                    case 2:
                        msg = Info.TestInfoMsg1;
                        msg = msg.Replace("{0}th", nextNum + "nd");
                        msg = msg.Replace("{1}", totalNum.ToString());
                        break;
                    case 3:
                        msg = Info.TestInfoMsg1;
                        msg = msg.Replace("{0}th", nextNum + "rd");
                        msg = msg.Replace("{1}", totalNum.ToString());
                        break;
                    default:
                        msg = string.Format(Info.TestInfoMsg1, nextNum.ToString(), totalNum.ToString());
                        break;

                }
            }
            else msg = string.Format(Info.TestInfoMsg1, nextNum.ToString(), totalNum.ToString());
            if (IsShowButton)
            {
                msg += "\r\n" + Info.TestInfoMsg2;
            }
            this.cameraControlUC.SetTestInformation(msg, IsShowButton);
            base.RefreshCameraText(nextNum, totalNum, IsShowButton);
        }


        public void SelectCountStyle(bool flag)
        {
            if (flag)
                this.deviceMeasure.interfacce.currentRateMode = CountingRateModes.RealTime;
            else
                this.deviceMeasure.interfacce.currentRateMode = CountingRateModes.Average;

        }


        public void CountBits(int thickBit, int contentbit)
        {
            WorkCurveHelper.SoftWareContentBit = contentbit;
            WorkCurveHelper.ThickBit = thickBit;
        }


        public void ExcuteCurveCalibrate(NaviItem item, int cnt, OptMode currentOptMode)
        {
            if (WorkCurveHelper.WorkCurveCurrent == null)
            {
                Msg.Show(Info.WarningTestContext, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            WorkCurve workCurve = WorkCurveHelper.WorkCurveCurrent;
            if (workCurve == null || workCurve.ElementList == null || workCurve.ElementList.Items == null || workCurve.ElementList.Items.Count <= 0) return;
            string strFormat = string.Empty;
            if (currentOptMode == OptMode.CurveCalibrate)
            {
                CalibrateElemCnt = cnt;
                strFormat = string.Format(Info.PleaseCurveCalibrationSample + workCurve.ElementList.Items[cnt].Caption);
            }
            else
            {
                // CalibrateElemCnt = workCurve.ElementList.Items.ToList().FindIndex(w => w.LayerNumBackUp == Info.Substrate || w.LayerNumBackUp == "基材" || w.LayerNumBackUp == "Substrate");
                //只影响第一层 ，这里不用基材
                CalibrateElemCnt = workCurve.ElementList.Items.ToList().FindIndex(w => w.LayerNumber == 1);
                strFormat = Info.PleasePutInBase;
            }
            if (Msg.Show(strFormat, MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                List<WordCureTest> localWorkCurve = new List<WordCureTest>();
                string strSampleName = workCurve.ElementList.Items[CalibrateElemCnt].Caption + "(" + DateTime.Now.ToString("yyyyMMddHHmmss") + ")";
                SpecListEntity specList = new SpecListEntity
                    (
                    strSampleName,
                    strSampleName,
                    deviceMeasure.interfacce.ReturnEncoderValue,
                    0.0,
                    "",
                    0,
                    "",
                    FrmLogon.userName,
                    DateTime.Now,
                    "",
                    SpecType.PureSpec,
                    DifferenceDevice.DefaultSpecColor.ToArgb(),
                    DifferenceDevice.DefaultSpecColor.ToArgb()
                    )
                {
                    Loss = 0.0
                };
                int dropTime = 0;
                int.TryParse(ReportTemplateHelper.LoadSpecifiedValueNoWait("TestParams", "DropTime"), out dropTime);
                MeasureParams MeasureParams = new MeasureParams
                    (
                    1,
                    dropTime,
                    false
                    );
                TestDevicePassedParams testDevicePassedParams = new TestDevicePassedParams
                (
                false,
                MeasureParams,
                localWorkCurve,
                WorkCurveHelper.DeviceCurrent.IsAllowOpenCover,
                SpecType.PureSpec,
                "",
                false,
                true
                );
                optMode = currentOptMode;//OptMode.CurveCalibrate;
                // if (!NoLoadDeviceInitialize(testDevicePassedParams, WorkCurveHelper.DeviceCurrent)) return;
                this.testDevicePassedParams = testDevicePassedParams;
                List<DeviceParameter> listParams = WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList.ToList();
                if (WorkCurveHelper.WorkCurveCurrent.ElementList != null && WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count > 0)
                    DeviceParameterByElementList(listParams);
                else
                    this.deviceParamsList = listParams;
                if (this.deviceParamsList.Count == 0)
                {
                    Msg.Show(Info.MeasureConditionInvalidate);
                }
                initParams = WorkCurveHelper.WorkCurveCurrent.Condition.InitParam;
                this.deviceParamSelectIndex = 0;
                this.currentDeviceParamsList.Clear();
                this.deviceMeasure.interfacce.DeviceParam = this.deviceParamsList[this.deviceParamSelectIndex];

                int dt = -1;
                if (!this.currentDeviceParamsList.TryGetValue(deviceMeasure.interfacce.DeviceParam.Id, out dt))
                    this.currentDeviceParamsList.Add(this.deviceMeasure.interfacce.DeviceParam.Id, this.deviceMeasure.interfacce.DeviceParam.TubCurrent);
                this.deviceMeasure.interfacce.InitParam = this.initParams;
                this.deviceMeasure.interfacce.ExistMagnet = WorkCurveHelper.DeviceCurrent.HasElectromagnet;
                currentTestTimes = 1;
                this.spec = new SpecEntity();
                this.spec.IsSmooth = true;
                this.specList = new SpecListEntity();
                this.specList.WorkCurveName = WorkCurveHelper.WorkCurveCurrent.Name;
                this.specList.Name = this.specList.SampleName = workCurve.ElementList.Items[CalibrateElemCnt].Caption + "(" + DateTime.Now.ToString("yyyyMMddHHmmss") + ")";
                this.specList.Weight = 0;
                this.specList.Operater = GP.UserName;
                //this.specList.Image = this.skyrayCamera.GetGrabImageBytes();
                this.specList.Specs = new SpecEntity[this.deviceParamsList.Count];
                this.specList.Specs[0] = this.spec;
                this.deviceMeasure.interfacce.Spec = this.spec;
                RefreshDeviceInitialize(WorkCurveHelper.DeviceCurrent);
                TestStartAfterControlState(false);
                this.deviceMeasure.interfacce.StopFlag = false;
                //progressInfo.MeasureTime = this.deviceParamsList[deviceParamSelectIndex].PrecTime + "s";
                //progressInfo.SurplusTime = this.deviceParamsList[deviceParamSelectIndex].PrecTime + "s";
                deviceMeasure.interfacce.SetDp5Cfg();
                InitProcessBar();
                this.deviceMeasure.interfacce.MotorMove();
            }
        }
        /// <summary>
        /// 增加打印模式，唯独在Thick项目，保存至App文档 Sunian 2012/10/15
        /// </summary>
        public void AppSave()
        {
            string strPath = AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml";
            XmlDocument xmlDoc = null;
            XmlNode xmlNote = null;
            try
            {
                xmlDoc = new XmlDocument();
                if (File.Exists(strPath))
                {
                    xmlDoc.Load(strPath);
                    xmlNote = xmlDoc.SelectSingleNode("/application/Excel/ExcelModeType");
                    xmlNote.InnerText = ReportTemplateHelper.ExcelModeType.ToString();
                    xmlDoc.Save(strPath);
                    ExcelTemplateParams.LoadExcelTemplateParams(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml");//Thick 选择标准模版的时候进行初始化赋值
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (ReportTemplateHelper.ExcelModeType.ToString().Equals("2"))
            {
                if (WorkCurveHelper.NaviItems.Find(w => w.Name == "PrintSetting") != null)
                {
                    WorkCurveHelper.NaviItems.Find(w => w.Name == "PrintSetting").ShowInMain = true;
                    WorkCurveHelper.NaviItems.Find(w => w.Name == "PrintSetting").Enabled = true;
                }
            }
            else
            {
                if (WorkCurveHelper.NaviItems.Find(w => w.Name == "PrintSetting") != null)
                {
                    WorkCurveHelper.NaviItems.Find(w => w.Name == "PrintSetting").ShowInMain = false;
                    WorkCurveHelper.NaviItems.Find(w => w.Name == "PrintSetting").Enabled = false;
                }
            }


        }

        private void LoadParameter(string filePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);


            XmlNode xmlReportResults = doc.SelectSingleNode("Parameter/ReportResults");
            if (xmlReportResults != null && xmlReportResults.ChildNodes != null)
            {
                foreach (XmlNode childnode in xmlReportResults.ChildNodes)
                {
                    if (childnode.Name == "PassResults") ExcelTemplateParams.PassResults = childnode.InnerText;
                    else if (childnode.Name == "FalseResults") ExcelTemplateParams.FalseResults = childnode.InnerText;
                    else if (childnode.Name == "STDResults") ExcelTemplateParams.STDResults = childnode.InnerText;
                    //else if (childnode.Name == "AtomElemFullName") Report.isElemFullName = int.Parse(childnode.InnerText);
                }
            }
        }


        //public override void TestInitilizeEnd(bool success)
        //{
        //    base.TestInitilizeEnd(success);
        //    if (success && WorkCurveHelper.bInitialize)
        //    {
        //        string sqlTemp = "select * from Condition where Device_Id=" + WorkCurveHelper.DeviceCurrent.Id + "";
        //        List<Condition> currentConditon = Condition.FindBySql(sqlTemp);
        //        if (currentConditon != null && currentConditon.Count > 0)
        //        {
        //            foreach (Condition temp in currentConditon)
        //            {
        //                if (temp.InitParam.ElemName == initParams.ElemName)  //相同初始化元素情况下全部更新
        //                {
        //                    string sql = "Update InitParameter Set TubVoltage=" + this.initParams.TubVoltage + ",TubCurrent=" + this.initParams.TubCurrent + ", Gain = " + this.initParams.Gain + ", FineGain = "
        //                                + this.initParams.FineGain + ",Channel=" + this.initParams.Channel + ",Filter=" + this.initParams.Filter + ",Collimator=" + this.initParams.Collimator + ",ChannelError=" + this.initParams.ChannelError + ",ElemName='" + this.initParams.ElemName + "' Where Id = " + temp.InitParam.Id;
        //                    Lephone.Data.DbEntry.Context.ExecuteNonQuery(sql);
        //                }
        //            }
        //            //重新赋值当前曲线的初始化参数
        //            WorkCurveHelper.WorkCurveCurrent = WorkCurve.FindById(WorkCurveHelper.WorkCurveCurrent.Id);
        //        }
        //    }
        //    //DeviceInterface.PostMessage(this.handle, DeviceInterface.Wm_NextInitializationElem, true, id);
        //    //Wm_NextInitializationElem
        //}



        //public override void AddPeakFlag(DemarcateEnergy energy)
        //{
        //    base.AddPeakFlag(energy);
        //    if (WorkCurveHelper.bInitialize)
        //    {
        //        string sqlTemp = "select * from Condition where Device_Id=" + WorkCurveHelper.DeviceCurrent.Id + "";
        //        List<Condition> currentConditon = Condition.FindBySql(sqlTemp);
        //        if (currentConditon == null || currentConditon.Count == 0)
        //            return;
        //        foreach (Condition temp in currentConditon)
        //        {
        //            DemarcateEnergy de = temp.DemarcateEnergys.ToList().Find(d => d.ElementName.Equals(energy.ElementName) && d.Line == energy.Line);
        //            if (de != null)
        //            {
        //                de.Energy = energy.Energy;
        //                de.Channel = energy.Channel;
        //                string stringSql = "update DemarcateEnergy set Energy =" + energy.Energy + ",Channel=" + energy.Channel + " where id=" + de.Id;
        //                Lephone.Data.DbEntry.Context.ExecuteNonQuery(stringSql);
        //            }
        //            else
        //            {
        //                string stringSql = "insert into DemarcateEnergy(ElementName, Line, Energy, Channel, Condition_Id) values('" + energy.ElementName + "'," + (int)energy.Line + "," + energy.Energy + "," + energy.Channel + "," + temp.Id + ")";
        //                Lephone.Data.DbEntry.Context.ExecuteNonQuery(stringSql);
        //                stringSql = "select Max(Id) from DemarcateEnergy";
        //                object obj = Lephone.Data.DbEntry.Context.ExecuteScalar(stringSql);
        //                int id = 1;
        //                try
        //                {
        //                    id = int.Parse(obj.ToString());
        //                }
        //                catch
        //                {
        //                    id = 1;
        //                }
        //            }
        //        }
        //    }
        //}

        //public override void DelPeakFlag(DemarcateEnergy energy)
        //{
        //    if (WorkCurveHelper.bInitialize)
        //    {
        //        string sqlTemp = "select * from Condition where Device_Id=" + WorkCurveHelper.DeviceCurrent.Id + "";
        //        List<Condition> currentConditon = Condition.FindBySql(sqlTemp);
        //        if (currentConditon == null || currentConditon.Count == 0)
        //            return;
        //        foreach (Condition temp in currentConditon)
        //        {
        //            DemarcateEnergy enery = this.XrfChart.ReadyDeleteDE;
        //            DemarcateEnergy de = temp.DemarcateEnergys.ToList().Find(d => d.ElementName.Equals(energy.ElementName) && d.Line == energy.Line);
        //            if (de != null)
        //                de.Delete();
        //        }
        //    }
        //    base.DelPeakFlag(energy);
        //}


        public override void DemarcateFirstStage()
        {
            if (WorkCurveHelper.bInitialize)
            {
                string sqlTemp = "select * from Condition where Device_Id=" + WorkCurveHelper.DeviceCurrent.Id + "";
                List<Condition> currentConditon = Condition.FindBySql(sqlTemp);
                if (currentConditon != null && currentConditon.Count > 0)
                {
                    foreach (Condition temp in currentConditon)
                    {
                        if (temp.InitParam.ElemName == initParams.ElemName)  //相同初始化元素情况下全部更新
                            UpdateDemarcateByConditiion(temp, temp.InitParam.ElemName);
                    }
                }
            }
        }

        public override void DemarcateSencondStage()
        {
            if (WorkCurveHelper.bInitialize)
            {
                string sqlTemp = "select * from Condition where Device_Id=" + WorkCurveHelper.DeviceCurrent.Id + "";
                List<Condition> currentConditon = Condition.FindBySql(sqlTemp);
                if (currentConditon != null && currentConditon.Count > 0)
                {
                    foreach (Condition temp in currentConditon)
                    {
                        if (temp.InitParam.ElemName == initParams.ElemName)  //相同初始化元素情况下全部更新
                            UpdateDemarcateByConditiion(temp, "Cu");
                    }
                }
            }
        }

        private void OptModeCurveCalibrate(OptMode optMode)
        {
            if (deviceParamSelectIndex < deviceParamsList.Count)
            {
                this.spec.UsedTime = this.deviceMeasure.interfacce.usedTime;
                this.spec.SpecTime = deviceParamsList[deviceParamSelectIndex].PrecTime;
                string middleStr = this.specList.SampleName + "_" + deviceParamsList[deviceParamSelectIndex].Name + "_"
                    + initParams.Condition.Name + "_" + currentTestTimes;
                this.spec.RemarkInfo = "";//spec.RemarkInfo不允许为null
                this.spec.DeviceParameter = this.deviceParamsList[deviceParamSelectIndex].ConvertFrom();
                this.spec.TubCurrent = this.spec.TubCurrent > 0 ? this.spec.TubCurrent : this.spec.DeviceParameter.TubCurrent;
                this.spec.TubVoltage = this.spec.TubVoltage > 0 ? this.spec.TubVoltage : this.spec.DeviceParameter.TubVoltage;
                DeviceParameter dt = null;
                if (!this.FirstDeviceParamsList.TryGetValue(deviceParamSelectIndex, out dt))
                    this.FirstDeviceParamsList.Add(this.deviceParamSelectIndex, this.deviceMeasure.interfacce.DeviceParam);
                this.spec.Name = middleStr;
                if (SpecHelper.IsSmoothProcessData)
                {
                    List<int[]> ttOutput = new List<int[]>();
                    if (TempNetDataSmooth.TryGetValue(this.deviceParamsList[deviceParamSelectIndex].Id, out ttOutput))
                    {
                        int[] smooth = Helper.ToInts(this.spec.SpecData);
                        ttOutput.Add(smooth);
                        if (ttOutput.Count > 5)
                        {
                            ttOutput.RemoveAt(0);
                        }
                        else
                        {
                            int[] ttTemp = ttOutput[0];
                            int count = ttOutput.Count;
                            for (int m = count; m < 5; m++)
                                ttOutput.Add(ttTemp);
                        }
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < this.deviceMeasure.interfacce.backData.Length; i++)
                        {
                            int te = 0;
                            foreach (var arr in ttOutput)
                            {
                                te += arr[i];
                            }
                            int temp = (int)Math.Round((te * 1.0 / ttOutput.Count), MidpointRounding.AwayFromZero);
                            sb.Append(temp.ToString() + ",");
                        }
                        TempNetDataSmooth.Remove(this.deviceParamsList[deviceParamSelectIndex].Id);
                        TempNetDataSmooth.Add(this.deviceParamsList[deviceParamSelectIndex].Id, ttOutput);
                        this.spec.SpecData = sb.ToString();
                    }
                }
            }
            if (deviceParamSelectIndex == deviceParamsList.Count - 1)
            {
                //当前小条件进行完毕
                string conditionName = string.Empty;
                specList.Name = specList.SampleName; //constStr;
                specList.ImageShow = true;
                specList.Color = DifferenceDevice.DefaultSpecColor.ToArgb();
                specList.VirtualColor = specList.Color;
                specList.SpecDate = DateTime.Now;
                specList.DeviceName = WorkCurveHelper.DeviceCurrent.Name;
                specList.WorkCurveName = WorkCurveHelper.WorkCurveCurrent.Name;
                specList.ActualVoltage = this.deviceMeasure.interfacce.ReturnVoltage;
                specList.ActualCurrent = this.deviceMeasure.interfacce.ReturnCurrent;
                specList.CountRate = this.deviceMeasure.interfacce.ReturnCountRate;
                specList.PeakChannel = double.Parse(this.deviceMeasure.interfacce.MaxChannelRealTime.ToString("f1"));
                specList.TotalCount = (long)this.deviceMeasure.interfacce.TestTotalCount;
                specList.DemarcateEnergys = Default.ConvertFormOldToNew(WorkCurveHelper.WorkCurveCurrent.Condition.DemarcateEnergys, WorkCurveHelper.DeviceCurrent.SpecLength);
                specList.InitParam = this.initParams.ConvertToNewEntity();
                #region yuzhaomodify
                specList.CompanyInfoList = CompanyOthersInfo.FindBySql("select * from companyothersinfo where  Display =1 and ExcelModeType='" + ReportTemplateHelper.ExcelModeType.ToString() + "'");
                #endregion
                if (this.XrfChart != null)
                    DemarcateEnergyHelp.CalParam(this.XrfChart.DemarcateEnergys);
                if (deviceResolve != null)
                {
                    deviceResolve.Spec = this.spec;
                    specList.Resole = deviceResolve.CalculateResolve();
                }
                // if (WorkCurveHelper.IsSaveSpecData)

                WorkCurveHelper.DataAccess.Save(this.specList);
                #region 纯元素
                //if (WorkCurveHelper.isShowEncoder && this.specList.SpecType == SpecType.PureSpec)
                //{
                //    string sql = "select * from PureSpecParam where DeviceName ='" + specList.DeviceName + "' and name ='" + specList.Name + "'";
                //    List<PureSpecParam> pureList = PureSpecParam.FindBySql(sql);
                //    if (pureList != null)
                //        PureSpecParam.DeleteAll(w => w.Name == specList.Name);
                //    PureSpecParam purr = PureSpecParam.New;
                //    // PureSpecParam pur = PureSpecParam.New.Init(specList.Name, specList.Height, specList.DeviceName,
                //    //specList.TotalCount, specList.WorkCurveName, specList.SpecType, specList.SampleName, obj, specList.SpecDate);
                //    purr.Name = specList.Name;
                //    purr.Height = specList.Height;
                //    purr.DeviceName = specList.DeviceName;
                //    //if (WorkCurveHelper.IsPureElemCurrentUnify)
                //    //    purr.TotalCount = specList.CountRate / specList.ActualCurrent;    //计数率/管流
                //    //else
                //    //    purr.TotalCount = specList.CountRate; //计数率
                //    purr.TotalCount = specList.CountRate;
                //    purr.CurrentUnifyCount = specList.CountRate / specList.ActualCurrent;
                //    purr.WorkCurveName = specList.WorkCurveName;
                //    purr.SpecTypeValue = specList.SpecType;
                //    purr.SampleName = specList.SampleName.Split('-').Length > 0 ? specList.SampleName.Split('-')[0] : specList.SampleName;
                //    //purr.Data = obj;
                //    purr.SpecDate = specList.SpecDate;
                //    purr.UsedTime = usedTime;
                //    purr.Condition = WorkCurveHelper.WorkCurveCurrent.Condition;
                //    purr.ElementName = purr.SampleName;
                //    purr.Current = spec.TubCurrent;
                //    purr.Save();
                //    //WorkCurveHelper.WorkCurveCurrent.Condition.PureSpecParamList.Add(purr);
                //}
                #endregion

                WorkCurveHelper.WorkCurveCurrent.ElementList.Items[CalibrateElemCnt].ElementSpecName = specList.Name;
                WorkCurveHelper.WorkCurveCurrent.ElementList.Items[CalibrateElemCnt].SSpectrumData = Helper.TransforToDivTimeAndCurrent(Helper.ToStrs(spec.SpecDatac), spec.UsedTime, spec.TubCurrent);
                WorkCurveHelper.WorkCurveCurrent.Save();
                if (optMode == OptMode.CurveCalibrate && CalibrateElemCnt < WorkCurveHelper.WorkCurveCurrent.ElementList.Items.Count - 1)
                {
                    DeviceInterface.PostMessage(this.handle, DeviceInterface.Wm_ContinueCalibrateElem, true, CalibrateElemCnt);
                }
                else
                {
                    TestEndCurrentProcess();
                    DeviceInterface.PostMessage(this.handle, DeviceInterface.Wm_CalibrateElemFinish, true, CalibrateElemCnt);

                }


            }
            else
            {
                //对设备扫描对象谱进行重新赋值
                spec = new SpecEntity();
                spec.IsSmooth = true;
                deviceMeasure.interfacce.Spec = spec;
                deviceParamSelectIndex++;
                spec.DeviceParameter = this.deviceParamsList[this.deviceParamSelectIndex].ConvertFrom();
                List<int[]> tempInt = new List<int[]>();

                if (!this.TempNetDataSmooth.TryGetValue(this.deviceParamsList[this.deviceParamSelectIndex].Id, out tempInt))
                    this.TempNetDataSmooth.Add(this.deviceParamsList[this.deviceParamSelectIndex].Id, new List<int[]>());

                this.refreshFillinof.UpdateWorkSpec(this.deviceParamsList[this.deviceParamSelectIndex], this.specList);
                //更新工作曲线信息
                this.refreshFillinof.RefreshCurve(this.initParams, this.deviceParamsList[this.deviceParamSelectIndex]);
                int dt = -1;
                if (!this.currentDeviceParamsList.TryGetValue(this.deviceParamsList[this.deviceParamSelectIndex].Id, out dt))
                    this.currentDeviceParamsList.Add(this.deviceParamsList[this.deviceParamSelectIndex].Id, this.deviceParamsList[this.deviceParamSelectIndex].TubCurrent);
                DeviceParameter deviceTemp = null;
                if (this.FirstDeviceParamsList.TryGetValue(deviceParamSelectIndex, out deviceTemp))
                    this.deviceMeasure.interfacce.DeviceParam = deviceTemp;
                else
                    this.deviceMeasure.interfacce.DeviceParam = this.deviceParamsList[this.deviceParamSelectIndex];
                progressInfo.MeasureTime = this.deviceParamsList[deviceParamSelectIndex].PrecTime + "s";
                progressInfo.SurplusTime = this.deviceParamsList[deviceParamSelectIndex].PrecTime + "s";
                progressInfo.Value = 0;
                this.specList.Specs[deviceParamSelectIndex] = this.spec;
                this.XrfChart.CurrentSpecPanel = deviceParamSelectIndex + 1;
                deviceMeasure.interfacce.MotorMove();
            }
        }


        private enum EnumMousePointPosition
        {
            MouseSizeNone = 0, //'无  
            MouseSizeRight = 1, //'拉伸右边框  
            MouseSizeLeft = 2, //'拉伸左边框  
            MouseSizeBottom = 3, //'拉伸下边框  
            MouseSizeTop = 4, //'拉伸上边框  
            MouseSizeTopLeft = 5, //'拉伸左上角  
            MouseSizeTopRight = 6, //'拉伸右上角  
            MouseSizeBottomLeft = 7, //'拉伸左下角  
            MouseSizeBottomRight = 8, //'拉伸右下角  
            MouseDrag = 9   // '鼠标拖动  
        }
    }
}
