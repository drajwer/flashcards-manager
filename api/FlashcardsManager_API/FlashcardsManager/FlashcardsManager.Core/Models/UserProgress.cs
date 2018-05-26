using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashcardsManager.Core.Repositories.Interfaces;

namespace FlashcardsManager.Core.Models
{
    public class UserProgress
    {
        [Key]
        [Column(Order = 1)]
        public string UserId { get; set; }
        public User User { get; set; }

        [Key]
        [Column(Order = 2)]
        public int FlashcardId { get; set; }
        public Flashcard Flashcard { get; set; }

        public int Progress { get; set; }

        public UserProgress()
        {
        }
        public UserProgress(User user, Flashcard flashcard)
        {
            UserId = user.Id;
            FlashcardId = flashcard.Id;
            Progress = 0;
        }
    }

}
