namespace Skyray.EDX.Common
{
    #region Author/About
    /************************************************************************************
    *  vtExtender   ToolStrip Extension Class                                           *
    *                                                                                   *
    *  Created:     Febuary 06, 2009                                                    *
    *  Purpose:     Extends style capabilities of a ToolStrip control                   *
    *  Revision:    1.1                                                                 *
    *  IDE:         C# 2005 SP1                                                         *
    *  Referenced:  Control Class NSP                                                   *
    *  Author:      John Underhill (Steppenwolfe)                                       *
    *                                                                                   *
    *************************************************************************************

    You can not:
    Sell or redistribute this code or the binary for profit.
    Use this in spyware, malware, or any generally acknowledged form of malicious software.
    Remove or alter the above author accreditation, or this disclaimer.

    You can:
    Use this code in your applications in any way you like.
    Use this in a published program, (a credit to vtdev.com would be nice)

    I will not:
    Except any responsibility for this code whatsoever. 
    There is no guarantee of fitness, nor should you have any expectation of support. 
    I further renounce any and all responsibilities for this code, in every way conceivable, 
    now, and for the rest of time. (clear enough?)

    Cheers,
    John
    steppenwolfe_2000@yahoo.com

    ***Revision History***
    -> 1.1
    First revision released Febuary 15

 
    */
    #endregion

    #region Directives
    using System;
    using System.Windows.Forms;
    using System.Reflection;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Diagnostics;
    using System.Timers;
    #endregion

    #region Enums
    public enum ToolStripType
    {
        VerticalGradient = 0,
        HorizontalGradient,
        FlatColor,
        Image,
        System
    }

    public enum MenuType
    {
        Vista = 0,
        Office,
        Custom,
        Flat
    }

    public enum ButtonHoverType
    {
        Flat = 0,
        Glow,
        Raised,
        Glass,
        Bevelled
    }

    public enum GripType
    {
        Dotted = 0,
        LargeDot,
        Solid
    }

    internal enum FadeType
    {
        None = 0,
        FadeIn,
        FadeOut,
        FadeFast
    }
    #endregion

    #region vtExtender
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class vtExtender : ToolStripRenderer, IDisposable
    {
        #region Base Class
        #region Enums
        internal enum ItemState
        {
            ItemHover = 0,
            OffControl,
            OffContainer
        }

        internal enum ButtonSelectedState
        {
            None = 0,
            Disabled,
            Checked,
            Focused,
            Pressed
        }
        #endregion

        #region Constants
        private const int WM_MOUSEMOVE = 0x200;
        private const int WM_MOUSELEAVE = 0x2A3;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_LBUTTONUP = 0x202;
        private const int WM_MOUSEHOVER = 0x2A1;
        private const int WM_PAINT = 0xF;
        #endregion

        #region API
        [DllImport("gdi32.dll")]
        private static extern uint SetPixel(IntPtr hdc, int X, int Y, int crColor);
        #endregion

        #region Fields
        private bool _toolTipEnable = false;
        private bool _toolTipUseIcon = false;
        private bool _toolTipRightToLeft = false;
        private int _toolTipMaximumLength = 200;
        private int _toolTipDelayTime = 1000;
        private int _toolTipVisibleTime = 2000;
        private Color _toolTipGradientBegin;
        private Color _toolTipGradientEnd;
        private Color _toolTipForeColor;

        private Color _buttonBorderColor = Color.DarkGray;
        private Color _buttonGradientBegin = Color.Silver;
        private Color _buttonGradientEnd = Color.Black;
        private Color _buttonForeColor = Color.Black;
        private Color _buttonFocusedForeColor = Color.CornflowerBlue;

        private Color _menuSelectorBarGradientBegin = Color.FromArgb(140, Color.White);
        private Color _menuSelectorBarGradientEnd = Color.FromArgb(120, 0xb3, 0xb3, 0xb3);// magenta is skip
        private Color _menuSelectorBarEdge = Color.FromArgb(250, Color.DarkGray);
        private Color _menuStripBorderColor = Color.DarkGray;
        private Color _menuStripGradientBegin = Color.Silver;
        private Color _menuStripGradientEnd = Color.Black;
        private Color _menuDropDownBackground = Color.FromArgb(255, 0xf5, 0xf5, 0xf5);
        private Color _menuForeColor = Color.White;
        private Color _menuFocusedForeColor = Color.LightSkyBlue;
        private Color _menuItemForeColor = Color.FromArgb(255, 0x77, 0x77, 0x77);
        private Color _menuItemFocusedForeColor = Color.Black;
        private Color _menuImageMarginGradientBegin = Color.Silver;
        private Color _menuImageMarginGradientEnd = Color.Magenta;

        private Color _statusStripBorderColor = Color.DarkGray;
        private Color _statusStripGradientBegin = Color.Silver;
        private Color _statusStripGradientEnd = Color.Black;

        private Color _toolStripBorderColor = Color.DarkGray;
        private Color _toolStripGradientBegin = Color.Silver;
        private Color _toolStripGradientEnd = Color.Black;

        private Color _dropArrowColor = Color.Silver;
        private Color _seperatorInnerColor = Color.DarkGray;
        private Color _seperatorOuterColor = Color.Black;

        private ButtonHoverType _buttonHoverEffect = ButtonHoverType.Raised;
        private GripType _gripStyle = GripType.Dotted;

        private ToolStripType _menuStripStyle = ToolStripType.HorizontalGradient;
        private Blend _menuStripGradientBlend = new Blend();
        private LinearGradientMode _menuStripGradientDirection = LinearGradientMode.Vertical;
        private MenuType _menuStyle = MenuType.Custom;

        private ToolStripType _statusStripStyle = ToolStripType.HorizontalGradient;
        private Blend _statusStripGradientBlend = new Blend();
        private LinearGradientMode _statusStripGradientDirection = LinearGradientMode.Vertical;

        private ToolStripType _toolStripStyle = ToolStripType.HorizontalGradient;
        private Blend _toolStripGradientBlend = new Blend();
        private LinearGradientMode _toolStripGradientDirection = LinearGradientMode.Vertical;

        private ComboBoxExtender _comboBoxExtender;
        private ToolStripExtender _toolStripExtender;
        private ToolTip _toolTip;
        private Dictionary<ToolStripItem, string> _toolTipText = new Dictionary<ToolStripItem, string>();
        private Dictionary<ToolStripItem, string> _toolTipTitle = new Dictionary<ToolStripItem, string>();
        #endregion

        #region Constructor
        public vtExtender()
        {
            Init();
        }

        private void Init()
        {
            _menuStripGradientBlend.Positions = new float[] { 0f, .4f, .5f, .8f, 1f };
            _menuStripGradientBlend.Factors = new float[] { 0f, .2f, .5f, 1f, .6f };
            _statusStripGradientBlend.Positions = new float[] { 0f, .4f, .5f, .8f, 1f };
            _statusStripGradientBlend.Factors = new float[] { 0f, .2f, .5f, 1f, .6f };
            _toolStripGradientBlend.Positions = new float[] { 0f, .4f, .5f, .8f, 1f };
            _toolStripGradientBlend.Factors = new float[] { 0f, .2f, .5f, 1f, .6f };
            SetGlobalStyles(ToolStripType.VerticalGradient, MenuType.Custom, ButtonHoverType.Raised, Color.DarkGray, Color.Black, null);
        }
        #endregion

        #region Event Handlers
        private void toolStrip_BackgroundImageChanged(object sender, EventArgs e)
        {
            ToolStrip toolStrip = (ToolStrip)sender;
            try
            {
                if (toolStrip.BackgroundImage != null)
                {
                    if (toolStrip is MenuStrip)
                        MenuStripStyle = ToolStripType.Image;
                    else if (toolStrip is StatusStrip)
                        StatusStripStyle = ToolStripType.Image;
                    else
                        ToolStripStyle = ToolStripType.Image;
                }
            }
            finally { }
        }

        private void item_MouseDown(object sender, MouseEventArgs e)
        {
            if (_toolTip != null)
                _toolTip.Stop();
        }

        private void Item_MouseEnter(object sender, EventArgs e)
        {
            if ((_toolTip != null) && (ToolTipEnable))
            {
                ToolStripItem item = (ToolStripItem)sender;
                Rectangle bounds = new Rectangle();
                if (_toolTipText.ContainsKey(item))
                {
                    string caption = _toolTipText[item];
                    string title = String.Empty;
                    if (_toolTipTitle.ContainsKey(item))
                        title = _toolTipTitle[item];
                    if (item.Owner.Orientation == Orientation.Horizontal)
                    {
                        bounds.Y = item.Bounds.Height + 10;
                        bounds.X = item.Bounds.X + 10;
                        bounds.Width = ToolTipMaximumLength;
                        bounds.Height = 20;
                    }
                    else
                    {
                        bounds.Y = item.Bounds.Y + 10;
                        bounds.X = item.Bounds.Width + 10;
                        bounds.Width = ToolTipMaximumLength;
                        bounds.Height = 20;
                    }
                    Size imageSize = item.Owner.ImageScalingSize;
                    Bitmap bmp = new Bitmap(item.Image, imageSize);
                    _toolTip.Start(title, caption, bmp, bounds);
                }
            }
        }

        private void Item_MouseLeave(object sender, EventArgs e)
        {
            if (_toolTip != null)
                _toolTip.Stop();
        }
        #endregion

        #region Properties
        #region Buttons
        /// <summary>
        /// Get/Set the button border color.
        /// </summary>
        public Color ButtonBorderColor
        {
            get { return _buttonBorderColor; }
            set { _buttonBorderColor = value; }
        }

        /// <summary>
        /// Get/Set the ToolStrip button border style.
        /// </summary>

        /// <summary>
        /// Get/Set the fade over effect style.
        /// </summary>
        public ButtonHoverType ButtonHoverEffect
        {
            get { return _buttonHoverEffect; }
            set { _buttonHoverEffect = value; }
        }

        /// <summary>
        /// Get/Set the ForeColor.
        /// </summary>
        public Color ButtonForeColor
        {
            get { return _buttonForeColor; }
            set
            {
                _buttonForeColor = value;
                _buttonForeColor = value;
            }
        }

        /// <summary>
        /// Get/Set the focused ForeColor.
        /// </summary>
        public Color ButtonFocusedForeColor
        {
            get { return _buttonFocusedForeColor; }
            set
            {
                _buttonFocusedForeColor = value;
                _buttonFocusedForeColor = value;
            }
        }

        /// <summary>
        /// Get/Set the starting color of the button fade gradient.
        /// </summary>
        public Color ButtonGradientBegin
        {
            get { return _buttonGradientBegin; }
            set { _buttonGradientBegin = value; }
        }

        /// <summary>
        /// Get/Set the ending color of the button fade gradient.
        /// </summary>
        public Color ButtonGradientEnd
        {
            get { return _buttonGradientEnd; }
            set { _buttonGradientEnd = value; }
        }
        #endregion

        #region Drop Arrow
        /// <summary>
        /// Get/Set the drop down arrow color.
        /// </summary>
        public Color DropArrowColor
        {
            get { return _dropArrowColor; }
            set { _dropArrowColor = value; }
        }
        #endregion

        #region Grip and Seperator
        /// <summary>
        /// Get/Set The Grip display style.
        /// </summary>
        public GripType GripStyle
        {
            get { return _gripStyle; }
            set { _gripStyle = value; }
        }

        /// <summary>
        /// Get/Set the seperator inner color.
        /// </summary>
        public Color SeperatorInnerColor
        {
            get { return _seperatorInnerColor; }
            set { _seperatorInnerColor = value; }
        }

        /// <summary>
        /// Get/Set the seperator outer color.
        /// </summary>
        public Color SeperatorOuterColor
        {
            get { return _seperatorOuterColor; }
            set { _seperatorOuterColor = value; }
        }
        #endregion

        #region Menu
        /// <summary>
        /// Get/Set the backcolor of drop down menus.
        /// </summary>
        public Color MenuBackGroundColor
        {
            get { return _menuDropDownBackground; }
            set { _menuDropDownBackground = value; }
        }

        /// <summary>
        /// Get/Set the drawing style for drop down menus.
        /// </summary>
        public MenuType MenuStyle
        {
            get { return _menuStyle; }
            set { _menuStyle = value; }
        }

        /// <summary>
        /// Get/Set the forecolor of drop down menus.
        /// </summary>
        public Color MenuForeColor
        {
            get { return _menuForeColor; }
            set { _menuForeColor = value; }
        }

        /// <summary>
        /// Get/Set the focused forecolor of drop down menus.
        /// </summary>
        public Color MenuFocusedForeColor
        {
            get { return _menuFocusedForeColor; }
            set { _menuFocusedForeColor = value; }
        }

        /// <summary>
        /// Get/Set the forecolor of drop down menu items.
        /// </summary>
        public Color MenuItemForeColor
        {
            get { return _menuItemForeColor; }
            set { _menuItemForeColor = value; }
        }

        /// <summary>
        /// Get/Set the focused forecolor of drop down menu items.
        /// </summary>
        public Color MenuItemFocusedForeColor
        {
            get { return _menuItemFocusedForeColor; }
            set { _menuItemFocusedForeColor = value; }
        }

        /// <summary>
        /// Get/Set the gradient start color of the drop down menu side bar.
        /// </summary>
        public Color MenuImageMarginGradientBegin
        {
            get { return _menuImageMarginGradientBegin; }
            set { _menuImageMarginGradientBegin = value; }
        }

        /// <summary>
        /// Get/Set the gradient end color of the drop down menu side bar.
        /// </summary>
        public Color MenuImageMarginGradientEnd
        {
            get { return _menuImageMarginGradientEnd; }
            set { _menuImageMarginGradientEnd = value; }
        }

        /// <summary>
        /// Get/Set the gradient start color of the menu selector bar.
        /// </summary>
        public Color MenuSelectorBarGradientBegin
        {
            get { return _menuSelectorBarGradientBegin; }
            set { _menuSelectorBarGradientBegin = value; }
        }

        /// <summary>
        /// Get/Set the gradient end color of the menu selector bar.
        /// </summary>
        public Color MenuSelectorBarGradientEnd
        {
            get { return _menuSelectorBarGradientEnd; }
            set { _menuSelectorBarGradientEnd = value; }
        }

        /// <summary>
        /// Get/Set the gradient edge color of the menu selector bar.
        /// </summary>
        public Color MenuSelectorBarEdge
        {
            get { return _menuSelectorBarEdge; }
            set { _menuSelectorBarEdge = value; }
        }

        /// <summary>
        /// Get/Set the MenuStrip border color.
        /// </summary>
        public Color MenuStripBorderColor
        {
            get { return _menuStripBorderColor; }
            set { _menuStripBorderColor = value; }
        }

        /// <summary>
        /// Get/Set the direction of the gradient.
        /// </summary>
        public LinearGradientMode MenuStripGradientDirection
        {
            get { return _menuStripGradientDirection; }
            set { _menuStripGradientDirection = value; }
        }

        /// <summary>
        /// Get/Set the blend factor of the gradient.
        /// </summary>
        public Blend MenuStripGradientBlend
        {
            get { return _menuStripGradientBlend; }
            set { _menuStripGradientBlend = value; }
        }

        /// <summary>
        /// Get/Set the starting color of the gradient.
        /// </summary>
        public Color MenuStripGradientBegin
        {
            get { return _menuStripGradientBegin; }
            set { _menuStripGradientBegin = value; }
        }

        /// <summary>
        /// Get/Set the ending color of the gradient.
        /// </summary>
        public Color MenuStripGradientEnd
        {
            get { return _menuStripGradientEnd; }
            set { _menuStripGradientEnd = value; }
        }

        /// <summary>
        /// Get/Set the drawing style for the menu strip.
        /// </summary>
        private ToolStripType MenuStripStyle
        {
            get { return _menuStripStyle; }
            set { _menuStripStyle = value; }

        }
        #endregion

        #region StatusStrip
        /// <summary>
        /// Get/Set the StatusStrip border color.
        /// </summary>
        public Color StatusStripBorderColor
        {
            get { return _statusStripBorderColor; }
            set { _statusStripBorderColor = value; }
        }

        /// <summary>
        /// Get/Set the blend factor of the gradient.
        /// </summary>
        public Blend StatusStripGradientBlend
        {
            get { return _statusStripGradientBlend; }
            set { _statusStripGradientBlend = value; }
        }

        /// <summary>
        /// Get/Set the direction of the gradient.
        /// </summary>
        public LinearGradientMode StatusStripGradientDirection
        {
            get { return _statusStripGradientDirection; }
            set { _statusStripGradientDirection = value; }
        }

        /// <summary>
        /// Get/Set the starting color of the gradient.
        /// </summary>
        public Color StatusStripGradientBegin
        {
            get { return _statusStripGradientBegin; }
            set { _statusStripGradientBegin = value; }
        }

        /// <summary>
        /// Get/Set the ending color of the gradient.
        /// </summary>
        public Color StatusStripGradientEnd
        {
            get { return _statusStripGradientEnd; }
            set { _statusStripGradientEnd = value; }
        }

        /// <summary>
        /// Get/Set the drawing style for the status strip.
        /// </summary>
        private ToolStripType StatusStripStyle
        {
            get { return _statusStripStyle; }
            set { _statusStripStyle = value; }
        }
        #endregion

        #region ToolStrip
        /// <summary>
        /// Get/Set the ToolStrip border color.
        /// </summary>
        public Color ToolStripBorderColor
        {
            get { return _toolStripBorderColor; }
            set { _toolStripBorderColor = value; }
        }

        /// <summary>
        /// Get/Set the blend factor of the gradient.
        /// </summary>
        public Blend ToolStripGradientBlend
        {
            get { return _toolStripGradientBlend; }
            set { _toolStripGradientBlend = value; }
        }

        /// <summary>
        /// Get/Set the direction of the gradient.
        /// </summary>
        public LinearGradientMode ToolStripGradientDirection
        {
            get { return _toolStripGradientDirection; }
            set { _toolStripGradientDirection = value; }
        }

        /// <summary>
        /// Get/Set the starting color of the gradient.
        /// </summary>
        public Color ToolStripGradientBegin
        {
            get { return _toolStripGradientBegin; }
            set { _toolStripGradientBegin = value; }
        }

        /// <summary>
        /// Get/Set the ending color of the gradient.
        /// </summary>
        public Color ToolStripGradientEnd
        {
            get { return _toolStripGradientEnd; }
            set { _toolStripGradientEnd = value; }
        }

        /// <summary>
        /// Get/Set the drawing style for the tool strip.
        /// </summary>
        private ToolStripType ToolStripStyle
        {
            get { return _toolStripStyle; }
            set { _toolStripStyle = value; }
        }
        #endregion

        #region ToolTip
        /// <summary>
        /// The amount of time in milliseconds before the ToolTip appears.
        /// </summary>
        public int ToolTipDelayTime
        {
            get { return _toolTipDelayTime; }
            set {
                _toolTipDelayTime = value;
                if (_toolTip != null)
                    _toolTip.DelayTime = value;
            }
        }

        /// <summary>
        /// Get/Set the ToolStrip enabled property.
        /// </summary>
        public bool ToolTipEnable
        {
            get { return _toolTipEnable; }
            set { _toolTipEnable = value; }
        }

        /// <summary>
        /// Get/Set the forecolor of drop down menu items.
        /// </summary>
        public Color ToolTipForeColor
        {
            get { return _toolTipForeColor; }
            set {
                _toolTipForeColor = value;
                if (_toolTip != null)
                    _toolTip.ForeColor = value;
            }
        }

        /// <summary>
        /// Get/Set the starting color of the gradient.
        /// </summary>
        public Color ToolTipGradientBegin
        {
            get { return _toolTipGradientBegin; }
            set {
                _toolTipGradientBegin = value;
                if (_toolTip != null)
                    _toolTip.GradientBegin = value;
            }
        }

        /// <summary>
        /// Get/Set the ending color of the gradient.
        /// </summary>
        public Color ToolTipGradientEnd
        {
            get { return _toolTipGradientEnd; }
            set {
                _toolTipGradientEnd = value;
                if (_toolTip != null)
                    _toolTip.GradientEnd = value;
            }
        }

        /// <summary>
        /// The maximum length of the ToolTip in pixels.
        /// </summary>
        public int ToolTipMaximumLength
        {
            get { return _toolTipMaximumLength; }
            set {
                _toolTipMaximumLength = value;
                if (_toolTip != null)
                    _toolTip.MaximumLength = value;
            }
        }

        /// <summary>
        /// Position the ToolTip text right to left.
        /// </summary>
        public bool ToolTipRightToLeft
        {
            get { return _toolTipRightToLeft; }
            set {
                _toolTipRightToLeft = value;
                if (_toolTip != null)
                    _toolTip.TextRightToLeft = value;
            }
        }

        /// <summary>
        /// Display the buttons icon in the ToolTip.
        /// </summary>
        public bool ToolTipUseIcon
        {
            get { return _toolTipUseIcon; }
            set {
                _toolTipUseIcon = value;
                if (_toolTip != null)
                    _toolTip.UseIcon = value;
            }
        }

        /// <summary>
        /// The length of time in milliseconds that the ToolTip remains visible.
        /// </summary>
        public int ToolTipVisibleTime
        {
            get { return _toolTipVisibleTime; }
            set {
                _toolTipVisibleTime = value;
                if (_toolTip != null)
                    _toolTip.VisibleTime = value;
            }
        }


        public void ToolTipTitle(ToolStripItem item, string title)
        {
            if (!String.IsNullOrEmpty(title))
            {
                _toolTipTitle.Add(item, title);
            }
        }

        public void UseCustomToolTips(ToolStrip toolStrip)
        {
            foreach (ToolStripItem item in toolStrip.Items)
            {
                if (!String.IsNullOrEmpty(item.ToolTipText))
                {
                    _toolTipText.Add(item, item.ToolTipText);
                    item.ToolTipText = "";
                    item.AutoToolTip = false;
                    item.MouseEnter += new EventHandler(Item_MouseEnter);
                    item.MouseLeave += new EventHandler(Item_MouseLeave);
                    item.MouseDown += new MouseEventHandler(item_MouseDown);
                }
            }
            if (_toolTipText.Count > 0)
            {
                _toolTip = new ToolTip(toolStrip.Handle);
                _toolTip.TextRightToLeft = ToolTipRightToLeft;
                ToolTipEnable = true;
            }
        }
        #endregion
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// Adds a ToolStrip control to the class. 
        /// </summary>
        /// <param name="control">The ToolStrip control to be stylized.</param>
        public void Add(Control control)
        {
            if (control is ToolStrip)
                _toolStripExtender = new ToolStripExtender(this, (ToolStrip)control);
            else
                throw new Exception("The control type is not supported.");
        }

        /// <summary>
        /// Sets styles for all added ToolStrip controls. 
        /// </summary>
        /// <param name="toolstyle">The ToolStrip control to be stylized.</param>
        /// <param name="menustyle">The Menu display style.</param>
        /// <param name="buttonstyle">The button hover effect style.</param>
        /// <param name="begin">The gradient start color, or flat color used to paint the control.</param>
        /// <param name="end">The gradient end color.</param>
        /// <param name="blend">The gradient blend used to control the gradient aspect. Pass null for default blend.</param>
        public void SetGlobalStyles(ToolStripType toolstyle, MenuType menuStyle, ButtonHoverType buttonStyle, Color begin, Color end, Blend blend)
        {
            SetToolStripStyle(toolstyle, begin, end, blend);
            SetMenuStripStyle(toolstyle, menuStyle, begin, end, blend);
            SetButtonHoverStyle(buttonStyle);
            SetStatusStripStyle(toolstyle, begin, end, blend);
        }

        /// <summary>
        /// Sets ToolStrip style elements. 
        /// </summary>
        /// <param name="style">The ToolStrip control to be stylized.</param>
        /// <param name="begin">The gradient start color, or flat color used to paint the control.</param>
        /// <param name="end">The gradient end color.</param>
        /// <param name="blend">The gradient blend used to control the gradient aspect.</param>
        public void SetToolStripStyle(ToolStripType style, Color begin, Color end, Blend blend)
        {
            ToolStripStyle = style;
            if (ToolStripStyle == ToolStripType.HorizontalGradient)
            {
                ToolStripGradientDirection = LinearGradientMode.Horizontal;
                ToolStripGradientBegin = begin;
                ToolStripGradientEnd = end;
                if (blend != null)
                    ToolStripGradientBlend = blend;
            }
            else if (ToolStripStyle == ToolStripType.VerticalGradient)
            {
                ToolStripGradientDirection = LinearGradientMode.Vertical;
                ToolStripGradientBegin = begin;
                ToolStripGradientEnd = end;
                if (blend != null)
                    ToolStripGradientBlend = blend;
            }
            else
            {
                ToolStripGradientBegin = begin;
                ToolStripGradientEnd = end;
            }
        }

        /// <summary>
        /// Sets MenuStrip style elements. 
        /// </summary>
        /// <param name="stripStyle">The MenuStrip control to be stylized.</param>
        /// <param name="menuStyle">The Menu display style.</param>
        /// <param name="begin">The gradient start color, or flat color used to paint the control.</param>
        /// <param name="end">The gradient end color.</param>
        /// <param name="blend">The gradient blend used to control the gradient aspect. Pass null for default blend.</param>
        public void SetMenuStripStyle(ToolStripType stripStyle, MenuType menuStyle, Color begin, Color end, Blend blend)
        {
            MenuStripStyle = stripStyle;
            if (MenuStripStyle == ToolStripType.HorizontalGradient)
            {
                MenuStripGradientDirection = LinearGradientMode.Horizontal;
                MenuStripGradientBegin = begin;
                MenuStripGradientEnd = end;
                if (blend != null)
                    MenuStripGradientBlend = blend;
            }
            else if (MenuStripStyle == ToolStripType.VerticalGradient)
            {
                MenuStripGradientDirection = LinearGradientMode.Vertical;
                MenuStripGradientBegin = begin;
                MenuStripGradientEnd = end;
                if (blend != null)
                    MenuStripGradientBlend = blend;
            }
            else
            {
                MenuStripGradientBegin = begin;
                MenuStripGradientEnd = end;
            }

            MenuStyle = menuStyle;
            if ((MenuStyle == MenuType.Custom) || (MenuStyle == MenuType.Flat))
            {
                MenuSelectorBarEdge = Color.FromArgb(250, Color.DarkGray);
                MenuSelectorBarGradientBegin = Color.FromArgb(140, Color.White);
                MenuSelectorBarGradientEnd = Color.FromArgb(120, 0xb3, 0xb3, 0xb3);
                ButtonGradientBegin = Color.White;
            }
            else if (MenuStyle == MenuType.Office)
            {
                MenuSelectorBarEdge = Color.FromArgb(250, 0xa9, 0xc8, 0xf5);
                MenuSelectorBarGradientBegin = Color.FromArgb(140, 0xe1, 0xf5, 0xfb);
                MenuSelectorBarGradientEnd = Color.FromArgb(120, 0xa5, 0xc4, 0xf1);
                ButtonBorderColor = Color.LightGray;
                ButtonGradientBegin = Color.FromArgb(200, 0x33, 0x99, 0xff);
                ButtonGradientEnd = Color.FromArgb(150, 0xc0, 0xdd, 0xfc);

            }
            else if (MenuStyle == MenuType.Vista)
            {
                MenuSelectorBarEdge = Color.FromArgb(150, 0xA9, 0xC8, 0xF5);
                MenuSelectorBarGradientBegin = Color.FromArgb(140, Color.White);
                MenuSelectorBarGradientEnd = Color.FromArgb(200, Color.PowderBlue);
                MenuBackGroundColor = Color.FromArgb(255, 0xf5, 0xf5, 0xf5);
                MenuImageMarginGradientBegin = Color.FromArgb(255, 0xec, 0xec, 0xec);
                ButtonGradientBegin = Color.DodgerBlue;
            }
        }

        /// <summary>
        /// Sets StatusStrip style elements. 
        /// </summary>
        /// <param name="style">The StatusStrip control to be stylized.</param>
        /// <param name="begin">The gradient start color, or flat color used to paint the control.</param>
        /// <param name="end">The gradient end color.</param>
        /// <param name="blend">The gradient blend used to control the gradient aspect. Pass null for default blend.</param>
        public void SetStatusStripStyle(ToolStripType style, Color begin, Color end, Blend blend)
        {
            StatusStripStyle = style;
            if (StatusStripStyle == ToolStripType.HorizontalGradient)
            {
                StatusStripGradientDirection = LinearGradientMode.Horizontal;
                StatusStripGradientBegin = begin;
                StatusStripGradientEnd = end;
                if (blend != null)
                    StatusStripGradientBlend = blend;
            }
            else if (StatusStripStyle == ToolStripType.VerticalGradient)
            {
                StatusStripGradientDirection = LinearGradientMode.Vertical;
                StatusStripGradientBegin = begin;
                StatusStripGradientEnd = end;
                if (blend != null)
                    StatusStripGradientBlend = blend;
            }
            else
            {
                StatusStripGradientBegin = begin;
                StatusStripGradientEnd = end;
            }
        }

        /// <summary>
        /// Sets the Button hover style effect. 
        /// </summary>
        /// <param name="style">The button hover style effect.</param>
        public void SetButtonHoverStyle(ButtonHoverType style)
        {
            ButtonHoverEffect = style;
        }
        #endregion

        #region Overridden Methods
        protected override void Initialize(ToolStrip toolStrip)
        {
            if (toolStrip is MenuStrip)
            {
                if (toolStrip.BackgroundImage != null)
                    MenuStripStyle = ToolStripType.Image;
                toolStrip.BackgroundImageChanged += new EventHandler(toolStrip_BackgroundImageChanged);
            }
            else if (toolStrip is StatusStrip)
            {
                if (toolStrip.BackgroundImage != null)
                    StatusStripStyle = ToolStripType.Image;
                toolStrip.BackgroundImageChanged += new EventHandler(toolStrip_BackgroundImageChanged);
            }
            else
            {
                if (toolStrip.BackgroundImage != null)
                    ToolStripStyle = ToolStripType.Image;
                toolStrip.BackgroundImageChanged += new EventHandler(toolStrip_BackgroundImageChanged);
            }
            base.Initialize(toolStrip);
        }

        protected override void InitializeContentPanel(ToolStripContentPanel contentPanel)
        {
            base.InitializeContentPanel(contentPanel);
        }

        protected override void InitializeItem(ToolStripItem item)
        {
            base.InitializeItem(item);
            if (item is ToolStripComboBox)
            {
                ToolStripComboBox combo = (ToolStripComboBox)item;
                _comboBoxExtender = new ComboBoxExtender(this, item.Owner, combo);
            }
        }

        protected override void InitializePanel(ToolStripPanel toolStripPanel)
        {
            base.InitializePanel(toolStripPanel);
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            // draw the arrow
            drawArrow(e.Graphics, e.ArrowRectangle);
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            ToolStripButton button = (ToolStripButton)e.Item;
            Rectangle bounds = new Rectangle(Point.Empty, button.Bounds.Size);

            // only draw if pressed
            if (button.Pressed || button.Checked)
                drawPressedButton(e.Graphics, bounds);
            else
                base.OnRenderButtonBackground(e);
        }

        protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
        {
            ToolStripDropDownButton button = (ToolStripDropDownButton)e.Item;
            Rectangle bounds = new Rectangle(Point.Empty, button.Bounds.Size);

            // only draw if pressed
            if (button.Pressed)
                drawPressedButton(e.Graphics, bounds);
            else
                base.OnRenderDropDownButtonBackground(e);
        }

        protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
        {
            bool vert = (e.GripDisplayStyle == ToolStripGripDisplayStyle.Vertical);

            if (e.GripStyle == ToolStripGripStyle.Visible)
            {
                Rectangle bounds = e.GripBounds;
                // grip direction
                if (vert)
                {
                    bounds.X = e.AffectedBounds.X;
                    bounds.Width = e.AffectedBounds.Width;
                    if (e.ToolStrip is MenuStrip)
                    {
                        // always returns vertical on menustrip: bug in GripDisplayStyle property?
                        if (e.AffectedBounds.Height > e.AffectedBounds.Width)
                        {
                            vert = false;
                            bounds.Y = e.AffectedBounds.Y;
                        }
                        else
                        {
                            // adjust to even margins tool-menu
                            e.ToolStrip.GripMargin = new Padding(0, 2, 0, 2);
                            bounds.Y = e.AffectedBounds.Y;
                            bounds.Height = e.AffectedBounds.Height;
                        }
                    }
                    else
                    {
                        e.ToolStrip.GripMargin = new Padding(2, 2, 4, 2);
                        bounds.X++;
                        bounds.Width++;
                    }
                }
                else
                {
                    bounds.Y = e.AffectedBounds.Y;
                    bounds.Height = e.AffectedBounds.Height;
                }
                // draw the selected style
                if ((GripStyle == GripType.Dotted) || (GripStyle == GripType.LargeDot))
                    drawDottedGrip(e.Graphics, bounds, vert);
                else
                    drawSolidGrip(e.Graphics, bounds, vert);
            }
        }

        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            // draw the margin area on a menu
            if ((e.ToolStrip is ContextMenuStrip) || (e.ToolStrip is ToolStripDropDownMenu))
                drawImageMargin(e.Graphics, e.AffectedBounds);
            else
                base.OnRenderImageMargin(e);
        }

        protected override void OnRenderItemBackground(ToolStripItemRenderEventArgs e)
        {
            base.OnRenderItemBackground(e);
        }

        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            // test for menu
            if (e.Item is ToolStripMenuItem)
            {
                // draw the checkbox
                ToolStripMenuItem toolItem = (ToolStripMenuItem)e.Item;
                drawItemCheck(e.Graphics, e.ImageRectangle, e.Image, toolItem.CheckState);
            }
            else
            {
                base.OnRenderItemCheck(e);
            }
        }

        protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
        {
            // image rendering
            if ((e.ToolStrip is ContextMenuStrip) || (e.ToolStrip is ToolStripDropDownMenu))
            {
                if (e.Image != null)
                {
                    if (e.Item.Enabled)
                    {
                        e.Graphics.DrawImage(e.Image, e.ImageRectangle);
                    }
                    else
                    {
                        ControlPaint.DrawImageDisabled(e.Graphics, e.Image,
                                                       e.ImageRectangle.X,
                                                       e.ImageRectangle.Y,
                                                       Color.Transparent);
                    }
                }
            }
            else if (e.ToolStrip is ToolStrip)
            {
                // brighten button image
                if ((e.Item.Image != null) && (e.Item.Selected))
                {
                    try
                    {
                        using (Bitmap buttonImage = new Bitmap(e.Item.Image))
                        {
                            using (ImageAttributes imageAttr = new ImageAttributes())
                            {
                                imageAttr.SetGamma(.8F);
                                e.Graphics.DrawImage(buttonImage,
                                    e.ImageRectangle,
                                    0, 0,
                                    buttonImage.Width,
                                    buttonImage.Height,
                                    GraphicsUnit.Pixel,
                                    imageAttr);
                            }
                        }
                    }
                    catch
                    {
                        base.OnRenderItemImage(e);
                    }
                }
                else
                {
                    base.OnRenderItemImage(e);
                }
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            if ((e.ToolStrip is MenuStrip) || (e.ToolStrip is ToolStrip) ||
                (e.ToolStrip is ContextMenuStrip) || (e.ToolStrip is ToolStripDropDownMenu))
            {
                if (!e.Item.Enabled)
                {
                    // draw disabled
                    e.TextColor = Color.FromKnownColor(KnownColor.InactiveCaptionText);
                }
                else
                {
                    if (e.ToolStrip is MenuStrip)
                    {
                        // menu text
                        if (e.Item.Pressed || e.Item.Selected)
                            e.TextColor = MenuFocusedForeColor;
                        else
                            e.TextColor = MenuForeColor;
                    }
                    else if ((e.ToolStrip is ContextMenuStrip) || (e.ToolStrip is ToolStripDropDownMenu))
                    {
                        if (MenuStyle == MenuType.Vista)
                        {
                            if (e.Item.Pressed || e.Item.Selected)
                                e.TextColor = Color.Black;
                            else
                                e.TextColor = Color.FromArgb(255, 0x77, 0x77, 0x77);
                        }
                        else
                        {
                            if (e.Item.Pressed || e.Item.Selected)
                                e.TextColor = MenuItemFocusedForeColor;
                            else
                                e.TextColor = MenuItemForeColor;
                        }
                    }
                    else
                    {
                        if (e.Item.Pressed || e.Item.Selected)
                            e.TextColor = ButtonFocusedForeColor;
                        else
                            e.TextColor = ButtonForeColor;
                    }
                }
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                base.OnRenderItemText(e);
            }
            else
            {
                base.OnRenderItemText(e);
            }
        }

        protected override void OnRenderLabelBackground(ToolStripItemRenderEventArgs e)
        {
            base.OnRenderLabelBackground(e);
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            Rectangle bounds = new Rectangle(Point.Empty, e.Item.Bounds.Size);

            if (e.ToolStrip is MenuStrip)
            {
                // draw menu item panel segment
                drawMenuItemBackground(e.Graphics, bounds, getButtonState(e.ToolStrip, e.Item));
            }
            else
            {
                // menu item focus bar
                if (getButtonState(e.ToolStrip, e.Item) == ButtonSelectedState.Focused)
                {
                    drawSelectorBar(e.Graphics, bounds, (e.ToolStrip.RightToLeft == RightToLeft.Yes));
                }
                else
                    e.Item.ForeColor = MenuForeColor;
            }
        }

        protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
        {
            ToolStripOverflowButton button = (ToolStripOverflowButton)e.Item;
            Rectangle bounds = new Rectangle(Point.Empty, button.Bounds.Size);
            bounds.Height--;

            if (ToolStripStyle != ToolStripType.System)
            {
                if (button.Pressed || button.Selected)
                {
                    drawPressedButton(e.Graphics, bounds);
                }
            }
            drawOverflowArrow(e.Graphics, bounds);
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            Rectangle bounds = new Rectangle(Point.Empty, e.Item.Bounds.Size);
            Color inner = Color.FromArgb(140, SeperatorInnerColor);
            Color outer = SeperatorOuterColor;

            if ((e.ToolStrip is ContextMenuStrip) || (e.ToolStrip is ToolStripDropDownMenu))
            {
                if (MenuStyle == MenuType.Vista)
                {
                    inner = Color.FromArgb(220, 245, 245, 245);
                    outer = Color.FromArgb(220, 197, 197, 197);
                }
                if (e.ToolStrip.RightToLeft == RightToLeft.Yes)
                {
                    bounds.Width -= 22;
                    bounds.X = 2;
                    drawSeperator(e.Graphics, bounds, inner, outer, e.Vertical);
                }
                else
                {
                    bounds.X += 22;
                    drawSeperator(e.Graphics, bounds, inner, outer, e.Vertical);
                }
            }
            else
            {
                drawSeperator(e.Graphics, bounds, inner, outer, e.Vertical);
            }
        }

        protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
        {
            ToolStripSplitButton button = (ToolStripSplitButton)e.Item;
            Rectangle bounds = button.DropDownButtonBounds; 
            bounds.X += (bounds.Width - 4) / 2;

            if (button.DropDownButtonPressed)
            {
                drawPressedButton(e.Graphics, button.DropDownButtonBounds);
                drawArrow(e.Graphics, bounds);
            }
            else if (button.ButtonPressed)
            {
                drawPressedButton(e.Graphics, button.ButtonBounds);
            }
            else if (button.IsOnDropDown)
            {
                drawPressedButton(e.Graphics, button.DropDownButtonBounds);
                drawArrow(e.Graphics, bounds);
            }
        }

        protected override void OnRenderStatusStripSizingGrip(ToolStripRenderEventArgs e)
        {
            Rectangle bounds = e.AffectedBounds;

            // draw the selected style
            if (GripStyle == GripType.Dotted)
                drawDottedStatusGrip(e.Graphics, e.AffectedBounds);
            else
                drawSolidStatusGrip(e.Graphics, e.AffectedBounds);
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip is MenuStrip)
            {
                // if image or system style skip drawing
                if ((MenuStripStyle == ToolStripType.Image) || (ToolStripStyle == ToolStripType.System))
                    base.OnRenderToolStripBackground(e);
                else
                    drawToolStripBackground(e.Graphics, e.ConnectedArea, e.ToolStrip);
            }
            else if ((e.ToolStrip is ContextMenuStrip) || (e.ToolStrip is ToolStripDropDownMenu))
            {
                drawToolStripBackground(e.Graphics, e.ConnectedArea, e.ToolStrip);
            }
            else if (e.ToolStrip is StatusStrip)
            {
                if ((StatusStripStyle == ToolStripType.Image) || (ToolStripStyle == ToolStripType.System))
                    base.OnRenderToolStripBackground(e);
                else
                    drawToolStripBackground(e.Graphics, e.ConnectedArea, e.ToolStrip);
            }
            else
            {
                if ((ToolStripStyle == ToolStripType.Image) || (ToolStripStyle == ToolStripType.System))
                    base.OnRenderToolStripBackground(e);
                else
                    drawToolStripBackground(e.Graphics, e.ConnectedArea, e.ToolStrip);
            }
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            Rectangle bounds = e.AffectedBounds;
            bounds.Width--;
            bounds.Height--;

            if (e.ToolStrip is MenuStrip)
            {
                if (e.ConnectedArea.IsEmpty)
                    drawToolStripBorder(e.Graphics, bounds, MenuStripBorderColor);
                else
                    drawToolStripBorder(e.Graphics, e.AffectedBounds, MenuStripBorderColor);
            }
            else if (e.ToolStrip is StatusStrip)
            {
                drawToolStripBorder(e.Graphics, bounds, StatusStripBorderColor);
            }
            else if (e.ToolStrip is ToolStrip)
            {
                if (e.ConnectedArea.IsEmpty)
                {
                    drawToolStripBorder(e.Graphics, bounds, ToolStripBorderColor);
                }
            }
        }

        protected override void OnRenderToolStripContentPanelBackground(ToolStripContentPanelRenderEventArgs e)
        {
            base.OnRenderToolStripContentPanelBackground(e);
        }

        protected override void OnRenderToolStripPanelBackground(ToolStripPanelRenderEventArgs e)
        {
            base.OnRenderToolStripPanelBackground(e);
        }

        protected override void OnRenderToolStripStatusLabelBackground(ToolStripItemRenderEventArgs e)
        {
            base.OnRenderToolStripStatusLabelBackground(e);
        }
        #endregion

        #region Drawing Methods
        private void drawArrow(Graphics g, Rectangle bounds)
        {
            // get the center
            int top = ((bounds.Height - 8) / 2) + 2;
            using (GraphicsMode mode = new GraphicsMode(g, SmoothingMode.AntiAlias))
            {
                using (GraphicsPath gp = new GraphicsPath())
                {
                    // draw the frame
                    gp.AddLine(new Point(bounds.X, top), new Point(bounds.X + 4, top));
                    gp.AddLine(new Point(bounds.X, top), new Point(bounds.X + 2, top + 2));
                    gp.AddLine(new Point(bounds.X + 2, top + 2), new Point(bounds.X + 4, top));
                    gp.CloseFigure();

                    // draw border
                    using (Pen borderPen = new Pen(Color.FromArgb(240, DropArrowColor), 0.5f))
                        g.DrawPath(borderPen, gp);

                    // fill path
                    using (Brush backBrush = new SolidBrush(DropArrowColor))
                        g.FillPath(backBrush, gp);
                }
            }
        }

        private void drawDottedGrip(Graphics g, Rectangle bounds, bool vertical)
        {
            // circle bounds
            bounds.Height -= 3;
            Point position = new Point(bounds.X, bounds.Y);
            int sep;
            Rectangle posRect = new Rectangle(0, 0, 2, 2);

            using (GraphicsMode mode = new GraphicsMode(g, SmoothingMode.AntiAlias))
            {
                // draw vista style grip
                IntPtr hdc;
                if (vertical)
                {
                    sep = bounds.Height;
                    position.Y += 8;
                    for (int i = 0; position.Y > 4; i += 4)
                    {
                        position.Y = sep - (2 + i);
                        if (GripStyle == GripType.LargeDot)
                        {
                            posRect.Location = position;
                            drawCircle(g, posRect, SeperatorOuterColor, SeperatorInnerColor);
                        }
                        else
                        {
                            hdc = g.GetHdc();
                            SetPixel(hdc, position.X, position.Y, ColorTranslator.ToWin32(SeperatorInnerColor));
                            SetPixel(hdc, position.X + 1, position.Y, ColorTranslator.ToWin32(SeperatorOuterColor));
                            SetPixel(hdc, position.X, position.Y + 1, ColorTranslator.ToWin32(SeperatorOuterColor));

                            SetPixel(hdc, position.X + 3, position.Y, ColorTranslator.ToWin32(SeperatorInnerColor));
                            SetPixel(hdc, position.X + 4, position.Y, ColorTranslator.ToWin32(SeperatorOuterColor));
                            SetPixel(hdc, position.X + 3, position.Y + 1, ColorTranslator.ToWin32(SeperatorOuterColor));
                            g.ReleaseHdc(hdc);
                        }
                    }
                }
                // horizontal orientation
                else
                {
                    bounds.Inflate(-2, 0);
                    sep = bounds.Width;
                    position.X += 2;
                    for (int i = 1; position.X > 0; i += 4)
                    {
                        position.X = sep - (2 + i);
                        if (GripStyle == GripType.LargeDot)
                        {
                            posRect.Location = position;
                            drawCircle(g, posRect, SeperatorOuterColor, SeperatorInnerColor);
                        }
                        else
                        {
                            hdc = g.GetHdc();
                            SetPixel(hdc, position.X, position.Y, ColorTranslator.ToWin32(SeperatorInnerColor));
                            SetPixel(hdc, position.X + 1, position.Y, ColorTranslator.ToWin32(SeperatorOuterColor));
                            SetPixel(hdc, position.X, position.Y + 1, ColorTranslator.ToWin32(SeperatorOuterColor));

                            SetPixel(hdc, position.X + 3, position.Y, ColorTranslator.ToWin32(SeperatorInnerColor));
                            SetPixel(hdc, position.X + 4, position.Y, ColorTranslator.ToWin32(SeperatorOuterColor));
                            SetPixel(hdc, position.X + 3, position.Y + 1, ColorTranslator.ToWin32(SeperatorOuterColor));
                            g.ReleaseHdc(hdc);
                        }
                    }
                }
            }
        }

        private void drawDottedStatusGrip(Graphics g, Rectangle bounds)
        {
            // circle bounds
            Rectangle shape = new Rectangle(0, 0, 2, 2);
            shape.X = bounds.Width - 17;
            shape.Y = bounds.Height - 8;
            using (GraphicsMode mode = new GraphicsMode(g, SmoothingMode.AntiAlias))
            {
                // lowest outer circle
                drawCircle(g, shape, SeperatorOuterColor, SeperatorInnerColor);
                shape.X = bounds.Width - 12;
                drawCircle(g, shape, SeperatorOuterColor, SeperatorInnerColor);
                shape.X = bounds.Width - 7;
                drawCircle(g, shape, SeperatorOuterColor, SeperatorInnerColor);
                shape.Y = bounds.Height - 13;
                drawCircle(g, shape, SeperatorOuterColor, SeperatorInnerColor);
                shape.Y = bounds.Height - 18;
                drawCircle(g, shape, SeperatorOuterColor, SeperatorInnerColor);
                shape.Y = bounds.Height - 13;
                shape.X = bounds.Width - 12;
                drawCircle(g, shape, SeperatorOuterColor, SeperatorInnerColor);
            }
        }

        private void drawImageMargin(Graphics g, Rectangle bounds)
        {
            if (MenuStyle == MenuType.Office)
            {
                // adjust the size
                bounds.Offset(1, 1);
                bounds.Width--;
                bounds.Height -= 2;
                // fill the box
                using (SolidBrush marginBrush = new SolidBrush(MenuImageMarginGradientBegin))
                    g.FillRectangle(marginBrush, bounds);
                // draw the inside border
                using (Pen penSeperator = new Pen(Color.DarkGray))
                    g.DrawLine(penSeperator, new Point(bounds.Width, bounds.Y), new Point(bounds.Width, bounds.Height));
            }
            else if (MenuStyle == MenuType.Custom)
            {
                bounds.Inflate(-1, -1);
                bounds.Height--;
                // not using a gradient
                if ((MenuImageMarginGradientBegin == MenuImageMarginGradientEnd) || (MenuImageMarginGradientEnd == Color.Magenta))
                {
                    using (SolidBrush marginBrush = new SolidBrush(MenuImageMarginGradientBegin))
                        g.FillRectangle(marginBrush, bounds);
                    // draw the inside border

                    using (Pen penSeperator = new Pen(Color.FromArgb(150, Color.DarkGray)))
                        g.DrawLine(penSeperator, new Point(bounds.Width + 1, bounds.Y), new Point(bounds.Width + 1, bounds.Height));
                }
                else
                {
                    drawBlendedGradient(g, 
                        LinearGradientMode.Horizontal,
                        MenuImageMarginGradientEnd,
                        MenuImageMarginGradientBegin, 
                        bounds, 
                        MenuStripGradientBlend);
                }
                // draw the inside border
                using (Pen penSeperator = new Pen(MenuImageMarginGradientBegin))
                    g.DrawRectangle(penSeperator, bounds);
            }
            else if (MenuStyle == MenuType.Vista)
            {
                bounds.Inflate(0, -1);
                using (SolidBrush marginBrush = new SolidBrush(Color.FromArgb(255, 0xf5, 0xf5, 0xf5)))
                    g.FillRectangle(marginBrush, bounds);
                // draw the inside border
                using (Pen penSeperator = new Pen(Color.LightGray))
                    g.DrawLine(penSeperator, new Point(bounds.Width - 1, bounds.Y), new Point(bounds.Width - 1, bounds.Height));
                // draw accent border
                using (Pen penAccent = new Pen(Color.White))
                    g.DrawLine(penAccent, new Point(bounds.Width, bounds.Y), new Point(bounds.Width, bounds.Height));
            }
            else
            {
                bounds.Inflate(0, -2);
                // draw the inside border
                using (Pen penSeperator = new Pen(SeperatorOuterColor))
                    g.DrawLine(penSeperator, new Point(bounds.Width - 1, bounds.Y), new Point(bounds.Width - 1, bounds.Height));
                // draw accent border
                using (Pen penAccent = new Pen(SeperatorInnerColor))
                    g.DrawLine(penAccent, new Point(bounds.Width, bounds.Y), new Point(bounds.Width, bounds.Height));
            }
        }

        private void drawItemCheck(Graphics g, Rectangle bounds, Image image, CheckState state)
        {
            bounds.X--;

            // border path
            using (GraphicsMode mode = new GraphicsMode(g, System.Drawing.Drawing2D.SmoothingMode.AntiAlias))
            {
                using (GraphicsPath boxPath = createRoundRectanglePath(g, bounds.X, bounds.Y, bounds.Width, bounds.Height, 1f))
                {
                    // draw the border
                    using (Pen boxPen = new Pen(MenuFocusedForeColor))
                        g.DrawPath(boxPen, boxPath);

                    // if empty fill
                    if (image == null)
                    {
                        // draw the background
                        using (SolidBrush backBrush = new SolidBrush(MenuBackGroundColor))
                            g.FillPath(backBrush, boxPath);
                    }
                    else
                    {
                        // draw the appropriate state
                        switch (state)
                        {
                            case CheckState.Checked:
                                bounds.Inflate(-3, -3);
                                // create inset path
                                using (GraphicsPath checkPath = createRoundRectanglePath(g, bounds.X, bounds.Y, bounds.Width, bounds.Height, 1f))
                                {
                                    // draw the outer region
                                    using (Region excludePath = new Region(boxPath))
                                    {
                                        excludePath.Exclude(checkPath);
                                        using (SolidBrush backBrush = new SolidBrush(MenuBackGroundColor))
                                            g.FillPath(backBrush, boxPath);
                                    }
                                    // draw the border
                                    using (Pen penBorder = new Pen(MenuStripGradientEnd))
                                        g.DrawPath(penBorder, checkPath);
                                    // draw the check
                                    drawBlendedGradient(g, LinearGradientMode.Vertical,
                                        Color.LimeGreen, Color.LightGreen, bounds, MenuStripGradientBlend);
                                }
                                break;
                            case CheckState.Indeterminate:
                                // fill disabled color
                                using (SolidBrush tickBrush = new SolidBrush(Color.FromKnownColor(KnownColor.ButtonShadow)))
                                    g.FillPath(tickBrush, boxPath);
                                break;
                        }
                    }
                }
            }
        }

        private void drawMenuItemBackground(Graphics g, Rectangle bounds, ButtonSelectedState state)
        {
            // access current theme choice
            if (MenuStyle == MenuType.Custom)
            {
                if (state == ButtonSelectedState.Pressed)
                {
                    drawPressedButton(g, bounds);
                }
                else if (state == ButtonSelectedState.Focused)
                {
                    if (ButtonHoverEffect == ButtonHoverType.Bevelled)
                        drawBevelledMask(g, bounds, 10);
                    else if (ButtonHoverEffect == ButtonHoverType.Flat)
                        drawFlatMask(g, bounds, 10);
                    else if (ButtonHoverEffect == ButtonHoverType.Glass)
                        drawGlassButton(g, bounds, 10);
                    else if (ButtonHoverEffect == ButtonHoverType.Glow)
                        drawDiffusedGlow(g, bounds, 10);
                    else if (ButtonHoverEffect == ButtonHoverType.Raised)
                        drawRaisedButton(g, bounds, 10);
                }
            }
            // draw a flat menu
            else if (MenuStyle == MenuType.Office)
            {
                if (state == ButtonSelectedState.Pressed)
                {
                    // draw connecting frame 
                    using (Pen framePen = new Pen(Color.LightGray))
                    {
                        g.DrawLine(framePen, new Point(bounds.X, bounds.Y), new Point(bounds.X, bounds.Height));
                        g.DrawLine(framePen, new Point(bounds.X + 1, bounds.Y), new Point(bounds.Width - 1, bounds.Y));
                        g.DrawLine(framePen, new Point(bounds.Width - 1, bounds.Y + 1), new Point(bounds.Width - 1, bounds.Height - 1));
                    }

                    bounds.Inflate(-1, -1);
                    using (Brush backBrush = new SolidBrush(MenuImageMarginGradientBegin))
                        g.FillRectangle(backBrush, bounds);
                }
                else if (state == ButtonSelectedState.Focused)
                {
                    drawOfficeMenuButton(g, bounds);
                }
            }
            // draw office style menu
            else if (MenuStyle == MenuType.Flat)
            {
                // flat style menu button
                if ((state == ButtonSelectedState.Pressed) || (state == ButtonSelectedState.Focused))
                    drawFlatMask(g, bounds, 10);
            }
            // draw vista style menu
            else if (MenuStyle == MenuType.Vista)
            {
                // draw a raised style menu button
                if ((state == ButtonSelectedState.Pressed) || (state == ButtonSelectedState.Focused))
                    drawVistaMenuButton(g, bounds);
            }
        }

        private void drawVistaMenuButton(Graphics g, Rectangle bounds)
        {
            using (Brush fillBrush = new SolidBrush(ButtonGradientBegin))
                g.FillRectangle(fillBrush, bounds);
        }

        private void drawOfficeMenuButton(Graphics g, Rectangle bounds)
        {
            bounds.Width--;
            bounds.Height--;
            using (Pen framePen = new Pen(ButtonGradientBegin))
                g.DrawRectangle(framePen, bounds);
            bounds.Inflate(-1, -1);
            bounds.Width++;
            bounds.Height++;
            using (Brush fillBrush = new SolidBrush(ButtonGradientEnd))
                g.FillRectangle(fillBrush, bounds);
        }

        private void drawOverflowArrow(Graphics g, Rectangle bounds)
        {
            // get the center
            int top = bounds.Y += 6;
            bounds.X += 6;
            using (GraphicsPath gp = new GraphicsPath())
            {
                // draw the frame
                gp.AddLine(new Point(bounds.X, top), new Point(bounds.X, top + 5));
                gp.AddLine(new Point(bounds.X, top + 5), new Point(bounds.X + 2, top + 3));
                gp.AddLine(new Point(bounds.X + 2, top + 3), new Point(bounds.X, top));
                gp.CloseFigure();

                // draw border
                using (Pen borderPen = new Pen(Color.FromArgb(240, DropArrowColor), 0.5f))
                    g.DrawPath(borderPen, gp);

                // fill path
                using (Brush backBrush = new SolidBrush(DropArrowColor))
                    g.FillPath(backBrush, gp);
            }
        }

        private void drawSelectorBar(Graphics g, Rectangle bounds, bool align)
        {
            bounds.Inflate(-2, -1);
            using (GraphicsMode mode = new GraphicsMode(g, SmoothingMode.AntiAlias))
            {
                using (GraphicsPath barPath = createRoundRectanglePath(g, bounds.X, bounds.Y, bounds.Width, bounds.Height, 1.0f))
                {
                    // draw the border
                    using (Pen borderPen = new Pen(MenuSelectorBarEdge, 0.5f))
                        g.DrawPath(borderPen, barPath);

                    // fill with a gradient
                    using (LinearGradientBrush fillBrush = new LinearGradientBrush(bounds, Color.FromArgb(150, MenuSelectorBarGradientBegin), Color.FromArgb(50, MenuSelectorBarGradientEnd), LinearGradientMode.Vertical))
                    {
                        fillBrush.Blend = MenuStripGradientBlend;
                        // fill the path
                        g.FillPath(fillBrush, barPath);
                    }
                }
            }
        }

        private void drawSeperator(Graphics g, Rectangle bounds, Color light, Color dark, bool vertical)
        {
            // draw inner and outer lines
            using (Pen inner = new Pen(light), outer = new Pen(dark))
            {
                if (vertical)
                {
                    g.DrawLine(outer, new Point(bounds.X + 3, bounds.Y + 2), new Point(bounds.X + 3, bounds.Height - 3));
                    g.DrawLine(inner, new Point(bounds.X + 4, bounds.Y + 3), new Point(bounds.X + 4, bounds.Height - 4));
                }
                else
                {
                    g.DrawLine(outer, new Point(bounds.X + 4, bounds.Y + 1), new Point(bounds.Width - 4, bounds.Y + 1));
                    g.DrawLine(inner, new Point(bounds.X + 4, bounds.Y + 2), new Point(bounds.Width - 4, bounds.Y + 2));
                }
            }
        }

        private void drawSolidGrip(Graphics g, Rectangle bounds, bool vertical)
        {
            if (vertical)
            {
                using (GraphicsMode mode = new GraphicsMode(g, SmoothingMode.AntiAlias))
                {
                    // create the path
                    using (GraphicsPath gripPath = createRoundRectanglePath(g, bounds.X, bounds.Y + 2, bounds.X + 2, bounds.Height - 6, 1f))
                    {
                        // draw the frame
                        using (Pen outer = new Pen(Color.FromArgb(150, SeperatorOuterColor), 0.5f))
                        {
                            g.DrawPath(outer, gripPath);
                            // fiil the path
                            using (Brush inner = new SolidBrush(Color.FromArgb(100, SeperatorInnerColor)))
                                g.FillPath(inner, gripPath);
                        }
                        // draw an accent
                        using (Pen accent = new Pen(Color.FromArgb(75, Color.Snow), .5f))
                            g.DrawLine(accent, new PointF(bounds.X + 2, bounds.Y + 4), new PointF(bounds.X + 2, bounds.Height - 4));
                    }
                }
            }
            else
            {
                using (GraphicsMode mode = new GraphicsMode(g, SmoothingMode.AntiAlias))
                {
                    using (GraphicsPath gripPath = createRoundRectanglePath(g, bounds.X + 2, bounds.Y, bounds.Width - 4, bounds.Y + 3, 1f))
                    {
                        using (Pen outer = new Pen(SeperatorOuterColor))
                        {
                            g.DrawPath(outer, gripPath);
                            using (Brush inner = new SolidBrush(Color.FromArgb(200, SeperatorInnerColor)))
                                g.FillPath(inner, gripPath);
                        }
                        using (Pen accent = new Pen(Color.FromArgb(90, Color.Snow), .5f))
                            g.DrawLine(accent, new PointF(bounds.X + 4, bounds.Y + 1), new PointF(bounds.Width - 4, bounds.Y + 1));
                    }
                }
            }
        }

        private void drawSolidStatusGrip(Graphics g, Rectangle bounds)
        {
            using (GraphicsMode mode = new GraphicsMode(g, SmoothingMode.AntiAlias))
            {
                using (Pen inner = new Pen(SeperatorInnerColor), outer = new Pen(SeperatorOuterColor))
                {
                    //outer line
                    g.DrawLine(outer, new Point(bounds.Width - 14, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 16));
                    g.DrawLine(inner, new Point(bounds.Width - 13, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 15));
                    // line
                    g.DrawLine(outer, new Point(bounds.Width - 12, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 14));
                    g.DrawLine(inner, new Point(bounds.Width - 11, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 13));
                    // line
                    g.DrawLine(outer, new Point(bounds.Width - 10, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 12));
                    g.DrawLine(inner, new Point(bounds.Width - 9, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 11));
                    // line
                    g.DrawLine(outer, new Point(bounds.Width - 8, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 10));
                    g.DrawLine(inner, new Point(bounds.Width - 7, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 9));
                    // inner line
                    g.DrawLine(outer, new Point(bounds.Width - 6, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 8));
                    g.DrawLine(inner, new Point(bounds.Width - 5, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 7));
                }
            }
        }

        private void drawToolStripBackground(Graphics g, Rectangle area, ToolStrip toolStrip)
        {
            Rectangle bounds = new Rectangle(Point.Empty, toolStrip.Bounds.Size);

            // right to left offset
            int offset;
            if (toolStrip.RightToLeft == RightToLeft.Yes)
                offset = 2;
            else
                offset = 25;

            if (!bounds.IsEmpty)
            {
                // draw the menustrip gradient
                if ((toolStrip is MenuStrip))
                {
                    bounds.Inflate(-1, -1);
                    if ((MenuStripStyle == ToolStripType.HorizontalGradient) || (MenuStripStyle == ToolStripType.VerticalGradient))
                    {
                        drawBlendedGradient(
                            g,
                            gradientDirection(toolStrip, MenuStripGradientDirection),
                            ToolStripGradientBegin,
                            ToolStripGradientEnd,
                            bounds,
                            MenuStripGradientBlend);
                    }
                    else if (MenuStripStyle == ToolStripType.FlatColor)
                    {
                        using (Brush fillBrush = new SolidBrush(MenuStripGradientBegin))
                            g.FillRectangle(fillBrush, bounds);
                    }
                }
                else if ((toolStrip is ContextMenuStrip) || (toolStrip is ToolStripDropDownMenu))
                {
                    // fill the area with the bc
                    if (MenuStyle == MenuType.Flat)
                    {
                        drawFrame(g, bounds, Color.LightGray, Color.DarkGray);
                        using (Brush fillBrush = new SolidBrush(MenuBackGroundColor))
                        {
                            bounds.Inflate(-1, -1);
                            g.FillRectangle(fillBrush, bounds);
                        }
                    }
                    // draw the background & panel
                    else if ((MenuStyle == MenuType.Office))
                    {
                        if (area.IsEmpty)
                        {
                            bounds.Inflate(-1, -1);// JU: changed e.AffectedBounds to bounds
                            drawFrame(g, bounds, Color.LightGray, Color.DarkGray);
                            using (Brush fillBrush = new SolidBrush(MenuBackGroundColor))
                            {
                                bounds.Inflate(-1, -1);
                                bounds.X = offset;
                                g.FillRectangle(fillBrush, bounds);
                            }
                        }
                        else
                        {
                            Rectangle clipBounds = area;
                            clipBounds.Inflate(-1, -1);
                            using (Region clipRegion = new Region(clipBounds))
                            {
                                g.SetClip(clipRegion, CombineMode.Exclude);
                                drawFrame(g, bounds, Color.LightGray, Color.DarkGray);
                                g.ResetClip();
                            }

                            using (Pen coverPen = new Pen(MenuImageMarginGradientBegin, 2f))
                                g.DrawLine(coverPen, new Point(area.X, area.Y), new Point(offset, area.Y));

                            using (Pen borderPen = new Pen(Color.DarkGray, 2f))
                            {
                                g.DrawLine(borderPen, new Point(offset, area.Y), new Point(area.Width + 1, area.Y));
                                g.DrawLine(borderPen, new Point(offset, bounds.Y), new Point(offset, bounds.Height - 1));
                            }

                            using (Brush fillBrush = new SolidBrush(MenuBackGroundColor))
                            {
                                bounds.Inflate(-1, -1);
                                bounds.X = offset;
                                g.FillRectangle(fillBrush, bounds);
                            }
                        }
                    }
                    else if (MenuStyle == MenuType.Vista)
                    {
                        drawFrame(g, bounds, Color.LightGray, Color.DarkGray);
                        using (Brush fillBrush = new SolidBrush(Color.FromArgb(255, 0xf5, 0xf5, 0xf5)))
                        {
                            bounds.Inflate(-1, -1);
                            bounds.X = 25;
                            g.FillRectangle(fillBrush, bounds);
                        }
                    }
                    // draw flat background
                    else if (MenuStyle == MenuType.Custom)
                    {
                        drawFrame(g, bounds, Color.LightGray, Color.DarkGray);
                        using (Brush fillBrush = new SolidBrush(MenuBackGroundColor))
                        {
                            bounds.Inflate(-1, -1);
                            bounds.X = 25;
                            g.FillRectangle(fillBrush, bounds);
                        }
                    }
                }
                // draw the statusstrip bg
                else if ((toolStrip is StatusStrip))
                {
                    bounds.Inflate(-1, -1);
                    if ((StatusStripStyle == ToolStripType.HorizontalGradient) || (StatusStripStyle == ToolStripType.VerticalGradient))
                    {
                        drawBlendedGradient(
                            g,
                            gradientDirection(toolStrip, StatusStripGradientDirection),
                            ToolStripGradientBegin,
                            ToolStripGradientEnd,
                            bounds,
                            MenuStripGradientBlend);
                    }
                    else if (StatusStripStyle == ToolStripType.FlatColor)
                    {
                        using (Brush fillBrush = new SolidBrush(StatusStripGradientBegin))
                            g.FillRectangle(fillBrush, bounds);
                    }
                }
                // draw the toolstrip bg
                else if ((toolStrip is ToolStrip))
                {
                    bounds.Inflate(-1, -1);
                    if ((ToolStripStyle == ToolStripType.HorizontalGradient) || (ToolStripStyle == ToolStripType.VerticalGradient))
                    {
                        drawBlendedGradient(
                            g,
                            gradientDirection(toolStrip, ToolStripGradientDirection),
                            ToolStripGradientBegin,
                            ToolStripGradientEnd,
                            bounds,
                            MenuStripGradientBlend);
                    }
                    else if (ToolStripStyle == ToolStripType.FlatColor)
                    {
                        using (Brush fillBrush = new SolidBrush(ToolStripGradientBegin))
                            g.FillRectangle(fillBrush, bounds);
                    }
                }
            }
        }

        private void drawToolStripBorder(Graphics g, Rectangle bounds, Color border)
        {
            // draw the frame
            using (Pen borderPen = new Pen(border))
                g.DrawRectangle(borderPen, bounds);
        }
        #endregion

        #region Helpers
        private GraphicsPath createRoundRectanglePath(Graphics g, float X, float Y, float width, float height, float radius)
        {
            // create a path
            GraphicsPath pathBounds = new GraphicsPath();
            pathBounds.AddLine(X + radius, Y, X + width - (radius * 2), Y);
            pathBounds.AddArc(X + width - (radius * 2), Y, radius * 2, radius * 2, 270, 90);
            pathBounds.AddLine(X + width, Y + radius, X + width, Y + height - (radius * 2));
            pathBounds.AddArc(X + width - (radius * 2), Y + height - (radius * 2), radius * 2, radius * 2, 0, 90);
            pathBounds.AddLine(X + width - (radius * 2), Y + height, X + radius, Y + height);
            pathBounds.AddArc(X, Y + height - (radius * 2), radius * 2, radius * 2, 90, 90);
            pathBounds.AddLine(X, Y + height - (radius * 2), X, Y + radius);
            pathBounds.AddArc(X, Y, radius * 2, radius * 2, 180, 90);
            pathBounds.CloseFigure();
            return pathBounds;
        }

        private void drawBlendedGradient(Graphics g, LinearGradientMode mode, Color begin, Color end, Rectangle rc, Blend bp)
        {
            using (LinearGradientBrush hb = new LinearGradientBrush(
                rc,
                begin,
                end,
                mode))
            {
                hb.Blend = bp;
                g.FillRectangle(hb, rc);
            }
        }

        private void drawBevelledMask(Graphics g, RectangleF bounds, int opacity)
        {
            // initial bounds
            bounds.Inflate(-1, -1);
            // use antialias
            using (GraphicsMode mode = new GraphicsMode(g, SmoothingMode.AntiAlias))
            {
                // create outline path
                using (GraphicsPath buttonPath = createRoundRectanglePath(
                    g,
                    bounds.X, bounds.Y,
                    bounds.Width, bounds.Height,
                    2))
                {
                    // draw the outline
                    using (Pen borderPen = new Pen(Color.FromArgb(opacity * 10, ButtonBorderColor), 0.5f))
                        g.DrawPath(borderPen, buttonPath);
                }

                // create inner path
                bounds.Inflate(-1, -1);
                using (GraphicsPath buttonPath = createRoundRectanglePath(
                    g,
                    bounds.X, bounds.Y,
                    bounds.Width, bounds.Height,
                    2))
                {
                    // draw the bevel
                    using (LinearGradientBrush bevelBrush = new LinearGradientBrush(
                        bounds,
                        Color.FromArgb(opacity * 10, ToolStripGradientBegin),
                        Color.FromArgb(opacity * 10, ToolStripGradientEnd),
                        LinearGradientMode.ForwardDiagonal))
                    {
                        using (Pen bevelPen = new Pen(bevelBrush, 1f))
                            g.DrawPath(bevelPen, buttonPath);

                    }
                    // fill the path
                    using (LinearGradientBrush gradientBrush = new LinearGradientBrush(
                        bounds,
                        Color.FromArgb(opacity * 5, Color.White),
                        Color.FromArgb(opacity * 5, Color.LightGray),
                        LinearGradientMode.ForwardDiagonal))
                    {
                        gradientBrush.Blend = ToolStripGradientBlend;
                        g.FillPath(gradientBrush, buttonPath);
                    }
                }
            }
        }

        private void drawCircle(Graphics g, Rectangle bounds, Color borderColor, Color fillColor)
        {
            using (GraphicsPath circlePath = new GraphicsPath())
            {
                // create the path
                circlePath.AddEllipse(bounds);
                circlePath.CloseFigure();
                // draw outer edge
                using (Pen borderPen = new Pen(borderColor))
                    g.DrawPath(borderPen, circlePath);
                // fill path
                using (Brush backBrush = new SolidBrush(fillColor))
                    g.FillPath(backBrush, circlePath);
            }
        }

        private void drawDiffusedGlow(Graphics g, RectangleF bounds, int opacity)
        {
            using (GraphicsMode mode = new GraphicsMode(g, SmoothingMode.AntiAlias))
            {
                using (GraphicsPath diffusedPath = createRoundRectanglePath(
                    g,
                    bounds.X, bounds.Y,
                    bounds.Width, bounds.Height,
                    2))
                {
                    using (Pen borderPen = new Pen(Color.FromArgb(opacity * 5, ButtonBorderColor), 0.8f))
                        g.DrawPath(borderPen, diffusedPath);
                }

                bounds.Inflate(-1, -1);
                using (GraphicsPath diffusedPath = createRoundRectanglePath(
                    g,
                    bounds.X, bounds.Y,
                    bounds.Width, bounds.Height,
                    2))
                {
                    using (Pen borderPen = new Pen(Color.FromArgb(opacity * 10, ButtonBorderColor), 0.8f))
                        g.DrawPath(borderPen, diffusedPath);
                }

                bounds.Inflate(-1, -1);
                using (GraphicsPath diffusedPath = createRoundRectanglePath(
                    g,
                    bounds.X, bounds.Y,
                    bounds.Width, bounds.Height,
                    2))
                {
                    using (Pen borderPen = new Pen(Color.FromArgb(opacity * 20, ButtonBorderColor), 0.8f))
                        g.DrawPath(borderPen, diffusedPath);
                }
            }
        }

        private void drawFlatMask(Graphics g, RectangleF bounds, int opacity)
        {
            // initial bounds
            bounds.Inflate(-1, -1);
            // draw using anti alias
            using (GraphicsMode mode = new GraphicsMode(g, SmoothingMode.AntiAlias))
            {
                // create the path
                using (GraphicsPath buttonPath = createRoundRectanglePath(
                    g,
                    bounds.X, bounds.Y,
                    bounds.Width, bounds.Height,
                    1f))
                {
                    // draw the outer edge
                    using (Pen borderPen = new Pen(Color.FromArgb(opacity * 15, ButtonBorderColor), .5f))
                        g.DrawPath(borderPen, buttonPath);
                }
                bounds.Inflate(-1, -1);

                using (GraphicsPath buttonPath = createRoundRectanglePath(
                    g,
                    bounds.X, bounds.Y,
                    bounds.Width, bounds.Height,
                    1f))
                {
                    // draw the inner edge
                    using (Pen borderPen = new Pen(Color.FromArgb(opacity * 5, ButtonBorderColor), 1.0f))
                        g.DrawPath(borderPen, buttonPath);

                    // create a thin gradient cover
                    using (LinearGradientBrush fillBrush = new LinearGradientBrush(
                        bounds,
                        Color.FromArgb(opacity * 5, ButtonGradientBegin),
                        Color.FromArgb(opacity * 5, Color.LightGray),
                        LinearGradientMode.Vertical))
                    {
                        // shift the blend factors
                        Blend blend = new Blend();
                        blend.Positions = new float[] { 0f, .3f, .6f, 1f };
                        blend.Factors = new float[] { 0f, .5f, .8f, .2f };
                        fillBrush.Blend = blend;
                        // fill the path
                        g.FillPath(fillBrush, buttonPath);
                    }
                }
            }
        }

        private void drawFrame(Graphics g, Rectangle bounds, Color light, Color dark)
        {
            using (Pen lightPen = new Pen(light), darkPen = new Pen(dark))
            {
                g.DrawLines(lightPen, new Point[] {
                        new Point (0, bounds.Height - 1), 
                        new Point (0, 0), 
                        new Point (bounds.Width - 1, 0)});
                g.DrawLines(darkPen, new Point[] {
                        new Point (0, bounds.Height - 1), 
                        new Point (bounds.Width - 1, bounds.Height - 1), 
                        new Point (bounds.Width - 1, 0)});
            }
        }

        private void drawGlassButton(Graphics g, RectangleF bounds, int opacity)
        {
            // initial bounds
            bounds.Inflate(-1, -1);
            // draw using anti alias
            using (GraphicsMode mode = new GraphicsMode(g, SmoothingMode.AntiAlias))
            {
                // draw the border around the button
                using (GraphicsPath buttonPath = createRoundRectanglePath(
                    g,
                    bounds.X, bounds.Y,
                    bounds.Width, bounds.Height,
                    1f))
                {
                    using (LinearGradientBrush borderBrush = new LinearGradientBrush(
                        bounds,
                        Color.FromArgb(opacity * 20, ButtonGradientEnd),
                        Color.FromArgb(opacity * 20, ButtonGradientBegin),
                        90f))
                    {
                        borderBrush.SetSigmaBellShape(0.5f);
                        using (Pen borderPen = new Pen(borderBrush, .5f))
                            g.DrawPath(borderPen, buttonPath);
                    }

                    // create a clipping region
                    RectangleF clipBounds = bounds;
                    clipBounds.Inflate(-1, -1);
                    using (GraphicsPath clipPath = createRoundRectanglePath(
                        g,
                        clipBounds.X, clipBounds.Y,
                        clipBounds.Width, clipBounds.Height,
                        1f))
                    {
                        using (Region region = new Region(clipPath))
                            g.SetClip(region, CombineMode.Exclude);
                    }

                    // fill in the edge accent
                    using (LinearGradientBrush edgeBrush = new LinearGradientBrush(
                        bounds,
                        Color.FromArgb(opacity * 15, ButtonBorderColor),
                        Color.FromArgb(opacity * 5, Color.Black),
                        90f))
                    {
                        edgeBrush.SetBlendTriangularShape(0.1f);
                        g.FillPath(edgeBrush, buttonPath);
                        g.ResetClip();
                        bounds.Inflate(-1, -1);
                    }

                    // fill the button with a subtle glow
                    using (LinearGradientBrush fillBrush = new LinearGradientBrush(
                        bounds,
                        Color.FromArgb(opacity * 10, Color.White),
                        Color.FromArgb(opacity * 5, ButtonGradientBegin),
                        LinearGradientMode.ForwardDiagonal))
                    {
                        fillBrush.SetBlendTriangularShape(0.4f);
                        g.FillPath(fillBrush, buttonPath);
                        g.ResetClip();
                    }
                }
            }
        }

        private void drawPressedButton(Graphics g, RectangleF bounds)
        {
            // initial bounds
            bounds.Inflate(-1, -1);
            // use anti alias
            using (GraphicsMode mode = new GraphicsMode(g, SmoothingMode.AntiAlias))
            {
                // draw the outer border
                using (GraphicsPath diffusedPath = createRoundRectanglePath(
                    g,
                    bounds.X, bounds.Y,
                    bounds.Width, bounds.Height,
                    1f))
                {
                    using (Pen borderPen = new Pen(Color.FromArgb(150, ButtonBorderColor), 0.5f))
                        g.DrawPath(borderPen, diffusedPath);
                }

                // draw the inner shading
                bounds.Inflate(-0.5f, -0.5f);
                using (GraphicsPath diffusedPath = createRoundRectanglePath(
                    g,
                    bounds.X, bounds.Y,
                    bounds.Width, bounds.Height,
                    1f))
                {
                    using (Pen borderPen = new Pen(Color.FromArgb(100, Color.Black), 0.5f))
                        g.DrawPath(borderPen, diffusedPath);
                }

                bounds.Inflate(-0.5f, -0.5f);
                using (GraphicsPath diffusedPath = createRoundRectanglePath(
                    g,
                    bounds.X, bounds.Y,
                    bounds.Width, bounds.Height,
                    1f))
                {
                    using (Pen borderPen = new Pen(Color.FromArgb(50, Color.Black), 0.5f))
                        g.DrawPath(borderPen, diffusedPath);
                    // draw a dim mask over the button
                    using (Brush fillBrush = new SolidBrush(Color.FromArgb(40, ToolStripGradientEnd)))
                        g.FillPath(fillBrush, diffusedPath);
                }
            }
        }

        private void drawRaisedButton(Graphics g, RectangleF bounds, int opacity)
        {
            bounds.Width--;
            bounds.Y++;
            bounds.Height -= 2;

            // draw using anti alias
            using (GraphicsMode mode = new GraphicsMode(g, SmoothingMode.AntiAlias))
            {
                // create the path
                using (GraphicsPath buttonPath = createRoundRectanglePath(
                    g,
                    bounds.X, bounds.Y,
                    bounds.Width, bounds.Height,
                    1.0f))
                {
                    // draw the outer edge
                    using (Pen borderPen = new Pen(Color.FromArgb(opacity * 15, Color.SlateGray), .5f))
                        g.DrawPath(borderPen, buttonPath);
                }
                bounds.Inflate(-1, -1);

                using (GraphicsPath buttonPath = createRoundRectanglePath(
                    g,
                    bounds.X, bounds.Y,
                    bounds.Width, bounds.Height,
                    1.0f))
                {
                    // draw the inner edge
                    using (Pen borderPen = new Pen(Color.FromArgb(opacity * 15, ButtonBorderColor), 1.5f))
                        g.DrawPath(borderPen, buttonPath);

                    // create a thin gradient cover
                    using (LinearGradientBrush fillBrush = new LinearGradientBrush(
                        bounds,
                        Color.FromArgb(opacity * 5, Color.FromArgb(200, Color.White)),
                        Color.FromArgb(opacity * 5, ButtonGradientEnd),
                        LinearGradientMode.Vertical))
                    {
                        // shift the blend factors
                        Blend blend = new Blend();
                        blend.Positions = new float[] { 0f, .3f, .6f, 1f };
                        blend.Factors = new float[] { 0f, .5f, .8f, .2f };
                        fillBrush.Blend = blend;
                        // fill the path
                        g.FillPath(fillBrush, buttonPath);
                    }
                }
            }
        }

        private ButtonSelectedState getButtonState(ToolStrip toolStrip, ToolStripItem item)
        {
            if (!item.Enabled)
                return ButtonSelectedState.Disabled;

            if (item is ToolStripButton)
            {
                ToolStripButton button = (ToolStripButton)item;
                if (button.Checked)
                    return ButtonSelectedState.Checked;
            }

            if (item.Pressed)
            {
                return ButtonSelectedState.Pressed;
            }
            else
            {
                Point cursorPos = toolStrip.PointToClient(Cursor.Position);
                if ((item.Visible) && (item.Bounds.Contains(cursorPos)))
                    return ButtonSelectedState.Focused;
                else
                    return ButtonSelectedState.None;
            }
        }

        private Rectangle getImageRect(ToolStripItem item)
        {
            Rectangle itemBounds = new Rectangle();
            if (item is ToolStripSplitButton)
            {
                ToolStripSplitButton button = (ToolStripSplitButton)item;
                itemBounds = button.Bounds;
            }
            else if (item is ToolStripDropDownButton)
            {
                ToolStripDropDownButton button = (ToolStripDropDownButton)item;
                itemBounds = button.Bounds;
                if (button.RightToLeft == RightToLeft.Yes)
                    itemBounds.Inflate(5, 0);
                else
                    itemBounds.Width -= 8;
            }
            else if (item is ToolStripButton)
            {
                ToolStripButton button = (ToolStripButton)item;
                itemBounds = button.Bounds;
            }
            Size imageSize = item.Owner.ImageScalingSize;
            return new Rectangle((itemBounds.Width - imageSize.Width) / 2,
                (itemBounds.Height - imageSize.Height) / 2,
                imageSize.Width, imageSize.Height);
        }

        private LinearGradientMode gradientDirection(ToolStrip toolStrip, LinearGradientMode gradientSetting)
        {
            if ((toolStrip.LayoutStyle == ToolStripLayoutStyle.Flow) && (gradientSetting == LinearGradientMode.Vertical))
            {
                if (toolStrip.Height > toolStrip.Width)
                    return LinearGradientMode.Horizontal;
                else
                    return LinearGradientMode.Vertical;
            }
            else if ((toolStrip.LayoutStyle == ToolStripLayoutStyle.Flow) && (gradientSetting == LinearGradientMode.Horizontal))
            {
                if (toolStrip.Height > toolStrip.Width)
                    return LinearGradientMode.Vertical;
                else
                    return LinearGradientMode.Horizontal;
            }
            else if ((toolStrip.LayoutStyle == ToolStripLayoutStyle.Table) && (gradientSetting == LinearGradientMode.Vertical))
            {
                if (toolStrip.Height > toolStrip.Width)
                    return LinearGradientMode.Horizontal;
                else
                    return LinearGradientMode.Vertical;
            }
            else if ((toolStrip.LayoutStyle == ToolStripLayoutStyle.Table) && (gradientSetting == LinearGradientMode.Horizontal))
            {
                if (toolStrip.Height > toolStrip.Width)
                    return LinearGradientMode.Vertical;
                else
                    return LinearGradientMode.Horizontal;
            }
            else if ((toolStrip.Orientation == Orientation.Horizontal) && (gradientSetting == LinearGradientMode.Vertical))
                return LinearGradientMode.Vertical;
            else if ((toolStrip.Orientation == Orientation.Vertical) && (gradientSetting == LinearGradientMode.Vertical))
                return LinearGradientMode.Horizontal;
            else if ((toolStrip.Orientation == Orientation.Vertical) && (gradientSetting == LinearGradientMode.Horizontal))
                return LinearGradientMode.Vertical;
            else if ((toolStrip.Orientation == Orientation.Horizontal) && (gradientSetting == LinearGradientMode.Horizontal))
                return LinearGradientMode.Horizontal;
            else
                return gradientSetting;
        }
        #endregion
        #endregion

        #region Finalizer
        public void Dispose()
        {
            if (_comboBoxExtender != null)
                _comboBoxExtender.Dispose();
            if (_toolStripExtender != null)
                _toolStripExtender.Dispose();
            if (_toolTip != null)
                _toolTip.Dispose();

        }

        ~vtExtender()
        {
            Dispose();
        }
        #endregion
        #endregion

        #region GraphicsMode
        internal class GraphicsMode : IDisposable
        {
            #region Instance Fields
            private Graphics _graphicCopy;
            private SmoothingMode _oldMode;
            #endregion

            #region Identity
            /// <summary>
            /// Initialize a new instance of the class.
            /// </summary>
            /// <param name="g">Graphics instance.</param>
            /// <param name="mode">Desired Smoothing mode.</param>
            public GraphicsMode(Graphics g, SmoothingMode mode)
            {
                _graphicCopy = g;
                _oldMode = _graphicCopy.SmoothingMode;
                _graphicCopy.SmoothingMode = mode;
            }

            /// <summary>
            /// Revert the SmoothingMode to original setting.
            /// </summary>
            public void Dispose()
            {
                _graphicCopy.SmoothingMode = _oldMode;
            }
            #endregion
        }
        #endregion

        #region ComboBoxExtender
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        internal class ComboBoxExtender : NativeWindow
        {
            #region Structs
            [StructLayout(LayoutKind.Sequential)]
            private struct RECT
            {
                internal RECT(int X, int Y, int Width, int Height)
                {
                    this.Left = X;
                    this.Top = Y;
                    this.Right = Width;
                    this.Bottom = Height;
                }
                internal int Left;
                internal int Top;
                internal int Right;
                internal int Bottom;
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct PAINTSTRUCT
            {
                internal IntPtr hdc;
                internal int fErase;
                internal RECT rcPaint;
                internal int fRestore;
                internal int fIncUpdate;
                internal int Reserved1;
                internal int Reserved2;
                internal int Reserved3;
                internal int Reserved4;
                internal int Reserved5;
                internal int Reserved6;
                internal int Reserved7;
                internal int Reserved8;
            }
            #endregion

            #region API
            [DllImport("user32.dll")]
            private static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT ps);

            [DllImport("user32.dll")]
            private static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT ps);
            #endregion

            #region Fields
            internal bool _bPainting = false;
            internal bool _bMoved = false;
            internal ToolStripComboBox _comboBox;
            internal ToolStrip _toolStrip;
            internal vtExtender _parentClass;
            #endregion

            #region Constructor
            public ComboBoxExtender(Object sender, ToolStrip toolStrip, ToolStripComboBox comboBox)
            {
                _parentClass = (vtExtender)sender;
                _toolStrip = toolStrip;
                _comboBox = comboBox;
                this.AssignHandle(_comboBox.ComboBox.Handle);
            }
            #endregion

            #region Properties
            internal vtExtender Parent
            {
                get { return _parentClass; }
            }

            internal ToolStrip ToolStrip
            {
                get { return _toolStrip; }
            }
            #endregion

            #region Methods
            private void drawCombo(ToolStripComboBox combo, bool focused)
            {
                Rectangle bounds;
                Rectangle clientBounds = combo.ComboBox.ClientRectangle;
                clientBounds.Height--;

                if (combo.DroppedDown)
                    focused = true;
                if (combo.IsOnOverflow)
                {
                    //bounds.X = 2;
                }
                if (combo.RightToLeft == RightToLeft.Yes)
                    bounds = new Rectangle(new Point(0, 0), new Size(14, combo.Height));
                else
                    bounds = new Rectangle(new Point(combo.Width - 14, 0), new Size(14, combo.Height));

                using (Graphics g = Graphics.FromHwnd(combo.ComboBox.Handle))
                {
                    // prepaint entire background
                    Rectangle flatRect = clientBounds;
                    if (combo.RightToLeft == RightToLeft.Yes)
                        flatRect.X += 14;
                    else
                        flatRect.Width -= 14;
                    flatRect.Inflate(-1, -1);
                    flatRect.Height++;
                    using (Brush flatBrush = new SolidBrush(Color.White))
                        g.FillRectangle(flatBrush, flatRect);

                    if (Parent.ToolStripStyle == ToolStripType.FlatColor)
                    {
                        // fill the background
                        using (Brush flatBrush = new SolidBrush(Parent.ButtonGradientBegin))
                            g.FillRectangle(flatBrush, bounds);
                        // draw a focused mask
                        if (focused)
                        {
                            bounds.Inflate(0, -1);
                            using (Brush flatBrush = new SolidBrush(Color.FromArgb(100, Color.White)))
                                g.FillRectangle(flatBrush, bounds);
                            bounds.Inflate(0, 1);
                        }
                        // frame the button
                        using (Pen framePen = new Pen(Color.FromArgb(100, Parent.SeperatorOuterColor), 0.5f))
                            g.DrawRectangle(framePen, clientBounds);
                        bounds.Width--;
                        // draw a border
                        using (Pen borderPen = new Pen(Color.FromArgb(200, Parent.SeperatorOuterColor), 0.5f))
                            g.DrawRectangle(borderPen, bounds);
                    }
                    else if (Parent.ToolStripStyle == ToolStripType.Image)
                    {
                        // blit the image in
                        if (ToolStrip.BackgroundImage != null)
                        {
                            g.DrawImage(ToolStrip.BackgroundImage, bounds, bounds, GraphicsUnit.Pixel);
                        }
                        if (focused)
                        {
                            // fill in the edge accent
                            using (LinearGradientBrush fillBrush = new LinearGradientBrush(bounds, Color.FromArgb(50, Parent.SeperatorInnerColor), Color.FromArgb(50, Parent.SeperatorOuterColor), 90f))
                            {
                                fillBrush.SetBlendTriangularShape(0.2f);
                                g.FillRectangle(fillBrush, bounds);
                            }
                        }
                        // draw the frame
                        using (Pen framePen = new Pen(Color.FromArgb(150, Color.Black), 0.5f))
                            g.DrawRectangle(framePen, clientBounds);
                        bounds.Width--;
                        using (Pen borderPen = new Pen(Color.FromArgb(200, Color.Black), 0.5f))
                            g.DrawRectangle(borderPen, bounds);
                    }
                    else
                    {
                        // fill in the edge accent
                        using (LinearGradientBrush fillBrush = new LinearGradientBrush(bounds, Parent.ToolStripGradientBegin, Parent.ToolStripGradientEnd, Parent.gradientDirection(_toolStrip, Parent.ToolStripGradientDirection)))
                        {
                            fillBrush.Blend = Parent.ToolStripGradientBlend;
                            g.FillRectangle(fillBrush, bounds);
                        }
                        // draw a focused mask
                        if (focused)
                        {
                            // fill in the mask
                            using (LinearGradientBrush fillBrush = new LinearGradientBrush(bounds, Color.FromArgb(50, Color.LightGray), Color.FromArgb(50, Color.White), 90f))
                            {
                                fillBrush.SetBlendTriangularShape(0.2f);
                                g.FillRectangle(fillBrush, bounds);
                            }
                        }
                        using (Pen framePen = new Pen(Color.FromArgb(150, Color.Black), 0.5f))
                            g.DrawRectangle(framePen, clientBounds);
                        bounds.Width--;
                        using (Pen borderPen = new Pen(Color.FromArgb(200, Color.Black), 0.5f))
                            g.DrawRectangle(borderPen, bounds);
                    }
                    drawComboArrow(g, bounds);
                }
            }

            private void drawComboArrow(Graphics g, Rectangle bounds)
            {
                // get the center
                int top = (bounds.Height / 2) - 8;
                bounds.X += 4;
                using (GraphicsPath gp = new GraphicsPath())
                {
                    // draw the frame
                    gp.AddLine(new Point(bounds.X, top), new Point(bounds.X + 4, top));
                    gp.AddLine(new Point(bounds.X, top), new Point(bounds.X + 2, top + 2));
                    gp.AddLine(new Point(bounds.X + 2, top + 2), new Point(bounds.X + 4, top));
                    gp.CloseFigure();

                    // draw border
                    using (Pen borderPen = new Pen(Parent.DropArrowColor))
                        g.DrawPath(borderPen, gp);

                    // fill path
                    using (Brush backBrush = new SolidBrush(Parent.DropArrowColor))
                        g.FillPath(backBrush, gp);
                }
            }

            private bool isOnItem(Rectangle bounds)
            {
                Point cursorPos = ToolStrip.PointToClient(Cursor.Position);
                if (bounds.Contains(cursorPos))
                    return true;
                return false;
            }

            public void Dispose()
            {
                this.ReleaseHandle();
            }
            #endregion

            #region WndProc

            protected override void WndProc(ref Message m)
            {
                PAINTSTRUCT tPaint = new PAINTSTRUCT();
                switch (m.Msg)
                {
                    case WM_PAINT:
                        if (Parent.ToolStripStyle == ToolStripType.System)
                        {
                            base.WndProc(ref m);
                        }
                        else
                        {
                            if (!_bPainting)
                            {
                                _bPainting = true;
                                // start painting engine
                                BeginPaint(m.HWnd, ref tPaint);
                                drawCombo(_comboBox, _bMoved);
                                // done
                                EndPaint(m.HWnd, ref tPaint);
                                _bPainting = false;
                            }
                            else
                            {
                                base.WndProc(ref m);
                            }
                        }
                        break;

                    case WM_MOUSEMOVE:
                        _bMoved = true;
                        base.WndProc(ref m);
                        break;

                    case WM_MOUSELEAVE:
                        _bMoved = false;
                        base.WndProc(ref m);
                        break;

                    default:
                        base.WndProc(ref m);
                        break;
                }
            }
            #endregion
        }
        #endregion

        #region ToolStripExtender
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        internal class ToolStripExtender : NativeWindow, IDisposable
        {
            #region API
            [DllImport("user32.dll")]
            internal static extern bool GetUpdateRect(IntPtr hWnd, out RECT rect, bool bErase);

            [DllImport("user32.dll")]
            internal static extern bool ValidateRect(IntPtr hWnd, ref RECT lpRect);

            [DllImport("gdi32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
            #endregion

            #region Struct
            [StructLayout(LayoutKind.Sequential)]
            internal struct RECT
            {
                internal RECT(int X, int Y, int Width, int Height)
                {
                    this.Left = X;
                    this.Top = Y;
                    this.Right = Width;
                    this.Bottom = Height;
                }
                internal int Left;
                internal int Top;
                internal int Right;
                internal int Bottom;
            }
            #endregion

            #region Fields
            internal ToolStrip _toolStrip;
            internal vtExtender _parentClass;
            internal Dictionary<ToolStripItem, FadeTimer> _fader = new Dictionary<ToolStripItem, FadeTimer>();
            #endregion

            #region Constructor
            public ToolStripExtender(Object sender, ToolStrip toolstrip)
            {
                _parentClass = (vtExtender)sender;
                _toolStrip = toolstrip;
                _toolStrip.Renderer = _parentClass;
                this.AssignHandle(_toolStrip.Handle);
                _toolStrip.ItemAdded += new ToolStripItemEventHandler(ToolStrip_ItemAdded);
                _toolStrip.ItemRemoved += new ToolStripItemEventHandler(ToolStrip_ItemRemoved);
                addItems();
            }
            #endregion

            #region Event Handlers
            internal void FadeTimer_Complete(object sender)
            {
                FadeTimer fader = (FadeTimer)sender;
                fader.Reset();
                Rectangle bounds = new Rectangle(Point.Empty, fader.ToolItem.Bounds.Size);
                //
                fader.ToolItem.Invalidate(bounds);
                //drawBackGround(fader.ToolItem.Bounds, fader.ButtonDc.Hdc);
            }

            internal void FadeTimer_Tick(object sender)
            {
                FadeTimer fader = (FadeTimer)sender;
                drawButtonFader(fader.ToolItem, fader.ButtonDc.Hdc, fader.TickCount);
            }

            internal void Item_MouseDown(object sender, MouseEventArgs e)
            {
                ToolStripItem item = (ToolStripItem)sender;
                if (_fader[item] != null)
                {
                    _fader[item].Reset();
                    drawBackGround(item.Bounds, _fader[item].ButtonDc.Hdc);
                }
            }

            internal void Item_MouseEnter(object sender, EventArgs e)
            {
                if (ToolStrip.Visible)
                {
                    ToolStripItem item = (ToolStripItem)sender;
                    if ((item != null) && (item.Visible) && (!item.Pressed) && (hasFader(item)))
                    {
                        if (item is ToolStripButton)
                        {
                            ToolStripButton button = (ToolStripButton)item;
                            if ((!button.Checked) && (item.Enabled))
                                _fader[item].Fade(FadeType.FadeIn);
                        }
                        else
                        {
                            if ((item.Enabled) && ((!item.IsOnDropDown) || (!item.IsOnOverflow)))
                                _fader[item].Fade(FadeType.FadeIn);
                        }
                    }
                }
            }

            internal void Item_MouseLeave(object sender, EventArgs e)
            {
                ToolStripItem item = (ToolStripItem)sender;
                if ((item != null) && (hasFader(item)) && (_fader[item] != null))
                {
                    if (_fader[item].IsReset)
                    {
                        _fader[item].Fade(FadeType.FadeFast);
                        _fader[item].TickCount = 0;
                    }
                    else
                    {
                        if (getState(item) == ItemState.OffControl)
                            _fader[item].Fade(FadeType.FadeFast);
                        else
                            _fader[item].Fade(FadeType.FadeOut);
                    }
                }
            }

            internal void Item_MouseUp(object sender, MouseEventArgs e)
            {
                ToolStripItem item = (ToolStripItem)sender;
                item.Invalidate();
            }

            internal void ToolStrip_ItemRemoved(object sender, ToolStripItemEventArgs e)
            {
                removeItemEvents(e.Item);
            }

            internal void ToolStrip_ItemAdded(object sender, ToolStripItemEventArgs e)
            {
                addItemEvents(e.Item);
            }
            #endregion

            #region Properties
            internal vtExtender Parent
            {
                get { return _parentClass; }
            }

            internal ToolStrip ToolStrip
            {
                get { return _toolStrip; }
            }
            #endregion

            #region Drawing
            internal void drawBackGround(Rectangle itemRect, IntPtr hdc)
            {
                Graphics g = Graphics.FromHwnd(Handle);
                BitBlt(g.GetHdc(), itemRect.X, itemRect.Y, itemRect.Width, itemRect.Height, hdc, 0, 0, 0xCC0020);
                g.ReleaseHdc();
                g.Dispose();
            }

            internal void drawButtonFader(ToolStripItem item, IntPtr hdc, int opacity)
            {
                // create a buffered temporary canvas
                Rectangle bounds = item.Bounds;
                cStoreDc backDc = new cStoreDc();
                backDc.Height = bounds.Height;
                backDc.Width = bounds.Width;
                // create graphics
                using (Graphics g = Graphics.FromHdc(backDc.Hdc))
                {
                    // blit in the background
                    BitBlt(backDc.Hdc, 0, 0, bounds.Width, bounds.Height, hdc, 0, 0, 0xCC0020);
                    RectangleF boundsF = new RectangleF(Point.Empty, bounds.Size);
                    if ((item is ToolStripButton) || (item is ToolStripDropDownButton) || (item is ToolStripOverflowButton))
                    {
                        if (Parent.ButtonHoverEffect == ButtonHoverType.Flat)
                            Parent.drawFlatMask(g, boundsF, opacity);
                        else if (Parent.ButtonHoverEffect == ButtonHoverType.Glass)
                            Parent.drawGlassButton(g, boundsF, opacity);
                        else if (Parent.ButtonHoverEffect == ButtonHoverType.Glow)
                            Parent.drawDiffusedGlow(g, boundsF, opacity);
                        else if (Parent.ButtonHoverEffect == ButtonHoverType.Raised)
                            Parent.drawRaisedButton(g, boundsF, opacity);
                        else if (Parent.ButtonHoverEffect == ButtonHoverType.Bevelled)
                            Parent.drawBevelledMask(g, boundsF, opacity);
                    }
                    else if (item is ToolStripSplitButton)
                    {
                        ToolStripSplitButton button = (ToolStripSplitButton)item;
                        RectangleF buttonF = button.ButtonBounds;
                        RectangleF dropF = button.DropDownButtonBounds;
                        if (Parent.ButtonHoverEffect == ButtonHoverType.Flat)
                        {
                            Parent.drawFlatMask(g, dropF, opacity);
                            Parent.drawFlatMask(g, buttonF, opacity);
                        }
                        else if (Parent.ButtonHoverEffect == ButtonHoverType.Glass)
                        {
                            Parent.drawGlassButton(g, dropF, opacity);
                            Parent.drawGlassButton(g, buttonF, opacity);
                        }
                        else if (Parent.ButtonHoverEffect == ButtonHoverType.Glow)
                        {
                            Parent.drawDiffusedGlow(g, dropF, opacity);
                            Parent.drawDiffusedGlow(g, buttonF, opacity);
                        }
                        else if (Parent.ButtonHoverEffect == ButtonHoverType.Raised)
                        {
                            Parent.drawRaisedButton(g, dropF, opacity);
                            Parent.drawRaisedButton(g, buttonF, opacity);
                        }
                        else if (Parent.ButtonHoverEffect == ButtonHoverType.Bevelled)
                        {
                            Parent.drawBevelledMask(g, dropF, opacity);
                            Parent.drawBevelledMask(g, buttonF, opacity);
                        }
                        bounds = button.DropDownButtonBounds;
                        bounds.X += (bounds.Width - 4) / 2;
                        Parent.drawArrow(g, bounds);
                    }
                }
                bounds = item.Bounds;
                // blit to screen
                using (Graphics g = Graphics.FromHwnd(ToolStrip.Handle))
                {
                    BitBlt(g.GetHdc(), bounds.X, bounds.Y, bounds.Width, bounds.Height, backDc.Hdc, 0, 0, 0xCC0020);
                    g.ReleaseHdc();
                    backDc.Dispose();
                }
            }
            #endregion

            #region Methods
            internal void addItems()
            {
                foreach (ToolStripItem item in ToolStrip.Items)
                {
                    addItemEvents(item);
                }
            }

            internal void addItemEvents(ToolStripItem item)
            {
                try
                {
                    if (hasFader(item))
                    {
                        item.MouseEnter += new EventHandler(Item_MouseEnter);
                        item.MouseLeave += new EventHandler(Item_MouseLeave);
                        item.MouseDown += new MouseEventHandler(Item_MouseDown);
                        item.MouseUp += new MouseEventHandler(Item_MouseUp);
                        _fader.Add(item, new FadeTimer(item, ToolStrip));
                        _fader[item].Tick += new FadeTimer.TickDelegate(FadeTimer_Tick);
                        _fader[item].Complete += new FadeTimer.CompleteDelegate(FadeTimer_Complete);
                    }
                }
                catch { }
            }

            internal bool bypassPaint()
            {
                Rectangle updateRect = updateRegion();
                foreach (ToolStripItem item in ToolStrip.Items)
                {
                    if ((_fader.ContainsKey(item)) && (_fader[item].TickCount > 0) && (!item.IsOnOverflow) && (!_fader[item].Invalidating) && (updateRect.IntersectsWith(item.Bounds)))
                    {
                        if (((_fader[item].FadeStyle == FadeType.FadeOut) || (_fader[item].FadeStyle == FadeType.FadeFast)))
                        {
                            RECT validRect = new RECT(updateRect.Left, updateRect.Top, updateRect.Right, updateRect.Bottom);
                            ValidateRect(ToolStrip.Handle, ref validRect);
                            return true;
                        }
                    }
                }
                return false;
            }

            internal bool isOnItem(Rectangle bounds)
            {
                Point cursorPos = ToolStrip.PointToClient(Cursor.Position);
                if (bounds.Contains(cursorPos))
                    return true;
                return false;
            }

            internal ItemState getState(ToolStripItem item)
            {
                ToolStripItem tc = selectedItem();
                if (tc == null)
                    return ItemState.OffContainer;
                else if (item == tc)
                    return ItemState.ItemHover;
                else
                    return ItemState.OffControl;
            }

            internal bool hasFader(ToolStripItem item)
            {
                if ((item is ToolStripButton) || (item is ToolStripDropDownButton) || (item is ToolStripSplitButton))
                    return true;
                return false;
            }

            internal void removeItemEvents(ToolStripItem item)
            {
                try
                {
                    if (hasFader(item))
                    {
                        item.MouseEnter -= new EventHandler(Item_MouseEnter);
                        item.MouseLeave -= new EventHandler(Item_MouseLeave);
                        item.MouseDown -= new MouseEventHandler(Item_MouseDown);
                        item.MouseUp -= new MouseEventHandler(Item_MouseUp);
                        _fader.Remove(item);
                    }
                }
                catch { }
            }

            internal ToolStripItem selectedItem()
            {
                Point clientPoint = ToolStrip.PointToClient(Cursor.Position);
                return ToolStrip.GetItemAt(clientPoint);
            }

            internal Rectangle updateRegion()
            {
                RECT updateRect;
                GetUpdateRect(ToolStrip.Handle, out updateRect, false);
                return new Rectangle(updateRect.Left, updateRect.Top, updateRect.Right - updateRect.Left, updateRect.Bottom - updateRect.Top);
            }
            #endregion

            #region WndProc
            protected override void WndProc(ref Message m)
            {
                switch (m.Msg)
                {
                    // bypasses an invalidation thrown by parent when mouse leaves
                    // a button, this causes a slight flicker when drawing fader
                    // if someone knows a better way, post a message..
                    case WM_PAINT:
                        if (!bypassPaint())
                            base.WndProc(ref m);
                        else
                            m.Result = new IntPtr(1);
                        break;

                    default:
                        base.WndProc(ref m);
                        break;
                }
            }
            #endregion

            #region Finalizer
            ~ToolStripExtender()
            {
                Dispose();
            }

            public void Dispose()
            {
                try
                {
                    this.ReleaseHandle();
                    foreach (FadeTimer ft in _fader.Values)
                    {
                        ft.Dispose();
                    }
                }
                catch { }
            }

            #endregion
        }
        #endregion

        #region ToolTip
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        internal class ToolTip : NativeWindow
        {
            #region Constants
            // setwindowpos
            static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
            static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
            static readonly IntPtr HWND_TOP = new IntPtr(0);
            static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
            // size/move
            internal const uint SWP_NOSIZE = 0x0001;
            internal const uint SWP_NOMOVE = 0x0002;
            internal const uint SWP_NOZORDER = 0x0004;
            internal const uint SWP_NOREDRAW = 0x0008;
            internal const uint SWP_NOACTIVATE = 0x0010;
            internal const uint SWP_FRAMECHANGED = 0x0020;
            internal const uint SWP_SHOWWINDOW = 0x0040;
            internal const uint SWP_HIDEWINDOW = 0x0080;
            internal const uint SWP_NOCOPYBITS = 0x0100;
            internal const uint SWP_NOOWNERZORDER = 0x0200;
            internal const uint SWP_NOSENDCHANGING = 0x0400;
            // styles
            internal const int TTS_ALWAYSTIP = 0x01;
            internal const int TTS_NOPREFIX = 0x02;
            internal const int TTS_NOANIMATE = 0x10;
            internal const int TTS_NOFADE = 0x20;
            internal const int TTS_BALLOON = 0x40;
            internal const int TTS_CLOSE = 0x80;
            internal const int TTS_USEVISUALSTYLE = 0x100;
            // window messages
            internal const int WM_NOTIFY = 0x4E;
            internal const int WM_REFLECT = 0x2000;
            internal const int WM_PAINT = 0xF;
            internal const int WM_SIZE = 0x5;
            internal const int WM_MOVE = 0x3;
            internal const int WM_SETFONT = 0x30;
            internal const int WM_GETFONT = 0x31;
            internal const int WM_SHOWWINDOW = 0x18;
            internal const int WM_MOUSEMOVE = 0x200;
            internal const int WM_MOUSELEAVE = 0x2A3;
            internal const int WM_LBUTTONDOWN = 0x201;
            internal const int WM_LBUTTONUP = 0x202;
            internal const int WM_LBUTTONDBLCLK = 0x203;
            internal const int WM_RBUTTONDOWN = 0x204;
            internal const int WM_RBUTTONUP = 0x205;
            internal const int WM_RBUTTONDBLCLK = 0x206;
            internal const int WM_MBUTTONDOWN = 0x207;
            internal const int WM_MBUTTONUP = 0x208;
            internal const int WM_MBUTTONDBLCLK = 0x209;
            internal const int WM_MOUSEWHEEL = 0x20A;
            internal const int WM_TIMER = 0x113;
            internal const int WM_NCPAINT = 0x85;
            internal const int WM_DESTROY = 0x2;
            internal const int WM_SETFOCUS = 0x7;
            internal const int WM_KILLFOCUS = 0x8;
            internal const int WM_IME_NOTIFY = 0x282;
            internal const int WM_IME_SETCONTEXT = 0x281;
            internal const int WM_ACTIVATE = 0x6;
            internal const int WM_NCACTIVATE = 0x86;
            internal const int WM_STYLECHANGED = 0x7d;
            internal const int WM_STYLECHANGING = 0x7c;
            internal const int WM_WINDOWPOSCHANGING = 0x46;
            internal const int WM_WINDOWPOSCHANGED = 0x47;
            internal const int WM_NCCALCSIZE = 0x83;
            internal const int WM_CTLCOLOR = 0x3d8d610;
            // window styles
            internal const int GWL_STYLE = (-16);
            internal const int GWL_EXSTYLE = (-20);
            internal const int SS_OWNERDRAW = 0xD;
            internal const int WS_OVERLAPPED = 0x0;
            internal const int WS_TABSTOP = 0x10000;
            internal const int WS_THICKFRAME = 0x40000;
            internal const int WS_HSCROLL = 0x100000;
            internal const int WS_VSCROLL = 0x200000;
            internal const int WS_BORDER = 0x800000;
            internal const int WS_CLIPCHILDREN = 0x2000000;
            internal const int WS_CLIPSIBLINGS = 0x4000000;
            internal const int WS_VISIBLE = 0x10000000;
            internal const int WS_CHILD = 0x40000000;
            internal const int WS_POPUP = -2147483648;
            // window extended styles
            internal const int WS_EX_LTRREADING = 0x0;
            internal const int WS_EX_LEFT = 0x0;
            internal const int WS_EX_RIGHTSCROLLBAR = 0x0;
            internal const int WS_EX_DLGMODALFRAME = 0x1;
            internal const int WS_EX_NOPARENTNOTIFY = 0x4;
            internal const int WS_EX_TOPMOST = 0x8;
            internal const int WS_EX_ACCEPTFILES = 0x10;
            internal const int WS_EX_TRANSPARENT = 0x20;
            internal const int WS_EX_MDICHILD = 0x40;
            internal const int WS_EX_TOOLWINDOW = 0x80;
            internal const int WS_EX_WINDOWEDGE = 0x100;
            internal const int WS_EX_CLIENTEDGE = 0x200;
            internal const int WS_EX_CONTEXTHELP = 0x400;
            internal const int WS_EX_RIGHT = 0x1000;
            internal const int WS_EX_RTLREADING = 0x2000;
            internal const int WS_EX_LEFTSCROLLBAR = 0x4000;
            internal const int WS_EX_CONTROLPARENT = 0x10000;
            internal const int WS_EX_STATICEDGE = 0x20000;
            internal const int WS_EX_APPWINDOW = 0x40000;
            internal const int WS_EX_NOACTIVATE = 0x8000000;
            internal const int WS_EX_LAYERED = 0x80000;
            #endregion

            #region Structs
            [StructLayout(LayoutKind.Sequential)]
            internal struct RECT
            {
                internal RECT(int X, int Y, int Width, int Height)
                {
                    this.Left = X;
                    this.Top = Y;
                    this.Right = Width;
                    this.Bottom = Height;
                }
                internal int Left;
                internal int Top;
                internal int Right;
                internal int Bottom;
            }
            #endregion

            #region API
            [DllImport("user32.dll", SetLastError = true)]
            internal static extern IntPtr CreateWindowEx(int exstyle, string lpClassName, string lpWindowName, int dwStyle,
                int x, int y, int nWidth, int nHeight, IntPtr hwndParent, IntPtr Menu, IntPtr hInstance, IntPtr lpParam);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool DestroyWindow(IntPtr hWnd);

            [DllImport("user32.dll", SetLastError = false)]
            private static extern IntPtr GetDesktopWindow();

            [DllImport("user32.dll", ExactSpelling = true)]
            internal static extern IntPtr SetTimer(IntPtr hWnd, int nIDEvent, uint uElapse, IntPtr lpTimerFunc);

            [DllImport("user32.dll", ExactSpelling = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool KillTimer(IntPtr hWnd, uint uIDEvent);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndAfter, int x, int y, int cx, int cy, uint flags);

            [DllImport("user32.dll")]
            internal static extern bool GetClientRect(IntPtr hWnd, ref RECT r);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

            [DllImport("user32.dll")]
            internal static extern int GetWindowLong(IntPtr hwnd, int nIndex);

            [DllImport("user32.dll")]
            internal static extern int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool GetCursorPos(ref Point lpPoint);

            [DllImport("user32.dll")]
            internal static extern bool ScreenToClient(IntPtr hWnd, ref Point lpPoint);

            [DllImport("user32.dll")]
            internal static extern IntPtr GetDC(IntPtr handle);

            [DllImport("user32.dll")]
            internal static extern int ReleaseDC(IntPtr handle, IntPtr hdc);

            [DllImport("gdi32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
            #endregion

            #region Fields
            internal bool _timerActive = false;
            internal bool _tipShowing = false;
            internal bool _textRightToLeft = false;
            internal bool _useIcon = false;
            internal int _timerTick = 0;
            internal int _horzOffset = 4;
            internal int _vertOffset = 4;
            internal int _delayTime = 1000;
            internal int _visibleTime = 2000;
            internal string _clientCaption = String.Empty;
            internal string _clientTitle = String.Empty;
            internal Color _foreColor = Color.Black;
            internal Color _gradientBegin = Color.White;
            internal Color _gradientEnd = Color.Silver;
            internal IntPtr _hTipWnd = IntPtr.Zero;
            internal IntPtr _hInstance = IntPtr.Zero;
            internal IntPtr _hParentWnd = IntPtr.Zero;
            internal Rectangle _clientBounds = new Rectangle();
            internal Font _titleFont;
            internal Font _captionFont;
            internal Bitmap _clientImage = null;
            #endregion

            #region Constructor
            public ToolTip(IntPtr hParentWnd)
            {
                Type t = typeof(ToolTip);
                Module m = t.Module;
                _hInstance = Marshal.GetHINSTANCE(m);
                _hParentWnd = hParentWnd;
                // create window
                _hTipWnd = CreateWindowEx(WS_EX_TOPMOST | WS_EX_TOOLWINDOW,
                    "STATIC", "",
                    SS_OWNERDRAW | WS_CHILD | WS_CLIPSIBLINGS | WS_OVERLAPPED,
                    0, 0,
                    0, 0,
                    GetDesktopWindow(),
                    IntPtr.Zero, _hInstance, IntPtr.Zero);
                // set starting position
                SetWindowPos(_hTipWnd, HWND_TOP,
                    0, 0,
                    0, 0,
                    SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE | SWP_NOOWNERZORDER);
                createFonts();
                this.AssignHandle(_hTipWnd);
            }
            #endregion

            #region Properties
            internal Rectangle Bounds
            {
                get { return _clientBounds; }
                set { _clientBounds = value; }
            }

            internal string Caption
            {
                get { return _clientCaption; }
                set { _clientCaption = value; }
            }

            internal int DelayTime
            {
                get { return _delayTime; }
                set { _delayTime = value; }
            }
            internal Color ForeColor
            {
                get { return _foreColor; }
                set { _foreColor = value; }
            }

            internal Color GradientBegin
            {
                get { return _gradientBegin; }
                set { _gradientBegin = value; }
            }

            internal Color GradientEnd
            {
                get { return _gradientEnd; }
                set { _gradientEnd = value; }
            }

            internal Bitmap ItemImage
            {
                get { return _clientImage; }
                set { _clientImage = value; }
            }

            public int MaximumLength
            {
                set { _clientBounds.Width = value; }
            }

            public bool TextRightToLeft
            {
                get { return _textRightToLeft; }
                set { _textRightToLeft = value; }
            }

            internal string Title
            {
                get { return _clientTitle; }
                set { _clientTitle = value; }
            }

            internal bool UseIcon
            {
                get { return _useIcon; }
                set{ _useIcon = value; }
            }

            internal int VisibleTime
            {
                get { return _visibleTime; }
                set { _visibleTime = value; }
            }
            #endregion

            #region Public Methods
            internal void Start(string title, string caption, Bitmap image, Rectangle bounds)
            {
                if (_timerActive)
                    Stop();
                destroyImage();
                Title = title;
                Caption = caption;
                ItemImage = image;
                Bounds = bounds;
                SetTimer(_hTipWnd, 1, 100, IntPtr.Zero);
            }

            internal void Stop()
            {
                // kill the timer
                KillTimer(_hTipWnd, 1);
                // hide the window
                showWindow(false);
                // reset properties
                Title = String.Empty;
                Caption = String.Empty;
                ItemImage = null;
                Bounds = Rectangle.Empty;
                // reset timer values
                _timerTick = 0;
                _tipShowing = false;
                _timerActive = false;
            }

            public void Dispose()
            {
                if (_hTipWnd != IntPtr.Zero)
                {
                    this.ReleaseHandle();
                    destroyFonts();
                    destroyImage();
                    DestroyWindow(_hTipWnd);
                    _hTipWnd = IntPtr.Zero;
                }
            }
            #endregion

            #region Internal Methods
            internal Rectangle calculateSize()
            {
                Rectangle bounds = new Rectangle();
                int offset = 0;

                // is there an image?
                if ((ItemImage != null) && (UseIcon) && (!String.IsNullOrEmpty(Title)))
                {
                    SizeF titleSize = calcTextSize(Title, _titleFont, 0);
                    bounds.Width = ItemImage.Size.Width + (int)titleSize.Width + 12;
                    bounds.Height = ItemImage.Size.Height + 8;
                    offset = ItemImage.Size.Width + 8;
                }
                else if (!String.IsNullOrEmpty(Title))
                {
                    SizeF titleSize = calcTextSize(Title, _titleFont, 0);
                    bounds.Width = (int)titleSize.Width + 8;
                    bounds.Height = (int)titleSize.Height + 8;
                    offset = 4;
                }
                else if ((ItemImage != null) && (UseIcon))
                {
                    bounds.Height = ItemImage.Size.Height + 8;
                    bounds.Width = ItemImage.Size.Width + 8;
                    offset = ItemImage.Size.Width + 8;
                }

                // add caption size
                int width = Bounds.Width;
                if (bounds.Width > width)
                    width = bounds.Width;
                else if (bounds.Width > 0)
                    width = bounds.Width;

                SizeF captionSize = calcTextSize(Caption, _captionFont, width - offset);
                bounds.Height += (int)captionSize.Height + 4;
                bounds.Width = width + 12;
                // return the sized rectangle
                return bounds;
            }

            private SizeF calcTextSize(string text, Font font, int width)
            {
                SizeF sF = new SizeF();
                IntPtr hdc = GetDC(_hTipWnd);
                Graphics g = Graphics.FromHdc(hdc);
                if (width > 0)
                    sF = g.MeasureString(text, font, width);
                else
                    sF = g.MeasureString(text, font);
                ReleaseDC(_hTipWnd, hdc);
                g.Dispose();
                return sF;
            }

            internal void copyBackground(Graphics g)
            {
                RECT windowRect = new RECT();
                GetWindowRect(_hTipWnd, ref windowRect);
                g.CopyFromScreen(windowRect.Left, windowRect.Top, 0, 0, new Size(windowRect.Right - windowRect.Left, windowRect.Bottom - windowRect.Top), CopyPixelOperation.SourceCopy);
            }

            internal void createFonts()
            {
                _titleFont = new Font("Tahoma", 8, FontStyle.Bold);
                _captionFont = new Font("Tahoma", 8, FontStyle.Regular);
            }

            internal GraphicsPath createRoundRectanglePath(Graphics g, float X, float Y, float width, float height, float radius)
            {
                // create a path
                GraphicsPath pathBounds = new GraphicsPath();
                pathBounds.AddLine(X + radius, Y, X + width - (radius * 2), Y);
                pathBounds.AddArc(X + width - (radius * 2), Y, radius * 2, radius * 2, 270, 90);
                pathBounds.AddLine(X + width, Y + radius, X + width, Y + height - (radius * 2));
                pathBounds.AddArc(X + width - (radius * 2), Y + height - (radius * 2), radius * 2, radius * 2, 0, 90);
                pathBounds.AddLine(X + width - (radius * 2), Y + height, X + radius, Y + height);
                pathBounds.AddArc(X, Y + height - (radius * 2), radius * 2, radius * 2, 90, 90);
                pathBounds.AddLine(X, Y + height - (radius * 2), X, Y + radius);
                pathBounds.AddArc(X, Y, radius * 2, radius * 2, 180, 90);
                pathBounds.CloseFigure();
                return pathBounds;
            }

            internal void destroyFonts()
            {
                if (_titleFont != null)
                    _titleFont.Dispose();
                if (_captionFont != null)
                    _captionFont.Dispose();
            }

            internal void destroyImage()
            {
                if (ItemImage != null)
                    ItemImage.Dispose();
                ItemImage = null;
            }

            internal void drawBackground(IntPtr hdc)
            {
                // create the graphics instance
                Graphics g = Graphics.FromHdc(hdc);
                // copy in the background to mimic transparency
                copyBackground(g);
                // create the shadow rect
                Rectangle shadowArea = new Rectangle(3, Bounds.Height - 3, Bounds.Width - 3, Bounds.Height);
                // draw the bottom shadow
                using (GraphicsMode mode = new GraphicsMode(g, SmoothingMode.AntiAlias))
                {
                    using (GraphicsPath shadowPath = createRoundRectanglePath(g, 4, Bounds.Height - 4, Bounds.Width - 4, Bounds.Height, 1f))
                    {
                        using (LinearGradientBrush shadowBrush = new LinearGradientBrush(shadowArea, Color.FromArgb(100, 0x99, 0x99, 0x99), Color.FromArgb(60, 0x44, 0x44, 0x44), LinearGradientMode.Vertical))
                        {
                            Blend blend = new Blend();
                            blend.Positions = new float[] { 0f, .3f, .6f, 1f };
                            blend.Factors = new float[] { 0f, .3f, .6f, .9f };
                            shadowBrush.Blend = blend;
                            g.FillPath(shadowBrush, shadowPath);
                        }
                    }
                    // draw the right shadow
                    using (GraphicsPath shadowPath = createRoundRectanglePath(g, Bounds.Width - 4, 4, Bounds.Width - 4, Bounds.Height - 8, 1f))
                    {
                        using (LinearGradientBrush shadowBrush = new LinearGradientBrush(shadowArea, Color.FromArgb(100, 0x99, 0x99, 0x99), Color.FromArgb(60, 0x44, 0x44, 0x44), LinearGradientMode.Horizontal))
                        {
                            Blend blend = new Blend();
                            blend.Positions = new float[] { 0f, .3f, .6f, 1f };
                            blend.Factors = new float[] { 0f, .3f, .6f, .9f };
                            shadowBrush.Blend = blend;
                            g.FillPath(shadowBrush, shadowPath);
                        }
                    }
                    // adjust the bounds
                    Rectangle fillBounds = new Rectangle(0, 0, Bounds.Width - 4, Bounds.Height - 4);
                    using (GraphicsPath fillPath = createRoundRectanglePath(g, fillBounds.X, fillBounds.Y, fillBounds.Width, fillBounds.Height, 2f))
                    {
                        using (LinearGradientBrush shadowBrush = new LinearGradientBrush(shadowArea, GradientBegin, GradientEnd, LinearGradientMode.Vertical))
                        {
                            // draw the frame
                            using (Pen fillPen = new Pen(Color.FromArgb(250, 0x44, 0x44, 0x44)))
                                g.DrawPath(fillPen, fillPath);
                            // fill the body
                            Blend blend = new Blend();
                            blend.Positions = new float[] { 0f, .4f, .6f, 1f };
                            blend.Factors = new float[] { 0f, .3f, .6f, .8f };
                            shadowBrush.Blend = blend;
                            g.FillPath(shadowBrush, fillPath);
                        }
                    }
                }
                g.Dispose();
            }

            internal void drawCaption(IntPtr hdc)
            {
                Graphics g = Graphics.FromHdc(hdc);
                using (StringFormat sF = new StringFormat())
                {
                    int vOffset;
                    int hOffset;

                    if ((ItemImage != null) && (UseIcon))
                    {
                        vOffset = ItemImage.Size.Width + 8;
                        hOffset = (ItemImage.Size.Height / 2) + (_titleFont.Height);
                    }
                    else if (!String.IsNullOrEmpty(Title))
                    {
                        vOffset = 4;
                        hOffset = (_titleFont.Height + 8);
                    }
                    else
                    {
                        vOffset = 4;
                        hOffset = 8;
                    }

                    sF.Alignment = StringAlignment.Near;
                    sF.LineAlignment = StringAlignment.Near;
                    if (TextRightToLeft)
                        sF.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                    using (Brush captionBrush = new SolidBrush(ForeColor))
                        g.DrawString(Caption, _captionFont, captionBrush, new RectangleF(vOffset, hOffset, Bounds.Width - vOffset, Bounds.Height - hOffset), sF);
                }
                g.Dispose();
            }

            internal void drawIcon(IntPtr hdc)
            {
                if (ItemImage != null)
                {
                    Graphics g = Graphics.FromHdc(hdc);
                    g.DrawImage(ItemImage, new Point(4, 4));
                    g.Dispose();
                }
            }

            internal void drawTitle(IntPtr hdc)
            {
                Graphics g = Graphics.FromHdc(hdc);
                using (StringFormat sF = new StringFormat())
                {
                    int vOffset;
                    int hOffset;

                    if ((ItemImage != null) && (UseIcon))
                    {
                        vOffset = ItemImage.Size.Width + 8;
                        hOffset = (ItemImage.Size.Height / 2) + 2;
                    }
                    else
                    {
                        vOffset = 4;
                        hOffset = 8;
                    }

                    sF.Alignment = StringAlignment.Near;
                    sF.LineAlignment = StringAlignment.Center;
                    sF.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
                    sF.FormatFlags = StringFormatFlags.NoWrap;

                    if (TextRightToLeft)
                        sF.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                    using (Brush titleBrush = new SolidBrush(ForeColor))
                        g.DrawString(Title, _titleFont, titleBrush, new PointF(vOffset, hOffset), sF);
                }
                g.Dispose();
            }

            internal void positionWindow()
            {
                if (_hTipWnd != IntPtr.Zero)
                {
                    // offset with screen position
                    RECT windowRect = new RECT();
                    GetWindowRect(_hParentWnd, ref windowRect);
                    windowRect.Left += Bounds.X;
                    windowRect.Top += Bounds.Y;
                    // position the window
                    SetWindowPos(_hTipWnd, HWND_TOPMOST, windowRect.Left, windowRect.Top, Bounds.Width, Bounds.Height, SWP_SHOWWINDOW | SWP_NOACTIVATE);
                }
            }

            internal void renderTip()
            {
                if ((Caption != String.Empty) && (Bounds != Rectangle.Empty))
                {
                    // create the canvas
                    _clientBounds.Height = 50;
                    Rectangle bounds = calculateSize();
                    bounds.X = Bounds.X;
                    bounds.Y = Bounds.Y;
                    Bounds = bounds;
                    cStoreDc drawDc = new cStoreDc();
                    drawDc.Width = Bounds.Width;
                    drawDc.Height = Bounds.Height;
                    positionWindow();
                    // show the window
                    showWindow(true);
                    // draw the background to the temp dc
                    drawBackground(drawDc.Hdc);
                    // draw image and text
                    if ((ItemImage != null) && (UseIcon))
                        drawIcon(drawDc.Hdc);
                    if (Title != String.Empty)
                        drawTitle(drawDc.Hdc);
                    drawCaption(drawDc.Hdc);
                    // draw the tempdc to the window
                    IntPtr hdc = GetDC(_hTipWnd);
                    BitBlt(hdc, 0, 0, Bounds.Width, Bounds.Height, drawDc.Hdc, 0, 0, 0xCC0020);
                    ReleaseDC(_hTipWnd, hdc);
                    // cleanup
                    drawDc.Dispose();
                }

            }

            internal void showWindow(bool show)
            {
                if (show)
                    SetWindowPos(_hTipWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_SHOWWINDOW | SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOSIZE);
                else
                    SetWindowPos(_hTipWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_HIDEWINDOW | SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOSIZE);
            }
            #endregion

            #region WndProc
            protected override void WndProc(ref Message m)
            {
                switch (m.Msg)
                {
                    case WM_TIMER:
                        _timerTick++;
                        if (_timerTick > (DelayTime / 100))
                        {
                            if (!_tipShowing)
                            {
                                _tipShowing = true;
                                renderTip();
                            }
                        }
                        if (_timerTick > ((DelayTime + VisibleTime) / 100))
                            Stop();
                        base.WndProc(ref m);
                        break;

                    default:
                        base.WndProc(ref m);
                        break;
                }
            }
            #endregion
        }
        #endregion
    }
    #endregion

    #region Fade Timer
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    internal class FadeTimer : IDisposable
    {
        #region Structs
        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            internal RECT(int X, int Y, int Width, int Height)
            {
                this.Left = X;
                this.Top = Y;
                this.Right = Width;
                this.Bottom = Height;
            }
            internal int Left;
            internal int Top;
            internal int Right;
            internal int Bottom;
        }
        #endregion

        #region API
        [DllImport("user32.dll")]
        internal static extern IntPtr GetDC(IntPtr handle);

        [DllImport("user32.dll")]
        internal static extern int ReleaseDC(IntPtr handle, IntPtr hdc);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);
        #endregion

        #region Events
        public delegate void CompleteDelegate(object sender);
        public delegate void TickDelegate(object sender);
        public event CompleteDelegate Complete;
        public event TickDelegate Tick;
        #endregion

        #region Fields
        internal bool _cancelTimer;
        internal bool _isReset;
        internal int _tickCounter;
        internal int _tickMaximum;
        internal FadeType _fadeType;
        internal ToolStripItem _toolItem;
        internal cStoreDc _buttonDc = new cStoreDc();
        internal ToolStrip _parentClass;
        internal System.Timers.Timer _aTimer;
        internal bool _invalidating = false;
        #endregion

        #region Constructor
        public FadeTimer(ToolStripItem item, object sender)
        {
            _tickCounter = 0;
            _tickMaximum = 10;
            _parentClass = (ToolStrip)sender;
            _toolItem = item;
            _aTimer = new System.Timers.Timer();
            _aTimer.Interval = 50;
            _aTimer.SynchronizingObject = (ISynchronizeInvoke)sender;
            _aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        }
        #endregion

        #region Properties
        internal cStoreDc ButtonDc
        {
            get { return _buttonDc; }
            set { _buttonDc = value; }
        }

        internal bool Invalidating
        {
            get { return _invalidating; }
            set { _invalidating = value; }
        }
        internal bool IsReset
        {
            get { return _isReset; }
            set { _isReset = value; }
        }

        internal bool Cancel
        {
            get { return _cancelTimer; }
            set { _cancelTimer = value; }
        }

        internal bool Enabled
        {
            get { return _aTimer.Enabled; }
        }

        internal FadeType FadeStyle
        {
            get { return _fadeType; }
            set { _fadeType = value; }
        }

        internal int TickCount
        {
            get { return _tickCounter; }
            set { _tickCounter = value; }
        }

        internal int TickMaximum
        {
            get { return _tickMaximum; }
            set { _tickMaximum = value; }
        }

        internal ToolStripItem ToolItem
        {
            get { return _toolItem; }
            set { _toolItem = value; }
        }
        #endregion

        #region Public Methods
        public void Dispose()
        {
            Reset();
            if (_buttonDc != null)
                _buttonDc.Dispose();
            if (_aTimer != null)
                _aTimer.Dispose();
            //GC.SuppressFinalize(this);
        }

        public void Fade(FadeType ft)
        {
            Cancel = false;
            IsReset = false;
            Invalidating = false;
            _fadeType = ft;
            if (_fadeType == FadeType.FadeIn)
            {
                TickCount = 0;
                captureDc();
            }
            else if (_fadeType == FadeType.FadeOut)
                TickCount = 10;
            else if (_fadeType == FadeType.FadeFast)
                TickCount = 10;
            _aTimer.Enabled = true;
        }

        public void Stop()
        {
            _aTimer.Stop();
        }

        public void Reset()
        {
            TickCount = 0;
            _fadeType = FadeType.None;
            IsReset = true;
            _aTimer.Stop();
            _aTimer.Enabled = false;
        }
        #endregion

        #region Event Handlers
        internal void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (Cancel)
            {
                Invalidating = true;
                if (Complete != null) Complete(this);
                return;
            }
            else
            {
                switch (_fadeType)
                {
                    case FadeType.FadeIn:
                        fadeIn();
                        break;
                    case FadeType.FadeFast:
                        fadeOut();
                        break;
                    case FadeType.FadeOut:
                        fadeOut();
                        break;
                }
            }
        }
        #endregion

        #region Internal Methods
        internal void captureDc()
        {
            try
            {
                _buttonDc.Width = _toolItem.Width;
                _buttonDc.Height = _toolItem.Height;
                if (_buttonDc.Hdc != IntPtr.Zero)
                {
                    using (Graphics g = Graphics.FromHdc(_buttonDc.Hdc))
                    {
                        RECT boundedRect = new RECT();
                        GetWindowRect(_parentClass.Handle, ref boundedRect);
                        g.CopyFromScreen(boundedRect.Left + _toolItem.Bounds.X, boundedRect.Top + _toolItem.Bounds.Y, 0, 0, new Size(_buttonDc.Width, _buttonDc.Height), CopyPixelOperation.SourceCopy);
                    }
                }
            }
            catch { }
        }

        internal void fadeIn()
        {
            if (TickCount < TickMaximum)
            {
                TickCount++;
                if (Tick != null) Tick(this);
            }
            else
            {
                TickCount = TickMaximum;
            }
        }

        internal void fadeOut()
        {
            if (TickCount > 0)
            {
                if (_fadeType == FadeType.FadeFast)
                {
                    TickCount -= 2;
                    if (TickCount < 0)
                        TickCount = 0;
                }
                else
                {
                    TickCount--;
                }
                if (Tick != null) Tick(this);
            }
            else
            {
                Reset();
                Invalidating = true;
                if (Complete != null) Complete(this);
            }
        }

        ~FadeTimer()
        {
            Dispose();
        }
        #endregion
    }
    #endregion

    #region StoreDc
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    internal class cStoreDc
    {
        [DllImport("gdi32.dll")]
        internal static extern IntPtr CreateDCA([MarshalAs(UnmanagedType.LPStr)]string lpszDriver, [MarshalAs(UnmanagedType.LPStr)]string lpszDevice, [MarshalAs(UnmanagedType.LPStr)]string lpszOutput, int lpInitData);

        [DllImport("gdi32.dll")]
        internal static extern IntPtr CreateDCW([MarshalAs(UnmanagedType.LPWStr)]string lpszDriver, [MarshalAs(UnmanagedType.LPWStr)]string lpszDevice, [MarshalAs(UnmanagedType.LPWStr)]string lpszOutput, int lpInitData);

        [DllImport("gdi32.dll")]
        internal static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, int lpInitData);

        [DllImport("gdi32.dll")]
        internal static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        internal static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true)]
        internal static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject(IntPtr hObject);

        internal int _Height = 0;
        internal int _Width = 0;
        internal IntPtr _Hdc = IntPtr.Zero;
        internal IntPtr _Bmp = IntPtr.Zero;
        internal IntPtr _BmpOld = IntPtr.Zero;

        public IntPtr Hdc
        {
            get { return _Hdc; }
        }

        public IntPtr HBmp
        {
            get { return _Bmp; }
        }

        public int Height
        {
            get { return _Height; }
            set
            {
                if (_Height != value)
                {
                    _Height = value;
                    ImageCreate(_Width, _Height);
                }
            }
        }

        public int Width
        {
            get { return _Width; }
            set
            {
                if (_Width != value)
                {
                    _Width = value;
                    ImageCreate(_Width, _Height);
                }
            }
        }

        internal void ImageCreate(int Width, int Height)
        {
            IntPtr pHdc = IntPtr.Zero;

            ImageDestroy();
            pHdc = CreateDCA("DISPLAY", "", "", 0);
            _Hdc = CreateCompatibleDC(pHdc);
            _Bmp = CreateCompatibleBitmap(pHdc, _Width, _Height);
            _BmpOld = SelectObject(_Hdc, _Bmp);
            if (_BmpOld == IntPtr.Zero)
            {
                ImageDestroy();
            }
            else
            {
                _Width = Width;
                _Height = Height;
            }
            DeleteDC(pHdc);
            pHdc = IntPtr.Zero;
        }

        internal void ImageDestroy()
        {
            if (_BmpOld != IntPtr.Zero)
            {
                SelectObject(_Hdc, _BmpOld);
                _BmpOld = IntPtr.Zero;
            }
            if (_Bmp != IntPtr.Zero)
            {
                DeleteObject(_Bmp);
                _Bmp = IntPtr.Zero;
            }
            if (_Hdc != IntPtr.Zero)
            {
                DeleteDC(_Hdc);
                _Hdc = IntPtr.Zero;
            }
        }

        public void Dispose()
        {
            ImageDestroy();
        }
    }
    #endregion
}
