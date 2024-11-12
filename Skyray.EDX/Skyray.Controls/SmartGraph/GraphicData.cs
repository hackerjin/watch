using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Skyray.Controls
{
    /// <summary>
    /// 需要画谱图的数据结构
    /// </summary>
    public struct GraphicData
    {
        public double YZoom;///<Y轴放大倍数
        public Color Color;  ///<颜色
        public bool Enabled; ///<状态
        public Point[] BorderPoint;///<边界点
        public int[] Data;  ///<谱数据
        ///
        public GraphicData(int[] data, Color color)
        {
            YZoom = 1;
            Color = color;
            Enabled = true;
            BorderPoint = new Point[data.Length + 2];
            Data = (int[])data.Clone();
        }
    }
}
