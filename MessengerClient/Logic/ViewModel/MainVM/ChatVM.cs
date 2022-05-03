using MessengerClient.Logic.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerClient.Logic.ViewModel.MainVM
{
    public class ChatVM : BaseVM
    {
        private ObservableCollection<MessageModel> messages;

        public ObservableCollection<MessageModel> Messages
        {
            get => messages;
            set
            {
                messages = value;
                OnPropertyChanged();
            }
        }

        
    }
}
