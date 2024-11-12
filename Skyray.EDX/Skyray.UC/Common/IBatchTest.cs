using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Specialized;

namespace Skyray.UC
{
    public interface IBatchTest
    {
        bool IsTestNormal {get;}
        Action ActionAfterTestFinished{get;set;}
        bool StartTest(RecordInfo ri, ref string msg);
    }

    /// <summary>
    /// 此类用于外部批量导入测试参数,进行自动测量
    /// </summary>
    [Serializable]
    public class RecordInfo
    {
        //public string SampleId { get; set; }
        public string SampleName { get; set; }
        public string Supplier { get; set; }

        public HybridDictionary OtherInfoDictionary;
    }
}
