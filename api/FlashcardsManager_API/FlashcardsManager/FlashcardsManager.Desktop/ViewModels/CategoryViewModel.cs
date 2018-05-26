using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FlashcardsManager.Core.ApiClient;
using FlashcardsManager.Core.Enums;
using FlashcardsManager.Core.Helpers;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Desktop.Helpers;
using FlashcardsManager.Core.UnitOfWork;
using FlashcardsManager.Desktop.ModelMaps;
using FlashcardsManager.Desktop.Views;
using MaterialDesignThemes.Wpf;

namespace FlashcardsManager.Desktop.ViewModels
{
    public class CategoryViewModel : ObservableObject, IPageViewModel
    {
        #region Fields/Constructors
        private readonly ApiClient _apiClient;
        private string _categoryName;
        private ICommand _updateCategoryCommand;
        private ICommand _deleteCategoryCommand;
        private ICommand _submitFormCommand;
        private ICommand _openEditDialogCommand;
        private ICommand _openAddDialogCommand;
        private List<CategoryDatagridRow> _categories;
        private int _pageSize;
        private int _pageIndex;

        public CategoryViewModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
            _categoryName = "";
            SearchText = "";
            FilterCommand = new RelayCommand(async param =>
            {
                PageIndex = 0;
                await Update();
            });
            SortCommand = new RelayCommand(async param => await ColumnHeader_HandleClick(param as string));
            PrevPageCommand = new RelayCommand(async param => await PrevPage(), param => PageIndex > 0);
            NextPageCommand = new RelayCommand(async param => await NextPage(), param => _categories?.Count > 0);
            _pageIndex = 0;
            _pageSize = 10;
        }

        #endregion

        #region Properties/Commands

        public string Name => "Categories";

        public string SearchText { get; set; }

        public CategoriesSortingCriterion SortingCriterion { get; set; }

        public bool Descending { get; set; }

        public int PageIndex { get => _pageIndex; set { _pageIndex = value; OnPropertyChanged(); } }

        public int PageSize { get => _pageSize; set { _pageSize = value; OnPropertyChanged(); Update(); } }

        public ICommand PrevPageCommand { get; }

        public ICommand NextPageCommand { get; }


        public string CategoryName
        {
            get => _categoryName;
            set
            {
                _categoryName = value;
                OnPropertyChanged();
            }
        }

        public List<CategoryDatagridRow> Categories
        {
            get
            {
                if (_categories == null)
                {
                    Task.Run(async () =>
                    {
                        return _categories = new List<CategoryDatagridRow>(await GetCategories());
                    });
                }
                return _categories;
            }
            set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }

        public ICommand FilterCommand { get; }

        public ICommand SortCommand { get; }

        public ICommand SubmitFormCommand
        {
            get
            {
                return _submitFormCommand ?? (_submitFormCommand = new RelayCommand(
                           async param => await SubmitForm(param as Category),
                           param =>
                               !string.IsNullOrEmpty((param as Category)?.Name) &&
                               !string.IsNullOrWhiteSpace((param as Category)?.Name) &&
                               !Categories.Any(row => row.Category.Name == (param as Category).Name)
                       ));
            }
        }
        public ICommand OpenAddDialogCommand
        {
            get
            {
                return _openAddDialogCommand ??
                       (_openAddDialogCommand = new RelayCommand(async param => await OpenAddDialog()));
            }
            set
            {
                if (value == _openAddDialogCommand) return;
                _openAddDialogCommand = value;
                OnPropertyChanged();
            }
        }

        public ICommand OpenEditDialogCommand
        {
            get
            {
                return _openEditDialogCommand ??
                       (_openEditDialogCommand =
                           new RelayCommand(async param => await OpenEditDialog(param as CategoryDatagridRow),
                               param => param is CategoryDatagridRow
                           ));
            }
        }

        public ICommand UpdateCategoryCommand
        {
            get
            {
                return _updateCategoryCommand ?? (_updateCategoryCommand =
                           new RelayCommand(async param => await UpdateCategory(param as Category),
                               param =>
                                   !string.IsNullOrEmpty((param as Category)?.Name) &&
                                   !string.IsNullOrWhiteSpace((param as Category).Name) &&
                                   !Categories.Any(row => row.Category.Name == (param as Category).Name)));
            }
        }

        public ICommand DeleteCategoryCommand
        {
            get
            {
                return _deleteCategoryCommand ?? (_deleteCategoryCommand =
                           new RelayCommand(async param => await DeleteCategory(param as CategoryDatagridRow),
                               param => param is CategoryDatagridRow));
            }
        }

        #endregion

        #region Methods
        private async Task DeleteCategory(CategoryDatagridRow categoryDatagridRow)
        {
            var result = MessageBox.Show("Are you sure you want to delete this item?", "Flashcards Manager", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
                return;
            var category = categoryDatagridRow.Category;
            await DeleteCategories(new List<Category>() { category });
        }

        private async Task DeleteCategories(List<Category> categoriesToDelete)
        {
            if (categoriesToDelete == null) return;
            foreach (var category in categoriesToDelete)
            {
                await _apiClient.DeleteJsonAsync<Category>(ApiUrls.CategoriesEndpoint, category.Id);
            }
            await Update();
        }
        private async Task OpenAddDialog()
        {
            var vm = new CategoryFormViewModel(SubmitFormCommand, "ADD");
            await DialogHost.Show(new CategoryForm() { DataContext = vm });
        }

        private async Task OpenEditDialog(CategoryDatagridRow categoryDatagridRow)
        {
            var category = categoryDatagridRow.Category;
            var formCategory = new Category()
            {
                Id = category.Id,
                Name = category.Name,
                //Flashcard = category.Flashcard
            };
            var vm = new CategoryFormViewModel(UpdateCategoryCommand, "UPDATE", formCategory);
            await DialogHost.Show(new CategoryForm() { DataContext = vm });
        }

        public async Task Update()
        {
            Categories = new List<CategoryDatagridRow>(await GetCategories());
        }

        private async Task<List<CategoryDatagridRow>> GetCategories()
        {
            var filteringModel = new CategoriesFilteringModel(SearchText, SortingCriterion, Descending, PageIndex, PageSize);
            var categories = await _apiClient.PostJsonAsync<CategoriesFilteringModel, List<Category>>(ApiUrls.CategoriesEndpoint + "/filter",
                filteringModel);
            var rows = categories.Select(c => new CategoryDatagridRow(c, c.Name)).ToList();
            foreach (var row in rows)
            {
                row.DeleteCommand = DeleteCategoryCommand;
                row.EditCommand = OpenEditDialogCommand;
                row.ViewModel = this;
            }
            return rows;
        }

        private async Task UpdateCategory(Category updatedCategory)
        {
            if (string.IsNullOrWhiteSpace(updatedCategory?.Name) || string.IsNullOrEmpty(updatedCategory.Name)) return;
            await _apiClient.PutJsonAsync<Category, object>(ApiUrls.CategoriesEndpoint, updatedCategory.Id, updatedCategory);
            await Update();
        }
        private async Task SubmitForm(Category formCategory)
        {
            await _apiClient.PostJsonAsync<Category, object>(ApiUrls.CategoriesEndpoint, formCategory);
            await Update();
        }

        private async Task PrevPage()
        {
            if (PageIndex > 0) PageIndex--;
            await Update();
        }

        private async Task NextPage()
        {
            PageIndex++;
            await Update();
        }

        private async Task ColumnHeader_HandleClick(string header)
        {
            foreach (var enumValue in Enum.GetValues(typeof(CategoriesSortingCriterion)))
            {
                var criterion = enumValue is CategoriesSortingCriterion sortingCriterion ? sortingCriterion : CategoriesSortingCriterion.None;
                if (criterion.ToString() != header) continue;
                if (SortingCriterion != criterion) Descending = false;
                else Descending = !Descending;
                SortingCriterion = criterion;
                await Update();
                return;
            }
        }
        #endregion
    }
}
