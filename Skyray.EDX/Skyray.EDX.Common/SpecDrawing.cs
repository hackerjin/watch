using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Skyray.EDXRFLibrary;
using Skyray.EDXRFLibrary.Spectrum;

namespace Skyray.EDX.Common
{
    public class SpecDrawing
    {
        public static void DrawSpec(ref Bitmap bitmap, List<SpecEntity> specs,ElementList elementList)
        {
            int intSpace = 20;//间距
            Graphics g = Graphics.FromImage(bitmap);
            Brush backBrush = new SolidBrush(Color.WhiteSmoke);
            Font font = new Font("Tahoma", 8, FontStyle.Regular);
            Pen linePen = new Pen(Color.Black);
            Brush textBrush = new SolidBrush(Color.Black);
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            g.FillRectangle(backBrush, rect);
            Rectangle rectLine = new Rectangle(1, 1, bitmap.Width - 2, bitmap.Height - 2);
            g.DrawRectangle(linePen, rectLine);
            Size textSize = g.MeasureString("123456", font).ToSize(); //字符串大小
            int wMargin = Convert.ToInt32(2 * bitmap.Width / 100.0);//左右边距
            int hMargin = Convert.ToInt32(bitmap.Height / 100.0) + 2;//上下边距
            int textWidth = textSize.Width;
            int textHeight = textSize.Height;

            int tempWidth = Convert.ToInt32(bitmap.Width - 2 * wMargin);
            int tempHeight = Convert.ToInt32((bitmap.Height - specs.Count * intSpace - specs.Count * (textHeight + 2 * hMargin)) / specs.Count * 1.0);

            for (int n = 1; n <= specs.Count; n++)
            {
                int length = specs[n-1].SpecDatas.Length;
                int maxValue = SpecHelper.GetHighSpecValue(specs[n-1].SpecDatas);
                double pixX = (tempWidth - textWidth) / (length * 1.0);
                double pixY = tempHeight / (maxValue * 1.0);
                if (maxValue == 0)
                {
                    pixY = 0;
                }
                int orgY = hMargin + n * textHeight + (n - 1) * (tempHeight + intSpace);
                int orgX = wMargin + textWidth;
                g.DrawLine(linePen, orgX, orgY + tempHeight, orgX + tempWidth, orgY + tempHeight);//画X轴
                g.DrawLine(linePen, orgX, orgY, orgX, orgY + tempHeight);//画Y轴

                int titleW = 0;
                for (int i = 0; i <= 5; i++) //画Y轴刻度
                {
                    int k = (int)Math.Round(maxValue * (5 - i) / 5.0);
                    titleW = g.MeasureString(k.ToString(), font).ToSize().Width;
                    int y = hMargin + textHeight + (int)Math.Round(tempHeight * i / 5.0);
                    g.DrawLine(linePen, orgX, y + (n - 1) * (tempHeight + textHeight + intSpace), orgX - 4, y + (n - 1) * (tempHeight + textHeight + intSpace));
                    g.DrawString(k.ToString(), font, textBrush, orgX - 4 - titleW, y - textHeight / 2 + (n - 1) * (tempHeight + textHeight + intSpace));
                }

                Point[] point = new Point[length];
                for (int i = 0; i < length; i++)
                {
                    point[i].X = orgX + Convert.ToInt32(Math.Round(pixX * i));
                    point[i].Y = hMargin + textHeight + tempHeight + (n - 1) * (tempHeight + textHeight + intSpace) - Convert.ToInt32(Math.Round(pixY * specs[n-1].SpecDatas[i]));
                    if (point[i].Y < hMargin + textHeight + (n - 1) * (tempHeight + textHeight + intSpace))
                    {
                        point[i].Y = hMargin + textHeight + (n - 1) * (tempHeight + textHeight + intSpace);
                    }
                    if (i % 500 == 0)
                    {
                        g.DrawLine(linePen, point[i].X, hMargin + textHeight + tempHeight + (n - 1) * (tempHeight + textHeight + intSpace), point[i].X, hMargin + textHeight + tempHeight + 4 + (n - 1) * (tempHeight + textHeight + intSpace));
                        titleW = g.MeasureString(i.ToString(), font).ToSize().Width / 2;
                        g.DrawString(i.ToString(), font, textBrush, point[i].X - titleW, hMargin + textHeight + tempHeight + 4 + (n - 1) * (tempHeight + textHeight + intSpace));
                    }
                    else if (i % 50 == 0)
                    {
                        g.DrawLine(linePen, point[i].X, hMargin + textHeight + tempHeight + (n - 1) * (tempHeight + textHeight + intSpace), point[i].X + 2, hMargin + textHeight + tempHeight + (n - 1) * (tempHeight + textHeight + intSpace));
                    }
                }
                g.DrawPolygon(linePen, point);
                if (elementList == null|| elementList.Items == null || elementList.Items.Count == 0)
                    return;
                foreach (CurveElement element in elementList.Items)
                {
                    int l = element.PeakLow;
                    int r = element.PeakHigh;
                    int w = r - l;
                    Point[] boundPoints = new Point[w + 3];
                    for (int j = l; j <= r; j++)
                    {
                        boundPoints[j - l].X = point[j].X;
                        boundPoints[j - l].Y = point[j].Y;
                    }
                    boundPoints[w + 1].X = boundPoints[w].X;
                    boundPoints[w + 1].Y = hMargin + textHeight + tempHeight + (n - 1) * (tempHeight + textHeight + intSpace);
                    boundPoints[w + 2].X = boundPoints[0].X;
                    boundPoints[w + 2].Y = boundPoints[w + 1].Y;
                    g.FillPolygon(new SolidBrush(Color.Red), boundPoints);
                    titleW = g.MeasureString(element.Caption, font).ToSize().Width / 2;
                    int peakCh = (l + r) / 2;
                    g.DrawString(element.Caption, font, new SolidBrush(Color.Red), point[peakCh].X - titleW, point[peakCh].Y - textHeight);
                }
            }
        }
    }
}
