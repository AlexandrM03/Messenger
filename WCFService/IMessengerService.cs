using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMessengerService" in both code and config file together.
    [ServiceContract(CallbackContract = typeof(IMessengerCallback))]
    public interface IMessengerService
    {
        [OperationContract]
        string Registration(string login, string password, string name, string surname, string path);
        [OperationContract]
        Dictionary<string, string> Login(string login, string password);
        [OperationContract]
        void Disconnect(int id);
        [OperationContract]
        List<Dictionary<string, string>> GetUsers();
        [OperationContract(IsOneWay = true)]
        void CreateChat(string name, string path, int admin, List<int> users);
        [OperationContract]
        List<Dictionary<string, string>> GetChats(int userId);
    }

    public interface IMessengerCallback
    {
        [OperationContract(IsOneWay = true)]
        void CreateChatCallback(int id, string name, int admin, string path);
    }
}
