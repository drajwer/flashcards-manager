using System;
using System.Linq;
using FlashcardsManager.Core.EF;
using FlashcardsManager.Core.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FlashcardsManager.Core.Repositories
{
    public class Repository<TEntity>: IRepository<TEntity> where TEntity : class
     {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(AppDbContext context)
        {
            _dbContext = context;
            _dbSet = context.Set<TEntity>();
        }
        public async Task<TEntity> GetById(params object[] keyValues)
        {
            if(keyValues == null) throw new ArgumentNullException(typeof(TEntity).FullName);
            if (keyValues.Length == 0) throw new ArgumentException(typeof(TEntity).FullName);

            return await _dbSet.FindAsync(keyValues);
        }

        public async Task Add(TEntity entity)
        {
            if(entity == null) throw new ArgumentNullException(typeof(TEntity).FullName);
            await _dbSet.AddAsync(entity);
        }

        public void Update(TEntity updatedEntity)
        {
            if (updatedEntity == null) throw new ArgumentNullException(typeof(TEntity).FullName);
            _dbSet.Update(updatedEntity);
        }

        public void Delete(TEntity entity)
        {
            if(entity == null) throw new ArgumentNullException(typeof(TEntity).FullName);
            _dbSet.Remove(entity);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbSet.AsQueryable();
        }
    }
}
