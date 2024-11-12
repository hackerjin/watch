using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace Skyray.Controls
{
    /// <summary>
    /// 修改至XFP 2.0,绘图控件
    /// </summary>
    public partial class SmartGraphic : Control
    {
        public const int MaxSpecCount = 7; ///<最多加载谱的个数
        private Color ruleColor; ///<尺子颜色
        private int ruleWidth;   ///<尺子的宽度

        public int RuleWidth
        {
            get { return ruleWidth; }
            set
            {
                if (value >= 0)
                {
                    ruleWidth = value;
                }
            }
        }
        public bool IsExistSpec { get; set; }

        public int maxChannel;  ///<最大的道址   
        private int maxValue;///<最大的值 
        private int xOff;        ///<道址平移的量
        private int xZoom;       ///<道址放大倍数
        private double xCoeff;   ///<X方向道与像素的比例
        private double yCoeff;   ///<Y方向道与像素的比例
        private int currChann;   ///<当前道
        private int currValue;   ///<当前道的值

        public bool DrawMoveLine { get; set; }
        public bool DrawShadow { get; set; }
        private bool mouseInShadowRect = false;
        private int _SectionCount = 4;
        // private Point mouseDownPoint;
        private int mouseDownX = 0;
        private Rectangle shadowRect;

        public Rectangle ShadowRect
        {
            get
            {
                return shadowRect;
            }
            set
            {
                shadowRect = value;
            }
        }

        Image img;     ///<画板
        private GraphicData[] SpecDatas = new GraphicData[MaxSpecCount];///<谱数据结构数组

        public delegate void DrawEvent(Graphics g);

        [Category("Appearance"), Description("控件画图之前发生"), Browsable(true)]
        public event DrawEvent midDrawGraphic;

        public event EventHandler<ShadowMovingEventArgs> ShadowMoved;

        public void ChangeShadowRect(double xMin, double xMax) //yuzhao20150624:X轴坐标变化事件,使得大图的X变化实时的映射到小图上，包括缩放和移动
        {
            shadowRect.X = GetX(xMin);
            shadowRect.Width = GetX(xMax) - shadowRect.X;
            this.Refresh();
        }

        public SmartGraphic()
        {
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint,
                true);
            // this.DoubleBuffered = true;设置双缓冲报错
            ruleColor = Color.Silver;
            DrawShadow = true;
            //mouseDownPoint = Point.Empty;
            shadowRect = new Rectangle(0, 0, 100, this.Height);
            ruleWidth = 0;
            xZoom = 1;
            SpecDatas[0].YZoom = 1;
            //maxChannel = 2048;
            maxValue = 1000;
            IsExistSpec = false;
            this.SizeChanged += new EventHandler(SmartGraphic_SizeChanged);
            this.MouseLeave += new System.EventHandler(this.SpecGraphic_MouseLeave);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SpecGraphic_Paint);
            if (DrawMoveLine)
                this.MouseMove += new MouseEventHandler(SpecGraphic_MouseMove);
            if (DrawShadow)
            {
                this.MouseDown += new MouseEventHandler(SmartGraphic_MouseDown);
                this.MouseUp += new MouseEventHandler(SmartGraphic_MouseUp);
                this.MouseMove += new MouseEventHandler(DragShadowRect);
            }
        }

        void SmartGraphic_SizeChanged(object sender, EventArgs e)
        {
            shadowRect = new Rectangle(0, 0, this.Width / _SectionCount, this.Height);
        }

        void DragShadowRect(object sender, MouseEventArgs e)
        {
            if (mouseInShadowRect)
            {
                int offSetX = e.X - mouseDownX;//偏移量
                int newX = shadowRect.X + offSetX;
                if (newX < 0)
                    newX = 0;
                if (newX + shadowRect.Width > this.Width)
                    newX = this.Width - shadowRect.Width;
                if (shadowRect.X != newX)
                {
                    Rectangle rect = Rectangle.Empty;
                    if (offSetX > 0)
                    {
                        rect = new Rectangle(shadowRect.X, 0,
                            shadowRect.Width + offSetX,
                            shadowRect.Height);
                    }
                    else
                    {
                        rect = new Rectangle(shadowRect.X + offSetX, 0,
                            shadowRect.Width - offSetX,
                            shadowRect.Height);
                    }                   
                    shadowRect.X = newX;
                    this.Invalidate(rect);
                }
                mouseDownX = e.X;
            }
        }

        void SmartGraphic_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseInShadowRect)             //yuzhao_20150604:防止误操作
                InitialChartScale();
            mouseInShadowRect = false;
            //if (ShadowMoved != null)
            //{
            //    var left = GetChann(shadowRect.X);
            //    var right = GetChann(shadowRect.X + shadowRect.Width);
            //    ShadowMoved(this, new ShadowMovingEventArgs
            //    {
            //        MinChannel = left,
            //        MaxChannel = right
            //    });
            //}
        }

        public void InitialChartScale()       
        {
            if (ShadowMoved != null)
            {
                if (xCoeff == 0)
                    xCoeff = (maxChannel * 1.0) / (xZoom * (Width - ruleWidth));
                var left = GetChann(shadowRect.X);
                var right = GetChann(shadowRect.X + shadowRect.Width);
                ShadowMoved(this, new ShadowMovingEventArgs
                {
                    MinChannel = left,
                    MaxChannel = right
                });
            }
        }

        void SmartGraphic_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownX = e.X;
            if (shadowRect.Contains(e.Location))
            {
                mouseInShadowRect = true;
            }
        }

        public void AddSpec(int index, int[] data, Color color)
        {
            if (index > 0 && index < MaxSpecCount)
            {
                SpecDatas[index] = new GraphicData(data, color);
                SpecDatas[index].YZoom = 1;// 对比谱和实谱打开后同步 --陈春花              
                Invalidate();
            }
            else if (index == 0)
            {
                double dblYZoom = 1;
                if (SpecDatas != null && SpecDatas.Length > index) dblYZoom = SpecDatas[index].YZoom;

                SpecDatas[index] = new GraphicData(data, color);
                SpecDatas[index].YZoom = dblYZoom;
                maxValue = GetMaxValue();

                IsExistSpec = true;
                Invalidate();
            }
        }

        public void RemoveSpec(int index)
        {
            if (index >= 0 && index < MaxSpecCount)
            {
                SpecDatas[index].Enabled = false;
                Invalidate();
            }
        }

        /// <summary>
        /// x转换为道址
        /// </summary>
        /// <param name="pixX">像素的横坐标</param>
        /// <returns>道址</returns>
        public int GetChann(int pixX)
        {
            return (int)(xCoeff * (pixX - ruleWidth)) + xOff;
        }

        public int GetX(double channel)
        {
            return (int)((channel - xOff) / xCoeff) + ruleWidth;
        }

        /// <summary>
        /// 获取最大计数值
        /// </summary>
        /// <returns>最大计数值</returns>
        private int GetMaxValue()
        {
            int maxValue = 0;
            for (int i = 0; i < maxChannel; i++)
            {
                if (SpecDatas[0].Data != null && SpecDatas[0].Data.Length > 0 && i < SpecDatas[0].Data.Length && SpecDatas[0].Data[i] > maxValue)
                {
                    maxValue = SpecDatas[0].Data[i];
                }
            }
            //最大值修改成最大值的1.2倍
            //maxValue = (int)Math.Round((maxValue * 1.1));
            //if (maxValue < 1000)
            //{
            //    maxValue = 1000;
            //}
            maxValue = maxValue * 1.2>100?(int)Math.Round(maxValue * 1.2):100;
            return maxValue;
        }

        /// <summary>
        /// 通道转化为像素的横坐标
        /// </summary>
        /// <param name="chann">道址</param>
        /// <returns>像素的横坐标</returns>
        public int GetXPix(int chann)
        {
            //return (int)Math.Round((chann - xOff) / xCoeff + ruleWidth);
            return (int)Math.Ceiling((chann - xOff) / xCoeff + ruleWidth);
        }

        /// <summary>
        /// 计数值转化为像素纵坐标
        /// </summary>
        /// <param name="value">计数值</param>
        /// <returns>像素纵坐标</returns>
        public int GetYPix(int value)
        {
            return (int)Math.Round(Height - ruleWidth - value / yCoeff);
        }

        /// <summary>
        /// 像素纵坐标转化为计数值
        /// </summary>
        /// <param name="pixY">像素纵坐标</param>
        /// <returns>计数值</returns>
        public int GetValue(int pixY)
        {
            return (int)Math.Round((Height - ruleWidth - pixY) * yCoeff);
        }

        /// <summary>
        /// 把值划分为count份作为单位大小
        /// </summary>
        /// <param name="Count">份数</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        private int FindUnitSize(int count, int maxValue, int minValue)
        {
            if (count > (maxValue - minValue))
            {
                count = maxValue - minValue;
            }
            int unit = (int)(maxValue - minValue) / count;
            int findUnit;
            for (int i = 1; i < 6; ++i)
            {
                findUnit = (int)Math.Pow(10, i);
                if (unit < findUnit)
                {
                    unit = findUnit;
                    break;
                }
                else if (unit < 2 * findUnit)
                {
                    unit = 2 * findUnit;
                    break;
                }
                else if (unit < 5 * findUnit)
                {
                    unit = 5 * findUnit;
                    break;
                }
            }
            return unit;
        }

        /// <summary>
        /// 图形左右平移
        /// </summary>
        /// <param name="step">目标步长</param>
        public void XScroll(ref int step)
        {
            int maxOff = (int)(maxChannel * (xZoom - 1) * 1.0 / xZoom);
            int minOff = 0;
            if (step < minOff)
            {
                step = minOff;
            }
            else if (step > maxOff)
            {
                step = maxOff;
            }
            else
            {
                xOff = step;
            }
        }

        /// <summary>
        /// X轴方向放大倍数
        /// </summary>
        public int XZoom
        {
            get { return xZoom; }
            set
            {
                if (value > 1)
                {
                    xZoom = value;
                }
                else
                {
                    xZoom = 1;
                }
                Invalidate();
            }
        }
        /// <summary>
        /// 设置y轴方向放大倍数
        /// </summary>
        /// <param name="index">谱数据的索引</param>
        /// <param name="yZoom">放大倍数</param>
        public void SetYZoom(int index, double yZoom)
        {
            if ((yZoom > 0) && (index >= 0) && (index < MaxSpecCount))
            {
                SpecDatas[index].YZoom = yZoom;
                Invalidate();
            }
        }

        /// <summary>
        /// 获取y轴方向放大倍数
        /// </summary>
        /// <param name="index">谱数据的索引</param>
        public double GetYZoom(int index)
        {
            if ((index >= 0) && (index < MaxSpecCount))
            {
                return SpecDatas[index].YZoom;
            }
            else
            {
                return 0.0;
            }
        }

        /// <summary>
        /// 标尺颜色
        /// </summary>
        public Color RuleColor
        {
            get { return ruleColor; }
            set
            {
                ruleColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 鼠标所在的道
        /// </summary>
        [Browsable(false)]
        public int CurrentChann
        {
            get { return currChann; }
        }

        /// <summary>
        /// 鼠标所在道的值；
        /// </summary>
        [Browsable(false)]
        public int CurrentValue
        {
            get { return currValue; }
        }

        public int MaxChannel
        {
            get { return maxChannel; }
            set
            {
                if ((value > 0) && (maxChannel != value))
                {
                    maxChannel = value;
                    for (int i = 0; i < MaxSpecCount; i++)
                    {
                        SpecDatas[i].Data = new int[value];
                        SpecDatas[i].BorderPoint = new Point[value + 2];
                    }
                }
            }
        }

        private void SpecGraphic_Paint(object sender, PaintEventArgs e)
        {
            using (Graphics g = e.Graphics)
            {
                DrawSpec(g);
                if (DrawShadow) DrawShadowRect(g);
            }
        }
        private void DrawShadowRect(Graphics g)
        {
            Pen pen = new Pen(Color.FromArgb(100, 155, 205, 108));
            g.FillRectangle(pen.Brush, shadowRect);
        }
        private void DrawSpec(Graphics g)
        {
            xCoeff = (maxChannel * 1.0) / (xZoom * (Width - ruleWidth));
            yCoeff = (maxValue * 1.0) / (SpecDatas[0].YZoom * (Height - ruleWidth));
            DrawSpec(g, true);//实谱
            if (ruleWidth != 0) DrawXRule(g);
            if (midDrawGraphic != null) midDrawGraphic(g);
            DrawSpec(g, false);//对比谱
            if (ruleWidth != 0) DrawYRule(g);
        }

        /// <summary>
        /// 抓图
        /// </summary>
        /// <param name="size">图片的大小</param>
        public Image GrabGraphic(Size size)
        {
            Bitmap bmp = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.ScaleTransform(1.0f * size.Width / Width, 1.0f * size.Height / Height);
            DrawSpec(g, true);
            if (midDrawGraphic != null)
            {
                midDrawGraphic(g);
            }
            DrawSpec(g, false);
            DrawXRule(g);
            DrawYRule(g);
            return img = (Image)bmp;
        }
        public class ShadowMovingEventArgs : EventArgs
        {
            public double MinChannel { get; set; }
            public double MaxChannel { get; set; }
        }
    }
}
