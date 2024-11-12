using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Xml;
using System.IO;
using System.Drawing.Imaging;
using System.Reflection;
using AForge.Video.DirectShow;
using AForge.Controls;
using Skyray.EDX.Common;

namespace Skyray.Camera
{

    public partial class SkyrayCamera : AForge.Controls.VideoSourcePlayer
    {
        public delegate void ReceiveMotorback();
        public ReceiveMotorback motorBack;
        public FilterInfoCollection videoDevices;
        public VideoCaptureDevice videoSourceDevice;
        public int selectedDeviceIndex = 0;
        public RotateFlipType videoRotateFlip = RotateFlipType.RotateNoneFlipNone;
        private bool IsGain = false;
        //private Size VedioFrameSize ;
        public string currentMultiName = string.Empty;
        public string curMultiName = string.Empty;

        private string CameraFileName = Application.StartupPath + "\\Camera.xml";
        public string PointPath = Application.StartupPath + "\\MultiPoints\\";
        public string MatrixPath = Application.StartupPath + "\\Matrix\\";

        public static int MaxResolution = 2000000;

        //private string strTestInformation=string.Empty;
        public event EventHandler FormatChanged;

       
        Pen definePen = new Pen(Color.Yellow, 0.1f);

        #region 内嵌的自定义结构

        public enum CameraMode
        {
            Move, Coee, lonely, Multiple, Cell, dotMatrix, matrixDot, dotDot
        }

        public float FontSizeCoef = 1.0f;
        /// <summary>
        /// 焦斑结构
        /// </summary>
        public struct Foci
        {
            /// <summary>
            /// 焦斑形状
            /// </summary>
            public FociShape Shape;
            /// <summary>
            /// 焦斑宽度
            /// </summary>
            public double Width;
            /// <summary>
            /// 焦斑高度
            /// </summary>
            public double Height;

            /// <summary>
            /// 焦斑在X轴位置
            /// </summary>
            public double FociX;

            /// <summary>
            /// 焦斑在Y轴位置
            /// </summary>
            public double FociY;
        }

        /// <summary>
        /// 焦斑的形状
        /// </summary>
        public enum FociShape
        {
            /// <summary>
            /// 矩形
            /// </summary>
            Rectangle,
            /// <summary>
            /// 椭圆
            /// </summary>
            Ellipse
        }

        /// <summary>
        /// 枚举：视频控件大小
        /// </summary>
        public enum CaptureFormat
        {
            /// <summary>
            /// 最小：160*120
            /// </summary>
            Smallest = 160 * 120,
            /// <summary>
            /// 较小：176*144
            /// </summary>
            Smaller = 240 * 180,
            /// <summary>
            /// 适中：320*240
            /// </summary>
            Middle = 320 * 240,
            /// <summary>
            /// 较大：352*288
            /// </summary>
            Larger = 480 * 360,
            /// <summary>
            /// 最大：640*480
            /// </summary>
            Largest = 640 * 480
        }

        public bool Opened
        {
            get { return this.VideoSource != null; }
        }

        #endregion

        #region 常量

        private const int MaxFociCount = 10; // 焦斑数上限

        #endregion

        #region 私有字段
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

        //图像上焦点中心位置
        private int fociBmpX;
        public int FociBmpX
        {
            get { return fociBmpX; }
            set { fociBmpX = value; }
        }
        private int fociBmpY;
        public int FociBmpY
        {
            get { return fociBmpY; }
            set { fociBmpY = value; }
        }

        private int pointRowCount;
        private int pointColCount;
        private double pointRowDistance;
        private double pointColDistance;

        


        public int VideoIndex
        {
            get { return selectedDeviceIndex; }
            set
            {
                if (selectedDeviceIndex != value)
                {
                    selectedDeviceIndex = value;
                    //System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    //string path = Application.StartupPath + "\\Camera.xml";
                    //if (System.IO.File.Exists(path))
                    //{
                    //    doc.Load(path);
                    //    System.Xml.XmlNode rootNode = doc.SelectSingleNode("Camera/CaptureIndex");
                    //    if (rootNode != null)
                    //    {
                    //        rootNode.InnerText = selectedDeviceIndex.ToString();
                    //        doc.Save(path);
                    //    }
                    //    else
                    //    {
                    //        XmlNode rootNode_application = doc.SelectSingleNode("Camera");
                    //        if (rootNode_application != null)
                    //        {
                    //            XmlElement newXmlNode_tag = doc.CreateElement("CaptureIndex");
                    //            newXmlNode_tag.InnerText = selectedDeviceIndex.ToString(); 
                    //            rootNode_application.AppendChild(newXmlNode_tag);
                    //            doc.Save(path);
                    //        }
                    //    }
                    //}
                    //this.Open();
                }

            }
        }

        private double camerCurrent;
        public double CamerCurrent
        {
            get { return camerCurrent; }
            set { camerCurrent = value; }
        }


        // 默认视频宽高
        private int capVideoWidth = 640;
        private int capVideoHeight = 480;


        public int CapVideoWidth
        {
            get
            {
                return capVideoWidth;
            }
            set
            {
                capVideoWidth = value;
            }
        }

        public int CapVideoHeight
        {
            get
            {
                return capVideoHeight;
            }

            set
            {
                capVideoHeight = value;
            }
        }

        // 默认视频宽高
        private double camerCurrentX = 0.6;
        private double camerCurrentY = 0.1;


        public double CamerCurrentX
        {
            get
            {
                return camerCurrentX;
            }
            set
            {
                camerCurrentX = value;
            }
        }

        public double CamerCurrentY
        {
            get
            {
                return camerCurrentY;
            }
            set
            {
                camerCurrentY = value;
            }
        }

        private int focusSize = 2;
        public int FocusSize
        {
            get
            {
                return focusSize;
            }
            set
            {
                focusSize = value;
            }

        }


        private bool isShowCenter = false;
        public bool IsShowCenter
        {
            get
            {
                return isShowCenter;
            }
            set
            {
                isShowCenter = value;
                UpdateOverlay();
            }
        }


        private bool isMotorMoveToFirst = false;
        public bool IsMotorMoveToFirst
        {
            get { return isMotorMoveToFirst; }
            set
            {
                isMotorMoveToFirst = value;
            }
        }


        private System.Timers.Timer timer;

        public static Color curColor = Color.Red;
        //private string s3000Mode="";
        #endregion


        #region 构造器

        /// <summary>
        /// 构造器
        /// </summary>
        public SkyrayCamera()
        {
            InitializeComponent();
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            // 初始化
            Format = CaptureFormat.Largest;
            Focis = new Foci[MaxFociCount];
            fociIndex = 0;
            this.Mode = SkyrayCamera.CameraMode.Move;
            fociCount = 8;
            //s3000Mode = ReportTemplateHelper.LoadSpecifiedValue("EDX3000", "Is3000D");
            //if (s3000Mode == "1")
            //{
            //    tsmiSetMoveRate.Visible = true;
            //}
            //viewWidth = 80;
            //viewHeight = 60;
            //capVideoWidth = this.Width;
            //capVideoHeight = this.Height;
            //ruleUnit = 5;
            //fociX = 40;
            //fociY = 30;
            //moveRateX = 24;
            //moveRateY = 24;
            //for (int i = 0; i < MaxFociCount; i++)
            //{
            //    Focis[i].Width = 10;
            //    Focis[i].Height = 10;
            //    Focis[i].Shape = FociShape.Ellipse;
            //}
            LoadParam(CameraFileName);
            this.MouseUp += new MouseEventHandler(SkyrayCamera_MouseUp);
            //this.Paint+=new PaintEventHandler(SkyrayCamera_Paint);
            //WorkCurveHelper.curCamera = this;

            ReloadLang();
            initialFilpStrip();
            InitalFrmLanguage();
            InitTimer();
            


        }


        private void InitTimer()
        {

            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Enabled = false;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timerElapse);
        }


        int showTimes = 0;
        private void timerElapse(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                showTimes++;
                if (showTimes >= 3)
                {
                    bShowCoeff = false;
                    timer.Enabled = false;
                    UpdateOverlay();
                }
            }
            catch
            {
            }
        }

        private System.Drawing.Point CurrentClickPoint;

        void SkyrayCamera_MouseUp(object sender, MouseEventArgs e)
        {
            //cmsCamera.Items[1].Visible = false;
            //if (ModifierKeys != Keys.Control && e.Button == MouseButtons.Right && ShowCameraMenu)
            //    cmsCamera.Show(this.PointToScreen(e.Location));
            //Point p = new Point(e.X, e.Y);
            //AddContiMeasurePoint(p);

            double rateWidth = (viewWidth == 0) ? 1 : (this.Width / viewWidth);
            double rateHeight = (viewHeight == 0) ? 1 : (this.Height / viewHeight);
            int halfFociWidth = Convert.ToInt32(Focis[fociIndex].Width * rateWidth / 2);
            int halfFociHeight = Convert.ToInt32(Focis[fociIndex].Height * rateHeight / 2);
            Point p = new Point(e.X, e.Y);
            CurrentClickPoint = p;
            Rectangle rect = new Rectangle();
            tsmiDelCurrentFlag.Visible = false;
            
            for (int m = 0; m < alContiTestPoints.Count; m++)
            {
                rect = new Rectangle(((Point)alContiTestPoints[m]).X - halfFociWidth, ((Point)alContiTestPoints[m]).Y - halfFociHeight, halfFociWidth * 2, halfFociHeight * 2);
                if (rect.Contains(p) && Mode3000D == CameraMode.Coee)
                {
                    tsmiDelCurrentFlag.Visible = true;
                    break;
                }
            }

            
            
            if (ModifierKeys != Keys.Control && e.Button == MouseButtons.Right && ShowCameraMenu)
            {
                

                if (GP.UserName == "用户")
                    this.tsmAdjustOrient.Visible = false;
                else
                    this.tsmAdjustOrient.Visible = true;

                if (alContiTestPoints.Count > 0)
                {
                    tsmiDelLastFlag.Visible = true;
                    tsmiDelAllFlag.Visible = true;
                }
                else
                {
                    tsmiDelLastFlag.Visible = false;
                    tsmiDelAllFlag.Visible = false;
                }
                if (this.alContiTestPoints.Count > 0)
                {
                    this.tsmiSaveMultiPoint.Visible = true;
                    if (this.Mode == CameraMode.dotDot)
                        this.tsmiSaveOutMultiPoint.Visible = true;
                    else
                        this.tsmiSaveOutMultiPoint.Visible = false;
                }
                else
                {
                    this.tsmiSaveMultiPoint.Visible = false;
                    this.tsmiSaveOutMultiPoint.Visible = false;
                }



                if (fed != null)
                {
                    
                    this.tsmiEditMultiPoint.Visible = false;
                    this.tsmiShowAllTestPoint.Visible = false;
                    this.tsmiSaveMultiPoint.Visible = false;
                    this.tsmiSaveOutMultiPoint.Visible = false;
                    this.tsmiDelAllFlag.Visible = false;
                    this.tsmiDelLastFlag.Visible = false;
                    this.tsmiDelCurrentFlag.Visible = false;


                }
                else
                {
                    if (this.Mode != CameraMode.Move && this.Mode != CameraMode.Cell && this.Mode != CameraMode.Coee)
                    {
                        this.tsmiShowAllTestPoint.Visible = true;
                        this.tsmiEditMultiPoint.Visible = true;

                    }
                }
               
               
                cmsCamera.Show(this.PointToScreen(e.Location));
            }
        }
        #endregion

        #region 公开属性

        public ToolStripMenuItem CameraFormat { get; set; }

        public bool IsWidthHeightChecked { get; set; }

        public bool ShowCameraMenu = true;

        private CameraMode _Mode;

        public CameraMode Mode
        {
            get { return _Mode; }
            set
            {
                if (value == CameraMode.Coee)
                {
                    tsmiDelLastFlag.Visible = false;
                    tsmiDelAllFlag.Visible = false;
                    tsmiSetMoveRate.Visible = true;
                    tsmiCoeeGraphWidthHeight.Visible = true;
                }
                else
                {
                    tsmiDelLastFlag.Visible = false;
                    tsmiDelAllFlag.Visible = false;
                    tsmiSetMoveRate.Visible = false;
                    tsmiCoeeGraphWidthHeight.Visible = false;
                }

                if (value == CameraMode.Multiple)
                {
                    tsmiShowAllTestPoint.Visible = true;
                    tsmiEditMultiPoint.Visible = true;

                }
                else if (value == CameraMode.dotMatrix || value == CameraMode.matrixDot || value == CameraMode.dotDot)
                {
                   tsmiShowAllTestPoint.Visible = true;
                    tsmiEditMultiPoint.Visible = true;
                }
                else
                {
                     tsmiShowAllTestPoint.Visible = false;
                    tsmiEditMultiPoint.Visible = false;
                }
                

                _Mode = value;
                //if (s3000Mode == "1")
                //{
                //    tsmiSetMoveRate.Visible = true;
                //}
            }
        }

        public CameraMode Mode3000D
        {
            get { return _Mode; }
            set
            {
                if (value == CameraMode.Coee)
                {
                    tsmiSaveMultiPoint.Visible = false;
                    tsmiDelLastFlag.Visible = false;
                    tsmiDelAllFlag.Visible = false;
                    tsmiSetMoveRate.Visible = true;
                    tsmiCoeeGraphWidthHeight.Visible = false;
                }
                else
                {
                    tsmiSaveMultiPoint.Visible = false;
                    tsmiDelLastFlag.Visible = false;
                    tsmiDelAllFlag.Visible = false;
                    tsmiSetMoveRate.Visible = false;
                    tsmiCoeeGraphWidthHeight.Visible = false;
                }


                _Mode = value;
            }
        }

        private string _IsAdminUse;
        public string IsAdminUse
        {
            get { return _IsAdminUse; }
            set
            {
                //if (value == "false")
                //{
                //    tsmiCameraParam.Visible = false;
                //}
                _IsAdminUse = value;
            }

        }

        /// <summary>
        /// 行测量点数
        /// </summary>
        public int PointRowCount
        {
            get
            {
                return pointRowCount;
            }
            set
            {
                pointRowCount = value;
            }
        }

        /// <summary>
        /// 列测量点数
        /// </summary>
        public int PointColCount
        {
            get
            {
                return pointColCount;
            }
            set
            {
                pointColCount = value;
            }
        }

        /// <summary>
        /// 行测量点距
        /// </summary>
        public double PointRowDistance
        {
            get
            {
                return pointRowDistance;
            }
            set
            {
                pointRowDistance = value;
            }
        }

        /// <summary>
        /// 列测量点距
        /// </summary>
        public double PointColDistance
        {
            get
            {
                return pointColDistance;
            }
            set
            {
                pointColDistance = value;
            }
        }

        private string _IsXRF;
        public string IsXRF
        {
            get { return _IsXRF; }
            set
            {
                if (value == "IsXRF")
                {
                    tsmiCameraFormat.Visible = false;
                }
                _IsXRF = value;
            }

        }

        public ContextMenuStrip CMenu
        {
            get { return cmsCamera; }
        }

        #region 视图参数

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
                viewWidth =  value;
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
                viewHeight =  value;
            }
        }
        /// <summary>
        /// 刻度尺自定义单位长度（已做简单的边界检查，不接受负数，最小限制为0）
        /// </summary>
        public float RuleUnit
        {
            get
            {
                return (float)ruleUnit;
            }
            set
            {
                //ruleUnit = (value < 2) ? 2 : value;
                ruleUnit = (value < 0) ? 0 : value;
            }
        }

        #endregion

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
                tsmiMoveRateValueX.Text = value.ToString();
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
                tsmiMoveRateValueY.Text = value.ToString();
            }
        }


        public float m_XStartRate = 0;
        public float m_XWidthRate = 1;
        public float m_YStartRate = 0;
        public float m_YHeightRate = 1;
        public float zoomCoeff = 0.1f;  //放大间隔

        //当前存放大小，不保存
        private float _xStartRate = 0;
        private float _xWidthRate = 1;
        private float _yStartRate = 0;
        private float _yHeightRate = 1;

        private float _currentZoomCoeff = 1; //图片截图倍数
        private float _cutPicCoeff = 1; //当前放大
        private bool bShowCoeff = false;  //显示放大倍数

        public float CutPicCoeff
        {
            get { return _cutPicCoeff; }
            set { _cutPicCoeff = value; }
        }

        #endregion

        #region 公开字段

        public bool AutoSaveSamplePic
        {
            get { return AutoSaveSamplePicToolStripMenuItem.Checked; }
            set { AutoSaveSamplePicToolStripMenuItem.Checked = value; }
        }

        public System.Collections.ArrayList ContiTestPoints
        {
            set
            {
                this.alContiTestPoints = value;
                //if(alContiTestPoints.Count > 0)
                UpdateOverlay();
            }
            get
            {
                return this.alContiTestPoints;
            }
        }

    
        private Point focusPoint;
        public Point FocusPoint
        {
            set
            {
                this.focusPoint = value;
                //if(alContiTestPoints.Count > 0)
                UpdateOverlay();
            }
        }


       

        /// <summary>
        /// 连测点
        /// </summary>
        public System.Collections.ArrayList alContiTestPoints = new System.Collections.ArrayList();
        public System.Collections.ArrayList alTempTestPoints = new System.Collections.ArrayList();

        public List<int> alContiTestHeights = new List<int>();
        
        /// <summary>
        /// 是否显示连测点
        /// </summary>
        private bool bIsShowTestPoints = true;
        public bool IsShowTestPoints
        {
            set
            {
                bIsShowTestPoints = value;
                UpdateOverlay();
            }
            get
            {
                return this.bIsShowTestPoints;
            }
           
        }

        public string DeviceName { get; set; }
        /// <summary>
        /// 当前视频格式
        /// </summary>
        public CaptureFormat Format;
        /// <summary>
        /// 焦斑数据
        /// </summary>
        public Foci[] Focis;

        #endregion

        #region 公开方法

        /// <summary>
        /// 采样大小
        /// </summary>
        public Int32 GetSampleSize()
        {
            //return cam.SampleSize;
            return 0;
        }


        /// <summary>
        /// 测量信息
        /// </summary>
        /// <param name="iBrightness"></param>
        public void SetTestInformation(string testInfo, bool IsManualTest)
        {
            //strTestInformation = testInfo;
            lblTestInfomation.Text = testInfo;
            //lblTestInfomation.Font= new Font("Verdana", 10);
            // lblTestInfomation.ForeColor = Color.GreenYellow;
            lblTestInfomation.Left = this.Width - lblTestInfomation.Width - 10;
            lblTestInfomation.Top = this.Bottom - lblTestInfomation.Height - 10;
            lblTestInfomation.BackColor = Color.Transparent;
            btnOk.Visible = IsManualTest;
            btnOk.Top = lblTestInfomation.Top - btnOk.Height - 2;
            btnOk.BringToFront();
            lblTestInfomation.BringToFront();
            this.Refresh();


            UpdateOverlay();
        }

      

        public bool IsCameralFlip;

      
        /// <summary>
        /// 打开摄像头
        /// </summary>
        public void Open()
        {
           
            if (VideoSource == null) 
                LoadParam(CameraFileName);
            
            this.Close();//edit by chuyaqin 2010-04-15 重复打开


            try
            {

                

                this.Location = new Point(0, 0);
                this.Size = new Size(capVideoWidth, capVideoHeight);


                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
               
                if (videoDevices.Count > 0)
                {
                    videoSourceDevice = new VideoCaptureDevice(videoDevices[selectedDeviceIndex].MonikerString);//连接摄像头。
                    if (IsGain)
                        videoSourceDevice.SetCameraProcAmp(VideoProcAmpProperty.Gain, 0, VideoProcAmpFlags.Auto);

                    if (!videoDevices[selectedDeviceIndex].Name.Contains("Daheng"))
                    {
                        foreach (VideoCapabilities vcb in videoSourceDevice.VideoCapabilities)
                        {
                            if (this.capVideoWidth == vcb.FrameSize.Width && this.capVideoHeight == vcb.FrameSize.Height)
                            {
                                videoSourceDevice.VideoResolution = vcb;
                                break;
                            }
                        }
                        if (videoSourceDevice.VideoResolution == null)
                        {
                            foreach (VideoCapabilities vcb in videoSourceDevice.VideoCapabilities)
                            {
                                if (vcb.FrameSize.Width * vcb.FrameSize.Height <= MaxResolution)
                                {
                                    videoSourceDevice.VideoResolution = vcb;
                                    
                                    break;
                                }
                            }

                        }
                    }
                  
                    
                   
                   
                    this.VideoSource = videoSourceDevice;


                    this.Start();

                    FlipMethod();



                    lblTestInfomation.BringToFront();
                    btnOk.BringToFront();
                    RefreshFlipItems();
                    switch (this.capVideoHeight * this.capVideoWidth)
                    {
                        case (int)CaptureFormat.Smallest:
                            Format = CaptureFormat.Smallest;
                            break;
                        case (int)CaptureFormat.Smaller:
                            Format = CaptureFormat.Smaller;
                            break;
                        case (int)CaptureFormat.Middle:
                            Format = CaptureFormat.Middle;
                            break;
                        case (int)CaptureFormat.Larger:
                            Format = CaptureFormat.Larger;
                            break;
                        case (int)CaptureFormat.Largest:
                            Format = CaptureFormat.Largest;
                            break;
                        default:
                            //Format = CaptureFormat.Larger;
                            Format = CaptureFormat.Largest;
                            break;

                    }
                    if (FormatChanged != null)
                    {
                        FormatChanged(null, null);
                    }




                }
                else
                {
                    videoSourceDevice = null;
                    this.VideoSource = null;
                    //this.lblCameraInfo.Text = "摄像头未打开";
                }

                //分屏显示
                //FormStartScreen(2);
            }
            catch (Exception e)
            {
                videoSourceDevice = null;
                this.VideoSource = null;
                Close();
            }
        }



      
        //分屏显示
        public void FormStartScreen(int screen)
        {
            if (Screen.AllScreens.Length < screen)
            {
                // MessageBox.Show("当前主机连接最多的屏幕是" + Screen.AllScreens.Length + " 个，不能投屏到第" + screen + "个 屏幕！");
                return;
            }
            FrmScreentCamer form = new FrmScreentCamer();
            form.FociX = FociX;
            form.FociY = FociY;
            form.RuleUnit = ruleUnit;
            form.ViewWidth = viewWidth;
            form.ViewHeight = viewHeight;
            form.videoSource = this.VideoSource;
            form.Focis = this.Focis;
            form.TopMost = true;
            screen = screen - 1;
            form.StartPosition = FormStartPosition.CenterScreen;
            Screen s = Screen.AllScreens[screen];
            form.Location = new System.Drawing.Point(s.Bounds.X, s.Bounds.Y);
            form.WindowState = FormWindowState.Maximized;
            form.Size = new Size(s.WorkingArea.Width, s.WorkingArea.Height);
            form.Show();
            form.BringToFront();

        }

        /// <summary>


        /// <summary>
        /// 关闭摄像头
        /// </summary>
        public void Close()
        {
            if (this.VideoSource != null)
            {
                if (Skyray.EDX.Common.Component.MotorAdvance.CameraBitmap != null)
                {
                    Skyray.EDX.Common.Component.MotorAdvance.CameraBitmap.Dispose();
                    Skyray.EDX.Common.Component.MotorAdvance.CameraBitmap = null;
                }
                this.Save(CameraFileName);
                this.SignalToStop();
                this.WaitForStop();
                //this.Stop();
                this.VideoSource = null;
            }
            //this.lblCameraInfo.Text = "摄像头未打开";
        }

        /// <summary>
        /// 保存参数
        /// </summary>
        /// <param name="fileName"></param>
        public void Save(string fileName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            XmlNode rootNode = doc.SelectSingleNode("Camera/LargeCameraCaptureIndex");
            string LargeCameraCaptureIndex = rootNode.InnerText;
            
            XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.Unicode);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement("Camera");
            writer.WriteElementString("CaptureIndex", (this.selectedDeviceIndex+1).ToString());
            writer.WriteElementString("LargeCameraCaptureIndex", LargeCameraCaptureIndex);
            writer.WriteElementString("ViewWidth", viewWidth.ToString());
            writer.WriteElementString("ViewHeight", viewHeight.ToString());
            writer.WriteElementString("RuleUnit", ruleUnit.ToString());
            writer.WriteElementString("FociX", fociX.ToString());
            writer.WriteElementString("FociY", fociY.ToString());
            writer.WriteElementString("BmpWidth", this.capVideoWidth.ToString());
            writer.WriteElementString("BmpHeight", this.capVideoHeight.ToString());
            writer.WriteElementString("MoveRateX", moveRateX.ToString());
            writer.WriteElementString("MoveRateY", moveRateY.ToString());
            writer.WriteElementString("AdjustFlip", ((int)videoRotateFlip).ToString());
            writer.WriteStartElement("Focis");
            for (int i = 0; i < fociCount; i++)
            {
                writer.WriteStartElement("Foci");
                writer.WriteAttributeString("Width", Focis[i].Width.ToString());
                writer.WriteAttributeString("Height", Focis[i].Height.ToString());
                writer.WriteAttributeString("Shape", Focis[i].Shape.ToString());
                writer.WriteAttributeString("FociX", Focis[i].FociX.ToString());
                writer.WriteAttributeString("FociY", Focis[i].FociY.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteStartElement("BmpCut");
            writer.WriteElementString("XStartRate", m_XStartRate.ToString());
            writer.WriteElementString("XWidthRate", m_XWidthRate.ToString());
            writer.WriteElementString("YStartRate", m_YStartRate.ToString());
            writer.WriteElementString("YWidthRate", m_YHeightRate.ToString());
            writer.WriteElementString("ZoomCoeff", zoomCoeff.ToString());
            writer.WriteEndElement();
            writer.WriteElementString("SaveImage", this.AutoSaveSamplePicToolStripMenuItem.Checked ? "1" : "0");
            writer.WriteStartElement("GridParam");
            writer.WriteElementString("XRowCount", pointRowCount.ToString());
            writer.WriteElementString("XPointDistance", pointRowDistance.ToString());
            writer.WriteElementString("YColCount", pointColCount.ToString());
            writer.WriteElementString("YPointDistance", pointColDistance.ToString());
            writer.WriteEndElement();

            //if (alTempTestPoints.Count > 0)
            //{
            //    writer.WriteStartElement("MultiPoints");
            //    for (int i = 0; i < alTempTestPoints.Count; i++)
            //    {
            //        writer.WriteStartElement("Point");
            //        writer.WriteAttributeString("X", ((Point)alTempTestPoints[i]).X.ToString());
            //        writer.WriteAttributeString("Y", ((Point)alTempTestPoints[i]).Y.ToString());
            //        writer.WriteEndElement();
            //    }
            //    writer.WriteEndElement();
            //}


            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }



        /// <summary>
        /// 保存参数
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveMulitPoints(string fileName)
        {
            XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.Unicode);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement("Camera");
           

            for (int i = 0; i < alContiTestPoints.Count; i++)
            {
                writer.WriteStartElement("Point");
                writer.WriteAttributeString("X", ((Point)alContiTestPoints[i]).X.ToString());
                writer.WriteAttributeString("Y", ((Point)alContiTestPoints[i]).Y.ToString());
                if(this.Mode == CameraMode.Multiple)
                    writer.WriteAttributeString("Z", alContiTestHeights[i].ToString());
                writer.WriteEndElement();
                
            }
            writer.WriteStartElement("Steps");
            writer.WriteAttributeString("X", WorkCurveHelper.xSteps.ToString());
            writer.WriteAttributeString("Y", WorkCurveHelper.ySteps.ToString());
            writer.WriteEndElement();

            writer.WriteEndElement();

           

            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }
       


        public string[] GetVideoNames()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count > 0)
            {
                string[] names = new string[videoDevices.Count];
                for (int i = 0; i < names.Length; i++)
                {
                    names[i] = videoDevices[i].Name;
                }
                return names;
            }
            return null;
        }

        public void LoadParam(string fileName)
        {
            //int PenColor = int.Parse(ReportTemplateHelper.LoadSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "AppParams.xml", "application/PenColor"));
            //switch (PenColor)
            //{
            //    case (1):
            //        {
            //            Skyray.Camera.SkyrayCamera.curColor = Color.Red;
            //            break;
            //        }
            //    case (2):
            //        {
            //            Skyray.Camera.SkyrayCamera.curColor = Color.Orange;
            //            break;
            //        }
            //    case (3):
            //        {
            //            Skyray.Camera.SkyrayCamera.curColor = Color.Yellow;
            //            break;
            //        }
            //    case (4):
            //        {
            //            Skyray.Camera.SkyrayCamera.curColor = Color.Green;
            //            break;
            //        }
            //    case (5):
            //        {
            //            Skyray.Camera.SkyrayCamera.curColor = Color.Cyan;
            //            break;
            //        }
            //    case (6):
            //        {
            //            Skyray.Camera.SkyrayCamera.curColor = Color.Blue;
            //            break;
            //        }
            //    case (7):
            //        {
            //            Skyray.Camera.SkyrayCamera.curColor = Color.Purple;
            //            break;
            //        }
            //    case (8):
            //        {
            //            Skyray.Camera.SkyrayCamera.curColor = Color.Black;
            //            break;
            //        }
            //    case (9):
            //        {
            //            Skyray.Camera.SkyrayCamera.curColor = Color.White;
            //            break;
            //        }

            //}

            if (!File.Exists(fileName)) return;
            try
            {

                XmlDocument doc = new XmlDocument();
                string param = string.Empty;
                doc.Load(fileName);
                XmlNode rootNode = doc.SelectSingleNode("Camera/IsGain");
                IsGain = rootNode == null ? false : bool.Parse(rootNode.InnerText);
                rootNode = doc.SelectSingleNode("Camera/CaptureIndex");
                selectedDeviceIndex = rootNode == null ? 0 : int.Parse(rootNode.InnerText) -1;

                rootNode = doc.SelectSingleNode("Camera/ViewWidth");
                ViewWidth = rootNode == null ? 0 : Convert.ToDouble(rootNode.InnerText);

                rootNode = doc.SelectSingleNode("Camera/ViewHeight");
                ViewHeight = rootNode == null ? 0 : Convert.ToDouble(rootNode.InnerText);

                rootNode = doc.SelectSingleNode("Camera/RuleUnit");
                RuleUnit = (float)(rootNode == null ? 0 : Convert.ToDouble(rootNode.InnerText));

                rootNode = doc.SelectSingleNode("Camera/FociX");
                fociX = rootNode == null ? 0 : Convert.ToDouble(rootNode.InnerText);

                rootNode = doc.SelectSingleNode("Camera/FociY");
                fociY = rootNode == null ? 0 : Convert.ToDouble(rootNode.InnerText);


                rootNode = doc.SelectSingleNode("Camera/BmpWidth");
                this.capVideoWidth = rootNode == null ? 0 : Convert.ToInt32(rootNode.InnerText);

                rootNode = doc.SelectSingleNode("Camera/BmpHeight");
                this.capVideoHeight = rootNode == null ? 0 : Convert.ToInt32(rootNode.InnerText);

                rootNode = doc.SelectSingleNode("Camera/MoveRateX");
                MoveRateX = rootNode == null ? 0 : Convert.ToDouble(rootNode.InnerText);

                rootNode = doc.SelectSingleNode("Camera/MoveRateY");
                MoveRateY = rootNode == null ? 0 : Convert.ToDouble(rootNode.InnerText);


                rootNode = doc.SelectSingleNode("Camera/AdjustFlip");
                videoRotateFlip = rootNode == null ? RotateFlipType.RotateNoneFlipNone : (RotateFlipType)Convert.ToInt32(rootNode.InnerText);
                rootNode = doc.SelectSingleNode("Camera/SaveImage");
                AutoSaveSamplePic = rootNode == null ? false : Convert.ToInt32(rootNode.InnerText) == 1;

                rootNode = doc.SelectSingleNode("Camera/BmpCut/XStartRate");
                m_XStartRate = _xStartRate = (rootNode != null) ? Convert.ToSingle(rootNode.InnerText) : 0;

                rootNode = doc.SelectSingleNode("Camera/BmpCut/XWidthRate");
                m_XWidthRate = _xWidthRate = (rootNode != null) ? Convert.ToSingle(rootNode.InnerText) : 1f;


                rootNode = doc.SelectSingleNode("Camera/BmpCut/YStartRate");
                m_YStartRate = _yStartRate = (rootNode != null) ? Convert.ToSingle(rootNode.InnerText) : 0;

                rootNode = doc.SelectSingleNode("Camera/BmpCut/YWidthRate");
                m_YHeightRate = _yHeightRate = (rootNode != null) ? Convert.ToSingle(rootNode.InnerText) : 1f;

                rootNode = doc.SelectSingleNode("Camera/BmpCut/ZoomCoeff");
                zoomCoeff = (rootNode != null) ? Convert.ToSingle(rootNode.InnerText) : 0.1f;


                rootNode = doc.SelectSingleNode("Camera/GridParam/XRowCount");
                pointRowCount = rootNode == null ? 0 : Convert.ToInt32(rootNode.InnerText);

                rootNode = doc.SelectSingleNode("Camera/GridParam/XPointDistance");
                pointRowDistance = rootNode == null ? 0 : Convert.ToDouble(rootNode.InnerText);

                rootNode = doc.SelectSingleNode("Camera/GridParam/YColCount");
                pointColCount = rootNode == null ? 0 : Convert.ToInt32(rootNode.InnerText);

                rootNode = doc.SelectSingleNode("Camera/GridParam/YPointDistance");
                pointColDistance = rootNode == null ? 0 : Convert.ToDouble(rootNode.InnerText);

                XmlNode FociList = doc.SelectSingleNode("Camera/Focis");
                XmlNodeList FocisNodes = FociList.SelectNodes("Foci");
                for (int i = 0; i < FocisNodes.Count; i++)
                {
                    Focis[i].Width = Convert.ToDouble(FocisNodes[i].Attributes["Width"].InnerText);
                    Focis[i].Height = Convert.ToDouble(FocisNodes[i].Attributes["Height"].InnerText);
                    Focis[i].Shape = (FociShape)Enum.Parse(typeof(FociShape), FocisNodes[i].Attributes["Shape"].InnerText);
                    Focis[i].FociX = (FocisNodes[i].Attributes["FociX"] == null) ? 3 : Convert.ToDouble(FocisNodes[i].Attributes["FociX"].InnerText);
                    Focis[i].FociY = (FocisNodes[i].Attributes["FociY"] == null) ? 2.5 : Convert.ToDouble(FocisNodes[i].Attributes["FociY"].InnerText);

                }

                //XmlNode MultiPointsList = doc.SelectSingleNode("Camera/MultiPoints");
                //XmlNodeList MultiPointsNodes = MultiPointsList.SelectNodes("Point");
                //alTempTestPoints.Clear();
                //for (int i = 0; i < MultiPointsNodes.Count; i++)
                //{
                //    int x, y = 0;
                //    bool isx = int.TryParse(MultiPointsNodes[i].Attributes["X"].InnerText, out x);
                //    bool isy = int.TryParse(MultiPointsNodes[i].Attributes["Y"].InnerText, out y);
                //    alTempTestPoints.Add(new Point(x, y));
                //}

            }
            catch (Exception ce)
            {

                MessageBox.Show(ce.Message);

            }

        }

        public string LoadSpecifiedNodeValue(string path, string node)
        {
            XmlDocument doc = new XmlDocument();
            string param = string.Empty;
            if (File.Exists(path))
            {
                doc.Load(path);
                XmlNode rootNode = doc.SelectSingleNode("Camera/" + node);
                if (rootNode != null)
                {
                    param = rootNode.InnerText;
                }
            }
            return param;
        }

        public bool SaveSpecifiedNodeValue(string path, string tag, string label, string value)
        {
            XmlDocument doc = new XmlDocument();
            if (!File.Exists(path)) return false;
            doc.Load(path);
            string strnodePath = "Camera/" + (label == null || label == string.Empty ? tag : tag + "/" + label);
            XmlNode rootNode = doc.SelectSingleNode(strnodePath);
            if (rootNode != null)
            {
                rootNode.InnerText = value;
                doc.Save(path);
            }
            else
            {
                XmlNode rootNode_application = doc.SelectSingleNode("Camera");
                if (rootNode_application != null)
                {
                    XmlNode tagNode = rootNode_application.SelectSingleNode(tag);
                    if (tagNode != null)
                    {
                        XmlElement xelabel = doc.CreateElement(label);
                        xelabel.InnerText = value;
                        tagNode.AppendChild(xelabel);
                        doc.Save(path);
                    }
                    else
                    {
                        XmlElement xetag = doc.CreateElement(tag);
                        xetag.InnerText = value;
                        rootNode_application.AppendChild(xetag);
                        if (label != null && label != string.Empty)
                        {
                            tagNode = rootNode_application.SelectSingleNode(tag);
                            XmlElement xelabel = doc.CreateElement(label);
                            xelabel.InnerText = value;
                            tagNode.AppendChild(xelabel);
                        }
                        doc.Save(path);
                    }
                }
            }
            return true;

        }



        /// <summary>
        /// 抓图
        /// </summary>
        /// <returns>抓得的图</returns>
        public Bitmap GrabImage()
        {
            if (this.VideoSource != null)
            {
                return this.GetCurrentVideoFrame();
            }
            else
            {
                return null;
            
            }

        }


        //private Bitmap CamerImage;
        //public Bitmap GetCamerImage
        //{
        //    get
        //    {
        //        return CamerImage;
        //    }

        //}


        public void GetImage(string filepath, string imageName)
        {
            if (WorkCurveHelper.DeviceCurrent.HasMotorSpin)
            {
                string filename = filepath + "/" + imageName + ".jpg";
                using (Bitmap bmp = (Bitmap)WorkCurveHelper.reportImage.Clone())
                {
                    ZipImage((Image)bmp, ImageFormat.Bmp, 200, 0);
                    if (bmp != null)
                        bmp.Save(filename, ImageFormat.Jpeg);
                }

            }
            else
            {
                lock (drawbmp)
                {
                    try
                    {
                        if (this.VideoSource == null) return;
                        string filename = filepath + "/" + imageName + ".jpg";
                        //IntPtr hbitmap = this.GetCurrentVideoFrame().GetHbitmap();
                        //bitmap = System.Drawing.Image.FromHbitmap(hbitmap);
                        ////Bitmap bmpNew = new Bitmap(bitmap.Width, bitmap.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                        //Rectangle rectSource = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                        //Rectangle rectDest = new Rectangle(0, 0, 320, 240);

                        //Bitmap bmpNew = new Bitmap(320, 240);
                        //g = Graphics.FromImage(bmpNew);
                        //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                        //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        //g.Clear(Color.White);
                        //g.DrawImage(bitmap, rectDest, rectSource, GraphicsUnit.Pixel);

                        //((Image)bmpNew).Save(filename, ImageFormat.Jpeg);
                        //g.Dispose();
                        //bitmap.Dispose();
                        ////销毁对象 否则会内存溢出
                        //DeleteObject(hbitmap);


                        using (Bitmap bmp = this.GetCurrentVideoFrame())
                        {
                            ZipImage((Image)bmp, ImageFormat.Bmp, 200, 0);
                            if (bmp != null)
                                bmp.Save(filename, ImageFormat.Jpeg);
                        }


                        //using (Bitmap bmp = this.GrabImage())
                        //{
                        //    if (bmp != null)
                        //        bmp.Save(filename, ImageFormat.Jpeg);
                        //}
                        //Bitmap bit = new Bitmap(this.Width, this.Height);//实例化一个和窗体一样大的bitmap
                        //g = Graphics.FromImage(bit);
                        //g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;//质量设为最高
                        //g.CopyFromScreen(this.Left, this.Top, 0, 0, new Size(this.Width, this.Height));//保存整个窗体为图片

                        ////bit.Save("weiboTemp.png");
                        //if(this.BackgroundImage!=null)
                        //this.BackgroundImage.Save(filename, ImageFormat.Jpeg);

                    }
                    catch (Exception ced)
                    {


                        using (System.IO.FileStream fileStream = new FileStream(Application.StartupPath + @"\CameraError.log", FileMode.Append))
                        {
                            using (System.IO.StreamWriter streamWriter = new StreamWriter(fileStream))
                            {
                                if (!System.IO.Directory.Exists(filepath))
                                {
                                    //MessageBox.Show("Directory path not find!");
                                    streamWriter.WriteLine(DateTime.Now.ToString() + "  " + "Directory path not find!");
                                }
                                else streamWriter.WriteLine(DateTime.Now.ToString() + "  " + ced.Message);
                            }
                        }
                        //MessageBox.Show(ced.Message);
                        //if (g != null)
                        //    g.Dispose();
                        //if (bitmap != null)
                        //    bitmap.Dispose();


                    }
                }

            }
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        private ImageCodecInfo GetCodecInfo(ImageFormat format)
        {

            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();

            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.FormatID == format.Guid) return ici;

            }
            return null;

        }


        private ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in info)
            {
                if (ici.MimeType == mimeType) return ici;
            }
            return null;
        }

        public Bitmap SetGrabImage(byte[] imagedata)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(imagedata))
            {
                Bitmap b = new Bitmap(ms);
                //img = Drawing.Image.FromStream(ms)
                return b;
            }
        }

        // 画连测点
        private void DrawContiMeasurePoint(Graphics g)
        {
            foreach (Point point in alContiTestPoints)
            {
                // 注意X和Y要平移一段距离，以便圆的中心落在鼠标点上
                //g.DrawEllipse(new Pen(Color.Red, 3), point.X - 3, point.Y - 3, 6, 6);
                Pen pen = new Pen(Color.Red, 3);
                g.DrawLines(pen, new Point[] { new Point(point.X - 4, point.Y), new Point(point.X + 4, point.Y) });
                g.DrawLines(pen, new Point[] { new Point(point.X, point.Y - 4), new Point(point.X, point.Y + 4) });
            }
        }
        public void RemoveAtContiMeasurePoint(int index)
        {
            if (this.alContiTestPoints.Count > 0 && index < this.alContiTestPoints.Count)
            {
                this.alContiTestPoints.RemoveAt(index);
                this.isShowCenter = false;
                UpdateOverlay();
            }
            if (this.alContiTestPoints.Count == 0)
            {
                tsmiSaveMultiPoint.Visible = false;
                tsmiDelLastFlag.Visible = false;
                tsmiDelAllFlag.Visible = false;
            }
        }


        public void AddRelativeContiMeasurePoint(Point p)
        {

            tsmiSaveMultiPoint.Visible = true;
            tsmiDelLastFlag.Visible = true;
            tsmiDelAllFlag.Visible = true;

            this.alContiTestPoints.Add(p);
            this.alTempTestPoints = (System.Collections.ArrayList)this.alContiTestPoints.Clone();

            
            UpdateOverlay();


        }
        public void AddAbsoluteContiMeasurePoint(bool isReplace)
        {

            WorkCurveHelper.waitMoveStop();
            tsmiSaveMultiPoint.Visible = true;
            tsmiDelLastFlag.Visible = true;
            tsmiDelAllFlag.Visible = true;

            this.curMultiName = string.Empty;
            if (this.fed == null)
            {
                this.alContiTestPoints.Add(new Point((int)WorkCurveHelper.X, (int)WorkCurveHelper.Y));
                this.alTempTestPoints = (System.Collections.ArrayList)this.alContiTestPoints.Clone();


                if (this.Mode == CameraMode.Multiple)
                {
                    MotorOperator.getZ();
                    this.alContiTestHeights.Add((int)WorkCurveHelper.Z);
                }

            }
            else
            {
                if (!isReplace)
                {
                    //此处判断是否是编辑多点文件中的点
                    bool isInsert = false;
                    for (int i = 0; i < this.fed.dgvMultiDatas.Rows.Count - 1; i++)
                    {
                        if (this.fed.dgvMultiDatas.Rows[i].Cells[0].Value.ToString() == "--" && !isInsert)
                        {

                            this.alContiTestPoints.Insert(i, new Point((int)WorkCurveHelper.X, (int)WorkCurveHelper.Y));
                            this.alTempTestPoints = (System.Collections.ArrayList)this.alContiTestPoints.Clone();

                            if (this.Mode == CameraMode.Multiple)
                            {
                                MotorOperator.getZ();
                                this.alContiTestHeights.Insert(i, (int)WorkCurveHelper.Z);
                            }


                            this.fed.dgvMultiDatas.Rows[i].Cells[0].Value = (i + 1).ToString();
                            this.fed.dgvMultiDatas.Rows[i].Cells[1].Value = WorkCurveHelper.TabWidth - WorkCurveHelper.X / WorkCurveHelper.XCoeff;
                            this.fed.dgvMultiDatas.Rows[i].Cells[2].Value = WorkCurveHelper.TabHeight - WorkCurveHelper.Y / WorkCurveHelper.YCoeff;
                            if (this.Mode == CameraMode.Multiple)
                                this.fed.dgvMultiDatas.Rows[i].Cells[3].Value = WorkCurveHelper.Z / WorkCurveHelper.ZCoeff;
                            else
                                this.fed.dgvMultiDatas.Rows[i].Cells[3].Value = 0;

                            isInsert = true;

                        }
                        else
                        {
                            this.fed.dgvMultiDatas.Rows[i].Cells[0].Value = (i + 1).ToString();
                        }

                    }

                    if (!isInsert)
                    {
                        this.alContiTestPoints.Add(new Point((int)WorkCurveHelper.X, (int)WorkCurveHelper.Y));
                        this.alTempTestPoints = (System.Collections.ArrayList)this.alContiTestPoints.Clone();
                        if (this.Mode == CameraMode.Multiple)
                        {
                            MotorOperator.getZ();
                            this.alContiTestHeights.Add((int)WorkCurveHelper.Z);
                        }


                        this.fed.dgvMultiDatas.Rows.Add();
                        int index = this.fed.dgvMultiDatas.Rows.Count - 2;
                        this.fed.dgvMultiDatas.Rows[index].Cells[0].Value = (index + 1).ToString();
                        this.fed.dgvMultiDatas.Rows[index].Cells[1].Value = WorkCurveHelper.TabWidth - WorkCurveHelper.X / WorkCurveHelper.XCoeff;
                        this.fed.dgvMultiDatas.Rows[index].Cells[2].Value = WorkCurveHelper.TabHeight - WorkCurveHelper.Y / WorkCurveHelper.YCoeff;
                        if (this.Mode == CameraMode.Multiple)
                            this.fed.dgvMultiDatas.Rows[index].Cells[3].Value = WorkCurveHelper.Z / WorkCurveHelper.ZCoeff;
                        else
                            this.fed.dgvMultiDatas.Rows[index].Cells[3].Value = 0;

                    }



                }
                else
                {
                    if (this.fed.dgvMultiDatas.SelectedRows[0].Index < this.fed.dgvMultiDatas.Rows.Count - 1)
                    {

                        int index = this.fed.dgvMultiDatas.SelectedRows[0].Index;

                        this.fed.dgvMultiDatas.SelectedRows[0].Cells[1].Value = WorkCurveHelper.TabWidth - WorkCurveHelper.X / WorkCurveHelper.XCoeff;
                        this.fed.dgvMultiDatas.SelectedRows[0].Cells[2].Value = WorkCurveHelper.TabHeight - WorkCurveHelper.Y / WorkCurveHelper.YCoeff;

                        if (this.Mode == CameraMode.Multiple)
                            this.fed.dgvMultiDatas.SelectedRows[0].Cells[3].Value = WorkCurveHelper.Z / WorkCurveHelper.ZCoeff;
                        else
                            this.fed.dgvMultiDatas.SelectedRows[0].Cells[3].Value = 0;


                        this.alContiTestPoints[index] = new Point((int)WorkCurveHelper.X, (int)WorkCurveHelper.Y);
                        this.alTempTestPoints = (System.Collections.ArrayList)this.alContiTestPoints.Clone();

                        if (this.Mode == CameraMode.Multiple)
                        {
                            MotorOperator.getZ();
                            this.alContiTestHeights[index] = (int)WorkCurveHelper.Z;
                        }




                    }
                }


            }




            UpdateOverlay();
        }

        /// <summary>
        /// 清除测量点
        /// </summary>
        public void ClearContiMeasurePoint()
        {
            if (this.alContiTestPoints != null )
            {
                this.isShowCenter = false;
                tsmiSaveMultiPoint.Visible = false;
                tsmiDelLastFlag.Visible = false;
                tsmiDelAllFlag.Visible = false;
                this.alContiTestPoints.Clear();
                this.alTempTestPoints.Clear();
                if (this.Mode == CameraMode.Multiple)
                    this.alContiTestHeights.Clear();
                UpdateOverlay();
            }
        }

     

        /// <summary>
        /// 显示所有测量点
        /// </summary>
        /// 
        public void ShowAllMeasurePoints()
        {
            if (alTempTestPoints == null) return;
            ContiTestPoints.Clear();

            for (int i = 0; i < alTempTestPoints.Count; i++)
            {
                ContiTestPoints.Add(alTempTestPoints[i]);
            }

         

            MethodInfo myMethod = WorkCurveHelper.thickType.GetMethod("multiReset");
            myMethod.Invoke(WorkCurveHelper.curThick, null);
        

            if (ContiTestPoints != null && ContiTestPoints.Count > 0)
            {
                UpdateOverlay();
                isShowCenter = true;
            } 

            UpdateOverlay();

            
        }




        public void LoadAllPointsFile()
        {

            // 添加前，先清空
            tsmiShowAllTestPoint.DropDownItems.Clear();

            DirectoryInfo dir = null;
            if (this.Mode == CameraMode.Multiple)
                dir = new DirectoryInfo(PointPath);
            else if (this.Mode == CameraMode.dotMatrix)
                dir = new DirectoryInfo(MatrixPath + "\\dotMatrix\\");
            else if (this.Mode == CameraMode.matrixDot)
                dir = new DirectoryInfo(MatrixPath + "\\matrixDot\\");
            else if (this.Mode == CameraMode.dotDot)
                dir = new DirectoryInfo(MatrixPath + "\\dotDot\\");

            FileInfo[] fi = dir.GetFiles();

            Boolean IsExistMulti = false;
            Boolean IsExistSavedMulti = false;
            if (fi.Length == 0)
            {
                return;
            }
            foreach (FileInfo file in fi)
            {
                if (file.Extension == ".xml")
                {
                    IsExistMulti = true;
                }
                if (file.Name == currentMultiName)
                {
                    IsExistSavedMulti = true;
                }
            }
            if (!IsExistMulti)
            {
                return;
            }

            ToolStripMenuItem tsmi;
            foreach (FileInfo file in fi)
            {
                if (file.Extension == ".xml")
                {
                    tsmi = new ToolStripMenuItem(file.Name);
                    if (currentMultiName == "")
                    {
                        currentMultiName = file.Name;
                    }
                    if (file.Name == currentMultiName)
                    {
                        tsmi.Checked = true;
                    }
                    if (!IsExistSavedMulti)
                    {
                        tsmi.Checked = true;
                        IsExistSavedMulti = !IsExistSavedMulti;

                    }
                    tsmi.Click += new EventHandler(tsmiMu_Click);
                    tsmiShowAllTestPoint.DropDownItems.Insert(tsmiShowAllTestPoint.DropDownItems.Count, tsmi);
                }
            }
            UpdateOverlay();
        }


        void tsmiMu_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            foreach (ToolStripMenuItem tsmi in tsmiShowAllTestPoint.DropDownItems)
            {
                tsmi.Checked = false;
            }
            item.Checked = true;

            currentMultiName = item.ToString();
            curMultiName = currentMultiName;
            if (this.Mode == CameraMode.Multiple)
                LoadMultiPoints(PointPath + currentMultiName);
            else if (this.Mode == CameraMode.dotMatrix)
                LoadMultiPoints(MatrixPath + "\\dotMatrix\\" + currentMultiName);
            else if (this.Mode == CameraMode.matrixDot)
                LoadMultiPoints(MatrixPath + "\\matrixDot\\" + currentMultiName);
            else if (this.Mode == CameraMode.dotDot)
                LoadMultiPoints(MatrixPath + "\\dotDot\\" + currentMultiName);

            ShowAllMeasurePoints();
        }



        public void LoadMultiPoints(string fileName)
        {
            bIsShowTestPoints = true;

            try
            {
                if (!File.Exists(fileName)) return;

                XmlDocument doc = new XmlDocument();
                string param = string.Empty;
                doc.Load(fileName);
                XmlNode MultiPointsList = doc.SelectSingleNode("Camera");
                XmlNodeList MultiPointsNodes = MultiPointsList.SelectNodes("Point");


                XmlNode steps = doc.SelectSingleNode("Steps");

                alTempTestPoints.Clear();
                alContiTestHeights.Clear();

                //此处先确定当前点距离平台可移动范围的距离
                float curDisLeft = WorkCurveHelper.X;
                float curDisTop = WorkCurveHelper.Y;
                float curDisRight = WorkCurveHelper.TabWidth * WorkCurveHelper.XCoeff - curDisLeft;
                float curDisBot = WorkCurveHelper.TabHeight * WorkCurveHelper.YCoeff - curDisTop;

                int xToCurX = 0;
                int yToCurY = 0;

                if (this.Mode == CameraMode.dotDot)
                {
                    if (fileName.Contains("里"))
                    {

                        for (int i = 0; i < MultiPointsNodes.Count; i++)
                        {
                            int x, y = 0;
                            bool isx = int.TryParse(MultiPointsNodes[i].Attributes["X"].InnerText, out x);
                            bool isy = int.TryParse(MultiPointsNodes[i].Attributes["Y"].InnerText, out y);

                            alTempTestPoints.Add(new Point(x, y));

                        }


                    }
                    else
                    {
                        if (this.ContiTestPoints.Count == 0)
                        {

                            Msg.Show("请先选择里多点文件");
                            return;
                        }
                        for (int i = 0; i < MultiPointsNodes.Count; i++)
                        {
                            int x, y = 0;
                            bool isx = int.TryParse(MultiPointsNodes[i].Attributes["X"].InnerText, out x);
                            bool isy = int.TryParse(MultiPointsNodes[i].Attributes["Y"].InnerText, out y);

                            if (i == 0)
                            {
                                xToCurX = (int)WorkCurveHelper.X - x;
                                yToCurY = (int)WorkCurveHelper.Y - y;

                            }
                            //在不以第一个多点的绝对坐标为第一个测量点时的检查
                            if (!WorkCurveHelper.multiReset)
                            {
                                //文件中的绝对坐标点到当前点的偏移值

                                if (x + xToCurX > WorkCurveHelper.TabWidthStep)
                                {

                                    Msg.Show("选点范围已超过平台左向可移动范围");
                                    return;
                                }
                                if (x + xToCurX < 0)
                                {

                                    Msg.Show("选点范围已超过平台右向可移动范围");
                                    return;
                                }
                                if (y + yToCurY > WorkCurveHelper.TabHeightStep)
                                {

                                    Msg.Show("选点范围已超过平台前向可移动范围");
                                    return;
                                }
                                if (y + yToCurY < 0)
                                {

                                    Msg.Show("选点范围已超过平台后向可移动范围");
                                    return;
                                }
                            }
                            else
                            {

                                if (x > WorkCurveHelper.TabWidthStep)
                                {

                                    Msg.Show("选点范围已超过平台左向可移动范围");
                                    return;
                                }
                                if (x < 0)
                                {

                                    Msg.Show("选点范围已超过平台右向可移动范围");
                                    return;
                                }
                                if (y > WorkCurveHelper.TabHeightStep)
                                {

                                    Msg.Show("选点范围已超过平台前向可移动范围");
                                    return;
                                }
                                if (y < 0)
                                {

                                    Msg.Show("选点范围已超过平台后向可移动范围");
                                    return;
                                }
                            }

                            alTempTestPoints.Add(new Point(x, y));
                        }

                        int DistanceX = ((Point)this.alContiTestPoints[0]).X;
                        int DistanceY = ((Point)this.alContiTestPoints[0]).Y;


                        //先将多点绝对坐标转换为相对坐标
                        for (int i = 0; i < this.alContiTestPoints.Count; i++)
                        {
                            Point temp = (Point)this.alContiTestPoints[i];
                            temp.Offset(-DistanceX, -DistanceY);
                            this.alContiTestPoints[i] = temp;
                        }
                       
                        
                        int outterNum = alTempTestPoints.Count;


                        xToCurX = (int)WorkCurveHelper.X - ((Point)alTempTestPoints[0]).X;
                        yToCurY = (int)WorkCurveHelper.Y - ((Point)alTempTestPoints[0]).Y;


                        for (int outterIndex = 0; outterIndex < outterNum; outterIndex++)
                        {
                            int outterX = ((Point)alTempTestPoints[outterIndex * this.ContiTestPoints.Count]).X;
                            int outterY = ((Point)alTempTestPoints[outterIndex * this.ContiTestPoints.Count]).Y;

                            for (int innerIndex = 0; innerIndex < this.ContiTestPoints.Count; innerIndex++)
                            {
                                
                                if (innerIndex == 0)
                                    continue;
                                Point temp = new Point(outterX, outterY);
                                temp.Offset(((Point)this.ContiTestPoints[innerIndex]).X, ((Point)this.ContiTestPoints[innerIndex]).Y);

                                //在不以第一个多点的绝对坐标为第一个测量点时的检查
                                if (!WorkCurveHelper.multiReset)
                                {
                                    //文件中的绝对坐标点到当前点的偏移值

                                    if ( temp.X + xToCurX > WorkCurveHelper.TabWidthStep)
                                    {

                                        Msg.Show("选点范围已超过平台左向可移动范围");
                                        return;
                                    }
                                    if (temp.X + xToCurX < 0)
                                    {

                                        Msg.Show("选点范围已超过平台右向可移动范围");
                                        return;
                                    }
                                    if (temp.Y + yToCurY > WorkCurveHelper.TabHeightStep)
                                    {

                                        Msg.Show("选点范围已超过平台前向可移动范围");
                                        return;
                                    }
                                    if (temp.Y + yToCurY < 0)
                                    {

                                        Msg.Show("选点范围已超过平台后向可移动范围");
                                        return;
                                    }
                                }
                                else
                                {

                                    if (temp.X > WorkCurveHelper.TabWidthStep)
                                    {

                                        Msg.Show("选点范围已超过平台左向可移动范围");
                                        return;
                                    }
                                    if (temp.X < 0)
                                    {

                                        Msg.Show("选点范围已超过平台右向可移动范围");
                                        return;
                                    }
                                    if (temp.Y > WorkCurveHelper.TabHeightStep)
                                    {

                                        Msg.Show("选点范围已超过平台前向可移动范围");
                                        return;
                                    }
                                    if (temp.Y < 0)
                                    {

                                        Msg.Show("选点范围已超过平台后向可移动范围");
                                        return;
                                    }
                                }
                               
                                alTempTestPoints.Insert(outterIndex * this.ContiTestPoints.Count +
                                    innerIndex, temp);

                            }
                        }



                    }
                }
                else if (this.Mode == CameraMode.dotMatrix)
                {

                    for (int i = 0; i < MultiPointsNodes.Count; i++)
                    {
                        int x, y = 0;
                        bool isx = int.TryParse(MultiPointsNodes[i].Attributes["X"].InnerText, out x);
                        bool isy = int.TryParse(MultiPointsNodes[i].Attributes["Y"].InnerText, out y);

                        alTempTestPoints.Add(new Point(x, y));

                    }
                }
                else if (this.Mode == CameraMode.matrixDot)
                {
                    if (this.ContiTestPoints.Count == 0)
                    {

                        Msg.Show("请先确定网格点");
                        return;
                    }

                    for (int i = 0; i < MultiPointsNodes.Count; i++)
                    {
                        int x, y = 0;
                        bool isx = int.TryParse(MultiPointsNodes[i].Attributes["X"].InnerText, out x);
                        bool isy = int.TryParse(MultiPointsNodes[i].Attributes["Y"].InnerText, out y);

                        if (i == 0)
                        {
                            xToCurX = (int)WorkCurveHelper.X - x;
                            yToCurY = (int)WorkCurveHelper.Y - y;

                        }
                        //在不以第一个多点的绝对坐标为第一个测量点时的检查
                        if (!WorkCurveHelper.multiReset)
                        {
                            //文件中的绝对坐标点到当前点的偏移值

                            if (x + xToCurX > WorkCurveHelper.TabWidthStep)
                            {

                                Msg.Show("选点范围已超过平台左向可移动范围");
                                return;
                            }
                            if (x + xToCurX < 0)
                            {

                                Msg.Show("选点范围已超过平台右向可移动范围");
                                return;
                            }
                            if (y + yToCurY > WorkCurveHelper.TabHeightStep)
                            {

                                Msg.Show("选点范围已超过平台前向可移动范围");
                                return;
                            }
                            if (y + yToCurY < 0)
                            {

                                Msg.Show("选点范围已超过平台后向可移动范围");
                                return;
                            }
                        }
                        else
                        {

                            if (x > WorkCurveHelper.TabWidthStep)
                            {

                                Msg.Show("选点范围已超过平台左向可移动范围");
                                return;
                            }
                            if (x < 0)
                            {

                                Msg.Show("选点范围已超过平台右向可移动范围");
                                return;
                            }
                            if (y > WorkCurveHelper.TabHeightStep)
                            {

                                Msg.Show("选点范围已超过平台前向可移动范围");
                                return;
                            }
                            if (y < 0)
                            {

                                Msg.Show("选点范围已超过平台后向可移动范围");
                                return;
                            }
                        }


                        alTempTestPoints.Add(new Point(x, y));
                    }

                    int DistanceX = ((Point)this.alContiTestPoints[0]).X;
                    int DistanceY = ((Point)this.alContiTestPoints[0]).Y;


                    //先将网格点绝对坐标转换为相对坐标
                    for (int i = 0; i < this.alContiTestPoints.Count; i++)
                    {
                        Point temp = (Point)this.alContiTestPoints[i];
                        temp.Offset(-DistanceX, -DistanceY);
                        this.alContiTestPoints[i] = temp;
                    }


                    int outterNum = alTempTestPoints.Count;


                    xToCurX = (int)WorkCurveHelper.X - ((Point)alTempTestPoints[0]).X;
                    yToCurY = (int)WorkCurveHelper.Y - ((Point)alTempTestPoints[0]).Y;


                    for (int outterIndex = 0; outterIndex < outterNum; outterIndex++)
                    {
                        int outterX = ((Point)alTempTestPoints[outterIndex * this.ContiTestPoints.Count]).X;
                        int outterY = ((Point)alTempTestPoints[outterIndex * this.ContiTestPoints.Count]).Y;

                        for (int innerIndex = 0; innerIndex < this.ContiTestPoints.Count; innerIndex++)
                        {

                            if (innerIndex == 0)
                                continue;
                            Point temp = new Point(outterX, outterY);
                            temp.Offset(((Point)this.ContiTestPoints[innerIndex]).X, ((Point)this.ContiTestPoints[innerIndex]).Y);

                            //在不以第一个多点的绝对坐标为第一个测量点时的检查
                            if (!WorkCurveHelper.multiReset)
                            {
                                //文件中的绝对坐标点到当前点的偏移值

                                if (temp.X + xToCurX > WorkCurveHelper.TabWidthStep)
                                {

                                    Msg.Show("选点范围已超过平台左向可移动范围");
                                    return;
                                }
                                if (temp.X + xToCurX < 0)
                                {

                                    Msg.Show("选点范围已超过平台右向可移动范围");
                                    return;
                                }
                                if (temp.Y + yToCurY > WorkCurveHelper.TabHeightStep)
                                {

                                    Msg.Show("选点范围已超过平台前向可移动范围");
                                    return;
                                }
                                if (temp.Y + yToCurY < 0)
                                {

                                    Msg.Show("选点范围已超过平台后向可移动范围");
                                    return;
                                }
                            }
                            else
                            {

                                if (temp.X > WorkCurveHelper.TabWidthStep)
                                {

                                    Msg.Show("选点范围已超过平台左向可移动范围");
                                    return;
                                }
                                if (temp.X < 0)
                                {

                                    Msg.Show("选点范围已超过平台右向可移动范围");
                                    return;
                                }
                                if (temp.Y > WorkCurveHelper.TabHeightStep)
                                {

                                    Msg.Show("选点范围已超过平台前向可移动范围");
                                    return;
                                }
                                if (temp.Y < 0)
                                {

                                    Msg.Show("选点范围已超过平台后向可移动范围");
                                    return;
                                }
                            }

                            alTempTestPoints.Insert(outterIndex * this.ContiTestPoints.Count +
                                innerIndex, temp);

                        }
                    }



                }
                else
                {
                    for (int i = 0; i < MultiPointsNodes.Count; i++)
                    {
                        int x, y, z = 0;
                        bool isx = int.TryParse(MultiPointsNodes[i].Attributes["X"].InnerText, out x);
                        bool isy = int.TryParse(MultiPointsNodes[i].Attributes["Y"].InnerText, out y);
                        bool isZ = false;
                        if (this.Mode == CameraMode.Multiple)
                            isZ = int.TryParse(MultiPointsNodes[i].Attributes["Z"].InnerText, out z);

                        if (i == 0)
                        {
                            xToCurX = (int)WorkCurveHelper.X - x;
                            yToCurY = (int)WorkCurveHelper.Y - y;

                        }
                        //在不以第一个多点的绝对坐标为第一个测量点时的检查
                        if (!WorkCurveHelper.multiReset)
                        {
                            //文件中的绝对坐标点到当前点的偏移值

                            if (x + xToCurX > WorkCurveHelper.TabWidthStep)
                            {

                                Msg.Show("选点范围已超过平台左向可移动范围");
                                return;
                            }
                            if (x + xToCurX < 0)
                            {

                                Msg.Show("选点范围已超过平台右向可移动范围");
                                return;
                            }
                            if (y + yToCurY > WorkCurveHelper.TabHeightStep)
                            {

                                Msg.Show("选点范围已超过平台前向可移动范围");
                                return;
                            }
                            if (y + yToCurY < 0)
                            {

                                Msg.Show("选点范围已超过平台后向可移动范围");
                                return;
                            }
                        }
                        else
                        {

                            if (x > WorkCurveHelper.TabWidthStep)
                            {

                                Msg.Show("选点范围已超过平台左向可移动范围");
                                return;
                            }
                            if (x < 0)
                            {

                                Msg.Show("选点范围已超过平台右向可移动范围");
                                return;
                            }
                            if (y > WorkCurveHelper.TabHeightStep)
                            {

                                Msg.Show("选点范围已超过平台前向可移动范围");
                                return;
                            }
                            if (y < 0)
                            {

                                Msg.Show("选点范围已超过平台后向可移动范围");
                                return;
                            }
                        }

                        if (this.Mode == CameraMode.Multiple)
                            alContiTestHeights.Add(z);

                        alTempTestPoints.Add(new Point(x, y));
                    }
                }
            }
            catch (Exception ce)
            {
                MessageBox.Show(ce.Message);
            }
        }
   


        /// <summary>
        /// 保存所有测量点
        /// </summary>
        public void SaveAllMeasurePoint()
        {
            if (ContiTestPoints == null) return;

            FrmNewPointsName fnew = new FrmNewPointsName();

            
            if (fnew.ShowDialog() == DialogResult.OK)
            {
                if (fnew.Name != null)
                {
                    // string filePath = Application.StartupPath + @"MultiPoints\" + fnew.Name + ".xml";
                    string filePath = null;
                    if (this.Mode == CameraMode.Multiple)
                        filePath = PointPath + fnew.Name + ".xml";
                    else if (this.Mode == CameraMode.dotMatrix)
                        filePath = MatrixPath + "\\dotMatrix\\" + fnew.Name + ".xml";
                    else if (this.Mode == CameraMode.matrixDot)
                        filePath = MatrixPath + "\\matrixDot\\" + fnew.Name + ".xml";
                    else if (this.Mode == CameraMode.dotDot)
                    {
                        if (isInDot)
                            filePath = MatrixPath + "\\dotDot\\" + "里_" + fnew.Name + ".xml";
                        else
                        {
                            filePath = MatrixPath + "\\dotDot\\" + "外_" + fnew.Name + ".xml";
                            isInDot = true;
                        }

                    }

                    

                   
                    if (System.IO.File.Exists(filePath))
                    {
                        if (MessageBox.Show(this,
                                              CameraInfo.CamerFileExist,
                                              "Infomation",
                                              MessageBoxButtons.YesNo,
                                              MessageBoxIcon.Information) == DialogResult.No)
                        {
                            return;
                        }
                    }

                    

                    
                    SaveMulitPoints(filePath);
                    LoadAllPointsFile();             

                    if (motorBack != null)
                        motorBack();

                    
                }
            }


            
          

            UpdateOverlay();
        }
        #endregion

        public bool largeCameraSelect = false;

        #region 其他

        private Bitmap m_OverlayBmp = null;

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
            if (ruleUnit <= 0) return null;
            refreshCameraInfo("");
            Bitmap bmpToProcess = new Bitmap(bmpWidth, bmpHeight);

            #region 画自定义图层
            using (Graphics g = Graphics.FromImage(bmpToProcess))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias; //去掉字体黑边


                // Pen pen = Pens.Red;
                Pen pen = new Pen(curColor, 1 * bmpWidth / 640);
                //Pen pen = new Pen(Color.Red, 1);
                if (bmpWidth > 1024)
                    FontSizeCoef = 5;
                else
                    FontSizeCoef = 1;
                Font font = new Font("等线", (int)(12 + FontSizeCoef * bmpWidth / 320), FontStyle.Regular);//
                // Font font = new Font("等线", (int)(12 + 36), FontStyle.Regular);//
                //const int lineSign = 3;
                int lineSign = 8 * bmpWidth / 320;
                // 实际画面和自定义画面大小之比，用于将自定义参数换算成实际参数
                double rateWidthTemp = (viewWidth == 0) ? 1 : (bmpWidth * 1.0 / viewWidth) * _currentZoomCoeff;
                double rateHeightTemp = (viewHeight == 0) ? 1 : (bmpHeight * 1.0 / viewHeight) * _currentZoomCoeff;


                double rateWidth = (viewWidth == 0) ? 1 : (bmpWidth * 1.0 / viewWidth);
                double rateHeight = (viewHeight == 0) ? 1 : (bmpHeight * 1.0 / viewHeight);

                // 刻度尺实际的单位长度，通过上面的比值换算求得
                float bmpXUnit = (float)(ruleUnit * rateWidthTemp);
                float bmpYUnit = (float)(ruleUnit * rateHeightTemp);

                float littleBmpXUnit = (float)(ruleUnit * 0.2f * rateWidthTemp);
                float littleBmpYUnit = (float)(ruleUnit * 0.2f * rateHeightTemp);
                

                float coeff = 2 - _currentZoomCoeff;
                //按准直器重新赋值
                if (fociIndex >= 0)
                {
                    fociX = Focis[fociIndex].FociX; //
                    fociY = Focis[fociIndex].FociY; //
                }

                // 翻转的工作被放在绘图结束之后
                int bmpFociX = fociBmpX = Convert.ToInt32(fociX * rateWidth);
                int bmpFociY = fociBmpY = Convert.ToInt32(fociY * rateHeight);
                //int bmpFociX =0;
                //int bmpFociY =0;
                //if (fociIndex >= 0)
                //{
                //    bmpFociX = fociBmpX = Convert.ToInt32(Focis[fociIndex].FociX * rateWidth);
                //    bmpFociY = fociBmpY = Convert.ToInt32(Focis[fociIndex].FociY * rateHeight);
                //}

                /****************开始绘图*****************/
                int halfFociWidth = 0;
                int halfFociHeight = 0;

                int widthRate = bmpWidth / 640;
                int heightRate = bmpHeight / 480;

                //画焦斑
                if (fociIndex >= 0)
                {
                    halfFociWidth = Convert.ToInt32(Focis[fociIndex].Width * _currentZoomCoeff * rateWidth / 2);
                    halfFociHeight = Convert.ToInt32(Focis[fociIndex].Height * _currentZoomCoeff * rateHeight / 2);
                    Rectangle rect = new Rectangle(bmpFociX - halfFociWidth, bmpFociY - halfFociHeight,
                                                 halfFociWidth * 2, halfFociHeight * 2);
                    switch (Focis[fociIndex].Shape)
                    {   //画椭圆
                        case FociShape.Ellipse:
                            g.DrawEllipse(pen, rect);
                            break;
                        //画矩形
                        case FociShape.Rectangle:
                            g.DrawRectangle(pen, rect);
                            break;
                    }


                    rect = new Rectangle(bmpFociX - 2, bmpFociY - 2,
                                                 4, 4);
                    g.DrawEllipse(pen, rect);
                }


                WorkCurveHelper.halfFociWidth = Convert.ToInt32(Focis[fociIndex].Width * _currentZoomCoeff * 640.0 / viewWidth / 2);
                WorkCurveHelper.halfFociHeight = Convert.ToInt32(Focis[fociIndex].Height * _currentZoomCoeff * 480.0 / viewHeight / 2); ;

                //画比例尺
                g.DrawLine(pen, new Point(3, bmpHeight - 8), new Point(3, bmpHeight - 4));
                g.DrawLine(pen, new PointF(3, bmpHeight - 6), new PointF(3 + bmpXUnit, bmpHeight - 6));
                g.DrawLine(pen, new PointF(3 + bmpXUnit, bmpHeight - 8), new PointF(3 + bmpXUnit, bmpHeight - 4));
                g.DrawString(ruleUnit.ToString("f3") + "mm", font, new SolidBrush(curColor), 3, bmpHeight - 11 - font.Size);

                //显示放大倍数
                if (bShowCoeff)
                    g.DrawString(Convert.ToString(_currentZoomCoeff) + "X", font, new SolidBrush(Color.WhiteSmoke), bmpWidth / 2 + 10, bmpHeight - 11 - font.Size);

                // 画横轴
                // g.DrawLine(pen, new Point(0, bmpFociY), new Point(bmpWidth, bmpFociY));
                g.DrawLine(pen, new Point(0, bmpFociY), new Point(bmpFociX - halfFociWidth, bmpFociY));
                g.DrawLine(pen, new Point(bmpFociX + halfFociWidth, bmpFociY), new Point(bmpWidth, bmpFociY));
                // 画纵轴
                //g.DrawLine(pen, new Point(bmpFociX, 0), new Point(bmpFociX, bmpHeight));
                g.DrawLine(pen, new Point(bmpFociX, 0), new Point(bmpFociX, bmpFociY - halfFociHeight));
                g.DrawLine(pen, new Point(bmpFociX, bmpFociY + halfFociHeight), new Point(bmpFociX, bmpHeight));

                // 从焦点起步，向左画横轴刻度
                float ruleScaleLinePosX = bmpFociX;   // 刻度线初始位置
                float ruleScaleLineLength = 0;
                g.DrawString(Convert.ToString(0), font, new SolidBrush(curColor), bmpFociX + 2, bmpFociY + 2);
                while (ruleScaleLinePosX > 0)   // 一直平移到画面最左端
                {
                    // 每次刻度线位置向左平移一个单位长度
                    ruleScaleLinePosX -= bmpXUnit;

                    ruleScaleLineLength += RuleUnit;
                    //取消显示X轴左边刻度
                    // g.DrawString("-" + Convert.ToString(ruleScaleLineLength), font, new SolidBrush(Color.Tomato), ruleScaleLinePosX - 8, bmpFociY + 2);
                    // 垂直画一条短线表示刻度
                    g.DrawLine(pen, new PointF(ruleScaleLinePosX, bmpFociY - lineSign), new PointF(ruleScaleLinePosX, bmpFociY));

                    
                }


                int no = 0;
                float littleRuleScaleLinePosX = bmpFociX;   // 刻度线初始位置

                while (littleRuleScaleLinePosX > 0)   // 一直平移到画面最左端
                {
                    no++;
                    littleRuleScaleLinePosX -= littleBmpXUnit;
                    if (no % 5 != 0)
                        g.DrawLine(pen, new PointF(littleRuleScaleLinePosX, bmpFociY - (lineSign >= 2 ? (int)(lineSign * 0.5) : lineSign)), new PointF(littleRuleScaleLinePosX, bmpFociY));

                }



                // 从焦点起步，向右画横轴刻度
                ruleScaleLinePosX = bmpFociX;   // 刻度线初始位置
                ruleScaleLineLength = 0;
                while (ruleScaleLinePosX < bmpWidth)   // 一直平移到画面最右端
                {
                    // 每次刻度线位置向右平移一个单位长度
                    ruleScaleLinePosX += bmpXUnit;
                    ruleScaleLineLength += RuleUnit;// bmpXUnit;
                    g.DrawString(Convert.ToString(ruleScaleLineLength), font, new SolidBrush(curColor), ruleScaleLinePosX - 4, bmpFociY + 2);

                    // 垂直画一条短线表示刻度
                    g.DrawLine(pen, new PointF(ruleScaleLinePosX, bmpFociY - lineSign), new PointF(ruleScaleLinePosX, bmpFociY));
                }


                no = 0;
                littleRuleScaleLinePosX = bmpFociX;   // 刻度线初始位置

                while (littleRuleScaleLinePosX < bmpWidth)   // 一直平移到画面最左端
                {
                    no++;
                    littleRuleScaleLinePosX += littleBmpXUnit;

                    if (no % 5 != 0)
                        g.DrawLine(pen, new PointF(littleRuleScaleLinePosX, bmpFociY - (lineSign >= 2 ? (int)(lineSign * 0.5) : lineSign)), new PointF(littleRuleScaleLinePosX, bmpFociY));

                }


                // 从焦点起步，向上画纵轴刻度
                float ruleScaleLinePosY = bmpFociY;   // 刻度线初始位置
                ruleScaleLineLength = 0;
                while (ruleScaleLinePosY > 0)   // 一直平移到画面最上端
                {
                    // 每次刻度线位置向上平移一个单位长度
                    ruleScaleLinePosY -= bmpYUnit;
                    ruleScaleLineLength += RuleUnit;
                    //取消显示Y刻度
                    //g.DrawString(Convert.ToString(ruleScaleLineLength), font, new SolidBrush(Color.Tomato), bmpFociX + 2, ruleScaleLinePosY - 8);

                    // 垂直画一条短线表示刻度
                    g.DrawLine(pen, new PointF(bmpFociX + lineSign, ruleScaleLinePosY), new PointF(bmpFociX, ruleScaleLinePosY));

                }


                no = 0;
                float littleRuleScaleLinePosY = bmpFociY;   // 刻度线初始位置

                while (littleRuleScaleLinePosY > 0)   // 一直平移到画面最左端
                {
                    no++;
                    littleRuleScaleLinePosY -= littleBmpYUnit;
                    if (no % 5 != 0)
                        g.DrawLine(pen, new PointF(bmpFociX + (lineSign >= 2 ? (int)(lineSign * 0.5) : lineSign), littleRuleScaleLinePosY), new PointF(bmpFociX, littleRuleScaleLinePosY));


                }


                // 从焦点起步，向下画纵轴刻度
                ruleScaleLinePosY = bmpFociY;   // 刻度线初始位置
                ruleScaleLineLength = 0;
                while (ruleScaleLinePosY < bmpHeight)   // 一直平移到画面最下端
                {
                    // 每次刻度线位置向下平移一个单位长度
                    ruleScaleLinePosY += bmpYUnit;
                    ruleScaleLineLength += RuleUnit;
                    //取消显示Y刻度
                    // g.DrawString("-" + Convert.ToString(ruleScaleLineLength), font, new SolidBrush(Color.Tomato), bmpFociX + 2, ruleScaleLinePosY - 8);
                    // 垂直画一条短线表示刻度
                    g.DrawLine(pen, new PointF(bmpFociX + lineSign, ruleScaleLinePosY), new PointF(bmpFociX, ruleScaleLinePosY));
                }


                no = 0;
                littleRuleScaleLinePosY = bmpFociY;   // 刻度线初始位置

                while (littleRuleScaleLinePosY < bmpHeight)   // 一直平移到画面最左端
                {
                    no++;
                    littleRuleScaleLinePosY += littleBmpYUnit;
                    if (no % 5 != 0)
                        g.DrawLine(pen, new PointF(bmpFociX + (lineSign >= 2 ? (int)(lineSign * 0.5) : lineSign), littleRuleScaleLinePosY), new PointF(bmpFociX, littleRuleScaleLinePosY));


                }




                //画图像清晰度识别位置

                //if (Skyray.EDX.Common.CameraRefMotor.IsShowDefine)
                //{
                //    if (definePen.Color == Color.Yellow)
                //        definePen.Color = Color.Green;
                //    else
                //        definePen.Color = Color.Yellow;
                //}
                //else
                //{
                //    definePen.Color = Color.Yellow;
                //}


                #region 画中心位
                //int halfWidth = bmpWidth / 2;
                //int halfHeight = bmpHeight / 2;
                //int RectLong = halfHeight / 6;
                ////bmpFociX  bmpFociY 中心位置


                ////左上角
                //g.DrawLine(definePen, new Point(bmpFociX - halfWidth / focusSize, bmpFociY - halfHeight / focusSize), new Point(bmpFociX - halfWidth / focusSize + RectLong, bmpFociY - halfHeight / focusSize));
                //g.DrawLine(definePen, new Point(bmpFociX - halfWidth / focusSize, bmpFociY - halfHeight / focusSize), new Point(bmpFociX - halfWidth / focusSize, bmpFociY - halfHeight / focusSize + RectLong));

                ////右上角
                //g.DrawLine(definePen, new Point(bmpFociX + halfWidth / focusSize - RectLong, bmpFociY - halfHeight / focusSize), new Point(bmpFociX + halfWidth / focusSize, bmpFociY - halfHeight / focusSize));
                //g.DrawLine(definePen, new Point(bmpFociX + halfWidth / focusSize, bmpFociY - halfHeight / focusSize), new Point(bmpFociX + halfWidth / focusSize, bmpFociY - halfHeight / focusSize + RectLong));


                ////左下角
                //g.DrawLine(definePen, new Point(bmpFociX - halfWidth / focusSize, bmpFociY + halfHeight / focusSize), new Point(bmpFociX - halfWidth / focusSize + RectLong, bmpFociY + halfHeight / focusSize));
                //g.DrawLine(definePen, new Point(bmpFociX - halfWidth / focusSize, bmpFociY + halfHeight / focusSize), new Point(bmpFociX - halfWidth / focusSize, bmpFociY + halfHeight / focusSize - RectLong));



                ////右下角
                //g.DrawLine(definePen, new Point(bmpFociX + halfWidth / focusSize - RectLong, bmpFociY + halfHeight / focusSize), new Point(bmpFociX + halfWidth / focusSize, bmpFociY + halfHeight / focusSize));
                //g.DrawLine(definePen, new Point(bmpFociX + halfWidth / focusSize, bmpFociY + halfHeight / focusSize), new Point(bmpFociX + halfWidth / focusSize, bmpFociY + halfHeight / focusSize - RectLong));

                #endregion
                //////左上角
                ////g.DrawLine(definePen, new Point(bmpFociX - halfWidth / 3, bmpFociY - halfHeight / 3), new Point(bmpFociX - halfWidth / 3 + RectLong, bmpFociY - halfHeight / 3));
                ////g.DrawLine(definePen, new Point(bmpFociX - halfWidth / 3, bmpFociY - halfHeight / 3), new Point(bmpFociX - halfWidth / 3, bmpFociY - halfHeight / 3 + RectLong));

                //////右上角
                ////g.DrawLine(definePen, new Point(bmpFociX + halfWidth / 3 - RectLong, bmpFociY - halfHeight / 3), new Point(bmpFociX + halfWidth / 3, bmpFociY - halfHeight / 3));
                ////g.DrawLine(definePen, new Point(bmpFociX + halfWidth / 3, bmpFociY - halfHeight / 3), new Point(bmpFociX + halfWidth / 3, bmpFociY - halfHeight / 3 + RectLong));


                //////左下角
                ////g.DrawLine(definePen, new Point(bmpFociX - halfWidth / 3, bmpFociY + halfHeight / 3), new Point(bmpFociX - halfWidth / 3 + RectLong, bmpFociY + halfHeight / 3));
                ////g.DrawLine(definePen, new Point(bmpFociX - halfWidth / 3, bmpFociY + halfHeight / 3), new Point(bmpFociX - halfWidth / 3, bmpFociY + halfHeight / 3 - RectLong));



                //////右下角
                ////g.DrawLine(definePen, new Point(bmpFociX + halfWidth / 3 - RectLong, bmpFociY + halfHeight / 3), new Point(bmpFociX + halfWidth / 3, bmpFociY + halfHeight / 3));
                ////g.DrawLine(definePen, new Point(bmpFociX + halfWidth / 3, bmpFociY + halfHeight / 3), new Point(bmpFociX + halfWidth / 3, bmpFociY + halfHeight / 3 - RectLong));




                /*******************************/

                //画中心点
                if (isShowCenter)
                {
                    g.DrawLine(pen, new Point(bmpFociX - halfFociWidth, bmpFociY), new Point(bmpFociX + halfFociWidth, bmpFociY));
                    g.DrawLine(pen, new Point(bmpFociX, bmpFociY - halfFociHeight), new Point(bmpFociX, bmpFociY + halfFociHeight));

                    //halfFociWidth = Convert.ToInt32(Focis[fociIndex].Width * rateWidth / 2);
                    //halfFociHeight = Convert.ToInt32(Focis[fociIndex].Height * rateHeight / 2);
                    //Rectangle rect = new Rectangle(bmpFociX - halfFociWidth, bmpFociY - halfFociHeight,
                    //                             halfFociWidth * 2, halfFociHeight * 2);
                    //g.DrawEllipse(pen, rect);

                }

                // 画连测点
                if (bIsShowTestPoints)
                {
                    halfFociWidth = Convert.ToInt32(Focis[fociIndex].Width * rateWidth * _currentZoomCoeff / 2);
                    halfFociHeight = Convert.ToInt32(Focis[fociIndex].Height * rateHeight * _currentZoomCoeff / 2);

                    int X, Y;
                    int stringpoint = 5;
                    if (halfFociHeight > 24) stringpoint = Convert.ToInt32(halfFociHeight / 2 - 10);

                    if (alContiTestPoints.Count == 0)
                        return bmpToProcess;
                    Font noFont = new Font("序号", halfFociHeight, FontStyle.Regular);//

                    Point curTestPoint;
                    if (alTempTestPoints.Count == alContiTestPoints.Count)
                        curTestPoint = (Point)alTempTestPoints[0];
                    else
                        curTestPoint = (Point)alTempTestPoints[alTempTestPoints.Count - alContiTestPoints.Count-1];

                    for (int m = 0; m < alContiTestPoints.Count; m++)
                    {
                        X = ((Point)alContiTestPoints[m]).X - curTestPoint.X;
                        Y = ((Point)alContiTestPoints[m]).Y - curTestPoint.Y;

                        float locX = 0;
                        float locY = 0;

                       

                        locX = (float)(bmpFociX + X / WorkCurveHelper.XCoeff * rateWidth);

                        locY = (float)(bmpFociY + Y / WorkCurveHelper.YCoeff * rateHeight);


                        

                        using (SolidBrush brush = new SolidBrush(curColor))
                        {
                            if(m <9)
                                g.DrawString((m + 1).ToString(), noFont, brush, locX - halfFociWidth + bmpWidth/640.0f * 7, locY - halfFociHeight + bmpHeight/480.0f*5 );
                            else
                                g.DrawString((m + 1).ToString(), noFont, brush, locX - halfFociWidth + bmpWidth / 640.0f * 0.4f, locY - halfFociHeight + bmpHeight / 480.0f * 5);

                        }

                        locX -= halfFociWidth;
                        locY -= halfFociHeight;


                        g.DrawEllipse(pen, locX, locY, halfFociWidth * 2, halfFociHeight * 2);

                    }


                }


            }
            #endregion
            // 翻转
            // bmpToProcess.RotateFlip(RotateFlipType.RotateNoneFlipY);
            // bmpToProcess.RotateFlip(RotateFlipType.Rotate270FlipY);
            return bmpToProcess;
        }


        // 画面大小调整事件
        private void SkyrayCamera_Resize(object sender, EventArgs e)
        {
            lblCameraInfo.Location = new Point((Width - lblCameraInfo.Width) / 2,
              (Height - lblCameraInfo.Height) / 2);

            UpdateOverlay();


        }

        // 消息循环
        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                //截获消息
                case WM_LBUTTONDOWN://鼠标左键单击
                    POINT pntCursorPos;
                    GetCursorPos(out pntCursorPos);
                    Point p = new Point(pntCursorPos.X, pntCursorPos.Y);
                    p = this.PointToClient(p);
                    if (this.VideoSource != null && Control.MouseButtons == MouseButtons.Left && Control.ModifierKeys == Keys.Shift)
                    {
                        if (DialogResult.Yes ==
                            MessageBox.Show(
                            this,
                            CameraInfo.MessageBoxTextCameraIsSureChangeFocus,
                            CameraInfo.MessageBoxCaptionTip,
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question))
                        {
                            fociX = viewWidth * p.X / this.Width;
                            fociY = viewHeight * p.Y / this.Height;
                            //按准直器重新赋值
                            if (fociIndex >= 0)
                            {
                                Focis[fociIndex].FociX = fociX; //
                                Focis[fociIndex].FociY = fociY; //
                            }
                            if (this.VideoSource != null)
                            {
                                this.UpdateOverlay();
                                //ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "Camera.xml", "Camera/FociX", fociX.ToString());
                                //ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "Camera.xml", "Camera/FociY", fociY.ToString());
                            }
                        }
                    }

                    break;

                case WM_CAMERA_UPDATEOVERLAY:
                    if (this.VideoSource != null)
                    {
                        this.UpdateOverlay();
                    }
                    break;
                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }

        #endregion

        #region 外部方法、机构及常量

        [DllImport("User32")]
        public extern static void mouse_event(int dwFlags, int dx, int dy, int dwData, IntPtr dwExtraInfo);

        [DllImport("User32")]
        public extern static void SetCursorPos(int x, int y);

        [DllImport("User32")]
        public extern static bool GetCursorPos(out POINT p);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        /// <summary>
        /// 移动鼠标
        /// </summary>
        public const int WM_MOUSEMOVE = 0x0200;
        /// <summary>
        /// 按下鼠标左键
        /// </summary>
        public const int WM_LBUTTONDOWN = 0x0201;
        /// <summary>
        /// 释放鼠标左键
        /// </summary>
        public const int WM_LBUTTONUP = 0x0202;
        /// <summary>
        /// 双击鼠标左键
        /// </summary>
        public const int WM_LBUTTONDBLCLK = 0x0203;
        /// <summary>
        /// 按下鼠标右键
        /// </summary>
        public const int WM_RBUTTONDOWN = 0x0204;
        /// <summary>
        /// 释放鼠标右键
        /// </summary>
        public const int WM_RBUTTONUP = 0x0205;
        /// <summary>
        /// 双击鼠标右键
        /// </summary>
        public const int WM_RBUTTONDBLCLK = 0x0206;
        /// <summary>
        /// 按下鼠标中键
        /// </summary>
        public const int WM_MBUTTONDOWN = 0x0207;
        /// <summary>
        /// 释放鼠标中键
        /// </summary>
        public const int WM_MBUTTONUP = 0x0208;
        /// <summary>
        /// 双击鼠标中键
        /// </summary>
        public const int WM_MBUTTONDBLCLK = 0x0209;
        /// <summary>
        /// 当鼠标轮子转动时发送此消息个当前有焦点的控件
        /// </summary>
        public const int WM_MOUSEWHEEL = 0x020A;

        // 用户自定义
        private const int WM_USER = 0x0400;
        /// <summary>
        /// 视频更新
        /// </summary>
        public const int WM_CAMERA_UPDATEOVERLAY = WM_USER + 50;

        #endregion

        #region 菜单事件

        private void tsmiOpen_Click(object sender, EventArgs e)
        {
            this.Open();
        }

        private void tsmiClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsmiCameraProperty_Click(object sender, EventArgs e)
        {
            if (videoSourceDevice == null) return;
            videoSourceDevice.DisplayPropertyPage(IntPtr.Zero);
        }
        #region 设置视频源和改变像素等

        private void ChangeVedioIndex(int value)
        {
            if (this.largeCameraSelect)
            {
                Skyray.EDX.Common.ReportTemplateHelper.SaveSpecifiedParamsValue(AppDomain.CurrentDomain.BaseDirectory + "Camera.xml", "Camera/LargeCameraCaptureIndex", (value + 1).ToString());
                
            }
            else
            {
                this.VideoIndex = value;
                this.Open();
                
            }
        }
        private void ChangeResoluteon(Size size)
        {
            
            this.capVideoWidth = size.Width;
            this.capVideoHeight = size.Height;
            this.Open();   
        }
        public bool SetCaptureFormat(Int32 formatSize)
        {
            this.Format = (CaptureFormat)formatSize;
            if (FormatChanged != null)
            {
                FormatChanged(null, null);
            }
            return true;
        }
        public  void tsmiCameraFormat_Click(object sender, EventArgs e)
        {
           
            if( (this.VideoSource != null && this.FociIndex >= 0) || this.largeCameraSelect)
            {
                FrmSetCameraFormat frmSetCameraFormat = new FrmSetCameraFormat(this);
                frmSetCameraFormat.EditResolution += new ChangeFrameSizeOp(ChangeResoluteon);
                frmSetCameraFormat.EditVideoIndex += new DoCameraOp(ChangeVedioIndex);

                if (this.largeCameraSelect)
                {
                    frmSetCameraFormat.cboCameraFormats.Enabled = false;
                    frmSetCameraFormat.cboVideoCapabilities.Enabled = false;
                }
                frmSetCameraFormat.ShowDialog(this);
                frmSetCameraFormat.EditResolution -= new ChangeFrameSizeOp(ChangeResoluteon);
                frmSetCameraFormat.EditVideoIndex -= new DoCameraOp(ChangeVedioIndex);


            }
            else
            {
                MessageBox.Show(this,
                   CameraInfo.MessageBoxTextCameraHasProblem,
                   CameraInfo.MessageBoxCaptionTip,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

        }
        #endregion


        private void tsmiCameraParam_Click(object sender, EventArgs e)
        {
            if (this.VideoSource != null)
            {
                FrmVideoParam paramForm = new FrmVideoParam(this, FociIndex);
                // 下面的赋值顺序不能变
                paramForm.pViewWidth = this.ViewWidth;
                paramForm.pViewHeight = this.ViewHeight;
                paramForm.pFocusX = this.Focis[this.FociIndex].FociX;//this.FociX;
                paramForm.pFocusY = this.Focis[this.FociIndex].FociY;//this.FociY;
                paramForm.pScaleUnit = this.RuleUnit;
                if (this.FociIndex != -1)
                {
                    paramForm.pSpotWidth = this.Focis[this.FociIndex].Width;
                    paramForm.pSpotHeight = this.Focis[this.FociIndex].Height;
                    paramForm.Shape = this.Focis[this.FociIndex].Shape;
                }
                if (paramForm.ShowDialog(this) == DialogResult.OK && decimal.Parse(paramForm.pScaleUnit.ToString()) != 0)
                {
                    for (int i = 0; i < this.Focis.Length; i++)
                        this.Focis[i].Shape = paramForm.Shape;
                    this.Focis[this.FociIndex].Shape = paramForm.Shape;
                    this.Focis[this.FociIndex].Width = paramForm.pSpotWidth;
                    this.Focis[this.FociIndex].Height = paramForm.pSpotHeight;
                    this.RuleUnit = (float)paramForm.pScaleUnit;
                    this.FociX = paramForm.pFocusX;
                    this.FociY = paramForm.pFocusY;
                    this.ViewWidth = paramForm.pViewWidth;
                    this.ViewHeight = paramForm.pViewHeight;
                    this.UpdateOverlay();
                    // this.Save(AppDomain.CurrentDomain.BaseDirectory + "Camera.xml");
                    this.Save(CameraFileName);
                }
            }
            else
            {
                MessageBox.Show(this,
                    CameraInfo.MessageBoxTextCameraNotOpened,
                    CameraInfo.MessageBoxCaptionTip,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void tsmiDelLastFlag_Click(object sender, EventArgs e)
        {
            if (ContiTestPoints.Count > 0)
            {
                RemoveAtContiMeasurePoint(ContiTestPoints.Count - 1);
                this.alTempTestPoints.RemoveAt(alTempTestPoints.Count-1);
            }
        }

        private void tsmiDelAllFlag_Click(object sender, EventArgs e)
        {
            ClearContiMeasurePoint();
        }

        private bool isInDot = true;

        

        private void tsmiShowAllTestPoint_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void tsmiSaveMultiPoint_Click(object sender, EventArgs e)
        {
             
            SaveAllMeasurePoint();
        }

        private void tsmiSaveOutMultiPoint_Click(object sender, EventArgs e)
        {
            isInDot = false;
            SaveAllMeasurePoint();
        }


        public FrmEditMultiPoints fed = null;

        public void tsmiEditMultiPoint_Click(object sender, EventArgs e)
        {
            if (sender == null)
            {
                if(fed != null)
                    fed.Close();
            }
            else
            {

                if (fed == null)
                {
                    if (this.Mode == CameraMode.Multiple)
                    {
                        fed = new FrmEditMultiPoints("Multiple");
                        fed.Show();
                    }
                    else if (this.Mode == CameraMode.dotMatrix)
                    {
                        fed = new FrmEditMultiPoints("dotMatrix");
                        fed.Show();
                    }
                    else if (this.Mode == CameraMode.matrixDot)
                    {
                        fed = new FrmEditMultiPoints("matrixDot");
                        fed.Show();
                    }
                    else if (this.Mode == CameraMode.dotDot)
                    {
                        fed = new FrmEditMultiPoints("dotDot");
                        fed.Show();
                    }
                }
            }
        }



        #endregion

        #region Thick

        protected Int32 _moveRateStepX;
        protected Int32 _moveRateStepY;
        protected Point _moveRateFocus;
        protected Point _moveRateTargetExpect;
        protected Point _moveRateTargetReal;

        /// <summary>
        /// 校正移动实际步长
        /// </summary>
        public Int32 MoveRateStepX
        {
            get
            {
                return _moveRateStepX;
            }
            set
            {
                _moveRateStepX = value;
            }
        }
        /// <summary>
        /// 校正移动实际步长
        /// </summary>
        public Int32 MoveRateStepY
        {
            get
            {
                return _moveRateStepY;
            }
            set
            {
                _moveRateStepY = value;
            }
        }
        /// <summary>
        /// 校正移动前焦点
        /// </summary>
        public Point MoveRateFocus
        {
            get
            {
                return _moveRateFocus;
            }
            set
            {
                _moveRateFocus = value;
            }
        }
        /// <summary>
        /// 校正移动目标点的期望位置
        /// </summary>
        public Point MoveRateTargetExpect
        {
            get
            {
                return _moveRateTargetExpect;
            }
            set
            {
                _moveRateTargetExpect = value;
            }
        }
        /// <summary>
        /// 校正移动目标点的实际位置
        /// </summary>
        public Point MoveRateTargetReal
        {
            get
            {
                return _moveRateTargetReal;
            }
            set
            {
                _moveRateTargetReal = value;
            }
        }

        /// <summary>
        /// 重计算移动比例
        /// </summary>
        /// <returns></returns>
        public void ReCalcMoveRate()
        {
            ReCalcMoveRate(_moveRateStepX, _moveRateStepY, _moveRateFocus, _moveRateTargetExpect, _moveRateTargetReal);
        }

        /// <summary>
        /// 重新计算移动比例
        /// </summary>
        /// <param name="stepX">欲从焦点位置（样品平台上和十字线中心对应的点）移动到期望位置时，平台在X方向实际移动的步长</param>
        /// <param name="stepY">欲从焦点位置移动到期望位置时，平台在Y方向实际移动的步长</param>
        /// <param name="a1">移动前，视频画面上的十字线中心坐标（也就是焦点位置）</param>
        /// <param name="b1">移动前，期望位置在画面上对应的坐标</param>
        /// <param name="b2">移动完毕后，期望位置在画面上对应的坐标</param>
        public void ReCalcMoveRate(Int32 stepX, Int32 stepY, Point focus, Point targetExpect, Point targetReal)
        {
            // 求移动后目标点期望位置和其实际位置在X方向的偏差
            Int32 offsetX = (targetReal.X - focus.X);
            // 求校正后的目标点X坐标
            Int32 targetBeforeMoveCorrectedX = targetExpect.X - offsetX;
            // 计算X方向的移动校正因子
            if (focus.X != targetBeforeMoveCorrectedX && stepX != 0)
            {
                MoveRateX = stepX / targetBeforeMoveCorrectedX - focus.X;
            }
            // 求移动后目标点期望位置和其实际位置在Y方向的偏差
            Int32 offsetY = (targetReal.Y - focus.Y);
            // 求校正后的目标点Y坐标
            Int32 targetBeforeMoveCorrectedY = targetExpect.Y - offsetY;
            // 计算Y方向的移动校正因子
            if (focus.Y != targetBeforeMoveCorrectedY && stepY != 0)
            {
                MoveRateY = stepY / targetBeforeMoveCorrectedY - focus.Y;
            }
        }

        #endregion

        private void tsmiSetMoveRate_VisibleChanged(object sender, EventArgs e)
        {
            if (tsmiSetMoveRate.Visible)
            {
                //if (this.Opened)
                //{
                //    tsmiSetMoveRate.Enabled = true;
                //}
                //else
                //{
                //    tsmiSetMoveRate.Enabled = false;
                //    return;
                //}
                tsmiMoveRateValueX.Text = this.MoveRateX.ToString();
                //tsmiMoveRateValueY.Text = this.MoveRateY.ToString();
                //tsmiProMeasureAdjustViewSideLength.Enabled =
                //    (User.Degree != UserDegree.Operator) && rbtnContiTestAdjustMode.Checked;
                //tsmiProMeasureDeleteLastPoint.Enabled = tsmiProMeasureDeleteAllPoints.Enabled =
                //    (((rbtnContiTestAdjustMode.Checked && tsmiProMeasureAdjustViewSideLength.Enabled && tsmiProMeasureAdjustViewSideLength.Checked)
                //    || rbtnContiTestGridMode.Checked || rbtnContiTestMultiMode.Checked)
                //    && myWebCamera.ContiTestPoints.Count > 0);
                //tsmiProMeasureSetMoveRate.Enabled = (User.Degree != UserDegree.Operator) && !tsmiProMeasureAdjustViewSideLength.Checked && rbtnContiTestAdjustMode.Checked;
            }
            else
            {
                double d;
                double.TryParse(tsmiMoveRateValueX.Text, out d);
                //tsmiMoveRateValueX.Text = d.ToString();
                this.MoveRateX = d;
                double.TryParse(tsmiMoveRateValueY.Text, out d);
                //tsmiMoveRateValueY.Text = d.ToString();
                this.MoveRateY = d;
            }
        }

        private void tsmiCoeeGraphWidthHeight_Click(object sender, EventArgs e)
        {
            tsmiCoeeGraphWidthHeight.Checked = !tsmiCoeeGraphWidthHeight.Checked;
            IsWidthHeightChecked = tsmiCoeeGraphWidthHeight.Checked;
        }
        //private FocusedAss FocusedAss = new FocusedAss();
        private void tsmiGraphy_Click(object sender, EventArgs e)
        {
            Bitmap bm = GrabImage();
            if (bm != null)
            {
                //double returnValue = FocusedAss.FocusedAssessment(bm);
                //MessageBox.Show(returnValue.ToString());
                savegraph.InitialDirectory = Application.StartupPath;
                savegraph.Filter = "Bitmap|*.bmp";
                savegraph.FileName = "";
                if (savegraph.ShowDialog(this) == DialogResult.OK)
                {
                    bm.Save(savegraph.FileName);
                }
                bm.Dispose();
            }

        }




        private void AutoSaveSamplePicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AutoSaveSamplePicToolStripMenuItem.Checked = !AutoSaveSamplePicToolStripMenuItem.Checked;
            this.AutoSaveSamplePic = AutoSaveSamplePicToolStripMenuItem.Checked;
            SaveSpecifiedNodeValue(AppDomain.CurrentDomain.BaseDirectory + "Camera.xml", "SaveImage", null, this.AutoSaveSamplePicToolStripMenuItem.Checked ? "1" : "0");
        }

        private void tsmiMoveRateValueX_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (tsmiMoveRateValueX.Text == "-") return;
                if (tsmiMoveRateValueX.Text == "") return;
                double x = double.Parse(tsmiMoveRateValueX.Text);
                SaveSpecifiedNodeValue(AppDomain.CurrentDomain.BaseDirectory + "Camera.xml", "MoveRateX", null, x.ToString());
                moveRateX = x;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void tsmiMoveRateValueY_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (tsmiMoveRateValueY.Text == "-") return;
                if (tsmiMoveRateValueY.Text == "") return;
                double y = double.Parse(tsmiMoveRateValueY.Text);
                SaveSpecifiedNodeValue(AppDomain.CurrentDomain.BaseDirectory + "Camera.xml", "MoveRateY", null, y.ToString());
                moveRateY = y;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tsmiDelCurrentFlag_Click(object sender, EventArgs e)
        {
            double rateWidth = (viewWidth == 0) ? 1 : (this.Width / viewWidth);
            double rateHeight = (viewHeight == 0) ? 1 : (this.Height / viewHeight);
            int halfFociWidth = Convert.ToInt32(Focis[fociIndex].Width * rateWidth / 2);
            int halfFociHeight = Convert.ToInt32(Focis[fociIndex].Height * rateHeight / 2);
            Rectangle rect = new Rectangle();
            for (int m = alContiTestPoints.Count - 1; m >= 0; m--)
            {
                rect = new Rectangle(((Point)alContiTestPoints[m]).X - halfFociWidth, ((Point)alContiTestPoints[m]).Y - halfFociHeight, halfFociWidth * 2, halfFociHeight * 2);
                if (rect.Contains(CurrentClickPoint))
                {
                    alContiTestPoints.RemoveAt(m);
                    alTempTestPoints.RemoveAt(m);
                }
            }
            UpdateOverlay();
        }
        public delegate void StartNextTest();
        public StartNextTest DoNextTest = null;
        private void btnOk_Click(object sender, EventArgs e)
        {
            //SendMessage(this.Parent.Handle, 500, 0, 0);
            if (DoNextTest != null)
            {
                DoNextTest();
            }
            this.GetImage(Application.StartupPath, DateTime.Now.ToString());
        }

        #region 多语言
        public static Dictionary<string, string> SkyrayCameraLangDic = new Dictionary<string, string>();
        private static void ReloadLang()
        {
            if (SkyrayCameraLangDic.Count > 0)
            {
                SkyrayCameraLangDic.Clear();
            }
            SkyrayCameraLangDic.Add("Flip0", "重置图像/180度旋转XY翻转");
            SkyrayCameraLangDic.Add("Flip1", "90度旋转无翻转/270度旋转XY翻转");
            SkyrayCameraLangDic.Add("Flip2", "180度旋转无翻转/XY翻转");
            SkyrayCameraLangDic.Add("Flip3", "270度旋转无翻转/90度旋转XY翻转");
            SkyrayCameraLangDic.Add("Flip4", "180度旋转Y翻转/X翻转");
            SkyrayCameraLangDic.Add("Flip5", "90度旋转X翻转/270度旋转Y翻转");
            SkyrayCameraLangDic.Add("Flip6", "Y翻转/180度旋转X翻转");
            SkyrayCameraLangDic.Add("Flip7", "90度旋转Y翻转/270度旋转X翻转");

            SkyrayCameraLangDic.Add("CameraIsNotOpen", "摄像头未打开");

            SkyrayCameraLangDic.Add("tsmiOpen", "打开");
            SkyrayCameraLangDic.Add("tsmiCameraProperty", "视频源");
            SkyrayCameraLangDic.Add("tsmiCameraFormat", "视频格式");
            SkyrayCameraLangDic.Add("tsmiCameraParam", "视频参数");
            SkyrayCameraLangDic.Add("AutoSaveSamplePicToolStripMenuItem", "自动保存样品图");
            SkyrayCameraLangDic.Add("tsmiGraphy", "抓图");
            SkyrayCameraLangDic.Add("tsmiClose", "关闭");
            SkyrayCameraLangDic.Add("tsmiDelCurrentFlag", "删除当前测量点");
            SkyrayCameraLangDic.Add("tsmiDelAllFlag", "删除所有测量点");
            SkyrayCameraLangDic.Add("tsmAdjustOrient", "调整方向");
            SkyrayCameraLangDic.Add("tsmiShowAllTestPoint", "显示测量点");
            SkyrayCameraLangDic.Add("tsmiSaveMultiPoint", "保存测量点");

            SkyrayCameraLangDic.Add("gbxFocus", "光斑中心");
            SkyrayCameraLangDic.Add("lblFocusX", "X坐标(mm)");
            SkyrayCameraLangDic.Add("lblFocusY", "Y坐标(mm)");
            SkyrayCameraLangDic.Add("lblSpotShape", "形  状");
            SkyrayCameraLangDic.Add("rbtnSpotShapeEllipse", "椭    圆");
            SkyrayCameraLangDic.Add("rbtnSpotShapeRectangle", "矩    形");
            SkyrayCameraLangDic.Add("gbxView", "画    面");
            SkyrayCameraLangDic.Add("lblViewWidth", "宽度(mm))");
            SkyrayCameraLangDic.Add("lblViewHeight", "高度(mm)");
            SkyrayCameraLangDic.Add("lblScaleUnit", "尺度单位(mm)");
            SkyrayCameraLangDic.Add("btnDefault", "默  认");
            SkyrayCameraLangDic.Add("btnAccept", "确  定");


            SkyrayCameraLangDic.Add("lblVideoDev", "视频设备");
            SkyrayCameraLangDic.Add("lblCameraSize", "像 素");
            SkyrayCameraLangDic.Add("btnCancel", "取 消");

            SkyrayCameraLangDic.Add("lblmultiName", "多点名称：");
            SkyrayCameraLangDic.Add("btnDel", "删  除");
            SkyrayCameraLangDic.Add("btnEdit", "编  辑");
            SkyrayCameraLangDic.Add("btnSave", "保  存");
            SkyrayCameraLangDic.Add("btnDeletePoint", "删除点");
            SkyrayCameraLangDic.Add("lblNewPointName", "新名称");


        }
        #endregion
        #region 旋转翻转
        private void initialFilpStrip()
        {
            if (!SkyrayCameraLangDic.ContainsKey("Flip0")) SkyrayCameraLangDic.Add("Flip0", "重置图像/180度旋转XY翻转");
            if (!SkyrayCameraLangDic.ContainsKey("Flip1")) SkyrayCameraLangDic.Add("Flip1", "90度旋转无翻转/270度旋转XY翻转");
            if (!SkyrayCameraLangDic.ContainsKey("Flip2")) SkyrayCameraLangDic.Add("Flip2", "180度旋转无翻转/XY翻转");
            if (!SkyrayCameraLangDic.ContainsKey("Flip3")) SkyrayCameraLangDic.Add("Flip3", "270度旋转无翻转/90度旋转XY翻转");
            if (!SkyrayCameraLangDic.ContainsKey("Flip4")) SkyrayCameraLangDic.Add("Flip4", "180度旋转Y翻转/X翻转");
            if (!SkyrayCameraLangDic.ContainsKey("Flip5")) SkyrayCameraLangDic.Add("Flip5", "90度旋转X翻转/270度旋转Y翻转");
            if (!SkyrayCameraLangDic.ContainsKey("Flip6")) SkyrayCameraLangDic.Add("Flip6", "Y翻转/180度旋转X翻转");
            if (!SkyrayCameraLangDic.ContainsKey("Flip7")) SkyrayCameraLangDic.Add("Flip7", "90度旋转Y翻转/270度旋转X翻转");

            tsmAdjustOrient.DropDownItems.Clear();
            ToolStripMenuItem tsMi = new ToolStripMenuItem(SkyrayCameraLangDic["Flip0"]);
            tsMi.Name = "tmsResetFlip";
            tsMi.Tag = 0;
            tsMi.Click += new EventHandler(tsMi_Click);
            tsmAdjustOrient.DropDownItems.Add(tsMi);

            tsMi = new ToolStripMenuItem(SkyrayCameraLangDic["Flip1"]);
            tsMi.Name = "tmsRotate90FlipNone";
            tsMi.Tag = 1;
            tsMi.Click += new EventHandler(tsMi_Click);
            tsmAdjustOrient.DropDownItems.Add(tsMi);

            tsMi = new ToolStripMenuItem(SkyrayCameraLangDic["Flip2"]);
            tsMi.Name = "tmsFlipXY";
            tsMi.Tag = 2;
            tsMi.Click += new EventHandler(tsMi_Click);
            tsmAdjustOrient.DropDownItems.Add(tsMi);


            tsMi = new ToolStripMenuItem(SkyrayCameraLangDic["Flip3"]);
            tsMi.Name = "tmsRotate270FlipNone";
            tsMi.Tag = 3;
            tsMi.Click += new EventHandler(tsMi_Click);
            tsmAdjustOrient.DropDownItems.Add(tsMi);

            tsMi = new ToolStripMenuItem(SkyrayCameraLangDic["Flip4"]);
            tsMi.Name = "tmsFlipX";
            tsMi.Tag = 4;
            tsMi.Click += new EventHandler(tsMi_Click);
            tsmAdjustOrient.DropDownItems.Add(tsMi);

            tsMi = new ToolStripMenuItem(SkyrayCameraLangDic["Flip5"]);
            tsMi.Name = "tmsRotate90FlipX";
            tsMi.Tag = 5;
            tsMi.Click += new EventHandler(tsMi_Click);
            tsmAdjustOrient.DropDownItems.Add(tsMi);

            tsMi = new ToolStripMenuItem(SkyrayCameraLangDic["Flip6"]);
            tsMi.Name = "tmsFlipY";
            tsMi.Tag = 6;
            tsMi.Click += new EventHandler(tsMi_Click);
            tsmAdjustOrient.DropDownItems.Add(tsMi);


            tsMi = new ToolStripMenuItem(SkyrayCameraLangDic["Flip7"]);
            tsMi.Name = "tmsRotate90FlipY";
            tsMi.Tag = 7;
            tsMi.Click += new EventHandler(tsMi_Click);
            tsmAdjustOrient.DropDownItems.Add(tsMi);
        }


        public void InitalFrmLanguage()
        {
            this.tsmiOpen.Text = SkyrayCameraLangDic.ContainsKey("tsmiOpen") ? SkyrayCameraLangDic["tsmiOpen"] : "打开";
            this.tsmiCameraProperty.Text = SkyrayCameraLangDic.ContainsKey("tsmiCameraProperty") ? SkyrayCameraLangDic["tsmiCameraProperty"] : "视频源";
            this.tsmiCameraFormat.Text = SkyrayCameraLangDic.ContainsKey("tsmiCameraFormat") ? SkyrayCameraLangDic["tsmiCameraFormat"] : "视频格式";
            this.tsmiCameraParam.Text = SkyrayCameraLangDic.ContainsKey("tsmiCameraParam") ? SkyrayCameraLangDic["tsmiCameraParam"] : "视频参数";

            this.AutoSaveSamplePicToolStripMenuItem.Text = SkyrayCameraLangDic.ContainsKey("AutoSaveSamplePicToolStripMenuItem") ? SkyrayCameraLangDic["AutoSaveSamplePicToolStripMenuItem"] : "自动保存样品图";
            this.tsmiGraphy.Text = SkyrayCameraLangDic.ContainsKey("tsmiGraphy") ? SkyrayCameraLangDic["tsmiGraphy"] : "抓图";

            this.tsmiClose.Text = SkyrayCameraLangDic.ContainsKey("tsmiClose") ? SkyrayCameraLangDic["tsmiClose"] : "关闭";
            this.tsmiDelCurrentFlag.Text = SkyrayCameraLangDic.ContainsKey("tsmiDelCurrentFlag") ? SkyrayCameraLangDic["tsmiDelCurrentFlag"] : "删除当前测量点";


            this.tsmiDelAllFlag.Text = SkyrayCameraLangDic.ContainsKey("tsmiDelAllFlag") ? SkyrayCameraLangDic["tsmiDelAllFlag"] : "删除所有测量点";
            this.tsmAdjustOrient.Text = SkyrayCameraLangDic.ContainsKey("tsmAdjustOrient") ? SkyrayCameraLangDic["tsmAdjustOrient"] : "调整方向";
            this.tsmiShowAllTestPoint.Text = SkyrayCameraLangDic.ContainsKey("tsmiShowAllTestPoint") ? SkyrayCameraLangDic["tsmiShowAllTestPoint"] : "显示测量点";
            this.tsmiSaveMultiPoint.Text = SkyrayCameraLangDic.ContainsKey("tsmiSaveMultiPoint") ? SkyrayCameraLangDic["tsmiSaveMultiPoint"] : "保存测量点";
            this.tsmiSaveOutMultiPoint.Text = SkyrayCameraLangDic.ContainsKey("tsmiSaveOutMultiPoint") ? SkyrayCameraLangDic["tsmiSaveOutMultiPoint"] : "保存外测量点";



            if (tsmAdjustOrient.DropDownItems.Count > 0)
            {
                foreach (ToolStripMenuItem ts in tsmAdjustOrient.DropDownItems)
                {
                    int inttype = (int)ts.Tag;
                    switch (inttype)
                    {
                        case 0:
                            ts.Text = SkyrayCameraLangDic["Flip0"];
                            break;
                        case 1:
                            ts.Text = SkyrayCameraLangDic["Flip1"];
                            break;
                        case 2:
                            ts.Text = SkyrayCameraLangDic["Flip2"];
                            break;
                        case 3:
                            ts.Text = SkyrayCameraLangDic["Flip3"];
                            break;
                        case 4:
                            ts.Text = SkyrayCameraLangDic["Flip4"];
                            break;
                        case 5:
                            ts.Text = SkyrayCameraLangDic["Flip5"];
                            break;
                        case 6:
                            ts.Text = SkyrayCameraLangDic["Flip6"];
                            break;
                        case 7:
                            ts.Text = SkyrayCameraLangDic["Flip7"];
                            break;
                        default:
                            break;


                    }
                }

            }

        }

        void tsMi_Click(object sender, EventArgs e)
        {
            if (this.VideoSource == null)
                return;
            ToolStripMenuItem but = sender as ToolStripMenuItem;
            videoRotateFlip = (RotateFlipType)but.Tag;
            RefreshFlipItems();
            FlipMethod();

            //throw new NotImplementedException();
        }

        public void RefreshFlipItems()
        {
            foreach (ToolStripMenuItem ts in tsmAdjustOrient.DropDownItems)
            {
                if ((RotateFlipType)ts.Tag == videoRotateFlip) ts.Checked = true;
                else ts.Checked = false;
            }
        }

        public void FlipMethod()
        {
            this.NewFrame -= new NewFrameHandler(videoSourcePlayer_NewFrame);
            this.NewFrame += new NewFrameHandler(videoSourcePlayer_NewFrame);
        }
        object drawbmp = new object();
        Bitmap newBitmap = null;
        int index = 0;

        public bool catchFlag = false;

        private void videoSourcePlayer_NewFrame(object sender, ref Bitmap image)
        {
            lock (drawbmp)
            {
                if (image != null)
                {
                    try
                    {
                        
                        image.RotateFlip(videoRotateFlip);
                        Graphics g = Graphics.FromImage(image);
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    
                   
                        if (_xWidthRate < 1 || _yHeightRate < 1)
                        {
                            if (newBitmap == null) newBitmap = new Bitmap(image.Width, image.Height);
                            Graphics gTemp = Graphics.FromImage(newBitmap);
                            Rectangle rectSource = new Rectangle((int)(image.Width * _xStartRate), (int)(image.Height * _yStartRate), (int)(image.Width * _xWidthRate), (int)(image.Height * _yHeightRate));

                            // Rectangle rectSource = new Rectangle((int)(image.Width * m_XStartRate), (int)(image.Height * m_YStartRate), (int)(image.Width * m_XWidthRate), (int)(image.Height * m_YHeightRate));
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

                        if (Skyray.EDX.Common.CameraRefMotor.IsShowDefine)
                        {

                            int halfw = image.Width / 2;
                            int halfh = image.Height / 2;
                            Bitmap newBitmap = new Bitmap((int)(halfw * 2 / focusSize), (int)(halfh * 2 / focusSize));
                            Graphics gTemp = Graphics.FromImage(newBitmap);
                            Rectangle rectSource = new Rectangle((int)(fociBmpX - halfw / focusSize), (int)(fociBmpY - halfh / focusSize), (int)(halfw * 2 / focusSize), (int)(halfh * 2 / focusSize));
                            Rectangle rectDest = new Rectangle(0, 0, newBitmap.Width, newBitmap.Height);
                            gTemp.DrawImage(image, rectDest, rectSource, GraphicsUnit.Pixel);
                            gTemp.Dispose();
                            camerCurrent = BitmapFocusedAssessmentC_Q(1, 0, newBitmap.GetHbitmap());

                        }
                        if (Skyray.EDX.Common.CameraRefMotor.IsTransLocked && !Skyray.EDX.Common.CameraRefMotor.IsReceiveLocked)//需要取图，并且不是在取谱时时才取
                        {
                            Console.WriteLine("传送音像" + index.ToString());
                            index++;
                            if (Skyray.EDX.Common.Component.MotorAdvance.CameraBitmap != null)
                            {
                                Skyray.EDX.Common.Component.MotorAdvance.CameraBitmap.Dispose();
                                Skyray.EDX.Common.Component.MotorAdvance.CameraBitmap = null;
                            }
                            Skyray.EDX.Common.Component.MotorAdvance.CameraBitmap = ((Bitmap)image.Clone()); ;
                            Skyray.EDX.Common.CameraRefMotor.IsReceiveLocked = true;
                        }
                        if (m_OverlayBmp != null)
                        {
                            if (!catchFlag)
                                g.DrawImage(m_OverlayBmp, 0, 0, image.Width, image.Height);
                        }
                        g.Dispose();
                        // }
                        //bitmap.Dispose();
                    }
                    catch
                    { }
                }
            }
        }

        #endregion

        public void refreshCameraInfo(string Info)
        {
            try
            {
                this.BeginInvoke(new Action(() =>
                { this.lblCameraInfo.Text = Info; }));
            }
            catch
            { }
        }

        private void cmsCamera_Opening(object sender, CancelEventArgs e)
        {

        }

        /// <summary>
        /// 压缩图片至200 Kb以下
        /// </summary>
        /// <param name="img">图片</param>
        /// <param name="format">图片格式</param>
        /// <param name="targetLen">压缩后大小</param>
        /// <param name="srcLen">原始大小</param>
        /// <returns>压缩后的图片</returns>
        public Image ZipImage(Image img, ImageFormat format, long targetLen, long srcLen)
        {
            //设置大小偏差幅度 10kb
            const long nearlyLen = 10240;
            //内存流  如果参数中原图大小没有传递 则使用内存流读取
            var ms = new MemoryStream();
            if (0 == srcLen)
            {
                img.Save(ms, format);
                srcLen = ms.Length;
            }

            //单位 由Kb转为byte 若目标大小高于原图大小，则满足条件退出
            targetLen *= 1024;
            if (targetLen > srcLen)
            {
                ms.SetLength(0);
                ms.Position = 0;
                img.Save(ms, format);
                img = Image.FromStream(ms);
                return img;
            }

            //获取目标大小最低值
            var exitLen = targetLen - nearlyLen;

            //初始化质量压缩参数 图像 内存流等
            var quality = (long)Math.Floor(100.00 * targetLen / srcLen);
            var parms = new EncoderParameters(1);

            //获取编码器信息
            ImageCodecInfo formatInfo = null;
            var encoders = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo icf in encoders)
            {
                if (icf.FormatID == format.Guid)
                {
                    formatInfo = icf;
                    break;
                }
            }

            //使用二分法进行查找 最接近的质量参数
            long startQuality = quality;
            long endQuality = 100;
            quality = (startQuality + endQuality) / 2;

            while (true)
            {
                //设置质量
                parms.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

                //清空内存流 然后保存图片
                ms.SetLength(0);
                ms.Position = 0;
                img.Save(ms, formatInfo, parms);

                //若压缩后大小低于目标大小，则满足条件退出
                if (ms.Length >= exitLen && ms.Length <= targetLen)
                {
                    break;
                }
                else if (startQuality >= endQuality) //区间相等无需再次计算
                {
                    break;
                }
                else if (ms.Length < exitLen) //压缩过小,起始质量右移
                {
                    startQuality = quality;
                }
                else //压缩过大 终止质量左移
                {
                    endQuality = quality;
                }

                //重新设置质量参数 如果计算出来的质量没有发生变化，则终止查找。这样是为了避免重复计算情况{start:16,end:18} 和 {start:16,endQuality:17}
                var newQuality = (startQuality + endQuality) / 2;
                if (newQuality == quality)
                {
                    break;
                }
                quality = newQuality;
                //Console.WriteLine("start:{0} end:{1} current:{2}", startQuality, endQuality, quality);
            }
            img = Image.FromStream(ms);
            return img;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction">调整系数 1 放大, -1缩小</param>
        public void ZoomInOut(int direction)
        {
            bShowCoeff = true;
            if (direction == 0)
            {
                //m_XWidthRate = m_YHeightRate = 1;
                //m_XStartRate = m_YStartRate = 0;
                _currentZoomCoeff = 1;
                _cutPicCoeff = 1;
                _xWidthRate = m_XWidthRate;
                _yHeightRate = m_YHeightRate;
                _xStartRate = m_XStartRate;
                _yStartRate = m_YStartRate;

            }
            else
            {
                float coeff = zoomCoeff * direction;
                _currentZoomCoeff = (float)Math.Round(_currentZoomCoeff + coeff, 1);  //放大倍数  1.1 1.2

                _cutPicCoeff = 1 / _currentZoomCoeff;

                if (_cutPicCoeff >= 1)
                {
                    _xWidthRate = m_XWidthRate;
                    _yHeightRate = m_YHeightRate;
                    _xStartRate = m_XStartRate;
                    _yStartRate = m_YStartRate;
                    _currentZoomCoeff = 1;
                }
                else
                {
                    //double halfFociWidth = Focis[fociIndex].Width * rateWidth / 2;
                    //double halfFociHeight = Convert.ToInt32(Focis[fociIndex].Height * rateHeight / 2);


                    _xWidthRate = m_XWidthRate * _cutPicCoeff;
                    _yHeightRate = m_YHeightRate * _cutPicCoeff;
                    //_xStartRate = (float)Math.Round((1 - _xWidthRate) / 2, 3);
                    //_yStartRate = (float)Math.Round((1 - _yHeightRate) / 2, 3);

                    _xStartRate = (float)Math.Round((1 - _xWidthRate) * fociX / viewWidth, 8);
                    _yStartRate = (float)Math.Round((1 - _yHeightRate) * fociY / viewHeight, 8);
                }

            }

            timer.Enabled = true;
            showTimes = 0;
            UpdateOverlay();
        }



        [DllImport("EDXRF.dll", EntryPoint = "BitmapFocusedAssessmentC_Q", CallingConvention = CallingConvention.StdCall)]
        internal static extern double BitmapFocusedAssessmentC_Q(double k1, double k2, IntPtr hbitmap);

       

    }
}
