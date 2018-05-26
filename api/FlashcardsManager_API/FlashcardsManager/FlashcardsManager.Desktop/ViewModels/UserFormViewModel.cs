using System.Windows.Input;
using FlashcardsManager.Core.Dtos;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Core.ViewModels;
using FlashcardsManager.Desktop.Helpers;

namespace FlashcardsManager.Desktop.ViewModels
{
    public class UserFormViewModel: ObservableObject
    {
        private UserDto _formUser;
        private ICommand _submitFormCommand;
        private string _submitLabel;

        public UserFormViewModel(ICommand submitFormCommand, string submitLabel, UserDto formUser = null)
        {
            if (formUser != null)
                FormUser = formUser;
            SubmitFormCommand = submitFormCommand;
            SubmitLabel = submitLabel;
        }


        public UserDto FormUser
        {
            get
            {
                if (_formUser == null)
                {
                    _formUser = new UserDto();
                }
                return _formUser;
            }
            set
            {
                if (value != _formUser)
                {
                    _formUser = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SubmitFormCommand
        {
            get { return _submitFormCommand; }
            set
            {
                if (value != _submitFormCommand)
                {
                    _submitFormCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SubmitLabel
        {
            get { return _submitLabel; }
            set
            {
                if (value != _submitLabel)
                {
                    _submitLabel = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
