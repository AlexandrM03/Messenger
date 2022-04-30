using MessengerClient.Logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerClient
{
    public class CurrentUser
    {
        public readonly static UserModel User = new UserModel();

        public static void SetNewUser(UserModel user)
        {
            User.Id = user.Id;
            User.Name = user.Name;
            User.Surname = user.Surname;
            User.Role = user.Role;
            User.Avatar = user.Avatar;
        }
    }
}
