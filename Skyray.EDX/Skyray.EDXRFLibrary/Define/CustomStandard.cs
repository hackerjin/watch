using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Util;
using Lephone.Data.Definition;

namespace Skyray.EDXRFLibrary
{
    [Serializable]
    public abstract class CustomStandard : DbObjectModel<CustomStandard>
    {
        [LengthAttribute(ColLength.StandardName)]
        public abstract string StandardName { get; set; }//标准名称

        public abstract bool CurrentSatadard { get; set; }//当前标准


        public abstract double TotalContentStandard { get; set; }//总标准含量

        public abstract bool IsSelectTotal { get; set; }

        public abstract double StandardThick { get; set; }//标准厚度最小值

        public abstract double StandardThickMax { get; set; } //标样厚度最大值

        [HasMany(OrderBy = "Id")]
        public abstract HasMany<StandardData> StandardDatas { get; set; }

        public abstract CustomStandard Init(string StandardName);
    }
    [Serializable]
    public abstract class StandardData : DbObjectModel<StandardData>
    {
        [BelongsTo, DbColumn("CustomStandard_Id")]
        public abstract CustomStandard CustomStandard { get; set; }

        [LengthAttribute(ColLength.CurveElementCaption), AllowNull]
        public abstract string ElementCaption { get; set; }//元素名称

        public abstract double StartStandardContent { get; set; }//起始标准含量

        public abstract double StandardContent { get; set; }//标准含量

        public abstract double StandardThick { get; set; }//标准厚度最小值

        public abstract double StandardThickMax { get; set; } //标样厚度最大值

        public abstract StandardData Init(string ElementCaption, double StandardContent, double StandardThick, double StandardThickMax);
    }
}
