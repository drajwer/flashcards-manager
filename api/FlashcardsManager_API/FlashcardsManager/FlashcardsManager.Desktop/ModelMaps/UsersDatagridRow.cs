using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FlashcardsManager.Core.Dtos;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Core.ViewModels;
using FlashcardsManager.Desktop.ViewModels;

namespace FlashcardsManager.Desktop.ModelMaps
{
    public class UsersDatagridRow 
    {
        public UserDto User { get; set; }
        public int Score { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public UsersViewModel ViewModel { get; set; }

        public UsersDatagridRow(UserDto user, int score)
        {
            User = user;
            Score = score;
        }
    }
}
