using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MessengerClient.Logic.ViewModel.LoginVM
{
    public class LoginVM : BaseVM
    {
        private Window signWindow;

        public LoginVM(Window signWindow)
        {
            this.signWindow = signWindow;
        }
    }
}
