using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerClient.Logic.Model
{
    public class ChatModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Admin { get; set; }
        public string Image { get; set; }
        public string LastMessage { get; set; }
        
    }
}
