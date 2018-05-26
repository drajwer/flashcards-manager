using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashcardsManager.Core.Enums
{
    public enum FlashcardsSearchCriterionEnum
    {
        [Description("All flashcards")]
        All = 0,
        [Description("Newly added flashcards")]
        New = 1,
        [Description("All old flashcards")]
        Old = 2,
        [Description("Old flashcards user had problem with")]
        Hard = 3,
        [Description("Old flashcards user mastered")]
        Known = 4
    }
}
