using System;
using System.Collections.Generic;
using System.Text;
using FlashcardsManager.Core.Enums;

namespace FlashcardsManager.Core.Helpers
{
    public class UsersFilteringModel : FilteringModelBase
    {
        public UsersFilteringModel(string searchText, UsersSortingCriterion sortingCriterion,
            bool descending, int pageIndex, int pageSize) : base(searchText, descending, pageIndex, pageSize)
        {
            SortingCriterion = sortingCriterion;
        }

        public UsersSortingCriterion SortingCriterion { get; set; }
    }
}
