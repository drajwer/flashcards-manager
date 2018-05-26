using System;
using System.Collections.Generic;
using System.Text;
using FlashcardsManager.Core.Enums;

namespace FlashcardsManager.Core.Helpers
{
    public class CategoriesFilteringModel : FilteringModelBase
    {
        public CategoriesFilteringModel(string searchText, CategoriesSortingCriterion sortingCriterion, 
            bool descending, int pageIndex, int pageSize) : base(searchText, descending, pageIndex, pageSize)
        {
            SortingCriterion = sortingCriterion;
        }

        public CategoriesSortingCriterion SortingCriterion { get; set; }
    }
}
