using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FlashcardsManager.Core.EF;
using FlashcardsManager.Core.Repositories;
using FlashcardsManager.Core.Repositories.Interfaces;
using FlashcardsManager.Core.UnitOfWork;
using FlashcardsManager.Api.Services;
using FlashcardsManager.Core.Models;

namespace FlashcardsManager.UnitTests
{
    public abstract class TestBase : IDisposable
    {
        protected IServiceProvider ServiceProvider;
        private readonly IServiceCollection _serviceCollection;

        protected AppDbContext Context { get; set; }
        protected IUnitOfWork UnitOfWork { get; set; }
        protected Api.Services.FilteringServices FilteringServices { get; set; }

        protected TestBase()
        {
            _serviceCollection = new ServiceCollection();
            Init();
            Context = ServiceProvider.GetService<AppDbContext>();
            UnitOfWork = ServiceProvider.GetService<IUnitOfWork>();
            FilteringServices = ServiceProvider.GetService<Api.Services.FilteringServices>();
        }

        public void Init()
        {
            _serviceCollection.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            });
            _serviceCollection.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            _serviceCollection.AddTransient<IUnitOfWork, UnitOfWork>();
            _serviceCollection.AddTransient<Api.Services.FilteringServices>();
            _serviceCollection.AddScoped<Seed>();

            ServiceProvider = _serviceCollection.BuildServiceProvider();
        }

        protected List<User> AddUsers(int count = 1)
        {
            var users = new List<User>();
            for (var i = 0; i < count; i++)
                users.Add(new User
                {
                    Id = i.ToString(), UserName = "I don't like writing tests " + i,
                    Name = "Mariusz " + i, Surname = "Wlazły " + i
                });
            AddToDatabase(users);
            return users;
        }

        protected List<Flashcard> AddFlashcards(Category category, int count = 1)
        {
            var flashcards = new List<Flashcard>();
            for (var i = 0; i < count; i++)
                flashcards.Add(new Flashcard { CategoryId = category.Id, Key = "Key " + i, Value = "Val " + i,
                    Category = category, KeyDescription = "Key desc " + i, ValueDescription = "Val desc " + i});
            AddToDatabase(flashcards);
            return flashcards;
        }

        protected List<Category> AddCategories(int count = 1)
        {
            var categories = new List<Category>();
            for (var i = 0; i < count; i++)
                categories.Add(new Category { Name = "Category " + i });
            AddToDatabase(categories);
            return categories;
        }

        protected List<UserProgress> AddUserProgress(IEnumerable<Flashcard> flashcards, IEnumerable<User> users)
        {
            var score = 5;
            var userProgress = (from flashcard in flashcards from user in users select new UserProgress(user, flashcard)).ToList();
            foreach (var progress in userProgress)
            {
                progress.Progress = score = (score * 89) % 109;
            }
            AddToDatabase(userProgress);
            return userProgress;
        }

        private void AddToDatabase<T>(IEnumerable<T> list) where T : class
        {
            Context.Set<T>().AddRange(list);
            Context.SaveChanges();
        }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }
    }
}
