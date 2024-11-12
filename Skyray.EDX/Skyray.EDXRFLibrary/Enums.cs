using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Skyray.EDXRFLibrary
{
    public enum MatchPlus
    {
        MatchOn,
        MatchOff
    }
    public enum TargetMode
    {
        OneTarget = 0,
        TwoTarget = 1
    }

    public enum SampleType
    {
        Test = 0,
        Standard = 1,
        Pure = 2
    }

    public enum SelectType
    {
        SampleName = 0,
        SampleType = 1,
        CurveName = 2,
        SpecType = 3
    }

    public enum SysFeatures
    {
        Start = 0,
        Init = 1,
        Energy = 2,
        Print = 3
    }

    public enum KeyModifiers
    {
        None = 0,
        Alt = 1,
        Ctrl = 2,
        Shift = 4//,
        //WindowsKey=8
    }

    public enum VacuumPumpType
    {
        Atmospheric,
        VacuumSi,
        Fixed
    }

    public enum ComType
    {
        USB,
        FPGA,
        BlueTooth,
        Parallel
    }

    public enum Pocket
    {
        PocketIII,  //手持3代
        PortableI   //便携1代
    }
    [Serializable]
    public enum FuncType
    {
        Rohs,
        XRF,
        Thick
    }
    [Serializable]
    public enum CalcType
    {
        EC,
        FP,
        PeakDivBase
    }

    public enum DllType
    {
        DLL3,
        DLL4
    }

    public enum UsbVersion
    {
        Usb1,
        Usb2
    }

    public enum XLine
    {
        Ka = 0,
        Kb = 1,
        La = 2,
        Lb = 3,
        Lr = 4,
        Le = 5
    }

    public enum OFFON
    {
        OFF = 0,
        ON = 1
    }

    public enum Rate
    {
        M20 = 20,
        M80 = 80
    }
    /// <summary>
    /// 元素标识
    /// </summary>
    public enum ElementFlag
    {
        /// <summary>
        /// 计算,  
        /// </summary>
        Calculated = 1,
        /// <summary>
        /// 不计算, 
        /// </summary>
        Fixed = 2,
        /// <summary>
        /// 差额， 
        /// </summary>
        Difference = 3,
        /// <summary>
        /// 添加剂  
        /// </summary>
        Added = 4,
        /// <summary>
        /// 内标法（主元素法）一组样品中只能包含一个主元素
        /// </summary>
        Internal = 5
    }
    /// <summary>
    /// 含量计算方法
    /// </summary>
    public enum CalculationWay
    {
        /// <summary>
        /// 插值 
        /// </summary>
        Insert = 1,
        /// <summary>
        /// 一次曲线  
        /// </summary>
        Linear = 2,
        /// <summary>
        /// 二次曲线  
        /// </summary>
        Conic = 3,
        /// <summary>
        /// 强度校正  
        /// </summary>
        IntensityCorrect = 4,
        /// <summary>
        /// 含量校正
        /// </summary>
        ContentContect = 5
    }

    /// <summary>
    /// 含量计算方法 cyq 1-一次强制过原点 2-一次不强制过原点 3-两次强制过原点  4 -两次强制不过原点
    /// </summary>
    public enum FpCalculationWay
    {
        /// <summary>
        /// 一次强制过元素点 
        /// </summary>
        LinearWithoutAnIntercept = 1,
        /// <summary>
        /// 一次不强制过元素点  
        /// </summary>
        LinearWithAnIntercept = 2,
        /// <summary>
        ///  两次强制过元素点   
        /// </summary>
        SquareWithoutAnIntercept = 3,
        /// <summary>
        /// 两次不强制过元素点  
        /// </summary>
        SquareWithAnIntercept = 4,
    }
    /// <summary>
    /// 背景强度计算方法
    /// </summary>
    public enum BaseIntensityWay
    {
        /// <summary>
        /// 全面积
        /// </summary>
        FullArea = 1,
        /// <summary>
        /// 连续谱背景
        /// </summary>
        WipeSpecialtyArea = 2
    }
    /// <summary>
    /// 强度计算方法
    /// </summary>
    public enum IntensityWay
    {
        /// <summary>
        /// 全面积
        /// </summary>
        FullArea = 1,
        /// <summary>
        /// 净面积
        /// </summary>
        NetArea = 2,
        /// <summary>
        /// 纯元素拟合
        /// </summary>
        Reference = 3,
        /// <summary>
        /// 只用峰面积内数据拟合
        /// </summary>
        FixedReference = 4,
        /// <summary>
        /// 高斯拟合
        /// </summary>
        Gauss = 5,
        /// <summary>
        /// 带本底的高斯拟合
        /// </summary>
        FixedGauss = 6,
        /// <summary>
        /// FP中高斯拟合
        /// </summary>
        FPGauss = 7
    }
    /// <summary>
    /// 镀层标志
    /// </summary>
    public enum LayerFlag
    {
        /// <summary>
        /// 计算,  
        /// </summary>
        Calculated = 1,
        /// <summary>
        /// 不计算, 
        /// </summary>
        Fixed = 2,
    }
    /// <summary>
    /// 含量单位
    /// </summary>
    public enum ContentUnit
    {
        /// <summary>
        /// 百分比
        /// </summary>
        per = 1,
        /// <summary>
        /// ppm
        /// </summary>
        ppm = 2,

        /// <summary>
        /// 千分比
        /// </summary>
        permillage = 3
    }
    /// <summary>
    /// 厚度单位
    /// </summary>
    public enum ThicknessUnit
    {
        /// <summary>
        /// 微英寸
        /// </summary>
        ur = 1,
        /// <summary>
        /// 微米
        /// </summary>
        um = 2,

        /// <summary>
        /// g/L
        /// </summary>
        gl=3,
        ///
    }
    //厚度计算方式cyq
    public enum ThCalculationWay
    {
        ThLinear = 0, //线系
        ThInsert = 1  //差值
    };

    //计算方式cyq
    public enum RohsSampleType
    {
        //[Description("Cr,Cl in Plastic")]
        //Plastic = 0,//塑料或镁铝
        //[Description("CrCdPbHg in Steel")]
        //StanlessSteel = 1, //不锈钢
        //[Description("CrCdPbHg in Brass,Zinc")]
        //Brass = 2, //黄铜
        //[Description("CrCdPbHg in Solder")]
        //Solder = 3, //焊锡
        //[Description("Polyethylene")]
        //Polyethylene = 4,
        //[Description("CrCdPbHg in Magnalium")]
        //Magnalium = 5
        CrClInPlastic = 0,//塑料或镁铝
        CrCdPbHgInSteel = 1, //不锈钢
        CrCdPbHgInBrassZinc = 2, //黄铜
        CrCdPbHgInSolder = 3, //焊锡
        Polyethylene = 4,
        CrCdPbHgInMagnalium = 5
    };

    public enum ConditionType
    {
        Normal,
        Match,
        Intelligent,
        Detection,
        Match2
    };

    public enum SpecType
    {
        StandSpec,
        PureSpec,
        UnKownSpec,
        UnSelected
    };

    public enum SpecLength
    {
        Min = 1024,
        Normal = 2048,
        Max = 4096
    };

    public enum Dp5Version
    {
        Dp5_CommonUsb = 0,
        Dp5_FastNet = 1,
        Dp5_FastUsb = 2
    };

    public enum ThickMode
    {
        Normal = 1,
        NiNi = 2,
        NiP = 3,
        Plating=4,
        NiCuNi=5,
        NiP2 = 6,
        NiCuNiFe = 7,
        NiCuNiFe2=8
    }
}

