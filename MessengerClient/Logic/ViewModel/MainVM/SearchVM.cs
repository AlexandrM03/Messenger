﻿using MessengerClient.Logic.Model;
using MessengerClient.ServiceMessenger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace MessengerClient.Logic.ViewModel.MainVM
{
    public class SearchVM : BaseVM
    {
        private ObservableCollection<UserModel> users;
        private ObservableCollection<UserModel> searchUsers;
        private ObservableCollection<UserModel> selectedUsers;
        private string searchText;

        public ObservableCollection<UserModel> Users
        {
            get => users; 
            set
            {
                users = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<UserModel> SelectedUsers
        {
            get => selectedUsers;
            set
            {
                selectedUsers = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectItemCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public SearchVM()
        {
            selectedUsers = new ObservableCollection<UserModel>();
            users = new ObservableCollection<UserModel>();
            MessengerServiceClient client = new MessengerServiceClient();
            foreach (Dictionary<string, string> user in client.GetUsers())
            {
                users.Add(new UserModel()
                {
                    Id = Int32.Parse(user["id"]),
                    Name = user["name"],
                    Surname = user["surname"],
                    Role = user["role"],
                    Avatar = user["path"]
                });
            }
            searchUsers = users;

            SelectItemCommand = new DelegateCommand(SelectItem);
            SearchCommand = new DelegateCommand(Search);
        }

        private void SelectItem(object obj)
        {
            if (!(obj is UserModel user))
                return;

            if (selectedUsers.Contains(user))
                selectedUsers.Remove(user);
            else
                selectedUsers.Add(user);
        }

        private void Search(object obj)
        {
            ObservableCollection<UserModel> temp = new ObservableCollection<UserModel>();
            foreach (UserModel user in searchUsers)
            {
                if (user.Name.ToLower().Contains(SearchText.ToLower()) || user.Surname.ToLower().Contains(SearchText.ToLower()))
                    temp.Add(user);
            }

            Users = temp;
        }
    }
}
