using FlashcardsManager.Core.ApiClient;
using FlashcardsManager.Desktop.Helpers;
using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FlashcardsManager.Desktop.ViewModels
{
    public class LoginViewModel : ObservableObject, IAuthViewModel
    {
        private string _username;
        private string _password;
        private ICommand _loginCommand;
        private Action _closeAction;
        private readonly ApiClient _apiClient;
        public LoginViewModel(ApiClient apiClient, Action closeAction)
        {
            _apiClient = apiClient;
            _closeAction = closeAction;
        }
        public ICommand ChangePageCommand { get; set; }
        public ICommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                    _loginCommand = new RelayCommand(
                        async x => await LoginAsync(),
                        x => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password)
                        );
                return _loginCommand;
            }
        }


        public string Username
        {
            get
            {
                if (_username == null)
                    _username = "";
                return _username;
            }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get
            {
                if (_password == null)
                    _password = "";
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public async Task LoginAsync()
        {
            var tokenClient = new TokenClient(ApiUrls.TokenEndpoint, ApiUrls.ClientName, ApiUrls.ClientSecret);
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(Username, Password, ApiUrls.Scope);

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                MessageBox.Show("Invalid username or password");
                return;
            }

            _apiClient.BearerToken = tokenResponse.AccessToken;
            _closeAction();
        }
    }
}
