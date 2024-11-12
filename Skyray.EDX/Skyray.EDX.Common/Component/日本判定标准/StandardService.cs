using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Skyray.EDX.Common.Component
{
    public class StandardService
    {
        private BindingList<ResultStandard> _standardList;
        public BindingList<ResultStandard> StandardList
        {
            get
            {
                return _standardList;
            }
            set
            {
                _standardList = value;
            }
        }

        public StandardService()
        {
            _standardList = new BindingList<ResultStandard>();
        }

        public void AddOne(ResultStandard standard)
        {
            _standardList.Add(standard);
        }

        public void Remove(ResultStandard standard)
        {
            _standardList.Remove(standard);
        }

        public void RemoveAt(int index)
        {
            _standardList.RemoveAt(index);
        }

        public void Clear()
        {
            _standardList.Clear();
        }
    }
}
