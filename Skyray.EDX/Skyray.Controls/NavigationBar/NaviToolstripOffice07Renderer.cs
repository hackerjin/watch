﻿#region License and Copyright

/*
 
Author:  Jacob Mesu
 
Attribution-Noncommercial-Share Alike 3.0 Unported
You are free:

    * to Share — to copy, distribute and transmit the work
    * to Remix — to adapt the work

Under the following conditions:

    * Attribution — You must attribute the work and give credits to the author or Skyray.Controls.net
    * Noncommercial — You may not use this work for commercial purposes. If you want to adapt
      this work for a commercial purpose, visit Skyray.Controls.net and request the Attribution-Share 
      Alike 3.0 Unported license for free. 

http://creativecommons.org/licenses/by-nc-sa/3.0/

*/
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Skyray.Controls.NavigationBar
{
   public class NaviToolstripOffice07Renderer : ToolStripProfessionalRenderer
   {
      #region Constructor 

      /// <summary>
      /// Initializes a new instance of the ToolstripOffice07Renderer class
      /// </summary>
      /// <param name="colorTable">The colors used to draw the MenuStrip</param>
      public NaviToolstripOffice07Renderer(ProfessionalColorTable colorTable)
         : base(colorTable)
      {
      }

      #endregion
   }
}
