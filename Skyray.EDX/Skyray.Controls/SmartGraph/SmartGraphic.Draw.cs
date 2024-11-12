using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Skyray.Controls
{
    public partial class SmartGraphic
    {
        /// <summary>
        /// 画X轴标尺
        /// </summary>
        /// <param name="g"></param>
        private void DrawXRule(Graphics g)
        {
            const int maxLength = 5;
            const int minLength = 3;
            Font font = new Font("Tahoma", 8, FontStyle.Regular);
            Brush RuleBrush = new SolidBrush(ruleColor);
            Brush textBrush = new SolidBrush(Color.Black);
            g.FillRectangle(RuleBrush, 0, Height - ruleWidth, Width, ruleWidth);
            Pen linePen = new Pen(Color.Black);
            int y = Height - ruleWidth;
            int x = Width - ruleWidth;
            int textW = 0;
            int tempValue = 0;
            g.DrawLine(linePen, ruleWidth, y, Width, y);
            y++;
            int unitSize = FindUnitSize(100, (int)maxChannel / xZoom, 0);
            int i = -1;
            int residual = 0;
            do
            {
                i++;
                tempValue = i * unitSize;
                x = GetXPix(tempValue);
                Math.DivRem(i, 5, out residual);

                //修改：12/28/2009 刘敏
                //目的
                if (x < ruleWidth)
                {
                    continue;
                }

                //画线
                if (residual != 0)
                {
                    g.DrawLine(linePen, x, y, x, y + minLength);
                }
                //画出数值和线
                else
                {
                    g.DrawLine(linePen, x, y, x, y + maxLength);
                    SizeF strSize = g.MeasureString(GetChann(x).ToString(), font); //要绘制的字符串大小（宽、高）
                    textW = ((int)(strSize.Width) / 2);
                    g.DrawString(tempValue.ToString(), font, textBrush, x - textW, y + maxLength);
                }
            } while (x <= Width - textW);
        }

        /// <summary>
        /// 画Y轴标尺
        /// </summary>
        /// <param name="g"></param>
        private void DrawYRule(Graphics g)
        {
            const int maxLength = 5;
            const int minLength = 3;
            Font font = new Font("Tahoma", 8, FontStyle.Regular);
            Brush ruleBrush = new SolidBrush(ruleColor);
            Brush textBrush = new SolidBrush(Color.Black);
            g.FillRectangle(ruleBrush, 0, 0, ruleWidth, Height - ruleWidth);
            Pen linePen = new Pen(Color.Black);
            int textH = ((int)(g.MeasureString("12345", font).Height) / 2);
            int textW = 0;
            int y = Height - ruleWidth;
            int x = ruleWidth;
            g.DrawLine(linePen, x, 0, x, y);
            x = x - 1;
            int unitSize = FindUnitSize(70, (int)(maxValue / SpecDatas[0].YZoom), 0);
            int residual = 0;
            int tempValue = 0;
            int i = -1;
            do
            {
                i++;
                tempValue = i * unitSize;
                y = GetYPix(tempValue);
                Math.DivRem(i, 5, out residual);
                //画线
                if (residual != 0)
                {
                    g.DrawLine(linePen, x - minLength, y, x, y);
                }
                //画线和数值
                else
                {
                    g.DrawLine(linePen, x - maxLength, y, x, y);
                    SizeF strSize = g.MeasureString(tempValue.ToString(), font);
                    textW = (int)strSize.Width;
                    g.DrawString(tempValue.ToString(), font, textBrush, x - textW - maxLength, y);
                }
            } while (y > 0);
        }

        // 修改：12-12-2009 田春华
        // 目的：   调整为对比谱与实谱分开画
        /// <summary>
        /// 画谱
        /// </summary>
        /// <param name="g">画板</param>
        /// <param name="bnref">true 画实谱
        /// false 画对比谱</param>
        ///private void DrawSpec(Graphics g)
        private void DrawSpec(Graphics g, bool bnref)
        {
            // 修改：12-12-2009 田春华
            // 目的：   谱图的显示问题
            //Brush backBrush = new SolidBrush(BackColor);
            //Rectangle rect = new Rectangle(ruleWidth + 1, 0, Width, Height - ruleWidth - 1);
            //g.FillRectangle(backBrush, rect);  //画显示谱的矩形区域 
            int value = 0;
            for (int i = 0; i < MaxSpecCount; i++)
            {
                if (SpecDatas[i].Enabled && SpecDatas[i].Data.Length > 0)
                {
                    for (int ch = 0; ch < maxChannel; ch++)
                    {
                        //SpecDatas[i].BorderPoint[ch].X = GetXPix(ch);
                        if (SpecDatas[i].BorderPoint.Length > ch)
                            SpecDatas[i].BorderPoint[ch].X = (GetXPix(ch) > ruleWidth ? GetXPix(ch) : ruleWidth);
                        if (i != 0)
                        {
                            if (SpecDatas[i].Data.Length > ch)
                                value = (int)Math.Round(SpecDatas[i].Data[ch] * SpecDatas[i].YZoom);
                        }
                        else
                        {
                            if (SpecDatas[i].Data.Length > ch)
                                value = SpecDatas[i].Data[ch];
                        }
                        if (SpecDatas[i].BorderPoint.Length > ch)
                        {
                            SpecDatas[i].BorderPoint[ch].Y = GetYPix(value);
                        }
                    }
                    if (SpecDatas[i].BorderPoint.Length > maxChannel)
                    {
                        SpecDatas[i].BorderPoint[maxChannel].X = SpecDatas[i].BorderPoint[maxChannel - 1].X;
                        SpecDatas[i].BorderPoint[maxChannel].Y = Height - ruleWidth;
                    }
                    if (SpecDatas[i].BorderPoint.Length > maxChannel + 1)
                    {
                        SpecDatas[i].BorderPoint[maxChannel + 1].X = ruleWidth;
                        SpecDatas[i].BorderPoint[maxChannel + 1].Y = Height - ruleWidth;
                    }
                    // 修改：12-12-2009 田春华
                    // 目的：   对比谱与实谱分开画
                    //if (i == 0)//画实谱
                    if (i == 0 && bnref)
                    {
                        g.FillPolygon(new SolidBrush(SpecDatas[i].Color), SpecDatas[i].BorderPoint);
                    }                   
                    else if (!bnref)
                    {
                        g.DrawPolygon(new Pen(SpecDatas[i].Color), SpecDatas[i].BorderPoint);
                    }
                }
            }
        }
    }
}
