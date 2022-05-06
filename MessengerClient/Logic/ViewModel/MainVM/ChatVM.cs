using MessengerClient.Logic.Model;
using Microsoft.Win32;
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
        public ICommand SendImageCommand { get; set; }

        public ChatVM()
        {
            Chat = new ChatModel();
            Messages = new ObservableCollection<MessageModel>();
            Message = new MessageModel();

            SendMessageCommand = new DelegateCommand(SendMessage);
            SendImageCommand = new DelegateCommand(SendImage);

            CurrentClient.SetChatVM(this);
        }

        private void SendMessage(object obj)
        {
            string message = Message.Text;
            Task.Factory.StartNew(() => CurrentClient.Client.SendMessage(message, DateTime.Now, CurrentUser.User.Id, Chat.Id));
            Message.Text = null;
            OnPropertyChanged("Message");
        }

        private void SendImage(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            if (openFileDialog.ShowDialog() == true)
            {
                Message.Path = openFileDialog.FileName;
            }
            Task.Factory.StartNew(() => CurrentClient.Client.SendImage(Message.Path, DateTime.Now, CurrentUser.User.Id, Chat.Id));
        }
    }
}
