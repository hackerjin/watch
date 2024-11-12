using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Skyray.EDXRFLibrary;
using System.Runtime.InteropServices;
using System.Drawing;

using System.Diagnostics;

namespace Skyray.EDX.Common.Component
{
    /// <summary>
    /// 移动平台电机
    /// </summary>
    public class MotorAdvance : CameraRefMotor
    {
        #region 委托和事件

        /// <summary>
        /// 聚焦开始
        /// </summary>
        public delegate void FocusPointBeginEventHandler();
        public static event FocusPointBeginEventHandler FocusPointBeginEvent;

        public delegate void FocusPointEndEventHandler();
        public static event FocusPointEndEventHandler FocusPointEndEvent;

        public delegate void FocusReplaceEndEventHandler();
        public static event FocusReplaceEndEventHandler FocusReplaceEndEvent;

        //自动聚焦停止  
        public delegate void AutoFocusEndEventHandler();
        public static event AutoFocusEndEventHandler AutoFocusEndEndEvent;


        #endregion

        private static Device _device;

        public static Device Device
        {
            get { return _device; }
            set
            {
                if (value != null)
                {
                    MotorInstance instance = new MotorInstance();
                    WorkCurveHelper.DeviceCurrent = value;
                    instance.CreateInstance(MoterType.XMotor);
                    instance.CreateInstance(MoterType.YMotor);
                    instance.CreateInstance(MoterType.ZMotor);
                    s_xMotor = MotorInstance.s_singleInstanceX;
                    s_yMotor = MotorInstance.s_singleInstanceY;
                    s_zMotor = MotorInstance.s_singleInstanceZ;
                    _device = value;
                }
            }
        }


        public static Bitmap CameraBitmap;
        #region 类字段

        private static XMotor s_xMotor;
        private static YMotor s_yMotor;
        private static ZMotor s_zMotor;

        #endregion

        public static XMotor xMotor
        {

            get
            {
                return s_xMotor;

            }

        }

        #region 构造器

        /// <summary>
        /// 默认构造器
        /// </summary>
        static MotorAdvance()
        {
        }

        #endregion


        #region 聚焦水平XY平台上的某点

        /// <summary>
        /// 聚焦参数
        /// </summary>
        public class FocusParams
        {
            public MotorDirections DirX;
            public Int32 StepX;
            public Int32 SpeedX;
            public MotorDirections DirY;
            public Int32 StepY;
            public Int32 SpeedY;
            public MotorDirections DirZ;
            public Int32 StepZ;
            public Int32 SpeedZ;


            public Boolean IsFocus;
            public Boolean NotFocusMeasure;
            public Boolean IsFlipDirection;


            public FocusParams(MotorDirections dirX, Int32 stepX, Int32 speedX, MotorDirections dirY, Int32 stepY, Int32 speedY, Int32 speedZ, Boolean isFocus, Boolean notFocusMeasure)
            {
                this.DirX = dirX;
                this.StepX = stepX;
                this.SpeedX = speedX;

                this.DirY = dirY;
                this.StepY = stepY;
                this.SpeedY = speedY;

                this.SpeedZ = speedZ;
                IsFocus = isFocus;
                NotFocusMeasure = notFocusMeasure;
            }

            public FocusParams(MotorDirections dirY, Int32 stepY, Int32 speedY, MotorDirections dirZ, Int32 stepZ, Int32 speedZ)
            {

                this.DirY = dirY;
                this.StepY = stepY;
                this.SpeedY = speedY;

                this.DirZ = dirZ;
                this.StepZ = stepZ;
                this.SpeedZ = speedZ;

            }


            public FocusParams(MotorDirections dirX, Int32 stepX, Int32 speedX, MotorDirections dirY, Int32 stepY, Int32 speedY, bool isFlipDirection)
            {
                this.DirX = dirX;
                this.StepX = stepX;
                this.SpeedX = speedX;

                this.DirY = dirY;
                this.StepY = stepY;
                this.SpeedY = speedY;
                this.IsFlipDirection = isFlipDirection;
            }
        }

        private static MotorDirections getRepDir(MotorDirections dir)
        {

            if (dir == MotorDirections.Positive)
                return MotorDirections.Negative;
            else
                return MotorDirections.Positive;
        }



        private static void DoFocusReplaceXY(FocusParams param)
        {
            if (param.IsFlipDirection)
            {
                ReplaceXYForThick(param);
            }
            else
            {
                ReplaceXYForEDXRF(param);
            }

        }

        private static void DoFocusReplaceYZ(FocusParams param)
        {
            s_zMotor.Speed = param.SpeedZ;

            MotorDirections repDir = getRepDir(param.DirZ);
            s_zMotor.MoveDirectly(repDir, 16777215);
            Thread.Sleep(200);
            repDir = getRepDir(param.DirY);
            s_yMotor.MoveDirectly(repDir, 16777215);
            do
            {
                Thread.Sleep(100);
                if (_bCancelAll)
                {
                    s_zMotor.Stop((int)repDir);
                    s_yMotor.Stop((int)repDir);
                    return;
                }
            }
            while (!s_zMotor.IsIdel && !s_yMotor.IsIdel);

        }


        private static void ReplaceXYForEDXRF(FocusParams param)
        {
            s_xMotor.Speed = param.SpeedX;
            MotorDirections repDir = getRepDir(param.DirX);
            s_xMotor.MoveDirectly(repDir, 25535);
            do
            {
                Thread.Sleep(100);
                if (_bCancelAll)
                {
                    s_xMotor.Stop((int)repDir);
                    return;
                }
            }
            while (!s_xMotor.IsIdel);
            s_xMotor.MoveDirectly(param.DirX, param.StepX);
            do
            {
                Thread.Sleep(100);
                if (_bCancelAll)
                {
                    s_xMotor.Stop((int)param.DirX);
                    return;
                }
            }
            while (!s_xMotor.IsIdel);

            s_yMotor.Speed = param.SpeedY;
            repDir = getRepDir(param.DirY);
            s_yMotor.MoveDirectly(repDir, 25535);
            do
            {
                Thread.Sleep(100);
                if (_bCancelAll)
                {
                    s_yMotor.Stop((int)repDir);
                    return;
                }
            }
            while (!s_yMotor.IsIdel);
            s_yMotor.MoveDirectly(param.DirY, param.StepY);
            do
            {
                Thread.Sleep(100);
                if (_bCancelAll)
                {
                    s_yMotor.Stop((int)param.DirY);
                    return;
                }
            }
            while (!s_yMotor.IsIdel);

            if (FocusReplaceEndEvent != null)
            {
                FocusReplaceEndEvent();
            }
        }

        private static void ReplaceXYForThick(FocusParams param)
        {

            s_xMotor.Speed = param.SpeedX;
            MotorDirections repDir = getRepDir(param.DirX);
            s_xMotor.MoveDirectly(repDir, 16777215); //
            Thread.Sleep(20);
            s_yMotor.Speed = param.SpeedY;
            repDir = getRepDir(param.DirY);
            s_yMotor.MoveDirectly(repDir, 16777215); //

            do
            {
                Thread.Sleep(100);
                if (_bCancelAll)
                {
                    s_xMotor.Stop((int)repDir);
                    s_yMotor.Stop((int)repDir);
                    return;
                }
            }
            while (!s_xMotor.IsIdel || !s_yMotor.IsIdel);

            Thread.Sleep(200);
            s_yMotor.Speed = param.SpeedY;
            s_yMotor.MoveDirectly(param.DirY, param.StepY);
            Thread.Sleep(300);
            s_xMotor.Speed = param.SpeedX;
            s_xMotor.MoveDirectly(param.DirX, param.StepX);
            do
            {
                Thread.Sleep(100);
                if (_bCancelAll)
                {
                    Console.WriteLine("motor stop!!!!!!!!!!!");
                    s_xMotor.Stop((int)param.DirX);
                    s_yMotor.Stop((int)param.DirY);
                    return;
                }
            }
            while (!s_xMotor.IsIdel || !s_yMotor.IsIdel);

            WorkCurveHelper.bMotorRestart = true;


            ////s_xMotor.Speed = param.SpeedX;
            ////MotorDirections repDir = getRepDir(param.DirX);
            ////s_xMotor.MoveDirectly(repDir, 16777215);
            ////do
            ////{
            ////    Thread.Sleep(100);
            ////    if (_bCancelAll)
            ////    {
            ////        s_xMotor.Stop((int)repDir);
            ////        return;
            ////    }
            ////}
            ////while (!s_xMotor.IsIdel);
            ////s_xMotor.MoveDirectly(param.DirX, param.StepX);
            ////do
            ////{
            ////    Thread.Sleep(100);
            ////    if (_bCancelAll)
            ////    {
            ////        s_xMotor.Stop((int)param.DirX);
            ////        return;
            ////    }
            ////}
            ////while (!s_xMotor.IsIdel);

            ////s_yMotor.Speed = param.SpeedY;
            ////repDir = getRepDir(param.DirY);
            ////s_yMotor.MoveDirectly(repDir, 16777215);
            ////do
            ////{
            ////    Thread.Sleep(100);
            ////    if (_bCancelAll)
            ////    {
            ////        s_yMotor.Stop((int)repDir);
            ////        return;
            ////    }
            ////}
            ////while (!s_yMotor.IsIdel);
            ////s_yMotor.MoveDirectly(param.DirY, param.StepY);
            ////do
            ////{
            ////    Thread.Sleep(100);
            ////    if (_bCancelAll)
            ////    {
            ////        s_yMotor.Stop((int)param.DirY);
            ////        return;
            ////    }
            ////}
            ////while (!s_yMotor.IsIdel);

            ////WorkCurveHelper.bMotorRestart = true;




        }

        private static void DoFocusPointXY(FocusParams param)
        {
            // 设置参数并移动相应电机
            s_xMotor.Speed = param.SpeedX;
            s_xMotor.MoveDirectly(param.DirX, param.StepX);
            do
            {
                Thread.Sleep(100);
                if (_bCancelAll)
                {
                    s_xMotor.Stop((int)param.DirX);
                    return;
                }
            }
            while (!s_xMotor.IsIdel);

            s_yMotor.Speed = param.SpeedY;
            s_yMotor.MoveDirectly(param.DirY, param.StepY);
            do
            {
                Thread.Sleep(100);
                if (_bCancelAll)
                {
                    s_yMotor.Stop((int)param.DirY);
                    return;
                }
            }
            while (!s_yMotor.IsIdel);
            if (FocusPointEndEvent != null)
            {
                FocusPointEndEvent();
            }
        }


        private static void DoFocusPoint(FocusParams param)
        {
            #region 分开移动
            ////if (param.IsFocus && FocusPointBeginEvent != null)
            ////{
            ////    FocusPointBeginEvent();
            ////}

            ////// 设置参数并移动相应电机
            ////s_xMotor.Speed = param.SpeedX;
            ////s_xMotor.MoveDirectly(param.DirX, param.StepX);
            ////do
            ////{
            ////    Thread.Sleep(100);
            ////    if (_bCancelAll)
            ////    {
            ////        s_xMotor.Stop((int)param.DirX);
            ////        return;
            ////    }
            ////}
            ////while (!s_xMotor.IsIdel);

            ////s_yMotor.Speed = param.SpeedY;
            ////s_yMotor.MoveDirectly(param.DirY, param.StepY);
            ////do
            ////{
            ////    Thread.Sleep(100);
            ////    if (_bCancelAll)
            ////    {
            ////        s_yMotor.Stop((int)param.DirY);
            ////        return;
            ////    }
            ////}
            ////while (!s_yMotor.IsIdel);

            ////if (param.IsFocus)
            ////{
            ////    if (!param.NotFocusMeasure)
            ////    {
            ////        s_zMotor.Speed = param.SpeedZ;
            ////        DoAutoTuneHeight(s_zMotor);
            ////    }
            ////    if (FocusPointEndEvent != null)
            ////    {
            ////        FocusPointEndEvent();
            ////    }
            ////}
            #endregion

            //同时移动
            if (param.IsFocus && FocusPointBeginEvent != null)
            {
                FocusPointBeginEvent();
            }

            if (param.StepX > param.StepY)
            {
                s_yMotor.Speed = param.SpeedY;
                s_yMotor.MoveDirectly(param.DirY, param.StepY);
                Thread.Sleep(10);
                // 设置参数并移动相应电机
                s_xMotor.Speed = param.SpeedX;
                s_xMotor.MoveDirectly(param.DirX, param.StepX);


            }
            else
            {
                // 设置参数并移动相应电机
                s_xMotor.Speed = param.SpeedX;
                s_xMotor.MoveDirectly(param.DirX, param.StepX);
                Thread.Sleep(10);
                s_yMotor.Speed = param.SpeedY;
                s_yMotor.MoveDirectly(param.DirY, param.StepY);
            }
            do
            {
                Thread.Sleep(100);
                if (_bCancelAll)
                {
                    s_xMotor.Stop((int)param.DirX);
                    s_yMotor.Stop((int)param.DirY);
                    return;
                }
            }
            while (!s_xMotor.IsIdel || !s_yMotor.IsIdel);

            if (param.IsFocus)
            {
                if (!param.NotFocusMeasure)
                {
                    s_zMotor.Speed = param.SpeedZ;
                    DoAutoTuneHeight(s_zMotor);
                }
                if (FocusPointEndEvent != null)
                {
                    FocusPointEndEvent();
                }
            }
        }

        // 聚焦
        private static void DoFocusPointSync(Object state)
        {
            lock (ooobj)
            {
                //if (s_xMotor.IsIdel &&
                //    s_yMotor.IsIdel &&
                //    s_zMotor.IsIdel
                //  )
                {
                    // 得到聚焦参数
                    FocusParams param = (FocusParams)state;
                    DoFocusPoint(param);
                }
            }
        }


        private static void DoFocusPointSyncXY(Object state)
        {
            lock (ooobj)
            {
                if (s_xMotor.IsIdel &&
                    s_yMotor.IsIdel
                  )
                {
                    // 得到聚焦参数
                    FocusParams param = (FocusParams)state;
                    DoFocusPointXY(param);
                }
            }
        }

        private static void DoFocusReplaceSyncXY(Object state)
        {
            lock (ooobj)
            {
                if (s_xMotor.IsIdel &&
                    s_yMotor.IsIdel
                  )
                {
                    // 得到聚焦参数
                    FocusParams param = (FocusParams)state;
                    DoFocusReplaceXY(param);
                }
            }
        }

        private static void DoFocusReplaceSyncYZ(Object state)
        {
            lock (ooobj)
            {
                if (s_zMotor.IsIdel &&
                    s_yMotor.IsIdel
                  )
                {
                    // 得到聚焦参数
                    FocusParams param = (FocusParams)state;
                    DoFocusReplaceYZ(param);
                }
            }
        }

        public static void FocusPointXY(FocusParams param)
        {
            _bCancelAll = false;
            Thread thr = new Thread(new ParameterizedThreadStart(DoFocusPointSyncXY));
            thr.Start(param);
        }


        public static void FocusReplaceXY(FocusParams param)
        {
            _bCancelAll = false;
            Thread thr = new Thread(new ParameterizedThreadStart(DoFocusReplaceSyncXY));
            thr.Start(param);
            //DoFocusReplaceSyncXY(param);
        }

        public static void FocusReplaceYZ(FocusParams param)
        {
            _bCancelAll = false;
            Thread thr = new Thread(new ParameterizedThreadStart(DoFocusReplaceSyncYZ));
            thr.Start(param);
            //DoFocusReplaceSyncXY(param);
        }


        /// <summary>
        /// 聚焦平台上的某点
        /// </summary>
        /// <param name="param"></param>
        public static void FocusPoint(FocusParams param)
        {
            _bCancelAll = false;
            Thread thr = new Thread(new ParameterizedThreadStart(DoFocusPointSync));
            thr.Start(param);
        }

        #endregion


        #region 升降平台自动高度调节

        // 自动调节升降平台高度
        private static void DoAutoTuneHeight(CameraRefMotor motor)
        {

        }




        private static void DoAutHeightother(CameraRefMotor motor)
        {
            try
            {
                IsStartFoundFocus = true;  //开始对焦操作
                outFocusRange = false;
                IsTransLocked = true;
                IsReceiveLocked = false;
                int maxpositionNum = -1;
                hasFindRange = false;
                hasFindFoucus = false;
                flag = string.Empty;
                isFinishFocus = false;
                int maxdefinition = 0;

                CurrentDirection = MotorDirections.Negative;
                // s_zMotor.Speed = 149;//149; 
                s_zMotor.Speed = 130;
                // Thread.Sleep(50);
                // GetPositionBitMap(0);  //获取像素，累计像素集
                // s_zMotor.MoveDirectly(CurrentDirection, 8000);  //向上走10mm   25600/3.175 *10 =80620  4000
                //s_zMotor.MoveDirectly(CurrentDirection, 16000);
                MotorOperator.MotorOperatorZThread((int)(-2 * WorkCurveHelper.ZCoeff));
                
                int cnt = 1;
                int type = 0;
                bool firstMove = true;
                #region   控制范围 大范围确定
                do
                {
                    if (_bCancelAll)
                    {
                        s_zMotor.Stop((int)CurrentDirection);
                        return;
                    }
                    currState = GetZFocusOther(ref maxpositionNum, ref maxdefinition, type, firstMove);
                    firstMove = false;
                    switch (currState)
                    {
                        case HeightStates.Hyper:  //高于焦点
                            // if (!s_zMotor.IsIdel) s_zMotor.Stop((int)CurrentDirection);
                            CurrentDirection = MotorDirections.Positive;
                            //s_zMotor.MoveDirectly(CurrentDirection, 32000);  //向下走10mm   25600/3.175 *10 =80620
                            MotorOperator.MotorOperatorZThread((int)(4 * WorkCurveHelper.ZCoeff));

                            Console.WriteLine("高于焦点，反方向执行");
                            type = 1;
                            break;
                        case HeightStates.Under:  //低于焦点
                            //  if ((CurrentDirection == MotorDirections.Positive) && (maxpositionNum == 0))
                            // if ((CurrentDirection == MotorDirections.Positive) )
                            {
                                //s_zMotor.MoveDirectly(MotorDirections.Negative, 16000);
                                MotorOperator.MotorOperatorZThread((int)(-2 * WorkCurveHelper.ZCoeff));

                                Console.WriteLine("Positive低于焦点,方向走negative：" + CurrentDirection.ToString());
                            }
                            type = 0;
                            //Console.WriteLine("低于焦点,方向：" + CurrentDirection.ToString());
                            break;
                        case HeightStates.InRange:
                            Console.WriteLine("找到区间");
                            hasFindRange = true;
                            type = 0;
                            break;
                        case HeightStates.Unkown:
                            CurrentDirection = MotorDirections.Positive; //未知位置时候向下查找
                            //s_zMotor.MoveDirectly(CurrentDirection, 16000);  //向下走10mm   25600/3.175 *10 =80620
                            MotorOperator.MotorOperatorZThread((int)(2 * WorkCurveHelper.ZCoeff));
                            type = 0;
                            Console.WriteLine("未知区域，向下查找");
                            break;
                        case HeightStates.At:
                            type = 0;
                            hasFindRange = true;
                            break;
                    }
                    cnt++;

                } while (!_bCancelAll && !hasFindRange && cnt < 10);



                #endregion
                if (!_bCancelAll && !outFocusRange)
                {
                    if (hasFindRange)//&& currState == HeightStates.Unkown)
                    {

                        if (CurrentDirection == MotorDirections.Positive)  //当前方向向下，需要向上找
                        {
                            Console.WriteLine(" 确定最后 向上走，maxposition位置：" + maxpositionNum.ToString());
                            CurrentDirection = MotorDirections.Negative;

                        }
                        else
                        {
                            Console.WriteLine("确定最后 向下走，maxposition位置：" + maxpositionNum.ToString());
                            CurrentDirection = MotorDirections.Positive;
                        }
                        s_zMotor.Speed = 149;
                        GetdefineLast(maxdefinition);
                    }

                }

                IsTransLocked = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("------------------error----------------" + ex.ToString());
            }
            finally
            {
                // outFocusRange = false;
                IsTransLocked = true;
                IsReceiveLocked = false;
                int maxpositionNum = 0;
                hasFindRange = false;
                hasFindFoucus = false;
                isFinishFocus = true;
                IsStartFoundFocus = false;  //对焦执行完毕
                if (_bCancelAll)
                {
                    s_zMotor.Stop((int)CurrentDirection);
                }

            }
        }


        private static void GetdefineLast(int maxDefinition)
        {
            int minrange = Convert.ToInt32(maxDefinition * 0.1);
            if (WorkCurveHelper.FocusArea != 3)
            {
                if (minrange > 1000)
                    minrange = Convert.ToInt32(maxDefinition * 0.01);
            }
            int min = maxDefinition - minrange;

            // IsReceiveLocked = false;
            int i = 0;
            Console.WriteLine("minDefinition:" + min.ToString());

            s_zMotor.MoveDirectly(CurrentDirection, MAX_XYZ_AXES_MOTOR_STEP);

            do
            {

                if (IsReceiveLocked)
                {
                    IsReceiveLocked = false;
                    //while (!s_zMotor.IsIdel)
                    //{
                    Thread.Sleep(50);
                    //}
                    if (CameraBitmap != null)
                        CurrentDefinition = MotorAdvance.GetMaxandCurrentDefi(CameraBitmap, WorkCurveHelper.cx, WorkCurveHelper.cy);  //获取像素;  
                    Console.WriteLine("CurrentDefinition -get last:" + CurrentDefinition.ToString());

                    if (CurrentDefinition > min)
                    {
                        hasFindFoucus = true;
                        s_zMotor.Stop();
                        Console.WriteLine("found focus :" + CurrentDefinition.ToString());
                    }
                    i++;
                    //  Thread.Sleep(50);
                }
            } while (!_bCancelAll && !hasFindFoucus && i < 100); ;
            if (WorkCurveHelper.CamerReturnStep > 0)
                s_zMotor.MoveDirectly((MotorDirections)Math.Abs((int)CurrentDirection - 1), WorkCurveHelper.CamerReturnStep);
        }



        private static void GetDefinitionStepByStep(MotorDirections currentDirection, int step)
        {
            IsReceiveLocked = false;
            //确定范围需要返回，at不需要
            if (currState == HeightStates.Unkown)
            {
                //int retstep = (WorkCurveHelper.FindCamerSecond * 2 - (step * 2)) > 0 ? WorkCurveHelper.FindCamerSecond * 2 - (step * 2) : 0;
                //if (retstep <= 0) return;
                // s_zMotor.MoveDirectly(CurrentDirection, Convert.ToInt32(4000 * 0.3) *retstep );  //向上走10mm   25600/3.175 *10 =80620
                int retstep = (WorkCurveHelper.FindCamerSecond * 2 - ((step + 1) * 2)) > 0 ? WorkCurveHelper.FindCamerSecond * 2 - ((step + 1) * 2) : 0;
                if (retstep <= 0) return;

                s_zMotor.MoveDirectly(CurrentDirection, 2000 * retstep); //2000
                Console.WriteLine("CurrentDirection:" + currentDirection.ToString() + " setp: " + retstep);
            }
            else
            {
                s_zMotor.MoveDirectly(CurrentDirection, 2000);
            }
            Thread.Sleep(200);
            lastTimeDefinition = 0;
            int i = 0;

            do
            {
                if (IsReceiveLocked)
                {
                    IsReceiveLocked = false;
                    while (!s_zMotor.IsIdel)
                    {
                        Console.WriteLine("motor is not idel + i=" + i.ToString());
                        Thread.Sleep(100);

                    }
                    if (CameraBitmap != null)
                        CurrentDefinition = MotorAdvance.GetMaxandCurrentDefi(CameraBitmap, WorkCurveHelper.cx, WorkCurveHelper.cy);  //获取像素
                    Console.WriteLine("一步步走的像素：" + CurrentDefinition.ToString());
                }
                if (lastTimeDefinition < CurrentDefinition)
                {
                    lastTimeDefinition = CurrentDefinition;
                }
                else if (lastTimeDefinition > 0 && lastTimeDefinition > CurrentDefinition)
                {
                    s_zMotor.Stop();

                    if (currentDirection == MotorDirections.Positive)
                    {
                        s_zMotor.MoveDirectly(MotorDirections.Negative, WorkCurveHelper.ZmotorStep * 2);  //向上走10mm   25600/3.175 *10 =80620   4000
                        Console.WriteLine("MotorDirections.Negative");
                    }
                    else
                    {
                        s_zMotor.MoveDirectly(MotorDirections.Positive, WorkCurveHelper.ZmotorStep * 2);  //向上走10mm   25600/3.175 *10 =80620    4000
                        Console.WriteLine("MotorDirections.Positive");
                    }

                    Thread.Sleep(500);
                    hasFindFoucus = true;
                    Console.WriteLine("i" + i.ToString() + "hasFindFoucus的lastTimeDefinition像素：" + lastTimeDefinition.ToString() + "hasFindFoucus的CurrentDefinition像素：" + CurrentDefinition.ToString());
                    break;
                }
                Console.WriteLine("i" + i.ToString() + "一步步走CurrentDefinition的像素：" + CurrentDefinition.ToString() + "一步步走lastTimeDefinition的像素：" + lastTimeDefinition.ToString());

                s_zMotor.MoveDirectly(CurrentDirection, WorkCurveHelper.ZmotorStep);  //向上走10mm   25600/3.175 *10 =80620    2000
                //Thread.Sleep(1000);
                i++;
            }

            while (!_bCancelAll && !hasFindFoucus && i < 10);
        }



        //private static void GetDefinitionStepByStep(MotorDirections currentDirection, int step)
        //{
        //    int retstep = (WorkCurveHelper.FindCamerSecond * 2 - (step * 2)) > 0 ? WorkCurveHelper.FindCamerSecond * 2 - (step * 2) : 0;
        //    if (retstep <= 0) return;
        //    // s_zMotor.MoveDirectly(CurrentDirection, Convert.ToInt32(4000 * 0.3) *retstep );  //向上走10mm   25600/3.175 *10 =80620
        //    s_zMotor.MoveDirectly(CurrentDirection, 4000 * retstep);
        //    IsReceiveLocked = false;
        //    lastTimeDefinition = 0;
        //    int i = 0;

        //    do
        //    {
        //        if (IsReceiveLocked)
        //        {
        //            IsReceiveLocked = false;
        //            while (!s_zMotor.IsIdel)
        //            {
        //                Thread.Sleep(100);
        //            }
        //            if (CameraBitmap != null)
        //                CurrentDefinition = MotorAdvance.GetMaxandCurrentDefi(CameraBitmap);  //获取像素
        //            Console.WriteLine("一步步走的像素：" + CurrentDefinition.ToString());
        //        }
        //        if (lastTimeDefinition < CurrentDefinition)
        //        {
        //            lastTimeDefinition = CurrentDefinition;
        //        }
        //        else if (lastTimeDefinition > 0 && lastTimeDefinition > CurrentDefinition)
        //        {
        //            s_zMotor.Stop();

        //            if (currentDirection == MotorDirections.Positive)
        //            {
        //                s_zMotor.MoveDirectly(MotorDirections.Negative, 8000);  //向上走10mm   25600/3.175 *10 =80620
        //                Console.WriteLine("MotorDirections.Negative");
        //            }
        //            else
        //            {
        //                s_zMotor.MoveDirectly(MotorDirections.Positive, 8000);  //向上走10mm   25600/3.175 *10 =80620
        //                Console.WriteLine("MotorDirections.Positive");
        //            }

        //            Thread.Sleep(500);
        //            hasFindFoucus = true;
        //            Console.WriteLine("i" + i.ToString() + "hasFindFoucus的lastTimeDefinition像素：" + lastTimeDefinition.ToString() + "hasFindFoucus的CurrentDefinition像素：" + CurrentDefinition.ToString());
        //            break;
        //        }
        //        Console.WriteLine("i" + i.ToString() + "一步步走CurrentDefinition的像素：" + CurrentDefinition.ToString() + "一步步走lastTimeDefinition的像素：" + lastTimeDefinition.ToString());
        //        s_zMotor.MoveDirectly(CurrentDirection, 4000);  //向上走10mm   25600/3.175 *10 =80620
        //        //Thread.Sleep(1000);
        //        i++;
        //    }

        //    while (!_bCancelAll && !hasFindFoucus && i < 10);
        //}


        private static HeightStates GetZState()
        {
            if (CameraBitmap == null)
                return HeightStates.Unkown;
            PreDefinition = CurrentDefinition;
            GetMaxandCurrentDefi();
            if (PreDefinition != 0 && PositionBitMap.Count > 3 && currState != HeightStates.At)
            {
                //if (PositionBitMap.Count == 3)
                //{
                //    avervalue = (int)PositionBitMap.Keys.Average() / 20;
                //    Console.WriteLine("边界值是:{0}", avervalue.ToString());
                //}
                int DefinitionInfo = GetDefinitionInfo(PositionBitMap, 3);
                if (CurrentDirection == MotorDirections.Negative)
                {
                    Console.WriteLine("上一步正在执行方向：" + CurrentDirection.ToString());
                    if (DefinitionInfo > 0)
                    {
                        return HeightStates.Under;
                    }
                    else if (DefinitionInfo < 0)
                    {
                        // return HeightStates.Hyper;
                        if (currState == HeightStates.Under)
                        {
                            return HeightStates.At;//找到焦点（焦点不是这点，而是清晰度最大的那点）
                        }
                        else if (currState == HeightStates.Hyper)
                            return HeightStates.Hyper;
                        else if (currState == HeightStates.Unkown)
                            return HeightStates.Hyper;
                        else
                            return HeightStates.At;
                    }
                    else
                    {
                        //if (PositionBitMap.Count > 10)
                        return HeightStates.Hyper;  //平均清晰度/20<500 
                        // return currState;
                    }
                }
                else
                {
                    if (DefinitionInfo > 0)
                    {
                        return HeightStates.Hyper;
                    }
                    else if (DefinitionInfo < 0)
                    {
                        // return HeightStates.Under;
                        if (currState == HeightStates.Under)
                            return HeightStates.Under;
                        else if (currState == HeightStates.Hyper)
                        {
                            return HeightStates.At;//找到焦点（焦点不是这点，而是清晰度最大的那点）
                        }
                        else if (currState == HeightStates.Unkown)
                            return HeightStates.Under;
                        else
                            return HeightStates.At;
                    }
                    else
                    {
                        return HeightStates.Under;  //平均清晰度/20<500 
                        // return currState;
                    }
                }
            }
            else if (currState == HeightStates.At)
            {
                return HeightStates.At;
            }
            else
                return HeightStates.Unkown;
        }


        private static HeightStates GetZFocus(ref int maxpositionNum)
        {
            //if (CameraBitmap == null)
            //    return HeightStates.Unkown;
            PositionBitMap.Clear();
            MaxDefinition = 0;
            MoveAccumulation(250, 0);  //移动
            KeyValuePair<double, int> maxPosition = PositionBitMap.ToList().Find(w => w.Key == MaxDefinition);

            maxpositionNum = maxPosition.Value;
            Console.WriteLine("// ----maxpositionNum: " + maxpositionNum.ToString());
            // Console.WriteLine("// ----retDefinitionInfo: " + retDefinitionInfo.ToString());

            if (maxPosition.Key > 100)
            {
                if (maxpositionNum == 0)  //清晰度持续下降,高于焦点
                {
                    if (CurrentDirection == MotorDirections.Negative)
                    {
                        flag += "a";
                        currState = HeightStates.Hyper;
                    }
                    else
                    {
                        flag += "c";
                        currState = HeightStates.Under;
                    }
                    //currState = HeightStates.Hyper;
                    Console.WriteLine("//清晰度持续下降,高于焦点 maxPosition: " + maxPosition.Key.ToString());
                }
                else if (maxpositionNum == WorkCurveHelper.FindCamerSecond - 1)   //清晰度持续上升,低于焦点
                {
                    if (CurrentDirection == MotorDirections.Negative)
                    {

                        currState = HeightStates.At;
                        Console.WriteLine("//打到清晰度不需要走step: " + maxPosition.Key.ToString());
                    }
                    else
                    {
                        if (flag == "abc")
                        {
                            currState = HeightStates.At;
                        }
                        else
                        {
                            flag += "b";
                            currState = HeightStates.Hyper;
                        }
                    }
                    Console.WriteLine("//清晰度持续上升,低于焦点maxPosition: " + maxPosition.Key.ToString() + "currState: " + currState.ToString());
                }
                else
                {
                    currState = HeightStates.Unkown;
                    Console.WriteLine("//清晰度在此范围内maxPosition: " + maxPosition.Key.ToString());
                }
            }
            else
            {
                outFocusRange = true;
                currState = HeightStates.Unkown;
                Console.WriteLine("//清晰度在此范围内maxPosition: " + maxPosition.Key.ToString());
            }
            Console.WriteLine("flag:" + flag);
            return currState;
        }



        private static HeightStates GetZFocusOther(ref int maxpositionNum, ref int maxdefinition, int type, bool firstMove)
        {

            PositionBitMap.Clear();
            MaxDefinition = 0;
            MoveAccumulation(50, type);  //移动
            KeyValuePair<double, int> maxPosition = PositionBitMap.ToList().Find(w => w.Key == MaxDefinition);

            double averagePosition = PositionBitMap.Keys.ToList().Average();
            double minPosition = PositionBitMap.Keys.ToList().Min();
            int posCount = 0;
            int negCout = 0;
            maxpositionNum = maxPosition.Value;
            maxdefinition = Convert.ToInt32(maxPosition.Key);
            Console.WriteLine("// ----maxpositionNum: " + maxpositionNum.ToString());
            Console.WriteLine("// ----minPosition: " + minPosition.ToString());
            Console.WriteLine("// ----averagePosition: " + averagePosition.ToString());

            //极差> 平均值
            // if (maxPosition.Key > 100 && (maxdefinition - minPosition) > averagePosition)
            double difference = 1;
            if (minPosition != 0)
            {
                difference = maxdefinition / minPosition;
            }
            Console.WriteLine("----difference: " + difference.ToString());
            //第一次必须向下
            if (firstMove)
            {
                currState = HeightStates.Hyper;
            }
            else
            {
                if (maxPosition.Key > 1000 && difference > 1.1)
                {
                    outFocusRange = false;
                    if (maxpositionNum == 0)  //清晰度持续下降,高于焦点
                    {
                        if (CurrentDirection == MotorDirections.Negative)
                        {
                            flag += "a";
                            currState = HeightStates.Hyper;
                        }
                        else
                        {
                            flag += "c";
                            currState = HeightStates.Under;
                        }
                        Console.WriteLine("//清晰度持续下降,高于焦点 maxPosition: " + maxPosition.Key.ToString());
                    }
                    else if (maxpositionNum == PositionBitMap.Count - 1)   //清晰度持续上升,低于焦点
                    {
                        if (CurrentDirection == MotorDirections.Negative)
                        {

                            //currState = HeightStates.At;
                            currState = HeightStates.Under;
                            Console.WriteLine("//低于焦点，继续上升： " + maxPosition.Key.ToString());
                        }
                        else
                        {
                            if (flag == "abc")
                            {
                                currState = HeightStates.At;
                            }
                            else
                            {
                                flag += "b";
                                currState = HeightStates.Hyper;
                            }
                        }

                        Console.WriteLine("//清晰度持续上升,低于焦点maxPosition: " + maxPosition.Key.ToString() + "currState: " + currState.ToString());
                    }
                    else
                    {
                        currState = HeightStates.InRange;
                        Console.WriteLine("//清晰度在此范围内maxPosition: " + maxPosition.Key.ToString());
                    }
                }
                else
                {
                    if (CurrentDirection == MotorDirections.Negative)
                    {
                        currState = HeightStates.Hyper;
                    }
                    else
                    {
                        currState = HeightStates.Under;
                    }

                    Console.WriteLine("//没有找到清晰度范围: " + maxPosition.Key.ToString() + "清晰度往：" + currState.ToString());
                }
            }
            Console.WriteLine("flag:" + flag);

            return currState;
        }


        /// <summary>
        /// 得到Z轴下一步方向
        /// </summary>
        /// <param name="maxpositionNum"></param>
        /// <param name="maxdefinition"></param>
        /// <returns></returns>
        private static HeightStates GetZPosition(ref int maxpositionNum, ref int maxdefinition)
        {
            int posCount = 0;
            int negCout = 0;
            PositionBitMap.Clear();
            MaxDefinition = 0;
            MoveAccumulation(50, 0);  //移动
            KeyValuePair<double, int> maxPosition = PositionBitMap.ToList().Find(w => w.Key == MaxDefinition);

            double averagePosition = PositionBitMap.Keys.ToList().Average();
            double minPosition = PositionBitMap.Keys.ToList().Min();

            maxpositionNum = maxPosition.Value;
            maxdefinition = Convert.ToInt32(maxPosition.Key);
            Console.WriteLine("// ----maxpositionNum: " + maxpositionNum.ToString());

            //极差> 平均值
            if (maxPosition.Key > 100 && (maxdefinition - minPosition) > averagePosition)
            {
                outFocusRange = false;

                for (int i = 1; i <= PositionBitMap.Count; i++)
                {
                    if (PositionBitMap.ElementAt(PositionBitMap.Count - i).Key < MaxDefinition && PositionBitMap.ElementAt(PositionBitMap.Count - i).Value > PositionBitMap[MaxDefinition])
                        posCount++;
                    else if (PositionBitMap.ElementAt(PositionBitMap.Count - i).Key < MaxDefinition && PositionBitMap.ElementAt(PositionBitMap.Count - i).Value < PositionBitMap[MaxDefinition])
                        negCout++;
                    else break;

                }

                if (maxpositionNum == 0)  //清晰度持续下降,高于焦点
                {
                    if (CurrentDirection == MotorDirections.Negative)
                    {
                        flag += "a";
                        currState = HeightStates.Hyper;
                    }
                    else
                    {
                        flag += "c";
                        currState = HeightStates.Under;
                    }
                    //currState = HeightStates.Hyper;
                    Console.WriteLine("//清晰度持续下降,高于焦点 maxPosition: " + maxPosition.Key.ToString());
                }
                else if (maxpositionNum == PositionBitMap.Count - 1)   //清晰度持续上升,低于焦点
                {
                    if (CurrentDirection == MotorDirections.Negative)
                    {

                        //currState = HeightStates.At;
                        currState = HeightStates.Under;
                        Console.WriteLine("//低于焦点，继续上升： " + maxPosition.Key.ToString());
                    }
                    else
                    {
                        if (flag == "abc")
                        {
                            currState = HeightStates.At;
                        }
                        else
                        {
                            flag += "b";
                            currState = HeightStates.Hyper;
                        }
                    }

                    Console.WriteLine("//清晰度持续上升,低于焦点maxPosition: " + maxPosition.Key.ToString() + "currState: " + currState.ToString());
                }
                else
                {
                    currState = HeightStates.InRange;
                    Console.WriteLine("//清晰度在此范围内maxPosition: " + maxPosition.Key.ToString());
                }
            }
            else
            {
                outFocusRange = true;
                currState = HeightStates.Unkown;
                Console.WriteLine("//没有找到清晰度范围: " + maxPosition.Key.ToString());
            }
            Console.WriteLine("flag:" + flag);

            return currState;
        }


        /// <summary>
        /// 1:持续增加，-1持续减小；0不确定
        /// </summary>
        /// <param name="DefiDic"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static int GetDefinitionInfo(Dictionary<double, int> DefiDic, int count)
        {
            int addcount = 0;
            int deccount = 0;

            //if (DefiDic.Count >= 3)
            //{
            //    double averagePosition = PositionBitMap.Keys.ToList().Average();
            //    double minPosition = PositionBitMap.Keys.ToList().Min();
            //}
            //int retNum = 0;
            for (int i = 1; i <= count; i++)
            {
                if (DefiDic.ElementAt(DefiDic.Count - i).Key - DefiDic.ElementAt(DefiDic.Count - i - 1).Key > 0)
                    addcount++;
                else if (DefiDic.ElementAt(DefiDic.Count - i).Key - DefiDic.ElementAt(DefiDic.Count - i - 1).Key < 0)
                    deccount++;
            }

            Console.WriteLine("addcount:" + addcount.ToString() + " deccount:" + deccount.ToString());
            if (addcount == count)
                return 1;
            else if (deccount == count)
                return -1;
            //else if (avervalue<500)
            //    return 0;
            else
                return 2;

        }



        /// <summary>
        /// 移动时间10s 左右
        /// </summary>
        private static void MoveAccumulation(int sleeptime, int type)
        {
            int i = 0;
            int movetime = (type == 0) ? 2500 : 5000;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            do
            {
                if (IsReceiveLocked)
                {
                    IsReceiveLocked = false;
                    Thread.Sleep(sleeptime);
                    GetPositionBitMap(i);  //获取像素，累计像素集
                    i++;
                }
            }
            while (!_bCancelAll && sw.Elapsed.TotalMilliseconds < movetime);
            if (IsReceiveLocked)
            {
                IsReceiveLocked = false;
                Thread.Sleep(sleeptime);
                GetPositionBitMap(i);  //结束后增加一次图像保存
            }
            string showtime = sw.Elapsed.TotalMilliseconds.ToString();
            Console.WriteLine("获取像素占用时间：{0}ms", sw.Elapsed.TotalMilliseconds);
            sw.Reset();
        }

        private static void GetPositionBitMap(int cnt)
        {
            if (CameraBitmap != null)
                CurrentDefinition = MotorAdvance.GetMaxandCurrentDefi(CameraBitmap, WorkCurveHelper.cx, WorkCurveHelper.cy);  //获取像素
            Console.WriteLine("cnt:" + cnt.ToString() + "  GetMaxandCurrentDefi-Current像素:" + CurrentDefinition.ToString());

            if (CurrentDefinition > MaxDefinition)
                MaxDefinition = CurrentDefinition;

            PositionBitMap.Add(CurrentDefinition, cnt);
        }

        private static void IsGetMaxDefinition(Dictionary<double, int> DefiDic)
        {
            int returnNum = 0;
            int posCount = 0;
            int negCout = 0;

            if (DefiDic.Count < 3) return;
            for (int i = 1; i <= 3; i++)
            {
                if (DefiDic.ElementAt(DefiDic.Count - i).Key < MaxDefinition && DefiDic.ElementAt(DefiDic.Count - i).Value > DefiDic[MaxDefinition])
                    posCount++;
                else if (DefiDic.ElementAt(DefiDic.Count - i).Key < MaxDefinition && DefiDic.ElementAt(DefiDic.Count - i).Value < DefiDic[MaxDefinition])
                    negCout++;
                else break;

            }
            if (posCount >= 3) PositivePositionFind |= true;
            else if (negCout >= 3) NegativePositionFind |= true;

            Console.WriteLine("-----满足正向条件：" + PositivePositionFind.ToString() + "反向条件：---" + NegativePositionFind.ToString());


        }

        // 自动调节升降平台高度（同步）
        private static void DoAutoTuneHeightSync()
        {
            lock (ooobj)
            {
                if (s_zMotor.IsIdel)
                {
                    //for (int i = 0; i < 5; i++)
                    {
                        DoAutHeightother(s_zMotor);
                    }
                }
            }
        }


        private static void DoAutoHeight(CameraRefMotor motor)
        {
            try
            {
                IsStartFoundFocus = true;  //开始对焦操作
                outFocusRange = false;
                IsTransLocked = true;
                IsReceiveLocked = false;
                int maxpositionNum = -1;
                hasFindRange = false;
                hasFindFoucus = false;
                flag = string.Empty;
                isFinishFocus = false;
                int maxdefinition = 0;
                MaxDefinition = 0;
                //第一步向上走
                CurrentDirection = MotorDirections.Negative;
                currState = HeightStates.Unkown;
                s_zMotor.Speed = 149;
                s_zMotor.MoveDirectly(CurrentDirection, MAX_XYZ_AXES_MOTOR_STEP);  //向上走10mm   25600/3.175 *10 =80620  4000

                HeightStates initState = GetZState();
                Console.WriteLine("当前清晰度：{0} 最大清晰度：{1} 当前基于焦点的位置：{2} 方向：{3}",
                     CurrentDefinition.ToString(), MaxDefinition.ToString(), initState.ToString()
                     , CurrentDirection.ToString());
                currState = initState;

                int cnt = 1;
                #region   控制范围 大范围确定
                do
                {
                    if (IsReceiveLocked)
                    {
                        IsReceiveLocked = false;
                        Thread.Sleep(50);
                        currState = GetZState();

                        switch (currState)
                        {
                            case HeightStates.Hyper:
                                {
                                    if (CurrentDirection == MotorDirections.Negative)
                                    {
                                        motor.Stop();   //向下走
                                        CurrentDirection = MotorDirections.Positive;
                                        motor.MoveDirectly(MotorDirections.Positive, MAX_XYZ_AXES_MOTOR_STEP);
                                        //Thread.Sleep(250);
                                    }
                                }
                                break;
                            case HeightStates.Under:
                                {
                                    if (CurrentDirection == MotorDirections.Positive)
                                    {
                                        motor.Stop();  //向上走
                                        CurrentDirection = MotorDirections.Negative;
                                        motor.MoveDirectly(MotorDirections.Negative, MAX_XYZ_AXES_MOTOR_STEP);
                                    }
                                }
                                break;
                            case HeightStates.At:
                                //移动到焦点位置
                                {
                                    motor.Stop();
                                    hasFindFoucus = true;
                                }
                                break;
                            case HeightStates.Unkown:
                                Console.WriteLine("状态Unkown");
                                break;
                            default:
                                break;
                        }
                        Console.WriteLine("2当前清晰度：{0} 最大清晰度：{1} 当前基于焦点的位置：{2} 方向：{3};Z轴电机号{4}",
                                          CurrentDefinition.ToString(), MaxDefinition.ToString(),
                                          currState.ToString()
                                          , CurrentDirection.ToString(), WorkCurveHelper.DeviceCurrent.MotorZCode.ToString());


                    }

                    if (motorIdleCount > 3)
                        currState = HeightStates.Under;
                    if (motor.IsIdel)
                    {
                        motorIdleCount++;
                    }
                    else
                        motorIdleCount = 0;


                } while (!_bCancelAll && !hasFindFoucus);

                motor.Stop();

                try
                {
                #endregion
                    if (!_bCancelAll && hasFindFoucus)
                    {
                        if (CurrentDirection == MotorDirections.Positive)  //当前方向向下，需要向上找
                        {
                            Console.WriteLine(" 确定最后 向上走，maxposition位置：" + maxpositionNum.ToString());
                            CurrentDirection = MotorDirections.Negative;
                        }
                        else
                        {
                            Console.WriteLine("确定最后 向下走，maxposition位置：" + maxpositionNum.ToString());
                            CurrentDirection = MotorDirections.Positive;
                            ////GetDefinitionStepByStep(CurrentDirection, maxpositionNum);
                        }
                        s_zMotor.Speed = 149;
                        GetdefineLast((int)MaxDefinition);
                    }
                    else
                        Msg.Show("焦点搜寻失败");
                }
                catch (Exception ex)
                {
                    Msg.Show("出错" + ex.ToString());
                }

                IsTransLocked = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("------------------error----------------" + ex.ToString());
            }
            finally
            {
                // outFocusRange = false;
                IsTransLocked = true;
                IsReceiveLocked = false;
                int maxpositionNum = 0;
                hasFindRange = false;
                hasFindFoucus = false;
                isFinishFocus = true;
                IsStartFoundFocus = false;  //对焦执行完毕
                // if (_bCancelAll)
                {
                    s_zMotor.Stop((int)CurrentDirection);
                }
                PositionBitMap.Clear();
                //if (AutoFocusEndEndEvent != null)
                //{
                //    AutoFocusEndEndEvent();
                //}
            }
        }



        // 开始自动调整高度（同步）
        // speed:移动速度
        private static void DoAutoTuneHeightSync(Object state)
        {
            s_zMotor.Speed = (Int32)state;
            DoAutoTuneHeightSync();
        }

        /// <summary>
        /// 开始自动调整高度，不可设置速度
        /// </summary>
        public static void AutoTuneHeightSync(Int32 Speed)
        {
            _bCancelAll = false;
            Thread thr = new Thread(new ParameterizedThreadStart(DoAutoTuneHeightSync));
            thr.Start(Speed);
        }



        /// <summary>
        /// 这里认为连续n个点清晰降低了，那就是远离焦点
        /// </summary>
        /// <param name="currentSate"></param>
        /// <returns></returns>
        private static HeightStates GetZState(HeightStates currentSate)
        {
            if (CameraBitmap == null)
                return HeightStates.Unkown;
            return HeightStates.Unkown;

        }
        #endregion


        #region 复位
        /// <summary>
        /// 复位0，0点
        /// </summary>
        public static void RepositionXY()
        {
            s_xMotor.Reposition(0);
            do
            {
                if (_bCancelAll)
                    return;
                Thread.Sleep(20);
            }
            while (!MotorAdvance.s_xMotor.IsIdel);

            s_yMotor.Reposition(0);
            do
            {
                if (_bCancelAll)
                    return;
                Thread.Sleep(20);
            }
            while (!MotorAdvance.s_yMotor.IsIdel);


        }





        #endregion


        /// <summary>
        /// Z电机高度状态
        /// </summary>
        public enum HeightStates
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
            At,
            /// <summary>
            /// 未知
            /// </summary>
            Unkown,

            ///找到区间
            InRange
        }
        #region  图像
        public static int Count = 1;

        public static void CancelAllStop()
        {

            _bCancelAll = true;

            isFinishFocus = true;
            s_zMotor.Stop();
            s_xMotor.Stop();
            s_yMotor.Stop();
            _bCancelAll = true;
        }

        public static void GetMaxandCurrentDefi()
        {
            IsReceiveLocked = true;
            if (CameraBitmap != null)
            {
                CurrentDefinition = GetMaxandCurrentDefi(CameraBitmap, 0.6, 0.1);
            }
            if (!PositionBitMap.ContainsKey(CurrentDefinition))
                PositionBitMap.Add(CurrentDefinition, PositionBitMap.Count + 1);
            if (CurrentDefinition > MaxDefinition)
                MaxDefinition = CurrentDefinition;
            Console.WriteLine("Current:" + CurrentDefinition.ToString() + "max" + MaxDefinition.ToString());
            Console.WriteLine("当前位置：" + PositionBitMap[CurrentDefinition].ToString() + "最高像素位置：" + PositionBitMap[MaxDefinition]);

        }


        public static double GetMaxandCurrentDefi(Bitmap bitmap, double x, double y)
        {
            IsReceiveLocked = true;
            Count++;
            if (bitmap != null)
            {
                int grade = 2;
                switch (WorkCurveHelper.FocusArea)
                {
                    case 1:
                        grade = 2;
                        break;
                    case 2:
                        grade = 4;
                        break;
                    case 3:
                        grade = 8;
                        break;
                }

                int halfw = bitmap.Width / 2;
                int halfh = bitmap.Height / 2;
                Bitmap newBitmap = new Bitmap((int)(halfw * 2 / grade), (int)(halfh * 2 / grade));
                Graphics gTemp = Graphics.FromImage(newBitmap);
                Rectangle rectSource = new Rectangle((int)(FociBMPX - halfw / grade), (int)(FociBMPY - halfh / grade), (int)(halfw * 2 / grade), (int)(halfh * 2 / grade));
                Rectangle rectDest = new Rectangle(0, 0, newBitmap.Width, newBitmap.Height);
                gTemp.DrawImage(bitmap, rectDest, rectSource, GraphicsUnit.Pixel);
                gTemp.Dispose();
                CurrentDefinition = BitmapFocusedAssessmentC_Q(0.6, 0.1, newBitmap.GetHbitmap());

            }


            return CurrentDefinition;
        }
        public static bool outFocusRange = false;  // 不在对焦范围内
        private static int motorIdleCount = 0;
        public static bool isFinishFocus = true;  //是否停止聚焦
        public static bool IsStartFoundFocus = false; //开始对焦;
        static string flag = string.Empty;
        static int avervalue = 0;
        const int CONTIDECOUNT = 3;
        static double lastTimeDefinition;
        static bool hasFindRange = false;   //是否找到范围
        static bool hasFindFoucus = false;  //是否找到焦点
        static HeightStates currState = HeightStates.Unkown;
        public static bool NegativePositionFind = false;  //反向连续5个点下降
        public static bool PositivePositionFind = false;  //正向连续5个点下降
        static double MaxDefinition;
        static double CurrentDefinition;
        static double PreDefinition;
        static MotorDirections CurrentDirection;//1下0上  移动方向 
        public static int FociBMPX;
        public static int FociBMPY;

        static Dictionary<double, int> PositionBitMap = new Dictionary<double, int>();
        [DllImport("EDXRF.dll", EntryPoint = "BitmapFocusedAssessmentC_Q", CallingConvention = CallingConvention.StdCall)]
        internal static extern double BitmapFocusedAssessmentC_Q(double k1, double k2, IntPtr hbitmap);



        #endregion
    }
}
