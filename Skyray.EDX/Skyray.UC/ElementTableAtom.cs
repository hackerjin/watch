using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Skyray.UC
{
    /// <summary>
    /// Ԫ�����ڱ�ѡ���Ԫ��
    /// </summary>
    internal struct ElemTableItem
    {
        /// <summary>
        /// ����
        /// </summary>
        public string Name;
        /// <summary>
        /// ���
        /// </summary>
        public string ID;
        /// <summary>
        /// ��
        /// </summary>
        public int Column;
        /// <summary>
        /// ��
        /// </summary>
        public int Row;
        /// <summary>
        /// �Ƿ�ѡ��
        /// </summary>
        public bool Selected;
    }

    /// <summary>
    /// Ԫ�����ڱ��е�ԭ��
    /// </summary>
    public partial class ElementTableAtom : Skyray.Language.UCMultiple
    {
        /// <summary>
        /// �Ƿ���ʾȷ��ȡ����ť
        /// </summary>

        //private bool _ShowButton = false;
        public bool ShowButton
        {
            //get { return _ShowButton; }
            set
            {
                //_ShowButton = value;
                this.btnClose.Visible = this.btnOK.Visible = value;
            }
        }

        public DialogResult DialogResult;//�Ի����
        private const string postfix = ";";            //<ѡ��Ԫ���������еĵĺ�׺
        private int gridHeight;             //<��Ԫ��ĸ߶�
        public int GridHeight
        {
            get { return gridHeight; }
            set
            {
                if (value > 0)
                {
                    gridHeight = value;
                    Height = gridHeight * 7 + gridSpace;
                    Invalidate();
                }
            }
        }
        private int gridWidth;              //<��Ԫ���Ŀ��
        public int GridWidth
        {
            get { return gridWidth; }
            set
            {
                if (value > 0)
                {
                    gridWidth = value;
                    Width = 18 * gridWidth + gridSpace;
                    Invalidate();
                }
            }
        }
        private int gridSpace;              //<����֮����
        private Color gridColor;              //<��Ԫ�����ɫ
        public Color GridColor
        {
            get { return gridColor; }
            set
            {
                if (value != gridColor)
                {
                    gridColor = value;
                    Invalidate();
                }
            }
        }
        private Color selectColor;          //<ѡ�е���ɫ
        public Color SelectColor
        {
            get { return selectColor; }
            set
            {
                if (value != selectColor)
                {
                    selectColor = value;
                    Invalidate();
                }
            }
        }
        private Color unselectColor;        //<δѡ�е���ɫ
        public Color UnselectColor
        {
            get { return unselectColor; }
            set
            {
                if (value != unselectColor)
                {
                    unselectColor = value;
                    Invalidate();
                }
            }
        }
        private ElemTableItem[] items;      //<���Ԫ�����͵���ֵ
        private int selectedCount //<ѡ��Ԫ�صĸ���
        {
            get
            {
                return selectedIndexs.Length / 4;
            }
        }
        private bool multiSelect;           //<�Ƿ���Զ�ѡ
        public bool MultiSelect
        {
            get { return multiSelect; }
            set
            {
                if (multiSelect != value)
                {
                    multiSelect = value;
                    int index;
                    if (!multiSelect)//�ɶ�ѡ��Ϊ��ѡ��ȫ����Ϊδѡ״̬
                    {
                        do
                        {
                            index = FirstSelectedIndex();
                            if (index >= 0)
                            {
                                RemoveSelectedIndex(index);
                                items[index].Selected = false;
                            }
                        } while (index >= 0);
                    }
                    Invalidate();
                }
            }
        }
        private string selectedIndexs;        //<ѡ�е�Ԫ��������������ʽΪ��<������>
        public override void PageLoad(object sender, EventArgs e)
        {
            base.PageLoad(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        public ElementTableAtom()
        {
            InitializeComponent();
            items = new ElemTableItem[86];
            gridHeight = 48;
            gridWidth = 40;
            gridSpace = 4;
            unselectColor = Color.FromArgb(175, 210, 255);
            selectColor = Color.Orange;
            BackColor = Color.White;
            gridColor = Color.FromArgb(200, 101, 147, 207);
            selectedIndexs = string.Empty;
            InitalizeItem();
        }
        /// <summary>
        /// ��ʼ��Ԫ�ص���Ϣ
        /// </summary>
        private void InitalizeItem()
        {
            int col, row, id;
            for (int i = 0; i < 82; i++)
            {
                id = 11 + i;
                items[i].ID = id.ToString();
                SetItemLocation(id, out col, out row);
                items[i].Column = col;
                items[i].Row = row;
                items[i].Selected = false;
            }
            items[82].ID = "*"; items[82].Name = "*";
            items[82].Row = 4; items[82].Column = 3;
            items[83].ID = "**"; items[83].Name = "**";
            items[83].Row = 5; items[83].Column = 3;
            items[84].ID = "*"; items[84].Name = "Lanthan";
            items[84].Row = 6; items[84].Column = 1;
            items[85].ID = "**"; items[85].Name = "Actinid";
            items[85].Row = 7; items[85].Column = 1;

            items[0].Name = "Na"; items[1].Name = "Mg"; items[2].Name = "Al";
            items[3].Name = "Si"; items[4].Name = "P"; items[5].Name = "S";
            items[6].Name = "Cl"; items[7].Name = "Ar"; items[8].Name = "K";
            items[9].Name = "Ca"; items[10].Name = "Sc"; items[11].Name = "Ti";
            items[12].Name = "V"; items[13].Name = "Cr"; items[14].Name = "Mn";
            items[15].Name = "Fe"; items[16].Name = "Co"; items[17].Name = "Ni";
            items[18].Name = "Cu"; items[19].Name = "Zn"; items[20].Name = "Ga";
            items[21].Name = "Ge"; items[22].Name = "As"; items[23].Name = "Se";
            items[24].Name = "Br"; items[25].Name = "Kr"; items[26].Name = "Rb";
            items[27].Name = "Sr"; items[28].Name = "Y"; items[29].Name = "Zr";
            items[30].Name = "Nb"; items[31].Name = "Mo"; items[32].Name = "Tc";
            items[33].Name = "Ru"; items[34].Name = "Rh"; items[35].Name = "Pd";
            items[36].Name = "Ag"; items[37].Name = "Cd"; items[38].Name = "In";
            items[39].Name = "Sn"; items[40].Name = "Sb"; items[41].Name = "Te";
            items[42].Name = "I"; items[43].Name = "Xe"; items[44].Name = "Cs";
            items[45].Name = "Ba"; items[46].Name = "La"; items[47].Name = "Ce";
            items[48].Name = "Pr"; items[49].Name = "Nd"; items[50].Name = "Pm";
            items[51].Name = "Sm"; items[52].Name = "Eu"; items[53].Name = "Gd";
            items[54].Name = "Tb"; items[55].Name = "Dy"; items[56].Name = "Ho";
            items[57].Name = "Er"; items[58].Name = "Tm"; items[59].Name = "Yb";
            items[60].Name = "Lu"; items[61].Name = "Hf"; items[62].Name = "Ta";
            items[63].Name = "W"; items[64].Name = "Re"; items[65].Name = "Os";
            items[66].Name = "Ir"; items[67].Name = "Pt"; items[68].Name = "Au";
            items[69].Name = "Hg"; items[70].Name = "Tl"; items[71].Name = "Pb";
            items[72].Name = "Bi"; items[73].Name = "Po"; items[74].Name = "At";
            items[75].Name = "Rn"; items[76].Name = "Fr"; items[77].Name = "Ra";
            items[78].Name = "Ac"; items[79].Name = "Th"; items[80].Name = "Pa";
            items[81].Name = "U";
        }

        ///<summary>
        ///����Ԫ����ʾλ��
        ///</summary>
        ///<param name="elemID">Ԫ�ص����</param>
        ///<param name="col">��</param>
        ///<param name="row">��</param>
        private void SetItemLocation(int elemID, out int col, out int row)
        {
            col = 0;
            row = 0;
            if (elemID < 13)
            {
                row = 1;
                col = elemID - 10;
            }
            else if (elemID < 57)
            {
                row = (elemID - 1) / 18 + 1;
                col = elemID - (row - 1) * 18;
            }
            else if (elemID < 72)
            {
                row = 6;
                col = elemID - 53;
            }
            else if (elemID < 89)
            {
                row = (elemID - 69) / 18 + 4;
                col = elemID - 68 - (row - 4) * 18;
            }
            else
            {
                row = 7;
                col = elemID - 85;
            }
        }

        /// <summary>
        /// ������Ԫ���ڵ�λ������
        /// </summary>
        /// <param name="row">�������</param>
        /// <param name="col">�������</param>
        /// <returns></returns>
        private Rectangle GridRect(int col, int row)
        {
            int left = 0;
            int top = 0;
            int width = 0;
            int height = 0;
            if (row >= 6 && row <= 7 && col >= 1 && col <= 3)
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

        /// <summary>
        /// �����ָ���������Ԫ�� 
        /// </summary>
        /// <param name="point">��</param>
        /// <returns>�������Ԫ�� </returns>
        private int ItemWithPoint(Point point)
        {
            int col = (point.X - gridSpace) / gridWidth + 1;
            int row = (point.Y - gridSpace) / gridHeight + 1;
            if ((col > 0) && (row > 0) && (col < 19) && (row < 8))
            {
                for (int i = 0; i < 82; i++)//85-87����ѡ
                {
                    if ((col == items[i].Column) && (row == items[i].Row))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// ��Ԫ��
        /// </summary>
        /// <param name="index">Ԫ�ص�����</param>
        /// <param name="g">ͼ����</param>
        private void DrawItem(Graphics g, int index)
        {
            Rectangle rect = GridRect(items[index].Column, items[index].Row);
            Brush backBrush;
            Brush textBrush;
            Pen pen = new Pen(gridColor);
            if (items[index].Selected)
            {
                backBrush = new SolidBrush(selectColor);
                textBrush = new SolidBrush(Color.White);
            }
            else
            {
                backBrush = new SolidBrush(unselectColor);
                textBrush = new SolidBrush(ForeColor);
            }
            g.FillRectangle(backBrush, rect);
            g.DrawRectangle(pen, rect);
            Size fontSize = g.MeasureString(items[index].Name, Font).ToSize();
            int x = (rect.Width - fontSize.Width) / 2 + rect.Left;
            int y = (rect.Height / 2 - fontSize.Height) / 2 + rect.Top;
            g.DrawString(items[index].Name, Font, textBrush, x, y);

            fontSize = g.MeasureString(items[index].ID, Font).ToSize();
            x = (rect.Width - fontSize.Width) / 2 + rect.Left;
            y = rect.Top + (rect.Height * 3 - 2 * fontSize.Height) / 4;
            g.DrawString(items[index].ID, Font, textBrush, x, y);
        }

        /// <summary>
        /// �õ���һ��ѡ��Ԫ�ص�����
        /// </summary>
        /// <returns></returns>
        private int FirstSelectedIndex()
        {
            int index = -1;
            index = selectedIndexs.IndexOf(postfix);
            if (index < 0)
            {
                return index;
            }
            index = Convert.ToInt32(selectedIndexs.Substring(0, index));
            return index;
        }

        /// <summary>
        /// ��ѡ��Ԫ�ص�������ɾ��һ��Ԫ������
        /// </summary>
        /// <param name="index">Ҫɾ����Ԫ������</param>
        private void RemoveSelectedIndex(int index)
        {
            string revIndex = index.ToString("000") + postfix;
            index = selectedIndexs.IndexOf(revIndex);
            if (index >= 0)
            {
                selectedIndexs = selectedIndexs.Remove(index, revIndex.Length);
            }
        }

        /// <summary>
        /// ���һ��Ԫ�ص�ѡ�е�Ԫ�ر���
        /// </summary>
        /// <param name="index"></param>
        private void AddSelectedIndex(int index)
        {
            selectedIndexs = selectedIndexs + index.ToString("000") + postfix;
        }

        /// <summary>
        /// ����һ��Ԫ�ص�״̬
        /// </summary>
        /// <param name="index">Ԫ������</param>
        /// <param name="selected">�Ƿ�ѡ��</param>
        private void SetItem(int index, bool selected)
        {
            if (items[index].Selected == selected)
            {
                return;
            }
            items[index].Selected = selected;
            if (!items[index].Selected)
            {
                items[index].Selected = false;
                RemoveSelectedIndex(index);
                Invalidate(GridRect(items[index].Column, items[index].Row));
                return;
            }
            //ѡ��Ԫ��
            if (multiSelect)
            {
                AddSelectedIndex(index);
            }
            else
            {
                int preIndex = FirstSelectedIndex();
                if (preIndex >= 0)
                {
                    items[preIndex].Selected = false;
                    Invalidate(GridRect(items[preIndex].Column, items[preIndex].Row));
                }
                selectedIndexs = string.Empty;
                AddSelectedIndex(index);
            }
            Invalidate(GridRect(items[index].Column, items[index].Row));
        }

        /// <summary>
        /// ��ͼ�¼�
        /// </summary>
        private void ElemTableGraphic_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < items.Length; i++)
            {
                DrawItem(e.Graphics, i);
            }
        }

        /// <summary>
        /// ��갴���¼�
        /// </summary>
        private void ElemTableGraphic_MouseDown(object sender, MouseEventArgs e)
        {
            int index = ItemWithPoint(new Point(e.X, e.Y));
            if (index >= 0)
            {
                if (MultiSelect)
                {
                    SetItem(index, !items[index].Selected);
                }
                else
                {
                    SetItem(index, true);
                    //SetItem(index, !items[index].Selected); 
                }
            }
        }

        /// <summary>
        ///  ѡ�е�Ԫ������ 
        /// </summary>
        /// <returns>ѡ�е�Ԫ������</returns>
        public string[] SelectedItems
        {
            get
            {
                if (selectedCount <= 0)
                {
                    return null;
                }
                int index;
                string[] separator = new string[] { postfix };
                string[] selItems = selectedIndexs.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < selItems.Length; i++)
                {
                    index = Convert.ToInt32(selItems[i]);
                    selItems[i] = items[index].Name;
                }
                return selItems;
            }
            set
            {
                if (value == null)
                {
                    return;
                }
                int length = value.Length;
                if (length < 0)
                {
                    return;
                }
                else
                {
                    // �����ѡԪ��
                    ClearSelection();

                    // ����ѡ��Ԫ��
                    for (int i = 0; i < length; i++)
                    {
                        SetItem(value[i], true);
                    }
                }
            }

        }
        /// <summary>
        /// �����ѡԪ��
        /// </summary>
        public void ClearSelection()
        {
            if (!this.Created)
            {
                return;
            }
            if (SelectedItems == null)
            {
                return;
            }
            string[] arrSelectedItems = SelectedItems;
            int length = arrSelectedItems.Length;
            for (int i = 0; i < length; i++)
            {
                //SetItem(arrSelectedItems[i], false);
                SetItem(arrSelectedItems[i], false);
                SetItem(arrSelectedItems[i], false);
            }
        }
        /// <summary>
        /// ����һ��Ԫ�ص�״̬
        /// </summary>
        /// <param name="index">Ԫ������</param>
        /// <param name="selected">�Ƿ�ѡ��</param>
        public void SetItem(string elemName, bool selected)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (elemName.Equals(items[i].Name))
                {
                    SetItem(i, selected);
                    break;
                }
            }
        }
        /// <summary>
        /// ѡ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.ParentForm.Close();

        }
        /// <summary>
        /// �ر�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.ParentForm.Close();
        }
    }
}