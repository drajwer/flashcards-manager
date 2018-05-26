using FlashcardsManager.Core.ApiClient;
using FlashcardsManager.Desktop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FlashcardsManager.Desktop
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public bool IsAuthorized { get; set; }
        public AuthWindow(ApiClient apiClient)
        {
            InitializeComponent();
            DataContext = new AuthWindowViewModel(new LoginViewModel(apiClient, Close), new RegisterViewModel(apiClient));
            IsAuthorized = false;
        }
        public void HandleSignInClose()
        {
            IsAuthorized = true;
            Close();
        }
    }
}
