using MessengerClient.Logic.Model;
using MessengerClient.ServiceMessenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MessengerClient.Logic.ViewModel.LoginVM
{
    public class RegistrationVM : BaseVM
    {
        private RegistrationModel registrationModel;
        private bool isCheckBoxChecked;

        public bool IsCheckBoxChecked
        {
            get => isCheckBoxChecked;
            set
            {
                isCheckBoxChecked = value;
                OnPropertyChanged();
            }
        }

        public RegistrationModel RegistrationModel
        {
            get => registrationModel;
            set
            {
                registrationModel = value;
                OnPropertyChanged();
            }
        }

        public ICommand RegistrationCommand { get; private set; }

        public RegistrationVM()
        {
            RegistrationModel = new RegistrationModel();
            RegistrationCommand = new DelegateCommand(Registration, CanRegistr);
        }

        private bool CanRegistr(object obj)
        {
            return IsCheckBoxChecked;
        }

        private void Registration(object obj)
        {
            if (!(obj is PasswordBox passwordBox))
                return;

            RegistrationModel.Password = passwordBox.Password;
            RegistrationModel.RepeatPassword = passwordBox.Password;

            MessengerServiceClient client = new MessengerServiceClient();
            client.Registration(registrationModel.Login,
                registrationModel.Password,
                registrationModel.Name,
                registrationModel.Surname,
                "");
        }
    }
}
