using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using System.Windows.Forms;
using System.Runtime.Remoting;
using Skyray.Controls;
using Skyray.Controls.NavigationBar;
using System.Drawing;
using System.ComponentModel;

using Skyray.Controls.Extension;

namespace Skyray.EDX.Common
{
    public class libSerialize
    {

        //public static string fileName = "test.txt";
        public static List<string> datagridviewName = new List<string>();
        public static XRFChart xrfChart;
        public static List<DataGridViewW> wholeDataView = new List<DataGridViewW>();
        public static ContainerObject clientPannel;
        public static List<object> DyObjs = new List<object>();
        public static NaviBar naviBar;
        public static ContainerObject containerObjTemp;

        //public static List<DataGridView> DGVs = new List<DataGridView>();


        public static void StoreControlInformation(ContainerObject containerObject, ref ContainObjectInformation containerInformation)
        {
            if (containerObject == null)
                return;
            containerInformation.InitContainerInfo(containerObject);
            List<ControlInformation> controlInformationList = new List<ControlInformation>();
            if (containerObject.Controls.Count == 0)
                return;
            foreach (System.Windows.Forms.Control controls in containerObject.Controls)
            {
                //包含的只是控件，而不是panel
                ControlInformation controlInformation = new ControlInformation();
                if (!(controls is ContainerObject))
                {
                    controlInformation.ContainerObjectType = controls.GetType().AssemblyQualifiedName;
                    controlInformation.ContainerStyle = controls.Dock;
                    controlInformation.objectName = controls.Name;
                    if (controls.Tag != null)
                    {
                        controlInformation.DataGridViewPosition = int.Parse(controls.Tag.ToString());
                        controlInformation.DataGridViewName = controls.AccessibleName;
                    }
                    controlInformationList.Add(controlInformation);
                    List<ControlInformation> controlInformationListChild = new List<ControlInformation>();
                    if (controls is TabControlW || controls is TabControl)
                    {
                        TabControl tabControls = controls as TabControl;
                        if (tabControls.TabPages.Count > 0)
                        {
                            foreach (TabPage tabPage in tabControls.TabPages)
                            {
                                if (tabPage.Controls.Count == 0)
                                    continue;

                                if (tabPage.Controls[0] is ContainerObject)
                                {
                                    ContainerObject recurveObj = tabPage.Controls[0] as ContainerObject;
                                    ContainObjectInformation containerInformationIon = new ContainObjectInformation();
                                    ControlInformation controlInformationChild = new ControlInformation();
                                    StoreControlInformation(recurveObj, ref containerInformationIon);
                                    controlInformationChild.InitContainerControl(containerInformationIon);
                                    controlInformationChild.ContainerName = tabPage.Name;
                                    controlInformationChild.ContainerLabel = tabPage.Text;
                                    controlInformationListChild.Add(controlInformationChild);
                                }
                            }
                            controlInformation.controlContainer = controlInformationListChild;
                        }
                    }
                    if (controls is NaviBar)
                    {
                        NaviBar naviBar1 = controls as NaviBar;
                        if (naviBar1.Bands.Count > 0)
                        {
                            foreach (NaviBand naviBand in naviBar1.Bands)
                            {
                                if (naviBand.ClientArea.Controls.Count == 0)
                                    continue;
                                if (naviBand.ClientArea.Controls[0] is ContainerObject)
                                {
                                    ContainerObject recurveObj = naviBand.ClientArea.Controls[0] as ContainerObject;
                                    ContainObjectInformation containerInformationIon = new ContainObjectInformation();
                                    ControlInformation controlInformationChild = new ControlInformation();
                                    StoreControlInformation(recurveObj, ref containerInformationIon);
                                    controlInformationChild.InitContainerControl(containerInformationIon);
                                    controlInformationChild.ContainerName = naviBand.Name;
                                    controlInformationChild.ContainerLabel = naviBand.Text;
                                    controlInformationListChild.Add(controlInformationChild);
                                }
                            }
                            controlInformation.controlContainer = controlInformationListChild;
                        }
                    }
                    continue;
                }
                ContainObjectInformation containerInformation0 = new ContainObjectInformation();
                ContainerObject ContainerTemp = controls as ContainerObject;
                StoreControlInformation(ContainerTemp, ref containerInformation0);
                controlInformation.InitContainerControl(containerInformation0);
                controlInformationList.Add(controlInformation);
            }
            containerInformation.ContainControls = controlInformationList;
        }

        /// <summary>
        /// 对ContainerObject集合进行抽取操作
        /// </summary>
        /// <param name="containerArray"></param>
        /// <returns></returns>
        public static ContainObjectInformation[] ExtractContainerObjectArray(List<ContainerObject> containerArray)
        {
            List<ContainObjectInformation> ContainObjectInformation = new List<ContainObjectInformation>();
            if (containerArray == null || containerArray.Count == 0)
                return null;
            foreach (ContainerObject enumerationObject in containerArray)
            {
                ContainObjectInformation containerInformation = new ContainObjectInformation();
                StoreControlInformation(enumerationObject, ref containerInformation);
                ContainObjectInformation.Add(containerInformation);
            }
            return ContainObjectInformation.ToArray();
        }

        /// <summary>
        /// 反序列化对象，并由对象产生ContainObject集合
        /// </summary>
        /// <param name="userDataInitialization"></param>
        public static List<ContainerObject> DeserializationObject(ContainObjectInformation[] userDataInformation)
        {
            if (userDataInformation == null && userDataInformation.Length == 0)
                return null;
            List<ContainerObject> listContainer = new List<ContainerObject>();
            foreach (ContainObjectInformation containInfo in userDataInformation)
            {
                if (containInfo == null)
                    continue;
                Type type = Type.GetType(containInfo.ContainerObjectType);
                object objectT = type.Assembly.CreateInstance(containInfo.ContainerObjectType);
                AddCtrlToList(objectT);
                ContainerObject tempContainer = objectT as ContainerObject;
                tempContainer.InitContainerObject(containInfo);
                if (containInfo.ContainControls != null && containInfo.ContainControls.Count > 0)
                    ControlIncludedChildControl(containInfo.ContainControls, tempContainer);
                listContainer.Add(tempContainer);
            }
            return listContainer;
        }

       
        /// <summary>
        /// 对面板中包含的控件进行反序列化为对象
        /// </summary>
        /// <param name="containInfo"></param>
        /// <param name="tempContainer"></param>
        private static void ControlIncludedChildControl(List<ControlInformation> containInfo, ContainerObject tempContainer)
        {
            if (containInfo == null || tempContainer == null)
                return;
            foreach (ControlInformation controlInfo in containInfo)
            {

                Type type = Type.GetType(controlInfo.ContainerObjectType); // 获取类型
                object objectControl = Activator.CreateInstance(type); // 创建类型的实例，即控件
             
                if (controlInfo.objectName == "ClientMain")
                    clientPannel = objectControl as ContainerObject;
                System.Windows.Forms.Control control = null;
                if (objectControl != null)
                {
                    control = objectControl as System.Windows.Forms.Control;
                    if (control is XRFChart)
                        xrfChart = control as XRFChart;
                    if (control is NaviBar)
                    {
                        NaviBar naviBar1 = control as NaviBar;
                        ((System.ComponentModel.ISupportInitialize)(naviBar1)).BeginInit();
                        control = objectControl as System.Windows.Forms.Control;
                        control.Dock = controlInfo.ContainerStyle;
                        //control.Dock = DockStyle.Left;
                        control.Name = controlInfo.objectName;
                        if (controlInfo.controlContainer != null && controlInfo.controlContainer.Count > 0)
                        {
                            foreach (ControlInformation childInfo in controlInfo.controlContainer)
                            {
                                NaviBand band = new NaviBand();
                                ContainerObject containerObj = new ContainerObject();
                                band.Name = childInfo.ContainerName;
                                band.Text = Info.InfoPnl;
                                ControlIncludedChildControl(childInfo.controlContainer, containerObj);
                                band.ClientArea.Controls.Add(containerObj);
                                containerObj.Dock = DockStyle.Fill;
                                naviBar1.Bands.Add(band);
                                naviBar1.ActiveBand = band;
                                band.LargeImage = Properties.Resources.bookmark_big;
                            }
                        }
                        if (clientPannel != null)
                        {
                            NaviBand bandNavi = new NaviBand();
                            bandNavi.Text = Info.NaviPnl;
                            containerObjTemp = new ContainerObject();
                            bandNavi.Name = Info.NaviPnl;
                            bandNavi.Text = Info.NaviPnl;
                            GetNaviButtonsAll(containerObjTemp, 30, clientPannel,false);
                            bandNavi.ClientArea.Controls.Add(containerObjTemp);
                            containerObjTemp.Dock = DockStyle.Fill;
                            bandNavi.LargeImage = Properties.Resources.wizard_big;
                            naviBar1.Bands.Add(bandNavi);
                        }
                        naviBar1.ShowMoreOptionsButton = false;
                        naviBar1.VisibleLargeButtons = naviBar1.Bands.Count;
                        ((System.ComponentModel.ISupportInitialize)(naviBar1)).EndInit();
                        naviBar = naviBar1;

                    }
                    else
                    {
                        control.Dock = controlInfo.ContainerStyle;
                        control.Name = controlInfo.objectName;
                    }

                    if (control is DataGridViewW)
                    {
                        datagridviewName.Add(control.Name);
                        EmbededObject EmbededObject = new EmbededObject();
                        EmbededObject.ObjectOrient = controlInfo.DataGridViewOrient;
                        EmbededObject.ObjectPosition = controlInfo.DataGridViewPosition;
                        EmbededObject.IdentifierName = controlInfo.DataGridViewName;
                        control.Tag = EmbededObject;
                        control.AccessibleName = controlInfo.DataGridViewName.Trim();
                        DataGridViewW dataView = (DataGridViewW)control;
                        dataView.ReadOnly = true;
                        wholeDataView.Add(dataView);
                    }


                    if (control is ContainerObject)
                    {
                        ContainerObject controlObj = control as ContainerObject;
                        if (controlInfo.controlContainer != null && controlInfo.controlContainer.Count > 0)
                            ControlIncludedChildControl(controlInfo.controlContainer, controlObj);
                    }
                    else if (control is TabControlW || control is TabControl)
                    {
                        if (control is TabControlW)
                        {
                            TabControlW tabControl = control as TabControlW;
                            if (controlInfo.controlContainer != null && controlInfo.controlContainer.Count > 0)
                            {
                                foreach (ControlInformation childInfo in controlInfo.controlContainer)
                                {
                                    TabPage newPage = new TabPage();
                                    newPage.Name = childInfo.ContainerName;
                                    newPage.Text = childInfo.ContainerLabel;
                                    ContainerObject containerObj = new ContainerObject();
                                    containerObj.Dock = DockStyle.Fill;
                                    newPage.Controls.Add(containerObj);
                                    ControlIncludedChildControl(childInfo.controlContainer, containerObj);
                                    tabControl.TabPages.Add(newPage);
                                    AddCtrlToList(newPage);
                                }
                            }
                        }
                        else
                        {
                            TabControl tabControl = control as TabControl;
                            if (controlInfo.controlContainer != null && controlInfo.controlContainer.Count > 0)
                            {
                                foreach (ControlInformation childInfo in controlInfo.controlContainer)
                                {
                                    TabPage newPage = new TabPage();
                                    newPage.Name = childInfo.ContainerName;
                                    newPage.Text = childInfo.ContainerLabel;
                                    ContainerObject containerObj = new ContainerObject();
                                    containerObj.Dock = DockStyle.Fill;
                                    newPage.Controls.Add(containerObj);
                                    ControlIncludedChildControl(childInfo.controlContainer, containerObj);
                                    tabControl.TabPages.Add(newPage);
                                    AddCtrlToList(newPage);
                                }
                            }
                        }
                    }
                    tempContainer.Controls.Add(control);
                    AddCtrlToList(control);
                }
              
            }
        }

        //private static int i = 0;

        //private const int naviInternal = 30;

        public static void GetNaviButtonsAll(ContainerObject control, int naviInternal, ContainerObject tabControl,bool flag)
        {
            List<NaviItem> NaviItems = WorkCurveHelper.NaviItems;
            int origianlPosition = 20;
            if (NaviItems != null && NaviItems.Count > 0)
            {
                foreach (NaviItem naviItem in NaviItems)
                {
                    if (naviItem.ShowInMain)
                    {
                        if (naviItem.Enabled)
                        {
                            naviItem.InitParam();
                            //naviItem.Btn.Location = new Point(10 + control.Size.Width / 3, origianlPosition + 10);
                            naviItem.Btn.Location = new Point((220 - naviItem.Btn.Width) / 2, origianlPosition);
                            //naviItem.Btn.KeepPress = true;
                            origianlPosition += naviInternal + 70;
                            //control.Controls.Add(naviItem.Btn);


                            naviItem.Lbl.Location = new Point(0, naviItem.Btn.Location.Y + naviItem.Btn.Height + 5);

                            control.Controls.Add(naviItem.Btn);
                            control.Controls.Add(naviItem.Lbl);
                            if (!flag)
                            AddTabPage(tabControl, naviItem);
                        }
                    }
                }

                foreach (NaviItem naviItem in NaviItems)
                {
                    if (naviItem.ShowInMain)
                    {
                        if (!naviItem.Enabled)
                        {
                            naviItem.InitParam();
                            //naviItem.Btn.KeepPress = true;
                            naviItem.Btn.Location = new Point((220 - naviItem.Btn.Width) / 2, origianlPosition);
                            origianlPosition += naviInternal + 70;

                            naviItem.Lbl.Location = new Point(0, naviItem.Btn.Location.Y + naviItem.Btn.Height + 5);
                            //control.Controls.Add(naviItem.Btn);
                            control.Controls.Add(naviItem.Btn);
                            control.Controls.Add(naviItem.Lbl);
                            if (!flag)
                            AddTabPage(tabControl, naviItem);
                        }
                    }
                }
            }
        }

        private static NaviItem preNaviItem;

        /// <summary>
        /// 对指定的按钮注册点击事件，在主客户区的TabCont)rol中进行切换
        /// </summary>
        /// <param name="tabCtrl"></param>
        /// <param name="btn"></param>
        public static void AddTabPage(ContainerObject containerObj, NaviItem naviItem)
        {
            if (containerObj == null || naviItem == null)
                return;
            naviItem.Btn.Click += delegate(object sender, EventArgs e)
            {
                naviItem.Btn.ToFocused = true;
                naviItem.Btn.Invalidate();
                //naviItem.Btn.IsPressed = true;
                if (preNaviItem != null && preNaviItem.Name != naviItem.Name)
                {
                    //preNaviItem.Btn.IsPressed = false;
                    preNaviItem.Btn.ToFocused = false;
                    preNaviItem.Btn.Refresh();
                }
                preNaviItem = naviItem;

                if (naviItem.ShowInMain)//在主窗体显示，填充控件
                {
                    bool existsFlag = false;
                    List<Control> listContr = new List<Control>();
                    foreach (Control control in containerObj.Controls)
                    {
                        if (control.Name == naviItem.Name && control.Name == "MainPage")
                        {
                            control.Visible = true;
                            existsFlag = true;
                        }
                        else
                        {
                            control.Visible = false;
                            if (control.Name != "MainPage")
                            {
                                containerObj.Controls.Remove(control);
                            }
                        }
                    }
                    if (!existsFlag)
                    {
                        if (naviItem != null && naviItem.TT != null)
                        {
                            naviItem.UserControl = naviItem.TT();
                            var typ = naviItem.UserControl.GetType();
                            if (typ.FullName == "Skyray.Print.UCPrint")
                            {
                                var propertyInfo = typ.GetProperty("ShowPropertyPanel");
                                if (propertyInfo != null) propertyInfo.SetValue(naviItem.UserControl, false, null);
                                propertyInfo = typ.GetProperty("ShowSourcePanel");
                                if (propertyInfo != null) propertyInfo.SetValue(naviItem.UserControl, false, null);
                                propertyInfo = typ.GetProperty("ShowStatusBar");
                                if (propertyInfo != null) propertyInfo.SetValue(naviItem.UserControl, false, null);
                                propertyInfo = typ.GetProperty("ShowMenuBar");
                                if (propertyInfo != null) propertyInfo.SetValue(naviItem.UserControl, false, null);
                                propertyInfo = typ.GetProperty("ShowToolBar");
                                if (propertyInfo != null) propertyInfo.SetValue(naviItem.UserControl, false, null);
                            }
                        }
                        ContainerObject tempObject = new ContainerObject();
                        tempObject.Name = naviItem.Name;
                        tempObject.Dock = DockStyle.Fill;
                        naviItem.UserControl.Dock = DockStyle.Fill;
                        if (naviItem.UserControl is Form)
                            (naviItem.UserControl as Form).TopLevel = false;//设置窗体为非顶级控件
                        tempObject.Controls.Add(naviItem.UserControl);
                        containerObj.Controls.Add(tempObject);
                    }
                }
                else
                {
                    if (naviItem.UserControl == null)
                        return;
                    if (naviItem.UserControl.GetType() == typeof(Form))
                    {
                        (naviItem.UserControl as Form).ShowDialog();
                    }
                    else
                    {
                        Form form = new Form();
                        form.Text = naviItem.Name;
                        form.Size = naviItem.UserControl.Size;
                        form.MaximizeBox = false;
                        form.MinimizeBox = false;
                        form.StartPosition = FormStartPosition.CenterScreen;
                        form.FormBorderStyle = FormBorderStyle.FixedSingle;
                        form.Controls.Add(naviItem.UserControl);
                        form.ShowDialog();
                    }
                }
            };
        }


        public static void Save(SaveSerialize contain,string fileName)
        {
            IFormatter formatter = new BinaryFormatter();
            if (File.Exists(fileName))
                File.Delete(fileName);
            Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, contain);
            stream.Close();
        }

        public static void SaveObj(object contain, string fileName)
        {
            IFormatter formatter = new BinaryFormatter();
            if (File.Exists(fileName))
                File.Delete(fileName);
            Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, contain);
            stream.Close();
        }

        private static void AddCtrlToList(object obj)
        {
            if (!DyObjs.Contains(obj))
            {
                DyObjs.Add(obj);
                //if (obj is DataGridView) DGVs.Add(obj as DataGridView);
            }
        }


          /// <summary>
        /// 反序列化获取分布对象
        /// </summary>
        /// <returns></returns>
        public static SaveSerialize GetPassedUserDataInitialization(string fileName)
        {
            IFormatter formatter = new BinaryFormatter();
            if (File.Exists(fileName))
            {
                using (FileStream _FileStream = new System.IO.FileStream(fileName,
                    System.IO.FileMode.Open,
                    System.IO.FileAccess.Read,
                    System.IO.FileShare.None
                    ))
                {
                    _FileStream.Position = 0;
                    _FileStream.Seek(0, SeekOrigin.Begin);
                    return (SaveSerialize)formatter.Deserialize(_FileStream);
                }

            }
            else
                return null;
        }

       
    }
}
