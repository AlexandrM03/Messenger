using MessengerClient.Logic.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MessengerClient.Logic.ViewModel.MainVM
{
    public class ChatVM : BaseVM
    {
        private ObservableCollection<MessageModel> messages;
        private ChatModel chat;
        private MessageModel message;

        public ObservableCollection<MessageModel> Messages
        {
            get => messages;
            set
            {
                messages = value;
                OnPropertyChanged();
            }
        }

        public ChatModel Chat
        {
            get => chat;
            set
            {
                chat = value;
                OnPropertyChanged();
            }
        }

        public MessageModel Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }

        public ICommand SendMessageCommand { get; set; }

        public ChatVM()
        {
            Chat = new ChatModel();
            Messages = new ObservableCollection<MessageModel>();
            Message = new MessageModel();

            SendMessageCommand = new DelegateCommand(SendMessage);

            CurrentClient.SetChatVM(this);
        }

        private void SendMessage(object obj)
        {
            //Task.Factory.StartNew(() => CurrentClient.Client.SendMessage(Message.Text, new DateTime(), CurrentUser.User.Id, Chat.Id));
            CurrentClient.Client.SendMessage(Message.Text, new DateTime(), CurrentUser.User.Id, Chat.Id);
        }
    }
}
