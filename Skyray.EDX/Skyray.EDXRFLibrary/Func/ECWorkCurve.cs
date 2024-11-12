using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.EDXRFLibrary
{
    class ECWorkCurve
    {
        //标样点数据点
        SampleRecord[,] SampleDatas;

        ///// <summary>
        ///// 设置优化数据表
        ///// </summary>
        ///// <param name="optimiztions"></param>
        //public static Optimization[] InitOptimizationList(ElementList elementlist)
        //{
        //    List<string> listOptim = new List<string>();
        //    for (int i = 0; i < elementlist.Items.Count; i++)
        //    {
        //        if (elementlist.Items[i].Optimiztion != null && elementlist.Items[i].Optimiztion.Count > 0
        //            && !listOptim.Contains(elementlist.Items[i].Caption))
        //        {
        //            //var iContent = elementlist.Items[i].Content;//含量

        //            //var value = elementlist.Items[i].Optimiztion[j].OptimiztionValue;
        //            //var range = elementlist.Items[i].Optimiztion[j].OptimiztionRange;
        //            //if (iContent >= value * (1 - range / 100) && iContent <= value * (1 + range / 100))
        //            {
        //                listOptim.Add(elementlist.Items[i].Caption);
        //            }
        //        }
        //    }
        //    Optimization[] optimizations = new Optimization[listOptim.Count];
        //    for (int i = 0; i < listOptim.Count; i++)
        //    {
        //        Optimization optiza = new Optimization();
        //        optiza.OptimizationList = new OptimizationItem[ArithDllInterface.MAXOPTIMIZATIONITEM];
        //        optiza.Text = listOptim[i];
        //        optiza.OptimizationItemNumber = 0;
        //        optimizations[i] = optiza;
        //    }
        //    for (int i = 0; i < optimizations.Length; i++)
        //    {
        //        for (int k = 0; k < elementlist.Items.Count; k++)
        //        {
        //            if (elementlist.Items[k].Caption.CompareTo(listOptim[i]) == 0)
        //            {
        //                int j;
        //                for (j = 0; j < elementlist.Items[k].Optimiztion.Count; j++)
        //                {
        //                    //var iContent = elementlist.Items[k].Content;//含量
        //                    var value = elementlist.Items[k].Optimiztion[j].OptimiztionValue;
        //                    //var range = elementlist.Items[k].Optimiztion[j].OptimiztionRange;2012-01-06
        //                    //朱庆华
        //                    var rangL = -elementlist.Items[k].Optimiztion[j].OptimiztionMin;
        //                    var rangR = elementlist.Items[k].Optimiztion[j].OptimiztionMax;
        //                    //if (iContent >= value * (1 - range / 100) && iContent <= value * (1 + range / 100))
        //                    {
        //                        OptimizationItem optItem = new OptimizationItem();
        //                        optItem.Factor = elementlist.Items[k].Optimiztion[j].OptimiztionFactor;
        //                        //optItem.Range = range/100;
        //                        optItem.RangeL = rangL;
        //                        optItem.RangeR = rangR;
        //                        optItem.Value = value;
        //                        optItem.OptimizetionType = elementlist.Items[k].Optimiztion[j].OptimizetionType;
        //                        optimizations[i].OptimizationList[optimizations[i].OptimizationItemNumber] = optItem;//此处出现几率性BUG。。。???
        //                        optimizations[i].OptimizationItemNumber++;
        //                    }
        //                }
        //                break;
        //            }
        //        }

        //    }
        //    //Optimization[] optimizations = new Optimization[0];
        //    return optimizations;
        //}
        ///// <summary>
        ///// 获得曲线标样信息
        ///// </summary>
        ///// <param name="standardSamples"></param>
        //public void GetStandardSamples(StandSample[] standardSamples, ElementList elementlist)
        //{
        //    List<string> sampleNames = new List<string>();
        //    for (int i = 0; i < standardSamples.Length; i++)
        //    {
        //        if (!sampleNames.Contains(standardSamples[i].SampleName))
        //        {
        //            sampleNames.Add(standardSamples[i].SampleName);
        //        }
        //    }
        //    SampleDatas = new SampleRecord[sampleNames.Count, elementlist.Items.Count];
        //    for (int i = 0; i < standardSamples.Length; i++)
        //    {
        //        int indexE = elementlist.GetElementIndex(standardSamples[i].ElementName);
        //        int indexS = sampleNames.IndexOf(standardSamples[i].SampleName);
        //        if (indexE >= 0 && indexS >= 0)
        //        {
        //            SampleRecord data = new SampleRecord();
        //            data.Enabled = standardSamples[i].Active;
        //            data.Text = standardSamples[i].ElementName;
        //            data.XValue = double.Parse(standardSamples[i].X);
        //            data.YValue = double.Parse(standardSamples[i].Y);
        //            SampleDatas[indexS, indexE] = data;
        //        }
        //    }
        //}
        /// <summary>
        /// 获得曲线标样信息
        /// </summary>
        /// <param name="standardSamples"></param>
        public void GetStandardSamples(ElementList elementlist)
        {
            //List<string> sampleNames = new List<string>();
            //for (int i = 0; i < elementlist.Items.Count; i++)
            //{
            //    for (int j = 0; j < elementlist.Items[i].Samples.Count; j++)
            //    {
            //        if (!sampleNames.Contains(elementlist.Items[i].Samples[j].SampleName))
            //        {
            //            sampleNames.Add(elementlist.Items[i].Samples[j].SampleName);
            //        }
            //    }
            //}
            //SampleDatas = new SampleRecord[sampleNames.Count, elementlist.Items.Count];
            //for (int i = 0; i < elementlist.Items.Count; i++)
            //{
            //    for (int j = 0; j < elementlist.Items[i].Samples.Count; j++)
            //    {
            //        int indexE = elementlist.GetElementIndex(elementlist.Items[i].Caption);
            //        int indexS = sampleNames.IndexOf(elementlist.Items[i].Samples[j].SampleName);
            //        if (indexE >= 0 && indexS >= 0)
            //        {
            //            SampleRecord data = new SampleRecord();
            //            data.Enabled = elementlist.Items[i].Samples[j].Active;
            //            data.Text = elementlist.Items[i].Samples[j].ElementName;
            //            data.XValue = double.Parse(elementlist.Items[i].Samples[j].X);
            //            data.YValue = double.Parse(elementlist.Items[i].Samples[j].Y);
            //            SampleDatas[indexS, indexE] = data;
            //        }
            //    }
            //}
            //2014-02-25修改全部不激活状态的样品不参与计算:针对PK的修改
            List<string> sampleNames = new List<string>();
            for (int i = 0; i < elementlist.Items.Count; i++)
            {
                for (int j = 0; j < elementlist.Items[i].Samples.Count; j++)
                {
                    if (elementlist.Items[i].Samples[j].Active&&!sampleNames.Contains(elementlist.Items[i].Samples[j].SampleName))
                    {
                        sampleNames.Add(elementlist.Items[i].Samples[j].SampleName);
                    }
                }
            }
            SampleDatas = new SampleRecord[sampleNames.Count, elementlist.Items.Count];
            for (int i = 0; i < elementlist.Items.Count; i++)
            {
                for (int j = 0; j < elementlist.Items[i].Samples.Count; j++)
                {
                    int indexE = elementlist.GetElementIndex(elementlist.Items[i].Caption);
                    int indexS = sampleNames.IndexOf(elementlist.Items[i].Samples[j].SampleName);
                    if (indexE >= 0 && indexS >= 0)
                    {
                        SampleRecord data = new SampleRecord();
                        data.Enabled = elementlist.Items[i].Samples[j].Active;
                        data.Text = elementlist.Items[i].Samples[j].ElementName;
                        data.XValue = double.Parse(elementlist.Items[i].Samples[j].X);
                        data.YValue = double.Parse(elementlist.Items[i].Samples[j].Y);
                        SampleDatas[indexS, indexE] = data;
                    }
                }
            }
        }


        public void ClearStandards()
        {
            SampleDatas = new SampleRecord[0, 0];
        }
        /// <summary>
        /// 经验系数法的求含量
        /// </summary>
        /// <param name="elementlist">元素列表</param>
        /// <returns></returns>
        public bool GetECContent(ElementList elementlist, bool IsRhLayer, double RhLayerFactor,SpecListEntity speclist)
        {
            GetStandardSamples(elementlist);
            if (SampleDatas.Length <= 0)
            {
                return false;
            }
            Element[] elementarray = new Element[elementlist.Items.Count];
            for (int i = 0; i < elementarray.Length; i++)
            {
                elementarray[i].Caption = elementlist.Items[i].Caption;
                elementarray[i].SpectrumData = new double[ArithDllInterface.MAXSPECTRUMDATALENGTH];
                double[] spec = Helper.ToDoubles(elementlist.Items[i].SSpectrumData);
                elementarray[i].SpectrumDataLen = spec.Length;
                for (int k = 0; k < elementarray[i].SpectrumDataLen; k++)
                {
                    if (k < spec.Length)
                    {
                        elementarray[i].SpectrumData[k] = spec[k];
                    }
                    else elementarray[i].SpectrumData[k] = 0;
                }
                elementarray[i].AtomicNumber = elementlist.Items[i].AtomicNumber;
                elementarray[i].AnalyteLine = (int)elementlist.Items[i].AnalyteLine;
                elementarray[i].BaseHigh = elementlist.Items[i].BaseHigh;
                elementarray[i].BaseLow = elementlist.Items[i].BaseLow;
                elementarray[i].PeakLow = elementlist.Items[i].PeakLow;
                elementarray[i].PeakHigh = elementlist.Items[i].PeakHigh;
                elementarray[i].IsPeakDivBase = elementlist.Items[i].PeakDivBase;
                elementarray[i].BaseIntensityMode = (int)elementlist.Items[i].BaseIntensityWay;
                elementarray[i].IntentsityMod = (int)elementlist.Items[i].IntensityWay;
                elementarray[i].InflueceCoefficientNumber = 0;
                elementarray[i].InfluenceElements = elementlist.Items[i].SInfluenceElements;
                elementarray[i].ReferenceElements = elementlist.Items[i].SReferenceElements;
                elementarray[i].InfluenceCoefficientList = new double[ArithDllInterface.MAXINFLUENCECOEFFICIENT];
                for (int j = 0; j < elementarray[i].InfluenceCoefficientList.Length && j < elementlist.Items[i].InfluenceCoefficientLists.Length; j++)
                {
                    elementarray[i].InfluenceCoefficientList[j] = elementlist.Items[i].InfluenceCoefficientLists[j];
                    elementarray[i].InflueceCoefficientNumber++;
                }
                elementarray[i].ConditionCode = elementlist.Items[i].ConditionID;
                elementarray[i].CalculationMod = (int)elementlist.Items[i].CalculationWay;
                elementarray[i].K0 = elementlist.Items[i].K0;
                elementarray[i].K1 = elementlist.Items[i].K1;
                elementarray[i].Error = elementlist.Items[i].Error;
                elementarray[i].Intensity = elementlist.Items[i].Intensity; 
                elementarray[i].Limit = elementlist.Items[i].Limit;
                elementarray[i].ElementMod = (int)elementlist.Items[i].Flag;
                elementarray[i].IsDisplay = elementlist.Items[i].IsDisplay;
            }
            ArithDllInterface.SetElementListConfigure(elementarray, elementarray.Length, IsRhLayer, RhLayerFactor, elementlist.RhIsMainElementInfluence);
           // Optimization[] optimizations = InitOptimizationList(elementlist);

            //XRF烧失量
            double dUnitaryValue = elementlist.IsUnitary ? elementlist.UnitaryValue : -1;

            if (elementlist.IsUnitary
                && speclist != null
                && speclist.Loss >= 0)
            {
                dUnitaryValue = dUnitaryValue * ((100 - speclist.Loss) / 100);
            }

            //if (elementlist.WorkCurve.FuncType == FuncType.XRF
            //    && elementlist.IsUnitary
            //    && speclist.Loss > 0)
            //{
            //    dUnitaryValue = dUnitaryValue * ((100 - speclist.Loss)/100);
            //}

            //ArithDllInterface.SetEcCurveConfigure(SampleDatas, SampleDatas.GetLength(0), SampleDatas.GetLength(1), elementlist.IsUnitary ? elementlist.UnitaryValue : -1, optimizations, optimizations.Length);
            //ArithDllInterface.SetEcCurveConfigure(SampleDatas, SampleDatas.GetLength(0), SampleDatas.GetLength(1), dUnitaryValue, optimizations, optimizations.Length);
            ArithDllInterface.SetEcCurveConfigure(SampleDatas, SampleDatas.GetLength(0), SampleDatas.GetLength(1), dUnitaryValue);
            int errCode = ArithDllInterface.CAPIERR_NOERROR;
            if (!ArithDllInterface.CaculateECContent(ref errCode, elementarray))
            {
                ArithDllInterface.MessageCatch(errCode);
                return false;
            }
            for (int i = 0; i < elementarray.Length; i++)
            {
                elementlist.Items[i].Content = (Double.IsInfinity(elementarray[i].Content) || Double.IsNaN(elementarray[i].Content)) ? 0 : elementarray[i].Content;
                elementlist.Items[i].Content = elementlist.Items[i].Content * elementlist.Items[i].Contentcoeff;
                elementlist.Items[i].Error = (Double.IsInfinity(elementarray[i].Error) || Double.IsNaN(elementarray[i].Error)) ? 0 : elementarray[i].Error;
                //elementlist.Items[i].SInfluenceCoefficients = Helper.ToStrs(new double[elementarray[i].InflueceCoefficientNumber]);
                //for (int j = 0; j < elementlist.Items[i].InfluenceCoefficientLists.Length; j++)
                //{
                //    elementlist.Items[i].InfluenceCoefficientLists[j] = elementarray[i].InfluenceCoefficientList[j];
                //}
                //elementlist.Items[i].SInfluenceCoefficients = Helper.ToStrs(elementlist.Items[i].InfluenceCoefficientLists);
            }

            #region 数据优化3

            //double sumOptimizeCont = 0;
            //double UnitaryValue = elementlist.IsUnitary ? elementlist.UnitaryValue : -1;
            //double[] orginContent = new double[elementlist.Items.Count];
            //for (int i = 0; i < elementlist.Items.Count; ++i)
            //{
            //    orginContent[i] = elementlist.Items[i].Content;
            //}

            //for (int i = 0; i < elementlist.Items.Count; ++i)
            //{
            //    double value = elementlist.Items[i].Content;
            //    bool hasOpt = false;
            //    for (int j = 0; j < elementlist.Items[i].Optimiztion.Count; j++)
            //    {
            //        if (elementlist.Items[i].Optimiztion[j].OptimizetionType == 2)
            //        {
            //            hasOpt = true;
            //            if (elementlist.Items[i].Optimiztion[j].OptimiztionValue + elementlist.Items[i].Optimiztion[j].OptimiztionMax >= value &&
            //                elementlist.Items[i].Optimiztion[j].OptimiztionValue - elementlist.Items[i].Optimiztion[j].OptimiztionMin <= value)
            //            {
            //                value = value + elementlist.Items[i].Optimiztion[j].OptimiztionFactor;
            //                elementlist.Items[i].Content = value;
            //                break;
            //            }
            //        }
            //    }
            //    if (hasOpt == true)
            //    {
            //        sumOptimizeCont += elementlist.Items[i].Content;
            //    }
            //}

            //if (UnitaryValue > 0 && elementlist.IsUnitary && UnitaryValue > sumOptimizeCont)
            //{
            //    double sumCont = 0;
            //    sumCont = -sumOptimizeCont;
            //    for (int i = 0; i < elementlist.Items.Count; ++i)
            //    {
            //        //if (elementList.Items[i].LayerNumber == layerCount + 1)
            //        {
            //            if (elementlist.Items[i].Content < 0)
            //            {
            //                elementlist.Items[i].Content = 0;
            //            }
            //            sumCont += elementlist.Items[i].Content;
            //        }
            //    }
            //    if (sumCont > 0)
            //    {
            //        sumCont = (UnitaryValue - sumOptimizeCont) / sumCont;
            //    }
            //    else
            //    {
            //        sumCont = 0;
            //    }
            //    for (int i = 0; i < elementlist.Items.Count; ++i)
            //    {
            //        if (elementlist.Items[i].Optimiztion.ToList().FindAll(a => a.OptimizetionType == 2).Count > 0)
            //        {
            //            continue;
            //        }
            //        elementlist.Items[i].Content *= sumCont;
            //    }
            //}
            //else if (UnitaryValue > 0 && elementlist.IsUnitary && UnitaryValue <= sumOptimizeCont)
            //{
            //    for (int i = 0; i < elementlist.Items.Count; ++i)
            //    {
            //        elementlist.Items[i].Content = orginContent[i];
            //    }
            //}

            #endregion

            #region 数据优化和归一

            double sumOptimizeCont = 0;
            double UnitaryValue = dUnitaryValue;

            double[] orginContent = new double[elementlist.Items.Count];
            for (int i = 0; i < elementlist.Items.Count; ++i)
            {
                orginContent[i] = elementlist.Items[i].Content;
            }

            for (int i = 0; i < elementlist.Items.Count; ++i)
            {
                double value = elementlist.Items[i].Content;
                bool hasOpt = false;
                //for (int j = 0; j < elementlist.Items[i].Optimiztion.Count; j++)
                //{
                //    if (elementlist.Items[i].Optimiztion[j].OptimizetionType == 2)
                //    {
                //        hasOpt = true;
                //        if (elementlist.Items[i].Optimiztion[j].OptimiztionValue + elementlist.Items[i].Optimiztion[j].OptimiztionMax >= value &&
                //            elementlist.Items[i].Optimiztion[j].OptimiztionValue - elementlist.Items[i].Optimiztion[j].OptimiztionMin <= value)
                //        {
                //            value = value + elementlist.Items[i].Optimiztion[j].OptimiztionFactor;
                //            elementlist.Items[i].Content = value;
                //            break;
                //        }
                //    }
                //    else if (elementlist.Items[i].Optimiztion[j].OptimizetionType == 1)
                //    {
                //        hasOpt = true;
                //        if (elementlist.Items[i].Optimiztion[j].OptimiztionValue + elementlist.Items[i].Optimiztion[j].OptimiztionMax >= value &&
                //            elementlist.Items[i].Optimiztion[j].OptimiztionValue - elementlist.Items[i].Optimiztion[j].OptimiztionMin <= value)
                //        {
                //            value = elementlist.Items[i].Optimiztion[j].OptimiztionValue;
                //            elementlist.Items[i].Content = value;
                //            break;
                //        }
                //    }
                //    else if (elementlist.Items[i].Optimiztion[j].OptimizetionType == 0)
                //    {
                //        hasOpt = true;
                //        if (elementlist.Items[i].Optimiztion[j].OptimiztionValue + elementlist.Items[i].Optimiztion[j].OptimiztionMax >= value &&
                //            elementlist.Items[i].Optimiztion[j].OptimiztionValue - elementlist.Items[i].Optimiztion[j].OptimiztionMin <= value)
                //        {
                //            value = value + (elementlist.Items[i].Optimiztion[j].OptimiztionValue - value) * elementlist.Items[i].Optimiztion[j].OptimiztionFactor;
                //            elementlist.Items[i].Content = value;
                //            break;
                //        }
                //    }
                //    else if (elementlist.Items[i].Optimiztion[j].OptimizetionType == 3 && ElementList.IsPKCatchValue)
                //    {
                //        hasOpt = true;
                //        value = value * (1 - elementlist.Items[i].Optimiztion[j].OptimiztionFactor) + elementlist.Items[i].Optimiztion[j].OptimiztionValue * elementlist.Items[i].Optimiztion[j].OptimiztionFactor;
                //        elementlist.Items[i].Content = value;
                //        break;
                //    }
                //}
                //优化1
                if (elementlist.Items[i].Optimiztion == null || elementlist.Items[i].Optimiztion.Count <= 0) continue;
                List<Optimiztion> lst = elementlist.Items[i].Optimiztion.ToList().FindAll(w => w.OptimizetionType == 0);
                if (lst != null && lst.Count > 0)
                {
                    hasOpt = true;
                    foreach (var op in lst)
                    {
                        if (op.OptimiztionValue + op.OptimiztionMax >= value &&
                            op.OptimiztionValue - op.OptimiztionMin <= value)
                        {
                            value = value + (op.OptimiztionValue - value) * op.OptimiztionFactor;
                            elementlist.Items[i].Content = value;
                            break;
                        }
                    }
                }
                //优化2
                lst = elementlist.Items[i].Optimiztion.ToList().FindAll(w => w.OptimizetionType == 1);
                if (lst != null && lst.Count > 0)
                {
                    hasOpt = true;
                    foreach (var op in lst)
                    {
                        if (op.OptimiztionValue + op.OptimiztionMax >= value &&
                            op.OptimiztionValue - op.OptimiztionMin <= value)
                        {
                            value = op.OptimiztionValue;
                            elementlist.Items[i].Content = value;
                            break;
                        }
                    }
                }
                //优化3
                lst = elementlist.Items[i].Optimiztion.ToList().FindAll(w => w.OptimizetionType == 2);
                if (lst != null && lst.Count > 0)
                {
                    hasOpt = true;
                    foreach (var op in lst)
                    {
                        if (op.OptimiztionValue + op.OptimiztionMax >= value &&
                            op.OptimiztionValue - op.OptimiztionMin <= value)
                        {
                            value = value + op.OptimiztionFactor;
                            elementlist.Items[i].Content = value;
                            break;
                        }
                    }
                }
                //优化4(PK功能)
                lst = elementlist.Items[i].Optimiztion.ToList().FindAll(w => w.OptimizetionType == 3);
                if (lst != null && lst.Count > 0&& ElementList.IsPKCatchValue)
                {
                    hasOpt = true;
                    foreach (var op in lst)
                    {
                        value = value * (1 - op.OptimiztionFactor) + op.OptimiztionValue * op.OptimiztionFactor;
                        elementlist.Items[i].Content = value;
                        break;
                    }

                   
                }
                if (hasOpt == true)
                {
                    sumOptimizeCont += elementlist.Items[i].Content;
                }
            }

            if (UnitaryValue > 0 && elementlist.IsUnitary && UnitaryValue > sumOptimizeCont)
            {
                double sumCont = 0;
                sumCont = -sumOptimizeCont;
                for (int i = 0; i < elementlist.Items.Count; ++i)
                {
                    //if (elementList.Items[i].LayerNumber == layerCount + 1)
                    {
                        if (elementlist.Items[i].Content < 0)
                        {
                            elementlist.Items[i].Content = 0;
                        }
                        sumCont += elementlist.Items[i].Content;
                    }
                }
                if (sumCont > 0)
                {
                    sumCont = (UnitaryValue - sumOptimizeCont) / sumCont;
                }
                else
                {
                    sumCont = 0;
                }
                for (int i = 0; i < elementlist.Items.Count; ++i)
                {
                    if (elementlist.Items[i].Optimiztion.ToList().FindAll(a => a.OptimizetionType != 3).Count > 0
                        || (elementlist.Items[i].Optimiztion.ToList().FindAll(a => a.OptimizetionType == 3).Count > 0 && ElementList.IsPKCatchValue))
                    {
                        continue;
                    }
                    elementlist.Items[i].Content *= sumCont;
                }
            }
            else if (UnitaryValue > 0 && elementlist.IsUnitary && UnitaryValue <= sumOptimizeCont)
            {
                for (int i = 0; i < elementlist.Items.Count; ++i)
                {
                    elementlist.Items[i].Content = orginContent[i];
                }
            }

            #endregion

            return true;

        }


        /// <summary>
        /// 根据元素名取得强度计算系数
        /// </summary>
        /// <param name="intensitycoefs">返回系数数组</param>
        /// <param name="strElemement">元素名</param>
        /// <returns></returns>
        public bool GetIntensityInfectCoefs(ElementList elementlist, out double[] intensitycoefs, int indexElemement)
        {
            GetStandardSamples(elementlist);
            if (SampleDatas.Length <= 0)
            {
                intensitycoefs = new double[0];
                return false;
            }
            Element[] elementarray = new Element[elementlist.Items.Count];
            for (int i = 0; i < elementarray.Length; i++)
            {
                elementarray[i].Caption = elementlist.Items[i].Caption;
                elementarray[i].SpectrumData = new double[ArithDllInterface.MAXSPECTRUMDATALENGTH];
                double[] spec = Helper.ToDoubles(elementlist.Items[i].SSpectrumData);
                elementarray[i].SpectrumDataLen = spec.Length;
                for (int k = 0; k < elementarray[i].SpectrumDataLen; k++)
                {
                    if (k < spec.Length)
                    {
                        elementarray[i].SpectrumData[k] = spec[k];
                    }
                    else elementarray[i].SpectrumData[k] = 0;
                }
                elementarray[i].AtomicNumber = elementlist.Items[i].AtomicNumber;
                elementarray[i].AnalyteLine = (int)elementlist.Items[i].AnalyteLine;
                elementarray[i].BaseHigh = elementlist.Items[i].BaseHigh;
                elementarray[i].BaseLow = elementlist.Items[i].BaseLow;
                elementarray[i].PeakLow = elementlist.Items[i].PeakLow;
                elementarray[i].PeakHigh = elementlist.Items[i].PeakHigh;
                elementarray[i].IsPeakDivBase = elementlist.Items[i].PeakDivBase;
                elementarray[i].BaseIntensityMode = (int)elementlist.Items[i].BaseIntensityWay;
                elementarray[i].IntentsityMod = (int)elementlist.Items[i].IntensityWay;
                elementarray[i].InflueceCoefficientNumber = 0;
                elementarray[i].InfluenceElements = elementlist.Items[i].SInfluenceElements;
                elementarray[i].ReferenceElements = elementlist.Items[i].SReferenceElements;
                elementarray[i].InfluenceCoefficientList = new double[ArithDllInterface.MAXINFLUENCECOEFFICIENT];
                for (int j = 0; j < elementarray[i].InfluenceCoefficientList.Length && j < elementlist.Items[i].InfluenceCoefficientLists.Length; j++)
                {
                    elementarray[i].InfluenceCoefficientList[j] = elementlist.Items[i].InfluenceCoefficientLists[j];
                    elementarray[i].InflueceCoefficientNumber++;
                }
                elementarray[i].ConditionCode = elementlist.Items[i].ConditionID;
                elementarray[i].CalculationMod = (int)elementlist.Items[i].CalculationWay;
                elementarray[i].K0 = elementlist.Items[i].K0;
                elementarray[i].K1 = elementlist.Items[i].K1;
                elementarray[i].Error = elementlist.Items[i].Error;
                elementarray[i].Intensity = elementlist.Items[i].Intensity;
                elementarray[i].ElementMod = (int)elementlist.Items[i].Flag;
                elementarray[i].IsDisplay = elementlist.Items[i].IsDisplay;
            }
            ArithDllInterface.SetElementListConfigure(elementarray, elementarray.Length, false, 0.0, elementlist.RhIsMainElementInfluence);
            //Optimization[] optimizations = InitOptimizationList(elementlist);
            //ArithDllInterface.SetEcCurveConfigure(SampleDatas, SampleDatas.GetLength(0), SampleDatas.GetLength(1), elementlist.IsUnitary ? elementlist.UnitaryValue : 0, optimizations, optimizations.Length);
            ArithDllInterface.SetEcCurveConfigure(SampleDatas, SampleDatas.GetLength(0), SampleDatas.GetLength(1), elementlist.IsUnitary ? elementlist.UnitaryValue : 0);
            double[] coefs = new double[ArithDllInterface.MAXINFLUENCECOEFFICIENT];
            int errCode = ArithDllInterface.CAPIERR_NOERROR;

            int count = ArithDllInterface.EcIntensityInfectedCoefs(ref errCode, coefs, indexElemement);
            if (count < 0)
            {
                ArithDllInterface.MessageCatch(errCode);
                elementlist.Items[indexElemement].SInfluenceCoefficients = "";
                intensitycoefs = new double[0];
                return false;
            }
            intensitycoefs = new double[count];
            if (count > 0)
            {
                elementlist.Items[indexElemement].SInfluenceCoefficients = Helper.ToStrs(coefs);
                for (int i = 0; i < intensitycoefs.Length; i++)
                {
                    intensitycoefs[i] = coefs[i];
                }
                return true;
            }
            else return false;
        }


        /// <summary>
        /// 根据元素名取得强度计算系数
        /// </summary>
        /// <param name="intensitycoefs">返回系数数组</param>
        /// <param name="strElemement">元素名</param>
        /// <returns></returns>
        public bool GetContentInfectCoefs(ElementList elementlist, out double[] contentcoefs, int indexElemement)
        {
            GetStandardSamples(elementlist);
            if (SampleDatas.Length <= 0)
            {
                contentcoefs = new double[0];
                return false;
            }
            Element[] elementarray = new Element[elementlist.Items.Count];
            for (int i = 0; i < elementarray.Length; i++)
            {
                elementarray[i].Caption = elementlist.Items[i].Caption;
                elementarray[i].SpectrumData = new double[ArithDllInterface.MAXSPECTRUMDATALENGTH];
                double[] spec = Helper.ToDoubles(elementlist.Items[i].SSpectrumData);
                elementarray[i].SpectrumDataLen = spec.Length;
                for (int k = 0; k < elementarray[i].SpectrumDataLen; k++)
                {
                    if (k < spec.Length)
                    {
                        elementarray[i].SpectrumData[k] = spec[k];
                    }
                    else elementarray[i].SpectrumData[k] = 0;
                }
                elementarray[i].AtomicNumber = elementlist.Items[i].AtomicNumber;
                elementarray[i].AnalyteLine = (int)elementlist.Items[i].AnalyteLine;
                elementarray[i].BaseHigh = elementlist.Items[i].BaseHigh;
                elementarray[i].BaseLow = elementlist.Items[i].BaseLow;
                elementarray[i].PeakLow = elementlist.Items[i].PeakLow;
                elementarray[i].PeakHigh = elementlist.Items[i].PeakHigh;
                elementarray[i].IsPeakDivBase = elementlist.Items[i].PeakDivBase;
                elementarray[i].BaseIntensityMode = (int)elementlist.Items[i].BaseIntensityWay;
                elementarray[i].IntentsityMod = (int)elementlist.Items[i].IntensityWay;
                elementarray[i].InflueceCoefficientNumber = 0;
                elementarray[i].InfluenceElements = elementlist.Items[i].SInfluenceElements;
                elementarray[i].ReferenceElements = elementlist.Items[i].SReferenceElements;
                elementarray[i].InfluenceCoefficientList = new double[ArithDllInterface.MAXINFLUENCECOEFFICIENT];
                for (int j = 0; j < elementarray[i].InfluenceCoefficientList.Length && j < elementlist.Items[i].InfluenceCoefficientLists.Length; j++)
                {
                    elementarray[i].InfluenceCoefficientList[j] = elementlist.Items[i].InfluenceCoefficientLists[j];
                    elementarray[i].InflueceCoefficientNumber++;
                }
                elementarray[i].ConditionCode = elementlist.Items[i].ConditionID;
                elementarray[i].CalculationMod = (int)elementlist.Items[i].CalculationWay;
                elementarray[i].K0 = elementlist.Items[i].K0;
                elementarray[i].K1 = elementlist.Items[i].K1;
                elementarray[i].Error = elementlist.Items[i].Error;
                elementarray[i].Intensity = elementlist.Items[i].Intensity;
                elementarray[i].ElementMod = (int)elementlist.Items[i].Flag;
                elementarray[i].IsDisplay = elementlist.Items[i].IsDisplay;
            }
            ArithDllInterface.SetElementListConfigure(elementarray, elementarray.Length, false, 0.0, elementlist.RhIsMainElementInfluence);
            //Optimization[] optimizations = InitOptimizationList(elementlist);
            //ArithDllInterface.SetEcCurveConfigure(SampleDatas, SampleDatas.GetLength(0), SampleDatas.GetLength(1), elementlist.IsUnitary ? elementlist.UnitaryValue : 0, optimizations, optimizations.Length);
            ArithDllInterface.SetEcCurveConfigure(SampleDatas, SampleDatas.GetLength(0), SampleDatas.GetLength(1), elementlist.IsUnitary ? elementlist.UnitaryValue : 0);
            double[] coefs = new double[ArithDllInterface.MAXINFLUENCECOEFFICIENT];
            int errCode = ArithDllInterface.CAPIERR_NOERROR;
            int count = ArithDllInterface.EcContentInfectedCoefs(ref errCode, coefs, indexElemement);
            if (count < 0)
            {
                ArithDllInterface.MessageCatch(errCode);
                contentcoefs = new double[0];
                return false;
            }
            contentcoefs = new double[count];
            if (count > 0)
            {
                for (int i = 0; i < contentcoefs.Length; i++)
                {
                    contentcoefs[i] = coefs[i];
                }
                return true;
            }
            else return false;
        }
    }
}