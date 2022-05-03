using MessengerClient.Logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerClient
{
    public class CurrentChat
    {
        public readonly static ChatModel Chat = new ChatModel();
        
        public static void SetNewChat(ChatModel chat)        
        {
            Chat.Id = chat.Id;
            Chat.Name = chat.Name;
            Chat.Admin = chat.Admin;
            Chat.Image = chat.Image;
            Chat.LastMessage = chat.LastMessage;
        }
    }
}
