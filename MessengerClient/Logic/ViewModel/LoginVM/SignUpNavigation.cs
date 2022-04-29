using MessengerClient.Presentation.View.Login.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MessengerClient.Logic.ViewModel.LoginVM
{
    public class SignUpNavigation
    {
        readonly UserControl login;
        readonly UserControl registation;
        Window window;
        Dictionary<string, UserControl> pages = new Dictionary<string, UserControl>();

        public SignUpNavigation(Window window)
        {
            this.window = window;
            login = new LoginUC(window);
            registation = new RegistrationUC();
            pages.Add("login", login);
            pages.Add("registration", registation);
        }

        public UserControl GetPage(string name)
        {
            return pages[name];
        }

        public void CloseWindow()
        {
            window.Close();
        }
    }
}
