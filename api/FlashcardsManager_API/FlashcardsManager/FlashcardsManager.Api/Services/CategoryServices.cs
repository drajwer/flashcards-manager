using FlashcardsManager.Core.Dtos;
using FlashcardsManager.Core.Enums;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Core.Repositories.Interfaces;
using FlashcardsManager.Core.UnitOfWork;
using System.Linq;
using System.Threading.Tasks;

namespace FlashcardsManager.Api.Services
{
    public class CategoryServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Category> _repository;

        public CategoryServices(IUnitOfWork unitOfWork, IRepository<Category> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public IQueryable<Category> Get(string userId, bool isAdmin)
        {
            return isAdmin ? _repository.GetAll() : _repository.GetAll().Where(c => c.CreatorId == userId || c.Availability == AvailabilityEnum.Public);
        }

        public IQueryable<Category> GetUsersOwnCategories(string userId, bool isAdmin)
        {
            return isAdmin ? _repository.GetAll() : _repository.GetAll().Where(c => c.CreatorId == userId);
        }

        public async Task<Category> Get(int id, string userId, bool isAdmin)
        {
            var category = await _repository.GetById(id);
            if (category == null || !HasPrivilege(category, userId, isAdmin) || !IsPublic(category))
                return null;
            return category;
        }

        public async Task<Category> Post(CategoryDto categoryDto, string userId)
        {
            var category = categoryDto.ToEntity();
            category.Id = 0;
            category.CreatorId = userId;
            if (category.Availability == AvailabilityEnum.Public)
                category.Availability = AvailabilityEnum.Pending;
            await _repository.Add(category);
            await _unitOfWork.SaveChangesAsync();
            return category;
        }

        public async Task<bool> Update(int id, CategoryDto categoryToUpdate, string userId)
        {
            var category = await _repository.GetById(id);
            if (category == null || !HasPrivilege(category, userId, false))
                return false;
            category.Name = categoryToUpdate.Name;
            category.Availability = categoryToUpdate.Availability == AvailabilityEnum.Public ?
                AvailabilityEnum.Pending : categoryToUpdate.Availability;
            _repository.Update(category);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id, string userId, bool isAdmin)
        {
            var category = await _repository.GetById(id);
            if (category == null || !HasPrivilege(category, userId, isAdmin))
                return false;

            _repository.Delete(category);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private bool HasPrivilege(Category category, string userId, bool isAdmin)
        {
            return isAdmin || category.CreatorId == userId;
        }

        private bool IsPublic(Category category)
        {
            return category.Availability == AvailabilityEnum.Public;
        }
    }
}
