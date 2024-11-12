using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Skyray.Controls;

namespace Skyray.EDX.Common
{
    /// <summary>
    /// 天瑞公用自定义消息框
    /// </summary>
    public class Msg
    {
        #region Msg Box Show

        public static DialogResult Show(string text, MessageBoxIcon icon)
        {
            return SkyrayMsgBox.Show(text, icon);
        }

        /// <summary>
        /// 显示具有指定文本的消息框
        /// </summary>
        /// <param name="text">要在消息框中显示的文本</param>
        /// <returns>DialogResult值之一</returns>
        public static DialogResult Show(string text)
        {
            return SkyrayMsgBox.Show(text);
        }

        /// <summary>
        /// 在指定对象的前面显示具有指定文本的消息框
        /// </summary>
        /// <param name="owner">指定对象</param>
        /// <param name="text">要在消息框中显示的文本</param>
        /// <returns>DialogResult值之一</returns>
        public static DialogResult Show(IWin32Window owner, string text)
        {
            return SkyrayMsgBox.Show(owner, text);
        }

        /// <summary>
        /// 显示具有指定文本和标题的消息框
        /// </summary>
        /// <param name="text">要在消息框中显示的文本</param>
        /// <param name="caption">要在消息框的标题栏中显示的文本</param>
        /// <returns>DialogResult值之一</returns>
        public static DialogResult Show(string text, string caption)
        {
            return SkyrayMsgBox.Show(text, caption);
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
            return SkyrayMsgBox.Show(owner, text, caption);
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
            return SkyrayMsgBox.Show(text, caption, buttons);
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
            return SkyrayMsgBox.Show(owner, text, caption, buttons);
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
            return SkyrayMsgBox.Show(text, caption, buttons, icon);
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
            return SkyrayMsgBox.Show(text, buttons, icon);
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
            return SkyrayMsgBox.Show(owner, text, caption, buttons, icon);
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
            return SkyrayMsgBox.Show(text, caption, buttons, icon, defaultButton);
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
            return SkyrayMsgBox.Show(owner, text, caption, buttons, icon, defaultButton);
        }

        #endregion

    }
}
