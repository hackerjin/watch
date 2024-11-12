using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace Skyray.EDX.Common.Component
{
    public class JapanStyle : INotifyPropertyChanged  
    {
        private Color _backColor;
        /// <summary>
        /// 背景颜色
        /// </summary>
        public Color BackColor
        {
            get
            {
                return _backColor;
            }
            set
            {
                if (_backColor != value)
                {
                   _backColor = value;
                    RaisePropertyChanged("BackColor");
                }
            }
        }

        private Color _foreColor;
        /// <summary>
        /// 字体颜色
        /// </summary>
        public Color ForeColor
        {
            get
            {
                return _foreColor;
            }
            set
            {
                if (_foreColor != value)
                {
                    _foreColor =value;
                    RaisePropertyChanged("ForeColor");
                }
            }
        }

        private Font _style;
        public Font Style 
        {
            get
            {
                return _style;
            }
            set
            {
                if (_style != value)
                {
                    _style = value;
                    RaisePropertyChanged("Style");
                }
            }
         }

        public static JapanStyle Default()
        {
                return new JapanStyle() {_backColor = Color.Transparent, _foreColor = Color.Black, _style = new Font("宋体",72, FontStyle.Bold) };
        }

        public string ParseToString()
        {
            return _backColor.ToArgb() + "," + _foreColor.ToArgb() + "," + _style.Size;
        }

        public static JapanStyle ParseFromString(string str)
        {
            try
            {
                string[] strArray = str.Split(',');
                return new JapanStyle()
                {
                    _backColor = Color.FromArgb(int.Parse(strArray.ElementAt(0))),
                    _foreColor = Color.FromArgb(int.Parse(strArray.ElementAt(1))),
                    _style = new Font("宋体", float.Parse(strArray.ElementAt(2)), FontStyle.Bold)
                };
            }
            catch (Exception e)
            {
                return JapanStyle.Default();
            }
        }



        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }  

        #endregion  
    }
}
