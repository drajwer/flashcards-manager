using FlashcardsManager.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Core.Options;
using FlashcardsManager.Core.Repositories;
using FlashcardsManager.Core.UnitOfWork;
using Microsoft.Extensions.Options;

namespace FlashcardsManager.UnitTests.Learning
{
    public class LearningServiceTestBase
    {
        protected LearningService Service;
        protected LearningServiceOptions Options;
        protected IUnitOfWork UnitOfWork;

        public LearningServiceTestBase()
        {
            UnitOfWork = new ListUnitOfWork(new ListRepository<Category>(), new ListRepository<Flashcard>(),
                new ListRepository<User>(), new ListRepository<UserProgress>());
            Options = new LearningServiceOptions();
            Service = new LearningService(UnitOfWork, new OptionsWrapper<LearningServiceOptions>(Options));
        }

        protected async Task<List<User>> AddUsers(int count = 1)
        {
            var users = new List<User>();
            for (var i = 0; i < count; i++)
                users.Add(new User { Id = i.ToString(), UserName = "I don't like writing tests " + i });
            foreach (var user in users)
            {
                await UnitOfWork.UserRepository.Add(user);
            }
            return users;
        }

        protected async Task<List<Flashcard>> AddFlashcards(Category category, int count = 1)
        {
            var flashcards = new List<Flashcard>();
            for (var i = 0; i < count; i++)
                flashcards.Add(new Flashcard { Id= i + 1, CategoryId = category.Id, Key = "Key " + i, Value = "Val " + i });
            foreach (var flashcard in flashcards)
            {
                await UnitOfWork.FlashcardRepository.Add(flashcard);
            }
            return flashcards;
        }

        protected async Task<List<Category>> AddCategories(int count = 1)
        {
            var categories = new List<Category>();
            for (var i = 0; i < count; i++)
                categories.Add(new Category { Name = "Category " + i });
            foreach (var category in categories)
            {
                await UnitOfWork.CategoryRepository.Add(category);
            }
            return categories;
        }

        protected async Task<List<UserProgress>> AddUserProgress(IEnumerable<Flashcard> flashcards, IEnumerable<User> users)
        {
            var userProgress = (from flashcard in flashcards from user in users select new UserProgress(user, flashcard)).ToList();
            foreach (var progress in userProgress)
            {
                await UnitOfWork.UserProgressRepository.Add(progress);
            }
            return userProgress;
        }

        protected void SetDefaultOpts()
        {
            Options.MaxHardProgress = -1;
            Options.MinKnownProgress = 1;
            Options.OnFailure = -2;
            Options.OnSuccess = 2;
            Options.OnPartial = 1;
            Options.MaxProgress = 2;
            Options.MinProgress = -2;
        }

    }
}
