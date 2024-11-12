using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Skyray.EDXRFLibrary;

namespace Skyray.EDX.Common
{
    public class Dogy
    {
        #region 字段

        // 内置时钟，检查密码狗是否存在，并刷新密码狗剩余时间
        // 刷新间隔默认12小时，可以更改
        private Timer tFoundUpdateDog;
        private Boolean bIsUnLocked;    // 是否解锁
        private long lngLeftHours;      // 剩余小时
        private long lngLeftDays;       // 剩余天数
        private long lngLeftSeconds;    // 剩余秒数(转化为m天n小时显示)
        private Port port = WorkCurveHelper.deviceMeasure.interfacce.port;

        //public static DllType dllType
        //{
        //    get;
        //    set;
        //}

        #endregion

        #region 属性

        /// <summary>
        /// 是否解锁
        /// </summary>
        public bool IsUnLocked
        {
            get
            {
                return bIsUnLocked;
            }
        }

        /// <summary>
        /// 密码狗检测的时间间隔
        /// </summary>
        public int Interval
        {
            set
            {
                tFoundUpdateDog.Interval = value;
            }
            get
            {
                return tFoundUpdateDog.Interval;
            }
        }

        /// <summary>
        /// 剩余小时
        /// </summary>
        public long LeftHours
        {
            get
            {
                return lngLeftHours;
            }
        }

        /// <summary>
        /// 剩余天数
        /// </summary>
        public long LeftDays
        {
            get
            {
                return lngLeftDays;
            }
        }

        /// <summary>
        /// 剩余秒数
        /// </summary>
        public long LeftSeconds
        {
            get
            {
                return lngLeftSeconds;
            }
        }

        #endregion

        #region 构造器

        /// <summary>
        /// 构造器
        /// </summary>
        public Dogy()
        {
            bIsUnLocked = false;
            lngLeftHours = 0;
            lngLeftDays = 0;

            tFoundUpdateDog = new Timer();  // 密码狗内置时钟
            tFoundUpdateDog.Interval = 12 * 60 * 60 * 1000;     // 默认走时间隔12小时

            tFoundUpdateDog.Tick += new EventHandler(UpdateLeftSeconds);    // 每走时一次更新一次剩余时间
        }

        /// <summary>
        /// 带参构造器
        /// </summary>
        /// <param name="interval">时间间隔</param>
        public Dogy(int interval)
            : this()    // 先调用默认构造器
        {
            tFoundUpdateDog.Interval = interval;
        }

        #endregion

        #region 事件模型：密码狗找不到

        /// <summary>
        /// 委托：未找到密码狗
        /// </summary>
        /// <param name="sender">激发事件的对象</param>
        /// <param name="e">一个EventArgs对象</param>
        public delegate void PasswordDogNotFoundEventHandler(object sender, EventArgs e);

        /// <summary>
        /// 事件：未找到密码狗
        /// </summary>
        public event PasswordDogNotFoundEventHandler PasswordDogNotFound;

        /// <summary>
        /// 当密码狗未找到时的处理
        /// </summary>
        /// <param name="e">一个EventArgs对象</param>
        protected void OnPasswordDogNotFound(EventArgs e)
        {
            if (PasswordDogNotFound != null)
            {
                PasswordDogNotFound(this, e);
            }
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 启动密码狗检测
        /// </summary>
        public void Start()
        {
            // 启动密码狗时先做一次检测
            UpdateLeftSeconds(this, new EventArgs());
            // 然后启动
            tFoundUpdateDog.Start();
        }

        /// <summary>
        /// 停止密码狗检测
        /// </summary>
        public void Stop()
        {
            tFoundUpdateDog.Stop();
        }

        #endregion

        #region 私有方法

        // 定时器Tick事件的回调方法：更新剩余时间
        private void UpdateLeftSeconds(object sender, EventArgs e)
        {
            if (!IsFoundPasswordDog())
            {
                // 如果密码狗未找到 激发未找到密码狗事件
                OnPasswordDogNotFound(e);
            }
        }

        ////田春华 2009、10、28 添加对dll3与dll4的支持

        ////取密码狗的key信息，返回两个值：密码狗存在标志 && 剩余秒数
        //[DllImport("TRUSBDev3.dll", EntryPoint = "GetKeyInfo", CharSet = CharSet.Ansi)]
        //private static extern int GetKeyInfo3(StringBuilder company, StringBuilder mode, StringBuilder serialNum, ref long LeftSencods);

        //[DllImport("TRUSBDev4.dll", EntryPoint = "GetKeyInfo", CharSet = CharSet.Ansi)]
        //private static extern int GetKeyInfo4(StringBuilder company, StringBuilder mode, StringBuilder serialNum, ref long LeftSencods);

        //private static int GetKeyInfo(StringBuilder company, StringBuilder mode, StringBuilder serialNum, ref long LeftSencods)
        //{
        //    int intReturn = 0;
        //    if (dllType == DllType.Dll3)
        //    {
        //        intReturn = GetKeyInfo3(company, mode, serialNum, ref LeftSencods);
        //    }
        //    else if (dllType == DllType.Dll4)
        //    {
        //        intReturn = GetKeyInfo4(company, mode, serialNum, ref LeftSencods);
        //    }
        //    return intReturn;
        //}

        // 判断密码狗是否存在 更新剩余时间
        public Boolean IsFoundPasswordDog()
        {
            // 这三个变量基本没用到，仅作为参数
            StringBuilder arrMode = new StringBuilder(256);
            StringBuilder arrCompany = new StringBuilder(256);
            StringBuilder arrSerialNum = new StringBuilder(256);
            if (0 == port.GetKeyInfo(arrMode, arrCompany, arrSerialNum, ref this.lngLeftSeconds))
            {
                
                if (0 < lngLeftSeconds)     // 剩余秒数大于0时计算剩余天/时数
                {
                    lngLeftDays = lngLeftSeconds / 86400;               // 剩余天数
                    lngLeftHours = (lngLeftSeconds % 86400) / 3600 + 1;     // 剩余小时数（除去天数后）
                    if (lngLeftHours > 0)
                    {
                        lngLeftDays++;
                    }
                }
                else if (-1 == lngLeftSeconds)  // 剩余秒数等于-1时表示解锁
                {
                    bIsUnLocked = true;
                }
                // 这里返回的true值仅代表密码狗是否存在 和天数是否到期没有任何关系
                return true;
            }
            else
            {
                // 找不到密码狗返回false
                return false;
            }
        }

        #endregion
    }
}
