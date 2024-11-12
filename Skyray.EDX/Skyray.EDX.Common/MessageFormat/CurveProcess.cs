using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.MessageInfo;
using Skyray.EDXRFLibrary;
using Skyray.Controls;

namespace Skyray.EDX.Common
{
    public class CurveProcess : MessageInterface
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
            Curve statics = tempObj as Curve;
            List<CurveItem> itemList = statics.curveResult;
            for (int j = 0; j < itemList.Count; j++)
            {
                dataGridView.Rows.Add(new string[] { itemList[j].Id.ToString(), itemList[j].Calibration, itemList[j].CalibrationType, itemList[j].ConditionName });
            }
            dataGridView.Refresh();
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
            Curve statics = obj as Curve;
            datagridview.Columns.Clear();
            datagridview.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            //datagridview.AllowUserToAddRows = false;
            DataGridViewTextBoxColumn elemColumns = new DataGridViewTextBoxColumn();
            elemColumns.Name = "Id";
            elemColumns.HeaderText = "ID";
            elemColumns.ReadOnly = true;
            elemColumns.SortMode = DataGridViewColumnSortMode.NotSortable;
            elemColumns.Width = 80;
            elemColumns.Visible = false;
            datagridview.Columns.Add(elemColumns);

            elemColumns = new DataGridViewTextBoxColumn();
            elemColumns.Name = "Calibration";
            elemColumns.HeaderText = Info.Calibration;
            elemColumns.ReadOnly = true;
            elemColumns.SortMode = DataGridViewColumnSortMode.NotSortable;
            elemColumns.Width = 80;
            datagridview.Columns.Add(elemColumns);

            elemColumns = new DataGridViewTextBoxColumn();
            elemColumns.Name = "CalibrationType";
            elemColumns.HeaderText = Info.CurveType;
            elemColumns.ReadOnly = true;
            elemColumns.SortMode = DataGridViewColumnSortMode.NotSortable;
            elemColumns.Width = 80;
            datagridview.Columns.Add(elemColumns);

            elemColumns = new DataGridViewTextBoxColumn();
            elemColumns.Name = "CalibrationCondition";
            elemColumns.HeaderText = Info.ConditionName;
            elemColumns.ReadOnly = true;
            elemColumns.SortMode = DataGridViewColumnSortMode.NotSortable;
            elemColumns.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //elemColumns.Width = 80;
            datagridview.Columns.Add(elemColumns);
            datagridview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            datagridview.MultiSelect = false;
            datagridview.AllowUserToAddRows = false;
            datagridview.AllowUserToDeleteRows = false;
        }
    }
}
