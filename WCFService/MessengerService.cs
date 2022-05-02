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
                    UserAuth userAuth = new UserAuth() { Login = login, Password = hashPassword };
                    Media media = new Media() { Path = path };
                    uow.UserAuthRepository.Add(userAuth);
                    uow.MediaRepository.Add(media);
                    User user = new User() { Name = name, Surname = surname, Role = "user", Media = media, UserAuth = userAuth };
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

        public Dictionary<string, string> Login(string login, string password)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                string hashPassword = HashManager.GetHash(password);
                int userAuthId = uow.UserAuthRepository.GetAll().Where(u => u.Login == login && u.Password == hashPassword).FirstOrDefault().Id;
                User user = uow.UserRepository.GetAll().Where(u => u.UserAuth.Id == userAuthId).FirstOrDefault();
                Media media = uow.MediaRepository.GetAll().Where(m => m.Id == user.Media.Id).FirstOrDefault();

                if (user != null)
                {
                    Dictionary<string, string> result = new Dictionary<string, string>();
                    result.Add("id", user.Id.ToString());
                    result.Add("name", user.Name);
                    result.Add("surname", user.Surname);
                    result.Add("role", user.Role);
                    result.Add("path", media.Path);

                    return result;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<Dictionary<string, string>> GetUsers()
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
                List<User> users = uow.UserRepository.GetAll();
                
                foreach (User user in users)
                {
                    Dictionary<string, string> userDict = new Dictionary<string, string>();
                    Media media = uow.MediaRepository.GetAll().Where(m => m.Id == user.Media.Id).FirstOrDefault();
                    
                    userDict.Add("id", user.Id.ToString());
                    userDict.Add("name", user.Name);
                    userDict.Add("surname", user.Surname);
                    userDict.Add("role", user.Role);
                    userDict.Add("path", media.Path);

                    result.Add(userDict);
                }
                return result;
            }
        }

        public void CreateChat(string name, string path, List<int> users)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                Media media = new Media() { Path = path };
                Chat chat = new Chat() { Name = name, Media = media };

                uow.MediaRepository.Add(media);
                uow.ChatRepository.Add(chat);

                foreach (int userId in users)
                {
                    User user = uow.UserRepository.GetAll().Where(u => u.Id == userId).FirstOrDefault();
                    ChatUser chatUser = new ChatUser() { Chat = chat, User = user };
                    uow.ChatUserRepository.Add(chatUser);
                }

                uow.Save();
            }
        }
    }
}
