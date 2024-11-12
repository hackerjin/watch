using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.EDXRFLibrary
{
    public class FpWorkCurve
    {
        public const int MAXCC = 5;	        ///< 测试条件最多5个
        public const int MAXELT = 35;       ///< 测试元素最多35个
        public const int MAXLAYER = 6;    	///< 镀层最多6层
        public const int MAXCMP = 35;	    ///< 测试的化合物最多35个
        public const int MAXSTD = 30;	    ///< 标准样品最多30个
        public const int MAXCMPELT = 7;  	///< 一个化合物中最多包含7个元素
        public const int MAXSIDCHRS = 24;	///< 样品的ID不能超过24个字符
        public const int MAXCMPCHRS = 18;	///< 化合物的名称不能超过18个字符
        public const int MAXELTS = 25;	    ///< 去卷积中元素不能大于25个
        public const int SPECSIZE = 4096;   ///< 通到不能大于4096
        private bool bInsert = false;
        public static double ThicknessLimit = double.MaxValue;
        public static ThickMode thickMode = ThickMode.Normal;
        /// <summary>
        /// FP的SourceData结构
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]

        internal struct FPSourceData
        {
            [MarshalAs(UnmanagedType.I4)]
            public int NumberOfConditionCode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXCC, ArraySubType = UnmanagedType.I2)]
            public short[] TargetAtomicNumber;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = MAXCC)]
            public float[] XrayTargetTakeOffAngle;       	// in Degrees
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = MAXCC)]
            public float[] TubeVoltage;               		// in kV
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = MAXCC)]
            public float[] TubeWindowThickness;       	// in millimeters
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = MAXCC)]
            public float[] XrayIncidentAngle;         		// in Degrees
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = MAXCC)]
            public float[] XrayEmergentAngle;        	 	// in Degrees
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I2, SizeConst = MAXCC)]
            public short[] PrimaryFilterAtomicNumber;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = MAXCC)]
            public float[] PrimaryFilterThickness;    		// in millimeters

            /// <summary>
            /// FPSourceData结构的构造函数
            /// </summary>
            /// <param name="useless">这个参数没有什么用途 只是用来覆盖无参数的构造器</param>        
            public FPSourceData(int numberOfConditionCode)
            {
                NumberOfConditionCode = numberOfConditionCode;
                if (numberOfConditionCode > MAXCC)
                {
                    NumberOfConditionCode = MAXCC;
                }
                TargetAtomicNumber = new short[MAXCC];
                XrayTargetTakeOffAngle = new float[MAXCC];
                TubeVoltage = new float[MAXCC];
                TubeWindowThickness = new float[MAXCC];
                XrayIncidentAngle = new float[MAXCC];
                XrayEmergentAngle = new float[MAXCC];
                PrimaryFilterAtomicNumber = new short[MAXCC];
                PrimaryFilterThickness = new float[MAXCC];
            }
        }
        /// <summary>
        /// FP的SampleData结构
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        internal struct FPSampleData
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXSIDCHRS + 1, ArraySubType = UnmanagedType.LPTStr)]
            public char[] SampleID;		//array for storing sample name

            [MarshalAs(UnmanagedType.I4)]
            public int NumberOfLayers;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXLAYER, ArraySubType = UnmanagedType.R4)]
            public float[] LayerDensity;                 	// in g/cm3

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXLAYER, ArraySubType = UnmanagedType.R4)]
            public float[] LayerThickness;               	// in microns

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXLAYER, ArraySubType = UnmanagedType.I1)]
            public sbyte[] LayerCalculationFlag;        	// 1-Calculated, 2-Fixed 

            [MarshalAs(UnmanagedType.I4)]
            public int NumberOfConstituents;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXELT * (MAXCMPCHRS + 1), ArraySubType = UnmanagedType.LPTStr)]
            public char[] ConstituentFormula;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXELT, ArraySubType = UnmanagedType.I1)]
            public sbyte[] ConstituentLayerNumber;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXELT, ArraySubType = UnmanagedType.R4)]
            public float[] ConstituentConcentration;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXELT, ArraySubType = UnmanagedType.I1)]
            public sbyte[] ConstituentConcentrationUnit;	// 1 –Weight percent, 2 - ppm

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXELT, ArraySubType = UnmanagedType.I1)]
            public sbyte[] ConstituentCalculationFlag;     	// 1-Calculated, 2-Fixed, 3-Difference, 4-Added

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXELT, ArraySubType = UnmanagedType.I2)]
            public short[] AnalyteAtomicNumber;            	// in Atomic Number

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXELT, ArraySubType = UnmanagedType.I1)]
            public sbyte[] AnalyteLine;                    	// 1-Ka,2-Kb,3-La,4-Lb,5-lr,6,Ma	

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXELT, ArraySubType = UnmanagedType.I1)]
            public sbyte[] AnalyteConditionCode;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXELT, ArraySubType = UnmanagedType.R4)]
            public float[] AnalyteIntensity;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXELT, ArraySubType = UnmanagedType.R4)]
            public float[] AnalyteBlank;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXELT, ArraySubType = UnmanagedType.R4)]
            public float[] ElementConcentration;           	// Not used for the time being.

            [MarshalAs(UnmanagedType.R4)]
            public float Asrat;

            [MarshalAs(UnmanagedType.R4)]
            public float Msthk;

            [MarshalAs(UnmanagedType.R4)]
            public float Loi;				// Additive/sample ratio, mass thickness, loss 

            // of ignition, used for bulk fused disk samples

            /// <summary>
            /// FPSampleData结构的构造函数
            /// </summary>
            /// <param name="useless">这个参数没有什么用途 只是用来覆盖无参数的构造器</param>                                          
            public FPSampleData(string useless)
            {
                SampleID = new char[MAXSIDCHRS + 1];		//array for storing sample name        
                NumberOfLayers = 1;
                LayerDensity = new float[MAXLAYER];                 	// in g/cm3        
                LayerThickness = new float[MAXLAYER];               	// in microns       
                LayerCalculationFlag = new sbyte[MAXLAYER];        	// 1-Calculated, 2-Fixed         
                NumberOfConstituents = 0;
                ConstituentFormula = new char[MAXELT * (MAXCMPCHRS + 1)];
                ConstituentLayerNumber = new sbyte[MAXELT];
                ConstituentConcentration = new float[MAXELT];
                ConstituentConcentrationUnit = new sbyte[MAXELT];	   // 1 –Weight percent, 2 - ppm       
                ConstituentCalculationFlag = new sbyte[MAXELT];     	// 1-Calculated, 2-Fixed, 3-Difference, 4-Added      
                AnalyteAtomicNumber = new short[MAXELT];            	// in Atomic Number      
                AnalyteLine = new sbyte[MAXELT];                    	// 1-Ka,2-Kb,3-La,4-Lb,5-lr,6,Ma	      
                AnalyteConditionCode = new sbyte[MAXELT];
                AnalyteIntensity = new float[MAXELT];
                AnalyteBlank = new float[MAXELT];
                ElementConcentration = new float[MAXELT];           	// Not used for the time being.         Asrat=0;
                Asrat = 0;
                Msthk = 0;
                Loi = 0;				// Additive/sample ratio, mass thickness, loss 
            }

            /// <summary>
            /// 拷贝
            /// </summary>
            /// <param name="data">把数据拷贝到Data</param>
            public void CopyTo(FPSampleData data)
            {
                data.SampleID = SampleID;
                data.NumberOfLayers = NumberOfLayers;
                LayerDensity.CopyTo(data.LayerDensity, 0);
                LayerThickness.CopyTo(data.LayerThickness, 0);
                LayerCalculationFlag.CopyTo(data.LayerCalculationFlag, 0);
                data.NumberOfConstituents = NumberOfConstituents;
                ConstituentFormula.CopyTo(data.ConstituentFormula, 0);
                ConstituentLayerNumber.CopyTo(data.ConstituentLayerNumber, 0);
                ConstituentConcentration.CopyTo(data.ConstituentConcentration, 0);
                ConstituentConcentrationUnit.CopyTo(data.ConstituentConcentrationUnit, 0);
                ConstituentCalculationFlag.CopyTo(data.ConstituentCalculationFlag, 0);
                AnalyteAtomicNumber.CopyTo(data.AnalyteAtomicNumber, 0);
                AnalyteLine.CopyTo(data.AnalyteLine, 0);            	// 1-Ka,2-Kb,3-La,4-Lb,5-lr,6,Ma	      
                AnalyteConditionCode.CopyTo(data.AnalyteConditionCode, 0);
                AnalyteIntensity.CopyTo(data.AnalyteIntensity, 0);
                AnalyteBlank.CopyTo(data.AnalyteBlank, 0);
                ElementConcentration.CopyTo(data.ElementConcentration, 0);
                data.Asrat = Asrat;
                data.Msthk = Msthk;
                data.Loi = Loi;
            }
        }

        /// <summary>
        /// 设置测试条件
        /// </summary>
        /// <param name="nCode">测试条件个数</param>
        /// <param name="src">指向fpSourceData结构的指针</param>
        /// <param name="cmpFlt">一个包含化合物名称的数组，即：[MAXCC][18]</param>
        /// <returns>设置成功，返回true，否则为false</returns>
        [DllImport("fppro.dll", EntryPoint = "fpSetSourceData")]
        private static extern bool FpSetSourceData(int nCode, ref FPSourceData src,
        [MarshalAs(UnmanagedType.AnsiBStr, SizeConst = MAXCC * MAXCMPCHRS)] string cmpFlt);

        /// <summary>
        /// 设置参与做曲线的标准样品
        /// </summary>
        /// <param name="nStds">标样个数</param>
        /// <param name="nMode">曲线类型，  1 - linear without an intercept, 2 - linear with an intercept, 3 - quadratic without an intercept, 4 - quadratic with an intercept.</param>
        /// <param name="stds">包含样品信息的fpSampleData结构数组</param>
        /// <param name="CalFileName">保存曲线信息的文件，不包含路径</param>
        /// <returns>成功返回true，否则为false</returns>
        [DllImport("fppro.dll", EntryPoint = "fpCalibrate")]
        private static extern bool FpCalibrate(int nStds, sbyte[] nMode, IntPtr pnt, [MarshalAs(UnmanagedType.LPStr)]string CalFileName);

        /// <summary>
        /// 计算样品含量和厚度
        /// </summary>
        /// <param name="smp">包含待测样品信息的FPSampleData结构</param>
        /// <param name="CalFileName">工作曲线文件名</param>
        /// <returns>成功返回true，否则为false</returns>
        [DllImport("fppro.dll", EntryPoint = "fpQuantify")]
        private static extern bool FpQuantify(IntPtr pnt, [MarshalAs(UnmanagedType.LPStr)]string CalFileName);

        /// <summary>
        /// 设置系统路径
        /// </summary>
        /// <param name="cmpFlt">路径名称</param>
        [DllImport("fppro.dll", CharSet = CharSet.Ansi, SetLastError = true, CallingConvention = CallingConvention.Winapi, EntryPoint = "SetRootPath")]
        private static extern bool SetRootPath([MarshalAs(UnmanagedType.LPStr)]string cmpFlt);




        
        /// <summary>
        /// 把系统绝对路径转换成相对路径
        /// </summary>
        public static bool FPSetRootPath(string rootPath)
        {
            return SetRootPath(rootPath);
        }

        /// <summary>
        /// 对元素安ID编号 （目前是从小到大）
        /// </summary>
        /// <returns></returns>
        private int[] SortElementID(ElementList elementList, bool IsOrder, bool IsCalContent)
        {
            //int[] sortIDs = new int[elementList.Items.Count];
            //for (int i = 0; i < elementList.Items.Count; ++i)
            //{
            //    int index = 0;
            //    for (int j = 0; j < i; j++)
            //    {
            //        if (elementList.Items[sortIDs[j]].AtomicNumber < elementList.Items[i].AtomicNumber)
            //        {
            //            index = j;
            //            break;
            //        }
            //    }
            //    for (int j = i; j > index; j--)
            //    {
            //        sortIDs[j] = sortIDs[j - 1];
            //    }
            //    sortIDs[index] = i;
            //}
            //2010-09-17排序后没法正确算厚度，被修改
            List<int> IDlist = new List<int>();
            int[] sortIDs = new int[elementList.Items.Count];
            if (IsOrder)
            {
                for (int i = 0; i < elementList.Items.Count; ++i)
                {
                    IDlist.Add(elementList.Items[i].AtomicNumber);
                }
                IDlist.Sort();
                //if (IsCalContent && elementList.RhIsLayer && IDlist.Contains(29))//铑是镀层的特殊处理
                //{
                //    IDlist.Remove(29);
                //    IDlist.Insert(0, 29);
                //}
                if (IsCalContent && elementList.RhIsLayer && elementList.LayerElemsInAnalyzer != null && elementList.LayerElemsInAnalyzer!=string.Empty)//是镀层的特殊处理
                {
                     string[] strElems = Helper.ToStrs(elementList.LayerElemsInAnalyzer);
                     int matchCount = IDlist.Count;//用来标记对比第几个。
                     int count=IDlist.Count;
                     for(int i=count;i>0;i--)
                     {
                         bool bFind = false;
                         for (int j = 0; j < elementList.Items.Count; j++)
                         {
                             if (elementList.Items[j].AtomicNumber == IDlist[matchCount - 1]&&strElems.Contains(elementList.Items[j].Caption))
                             {
                                 bFind = true;
                                 IDlist.Remove(elementList.Items[j].AtomicNumber);
                                 IDlist.Insert(0, elementList.Items[j].AtomicNumber);
                                 break;
                             }
                         }
                         if (!bFind) matchCount--;
                     }
                }
                for (int i = 0; i < IDlist.Count; ++i)
                {
                    for (int j = 0; j < elementList.Items.Count; j++)
                    {
                        if (elementList.Items[j].AtomicNumber == IDlist[i])
                        {
                            sortIDs[i] = j;
                            break;
                        }
                    }
                }
            }
            else
            {
                List<CurveElement> listOrder = elementList.Items.ToList().OrderBy(d => d.LayerNumber).ToList();
                //2010-09-17 改为按层次排序
                for (int i = 0; i < listOrder.Count; ++i)
                {
                    int index = elementList.Items.IndexOf(listOrder[i]);
                    IDlist.Add(index);
                }
                sortIDs = IDlist.ToArray();
            }
            return sortIDs;
        }
        /// <summary>
        /// 校正工作曲线,校正文件为temp.cal Type 0,为计算含量，1为计算厚度
        /// </summary>
        public bool Calibrate(ElementList elementList, int Type,out List<string> sampleNames)
        {
            //List<string> sampleNames = new List<string>();
            sampleNames = new List<string>();
            for (int i = 0; i < elementList.Items.Count; i++)
            {
                for (int j = 0; j < elementList.Items[i].Samples.Count; j++)
                {
                    if (!sampleNames.Contains(elementList.Items[i].Samples[j].SampleName) && elementList.Items[i].Samples[j].Active)
                    {
                        sampleNames.Add(elementList.Items[i].Samples[j].SampleName);
                    }
                }
            }
            int sampleCount = sampleNames.Count;
            int pureCount = 0;
            //FPSampleData[] stds = new FPSampleData[MAXSTD];
            if (Type == 0)
            {
                pureCount = 0;
            }
            else if (elementList.PureAsInfinite)
            {
                pureCount = elementList.Items.Count;
            }
            int AllSpCount = sampleCount + pureCount >= MAXSTD ? MAXSTD : sampleCount + pureCount;
            FPSampleData[] stds = new FPSampleData[AllSpCount];
            stds[0] = new FPSampleData(string.Empty);
            int stdSize = Marshal.SizeOf(stds[0]);
            //IntPtr pnt = Marshal.AllocHGlobal(stdSize * MAXSTD);
            IntPtr pnt = Marshal.AllocHGlobal(stdSize * (AllSpCount));
            pureCount = 0;
            try
            {
                int curOffset = 0;

                int[] indexs;
                if (Type == 0)
                {
                    indexs = SortElementID(elementList, true,false);
                }
                else
                {
                    indexs = SortElementID(elementList, false,false);
                }
                if (sampleCount > 0)
                {
                    for (int i = 0; i < sampleCount && i < MAXSTD; ++i)
                    {
                        stds[i] = new FPSampleData(sampleNames[i]);
                        stds[i].NumberOfConstituents = elementList.Items.Count;
                        //SampleID不支持中文，可随便取其他名字
                        //SampDatas[i, 0].SampCaption.ToCharArray().CopyTo(stds[i].SampleID, 0);
                        //delete by chuyaqin 读取样品理论强度时没法匹配
                        i.ToString().ToCharArray().CopyTo(stds[i].SampleID, 0);
                        //sampleNames[i].ToCharArray().CopyTo(stds[i].SampleID, 0);
                        stds[i].Asrat = 0;
                        stds[i].Msthk = 0;
                        stds[i].Loi = 0;
                        for (int j = 0; j < elementList.Items.Count; ++j)
                        {
                            elementList.Items[indexs[j]].Formula.ToCharArray().CopyTo(stds[i].ConstituentFormula, j * (MAXCMPCHRS + 1));
                            if (Type == 0)
                            {
                                stds[i].ConstituentLayerNumber[j] = 1;
                            }
                            else stds[i].ConstituentLayerNumber[j] = (sbyte)(elementList.Items[indexs[j]].LayerNumber);
                            stds[i].ConstituentConcentrationUnit[j] = (sbyte)1;//(sbyte)elementList.Items[indexs[j]].ContentUnit 单位固定为%，厚度单位出来的都是um
                            stds[i].ConstituentCalculationFlag[j] = (sbyte)elementList.Items[indexs[j]].Flag; //1
                            //stds[i].ConstituentCalculationFlag[j] = 1;
                            if (stds[i].ConstituentCalculationFlag[j] == 3 || Type == 1)//差额和基材含量作为计算。
                            {
                                stds[i].ConstituentCalculationFlag[j] = (sbyte)1; //1
                            }
                            stds[i].AnalyteAtomicNumber[j] = (short)elementList.Items[indexs[j]].AtomicNumber;
                            stds[i].AnalyteLine[j] = (sbyte)(elementList.Items[indexs[j]].AnalyteLine + 1);
                            stds[i].AnalyteConditionCode[j] = (sbyte)(elementList.Items[indexs[j]].ConditionID + 1);//1 
                            StandSample stdtemp = StandSample.New;
                            for (int k = 0; k < elementList.Items[indexs[j]].Samples.Count; k++)
                            {
                                if (elementList.Items[indexs[j]].Samples[k].SampleName.CompareTo(sampleNames[i]) == 0)
                                {
                                    stdtemp = elementList.Items[indexs[j]].Samples[k];
                                    break;
                                }
                            }
                            ////add by chuyaqin 2011-04-24 样品名换成id
                            //stdtemp.Id.ToString().ToCharArray().CopyTo(stds[i].SampleID, 0);
                            stds[i].AnalyteIntensity[j] = stdtemp.Active?float.Parse(stdtemp.X):0;
                            stds[i].AnalyteBlank[j] = 0;
                            stds[i].ElementConcentration[j] =stdtemp.Active?float.Parse(stdtemp.Y):0;
                            stds[i].ConstituentConcentration[j] = stds[i].ElementConcentration[j];
                            if (Type == 0)
                            {
                                stds[i].NumberOfLayers = 1;
                                stds[i].LayerDensity[0] = 0;               //(float)stdtemp.Density
                               // stds[i].LayerThickness[0] = float.Parse(stdtemp.Z);//stdtemp.Thickness
                                stds[i].LayerThickness[0] = (float)Double.PositiveInfinity;
                                stds[i].LayerCalculationFlag[0] = (sbyte)elementList.Items[indexs[j]].LayerFlag;
                            }
                            else if (stdtemp.Layer != 0)
                            {
                                //--------test
                                //stdtemp.TotalLayer = 5;
                                stds[i].NumberOfLayers = stdtemp.TotalLayer;
                                stds[i].LayerDensity[stdtemp.Layer - 1] = 0;//stdtemp.Layer - 1(float)stdtemp.Density
                                stds[i].LayerThickness[stdtemp.Layer - 1] = float.Parse(stdtemp.Z);//stdtemp.Layer - 1
                                stds[i].LayerCalculationFlag[stdtemp.Layer - 1] = (sbyte)elementList.Items[indexs[j]].LayerFlag;//stdtemp.Layer - 1
                                //stds[i].LayerCalculationFlag[stdtemp.Layer - 1] = 1;
                            }
                        }
                        //给各层的厚度赋值
                        for (int spi = 0; spi < stds[i].NumberOfLayers; spi++)
                        {
                            string spName=sampleNames[i];
                            double dbldensity = 0;
                            double dblSumY = 0;
                            double tempCal = 0;
                            for (int tempi = 0; tempi < elementList.Items.Count; tempi++)
                            {
                                if (Type == 1 && elementList.Items[tempi].LayerNumber == spi + 1)
                                {
                                    StandSample stemp = elementList.Items[tempi].Samples.First(w => w.SampleName == spName);
                                    if (stemp != null && stemp.Density > 0 && stemp.Y != null && double.Parse(stemp.Y) > 0)
                                    {
                                        tempCal += double.Parse(stemp.Y) / stemp.Density;
                                        dblSumY += double.Parse(stemp.Y);
                                    }
                                }
                                else if (Type == 0)
                                {
                                    StandSample stemp = elementList.Items[tempi].Samples.First(w => w.SampleName == spName);
                                    if (stemp != null && stemp.Density > 0 && stemp.Y != null && double.Parse(stemp.Y) > 0)
                                    {
                                        tempCal += double.Parse(stemp.Y) / stemp.Density;
                                        dblSumY += double.Parse(stemp.Y);
                                    }
                                }
                            }
                            if (tempCal > 0)
                                dbldensity = dblSumY / tempCal;
                            stds[i].LayerDensity[spi] = (float)dbldensity;
                        }
                        //把fpdata的数据考到内存
                        IntPtr pnt2 = new IntPtr(pnt.ToInt32() + curOffset);
                        Marshal.StructureToPtr(stds[i], pnt2, true);
                        curOffset += Marshal.SizeOf(stds[i]);
                    }
                }
                if (Type == 1 && elementList.PureAsInfinite)
                {
                    pureCount=elementList.Items.Count;
                    //double[] doubleIntensity;
                    //Intensity.GetThickIntensityPure(out doubleIntensity, elementList, DemarcateEnergyHelp.k0, DemarcateEnergyHelp.k1);
                    for (int i = 0; i < pureCount && sampleCount + i < MAXSTD; ++i)
                    {
                        stds[sampleCount + i] = new FPSampleData(elementList.Items[i].ElementSpecName);
                        stds[sampleCount + i].NumberOfConstituents = 1;
                        //SampleID不支持中文，可随便取其他名字
                        //SampDatas[i, 0].SampCaption.ToCharArray().CopyTo(stds[i].SampleID, 0);
                        (sampleCount + i).ToString().ToCharArray().CopyTo(stds[sampleCount + i].SampleID, 0);
                        // elementList.Items[i].ElementSpecName
                        stds[sampleCount + i].Asrat = 0;
                        stds[sampleCount + i].Msthk = 0;
                        stds[sampleCount + i].Loi = 0;
                        elementList.Items[i].Formula.ToCharArray().CopyTo(stds[sampleCount + i].ConstituentFormula, 0 * (MAXCMPCHRS + 1));
                        stds[sampleCount + i].ConstituentLayerNumber[0] = 1;//(sbyte)(elementList.Items[indexs[i]].LayerNumber)
                        stds[sampleCount + i].ConstituentConcentrationUnit[0] = (sbyte)1;//(sbyte)elementList.Items[indexs[j]].ContentUnit 单位固定为%，厚度单位出来的都是um
                        stds[sampleCount + i].ConstituentCalculationFlag[0] = (sbyte)1; //1
                        stds[sampleCount + i].AnalyteAtomicNumber[0] = (short)elementList.Items[i].AtomicNumber;
                        stds[sampleCount + i].AnalyteLine[0] = (sbyte)(elementList.Items[i].AnalyteLine + 1);
                        stds[sampleCount + i].AnalyteConditionCode[0] = (sbyte)(elementList.Items[i].ConditionID + 1);//1 
                        stds[sampleCount + i].AnalyteIntensity[0] = (float)1;//doubleIntensity[i]
                        stds[sampleCount + i].ElementConcentration[0] = (float)100;
                        stds[sampleCount + i].ConstituentConcentration[0] = stds[sampleCount + i].ElementConcentration[0];
                        stds[sampleCount + i].AnalyteBlank[0] = 0;
                        stds[sampleCount + i].NumberOfLayers = 1;
                        int AtomID = stds[sampleCount + i].AnalyteAtomicNumber[0];
                        stds[sampleCount + i].LayerDensity[0] = (float)Atoms.AtomList.Find(w => w.AtomID == AtomID).AtomDensity;//stdtemp.Layer - 1(float)0
                        stds[sampleCount + i].LayerThickness[0] = (float)Double.PositiveInfinity;
                        stds[sampleCount + i].LayerCalculationFlag[0] = (sbyte)2;//stdtemp.Layer - 1elementList.Items[i].LayerFlag
                        //把fpdata的数据考到内存
                        IntPtr pnt2 = new IntPtr(pnt.ToInt32() + curOffset);
                        Marshal.StructureToPtr(stds[sampleCount + i], pnt2, true);
                        curOffset += Marshal.SizeOf(stds[sampleCount + i]);
                        sampleNames.Add(elementList.Items[i].ElementSpecName);
                    }
                }
                if (AllSpCount <= 0)
                {
                    Marshal.FreeHGlobal(pnt);
                    return false;
                }
                sbyte[] nMode = new sbyte[MAXELT];
                for (int i = 0; i < nMode.Length; i++)
                {
                    nMode[i] = 2;//默认为过原点
                }

                for (int j = 0; j < elementList.Items.Count; ++j)
                {
                    switch (elementList.Items[indexs[j]].FpCalculationWay)
                    {
                        case FpCalculationWay.LinearWithoutAnIntercept:
                            nMode[j] = 1;
                            break;
                        case FpCalculationWay.LinearWithAnIntercept:
                            nMode[j] = 2;
                            break;
                        case FpCalculationWay.SquareWithoutAnIntercept:
                            nMode[j] = 3;
                            break;
                        case FpCalculationWay.SquareWithAnIntercept:
                            nMode[j] = 4;
                            break;
                    }
                }

               // bool b = FpCalibrate(AllSpCount, nMode, pnt, "./FpFiles/bak.cal");
                bool b = FpCalibrate(AllSpCount, nMode, pnt, "");

                //bool b = FpCalibrate(AllSpCount, nMode, pnt, "temp.cal");//fp新库调试使用
                Marshal.FreeHGlobal(pnt);
                return b;
            }
            catch (Exception)
            {
                Marshal.FreeHGlobal(pnt);
                return false;
            }

        }

        /// <summary>
        /// 校正工作曲线,校正文件为temp.cal , TypeCalibration 校正类型 0，线性 1，插值
        /// </summary>
        public bool Calibrate2(ElementList elementList, float[] layerThickness)
        {
            CurveElement ce = elementList.Items.ToList().Find(w => w.LayerNumber == 1);
            double currentThick = layerThickness[0];
            int minThickIndex = -1;
            double minThickDelta = 0;
            List<string> sampleNames = new List<string>();
            for (int j = 0; j < ce.Samples.Count; j++)
            {
                if (minThickIndex==-1||(ce.Samples[j].Active&&Math.Abs(float.Parse(ce.Samples[j].Z) - currentThick) < minThickDelta))
                {
                    minThickIndex = j;
                    minThickDelta=Math.Abs(float.Parse(ce.Samples[j].Z) - currentThick);
                }
            }
            sampleNames.Add(ce.Samples[minThickIndex].SampleName);
            int sampleCount = sampleNames.Count;
            FPSampleData[] stds = new FPSampleData[MAXSTD];
            stds[0] = new FPSampleData(string.Empty);
            int stdSize = Marshal.SizeOf(stds[0]);
            IntPtr pnt = Marshal.AllocHGlobal(stdSize * MAXSTD);
            try
            {
                int curOffset = 0;

                int[] indexs;
                indexs = SortElementID(elementList, false,false);
                for (int i = 0; i < sampleCount; ++i)
                {
                    stds[i] = new FPSampleData(sampleNames[i]);
                    stds[i].NumberOfConstituents = elementList.Items.Count;
                    //SampleID不支持中文，可随便取其他名字
                    //SampDatas[i, 0].SampCaption.ToCharArray().CopyTo(stds[i].SampleID, 0);
                    i.ToString().ToCharArray().CopyTo(stds[i].SampleID, 0);
                    stds[i].Asrat = 0;
                    stds[i].Msthk = 0;
                    stds[i].Loi = 0;
                    for (int j = 0; j < elementList.Items.Count; ++j)
                    {
                        elementList.Items[indexs[j]].Formula.ToCharArray().CopyTo(stds[i].ConstituentFormula, j * (MAXCMPCHRS + 1));
                        stds[i].ConstituentLayerNumber[j] = (sbyte)(elementList.Items[indexs[j]].LayerNumber);
                        stds[i].ConstituentConcentrationUnit[j] = 1;//(sbyte)elementList.Items[indexs[j]].ContentUnit 单位固定为%
                        //stds[i].ConstituentCalculationFlag[j] = (sbyte)elementList.Items[indexs[j]].Flag; //1
                        stds[i].ConstituentCalculationFlag[j] = 1;
                        stds[i].AnalyteAtomicNumber[j] = (short)elementList.Items[indexs[j]].AtomicNumber;
                        stds[i].AnalyteLine[j] = (sbyte)(elementList.Items[indexs[j]].AnalyteLine + 1);
                        stds[i].AnalyteConditionCode[j] = (sbyte)(elementList.Items[indexs[j]].ConditionID + 1);//1 
                        StandSample stdtemp = elementList.Items[indexs[j]].Samples.ToList().Find(w => w.SampleName == sampleNames[i]);
                        if (stdtemp == null) continue;
                        stds[i].AnalyteIntensity[j] = float.Parse(stdtemp.X);
                        stds[i].AnalyteBlank[j] = 0;
                        stds[i].ElementConcentration[j] = float.Parse(stdtemp.Y);
                        stds[i].ConstituentConcentration[j] = stds[i].ElementConcentration[j];
                        if (stdtemp.Layer != 0)
                        {
                            //stdtemp.TotalLayer = 5;
                            stds[i].NumberOfLayers = stdtemp.TotalLayer;
                            stds[i].LayerDensity[stdtemp.Layer - 1] = 0;//(float)stdtemp.Density
                            stds[i].LayerThickness[stdtemp.Layer - 1] = float.Parse(stdtemp.Z);
                            stds[i].LayerCalculationFlag[stdtemp.Layer - 1] = (sbyte)elementList.Items[indexs[j]].LayerFlag;
                        }
                    }
                    for (int spi = 0; spi < stds[i].NumberOfLayers; spi++)
                    {
                        string spName = sampleNames[i];
                        double dbldensity = 0;
                        double dblSumY = 0;
                        double tempCal = 0;
                        for (int tempi = 0; tempi < elementList.Items.Count; tempi++)
                        {
                            if (elementList.Items[tempi].LayerNumber == spi + 1)
                            {
                                StandSample stemp = elementList.Items[tempi].Samples.First(w => w.SampleName == spName);
                                if (stemp != null && stemp.Density > 0 && stemp.Y != null && double.Parse(stemp.Y) > 0)
                                {
                                    tempCal += double.Parse(stemp.Y) / stemp.Density;
                                    dblSumY += double.Parse(stemp.Y);
                                }
                            }
                        }
                        if (tempCal > 0)
                            dbldensity = dblSumY / tempCal;
                        stds[i].LayerDensity[spi] = (float)dbldensity;
                    }
                    //把fpdata的数据考到内存
                    IntPtr pnt2 = new IntPtr(pnt.ToInt32() + curOffset);
                    Marshal.StructureToPtr(stds[i], pnt2, true);
                    curOffset += Marshal.SizeOf(stds[i]);
                }
                sbyte[] nMode = new sbyte[MAXELT];
                for (int i = 0; i < nMode.Length; i++)
                {
                    nMode[i] = 1;
                }
                FpCalibrate(sampleCount, nMode, pnt, "");

                //FpCalibrate(sampleCount, nMode, pnt, "temp.cal");//fp新库调试使用
                for (int j = 0; j < elementList.Items.Count; ++j)
                {
                    switch (elementList.Items[indexs[j]].FpCalculationWay)
                    {
                        case FpCalculationWay.LinearWithoutAnIntercept:
                            nMode[j] = 1;
                            break;
                        case FpCalculationWay.LinearWithAnIntercept:
                            nMode[j] = 2;
                            break;
                        case FpCalculationWay.SquareWithoutAnIntercept:
                            nMode[j] = 3;
                            break;
                        case FpCalculationWay.SquareWithAnIntercept:
                            nMode[j] = 4;
                            break;
                    }
                }
                bool b = FpCalibrate(sampleCount, nMode, pnt, "");

                //bool b = FpCalibrate(sampleCount, nMode, pnt, "temp.cal");//fp新库调试使用
                Marshal.FreeHGlobal(pnt);
                return b;
            }
            catch (Exception)
            {
                Marshal.FreeHGlobal(pnt);
                return false;
            }

        }
        /// <summary>
        /// 计算含量和厚度 Type 0,为计算含量，1为计算厚度
        /// </summary>
        public bool CalContent(ElementList elementList, int totalLayer, float[] layerDensitys, ref float[] layerThickness, int Type, SpecListEntity speclist)
        {
            #region 消去值的处理
            if (Type == 0)//计算含量时
            {
                foreach (var ele in elementList.Items)
                {
                    if (ele.Expunction == null)
                    {
                        break;
                    }
                    double Expunctionintensity = 0;
                    switch (ele.AnalyteLine)
                    {
                        case XLine.Ka:
                            Expunctionintensity = ele.Expunction.Ka;
                            break;
                        case XLine.Kb:
                            Expunctionintensity = ele.Expunction.Kb;
                            break;
                        case XLine.La:
                            Expunctionintensity = ele.Expunction.La;
                            break;
                        case XLine.Lb:
                            Expunctionintensity = ele.Expunction.Lb;
                            break;
                        case XLine.Lr:
                            Expunctionintensity = ele.Expunction.Lr;
                            break;
                        case XLine.Le:
                            Expunctionintensity = ele.Expunction.Le;
                            break;
                    }
                    if (Expunctionintensity > 0)
                    {
                        if (Expunctionintensity > ele.Intensity)
                            ele.Intensity = 0;
                        else
                            ele.Intensity -= Expunctionintensity;
                    }
                }
            }
            #endregion
            //elementList.RhIsLayer = true;
            FPSampleData[] smp = new FPSampleData[2];
            for (int i = 0; i < 2; i++)
            {
                smp[i] = new FPSampleData(string.Empty);
            }
            int[] indexs; //SortElementID(elementList)
            if (Type == 0)
            {
                indexs = SortElementID(elementList, true,true);
            }
            else
            {
                indexs = SortElementID(elementList, false,true);
            }
            // int[] indexs = { 0, 1, 2 };
            smp[0].NumberOfLayers = totalLayer;
            smp[0].NumberOfConstituents = elementList.Items.Count;
            smp[0].SampleID[0] = 'a';
            smp[0].Asrat = 0;
            smp[0].Msthk = 0;
            smp[0].Loi = 0;
            for (int j = 0; j < layerDensitys.Length; ++j)
            {
                smp[0].LayerDensity[j] = (float)layerDensitys[j];
                //smp[0].LayerCalculationFlag[j] = (sbyte)LayerFlag.Calculated;
                smp[0].LayerThickness[j] = 0;
            }
            string[] strElemsLayer = null;
            if (elementList.RhIsLayer)
            {
                strElemsLayer = Helper.ToStrs(elementList.LayerElemsInAnalyzer == null ? "" : elementList.LayerElemsInAnalyzer);
            }
            for (int j = 0; j < elementList.Items.Count; ++j)
            {
                elementList.Items[indexs[j]].Formula.ToCharArray().CopyTo(smp[0].ConstituentFormula, j * (MAXCMPCHRS + 1));
                //铑是镀层的处理
                if (Type == 0 
                    && elementList.RhIsLayer
                   // && elementList.Items.First(w => w.Caption.ToUpper() == "CU") != null)
                    &&strElemsLayer.Length>0)
                {
                    //if (elementList.Items[indexs[j]].Caption.ToUpper().Equals("CU"))
                    if (strElemsLayer.Contains(elementList.Items[indexs[j]].Caption))
                    {
                        smp[0].LayerCalculationFlag[0] = 1;
                        smp[0].ConstituentLayerNumber[j] = 1;
                        elementList.Items[indexs[j]].LayerNumber = 1;
                    }
                    else
                    {
                        smp[0].LayerCalculationFlag[1] = 2;
                        smp[0].ConstituentLayerNumber[j] = 2;
                        elementList.Items[indexs[j]].LayerNumber = 2;
                    }
                    // smp[0].ConstituentLayerNumber[j] =2;
                    smp[0].NumberOfLayers = 2;
                    smp[0].ConstituentCalculationFlag[indexs[j]] = 1;
                }
                else if (Type == 0)
                {
                    smp[0].ConstituentLayerNumber[j] = 1;
                    elementList.Items[indexs[j]].LayerNumber = 1;
                    smp[0].ConstituentCalculationFlag[j] = (sbyte)elementList.Items[indexs[j]].Flag;
                }
                else
                {
                    smp[0].ConstituentLayerNumber[j] = (sbyte)elementList.Items[indexs[j]].LayerNumber;
                    smp[0].LayerCalculationFlag[elementList.Items[indexs[j]].LayerNumber - 1] = (sbyte)elementList.Items[indexs[j]].LayerFlag;
                    smp[0].ConstituentCalculationFlag[j] = 1;
                }
                smp[0].ConstituentConcentrationUnit[j] = 1;//(sbyte)elementList.Items[indexs[j]].ContentUnit 1单位固定为%
                //smp[0].ConstituentCalculationFlag[j] = (sbyte)elementList.Items[indexs[j]].LayerFlag;
                smp[0].AnalyteAtomicNumber[j] = (short)elementList.Items[indexs[j]].AtomicNumber;
                smp[0].AnalyteLine[j] = (sbyte)(elementList.Items[indexs[j]].AnalyteLine + 1);
                smp[0].AnalyteConditionCode[j] = (sbyte)(elementList.Items[indexs[j]].ConditionID + 1);
                smp[0].AnalyteIntensity[j] = (float)elementList.Items[indexs[j]].Intensity;
                smp[0].AnalyteBlank[j] = 0;
                smp[0].ElementConcentration[j] = 0;// (float)Elements.Item[j].Content;  
                smp[0].ConstituentConcentration[j] = 0;
            }

            //第二个样品 数据必须全部为空         
            smp[1].NumberOfLayers = 0;
            smp[1].NumberOfConstituents = 0;
            int curOffset = 0;
            IntPtr pnt = Marshal.AllocHGlobal(Marshal.SizeOf(smp[0]) * 2);
            for (int i = 0; i < 2; i++)
            {
                IntPtr pnt2 = new IntPtr(pnt.ToInt32() + curOffset);
                Marshal.StructureToPtr(smp[i], pnt2, true);
                curOffset += Marshal.SizeOf(smp[i]);
            }
            try
            {
                PopupKiller PopupKiller = new PopupKiller();
                PopupKiller.StartKill("Warnning", "", "");
                bool b = FpQuantify(pnt, "temp.cal");
                PopupKiller.EndKill();
                if (!b)
                {
                    Marshal.FreeHGlobal(pnt);
                    return false;
                }
                //把内存的数据考到smp           
                smp[0] = (FPSampleData)Marshal.PtrToStructure(pnt, typeof(FPSampleData));
                Marshal.FreeHGlobal(pnt);
            }
            catch
            {
                PopupKiller PopupKiller = new PopupKiller();
                PopupKiller.StartKill("Warnning", "", "");
                Marshal.FreeHGlobal(pnt);
                PopupKiller.EndKill();
                return false;
            }
            for (int i = 0; i < elementList.Items.Count; ++i)
            {

                elementList.Items[indexs[i]].Content = smp[0].ConstituentConcentration[i];
                //元素含量 =含量*含量系数
                elementList.Items[indexs[i]].Content = elementList.Items[indexs[i]].Content * elementList.Items[indexs[i]].Contentcoeff;
                //增加Rh是镀层,Rh的含量赋值为0，其他元素归一。
                //if (elementList.RhIsLayer && Type == 0 && elementList.Items[indexs[i]].Caption.ToUpper().Equals("RH"))
                //    elementList.Items[indexs[i]].Content = 0;

                //elementList.Items[indexs[i]].Error = 0;
                
            }
            bool bOutLimit = false;
            for (int i = 0; i < totalLayer; ++i)
            {
                layerThickness[i] = smp[0].LayerThickness[i];
                bOutLimit = bOutLimit || (smp[0].LayerThickness[i] > ThicknessLimit);
            }

            if (bOutLimit) return false;
            //add by caoyunhe 20110602
            
            //每一层归一
            //double UnitaryValue = elementList.UnitaryValue;//------------------------------------

            //XRF烧失量
            double UnitaryValue = elementList.IsUnitary ? elementList.UnitaryValue : -1;
            if (elementList.WorkCurve.FuncType == FuncType.XRF
                && elementList.IsUnitary
                && speclist != null
                && speclist.Loss > 0)
            {
                UnitaryValue = UnitaryValue * ((100 - speclist.Loss) / 100);
            }

            for (int layerCount = 0; layerCount < smp[0].NumberOfLayers; layerCount++)
            {
                //归一
                if (UnitaryValue > 0 && elementList.IsUnitary)
                {
                    double sumCont = 0;
                    for (int i = 0; i < elementList.Items.Count; ++i)
                    {
                        if (elementList.Items[i].LayerNumber == layerCount + 1)
                        {
                            if (elementList.Items[i].Content < 0 || (elementList.Items[i].LayerFlag == LayerFlag.Calculated && layerThickness[layerCount] == 0))
                            {
                                elementList.Items[i].Content = 0;
                            }
                            sumCont += elementList.Items[i].Content;
                        }
                    }
                    if (sumCont > 0)
                    {
                        sumCont = UnitaryValue / sumCont;
                    }
                    else
                    {
                        sumCont = 0;
                    }
                    for (int i = 0; i < elementList.Items.Count; ++i)
                    {
                        if (elementList.Items[i].LayerNumber == layerCount + 1)
                        {
                            elementList.Items[i].Content *= sumCont;
                        }
                    }
                }
            }
            //fp误差的修改
            for (int i = 0; i < elementList.Items.Count; ++i)
            {
                elementList.Items[i].Error *= elementList.Items[i].Content;
            }
            //记录原始含量
            double[] orginContent = new double[elementList.Items.Count];
            for (int i = 0; i < elementList.Items.Count; ++i)
            {
                orginContent[i] = elementList.Items[i].Content;
            }
            
            //每一层去做优化一和优化二
            for (int layerCount = 0; layerCount < smp[0].NumberOfLayers; layerCount++)
            {
                double sumOptimizeCont = 0;

                #region 优化1
                ////double UnitaryValue = elementList.UnitaryValue;
                ////优化1
                //for (int i = 0; i < elementList.Items.Count; ++i)
                //{
                //    if (elementList.Items[i].LayerNumber == layerCount + 1)
                //    {
                //        double value = elementList.Items[i].Content;
                //        bool hasOpt = false;
                //        for (int j = 0; j < elementList.Items[i].Optimiztion.Count; j++)
                //        {
                //            if (elementList.Items[i].Optimiztion[j].OptimizetionType == 0)
                //            {
                //                hasOpt = true;
                //                if (value - elementList.Items[i].Optimiztion[j].OptimiztionValue >= -elementList.Items[i].Optimiztion[j].OptimiztionMin
                //               && value - elementList.Items[i].Optimiztion[j].OptimiztionValue <= elementList.Items[i].Optimiztion[j].OptimiztionMax)
                //                {
                //                    value = value + (elementList.Items[i].Optimiztion[j].OptimiztionValue - value) * elementList.Items[i].Optimiztion[j].OptimiztionFactor;
                //                    elementList.Items[i].Content = value;
                //                    break;
                //                }
                //            }
                //        }
                //        if (hasOpt == true)
                //        {
                //            sumOptimizeCont += elementList.Items[i].Content;
                //        }
                //    }

                //}

                ////归一
                //if (UnitaryValue > 0 && elementList.IsUnitary && UnitaryValue > sumOptimizeCont)
                //{
                //    double sumCont = 0;
                //    sumCont = -sumOptimizeCont;
                //    for (int i = 0; i < elementList.Items.Count; ++i)
                //    {
                //        if (elementList.Items[i].LayerNumber == layerCount + 1)
                //        {
                //            if (elementList.Items[i].Content < 0 || (elementList.Items[i].LayerFlag == LayerFlag.Calculated && layerThickness[layerCount] == 0))
                //            {
                //                elementList.Items[i].Content = 0;
                //            }
                //            sumCont += elementList.Items[i].Content;
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
                //    for (int i = 0; i < elementList.Items.Count; ++i)
                //    {
                //        if (elementList.Items[i].LayerNumber != layerCount + 1 || elementList.Items[i].Optimiztion.ToList().FindAll(a => a.OptimizetionType == 0).Count > 0)
                //        {
                //            continue;
                //        }
                //        elementList.Items[i].Content *= sumCont;
                //    }
                //}
                //else if (UnitaryValue > 0 && elementList.IsUnitary && UnitaryValue <= sumOptimizeCont)
                //{
                //    for (int i = 0; i < elementList.Items.Count; ++i)
                //    {
                //        elementList.Items[i].Content = orginContent[i];
                //    }
                //}

                #endregion

                #region 优化2
                //sumOptimizeCont = 0;
                //for (int i = 0; i < elementList.Items.Count; ++i)
                //{
                //    if (elementList.Items[i].LayerNumber == layerCount + 1)
                //    {
                //        double value = elementList.Items[i].Content;
                //        bool hasOpt = false;
                //        for (int j = 0; j < elementList.Items[i].Optimiztion.Count; j++)
                //        {
                //            //if (Math.Abs(elementList.Items[i].Optimiztion[j].OptimiztionValue - value)
                //            //    < elementList.Items[i].Optimiztion[j].OptimiztionValue * elementList.Items[i].Optimiztion[j].OptimiztionRange / 100)
                //            if (elementList.Items[i].Optimiztion[j].OptimizetionType == 1)
                //            {
                //                hasOpt = true;
                //                //if (Math.Abs(elementList.Items[i].Optimiztion[j].OptimiztionValue - value)
                //                //< elementList.Items[i].Optimiztion[j].OptimiztionRange)
                //                if (value - elementList.Items[i].Optimiztion[j].OptimiztionValue >= -elementList.Items[i].Optimiztion[j].OptimiztionMin
                //                    && value - elementList.Items[i].Optimiztion[j].OptimiztionValue <= elementList.Items[i].Optimiztion[j].OptimiztionMax)
                //                {
                //                    //value = value + (elementList.Items[i].Optimiztion[j].OptimiztionValue - value) * elementList.Items[i].Optimiztion[j].OptimiztionFactor / 100;
                //                    //value = value + elementList.Items[i].Optimiztion[j].OptimiztionFactor;
                //                    value = elementList.Items[i].Optimiztion[j].OptimiztionValue;
                //                    elementList.Items[i].Content = value;
                //                    break;
                //                }
                //            }
                //        }
                //        if (hasOpt == true)
                //        {
                //            sumOptimizeCont += elementList.Items[i].Content;
                //        }
                //    }
                //}
                ////UnitaryValue = elementList.UnitaryValue;
                ////归一
                //if (UnitaryValue > 0 && elementList.IsUnitary && UnitaryValue > sumOptimizeCont)
                //{
                //    double sumCont = 0;
                //    sumCont = -sumOptimizeCont;
                //    for (int i = 0; i < elementList.Items.Count; ++i)
                //    {
                //        if (elementList.Items[i].LayerNumber == layerCount + 1)
                //        {
                //            if (elementList.Items[i].Content < 0 || (elementList.Items[i].LayerFlag == LayerFlag.Calculated && layerThickness[layerCount] == 0))
                //            {
                //                elementList.Items[i].Content = 0;
                //            }
                //            sumCont += elementList.Items[i].Content;
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
                //    for (int i = 0; i < elementList.Items.Count; ++i)
                //    {
                //        if (elementList.Items[i].LayerNumber != layerCount + 1 || elementList.Items[i].Optimiztion.ToList().FindAll(a => a.OptimizetionType == 1).Count > 0)
                //        {
                //            continue;
                //        }
                //        elementList.Items[i].Content *= sumCont;
                //    }
                //}
                //else if (UnitaryValue > 0 && elementList.IsUnitary && UnitaryValue <= sumOptimizeCont)
                //{
                //    for (int i = 0; i < elementList.Items.Count; ++i)
                //    {
                //        elementList.Items[i].Content = orginContent[i];
                //    }
                //}
                #endregion

                #region 优化3
                //sumOptimizeCont = 0;

                //for (int i = 0; i < elementList.Items.Count; ++i)
                //{
                //    if (elementList.Items[i].LayerNumber == layerCount + 1)
                //    {
                //        double value = elementList.Items[i].Content;
                //        bool hasOpt = false;
                //        for (int j = 0; j < elementList.Items[i].Optimiztion.Count; j++)
                //        {
                //            if (elementList.Items[i].Optimiztion[j].OptimizetionType == 2)
                //            {
                //                hasOpt = true;
                //                if (elementList.Items[i].Optimiztion[j].OptimiztionValue + elementList.Items[i].Optimiztion[j].OptimiztionMax >= value &&
                //                    elementList.Items[i].Optimiztion[j].OptimiztionValue - elementList.Items[i].Optimiztion[j].OptimiztionMin <= value)
                //                {
                //                    value = value + elementList.Items[i].Optimiztion[j].OptimiztionFactor;
                //                    elementList.Items[i].Content = value;
                //                    break;
                //                }
                //            }
                //        }
                //        if (hasOpt == true)
                //        {
                //            sumOptimizeCont += elementList.Items[i].Content;
                //        }
                //    }

                //}

                ////归一
                //if (UnitaryValue > 0 && elementList.IsUnitary && UnitaryValue > sumOptimizeCont)
                //{
                //    double sumCont = 0;
                //    sumCont = -sumOptimizeCont;
                //    for (int i = 0; i < elementList.Items.Count; ++i)
                //    {
                //        if (elementList.Items[i].LayerNumber == layerCount + 1)
                //        {
                //            if (elementList.Items[i].Content < 0 || (elementList.Items[i].LayerFlag == LayerFlag.Calculated && layerThickness[layerCount] == 0))
                //            {
                //                elementList.Items[i].Content = 0;
                //            }
                //            sumCont += elementList.Items[i].Content;
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
                //    for (int i = 0; i < elementList.Items.Count; ++i)
                //    {
                //        if (elementList.Items[i].LayerNumber != layerCount + 1 || elementList.Items[i].Optimiztion.ToList().FindAll(a => a.OptimizetionType == 2).Count > 0)
                //        {
                //            continue;
                //        }
                //        elementList.Items[i].Content *= sumCont;
                //    }
                //}
                //else if (UnitaryValue > 0 && elementList.IsUnitary && UnitaryValue <= sumOptimizeCont)
                //{
                //    for (int i = 0; i < elementList.Items.Count; ++i)
                //    {
                //        elementList.Items[i].Content = orginContent[i];
                //    }
                //}
                #endregion

                for (int i = 0; i < elementList.Items.Count; ++i)
                {
                    if (elementList.Items[i].LayerNumber == layerCount + 1)
                    {
                        double value = elementList.Items[i].Content;
                        bool hasOpt = false;
                        for (int j = 0; j < elementList.Items[i].Optimiztion.Count; j++)
                        {
                            if (elementList.Items[i].Optimiztion[j].OptimizetionType == 2)
                            {
                                hasOpt = true;
                                if (elementList.Items[i].Optimiztion[j].OptimiztionValue + elementList.Items[i].Optimiztion[j].OptimiztionMax >= value &&
                                    elementList.Items[i].Optimiztion[j].OptimiztionValue - elementList.Items[i].Optimiztion[j].OptimiztionMin <= value)
                                {
                                    value = value + elementList.Items[i].Optimiztion[j].OptimiztionFactor;
                                    elementList.Items[i].Content = value;
                                    break;
                                }
                            }
                            else  if (elementList.Items[i].Optimiztion[j].OptimizetionType == 0)
                            {
                                hasOpt = true;
                                if (value - elementList.Items[i].Optimiztion[j].OptimiztionValue >= -elementList.Items[i].Optimiztion[j].OptimiztionMin
                               && value - elementList.Items[i].Optimiztion[j].OptimiztionValue <= elementList.Items[i].Optimiztion[j].OptimiztionMax)
                                {
                                    value = value + (elementList.Items[i].Optimiztion[j].OptimiztionValue - value) * elementList.Items[i].Optimiztion[j].OptimiztionFactor;
                                    elementList.Items[i].Content = value;
                                    break;
                                }
                            }
                            else if (elementList.Items[i].Optimiztion[j].OptimizetionType == 1)
                            {
                                hasOpt = true;
                                if (value - elementList.Items[i].Optimiztion[j].OptimiztionValue >= -elementList.Items[i].Optimiztion[j].OptimiztionMin
                                    && value - elementList.Items[i].Optimiztion[j].OptimiztionValue <= elementList.Items[i].Optimiztion[j].OptimiztionMax)
                                {
                                    value = elementList.Items[i].Optimiztion[j].OptimiztionValue;
                                    elementList.Items[i].Content = value;
                                    break;
                                }
                            }
                            else if (elementList.Items[i].Optimiztion[j].OptimizetionType == 3 && ElementList.IsPKCatchValue)
                            {
                                hasOpt = true;
                                value = value * (1 - elementList.Items[i].Optimiztion[j].OptimiztionFactor) + elementList.Items[i].Optimiztion[j].OptimiztionValue * elementList.Items[i].Optimiztion[j].OptimiztionFactor;
                                elementList.Items[i].Content = value;
                                break;
                            }
                        }
                        if (hasOpt == true)
                        {
                            sumOptimizeCont += elementList.Items[i].Content;
                        }
                    }

                }

                //归一
                if (UnitaryValue > 0 && elementList.IsUnitary && UnitaryValue > sumOptimizeCont)
                {
                    double sumCont = 0;
                    sumCont = -sumOptimizeCont;
                    for (int i = 0; i < elementList.Items.Count; ++i)
                    {
                        if (elementList.Items[i].LayerNumber == layerCount + 1)
                        {
                            if (elementList.Items[i].Content < 0 || (elementList.Items[i].LayerFlag == LayerFlag.Calculated && layerThickness[layerCount] == 0))
                            {
                                elementList.Items[i].Content = 0;
                            }
                            sumCont += elementList.Items[i].Content;
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
                    for (int i = 0; i < elementList.Items.Count; ++i)
                    {
                        if (elementList.Items[i].LayerNumber != layerCount + 1 
                            || elementList.Items[i].Optimiztion.ToList().FindAll(a => a.OptimizetionType != 3).Count > 0
                            || (elementList.Items[i].Optimiztion.ToList().FindAll(a => a.OptimizetionType == 3).Count > 0 && ElementList.IsPKCatchValue))
                        {
                            continue;
                        }
                        elementList.Items[i].Content *= sumCont;
                    }
                }
                else if (UnitaryValue > 0 && elementList.IsUnitary && UnitaryValue <= sumOptimizeCont)
                {
                    for (int i = 0; i < elementList.Items.Count; ++i)
                    {
                        elementList.Items[i].Content = orginContent[i];
                    }
                }
            }
            //fpthick的插值算法加入
            //elementList.Items[0].CalculationWay = CalculationWay.Insert;测试用
            if (!bInsert)
            {
                //for (int i = 0; i < elementList.Items.Count; i++)
                //{
                //    for (int j = 0; j < elementList.Items[i].Samples.Count; j++)
                //    {
                //        if (elementList.Items[i].Samples[j].Active && elementList.Items[i].CalculationWay == CalculationWay.Insert)
                //        {
                //            bInsert = Type == 1 && bInsert == false ? true : false;
                //            break;
                //        }
                //    }
                //    if (bInsert)
                //    {
                //        break;
                //    }
                //}

                bInsert = IsInsertInThick(elementList, 1);

                if (bInsert && Type == 1)
                {
                    if (Calibrate2(elementList, layerThickness) && CalContent(elementList, totalLayer, layerDensitys, ref layerThickness, Type, speclist))
                    {
                        bInsert = false;
                        return true;
                    }
                    bInsert = false;
                    return false;
                }
            }
            bInsert = false;
            return true;
        }


        public bool IsInsertInThick(ElementList elementList, int Type)
        {
            if (Type != 1) return false;
            for (int i = 0; i < elementList.Items.Count; i++)
            {
                for (int j = 0; j < elementList.Items[i].Samples.Count; j++)
                {
                    if (elementList.Items[i].Samples[j].Active && elementList.Items[i].CalculationWay == CalculationWay.Insert)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


    
        /// <summary>
        /// 多条件测量环境
        /// </summary>
        /// <param name="condition">多条件测量环境</param>
        /// <param name="rayTube">光管类</param>
        /// <returns></returns>
        public static bool SetSourceData(Condition condition, Tubes rayTube, float HeightAngle)
        {
            FPSourceData sourceData = new FPSourceData(condition.DeviceParamList.Count);
            //int atomID = rayTube.MaterialAtomNum;
            int atomID = Atoms.AtomList.Find(wde => wde.AtomName == rayTube.Material) == null ? -1 : Atoms.AtomList.Find(wde => wde.AtomName == rayTube.Material).AtomID;
            for (int i = 0; i < condition.DeviceParamList.Count; i++)
            {
                sourceData.TargetAtomicNumber[i] = (short)rayTube.AtomNum;
                sourceData.XrayTargetTakeOffAngle[i] = (float)rayTube.Angel;
                //fp参数里面的管压设置2011-08-01 
                if (condition.DeviceParamList[i].IsFaceTubVoltage && condition.DeviceParamList[i].FaceTubVoltage!=0)
                    sourceData.TubeVoltage[i] = condition.DeviceParamList[i].FaceTubVoltage;
                else 
                    sourceData.TubeVoltage[i] = condition.DeviceParamList[i].TubVoltage;
                //sourceData.TubeVoltage[i] = condition.DeviceParamList[i].TubVoltage;
                sourceData.XrayIncidentAngle[i] = (float)rayTube.Incident;
                // 计算出射角 每一次根据谱高度参与计算
                //if (Height > 0)
                //{
                //    // float exitValue = (float)(Math.Atan((44.46 + Height) / 44.46) / Math.PI * 180);
                //  //  float exitValue = (float)(Math.Atan((19.5 + 0) / 33.2) / Math.PI * 180);

                //    float exitValue = (float)(Math.Atan((22.5 + Height) / 33.2) / Math.PI * 180);
                   
                //    exitValue = float.IsInfinity(exitValue) || float.IsNaN(exitValue) ? 45 : exitValue;
                //    sourceData.XrayEmergentAngle[i] = exitValue;//rayTube.Exit;//(float)
                //}
                //else
                //{
                //    sourceData.XrayEmergentAngle[i] = rayTube.Exit;
                //}
                sourceData.XrayEmergentAngle[i] = HeightAngle;   // 角度在外面算好
               // sourceData.XrayEmergentAngle[i] = rayTube.Exit;
               
                //sourceData.XrayEmergentAngle[i] = rayTube.Exit;//(float)
                //sourceData.TubeWindowThickness[i] = (float)rayTube.Thickness;
                //sourceData.PrimaryFilterAtomicNumber[i] = 54;
                //sourceData.PrimaryFilterThickness[i] = 0.2f;
                if (!rayTube.Material.Equals("Be"))
                {
                    sourceData.TubeWindowThickness[i] = 0;
                    sourceData.PrimaryFilterThickness[i] = (float)condition.DeviceParamList[i].Thickness;
                   
                    if (atomID > 0)
                    {
                        sourceData.PrimaryFilterAtomicNumber[i] = (short)atomID;
                    }
                    else
                    {
                        sourceData.PrimaryFilterAtomicNumber[i] = -1;
                    }
                }
                else
                {
                    sourceData.TubeWindowThickness[i] = (float)condition.DeviceParamList[i].Thickness;
                    int filterId = condition.DeviceParamList[i].FilterIdx - 1;
                    //朱庆华
                    if (condition.Device.Filter.Count > 0 && condition.Device.Filter[filterId].AtomNum > 0)
                    {
                        sourceData.PrimaryFilterAtomicNumber[i] = (short)condition.Device.Filter[filterId].AtomNum;
                    }
                    else
                    {
                        sourceData.PrimaryFilterAtomicNumber[i] = -1;
                    }
                    // sourceData.PrimaryFilterAtomicNumber[i] = 0;//已修改如果是化合物为0，否则为实际值
                    if (condition.Device.Filter.Count > 0)
                        sourceData.PrimaryFilterThickness[i] = (float)condition.Device.Filter[filterId].FilterThickness;//已修改 实际值
                }
            }

            if (!rayTube.Material.Equals("Be"))
            {
                return FpSetSourceData(condition.DeviceParamList.Count, ref sourceData, rayTube.Material);
            }
            else
            {
                return FpSetSourceData(condition.DeviceParamList.Count, ref sourceData, string.Empty);//滤光片的化学式
            }
        }


        /// <summary>
        /// 读取理论强度的
        /// </summary>
        /// <param name="elementlist"></param>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public bool ReadPLOTFile(ElementList elementlist, string strFileName,ref double[,] elemI,ref double [,] elemC ,ref List<string> elementsName)
        {
            //double[,] elemC = new double[MAXSTD, MAXELT];//理论强度
            //double[,] elemI = new double[MAXSTD, MAXELT];//实测强度
            FileStream file = new FileStream(strFileName, FileMode.Open);
            try
            {
                if (File.Exists(strFileName))
                {
                    Decoder code = Encoding.UTF8.GetDecoder();
                    Byte[] byteData = new Byte[240];
                    char[] charData = new char[240];
                    string[] standNames = new string[MAXSTD];
                    string[] element = new string[MAXELT];
                    //读元素名
                    file.Seek(12, SeekOrigin.Begin);
                    file.Read(byteData, 0, 4);
                    int standCount = byteData[0] + byteData[1] * 8;
                    int elemCount = byteData[2] + byteData[3] * 8;
                    for (int i = 0; i < elemCount; i++)
                    {
                        file.Read(byteData, 0, 5);
                        code.GetChars(byteData, 0, 5, charData, 0);
                        element[i] = new string(charData, 0, 2);
                        elementsName.Add(element[i]);
                    }
                    //读标样名
                    file.Seek(elemCount, SeekOrigin.Current);
                    for (int i = 0; i < standCount; i++)
                    {
                        file.Read(byteData, 0, MAXSIDCHRS);
                        code.GetChars(byteData, 0, MAXSIDCHRS, charData, 0);
                        standNames[i] = new string(charData, 0, MAXSIDCHRS);
                    }
                    file.Seek(standCount * elemCount, SeekOrigin.Current);
                    //读取理论强度值和实测强度值
                    byte[] iData = new byte[4];
                    byte[] cData = new byte[4];
                    standCount = standCount < elemI.GetLength(0) ? standCount : elemI.GetLength(0);
                    for (int i = 0; i < standCount; i++)
                    {
                        file.Read(byteData, 0, 8 * elemCount);
                        for (int j = 0; j < elemCount; j++)
                        {
                            for (int k = 0; k < 4; k++)
                            {
                                iData[k] = byteData[j * 8 + k];
                                cData[k] = byteData[j * 8 + 4 + k];
                            }
                            elemI[i, j] = System.BitConverter.ToSingle(iData, 0);//实测强度
                            elemC[i, j] = System.BitConverter.ToSingle(cData, 0);//元素的理论强度
                            //for (int l = 0; l < elementlist.Items.Count; l++)
                            //{
                            //    if (elementlist.Items[l].Caption.ToUpper().Equals(element[j]))
                            //    {
                            //        for (int q = 0; q < elementlist.Items[l].Samples.Count; q++)
                            //        {
                            //            //if (elementlist.Items[l].Samples[q].SampleName.Equals(standNames[i].TrimEnd('\0')))
                            //            //if (elementlist.Items[l].Samples[q].Id.ToString().Equals(standNames[i].TrimEnd('\0')))
                            //            if (elementlist.Items[l].Samples[q].X == elemI[i, j])
                            //            {
                            //                //elementlist.Items[l].Samples[q].IntensityC = elemC[i, j];
                            //                elementlist.Items[l].Samples[q].TheoryX = elemC[i, j];
                            //                break;
                            //            }
                            //        }
                            //    }
                            //}
                        }
                    }
                    //int im = 0;
                    //im = elemI.GetLength(0);
                    file.Close();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                file.Close();
                throw new Exception(ex.Message);
            }
            finally
            {
                file.Close();
            }
        }

        /// <summary>
        /// 从指定的路径下把Fp的文件拷到临时目录下
        /// </summary>
        /// <param name="sourcePath">源目录</param>
        /// <param name="destPath">目标目录</param>
        public static void CopyFPFiles(string sourcePath, string destPath)
        {
            System.IO.File.Copy(sourcePath + "N1COEF.DAT", destPath + "N1COEF.DAT", true);
            System.IO.File.Copy(sourcePath + "_FP.DAT", destPath + "_FP.DAT", true);
            System.IO.File.Copy(sourcePath + "_FTL.DAT", destPath + "_FTL.DAT", true);
            System.IO.File.Copy(sourcePath + "temp.cal", destPath + "temp.cal", true);
        }

        /// <summary>
        /// fP法中的高斯拟合
        /// </summary>
        /// <param name="spMem">
        /// spMem[SPECSIZE,0]:存放待计算的谱数据；spMem[SPEC_SIZE,1]:存放的本底数据
        /// spMem[SPECSIZE,2]:存放去本底后的数据，spMem[SPECSIZE,3]：存放拟合后的数据。
        ///</param>
        /// <param name="spSize">总的道数</param>
        /// <param name="binSize">能量/道</param>
        /// <param name="acqTime">测试时间</param>
        /// <param name="excEng">激发电压</param>
        /// <param name="sysFwhm">sysFwhm[0],能量半高宽，sysFwhm[1]，所在道的能量</param>
        /// <param name="nElt">要分析元素的个数</param>
        /// <param name="lz">
        /// lz[0]:元素序号列表，有低向高的顺序排列，lz[1]:元素线系代码列表。
        /// 线系代码： 1 - 3 : K, L, M。实际中没有用。; 4 - Ka, 5 – Kb
        /// 6 - La , 7 - Lb, 8 - Lg, 9 - Ll, 10 – Ln, 11 - Ma, 
        ///12 - Mb, 13 - Mz, 14 - Mn, 15 - Mg, 16 - Me, 17 – Md. 
        /// </param>
        /// <param name="lBound">lBound[0]:存放元素的左边界，lBound[1]:存放元素的右边界</param>
        /// <param name="x2">判断拟合好坏的一个指针</param>
        /// <param name="ints">ints[0],返回拟合强度，ints[1],返回拟合强度误差</param>
        [DllImport("fppro.dll", EntryPoint = "spDecoGauProc")]
        private static extern bool spDecoGauProc(float[] spMem, short spSize, float binSize, float acqTime,
          float excEng, float[] sysFwhm, short nElt, short[] lz, float[] lBound, ref float x2, float[] ints);

        /// <summary>
        /// 平移data中的数据 使第0道对应的能量为零
        /// </summary>
        private static void SpecDataShiftZero(int[] data, double demarcateIntercept, double demarcateSlope)
        {
            if (demarcateSlope != 0)
            {
                int shiftChann = -Convert.ToInt32(demarcateIntercept / demarcateSlope);
                if (shiftChann > 0 && shiftChann < data.Length)//左移
                {
                    for (int i = 0; i < data.Length - shiftChann; i++)
                    {

                        data[i] = data[i + shiftChann];
                    }
                    for (int i = data.Length - shiftChann; i < data.Length; i++)
                    {
                        data[i] = 0;
                    }
                }
                else if (shiftChann < 0)// 右移
                {
                    for (int i = data.Length + shiftChann; i >= -shiftChann; i--)
                    {
                        data[i] = data[i + shiftChann];
                    }
                    for (int i = 0; i > 0 && i < -shiftChann && i < data.Length; i++)
                    {
                        data[i] = 0;
                    }
                }
            }

        }

        /// <summary>
        /// 平移data中的数据 使第0道对应的能量为零
        /// </summary>
        public static void SpecDataShiftZero(float[] data, double demarcateIntercept, double demarcateSlope)
        {
            if (demarcateSlope != 0)
            {
                int shiftChann = -Convert.ToInt32(demarcateIntercept / demarcateSlope);
                if (shiftChann > 0 && shiftChann < data.Length)//左移
                {
                    for (int i = 0; i < data.Length - shiftChann; i++)
                    {

                        data[i] = data[i + shiftChann];
                    }
                    for (int i = data.Length - shiftChann; i < data.Length; i++)
                    {
                        data[i] = 0;
                    }
                }
                else if (shiftChann < 0)// 右移
                {
                    for (int i = data.Length + shiftChann; i >= -shiftChann; i--)
                    {
                        data[i] = data[i + shiftChann];
                    }
                    for (int i = 0; i > 0 && i < -shiftChann && i < data.Length; i++)
                    {
                        data[i] = 0;
                    }
                }
            }

        }
#region 未用到的接口
        ///// <summary>
        ///// fp强度校正，用于纯元素库
        ///// </summary>
        ///// <param name="ce">元素</param>
        ///// <param name="demarcateIntercept">能量截距</param>
        ///// <param name="demarcateSlope">能量斜率</param>
        ///// <param name="detector">探测器</param>
        ///// <param name="fpIntensity">返回的强度</param>
        ///// <returns>返回计算是否成功</returns>
        //public static bool CaculateIntensity(CurveElement ce, double demarcateIntercept, double demarcateSlope, Detector detector, out double[] fpIntensity)
        //{

        //    fpIntensity = new double[4];
        //    XLine lineCur = XLine.Ka;
        //    int[] data = Helper.ToInts(ce.SSpectrumData);
        //    SpecDataShiftZero(data, demarcateIntercept, demarcateSlope);
        //    float[] spMemo = new float[SPECSIZE * 4];
        //    for (int i = 0; i < data.Length; i++)
        //    {
        //        spMemo[4 * i] = (1.0f * data[i]);
        //        spMemo[4 * i + 2] = spMemo[4 * i];
        //    }
        //    float[] sysFwhm = new float[2];
        //    sysFwhm[0] = (float)detector.Fwhm;
        //    sysFwhm[1] = (float)detector.Energy;
        //    float channSize = (float)demarcateSlope * 1000;
        //    for (int j = 0; j < 4; j++)
        //    {



        //        short[] lz = new short[2 * MAXELTS];
        //        float[] lBound = new float[2 * MAXELTS];
        //        lz[0] = (short)ce.AtomicNumber;
        //        lz[MAXELTS] = (short)(lineCur + 4);
        //        lBound[0] = 0;
        //        lBound[MAXELTS] = 0;


        //        float x2 = 1.0f;
        //        float[] ints = new float[2 * MAXELTS];
        //        if (!spDecoGauProc(spMemo, (short)data.Length, channSize, 1, 50, sysFwhm, 1, lz, lBound, ref x2, ints))
        //        {
        //            return false;
        //        }
        //        //加入fpGauss 的峰背比2011-06-17
        //        double totalArea = 1.0;
        //        if (ce.PeakDivBase)
        //        {
        //            int left = ce.BaseLow;
        //            int right = ce.BaseHigh;
        //            totalArea = TotalArea(left, right, data) / 1.0;
        //        }
        //        // 取出强度值
        //        fpIntensity[j] = ints[0];
        //        fpIntensity[j] /= totalArea;
        //        lineCur++;
        //    }
        //    return true;
        //}

        ///// <summary>
        ///// Fp法高斯拟合
        ///// </summary>
        //public static double[] FPGaussPure(ElementList elementlist, double Intercept, double Slope)
        //{
        //    List<double> listdouble = new List<double>();
        //    for (int elemCount = 0; elemCount < elementlist.Items.Count; elemCount++)
        //    {
        //        if (elementlist.Items[elemCount].IntensityWay != IntensityWay.FPGauss)
        //            continue;

        //        SpecList list = SpecList.FindOne(w => w.Name == elementlist.Items[elemCount].ElementSpecName);
        //        if (list == null)
        //        {
        //            listdouble.Add(0);
        //            continue;
        //        }
        //        short[] lz = new short[2 * MAXELTS];
        //        float[] lBound = new float[2 * MAXELTS];
        //        int[] data = new int[list.Specs[elementlist.Items[elemCount].ConditionID].SpecDatas.Length];
        //        list.Specs[elementlist.Items[elemCount].ConditionID].SpecDatas.CopyTo(data, 0);
        //        SpecDataShiftZero(data, Intercept, Slope);
        //        float[] spMemo = new float[SPECSIZE * 4];
        //        for (int i = 0; i < data.Length; i++)
        //        {
        //            spMemo[4 * i] = (1.0f * data[i]);
        //            spMemo[4 * i + 2] = spMemo[4 * i];
        //        }
        //        int index = 0;
        //        lz[index] = (short)elementlist.Items[elemCount].AtomicNumber;
        //        lz[MAXELTS + index] = (short)(elementlist.Items[elemCount].AnalyteLine + 4);
        //        lBound[0] = 0;
        //        lBound[MAXELTS] = 0;
        //        float[] sysFwhm = new float[2];
        //        sysFwhm[0] = (float)170;//170
        //        sysFwhm[1] = (float)5.895;//5.895
        //        float x2 = 1.0f;
        //        float[] ints = new float[2 * MAXELTS];
        //        //spDecoGauProc(spMemo, Device.MaxChann, FPWorkCurve.ChannSize, spectrum.SampInfo.UsedTime, spectrum.DevParam.TubVoltage,
        //        //   sysFwhm, (short)elemCount, lz, lBound, ref x2,  ints);
        //        spDecoGauProc(spMemo, (short)data.Length, (float)Slope * 1000, 1, 50, sysFwhm, (short)list.Specs[elementlist.Items[elemCount].ConditionID].SpecTime, lz, lBound, ref x2, ints);

        //        listdouble.Add(ints[0]);

        //    }
        //    return listdouble.ToArray();
        //}
#endregion
        
        /// <summary>
        /// Fp法高斯拟合
        /// </summary>
        public static void FPGauss(ElementList elementlist, SpecEntity[] spec, double Intercept, double Slope)
        {

            for (int conditionId = 0; conditionId < spec.Length; conditionId++)
            {
                int countElemCur = 0;
                List<int> caculElemList = new List<int>();
                for (int j = 0; j < elementlist.Items.Count; j++)
                {
                    if (elementlist.Items[j].IntensityWay == IntensityWay.FPGauss && elementlist.Items[j].ConditionID == conditionId)
                    {
                        caculElemList.Add(j);
                        countElemCur++;
                    }
                }
                if (countElemCur <= 0)
                {
                    continue;
                }
                short[] lz = new short[2 * MAXELTS];
                float[] lBound = new float[2 * MAXELTS];
                int[] data = new int[spec[conditionId].SpecDatac.Length];
                int[] dataBAC =Helper.ToInts(spec[conditionId].SpecData);
                if (spec[conditionId].IsSmooth)
                    dataBAC = Helper.Smooth(dataBAC, SpecHelper.SmoothTimes);
                dataBAC = SpecProcess.SpectrumProc(SpecHelper.CURRENTWorkCurveTemp, dataBAC, (float)spec[conditionId].UsedTime, 2);
                spec[conditionId].SpecDatac.CopyTo(data, 0);
                SpecDataShiftZero(data, Intercept, Slope);
                SpecDataShiftZero(dataBAC, Intercept, Slope);
                float[] spMemo = new float[SPECSIZE * 4];
                for (int i = 0; i < data.Length; i++)
                {
                    //spMemo[4 * i] = (1.0f * data[i]);
                    //spMemo[4 * i + 2] = spMemo[4 * i];
                    spMemo[4 * i] = (1.0f * dataBAC[i]);
                    spMemo[4 * i + 2] = 1.0f*data[i];
                }

                for (int i = 0; i < countElemCur; ++i)
                {
                    int index = 0;
                    for (int j = 0; j < i; j++)
                    {
                        if (lz[j] < elementlist.Items[caculElemList[i]].AtomicNumber)
                        {
                            index = j;
                            break;
                        }
                    }
                    for (int j = i; j > index; j--)
                    {
                        lz[j] = lz[j - 1];
                        lz[MAXELTS + j] = lz[MAXELTS + j - 1];
                    }
                    lz[index] = (short)elementlist.Items[caculElemList[i]].AtomicNumber;
                    lz[MAXELTS + index] = (short)(elementlist.Items[caculElemList[i]].AnalyteLine + 4);
                    lBound[i] = 0;
                    lBound[MAXELTS + i] = 0;
                }
                float[] sysFwhm = new float[2];
                sysFwhm[0] = (float)SpecHelper.CurrentDevice.Detector.Fwhm;//170
                sysFwhm[1] = (float)SpecHelper.CurrentDevice.Detector.Energy;//5.895
                float x2 = 1.0f;
                float[] ints = new float[2 * MAXELTS];
                //spDecoGauProc(spMemo, Device.MaxChann, FPWorkCurve.ChannSize, spectrum.SampInfo.UsedTime, spectrum.DevParam.TubVoltage,
                //   sysFwhm, (short)elemCount, lz, lBound, ref x2,  ints);
                spDecoGauProc(spMemo, (short)data.Length, (float)Slope * 1000, (float)spec[conditionId].UsedTime, (float)spec[conditionId].TubVoltage, sysFwhm, (short)countElemCur, lz, lBound, ref x2, ints);
                 //spDecoGauProc(spMemo, (short)data.Length, (float)Slope * 1000, spec[conditionId].UsedTime, spec[conditionId].TubVoltage, sysFwhm, (short)countElemCur, lz, lBound, ref x2, ints);

                int[] data1 = new int[spec[conditionId].SpecDatac.Length];
                for (int i = 0; i < data.Length; i++)
                {
                    data1[i] = (int)spMemo[4 * i + 3];
                }

                // 取出强度值
                for (int i = 0; i < countElemCur; ++i)
                {
                    for (int j = 0; j < countElemCur; ++j)
                    {
                        if (lz[i] == elementlist.Items[caculElemList[j]].AtomicNumber&&lz[MAXELTS+i]==(short)(elementlist.Items[caculElemList[j]].AnalyteLine+4))
                        {
                            //加入fpGauss 的峰背比2011-06-17
                            double totalArea = 1.0;
                            //if (elementlist.Items[caculElemList[j]].PeakDivBase)
                            //{
                            //    int left=elementlist.Items[caculElemList[j]].BaseLow;
                            //    int right=elementlist.Items[caculElemList[j]].BaseHigh;
                            //    totalArea = TotalArea(left, right, data) / (spec[conditionId].UsedTime * 1.0);
                            //}
                            elementlist.Items[caculElemList[j]].Intensity = ints[i] / (spec[conditionId].UsedTime * 1.0);
                            elementlist.Items[caculElemList[j]].Intensity /= totalArea;
                            totalArea = TotalArea(elementlist.Items[caculElemList[j]].PeakLow, elementlist.Items[caculElemList[j]].PeakHigh, data);
                            if (totalArea != 0)
                                elementlist.Items[caculElemList[j]].Error = 1 / Math.Sqrt(totalArea);
                            else elementlist.Items[caculElemList[j]].Error = 0;//全面积求误差
                            //if (ints[i] != 0)
                            //    elementlist.Items[caculElemList[j]].Error = 1 / Math.Sqrt(ints[i] * spec[conditionId].UsedTime);
                            //else elementlist.Items[caculElemList[j]].Error = 0;
                            //elementlist.Items[caculElemList[j]].Error = ints[MAXELTS + i];
                            break;
                        }
                    }
                }
            }

        }
       

        public static int TotalArea(int left ,int right,int[] spec)
        {
            int total = 0;
            for (int i = left; i <= right;i++ )
            {
                total += spec[i];
            }
            return total;
        }
    }
}
