using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WCFService.DTO;
using WCFService.Unit;

namespace MessengerTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Register()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                Media media = new Media()
                {
                    Path = "Path"
                };
                unitOfWork.MediaRepository.Add(media);
                unitOfWork.Save();

                UserAuth userAuth = new UserAuth()
                {
                    Login = "Login",
                    Password = "Password"
                };
                unitOfWork.UserAuthRepository.Add(userAuth);
                unitOfWork.Save();

                User user = new User()
                {
                    Name = "Name",
                    Surname = "Surname",
                    Role = "user",
                    Media = media,
                    UserAuth = userAuth
                };

                unitOfWork.UserRepository.Add(user);
                unitOfWork.Save();

                Assert.AreNotEqual(user.Id, 0);
            }
        }

        [TestMethod]
        public void Can_Login()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                Media media = new Media()
                {
                    Path = "Path"
                };
                unitOfWork.MediaRepository.Add(media);
                unitOfWork.Save();

                UserAuth userAuth = new UserAuth()
                {
                    Login = "Login",
                    Password = "Password"
                };
                unitOfWork.UserAuthRepository.Add(userAuth);
                unitOfWork.Save();

                User user = new User()
                {
                    Name = "Name",
                    Surname = "Surname",
                    Role = "user",
                    Media = media,
                    UserAuth = userAuth
                };

                unitOfWork.UserRepository.Add(user);
                unitOfWork.Save();

                UserAuth loginedUserAuth = unitOfWork.UserAuthRepository.GetAll().Where(u => u.Login == "Login" && u.Password == "Password").FirstOrDefault();
                User loginedUser = unitOfWork.UserRepository.GetAll().Where(u => u.UserAuth == loginedUserAuth).FirstOrDefault();
                Assert.IsNotNull(loginedUser);
            }
        }

        [TestMethod]
        public void Can_Update_Avatar()
        {
            string path = "Path";
            string path_updated = "Path_updated";

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                Media media = new Media()
                {
                    Path = path
                };
                unitOfWork.MediaRepository.Add(media);
                unitOfWork.Save();

                UserAuth userAuth = new UserAuth()
                {
                    Login = "Login",
                    Password = "Password"
                };
                unitOfWork.UserAuthRepository.Add(userAuth);
                unitOfWork.Save();

                User user = new User()
                {
                    Name = "Name",
                    Surname = "Surname",
                    Role = "user",
                    Media = media,
                    UserAuth = userAuth
                };

                unitOfWork.UserRepository.Add(user);
                unitOfWork.Save();

                user.Media.Path = path_updated;
                unitOfWork.UserRepository.Update(user);
                unitOfWork.Save();

                Assert.AreEqual(user.Media.Path, path_updated);
            }
        }

        [TestMethod]
        public void Can_Create_Chat()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                Media media = new Media()
                {
                    Path = "Path"
                };
                unitOfWork.MediaRepository.Add(media);
                unitOfWork.Save();

                UserAuth userAuth = new UserAuth()
                {
                    Login = "Login",
                    Password = "Password"
                };
                unitOfWork.UserAuthRepository.Add(userAuth);
                unitOfWork.Save();

                User user = new User()
                {
                    Name = "Name",
                    Surname = "Surname",
                    Role = "user",
                    Media = media,
                    UserAuth = userAuth
                };

                unitOfWork.UserRepository.Add(user);
                unitOfWork.Save();

                Chat chat = new Chat()
                {
                    Name = "Name",
                    Admin = user,
                    Media = media
                };

                unitOfWork.ChatRepository.Add(chat);
                unitOfWork.Save();

                Assert.AreNotEqual(chat.Id, 0);
            }
        }

        [TestMethod]
        public void Can_Accept_Report()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                Media media = new Media()
                {
                    Path = "Path"
                };
                unitOfWork.MediaRepository.Add(media);
                unitOfWork.Save();

                UserAuth userAuth = new UserAuth()
                {
                    Login = "Login",
                    Password = "Password"
                };
                unitOfWork.UserAuthRepository.Add(userAuth);
                unitOfWork.Save();

                User user = new User()
                {
                    Name = "Name",
                    Surname = "Surname",
                    Role = "user",
                    Media = media,
                    UserAuth = userAuth
                };

                unitOfWork.UserRepository.Add(user);
                unitOfWork.Save();

                Chat chat = new Chat()
                {
                    Name = "Name",
                    Admin = user,
                    Media = media
                };

                unitOfWork.ChatRepository.Add(chat);
                unitOfWork.Save();

                Message message = new Message()
                {
                    Text = "Text",
                    User = user,
                    Chat = chat,
                    Media = media,
                    Date = DateTime.Now
                };

                unitOfWork.MessageRepository.Add(message);
                unitOfWork.Save();

                Report report = new Report()
                {
                    Message = message
                };

                unitOfWork.ReportRepository.Add(report);
                unitOfWork.Save();

                Message reported = unitOfWork.MessageRepository.GetAll().Where(m => m.Id == report.Message.Id).FirstOrDefault();
                reported.Text = "This message was hidden by admin";
                unitOfWork.MessageRepository.Update(reported);
                unitOfWork.Save();
                Message updated = unitOfWork.MessageRepository.GetAll().Where(m => m.Id == reported.Id).FirstOrDefault();

                Assert.AreEqual(updated.Text, "This message was hidden by admin");
            }
        }
    }
}
