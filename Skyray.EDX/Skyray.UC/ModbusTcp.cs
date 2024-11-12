using System;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Modbus.Data;
using Modbus.Device;
using Modbus.Utility;
using Skyray.EDX.Common;
using Skyray.Language;
using System.Windows.Forms;
namespace Skyray.UC
{
    public class ModbusTcp
    {

        public static byte slaveId = 1;
        public static int port = 502;
        public static IPAddress address = new IPAddress(new byte[] { 127, 0, 0, 1 });
        public static TcpListener slaveTcpListener = new TcpListener(address, port);
        public static ModbusSlave slave = ModbusTcpSlave.CreateTcp(slaveId, slaveTcpListener);
        public static byte FC = 0;
        public static  ModbusSlaveRequestEventArgs requestArg = null;
        
        

        public static ushort[] Bytes2Ushorts(byte[] src)
        {
            int len = src.Length;
            byte[] srcPlus = new byte[len + 1];
            src.CopyTo(srcPlus, 0);
            int count = len >> 1;

            if (len % 2 != 0)
            {
                count += 1;
            }

            ushort[] dest = new ushort[count];


            for (int i = 0; i < count; i++)
            {
                dest[i] = (ushort)(srcPlus[i * 2] & 0xff | srcPlus[2 * i + 1] << 8);
            }


            return dest;
        }





        public static void SetReal(ushort[] src, int start, float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            ushort[] dest = Bytes2Ushorts(bytes);

            ushort temp = dest[0];
            dest[0] = dest[1];
            dest[1] = temp;
            dest.CopyTo(src, start);
        }


        public static void slave_ModbusSlaveRequestReceived(object sender, ModbusSlaveRequestEventArgs arg)
        {
         
            FC =  arg.Message.FunctionCode;
            
        }

        public delegate void btStart();
        public delegate void btStop();
        public static void DataStore_DataStoreWrittenTo(object sender, DataStoreEventArgs arg)
        {
            if ( FC != 5)
                return;       
            FC = 0;
            switch (arg.StartAddress)
            {
                case 0:
                    {
                        if (slave.DataStore.CoilDiscretes[1])
                        {

                            WorkCurveHelper.startDoing++;
                            UCCameraControl temp = (UCCameraControl)(WorkCurveHelper.ucCameraControl1);
                            temp.BeginInvoke(new btStart( temp.buttonWStart_Modbus),null);
                        }
                            
                        break;
                    };

                case 2:
                    {

                        if (slave.DataStore.CoilDiscretes[3])
                        {
                            WorkCurveHelper.stopDoing++;
                            
                           
                            UCCameraControl temp = (UCCameraControl)(WorkCurveHelper.ucCameraControl1);
                            temp.BeginInvoke(new btStop(temp.buttonWStop_Modbus), null);
                            
                        }

                        break;

                    };

                case 3:
                    {

                        if (slave.DataStore.CoilDiscretes[4])
                        {
                            WorkCurveHelper.initDoing++;
                            WorkCurveHelper.dataStore.InputDiscretes[2] = true;
                            new System.Threading.Thread(new System.Threading.ThreadStart(DifferenceDevice.uc.ExcuteInitializationOperation_Modbus)).Start();
                            Thread.Sleep(5000);
                            slave.DataStore.CoilDiscretes[4] = true;
                        }
                        break;

                    };

            }



        }

        public static void Main()
        {
            slave.ModbusSlaveRequestReceived += new EventHandler<ModbusSlaveRequestEventArgs>(slave_ModbusSlaveRequestReceived);
            slave.DataStore.DataStoreWrittenTo += new EventHandler<DataStoreEventArgs>(DataStore_DataStoreWrittenTo);
            slaveTcpListener.Start();
            slave.Listen();

            Thread.Sleep(Timeout.Infinite);


        }



    }
}
