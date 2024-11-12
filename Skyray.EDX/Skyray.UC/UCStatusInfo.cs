using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.Language;

namespace Skyray.UC
{
    public partial class UCStatusInfo :UCMultiple
    {
        public UCStatusInfo()
        {
            InitializeComponent();
        }

        public int Length
        {
            set
            {
                int original = this.progressBar1.Width;
                this.progressBar1.Width = value;
                this.labelSupus.Location = new Point(this.labelSupus.Location.X + value - original, this.labelSupus.Location.Y);
                this.labelFinishTime.Location = new Point(this.labelFinishTime.Location.X + value - original, this.labelFinishTime.Location.Y);
            }

        }

        public Label MeasureTime
        {
            get
            {
                return this.labelSetingTime;
            }
        }

        public Label SupusTime
        {
            get
            {
                return this.labelFinishTime;
            }
        }

        public ProgressBar ProgressBar
        {
            get
            {
                return this.progressBar1;
            }
        }
    }
}
