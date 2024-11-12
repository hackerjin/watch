using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Skyray.UC
{
    public partial class AutoDockManage : Component
    {
        private Form _form;

        public AutoDockManage()
        {
            InitializeComponent();
        }

        public AutoDockManage(IContainer container)
        {
            container.Add(this);
            System.Windows.Forms.Timer timer = new Timer();
            timer.Interval = 50;
            timer.Tick += new EventHandler(CheckPosTimer_Tick);
            timer.Start();
            InitializeComponent();
        }

        [Description("用于控制要自动Dock的窗体")]
        public Form DockForm
        {
            get
            {
                return _form;
            }
            set
            {
                _form = value;
                if (_form != null)
                {
                    _form.LocationChanged += new EventHandler(_form_LocationChanged);
                    _form.SizeChanged += new EventHandler(_form_SizeChanged);
                    _form.TopMost = true;
                }
            }
        }

        private bool IsOrg = false;
        private Rectangle lastBoard;
        private const int DOCKING = 0;
        private const int PRE_DOCKING = 1;
        private const int OFF = 2;

        private int status = 2;

        private void CheckPosTimer_Tick(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            if (_form == null || IsOrg == false)
            {
                return;
            }

            if (_form.Bounds.Contains(Cursor.Position))
            {
                switch (dockSide)
                {
                    case AnchorStyles.Top:
                        if (status == DOCKING)
                            _form.Location = new Point(_form.Location.X, 0);
                        break;
                    case AnchorStyles.Right:
                        if (status == DOCKING)
                            _form.Location = new Point(Screen.PrimaryScreen.Bounds.Width - _form.Width, 1);
                        break;
                    case AnchorStyles.Left:
                        if (status == DOCKING)
                            _form.Location = new Point(0, 1);
                        break;
                }
            }
            else
            {
                switch (dockSide)
                {
                    case AnchorStyles.Top:
                        _form.Location = new Point(_form.Location.X, (_form.Height - 4) * (-1));
                        break;
                    case AnchorStyles.Right:
                        _form.Size = new Size(_form.Width, Screen.PrimaryScreen.WorkingArea.Height);
                        _form.Location = new Point(Screen.PrimaryScreen.Bounds.Width - 4, 1);
                        break;
                    case AnchorStyles.Left:
                        _form.Size = new Size(_form.Width, Screen.PrimaryScreen.WorkingArea.Height);
                        _form.Location = new Point((-1) * (_form.Width - 4), 1);
                        break;
                    case AnchorStyles.None:
                        if (IsOrg == true && status == OFF)
                        {
                            if (_form.Bounds.Width != lastBoard.Width || _form.Bounds.Height != lastBoard.Height)
                            {
                                _form.Size = new Size(lastBoard.Width, lastBoard.Height);
                            }
                        }
                        break;
                }
            }
        }

        internal AnchorStyles dockSide = AnchorStyles.None;

        private void GetDockSide()
        {
            if (_form.Top <= 0)
            {
                dockSide = AnchorStyles.Top;
                if (_form.Bounds.Contains(Cursor.Position))
                    status = PRE_DOCKING;
                else
                    status = DOCKING;
            }
            else if (_form.Left <= 0)
            {
                dockSide = AnchorStyles.Left;
                if (_form.Bounds.Contains(Cursor.Position))
                    status = PRE_DOCKING;
                else
                    status = DOCKING;
            }
            else if (_form.Left >= Screen.PrimaryScreen.Bounds.Width - _form.Width)
            {
                dockSide = AnchorStyles.Right;
                if (_form.Bounds.Contains(Cursor.Position))
                    status = PRE_DOCKING;
                else
                    status = DOCKING;
            }
            else
            {
                dockSide = AnchorStyles.None;
                status = OFF;
            }
        }

        private void _form_LocationChanged(object sender, EventArgs e)
        {
            GetDockSide();
            if (IsOrg == false)
            {
                lastBoard = _form.Bounds;
                IsOrg = true;
            }
        }

        private void _form_SizeChanged(object sender, EventArgs e)
        {
            if (IsOrg == true && status == OFF)
            {
                lastBoard = _form.Bounds;
            }
        }
    }
}
