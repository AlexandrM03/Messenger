using MessengerClient.Logic.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MessengerClient.Logic.ViewModel.MainVM
{
    public class ChatInfoVM : BaseVM
    {
        private ObservableCollection<UserModel> users;
        private ObservableCollection<UserModel> otherUsers;

        public ObservableCollection<UserModel> Users
        {
            get => users;
            set
            {
                users = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<UserModel> OtherUsers
        {
            get => otherUsers;
            set
            {
                otherUsers = value;
                OnPropertyChanged();
            }
        }

        public ICommand RemoveUserCommand { get; set; }
        public ICommand AddUserCommand { get; set; }

        public ChatInfoVM()
        {
            Users = new ObservableCollection<UserModel>();

            RemoveUserCommand = new DelegateCommand(RemoveUser, CanExecute);
            AddUserCommand = new DelegateCommand(AddUser, CanExecute);

            CurrentClient.SetChatInfoVM(this);
        }

        private void RemoveUser(object obj)
        {
            if (!(obj is UserModel user))
                return;

            CurrentClient.Client.DeleteUserFromChat(CurrentChat.Chat.Id, user.Id);
            Users.Remove(user);
            OtherUsers.Add(user);
        }

        private void AddUser(object obj)
        {
            if (!(obj is UserModel user))
                return;

            CurrentClient.Client.AddUserToChat(CurrentChat.Chat.Id, user.Id);
            Users.Add(user);
            OtherUsers.Remove(user);
        }

        private bool CanExecute(object obj)
        {
            if (!(obj is UserModel user))
                return false;

            return CurrentUser.User.Id == CurrentChat.Chat.Admin || CurrentUser.User.Id == user.Id;
        }
    }
}
