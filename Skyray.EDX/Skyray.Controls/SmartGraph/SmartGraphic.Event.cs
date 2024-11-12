using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Skyray.Controls
{
    public partial class SmartGraphic
    {
        private int mouseX;    ///<鼠标的横坐标
        private int mouseY;    ///<鼠标的纵坐标

        private void SpecGraphic_MouseEnter(object sender, EventArgs e)
        {
            mouseX = -1;
            mouseY = -1; 
        }

        private void SpecGraphic_MouseLeave(object sender, EventArgs e)
        {
            if ((mouseX > ruleWidth) && (mouseY < (Height - ruleWidth)))
            {
                ControlPaint.DrawReversibleLine(PointToScreen(new Point(mouseX, 0)), PointToScreen(new Point(mouseX, Height - ruleWidth - 1)), Color.Black);
            }
            mouseX = -1;
            mouseY = -1;
            Invalidate();
        }

        private void SpecGraphic_MouseMove(object sender, MouseEventArgs e)
        {
            #region Draw Move Line
            if ((mouseX > 0) && (mouseY > 0))
            {
                ControlPaint.DrawReversibleLine(PointToScreen(new Point(mouseX, 0)), PointToScreen(new Point(mouseX, Height - ruleWidth - 1)), Color.Black); //根据具体颜色的背景画可逆的线
                mouseX = -1;
                mouseY = -1;
            }
            if ((e.X > ruleWidth) && (e.Y < (Height - ruleWidth)))
            {
                currChann = GetChann(e.X);
                if (SpecDatas[0].Data != null)
                    if ((currChann >= 0) && (currChann < SpecDatas[0].Data.Length))
                    {
                        currValue = SpecDatas[0].Data[currChann];
                    }
                ControlPaint.DrawReversibleLine(PointToScreen(new Point(e.X, 0)), PointToScreen(new Point(e.X, Height - ruleWidth - 1)), Color.Black); //画鼠标当前所在点的线
                mouseX = e.X;
                mouseY = e.Y;
            }
            #endregion


        }
    }
}
