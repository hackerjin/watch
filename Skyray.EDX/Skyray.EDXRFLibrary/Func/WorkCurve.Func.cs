using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Common;
using System.Data.SQLite;
using Skyray.EDXRFLibrary.Spectrum;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Skyray.EDXRFLibrary
{
    partial class WorkCurve
    {
        ECWorkCurve ecwork = new ECWorkCurve();
        FpWorkCurve fpwork = new FpWorkCurve();
        ThWorkCurve thwork = new ThWorkCurve();
        ECWorkCurve xrfwork = new ECWorkCurve();

        private bool IsAdjustInit = false;
        public void SetIsAdjustInit(bool adjustvalue)
        {
            IsAdjustInit = adjustvalue;
        }

        public void SetFpCalibrated(bool bvalue)
        {
            FpCalibrated = bvalue;
        }
        private bool  FpCalibrated = false;

        public void SetCurrent(int value)
        {
            ConCurrent = value;
        }
        private int ConCurrent = 0;


        private double TotalThickness = 0;
        private bool IsCalcThickess = false;
        public void SetTotalThickness(double thickness, bool iscalc)
        {
            IsCalcThickess = iscalc;
            TotalThickness = thickness;
        }


        private SpecListEntity BasePureSpec;
        public void SetBasePureSpec(SpecListEntity basePureSpec)
        {
            BasePureSpec = basePureSpec;
        }

        //public void SetEncoderHeight(float value)
        //{
        //    EncoderHeight = value;
        //}
        //private float EncoderHeight = 0;

        private double Avalue = 0;
        private double Nvalue = 0;
        private double CalcLimit = 0;
        private double Kvalue = 0;
        private double Bvalue = 0;
        private double ElemAValue = 0;   //对cu影响公式
        private double ElemNValue = 0;
        private double ImpactAValue = 0;   //对NI影响公式
        private double ImpactNValue = 0;
        
        
        public void SetANValue(
            double avalue, double nvalue,double limit, double kvalue,double bvalue,
            double elemAValue, double elemNValue, double impactAValue, double impactNValue
            )
        {
            Avalue = avalue;
            Nvalue = nvalue;
            CalcLimit = limit;
            Kvalue = kvalue;
            Bvalue = bvalue;

            ElemAValue = elemAValue;
            ElemNValue = elemNValue;
            ImpactAValue = impactAValue;
            ImpactNValue = impactNValue;

        }

        public void SetAngle(string TubeAngle,bool isEncoder)
        {
            IsShowEncoder = isEncoder;
            if (isEncoder)
            {
                double x = 19.5;
                double y = 33.2;
                double outvaluex = 0;
                double outvaluey = 0;
                if (TubeAngle != null && TubeAngle.Split(',').Length > 1)
                {
                    string[] tubevalue = TubeAngle.Split(',');
                    if (double.TryParse(tubevalue[0], out outvaluex))
                        x = outvaluex;
                    if (double.TryParse(tubevalue[1], out outvaluey))
                        y = outvaluey;
                    if (y == 0)
                    {
                        x = 19.5;
                        y = 33.2;
                    }
                }
                AngleX = x;
                AngleY = y;
            }
        }
        private double AngleX = 0;
        private double AngleY = 0;
        private bool IsShowEncoder = false;


        private float CalcAnge(double Height)
        {

            float retAngle = 0;
            if (IsShowEncoder)
            {
                if (Height > 0)
                    retAngle = (float)(Math.Atan((AngleX + Height) / AngleY) / Math.PI * 180);
                else
                    retAngle = (float)Condition.Device.Tubes.Exit;
            }
            else
            {
                retAngle = (float)Condition.Device.Tubes.Exit;
            }
            return retAngle;
        }

        private List<CurveElement> lstCurveElemIntensity = new List<CurveElement>();

        public List<CurveElement> GetFirstLayerIntensity
        {
            get { return lstCurveElemIntensity; }
        }

        private bool _isOptionJoinFirstI = false;
        public bool IsOptionJoinFirstI
        {
            get { return _isOptionJoinFirstI; }
        }

        /// <summary>
        /// 计算强度
        /// </summary>
        /// <param name="specList"></param>
        /// <returns></returns>
        public bool CaculateIntensity(params SpecListEntity[] specList)
        {
            bool b = true;
            double baseIntensity = 0;
            foreach (var list in specList)
            {
                switch (FuncType)
                {
                    case FuncType.XRF:
                        b = Intensity.GetIntensity(ElementList, list.Specs.ToArray(), DemarcateEnergyHelp.k0, DemarcateEnergyHelp.k1, Condition.Device.Detector, Condition.Device.Detector.FixGaussDelta);
                        if (!b) return false;
                        break;
                    case FuncType.Thick:
                        if (IsBaseAdjust && BasePureSpec != null)
                        {

                           // int baselayer = ElementList.Items.ToList().Max(w => w.LayerNumber);
                            if (BasePureSpec != null && BasePureSpec.Specs != null)
                            {
                                b = Intensity.GetIntensity(ElementList, BasePureSpec.Specs.ToArray(), DemarcateEnergyHelp.k0, DemarcateEnergyHelp.k1, Condition.Device.Detector, Condition.Device.Detector.FixGaussDelta);
                                if (!b) return false;
                                if (ElementList.Items.Count > 0)
                                    baseIntensity = ElementList.Items[ElementList.Items.ToList().FindIndex(w=>w.LayerNumber ==1)].Intensity;   //使用第一层强度
                            }
                        }
                        if (CalcType == CalcType.PeakDivBase)
                        {
                            
                            b = Intensity.GetIntensity(ElementList, list.Specs.ToArray(), DemarcateEnergyHelp.k0, DemarcateEnergyHelp.k1, Condition.Device.Detector, Condition.Device.Detector.FixGaussDelta);
                            if (!b) return false;
                        }
                        else
                        {
                            if (FpWorkCurve.thickMode == ThickMode.NiCuNiFe)
                            {
                                //NiCuNiFe特殊曲线
                               b = CalcNiCuNiFeIntensity(list,false,"");
                            }
                            else if (FpWorkCurve.thickMode == ThickMode.NiCuNiFe2)
                            {
                                string elemname = ElementList.Items.ToList().Find(w => w.LayerNumber == 1).Caption;
                                b = CalcNiCuNiFeIntensityOther(list, true, elemname);
                            }
                            else
                            {
                                //计算介入强度
                                b = IntensityByBolder(list);
                            }
                            
                        
                            if (!b) return false;
                        }


                        //初始化强度校正
                        if (IsAdjustInit)
                        {
                            foreach (CurveElement ce in ElementList.Items)
                            {
                                ce.Intensity = ce.Intensity * Condition.InitParam.InitCalibrateRatio;
                            }
                        }

                        lstCurveElemIntensity.Clear();

                        //临时存放第一层强度
                        List<CurveElement> lstTempIntensity = ElementList.Items.ToList().FindAll(w => w.LayerNumber == 1);
                        if (lstTempIntensity.Count > 0)
                        {
                            // double[] tempIntensity = new double[lstTempIntensity.Count];
                            lstCurveElemIntensity.Clear();
                            for (int i = 0; i < lstTempIntensity.Count; i++)
                            {
                                CurveElement elem = CurveElement.New;
                                elem.Caption = lstTempIntensity[i].Caption;
                                elem.Intensity = lstTempIntensity[i].Intensity;
                                lstCurveElemIntensity.Add(elem);
                                // tempIntensity[i] = lstTempIntensity[i].Intensity;
                            }
                        }

                        _isOptionJoinFirstI = false; //优化前置false
                        ///数据优化4,进行强度修改
                        string[] Encodervalue = new string[ElementList.Items.Count];
                        string[] EncoderX = new string[ElementList.Items.Count];
                        for (int k = 0; k < ElementList.Items.Count; k++)
                        {
                            Encodervalue[k] = ElementList.Items[k].Intensity > 0 ? ElementList.Items[k].Intensity.ToString() : "0";
                            EncoderX[k] = ElementList.Items[k].Caption.ToLower();
                        }

                        for (int i = 0; i < ElementList.Items.Count; ++i)
                        {
                            for (int j = 0; j < ElementList.Items[i].Optimiztion.Count; j++)
                            {
                                if (ElementList.Items[i].Optimiztion[j].OptimizetionType == 3)
                                {
                                    if (!ElementList.Items[i].Optimiztion[j].IsJoinIntensity)
                                        _isOptionJoinFirstI = true;
                                    double optIntensity;
                                    #region 旧
                                    //if (FpWorkCurve.thickMode == ThickMode.NiCuNiFe && (ElementList.Items[i].Caption == "Ni"))
                                    //{
                                    //    //数据优化NI时，强度优化到Co上。
                                    //    CurveElement ceTemp = ElementList.Items.ToList().Find(w => w.Caption == "Co");
                                    //    if (ceTemp != null)
                                    //        optIntensity = ceTemp.Intensity;
                                    //    else
                                    //        continue;   //没有co时，Ni不优化
                                    //    ElementList.Items[i].Optimiztion[j].OptExpression = ElementList.Items[i].Optimiztion[j].OptExpression.Replace("Ni", "Co");
                                    //    if (optIntensity < 0) optIntensity = 0;
                                    //    if (optIntensity > ElementList.Items[i].Optimiztion[j].OptimiztionMin
                                    //       && optIntensity <= ElementList.Items[i].Optimiztion[j].OptimiztionMax)
                                    //    {
                                    //        double retEncodervalue = 0;
                                    //        CustomFieldByFortum(ElementList.Items[i].Optimiztion[j].OptExpression, EncoderX, Encodervalue, 0, out retEncodervalue);
                                    //        ElementList.Items.ToList().Find(w => w.Caption == "Co").Intensity = retEncodervalue;
                                    //    }
                                    //}
                                    #endregion
                                    if ((FpWorkCurve.thickMode == ThickMode.NiCuNiFe || FpWorkCurve.thickMode == ThickMode.NiCuNiFe2) && (ElementList.Items[i].Caption == "Fe"))
                                    {
                                        //数据优化NI时，强度优化到Co上。
                                        CurveElement ceTemp = ElementList.Items.ToList().Find(w => w.Caption == "Co");
                                        if (ceTemp != null)
                                            optIntensity = ceTemp.Intensity;
                                        else
                                            continue;   //没有co时，Ni不优化
                                        ElementList.Items[i].Optimiztion[j].OptExpression = ElementList.Items[i].Optimiztion[j].OptExpression.Replace("Fe", "Co");
                                        if (optIntensity < 0) optIntensity = 0;
                                        if (optIntensity > ElementList.Items[i].Optimiztion[j].OptimiztionMin
                                           && optIntensity <= ElementList.Items[i].Optimiztion[j].OptimiztionMax)
                                        {
                                            double retEncodervalue = 0;
                                            CustomFieldByFortum(ElementList.Items[i].Optimiztion[j].OptExpression, EncoderX, Encodervalue, 0, out retEncodervalue);
                                            ElementList.Items.ToList().Find(w => w.Caption == "Co").Intensity = retEncodervalue;
                                        }
                                    }
                                    else
                                    {
                                        optIntensity = ElementList.Items[i].Intensity;
                                        if (optIntensity < 0) optIntensity = 0;
                                        if (optIntensity >= ElementList.Items[i].Optimiztion[j].OptimiztionMin
                                        && optIntensity <= ElementList.Items[i].Optimiztion[j].OptimiztionMax)
                                        {
                                            double retEncodervalue = 0;
                                            CustomFieldByFortum(ElementList.Items[i].Optimiztion[j].OptExpression, EncoderX, Encodervalue, 0, out retEncodervalue);
                                            ElementList.Items[i].Intensity = retEncodervalue;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                }

                //增加管流归一的功能
                if (IsCurrentNormalize)
                {
                    for (int i = 0; i < ElementList.Items.Count; i++)
                    {
                        double current = list.Specs[ElementList.Items[i].ConditionID].TubCurrent;
                        ElementList.Items[i].Intensity = ElementList.Items[i].Intensity < 0 ? 0 : ElementList.Items[i].Intensity / current;
                    }
                }

                if (IsBaseAdjust)
                {
                    for (int i = 0; i < ElementList.Items.Count; i++)
                    {
                        if (baseIntensity > 0)
                        {
                            ElementList.Items[i].Intensity = ElementList.Items[i].Intensity / baseIntensity;
                        }
                    }
                }
               
              
            }

            return b;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private bool CalcNiCuNiFeIntensityOther(SpecListEntity list, bool isAddElem, string AddElem)
        {
            bool b = false;
            try
            {
                double cuKbIntensity = 0;
                double cuKaIntensity = 0;
                //cu kb
                CurveElement cuElem = ElementList.Items.ToList().Find(w => w.Caption == "Cu");
                int high = cuElem.PeakHigh;
                int low = cuElem.PeakLow;
                cuElem.AnalyteLine = XLine.Kb;
                double cuEnergy = Atoms.AtomList.Find(a => a.AtomName == cuElem.Caption).Kb;
                int channel = DemarcateEnergyHelp.GetChannel(cuEnergy);
                cuElem.PeakLow = channel - 20 < 0 ? 0 : channel - 20;
                cuElem.PeakLow = cuElem.PeakLow > 4096 ? 4095 : cuElem.PeakLow;
                cuElem.PeakHigh = channel + 20 > 4096 ? 4095 : channel + 20;
                cuElem.PeakHigh = cuElem.PeakHigh < 0 ? 0 : cuElem.PeakHigh;
                if (IntensityByBolder(list))
                {
                    cuKbIntensity = cuElem.Intensity;
                }
                cuElem.AnalyteLine = XLine.Ka;
                cuElem.PeakLow = low;
                cuElem.PeakHigh = high;

                //计算介入强度
                b = IntensityByBolder(list);
                if (!b)
                {
                    return false;
                }
                //ni元素
                CurveElement em = ElementList.Items.ToList().Find(w => w.Caption == "Ni");
                cuKaIntensity = ElementList.Items.ToList().Find(w => w.Caption == "Cu").Intensity;
                double kbka = cuKaIntensity > 0 ? cuKbIntensity / cuKaIntensity : 0;
               
                //ag对kbka影响的系数
                double ImpactAg = ElemAValue * Math.Exp(ElemNValue * kbka);
                double coeff = kbka / ImpactAg;

                double TheoryNi1 = Avalue * Math.Exp(Nvalue * coeff);

                double elemFirstIntensity = ElementList.Items.ToList().Find(w => w.LayerNumber == 1).Intensity;

                //第一层元素影响因子
                double firstElemCoeff = ImpactAValue * Math.Log(elemFirstIntensity) + ImpactNValue;

                double Ni1Intensity = firstElemCoeff * TheoryNi1;  //因子*理论

                Console.WriteLine("emcaption:" + em.Caption +":"+em.Intensity.ToString());
                double N2Intensity = em.Intensity - Ni1Intensity > 0 ? em.Intensity - Ni1Intensity : 0;


                CurveElement addElem = CurveElement.New.Init("Co", false, "Co", 27, 4, "第四层", XLine.Ka, 0, 0, 0, 0, false, 0, 0, 0, 0.00,
               IntensityWay.NetArea, false, 0, 0, 0, CalculationWay.Insert, FpCalculationWay.LinearWithoutAnIntercept, ElementFlag.Calculated, LayerFlag.Calculated,
                   ContentUnit.per, ThicknessUnit.um, "", "", "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", false, Color.Red.ToArgb(), " ", 0, "", false, true, true, false, 1, 0, "", "", "", false, "Co");
                addElem.DevParamId = em.DevParamId;
                if (!ElementList.Items.Contains(addElem))
                    ElementList.Items.Add(addElem);
                ElementList.Items.ToList().Find(w => w.Caption == "Fe").LayerNumber = 5;

                foreach (var elem in ElementList.Items)
                {
                    switch (elem.Caption)
                    {
                        case "Ni":
                            elem.Intensity = Ni1Intensity;
                            break;
                        case "Cu":
                            elem.Intensity = cuKaIntensity;
                            break;
                        case "Co":
                            elem.Intensity = N2Intensity;
                            break;
                    }

                }
                return true;

                //foreach (var elem in ElementList.Items)
                //{
                //    switch (elem.Caption)
                //    {
                //        case "Ag":
                //            elem.Intensity = 0.22391;
                //            break;
                //        case "Ni":
                //            elem.Intensity = 0.0997;
                //            break;
                //        case "Cu":
                //            elem.Intensity = 0.03138;
                //            break;
                //        case "Co":
                //            elem.Intensity = 0.03804;
                //            break;
                //        case "Fe":
                //            elem.Intensity = 0.00309;
                //            break;
                //    }

                //}
                return true;
            }
            catch
            {
                return false;
            }

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private bool CalcNiCuNiFeIntensity(SpecListEntity list,bool isAddElem ,string AddElem)
        {
            bool b = false;
            double cuKbIntensity = 0;
            double cuKaIntensity = 0;
            //cu kb
            CurveElement cuElem = ElementList.Items.ToList().Find(w => w.Caption == "Cu");
            int high = cuElem.PeakHigh;
            int low = cuElem.PeakLow;
            cuElem.AnalyteLine = XLine.Kb;
            double cuEnergy = Atoms.AtomList.Find(a => a.AtomName == cuElem.Caption).Kb;
            int channel = DemarcateEnergyHelp.GetChannel(cuEnergy);
            cuElem.PeakLow = channel - 20 < 0 ? 0 : channel - 20;
            cuElem.PeakLow = cuElem.PeakLow > 4096 ? 4095 : cuElem.PeakLow;
            cuElem.PeakHigh = channel + 20 > 4096 ? 4095 : channel + 20;
            cuElem.PeakHigh = cuElem.PeakHigh < 0 ? 0 : cuElem.PeakHigh;
            if (IntensityByBolder(list))
            {
                cuKbIntensity = cuElem.Intensity;
            }
            cuElem.AnalyteLine = XLine.Ka;
            cuElem.PeakLow = low;
            cuElem.PeakHigh = high;

            //计算介入强度
            b = IntensityByBolder(list);
            if (!b)
            {
                return false;
            }
            //ni元素
            CurveElement em = ElementList.Items.ToList().Find(w => w.Caption == "Ni");
            cuKaIntensity = ElementList.Items.ToList().Find(w => w.Caption == "Cu").Intensity;
            double rate = cuKaIntensity > 0 ? cuKbIntensity / cuKaIntensity : 0;
            double totalNi = em.Intensity;

            double Ni1 = 0;
            if (rate < CalcLimit)
            {
                Ni1 = Avalue * Math.Exp(Nvalue * rate);
            }
            else
            {
                Ni1 = Kvalue * rate + Bvalue;
            }

            double Ni3 = (totalNi - Ni1) > 0 ? totalNi - Ni1 : 0;
            double FeIntensity = ElementList.Items.ToList().Find(w => w.Caption == "Fe").Intensity;

            CurveElement addElem = CurveElement.New.Init("Co", false, "Co", 27, 3, "第三层", XLine.Ka, 0, 0, 0, 0, false, 0, 0, 0, 0.00,
            IntensityWay.NetArea, false, 0, 0, 0, CalculationWay.Insert, FpCalculationWay.LinearWithoutAnIntercept, ElementFlag.Calculated, LayerFlag.Calculated,
                ContentUnit.per, ThicknessUnit.um, "", "", "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", false, Color.Red.ToArgb(), " ", 0, "", false, true, true, false, 1, 0, "", "", "", false, "Co");
            addElem.DevParamId = em.DevParamId;
            if (!ElementList.Items.Contains(addElem))
                ElementList.Items.Add(addElem);
            ElementList.Items.ToList().Find(w => w.Caption == "Fe").LayerNumber = 4;

            foreach (var elem in ElementList.Items)
            {
                switch (elem.Caption)
                {
                    case "Ni":
                        elem.Intensity = Ni1;
                        break;
                    case "Cu":
                        elem.Intensity = cuKaIntensity;
                        break;
                    case "Co":
                        elem.Intensity = Ni3;
                        break;
                    case "Fe":
                        elem.Intensity = FeIntensity;
                        break;
                }

            }
            return true;

        }
        /// <summary>
        /// 介入计算强度
        /// </summary>
        private bool IntensityByBolder(SpecListEntity list)
        {
            bool b = false;
            try
            {
                //临时存放
                List<CurveElement> currentLstItems = ElementList.Items.ToList().FindAll(w => w.IsBorderlineElem == true);
                List<TempElement> tempElemList = new List<TempElement>();

                if (currentLstItems != null && currentLstItems.Count > 0)
                {
                    foreach (CurveElement elem in currentLstItems)
                    {

                        TempElement tempElem = new TempElement();
                        tempElem.Caption = elem.Caption;

                        string tempPureSSpectrumn = elem.SSpectrumData;   //Au临时存放纯元素
                        double[] pureData = Helper.ToDoubles(elem.SSpectrumData);
                        double[] tempData = new double[pureData.Length];
                        double sumtemp = 0;
                        double sumspec = 0;
                        for (int i = 0; i < pureData.Length; i++)
                        {
                            if (i >= elem.PeakLow && i <= elem.PeakHigh)
                            {
                                tempData[i] = pureData[i];
                            }
                        }
                        ElementList.Items.ToList().Find(w => w.Caption == elem.Caption).SSpectrumData = Helper.ToStrs(tempData);
                        b = Intensity.GetThickIntensity(ElementList, list.Specs.ToArray(), DemarcateEnergyHelp.k0, DemarcateEnergyHelp.k1, Condition.Device.Detector, Condition.Device.Detector.FixGaussDelta, ConCurrent);
                        if (!b)
                        {
                            return false;
                        }
                        tempElem.Intensity = ElementList.Items.ToList().Find(w => w.Caption == elem.Caption).Intensity;
                        ElementList.Items.ToList().Find(w => w.Caption == elem.Caption).SSpectrumData = tempPureSSpectrumn;
                        tempElemList.Add(tempElem);
                    }
                }
                b = Intensity.GetThickIntensity(ElementList, list.Specs.ToArray(), DemarcateEnergyHelp.k0, DemarcateEnergyHelp.k1, Condition.Device.Detector, Condition.Device.Detector.FixGaussDelta, ConCurrent);
                if (!b)
                {
                    return false;
                }
                if (tempElemList != null && tempElemList.Count > 0)
                {
                    foreach (TempElement temp in tempElemList)
                    {
                        ElementList.Items.ToList().Find(w => w.Caption == temp.Caption).Intensity = temp.Intensity;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        private CurveElement GetAuLaPeak(CurveElement element)
        {
            double energy = 0;
            Atom atom = Atoms.AtomList.Find(a => a.AtomName == element.Caption);
            int Id = 0;
            if (atom != null)
            {
                energy = Atoms.AtomList.Find(a => a.AtomName == element.Caption).La;
                element.AnalyteLine = XLine.La;
                int channel = DemarcateEnergyHelp.GetChannel(energy);
                int specLength = (int)element.SpectrumData.Length;
                element.PeakLow = channel - 10 < 0 ? 0 : channel - 10;  //10左右通道区间
                element.PeakLow = element.PeakLow > specLength ? specLength - 1 : element.PeakLow;
                element.PeakHigh = channel + 10 > specLength ? specLength - 1 : channel + 10;
                element.PeakHigh = element.PeakHigh < 0 ? 0 : element.PeakHigh;
                if (element.PeakLow > element.PeakHigh) element.PeakLow = element.PeakHigh;
            }
            else
            {
            }
            return element;
        }

        public bool CheckTotalArea( SpecListEntity specList)
        {
            for (int i = 0; i < ElementList.Items.Count; i++)
            {
                double TotalArea =SpecHelper.TotalArea(ElementList.Items[i].PeakLow,ElementList.Items[i].PeakHigh,specList.Specs[ElementList.Items[i].ConditionID].SpecDatas);
                if (TotalArea < SpecHelper.ElemCountLimit) return false;
            }
            return true;
        }

      

        #region CalculateContentOld
        public bool CacultateContent(SpecListEntity specList)
        {
            if (IntensityCalibration != null && IntensityCalibration.Count > 0)
            {
                //add by chuyaqin 2011-04-24 强度校正算法的加入
                for (int i = 0; i < ElementList.Items.Count; i++)
                {
                    double ReviseCoef = 1.0;
                    double ReviseK = 0;
                    IntensityCalibration ic = IntensityCalibration.ToList().Find(w => w.Element == ElementList.Items[i].Caption);
                    if (ic != null && ic.CalibrateIn != 0 && ic.OriginalIn != 0)
                    {
                        ReviseCoef = (ic.InCalType == 2 ? (ic.CalibrateIn - ic.CalibrateBaseIn) / (ic.OriginalIn - ic.OriginalBaseIn) : (ic.CalibrateIn / ic.OriginalIn));
                        ReviseK = ic.InCalType == 2 ? ic.CalibrateIn - ic.OriginalIn * (ic.CalibrateIn - ic.CalibrateBaseIn) / (ic.OriginalIn - ic.OriginalBaseIn) : ReviseK;
                    }
                    ReviseCoef = Double.IsInfinity(ReviseCoef) || Double.IsNaN(ReviseCoef) ? 1.0 : ReviseCoef;
                    ReviseK = Double.IsInfinity(ReviseK) || Double.IsNaN(ReviseK) ? 0 : ReviseK;
                    ElementList.Items[i].Intensity = ElementList.Items[i].Intensity * ReviseCoef + ReviseK;
                }
            }

            switch (FuncType)
            {

                case FuncType.XRF:
                    bool b1 = false;
                    if (CalcType == CalcType.EC)
                    {
                        //return xrfwork.GetECContent(ElementList, ElementList.RhIsLayer, ElementList.RhLayerFactor);
                        b1 = xrfwork.GetECContent(ElementList, ElementList.RhIsLayer, ElementList.RhLayerFactor, specList);
                    }
                    else
                    {
                        #region 20200604 cyq
                        int totalLayer = 1;
                        float[] layerDensitys = new float[1];
                        layerDensitys[0] = 0.0f;
                        float[] layerThickness = new float[1];

                        string strCurrentDir = System.IO.Directory.GetCurrentDirectory();
                        // FpWorkCurve.CopyFPFiles(AppDomain.CurrentDomain.BaseDirectory + "\\fpFiles\\", AppDomain.CurrentDomain.BaseDirectory + "\\FpWorkTemp\\");
                        System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
                        if (!FpCalibrated)
                        {
                            System.IO.File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\fpFiles\\temp.cal", "temp.cal", true);
                        }
                        //bool b = false;
                        if (FpWorkCurve.FPSetRootPath("./FpFiles"))//"./FpWorkTemp"
                        {
                            // System.IO.File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\fpFiles\\temp.cal", AppDomain.CurrentDomain.BaseDirectory + "\\FpWorkTemp\\temp.cal", true);
                            // System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\FpWorkTemp");
                            if (FpWorkCurve.SetSourceData(Condition, Condition.Device.Tubes, CalcAnge(specList.Height)))
                            {
                                List<string> listSamples;
                                if (!FpCalibrated)
                                {
                                    FpCalibrated = fpwork.Calibrate(ElementList, 0, out listSamples);
                                }
                                if (FpCalibrated && fpwork.CalContent(ElementList, totalLayer, layerDensitys, ref layerThickness, 0, specList))
                                {
                                    b1 = true;
                                    //铑是镀层的添加
                                    //if (ElementList.RhIsLayer && ElementList.Items.First(w => w.Caption.ToUpper() == "RH") != null)
                                    //{
                                    //    ElementList.Items.First(w => w.Caption.ToUpper() == "RH").Thickness = layerThickness[0];
                                    //}
                                    if (ElementList.RhIsLayer)
                                    {
                                        string[] strElems = Helper.ToStrs(ElementList.LayerElemsInAnalyzer);
                                        foreach (string eleStr in strElems)
                                        {
                                            if (ElementList.Items.Count(w => w.Caption.ToUpper() == eleStr.ToUpper()) <= 0) continue;
                                            ElementList.Items.First(w => w.Caption.ToUpper() == eleStr.ToUpper()).Thickness = layerThickness[0];
                                        }
                                    }
                                }
                                //b = fpwork.CalContent(ElementList, totalLayer, layerDensitys, ref layerThickness);
                            }
                        }
                        System.IO.Directory.SetCurrentDirectory(strCurrentDir);
                        #endregion


                    }
                    GetGadeContent(ElementList);
                    return b1;
                default:
                    return false;
            }
        }
        #endregion

        private double GetEmCalibrateCoef(double ReviseCoef, double emch,bool IsBaseDIV)
        {
            int SpecLeft=0, SpecRight=2048;
            foreach (var p in Condition.DeviceParamList)
            {
                if (emch < p.EndChann && emch > p.BeginChann)
                {
                    SpecLeft = p.BeginChann;
                    SpecRight = p.EndChann;
                    break;
                }
            }

            List<string> empeakNames = new List<string>();
            List<double> empeakchs = new List<double>();
            List<double> empeakCoefs = new List<double>();
            double coef1 = 1.0f, coef2 = 1.0f, ch1=0f, ch2=0f, chTemp = SpecRight - SpecLeft;
            double baseCoef = 1.0f;
            foreach (var inc in IntensityCalibration)
            {

                double inch = (inc.PeakLeft + inc.PeakRight) / 2;
                if (inch > SpecRight || inch < SpecLeft || inc.OriginalIn <= 0 || inc.CalibrateIn <= 0) continue;
                double coeftemp = inc.CalibrateIn / inc.OriginalIn;
                if (Math.Abs(emch - inch) < chTemp)
                {
                    coef1 = coeftemp;
                    ch1 = inch;
                    chTemp = Math.Abs(emch - inch);
                    baseCoef=inc.OriginalBaseIn>0?inc.CalibrateBaseIn/inc.OriginalBaseIn:1;
                }
                empeakNames.Add(inc.Element);
                empeakCoefs.Add(coeftemp);
                empeakchs.Add(inch);
            }
            chTemp = SpecRight - SpecLeft;
            for (int j = 0; j < empeakchs.Count; j++)
            {
                if (Math.Abs(emch - empeakchs[j]) < chTemp && empeakchs[j] != ch1)
                {
                    coef2 = empeakCoefs[j];
                    ch2 = empeakchs[j];
                    chTemp = Math.Abs(emch - empeakchs[j]);
                }
            }
            ReviseCoef = (Math.Abs(emch - ch1) < 2 || empeakchs.Count<=1) ? coef1 : (coef2 - coef1) * emch / (ch2 - ch1) + (coef2 - (coef2 - coef1) * ch2 / (ch2 - ch1));
            ReviseCoef = (Double.IsInfinity(ReviseCoef)||Double.IsNaN(ReviseCoef)) ? 1.0f : ReviseCoef;
            if (IsBaseDIV)
                ReviseCoef /= baseCoef;
            return ReviseCoef;
        }

        /// <summary>
        /// 测厚计算
        /// </summary>
        /// <param name="layerThickness"></param>
        /// <returns></returns>
        public bool CaculateThick(SpecListEntity specList, double ThicknessLimit, bool IsOptimiztion)
        {
            switch (FuncType)
            {
                case FuncType.Thick:
                    if (CalcType == CalcType.EC)
                    {
                        ElementList.DBlLimt = 0.99999;
                        //return CaculateThick(ElementList.IsAbsorption, ElementList.ThCalculationWay, ElementList.DBlLimt);
                        //if (!CaculateThick(ElementList.IsAbsorption, ElementList.ThCalculationWay, ElementList.DBlLimt))
                        if (!CaculateThick(this.IsAbsorb, ElementList.ThCalculationWay, ElementList.DBlLimt))
                        {
                            return false;
                        }
                    }
                    else if (CalcType == CalcType.FP)
                    {
                        List<int> layers = new List<int>();
                        List<float> densitys = new List<float>();
                        int maxLayer = 0;
                        for (int i = 0; i < ElementList.Items.Count; i++)
                        {
                            if (!layers.Contains(ElementList.Items[i].LayerNumber))
                            {
                                layers.Add(ElementList.Items[i].LayerNumber);
                                if (maxLayer < ElementList.Items[i].LayerNumber)
                                {
                                    maxLayer = ElementList.Items[i].LayerNumber;
                                }
                                densitys.Add((float)ElementList.Items[i].LayerDensity);
                            }
                        }
                        float[] laydensitys = new float[maxLayer];
                        for (int i = 0; i < layers.Count; i++)
                        {
                            laydensitys[layers[i] - 1] = 0;  //不需要面密度参与  // densitys[i];
                        }
                        //return CaculateThick(maxLayer, laydensitys);

                        if (!CaculateThick(maxLayer, laydensitys, specList, ThicknessLimit))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        bool b1 = false;
                        if (IntensityCalibration != null && IntensityCalibration.Count > 0)
                        {
                            //add by chuyaqin 2011-04-24 强度校正算法的加入
                            for (int i = 0; i < ElementList.Items.Count; i++)
                            {
                                //强度校正算法的修改-----------2014-02-17--------------------------------------------
                                //IntensityCalibration ic = IntensityCalibration.ToList().Find(w => w.Element == ElementList.Items[i].Caption);
                                //double ReviseCoef = 1.0;
                                //if (ic != null && ic.CalibrateIn != 0 && ic.OriginalIn != 0)
                                //{
                                //    ReviseCoef = Double.IsInfinity(ic.CalibrateIn / ic.OriginalIn) ? 1.0 : ic.CalibrateIn / ic.OriginalIn;
                                //}
                                //ElementList.Items[i].Intensity = ElementList.Items[i].Intensity * ReviseCoef;
                                double ReviseCoef = 1.0;
                                double emch = (ElementList.Items[i].PeakLow + ElementList.Items[i].PeakHigh) / 2.0f;
                                ReviseCoef = GetEmCalibrateCoef(ReviseCoef, emch, ElementList.Items[i].PeakDivBase&&this.InCalType==2);
                                ElementList.Items[i].Intensity = ElementList.Items[i].Intensity * ReviseCoef;
                                if (FpWorkCurve.thickMode == ThickMode.Plating)
                                {
                                    //elementlist.Items[ecElementIndexList[i]].Intensity = elementarray[0].Intensity;
                                    
                                }
                            }
                        }
                       
                            //return xrfwork.GetECContent(ElementList, ElementList.RhIsLayer, ElementList.RhLayerFactor);
                        if (!xrfwork.GetECContent(ElementList, ElementList.RhIsLayer, ElementList.RhLayerFactor, specList))
                        {
                            return false;
                        }
                        GetGadeContent(ElementList);
                      
                       
                    }
                    for (int i = 0; i < ElementList.Items.Count; ++i)
                    {
                        double value =0;
                        if (CalcType == CalcType.PeakDivBase)
                        { 
                            ElementList.Items[i].Thickness = ElementList.Items[i].Content;

                        }
                        if (IsOptimiztion)  //为false时不需要优化
                        {
                           
                            value = ElementList.Items[i].Thickness;
                           
                            for (int j = 0; j < ElementList.Items[i].Optimiztion.Count; j++)
                            {
                                if (ElementList.Items[i].Optimiztion[j].OptimizetionType == 0)
                                {
                                    double optimiztion = ElementList.Items[i].Optimiztion[j].OptimiztionValue;
                                    if (ElementList.Items[i].ThicknessUnit == ThicknessUnit.ur)
                                    {
                                        optimiztion = optimiztion * 0.0254f;
                                        if (value - ElementList.Items[i].Optimiztion[j].OptimiztionValue * 0.0254f >= ElementList.Items[i].Optimiztion[j].OptimiztionMin * 0.0254f
                                    && value - ElementList.Items[i].Optimiztion[j].OptimiztionValue * 0.0254f <= ElementList.Items[i].Optimiztion[j].OptimiztionMax * 0.0254f)
                                        {
                                            value = value + (optimiztion - value) * ElementList.Items[i].Optimiztion[j].OptimiztionFactor;
                                            ElementList.Items[i].Thickness = value;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (value - ElementList.Items[i].Optimiztion[j].OptimiztionValue >= ElementList.Items[i].Optimiztion[j].OptimiztionMin
                                        && value - ElementList.Items[i].Optimiztion[j].OptimiztionValue <= ElementList.Items[i].Optimiztion[j].OptimiztionMax)
                                        {
                                            value = value + (optimiztion - value) * ElementList.Items[i].Optimiztion[j].OptimiztionFactor;
                                            ElementList.Items[i].Thickness = value;
                                            break;
                                        }
                                    }
                                }
                                else if (ElementList.Items[i].Optimiztion[j].OptimizetionType == 1)  // 直接乘系数
                                {
                                    if ((FpWorkCurve.thickMode == ThickMode.NiCuNiFe || FpWorkCurve.thickMode == ThickMode.NiCuNiFe2) && (ElementList.Items[i].Caption == "Fe"))
                                    {
                                        value = ElementList.Items.ToList().Find(w => w.Caption == "Co").Thickness;
                                        value = value * ElementList.Items[i].Optimiztion[j].OptimiztionFactor;
                                        ElementList.Items.ToList().Find(w => w.Caption == "Co").Thickness = value;
                                    }
                                    else
                                    {
                                        value = value * ElementList.Items[i].Optimiztion[j].OptimiztionFactor;
                                        ElementList.Items[i].Thickness = value;
                                    }
                                }
                                else if (ElementList.Items[i].Optimiztion[j].OptimizetionType == 2)
                                {
                                    string[] Encodervalue = new string[1];
                                    string[] EncoderX = new string[1];
                                    double retEncodervalue = 0;
                                    Encodervalue[0] = ElementList.Items[i].Thickness.ToString();
                                    EncoderX[0] = "x";
                                    CustomFieldByFortum(ElementList.Items[i].Optimiztion[j].OptExpression, EncoderX, Encodervalue, 0, out retEncodervalue);
                                    ElementList.Items[i].Thickness = retEncodervalue;

                                }
                            }
                        }
                    }
                    return true;
                default:
                    return false;
            }
        }


        public static bool CustomFieldByFortum(string expression, string[] elements, string[] values, int index, out double outValue)
        {
            outValue = double.Epsilon;
            if (elements == null || elements.Length == 0 || values == null
            || values.Length == 0 || elements.Length != values.Length)
                return false;
            for (int i = 0; i < elements.Length; i++)
            {
                string[] str = values[i].Split(',');
                for (int j = 0; j < str.Length; j++)
                {
                    if (j == index)
                    {
                        if (str[j] == "")
                            return false;
                        string[] exps = expression.Split(new char[] { '+', '-', '*', '/', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int k = 0; k < exps.Length; k++)
                        {
                            if (String.Compare(exps[k], elements[i], true) == 0)
                                expression = expression.Replace(exps[k], str[j]);
                        }
                        break;
                    }
                }
            }

            outValue = Parse(expression);
            return true;
        }


        #region 求值经典算法
        /// <summary>
        /// 求值的经典算法
        /// </summary>
        /// <param name="expression">字符串表达式</param>
        /// <returns></returns>
        public static double Parse(string expression)
        {
            string postfixExpression = InfixToPostfix(expression);
            Stack<double> results = new Stack<double>();
            double x, y;
            for (int i = 0; i < postfixExpression.Length; i++)
            {
                char ch = postfixExpression[i];
                if (char.IsWhiteSpace(ch)) continue;
                switch (ch)
                {
                    case '+':
                        y = results.Pop();
                        x = results.Pop();
                        results.Push(x + y);
                        break;
                    case '-':
                        y = results.Pop();
                        x = results.Pop();
                        results.Push(x - y);
                        break;
                    case '*':
                        y = results.Pop();
                        x = results.Pop();
                        results.Push(x * y);
                        break;
                    case '/':
                        y = results.Pop();
                        x = results.Pop();
                        results.Push(x / y);
                        break;
                    default:
                        int pos = i;
                        StringBuilder operand = new StringBuilder();
                        do
                        {
                            operand.Append(postfixExpression[pos]);
                            pos++;
                        } while (char.IsDigit(postfixExpression[pos]) || postfixExpression[pos] == '.');
                        i = --pos;

                        double d = 0.0;
                        try
                        {
                            d = double.Parse(operand.ToString());
                        }
                        catch (System.Exception)
                        {

                        }
                        results.Push(d);
                        break;
                }
            }
            return results.Peek();
        }
        #endregion


        #region 中缀转后缀
        /// <summary>
        /// 中缀表达式转换为后缀表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string InfixToPostfix(string expression)
        {
            Stack<char> operators = new Stack<char>();
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < expression.Length; i++)
            {
                char ch = expression[i];
                if (char.IsWhiteSpace(ch)) continue;
                switch (ch)
                {
                    case '+':
                    case '-':
                        while (operators.Count > 0)
                        {
                            char c = operators.Pop();   //pop Operator
                            if (c == '(')
                            {
                                operators.Push(c);      //push Operator
                                break;
                            }
                            else
                            {
                                result.Append(c);
                            }
                        }
                        operators.Push(ch);
                        result.Append(" ");
                        break;
                    case '*':
                    case '/':
                        while (operators.Count > 0)
                        {
                            char c = operators.Pop();
                            if (c == '(')
                            {
                                operators.Push(c);
                                break;
                            }
                            else
                            {
                                if (c == '+' || c == '-')
                                {
                                    operators.Push(c);
                                    break;
                                }
                                else
                                {
                                    result.Append(c);
                                }
                            }
                        }
                        operators.Push(ch);
                        result.Append(" ");
                        break;
                    case '(':
                        operators.Push(ch);
                        break;
                    case ')':
                        while (operators.Count > 0)
                        {
                            char c = operators.Pop();
                            if (c == '(')
                            {
                                break;
                            }
                            else
                            {
                                result.Append(c);
                            }
                        }
                        break;
                    default:
                        result.Append(ch);
                        break;
                }
            }
            while (operators.Count > 0)
            {
                result.Append(operators.Pop()); //pop All Operator
            }
            return result.ToString();
        }
        #endregion
        /// <summary>
        /// th'计算厚度（私有）
        /// </summary>
        /// <param name="isAbsorption">是否自吸收</param>
        /// <param name="intCalculationWay">厚度计算方法</param>
        /// <param name="dblLimit">极限值</param>
        /// <returns>是否计算成功</returns>
        private bool CaculateThick(bool isAbsorption, ThCalculationWay intCalculationWay, double dblLimit)
        {
            ElementList.DBlLimt = 0.99999;
            if (thwork.GetThick(ElementList, isAbsorption, intCalculationWay, dblLimit))
            {
                return true;
            }
            else return false;

        }


        [DllImport("Thick.dll", EntryPoint = "ThickAlgorithm")]
        private static extern bool ThickAlgorithm([In, Out]Element[] elemList, int elemCount, int totalLayer);

        /// <summary>
        /// fp计算厚度（私有）
        /// </summary>
        /// <param name="totalLayer">总层数</param>
        /// <param name="layerDensitys">每层的密度</param>
        /// <param name="layerThickness">返回每层厚度</param>
        /// <returns>是否计算成功</returns>
        private bool CaculateThick(int totalLayer, float[] layerDensitys, SpecListEntity specList,double ThicknessLimit)
        {
          //  FpWorkCurve.ThicknessLimit = ThicknessLimit;
            float[] layerThickness = new float[totalLayer];
             string strCurrentDir = System.IO.Directory.GetCurrentDirectory();
            System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
           // System.IO.File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\fpFiles\\temp.cal", "temp.cal", true);

            bool bWorkCurveIsInsert = fpwork.IsInsertInThick(ElementList, 1);
            //if (bWorkCurveIsInsert)
            //{
            //    FpCalibrated = false;
            //}
            //if (!FpCalibrated)
            //{
                System.IO.File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\fpFiles\\temp.cal", "temp.cal", true);
            //}

            if (FpWorkCurve.FPSetRootPath("./FpFiles"))
            {
                #region
                //////test;
                //ElementList.Items.Clear();
                //CurveElement ce = CurveElement.New;
                //ce.AnalyteLine = XLine.La;
                //ce.AtomicNumber = 79;
                //ce.Caption = "Au";
                //ce.Formula = "Au";
                //ce.LayerNumber = 1;
                //ce.Intensity = 54.18233;
                //ce.ConditionID = 0;
                //ce.Flag = ElementFlag.Calculated;
                //ce.LayerFlag = LayerFlag.Calculated;
                //ce.ContentUnit = ContentUnit.per;
                //CurveElement ce2 = CurveElement.New;
                //ce2.AnalyteLine = XLine.Ka;
                //ce2.AtomicNumber = 28;
                //ce2.Caption = "Ni";
                //ce2.Formula = "Ni";
                //ce2.LayerNumber = 2;
                //ce2.Intensity = 461.4727;
                //ce2.ConditionID = 0;
                //ce2.Flag = ElementFlag.Calculated;
                //ce2.LayerFlag = LayerFlag.Calculated;
                //ce2.ContentUnit = ContentUnit.per;
                //CurveElement ce3 = CurveElement.New;
                //ce3.AnalyteLine = XLine.Ka;
                //ce3.AtomicNumber = 29;
                //ce3.Caption = "Cu";
                //ce3.Formula = "Cu";
                //ce3.LayerNumber = 3;
                //ce3.Intensity = 906.452;
                //ce3.ConditionID = 0;
                //ce3.Flag = ElementFlag.Calculated;
                //ce3.LayerFlag = LayerFlag.Fixed;
                //ce3.ContentUnit = ContentUnit.per;

                //StandSample ss11 = StandSample.New;
                //ss11.X = 54.18233;
                //ss11.Y = 100;
                //ss11.Z = 0.3683;
                //ss11.TotalLayer = 3;
                //ss11.SampleName = "1#au01";
                //ss11.ElementName = "Au";
                //ss11.Layer = 1;
                //ce.Samples.Add(ss11);

                //StandSample ss12 = StandSample.New;
                //ss12.X = 461.4727;
                //ss12.Y = 100;
                //ss12.Z = 1.9304;
                //ss12.TotalLayer = 3;
                //ss12.SampleName = "1#au01";
                //ss12.ElementName = "Ni";
                //ss12.Layer = 2;
                //ce2.Samples.Add(ss12);

                //StandSample ss13 = StandSample.New;
                //ss13.X = 906.452;
                //ss13.Y = 100;
                //ss13.Z = 0;
                //ss13.TotalLayer = 3;
                //ss13.SampleName = "1#au01";
                //ss13.ElementName = "Cu";
                //ss13.Layer = 3;
                //ce3.Samples.Add(ss13);

                ////StandSample ss21 = StandSample.New;
                ////ss21.X = 10.314211;
                ////ss21.Y = 0.547;
                ////ss21.TotalLayer = 1;
                ////ss21.SampleName = "2#au01";
                ////ss21.ElementName = "V";
                ////ss21.Layer = 1;
                ////ce.Samples.Add(ss21);

                ////StandSample ss22 = StandSample.New;
                ////ss22.X = 672.137969;
                ////ss22.Y = 28.24;
                ////ss22.TotalLayer = 1;
                ////ss22.SampleName = "2#au01";
                ////ss22.ElementName = "Cr";
                ////ss22.Layer = 1;
                ////ce2.Samples.Add(ss22);

                ////StandSample ss23 = StandSample.New;
                ////ss23.X = 93.571104;
                ////ss23.Y = 2.53;
                ////ss23.TotalLayer = 1;
                ////ss23.SampleName = "2#au01";
                ////ss23.ElementName = "Mn";
                ////ss23.Layer = 1;
                ////ce3.Samples.Add(ss23);

                ////StandSample ss31 = StandSample.New;
                ////ss31.X = 0;
                ////ss31.Y = 0;
                ////ss31.TotalLayer = 0;
                ////ss31.SampleName = "3#au01";
                ////ss31.ElementName = "Au";
                ////ss31.Layer = 0;
                //////ce.Samples.Add(ss31);

                ////StandSample ss32 = StandSample.New;
                ////ss32.X = 0;
                ////ss32.Y = 0;
                ////ss32.TotalLayer = 0;
                ////ss32.SampleName = "3#au01";
                ////ss32.ElementName = "Ag";
                ////ss32.Layer = 0;
                //////ce2.Samples.Add(ss32);

                ////StandSample ss33 = StandSample.New;
                ////ss33.X = 5022;
                ////ss33.Y = 99.99;
                ////ss33.TotalLayer = 0;
                ////ss33.SampleName = "3#au01";
                ////ss33.ElementName = "Cu";
                ////ss33.Layer = 0;
                ////ce3.Samples.Add(ss33);

                //ElementList.Items.Add(ce);
                //ElementList.Items.Add(ce2);
                //ElementList.Items.Add(ce3);
                //Condition.DeviceParamList.Clear();
                //DeviceParameter dp = DeviceParameter.New;
                //dp.BeginChann = 50;
                //dp.EndChann = 200;
                //dp.CollimatorIdx = 0;
                //dp.MinRate = 600;
                //dp.MaxRate = 8000;
                //dp.PrecTime = 100;
                //dp.TubCurrent = 200;
                //dp.TubVoltage = 35;
                //dp.VacuumTime = 5;
                //Condition.DeviceParamList.Add(dp);
                ////Condition.Device.Tubes.Angel = 19;
                ////Condition.Device.Tubes.AtomNum = 74;
                ////Condition.Device.Tubes.Incident = 40;
                ////Condition.Device.Tubes.Thickness = 1.9;
                ////Condition.Device.Tubes.Exit = 35;
                ////Condition.Device.Tubes.Material = "SiO2";
                ////Condition.Device.Tubes.MaterialAtomNum = 0;
                ////测试数据
                //Condition.Device.Tubes.AtomNum = 74;
                //Condition.Device.Tubes.Angel = 19;
                //Condition.Device.Tubes.Incident = 90;
                //Condition.Device.Tubes.Exit = 51;
                //Condition.Device.Tubes.MaterialAtomNum = 54;
                //Condition.Device.Tubes.Thickness = 0;
                //Condition.DeviceParamList[0].TubVoltage = 45;
                //totalLayer = 3;
                //layerDensitys = new float[3];
                //layerThickness = new float[totalLayer];
                #endregion


                if (FpWorkCurve.SetSourceData(Condition, Condition.Device.Tubes, CalcAnge(specList.Height)))
                {
                    List<string> listSamples;
                    if (FpWorkCurve.thickMode == ThickMode.Normal  || FpWorkCurve.thickMode == ThickMode.NiP2 || FpWorkCurve.thickMode == ThickMode.NiCuNiFe || FpWorkCurve.thickMode == ThickMode.NiCuNiFe2)
                    {
                        //if (!FpCalibrated)  //打开曲线计算一次
                        {
                            FpCalibrated = fpwork.Calibrate(ElementList, 1, out listSamples);
                        }
                        double[] dThickness = new double[ElementList.Items.Count];
                        if (FpCalibrated && fpwork.CalContent(ElementList, totalLayer, layerDensitys, ref layerThickness, 1, specList))
                        {
                            //元素列表赋值
                            for (int i = 0; i < ElementList.Items.Count; i++)
                            {
                                if (ElementList.Items[i].LayerNumber <= layerThickness.Length)
                                {
                                    ElementList.Items[i].Thickness = layerThickness[ElementList.Items[i].LayerNumber - 1];
                                    dThickness[i] = ElementList.Items[i].Thickness;
                                }
                            }
                            System.IO.Directory.SetCurrentDirectory(strCurrentDir);
                            return true;
                        }
                        else
                        {
                            //元素列表赋值
                            for (int i = 0; i < ElementList.Items.Count; i++)
                            {
                                if (ElementList.Items[i].LayerNumber <= layerThickness.Length)
                                {
                                    ////如果测量超范围 ，显示-1
                                    //if (layerThickness[ElementList.Items[i].LayerNumber - 1] > ThicknessLimit)
                                    //    ElementList.Items[i].Thickness = -1;
                                    //else
                                    ElementList.Items[i].Thickness = layerThickness[ElementList.Items[i].LayerNumber - 1];
                                }
                            }
                        }



                    }
                    else if (FpWorkCurve.thickMode == ThickMode.NiNi)
                    {
                        #region 旧版NICUNI
                        List<CurveElement> elems = new List<CurveElement>();
                        elems = ElementList.Items.OrderBy(x => x.LayerNumber).ToList();

                        Element[] elementarray = new Element[ElementList.Items.Count];
                        for (int i = 0; i < elementarray.Length; i++)
                        {
                            elementarray[i].Formula = elems[i].Formula;
                            elementarray[i].AtomicNumber = elems[i].AtomicNumber;
                            elementarray[i].AnalyteLine = (int)elems[i].AnalyteLine;
                            elementarray[i].ConditionCode = elems[i].ConditionID;
                            elementarray[i].Intensity = elems[i].Intensity;
                            elementarray[i].LayerDensity = elems[i].LayerDensity;
                            elementarray[i].LayerNumber = elems[i].LayerNumber;
                            elementarray[i].ElementMod = (int)elems[i].LayerFlag;

                        }

                        if (ThickAlgorithm(elementarray, elementarray.Length, totalLayer))
                        {

                            for (int i = 0; i < ElementList.Items.Count; i++)
                            {
                                ElementList.Items[i].Content = elementarray[ElementList.Items[i].LayerNumber - 1].Content;

                                ElementList.Items[i].Thickness = elementarray[ElementList.Items[i].LayerNumber - 1].Thickness;


                            }


                            System.IO.Directory.SetCurrentDirectory(strCurrentDir);
                            return true;
                        }
                        #endregion

                    }
                    else if (FpWorkCurve.thickMode == ThickMode.NiCuNi)
                    {
                          #region  重新写NICUNI

                        double[] dThickness = new double[ElementList.Items.Count];
                        Dictionary<string, int> dicElemOrder = new Dictionary<string, int>();
 
                        foreach (var a in ElementList.Items)
                        {
                            dicElemOrder.Add(a.Caption, a.LayerNumber);
                            if (a.LayerNumber > 1)
                                a.LayerNumber = a.LayerNumber - 1;
                        }
                       
                        FpCalibrated = fpwork.Calibrate(ElementList, 1, out listSamples);
                       
                        if (FpCalibrated && fpwork.CalContent(ElementList, totalLayer, layerDensitys, ref layerThickness, 1, specList))
                        {
                            //元素列表赋值
                            for (int i = 0; i < ElementList.Items.Count; i++)
                            {
                                if (ElementList.Items[i].LayerNumber <= layerThickness.Length)
                                {
                                    dThickness[i] = ElementList.Items[i].Content / 100;
                                }
                                dThickness[0] = layerThickness[ElementList.Items[i].LayerNumber - 1];
                            }

                            foreach (var a in ElementList.Items)
                            {
                                a.LayerNumber = dicElemOrder.ToList().Find(w => w.Key == a.Caption).Value;
                            }
                           
                            FpCalibrated = fpwork.Calibrate(ElementList, 1, out listSamples);
                            if (FpCalibrated && fpwork.CalContent(ElementList, totalLayer, layerDensitys, ref layerThickness, 1, specList))
                            {
                                //元素列表赋值
                                for (int i = 0; i < ElementList.Items.Count; i++)
                                {
                                    if(dThickness.Length > 1&& i==1)
                                    {
                                        if (dThickness[1] >= 0.7)
                                            ElementList.Items[i].Thickness = dThickness[1] * layerThickness[ElementList.Items[i].LayerNumber - 1] * 1.05;
                                        else
                                        {
                                            ElementList.Items[i].Thickness = layerThickness[ElementList.Items[i].LayerNumber - 1];

                                        }
                                        
                                    }
                                    else
                                        ElementList.Items[i].Thickness = dThickness[1] * layerThickness[ElementList.Items[i].LayerNumber - 1];
                                   
                                }
                                ElementList.Items[0].Thickness = (dThickness[0] - ElementList.Items[1].Thickness - ElementList.Items[2].Thickness) * dThickness[1];
                            }
                            System.IO.Directory.SetCurrentDirectory(strCurrentDir);
                            return true;
                        }
                        else
                        {
                            //元素列表赋值
                            for (int i = 0; i < ElementList.Items.Count; i++)
                            {
                                if (ElementList.Items[i].LayerNumber <= layerThickness.Length)
                                {
                                    ElementList.Items[i].Thickness = layerThickness[ElementList.Items[i].LayerNumber - 1];
                                }
                            }
                        }

                        #endregion
                    }
                    else
                    {
                     
                        for (int i = 0; i < ElementList.Items.Count; i++)
                        {
                            if (ElementList.Items[i].LayerNumber > 1)
                                ElementList.Items[i].LayerNumber = ElementList.Items[i].LayerNumber - 1;
                            ElementList.Items[i].Thickness = 0;
                        }
                        System.IO.Directory.SetCurrentDirectory(strCurrentDir);
                        return false;
                    }
                }
            }
            System.IO.Directory.SetCurrentDirectory(strCurrentDir);
            return false;
        }

        /// <summary>
        /// fp设置计算用的测量条件
        /// </summary>
        /// <param name="condition">测量条件</param>
        /// <param name="raytub">光管参数</param>
        /// <param name="primaryFilterAtomicNumber">光管材料元素的ID</param>
        /// <returns>设置是否成功</returns>
        public bool SetSourceData()
        {
            return FpWorkCurve.SetSourceData(this.Condition, this.Condition.Device.Tubes, Condition.Device.Tubes.Exit);
        }

        /// <summary>
        /// EC强度度校正后的强度
        /// </summary>
        /// <param name="strElementName">元素名</param>
        /// <param name="sampleIntensitys">校正后强度的数组</param>
        /// <returns>返回是否成功</returns>
        public bool GetCalibrateIntensity(string strElementName, out double[] sampleIntensitys)
        {
            switch (FuncType)
            {
                //case FuncType.Rohs:
                case FuncType.XRF:
                    double[] intensityCoefs = new double[0];
                    int i = ElementList.GetElementIndex(strElementName);
                    if (!ecwork.GetIntensityInfectCoefs(ElementList, out intensityCoefs, i))
                    {
                        sampleIntensitys = new double[0];
                        return false;
                    }
                    foreach (var reference in ElementList.Items[i].ElementRefs)
                    {
                        reference.RefConf = 0;
                    }
                    sampleIntensitys = new double[ElementList.Items[i].Samples.Count];
                    if (intensityCoefs.Length < ElementList.Items[i].InfluenceElements.Length + 1)
                    {
                        sampleIntensitys = new double[0];
                        return false;
                    }
                    int IndexMain = ElementList.Items.ToList().FindIndex(f => f.Flag == ElementFlag.Internal);
                    for (int j = 0; j < ElementList.Items[i].Samples.Count; j++)
                    {
                        string strSampleName = ElementList.Items[i].Samples[j].SampleName;
                        int coefIndex = 2;
                        for (int k = 0; k < ElementList.Items[i].InfluenceElements.Length && coefIndex < intensityCoefs.Length; k++)
                        {
                            int indexIn = ElementList.GetElementIndex(ElementList.Items[i].InfluenceElements[k]);
                            if (IndexMain!=-1)
                            {
                                //2010-10-09 cyq
                                if (indexIn == IndexMain)
                                {
                                    continue;
                                }
                                StandSample stand = ElementList.Items[IndexMain].Samples.ToList().Find(c => c.SampleName == strSampleName);
                                for (int l = 0; l < ElementList.Items[indexIn].Samples.Count; l++)
                                {
                                    if (ElementList.Items[indexIn].Samples[l].SampleName.CompareTo(strSampleName) == 0)
                                    {
                                        sampleIntensitys[j] += intensityCoefs[coefIndex] * double.Parse(ElementList.Items[indexIn].Samples[l].X)/double.Parse(stand.X);
                                        ElementRef er = ElementList.Items[i].ElementRefs.ToList().Find(r => r.Name == ElementList.Items[indexIn].Caption);
                                        er.RefConf = intensityCoefs[coefIndex];
                                        //er.Save();
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int l = 0; l < ElementList.Items[indexIn].Samples.Count; l++)
                                {
                                    if (ElementList.Items[indexIn].Samples[l].SampleName.CompareTo(strSampleName) == 0)
                                    {
                                        sampleIntensitys[j] += intensityCoefs[coefIndex] * double.Parse(ElementList.Items[indexIn].Samples[l].X);
                                        ElementRef er = ElementList.Items[i].ElementRefs.ToList().Find(r => r.Name == ElementList.Items[indexIn].Caption);
                                        er.RefConf = intensityCoefs[coefIndex];
                                        //er.Save();
                                        break;
                                    }
                                }
                            }
                            
                            coefIndex++;
                        }
                        if (intensityCoefs[1]==0.0)
                        {
                            sampleIntensitys[j] = 0.0;
                        }
                        else if (IndexMain!=-1)
                        {
                            StandSample stand = ElementList.Items[IndexMain].Samples.ToList().Find(c => c.SampleName == strSampleName);
                            sampleIntensitys[j] = (sampleIntensitys[j] / intensityCoefs[1] + 1) * double.Parse(ElementList.Items[i].Samples[j].X) / double.Parse(stand.X);
                        }
                        else sampleIntensitys[j] = (sampleIntensitys[j] / intensityCoefs[1] + 1) * double.Parse(ElementList.Items[i].Samples[j].X);
                    }
                    return true;
                case FuncType.Thick:
                    //if (!thwork.GetThickInfectedCoefs(ElementList, IsAbsorb, ElementList.ThCalculationWay, ElementList.DBlLimt))
                    //{
                    //    sampleIntensitys = new double[0];
                    //    return false;
                    //}
                    int ii = ElementList.GetElementIndex(strElementName);
                    sampleIntensitys = new double[ElementList.Items[ii].Samples.Count];
                    if (sampleIntensitys.Length <= 0)
                    {
                        return true;
                    }
                    for (int j = 0; j < ElementList.Items[ii].Samples.Count; j++)
                    {
                        
                        string strSampleName = ElementList.Items[ii].Samples[j].SampleName;
                        for (int k = 0; k < ElementList.Items.Count;k++ )
                        {
                            StandSample sp= ElementList.Items[k].Samples.ToList().Find(wde=>wde.SampleName.Equals(strSampleName));
                            ElementList.Items[k].Intensity = sp != null ? double.Parse(sp.X) : 0;
                        }
                        if (!thwork.GetThickInfectedCoefs(ElementList, IsAbsorb, ElementList.ThCalculationWay, ElementList.DBlLimt))
                        {
                            sampleIntensitys = new double[0];
                            return false;
                        }
                        double r = double.Parse(ElementList.Items[ii].Samples[j].X);
                        int indexIn = -1;
                        for (int k = 1; k < ElementList.Items[ii].Samples[j].Layer; k++)
                        {
                            for (int l = 0; l < ElementList.Items.Count; l++)
                            {
                                if (ElementList.Items[l].LayerNumber == k)
                                {
                                    indexIn = l;
                                    break;
                                }
                            }
                            for (int l = 1; l < ElementList.Items[indexIn].Samples.Count; l++)
                            {
                                if (ElementList.Items[indexIn].Samples[l].SampleName.CompareTo(strSampleName) == 0)
                                {
                                    r /= Math.Exp(ElementList.Items[ii].InfluenceCoefficientLists[k] * float.Parse(ElementList.Items[indexIn].Samples[l].Z));
                                    break;
                                }
                            }
                        }
                        //if (IsAbsorb && ElementList.Items[ii].Samples[j].TotalLayer - 1 == ElementList.Items[ii].LayerNumber)
                        if (IsAbsorb && ElementList.Items.Count-1 == ElementList.Items[ii].LayerNumber)
                        {
                            sampleIntensitys[j] = -Math.Log(r);//吸收法
                        }
                        else sampleIntensitys[j] = -Math.Log(1 - r);//发射法
                    }
                    return true;
                default:
                    sampleIntensitys = new double[0];
                    return false;
            }

        }

        /// <summary>
        /// EC浓度校正后的强度
        /// </summary>
        /// <param name="strElementName">元素名</param>
        /// <param name="sampleContent">浓度校正后强度的数组</param>
        /// <returns>返回是否成功</returns>
        public bool GetCalibrateContent(string strElementName, out double[] sampleIntensity)
        {
            double[] intensityCoefs = new double[0];
            int i = ElementList.GetElementIndex(strElementName);
            if (!ecwork.GetContentInfectCoefs(ElementList, out intensityCoefs, i))
            {
                sampleIntensity = new double[0];
                return false;
            }
            foreach (var reference in ElementList.Items[i].ElementRefs)
            {
                reference.RefConf = 0;
            }
            

            sampleIntensity = new double[ElementList.Items[i].Samples.Count];
            if (intensityCoefs.Length < ElementList.Items[i].InfluenceElements.Length + 1)
            //if (intensityCoefs.Length < ElementList.Items[i].InfluenceElements.Length + 2)//原来语句
            {
                sampleIntensity = new double[0];
                return false;
            }
            int IndexMain = ElementList.Items.ToList().FindIndex(f => f.Flag == ElementFlag.Internal);
            for (int j = 0; j < ElementList.Items[i].Samples.Count; j++)
            {
                string strSampleName = ElementList.Items[i].Samples[j].SampleName;
                int coefIndex = 2;
                for (int k = 0; k < ElementList.Items[i].InfluenceElements.Length && coefIndex < intensityCoefs.Length; k++)
                {
                    int indexIn = ElementList.GetElementIndex(ElementList.Items[i].InfluenceElements[k]);
                    //2010-10-09 cyq
                    if (IndexMain!=-1)
                    {
                        if (indexIn == IndexMain)
                        {
                            continue;
                        }
                        StandSample stand = ElementList.Items[IndexMain].Samples.ToList().Find(c => c.SampleName == strSampleName);

                        for (int l = 0; l < ElementList.Items[indexIn].Samples.Count; l++)
                        {
                            if (ElementList.Items[indexIn].Samples[l].SampleName.CompareTo(strSampleName) == 0)
                            {
                                sampleIntensity[j] += intensityCoefs[coefIndex] * double.Parse(ElementList.Items[indexIn].Samples[l].Y)/double.Parse(stand.Y);
                                ElementRef er = ElementList.Items[i].ElementRefs.ToList().Find(r => r.Name == ElementList.Items[indexIn].Caption);
                                er.RefConf = intensityCoefs[coefIndex];
                                //er.Save();
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int l = 0; l < ElementList.Items[indexIn].Samples.Count; l++)
                        {
                            if (ElementList.Items[indexIn].Samples[l].SampleName.CompareTo(strSampleName) == 0)
                            {
                                sampleIntensity[j] += intensityCoefs[coefIndex] * double.Parse(ElementList.Items[indexIn].Samples[l].Y);
                                ElementRef er = ElementList.Items[i].ElementRefs.ToList().Find(r => r.Name == ElementList.Items[indexIn].Caption);
                                er.RefConf = intensityCoefs[coefIndex];
                                //er.Save();
                                break;
                            }
                        }
                    }
                    coefIndex++;
                }
                if (intensityCoefs[1] == 0.0)
                {
                    sampleIntensity[j] = 0.0;
                }
                else if (IndexMain != -1)
                {
                    StandSample stand = ElementList.Items[IndexMain].Samples.ToList().Find(c => c.SampleName == strSampleName);
                    sampleIntensity[j] = (sampleIntensity[j] / intensityCoefs[1] + 1) * double.Parse(ElementList.Items[i].Samples[j].X)/double.Parse(stand.X);
                }
                else sampleIntensity[j] = (sampleIntensity[j] / intensityCoefs[1] + 1) * double.Parse(ElementList.Items[i].Samples[j].X) ;
            }
            return true;
        }

        /// <summary>
        /// fp取得实测强度和理论强度
        /// </summary>
        /// <param name="fpCaculationWay">fp计算方法</param>
        /// <param name="strCurvePath">曲线目录</param>
        /// <returns></returns>
        public bool GetCalibrateItensity(out double[,] elemI, out double[,] elemC, out List<string> samplesName,out List<string> elementsName)
        {
            foreach (var element in ElementList.Items)
            {
                if (element.ThicknessUnit == ThicknessUnit.ur)
                {
                    foreach (var sample in element.Samples)
                    {
                        sample.Z = (double.Parse(sample.Z) * 0.0254).ToString();
                    }
                }
            }

            string strCurrentDir = System.IO.Directory.GetCurrentDirectory();
            System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            System.IO.File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\fpFiles\\temp.cal", "temp.cal", true);
            if (FpWorkCurve.FPSetRootPath("./FpFiles"))
            {
                if (FpWorkCurve.SetSourceData(Condition, Condition.Device.Tubes, Condition.Device.Tubes.Exit))
                {
                    
                    if (FuncType==FuncType.Thick&&CalcType==CalcType.FP&&fpwork.Calibrate(ElementList,1,out samplesName))
                    {
                        elemI = new double[samplesName.Count, ElementList.Items.Count];
                        elemC = new double[samplesName.Count, ElementList.Items.Count];
                        elementsName=new List<string>();
                        if (fpwork.ReadPLOTFile(ElementList, AppDomain.CurrentDomain.BaseDirectory + "fpFiles\\PLOT.DAT",ref elemI,ref elemC,ref elementsName))
                        {
                            System.IO.Directory.SetCurrentDirectory(strCurrentDir);
                            translate();
                            return true;
                        }
                    }
                    else if (FuncType == FuncType.XRF && CalcType == CalcType.FP && fpwork.Calibrate(ElementList, 0,out samplesName))
                    {
                        elemI = new double[samplesName.Count, ElementList.Items.Count];
                        elemC = new double[samplesName.Count, ElementList.Items.Count];
                        elementsName=new List<string>();
                        if (fpwork.ReadPLOTFile(ElementList, AppDomain.CurrentDomain.BaseDirectory + "fpFiles\\PLOT.DAT", ref elemI, ref elemC,ref elementsName))
                        {
                            System.IO.Directory.SetCurrentDirectory(strCurrentDir);
                            translate();
                            return true;
                        }
                    }
                }
            }
            System.IO.Directory.SetCurrentDirectory(strCurrentDir);
            samplesName = new List<string>();
            elementsName = new List<string>();
            elemI = new double[0,0];
            elemC = new double[0, 0];
            translate();
            return false;
        }

        private void translate()
        {
            foreach (var element in ElementList.Items)
            {
                if (element.ThicknessUnit == ThicknessUnit.ur)
                {
                    foreach (var sample in element.Samples)
                    {
                        sample.Z = (double.Parse(sample.Z) / 0.0254).ToString();
                    }
                }
            }
        }

        /// <summary>
        /// fp 最后一次优化（样品含量靠近）
        /// </summary>
        /// <param name="elementList"></param>
        /// <returns></returns>
        public bool GetGadeContent(ElementList elementList)
        {
            if (!Helper.IGetGradeContent)
            {
                return true;
            }
            int elementCount=elementList.Items.Count;
            SElement[] elements = new SElement[elementCount];
            //List<MatchSample> lst = new List<MatchSample>();
           
            //lst = MatchSample.FindAll();
           
            //if (lst.Count == 0)
            //{
            //    return false;
            //}
            //int samplesCount=lst.Count;
            //double[,] ScalarContent=new double[samplesCount,elementCount];
            for (int i = 0; i < elementCount;i++ )
            {
                elements[i].Content = elementList.Items[i].Content;
                elements[i].Error = elementList.Items[i].Error;
                elements[i].Caption = elementList.Items[i].Caption;
                elements[i].Weight = 1;
            }
            
            //Console.Write(DateTime.Now.ToString());
            //for (int i = 0; i < samplesCount;i++ )
            //{
            //    //List<ElementMember> emlist = lst[i].Elements.ToList();
            //   // ElementMember[] emlist = lst[i].Elements.ToArray();
            //    Dictionary<string, ElementMember> emlist = lst[i].Elements.ToDictionary(wde => wde.Element);

            //    for (int j = 0; j < elementCount;j++ )
            //    {
            //        //for (int k = 0; k < emlist.Count; k++)
            //        //{
            //        //    if (emlist[k].Element == elements[j].Name)
            //        //    {
            //        //        ScalarContent[i, j] = emlist[k].Content;
            //        //        break;
            //        //    }
            //        //}

            //       // ElementMember em = lst[i].Elements.ToList().Find(w => w.Element == elements[j].Name);
            //       // ElementMember em = emlist.Find(w => w.Element==elements[j].Name);
            //        //ElementMember em=null;
            //        //emlist.TryGetValue(elements[j].Name, out em);
            //        ScalarContent[i, j] = em != null ? em.Content : 0f;
            //    }
                
            //}
           
            Console.Write(DateTime.Now.ToString());
            if (elementCount>0&&ArithDllInterface.GetGradeContent(elements,elementCount, 0.5))
            {
                Console.Write(DateTime.Now.ToString());
                for (int i = 0; i < elementCount; i++)
                {
                    elementList.Items[i].Content = elements[i].Content;    
                }
                return true;
            }
            else return false;
        }


        public bool GetFpStandSamplesContent(ElementList elementList, int elemenIndex, out List<StandSample> Samples, SpecListEntity specList)
        {
            string strCurrentDir = System.IO.Directory.GetCurrentDirectory();
            System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            System.IO.File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\fpFiles\\temp.cal", "temp.cal", true);
            //bool b = false;
            if (FpWorkCurve.FPSetRootPath("./FpFiles"))
            {
                if (FpWorkCurve.SetSourceData(Condition, Condition.Device.Tubes, CalcAnge(specList.Height)))
                {
                    List<string> samplesName;
                    if (!fpwork.Calibrate(elementList, 0, out samplesName))
                    {
                        Samples = new List<StandSample>();
                        return false;
                    }
                    //List<StandSample> lsTemp = elementList.Items[elemenIndex].Samples.OrderByDescending(data => data.X).ToList<StandSample>();
                    List<StandSample> lsTemp = elementList.Items[elemenIndex].Samples.ToList();
                    int sampleCount = lsTemp.Count;
                    int elementCount = elementList.Items.Count;
                    for (int i = 0; i < sampleCount; i++)
                    {
                        for (int j = 0; j < elementCount; j++)
                        {
                            StandSample standi = elementList.Items[j].Samples.First(w => w.SampleName == lsTemp[i].SampleName);
                            elementList.Items[j].Intensity = double.Parse(standi.X);
                        }
                        int totalLayer = 1;
                        float[] layerDensitys = new float[1];
                        layerDensitys[0] = 0.0f;
                        float[] layerThickness = new float[1];
                        if (!fpwork.CalContent(elementList, totalLayer, layerDensitys, ref layerThickness, 0, specList))
                        {
                            Samples = new List<StandSample>();
                            return false;
                        }
                        lsTemp[i].TheoryX = elementList.Items[elemenIndex].Content;
                    }
                    System.IO.Directory.SetCurrentDirectory(strCurrentDir);
                    Samples = lsTemp;
                    return true;
                }
            }
            System.IO.Directory.SetCurrentDirectory(strCurrentDir);
            Samples = new List<StandSample>();
            return false;
        }

        //Ec的计算值和真实值的计算
        public bool GetECStandSamplesContent(ElementList elementList, int elemenIndex, out List<StandSample> Samples, SpecListEntity specList)
        {
            List<StandSample> lsTemp = elementList.Items[elemenIndex].Samples.ToList();
            int sampleCount = lsTemp.Count;
            int elementCount = elementList.Items.Count;
            for (int i = 0; i < sampleCount; i++)
            {
                for (int j = 0; j < elementCount; j++)
                {
                    StandSample standi = elementList.Items[j].Samples.First(w => w.SampleName == lsTemp[i].SampleName);
                    elementList.Items[j].Intensity = double.Parse(standi.X);
                }
                if (ecwork != null && !ecwork.GetECContent(elementList, elementList.RhIsLayer,elementList.RhLayerFactor, specList))
                {
                    Samples = new List<StandSample>();
                    return false;
                }
                lsTemp[i].TheoryX = elementList.Items[elemenIndex].Content;
            }
            Samples = lsTemp;
            return true;
        }

        /// <summary>
        /// 计算样品的不确定度
        /// </summary>
        /// <param name="dblTestResults">样品的计算结果</param>
        /// <param name="strElemName">计算不确定度的元素</param>
        /// <param name="strSimilarStand">类似标样</param>
        /// <param name="Ud">输入的未知样的不一定度</param>
        /// <param name="Uc">计算的校正曲线的不确定度</param>
        /// <param name="Ub">输入的标准物质不确定度</param>
        /// <param name="Ua">测量重复性与样品均匀性不稳定度</param>
        /// <param name="Ux">合成不确定度</param>
        /// <param name="U">扩展不确定度</param>
        /// <returns>是否计算成功</returns>
        public bool GetUncertainty(List<double> dblTestResults, string strElemName, double Ub, double Ud, ref double Uc, ref double Ua, ref double Ux, ref double U,ref double TestAvg)
        {
            CurveElement ce = this.ElementList.Items.ToList().Find(w => w.Caption.ToLower().Equals(strElemName.ToLower()));
            if (dblTestResults.Count < 3||ce==null) return false;
            List<StandSample> Calibarates;
            if (this.CalcType == CalcType.EC)
                this.GetECStandSamplesContent(this.ElementList, this.ElementList.Items.IndexOf(ce), out Calibarates, null);
            else this.GetFpStandSamplesContent(this.ElementList, this.ElementList.Items.IndexOf(ce), out Calibarates, null);
            ////测试
            //Calibarates[0].Y = "99.994";
            //Calibarates[0].TheoryX = 99.96;
            //Calibarates[1].Y = "99.60";
            //Calibarates[1].TheoryX = 99.51;
            //Calibarates[2].Y = "98.98";
            //Calibarates[2].TheoryX = 98.85;
            //Calibarates[3].Y = "97.99";
            //Calibarates[3].TheoryX = 97.98;
            //Calibarates[4].Y = "96.00";
            //Calibarates[4].TheoryX = 96.11;
            //Calibarates[5].Y = "91.68";
            //Calibarates[5].TheoryX = 91.78;
            ////测试

            double m = 1.0f, b = 0f;//斜率和截距
            double xx = 0;
            double xy = 0;
            double x = 0;
            double y = 0;
            double AvgStard = 0;
            int count = 0;

            for (int i = 0; i < Calibarates.Count; i++)
            {
                if (Calibarates[i].Active)
                {
                    x += double.Parse(Calibarates[i].Y);
                    y += Calibarates[i].TheoryX;
                    xx += double.Parse(Calibarates[i].Y) * double.Parse(Calibarates[i].Y);
                    xy += double.Parse(Calibarates[i].Y) * Calibarates[i].TheoryX;
                    count++;
                }
            }
            AvgStard = x / count;
            m = (x * y - count * xy) / (x * x - count * xx);
            b = (y - m * x) / count;

            //求Ua
            double Avg = 0;
            double AvgCal = 0;
            double SD=0;
            for(int i=0;i<dblTestResults.Count;i++)
            {
                Avg+=dblTestResults[i];
                AvgCal+=(dblTestResults[i]-b)/m;
            }
            Avg/=dblTestResults.Count;
            AvgCal/=dblTestResults.Count;
            for(int i=0;i<dblTestResults.Count;i++)
            {
                SD+=((dblTestResults[i]-b)/m-AvgCal)*((dblTestResults[i]-b)/m-AvgCal);
            }
            Ua = Math.Sqrt(SD / (dblTestResults.Count-1))* 100 / (AvgCal * Math.Sqrt(dblTestResults.Count));

            //求Ub
            //Ub = double.Parse(standsp.Uncertainty);
            
            //求Uc
            double rsd = 0;
            count = 0;
            double sdSum = 0;
            for (int i = 0; i < Calibarates.Count; i++)
            {
                if (Calibarates[i].Active)
                {
                    double dblTempC=double.Parse(Calibarates[i].Y);
                    double dblCaliC=Calibarates[i].TheoryX ;
                    rsd += (dblCaliC - dblTempC * m - b) * (dblCaliC - dblTempC * m - b);
                    sdSum += (dblTempC - AvgStard) * (dblTempC - AvgStard);
                    count++;
                }
            }
            rsd = Math.Sqrt(rsd / (count - 2));
            Uc = rsd / m * Math.Sqrt(1.0 / count + 1.0 / dblTestResults.Count + (Avg - AvgStard) * (Avg - AvgStard) / sdSum);

            //Ux
            Ux = Math.Sqrt(Ua * Ua + Ub * Ub + Uc * Uc + Ud * Ud);
            U = 2 * Ux;
            TestAvg = Avg;
            return true;
        }

        /// <summary>
        /// 计算样品的不确定度
        /// </summary>
        /// <param name="dblTestResults">样品的计算结果</param>
        /// <param name="strElemName">计算不确定度的元素</param>
        /// <param name="strSimilarStand">类似标样</param>
        /// <param name="Ud">输入的未知样的不一定度</param>
        /// <param name="Uc">计算的校正曲线的不确定度</param>
        /// <param name="Ub">输入的标准物质不确定度</param>
        /// <param name="Ua">测量重复性与样品均匀性不稳定度</param>
        /// <param name="Ux">合成不确定度</param>
        /// <param name="U">扩展不确定度</param>
        /// <returns>是否计算成功</returns>
        public bool GetUncertainty(List<double> dblTestResults, string strElemName, double Ub, double Ud, ref double Uc, ref double Ua, ref double Ux, ref double U, ref double TestAvg, ref List<StandSample> StandSampleList)
        {
            CurveElement ce = this.ElementList.Items.ToList().Find(w => w.Caption.ToLower().Equals(strElemName.ToLower()));
            if (dblTestResults.Count < 3 || ce == null) return false;
            List<StandSample> Calibarates;
            if (this.CalcType == CalcType.EC)
                this.GetECStandSamplesContent(this.ElementList, this.ElementList.Items.IndexOf(ce), out Calibarates, null);
            else this.GetFpStandSamplesContent(this.ElementList, this.ElementList.Items.IndexOf(ce), out Calibarates, null);
            ////测试
            //Calibarates[0].Y = "99.994";
            //Calibarates[0].TheoryX = 99.96;
            //Calibarates[1].Y = "99.60";
            //Calibarates[1].TheoryX = 99.51;
            //Calibarates[2].Y = "98.98";
            //Calibarates[2].TheoryX = 98.85;
            //Calibarates[3].Y = "97.99";
            //Calibarates[3].TheoryX = 97.98;
            //Calibarates[4].Y = "96.00";
            //Calibarates[4].TheoryX = 96.11;
            //Calibarates[5].Y = "91.68";
            //Calibarates[5].TheoryX = 91.78;
            ////测试
            StandSampleList = Calibarates;

            double m = 1.0f, b = 0f;//斜率和截距
            double xx = 0;
            double xy = 0;
            double x = 0;
            double y = 0;
            double AvgStard = 0;
            int count = 0;

            for (int i = 0; i < Calibarates.Count; i++)
            {
                if (Calibarates[i].Active)
                {
                    x += double.Parse(Calibarates[i].Y);
                    y += Calibarates[i].TheoryX;
                    xx += double.Parse(Calibarates[i].Y) * double.Parse(Calibarates[i].Y);
                    xy += double.Parse(Calibarates[i].Y) * Calibarates[i].TheoryX;
                    count++;
                }
            }
            AvgStard = x / count;
            m = (x * y - count * xy) / (x * x - count * xx);
            b = (y - m * x) / count;

            //求Ua
            double Avg = 0;
            double AvgCal = 0;
            double SD = 0;
            for (int i = 0; i < dblTestResults.Count; i++)
            {
                Avg += dblTestResults[i];
                AvgCal += (dblTestResults[i] - b) / m;
            }
            Avg /= dblTestResults.Count;
            AvgCal /= dblTestResults.Count;
            for (int i = 0; i < dblTestResults.Count; i++)
            {
                SD += ((dblTestResults[i] - b) / m - AvgCal) * ((dblTestResults[i] - b) / m - AvgCal);
            }
            Ua = Math.Sqrt(SD / (dblTestResults.Count - 1)) * 100 / (AvgCal * Math.Sqrt(dblTestResults.Count));

            //求Ub
            //Ub = double.Parse(standsp.Uncertainty);

            //求Uc
            double rsd = 0;
            count = 0;
            double sdSum = 0;
            for (int i = 0; i < Calibarates.Count; i++)
            {
                if (Calibarates[i].Active)
                {
                    double dblTempC = double.Parse(Calibarates[i].Y);
                    double dblCaliC = Calibarates[i].TheoryX;
                    rsd += (dblCaliC - dblTempC * m - b) * (dblCaliC - dblTempC * m - b);
                    sdSum += (dblTempC - AvgStard) * (dblTempC - AvgStard);
                    count++;
                }
            }
            rsd = Math.Sqrt(rsd / (count - 2));
            Uc = rsd / m * Math.Sqrt(1.0 / count + 1.0 / dblTestResults.Count + (Avg - AvgStard) * (Avg - AvgStard) / sdSum);

            //Ux
            Ux = Math.Sqrt(Ua * Ua + Ub * Ub + Uc * Uc + Ud * Ud);
            U = 2 * Ux;
            TestAvg = Avg;
            return true;
        }
    }

    public class TempElement
    {
        public string Caption;
        public double Intensity;
    }

}
