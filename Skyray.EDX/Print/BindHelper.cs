using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Skyray.Print
{
    /// <summary>
    /// 绑定帮助类
    /// </summary>
    public static class BindHelper
    {
        /// <summary>
        /// 绑定至控件
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="ctrlProperty"></param>
        /// <param name="objDataSource"></param>
        /// <param name="fieldName"></param>
        /// <param name="changeAtOnce"></param>
        /// <returns></returns>
        public static Binding BindToCtrl(Control ctrl, string ctrlProperty, object objDataSource, string fieldName, bool changeAtOnce)
        {
            ctrl.DataBindings.Clear();
            var flag = changeAtOnce ? DataSourceUpdateMode.OnPropertyChanged : DataSourceUpdateMode.Never;
            Binding binding = new Binding(ctrlProperty, objDataSource, fieldName, true, flag);
            ctrl.DataBindings.Add(binding);
            return binding;
        }
        /// <summary>
        /// 绑定Value值控件
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="objDataSource"></param>
        /// <param name="fieldName"></param>
        /// <param name="changeAtOnce"></param>
        /// <returns></returns>
        public static Binding BindValueToCtrl(Control ctrl, object objDataSource, string fieldName, bool changeAtOnce)
        {
            return BindToCtrl(ctrl, "Value", objDataSource, fieldName, changeAtOnce);
        }
        /// <summary>
        /// 绑定Text属性至控件
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="objDataSource"></param>
        /// <param name="fieldName"></param>
        /// <param name="changeAtOnce"></param>
        /// <returns></returns>
        public static Binding BindTextToCtrl(Control ctrl, object objDataSource, string fieldName, bool changeAtOnce)
        {
            return BindToCtrl(ctrl, "Text", objDataSource, fieldName, changeAtOnce);
        }
        /// <summary>
        /// 绑定Checked属性至控件
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="objDataSource"></param>
        /// <param name="fieldName"></param>
        /// <param name="changeAtOnce"></param>
        /// <returns></returns>
        public static Binding BindCheckedToCtrl(Control ctrl, object objDataSource, string fieldName, bool changeAtOnce)
        {
            return BindToCtrl(ctrl, "Checked", objDataSource, fieldName, changeAtOnce);
        }
    }
}
