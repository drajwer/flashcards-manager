using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashcardsManager.Core.Enums;
using FlashcardsManager.Core.Models;

namespace FlashcardsManager.Core.Services
{
    public interface ILearningService
    {
        Task<List<Flashcard>> GetFlashcards(User user, int categoryId, FlashcardsSearchCriterionEnum mode, int count);
        Task ProceedFlashcard(int flashcardId, User user, FlashcardResult result);
        Score GetUserScore(User user);
        Task<Score> GetUserScoreForCategory(User user, int categoryId);


    }
}
