using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.EDXRFLibrary
{
    class ThWorkCurve
    {

        /// <summary>
        /// 测厚的计算厚度
        /// </summary>
        /// <param name="elementlist">元素列表</param>
        /// <param name="standards">标样数据</param>
        /// <param name="isAbsorption">是否自吸收</param>
        /// <param name="intCalculationWay">厚度就算方法</param>
        /// <param name="dblLimit">极限值</param>
        /// <returns></returns>
        public bool GetThick(ElementList elementlist, bool isAbsorption, ThCalculationWay intCalculationWay, double dblLimit)
        {
            int standardLen = 0;
            for (int i = 0; i < elementlist.Items.Count; i++)
            {
                standardLen += elementlist.Items[i].Samples.Count;
            }
            int count = 0;
            ThSampleRecord[] sampledatas = new ThSampleRecord[standardLen];
            for (int i = 0; i < elementlist.Items.Count; i++)
            {
                for (int j = 0; j < elementlist.Items[i].Samples.Count; j++)
                {
                    sampledatas[count].SampleCaption = elementlist.Items[i].Samples[j].SampleName;
                    sampledatas[count].ElementCaption = elementlist.Items[i].Samples[j].ElementName;
                    sampledatas[count].Layer = elementlist.Items[i].Samples[j].Layer;
                    sampledatas[count].TotalLayer = elementlist.Items[i].Samples[j].TotalLayer;
                    sampledatas[count].XValue = double.Parse(elementlist.Items[i].Samples[j].X);
                    
                    sampledatas[count].YValue = double.Parse(elementlist.Items[i].Samples[j].Z);
                    count++;
                }

            }
            if (sampledatas.Length <= 0)
            {
                return false;
            }
            Element[] elementarray = new Element[elementlist.Items.Count];
            //List<CurveElement> tempList = elementlist.Items.ToList<CurveElement>();
            //IOrderedEnumerable<CurveElement> listOrder = tempList.OrderBy(w => w.LayerNumber);
            //int count = 0;
            //foreach (var element in listOrder)
            //{
            //    //排序
            //    elementarray[count].Caption = element.Caption;
            //    elementarray[count].SpectrumData = new double[ArithDllInterface.MAXSPECTRUMDATALENGTH];
            //    int[] spec = Helper.ToInts(element.SSpectrumData);
            //    elementarray[count].SpectrumDataLen = spec.Length;
            //    for (int k = 0; k < elementarray[count].SpectrumDataLen; k++)
            //    {
            //        if (k < spec.Length)
            //        {
            //            elementarray[count].SpectrumData[k] = spec[k];
            //        }
            //        else elementarray[count].SpectrumData[k] = 0;
            //    }
            //    elementarray[count].AtomicNumber = element.AtomicNumber;
            //    elementarray[count].AnalyteLine = (int)element.AnalyteLine;
            //    elementarray[count].BaseHigh = element.BaseHigh;
            //    elementarray[count].BaseLow = element.BaseLow;
            //    elementarray[count].PeakLow = element.PeakLow;
            //    elementarray[count].PeakHigh = element.PeakHigh;
            //    elementarray[count].IsPeakDivBase = element.PeakDivBase;
            //    elementarray[count].IntentsityMod = (int)element.IntensityWay;
            //    elementarray[count].InflueceCoefficientNumber = 0;
            //    elementarray[count].InfluenceElements = element.SInfluenceElements;
            //    elementarray[count].ReferenceElements = element.SReferenceElements;
            //    elementarray[count].InfluenceCoefficientList = new double[ArithDllInterface.MAXINFLUENCECOEFFICIENT];
            //    for (int j = 0; j < elementarray[count].InfluenceCoefficientList.Length && j < element.InfluenceCoefficientLists.Length; j++)
            //    {
            //        elementarray[count].InfluenceCoefficientList[j] = element.InfluenceCoefficientLists[j];
            //        elementarray[count].InflueceCoefficientNumber++;
            //    }
            //    elementarray[count].ConditionCode = element.ConditionID;
            //    elementarray[count].CalculationMod = (int)element.CalculationWay;
            //    elementarray[count].K0 = element.K0;
            //    elementarray[count].K1 = element.K1;
            //    elementarray[count].Error = element.Error;
            //    elementarray[count].Intensity = element.Intensity;
            //    elementarray[count].LayerDensity = element.LayerDensity;
            //    elementarray[count].LayerNumber = element.LayerNumber;
            //    elementarray[count].LayerMod = (int)element.LayerFlag;
            //    elementarray[count].Thickness = element.Thickness;
            //    elementarray[count].ThicknessMod = (int)element.ThicknessUnit;
            //    count++;

            //}
            for (int i = 0; i < elementarray.Length; i++)
            {
                int desti = elementlist.Items[i].LayerNumber - 1;
                elementarray[desti].Caption = elementlist.Items[i].Caption;
                elementarray[desti].SpectrumData = new double[ArithDllInterface.MAXSPECTRUMDATALENGTH];
                double[] spec = Helper.ToDoubles(elementlist.Items[i].SSpectrumData);
                elementarray[desti].SpectrumDataLen = spec.Length;
                for (int k = 0; k < elementarray[desti].SpectrumDataLen; k++)
                {
                    if (k < spec.Length)
                    {
                        elementarray[desti].SpectrumData[k] = spec[k];
                    }
                    else elementarray[desti].SpectrumData[k] = 0;
                }
                elementarray[desti].AtomicNumber = elementlist.Items[i].AtomicNumber;
                elementarray[desti].AnalyteLine = (int)elementlist.Items[i].AnalyteLine;
                elementarray[desti].BaseHigh = elementlist.Items[i].BaseHigh;
                elementarray[desti].BaseLow = elementlist.Items[i].BaseLow;
                elementarray[desti].PeakLow = elementlist.Items[i].PeakLow;
                elementarray[desti].PeakHigh = elementlist.Items[i].PeakHigh;
                elementarray[desti].IsPeakDivBase = elementlist.Items[i].PeakDivBase;
                elementarray[desti].IntentsityMod = (int)elementlist.Items[i].IntensityWay;
                elementarray[desti].InflueceCoefficientNumber = 0;
                elementarray[desti].InfluenceElements = elementlist.Items[i].SInfluenceElements;
                elementarray[desti].ReferenceElements = elementlist.Items[i].SReferenceElements;
                elementarray[desti].InfluenceCoefficientList = new double[ArithDllInterface.MAXINFLUENCECOEFFICIENT];
                for (int j = 0; j < elementarray[desti].InfluenceCoefficientList.Length && j < elementlist.Items[i].InfluenceCoefficientLists.Length; j++)
                {
                    elementarray[desti].InfluenceCoefficientList[j] = elementlist.Items[i].InfluenceCoefficientLists[j];
                    elementarray[desti].InflueceCoefficientNumber++;
                }
                elementarray[desti].ConditionCode = elementlist.Items[i].ConditionID;
                elementarray[desti].CalculationMod = (int)elementlist.Items[i].CalculationWay;
                elementarray[desti].K0 = elementlist.Items[i].K0;
                elementarray[desti].K1 = elementlist.Items[i].K1;
                elementarray[desti].Error = 0;
                elementarray[desti].Intensity = elementlist.Items[i].Intensity;
                elementarray[desti].LayerDensity = elementlist.Items[i].LayerDensity;
                elementarray[desti].LayerNumber = elementlist.Items[i].LayerNumber;
                //elementarray[desti].LayerMod = (int)elementlist.Items[i].LayerFlag;
                elementarray[desti].Thickness = 0;//(int)elementlist.Items[i].Thickness
                //elementarray[desti].ThicknessMod = (int)elementlist.Items[i].ThicknessUnit;
                elementarray[i].ElementMod = (int)elementlist.Items[i].Flag;
            }
            ArithDllInterface.SetElementListConfigure(elementarray, elementarray.Length, false, 0.0, elementlist.RhIsMainElementInfluence);
            ArithDllInterface.SetThCurveConfigure(sampledatas, sampledatas.Length);
            int errCode = ArithDllInterface.CAPIERR_NOERROR;
            if (!ArithDllInterface.CaculateThick(ref errCode, elementarray, isAbsorption, intCalculationWay, dblLimit))
            {
                ArithDllInterface.MessageCatch(errCode);
                return false;
            }
            for (int i = 0; i < elementarray.Length; i++)
            {
                int sourcei = elementlist.Items[i].LayerNumber - 1;
                elementlist.Items[i].Thickness = elementarray[sourcei].Thickness;
                double[] InfluenceCoefficients = new double[elementarray[sourcei].InflueceCoefficientNumber];
                for (int j = 0; j < elementlist.Items[i].InfluenceCoefficientLists.Length; j++)
                {
                    InfluenceCoefficients[j] = elementarray[sourcei].InfluenceCoefficientList[j];
                }
                elementlist.Items[i].SInfluenceCoefficients = Helper.ToStrs(InfluenceCoefficients);
            }
            return true;
        }
        /// <summary>
        /// 测厚的计算系数
        /// </summary>
        /// <param name="elementlist">元素列表</param>
        /// <param name="standards">标样数据</param>
        /// <param name="isAbsorption">是否自吸收</param>
        /// <param name="intCalculationWay">厚度就算方法</param>
        /// <param name="dblLimit">极限值</param>
        /// <returns></returns>
        public bool GetThickInfectedCoefs(ElementList elementlist, bool isAbsorption, ThCalculationWay intCalculationWay, double dblLimit)
        {
            int standardLen = 0;
            for (int i = 0; i < elementlist.Items.Count; i++)
            {
                standardLen += elementlist.Items[i].Samples.Count;
            }
            ThSampleRecord[] sampledatas = new ThSampleRecord[standardLen];
            int count = 0;
            for (int i = 0; i < elementlist.Items.Count; i++)
            {
                for (int j = 0; j < elementlist.Items[i].Samples.Count; j++)
                {
                    sampledatas[count].SampleCaption = elementlist.Items[i].Samples[j].SampleName;
                    sampledatas[count].ElementCaption = elementlist.Items[i].Samples[j].ElementName;
                    sampledatas[count].Layer = elementlist.Items[i].Samples[j].Layer;
                    sampledatas[count].TotalLayer = elementlist.Items[i].Samples[j].TotalLayer;
                    sampledatas[count].XValue = double.Parse(elementlist.Items[i].Samples[j].X);
                    sampledatas[count].YValue = double.Parse(elementlist.Items[i].Samples[j].Z);
                    count++;
                }

            }
            if (sampledatas.Length <= 0)
            {
                return false;
            }
            Element[] elementarray = new Element[elementlist.Items.Count];
            //List<CurveElement> tempList = elementlist.Items.ToList<CurveElement>();
            //IOrderedEnumerable<CurveElement> listOrder = tempList.OrderBy(w => w.LayerNumber);
            //int count = 0;
            //foreach (var element in listOrder)
            //{
            //    //排序
            //    elementarray[count].Caption = element.Caption;
            //    elementarray[count].SpectrumData = new double[ArithDllInterface.MAXSPECTRUMDATALENGTH];
            //    int[] spec = Helper.ToInts(element.SSpectrumData);
            //    elementarray[count].SpectrumDataLen = spec.Length;
            //    for (int k = 0; k < elementarray[count].SpectrumDataLen; k++)
            //    {
            //        if (k < spec.Length)
            //        {
            //            elementarray[count].SpectrumData[k] = spec[k];
            //        }
            //        else elementarray[count].SpectrumData[k] = 0;
            //    }
            //    elementarray[count].AtomicNumber = element.AtomicNumber;
            //    elementarray[count].AnalyteLine = (int)element.AnalyteLine;
            //    elementarray[count].BaseHigh = element.BaseHigh;
            //    elementarray[count].BaseLow = element.BaseLow;
            //    elementarray[count].PeakLow = element.PeakLow;
            //    elementarray[count].PeakHigh = element.PeakHigh;
            //    elementarray[count].IsPeakDivBase = element.PeakDivBase;
            //    elementarray[count].IntentsityMod = (int)element.IntensityWay;
            //    elementarray[count].InflueceCoefficientNumber = 0;
            //    elementarray[count].InfluenceElements = element.SInfluenceElements;
            //    elementarray[count].ReferenceElements =element.SReferenceElements;
            //    elementarray[count].InfluenceCoefficientList = new double[ArithDllInterface.MAXINFLUENCECOEFFICIENT];
            //    for (int j = 0; j < elementarray[count].InfluenceCoefficientList.Length && j < element.InfluenceCoefficientLists.Length; j++)
            //    {
            //        elementarray[count].InfluenceCoefficientList[j] = element.InfluenceCoefficientLists[j];
            //        elementarray[count].InflueceCoefficientNumber++;
            //    }
            //    elementarray[count].ConditionCode = element.ConditionID;
            //    elementarray[count].CalculationMod = (int)element.CalculationWay;
            //    elementarray[count].K0 = element.K0;
            //    elementarray[count].K1 = element.K1;
            //    elementarray[count].Error = element.Error;
            //    elementarray[count].Intensity = element.Intensity;
            //    elementarray[count].LayerDensity = element.LayerDensity;
            //    elementarray[count].LayerNumber = element.LayerNumber;
            //    elementarray[count].LayerMod = (int)element.LayerFlag;
            //    elementarray[count].Thickness = element.Thickness;
            //    elementarray[count].ThicknessMod = (int)element.ThicknessUnit;
            //    count++;

            //}
            for (int i = 0; i < elementarray.Length; i++)
            {
                //排序
                int desti = elementlist.Items[i].LayerNumber - 1;
                elementarray[desti].Caption = elementlist.Items[i].Caption;
                elementarray[desti].SpectrumData = new double[ArithDllInterface.MAXSPECTRUMDATALENGTH];
                double[] spec = Helper.ToDoubles(elementlist.Items[i].SSpectrumData);
                elementarray[desti].SpectrumDataLen = spec.Length;
                for (int k = 0; k < elementarray[i].SpectrumDataLen; k++)
                {
                    if (k < spec.Length)
                    {
                        elementarray[desti].SpectrumData[k] = spec[k];
                    }
                    else elementarray[desti].SpectrumData[k] = 0;
                }
                elementarray[desti].AtomicNumber = elementlist.Items[i].AtomicNumber;
                elementarray[desti].AnalyteLine = (int)elementlist.Items[i].AnalyteLine;
                elementarray[desti].BaseHigh = elementlist.Items[i].BaseHigh;
                elementarray[desti].BaseLow = elementlist.Items[i].BaseLow;
                elementarray[desti].PeakLow = elementlist.Items[i].PeakLow;
                elementarray[desti].PeakHigh = elementlist.Items[i].PeakHigh;
                elementarray[desti].IsPeakDivBase = elementlist.Items[i].PeakDivBase;
                elementarray[desti].IntentsityMod = (int)elementlist.Items[i].IntensityWay;
                elementarray[desti].InflueceCoefficientNumber = 0;
                elementarray[desti].InfluenceElements = elementlist.Items[i].SInfluenceElements;
                elementarray[desti].ReferenceElements = elementlist.Items[i].SReferenceElements;
                elementarray[desti].InfluenceCoefficientList = new double[ArithDllInterface.MAXINFLUENCECOEFFICIENT];
                for (int j = 0; j < elementarray[desti].InfluenceCoefficientList.Length ; j++)//&& j < elementlist.Items[i].InfluenceCoefficientLists.Length
                {
                    elementarray[desti].InfluenceCoefficientList[j] = 0;
                    //elementarray[desti].InflueceCoefficientNumber++;
                }
                elementarray[desti].InflueceCoefficientNumber=0;
                elementarray[desti].ConditionCode = elementlist.Items[i].ConditionID;
                elementarray[desti].CalculationMod = (int)elementlist.Items[i].CalculationWay;
                elementarray[desti].K0 = elementlist.Items[i].K0;
                elementarray[desti].K1 = elementlist.Items[i].K1;
                //elementarray[desti].Error = elementlist.Items[i].Error;
                elementarray[desti].Intensity = elementlist.Items[i].Intensity;
                elementarray[desti].Error = 0;
                //elementarray[desti].Intensity = 0;
                elementarray[desti].LayerDensity = elementlist.Items[i].LayerDensity;
                elementarray[desti].LayerNumber = elementlist.Items[i].LayerNumber;
                //elementarray[desti].LayerMod = (int)elementlist.Items[i].LayerFlag;
                //elementarray[desti].Thickness = (int)elementlist.Items[i].Thickness;
                elementarray[desti].Thickness = 0;
               //elementarray[desti].ThicknessMod = (int)elementlist.Items[i].ThicknessUnit;
                elementarray[i].ElementMod = (int)elementlist.Items[i].Flag;
            }
            ArithDllInterface.SetElementListConfigure(elementarray, elementarray.Length, false, 0.0, elementlist.RhIsMainElementInfluence);
            ArithDllInterface.SetThCurveConfigure(sampledatas, sampledatas.Length);
            int errCode = ArithDllInterface.CAPIERR_NOERROR;
            if (!ArithDllInterface.CaculateThickInfectedCoefs(ref errCode, elementarray, isAbsorption, intCalculationWay, dblLimit))
            {
                ArithDllInterface.MessageCatch(errCode);
                return false;
            }
            for (int i = 0; i < elementarray.Length; i++)
            {
                int sourcei = elementlist.Items[i].LayerNumber - 1;
                double[] InfluenceCoefficients = new double[elementarray[sourcei].InflueceCoefficientNumber];
                for (int j = 0; j < InfluenceCoefficients.Length; j++)
                {
                    InfluenceCoefficients[j] = elementarray[sourcei].InfluenceCoefficientList[j];
                    if (double.IsNaN(InfluenceCoefficients[j]))
                    {
                        throw new Exception(LibraryInfo.CAPIERR_RESULTOVERLIMIT);
                    }
                }
                elementlist.Items[i].SInfluenceCoefficients = Helper.ToStrs(InfluenceCoefficients);
            }
            return true;
        }
    }
}
