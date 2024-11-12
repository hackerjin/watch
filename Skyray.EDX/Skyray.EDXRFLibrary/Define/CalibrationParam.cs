
using Lephone.Data.Definition;
using Lephone.Util;
using System;


namespace Skyray.EDXRFLibrary
{
    [Serializable]
    public class Auto : Attribute
    {
        public string Key { get; set; }
        public string Text { get; set; }
        public Auto()
        { 
        }
        public Auto(string text)
        {
            Text = text;
        }
        public Auto(string key, string text)
        {
            Key = key;
            Text = text;
        }
        public override string ToString()
        {
            //return this.GetType().FullName + "." + Key;
            return Key;
        }
    }
    [Serializable]
    [Auto("谱处理参数设置")]
    public abstract class CalibrationParam : DbObjectModel<CalibrationParam>,ICloneable
    {
        [BelongsTo, DbColumn("WorkCurve_Id")]
        public abstract WorkCurve WorkCurve { get; set; }

        
        [Auto("角度")]
        public abstract double EscapeAngle { get; set; }
        [Auto("因子")]
        public abstract double EscapeFactor { get; set; }
        [Auto("逃逸峰处理")]
        public abstract bool IsEscapePeakProcess { get; set; }

        [Auto("脉冲对分辨率(ms)")]
        public abstract double PulseResolution { get; set; }//脉冲对分辨率(ms)
        [Auto("因子")]
        public abstract double SumFactor { get; set; }
        [Auto("和峰处理")]
        public abstract bool IsSumPeakProcess { get; set; }
        
        [Auto("次数")]
        public abstract int RemoveFirstTimes { get; set; }
        [Auto("因子")]
        public abstract double RemoveFirstFactor { get; set; }
        [Auto("扣本底1(高分辨率)")]
        public abstract bool IsRemoveBackGroundOne { get; set; }
        
        [Auto("次数")]
        public abstract int RemoveSecondTimes { get; set; }
        [Auto("因子")]
        public abstract double RemoveSecondFactor { get; set; }
        [Auto("扣本底2(低分辨率)")]
        public abstract bool IsRemoveBackGroundTwo { get; set; }
        
        [Auto("背景点(逗号隔开)")]
        public abstract string BackGroundPoint { get; set; }//背景点(用,分割,参考(1,2,3))
        [Auto("扣本底3(折线法)")]
        public abstract bool IsRemoveBackGroundThree { get; set; }

        [Auto("扣本底4(基线校正)")]
        public abstract bool IsRemoveBackGroundFour { get; set; }
        [Auto("扣本底4次数(1-3)")]
        public abstract int RemoveFourTimes { get; set; }//次数
        [Auto("扣本底4范围left")]
        public abstract int RemoveFourLeft { get; set; }//扣本底4范围left
        [Auto("扣本底4范围right")]
        public abstract int RemoveFourRight { get; set; }//扣本底4范围right

        //[Auto("扣本底5(拟合)")]
        //public abstract bool IsRemoveBackGroundFive { get; set; }
        //[Auto("扣本底5（拟合）")]
        //public abstract string BackGroundPointFive { get; set; }


        public abstract CalibrationParam Init(bool IsEscapePeakProcess,
                                            double EscapeAngle,
                                            double EscapeFactor,
                                            bool IsSumPeakProcess,
                                            double PulseResolution,
                                            double SumFactor,
                                            bool IsRemoveBackGroundOne,
                                            int RemoveFirstTimes,
                                            double RemoveFirstFactor,
                                            bool IsRemoveBackGroundTwo,
                                            int RemoveSecondTimes,
                                            double RemoveSecondFactor,
                                            bool IsRemoveBackGroundThree,
                                            string BackGroundPoint,
                                            bool IsRemoveBackGroundFour,
                                            int RemoveFourTimes,
                                            int RemoveFourLeft,
                                            int RemoveFourRight
            );

        #region ICloneable Members

        public object Clone()
        {
            return BaseObject.Clone(this);
        }

        #endregion
    }

    [Serializable]
    [Auto("强度偏移校正")]
    public abstract class IntensityCalibration : DbObjectModel<IntensityCalibration>, ICloneable
    {
        [BelongsTo, DbColumn("WorkCurve_Id")]
        public abstract WorkCurve WorkCurve { get; set; }


        [Auto("元素")]
        public abstract string Element{ get; set; }
        [Auto("原始强度")]
        public abstract double OriginalIn { get; set; }//目前测量时的强度 高标
        [Auto("校正强度")]
        public abstract double CalibrateIn { get; set; }//做曲线时的强度  高标

        [Auto("峰左界")]
        public abstract int PeakLeft { get; set; }//选择的元素左界
        [Auto("峰右界")]
        public abstract int PeakRight { get; set; }//选择的元素右界
        [Auto("背景原始强度")]
        public abstract double OriginalBaseIn { get; set; }//选择的元素左界 低标
        [Auto("背景校正强度")]
        public abstract double CalibrateBaseIn { get; set; }//选择的元素右界 低标

        public abstract int InCalType { get; set; }//测量校正类型 1,α，2，β


        public abstract IntensityCalibration Init(string Element,
                                            double OriginalIn,  
                                            double CalibrateIn,
                                            int PeakLeft,int PeakRight,
                                            double OriginalBaseIn,  
                                            double CalibrateBaseIn,
                                            int InCalType
                                            );

        #region ICloneable Members

        public object Clone()
        {
            return BaseObject.Clone(this);
        }
        #endregion
    }
}
