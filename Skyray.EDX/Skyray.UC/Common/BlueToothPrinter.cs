using System;
using System.Collections.Generic;
using System.Linq;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System.Net.Sockets;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Threading;
using System.Globalization;
using Microsoft.Win32;
using System.Drawing.Imaging;
using System.Drawing;

namespace Skyray.EDX.Common.Component
{
    public class BlueToothPrinter 
    {


        public BlueToothPrinter()
        {
           
        }

        #region Methods
        public void Write(string str)
        {
            byte[] array = GBKEncoder.ToBytes(str);
            Write(array);
        }
        public void Write(byte[] array)
        {
            _printStream.Write(array, 0, array.Length);
        }

        /// <summary>
        /// mode == 1  => Vert.dots==8  1:3
        /// mode == 33 => Vert.dots==24 1:1
        /// </summary>
        public unsafe void Write(Bitmap bmp, byte mode)
        {
            byte[] LINESP_CMD = { 0x1b, 0x33, 24 };                //行间距指令
            byte[] HEADER_CMD = { 0x1b, 0x2a, mode, 0x00, 0x00 };  //打印位图指令
            byte[] ENTER_CMD = { 0x0a };                           //进纸换行指令
            // 打印机一次能打印的点阵面积是(HorzDots x VerDots) => 称为单位位图
            // 默认HorzDots < 纸张宽度，否则要作缩放
            int HorzDots = bmp.Width;
            int VertDots = mode == 1 ? 8 : 24;
            int iBmpH = bmp.Height;
            int iBmpNum = (iBmpH - 1) / VertDots + 1; // 需拼接的位图数

            //设置打印位图指令头，宽度参数；
            HEADER_CMD[3] = (byte)HorzDots;        // 低8位
            HEADER_CMD[4] = (byte)(HorzDots >> 8); // 高8位

            int rectSend = HEADER_CMD.Length + HorzDots * (VertDots / 8) + ENTER_CMD.Length;
            byte[] rect = new byte[rectSend];

            // 发送的总字节数
            int totalSend = LINESP_CMD.Length + (rectSend * iBmpNum);
            byte[] total = new byte[totalSend];

            // 行间距指令
            Buffer.BlockCopy(LINESP_CMD, 0, total, 0, LINESP_CMD.Length);
            int ptr = LINESP_CMD.Length; // 行字节游标，此时指向位图矩阵首位

            int rectPos = 0;
            BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            int stride = bmData.Stride;
            IntPtr Scan0 = bmData.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte red, green, blue, gray;
                int y = 0; // Y方向像素绝对坐标
                byte* l;

                for (int n = 0; n < iBmpNum; ++n)
                {
                    Array.Clear(rect, 0, rect.Length);
                    rectPos = 0;
                    Buffer.BlockCopy(HEADER_CMD, 0, rect, rectPos, HEADER_CMD.Length);
                    rectPos += HEADER_CMD.Length;

                    // 第n幅 (bmp.Width x VertDots) bitmap
                    for (int x = 0; x < HorzDots; ++x)
                    {
                        for (int k = 0; k < VertDots; ++k)
                        {
                            y = n * VertDots + k;
                            if (y < iBmpH)
                            {
                                l = p + stride * y + 3 * x;
                                blue = *l;
                                green = *(l + 1);
                                red = *(l + 2);
                                gray = (byte)(.299 * red + .587 * green + .114 * blue);
                                if (gray < 128)
                                {
                                    if (mode == 1)
                                    {
                                        rect[rectPos] |= (byte)(1 << (7 - k));
                                    }
                                    else if (mode == 33)
                                    {
                                        if (k < 8)
                                        {
                                            rect[rectPos] |= (byte)(1 << (7 - k));
                                        }
                                        else if (k < 16)
                                        {
                                            rect[rectPos + 1] |= (byte)(1 << (15 - k));
                                        }
                                        else
                                        {
                                            rect[rectPos + 2] |= (byte)(1 << (23 - k));
                                        }
                                    }
                                }
                            }
                        }
                        if (mode == 1)
                        {
                            ++rectPos;
                        }
                        else
                        {
                            rectPos += 3;
                        }
                    }
                    Buffer.BlockCopy(ENTER_CMD, 0, rect, rectPos, ENTER_CMD.Length);
                    Buffer.BlockCopy(rect, 0, total, ptr, rectSend);
                    ptr += rectSend;
                }
            }
            bmp.UnlockBits(bmData);
            Write(total);
        }
        public void Write(Bitmap bmp)
        {
            byte[] LINESP_CMD = { 0x1b, 0x33, 0x00 };              //行间距指令
            byte[] HEADER_CMD = { 0x1b, 0x2a, 0x01, 0x00, 0x00 };  //打印位图指令
            byte[] ENTER_CMD = { 0x0d, 0xa };                      //回车换行指令

            int iBmpW = bmp.Width;
            int iBmpH = bmp.Height;
            int iBmpNum = (iBmpH - 1) / 8 + 1; // 需拼接的位图数

            //设置打印位图指令头，宽度参数；
            HEADER_CMD[3] = (byte)iBmpW;// 低8位
            HEADER_CMD[4] = (byte)(iBmpW >> 8); // 高8位

            int iBmpDataLen = HEADER_CMD.Length + iBmpW + ENTER_CMD.Length; // 单位位图的宽度
            int lpCount = LINESP_CMD.Length + (iBmpDataLen * iBmpNum);      // 所有数据长度
            byte[] lpData = new byte[lpCount];
            int ptr = 0;
            Buffer.BlockCopy(LINESP_CMD, 0, lpData, 0, LINESP_CMD.Length);
            ptr += LINESP_CMD.Length;

            byte[] lpTemp = new byte[iBmpDataLen];
            int lpTempIndex = 0;

            BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            int stride = bmData.Stride;
            IntPtr Scan0 = bmData.Scan0;
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                int nOffset = stride - iBmpW * 3;
                byte red, green, blue, gray;
                int y = 0;
                byte* l;
                for (int i = 0; i < iBmpNum; ++i)
                {
                    // 打印位图指令
                    lpTempIndex = 0;
                    Array.Clear(lpTemp, 0, lpTemp.Length);
                    Array.Copy(HEADER_CMD, 0, lpTemp, lpTempIndex, HEADER_CMD.Length);
                    lpTempIndex += HEADER_CMD.Length;

                    // 第i幅 (iBmpW x 8) bitmap
                    for (int x = 0; x < iBmpW; ++x)
                    {
                        for (int k = 0; k < 8; ++k)
                        {
                            y = i * 8 + k;
                            if (y >= iBmpH)
                                continue;

                            l = p + stride * y + 3 * x;
                            blue = *l;
                            green = *(l + 1);
                            red = *(l + 2);

                            gray = (byte)(.299 * red + .587 * green + .114 * blue);
                            if (gray < 128)
                            {
                                lpTemp[lpTempIndex] |= (byte)(1 << (7 - k));
                            }
                        }
                        ++lpTempIndex;
                    }
                    // 回车换行
                    Buffer.BlockCopy(ENTER_CMD, 0, lpTemp, lpTempIndex, ENTER_CMD.Length);
                    lpTempIndex += ENTER_CMD.Length;

                    Buffer.BlockCopy(lpTemp, 0, lpData, ptr, iBmpDataLen);
                    ptr += iBmpDataLen;
                }
            }
            bmp.UnlockBits(bmData);
            Write(lpData);
        }

        public BluetoothDeviceInfo[] ScanDevices()
        {
            if (_bluetoothCli == null)
            {
                _bluetoothCli = new BluetoothClient();
            }
            BluetoothDeviceInfo[] BthDevices = _bluetoothCli.DiscoverDevices();
            return BthDevices;
        }

        /// <summary>
        ///  异步搜索蓝牙设备
        /// </summary>
        /// <param name="maxDevices">设备数量上限</param>
        public void BeginDiscoverDevices(int maxDevices)
        {
            //if (_bluetoothCli == null)
            //{
            //    _bluetoothCli = new BluetoothClient();
            //}

            //_bluetoothCli.DiscoverDevices();
           // _bluetoothCli.BeginDiscoverDevices(maxDevices, true, true, true, false, new AsyncCallback(DiscoverCompletedHandler), null);
          
        }
        /// <summary>
        /// 异步搜索结果回调
        /// </summary>
        private void DiscoverCompletedHandler(IAsyncResult ar)
        {
            //try
            //{
                
            //    _bluetoothDevs = _bluetoothCli.EndDiscoverDevices(ar);
            //    //.Where(d => d.DeviceName.StartsWith(DEV_PREFIX)).ToArray<BluetoothDeviceInfo>();
            //    if (DeviceFound != null)
            //        DeviceFound(_bluetoothDevs);
            //}
            //finally
            //{
            
            //}
        }



        /// <summary>
        /// 选择打印机设备
        /// </summary>
        /// <param name="o"></param>
        public void SelectDevice(object o)
        {
          //  _bluetoothPrinter = o as BluetoothDeviceInfo;
            _bluetoothPrinter = o as BluetoothAddress;         
        }


        /// <summary>
        /// 本机是否有蓝牙
        /// </summary>
        /// <returns></returns>
        public bool IsExsitPrint()
        {
            bool ret = false;
            _bluetoothRadio = BluetoothRadio.PrimaryRadio;
            if (_bluetoothRadio != null)
                ret = true;

            return ret;
        }         

      

        /// <summary>
        /// 与打印机配对, 并返回流
        /// </summary>
        /// <param name="printStream">WriteStream</param>
        public bool Connect()
        {
            if (IsAlive)
                return true;
            bool ret = false;

            if (_bluetoothPrinter == null)
            {
                //string address = GetPossibleAddress();
                //if (address != null)
                //{
                //    _bluetoothPrinter = new BluetoothDeviceInfo(new BluetoothAddress(
                //         long.Parse(address, NumberStyles.HexNumber, CultureInfo.CurrentCulture.NumberFormat)));
                //}
                //else
                if (WorkCurveHelper.PrintName != null || WorkCurveHelper.PrintName == "" || WorkCurveHelper.PrintName.Equals(string.Empty))
                {
                    //_bluetoothPrinter = new BluetoothDeviceInfo(new BluetoothAddress(long.Parse(long.Parse(WorkCurveHelper.PrintName, NumberStyles.HexNumber, CultureInfo.CurrentCulture.NumberFormat))));

                    try
                    {
                        _bluetoothPrinter = new BluetoothAddress(
                            long.Parse(WorkCurveHelper.PrintName, NumberStyles.HexNumber, CultureInfo.CurrentCulture.NumberFormat));
                    }
                    catch(Exception ce)
                    {
                        Msg.Show(ce.Message);
                        return false;
                    }
                }
                else
                {
                    return ret;
                }

            }

            if (_bluetoothCli != null)
                _bluetoothCli.Dispose();

          

            // 1. Pair it if not
            if (!IsPaired)
                BluetoothSecurity.PairRequest(_bluetoothPrinter, "0000");
            

            // 2. Get the write stream
            _bluetoothCli = new BluetoothClient();
            try
            {
                _bluetoothCli.Connect(_bluetoothPrinter, BluetoothService.SerialPort);
                if (_printStream != null)
                    _printStream.Dispose();
                _printStream = _bluetoothCli.GetStream();
                ret = true;
                IsPaired = true;
               
            }
            catch
            {
                if (_printStream != null)
                {
                    _printStream.Dispose();
                    _printStream = null;
                }
                ret = false;
                IsPaired = false;
            }
            finally
            {
              
            }
            IsAlive = ret;
            return ret;
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            IsAlive = false;
            if (_printStream != null)
            {
                _printStream.Close();
                _printStream.Dispose();
                _printStream = null;
            }
            if (_bluetoothCli != null)
            {
                _bluetoothCli.Close();
                _bluetoothCli.Dispose();
                _bluetoothCli = null;
            }
        }

        /// <summary>
        /// 删除所有配对设备
        /// </summary>
        private void ClearPairing()
        {
            RegistryKey rk = Registry.LocalMachine.OpenSubKey("\\SOFTWARE\\Microsoft\\Bluetooth\\Device", true);

            string[] subKeyNames = rk.GetSubKeyNames();
            string target = "name";
            RegistryKey rkk = null;
            string[] names = null;
            foreach (string sk in subKeyNames)
            {
                rkk = rk.OpenSubKey(sk);
                names = rkk.GetValueNames();
                rkk.Close();
                if (names.Contains(target))
                {
                    rk.DeleteSubKeyTree(sk);
                }
            }
            rk.Close();
        }
        /// <summary>
        /// 从历史记录尝试获取打印机地址
        /// </summary>
        private string GetPossibleAddress()
        {
            string ret = null;
            RegistryKey rk = Registry.LocalMachine.OpenSubKey("\\SOFTWARE\\Microsoft\\Bluetooth\\Device", false);
            string[] subKeyNames = rk.GetSubKeyNames();
            const string target = "HDT312A";
            RegistryKey rkk = null;
            string name = null;
            foreach (string sk in subKeyNames)
            {
                rkk = rk.OpenSubKey(sk);
                try
                {
                    name = rkk.GetValue("name", null).ToString();
                }
                catch { }
                finally
                {
                    rkk.Close();
                }
                if (name == target)
                {
                    ret = sk;
                    break;
                }
            }
            rk.Close();
            return ret;
        }

            
        /// <summary>
        /// 写入注册表信息 第一次蓝牙连接时，写入注册表
        /// </summary>
        private void WriteRegistry()
        {
            RegistryKey rk =null;
            if (Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft").SubKeyCount <= 0)
            {
                Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft");
            }
            rk = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft", true);
            rk.SetValue("name", "HDT312A"); 
        }

      
        #endregion

        #region Properties
        /// <summary>
        /// 与打印机连接是否活跃
        /// </summary>
        public bool IsAlive { get; set; }
        /// <summary>
        /// 是否已经与打印机配对
        /// </summary>
        public bool IsPaired
        {
         get;set;
            //get
            //{
            //    bool ret = false;
            //    RegistryKey rk = Registry.LocalMachine.OpenSubKey("\\SOFTWARE\\Microsoft\\Bluetooth\\Device", false);
            //    string[] subKeyNames = rk.GetSubKeyNames();
            //    string target = "name";
            //    object o = null;
            //    RegistryKey rkk = null;
            //    foreach (string sk in subKeyNames)
            //    {
            //        rkk = rk.OpenSubKey(sk);
            //        o = rkk.GetValue(target);
            //        rkk.Close();
            //        if (o != null /* && o.ToString().StartsWith(DEV_PREFIX)*/)
            //        {
            //            ret = true;
            //            break;
            //        }
            //    }
            //    rk.Close();
            //    return ret;
            //}
        }

        /// <summary>
        /// 是否为芝柯设备
        /// </summary>
        public bool IsZiCox
        {
            get
            {
                bool ret = false;
                if (this.Name != null && (this.Name.Contains("XT") || this.Name.Contains("HT")))
                    ret = true;
                return ret;
            }
        }
        /// <summary>
        /// 选中打印机
        /// </summary>
        public bool IsSelectBlue { get; set; }
        /// <summary>
        /// 打印机名
        /// </summary>
        public string Name
        {
            get
            {
                if (_bluetoothPrinter != null)
                    return _bluetoothPrinter.ToString();
                else
                    return string.Empty;
            }
        }
        #endregion

        #region Variables
        private BluetoothClient _bluetoothCli;
        private NetworkStream _printStream;
        private BluetoothDeviceInfo[] _bluetoothDevs;
        private BluetoothDeviceInfo blueDeviceInfo;
        private BluetoothAddress _bluetoothPrinter;
        private BluetoothRadio _bluetoothRadio;
        #endregion

        #region Commands
        /// <summary>
        /// 清空缓冲区数据，复位所有参数
        /// </summary>
        public static readonly byte[] ESC = new byte[] { 0x1B, 0x40, 0x00 };
        /// <summary>
        /// 打印缓冲区数据，并换行
        /// </summary>
        public static readonly byte[] LF = new byte[] { 0x0A };
        /// <summary>
        /// 打印缓冲区数据，并回车
        /// </summary>
        public static readonly byte[] CR = new byte[] { 0x0D };
        /// <summary>
        /// 调用字库
        /// </summary>
        public static readonly byte[] CHARS = new byte[] { 0x1B, 0x38 };
        #endregion
        #region Events
        public delegate void DeviceFoundEventHandler(BluetoothDeviceInfo[] devices);
        public event DeviceFoundEventHandler DeviceFound;
        #endregion
  
    }
}
