using FlashcardsManager.Core.Dtos;
using FlashcardsManager.Core.Enums;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Core.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashcardsManager.Api.Services
{
    public class AdministratorServices
    {
        private IUnitOfWork _unitOfWork;

        public AdministratorServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ChangeCategoryAvailability(int id, AvailabilityEnum newAvailability)
        {
            var category = await _unitOfWork.CategoryRepository.GetById(id);
            if (category.Availability == AvailabilityEnum.Private) return false;  //admin can't publish someone's private category
            category.Availability = newAvailability;
            _unitOfWork.CategoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public IQueryable<CategoryDto> GetPendingCategories()
        {
            return _unitOfWork.CategoryRepository.GetAll().Where(c => c.Availability == AvailabilityEnum.Pending)
                .Select(c => new CategoryDto(c));
        }
    }
}
