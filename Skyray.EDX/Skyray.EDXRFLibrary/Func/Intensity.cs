using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.EDXRFLibrary
{
    class Intensity
    {
        /// <summary>
        /// 强度计算
        /// </summary>
        /// <param name="elementlist">元素列表</param>
        /// <param name="spec">谱数据数组</param>
        /// <param name="demarcateIntercept">能量刻度的截距</param>
        /// <param name="demarcateSlope">能量刻度的斜率</param>
        /// <param name="detector">探测器</param>
        /// <param name="FpDeltaCoef">FixedGaussMode用到的参数</param>
        /// <returns>计算是否成功</returns>
        public static bool GetIntensity(ElementList elementlist, SpecEntity[] spec, double demarcateIntercept, double demarcateSlope, Detector detector, double FxDeltaCoef)
        {
            int ecElemntCount = 0;
            List<int> ecElementIndexList = new List<int>();
            for (int i = 0; i < elementlist.Items.Count;i++ )
            {
                if (elementlist.Items[i].IntensityWay!=IntensityWay.FPGauss)
                {
                    ecElemntCount++;
                    //ecElementIndexList.Add(i);
                }
                ecElementIndexList.Add(i);
            }
            if (ecElemntCount!=elementlist.Items.Count) //2011-07-08 调整顺序
            {
                FpWorkCurve.FPGauss(elementlist, spec, demarcateIntercept, demarcateSlope);
            }
            int length = (int)elementlist.WorkCurve.Condition.Device.SpecLength; //add by cayunhe
            Element[] elementarray = new Element[ecElementIndexList.Count];
            int maxConditionID = 0;
            for (int i = 0; i < elementarray.Length; i++)
            {
                elementarray[i].Caption = elementlist.Items[ecElementIndexList[i]].Caption;
                elementarray[i].Formula = elementlist.Items[ecElementIndexList[i]].Formula;
                //2011-08-30修改元素对应的谱序号。
                //bool specTrue = false;
                //for (int j = 0; j < spec.Length; j++)
                //{
                //    if (elementlist.Items[ecElementIndexList[i]].DevParamId == spec[j].DeviceParameter.Id)
                //    {
                //        specTrue = true;
                //        elementarray[i].ConditionCode = j;
                //        break;
                //    }
                //}
                //if (!specTrue)
                //{
                //    return false;
                //}
                elementarray[i].ConditionCode = elementlist.Items[ecElementIndexList[i]].ConditionID;
                if (elementarray[i].ConditionCode >maxConditionID)
                {
                    maxConditionID = elementarray[i].ConditionCode;
                }
                elementarray[i].SpectrumData = new double[ArithDllInterface.MAXSPECTRUMDATALENGTH];
                double[] specE = Helper.ToDoubles(elementlist.Items[ecElementIndexList[i]].SSpectrumData);
                elementarray[i].SpectrumDataLen = specE.Length;
                for (int k = 0; k < elementarray[i].SpectrumDataLen; k++)
                {
                    if (k < specE.Length)
                    {
                        elementarray[i].SpectrumData[k] = specE[k];
                    }
                    else elementarray[i].SpectrumData[k] = 0;
                }
                elementarray[i].AtomicNumber = elementlist.Items[ecElementIndexList[i]].AtomicNumber;
                elementarray[i].AnalyteLine = (int)elementlist.Items[ecElementIndexList[i]].AnalyteLine;
                elementarray[i].BaseHigh = elementlist.Items[ecElementIndexList[i]].BaseHigh;
                elementarray[i].BaseLow = elementlist.Items[ecElementIndexList[i]].BaseLow;
                elementarray[i].PeakLow = elementlist.Items[ecElementIndexList[i]].PeakLow;
                elementarray[i].PeakHigh = elementlist.Items[ecElementIndexList[i]].PeakHigh;
                //isPeak = elementlist.Items[ecElementIndexList[i]].PeakDivBase;
                elementarray[i].IsPeakDivBase = elementlist.Items[ecElementIndexList[i]].PeakDivBase;
                elementarray[i].BaseIntensityMode = (int)elementlist.Items[ecElementIndexList[i]].BaseIntensityWay;
                elementarray[i].IntentsityMod = (int)elementlist.Items[ecElementIndexList[i]].IntensityWay;
                elementarray[i].InflueceCoefficientNumber = 0;
                elementarray[i].InfluenceElements = elementlist.Items[ecElementIndexList[i]].SInfluenceElements;
                elementarray[i].InfluenceCoefficientList = new double[ArithDllInterface.MAXINFLUENCECOEFFICIENT];
                for (int j = 0; j < elementarray[i].InfluenceCoefficientList.Length && j < elementlist.Items[ecElementIndexList[i]].InfluenceCoefficientLists.Length; j++)
                {
                    elementarray[i].InfluenceCoefficientList[j] = elementlist.Items[ecElementIndexList[i]].InfluenceCoefficientLists[j];
                    elementarray[i].InflueceCoefficientNumber++;
                }
                elementarray[i].CalculationMod = (int)elementlist.Items[ecElementIndexList[i]].CalculationWay;
                elementarray[i].K0 = elementlist.Items[ecElementIndexList[i]].K0;
                elementarray[i].K1 = elementlist.Items[ecElementIndexList[i]].K1;
                elementarray[i].IsIntensityCompare = elementlist.Items[ecElementIndexList[i]].IntensityComparison;
                elementarray[i].ComparisionCoefficient = elementlist.Items[ecElementIndexList[i]].ComparisonCoefficient;
                elementarray[i].BPeakHigh = elementlist.Items[ecElementIndexList[i]].BPeakHigh;
                elementarray[i].BPeakLow = elementlist.Items[ecElementIndexList[i]].BPeakLow;
                elementarray[i].Intensity=elementlist.Items[ecElementIndexList[i]].IntensityWay==IntensityWay.FPGauss? elementlist.Items[ecElementIndexList[i]].Intensity:0;
                elementarray[i].Error = elementlist.Items[ecElementIndexList[i]].IntensityWay == IntensityWay.FPGauss ? elementlist.Items[ecElementIndexList[i]].Error : 0;

                


                //add by chuyaqin 2011-07-11
                string strRefElements=string.Empty;
                string strRefLR=string.Empty;
                for (int j=0;j<elementlist.Items[ecElementIndexList[i]].References.Count;j++)
                {
                      strRefElements+=elementlist.Items[ecElementIndexList[i]].References[j].ReferenceElementName+",";
                      strRefLR+=elementlist.Items[ecElementIndexList[i]].References[j].ReferenceLeftBorder+","+elementlist.Items[ecElementIndexList[i]].References[j].ReferenceRightBorder+";";
                }
                elementarray[i].ReferenceElements =strRefElements;
                elementarray[i].ReferenceElementsLR = strRefLR;
            }
            if (maxConditionID>=spec.Length)
            {
                return false;
            }
            int errCode = ArithDllInterface.CAPIERR_NOERROR;
            ArithDllInterface.SetElementListConfigure(elementarray, elementarray.Length, elementlist.RhIsLayer, elementlist.RhLayerFactor, elementlist.RhIsMainElementInfluence);
            ArithDllInterface.SetIntFixedGaussParam( ref errCode,FxDeltaCoef);

            #region  slow
            int[,] datas = new int[spec.Length, length];
            //int[] usedTimes = new int[spec.Length];
            double[] usedTimes = new double[spec.Length];
            for (int i = 0; i < spec.Length; i++)
            {
                var ints = spec[i].SpecDatac;
                for (int j = 0; j < length; j++)
                {
                    datas[i, j] = ints[j];
                }
                //usedTimes[i] = Convert.ToInt32(spec[i].UsedTime);
                usedTimes[i] = spec[i].UsedTime;
            }
            #endregion
            errCode = ArithDllInterface.CAPIERR_NOERROR;
            if (!ArithDllInterface.CaculateIntensity(ref errCode, elementarray, datas, spec.Length, length, usedTimes))
            {
                ArithDllInterface.MessageCatch(errCode);
                return false;
            }
            for (int i = 0; i < elementarray.Length; i++)
            {
               
                if (Double.IsNaN(elementarray[i].Intensity))
                {
                    elementlist.Items[ecElementIndexList[i]].Intensity = 0;
                    elementlist.Items[ecElementIndexList[i]].Error = 0;
                }
                else
                {
                    elementlist.Items[ecElementIndexList[i]].Intensity = elementarray[i].Intensity;
                    elementlist.Items[ecElementIndexList[i]].Error = elementarray[i].Error;

                    if (FpWorkCurve.thickMode == ThickMode.Plating)
                    {
                        elementlist.Items[ecElementIndexList[i]].Intensity = elementarray.First(x=>x.LayerNumber==0).Intensity;
                        elementlist.Items[ecElementIndexList[i]].Error = elementarray.First(x => x.LayerNumber == 0).Error;
                    }

                }
            }
            return true;
        }

     
        public static double FullArea(int left, int right, int[] Data, double UsedTime)
        {
            int area = 0;
            for (int i = left; i <= right; i++)
            {
                area += Data[i];
            }
            return area / UsedTime;
        }

        /// <summary>
        /// 强度计算
        /// </summary>
        /// <param name="elementlist">元素列表</param>
        /// <param name="spec">谱数据数组</param>
        /// <param name="demarcateIntercept">能量刻度的截距</param>
        /// <param name="demarcateSlope">能量刻度的斜率</param>
        /// <param name="detector">探测器</param>
        /// <param name="FpDeltaCoef">FixedGaussMode用到的参数</param>
        /// <returns>计算是否成功</returns>
        public static bool GetThickIntensity(ElementList elementlist, SpecEntity[] spec, double demarcateIntercept, double demarcateSlope, Detector detector, double FxDeltaCoef, int ConCurrent)
        {
            int ecElemntCount = 0;
            List<int> ecElementIndexList = new List<int>();
            for (int i = 0; i < elementlist.Items.Count; i++)
            {
                if (elementlist.Items[i].IntensityWay != IntensityWay.FPGauss)
                {
                    ecElemntCount++;
                    //ecElementIndexList.Add(i);
                }
                ecElementIndexList.Add(i);
            }
            if (ecElemntCount != elementlist.Items.Count)
            {
                FpWorkCurve.FPGauss(elementlist, spec, demarcateIntercept, demarcateSlope);
            }
            int length = (int)elementlist.WorkCurve.Condition.Device.SpecLength; //add by cayunhe
            Element[] elementarray = new Element[ecElementIndexList.Count];
            int maxConditionID = 0;
            for (int i = 0; i < elementarray.Length; i++)
            {
                elementarray[i].Caption = elementlist.Items[ecElementIndexList[i]].Caption;
                elementarray[i].Formula = elementlist.Items[ecElementIndexList[i]].Formula;
                ////2011-08-30修改元素对应的谱序号。
                //bool specTrue = false;
                //for (int j = 0; j < spec.Length; j++)
                //{
                //    if (elementlist.Items[ecElementIndexList[i]].DevParamId == spec[j].DeviceParameter.Id)
                //    { 
                //        specTrue = true;
                //        elementarray[i].ConditionCode = j;
                //        break;
                //    }
                //}
                //if (!specTrue)
                //{
                //    return false;
                //    //throw new Exception(Info.CAPIERR_OTHER);
                //}
                elementarray[i].ConditionCode = elementlist.Items[ecElementIndexList[i]].ConditionID;
                if (elementarray[i].ConditionCode > maxConditionID)
                {
                    maxConditionID = elementarray[i].ConditionCode;
                }
                elementarray[i].SpectrumData = new double[ArithDllInterface.MAXSPECTRUMDATALENGTH];
                double[] specE = Helper.ToDoubles(elementlist.Items[ecElementIndexList[i]].SSpectrumData);
                elementarray[i].SpectrumDataLen = specE.Length;
                //换算单秒空测谱  
                //空测谱/时间    未加到此版本


                //   //除以实际管流 * 设置管流
                for (int k = 0; k < elementarray[i].SpectrumDataLen; k++)
                {
                    if (k < specE.Length)
                    {
                      //  elementarray[i].SpectrumData[k] = specE[k] ;
                         elementarray[i].SpectrumData[k] = specE[k] * spec[0].TubCurrent;
                        //if (elementarray[i].Caption == "Ni")
                        //    elementarray[i].SpectrumData[k] = specE[k] * 1.2447;
                        //else
                        //    elementarray[i].SpectrumData[k] = specE[k] * 1.2232;
                    }
                    else elementarray[i].SpectrumData[k] = 0;
                }
                elementarray[i].AtomicNumber = elementlist.Items[ecElementIndexList[i]].AtomicNumber;
                elementarray[i].AnalyteLine = (int)elementlist.Items[ecElementIndexList[i]].AnalyteLine;
                elementarray[i].BaseHigh = elementlist.Items[ecElementIndexList[i]].BaseHigh;
                elementarray[i].BaseLow = elementlist.Items[ecElementIndexList[i]].BaseLow;
                elementarray[i].PeakLow = elementlist.Items[ecElementIndexList[i]].PeakLow;
                elementarray[i].PeakHigh = elementlist.Items[ecElementIndexList[i]].PeakHigh;
                elementarray[i].IsPeakDivBase = elementlist.Items[ecElementIndexList[i]].PeakDivBase;
                elementarray[i].BaseIntensityMode = (int)elementlist.Items[ecElementIndexList[i]].BaseIntensityWay;
                elementarray[i].IsIntensityCompare = elementlist.Items[ecElementIndexList[i]].IntensityComparison;
                elementarray[i].IntentsityMod = (int)elementlist.Items[ecElementIndexList[i]].IntensityWay;
                elementarray[i].InflueceCoefficientNumber = 0;
                elementarray[i].InfluenceElements = elementlist.Items[ecElementIndexList[i]].SInfluenceElements;
                elementarray[i].InfluenceCoefficientList = new double[ArithDllInterface.MAXINFLUENCECOEFFICIENT];
                for (int j = 0; j < elementarray[i].InfluenceCoefficientList.Length && j < elementlist.Items[ecElementIndexList[i]].InfluenceCoefficientLists.Length; j++)
                {
                    elementarray[i].InfluenceCoefficientList[j] = elementlist.Items[ecElementIndexList[i]].InfluenceCoefficientLists[j];
                    elementarray[i].InflueceCoefficientNumber++;
                }
                elementarray[i].CalculationMod = (int)elementlist.Items[ecElementIndexList[i]].CalculationWay;
                elementarray[i].K0 = elementlist.Items[ecElementIndexList[i]].K0;
                elementarray[i].K1 = elementlist.Items[ecElementIndexList[i]].K1;
                elementarray[i].IsIntensityCompare = elementlist.Items[ecElementIndexList[i]].IntensityComparison;
                elementarray[i].ComparisionCoefficient = elementlist.Items[ecElementIndexList[i]].ComparisonCoefficient;
                elementarray[i].BPeakHigh = elementlist.Items[ecElementIndexList[i]].BPeakHigh;
                elementarray[i].BPeakLow = elementlist.Items[ecElementIndexList[i]].BPeakLow;
                elementarray[i].Intensity = elementlist.Items[ecElementIndexList[i]].IntensityWay == IntensityWay.FPGauss ? elementlist.Items[ecElementIndexList[i]].Intensity : 0;
                elementarray[i].Error = elementlist.Items[ecElementIndexList[i]].IntensityWay == IntensityWay.FPGauss ? elementlist.Items[ecElementIndexList[i]].Error : 0;
                //add by chuyaqin 2011-07-11
                string strRefElements = string.Empty;
                string strRefLR = string.Empty;
                for (int j = 0; j < elementlist.Items[ecElementIndexList[i]].References.Count; j++)
                {
                    strRefElements += elementlist.Items[ecElementIndexList[i]].References[j].ReferenceElementName + ",";
                    strRefLR += elementlist.Items[ecElementIndexList[i]].References[j].ReferenceLeftBorder + "," + elementlist.Items[ecElementIndexList[i]].References[j].ReferenceRightBorder + ";";
                }
                elementarray[i].ReferenceElements = strRefElements;
                elementarray[i].ReferenceElementsLR = strRefLR;
            }

            if (maxConditionID >= spec.Length)
            {
                return false;
            }

            int errCode = ArithDllInterface.CAPIERR_NOERROR;


            ArithDllInterface.SetElementListConfigure(elementarray, elementarray.Length, elementlist.RhIsLayer, elementlist.RhLayerFactor, elementlist.RhIsMainElementInfluence);
            ArithDllInterface.SetIntFixedGaussParam(ref errCode, FxDeltaCoef);

            int[,] datas = new int[spec.Length, length];
            //int[] usedTimes = new int[spec.Length];
            double[] usedTimes = new double[spec.Length];
            for (int i = 0; i < spec.Length; i++)
            {
                //单秒空测谱 * 谱时间 未加此版本
                var ints = spec[i].SpecDatac;  
                for (int j = 0; j < length; j++)
                {
                    datas[i, j] = ints[j]; // 测金

                    //除以实际管流 * 设置管流
                  //  datas[i, j] = (int)Math.Round(ints[j] / spec[i].TubCurrent * 2000);
                  // datas[i, j] = (int)Math.Round(ints[j] / spec[i].TubCurrent);  //归一管流

                }
                usedTimes[i] = spec[i].UsedTime;
            }
            errCode = ArithDllInterface.CAPIERR_NOERROR;
            if (!ArithDllInterface.ThickCalculateIntensity(ref errCode, elementarray, datas, spec.Length, length, usedTimes))
            {
                ArithDllInterface.MessageCatch(errCode);
                return false;
            }
            for (int i = 0; i < elementarray.Length; i++)
            {
                if (Double.IsNaN(elementarray[i].Intensity))
                {
                    elementlist.Items[ecElementIndexList[i]].Intensity = 0;
                    elementlist.Items[ecElementIndexList[i]].Error = 0;
                }
                else
                {
                    elementlist.Items[ecElementIndexList[i]].Intensity = elementarray[i].Intensity;
                    elementlist.Items[ecElementIndexList[i]].Error = elementarray[i].Error;
                }
            }
            return true;
        }



        

#region  未用到的
        ///// <summary>
        ///// 强度计算
        ///// </summary>
        ///// <param name="elementlist">元素列表</param>
        ///// <param name="spec">谱数据数组</param>
        ///// <param name="demarcateIntercept">能量刻度的截距</param>
        ///// <param name="demarcateSlope">能量刻度的斜率</param>
        ///// <param name="detector">探测器</param>
        ///// <param name="FpDeltaCoef">FixedGaussMode用到的参数</param>
        ///// <returns>计算是否成功</returns>
        //public static bool GetThickIntensityPure(out double[] doubleIntensity, ElementList elementlist, double demarcateIntercept, double demarcateSlope)
        //{
        //    int ecElemntCount = 0;
        //    List<int> ecElementIndexList = new List<int>();
        //    for (int i = 0; i < elementlist.Items.Count; i++)
        //    {
        //        if (elementlist.Items[i].IntensityWay != IntensityWay.FPGauss)
        //        {
        //            ecElemntCount++;
        //            //ecElementIndexList.Add(i);
        //        }
        //        ecElementIndexList.Add(i);
        //    }
        //    double[] aa = FpWorkCurve.FPGaussPure(elementlist, demarcateIntercept, demarcateSlope);
        //    doubleIntensity = new double[elementlist.Items.Count];
        //    int fpcount = 0;
        //    for (int i = 0; i < elementlist.Items.Count; i++)
        //    {
        //        if (elementlist.Items[i].IntensityWay == IntensityWay.FPGauss)
        //        {
        //            doubleIntensity[i] = aa[fpcount++];
        //        }
        //    }
        //    int[] usedTimes = new int[ecElemntCount];
        //    double fixCoef = 0;
        //    int[,] datas = new int[ecElemntCount, ArithDllInterface.MAXSPECTRUMDATALENGTH];
        //    //if (ecElemntCount > 0)
        //    {
        //        //int length = 2048;//这里 需要记录在数据库
        //        Element[] elementarray = new Element[ecElementIndexList.Count];
        //        for (int i = 0; i < elementarray.Length; i++)
        //        {
        //            elementarray[i].Caption = elementlist.Items[ecElementIndexList[i]].Caption;
        //            elementarray[i].Formula = elementlist.Items[ecElementIndexList[i]].Formula;
        //            elementarray[i].SpectrumData = new double[ArithDllInterface.MAXSPECTRUMDATALENGTH];
        //            double[] specE = Helper.ToDoubles(elementlist.Items[ecElementIndexList[i]].SSpectrumData);
        //            elementarray[i].SpectrumDataLen = specE.Length;
        //            for (int k = 0; k < elementarray[i].SpectrumDataLen; k++)
        //            {
        //                if (k < specE.Length)
        //                {
        //                    elementarray[i].SpectrumData[k] = specE[k];
        //                }
        //                else
        //                {

        //                    elementarray[i].SpectrumData[k] = 0;
        //                }
        //            }
        //            usedTimes[i] = 1;
        //            elementarray[i].AtomicNumber = elementlist.Items[ecElementIndexList[i]].AtomicNumber;
        //            elementarray[i].AnalyteLine = (int)elementlist.Items[ecElementIndexList[i]].AnalyteLine;
        //            elementarray[i].BaseHigh = elementlist.Items[ecElementIndexList[i]].BaseHigh;
        //            elementarray[i].BaseLow = elementlist.Items[ecElementIndexList[i]].BaseLow;
        //            elementarray[i].PeakLow = elementlist.Items[ecElementIndexList[i]].PeakLow;
        //            elementarray[i].PeakHigh = elementlist.Items[ecElementIndexList[i]].PeakHigh;
        //            elementarray[i].IsPeakDivBase = elementlist.Items[ecElementIndexList[i]].PeakDivBase;
        //            elementarray[i].IsIntensityCompare = elementlist.Items[ecElementIndexList[i]].IntensityComparison;
        //            elementarray[i].IntentsityMod = (int)elementlist.Items[ecElementIndexList[i]].IntensityWay;
        //            elementarray[i].InflueceCoefficientNumber = 0;
        //            elementarray[i].InfluenceElements = elementlist.Items[ecElementIndexList[i]].SInfluenceElements;
        //            //elementarray[i].ReferenceElements = elementlist.Items[ecElementIndexList[i]].SReferenceElements;
        //            //add by chuyaqin 2011-07-11
        //            string strRefElements = string.Empty;
        //            string strRefLR = string.Empty;
        //            for (int j = 0; j < elementlist.Items[ecElementIndexList[i]].References.Count; j++)
        //            {
        //                strRefElements += elementlist.Items[ecElementIndexList[i]].References[j].ReferenceElementName + ",";
        //                strRefLR += elementlist.Items[ecElementIndexList[i]].References[j].ReferenceLeftBorder + "," + elementlist.Items[ecElementIndexList[i]].References[j].ReferenceRightBorder + ";";
        //            }
        //            elementarray[i].ReferenceElements = strRefElements;
        //            elementarray[i].ReferenceElementsLR = strRefLR;

        //            elementarray[i].InfluenceCoefficientList = new double[ArithDllInterface.MAXINFLUENCECOEFFICIENT];
        //            for (int j = 0; j < elementarray[i].InfluenceCoefficientList.Length && j < elementlist.Items[i].InfluenceCoefficientLists.Length; j++)
        //            {
        //                elementarray[i].InfluenceCoefficientList[j] = elementlist.Items[ecElementIndexList[i]].InfluenceCoefficientLists[j];
        //                elementarray[i].InflueceCoefficientNumber++;
        //            }
        //            elementarray[i].ConditionCode = i;
        //            elementarray[i].CalculationMod = (int)elementlist.Items[ecElementIndexList[i]].CalculationWay;
        //            elementarray[i].K0 = elementlist.Items[ecElementIndexList[i]].K0;
        //            elementarray[i].K1 = elementlist.Items[ecElementIndexList[i]].K1;
        //            //elementarray[i].Error = elementlist.Items[ecElementIndexList[i]].Error;
        //            elementarray[i].Error = 0;
        //            elementarray[i].ComparisionCoefficient = elementlist.Items[ecElementIndexList[i]].ComparisonCoefficient;
        //            elementarray[i].BPeakHigh = elementlist.Items[ecElementIndexList[i]].BPeakHigh;
        //            elementarray[i].BPeakLow = elementlist.Items[ecElementIndexList[i]].BPeakLow;
        //            //elementarray[i].Intensity = 0;
        //            elementarray[i].Intensity = elementlist.Items[ecElementIndexList[i]].IntensityWay == IntensityWay.FPGauss ? elementlist.Items[ecElementIndexList[i]].Intensity : 0;
        //            elementarray[i].Error = elementlist.Items[ecElementIndexList[i]].IntensityWay == IntensityWay.FPGauss ? elementlist.Items[ecElementIndexList[i]].Error : 0;
        //            SpecList list = SpecList.FindOne(w => w.Name == elementlist.Items[ecElementIndexList[i]].ElementSpecName);
        //            if (list == null)
        //            {
        //                usedTimes[i] = 1;
        //                continue;
        //            }
        //            int[] datatemp = list.Specs[elementlist.Items[ecElementIndexList[i]].ConditionID].SpecDatas;
        //            for (int k = 0; k < datatemp.Length; k++)
        //            {
        //                datas[i, k] = datatemp[k];
        //            }
        //            usedTimes[i] = (int)list.Specs[elementlist.Items[ecElementIndexList[i]].ConditionID].SpecTime;
        //            fixCoef = list.Specs[0].DeviceParameter.Condition.Device.Detector.FixGaussDelta;
        //        }
        //        int errCode = ArithDllInterface.CAPIERR_NOERROR;

        //        ArithDllInterface.SetElementListConfigure(elementarray, elementarray.Length, elementlist.RhIsLayer, elementlist.RhLayerFactor);
        //        ArithDllInterface.SetIntFixedGaussParam(ref errCode, fixCoef);

        //        if (!ArithDllInterface.ThickCalculateIntensity(ref errCode, elementarray, datas, ecElemntCount, ArithDllInterface.MAXSPECTRUMDATALENGTH, usedTimes))
        //        {
        //            ArithDllInterface.MessageCatch(errCode);
        //            return false;
        //        }
        //        for (int i = 0; i < elementarray.Length; i++)
        //        {
        //            if (Double.IsNaN(elementarray[i].Intensity))
        //            {
        //                doubleIntensity[ecElementIndexList[i]] = 0;
        //            }
        //            else
        //            {
        //                doubleIntensity[ecElementIndexList[i]] = elementarray[i].Intensity;
        //            }
        //        }
        //    }

        //    return true;
        //}
#endregion
        
    }
}
