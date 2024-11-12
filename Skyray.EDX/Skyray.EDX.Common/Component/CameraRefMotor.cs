using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Skyray.EDXRFLibrary;

namespace Skyray.EDX.Common
{
    /// <summary>
    /// 电机类
    /// </summary>
    public class CameraRefMotor
    {
        #region 常量

        private const int dueTime = 10;    // 监控间隔时间0.1 秒
        /// <summary>
        /// 启动步长
        /// </summary>
        public const int DOWNSTART_XYZ_AXES_MOTOR_SPEED_GEAR = 50;
        /// <summary>
        /// XYZ电机最大步长
        /// </summary>
        public const int MAX_XYZ_AXES_MOTOR_STEP = 16777215;//0x00FFFFFF
        //public const int MAX_XYZ_AXES_MOTOR_STEP = 9000000;
        /// <summary>
        /// 步进电机最大步长
        /// </summary>
        public const int MAX_QUANTA_MOTOR_STEP = 8000;
        /// <summary>
        /// Z电机最大速度
        /// </summary>
        public const int MAX_Z_AXES_MOTOR_SPEED_GEAR = 10;
        /// <summary>
        /// 自动高度调节速度
        /// </summary>
        public const int AUTO_ADJUST_HEIGHT_SPEED_GEAR = MAX_Z_AXES_MOTOR_SPEED_GEAR + 80;
        /// <summary>
        /// XY电机最大速度
        /// </summary>
        public const int MAX_XY_AXES_MOTOR_SPEED_GEAR = 80;
        /// <summary>
        /// XYZ电机最小速度
        /// </summary>
        public const int MIN_XYZ_AXES_MOTOR_SPEED_GEAR = 150;


        //public const int FIXED_STEP_CONSTANT = 20;
        /// <summary>
        /// 步进电机最小速度
        /// </summary>
        public const int MIN_QUANTA_MOTOR_SPEED_GEAR = 150;
        /// <summary>
        /// 间隔时间
        /// </summary>
        public const int RATE_SAMPLING_INTERVAL_TIME = 10;
        /// <summary>
        /// 间隔步长
        /// </summary>
        public const int RATE_SAMPLING_INTERVAL_STEP = 50;
        /// <summary>
        /// 取样管压
        /// </summary>
        public const int RATE_SAMPLING_TUBE_VOLTAGE = 40;
        /// <summary>
        /// 取样管流
        /// </summary>
        public const int RATE_SAMPLING_TUBE_CURRENT = 200;
        /// <summary>
        /// 取样粗调码
        /// </summary>
        public const int RATE_SAMPLING_COARSE_CODE = 60;
        /// <summary>
        /// 取样细调码
        /// </summary>
        public const int RATE_SAMPLING_FINE_CODE = 120;
        /// <summary>
        /// 每步实际长度
        /// </summary>
        public const double RATE_SAMPLING_ONE_STEP_LENGTH = 0.0025;
        /// <summary>
        /// 热点区域半径
        /// </summary>
        public const int RATE_SAMPLING_HOT_AREA_RIDUS = 5;

        #endregion


        #region 字段

        protected static Boolean s_Locked;
        protected static Boolean _bCancelAll;    // 取消所有电机移动

        protected MotorDirections _lastDirection;
        protected Port port;      // 接口
        private int speedGear;  // 电机速度齿轮
        protected Boolean _bCancel;    // 取消当前电机移动

        /// <summary>
        /// 最大的范围
        /// </summary>
        public int MaxStep;

        /// <summary>
        /// 对应的UI控制句柄
        /// </summary>
        public IntPtr Handle;

        /// <summary>
        /// 编号
        /// </summary>
        public int ID;
        
        /// <summary>
        /// 电机的名称
        /// </summary>
        public MotorName Name;


        /// <summary>
        /// 正方向移动标志，1,或者0
        /// </summary>
        public int DirectionFlag;

        public  Int32 RepositionStep;


        /// <summary>
        /// 是否存在
        /// </summary>
        public bool Exist;

        #endregion


        #region 委托和事件

        public delegate void MotorMoveBeginEventHandler();

        public static event MotorMoveBeginEventHandler MotorMoveBeginEvent;

        public delegate void MotorMoveEndEventHandler();

        public static event MotorMoveEndEventHandler MotorMoveEndEvent;

        public delegate void MotorNoMoveEventHandle();

        //public static event MotorNoMoveEventHandle OnMotorNoMoveEventHandle;
        public delegate void ChangeStateEventHandler();

        public static event ChangeStateEventHandler ChangeStateEvent;
        #endregion


        #region 构造器

        protected CameraRefMotor()
        {
            InitMotor();
        }


        public void InitMotor()
        {
            port = WorkCurveHelper.deviceMeasure.interfacce.port;
            speedGear = MIN_XYZ_AXES_MOTOR_SPEED_GEAR;
            MaxStep = MAX_XYZ_AXES_MOTOR_STEP;
            Exist = true;
            _lastDirection = MotorDirections.Positive;
            //_bCancelAll = false;
            //_bCancel = false;
        }

        static CameraRefMotor()
        {
            //_bCancelAll = false;
            s_Locked = false;
        }

        #endregion


        #region 属性

        /// <summary>
        /// 电机移动速度
        /// </summary>
        public int Speed
        {
            set
            {
                if (value > MIN_XYZ_AXES_MOTOR_SPEED_GEAR)
                {
                    speedGear = MIN_XYZ_AXES_MOTOR_SPEED_GEAR;
                }
                else if (value < MAX_Z_AXES_MOTOR_SPEED_GEAR)
                {
                    speedGear = MAX_Z_AXES_MOTOR_SPEED_GEAR;
                }
                else
                {
                    speedGear = value;
                }
            }
            get
            {
                return speedGear;
            }
        }

        /// <summary>
        /// 用于判断是否空闲
        /// </summary>
        public virtual Boolean IsIdel
        {
            get
            {
                return (port!=null && port.MotorIsIdel(ID));
            }
        }

        /// <summary>
        /// 用于判断是否到达触碰开关
        /// </summary>
        public Boolean IsAtTouch
        {
            get
            {
                Int32 info = 0;
                WorkCurveHelper.deviceMeasure.interfacce.port.GetMotorInfo(ref info);
                Int32 flag;
                Int32 id = ID;
                switch (id)
                {
                    case 0:
                    case 1:
                        flag = id * 2 + 1;
                        return 0 == (info << (31 - flag)) >> 31;
                    case 2:
                        flag = 9;
                        break;
                    case 3:
                        flag = 12;
                        break;
                    case 4:
                        flag = 5;
                        break;
                    default:
                        flag = -1;
                        return false;
                }
                return (info << (31 - flag) >> 31) != 0 || (info << (31 - flag) >> 31) != 0;
            }
        }
        #endregion


        #region 方法


        #region 操作安全的移动
        public Thread thr = null;
        public bool IsExist = false;
        
        //------自动识别
        public static bool IsTransLocked = false;   //是否传送影像
        public static bool IsReceiveLocked = false;  //接收影像后对应是否有接收状态反馈  
        public static bool IsShowDefine = false;
        public static double ShowCurrentDefin;

        public MotorDirections CurrentDir;
        private bool flag = false;

        /// <summary>
        /// 操作安全的移动（不断的循环发送移动命令，每次只移动一小段，这样能保证USB通讯中断后电机仍能停止）
        /// </summary>
        /// <param name="dir">用于设置方向</param>
        protected void MoveSafely()
        {
            _bCancelAll = false;
            _bCancel = false;
            flag = true;
            UsbPort.UsbIdel = true;
            if (thr == null)
            {
                thr = new Thread(new ParameterizedThreadStart(DoMoveSafely));
                thr.IsBackground = true;
                thr.Start();
            }
            else
                flag = true;
        }
        #endregion

        #region 常规移动
        public static object ooobj = new object();

        /// <summary>
        /// 操作安全的移动
        /// </summary>
        protected void DoMoveSafely(Object state)
        {
            while (true)
            {
                if (!flag)
                    continue;
                if (MotorMoveBeginEvent != null)
                Thread.Sleep(200);
                if (flag&&IsIdel)
                {
                    if (MotorMoveBeginEvent != null)
                    {
                        MotorMoveBeginEvent();
                    }
                    while (!_bCancelAll && !_bCancel)
                    {
                        MoveDirectly(this.CurrentDir, 6000);
                        Thread.Sleep(200);
                    }
                    // 保证电机停止
                    Stop((int)this.CurrentDir);
                    flag = false;
                    if (MotorMoveEndEvent != null)
                    {
                        MotorMoveEndEvent();
                    }
                }
            }
        }

        /// <summary>
        /// 操作安全的移动（不断的循环发送移动命令，每次只移动一小段，这样能保证USB通讯中断后电机仍能停止）
        /// </summary>
        /// <param name="dir">用于设置方向</param>
        protected void MoveSafely(MotorDirections dir)
        {
            _bCancelAll = false;
            if (thr != null && thr.IsAlive)
                thr.Abort();
            thr = new Thread(new ParameterizedThreadStart(DoMoveSafely));
            thr.IsBackground = true;
            thr.Start(dir);
        }

        #endregion


        #region 常规移动
        // 指定了方向和步数的移动
        protected void DoMoveSync(MotorDirections dir, Int32 step)
        {
            if (IsIdel)
            {
                if (MotorMoveBeginEvent != null)
                {
                    MotorMoveBeginEvent();
                }
                MoveDirectly(_lastDirection = dir, step);
                 // 循环检查状态位，电机停止，循环也跟着停止
                while (!IsIdel)
                {
                    if (_bCancelAll || _bCancel)
                    {
                        Stop((int)dir);
                        break;
                    }
                }
                Thread.Sleep(100);
                if (MotorMoveEndEvent != null)
                {
                    MotorMoveEndEvent();
                }
            }
                  
        }

        // 指定了方向、步数和速度的移动
        protected void DoMoveSync(MotorDirections dir, Int32 step, Int32 speed)
        {
            this.Speed = speed;
            DoMoveSync(dir, step);
        }

        // 移动，用于线程参数
        private void DoMoveSync(Object state)
        {
            Int32 step = (Int32)state;
            DoMoveSync(_lastDirection, step);
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="step"></param>
        protected void MoveSync(MotorDirections dir, Int32 step)
        {
            _bCancelAll = false;
            if (thr != null && thr.IsAlive)
                thr.Abort();
            thr = new Thread(new ParameterizedThreadStart(DoMoveSync));
            thr.IsBackground = true;
            thr.Start(step);
        }

        /// <summary>
        /// 指定了方向和步长的移动。只简单的发送移动命令，不做任何线程处理。
        /// </summary>
        /// <param name="dir">方向</param>
        /// <param name="step">步长</param>
        public void MoveDirectly(MotorDirections dir, Int32 step)
        {
            port.MotorControl(ID, (Int32)dir, step, true, this.Speed);
        }

        #endregion


        /// <summary>
        /// 停止移动
        /// </summary>
        public void Stop(int dir)
        {
            port.MotorControl(ID, dir, 0, true, this.Speed);
        }

        public void Stop()
        {
            port.MotorControl(ID, 1, 0, true, this.Speed);
            Thread.Sleep(10);
            port.MotorControl(ID, 0, 0, true, this.Speed);
        }


        public void Cancel()
        {
            _bCancel = true;
        }

        public void MoveAutoZMotor()
        {
            port.MoveZAutoMotor(this.Speed);
        }

        /// <summary>
        /// 取消移动
        /// </summary>
        public static void CancelAll()
        {
            _bCancelAll = true;
            
        }
        

        #endregion


        //#region 同步互斥

        //// 等待解锁，等待时间默认为5分钟，等待间隔默认为100毫秒
        //protected static Boolean WaitUnlock()
        //{
        //    return WaitUnlock(300);
        //}

        //// 等待解锁，只限定等待时间，等待间隔默认为100毫秒
        //protected static Boolean WaitUnlock(Int32 waitTime)
        //{
        //    return WaitUnlock(waitTime, 100);
        //}

        //// 等待解锁。
        //// total - 等待总时间，单位为秒； interval - 等待循环的间隔，单位为毫秒。
        //// 返回true表示解锁成功，否则表示解锁取消或超时，这时将自动解锁
        //protected static Boolean WaitUnlock(Int32 waitTime, Int32 interval)
        //{
        //    Int32 elapse = 0;
        //    waitTime = waitTime * 1000; // 将等待总时间的单位换成毫秒
        //    while (s_Locked)
        //    {
        //        if (_bCancelAll || elapse >= waitTime)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            Thread.Sleep(interval);
        //            elapse += interval;
        //        }
        //    }
        //    return true;
        //}

        //// 锁
        //protected static void Lock()
        //{
        //    s_Locked = true;
        //}

        //// 解锁
        //protected static void Unlock()
        //{
        //    s_Locked = false;
        //}

        //#endregion


        #region 复位

        //private void DoReposition(Object state)
        //{
        //    Int32 step = RepositionStep = (Int32)state;
        //    lock (ooobj)
        //    {
        //        if (IsIdel)
        //        {
        //            DoMoveSync(MotorDirections.Positive, MAX_XYZ_AXES_MOTOR_STEP, MAX_XY_AXES_MOTOR_SPEED_GEAR);
        //            DoMoveSync(MotorDirections.Negative, step, MAX_XY_AXES_MOTOR_SPEED_GEAR);
        //        }
        //    }
        //}


        private void DoReposition(Object state)
        {
            Int32 step = RepositionStep = (Int32)state;
            lock (ooobj)
            {
                if (IsIdel)
                {
                    DoMoveSync(MotorDirections.Positive, MAX_XYZ_AXES_MOTOR_STEP, MAX_XY_AXES_MOTOR_SPEED_GEAR);
                    DoMoveSync(MotorDirections.Negative, step, MAX_XY_AXES_MOTOR_SPEED_GEAR);
                   
                }
            }
        }

    

        public void Reposition(Int32 step)
        {
            if (thr != null && thr.IsAlive)
                thr.Abort();
            thr = new Thread(new ParameterizedThreadStart(DoReposition));
            thr.IsBackground = true;
            thr.Start(step);
        }

        #endregion

    }

    /// <summary>
    /// 电机移动方向
    /// </summary>
    public enum MotorDirections
    {
        /// <summary>
        /// 反向
        /// </summary>
        Negative = 0,
        /// <summary>
        /// 正向
        /// </summary>
        Positive = 1,
    }
}
