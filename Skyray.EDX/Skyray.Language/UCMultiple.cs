using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.Controls;

namespace Skyray.Language
{
    public partial class UCMultiple : UserControl
    {
        public DialogResult dialogResult;

        [DefaultValue(false)]
        public virtual bool IsSignlObject { get; set; }

        public UCMultiple()
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

        public virtual void ExcuteEndProcess(params object[] objs)
        { }

        //public virtual void OpenSplitUc(bool flag, string TitleName, bool isModel, bool noneStyle)
        //{
        //    if (!this.LoadConditionAnalyser())
        //        return;
        //    Form form = new Form();
        //    form.BackColor = Color.White;
        //    form.MinimizeBox = false;
        //    form.ShowInTaskbar = false;
        //    int padSpace = 0;
        //    form.Padding = new Padding(padSpace, padSpace, padSpace, padSpace);
        //    form.Controls.Add(this);
        //    form.MaximizeBox = flag;
        //    form.Text = TitleName;
        //    //form.FormClosing += (s, ex) =>
        //    //{
        //    //    control.IsSignlObject = false;
        //    //};
        //    if (!flag)
        //    {
        //        form.FormBorderStyle = FormBorderStyle.FixedSingle;
        //    }
        //    if (noneStyle)
        //    {
        //        form.FormBorderStyle = FormBorderStyle.None;
        //    }
        //    this.AutoScroll = true;
        //    form.ClientSize = new Size(this.Width + padSpace * 2, this.Height + padSpace * 2);
        //    int width = Screen.PrimaryScreen.Bounds.Width;
        //    int hight = Screen.PrimaryScreen.Bounds.Height;
        //    if (width < this.Width)
        //        form.ClientSize = new Size(width, form.ClientSize.Height);
        //    if (hight < this.Height)
        //        form.ClientSize = new Size(form.ClientSize.Width, hight);
        //    form.ShowIcon = false;
        //    this.Dock = DockStyle.Fill;
        //    form.StartPosition = FormStartPosition.CenterScreen;
        //    if (isModel)
        //    {
        //        if (DialogResult.OK == form.ShowDialog())
        //            this.ExcuteEndProcess(null);
        //        else
        //        {
        //            this.ExcuteCloseProcess(null);
        //        }
        //    }
        //    else
        //    {
        //        form.Show();
        //    }
        //}


        public virtual void OpenUC(bool flag, string TitleName,bool isModel, bool noneStyle) {
            if (!this.LoadConditionAnalyser())
                return;
            Form form = new Form();
            form.BackColor = Color.White;
            form.MinimizeBox = false;
            form.ShowInTaskbar = false;
            int padSpace = 0;
            form.Padding = new Padding(padSpace, padSpace, padSpace, padSpace);
            form.Controls.Add(this);
            form.MaximizeBox = flag;
            form.Text = TitleName;
            //form.FormClosing += (s, ex) =>
            //{
            //    control.IsSignlObject = false;
            //};
            if (!flag)
            {
                form.FormBorderStyle = FormBorderStyle.FixedSingle;
            }
            if (noneStyle)
            {
                form.FormBorderStyle = FormBorderStyle.None;
            }
            this.AutoScroll = true;
            form.ClientSize = new Size(this.Width + padSpace * 2, this.Height + padSpace * 2);
            int width = Screen.PrimaryScreen.Bounds.Width;
            int hight = Screen.PrimaryScreen.Bounds.Height;
            if (width < this.Width)
                form.ClientSize = new Size(width, form.ClientSize.Height);
            if (hight < this.Height)
                form.ClientSize = new Size(form.ClientSize.Width, hight);
            form.ShowIcon = false;
            this.Dock = DockStyle.Fill;
            form.StartPosition = FormStartPosition.CenterScreen;
            if (isModel)
            {
                if (DialogResult.OK == form.ShowDialog())
                    this.ExcuteEndProcess(null);
                else
                {
                    this.ExcuteCloseProcess(null);
                }
            }
            else
            {
                form.ShowInTaskbar = true;
                form.Show();
            }
        }

        public virtual void ExcuteCloseProcess(params object[] objs)
        { }

        public virtual bool LoadConditionAnalyser() { return true; }


        private void RecurseFindDatagrid(Control control)
        {
            foreach (Control tempControl in control.Controls)
            {
                if (LanguageModel.ChangedFontByNewFont&&(tempControl.GetType() == typeof(DataGridViewW) || tempControl.GetType() == typeof(DataGridView)))
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
            if (this.DesignMode || Lang.Model== null) return;
            if (Lang.Model != null && Lang.Model.CurrentLang.IsDefaultLang && Skyray.Language.Param.SaveTextToDB)
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
                if (Lang.Model != null)
                    Lang.Model.LanguageChanged += new EventHandler(Model_LanguageChanged);
            }
            base.OnCreateControl();
        }

        private void UCMultiple_Load(object sender, EventArgs e)
        {

        }

        //protected override bool ProcessDialogKey(Keys keyData)
        //{
        //    return base.ProcessDialogKey(keyData);
        //}
    }
}
