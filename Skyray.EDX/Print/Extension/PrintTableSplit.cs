using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.Print
{
    public static class PrintTableSplit
    {
        ///// <summary>
        ///// 行分页
        ///// </summary>
        ///// <param name="MaxY"></param>
        ///// <param name="StartY"></param>
        ///// <returns></returns>
        //public static List<SplitRowInfo> SplitTableRow(this TreeNodeInfo info, PrintPage page, int StartY)
        //{
        //    List<SplitRowInfo> SplitRowInfos = new List<SplitRowInfo>();

        //    int ColHeight = info.ColHeight;
        //    int RowHeight = info.RowHeight;
        //    int RowCount = info.Table.Rows.Count;

        //    int height0 = page.MaxY - StartY;//本页还剩余高度

        //    int n0 = (height0 - ColHeight) / RowHeight;//本页能显示行数

        //    if (n0 > 0)
        //    {
        //        int nTemp = n0 > RowCount ? RowCount : n0;
        //        SplitRowInfos.Add(new SplitRowInfo
        //        {
        //            SplitTableType = SplitTableType.First,
        //            Indexs = new IndexInfo
        //            {
        //                Start = 0,
        //                End = nTemp - 1
        //            }
        //        });
        //    }
        //    else
        //    {
        //        n0 = 0;//本页一行数据都显示不全
        //    }

        //    int nRow = (page.MaxY - page.HeaderHeight - ColHeight) / RowHeight;//每页能显示的行总数

        //    //int nRow = (MaxY - ContentRect.Top - ColHeight) / RowHeight;//每页能显示的行总数
        //    int n1 = (RowCount - n0) % nRow;//最后一页行数

        //    int nPage = (RowCount - n0) / nRow;//除去第一页，共分页数 

        //    for (int i = 0; i < nPage; i++)
        //    {
        //        int startrow = i * nRow + n0;
        //        int endrow = startrow + nRow - 1;
        //        SplitRowInfos.Add(new SplitRowInfo
        //        {
        //            SplitTableType = SplitTableType.Center,
        //            Indexs = new IndexInfo
        //            {
        //                Start = startrow,
        //                End = endrow
        //            }
        //        });
        //    }

        //    if (n1 > 0)//非整页
        //    {
        //        //nPage++;//分页增加
        //        int startrow = nPage * nRow + n0;
        //        int endrow = startrow + n1 - 1;
        //        SplitRowInfos.Add(new SplitRowInfo
        //        {
        //            SplitTableType = SplitTableType.End,
        //            Indexs = new IndexInfo
        //            {
        //                Start = startrow,
        //                End = endrow
        //            }
        //        });
        //    }

        //    return SplitRowInfos;
        //}
    }
}
