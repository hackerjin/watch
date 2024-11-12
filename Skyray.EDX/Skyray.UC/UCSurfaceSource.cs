using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.Language;
using Skyray.EDXRFLibrary;
using Lephone.Data.Common;

namespace Skyray.UC
{
    public partial class UCSurfaceSource : UCMultiple
    {
        private SurfaceSourceLight surfaceSource;
        private bool visible = false;
        public UCSurfaceSource()
        {
            InitializeComponent();
        }

        private void scrSurfaceSource_Scroll(object sender, ScrollEventArgs e)
        {
            if (visible)
            {
                this.lblValueOne.Text = scrSurfaceSourceOne.Value.ToString();
                this.lblValueTwo.Text = scrSurfaceSourceTwo.Value.ToString();
                this.lblValueThird.Text = scrSurfaceSourceThird.Value.ToString();
                this.lblValueFourth.Text = scrSurfaceSourceFourth.Value.ToString();
                surfaceSource.FirstLight = ushort.Parse(lblValueOne.Text);
                surfaceSource.SecondLight = ushort.Parse(lblValueTwo.Text);
                surfaceSource.ThirdLight = ushort.Parse(lblValueThird.Text);
                surfaceSource.FourthLight = ushort.Parse(lblValueFourth.Text);
                //DifferenceDevice.MediumAccess.SetSurfaceSource(surfaceSource.FirstLight, surfaceSource.SecondLight, surfaceSource.ThirdLight, surfaceSource.FourthLight);
            }
        }

        private void UCSurfaceSource_Load(object sender, EventArgs e)
        {
            visible = false;
            surfaceSource = SurfaceSourceLight.FindAll()[0];
            scrSurfaceSourceOne.Value = surfaceSource.FirstLight;
            scrSurfaceSourceTwo.Value = surfaceSource.SecondLight;
            scrSurfaceSourceThird.Value = surfaceSource.ThirdLight;
            scrSurfaceSourceFourth.Value = surfaceSource.FourthLight;
            scrSurfaceSourceAll.Value = surfaceSource.FirstLight;
            this.lblValueOne.Text = surfaceSource.FirstLight.ToString();
            this.lblValueTwo.Text = surfaceSource.SecondLight.ToString();
            this.lblValueThird.Text = surfaceSource.ThirdLight.ToString();
            this.lblValueFourth.Text = surfaceSource.FourthLight.ToString();
            this.lblValueAll.Text = surfaceSource.FirstLight.ToString();
            visible = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            surfaceSource.Save();
            EDXRFHelper.GotoMainPage(this);//返回主界面
        }

        private void scrSurfaceSourceAll_Scroll(object sender, ScrollEventArgs e)
        {
            if (visible)
            {
                visible = false;
                this.lblValueAll.Text = scrSurfaceSourceAll.Value.ToString();
                scrSurfaceSourceOne.Value = scrSurfaceSourceTwo.Value = scrSurfaceSourceThird.Value = scrSurfaceSourceFourth.Value = scrSurfaceSourceAll.Value;
                lblValueOne.Text = lblValueTwo.Text = lblValueThird.Text = lblValueFourth.Text = this.lblValueAll.Text;
                surfaceSource.FirstLight = surfaceSource.SecondLight = surfaceSource.ThirdLight = surfaceSource.FourthLight = ushort.Parse(lblValueAll.Text);
                //DifferenceDevice.MediumAccess.SetSurfaceSource(surfaceSource.FirstLight, surfaceSource.SecondLight, surfaceSource.ThirdLight, surfaceSource.FourthLight);
                visible = true;
            }
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            DifferenceDevice.MediumAccess.SetSurfaceSource(surfaceSource.FirstLight, surfaceSource.SecondLight, surfaceSource.ThirdLight, surfaceSource.FourthLight);
        }
    }
}
