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
        private readonly Page reports;

        public AdminNavigation()
        {
            online = new OnlineUsersPage();
            reports = new ReportsPage();

            pages.Add("online", online);
            pages.Add("reports", reports);
        }

        public Page GetPage(string name)
        {
            return pages[name];
        }
    }
}
