using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.EDX.Common;
using Skyray.Language;

namespace Skyray.UC
{
    /// <summary>
    /// 选择层类
    /// </summary>
    public partial class FrmSelectLayer : Skyray.Language.MultipleForm
    {
        /// <summary>
        /// 单/多层标志
        /// </summary>
        public int LayerNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public FrmSelectLayer()
        {
            InitializeComponent();
            this.radSingle.Text = Info.SingleLayer;
            this.radMultiLayer.Text = Info.MultiLayer;
        }
        /// <summary>
        /// 选择单层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radSingle_CheckedChanged(object sender, EventArgs e)
        {
            if (radSingle.Checked)
            { LayerNO = 1; }
        }
        /// <summary>
        /// 选择多层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radMultiLayer_CheckedChanged(object sender, EventArgs e)
        {
            if (radMultiLayer.Checked)
            { LayerNO = 2; }
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (LayerNO < 1)
            {
                Skyray.Controls.SkyrayMsgBox.Show(Info.SelectLayer);
                return;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
