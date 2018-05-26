using System.Windows;
using FlashcardsManager.Desktop.ViewModels;

namespace FlashcardsManager.Desktop
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        public DialogWindow(LearningDialogViewModel learningDialogViewModel)
        {
            InitializeComponent();
            DataContext = learningDialogViewModel;
        }
    }
}
