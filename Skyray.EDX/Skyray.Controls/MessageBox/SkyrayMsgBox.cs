using System.Windows.Forms;

namespace Skyray.Controls
{
    /// <summary>
    /// 天瑞公用自定义消息框
    /// </summary>
    public sealed class SkyrayMsgBox
    {
        public static MessageWindow MsgWin = null;

        static DialogResult ShowWindow(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultbutton, bool IsModelDlg)
        {
            if (MsgWin == null)
                MsgWin = new MessageWindow();
            MsgWin.StartPosition = FormStartPosition.CenterScreen;
            //MsgWin.DrawWindow(owner, text, caption, buttons, icon, defaultbutton, IsModelDlg);
            DialogResult result = MsgWin.DrawWindow(owner, text, caption, buttons, icon, defaultbutton, IsModelDlg);
            //if (IsModelDlg)
            //{
            //    result = MsgWin.DrawWindow(owner, text, caption, buttons, icon, defaultbutton, IsModelDlg);
            //}
            //else
            //{
            //    MsgWin.DrawWindow(owner, text, caption, buttons, icon, defaultbutton, IsModelDlg);
            //    result = MsgWin.Res;               
            //}
            //MsgWin.Close();

            //MsgWin.Close();

            return result;
        }


        // 自定义消息框显示方法
        static DialogResult ShowWindow(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultbutton)
        {

            return ShowWindow(owner, text, caption, buttons, icon, defaultbutton, true);
        }

        #region Msg Box Show


        public static DialogResult ShowModellessDialog(string text, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return ShowWindow(null, text, CommonsInfo.strMsgTitle, buttons, icon, MessageBoxDefaultButton.Button1, false);
        }


        public static DialogResult Show(string text, MessageBoxIcon icon)
        {
            return ShowWindow(null, text, CommonsInfo.strMsgTitle, MessageBoxButtons.OK, icon, MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// 显示具有指定文本的消息框
        /// </summary>
        /// <param name="text">要在消息框中显示的文本</param>
        /// <returns>DialogResult值之一</returns>
        public static DialogResult Show(string text)
        {
            return ShowWindow(null, text, CommonsInfo.strMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// 在指定对象的前面显示具有指定文本的消息框
        /// </summary>
        /// <param name="owner">指定对象</param>
        /// <param name="text">要在消息框中显示的文本</param>
        /// <returns>DialogResult值之一</returns>
        public static DialogResult Show(IWin32Window owner, string text)
        {
            return ShowWindow(null, text, "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// 显示具有指定文本和标题的消息框
        /// </summary>
        /// <param name="text">要在消息框中显示的文本</param>
        /// <param name="caption">要在消息框的标题栏中显示的文本</param>
        /// <returns>DialogResult值之一</returns>
        public static DialogResult Show(string text, string caption)
        {
            return ShowWindow(null, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// 在指定对象的前面显示具有指定文本和标题的消息框
        /// </summary>
        /// <param name="owner">指定对象</param>
        /// <param name="text">要在消息框中显示的文本</param>
        /// <param name="caption">要在消息框的标题栏中显示的文本</param>
        /// <returns>DialogResult值之一</returns>
        public static DialogResult Show(IWin32Window owner, string text, string caption)
        {
            return ShowWindow(owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// 显示具有指定文本、标题和按钮的消息框
        /// </summary>
        /// <param name="text">要在消息框中显示的文本</param>
        /// <param name="caption">要在消息框的标题栏中显示的文本</param>
        /// <param name="buttons">MessageBoxButtons值之一，可指定在消息框中显示哪些按钮</param>
        /// <returns>DialogResult值之一</returns>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
        {
            return ShowWindow(null, text, caption, buttons, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// 在指定对象的前面显示具有指定文本、标题和按钮的消息框
        /// </summary>
        /// <param name="owner">指定对象</param>
        /// <param name="text">要在消息框中显示的文本</param>
        /// <param name="caption">要在消息框的标题栏中显示的文本</param>
        /// <param name="buttons">MessageBoxButtons值之一，可指定在消息框中显示哪些按钮</param>
        /// <returns>DialogResult值之一</returns>
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
        {
            return ShowWindow(owner, text, caption, buttons, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// 显示具有指定文本、标题、按钮和图标的消息框
        /// </summary>
        /// <param name="text">要在消息框中显示的文本</param>
        /// <param name="caption">要在消息框的标题栏中显示的文本</param>
        /// <param name="buttons">MessageBoxButtons值之一，可指定在消息框中显示哪些按钮</param>
        /// <param name="icon">MessageBoxIcon值之一，它指定在消息框中显示哪个图标</param>
        /// <returns>DialogResult值之一</returns>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return ShowWindow(null, text, caption, buttons, icon, MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// 在指定对象的前面显示具有指定文本、标题、按钮和图标的消息框
        /// </summary>
        /// <param name="owner">指定对象</param>
        /// <param name="text">要在消息框中显示的文本</param>
        /// <param name="caption">要在消息框的标题栏中显示的文本</param>
        /// <param name="buttons">MessageBoxButtons值之一，可指定在消息框中显示哪些按钮</param>
        /// <param name="icon">MessageBoxIcon值之一，它指定在消息框中显示哪个图标</param>
        /// <returns>DialogResult值之一</returns>
        public static DialogResult Show(string text, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return ShowWindow(null, text, CommonsInfo.strMsgTitle, buttons, icon, MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// 在指定对象的前面显示具有指定文本、标题、按钮和图标的消息框
        /// </summary>
        /// <param name="owner">指定对象</param>
        /// <param name="text">要在消息框中显示的文本</param>
        /// <param name="caption">要在消息框的标题栏中显示的文本</param>
        /// <param name="buttons">MessageBoxButtons值之一，可指定在消息框中显示哪些按钮</param>
        /// <param name="icon">MessageBoxIcon值之一，它指定在消息框中显示哪个图标</param>
        /// <returns>DialogResult值之一</returns>
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return ShowWindow(owner, text, caption, buttons, icon, MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// 显示具有指定文本、标题、按钮、图标和默认按钮的消息框
        /// </summary>
        /// <param name="text">要在消息框中显示的文本</param>
        /// <param name="caption">要在消息框的标题栏中显示的文本</param>
        /// <param name="buttons">MessageBoxButtons值之一，可指定在消息框中显示哪些按钮</param>
        /// <param name="icon">MessageBoxIcon值之一，它指定在消息框中显示哪个图标</param>
        /// <param name="defaultButton">MessageBoxDefaultButton值之一，可指定消息框中的默认按钮</param>
        /// <returns>DialogResult值之一</returns>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return ShowWindow(null, text, caption, buttons, icon, defaultButton);
        }

        /// <summary>
        /// 在指定对象的前面显示具有指定文本、标题、按钮、图标和默认按钮的消息框
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="text">要在消息框中显示的文本</param>
        /// <param name="caption">要在消息框的标题栏中显示的文本</param>
        /// <param name="buttons">MessageBoxButtons值之一，可指定在消息框中显示哪些按钮</param>
        /// <param name="icon">MessageBoxIcon值之一，它指定在消息框中显示哪个图标</param>
        /// <param name="defaultButton">MessageBoxDefaultButton值之一，可指定消息框中的默认按钮</param>
        /// <returns>DialogResult值之一</returns>
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return ShowWindow(owner, text, caption, buttons, icon, defaultButton);
        }
        #endregion
    }
}
