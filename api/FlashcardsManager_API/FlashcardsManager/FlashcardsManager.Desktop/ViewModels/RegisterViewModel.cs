using FlashcardsManager.Core.ApiClient;
using FlashcardsManager.Core.ViewModels;
using FlashcardsManager.Desktop.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FlashcardsManager.Desktop.ViewModels
{
    public class RegisterViewModel : ObservableObject, IAuthViewModel
    {
        private RegisterFormViewModel _form;
        private ICommand _registerCommand;
        private ApiClient _apiClient;
        public RegisterViewModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        public ICommand ChangePageCommand { get; set; }
        public ICommand RegisterCommand
        {
            get
            {
                if (_registerCommand == null)
                    _registerCommand = new RelayCommand(
                        async x => await RegisterAsync(),
                        x => !string.IsNullOrWhiteSpace(Form.Username) && !string.IsNullOrWhiteSpace(Form.Password) && Form.Password == Form.ConfirmPassword
                        );
                return _registerCommand;
            }
        }


        public RegisterFormViewModel Form
        {
            get
            {
                if (_form == null)
                    _form = new RegisterFormViewModel();
                return _form;
            }
            set
            {
                _form = value;
                OnPropertyChanged();
            }
        }

        public async Task RegisterAsync()
        {
            var form = _form;
            var result = await _apiClient.PostJsonAsync<RegisterFormViewModel>(ApiUrls.RegisterAction, form);
            if (!result)
            {
                MessageBox.Show("Cannot register an user.");
                return;
            }
            MessageBox.Show("Registration succeeded. Please sign in.");
            _form = new RegisterFormViewModel();
            ChangePageCommand.Execute(this);
        }
    }
}
