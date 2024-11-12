using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.Controls;
using System.Reflection;
using Skyray.EDXRFLibrary;
using System.Data;
using Lephone.Data.Definition;
using Lephone.Data.Common;
using Lephone.Util;
using System.Windows.Forms;

namespace Skyray.EDX.Common
{
    public delegate void UpdateStatesInformation(DataGridViewW datagridView,ElementList list,int currentTimes);

    public abstract class MessageInterface
    {
        public static List<Auto> AutoDic = new List<Auto>();

        public event UpdateStatesInformation onUpdateStatesInformation;
        /// <summary>
        ///  容器填充按照感兴趣元素动态
        /// </summary>
        /// <param name="orientType"></param>
        /// <param name="isFixed"></param>
        /// <param name="tempobj"></param>
        /// <param name="dataGridView"></param>
        public abstract void RecordElementValusInfo(bool flag, bool orientType, bool isFixed, BaseMessage tempobj, DataGridViewW dataGridView,ElementList list);

        /// <summary>
        /// 构造容器
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isFixed"></param>
        /// <param name="orientType"></param>
        /// <param name="datagridview"></param>
        public abstract void ContructDataContainer(BaseMessage obj, bool isFixed, bool orientType, DataGridViewW datagridview);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataGrid"></param>
        public virtual void SendStatisticsInfo(DataGridViewW dataGrid,ElementList list,int currentTimes)
        {
            if (onUpdateStatesInformation != null)
                onUpdateStatesInformation(dataGrid, list,currentTimes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orientType"></param>
        /// <param name="isFixed"></param>
        /// <param name="sendContext"></param>
        /// <param name="dataGridView"></param>
        public void StaticCommonInfo(bool orientType, bool isFixed, BaseMessage sendContext, DataGridViewW dataGridView)
        {
            int i = 0;
            Type type = sendContext.GetType();
            string SValue = string.Empty;
            foreach (PropertyInfo propInfo in type.GetProperties())
            {
                object[] objAttrs = propInfo.GetCustomAttributes(typeof(Auto), true);
                if (objAttrs.Length == 0) continue;//未标记Auto属性则不显示

                SValue = (propInfo.GetValue(sendContext, null) == null ? "" : propInfo.GetValue(sendContext, null).ToString());
                Auto attr = objAttrs[0] as Auto;
                if (orientType)
                {
                    if (isFixed)
                        dataGridView[0, 0].Value = attr.Text;

                    else
                    {
                        int currentRowCount = dataGridView.Rows.Count;
                        dataGridView[0, currentRowCount - 1].Value = SValue;
                    }
                }
                else if (isFixed)
                    dataGridView["valueColumn", i].Value = SValue;
                i++;
            }
        }

        /// <summary>
        /// 容器横着排列构造
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dataGridView"></param>
        public void DataGridViewHorizontal(Type type, DataGridViewW dataGridView)
        {
            foreach (PropertyInfo propInfo in type.GetProperties())
            {
                object[] objAttrs = propInfo.GetCustomAttributes(typeof(Auto), true);
                if (objAttrs.Length > 0)
                {
                    Auto attr = objAttrs[0] as Auto;

                    if (attr != null)
                    {
                        AddToDic(attr);
                        //横着排
                        DataGridViewTextBoxColumn elemColumns = new DataGridViewTextBoxColumn();
                        elemColumns.Name = attr.Text;
                        elemColumns.HeaderText = attr.Text;
                        elemColumns.ReadOnly = true;
                        //elemColumns.SortMode = DataGridViewColumnSortMode.NotSortable;
                        elemColumns.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        elemColumns.Tag = attr.Key;//记录关键字
                        dataGridView.Columns.AddRange(elemColumns);
                    }
                }
            }
            //dataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.MultiSelect = false;
            //dataGridView.Columns[dataGridView.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dataGridView.Enabled = false;
        }

        /// <summary>
        /// 增加到字典
        /// </summary>
        /// <param name="attr"></param>
        private void AddToDic(Auto attr)
        {
            if (!string.IsNullOrEmpty(attr.Key))
            {
                var A = AutoDic.FirstOrDefault(a => a.Key == attr.Key);
                if (A == null) AutoDic.Add(attr);
            }
        }
        /// <summary>
        /// 容器垂直排列
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dataGridView"></param>
        public void DataGridViewVertical(Type type, DataGridViewW dataGridView)
        {
            List<Auto> newList = new List<Auto>();
            foreach (PropertyInfo propInfo in type.GetProperties())
            {
                object[] objAttrs = propInfo.GetCustomAttributes(typeof(Auto), true);
                if (objAttrs.Length > 0)
                {
                    var auto = objAttrs[0] as Auto;
                    AddToDic(auto);
                    newList.Add(auto);
                }
            }
            DataGridViewTextBoxColumn dataColumn = new DataGridViewTextBoxColumn();
            dataColumn.Name = "nameColumn";
            dataColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            //dataColumn.Width = 80;
            dataGridView.Columns.Add(dataColumn);
            dataColumn = new DataGridViewTextBoxColumn();
            dataColumn.Name = "valueColumn";
            dataColumn.Width = 80;
            dataColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView.Columns.Add(dataColumn);
            dataGridView.ColumnHeadersVisible = false;
            dataGridView.RowCount = newList.Count + 1;
            int i = 0;
            foreach (var attr in newList)
            {
                dataGridView["nameColumn", i].Value = attr.Text;
                dataGridView.Rows[i].Tag = attr.Key;
                i++;
            }
            dataGridView.AllowUserToAddRows = false;
            //dataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridView.MultiSelect = false;
            //dataGridView.Columns[dataGridView.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sendContext"></param>
        /// <param name="orientType"></param>
        /// <param name="dataGridView"></param>
        /// <param name="isFixed"></param>
        public void ContructDataCommonInfo(object sendContext, bool orientType, DataGridViewW dataGridView, bool isFixed)
        {
            if (sendContext == null || dataGridView == null)
                return;
            dataGridView.Columns.Clear();
            Type type = sendContext.GetType();
            if (orientType)
                DataGridViewHorizontal(type, dataGridView);
            else
                DataGridViewVertical(type, dataGridView);
        }
    }
}
