using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;
using Skyray.EDXRFLibrary.Define;

namespace Skyray.EDXRFLibrary
{
    public abstract partial class HistoryRecord : DbObjectModel<HistoryRecord>
    {
        [Auto("SampleName", "Info.SampleName"), Length(ColLength.SampleName)]
        public abstract string SampleName { get; set; }

        [Auto("SampleName", "Info.SpecName"), Length(ColLength.SpecListName)]
        public string SpecListName { get;set; }

        [Auto("Supplier", "Info.Supplier"), Length(ColLength.SupplierName), AllowNull]
        public abstract string Supplier { get; set; }

        [Auto("Weight", "Info.Weight")]
        public abstract double Weight { get; set; }

        [Auto("Height", "Info.Height")]
        public abstract double Height { get; set; }

        [Auto("CalcAngleHeight", "Info.CalcAngleHeight")]
        public abstract double CalcAngleHeight { get; set; }

        [Auto("Shape", "Info.Shape"), Length(ColLength.Shape), AllowNull]
        public abstract string Shape { get; set; }

        [Auto("Operater", "Info.Operator"), Length(ColLength.Operater), AllowNull]
        public abstract string Operater { get; set; }

        [Auto("SpecDate", "Info.SpecDate")]
        public abstract DateTime SpecDate { get; set; }

        [Auto("SpecSummary", "Info.Description"), Length(ColLength.SpecSummary), AllowNull]
        public abstract string SpecSummary { get; set; }

        [Auto("WorkCurveId", "Info.WorkingCurve")]
        public abstract long WorkCurveId { set; get; }

        [Auto("DeviceName", "Info.Device")]
        public abstract string DeviceName { set; get; }

        [Auto("CaculateTime", "Info.CaculateTime")]
        public abstract DateTime CaculateTime { set; get; }

        [HasMany(OrderBy = "Id")]
        public abstract HasMany<HistoryElement> HistoryElement { get; set; }

        [Auto("Info.ActualVoltage")]
        public abstract double ActualVoltage { get; set; }//实际管压
        [Auto("Info.ActualCurrent")]
        public abstract double ActualCurrent { get; set; }//实际管流
        [Auto("Info.CountRate")]
        public abstract double CountRate { get; set; }//计数率
        [Auto("Info.PeakChannel")]
        public abstract double PeakChannel { get; set; }//峰通道
        [Auto("Info.Resolve")]
        public abstract double Resole { get; set; }//分辨率
        [Auto("Info.strTotalCount")]
        //public abstract int TotalCount { get; set; }//总计数
        public abstract long TotalCount { get; set; }//总计数

        [Auto("Info.HistoryRecordCode")]
        public abstract string HistoryRecordCode { get; set; }//流水号

        [AllowNull]
        public abstract string FilePath { get; set; }

        public abstract TotalEditionType EditionType { get; set; }

        public abstract bool IsScan { get; set; }//是否是扫谱记录

        public abstract int StockStatus { get; set; }//库存状态 1-入库 0-未入库
        [AllowNull]
        public abstract string Specifications { get; set; }//规格标准

        public abstract double AreaDensity { get; set; }//面密度
    }

    public enum TotalEditionType
    {
        Default=0,
        EDXRF,
        ROHS4,
        ROHS3,
        FPThick,
        Thick800A,
        XFP2,
        XRF
    }
}
