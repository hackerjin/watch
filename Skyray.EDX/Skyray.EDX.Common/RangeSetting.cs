using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.EDX.Common
{
    [Serializable]
    public class RangeSetting
    {
        public string ElementName { get; set; }

        public double MinValue { get; set; }

        public double MaxValue { get; set; }

        public RangeSetting(string name, double minValue, double maxValue)
        {
            this.ElementName = name;
            this.MinValue = minValue;
            this.MaxValue = maxValue;
        }
    }

    [Serializable]
    public class CurrentRange
    {
        public bool Selected { get; set; }

        public int CurrentRangeIndex { get; set; }

        public string CurrentName { get; set; }

        public List<RangeSetting> RangesSet { get; set; }

        public CurrentRange(string name,bool selected,List<RangeSetting> currentSet,int currentIndex)
        {
            this.CurrentName = name;
            this.Selected = selected;
            this.RangesSet = currentSet;
            this.CurrentRangeIndex = currentIndex;
        }
    }

    [Serializable]
    public class SerializeRange
    {
        public List<CurrentRange> RangesList { get; set; }

        public int GetMaxIndex()
        {
            int maxIndex = 0;
            if (RangesList != null && RangesList.Count > 0)
            {
                foreach (CurrentRange range in RangesList)
                    if (range.CurrentRangeIndex > maxIndex)
                        maxIndex = range.CurrentRangeIndex;
            }
            return maxIndex;
        }

        public SerializeRange()
        {
            RangesList = new List<CurrentRange>();
        }
    }
}
