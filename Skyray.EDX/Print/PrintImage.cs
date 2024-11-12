using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Skyray.Print
{
    public class PrintImage : PrintCtrl
    {
        /// <summary>
        /// 图片
        /// </summary>
        private Image _Image;

        public Image Image
        {
            get { return _Image; }
            set
            {
                if (value == null) throw new Exception("Image Can not be null!");
                _Image = value;
            }
        }

        /// <summary>
        /// 绘制图形边框
        /// </summary>
        private bool _DrawImageBorder;
        /// <summary>
        /// 绘制图形边框
        /// </summary>
        public bool DrawImageBorder
        {
            get
            {
                return _DrawImageBorder;
            }
            set
            {
                _DrawImageBorder = value;
                if (Param.ChangeDataSourceValue) NodeInfo.ShowPicBorder = value;
                Invalidate(false);
            }
        }

        /// <summary>
        /// 绘制图形边框的颜色
        /// </summary>
        private Color _ImageBorderColor = Color.Black;
        /// <summary>
        /// 绘制图形边框的颜色
        /// </summary>
        public Color ImageBorderColor
        {
            get
            {
                return _ImageBorderColor;
            }
            set
            {
                _ImageBorderColor = value;
                //_DrawImageBorder = true;
                if (Param.ChangeDataSourceValue) NodeInfo.PicBorderColor = value;
                Invalidate(false);
            }
        }
        /// <summary>
        /// OnPaint事件重载
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (Image != null)
            {
                var rect = new Rectangle(BaseRect.Left,
                    BaseRect.Top + TextSize.Height + TextVSpace,
                    BaseRect.Width, BaseRect.Height - TextSize.Height - TextVSpace);
                
                e.Graphics.DrawImage(Image, rect);
                
                if (DrawImageBorder)//绘制图片边框
                    e.Graphics.DrawRectangle(PrintHelper.GetPen(ImageBorderColor, DashStyle.Solid, 1), rect);
            }
        }
        //public override void CalcSize()
        //{
        //    base.CalcSize();          
        //}
        /// <summary>
        /// 设置大小事件重载
        /// </summary>
        public override void SetSize()
        {
            if (base.Size == Size.Empty)
            {
                base.MinimumSize = new Size(Param.ImageMinSize.Width, Param.ImageMinSize.Height);
                int width = Sqare.Width * 3 / 2 + _Image.Size.Width;
                int height = Sqare.Height * 3 / 2 + _Image.Size.Height + TextVSpace + TextSize.Height;

                if (width > base.MaximumSize.Width)
                {
                    var coff = base.MaximumSize.Width / (double)width;//按照宽度进行缩放
                    int tempHeight = (int)(height * coff);
                    if (tempHeight > base.MaximumSize.Height)//缩放后仍大于最大高度
                    {
                        coff = base.MaximumSize.Height / (double)height;//按照高度缩放
                        width = (int)(width * coff);
                        height = base.MaximumSize.Height;
                    }
                    else
                    {
                        width = base.MaximumSize.Width;
                        height = tempHeight;
                    }
                }

                base.Size = new Size(width, height);
            }
            
            //else
            //{
            //    //base.Width = Math.Max(base.Width, base.TextSize.Width);    //设置宽度
            //}

            var titleSize = new Size(base.TitleRect.Size.Width, base.TextSize.Height + this.TextVSpace);
            base.TitleRect = new Rectangle(base.TitleRect.Location, titleSize);
            //修改：何晓明 2011-02-21
            //原因：Text字体变化时图形控件整体不变
            base.Height = this.Height;//base.TextSize.Height + this.TextVSpace+this.Image.Height;
            //
        }
        /// <summary>
        /// 文本字体改变事件
        /// </summary>
        /// <param name="offsetY"></param>
        public override void TextFontChanged(int offsetY)
        {
            base.Height += offsetY;//高度增加offset
        }
        /// <summary>
        /// 文本垂直间距改变
        /// </summary>
        /// <param name="offsetY"></param>
        public override void TextVSpaceChanged(int offsetY)
        {
            base.Height += offsetY;//高度增加offset
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="panel"></param>
        public PrintImage(PrintPanel panel)
            : base(panel)
        {
            //调用父类构造函数
        }
    }
}
