using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skyray.Controls;

namespace Skyray.Print
{
    public class UIHelper
    {
        //修改：何晓明 2011-01-13
        //原因：定制保存提示信息
        /// <summary>
        /// 保存对话框选中结果
        /// </summary>
        public static DialogResult dlgResult= DialogResult.OK;
        //
        /// <summary>
        /// 获取保存文件名称
        /// </summary>
        /// <param name="filter">字符串型过滤文件信息</param>
        /// <returns></returns>
        public static string GetFileToSaveName(string filter)
        {
            SaveFileDialog sdlg = new SaveFileDialog();
            sdlg.Filter = filter;
            sdlg.InitialDirectory = Application.StartupPath+"\\Report";
            sdlg.RestoreDirectory = true;
            //修改：何晓明 2011-01-13
            //原因：定制保存提示信息
            sdlg.FileOk += new System.ComponentModel.CancelEventHandler(sdlg_FileOk);
            sdlg.OverwritePrompt = false;
            //
            sdlg.ShowDialog();
            if (dlgResult == DialogResult.OK)
            {
                return sdlg.FileName;
            }
            return string.Empty;
        }
        //修改：何晓明 2011-01-13
        //原因：定制保存提示信息
        static void sdlg_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (System.IO.File.Exists(((SaveFileDialog)sender).FileName))
            {
                String _strWarn = PrintInfo.File +" "+ ((SaveFileDialog)sender).FileName+" " +PrintInfo.FileExists+ PrintInfo.OverWrite;
                //if(DialogResult.Yes==MessageBox.Show(_strWarn ,((SaveFileDialog)sender).Title,MessageBoxButtons.YesNo,MessageBoxIcon .Warning,MessageBoxDefaultButton.Button1))
                if(DialogResult.Yes==Skyray.Controls.SkyrayMsgBox.Show(_strWarn,PrintInfo.Tip,MessageBoxButtons.YesNo,MessageBoxIcon.Warning,MessageBoxDefaultButton.Button1))
                {
                    dlgResult = DialogResult.OK;
                }
                else
                {
                    dlgResult = DialogResult.Cancel;
                }
            }
            else
            {
                dlgResult = DialogResult.OK;
            }
        }
        //
        public static void OpenFile(string FileName)
        {
            Help.ShowHelp(null, FileName);
        }

        public static void AskToOpenFile(string FileName)
        {
            if (DialogResult.Yes == SkyrayMsgBox.Show(PrintInfo.OpenNow,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                OpenFile(FileName);//打开
            }
        }
    }
}
