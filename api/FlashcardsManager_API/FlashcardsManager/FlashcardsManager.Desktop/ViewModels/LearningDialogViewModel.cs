using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using FlashcardsManager.Core.Enums;
using FlashcardsManager.Core.Helpers;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Core.Services;
using FlashcardsManager.Desktop.Helpers;
using FlashcardsManager.Core.ApiClient;
using FlashcardsManager.Core.ViewModels;
using System.Threading.Tasks;

namespace FlashcardsManager.Desktop.ViewModels
{
    public class LearningDialogViewModel : ObservableObject
    {
        #region Fields/Constructors
        private readonly ApiClient _apiClient;
        private readonly Category _category;
        private readonly User _user;
        private readonly FlashcardsSearchCriterionEnum _searchCriterionEnum;
        //private readonly IService _service;
        private Flashcard _flashcard;
        private List<Flashcard> _flashcards;
        private bool _isCardFlapped;
        private string _question;
        private string _description;
        private readonly Random _random;
        private int _batchIndex;
        private int _flashcardIndex;

        // commands
        private ICommand _flipCardCommand;
        private ICommand _correctAnswerCommand;
        private ICommand _incompleteAnswerCommand;
        private ICommand _wrongAnswerCommand;

        // counters
        private int _wrongCount;
        private int _partialCount;
        private int _correctCount;
        public LearningDialogViewModel(ApiClient apiClient, List<Flashcard> flashcards)
        {
            _apiClient = apiClient;
            _batchIndex = 0;
            _flashcardIndex = 0;
            _flashcards = flashcards;
            _flashcard = DrawFlashcard();
            _isCardFlapped = false;
            Question = _flashcard.Key;
            Description = _flashcard.KeyDescription;
            _wrongCount = 0;
            _partialCount = 0;
            _correctCount = 0;
        }

        #endregion

        #region Properties/Commands

        public Flashcard Flashcard
        {
            get { return _flashcard; }
            set
            {
                _flashcard = value;
                OnPropertyChanged();
            }
        }

        public Visibility ShowBack => Convert(_isCardFlapped);

        public Visibility ShowFront => Convert(!_isCardFlapped);

        public bool IsCardFlapped
        {
            get { return _isCardFlapped; }
            set
            {
                _isCardFlapped = value;
                OnPropertyChanged("ShowFront");
                OnPropertyChanged("ShowBack");
            }
        }

        public ICommand FlipCardCommand
        {
            get
            {
                return _flipCardCommand ?? (_flipCardCommand = new RelayCommand(param => FlipCard(), param => _flashcards?.Count > 0 &&_flashcardIndex <= _flashcards?.Count));
            }
        }

        public ICommand CorrectAnswerCommand
        {
            get
            {
                return _correctAnswerCommand ??
                       (_correctAnswerCommand = new RelayCommand(async param => await CorrectAnswerAsync(), param => true));
            }
        }
        public ICommand IncompleteAnswerCommand
        {
            get
            {
                return _incompleteAnswerCommand ??
                       (_incompleteAnswerCommand = new RelayCommand(async param => await IncompleteAnswerAsync(), param => true));
            }
        }
        public ICommand WrongAnswerCommand
        {
            get
            {
                return _wrongAnswerCommand ??
                       (_wrongAnswerCommand = new RelayCommand(async param => await WrongAnswerAsync(), param => true));
            }
        }

        public string Question
        {
            get { return _question; }
            set { _question = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(); }
        }

        private Visibility Convert(bool isCardFlapped)
        {
            return isCardFlapped ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion

        #region Methods

        private void FlipCard()
        {
            IsCardFlapped = !_isCardFlapped;
            Question = _isCardFlapped ? _flashcard.Value : _flashcard.Key;
            Description = _isCardFlapped ? _flashcard.ValueDescription : _flashcard.KeyDescription;
        }

        private async Task CorrectAnswerAsync()
        {
            _correctCount++;
            var requestResult = await PostResult(FlashcardResult.Success);
            if (!requestResult)
                return;
            _flashcard = DrawFlashcard();
            FlipCard();
        }

        private async Task<bool> PostResult(FlashcardResult result)
        {
            return await _apiClient.PostJsonAsync(ApiUrls.LearningResultEndpoint, new FlashcardResultViewModel() { FlashcardId = _flashcard.Id, Result = result });
        }

        private async Task IncompleteAnswerAsync()
        {
            _partialCount++;
            var requestResult = await PostResult(FlashcardResult.Partial);
            if (!requestResult)
                return;
            _flashcard = DrawFlashcard();
            FlipCard();
        }
        private async Task WrongAnswerAsync()
        {
            _wrongCount++;
            var requestResult = await PostResult(FlashcardResult.Fail);
            if (!requestResult)
                return;
            _flashcard = DrawFlashcard();
            FlipCard();
        }

        private Flashcard DrawFlashcard()
        {
            if (_flashcards == null || _flashcards.Count == 0)
                return new Flashcard()
                {
                    Key = "No flashcards available in selected category",
                    KeyDescription = "Please add flashcards"
                };
            if (_flashcardIndex >= _flashcards.Count)
            {
                _flashcardIndex++;
                return new Flashcard()
                {
                    Key = "Learning finished",
                    KeyDescription = $"Correct:{_correctCount} Not entirely: {_partialCount} Wrong: {_wrongCount}"
                };
            }
            return _flashcards[_flashcardIndex++];
        }

        #endregion
    }
}
