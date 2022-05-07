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

        public ObservableCollection<UserModel> Users
        {
            get => users;
            set
            {
                users = value;
                OnPropertyChanged();
            }
        }

        public ICommand RemoveUserCommand { get; set; }

        public ChatInfoVM()
        {
            Users = new ObservableCollection<UserModel>();

            RemoveUserCommand = new DelegateCommand(RemoveUser, CanRemove);
            
            CurrentClient.SetChatInfoVM(this);
        }

        private void RemoveUser(object obj)
        {
            if (!(obj is UserModel user))
                return;

            CurrentClient.Client.DeleteUserFromChat(CurrentChat.Chat.Id, user.Id);
            Users.Remove(user);
        }

        private bool CanRemove(object obj)
        {
            return CurrentUser.User.Id == CurrentChat.Chat.Admin;
        }
    }
}
