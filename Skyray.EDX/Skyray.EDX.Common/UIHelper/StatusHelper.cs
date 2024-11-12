using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.Controls;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Skyray.Controls.NavigationBar;

namespace Skyray.EDX.Common
{
    /// <summary>
    /// 参数Item
    /// </summary>
    public class ParamItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public ParamItem(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
    /// <summary>
    /// 参数组
    /// </summary>
    public class ParamGroup
    {
        public string Name { get; set; }
        public ParamItem[] ParamItems { get; set; }

        public ParamGroup(string name, params ParamItem[] paramItems)
        {
            this.Name = name;
            this.ParamItems = paramItems;
        }
    }

    /// <summary>
    /// 状态栏Item
    /// </summary>
    public class StatusInfo
    {
        public string Id { get; set; }
        public Type Type { get; set; }
        public Image Image { get; set; }
        public string DisplayText { get; set; }
        public bool BEndGroup { get; set; }
        public StatusInfo(string id, string text, bool endGroup)
        {
            this.Id = id;
            this.DisplayText = text;
            this.BEndGroup = endGroup;
            this.Type = typeof(ToolStripLabel);
        }
    }

    /// <summary>
    /// 导航栏Item
    /// </summary>
    public class NaviInfo
    {
        public string Text { get; set; }
        public Image SmallImage { get; set; }
        public Image BigImage { get; set; }
        public Control[] Controls { get; set; }
        public NaviInfo(string text, Image smallImage, Image bigImage, params Control[] controls)
        {
            this.Text = text;
            this.SmallImage = smallImage;
            this.BigImage = bigImage;
            this.Controls = controls;
        }
    }

    public static class BindHelper
    {
        public static void ToStatus(this StatusStripW statusBar, params StatusInfo[] infos)
        {
            if (infos == null || infos.Length == 0 || statusBar == null) return;

            statusBar.Items.Clear();
            foreach (var info in infos)
            {
                if (info.Type == typeof(ToolStripLabel))
                {
                    ToolStripLabel labelId = new ToolStripLabel(info.Id);
                    labelId.Margin = new Padding(0, 3, 0, 3);//边距
                    if (info.Image != null) labelId.Image = info.Image;

                    ToolStripLabel labelValue = new ToolStripLabel(info.DisplayText);
                    labelValue.Margin = new Padding(3, 2, 6, 2);

                    statusBar.Items.Add(labelId);
                    statusBar.Items.Add(labelValue);

                    if (info.BEndGroup)
                    {
                        ToolStripSeparator sep = new ToolStripSeparator();//分割线
                        sep.Margin = new Padding(0, 3, 0, 3);//边距
                        statusBar.Items.Add(sep);//添加分割线
                    }

                }
            }
        }

        public static void ToNaviBar(this NaviBar bar, int ActiveBandIndex, params NaviInfo[] infos)
        {
            if (infos == null || infos.Length == 0 || bar == null) return;
            NaviBand band = null;
            foreach (var info in infos)
            {
                band = new NaviBand();
                band.Text = info.Text;
                band.SmallImage = info.SmallImage;
                band.LargeImage = info.BigImage;
                if (info.Controls != null && info.Controls.Length != 0)
                {
                    foreach (Control ctrl in info.Controls)
                    {
                        band.ClientArea.Controls.Add(ctrl);
                    }
                }
                bar.Bands.Add(band);
            }

            if (bar.Bands[ActiveBandIndex] != null) bar.ActiveBand = bar.Bands[ActiveBandIndex];

        }

        public static Binding BindToCtrl(this Control ctrl, string ctrlProperty, object objDataSource, string fieldName, bool changeAtOnce)
        {
            Binding oldBinding = null;
            foreach (Binding bind in ctrl.DataBindings)
            {
                if (bind.PropertyName == ctrlProperty) oldBinding = bind;//查找以前的绑定
            }
            if (oldBinding != null) ctrl.DataBindings.Remove(oldBinding);//移除之前的绑定

            var flag = changeAtOnce ? DataSourceUpdateMode.OnPropertyChanged : DataSourceUpdateMode.Never;
            Binding binding = new Binding(ctrlProperty, objDataSource, fieldName, true, flag);
            ctrl.DataBindings.Add(binding);
            return binding;
        }

        public static Binding BindSelectedIndexToCtrl(this Control ctrl, object objDataSource, string fieldName, bool changeAtOnce)
        {
            return BindToCtrl(ctrl, "SelectedIndex", objDataSource, fieldName, changeAtOnce);
        }

        public static Binding BindSelectedValueToCtrl(this Control ctrl, object objDataSource, string fieldName, bool changeAtOnce)
        {
            return BindToCtrl(ctrl, "SelectedValue", objDataSource, fieldName, changeAtOnce);
        }

        public static Binding BindValueToCtrl(this Control ctrl, object objDataSource, string fieldName, bool changeAtOnce)
        {
            return BindToCtrl(ctrl, "Value", objDataSource, fieldName, changeAtOnce);
        }
        public static Binding BindTextToCtrl(this  Control ctrl, object objDataSource, string fieldName, bool changeAtOnce)
        {
            return BindToCtrl(ctrl, "Text", objDataSource, fieldName, changeAtOnce);
        }

        public static Binding BindCheckedToCtrl(this Control ctrl, object objDataSource, string fieldName, bool changeAtOnce)
        {
            return BindToCtrl(ctrl, "Checked", objDataSource, fieldName, changeAtOnce);
        }

        public static Binding BindEnabledToCtrl(this Control ctrl, object objDataSource, string fieldName, bool changeAtOnce)
        {
            return BindToCtrl(ctrl, "Enabled", objDataSource, fieldName, changeAtOnce);
        }


        public static Binding BindValueToCtrl(this Control ctrl, object objDataSource, string fieldName)
        {
            return BindToCtrl(ctrl, "Value", objDataSource, fieldName, true);
        }
        public static Binding BindTextToCtrl(this  Control ctrl, object objDataSource, string fieldName)
        {
            return BindToCtrl(ctrl, "Text", objDataSource, fieldName, true);
        }

        public static Binding BindCheckedToCtrl(this Control ctrl, object objDataSource, string fieldName)
        {
            return BindToCtrl(ctrl, "Checked", objDataSource, fieldName, true);
        }

        public static Binding BindEnabledToCtrl(this Control ctrl, object objDataSource, string fieldName)
        {
            return BindToCtrl(ctrl, "Enabled", objDataSource, fieldName, true);
        }

        public static Binding BindVisibleToCtrl(this Control ctrl, object objDataSource, string fieldName)
        {
            return BindToCtrl(ctrl, "Visible", objDataSource, fieldName, true);
        }

        //public static Control ttt(this Control ctrl, string sss)
        //{
        //    return ctrl;
        //}
        //public static Control ttt2(this Control ctrl, string sss)
        //{
        //    return ctrl;
        //}
        //public static Control ttt3(this Control ctrl, string sss)
        //{
        //    return ctrl;
        //}

        //public static void BindToTabCtrlW(TabControlW tabCtrl, params ParamGroup[] paramGroups)
        //{
        //    TabPage tabPage;//页面TabPage
        //    tabCtrl.TabPages.Clear();//先清空页面
        //    foreach (var group in paramGroups)
        //    {
        //        tabPage = new TabPage(group.Name);//新增一页
        //        TransparentGrid grid = new TransparentGrid();//构造背景透明的表格
        //        BindToTransGrid(grid, group.ParamItems);// 绑定参数至表格
        //        grid.Dock = DockStyle.Fill;//填充
        //        tabPage.Controls.Add(grid);//添加控件
        //    }
        //}

        ///// <summary>
        ///// 绑定参数至表格
        ///// </summary>
        ///// <param name="grid"></param>
        ///// <param name="items"></param>
        //public static void BindToTransGrid(TransparentGrid grid, params ParamItem[] items)
        //{
        //    grid.ColumnsCount = 2;
        //    int intRowCount = items.Length;
        //    for (int i = 0; i < intRowCount; i++)
        //    {
        //        GridHelper.SetStringCell(items[i].Key, grid[i, 0], grid.CellViewKey);
        //        GridHelper.SetStringCell(items[i].Value, grid[i, 1], grid.CellViewValue);
        //    }
        //    grid.AutoSizeCells();
        //    grid.Columns.StretchToFit();
        //    grid.Rows.StretchToFit();
        //}
    }
}
