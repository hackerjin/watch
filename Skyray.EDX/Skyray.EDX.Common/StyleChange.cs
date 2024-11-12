using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using System.Windows.Forms;
using System.Runtime.Remoting;
using Skyray.Controls;
using Skyray.Controls.NavigationBar;

using Skyray.Controls.Extension;

namespace Skyray.EDX.Common
{
    public class StyleChange
    {
        public static string configureFileName = "test1.txt";

        /// <summary>
        /// 反序列化对象，并由对象产生ContainObject集合
        /// </summary>
        /// <param name="userDataInitialization"></param>
        public static List<ContainerObject> DeserializationObject(ContainObjectInformation[] userDataInformation, List<DataGridViewW> listDgv)
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
                ContainerObject tempContainer = objectT as ContainerObject;
                tempContainer.InitContainerObject(containInfo);
                if (containInfo.ContainControls != null && containInfo.ContainControls.Count > 0)
                    ControlIncludedChildControlFromConfig(containInfo.ContainControls, tempContainer, listDgv);
                listContainer.Add(tempContainer);
            }
            return listContainer;
        }

        /// <summary>
        /// 对面板中包含的控件进行反序列化为对象
        /// </summary>
        /// <param name="containInfo"></param>
        /// <param name="tempContainer"></param>
        private static void ControlIncludedChildControlFromConfig(List<ControlInformation> containInfo, ContainerObject tempContainer, List<DataGridViewW> listDgv)
        {
            if (containInfo == null || tempContainer == null)
                return;
            foreach (ControlInformation controlInfo in containInfo)
            {
                Type type = Type.GetType(controlInfo.ContainerObjectType); // 获取类型
                object objectControl = Activator.CreateInstance(type); // 创建类型的实例，即控件
                System.Windows.Forms.Control control = null;
                if (objectControl != null)
                {
                    control = objectControl as System.Windows.Forms.Control;
                   
                    if (control is XRFChart)
                    {
                        control = libSerialize.xrfChart;
                    }
                    if (control is NaviBar)
                    {
                        NaviBar naviBar1 = control as NaviBar;
                        ((System.ComponentModel.ISupportInitialize)(naviBar1)).BeginInit();
                        control = objectControl as System.Windows.Forms.Control;
                        control.Dock = controlInfo.ContainerStyle;
                        control.Name = controlInfo.objectName;
                        if (controlInfo.controlContainer != null && controlInfo.controlContainer.Count > 0)
                        {
                            foreach (ControlInformation childInfo in controlInfo.controlContainer)
                            {
                                NaviBand band = new NaviBand();
                                ContainerObject containerObj = new ContainerObject();
                                band.Name = childInfo.ContainerName;
                                band.Text = childInfo.ContainerLabel;
                                ControlIncludedChildControlFromConfig(childInfo.controlContainer, containerObj, listDgv);
                                band.ClientArea.Controls.Add(containerObj);
                                containerObj.Dock = DockStyle.Fill;
                                naviBar1.Bands.Add(band);
                            }
                        }
                        ((System.ComponentModel.ISupportInitialize)(naviBar1)).EndInit();
                    }
                    else
                    {
                        control.Dock = controlInfo.ContainerStyle;
                        control.Name = controlInfo.objectName;
                    }

                    if (control is DataGridViewW)
                    {
                        control.AccessibleName = controlInfo.DataGridViewName;
                        if (listDgv != null && listDgv.Count > 0)
                        {
                            DataGridViewW findData = listDgv.Find(w=>w.AccessibleName == controlInfo.DataGridViewName.Trim());
                            if (findData != null)
                                control = findData;
                        }
                    }

                    if (control is ContainerObject)
                    {
                        ContainerObject controlObj = control as ContainerObject;
                        if (controlInfo.controlContainer != null && controlInfo.controlContainer.Count > 0)
                            ControlIncludedChildControlFromConfig(controlInfo.controlContainer, controlObj, listDgv);
                    }
                    else if (control is TabControl)
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
                                ControlIncludedChildControlFromConfig(childInfo.controlContainer, containerObj, listDgv);
                                newPage.Controls.Add(containerObj);
                                containerObj.Dock = DockStyle.Fill;
                                tabControl.TabPages.Add(newPage);
                            }
                        }
                    }
                    tempContainer.Controls.Add(control);
                }
            }
        }
    }
}
