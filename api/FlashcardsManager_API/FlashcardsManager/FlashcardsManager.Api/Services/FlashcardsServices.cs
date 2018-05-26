using FlashcardsManager.Core.Dtos;
using FlashcardsManager.Core.Enums;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Core.Repositories.Interfaces;
using FlashcardsManager.Core.UnitOfWork;
using System.Linq;
using System.Threading.Tasks;

namespace FlashcardsManager.Api.Services
{
    public class FlashcardsServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Flashcard> _repository;

        public FlashcardsServices(IUnitOfWork unitOfWork, IRepository<Flashcard> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public IQueryable<Flashcard> Get(string userId, bool isAdmin)
        {
            return isAdmin ? _repository.GetAll() :
                    _repository.GetAll().Where(f => f.Category.CreatorId == userId || f.Category.Availability == AvailabilityEnum.Public);
        }

        public async Task<Flashcard> Get(int id, string userId, bool isAdmin)
        {
            var flashcard = await _repository.GetById(id);
            if (!await HasPrivilege(flashcard, userId, isAdmin) && !await IsPublic(flashcard))
                return null;
            return flashcard;
        }

        public IQueryable<Flashcard> GetFlashcardsOfCategory(int categoryId, string userId, bool isAdmin)
        {
            return isAdmin ? _repository.GetAll().Where(f => f.CategoryId == categoryId)
                :
                _repository.GetAll().Where(f => f.CategoryId == categoryId && (f.Category.CreatorId == userId
                                            || f.Category.Availability == AvailabilityEnum.Public));
        }

        public async Task<Flashcard> Post(FlashcardDto flashcardDto, string userId, bool isAdmin)
        {
            var flashcard = flashcardDto.ToEntity();
            if (!(await HasPrivilege(flashcard, userId, isAdmin)) && !await IsPublic(flashcard))
                return null;
            await _repository.Add(flashcard);
            await _unitOfWork.SaveChangesAsync();
            return flashcard;
        }

        public async Task<bool> Delete(int id, string userId, bool isAdmin)
        {
            var flashcard = await _repository.GetById(id);
            if (!await HasPrivilege(flashcard, userId, isAdmin))
                return false;

            _repository.Delete(flashcard);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private async Task<bool> HasPrivilege(Flashcard flashcard, string userId, bool isAdmin)
        {
            if (flashcard == null) return false;
            var category = await _unitOfWork.CategoryRepository.GetById(flashcard.CategoryId);
            return category != null && (isAdmin || category.CreatorId == userId);
        }

        private async Task<bool> IsPublic(Flashcard flashcard)
        {
            var category = await _unitOfWork.CategoryRepository.GetById(flashcard.CategoryId);
            return category != null && category.Availability == AvailabilityEnum.Public;

        }
    }
}
