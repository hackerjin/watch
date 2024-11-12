using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Skyray.EDX.Common
{
    public class UsbPortDelegate
    {
        /// <summary>
        /// 打开设备
        /// </summary>
        /// <returns></returns>
        public delegate int bOpenDevice();

        /// <summary>
        /// 关闭设备
        /// </summary>
        /// <returns></returns>
        public delegate int bCloseDevice();

        /// <summary>
        /// 获取设备版本号
        /// </summary>
        /// <returns></returns>
        public delegate int iGetDevVersion3();

        public delegate int bXRayTubeSel(int index);

        public delegate int iGetDevVersion();

        /// <summary>
        /// 设置X光管高压、管流值、放大倍数
        /// </summary>
        /// <param name="iVoltage"></param>
        /// <param name="iCurrent"></param>
        /// <param name="iGain"></param>
        /// <param name="iFineGain"></param>
        /// <returns></returns>
        public delegate int bSetDA(int iVoltage, int iCurrent, int iGain, int iFineGain);
        /// <summary>
        /// 回读X光管高压、管流、温度、真空度
        /// </summary>
        /// <param name="iVoltage"></param>
        /// <param name="iCurrent"></param>
        /// <param name="iTemp"></param>
        /// <param name="iVacuum"></param>
        /// <returns></returns>
        public delegate int bGetState3(ref int iVoltage, ref int iCurrent, ref int iTemp, ref int iVacuum);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iVoltage"></param>
        /// <param name="iCurrent"></param>
        /// <param name="iTemp"></param>
        /// <param name="iVacuum"></param>
        /// <param name="isCoverClose"></param>
        /// <returns></returns>
        public delegate int bGetState4(ref int iVoltage, ref int iCurrent, ref int iTemp, ref int iVacuum, ref int isCoverClose);

        public delegate int bEnableCoverSwitch(bool allowUncover);

        /// <summary>
        /// 开启高压
        /// </summary>
        /// <returns></returns>
        public delegate int bOpenHV();
        /// <summary>
        /// 关闭高压
        /// </summary>
        /// <returns></returns>
        public delegate int bCloseHV();
        /// <summary>
        /// 开启真空泵
        /// </summary>
        /// <returns></returns>
        public delegate int bOenPump();
        /// <summary>
        /// 关闭真空泵
        /// </summary>
        /// <returns></returns>
        public delegate int bClosePump();
        /// <summary>
        /// 点亮高压状态灯
        /// </summary>
        /// <returns></returns>
        public delegate int bOpenHVLamp();
        /// <summary>
        /// 关闭高压状态灯
        /// </summary>
        /// <returns></returns>
        public delegate int bCloseHVLamp();
        /// <summary>
        /// 重新开始1秒计数
        /// </summary>
        /// <returns></returns>
        public delegate int bReStart();

        /// <summary>
        /// 获取MCA数据
        /// </summary>
        /// <returns></returns>
        public unsafe delegate Byte* GetMCAData();
        /// <summary>
        /// 步进电机控制
        /// </summary>
        /// <param name="iIndex"></param>
        /// <param name="iDir"></param>
        /// <param name="iCycle"></param>
        /// <param name="iSwtStop"></param>
        /// <returns></returns>
        public delegate int bMotoControl(int iIndex, int iDir, int iCycle, int iSwtStop, int spwspeed);

        public delegate int bMotoControl3(int iIndex, int iDir, int iCycle, int iSwtStop);

        /// <summary>
        /// 获取电机及接近开关状态
        /// </summary>
        /// <param name="iMotorInfo"></param>
        /// <returns></returns>
        public delegate int bGetMotoInfo(ref int iMotorInfo);


        public delegate int GetKeyInfo(StringBuilder company, StringBuilder mode, StringBuilder serialNum, ref long LeftSencods);

       /// <summary>
       /// 获取样品腔状态
       /// </summary>
       /// <param name="status"></param>
       /// <param name="statusLen"></param>
       /// <returns></returns>
        public delegate bool bGetChamberInfos(byte[] status, ref int statusLen);

        /// <summary>
        /// 样品腔的复位
        /// </summary>
        /// <returns></returns>
        public delegate bool bResetChamber();

        /// <summary>
        /// 样品腔的复位
        /// </summary>
        /// <returns></returns>
        public delegate bool bLockHV(int iLock);
    }
}
