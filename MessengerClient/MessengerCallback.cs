using MessengerClient.Logic.Model;
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
    }
}
