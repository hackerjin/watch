using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;
using Lephone.Util;
using Skyray.EDXRFLibrary;
using System.Windows.Forms;
using Skyray.Controls;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Skyray.EDX.Common
{
    public class ThickCaculate:BaseMessage
    {
        /// <summary>
        /// 测厚结果对象
        /// </summary>
        public List<ThickCaculateRecord> measurementResult { set; get; }

        [Auto("行数")]
        public int rowNumber { set; get; }

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public ThickCaculate()
        {
            this.IsFixed = false;
            this.rowNumber = 1;
            this.Position = 4;
            this.type = DataGridViewType.TestThick;
            measurementResult = new List<ThickCaculateRecord>();
        }

        /// <summary>
        /// 带参数构造函数
        /// </summary>
        /// <param name="result"></param>
        /// <param name="rowNumber"></param>
        public ThickCaculate(List<ThickCaculateRecord> result, int rowNumber)
        {
            this.measurementResult = result;
            this.rowNumber = rowNumber;
        }
    }

    public class ThickCaculateRecord
    {
        /// <summary>
        /// 计数
        /// </summary>
        public int Count { set; get; }

        /// <summary>
        /// 元素的名称
        /// </summary>
        public string Elements { set; get; }

        /// <summary>
        /// 元素对于的值
        /// </summary>
        public double ElementsThickValue { set; get; }


        public double ElementsContextValue { set; get; }


        public string ElementsThickUnit { set; get; }

        public string ElementsContextUnit { set; get; }

        /// <summary>
        /// 统计记录构造函数
        /// </summary>
        /// <param name="count"></param>
        /// <param name="elements"></param>
        /// <param name="elementValue"></param>
        public ThickCaculateRecord(int count, string elements, double elementValue, double elementsContextValue,
                                    string elementsThickUnit, string elementsContextUnit)
        {
            this.Count = count;
            this.Elements = elements;
            this.ElementsThickValue = elementValue;
            this.ElementsContextValue = elementsContextValue;
            this.ElementsThickUnit = elementsThickUnit;
            this.ElementsContextUnit = elementsContextUnit;
        }

        public ThickCaculateRecord(int count, string elements, double elementValue,
                                   string elementsThickUnit)
        {
            this.Count = count;
            this.Elements = elements;
            this.ElementsThickValue = elementValue;
            //this.ElementsContextValue = elementsContextValue;
            this.ElementsThickUnit = elementsThickUnit;
            //this.ElementsContextUnit = elementsContextUnit;
        }
    }

    public class ThickCaculateProcess : MessageInterface
    {
        /// <summary>
        /// 填充测厚容器对象
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="orientType"></param>
        /// <param name="isFixed"></param>
        /// <param name="tempobj"></param>
        /// <param name="dataGridView"></param>
        public override void RecordElementValusInfo(bool flag,bool orientType, bool isFixed, BaseMessage tempobj, Skyray.Controls.DataGridViewW dataGridView,ElementList list)
        {
            ThickCaculate measure = tempobj as ThickCaculate;
            int rowIndex = dataGridView.RowCount;
            if (measure.measurementResult.Count == 0)
                return;
            int Count = measure.measurementResult[0].Count;
            for (int i = 0; i < dataGridView.Columns.Count; i++)
            {
                ThickCaculateRecord record = measure.measurementResult.Find(w => String.Compare(w.Elements, dataGridView.Columns[i].Name,true)==0);
                if (i==0)
                {
                    dataGridView.Rows[Count - 1].Cells[i].Value = Count;
                    continue;
                }
                if (record == null)
                    continue;
                if (WorkCurveHelper.WorkCurveCurrent.IsThickShowContent)
                {
                    if (Convert.ToBoolean(i % 2))
                        dataGridView.Rows[Count - 1].Cells[i].Value = (record.ElementsThickUnit.ToString().Equals("um")) ? record.ElementsThickValue.ToString("f" + WorkCurveHelper.ThickBit) : (record.ElementsThickValue / 0.0254).ToString("f" + WorkCurveHelper.ThickBit);
                    else
                    {
                        double temop = record.ElementsContextValue >= 100 ? 100 : record.ElementsContextValue;
                        dataGridView.Rows[Count - 1].Cells[i].Value = (record.ElementsContextUnit.Equals("per")) ? temop.ToString("f" + WorkCurveHelper.ContentBit) : (temop * 10000).ToString("f" + WorkCurveHelper.ContentBit);
                    }
                }
                else
                    dataGridView.Rows[Count - 1].Cells[i].Value = (record.ElementsThickUnit.ToString().Equals("um")) ? record.ElementsThickValue.ToString("f" + WorkCurveHelper.ThickBit) : (record.ElementsThickValue / 0.0254).ToString("f" + WorkCurveHelper.ThickBit);
            }
            if (!flag)
                SendStatisticsInfo(dataGridView, list, Count);
        }


        /// <summary>
        /// 发送统计信息
        /// </summary>
        /// <param name="dataGrid"></param>
        public override void SendStatisticsInfo(DataGridViewW dataGrid, ElementList list,int currentTimes)
        {
            base.SendStatisticsInfo(dataGrid, list, currentTimes);
        }

        /// <summary>
        /// 构造测厚容器对象
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isFixed"></param>
        /// <param name="orientType"></param>
        /// <param name="datagridview"></param>
        public override void ContructDataContainer(BaseMessage obj, bool isFixed, bool orientType, Skyray.Controls.DataGridViewW datagridview)
        {
            ThickCaculate measure = obj as ThickCaculate;
            datagridview.AllowUserToResizeColumns = false;
            datagridview.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            datagridview.Columns.Clear();
            if (measure.measurementResult == null || measure.measurementResult.Count == 0)
                return;
            int rowNumber = measure.rowNumber;
            //if (orientType)
            //{
                if (!WorkCurveHelper.WorkCurveCurrent.IsThickShowContent)
                {
                    datagridview.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                    DataGridViewTextBoxColumn elemColumns = new DataGridViewTextBoxColumn();
                    elemColumns.Name = "Count";
                    elemColumns.HeaderText = Info.Number;
                    elemColumns.ReadOnly = true;
                    elemColumns.SortMode = DataGridViewColumnSortMode.NotSortable;
                    elemColumns.Width = 80;
                    datagridview.Columns.Add(elemColumns);
                    datagridview.RowCount = rowNumber;
                    foreach (ThickCaculateRecord meaurement in measure.measurementResult)
                    {
                        elemColumns = new DataGridViewTextBoxColumn();
                        elemColumns.Name = meaurement.Elements;
                        if (meaurement.ElementsThickUnit.Equals("ur"))
                            meaurement.ElementsThickUnit = "u〞";
                        elemColumns.HeaderText = meaurement.Elements + "(" + meaurement.ElementsThickUnit + ")";
                        elemColumns.ReadOnly = true;
                        elemColumns.SortMode = DataGridViewColumnSortMode.NotSortable;
                        elemColumns.Width = 100;
                        datagridview.Columns.Add(elemColumns);
                    }
                }
                else
                {
                   
                    DataGridViewTextBoxColumn elemColumns = new DataGridViewTextBoxColumn();
                    elemColumns.SortMode = DataGridViewColumnSortMode.NotSortable;
                    elemColumns.Name = "Count";
                    elemColumns.HeaderText = Info.Number;
                    elemColumns.ReadOnly = true;
                    elemColumns.Width = 80;
                    datagridview.Columns.Add(elemColumns);
                    datagridview.RowCount = rowNumber;
                    foreach (ThickCaculateRecord meaurement in measure.measurementResult)
                    {
                        elemColumns = new DataGridViewTextBoxColumn();
                        elemColumns.SortMode = DataGridViewColumnSortMode.NotSortable;
                        elemColumns.ReadOnly = true;
                        elemColumns.Name = meaurement.Elements;
                        if (meaurement.ElementsThickUnit.Equals("ur"))
                            meaurement.ElementsThickUnit = "u〞";
                        elemColumns.HeaderText = meaurement.Elements+"("+meaurement.ElementsThickUnit+")";
                        elemColumns.Width = 80;
                        datagridview.Columns.Add(elemColumns);

                        elemColumns = new DataGridViewTextBoxColumn();
                        elemColumns.SortMode = DataGridViewColumnSortMode.NotSortable;
                        elemColumns.ReadOnly = true;
                        elemColumns.Name = meaurement.Elements;
                        string headName = string.Empty;
                        if (meaurement.ElementsContextUnit.Equals("per"))
                            headName = "%";
                        else
                            headName = meaurement.ElementsContextUnit;
                        elemColumns.HeaderText = meaurement.Elements + "(" + headName + ")";
                        elemColumns.Width = 80;
                        datagridview.Columns.Add(elemColumns);
                    }
                    //elemColumns = new DataGridViewTextBoxColumn();
                    //elemColumns.Name = "";
                    //elemColumns.Visible = false;
                    //elemColumns.Width = 80;
                    //datagridview.Columns.Add(elemColumns);
                }
            //}
            datagridview.MultiSelect = false;
        }

        #region
        int top = 0;
        int left = 0;
        int height = 0;
        int width1 = 0;

        void datagridview_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridView dgv = (DataGridView)(sender);
            if (!WorkCurveHelper.WorkCurveCurrent.IsThickShowContent)
                return;
            if (e.RowIndex == -1 && e.ColumnIndex >= 0)
            {
                if (e.ColumnIndex == 0)
                {
                    top = e.CellBounds.Top;
                    left = e.CellBounds.Left;
                    height = e.CellBounds.Height;
                    width1 = e.CellBounds.Width;

                    string columnValue = "元素";
                    SizeF sf = e.Graphics.MeasureString(columnValue, e.CellStyle.Font);
                    float lstr = (width1 - sf.Width) / 2;
                    float rstr = (height/2 - sf.Height) / 2;

                    e.Graphics.DrawString(columnValue, e.CellStyle.Font,
                                                  new SolidBrush(e.CellStyle.ForeColor),
                                                    left + lstr,
                                                    top + height / 2+rstr,
                                                    StringFormat.GenericDefault);
                    return;
                }

                if (Convert.ToBoolean(e.ColumnIndex % 2))
                {
                    top = e.CellBounds.Top;
                    left = e.CellBounds.Left;
                    height = e.CellBounds.Height;
                    width1 = e.CellBounds.Width;
                }


                if (e.ColumnIndex+1 > dgv.Columns.Count-1)
                    return;
                int width2 = dgv.Columns[e.ColumnIndex+1].Width;
                Rectangle rect = new Rectangle(left, top, width1 + width2, e.CellBounds.Height);
                //using (Brush backColorBrush = new SolidBrush(e.CellStyle.BackColor))
                //{
                //    //抹去原来的cell背景
                //    e.Graphics.FillRectangle(backColorBrush, rect);
                //    Brush brush = new SolidBrush(Color.FromArgb(234, 247, 254));
                //    e.Graphics.FillRectangle(brush,rect);
                //}
                Color Color1 = Color.FromArgb(227, 239, 255);
                Color Color2 = Color.FromArgb(175, 210, 255);


                using (Brush backbrush = new LinearGradientBrush(rect, Color1, Color2, LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(backbrush, rect);
                }

                //using (Brush brush = new SolidBrush(Color.FromArgb(227, 239, 255)))
                //{
                //    //抹去原来的cell背景
                //    e.Graphics.FillRectangle(brush, rect);
                //}

                using (Pen gridLinePen = new Pen(dgv.GridColor))
                {
                    e.Graphics.DrawLine(gridLinePen, left, top, left + width1 + width2, top);

                    e.Graphics.DrawLine(gridLinePen, left, top, left, top+height);
                    e.Graphics.DrawLine(gridLinePen, left, top + height / 2, left + width1 + width2, top + height /

2);

                    e.Graphics.DrawLine(gridLinePen, left + width1, top + height / 2, left + width1, top + height);

                    //计算绘制字符串的位置
                    string columnValue = dgv.Columns[e.ColumnIndex].Name;
                    SizeF sf = e.Graphics.MeasureString(columnValue, e.CellStyle.Font);
                    float lstr = (width1 + width2 - sf.Width) / 2;
                    float rstr = (height / 2 - sf.Height) / 2;
                    //画出文本框

                    if (columnValue != "")
                    {
                        e.Graphics.DrawString(columnValue, e.CellStyle.Font,
                                                   new SolidBrush(e.CellStyle.ForeColor),
                                                     left + lstr,
                                                     top + rstr,
                                                     StringFormat.GenericDefault);
                    }

                    //计算绘制字符串的位置
                    columnValue = "厚度";
                    sf = e.Graphics.MeasureString(columnValue, e.CellStyle.Font);
                    lstr = (width1 - sf.Width) / 2;
                    rstr = (height / 2 - sf.Height) / 2;
                    //画出文本框

                    if (columnValue != "")
                    {
                        e.Graphics.DrawString(columnValue, e.CellStyle.Font,
                                                   new SolidBrush(e.CellStyle.ForeColor),
                                                     left + lstr,
                                                     top + height / 2 + rstr,
                                                     StringFormat.GenericDefault);
                    }


                    //计算绘制字符串的位置
                    columnValue = "含量";
                    sf = e.Graphics.MeasureString(columnValue, e.CellStyle.Font);
                    lstr = (width2 - sf.Width) / 2;
                    rstr = (height / 2 - sf.Height) / 2;
                    //画出文本框

                    if (columnValue != "")
                    {
                        e.Graphics.DrawString(columnValue, e.CellStyle.Font,
                                                   new SolidBrush(e.CellStyle.ForeColor),
                                                     left + width1 + lstr,
                                                     top + height / 2 + rstr,
                                                     StringFormat.GenericDefault);
                    }

                }
                e.Handled = true;


            }
        }
        #endregion
    }
}
