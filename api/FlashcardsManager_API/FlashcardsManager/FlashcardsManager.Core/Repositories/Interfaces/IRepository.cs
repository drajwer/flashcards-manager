using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashcardsManager.Core.Models;

namespace FlashcardsManager.Core.Repositories.Interfaces
{
    public interface IRepository<TType> where TType : class
    {
        Task Add(TType entity);
        void Update(TType updatedEntity);
        void Delete(TType entity);
        Task<TType> GetById(params object[] id);
        IQueryable<TType> GetAll();
    }
}
