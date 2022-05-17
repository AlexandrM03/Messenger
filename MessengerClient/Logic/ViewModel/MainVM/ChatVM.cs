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
        public ICommand SendReportCommand { get; set; }
        public ICommand GoToChatInfoCommand { get; set; }

        public ChatVM()
        {
            Chat = new ChatModel();
            Messages = new ObservableCollection<MessageModel>();
            Message = new MessageModel();

            SendMessageCommand = new DelegateCommand(SendMessage);
            SendImageCommand = new DelegateCommand(SendImage);
            SendReportCommand = new DelegateCommand(SendReport);
            GoToChatInfoCommand = new DelegateCommand(GoToChatInfo);

            CurrentClient.SetChatVM(this);
        }

        private void SendMessage(object obj)
        {
            string message = Message.Text;
            if (message != null)
            {
                Task.Factory.StartNew(() => CurrentClient.Client.SendMessage(message, DateTime.Now, CurrentUser.User.Id, Chat.Id));
                Message.Text = null;
                OnPropertyChanged("Message");
            }
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

        private void SendReport(object obj)
        {
            if (!(obj is MessageModel message))
                return;

            Task.Factory.StartNew(() => CurrentClient.Client.ReportMessage(message.Id));
        }

        private void GoToChatInfo(object obj)
        {
            List<Dictionary<string, string>> dictionary = CurrentClient.Client.GetChatMembers(Chat.Id).ToList();
            ObservableCollection<UserModel> userModels = new ObservableCollection<UserModel>();
            foreach (Dictionary<string, string> user in dictionary)
            {
                UserModel userModel = new UserModel()
                {
                    Id = Int32.Parse(user["id"]),
                    Name = user["name"],
                    Surname = user["surname"],
                    Avatar = user["avatar"]
                };

                if (userModel.Id == CurrentChat.Chat.Admin)
                    userModels.Insert(0, userModel);
                else
                    userModels.Add(userModel);
            }

            CurrentClient.Callback.ChatInfoVM.Users = userModels;

            List<Dictionary<string, string>> secondDictionary = CurrentClient.Client.GetUsers().ToList();
            ObservableCollection<UserModel> secondUserModels = new ObservableCollection<UserModel>();
            foreach (Dictionary<string, string> user in secondDictionary)
            {
                UserModel userModel = new UserModel()
                {
                    Id = Int32.Parse(user["id"]),
                    Name = user["name"],
                    Surname = user["surname"],
                    Avatar = user["path"]
                };

                secondUserModels.Add(userModel);
            }

            secondUserModels = new ObservableCollection<UserModel>(secondUserModels.Where(x => !userModels.Any(y => y.Id == x.Id)));
            CurrentClient.Callback.ChatInfoVM.OtherUsers = secondUserModels;
            CurrentClient.Callback.MainVM.MainContent = CurrentClient.Callback.MainVM.Navigation.GetPage("chatInfo");
        }
    }
}
