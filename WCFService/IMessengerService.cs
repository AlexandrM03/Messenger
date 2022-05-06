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
        [OperationContract]
        List<Dictionary<string, string>> GetMessages(int chatId);
        [OperationContract(IsOneWay = true)]
        void SendMessage(string text, DateTime dateTime, int senderId, int chatId);
        [OperationContract(IsOneWay = true)]
        void SendImage(string path, DateTime dateTime, int senderId, int chatId);
        [OperationContract]
        void ChangeAvatar(int id, string path);
        [OperationContract(IsOneWay = true)]
        void ReportMessage(int id);
        [OperationContract]
        void AcceptReport(int id);
        [OperationContract]
        List<Dictionary<string, string>> GetReports();
    }

    public interface IMessengerCallback
    {
        [OperationContract(IsOneWay = true)]
        void CreateChatCallback(int id, string name, int admin, string path);
        [OperationContract(IsOneWay = true)]
        void SendMessageCallback(int id, string text, DateTime date, string name, string surname, string avatar, int chatId);
        [OperationContract(IsOneWay = true)]
        void AdminUpdate(int id, string name, string surname, string message);
        [OperationContract(IsOneWay = true)]
        void SendImageCallback(int id, string path, DateTime date, string name, string surname, string avatar, int chatId);
        [OperationContract(IsOneWay = true)]
        void ReportCallback(int id, string name, string surname, Dictionary<string, string> message);
    }
}
