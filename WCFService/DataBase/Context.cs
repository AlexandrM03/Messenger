using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFService.DTO;

namespace WCFService.DataBase
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserAuth> UserAuths { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatUser> ChatUsers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Media> Medias { get; set; }
        public DbSet<Report> Reports { get; set; }

        public Context() : base("DefaultConnection")
        { }
    }
}
