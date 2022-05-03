using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerClient.Logic.Model
{
    public class MessageModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Date { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Avatar { get; set; }
    }
}
