using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace Skyray.Print
{
    /// <summary>
    /// PrintCtrl功能扩展类
    /// </summary>
    public static class PrintCtrlExtension
    {
        /// <summary>
        /// PrintCtrl类到Bitmap类
        /// </summary>
        /// <param name="printCtrl">传入PrintCtrl信息</param>
        /// <param name="bitmap">传入Bitmap信息</param>
        /// <param name="top">出入顶部信息</param>
        public static void CtrlToBitmap(this PrintCtrl printCtrl, Bitmap bitmap, int top)
        {
            bool select = printCtrl.Selected;
            if (select) printCtrl.Selected = false;

            printCtrl.DrawToBitmap(bitmap, new Rectangle(printCtrl.Left, top,
                printCtrl.Width, printCtrl.Height));

            if (select) printCtrl.Selected = true;
        }
    }
}
