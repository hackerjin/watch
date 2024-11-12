using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Skyray.Controls
{
    public class Splasher
    {
        /// <summary>
        /// Splasher窗体
        /// </summary>
        static FrmSplash SplashForm = null;
        static Thread MySplashThread = null;

        /// <summary>
        /// 提示窗体背景图片
        /// </summary>
        public static Image BackGroundImage = null;

        /// <summary>
        /// 信息提示标签字体
        /// </summary>
        public static Font LblFont = null;

        /// <summary>
        /// 信息提示标签的位置
        /// </summary>
        public static Point LblLocation = Point.Empty;

        /// <summary>
        /// 信息提示标签的字体颜色
        /// </summary>
        public static Color LblColor = Color.Empty;

        static void ShowThread()
        {
           // SplashForm = new FrmSplash();
            if (BackGroundImage != null) SplashForm.BackgroundImage = BackGroundImage;//设置背景图片
            if (Point.Empty != LblLocation) SplashForm.lblStatusInfo.Location = LblLocation;//设置信息提示标签的位置
            if (LblFont != null) SplashForm.lblStatusInfo.Font = LblFont;//设置信息提示标签的字体
            if (LblColor != Color.Empty) SplashForm.lblStatusInfo.ForeColor = LblColor;//设置信息提示标签的字体颜色     
            Application.Run(SplashForm);
        }


        /// <summary>
        /// 显示启动画面
        /// </summary>
        static public void Show()
        {
            if (MySplashThread != null)
                return;
            SplashForm = new FrmSplash();
            MySplashThread = new Thread(new ThreadStart(Splasher.ShowThread));
            MySplashThread.IsBackground = true;
            MySplashThread.SetApartmentState(ApartmentState.STA);
            MySplashThread.Start();
        }

        /// <summary>
        /// 关闭启动画面
        /// </summary>
        public static void Close()
        {
            if (MySplashThread == null) return;
            if (SplashForm == null) return;

            try
            {
                SplashForm.Invoke(new MethodInvoker(SplashForm.Close));
            }
            catch (Exception)
            {
            }
            MySplashThread = null;
            SplashForm = null;
        }

        /// <summary>
        /// 启动状态文本显示
        /// </summary>
        public static string Status
        {
            set
            {
                if (SplashForm == null)
                {
                    return;
                }

                SplashForm.StatusInfo = value;
            }
            get
            {
                if (SplashForm == null)
                {
                    throw new InvalidOperationException("Splash Form not on screen");
                }
                return SplashForm.StatusInfo;
            }
        }
    }
}
