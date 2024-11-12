using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;
using Skyray.EDXRFLibrary.Define;

namespace Skyray.EDXRFLibrary.Define
{
    [Serializable]
    public abstract class CompanyOthersInfo : DbObjectModel<CompanyOthersInfo>
    {

        [LengthAttribute(ColLength.CompanyOthersInfoName)]
        public abstract string Name { get; set; }

        public abstract bool Display { get; set; }

        ///// <summary>
        ///// 工作曲线的ID
        ///// </summary>
        //public abstract long WorkCurveId { set; get; }

        [HasMany(OrderBy = "Id")]
        public abstract HasMany<CompanyOthersListInfo> CompanyOthersListInfo { get; set; }

        public abstract bool IsReport { get; set; }//添加的是否为在打印报表中传递信息，并且显示

        public abstract int ControlType { get; set; }//控件类型

        public abstract string ExcelModeType { get; set; }//打印模板类型

        public abstract string ExcelModeTarget { get; set; }//打印模板被替代对象

        public abstract string DefaultValue { get; set; }//默认值

        public abstract CompanyOthersInfo Init(string Name, bool Display, bool IsReport, int ControlType, string ExcelModeType, string ExcelModeTarget, string DefaultValue);

        //public abstract CompanyOthersInfo Init(string Name, bool Display, long WorkCurveId, bool IsReport, int ControlType, string ExcelModeType, string DefaultValue);
    }
    [Serializable]
    public abstract class CompanyOthersListInfo : DbObjectModel<CompanyOthersListInfo>
    {
        [BelongsTo, DbColumn("CompanyOthersInfo_Id")]
        public abstract CompanyOthersInfo CompanyOthersInfo { get; set; }

        [LengthAttribute(ColLength.CompanyOthersListInfo)]
        public abstract string ListInfo { get; set; }

        public abstract bool Display { get; set; }

        public abstract CompanyOthersListInfo Init(CompanyOthersInfo CompanyOthersInfo, string ListInfo, bool Display);
    }
    [Serializable]
    public abstract class HistoryCompanyOtherInfo : DbObjectModel<HistoryCompanyOtherInfo>
    {
        /// <summary>
        /// 工作曲线的ID
        /// </summary>
        public abstract long WorkCurveId { set; get; }

        [BelongsTo, DbColumn("History_Id")]
        public abstract HistoryRecord HistoryRecord { get; set; }

        [BelongsTo, DbColumn("CompanyOthersInfo_Id")]
        public abstract CompanyOthersInfo CompanyOthersInfo { get; set; }

        [BelongsTo, DbColumn("CompanyOthersListInfo_Id")]
        public abstract CompanyOthersListInfo CompanyOthersListInfo { get; set; }

        public abstract string ListInfo { get; set; }

        public abstract HistoryCompanyOtherInfo Init(long WorkCurveId,
            HistoryRecord HistoryRecord, 
            CompanyOthersInfo CompanyOthersInfo, 
            CompanyOthersListInfo CompanyOthersListInfo, 
            string ListInfo);
    }
}
