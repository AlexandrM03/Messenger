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
    public class MessengerService : IMessengerService
    {
        public string Registrarion(string login, string password, string name, string surname, string path)
        {
            // validate TODO
            using (UnitOfWork uow = new UnitOfWork())
            {
                string hashPassword = HashManager.GetHash(password);
                UserAuth userAuth = new UserAuth() { Login = login, Password = hashPassword };
                Media media = new Media() { Path = path };
                uow.UserAuthRepository.Add(userAuth);
                uow.MediaRepository.Add(media);
                User user = new User() { Name = name, Surname = surname, Role = "user", IdMedia = media.Id, IdUserAuth = userAuth.Id };
                uow.UserRepository.Add(user);
                return "Nice";
            }

            return "Nonono";
        }
    }
}
