﻿using MessengerClient.Logic.Model;
using MessengerClient.ServiceMessenger;
using System;
using System.Collections.Generic;
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
        private List<ChatModel> chats;

        public List<ChatModel> Chats
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

        public MainVM()
        {
            navigation = new MainNavigation();

            MainContent = navigation.GetPage("account");

            GoToAccount = new DelegateCommand(AcccountOpen);
            GoToSearch = new DelegateCommand(SearchOpen);

            MessengerServiceClient client = new MessengerServiceClient();
            chats = new List<ChatModel>();

            foreach (Dictionary<string, string> chat in client.GetChats(CurrentUser.User.Id))
            {
                chats.Add(new ChatModel()
                {
                    Id = Int32.Parse(chat["id"]),
                    Name = chat["name"],
                    Admin = Int32.Parse(chat["admin"]),
                    Image = chat["path"]
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
    }
}