using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FlashcardsManager.Core.Helpers
{
    public abstract class FilteringModelBase
    {
        protected FilteringModelBase(string searchText, bool descending, int pageIndex, int pageSize)
        {
            SearchText = searchText;
            Descending = descending;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
        [MinLength(0)]
        public string SearchText { get; set; }
        public bool Descending { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
