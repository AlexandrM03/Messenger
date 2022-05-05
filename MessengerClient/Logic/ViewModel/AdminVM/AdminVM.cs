using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MessengerClient.Logic.ViewModel.AdminVM
{
    public class AdminVM : BaseVM
    {
        private Page mainContent;
        private AdminNavigation navigation;

        public Page MainContent
        {
            get => mainContent;
            set
            {
                mainContent = value;
                OnPropertyChanged();
            }
        }

        public ICommand GoToOnlineUsersCommand { get; set; }
        
        public AdminVM()
        {
            navigation = new AdminNavigation();
            MainContent = navigation.GetPage("online");

            GoToOnlineUsersCommand = new DelegateCommand(GoToOnlineUsers);
        }

        private void GoToOnlineUsers(object obj)
        {
            MainContent = navigation.GetPage("online");
        }
    }
}
