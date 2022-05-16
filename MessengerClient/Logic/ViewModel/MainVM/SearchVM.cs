using MessengerClient.Logic.Model;
using MessengerClient.ServiceMessenger;
using Microsoft.Win32;
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
        private ChatModel chatModel;

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

        public ChatModel ChatModel
        {
            get => chatModel;
            set
            {
                chatModel = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectItemCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand CreateChatCommand { get; set; }
        public ICommand ChooseImageCommand { get; set; }

        public SearchVM()
        {
            selectedUsers = new ObservableCollection<UserModel>();
            users = new ObservableCollection<UserModel>();
            //MessengerServiceClient client = new MessengerServiceClient(null);
            foreach (Dictionary<string, string> user in CurrentClient.Client.GetUsers())
            {
                if (CurrentUser.User.Id != Int32.Parse(user["id"]))
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
            chatModel = new ChatModel();

            SelectItemCommand = new DelegateCommand(SelectItem);
            SearchCommand = new DelegateCommand(Search);
            CreateChatCommand = new DelegateCommand(CreateChat);
            ChooseImageCommand = new DelegateCommand(ChooseImage);
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

        private async void CreateChat(object obj)
        {
            int[] users_id = selectedUsers.Select(u => u.Id).ToArray();

            //MessengerServiceClient client = new MessengerServiceClient(null);
            await Task.Factory.StartNew(() => CurrentClient.Client.CreateChat(ChatModel.Name, ChatModel.Image, CurrentUser.User.Id, users_id));
        }

        private void ChooseImage(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            if (openFileDialog.ShowDialog() == true)
            {
                chatModel.Image = openFileDialog.FileName;
            }
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
