using System;
using System.Collections.Generic;
using System.Text;
using Skyray.Controls.Tree.NodeControls;

namespace Skyray.Controls.Tree
{
	public interface IToolTipProvider
	{
		string GetToolTip(TreeNodeAdv node, NodeControl nodeControl);
	}
}
