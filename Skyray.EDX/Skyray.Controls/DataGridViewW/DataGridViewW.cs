using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using Skyray.Controls.XPander;
using System.ComponentModel;
using Aspose.Cells;
using Printing.DataGridViewPrint.Tools;
using System.Drawing.Printing;
using System.Data;
using System.Reflection;
using System.Linq;


namespace Skyray.Controls
{
    //部分代码只适用于Net2.0 
    [ToolboxBitmap(typeof(DataGridViewW), "Bitmap.GridViewControl.bmp")] //定义在工具箱里显示图标,可以去掉
    /**/
    /// <summary>
    /// GridViewControl是一个可定义部分外观的DataGridView
    /// 注意要想使得自画的背景有效,得把DataGridViewa相应的颜色属性设为Color.Transparent,这个在代码里没处理
    /// </summary>
    public class DataGridViewW : DataGridView, ISkyrayStyle
    {
        /// <summary>
        /// 需要打印的列索引集合
        /// </summary>
        public List<int> ToPrintCols { get; set; }

        /// <summary>
        /// 需要打印的行索引集合
        /// </summary>
        public List<int> ToPrintRows { get; set; }

        /// <summary>
        /// 打印文档对象
        /// </summary>
        public PrintDocument PDC { get; set; }

        #region Fields

        private Color _ColumnHeaderColor1 = Color.FromArgb(227, 239, 255);
        private Color _ColumnHeaderColor2 = Color.FromArgb(175, 210, 255);
        private Color _SelectedRowColor1 = Color.White;

        // private Color _SelectedRowColor2 = Color.FromArgb(171, 217, 254);
        private Color _SelectedRowColor2 = Color.FromArgb(220, 237, 206);
        private Color _PrimaryRowColor1 = Color.White;
        private Color _PrimaryRowColor2 = Color.FromArgb(255, 249, 232);
        private Color _SecondaryRowColor1 = Color.White;
        private Color _SecondaryRowColor2 = Color.Black;
        private ContextMenuStrip cmsExport;
        private IContainer components;
        private ToolStripMenuItem tsmiExprotToExcel;
        private int _SecondaryLength = 1;

        #endregion

        private ToolStripMenuItem tsmiPrintPreview;
        private ToolStripMenuItem tsmiPrint;

        #region Properties

        public ContextMenuStrip CMenu
        {
            get
            {
                return this.cmsExport;
            }
        }

        private bool _ShowEportContextMenu = false;
        public bool ShowEportContextMenu
        {
            get { return _ShowEportContextMenu; }
            set { _ShowEportContextMenu = value; }
        }

        public Color ColumnHeaderColor1 //表头起始颜色
        {
            get { return _ColumnHeaderColor1; }
            set { _ColumnHeaderColor1 = value; this.Invalidate(); }
        }

        public Color ColumnHeaderColor2 //表头终止颜色
        {
            get { return _ColumnHeaderColor2; }
            set
            {
                _ColumnHeaderColor2 = value;
                this.Invalidate();
            }
        }

        public Color PrimaryRowcolor1 //奇行起始颜色
        {
            get { return _PrimaryRowColor1; }
            set
            {
                if (value.IsEmpty || value == Color.Transparent)
                    _PrimaryRowColor1 = Color.White;
                else
                    _PrimaryRowColor1 = value;
            }
        }

        public Color PrimaryRowcolor2//奇行终止颜色
        {
            get { return _PrimaryRowColor2; }
            set
            {
                if (value.IsEmpty || value == Color.Transparent)
                    _PrimaryRowColor2 = Color.White;
                else
                    _PrimaryRowColor2 = value;
            }
        }

        public Color SecondaryRowColor1//偶行起始颜色
        {
            get { return _SecondaryRowColor1; }
            set
            {
                if (value.IsEmpty || value == Color.Transparent)
                    _SecondaryRowColor1 = Color.White;
                else
                    _SecondaryRowColor1 = value;
            }
        }

        public Color SecondaryRowColor2//偶行起始颜色
        {
            get { return _SecondaryRowColor2; }
            set
            {
                if (value.IsEmpty || value == Color.Transparent)
                    _SecondaryRowColor2 = Color.White;
                else
                    _SecondaryRowColor2 = value;
            }
        }

        public int SecondaryLength //这个长度现在是指导隔多少个行出现一个偶行
        {
            get { return _SecondaryLength; }
            set { _SecondaryLength = value; }
        }
        [Browsable(false)]
        public Color SelectedRowColor1 //选中行起始颜色
        {
            get { return _SelectedRowColor1; }
            set { _SelectedRowColor1 = value; }
        }
        [Browsable(false)]
        public Color SelectedRowColor2 //选中行终止颜色
        {
            get { return _SelectedRowColor2; }
            set { _SelectedRowColor2 = value; }
        }

        //public new DataGridViewColumnHeadersHeightSizeMode ColumnHeadersHeightSizeMode
        //{
        //    get { return base.ColumnHeadersHeightSizeMode; }
        //    set
        //    {
        //        base.ColumnHeadersHeightSizeMode = value;
        //        base.ColumnHeadersHeight += 10;
        //    }
        //}
        public new int ColumnHeadersHeight
        {
            get { return base.ColumnHeadersHeight; }
            set
            {
                if (value > 24)
                {
                    base.ColumnHeadersHeight = value;
                }
                else
                {
                    base.ColumnHeadersHeight = 24;
                }
            }

        }



        /// <summary>
        /// 属性重载
        /// </summary>
        public new bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                // this.BackgroundColor = this.RowTemplate.DefaultCellStyle.BackColor = value ? Color.White : Color.LightGray;
                this.Style = value ? Style.Office2007Blue : Style.Office2007Sliver;

            }
        }
        #endregion


        private void Init()
        {
            this.RowHeadersVisible = false;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.AllowUserToResizeRows = false;
            this.BorderStyle = BorderStyle.None;
            this.RowTemplate.DefaultCellStyle.SelectionBackColor = Color.Transparent;
            this.RowTemplate.DefaultCellStyle.SelectionForeColor = Color.Black;
            this.BackgroundColor = Color.FromArgb(234, 247, 254);
            //this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.RowPrePaint += new DataGridViewRowPrePaintEventHandler(this.GridView_RowPrePaint);
            this.CellPainting += new DataGridViewCellPaintingEventHandler(this.GridView_CellPainting);
            //this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DataGridViewW_MouseClick);
            PDC = new PrintDocument();
            this.cmsExport.Items[0].Visible = this.cmsExport.Items[1].Visible = this.cmsExport.Items[2].Visible = false;
        }

        public DataGridViewW()
        {
            SetStyle(
            ControlStyles.SupportsTransparentBackColor |
            ControlStyles.OptimizedDoubleBuffer
                 | ControlStyles.UserPaint
                 | ControlStyles.AllPaintingInWmPaint
                 | ControlStyles.ResizeRedraw, true);
            InitializeComponent();
            Init();
        }

        private static void DrawLinearGradient(Rectangle Rec, Graphics Grp, Color Color1, Color Color2)
        {
            if (Color1 == Color2)
            {
                Brush backbrush = new SolidBrush(Color1);
                Grp.FillRectangle(backbrush, Rec);
            }
            else
            {
                using (Brush backbrush = new LinearGradientBrush(Rec, Color1, Color2, LinearGradientMode.Vertical))
                {
                    Grp.FillRectangle(backbrush, Rec);
                }
            }
        }

        //private void DrawSelectRow(Rectangle Rec, Graphics Grp)
        //{
        //    ColorBlend mix = new ColorBlend();
        //    Color[] colors = ColorHelper.colors1;

        //    mix.Colors = new Color[] { colors[0], colors[1], colors[2], colors[3] };

        //    mix.Positions = new float[] { 0.0F, 0.3F, 0.35F, 1.0F };

        //    var lgbrush =
        //        new LinearGradientBrush(Rec,
        //            Color.Transparent,
        //            Color.Transparent,
        //            LinearGradientMode.Vertical);

        //    lgbrush.InterpolationColors = mix;
        //    Grp.FillRectangle(lgbrush, Rec);

        //}
        private void GridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                if (!(_ColumnHeaderColor1 == Color.Transparent) && !(_ColumnHeaderColor2 == Color.Transparent) &&
                    !_ColumnHeaderColor1.IsEmpty && !_ColumnHeaderColor2.IsEmpty)
                {
                    DrawLinearGradient(e.CellBounds, e.Graphics, _ColumnHeaderColor1, _ColumnHeaderColor2);
                    e.Paint(e.ClipBounds, (DataGridViewPaintParts.All & ~DataGridViewPaintParts.Background));
                    e.Handled = true;
                }
            }
            else if ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
            {
                if (this.RowTemplate.DefaultCellStyle.SelectionBackColor == Color.Transparent)
                {
                    //DrawLinearGradient(e.CellBounds, e.Graphics, _SelectedRowColor1, _SelectedRowColor2);
                    //DrawSelectRow(e.CellBounds, e.Graphics);
                    GraphHelper.FillRect(e.CellBounds, e.Graphics);
                }
            }
        }

        private void RenderButtonBackground(Graphics graphics, Rectangle bounds, Color colorGradientBegin, Color colorGradientMiddle, Color colorGradientEnd)
        {
            RectangleF upperRectangle = bounds;
            upperRectangle.Height = bounds.Height * 0.4f;

            using (LinearGradientBrush upperLinearGradientBrush = new LinearGradientBrush(
                    upperRectangle,
                    colorGradientBegin,
                    colorGradientMiddle,
                    LinearGradientMode.Vertical))
            {
                if (upperLinearGradientBrush != null)
                {
                    Blend blend = new Blend();
                    blend.Positions = new float[] { 0.0F, 1.0F };
                    blend.Factors = new float[] { 0.0F, 0.6F };
                    upperLinearGradientBrush.Blend = blend;
                    graphics.FillRectangle(upperLinearGradientBrush, upperRectangle);
                }
            }

            RectangleF lowerRectangle = bounds;
            lowerRectangle.Y = upperRectangle.Height;
            lowerRectangle.Height = bounds.Height - upperRectangle.Height;

            using (LinearGradientBrush lowerLinearGradientBrush = new LinearGradientBrush(
                    lowerRectangle,
                    colorGradientMiddle,
                    colorGradientEnd,
                    LinearGradientMode.Vertical))
            {
                if (lowerLinearGradientBrush != null)
                {
                    graphics.FillRectangle(lowerLinearGradientBrush, lowerRectangle);
                }
            }
            RectangleF correctionRectangle = lowerRectangle;
            correctionRectangle.Y -= 1;
            correctionRectangle.Height = 2;
            using (SolidBrush solidBrush = new SolidBrush(colorGradientMiddle))
            {
                graphics.FillRectangle(solidBrush, correctionRectangle);
            }
        }

        private void GridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            Rectangle rowBounds = new Rectangle(0, e.RowBounds.Top, this.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) -
                                                  this.HorizontalScrollingOffset + 1, e.RowBounds.Height + 1);

            e.PaintParts &= ~DataGridViewPaintParts.Focus;
            if ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
            {
                if (this.RowTemplate.DefaultCellStyle.SelectionBackColor == Color.Transparent)
                {
                    //DrawLinearGradient(rowBounds, e.Graphics, _SelectedRowColor1, _SelectedRowColor2);
                    //DrawSelectRow(rowBounds, e.Graphics);
                    GraphHelper.FillRect(rowBounds, e.Graphics);
                }
            }
            else
            {
                if (this.RowTemplate.DefaultCellStyle.BackColor == Color.Transparent)
                {
                    if (e.RowIndex % _SecondaryLength == 1)
                    {
                        DrawLinearGradient(rowBounds, e.Graphics, _PrimaryRowColor1, _PrimaryRowColor2);
                    }
                    else
                    {
                        DrawLinearGradient(rowBounds, e.Graphics, _SecondaryRowColor1, _SecondaryRowColor2);
                    }
                }
            }
        }

        #region ISkyrayStyle Members

        private Style _Style = Style.Office2007Blue;

        public Style Style
        {
            get
            {
                return _Style;
            }
            set
            {
                _Style = value;
                SetStyle(_Style);
            }
        }

        public void SetStyle(Style style)
        {
            switch (style)
            {
                case Style.Office2007Blue:
                    _ColumnHeaderColor1 = Color.FromArgb(227, 239, 255);
                    _ColumnHeaderColor2 = Color.FromArgb(175, 210, 255);
                    this.Invalidate();
                    break;
                case Style.Office2007Sliver:
                    _ColumnHeaderColor1 = Color.FromArgb(240, 240, 240);
                    _ColumnHeaderColor2 = Color.LightGray;

                    this.Invalidate();
                    break;

                default: break;
            }
        }

        #endregion

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cmsExport = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiExprotToExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPrintPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsExport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // cmsExport
            // 
            this.cmsExport.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiExprotToExcel,
            this.tsmiPrintPreview,
            this.tsmiPrint});
            this.cmsExport.Name = "cmsExport";
            this.cmsExport.Size = new System.Drawing.Size(125, 70);
            // 
            // tsmiExprotToExcel
            // 
            this.tsmiExprotToExcel.Name = "tsmiExprotToExcel";
            this.tsmiExprotToExcel.Size = new System.Drawing.Size(124, 22);
            this.tsmiExprotToExcel.Text = "导出Excel";
            this.tsmiExprotToExcel.Click += new System.EventHandler(this.tsmiExprotToExcel_Click);
            // 
            // tsmiPrintPreview
            // 
            this.tsmiPrintPreview.Name = "tsmiPrintPreview";
            this.tsmiPrintPreview.Size = new System.Drawing.Size(124, 22);
            this.tsmiPrintPreview.Text = "打印预览";
            this.tsmiPrintPreview.Click += new System.EventHandler(this.tsmiPrintPreview_Click);
            // 
            // tsmiPrint
            // 
            this.tsmiPrint.Name = "tsmiPrint";
            this.tsmiPrint.Size = new System.Drawing.Size(124, 22);
            this.tsmiPrint.Text = "打印";
            this.tsmiPrint.Click += new System.EventHandler(this.tsmiPrint_Click);
            // 
            // DataGridViewW
            // 
            this.RowTemplate.Height = 23;
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DataGridViewW_MouseClick);
            this.cmsExport.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }


        public string ExportExcel_Public_SpecialRow(out bool hasRecord)
        {
            Workbook workbook = new Workbook();
            Cells cells = workbook.Worksheets[0].Cells;
            string savePath = string.Empty;
            hasRecord = false;
            int k = 0;
            for (int j = 1; j < this.Columns.Count; j++)
            {
                var col = this.Columns[j];
                if (col.Visible)
                {
                    var cell = cells[0, k];
                    cell.PutValue(col.HeaderText);
                    int row = 0;
                    for (int i = 0; i < this.Rows.Count; i++)
                    {
                        var obj = this[0, i].Value;
                        if (obj == null|| !bool.Parse(obj.ToString()))
                            continue;
                        if (obj.GetType() == typeof(bool) && bool.Parse(obj.ToString()))
                        {
                            cell = cells[row + 1, k];
                            Aspose.Cells.Style styletemp = cell.GetDisplayStyle();
                            styletemp.Font.Color = this[j, i].Style.ForeColor;
                            obj = this[j, i].Value;                            
                            if (obj != null)
                            {
                                var typ = obj.GetType();
                                if (typ == typeof(DateTime))
                                {
                                    styletemp.Custom = "yyyy-mm-dd hh:mm:ss";
                                    cell.PutValue(obj.ToString());
                                }
                                else if (typ == typeof(string))
                                {
                                    var str = obj.ToString();
                                    if (str.IsNum())
                                        cell.PutValue(double.Parse(str));
                                    else
                                        cell.PutValue(str);
                                }
                                else
                                {
                                    cell.PutValue(obj);
                                }
                                hasRecord = true;
                            }
                            cell.SetStyle(styletemp);
                        }
                        row++;
                    }
                    k++;
                }
            }
            if (hasRecord)
            {
                SaveFileDialog sdlg = new SaveFileDialog();
                sdlg.Filter = "Excel File(*.xls)|*.xls";
                if (sdlg.ShowDialog() == DialogResult.OK)
                {                      

                    workbook.Save(sdlg.FileName);
                    
                    savePath = sdlg.FileName;
                }
            }
            return savePath;
        }

        /// <summary>
        /// datagridview导出到Excel
        /// </summary>
        /// <param name="ischeckcolumn">当前datagridView是否存在勾选列</param>
        /// <param name="hasRecord">判断当前是否存在导出的记录</param>
        /// <param name="outWorkbook">返回生成的excel对象</param>
        /// <returns></returns>
        public string ExportExcel_Public_SpecialRow(bool ischeckcolumn,out bool hasRecord, out Workbook outWorkbook)
        {
            Workbook workbook = new Workbook();
            Cells cells = workbook.Worksheets[0].Cells;
            string savePath = string.Empty;
            hasRecord = false;
            int k = 0;
            for (int j = (ischeckcolumn) ? 1 : 0; j < this.Columns.Count; j++)
            {
                var col = this.Columns[j];
                if (col.Visible)
                {
                    var cell = cells[0, k];
                    Aspose.Cells.Style style=cell.GetDisplayStyle();
                    style.HorizontalAlignment=TextAlignmentType.Center;
                    cell.SetStyle(style);
                    cell.PutValue(col.HeaderText);
                    int row = 0;
                    for (int i = 0; i < this.Rows.Count; i++)
                    {
                        var obj = this[0, i].Value;
                        if (ischeckcolumn)
                        {
                            if (obj == null || !bool.Parse(obj.ToString()))
                                continue;
                            if (obj.GetType() == typeof(bool) && bool.Parse(obj.ToString()))
                            {
                                cell = cells[row + 1, k];
                                Aspose.Cells.Style styletemp = cell.GetDisplayStyle();
                                styletemp.Font.Color = this[j, i].Style.ForeColor;
                                styletemp.Font.IsBold = this[j, i].Style.Font != null ? this[j, i].Style.Font.Bold : false;
                                
                                obj = this[j, i].Value;
                                if (obj != null)
                                {
                                    var typ = obj.GetType();
                                    if (typ == typeof(DateTime))
                                    {
                                        styletemp.Custom = "yyyy-mm-dd hh:mm:ss";
                                        cell.PutValue(obj.ToString());
                                    }
                                    else if (typ == typeof(string))
                                    {
                                        var str = obj.ToString();
                                        if (str.IsNum())
                                            cell.PutValue(double.Parse(str));
                                        else
                                            cell.PutValue(str);
                                    }
                                    else
                                    {
                                        cell.PutValue(obj);
                                    }
                                    hasRecord = true;
                                    if (this.Columns[j].Name == "ReportPath")
                                        workbook.Worksheets[0].Hyperlinks.Add(cell.Name, cell.Row, cell.Column, obj.ToString());
                                }
                                 cell.SetStyle(styletemp);
                            }
                        }
                        else
                        {
                            if (obj == null )
                                continue;
                            cell = cells[row + 1, k];
                            Aspose.Cells.Style styletemp = cell.GetDisplayStyle();
                            styletemp.Font.Color = this[j, i].Style.ForeColor;
                            styletemp.Font.IsBold = this[j, i].Style.Font != null ? this[j, i].Style.Font.Bold : false;
                            obj = this[j, i].Value;
                            if (obj != null)
                            {
                                var typ = obj.GetType();
                                if (typ == typeof(DateTime))
                                {
                                    styletemp.Custom = "yyyy-mm-dd hh:mm:ss";
                                    cell.PutValue(obj.ToString());
                                }
                                else if (typ == typeof(string))
                                {
                                    var str = obj.ToString();
                                    if (str.IsNum())
                                        cell.PutValue(double.Parse(str));
                                    else
                                        cell.PutValue(str);
                                }
                                else
                                {
                                    cell.PutValue(obj);
                                }
                                hasRecord = true;
                                if (this.Columns[j].Name == "ReportPath")
                                {
                                    int hyIndex=workbook.Worksheets[0].Hyperlinks.Add(cell.Name, 1, 1, obj.ToString());
                                    workbook.Worksheets[0].Hyperlinks[hyIndex].ScreenTip = obj.ToString();
                                    styletemp.Font.Color = Color.Blue;
                                    styletemp.Font.IsItalic = true;
                                    styletemp.Font.Underline = FontUnderlineType.Single;
                                }
                                cell.SetStyle(styletemp);
                            }
                        }
                        row++;
                    }
                    k++;
                }
            }
            //ws.Cells.CreateRange(ws.Cells[0].Row, ws.Cells[0].Column, ws.Cells.Rows.Count, ws.Cells.Columns.Count);
          //  Range range = workbook.Worksheets[0].Cells.CreateRange(0, 0, 4, 6); 
            workbook.Worksheets[0].AutoFitColumns();
          //  workbook.Worksheets[0].FreezePanes("A1", 10, 5);
           
            outWorkbook = workbook;
            return savePath;
        }

        /// <summary>
        /// 按照设定格式导出Excel
        /// </summary>
        public string ExportExcel_Public_SpecialRow1(bool ischeckcolumn, out bool hasRecord, out Workbook outWorkbook, List<string> elementList, string workRegion, out bool xmlLoaded)
        {
            IEnumerable<DataRow> rows1 = null;
            IEnumerable<DataRow> rows2 = null;
            bool hasContent = false;
            Workbook workbook = new Workbook();
            Cells cells = workbook.Worksheets[0].Cells;
            string savePath = string.Empty;
            hasRecord = false;
            xmlLoaded = false;
            bool isElement = false;
            try
            {
                string xmlPath = Application.StartupPath + @"\ElementParams.xml";
                Assembly ass = Assembly.LoadFile(Application.StartupPath + @"\Skyray.SetHistoryRecordInfo.dll");
                Type t = ass.GetType("Skyray.SetHistoryRecordInfo.Form1");
                MethodInfo miReadXml = t.GetMethod("ReadXmlEx", new Type[] { typeof(int), typeof(String) });
                object objFrm = Activator.CreateInstance(t);

                rows1 = miReadXml.Invoke(objFrm, new object[] { 0, xmlPath }) as IEnumerable<DataRow>;
                rows2 = miReadXml.Invoke(objFrm, new object[] { 1, xmlPath }) as IEnumerable<DataRow>;

                var queryContent = from p in rows1
                                   where p["Content"].ToString().Length > 0
                                   && elementList.Contains(p["Element"].ToString())
                                   select p;
                if (queryContent != null && queryContent.ToList().Count > 0)
                    hasContent = true;
                else
                {
                    hasContent = false;
                }

                xmlLoaded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.ToString());
                outWorkbook = null;
                return string.Empty;
            }

            int k = 0;
            for (int j = (ischeckcolumn) ? 1 : 0; j < this.Columns.Count; j++)
            {
                var col = this.Columns[j];
                isElement = elementList.Contains(col.HeaderText);
                if (col.Visible)
                {
                    //row1
                    var cell = cells[0, k];
                    Aspose.Cells.Style style = cell.GetDisplayStyle();
                    style.HorizontalAlignment = TextAlignmentType.Center;
                    cell.SetStyle(style);
                    cell.PutValue(isElement ? col.HeaderText + "(PPM)" : col.HeaderText);
                    //row2

                    if (hasContent)
                    {
                        var cell2 = cells[1, k];
                        Aspose.Cells.Style style2 = cell.GetDisplayStyle();
                        style2.HorizontalAlignment = TextAlignmentType.Center;
                        cell2.SetStyle(style2);
                        cell2.PutValue(string.Empty);
                        //Merge
                        if (!isElement)
                            cells.Merge(0, k, 2, 1);
                    }


                    int row = 0;
                    for (int i = 0; i < this.Rows.Count; i++)
                    {
                        var obj = this[0, i].Value;
                        if (ischeckcolumn)
                        {
                            if (obj == null || !bool.Parse(obj.ToString()))
                                continue;
                            if (obj.GetType() == typeof(bool) && bool.Parse(obj.ToString()))
                            {
                                cell = cells[row + (hasContent ? 2 : 1), k];
                                Aspose.Cells.Style styletemp = cell.GetDisplayStyle();
                                styletemp.Font.Color = this[j, i].Style.ForeColor;
                                styletemp.Font.IsBold = this[j, i].Style.Font != null ? this[j, i].Style.Font.Bold : false;

                                obj = this[j, i].Value;
                                if (obj != null)
                                {
                                    var typ = obj.GetType();
                                    if (typ == typeof(DateTime))
                                    {
                                        styletemp.Custom = "yyyy-mm-dd hh:mm:ss";
                                        cell.PutValue(obj.ToString());
                                    }
                                    else if (typ == typeof(string))
                                    {
                                        var str = obj.ToString();
                                        if (str.IsNum())
                                            cell.PutValue(double.Parse(str));
                                        else
                                            cell.PutValue(str);
                                    }
                                    else
                                    {
                                        cell.PutValue(obj);
                                    }
                                    hasRecord = true;
                                    if (this.Columns[j].Name == "ReportPath")
                                        workbook.Worksheets[0].Hyperlinks.Add(cell.Name, cell.Row, cell.Column, obj.ToString());
                                }
                                cell.SetStyle(styletemp);
                            }
                        }
                        else
                        {
                            if (obj == null)
                                continue;
                            cell = cells[row + (hasContent ? 2 : 1), k];
                            Aspose.Cells.Style styletemp = cell.GetDisplayStyle();
                            styletemp.Font.Color = this[j, i].Style.ForeColor;
                            styletemp.Font.IsBold = this[j, i].Style.Font != null ? this[j, i].Style.Font.Bold : false;
                            obj = this[j, i].Value;
                            if (obj != null)
                            {
                                var typ = obj.GetType();
                                if (typ == typeof(DateTime))
                                {
                                    styletemp.Custom = "yyyy-mm-dd hh:mm:ss";
                                    cell.PutValue(obj.ToString());
                                }
                                else if (typ == typeof(string))
                                {
                                    var str = obj.ToString();
                                    if (str.IsNum())
                                    {
                                        double d = double.Parse(str);
                                        cell.PutValue(d);
                                        if (isElement)
                                        {
                                            var query = from p in rows1
                                                        where
                                                            p["WorkRegion"].ToString() == workRegion &&
                                                            p["Element"].ToString() == col.HeaderText
                                                        select p;
                                            if (query != null && query.ToList().Count > 0)
                                            {
                                                for (int l = 0; l < query.ToList().Count; l++)
                                                {
                                                    double min = double.Parse(query.ToList()[l]["MinValue"].ToString());
                                                    double max = double.Parse(query.ToList()[l]["MaxValue"].ToString());
                                                    int fillColor = int.Parse(query.ToList()[l]["FillColor"].ToString());
                                                    int textColor = int.Parse(query.ToList()[l]["TextColor"].ToString());
                                                    string content = query.ToList()[l]["Content"].ToString();
                                                    if (hasContent)
                                                        cells[1, k].PutValue(content);
                                                    if (d >= min && d < max)
                                                    {
                                                        styletemp.Pattern = BackgroundType.Solid;
                                                        styletemp.ForegroundColor = Color.FromArgb(fillColor);
                                                        styletemp.Font.Color = Color.FromArgb(textColor);
                                                        //styletemp.Borders[BorderType.TopBorder].LineStyle = CellBorderType.None;
                                                        //styletemp.Borders[BorderType.TopBorder].Color = cells[0, 0].GetStyle().Borders[BorderType.TopBorder].Color;
                                                        //styletemp.Borders[BorderType.RightBorder].LineStyle = CellBorderType.None;
                                                        //styletemp.Borders[BorderType.RightBorder].Color = cells[0, 0].GetStyle().Borders[BorderType.TopBorder].Color;
                                                        //styletemp.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.None;
                                                        //styletemp.Borders[BorderType.BottomBorder].Color = cells[0, 0].GetStyle().Borders[BorderType.TopBorder].Color;
                                                        //styletemp.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.None;
                                                        //styletemp.Borders[BorderType.LeftBorder].Color = cells[0, 0].GetStyle().Borders[BorderType.TopBorder].Color;
                                                        break;
                                                    }
                                                }
                                            }

                                        }
                                    }
                                    else
                                        cell.PutValue(str);
                                }
                                else
                                {
                                    cell.PutValue(obj);
                                }
                                hasRecord = true;
                                if (this.Columns[j].Name == "ReportPath")
                                {
                                    int hyIndex = workbook.Worksheets[0].Hyperlinks.Add(cell.Name, 1, 1, obj.ToString());
                                    workbook.Worksheets[0].Hyperlinks[hyIndex].ScreenTip = obj.ToString();
                                    styletemp.Font.Color = Color.Blue;
                                    styletemp.Font.IsItalic = true;
                                    styletemp.Font.Underline = FontUnderlineType.Single;
                                }
                                cell.SetStyle(styletemp);
                            }
                        }
                        row++;
                    }
                    k++;
                }
            }

            if (rows2 != null && rows2.ToList().Count > 0)
            {
                int maxColumnIndex = k;
                string name = string.Empty;
                string value = string.Empty;
                int position = 0;
                for (int i = 0; i < rows2.ToList().Count; i++)
                {
                    name = rows2.ToList()[i]["Name"].ToString();
                    value = rows2.ToList()[i]["Value"].ToString();
                    position = int.Parse(rows2.ToList()[i]["Position"].ToString());
                    int confirmedPosition = maxColumnIndex - position < 0 ? 0 : maxColumnIndex - position;

                    cells.InsertColumn(confirmedPosition);
                    var cell = cells[0, confirmedPosition];
                    Aspose.Cells.Style style = cell.GetDisplayStyle();
                    style.HorizontalAlignment = TextAlignmentType.Center;
                    cell.SetStyle(style);
                    cell.PutValue(name);
                    if (hasContent)
                        cells.Merge(0, confirmedPosition, 2, 1);
                    int headerRowCount = hasContent ? 2 : 1;
                    for (int j = headerRowCount; j < cells.Rows.Count; j++)
                    {
                        cell = cells[j, confirmedPosition];
                        cell.SetStyle(style);
                        cell.PutValue(value);
                    }

                    maxColumnIndex++;
                }
            }

            //插入首列序号
            cells.InsertColumn(0);
            int diff = hasContent ? 2 : 1;
            if(hasContent)
                cells.Merge(0, 0, 2, 1);
            for (int i = diff; i < cells.Rows.Count; i++)
            {
                var cell = cells[i, 0];
                cell.PutValue(i + 1 - diff);
            }

            //ws.Cells.CreateRange(ws.Cells[0].Row, ws.Cells[0].Column, ws.Cells.Rows.Count, ws.Cells.Columns.Count);
            //  Range range = workbook.Worksheets[0].Cells.CreateRange(0, 0, 4, 6); 
            workbook.Worksheets[0].AutoFitColumns();
            //  workbook.Worksheets[0].FreezePanes("A1", 10, 5);

            outWorkbook = workbook;

            return savePath;
        }
        public void CreateChart(Workbook curWorkbook, int startrow, int startcolumn,string bitmapPath)
        {

           //int index = curWorkbook.Worksheets[0].Charts.Add(ChartType.Line, startrow, startcolumn, startrow + 10, startcolumn + 10);
           //Chart chart=curWorkbook.Worksheets[0].Charts[index];
           //chart.PivotSource = curWorkbook.Worksheets[0].Name + "!$B$9:$B$12";//"=Sheet1!$B$9:$B$12";
           //chart.HidePivotFieldButtons=false;

            curWorkbook.Worksheets[0].Pictures.Add(startrow, startcolumn, bitmapPath);
        }
        public string ExportExcel_Public_SpecialRowNew(bool ischeckcolumn, out bool hasRecord, out Workbook outWorkbook, List<string> otherInfo, List<string> redFieldInfo)
        {
            Workbook workbook = new Workbook();
            Cells cells = workbook.Worksheets[0].Cells;
            
            string savePath = string.Empty;
            hasRecord = false;
            int k = 0;
            int rowmun = this.Rows.Count;
            Worksheet sheet = workbook.Worksheets[0];
            
            for (int j = (ischeckcolumn) ? 1 : 0; j < this.Columns.Count; j++)
            {
                var col = this.Columns[j];
                if (col.Visible)
                {
                    var cell = cells[0, k];
                    cell.PutValue(col.HeaderText);
                    Aspose.Cells.Style styple = cell.GetStyle();
                    styple.IsTextWrapped = true;
                    cell.SetStyle(styple);
                    if (redFieldInfo.Count>0)
                        
                    {
                        foreach(string strRedInfo in redFieldInfo)
                            if (col.HeaderText.IndexOf(strRedInfo) != -1)
                            {
                                Aspose.Cells.Style styletemp = cell.GetDisplayStyle();
                                styletemp.Font.Color = Color.Red;
                                cell.SetStyle(styletemp);
                            }
                    }

                    

                    int row = 0;
                    for (int i = 0; i < this.Rows.Count; i++)
                    {
                        var obj = this[0, i].Value;

                        if (ischeckcolumn)
                        {
                            if (obj == null || !bool.Parse(obj.ToString()))
                                continue;
                            if (obj.GetType() == typeof(bool) && bool.Parse(obj.ToString()))
                            {
                                cell = cells[row + 1, k];
                                Aspose.Cells.Style styletemp = cell.GetDisplayStyle();
                                styletemp.Font.Color = this[j, i].Style.ForeColor;
                               
                                obj =  this[j, i].Value;
                                if (obj != null)
                                {
                                    var typ = obj.GetType();
                                    if (typ == typeof(DateTime))
                                    {
                                        styletemp.Custom = "yyyy-mm-dd hh:mm:ss";
                                        cell.PutValue(obj.ToString());
                                    }
                                    else if (typ == typeof(string))
                                    {
                                        var str = obj.ToString();
                                        if (str.IsNum())
                                            cell.PutValue(double.Parse(str));
                                        else
                                            cell.PutValue(str);
                                    }
                                    else
                                    {
                                        cell.PutValue(obj);
                                    }
                                    hasRecord = true;
                                    if (this.Columns[j].Name == "ReportPath")
                                        workbook.Worksheets[0].Hyperlinks.Add(cell.Name, cell.Row, cell.Column, obj.ToString());
                                }
                                cell.SetStyle(styletemp);
                            }
                        }
                        else
                        {
                            if (obj == null)
                                continue;
                            sheet.AutoFitRow(row + 1);
                            cell = cells[row + 1, k];
                            Aspose.Cells.Style styletemp = cell.GetDisplayStyle();
                            styletemp.Font.Color = this[j, i].Style.ForeColor;
                            obj =  this[j, i].Value;
                            if (obj != null)
                            {
                                var typ = obj.GetType();
                                if (typ == typeof(DateTime))
                                {
                                    styletemp.Custom = "yyyy-mm-dd hh:mm:ss";
                                    cell.PutValue(obj.ToString());
                                }
                                else if (typ == typeof(string))
                                {
                                    var str = obj.ToString();
                                    if (str.IsNum())
                                        cell.PutValue(double.Parse(str));
                                    else
                                        cell.PutValue(str);
                                }
                                else
                                {
                                    cell.PutValue(obj);
                                }
                                hasRecord = true;
                                if (this.Columns[j].Name == "ReportPath")
                                    workbook.Worksheets[0].Hyperlinks.Add(cell.Name, cell.Row, cell.Column, obj.ToString());
                            }
                            cell.SetStyle(styletemp);
                        }
                        row++;
                    }
                    k++;
                }
            }
            if (otherInfo != null)
            {
                int irow = rowmun + 2;
                foreach (string strinfo in otherInfo)
                {
                    irow++;
                    var cell1 = cells[irow, 0];
                    var obj1 = strinfo;
                    cell1.PutValue(obj1);
                }
            }

            sheet.AutoFitRow(0);
            outWorkbook = workbook;
            return savePath;
        }

        

        public string ExportExcel_Public()
        {
            Workbook workbook = new Workbook();
            Cells cells = workbook.Worksheets[0].Cells;

            int k = 0;
            bool hasRecord = false;
            for (int j = k; j < this.ColumnCount; j++)
            {
                var col = this.Columns[j];
                if (col.Visible)
                {
                    var cell = cells[0, k];
                    cell.PutValue(col.HeaderText);
                    for (int i = 0; i < this.RowCount; i++)
                    {
                        cell = cells[i + 1, k];
                        var obj = this[j, i].Value;
                        if (obj != null)
                        {
                            var typ = obj.GetType();
                            if (typ == typeof(DateTime))
                            {
                                Aspose.Cells.Style styletemp = cell.GetDisplayStyle();
                                styletemp.Custom = "yyyy-mm-dd hh:mm:ss";
                                cell.SetStyle(styletemp);
                                cell.PutValue(obj.ToString());
                            }
                            else if (typ == typeof(string))
                            {
                                var str = obj.ToString();
                                if (str.IsNum())
                                    cell.PutValue(double.Parse(str));
                                else
                                    cell.PutValue(str);
                            }
                            else
                            {
                                cell.PutValue(obj);
                            }
                            hasRecord = true;
                        }
                    }
                    k++;
                }
            }
            if (hasRecord)
            {
                SaveFileDialog sdlg = new SaveFileDialog();
                sdlg.Filter = "Excel File(*.xls)|*.xls";
                if (sdlg.ShowDialog() == DialogResult.OK)
                {
                    workbook.Save(sdlg.FileName);
                }
                return sdlg.FileName;
            }
            return null;
        }



        private void tsmiExprotToExcel_Click(object sender, EventArgs e)
        {
            ExportExcel_Public();
        }

        private void DataGridViewW_MouseClick(object sender, MouseEventArgs e)
        {
            if (ShowEportContextMenu && e.Button == MouseButtons.Right
                && this.HitTest(e.X, e.Y).Type == DataGridViewHitTestType.Cell)
            {
                cmsExport.Show(this.PointToScreen(e.Location));
            }
        }

        private void tsmiPrint_Click(object sender, EventArgs e)
        {
            Print(false);
        }

        private void tsmiPrintPreview_Click(object sender, EventArgs e)
        {
            Print(true);
        }
        public void Print(bool isPreview)
        {
            if (PDC != null && this.Rows.Count > 0)
            {
                var dgv = (this as DataGridView);
                dgv.Print(PDC, true, false, false,
                    false, isPreview, ToPrintCols, ToPrintRows);
            }
        }

        public void Add(DataGridViewTextBoxColumnW columnW)
        {
            this.Columns.Add(columnW);
        }

        public override void Refresh()
        {
            base.Refresh();
            HeaderTextRefresh();
        }

        public void HeaderTextRefresh()
        {
            foreach (var column in this.Columns)
            {
                if (column is DataGridViewTextBoxColumnW)
                    ((DataGridViewTextBoxColumnW)column).HeaderTextRefresh();
            }
        }
    }

    public class DataGridViewTextBoxColumnW : DataGridViewTextBoxColumn
    {
        public DataGridViewTextBoxColumnW(DataGridViewColumnHeaderCellW headerCell)
        {
            this.HeaderCell = headerCell;
            HeaderTextRefresh();
        }

        public void HeaderTextRefresh()
        {
            if (HeaderCell is DataGridViewColumnHeaderCellW)
            {
                try
                {
                    var cell = ((DataGridViewColumnHeaderCellW)HeaderCell);
                    var value = cell.DataSourceType.InvokeMember(cell.FieldName, System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.GetProperty, null, cell.HeaderTextDataSource, null);
                    this.HeaderText = cell.Prefix + (value == null ? "" : value.ToString()) + cell.Suffix;
                }
                catch { }
            }
        }
    }

    public class DataGridViewColumnHeaderCellW : DataGridViewColumnHeaderCell
    {
        public object HeaderTextDataSource { get; set; }
        private Type dataSourceType = null;
        public Type DataSourceType {
            get
            {
                if (HeaderTextDataSource != null&&dataSourceType==null)
                    dataSourceType= HeaderTextDataSource.GetType();
                return dataSourceType;
            }
            set { dataSourceType = value; }
        }
        public string FieldName { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
    }
}



