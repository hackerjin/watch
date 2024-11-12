using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Skyray.Print
{
    /// <summary>
    /// 页面属性
    /// </summary>
    public partial class UCProperty : Skyray.Language.UCMultiple
    {
        /// <summary>
        /// 页面对象
        /// </summary>
        private PrintPage _Page;

        /// <summary>
        /// 页面对象
        /// </summary>
        [Browsable(false)]
        public PrintPage Page
        {
            get { return _Page; }
            set
            {
                if (value == null || value == _Page) return;
                _Page = value;
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public UCProperty()
        {
            InitializeComponent();
            this.Visible = false;//初始时不显示
        }

        /// <summary>
        /// 当前选择对象
        /// </summary>
        private object _PropertyObject;

        /// <summary>
        /// 当前选择对象
        /// </summary>
        [Browsable(false), DefaultValue(null)]
        public object PropertyObject
        {
            get
            {
                return this._PropertyObject;//返回
            }
            set
            {
                if (value == null || value.GetType() == typeof(PrintPanel))
                {
                    this.Visible = false;
                    return;
                }
                this.Visible = true;
               
                this._PropertyObject = value;

                var printCtrl = value as PrintCtrl;
               // pnlSaveValue.Visible = printCtrl.Panel.Type != CtrlType.Body;
                var info = printCtrl.NodeInfo;
            
                txtLabel.Enabled = info.EnableTextEdit;//文本编辑状态
                colorLabel.Enabled = info.EnableTextColorEdit;//文本颜色编辑状态
                fontLabel.Enabled = info.EnableTextFontEdit; //文本字体编辑状态                      

                BindHelper.BindCheckedToCtrl(chkSaveValue, printCtrl, "SaveValue", true);
                BindHelper.BindTextToCtrl(txtLabel, printCtrl, "Text", true);//Text
                BindHelper.BindValueToCtrl(colorLabel, printCtrl, "TextColor", true);//TextColor
                BindHelper.BindValueToCtrl(fontLabel, printCtrl, "TextFont", true);//TextFont

                switch (printCtrl.Type)
                {
                    case CtrlType.Field://Field
                        tabCtrlProperty.SelectedIndex = 0;//设置索引
                        pnlTextProperty.Visible = true;//显示文本设置Panel
                        BindHelper.BindTextToCtrl(txtFieldValue, printCtrl, "TextValue", true);
                        BindHelper.BindValueToCtrl(fontField, printCtrl, "TextValueFont", true);
                        BindHelper.BindValueToCtrl(colorField, printCtrl, "TextValueColor", true);
                        break;

                    case CtrlType.Image://Image
                        tabCtrlProperty.SelectedIndex = 1;//设置索引
                        pnlTextProperty.Visible = true;//显示文本设置Panel
                        BindHelper.BindValueToCtrl(colorImageBorder, printCtrl, "ImageBorderColor", true);
                        BindHelper.BindCheckedToCtrl(chkShowImageBorder, printCtrl, "DrawImageBorder", true);
                        BindHelper.BindValueToCtrl(numTextVSpaceOfImage, printCtrl, "TextVSpace", true);
                        break;

                    case CtrlType.Grid://Grid
                        tabCtrlProperty.SelectedIndex = 2;//设置索引
                        pnlTextProperty.Visible = true;//显示文本设置Panel
                        //BindHelper.BindTextToCtrl(cboGridBorderStyle, printCtrl, "GridBorderType", true);
                        BindHelper.BindValueToCtrl(numTextVSpaceOfGrid, printCtrl, "TextVSpace", true);
                        break;

                    case CtrlType.Line://Line
                        tabCtrlProperty.SelectedIndex = 3;//设置索引
                        pnlTextProperty.Visible = true;//显示文本设置Panel
                        break;
                    case CtrlType.Label://Label
                        pnlTextProperty.Visible = true;//显示文本设置Panel
                        tabCtrlProperty.SelectedIndex = 4;//tabPage为空
                        break;
                    case CtrlType.ComposeTable:
                        //pnlTextProperty.Visible = true;//显示文本设置Panel
                        //tabCtrlProperty.SelectedIndex = 5;//tabPage为空
                        break;
                    default:
                        tabCtrlProperty.SelectedIndex = 4;//tabPage为空
                        pnlTextProperty.Visible = false;//不显示文本设置Panel
                        break;
                }
            }
        }

        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void UCPropertyLoad(object sender, EventArgs e)
        {
            tabCtrlProperty.ShowTabs = false;//不显示Tabs

            var items = Enum.GetNames(typeof(BorderType));//获取边框类型集合

            foreach (var item in items) cboGridBorderStyle.Items.Add(item);//表格边框样式

            //页面样式设置
            foreach (TabPage page in tabCtrlProperty.TabPages)
            {
                page.UseVisualStyleBackColor = false;
                page.BackColor = Color.White;
            }
        }

        private void chkShowImageBorder_CheckedChanged(object sender, EventArgs e)
        {
            //BindHelper.BindCheckedToCtrl(chkShowImageBorder, printCtrl, "DrawImageBorder", true);
        }

        private void nupdownColumn_ValueChanged(object sender, EventArgs e)
        {

        }

        private void lblColumnsIndex_Click(object sender, EventArgs e)
        {

        }

        private void nupdownRow_ValueChanged(object sender, EventArgs e)
        {

        }

        private void lblRowsIndex_Click(object sender, EventArgs e)
        {

        }
    }
}
