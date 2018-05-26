using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Core.Repositories.Interfaces;

namespace FlashcardsManager.Core.Repositories
{
    public class ListRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private List<TEntity> _entities;
        public ListRepository()
        {
            _entities = new List<TEntity>();
        }


        public async Task Add(TEntity entity)
        {
            var category = entity as Category;
            if(category != null)
                category.Id = _entities.Count;
            _entities.Add(entity);
        }

        public void Update(TEntity updatedEntity)
        {
            // Does nothing
        }

        public void Delete(TEntity entity)
        {
            _entities.Remove(entity);
        }       
        public async Task<TEntity> GetById(params object[] id)
        {
            List<Category> categories = _entities as List<Category>;
            List<Flashcard> flashcards  = _entities as List<Flashcard>;
            List<User> users = _entities as List<User>;
            List<UserProgress> userProgress = _entities as List<UserProgress>;

            if (categories != null)
            {
                return categories.FirstOrDefault(c => c.Id == (int)id[0]) as TEntity;
            }
            if (flashcards != null)
            {
                return flashcards.FirstOrDefault(c => c.Id == (int)id[0]) as TEntity;
            }
            if (users != null)
            {
                return users.FirstOrDefault(c => c.Id == (string)id[0]) as TEntity;
            }
            return userProgress?.FirstOrDefault(up => up.UserId == (string) id[0] && up.FlashcardId == (int) id[1]) as TEntity; 
        }

        public IQueryable<TEntity> GetAll()
        {
            return _entities.AsQueryable();
        }
    }
}
