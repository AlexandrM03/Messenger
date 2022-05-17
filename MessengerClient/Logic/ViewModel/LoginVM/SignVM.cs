using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MessengerClient.Logic.ViewModel.LoginVM
{
    public class SignVM : BaseVM
    {
        ContentControl control;
        private double contentOpacity;

        public double ContentOpacity
        {
            get => contentOpacity; 
            set
            {
                contentOpacity = value;
                OnPropertyChanged();
            }
        }

        public ContentControl Content
        {
            get => control;
            set
            {
                control = value;
                OnPropertyChanged();
            }
        }

        public ICommand GoToLoginCommand { get; private set; }
        public ICommand GoToRegistrationCommand { get; private set; }
        private SignUpNavigation navigation;

        public SignVM(Window window)
        {
            navigation = new SignUpNavigation(window);

            Content = navigation.GetPage("login");
            ContentOpacity = 1.0;

            GoToLoginCommand = new DelegateCommand(GoToLogin);
            GoToRegistrationCommand = new DelegateCommand(GoToRegistration);
        }

        private void GoToLogin(object obj)
        {
            SlowOpacity(navigation.GetPage("login"));
        }

        private void GoToRegistration(object obj)
        {
            SlowOpacity(navigation.GetPage("registration"));
        }

        private async void SlowOpacity(UserControl control)
        {
            await Task.Factory.StartNew(() =>
            {
                for (double i = 1.0; i > 0.0; i -= 0.05)
                {
                    ContentOpacity = i;
                    Thread.Sleep(25);
                }

                Content = control;

                for (double i = 0.0; i < 1.1; i += 0.05)
                {
                    ContentOpacity = i;
                    Thread.Sleep(25);
                }
            });
        }
    }
}
