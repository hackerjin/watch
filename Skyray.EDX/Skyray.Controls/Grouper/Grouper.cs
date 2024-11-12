/*
 [PLEASE DO NOT MODIFY THIS HEADER INFORMATION]---------------------
 Title: Grouper
 Description: A rounded groupbox with special painting features. 
 Date Created: December 17, 2005
 Author: Adam Smith
 Author Email: ibulwark@hotmail.com
 Websites: http://www.ebadgeman.com | http://www.codevendor.com
 
 Version History:
 1.0a - Beta Version - Release Date: December 17, 2005 
 -------------------------------------------------------------------
 */

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;

namespace Skyray.Controls
{
    /// <summary>A special custom rounding GroupBox with many painting features.</summary>
    //[ToolboxBitmap(typeof(Grouper), "Skyray.AFSControls.Images.Grouper.bmp")] //���ù�����ͼ��
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public class Grouper : System.Windows.Forms.UserControl
    {
        #region Enumerations

        /// <summary>A special gradient enumeration.</summary>
        public enum GroupBoxGradientMode
        {
            /// <summary>Specifies no gradient mode.</summary>
            None = 4,

            /// <summary>Specifies a gradient from upper right to lower left.</summary>
            BackwardDiagonal = 3,

            /// <summary>Specifies a gradient from upper left to lower right.</summary>
            ForwardDiagonal = 2,

            /// <summary>Specifies a gradient from left to right.</summary>
            Horizontal = 0,

            /// <summary>Specifies a gradient from top to bottom.</summary>
            Vertical = 1
        }
        //ADD by WZW 2008-12-16
        public enum GroupBoxAlignMode
        {
            /// <summary>�����.</summary>
            Left = 0,

            /// <summary>�м����.</summary>
            Center = 1,

            /// <summary>�Ҷ���.</summary>
            Right = 2
        }


        #endregion

        #region Variables

        private System.ComponentModel.Container components = null;
        private int V_RoundCorners = 4;
        private string V_GroupTitle = "The Grouper";
        private System.Drawing.Color V_BorderColor = Color.LightSteelBlue;
        private float V_BorderThickness = 1;
        private bool V_ShadowControl = false;
        private System.Drawing.Color V_BackgroundColor = Color.Transparent;
        private System.Drawing.Color V_BackgroundGradientColor = Color.Transparent;
        private GroupBoxGradientMode V_BackgroundGradientMode = GroupBoxGradientMode.None;
        private System.Drawing.Color V_ShadowColor = Color.DarkGray;
        private int V_ShadowThickness = 3;
        private System.Drawing.Image V_GroupImage = null;
        private System.Drawing.Color V_CustomGroupBoxColor = Color.Transparent;
        private bool V_PaintGroupBox = false;
        private System.Drawing.Color V_BackColor = Color.Transparent;


        //Add BY WZW 2008-12-17 
        private bool V_ShowTileRectangle = false;//�Ƿ���Ʊ���߿�

        private GroupBoxAlignMode V_GroupBoxAlignMode = GroupBoxAlignMode.Left;//����߿���뷽ʽ
        private int V_XTrans = 0;//��¼ƽ�ƾ���
        private int V_TextLineSpace = 2;//��������ı���GroupBox�����ϱ��߾���
        private int V_TitleLeftSpace = 18;//�������ͼƬ��GroupBox�����ϱ��߾���
        private int V_HeaderRoundCorners = 4;
        private bool V_BorderTopOnly = false;
        #endregion

        #region Properties

        //Add By WZW 2008-12-16 Edit BY WZW 2008-12-17

        /// <summary>�������ͼƬ��GroupBox���ϱ��߾���</summary>

        [Category("Appearance"), Description("�������GroupBox����߾���")]
        public int TitleLeftSpace
        {
            get { return V_TitleLeftSpace; }
            set
            {
                if (value <= 24 && value >= -24)
                {
                    V_TitleLeftSpace = value;
                }
                else
                {
                    V_TitleLeftSpace = 0;
                }
                this.Invalidate();
            }
        }
        //Add by wzw 2009-03-27
        [Category("Appearance"), Description("ֻ��ʾ�ϱ߿�")]
        public bool BorderTopOnly
        {
            get { return this.V_BorderTopOnly; }
            set
            {
                this.V_BorderTopOnly = value;
                this.Invalidate();
            }
        }

        /// <summary>��������ı���GroupBox�����ϱ��߾���,�������Ʊ߿�ʱ��Ч</summary>
        [Category("Appearance"), Description("��������ı���GroupBox�����ϱ��߾���")]
        public int TextLineSpace
        {
            get { return V_TextLineSpace; }
            set
            {
                if (value >= -8 && value <= 8)
                {
                    V_TextLineSpace = value;
                }
                else
                {
                    V_TextLineSpace = 0;
                }
                this.Invalidate();
            }
        }
        /// <summary>�������뷽ʽ</summary>
        [Category("Appearance"), Description("�������뷽ʽ")]
        public GroupBoxAlignMode GroupBoxAlign
        {
            get { return V_GroupBoxAlignMode; }
            set { V_GroupBoxAlignMode = value; this.Invalidate(); }
        }

        /// <summary>�Ƿ���ʾ�������</summary>
        [Category("Appearance"), Description("�Ƿ���ʾ�������")]
        [DefaultValue(false)]
        public bool ShowTileRectangle { get { return V_ShowTileRectangle; } set { V_ShowTileRectangle = value; this.Invalidate(); } }

        /// <summary>This feature will paint the background color of the control.</summary>
        [Category("Appearance"), Description("This feature will paint the background color of the control.")]
        public override System.Drawing.Color BackColor { get { return V_BackColor; } set { V_BackColor = value; this.Invalidate(); } }

        /// <summary>This feature will paint the group title background to the specified color if PaintGroupBox is set to true.</summary>
        [Category("Appearance"), Description("This feature will paint the group title background to the specified color if PaintGroupBox is set to true.")]
        public System.Drawing.Color CustomGroupBoxColor { get { return V_CustomGroupBoxColor; } set { V_CustomGroupBoxColor = value; this.Invalidate(); } }

        /// <summary>This feature will paint the group title background to the CustomGroupBoxColor.</summary>
        [Category("Appearance"), Description("This feature will paint the group title background to the CustomGroupBoxColor.")]
        public bool PaintGroupBox { get { return V_PaintGroupBox; } set { V_PaintGroupBox = value; this.Invalidate(); } }

        /// <summary>This feature can add a 16 x 16 image to the group title bar.</summary>
        [Category("Appearance"), Description("This feature can add a 16 x 16 image to the group title bar.")]
        public System.Drawing.Image GroupImage { get { return V_GroupImage; } set { V_GroupImage = value; this.Invalidate(); } }

        /// <summary>This feature will change the control's shadow color.</summary>
        [Category("Appearance"), Description("This feature will change the control's shadow color.")]
        public System.Drawing.Color ShadowColor { get { return V_ShadowColor; } set { V_ShadowColor = value; this.Invalidate(); } }

        /// <summary>This feature will change the size of the shadow border.</summary>
        [Category("Appearance"), Description("This feature will change the size of the shadow border.")]
        public int ShadowThickness
        {
            get { return V_ShadowThickness; }
            set
            {
                if (value > 10)
                {
                    V_ShadowThickness = 10;
                }
                else
                {
                    if (value < 1) { V_ShadowThickness = 1; }
                    else { V_ShadowThickness = value; }
                }

                this.Invalidate();
            }
        }


        /// <summary>This feature will change the group control color. This color can also be used in combination with BackgroundGradientColor for a gradient paint.</summary>
        [Category("Appearance"), Description("This feature will change the group control color. This color can also be used in combination with BackgroundGradientColor for a gradient paint.")]
        public System.Drawing.Color BackgroundColor { get { return V_BackgroundColor; } set { V_BackgroundColor = value; this.Invalidate(); } }

        /// <summary>This feature can be used in combination with BackgroundColor to create a gradient background.</summary>
        [Category("Appearance"), Description("This feature can be used in combination with BackgroundColor to create a gradient background.")]
        public System.Drawing.Color BackgroundGradientColor { get { return V_BackgroundGradientColor; } set { V_BackgroundGradientColor = value; this.Invalidate(); } }

        /// <summary>This feature turns on background gradient painting.</summary>
        [Category("Appearance"), Description("This feature turns on background gradient painting.")]
        public GroupBoxGradientMode BackgroundGradientMode { get { return V_BackgroundGradientMode; } set { V_BackgroundGradientMode = value; this.Invalidate(); } }


        /// <summary>This feature will round the corners of the control.</summary>
        [Category("Appearance"), Description("This feature will round the corners of the control.")]
        public int RoundCorners
        {
            get { return V_RoundCorners; }
            set
            {
                if (value > 20)
                {
                    V_RoundCorners = 20;
                }
                else
                {
                    if (value < 1) { V_RoundCorners = 1; }
                    else { V_RoundCorners = value; }
                }

                this.Invalidate();
            }
        }
        //Add by WZW 2008-12-25
        /// <summary>This feature will round the Header corners of the control.</summary>
        [Category("Appearance"), Description("This feature will round the Header corners of the control.")]
        public int HeaderRoundCorners
        {
            get { return V_HeaderRoundCorners; }
            set
            {
                if (value > 20)
                {
                    V_HeaderRoundCorners = 20;
                }
                else
                {
                    if (value < 1) { V_HeaderRoundCorners = 1; }
                    else { V_HeaderRoundCorners = value; }
                }

                this.Invalidate();
            }
        }
        /// <summary>This feature will add a group title to the control.</summary>
        [Category("Appearance"), Description("This feature will add a group title to the control.")]
        public string GroupTitle { get { return V_GroupTitle; } set { V_GroupTitle = value; this.Invalidate(); } }

        /// <summary>This feature will allow you to change the color of the control's border.</summary>
        [Category("Appearance"), Description("This feature will allow you to change the color of the control's border.")]
        public System.Drawing.Color BorderColor { get { return V_BorderColor; } set { V_BorderColor = value; this.Invalidate(); } }

        /// <summary>This feature will allow you to set the control's border size.</summary>
        [Category("Appearance"), Description("This feature will allow you to set the control's border size.")]
        public float BorderThickness
        {
            get { return V_BorderThickness; }
            set
            {
                if (value > 3)
                {
                    V_BorderThickness = 3;
                }
                else
                {
                    if (value < 1) { V_BorderThickness = 1; }
                    else { V_BorderThickness = value; }
                }
                this.Invalidate();
            }
        }

        /// <summary>This feature will allow you to turn on control shadowing.</summary>
        [Category("Appearance"), Description("This feature will allow you to turn on control shadowing.")]
        public bool ShadowControl { get { return V_ShadowControl; } set { V_ShadowControl = value; this.Invalidate(); } }

        #endregion

        #region Constructor

        /// <summary>This method will construct a new GroupBox control.</summary>
        public Grouper()
        {
            InitializeStyles();
            InitializeGroupBox();
        }


        #endregion

        #region DeConstructor

        /// <summary>This method will dispose of the GroupBox control.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing) { if (components != null) { components.Dispose(); } }
            base.Dispose(disposing);
        }


        #endregion

        #region Initialization

        /// <summary>This method will initialize the controls custom styles.</summary>
        private void InitializeStyles()
        {
            //Set the control styles----------------------------------
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //--------------------------------------------------------
        }


        /// <summary>This method will initialize the GroupBox control.</summary>
        private void InitializeGroupBox()
        {
            components = new System.ComponentModel.Container();
            this.Resize += new EventHandler(GroupBox_Resize);
            this.DockPadding.All = 0;
            this.Name = "GroupBox";
            this.GroupBoxAlign = GroupBoxAlignMode.Center;
            this.Size = new System.Drawing.Size(368, 288);

        }

        #endregion

        #region Protected Methods

        /// <summary>Overrides the OnPaint method to paint control.</summary>
        /// <param name="e">The paint event arguments.</param>        
        protected override void OnPaint(PaintEventArgs e)
        {
            PaintGroupText(e.Graphics);
            PaintBack(e.Graphics);
        }

        #endregion

        #region Private Methods
        /// <summary>This method will paint the group title.</summary>
        /// <param name="g">The paint event graphics object.</param>
        private void PaintGroupText(System.Drawing.Graphics g)
        {
            //Check if string has something-------------
            if (this.GroupTitle == string.Empty) { return; }
            //------------------------------------------

            //Set Graphics smoothing mode to Anit-Alias-- 
            g.SmoothingMode = SmoothingMode.AntiAlias;
            //-------------------------------------------

            //Declare Variables------------------
            SizeF StringSize = g.MeasureString(this.GroupTitle, this.Font);
            Size StringSize2 = StringSize.ToSize();


            if (this.GroupImage != null) { StringSize2.Width += 18; }
            //int ArcWidth = this.RoundCorners;
            //int ArcHeight = this.RoundCorners;
            int ArcWidth = this.HeaderRoundCorners;
            int ArcHeight = this.HeaderRoundCorners;
            //int intX1 = 0;
            //int intX2 = 0;

            //int ArcX1 = 10;
            //int ArcX2 = (StringSize2.Width + 34) - (ArcWidth + 1);
            int ArcX1 = 20;
            int ArcX2 = (StringSize2.Width + 34) - (ArcWidth + 1);
            int ArcY1 = 2;
            int ArcY2 = 22 - (ArcHeight + 1);

            //Add by WZW 2008-12-16 ����GroupTitleBox���뷽ʽ ===Start
            int intMidX = this.Size.Width / 2;
            int intMidX1 = (ArcX2 - ArcX1) / 2;
            int CustomStringWidth = (this.GroupImage != null) ? 44 : 28;
            V_XTrans = intMidX - intMidX1 - ArcX1;//X����ƽ�ƾ���
            int intTitleTextSpace = CustomStringWidth - ArcX1;
            //�м����
            if (this.V_GroupBoxAlignMode == GroupBoxAlignMode.Center)
            {
                ArcX1 = intMidX - intMidX1;
                ArcX2 = intMidX + intMidX1;
                CustomStringWidth = ArcX1 + intTitleTextSpace;
            }
            else if (this.V_GroupBoxAlignMode == GroupBoxAlignMode.Right)
            {
                ArcX1 = intMidX + intMidX - ArcX2;
                ArcX2 = ArcX1 + intMidX1 + intMidX1;
                CustomStringWidth = ArcX1 + intTitleTextSpace;
            }
            //����������߾���
            ArcX1 -= this.TitleLeftSpace;
            ArcX2 -= this.TitleLeftSpace;

            //Add by WZW 2008-12-16 ����GroupTitleBox���뷽ʽ ===End
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            System.Drawing.Brush BorderBrush = new SolidBrush(this.BorderColor);
            System.Drawing.Pen BorderPen = new Pen(BorderBrush, this.BorderThickness);
            System.Drawing.Drawing2D.LinearGradientBrush BackgroundGradientBrush = null;
            System.Drawing.Brush BackgroundBrush = (this.PaintGroupBox) ? new SolidBrush(this.CustomGroupBoxColor) : new SolidBrush(this.BackgroundColor);
            System.Drawing.SolidBrush TextColorBrush = new SolidBrush(this.ForeColor);
            System.Drawing.SolidBrush ShadowBrush = null;
            System.Drawing.Drawing2D.GraphicsPath ShadowPath = null;
            //-----------------------------------

            //Check if shadow is needed----------
            if (this.ShadowControl)
            {
                ShadowBrush = new SolidBrush(this.ShadowColor);
                ShadowPath = new System.Drawing.Drawing2D.GraphicsPath();
                ShadowPath.AddArc(ArcX1 + (this.ShadowThickness - 1), ArcY1 + (this.ShadowThickness - 1), ArcWidth, ArcHeight, 180, GroupBoxConstants.SweepAngle); // Top Left
                ShadowPath.AddArc(ArcX2 + (this.ShadowThickness - 1), ArcY1 + (this.ShadowThickness - 1), ArcWidth, ArcHeight, 270, GroupBoxConstants.SweepAngle); //Top Right
                ShadowPath.AddArc(ArcX2 + (this.ShadowThickness - 1), ArcY2 + (this.ShadowThickness - 1), ArcWidth, ArcHeight, 360, GroupBoxConstants.SweepAngle); //Bottom Right
                ShadowPath.AddArc(ArcX1 + (this.ShadowThickness - 1), ArcY2 + (this.ShadowThickness - 1), ArcWidth, ArcHeight, 90, GroupBoxConstants.SweepAngle); //Bottom Left
                ShadowPath.CloseAllFigures();

                //Paint Rounded Rectangle------------
                g.FillPath(new SolidBrush(Color.Transparent), ShadowPath);
                //g.FillPath(ShadowBrush, ShadowPath);
                //-----------------------------------
            }
            //-----------------------------------
            //Edit BY WZW 2008-12-16
            //�Ƿ���ʾ����߿�------
            if (this.V_ShowTileRectangle)
            {
                //����title�߿�·��
                path.AddArc(ArcX1, ArcY1, ArcWidth, ArcHeight, 180, GroupBoxConstants.SweepAngle); // Top Left
                path.AddArc(ArcX2, ArcY1, ArcWidth, ArcHeight, 270, GroupBoxConstants.SweepAngle); //Top Right
                path.AddArc(ArcX2, ArcY2, ArcWidth, ArcHeight, 360, GroupBoxConstants.SweepAngle); //Bottom Right
                path.AddArc(ArcX1, ArcY2, ArcWidth, ArcHeight, 90, GroupBoxConstants.SweepAngle); //Bottom Left
                path.CloseAllFigures();
                //��Title�߿�
                g.DrawPath(BorderPen, path);
            }

            //-----------------------------------

            //Check if Gradient Mode is enabled--
            if (this.PaintGroupBox)
            {
                //Paint Rounded Rectangle------------
                g.FillPath(BackgroundBrush, path);
                //-----------------------------------
            }
            else
            {
                if (this.BackgroundGradientMode == GroupBoxGradientMode.None)
                {
                    //Paint Rounded Rectangle------------
                    g.FillPath(BackgroundBrush, path);
                    //-----------------------------------
                }
                else
                {
                    BackgroundGradientBrush = new LinearGradientBrush(new Rectangle(0, 0, this.Width, this.Height), this.BackgroundColor, this.BackgroundGradientColor, (LinearGradientMode)this.BackgroundGradientMode);

                    //Paint Rounded Rectangle------------
                    g.FillPath(BackgroundGradientBrush, path);
                    //-----------------------------------
                }
            }
            //-----------------------------------

            //����title����-------------------------
            //int CustomStringWidth = (this.GroupImage != null) ? 44 : 28;
            //if(this.GroupImage!=null)
            //{
            //    CustomStringWidth += 2;
            //}

            //g.DrawString(this.GroupTitle, this.Font, TextColorBrush, CustomStringWidth, 5);

            // g.DrawString(this.GroupTitle, this.Font, TextColorBrush, CustomStringWidth-this.TitleLeftSpace-2, 5);
            g.DrawString(this.GroupTitle, this.Font, TextColorBrush, CustomStringWidth - this.TitleLeftSpace, 5);
            //-----------------------------------

            //Draw GroupImage if there is one----
            if (this.GroupImage != null)
            {
                //�������˱�����빦��,���޸�X����ֵ By  WZW 2008-12-17
                g.DrawImage(this.GroupImage, ArcX1 + 5, 4, 16, 16);
            }

            //�ͷ���Դ------------
            if (path != null) { path.Dispose(); }
            if (BorderBrush != null) { BorderBrush.Dispose(); }
            if (BorderPen != null) { BorderPen.Dispose(); }
            if (BackgroundGradientBrush != null) { BackgroundGradientBrush.Dispose(); }
            if (BackgroundBrush != null) { BackgroundBrush.Dispose(); }
            if (TextColorBrush != null) { TextColorBrush.Dispose(); }
            if (ShadowBrush != null) { ShadowBrush.Dispose(); }
            if (ShadowPath != null) { ShadowPath.Dispose(); }
        }


        /// <summary>This method will paint the control.</summary>
        /// <param name="g">The paint event graphics object.</param>
        private void PaintBack(System.Drawing.Graphics g)
        {
            //Set Graphics smoothing mode to Anit-Alias-- 
            g.SmoothingMode = SmoothingMode.AntiAlias;
            //-------------------------------------------

            //Declare Variables------------------
            int ArcWidth = this.RoundCorners * 2;
            int ArcHeight = this.RoundCorners * 2;
            int ArcX1 = 0;
            int ArcX2 = (this.ShadowControl) ? (this.Width - (ArcWidth + 1)) - this.ShadowThickness : this.Width - (ArcWidth + 1);
            int ArcY1 = 10;
            int ArcY2 = (this.ShadowControl) ? (this.Height - (ArcHeight + 1)) - this.ShadowThickness : this.Height - (ArcHeight + 1);

            int intXEnd = 20;

            if (GroupBoxAlignMode.Center == this.V_GroupBoxAlignMode)
            {
                intXEnd += V_XTrans;
            }
            else if (GroupBoxAlignMode.Right == this.V_GroupBoxAlignMode)
            {
                intXEnd += V_XTrans * 2;
            }
            int intTextLength = g.MeasureString(this.GroupTitle, this.Font).ToSize().Width + 34 - (ArcWidth + 1) - 20;
            int intXStart = intXEnd + intTextLength + ArcWidth;
            int X1 = intXEnd;
            int X2 = intXStart;

            if (this.GroupImage != null)
            {
                if (this.ShowTileRectangle)
                {
                    X2 += 18;
                }
                else
                {
                    X2 += 10;
                }
            }
            else
            {
                if (!this.ShowTileRectangle)
                {
                    X1 += 8 - this.TextLineSpace;
                    X2 -= 8 - this.TextLineSpace;
                }
            }
            X1 -= this.TitleLeftSpace;
            X2 -= this.TitleLeftSpace;

            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            System.Drawing.Brush BorderBrush = new SolidBrush(this.BorderColor);
            System.Drawing.Pen BorderPen = new Pen(BorderBrush, this.BorderThickness);
            System.Drawing.Drawing2D.LinearGradientBrush BackgroundGradientBrush = null;
            System.Drawing.Brush BackgroundBrush = new SolidBrush(this.BackgroundColor);
            System.Drawing.SolidBrush ShadowBrush = null;
            System.Drawing.Drawing2D.GraphicsPath ShadowPath = null;
            //-----------------------------------

            //Check if shadow is needed----------
            if (this.ShadowControl)
            {
                ShadowBrush = new SolidBrush(this.ShadowColor);
                ShadowPath = new System.Drawing.Drawing2D.GraphicsPath();
                ShadowPath.AddArc(ArcX1 + this.ShadowThickness, ArcY1 + this.ShadowThickness, ArcWidth, ArcHeight, 180, GroupBoxConstants.SweepAngle); // Top Left
                ShadowPath.AddArc(ArcX2 + this.ShadowThickness, ArcY1 + this.ShadowThickness, ArcWidth, ArcHeight, 270, GroupBoxConstants.SweepAngle); //Top Right
                ShadowPath.AddArc(ArcX2 + this.ShadowThickness, ArcY2 + this.ShadowThickness, ArcWidth, ArcHeight, 360, GroupBoxConstants.SweepAngle); //Bottom Right
                ShadowPath.AddArc(ArcX1 + this.ShadowThickness, ArcY2 + this.ShadowThickness, ArcWidth, ArcHeight, 90, GroupBoxConstants.SweepAngle); //Bottom Left
                ShadowPath.CloseAllFigures();

                //Paint Rounded Rectangle------------
                g.FillPath(ShadowBrush, ShadowPath);
                //-----------------------------------
            }
            //-----------------------------------
            //Edit BY WZW 2008-12-16
            //Create Rounded Rectangle Path------
            //path.AddArc(ArcX1, ArcY1, ArcWidth, ArcHeight, 180, GroupBoxConstants.SweepAngle); // Top Left
            //path.AddArc(ArcX2, ArcY1, ArcWidth, ArcHeight, 270, GroupBoxConstants.SweepAngle); //Top Right
            //path.AddArc(ArcX2, ArcY2, ArcWidth, ArcHeight, 360, GroupBoxConstants.SweepAngle); //Bottom Right
            //path.AddArc(ArcX1, ArcY2, ArcWidth, ArcHeight, 90, GroupBoxConstants.SweepAngle); //Bottom Left
            //path.CloseAllFigures(); 
            //g.DrawPath(BorderPen, path);
            //-----------------------------------
            //g.DrawArc(ArcX1, ArcY1, ArcWidth, ArcHeight, 180, GroupBoxConstants.SweepAngle);

            //���Ʊ�����߿�-----------------------


            //���Ͻ�
            g.DrawArc(BorderPen, ArcX1, ArcY1, ArcWidth, ArcHeight, 180, GroupBoxConstants.SweepAngle);
            //���������Ϸ����λ��ߣ�����֮��Ϊ�������

            g.DrawLine(BorderPen, new Point(ArcX1 + this.RoundCorners, ArcY1), new Point(X1, ArcY1));//���һ��ֱ��
            g.DrawLine(BorderPen, new Point(X2, ArcY1), new Point(ArcX2 + this.RoundCorners, ArcY1));//�ұ�һ��ֱ��

            //���Ͻ�
            g.DrawArc(BorderPen, ArcX2, ArcY1, ArcWidth, ArcHeight, 270, GroupBoxConstants.SweepAngle);

            if (!V_BorderTopOnly)
            {
                //����������
                g.DrawLine(BorderPen, new Point(ArcX2 + ArcWidth, ArcY1 + this.RoundCorners), new Point(ArcX2 + ArcHeight, ArcY2 + this.RoundCorners));
                //���½�
                g.DrawArc(BorderPen, ArcX2, ArcY2, ArcWidth, ArcHeight, 360, GroupBoxConstants.SweepAngle);
                //����������
                g.DrawLine(BorderPen, new Point(ArcX2 + this.RoundCorners, ArcY2 + ArcWidth), new Point(ArcX1 + this.RoundCorners, ArcY2 + ArcWidth));
                //���½�
                g.DrawArc(BorderPen, ArcX1, ArcY2, ArcWidth, ArcHeight, 90, GroupBoxConstants.SweepAngle);
                //����ֻ����
                g.DrawLine(BorderPen, new Point(ArcX1, ArcY2 + this.RoundCorners), new Point(ArcX1, ArcY1 + this.RoundCorners));
                //g.DrawPath(BorderPen, path);
            }

            //Check if Gradient Mode is enabled--
            if (this.BackgroundGradientMode == GroupBoxGradientMode.None)
            {
                //Paint Rounded Rectangle------------
                g.FillPath(BackgroundBrush, path);
                //-----------------------------------
            }
            else
            {
                BackgroundGradientBrush = new LinearGradientBrush(new Rectangle(0, 0, this.Width, this.Height), this.BackgroundColor, this.BackgroundGradientColor, (LinearGradientMode)this.BackgroundGradientMode);

                //Paint Rounded Rectangle------------
                g.FillPath(BackgroundGradientBrush, path);
                //-----------------------------------
            }
            //-----------------------------------

            //Delete BY WZW 
            //Paint Borded-----------------------
            //g.DrawPath(BorderPen, path);
            //g.DrawLine(SystemPens.Control, 10, 10, 100, 10);
            //-----------------------------------

            //Destroy Graphic Objects------------
            if (path != null) { path.Dispose(); }
            if (BorderBrush != null) { BorderBrush.Dispose(); }
            if (BorderPen != null) { BorderPen.Dispose(); }
            if (BackgroundGradientBrush != null) { BackgroundGradientBrush.Dispose(); }
            if (BackgroundBrush != null) { BackgroundBrush.Dispose(); }
            if (ShadowBrush != null) { ShadowBrush.Dispose(); }
            if (ShadowPath != null) { ShadowPath.Dispose(); }
            //-----------------------------------
        }


        /// <summary>This method fires when the GroupBox resize event occurs.</summary>
        /// <param name="sender">The object the sent the event.</param>
        /// <param name="e">The event arguments.</param>
        private void GroupBox_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }


        #endregion
    }
}
