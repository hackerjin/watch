using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.Controls;
using System.Drawing;
using System.Reflection;
using Lephone.Util;
using Skyray.EDXRFLibrary;
using System.Data;

namespace Skyray.EDX.Common
{
    public class CtrlFactory
    {
        public static LabelPosition LabelPos = LabelPosition.Top;//标签所在位置
        public static int ColCount = 2;//列总数

        public static Padding Pad = new Padding(5, 20, 5, 0); //边距
        public static int RowHeight = 50;//行高

        public static int LabelWidth = 80;//标签宽度
        public static int CtrlWidth = 120;//控件宽度
        public static int LabelCtrlOffSet = 10;//横向排列时，标签与控件之间的距离

        public static int CellMarginX = 10;//单元格最小X间距       
        public static int CellMarginY = 10;//单元格最小Y间距    

        public static bool AutoWidth = true;//自动调整父容器宽度
        public static bool AutoHeight = true;//自动调整父容器高度
        public static int LabelOffSetY = 5;//标签Y轴偏移量

        public static string strLabelPrefix = "AutoLabel";//自动生成的标签名称前缀
        public static string strControlPrefix = "AutoCtrl";//自动生成的控件名称前缀
        public static void FillCtrlToContainer(Control ctrlContainer, int colCount, LabelPosition pos, List<Control> ctrls, bool CtrlAddToContainer)
        {
            ColCount = colCount;
            LabelPos = pos;
            //if (LabelPos == LabelPosition.Top) 
            //{
            //    RowHeight = 15 + 21 + 10;
            //}
            //else
            //{
            //    RowHeight = 21 + 10 + 16; //+16
            //}

            bool b = ctrls.Count / 2 % ColCount == 0;//是否整除
            int rowCount = b ? ctrls.Count / 2 / ColCount : ctrls.Count / 2 / ColCount + 1;//获取表格行总数


                if (LabelPos == LabelPosition.Top) 
            {
                RowHeight = 15 + 21 + 10;
            }
            else
            {
                RowHeight = (int)(ctrlContainer.Height / rowCount) - 5;
            }

            int MinWidth = 0;
            int MinHeight = Pad.Top + Pad.Bottom + RowHeight * rowCount;//获取最小容器高;
            if (LabelPos == LabelPosition.Top)
            {
                MinWidth = Pad.Left + Pad.Right + CtrlWidth * ColCount + CellMarginX;//获取最小容器宽                
            }
            else if (LabelPos == LabelPosition.Left)
            {
                MinWidth = Pad.Left + Pad.Right
                    + (LabelWidth + CtrlWidth + LabelCtrlOffSet) * ColCount
                    + CellMarginX * (ColCount - 1);//获取最小容器宽      
            }

            if (ctrlContainer.Width < MinWidth && AutoWidth) ctrlContainer.Width = MinWidth;//小于最小宽，调整容器宽度至最小宽
            if (ctrlContainer.Height < MinHeight && AutoHeight) ctrlContainer.Height = MinHeight;//小于最小高，调整容器高度至最小高


            int y = Pad.Top;//首行控件的纵轴坐标
            Point[] pnts = new Point[ColCount];//首行每个单元格的坐标点
            int Width = ctrlContainer.Size.Width - Pad.Left - Pad.Right;//容器宽
            int cellWith = Width / ColCount;//单元格宽
            int TotalWidths = 0;

            if (LabelPos == LabelPosition.Top)
            {
                TotalWidths = LabelWidth > CtrlWidth ? LabelWidth : CtrlWidth;
            }
            else if (LabelPos == LabelPosition.Left)
            {
                TotalWidths = LabelWidth + CtrlWidth + LabelCtrlOffSet + CellMarginX;
            }

            for (int i = 0; i < ColCount; i++)
            {

                if (LabelPos == LabelPosition.Left)
                {
                    pnts[i] = new Point(20 + Pad.Left + i * Width / ColCount, y);
                }
                else
                {
                    pnts[i] = new Point((cellWith - TotalWidths) / 2 + Pad.Left + i * Width / ColCount, y);
                }
            }

            for (int i = 0; i < ctrls.Count / 2; i++)
            {
                int j = i % ColCount;//获取控件所在列数
                int k = (int)i / ColCount;//获取控件所在行数

                //获取标签控件所在坐标点,控件居中对齐
                Point p = new Point(pnts[j].X, pnts[j].Y + RowHeight * k);

                if (LabelPos == LabelPosition.Top)
                {
                    ctrls[i * 2].Location = p;//标签控件位置
                    ctrls[i * 2 + 1].Location = new Point(p.X, p.Y + ctrls[i * 2].Height - 5);//编辑控件
                }
                else if (LabelPos == LabelPosition.Left)
                {
                    ctrls[i * 2].Location = new Point(p.X, p.Y + 5);//标签控件位置                    

                    //ctrls[i * 2 + 1].Location = new Point(p.X + LabelWidth, p.Y);//编辑控件
                    ctrls[i * 2 + 1].Location = new Point(cellWith + p.X - CtrlWidth - 20 - 60, p.Y);//编辑控件
                }
                if (CtrlAddToContainer)
                {
                    ctrlContainer.Controls.Add(ctrls[i * 2]);//向容器内添加标签控件
                    ctrlContainer.Controls.Add(ctrls[i * 2 + 1]);//向容器内添加编辑控件
                }
            }
        }

        public static Control CreateCtrlByType(Type typ)
        {
            Control ctrl = null;
            if (typ == typeof(string) || typ == typeof(float))
            {
                ctrl = new TextBoxW();
            }
            else if (typ == typeof(int))
            {
                ctrl = new NumricUpDownW();
            }
            else if (typ == typeof(double))
            {
                ctrl = new NumricUpDownW();
            }
            else if (typ == typeof(DateTime))
            {
                ctrl = new DateTimePicker();
            }
            else if (typ == typeof(bool))
            {
                ctrl = new CheckBoxW();
            }
            else
            {
                if (typ.BaseType == typeof(System.Enum))
                {
                    ctrl = new ComboBoxW();
                    ComboBoxW comboBox = ctrl as ComboBoxW;
                    if (typ == typeof(SpecLength))
                    {

                        foreach (var str in Enum.GetNames(typ))
                        {
                            int length = (int)Enum.Parse(typ, str);//str;
                            comboBox.Items.Add(length);
                        }
                    }
                    else
                    {
                        foreach (var str in Enum.GetNames(typ))
                        {
                            comboBox.Items.Add(str);
                        }
                    }
                    comboBox.SelectedIndex = 0;
                }
                else
                {
                    ctrl = Activator.CreateInstance(typ) as Control;//反射创建实例
                }
            }

            return ctrl;
        }

        /// <summary>
        /// 获取实体类型对应的编辑控件集合
        /// </summary>
        /// <param name="typ">实体类型</param>
        /// <param name="bExclude">False：包含字段 True：排除字段,当Fields参数为null时，本参数设置无效</param>
        /// <param name="Fields">字段名称集合</param>
        /// <returns></returns>
        public static List<Control> GetEditCtrls(Type typ, bool bExclude, params string[] Fields)
        {
            List<Control> lstCtrl = new List<Control>();
            Control ctrl = null;

            foreach (PropertyInfo info in typ.GetProperties())
            {
                if (Fields != null && Fields.Length > 0)
                {
                    var b = Fields.Contains(info.Name);//检查是否包含字段
                    if (b && bExclude) continue;//包含排除字段
                    if (!b && !bExclude) continue;//非包含字段
                }

                foreach (Auto auto in info.GetCustomAttributes(typeof(Auto), true))
                {
                    ctrl = CreateCtrlByType(info.PropertyType);
                    if (ctrl == null) continue;
                    bool bCheck = TargetDic.TargetDictionary.ContainsKey(info.Name);
                    if (bCheck)
                    {
                        var check = TargetDic.TargetDictionary[info.Name];
                        if (!check)
                            continue;
                    }

                    Label label = new Label();
                    label.AutoSize = true;
                    label.Text = auto.Text;//标签显示文本


                    label.Name = strLabelPrefix + info.Name;//标签名称

                    lstCtrl.Add(label);

                    ctrl.Name = strControlPrefix + info.Name;//控件名称

                    if (typeof(NumricUpDownW) == ctrl.GetType())
                    {
                        var numUpDown = (NumricUpDownW)ctrl;
                        bool bExistKey = Ranges.RangeDictionary.ContainsKey(info.Name);
                        if (bExistKey)
                        {
                            var rangeInfo = Ranges.RangeDictionary[info.Name];
                            numUpDown.Maximum = rangeInfo.Max;
                            numUpDown.Minimum = rangeInfo.Min;
                            numUpDown.DecimalPlaces = rangeInfo.DecimalPlaces;
                            numUpDown.Increment = rangeInfo.Increment;
                        }
                    }
                    ctrl.Width = CtrlWidth;
                    

                    lstCtrl.Add(ctrl);
                }
            }
            return lstCtrl;//返回控件集合
        }

        /// <summary>
        /// 获取类型全部字段对应的编辑控件集合
        /// </summary>
        /// <param name="typ"></param>
        /// <returns></returns>
        public static List<Control> GetEditCtrls(Type typ)
        {
            return GetEditCtrls(typ, true, null);
        }

        /// <summary>
        /// 获取标签控件
        /// </summary>
        /// <param name="lstCtrl"></param>
        /// <returns></returns>
        public static IEnumerable<Control> GetLabelCtrlList(List<Control> lstCtrl)
        {
            for (int i = 0; i < lstCtrl.Count; i++)
            {
                if (i % 2 == 0) yield return lstCtrl[i];
            }
        }

        /// <summary>
        /// 获取编辑控件
        /// </summary>
        /// <param name="lstCtrl"></param>
        /// <returns></returns>
        public static IEnumerable<Control> GetEditCtrlList(List<Control> lstCtrl)
        {
            for (int i = 0; i < lstCtrl.Count; i++)
            {
                if (i % 2 != 0) yield return lstCtrl[i];
            }
        }

        /// <summary>
        /// 值绑定至控件
        /// </summary>
        /// <param name="lstEditCtrl">绑定的目标控件</param>
        /// <param name="obj">绑定的对象数据源</param>
        /// <param name="ChangeAtOnce">是否控件值改变，数据源立即改变</param>
        public static List<Binding> BindValue(IEnumerable<Control> lstEditCtrl, object obj, bool ChangeAtOnce)
        {
            Type typ;
            List<Binding> Binds = new List<Binding>();
            Binding bind = null;
            foreach (var ctrl in lstEditCtrl)
            {
                typ = ctrl.GetType();
                string strFieldName = ctrl.Name.Replace(strControlPrefix, "");
                if (typ == typeof(TextBoxW))
                {
                    bind = BindHelper.BindTextToCtrl(ctrl, obj, strFieldName, ChangeAtOnce);
                }
                else if (typ == typeof(NumricUpDownW))
                {
                    bind = BindHelper.BindValueToCtrl(ctrl, obj, strFieldName, ChangeAtOnce);
                }
                else if (typ == typeof(CheckBoxW))
                {
                    bind = BindHelper.BindCheckedToCtrl(ctrl, obj, strFieldName, ChangeAtOnce);
                }
                else if (typ == typeof(ComboBoxW))
                {
                    bind = BindHelper.BindTextToCtrl(ctrl, obj, strFieldName, ChangeAtOnce);
                }
                Binds.Add(bind);
            }
            Binds.TrimExcess();
            return Binds;

        }

        public static void WriteValue(IEnumerable<Control> lstEditCtrl)
        {
            foreach (Control ctrl in lstEditCtrl)
            {
                foreach (Binding bind in ctrl.DataBindings)
                {
                    bind.WriteValue();
                }
            }
        }

        public static MTSplitter GetSpliter(SpliterType spliterType)
        {
            MTSplitter spliter = new MTSplitter();
            switch (spliterType)
            {
                case SpliterType.HTop:
                    spliter.Cursor = Cursors.SizeNS;
                    spliter.Dock = System.Windows.Forms.DockStyle.Top;
                    break;
                case SpliterType.HBottom:
                    spliter.Cursor = Cursors.SizeNS;
                    spliter.Dock = System.Windows.Forms.DockStyle.Bottom;
                    break;
                case SpliterType.VLeft:
                    spliter.Cursor = Cursors.SizeWE;
                    spliter.Dock = System.Windows.Forms.DockStyle.Left;
                    break;
                case SpliterType.VRight:
                    spliter.Cursor = Cursors.SizeNS;
                    spliter.Dock = System.Windows.Forms.DockStyle.Right;
                    break;
            }
            return spliter;
        }

        public static Grouper GetGrouper()
        {
            Grouper gp = new Grouper();
            //gp.GroupTitle = strTitle;
            gp.GroupBoxAlign = Grouper.GroupBoxAlignMode.Left;
            return gp;
        }

        public static ButtonW GetButtonW()
        {
            return new ButtonW();
        }
    }
    public class EnumInfo
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
