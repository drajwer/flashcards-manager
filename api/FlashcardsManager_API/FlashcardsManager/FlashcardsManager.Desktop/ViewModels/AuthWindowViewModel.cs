using FlashcardsManager.Desktop.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlashcardsManager.Desktop.ViewModels
{
    public class AuthWindowViewModel : ObservableObject
    {
        private readonly IAuthViewModel _loginViewModel;
        private readonly IAuthViewModel _registerViewModel;
        private IAuthViewModel _currentPageViewModel;

        private ICommand _changePageCommand;

        public AuthWindowViewModel(IAuthViewModel loginVm, IAuthViewModel registerVm)
        {
            _loginViewModel = loginVm;
            _registerViewModel = registerVm;

            _loginViewModel.ChangePageCommand = ChangePageCommand;
            _registerViewModel.ChangePageCommand = ChangePageCommand;

            CurrentPageViewModel = loginVm;
        }

        #region Properties / Commands

        public ICommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {
                    _changePageCommand = new RelayCommand(
                        p => ChangeViewModel());
                }

                return _changePageCommand;
            }
        }

        public IAuthViewModel CurrentPageViewModel
        {
            get { return _currentPageViewModel; }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    OnPropertyChanged("CurrentPageViewModel");
                }
            }
        }

        #endregion

        #region Methods

        private void ChangeViewModel()
        {
            CurrentPageViewModel = CurrentPageViewModel == _loginViewModel ? _registerViewModel : _loginViewModel;
        }

        #endregion
    }
}
