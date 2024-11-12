using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Skyray.Controls;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Skyray.EDX.Common
{
    public class ContainerObject : System.Windows.Forms.Panel, ISkyrayStyle
    {

        /// <summary>
        /// 容器的属性，false请求方，true还是相应方
        /// </summary>
        public bool ContainerAttribute { set; get; }

        /// <summary>
        /// 附近信息
        /// </summary>
        public string ContainerLabel { get; set; }

        private string name1;
        public string Name1
        {
            get { return name1; }
            set { name1 = value; }
        }
//<<<<<<< .mine
        [DefaultValue(true)]
        private bool visible = true;
        public bool Visible { get { return visible; } set { visible = value; } }
        
//=======
//        [DefaultValue(true)]
//        private bool visible = true;
//        public bool Visible { get { return visible; } set { visible = value; } }

//>>>>>>> .r2209
        //public new string Name { get { return base.Name; } set { base.Name = value; } }

        /// <summary>
        /// 大图标
        /// </summary>
        public Image SmallImage { get; set; }

        /// <summary>
        /// 小图标
        /// </summary>
        public Image BigImage { get; set; }

        /// <summary>
        /// 指定方向控件见的间隔
        /// </summary>
        public int ControlInternal { set; get; }

        /// <summary>
        /// 排列方向，0为水平方向，1为垂直方向，2为根据内部对象坐标放置
        /// </summary>
        //public PartitionStyle OrientStype { set; get; }

        /// <summary>
        /// 为false为按照开始位置坐标和控件见的间隔进行排列，
        /// 否则为按照内部元素的位置信息进行排列
        /// </summary>
        public bool IncludeInnerCoordinate { set; get; }

        /// <summary>
        /// 判断是否递归嵌入
        /// </summary>
        public bool IsReverseEmbeded { set; get; }

        /// <summary>
        /// 目前容器的索引
        /// </summary>
        public int CurrentPanelIndex { set; get; }

        /// <summary>
        /// 当前待分区的数目
        /// </summary>
        public int CurrentPlanningNumber { set; get; }

        private ToolStripProfessionalRenderer _renderer;

        public ToolStripProfessionalRenderer Renderer
        {
            get
            {
                if (_renderer == null)
                    this._renderer = new Office2007Renderer();
                return _renderer;
            }
            set
            {
                _renderer = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ContainerObject()
            : base()
        {
            //Visible = true;
            this.AutoScroll = true;
            SetStyles();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_renderer == null)
                this._renderer = new Office2007Renderer();
            if (ClientRectangle.Width > 0 && ClientRectangle.Height > 0)
            {
                using (LinearGradientBrush backBrush = new LinearGradientBrush(this.ClientRectangle,
                                                                                   _renderer.ColorTable.ToolStripContentPanelGradientEnd,
                                                                                   _renderer.ColorTable.ToolStripContentPanelGradientBegin,
                                                                                   90f))
                {
                    e.Graphics.FillRectangle(backBrush, this.ClientRectangle);
                }
            }
        }

        private void SetStyles()
        {
            base.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.ResizeRedraw, true);
            base.UpdateStyles();
        }

        public void InitContainerObject(ContainObjectInformation containerInfo)
        {
            this.Dock = containerInfo.ContainerStyle;
            this.Name = containerInfo.ContainerName;
            this.ContainerAttribute = containerInfo.ContainerAttribute;
            this.ContainerLabel = containerInfo.ContainerLabel;
            this.ControlInternal = containerInfo.ControlInternal;
            this.SmallImage = containerInfo.SmallImage;
            this.BigImage = containerInfo.BigImage;
            this.IncludeInnerCoordinate = containerInfo.IncludeInnerCoordinate;
            this.IsReverseEmbeded = containerInfo.IsReverseEmbeded;
            this.CurrentPanelIndex = containerInfo.CurrentPanelIndex;
            this.CurrentPlanningNumber = containerInfo.CurrentPlanningNumber;
        }

        #region ISkyrayStyle Members

        public void SetStyle(Style style)
        {
            switch (style)
            {
                case Style.Office2007Blue:
                    this._renderer = new Office2007Renderer();
                    this.Refresh();
                    break;
                case Style.Office2007Sliver:
                    this._renderer = new Office2007Renderer(new Office2007SilverColorTable());
                    this.Refresh();
                    break;
                default: break;
            }
        }

        private Style _Style = Style.Custom;
        public Style Style
        {
            get
            {
                return _Style;
            }
            set
            {
                _Style = value;
                SetStyle(_Style);
            }
        }
        #endregion
    }
}
