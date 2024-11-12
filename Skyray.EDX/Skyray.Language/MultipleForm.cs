using System;
using System.Windows.Forms;
using Skyray.Controls;

namespace Skyray.Language
{
    public partial class MultipleForm : Form
    {
        public MultipleForm()
        {
            InitializeComponent();
        }

        private void Model_LanguageChanged(object sender, EventArgs e)
        {
            Lang.Model.SetFormText(true, this);
            SetText();
        }
        /// <summary>
        /// 设置文本
        /// </summary>
        public virtual void SetText()
        {

        }
        /// <summary>
        /// 保存文本
        /// </summary>
        public virtual void SaveText()
        {

        }

        private void RecurseFindDatagrid(Control control)
        {
            foreach (Control tempControl in control.Controls)
            {
                if (LanguageModel.ChangedFontByNewFont && (tempControl.GetType() == typeof(DataGridViewW) || tempControl.GetType() == typeof(DataGridView)))
                {
                    tempControl.Font = LanguageModel.newFont;
                }
                else RecurseFindDatagrid(tempControl);
            }
        }
        /// <summary>
        /// 虚-页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void PageLoad(object sender, EventArgs e)
        {
            if (this.DesignMode) return;
            if (Lang.Model != null && Skyray.Language.Param.SaveTextToDB && Skyray.Language.Lang.Model.CurrentLang.IsDefaultLang)
            {
                Lang.Model.SaveFormText(this);
                SaveText();
            }
            else
            {
                RecurseFindDatagrid(this);
                Lang.Model.SetFormText(this);
                SetText();
            }
        }
        protected override void OnCreateControl()
        {
            if (!DesignMode)
            {
                this.Load += new EventHandler(PageLoad);
                Lang.Model.LanguageChanged += new EventHandler(Model_LanguageChanged);
            }
            base.OnCreateControl();
        }
    }
}
