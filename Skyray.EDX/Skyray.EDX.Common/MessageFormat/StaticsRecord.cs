using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.Controls;
using System.Reflection;
using Lephone.Data.Definition;
using Lephone.Util;
using Skyray.EDXRFLibrary;

namespace Skyray.EDX.Common
{
    public class StaticsRecord
    {
        public string Elements { set; get; }

        [Auto("MaxValue", "最大值")]
        public string MaxValue { set; get; }

        [Auto("MinValue", "最小值")]
        public string MinValue { set; get; }

        [Auto("AverageValue", "平均值")]
        public string AverageValue { set; get; }

        [Auto("SDValue", "标准偏差")]
        public string SDValue { set; get; }

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public StaticsRecord()
        { 
        }

        /// <summary>
        /// 带参数构造函数
        /// </summary>
        /// <param name="maxValue"></param>
        /// <param name="minValue"></param>
        /// <param name="averageValue"></param>
        /// <param name="sdValue"></param>
        public StaticsRecord(string maxValue, string minValue, string averageValue, string sdValue)
        {
            this.MaxValue = maxValue;
            this.MinValue = minValue;
            this.AverageValue = averageValue;
            this.SDValue = sdValue;
        }
    }

    public class Statics : BaseMessage
    {
        /// <summary>
        /// 统计结果
        /// </summary>
        public List<StaticsRecord> staticsResult { set; get; }

        [Auto("行数")]
        public int rowNumber { set; get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Statics()
        {
            this.IsFixed = false;
            this.rowNumber = 1;
            this.Position = 5;
            this.type = DataGridViewType.StatisticsInfo;
            staticsResult = new List<StaticsRecord>();
        }

        /// <summary>
        /// 带参数构造函数
        /// </summary>
        /// <param name="staticsResult"></param>
        /// <param name="rowNumber"></param>
        public Statics(List<StaticsRecord> staticsResult,int rowNumber)
            :this()
        {
            this.staticsResult = staticsResult;
            this.rowNumber = rowNumber;
        }
    }
    public class StaticsProcess : MessageInterface
    {
        /// <summary>
        /// 填充统计数据容器
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="orientType"></param>
        /// <param name="isFixed"></param>
        /// <param name="tempObj"></param>
        /// <param name="dataGridView"></param>
        public override void RecordElementValusInfo(bool flag,bool orientType, bool isFixed,BaseMessage tempObj, DataGridViewW dataGridView,ElementList list)
        {
             Statics statics = tempObj as Statics;
             for (int j = 0; j < dataGridView.Rows.Count; j++)
             {
                 if (dataGridView.Rows[j].Cells["statColumns"].Value != null)
                 {
                     StaticsRecord targetRecord = statics.staticsResult.Find(delegate(StaticsRecord s)
                     { return String.Compare(s.Elements,dataGridView.Rows[j].Cells["statColumns"].Value.ToString(),true)==0; });
                     if (targetRecord != null)
                     {
                         dataGridView.Rows[j].Cells["MaxValue"].Value = targetRecord.MaxValue;
                         dataGridView.Rows[j].Cells["MinValue"].Value = targetRecord.MinValue;
                         dataGridView.Rows[j].Cells["MeanValue"].Value = targetRecord.AverageValue;
                         dataGridView.Rows[j].Cells["SDValue"].Value = targetRecord.SDValue;
                     }
                 }
             }
           
        }

        /// <summary>
        /// 构造统计容器对象
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isFixed"></param>
        /// <param name="orientType"></param>
        /// <param name="datagridview"></param>
        public override void ContructDataContainer(BaseMessage obj, bool isFixed, bool orientType, DataGridViewW datagridview)
        {
            Statics statics = obj as Statics;
            datagridview.AllowUserToResizeColumns = false;
            datagridview.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            datagridview.Columns.Clear();
            if (statics.staticsResult == null || statics.staticsResult.Count == 0)
                return;
            List<StaticsRecord> sourceRecord = statics.staticsResult;
            DataGridViewTextBoxColumn elemColumns = new DataGridViewTextBoxColumn();
            elemColumns.Name = "statColumns".ToString();
            elemColumns.HeaderText = Info.ElementName;
            elemColumns.ReadOnly = true;
            elemColumns.SortMode = DataGridViewColumnSortMode.NotSortable;
            elemColumns.Width = 80;
            datagridview.Columns.Add(elemColumns);
            datagridview.RowCount = sourceRecord.Count;
            string[] staticsHeadText = { Info.MaxValue, Info.MinValue, Info.MeanValue, Info.SDValue };
            elemColumns = new DataGridViewTextBoxColumn();
            elemColumns.Name = "MaxValue";
            elemColumns.HeaderText = Info.MaxValue;
            elemColumns.ReadOnly = true;
            elemColumns.SortMode = DataGridViewColumnSortMode.NotSortable;
            elemColumns.Width = 80;
            datagridview.Columns.Add(elemColumns);

            elemColumns = new DataGridViewTextBoxColumn();
            elemColumns.Name = "MinValue";
            elemColumns.HeaderText = Info.MinValue;
            elemColumns.ReadOnly = true;
            elemColumns.SortMode = DataGridViewColumnSortMode.NotSortable;
            elemColumns.Width = 80;
            datagridview.Columns.Add(elemColumns);

            elemColumns = new DataGridViewTextBoxColumn();
            elemColumns.Name = "MeanValue";
            elemColumns.HeaderText = Info.MeanValue;
            elemColumns.ReadOnly = true;
            elemColumns.SortMode = DataGridViewColumnSortMode.NotSortable;
            elemColumns.Width = 80;
            datagridview.Columns.Add(elemColumns);

            elemColumns = new DataGridViewTextBoxColumn();
            elemColumns.Name = "SDValue";
            elemColumns.HeaderText = Info.SDValue;
            elemColumns.ReadOnly = true;
            elemColumns.SortMode = DataGridViewColumnSortMode.NotSortable;
            elemColumns.Width = 80;
            datagridview.Columns.Add(elemColumns);

            if (sourceRecord != null && sourceRecord.Count > 0)
            {
                int j = 0;
                foreach (StaticsRecord meaurement in sourceRecord)
                {
                    datagridview.Rows[j].Cells["statColumns"].Value = meaurement.Elements;
                    j++;
                }
            }
            datagridview.MultiSelect = false;
            datagridview.Scroll += new ScrollEventHandler(datagridview_Scroll);
        }

        void datagridview_Scroll(object sender, ScrollEventArgs e)
        {
            //int i = 0;
        }
    }
}
