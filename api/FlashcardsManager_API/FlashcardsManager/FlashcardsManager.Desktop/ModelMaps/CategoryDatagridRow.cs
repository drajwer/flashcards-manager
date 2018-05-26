using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Desktop.ViewModels;

namespace FlashcardsManager.Desktop.ModelMaps
{
    public class CategoryDatagridRow
    {
        public Category Category { get; set; }
        public string Name { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public CategoryViewModel ViewModel { get; set; }

        public CategoryDatagridRow(Category category, string name)
        {
            Category = category;
            Name = name;
        }
    }
}
