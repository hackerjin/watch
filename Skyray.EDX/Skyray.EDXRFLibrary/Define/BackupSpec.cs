using Lephone.Data.Definition;

using Lephone.Util;

using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;

namespace Skyray.EDXRFLibrary
{
//    [Serializable]
//    public abstract partial class SpecList : DbObjectModel<SpecList>
//    {
//        /// <summary>
//        /// 所属测量条件
//        /// </summary>
//        [BelongsTo, DbColumn("Condition_Id"), Auto("Condition", "所属条件")]
//        public abstract Condition Condition { get; set; }

//        /// <summary>
//        /// 工作曲线的ID
//        /// </summary>
//        [Auto("工作曲线的ID")]
//        public abstract long WorkCurveId { set; get; }

//        /// <summary>
//        /// 包含子谱数据
//        /// </summary>
//        [HasMany(OrderBy = "Id")]
//        public abstract HasMany<Spec> Specs { get; set; }
//        /// <summary>
//        /// 谱名称
//        /// </summary>
//        [Length(ColLength.SpecListName), Auto("谱名称")]
//        public abstract string Name { get; set; }

//        [Auto("SampleName", "样品名称"), Length(ColLength.SampleName)]
//        public abstract string SampleName { get; set; }

//        [Auto("Supplier", "供应商"), Length(ColLength.SupplierName), AllowNull]
//        public abstract string Supplier { get; set; }

//        [Auto("Weight", "重量")]
//        public abstract double? Weight { get; set; }

//        [Auto("Shape", "形狀"), Length(ColLength.Shape), AllowNull]
//        public abstract string Shape { get; set; }

//        [Auto("Operater", "操作員"), Length(ColLength.Operater), AllowNull]
//        public abstract string Operater { get; set; }

//        [Auto("SpecDate", "扫谱日期")]
//        public abstract DateTime? SpecDate { get; set; }

//        [Auto("SpecSummary", "描述信息"), Length(ColLength.SpecSummary), AllowNull]
//        public abstract string SpecSummary { get; set; }

//        [Auto("谱类型")]
//        public abstract SpecType SpecType { get; set; }
//        [Auto("实际管压")]
//        public abstract double ActualVoltage { get; set; }//实际管压
//        [Auto("实际管流")]
//        public abstract double ActualCurrent { get; set; }//实际管流
//        [Auto("计数率")]
//        public abstract double CountRate { get; set; }//计数率
//        [Auto("峰通道")]
//        public abstract double PeakChannel { get; set; }//峰通道
//        [Auto("分辨率")]
//        public abstract double Resole { get; set; }//分辨率
//        [Auto("总计数")]
//        public abstract int TotalCount { get; set; }//总计数
//        [Auto("命名规则：0非规则，1为规则")]
//        public abstract int NameType { get; set; }//0非规则，1为规则


//        [AllowNull]
//        public abstract Byte[] Image { get; set; }
//        [Auto("是否显示图片")]
//        public abstract bool ImageShow { get; set; }

//        /// <summary>
//        /// 颜色
//        /// </summary>
//        [Auto("颜色")]
//        public abstract int Color { get; set; }

//        /// <summary>
//        /// 对比谱颜色
//        /// </summary>
//        [Auto("对比谱颜色")]
//        public abstract int VirtualColor { get; set; }

//        /// <summary>
//        /// 烧失量
//        /// </summary>
//        [Auto("烧失量")]
//        public abstract double Loss { get; set; }

//        public abstract SpecList Init(string Name,
//            string SampleName,
//            string Supplier,
//            double? Weight,
//            string Shape,
//            string Operater,
//            DateTime? SpecDate,
//            string SpecSummary,
//            SpecType SpecType
//            );
//    }

//    [Serializable]
//    public abstract partial class Spec : DbObjectModel<Spec>
//    {
//        /// <summary>
//        /// 所属谱列表
//        /// </summary>
//        [BelongsTo, DbColumn("SpecList_Id"), Auto("所属谱列表")]
//        public abstract SpecList SpecList { get; set; }
//        /// <summary>
//        /// 所属测试条件
//        /// </summary>
//        [BelongsTo, DbColumn("DeviceParameter_Id"), Auto("所属测试条件")]
//        public abstract DeviceParameter DeviceParameter { set; get; }
//        [Auto("是否平滑")]
//        public abstract bool IsSmooth { get; set; }
//        /// <summary>
//        /// 子谱名称
//        /// </summary>
//        [Length(ColLength.SpecName), AllowNull, Auto("子谱名称")]
//        public abstract string Name { get; set; }

//#if SqlServer
//        [Length(ColLength.SpecDataByte)]
//        public abstract byte[] SpecDataByte { get; set; }

//        private string _SpecData = string.Empty;
//        /// <summary>
//        ///子谱数据
//        /// </summary>
//        public string SpecData
//        {
//            get
//            {
//                if (_SpecData == string.Empty)
//                {
//                    _SpecData = Helper.ToString(SpecDataByte);
//                }
//                return _SpecData;
//            }
//            set
//            {
//                _SpecData = value;
//                SpecDataByte = Helper.ToBytes(_SpecData);
//            }
//        }
//#else
//        public abstract string SpecData { get; set; }
//#endif
//        //public abstract string SpecOrignialData { get; set; }

//        public int[] SpecDatas
//        {
//            get
//            {
//                if (SpecData == null)
//                {
//                    return Helper.ToInts("");
//                }
//                //if (SpecData != null && SpecOrignialData == null && !SpecHelper.IsSmoothProcessData)
//                //    SpecOrignialData = SpecData;
//                //string tempString = string.Empty;
//                //if (SpecHelper.IsSmoothProcessData)
//                //    tempString = SpecData;
//                //else
//                //    tempString = SpecOrignialData;
//                //if (tempString == null)
//                //{ return Helper.ToInts(""); }
//                int[] intSpec = Helper.ToInts(SpecData);
//                if (IsSmooth)
//                    intSpec = Helper.Smooth(intSpec, SpecHelper.SmoothTimes);
//                if (this.DeviceParameter != null && DemarcateEnergyHelp.k1 > 0)//&& UsedTime >= this.DeviceParameter.PrecTime
//                {
//                    int[] returnInt = SpectrumProc(SpecHelper.CURRENTWorkCurveTemp, intSpec);
//                    return returnInt;
//                }
//                return intSpec;
                
//            }
//        }

//        [Auto("扫谱时间")]
//        public abstract double SpecTime { get; set; }
//        [Auto("所用时间")]
//        public abstract double UsedTime { get; set; }
//        [Auto("管压")]
//        public abstract int TubVoltage { get; set; }
//        [Auto("管流")]
//        public abstract int TubCurrent { get; set; }


//        //[Auto("附加信息"), Length(ColLength.RemarkInfo)]
//        [Auto("附加信息")]
//        public abstract string RemarkInfo { get; set; }

//        public abstract Spec Init(
//            string Name,
//            string SpecData,
//            double SpecTime,
//            double UsedTime,
//            int TubVoltage,
//            int TubCurrent, string RemarkInfo);

//        public object Clone()
//        {
//            Spec cloneSpec = this.MemberwiseClone() as Spec;
//            return cloneSpec;
//        }
//    }

    [Serializable]
    public class BackUpSpecList
    {
        public string Name { get; set; }

        public string SampleName { get; set; }

        public SpecType SpecType { get; set; }

        public Byte[] Image { get; set; }

        public DateTime? SpecDate { get; set; }

        public long ConditionId { get; set; }

        //public long WorkCurveId { get; set; }
        public string ConditionName { get; set; }

        public int Color { get; set; }

        public int VirtualColor { get; set; }

        public string SpecSummary { get; set; }

        public string Supplier { get; set; }

        public double? Weight { get; set; }

        public string Shape { get; set; }

        public string Operater { get; set; }

        public List<BackUpSpec> BackUpSpecLst;

        public long originalSpecId { get; set; }

        public bool IsNewType=true; 

        public BackUpSpecList()
        {
            BackUpSpecLst = new List<BackUpSpec>();
        }
    }

    [Serializable]
    public class BackUpSpec
    {
        public string Name { get; set; }

        public string SpecData { get; set; }

        public double SpecTime { get; set; }

        public double UsedTime { get; set; }

        public int TubVoltage { get; set; }

        public int TubCurrent { get; set; }

        public long xrfchartId { get; set; }

        public byte[] XRFChart { get; set; }
    }
}
