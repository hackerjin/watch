
using System;
using System.Collections.Generic;

using System.Text;
using ZedGraph;

namespace Skyray.Controls.Extension
{
    public class Boundary
    {
        public PointPairList list { get; private set; }
        public TextObj text { get; private set; }
        public Boundary(PointPairList list, TextObj text)
        {
            this.list = list;
            this.text = text;
        }
    }
}