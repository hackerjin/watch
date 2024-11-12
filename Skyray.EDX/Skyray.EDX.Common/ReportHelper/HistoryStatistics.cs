using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDX.Common;

namespace Skyray.EDXRFLibrary.Define
{
    public class HistoryStatistics
    {
        public List<long> ListHistoryRecordId { get; set; }

        public ElementList ElementList { get; set; }

        public int Decimals { get; set; }

        public double NDValue { get; set; }

        public StandardJudgeType StandardContentJudge { get; set; }
        /// <summary>
        /// 次序列
        /// </summary>
        public List<int> ListSequence { get; set; }
        /// <summary>
        /// 总含量标准
        /// </summary>
        public List<double?> ListStandardContentTotal { get; set; }
        /// <summary>
        /// 总含量判定结果
        /// </summary>
        public List<StandardResult?> ListStandardContentTotalResult { get; set; }
        
        public List<HistoryStatisticsElement> ListHistoryStatisticsElement { get; set; }
        /// <summary>
        /// 所有历史记录的平均K金值
        /// </summary>
        public double Karat { get; set; }

        private HistoryStatistics()
        {
            this.ListHistoryStatisticsElement = new List<HistoryStatisticsElement>();
            this.ListHistoryRecordId = new List<long>();
            this.ListStandardContentTotal = new List<double?>();
            this.ListStandardContentTotalResult = new List<StandardResult?>();
            this.ListSequence = new List<int>();
            var slot = System.Threading.Thread.GetNamedDataSlot("decimals");
            this.Decimals = System.Threading.Thread.GetData(slot).ConvertToType(Decimals);
            var ndValue = System.Threading.Thread.GetNamedDataSlot("ndValue");
            this.NDValue = System.Threading.Thread.GetData(ndValue).ConvertToType(NDValue);
            this.NDValue = this.NDValue == 0 ? -1 : this.NDValue;
        }

        public HistoryStatistics(List<long> ListHistoryRecordId):this(ListHistoryRecordId,ElementList.New)
        {
            this.ElementList = WorkCurve.FindById(HistoryRecord.FindById(ListHistoryRecordId.FirstOrDefault()).WorkCurveId).ElementList;
        }
        public HistoryStatistics(List<long> ListHistoryRecordId, ElementList elementList):this(ListHistoryRecordId, elementList, StandardJudgeType.ContentOnly)
        {            
        }
        public HistoryStatistics(List<long> ListHistoryRecordId, ElementList elementList, StandardJudgeType standardContentJudge):this()          
        {
            this.StandardContentJudge = standardContentJudge;
            this.ListHistoryRecordId = ListHistoryRecordId;
            foreach (long l in this.ListHistoryRecordId)
            {
                HistoryRecord record = HistoryRecord.FindById(l);

                if (record != null)
                {
                    var customStandard = CustomStandard.FindById(record.HistoryElement.LastOrDefault().customstandard_Id);//标准库信息    
                    if (customStandard != null)
                    {
                        if (customStandard.IsSelectTotal)
                            this.ListStandardContentTotal.Add(customStandard.TotalContentStandard);
                        else
                            this.ListStandardContentTotal.Add(null);
                    }
                    else
                    {
                        this.ListStandardContentTotal.Add(null);
                    }
                    this.ListSequence.Add(ListSequence.Count + 1);
                    this.ListStandardContentTotalResult.Add(null);
                }
            }
            this.ElementList = elementList;
            CreateListHistoryStatisticsElement();
        }

        private void UnitHisElements(List<long> historyRecordId)
        {
            var _ListCurveElementName = this.ElementList.Items.ToList().Select(w => { return w.Caption==w.Formula? w.Caption:w.Formula; });
            var strHistoryRecordId = historyRecordId.ConvertAll<string>(w=>w.ToString()).Aggregate((p, n) => p + "," + n);
            var _ListHistoryElementName = HistoryElement.FindBySql("select * from HistoryElement where HistoryRecord_Id in ("+strHistoryRecordId+")")
                .Select(w=>w.elementName).Distinct();
            var _ListLeft = _ListHistoryElementName.Except(_ListCurveElementName);
            foreach(var name in _ListLeft)
            {
                CurveElement curEle = CurveElement.New;
                curEle.Caption = name;
                curEle.RowsIndex = ElementList.Items.Count;
                ElementList.Items.Add(curEle);
            }
        }

        private void CreateListHistoryStatisticsElement()
        {
            UnitHisElements( ListHistoryRecordId);//添加感兴趣元素外的历史记录元素
            foreach (var ele in ElementList.Items)//遍历所有感兴趣元素 假设感兴趣元素为历史记录元素全包括
            {
                var historyStatisticsElement = new HistoryStatisticsElement(ele.Formula,ele.RowsIndex);//(ele.Caption, ele.RowsIndex);//添加感兴趣元素信息
                
                var customStandard = CustomStandard.New;//新建标准库对象                
                foreach (long id in ListHistoryRecordId)
                {
                    HistoryRecord record = HistoryRecord.FindById(id);
                    if (record != null)//历史记录信息--不添加
                    {
                        var hisEles = record.HistoryElement.ToList();
                        var hisEle = hisEles.Find(w => w.elementName == ele.Caption||w.elementName==ele.Formula);
                        if (hisEle == null)//HistoryElement values
                        {
                            hisEle = HistoryElement.New;
                        }
                        historyStatisticsElement.ListAverageValue.Add(hisEle.AverageValue.Round(Decimals));
                        historyStatisticsElement.ListCaculateIntensity.Add(hisEle.CaculateIntensity.Round(Decimals));
                        double dValue = 0;
                        double.TryParse(hisEle.contextelementValue, out dValue);
                        historyStatisticsElement.ListContent.Add(dValue.Round(Decimals,NDValue));
                        historyStatisticsElement.ListCustomstandard_Id.Add(hisEle.customstandard_Id);
                        historyStatisticsElement.ListElementIntensity.Add(hisEle.ElementIntensity.Round(Decimals));
                        historyStatisticsElement.ListError.Add(hisEle.Error.Round(Decimals));
                        historyStatisticsElement.ListHasShow.Add(false);
                        historyStatisticsElement.ListHistoryRecord.Add(record);
                        double.TryParse(hisEle.thickelementValue, out dValue);
                        historyStatisticsElement.ListThickElementValue.Add(dValue.Round(Decimals));
                        historyStatisticsElement.ListThickUnitValue.Add(hisEle.thickunitValue);
                        historyStatisticsElement.ListUnitValue.Add((ContentUnit)hisEle.unitValue);
                        historyStatisticsElement.ListStandardContentResult.Add(null);
                        historyStatisticsElement.ListStandardThickResult.Add(null);
                        
                        customStandard = CustomStandard.FindById(historyStatisticsElement.ListCustomstandard_Id.LastOrDefault());//标准库信息    
                        
                        if (customStandard != null)
                        {
                            var standardDatas = StandardData.FindBySql("select * from StandardData where  CustomStandard_Id=" + customStandard.Id);
                            var standardData = standardDatas.Find(w => w.ElementCaption == historyStatisticsElement.StatisticsCaption);//元素名相同
                            if (standardData != null)
                            {
                                historyStatisticsElement.ListStandardContent.Add(standardData.StandardContent.Round(Decimals,NDValue));                                
                                historyStatisticsElement.ListStandardThick.Add(standardData.StandardThick.Round(Decimals));
                                historyStatisticsElement.ListStandardStartContent.Add(standardData.StartStandardContent.Round(Decimals));                                
                            }
                            else
                            {
                                historyStatisticsElement.ListStandardContent.Add(null);
                                historyStatisticsElement.ListStandardThick.Add(null);
                                historyStatisticsElement.ListStandardStartContent.Add(null);
                            }
                        }
                        else
                        {
                            historyStatisticsElement.ListStandardContent.Add(null);
                            historyStatisticsElement.ListStandardThick.Add(null);
                            historyStatisticsElement.ListStandardStartContent.Add(null);
                        }
                    }
                }
                if (!IsListItemsCountSame(historyStatisticsElement))
                    throw new Exception("元素属性集合总数不一致");
                SetContentStandardResult(historyStatisticsElement);
                this.ListHistoryStatisticsElement.Add(historyStatisticsElement);
                this.Karat += historyStatisticsElement.GetKarat().Round(Decimals);
            }
            SetTotalContentSandardResult();//设置所有总判断
        }

        private bool IsListItemsCountSame(object obj)
        {
            Type type = obj.GetType();
            System.Reflection.PropertyInfo[] pros = type.GetProperties();//获取对象所有属性
            //var genericPros = pros.Where(w => w.PropertyType.Name.Contains("List");
            //获取所有集合属性
            var genericPros = pros.Where(w => w.PropertyType.IsGenericType == true && w.PropertyType.GetInterface("IEnumerable") != null);
            //获取所有集合属性集合的长度
            var listCount = genericPros.Select(w => w.PropertyType.GetProperty("Count").GetValue(w.GetValue(obj, null), null)).Cast<int>();
            //如果所有长度相等只有一个唯一值
            return listCount.Distinct().Count() == 1;
        }

        public void SetContentStandardResult(HistoryStatisticsElement historyStatisticsElement)
        {            
            for (int i = 0; i < historyStatisticsElement.ListContent.Count; i++)
            {
                if (historyStatisticsElement.ListStandardContent[i] == null) continue;//含量标准为空 结果为空
                double source = historyStatisticsElement.ListContent[i];
                if (StandardJudgeType.ContentOnly == this.StandardContentJudge)
                {
                    if (source.Equals(double.NaN)) source = 0;
                    if (source >= historyStatisticsElement.ListStandardContent[i]) //大于或等于
                    {
                        historyStatisticsElement.ListStandardContentResult[i] = StandardResult.Fail;//超出Fail
                        if (historyStatisticsElement.StatisticsCaption.Equals("Cr") || historyStatisticsElement.StatisticsCaption.Equals("Br"))
                        {
                            historyStatisticsElement.ListStandardContentResult[i] = StandardResult.TBD;//Br Cr则为TBD
                        }
                    }
                    else//小于标准Pass
                    {
                        historyStatisticsElement.ListStandardContentResult[i] = StandardResult.Pass;
                    }
                }
                //含量+误差
                else if (StandardJudgeType.ContentAndError == this.StandardContentJudge)
                {
                    source = historyStatisticsElement.ListContent[i] + historyStatisticsElement.ListError[i];
                    if (source.Equals(double.NaN)) source = 0;
                    double? low = historyStatisticsElement.ListStandardStartContent[i];//下限
                    double? top= historyStatisticsElement.ListStandardContent[i];//上限
                    if (source <= low)
                        historyStatisticsElement.ListStandardContentResult[i] = StandardResult.Pass;
                    else if (source > low && source <= top)
                        historyStatisticsElement.ListStandardContentResult[i] = StandardResult.TBD;
                    else if (source > top)
                        historyStatisticsElement.ListStandardContentResult[i] = StandardResult.Fail;
                }

            }
        }

        private void SetTotalContentSandardResult()
        {
            for (int i = 0; i < this.ListStandardContentTotalResult.Count; i++)//各历史记录
            {
                double? d = null;
                StandardResult? levelResult = null;
                foreach (var stsEle in this.ListHistoryStatisticsElement)//各历史记录元素
                {
                    if (stsEle.ListStandardContent[i] == null) continue;//无标准库元素不计算
                    if(d==null) d=0;//去空值
                    d += stsEle.ListContent[i];//累加标准库元素值
                    if (stsEle.ListStandardContentResult[i] == null) continue;
                    if (levelResult == null)//获取标准判定结果
                        levelResult = stsEle.ListStandardContentResult[i];
                    else//取最高级标准判定结果
                        levelResult = (StandardResult)Math.Max((int)stsEle.ListStandardContentResult[i], (int)levelResult);
                }
                if (this.ListStandardContentTotal[i] == null)
                    this.ListStandardContentTotalResult[i] = levelResult;
                else if (d < this.ListStandardContentTotal[i])
                    this.ListStandardContentTotalResult[i] = (StandardResult)Math.Max((int)levelResult, (int)StandardResult.Pass);
                else
                    this.ListStandardContentTotalResult[i] = StandardResult.Fail;
            }

        }
    }

    public class HistoryStatisticsElement
    {
        /// <summary>
        /// 元素名称
        /// </summary>
        [TableHeaderOf("ListContent", "ListHistoryRecord", "ListElementIntensity", "ListCaculateIntensity", "ListElementIntensity", "ListStandardContent",
            "ListCaculateIntensity", "ListCustomstandard_Id", "ListUnitValue", "ListThickUnitValue", "ListThickElementValue", "ListStandardStartContent",
             "ListStandardThick","ListStandardContentResult","ListStandardThickResult","ListError","ListAverageValue","ListHasShow")]
        public string StatisticsCaption { get; set; }
        private Atom atom;
        /// <summary>
        /// 元素全称中文
        /// </summary>
        public string StatisticsAtomAllNameCN { get { return atom.AtomNameCN; } }
        /// <summary>
        /// 元素名称EN
        /// </summary>
        public string StatisticsAtomAllNameEN { get { return atom.AtomNameEN; } }
        /// <summary>
        /// 元素密度
        /// </summary>
        public double StatisticsAtomDensity { get { return atom.AtomDensity; } }
        /// <summary>
        /// 原子量
        /// </summary>
        public double StatisticsAtomWeight { get { return atom.AtomWeight; } }
        /// <summary>
        /// 默认线系
        /// </summary>
        public XLine StatisticsAtomDefaultLine { get { return atom.DefaultLine; } }
        /// <summary>
        /// Ka值
        /// </summary>
        public double StatisticsAtomKa { get { return atom.Ka; } }
        /// <summary>
        /// Kb值
        /// </summary>
        public double StatisticsAtomKb { get { return atom.Kb; } }
        /// <summary>
        /// La值
        /// </summary>
        public double StatisticsAtomLa { get { return atom.La; } }
        /// <summary>
        /// Lb值
        /// </summary>
        public double StatisticsAtomLb { get { return atom.Lb; } }
        /// <summary>
        /// Le值
        /// </summary>
        public double StatisticsAtomLe { get { return atom.Le; } }
        ///<summary>
        /// Lr值
        /// </summary>
        public double StatisticsAtomLr { get { return atom.Lr; } }
        /// <summary>
        /// 元素序号
        /// </summary>
        public int StatisticsRowIndex { get; set; }

        /// <summary>
        ///含量集合 该元素在各历史记录中的含量
        /// </summary>
        public List<double> ListContent { get; set; }

        /// <summary>
        /// 历史记录集合 该元素包含在的历史记录集合
        /// </summary>
        public List<HistoryRecord> ListHistoryRecord { get; set; }

        /// <summary>
        /// 元素全面积集合 该元素在各历史记录中的元素全面积
        /// </summary>
        public List<double>  ListElementIntensity { get; set; }

        //计算强度
        /// <summary>
        /// 计算强度 该元素在各历史记录中的计算强度
        /// </summary>
        public List<double> ListCaculateIntensity { get; set; }

        //标准库Id
        /// <summary>
        /// 标准库Id 该元素在各历史记录中对应的标准库Id
        /// </summary>
        public List<long> ListCustomstandard_Id { set; get; }

        /// <summary>
        /// 含量单位
        /// </summary>
        public List<ContentUnit> ListUnitValue { set; get; }
        /// <summary>
        /// 厚度单位
        /// </summary>
        public List<int> ListThickUnitValue { set; get; }
        /// <summary>
        /// 厚度值
        /// </summary>
        public List<double> ListThickElementValue { set; get; }

        /// <summary>
        /// 标准含量限制值
        /// </summary>
        public List<double?> ListStandardContent { set; get; }
        /// <summary>
        /// 标准起始含量限制值
        /// </summary>
        public List<double?> ListStandardStartContent { get; set; }//起始标准含量
        /// <summary>
        /// 标准厚度限制值
        /// </summary>
        public List<double?> ListStandardThick { get; set; }//标准厚度
        /// <summary>
        /// 含量判定结果
        /// </summary>
        public List<StandardResult?> ListStandardContentResult { get; set; }
        /// <summary>
        /// 标准厚度判定结果
        /// </summary>
        public List<StandardResult?> ListStandardThickResult { get; set; }
        /// <summary>
        /// 误差
        /// </summary>
        public List<double> ListError { get; set; }

        
        public List<double> ListAverageValue { get; set; }
        
        /// <summary>
        /// 是否显示处理
        /// </summary>
        public List<bool> ListHasShow { get; set; }

        private HistoryStatisticsElement()
        {
            this.ListAverageValue = new List<double>();
            this.ListCaculateIntensity = new List<double>();
            this.ListContent = new List<double>();
            this.ListCustomstandard_Id = new List<long>();
            this.ListElementIntensity = new List<double>();
            this.ListError = new List<double>();
            this.ListHasShow = new List<bool>();
            this.ListHistoryRecord = new List<HistoryRecord>();
            this.ListThickElementValue = new List<double>();
            this.ListThickUnitValue = new List<int>();
            this.ListUnitValue = new List<ContentUnit>();
            this.ListStandardContent = new List<double?>();
            this.ListStandardStartContent = new List<double?>();
            this.ListStandardThick = new List<double?>();
            this.ListStandardContentResult = new List<StandardResult?>();
            this.ListStandardThickResult = new List<StandardResult?>();            
        }

        public HistoryStatisticsElement(string statisticsCaption, int index):this()
        {
            this.StatisticsCaption = statisticsCaption;
            this.StatisticsRowIndex = index;
            this.atom = Atoms.GetAtom(StatisticsCaption);
            if(Oxide.FindOne(w=>w.OxideName==statisticsCaption)!=null)
            this.atom = this.atom ?? Oxide.Find(w => w.OxideName == statisticsCaption).Select(w => w.Atom).FirstOrDefault();
            this.atom = this.atom ?? Atom.New;
        }

        public double GetKarat()
        {
            double d = 0;
            if (string.Compare(StatisticsCaption, "Au", true) == 0)
            {
               // d = ListContent.Average()*24/99.995;
                d = ListContent.Average() * 24 / WorkCurveHelper.KaratTranslater;
            }
            return d;
        }
    }

    public enum StandardResult
    {
        Pass =-1,
        TBD = 0,
        Fail = 1,
    }

    public enum StandardJudgeType
    {
        ContentOnly =1,
        ContentAndError = 2
    }
    /// <summary>
    /// 表头标识
    /// </summary>
    public class TableHeaderOfAttribute : Attribute
    {
        /// <summary>
        /// 受表头控制的字段
        /// </summary>
        public List<string> Items = new List<string>();
        public TableHeaderOfAttribute(params string[] items)
        {
            Items.AddRange( items);
        }
    }
}
