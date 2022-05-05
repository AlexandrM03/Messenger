using MessengerClient.Logic.Model;
using MessengerClient.Logic.ViewModel.AdminVM;
using MessengerClient.Logic.ViewModel.MainVM;
using MessengerClient.ServiceMessenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerClient
{
    public class MessengerCallback : IMessengerServiceCallback
    {
        public MainVM MainVM { get; set; }
        public ChatVM ChatVM { get; set; }
        public OnlineUsersVM OnlineUsersVM { get; set; }

        public void AdminUpdate(int id, string name, string surname, string message)
        {
            OnlineUsersVM.UserInfo.Add(new AdminInfoModel()
            {
                Id = id,
                Name = name,
                Surname = surname,
                Message = message
            });
        }

        public void CreateChatCallback(int id, string name, int admin, string path)
        {
            MainVM.Chats.Add(new ChatModel()
            {
                Id = id,
                Name = name,
                Admin = admin,
                Image = path
            });
        }

        public void SendMessageCallback(int id, string text, DateTime date, string name, string surname, string avatar, int chatId)
        {
            if (CurrentChat.Chat.Id == chatId)
            {
                ChatVM.Messages.Add(new MessageModel()
                {
                    Id = id,
                    Text = text,
                    Date = date.ToString(),
                    Name = name,
                    Surname = surname,
                    Avatar = avatar
                });
            }
        }
    }
}
