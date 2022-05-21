using MessengerClient.Logic.Model;
using MessengerClient.Logic.ViewModel.AdminVM;
using MessengerClient.Logic.ViewModel.MainVM;
using MessengerClient.Presentation.View.Main;
using MessengerClient.ServiceMessenger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ReportVM ReportVM { get; set; }
        public ChatInfoVM ChatInfoVM { get; set; }

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
            MainVM.Chats.Insert(0, new ChatModel()
            {
                Id = id,
                Name = name,
                Admin = admin,
                Image = path
            });
        }

        public void DeleteFromChatCallback(int id)
        {
            MainVM.Chats.Remove(MainVM.Chats.First(x => x.Id == id));
            if ((MainVM.MainContent is ChatPage) && CurrentChat.Chat.Id == id)
            {
                MainVM.MainContent = MainVM.Navigation.GetPage("account");
            }
        }

        public void AddToChatCallback(int id, string name, int admin, string path, string lastMessage)
        {
            MainVM.Chats.Insert(0, new ChatModel()
            {
                Id = id,
                Name = name,
                Admin = admin,
                Image = path,
                LastMessage = lastMessage
            });
        }

        public void ReportCallback(int id, string name, string surname, Dictionary<string, string> message)
        {
            ReportModel report = new ReportModel()
            {
                Id = id,
                Message = new MessageModel()
                {
                    Name = name,
                    Surname = surname
                }
            };
            if (message["type"] == "image")
                report.Message.Path = message["content"];
            else
                report.Message.Text = message["content"];

            ReportVM.Reports.Add(report);
        }

        public void SendImageCallback(int id, string path, DateTime date, string name, string surname, string avatar, int chatId, int senderId)
        {
            if (CurrentChat.Chat.Id == chatId)
            {
                ChatVM.Messages.Add(new MessageModel()
                {
                    Id = id,
                    Path = path,
                    Date = date.ToString(),
                    Name = name,
                    Surname = surname,
                    Avatar = avatar,
                    IsMine = senderId == CurrentUser.User.Id
                });
            }

            ChatModel chatModel = MainVM.Chats.First(c => c.Id == chatId);
            MainVM.Chats.Remove(chatModel);
            chatModel.LastMessage = "Image";
            MainVM.Chats.Insert(0, chatModel);
        }

        public void SendMessageCallback(int id, string text, DateTime date, string name, string surname, string avatar, int chatId, int senderId)
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
                    Avatar = avatar,
                    IsMine = senderId == CurrentUser.User.Id
                });
            }

            ChatModel chatModel = MainVM.Chats.First(c => c.Id == chatId);
            MainVM.Chats.Remove(chatModel);
            chatModel.LastMessage = text;
            MainVM.Chats.Insert(0, chatModel);
        }
    }        
}
