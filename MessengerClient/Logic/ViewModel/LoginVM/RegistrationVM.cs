using MessengerClient.Logic.Model;
using MessengerClient.ServiceMessenger;
using Microsoft.Win32;
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
        public ICommand OpenFileDialogCommand { get; private set; }

        public RegistrationVM()
        {
            RegistrationModel = new RegistrationModel()
            {
                Path = @"D:\4 семестр\КП ООП\Messenger\Resources\default.png"
            };
            RegistrationCommand = new DelegateCommand(Registration, CanRegistr);
            OpenFileDialogCommand = new DelegateCommand(ChooseImage);
        }

        private void ChooseImage(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            if (openFileDialog.ShowDialog() == true)
            {
                RegistrationModel.Path = openFileDialog.FileName;
                OnPropertyChanged("RegistrationModel");
            }
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

            string result = CurrentClient.Client.Registration(registrationModel.Login,
                registrationModel.Password,
                registrationModel.Name,
                registrationModel.Surname,
                registrationModel.Path);
            if (result != "Nice")
            {
                MessageBox.Show(result);
            }
        }
    }
}
