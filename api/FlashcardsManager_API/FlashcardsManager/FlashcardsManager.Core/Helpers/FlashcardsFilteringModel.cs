using System;
using System.Collections.Generic;
using System.Text;
using FlashcardsManager.Core.Enums;

namespace FlashcardsManager.Core.Helpers
{
    public class FlashcardsFilteringModel : FilteringModelBase
    {
        public FlashcardsFilteringModel(string searchText, FlashcardsSortingCriterion sortingCriterion,
            bool descending, int pageIndex, int pageSize) : base(searchText, descending, pageIndex, pageSize)
        {
            SortingCriterion = sortingCriterion;
        }

        public FlashcardsSortingCriterion SortingCriterion { get; set; }
    }
}
