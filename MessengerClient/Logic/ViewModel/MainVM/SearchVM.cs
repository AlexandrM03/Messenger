using MessengerClient.Logic.Model;
using MessengerClient.ServiceMessenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace MessengerClient.Logic.ViewModel.MainVM
{
    public class SearchVM : BaseVM
    {
        private List<UserModel> users;
        private List<UserModel> selectedUsers;
        private Brush searchBackground;

        public List<UserModel> Users
        {
            get => users; 
            set
            {
                users = value;
                OnPropertyChanged();
            }
        }

        public Brush SearchBackground
        {
            get => searchBackground;
            set
            {
                searchBackground = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectItemCommand { get; set; }

        public SearchVM()
        {
            selectedUsers = new List<UserModel>();
            users = new List<UserModel>();
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

            SelectItemCommand = new DelegateCommand(SelectItem);
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
    }
}
