using MessengerClient.Logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MessengerClient.Logic.ViewModel.LoginVM
{
    public class LoginVM : BaseVM
    {
        private Window signWindow;
        private LoginModel loginModel;

        public LoginModel LoginModel
        {
            get => loginModel; 
            set
            {
                loginModel = value;
                OnPropertyChanged();
            }
        }

        public ICommand SignInCommand { get; private set; } 

        public LoginVM(Window window)
        {
            signWindow = window;
            LoginModel = new LoginModel();
            SignInCommand = new DelegateCommand(SignIn);
        }

        private void SignIn(object obj)
        {
            if (!(obj is PasswordBox passwordBox))
                return;
            LoginModel.Password = passwordBox.Password;

            if (LoginModel.Login == null || LoginModel.Password == null)
            {
                MessageBox.Show("Введите логин и пароль");
                return;
            }

            Dictionary<string, string> userData;
            
            // TODO
            if (CurrentUser.User.Role.Equals("user"))
            {
                signWindow.Close();
                return;
            }
        }
    }
}
