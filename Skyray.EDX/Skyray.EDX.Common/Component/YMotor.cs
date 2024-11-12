using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Skyray.EDXRFLibrary;

namespace Skyray.EDX.Common.Component
{
    /// <summary>
    /// Y电机
    /// </summary>
    public class YMotor : CameraRefMotor
    {
        public YMotor(DeviceMeasure device)
            : base()
        {
            InitYMotor(device);
        }

        public void InitYMotor(DeviceMeasure device)
        {
            
            ID = WorkCurveHelper.DeviceCurrent.MotorYCode;
            DirectionFlag = WorkCurveHelper.DeviceCurrent.MotorYDirect;
            MaxStep = WorkCurveHelper.DeviceCurrent.MotorYMaxStep;
            this.Speed = WorkCurveHelper.DeviceCurrent.MotorYSpeed;
        }

        //public  YMotor CreateInstance(Device device)
        //{
        //    if (s_singleInstance == null)
        //    {
        //        s_singleInstance = new YMotor(device);
        //    }
        //    else
        //    {
        //        s_singleInstance.InitMotor();
        //        InitYMotor(device);
        //    }
        //    return s_singleInstance;
        //}

        protected void DoMoveOut(Object state)
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

        protected void DoMoveIn(Object state)
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

        public void MoveOut(Int32 step)
        {
            _bCancelAll = _bCancel = false;
            Thread thr = new Thread(new ParameterizedThreadStart(DoMoveOut));
            thr.Start(step);
        }

        public void MoveOut(Int32 step, Int32 speed)
        {
            this.Speed = speed;
            MoveOut(step);
        }

        public void MoveIn(Int32 step)
        {
            _bCancelAll = _bCancel = false;
            Thread thr = new Thread(new ParameterizedThreadStart(DoMoveIn));
            thr.Start(step);
        }

        public void MoveIn(Int32 step, Int32 speed)
        {
            this.Speed = speed;
            MoveIn(step);
        }
    }
}
