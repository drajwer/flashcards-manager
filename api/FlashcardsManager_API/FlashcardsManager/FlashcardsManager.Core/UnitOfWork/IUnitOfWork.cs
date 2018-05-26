using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Core.Repositories.Interfaces;

namespace FlashcardsManager.Core.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Category> CategoryRepository { get; }
        IRepository<Flashcard> FlashcardRepository { get; }
        IRepository<User> UserRepository { get; }
        IRepository<UserProgress> UserProgressRepository { get; }

        Task SaveChangesAsync();

    }
}
