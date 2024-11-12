using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using Lephone.Data;
using Lephone.Util;
using Skyray.Controls;

using System.ComponentModel;
using Skyray.EDXRFLibrary;
namespace Skyray.EDX.Common
{
    public class ModelEditor : Panel
    {
        #region 私有变量
        private Grouper GroupBox;
        private ButtonW BtnSave;
        private ButtonW BtnCancel;
        #endregion

        #region 事件
        public event EventHandler OnSaved;
        public event EventHandler OnDataSourceChanged;
        public event EventHandler OnLayoutSourceChanged;
        #endregion

        #region 属性

        public string GroupTitle
        {
            get { return GroupBox == null ? "" : GroupBox.GroupTitle; }
            set
            {
                if (GroupBox != null) GroupBox.GroupTitle = value;
            }
        }

        /// <summary>
        /// 所有自动生成的控件
        /// </summary>
        [Browsable(false), DefaultValue(typeof(List<Control>), "null")]
        public List<Control> AllControls { get; set; }

        /// <summary>
        /// 编辑控件集合
        /// </summary>
        [Browsable(false), DefaultValue(typeof(IEnumerable<Control>), "null")]
        public IEnumerable<Control> EditControls { get; set; }

        /// <summary>
        /// 标签集合
        /// </summary>
        [Browsable(false), DefaultValue(typeof(IEnumerable<Control>), "null")]
        public IEnumerable<Control> LabelControls { get; set; }

        /// <summary>
        /// 标签位置
        /// </summary>
        public LabelPosition LabelPosition { get; set; }

        private object _DataSource;
        /// <summary>
        /// 绑定的数据源对象
        /// </summary>        
        public object DataSource
        {
            get { return _DataSource; }
            set
            {
                if (value != _DataSource && value != null)
                {
                    _DataSource = value;
                    DataSourceChanged();
                    if (OnDataSourceChanged != null) OnDataSourceChanged(this, null);
                }
            }
        }

        #endregion

        private void Bind()
        {
            DataSourceChanged();
            if (OnDataSourceChanged != null) OnDataSourceChanged(this, null);
        }
        private string _SLayoutType;

        public string SLayoutType
        {
            get
            {
                return _SLayoutType;
            }
            set
            {
                _SLayoutType = value;
                if (DesignMode)
                {
                    if (!string.IsNullOrEmpty(_SLayoutType))
                    {
                        var ass = System.Reflection.Assembly.Load(@"Skyray.EDXRFLibrary");
                        var typ = ass.GetType(_SLayoutType);
                        if (typ != null)
                        {
                            Label labl = new Label();
                            labl.Text = "测试";
                            Controls.Add(labl);
                        }
                    }
                }

            }
        }

        public Type LayoutType { get; set; }

        private LayoutSource _LayoutSource;
        /// <summary>
        /// 界面控件数据源
        /// </summary>
        [Browsable(false), DefaultValue(typeof(LayoutSource), "null")]
        public LayoutSource LayoutSource
        {
            get { return _LayoutSource; }
            set
            {
                if (_LayoutSource != value)
                {
                    _LayoutSource = value;
                    LayoutSourceChanged();
                    if (OnLayoutSourceChanged != null) OnLayoutSourceChanged(this, null);
                }
            }
        }

        private BindingSource BS { get; set; }

        public ModelEditor()
        {

        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitDYControl()
        {
            if (LayoutSource.ShowGroupBox)
            {
                GroupBox = CtrlFactory.GetGrouper();//Grouper       
                GroupBox.Location = new Point(5, 5);
                GroupBox.Size = new Size(this.Width - 10, this.Height - 10);
                GroupBox.Anchor = AnchorStyles.Bottom |
                    AnchorStyles.Left |
                    AnchorStyles.Right |
                    AnchorStyles.Top;
                this.Controls.Add(GroupBox);//添加控件
            }
            else
            {
                this.Controls.Remove(GroupBox);//移除控件
                GroupBox = null;
            }

            if (LayoutSource.ShowSaveCancelButton)
            {
                Point p1 = Point.Empty;
                if (BtnCancel == null)
                {
                    BtnCancel = CtrlFactory.GetButtonW();
                    p1 = new Point((this.Width - BtnCancel.Width * 2 - 20) / 2, this.Height - 40);
                    BtnCancel.Location = new Point(p1.X + 20 + BtnCancel.Width, p1.Y);
                    BtnCancel.Text = Info.Cancel;
                    BtnCancel.Click += new EventHandler(BtnCancel_Click);
                    this.Controls.Add(BtnCancel);
                }
                if (BtnSave == null)
                {
                    BtnSave = CtrlFactory.GetButtonW();
                    BtnSave.Text = Info.Save;
                    BtnSave.Location = p1;
                    BtnSave.Click += new EventHandler(BtnSave_Click);
                    this.Controls.Add(BtnSave);
                }
            }
            else
            {
                if (BtnSave != null)
                {
                    this.Controls.Remove(BtnSave);//移除控件
                    BtnSave = null;//释放资源
                }
                if (BtnCancel != null)
                {
                    this.Controls.Remove(BtnCancel);//移除控件
                    BtnCancel = null;//释放资源
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Save();
            if (this.TopLevelControl is Form)
            {
                Form form = this.TopLevelControl as Form;
                form.DialogResult = DialogResult.OK;
                form.Close();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (this.TopLevelControl is Form)
            {
                (this.TopLevelControl as Form).Close();
            }
        }

        private void FillContainer()
        {
            Type typ = LayoutType;//获取类型
            if (typ == null) return;
            var autoFlag = ClassHelper.GetAttribute<Auto>(typ, true);//获取Auto属性
            GroupBox.GroupTitle = autoFlag == null ? string.Empty : autoFlag.Text;

            char[] splitChar = new char[] { ',' };
            string[] strs = LayoutSource.BExclude ?
                LayoutSource.ExcludeFields.Split(splitChar, StringSplitOptions.RemoveEmptyEntries) :
                LayoutSource.IncludeFields.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);

            AllControls = CtrlFactory.GetEditCtrls(typ, LayoutSource.BExclude, strs);//所有自动生成的控件
            LabelControls = CtrlFactory.GetLabelCtrlList(AllControls);
            EditControls = CtrlFactory.GetEditCtrlList(AllControls);

            if (LayoutSource.ShowGroupBox)
                CtrlFactory.FillCtrlToContainer(GroupBox, LayoutSource.ColCount, LayoutSource.LabelPos, AllControls, true);
            else
                CtrlFactory.FillCtrlToContainer(this, LayoutSource.ColCount, LayoutSource.LabelPos, AllControls, true);
        }

        private void LayoutSourceChanged()
        {
            InitDYControl();
            FillContainer();
        }

        private void DataSourceChanged()
        {
            if (LayoutSource == null || EditControls == null || DataSource == null) return;
            BS = new BindingSource(DataSource, "");//构造绑定数据源
            CtrlFactory.BindValue(EditControls, BS, !LayoutSource.ShowSaveCancelButton);
        }

        private void Save()
        {
            if (BS == null) return;
            BS.RaiseListChangedEvents = false;
            CtrlFactory.WriteValue(EditControls);
            BS.RaiseListChangedEvents = true;
            if (OnSaved != null) OnSaved(this, null);
        }

        public void Init(Type typeOfModel, LayoutSource layoutSource)
        {
            Init(typeOfModel, layoutSource, null);
        }

        public void Init(Type typeOfModel, LayoutSource layoutSource, object objSource)
        {
            LayoutType = typeOfModel;
            LayoutSource = layoutSource;
            DataSource = objSource;
        }
    }

    public enum LabelPosition
    {
        Top,
        Left
    }
    public enum SpliterType
    {
        HTop,
        HBottom,
        VLeft,
        VRight
    }
}
