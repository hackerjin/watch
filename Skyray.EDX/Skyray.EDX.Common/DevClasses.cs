using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;

namespace Skyray.EDX.Common
{
    /// <summary>
    /// 下列所需函数可参考MSDN中与驱动程序相关的API函数
    /// </summary>
    public class Externs
    {
        public const int DIGCF_ALLCLASSES = (0x00000004);
        public const int DIGCF_PRESENT = (0x00000002);
        public const int INVALID_HANDLE_VALUE = -1;
        public const int SPDRP_DEVICEDESC = (0x00000000);
        public const int MAX_DEV_LEN = 1000;
        public const int DEVICE_NOTIFY_WINDOW_HANDLE = (0x00000000);
        public const int DEVICE_NOTIFY_SERVICE_HANDLE = (0x00000001);
        public const int DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = (0x00000004);
        public const int DBT_DEVTYP_DEVICEINTERFACE = (0x00000005);
        public const int DBT_DEVNODES_CHANGED = (0x0007);
        public const int WM_DEVICECHANGE = (0x0219);
        public const int DIF_PROPERTYCHANGE = (0x00000012);
        public const int DICS_FLAG_GLOBAL = (0x00000001);
        public const int DICS_FLAG_CONFIGSPECIFIC = (0x00000002);
        public const int DICS_ENABLE = (0x00000001);
        public const int DICS_DISABLE = (0x00000002);

        /// <summary>
        /// 注册设备或者设备类型，在指定的窗口返回相关的信息
        /// </summary>
        /// <param name="hRecipient"></param>
        /// <param name="NotificationFilter"></param>
        /// <param name="Flags"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, DEV_BROADCAST_DEVICEINTERFACE NotificationFilter, UInt32 Flags);

        /// <summary>
        /// 通过名柄，关闭指定设备的信息。
        /// </summary>
        /// <param name="hHandle"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint UnregisterDeviceNotification(IntPtr hHandle);

        /// <summary>
        /// 获取一个指定类别或全部类别的所有已安装设备的信息
        /// </summary>
        /// <param name="gClass"></param>
        /// <param name="iEnumerator"></param>
        /// <param name="hParent"></param>
        /// <param name="nFlags"></param>
        /// <returns></returns>
        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern IntPtr SetupDiGetClassDevs(ref Guid gClass, UInt32 iEnumerator, IntPtr hParent, UInt32 nFlags);

        /// <summary>
        /// 销毁一个设备信息集合，并且释放所有关联的内存
        /// </summary>
        /// <param name="lpInfoSet"></param>
        /// <returns></returns>
        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern int SetupDiDestroyDeviceInfoList(IntPtr lpInfoSet);

        /// <summary>
        /// 枚举指定设备信息集合的成员，并将数据放在SP_DEVINFO_DATA中
        /// </summary>
        /// <param name="lpInfoSet"></param>
        /// <param name="dwIndex"></param>
        /// <param name="devInfoData"></param>
        /// <returns></returns>
        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiEnumDeviceInfo(IntPtr lpInfoSet, UInt32 dwIndex, SP_DEVINFO_DATA devInfoData);

        /// <summary>
        /// 获取指定设备的属性
        /// </summary>
        /// <param name="lpInfoSet"></param>
        /// <param name="DeviceInfoData"></param>
        /// <param name="Property"></param>
        /// <param name="PropertyRegDataType"></param>
        /// <param name="PropertyBuffer"></param>
        /// <param name="PropertyBufferSize"></param>
        /// <param name="RequiredSize"></param>
        /// <returns></returns>
        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiGetDeviceRegistryProperty(IntPtr lpInfoSet, SP_DEVINFO_DATA DeviceInfoData, UInt32 Property, UInt32 PropertyRegDataType, StringBuilder PropertyBuffer, UInt32 PropertyBufferSize, IntPtr RequiredSize);

        /// <summary>
        /// 停用设备
        /// </summary>
        /// <param name="DeviceInfoSet"></param>
        /// <param name="DeviceInfoData"></param>
        /// <param name="ClassInstallParams"></param>
        /// <param name="ClassInstallParamsSize"></param>
        /// <returns></returns>
        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetupDiSetClassInstallParams(IntPtr DeviceInfoSet, IntPtr DeviceInfoData, IntPtr ClassInstallParams, int ClassInstallParamsSize);

        /// <summary>
        /// 启用设备
        /// </summary>
        /// <param name="InstallFunction"></param>
        /// <param name="DeviceInfoSet"></param>
        /// <param name="DeviceInfoData"></param>
        /// <returns></returns>
        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        public static extern Boolean SetupDiCallClassInstaller(UInt32 InstallFunction, IntPtr DeviceInfoSet, IntPtr DeviceInfoData);

        /// <summary>
        /// RegisterDeviceNotification所需参数
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct DEV_BROADCAST_HANDLE
        {
            public int dbch_size;
            public int dbch_devicetype;
            public int dbch_reserved;
            public IntPtr dbch_handle;
            public IntPtr dbch_hdevnotify;
            public Guid dbch_eventguid;
            public long dbch_nameoffset;
            public byte dbch_data;
            public byte dbch_data1;
        }

        // WM_DEVICECHANGE message
        [StructLayout(LayoutKind.Sequential)]
        public class DEV_BROADCAST_DEVICEINTERFACE
        {
            public int dbcc_size;
            public int dbcc_devicetype;
            public int dbcc_reserved;
        }

        /// <summary>
        /// 设备信息数据
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class SP_DEVINFO_DATA
        {
            public int cbSize;
            public Guid classGuid;
            public int devInst;
            public ulong reserved;
        };

        /// <summary>
        /// 属性变更参数
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class SP_PROPCHANGE_PARAMS
        {
            public SP_CLASSINSTALL_HEADER ClassInstallHeader = new SP_CLASSINSTALL_HEADER();
            public int StateChange;
            public int Scope;
            public int HwProfile;
        };

        /// <summary>
        /// 设备安装
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class SP_DEVINSTALL_PARAMS
        {
            public int cbSize;
            public int Flags;
            public int FlagsEx;
            public IntPtr hwndParent;
            public IntPtr InstallMsgHandler;
            public IntPtr InstallMsgHandlerContext;
            public IntPtr FileQueue;
            public IntPtr ClassInstallReserved;
            public int Reserved;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string DriverPath;
        };

        [StructLayout(LayoutKind.Sequential)]
        public class SP_CLASSINSTALL_HEADER
        {
            public int cbSize;
            public int InstallFunction;
        };
    }

    public class HardwareClass
    {
        #region 属性
        /// <summary>
        /// 返回所有硬件信息列表
        /// </summary>
        /// <returns></returns>
        public string[] List
        {

            get
            {
                List<string> HWList = new List<string>();
                try
                {
                    Guid myGUID = System.Guid.Empty;
                    IntPtr hDevInfo = Externs.SetupDiGetClassDevs(ref myGUID, 0, IntPtr.Zero, Externs.DIGCF_ALLCLASSES | Externs.DIGCF_PRESENT);
                    if (hDevInfo.ToInt32() == Externs.INVALID_HANDLE_VALUE)
                    {
                        throw new Exception("Invalid Handle");
                    }
                    Externs.SP_DEVINFO_DATA DeviceInfoData;
                    DeviceInfoData = new Externs.SP_DEVINFO_DATA();
                    DeviceInfoData.cbSize = 28;
                    DeviceInfoData.devInst = 0;
                    DeviceInfoData.classGuid = System.Guid.Empty;
                    DeviceInfoData.reserved = 0;
                    UInt32 i;
                    StringBuilder DeviceName = new StringBuilder("");
                    DeviceName.Capacity = Externs.MAX_DEV_LEN;
                    for (i = 0; Externs.SetupDiEnumDeviceInfo(hDevInfo, i, DeviceInfoData); i++)
                    {
                        if (!Externs.SetupDiGetDeviceRegistryProperty(hDevInfo, DeviceInfoData, Externs.SPDRP_DEVICEDESC, 0, DeviceName, Externs.MAX_DEV_LEN, IntPtr.Zero))
                        {
                            continue;
                        }
                        HWList.Add(DeviceName.ToString());
                    }
                    Externs.SetupDiDestroyDeviceInfoList(hDevInfo);
                }
                catch (Exception ex)
                {
                    throw new Exception("枚举设备列表出错", ex);
                }
                return HWList.ToArray();
            }
        }

        #endregion

        #region 公共事件
        /// <summary>
        /// 清理非托管资源
        /// </summary>
        /// <param name="callback"></param>
        public void Dispose(IntPtr callback)
        {
            try
            {
                Externs.UnregisterDeviceNotification(callback);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 设置指定设备的状态
        /// </summary>
        /// <param name="match">设备名称</param>
        /// <param name="bEnable">是否启用</param>
        /// <returns></returns>
        public bool SetState(string[] match, bool bEnable)
        {
            try
            {
                Guid myGUID = System.Guid.Empty;
                IntPtr hDevInfo = Externs.SetupDiGetClassDevs(ref myGUID, 0, IntPtr.Zero, Externs.DIGCF_ALLCLASSES | Externs.DIGCF_PRESENT);
                if (hDevInfo.ToInt32() == Externs.INVALID_HANDLE_VALUE)
                {
                    return false;
                }
                Externs.SP_DEVINFO_DATA DeviceInfoData;
                DeviceInfoData = new Externs.SP_DEVINFO_DATA();
                DeviceInfoData.cbSize = 28;
                DeviceInfoData.devInst = 0;
                DeviceInfoData.classGuid = System.Guid.Empty;
                DeviceInfoData.reserved = 0;
                UInt32 i;
                StringBuilder DeviceName = new StringBuilder("");
                DeviceName.Capacity = Externs.MAX_DEV_LEN;
                for (i = 0; Externs.SetupDiEnumDeviceInfo(hDevInfo, i, DeviceInfoData); i++)
                {
                    while (!Externs.SetupDiGetDeviceRegistryProperty(hDevInfo, DeviceInfoData, Externs.SPDRP_DEVICEDESC, 0, DeviceName, Externs.MAX_DEV_LEN, IntPtr.Zero))
                    {
                    }
                    bool bMatch = true;
                    foreach (string search in match)
                    {
                        if (!DeviceName.ToString().ToLower().Contains(search.ToLower()))
                        {
                            bMatch = false;
                            break;
                        }
                    }
                    if (bMatch)
                    {
                        OpenClose(hDevInfo, DeviceInfoData, bEnable);
                    }
                }
                Externs.SetupDiDestroyDeviceInfoList(hDevInfo);
            }
            catch (Exception ex)
            {
                throw new Exception("枚举设备信息出错！", ex);
                //return false;
            }
            return true;
        }

        /// <summary>
        /// 允许一个窗口或者服务接收所有硬件的通知
        /// 注:目前还没有找到一个比较好的方法来处理如果通知服务。
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="UseWindowHandle"></param>
        /// <returns></returns>
        public bool AllowNotifications(IntPtr callback, bool UseWindowHandle)
        {
            try
            {
                Externs.DEV_BROADCAST_DEVICEINTERFACE dbdi = new Externs.DEV_BROADCAST_DEVICEINTERFACE();
                dbdi.dbcc_size = Marshal.SizeOf(dbdi);
                dbdi.dbcc_reserved = 0;
                dbdi.dbcc_devicetype = Externs.DBT_DEVTYP_DEVICEINTERFACE;
                if (UseWindowHandle)
                    Externs.RegisterDeviceNotification(callback, dbdi, Externs.DEVICE_NOTIFY_ALL_INTERFACE_CLASSES | Externs.DEVICE_NOTIFY_WINDOW_HANDLE);
                else
                    Externs.RegisterDeviceNotification(callback, dbdi, Externs.DEVICE_NOTIFY_ALL_INTERFACE_CLASSES | Externs.DEVICE_NOTIFY_SERVICE_HANDLE);
                return true;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return false;
            }
        }

        #endregion

        #region 私有事件
        /// <summary>
        /// 开启或者关闭指定的设备驱动
        /// 注意：该方法目前没有检查是否需要重新启动计算机。^.^
        /// </summary>
        /// <param name="hDevInfo"></param>
        /// <param name="devInfoData"></param>
        /// <param name="bEnable"></param>
        /// <returns></returns>
        private bool OpenClose(IntPtr hDevInfo, Externs.SP_DEVINFO_DATA devInfoData, bool bEnable)
        {
            try
            {
                int szOfPcp;
                IntPtr ptrToPcp;
                int szDevInfoData;
                IntPtr ptrToDevInfoData;
                Externs.SP_PROPCHANGE_PARAMS SP_PROPCHANGE_PARAMS1 = new Externs.SP_PROPCHANGE_PARAMS();
                if (bEnable)
                {
                    SP_PROPCHANGE_PARAMS1.ClassInstallHeader.cbSize = Marshal.SizeOf(typeof(Externs.SP_CLASSINSTALL_HEADER));
                    SP_PROPCHANGE_PARAMS1.ClassInstallHeader.InstallFunction = Externs.DIF_PROPERTYCHANGE;
                    SP_PROPCHANGE_PARAMS1.StateChange = Externs.DICS_ENABLE;
                    SP_PROPCHANGE_PARAMS1.Scope = Externs.DICS_FLAG_GLOBAL;
                    SP_PROPCHANGE_PARAMS1.HwProfile = 0;

                    szOfPcp = Marshal.SizeOf(SP_PROPCHANGE_PARAMS1);
                    ptrToPcp = Marshal.AllocHGlobal(szOfPcp);
                    Marshal.StructureToPtr(SP_PROPCHANGE_PARAMS1, ptrToPcp, true);
                    szDevInfoData = Marshal.SizeOf(devInfoData);
                    ptrToDevInfoData = Marshal.AllocHGlobal(szDevInfoData);

                    if (Externs.SetupDiSetClassInstallParams(hDevInfo, ptrToDevInfoData, ptrToPcp, Marshal.SizeOf(typeof(Externs.SP_PROPCHANGE_PARAMS))))
                    {
                        Externs.SetupDiCallClassInstaller(Externs.DIF_PROPERTYCHANGE, hDevInfo, ptrToDevInfoData);
                    }
                    SP_PROPCHANGE_PARAMS1.ClassInstallHeader.cbSize = Marshal.SizeOf(typeof(Externs.SP_CLASSINSTALL_HEADER));
                    SP_PROPCHANGE_PARAMS1.ClassInstallHeader.InstallFunction = Externs.DIF_PROPERTYCHANGE;
                    SP_PROPCHANGE_PARAMS1.StateChange = Externs.DICS_ENABLE;
                    SP_PROPCHANGE_PARAMS1.Scope = Externs.DICS_FLAG_CONFIGSPECIFIC;
                    SP_PROPCHANGE_PARAMS1.HwProfile = 0;
                }
                else
                {
                    SP_PROPCHANGE_PARAMS1.ClassInstallHeader.cbSize = Marshal.SizeOf(typeof(Externs.SP_CLASSINSTALL_HEADER));
                    SP_PROPCHANGE_PARAMS1.ClassInstallHeader.InstallFunction = Externs.DIF_PROPERTYCHANGE;
                    SP_PROPCHANGE_PARAMS1.StateChange = Externs.DICS_DISABLE;
                    SP_PROPCHANGE_PARAMS1.Scope = Externs.DICS_FLAG_CONFIGSPECIFIC;
                    SP_PROPCHANGE_PARAMS1.HwProfile = 0;
                }
                szOfPcp = Marshal.SizeOf(SP_PROPCHANGE_PARAMS1);
                ptrToPcp = Marshal.AllocHGlobal(szOfPcp);
                Marshal.StructureToPtr(SP_PROPCHANGE_PARAMS1, ptrToPcp, true);
                szDevInfoData = Marshal.SizeOf(devInfoData);
                ptrToDevInfoData = Marshal.AllocHGlobal(szDevInfoData);
                Marshal.StructureToPtr(devInfoData, ptrToDevInfoData, true);

                bool rslt1 = Externs.SetupDiSetClassInstallParams(hDevInfo, ptrToDevInfoData, ptrToPcp, Marshal.SizeOf(typeof(Externs.SP_PROPCHANGE_PARAMS)));
                bool rstl2 = Externs.SetupDiCallClassInstaller(Externs.DIF_PROPERTYCHANGE, hDevInfo, ptrToDevInfoData);
                if ((!rslt1) || (!rstl2))
                {
                    //throw new Exception("不能更改设备状态。");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch 
            {
                return false;
            }
        }

        #endregion
    }

}
