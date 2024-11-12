using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Linq;
using System.Data;
using Skyray.Controls;
using SourceGrid;


namespace Skyray.Print
{
    /// <summary>
    /// 打印标签控件
    /// </summary>
    public class PrintCtrl : Control
    {
        /// <summary>
        /// 选择改变事件
        /// </summary>
        public event EventHandler<EventArgs> SelectedChanged;

        #region 私有变量

        /// <summary>
        /// 字体改变时，记录宽度增加值
        /// </summary>
        private int OffsetWidth = 0;

        /// <summary>
        /// 字体改变时，记录高度增加值
        /// </summary>
        private int OffsetHeight = 0;

        /// <summary>
        /// 八爪大小
        /// </summary>
        private Size _Sqare = new Size(6, 6);// 八爪大小

        /// <summary>
        /// 八爪内矩形区域
        /// </summary>
        private Rectangle _baseRect;//八爪内矩形区域         
        /// <summary>
        /// 拖拽手柄的矩形区域
        /// </summary>
        private Rectangle _DragImageRect;//拖拽手柄的矩形区域
        /// <summary>
        /// 八爪矩形
        /// </summary>
        private Rectangle[] _SmallRect = new Rectangle[8];//八爪矩形
        /// <summary>
        /// 记录是否正在拖拽
        /// </summary>
        private bool _Dragging = false;//记录是否正在拖拽
        /// <summary>
        /// 拖拽起始点
        /// </summary>
        private Point _DragStart = Point.Empty;//拖拽起始点 
        /// <summary>
        /// 拖拽图形
        /// </summary>
        public Image _DragImage;
        /// <summary>
        /// 记录当前鼠标模式
        /// </summary>
        private RESIZE_BORDER _CurrBorder;//记录当前鼠标模式
        /// <summary>
        /// 鼠标X轴移动距离
        /// </summary>
        private int
            _OffSetX,//鼠标X轴移动距离
            _OffSetY, //鼠标Y轴移动距离
            _LeftNew, //控件新坐标Left
            _TopNew,//控件新坐标Top
            _WidthNewA, //控件新宽度，自增
            _HeightNewA, //控件新高度，自增
            _WidthNewD, //控件新宽度，自减
            _HeightNewD;//控件新高度，自减

        #endregion

        #region 属性

        public bool SaveValue { get; set; }

        private Rectangle _ContentRect = new Rectangle();
        /// <summary>
        /// 客户区域矩形
        /// </summary>
        public Rectangle ContentRect
        {
            get { return _ContentRect; }
            set
            {
                _ContentRect = value;
            }
        }

        private Rectangle _TitleRect = new Rectangle();
        public Rectangle TitleRect
        {
            get { return _TitleRect; }
            set { _TitleRect = value; }
        }

        /// <summary>
        /// 控件关联节点属性
        /// </summary>
        public TreeNodeInfo NodeInfo { get; set; }

        /// <summary>
        /// Sqare
        /// </summary>
        public Size Sqare
        {
            get { return _Sqare; }
        }

        /// <summary>
        /// 文本大小
        /// </summary>
        private Size _TextSize = Size.Empty;
        /// <summary>
        /// 文本大小
        /// </summary>
        public Size TextSize
        {
            get { return _TextSize; }

        }
        /// <summary>
        /// 是否绘制拖拽手柄
        /// </summary>
        public bool DrawDragImage { get; set; }


        /// <summary>
        /// 客户区域
        /// </summary>
        public Rectangle BaseRect
        {
            get { return _baseRect; }
        }

        /// <summary>
        /// 是否为设计模式
        /// </summary>
        private bool _IsDesignMode = true;

        /// <summary>
        /// 控件是否为设计模式
        /// </summary>
        public bool IsDesignMode
        {
            get { return _IsDesignMode; }
            set { _IsDesignMode = value; }
        }

        /// <summary>
        /// 是否初始化完毕
        /// </summary>
        private bool _InitFinish;

        /// <summary>
        /// 记录控件的属性是否加载完全
        /// </summary>
        public bool InitFinish
        {
            get { return _InitFinish; }
            set
            {
                _InitFinish = value;
                if (value)
                {
                    InitFinished();
                }
            }
        }

        /// <summary>
        /// 父容器
        /// </summary>
        public PrintPanel Panel { get; private set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        private bool _Selected = false;

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Selected
        {
            get { return _Selected; }
            set
            {
                if (value != _Selected)//非空
                {
                    if (value)
                        Panel.SelectedCtrls.Add(this);
                    else
                        Panel.SelectedCtrls.Remove(this);//从选择容器中移除本控件

                    _Selected = value;//赋值
                    SetEvent();//事件添加
                    this.Invalidate();//刷新绘制八爪
                    if (SelectedChanged != null) //判断选择事件是否为空
                        SelectedChanged(this, null);//选择改变触发
                }
            }
        }

        /// <summary>
        /// 文本字体
        /// </summary>
        private Font _TextFont; //与Font属性公用时，该控件内部所有控件字体属性会随之改变
        public Font TextFont
        {
            get { return _TextFont; }
            set
            {
                _TextFont = value;
                if (Param.ChangeDataSourceValue) NodeInfo.TextFont = value;
                if (_InitFinish) RePaint();//刷新
            }
        }

        /// <summary>
        /// 加载完毕
        /// </summary>
        public virtual void InitFinished()
        {
            RePaint();
        }
        /// <summary>
        /// 控件类型
        /// </summary>
        public CtrlType Type { get; set; }

        /// <summary>
        /// 文本颜色
        /// </summary>
        private Color _TextColor = Color.Black;

        /// <summary>
        /// 文本颜色
        /// </summary>
        public Color TextColor
        {
            get { return _TextColor; }
            set
            {
                _TextColor = value;
                if (Param.ChangeDataSourceValue) NodeInfo.TextColor = value;
                base.Invalidate();
            }
        }

        /// <summary>
        /// 文本值与下方元素的垂直距离
        /// </summary>
        private int _TextVSpace = 0;

        /// <summary>
        /// 文本值与下方元素的垂直距离
        /// </summary>
        public int TextVSpace
        {
            get { return _TextVSpace; }
            set
            {
                _TextVSpace = value;//属性赋值
                if (Param.ChangeDataSourceValue) NodeInfo.TextVSpace = value;
                if (InitFinish) RePaint();
            }
        }

        /// <summary>
        /// 文本垂直间距改变
        /// </summary>
        /// <param name="offsetY"></param>
        public virtual void TextVSpaceChanged(int offsetY)
        {
            if (InitFinish) RePaint();
        }

        /// <summary>
        /// 文本字体改变
        /// </summary>
        /// <param name="offsetY"></param>
        public virtual void TextFontChanged(int offsetY)
        {
            if (InitFinish) RePaint();
        }
        #endregion

        #region 构造函数

        public PrintCtrl(PrintPanel panel)
            : this(panel, false)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="panel"></param>
        public PrintCtrl(PrintPanel panel, bool isDrawDragImage)
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer
                    | ControlStyles.UserPaint
                    | ControlStyles.AllPaintingInWmPaint
                    | ControlStyles.ResizeRedraw, true);

            DrawDragImage = isDrawDragImage;//是否绘制拖动图片

            _DragImage = Properties.Resources.X;//拖动箭头           
            _DragImageRect = new Rectangle(0, 0, _DragImage.Width, _DragImage.Height);//手柄矩形 
            Panel = panel;

            _baseRect = DrawDragImage ?
                new Rectangle(_Sqare.Width + _DragImage.Width / 2, _Sqare.Height + _DragImage.Height / 2, 1, 1) :
                new Rectangle(_Sqare.Width, _Sqare.Height, 1, 1);

            base.Click += new EventHandler(PrintCtrl_Click);//注册单击事件
            base.SizeChanged += new EventHandler(PrintCtrl_SizeChanged);//注册大小改变事件
            base.TextChanged += new EventHandler(PrintCtrl_TextChanged);

            this.MaximumSize = Panel.GetInnerCtrlMaxSize();
            base.Padding = new Padding(_baseRect.Left - 1, _baseRect.Top - 1, 1, 1);
        }

        void PrintCtrl_TextChanged(object sender, EventArgs e)
        {
            if (Param.ChangeDataSourceValue)
            {
                NodeInfo.Text = base.Text;
            }
            if (InitFinish) RePaint();
        }

        /// <summary>
        /// 控件大小改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PrintCtrl_SizeChanged(object sender, EventArgs e)
        {
            PreSelectRectPaint();
        }

        #endregion

        #region 重绘事件（选择框绘制）
        /// <summary>
        /// 设置选择框
        /// </summary>
        private void SetRectangles()
        {
            bool b = (Type == CtrlType.Image||Type == CtrlType.Grid || Type == CtrlType.ComposeTable);//图形

            //简单三个点的拖拽
            if (Param.ThreeRect)
            {
                //右下
                _SmallRect[3] = b ? new Rectangle(new Point(_baseRect.X + _baseRect.Width, _baseRect.Y + _baseRect.Height), _Sqare)
                    : Rectangle.Empty;

                //下中
                _SmallRect[5] = b ? new Rectangle(new Point(_baseRect.X + (_baseRect.Width / 2) - (_Sqare.Width / 2), _baseRect.Y + _baseRect.Height), _Sqare)
                    : Rectangle.Empty;

                //右中
                _SmallRect[7] = Type == CtrlType.Label ? Rectangle.Empty
                    : new Rectangle(new Point(_baseRect.X + _baseRect.Width, _baseRect.Y + (_baseRect.Height / 2) - (_Sqare.Height / 2)), _Sqare);

            }
            else
            {
                //左上
                _SmallRect[0] = Type == CtrlType.Image ?
                    new Rectangle(new Point(_baseRect.X - _Sqare.Width, _baseRect.Y - _Sqare.Height), _Sqare)
                    : Rectangle.Empty;
                //右上
                _SmallRect[1] = b ?
                    new Rectangle(new Point(_baseRect.X + _baseRect.Width, _baseRect.Y - _Sqare.Height), _Sqare)
                    : Rectangle.Empty;

                //左下
                _SmallRect[2] = b ? new Rectangle(new Point(_baseRect.X - _Sqare.Width, _baseRect.Y + _baseRect.Height), _Sqare)
                    : Rectangle.Empty;

                //右下
                _SmallRect[3] = b ? new Rectangle(new Point(_baseRect.X + _baseRect.Width, _baseRect.Y + _baseRect.Height), _Sqare)
                    : Rectangle.Empty;

                //上中
                _SmallRect[4] = b ? new Rectangle(new Point(_baseRect.X + (_baseRect.Width / 2) - (_Sqare.Width / 2), _baseRect.Y - _Sqare.Height), _Sqare)
                    : Rectangle.Empty;
                //下中
                _SmallRect[5] = b ? new Rectangle(new Point(_baseRect.X + (_baseRect.Width / 2) - (_Sqare.Width / 2), _baseRect.Y + _baseRect.Height), _Sqare)
                    : Rectangle.Empty;

                //左中
                _SmallRect[6] = Type == CtrlType.Label ? Rectangle.Empty
                    : new Rectangle(new Point(_baseRect.X - _Sqare.Width, _baseRect.Y + (_baseRect.Height / 2) - (_Sqare.Height / 2)), _Sqare);
                //右中
                _SmallRect[7] = Type == CtrlType.Label ? Rectangle.Empty
                    : new Rectangle(new Point(_baseRect.X + _baseRect.Width, _baseRect.Y + (_baseRect.Height / 2) - (_Sqare.Height / 2)), _Sqare);
            }
        }


        /// <summary>
        /// 设置BaseRect对象
        /// </summary>
        public void SetBaseRectSize()
        {
            if (DrawDragImage)//判断是否绘制拖拽图形
            {
                _baseRect.Width = base.Width - _Sqare.Width - _DragImage.Width;
                _baseRect.Height = base.Height - _Sqare.Height - _DragImage.Height;
            }
            else
            {
                //_baseRect.Width = base.Width - _Sqare.Width * 2 - 1;
                if (Type == CtrlType.Label)
                {
                    _baseRect.Width = base.Width - _Sqare.Width * 2 +2;
                }
                else
                {
                    _baseRect.Width = base.Width - _Sqare.Width * 2 - 1;
                }
                _baseRect.Height = base.Height - _Sqare.Height * 2 - 1;
            }

            int textHeight = TextSize.Height + TextVSpace;

            _TitleRect.Location = _baseRect.Location;
            _TitleRect.Size = new Size(_baseRect.Width, textHeight);

            _ContentRect.Location = new Point(_baseRect.Left, textHeight + BaseRect.Top);
            _ContentRect.Size = new Size(_baseRect.Width, _baseRect.Height - textHeight);
        }


        /// <summary>
        /// 重绘前参数准备
        /// </summary>
        public void PreSelectRectPaint()
        {
            SetBaseRectSize();
            SetRectangles();
        }

        /// <summary>
        /// 绘制事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            #region 八爪绘制
            if (_Selected)
            {
                e.Graphics.DrawRectangle(Param.DashPen,//虚线框
                (_Sqare.Width + (DrawDragImage == true ? _DragImage.Width : 0)) / 2,//左上顶点X
                 (_Sqare.Height + (DrawDragImage == true ? _DragImage.Height : 0)) / 2,//左上顶点Y
                 _baseRect.Width + _Sqare.Width,//width
                 _baseRect.Height + _Sqare.Height);//height

                for (int i = 0; i < 8; i++)//八爪框
                {
                    //绘制拖动手柄或者矩形为空时，不绘制左上小方框
                    if (_SmallRect[i] == Rectangle.Empty) continue;

                    e.Graphics.FillRectangle(Panel.CurrentCtrl == this ? Brushes.White
                        : Brushes.DarkGray, _SmallRect[i]);//填充小方格，最后一个选择的控件用白色填充

                    e.Graphics.DrawRectangle(Pens.Black, _SmallRect[i]);//绘制八爪
                }

                if (DrawDragImage) e.Graphics.DrawImage(_DragImage, Point.Empty);//拖拽手柄 
            }
            #endregion

            #region 文本绘制

            if (!string.IsNullOrEmpty(Text))
            {
                if (Text == "样品图" || Text=="SampleImage")
                {
                    Text = "";
                }
                var font = TextFont == null ? Font : TextFont;
                int max = this.Size.Width - this._Sqare.Width;
                string sText = GetShortText(e.Graphics, Text, _TextSize.Width, font, max);
                e.Graphics.DrawString(sText,//文本
                  font, //字体
                   TextColor.IsEmpty ? Brushes.Black : new Pen(TextColor).Brush, //画刷
                     Padding.Left,//坐标X
                     Padding.Top);//坐标Y
            }
            #endregion
        }

        public string GetShortText(Graphics g, string text, int textWidth, Font font, int max)
        {
            string sText = text;
            if (textWidth > max)
            {
                for (int i = text.Length - 1; i >= 0; i--)
                {
                    sText = text.Remove(i) + "..";
                    var size = g.MeasureString(sText, font);
                    if (size.Width <= max)
                    {
                        if (i + 1 < text.Length - 1) sText = text.Remove(i + 1) + "..";
                        break;
                    }
                }
            }
            return sText;
        }

        #endregion

        #region  HitTest
        /// <summary>
        /// 检查点击的位置
        /// </summary>
        /// <param name="point">当前点位置</param>
        /// <returns></returns>
        private bool Hit_Test(Point point)
        {
            if (_Dragging) return false;//正在拖动不响应

            Cursor.Current = Cursors.Arrow;
            _CurrBorder = RESIZE_BORDER.RB_NONE;//复位

            if (DrawDragImage)//绘制手柄
            {
                if (_DragImageRect.Contains(point))
                {
                    Cursor.Current = Cursors.SizeAll;
                    _CurrBorder = RESIZE_BORDER.RB_SIZEALL;
                }
            }
            else
            {
                if (_SmallRect[0].Contains(point))
                {
                    Cursor.Current = Cursors.SizeNWSE;
                    _CurrBorder = RESIZE_BORDER.RB_TOPLEFT;
                }
            }

            if (_SmallRect[3].Contains(point))
            {
                Cursor.Current = Cursors.SizeNWSE;
                _CurrBorder = RESIZE_BORDER.RB_BOTTOMRIGHT;
            }
            else if (_SmallRect[1].Contains(point))
            {
                Cursor.Current = Cursors.SizeNESW;
                _CurrBorder = RESIZE_BORDER.RB_TOPRIGHT;
            }
            else if (_SmallRect[2].Contains(point))
            {
                Cursor.Current = Cursors.SizeNESW;
                _CurrBorder = RESIZE_BORDER.RB_BOTTOMLEFT;
            }
            else if (_SmallRect[4].Contains(point))
            {
                Cursor.Current = Cursors.SizeNS;
                _CurrBorder = RESIZE_BORDER.RB_TOP;
            }
            else if (_SmallRect[5].Contains(point))
            {
                Cursor.Current = Cursors.SizeNS;
                _CurrBorder = RESIZE_BORDER.RB_BOTTOM;
            }
            else if (_SmallRect[6].Contains(point))
            {
                Cursor.Current = Cursors.SizeWE;
                _CurrBorder = RESIZE_BORDER.RB_LEFT;
            }
            else if (_SmallRect[7].Contains(point))
            {
                Cursor.Current = Cursors.SizeWE;
                _CurrBorder = RESIZE_BORDER.RB_RIGHT;
            }

            else if (this._baseRect.Contains(point))
            {
                Cursor.Current = DrawDragImage ? Cursors.Arrow : Cursor.Current = Cursors.SizeAll;
                _CurrBorder = DrawDragImage ? RESIZE_BORDER.RB_NONE : RESIZE_BORDER.RB_SIZEALL;
            }
            return true;
        }

        /// <summary>
        /// Check point position to see if it's on tracker
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns></returns>
        private bool Hit_Test(int x, int y)
        {
            return Hit_Test(new Point(x, y));
        }
        #endregion

        #region  计算控件位置及大小范围

        /// <summary>
        /// 设置新的大小及位置
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="ctrl"></param>
        public void SetNewSizeAndLocation(int X, int Y, Control ctrl)
        {
            //计算大小和位置
            CalcSizeAndLocation(X, Y, ctrl);

            int widthNew = ctrl.Width;//记录新的宽度
            int widthOld = ctrl.Width;//记录旧的宽度

            #region  更改大小及位置

            if (_CurrBorder == RESIZE_BORDER.RB_SIZEALL)
            {
                //ctrl.Left = _LeftNew;
                //ctrl.Top = _TopNew;
                ctrl.Location = new Point(_LeftNew, _TopNew);
            }
            else if (_CurrBorder == RESIZE_BORDER.RB_TOPLEFT)
            {
                //ctrl.Left = _LeftNew;
                //ctrl.Top = _TopNew;
                ctrl.Location = new Point(_LeftNew, _TopNew);
                widthNew = _WidthNewD;
                ctrl.Height = _HeightNewD;
            }
            else if (_CurrBorder == RESIZE_BORDER.RB_TOP)
            {
                ctrl.Top = _TopNew;
                ctrl.Height = _HeightNewD;
            }
            else if (_CurrBorder == RESIZE_BORDER.RB_TOPRIGHT)
            {
                ctrl.Top = _TopNew;
                widthNew = _WidthNewA;
                ctrl.Height = _HeightNewD;
            }
            else if (_CurrBorder == RESIZE_BORDER.RB_BOTTOMLEFT)
            {
                ctrl.Left = _LeftNew;
                widthNew = _WidthNewD;
                ctrl.Height = _HeightNewA;
            }
            else if (_CurrBorder == RESIZE_BORDER.RB_BOTTOM)
            {
                ctrl.Height = _HeightNewA;
            }
            else if (_CurrBorder == RESIZE_BORDER.RB_BOTTOMRIGHT)
            {
                widthNew = _WidthNewA;
                ctrl.Height = _HeightNewA;
            }
            else if (_CurrBorder == RESIZE_BORDER.RB_LEFT)
            {
                ctrl.Left = _LeftNew;
                widthNew = _WidthNewD;
            }
            else if (_CurrBorder == RESIZE_BORDER.RB_RIGHT)
            {
                widthNew = _WidthNewA;
            }
            if (widthNew != widthOld)
            {
                WidthChanging(widthOld, widthNew);
                ctrl.Width = widthNew;
                WidthChanged(widthOld, widthNew);
            }
            #endregion
        }

        /// <summary>
        /// 计算控件大小及位置
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="ctrl"></param>
        public void CalcSizeAndLocation(int X, int Y, Control ctrl)
        {
            _OffSetX = X - _DragStart.X;//X轴偏移量
            _OffSetY = Y - _DragStart.Y;//Y轴偏移量

            _LeftNew = Math.Max(Panel.CtrlMargin.Left, ctrl.Left + _OffSetX);//左顶点X
            _TopNew = Math.Max(Panel.CtrlMargin.Top, ctrl.Top + _OffSetY);//左顶点Y

            _LeftNew = Math.Min(
                Math.Max(Panel.ClientSize.Width - Panel.CtrlMargin.Right - ctrl.Width, Panel.CtrlMargin.Left),
                _LeftNew);//左顶点X
            _TopNew = Math.Min(
                 Math.Max(Panel.ClientSize.Height - Panel.CtrlMargin.Bottom - ctrl.Height, Panel.CtrlMargin.Top),
                _TopNew);//左顶点Y

            _WidthNewA = ctrl.Width + _OffSetX;
            if (_WidthNewA + ctrl.Left > Panel.ClientSize.Width - Panel.CtrlMargin.Right)
                _WidthNewA = Panel.ClientSize.Width - Panel.CtrlMargin.Right - ctrl.Left;

            _HeightNewA = ctrl.Height + _OffSetY;
            if (_HeightNewA + ctrl.Top > Panel.ClientSize.Height - Panel.CtrlMargin.Bottom)
                _HeightNewA = Panel.ClientSize.Height - Panel.CtrlMargin.Bottom - ctrl.Top;

            _WidthNewD = ctrl.Width - _OffSetX;
            if (_LeftNew == Panel.CtrlMargin.Left)//最左端
                _WidthNewD = ctrl.Width + ctrl.Left - Panel.CtrlMargin.Left;

            _HeightNewD = ctrl.Height - _OffSetY;
            if (_TopNew == Panel.CtrlMargin.Top)//最顶端
                _HeightNewD = ctrl.Height + ctrl.Top - Panel.CtrlMargin.Top;

            if (_WidthNewA < ctrl.MinimumSize.Width) _WidthNewA = ctrl.MinimumSize.Width;//最小宽
            if (_WidthNewD < ctrl.MinimumSize.Width) _WidthNewD = ctrl.MinimumSize.Width;//最小宽

            if (_HeightNewA < ctrl.MinimumSize.Height) _HeightNewA = ctrl.MinimumSize.Height;//最小高
            if (_HeightNewD < ctrl.MinimumSize.Height) _HeightNewD = ctrl.MinimumSize.Height;//最小高

            var typ = ctrl.GetType();
            if (typ == typeof(PrintCtrl))
            {
                _WidthNewA = _WidthNewD = ctrl.Width;
                _HeightNewA = _HeightNewD = ctrl.Height;
            }
            //else if (typ == typeof(PrintField) || typ == typeof(PrintTable))
            else if (typ == typeof(PrintField))
            {
                _HeightNewA = _HeightNewD = ctrl.Height;
            }
        }

        /// <summary>
        /// 设置控件新的大小及位置
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ctrl"></param>
        public void SetNewSizeAndLocation(MouseEventArgs e, Control ctrl)
        {
            SetNewSizeAndLocation(e.X, e.Y, ctrl);//设置
        }

        /// <summary>
        ///计算控件大小及位置
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ctrl"></param>
        private void CalcSizeAndLocation(MouseEventArgs e, Control ctrl)
        {
            CalcSizeAndLocation(e.X, e.Y, ctrl);//计算
        }

        #endregion

        #region 拖动控件时，在父容器中绘制轨迹

        /// <summary>
        /// 更新各矩形对应的滑动边框
        /// </summary>
        /// <param name="ctrl"></param>
        public void SlideBorder(Control ctrl)
        {
            Rectangle rect = Rectangle.Empty;
            if (_CurrBorder == RESIZE_BORDER.RB_SIZEALL)//All
            {
                rect = new Rectangle(_LeftNew, _TopNew, ctrl.Width, ctrl.Height);
            }
            else if (_CurrBorder == RESIZE_BORDER.RB_TOPLEFT)//RB_TOPLEFT
            {
                rect = new Rectangle(_LeftNew, _TopNew, _WidthNewD, _HeightNewD);
            }
            else if (_CurrBorder == RESIZE_BORDER.RB_TOP)//RB_TOP
            {
                rect = new Rectangle(ctrl.Left, _TopNew, ctrl.Width, _HeightNewD);
            }
            else if (_CurrBorder == RESIZE_BORDER.RB_TOPRIGHT)//TOPRIGHT
            {
                rect = new Rectangle(ctrl.Left, _TopNew, _WidthNewA, _HeightNewD);
            }
            else if (_CurrBorder == RESIZE_BORDER.RB_BOTTOMLEFT)//BOTTOMLEFT
            {
                //ctrl.Height = _HeightNewA;
                rect = new Rectangle(_LeftNew, ctrl.Top, _WidthNewD, _HeightNewA);
            }
            else if (_CurrBorder == RESIZE_BORDER.RB_BOTTOM)//BOTTOM
            {
                //ctrl.Height = _HeightNewA;
                rect = new Rectangle(ctrl.Left, ctrl.Top, ctrl.Width, _HeightNewA);
            }
            else if (_CurrBorder == RESIZE_BORDER.RB_BOTTOMRIGHT)//BOTTOMRIGHT
            {
                //ctrl.Height = _HeightNewA;
                //ctrl.Width = _WidthNewA;
                rect = new Rectangle(ctrl.Left, ctrl.Top, _WidthNewA, _HeightNewA);
            }
            else if (_CurrBorder == RESIZE_BORDER.RB_LEFT)//LEFT
            {
                //ctrl.Width = _WidthNewD;
                rect = new Rectangle(_LeftNew, ctrl.Top, _WidthNewD, ctrl.Height);
            }
            else if (_CurrBorder == RESIZE_BORDER.RB_RIGHT)//RIGHT
            {
                //ctrl.Width = _WidthNewA;
                rect = new Rectangle(ctrl.Left, ctrl.Top, _WidthNewA, ctrl.Height);
            }
            if (DrawDragImage) //客户区域
            {
                rect = new Rectangle(rect.Left + _Sqare.Width + _DragImage.Width / 2,
                    rect.Top + _Sqare.Height + _DragImage.Height / 2,
                    rect.Width - _Sqare.Width - 2 - _DragImage.Width,
                   rect.Height - _Sqare.Height - 2 - this._DragImage.Height);
            }
            else
            {
                rect = new Rectangle(rect.Left + _Sqare.Width / 2,
                    rect.Top + _Sqare.Height / 2,
                    rect.Width - _Sqare.Width,
                   rect.Height - _Sqare.Height);
            }
            Panel.Rects[ctrl] = rect;//拖动矩形
        }

        /// <summary>
        /// 拖动控件时，在父容器中绘制轨迹
        /// </summary>
        /// <param name="e"></param>
        private void ShowSlideBorder(MouseEventArgs e, List<PrintCtrl> lstCtrls)
        {
            Panel.Rects.Clear();
            //遍历控件集合
            foreach (var ctrl in lstCtrls)
            {
                CalcSizeAndLocation(e, ctrl);//计算控件位置
                SlideBorder(ctrl);//更新各矩形对应的滑动边框
            }
            Panel.InvalidateW();//刷新容器
        }

        #endregion

        #region 控件事件
        /// <summary>
        /// 单机事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintCtrl_Click(object sender, EventArgs e)
        {
            var lastCtrl = Panel.CurrentCtrl;//记录上次点击的控件
            Panel.CurrentCtrl = this;//当前选择控件

            foreach (PrintCtrl myCtrl in Panel.Controls)
            {
                if (myCtrl == this)
                {
                    if (!myCtrl.Selected) myCtrl.Selected = true;//选择
                }
                else
                {
                    if (Keys.Control == Control.ModifierKeys)//Ctrl未按下
                    {
                        if (lastCtrl != null) lastCtrl.Invalidate();//为非最后选中控件重绘
                    }
                    else
                    {
                        if (myCtrl.Selected && !_Dragging) myCtrl.Selected = false;//非选择                                
                    }
                }
            }
            Panel.Page.PropertyObject = Panel.SelectedCtrls.Count > 1 ? null : this;//当前选择的控件            
        }

        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintCtrl_MouseDown(object sender, MouseEventArgs e)
        {
            //this.Panel.Page.Parent.Focus();//Tabpage获得焦点
            Panel.Focus();
            this.Panel.Page.SelectedPanel = this.Panel;
            _Dragging = true;//开始拖拽
            _DragStart = new Point(e.X, e.Y);//记录开始拖拽位置
            this.Capture = true;
        }

        /// <summary>
        /// 鼠标松开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintCtrl_MouseUp(object sender, MouseEventArgs e)
        {
            if (_Dragging && _CurrBorder != RESIZE_BORDER.RB_NONE)
            {
                foreach (Control ctrl in Panel.SelectedCtrls)
                {
                    SetNewSizeAndLocation(e, ctrl);
                    Panel.Rects.Remove(ctrl);
                    //ctrl.Invalidate();
                }
                Panel.InvalidateW();//刷新 
            }

            _Dragging = false;//停止拖拽
            this.Capture = false;
            if (e.Button == MouseButtons.Right) Panel.Page.UCPrint.ShowCmsCtrl(this.PointToScreen(e.Location));
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintCtrl_MouseMove(object sender, MouseEventArgs e)
        {
            Hit_Test(e.X, e.Y);//检测鼠标状态
            if (_Dragging && _CurrBorder != RESIZE_BORDER.RB_NONE)
            {
                ShowSlideBorder(e, Panel.SelectedCtrls);//绘制移动边框  
            }
            //Update by HeXiaoMing 2010-12-09 9:15
            //Reason : 修正鼠标拖动边框时鼠标图标变成光标bug
            this.Cursor = Cursor.Current;
            //
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        private void SetEvent()
        {
            if (Selected)
            {
                this.MouseDown += new MouseEventHandler(PrintCtrl_MouseDown);
                this.MouseUp += new MouseEventHandler(PrintCtrl_MouseUp);
                this.MouseMove += new MouseEventHandler(PrintCtrl_MouseMove);
            }
            else
            {
                this.MouseDown -= new MouseEventHandler(PrintCtrl_MouseDown);
                this.MouseUp -= new MouseEventHandler(PrintCtrl_MouseUp);
                this.MouseMove -= new MouseEventHandler(PrintCtrl_MouseMove);
            }
        }

        /// <summary>
        /// 宽度即将改变事件
        /// </summary>
        /// <param name="oldWidth"></param>
        /// <param name="newWidth"></param>
        public virtual void WidthChanging(int oldWidth, int newWidth) { }

        /// <summary>
        /// 宽度改变事件
        /// </summary>
        /// <param name="oldWidth"></param>
        /// <param name="newWidth"></param>
        public virtual void WidthChanged(int oldWidth, int newWidth)
        {
            RePaint();//重绘
        }


        /// <summary>
        /// 计算容器内元素大小
        /// </summary>
        public virtual void CalcSize()
        {
            using (Graphics g = base.CreateGraphics())
            {
                var size1 = g.MeasureString(Text, TextFont == null ? Font : TextFont);//文字大小

                Size temp = _TextSize;
                //控件头大小
                _TextSize = new Size((size1.Width + 0.5f).GetNearInt(),
                    string.IsNullOrEmpty(Text) ? 0 : (size1.Height + 0.5f).GetNearInt());

                OffsetWidth = _TextSize.Width - temp.Width;
                OffsetHeight = _TextSize.Height - temp.Height;
            }
        }
        /// <summary>
        /// 重绘事件
        /// </summary>
        public virtual void RePaint()
        {
            CalcSize();//计算文本大小
            SetSize();//设置控件大小       
            AdjustX();
            Invalidate();
        }
        /// <summary>
        /// 设置大小
        /// </summary>
        public virtual void SetSize()
        {
            var size = new Size(_TextSize.Width + Sqare.Width + Padding.Right,
                     _TextSize.Height + Sqare.Height + Padding.Bottom + TextVSpace);

            base.Size = size;
        }

        private void AdjustX()
        {
            int w1 = base.Location.X + base.Width;
            int w2 = Panel.Width - Panel.CtrlMargin.Right;
            if (w1 > w2)
            {
                int left = base.Left - (w1 - w2);
                if (left < Panel.CtrlMargin.Left) left = Panel.CtrlMargin.Left;
                base.Left = left;
            }
        }

        #endregion

    }
}