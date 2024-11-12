using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;
using Lephone.Util;
using Skyray.EDXRFLibrary;
using Skyray.Controls;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace Skyray.EDX.Common
{
    public class ThickStaticsRecord
    {
        public string Elements { set; get; }

        public string ContextUnit { set; get; }

        public string ThickUnit { set; get; }

        [Auto("MaxValue", "最大值")]
        public string ThickMaxValue { set; get; }

        public string ContextmaxValue { set; get; }

        [Auto("MinValue", "最小值")]
        public string ThickMinValue { set; get; }

        public string ContextminValue { set; get; }


        [Auto("AverageValue", "平均值")]
        public string ThickAverageValue { set; get; }

        public string ContextAverageValue { set; get; }

        [Auto("SDValue", "方差")]
        public string ThickSDValue { set; get; }

        public string ContextSDValue { set; get; }

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public ThickStaticsRecord()
        {
        }

        /// <summary>
        /// 带参数构造函数
        /// </summary>
        /// <param name="maxValue"></param>
        /// <param name="minValue"></param>
        /// <param name="averageValue"></param>
        /// <param name="sdValue"></param>
        public ThickStaticsRecord(string thickMaxValue, string thickMinValue, string thickAverageValue, string thickSDValue,
                                  string contextmaxValue, string contextminValue, string contextAverageValue, string contextSDValue)
        {
            this.ThickMaxValue = thickMaxValue;
            this.ThickMinValue = thickMinValue;
            this.ThickAverageValue = thickAverageValue;
            this.ThickSDValue = thickSDValue;
            this.ContextAverageValue = contextAverageValue;
            this.ContextmaxValue = contextmaxValue;
            this.ContextSDValue = contextSDValue;
            this.ContextminValue = contextminValue;
        }

        public ThickStaticsRecord(string thickMaxValue, string thickMinValue, string thickAverageValue, string thickSDValue)
        {
            this.ThickMaxValue = thickMaxValue;
            this.ThickMinValue = thickMinValue;
            this.ThickAverageValue = thickAverageValue;
            this.ThickSDValue = thickSDValue;
            //this.ContextAverageValue = contextAverageValue;
            //this.ContextmaxValue = contextmaxValue;
            //this.ContextSDValue = contextSDValue;
            //this.ContextminValue = contextminValue;
        }
    }

        public class ThickStatics : BaseMessage
        {
            /// <summary>
            /// 统计结果
            /// </summary>
            public List<ThickStaticsRecord> staticsResult { set; get; }

            [Auto("行数")]
            public int rowNumber { set; get; }

            /// <summary>
            /// 构造函数
            /// </summary>
            public ThickStatics()
            {
                this.IsFixed = false;
                this.rowNumber = 1;
                this.Position = 5;
                this.type = DataGridViewType.ThickStatics;
                staticsResult = new List<ThickStaticsRecord>();
            }

            /// <summary>
            /// 带参数构造函数
            /// </summary>
            /// <param name="staticsResult"></param>
            /// <param name="rowNumber"></param>
            public ThickStatics(List<ThickStaticsRecord> staticsResult, int rowNumber)
                : this()
            {
                this.staticsResult = staticsResult;
                this.rowNumber = rowNumber;
            }
        }
        public class ThickStaticsProcess : MessageInterface
        {
            /// <summary>
            /// 填充统计数据容器
            /// </summary>
            /// <param name="flag"></param>
            /// <param name="orientType"></param>
            /// <param name="isFixed"></param>
            /// <param name="tempObj"></param>
            /// <param name="dataGridView"></param>
            public override void RecordElementValusInfo(bool flag, bool orientType, bool isFixed, BaseMessage tempObj, DataGridViewW dataGridView, ElementList list)
            {
                ThickStatics statics = tempObj as ThickStatics;
                for (int j = 0; j < dataGridView.Rows.Count; j++)
                {
                    if ((string)dataGridView.Rows[j].Tag != null)
                    {
                        ThickStaticsRecord targetRecord = statics.staticsResult.Find(delegate(ThickStaticsRecord s)
                        { return s.Elements == (string)dataGridView.Rows[j].Tag; });
                        if (targetRecord != null)
                        {
                            if (!WorkCurveHelper.WorkCurveCurrent.IsThickShowContent)
                            {
                                dataGridView.Rows[j].Cells["MaxValue"].Value = targetRecord.ThickMaxValue;
                                dataGridView.Rows[j].Cells["MinValue"].Value = targetRecord.ThickMinValue;
                                dataGridView.Rows[j].Cells["MeanValue"].Value = targetRecord.ThickAverageValue;
                                dataGridView.Rows[j].Cells["SDValue"].Value = targetRecord.ThickSDValue;
                            }
                            else 
                            {
                                if (!Convert.ToBoolean(j % 2))
                                {
                                    dataGridView.Rows[j].Cells["MaxValue"].Value = targetRecord.ThickMaxValue;
                                    dataGridView.Rows[j].Cells["MinValue"].Value = targetRecord.ThickMinValue;
                                    dataGridView.Rows[j].Cells["MeanValue"].Value = targetRecord.ThickAverageValue;
                                    dataGridView.Rows[j].Cells["SDValue"].Value = targetRecord.ThickSDValue;
                                }
                                else
                                {
                                    dataGridView.Rows[j].Cells["MaxValue"].Value = targetRecord.ContextmaxValue;
                                    dataGridView.Rows[j].Cells["MinValue"].Value = targetRecord.ContextminValue;
                                    dataGridView.Rows[j].Cells["MeanValue"].Value = targetRecord.ContextAverageValue;
                                    dataGridView.Rows[j].Cells["SDValue"].Value = targetRecord.ContextSDValue;
                                }
                            }
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
                ThickStatics statics = obj as ThickStatics;
                datagridview.AllowUserToResizeColumns = false;
                datagridview.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                datagridview.Columns.Clear();
                if (statics.staticsResult == null || statics.staticsResult.Count == 0)
                    return;
                List<ThickStaticsRecord> sourceRecord = statics.staticsResult;
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
                    if (!WorkCurveHelper.WorkCurveCurrent.IsThickShowContent)
                    {
                        int j = 0;
                        foreach (ThickStaticsRecord meaurement in sourceRecord)
                        {
                            datagridview.Rows[j].Tag = meaurement.Elements;
                            if (meaurement.ThickUnit.Equals("ur"))
                                meaurement.ThickUnit = "u〞";
                            datagridview.Rows[j].Cells["statColumns"].Value = meaurement.Elements + "(" + meaurement.ThickUnit+")";
                            j++;
                        }
                    }
                    else
                    {
                        datagridview.RowHeadersWidth = 80;
                        datagridview.RowCount = 2 * sourceRecord.Count;

                        int j = 0;
                        foreach (ThickStaticsRecord meaurement in sourceRecord)
                        {
                            datagridview.Rows[j].Tag = meaurement.Elements;
                            if (meaurement.ThickUnit.Equals("ur"))
                                meaurement.ThickUnit = "u〞";
                            datagridview.Rows[j].Cells["statColumns"].Value = meaurement.Elements +"(" + meaurement.ThickUnit + ")";
                            j++;
                            datagridview.Rows[j].Tag = meaurement.Elements;
                            string headName = string.Empty;
                            if (meaurement.ContextUnit.Equals("per"))
                                headName = "%";
                            else
                                headName = meaurement.ContextUnit;
                            datagridview.Rows[j].Cells["statColumns"].Value = meaurement.Elements + "(" + headName + ")";
                            j++;
                        }
                    }
                }
                datagridview.MultiSelect = false;
            }

            #region
            int top = 0;
            int left = 0;
            int height1 = 0;
            int width = 0;
            void datagridview_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
            {
                DataGridView dgv = (DataGridView)(sender);
                if (!WorkCurveHelper.WorkCurveCurrent.IsThickShowContent)
                    return;
                if (e.ColumnIndex == -1)
                {
                    if (e.RowIndex == -1)
                    {
                        top = e.CellBounds.Top;
                        left = e.CellBounds.Left;
                        height1 = e.CellBounds.Height;
                        width = e.CellBounds.Width;

                        string columnValue = "元素";
                        SizeF sf = e.Graphics.MeasureString(columnValue, e.CellStyle.Font);
                        float rstr = (height1 / 2 - sf.Height) / 2;

                        e.Graphics.DrawString(columnValue, e.CellStyle.Font,
                                                      new SolidBrush(e.CellStyle.ForeColor),
                                                        left,
                                                        top + rstr + height1 / 3,
                                                        StringFormat.GenericDefault);
                        return;
                    }

                    if (Convert.ToBoolean(e.RowIndex % 2))
                    {
                        top = e.CellBounds.Top;
                        left = e.CellBounds.Left;
                        height1 = e.CellBounds.Height;
                        width = e.CellBounds.Width;
                    }
                    else
                    {
                        if (e.RowIndex + 1 > dgv.Rows.Count - 1)
                            return;
                        int height2 = dgv.Rows[e.RowIndex + 1].Height;

                        Rectangle rect1 = new Rectangle(left, top, width, height1 + height2);

                        Color Color1 = Color.FromArgb(227, 239, 255);
                        Color Color2 = Color.FromArgb(175, 210, 255);

                        using (Brush backbrush = new LinearGradientBrush(rect1, Color1, Color2, LinearGradientMode.Vertical))
                        {
                            e.Graphics.FillRectangle(backbrush, rect1);
                        }
                    }

                    

                    //using (Pen gridLinePen = new Pen(dgv.GridColor))
                    //{
                    //    e.Graphics.DrawLine(gridLinePen, left, top, left + width, top);
                    //    e.Graphics.DrawLine(gridLinePen, left + width / 2, top, left + width / 2, top + height1 + height2);
                    //    e.Graphics.DrawLine(gridLinePen, left + width / 2, top + height1, left + width, top + height1);

                    //    //计算绘制字符串的位置
                    //    string columnValue = dgv.Rows[e.RowIndex].Tag as string;
                    //    SizeF sf = e.Graphics.MeasureString(columnValue, e.CellStyle.Font);
                    //    float lstr = (height1 + height2 - sf.Height) / 2;
                    //    float rstr = (width / 2 - sf.Width) / 2;
                    //    //画出文本框

                    //    if (columnValue != "")
                    //    {
                    //        e.Graphics.DrawString(columnValue, e.CellStyle.Font,
                    //                                   new SolidBrush(e.CellStyle.ForeColor),
                    //                                     left + lstr,
                    //                                     top + rstr,
                    //                                     StringFormat.GenericDefault);
                    //    }


                    //    //计算绘制字符串的位置
                    //    columnValue = "厚度";
                    //    sf = e.Graphics.MeasureString(columnValue, e.CellStyle.Font);
                    //    lstr = (height1 - sf.Height) / 2;
                    //    rstr = (width / 2 - sf.Width) / 2;
                    //    //画出文本框

                    //    if (columnValue != "")
                    //    {
                    //        e.Graphics.DrawString(columnValue, e.CellStyle.Font,
                    //                                   new SolidBrush(e.CellStyle.ForeColor),
                    //                                     left + width / 2 + rstr,
                    //                                     top + lstr,
                    //                                     StringFormat.GenericDefault);
                    //    }


                    //    //计算绘制字符串的位置
                    //    columnValue = "含量";
                    //    sf = e.Graphics.MeasureString(columnValue, e.CellStyle.Font);
                    //    lstr = (height1 - sf.Height) / 2;
                    //    rstr = (width / 2 - sf.Width) / 2;
                    //    //画出文本框

                    //    if (columnValue != "")
                    //    {
                    //        e.Graphics.DrawString(columnValue, e.CellStyle.Font,
                    //                                   new SolidBrush(e.CellStyle.ForeColor),
                    //                                     left + width / 2 + rstr,
                    //                                     top + height1 + lstr,
                    //                                     StringFormat.GenericDefault);
                    //    }

                    //}
                    //e.Handled = true;
                }
            }
            #endregion
        }
}
