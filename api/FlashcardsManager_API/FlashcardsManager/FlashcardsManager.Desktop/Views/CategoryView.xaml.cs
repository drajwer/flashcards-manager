using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using FlashcardsManager.Desktop.ModelMaps;
using FlashcardsManager.Desktop.ViewModels;

namespace FlashcardsManager.Desktop.Views
{
    /// <summary>
    /// Interaction logic for CategoryView.xaml
    /// </summary>
    public partial class CategoryView : UserControl
    {
        public CategoryView()
        {
            InitializeComponent();
            //this.DataContext = new CategoryViewModel();
        }
        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Delete)
                return;
            var result = MessageBox.Show("Are you sure you want to delete these items?", "Flashcards Manager", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (result != MessageBoxResult.Yes)
                return;
            var vm = DataContext as CategoryViewModel;
            var grid = sender as DataGrid;
            var rows = grid?.SelectedItems;
            if (rows == null)
                return;
            var categories = new List<Category>();
            foreach (var row in rows)
            {
                var userRow = row as CategoryDatagridRow;
                if (userRow?.Category == null)
                    continue;
                categories.Add(userRow.Category);
            }
            var command = vm?.DeleteCategoryCommand;

            if (command != null && command.CanExecute(categories))
                command.Execute(categories);
        }

        private void ColumnHeaderHandleClick(object sender, RoutedEventArgs e)
        {
            if (sender is DataGridColumnHeader columnHeader)
            {
                var vm = this.DataContext as CategoryViewModel;
                vm.SortCommand.Execute(columnHeader.Column.Header as string);
            }

        }
    }
}
