using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common;
using System.IO;
using Skyray.EDX.Common.Component;

namespace Skyray.UC
{
    public partial class UCMatchDemo : Skyray.Language.UCMultiple
    {
        public UCMatchDemo()
        {
            InitializeComponent();
            SetRichText();
        }

        private void SetRichText()
        {
            string matter = string.Empty;
            int id = 0;
            if (DifferenceDevice.irohs == null)
                return;
            DifferenceDevice.irohs.MatchWorkCurve(ref matter, ref id, 1);
            string showText = string.Empty;
            showText += Info.PeakDivBase + " " + Rohs.matter.Ratio + "\n";
            showText += Info.BaseLow+ " " + Rohs.matter.BasePeak.Left + "\n";
            showText += Info.BaseHigh+" " + Rohs.matter.BasePeak.Right + "\n";
            showText += Info.MainPeakLeft+" " + Rohs.matter.MainPeak.Left + "\n";
            showText += Info.MainPeakRight+" " + Rohs.matter.MainPeak.Right + "\n";
            showText += Info.BaseArea + Rohs.matter.BasePeak.Area + "\n";
            showText +=Info.MainArea + Rohs.matter.MainPeak.Area + "\n";

            double tempRatio = 0;
            if (Rohs.matter.BasePeak.Area != 0)
            {
                tempRatio = Rohs.matter.MainPeak.Area / Rohs.matter.BasePeak.Area;
            }
            else
            {
                tempRatio = Rohs.matter.Ratio + 1;
            }
            showText += Info.PeakDivBase + " " + tempRatio + "\n";
            if (tempRatio > Rohs.matter.Ratio)
                showText += Info.IronOrNoIron + Info.Iron+"\n";
            else
                showText += Info.IronOrNoIron + Info.NoIron+"\n";
            showText += Info.ContextNotify+"\n";
            if (Rohs.matter.peakRatio != null)
            {
                showText += Rohs.matter.peakRatio + "\n";
                string[] str = Rohs.matter.peakRatio.Split('|');
                if (str != null && str.Length > 0)
                {
                    for (int i = 0; i < str.Length; i++)
                        showText += str[i] + "\n";
                }
            }
            this.richTextMathDemoInfo.Text = showText;
        }
    }
}
