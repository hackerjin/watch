using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Skyray.Controls
{
    /// <summary>
    /// A FrontPage style table dimensions picker.
    /// </summary>
    public class TablePicker : System.Windows.Forms.Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public TablePicker()
        {
            // Activates double buffering
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // TablePicker
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(445, 445);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TablePicker";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "TablePicker";
            this.Deactivate += new System.EventHandler(this.TablePicker_Deactivate);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TablePicker_Paint);
            this.MouseEnter += new System.EventHandler(this.TablePicker_MouseEnter);
            this.Click += new System.EventHandler(this.TablePicker_Click);
            this.MouseLeave += new System.EventHandler(this.TablePicker_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TablePicker_MouseMove);
            this.ResumeLayout(false);

        }
        #endregion

        private Pen BeigePen = new Pen(Color.Beige, 1);
        private Brush BeigeBrush = System.Drawing.Brushes.Beige;
        private Brush GrayBrush = System.Drawing.Brushes.Gray;
        private Brush BlackBrush = System.Drawing.Brushes.Black;
        private Brush WhiteBrush = System.Drawing.Brushes.White;
        private Pen BorderPen = new Pen(SystemColors.ControlDark);
        private Pen BluePen = new Pen(Color.SlateGray, 1);

        private string DispText = "Cancel";	// Display text
       // private int DispHeight = 20;		// Display ("Table 1x1", "Cancel")
        private Font DispFont = new Font("Tahoma", 8.25F);
        private int SquareX = 20;			// Width of squares 
        private int SquareY = 20;			// Height of squares

        private int SquareQX = 22;			// Number of visible squares (X)
        private int SquareQY = 22;			// Number of visible squares (Y)

        private int SelQX = 1;				// Number of selected squares (x)
        private int SelQY = 1;				// Number of selected squares (y)

        private bool bHiding = false;
        private bool bCancel = true;	// Determines whether to Cancel

        /// <summary>
        /// Similar to <code><see cref="DialogResult"/> 
        /// == <see cref="DialogResult.Cancel"/></code>,
        /// but is used as a state value before the form
        /// is hidden and cancellation is finalized.
        /// </summary>
        public bool Cancel
        {
            get
            {
                return bCancel;
            }
        }

        /// <summary>
        /// Returns the number of columns, or the horizontal / X count,
        /// of the selection.
        /// </summary>
        public int SelectedColumns
        {
            get
            {
                return SelQX;
            }
        }

        /// <summary>
        /// Returns the number of rows, or the vertical / Y count, 
        /// of the selection.
        /// </summary>
        public int SelectedRows
        {
            get
            {
                return SelQY;
            }
        }

        private void TablePicker_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawRectangle(BorderPen, 0, 0, this.Width - 1, this.Height - 1);

            int dispY = ((SquareY - 1) * SquareQY) + SquareQY + 4;
            if (this.Cancel)
            {
                DispText = "Cancel";
            }
            else
            {
                DispText = SelQX.ToString() + " by " + SelQY.ToString() + " Table";
            }
            g.DrawString(DispText, DispFont, BlackBrush, 3, dispY + 2);

            // Draw each of the squares and fill with the default color.
            for (int x = 0; x < SquareQX; x++)
            {
                for (int y = 0; y < SquareQY; y++)
                {
                    g.FillRectangle(WhiteBrush, (x * SquareX) + 3, (y * SquareY) + 3, SquareX - 2, SquareY - 2);
                    g.DrawRectangle(BorderPen, (x * SquareX) + 3, (y * SquareY) + 3, SquareX - 2, SquareY - 2);
                }
            }

            // Go back and paint the squares with selection colors.
            for (int x = 0; x < SelQX; x++)
            {
                for (int y = 0; y < SelQY; y++)
                {
                    g.FillRectangle(BeigeBrush, (x * SquareX) + 3, (y * SquareY) + 3, SquareX - 2, SquareY - 2);
                    g.DrawRectangle(BluePen, (x * SquareX) + 3, (y * SquareY) + 3, SquareX - 2, SquareY - 2);
                }
            }
        }

        /// <summary>
        /// Detect termination. Hides form.
        /// </summary>
        private void TablePicker_Deactivate(object sender, System.EventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// Detects mouse movement. Tracks table dimensions selection.
        /// </summary>
        private void TablePicker_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.X > this.Width - 10 || e.Y > this.Height - 10) return;

            int sqx = e.X / SquareX + 1;
            int sqy = e.Y / SquareY + 1;

            bool changed = false;
            if (sqx != SelQX)
            {
                changed = true;
                SelQX = sqx;
            }
            if (sqy != SelQY)
            {
                changed = true;
                SelQY = sqy;
            }

            if (changed && OnSelectChanged != null)
            {
                OnSelectChanged(null, new SelectChangedEventArgs());
            }

            //Í¨ÖªÖØ»æ
            if (changed) Invalidate();
        }

        /// <summary>
        /// Detects mouse sudden exit from the form to indicate 
        /// escaped (canceling) state.
        /// </summary>
        private void TablePicker_MouseLeave(object sender, System.EventArgs e)
        {
            if (!bHiding) bCancel = true;
            this.DialogResult = DialogResult.Cancel;
            this.Invalidate();
        }

        /// <summary>
        /// Cancels the prior cancellation caused by MouseLeave.
        /// </summary>
        private void TablePicker_MouseEnter(object sender, System.EventArgs e)
        {
            bHiding = false;
            bCancel = false;
            this.DialogResult = DialogResult.OK;
            this.Invalidate();
        }

        /// <summary>
        /// Detects that the user made a selection by clicking.
        /// </summary>
        private void TablePicker_Click(object sender, System.EventArgs e)
        {
            bHiding = true; // Not the same as Visible == false
            // because bHiding suggests that the control
            // is still "active" (not canceled).
            this.Hide();
        }

        public event EventHandler<SelectChangedEventArgs> OnSelectChanged;
        public class SelectChangedEventArgs : EventArgs { }
    }
}
