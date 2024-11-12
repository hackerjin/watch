using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.Print
{
    public class CtrlGrouper
    {
        /// <summary>
        /// 组相对位置
        /// </summary>
        /// <param name="groups"></param>
        /// <param name="headerHeight"></param>
        /// <param name="maxY"></param>
        public static void CalcRelativePosition(List<GroupInfo> groups, int maxY)
        {
            for (int i = 0; i < groups.Count; i++)
            {
                groups[i].StartY = (i == 0 ? groups[i].Top
                    : groups[i].Top - groups[i - 1].Bottom);//计算相对位置

                if (groups[i].StartY > maxY) //如果间距 超过
                    groups[i].StartY = groups[i].StartY % maxY;  //起始点
            }
        }
        /// <summary>
        /// 获取组索引
        /// </summary>
        /// <param name="ints"></param>
        /// <param name="lastIndex"></param>
        /// <returns></returns>
        private static List<IndexInfo> GetGroupIndex(List<int> ints, int lastIndex)
        {
            List<IndexInfo> groupIndexs = new List<IndexInfo>();
            int startIndex = 0;
            foreach (var i in ints)
            {
                groupIndexs.Add(new IndexInfo { Start = startIndex, End = i });
                startIndex = i;
            }
            groupIndexs.Add(new IndexInfo { Start = startIndex, End = lastIndex });
            return groupIndexs;
        }

        /// <summary>
        /// 检查控件分组
        /// </summary>
        /// <param name="lstAllCtrl"></param>
        /// <param name="groupIndexs"></param>
        /// <returns></returns>
        public static bool CheckGroup(CellControl[] lstAllCtrl, List<IndexInfo> groupIndexs)
        {
            return true;
        }

        /// <summary>
        /// 控件分组
        /// </summary>
        /// <param name="lstAllCtrl"></param>
        /// <param name="ints"></param>
        /// <returns></returns>
        public static List<GroupInfo> GroupCtrls(CellControl[] lstAllCtrl, List<int> ints)
        {
            var groupIndexs = GetGroupIndex(ints, lstAllCtrl.Length);//分组信息
            List<GroupInfo> groups = new List<GroupInfo>();
            foreach (var index in groupIndexs)
            {
                var groupInfo = new GroupInfo();
                groupInfo.CtrlIndex = index;

                var top = lstAllCtrl[index.Start].Point.Y;
                int bottom = 0;
                int y = 0;
                for (int i = index.Start; i < index.End; i++)
                {
                    var cellCtrl = lstAllCtrl[i];
                    y = cellCtrl.Point.Y;
                    bottom = Math.Max(bottom, y + cellCtrl.Size.Height);
                    lstAllCtrl[i].RelativeY = y - top;
                    //修改：何晓明 2011-03-10
                    //原因：一个组中表格数量多于一个时出错
                    if(cellCtrl.PrintCtrl.Type== CtrlType.Grid)
                    {
                        groupInfo.Tables.Add(cellCtrl.PrintCtrl as PrintTable);
                    }
                    //
                }

                groupInfo.Top = top;
                groupInfo.Bottom = bottom;
                groupInfo.Height = bottom - top;
                groups.Add(groupInfo);
            }
            return groups;
        }
        /// <summary>
        /// 获取组分隔索引 
        /// </summary>
        /// <param name="lstAllCtrl"></param>
        /// <returns></returns>
        public static List<int> GetGroupSplitIndex(CellControl[] lstAllCtrl)
        {
            List<int> ints = new List<int>();
            int maxBottom = 0;
            int temp = 0;
            int tempBottom = 0;
            CellControl cellCtrl;

            while (temp < lstAllCtrl.Length)
            {
                int sIndex = temp;
                for (int i = temp; i < lstAllCtrl.Length; i++)
                {
                    temp = i;

                    //相邻两个控件的最大底坐标

                    cellCtrl = lstAllCtrl[i == 0 ? 0 : i - 1];

                    tempBottom = cellCtrl.Point.Y + cellCtrl.Size.Height;

                    maxBottom = Math.Max(tempBottom, maxBottom);

                    if (lstAllCtrl[i].Point.Y > maxBottom)//不相交
                    {
                        ints.Add(i);

                        temp++;
                        break;
                    }
                    else
                    {
                        if (i + 1 == lstAllCtrl.Length)//最后一个控件
                        {
                            temp++;//退出总循环
                        }
                    }
                }
            }
            return ints;
        }
    }
}
