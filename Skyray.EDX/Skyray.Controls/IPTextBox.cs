using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Skyray.Controls
{
	/** \class IPTextBox
	 * \brief An IP Address Box
	 * 
	 * A TextBox that only allows entry of a valid ip address
	 **
	 */
	public class IPTextBox: System.Windows.Forms.UserControl
	{
        private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ToolTip toolTip1;
        private TextBox Box1;
        private Label label1;
        private TextBox Box4;
        private Label label3;
        private TextBox Box3;
        private Label label2;
        private TextBox Box2;
		private System.ComponentModel.IContainer components;
		
		/** Sets and Gets the tooltiptext on toolTip1 */
		public string ToolTipText
		{
			get
			{	return this.toolTip1.GetToolTip(Box1); }
			set
			{
				this.toolTip1.SetToolTip(Box1,value);
				this.toolTip1.SetToolTip(Box2,value);
				this.toolTip1.SetToolTip(Box3,value);
				this.toolTip1.SetToolTip(Box4,value);
				this.toolTip1.SetToolTip(label1,value);
				this.toolTip1.SetToolTip(label2,value);
				this.toolTip1.SetToolTip(label3,value);
			}
		}		

		/** Set or Get the string that represents the value in the box */
		public override string Text
		{
			get 
			{
				return Box1.Text + "." + Box2.Text + "." + Box3.Text + "." + Box4.Text;
			}
			set
			{
				if (value != "" && value != null)
				{
					string[] pieces = new string[4];
					pieces = value.ToString().Split(".".ToCharArray(),4);
					Box1.Text = pieces[0];
					Box2.Text = pieces[1];
					Box3.Text = pieces[2];
					Box4.Text = pieces[3];
				}
				else
				{
					Box1.Text = "";
					Box2.Text = "";
					Box3.Text = "";
					Box4.Text = "";
				}
			}
		}

		/** Returns whether all Box1 thru Box4 have a valid IP octet
		 * \return True if valid, false otherwise
		 * */
		public bool IsValid()
		{
			try
			{
				int checkval = int.Parse(Box1.Text);
				if(checkval < 0 || checkval > 255)
					return false;
				checkval = int.Parse(Box2.Text);
				if(checkval < 0 || checkval > 255)
					return false;
				checkval = int.Parse(Box3.Text);
				if(checkval < 0 || checkval > 255)
					return false;
				checkval = int.Parse(Box4.Text);
				if(checkval < 0 || checkval > 255)
					return false;
				else
					return true;
			}
			catch
			{
				return false;
			}
		}

		public IPTextBox()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Box4 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Box3 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Box2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Box1 = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.Box4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.Box3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.Box2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.Box1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(180, 23);
            this.panel1.TabIndex = 0;
            this.panel1.EnabledChanged += new System.EventHandler(this.panel1_EnabledChanged);
            // 
            // Box4
            // 
            this.Box4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Box4.Location = new System.Drawing.Point(146, 4);
            this.Box4.MaxLength = 3;
            this.Box4.Name = "Box4";
            this.Box4.Size = new System.Drawing.Size(25, 13);
            this.Box4.TabIndex = 13;
            this.Box4.TabStop = false;
            this.Box4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Box4.Click += new System.EventHandler(this.Box4_Click);
            this.Box4.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Box4_KeyUp);
            this.Box4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Box4_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(132, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(10, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = ".";
            // 
            // Box3
            // 
            this.Box3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Box3.Location = new System.Drawing.Point(101, 4);
            this.Box3.MaxLength = 3;
            this.Box3.Name = "Box3";
            this.Box3.Size = new System.Drawing.Size(25, 13);
            this.Box3.TabIndex = 11;
            this.Box3.TabStop = false;
            this.Box3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Box3.Click += new System.EventHandler(this.Box3_Click);
            this.Box3.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Box3_KeyUp);
            this.Box3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Box3_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(85, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = ".";
            // 
            // Box2
            // 
            this.Box2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Box2.Location = new System.Drawing.Point(54, 4);
            this.Box2.MaxLength = 3;
            this.Box2.Name = "Box2";
            this.Box2.Size = new System.Drawing.Size(25, 13);
            this.Box2.TabIndex = 9;
            this.Box2.TabStop = false;
            this.Box2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Box2.Click += new System.EventHandler(this.Box2_Click);
            this.Box2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Box2_KeyUp);
            this.Box2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Box2_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(10, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = ".";
            // 
            // Box1
            // 
            this.Box1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Box1.Location = new System.Drawing.Point(4, 4);
            this.Box1.MaxLength = 3;
            this.Box1.Name = "Box1";
            this.Box1.Size = new System.Drawing.Size(25, 13);
            this.Box1.TabIndex = 7;
            this.Box1.TabStop = false;
            this.Box1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Box1.Click += new System.EventHandler(this.Box1_Click);
            this.Box1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Box1_KeyUp);
            this.Box1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Box1_KeyPress);
            // 
            // IPTextBox
            // 
            this.Controls.Add(this.panel1);
            this.Name = "IPTextBox";
            this.Size = new System.Drawing.Size(180, 23);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion
		
		/** 
		 * \ifnot hide_events
		 * Checks that a string passed in resolves to an integer value between 0 and 255
		 * \param inString The string passed in for testing
		 * \return True if the string is between 0 and 255 inclusively, false otherwise
		 * \endif
		 * */
		private bool IsValid(string inString,TextBox box1)
		{
			try 
			{
				int theValue = int.Parse(inString);
				if(theValue >=0 && theValue <= 255)
					return true;
				else
				{	
					MessageBox.Show("您必须输入一个 0 - 255 的数字","错误");
                    box1.Text = "255";
					return false;
				}
			}
			catch
			{
				return false;
			}
		}

		/// \ifnot hide_events
		/// Performs KeyPress analysis and handling to ensure a valid ip octet is
		/// being entered in Box1.
		/// \endif
		private void Box1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			//Only Accept a '.', a numeral, or backspace
			if(e.KeyChar.ToString() == "." || Char.IsDigit(e.KeyChar) || e.KeyChar == 8)
			{
				//If the key pressed is a '.'
				if(e.KeyChar.ToString() == ".")
				{
					//If the Text is a valid ip octet move to the next box
					if(Box1.Text != "" && Box1.Text.Length != Box1.SelectionLength)
					{
                        if (IsValid(Box1.Text,Box1))
                        {
                            Box2.Focus();
                            Box2.SelectAll();
                        }
                        else
                            Box1.SelectAll();
					}
					e.Handled = true;
				}
			
				//If we are not overwriting the whole text
				else if(Box1.SelectionLength != Box1.Text.Length)
				{	
					//Check that the new Text value will be a valid
					// ip octet then move on to next box
					if(Box1.Text.Length == 2)
					{
						if(e.KeyChar == 8)
							Box1.Text.Remove(Box1.Text.Length-1,1);
						else if(!IsValid(Box1.Text.Insert(Box1.SelectionStart,e.KeyChar.ToString()),Box1))
						{
							Box1.SelectAll();
							e.Handled = true;
						}
						else
						{
							Box2.Focus();
                            Box2.SelectAll();
						}
					}
				}
			}
			//Do nothing if the keypress is not numeral, backspace, or '.'
			else
				e.Handled = true;
		}

		/// \ifnot hide_events
		/// Performs KeyPress analysis and handling to ensure a valid ip octet is
		/// being entered in Box2.
		/// \endif
		private void Box2_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			//Similar to Box1_KeyPress but in special case for backspace moves cursor
			//to the previouse box (Box1)
			if(e.KeyChar.ToString() == "." || Char.IsDigit(e.KeyChar) || e.KeyChar == 8)
			{
				if(e.KeyChar.ToString() == ".")
				{
					if(Box2.Text != "" && Box2.Text.Length != Box2.SelectionLength)
					{
                        if (IsValid(Box2.Text,Box2))
                        {
                            Box3.Focus();
                            Box3.SelectAll();
                        }
                        else
                            Box2.SelectAll();
					}
					e.Handled = true;
				}			
				else if(Box2.SelectionLength != Box2.Text.Length)
				{
					if(Box2.Text.Length == 2)
					{
						if(e.KeyChar == 8)
						{
							Box2.Text.Remove(Box2.Text.Length-1,1);
						}
						else if(!IsValid(Box2.Text.Insert(Box2.SelectionStart,e.KeyChar.ToString()),Box2))
						{
							Box2.SelectAll();
							e.Handled = true;
						}
						else
						{
							Box3.Focus();
                            Box3.SelectAll();
						}
					}
				}
				else if(Box2.Text.Length == 0 && e.KeyChar == 8)
				{
					Box1.Focus();
					Box1.SelectionStart = Box1.Text.Length;
				}
			}
			else
				e.Handled = true;
		
		}

		/// \ifnot hide_events
		/// Performs KeyPress analysis and handling to ensure a valid ip octet is
		/// being entered in Box3.
		/// \endif
		private void Box3_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			//Identical to Box2_KeyPress except that previous box is Box2 and
			//next box is Box3
			if(e.KeyChar.ToString() == "." || Char.IsDigit(e.KeyChar) || e.KeyChar == 8)
			{
				if(e.KeyChar.ToString() == ".")
				{
					if(Box3.Text != "" && Box3.SelectionLength != Box3.Text.Length)
					{
                        if (IsValid(Box3.Text,Box3))
                        {
                            Box4.Focus();
                            Box4.SelectAll();
                        }
                        else
                            Box3.SelectAll();
					}
					e.Handled = true;
				}			
				else if(Box3.SelectionLength != Box3.Text.Length)
				{
					if(Box3.Text.Length == 2)
					{
						if(e.KeyChar == 8)
						{
							Box3.Text.Remove(Box3.Text.Length-1,1);
						}
						else if(!IsValid(Box3.Text.Insert(Box3.SelectionStart, e.KeyChar.ToString()),Box3))
						{
							Box3.SelectAll();
							e.Handled = true;
						}
						else
						{
							Box4.Focus();
                            Box4.SelectAll();
						}
					}
				}
				else if(Box3.Text.Length == 0 && e.KeyChar == 8)
				{
					Box2.Focus();
					Box2.SelectionStart = Box2.Text.Length;
				}
			}
			else
				e.Handled = true;
		}

		/// \ifnot hide_events
		/// Performs KeyPress analysis and handling to ensure a valid ip octet is
		/// being entered in Box4.
		/// \endif
		private void Box4_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			//Similar to Box3 but ignores the '.' character and does not advance
			//to the next box.  Also Box3 is previous box for backspace case.
			if(Char.IsDigit(e.KeyChar) || e.KeyChar == 8)
			{
				if(Box4.SelectionLength != Box4.Text.Length)
				{
					if(Box4.Text.Length == 2)
					{
						if(e.KeyChar == 8)
						{
							Box4.Text.Remove(Box4.Text.Length-1,1);
						}
						else if(!IsValid(Box4.Text.Insert(Box4.SelectionStart,e.KeyChar.ToString()),Box4))
						{
							Box4.SelectAll();
							e.Handled = true;
						}
					}
				}
				else if(Box4.Text.Length == 0 && e.KeyChar == 8)
				{
					Box3.Focus();
					Box3.SelectionStart = Box3.Text.Length;
				}
			}
			else
				e.Handled = true;
		}

		/// \ifnot hide_events
		/// Selects All text in a box for overwriting upon entering the box
		/// \endif
		private void Box_Enter(object sender, System.EventArgs e)
		{
			TextBox tb = (TextBox) sender;
			tb.SelectAll();
		}

		/// \ifnot hide_events
		/// Ensures a consistent "grayed out" look when the control is disabled
		/// \endif
		private void label_EnabledChanged(object sender, System.EventArgs e)
		{
			Label lbl = (Label) sender;
			if(lbl.Enabled)
				lbl.BackColor = SystemColors.Window;
			else
				lbl.BackColor = SystemColors.Control;
		}

		/// \ifnot hide_events
		/// Ensures a consistent "grayed out" look when the control is disabled
		/// \endif
		private void panel1_EnabledChanged(object sender, System.EventArgs e)
		{
			Panel pan = (Panel) sender;
			if(pan.Enabled)
				pan.BackColor = SystemColors.Window;
			else
				pan.BackColor = SystemColors.Control;
		}

        private int preSelectStart;

        private void Box1_KeyUp(object sender, KeyEventArgs e)
        {
            if (preSelectStart == Box1.SelectionStart && Box1.SelectionStart == Box1.Text.Length && (e.KeyCode == Keys.Right||e.KeyCode == Keys.Down))
            {
                Box2.Focus();
                preSelectStart=Box2.SelectionStart = 0;
                return;
            }
            preSelectStart = Box1.SelectionStart;
        }

        private void Box2_KeyUp(object sender, KeyEventArgs e)
        {
            if (preSelectStart == Box2.SelectionStart && Box2.SelectionStart == Box2.Text.Length && (e.KeyCode == Keys.Right || e.KeyCode == Keys.Down))
            {
                Box3.Focus();
                preSelectStart=Box3.SelectionStart = 0;
                return;
            }
            if (preSelectStart == Box2.SelectionStart && Box2.SelectionStart == 0 && (e.KeyCode == Keys.Left|| e.KeyCode == Keys.Up))
            {
                Box1.Focus();
                preSelectStart = Box1.SelectionStart = Box1.Text.Length;
                return;
            }
            preSelectStart = Box2.SelectionStart;
        }

        private void Box3_KeyUp(object sender, KeyEventArgs e)
        {
            if (preSelectStart == Box3.SelectionStart && Box3.SelectionStart == Box3.Text.Length && (e.KeyCode == Keys.Right || e.KeyCode == Keys.Down))
            {
                Box4.Focus();
                preSelectStart=Box4.SelectionStart = 0;
                return;
            }

            if (preSelectStart == Box3.SelectionStart && Box3.SelectionStart == 0 && (e.KeyCode == Keys.Left || e.KeyCode == Keys.Up))
            {
                Box2.Focus();
                preSelectStart = Box2.SelectionStart = Box2.Text.Length;
                return;
            }
            preSelectStart = Box3.SelectionStart;
        }

        private void Box4_KeyUp(object sender, KeyEventArgs e)
        {
            if (preSelectStart == Box4.SelectionStart && Box4.SelectionStart == 0 && (e.KeyCode == Keys.Left || e.KeyCode == Keys.Up))
            {
                Box3.Focus();
                preSelectStart = Box3.SelectionStart=Box3.Text.Length;
                return;
            }
            preSelectStart = Box4.SelectionStart;
        }

        public event KeyEventHandler clickFocus;

        private void Box4_Click(object sender, EventArgs e)
        {
            preSelectStart = Box4.SelectionStart;
            if (clickFocus != null)
                clickFocus(null, null);
        }

        private void Box1_Click(object sender, EventArgs e)
        {
            preSelectStart = Box1.SelectionStart;
            if (clickFocus != null)
                clickFocus(null, null);
        }

        private void Box2_Click(object sender, EventArgs e)
        {
            preSelectStart = Box2.SelectionStart;
            if (clickFocus != null)
                clickFocus(null, null);
        }

        private void Box3_Click(object sender, EventArgs e)
        {
            preSelectStart = Box3.SelectionStart;
            if (clickFocus != null)
                clickFocus(null, null);
        }
	}
}
