using MessengerClient.Logic.Model;
using MessengerClient.Presentation.View.Admin;
using MessengerClient.ServiceMessenger;
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

            if (String.IsNullOrEmpty(LoginModel.Password))
            {
                MessageBox.Show("Enter the password");
                return;
            }

            Dictionary<string, string> userData = CurrentClient.Client.Login(LoginModel.Login, LoginModel.Password);
            
            if (userData == null)
            {
                MessageBox.Show("Wrong login or password");
                return;
            }

            UserModel user = new UserModel()
            {
                Id = Int32.Parse(userData["id"]),
                Name = userData["name"],
                Surname = userData["surname"],
                Role = userData["role"],
                Avatar = userData["path"]
            };
            CurrentUser.SetNewUser(user);

            if (CurrentUser.User.Role.Equals("user"))
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                signWindow.Close();
            }
            else
            {
                Admin admin = new Admin();
                admin.Show();
                signWindow.Close();
            }
        }
    }
}
