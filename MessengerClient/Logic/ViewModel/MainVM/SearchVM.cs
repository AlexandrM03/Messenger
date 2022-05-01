using MessengerClient.Logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerClient.Logic.ViewModel.MainVM
{
    public class SearchVM : BaseVM
    {
        private List<UserModel> users;

        public List<UserModel> Users
        {
            get => users; 
            set
            {
                users = value;
                OnPropertyChanged();
            }
        }

        public SearchVM()
        {
            Users = new List<UserModel>();
        }
    }
}
