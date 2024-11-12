using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Skyray.UC
{
    public partial class UCPipeTime : UserControl
    {
        public UCPipeTime()
        {
            InitializeComponent();
        }

        public PictureBox PictureBox
        {
            get
            {
                return this.pictureBox1;
            }
        }
    }
}
