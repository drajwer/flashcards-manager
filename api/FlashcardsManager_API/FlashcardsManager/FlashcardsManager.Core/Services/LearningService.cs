using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlashcardsManager.Core.Enums;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Core.Options;
using FlashcardsManager.Core.UnitOfWork;
using Microsoft.Extensions.Options;

namespace FlashcardsManager.Core.Services
{
    public class LearningService : ILearningService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly LearningServiceOptions _opts;

        public LearningService(IUnitOfWork unitOfWork, IOptions<LearningServiceOptions> optsAccessor)
        {
            _unitOfWork = unitOfWork;
            _opts = optsAccessor.Value;
        }

        public async Task<List<Flashcard>> GetFlashcards(User user, int categoryId, FlashcardsSearchCriterionEnum mode, int count)
        {
            EnsureUser(user);
            var category = await _unitOfWork.CategoryRepository.GetById(categoryId);
            EnsureCategory(category);
            var flashcards = _unitOfWork.FlashcardRepository.GetAll();
            flashcards = flashcards.Where(f => f.CategoryId == categoryId);
            flashcards = FilterFlashcards(flashcards, user, mode);

            flashcards = flashcards.OrderBy(r => Guid.NewGuid()).Take(count);
            return flashcards.ToList();
        }

        private IQueryable<Flashcard> FilterFlashcards(IQueryable<Flashcard> flashcards, User user, FlashcardsSearchCriterionEnum mode)
        {
            if (mode == FlashcardsSearchCriterionEnum.New)
                flashcards = flashcards.Where(f => f.UserProgress.All(up => up.UserId != user.Id));
            else if (mode != FlashcardsSearchCriterionEnum.All)
            {
                flashcards = flashcards.Where(f =>
                    f.UserProgress.Any(up => up.UserId == user.Id && IsProgressMatchMode(up.Progress, mode)));
            }

            return flashcards;
        }

        private bool IsProgressMatchMode(int progress, FlashcardsSearchCriterionEnum mode)
        {
            int maxHardProgress = _opts.MaxHardProgress;
            int minKnownProgress = _opts.MinKnownProgress;
            switch (mode)
            {
                case FlashcardsSearchCriterionEnum.Hard:
                    return progress <= maxHardProgress;
                case FlashcardsSearchCriterionEnum.Known:
                    return progress >= minKnownProgress;
                case FlashcardsSearchCriterionEnum.Old:
                    return true;
                default:
                    throw new ArgumentException(nameof(mode));
            }
        }

        private static void EnsureCategory(Category category)
        {
            if (category == null)
                throw new ArgumentException(nameof(category));
        }

        public async Task ProceedFlashcard(int flashcardId, User user, FlashcardResult result)
        {
            var flashcard = await _unitOfWork.FlashcardRepository.GetById(flashcardId);
            EnsureUser(user);
            EnsureFlashcard(flashcard);

            var userProgress = await _unitOfWork.UserProgressRepository.GetById(user.Id, flashcardId);
            if (userProgress == null)
            {
                userProgress = new UserProgress(user, flashcard);
                userProgress.Progress = CalculateNewProgress(userProgress.Progress, result);
                await _unitOfWork.UserProgressRepository.Add(userProgress);
            }
            else
            {
                userProgress.Progress = CalculateNewProgress(userProgress.Progress, result);
                _unitOfWork.UserProgressRepository.Update(userProgress);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        private static void EnsureFlashcard(Flashcard flashcard)
        {
            if (flashcard == null)
            {
                throw new ArgumentException(nameof(flashcard));
            }
        }

        private static void EnsureUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
        }

        private int CalculateNewProgress(int oldProgress, FlashcardResult result)
        {
            int minProgress = _opts.MinProgress;
            int maxProgress = _opts.MaxProgress;
            int newProgress = oldProgress;
            switch (result)
            {
                case FlashcardResult.Success:
                    newProgress += _opts.OnSuccess;
                    break;
                case FlashcardResult.Partial:
                    newProgress += _opts.OnPartial;
                    break;
                case FlashcardResult.Fail:
                    newProgress += _opts.OnFailure;
                    break;
                default:
                    throw new ArgumentException(nameof(result));
            }
            newProgress = Math.Max(newProgress, minProgress);
            newProgress = Math.Min(newProgress, maxProgress);
            return newProgress;
        }

        public Score GetUserScore(User user)
        {
            EnsureUser(user);
            var userProgress = _unitOfWork.UserProgressRepository.GetAll().Where(up => up.UserId == user.Id);
            var sumPoints = userProgress.Sum(up => up.Progress);

            return new Score(sumPoints);
        }

        public async Task<Score> GetUserScoreForCategory(User user, int categoryId)
        {
            EnsureUser(user);
            var category = await _unitOfWork.CategoryRepository.GetById(categoryId);
            EnsureCategory(category);
            var userProgress = _unitOfWork.UserProgressRepository.GetAll()
                .Where(up => up.UserId == user.Id && up.Flashcard.Category.Id == categoryId);
            var sumPoints = userProgress.Sum(up => up.Progress);

            return new Score(sumPoints);
        }
    }
}
