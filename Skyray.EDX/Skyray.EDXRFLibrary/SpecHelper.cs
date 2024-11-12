using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Skyray.EDXRFLibrary
{
    public class SpecHelper
    {
        public static WorkCurve CURRENTWorkCurveTemp;
        public static Device CurrentDevice;
        public static int SmoothTimes = 1;
        public static int ElemCountLimit = 0;

        public static bool IsSmoothProcessData = false;
        ///// <summary>
        ///// 当前小条件下最高信道
        ///// </summary>
        ///// <param name="DevParam">当前小条件</param>
        ///// <param name="Data">当前谱数据</param>
        ///// <returns></returns>
        //public static int GetHighSpecChannel(int start,int end, int[] Data)
        //{
        //    if (Data == null || end > Data.Length-1)
        //        return 0;
        //    int chann = start;
        //    for (int i = 0; i < end; ++i)
        //    {
        //        if (Data[chann] < Data[i])
        //        {
        //            chann = i;
        //        }
        //    }
        //    return chann;
        //}
        /// <summary>
        /// 当前小条件下最高信道
        /// </summary>
        /// <param name="DevParam">当前小条件</param>
        /// <param name="Data">当前谱数据</param>
        /// <returns></returns>
        public static int GetHighSpecChannel(int start, int end, int[] Data)
        {
            if (Data == null || end > Data.Length - 1)
                return 0;
            int chann = start;
            for (int i = start; i < end; ++i)
            {
                if (Data[chann] < Data[i])
                {
                    chann = i;
                }
            }
            return chann;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static int GetHighSpecValue(int[] Data)
        {
            if (Data == null || Data.Length == 0)
                return 0;
            int maxValue = Data[0];
            for (int i = 0; i < Data.Length; ++i)
            {
                if (maxValue < Data[i])
                {
                    maxValue = Data[i];
                }
            }
            return maxValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxValue"></param>
        /// <param name="maxChannel"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int GetMinValue(int maxValue,int maxChannel,int[] data)
        {
            double tempValue = maxValue * 0.1;
            int rightValeCh = maxChannel;
            int bakChOfMinValue = 0;
            for (int i = rightValeCh + 1; i < data.Length; i++)
            {
                if (tempValue >= data[i])
                {
                    rightValeCh = i; //右峰谷
                    break;
                }
            }
            int leftValeCh = maxChannel * 2 - rightValeCh; //左峰谷

            int tempRightPeakCh = rightValeCh;
            for (int i = rightValeCh + 1; i < data.Length; i++) //第二峰在最高峰的右边
            {
                if (data[tempRightPeakCh] < data[i])
                {
                    tempRightPeakCh = i;
                }
            }

            int tempLeftPeakCh = leftValeCh;
            for (int i = leftValeCh - 1; i > 50; i--) //第二峰在最高峰的左边
            {
                if (data[tempLeftPeakCh] < data[i])
                {
                    tempLeftPeakCh = i;
                }
            }

            if (data[tempRightPeakCh] > data[tempLeftPeakCh])
            {
                bakChOfMinValue = tempRightPeakCh;
            }
            else
            {
                bakChOfMinValue = tempLeftPeakCh;
            }

            return bakChOfMinValue;
        }


        /// 找谱的理论最高点加权平均法
        /// </summary>
        /// <returns></returns>
        public static Double FitChannOfMaxValue(int begin,int end, int[] Data)
        {
            if (Data == null || end > Data.Length - 1)
                return 0;
            int ch = GetHighSpecChannel(begin,end, Data);
            int value = Data[ch] / 2;
            int high = ch;
            int low = ch;
            for (int i = ch + 1; i < Data.Length; i++)
            {
                if (Data[i] <= value)
                {
                    high = i;
                    break;
                }
            }
            for (int i = ch - 1; i > 0; i--)
            {
                if (Data[i] <= value)
                {
                    low = i;
                    break;
                }
            }
            //Int64 pa = 0;
            //Int64 pb = 0;
            double pa = 0;
            double pb = 0;
            for (int i = low; i <= high; i++)
            {
                pa += (double)Data[i] * i;
                pb += (double)Data[i];
            }
            if (pb <= 0)
            {
                return 0;
            }
            else
            {
                return pa * 1.0 / pb;
            }
        }

        public static Double FitChannOfMaxValueEdit(int begin, int end, int[] Data, ref int low, ref int high, ref bool lresult, ref bool rresult)
        {
            int ch = GetHighSpecChannel(begin, end, Data);
            int value = Data[ch] / 2;
            high = ch;
            low = ch;
            for (int i = ch + 1; i < Data.Length; i++)
            {
                if (Data[i] == value)
                {
                    high = i;
                    lresult = true;
                    break;
                }
                if (Data[i] < value)
                {
                    high = i;
                    break;
                }
            }
            for (int i = ch - 1; i > 0; i--)
            {
                if (Data[i] == value)
                {
                    low = i;
                    rresult = true;
                    break;
                }
                if (Data[i] <= value)
                {
                    low = i;
                    break;
                }
            }
            return ch;
        }


        /// 找谱的理论最高点通道,以及峰的两个边界
        /// </summary>
        /// <returns></returns>
        public static Double FitChannOfMaxValue(int begin, int end, int[] Data, ref int low, ref int high)
        {
            int ch = GetHighSpecChannel(begin, end, Data);
            int value = Data[ch] / 2;
            high = ch;
            low = ch;
            for (int i = ch + 1; i < Data.Length; i++)
            {
                if (Data[/*high*/i] <= value)
                {
                    high = i;
                    break;
                }
            }
            for (int i = ch - 1; i > 0; i--)
            {
                if (Data[/*low*/i] <= value)
                {
                    low = i;
                    break;
                }
            }
            double pa = 0;
            double pb = 0;
            for (int i = low; i <= high; i++)
            {
                pa += (double)Data[i] * i;
                pb += (double)Data[i];
            }
            if (pb <= 0)
            {
                return 0;
            }
            else
            {
                return pa * 1.0 / pb;
            }
        }

        /// 高斯算法找谱的理论最高点的值,以及峰的两个边界
        /// </summary>
        /// <returns></returns>
        public static Double GuassValueOfMaxValue(int begin, int end, int[] Data, ref int low, ref int high)
        {
            int ch = GetHighSpecChannel(begin, end, Data);
            int value = Data[ch] / 2;
            high = ch;
            low = ch;
            for (int i = ch + 1; i < Data.Length; i++)
            {
                if (Data[/*high*/i] <= value)
                {
                    high = i;
                    break;
                }
            }
            for (int i = ch - 1; i > 0; i--)
            {
                if (Data[/*low*/i] <= value)
                {
                    low = i;
                    break;
                }
            }

            int guassend = (int)(ch + (high - ch) * 1.5f);
            int guassbegin = (int)(ch - (ch - low) * 1.5f);
            //int guassend = high;
            //int guassbegin = low;
            Double[] datas = new double[guassend - guassbegin + 1];
            double yleft = Data[guassbegin];
            double yright = Data[guassend];
            //double k1 = (yleft - yright) / (guassbegin - guassend);//(Spec.Data[left]-Spec.Data[right])*1.0/(left-right);
            //double k0 = Data[guassbegin] - k1 * guassbegin;

            for (int i = guassbegin; i <= guassend; i++)
            {
                //datas[i - guassbegin] = Data[i] - k1 * i - k0;
                //if (datas[i - guassbegin] <= 0)
                //{
                //    datas[i - guassbegin] = 0;
                //}
                //else
                //{
                //    datas[i - guassbegin] = Math.Log(datas[i - guassbegin]);
                //}
                datas[i - guassbegin] = Math.Log(Data[i]);
            }

            double[,] a = new double[guassend - guassbegin + 1, 3];
            double[] x = new double[3];
            for (int i = 0; i < datas.Length; i++)
            {
                a[i, 0] = 1;
                a[i, 1] = i + guassbegin;
                a[i, 2] = a[i, 1] * a[i, 1];
            }
            ArithDllInterface.MaxtrixEquation(datas.Length, 3, a, datas, x);
            //CMatrix.Equation(datas.Length, 3, a, datas, x);
            ////求面积
            //double ymax = x[0] - x[1] * x[1] / (4 * x[2]);
            //ymax = Math.Exp(ymax);
            //double s = ymax * Math.Sqrt(Math.PI / -x[2]);
            //double xmax=(-(x[1])/(2*x[2]));
            //double ymax = x[0] + xmax * x[1] + x[2] * xmax * xmax;
            //double FHWM = 2*Math.Sqrt(Math.Log(2) *(- 1/x[2]));
            double FHWM = 2 * Math.Sqrt(Math.Log(2) * (-1 / x[2]));
            
            return (double.IsInfinity(FHWM)||double.IsNaN(FHWM))?0:FHWM;//修正分辨率为非数字的时，数据库为null,导致历史记录的错误
        }

        /// 去本底 适用于低性能探测器 如正比计数管
        /// </summary>
        /// <param name="fSpec">保数普数据的数组</param>
        /// <param name="nBins">数组的长度</param>
        /// <param name="ampFac">通常设为1</param>
        [DllImport("fppro.dll", EntryPoint = "spBkgAutoProc2")]
        private static extern void spBkgAutoProc2(float[] fSpec, int nBins, float ampFac);
        /** 去本底 适用于高性能探测器 **/
        [DllImport("fppro.dll", EntryPoint = "spBkgAutoProc1")]
        private static extern void spBkgAutoProc1(
            float[] fSpec,      // 保存谱数据的数组
            int nBins,          // 数组的长度
            float kevch         // keV/通道
        );
        /// <summary>
        /// 去本底
        /// </summary>
        public static void RemoveBase(int[] Data,Device device)
        {
            float[] fspec = new float[Data.Length];
            int McaBin = Data.Length;
            Array.Copy(Data, fspec, Data.Length);
            switch (device.Detector.Type)
            {
                case DetectorType.Si:
                    spBkgAutoProc1(fspec, McaBin, 1);
                    break;
                case DetectorType.Xe:
                    spBkgAutoProc2(fspec, McaBin, (float)DemarcateEnergyHelp.k1);
                    break;
                default:
                    break;
            }
            for (int i = 0; i < McaBin; i++)
            {
                Data[i] = Convert.ToInt32(fspec[i]);
            }
        }

        [DllImport("fppro.dll", EntryPoint = "spEscProc")]
        private static extern void spEscProc(float[] fSpec, int nBins, float kevch, float fDetAngle, float ampFac);
        [DllImport("fppro.dll", EntryPoint = "spSumProc")]
        private static extern void spSumProc(float[] fSpec, int nBins, float kevch, int ChanOff, float ppr, float acqTime, float adjFac);
        [DllImport("fppro.dll", EntryPoint = "spSetDetectorType")]
        private static extern void spSetDetectorType(char detectorType);
        /// <summary>
        /// 去除逃逸峰
        /// </summary>
        public static void RemoveEscapePeaks(int[] Data,Device device)
        {
            switch (device.Detector.Type)
            {
                case DetectorType.Si:
                    spSetDetectorType('1');
                    break;
                case DetectorType.Xe:
                    spSetDetectorType('2');
                    break;
                default:
                    spSetDetectorType('2');
                    break;
            }
            int nBins = Data.Length;
            float[] fspec = new float[nBins];
            Array.Copy(Data, fspec, nBins);
            float kevch = (float)DemarcateEnergyHelp.k1;
            float fDetAngle = 0;
            float ampFac = 1;
            spEscProc(fspec, nBins, kevch, fDetAngle, ampFac);
            for (int i = 0; i < Data.Length; i++)
            {
                Data[i] = Convert.ToInt32(fspec[i]);
            }
        }
        /// <summary>
        /// 去除和峰
        /// </summary>
        public static void RemoveSumPeaks(int[] Data,int usedTime)
        {
            int nBins = Data.Length;
            float[] fspec = new float[nBins];
            //Data = (int[])(DataBak.Clone());
            Array.Copy(Data, fspec, nBins);
            float kevch = (float)DemarcateEnergyHelp.k1;
            //float fDetAngle = 0;
            //float ampFac = 1;
            //Globals.PopupKiller.StartKill("fppro", Skyray.Common.Info.MessageBoxCaptionTip, Skyray.Common.Info.MessageBoxTextFPSpectrumProcessException);
            spSumProc(fspec, nBins, kevch, 0, 1, usedTime, 1);
            //spEscProc(fspec, nBins, kevch, fDetAngle, ampFac);
            for (int i = 0; i < Data.Length; i++)
            {
                Data[i] = Convert.ToInt32(fspec[i]);
            }
        }

        public static double TotalArea(int left, int right, int[] Data)
        {
            double total = 0;
            for (int i = left; i <= right; i++)
            {
                total += Data[i];
            }
            return total;
        }
        /// <summary>
        /// 平滑谱
        /// </summary>
        /// <param name="fSpec">谱数据</param>
        /// <param name="nBins">谱的总通道数</param>
        [DllImport("fppro.dll", EntryPoint = "spSmoothProc")]
        private static extern void spSmoothProc(float[] fSpec, int nBins);

        /// <summary>
        /// 寻峰算法
        /// </summary>
        /// <param name="SpecData">谱数据</param>
        /// <param name="FWHM">半高宽</param>
        /// <param name="WindowWidth">窗宽</param>
        /// <param name="TRH1">灵敏度阈值(normal 2-5)</param>
        /// <param name="PVDistance">峰谷距阈值</param>
        /// <param name="AreaLimt">面积比阈值（匹配滤波后均方根值的净面积/全面积）</param>
        /// <returns></returns>
        
        public static int[] Find(double[] SpecData, double FWHM, int WindowWidth, double TRH1, double PVDistance, double AreaLimt)
        {
            if (SpecData == null)
                return null;
            if (SpecData.Length < 2 * WindowWidth + 1)
                return null;
            double[] C = new double[2 * WindowWidth + 1];//匹配滤波器系数
            double sigma = FWHM / 2.355;
            double b = 0;
            for (int i = -WindowWidth; i <= WindowWidth; i++)
            {
                b += 1.0 / (2.0 * (double)WindowWidth + 1.0) * Math.Exp(-(double)i * (double)i / (2.0 * sigma * sigma));
            }
            for (int i = -WindowWidth; i <= WindowWidth; i++)
            {
                C[i + WindowWidth] = Math.Exp(-(double)i * (double)i / (2.0 * sigma * sigma)) - b;
            }
            double[] Y0 = new double[SpecData.Length];//高斯零面积匹配滤波结果
            double[] dY0 = new double[SpecData.Length];
            double[] R = new double[SpecData.Length];
            double[] Rn = new double[SpecData.Length];//R 5点平滑结果
            double[] dRn = new double[SpecData.Length - 1];//Rn 一阶导数
            double AreaAll = 0;//Rn的全面积
            for (int i = WindowWidth; i < SpecData.Length - WindowWidth; i++)
            {
                Y0[i] = 0;
                dY0[i] = 0;
                for (int j = -WindowWidth; j <= WindowWidth; j++)
                {
                    Y0[i] += C[j + WindowWidth] * SpecData[i + j];
                    dY0[i] += C[j + WindowWidth] * C[j + WindowWidth] * SpecData[i + j];
                }
                if (dY0[i] < 0)
                    dY0[i] = 0;
                dY0[i] = Math.Sqrt(dY0[i]);
                if (dY0[i] == 0)
                    R[i] = 0;
                else
                    R[i] = Y0[i] / dY0[i];
            }
            for (int i = 2; i < R.Length - 2; i++)
            {
                if (i >= 2 && i < R.Length - 2)
                    Rn[i] = (R[i - 2] + R[i - 1] + R[i] + R[i + 1] + R[i + 2]) / 5;
                else
                    Rn[i] = R[i];
            }
            for (int i = 0; i < Rn.Length; i++)
                AreaAll += Rn[i] > 0 ? Rn[i] : 0;
            for (int i = 0; i < dRn.Length - 1; i++)
                dRn[i] = Rn[i + 1] - Rn[i];
            int[] PeakPos = new int[SpecData.Length];
            int[] ValePos = new int[SpecData.Length];
            int PNum = 0, VNum = 0;
            for (int i = 0; i < dRn.Length - 1; i++)
            {
                if (dRn[i] > 0 && dRn[i + 1] <= 0)
                {
                    PeakPos[PNum] = i + 1;
                    PNum++;
                }
                if (dRn[i] < 0 && dRn[i + 1] >= 0)
                {
                    ValePos[VNum] = i + 1;
                    VNum++;
                }
            }
            double AreaN = 0;
            int[] nPeakPos = new int[PNum];
            int PeakNum = 0;
            for (int i = 0; i < PNum; i++)
            {
                if (Rn[PeakPos[i]] >= TRH1)
                {
                    nPeakPos[PeakNum] = PeakPos[i];
                    PeakNum++;
                    continue;
                }
                else
                {
                    if (PeakPos[0] > ValePos[0])
                    {
                        double PVDis = Rn[PeakPos[i]] - Rn[ValePos[i]];
                        if (i + 1 < VNum)
                        {
                            if (PVDis < Rn[PeakPos[i]] - Rn[ValePos[i + 1]])
                                PVDis = Rn[PeakPos[i]] - Rn[ValePos[i + 1]];
                        }
                        if (PVDis < PVDistance)
                            continue;
                        else
                        {
                            int startpos = ValePos[i];
                            int endpos = i + 1 < VNum ? ValePos[i + 1] : Rn.Length - 1;
                            for (int j = startpos; j <= endpos; j++)
                            {
                                if (Rn[j] > 0)
                                    AreaN += Rn[j];
                            }
                            if (AreaN >= AreaLimt * Math.Sqrt(AreaAll))
                            {
                                nPeakPos[PeakNum] = PeakPos[i];
                                PeakNum++;
                            }
                        }
                    }
                    else
                    {
                        double PVDis = Rn[PeakPos[i]] - Rn[ValePos[i]];
                        if (i - 1 >= 0)
                        {
                            if (PVDis < Rn[PeakPos[i]] - Rn[ValePos[i - 1]])
                                PVDis = Rn[PeakPos[i]] - Rn[ValePos[i - 1]];
                        }
                        if (PVDis < PVDistance)
                            continue;
                        else
                        {
                            int startpos = i - 1 >= 0 ? ValePos[i - 1] : 0;
                            int endpos = ValePos[i];
                            for (int j = startpos; j <= endpos; j++)
                            {
                                if (Rn[j] > 0)
                                    AreaN += Rn[j];
                            }
                            if (AreaN >= AreaLimt * Math.Sqrt(AreaAll))
                            {
                                nPeakPos[PeakNum] = PeakPos[i];
                                PeakNum++;
                            }
                        }
                    }
                }
            }
            int[] PeakPostions = new int[PeakNum];
            for (int i = 0; i < PeakPostions.Length; i++)
                PeakPostions[i] = nPeakPos[i];
            return PeakPostions;
        }
         /*
        /// <summary>
        /// 寻峰算法
        /// </summary>
        /// <param name="SpecData">谱数据</param>
        /// <param name="FWHM">半高宽</param>
        /// <param name="WindowWidth">窗宽</param>
        /// <param name="TRH1">灵敏度阈值(normal 2-5)</param>
        /// <param name="PVDistance">峰谷距阈值</param>
        /// <param name="AreaLimt">面积比阈值（匹配滤波后均方根值的净面积/全面积）</param>
        /// <returns></returns>
        public static int[] Find(double[] SpecData, double FWHM, int WindowWidth, double TRH1, double PVDistance, double AreaLimt)
        {
            if (SpecData == null)
                return null;
            if (SpecData.Length < 2 * WindowWidth + 1)
                return null;
            double[] C = new double[2 * WindowWidth + 1];//匹配滤波器系数
            double sigma = FWHM / 2.355;
            double b = 0;
            for (int i = -WindowWidth; i <= WindowWidth; i++)
            {
                b += 1.0 / (2.0 * (double)WindowWidth + 1.0) * Math.Exp(-(double)i * (double)i / (2.0 * sigma * sigma));
            }
            for (int i = -WindowWidth; i <= WindowWidth; i++)
            {
                C[i + WindowWidth] = Math.Exp(-(double)i * (double)i / (2.0 * sigma * sigma)) - b;
            }
            //double[] Y0 = new double[SpecData.Length];//高斯零面积匹配滤波结果
            //double[] dY0 = new double[SpecData.Length];
            double[] R = new double[SpecData.Length];
            double[] Rn = new double[SpecData.Length];//R 5点平滑结果
            //double[] dRn = new double[SpecData.Length - 1];//Rn 一阶导数
            double AreaAll = 0;//Rn的全面积
            double y0 = 0.0;
            double dy0 = 0.0;
            for (int i = WindowWidth; i < SpecData.Length - WindowWidth; i++)
            {
                y0 = 0.0;
                dy0 = 0.0;
                for (int j = -WindowWidth; j <= WindowWidth; j++)
                {
                    y0 += C[j + WindowWidth] * SpecData[i + j];
                    dy0 += C[j + WindowWidth] * C[j + WindowWidth] * SpecData[i + j];
                }
                if (dy0 < 0)
                    dy0 = 0;
                dy0 = Math.Sqrt(dy0);
                if (dy0 == 0)
                    R[i] = 0;
                else
                    R[i] = y0 / dy0;
            }
            for (int i = 2; i < R.Length - 2; i++)
            {
                if (i >= 2 && i < R.Length - 2)
                    Rn[i] = (R[i - 2] + R[i - 1] + R[i] + R[i + 1] + R[i + 2]) / 5;
                else
                    Rn[i] = R[i];
            }
            //后面都没有用R[],可以用R[]代替dR[]
            for (int i = 0; i < Rn.Length; i++)
                AreaAll += Rn[i] > 0 ? Rn[i] : 0;
            for (int i = 0; i < R.Length - 2; i++)
                R[i] = Rn[i + 1] - Rn[i];//将R[]作为一阶导存储，只用前Length - 2个数组

            int[] PeakPos = new int[SpecData.Length];
            int[] ValePos = new int[SpecData.Length];
            int PNum = 0, VNum = 0;
            for (int i = 0; i < R.Length - 1; i++)
            {
                if (R[i] > 0 && R[i + 1] <= 0)
                {
                    PeakPos[PNum] = i + 1;
                    PNum++;
                }
                if (R[i] < 0 && R[i + 1] >= 0)
                {
                    ValePos[VNum] = i + 1;
                    VNum++;
                }
            }
            double AreaN = 0;
            int[] nPeakPos = new int[PNum];
            int PeakNum = 0;
            for (int i = 0; i < PNum; i++)
            {
                if (Rn[PeakPos[i]] >= TRH1)
                {
                    nPeakPos[PeakNum] = PeakPos[i];
                    PeakNum++;
                    continue;
                }
                else
                {
                    if (PeakPos[0] > ValePos[0])
                    {
                        double PVDis = Rn[PeakPos[i]] - Rn[ValePos[i]];
                        if (i + 1 < VNum)
                        {
                            if (PVDis < Rn[PeakPos[i]] - Rn[ValePos[i + 1]])
                                PVDis = Rn[PeakPos[i]] - Rn[ValePos[i + 1]];
                        }
                        if (PVDis < PVDistance)
                            continue;
                        else
                        {
                            int startpos = ValePos[i];
                            int endpos = i + 1 < VNum ? ValePos[i + 1] : Rn.Length - 1;
                            for (int j = startpos; j <= endpos; j++)
                            {
                                if (Rn[j] > 0)
                                    AreaN += Rn[j];
                            }
                            if (AreaN >= AreaLimt * Math.Sqrt(AreaAll))
                            {
                                nPeakPos[PeakNum] = PeakPos[i];
                                PeakNum++;
                            }
                        }
                    }
                    else
                    {
                        double PVDis = Rn[PeakPos[i]] - Rn[ValePos[i]];
                        if (i - 1 >= 0)
                        {
                            if (PVDis < Rn[PeakPos[i]] - Rn[ValePos[i - 1]])
                                PVDis = Rn[PeakPos[i]] - Rn[ValePos[i - 1]];
                        }
                        if (PVDis < PVDistance)
                            continue;
                        else
                        {
                            int startpos = i - 1 >= 0 ? ValePos[i - 1] : 0;
                            int endpos = ValePos[i];
                            for (int j = startpos; j <= endpos; j++)
                            {
                                if (Rn[j] > 0)
                                    AreaN += Rn[j];
                            }
                            if (AreaN >= AreaLimt * Math.Sqrt(AreaAll))
                            {
                                nPeakPos[PeakNum] = PeakPos[i];
                                PeakNum++;
                            }
                        }
                    }
                }
            }
            int[] PeakPostions = new int[PeakNum];
            for (int i = 0; i < PeakPostions.Length; i++)
                PeakPostions[i] = nPeakPos[i];
            return PeakPostions;
        }

        public static int[] Find(int[] SpecData, double FWHM, int WindowWidth, double TRH1, double PVDistance, double AreaLimt)
        {
            if (SpecData == null)
                return null;
            if (SpecData.Length < 2 * WindowWidth + 1)
                return null;
            double[] C = new double[2 * WindowWidth + 1];//匹配滤波器系数
            double sigma = FWHM / 2.355;
            double b = 0;
            for (int i = -WindowWidth; i <= WindowWidth; i++)
            {
                b += 1.0 / (2.0 * (double)WindowWidth + 1.0) * Math.Exp(-(double)i * (double)i / (2.0 * sigma * sigma));
            }
            for (int i = -WindowWidth; i <= WindowWidth; i++)
            {
                C[i + WindowWidth] = Math.Exp(-(double)i * (double)i / (2.0 * sigma * sigma)) - b;
            }
            //double[] Y0 = new double[SpecData.Length];//高斯零面积匹配滤波结果
            //double[] dY0 = new double[SpecData.Length];
            double[] R = new double[SpecData.Length];
            double[] Rn = new double[SpecData.Length];//R 5点平滑结果
            //double[] dRn = new double[SpecData.Length - 1];//Rn 一阶导数
            double AreaAll = 0;//Rn的全面积
            double y0 = 0.0;
            double dy0 = 0.0;
            for (int i = WindowWidth; i < SpecData.Length - WindowWidth; i++)
            {
                y0 = 0.0;
                dy0 = 0.0;
                for (int j = -WindowWidth; j <= WindowWidth; j++)
                {
                    y0 += C[j + WindowWidth] * SpecData[i + j];
                    dy0 += C[j + WindowWidth] * C[j + WindowWidth] * SpecData[i + j];
                }
                if (dy0 < 0)
                    dy0 = 0;
                dy0 = Math.Sqrt(dy0);
                if (dy0 == 0)
                    R[i] = 0;
                else
                    R[i] = y0 / dy0;
            }
            for (int i = 2; i < R.Length - 2; i++)
            {
                if (i >= 2 && i < R.Length - 2)
                    Rn[i] = (R[i - 2] + R[i - 1] + R[i] + R[i + 1] + R[i + 2]) / 5;
                else
                    Rn[i] = R[i];
            }
            //后面都没有用R[],可以用R[]代替dR[]
            for (int i = 0; i < Rn.Length; i++)
                AreaAll += Rn[i] > 0 ? Rn[i] : 0;
            for (int i = 0; i < R.Length - 2; i++)
                R[i] = Rn[i + 1] - Rn[i];//将R[]作为一阶导存储，只用前Length - 2个数组

            int[] PeakPos = new int[SpecData.Length];
            int[] ValePos = new int[SpecData.Length];
            int PNum = 0, VNum = 0;
            for (int i = 0; i < R.Length - 1; i++)
            {
                if (R[i] > 0 && R[i + 1] <= 0)
                {
                    PeakPos[PNum] = i + 1;
                    PNum++;
                }
                if (R[i] < 0 && R[i + 1] >= 0)
                {
                    ValePos[VNum] = i + 1;
                    VNum++;
                }
            }
            double AreaN = 0;
            int[] nPeakPos = new int[PNum];
            int PeakNum = 0;
            for (int i = 0; i < PNum; i++)
            {
                if (Rn[PeakPos[i]] >= TRH1)
                {
                    nPeakPos[PeakNum] = PeakPos[i];
                    PeakNum++;
                    continue;
                }
                else
                {
                    if (PeakPos[0] > ValePos[0])
                    {
                        double PVDis = Rn[PeakPos[i]] - Rn[ValePos[i]];
                        if (i + 1 < VNum)
                        {
                            if (PVDis < Rn[PeakPos[i]] - Rn[ValePos[i + 1]])
                                PVDis = Rn[PeakPos[i]] - Rn[ValePos[i + 1]];
                        }
                        if (PVDis < PVDistance)
                            continue;
                        else
                        {
                            int startpos = ValePos[i];
                            int endpos = i + 1 < VNum ? ValePos[i + 1] : Rn.Length - 1;
                            for (int j = startpos; j <= endpos; j++)
                            {
                                if (Rn[j] > 0)
                                    AreaN += Rn[j];
                            }
                            if (AreaN >= AreaLimt * Math.Sqrt(AreaAll))
                            {
                                nPeakPos[PeakNum] = PeakPos[i];
                                PeakNum++;
                            }
                        }
                    }
                    else
                    {
                        double PVDis = Rn[PeakPos[i]] - Rn[ValePos[i]];
                        if (i - 1 >= 0)
                        {
                            if (PVDis < Rn[PeakPos[i]] - Rn[ValePos[i - 1]])
                                PVDis = Rn[PeakPos[i]] - Rn[ValePos[i - 1]];
                        }
                        if (PVDis < PVDistance)
                            continue;
                        else
                        {
                            int startpos = i - 1 >= 0 ? ValePos[i - 1] : 0;
                            int endpos = ValePos[i];
                            for (int j = startpos; j <= endpos; j++)
                            {
                                if (Rn[j] > 0)
                                    AreaN += Rn[j];
                            }
                            if (AreaN >= AreaLimt * Math.Sqrt(AreaAll))
                            {
                                nPeakPos[PeakNum] = PeakPos[i];
                                PeakNum++;
                            }
                        }
                    }
                }
            }
            int[] PeakPostions = new int[PeakNum];
            for (int i = 0; i < PeakPostions.Length; i++)
                PeakPostions[i] = nPeakPos[i];
            return PeakPostions;
        }
          * */
    }
}
