using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.EDX.Common
{
    public class datagridviewCheckboxHeaderEventArgs : EventArgs
    {
        public datagridviewCheckboxHeaderEventArgs(bool checkState)
            : base()
        {
            this.checkedState = checkState;
        }

        private bool checkedState = false;

        public bool CheckedState
        {
            get { return checkedState; }
            set { checkedState = value; }
        }

    }
}
