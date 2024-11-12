using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lephone.Data.Definition;
using Lephone.Util;
using Skyray.EDXRFLibrary;
using System.Reflection;
using Skyray.Controls;
using System.Windows.Forms;
using System.Drawing;


namespace Skyray.EDX.Common
{
    public class Measurement : BaseMessage
    {
        public List<MeasurementRecord> measurementResult { set; get; }

        [Auto("行数")]
        public int rowNumber { set; get; }

        public Measurement()
        {
            this.IsFixed = false;
            this.rowNumber = 1;
            this.Position = 4;
            this.type = DataGridViewType.TestResult;
            measurementResult = new List<MeasurementRecord>();
        }

        public Measurement(List<MeasurementRecord> result, int rowNumber)
        {
            this.measurementResult = result;
            this.rowNumber = rowNumber;
        }
    }

    public class MeasurementRecord
    {
        /// <summary>
        /// 计数
        /// </summary>
        public int Count { set; get; }

        /// <summary>
        /// 元素名称
        /// </summary>
        public string Elements { set; get; }

        /// <summary>
        /// 元素的值
        /// </summary>
        public double ElementsValue { set; get; }


        public string ElementUnit { set; get; }



        /// <summary>
        /// 带参数构造函数
        /// </summary>
        /// <param name="count"></param>
        /// <param name="elements"></param>
        /// <param name="elementValue"></param>
        public MeasurementRecord(int count, string elements, double elementValue, string elementUnit)
        {
            this.Count = count;
            this.Elements = elements;
            this.ElementsValue = elementValue;
            this.ElementUnit = elementUnit;
        }
    }

    public class MeasureRecordProcess : MessageInterface
    {
        /// <summary>
        /// 填充容器数据
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="orientType"></param>
        /// <param name="isFixed"></param>
        /// <param name="tempobj"></param>
        /// <param name="dataGridView"></param>
        public override void RecordElementValusInfo(bool flag, bool orientType, bool isFixed, BaseMessage tempobj, DataGridViewW dataGridView, ElementList list)
        {
            Measurement measure = tempobj as Measurement;
            int rowIndex = dataGridView.RowCount;
            int currentTimes = 0;
            if (measure.measurementResult.Count > 0)
            {
                currentTimes = measure.measurementResult[0].Count;
                bool showFlag = true;
                foreach (MeasurementRecord record in measure.measurementResult)
                {
                    dataGridView.Rows[record.Count - 1].Cells["Count"].Value = record.Count;
                    dataGridView.Rows[record.Count - 1].Cells[record.Elements].Value = (record.ElementUnit.Equals("per")) ? record.ElementsValue.ToString("f4") : (record.ElementsValue * 10000).ToString("f4");
                    if (WorkCurveHelper.DeviceFunctype == FuncType.Rohs && WorkCurveHelper.CurrentStandard != null)
                    {
                        List<StandardData> standardData = WorkCurveHelper.CurrentStandard.StandardDatas.ToList();
                        StandardData standSample = standardData.Find(w => String.Compare(w.ElementCaption, record.Elements, true) == 0);
                        try
                        {
                            if (standSample != null)
                            {
                                double elementDouble = record.ElementsValue * 10000;
                                if (elementDouble > standSample.StandardContent)
                                {
                                    Font font = dataGridView.Rows[record.Count - 1].Cells[record.Elements].Style.Font;
                                    dataGridView.Rows[record.Count - 1].Cells[record.Elements].Style.ForeColor = Color.Red;
                                    showFlag = false;
                                    dataGridView.Rows[record.Count - 1].Cells["Aditional"].Value = "false";
                                }
                            }
                        }
                        catch (FormatException e)
                        {
                            throw e;
                        }
                        if (showFlag)
                            {
                                dataGridView.Rows[record.Count - 1].Cells["Aditional"].Value = "true";
                            }
                    }

                    if (WorkCurveHelper.DeviceFunctype == FuncType.XRF && WorkCurveHelper.CategoryCurrent != null && WorkCurveHelper.WorkCurveCurrent.ElementList.IsReportCategory)
                    {
                        SpecificationsExample examples = WorkCurveHelper.MatchSpecifications(list);
                        dataGridView.Rows[record.Count - 1].Cells["Specifications"].Value = examples.ExampleName;
                    }
                }
               
                if (!flag)
                    SendStatisticsInfo(dataGridView, list, currentTimes);
            }
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
        /// 构造容器对象
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isFixed"></param>
        /// <param name="orientType"></param>
        /// <param name="datagridview"></param>
        public override void ContructDataContainer(BaseMessage obj, bool isFixed, bool orientType, DataGridViewW datagridview)
        {
            Measurement measure = obj as Measurement;
            datagridview.AllowUserToResizeColumns = false;
            datagridview.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            datagridview.Columns.Clear();
            if (measure.measurementResult == null || measure.measurementResult.Count == 0)
                return;
            int rowNumber = measure.rowNumber;
            //if (orientType)
            //{
                DataGridViewTextBoxColumn elemColumns = new DataGridViewTextBoxColumn();
                elemColumns.Name = "Count";
                elemColumns.HeaderText = Info.Number;
                elemColumns.ReadOnly = true;
                elemColumns.SortMode = DataGridViewColumnSortMode.NotSortable;
                elemColumns.Width = 80;
                datagridview.Columns.Add(elemColumns);

               
                datagridview.RowCount = rowNumber;
                foreach (MeasurementRecord meaurement in measure.measurementResult)
                {
                    elemColumns = new DataGridViewTextBoxColumn();
                    elemColumns.Name = meaurement.Elements;
                    string headName = string.Empty;
                    if (meaurement.ElementUnit.Equals("per"))
                        headName = "%";
                    else
                        headName = meaurement.ElementUnit;
                    elemColumns.HeaderText = meaurement.Elements + "(" + headName + ")";
                    elemColumns.ReadOnly = true;
                    elemColumns.SortMode = DataGridViewColumnSortMode.NotSortable;
                    elemColumns.Width = 80;
                    datagridview.Columns.Add(elemColumns);
                }
                if (WorkCurveHelper.DeviceFunctype == FuncType.Rohs && WorkCurveHelper.CurrentStandard !=null && !WorkCurveHelper.CurrentStandard.StandardName.IsNullOrEmpty())
                {
                    elemColumns = new DataGridViewTextBoxColumn();
                    elemColumns.Name = "Aditional";
                    elemColumns.HeaderText = WorkCurveHelper.CurrentStandard.StandardName;
                    elemColumns.ReadOnly = true;
                    elemColumns.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    elemColumns.SortMode = DataGridViewColumnSortMode.NotSortable;
                    datagridview.Columns.Add(elemColumns);
                }

                if (WorkCurveHelper.DeviceFunctype == FuncType.XRF && WorkCurveHelper.CategoryCurrent != null && WorkCurveHelper.WorkCurveCurrent.ElementList.IsReportCategory)
                {
                    elemColumns = new DataGridViewTextBoxColumn();
                    elemColumns.Name = "Specifications";
                    elemColumns.HeaderText = Info.SpecificationsCategory;
                    elemColumns.ReadOnly = true;
                    elemColumns.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    elemColumns.SortMode = DataGridViewColumnSortMode.NotSortable;
                    datagridview.Columns.Add(elemColumns);
                }
            //}
            datagridview.MultiSelect = false;
        }
    }

}
