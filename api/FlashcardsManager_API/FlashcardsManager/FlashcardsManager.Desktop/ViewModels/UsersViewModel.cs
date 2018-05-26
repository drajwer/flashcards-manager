using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using FlashcardsManager.Core.ApiClient;
using FlashcardsManager.Core.Dtos;
using FlashcardsManager.Core.Enums;
using FlashcardsManager.Core.Helpers;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Core.UnitOfWork;
using FlashcardsManager.Core.ViewModels;
using FlashcardsManager.Desktop.Helpers;
using FlashcardsManager.Desktop.ModelMaps;
using FlashcardsManager.Desktop.Views;
using MaterialDesignThemes.Wpf;

namespace FlashcardsManager.Desktop.ViewModels
{
    public class UsersViewModel : ObservableObject, IPageViewModel
    {
        #region Fields/Constructors

        private readonly ApiClient _apiClient;
        private ICommand _submitFormCommand;
        private ICommand _deleteUsersCommand;
        private ICommand _deleteUserCommand;
        private ICommand _openEditDialogCommand;
        private ICommand _updateUserCommand;
        private ICommand _openAddDialogCommand;
        private ICommand _filterCommand;
        private int _pageSize;
        private int _pageIndex;

        private List<UsersDatagridRow> _users;

        public UsersViewModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
            SearchText = "";
            FilterCommand = new RelayCommand(async param =>
            {
                PageIndex = 0;
                await Update();
            });
            SortCommand = new RelayCommand(async param => await ColumnHeader_HandleClick(param as string));
            PrevPageCommand = new RelayCommand(async param => await PrevPage(), param => PageIndex > 0);
            NextPageCommand = new RelayCommand(async param => await NextPage(), param => _users?.Count > 0);
            _pageIndex = 0;
            _pageSize = 10;
        }

        #endregion

        #region Properties/Commands

        public string Name => "Users";

        public string SearchText { get; set; }

        public UsersSortingCriterion SortingCriterion { get; set; }

        public bool Descending { get; set; }

        public int PageIndex { get => _pageIndex; set { _pageIndex = value; OnPropertyChanged(); } }

        public int PageSize { get => _pageSize; set { _pageSize = value; OnPropertyChanged(); Update(); } }

        public ICommand PrevPageCommand { get; }

        public ICommand NextPageCommand { get; }


        public List<UsersDatagridRow> Users
        {
            get
            {
                if (_users == null)
                {

                    Task.Run(async () => { _users = await GetUsers(); });
                }
                return _users;
            }
            set
            {
                if (_users == value)
                    return;
                _users = value;
                OnPropertyChanged();
            }
        }

        public ICommand FilterCommand { get; }

        public ICommand SortCommand { get; }

        public ICommand DeleteUserCommand
        {
            get
            {
                return _deleteUserCommand ?? (_deleteUserCommand = new RelayCommand(
                           async param => await DeleteUser(param as UsersDatagridRow),
                           param => ((param as UsersDatagridRow) != null)
                       ));
            }
        }

        private async Task DeleteUser(UsersDatagridRow usersDatagridRow)
        {
            var result = MessageBox.Show("Are you sure you want to delete this item?", "Flashcards Manager", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
                return;
            var user = usersDatagridRow.User;
            await DeleteUsers(new List<UserDto>() { user });
        }

        public ICommand DeleteUsersCommand
        {
            get
            {
                return _deleteUsersCommand ?? (_deleteUsersCommand = new RelayCommand(
                           async param => await DeleteUsers(param as List<UserDto>),
                           param => ((param as List<UserDto>) != null)
                       ));
            }
        }

        public ICommand SubmitFormCommand
        {
            get
            {
                return _submitFormCommand ?? (_submitFormCommand = new RelayCommand(
                           async param => await SubmitForm(param as UserDto),
                           param => !string.IsNullOrEmpty((param as UserDto)?.UserName) &&
                                    !Users.Any(row => row.User.UserName == (param as UserDto).UserName)
                       ));
            }
        }
        public ICommand OpenAddDialogCommand
        {
            get
            {
                return _openAddDialogCommand ??
                       (_openAddDialogCommand = new RelayCommand(async param => await OpenAddDialog()));
            }
            set
            {
                if (value != _openAddDialogCommand)
                {
                    _openAddDialogCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand OpenEditDialogCommand
        {
            get
            {
                return _openEditDialogCommand ?? (_openEditDialogCommand = new RelayCommand(
                           async param => await OpenEditDialog(param as UsersDatagridRow),
                           param => param is UsersDatagridRow
                       ));
            }
        }

        public ICommand UpdateUserCommand
        {
            get
            {
                return _updateUserCommand ?? (_updateUserCommand = new RelayCommand(
                           async param => await UpdateUser(param as UserDto),
                           param => !string.IsNullOrEmpty((param as UserDto)?.UserName) &&
                                    !Users.Any(row => row.User.UserName == (param as UserDto).UserName)
                       ));
            }
        }

        #endregion

        #region Methods

        private async Task OpenAddDialog()
        {
            var vm = new UserFormViewModel(SubmitFormCommand, "ADD");
            await DialogHost.Show(new UserForm() { DataContext = vm });
        }

        private async Task OpenEditDialog(UsersDatagridRow usersDatagridRow)
        {
            var user = usersDatagridRow.User;
            var formUser = new UserDto()
            {
                //Id = user.Id, //TODO: We should disable editing
                UserName = user.UserName,
                Name = user.Name,
                Surname = user.Surname
            };
            var vm = new UserFormViewModel(UpdateUserCommand, "UPDATE", formUser);
            await DialogHost.Show(new UserForm() { DataContext = vm });
        }

        private async Task UpdateUser(UserDto formUser)
        {
            if (string.IsNullOrWhiteSpace(formUser?.UserName))  //TODO: Same as above
                return;

            //await _apiClient.PutJsonAsync<UserDto, object>(ApiUrls.UserEndpoint, formUser.Id, formUser);
            await Update();
        }

        public async Task Update()
        {
            Users = await GetUsers();
        }

        private async Task DeleteUsers(List<UserDto> usersToDelete)
        {
            if (usersToDelete == null)
                return;
            foreach (var user in usersToDelete)
            {
                //await _apiClient.DeleteJsonAsync<UserDto>(ApiUrls.UserEndpoint, user.Id);
            }

            Users = await GetUsers();
        }

        private async Task SubmitForm(UserDto formUser)
        {

            //formUser.Id = Guid.NewGuid().ToString();
            await _apiClient.PostJsonAsync<UserDto, object>(ApiUrls.UserEndpoint, formUser);
            await Update();
        }

        private async Task<List<UsersDatagridRow>> GetUsers()
        {
            var filteringModel = new UsersFilteringModel(SearchText, SortingCriterion, Descending, _pageIndex, PageSize);
            var users = await _apiClient.PostJsonAsync<UsersFilteringModel, List<UserDto>>(ApiUrls.UserEndpoint + "/filter",
                filteringModel);
            var rows = users.Select(u => new UsersDatagridRow(u, u.Score.SumPoints)).ToList();//u.UserProgress.Sum(p => p.Progress))
            foreach (var row in rows)
            {
                row.EditCommand = OpenEditDialogCommand;
                row.DeleteCommand = DeleteUserCommand;
                row.ViewModel = this;
            }
            return rows;
        }

        private async Task PrevPage()
        {
            if (PageIndex > 0) PageIndex--;
            await Update();
        }

        private async Task NextPage()
        {
            PageIndex++;
            await Update();
        }

        private async Task ColumnHeader_HandleClick(string header)
        {
            foreach (var enumValue in Enum.GetValues(typeof(UsersSortingCriterion)))
            {
                var criterion = enumValue is UsersSortingCriterion sortingCriterion ? sortingCriterion : UsersSortingCriterion.None;
                if (!string.Equals(criterion.ToString(), header, StringComparison.CurrentCultureIgnoreCase)) continue;
                if (SortingCriterion != criterion) Descending = false;
                else Descending = !Descending;
                SortingCriterion = criterion;
                await Update();
                return;
            }
        }
        #endregion
    }

}
