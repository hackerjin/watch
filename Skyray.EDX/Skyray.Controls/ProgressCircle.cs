/////////////////////////////////////////////////////////////////////////////
// ProgressCircle.cs - progress control
//
// Written by Sergio A. B. Petrovcic (sergio_petrovcic@hotmail.com)
// Copyright (c) 2008 Sergio A. B. Petrovcic.
//
// This code may be used in compiled form in any way you desire. This
// file may be redistributed by any means PROVIDING it is 
// not sold for profit without the authors written consent, and 
// providing that this notice and the authors name is included.
//
// This file is provided "as is" with no expressed or implied warranty.
// The author accepts no liability if it causes any damage to you or your
// computer whatsoever.
//
// If you find bugs, have suggestions for improvements, etc.,
// please contact the author.
//
// History (Date/Author/Description):
// ----------------------------------
//
// 2008/04/23: Sergio A. B. Petrovcic
// - Original implementation

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Skyray.Controls
{
    public partial class ProgressCircle : UserControl
    {
        public delegate void EventHandler(object sender, string message);
        public event EventHandler m_EventIncremented;
        [Category("ProgressCircle"), Description("Event raised everytime the component is incremented. Author: Sergio Augusto Bitencourt Petrovcic")]
        public event EventHandler PCIncremented
        {
            add { m_EventIncremented += new EventHandler(value); }
            remove { m_EventIncremented -= new EventHandler(value); }
        }
        public event EventHandler m_EventCompleted;
        [Category("ProgressCircle"), Description("Event raised when the component get completed. Author: Sergio Augusto Bitencourt Petrovcic")]
        public event EventHandler PCCompleted
        {
            add { m_EventCompleted += new EventHandler(value); }
            remove { m_EventCompleted -= new EventHandler(value); }
        }

        private LinearGradientMode m_eLinearGradientMode = LinearGradientMode.Vertical;
        [Category("ProgressCircle"), Description("Gradient orientation. Author: Sergio Augusto Bitencourt Petrovcic")]
        public LinearGradientMode PCLinearGradientMode
        {
            get { return m_eLinearGradientMode; }
            set { m_eLinearGradientMode = value; }
        }
        private Color m_oColor1RemainingTime = Color.Navy;
        [Category("ProgressCircle"), Description("Color 1 of remaining time. Author: Sergio Augusto Bitencourt Petrovcic")]
        public Color PCRemainingTimeColor1
        {
            get { return m_oColor1RemainingTime; }
            set { m_oColor1RemainingTime = value; }
        }
        private Color m_oColor2RemainingTime = Color.LightSlateGray;
        [Category("ProgressCircle"), Description("Color 2 of remaining time. Author: Sergio Augusto Bitencourt Petrovcic")]
        public Color PCRemainingTimeColor2
        {
            get { return m_oColor2RemainingTime; }
            set { m_oColor2RemainingTime = value; }
        }
        private Color m_oColor1ElapsedTime = Color.IndianRed;
        [Category("ProgressCircle"), Description("Color 1 of elapsed time. Author: Sergio Augusto Bitencourt Petrovcic")]
        public Color PCElapsedTimeColor1
        {
            get { return m_oColor1ElapsedTime; }
            set { m_oColor1ElapsedTime = value; }
        }
        private Color m_oColor2ElapsedTime = Color.Gainsboro;
        [Category("ProgressCircle"), Description("Color 2 of elapsed time. Author: Sergio Augusto Bitencourt Petrovcic")]
        public Color PCElapsedTimeColor2
        {
            get { return m_oColor2ElapsedTime; }
            set { m_oColor2ElapsedTime = value; }
        }
        private int m_iTotalTime = 100;
        [Category("ProgressCircle"), Description("Total time. Author: Sergio Augusto Bitencourt Petrovcic")]
        public int PCTotalTime
        {
            get { return m_iTotalTime; }
            set { m_iTotalTime = value; }
        }
        private int m_iElapsedTime = 0;
        [Category("ProgressCircle"), Description("Elapsed time. Author: Sergio Augusto Bitencourt Petrovcic")]
        public int PCElapsedTime
        {
            get { return m_iElapsedTime; }
            set { m_iElapsedTime = value; }
        }

        public ProgressCircle()
        {
            InitializeComponent();
        }

        public void Increment(int a_iValue)
        {
            if (m_iElapsedTime > m_iTotalTime)
                return;

            if (m_iElapsedTime + a_iValue >= m_iTotalTime)
            {
                m_iElapsedTime = m_iTotalTime;
                this.Refresh();
                if (m_EventIncremented != null)
                    m_EventIncremented(this, null);
                if (m_EventCompleted != null)
                    m_EventCompleted(this, null);
            }
            else
            {
                m_iElapsedTime += a_iValue;
                this.Refresh();
                if (m_EventIncremented != null)
                    m_EventIncremented(this, null);
            }
        }
        private void ProgressCircle_Paint(object sender, PaintEventArgs e)
        {
            Rectangle t_oRectangle = new Rectangle(0, 0, this.Width, this.Height);
            Brush t_oBrushRemainingTime = new LinearGradientBrush(t_oRectangle, m_oColor1RemainingTime, m_oColor2RemainingTime, m_eLinearGradientMode);
            Brush t_oBrushElapsedTime = new LinearGradientBrush(t_oRectangle, m_oColor1ElapsedTime, m_oColor2ElapsedTime, m_eLinearGradientMode);
            e.Graphics.FillEllipse(t_oBrushRemainingTime, t_oRectangle);
            e.Graphics.FillPie(t_oBrushElapsedTime, t_oRectangle, -90f, (float)(360 * m_iElapsedTime / m_iTotalTime));
        }
    }
}