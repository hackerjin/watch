using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using Skyray.EDX.Common;
using Skyray.Controls;
using Skyray.EDXRFLibrary;
using System.Data;
using Lephone.Data;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.EDX.Common
{
    public class TabControlHelper
    {
        public static List<Atom> listAtom = Atoms.AtomList;

        public static DataGridViewW GetControlFromPanel(string controlName, Control container)
        {
            if (controlName == null || container == null || container.Controls.Count == 0)
                return null;
            DataGridViewW returnW = null;
            Control[] control = container.Controls.Find(controlName, true);
            if (control == null || control.Length == 0)
                return null;
            foreach (Control tempControl in control)
            {
                if (tempControl.GetType() == typeof(DataGridViewW))
                {
                    returnW = (DataGridViewW)tempControl;
                    returnW.ReadOnly = true;
                    returnW.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
                    return returnW;
                }
            }
            return null;
        }

        #region 匹配最高备份
        //public static List<MatchEntity> MatchingHighMatchLevel(List<WorkCurve> findCurve, SpecList specList,
        // int maxOffSet, List<DeviceParameter> listDevice, ref WorkCurve finalWordCurve)
        //{
        //    double maxMatchlevel = double.Epsilon;
        //    List<MatchEntity> matchEntity = new List<MatchEntity>();
        //    foreach (WorkCurve curve in findCurve)
        //    {
        //        double totalLevel = 1.00;
        //        foreach (SpecList kl in curve.Condition.SpecLists)
        //        {
        //            foreach (Spec specT in kl.Specs)
        //            {
        //                foreach (Spec specCurrent in specList.Specs)
        //                {
        //                    if (specT.Name == specCurrent.Name)
        //                    {
        //                        DeviceParameter currentDeviceParam = listDevice.Find(delegate(DeviceParameter dp) { return dp.Name == specT.DeviceParameter.Name; });
        //                        double matchLevel = Matching(ConvertToArrayInterger(specCurrent.SpecData),
        //                                                         maxOffSet, GetHighSpecChannel(currentDeviceParam, specCurrent.SpecData),
        //                                                         TabControlHelper.ConvertToArrayInterger(specT.SpecData));
        //                        totalLevel = totalLevel * matchLevel;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        MatchEntity entity = new MatchEntity();
        //        entity.workCurve = curve;
        //        entity.Matching = totalLevel;
        //        matchEntity.Add(entity);
        //        if (totalLevel > maxMatchlevel)
        //        {
        //            maxMatchlevel = totalLevel;
        //            finalWordCurve = curve;
        //        }
        //    }
        //    return matchEntity;
        //}

        //public static List<MatchEntity> MatchWorkCurve(int maxOffSet, Spec spec, DeviceParameter deviceParams)
        //{
        //    List<WorkCurve> listWork = WorkCurve.FindAll();
        //    if (listWork.Count == 0)
        //        return null;
        //    List<MatchEntity> entityList = new List<MatchEntity>();
        //    foreach (WorkCurve workCurve in listWork)
        //    {
        //        foreach (SpecList specTempList in workCurve.Condition.SpecLists)
        //        {
        //            foreach (Spec specTemp in specTempList.Specs)
        //            {
        //                double matchLevel = Matching(ConvertToArrayInterger(spec.SpecData),
        //                                    maxOffSet, GetHighSpecChannel(deviceParams,
        //                                    spec.SpecData), ConvertToArrayInterger(specTemp.SpecData));
        //                MatchEntity matchEntity = new MatchEntity();
        //                matchEntity.workCurve = workCurve;
        //                matchEntity.Matching = matchLevel;
        //                entityList.Add(matchEntity);
        //            }
        //        }
        //    }
        //    return entityList;
        //}
        #endregion

        private static double CaculateWorkCurveMatching(SpecEntity specs, WorkCurve workCurve, int maxOffSet)
        {
            double currentMaxLevel = 0d;
            string getMatchId = workCurve.ElementList.MatchSpecListIdStr;
            if (!string.IsNullOrEmpty(getMatchId))
            {
                string[] str = getMatchId.Split(',');
                int i = 0;
                while (i < str.Length)
                {
                    if (!string.IsNullOrEmpty(str[i]))
                    {
                        bool checkedMatch = str[i + 1] == "1" ? true : false;
                        if (checkedMatch)
                        {
                            var matchList = WorkCurveHelper.DataAccess.Query(str[i]);
                            if (matchList != null && matchList.Specs.Length > 0)
                            {
                                double tempMaxLevel = 0d;
                                foreach (var sp in matchList.Specs)
                                {
                                    tempMaxLevel = Matching(specs.SpecDatas, maxOffSet, SpecHelper.GetHighSpecChannel(sp.DeviceParameter.BeginChann, sp.DeviceParameter.EndChann, sp.SpecDatas), sp.SpecDatas);
                                    if (tempMaxLevel.CompareTo(currentMaxLevel) > 0)
                                        currentMaxLevel = tempMaxLevel;
                                }
                            }
                        }
                    }
                    i += 2;
                }
            }
            return currentMaxLevel;
        }

        /// <summary>
        /// 找到相同条件下所有工作曲线，并计算匹配相似度
        /// </summary>
        /// <param name="currentSpec">测量出来的谱数据</param>
        /// <param name="deviceParams">当前测量小条件</param>
        /// <returns></returns>
        public static List<MatchEntity> FindCurveIncludedCondition(SpecEntity currentSpec, DeviceParameter deviceParams, int maxOffSet,int flag)
        {
            
            //double outputMaxWorkCurve = 0d;
            List<MatchEntity> matchEntity = new List<MatchEntity>();
            List<WorkCurve> allWorkCurve = WorkCurve.FindBySql(@"select distinct a.* from WorkCurve a inner join Condition b on a.Condition_Id = b.Id inner join Device c 
                                    on b.Device_Id = c.Id where b.Device_Id=" + WorkCurveHelper.DeviceCurrent.Id + " and a.FuncType =" + flag);
            if (allWorkCurve == null || allWorkCurve.Count == 0)
                return null;
            foreach (WorkCurve workCurve in allWorkCurve)
            {
                if (!workCurve.IsJoinMatch) continue;// add by caoyunhe
                //double currentMaxLevel = TraverseCurrentWorkCurve(workCurve, specs, dicSpec);
                double currentMaxLevel = CaculateWorkCurveMatching(currentSpec, workCurve,maxOffSet);
                if (currentMaxLevel > 0)
                {
                    MatchEntity currentMatch = new MatchEntity(workCurve, currentMaxLevel);
                    matchEntity.Add(currentMatch);
                }
                //if (currentMaxLevel.CompareTo(outputMaxWorkCurve) > 0)
                //    maxWorkCurve = workCurve;
                //outputMaxWorkCurve = currentMaxLevel;
            }
            return matchEntity;
//            if (currentSpec == null || deviceParams == null)
//                return null;
//            List<MatchEntity> matchEntity = new List<MatchEntity>();
////            var list = Spec.FindBySql(@"select * from Spec a inner join SpecList b on a.SpecList_Id=b.Id inner join Condition c 
////                                        on b.Condition_Id = c.Id inner join Device d on c.Device_Id=d.Id where c.Type ="+(int)ConditionType.Match+ " and c.Device_Id="+ WorkCurveHelper.DeviceCurrent.Id);
//            var list = Spec.FindBySql(@"select * from Spec a inner join SpecList b on a.SpecList_Id=b.Id inner join Condition c 
//                                        on b.Condition_Id = c.Id inner join Device d on c.Device_Id=d.Id where c.Device_Id=" + WorkCurveHelper.DeviceCurrent.Id);
//            if (list == null || list.Count == 0)
//                return null;
//            Dictionary<long, double> dicSpec = new Dictionary<long, double>();
//            foreach (Spec spec in list)
//            {
//                if (spec.DeviceParameter == null || string.IsNullOrEmpty(spec.SpecData) || spec.SpecDatas.Length !=(int)WorkCurveHelper.DeviceCurrent.SpecLength)
//                    continue;
//                double matchingLevel = Matching(currentSpec.SpecDatas, maxOffSet, SpecHelper.GetHighSpecChannel(spec.DeviceParameter.BeginChann,spec.DeviceParameter.EndChann, spec.SpecDatas), spec.SpecDatas);
//                if (!dicSpec.ContainsKey(spec.Id) && matchingLevel>0d)
//                    dicSpec.Add(spec.Id, matchingLevel);
//            }
//            if (dicSpec.Count > 0)
//            {
//                MaxHighWorkCurve(dicSpec, ref matchEntity, ref maxWorkCurve, list, flag);
//            }
//            return matchEntity;
        }

        /// <summary>
        /// 找到最匹配的工作曲线及统计所有相同匹配条件下的工作曲线
        /// </summary>
        /// <param name="dicSpec">相同匹配条件下具有的匹配度</param>
        /// <param name="matchEntity">所有对应的匹配度</param>
        /// <param name="maxWorkCurve">输出匹配度最高的曲线</param>
        /// <param name="specs">所有相同匹配条件下的谱</param>
//        private static void MaxHighWorkCurve(Dictionary<long, double> dicSpec, ref List<MatchEntity> matchEntity, ref WorkCurve maxWorkCurve, List<Spec> specs,int flag)
//        {
//            double outputMaxWorkCurve = 0d;
//            List<WorkCurve> allWorkCurve = WorkCurve.FindBySql(@"select distinct a.* from WorkCurve a inner join Condition b on a.Condition_Id = b.Id inner join Device c 
//                                    on b.Device_Id = c.Id where b.Device_Id=" + WorkCurveHelper.DeviceCurrent.Id + " and a.FuncType =" + flag);
//            if (allWorkCurve == null || allWorkCurve.Count == 0)
//                return;
//            foreach (WorkCurve workCurve in allWorkCurve)
//            {
//                if (!workCurve.IsJoinMatch) continue;// add by caoyunhe
//                double currentMaxLevel = TraverseCurrentWorkCurve(workCurve, specs, dicSpec);
//                if (currentMaxLevel > 0)
//                {
//                    MatchEntity currentMatch = new MatchEntity(workCurve, currentMaxLevel);
//                    matchEntity.Add(currentMatch);
//                }
//                if (currentMaxLevel.CompareTo(outputMaxWorkCurve) > 0)
//                    maxWorkCurve = workCurve;
//                outputMaxWorkCurve = currentMaxLevel;
//            }
//        }

        /// <summary>
        /// 指定工作曲线下计算出最高匹配度
        /// </summary>
        /// <param name="workCurve">指定的工作曲线</param>
        /// <param name="specs">所有相同匹配条件下的谱</param>
        /// <param name="dicSpec">相同匹配条件下具有的匹配度</param>
        /// <returns></returns>
        //private static double TraverseCurrentWorkCurve(WorkCurve workCurve, List<Spec> specs, Dictionary<long, double> dicSpec)
        //{
        //    double currentMaxLevel = 0d;
        //    if (workCurve.ElementList == null || workCurve.ElementList.Items.Count == 0)
        //        return currentMaxLevel;
        //    List<CurveElement> currentCurveElements = workCurve.ElementList.Items.ToList();
        //    if (currentCurveElements == null || currentCurveElements.Count == 0)
        //        return currentMaxLevel;
        //    foreach (CurveElement currentElement in currentCurveElements)
        //        FindCurrentElementStandSample(currentElement, specs, dicSpec, ref currentMaxLevel);
        //    return currentMaxLevel;
        //}

        /// <summary>
        /// 当前感兴趣元素下对应匹配条件下的匹配度
        /// </summary>
        /// <param name="currentElement">当前元素</param>
        /// <param name="specs">所有相同匹配条件下的谱</param>
        /// <param name="dicSpec">相同匹配条件下具有的匹配度</param>
        /// <param name="currentMaxLevel">变化的最大匹配度</param>
        //private static double FindCurrentElementStandSample(CurveElement currentElement, SpecEntity spec,int maxofset)
        //{
        //    double currentMaxLevel = 0d;
        //    List<StandSample> elementSamples = currentElement.Samples.ToList();
        //    if (elementSamples == null || elementSamples.Count == 0)
        //        return 0d;
        //    foreach (StandSample samples in elementSamples)
        //    {
        //        if (!samples.IsMatch) continue;
        //        if (samples.MatchSpecName.IsNullOrEmpty() || samples.MatchSpecListId == 0) continue;
        //        var matchList =  SpecList.FindById(samples.MatchSpecListId);
        //        if (matchList == null || matchList.Specs.Count < 0) continue;
        //        double tempMaxLevel = 0d;
        //        foreach (var sp in matchList.Specs)
        //        {
        //            tempMaxLevel = Matching(spec.SpecDatas, maxofset, SpecHelper.GetHighSpecChannel(sp.DeviceParameter.BeginChann, sp.DeviceParameter.EndChann, sp.SpecDatas), sp.SpecDatas);
        //            if (tempMaxLevel.CompareTo(currentMaxLevel) > 0)
        //                currentMaxLevel = tempMaxLevel;
        //        }
        //    }
        //    return currentMaxLevel;
        //}

        /// <summary>
        /// 求匹配度
        /// </summary>
        /// <param name="data">匹配的数据</param>
        /// <returns></returns>
        public static double Matching(int[] data, int maxOffSet, int ChannOfMaxValue, int[] Data)
        {
            double eb = difference(data, -maxOffSet, maxOffSet, Data);
            double tempEb;
            for (int i = -maxOffSet + 1; i <= maxOffSet; i++)
            {
                tempEb = difference(data, i, maxOffSet, Data);
                if (tempEb > eb)
                {
                    eb = tempEb;
                }
            }
            //构建一个新谱数据
            //int keyValue = Data[ChannOfMaxValue] / 2;
            //int[] dataC = new int[Data.Length];
            //for (int i = 0; i < Data.Length; i++)
            //{
            //    if (Data[i] > keyValue)
            //    {
            //        dataC[i] = 0;
            //    }
            //    else
            //    {
            //        dataC[i] = keyValue + keyValue;
            //    }
            //}
            ////算新谱的差额

            //double ec = difference(dataC, 0, maxOffSet, Data);
            //匹配度
            return Math.Sqrt(eb)*100;
        }

        /// <summary>
        /// 计算两个谱数据的差额
        /// </summary>
        /// <param name="data">另一个谱数据</param>
        /// <param name="offset">道偏移量</param>
        /// <returns>差额</returns>
        private static double difference(int[] data, int offset, int maxOffSet, int[] Data)
        {
            double sumAB = 0;
            double sumB2 = 0;
            double yB2 = 0;
            double dataSum = 0;
            double DataSum = 0;
            for (int i = maxOffSet; i < Data.Length - maxOffSet; i++)
            {
                dataSum += data[i + offset];
                DataSum += Data[i];
            }
            double averagedata = dataSum/(Data.Length - 2*maxOffSet);
            double averageData = DataSum / (Data.Length - 2 * maxOffSet);
            for (int i = maxOffSet; i < Data.Length - maxOffSet; i++)
            {
                sumAB += (data[i + offset] - averagedata) * (Data[i]-averageData);
                sumB2 += (data[i + offset] - averagedata) * (data[i + offset] - averagedata);
                yB2 += (Data[i] - averageData) * (Data[i] - averageData);
            }
            //拟合系数
            double k = sumAB / sumB2;      //斜率
            double k0 = averageData - k * averagedata;    //截距
            //差额
            double eb = 0;
            for (int i = maxOffSet; i < Data.Length - maxOffSet; i++)
            {
                eb += (Data[i] - k * data[i + offset] - k0) * (Data[i] - k * data[i + offset] - k0);
            }

            return (1 - eb / yB2);
        }

       

        /// <summary>
        /// 根据测量次数信息统计各个元素的各个参数
        /// </summary>
        /// <param name="MinValue">最小值</param>
        /// <param name="MaxValue">最大值</param>
        /// <param name="AvgValue">平均值</param>
        /// <param name="SdValue">方差</param>
        /// <param name="dataGridView">当前测量含量容器</param>
        /// <param name="testCurrentTimes">当前扫描次数</param>
        /// <param name="colName">列名</param>
        public static void CaculateStaticsData(ref double MinValue, ref double MaxValue, ref double AvgValue,
                    ref double SdValue, DataGridView dataGridView, int testCurrentTimes, string colName)
        {
            if (dataGridView == null)
                return;
            double cell, sum, sum2, min, max, warp;
            min = double.MaxValue;
            max = double.MinValue;
            sum = 0;
            sum2 = 0;
            int nullCount = 0;
            for (int row = 0; row < testCurrentTimes; row++)
            {
                if (dataGridView[colName, row].Value == null)
                {
                    nullCount++;
                    continue;
                }
                cell = Convert.ToDouble(dataGridView[colName, row].Value.ToString());
                if (min > cell)
                {
                    min = cell;
                }
                if (max < cell)
                {
                    max = cell;
                }
                sum += cell;
                sum2 += cell * cell;
            }
            MinValue = min; //最小值
            MaxValue = max; //最大值
            sum /= (testCurrentTimes - nullCount);
            AvgValue = sum; //求和
            if (testCurrentTimes - nullCount > 1)
            {
                sum = Math.Round(sum, 4);
                double powTotal = 0;
                for (int i = 0; i < testCurrentTimes; i++)
                {
                    if (dataGridView[colName, i].Value == null)
                        continue;
                    double dcell = Convert.ToDouble(dataGridView[colName, i].Value.ToString());
                    powTotal += Math.Pow((dcell - sum), 2);
                }
                warp = Math.Sqrt(powTotal / (testCurrentTimes - nullCount - 1));
                SdValue = warp;
            }
            else
            {
                SdValue = 0d;
            }
        }

        public static void CaculateStaticsData(ref string MinValue, ref string MaxValue, ref string AvgValue,
                    ref string SdValue, DataTable datable, int testCurrentTimes, string colName)
        {
            if (datable == null)
                return;
            // 修改：何晓明 2011-05-27 ND 直接报 ND
            for (int i = 0; i < datable.Rows.Count; i++)
            {
                if (datable.Rows[i][colName].ToString().ToUpper() == "ND")
                {
                    MinValue = MaxValue = AvgValue = SdValue = "ND";
                    return;
                }
            }

            double cell, sum, sum2, min, max, warp;
            min = double.MaxValue;
            max = double.MinValue;
            sum = 0;
            sum2 = 0;
            int nullCount = 0;
            //if (datable.Rows[0][colName]!= null)
            //{
            //    //修改：何晓明 2011-05-26 ND 不能转换为数字
            //    //cell = Convert.ToDouble(datable.Rows[0][colName].ToString());
            //    double.TryParse(datable.Rows[0][colName].ToString(), out cell);                
            //    //
            //    min = cell;
            //    max = min;
            //    sum = min;
            //    sum2 = cell * cell;
            for (int row = 0; row < testCurrentTimes; row++)
            {
                //修改：何晓明 2011-05-26 ND 不能转换为数字
                //cell = Convert.ToDouble((datable.Rows[row][colName].ToString() == "") ? "0" : datable.Rows[row][colName].ToString());
                //cell = Convert.ToDouble((datable.Rows[row][colName].ToString() == ""||datable.Rows[row][colName].ToString() =="ND") ? "0" : datable.Rows[row][colName].ToString());
                if (datable.Rows[row][colName] == null)
                {
                    nullCount++;
                    continue;
                }
                double.TryParse(datable.Rows[row][colName].ToString(), out cell);
                if (min > cell)
                {
                    min = cell;
                }
                if (max < cell)
                {
                    max = cell;
                }
                sum += cell;
                sum2 += cell * cell;
            }
            MinValue = min.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()); //最小值
            MaxValue = max.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()); //最大值
            sum /= (testCurrentTimes - nullCount);
            AvgValue = sum.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()); //求和
            if (testCurrentTimes > 1)
            {
                sum = Math.Round(sum, 4);
                double powTotal = 0;
                for (int i = 0; i < testCurrentTimes; i++)
                {
                    if (datable.Rows[i][colName] == null)
                        continue;
                    double dcell = 0;
                    double.TryParse(datable.Rows[i][colName].ToString(), out dcell);
                    powTotal += Math.Pow((dcell - sum), 2);
                }
                warp = Math.Sqrt(powTotal / (testCurrentTimes - nullCount - 1));

                //warp = Math.Sqrt(Math.Abs(sum2 - sum * sum * testCurrentTimes) / (testCurrentTimes - 1));
                SdValue = warp.ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString());
            }
            else
            {
                SdValue = string.Empty;
            }
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MinValue"></param>
        /// <param name="MaxValue"></param>
        /// <param name="AvgValue"></param>
        /// <param name="SdValue"></param>
        /// <param name="dataGridView"></param>
        /// <param name="testCurrentTimes"></param>
        /// <param name="colName"></param>
        /// <param name="flag">取厚度结果为true，否则为false</param>
        public static void CaculateStaticsData(ref double MinValue, ref double MaxValue, ref double AvgValue,
                   ref double SdValue, DataGridViewW dataGridView, int testCurrentTimes, string colName, string flag)
        {
            if (dataGridView == null)
                return;
            double cell, sum, sum2, min, max, warp;
            if (dataGridView.Columns.Count == 0)
                return;
            for (int i = 1; i < dataGridView.Columns.Count; i++)
            {
                //string equalFunc = string.Empty;
                //if (!flag)
                //    equalFunc = "Content";
                //else
                //    equalFunc = "Thick";
                if (String.Compare(dataGridView.Columns[i].Name, colName, true) == 0 && dataGridView.Columns[i].Tag.ToString().Equals(flag))
                {
                    min = double.MaxValue;
                    max = double.MinValue;
                    sum = 0;
                    sum2 = 0;
                    int nullCount = 0;
                    for (int row = 0; row < testCurrentTimes; row++)
                    {
                        if (dataGridView.Rows[row].Cells[i].Value == null)
                        {
                            nullCount++;
                            continue;
                        }
                        cell = Convert.ToDouble(dataGridView.Rows[row].Cells[i].Value.ToString());
                        if (min > cell)
                        {
                            min = cell;
                        }
                        if (max < cell)
                        {
                            max = cell;
                        }
                        sum += cell;
                        sum2 += cell * cell;
                    }
                    MinValue = min; //最小值
                    MaxValue = max; //最大值
                    sum /= (testCurrentTimes - nullCount);
                    AvgValue = sum; //求和
                    if (testCurrentTimes > 1)
                    {
                        warp = Math.Sqrt(Math.Abs(sum2 - sum * sum * (testCurrentTimes - nullCount)) / (testCurrentTimes - nullCount - 1));
                        SdValue = warp;
                    }
                    else
                    {
                        SdValue = 0d;
                    }
                    break;
                }
            }
        }

        private List<DataRow> AddStaticsRows(DataTable dt, string HeadColumnName)
        {
            //计算带单位的历史记录的最大值、最小值、平均值及标准偏差
            var lr = dt.Select();

            DataRow drMax = dt.NewRow();//新建最大值行
            DataRow drMin = dt.NewRow();//新建最小值行
            DataRow drAva = dt.NewRow();//新建平均值行
            DataRow drSD = dt.NewRow();//新建标准偏差行

            foreach (DataColumn column in dt.Columns)
            {

                if (string.Compare(column.ColumnName, HeadColumnName, true) == 0)
                {
                    //首列填充列头说明信息
                    drMax[column] = Info.MaxValue;
                    drMin[column] = Info.MinValue;
                    drAva[column] = Info.MeanValue;
                    drSD[column] = Info.SDValue;
                    continue;
                }
                //获取该列单位类型个数
                var majorUnit = from l in lr
                                where (l[column] != System.DBNull.Value && l[column].ToString() != "")//过滤空白项
                                group l by l[column].ToString().Split('(').LastOrDefault() into g
                                orderby g.Count() descending
                                select new
                                {
                                    Key = g.Key,
                                    Count = g.Count()
                                };
                //只有一个类型的单位直接计算
                var sMajorUnit = "(" + majorUnit.FirstOrDefault().Key;//获取单位个数最多的那个单位作为统计结果平均值和标准偏差的单位
                if (majorUnit.Count() == 1)
                {
                    var value = from l in lr
                                where (l[column] != System.DBNull.Value && l[column].ToString() != "")//过滤空白项
                                group l by 1 into g
                                select new
                                {//获取最大值、最小值、平均值、标准偏差数值
                                    Max = g.Max(p => Convert.ToDouble(p[column] == DBNull.Value ? "0" : p[column].ToString().Split('(').FirstOrDefault())),
                                    Min = g.Min(p => Convert.ToDouble(p[column] == DBNull.Value ? "0" : p[column].ToString().Split('(').FirstOrDefault())),
                                    Ava = g.Average(p => Convert.ToDouble(p[column] == DBNull.Value ? "0" : p[column].ToString().Split('(').FirstOrDefault())),
                                    SD = Math.Sqrt(g.Aggregate(0.0, (result, p) => result +
                                       Math.Pow(Convert.ToDouble(p[column] == DBNull.Value ? "0" : p[column].ToString().Split('(').FirstOrDefault()) -
                                       g.Average(w => Convert.ToDouble(p[column] == DBNull.Value ? "0" : w[column].ToString().Split('(').FirstOrDefault()))
                                       , 2)
                                       / (g.Count() - 1))
                                       )
                                };
                    drMax[column] = value.FirstOrDefault().Max.ToString("f" + WorkCurveHelper.SoftWareContentBit) + sMajorUnit;//附加计算结果单位
                    drMin[column] = value.FirstOrDefault().Min.ToString("f" + WorkCurveHelper.SoftWareContentBit) + sMajorUnit;
                    drAva[column] = value.FirstOrDefault().Ava.ToString("f" + WorkCurveHelper.SoftWareContentBit) + sMajorUnit;
                    drSD[column] = value.FirstOrDefault().SD.ToString("f" + WorkCurveHelper.SoftWareContentBit) + sMajorUnit;
                    continue;
                }

                //多于一个单位的转换成ppm统一计算再由ppm转回主单位 可以兼容一个单位 但效率低
                double Max = double.MinValue, Min = double.MaxValue, Ava = 0.0, SD = 0.0;
                foreach (DataRow row in dt.Rows)
                {
                    string value = row[column].ToString();
                    if (string.IsNullOrEmpty(value)) continue;//过滤空白项
                    string[] values = value.Split('(');
                    double valueDgt = Convert.ToDouble(values.FirstOrDefault());
                    valueDgt *= values.LastOrDefault().Contains("%") ? 10000 : values.LastOrDefault().Contains("‰") ? 1000 : 1;//不同单位转换为ppm
                    if (Max < valueDgt)
                    {
                        drMax[column] = value;
                        Max = valueDgt;
                    }

                    if (Min > valueDgt)
                    {
                        drMin[column] = value;
                        Min = valueDgt;
                    }
                    Ava += valueDgt;
                }
                Ava /= dt.Rows.Count;
                int iUnitConvert = sMajorUnit.Contains("%") ? 10000 : sMajorUnit.Contains("‰") ? 1000 : 1;//换算回主单位对应的系数
                drAva[column] = (Ava / iUnitConvert).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + sMajorUnit;//ppm转换回主单位
                foreach (DataRow row in dt.Rows)
                {
                    string value = row[column].ToString();
                    if (string.IsNullOrEmpty(value)) continue;//过滤空白项
                    string[] values = value.Split('(');
                    double valueDgt = Convert.ToDouble(values.FirstOrDefault());
                    valueDgt *= values.LastOrDefault().Contains("%") ? 10000 : values.LastOrDefault().Contains("‰") ? 1000 : 1;
                    SD += Math.Pow(valueDgt - Ava, 2);
                }
                SD = Math.Sqrt(SD / (dt.Rows.Count - 1));
                drSD[column] = (SD / iUnitConvert).ToString("f" + WorkCurveHelper.SoftWareContentBit.ToString()) + sMajorUnit;
            }
            dt.Rows.Add(drMax);
            dt.Rows.Add(drMin);
            dt.Rows.Add(drAva);
            dt.Rows.Add(drSD);
            List<DataRow> lstRows = new List<DataRow>();
            lstRows.Add(drMax);
            lstRows.Add(drMin);
            lstRows.Add(drAva);
            lstRows.Add(drSD);
            return lstRows;
        }

        /// <summary>
        /// 把整形数字合并成字符串
        /// </summary>
        /// <param name="intArray">当前整形数组</param>
        /// <returns></returns>
        public static string ConvertArrayToString(int[] intArray)
        {
            //string tempString = string.Empty;
            //if (intArray.Length == 0)
            //    return tempString;
            ////foreach (int tempInt in intArray)
            ////    tempString += tempInt + ",";
            ////tempString = tempString.Substring(0, tempString.Length - 1);
            //for (int i = 0; i < intArray.Length - 1;i++ )
            //{
            //    tempString += intArray[i] + ",";
            //}
            //tempString += intArray[intArray.Length - 1];
            //return tempString;
            return Helper.ToStrs(intArray);
        }

        /// <summary>
        /// 分拆字符串生成整形数组
        /// </summary>
        /// <param name="str">待分拆字符串</param>
        /// <returns></returns>
        public static int[] ConvertToArrayInterger(string str)
        {
            string[] strSplit = str.Split(',');
            int[] Data = new int[strSplit.Length];
            if (strSplit.Length == 0)
                return null;
            for (int i = 0; i < strSplit.Length; i++)
                Data[i] = int.Parse(strSplit[i]);
            return Data;
        }

        #region 各个DataGridViewW初始化备份

        //public static void InitTestResultStatics(ElementList Elements, DataGridViewW elemGrid, DataGridViewW statGrid)
        //{
        //    //元素栏
        //    DataGridViewColumn[] elemColumns = new DataGridViewColumn[Elements.Items.Count + 1 + Elements.CustomFields.Count];
        //    //统计栏
        //    DataGridViewColumn[] statColumns = new DataGridViewColumn[staticsColumnCount];

        //    //元素栏设置
        //    elemColumns[0] = new DataGridViewTextBoxColumn();
        //    elemColumns[0].Name = "elemColumns0";
        //    elemColumns[0].HeaderText = Info.Number;
        //    elemColumns[0].ReadOnly = true;
        //    elemColumns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
        //    elemColumns[0].Width = 50;
        //    //状态栏设置
        //    statColumns[0] = new DataGridViewTextBoxColumn();
        //    statColumns[0].Name = "statColumns0";///////////////////////////////////////////
        //    statColumns[0].HeaderText = " ";
        //    statColumns[0].ReadOnly = true;
        //    statColumns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
        //    statColumns[0].Width = 50;

        //    for (int i = 0; i < elemColumns.Length - 1; i++)
        //    {
        //        elemColumns[i + 1] = new DataGridViewTextBoxColumn();
        //        //elemColumns[i + 1].Name = "elemColumns" + i.ToString();
        //        elemColumns[i + 1].Name = "elemColumns" + (i + 1).ToString(); ///////Modify By Tianchunhua
        //        if (i < Elements.Items.Count)
        //            elemColumns[i + 1].HeaderText = Elements.Items[i].Caption;
        //        else elemColumns[i + 1].HeaderText = Elements.CustomFields[i - Elements.Items.Count].Name;
        //        elemColumns[i + 1].ReadOnly = true;
        //        elemColumns[i + 1].SortMode = DataGridViewColumnSortMode.NotSortable;
        //        elemColumns[i + 1].Width = 60;
        //    }

        //    string[] staticsHeadText = { Info.MaxValue, Info.MinValue, Info.MeanValue, Info.SDValue };
        //    for (int i = 0; i < staticsColumnCount - 1; i++)
        //    {
        //        statColumns[i + 1] = new DataGridViewTextBoxColumn();
        //        statColumns[i + 1].Name = "statColumns" + (i + 1).ToString();////////潘凤修改
        //        //statColumns[i + 1].Name = "statColumns" + i.ToString();
        //        statColumns[i + 1].HeaderText = staticsHeadText[i];// Elements.Item[i].Caption;
        //        statColumns[i + 1].ReadOnly = true;
        //        statColumns[i + 1].SortMode = DataGridViewColumnSortMode.NotSortable;
        //        statColumns[i + 1].Width = 60;
        //    }

        //    //清空表格
        //    elemGrid.Columns.Clear();
        //    statGrid.Columns.Clear();
        //    //添加数据
        //    //状态栏为四行



        //    elemGrid.Columns.AddRange(elemColumns);
        //    statGrid.Columns.AddRange(statColumns);
        //    statGrid.RowCount = 10;
        //    //元素栏行数为测量次数
        //    elemGrid.RowCount = 10;
        //    for (int j = 0; j < elemColumns.Length - 1; j++)
        //    {
        //        statGrid.Rows[j].Cells["statColumns0"].Value = Elements.Items[j].Caption.ToString();
        //    }
        //}
        //public static void InitDataGridPage(DataGridViewW infoGrid, string tabPageName)
        //{
        //    if (tabPageName == Resource.Calibration)
        //    {
        //        DataGridViewTextBoxColumn dataColumn = new DataGridViewTextBoxColumn();
        //        dataColumn.Name = "nameColumn";
        //        infoGrid.Columns.Add(dataColumn);

        //        dataColumn = new DataGridViewTextBoxColumn();
        //        dataColumn.Name = "valueColumn";
        //        infoGrid.Columns.Add(dataColumn);
        //        infoGrid.ColumnHeadersVisible = false;
        //        infoGrid.RowCount = 13;

        //        infoGrid["nameColumn", 0].Value = Resource.Calibration;
        //        infoGrid["nameColumn", 1].Value = Resource.MeasureTime;
        //        infoGrid["nameColumn", 2].Value = Resource.Voltage;
        //        infoGrid["nameColumn", 3].Value = Resource.Current;
        //        infoGrid["nameColumn", 4].Value = Resource.Filter;
        //        infoGrid["nameColumn", 5].Value = Resource.Collimator;
        //        infoGrid["nameColumn", 6].Value = Resource.VacuumizeByTime;
        //        infoGrid["nameColumn", 7].Value = Resource.AdjustCountRate;
        //        infoGrid["nameColumn", 8].Value = Resource.MaxCountRate;
        //        infoGrid["nameColumn", 9].Value = Resource.MinCountRate;
        //        infoGrid["nameColumn", 10].Value = Resource.InitalElem;
        //        infoGrid["nameColumn", 11].Value = Resource.InitalChann;
        //        infoGrid["nameColumn", 12].Value = Resource.MeasureParam;

        //    }
        //    else if (tabPageName == Resource.Spectrum)
        //    {
        //        DataGridViewTextBoxColumn dataColumn = new DataGridViewTextBoxColumn();
        //        dataColumn.Name = "colSpectrumName";
        //        infoGrid.Columns.Add(dataColumn);

        //        dataColumn = new DataGridViewTextBoxColumn();
        //        dataColumn.Name = "valueColumn";
        //        infoGrid.Columns.Add(dataColumn);
        //        infoGrid.ColumnHeadersVisible = false;
        //        infoGrid.RowCount = 13;

        //        infoGrid["colSpectrumName", 0].Value = Resource.SampleName;
        //        infoGrid["colSpectrumName", 1].Value = Resource.Supplier;
        //        infoGrid["colSpectrumName", 2].Value = Resource.Weight;
        //        infoGrid["colSpectrumName", 3].Value = Resource.Shape;
        //        infoGrid["colSpectrumName", 4].Value = Resource.MeasureDate;
        //        infoGrid["colSpectrumName", 5].Value = Resource.MeasureTime;
        //        infoGrid["colSpectrumName", 6].Value = Resource.Operator;
        //        infoGrid["colSpectrumName", 7].Value = Resource.Current;
        //        infoGrid["colSpectrumName", 8].Value = Resource.Voltage;
        //        infoGrid["colSpectrumName", 9].Value = Resource.Filter;
        //        infoGrid["colSpectrumName", 10].Value = Resource.Collimator;
        //        infoGrid["colSpectrumName", 11].Value = Resource.Channel;
        //        infoGrid["colSpectrumName", 12].Value = Resource.Summary;
        //    }
        //    else if (tabPageName == Resource.Qualitative)
        //    {
        //        DataGridViewTextBoxColumn dataColumn = new DataGridViewTextBoxColumn();
        //        dataColumn.Name = "colQualitativeName";
        //        infoGrid.Columns.Add(dataColumn);

        //        dataColumn = new DataGridViewTextBoxColumn();
        //        dataColumn.Name = "valueColumn";
        //        infoGrid.Columns.Add(dataColumn);
        //        infoGrid.ColumnHeadersVisible = false;
        //        infoGrid.RowCount = 9;

        //        infoGrid["colQualitativeName", 0].Value = Resource.Count;
        //        infoGrid["colQualitativeName", 1].Value = Resource.Channel;
        //        infoGrid["colQualitativeName", 2].Value = Resource.Energy;
        //        infoGrid["colQualitativeName", 3].Value = "Ka";
        //        infoGrid["colQualitativeName", 4].Value = "Kb";
        //        infoGrid["colQualitativeName", 5].Value = "La";
        //        infoGrid["colQualitativeName", 6].Value = "Lb";
        //        infoGrid["colQualitativeName", 7].Value = "Lr";
        //        infoGrid["colQualitativeName", 8].Value = "Le";

        //    }
        //    else if (tabPageName == Resource.Device)
        //    {
        //        DataGridViewTextBoxColumn dataColumn = new DataGridViewTextBoxColumn();
        //        dataColumn.Name = "colDeviceName";
        //        infoGrid.Columns.Add(dataColumn);

        //        dataColumn = new DataGridViewTextBoxColumn();
        //        dataColumn.Name = "valueColumn";
        //        infoGrid.Columns.Add(dataColumn);
        //        infoGrid.ColumnHeadersVisible = false;
        //        infoGrid.RowCount = 9;

        //        infoGrid["colDeviceName", 0].Value = Resource.Current;
        //        infoGrid["colDeviceName", 1].Value = Resource.Voltage;
        //        infoGrid["colDeviceName", 2].Value = Resource.CoarseCode;
        //        infoGrid["colDeviceName", 3].Value = Resource.FineCode;
        //        infoGrid["colDeviceName", 4].Value = Resource.CountRate;
        //        infoGrid["colDeviceName", 5].Value = Resource.MeasureTime;
        //        infoGrid["colDeviceName", 6].Value = Resource.Channel;
        //        infoGrid["colDeviceName", 7].Value = Resource.Count;
        //        infoGrid["colDeviceName", 8].Value = Resource.VacuumDegree;
        //    }

        //    for (int i = 0; i < infoGrid.Rows.Count; i++)
        //        infoGrid.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(247)))), ((int)(((byte)(254)))));
        //}

        //public static void RefreshDataInfoGrid(DataGridViewW infoGrid, DeviceParameter DeviceParam)
        //{
        //    if (infoGrid == null)
        //        return;
        //    if (WorkCurveHelper.WorkCurveCurrent == null)
        //    {
        //        foreach (DataGridViewRow dgvr in infoGrid.Rows)
        //        {
        //            dgvr.Cells["valueColumn"].Value = null;
        //        }
        //        return;
        //    }

        //    //infoGrid["valueColumn", 0].Value = WorkCurveHelper.WorkCurveCurrent.;
        //    infoGrid["valueColumn", 1].Value = DeviceParam.PrecTime;
        //    infoGrid["valueColumn", 2].Value = DeviceParam.TubVoltage;
        //    infoGrid["valueColumn", 3].Value = DeviceParam.TubCurrent;
        //    infoGrid["valueColumn", 4].Value = DeviceParam.FilterIdx;
        //    infoGrid["valueColumn", 5].Value = DeviceParam.CollimatorIdx;
        //    infoGrid["valueColumn", 6].Value = DeviceParam.IsVacuum ? Resource.Yes : Resource.No;
        //    infoGrid["valueColumn", 7].Value = DeviceParam.IsAdjustRate ? Resource.Yes : Resource.No;
        //    infoGrid["valueColumn", 8].Value = DeviceParam.MinRate;
        //    infoGrid["valueColumn", 9].Value = DeviceParam.MaxRate;
        //    //infoGrid["valueColumn", 10].Value = TestDevice.InitParam.ElemName;
        //    //infoGrid["valueColumn", 11].Value = TestDevice.InitParam.Channel;
        //    //infoGrid["valueColumn", 12].Value = this.conditionToolComboBox.SelectedItem;
        //    //// 修改：12-5-2009 田春华
        //    //// 目的：   针对edx3600作调整
        //    //if (TestDevice.Name.Equals(EDX3600, StringComparison.OrdinalIgnoreCase))
        //    //{
        //    //    infoGrid["valueColumn", 2].Value = TestDevice.DeviceParam.TubVoltage.ToString("f1");
        //    //}
        //}

        //public static void RefreshDataQualitative(DataGridViewW dgvQualitative, XRFChart xrfChart)
        //{
        //    dgvQualitative["valueColumn", 0].Value = 0;//当前值
        //    dgvQualitative["valueColumn", 1].Value = xrfChart.ICurrentChannel;//当前道
        //    //if (Curve != null)
        //    //{
        //    double energya = DemarcateEnergyHelp.GetEnergy(xrfChart.ICurrentChannel);//能量
        //    // 修改：12/8/2009 刘敏
        //    // 目的：　  显示为"0.000"格式
        //    //dgvQualitative["colQualitativeValue", 2].Value = energy.ToString("000.000");
        //    dgvQualitative["valueColumn", 2].Value = energya.ToString("0.000");
        //    dgvQualitative["valueColumn", 3].Value = GetString(xrfChart.Atoms.FindAll(delegate(Atom s) { return Math.Abs(s.Ka - energya) <= 0.165; }));//得到特定能量特定谱线的元素
        //    dgvQualitative["valueColumn", 4].Value = GetString(xrfChart.Atoms.FindAll(delegate(Atom s) { return Math.Abs(s.Kb - energya) <= 0.165; }));
        //    dgvQualitative["valueColumn", 5].Value = GetString(xrfChart.Atoms.FindAll(delegate(Atom s) { return Math.Abs(s.La - energya) <= 0.165; }));
        //    dgvQualitative["valueColumn", 6].Value = GetString(xrfChart.Atoms.FindAll(delegate(Atom s) { return Math.Abs(s.Lb - energya) <= 0.165; }));
        //    dgvQualitative["valueColumn", 7].Value = GetString(xrfChart.Atoms.FindAll(delegate(Atom s) { return Math.Abs(s.Lr - energya) <= 0.165; }));
        //    dgvQualitative["valueColumn", 8].Value = GetString(xrfChart.Atoms.FindAll(delegate(Atom s) { return Math.Abs(s.Le - energya) <= 0.165; }));
        //    //}

        //    energya.ToString("f4");

        //}
        #endregion

        /// <summary>
        /// 刷新定性分析DataGridViewW 
        /// </summary>
        /// <param name="cout">测试次数</param>
        /// <param name="dgvQualitative">定性分析对象</param>
        /// <param name="icurrentChanle">信道的当前值</param>
        /// <param name="listAtom">所有的原子</param>
        //public static void RefreshDataQualitative(int cout, Qualitative dgvQualitative, int icurrentChanle, double energy)
        //{
        //    dgvQualitative.Count = cout;//当前值
        //    dgvQualitative.Channel = icurrentChanle;//当前道
        //    dgvQualitative.Energy = energy.ToString("0.000");
        //    //dgvQualitative.Ka = GetString(listAtom.FindAll(delegate(Atom s) { return Math.Abs(s.Ka - energya) <= 0.165; }));//得到特定能量特定谱线的元素
        //    //dgvQualitative.Kb = GetString(listAtom.FindAll(delegate(Atom s) { return Math.Abs(s.Kb - energya) <= 0.165; }));
        //    //dgvQualitative.La = GetString(listAtom.FindAll(delegate(Atom s) { return Math.Abs(s.La - energya) <= 0.165; }));
        //    //dgvQualitative.Lb = GetString(listAtom.FindAll(delegate(Atom s) { return Math.Abs(s.Lb - energya) <= 0.165; }));
        //    //dgvQualitative.Lr = GetString(listAtom.FindAll(delegate(Atom s) { return Math.Abs(s.Lr - energya) <= 0.165; }));
        //    //dgvQualitative.Le = GetString(listAtom.FindAll(delegate(Atom s) { return Math.Abs(s.Le - energya) <= 0.165; }));
        //    //energya.ToString("f4");
        //    if (icurrentChanle != 0)
        //        FillQualitative(dgvQualitative, energy);
        //}

        ///// <summary>
        ///// 填充定性分析对象
        ///// </summary>
        ///// <param name="dgvQualitative"></param>
        ///// <param name="energya"></param>
        //private static void FillQualitative(Qualitative dgvQualitative, double energya)
        //{
        //    List<Atom> findAtomKa = new List<Atom>();
        //    List<Atom> findAtomKb = new List<Atom>();
        //    List<Atom> findAtomLa = new List<Atom>();
        //    List<Atom> findAtomLb = new List<Atom>();
        //    List<Atom> findAtomLr = new List<Atom>();
        //    List<Atom> findAtomLe = new List<Atom>();
        //    foreach (Atom atom in listAtom)
        //    {
        //        if (Math.Abs(atom.Ka - energya) <= EnergyWindow)
        //            findAtomKa.Add(atom);
        //        if (Math.Abs(atom.Kb - energya) <= EnergyWindow)
        //            findAtomKb.Add(atom);
        //        if (Math.Abs(atom.La - energya) <= EnergyWindow)
        //            findAtomLa.Add(atom);
        //        if (Math.Abs(atom.Lb - energya) <= EnergyWindow)
        //            findAtomLb.Add(atom);
        //        if (Math.Abs(atom.Lr - energya) <= EnergyWindow)
        //            findAtomLr.Add(atom);
        //        if (Math.Abs(atom.Le - energya) <= EnergyWindow)
        //            findAtomLe.Add(atom);
        //    }
        //    dgvQualitative.Ka = GetString(findAtomKa);
        //    dgvQualitative.Kb = GetString(findAtomKb);
        //    dgvQualitative.La = GetString(findAtomLa);
        //    dgvQualitative.Lb = GetString(findAtomLb);
        //    dgvQualitative.Lr = GetString(findAtomLr);
        //    dgvQualitative.Le = GetString(findAtomLe);
        //}

        //private static double EnergyWindow = 0.3;
        ///// <summary>
        ///// 获得特定线系能量附近的原子
        ///// </summary>
        ///// <param name="Energy">线系能量</param>
        ///// <param name="Line">线系</param>
        ///// <returns></returns>
        //public static string GetAtoms(double energy, XLine line)
        //{
        //    int index = (int)line - (int)XLine.Ka;
        //    string names = string.Empty;
        //    for (int i = 0; i < listAtom.Count; i++)
        //    {
        //        if (Math.Abs(listAtom[i].Energy[index] - energy) <= EnergyWindow)
        //        {
        //            names = names + listAtom[i].AtomName + " ";
        //        }
        //    }
        //    return names;
        //}

        /// <summary>
        /// 查找所有的原子信息
        /// </summary>
        /// <param name="listAtom"></param>
        /// <returns></returns>
        public static string GetString(List<Atom> listAtom)
        {
            if (listAtom == null || listAtom.Count == 0)
                return null;
            string stringList = string.Empty;
            foreach (Atom atom in listAtom)
                stringList += atom.AtomName + " ";
            return stringList;
        }

        #region 中缀转后缀
        /// <summary>
        /// 中缀表达式转换为后缀表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string InfixToPostfix(string expression)
        {
            Stack<char> operators = new Stack<char>();
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < expression.Length; i++)
            {
                char ch = expression[i];
                if (char.IsWhiteSpace(ch)) continue;
                switch (ch)
                {
                    case '+':
                    case '-':
                        while (operators.Count > 0)
                        {
                            char c = operators.Pop();   //pop Operator
                            if (c == '(')
                            {
                                operators.Push(c);      //push Operator
                                break;
                            }
                            else
                            {
                                result.Append(c);
                            }
                        }
                        operators.Push(ch);
                        result.Append(" ");
                        break;
                    case '*':
                    case '/':
                        while (operators.Count > 0)
                        {
                            char c = operators.Pop();
                            if (c == '(')
                            {
                                operators.Push(c);
                                break;
                            }
                            else
                            {
                                if (c == '+' || c == '-')
                                {
                                    operators.Push(c);
                                    break;
                                }
                                else
                                {
                                    result.Append(c);
                                }
                            }
                        }
                        operators.Push(ch);
                        result.Append(" ");
                        break;
                    case '(':
                        operators.Push(ch);
                        break;
                    case ')':
                        while (operators.Count > 0)
                        {
                            char c = operators.Pop();
                            if (c == '(')
                            {
                                break;
                            }
                            else
                            {
                                result.Append(c);
                            }
                        }
                        break;
                    default:
                        result.Append(ch);
                        break;
                }
            }
            while (operators.Count > 0)
            {
                result.Append(operators.Pop()); //pop All Operator
            }
            return result.ToString();
        }
        #endregion

        #region 求值经典算法
        /// <summary>
        /// 求值的经典算法
        /// </summary>
        /// <param name="expression">字符串表达式</param>
        /// <returns></returns>
        public static double Parse(string expression)
        {
            string postfixExpression = InfixToPostfix(expression);
            Stack<double> results = new Stack<double>();
            double x, y;
            for (int i = 0; i < postfixExpression.Length; i++)
            {
                char ch = postfixExpression[i];
                if (char.IsWhiteSpace(ch)) continue;
                switch (ch)
                {
                    case '+':
                        y = results.Pop();
                        x = results.Pop();
                        results.Push(x + y);
                        break;
                    case '-':
                        y = results.Pop();
                        x = results.Pop();
                        results.Push(x - y);
                        break;
                    case '*':
                        y = results.Pop();
                        x = results.Pop();
                        results.Push(x * y);
                        break;
                    case '/':
                        y = results.Pop();
                        x = results.Pop();
                        results.Push(x / y);
                        break;
                    default:
                        int pos = i;
                        StringBuilder operand = new StringBuilder();
                        do
                        {
                            operand.Append(postfixExpression[pos]);
                            pos++;
                        } while (char.IsDigit(postfixExpression[pos]) || postfixExpression[pos] == '.');
                        i = --pos;

                        double d = 0.0;
                        try
                        {
                            d = double.Parse(operand.ToString());
                        }
                        catch (System.Exception )
                        {

                        }
                        results.Push(d);
                        break;
                }
            }
            return results.Peek();
        }
        #endregion

        public static bool CustomFieldByFortum(string expression, string[] elements, string[] values, int index, out double outValue)
        {
            outValue = double.Epsilon;
            if (elements == null || elements.Length == 0 || values == null
            || values.Length == 0 || elements.Length != values.Length)
                return false;
            for (int i = 0; i < elements.Length; i++)
            {
                string[] str = values[i].Split(',');
                for (int j = 0; j < str.Length; j++)
                {
                    if (j == index)
                    {
                        if (str[j] == "")
                            return false;
                        string[] exps = expression.Split(new char[] { '+', '-', '*', '/', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int k = 0; k < exps.Length; k++)
                        {
                            if (String.Compare(exps[k], elements[i], true) == 0)
                                expression = expression.Replace(exps[k], str[j]);
                        }
                        break;
                    }
                }
            }

            outValue = Parse(expression);
            return true;
        }


        public static bool ExpressionContainElements(CustomField field)
        {
            string[] exps = field.Expression.Split(new char[] { '+', '-', '*', '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (exps.Length <= 1) return false;
            for (int i = 0; i < exps.Length; i++)
            {
                try
                {

                    double count = double.Parse(exps[i]);
                }
                catch
                {
                    CurveElement elements = WorkCurveHelper.WorkCurveCurrent.ElementList.Items.ToList().Find(w => String.Compare(w.Caption, exps[i], true) == 0);
                    if (elements == null)
                    {
                        return false;
                    }
                }


            }
            return true;
        }

        /// <summary>
        /// 找chann点附近找峰的高度和半高宽
        /// </summary>
        /// <returns></returns>
        public static void FitPeakGauss(int chann, int sysFwhm, ref double fwhm, ref double ph,int[] Data)
        {
            //找峰的理论最高点
            int ch = chann;
            if (chann - sysFwhm < 0 || chann + sysFwhm >= Data.Length)
            {
                fwhm = 0;
                ph = 0;
                return;
            }
            for (int i = chann - sysFwhm; i <= chann + sysFwhm; i++)
            {
                if (Data[ch] < Data[i])
                {
                    ch = i;
                }
            }
            int value = Data[ch] / 2;
            int high = ch;
            int low = ch;
            for (int i = ch + 1; i < Data.Length; i++)
            {
                if (Data[i] <= value)
                {
                    high = i;
                    break;
                }
            }
            for (int i = ch - 1; i > 0; i--)
            {
                if (Data[i] <= value)
                {
                    low = i;
                    break;
                }
            }
            int pa = 0;
            int pb = 0;
            for (int i = low; i <= high; i++)
            {
                pa += Data[i] * i;
                pb += Data[i];
            }
            double pp = pa * 1.0 / pb;//理论峰位置
            //边界
            ph = Data[ch];
            if (Data[low + 1] == Data[low] || Data[high] == Data[high - 1])
            {
                fwhm = 0;
                return;
            }
            double fineLow = low + (Data[low] - Data[ch] / 2.0) / (Data[low + 1] - Data[low]);
            double fineHigh = high + (Data[high] - Data[ch] / 2.0) / (Data[high] - Data[high - 1]);
            fwhm = fineHigh - fineLow;

        }
    }
}
