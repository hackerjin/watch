using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;
using System.Drawing;
using Lephone.Data.Common;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Skyray.EDXRFLibrary.Define;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.UC
{
    /// <summary>
    /// 整个框架中的全局变量
    /// </summary>
    public class DifferenceDevice
    {
        public static UCCameraControl CurCameraControl = null;

        public static string ChinawareFileName = "";

        public static int MaxCount = 3;

        public static double StumerValue = 0.95;

        public static bool ConfigUI = true;

        public static ExceptClientGrobal uc = new ExceptClientGrobal();

        public static TitleIco TitleIco;

        public static bool Spin = false;

        /// <summary>
        /// 全局是否为Rohs
        /// </summary>
        public static bool IsRohs;

        /// <summary>
        /// 全局是否为XRF
        /// </summary>
        public static bool IsXRF;

        public static bool IsConnect;//控制摄像头进行中，当滤光片等一起在移动是，进行添加点

        /// <summary>
        /// 全局是否为Thick
        /// </summary>
        public static bool IsThick;

        public static bool IsAnalyser;

        /// <summary>
        /// 默认左右边界与中间道的距离
        /// </summary>
        public static int PeakLowHightSpacing = 10;

        public static int DefaultMaxConditionParamterCount = 5;

        public static InterfaceClass interClassMain;



        public static IMediumAccess MediumAccess = new MediumAccess();
        //public static Skyray.UC.Common.IntensityCorrect IntensityCorrect = new Skyray.UC.Common.IntensityCorrect();

        public static IRohsAccess irohs;
        public static IThickAccess ithick;
        public static IEDXRF iEdxrf;

        /// <summary>
        /// 全局是否为Match
        /// </summary>
        public static bool IsMatch;

        /// <summary>
        /// 匹配时间
        /// </summary>
        public static int MatchTime;
        /// <summary>
        /// 分析时间
        /// </summary>
        public static int AnalysisTimes = 1;

        /// <summary>
        /// 丢包时间
        /// </summary>
        public static int DropDataTime;
        /// <summary>
        /// 是否开盖
        /// </summary>
        public static bool IsOpenCover;
        /// <summary>
        /// 最大层数
        /// </summary>
        public static int MAX_LAYER_NUMBER_INT = 6;
        /// <summary>
        /// 强度计算方法
        /// </summary>
        public static object[] IntensityWay;
        /// <summary>
        /// 厚度层
        /// </summary>
        public static string[] LayerName = new string[] { Info.Substrate, Info.FirstLayer, Info.SecondLayer, Info.ThirdLayer, Info.ForthLayer, Info.FifthLayer };

        public static Color DefaultSpecColor = Color.Blue;

        public static double[] param;

        public static string[] gradeNTName;

        public static int gradeNTNum;

        public static int MatchNum;

        public static bool Specification = false;

        public static bool ReportChinaWare = false;

        public static bool Refresh = false;

        public static bool IsExitUsb = false;

        public static bool IsAutoSaveReport = false;//是否进行自动保存报表功能


        public static bool IsShowWeight = false;
        public static bool IsShowKarat = false;
        /// <summary>
        /// 根据确定的软件类型实例化对象及软件功能
        /// </summary>
        /// <returns></returns>
        public static InterfaceClass CreateInstance()
        {

            InterfaceClass interClass = null;
            if (IsThick)
            {
                interClass = new Thick();
                WorkCurveHelper.curThick = interClass;
                ithick = new ThickAccess((Thick)interClass);
                WorkCurveHelper.DeviceFunctype = FuncType.Thick;
            }
            interClassMain = interClass;
            return interClass;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="workCurve"></param>
        /// <param name="Hegiht"></param>
        /// <param name="isRcalcSample">是否重新计算标样强度</param>
        public static void TransHeightPure(WorkCurve workCurve, double Hegiht, bool isRcalcSample, SpecListEntity templist, int setCurrent)
        {
            if (!IsThick || workCurve == null || workCurve.ElementList == null || workCurve.ElementList.Items.Count <= 0) return;
            //WorkCurveHelper.PureSpecListEntiry.Clear();
            foreach (var element in workCurve.ElementList.Items)
            {
                // if (element.ElementSpecName != "")//改变纯元素谱数据
                if (element.ElementEncoderSpecName != "")//改变纯元素谱数据
                {
                    //List<PureSpecParam> pureList = PureSpecParam.FindBySql("select * from PureSpecParam where samplename ='" + element.Caption + "' and condition_id = " + workCurve.Condition.Id + " and SpecTypeValue =1" + " order by height");
                    List<PureSpecParam> pureList = PureSpecParam.FindBySql("select * from PureSpecParam where ElementName ='" + element.ElementEncoderSpecName + "' and condition_id = " + workCurve.Condition.Id + " and SpecTypeValue =1" + " order by height");

                    if (pureList == null || pureList.Count <= 0) break;
                    if (pureList.Count < 3) break;

                    double[] coeff = new double[4];
                    PointF[] pointValues = new PointF[pureList.Count];
                    for (int i = 0; i < pureList.Count; i++)
                    {
                        pointValues[i].X = (float)pureList[i].Height;
                        pointValues[i].Y = (float)pureList[i].TotalCount; //这里totalcount是计数率不是总计数
                    }
                    CalSquareParam(pointValues, coeff, 2, false); //二次计算
                    SqlParams params0 = new SqlParams("Name", pureList[0].Name, false);
                    List<SpecListEntity> lstSpecListmin = WorkCurveHelper.DataAccess.Query(new SqlParams[] { params0 });
                    double CalcTotalCount = coeff[0] * Hegiht * Hegiht + coeff[1] * Hegiht + coeff[2];
                    int[] data = new int[lstSpecListmin[0].Specs[0].SpecDatac.Length];
                    int[] datas = new int[lstSpecListmin[0].Specs[0].SpecDatac.Length];
                    if (lstSpecListmin.Count > 0)
                    {
                        data = lstSpecListmin[0].Specs[0].SpecDatac;
                        datas = lstSpecListmin[0].Specs[0].SpecDatas;
                    }

                    int[] RecoverSpec = new int[data.Length];
                    double calcRev;
                    if (WorkCurveHelper.IsPureElemCurrentUnify)   //管流要归一   
                        calcRev = (double)CalcTotalCount / (lstSpecListmin[0].CountRate / lstSpecListmin[0].ActualCurrent);
                    else
                        calcRev = (double)CalcTotalCount / lstSpecListmin[0].CountRate;
                    int totbal = 0;
                    for (int i = 0; i < data.Length; i++)
                    {
                        RecoverSpec[i] = (int)(Math.Round(data[i] * calcRev));

                        // RecoverSpec[i] = (int)(Math.Round(data[i] / lstSpecListmin[0].ActualCurrent * templist.ActualCurrent * calcRev));
                        totbal += RecoverSpec[i];
                        //纯元素谱数据 / 管流 * 未知样谱的管流    20211221                                //纯元素谱实际管流                 //当前测试的未知样的管流 
                        // RecoverSpec[i] = (int)(Math.Round(data[i] * calcRev / lstSpecListmin[0].ActualCurrent * setCurrent));
                    }
                    Console.WriteLine(totbal.ToString());
                    //element.SSpectrumData = Helper.TransforToDivTime(Helper.ToStrs(RecoverSpec), lstSpecListmin[0].Specs[0].UsedTime);
                    element.SSpectrumData = Helper.TransforToDivTimeAndCurrent(Helper.ToStrs(RecoverSpec), lstSpecListmin[0].Specs[0].UsedTime, lstSpecListmin[0].ActualCurrent);
                    // element.SSpectrumData = Helper.TransforToDivTimeAndCurrent(Helper.ToStrs(RecoverSpec), lstSpecListmin[0].Specs[0].UsedTime, templist.ActualCurrent);


                }

                if (isRcalcSample)
                {
                    foreach (var sample in element.Samples)
                    {
                        if (element.LayerNumBackUp == "基材") continue;
                        // SqlParams params0 = new SqlParams("SpecTypeValue", "2", true);
                        SqlParams params1 = new SqlParams("Name", sample.SampleName, false);
                        List<SpecListEntity> lstSpecList = WorkCurveHelper.DataAccess.Query(new SqlParams[] { params1 });
                        if (lstSpecList.Count > 0)
                        {
                            try
                            {
                                workCurve.CaculateIntensity(lstSpecList[0]);
                            }
                            catch (Exception ex)
                            {
                                Msg.Show(lstSpecList[0].SampleName + ex.Message);
                                continue;
                            }
                            sample.X = element.Intensity.ToString();
                            //  double[] coeff = new double[3] { 0.0115, -2.0047, 101.48 };

                            //double[] coeff = new double[3] { 0.1904, -11.898, 335.19 };
                            //double[] coff9 = new double[3] { 0.1057, -16.444, 769.48 }; 
                            //double intensity = element.Intensity;
                            //if (sample.SampleName.Contains("Ni2.29Fe"))
                            //sample.X = (coeff[0] * Hegiht * Hegiht + coeff[1] * Hegiht + coeff[2]).ToString();
                            //else
                            //    sample.X = (coff9[0] * Hegiht * Hegiht + coff9[1] * Hegiht + coff9[2]).ToString();
                            //Console.WriteLine(sample.X);
                        }
                    }

                }

            }

        }


        public static void TransHeightPureZero(WorkCurve workCurve, double Hegiht, bool isRcalcSample, SpecListEntity templist, int setCurrent)
        {
            if (!IsThick || workCurve == null || workCurve.ElementList == null || workCurve.ElementList.Items.Count <= 0) return;
            bool isAdjust = WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList[0].IsAdjustRate;
            foreach (var element in workCurve.ElementList.Items)
            {
                if (element.ElementSpecName != "")//改变纯元素谱数据
                {
                    List<PureSpecParam> pureList = PureSpecParam.FindBySql("select * from PureSpecParam where samplename ='" + element.Caption + "' " + " and SpecTypeValue =1" + " and DeviceName ='" + workCurve.Condition.Device.Name + "' order by height");
                    if (pureList == null || pureList.Count <= 0) break;
                    if (pureList.Count < 3) break;
                    double[] coeff = new double[4];
                    int dim = 2;
                    if (WorkCurveHelper.PureCalcType == 3)
                        dim = 3;
                    PointF[] pointValues = new PointF[pureList.Count];
                    for (int i = 0; i < pureList.Count; i++)
                    {
                        pointValues[i].X = (float)pureList[i].Height;
                        if (isAdjust)
                            pointValues[i].Y = (float)Math.Round(pureList[i].CurrentUnifyCount, 6);
                        else
                            pointValues[i].Y = (float)Math.Round(pureList[i].TotalCount, 6);
                    }
                    CalSquareParam(pointValues, coeff, dim, false); //二次计算,需要得到coeff[2] ,即零点位的值
                    //零点totalcount
                    double ZeroTotalCount = coeff[3];

                    //// 未知样totalcountt系数
                    //double CalcTotalCount = coeff[0] * Hegiht * Hegiht + coeff[1] * Hegiht + coeff[2];


                    //未知样系数
                    //double UnKnowLine = CalcTotalCount / ZeroTotalCount;

                    //使用高度与（total与零点totao的比值）二次计算
                    int SumCount = pureList.Count + 1;
                    PointF[] pointLineValues = new PointF[SumCount];
                    pointLineValues[0].X = 0;
                    pointLineValues[0].Y = 1;
                    double[] coeffLine = new double[4];
                    for (int m = 0; m < pureList.Count; m++)
                    {
                        pointLineValues[m + 1].X = (float)pureList[m].Height;
                        if (isAdjust)
                            pointLineValues[m + 1].Y = (float)Math.Round(pureList[m].CurrentUnifyCount / ZeroTotalCount, 6); //这里totalcount是计数率不是总计数
                        else
                            pointLineValues[m + 1].Y = (float)Math.Round(pureList[m].TotalCount / ZeroTotalCount, 6);
                    }
                    CalSquareParam(pointLineValues, coeffLine, dim, false); //二次计算,得到与零点比值的值与高度的公式

                    SqlParams params0 = new SqlParams("Name", element.ElementSpecName, false);
                    List<SpecListEntity> lstSpecListmin = WorkCurveHelper.DataAccess.Query(new SqlParams[] { params0 });
                    double minhegiht = lstSpecListmin[0].Height;
                    double minheightline = 0;
                    if (dim == 2)
                        minheightline = coeffLine[1] * minhegiht * minhegiht + coeffLine[2] * minhegiht + coeffLine[3];
                    else
                        minheightline = coeffLine[0] * minhegiht * minhegiht * minhegiht + coeffLine[1] * minhegiht * minhegiht + coeffLine[2] * minhegiht + coeffLine[3];

                    double minTotalCount = 0;
                    if (isAdjust)
                    {
                        minTotalCount = lstSpecListmin[0].CountRate / lstSpecListmin[0].ActualCurrent;
                    }
                    else
                    {
                        minTotalCount = lstSpecListmin[0].CountRate;
                    }

                    //当前纯元素高度为0时的计数
                    double zeroTotalCount = minTotalCount / minheightline;
                    int[] zerodatas = new int[lstSpecListmin[0].Specs[0].SpecDatac.Length];

                    int[] data = new int[lstSpecListmin[0].Specs[0].SpecDatac.Length];
                    int[] datas = new int[lstSpecListmin[0].Specs[0].SpecDatac.Length];
                    if (lstSpecListmin.Count > 0)
                    {
                        data = lstSpecListmin[0].Specs[0].SpecDatac;
                        datas = lstSpecListmin[0].Specs[0].SpecDatas;
                    }

                    for (int i = 0; i < zerodatas.Length; i++)
                    {
                        zerodatas[i] = (int)Math.Round(data[i] / minheightline);
                    }

                    //未知的totalcount 
                    double unknowHegihtLine = 0;
                    if (dim == 2)
                        unknowHegihtLine = coeffLine[1] * Hegiht * Hegiht + coeffLine[2] * Hegiht + coeffLine[3];
                    else
                        unknowHegihtLine = coeffLine[0] * Hegiht * Hegiht * Hegiht + coeffLine[1] * Hegiht * Hegiht + coeffLine[2] * Hegiht + coeffLine[3];

                    //double unknowHegihtLine = coeffLine[0] * Hegiht * Hegiht + coeffLine[1] * Hegiht + coeffLine[2];
                    double unKnowTotalCount = zeroTotalCount * unknowHegihtLine;
                    double calcRev = 0; ;
                    int[] RecoverSpec = new int[data.Length];

                    for (int i = 0; i < data.Length; i++)
                    {
                        RecoverSpec[i] = (int)(Math.Round(zerodatas[i] * unknowHegihtLine * WorkCurveHelper.PureAdjustCoeff));
                    }

                    // element.SSpectrumData = Helper.TransforToDivTimeAndCurrent(Helper.ToStrs(RecoverSpec), lstSpecListmin[0].Specs[0].UsedTime, templist.ActualCurrent);
                    element.SSpectrumData = Helper.TransforToDivTimeAndCurrent(Helper.ToStrs(RecoverSpec), lstSpecListmin[0].Specs[0].UsedTime, lstSpecListmin[0].ActualCurrent);
                }

                //if (isRcalcSample)
                //{
                //    foreach (var sample in element.Samples)
                //    {
                //        if (element.LayerNumBackUp == "基材") continue;
                //        // SqlParams params0 = new SqlParams("SpecTypeValue", "2", true);
                //        SqlParams params1 = new SqlParams("Name", sample.SampleName, false);
                //        List<SpecListEntity> lstSpecList = WorkCurveHelper.DataAccess.Query(new SqlParams[] { params1 });
                //        if (lstSpecList.Count > 0)
                //        {
                //            try
                //            {
                //                workCurve.CaculateIntensity(lstSpecList[0]);
                //            }
                //            catch (Exception ex)
                //            {
                //                Msg.Show(lstSpecList[0].SampleName + ex.Message);
                //                continue;
                //            }
                //            sample.X = element.Intensity.ToString();
                //            //  double[] coeff = new double[3] { 0.0115, -2.0047, 101.48 };

                //            //double[] coeff = new double[3] { 0.1904, -11.898, 335.19 };
                //            //double[] coff9 = new double[3] { 0.1057, -16.444, 769.48 }; 
                //            //double intensity = element.Intensity;
                //            //if (sample.SampleName.Contains("Ni2.29Fe"))
                //            //sample.X = (coeff[0] * Hegiht * Hegiht + coeff[1] * Hegiht + coeff[2]).ToString();
                //            //else
                //            //    sample.X = (coff9[0] * Hegiht * Hegiht + coff9[1] * Hegiht + coff9[2]).ToString();
                //            //Console.WriteLine(sample.X);
                //        }
                //    }

                //}

            }

        }

        public static void TransSSpectrumnHeight(string DeviceName, CurveElement element, double Hegiht, bool isElementSpecName, bool IsPureElemCurrentUnify)
        {
            if (!IsThick || element == null) return;
            bool isAdjust = WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList[0].IsAdjustRate;

            string pureSampleName = isElementSpecName ? element.ElementSpecName : element.ElementSpecNameNoFilter;
            if (pureSampleName != "")//改变纯元素谱数据
            {
                List<PureSpecParam> pureList = PureSpecParam.FindBySql("select * from PureSpecParam where samplename ='" + element.Caption + "' " + " and SpecTypeValue =1" + " and DeviceName ='" + DeviceName + "' order by height");
                if (pureList == null || pureList.Count <= 0) return;
                if (pureList.Count < 3) return;
                double[] coeff = new double[4];
                SqlParams params0 = new SqlParams("Name", pureSampleName, false);
                List<SpecListEntity> lstSpecListmin = WorkCurveHelper.DataAccess.Query(new SqlParams[] { params0 });
                int[] data = new int[lstSpecListmin[0].Specs[0].SpecDatac.Length];
                int[] datas = new int[lstSpecListmin[0].Specs[0].SpecDatac.Length];
                int[] RecoverSpec = new int[data.Length];

                int dim = 2;
                if (WorkCurveHelper.PureCalcType == 3)
                    dim = 3;
                PointF[] pointValues = new PointF[pureList.Count];
                for (int i = 0; i < pureList.Count; i++)
                {
                    pointValues[i].X = (float)pureList[i].Height;
                    if (isAdjust)
                        pointValues[i].Y = (float)Math.Round(pureList[i].CurrentUnifyCount, 6);
                    else
                        pointValues[i].Y = (float)Math.Round(pureList[i].TotalCount, 6);
                }
                CalSquareParam(pointValues, coeff, dim, false); //二次计算,需要得到coeff[2] ,即零点位的值

                if (IsPureElemCurrentUnify)
                {
                    double ZeroTotalCount = coeff[3];
                    int SumCount = pureList.Count + 1;
                    PointF[] pointLineValues = new PointF[SumCount];
                    pointLineValues[0].X = 0;
                    pointLineValues[0].Y = 1;
                    double[] coeffLine = new double[4];
                    for (int m = 0; m < pureList.Count; m++)
                    {
                        pointLineValues[m + 1].X = (float)pureList[m].Height;
                        if (isAdjust)
                            pointLineValues[m + 1].Y = (float)Math.Round(pureList[m].CurrentUnifyCount / ZeroTotalCount, 6); //这里totalcount是计数率不是总计数
                        else
                            pointLineValues[m + 1].Y = (float)Math.Round(pureList[m].TotalCount / ZeroTotalCount, 6);
                    }
                    CalSquareParam(pointLineValues, coeffLine, dim, false); //二次计算,得到与零点比值的值与高度的公式


                    double minhegiht = lstSpecListmin[0].Height;
                    double minheightline = 0;
                    if (dim == 2)
                        minheightline = coeffLine[1] * minhegiht * minhegiht + coeffLine[2] * minhegiht + coeffLine[3];
                    else
                        minheightline = coeffLine[0] * minhegiht * minhegiht * minhegiht + coeffLine[1] * minhegiht * minhegiht + coeffLine[2] * minhegiht + coeffLine[3];

                    double minTotalCount = 0;
                    if (isAdjust)
                    {
                        minTotalCount = lstSpecListmin[0].CountRate / lstSpecListmin[0].ActualCurrent;
                    }
                    else
                    {
                        minTotalCount = lstSpecListmin[0].CountRate;
                    }

                    //当前纯元素高度为0时的计数
                    double zeroTotalCount = minTotalCount / minheightline;
                    int[] zerodatas = new int[lstSpecListmin[0].Specs[0].SpecDatac.Length];


                    if (lstSpecListmin.Count > 0)
                    {
                        data = lstSpecListmin[0].Specs[0].SpecDatac;
                        datas = lstSpecListmin[0].Specs[0].SpecDatas;
                    }
                    for (int i = 0; i < zerodatas.Length; i++)
                    {
                        zerodatas[i] = (int)Math.Round(data[i] / minheightline);
                    }
                    //未知的totalcount 
                    double unknowHegihtLine = 0;
                    if (dim == 2)
                        unknowHegihtLine = coeffLine[1] * Hegiht * Hegiht + coeffLine[2] * Hegiht + coeffLine[3];
                    else
                        unknowHegihtLine = coeffLine[0] * Hegiht * Hegiht * Hegiht + coeffLine[1] * Hegiht * Hegiht + coeffLine[2] * Hegiht + coeffLine[3];

                    //double unknowHegihtLine = coeffLine[0] * Hegiht * Hegiht + coeffLine[1] * Hegiht + coeffLine[2];
                    double unKnowTotalCount = zeroTotalCount * unknowHegihtLine;
                    double calcRev = 0; ;

                    for (int i = 0; i < data.Length; i++)
                    {
                        RecoverSpec[i] = (int)(Math.Round(zerodatas[i] * unknowHegihtLine * WorkCurveHelper.PureAdjustCoeff));
                    }
                }
                else
                {
                    double CalcTotalCount = 0;
                    if (WorkCurveHelper.PureCalcType == 2)
                        CalcTotalCount = coeff[1] * Hegiht * Hegiht + coeff[2] * Hegiht + coeff[3];
                    else if (WorkCurveHelper.PureCalcType == 3)
                        CalcTotalCount = coeff[0] * Hegiht * Hegiht * Hegiht + coeff[1] * Hegiht * Hegiht + coeff[2] * Hegiht + coeff[3];

                    if (lstSpecListmin.Count > 0)
                    {
                        data = lstSpecListmin[0].Specs[0].SpecDatac;
                        datas = lstSpecListmin[0].Specs[0].SpecDatas;
                    }
                    double calcRev;
                    if (isAdjust)
                        calcRev = (double)CalcTotalCount / (lstSpecListmin[0].CountRate / lstSpecListmin[0].ActualCurrent);
                    else
                        calcRev = (double)CalcTotalCount / lstSpecListmin[0].CountRate;

                    for (int i = 0; i < data.Length; i++)
                    {
                        RecoverSpec[i] = (int)(Math.Round(data[i] * calcRev * WorkCurveHelper.PureAdjustCoeff));
                    }
                }
                element.SSpectrumData = Helper.TransforToDivTimeAndCurrent(Helper.ToStrs(RecoverSpec), lstSpecListmin[0].Specs[0].UsedTime, lstSpecListmin[0].ActualCurrent);
            }

        }


        public static void TransHeightPureByZero(WorkCurve workCurve, double Hegiht, bool isRcalcSample, SpecListEntity templist, int setCurrent)
        {
            if (!IsThick || workCurve == null || workCurve.ElementList == null || workCurve.ElementList.Items.Count <= 0) return;
            //WorkCurveHelper.PureSpecListEntiry.Clear();
            bool isAdjust = WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList[0].IsAdjustRate;
            foreach (var element in workCurve.ElementList.Items)
            {
                // if (element.ElementSpecName != "")//改变纯元素谱数据
                if (element.ElementSpecName != "")//改变纯元素谱数据
                {
                    //List<PureSpecParam> pureList = PureSpecParam.FindBySql("select * from PureSpecParam where samplename ='" + element.Caption + "' and condition_id = " + workCurve.Condition.Id + " and SpecTypeValue =1" + " order by height");
                    List<PureSpecParam> pureList = PureSpecParam.FindBySql("select * from PureSpecParam where ElementName ='" + element.Caption + "'  and SpecTypeValue =1" + " order by height");

                    if (pureList == null || pureList.Count <= 0) break;
                    if (pureList.Count < 3) break;
                    double[] coeff = new double[4];

                    PointF[] pointValues = new PointF[pureList.Count];
                    for (int i = 0; i < pureList.Count; i++)
                    {
                        pointValues[i].X = (float)pureList[i].Height;
                        if (isAdjust)
                            pointValues[i].Y = (float)pureList[i].CurrentUnifyCount; //这里totalcount是计数率不是总计数
                        else
                            pointValues[i].Y = (float)pureList[i].TotalCount; //这里totalcount是计数率不是总计数
                    }
                    int dim = 2;
                    if (WorkCurveHelper.PureCalcType == 3)
                        dim = 3;
                    CalSquareParam(pointValues, coeff, dim, false); //二次计算
                    SqlParams params0 = new SqlParams("Name", element.ElementSpecName, false);
                    List<SpecListEntity> lstSpecListmin = WorkCurveHelper.DataAccess.Query(new SqlParams[] { params0 });
                    double CalcTotalCount = 0;
                    //if (WorkCurveHelper.PureCalcType == 2)
                    //    CalcTotalCount = coeff[0] * Hegiht * Hegiht * WorkCurveHelper.PureAdjustCoeff + coeff[1] * Hegiht * WorkCurveHelper.PureAdjustCoeff + coeff[2] * WorkCurveHelper.PureAdjustCoeff;
                    //else if (WorkCurveHelper.PureCalcType == 3)
                    //    CalcTotalCount = coeff[0] * Hegiht * Hegiht * Hegiht * WorkCurveHelper.PureAdjustCoeff + coeff[1] * Hegiht * Hegiht * WorkCurveHelper.PureAdjustCoeff + coeff[2] * Hegiht * WorkCurveHelper.PureAdjustCoeff + coeff[3] * WorkCurveHelper.PureAdjustCoeff;
                    if (WorkCurveHelper.PureCalcType == 2)
                        CalcTotalCount = coeff[1] * Hegiht * Hegiht + coeff[2] * Hegiht + coeff[3];
                    else if (WorkCurveHelper.PureCalcType == 3)
                        CalcTotalCount = coeff[0] * Hegiht * Hegiht * Hegiht + coeff[1] * Hegiht * Hegiht + coeff[2] * Hegiht + coeff[3];
                    int[] data = new int[lstSpecListmin[0].Specs[0].SpecDatac.Length];
                    int[] datas = new int[lstSpecListmin[0].Specs[0].SpecDatac.Length];
                    if (lstSpecListmin.Count > 0)
                    {
                        data = lstSpecListmin[0].Specs[0].SpecDatac;
                        datas = lstSpecListmin[0].Specs[0].SpecDatas;
                    }

                    int[] RecoverSpec = new int[data.Length];
                    double calcRev;
                    //if (WorkCurveHelper.IsPureElemCurrentUnify)   //管流要归一   
                    //    calcRev = (double)CalcTotalCount / (lstSpecListmin[0].CountRate / lstSpecListmin[0].ActualCurrent);
                    //else
                    if (isAdjust)
                        calcRev = (double)CalcTotalCount / (lstSpecListmin[0].CountRate / lstSpecListmin[0].ActualCurrent);
                    else
                        calcRev = (double)CalcTotalCount / lstSpecListmin[0].CountRate;
                    // int totbal = 0;
                    for (int i = 0; i < data.Length; i++)
                    {
                        RecoverSpec[i] = (int)(Math.Round(data[i] * calcRev * WorkCurveHelper.PureAdjustCoeff));

                        // RecoverSpec[i] = (int)(Math.Round(data[i] / lstSpecListmin[0].ActualCurrent * templist.ActualCurrent * calcRev));
                        //totbal += RecoverSpec[i];
                        // Console.WriteLine("total:" + totbal.ToString());
                    }
                    // Console.WriteLine(totbal.ToString());
                    //element.SSpectrumData = Helper.TransforToDivTime(Helper.ToStrs(RecoverSpec), lstSpecListmin[0].Specs[0].UsedTime);
                    //element.SSpectrumData = Helper.TransforToDivTimeAndCurrent(Helper.ToStrs(RecoverSpec), lstSpecListmin[0].Specs[0].UsedTime, lstSpecListmin[0].ActualCurrent);
                    element.SSpectrumData = Helper.TransforToDivTimeAndCurrent(Helper.ToStrs(RecoverSpec), lstSpecListmin[0].Specs[0].UsedTime, lstSpecListmin[0].ActualCurrent);



                }


            }
        }


        public static void TransHeightPureSpecData(WorkCurve workCurve, List<SpecListEntity> specList)
        {
            if (!IsThick || workCurve == null || workCurve.ElementList == null || workCurve.ElementList.Items.Count <= 0 || specList.Count <= 0 || (specList[0].SpecType == SpecType.PureSpec)) return;
            foreach (var templist in specList)
            {
                foreach (var element in workCurve.ElementList.Items)
                {

                    if (element.ElementSpecName != "")//改变纯元素谱数据
                    {
                        List<PureSpecParam> pureList = PureSpecParam.FindBySql("select * from PureSpecParam where ElementName ='" + element.Caption + "' " + " and SpecTypeValue =1" + " order by height");
                        if (pureList == null || pureList.Count <= 1) break;
                        PureSpecParam onePur = pureList.Find(w => w.Height == templist.Height);
                        if (onePur != null)
                        {
                            SqlParams params0 = new SqlParams("SpecTypeValue", "1", true);
                            SqlParams params1 = new SqlParams("Name", onePur.Name, false);
                            List<SpecListEntity> lstSpecList = WorkCurveHelper.DataAccess.Query(new SqlParams[] { params0, params1 });
                            if (lstSpecList.Count > 0)
                            {
                                int[] data = lstSpecList[0].Specs[0].SpecDatac;
                                // element.SSpectrumData = Helper.TransforToDivTime(Helper.ToStrs(data), lstSpecList[0].Specs[0].UsedTime);
                                element.SSpectrumData = Helper.TransforToDivTimeAndCurrent(Helper.ToStrs(data), lstSpecList[0].Specs[0].UsedTime, lstSpecList[0].ActualCurrent);

                            }
                            // element.SSpectrumData = Helper.TransforToDivTime(Helper.ToStrs(onePur.Data), onePur.UsedTime);
                        }
                        else
                        {
                            int index = pureList.Select((d, i) =>
                            {
                                return new
                                {
                                    value = d,
                                    Index = i
                                };
                            }).OrderBy(x => Math.Abs(x.value.Height - templist.Height)).First().Index;

                            int min = 0;
                            int max = 0;
                            if (index == 0)
                            {
                                min = index;
                                max = index + 1;
                            }
                            else if (index == pureList.Count - 1)
                            {
                                min = index - 1;
                                max = index;
                            }
                            else if (pureList[index].Height > templist.Height)
                            {
                                min = index - 1;
                                max = index;
                            }
                            else
                            {
                                min = index;
                                max = index + 1;
                            }

                            /// 找出两条曲线
                            PureSpecParam pmin = pureList[min];
                            PureSpecParam pmax = pureList[max];

                            double ReviseCoef = 1.0;
                            double ReviseK = 0;
                            ReviseCoef = (pmin.TotalCount - pmax.TotalCount) / (pmin.Height - pmax.Height);
                            ReviseK = pmin.TotalCount - (pmin.TotalCount - pmax.TotalCount) / (pmin.Height - pmax.Height) * pmin.Height;
                            ReviseCoef = Double.IsInfinity(ReviseCoef) || Double.IsNaN(ReviseCoef) ? 1.0 : ReviseCoef;
                            ReviseK = Double.IsInfinity(ReviseK) || Double.IsNaN(ReviseK) ? 0 : ReviseK;

                            // SqlParams params0 = new SqlParams("SpecTypeValue", "1", true);
                            SqlParams params0 = new SqlParams("Name", pureList[index].Name, false);
                            List<SpecListEntity> lstSpecListmin = WorkCurveHelper.DataAccess.Query(new SqlParams[] { params0 });
                            double CalcTotalCount = templist.Height * ReviseCoef + ReviseK;
                            int[] data = new int[lstSpecListmin[0].Specs[0].SpecDatac.Length];
                            if (lstSpecListmin.Count > 0)
                            {
                                data = lstSpecListmin[0].Specs[0].SpecDatac;
                                // element.SSpectrumData = Helper.TransforToDivTime(Helper.ToStrs(data), lstSpecListmin[0].Specs[0].UsedTime);
                            }

                            int[] RecoverSpec = new int[data.Length];
                            //  double calcRev = (double)CalcTotalCount / lstSpecListmin[0].CountRate;
                            double calcRev;
                            //if (WorkCurveHelper.IsPureElemCurrentUnify)   //管流要归一   
                            if (WorkCurveHelper.WorkCurveCurrent.Condition.DeviceParamList[0].IsAdjustRate)   //调节计数率的时候归一
                                calcRev = (double)CalcTotalCount / (lstSpecListmin[0].CountRate / lstSpecListmin[0].ActualCurrent);
                            else
                                calcRev = (double)CalcTotalCount / lstSpecListmin[0].CountRate;
                            for (int i = 0; i < data.Length; i++)
                            {
                                RecoverSpec[i] = (int)(Math.Round(data[i] * calcRev));
                                // RecoverSpec[i] = (int)(Math.Round(data[i] / lstSpecListmin[0].ActualCurrent * templist.ActualCurrent * calcRev));
                            }


                            // element.SSpectrumData = Helper.ToStrs(RecoverSpec);
                            // element.SSpectrumData = Helper.TransforToDivTime(Helper.ToStrs(RecoverSpec), lstSpecListmin[0].Specs[0].UsedTime);
                            element.SSpectrumData = Helper.TransforToDivTimeAndCurrent(Helper.ToStrs(RecoverSpec), lstSpecListmin[0].Specs[0].UsedTime, lstSpecListmin[0].ActualCurrent);

                        }
                        //SqlParams params0 = new SqlParams("SpecTypeValue", "1", true);
                        //SqlParams params1 = new SqlParams("Name", element.ElementSpecName, false);
                        //List<SpecListEntity> lstSpecList = WorkCurveHelper.DataAccess.Query(new SqlParams[] { params0, params1 });
                        //if (lstSpecList.Count > 0)
                        //{
                        //    int[] data = lstSpecList[0].Specs[0].SpecDatac;
                        //    element.SSpectrumData = Helper.TransforToDivTime(Helper.ToStrs(data), lstSpecList[0].Specs[0].UsedTime);
                        //}
                    }
                    //foreach (var sample in element.Samples)
                    //{
                    //    SqlParams params0 = new SqlParams("SpecTypeValue", "0", true);
                    //    SqlParams params1 = new SqlParams("Name", sample.SampleName, false);
                    //    List<SpecListEntity> lstSpecList = WorkCurveHelper.DataAccess.Query(new SqlParams[] { params0, params1 });
                    //    if (lstSpecList.Count > 0)
                    //    {
                    //        try
                    //        {
                    //            workCurve.CaculateIntensity(lstSpecList[0]);
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            Msg.Show(lstSpecList[0].SampleName + ex.Message);
                    //            continue;
                    //        }
                    //        sample.X = element.Intensity.ToString();
                    //    }
                    //}
                }
            }
        }





        /// <summary>
        /// 纯元素谱和标样谱管流归一 202112221
        /// </summary>
        /// <param name="workCurve"></param>
        /// <param name="setCurrent"></param>
        public static void TranCurrentAtOne(WorkCurve workCurve, int setCurrent)
        {
            if (!IsThick || workCurve == null || workCurve.ElementList == null || workCurve.ElementList.Items.Count <= 0) return;
            foreach (var element in workCurve.ElementList.Items)
            {
                if (element.ElementSpecName != "")//改变纯元素谱数据
                {
                    SqlParams params0 = new SqlParams("SpecTypeValue", "1", true);
                    SqlParams params1 = new SqlParams("Name", element.ElementSpecName, false);
                    List<SpecListEntity> lstSpecList = WorkCurveHelper.DataAccess.Query(new SqlParams[] { params0, params1 });
                    if (lstSpecList.Count > 0)
                    {
                        int[] data = lstSpecList[0].Specs[0].SpecDatac;
                        int[] RecoverSpec = new int[data.Length];
                        for (int i = 0; i < data.Length; i++)
                        {
                            // RecoverSpec[i] = (int)(Math.Round(data[i] * calcRev));
                            //纯元素谱数据 / 管流 * 未知样谱的管流    20211221                                //纯元素谱实际管流                 //当前测试的未知样的管流 
                            RecoverSpec[i] = (int)(Math.Round(data[i] / lstSpecList[0].ActualCurrent * setCurrent));
                        }

                        //  element.SSpectrumData = Helper.TransforToDivTime(Helper.ToStrs(RecoverSpec), lstSpecList[0].Specs[0].UsedTime);
                        element.SSpectrumData = Helper.TransforToDivTimeAndCurrent(Helper.ToStrs(RecoverSpec), lstSpecList[0].Specs[0].UsedTime, lstSpecList[0].ActualCurrent);

                    }
                }
                if (element.Samples != null && element.Samples.Count > 0)
                {
                    foreach (var sample in element.Samples)
                    {
                        SqlParams params0 = new SqlParams("SpecTypeValue", "0", true);
                        SqlParams params1 = new SqlParams("Name", sample.SampleName, false);
                        List<SpecListEntity> lstSpecList = WorkCurveHelper.DataAccess.Query(new SqlParams[] { params0, params1 });
                        if (lstSpecList.Count > 0)
                        {
                            try
                            {
                                workCurve.CaculateIntensity(lstSpecList[0]);
                            }
                            catch (Exception ex)
                            {
                                Msg.Show(lstSpecList[0].SampleName + ex.Message);
                                continue;
                            }
                            sample.X = element.Intensity.ToString();
                        }
                    }
                }
            }
        }



        public static bool CalSquareParam(PointF[] Points, double[] coeff, int dim, bool IZero)
        {
            double[] abs = new double[Points.Length];
            double[] con = new double[Points.Length];
            for (int i = 0; i < Points.Length; i++)
            {
                abs[i] = Points[i].X;
                con[i] = Points[i].Y;
            }
            try
            {
                CalculateCurve(Points, dim, IZero, coeff);
            }
            catch
            {
                return false;
            }
            if (IZero)
            {
                double a = coeff[0];
                double b = coeff[1];
                coeff[0] = b;
                coeff[1] = a;
            }
            return true;
        }

        public static bool CalculateCurve(PointF[] Points, int dim, bool izero, double[] x)
        {
            double[] transX = new double[x.Length];
            if (izero)
            {
                double[,] A = new double[Points.Length, dim];
                double[] B = new double[Points.Length];
                int intValid = 0;
                for (int i = 0; i < Points.Length; i++)
                {
                    for (int j = 0; j < dim; j++)
                    {
                        A[intValid, j] = Math.Pow(Points[i].X, j + 1);
                    }
                    B[intValid] = Points[i].Y;
                    intValid++;
                }
                if (intValid < dim)
                {
                    return false;
                }
                MatrixFun.MatrixEquation(Points.Length, dim, A, B, transX);
                for (int i = 0; i < x.Length; i++)
                {
                    x[i] = transX[i];
                }
            }
            else
            {
                double[,] A = new double[Points.Length, dim + 1];
                double[] B = new double[Points.Length];
                int intValid = 0;
                for (int i = 0; i < Points.Length; i++)
                {
                    A[intValid, 0] = 1;
                    for (int j = 0; j < dim; j++)
                        A[intValid, j + 1] = Math.Pow(Points[i].X, j + 1);
                    B[intValid] = Points[i].Y;
                    intValid++;
                }
                if (intValid < dim + 1)
                {
                    return false;
                }
                MatrixFun.MatrixEquation(Points.Length, dim + 1, A, B, transX);
                for (int i = 0; i < x.Length; i++)
                {
                    x[i] = transX[transX.Length - i - 1];
                }
            }

            return true;
        }

        /// <summary>
        /// 为Thick曲线重置标样和纯谱数据,暂时不考虑
        /// </summary>
        /// <param name="workCurve">当前工作曲线</param>
        public static void TransSpecDataForRemoveBg(WorkCurve workCurve)
        {
            if (!IsThick || workCurve == null || workCurve.ElementList == null || workCurve.ElementList.Items.Count <= 0) return;
            foreach (var element in workCurve.ElementList.Items)
            {
                if (element.ElementSpecName != "")//改变纯元素谱数据
                {
                    SqlParams params0 = new SqlParams("SpecTypeValue", "1", true);
                    SqlParams params1 = new SqlParams("Name", element.ElementSpecName, false);
                    List<SpecListEntity> lstSpecList = WorkCurveHelper.DataAccess.Query(new SqlParams[] { params0, params1 });
                    if (lstSpecList.Count > 0)
                    {
                        int[] data = lstSpecList[0].Specs[0].SpecDatac;
                        //  element.SSpectrumData = Helper.TransforToDivTime(Helper.ToStrs(data), lstSpecList[0].Specs[0].UsedTime);
                        element.SSpectrumData = Helper.TransforToDivTimeAndCurrent(Helper.ToStrs(data), lstSpecList[0].Specs[0].UsedTime, lstSpecList[0].ActualCurrent);

                    }
                }
                foreach (var sample in element.Samples)
                {
                    SqlParams params0 = new SqlParams("SpecTypeValue", "0", true);
                    SqlParams params1 = new SqlParams("Name", sample.SampleName, false);
                    List<SpecListEntity> lstSpecList = WorkCurveHelper.DataAccess.Query(new SqlParams[] { params0, params1 });
                    if (lstSpecList.Count > 0)
                    {
                        try
                        {
                            workCurve.CaculateIntensity(lstSpecList[0]);
                        }
                        catch (Exception ex)
                        {
                            Msg.Show(lstSpecList[0].SampleName + ex.Message);
                            continue;
                        }
                        sample.X = element.Intensity.ToString();
                    }
                }
            }
        }
    }

    enum MatchModel
    {
        Auto,
        Manually
    }

    public enum AddSpectrumType
    {
        OpenSpectrum,
        OpenVisualSpec,
        OpenStandardSpec,
        OpenPureSpec,
        OpenExploreSpec
    }
}
