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
        private List<ServerUser> connectedAdmins = new List<ServerUser>();
        
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
                    if (user.Role == "admin")
                        connectedAdmins.Add(new ServerUser
                        {
                            Id = user.Id,
                            OperationContext = OperationContext.Current
                        });
                    else
                    {
                        foreach (var userAdmin in connectedAdmins)
                        {
                            userAdmin.OperationContext.GetCallbackChannel<IMessengerCallback>().AdminUpdate(user.Id, user.Name, user.Surname, "connected");
                        }

                        connectedUsers.Add(new ServerUser
                        {
                            Id = user.Id,
                            OperationContext = OperationContext.Current
                        });
                    }
                    
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
            using (UnitOfWork uow = new UnitOfWork())
            {
                User user = uow.UserRepository.GetAll().Where(u => u.Id == id).FirstOrDefault();
                if (user.Role == "user")
                {
                    connectedUsers.Remove(connectedUsers.Where(u => u.Id == id).FirstOrDefault());

                    foreach (var userAdmin in connectedAdmins)
                    {
                        userAdmin.OperationContext.GetCallbackChannel<IMessengerCallback>().AdminUpdate(user.Id, user.Name, user.Surname, "disconnected");
                    }
                }
                else
                {
                    connectedAdmins.Remove(connectedAdmins.Where(u => u.Id == id).FirstOrDefault());
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
                uow.Save();
                
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
                    Message lastMessage = uow.MessageRepository.GetAll().Where(m => m.Chat == chat).OrderByDescending(m => m.Date).FirstOrDefault();
                    Media media = medias.Where(m => m.Id == chat.Media.Id).FirstOrDefault();

                    chatDict.Add("id", chat.Id.ToString());
                    chatDict.Add("name", chat.Name);
                    chatDict.Add("admin", chat.Admin.Id.ToString());
                    chatDict.Add("path", media.Path);
                    if (lastMessage != null)
                        if (lastMessage.Media != null)
                            chatDict.Add("last_message", "Image");
                        else
                            chatDict.Add("last_message", lastMessage.Text);
                    else
                        chatDict.Add("last_message", "");

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
                    messageDict.Add("date", message.Date.ToString());

                    if (message.Text == null)
                    {
                        Media image = medias.Where(m => m.Id == message.Media.Id).FirstOrDefault();
                        messageDict.Add("type", "image");
                        messageDict.Add("content", image.Path);
                    }
                    else
                    {
                        messageDict.Add("type", "text");
                        messageDict.Add("content", message.Text);
                    }

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
                Message message = new Message() { Text = text, Date = dateTime, Media = null, User = sender, Chat = chat };
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

        public void SendImage(string path, DateTime dateTime, int senderId, int chatId)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                User sender = uow.UserRepository.GetAll().Where(u => u.Id == senderId).FirstOrDefault();
                Media media = uow.MediaRepository.GetAll().Where(m => m.Id == sender.Media.Id).FirstOrDefault();
                Chat chat = uow.ChatRepository.GetAll().Where(c => c.Id == chatId).FirstOrDefault();
                Media image = new Media() { Path = path };
                uow.MediaRepository.Add(image);
                Message message = new Message() { Text = null, Date = dateTime, Media = image, User = sender, Chat = chat };
                uow.MessageRepository.Add(message);

                List<int> retrievers = uow.ChatUserRepository.GetAll().Where(cu => cu.Chat == chat).Select(cu => cu.User.Id).ToList();
                foreach (int userId in retrievers)
                {
                    ServerUser connectedUser = connectedUsers.Where(u => u.Id == userId).FirstOrDefault();
                    if (connectedUser != null)
                    {
                        connectedUser.OperationContext.GetCallbackChannel<IMessengerCallback>()
                            .SendImageCallback(message.Id, path, message.Date, sender.Name, sender.Surname, media.Path, chatId);
                    }
                }

                uow.Save();
            }
        }

        public void ChangeAvatar(int id, string path)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                User user = uow.UserRepository.GetAll().Where(u => u.Id == id).FirstOrDefault();
                Media media = uow.MediaRepository.GetAll().Where(m => m.Id == user.Media.Id).FirstOrDefault();
                media.Path = path;
                uow.MediaRepository.Update(media);
                uow.Save();
            }  
        }

        public void ReportMessage(int id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                Message message = uow.MessageRepository.GetAll().Where(m => m.Id == id).FirstOrDefault();
                User user = uow.UserRepository.GetAll().Where(u => u.Id == message.User.Id).FirstOrDefault();
                Report report = new Report() { Message = message };
                uow.ReportRepository.Add(report);

                Dictionary<string, string> content = new Dictionary<string, string>();
                if (message.Text == null)
                {
                    Media media = uow.MediaRepository.GetAll().Where(m => m.Id == message.Media.Id).FirstOrDefault();
                    content.Add("type", "image");
                    content.Add("content", media.Path);
                }
                else
                {
                    content.Add("type", "text");
                    content.Add("content", message.Text);
                }
                uow.Save();

                foreach (var userAdmin in connectedAdmins)
                {
                    userAdmin.OperationContext.GetCallbackChannel<IMessengerCallback>().ReportCallback(report.Id, user.Name, user.Surname, content);
                }

            }
        }

        public void AcceptReport(int id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                Report report = uow.ReportRepository.GetAll().Where(r => r.Id == id).FirstOrDefault();
                Message message = uow.MessageRepository.GetAll().Where(m => m.Id == report.Message.Id).FirstOrDefault();
                // Ne rabotaet
                message.Media = null;
                message.Text = "This message was hidden by admin";
                uow.MessageRepository.Update(message);
                uow.ReportRepository.Remove(report.Id);
                uow.Save();
            }
        }

        public List<Dictionary<string, string>> GetReports()
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                List<Report> reports = uow.ReportRepository.GetAll();
                List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
                foreach (Report report in reports)
                {
                    Dictionary<string, string> reportDict = new Dictionary<string, string>();
                    Message message = uow.MessageRepository.GetAll().Where(m => m.Id == report.Message.Id).FirstOrDefault();
                    User user = uow.UserRepository.GetAll().Where(u => u.Id == message.User.Id).FirstOrDefault();

                    reportDict.Add("id", report.Id.ToString());
                    reportDict.Add("name", user.Name);
                    reportDict.Add("surname", user.Surname);
                    if (message.Text == null)
                    {
                        Media media = uow.MediaRepository.GetAll().Where(m => m.Id == message.Media.Id).FirstOrDefault();
                        reportDict.Add("type", "image");
                        reportDict.Add("content", media.Path);
                    }
                    else
                    {
                        reportDict.Add("type", "text");
                        reportDict.Add("content", message.Text);
                    }

                    result.Add(reportDict);
                }

                return result;
            }
        }

        public void DeleteReport(int id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                Report report = uow.ReportRepository.GetAll().Where(r => r.Id == id).FirstOrDefault();
                uow.ReportRepository.Remove(report.Id);
                uow.Save();
            }
        }

        public List<Dictionary<string, string>> GetChatMembers(int id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                Chat chat = uow.ChatRepository.GetAll().Where(c => c.Id == id).FirstOrDefault();
                List<ChatUser> chatUser = uow.ChatUserRepository.GetAll().Where(cu => cu.Chat == chat).ToList();
                List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
                foreach (ChatUser cu in chatUser)
                {
                    Dictionary<string, string> chatUserDict = new Dictionary<string, string>();
                    User user = uow.UserRepository.GetAll().Where(u => u.Id == cu.User.Id).FirstOrDefault();
                    Media media = uow.MediaRepository.GetAll().Where(m => m.Id == user.Media.Id).FirstOrDefault();
                    chatUserDict.Add("id", user.Id.ToString());
                    chatUserDict.Add("name", user.Name);
                    chatUserDict.Add("surname", user.Surname);
                    chatUserDict.Add("avatar", media.Path);
                    result.Add(chatUserDict);
                }

                return result;
            }
        }

        public void DeleteUserFromChat(int chat_id, int user_id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                Chat chat = uow.ChatRepository.GetAll().Where(c => c.Id == chat_id).FirstOrDefault();
                User user = uow.UserRepository.GetAll().Where(u => u.Id == user_id).FirstOrDefault();
                ChatUser chatUser = uow.ChatUserRepository.GetAll().Where(cu => cu.Chat == chat && cu.User == user).FirstOrDefault();
                uow.ChatUserRepository.Remove(chatUser.Id);
                uow.Save();

                ServerUser removedUser = connectedUsers.Where(u => u.Id == user_id).FirstOrDefault();
                if (removedUser != null)
                {
                    removedUser.OperationContext.GetCallbackChannel<IMessengerCallback>().DeleteFromChatCallback(chat_id);
                }
            }
        }
    }
}
