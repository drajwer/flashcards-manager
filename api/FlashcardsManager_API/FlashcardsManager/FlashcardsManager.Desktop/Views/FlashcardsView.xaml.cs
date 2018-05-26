using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Desktop.ViewModels;

namespace FlashcardsManager.Desktop
{
    /// <summary>
    /// Interaction logic for ProductsView.xaml
    /// </summary>
    public partial class FlashcardsView : UserControl
    {
        public FlashcardsView()
        {
            InitializeComponent();
        }

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key != Key.Delete)
                return;
            var result = MessageBox.Show("Are you sure you want to delete this items?", "Flashcards Manager", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if(result != MessageBoxResult.Yes)
                return;
            var vm = DataContext as FlashcardsViewModel;
            var grid = sender as DataGrid;
            var flashcards = grid?.SelectedCells.Select(cell => cell.Item as Flashcard).ToList();
            var command = vm?.DeleteFlashcardCommand;

            if(command != null && command.CanExecute(flashcards))
                command.Execute(flashcards);
        }

        private void ColumnHeaderHandleClick(object sender, RoutedEventArgs e)
        {
            if (sender is DataGridColumnHeader columnHeader)
            {
                var vm = this.DataContext as FlashcardsViewModel;
                vm.SortCommand.Execute(columnHeader.Column.Header as string);
            }

        }
    }
}
