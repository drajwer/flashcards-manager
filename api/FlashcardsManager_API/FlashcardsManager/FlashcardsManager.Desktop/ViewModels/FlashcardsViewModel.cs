using System;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Desktop.Helpers;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FlashcardsManager.Core.ApiClient;
using FlashcardsManager.Core.Enums;
using FlashcardsManager.Core.Helpers;
using FlashcardsManager.Core.UnitOfWork;
using FlashcardsManager.Desktop.ModelMaps;

namespace FlashcardsManager.Desktop
{
    public class FlashcardsViewModel : ObservableObject, IPageViewModel
    {
        #region Fields/Constructors

        private readonly ApiClient _apiClient;
        private ICommand _submitFormCommand;
        private ICommand _deleteFlashcardsCommand;
        private ICommand _deleteFlashcardCommand;
        private Flashcard _formFlashcard;
        private List<FlashcardDatagridRow> _flashcards;
        private List<Category> _availibleCategories;
        private int _pageSize;
        private int _pageIndex;

        public FlashcardsViewModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
            SearchText = "";
            FilterCommand = new RelayCommand(async param =>
            {
                PageIndex = 0;
                await Update();
            });
            SortCommand = new RelayCommand(async param => await ColumnHeader_HandleClick(param as string));
            PrevPageCommand = new RelayCommand(async param => await PrevPage(), param => PageIndex > 0);
            NextPageCommand = new RelayCommand(async param => await NextPage(), param =>_flashcards?.Count > 0);
            _pageIndex = 0;
            _pageSize = 10;
        }

        #endregion

        #region Properties/Commands

        public string Name => "Flashcards";

        public string SearchText { get; set; }

        public FlashcardsSortingCriterion SortingCriterion { get; set; }

        public bool Descending { get; set; }

        public int PageIndex { get => _pageIndex; set { _pageIndex = value; OnPropertyChanged(); } }

        public int PageSize { get => _pageSize; set { _pageSize = value; OnPropertyChanged(); Update(); } }

        public ICommand FilterCommand { get; }

        public ICommand SortCommand { get; }

        public ICommand PrevPageCommand { get; }

        public ICommand NextPageCommand { get; }

        public List<FlashcardDatagridRow> Flashcards
        {
            get
            {
                if (_flashcards == null)
                {
                    Task.Run(async () => { _flashcards = await GetFlashcards(); });
                }
                return _flashcards;
            }
            set
            {
                if (_flashcards == value)
                    return;
                _flashcards = value;
                OnPropertyChanged();
            }
        }

        public List<Category> AvailibleCategories
        {
            get
            {
                if (_availibleCategories == null)
                    Task.Run(async () => { _availibleCategories = await GetCategories(); });
                return _availibleCategories;
            }
            set
            {
                if (_availibleCategories != null && _availibleCategories != value)
                {
                    _availibleCategories = value;
                    OnPropertyChanged();
                }
            }
        }

        public Flashcard FormFlashcard
        {
            get
            {
                if (_formFlashcard == null)
                {
                    _formFlashcard = new Flashcard();
                }
                return _formFlashcard;
            }
            set
            {
                if (value != _formFlashcard)
                {
                    _formFlashcard = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SubmitFormCommand
        {
            get
            {
                if (_submitFormCommand == null)
                {
                    _submitFormCommand = new RelayCommand(
                        async param => await SubmitForm(),
                        param => !string.IsNullOrEmpty(FormFlashcard.Key) && !string.IsNullOrEmpty(FormFlashcard.Value) && FormFlashcard.Category != null
                    );
                }
                return _submitFormCommand;
            }
        }

        public ICommand DeleteFlashcardCommand
        {
            get
            {
                if (_deleteFlashcardCommand == null)
                {
                    _deleteFlashcardCommand = new RelayCommand(
                        async param => await DeleteFlashcard(param as FlashcardDatagridRow),
                        param => true//((param as FlashcardDatagridRow) != null)
                    );
                }
                return _deleteFlashcardCommand;
            }
        }

        private async Task DeleteFlashcard(FlashcardDatagridRow flashcardDatagridRow)
        {
            var flashcard = flashcardDatagridRow.Flashcard;
            var result = MessageBox.Show("Are you sure you want to delete this item?", "Flashcards Manager",
                MessageBoxButton.YesNo);
            if (flashcard == null || result != MessageBoxResult.Yes)
                return;
            await DeleteFlashcards(new List<Flashcard> { flashcard });
        }

        public ICommand DeleteFlashcardsCommand
        {
            get
            {
                if (_deleteFlashcardsCommand == null)
                {
                    _deleteFlashcardsCommand = new RelayCommand(
                        async param => await DeleteFlashcards(param as List<Flashcard>),
                        param => ((param as List<Flashcard>) != null)
                    );
                }
                return _deleteFlashcardsCommand;
            }
        }

        #endregion

        #region Methods
        public async Task Update()
        {
            Flashcards = await GetFlashcards();
            AvailibleCategories = await GetCategories();
        }

        private async Task DeleteFlashcards(List<Flashcard> flashcardsToDelete)
        {
            if (flashcardsToDelete == null)
                return;
            foreach (var flashcard in flashcardsToDelete)
            {
                await _apiClient.DeleteJsonAsync<Flashcard>(ApiUrls.FlashcardsEndpoint, flashcard.Id);
            }

            Flashcards = await GetFlashcards();
        }

        private async Task SubmitForm()
        {
            var flashcard = FormFlashcard;
            flashcard.CategoryId = FormFlashcard.Category.Id;
            flashcard.Category = null;
            await _apiClient.PostJsonAsync<Flashcard, object>(ApiUrls.FlashcardsEndpoint, flashcard);
            FormFlashcard = new Flashcard();
            Flashcards = await GetFlashcards();
        }

        private async Task<List<FlashcardDatagridRow>> GetFlashcards()
        {
            var filteringModel = new FlashcardsFilteringModel(SearchText, SortingCriterion, Descending, PageIndex, PageSize);
            var flashcards = await _apiClient.PostJsonAsync<FlashcardsFilteringModel, List<Flashcard>>(ApiUrls.FlashcardsEndpoint + "/filter",
                filteringModel);
            //foreach (var flashcard in flashcards)
            //{
            //    flashcard.Category = this.AvailibleCategories.FirstOrDefault(c => c.Id == flashcard.CategoryId);
            //}
            var rows = flashcards.Select(f => new FlashcardDatagridRow(f, DeleteFlashcardCommand)).ToList();

            return rows;
        }

        private async Task<List<Category>> GetCategories()
        {
            return await _apiClient.GetJsonAsync<List<Category>>(ApiUrls.CategoriesEndpoint);
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
            foreach (var enumValue in Enum.GetValues(typeof(FlashcardsSortingCriterion)))
            {
                var criterion = enumValue is FlashcardsSortingCriterion sortingCriterion ? sortingCriterion : FlashcardsSortingCriterion.None;
                if (criterion.ToString().ToLower() != header.ToLower().Replace(" ", "")) continue;
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
