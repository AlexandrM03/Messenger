using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MessengerClient.Logic.ViewModel.MainVM
{
    public class AccountVM : BaseVM
    {
        private string avatar;
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Avatar 
        {
            get => avatar;
            set
            {
                avatar = value;
                OnPropertyChanged();
            }
        }

        public ICommand ChangeAvatarCommand { get; set; }

        public AccountVM()
        {
            Name = CurrentUser.User.Name;
            Surname = CurrentUser.User.Surname;
            Avatar = CurrentUser.User.Avatar;

            ChangeAvatarCommand = new DelegateCommand(ChangeAvatar);
        }

        private void ChangeAvatar(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            if (openFileDialog.ShowDialog() == true)
            {
                Avatar = openFileDialog.FileName;
                CurrentUser.User.Avatar = Avatar;

                CurrentClient.Client.ChangeAvatar(CurrentUser.User.Id, Avatar);
            }
        }
    }
}
