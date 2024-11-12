using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Skyray.Controls;
using Skyray.Controls.Extension;

namespace Skyray.EDX.Common
{
    [Serializable]
    public class UserDataInitialization
    {
        /// <summary>
        /// 导航栏对象
        /// </summary>
        public ContainObjectInformation[] DataSourceNavi { get; set; }

        /// <summary>
        /// 客户区对象
        /// </summary>
        public ContainObjectInformation[] DataSourceClient { set; get; }

        /// <summary>
        /// 是否显示启动画面
        /// </summary>
        public  bool ShowSplasher { get; set; }

        /// <summary>
        /// 启动画面显示图片
        /// </summary>
        public  Image SplasherImage { get; set; }

        /// <summary>
        /// 主窗体标题
        /// </summary>
        public  string Title { get; set; }

        /// <summary>
        /// 主窗体谱图
        /// </summary>
        public List<SpecData> MainSpecData { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public  Size MiniSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public  UIStyle Layout { get; set; }

        /// <summary>
        /// 是否显示为状态栏标记位
        /// </summary>
        public  bool ShowStatusBar { get; set; }

        /// <summary>
        /// 是否显示菜单栏标记位
        /// </summary>
        public  bool ShowMenuBar { get; set; }

        /// <summary>
        /// 是否显示工具栏标记位
        /// </summary>
        public  bool ShowToolStrip { get; set; }

        /// <summary>
        /// 导航栏方向
        /// </summary>
        public DockStyle Navistyle { set; get; }

        /// <summary>
        /// 主客户区排列风格
        /// </summary>
        public PartitionStyle PartitionStyle { set; get; }

        /// <summary>
        /// 主客户区分区的数目
        /// </summary>
        public int PartitionNumber { set; get; }

        /// <summary>
        /// 主客户区目前的方向
        /// </summary>
        public DockStyle MainClientStyle { set; get; }


        /// <summary>
        /// 记录配置工具中生成的Datagridviw的名称
        /// </summary>
        public List<string> DataGridViewWName { set; get; }

        /// <summary>
        /// 用户希望的位置方向
        /// </summary>
        public int SplitPosition { set; get; }

        //public Dictionary<string, float> SplitterPlan;

        /// <summary>
        /// 各个菜单项集合
        /// </summary>
        public List<MenuConfig> listMenu { set; get; }
        
        public List<SplitSetingObject> listSplit { set; get; }

        public UserDataInitialization()
        {
            listMenu = new List<MenuConfig>();
            DataGridViewWName = new List<string>();
            listSplit = new List<SplitSetingObject>();
        }
    }



    /// <summary>
    /// 界面样式
    /// </summary>
    public enum UIStyle
    {
        XRF,
        Rohs,
        Thick
    }
    #region
    //FrmMain.DataSourceStatus = new StatusInfo[] { 
    //new StatusInfo("联机状态","脱机",false),
    //new StatusInfo("联机状态","脱机",true),
    //new StatusInfo("联机状态","脱机",false)
    //};

    //ConfigurationUserControl testControl = new ConfigurationUserControl();
    //testControl.OrientStype = 1;
    //Point startPoint = new Point(40, 0);
    //testControl.OrientCoordinate = startPoint;

    //testControl.AddToConfigurationCollection(new DynamicControls(startPoint, new UCCurve(), null));

    //ConfigurationUserControl testControl1 = new ConfigurationUserControl();
    //testControl1.AddToConfigurationCollection(new DynamicControls(startPoint, new ButtonW(), null));
    //testControl1.AddToConfigurationCollection(new DynamicControls(startPoint, new ButtonW(), null));
    //testControl1.AddToConfigurationCollection(new DynamicControls(startPoint, new ButtonW(), null));
    //testControl1.OrientStype = 1;
    //Point point = new Point(10, 10);
    //testControl1.OrientCoordinate = point;


    //FrmMain.DataSourceNavi = new ContainerObject[] { 
    //   new ContainerObject("Item1",Res.bookmark,Res.bookmark_big,testControl,true),
    //   new ContainerObject("Item2",Res.wizard,Res.wizard_big,testControl1,false)

    //};
    #endregion
}
