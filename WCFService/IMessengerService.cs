using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMessengerService" in both code and config file together.
    [ServiceContract]
    public interface IMessengerService
    {
        [OperationContract]
        string Registration(string login, string password, string name, string surname, string path);
        [OperationContract]
        Dictionary<string, string> Login(string login, string password);
        [OperationContract]
        List<Dictionary<string, string>> GetUsers();
    }
}
