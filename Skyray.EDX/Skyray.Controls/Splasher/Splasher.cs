using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Skyray.Controls
{
    public class Splasher
    {
        /// <summary>
        /// Splasher����
        /// </summary>
        static FrmSplash SplashForm = null;
        static Thread MySplashThread = null;

        /// <summary>
        /// ��ʾ���屳��ͼƬ
        /// </summary>
        public static Image BackGroundImage = null;

        /// <summary>
        /// ��Ϣ��ʾ��ǩ����
        /// </summary>
        public static Font LblFont = null;

        /// <summary>
        /// ��Ϣ��ʾ��ǩ��λ��
        /// </summary>
        public static Point LblLocation = Point.Empty;

        /// <summary>
        /// ��Ϣ��ʾ��ǩ��������ɫ
        /// </summary>
        public static Color LblColor = Color.Empty;

        static void ShowThread()
        {
           // SplashForm = new FrmSplash();
            if (BackGroundImage != null) SplashForm.BackgroundImage = BackGroundImage;//���ñ���ͼƬ
            if (Point.Empty != LblLocation) SplashForm.lblStatusInfo.Location = LblLocation;//������Ϣ��ʾ��ǩ��λ��
            if (LblFont != null) SplashForm.lblStatusInfo.Font = LblFont;//������Ϣ��ʾ��ǩ������
            if (LblColor != Color.Empty) SplashForm.lblStatusInfo.ForeColor = LblColor;//������Ϣ��ʾ��ǩ��������ɫ     
            Application.Run(SplashForm);
        }


        /// <summary>
        /// ��ʾ��������
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
        /// �ر���������
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
        /// ����״̬�ı���ʾ
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
