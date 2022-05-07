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
        private readonly Page chatInfo;

        public MainNavigation()
        {
            account = new AccountPage();
            search = new SearchPage();
            chatInfo = new ChatInfoPage();

            pages.Add("account", account);
            pages.Add("search", search);
            pages.Add("chat", new ChatPage());
            pages.Add("chatInfo", chatInfo);
        }

        public Page GetPage(string name)
        {
            return pages[name];
        }
    }
}
