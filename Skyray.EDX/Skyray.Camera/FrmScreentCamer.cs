using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge;
using AForge.Controls;
using AForge.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing;

using Skyray.EDX.Common;
namespace Skyray.Camera
{
    public partial class FrmScreentCamer : Form
    {
        private int fociCount;  // 焦斑数
        private int fociIndex;  // 焦点序号
        private double viewWidth;   // 画面宽度
        private double viewHeight;  // 画面高度
        private double ruleUnit;    // 刻度尺单位
        private double fociX;   // 焦点X坐标
        private double fociY;   // 焦点Y坐标
        private bool mouseModifyFoci;   // 是否允许鼠标改动焦点位置
        private double moveRateX;   // X方向移动距离
        private double moveRateY;   // Y方向移动距离
        public int FormatWidth { set; get; }
        public int FormatHight { set; get; }


        protected double XStartRate = 0.15;//从左往右
        protected double XWidthRate = 0.6;
        protected double YStartRate = 0.5;//从上往下
        protected double YHeightRate = 0.4;//从上往下

        private int pointRowCount;
        private int pointColCount;
        private double pointRowDistance;
        private double pointColDistance;


        public float m_XStartRate = 0;
        public float m_XWidthRate = 1;
        public float m_YStartRate = 0;
        public float m_YHeightRate = 1;

         /// <summary>
        /// 焦斑序号（已做简单的边界检查，最小可为-1，最大可为当前焦斑数减1）
        /// </summary>
        public int FociIndex
        {
            get
            {
                return fociIndex;
            }
            set
            {
                if (value < -1)
                {
                    fociIndex = -1;
                }
                else if (value < fociCount)
                {
                    fociIndex = value;
                }
                else
                {
                    fociIndex = fociCount - 1;
                }
                UpdateOverlay();
            }
        }


        /// <summary>
        /// 焦斑的自定义x坐标（已做简单的边界检查，不接受负数，最小限制为0）
        /// </summary>
        public double FociX
        {
            get
            {
                return fociX;
            }
            set
            {
                //fociX = (value < 10) ? 10 : value;
                fociX = (value < 0) ? 0 : value;
            }
        }
        /// <summary>
        /// 焦斑的自定义y坐标（已做简单的边界检查，不接受负数，最小限制为0）
        /// </summary>
        public double FociY
        {
            get
            {
                return fociY;
            }
            set
            {
                fociY = (value < 0) ? 0 : value;
            }
        }
        /// <summary>
        /// 画面自定义宽度（已做简单的边界检查，不接受负数，最小限制为0）
        /// </summary>
        public double ViewWidth
        {
            get
            {
                return viewWidth;
            }
            set
            {
                viewWidth = (value < 5) ? 5 : value;
            }
        }
        /// <summary>
        /// 画面自定义高度（已做简单的边界检查，不接受负数，最小限制为0）
        /// </summary>
        public double ViewHeight
        {
            get
            {
                return viewHeight;
            }
            set
            {
                viewHeight = (value < 5) ? 5 : value;
            }
        }
        /// <summary>
        /// 刻度尺自定义单位长度（已做简单的边界检查，不接受负数，最小限制为0）
        /// </summary>
        public double RuleUnit
        {
            get
            {
                return ruleUnit;
            }
            set
            {
                ruleUnit = (value < 0) ? 0 : value;
               // ruleUnit = (value < 0) ? 0 : value;
            }
        }

   

        /// <summary>
        /// 指示能否通过鼠标改变焦斑位置
        /// </summary>
        public bool MouseModifyFoci
        {
            get
            {
                return mouseModifyFoci;
            }
            set
            {
                mouseModifyFoci = value;
            }
        }

        /// <summary>
        /// X方向移动比例
        /// </summary>
        public double MoveRateX
        {
            get
            {
                return moveRateX;
            }
            set
            {
                moveRateX = value;
               // tsmiMoveRateValueX.Text = value.ToString();
            }
        }
        /// <summary>
        /// Y方向移动比例
        /// </summary>
        public double MoveRateY
        {
            get
            {
                return moveRateY;
            }
            set
            {
                moveRateY = value;
             //   tsmiMoveRateValueY.Text = value.ToString();
            }
        }

  
        public float FontSizeCoef = 1.0f;
        /// <summary>
        /// 焦斑形态
        /// </summary>
        public Skyray.Camera.SkyrayCamera.Foci[] Focis;


        public FrmScreentCamer()
        {
            InitializeComponent();
        }

        public IVideoSource videoSource;

        

        private void FrmScreentCamer_Load(object sender, EventArgs e)
        {
            this.skyrayCamera1.VideoSource = videoSource;
           
            if (this.skyrayCamera1.Opened)
                this.skyrayCamera1.refreshCameraInfo("");
            FlipMethod();
            
        }


        public void FlipMethod()
        {
            this.skyrayCamera1.NewFrame -= new VideoSourcePlayer.NewFrameHandler(videoSourcePlayer_NewFrame);
            this.skyrayCamera1.NewFrame += new VideoSourcePlayer.NewFrameHandler(videoSourcePlayer_NewFrame);
        }


        object drawbmp = new object();
        Bitmap newBitmap = null;
        private void videoSourcePlayer_NewFrame(object sender, ref Bitmap image)
        {
            lock (drawbmp)
            {
                if (image != null)
                {
                    try
                    {

                        //if (!this.bRefreshFistFrame) return;

                        //if (m_OverlayBmp == null) UpdateOverlay();
                        image.RotateFlip(RotateFlipType.RotateNoneFlipNone);

                        //if (m_OverlayBmp != null)
                        //{
                        Graphics g = Graphics.FromImage(image);
                        // g.Clear(this.BackColor);
                        //Bitmap bitmap = CreateRealBitmap(image.Width, image.Height);
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        if (m_XWidthRate < 1 || m_YHeightRate < 1)
                        {
                            if (newBitmap == null) newBitmap = new Bitmap(image.Width, image.Height);
                            Graphics gTemp = Graphics.FromImage(newBitmap);
                            Rectangle rectSource = new Rectangle((int)(image.Width * m_XStartRate), (int)(image.Height * m_YStartRate), (int)(image.Width * m_XWidthRate), (int)(image.Height * m_YHeightRate));
                            //Rectangle rectDest = new Rectangle(0, 0, image.Width, image.Height);
                            Rectangle rectDest = new Rectangle(0, 0, newBitmap.Width, newBitmap.Height);
                            gTemp.DrawImage(image, rectDest, rectSource, GraphicsUnit.Pixel);
                            g.DrawImage(newBitmap, 0, 0, image.Width, image.Height);
                            gTemp.Dispose();
                        }
                        try
                        {
                            if (m_OverlayBmp == null)
                                m_OverlayBmp = CreateRealBitmap(image.Width, image.Height);
                        }
                        catch
                        {
                            if (m_OverlayBmp != null)
                            {
                                m_OverlayBmp.Dispose();
                                m_OverlayBmp = null;
                            }
                        }
                        if (m_OverlayBmp != null)
                            g.DrawImage(m_OverlayBmp, 0, 0, image.Width, image.Height);
                        g.Dispose();
                        // }
                        //bitmap.Dispose();
                    }
                    catch
                    { }
                }
            }
        }


        public Bitmap m_OverlayBmp = null;

        public void UpdateOverlay()
        {
            lock (drawbmp)
            {
                if (m_OverlayBmp != null)
                {
                    m_OverlayBmp.Dispose();
                    m_OverlayBmp = null;
                }
                //try
                //{
                //    m_OverlayBmp = CreateRealBitmap(this.Width, this.Height);
                //}
                //catch
                //{
                //    if (m_OverlayBmp != null)
                //    {
                //        m_OverlayBmp.Dispose();
                //        m_OverlayBmp = null;
                //    }
                //}
            }
        }
        public Bitmap CreateRealBitmap(int bmpWidth, int bmpHeight)
        {
            if (ruleUnit <= 0)  return null;
           /// refreshCameraInfo("");
            Bitmap bmpToProcess = new Bitmap(bmpWidth, bmpHeight);

            #region 画自定义图层
            using (Graphics g = Graphics.FromImage(bmpToProcess))
            {
                // Pen pen = Pens.Red;
                Pen pen = new Pen(Color.Red, 1 * bmpWidth / 320);
                Font font = new Font("Verdana", (int)(10 * 1* bmpWidth / 320));//
                //const int lineSign = 3;
                int lineSign = 3 * bmpWidth / 320;
                // 实际画面和自定义画面大小之比，用于将自定义参数换算成实际参数
                double rateWidth = (viewWidth == 0) ? 1 : (bmpWidth * 1.0 / viewWidth);
                double rateHeight = (viewHeight == 0) ? 1 : (bmpHeight * 1.0 / viewHeight);

                // 刻度尺实际的单位长度，通过上面的比值换算求得
                int bmpXUnit = Convert.ToInt32(ruleUnit * rateWidth);
                int bmpYUnit = Convert.ToInt32(ruleUnit * rateHeight);

                // 翻转的工作被放在绘图结束之后
                int bmpFociX = Convert.ToInt32(fociX * rateWidth);
                int bmpFociY = Convert.ToInt32((fociY) * rateHeight);

                /****************开始绘图*****************/

                //画比例尺
                g.DrawLine(pen, new System.Drawing.Point(3, bmpHeight - 8), new System.Drawing.Point(3, bmpHeight - 4));
                g.DrawLine(pen, new System.Drawing.Point(3, bmpHeight - 6), new System.Drawing.Point(3 + bmpXUnit, bmpHeight - 6));
                g.DrawLine(pen, new System.Drawing.Point(3 + bmpXUnit, bmpHeight - 8), new System.Drawing.Point(3 + bmpXUnit, bmpHeight - 4));
                g.DrawString(Convert.ToString(ruleUnit) + "mm", font, new SolidBrush(Color.Tomato), 3, bmpHeight - 11 - font.Size);

                // 画横轴

                g.DrawLine(pen, new System.Drawing.Point(0, bmpFociY), new System.Drawing.Point(bmpWidth, bmpFociY));
                //g.DrawLine(pen, new PointF(0, (Single)(98 * 1.0)), new PointF((Single)(bmpWidth * 1.0), (Single)(98 * 1.0)));
                // 画纵轴
                g.DrawLine(pen, new System.Drawing.Point(bmpFociX, 0), new System.Drawing.Point(bmpFociX, bmpHeight));

                // 从焦点起步，向左画横轴刻度
                int ruleScaleLinePosX = bmpFociX;   // 刻度线初始位置
                double ruleScaleLineLength = 0;
                g.DrawString(Convert.ToString(0), font, new SolidBrush(Color.Tomato), bmpFociX + 2, bmpFociY + 2);
                while (ruleScaleLinePosX > 0)   // 一直平移到画面最左端
                {
                    // 每次刻度线位置向左平移一个单位长度
                    ruleScaleLinePosX -= bmpXUnit;

                    ruleScaleLineLength += RuleUnit;
                    g.DrawString("-" + Convert.ToString(ruleScaleLineLength), font, new SolidBrush(Color.Tomato), ruleScaleLinePosX - 8, bmpFociY + 2);


                    // 垂直画一条短线表示刻度
                    g.DrawLine(pen, new System.Drawing.Point(ruleScaleLinePosX, bmpFociY + lineSign), new System.Drawing.Point(ruleScaleLinePosX, bmpFociY - lineSign));
                    //g.DrawLine(pen,new PointF(
                }

                // 从焦点起步，向右画横轴刻度
                ruleScaleLinePosX = bmpFociX;   // 刻度线初始位置
                ruleScaleLineLength = 0;
                while (ruleScaleLinePosX < bmpWidth)   // 一直平移到画面最右端
                {
                    // 每次刻度线位置向右平移一个单位长度
                    ruleScaleLinePosX += bmpXUnit;
                    ruleScaleLineLength += RuleUnit;// bmpXUnit;
                    g.DrawString(Convert.ToString(ruleScaleLineLength), font, new SolidBrush(Color.Tomato), ruleScaleLinePosX - 4, bmpFociY + 2);

                    // 垂直画一条短线表示刻度
                    g.DrawLine(pen, new System.Drawing.Point(ruleScaleLinePosX, bmpFociY + lineSign), new System.Drawing.Point(ruleScaleLinePosX, bmpFociY - lineSign));
                }

                // 从焦点起步，向上画纵轴刻度
                int ruleScaleLinePosY = bmpFociY;   // 刻度线初始位置
                ruleScaleLineLength = 0;
                while (ruleScaleLinePosY > 0)   // 一直平移到画面最上端
                {
                    // 每次刻度线位置向上平移一个单位长度
                    ruleScaleLinePosY -= bmpYUnit;
                    ruleScaleLineLength += RuleUnit;
                    g.DrawString(Convert.ToString(ruleScaleLineLength), font, new SolidBrush(Color.Tomato), bmpFociX + 2, ruleScaleLinePosY - 8);

                    // 垂直画一条短线表示刻度
                    g.DrawLine(pen, new System.Drawing.Point(bmpFociX + lineSign, ruleScaleLinePosY), new System.Drawing.Point(bmpFociX - lineSign, ruleScaleLinePosY));

                }
                // 从焦点起步，向下画纵轴刻度
                ruleScaleLinePosY = bmpFociY;   // 刻度线初始位置
                ruleScaleLineLength = 0;
                while (ruleScaleLinePosY < bmpHeight)   // 一直平移到画面最下端
                {
                    // 每次刻度线位置向下平移一个单位长度
                    ruleScaleLinePosY += bmpYUnit;
                    ruleScaleLineLength += RuleUnit;
                    g.DrawString("-" + Convert.ToString(ruleScaleLineLength), font, new SolidBrush(Color.Tomato), bmpFociX + 2, ruleScaleLinePosY - 8);
                    // 垂直画一条短线表示刻度
                    g.DrawLine(pen, new System.Drawing.Point(bmpFociX + lineSign, ruleScaleLinePosY), new System.Drawing.Point(bmpFociX - lineSign, ruleScaleLinePosY));
                }

                //画焦斑
                if (fociIndex >= 0)
                {
                    int halfFociWidth = Convert.ToInt32(Focis[fociIndex].Width * rateWidth / 2);
                    int halfFociHeight = Convert.ToInt32(Focis[fociIndex].Height * rateHeight / 2);
                    Rectangle rect = new Rectangle(bmpFociX - halfFociWidth, bmpFociY - halfFociHeight,
                                                 halfFociWidth * 2, halfFociHeight * 2);
                    switch (Focis[fociIndex].Shape)
                    {   //画椭圆
                        case  SkyrayCamera.FociShape.Ellipse:
                            g.DrawEllipse(pen, rect);
                            break;
                        //画矩形
                        case SkyrayCamera.FociShape.Rectangle:
                            g.DrawRectangle(pen, rect);
                            break;
                    }
                }

               

            }
            #endregion
            // 翻转
            //bmpToProcess.RotateFlip(RotateFlipType.RotateNoneFlipY);
            // bmpToProcess.RotateFlip(RotateFlipType.Rotate270FlipY);
            return bmpToProcess;
        }

        //private void writeLine()
        //{
        //    using (Graphics g = m_pic_ShowImage.CreateGraphics())
        //    {
        //        try
        //        {
        //            if (m_OverlayBmp == null)
        //                m_OverlayBmp = CreateRealBitmap(m_pic_ShowImage.Width, m_pic_ShowImage.Height);
        //        }
        //        catch
        //        {
        //            if (m_OverlayBmp != null)
        //            {
        //                m_OverlayBmp.Dispose();
        //                m_OverlayBmp = null;
        //            }
        //        }
        //        if (m_OverlayBmp != null)
        //            g.DrawImage(m_OverlayBmp, 0, 0, m_pic_ShowImage.Width, m_pic_ShowImage.Height);
        //        //Pen pen = new Pen(Color.White, 5);
        //        //Font font = new Font("Verdana", (int)(10));
        //        //g.DrawLine(pen, new Point(0, m_nHeigh / 2), new Point(m_nWidth, m_nHeigh / 2));
        //    }
        //}

    }
}
