using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Skyray.Controls
{
   
    public delegate void SkinFormBorderRenderEventHandler(
        object sender,
        SkinFormBorderRenderEventArgs e);

    public class SkinFormBorderRenderEventArgs : PaintEventArgs
    {
        private FrmSkyray _skinForm;
        private bool _active;

        public SkinFormBorderRenderEventArgs(
            FrmSkyray skinForm,
            Graphics g,
            Rectangle clipRect,
            bool active)
            : base(g, clipRect)
        {
            _skinForm = skinForm;
            _active = active;
        }

        public FrmSkyray SkinForm
        {
            get { return _skinForm; }
        }

        public bool Active
        {
            get { return _active; }
        }
    }
}
