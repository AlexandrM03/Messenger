using MessengerClient.ServiceMessenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerClient.Logic.Model
{
    public class ClientModel
    {
        public int Id { get; set; }
        public MessengerServiceClient Client { get; set; }
    }
}
