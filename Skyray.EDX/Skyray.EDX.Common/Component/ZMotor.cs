using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Skyray.EDXRFLibrary;

namespace Skyray.EDX.Common.Component
{
    /// <summary>
    /// Z电机
    /// </summary>
    public class ZMotor : CameraRefMotor
    {
        /// <summary>
        /// Z电机高度状态
        /// </summary>
        public enum ZHeightStates
        {
            /// <summary>
            /// 高于焦点
            /// </summary>
            Hyper,
            /// <summary>
            /// 低于焦点
            /// </summary>
            Under,
            /// <summary>
            /// 位于焦点
            /// </summary>
            At
        }

        //private  ZMotor s_singleInstance;

        private Boolean m_bIsHeightLazerOpened;

        /// <summary>
        /// 激光是否可用。只读属性。
        /// </summary>
        public Boolean IsLazerEnable
        {
            get
            {
                return this.Exist && !new VacuumPump(WorkCurveHelper.deviceMeasure.interfacce.port).Exist;
            }
        }

        // 私有默认构造器，实现单实例
        public ZMotor(DeviceMeasure device)
            : base()
        {
            InitZMotor(device);
        }

        public void InitZMotor(DeviceMeasure device)
        {
           
                ID = WorkCurveHelper.DeviceCurrent.MotorZCode;
                DirectionFlag = WorkCurveHelper.DeviceCurrent.MotorZDirect;
                MaxStep = WorkCurveHelper.DeviceCurrent.MotorZMaxStep;
                this.Speed = WorkCurveHelper.DeviceCurrent.MotorZSpeed;
        }

        ///// <summary>
        ///// 类构造器
        ///// </summary>
        //static ZMotor(Device device)
        //{
        //    s_singleInstance = new ZMotor(device);
        //}

        /// <summary>
        /// 取得Z电机实例
        /// </summary>
        /// <returns>Z电机实例</returns>
        //public  ZMotor CreateInstance(Device device)
        //{
        //    if (s_singleInstance == null)
        //    {
        //        s_singleInstance = new ZMotor(device);
        //    }
        //    else
        //    {
        //        s_singleInstance.InitMotor();
        //        InitZMotor(device);
        //    }
        //    return s_singleInstance;
        //}

        /// <summary>
        /// 打开高度激光
        /// </summary>
        public void OpenHeightLazer()
        {
            if (IsLazerEnable)
            {
                //m_bIsHeightLazerOpened = port.ClosePump();//usb--->xnet和usb
                 port.OpenHeightLaser(true);
                 m_bIsHeightLazerOpened = true;

            }
        }

        /// <summary>
        /// 关闭高度激光
        /// </summary>
        public void CloseHeightLazer()
        {
            if (IsLazerEnable)
            {
                //m_bIsHeightLazerOpened = !port.OpenPump();//usb--->xnet和usb
                port.OpenHeightLaser(false);
                m_bIsHeightLazerOpened = true;
            }
        }

        // 获取高度状态
        public ZHeightStates GetHeightState()
        {
            ZHeightStates state;
            int info = 0;
            port.GetMotorInfo(ref info);
            info = info >> 14;
            if (info == 1 || info == 2)
            {
                state = ZHeightStates.At;
            }
            else if (info == 3)   // 高于焦点
            {
                state = ZHeightStates.Hyper;
            }
            else if (info == 0)   // 低于焦点
            {
                state = ZHeightStates.Under;
            }
            else
            {
                throw new Exception("Cann't identify Z motor height info.");
            }
            return state;
        }

        // 向上移动
        protected void DoMoveUp(Object state)
        {
            Int32 step = (Int32)state;
            lock (ooobj)
            {
                if (DirectionFlag == 0)
                  DoMoveSync(MotorDirections.Negative, step);
                else
                  DoMoveSync(MotorDirections.Positive, step);
            }
        }

        // 向下移动
        protected void DoMoveDown(Object state)
        {
            Int32 step = (Int32)state;
            lock (ooobj)
            {
                if (DirectionFlag == 0)
                    DoMoveSync(MotorDirections.Positive, step);
                else
                    DoMoveSync(MotorDirections.Negative, step);
            }
        }

        //protected void DoMoveDownSafely()
        //{
        //    DoMoveSafely(MotorDirections.Positive);
        //}

        #region 公共方法
      
        /// <summary>
        /// 按上次移动的速度向上移动指定步长
        /// </summary>
        /// <param name="step">移动步长</param>
        public void MoveUp(Int32 step)
        {
            _bCancelAll = _bCancel = false;
             if (thr != null && thr.IsAlive)
                thr.Abort();
            thr = new Thread(new ParameterizedThreadStart(DoMoveUp));
            thr.IsBackground = true;
            thr.Start(step);
        }

        /// <summary>
        /// 按上次移动的速度向下移动指定步长
        /// </summary>
        /// <param name="step">移动步长</param>
        public void MoveDown(Int32 step)
        {
            _bCancelAll = _bCancel = false;
            if (thr != null && thr.IsAlive)
                thr.Abort();
            thr = new Thread(new ParameterizedThreadStart(DoMoveDown));
            thr.IsBackground = true;
            thr.Start(step);
        }

        /// <summary>
        /// 按指定的速度向上移动指定步长
        /// </summary>
        /// <param name="step">移动步长</param>
        /// <param name="speed">移动速度</param>
        public void MoveUp(Int32 step, Int32 speed)
        {
            this.Speed = speed;
            MoveUp(step);
        }

        /// <summary>
        /// 按指定的速度向下移动指定步长
        /// </summary>
        /// <param name="step">移动步长</param>
        /// <param name="speed">移动速度</param>
        public void MoveDown(Int32 step, Int32 speed)
        {
            this.Speed = speed;
            MoveDown(step);
        }

        /// <summary>
        /// 安全移动
        /// </summary>
        public void MoveDownSafely()
        {
            if (DirectionFlag == 0)
               CurrentDir =MotorDirections.Positive;
            else
               CurrentDir=MotorDirections.Negative;
            base.MoveSafely();
        }

        public void MoveUpSafely()
        {
            if (DirectionFlag == 0)
                CurrentDir = MotorDirections.Negative;
            else
                CurrentDir = MotorDirections.Positive;
            base.MoveSafely();
        }

        public void MoveUpSafely(Int32 speed)
        {
            _bCancel = false;
            this.Speed = speed;
            MoveUpSafely();
        }

        /// <summary>
        /// 安全移动
        /// </summary>
        /// <param name="speed">移动速度</param>
        public void MoveDownSafely(Int32 speed)
        {
            _bCancel = false;
            this.Speed = speed;
            MoveDownSafely();
        }

        #endregion
    }
}
