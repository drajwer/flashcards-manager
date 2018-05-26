using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Core.Repositories.Interfaces;

namespace FlashcardsManager.Core.UnitOfWork
{
    public class ListUnitOfWork : IUnitOfWork
    {
        public IRepository<Category> CategoryRepository { get; }
        public IRepository<Flashcard> FlashcardRepository { get; }
        public IRepository<User> UserRepository { get; }
        public IRepository<UserProgress> UserProgressRepository { get; }

        public ListUnitOfWork(IRepository<Category> categoryRepository, IRepository<Flashcard> flashcardRepository, IRepository<User> userRepository, IRepository<UserProgress> userProgressRepository)
        {
            CategoryRepository = categoryRepository;
            FlashcardRepository = flashcardRepository;
            UserRepository = userRepository;
            UserProgressRepository = userProgressRepository;
        }

        public void Dispose()
        {
        }

        public async Task SaveChangesAsync()
        {
            
        }
    }
}
