using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFService.DataBase;
using WCFService.DTO;
using WCFService.Repository;

namespace WCFService.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        private Context context;
        private Repository<User> userRepository;
        private Repository<UserAuth> userAuthRepository;
        private Repository<Chat> chatRepository;
        private Repository<ChatUser> chatUserRepository;
        private Repository<Message> messageRepository;
        private Repository<Media> mediaRepository;
        private Repository<Report> reportRepository;

        public Repository<User> UserRepository
        {
            get
            {
                if (userRepository == null)
                    userRepository = new Repository<User>(context);
                return userRepository;
            }
        }

        public Repository<UserAuth> UserAuthRepository
        {
            get
            {
                if (userAuthRepository == null)
                    userAuthRepository = new Repository<UserAuth>(context);
                return userAuthRepository;
            }
        }

        public Repository<Chat> ChatRepository
        {
            get
            {
                if (chatRepository == null)
                    chatRepository = new Repository<Chat>(context);
                return chatRepository;
            }
        }

        public Repository<ChatUser> ChatUserRepository
        {
            get
            {
                if (chatUserRepository == null)
                    chatUserRepository = new Repository<ChatUser>(context);
                return chatUserRepository;
            }
        }

        public Repository<Message> MessageRepository
        {
            get
            {
                if (messageRepository == null)
                    messageRepository = new Repository<Message>(context);
                return messageRepository;
            }
        }

        public Repository<Media> MediaRepository
        {
            get
            {
                if (mediaRepository == null)
                    mediaRepository = new Repository<Media>(context);
                return mediaRepository;
            }
        }

        public Repository<Report> ReportRepository
        {
            get
            {
                if (reportRepository == null)
                    reportRepository = new Repository<Report>(context);
                return reportRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
