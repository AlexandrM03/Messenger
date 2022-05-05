using MessengerClient.Presentation.View.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MessengerClient.Logic.ViewModel.AdminVM
{
    public class AdminNavigation
    {
        Dictionary<string, Page> pages = new Dictionary<string, Page>();

        private readonly Page online;

        public AdminNavigation()
        {
            online = new OnlineUsersPage();

            pages.Add("online", online);
        }

        public Page GetPage(string name)
        {
            return pages[name];
        }
    }
}
