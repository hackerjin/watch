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
    /// ���������
    /// </summary>
    public enum MotorName
    {
        OfFilter,     ///�˹�Ƭ
        OfCollimator, ///׼ֱ��
        OfChamber,        ///��Ʒǻ
        OfXAxes,      ///X��
        OfYAxes,      ///Y��
        OfZAxes,       ///Z��
        OfTarget       ///�в�
    }

    /// <summary>
    /// ����ƶ��¼�
    /// </summary>
    public class MotorMoveStopEvent : EventArgs
    {
        private int id;         ///����ı��
        public int ID
        {
            get { return id; }
        }
        private MotorName name; ///�������
        public MotorName Name
        {
            get { return name; }
        }

        /// <summary>
        /// ����ƶ��¼�
        /// </summary>
        /// <param name="id">����ı��</param>
        /// <param name="name">�������</param>
        public MotorMoveStopEvent(int id, MotorName name)
        {
            this.id = id;
            this.name = name;
        }
    }

    /// <summary>
    /// �����
    /// </summary>
    public class Motor
    {

        private int index;

        /// <summary>
        /// ��ǰ������Ŀ��   
        /// </summary>
        public int Index         ///��ǰ������Ŀ������   
        {
            set { index = value; }
            get { return index; }
        }
        public int MaxStep = 65535;        /// ���ķ�Χ
        public int DefSpeed = 120;       //�ƶ��ٶ�
        
        private const int dueTime = 100; ///��ؼ��ʱ��0.1 ��
        //public NetControl.NetControl port;
        private Port port = null;
            
        public event EventHandler<MotorMoveStopEvent> OnMoveStop;
        public bool OnMoveStopEnabled=true;
        public int ID;              ///���
        public MotorName Name;      ///���������
        public int DirectionFlag;   ///�������ƶ���־��1,����0
        public bool Exist;          ///�Ƿ����
        protected bool isMoving;       ///����Ƿ����ƶ�  
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
        /// �Ƿ����
        /// </summary>
        public bool IsIdel
        {
            get
            {

                return port.MotorIsIdel(ID);
            }
        }
    
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="motorName">���������</param>
        public Motor(MotorName motorName,Port port)
        {
            Name = motorName;
        
            isMoving = false;
            this.port = port;
        }

        /// <summary>
        /// ��DirectionFlag�෴��ֵ
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
                if (this.Name == MotorName.OfChamber && WorkCurveHelper.DeviceTypeForChamber.ToUpper().Equals("NEWEDX6000B"))//��6000B����Ʒǻ�Ĳ���
                {
                   // IsSucceed = false;
                    isMoving = true;
                    int steps = 0;
                    int chamindex = -1;
                    bool istruestep = false;
                    bool isinitialed = false;
                    bool isbusy = false;
                    int stepTemp = repositionStep;

                    //ֱ���ƶ���Ʒ���ƶ�֮ǰ���ж��Ƿ����
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
                        port.MotorControl(ID, dir, MaxStep, true, DefSpeed);//��λ
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

                        } while (!IsIdel);//�ƶ�
                    }
                    IsSucceed = !stopFlag &&(conCount < iMaxConCount);
                }
                isMoving = false;
                //Console.WriteLine(this.Name.ToString() + IsSucceed + "�ƶ���ϣ�");
                if (OnMoveStop != null && OnMoveStopEnabled)//���ͽ�����Ϣ
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
        /// ��λ
        /// </summary>
        /// <param name="maxStep">��������ƶ�����󲽳�</param>
        /// <param name="repositionStep">��λ���λ�ò���</param>
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
                if (OnMoveStop != null&&OnMoveStopEnabled)//���ͽ�����Ϣ
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
    /// ֻ���ƶ����ض�λ�õĵ��(���˹�Ƭ��׼ֱ������Ʒǻ�ĵ��)
    /// </summary>
    public class QuantaMotor : Motor
    {
        public int[] Target;          ///���ƶ���Ŀ��
        public int RepositionIndex;


        /// <summary>
        ///�ɹ���Ŀ��ĸ���
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
        /// ���캯��
        /// </summary>
        /// <param name="motorName">�������</param>
        /// <param name="portType">�ӿ�����</param>
        public QuantaMotor(MotorName motorName,Port port,object obj,int speed)
            : base(motorName, port)
        {
            Index = -1;
            this.theWholeObj = obj;
            this.DefSpeed = speed;
        }

        ///// <summary>
        ///// ��λ
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
        /// �ƶ���Ŀ��λ��
        /// </summary>
        /// <param name="targetIndex">Ŀ��λ������</param>
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
