using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Skyray.Controls
{
    public static class Utils
    {
        public static void MakeSizable(this Control src)
        {
            var md = MouseDirection.None;
            var mouseDown = false;
            var startPoint = Point.Empty;
            var oriSize = src.Size;
            src.MouseDown += (s, e) =>
            {
                if (e.Button != MouseButtons.Left || md == MouseDirection.None)
                    return;
                mouseDown = true;
                startPoint = e.Location;
                oriSize = src.Size;
            };
            src.MouseMove += (s, e) =>
            {
                if (!mouseDown)
                {
                    var p = e.Location;
                    if (p.X >= src.Width - 10 && p.Y >= src.Height - 10)
                    {
                        src.Cursor = Cursors.SizeNWSE;
                        md = MouseDirection.Declining;
                    }
                    else if (p.X >= src.Width - 5)
                    {
                        src.Cursor = Cursors.SizeWE;
                        md = MouseDirection.Herizontal;
                    }
                    else if (p.Y >= src.Height - 5)
                    {
                        src.Cursor = Cursors.SizeNS;
                        md = MouseDirection.Vertical;
                    }
                    else
                    {
                        src.Cursor = Cursors.Default;
                        md = MouseDirection.None;
                    }
                    return;
                }
                switch (md)
                {
                    case MouseDirection.Declining:
                        src.Size = oriSize + new Size(e.Location.X - startPoint.X, e.Location.Y - startPoint.Y);
                        break;
                    case MouseDirection.Herizontal:
                        src.Size = oriSize + new Size(e.Location.X - startPoint.X, 0);
                        break;
                    case MouseDirection.Vertical:
                        src.Size = oriSize + new Size(0, e.Location.Y - startPoint.Y);
                        break;
                    default:
                        break;
                }

            };

            src.MouseUp += (s, e) =>
            {
                if (e.Button != MouseButtons.Left || md == MouseDirection.None)
                    return;
                mouseDown = false;
                startPoint = Point.Empty;
                md = MouseDirection.None;
            };

        }

        public static void MakeOthersSizable(this Control src, Control dst)
        {
            var md = MouseDirection.None;
            var mouseDown = false;
            var startPoint = Point.Empty;
            var oriSize = dst.Size;
            src.MouseDown += (s, e) =>
            {
                if (e.Button != MouseButtons.Left || md == MouseDirection.None)
                    return;
                mouseDown = true;
                startPoint = e.Location;
                oriSize = dst.Size;
            };
            src.MouseMove += (s, e) =>
            {
                if (!mouseDown)
                {
                    var p = e.Location;
                    if (p.X >= src.Width - 10 && p.Y >= src.Height - 10)
                    {
                        src.Cursor = Cursors.SizeNWSE;
                        md = MouseDirection.Declining;
                    }
                    else if (p.X >= src.Width - 5)
                    {
                        src.Cursor = Cursors.SizeWE;
                        md = MouseDirection.Herizontal;
                    }
                    else if (p.Y >= src.Height - 5)
                    {
                        src.Cursor = Cursors.SizeNS;
                        md = MouseDirection.Vertical;
                    }
                    else
                    {
                        src.Cursor = Cursors.Default;
                        md = MouseDirection.None;
                    }
                    return;
                }
                switch (md)
                {
                    case MouseDirection.Declining:
                        dst.Size = oriSize + new Size(e.Location.X - startPoint.X, e.Location.Y - startPoint.Y);
                        break;
                    case MouseDirection.Herizontal:
                        dst.Size = oriSize + new Size(e.Location.X - startPoint.X, 0);
                        break;
                    case MouseDirection.Vertical:
                        dst.Size = oriSize + new Size(0, e.Location.Y - startPoint.Y);
                        break;
                    default:
                        break;
                }

            };

            src.MouseUp += (s, e) =>
            {
                if (e.Button != MouseButtons.Left || md == MouseDirection.None)
                    return;
                mouseDown = false;
                startPoint = Point.Empty;
                md = MouseDirection.None;
            };

        }

        public static void MakeMovable(this Control src)
        {
            var mouseDown = false;
            var startPoint = Point.Empty;
            src.MouseDown += (s, e) =>
            {
                if (e.Button != MouseButtons.Left || src.Cursor != Cursors.Default || (src is Form && (src as Form).WindowState == FormWindowState.Maximized))
                    return;
                mouseDown = true;
                startPoint = e.Location;

            };

            src.MouseMove += (s, e) =>
            {
                if (e.Button != MouseButtons.Left || !mouseDown)
                    return;
                src.Location = new Point(src.Location.X + (e.Location.X - startPoint.X), src.Location.Y + (e.Location.Y - startPoint.Y));

            };

            src.MouseUp += (s, e) =>
            {
                if (e.Button != MouseButtons.Left || !mouseDown)
                    return;
                mouseDown = false;
                startPoint = Point.Empty;
            };
        }

        public static void MakeOthersMovable(this Control src, Control dst)
        {
            var mouseDown = false;
            var startPoint = Point.Empty;
            src.MouseDown += (s, e) =>
            {
                if (e.Button != MouseButtons.Left || src.Cursor != Cursors.Default || (dst is Form && (dst as Form).WindowState == FormWindowState.Maximized))
                    return;
                mouseDown = true;
                startPoint = e.Location;

            };

            src.MouseMove += (s, e) =>
            {
                if (e.Button != MouseButtons.Left || !mouseDown)
                    return;
                dst.Location = new Point(dst.Location.X + (e.Location.X - startPoint.X), dst.Location.Y + (e.Location.Y - startPoint.Y));

            };

            src.MouseUp += (s, e) =>
            {
                if (e.Button != MouseButtons.Left || !mouseDown)
                    return;
                mouseDown = false;
                startPoint = Point.Empty;
            };
        }

        public static void SafeCall(this Control src, Action act)
        {
            if (src == null || act == null)
                throw new ArgumentNullException("src or act");
            if (src.InvokeRequired)
            {
                src.Invoke(act);
                return;
            }
            act();

        }

        public static bool SafeCall(this Control src, Func<bool> func)
        {
            if (src == null || func == null)
                throw new ArgumentNullException("src or act");
            if (src.InvokeRequired)
            {
                return (bool)src.Invoke(func);
            }
            return func();
        }
    }

    public enum MouseDirection
    {
        None, //不做标志，即不拖动窗体改变大小 

        Herizontal,//水平方向拖动，只改变窗体的宽度  

        Vertical,//垂直方向拖动，只改变窗体的高度  

        Declining//倾斜方向，同时改变窗体的宽度和高度
    }
}

