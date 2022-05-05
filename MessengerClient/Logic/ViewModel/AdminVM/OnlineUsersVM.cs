using MessengerClient.Logic.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerClient.Logic.ViewModel.AdminVM
{
    public class OnlineUsersVM : BaseVM
    {
        private ObservableCollection<AdminInfoModel> userInfo;

        public ObservableCollection<AdminInfoModel> UserInfo
        {
            get => userInfo;
            set
            {
                userInfo = value;
                OnPropertyChanged();
            }
        }

        public OnlineUsersVM()
        {
            UserInfo = new ObservableCollection<AdminInfoModel>();

            CurrentClient.SetOnlineVM(this);
        }
    }
}
