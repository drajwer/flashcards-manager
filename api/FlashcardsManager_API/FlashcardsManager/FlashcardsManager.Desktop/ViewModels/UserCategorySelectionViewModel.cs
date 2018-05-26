using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FlashcardsManager.Core.Enums;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Core.Services;
using FlashcardsManager.Core.UnitOfWork;
using FlashcardsManager.Desktop.Helpers;
using FlashcardsManager.Core.ApiClient;

namespace FlashcardsManager.Desktop.ViewModels
{

    public class UserCategorySelectionViewModel : ObservableObject, IPageViewModel
    {
        #region Fields/Constructors

        private ICommand _startLearningCommand;
        private Category _selectedCategory;
        private FlashcardsSearchCriterionEnum _selectedCriterion;
        private ApiClient _apiClient;
        private List<Category> _categories;

        public UserCategorySelectionViewModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
            SearchCriteria = GetEnumValues();
            _selectedCriterion = SearchCriteria.ElementAt(0);
            _selectedCategory = null;
            Update();
        }

        #endregion

        #region Properties/Commands

        public string Name => "Start learning";
        public async Task Update()
        {
            Categories = await GetCategoriesAsync();
        }

        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
            }
        }

        public List<Category> Categories
        {
            get
            {
                return _categories;
            }
            private set
            {
                if (_categories == value)
                    return;
                _categories = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<FlashcardsSearchCriterionEnum> SearchCriteria { get; }

        public FlashcardsSearchCriterionEnum SelectedSearchCriterion
        {
            get { return _selectedCriterion; }
            set { _selectedCriterion = value; OnPropertyChanged(); }
        }

        public ICommand StartLearningCommand
        {
            get
            {
                return _startLearningCommand ?? (_startLearningCommand =
                           new RelayCommand(async param => await StartLearningAsync(),
                               param => (_selectedCategory != null)));
            }
        }

        #endregion

        #region Methods

        private async Task StartLearningAsync()
        {
            if (_selectedCategory == null) return;
            var dialogVM = new LearningDialogViewModel(_apiClient, await GetFlashcardsToLearn());
            var dialog = new DialogWindow(dialogVM);
            dialog.ShowDialog();
        }
        private async Task<List<Flashcard>> GetFlashcardsToLearn()
        {
            string url = $"{ApiUrls.LearningFlashcardsEndpoint}/{SelectedCategory.Id}?strategy={(int)SelectedSearchCriterion}";
            return await _apiClient.GetJsonAsync<List<Flashcard>>(url);
        } 

        private async Task<List<Category>> GetCategoriesAsync()
        {
            return await _apiClient.GetJsonAsync<List<Category>>(ApiUrls.CategoriesEndpoint);
        }


        private IEnumerable<FlashcardsSearchCriterionEnum> GetEnumValues()
        {
            return Enum.GetValues(typeof(FlashcardsSearchCriterionEnum))
                               .Cast<FlashcardsSearchCriterionEnum>()
                               .ToList();
        }

        #endregion
    }
}
