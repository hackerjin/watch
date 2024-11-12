using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Skyray.Print
{
    public class PrintField : PrintCtrl
    {
        #region 私有字段
        //private Size _ValueTextSize;
        #endregion

        #region 属性
        /// <summary>
        /// 值文本大小
        /// </summary>
        public Size ValueSize { get; set; }
        /// <summary>
        /// 值文本开始绘制的横坐标
        /// </summary>
        /// 
        //修改：何晓明 2011-03-03
        //原因：Field文本无法对齐 Field从模板打开ValueTextStartX位置不对
        public int ValueTextStartX
        {
            get { return BaseRect.Left + TextValueSpace +TextSize.Width; }
            
        }
        //public int ValueTextStartX
        //{
        //    get { return BaseRect.Left + TextValueSpace + TextSize.Width + base.Width - MinimumSize.Width; }            

        //}
        //
        /// <summary>
        /// 文本与文本值的间距
        /// </summary>
        private int _TextValueSpace = 10;
        /// <summary>
        /// 文本与文本值的间距
        /// </summary>
        public int TextValueSpace
        {
            get { return _TextValueSpace; }
            set
            {
                _TextValueSpace = value;
                if (Param.ChangeDataSourceValue) NodeInfo.TextValueSpace = value;
                if (InitFinish) RePaint();
            }
        }
        /// <summary>
        /// 文本值
        /// </summary>
        private string _TextValue;
        /// <summary>
        /// 值，用于字段类型
        /// </summary>
        public string TextValue
        {
            get { return _TextValue; }
            set
            {
                _TextValue = value;
                if (Param.ChangeDataSourceValue) NodeInfo.TextValue = value;
                if (InitFinish) RePaint();
            }
        }

        /// <summary>
        /// 值字体
        /// </summary>
        private Font _TextValueFont;

        /// <summary>
        /// 文本值字体
        /// </summary>
        public Font TextValueFont
        {
            get { return _TextValueFont; }
            set
            {
                _TextValueFont = value;
                if (Param.ChangeDataSourceValue) NodeInfo.TextValueFont = value;
                if (InitFinish) RePaint();
            }
        }
        /// <summary>
        /// 文本值颜色
        /// </summary>
        private Color _TextValueColor = Color.Black;

        /// <summary>
        /// 文本值颜色
        /// </summary>
        public Color TextValueColor
        {
            get { return _TextValueColor; }
            set
            {
                _TextValueColor = value;
                if (Param.ChangeDataSourceValue) NodeInfo.TextValueColor = value;
                Invalidate(false);
            }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="panel"></param>
        public PrintField(PrintPanel panel)
            : base(panel)
        {

        }
        #endregion

        #region 事件重载
        /// <summary>
        /// Onpaint事件重载
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!string.IsNullOrEmpty(TextValue))
            {
                int max = this.Size.Width - this.Sqare.Width/2 - ValueTextStartX ;
                var font = TextValueFont == null ? Font : TextValueFont;
                string stext = base.GetShortText(e.Graphics, TextValue, ValueSize.Width, font, max);
                e.Graphics.DrawString(stext,//文本
                  font, //字体
                  TextValueColor.IsEmpty ? Brushes.Black : new Pen(TextValueColor).Brush, //画刷
                  ValueTextStartX,//坐标X
                  Padding.Top + 1);//坐标Y
            }
        }
        /// <summary>
        /// 计算大小事件重载
        /// </summary>
        public override void CalcSize()
        {
            base.CalcSize();
            using (Graphics g = this.CreateGraphics())
            {
                //文字大小               
                var size = g.MeasureString(TextValue, TextValueFont == null ? Font : TextValueFont);
                ValueSize = new Size(size.Width.GetNearInt(), size.Height.GetNearInt());
            }
        }
        /// <summary>
        /// 设置大小事件重载
        /// </summary>
        public override void SetSize()
        {
            int w = TextSize.Width + TextValueSpace + ValueSize.Width + 10;
            int h = Math.Max(TextSize.Height, ValueSize.Height) + 8;
            base.MinimumSize = new Size(Math.Min(w, base.MaximumSize.Width), h);
            base.MaximumSize = new Size(base.MaximumSize.Width, h);

        }
        #endregion
    }
}
