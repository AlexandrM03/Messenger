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
    public static class CurrentClient
    {        
        public readonly static MessengerCallback Callback  = new MessengerCallback();
        public readonly static MessengerServiceClient Client = new MessengerServiceClient(new System.ServiceModel.InstanceContext(Callback));

        public static void SetMainVM(MainVM mainVM)
        {
            Callback.MainVM = mainVM;
        }

        public static void SetChatVM(ChatVM chatVM)
        {
            Callback.ChatVM = chatVM;
        }
    }
}