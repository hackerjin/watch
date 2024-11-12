using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Skyray.Controls
{

    #region E N U M S
    public enum LineStyle
    {
        None = 0,
        Vertical = 1,
        Horizontal = 2,
        Box = 3
    }
    public enum GradientDirection
    {
        Horizontal = 1,
        Vertical = 2
    }

    #endregion

    /// <summary>
    /// Summary description for UserControl1.
    /// </summary>
    public class SimpleLine : System.Windows.Forms.UserControl
    {

        #region D E C L A R A T I O N S

        private System.Drawing.Color _color = System.Drawing.Color.Black;
        private System.Drawing.Color _fillColor = System.Drawing.Color.Transparent;
        private int _lineWidth = 1;
        private LineStyle _lineStyle;
        private bool _fitToParent = false;
        private System.Drawing.Color _Gradient = System.Drawing.Color.Transparent;
        private bool _useGradient = false;
        private GradientDirection _gradientAngle = GradientDirection.Horizontal;

        // Added AfterPaint event, which will be raised after the control paints itself
        public event PaintEventHandler AfterPaint;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        #endregion

        #region C O N S T R U C T O R
        public SimpleLine()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            DrawLine();
            // TODO: Add any initialization after the InitComponent call
        }
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region P R O P E R T I E S

        /// <summary>
        /// Enum indicating horizontal or vertical gradient draw
        /// </summary>
        [Category("Custom")]
        public GradientDirection GradientAngle
        {
            get { return _gradientAngle; }
            set
            {
                _gradientAngle = value;
                DrawLine();
            }
        }

        /// <summary>
        /// For lines, this will scale the line to fit right-to-left, or top-to-bottom
        /// </summary>
        [Category("Custom")]
        public bool FitToParent
        {
            get { return _fitToParent; }
            set
            {
                _fitToParent = value;
                DrawLine();
            }
        }

        /// <summary>
        /// If set to true, gradient will draw with FillColor and Gradient color
        /// </summary>
        [Category("Custom")]
        public bool UseGradient
        {
            get { return _useGradient; }
            set
            {
                _useGradient = value;
                DrawLine();
            }
        }

        [Category("Custom")]
        public System.Drawing.Color FillColor
        {
            get { return _fillColor; }
            set
            {
                _fillColor = value;
                DrawLine();            
            }
        }

        [Category("Custom")]
        public System.Drawing.Color Gradient
        {
            get { return _Gradient; }
            set
            {
                _Gradient = value;
                DrawLine();
            }
        }

        [Category("Custom")]
        public System.Drawing.Color LineColor
        {
            get
            {
                if (_color == Color.Transparent) { _color = Parent.BackColor; }
                return _color;

            }
            set
            {
                _color = value;
                DrawLine();
            }
        }

        [Category("Custom")]
        public int LineWidth
        {
            get { return _lineWidth; }
            set
            {
                _lineWidth = value;
                DrawLine();
            }
        }

        /// <summary>
        /// Enum indicating horizontal line, vertical line, or Box
        /// </summary>
        [Category("Custom")]
        public LineStyle Style
        {
            get { return _lineStyle; }
            set
            {
                _lineStyle = value;
                if (value == LineStyle.Vertical && Height <= LineWidth) { Height = 50; Width = LineWidth; }
                if (value == LineStyle.Horizontal && Width <= LineWidth) { Width = 50; Height = LineWidth; }
                if ((value == LineStyle.Box) && ((Width <= LineWidth) || (Height <= LineWidth)))
                {
                    Height = 50;
                    Width = 50;
                }
                DrawLine();
            }
        }
        #endregion

        #region E V E N T S

        protected override void InitLayout()
        {
            DrawLine();
        }


        #endregion

        #region M E T H O D S

        public void DrawLine()
        {
            if (this.Parent == null) { return; } //we don't want the control to draw on itself at design time

            if (this.Style == LineStyle.None) //default to Horizontal line, when placed on a parent
            {
                _lineStyle = LineStyle.Horizontal;
                _lineWidth = 1;
                this.Left = (Parent.Width / 2) - this.Width / 2;
                this.Top = Parent.Height / 2;
            }

            Graphics g = this.CreateGraphics(); //create the graphics object
            g.Clear(Parent.BackColor);
            Pen pn;
            if (this.Style == LineStyle.Vertical || this.Style == LineStyle.Horizontal)
                pn = new Pen(LineColor, LineWidth * 2);
            else
                pn = new Pen(LineColor, LineWidth);

            Point pt1 = new Point(0, 0);
            Point pt2;
            if (this.Style == LineStyle.Horizontal)
            {
                if (FitToParent == true)
                {
                    this.Left = 0;
                    this.Width = Parent.ClientRectangle.Width;
                }

                this.Height = LineWidth;
                if (this.Height < 1) { this.Height = 1; }

                pt2 = new Point(Width, 0);
                if (UseGradient == false)
                {
                    g.DrawLine(pn, pt1, pt2);

                }
                else
                {
                    Rectangle rect = new Rectangle(new Point(0, 0), new Size(this.ClientRectangle.Width, LineWidth));
                    if (FillColor == Color.Transparent) { FillColor = Parent.BackColor; }
                    {
                        LinearGradientBrush lgb = new LinearGradientBrush(rect, FillColor, Gradient, 0, false);
                        g.FillRectangle(lgb, 0, 0, this.Width, LineWidth);
                    }
                }
            }
            else if (this.Style == LineStyle.Vertical)
            {
                if (FitToParent == true)
                {
                    this.Top = 0;
                    this.Height = Parent.Height;
                }

                this.Width = LineWidth;
                if (this.Width < 1) { this.Width = 1; }

                pt2 = new Point(0, Height);
                if (UseGradient == false)
                {
                    g.DrawLine(pn, pt1, pt2);
                }
                else
                {
                    Rectangle rect = new Rectangle(new Point(0, 0), new Size(LineWidth, this.Height));
                    if (FillColor == Color.Transparent) { FillColor = Parent.BackColor; }
                    {
                        LinearGradientBrush lgb = new LinearGradientBrush(rect, FillColor, Gradient, 90, false);
                        g.FillRectangle(lgb, 0, 0, LineWidth, this.Height);

                    }
                }
            }
            else if (this.Style == LineStyle.Box)
            {
                if (FitToParent == true)
                {
                    this.Top = 0;
                    this.Left = 0;
                    this.Width = Parent.Width;
                    this.Height = Parent.Height;
                }

                Rectangle rect = new Rectangle(new Point(0, 0), new Size(this.Width, this.Height));
                if (FillColor == Color.Transparent) { FillColor = Parent.BackColor; }
                if (UseGradient)
                {
                    LinearGradientBrush lgb = new LinearGradientBrush(rect, FillColor, Gradient, GradientAngle == GradientDirection.Horizontal ? 0 : 90, false);
                    g.FillRectangle(lgb, 0, 0, this.Width - LineWidth, this.Height - LineWidth);

                }
                else
                {
                    SolidBrush sb = new SolidBrush(FillColor);
                    g.FillRectangle(sb, 0, 0, this.Width - LineWidth, this.Height - LineWidth);

                }

                decimal mod = Decimal.Remainder((decimal)LineWidth, (decimal)2);
                int offset = 0;
                if (mod != 0 && LineWidth != 1) { offset = 1; }

                rect.Offset(LineWidth / 2, LineWidth / 2);
                rect.Height = rect.Height - LineWidth + offset - 1;
                rect.Width = rect.Width - LineWidth + offset - 1;
                if (LineWidth > 0) { g.DrawRectangle(pn, rect); }
            }
            g.Dispose();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            OnResize(EventArgs.Empty);

            // Added AfterPaint event, which will be raised after the control paints itself
            if (AfterPaint != null)
                AfterPaint(this, e);
        }

        protected override void OnResize(EventArgs e)
        {

            DrawLine();
            base.OnResize(e);
        }



        #endregion

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SimpleLine
            // 
            this.Name = "SimpleLine";
            this.Size = new System.Drawing.Size(355, 10);
            this.ResumeLayout(false);

        }
        #endregion
    }
}
