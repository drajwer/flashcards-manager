using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashcardsManager.Core.EF;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Core.Repositories.Interfaces;

namespace FlashcardsManager.Core.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IRepository<Category> CategoryRepository { get; }
        public IRepository<Flashcard> FlashcardRepository { get; }
        public IRepository<User> UserRepository { get; }
        public IRepository<UserProgress> UserProgressRepository { get; }

        private readonly AppDbContext _dbContext;

        public UnitOfWork(AppDbContext dbContext, IRepository<Category> categoryRepository, IRepository<Flashcard> flashcardRepository, IRepository<User> userRepository, IRepository<UserProgress> userpProgressRepository)
        {
            _dbContext = dbContext;
            CategoryRepository = categoryRepository;
            FlashcardRepository = flashcardRepository;
            UserRepository = userRepository;
            UserProgressRepository = userpProgressRepository;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
