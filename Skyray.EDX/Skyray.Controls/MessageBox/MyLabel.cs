using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Skyray.Controls
{
    public partial class MyLabel : System.Windows.Forms.Label
    {
        int lineDistance = 5;
        public int LineDistance
        {
            get { return lineDistance; }
            set { lineDistance = value; }
        }
        public MyLabel()
        {
            InitializeComponent();
            

        }
        private int NeedStrWith;
        public int LabelMaxWith
        {
            get { return NeedStrWith; }
        }

        public MyLabel(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }
        public void RefreshSize()
        {
           Graphics g=this.CreateGraphics();
            SizeF textSize = g.MeasureString("行高", this.Font);
            int lineCount = 0;
            string drawstringTemp = this.Text;
            int MaxX = 0;
            for (int i = 0; i < drawstringTemp.Length - 1; i++)
            {
                if ((drawstringTemp[i] == '\r' && drawstringTemp[i + 1] == '\n')
                    || (drawstringTemp[i] == '\n' && drawstringTemp[i + 1] == '\r'))
                {
                    string subN = drawstringTemp.Substring(0, i + 2);
                    int MaxXtemp = (int)Math.Round(g.MeasureString(subN, this.Font).Width + 0.5);
                    MaxX = MaxX < MaxXtemp ? MaxXtemp : MaxX;
                    drawstringTemp = drawstringTemp.Substring(i + 2);
                    i = 0;
                    lineCount++;
                }
                else if (i >= drawstringTemp.Length - 2)
                {
                    string subN = drawstringTemp.Substring(0, drawstringTemp.Length);
                    int MaxXtemp = (int)Math.Round(g.MeasureString(subN, this.Font).Width + 0.5);
                    MaxX = MaxX < MaxXtemp ? MaxXtemp : MaxX;
                    lineCount++;
                    break;
                }
            }
            NeedStrWith = MaxX;
            this.Width = MaxX;
            this.Height = Convert.ToInt16((textSize.Height + lineDistance) * lineCount);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            Graphics g = e.Graphics;
            string drawString = this.Text;
            Font drawFont = this.Font;
            SolidBrush drawBrush = new SolidBrush(this.ForeColor);
            SizeF textSize = g.MeasureString("行高", this.Font);
            this.AutoSize = false;
            StringFormat drawFromat = new StringFormat();
            int step = 1;
            string drawstringTemp = drawString;
            int MaxX = 0;
            int lineCount = 0;
            for (int i = 0; i < drawstringTemp.Length-1; i++)
            {
                if ((drawstringTemp[i] == '\r' && drawstringTemp[i+1] == '\n')
                    || (drawstringTemp[i] == '\n' && drawstringTemp[i + 1] == '\r'))
                {
                    string subN = drawstringTemp.Substring(0, i + 2);
                    int MaxXtemp = (int)Math.Round(g.MeasureString(subN, this.Font).Width+0.5);
                    MaxX = MaxX < MaxXtemp ? MaxXtemp : MaxX;
                    drawstringTemp = drawstringTemp.Substring(i+2);
                    i = 0;
                    lineCount++;
                }
                else if (i >= drawstringTemp.Length - 2)
                {
                    string subN = drawString.Substring(0, drawstringTemp.Length);
                    int MaxXtemp =(int)Math.Round( g.MeasureString(subN, this.Font).Width+0.5);
                    MaxX = MaxX < MaxXtemp ? MaxXtemp : MaxX;
                    lineCount++;
                    break;
                }
            }
            //int lineCount = Convert.ToInt16(textSize.Width / this.Width) + 1;//计算行数
            //this.Height = Convert.ToInt16((textSize.Height + lineDistance) * lineCount);
            int MaxY = 0;
            //lineCount = drawString.Length;
            float x = 0f;

            for (int i = 0; i < lineCount; i++)
            {
                int charCount;
                for (charCount = 0;  charCount < drawString.Length; charCount++)
                {
                    string subN = drawString.Substring(0, charCount);
                    string subN1 = drawString.Substring(0, charCount + 1);
                    //if ((g.MeasureString(subN, this.Font).Width <= this.Width) && g.MeasureString(subN1, this.Font).Width > this.Width)
                    //{
                    //    step = charCount;
                    //    break;
                    //}
                    if (subN1.Contains('\r') && subN1.Contains('\n'))
                    {
                        step = charCount+1;
                        break;
                    }
                }
                string substr;
                if (charCount == drawString.Length)//追后一行
                {
                    substr = drawString;
                    e.Graphics.DrawString(substr, drawFont, drawBrush, x, Convert.ToInt16(textSize.Height * i) + i * lineDistance, drawFromat);
                   // MaxY = Convert.ToInt16(textSize.Height * (i+1)) + (i+1) * lineDistance;
                    break;
                }
                else
                {
                    substr = drawString.Substring(0, step);
                    drawString = drawString.Substring(step);
                    e.Graphics.DrawString(substr, drawFont, drawBrush, x, Convert.ToInt16(textSize.Height * i) + i * lineDistance, drawFromat);
                    //MaxY = Convert.ToInt16(textSize.Height * (i + 1)) + (i + 1) * lineDistance;
                }
            }
            //this.Height = MaxY;
        }
    }
}
