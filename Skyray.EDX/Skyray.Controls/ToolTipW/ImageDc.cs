using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Security;
using System.Security.Permissions;
using Skyray.API;
namespace Skyray.Controls
{
    /* 作者：Starts_2000
     * 日期：2010-01-09
     * 网站：http://www.csharpwin.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.csharpwin.com/csol.html）。
     */

    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class ImageDc : IDisposable
    {
        private int _height = 0;
        private int _width = 0;
        private IntPtr _pHdc = IntPtr.Zero;
        private IntPtr _pBmp = IntPtr.Zero;
        private IntPtr _pBmpOld = IntPtr.Zero;

        public ImageDc(int width, int height, IntPtr hBmp)
        {
            CreateImageDc(width, height, hBmp);
        }

        public ImageDc(int width, int height)
        {
            CreateImageDc(width, height, IntPtr.Zero);
        }

        public IntPtr Hdc
        {
            get { return _pHdc; }
        }

        public IntPtr HBmp
        {
            get { return _pBmp; }
        }

        private void CreateImageDc(int width, int height, IntPtr hBmp)
        {
            IntPtr pHdc = IntPtr.Zero;

            pHdc = WinMethod.CreateDCA("DISPLAY", "", "", 0);
            _pHdc = WinMethod.CreateCompatibleDC(pHdc);
            if (hBmp != IntPtr.Zero)
            {
                _pBmp = hBmp;
            }
            else
            {
                _pBmp = WinMethod.CreateCompatibleBitmap(pHdc, width, height);
            }
            _pBmpOld = WinMethod.SelectObject(_pHdc, _pBmp);
            if (_pBmpOld == IntPtr.Zero)
            {
                ImageDestroy();
            }
            else
            {
                _width = width;
                _height = height;
            }
            WinMethod.DeleteDC(pHdc);
            pHdc = IntPtr.Zero;
        }

        private void ImageDestroy()
        {
            if (_pBmpOld != IntPtr.Zero)
            {
                WinMethod.SelectObject(_pHdc, _pBmpOld);
                _pBmpOld = IntPtr.Zero;
            }
            if (_pBmp != IntPtr.Zero)
            {
                WinMethod.DeleteObject(_pBmp);
                _pBmp = IntPtr.Zero;
            }
            if (_pHdc != IntPtr.Zero)
            {
                WinMethod.DeleteDC(_pHdc);
                _pHdc = IntPtr.Zero;
            }
        }

        public void Dispose()
        {
            ImageDestroy();
        }
    }
}
