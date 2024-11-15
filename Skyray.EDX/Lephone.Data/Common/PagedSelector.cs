﻿using System;
using System.Collections;
using Lephone.Data.Definition;

namespace Lephone.Data.Common
{
    public class PagedSelector<T> : IPagedSelector where T : class, IDbObject
    {
        protected Condition iwc;
        protected OrderBy oc;
        internal int _PageSize;
        protected DbContext Entry;
        internal bool isDistinct;
        protected long ResultCount = -1;
        protected long PageCount = -1;

        public PagedSelector(Condition iwc, OrderBy oc, int pageSize, DbContext ds)
            : this(iwc, oc, pageSize, ds, false)
        {
        }

        public PagedSelector(Condition iwc, OrderBy oc, int pageSize, DbContext ds, bool isDistinct)
        {
            this.iwc = iwc;
            this.oc = oc;
            this._PageSize = pageSize;
            this.Entry = ds;
            this.isDistinct = isDistinct;
        }

        int IPagedSelector.PageSize
        {
            get { return _PageSize; }
        }

        public long GetResultCount()
        {
            if (ResultCount < 0)
            {
                ResultCount = Entry.GetResultCount(typeof(T), iwc, isDistinct);
            }
            return ResultCount;
        }

        public long GetPageCount()
        {
            if (PageCount < 0)
            {
                PageCount = (long) Math.Floor((double) (GetResultCount() - 1)/_PageSize) + 1;
            }
            return PageCount;
        }

        public virtual IList GetCurrentPage(long pageIndex)
        {
            long startWith = _PageSize * pageIndex;
            long tn = startWith + _PageSize;
            var query = Entry.From<T>().Where(iwc).OrderBy(oc.OrderItems.ToArray()).Range(startWith + 1, tn);
            if(isDistinct)
            {
                return query.SelectDistinct();
            }
            return query.Select();
        }
    }
}
