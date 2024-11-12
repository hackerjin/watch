/* ***
 * 作者：
 * 修改日期：2009-03-03
 * 类型1：结构ElemTableItem
 * 作用1：抽象一个元素周期表的项
 * 类型2：类ElemTableGraphic
 * 作用2：抽象一个元素周期表
 * ***/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;

namespace Skyray.Controls.ElementTable
{
    public class ElementTable : Control
    {
        public const int LineCount = 6;

        #region 字段

        /** 单元格的高度 **/
        private int gridHeight;

        /** 单元个的宽度 **/
        private int gridWidth;

        /** 网格之间宽度 **/
        private int gridSpace;

        /** 单元框的颜色 **/
        private Color gridColor;

        /** 选中的颜色 **/
        private Color selectColor;

        /** 未选中的颜色 **/
        private Color unselectColor;

        /** 存放元素新型的数值 **/
        private ElemTableItem[] items;

        /** 是否可以多选 **/
        private bool multiSelect = true;


        /**某一元素只能选择单一线系**/
        private bool signelLine;

        Brush backBrush;

        Brush textBrush;

        Brush backSelectedBrush;

        Brush textSelectedBrush;

        Pen pen;

        private int _MaxLayElement;

        public int MaxLayElement
        {
            get { return _MaxLayElement; }
            set { _MaxLayElement = value; }
        }

        #endregion

        #region 属性

        public bool ShowKL { get; set; }

        /// <summary>
        /// 单元格高度
        /// </summary>
        public int GridHeight
        {
            get
            {
                return gridHeight;
            }
            set
            {
                if (value > 0)
                {
                    gridHeight = value;
                    Height = gridHeight * 7 + gridSpace;
                    // Invalidate();
                }
            }
        }

        /// <summary>
        /// 单元格宽度
        /// </summary>
        public int GridWidth
        {
            get
            {
                return gridWidth;
            }
            set
            {
                if (value > 0)
                {
                    gridWidth = value;
                    Width = 18 * gridWidth + gridSpace;
                    //Invalidate();
                }
            }
        }

        /// <summary>
        /// 格线颜色
        /// </summary>
        public Color GridColor
        {
            get
            {
                return gridColor;
            }
            set
            {
                if (value != gridColor)
                {
                    gridColor = value;
                    //Invalidate();
                }
            }
        }

        /// <summary>
        /// 选中项的颜色
        /// </summary>
        public Color SelectColor
        {
            get
            {
                return selectColor;
            }
            set
            {
                if (value != selectColor)
                {
                    selectColor = value;
                    //Invalidate();
                }
            }
        }

        /// <summary>
        /// 未选中项的颜色
        /// </summary>
        public Color UnselectColor
        {
            get { return unselectColor; }
            set
            {
                if (value != unselectColor)
                {
                    unselectColor = value;
                    // Invalidate();
                }
            }
        }

        public bool MultiSelect
        {
            get { return multiSelect; }
            set
            {
                if (multiSelect != value)
                {
                    multiSelect = value;
                }
            }
        }

        public bool SignelLine
        {
            get { return signelLine; }
            set { signelLine = value; }
        }

        #endregion

        #region 构造器

        /// <summary>
        /// 实例构造函数
        /// </summary>
        public ElementTable()
        {
            items = new ElemTableItem[96];
            InitalizeItem();
            gridHeight = 50;
            gridWidth = 44;
            gridSpace = 4;
            multiSelect = true;
            _MaxLayElement = 35;
            unselectColor = Color.FromArgb(175, 210, 255);
            selectColor = Color.Orange;
            BackColor = Color.White;
            gridColor = Color.FromArgb(200, 101, 147, 207);

            backBrush = new SolidBrush(unselectColor);
            textBrush = new SolidBrush(ForeColor);
            backSelectedBrush = new SolidBrush(selectColor);
            textSelectedBrush = new SolidBrush(Color.DarkBlue);
            pen = new Pen(gridColor);

            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ElemTableGraphic_MouseDown);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ElemTableGraphic_Paint);
        }

        private List<ElemTableItem> GetSelItem()
        {
            List<ElemTableItem> lst = new List<ElemTableItem>();
            foreach (var item in items)
            {
                //if (item.SelectedLines.Any(x => x) || item.Selected) lst.Add(item);
                if (item.Selected) lst.Add(item);
            }
            return lst;
        }

        public void GetSelectedItem(out String[] names, out Int32[] lines)
        {
            List<string> lstNames = new List<string>();
            List<int> lstLines = new List<int>();
            foreach (var item in items)
            {
                for (int i = 0; i < LineCount; i++)
                {
                    if (item.SelectedLines[i] && item.Selected)
                    {
                        lstNames.Add(item.Name);
                        lstLines.Add(i);
                    }
                }
            }
            names = lstNames.ToArray();
            lines = lstLines.ToArray();
        }
        private void ClearSelection()
        {
            //if (LastClickItem != null)
            //{
            //    ResetItem(LastClickItem);
            //    LastClickItem = null;
            //}         

            foreach (var item in GetSelItem())
            {
                item.Selected = false;
                item.SelectedLines = new bool[LineCount];
                Invalidate(GridRect(item.Column, item.Row));
            }
        }

        public void SetSelectedItem(String[] names, Int32[] lines)
        {
            ClearSelection();
            if (names != null && lines != null && names.Length > 0)
            {
                bool isSingleSelect = !multiSelect;
                if (isSingleSelect)
                {
                    LastClickItem = items.FirstOrDefault(x => x.Name == names[0]);
                }

                int count = isSingleSelect ? 1 : names.Length;

                for (int j = 0; j < count; j++)
                {
                    foreach (var item in items)
                    {
                        if (names[j] == item.Name)
                        {
                            item.Selected = item.SelectedLines[lines[j]] = true;
                            Invalidate(GridRect(item.Column, item.Row));
                            break;
                        }
                    }
                }
            }

        }

        #endregion

        /// <summary>
        /// 初始化元素的信息
        /// </summary>
        private void InitalizeItem()
        {
            int col, row, id;
            for (int i = 0; i < 96; i++)
            {
                items[i] = new ElemTableItem();
            }
            for (int i = 0; i < 92; i++)
            {
                id = 1 + i;
                items[i].ID = id.ToString();
                SetItemLocation(id, out col, out row);
                items[i].Column = col;
                items[i].Row = row;
                items[i].Selected = false;
            }
            items[92].ID = "*"; items[92].Name = "*";
            items[92].Row = 6; items[92].Column = 3;
            items[93].ID = "**"; items[93].Name = "**";
            items[93].Row = 7; items[93].Column = 3;
            items[94].ID = "*"; items[94].Name = "Lanthan";
            items[94].Row = 8; items[94].Column = 1;
            items[95].ID = "**"; items[95].Name = "Actinid";
            items[95].Row = 9; items[95].Column = 1;

            items[0].Name = "H"; items[1].Name = "He"; items[2].Name = "Li";
            items[3].Name = "Be"; items[4].Name = "B"; items[5].Name = "C";
            items[6].Name = "N"; items[7].Name = "O"; items[8].Name = "F";
            items[9].Name = "Ne";




            items[10].Name = "Na"; items[11].Name = "Mg"; items[12].Name = "Al";
            items[13].Name = "Si"; items[14].Name = "P"; items[15].Name = "S";
            items[16].Name = "Cl"; items[17].Name = "Ar"; items[18].Name = "K";
            items[19].Name = "Ca"; items[20].Name = "Sc"; items[21].Name = "Ti";
            items[22].Name = "V"; items[23].Name = "Cr"; items[24].Name = "Mn";
            items[25].Name = "Fe"; items[26].Name = "Co"; items[27].Name = "Ni";
            items[28].Name = "Cu"; items[29].Name = "Zn"; items[30].Name = "Ga";
            items[31].Name = "Ge"; items[32].Name = "As"; items[33].Name = "Se";
            items[34].Name = "Br"; items[35].Name = "Kr"; items[36].Name = "Rb";
            items[37].Name = "Sr"; items[38].Name = "Y"; items[39].Name = "Zr";
            items[40].Name = "Nb"; items[41].Name = "Mo"; items[42].Name = "Tc";
            items[43].Name = "Ru"; items[44].Name = "Rh"; items[45].Name = "Pd";
            items[46].Name = "Ag"; items[47].Name = "Cd"; items[48].Name = "In";
            items[49].Name = "Sn"; items[50].Name = "Sb"; items[51].Name = "Te";
            items[52].Name = "I"; items[53].Name = "Xe"; items[54].Name = "Cs";
            items[55].Name = "Ba"; items[56].Name = "La"; items[57].Name = "Ce";
            items[58].Name = "Pr"; items[59].Name = "Nd"; items[60].Name = "Pm";
            items[61].Name = "Sm"; items[62].Name = "Eu"; items[63].Name = "Gd";
            items[64].Name = "Tb"; items[65].Name = "Dy"; items[66].Name = "Ho";
            items[67].Name = "Er"; items[68].Name = "Tm"; items[69].Name = "Yb";
            items[70].Name = "Lu"; items[71].Name = "Hf"; items[72].Name = "Ta";
            items[73].Name = "W"; items[74].Name = "Re"; items[75].Name = "Os";
            items[76].Name = "Ir"; items[77].Name = "Pt"; items[78].Name = "Au";
            items[79].Name = "Hg"; items[80].Name = "Tl"; items[81].Name = "Pb";
            items[82].Name = "Bi"; items[83].Name = "Po"; items[84].Name = "At";
            items[85].Name = "Rn"; items[86].Name = "Fr"; items[87].Name = "Ra";
            items[88].Name = "Ac"; items[89].Name = "Th"; items[90].Name = "Pa";
            items[91].Name = "U";
        }

        ///<summary>
        ///设置元素显示位置
        ///</summary>
        ///<param name="elemID">元素的序号</param>
        private void SetItemLocation(int elemID, out int col, out int row)
        {
            col = 0;
            row = 0;
            if (elemID==1)
            {
                row = 1;
                col = elemID;
            }
            else if (elemID == 2)
            {
                row = 1;
                col = elemID+16;
            }
            else if (elemID < 5)
            {
                row = 2;
                col = elemID -2;
            }
            else if (elemID < 11)
            {
                row = 2;
                col = elemID + 8;
            }
            else if (elemID < 13)
            {
                row = 3;
                col = elemID - 10;
            }
            else if (elemID < 19)
            {
                row = 3;
                col = elemID;
            }

            else if (elemID < 57)
            {
                row = (elemID - 1) / 18 + 3;
                col = elemID - (row - 3) * 18;
            }
            else if (elemID < 72)
            {
                row = 8;
                col = elemID - 53;
            }
            else if (elemID < 87)
            {
                row = 6;
                col = elemID - 68;
            }
            else if (elemID < 89)
            {
                row = 7;
                col = elemID - 86;
            }
            else
            {
                row = 9;
                col = elemID - 85;
            }

            //col = 0;
            //row = 0;
            //if (elemID < 13)
            //{
            //    row = 1;
            //    col = elemID - 10;
            //}
            //else if (elemID < 57)
            //{
            //    row = (elemID - 1) / 18 + 1;
            //    col = elemID - (row - 1) * 18;
            //}
            //else if (elemID < 72)
            //{
            //    row = 6;
            //    col = elemID - 53;
            //}
            //else if (elemID < 89)
            //{
            //    row = (elemID - 69) / 18 + 4;
            //    col = elemID - 68 - (row - 4) * 18;
            //}
            //else
            //{
            //    row = 7;
            //    col = elemID - 85;
            //}
        }

        /// <summary>
        /// 求网格单元所在的位置区域
        /// </summary>
        /// <param name="row">网格的行</param>
        /// <param name="col">网格的列</param>
        /// <returns></returns>
        private Rectangle GridRect(int col, int row)
        {
            int left = 0;
            int top = 0;
            int width = 0;
            int height = 0;
            if (row >= 8 && row <= 9 && col >= 1 && col <= 3)
            {
                left = gridSpace;
                width = 3 * gridWidth - gridSpace;
                top = (row - 1) * gridHeight + gridSpace;
                height = gridHeight - gridSpace;
            }
            else
            {
                left = (col - 1) * gridWidth + gridSpace;
                top = (row - 1) * gridHeight + gridSpace;
                width = gridWidth - gridSpace;
                height = gridHeight - gridSpace;
            }
            return new Rectangle(left, top, width, height);
        }

        private Rectangle GridRectK(int col, int row)
        {
            Rectangle rect = GridRect(col, row);
            return new Rectangle(rect.X + gridSpace / 2, rect.Y + rect.Height / 2 + gridSpace / 2, rect.Width / 2 - gridSpace, rect.Height / 2 - gridSpace);
        }

        private Rectangle GridRectL(int col, int row)
        {
            Rectangle rect = GridRect(col, row);
            return new Rectangle(rect.X + gridSpace / 2 + rect.Width / 2, rect.Y + rect.Height / 2 + gridSpace / 2, rect.Width / 2 - gridSpace, rect.Height / 2 - gridSpace);
        }

        /// <summary>
        /// 求包含指定点坐标的元素
        /// </summary>
        /// <param name="point">点</param>
        /// <returns>包含点的元素</returns>
        private bool ItemWithPoint(Point point, ref int AtomId, ref int LineId)
        {
            bool HitItem = false;
            int col = (point.X - gridSpace) / gridWidth + 1;
            int row = (point.Y - gridSpace) / gridHeight + 1;

            if ((col > 0) && (row > 0) && (col < 19) && (row < 10))
            {
                Int32 line = -1;
                int temp = ShowKL ? gridHeight / 2 : gridHeight;
                if ((row * gridHeight - point.Y) < temp)
                {
                    if (ShowKL)
                    {
                        if ((col * gridWidth - point.X) < gridWidth / 2)
                        {
                            line = 2;
                        }
                        else
                        {
                            line = 0;
                        }
                    }
                }

                for (int i = 0; i < 92; i++)//85-87不可选
                {
                    if ((col == items[i].Column) && (row == items[i].Row))
                    {
                        AtomId = i;
                        LineId = line;
                        HitItem = true;
                        break;
                    }
                }
            }
            return HitItem;
        }

        /// <summary>
        /// 画元素
        /// </summary>
        /// <param name="index">元素的索引</param>
        private void DrawItem(Graphics g, int index)
        {
            // 求需要绘制的矩形
            Int32 itemCol = items[index].Column;
            Int32 itemRow = items[index].Row;
            Rectangle rect = GridRect(itemCol, itemRow);
            Rectangle rectK = GridRectK(itemCol, itemRow);
            Rectangle rectL = GridRectL(itemCol, itemRow);

            g.FillRectangle(backBrush, rect);
            //g.DrawRectangle(pen, rect);

            if ((itemRow >= 8 && itemRow <= 9 && itemCol >= 1 && itemCol <= 3) ||
                (itemRow >= 6 && itemRow <= 7 && itemCol == 3))
            {
                Size fontSize = g.MeasureString(items[index].Name, Font).ToSize();
                int x = (rect.Width - fontSize.Width) / 2 + rect.Left;
                int y = (rect.Height / 2 - fontSize.Height) / 2 + rect.Top;

                g.DrawString(items[index].Name, Font, textBrush, x, y);

                fontSize = g.MeasureString(items[index].ID, Font).ToSize();
                x = (rect.Width - fontSize.Width) / 2 + rect.Left;
                y = rect.Top + (rect.Height * 3 - 2 * fontSize.Height) / 4;
                g.DrawString(items[index].ID, Font, textBrush, x, y);
            }
            else
            {
                if (ShowKL)
                {
                    String strToDraw = items[index].ID + " " + items[index].Name;
                    Size fontSize = g.MeasureString(strToDraw, Font).ToSize();
                    int x = (rect.Width - fontSize.Width) / 2 + rect.Left;
                    int y = (rect.Height / 2 - fontSize.Height) / 2 + rect.Top;
                    g.DrawString(strToDraw, Font, textBrush, x, y);

                    //加横线
                    g.DrawLine(pen, rect.Left, (rect.Height / 2) + rect.Top, (rect.Width) + rect.Left, (rect.Height / 2) + rect.Top);

                    fontSize = g.MeasureString("K", Font).ToSize();
                    x = (rectK.Width - fontSize.Width) / 2 + rectK.Left;
                    y = rectK.Top + (rectK.Height - fontSize.Height) / 2;
                    if (items[index].SelectedLines[0])
                    {
                        //g.FillRectangle(backSelectedBrush, rectK);
                        rectK.Offset(-1, -1);
                        rectK.Size = new Size(rectK.Size.Width + 3, rectK.Size.Height + 3);
                        GraphHelper.FillRect(rectK, g);
                        g.DrawString("K", Font, textSelectedBrush, x, y);
                    }
                    else
                    {
                        g.FillRectangle(backBrush, rectK);
                        g.DrawString("K", Font, textBrush, x, y);
                    }

                    //加竖线
                    g.DrawLine(pen, (rect.Width / 2) + rect.Left, (rect.Height / 2) + rect.Top, (rect.Width / 2) + rect.Left, rect.Height + rect.Top);

                    fontSize = g.MeasureString("L", Font).ToSize();
                    x = (rectL.Width - fontSize.Width) / 2 + rectL.Left;
                    y = rectL.Top + (rectL.Height - fontSize.Height) / 2;

                    if (items[index].SelectedLines[2])
                    {
                        //g.FillRectangle(backSelectedBrush, rectL);
                        rectL.Offset(-1, -1);
                        rectL.Size = new Size(rectL.Size.Width + 3, rectL.Size.Height + 3);
                        GraphHelper.FillRect(rectL, g);
                        g.DrawString("L", Font, textSelectedBrush, x, y);
                    }
                    else
                    {
                        g.FillRectangle(backBrush, rectL);
                        g.DrawString("L", Font, textBrush, x, y);
                    }
                }
                else
                {
                    var str1 = items[index].ID;
                    var str2 = items[index].Name;
                    Size fontSize1 = g.MeasureString(str1, Font).ToSize();
                    Size fontSize2 = g.MeasureString(str2, Font).ToSize();

                    int x1 = (rect.Width - fontSize1.Width) / 2 + rect.Left;
                    int x2 = (rect.Width - fontSize2.Width) / 2 + rect.Left;

                    int offset = (rect.Height - fontSize1.Height - fontSize2.Height - 10) / 2;
                    int y1 = offset + rect.Top;
                    int y2 = rect.Bottom - offset - fontSize2.Height;

                    if (items[index].Selected)
                    {
                        //g.FillRectangle(backSelectedBrush, rect);                    
                        GraphHelper.FillRect(rect, g);
                    }

                    g.DrawString(str1, Font, textBrush, x1, y1);
                    g.DrawString(str2, Font, textBrush, x2, y2);
                }
            }
            g.DrawRectangle(pen, rect);
        }


        /// <summary>
        /// 画图事件
        /// </summary>
        private void ElemTableGraphic_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < items.Length; i++)
            {
                DrawItem(e.Graphics, i);
            }
        }
        private ElemTableItem LastClickItem;
        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        private void ElemTableGraphic_MouseDown(object sender, MouseEventArgs e)
        {
            Int32 index = -1;
            Int32 line = -1;

            bool b = ItemWithPoint(new Point(e.X, e.Y), ref index, ref line);
            if (b)
            {

                var item = items[index];
                var lst = GetSelItem();
                if (lst.Count >= _MaxLayElement && !lst.Contains(item)) return;



                if (line == -1)
                {
                    bool HasSelectedItem = item.SelectedLines.Any(x => x);
                    if (!HasSelectedItem) item.SelectedLines[0] = true;//一个未选中,选择Ka线系
                    item.Selected = !item.Selected;
                }
                else
                {
                    item.SelectedLines[line] = !item.SelectedLines[line];
                    item.Selected = item.SelectedLines.Any(x => x);
                }
                if (item.Selected && !multiSelect && LastClickItem != null)
                {
                    ResetItem(LastClickItem);
                }
                LastClickItem = item.Selected ? item : null;


                Invalidate(GridRect(item.Column, item.Row));

            }
        }

        private void ResetItem(ElemTableItem item)
        {
            item.Selected = false;
            item.SelectedLines = new bool[LineCount];
            Invalidate(GridRect(item.Column, item.Row));

        }
    }

    internal class ElemTableItem
    {
        public string Name;
        public string ID;
        public int Column;
        public int Row;
        public bool[] SelectedLines;
        public bool Selected;
        public ElemTableItem()
        {
            SelectedLines = new bool[ElementTable.LineCount];
        }
    }
}