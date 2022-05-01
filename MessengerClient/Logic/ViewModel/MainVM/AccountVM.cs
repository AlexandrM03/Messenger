using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerClient.Logic.ViewModel.MainVM
{
    public class AccountVM : BaseVM
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Avatar { get; set; }

        public AccountVM()
        {
            Name = CurrentUser.User.Name;
            Surname = CurrentUser.User.Surname;
            Avatar = CurrentUser.User.Avatar;
        }
    }
}
