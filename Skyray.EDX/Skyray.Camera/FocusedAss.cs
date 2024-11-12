using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Skyray.Camera
{
    public class FocusedAss
    {
        /// <summary>
        /// 得到颜色对应的灰度值
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public double GetGrayscaleValue(Color color)
        {
            return 0.3 * color.R + 0.59 * color.G + 0.11 * color.B;
        }
        /// <summary>
        /// 返回指定区域内的区域聚焦评价值
        /// </summary>
        /// <param name="b_w">宽度起始像素</param>
        /// <param name="e_w">宽度终止像素</param>
        /// <param name="b_h">高度起始像素</param>
        /// <param name="e_h">高度终止像素</param>
        /// <param name="bitmap">指定图像</param>
        /// <returns></returns>
        public double CalculationZoneFocus(int b_w, int e_w, int b_h, int e_h, Bitmap bitmap)
        {
            // 对参数的检查
            if (bitmap == null || b_w < 0 || (e_w >= bitmap.Width && e_w < b_w) || b_h < 0 || (e_h >= bitmap.Height && e_h < b_h + 1))
            {
                return 0.0;
            }
            double FocusValue = 0.0;
            for (int i = b_w; i <= e_w; i++)
            {
                for (int j = b_h; j < e_h - 1; j++)
                {
                    FocusValue += Math.Pow(GetGrayscaleValue(bitmap.GetPixel(i, j + 1)) - GetGrayscaleValue(bitmap.GetPixel(i, j)), 2);
                }
            }
            return FocusValue;
        }

        /// <summary>
        /// 聚焦评估结果,参数默认
        /// </summary>
        /// <param name="bitmap">指定图像</param>
        /// <returns></returns>
        public double FocusedAssessment(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                return 0.0;
            }
            double FVa = CalculationZoneFocus(0, bitmap.Width / 3, 0, bitmap.Height / 3, bitmap);
            double FVb = CalculationZoneFocus(bitmap.Width / 3 * 2, bitmap.Width / 3 * 3, 0, bitmap.Height / 3, bitmap);
            double FVc = CalculationZoneFocus(0, bitmap.Width / 3, bitmap.Height / 3 * 2, bitmap.Height / 3 * 3, bitmap);
            double FVd = CalculationZoneFocus(bitmap.Width / 3 * 2, bitmap.Width / 3 * 3, bitmap.Height / 3 * 2, bitmap.Height / 3 * 3, bitmap);
            double FVo = CalculationZoneFocus(bitmap.Width / 3, bitmap.Width / 3 * 2, bitmap.Height / 3, bitmap.Height / 3 * 2, bitmap);

            return FVo * 0.4 + 0.15 * (FVa + FVb + FVc + FVd);
        }
        /// <summary>
        /// 聚焦评估结果，更一般
        /// </summary>
        /// <param name="k1">中心区域的权重</param>
        /// <param name="k2">周围区域的权重</param>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public double FocusedAssessment(double k1, double k2, Bitmap bitmap)
        {
            if (bitmap == null)
            {
                return 0.0;
            }
            if (k1 * k2 <= 0.0 && k1 < 0)
            {
                return 0.0;
            }
            double FVa = CalculationZoneFocus(0, bitmap.Width / 3, 0, bitmap.Height / 3, bitmap);
            double FVb = CalculationZoneFocus(bitmap.Width / 3 * 2, bitmap.Width / 3 * 3, 0, bitmap.Height / 3, bitmap);
            double FVc = CalculationZoneFocus(0, bitmap.Width / 3, bitmap.Height / 3 * 2, bitmap.Height / 3 * 3, bitmap);
            double FVd = CalculationZoneFocus(bitmap.Width / 3 * 2, bitmap.Width / 3 * 3, bitmap.Height / 3 * 2, bitmap.Height / 3 * 3, bitmap);
            double FVo = CalculationZoneFocus(bitmap.Width / 3, bitmap.Width / 3 * 2, bitmap.Height / 3, bitmap.Height / 3 * 2, bitmap);

            double k = k1 / k2;
            return FVo / (4 * k + 1) + k1 / k * (FVa + FVb + FVc + FVd);
        }
    }
}
