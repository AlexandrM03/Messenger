using MessengerClient.Logic.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerClient.Logic.ViewModel.AdminVM
{
    public class ReportVM : BaseVM
    {
        private ObservableCollection<ReportModel> reports;

        public ObservableCollection<ReportModel> Reports
        {
            get => reports;
            set
            {
                reports = value;
                OnPropertyChanged();
            }
        }

        public ReportVM()
        {
            Reports = new ObservableCollection<ReportModel>();
        }
    }
}
