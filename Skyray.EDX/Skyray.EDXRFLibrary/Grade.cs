using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;

namespace Skyray.EDXRFLibrary
{
    class Grade
    {
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    /// <param name="elements">样本数组</param>
        //    /// <param name="ScalarContent">牌号库含量数据</param>
        //    /// <param name="u">校正参数</param>
        //    /// <returns></returns>
        //    public static bool GetGrade(GradeElement[] elements, double[,] ScalarContent, double u)
        //    {
        //        const int MAX_DISTANCE = 10000;
        //        const int ET_DISANCE = 10;
        //        //元素值，牌号最大值，牌号最小值、元素权重的临时记录变量
        //        double e_value, e_min, e_max, e_weight;
        //        //最近牌号的距离，位置，牌号序号
        //        double distance = MAX_DISTANCE;
        //        int t_grade = -1;
        //        //样本的长度，即：样本的元素个数
        //        int element_num = elements.Length;
        //        //标量库中标量的个数
        //        int grade_num = ScalarContent.GetLength(0);//获得行数
        //        double temp_dis = 0;
        //        for (int i = 0; i < grade_num; i++)
        //        {
        //            temp_dis = 0.0;
        //            for (int j = 0; j < element_num; j++)
        //            {
        //                e_value = ScalarContent[i, j];
        //                e_min = elements[j].Content * (1 - elements[j].Error);
        //                e_max = elements[j].Content * (1 + elements[j].Error);
        //                e_weight = elements[j].Weight;
        //                if (e_value < e_min)
        //                {
        //                    temp_dis += e_weight * (e_min - e_value) * (e_min - e_value);
        //                }
        //                if (e_value > e_max)
        //                {
        //                    temp_dis += e_weight * (e_max - e_value) * (e_max - e_value);
        //                }
        //            }
        //            temp_dis = Math.Sqrt(temp_dis);
        //            if (distance > temp_dis)
        //            {
        //                distance = temp_dis;
        //                t_grade = i;
        //            }
        //        }

        //        if (ET_DISANCE > distance || t_grade == -1)
        //        {
        //            return false;
        //        }
        //        //储存待测标量的原始含量总和
        //        double oldContentSum = 0.0;
        //        //储存待测标量的校正后含量总和
        //        double newContentSum = 0.0;
        //        for (int i = 0; i < element_num; i++)
        //        {
        //            oldContentSum += elements[i].Content;
        //            elements[i].Content = ScalarContent[t_grade, i] * u + (1 - u) * elements[i].Content;
        //            newContentSum += elements[i].Content;
        //        }
        //        //校正含量
        //        for (int i = 0; i < element_num; i++)
        //        {
        //            elements[i].Content *= oldContentSum / newContentSum;
        //        }
        //        return true;
        //    }
        //}
    }

    [DbContext("SamplesData")]
    public abstract class ElementMember : DbObjectModel<ElementMember>
    {
        /// <summary>
        /// 所属元素
        /// </summary>
        [BelongsTo, DbColumn("MatchSample_Id")]
        public abstract MatchSample MatchSampleMember { get; set; }

        public abstract string Element { get; set; }//元素名

        public abstract double Content { get; set; }//元素含量

        // public abstract ElementMember Init(string Element, double Content);
    }

    [DbContext("SamplesData")]
    public abstract class MatchSample : DbObjectModel<MatchSample>, ICloneable
    {

        public abstract int SampleID { get; set; } //编号

        public abstract string SampleName { get; set; } //样品规格型号

        public abstract string SampleType { get; set; } //标样样类型

        public abstract string SampleGrade { get; set; } //牌号
        ///<summary>
        /// 包含规格
        /// </summary>
        [HasMany(OrderBy = "id")]
        public abstract HasMany<ElementMember> Elements { get; set; }

        // public abstract MatchSample Init(string Caption, string SampleType);

        #region ICloneable 成员

        public object Clone()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    //public struct GradeElement
    //{
    //    public string Name;
    //    public double Content;
    //    public double Error;
    //    public double Weight;
    //}

}
