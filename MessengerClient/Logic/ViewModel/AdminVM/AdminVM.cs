using MessengerClient.Logic.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MessengerClient.Logic.ViewModel.AdminVM
{
    public class AdminVM : BaseVM
    {
        private Page mainContent;
        private readonly AdminNavigation navigation;

        public Page MainContent
        {
            get => mainContent;
            set
            {
                mainContent = value;
                OnPropertyChanged();
            }
        }

        public ICommand GoToOnlineUsersCommand { get; set; }
        public ICommand GoToReportsCommand { get; set; }

        public AdminVM()
        {
            navigation = new AdminNavigation();
            MainContent = navigation.GetPage("online");

            GoToOnlineUsersCommand = new DelegateCommand(GoToOnlineUsers);
            GoToReportsCommand = new DelegateCommand(GoToReports);
        }

        private void GoToOnlineUsers(object obj)
        {
            MainContent = navigation.GetPage("online");
        }

        private void GoToReports(object obj)
        {
            List<Dictionary<string, string>> reports = CurrentClient.Client.GetReports().ToList();
            ObservableCollection<ReportModel> reportModels = new ObservableCollection<ReportModel>();
            foreach (var report in reports)
            {
                ReportModel reportModel = new ReportModel();
                reportModel.Id = Int32.Parse(report["id"]);
                MessageModel messageModel = new MessageModel()
                {
                    Name = report["name"],
                    Surname = report["surname"]
                };
                if (report["type"] == "image")
                    messageModel.Path = report["content"];
                else
                    messageModel.Text = report["content"];
                reportModel.Message = messageModel;

                reportModels.Add(reportModel);
            }
            CurrentClient.Callback.ReportVM.Reports = reportModels;
            MainContent = navigation.GetPage("reports");
        }
    }
}
