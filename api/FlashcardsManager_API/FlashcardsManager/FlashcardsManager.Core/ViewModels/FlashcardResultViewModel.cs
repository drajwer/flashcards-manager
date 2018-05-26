using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FlashcardsManager.Core.Enums;

namespace FlashcardsManager.Core.ViewModels
{
    public class FlashcardResultViewModel
    {
        [Required]
        public int FlashcardId { get; set; }

        [Required]
        public FlashcardResult Result { get; set; }
    }
}
