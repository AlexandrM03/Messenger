using MessengerClient.Logic.Model;
using MessengerClient.ServiceMessenger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MessengerClient.Logic.ViewModel.MainVM
{
    public class MainVM : BaseVM
    {
        private Page mainContent;

        private MainNavigation navigation;
        private ObservableCollection<ChatModel> chats;

        public MainNavigation Navigation
        {
            get => navigation;
        }

        public ObservableCollection<ChatModel> Chats
        {
            get => chats;
            set
            {
                chats = value;
                OnPropertyChanged();
            }
        }

        public Page MainContent
        {
            get => mainContent; 
            set
            {
                mainContent = value;
                OnPropertyChanged();
            }
        }

        // Navigation commands
        public ICommand GoToAccount { get; private set; }
        public ICommand GoToSearch { get; private set; }
        public ICommand GoToChat { get; private set; }

        public MainVM()
        {
            navigation = new MainNavigation();

            MainContent = navigation.GetPage("account");

            GoToAccount = new DelegateCommand(AcccountOpen);
            GoToSearch = new DelegateCommand(SearchOpen);
            GoToChat = new DelegateCommand(ChatOpen);

            chats = new ObservableCollection<ChatModel>();

            CurrentClient.SetMainVM(this);
            foreach (Dictionary<string, string> chat in CurrentClient.Client.GetChats(CurrentUser.User.Id))
            {
                chats.Add(new ChatModel()
                {
                    Id = Int32.Parse(chat["id"]),
                    Name = chat["name"],
                    Admin = Int32.Parse(chat["admin"]),
                    Image = chat["path"],
                    LastMessage = chat["last_message"],
                });
            }
        }

        private void AcccountOpen(object obj)
        {
            MainContent = navigation.GetPage("account");
        }

        private void SearchOpen(object obj)
        {
            MainContent = navigation.GetPage("search");
        }

        private void ChatOpen(object obj)
        {
            if (!(obj is ChatModel chat))
                return;

            CurrentChat.SetNewChat(chat);
            CurrentClient.Callback.ChatVM.Chat = chat;

            List<Dictionary<string, string>> messagesDictionary = CurrentClient.Client.GetMessages(chat.Id).ToList();
            ObservableCollection<MessageModel> messages = new ObservableCollection<MessageModel>();
            foreach (Dictionary<string, string> message in messagesDictionary)
            {
                MessageModel messageModel = new MessageModel()
                {
                    Id = Int32.Parse(message["id"]),
                    Name = message["name"],
                    Surname = message["surname"],
                    Avatar = message["path"],
                    Date = message["date"]
                };

                if (message["type"] == "image")
                    messageModel.Path = message["content"];
                else
                    messageModel.Text = message["content"];

                messages.Add(messageModel);
            }
            CurrentClient.Callback.ChatVM.Messages = messages;
            MainContent = navigation.GetPage("chat");
        }
    }
}
