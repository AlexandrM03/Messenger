using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WCFService.DTO;
using WCFService.Unit;

namespace WCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MessengerService" in both code and config file together.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class MessengerService : IMessengerService
    {
        public string Registration(string login, string password, string name, string surname, string path)
        {
            // validate TODO
            try
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    string hashPassword = HashManager.GetHash(password);
                    UserAuth userAuth = new UserAuth() { Login = login, Password = password };
                    Media media = new Media() { Path = path };
                    uow.UserAuthRepository.Add(userAuth);
                    uow.MediaRepository.Add(media);
                    User user = new User() { Name = name, Surname = surname, Role = "user", IdMedia = media.Id, IdUserAuth = userAuth.Id };
                    uow.UserRepository.Add(user);
                    //User user = new User() { Id = 1, IdMedia = 1, IdUserAuth = 1, Name = name, Surname = surname, Role = "user" };
                    uow.UserRepository.Add(user);
                    uow.Save();
                    return "Nice";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
