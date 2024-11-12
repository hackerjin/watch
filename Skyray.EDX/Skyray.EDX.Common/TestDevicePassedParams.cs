using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDXRFLibrary;
using Skyray.EDXRFLibrary.Spectrum;
using Skyray.EDXRFLibrary.Define;

namespace Skyray.EDX.Common
{
    public class TestDevicePassedParams 
    {

        public SpecListEntity Spec { set; get; }
        /// <summary>
        /// 匹配信息
        /// </summary>
        public bool MatchChecked { set; get; }

        /// <summary>
        /// 测量参数
        /// </summary>
        public MeasureParams MeasureParams { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public List<WordCureTest> WordCureTestList { set; get; }

        /// <summary>
        /// 谱类型
        /// </summary>
        public SpecType SpecType { set; get; }

        ///// <summary>
        ///// 保存附近信息
        ///// </summary>
        public string RemarkInformation { set; get; }


        public int MatchTime { set; get; }

        public bool IsAdditionSpec { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsAllowOpenCover { set; get; }


        public bool IsRuleName { set; get; }

        public TestDevicePassedParams(bool MatchChecked, MeasureParams MeasureParams,
                                    List<WordCureTest> WordCureTestList,
                                    bool IsAllowOpenCover, SpecType specType, string remarkInformation, bool isAdditionSpec
            , bool IsRuleName)
        {
            //this.Spec = spec;
            this.MatchChecked = MatchChecked;
            this.MeasureParams = MeasureParams;
            this.WordCureTestList = WordCureTestList;
            this.IsAllowOpenCover = IsAllowOpenCover;
            this.SpecType = specType;
            this.RemarkInformation = remarkInformation;
            this.IsAdditionSpec = isAdditionSpec;
            this.IsRuleName = IsRuleName;
        }

    }

    public  class MatchInfo 
    {
        /// <summary>
        /// 匹配模式
        /// </summary>
        public  string MatchPattern { set; get; }

        /// <summary>
        /// 匹配时间
        /// </summary>
        public  int MatchTime { set; get; }

        public string MatchCondition { set; get; }

        public MatchInfo(string matchPattern,string matchCondition, int matchTime)
        {
            this.MatchPattern = matchPattern;
            this.MatchCondition = matchCondition;
            this.MatchTime = matchTime;
        }
       
    }

    public class MeasureParams 
    {
        /// <summary>
        /// 测量次数
        /// </summary>
        public int MeasureNumber { set; get; }

        /// <summary>
        /// 失包时间
        /// </summary>
        public int DiscardTime { set; get; }

        public bool IsManualTest { set; get; }
        public MeasureParams(int measureNumber, int discardTime,bool isManualTest)
        {
            this.MeasureNumber = measureNumber;
            this.DiscardTime = discardTime;
            this.IsManualTest = isManualTest;
        }

    }

    public  class WordCureTest 
    {
        /// <summary>
        /// 谱图对象
        /// </summary>
        public SpecListEntity Spec { set; get; }

        /// <summary>
        /// 编号
        /// </summary>
        public string SerialNumber { set; get; }

        /// <summary>
        /// 工作曲线名称
        /// </summary>
        public  string WordCurveName { set; get; }

        /// <summary>
        /// 工作曲线ID
        /// </summary>
        public string WordCurveID { set; get; }

        public SampleInfo sampleInfo { get; set; }

        public List<CompanyOthersInfo> CompanyInfoList = new List<CompanyOthersInfo>();
             
        public WordCureTest()
        { }

        public WordCureTest(string serialNumber, SpecListEntity spec, string wordCurveName,string workCurveID)
        {
            this.SerialNumber = serialNumber;
            this.Spec = spec;
            this.WordCurveName = wordCurveName;
            this.WordCurveID = workCurveID;
        }
    }

    [Serializable]
    public class WordCureTestSerialize
    {

        /// <summary>
        /// 编号
        /// </summary>
        public string SerialNumber { set; get; }

        /// <summary>
        /// 工作曲线名称
        /// </summary>
        public string WordCurveName { set; get; }

        /// <summary>
        /// 工作曲线ID
        /// </summary>
        public string WordCurveID { set; get; }

        public SampleInfo sampleInfo { get; set; }

        public WordCureTestSerialize()
        { }

        public WordCureTestSerialize(string serialNumber, string wordCurveName, SampleInfo sampleInfo1,string workCurveID)
        {
            this.SerialNumber = serialNumber;
            this.WordCurveName = wordCurveName;
            this.sampleInfo = sampleInfo1;
            this.WordCurveID = WordCurveID;
        }
    }

    [Serializable]
    public class SampleInfo
    {
        /// <summary>
        /// 样品名称
        /// </summary>
        public string SampleName { set; get; }

        /// <summary>
        /// 样品形状
        /// </summary>
        public string Shape { set; get; }

        /// <summary>
        /// 供应商
        /// </summary>
        public string Supplier { set; get; }

        /// <summary>
        /// 重量
        /// </summary>
        public double Weight { set; get; }

        /// <summary>
        /// 描述
        /// </summary>
        public string SpecSummary { set; get; }

        /// <summary>
        /// 烧失量
        /// </summary>
        public double Loss { set; get; }

        public SampleInfo()
        { }

        public SampleInfo(string sampleName, string shape, string supplier, double weight, string specSummay, double loss)
        {
            this.Supplier = supplier;
            this.Shape = shape;
            this.SpecSummary = specSummay;
            this.SampleName = sampleName;
            this.Weight = weight;
            this.Loss = loss;
        }
    }
}
