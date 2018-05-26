using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashcardsManager.Core.Helpers;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Core.UnitOfWork;

namespace FlashcardsManager.Core.Services
{
    public interface IService
    {
        Task ChangeUserProgress(Flashcard flashcard, User user, int value);
        //void RenameCategory(int categoryId, string newName);
        IQueryable<Flashcard> FilterFlashcards(FlashcardsSearchModel searchModel);
        IQueryable<Flashcard> GetBatch(IQueryable<Flashcard> flashcards, int batchSize, int batchIndex);
        List<Flashcard> Shuffle(ref List<Flashcard> list, Random rand);
    }
}
