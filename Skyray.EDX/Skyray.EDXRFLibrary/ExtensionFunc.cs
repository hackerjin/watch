using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.EDXRFLibrary
{
    public static class ExtensionFunc
    {
        public static int GetElementIndex(this ElementList list, string strElementN)
        {
            int index = -1;
            for (int i = 0; i < list.Items.Count; i++)
            {
                if (list.Items[i].Caption.CompareTo(strElementN) == 0)
                {
                    return i;
                }
            }
            return index;
        }
    }
}
