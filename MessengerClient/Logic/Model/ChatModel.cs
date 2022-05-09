using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerClient.Logic.Model
{
    public class ChatModel : INotifyPropertyChanged
    {
        private bool isSelected = false;
        
        public int Id { get; set; }
        public string Name { get; set; }
        public int Admin { get; set; }
        public string Image { get; set; }
        public string LastMessage { get; set; }
        public bool IsSelected 
        {
            get => isSelected;
            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
