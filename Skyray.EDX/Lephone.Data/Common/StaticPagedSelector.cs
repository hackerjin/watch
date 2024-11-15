﻿using System.Collections;
using Lephone.Data.Definition;

namespace Lephone.Data.Common
{
    public class StaticPagedSelector<T> : PagedSelector<T> where T : class, IDbObject
    {
        public StaticPagedSelector(Condition iwc, OrderBy oc, int pageSize, DbContext ds)
            : base(iwc, oc, pageSize, ds)
        {
        }

        public StaticPagedSelector(Condition iwc, OrderBy oc, int pageSize, DbContext ds, bool isDistinct)
            : base(iwc, oc, pageSize, ds, isDistinct)
        {
        }

        public override IList GetCurrentPage(long pageIndex)
        {
            long rc = GetResultCount();
            var firstPageSize = (int)(rc % _PageSize);
            if (firstPageSize == 0)
            {
                firstPageSize = _PageSize;
            }
            var pages = (int)((rc - firstPageSize) / _PageSize);
            pageIndex = pages - pageIndex;
            if (pageIndex <= 0)
            {
                return Entry.From<T>().Where(iwc).OrderBy(oc.OrderItems.ToArray()).Range(1, firstPageSize).Select();
            }
            long startWith = firstPageSize + _PageSize * (pageIndex - 1);
            long tn = startWith + _PageSize;
            IList ret = Entry.From<T>().Where(iwc).OrderBy(oc.OrderItems.ToArray()).Range(startWith + 1, tn).Select();
            return ret;
        }
    }
}
