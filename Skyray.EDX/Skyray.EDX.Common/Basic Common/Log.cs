using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Skyray.Controls;

namespace Skyray.EDX.Common
{
    /// <summary>
    /// 日志记录帮助类
    /// </summary>
    public class Log
    {
        static log4net.ILog LogInfo;
        static log4net.ILog LogError;
        public static bool Inited = false;//是否已经初始化

        public static void InitLog()
        {
            LogError = log4net.LogManager.GetLogger("LogError");
            LogInfo = log4net.LogManager.GetLogger("LogInfo");

            log4net.Config.XmlConfigurator.Configure();
            Inited = true;
        }

        /// <summary>
        /// 写信息日志
        /// </summary>
        /// <param name="objInfo"></param>
        public static void Info(object objInfo)
        {
            LogInfo.Info(objInfo);
        }

        /// <summary>
        /// 写信息日志
        /// </summary>
        /// <param name="objInfo"></param>
        public static void Info(object objInfo, Exception ex)
        {
            LogInfo.Info(objInfo, ex);
        }

        /// <summary>
        /// 写错误日志
        /// </summary>
        /// <param name="objInfo"></param>
        public static void Error(object objInfo)
        {
            LogError.Error(objInfo);
        }
        /// <summary>
        /// 写错误日志
        /// </summary>
        /// <param name="objInfo"></param>
        /// <param name="ex"></param>
        public static void Error(object objInfo, Exception ex)
        {
            LogError.Error(objInfo, ex);
        }
    }
}
