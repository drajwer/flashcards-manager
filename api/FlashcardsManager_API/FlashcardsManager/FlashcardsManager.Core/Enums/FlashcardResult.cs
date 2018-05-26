using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashcardsManager.Core.Enums
{
    public enum FlashcardResult
    {
        [Description("User resolved flashcard")]
        Success = 0,

        [Description("User resolved flashcard partially")]
        Partial = 1,

        [Description("User failed to resolve flashcard")]
        Fail = 2

    }
}
