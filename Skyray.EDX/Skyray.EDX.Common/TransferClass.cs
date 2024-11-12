using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using Skyray.Controls;
using Skyray.Language;

namespace Skyray.EDX.Common
{
    /// <summary>
    ///配置工具基本布局中传递的对象，由用户控件UserControlDefinite向配置软件界面UserConfigureForm抛出对象
    ///由配置软件界面进行处理。
    /// </summary>
    public class ParamsTransferClass
    {
        /// <summary>
        /// 导航栏的方向
        /// </summary>
        public DockStyle NaviStyle { set; get; }

        /// <summary>
        /// 布局分区类型，如按行，还是按列
        /// </summary>
        public PartitionStyle PartitionStyle { set; get; }

        /// <summary>
        /// 程序的名称
        /// </summary>
        public string ProgramTitle { set; get; }

        /// <summary>
        /// 客户区分区的数目
        /// </summary>
        public int SettingNumber { set; get; }

        /// <summary>
        /// 是否显示菜单
        /// </summary>
        public bool ShowMenu { set; get; }

        /// <summary>
        /// 是否显示状态
        /// </summary>
        public bool ShowStatus { set; get; }

        /// <summary>
        /// 是否显示工具
        /// </summary>
        public bool ShowTools { set; get; }

        public ParamsTransferClass(DockStyle naviStyle, PartitionStyle partitionStyle, string title,
            int settingNumber, bool showMenu, bool showTools, bool showStatus)
        {
            this.NaviStyle = naviStyle;
            this.PartitionStyle = partitionStyle;
            this.ProgramTitle = title;
            this.SettingNumber = settingNumber;
            this.ShowMenu = showMenu;
            this.ShowStatus = showStatus;
            this.ShowTools = showTools;

        }
    }

    /// <summary>
    /// 用户控件UserControlDefinite向配置工具界面UserConfigureForm抛出要添加的控件对象
    /// </summary>
    public class SencondTransferClass
    {
        /// <summary>
        /// 父容器的名称
        /// </summary>
        public string SelectPanel { set; get; }

        /// <summary>
        /// 自定义位置信息
        /// </summary>
        public Point DefinitePosition { set; get; }

        /// <summary>
        /// 要添加控件的类型名称
        /// </summary>
        public string ControlTypeName { set; get; }

        /// <summary>
        /// 是导航栏为true，否则为false
        /// </summary>
        public bool PanelFlag { set; get; }

        public bool IsUserControl { set; get; }

        public string ControlName { set; get; }

        public string CellName { set; get; }

        public int ObjectOrient { set; get; }

        public DockStyle ObjectStyle { set; get; }

        /// <summary>
        /// 记录控件的位置
        /// </summary>
        public int ObjectPosition { set; get; }

        public SencondTransferClass(string selectPanel, string controlName,
            bool panelFlag, bool isUserControl, string controlTypeName, string cellName, int objectPosition, int objectOrient)
        {
            this.SelectPanel = selectPanel;
            this.ControlName = controlName;
            this.PanelFlag = panelFlag;
            this.IsUserControl = isUserControl;
            this.ControlTypeName = controlTypeName;
            this.CellName = cellName;
            this.ObjectPosition = objectPosition;
            this.ObjectOrient = objectOrient;
        }
    }

    /// <summary>
    /// 用户控件UserControlDefinite向配置工具界面UserConfigureForm抛出导航band的名字更新
    /// </summary>
    public class NaviSettingPage
    {
        /// <summary>
        /// 设置的band的名称
        /// </summary>
        public string BandName { set; get; }

        /// <summary>
        /// 在导航栏中选择的band的panel的名称
        /// </summary>
        public string SelectPanel { set; get; }

        public NaviSettingPage(string bandName, string selectPanel)
        {
            this.BandName = bandName;
            this.SelectPanel = selectPanel;
        }
    }

    [Serializable]
    public class SplitSetingObject
    {
        public string SplitterName { set; get; }
        public bool stype { set; get; }
        public float rate { set; get; }
        public float minRate { set; get; }
        public int currentTarget { get; set; }
        public int currentMinnum { get; set; }

        public SplitSetingObject(string splitterName, bool style, float rate)
        {
            this.SplitterName = splitterName;
            this.stype = style;
            this.rate = rate;
        }
    }

    public class NaviItem
    {
        private string _Text;

        public string Text
        {
            get { return _Text; }
            set
            {
                if (BtnDropDown != null)
                    BtnDropDown.Text = value;
                //if (Btn != null)
                //    Btn.Text = value;
                if (Lbl != null)
                    Lbl.Text = value;

                if (MenuStripItem != null)
                    MenuStripItem.Text = value;
                if (LabelStrip != null)
                    LabelStrip.Text = value;
                _Text = value;
            }

        }

        private string _Name;

        public bool IsModel;

        public string Name
        {
            get { return _Name; }
            set
            {
                if (BtnDropDown != null)
                    BtnDropDown.Name = "ttbtn" + value;
                if (Btn != null)
                    Btn.Name = "btn" + value;
                if (MenuStripItem != null)
                    MenuStripItem.Name = "msi" + value;
                if (LabelStrip != null)
                    LabelStrip.Name = "ls" + value;
                _Name = value;
            }

        }
        private bool _Enabled = false;

        public bool IsMaxnium = false;

        public bool NoneStyle = false;

        //public bool RegisterEvent = false;

        public bool Enabled
        {
            get { return _Enabled; }
            set
            {
                if (BtnDropDown != null)
                    BtnDropDown.Visible = value;
                if (Btn != null)
                    Btn.Visible = value;
                if (MenuStripItem != null)
                    MenuStripItem.Visible = value;
                if (ComboStrip != null)
                    ComboStrip.Visible = value;
                if (LabelStrip != null)
                    LabelStrip.Visible = value;
                if ((Btn.Tag as Label) != null)
                    (Btn.Tag as Label).Visible = value;
                _Enabled = value;
            }
        }

        private bool _enableControl;
        public bool EnabledControl
        {
            get { return _enableControl; }
            set
            {
                if (BtnDropDown != null)
                {
                    BtnDropDown.Enabled = value;
                    if (!value)
                    {
                        BtnDropDown.AutoToolTip = false;
                        BtnDropDown.ToolTipText = "";
                    }
                    else
                    {
                        BtnDropDown.AutoToolTip = true;
                    }
                }
                if (Btn != null)
                    Btn.Enabled = value;
                if (MenuStripItem != null)
                    MenuStripItem.Enabled = value;
                if (ComboStrip != null)
                    ComboStrip.Enabled = value;
                if (LabelStrip != null)
                    LabelStrip.Enabled = value;
                if ((Btn.Tag as Label) != null)
                    (Btn.Tag as Label).Enabled = value;
                _enableControl = value;
            }
        }
        public Image _Image;

        public Image Image
        {
            get { return _Image; }
            set
            {
                if (BtnDropDown != null)
                    BtnDropDown.Image = value;
                //if (Btn != null)
                //    Btn.Visible = value;
                //if (MenuStripItem != null)
                //    MenuStripItem.Visible = value;
                _Image = value;
            }
        }

        public bool ShowInMain { get; set; }

        public Control UserControl { set; get; }

        public int Index { get; set; }

        public delegate UCMultiple ToAddControl();

        //public delegate Form FormEmbeded();

        //public FormEmbeded FormTT { set; get; }

        public ToAddControl TT { get; set; }

        public Skyray.Controls.ButtonW Btn { get; set; }
        public System.Windows.Forms.Label Lbl { get; set; }

        public delegate void ExcuteRequireProcess(NaviItem item);

        public ExcuteRequireProcess excuteRequire { set; get; }

        public ToolStripButton BtnDropDown { get; set; }

        public ToolStripMenuItem MenuStripItem { set; get; }

        public ToolStripLabel LabelStrip { set; get; }

        public ToolStripComboBox ComboStrip { set; get; }

        public bool ShowInToolBar { get; set; }

        public delegate bool ExcutePreCondition();

        public ExcutePreCondition OnExcutePreCondition;

        public bool ShowInQuickMenuBar { get; set; }

        public bool IsMenuMain { set; get; }

        //public bool ShowInNavi { set; get; }

        public int FlagType { set; get; }//0为按钮，1为文本，2为辅选

        public int FunctionType { set; get; }


        public NaviItem()
        {
            Btn = new ButtonW();
            Lbl = new Label();
            Btn.Tag = Lbl;
            BtnDropDown = new ToolStripButton();
            BtnDropDown.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            BtnDropDown.Click += new EventHandler(ProcessCommon1);
            MenuStripItem = new ToolStripMenuItem();
            MenuStripItem.Click += new EventHandler(ProcessCommon);
            LabelStrip = new ToolStripLabel();
            ComboStrip = new ToolStripComboBox();
            ComboStrip.DropDownStyle = ComboBoxStyle.DropDownList;
            //ShowInToolBar = true;
            //ShowInMain = true;
            //ShowInNavi = true;
            //BtnDropDown.Size = new Size(50, 50);
            //BtnDropDown.BackgroundImageLayout = ImageLayout.Stretch;
            IsModel = true;
            EnabledControl = true;
        }

        public void InitParam()
        {
            Btn.ShowBase = ButtonW.e_showbase.No;
            //Btn.KeepPress = true;
            Btn.ImageAlign = ContentAlignment.MiddleCenter;
            Btn.ImageLocation = ButtonW.e_imagelocation.Top;
            //Btn.TextImageRelation = TextImageRelation.ImageAboveText;
            Btn.Size = new Size(65, 65);
            //Btn.Font = new Font(Btn.Font.Name, 8.0F,
            //    Btn.Font.Style);
            Btn.Image = Image;
          

            Lbl.AutoSize = false;
            Lbl.Size = new Size(220, 20);
            Lbl.TextAlign = ContentAlignment.MiddleCenter;
          

            Btn.BackgroundImageLayout = ImageLayout.Stretch;

            //Btn.Tag = Lbl;
        }
        public NaviItem(string name, bool showInMain, int index)
        {
            this.Name = name;
            this.ShowInMain = showInMain;
            this.Index = index;
        }

        public void InitNaviItem(NaviItemInformation naviInfo)
        {
            this.Name = naviInfo.Name;
            this.Image = naviInfo.Image;
            this.ShowInMain = naviInfo.ShowInMain;
            this.Index = naviInfo.Index;
            this.Enabled = naviInfo.Enable;
            //this.MethodName = naviInfo.MethodName;
            if (naviInfo.UserControlName != null)
            {
                Assembly assebley = Assembly.LoadFrom(@"Skyray.UC.dll");
                object objectControl = assebley.CreateInstance(naviInfo.UserControlName);
                Control control = objectControl as Control;
                this.UserControl = control;
            }
        }

        private void ProcessCommon(object sender, EventArgs e)
        {
            ToolStripMenuItem itemTool = sender as ToolStripMenuItem;
            if (itemTool == null || itemTool.Tag == null)
                return;
            NaviItem NaviItem = itemTool.Tag as NaviItem;
            if (NaviItem.TT == null && excuteRequire != null)
            {
                NaviItem.excuteRequire(NaviItem);
            }
            else if (NaviItem.TT != null)
            {
                var obj = NaviItem.TT();
                if (obj == null) return;
                if (obj.GetType() == typeof(Form))
                    NaviItem.TT().Show();
                else
                {
                    WorkCurveHelper.OpenUC(obj, NaviItem.IsMaxnium, NaviItem.Text, NaviItem.IsModel, NaviItem.NoneStyle);
                }
            }
        }

        private void ProcessCommon1(object sender, EventArgs e)
        {
            ToolStripButton itemTool = sender as ToolStripButton;
            if (itemTool == null || itemTool.Tag == null)
                return;
            NaviItem NaviItem = itemTool.Tag as NaviItem;
            if (NaviItem.TT == null && excuteRequire != null)
            {
                NaviItem.excuteRequire(NaviItem);
            }
            else if (NaviItem.TT != null)
            {
                var obj = NaviItem.TT();
                if (obj == null) return;
                if (obj.GetType() == typeof(Form))
                    obj.Show();
                else
                {
                    WorkCurveHelper.OpenUC(obj, NaviItem.IsMaxnium, NaviItem.Text, NaviItem.IsModel, NaviItem.NoneStyle);
                }
            }
        }
        public override string ToString()
        {
            return this.GetType().FullName + "." + Name;
        }
    }

    [Serializable]
    public enum PartitionStyle
    {
        Row,
        Column,
        None,
    }

    [Serializable]
    public class NaviItemInformation
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public Image Image { get; set; }

        /// <summary>
        /// 用户控件的名称
        /// </summary>
        public string UserControlName { get; set; }

        public Shortcut ShortCutKeys { get; set; }

        /// <summary>
        /// 是否显示在主界面中
        /// </summary>
        public bool ShowInMain { get; set; }

        /// <summary>
        /// 当前的位置索引
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 指定的菜单项，工具项是否可见
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 是否显示在工具栏
        /// </summary>
        public bool ShowInToolBar { get; set; }

        /// <summary>
        /// 是否显示在导航栏中
        /// </summary>
        public bool ShowInQuickMenuBar { get; set; }
        //public string MethodName { set; get; }

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public NaviItemInformation()
        { }

        /// <summary>
        /// 带参数构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="index"></param>
        /// <param name="userControlName"></param>
        /// <param name="showInMain"></param>
        public NaviItemInformation(string name, int index, string userControlName, bool showInMain)
        {
            this.Name = name;
            this.ShowInMain = showInMain;
            this.UserControlName = userControlName;
            this.Index = index;
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="naviItem"></param>
        public void InitNaviItemInformation(NaviItem naviItem)
        {
            this.Name = naviItem.Name;
            this.Image = naviItem.Image;
            this.Index = naviItem.Index;
            this.ShowInMain = naviItem.ShowInMain;
            this.Enable = naviItem.Enabled;
            //this.MethodName = naviItem.MethodName;
            if (naviItem.TT != null && naviItem.TT() != null)
            {
                this.UserControlName = naviItem.TT().GetType().ToString();
            }
        }


    }
}
