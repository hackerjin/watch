using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge;
using AForge.Controls;
using AForge.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;


namespace Skyray.UC
{
    public partial class FrmShowNewCamer : Form
    {
        public FrmShowNewCamer()
        {
            InitializeComponent();
        }

        private void FrmShowNewCamer_Leave(object sender, EventArgs e)
        {

        }
        public IVideoSource videoSource;
        private void FrmShowNewCamer_Load(object sender, EventArgs e)
        {
            this.skyrayCamera1.VideoSource = videoSource;
        }
      
    }
}
