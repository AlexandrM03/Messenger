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
        private List<ServerUser> connectedUsers = new List<ServerUser>();
        
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

                if (user != null)
                {
                    connectedUsers.Add(new ServerUser
                    {
                        Id = user.Id,
                        OperationContext = OperationContext.Current
                    });
                    
                    Media media = uow.MediaRepository.GetAll().Where(m => m.Id == user.Media.Id).FirstOrDefault();
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

        public void Disconnect(int id)
        {
            connectedUsers.Remove(connectedUsers.Where(u => u.Id == id).FirstOrDefault());
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

        public void CreateChat(string name, string path, int admin, List<int> users)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                Media media = new Media() { Path = path };
                uow.MediaRepository.Add(media);

                List<User> allUsers = uow.UserRepository.GetAll();
                
                User adminUser = allUsers.Where(u => u.Id == admin).FirstOrDefault();
                Chat chat = new Chat() { Name = name, Admin = adminUser, Media = media };
                uow.ChatRepository.Add(chat);

                ChatUser adminOfChat = new ChatUser() { Chat = chat, User = adminUser };
                uow.ChatUserRepository.Add(adminOfChat);

                foreach (int userId in users)
                {
                    User user = allUsers.Where(u => u.Id == userId).FirstOrDefault();
                    ChatUser chatUser = new ChatUser() { Chat = chat, User = user };
                    uow.ChatUserRepository.Add(chatUser);

                    ServerUser connectedUser = connectedUsers.Where(u => u.Id == userId).FirstOrDefault();
                    if (connectedUser != null)
                    {
                        connectedUser.OperationContext.GetCallbackChannel<IMessengerCallback>().CreateChatCallback(chat.Id, chat.Name, adminUser.Id, media.Path);
                    }
                }

                ServerUser connectedUserCreator = connectedUsers.Where(u => u.Id == admin).FirstOrDefault();
                connectedUserCreator.OperationContext.GetCallbackChannel<IMessengerCallback>().CreateChatCallback(chat.Id, chat.Name, adminUser.Id, media.Path);

                uow.Save();
            }
        }

        public List<Dictionary<string, string>> GetChats(int userId)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
                List<Chat> chats = uow.ChatRepository.GetAll();
                List<Media> medias = uow.MediaRepository.GetAll();
                
                User currentUser = uow.UserRepository.GetAll().Where(u => u.Id == userId).FirstOrDefault();
                
                List<ChatUser> chatsOfUser = uow.ChatUserRepository.GetAll().Where(cu => cu.User == currentUser).ToList();

                foreach (ChatUser chatUser in chatsOfUser)
                {
                    Dictionary<string, string> chatDict = new Dictionary<string, string>();
                    Chat chat = chats.Where(c => c.Id == chatUser.Chat.Id).FirstOrDefault();
                    Media media = medias.Where(m => m.Id == chat.Media.Id).FirstOrDefault();

                    chatDict.Add("id", chat.Id.ToString());
                    chatDict.Add("name", chat.Name);
                    chatDict.Add("admin", chat.Admin.Id.ToString());
                    chatDict.Add("path", media.Path);

                    result.Add(chatDict);
                }
                return result;
            }
        }

        public List<Dictionary<string, string>> GetMessages(int chatId)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
                Chat chat = uow.ChatRepository.GetAll().Where(c => c.Id == chatId).FirstOrDefault();
                List<Message> messages = uow.MessageRepository.GetAll().Where(m => m.Chat == chat).ToList();
                List<User> users = uow.UserRepository.GetAll();
                List<Media> medias = uow.MediaRepository.GetAll();

                foreach (Message message in messages)
                {
                    Dictionary<string, string> messageDict = new Dictionary<string, string>();
                    User user = users.Where(u => u.Id == message.User.Id).FirstOrDefault();
                    Media media = medias.Where(m => m.Id == user.Media.Id).FirstOrDefault();

                    messageDict.Add("id", message.Id.ToString());
                    messageDict.Add("name", user.Name);
                    messageDict.Add("surname", user.Surname);
                    messageDict.Add("path", media.Path);
                    messageDict.Add("text", message.Text);
                    messageDict.Add("date", message.Date.ToString());

                    result.Add(messageDict);
                }
                return result;
            }
        }

        public void SendMessage(string text, DateTime dateTime, int senderId, int chatId)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                User sender = uow.UserRepository.GetAll().Where(u => u.Id == senderId).FirstOrDefault();
                Media media = uow.MediaRepository.GetAll().Where(m => m.Id == sender.Media.Id).FirstOrDefault();
                Chat chat = uow.ChatRepository.GetAll().Where(c => c.Id == chatId).FirstOrDefault();
                string sqlFormattedDate = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
                Message message = new Message() { Text = text, Date = dateTime, User = sender, Chat = chat };
                uow.MessageRepository.Add(message);

                List<int> retrievers = uow.ChatUserRepository.GetAll().Where(cu => cu.Chat == chat).Select(cu => cu.User.Id).ToList();
                foreach (int userId in retrievers)
                {
                    ServerUser connectedUser = connectedUsers.Where(u => u.Id == userId).FirstOrDefault();
                    if (connectedUser != null)
                    {
                        connectedUser.OperationContext.GetCallbackChannel<IMessengerCallback>()
                            .SendMessageCallback(message.Id, message.Text, message.Date, sender.Name, sender.Surname, media.Path, chatId);
                    }
                }

                uow.Save();
            }
        }
    }
}
