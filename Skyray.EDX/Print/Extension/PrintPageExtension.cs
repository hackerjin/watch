using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Skyray.Print
{
    /// <summary>
    /// PrintPage功能扩展类
    /// </summary>
    public static class PrintPageExtension
    {
        /// <summary>
        /// 获取打印模板页眉包含控件
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static CellControl[] GetHeaderCellControls(this PrintPage page)
        {        
            var lstAllCtrl = (from PrintCtrl ctl
                             in page.Header.Controls
                              select new CellControl()
                              {
                                  PrintCtrl = ctl,
                                  Point = new Point
                                  {
                                      X = ctl.Location.X + ctl.BaseRect.Left,
                                      Y = ctl.Location.Y + ctl.BaseRect.Top
                                  },
                                  Size = ctl.BaseRect.Size
                              })
                              .ToArray();
            return lstAllCtrl;
        }

        /// <summary>
        /// 获取打印模板页脚包含控件
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static CellControl[] GetFootCellControls(this PrintPage page)
        {
            int headerHeight = page.ShowHeader ? page.Header.Height : 0;
            int codyHegiht = page.Body.Size.Height;
            var lstAllCtrl = (from PrintCtrl ctl
                             in page.Footer.Controls
                              select new CellControl()
                              {
                                  PrintCtrl = ctl,
                                  Point = new Point
                                  {
                                      X = ctl.Location.X + ctl.BaseRect.Left,
                                      Y = headerHeight + ctl.Location.Y + ctl.BaseRect.Top + codyHegiht
                                  },
                                  Size = ctl.BaseRect.Size
                              })
                              .ToArray();
            return lstAllCtrl;
        }
        /// <summary>
        /// 获得打印模板体包含控件
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static CellControl[] GetBodyCellControls(this PrintPage page)
        {
            int headerHeight = page.ShowHeader ? page.Header.Height : 0;
            var lstAllCtrl = (from PrintCtrl ctl in page.Body.Controls orderby ctl.Location.Y ascending select new CellControl()
                               {
                                   PrintCtrl = ctl,
                                   Point = new Point
                                   {
                                       X = ctl.Location.X + ctl.BaseRect.Left,
                                       Y = headerHeight + ctl.Location.Y + ctl.BaseRect.Top
                                   },
                                   Size = ctl.BaseRect.Size
                               })
                              .ToArray();
            return lstAllCtrl;
        }


        /// <summary>
        /// 获得Bitmap对象
        /// </summary>
        /// <returns></returns>
        public static Bitmap GetBitmap(this PrintPage Page)
        {

            int width = Page.SizeOfPaper.Width;//宽度
            int height = Page.SizeOfPaper.Height;//高度
            //对象创建
            Bitmap bitmap = Page.Dir == PrintDirection.Vertical ?
                new Bitmap(height, width) : new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);//白色背景
            }
            //if (Page.ShowHeader)//表头
            //{
            //    foreach (PrintCtrl ctrl in Page.Header.Controls)
            //        ctrl.CtrlToBitmap(bitmap, ctrl.Top);//表头
            //}
            //if (Page.ShowFooter)//页尾
            //{
            //    //页尾
            //    foreach (PrintCtrl ctrl in Page.Footer.Controls)
            //        ctrl.CtrlToBitmap(bitmap, ctrl.Top + height - Page.Footer.Height);
            //}
            return bitmap;
        }

        /// <summary>
        /// 获取分组信息
        /// </summary>
        /// <param name="Page">传入PrintPage类本身</param>
        /// <param name="lstAllCtrl">传入CellControl类集合</param>
        /// <returns></returns>
        public static List<GroupInfo> GetGroupInfo(this PrintPage Page, CellControl[] lstAllCtrl)
        {

            List<int> ints = CtrlGrouper.GetGroupSplitIndex(lstAllCtrl);//分割索引

            List<GroupInfo> groups = CtrlGrouper.GroupCtrls(lstAllCtrl, ints);//获取分组信息

            CtrlGrouper.CalcRelativePosition(groups, Page.MaxY);//计算组相对位置

            return groups;
        }

    }
}
