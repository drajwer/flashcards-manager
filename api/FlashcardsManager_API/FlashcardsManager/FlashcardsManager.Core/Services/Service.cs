using System;
using System.Collections.Generic;
using System.Linq;
using FlashcardsManager.Core.Enums;
using FlashcardsManager.Core.Helpers;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Core.UnitOfWork;
using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;

namespace FlashcardsManager.Core.Services
{
    public class Service : IService
    {
        private readonly IUnitOfWork _unitOfWork;

        public Service(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ChangeUserProgress(Flashcard flashcard, User user, int value)
        {
            var userProgress = (await _unitOfWork.UserProgressRepository.GetById(user.Id, flashcard.Id)) ??
                               new UserProgress(user, flashcard);
            userProgress.Progress += value;
            _unitOfWork.UserProgressRepository.Update(userProgress);
            await _unitOfWork.SaveChangesAsync();
        }

        public IQueryable<Flashcard> FilterFlashcards(FlashcardsSearchModel searchModel)
        {
            if(searchModel == null) throw new ArgumentNullException();
            var flashcards = _unitOfWork.FlashcardRepository.GetAll();
            flashcards = flashcards.Where(f => f.CategoryId == searchModel.CategoryId);
            switch (searchModel.FlashcardsSearchCriterionEnum)
            {
                // new flashcards for specific user are those that don't have corresponding userProgress in database
                case FlashcardsSearchCriterionEnum.New:
                    flashcards =
                        flashcards.Where(f => f.UserProgress.All(up => up.UserId != searchModel.UserId));
                    break;
                case FlashcardsSearchCriterionEnum.Old:
                    flashcards =
                        flashcards.Where(f => f.UserProgress.Any(up => up.UserId == searchModel.UserId));
                    break;
            }
            return flashcards;
        }

        public IQueryable<Flashcard> GetBatch(IQueryable<Flashcard> flashcards, int batchSize, int batchIndex)
        {
            if(flashcards == null) throw new ArgumentNullException();
            if(batchSize < 0 || batchIndex < 0) throw new ArgumentOutOfRangeException();
            return flashcards.OrderBy(f => 1).Skip(batchIndex*batchSize).Take(batchSize);
        }
        public List<Flashcard> Shuffle(ref List<Flashcard> list, Random rand)
        {
            if (list == null || rand == null) throw new ArgumentNullException();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rand.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
    }
}
