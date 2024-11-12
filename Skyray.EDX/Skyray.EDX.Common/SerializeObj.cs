using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Skyray.EDX.Common
{
    [Serializable]
    public class ContainObjectInformation
    {
        /// <summary>
        /// 容器类型名称
        /// </summary>
        public string ContainerObjectType { set; get; }

        /// <summary>
        /// 容器的风格
        /// </summary>
        public DockStyle ContainerStyle { set; get; }

        /// <summary>
        /// 面板的名称
        /// </summary>
        public string ContainerName { set; get; }

        /// <summary>
        /// 容器包含的对象集合信息
        /// </summary>
        public List<ControlInformation> ContainControls { set; get; }

        /// <summary>
        /// 容器的属性，false请求方，true还是相应方
        /// </summary>
        public bool ContainerAttribute { set; get; }

        /// <summary>
        /// 附近信息
        /// </summary>
        public string ContainerLabel { get; set; }

         ///<summary>
         ///大图标
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

        /// <summary>
        /// 根据ContainerObject对象初始化
        /// </summary>
        /// <param name="containerObject"></param>
        public void InitContainerInfo(ContainerObject containerObject)
        {
            this.ContainerObjectType = containerObject.GetType().ToString();
            this.ContainerStyle = containerObject.Dock;
            this.ContainerName = containerObject.Name;
            this.ContainerAttribute = containerObject.ContainerAttribute;
            this.ContainerLabel = containerObject.ContainerLabel;
            this.ControlInternal = containerObject.ControlInternal;
            //this.OrientStype = containerObject.OrientStype;
            this.SmallImage = containerObject.SmallImage;
            this.BigImage = containerObject.BigImage;
            this.IncludeInnerCoordinate = containerObject.IncludeInnerCoordinate;
            this.IsReverseEmbeded = containerObject.IsReverseEmbeded;
            this.CurrentPanelIndex = containerObject.CurrentPanelIndex;
            this.CurrentPlanningNumber = containerObject.CurrentPlanningNumber;
        }


    }

    [Serializable]
    public class ControlInformation : ContainObjectInformation
    {
        /// <summary>
        /// 控件的信息
        /// </summary>
        public string ControlType { set; get; }

        /// <summary>
        /// 0为skyray控件，1为用户控件，2为一般控件,3为摄像头控件，4为谱图控件
        /// </summary>
        public int IsUserControl { set; get; }

        /// <summary>
        /// 控件的排放
        /// </summary>
        public DockStyle ControlStyle { set; get; }

        /// <summary>
        /// 对于控件内再包含控件的情况
        /// </summary>
        public List<ControlInformation> controlContainer { set; get; }

        /// <summary>
        /// 对象在整个界面的布局位置
        /// </summary>
        public Point ObjectPosition { set; get; }

        /// <summary>
        /// 内嵌对象的名字
        /// </summary>
        public string objectName { set; get; }

        /// <summary>
        /// 是否为流排列
        /// </summary>
        public bool IsFlowLayout { set; get; }


        /// <summary>
        /// 数据容器的位置
        /// </summary>
        public int DataGridViewPosition { set; get; }

        public string DataGridViewName { set; get; }

        /// <summary>
        /// 数据容器的方向
        /// </summary>
        public int DataGridViewOrient { set; get; }

        /// <summary>
        /// 是否为表排列
        /// </summary>
        public bool IsTabLayout { set; get; }

        /// <summary>
        /// 行数
        /// </summary>
        public int rowNumber { set; get; }

        /// <summary>
        /// 列数
        /// </summary>
        public int ColumnNumber { set; get; }

        /// <summary>
        /// 按钮事件对象信息
        /// </summary>
        //public NaviItemInformation NaviItem { set; get; }

        /// <summary>
        /// 是否为tabControl
        /// </summary>
        public bool IsTabControl { set; get; }

        /// <summary>
        /// tabControl中tabPage集合及相应的控件
        /// </summary>
        //public Dictionary<string, TabPageInforControl> TabControlPage { set; get; }

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public ControlInformation()
        {
            //this.NaviItem = new NaviItemInformation();
        }

        /// <summary>
        /// 对相应的对象的属性进行赋值
        /// </summary>
        /// <param name="controls"></param>
        public void InitControlInformation()
        {
            //if (controls.EmbededObject != null)
            //    this.ControlType = controls.EmbededObject.GetType().ToString();
            //this.ControlStyle = controls.ObjectStyle;
            //this.ObjectPosition = controls.ObjectPosition;
            //if (controls.EmbededObject != null)
            //    this.objectName = (controls.EmbededObject as Control).Name;
            //this.IsUserControl = controls.IsUserControl;
            //this.IsTabLayout = controls.IsTabLayout;
            //this.IsFlowLayout = controls.IsFlowLayout;
            //this.IsTabControl = controls.IsTabControl;
            //this.rowNumber = controls.rowNumber;
            //this.DataGridViewPosition = controls.DataGridViewPosition;
            //this.DataGridViewOrient = controls.ObjectOrient;
            //this.ColumnNumber = controls.ColumnNumber;
            //if (controls.ControlNavItem != null)
            //    this.NaviItem.InitNaviItemInformation(controls.ControlNavItem);
            //if (controls.TabControlPage != null)
            //    this.TabControlPage = controls.TabControlPage;
        }

        /// <summary>
        /// 对递归对象容器的信息进行填充
        /// </summary>
        /// <param name="containObjectInfo"></param>
        public void InitContainerControl(ContainObjectInformation containObjectInfo)
        {
            this.ContainerObjectType = containObjectInfo.ContainerObjectType;
            this.ContainerStyle = containObjectInfo.ContainerStyle;
            this.ContainerName = containObjectInfo.ContainerName;
            this.objectName = containObjectInfo.ContainerName;
            this.ContainerAttribute = containObjectInfo.ContainerAttribute;
            this.ContainerLabel = containObjectInfo.ContainerLabel;
            this.ControlInternal = containObjectInfo.ControlInternal;
            //this.OrientStype = containObjectInfo.OrientStype;
            //this.SmallImage = containObjectInfo.SmallImage;
            //this.BigImage = containObjectInfo.BigImage;
            this.IncludeInnerCoordinate = containObjectInfo.IncludeInnerCoordinate;
            this.IsReverseEmbeded = containObjectInfo.IsReverseEmbeded;
            this.CurrentPanelIndex = containObjectInfo.CurrentPanelIndex;
            this.controlContainer = containObjectInfo.ContainControls;
        }

    }
}
