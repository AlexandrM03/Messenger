using MessengerClient.Presentation.View.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MessengerClient.Logic.ViewModel.MainVM
{
    public class MainNavigation
    {
        Dictionary<string, Page> pages = new Dictionary<string, Page>();

        private readonly Page account;
        private readonly Page search;

        public MainNavigation()
        {
            account = new AccountPage();
            
            pages.Add("account", account);
            pages.Add("search", new SearchPage());
        }

        public Page GetPage(string name)
        {
            return pages[name];
        }
    }
}
