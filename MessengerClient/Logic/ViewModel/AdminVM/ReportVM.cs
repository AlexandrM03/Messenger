using MessengerClient.Logic.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        public ICommand AcceptReportCommand { get; set; }
        public ICommand DeclineReportCommand { get; set; }

        public ReportVM()
        {
            Reports = new ObservableCollection<ReportModel>();

            AcceptReportCommand = new DelegateCommand(AcceptReport);
            DeclineReportCommand = new DelegateCommand(DeclineReport);
            
            CurrentClient.SetReportVM(this);
        }

        private void AcceptReport(object obj)
        {
            if (!(obj is ReportModel report))
                return;

            CurrentClient.Client.AcceptReport(report.Id);
            Reports.Remove(report);
        }

        private void DeclineReport(object obj)
        {
            if (!(obj is ReportModel report))
                return;

            CurrentClient.Client.DeleteReport(report.Id);
            Reports.Remove(report);
        }
    }
}
