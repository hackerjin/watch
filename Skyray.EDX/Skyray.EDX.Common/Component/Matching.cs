using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDXRFLibrary;

namespace Skyray.EDX.Common
{
    /// <summary>
    /// 匹配结构
    /// </summary>
    public struct MatchEntity : IComparable<MatchEntity>
    {
        public WorkCurve workCurve; //曲线
        public double Matching;  //匹配系数

        public MatchEntity(WorkCurve worcure, double matching)
        {
            this.workCurve = worcure;
            this.Matching = matching;
        }

        public int CompareTo(MatchEntity other)
        {
            return -Matching.CompareTo(other.Matching);
        }
    }
}
