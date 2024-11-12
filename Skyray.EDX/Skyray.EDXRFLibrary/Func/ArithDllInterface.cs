using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Skyray.EDXRFLibrary
{
    //曲线元素结构体// 能谱类软件合并
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct Element
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ArithDllInterface.MAXCAPTIONLENGTH)]
        public string Caption;//元素或化合物名称
        [MarshalAs(UnmanagedType.Bool)]
        public bool IsDisplay;//在计算结果和报告中是否显示结果，默认显示
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ArithDllInterface.MAXFORMULALENGTH)]
        public string Formula;//化学式
        [MarshalAs(UnmanagedType.I4)]
        public int AtomicNumber;//测量元素的原子序号
        [MarshalAs(UnmanagedType.I4)]
        public int LayerNumber;//所在的层数
        [MarshalAs(UnmanagedType.I4)]
        public int AnalyteLine;//特征线 1-k,2-k,3-L,4-L,5-L
        [MarshalAs(UnmanagedType.I4)]
        public int PeakLow;//峰左界
        [MarshalAs(UnmanagedType.I4)]
        public int PeakHigh;//峰右界
        [MarshalAs(UnmanagedType.I4)]
        public int BaseLow;//背左界
        [MarshalAs(UnmanagedType.I4)]
        public int BaseHigh;//背右界
        [MarshalAs(UnmanagedType.Bool)]
        public bool IsPeakDivBase;//是否采用峰背比
        [MarshalAs(UnmanagedType.I4)]
        public int BaseIntensityMode;//背景计算方法
        [MarshalAs(UnmanagedType.R8)]
        public double LayerDensity;//层密度
        [MarshalAs(UnmanagedType.R8)]
        public double Intensity;//强度
        [MarshalAs(UnmanagedType.R8)]
        public double Content;//含量
        [MarshalAs(UnmanagedType.R8)]
        public double Thickness;//厚度
        [MarshalAs(UnmanagedType.I4)]
        public int IntentsityMod;//强度计算方法 1-FullArea,2-NetArea,3- Reference,4FixedReferenceMode 5 Gauss,6 FixedGauss, 7 FPGauss
        [MarshalAs(UnmanagedType.Bool)]
        public bool IsIntensityCompare;//是否比较a，b系的强度
        [MarshalAs(UnmanagedType.R8)]
        public double ComparisionCoefficient;//a/b的阈值
        [MarshalAs(UnmanagedType.I4)]
        public int BPeakLow;//b峰的左边界
        [MarshalAs(UnmanagedType.I4)]
        public int BPeakHigh;//b峰的右边界
        [MarshalAs(UnmanagedType.I4)]
        public int CalculationMod;//含量计算方法 1-Insert插值 2-linear一次曲线 3 conic二次曲线  4 IntensityCorrect强度校正  5 ContentContect含量校正
        [MarshalAs(UnmanagedType.I4)]
        public int ElementMod;//元素标志 1-Calculated计算, 2-Fixed不计算, 3-Difference,差额，4-Added添加剂  5-Internal 内标法；
        //[MarshalAs(UnmanagedType.I4)]
        //public int LayerMod; //镀层标志  1-Calculated, 2-Fixed
        //[MarshalAs(UnmanagedType.I4)]
        //public int ContentMod; //含量单位 1 –%, 2 - ppm
        //[MarshalAs(UnmanagedType.I4)]
        //public int ThicknessMod;//厚度单位 1—mu  2—u’’
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ArithDllInterface.MAXFORMULALENGTH)]
        public string ReferenceElements;    //拟合元素 格式为： “元素名，元素名，元素名....”
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ArithDllInterface.MAXFORMULALENGTH)]
        public string InfluenceElements;//影响元素  格式为： “元素名，元素名，元素名....”
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R8, SizeConst = ArithDllInterface.MAXINFLUENCECOEFFICIENT)]
        public double[] InfluenceCoefficientList;//影响系数 
        [MarshalAs(UnmanagedType.I4)]
        public int InflueceCoefficientNumber;
        ////下面三个数据只有在测量粉末或液体的时候才用到
        //[MarshalAs(UnmanagedType.R8)]
        //public double Asrat;//添加剂比率
        //[MarshalAs(UnmanagedType.R8)]
        //public double Msthk;//质量厚度
        //[MarshalAs(UnmanagedType.R8)]
        //public double Loi;//灼烧损失率
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R8, SizeConst = ArithDllInterface.MAXSPECTRUMDATALENGTH)]
        public double[] SpectrumData;//纯元素普数据
        [MarshalAs(UnmanagedType.I4)]
        public int SpectrumDataLen;
        [MarshalAs(UnmanagedType.R8)]
        public double Limit; //如果测量的结果小于Limit,则含量=含量*K1+K0;
        [MarshalAs(UnmanagedType.R8)]
        public double K1;
        [MarshalAs(UnmanagedType.R8)]
        public double K0;
        [MarshalAs(UnmanagedType.R8)]
        public double Error;//误差
        [MarshalAs(UnmanagedType.R8)]
        public double ErrorK1;//计算误差的时候rohs用到的两个系数
        [MarshalAs(UnmanagedType.R8)]
        public double ErrorK0;
        [MarshalAs(UnmanagedType.I4)]
        public int ConditionCode;//测量条件序号
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ArithDllInterface.MAXREFELEMLR)]
        public string ReferenceElementsLR;  
    };
    ////优化数据结构体
    //[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    //internal struct OptimizationItem
    //{
    //    [MarshalAs(UnmanagedType.R8)]
    //    public double Value; //优化点
    //    //[MarshalAs(UnmanagedType.R8)]
    //    //public double Range; //范围（%）
    //    [MarshalAs(UnmanagedType.R8)]
    //    public double Factor; //优化因子（）
    //    [MarshalAs(UnmanagedType.I4)]
    //    public int OptimizetionType; //0一次优化，1二次优化
    //    [MarshalAs(UnmanagedType.R8)]
    //    public double RangeL; //范围（%）
    //    [MarshalAs(UnmanagedType.R8)]
    //    public double RangeR; //范围（%）
        


    //};
    ////一个元素的优化数据结构体
    //[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    //internal struct Optimization
    //{
    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ArithDllInterface.MAXCAPTIONLENGTH)]
    //    public string Text;//元素名称
    //    [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = ArithDllInterface.MAXOPTIMIZATIONITEM)]
    //    public OptimizationItem[] OptimizationList;
    //    [MarshalAs(UnmanagedType.I4)]
    //    public int OptimizationItemNumber;
    //};
    //EC标样点的信息
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct SampleRecord
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ArithDllInterface.MAXCAPTIONLENGTH)]
        public string Text;       //<元素名称  
        [MarshalAs(UnmanagedType.R8)]
        public double XValue;  //<强度值
        [MarshalAs(UnmanagedType.R8)]
        public double YValue;    //<含量值
        [MarshalAs(UnmanagedType.Bool)]
        public bool Enabled; //<数据的状态，False不参与计算
    };
    //测厚标样点的信息
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct ThSampleRecord
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ArithDllInterface.MAXFORMULALENGTH)]
        public string SampleCaption;      //标样名称
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ArithDllInterface.MAXCAPTIONLENGTH)]
        public string ElementCaption;      //<元素名称 
        [MarshalAs(UnmanagedType.I4)]
        public int Layer;//<所在层
        [MarshalAs(UnmanagedType.I4)]
        public int TotalLayer;//<总层数
        [MarshalAs(UnmanagedType.R8)]
        public double XValue;  //<强度值
        [MarshalAs(UnmanagedType.R8)]
        public double YValue;    //<含量值
        [MarshalAs(UnmanagedType.Bool)]
        public bool Enabled; //<数据的状态，False不参与计算
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct SElement
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string Caption;//元素或化合物名称
        [MarshalAs(UnmanagedType.R8)]
        public double Content;//层密度
        [MarshalAs(UnmanagedType.R8)]
        public double Error;//层密度
        [MarshalAs(UnmanagedType.R8)]
        public double Weight;//层密度

    }

    class ArithDllInterface
    {
        public const int MAXSPECTRUMDATALENGTH = 4096;//谱数据的最大值
        public const int MAXOPTIMIZATIONITEM = 30; //一个元素的优化节点个数的最大值
        public const int MAXINFLUENCECOEFFICIENT = 25; //一个元素的影响系数个数最大值
        public const int MAXFORMULALENGTH = 100;//化学式最大长度
        public const int MAXREFELEMLR = 400;//拟合元素的边界
        public const int MAXCAPTIONLENGTH = 20;//元素名称最大长度
        public const int CAPIERR_MATRIXERROR = 0x0001;//拟合错误
        public const int CAPIERR_NOERROR = 0x0000; //没有错误
        public const int CAPIERR_NOREFELEMERROR = 0x0002;//没有拟合元素
        public const int CAPIERR_LESSSAMPLEERROR = 0x0003;//标样不够，无法计算影响系数
        public const int CAPIERR_N0SAMPLEERROR = 0x0004;//未设置标准样品数据
        public const int CAPIERR_MAINELEMINTENSITYZEROERROR = 0x005;//内标元素的强度为零
        public const int CAPIERR_RESULTOVERLIMIT = 0x006;//测量结果越界
        public const int CAPIERR_NOELEM = 0x007;//没有测量元素
        public const int CAPIERR_OTHER = 0x008;//除数为0
        //设置曲线元素列表设置
        [DllImport("EDXRF.dll", EntryPoint = "MaxtrixEquation", CallingConvention = CallingConvention.StdCall)]
        internal static extern void MaxtrixEquation(int m, int n, double[,]a, double[] b,double[]x);
        //设置曲线元素列表设置
        [DllImport("EDXRF.dll", EntryPoint = "SetElementListConfigure", CallingConvention = CallingConvention.StdCall)]
        internal static extern void SetElementListConfigure([In, Out] Element[] elementarray, int arrayLen, bool rhIsLayer, double rhLayerFactor, bool rhIsMainElementInfluence);

        //设置Ec曲线的配置
        [DllImport("EDXRF.dll", EntryPoint = "SetEcCurveConfigure", CallingConvention = CallingConvention.StdCall)]
        //internal static extern void SetEcCurveConfigure(SampleRecord[,] data, int dataX, int dataY, double unitaryValue, Optimization[] optimization, int optimizationLen);
        internal static extern void SetEcCurveConfigure(SampleRecord[,] data, int dataX, int dataY, double unitaryValue);

        //设置测厚曲线的配置
        [DllImport("EDXRF.dll", EntryPoint = "SetThCurveConfigure", CallingConvention = CallingConvention.StdCall)]
        internal static extern void SetEcCurveConfigure(ThSampleRecord[] data, int dataLen);

        //设置强度计算中的Fp参数
        [DllImport("EDXRF.dll", EntryPoint = "SetIntFixedGaussParam", CallingConvention = CallingConvention.StdCall)]
        internal static extern void SetIntFixedGaussParam(ref int errorCode,double deltaCoeff);

        //取得元素列表个数
        [DllImport("EDXRF.dll", EntryPoint = "GetElementCount", CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetElementCount();

        //普通的计算强度
        [DllImport("EDXRF.dll", EntryPoint = "CaculateIntensity", CallingConvention = CallingConvention.StdCall)]
        //internal static extern bool CaculateIntensity(ref int errorCode, [In, Out]Element[] elementarray, int[,] data, int specCount, int dataLen, int[] usedTime);
        internal static extern bool CaculateIntensity(ref int errorCode, [In, Out]Element[] elementarray, int[,] data, int specCount, int dataLen, double[] usedTime);
        //Rohs计算强度
        [DllImport("EDXRF.dll", EntryPoint = "RoHSCaculateIntensity", CallingConvention = CallingConvention.StdCall)]
        //internal static extern bool RoHSCaculateIntensity(ref int errorCode, [In, Out]Element[] elementarray, int[,] data, int specCount, int dataLen, int[] usedTime,double CdSnCoeff,int SnLeft,int SnRight,int SampleType);
        internal static extern bool RoHSCaculateIntensity(ref int errorCode, [In, Out]Element[] elementarray, int[,] data, int specCount, int dataLen, double[] usedTime, double CdSnCoeff, int SnLeft, int SnRight, int SampleType);
        //测厚就算强度
        [DllImport("EDXRF.dll", EntryPoint = "ThickCalculateIntensity", CallingConvention = CallingConvention.StdCall)]
        //internal static extern bool ThickCalculateIntensity(ref int errorCode, [In, Out]Element[] elementarray, int[,] data, int specCount, int dataLen, int[] usedTime);
        internal static extern bool ThickCalculateIntensity(ref int errorCode, [In, Out]Element[] elementarray, int[,] data, int specCount, int dataLen, double[] usedTime);
        //Ec含量计算
        [DllImport("EDXRF.dll", EntryPoint = "CaculateECContent", CallingConvention = CallingConvention.StdCall)]
        internal static extern bool CaculateECContent(ref int errorCode, [In, Out]Element[] elementarray);

        //Rosh含量计算
        [DllImport("EDXRF.dll", EntryPoint = "CaculateECRoHSContent", CallingConvention = CallingConvention.StdCall)]
        internal static extern bool CaculateECRoHSContent(ref int errorCode, [In, Out]Element[] elementarray, int sampleType, double cntRatio);

        //EC强度计算系数
        [DllImport("EDXRF.dll", EntryPoint = "EcIntensityInfectedCoefsByIndex", CallingConvention = CallingConvention.StdCall)]
        internal static extern int EcIntensityInfectedCoefs(ref int errorCode, [In, Out]double[] coefs, int index);

        //EC含量计算系数
        [DllImport("EDXRF.dll", EntryPoint = "EcContentInfectedCoefsByIndex", CallingConvention = CallingConvention.StdCall)]
        internal static extern int EcContentInfectedCoefs(ref int errorCode, [In, Out]double[] coefs, int index);

        //thick
        [DllImport("EDXRF.dll", EntryPoint = "CaculateThick", CallingConvention = CallingConvention.StdCall)]
        internal static extern bool CaculateThick(ref int errorCode, [In, Out]Element[] elementarray, bool isAbsorption, ThCalculationWay intCalculationWay, double dblLimit);

        //thick
        [DllImport("EDXRF.dll", EntryPoint = "SetThCurveConfigure", CallingConvention = CallingConvention.StdCall)]
        internal static extern void SetThCurveConfigure(ThSampleRecord[] ths, int thsLen);

        //thick
        [DllImport("EDXRF.dll", EntryPoint = "CaculateThickInfectedCoefs", CallingConvention = CallingConvention.StdCall)]
        internal static extern bool CaculateThickInfectedCoefs(ref int errorCode, [In, Out]Element[] elementarray, bool isAbsorption, ThCalculationWay intCalculationWay, double dblLimit);

        //设置曲线元素列表设置
        [DllImport("GetGradeContent.dll", EntryPoint = "GetGradeContent", CallingConvention = CallingConvention.StdCall)]
        internal static extern bool GetGradeContent([In, Out] SElement[] elementarray, int arrayLen, double u);

        internal static void MessageCatch(int errCode)
        {
            switch (errCode)
            {
                case CAPIERR_NOERROR://无错
                    //System.Windows.Forms.MessageBox.Show("No ERRORS");
                    throw new Exception("No Errors!");
                case CAPIERR_MATRIXERROR://曲线拟合错误
                    throw new Exception(LibraryInfo.CAPIERR_MATRIXERROR);
                case CAPIERR_NOREFELEMERROR://没有拟合元素
                    throw new Exception(LibraryInfo.CAPIERR_NOREFELEMERROR);
                case CAPIERR_LESSSAMPLEERROR://样品不够
                    throw new Exception(LibraryInfo.CAPIERR_LESSSAMPLEERROR);
                case CAPIERR_N0SAMPLEERROR://dll未设置样品
                    throw new Exception(LibraryInfo.CAPIERR_N0SAMPLEERROR);
                case CAPIERR_MAINELEMINTENSITYZEROERROR://内标元素强度为0
                    throw new Exception(LibraryInfo.CAPIERR_MAINELEMINTENSITYZEROERROR);
                case CAPIERR_RESULTOVERLIMIT://测量结果越界
                    throw new Exception(LibraryInfo.CAPIERR_RESULTOVERLIMIT);
                case CAPIERR_NOELEM://没有测量元素
                    throw new Exception(LibraryInfo.CAPIERR_NOELEM);
                case CAPIERR_OTHER:
                    throw new Exception(LibraryInfo.CAPIERR_OTHER);
                default:
                    break;
            }
        }

    }
}
