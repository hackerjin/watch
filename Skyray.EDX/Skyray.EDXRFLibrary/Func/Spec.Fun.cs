using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Skyray.EDXRFLibrary
{
    //partial class SpecList
    //{

    //    /// <summary>
    //    /// 逃逸峰处理
    //    /// </summary>
    //    /// <param name="fSpec">普数据</param>
    //    /// <param name="nBins">数组长度</param>
    //    /// <param name="kevch">谱线能量分辨率 单位:Kev/channel</param>
    //    /// <param name="fDetAngle">X射线进入探测器与普通光线照射探测器表面的夹角，默认值为0</param>
    //    /// <param name="ampFac">测试目的调节因子，常规使用为1</param>
    //    [DllImport("fppro.dll", EntryPoint = "spEscProc")]
    //    private static extern void spEscProc(float[] fSpec, int nBins, float kevch, float fDetAngle, float ampFac);

    //    /// <summary>
    //    /// 堆积峰处理
    //    /// </summary>
    //    /// <param name="fSpec">谱线数据</param>
    //    /// <param name="nBins">数组长度</param>
    //    /// <param name="kevch">谱线能量分辨率 单位:Kev/channel</param>
    //    /// <param name="ChanOff">峰漂移，常规使用为0</param>
    //    /// <param name="ppr">脉冲对分辨率：绝大多数为1</param>
    //    /// <param name="acqTime">采集时间</param>
    //    /// <param name="adjFac">调节因子：常规为1</param>
    //    [DllImport("fppro.dll", EntryPoint = "spSumProc")]
    //    private static extern void spSumProc(float[] fSpec, int nBins, float kevch, int ChanOff, float ppr, float acqTime, float adjFac);

    //    /// <summary>
    //    /// 去本底 适用于高性能探测器 如SI(LI)
    //    /// </summary>
    //    /// <param name="fSpec">保数普数据的数组</param>
    //    /// <param name="nBins">数组的长度</param>
    //    /// <param name="ampFac">道宽：单位 kev / ch</param>
    //    [DllImport("fppro.dll", EntryPoint = "spBkgAutoProc1")]
    //    private static extern void spBkgAutoProc1(float[] fSpec, int nBins, float kevch);

    //    /// <summary>
    //    /// 去本底 适用于低性能探测器 如正比计数管
    //    /// </summary>
    //    /// <param name="fSpec">保数普数据的数组</param>
    //    /// <param name="nBins">数组的长度</param>
    //    /// <param name="ampFac">通常设为1</param>
    //    [DllImport("fppro.dll", EntryPoint = "spBkgAutoProc2")]
    //    private static extern void spBkgAutoProc2(float[] fSpec, int nBins, float ampFac);

    //    /// <summary>
    //    /// 给定本底的去扣除方法
    //    /// </summary>
    //    /// <param name="fSpec">普数据</param>
    //    /// <param name="nBins">普数据点数</param>
    //    /// <param name="wPts">本底所在道数</param>
    //    [DllImport("fppro.dll", EntryPoint = "spBkgStLineProc")]
    //    private static extern void spBkgStLineProc(float[] fSpec, int nBins, ushort[] wPts);


    //    /// <summary>
    //    /// 设置探测器类型
    //    /// </summary>
    //    /// <param name="dectorType">1=半导体探测器；2=计数盒</param>
    //    [DllImport("fppro.dll", EntryPoint = "spSetDetectorType")]
    //    private static extern void spSetDetectorType(byte dectorType);


    //    /// <summary>
    //    /// 去除背景3(给给定本底扣除)
    //    /// </summary>
    //    /// <param name="Pts"></param>
    //    public bool RemoveBaseStLine(float[] fSpec,int specLen,string BkgAutoProc3Points)
    //    {
    //        if (BkgAutoProc3Points == null || BkgAutoProc3Points.Trim()==string.Empty)
    //        {
    //            return false;
    //        }
    //        string[] bkg = ((BkgAutoProc3Points).ToString()).Split(new char[] { ',' });
    //        ushort[] Pts = new ushort[bkg.Length];
    //        try
    //        {
    //            Pts = new ushort[bkg.Length];
    //            for (int i = 0; i < Pts.Length; i++)
    //            {
    //                Pts[i] = Convert.ToUInt16(bkg[i]);
    //            }
    //        }
    //        catch (Exception)
    //        {
    //            return false;// ("字符串格式错误！");
    //        }
    //        ushort[] wPts = new ushort[Pts.Length + 1];
    //        wPts[Pts.Length] = 0;
    //        Array.Copy(Pts, wPts, Pts.Length);
    //        spBkgStLineProc(fSpec, specLen, wPts);
    //        //折线法扣本底
    //        for (int j = 0; j < specLen; j++)
    //        {
    //            fSpec[j] = fSpec[j] < 0 ? 0 : fSpec[j];
    //        }
    //        return true;
    //    }
    //      /// <summary>
    //    /// 去本底1/2
    //    /// </summary>
    //    /// <param name="fSpec">谱数据</param>
    //    /// <param name="specLen">谱长</param>
    //    /// <param name="iNumber">用于指示使用哪一种扣本底方式 1：  扣本底1  2：  扣本底2 </param>
    //    /// <param name="secParam">去除次数</param>
    //    /// <param name="factor">因子</param>
    //     public void RemoveBase(float[] fSpec,int specLen, int iNumber, int secParam, float factor)
    //     {
    //          for (int j = 0; j < secParam;j++ )
    //            {
    //                switch (iNumber)
    //                {
    //                    case 1:
    //                        spBkgAutoProc1(fSpec, specLen, factor);
    //                        break;
    //                    case 2:
    //                        spBkgAutoProc2(fSpec, specLen, factor);
    //                        break;
    //                    default:
    //                        break;
    //                }

    //            }
    //     }
    //    /// <summary>
    //    /// 谱线处理
    //    /// </summary>
    //    /// <param name="workCurve">工作曲线</param>
    //    /// <param name="type">探测器类型</param>
    //     /// <param name="specLenth">谱长</param>
    //    public void SpectrumProc(WorkCurve workCurve,DetectorType type,int specLenth)
    //    {
    //        if (workCurve==null||workCurve.CalibrationParam==null)
    //        {
    //            return;
    //        }
    //        int SpecLength = specLenth;
    //        spSetDetectorType(type);
    //        float[] fSpec = new float[SpecLength];
    //        int SpecCount = this.Specs.Count;
            
    //        for (int i = 0; i < SpecCount; i++)
    //        {
    //            int[] spec = new int[SpecLength];
    //            Array.Copy(this.Specs[i].SpecDatas, fSpec, SpecLength);
    //            //逃逸峰
    //            if (workCurve.CalibrationParam.IsEscapePeakProcess)
    //            {
    //                spEscProc(fSpec, SpecLength, (float)DemarcateEnergyHelp.k1, (float)workCurve.CalibrationParam.EscapeAngle, (float)workCurve.CalibrationParam.EscapeFactor);
    //            }
    //            //和峰
    //            if (workCurve.CalibrationParam.IsSumPeakProcess)
    //            {
    //                spSumProc(fSpec, SpecLength, (float)DemarcateEnergyHelp.k1, DemarcateEnergyHelp.GetChannel(0), (float)workCurve.CalibrationParam.PulseResolution, (float)this.Specs[i].SpecTime, (float)workCurve.CalibrationParam.SumFactor);
    //            }
    //            //去本底1
    //            if (workCurve.CalibrationParam.IsRemoveBackGroundOne)
    //            {
    //                RemoveBase(fSpec,SpecLength,1,workCurve.CalibrationParam.RemoveFirstTimes,(float)workCurve.CalibrationParam.RemoveFirstFactor);
    //            }
    //            //去本底2
    //            if (workCurve.CalibrationParam.IsRemoveBackGroundTwo)
    //            {
    //                RemoveBase(fSpec,SpecLength,2,workCurve.CalibrationParam.RemoveSecondTimes,(float)workCurve.CalibrationParam.RemoveSecondFactor);
    //            }
    //            //去本底3
    //            if (workCurve.CalibrationParam.IsRemoveBackGroundThree)
    //            {
    //                RemoveBaseStLine(fSpec, SpecLength, workCurve.CalibrationParam.BackGroundPoint);
    //            }
    //            for (int j = 0; j < SpecLength; j++)
    //            {
    //                spec[j] = Convert.ToInt32(fSpec[j]);
    //            }
    //            Specs[i].SpecData = Helper.ToStrs(spec);
    //        }
    //    }


    //    //设置探测器的类型
    //    public static void spSetDetectorType(DetectorType type)
    //    {
    //        switch (type)
    //        {
    //            case DetectorType.Dp5:
    //                spSetDetectorType((byte)1);
    //                break;
    //            case DetectorType.Si:
    //                spSetDetectorType((byte)1);
    //                break;
    //            case DetectorType.Xe:
    //                spSetDetectorType((byte)2);
    //                break;
    //            default:
    //                spSetDetectorType((byte)1);
    //                break;
    //        }

    //    }
    //}

    public class SpecProcess
    {
        /// <summary>
        /// 逃逸峰处理
        /// </summary>
        /// <param name="fSpec">普数据</param>
        /// <param name="nBins">数组长度</param>
        /// <param name="kevch">谱线能量分辨率 单位:Kev/channel</param>
        /// <param name="fDetAngle">X射线进入探测器与普通光线照射探测器表面的夹角，默认值为0</param>
        /// <param name="ampFac">测试目的调节因子，常规使用为1</param>
        [DllImport("fppro.dll", EntryPoint = "spEscProc")]
        private static extern void spEscProc(float[] fSpec, int nBins, float kevch, float fDetAngle, float ampFac);

        /// <summary>
        /// 堆积峰处理
        /// </summary>
        /// <param name="fSpec">谱线数据</param>
        /// <param name="nBins">数组长度</param>
        /// <param name="kevch">谱线能量分辨率 单位:Kev/channel</param>
        /// <param name="ChanOff">峰漂移，常规使用为0</param>
        /// <param name="ppr">脉冲对分辨率：绝大多数为1</param>
        /// <param name="acqTime">采集时间</param>
        /// <param name="adjFac">调节因子：常规为1</param>
        [DllImport("fppro.dll", EntryPoint = "spSumProc")]
        private static extern void spSumProc(float[] fSpec, int nBins, float kevch, int ChanOff, float ppr, float acqTime, float adjFac);

        /// <summary>
        /// 去本底 适用于高性能探测器 如SI(LI)
        /// </summary>
        /// <param name="fSpec">保数普数据的数组</param>
        /// <param name="nBins">数组的长度</param>
        /// <param name="ampFac">道宽：单位 kev / ch</param>
        [DllImport("fppro.dll", EntryPoint = "spBkgAutoProc1")]
        private static extern void spBkgAutoProc1(float[] fSpec, int nBins, float kevch);

        /// <summary>
        /// 去本底 适用于低性能探测器 如正比计数管
        /// </summary>
        /// <param name="fSpec">保数普数据的数组</param>
        /// <param name="nBins">数组的长度</param>
        /// <param name="ampFac">通常设为1</param>
        [DllImport("fppro.dll", EntryPoint = "spBkgAutoProc2")]
        private static extern void spBkgAutoProc2(float[] fSpec, int nBins, float ampFac);

        /// <summary>
        /// 给定本底的去扣除方法
        /// </summary>
        /// <param name="fSpec">普数据</param>
        /// <param name="nBins">普数据点数</param>
        /// <param name="wPts">本底所在道数</param>
        [DllImport("fppro.dll", EntryPoint = "spBkgStLineProc")]
        private static extern void spBkgStLineProc(float[] fSpec, int nBins, ushort[] wPts);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fSpec"></param>
        /// <param name="nBins"></param>
        /// <param name="LoExp"></param>
        /// <param name="HiExp"></param>
        /// <param name="LoAct"></param>
        /// <param name="HiAct"></param>
         [DllImport("fppro.dll", EntryPoint = "spSpectrumCalibrate")]
        public static extern void spSpectrumCalibrate(float[] fSpec, int nBins, float LoExp, float HiExp, float LoAct, float HiAct );
        /// <summary>
        /// 设置探测器类型
        /// </summary>
        /// <param name="dectorType">1=半导体探测器；2=计数盒</param>
        [DllImport("fppro.dll", EntryPoint = "spSetDetectorType")]
        private static extern void spSetDetectorType(byte dectorType);



        [DllImport("RubberBand.dll", EntryPoint = "RubberBand")]
        private static extern void DllRubberBand(double[] xdatas, double[] ydatas,
             double[] newYdatas, int datalength, int loopCount);
        /// <summary>
        /// 去除背景3(给给定本底扣除)
        /// </summary>
        /// <param name="Pts"></param>
        public static bool RemoveBaseStLine(float[] fSpec, int specLen, string BkgAutoProc3Points)
        {
            if (BkgAutoProc3Points == null || BkgAutoProc3Points.Trim() == string.Empty)
            {
                return false;
            }
            string[] bkg = ((BkgAutoProc3Points).ToString()).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            ushort[] Pts = new ushort[bkg.Length];
            try
            {
                //Pts = new ushort[bkg.Length];
                for (int i = 0; i < Pts.Length; i++)
                {
                    Pts[i] = Convert.ToUInt16(bkg[i]);
                }
            }
            catch (Exception)
            {
                return false;// ("字符串格式错误！");
            }
            ushort[] wPts = new ushort[Pts.Length + 1];
            wPts[Pts.Length] = 0;
            Array.Copy(Pts, wPts, Pts.Length);
            spBkgStLineProc(fSpec, specLen, wPts);
            //折线法扣本底
            for (int j = 0; j < specLen; j++)
            {
                fSpec[j] = fSpec[j] < 0 ? 0 : fSpec[j];
            }
            return true;
        }

        /// <summary>
        /// 去本底1/2
        /// </summary>
        /// <param name="fSpec">谱数据</param>
        /// <param name="specLen">谱长</param>
        /// <param name="iNumber">用于指示使用哪一种扣本底方式 1：  扣本底1  2：  扣本底2 </param>
        /// <param name="secParam">去除次数</param>
        /// <param name="factor">因子</param>
        public static void RemoveBase(float[] fSpec, int specLen, int iNumber, int secParam, float factor)
        {
            for (int j = 0; j < secParam; j++)
            {
                switch (iNumber)
                {
                    case 1:
                        spBkgAutoProc1(fSpec, specLen, factor);
                        break;
                    case 2:
                        if(IsCanRemoveBkgTwo(fSpec, factor))
                        spBkgAutoProc2(fSpec, specLen, factor);
                        break;
                    default:
                        break;
                }

            }
        }

        static float[] fSpec = null;
        static int[] specnew = null;

        //public static int[] SpectrumProc(WorkCurve workCurve, int[] SpecOrg, float usedTime, int type)
        //{
        //    int[] back = null;
        //    return SpectrumProc(workCurve, SpecOrg, usedTime, type, back);

        //}

        /// <summary>
        /// 谱处理
        /// </summary>
        /// <param name="workCurve">曲线设置信息</param>
        /// <param name="SpecOrg">谱的原始数据</param>
        /// <param name="usedTime">谱的时间</param>
        /// <param name="type">1,扣本底 2，不扣本底</param>
        /// <returns>谱处理后的数据</returns>
        public static int[] SpectrumProc(WorkCurve workCurve, int[] SpecOrg,float usedTime,int type)  
        {
            if (workCurve == null || workCurve.CalibrationParam == null || workCurve.Condition == null || workCurve.Condition.Device == null || workCurve.Condition.Device.Detector == null)
            {
                return SpecOrg;
            }
            int SpecLength = SpecOrg.Length;
            //if (fSpec == null)
                fSpec = new float[SpecLength];
            //if (specnew == null)
                specnew = new int[SpecLength];
            Array.Clear(fSpec, 0, SpecLength);
            Array.Clear(specnew, 0, SpecLength);
            try
            {

                //FpWorkCurve.SetSourceData(workCurve.Condition, workCurve.Condition.Device.Tubes);
                //spSetDetectorType(workCurve.Condition.Device.Detector.Type);
                Array.Copy(SpecOrg, fSpec, SpecLength);
                if (workCurve.CalibrationParam.IsEscapePeakProcess)//逃逸峰
                {
spSetDetectorType(workCurve.Condition.Device.Detector.Type);
                    spEscProc(fSpec, SpecLength, (float)DemarcateEnergyHelp.k1, (float)workCurve.CalibrationParam.EscapeAngle, (float)workCurve.CalibrationParam.EscapeFactor);
                }
                if (workCurve.CalibrationParam.IsSumPeakProcess)//和峰2011-08-08 零能量值的通道的Offset
                {
                    spSumProc(fSpec, SpecLength, (float)DemarcateEnergyHelp.k1, -DemarcateEnergyHelp.GetChannel(0), (float)workCurve.CalibrationParam.PulseResolution, usedTime, (float)workCurve.CalibrationParam.SumFactor);
                }
                if (type==2)  //fpguass 里面用到扣本底之前的数据
                {
                    for (int j = 0; j < SpecLength; j++)
                    {
                        specnew[j] = Convert.ToInt32(fSpec[j] >= 0 ? fSpec[j] : 0);
                    }
                    return specnew;
                }
                if (workCurve.CalibrationParam.IsRemoveBackGroundOne)//去本底1
                {
                    RemoveBase(fSpec, SpecLength, 1, workCurve.CalibrationParam.RemoveFirstTimes, (float)DemarcateEnergyHelp.k1);
                }
                if (workCurve.CalibrationParam.IsRemoveBackGroundTwo)//去本底2
                {
                    //Console.WriteLine(Helper.ToStrs(SpecOrg));
                    //Console.WriteLine(Helper.ToStrs(fSpec));

                    
                    //spSetDetectorType(DetectorType.Xe);
                    RemoveBase(fSpec, SpecLength, 2, workCurve.CalibrationParam.RemoveSecondTimes, (float)workCurve.CalibrationParam.RemoveSecondFactor);
                }
                if (workCurve.CalibrationParam.IsRemoveBackGroundThree)//去本底3
                {
                    RemoveBaseStLine(fSpec, SpecLength, workCurve.CalibrationParam.BackGroundPoint);
                }
                //添加去本底4
                if (workCurve.CalibrationParam.IsRemoveBackGroundFour
                    && workCurve.CalibrationParam.RemoveFourTimes > 0
                    && workCurve.CalibrationParam.RemoveFourLeft > 0
                    && workCurve.CalibrationParam.RemoveFourRight > 0
                    && workCurve.CalibrationParam.RemoveFourLeft < workCurve.CalibrationParam.RemoveFourRight)
                {
                    int left = workCurve.CalibrationParam.RemoveFourLeft;
                    int right = workCurve.CalibrationParam.RemoveFourRight;
                    double[] x = new double[right - left + 1];
                    double[] Y = new double[right - left + 1];
                    for (int i = 0; i < x.Length; i++)
                    {
                        x[i] = i + 1;
                        Y[i] = fSpec[left + i - 1];
                    }
                    double[] newY = new double[right - left + 1];

                    //Array.Copy(fSpec, Y, SpecLength);
                    try
                    {
                        DllRubberBand(x, Y, newY, x.Length, workCurve.CalibrationParam.RemoveFourTimes);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    for (int j = 0; j < right - left + 1; j++)
                    {
                        fSpec[left + j - 1] = (float)newY[j];
                    }
                }
                //if (workCurve.CalibrationParam.IsRemoveBackGroundFive)
                //{
                //    int row, col, n;
                //    double temp = 0;
                //    double[,] A;
                //    double[] B;
                //    double[] C = new double[7];
                //    string[] tempStrArr = workCurve.CalibrationParam.BackGroundPointFive.Split('|');
                //    Border[] borders = new Border[tempStrArr.Length];
                //    for (int i = 0; i < tempStrArr.Length; i++)
                //    {
                //        Border bd = new Border(tempStrArr[i]);
                //        borders[i] = bd;
                //    }
                //    for (int i = 0; i < borders.Length; i++)
                //    {
                //        col = borders[i].Order + 1;
                //        row = 0;
                //        for (int j = 0; j < borders[i].ChildCount; j++)
                //            row += borders[i][j].Right - borders[i][j].Left + 1;

                //        A = new double[row, col];
                //        B = new double[row];
                //        row = 0;

                //        for (int j = 0; j < borders[i].ChildCount; j++)
                //        {
                //            for (int k = borders[i][j].Left; k <= borders[i][j].Right; k++)
                //            {
                //                B[row] = fSpec[k];
                //                A[row, 0] = 1;
                //                for (n = 1; n < col; n++)
                //                    A[row, n] = A[row, n - 1] * k;

                //                row++;
                //            }
                //        }
                //        try
                //        {

                //            MatrixEquation(row, col, A, B, C);
                //        }
                //        catch (Exception ex)
                //        {
                //            Console.WriteLine(ex.ToString());
                //        }

                //        for (int j = borders[i].Left; j <= borders[i].Right; j++)
                //        {
                //            temp = 1;
                //            Back[j] = (int)C[0];
                //            for (n = 1; n < col; n++)
                //            {
                //                temp *= j;
                //                Back[j] += (int)(C[n] * temp);
                //            }

                //        }

                //    }
                //    for (int j = 0; j < SpecLength; j++)
                //    {
                //        specnew[j] = Convert.ToInt32(SpecOrg[j] - Back[j] >= 0 ? SpecOrg[j] - Back[j] : 0); //原始谱数据减去背景得到真实谱数据
                //    }
                //    return specnew;
                //}
                for (int j = 0; j < SpecLength; j++)
                {
                    specnew[j] = Convert.ToInt32(fSpec[j] >= 0 ? fSpec[j] : 0);
                    //if(Back != null)
                    //    Back[j] = SpecOrg[j] - specnew[j] > 0 ? SpecOrg[j] - specnew[j] : 0;
                }
            }
            catch 
            {
                ;
            }
            
            return specnew;
        }

        public static void SpectrumProcBySkraySpecial(WorkCurve workCurve, ref int[] SpecOrg, float usedTime)
        {
            if (workCurve==null||workCurve.SpecialRemoveParam==null||workCurve.SpecialRemoveParam.RemoveItems.Count <= 0) return;
            foreach (var item in workCurve.SpecialRemoveParam.RemoveItems)
            {

                //拟合谱求半高宽
                if (item.PeakLow >= item.PeakHigh || item.PeakLow <= 0 || item.PeakHigh > SpecOrg.Length ) continue;
                //求被选中部分的净面积跟阈值比较，大者才做扣除
                double netArea = 0;
	            int datasLen=item.PeakHigh - item.PeakLow + 1;
                double [] datas = new double[datasLen];
                double yleft = SpecOrg[item.PeakLow];
                double yright = SpecOrg[item.PeakHigh];
                double k1 = (yleft - yright) / (item.PeakLow - item.PeakHigh);//(Spec.Data[left]-Spec.Data[right])*1.0/(left-right);
                double k0 = SpecOrg[item.PeakLow] - k1 * item.PeakLow;
                for (int i = item.PeakLow; i <= item.PeakHigh; i++)
                {
                    datas[i - item.PeakLow] = SpecOrg[i] - k1 * i - k0;
                    netArea += datas[i - item.PeakLow];
                    if (datas[i - item.PeakLow] <= 0)
	                {
                        datas[i - item.PeakLow] = 0;
	                }
	                else
	                {
                        datas[i - item.PeakLow] = Math.Log(datas[i - item.PeakLow]);
	                }
                }
                if (netArea / usedTime < item.AreaLimit)
                {
                    if (workCurve.SpecialRemoveParam.BaseLow > 0 && workCurve.SpecialRemoveParam.BaseHigh > workCurve.SpecialRemoveParam.BaseLow)
                    {
                        for (int i = item.PeakLow; i <= item.PeakHigh; i++)
                        {
                            SpecOrg[i] = SpecOrg[(i - item.PeakLow) % (workCurve.SpecialRemoveParam.BaseHigh - workCurve.SpecialRemoveParam.BaseLow) + workCurve.SpecialRemoveParam.BaseLow];
                        }
                    }
                    continue;
                }
                 double[,] a = new double[datasLen, 3];
                 double[] x = new double[3];
                for (int i = 0; i < datasLen; i++)
                {
	                a[i,0] = 1;
                    a[i, 1] = i + item.PeakLow;
	                a[i,2] = a[i,1] * a[i,1];
                }
                 MatrixFun.MatrixEquation(datasLen, 3, a, datas, x);

                // a实际谱的H
                 double ymax = x[0] + x[1] * x[1] / (4*(-x[2]));
                 ymax = Math.Exp(ymax);
                //delta
                 double c = -1 / x[2];
                //x0
                 double b = x[1]/ 2 * c;
                //根据阈值算H1
                 double ymax1 = item.AreaLimit * usedTime / Math.Sqrt(3.1415927f * c);

                 bool AllZero = true;
                 for (int i = item.PeakLow; i <= item.PeakHigh; i++)
                 {
                     double smoothtemp = SpecOrg[i] - ymax1 * Math.Exp(-Math.Pow((i - b), 2) / c);
                     SpecOrg[i] = smoothtemp>0?(int)smoothtemp:0;
                     AllZero = AllZero && (SpecOrg[i] == 0);
                 }
                 if (AllZero && workCurve.SpecialRemoveParam.BaseLow > 0 && workCurve.SpecialRemoveParam.BaseHigh > workCurve.SpecialRemoveParam.BaseLow)
                 {
                     for (int i = item.PeakLow; i <= item.PeakHigh; i++)
                     {
                         SpecOrg[i] = SpecOrg[(i - item.PeakLow) % (workCurve.SpecialRemoveParam.BaseHigh - workCurve.SpecialRemoveParam.BaseLow) + workCurve.SpecialRemoveParam.BaseLow];
                     }
                 }

            }

        }

        public static void spSetDetectorType(DetectorType type)
        {
            switch (type)
            {
                //case DetectorType.Dp5:
                //    spSetDetectorType((byte)1);
                //    break;
                case DetectorType.Si:
                    spSetDetectorType((byte)1);
                    break;
                case DetectorType.Xe:
                    spSetDetectorType((byte)2);
                    break;
                default:
                    spSetDetectorType((byte)1);
                    break;
            }

        }
        // add by chuyaqin 2011-10-12 找拐点
        public static bool IsCanRemoveBkgTwo(float[] fSpec, float ampfac)
        {
            int nBins=fSpec.Length;
            float[] xl = new float[fSpec.Length];
            if (ampfac==0)
            {
                ampfac = 1.0f;
            }
            for (int i = 0; i < nBins;i++ )
            {
                xl[i] = fSpec[i] * ampfac;
            }
            float ptb1=0;
            float ptb2=0;
            int Filtersize = 25;
            int Nfstart=9;
            int istart = (Filtersize + 3) / 2;
            int istop = nBins - 24;
            float bsum=0;
            float ptxv = 0;
            int bkch0=0;
            ptb1 = xl[istart-2];
            ptb2 = xl[istart - 1]; 
            for (int i=istart-Filtersize/2-1;i<istart+Filtersize/2;i++)
            {
                bsum+=xl[i];
            }
            for (int i = istart; i < istop-1 ;i++ )
            {
                bsum += -xl[i - Filtersize / 2 + 1] + xl[i + Filtersize / 2];
                ptxv = bsum / Filtersize;
                if (bkch0!=-1)
                {
                    if (ptxv > xl[i]) bkch0++;
                    else bkch0=0;
                    if (bkch0 != Nfstart)
                    {
                        ptb1 = ptb2;
                        ptb2 = ptxv;
                        continue;
                    }
                    bkch0 = -1;
                }
                if (ptb2>0&&ptb2<=ptb1&&ptb2<ptxv&&xl[i-3]*1.5>=ptb2)
                {
                    return true;
                }
                ptb1 = ptb2;
                ptb2 = ptxv;
            }
            return false;
        }

        //解矩阵方程(Ax=B)
        public static void MatrixEquation(int m, int n, double[,] a, double[] b, double[] x)
        {
            double[,] at = new double[n, m]; //A转置矩阵
            double[,] a1 = new double[n, n]; //B逆矩阵
            double[,] c = new double[m, 2];
            double[,] b2 = new double[m, 2];
            for (int i = 0; i <= n - 1; i++)
                for (int j = 0; j <= m - 1; j++)
                {
                    at[i, j] = a[j, i];
                }
            Trmul(n, m, n, at, a, a1);
            Rinv(n, a1);
            for (int i = 0; i <= m - 1; i++)
                b2[i, 0] = b[i];
            Trmul(n, m, 1, at, b2, c);
            Trmul(n, n, 1, a1, c, at);
            for (int i = 0; i <= n - 1; i++)
                x[i] = at[i, 0];
        }

        public static void Trmul(int m, int n, int k, double[,] a, double[,] b, double[,] c) //矩阵相乘
        {
            // a[m,n] b[n,k] c[m,k]
            int i, j, L;
            for (i = 0; i <= m - 1; i++)
                for (j = 0; j <= k - 1; j++)
                {
                    c[i, j] = 0.0;
                    for (L = 0; L <= n - 1; L++)
                        c[i, j] = c[i, j] + a[i, L] * b[L, j];
                }
        }

        public static void Rinv(int n, double[,] a)//矩阵的逆
        {
            int[] si = new int[n];
            int[] sj = new int[n];
            int i, j, k;
            double d, p;

            for (k = 0; k < n; k++)
            {
                d = 0.0;
                for (i = k; i < n; i++)
                    for (j = k; j < n; j++)
                    {
                        p = System.Math.Abs(a[i, j]);
                        if (p > d)
                        {
                            d = p; si[k] = i; sj[k] = j;
                        }
                    }
                if ((d + 1.0) == 1.0)
                {
                    throw new Exception("");
                }
                if (si[k] != k)
                    for (j = 0; j < n; j++)
                    {
                        p = a[k, j];
                        a[k, j] = a[si[k], j];
                        a[si[k], j] = p;
                    }
                if (sj[k] != k)
                    for (i = 0; i < n; i++)
                    {
                        p = a[i, k];
                        a[i, k] = a[i, sj[k]];
                        a[i, sj[k]] = p;
                    }

                a[k, k] = 1.0 / a[k, k];
                for (j = 0; j < n; j++)
                    if (j != k)
                        a[k, j] = a[k, j] * a[k, k];
                for (i = 0; i < n; i++)
                    if (i != k)
                        for (j = 0; j < n; j++)
                            if (j != k)
                                a[i, j] = a[i, j] - a[i, k] * a[k, j];
                for (i = 0; i < n; i++)
                    if (i != k)
                        a[i, k] = -a[i, k] * a[k, k];
            }

            for (k = n - 1; k >= 0; k--)
            {
                if (sj[k] != k)
                    for (j = 0; j <= n - 1; j++)
                    {
                        p = a[k, j]; a[k, j] = a[sj[k], j]; a[sj[k], j] = p;
                    }
                if (si[k] != k)
                    for (i = 0; i <= n - 1; i++)
                    {
                        p = a[i, k]; a[i, k] = a[i, si[k]]; a[i, si[k]] = p;
                    }
            }
        }

        public class Border
        {
            public Border(string strBkg)
            {
                string[] tempstrs = strBkg.Split('*');
                string[] strBorder = tempstrs[0].Split(';');
                for (int i = 0; i < strBorder.Length; i++)
                {
                    string[] strArr = strBorder[i].Split(',');
                    BorderItem bi = new BorderItem() { Left = int.Parse(strArr[0]), Right = int.Parse(strArr[1]) };
                    Childs.Add(bi);
                }
                this.Order = int.Parse(tempstrs[1]);
                this.Left = Childs[0].Left;
                this.Right = Childs[ChildCount - 1].Right;
            }

            public struct BorderItem
            {
                public int Left;
                public int Right;
            }
            public int Order;  //多项式的阶
            public int Left;
            public int Right;
            List<BorderItem> Childs = new List<BorderItem>();
            public int ChildCount
            {
                get { return Childs.Count; }
            }
            public BorderItem this[int index]
            {
                get { return Childs[index]; }
            }
        }
    }
}
