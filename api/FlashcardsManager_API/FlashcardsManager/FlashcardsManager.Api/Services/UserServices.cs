using FlashcardsManager.Core.Dtos;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Core.Repositories.Interfaces;
using FlashcardsManager.Core.UnitOfWork;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FlashcardsManager.Api.Services
{
    public class UserServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<User> _repository;

        public UserServices(IUnitOfWork unitOfWork, IRepository<User> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public IQueryable<User> Get()
        {
            return _repository.GetAll().Include(u => u.UserProgress).AsQueryable();
        }

        public User Get(string id)
        {
            return Get().FirstOrDefault(u => u.Id == id);
        }

        public async Task<bool> Update(string id, UserDto userToUpdate, string userId, bool isAdmin)
        {
            if (!HasPrivilege(id, userId, isAdmin))
                return false;
            var user = await _repository.GetById(id);
            if (user == null)
                return false;

            user.Name = userToUpdate.Name;
            user.Surname = userToUpdate.Surname;

            _repository.Update(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(string id, string userId, bool isAdmin)
        {
            if (!HasPrivilege(id, userId, isAdmin))
                return false;
            var user = await _repository.GetById(id);
            if (user == null)
                return false;

            _repository.Delete(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private bool HasPrivilege(string id, string userId, bool isAdmin)
        {
            return isAdmin || id == userId;
        }
    }
}
