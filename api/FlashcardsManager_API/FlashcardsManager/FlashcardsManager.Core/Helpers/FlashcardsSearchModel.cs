using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashcardsManager.Core.Enums;

namespace FlashcardsManager.Core.Helpers
{
    public class FlashcardsSearchModel
    {
        public string  UserId { get; set; }
        public int CategoryId { get; set; }
        public FlashcardsSearchCriterionEnum FlashcardsSearchCriterionEnum { get; set; }

        public FlashcardsSearchModel(string userId, int categoryId, FlashcardsSearchCriterionEnum criterion)
        {
            UserId = userId;
            CategoryId = categoryId;
            FlashcardsSearchCriterionEnum = criterion;
        }
    }
}
