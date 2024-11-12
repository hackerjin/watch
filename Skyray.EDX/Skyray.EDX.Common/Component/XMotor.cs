using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Skyray.EDXRFLibrary;

namespace Skyray.EDX.Common.Component
{
    /// <summary>
    /// X电机
    /// </summary>
    public class XMotor : CameraRefMotor
    {
        public XMotor(DeviceMeasure device)
            : base()
        {
            InitXMotor(device);
        }

        public void InitXMotor(DeviceMeasure device)
        {
            ID = WorkCurveHelper.DeviceCurrent.MotorXCode;
            DirectionFlag = WorkCurveHelper.DeviceCurrent.MotorXDirect;
            MaxStep = WorkCurveHelper.DeviceCurrent.MotorXMaxStep;
            this.Speed = WorkCurveHelper.DeviceCurrent.MotorXMaxStep;
        }

        protected void DoMoveRight(Object state)
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

        protected void DoMoveLeft(Object state)
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

        public void MoveRight(Int32 step)
        {
            _bCancelAll = _bCancel = false;
            Thread thr = new Thread(new ParameterizedThreadStart(DoMoveRight));
            thr.Start(step);
        }

        public void MoveRight(Int32 step, Int32 speed)
        {
            this.Speed = speed;
            MoveRight(step);
        }

        public void MoveLeft(Int32 step)
        {
            _bCancelAll = _bCancel = false;
            Thread thr = new Thread(new ParameterizedThreadStart(DoMoveLeft));
            thr.Start(step);
        }

        public void MoveLeft(Int32 step, Int32 speed)
        {
            this.Speed = speed;
            MoveLeft(step);
        }
    }
}
