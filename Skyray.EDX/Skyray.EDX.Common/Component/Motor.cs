using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml;
using Skyray.EDXRFLibrary;
using Skyray.EDX.Common.Component;

namespace Skyray.EDX.Common
{
    /// <summary>
    /// 电机的名称
    /// </summary>
    public enum MotorName
    {
        OfFilter,     ///滤光片
        OfCollimator, ///准直器
        OfChamber,        ///样品腔
        OfXAxes,      ///X轴
        OfYAxes,      ///Y轴
        OfZAxes,       ///Z轴
        OfTarget       ///靶材
    }

    /// <summary>
    /// 电机移动事件
    /// </summary>
    public class MotorMoveStopEvent : EventArgs
    {
        private int id;         ///电机的编号
        public int ID
        {
            get { return id; }
        }
        private MotorName name; ///电机名称
        public MotorName Name
        {
            get { return name; }
        }

        /// <summary>
        /// 电机移动事件
        /// </summary>
        /// <param name="id">电机的编号</param>
        /// <param name="name">电机名称</param>
        public MotorMoveStopEvent(int id, MotorName name)
        {
            this.id = id;
            this.name = name;
        }
    }

    /// <summary>
    /// 电机类
    /// </summary>
    public class Motor
    {

        private int index;

        /// <summary>
        /// 当前所处的目标   
        /// </summary>
        public int Index         ///当前所处的目标索引   
        {
            set { index = value; }
            get { return index; }
        }
        public int MaxStep = 65535;        /// 最大的范围
        public int DefSpeed = 120;       //移动速度
        
        private const int dueTime = 100; ///监控间隔时间0.1 秒
        //public NetControl.NetControl port;
        private Port port = null;
            
        public event EventHandler<MotorMoveStopEvent> OnMoveStop;
        public bool OnMoveStopEnabled=true;
        public int ID;              ///编号
        public MotorName Name;      ///电机的名称
        public int DirectionFlag;   ///正方向移动标志，1,或者0
        public bool Exist;          ///是否存在
        protected bool isMoving;       ///电机是否在移动  
                                       ///
        public object theWholeObj;

        public Thread thread = null;
        private int repositionStep;

        public bool stopFlag = false;
        public bool IsSucceed = true;

        public int iMaxConCount = 50;
        public bool IsMoving
        {
            get { return isMoving && Exist; }
            set { this.isMoving = value; }
        }

        /// <summary>
        /// 是否空闲
        /// </summary>
        public bool IsIdel
        {
            get
            {

                return port.MotorIsIdel(ID);
            }
        }
    
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="motorName">电机的名称</param>
        public Motor(MotorName motorName,Port port)
        {
            Name = motorName;
        
            isMoving = false;
            this.port = port;
        }

        /// <summary>
        /// 求DirectionFlag相反的值
        /// </summary>
        private int ReverseDirectionFlag()
        {
            if (DirectionFlag == 0)
            {
                return 1;
            }
            else return 0;

        }

        private void SendStopMsg(MessageFormat format)
        {
            if (OnMoveStop != null && OnMoveStopEnabled)
            {
                MotorMoveStopEvent e = new MotorMoveStopEvent(ID, Name);
                EventHandler<MotorMoveStopEvent> temp = OnMoveStop;
                temp(this, e);
                if (this.Name == MotorName.OfFilter)
                {
                    format = new MessageFormat(Info.FilterMotorEnd, 0);
                    WorkCurveHelper.specMessage.localMesage.Add(format);
                }
                else if (this.Name == MotorName.OfCollimator)
                {
                    format = new MessageFormat(Info.CollimatMotorEnd, 0);
                    WorkCurveHelper.specMessage.localMesage.Add(format);
                }
                else if (this.Name == MotorName.OfChamber)
                {
                    format = new MessageFormat(Info.ChamberMotorEnd, 0);
                    WorkCurveHelper.specMessage.localMesage.Add(format);
                }
                else if (this.Name == MotorName.OfTarget)
                {
                    format = new MessageFormat(Info.TargetMotorEnd, 0);
                    WorkCurveHelper.specMessage.localMesage.Add(format);
                }
            }
            if (OnMoveStop != null) OnMoveStopEnabled = true;
            
        }

        private void CheckStop(int index, int dir, int step, bool swt, int speed, MessageFormat format)
        {
            port.MotorControl(index, dir, step, swt, speed);
            index = -1;
            isMoving = true;
            SendStopMsg(format);
        }

        private void ExcuteReposition()
        {
            System.Threading.Monitor.Enter(theWholeObj);
            try
            {
                MessageFormat format = new MessageFormat();
                if (this.Name == MotorName.OfFilter && OnMoveStopEnabled)
                {
                    format = new MessageFormat(Info.FilterMotor, 0);
                    WorkCurveHelper.specMessage.localMesage.Add(format);
                }
                else if (this.Name == MotorName.OfCollimator 
                    &&WorkCurveHelper.DeviceCurrent!=null
                    &&this.index > 0 
                    && this.index <= WorkCurveHelper.DeviceCurrent.Collimators.Count)
                {
                    string ss=string.Format(Info.CollimatMotor,WorkCurveHelper.DeviceCurrent.Collimators[this.index-1].Diameter);
                    format = new MessageFormat(ss, 0);
                    WorkCurveHelper.specMessage.localMesage.Add(format);
                }
                else if (this.Name == MotorName.OfChamber)
                {
                    format = new MessageFormat(Info.ChamberMotor, 0);
                    WorkCurveHelper.specMessage.localMesage.Add(format);
                }
                else if (this.Name == MotorName.OfTarget)
                {
                    format = new MessageFormat(Info.TargetMotor, 0);
                    WorkCurveHelper.specMessage.localMesage.Add(format);
                }
                UsbPort.UsbIdel = true;
                if (this.Name == MotorName.OfChamber && WorkCurveHelper.DeviceTypeForChamber.ToUpper().Equals("NEWEDX6000B"))//新6000B的样品腔的操作
                {
                   // IsSucceed = false;
                    isMoving = true;
                    int steps = 0;
                    int chamindex = -1;
                    bool istruestep = false;
                    bool isinitialed = false;
                    bool isbusy = false;
                    int stepTemp = repositionStep;

                    //直接移动样品杯移动之前不判断是否空闲
                    //do
                    //{
                    //    Thread.Sleep(200);
                    //    if (stopFlag)
                    //    {
                    //        CheckStop(ID, DirectionFlag, 0, true, DefSpeed, format);
                    //        return;
                    //    }
                    //} while (!IsIdel);

                    port.MotorControl(ID, DirectionFlag, stepTemp, true, DefSpeed);
                    do
                    {
                        Thread.Sleep(1000);
                        if (stopFlag)
                        {
                            CheckStop(ID, DirectionFlag, 0, true, DefSpeed, format);
                            return;
                        }
                    } while (!IsIdel);
                    port.GetChamberStatus(ref steps, ref chamindex, ref istruestep, ref isinitialed, ref isbusy);
                    Index = chamindex;
                    


                }
                else
                {
                    int conCount = 0;
                    do
                    {
                        Thread.Sleep(200);
                        if (stopFlag||conCount>=iMaxConCount)
                        {
                            if (conCount >= iMaxConCount)
                            {
                                Log.Error(format.messageContext + "error");
                                //WorkCurveHelper.specMessage.localMesage.Add(new MessageFormat(format.messageContext + "error", 0));
                            }
                            CheckStop(ID, DirectionFlag, 0, true, DefSpeed, format);
                            isMoving = false;
                            //return;
                            break;
                        }
                        conCount++;
                        //Console.WriteLine(this.Name.ToString() + conCount);
                    } while (!IsIdel);
                    if (!stopFlag && conCount < iMaxConCount)
                    {
                        conCount = 0;
                        int dir = ReverseDirectionFlag();
                        port.MotorControl(ID, dir, MaxStep, true, DefSpeed);//复位
                        do
                        {
                            Thread.Sleep(1000);
                            if (stopFlag || conCount >= iMaxConCount)
                            {
                                if (conCount >= iMaxConCount)
                                {
                                    Log.Error(format.messageContext + "error");
                                }
                                CheckStop(ID, dir, 0, true, DefSpeed, format);
                                isMoving = false;
                                //return;
                                break;
                            }
                            conCount++;
                        } while (!IsIdel);
                        if (this.Name == MotorName.OfTarget)
                        {
                            Thread.Sleep(1000);
                        }
                        if (repositionStep >= 0)
                        {
                            Thread.Sleep(200);
                            port.MotorControl(ID, DirectionFlag, repositionStep, true, DefSpeed);
                        }
                    }
                    

                    if (!stopFlag && conCount < iMaxConCount)
                    {
                        conCount = 0;
                        do
                        {
                            Thread.Sleep(1000);
                            if (stopFlag || conCount >= iMaxConCount)
                            {
                                if (conCount >= iMaxConCount)
                                {
                                    Log.Error(format.messageContext + "error");
                                    //WorkCurveHelper.specMessage.localMesage.Add(new MessageFormat(format.messageContext + "error", 0));
                                }
                                CheckStop(ID, DirectionFlag, 0, true, DefSpeed, format);
                                isMoving = false;
                                //return;
                                break;
                            }
                            conCount++;
                            //Console.WriteLine(this.Name.ToString() + conCount);

                        } while (!IsIdel);//移动
                    }
                    IsSucceed = !stopFlag &&(conCount < iMaxConCount);
                }
                isMoving = false;
                //Console.WriteLine(this.Name.ToString() + IsSucceed + "移动完毕！");
                if (OnMoveStop != null && OnMoveStopEnabled)//发送结束消息
                {
                    MotorMoveStopEvent e = new MotorMoveStopEvent(ID, Name);
                    EventHandler<MotorMoveStopEvent> temp = OnMoveStop;
                    temp(this, e);
                }
                if (OnMoveStop != null) OnMoveStopEnabled = true;
            }
            finally
            {
                System.Threading.Monitor.Exit(theWholeObj);
            }
        }
        /// <summary>
        /// 复位
        /// </summary>
        /// <param name="maxStep">电机允许移动的最大步长</param>
        /// <param name="repositionStep">复位点的位置步长</param>
        protected void Reposition(int maxStep, int repositionStep)
        {
            isMoving = true;
            this.repositionStep = repositionStep;
            thread = new Thread(new ThreadStart(ExcuteReposition));
            thread.IsBackground = true;
            thread.Priority = ThreadPriority.Highest;
            thread.Name = this.Name.ToString();
            thread.Start();
        }

        protected void NNMove()
        {
            isMoving = true;
            thread = new Thread(new ThreadStart(NotMove));
            thread.IsBackground = true;
            thread.Priority = ThreadPriority.Highest;
            thread.Name = this.Name.ToString();
            thread.Start();
        }

        public void NotMove()
        {
            System.Threading.Monitor.Enter(theWholeObj);
            try
            {
                isMoving = false;
                if (OnMoveStop != null&&OnMoveStopEnabled)//发送结束消息
                {
                    MotorMoveStopEvent e = new MotorMoveStopEvent(ID, Name);
                    EventHandler<MotorMoveStopEvent> temp = OnMoveStop;
                    temp(this, e);
                }
                if (OnMoveStop != null) OnMoveStopEnabled = true;
            }
            finally
            {
                System.Threading.Monitor.Exit(theWholeObj);
            }
        }
    }

    /// <summary>
    /// 只能移动到特定位置的电机(如滤光片、准直器、样品腔的电机)
    /// </summary>
    public class QuantaMotor : Motor
    {
        public int[] Target;          ///可移动的目标
        public int RepositionIndex;


        /// <summary>
        ///可供移目标的个数
        /// </summary>
        public int TargetCount
        {
            get
            {
                if (Target != null)
                {
                    return Target.Length;
                }
                else return 0;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="motorName">电机名称</param>
        /// <param name="portType">接口类型</param>
        public QuantaMotor(MotorName motorName,Port port,object obj,int speed)
            : base(motorName, port)
        {
            Index = -1;
            this.theWholeObj = obj;
            this.DefSpeed = speed;
        }

        ///// <summary>
        ///// 复位
        ///// </summary>
        //public void Reposition()
        //{
        //    if (IsIdel)
        //    {
        //        int repositionStep = Target[RepositionIndex];
        //        Reposition(MaxStep, repositionStep);
        //        Index = RepositionIndex;
        //    }
        //}

        /// <summary>
        /// 移动到目标位置
        /// </summary>
        /// <param name="targetIndex">目标位置索引</param>
        public void MoveTo(int targetIndex)
        {
            if ((targetIndex >= 0) && (targetIndex <= TargetCount) && targetIndex != Index && targetIndex >= 1)
            {

                Index = targetIndex;
                if (Target[targetIndex - 1] < 0)
                    NNMove();
                else
                    Reposition(MaxStep, Target[targetIndex - 1]);
                
            }
            else
            {
                NNMove();
            }
        }
    }
}
