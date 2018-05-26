using System.Windows.Input;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Desktop.Helpers;

namespace FlashcardsManager.Desktop.ViewModels
{
    public class CategoryFormViewModel : ObservableObject
    {
        private Category _formCategory;
        private ICommand _submitFormCommand;
        private string _submitLabel;

        public CategoryFormViewModel(ICommand submitFormCommand, string submitLabel, Category formCategory = null)
        {
            if (formCategory != null)
                FormCategory = formCategory;
            SubmitFormCommand = submitFormCommand;
            SubmitLabel = submitLabel;
        }

        public Category FormCategory
        {
            get { return _formCategory ?? (_formCategory = new Category()); }
            set
            {
                if (value == _formCategory) return;
                _formCategory = value;
                OnPropertyChanged();
            }
        }

        public ICommand SubmitFormCommand
        {
            get { return _submitFormCommand; }
            set
            {
                if (value == _submitFormCommand) return;
                _submitFormCommand = value;
                OnPropertyChanged();
            }
        }

        public string SubmitLabel
        {
            get { return _submitLabel; }
            set
            {
                if (value == _submitLabel) return;
                _submitLabel = value;
                OnPropertyChanged();
            }
        }
    }
}
